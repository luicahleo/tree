using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TreeCore.Clases;
using TreeCore.Data;
using TreeCore.ModExportarImportar;
using TreeCore.Page;

namespace TreeCore.PaginasComunes
{
    public partial class Formulas : TreeCore.Page.BasePageExtNet
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        BaseUserControl currentUC;
        const string FORMULA = "FORMULA";
        const string INVENTARIO_ELEMENTO_ID = "InventarioElementoID";
        public const string DINAMICO = "DINAMICO";
        public const string ESTATICO = "ESTATICO";

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                log4net.Config.XmlConfigurator.Configure();

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                //             //#region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, gridElementos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                //             //#endregion

                //             #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                //             #endregion


                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }
                storePrincipal.Reload();
                

                
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = "ProductCatalogServiciosContenedor.aspx";
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { }},
            { "Post", new List<ComponentBase> { btnAnadir }},
            { "Put", new List<ComponentBase> { }},
            { "Delete", new List<ComponentBase> { btnEliminar }}
            };
        }

        #region STORES
        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {

                #region Controllers
                TablasModeloDatosController cTablasModeloDatos = new TablasModeloDatosController();
                CoreObjetosNegocioTiposController cObjetosNegociosTipos = new CoreObjetosNegocioTiposController();
                #endregion

                try
                {
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];
                    long cliID = long.Parse(hdCliID.Value.ToString());
                    string seleccionado = hdArbol.Value.ToString();
                    List<Elementos> lista = null;


                    if (hdObjetoNegocioTipoID.Value.ToString() != "")
                    {
                        long tablaID = long.Parse(hdObjetoNegocioTipoID.Value.ToString());
                        CoreObjetosNegocioTipos tabla = cObjetosNegociosTipos.GetItem(tablaID);
                        btnAnadir.Disabled = false;
                        colIcono.Hidden = false;
                        lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, cliID);
                    }
                    else
                    {
                        colIcono.Hidden = true;
                        btnAnadir.Disabled = true;
                        lista = new List<Elementos>();

                        List<CoreObjetosNegocioTipos> list = listaTablasModeloDatos();

                        foreach (CoreObjetosNegocioTipos item in list)
                        {

                            lista.Add(new Elementos(item.CoreObjetoNegocioTipoID, item.ClaveRecurso, item.CoreObjetoNegocioTipoID, item.ObjetoTipoID, item.TablaModeloDatoID));


                        }
                    }

                    if (lista != null)
                    {
                        storePrincipal.DataSource = lista;

                        PageProxy temp = (PageProxy)storePrincipal.Proxy[0];
                        temp.Total = iCount;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Elementos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.CoreFormulas> listaDatos;
            List<Elementos> lista = new List<Elementos>();

            #region Controllers
            TablasModeloDatosController cTablasModeloDatos = new TablasModeloDatosController();
            CoreObjetosNegocioTiposController cObjetosNegociosTipos = new CoreObjetosNegocioTiposController();
            CoreFormulasController cFormulas = new CoreFormulasController();
            #endregion
            try
            {

                if (lClienteID.HasValue)
                {
                    long tablaID = long.Parse(hdObjetoNegocioTipoID.Value.ToString());
                    CoreObjetosNegocioTipos tabla = cObjetosNegociosTipos.GetItem(tablaID);

                    listaDatos = cFormulas.GetItemsWithExtNetFilterList<Data.CoreFormulas>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "CoreObjetoNegocioTipoID==" + tablaID);
                    foreach (CoreFormulas item in listaDatos)
                    {
                        lista.Add(new Elementos(item.CoreFormulaID, item.Nombre, "../../ima/ico-functionalityDataGrid.svg", item.CoreObjetoNegocioTipoID.Value, FORMULA));
                    }

                }
                else
                {
                    lista = null;
                }
            }
            catch (Exception ex)
            {
                lista = null;
                log.Error(ex.Message);
            }

            return lista;
        }

        #region ListaInventario
        private class Elementos
        {
            public long CoreObjetoNegocioTipoID { get; set; }
            public long? TablaModeloDatoID { get; set; }
            public long? ObjetoTipoID { get; set; }

            public long Id { get; set; }
            public string Nombre { get; set; }
            public string Tipo { get; set; }
            public string Icono { get; set; }



            public Elementos(long id, string nombre, string icono, long CoreObjetoNegocioTipoID, string tipo)
            {
                this.Id = id;
                this.Nombre = nombre;
                this.Icono = icono;
                this.CoreObjetoNegocioTipoID = CoreObjetoNegocioTipoID;
                this.Tipo = tipo;
            }
            public Elementos(long id, string nombre, long CoreObjetoNegocioTipoID, long? objetoTipoID, long tablaModelosDatosId)
            {
                this.Id = id;
                this.Nombre = nombre;
                this.CoreObjetoNegocioTipoID = CoreObjetoNegocioTipoID;
                this.ObjetoTipoID = objetoTipoID;
                this.TablaModeloDatoID = tablaModelosDatosId;
            }

        }

        
        #endregion

        #region TablasModelosDatos
        protected List<CoreObjetosNegocioTipos> listaTablasModeloDatos()
        {
            CoreObjetosNegocioTiposController cCoreObjetosNegocioTipos = new CoreObjetosNegocioTiposController();
            List<CoreObjetosNegocioTipos> lista = cCoreObjetosNegocioTipos.GetActivos(ClienteID.Value);
            try
            {


                if (lista != null)
                {
                    lista.ForEach(tmp =>
                    {
                        if (tmp.ClaveRecurso == "" || tmp.ClaveRecurso == null)
                        {
                            tmp.ClaveRecurso = tmp.Codigo;
                        }
                        else
                        {
                            tmp.ClaveRecurso = GetGlobalResource(tmp.ClaveRecurso);
                        }

                    });


                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }
        #endregion

        #region storeSelectCamposVinculados
        protected void storeSelectCamposVinculados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {

                #region Controllers
                CoreFormulasController cCoreFormulas = new CoreFormulasController();
                CoreInventarioCategoriasAtributosCategoriasController cCoreInventarioCategoriasAtributosCategorias = new CoreInventarioCategoriasAtributosCategoriasController();
                InventarioCategoriasController cInventarioCategorias = new InventarioCategoriasController();
                TiposDatosController cTiposDatos = new TiposDatosController();
                ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();
                TablasModeloDatosController cTablasModeloDatos = new TablasModeloDatosController();
                #endregion

                try
                {
                    List<AtributosDinamicos> lista = new List<AtributosDinamicos>();
                    List<ColumnasModeloDatos> lColumnasModeloDatos;
                    List<TablasModeloDatos> lTablasModeloDatos;
                    string sRuta = hdCampoVinculadoRuta.Value.ToString();
                    PathFormulas ruta = (PathFormulas)JSON.Deserialize(sRuta, typeof(PathFormulas));
                    long tablaID = 0;
                    long idTmp = 0;

                    CoreFormulas formula = cCoreFormulas.GetItem(long.Parse(hdFormulaID.Value.ToString()));
                    List<TiposDatos> tiposDatos = cTiposDatos.GetItemList();
                    if (ruta == null || ruta.path == null || (ruta.path.Count > 0 && ruta.path.Last().tipo == DINAMICO))
                    {
                        tablaID = (long)formula.CoreObjetosNegocioTipos.TablaModeloDatoID;
                    }
                    else
                    {
                        tablaID = ruta.path.Last().id;
                    }


                    #region Campos Estaticos

                    if (hdCampoVinculadoTipo.Value.ToString() == "" || hdCampoVinculadoTipo.Value.ToString() == ESTATICO)
                    {
                        #region Tablas
                        lTablasModeloDatos = cTablasModeloDatos.GetTablasAsociadas(tablaID);
                        lTablasModeloDatos.ForEach(c =>
                        {
                            lista.Add(new AtributosDinamicos(c.TablaModeloDatosID, GetGlobalResource(c.ClaveRecurso), idTmp++, false, "TablaModeloDato", true));
                        });
                        #endregion
                        #region Columnas de tabla
                        lColumnasModeloDatos = cColumnasModeloDatos.GetColumnasNoFkByTabla(tablaID);
                        lColumnasModeloDatos.ForEach(c =>
                        {
                            TiposDatos tipoDato = tiposDatos.Find(td => td.TipoDatoID == c.TipoDatoID.Value);
                            if (tipoDato.Codigo == "NUMERICO")
                            {
                                lista.Add(new AtributosDinamicos(c.ColumnaModeloDatosID, GetGlobalResource(c.ClaveRecurso), idTmp++, false, tipoDato.Codigo, false));
                            }
                        });
                        #endregion
                    }
                    #endregion


                    #region Campos Dinamicos
                    if (hdCampoVinculadoTipo.Value.ToString() == "" || hdCampoVinculadoTipo.Value.ToString() == DINAMICO)
                    {
                        long categoriaID = long.Parse(hdCampoVinculadoCategoria.Value.ToString());
                        if (categoriaID > 0)
                        {
                            #region Categorias vinculadas
                            List<InventarioCategorias> vinculadas = cInventarioCategorias.GetVinculadas(categoriaID);
                            vinculadas.ForEach(v =>
                            {
                                lista.Add(new AtributosDinamicos(v.InventarioCategoriaID, v.InventarioCategoria, idTmp++, true, "InventarioCategoria", true));
                            });
                            #endregion
                            #region Campos de categorias vinculadas
                            List<CoreAtributosConfiguraciones> CoreAtributosConfiguraciones = cCoreInventarioCategoriasAtributosCategorias.GetAtributosByInventarioCategoriaID(categoriaID);
                            CoreAtributosConfiguraciones.ForEach(attrConfig =>
                            {
                                if (lista.Find(attr => attr.TypeDynamicID == attrConfig.CoreAtributoConfiguracionID) == null)
                                {
                                    TiposDatos tipoDato = cTiposDatos.GetItem(attrConfig.TipoDatoID);

                                    if (tipoDato.Codigo == "NUMERICO")
                                    {
                                        lista.Add(new AtributosDinamicos(attrConfig.CoreAtributoConfiguracionID, attrConfig.Codigo, idTmp++, true, tipoDato.Codigo, false));
                                    }
                                }
                            });
                            #endregion
                        }
                        #region Campos estaticos del elemento dinamico
                        #region Tablas
                        lTablasModeloDatos = cTablasModeloDatos.GetTablasAsociadas(tablaID);
                        AtributosDinamicos dato;
                        bool control;
                        lTablasModeloDatos.ForEach(c =>
                        {
                            control = false;
                            dato = new AtributosDinamicos(c.TablaModeloDatosID, GetGlobalResource(c.ClaveRecurso), idTmp++, false, "TablaModeloDato", true);
                            foreach (AtributosDinamicos item in lista)
                            {
                                if (item.TypeDynamicID == dato.TypeDynamicID && item.Name == dato.Name)
                                {
                                    control = true;
                                }
                            }

                            if (!control)
                            {
                                lista.Add(dato);
                            }

                        });
                        #endregion
                        #region Columnas de tabla
                        lColumnasModeloDatos = cColumnasModeloDatos.GetColumnasNoFkByTabla(tablaID);
                        lColumnasModeloDatos.ForEach(c =>
                        {
                            TiposDatos tipoDato = tiposDatos.Find(td => td.TipoDatoID == c.TipoDatoID.Value);
                            lista.Add(new AtributosDinamicos(c.ColumnaModeloDatosID, GetGlobalResource(c.ClaveRecurso), idTmp++, false, tipoDato.Codigo, false));
                        });
                        #endregion
                        #endregion
                    }
                    #endregion

                    if (lista != null)
                    {

                        storeSelectCamposVinculados.DataSource = lista;

                        PageProxy temp = (PageProxy)storeSelectCamposVinculados.Proxy[0];
                        temp.Total = lista.Count;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }
        #endregion


        #region storeCampos
        protected void storeCampos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                #region Controllers
                CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();
                CoreExportacionDatosPlantillasFilasController cCoreExportacionDatosPlantillasFilas = new CoreExportacionDatosPlantillasFilasController();
                CoreExportacionDatosPlantillasColumnasController cCoreExportacionDatosPlantillasColumnas = new CoreExportacionDatosPlantillasColumnasController();
                CoreObjetosNegocioTiposController cObjetosNegociosTipos = new CoreObjetosNegocioTiposController();
                TablasModeloDatosController cTablasModeloDatos = new TablasModeloDatosController();
                ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();
                #endregion

                try
                {
                    long lS = long.Parse(hdEditadoObjetoID.Value.ToString());
                    long tablaModeloDatoID = long.Parse(hdTablaModeloDatoID.Value.ToString());
                    long objetoNegocioTipoID = long.Parse(hdObjetoNegocioTipoID.Value.ToString());
                    TablasModeloDatos tablaModeloDato = cTablasModeloDatos.GetItem(tablaModeloDatoID);
                    CoreObjetosNegocioTipos objetoNegocioTipo = cObjetosNegociosTipos.GetItem(objetoNegocioTipoID);
                    bool isCategoryInventory = false;
                    List<long> idsCategories = new List<long>();
                    if (objetoNegocioTipo.TablasModeloDatos.Indice == "InventarioCategoriaID")
                    {
                        isCategoryInventory = true;
                        idsCategories.Add(objetoNegocioTipo.ObjetoTipoID.Value);
                    }

                    bool conAtributosDinamicos = (tablaModeloDato.Indice == "InventarioCategoriaID");

                    List<CampoFiltroInventario> listaCampos = CamposFiltroInventario.GetCamposFiltrosExportacionDatosModeloDato(long.Parse(hdCliID.Value.ToString()), tablaModeloDatoID, conAtributosDinamicos, idsCategories, isCategoryInventory);

                    if (listaCampos != null)
                    {
                        foreach (var item in listaCampos)
                        {
                            if (item.Name.StartsWith("str"))
                            {
                                item.Name = GetGlobalResource(item.Name);
                                item.Name = item.Name.Replace(" ", "").Replace("(", "").Replace(")", "");


                            }
                        }
                        storeCampos.DataSource = listaCampos;
                        storeCampos.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }
        #endregion


        #endregion


        #region DIRECTMETHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            CoreFormulasController cFormulas = new CoreFormulasController();
            long lCliID = 0;
            InfoResponse oResponse;

            try
            {
                if (bAgregar)
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    Data.CoreFormulas oDato = new Data.CoreFormulas();

                    string tipo = hdArbol.Value.ToString();
                    oDato.Nombre = txtNombre.Text;
                    oDato.Codigo = txtCodigo.Text;
                    oDato.ClienteID = lCliID;
                    if (hdObjetoNegocioTipoID.Value.ToString() != "")
                    {
                        oDato.CoreObjetoNegocioTipoID = long.Parse(hdObjetoNegocioTipoID.Value.ToString());
                    }


                    oDato.Formula = "";
                    oDato.Activo = true;

                    oResponse = cFormulas.Add(oDato);

                    if (oResponse.Result)
                    {

                        oResponse = cFormulas.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();
                        }
                        else
                        {
                            cFormulas.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                            return direct;
                        }
                    }
                    else
                    {
                        cFormulas.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        return direct;
                    }
                }

                    

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse GuardarFormula(string Formula, string variables)
        {
            DirectResponse direct = new DirectResponse();
            CoreFormulasController cFormulas = new CoreFormulasController();
            InfoResponse oResponse;
            try
            {
                long formulaID = long.Parse(hdFormulaID.Value.ToString());
                CoreFormulas dato = cFormulas.GetItem(formulaID);
                dato.Formula = string.Join("", Formula);
                dato.JsonVariables = variables;
                oResponse = cFormulas.Update(dato);

                if (oResponse.Result)
                {

                    oResponse = cFormulas.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        storePrincipal.DataBind();
                    }
                    else
                    {
                        cFormulas.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        return direct;
                    }
                }
                else
                {
                    cFormulas.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                    return direct;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse CargarFormula()
        {
            DirectResponse direct = new DirectResponse();
            CoreFormulasController cFormulas = new CoreFormulasController();

            try
            {
                long formulaID = long.Parse(hdFormulaID.Value.ToString());
                CoreFormulas dato = cFormulas.GetItem(formulaID);
                string formula = dato.Formula;
                formula = formula.Trim();
                hdFormula.SetValue(formula);
                string VariablesFormula = dato.JsonVariables;
                hdVariablesFormulas.SetValue(VariablesFormula);

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }
            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            CoreFormulasController cFormulas = new CoreFormulasController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                CoreFormulas oDato = cFormulas.GetItem(lID);
                oResponse = cFormulas.Delete(oDato);
                

                if (oResponse.Result)
                {

                    oResponse = cFormulas.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        storePrincipal.DataBind();
                    }
                    else
                    {
                        cFormulas.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        return direct;
                    }
                }
                else
                {
                    cFormulas.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                    return direct;
                }

            }
            catch (Exception ex)
            {
                if (ex is SqlException Sql)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                    log.Error(Sql.Message);
                    return direct;
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse NombreRuta(JsonObject campo)
        {
            DirectResponse direct = new DirectResponse();
            CoreFormulasController cFormulas = new CoreFormulasController();
            CoreAtributosConfiguracionesController cCoreAtributosConfiguraciones = new CoreAtributosConfiguracionesController();
            InventarioCategoriasController cInventarioCategorias = new InventarioCategoriasController();
            ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();
            try
            {
                List<long> IDsTablas = new List<long>();
                List<long> IDsCategorias = new List<long>();
                string displayFields = "";
                string prueba = campo.ToJsonString();
                CampoVinculadoFormulas campoVinculado = (CampoVinculadoFormulas)JSON.Deserialize(campo.ToJsonString(), typeof(CampoVinculadoFormulas));
                if (campoVinculado.Ruta != null)
                {
                    foreach (ItemPathFormulas ipath in campoVinculado.Ruta.path)
                    {
                        if (ipath.tipo == DINAMICO)
                        {
                            IDsCategorias.Add(ipath.id);
                        }
                        else if (ipath.tipo == ESTATICO)
                        {
                            IDsTablas.Add(ipath.id);
                        }
                    }
                }
                List<InventarioCategorias> atributos = cInventarioCategorias.GetCategoriasByCategoriasIDs(IDsCategorias);
                List<TablasModeloDatos> tablas = cColumnasModeloDatos.GetTablasByIds(IDsTablas);
                if (campoVinculado.Ruta != null)
                {
                    foreach (ItemPathFormulas ipath in campoVinculado.Ruta.path)
                    {
                        if (ipath.tipo == DINAMICO)
                        {
                            InventarioCategorias tb = atributos.Find(t => t.InventarioCategoriaID == ipath.id);
                            displayFields += tb.InventarioCategoria + "";
                        }
                        else if (ipath.tipo == ESTATICO)
                        {
                            TablasModeloDatos tb = tablas.Find(t => t.TablaModeloDatosID == ipath.id);
                            displayFields += GetGlobalResource(tb.ClaveRecurso) + "";
                        }
                    }
                }
                if (campoVinculado.esDinamico)
                {
                    displayFields += cCoreAtributosConfiguraciones.GetItem(campoVinculado.campoVinculadoID).Nombre;
                }
                else
                {
                    displayFields = displayFields + GetGlobalResource(cColumnasModeloDatos.GetItem(campoVinculado.campoVinculadoID).ClaveRecurso);
                }
                hdRuta.SetValue(displayFields);

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }
            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #region AtributosDinamicos
        public class AtributosDinamicos
        {
            public long TypeDynamicID { get; set; }
            public long ID { get; set; }
            public string Name { get; set; }
            public bool Dynamic { get; set; }
            public string DataType { get; set; }
            public bool esCarpeta { get; set; }

            public AtributosDinamicos(long TypeDynamicID, string Name, long ID, bool Dynamic, string DataType, bool esCarpeta)
            {
                this.TypeDynamicID = TypeDynamicID;
                this.ID = ID;
                this.Name = Name;
                this.Dynamic = Dynamic;
                this.DataType = DataType;
                this.esCarpeta = esCarpeta;
            }
        }
        #endregion

        #region Campo vinculado
        public class CampoVinculadoFormulas
        {
            public long variableId { get; set; }
            public string name { get; set; }
            public long campoVinculadoID { get; set; }
            public bool selectorCampoVinculado { get; set; }
            public bool esDinamico { get; set; }
            public PathFormulas Ruta { get; set; }

            public CampoVinculadoFormulas(long id, string name, long CampoVinculadoID, bool selectorCampoVinculado, PathFormulas ruta, bool EsDinamico)
            {
                this.variableId = id;
                this.name = name;
                this.campoVinculadoID = CampoVinculadoID;
                this.selectorCampoVinculado = selectorCampoVinculado;
                this.esDinamico = EsDinamico;
                this.Ruta = ruta;
            }
        }

        public class PathFormulas
        {
            public List<ItemPathFormulas> path { get; set; }
        }
        public class ItemPathFormulas
        {
            public long id { get; set; }
            public string uID { get; set; }
            public string tipo { get; set; }
        }

        #endregion

        #endregion



    }


}