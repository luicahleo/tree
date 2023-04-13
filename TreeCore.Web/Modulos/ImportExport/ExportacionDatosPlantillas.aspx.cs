using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using TreeCore.Clases;
using TreeCore.Data;

namespace TreeCore.ModExportarImportar
{

    public partial class ExportacionDatosPlantillas : Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();
        private int numItemsPreview = 5;

        #region EVENTOS DE PAGINA
        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFiltersTemplate, storeExportacionDatosPlantillas, gridTemplates.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                List<string> listaIgnoreTab = new List<string>()
                { };

                Comun.CreateGridFilters(gridFiltersTemplate, storeExportacionDatosPlantillas, gridTemplates.ColumnModel, listaIgnoreTab, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }

                hdLocale.Value = _Locale.ToString();
            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        colClaveRecurso.Hidden = false;

                        List<Data.Vw_CoreExportacionDatosPlantillas> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        bool bActivo = Request.QueryString["aux"] == "true";
                        string txtBuscado = Request.QueryString["aux3"];
                        hdStringBuscador.SetValue(txtBuscado);
                        int iCount = 0;

                        colActivo.Hidden = bActivo;
                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID, bActivo);


                        listaDatos.ForEach(i =>
                        {
                            i.ClaveRecurso = GetGlobalResource(i.ClaveRecurso);
                        });

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(gridTemplates.ColumnModel, listaDatos, Response, "", GetGlobalResource("strExportacionDatosPlantillas").ToString(), _Locale);
                            log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            EstadisticasController cEstadisticas = new EstadisticasController();
                            cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        Response.Write("ERROR: " + ex.Message);
                    }

                    Response.End();
                }

                ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);
            }
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            hdPermiteEdicion.SetValue("");
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            List<string> listFun = ((List<string>)(this.Session["FUNTIONALITIES"]));
            var UserInterface = ModulesController.GetUserInterfaces().FirstOrDefault(x => x.Page.ToLower() == sPagina.ToLower());
            var listFunPag = listFun.Where(x => $"{x.Split('@')[0]}" == UserInterface.Code);

            if (listFunPag.Where(x => x.Split('@')[1] == "Put").ToList().Count > 0)
            {
                hdPermiteEdicion.SetValue(true);
            }
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { btnAnadir }},
            { "Put", new List<ComponentBase> { btnEditar, btnActivar, cmbField, btnAdd, btnSaveFilter  }},
            { "Delete", new List<ComponentBase> { btnEliminar,colEliminarFiltro }}
        };
        }

        #endregion

        #region STORES

        #region Principal
        protected void storeExportacionDatosPlantillas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort = null, sDir = null;
                    if (e.Sort != null && e.Sort.Length > 0)
                    {
                        sSort = e.Sort[0].Property.ToString();
                        sDir = e.Sort[0].Direction.ToString();
                    }
                    else
                    {
                        sSort = "";
                        sDir = "";
                    }

                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];

                    List<Vw_CoreExportacionDatosPlantillas> lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()), btnActivos.Pressed);

                    if (lista != null)
                    {
                        lista.ForEach(tmp =>
                        {
                            tmp.ClaveRecurso = GetGlobalResource(tmp.ClaveRecurso);
                        });

                        storeExportacionDatosPlantillas.DataSource = lista;

                        PageProxy temp = (PageProxy)storeExportacionDatosPlantillas.Proxy[0];
                        temp.Total = lista.Count;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Vw_CoreExportacionDatosPlantillas> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID, bool activos)
        {
            CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();
            List<Vw_CoreExportacionDatosPlantillas> lista;

            try
            {
                if (lClienteID.HasValue)
                {
                    if (activos)
                    {
                        lista = cCoreExportacionDatosPlantillas.GetItemsWithExtNetFilterList<Vw_CoreExportacionDatosPlantillas>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, /*"ClienteID == " + lClienteID + */" Activo == " + true);
                    }
                    else
                    {
                        lista = cCoreExportacionDatosPlantillas.GetItemsWithExtNetFilterList<Vw_CoreExportacionDatosPlantillas>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, /*"ClienteID == " + lClienteID*/"");
                    }

                    if (lista != null && hdStringBuscador.Value != null && !string.IsNullOrEmpty(hdStringBuscador.Value.ToString()))
                    {
                        string textoBuscado = hdStringBuscador.Value.ToString();
                        lista = lista.FindAll(i => GetGlobalResource(i.ClaveRecurso.ToLower()).Contains(textoBuscado.ToLower()) || i.Nombre.ToLower().Contains(textoBuscado.ToLower()));
                    }
                }
                else
                {
                    lista = new List<Vw_CoreExportacionDatosPlantillas>();
                }

            }
            catch (Exception ex)
            {
                lista = null;
                log.Error(ex.Message);
            }

            return lista;
        }
        #endregion

        #region TablasModelosDatos
        protected void storeTablasModeloDatos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                TablasModeloDatosController cTablasModeloDatos = new TablasModeloDatosController();

                try
                {
                    List<TablasModeloDatos> lista = cTablasModeloDatos.GetActivos(ClienteID.Value);

                    if (lista != null)
                    {
                        lista.ForEach(tmp =>
                        {
                            tmp.ClaveRecurso = GetGlobalResource(tmp.ClaveRecurso);
                        });

                        storeTablasModeloDatos.DataSource = lista;

                        PageProxy temp = (PageProxy)storeTablasModeloDatos.Proxy[0];
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

        #region ColumnasModeloDatos
        protected void storeColumnasModeloDatos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();
                CoreInventarioCategoriasAtributosCategoriasController cCoreInventarioCategoriasAtributosCategorias = new CoreInventarioCategoriasAtributosCategoriasController();
                TiposDatosController cTiposDatos = new TiposDatosController();

                try
                {
                    List<AtributosDinamicos> attrDynamics = new List<AtributosDinamicos>();
                    List<ColumnasModeloDatos> lista = cColumnasModeloDatos.GetColumnasTablasNoFk(long.Parse(hdTablaseleccionadaID.Value.ToString()));

                    if (!hdColumnaSeleccionadaID.IsEmpty)
                    {
                        //Tabla de categoria
                        long lSeleccion = long.Parse(hdColumnaSeleccionadaID.Value.ToString());

                        List<TipoDinamico> tiposDinamicos = GetTiposDinamicos(lSeleccion);

                        tiposDinamicos.ForEach(tipDynamic =>
                        {

                            List<CoreAtributosConfiguraciones> CoreAtributosConfiguraciones = cCoreInventarioCategoriasAtributosCategorias.GetAtributosByInventarioCategoriaID(tipDynamic.Id);

                            CoreAtributosConfiguraciones.ForEach(attrConfig =>
                            {
                                //if (attrDynamics.Find(attr => attr.ID == attrConfig.CoreAtributoConfiguracionID) == null)
                                //{
                                    TiposDatos tipoDato = cTiposDatos.GetItem(attrConfig.TipoDatoID);
                                    attrDynamics.Add(new AtributosDinamicos(tipDynamic.Id, attrConfig.Codigo, attrConfig.CoreAtributoConfiguracionID, true, tipoDato.Codigo, false));
                                //}
                                //else
                                //{
                                //    log.Info("Atribute found");
                                //}
                            });
                        });
                    }

                    if (lista != null)
                    {
                        lista.ForEach(tmp =>
                        {
                            tmp.ClaveRecurso = GetGlobalResource(tmp.ClaveRecurso);
                            TiposDatos tipoDato = cTiposDatos.GetItem(tmp.TipoDatoID.Value);
                            attrDynamics.Add(new AtributosDinamicos(-1, tmp.ClaveRecurso, tmp.ColumnaModeloDatosID, false, tipoDato.Codigo, false));
                        });


                        storeColumnasModeloDatos.DataSource = attrDynamics;

                        PageProxy temp = (PageProxy)storeColumnasModeloDatos.Proxy[0];
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

        #region storeColumnasModeloDatosForm
        protected void storeColumnasModeloDatosForm_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();

                try
                {
                    List<ColumnasModeloDatos> lista = cColumnasModeloDatos.GetColumnasTipoLista(long.Parse(hdTablaModeloDatoForm.Value.ToString()));

                    if (lista != null)
                    {
                        lista.ForEach(tmp =>
                        {
                            tmp.ClaveRecurso = GetGlobalResource(tmp.ClaveRecurso);
                        });

                        storeColumnasModeloDatosForm.DataSource = lista;

                        PageProxy temp = (PageProxy)storeColumnasModeloDatosForm.Proxy[0];
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

        #region Frecuencias
        protected void storeFrecuencias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreServiciosFrecuenciasController cCoreServiciosFrecuencias = new CoreServiciosFrecuenciasController();

                try
                {
                    var lista = cCoreServiciosFrecuencias.GetActivos(ClienteID.Value);

                    if (lista != null)
                    {
                        storeFrecuencias.DataSource = lista;

                        PageProxy temp = (PageProxy)storeFrecuencias.Proxy[0];
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

        #region storeReglaTransformacion
        protected void storeReglaTransformacion_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreExportacionDatosPlantillasReglasController cCoreExportacionDatosPlantillasReglas = new CoreExportacionDatosPlantillasReglasController();

                try
                {
                    long celdaID = long.Parse(hdCeldaAddTransformation.Value.ToString());

                    updateTiposDatosOperadores();


                    List<object> lista = cCoreExportacionDatosPlantillasReglas.GetListReglas(celdaID);

                    if (lista != null)
                    {
                        storeReglaTransformacion.DataSource = lista;

                        PageProxy temp = (PageProxy)storeReglaTransformacion.Proxy[0];
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

        #region storeValorRegla
        protected void storeValorRegla_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {

                CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();

                try
                {
                    long celdaID = long.Parse(hdCeldaID.Value.ToString());
                    CoreExportacionDatosPlantillasCeldas celda = cCoreExportacionDatosPlantillasCeldas.GetItem(celdaID);

                    List<JsonObject> listaDatos = GetListItemsOfTypeList(celda);

                    if (listaDatos != null)
                    {
                        storeValorRegla.DataSource = listaDatos;
                        PageProxy temp = (PageProxy)storeValorRegla.Proxy[0];
                        temp.Total = listaDatos.Count;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<JsonObject> GetListItemsOfTypeList(CoreExportacionDatosPlantillasCeldas celda)
        {
            List<JsonObject> listaDatos = new List<JsonObject>();
            string query = null;

            #region CONTROLLERS
            CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();
            CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
            CoreAtributosConfiguracionesController cCoreAtributosConfiguraciones = new CoreAtributosConfiguracionesController();
            ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();
            EmplazamientosController cController = new EmplazamientosController();
            #endregion

            try
            {

                #region Get query
                if (celda.CoreAtributosConfiguracionID.HasValue)
                {
                    CoreAtributosConfiguraciones oAtrConf = cCoreAtributosConfiguraciones.GetItem(celda.CoreAtributosConfiguracionID.Value);

                    query = cAtributosConf.GetQueryItemsFiltros(oAtrConf.CoreAtributoConfiguracionID);

                }
                else if (celda.ColumnasModeloDatoID.HasValue)
                {
                    CoreExportacionDatosPlantillas plantilla = cCoreExportacionDatosPlantillas.GetByCelda(celda.CoreExportacionDatosPlantillasCeldasID);

                    ColumnasModeloDatos colFK = cColumnasModeloDatos.getColumnaByTablaAndColumn(plantilla.TablaModeloDatosID, celda.ColumnasModeloDatoID.Value);
                    string nombreTabla = cColumnasModeloDatos.getDataSourceTablaColumna(colFK.ColumnaModeloDatosID);
                    string nombreColumna = cColumnasModeloDatos.getNombreColumnaByTabla(colFK.ColumnaModeloDatosID);
                    string indice = cColumnasModeloDatos.getIndiceColumna(colFK.ColumnaModeloDatosID);

                    query = " SELECT " + nombreColumna + ", " + indice + " FROM " + nombreTabla;
                }
                else if (celda.CampoVinculado != null)
                {
                    Export.CampoVinculado campoVinculado = (Export.CampoVinculado)JSON.Deserialize(celda.CampoVinculado, typeof(Export.CampoVinculado));
                    long CampoVinculadoID = campoVinculado.CampoVinculadoID;


                    if (campoVinculado.EsDinamico)
                    {
                        CoreAtributosConfiguraciones oAtrConf = cCoreAtributosConfiguraciones.GetItem(CampoVinculadoID);

                        query = cAtributosConf.GetQueryItemsFiltros(oAtrConf.CoreAtributoConfiguracionID);
                    }
                    else
                    {
                        Export.ItemPath itemPath = campoVinculado.Ruta.path.Last();

                        ColumnasModeloDatos colFK = cColumnasModeloDatos.getColumnaByTablaAndColumn(itemPath.id, CampoVinculadoID);
                        string nombreTabla = cColumnasModeloDatos.getDataSourceTablaColumna(colFK.ColumnaModeloDatosID);
                        string nombreColumna = cColumnasModeloDatos.getNombreColumnaByTabla(colFK.ColumnaModeloDatosID);
                        string indice = cColumnasModeloDatos.getIndiceColumna(colFK.ColumnaModeloDatosID);

                        query = " SELECT " + nombreColumna + ", " + indice + " FROM " + nombreTabla;
                    }
                }
                #endregion

                #region Eejecutar query
                if (query != null)
                {
                    DataTable tablaValores = cController.EjecutarQuery(query);
                    JsonObject oDato;

                    if (tablaValores.Rows.Count > 0)
                    {
                        foreach (DataRow oRow in tablaValores.Rows)
                        {
                            oDato = new JsonObject();
                            oDato.Add("Name", oRow[0].ToString());
                            oDato.Add("ID", oRow[1].ToString());
                            listaDatos.Add(oDato);
                        }
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }
        #endregion

        #region storeColumnasModelo
        protected void storeColumnasModelo_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreExportacionDatosPlantillasColumnasController cCoreExportacionDatosPlantillasColumnas = new CoreExportacionDatosPlantillasColumnasController();

                try
                {
                    long lS = long.Parse(GridRowSelectTemplate.SelectedRecordID);
                    List<CoreExportacionDatosPlantillasColumnas> lista = cCoreExportacionDatosPlantillasColumnas.GetByPlantillaID(lS);

                    if (lista != null)
                    {
                        storeColumnasModelo.DataSource = lista;

                        PageProxy temp = (PageProxy)storeColumnasModelo.Proxy[0];
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

        #region storeFilasModelo
        protected void storeFilasModelo_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreExportacionDatosPlantillasFilasController cCoreExportacionDatosPlantillasFilas = new CoreExportacionDatosPlantillasFilasController();

                try
                {
                    long lS = long.Parse(GridRowSelectTemplate.SelectedRecordID);
                    List<CoreExportacionDatosPlantillasFilas> lista = cCoreExportacionDatosPlantillasFilas.GetByPlantillaID(lS);

                    if (lista != null)
                    {
                        storeFilasModelo.DataSource = lista;

                        PageProxy temp = (PageProxy)storeFilasModelo.Proxy[0];
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

        #region storeCeldas
        protected void storeCeldas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();

                try
                {
                    long lS = long.Parse(GridRowSelectTemplate.SelectedRecordID);
                    List<CoreExportacionDatosPlantillasCeldas> lista = cCoreExportacionDatosPlantillasCeldas.GetByPlantillaID(lS);

                    if (lista != null)
                    {
                        storeCeldas.DataSource = lista;

                        PageProxy temp = (PageProxy)storeCeldas.Proxy[0];
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

        #region storeColumnaCategoria
        protected void storeColumnaCategoria_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<TipoDinamico> tiposDinamicos;

                try
                {
                    long columnaModeloDatoID = long.Parse(hdColumnaSeleccionadaID.Value.ToString());

                    tiposDinamicos = GetTiposDinamicos(columnaModeloDatoID);

                    if (tiposDinamicos != null)
                    {
                        storeColumnaCategoria.DataSource = tiposDinamicos;

                        PageProxy temp = (PageProxy)storeColumnaCategoria.Proxy[0];
                        temp.Total = tiposDinamicos.Count;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }
        #endregion

        #region FILTROS

        #region storeCampos
        protected void storeCampos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                #region Controllers
                CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();
                CoreExportacionDatosPlantillasFilasController cCoreExportacionDatosPlantillasFilas = new CoreExportacionDatosPlantillasFilasController();
                CoreExportacionDatosPlantillasColumnasController cCoreExportacionDatosPlantillasColumnas = new CoreExportacionDatosPlantillasColumnasController();
                TablasModeloDatosController cTablasModeloDatos = new TablasModeloDatosController();
                ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();
                #endregion

                try
                {
                    long lS = long.Parse(GridRowSelectTemplate.SelectedRecordID);
                    CoreExportacionDatosPlantillas plantilla = cCoreExportacionDatosPlantillas.GetItem(lS);
                    TablasModeloDatos tablaModeloDato = cTablasModeloDatos.GetItem(plantilla.TablaModeloDatosID);
                    List<CoreExportacionDatosPlantillasFilas> columnas = cCoreExportacionDatosPlantillasFilas.GetByPlantillaID(lS);
                    bool isCategoryInventory = false;

                    if (plantilla != null && plantilla.ColumnaModeloDatoID.HasValue)
                    {
                        TablasModeloDatos tabla = cColumnasModeloDatos.GetTablaByColumna(plantilla.ColumnaModeloDatoID.Value);
                        if (tabla.Indice == "InventarioCategoriaID")
                        {
                            isCategoryInventory = true;
                        }
                    }

                    List<long> idsCategories = (from c in columnas where c.TipoFiltroID.HasValue select c.TipoFiltroID.Value).ToList();


                    bool conAtributosDinamicos = (tablaModeloDato.Indice == "InventarioElementoID");

                    List<CampoFiltroInventario> listaCampos = CamposFiltroInventario.GetCamposFiltrosExportacionDatosPlantillas(long.Parse(hdCliID.Value.ToString()), lS, conAtributosDinamicos, idsCategories, isCategoryInventory);

                    if (listaCampos != null)
                    {
                        foreach (var item in listaCampos)
                        {
                            if (item.Name.StartsWith("str"))
                            {
                                item.Name = GetGlobalResource(item.Name);
                            }

                            if (!string.IsNullOrEmpty(item.NameCategory))
                            {
                                item.Name = item.NameCategory + " - " + item.Name;
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

        #region Dinamicos
        protected void storeTiposDinamicos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    EmplazamientosController cController = new EmplazamientosController();
                    DataTable tablaValores = cController.EjecutarQuery(hdQuery.Value.ToString());
                    JsonObject oDato;
                    List<JsonObject> listaDatos = new List<JsonObject>();
                    if (tablaValores.Rows.Count > 0)
                    {
                        foreach (DataRow oRow in tablaValores.Rows)
                        {
                            oDato = new JsonObject();
                            oDato.Add("Name", oRow[0].ToString());
                            oDato.Add("ID", oRow[1].ToString());
                            listaDatos.Add(oDato);
                        }
                        storeTiposDinamicos.DataSource = listaDatos;
                        storeTiposDinamicos.DataBind();
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

        #region storeSelectCamposVinculados
        protected void storeSelectCamposVinculados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {

                #region Controllers
                CoreExportacionDatosPlantillasCeldasController cPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
                CoreExportacionDatosPlantillasFilasController cPlantillasFilas = new CoreExportacionDatosPlantillasFilasController();
                CoreExportacionDatosPlantillasController cPlantillas = new CoreExportacionDatosPlantillasController();
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
                    long celdaID = long.Parse(hdCampoVinculadoCeldaID.Value.ToString());
                    string sRuta = hdCampoVinculadoRuta.Value.ToString();

                    Export.Path ruta = (Export.Path)JSON.Deserialize(sRuta, typeof(Export.Path));
                    long tablaID = 0;
                    long idTmp = 0;

                    CoreExportacionDatosPlantillasCeldas celda = cPlantillasCeldas.GetItem(celdaID);
                    CoreExportacionDatosPlantillasFilas fila = cPlantillasFilas.GetByCeldaID(celdaID);
                    CoreExportacionDatosPlantillas Plantilla = cPlantillas.GetByCelda(celdaID);
                    List<TiposDatos> tiposDatos = cTiposDatos.GetItemList();


                    if (ruta == null || ruta.path == null || (ruta.path.Count > 0 && ruta.path.Last().tipo == Export.DINAMICO))
                    {
                        tablaID = Plantilla.TablaModeloDatosID;
                    }
                    else
                    {
                        tablaID = ruta.path.Last().id;
                    }


                    #region Campos Dinamicos
                    if (fila.TipoFiltroID.HasValue && hdCampoVinculadoTipo.Value.ToString() == "" || hdCampoVinculadoTipo.Value.ToString() == Export.DINAMICO)
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
                                    lista.Add(new AtributosDinamicos(attrConfig.CoreAtributoConfiguracionID, attrConfig.Codigo, idTmp++, true, tipoDato.Codigo, false));
                                }
                            });
                            #endregion
                        }

                        #region Campos estaticos del elemento dinamico

                        #region Tablas
                        lTablasModeloDatos = cTablasModeloDatos.GetTablasAsociadas(Plantilla.TablaModeloDatosID);
                        lTablasModeloDatos.ForEach(c =>
                        {
                            lista.Add(new AtributosDinamicos(c.TablaModeloDatosID, GetGlobalResource(c.ClaveRecurso), idTmp++, false, "TablaModeloDato", true));
                        });
                        #endregion

                        #region Columnas de tabla
                        lColumnasModeloDatos = cColumnasModeloDatos.GetColumnasNoFkByTabla(Plantilla.TablaModeloDatosID);
                        lColumnasModeloDatos.ForEach(c =>
                        {
                            TiposDatos tipoDato = tiposDatos.Find(td => td.TipoDatoID == c.TipoDatoID.Value);
                            lista.Add(new AtributosDinamicos(c.ColumnaModeloDatosID, GetGlobalResource(c.ClaveRecurso), idTmp++, false, tipoDato.Codigo, false));
                        });
                        #endregion

                        #endregion
                    }
                    #endregion

                    #region Campos Estaticos
                    else if (hdCampoVinculadoTipo.Value.ToString() == "" || hdCampoVinculadoTipo.Value.ToString() == Export.ESTATICO)
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
                            lista.Add(new AtributosDinamicos(c.ColumnaModeloDatosID, GetGlobalResource(c.ClaveRecurso), idTmp++, false, tipoDato.Codigo, false));
                        });
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

        #region storeCoreExportacionDatosPlantillasReglasCeldas
        protected void storeCoreExportacionDatosPlantillasReglasCeldas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreExportacionDatosPlantillasReglasCeldasController cCoreExportacionDatosPlantillasReglasCeldas = new CoreExportacionDatosPlantillasReglasCeldasController();

                List<Vw_CoreExportacionDatosPlantillasReglasCeldas> transformaciones;


                try
                {
                    long celdaTransformacionID = long.Parse(hdCeldaAddTransformation.Value.ToString());
                    transformaciones = cCoreExportacionDatosPlantillasReglasCeldas.GetByCeldaID(celdaTransformacionID);

                    if (transformaciones != null)
                    {
                        storeCoreExportacionDatosPlantillasReglasCeldas.DataSource = transformaciones;

                        PageProxy temp = (PageProxy)storeCoreExportacionDatosPlantillasReglasCeldas.Proxy[0];
                        temp.Total = transformaciones.Count;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }
        #endregion

        #region storeCoreExportacionDatosPlantillasHistoricos
        protected void storeCoreExportacionDatosPlantillasHistoricos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreExportacionDatosPlantillasHistoricosController cCoreExportacionDatosPlantillasHistoricos = new CoreExportacionDatosPlantillasHistoricosController();

                long lS = long.Parse(GridRowSelectTemplate.SelectedRecordID);

                try
                {
                    List<CoreExportacionDatosPlantillasHistoricos> historicos = cCoreExportacionDatosPlantillasHistoricos.GetActivosByPlantillaID(lS);

                    if (historicos != null)
                    {
                        storeCoreExportacionDatosPlantillasHistoricos.DataSource = historicos;

                        PageProxy temp = (PageProxy)storeCoreExportacionDatosPlantillasHistoricos.Proxy[0];
                        temp.Total = historicos.Count;
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

        #region DIRECT METHOD

        #region AgregarEditar
        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();
            CoreServiciosFrecuenciasController cCoreServiciosFrecuencias = new CoreServiciosFrecuenciasController();
            cCoreServiciosFrecuencias.SetDataContext(cCoreExportacionDatosPlantillas.Context);


            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    if (!bAgregar)
                    {
                        bool actuExpo;
                        bool actuFrecu;
                        long lS = long.Parse(GridRowSelectTemplate.SelectedRecordID);
                        CoreExportacionDatosPlantillas oDato;
                        oDato = cCoreExportacionDatosPlantillas.GetItem(lS);

                        CoreServiciosFrecuencias fDato;
                        fDato = cCoreServiciosFrecuencias.GetItem(long.Parse(oDato.CoreServicioFrecuenciaID.ToString()));

                        if (oDato.Nombre == txtNombre.Text)
                        {
                            oDato.Nombre = txtNombre.Text;
                            oDato.TablaModeloDatosID = long.Parse((string)cmbTablasModelosDatos.Value);
                            oDato.CoreServicioFrecuenciaID = oDato.CoreServicioFrecuenciaID;
                            oDato.TipoFichero = cmbTipoFichero.Value.ToString();

                            if (!cmbColumnasModeloDatosForm.IsEmpty)
                            {
                                oDato.ColumnaModeloDatoID = long.Parse((string)cmbColumnasModeloDatosForm.Value);
                            }
                        }
                        else
                        {
                            if (cCoreExportacionDatosPlantillas.RegistroDuplicado(txtNombre.Text))
                            {
                                log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                            }
                            else
                            {
                                oDato.Nombre = txtNombre.Text;
                                oDato.TablaModeloDatosID = long.Parse((string)cmbTablasModelosDatos.Value);
                                //oDato.CoreServicioFrecuenciaID = long.Parse(cmbFrecuencias.Value.ToString());
                                oDato.TipoFichero = cmbTipoFichero.Value.ToString();

                                if (!cmbColumnasModeloDatosForm.IsEmpty)
                                {
                                    oDato.ColumnaModeloDatoID = long.Parse((string)cmbColumnasModeloDatosForm.Value);
                                }
                            }
                        }
                        actuExpo = cCoreExportacionDatosPlantillas.UpdateItem(oDato);

                        fDato.Nombre = fDato.Nombre;
                        fDato.FechaInicio = ProgramadorExportar.FechaInicio;

                        if (ProgramadorExportar.Frecuencias == "NoSeRepite")
                        {
                            fDato.FechaFin = null;
                        }
                        else
                        {
                            if (fDato.FechaFin != null)
                            {
                                fDato.FechaFin = ProgramadorExportar.FechaFin;
                            }
                        }

                        fDato.TipoFrecuencia = ProgramadorExportar.Frecuencias;
                        fDato.CronFormat = ProgramadorExportar.CronFormat;

                        actuFrecu = cCoreServiciosFrecuencias.UpdateItem(fDato);

                        if (actuFrecu != null && actuExpo != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storeExportacionDatosPlantillas.DataBind();
                            trans.Complete();
                        }
                        else if (actuFrecu != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storeExportacionDatosPlantillas.DataBind();
                            trans.Complete();
                        }
                        else if (actuExpo != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storeExportacionDatosPlantillas.DataBind();
                            trans.Complete();
                        }
                        else
                        {
                            trans.Dispose();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            return direct;
                        }

                    }
                    else
                    {

                        Data.CoreServiciosFrecuencias oFrecuencia = new Data.CoreServiciosFrecuencias();
                        //if (ProgramadorExportar.Frecuencias == "NoSeRepite")
                        //{
                        oFrecuencia.Nombre = "Cron_" + txtNombre.Text;
                        oFrecuencia.Activo = true;
                        oFrecuencia.FechaInicio = ProgramadorExportar.FechaInicio;
                        oFrecuencia.CronFormat = ProgramadorExportar.CronFormat;
                        oFrecuencia.TipoFrecuencia = ProgramadorExportar.Frecuencias;

                        if (ProgramadorExportar.FechaFin != DateTime.MinValue)
                        {
                            oFrecuencia.FechaFin = ProgramadorExportar.FechaFin;
                        }
                        oFrecuencia = cCoreServiciosFrecuencias.AddItem(oFrecuencia);

                        if (oFrecuencia != null)
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        }
                        //}


                        if (cCoreExportacionDatosPlantillas.RegistroDuplicado(txtNombre.Text))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            CoreExportacionDatosPlantillas oDato = new CoreExportacionDatosPlantillas()
                            {
                                Activo = true,
                                Nombre = txtNombre.Text,
                                TablaModeloDatosID = long.Parse((string)cmbTablasModelosDatos.Value),
                                CoreServicioFrecuenciaID = oFrecuencia.CoreServicioFrecuenciaID,
                                TipoFichero = cmbTipoFichero.Value.ToString()
                            };

                            if (!cmbColumnasModeloDatosForm.IsEmpty)
                            {
                                oDato.ColumnaModeloDatoID = long.Parse((string)cmbColumnasModeloDatosForm.Value);
                            }

                            oDato = cCoreExportacionDatosPlantillas.AddItem(oDato);

                            if (oDato != null)
                            {
                                log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                storeExportacionDatosPlantillas.DataBind();
                                trans.Complete();
                            }
                            else
                            {
                                trans.Dispose();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                return direct;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    if (ex is SqlException Sql)
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
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
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }
        #endregion

        #region Eliminar
        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();

            #region Controllers
            CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();
            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            cCoreExportacionDatosPlantillasCeldas.SetDataContext(cCoreExportacionDatosPlantillas.Context);
            CoreExportacionDatosPlantillasFilasController cCoreExportacionDatosPlantillasFilas = new CoreExportacionDatosPlantillasFilasController();
            cCoreExportacionDatosPlantillasFilas.SetDataContext(cCoreExportacionDatosPlantillas.Context);
            CoreExportacionDatosPlantillasColumnasController cCoreExportacionDatosPlantillasColumnas = new CoreExportacionDatosPlantillasColumnasController();
            cCoreExportacionDatosPlantillasColumnas.SetDataContext(cCoreExportacionDatosPlantillas.Context);
            CoreServiciosFrecuenciasController cCoreServiciosFrecuencias = new CoreServiciosFrecuenciasController();
            cCoreServiciosFrecuencias.SetDataContext(cCoreExportacionDatosPlantillas.Context);
            #endregion

            long lID = long.Parse(GridRowSelectTemplate.SelectedRecordID);

            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    List<long> columnasIDs = new List<long>();
                    CoreExportacionDatosPlantillas plantilla = cCoreExportacionDatosPlantillas.GetItem(lID);
                    List<CoreExportacionDatosPlantillasCeldas> celdas = cCoreExportacionDatosPlantillasCeldas.GetByPlantillaID(lID);

                    if (celdas != null)
                    {
                        celdas = celdas.FindAll(x => x.CeldaPadreID == null);
                        celdas.ForEach(celda =>
                        {
                            if (!columnasIDs.Contains(celda.CoreExportacionDatosPlantillaColumnaID))
                            {
                                columnasIDs.Add(celda.CoreExportacionDatosPlantillaColumnaID);
                            }
                            cCoreExportacionDatosPlantillasCeldas.DeleteCeldaCascada(celda.CoreExportacionDatosPlantillasCeldasID, celda.CoreExportacionDatosPlantillaFilaID, celda.CoreExportacionDatosPlantillaColumnaID);
                        });
                    }

                    List<CoreExportacionDatosPlantillasFilas> filas = cCoreExportacionDatosPlantillasFilas.GetByPlantillaID(lID);
                    if (filas != null)
                    {
                        filas.ForEach(fila =>
                        {
                            if (!cCoreExportacionDatosPlantillasFilas.DeleteItem(fila.CoreExportacionDatosPlantillaFilaID))
                            {
                                trans.Dispose();
                            }
                        });
                    }

                    columnasIDs.ForEach(col =>
                    {
                        if (!cCoreExportacionDatosPlantillasColumnas.DeleteItem(col))
                        {
                            trans.Dispose();
                        }
                    });

                    long? idFrecuencia = (plantilla.CoreServicioFrecuenciaID.HasValue) ? plantilla.CoreServicioFrecuenciaID.Value : new long?();

                    if (!cCoreExportacionDatosPlantillas.DeleteItem(lID))
                    {
                        trans.Dispose();
                    }

                    if (idFrecuencia.HasValue && !cCoreServiciosFrecuencias.DeleteItem(idFrecuencia.Value))
                    {
                        trans.Dispose();
                    }

                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";

                    trans.Complete();
                }
                catch (Exception ex)
                {
                    trans.Dispose();
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
            }

            return direct;
        }
        #endregion

        #region Activar
        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();

            try
            {
                long lID = long.Parse(GridRowSelectTemplate.SelectedRecordID);

                CoreExportacionDatosPlantillas oDato;
                oDato = cCoreExportacionDatosPlantillas.GetItem(lID);

                oDato.Activo = !oDato.Activo;

                if (cCoreExportacionDatosPlantillas.UpdateItem(oDato))
                {
                    storeExportacionDatosPlantillas.DataBind();
                    log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
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
        #endregion

        #region MostrarEditar
        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();
            CoreServiciosFrecuenciasController cServiciosFrecuencias = new CoreServiciosFrecuenciasController();

            try
            {
                long lS = long.Parse(GridRowSelectTemplate.SelectedRecordID);

                Data.CoreExportacionDatosPlantillas oDato;
                oDato = cCoreExportacionDatosPlantillas.GetItem<Data.CoreExportacionDatosPlantillas>(lS);

                txtNombre.SetText(oDato.Nombre);
                cmbTablasModelosDatos.SetValue(oDato.TablaModeloDatosID);
                cmbTablasModelosDatos.Disable();
                cmbColumnasModeloDatosForm.SetValue(oDato.ColumnaModeloDatoID);
                cmbColumnasModeloDatosForm.Disable();
                //cmbFrecuencias.SetValue(oDato.CoreServicioFrecuenciaID);
                cmbTipoFichero.SetValue(oDato.TipoFichero);

                #region Frecuencia
                //FRECUENCIAS
                if (oDato.CoreServicioFrecuenciaID.HasValue)
                {
                    Data.CoreServiciosFrecuencias oFrecuencias = cServiciosFrecuencias.GetItem(oDato.CoreServicioFrecuenciaID.Value);

                    ProgramadorExportar.Frecuencias = oFrecuencias.TipoFrecuencia;
                    ProgramadorExportar.FechaInicio = oFrecuencias.FechaInicio;

                    if (oFrecuencias.FechaFin != null)
                    {
                        ProgramadorExportar.FechaFin = (DateTime)oFrecuencias.FechaFin;
                    }

                    if (oFrecuencias.TipoFrecuencia == "SemanalCustom")
                    {

                        string[] cron = oFrecuencias.CronFormat.Split(' ');
                        string diasSemana = cron.Last();
                        string[] Dias = diasSemana.Split(',');
                        //string diasFormateado = "";
                        //foreach (var dia in Dias)
                        //{
                        //    diasFormateado = (diasFormateado + "'"  + dia + "', ");
                        //}
                        //diasFormateado = diasFormateado.Remove((diasFormateado.Length - 1), 1);
                        //diasFormateado = diasFormateado.Remove((diasFormateado.Length - 1), 1);

                        ProgramadorExportar.Dias = Dias.ToList();

                        //foreach (var item in ProgramadorExportar.Dias)
                        //{
                        //}
                    }
                    else if (oFrecuencias.TipoFrecuencia == "MensualCustom")
                    {

                        string[] cron = oFrecuencias.CronFormat.Split(' ');
                        string diaMes = cron[2];
                        ProgramadorExportar.DiaCadaMes = long.Parse(diaMes);
                        string meses = cron[3];

                        if (meses.Contains("/"))
                        {
                            //long numeroMes = 0;

                            string[] separador = meses.Split('/');
                            //if (separador[0] == "January" || separador[0] == "1")
                            //{
                            //    numeroMes = 1;
                            //}
                            //else if (separador[0] == "February" || separador[0] == "2")
                            //{
                            //    numeroMes = 2;
                            //}
                            //else if (separador[0] == "March" || separador[0] == "3")
                            //{
                            //    numeroMes = 3;
                            //}
                            //else if (separador[0] == "April" || separador[0] == "4")
                            //{
                            //    numeroMes = 4;
                            //}
                            //else if (separador[0] == "May" || separador[0] == "5")
                            //{
                            //    numeroMes = 5;
                            //}
                            //else if (separador[0] == "June" || separador[0] == "6")
                            //{
                            //    numeroMes = 6;
                            //}
                            //else if (separador[0] == "July" || separador[0] == "7")
                            //{
                            //    numeroMes = 7;
                            //}
                            //else if (separador[0] == "August" || separador[0] == "8")
                            //{
                            //    numeroMes = 8;
                            //}
                            //else if (separador[0] == "September" || separador[0] == "9")
                            //{
                            //    numeroMes = 9;
                            //}
                            //else if (separador[0] == "October" || separador[0] == "10")
                            //{
                            //    numeroMes = 10;
                            //}
                            //else if (separador[0] == "November" || separador[0] == "11")
                            //{
                            //    numeroMes = 11;
                            //}
                            //else if (separador[0] == "December" || separador[0] == "12")
                            //{
                            //    numeroMes = 12;
                            //}

                            ProgramadorExportar.TipoFrecuencia = '/' + separador[1];
                            //ProgramadorExportar.MesInicio = numeroMes;
                        }
                        else
                        {
                            string[] MesesArray = meses.Split(',');
                            ProgramadorExportar.Meses = MesesArray.ToList();
                        }
                    }
                }
                #endregion

                winAddTemplateDataExport.Show();
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
        #endregion

        #region Guardar Modelo

        #region Guardar Columna
        [DirectMethod()]
        public DirectResponse GuardarColumna(string sJSON)
        {
            DirectResponse direct = new DirectResponse();
            CoreExportacionDatosPlantillasColumnasController cCoreExportacionDatosPlantillasColumnas = new CoreExportacionDatosPlantillasColumnasController();
            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            cCoreExportacionDatosPlantillasCeldas.SetDataContext(cCoreExportacionDatosPlantillasColumnas.Context);

            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    JObject col = JObject.Parse(sJSON);
                    long lS = long.Parse(GridRowSelectTemplate.SelectedRecordID);

                    string nomCol = (string)col["nombre"];
                    bool eliminar = false;

                    if (col.ContainsKey("remove"))
                    {
                        eliminar = (bool)col["remove"];
                    }

                    long? idCol;

                    try
                    {
                        idCol = (long)col["id"];
                    }
                    catch (Exception ex)
                    {
                        idCol = null;
                    }

                    CoreExportacionDatosPlantillasColumnas columna;
                    if (idCol == null)
                    {
                        List<long> filasIDs = new List<long>();
                        columna = new CoreExportacionDatosPlantillasColumnas()
                        {
                            Nombre = nomCol
                        };

                        if (col.ContainsKey("filasIDs"))
                        {
                            JArray filasIDJSON = (JArray)col["filasIDs"];

                            for (int i = 0; i < filasIDJSON.Count; i++)
                            {
                                filasIDs.Add((long)filasIDJSON[i]);
                            }
                        }


                        columna = cCoreExportacionDatosPlantillasColumnas.AddItem(columna);
                        if (columna != null)
                        {
                            filasIDs.ForEach(idFila =>
                            {

                                CoreExportacionDatosPlantillasCeldas celda = new CoreExportacionDatosPlantillasCeldas()
                                {
                                    CoreExportacionDatosPlantillaFilaID = idFila,
                                    CoreExportacionDatosPlantillaColumnaID = columna.CoreExportacionDatosPlantillaColumnaID
                                };

                                cCoreExportacionDatosPlantillasCeldas.AddItem(celda);
                            });
                        }
                    }
                    else if (eliminar)
                    {
                        cCoreExportacionDatosPlantillasCeldas.DeleteByColumna(idCol.Value);

                        if (cCoreExportacionDatosPlantillasColumnas.DeleteItem(idCol.Value))
                        {

                        }
                    }
                    else
                    {
                        columna = cCoreExportacionDatosPlantillasColumnas.GetItem(idCol.Value);
                        columna.Nombre = nomCol;

                        if (cCoreExportacionDatosPlantillasColumnas.UpdateItem(columna))
                        {

                        }
                    }

                    direct.Success = true;
                    trans.Complete();
                }
                catch (Exception ex)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    trans.Dispose();
                    return direct;
                }
            }

            direct.Result = GetModeloPlantilla().Result;

            return direct;
        }
        #endregion

        #region Guardar celda
        [DirectMethod()]
        public DirectResponse GuardarCelda(string sJSON, long filaID)
        {
            DirectResponse direct = new DirectResponse();

            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    JObject celda = JObject.Parse(sJSON);
                    long? idCelda;
                    long? attributoID;
                    long? colID;
                    bool eliminarCelda = false;
                    string separador = null;
                    long? celdaPadreID = null;
                    bool atributoDinamico = false;
                    string FormatoFecha = null;//Comun.FORMATO_FECHA;

                    try
                    {
                        idCelda = (long)celda["id"];
                    }
                    catch (Exception ex)
                    {
                        idCelda = null;
                    }

                    try
                    {
                        attributoID = (long)celda["attributoID"];
                    }
                    catch (Exception ex)
                    {
                        attributoID = null;
                    }

                    try
                    {
                        atributoDinamico = (bool)celda["esDinamico"];
                    }
                    catch (Exception ex)
                    {
                        atributoDinamico = false;
                    }

                    try
                    {
                        colID = (long)celda["columnID"];
                    }
                    catch (Exception ex)
                    {
                        colID = 0;//idsColumns[(string)celda["columnID"]];
                    }

                    if (celda.ContainsKey("remove"))
                    {
                        eliminarCelda = (bool)celda["remove"];
                    }
                    if (celda.ContainsKey("separador"))
                    {
                        separador = (string)celda["separador"];
                    }
                    if (celda.ContainsKey("celdaPadreID") && ((long?)celda["celdaPadreID"]) != null)
                    {
                        celdaPadreID = (long)celda["celdaPadreID"];
                    }
                    if (celda.ContainsKey("FormatoFecha"))
                    {
                        FormatoFecha = (string)celda["FormatoFecha"];
                    }


                    CoreExportacionDatosPlantillasCeldas oCelda;
                    if (idCelda == null)
                    {
                        //AgregarCelda
                        oCelda = new CoreExportacionDatosPlantillasCeldas()
                        {
                            CoreExportacionDatosPlantillaColumnaID = colID.Value,
                            CoreExportacionDatosPlantillaFilaID = filaID,
                            Separador = separador,
                            CeldaPadreID = celdaPadreID
                        };

                        if (attributoID != null && attributoID.Value > 0)
                        {
                            if (!EliminarTransformacionSiCambiaAtributo(cCoreExportacionDatosPlantillasCeldas.Context, oCelda, attributoID.Value))
                            {
                                trans.Dispose();
                            }

                            if (!atributoDinamico)
                            {
                                oCelda.ColumnasModeloDatoID = attributoID.Value;
                                oCelda.CoreAtributosConfiguracionID = null;
                            }
                            else
                            {
                                oCelda.CoreAtributosConfiguracionID = attributoID.Value;
                                oCelda.ColumnasModeloDatoID = null;
                            }
                            oCelda.CampoVinculado = null;
                        }

                        oCelda.FormatoFecha = FormatoFecha;

                        cCoreExportacionDatosPlantillasCeldas.AddItem(oCelda);

                        if (celdaPadreID.HasValue)
                        {
                            CoreExportacionDatosPlantillasCeldas celdaPadre = cCoreExportacionDatosPlantillasCeldas.GetItem(oCelda.CeldaPadreID.Value);
                            if (celdaPadre != null && string.IsNullOrEmpty(celdaPadre.Separador))
                            {
                                celdaPadre.Separador = "-";
                                cCoreExportacionDatosPlantillasCeldas.UpdateItem(celdaPadre);
                            }
                        }

                        direct.Success = true;
                    }
                    else if (eliminarCelda)
                    {
                        //Eliminar
                        direct.Success = cCoreExportacionDatosPlantillasCeldas.DeleteItem(idCelda.Value);
                    }
                    else
                    {
                        //EditarCelda
                        oCelda = cCoreExportacionDatosPlantillasCeldas.GetItem(idCelda.Value);
                        oCelda.CoreExportacionDatosPlantillaColumnaID = colID.Value;
                        oCelda.CoreExportacionDatosPlantillaFilaID = filaID;
                        oCelda.Separador = separador;
                        oCelda.CeldaPadreID = celdaPadreID;
                        oCelda.FormatoFecha = FormatoFecha;

                        if (attributoID != null && attributoID.Value > 0)
                        {
                            if (!EliminarTransformacionSiCambiaAtributo(cCoreExportacionDatosPlantillasCeldas.Context, oCelda, attributoID.Value))
                            {
                                trans.Dispose();
                            }

                            if (!atributoDinamico)
                            {
                                oCelda.ColumnasModeloDatoID = attributoID.Value;
                                oCelda.CoreAtributosConfiguracionID = null;
                            }
                            else
                            {
                                oCelda.CoreAtributosConfiguracionID = attributoID.Value;
                                oCelda.ColumnasModeloDatoID = null;
                            }
                        }

                        direct.Success = cCoreExportacionDatosPlantillasCeldas.UpdateItem(oCelda);
                    }



                    trans.Complete();
                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }
            }

            direct.Result = GetModeloPlantilla().Result;

            return direct;
        }
        #endregion

        #region Eliminar celda
        [DirectMethod()]
        public DirectResponse EliminarCelda(long celdaID, long filaID, long columnaID)
        {
            DirectResponse direct = new DirectResponse();
            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();

            try
            {
                cCoreExportacionDatosPlantillasCeldas.DeleteCeldaCascada(celdaID, filaID, columnaID);

                direct.Result = GetModeloPlantilla().Result;

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #region Guardar Fila
        [DirectMethod()]
        public DirectResponse GuardarFila(string sJSON)
        {
            DirectResponse direct = new DirectResponse();
            
            #region Controllers
            CoreExportacionDatosPlantillasFilasController cCoreExportacionDatosPlantillasFilas = new CoreExportacionDatosPlantillasFilasController();
            CoreExportacionDatosPlantillasColumnasController cCoreExportacionDatosPlantillasColumnas = new CoreExportacionDatosPlantillasColumnasController();
            cCoreExportacionDatosPlantillasColumnas.SetDataContext(cCoreExportacionDatosPlantillasFilas.Context);
            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            cCoreExportacionDatosPlantillasCeldas.SetDataContext(cCoreExportacionDatosPlantillasFilas.Context);
            #endregion
            
            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                

                    JObject tipoFila = JObject.Parse(sJSON);
                    long lS = long.Parse(GridRowSelectTemplate.SelectedRecordID);

                    List<CoreExportacionDatosPlantillasColumnas> columnas = cCoreExportacionDatosPlantillasColumnas.GetByPlantillaID(lS);

                    bool eliminarFila = false;
                    long? filaID;
                    long? TipoFiltroID = null;
                    long? TipoFiltroDinamicoID = null;

                    try
                    {
                        filaID = (long)tipoFila["tipoID"];
                    }
                    catch (Exception ex)
                    {
                        filaID = null;
                    }
                    //long columnaModeloDatoID = (long)tipoFila["columnaModeloDatoID"];
                    //tipoFila["attributoID"];

                    if (tipoFila.ContainsKey("remove"))
                    {
                        eliminarFila = (bool)tipoFila["remove"];
                    }
                    if (tipoFila.ContainsKey("TipoFiltroID"))
                    {
                        TipoFiltroID = (long)tipoFila["TipoFiltroID"];
                    }
                    if (tipoFila.ContainsKey("TipoFiltroDinamicoID"))
                    {
                        TipoFiltroDinamicoID = (long)tipoFila["TipoFiltroDinamicoID"];
                    }


                    CoreExportacionDatosPlantillasFilas fila;

                    if (filaID == null)
                    {
                        //ADD
                        fila = new CoreExportacionDatosPlantillasFilas()
                        {
                            CoreExportacionDatosPlantillaID = lS
                        };

                        if (TipoFiltroID.HasValue)
                        {
                            fila.TipoFiltroID = TipoFiltroID;
                        }
                        else if (TipoFiltroDinamicoID.HasValue)
                        {
                            fila.TipoFiltroDinamicoID = TipoFiltroDinamicoID;
                        }

                        fila = cCoreExportacionDatosPlantillasFilas.AddItem(fila);
                        filaID = fila.CoreExportacionDatosPlantillaFilaID;

                        if (fila != null)
                        {
                            columnas.ForEach(col =>
                            {
                                CoreExportacionDatosPlantillasCeldas celda = new CoreExportacionDatosPlantillasCeldas()
                                {
                                    CoreExportacionDatosPlantillaColumnaID = col.CoreExportacionDatosPlantillaColumnaID,
                                    CoreExportacionDatosPlantillaFilaID = fila.CoreExportacionDatosPlantillaFilaID
                                };

                                cCoreExportacionDatosPlantillasCeldas.AddItem(celda);
                            });
                        }
                    }
                    else if (eliminarFila)
                    {
                        //REMOVE
                        cCoreExportacionDatosPlantillasFilas.DeleteItem(filaID.Value);
                        filaID = filaID.Value;
                    }
                    else
                    {
                        //EDIT
                        fila = cCoreExportacionDatosPlantillasFilas.GetItem(filaID.Value);
                        long? TipoFiltroTmp = null;
                        long? TipoFiltroOld = null;

                        if (TipoFiltroID.HasValue)
                        {
                            TipoFiltroOld = fila.TipoFiltroID;
                            TipoFiltroTmp = TipoFiltroID;
                            fila.TipoFiltroID = TipoFiltroID;
                        }
                        else if (TipoFiltroDinamicoID.HasValue)
                        {
                            TipoFiltroOld = fila.TipoFiltroDinamicoID;
                            TipoFiltroTmp = TipoFiltroDinamicoID;
                            fila.TipoFiltroDinamicoID = TipoFiltroDinamicoID;
                        }

                        if (TipoFiltroOld != TipoFiltroTmp && TipoFiltroOld != null)
                        {
                            EliminarCeldasByFila(fila.CoreExportacionDatosPlantillaFilaID, cCoreExportacionDatosPlantillasFilas.Context);
                            #region crearCeldas vacias
                            foreach (CoreExportacionDatosPlantillasColumnas columna in columnas)
                            {
                                CoreExportacionDatosPlantillasCeldas celdTmp = new CoreExportacionDatosPlantillasCeldas()
                                                                            {
                                                                                CoreExportacionDatosPlantillaFilaID = fila.CoreExportacionDatosPlantillaFilaID,
                                                                                CoreExportacionDatosPlantillaColumnaID = columna.CoreExportacionDatosPlantillaColumnaID
                                                                            };
                                celdTmp = cCoreExportacionDatosPlantillasCeldas.AddItem(celdTmp);
                                if (celdTmp == null)
                                {
                                    trans.Dispose();
                                }
                            }
                            #endregion
                        }

                        cCoreExportacionDatosPlantillasFilas.UpdateItem(fila);
                        filaID = filaID.Value;
                    }

                    trans.Complete();
                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }

                direct = GetModeloPlantilla();
            }
            return direct;
        }
        #endregion

        #region EliminarFila
        [DirectMethod()]
        public DirectResponse EliminarFila(long filaID)
        {
            DirectResponse direct = new DirectResponse();

            #region Controllers
            CoreExportacionDatosPlantillasFilasController cPlantillasFilas = new CoreExportacionDatosPlantillasFilasController();
            CoreExportacionDatosPlantillasCeldasController cPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            cPlantillasCeldas.SetDataContext(cPlantillasFilas.Context);

            #endregion

            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    EliminarCeldasByFila(filaID, cPlantillasFilas.Context);

                    if (!cPlantillasFilas.DeleteItem(filaID))
                    {
                        trans.Dispose();
                    }
                    else
                    {
                        trans.Complete();
                    }
                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }
            }

            direct.Result = GetModeloPlantilla().Result;

            return direct;
        }
        #endregion

        #region Reglas de transformación

        #region Delete regla
        [DirectMethod()]
        public DirectResponse DeleteTransformation(long reglaCeldaID, long columnID, long typID)
        {
            DirectResponse direct = new DirectResponse();
            CoreExportacionDatosPlantillasReglasCeldasController cCoreExportacionDatosPlantillasReglasCeldas = new CoreExportacionDatosPlantillasReglasCeldasController();

            try
            {
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
                {
                    long celdaID = cCoreExportacionDatosPlantillasReglasCeldas.GetItem(reglaCeldaID).CoreExportacionDatosPlantillasCeldaID.Value;
                    if (cCoreExportacionDatosPlantillasReglasCeldas.DeleteItem(reglaCeldaID))
                    {
                        cCoreExportacionDatosPlantillasReglasCeldas.ReordenarReglasByCelda(celdaID);

                        direct.Success = true;
                        trans.Complete();
                    }
                    else
                    {
                        trans.Dispose();
                        direct.Success = false;
                    }
                }
                direct.Result = GetModeloPlantilla().Result;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #region Guardar Regla
        [DirectMethod()]
        public DirectResponse AddTransformationRuleGuardar(long celdaID, long? reglaTransFormacionID)
        {
            DirectResponse direct = new DirectResponse();
            bool ValorCeldaValueMax = false;
            bool guardar = false;

            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            CoreExportacionDatosPlantillasReglasCeldasController cCoreReglasCeldas = new CoreExportacionDatosPlantillasReglasCeldasController();
            CoreExportacionDatosPlantillasReglasCeldas oReglaTransformacion;

            try
            {
                CoreExportacionDatosPlantillasCeldas celda = cCoreExportacionDatosPlantillasCeldas.GetItem(celdaID);
                celda.FormatoFecha = txtFormatoFecha.Text;
                cCoreExportacionDatosPlantillasCeldas.UpdateItem(celda);

                if (reglaTransFormacionID.HasValue)
                {
                    oReglaTransformacion = cCoreReglasCeldas.GetItem(reglaTransFormacionID.Value);
                }
                else
                {
                    oReglaTransformacion = new CoreExportacionDatosPlantillasReglasCeldas();
                    oReglaTransformacion.Orden = cCoreReglasCeldas.GetNuevoNumeroOrden(celdaID);
                }
                oReglaTransformacion.CoreExportacionDatosPlantillasCeldaID = celdaID;
                oReglaTransformacion.CheckValorDefecto = btnValuePorDefecto.Pressed;

                if (cmbRule.SelectedItem != null && cmbRule.SelectedItem.Value != null && !string.IsNullOrEmpty(txtCellValueIs.Text))
                {
                    oReglaTransformacion.CoreExportacionDatosPlantillasReglaID = long.Parse(cmbRule.SelectedItem.Value.ToString());
                    oReglaTransformacion.ValorIf = txtCellValueIs.Value.ToString();


                    oReglaTransformacion.ValorCelda = string.Empty;
                    if (bool.Parse(ReglaRequiereValor.Value.ToString()))
                    {
                        if (txtValorRegla.Value != null && !string.IsNullOrEmpty(txtValorRegla.Value.ToString()))
                        {
                            oReglaTransformacion.ValorCelda = txtValorRegla.Value.ToString();
                        }
                        else if (numberValorRegla.Value != null && !string.IsNullOrEmpty(numberValorRegla.Value.ToString()))
                        {
                            oReglaTransformacion.ValorCelda = numberValorRegla.Value.ToString();
                        }
                        else if (dateValorRegla.Value != null && !string.IsNullOrEmpty(dateValorRegla.Value.ToString()))
                        {
                            oReglaTransformacion.ValorCelda = dateValorRegla.Value.ToString();
                        }
                        else if (cmbValorRegla.SelectedItems != null && cmbValorRegla.SelectedItems.Count > 0)
                        {
                            string s = "";
                            foreach (ListItem item in cmbValorRegla.SelectedItems)
                            {
                                s += (string.IsNullOrEmpty(s) ? "" : ",");
                                s += (!string.IsNullOrEmpty(item.Value)) ? item.Value : item.Text;
                            }

                            oReglaTransformacion.ValorCelda = s;
                        }
                        else if (checkboxValorRegla.Value != null && !string.IsNullOrEmpty(checkboxValorRegla.Value.ToString()))
                        {
                            oReglaTransformacion.ValorCelda = checkboxValorRegla.Checked.ToString();
                        }
                    }

                    ValorCeldaValueMax = (oReglaTransformacion.ValorCelda.Length > 2000);

                    //if (chkValuePorDefecto.Checked)
                    //{
                    oReglaTransformacion.ValorDefecto = (txtElseCellValue.Value == null) ? "" : txtElseCellValue.Value.ToString();
                    //}

                    guardar = true;
                }
                else if (reglaTransFormacionID.HasValue || btnValuePorDefecto.Pressed)
                {
                    oReglaTransformacion.ValorIf = "";
                    oReglaTransformacion.ValorCelda = "";
                    if (btnValuePorDefecto.Pressed)
                    {
                        oReglaTransformacion.ValorDefecto = (txtElseCellValue.Value == null) ? "" : txtElseCellValue.Value.ToString();
                    }
                    else
                    {
                        oReglaTransformacion.ValorDefecto = "";
                    }
                    guardar = true;
                }

                if (reglaTransFormacionID.HasValue)
                {
                    cCoreReglasCeldas.UpdateItem(oReglaTransformacion);

                    direct.Success = true;
                }
                else if (guardar)
                {
                    oReglaTransformacion = cCoreReglasCeldas.AddItem(oReglaTransformacion);
                    if (oReglaTransformacion != null)
                    {
                        direct.Success = true;
                    }
                }

                direct.Result = GetModeloPlantilla().Result;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                if (ValorCeldaValueMax)
                {
                    direct.Result = GetGlobalResource("strValorDemasiadoGrande");
                }
                else
                {
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                }
                
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #region Mostrar/Editar Regla
        [DirectMethod()]
        public DirectResponse MostrarEditarTransformacion(long? reglaCeldaID, long? celdID, string TipoDato)
        {
            DirectResponse direct = new DirectResponse();

            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            CoreExportacionDatosPlantillasReglasCeldasController cCoreReglasCeldas = new CoreExportacionDatosPlantillasReglasCeldasController();
            Vw_CoreExportacionDatosPlantillasReglasCeldas oRegla;
            CoreExportacionDatosPlantillasCeldas oCelda = null;

            try
            {
                if (reglaCeldaID.HasValue)
                {
                    oRegla = cCoreReglasCeldas.GetItemVw(reglaCeldaID.Value);
                    oCelda = cCoreExportacionDatosPlantillasCeldas.GetItem(oRegla.CoreExportacionDatosPlantillasCeldaID.Value);

                    labelBtnValuePorDefecto.Hide();
                    btnValuePorDefecto.Hide();
                    btnValuePorDefecto.SetPressed(oRegla.CheckValorDefecto);
                    cmbRule.SetValue(oRegla.CoreExportacionDatosPlantillasReglaID);
                    txtCellValueIs.SetValue(oRegla.ValorIf);
                    txtElseCellValue.SetValue(oRegla.ValorDefecto);

                    if (oRegla.RequiereValor.HasValue)
                    {
                        ReglaRequiereValor.SetValue(oRegla.RequiereValor.Value);
                    }

                    dateValorRegla.Hide();
                    numberValorRegla.Hide();
                    txtValorRegla.Hide();
                    checkboxValorRegla.Hide();
                    if (TipoDato == Comun.TIPODATO_CODIGO_FECHA)
                    {
                        dateValorRegla.Show();
                        dateValorRegla.Enable();

                        if (!string.IsNullOrEmpty(oRegla.ValorCelda))
                        {
                            dateValorRegla.Value = (DateTime.Parse(oRegla.ValorCelda));
                        }
                        else
                        {
                            dateValorRegla.Hide();
                        }
                    }
                    else if (TipoDato == Comun.TIPODATO_CODIGO_NUMERICO ||
                        TipoDato == Comun.TIPODATO_CODIGO_NUMERICO_ENTERO ||
                        TipoDato == Comun.TIPODATO_CODIGO_NUMERICO_FLOTANTE)
                    {
                        if (oRegla.RequiereValor != null && oRegla.RequiereValor.Value)
                        {
                            numberValorRegla.Show();
                            numberValorRegla.Enable();
                            numberValorRegla.SetValue(float.Parse(oRegla.ValorCelda));
                        }
                        else
                        {
                            numberValorRegla.Hide();
                            numberValorRegla.Disable();
                            numberValorRegla.SetValue(0);
                        }
                    }
                    else if (TipoDato == Comun.TIPODATO_CODIGO_BOOLEAN)
                    {
                        checkboxValorRegla.Show();
                        checkboxValorRegla.Enable();
                        checkboxValorRegla.SetValue(bool.Parse(oRegla.ValorCelda));
                    }
                    else if (TipoDato == Comun.TIPODATO_CODIGO_LISTA || TipoDato == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                    {
                        string[] values =  oRegla.ValorCelda.Split(',');

                        cmbValorRegla.Show();
                        cmbValorRegla.Enable();
                        cmbValorRegla.SetValue(values);
                    }
                    else
                    {
                        txtValorRegla.Show();
                        txtValorRegla.Enable();
                        txtValorRegla.SetValue(oRegla.ValorCelda);
                    }

                    if (oRegla.CheckValorDefecto)
                    {
                        txtValorRegla.Hide();
                        txtValorRegla.Disable();
                        txtValorRegla.AllowBlank = true;

                        cmbRule.Hide();
                        txtValorRegla.Hide();
                        txtCellValueIs.Hide();
                        cmbRule.Disable();
                        txtValorRegla.Disable();
                        txtCellValueIs.Disable();

                        txtElseCellValue.Enable();
                        txtElseCellValue.Show();
                    }
                    else
                    {
                        /*txtValorRegla.Show();
                        txtValorRegla.Enable();
                        txtValorRegla.AllowBlank = false;

                        cmbRule.Show();
                        cmbRule.Enable();
                        txtValorRegla.Show();
                        txtValorRegla.Enable();
                        txtCellValueIs.Show();
                        txtCellValueIs.Enable();*/

                        txtElseCellValue.Hide();
                    }
                }
                else
                {
                    btnValuePorDefecto.Hide();
                    labelBtnValuePorDefecto.Hide();

                    cmbRule.Hide();
                    cmbRule.Disable();
                    cmbRule.AllowBlank = true;

                    txtCellValueIs.Hide();
                    txtCellValueIs.Disable();
                    txtCellValueIs.AllowBlank = true;

                    txtValorRegla.Hide();
                    txtValorRegla.Disable();
                    txtValorRegla.AllowBlank = true;

                    txtElseCellValue.Hide();
                    txtElseCellValue.Disable();
                    txtElseCellValue.AllowBlank = true;
                }

                if (celdID.HasValue && TipoDato == Comun.TIPODATO_CODIGO_FECHA)
                {
                    if (oCelda == null)
                    {
                        oCelda = cCoreExportacionDatosPlantillasCeldas.GetItem(celdID.Value);
                    }
                    txtFormatoFecha.SetValue(oCelda.FormatoFecha);
                    //txtFormatoFecha.Show();
                    txtFormatoFecha.Enable();
                    txtFormatoFecha.AllowBlank = false;
                }
                else
                {
                    txtFormatoFecha.Hide();
                    txtFormatoFecha.Disable();
                    txtFormatoFecha.AllowBlank = true;
                }

                winAddTransformationRule.Show();
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            return direct;
        }
        #endregion
        
        #region TextoFijo

        #region MostrarEditarTextoFijo
        [DirectMethod()]
        public DirectResponse MostrarEditarTextoFijo(long celdaID)
        {
            DirectResponse direct = new DirectResponse();

            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();

            try
            {
                CoreExportacionDatosPlantillasCeldas celda = cCoreExportacionDatosPlantillasCeldas.GetItem(celdaID);
                hdCeldaIDTextoFijo.SetValue(celdaID.ToString());

                txtTextoFijo.Text = (celda.TextoFijo == null) ? "" : celda.TextoFijo;

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            return direct;
        }
        #endregion

        #region GuardarTextoFijo
        [DirectMethod()]
        public DirectResponse GuardarTextoFijo()
        {
            DirectResponse direct = new DirectResponse();

            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();

            try
            {
                long celdaID = long.Parse(hdCeldaIDTextoFijo.Value.ToString());
                CoreExportacionDatosPlantillasCeldas celda = cCoreExportacionDatosPlantillasCeldas.GetItem(celdaID);
                celda.TextoFijo = txtTextoFijo.Text;

                direct.Success = cCoreExportacionDatosPlantillasCeldas.UpdateItem(celda);
                direct.Result = GetModeloPlantilla().Result;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #endregion

        #region DeleteFormatoFecha
        [DirectMethod()]
        public DirectResponse DeleteFormatoFecha(long celdaID)
        {
            DirectResponse direct = new DirectResponse();
            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();

            try
            {
                CoreExportacionDatosPlantillasCeldas celda = cCoreExportacionDatosPlantillasCeldas.GetItem(celdaID);
                celda.FormatoFecha = null;

                if (cCoreExportacionDatosPlantillasCeldas.UpdateItem(celda))
                {
                    direct.Success = true;
                    direct.Result = GetModeloPlantilla().Result;
                }
                else
                {
                    direct.Success = false;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #region Validar formato fecha
        [DirectMethod()]
        public DirectResponse ValidarFormatoFecha(string formato)
        {
            DirectResponse direct = new DirectResponse();


            try
            {
                bool valido = false;

                try
                {
                    DateTime.Now.ToString(formato);
                    valido = true;
                }
                catch (Exception ex)
                {
                    valido = false;
                }

                direct.Result = valido;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        #endregion

        #region ActualizarOrdenTransformacion
        [DirectMethod()]
        public DirectResponse ActualizarOrdenTransformacion(long idTransformacion, int orden)
        {
            DirectResponse direct = new DirectResponse();
            CoreExportacionDatosPlantillasReglasCeldasController cCoreExportacionDatosPlantillasReglasCeldas = new CoreExportacionDatosPlantillasReglasCeldasController();

            try
            {
                CoreExportacionDatosPlantillasReglasCeldas transformacion = cCoreExportacionDatosPlantillasReglasCeldas.GetItem(idTransformacion);
                transformacion.Orden = orden;

                if (cCoreExportacionDatosPlantillasReglasCeldas.UpdateItem(transformacion))
                {
                    direct.Success = true;
                }

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #endregion

        #region ResetCeldToInitState
        [DirectMethod()]
        public DirectResponse ResetCeldToInitState(long colID, long tipoID, long celdaID) {
            DirectResponse direct = new DirectResponse();

            #region Controllers
            CoreExportacionDatosPlantillasCeldasController cPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            #endregion

            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    cPlantillasCeldas.ResetCelda(celdaID);

                    trans.Complete();
                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }
            }

            direct.Result = GetModeloPlantilla().Result;

            return direct;
        }
        #endregion

        #endregion

        #region GetModeloPlantilla
        [DirectMethod()]
        public DirectResponse GetModeloPlantilla()
        {
            DirectResponse direct = new DirectResponse();

            #region Controllers
            CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();
            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            CoreExportacionDatosPlantillasColumnasController cCoreExportacionDatosPlantillasColumnas = new CoreExportacionDatosPlantillasColumnasController();
            CoreExportacionDatosPlantillasFilasController cCoreExportacionDatosPlantillasFilas = new CoreExportacionDatosPlantillasFilasController();
            CoreExportacionDatosPlantillasReglasCeldasController cCoreReglasCeldas = new CoreExportacionDatosPlantillasReglasCeldasController();
            ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();
            CoreAtributosConfiguracionesController cCoreAtributosConfiguraciones = new CoreAtributosConfiguracionesController();
            InventarioCategoriasController cInventarioCategorias = new InventarioCategoriasController();
            #endregion

            JObject objJSON = new JObject();
            bool mostrarColumnaCategoria = false;
            bool esCategoriaDinamica = false;

            try
            {
                long lS = long.Parse(GridRowSelectTemplate.SelectedRecordID);

                Vw_CoreExportacionDatosPlantillas oPlantilla = cCoreExportacionDatosPlantillas.GetItem<Vw_CoreExportacionDatosPlantillas>(lS);

                if (oPlantilla != null)
                {
                    List<CoreExportacionDatosPlantillasColumnas> columnas = cCoreExportacionDatosPlantillasColumnas.GetByPlantillaID(lS);
                    if (columnas.Count == 0)
                    {
                        CrearNuevacolumna(oPlantilla);
                        columnas = cCoreExportacionDatosPlantillasColumnas.GetByPlantillaID(lS);
                    }

                    List<CoreExportacionDatosPlantillasFilas> filas = cCoreExportacionDatosPlantillasFilas.GetByPlantillaID(lS);
                    List<CoreExportacionDatosPlantillasCeldas> celdas = cCoreExportacionDatosPlantillasCeldas.GetByPlantillaID(lS);

                    objJSON.Add("Activo", oPlantilla.Activo);
                    objJSON.Add("ClaveRecurso", GetGlobalResource(oPlantilla.ClaveRecurso));
                    objJSON.Add("CoreExportacionDatoPlantillaID", oPlantilla.CoreExportacionDatoPlantillaID);
                    objJSON.Add("Nombre", oPlantilla.Nombre);
                    objJSON.Add("UnaVez", oPlantilla.UnaVez);
                    objJSON.Add("campoFila", "");
                    //objJSON.Add("Filtro", oPlantilla.Filtro);

                    #region Filas y Celdas
                    JArray jArrayFilas = new JArray();
                    foreach(CoreExportacionDatosPlantillasFilas fila in filas)
                    {
                        JObject filaJSON = new JObject();
                        filaJSON.Add("tipoID", fila.CoreExportacionDatosPlantillaFilaID);
                        //filaJSON.Add("columnaID", fila.ColumnasModeloDatoID);
                        // filaJSON.Add("attributoID", fila.ColumnasModeloDatoID);

                        if (oPlantilla.ColumnaModeloDatoID.HasValue && (!filaJSON.ContainsKey("TipoFiltroID") || !filaJSON.ContainsKey("TipoFiltroDinamicoID")))
                        {
                            mostrarColumnaCategoria = true;

                            if (fila.TipoFiltroID.HasValue)
                            {
                                filaJSON.Add("TipoFiltroID", fila.TipoFiltroID);
                            }
                            else if (fila.TipoFiltroDinamicoID.HasValue)
                            {
                                esCategoriaDinamica = true;
                                filaJSON.Add("TipoFiltroDinamicoID", fila.TipoFiltroDinamicoID);
                            }
                        }

                        #region Celdas
                        JArray jArrayCeldas = new JArray();
                        foreach(var celda in celdas)
                        {
                            string tipoDatoCelda = "";
                            if (celda.CoreExportacionDatosPlantillaFilaID == fila.CoreExportacionDatosPlantillaFilaID)
                            {
                                tipoDatoCelda = cCoreExportacionDatosPlantillasCeldas.getTipoDato(celda);
                                JObject celdaJSON = new JObject();
                                celdaJSON.Add("id", celda.CoreExportacionDatosPlantillasCeldasID);
                                if (celda.ColumnasModeloDatoID != null)
                                {
                                    celdaJSON.Add("esDinamico", false);
                                    celdaJSON.Add("attributoID", celda.ColumnasModeloDatoID);
                                }
                                else
                                {
                                    celdaJSON.Add("esDinamico", true);
                                    celdaJSON.Add("attributoID", celda.CoreAtributosConfiguracionID);
                                }
                                celdaJSON.Add("columnID", celda.CoreExportacionDatosPlantillaColumnaID);
                                celdaJSON.Add("columnaModeloDatoID", celda.ColumnasModeloDatoID);
                                celdaJSON.Add("CoreAtributosConfiguracionID", celda.CoreAtributosConfiguracionID);

                                #region CampoVinculado
                                if (!string.IsNullOrEmpty(celda.CampoVinculado))
                                {
                                    List<long> IDsTablas = new List<long>();
                                    List<long> IDsCategorias = new List<long>();
                                    string displayFields = "";
                                    Export.CampoVinculado campoVinculado = JsonConvert.DeserializeObject<Export.CampoVinculado>(celda.CampoVinculado);


                                    if (campoVinculado.Ruta != null)
                                    {
                                        foreach (Export.ItemPath ipath in campoVinculado.Ruta.path)
                                        {
                                            if (ipath.tipo == Export.DINAMICO)
                                            {
                                                IDsCategorias.Add(ipath.id);
                                            }
                                            else if (ipath.tipo == Export.ESTATICO)
                                            {
                                                IDsTablas.Add(ipath.id);
                                            }
                                        }
                                    }
                                    

                                    List<InventarioCategorias> atributos = cInventarioCategorias.GetCategoriasByCategoriasIDs(IDsCategorias);
                                    List<TablasModeloDatos> tablas = cColumnasModeloDatos.GetTablasByIds(IDsTablas);
                                    if (campoVinculado.Ruta != null)
                                    {
                                        foreach (Export.ItemPath ipath in campoVinculado.Ruta.path)
                                        {
                                            if (ipath.tipo == Export.DINAMICO)
                                            {
                                                InventarioCategorias tb = atributos.Find(t => t.InventarioCategoriaID == ipath.id);
                                                displayFields += tb.InventarioCategoria + "/";
                                            }
                                            else if (ipath.tipo == Export.ESTATICO)
                                            {
                                                TablasModeloDatos tb = tablas.Find(t => t.TablaModeloDatosID == ipath.id);
                                                displayFields += GetGlobalResource(tb.ClaveRecurso) + "/";
                                            }
                                        }
                                    }

                                    if (campoVinculado.EsDinamico)
                                    {
                                        displayFields += cCoreAtributosConfiguraciones.GetItem(campoVinculado.CampoVinculadoID).Nombre;
                                    }
                                    else
                                    {
                                        displayFields = displayFields + GetGlobalResource(cColumnasModeloDatos.GetItem(campoVinculado.CampoVinculadoID).ClaveRecurso);
                                    }

                                    JObject jo = new JObject();
                                    jo.Add("CampoVinculadoID", campoVinculado.CampoVinculadoID);
                                    jo.Add("EsDinamico", campoVinculado.EsDinamico);
                                    jo.Add("Ruta", (campoVinculado.Ruta != null) ? campoVinculado.Ruta.ToString() : null);
                                    jo.Add("DisplayField", displayFields);
                                    jo.Add("TipoDato", campoVinculado.TipoDato);

                                    celdaJSON.Add("CampoVinculado", jo);
                                }
                                #endregion

                                celdaJSON.Add("TipoDato", tipoDatoCelda);
                                celdaJSON.Add("FormatoFecha", celda.FormatoFecha);
                                celdaJSON.Add("TextoFijo", celda.TextoFijo);

                                List<CoreExportacionDatosPlantillasCeldas> enMismaColumna = celdas.FindAll(c => c.CoreExportacionDatosPlantillaColumnaID == celda.CoreExportacionDatosPlantillaColumnaID && c.CoreExportacionDatosPlantillasCeldasID != celda.CoreExportacionDatosPlantillasCeldasID);
                                List<CoreExportacionDatosPlantillasCeldas> celdasHijas = celdas.FindAll(c => c.CoreExportacionDatosPlantillaColumnaID == celda.CoreExportacionDatosPlantillaColumnaID && c.CeldaPadreID == celda.CoreExportacionDatosPlantillasCeldasID);

                                if (enMismaColumna.Count > 0)
                                {
                                    celdaJSON.Add("celdaPadreID", celda.CeldaPadreID);
                                    celdaJSON.Add("separador", celda.Separador);

                                    bool configurarSeparador = (celdasHijas.Count > 0);
                                    celdaJSON.Add("configurarSeparador", configurarSeparador);
                                    if (celda.CeldaPadreID.HasValue || celdasHijas.Count > 0)
                                    {
                                        celdaJSON.Add("mismaCelda", true);
                                    }
                                    else
                                    {
                                        celdaJSON.Add("mismaCelda", false);
                                    }
                                }

                                #region Reglas de transformación
                                JArray jArrayReglasTransfor = new JArray();

                                List<Vw_CoreExportacionDatosPlantillasReglasCeldas> reglasCeldas = cCoreReglasCeldas.GetByCeldaID(celda.CoreExportacionDatosPlantillasCeldasID);

                                reglasCeldas = Export.ordenarListaReglasDeTransformacion(reglasCeldas);

                                reglasCeldas.ForEach(reglaCelda =>
                                {
                                    JObject regla = new JObject();
                                    regla.Add("regla", reglaCelda.Nombre);
                                    regla.Add("reglaID", reglaCelda.CoreExportacionDatosPlantillasReglaID);
                                    regla.Add("reglaCeldaID", reglaCelda.CoreExportacionDatosPlantillasReglaCeldaID);
                                    regla.Add("ValorDefecto", reglaCelda.ValorDefecto);
                                    regla.Add("ValorIf", reglaCelda.ValorIf);
                                    regla.Add("CheckValorDefecto", reglaCelda.CheckValorDefecto);

                                    if (reglaCelda.CheckValorDefecto) {
                                        #region valor por defecto

                                        regla.Add("valor", reglaCelda.ValorDefecto);
                                        regla.Add("valorDisplay", reglaCelda.ValorDefecto);
                                        #endregion
                                    }
                                    else
                                    {
                                        #region No valor por defecto
                                        if (tipoDatoCelda == Comun.TIPODATO_CODIGO_FECHA)
                                        {
                                            try
                                            {
                                                string sValor = DateTime.Parse(reglaCelda.ValorCelda).ToString(celda.FormatoFecha);
                                                regla.Add("valor", sValor);
                                                regla.Add("valorDisplay", sValor);
                                            }
                                            catch (Exception ex)
                                            {
                                                string sValor = reglaCelda.ValorCelda;
                                                regla.Add("valor", sValor);
                                                regla.Add("valorDisplay", sValor);
                                                log.Error(ex);
                                            }
                                        }
                                        else if (tipoDatoCelda == Comun.TIPODATO_CODIGO_LISTA || tipoDatoCelda == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                                        {
                                            string sValor = reglaCelda.ValorCelda;
                                            string sDisplayValor = "";

                                            string[] sValores = sValor.Split(',');

                                            List<JsonObject> listaValores = GetListItemsOfTypeList(celda);
                                            foreach (string v in sValores)
                                            {
                                                JsonObject item = listaValores.Find(i => {
                                                    object id;
                                                    bool exist = i.TryGetValue("ID", out id);

                                                    return exist && id.ToString() == v;
                                                });

                                                object obj;
                                                if (item != null && item.TryGetValue("Name", out obj))
                                                {
                                                    sDisplayValor += (string.IsNullOrEmpty(sDisplayValor) ? "" : ", ");
                                                    sDisplayValor += item["Name"].ToString();
                                                }
                                            }

                                            regla.Add("valor", sValor);
                                            regla.Add("valorDisplay", sDisplayValor);
                                        }
                                        else
                                        {
                                            string sValor = reglaCelda.ValorCelda;
                                            regla.Add("valor", sValor);
                                            regla.Add("valorDisplay", sValor);
                                        }
                                        #endregion
                                    }

                                    regla.Add("NombreOperador", reglaCelda.Nombre);
                                    regla.Add("Operador", reglaCelda.Operador);
                                    regla.Add("ClaveRecursoOperador", reglaCelda.ClaveRecurso);

                                    jArrayReglasTransfor.Add(regla);
                                });

                                celdaJSON.Add("reglasTransformacion", jArrayReglasTransfor);
                                #endregion

                                jArrayCeldas.Add(celdaJSON);
                            }
                        }

                        filaJSON.Add("celdas", jArrayCeldas);
                        #endregion

                        jArrayFilas.Add(filaJSON);
                    }

                    objJSON.Add("tiposFila", jArrayFilas);

                    bool mostrarBotonAddFila = false;
                    if (oPlantilla.ColumnaModeloDatoID.HasValue)
                    {
                        List<TipoDinamico> categoriasDinamicas = GetTiposDinamicos(oPlantilla.ColumnaModeloDatoID.Value);
                        mostrarBotonAddFila = (filas.Count < categoriasDinamicas.Count);
                    }
                    objJSON.Add("mostrarBotonAddFila", mostrarBotonAddFila);
                    #endregion

                    #region Columns
                    JArray jArrayColumns = new JArray();
                    columnas.ForEach(columna =>
                    {
                        JObject columnaJSON = new JObject();
                        columnaJSON.Add("id", columna.CoreExportacionDatosPlantillaColumnaID);
                        columnaJSON.Add("nombre", columna.Nombre);

                        jArrayColumns.Add(columnaJSON);
                    });

                    objJSON.Add("columnas", jArrayColumns);
                    #endregion

                    objJSON.Add("mostrarColumnaCategoria", mostrarColumnaCategoria);
                    objJSON.Add("esCategoriaDinamica", esCategoriaDinamica);
                }

                direct.Success = true;
                direct.Result = objJSON;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }


            return direct;
        }
        #endregion

        #region ShowPreview
        [DirectMethod()]
        public DirectResponse ShowPreview()
        {
            DirectResponse direct = new DirectResponse();
            CoreExportacionDatosPlantillas plantilla;
            List<string> cabecera = new List<string>();
            List<JsonObject> listaOut = new List<JsonObject>();
            string PreviewID = "PreviewID";

            CoreExportacionDatosPlantillasColumnasController cCoreExportacionDatosPlantillasColumnas = new CoreExportacionDatosPlantillasColumnasController();
            CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();
            CoreExportacionDatosPlantillasFilasController cCoreExportacionDatosPlantillasFilas = new CoreExportacionDatosPlantillasFilasController();
            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController(); CoreExportacionDatosPlantillasReglasCeldasController cCoreExportacionDatosPlantillasReglasCeldas = new CoreExportacionDatosPlantillasReglasCeldasController();
            ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();
            TiposDatosController cTiposDatos = new TiposDatosController();

            try
            {


                long plantillaID = long.Parse(GridRowSelectTemplate.SelectedRecordID);


                plantilla = cCoreExportacionDatosPlantillas.GetItem(plantillaID);

                List<CoreExportacionDatosPlantillasColumnas> columnas = cCoreExportacionDatosPlantillasColumnas.GetByPlantillaID(plantillaID);
                Dictionary<string, string> formatosFecha = new Dictionary<string, string>();

                List<ColumnasModeloDatos> columnasModeloDatos = cColumnasModeloDatos.GetItemList();
                List<Vw_CoreExportacionDatosPlantillasReglasCeldas> reglasCelda = cCoreExportacionDatosPlantillasReglasCeldas.GetByPlantilla(plantilla.CoreExportacionDatoPlantillaID);

                #region load dictionaries
                Dictionary<long, ColumnasModeloDatos> dicColumnasModeloDatos = new Dictionary<long, ColumnasModeloDatos>();
                Dictionary<long, string> dicNombresTablas = new Dictionary<long, string>();
                Dictionary<long, string> dicControladores = new Dictionary<long, string>();
                Dictionary<long, string> dicDisplay = new Dictionary<long, string>();
                Dictionary<long, TiposDatos> dicTiposDatos = new Dictionary<long, TiposDatos>();

                columnasModeloDatos.ForEach(colModDat => {
                    if (colModDat.ForeignKeyID.HasValue)
                    {
                        string sNombreTabla = cColumnasModeloDatos.getDataSourceTablaColumna(colModDat.ForeignKeyID.Value);
                        if (!dicNombresTablas.ContainsKey(colModDat.ForeignKeyID.Value))
                        {
                            dicNombresTablas.Add(colModDat.ForeignKeyID.Value, sNombreTabla);
                        }
                    }

                    if (colModDat.ForeignKeyID.HasValue)
                    {
                        string sControlador = cColumnasModeloDatos.getControllerColumna(colModDat.ForeignKeyID.Value);
                        if (!dicControladores.ContainsKey(colModDat.ForeignKeyID.Value))
                        {
                            dicControladores.Add(colModDat.ForeignKeyID.Value, sControlador);
                        }
                    }

                    string sDisplay = cColumnasModeloDatos.getDisplay(colModDat.ColumnaModeloDatosID);
                    if (!dicDisplay.ContainsKey(colModDat.ColumnaModeloDatosID))
                    {
                        dicDisplay.Add(colModDat.ColumnaModeloDatosID, sDisplay);
                    }

                    if (!dicTiposDatos.ContainsKey(colModDat.TipoDatoID.Value))
                    {
                        TiposDatos tipoDato = cTiposDatos.GetItem(colModDat.TipoDatoID.Value);
                        if (tipoDato != null)
                        {
                            dicTiposDatos.Add(colModDat.TipoDatoID.Value, tipoDato);
                        }
                    }
                });

                if (plantilla.ColumnaModeloDatoID.HasValue)
                {
                    ColumnasModeloDatos colModDatFiltro = cColumnasModeloDatos.GetItem(plantilla.ColumnaModeloDatoID.Value);
                    if (colModDatFiltro != null && !dicColumnasModeloDatos.ContainsKey(plantilla.ColumnaModeloDatoID.Value))
                    {
                        dicColumnasModeloDatos.Add(plantilla.ColumnaModeloDatoID.Value, colModDatFiltro);
                    }
                }

                #endregion





                //if (hdIDPlantillaPreview.IsEmpty || long.Parse(hdIDPlantillaPreview.Value.ToString()) != plantillaID)
                //{
                limpiarGridPreview();

                #region Generar ColumnModel

                ModelField modelFieldID = new ModelField
                {
                    Name = PreviewID,
                    Type = ModelFieldType.Int
                };

                storegrdPreview.ModelInstance.Fields.Add(modelFieldID);



                int index = 0;
                foreach (CoreExportacionDatosPlantillasColumnas columna in columnas)
                {
                    ModelField modelField = new ModelField
                    {
                        Name = columna.Nombre.Replace(" ", "").Replace("(", "").Replace(")", "").Replace(",", "").Replace("/", ""),
                        Type = ModelFieldType.String
                    };

                    storegrdPreview.ModelInstance.Fields.Add(modelField);

                    Column col = new Column
                    {
                        ID = "col" + index + columna.CoreExportacionDatosPlantillaColumnaID,
                        Text = columna.Nombre,
                        DataIndex = columna.CoreExportacionDatosPlantillaColumnaID.ToString(),
                        Hidden = false,
                        MinWidth = 110,
                        Flex = 1,
                        Fixed = true
                    };

                    grdPreview.InsertColumn(index++, col);

                }
                #endregion
                //}

                #region Generar Datos
                List<CoreExportacionDatosPlantillasCeldas> celdas = cCoreExportacionDatosPlantillasCeldas.GetByPlantillaID(plantilla.CoreExportacionDatoPlantillaID);
                List<CoreExportacionDatosPlantillasFilas> filas = cCoreExportacionDatosPlantillasFilas.GetByPlantillaID(plantilla.CoreExportacionDatoPlantillaID);

                if (columnas != null)
                {
                    columnas.ForEach(col =>
                    {
                        cabecera.Add(col.Nombre);
                    });
                }

                foreach (CoreExportacionDatosPlantillasFilas fila in filas)
                {
                    long? TipoFiltroID = null;
                    long? TipoFiltroDinamicoID = null;
                    if (fila.TipoFiltroID.HasValue)
                    {
                        TipoFiltroID = fila.TipoFiltroID;
                    }
                    else if (fila.TipoFiltroDinamicoID.HasValue)
                    {
                        TipoFiltroDinamicoID = fila.TipoFiltroDinamicoID;
                    }


                    List<CoreExportacionDatosPlantillasCeldas> celdasTemp = celdas.FindAll(c => c.CoreExportacionDatosPlantillaFilaID == fila.CoreExportacionDatosPlantillaFilaID);

                    string query = Export.GenerateQuery(plantilla, celdasTemp, plantilla.TablaModeloDatosID, plantilla.ColumnaModeloDatoID, numItemsPreview, TipoFiltroID, TipoFiltroDinamicoID);
                    log.Info("Generated Query");
                    List<JsonObject> listaTemp = cCoreExportacionDatosPlantillas.ejecutarConsulta(query);
                    log.Info("Query executed");
                    listaTemp = Export.GetBodyExcel(plantilla, listaTemp, columnas, celdas, filas, formatosFecha, out formatosFecha, fila,
                        dicColumnasModeloDatos, dicNombresTablas, dicControladores, dicDisplay, dicTiposDatos, reglasCelda);

                    listaOut.AddRange(listaTemp);
                }

                int indexPreviewID = 1;
                listaOut.ForEach(objJson =>
                {
                    if (!objJson.ContainsKey(PreviewID))
                    {
                        objJson.Add(PreviewID, indexPreviewID++);
                    }
                });

                log.Info("Results of all queries: " + listaOut.Count);

                #endregion

                hdIDPlantillaPreview.SetValue(plantillaID);
                direct.Result = listaOut;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #region Filtro

        #region GuardarFiltro
        [DirectMethod()]
        public DirectResponse GuardarFiltro(string sJSON)
        {
            DirectResponse direct = new DirectResponse();

            CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();


            try
            {
                long plantillaID = long.Parse(GridRowSelectTemplate.SelectedRecordID);

                CoreExportacionDatosPlantillas plantilla = cCoreExportacionDatosPlantillas.GetItem(plantillaID);

                plantilla.Filtro = sJSON;

                if (!cCoreExportacionDatosPlantillas.UpdateItem(plantilla))
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #region EliminarFiltro
        [DirectMethod()]
        public DirectResponse EliminarFiltro()
        {
            DirectResponse direct = new DirectResponse();

            CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();

            try
            {
                long plantillaID = long.Parse(GridRowSelectTemplate.SelectedRecordID);

                CoreExportacionDatosPlantillas plantilla = cCoreExportacionDatosPlantillas.GetItem(plantillaID);

                plantilla.Filtro = null;

                if (!cCoreExportacionDatosPlantillas.UpdateItem(plantilla))
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #endregion

        #region GuardarCampoVinculado
        [DirectMethod()]
        public DirectResponse GuardarCampoVinculado(long campoVinculadoID, bool Dynamic, string tipoDato)
        {
            DirectResponse direct = new DirectResponse();

            CoreExportacionDatosPlantillasCeldasController cPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();

            try
            {
                long celdaID = long.Parse(hdCampoVinculadoCeldaID.Value.ToString());
                CoreExportacionDatosPlantillasCeldas celda = cPlantillasCeldas.GetItem(celdaID);
                string sRuta = hdCampoVinculadoRuta.Value.ToString();
                
                Export.Path ruta = JsonConvert.DeserializeObject<Export.Path>(sRuta);

                Export.CampoVinculado campoVinculado = new Export.CampoVinculado(campoVinculadoID, Dynamic, ruta, tipoDato);
                string sCampoVinculado = JSON.Serialize(campoVinculado);

                if (tipoDato == Comun.TIPODATO_CODIGO_FECHA)
                {
                    celda.FormatoFecha = Comun.FORMATO_FECHA;
                }

                celda.CampoVinculado = sCampoVinculado;
                celda.CoreAtributosConfiguracionID = null;
                celda.ColumnasModeloDatoID = null;

                cPlantillasCeldas.UpdateItem(celda);


                direct.Success = true;
                direct.Result = GetModeloPlantilla().Result;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }
        #endregion

        #region Historico

        #region DescargarHistorico
        [DirectMethod()]
        public void DescargarHistorico(long historicoID)
        {
            CoreExportacionDatosPlantillasHistoricosController cCoreExportacionDatosPlantillasHistoricos = new CoreExportacionDatosPlantillasHistoricosController();

            try
            {
                CoreExportacionDatosPlantillasHistoricos historico = cCoreExportacionDatosPlantillasHistoricos.GetItem(historicoID);
                if (historico != null)
                {

                    string path = Path.Combine(DirectoryMapping.GetExportDirectory(), historico.Archivo);

                    FileInfo file = new FileInfo(path);
                    string nombreArchivo = historico.Archivo;
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ContentType = Comun.GetMimeType("." + historico.Archivo.Split('.').Last());
                    
                    Response.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo);
                    
                    HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
                    HttpContext.Current.Response.TransmitFile(path);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        #endregion

        #region DesactivarHistorico
        [DirectMethod()]
        public DirectResponse DesactivarHistorico(long historicoID)
        {
            DirectResponse direct = new DirectResponse();

            CoreExportacionDatosPlantillasHistoricosController cCoreExportacionDatosPlantillasHistoricos = new CoreExportacionDatosPlantillasHistoricosController();

            try
            {
                CoreExportacionDatosPlantillasHistoricos historico = cCoreExportacionDatosPlantillasHistoricos.GetItem(historicoID);
                if (historico != null)
                {
                    historico.Activo = false;
                    direct.Success = cCoreExportacionDatosPlantillasHistoricos.UpdateItem(historico);
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
        #endregion

        #endregion

        #endregion

        #region FUNCTIONS

        #region CrearNuevacolumna
        void CrearNuevacolumna(Vw_CoreExportacionDatosPlantillas Plantilla)
        {
            CoreExportacionDatosPlantillasFilasController cCoreExportacionDatosPlantillasFilas = new CoreExportacionDatosPlantillasFilasController();
            CoreExportacionDatosPlantillasColumnasController cCoreExportacionDatosPlantillasColumnas = new CoreExportacionDatosPlantillasColumnasController();
            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();

            try
            {
                CoreExportacionDatosPlantillasFilas fila = new CoreExportacionDatosPlantillasFilas()
                {
                    CoreExportacionDatosPlantillaID = Plantilla.CoreExportacionDatoPlantillaID
                };

                fila = cCoreExportacionDatosPlantillasFilas.AddItem(fila);

                CoreExportacionDatosPlantillasColumnas columna = new CoreExportacionDatosPlantillasColumnas()
                {
                    Nombre = GetGlobalResource(Comun.jsNombreColumna)
                };

                columna = cCoreExportacionDatosPlantillasColumnas.AddItem(columna);


                if (fila != null && columna != null)
                {

                    CoreExportacionDatosPlantillasCeldas celda = new CoreExportacionDatosPlantillasCeldas()
                    {
                        CoreExportacionDatosPlantillaColumnaID = columna.CoreExportacionDatosPlantillaColumnaID,
                        CoreExportacionDatosPlantillaFilaID = fila.CoreExportacionDatosPlantillaFilaID
                    };

                    cCoreExportacionDatosPlantillasCeldas.AddItem(celda);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }
        #endregion

        #region EliminarCeldasByFila
        public void EliminarCeldasByFila(long FilaID, TreeCoreContext context)
        {
            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            cCoreExportacionDatosPlantillasCeldas.SetDataContext(context);


            try
            {
                cCoreExportacionDatosPlantillasCeldas.DeleteByFila(FilaID);


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        #endregion

        #region ResetCeldasByFila
        public void ResetCeldasByFila(long FilaID, TreeCoreContext context)
        {
            CoreExportacionDatosPlantillasCeldasController cCoreExportacionDatosPlantillasCeldas = new CoreExportacionDatosPlantillasCeldasController();
            cCoreExportacionDatosPlantillasCeldas.SetDataContext(context);


            try
            {
                cCoreExportacionDatosPlantillasCeldas.ResetByFila(FilaID);


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        #endregion

        #region GetTiposDinamicos
        public List<TipoDinamico> GetTiposDinamicos(long columnaModeloDatoID)
        {
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();

            List<TipoDinamico> tiposDinamicos;

            try
            {
                string sNombreTabla = cColumnas.getDataSourceTablaColumna(columnaModeloDatoID);
                string sControlador = cColumnas.getControllerColumna(columnaModeloDatoID);
                string sIndice = cColumnas.getIndiceColumna(columnaModeloDatoID);
                string sColumna = cColumnas.getNombreColumnaByTabla(columnaModeloDatoID);

                Type tipo = Type.GetType("TreeCore.Data." + sNombreTabla.Split('.')[1]);
                Type tipoControlador = Type.GetType("CapaNegocio." + sControlador);

                if (tipo == null)
                {
                    tipo = Type.GetType("TreeCore.Data.Vw_" + sNombreTabla.Split('_')[1]);
                }

                if (tipoControlador.BaseType.GenericTypeArguments[0].FullName != tipo.FullName)
                {
                    tipo = Type.GetType(tipoControlador.BaseType.GenericTypeArguments[0].FullName);
                }

                var instance = Activator.CreateInstance(tipoControlador);
                Type[] tipos = new Type[1];
                tipos[0] = typeof(long);
                MethodInfo method = tipoControlador.GetMethod("GetActivos", tipos);
                dynamic list = method.Invoke(instance, new object[] { ClienteID });
                tiposDinamicos = new List<TipoDinamico>();

                foreach (Object obj in list)
                {
                    if (sColumna != null && sIndice != null)
                    {
                        PropertyInfo propID = tipo.GetProperty(sIndice);
                        PropertyInfo propName = tipo.GetProperty(sColumna);

                        if (propID != null && propName != null)
                        {
                            long lID = (long)propID.GetValue(obj, null);
                            string sName = (string)propName.GetValue(obj, null);

                            tiposDinamicos.Add(new TipoDinamico(sName, lID));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                tiposDinamicos = new List<TipoDinamico>();
            }

            return tiposDinamicos;
        }
        #endregion

        #region limpiarGridPreview
        private void limpiarGridPreview()
        {
            for (int i = 0; i < storegrdPreview.ModelInstance.Fields.Count; i++)
            {
                storegrdPreview.ModelInstance.Fields.RemoveAt(i);
            }

            grdPreview.RemoveAllColumns();

        }
        #endregion

        #region updateTiposDatosOperadores()
        private void updateTiposDatosOperadores()
        {
            TiposDatosOperadoresController cTiposDatosOperadores = new TiposDatosOperadoresController();
            CoreExportacionDatosPlantillasReglasController cCoreExportacionDatosPlantillasReglas = new CoreExportacionDatosPlantillasReglasController();

            try
            {
                List<TiposDatosOperadores> operadores = cTiposDatosOperadores.GetActivos(ClienteID.Value);
                List<CoreExportacionDatosPlantillasReglas> reglas = cCoreExportacionDatosPlantillasReglas.GetActivos(ClienteID.Value);

                List<long> idsOperadoresInReglas = new List<long>();

                reglas.ForEach(re =>
                {
                    if (!idsOperadoresInReglas.Contains(re.TipoDatoOperadorID))
                    {
                        idsOperadoresInReglas.Add(re.TipoDatoOperadorID);
                    }
                });

                operadores.ForEach(op =>
                {
                    if (!idsOperadoresInReglas.Contains(op.TipoDatoOperadorID))
                    {
                        cCoreExportacionDatosPlantillasReglas.AddItem(new CoreExportacionDatosPlantillasReglas()
                        {
                            TipoDatoOperadorID = op.TipoDatoOperadorID
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        #endregion

        #region EliminarTransformacionSiCambiaAtributo
        public bool EliminarTransformacionSiCambiaAtributo(TreeCoreContext context, CoreExportacionDatosPlantillasCeldas celda, long value)
        {
            bool result = true;

            CoreExportacionDatosPlantillasReglasCeldasController cPlantillasReglasCeldas = new CoreExportacionDatosPlantillasReglasCeldasController();
            cPlantillasReglasCeldas.SetDataContext(context);

            try
            {
                if ((celda.ColumnasModeloDatoID.HasValue && celda.ColumnasModeloDatoID.Value != value) ||
                (celda.CoreAtributosConfiguracionID.HasValue && celda.CoreAtributosConfiguracionID.Value != value))
                {
                    List<Vw_CoreExportacionDatosPlantillasReglasCeldas> transformaciones = cPlantillasReglasCeldas.GetByCeldaID(celda.CoreExportacionDatosPlantillasCeldasID);

                    foreach (var t in transformaciones)
                    {
                        if (!cPlantillasReglasCeldas.DeleteItem(t.CoreExportacionDatosPlantillasReglaCeldaID))
                        {
                            result = false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                result = false;
                log.Error(ex);
            }

            return result;
        }
        #endregion

        #endregion

    }

    #region TipoDinamico
    public class TipoDinamico
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public TipoDinamico(string name, long id)
        {
            this.Name = name;
            this.Id = id;
        }
    }
    #endregion

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

}