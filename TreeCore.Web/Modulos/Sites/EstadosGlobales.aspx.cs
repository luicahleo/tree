using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using TreeCore.CapaNegocio.Global.Administracion;
using TreeCore.Clases;

namespace TreeCore.ModGlobal
{
    public partial class EstadosGlobales : TreeCore.Page.BasePageExtNet
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
                        List<Data.EstadosGlobales> listaDatos = new List<Data.EstadosGlobales>();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storeImagenes = this.cmbImagen.GetStore();
            this.storeImagenes.DataSource = new object[]
            {
                new object[] { "ico-blocked.svg", (String)GetLocalResourceObject("jsBloquear"), "ico-blocked" },
                new object[] { "ico-onAir.svg", (String)GetLocalResourceObject("jsEnAire"), "ico-onAir" },
                new object[] { "ico-damaged.svg", (String)GetLocalResourceObject("jsAveriado"), "ico-damaged" },
                new object[] { "ico-outofservice.svg", (String)GetLocalResourceObject("jsFueraServicio"), "ico-outofservice" },
                new object[] { "ico-trash.svg", (String)GetLocalResourceObject("jsDesinstalar"), "ico-trash" },
            };
            this.storeImagenes.DataBind();

            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { btnAnadir }},
            { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnDefecto, btnBloquear }},
            { "Delete", new List<ComponentBase> { btnEliminar }}
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

        private List<Data.EstadosGlobales> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.EstadosGlobales> listaDatos;
            EstadosGlobalesController CEstadosGlobales = new EstadosGlobalesController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CEstadosGlobales.GetItemsWithExtNetFilterList<Data.EstadosGlobales>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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
        public DirectResponse AgregarEditar(bool bAgregar)

        {
            DirectResponse direct = new DirectResponse();
            EstadosGlobalesController cEstadosGlobales = new EstadosGlobalesController();
            long lCliID = 0;
            InfoResponse oResponse;

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.EstadosGlobales oDato;
                    oDato = cEstadosGlobales.GetItem(lS);
                    oDato.EstadoGlobal = txtEstadoGlobal.Text;
                    oDato.Imagen = cmbImagen.SelectedItem.Value.ToString();
                    oDato.Activo = chkActivo.Checked;
                    oDato.Visible = chkVisible.Checked;
                    oDato.Analytics = chkAnalytics.Checked;
                    oDato.Bloqueante = chkBloqueante.Checked;
                    oDato.Desactivo = chkDesactivo.Checked;
                    oDato.Desinstalado = chkDesinstalado.Checked;

                    oResponse = cEstadosGlobales.UpdateEstado(oDato);
                    if (oResponse.Result)
                    {
                        oResponse = cEstadosGlobales.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cEstadosGlobales.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cEstadosGlobales.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());
                    Data.EstadosGlobales oDato = new Data.EstadosGlobales();
                    oDato.EstadoGlobal = txtEstadoGlobal.Text;
                    oDato.Imagen = cmbImagen.SelectedItem.Value.ToString();
                    oDato.Activo = chkActivo.Checked;
                    oDato.Visible = chkVisible.Checked;
                    oDato.Analytics = chkAnalytics.Checked;
                    oDato.Bloqueante = chkBloqueante.Checked;
                    oDato.Desactivo = chkDesactivo.Checked;
                    oDato.Desinstalado = chkDesinstalado.Checked;
                    oDato.ClienteID = lCliID;

                    oResponse = cEstadosGlobales.AddEstado(oDato);
                    if (oResponse.Result)
                    {
                        oResponse = cEstadosGlobales.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cEstadosGlobales.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cEstadosGlobales.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
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
            
            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();

            EstadosGlobalesController cEstadosGlobales = new EstadosGlobalesController();

            try

            {
                long S = Convert.ToInt64(GridRowSelect.SelectedRecordID);

                Data.EstadosGlobales dato = cEstadosGlobales.GetItem(S);
                txtEstadoGlobal.Text = dato.EstadoGlobal;
                cmbImagen.Value = dato.Imagen;

                if (dato.Activo == false || dato.Activo == null)
                    chkActivo.Checked = false;
                else
                    chkActivo.Checked = true;
                if (dato.Visible == false || dato.Visible == null)
                    chkVisible.Checked = false;
                else
                    chkVisible.Checked = true;
                if (dato.Analytics == false || dato.Analytics == null)
                    chkAnalytics.Checked = false;
                else
                    chkAnalytics.Checked = true;

                chkDesactivo.Checked = dato.Desactivo;
                chkDesinstalado.Checked = dato.Desinstalado;
                chkBloqueante.Checked = dato.Bloqueante;

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
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.EstadosGlobales oDato = CEstadosGlobales.GetItem(lID);
                oResponse = CEstadosGlobales.SetDefecto(oDato);
                if (oResponse.Result)
                {
                    oResponse = CEstadosGlobales.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        CEstadosGlobales.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    CEstadosGlobales.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
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

        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            EstadosGlobalesController CEstadosGlobales = new EstadosGlobalesController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.EstadosGlobales oDato = CEstadosGlobales.GetItem(lID);
                oResponse = CEstadosGlobales.Delete(oDato);
                if (oResponse.Result)
                {
                    oResponse = CEstadosGlobales.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        CEstadosGlobales.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    CEstadosGlobales.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
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

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            EstadosGlobalesController cController = new EstadosGlobalesController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.EstadosGlobales oDato = cController.GetItem(lID);
                oResponse = cController.ModificarActivar(oDato);
                if (oResponse.Result)
                {
                    oResponse = cController.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cController.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cController.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
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


        [DirectMethod]
        public DirectResponse AgregarProyectosTipos()
        {
            DirectResponse direct = new DirectResponse();
            EstadosGlobalesBloqueadosController cBloqueados = new EstadosGlobalesBloqueadosController();
            InfoResponse oResponse;

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
                    oResponse = cBloqueados.Add(dato);
                    if (oResponse.Result)
                    {
                        oResponse = cBloqueados.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storeProyectosTipos.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cBloqueados.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cBloqueados.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
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

        /// <summary>
        /// Quitar ProyectosTipos A un Usuario
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public DirectResponse QuitarProyectosTipos()
        {
            DirectResponse direct = new DirectResponse();
            EstadosGlobalesBloqueadosController cBloqueados = new EstadosGlobalesBloqueadosController();
            InfoResponse oResponse;
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
                        Data.EstadosGlobalesBloqueados oBloqueado = cBloqueados.GetItem(Int64.Parse(selec.RecordID));
                        if(oBloqueado != null)
                        {
                            oResponse = cBloqueados.Delete(oBloqueado);

                            if (oResponse.Result)
                            {
                                oResponse = cBloqueados.SubmitChanges();
                                if (oResponse.Result)
                                {
                                    log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                                    storeProyectosTipos.DataBind();

                                    direct.Success = true;
                                    direct.Result = "";
                                }
                                else
                                {
                                    cBloqueados.DiscardChanges();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(oResponse.Description);
                                }
                            }
                            else
                            {
                                cBloqueados.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(oResponse.Description);
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }
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

        #endregion

    }
}