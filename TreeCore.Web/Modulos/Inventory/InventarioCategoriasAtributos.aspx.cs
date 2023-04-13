using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using System.Data.SqlClient;
using log4net;
using System.Reflection;
using System.Transactions;
using System.Linq;
using TreeCore.Clases;

namespace TreeCore.ModInventario
{
    public partial class InventarioCategoriasAtributos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        public List<Componentes.CategoriasAtributos> listaCategorias;
        protected bool SoloLectura = false;

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {

                listaCategorias = new List<Componentes.CategoriasAtributos>();


                ResourceManagerOperaciones(ResourceManagerTreeCore);

                //#region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                //Comun.CreateGridFilters(gridFilter, storePrincipal, GridPanelCategorias.ColumnModel, listaIgnore, _Locale);
                //log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                //#endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(GridPanelCategorias, storePrincipal, GridPanelCategorias.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                    cmbClientes.Hidden = false;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }

                switch (Comun.GetRestriccionDefectoInventario())
                {
                    case (long)Comun.RestriccionesAtributoDefecto.ACTIVE:
                        btnRestriccionActive.Disable();
                        break;
                    case (long)Comun.RestriccionesAtributoDefecto.DISABLED:
                        btnRestriccionDisabled.Disable();
                        break;
                    case (long)Comun.RestriccionesAtributoDefecto.HIDDEN:
                        btnRestriccionHidden.Disable();
                        break;
                    default:
                        break;
                }
            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<Data.InventarioAtributosCategorias> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        listaDatos = ListaPrincipal(CliID, true, true, true, true);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(GridPanelCategorias.ColumnModel, listaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                            log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            EstadisticasController cEstadisticas = new EstadisticasController();
                            cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.INVENTARIO), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
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
            SoloLectura = true;

            sPagina = "InventarioCategoriasContenedor.aspx";
            List<string> listFun = ((List<string>)(this.Session["FUNTIONALITIES"]));
            var UserInterface = ModulesController.GetUserInterfaces().FirstOrDefault(x => x.Page.ToLower() == sPagina.ToLower());
            var listFunPag = listFun.Where(x => $"{x.Split('@')[0]}" == UserInterface.Code);

            if (listFunPag.Where(x => x.Split('@')[1] == "Put").ToList().Count > 0)
            {
                SoloLectura = false;
            }
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { }},
            { "Post", new List<ComponentBase> { btnAnadir }},
            { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnRestriccionActive, btnRestriccionDisabled, btnRestriccionHidden }},
            { "Delete", new List<ComponentBase> { btnEliminar }}
            };

            PintarCategorias(false);
            AtributosConfiguracion.TipoAtributo = Comun.MODULOINVENTARIO;
        }
        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);            
            AtributosConfiguracion.TipoAtributo = Comun.MODULOINVENTARIO;
        }

        #endregion

        #region STORES

        #region PRINCIPAL

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var vLista = ListaPrincipal(long.Parse(hdCliID.Value.ToString()), btnActivos.Pressed, true, true, true);

                    if (vLista != null)
                    {
                        storePrincipal.DataSource = vLista;
                        storePrincipal.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.InventarioAtributosCategorias> ListaPrincipal(long lClienteID, bool Activo, bool bSecc, bool bSubc, bool bSubcPla)
        {
            List<string> listaValoresSeleccionados;
            List<Data.InventarioAtributosCategorias> listaDatos;
            InventarioAtributosCategoriasController cInventarioAtributosCategorias = new InventarioAtributosCategoriasController();

            try
            {
                if (lClienteID != null)
                {
                    if (cmbTiposVinculaciones.SelectedItems.Count > 0)
                    {
                        listaValoresSeleccionados = new List<string>();
                        foreach (var item in cmbTiposVinculaciones.SelectedItems)
                        {
                            listaValoresSeleccionados.Add(item.Value);
                        }
                        listaDatos = cInventarioAtributosCategorias.getCategoriasFiltradas((long)lClienteID, Activo,
                            listaValoresSeleccionados.Contains("0"),
                            listaValoresSeleccionados.Contains("1"),
                            listaValoresSeleccionados.Contains("2"));
                    }
                    else
                    {
                        listaDatos = cInventarioAtributosCategorias.getCategoriasFiltradas((long)lClienteID, Activo, bSecc, bSubc, bSubcPla);
                    }
                }
                else
                {
                    listaDatos = null;
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        #endregion

        #region CLIENTES

        protected void storeClientes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Clientes> listaClientes;

                    listaClientes = ListaClientes();

                    if (listaClientes != null)
                    {
                        storeClientes.DataSource = listaClientes;
                    }
                    if (ClienteID.HasValue)
                    {
                        cmbClientes.SelectedItem.Value = ClienteID.Value.ToString();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.Clientes> ListaClientes()
        {
            List<Data.Clientes> listaDatos;
            ClientesController cClientes = new ClientesController();

            try
            {
                listaDatos = cClientes.GetActivos();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse CambiarRestriccionDefecto(string sBtn)
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                switch (sBtn)
                {
                    case "Active":
                        Comun.SetRestriccionDefectoInventario((long)Comun.RestriccionesAtributoDefecto.ACTIVE);
                        break;
                    case "Disabled":
                        Comun.SetRestriccionDefectoInventario((long)Comun.RestriccionesAtributoDefecto.DISABLED);
                        break;
                    case "Hidden":
                        Comun.SetRestriccionDefectoInventario((long)Comun.RestriccionesAtributoDefecto.HIDDEN);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)

        {
            DirectResponse direct = new DirectResponse();
            long lCliID = 0;
            InventarioCategoriasController cCat = new InventarioCategoriasController();
            InventarioAtributosCategoriasController cCategoria = new InventarioAtributosCategoriasController();
            cCat.SetDataContext(cCategoria.Context);

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.InventarioAtributosCategorias oDato;

                    oDato = cCategoria.GetItem(lS);

                    if (oDato.InventarioAtributoCategoria == txtInventarioCategoria.Text)
                    {
                        oDato.InventarioAtributoCategoria = txtInventarioCategoria.Text;
                    }
                    else
                    {
                        if (ClienteID.HasValue)
                        {
                            lCliID = long.Parse(hdCliID.Value.ToString());
                        }

                        if (cCategoria.RegistroDuplicado(txtInventarioCategoria.Text, lCliID) || cCat.CodigoDuplicado(txtInventarioCategoria.Text, null, lCliID))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            return direct;
                        }
                        else
                        {
                            if (cCategoria.NombreDuplicadoAtributos(txtInventarioCategoria.Text, oDato.InventarioAtributoCategoriaID))
                            {
                                direct.Success = false;
                                direct.Result = GetGlobalResource("strNombreDuplicadoAtributo");
                                return direct;
                            }
                            else
                            {
                                oDato.InventarioAtributoCategoria = txtInventarioCategoria.Text;
                                if (cCategoria.UpdateItem(oDato))
                                {
                                    log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                                    storePrincipal.DataBind();
                                }
                            }
                        }
                    }


                }
                else
                {
                    if (ClienteID.HasValue)
                    {
                        lCliID = long.Parse(hdCliID.Value.ToString());
                    }

                    if (cCategoria.RegistroDuplicado(txtInventarioCategoria.Text, lCliID) || cCat.CodigoDuplicado(txtInventarioCategoria.Text, null, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.InventarioAtributosCategorias oDato = new Data.InventarioAtributosCategorias();
                        oDato.InventarioAtributoCategoria = txtInventarioCategoria.Text;
                        oDato.Activo = true;
                        if (btnSeccion.Pressed)
                        {
                            oDato.EsSubcategoria = false;
                            oDato.EsPlantilla = false;
                        }
                        else if (btnSubcategoria.Pressed)
                        {
                            oDato.EsSubcategoria = true;
                            oDato.EsPlantilla = false;
                        }
                        else if (btnSubcategoriaPlantilla.Pressed)
                        {
                            oDato.EsSubcategoria = true;
                            oDato.EsPlantilla = true;
                        }

                        if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                        {
                            oDato.ClienteID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.ClienteID = lCliID;
                        }

                        using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
                        {
                            try
                            {
                                if ((oDato = cCategoria.AddItem(oDato)) != null)
                                {
                                    storePrincipal.DataBind();

                                    if (oDato.EsSubcategoria)
                                    {
                                        CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCatConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
                                        cCatConf.SetDataContext(cCategoria.Context);
                                        Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones oCatConf = new Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones
                                        {
                                            InventarioAtributoCategoriaID = oDato.InventarioAtributoCategoriaID,
                                            InventarioCategoriaID = null
                                        };
                                        if ((oCatConf = cCatConf.AddItem(oCatConf)) != null)
                                        {
                                            //CoreInventarioCategoriasAtributosCategoriasController cCatVin = new CoreInventarioCategoriasAtributosCategoriasController();
                                            //cCatVin.SetDataContext(cCategoria.Context);
                                            //Data.CoreInventarioCategoriasAtributosCategorias oCatVin = new Data.CoreInventarioCategoriasAtributosCategorias
                                            //{
                                            //    CoreInventarioCategoriaAtributoCategoriaConfiguracionID = oCatConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID,
                                            //    InventarioCategoriaID = null,
                                            //    Orden = 0
                                            //};
                                            //if (cCatVin.AddItem(oCatVin) != null)
                                            //{
                                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                            trans.Complete();
                                            //}
                                            //else
                                            //{
                                            //    trans.Dispose();
                                            //    direct.Success = false;
                                            //    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                            //    return direct;
                                            //}
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
                                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                        trans.Complete();
                                    }
                                }
                                else
                                {
                                    trans.Dispose();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                    return direct;
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
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            InventarioAtributosCategoriasController cCategorias = new InventarioAtributosCategoriasController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.InventarioAtributosCategorias oDato;
                oDato = cCategorias.GetItem(lS);

                txtInventarioCategoria.Text = oDato.InventarioAtributoCategoria;
                btnSeccion.SetPressed(false);
                btnSubcategoria.SetPressed(false);
                btnSubcategoriaPlantilla.SetPressed(false);
                if (!oDato.EsSubcategoria)
                {
                    btnSeccion.SetPressed(true);
                }
                else if (!oDato.EsPlantilla)
                {
                    btnSubcategoria.SetPressed(true);
                }
                else
                {
                    btnSubcategoriaPlantilla.SetPressed(true);
                }
                winGestion.Show();

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
            InventarioAtributosCategoriasController cInventarioAtributosCategorias = new InventarioAtributosCategoriasController();
            Data.InventarioAtributosCategorias oInventarioAtributosCategorias;
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCatConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            cCatConf.SetDataContext(cInventarioAtributosCategorias.Context);
            Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones oCatConf;
            CoreInventarioCategoriasAtributosCategoriasController cCatVin = new CoreInventarioCategoriasAtributosCategoriasController();
            cCatVin.SetDataContext(cInventarioAtributosCategorias.Context);
            Data.CoreInventarioCategoriasAtributosCategorias oCatVin;

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                oInventarioAtributosCategorias = cInventarioAtributosCategorias.GetItem(lID);
                if (!oInventarioAtributosCategorias.EsSubcategoria)
                {
                    if (cInventarioAtributosCategorias.DeleteItem(lID))
                    {
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                        direct.Success = true;
                        direct.Result = "";
                    }
                }
                else
                {
                    using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
                    {
                        try
                        {
                            oCatConf = cCatConf.GetPlantilla(lID);
                            //oCatVin = cCatVin.GetPlantilla(lID);
                            //if (cCatVin.DeleteItem(oCatVin.CoreInventarioCategoriaAtributoCategoriaID))
                            //{
                            if (cCatConf.DeleteItem(oCatConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID))
                            {
                                if (cInventarioAtributosCategorias.DeleteItem(lID))
                                {
                                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                                    direct.Success = true;
                                    direct.Result = "";
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
                            //}
                            //else
                            //{
                            //    trans.Dispose();
                            //    direct.Success = false;
                            //    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            //    return direct;
                            //}

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
                }

                cInventarioAtributosCategorias = null;
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
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            InventarioAtributosCategoriasController cController = new InventarioAtributosCategoriasController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.InventarioAtributosCategorias oDato;
                oDato = cController.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cController.UpdateItem(oDato))
                {
                    storePrincipal.DataBind();
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

        [DirectMethod()]
        public DirectResponse SeleccionarCategoriaPlantilla()
        {
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCategorias = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                long cliID = long.Parse(hdCliID.Value.ToString());
                Componentes.CategoriasAtributos compCatAtri;
                hdListaCategorias.SetValue("");
                if (listaCategorias == null)
                {
                    listaCategorias = new List<Componentes.CategoriasAtributos>();
                }
                else
                {
                    listaCategorias.Clear();
                }
                long lID = long.Parse(hdCatSelect.Value.ToString());
                Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones item = cCategorias.GetPlantilla(lID);
                compCatAtri = (Componentes.CategoriasAtributos)this.LoadControl("../../Componentes/CategoriasAtributos.ascx");
                compCatAtri.ID = "CAT" + item.CoreInventarioCategoriaAtributoCategoriaConfiguracionID.ToString();
                compCatAtri.CategoriaAtributoID = item.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                compCatAtri.CategoriaAtributoAsignacionID = item.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                compCatAtri.Nombre = item.InventarioAtributosCategorias.InventarioAtributoCategoria;
                compCatAtri.Orden = 0;
                compCatAtri.Modulo = (long)Comun.Modulos.INVENTARIO;
                compCatAtri.ActivarBotonEliminarCategoria = false;
                compCatAtri.EsSoloLectura = this.SoloLectura;
                if (!item.InventarioAtributosCategorias.EsSubcategoria)
                {
                    compCatAtri.TipoCategoria = "Seccion";
                }
                else if (!item.InventarioAtributosCategorias.EsPlantilla)
                {
                    compCatAtri.TipoCategoria = "Subcategoria";
                }
                else
                {
                    compCatAtri.TipoCategoria = "SubcategoriaPlantilla";
                }
                listaCategorias.Add(compCatAtri);
                PintarCategorias(true);
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }


        [DirectMethod(Timeout = 120000)]
        public DirectResponse PintarCategorias(bool Update)
        {
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCategoriasVin = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            Componentes.CategoriasAtributos oComponente;
            Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones oDato;
            List<Ext.Net.AbstractComponent> listaBotonAñadirCategorias;
            long cliID = long.Parse(hdCliID.Value.ToString());
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            if (listaCategorias == null)
            {
                try
                {
                    listaCategorias = new List<Componentes.CategoriasAtributos>();
                    if (hdListaCategorias.Value.ToString() != "")
                    {
                        foreach (var idCate in hdListaCategorias.Value.ToString().Split(','))
                        {
                            oDato = cCategoriasVin.GetItem(long.Parse(idCate));
                            if (oDato != null)
                            {
                                oComponente = (Componentes.CategoriasAtributos)this.LoadControl("../../Componentes/CategoriasAtributos.ascx");
                                oComponente.ID = "CAT" + oDato.CoreInventarioCategoriaAtributoCategoriaConfiguracionID.ToString();
                                oComponente.CategoriaAtributoID = oDato.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                                oComponente.CategoriaAtributoAsignacionID = oDato.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                                oComponente.Nombre = oDato.InventarioAtributosCategorias.InventarioAtributoCategoria;
                                oComponente.Orden = 0;
                                oComponente.Modulo = (long)Comun.Modulos.INVENTARIO;
                                oComponente.ActivarBotonEliminarCategoria = false;
                                oComponente.EsSoloLectura = this.SoloLectura;
                                if (!oDato.InventarioAtributosCategorias.EsSubcategoria)
                                {
                                    oComponente.TipoCategoria = "Seccion";
                                }
                                else if (!oDato.InventarioAtributosCategorias.EsPlantilla)
                                {
                                    oComponente.TipoCategoria = "Subcategoria";
                                }
                                else
                                {
                                    oComponente.TipoCategoria = "SubcategoriaPlantilla";
                                }
                                listaCategorias.Add(oComponente);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                }
            }
            try
            {

                listaCategorias = listaCategorias.OrderBy(it => it.Orden).ToList();

                foreach (var item in listaCategorias)
                {
                    pnConfigurador.ContentControls.Add(item);
                }
                if (Update)
                {
                    pnConfigurador.UpdateContent();

                    string categorias = "";
                    foreach (var item in listaCategorias)
                    {
                        item.PintarAtributos(true);
                        if (categorias != "")
                            categorias += ",";
                        categorias += item.CategoriaAtributoID.ToString();
                    }
                    hdListaCategorias.SetValue(categorias);
                }


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }
            return direct;
        }

        #endregion

    }
}