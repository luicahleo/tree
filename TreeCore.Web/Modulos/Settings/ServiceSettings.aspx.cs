using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Reflection;
using System.Data.SqlClient;
using System.Transactions;
using Newtonsoft.Json;
using TreeCore.CapaNegocio.Global.Administracion;
using TreeCore.Data;

namespace TreeCore.ModGlobal
{
    public partial class ServiceSettings : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                Comun.CreateGridFilters(gridFiltersProyectosTipos, storeProyectosTipos, gridProyectosTipos.ColumnModel, listaIgnore, _Locale);
                Comun.CreateGridFilters(gridFiltersProyectoTipoLibre, storeProyectosTiposLibres, gridProyectosTiposLibres.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grid, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
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
            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<Data.CoreServiceSettings> listaDatos = new List<Data.CoreServiceSettings>();
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
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

        /*protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
            btnEditar.Hidden = false;
            btnAnadir.Enabled = false;
             if (!IsPostBack && !RequestManager.IsAjaxRequest)
             {
                 List<Data.Vw_Funcionalidades> listFun = ((List<Data.Vw_Funcionalidades>)(this.Session["LISTAFUNCIONALIDADES"]));
                 List<TreeCore.Data.Modulos> listaMod = ((List<TreeCore.Data.Modulos>)this.Session["LISTAMODULOS"]);

                 btnAnadir.Hidden = true;
                 btnEditar.Hidden = true;
                 btnEliminar.Hidden = true;
                 btnRefrescar.Hidden = false;
                 btnActivar.Hidden = true;
                 btnDefecto.Hidden = true;
                 btnBloquear.Hidden = true;
                 btnDescargar.Hidden = true;
                 btnEditar.Hidden = false;
                 if (Comun.ComprobarFuncionalidadSoloLectura(System.IO.Path.GetFileName(Request.Url.AbsolutePath), listFun, listaMod))
                 {
                     btnAnadir.Hidden = true;
                     btnEditar.Hidden = true;
                     btnEliminar.Hidden = true;
                     btnRefrescar.Hidden = false;
                     btnActivar.Hidden = true;
                     btnDefecto.Hidden = true;
                     btnBloquear.Hidden = true;
                     btnDescargar.Hidden = true;
                 }
                 else if (Comun.ComprobarFuncionalidadTotal(System.IO.Path.GetFileName(Request.Url.AbsolutePath), listFun, listaMod))
                 {
                     btnAnadir.Hidden = false;
                     btnEditar.Hidden = false;
                     btnEliminar.Hidden = false;
                     btnRefrescar.Hidden = false;
                     btnActivar.Hidden = false;
                     btnDefecto.Hidden = false;
                     btnBloquear.Hidden = false;
                     btnDescargar.Hidden = false;
                 }
                 if (Comun.ComprobarFuncionalidadDescarga(System.IO.Path.GetFileName(Request.Url.AbsolutePath), listFun, listaMod))
                 {
                     btnDescargar.Hidden = false;
                 }
                 btnEditar.Hidden = false;
             }
    } */

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = "GlobalSettingsContenedor.aspx";
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> { }},
                { "Post", new List<ComponentBase> { btnAnadir }},
                { "Put", new List<ComponentBase> { btnEditar }},
                { "Delete", new List<ComponentBase> { }}
            };
        }

        #endregion

        #region STORES

        #region STORE PRINCIPAL

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];



                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()));

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

        private List<Data.CoreServiceSettings> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.CoreServiceSettings> listaDatos;
            CoreServiceSettingsController CEstadosGlobales = new CoreServiceSettingsController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CEstadosGlobales.GetItemsWithExtNetFilterList<Data.CoreServiceSettings>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
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
                    List<Data.Clientes> listaClientes = new List<Data.Clientes>();

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
            List<Data.Clientes> listaDatos = new List<Data.Clientes>();
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

        #region PROYECTOS TIPOS
        protected void storeProyectosTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                EstadosGlobalesBloqueadosController cBloqueados = new EstadosGlobalesBloqueadosController();
                List<Data.Vw_EstadosGlobalesBloqueados> lista;
                try
                {
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];

                    lista = cBloqueados.proyectosTiposAsignados(Int64.Parse(GridRowSelect.SelectedRecordID));
                    lista = Clases.LinqEngine.PagingItemsListWithExtNetFilter(lista, sFiltro, "", sSort, sDir, e.Start, e.Limit, ref iCount);

                    if (lista != null)
                    {
                        storeProyectosTipos.DataSource = lista;
                        PageProxy temp = (PageProxy)storeProyectosTipos.Proxy[0];
                        temp.Total = iCount;

                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }
        #endregion

        #region PROYECTOS TIPOS LIBRES

        /// <summary>
        /// Cargar los ProyectosTipos Sin asignar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void storeProyectosTiposLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                EstadosGlobalesBloqueadosController cBloqueados = new EstadosGlobalesBloqueadosController();
                List<Data.ProyectosTipos> lista;
                try
                {
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];
                    lista = cBloqueados.proyectosTiposNoAsignado(Int64.Parse(GridRowSelect.SelectedRecordID));
                    lista = Clases.LinqEngine.PagingItemsListWithExtNetFilter(lista, sFiltro, "", sSort, sDir, e.Start, e.Limit, ref iCount);

                    if (lista != null)
                    {
                        storeProyectosTiposLibres.DataSource = lista;
                        PageProxy temp = (PageProxy)storeProyectosTiposLibres.Proxy[0];
                        temp.Total = iCount;

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

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)

        {
            DirectResponse direct = new DirectResponse();

            CoreServiceSettingsController cCoreServiceSettings = new CoreServiceSettingsController();
            long cliID = 0;

            try

            {
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.CoreServiceSettings dato = cCoreServiceSettings.GetItem(S);

                    dato.HoraEjecucion = Convert.ToInt32(txtHoraEjecucion.Text);
                    dato.MinutoEjecucion = Convert.ToInt32(txtMinutoEjecucion.Text);
                    cCoreServiceSettings.UpdateItem(dato);
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

            CoreServiceSettingsController cCoreServiceSettings = new CoreServiceSettingsController();

            try

            {
                long S = Convert.ToInt64(GridRowSelect.SelectedRecordID);

                Data.CoreServiceSettings dato = cCoreServiceSettings.GetItem(S);
                txtHoraEjecucion.Text = dato.HoraEjecucion.ToString();

                txtMinutoEjecucion.Text = dato.MinutoEjecucion.ToString();
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
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            EstadosGlobalesController CEstadosGlobales = new EstadosGlobalesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.EstadosGlobales oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = CEstadosGlobales.GetDefault(long.Parse(hdCliID.Value.ToString()));

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.EstadoGlobalID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            CEstadosGlobales.UpdateItem(oDato);
                        }

                        oDato = CEstadosGlobales.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        CEstadosGlobales.UpdateItem(oDato);
                    }
                    else
                    {
                        oDato.Defecto = !oDato.Defecto;
                        CEstadosGlobales.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = CEstadosGlobales.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    CEstadosGlobales.UpdateItem(oDato);
                }

                log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));

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
            EstadosGlobalesController CEstadosGlobales = new EstadosGlobalesController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                //if (CEstadosGlobales.RegistroDefecto(lID))
                //{
                //    direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                //    direct.Success = false;
                //}
                //else if (CEstadosGlobales.DeleteItem(lID))
                //{
                //    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                //    direct.Success = true;
                //    direct.Result = "";
                //}

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
            EstadosGlobalesController cController = new EstadosGlobalesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.EstadosGlobales oDato = cController.GetItem(lID);
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


        [DirectMethod]
        public DirectResponse AgregarProyectosTipos()
        {
            DirectResponse direct = new DirectResponse();
            EstadosGlobalesBloqueadosController cBloqueados = new EstadosGlobalesBloqueadosController();


            try
            {
                //Registro de Estadistica
                EstadisticasController cEstadisticas = new EstadisticasController();
                ProyectosTiposController cProyTip = new ProyectosTiposController();
                Data.ProyectosTipos ptip = cProyTip.GetProyectosTiposByNombre(Comun.MODULOGLOBAL);
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ptip.ProyectoTipoID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticasAgregarProyectoTipo), Comun.MODIFICAR_PROYECTOS_TIPOS);

                foreach (SelectedRow selec in GridRowSelectProyectoTipoLibre.SelectedRows)
                {
                    Data.EstadosGlobalesBloqueados dato = new Data.EstadosGlobalesBloqueados();
                    dato.ProyectoTipoID = Int64.Parse(selec.RecordID);
                    dato.EstadoGlobalID = Int64.Parse(GridRowSelect.SelectedRecordID);
                    dato = cBloqueados.AddItem(dato);

                }


                cBloqueados = null;
                storeProyectosTipos.DataBind();
                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            return direct;
        }

        /// <summary>
        /// Quitar ProyectosTipos A un Usuario
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public DirectResponse QuitarProyectosTipos()
        {
            DirectResponse direct = new DirectResponse();
            EstadosGlobalesBloqueadosController cBloqueados = new EstadosGlobalesBloqueadosController();

            try
            {
                //Registro de Estadistica
                EstadisticasController cEstadisticas = new EstadisticasController();
                ProyectosTiposController cProyTip = new ProyectosTiposController();
                Data.ProyectosTipos ptip = cProyTip.GetProyectosTiposByNombre(Comun.MODULOGLOBAL);
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ptip.ProyectoTipoID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticasAgregarProyectoTipo), Comun.MODIFICAR_PROYECTOS_TIPOS);

                foreach (SelectedRow selec in GridRowSelectProyectosTipos.SelectedRows)
                {
                    try
                    {
                        cBloqueados.DeleteItem(Int64.Parse(selec.RecordID));
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }
                }


                cBloqueados = null;
                storeProyectosTipos.DataBind();
                direct.Success = true;
                direct.Result = "";
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
        public DirectResponse AgregarServicio()
        {
            DirectResponse direct = new DirectResponse();
            CoreServiceSettingsController cCoreServiceSettings = new CoreServiceSettingsController();
            Data.CoreServiceSettings dato;
            long lCliID = 0;
            long servicio;
            long servicioAsignado;
            long producCatalog;
            double precio;
            try
            {


            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }
            direct.Success = true;
            direct.Result = ""; return direct;
        }



        #endregion

    }
}