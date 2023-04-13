using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Reflection;
using System.Data.SqlClient;
using System.Transactions;

namespace TreeCore.ModGlobal
{
    public partial class LoginSettings : TreeCore.Page.BasePageExtNet
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
                { "CodigoRol" };

                Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                List<string> listaIgnoreLDAP = new List<string>() { };
                Comun.CreateGridFilters(gridFiltersLDAP, storeToolConexiones, gridLDAP.ColumnModel, listaIgnoreLDAP, _Locale);
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

                #region DATOS CARGA

                try
                {
                    ParametrosController cParam = new ParametrosController();
                    Data.Parametros oFactor;
                    Data.Parametros oMulti;

                    oFactor = cParam.GetItemByName(Comun.DOBLE_VALIDACION);

                    if (oFactor != null)
                    {
                        if (oFactor.Valor == "SI" || oFactor.Valor == "YES")
                        {
                            btnToggleFactor.Pressed = true;
                            CambiarParametro("FACTOR", "");
                        }
                        else if (oFactor.Valor == "NO")
                        {
                            btnToggleFactor.Pressed = false;
                        }
                    }

                    oMulti = cParam.GetItemByName(Comun.ACCESO_MULTIHOMING);

                    if (oMulti != null)
                    {
                        if (oMulti.Valor == "SI" || oMulti.Valor == "YES")
                        {
                            X.Js.Call("cargarComboRoles");
                            btnToggleMultihoming.Pressed = true;
                            CambiarParametro("MULTIHOMING", "");

                            storePrincipal.Reload();
                            btnAnadir.Enable();
                            btnRefrescar.Enable();
                            btnDescargar.Enable();
                        }
                        else if (oMulti.Valor == "NO")
                        {
                            btnToggleMultihoming.Pressed = false;
                        }
                    }


                    ToolIntegracionesServiciosMetodosController cIntegraciones = new ToolIntegracionesServiciosMetodosController();
                    ToolConexionesController cConex = new ToolConexionesController();
                    Data.Vw_ToolServicios oDato;
                    List<Data.ToolConexiones> listaConexiones;

                    oDato = cIntegraciones.GetIntegracionByNombreServicio(Comun.INTEGRACION_SERVICIO_IDENTIFICACION);
                    listaConexiones = cConex.getListaConexiones((long)oDato.IntegracionID);

                    if (oDato != null)
                    {
                        if ((bool)oDato.Activo && listaConexiones.Count > 0)
                        {
                            btnToggleLDAP.Pressed = true;
                            CambiarParametro("LDAP", "");

                            storeToolConexiones.Reload();
                            btnAnadirLDAP.Enable();
                            btnRefrescarLDAP.Enable();
                            btnDescargarLDAP.Enable();
                            btnTgLoginCreated.Enable();

                            Data.Parametros oCrear = null;
                            oCrear = cParam.GetItemByName(Comun.LDAP_CREAR_USUARIO);

                            if (oCrear != null)
                            {
                                if (oCrear.Valor == "SI" || oCrear.Valor == "YES" || oCrear.Valor == "True")
                                {
                                    btnTgLoginCreated.Pressed = true;
                                }
                                else if (oCrear.Valor == "NO")
                                {
                                    btnTgLoginCreated.Pressed = false;
                                }
                            }
                        }
                        else
                        {
                            btnToggleLDAP.Pressed = false;

                            storeToolConexiones.Reload();
                            btnAnadirLDAP.Enable();
                            btnRefrescarLDAP.Enable();
                            btnDescargarLDAP.Enable();
                            btnTgLoginCreated.Enable();
                            btnDescargarLDAP.Enable();
                            btnRefrescarLDAP.Enable();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                #endregion
            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        string sGrid = Request.QueryString["aux3"];
                        int iCount = 0;

                        if (sGrid == "-1")
                        {
                            List<Data.ToolConexiones> listaConexiones = null;
                            listaConexiones = ListaLDAP(0, 0, sOrden, sDir, ref iCount, sFiltro);

                            #region ESTADISTICAS
                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridLDAP.ColumnModel, listaConexiones, Response, "", "Login", _Locale);
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
                        else
                        {
                            List<Data.Vw_GruposAccesosWebRoles> listaDatos = null;
                            listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro);

                            foreach (Data.Vw_GruposAccesosWebRoles oDato in listaDatos)
                            {
                                GruposAccesosWebRolesController cGrupos = new GruposAccesosWebRolesController();
                                RolesController cRoles = new RolesController();
                                List<long> listaFinal = new List<long>();

                                if (oDato.CodigoRol != null && oDato.CodigoRol != "")
                                {
                                    long lRolID = long.Parse(oDato.CodigoRol.Split('(')[0]);
                                    string sRol = cRoles.getCodigoByID(lRolID);

                                    Data.GruposAccesosWebRoles oValor = cGrupos.GetItem(oDato.GrupoAccesoWebRolID);
                                    List<long> listaRoles = cGrupos.getListaByGrupoAcceso(oValor.GrupoAccesoWebID);

                                    foreach (long lRol in listaRoles)
                                    {
                                        if (!listaFinal.Contains(lRol))
                                        {
                                            listaFinal.Add(lRol);
                                        }
                                    }

                                    oDato.CodigoRol = sRol + " (" + listaFinal.Count + ")";
                                }
                            }

                            #region ESTADISTICAS
                            try
                            {
                                Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", "Login", _Locale);
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
            sPagina = "GlobalSettingsContenedor.aspx";
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { grid, gridLDAP } },
                { "Download", new List<ComponentBase> { btnDescargar, btnDescargarLDAP }},
                { "Post", new List<ComponentBase> { btnAnadir, btnAnadirLDAP }},
                { "Put", new List<ComponentBase> { btnEditar, btnEditarLDAP,  }},
                { "Delete", new List<ComponentBase> { btnEliminar, btnEliminarLDAP }}
            };
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
                    List<object> listaPrincipal = new List<object>();
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];

                    if (btnToggleMultihoming.Pressed)
                    {
                        var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro);

                        if (lista != null)
                        {


                            foreach (Data.Vw_GruposAccesosWebRoles oDato in lista)
                            {
                                GruposAccesosWebRolesController cGrupos = new GruposAccesosWebRolesController();
                                GruposAccesosWebController cGruposAccesos = new GruposAccesosWebController();
                                RolesController cRoles = new RolesController();
                                List<long> listaFinal = new List<long>();

                                if (oDato.CodigoRol != null && oDato.CodigoRol != "")
                                {
                                    long lRolID = long.Parse(oDato.CodigoRol.Split('(')[0]);
                                    string sRol = cRoles.getCodigoByID(lRolID);

                                    Data.GruposAccesosWebRoles oValor = cGrupos.GetItem(oDato.GrupoAccesoWebRolID);
                                    List<long> listaRoles = cGrupos.getListaByGrupoAcceso(oValor.GrupoAccesoWebID);

                                    foreach (long lRol in listaRoles)
                                    {
                                        if (!listaFinal.Contains(lRol))
                                        {
                                            listaFinal.Add(lRol);
                                        }
                                    }

                                    oDato.CodigoRol = sRol + " (" + listaFinal.Count + ")";
                                }

                                string sURL = cGruposAccesos.getURLByGrupoAcceso(oDato.GrupoAcceso);

                                Object oLogin = new
                                {
                                    GrupoAccesoWebRolID = oDato.GrupoAccesoWebRolID,
                                    GrupoAcceso = oDato.GrupoAcceso,
                                    CodigoRol = oDato.CodigoRol,
                                    ActivoRol = oDato.ActivoRol,
                                    ActivoGrupo = oDato.ActivoGrupo,
                                    URL = sURL,
                                };

                                listaPrincipal.Add(oLogin);
                            }

                            storePrincipal.DataSource = listaPrincipal;
                            PageProxy temp = (PageProxy)storePrincipal.Proxy[0];
                            temp.Total = iCount;
                        }
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_GruposAccesosWebRoles> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.Vw_GruposAccesosWebRoles> listaDatos;
            List<string> listaGruposAccesos = new List<string>();
            Data.Vw_GruposAccesosWebRoles oDato;
            List<Data.Vw_GruposAccesosWebRoles> listaFinal = new List<Data.Vw_GruposAccesosWebRoles>();
            GruposAccesosWebRolesController cRoles = new GruposAccesosWebRolesController();

            try
            {
                listaDatos = cRoles.GetItemsWithExtNetFilterList<Data.Vw_GruposAccesosWebRoles>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);

                if (listaDatos != null)
                {
                    foreach (Data.Vw_GruposAccesosWebRoles oLista in listaDatos)
                    {
                        if (!listaGruposAccesos.Contains(oLista.GrupoAcceso))
                        {
                            listaGruposAccesos.Add(oLista.GrupoAcceso);
                        }
                    }

                    foreach (string sValor in listaGruposAccesos)
                    {
                        oDato = cRoles.getVistaByGrupoAcceso(sValor);

                        if (oDato != null)
                        {
                            listaFinal.Add(oDato);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaFinal;
        }

        #endregion

        #region ROLES

        protected void storeRoles_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Roles> listaRoles;
                    RolesController cRoles = new RolesController();

                    listaRoles = cRoles.GetActivos(long.Parse(hdCliID.Value.ToString()));

                    if (listaRoles != null)
                    {
                        storeRoles.DataSource = listaRoles;
                        storeRoles.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region ROLES ASIGNADOS

        protected void storeRolesAsignados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Roles> listaDatos;
                    List<long> listaIDs;
                    RolesController cRoles = new RolesController();
                    GruposAccesosWebRolesController cGruposRoles = new GruposAccesosWebRolesController();

                    listaIDs = cGruposRoles.getRolByGrupoAcceso(long.Parse(hdGrupoAccesoWeb.Value.ToString()));
                    listaDatos = cRoles.getRolesAsignados(listaIDs);

                    if (listaDatos != null)
                    {
                        storeRolesAsignados.DataSource = listaDatos;
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

        #endregion

        #region TOOL CONEXIONES

        protected void storeToolConexiones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
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

                    ToolIntegracionesServiciosMetodosController cIntegraciones = new ToolIntegracionesServiciosMetodosController();
                    Data.Vw_ToolServicios oDato;

                    oDato = cIntegraciones.GetIntegracionByNombreServicio(Comun.INTEGRACION_SERVICIO_IDENTIFICACION);

                    if (oDato != null)
                    {
                        //if ((bool)oDato.Activo)
                        //{
                        var lista = ListaLDAP(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro);

                        if (lista != null)
                        {
                            storeToolConexiones.DataSource = lista;
                            PageProxy temp = (PageProxy)storeToolConexiones.Proxy[0];
                            temp.Total = iCount;
                        }
                        //}
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.ToolConexiones> ListaLDAP(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.ToolConexiones> listaDatos;
            ToolConexionesController cConex = new ToolConexionesController();
            ToolServiciosController cServ = new ToolServiciosController();
            ToolIntegracionesController cInt = new ToolIntegracionesController();

            try
            {
                long lServicioID = cServ.GetServicioByName(Comun.INTEGRACION_SERVICIO_IDENTIFICACION).ServicioID;
                Data.Vw_ToolConexiones oVista = cConex.GetConexionByServicioID(lServicioID);
                long lIntegracionID = cInt.GetItem((long)oVista.IntegracionID).IntegracionID;
                listaDatos = cConex.GetItemsWithExtNetFilterList<Data.ToolConexiones>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "IntegracionID == " + lIntegracionID);

            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar, List<string> listaRolesIDs, List<string> listaRolesIDsAsignados)
        {
            DirectResponse direct = new DirectResponse();
            GruposAccesosWebRolesController cGrupos = new GruposAccesosWebRolesController();
            GruposAccesosWebController cController = new GruposAccesosWebController();

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.GruposAccesosWebRoles oGrupo = cGrupos.GetItem(lS);
                    Data.GruposAccesosWeb oAcceso = cController.GetItem(oGrupo.GrupoAccesoWebID);

                    if (cController.RegistroDuplicadoLogin(txtURL.Text, txtGrupoAcceso.Text, long.Parse(hdCliID.Value.ToString())))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        oAcceso.GrupoAcceso = txtGrupoAcceso.Text;
                        oAcceso.URL = txtURL.Text;
                    }
                    if (cController.UpdateItem(oAcceso))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();

                        foreach (string sAsignado in listaRolesIDsAsignados)
                        {
                            if (!listaRolesIDs.Contains(sAsignado))
                            {
                                Data.GruposAccesosWebRoles dato = cGrupos.getAccesoWebByRol(oAcceso.GrupoAccesoWebID, long.Parse(sAsignado));
                                cGrupos.DeleteItem(dato.GrupoAccesoWebRolID);
                                log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            }
                        }
                        foreach (string sRol in listaRolesIDs)
                        {
                            if (!listaRolesIDsAsignados.Contains(sRol))
                            {
                                if (!cGrupos.RegistroDuplicado(oAcceso.GrupoAccesoWebID, long.Parse(sRol)))
                                {
                                    Data.GruposAccesosWebRoles oRol = new Data.GruposAccesosWebRoles();
                                    oRol.GrupoAccesoWebID = oAcceso.GrupoAccesoWebID;
                                    oRol.RolID = long.Parse(sRol);
                                    cGrupos.AddItem(oRol);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (cController.RegistroDuplicadoLogin(txtURL.Text, txtGrupoAcceso.Text, long.Parse(hdCliID.Value.ToString())))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.GruposAccesosWeb dato = new Data.GruposAccesosWeb();
                        dato.GrupoAcceso = txtGrupoAcceso.Text;
                        dato.URL = txtURL.Text;
                        dato.ClienteID = long.Parse(hdCliID.Value.ToString());
                        dato.Activo = true;

                        if (cController.AddItem(dato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();

                            foreach (string sValor in listaRolesIDs)
                            {
                                if (!cGrupos.RegistroDuplicado(dato.GrupoAccesoWebID, long.Parse(sValor)))
                                {
                                    Data.GruposAccesosWebRoles oRol = new Data.GruposAccesosWebRoles();
                                    oRol.GrupoAccesoWebID = dato.GrupoAccesoWebID;
                                    oRol.RolID = long.Parse(sValor);
                                    cGrupos.AddItem(oRol);
                                }
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
            GruposAccesosWebRolesController cGrupos = new GruposAccesosWebRolesController();
            GruposAccesosWebController cController = new GruposAccesosWebController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);
                Data.GruposAccesosWebRoles oGrupo = cGrupos.GetItem(lS);
                Data.GruposAccesosWeb oAcceso = cController.GetItem(oGrupo.GrupoAccesoWebID);

                txtGrupoAcceso.Text = oAcceso.GrupoAcceso;
                txtURL.Text = oAcceso.URL;

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
            GruposAccesosWebRolesController cGrupos = new GruposAccesosWebRolesController();
            GruposAccesosWebController cWeb = new GruposAccesosWebController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                long lGrupoID = cGrupos.GetItem(lID).GrupoAccesoWebID;

                if (lGrupoID != 0)
                {
                    List<long> listaRoles = cGrupos.getIDsByGrupo(lGrupoID);

                    foreach (long lRol in listaRoles)
                    {
                        if (cGrupos.DeleteItem(lRol))
                        {
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            direct.Success = true;
                            direct.Result = "";
                        }
                    }

                    if (cWeb.DeleteItem(lGrupoID))
                    {
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                        direct.Success = true;
                        direct.Result = "";
                    }
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
        public DirectResponse AgregarEditarLDAP(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            ToolConexionesController cConex = new ToolConexionesController();

            ToolIntegracionesServiciosMetodosController cIntegraciones = new ToolIntegracionesServiciosMetodosController();
            cIntegraciones.SetDataContext(cConex.Context);

            try
            {
                Data.Vw_ToolServicios oDato = cIntegraciones.GetIntegracionByNombreServicio(Comun.INTEGRACION_SERVICIO_IDENTIFICACION);
                List<Data.ToolConexiones> listaConexiones;
                string sIntegracion = oDato.Integracion;
                string sNombreClase = oDato.NombreClase;
                Type type = Type.GetType(sNombreClase);
                string sDominioABuscar = "";
                object classInstance = Activator.CreateInstance(type, sDominioABuscar);

                Data.ToolConexiones oConexion;

                if (!bAgregar)
                {
                    using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
                    {
                        try
                        {
                            long lS = long.Parse(GridRowSelectLDAP.SelectedRecordID);
                            oConexion = cConex.GetItem(lS);

                            if (txtServer.Text == oConexion.Servidor && txtUsuario.Text == oConexion.Usuario)
                            {
                                oConexion.Servidor = txtServer.Text;
                                oConexion.Usuario = txtUsuario.Text;

                                if (txtPasswordConfirm.Text == txtPasswordField.Text)
                                {
                                    oConexion.Clave = Util.EncryptKey(txtPasswordField.Text);
                                }
                                else
                                {
                                    MensajeBox(GetGlobalResource("jsError"), GetGlobalResource("jsClaveDiferente"), MessageBox.Icon.ERROR, null);
                                    trans.Complete();

                                    direct.Success = true;
                                    direct.Result = "error";
                                    return direct;
                                }

                                if (cConex.UpdateItem(oConexion))
                                {
                                    log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));

                                    if (btnToggleLDAP.Pressed)
                                    {
                                        Data.ToolIntegracionesServiciosMetodos oIntg;

                                        if (oDato != null)
                                        {
                                            oIntg = cIntegraciones.getItemByIntegracionID(oDato.IntegracionID);

                                            if (oIntg != null)
                                            {
                                                if (type != null)
                                                {
                                                    MethodInfo methodInfo = type.GetMethod("ValidarUsuarioDominio");

                                                    if (methodInfo != null)
                                                    {
                                                        object result = null;
                                                        ParameterInfo[] parameters = methodInfo.GetParameters();

                                                        if (parameters.Length == 0)
                                                        {
                                                            result = methodInfo.Invoke(classInstance, null);
                                                        }
                                                        else
                                                        {
                                                            string sClave = Util.DecryptKey(oConexion.Clave);
                                                            object[] parametersArray = new object[] { oConexion.Usuario, sClave, oConexion.Servidor };
                                                            result = methodInfo.Invoke(classInstance, parametersArray);

                                                            if (result != null && (bool)result == true)
                                                            {
                                                                oIntg.Activo = true;
                                                                cIntegraciones.UpdateItem(oIntg);
                                                            }
                                                            else
                                                            {
                                                                listaConexiones = cConex.getListaConexiones((long)oDato.IntegracionID);

                                                                if (listaConexiones != null && listaConexiones.Count == 1)
                                                                {
                                                                    MensajeBox(GetGlobalResource("jsError"), GetGlobalResource("strMsgUsuarioClaveIncorrectos"), MessageBox.Icon.ERROR, null);

                                                                    direct.Success = true;
                                                                    direct.Result = "error";
                                                                    return direct;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                MensajeBox(GetGlobalResource("jsError"), GetGlobalResource("jsIntegracionNoEncontrada"), MessageBox.Icon.ERROR, null);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (cConex.RegistroDuplicadoLogin(txtServer.Text, txtUsuario.Text))
                                {
                                    log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                    MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                                }
                                else
                                {
                                    if (txtServer.Text != "" && oConexion.Servidor != txtServer.Text)
                                    {
                                        oConexion.Servidor = txtServer.Text;
                                    }

                                    if (txtUsuario.Text != "" && oConexion.Usuario != txtUsuario.Text)
                                    {
                                        oConexion.Usuario = txtUsuario.Text;
                                    }

                                    if (txtPasswordField.Text != "" && txtPasswordConfirm.Text != "" && txtPasswordConfirm.Text == txtPasswordField.Text)
                                    {
                                        oConexion.Clave = Util.EncryptKey(txtPasswordField.Text);
                                    }
                                    else
                                    {
                                        MensajeBox(GetGlobalResource("jsError"), GetGlobalResource("jsClaveDiferente"), MessageBox.Icon.ERROR, null);
                                        trans.Complete();

                                        direct.Success = true;
                                        direct.Result = "error";
                                        return direct;
                                    }

                                    if (cConex.UpdateItem(oConexion))
                                    {
                                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));

                                        if (btnToggleLDAP.Pressed)
                                        {
                                            Data.ToolIntegracionesServiciosMetodos oIntg;

                                            if (oDato != null)
                                            {
                                                oIntg = cIntegraciones.getItemByIntegracionID(oDato.IntegracionID);

                                                if (oIntg != null)
                                                {
                                                    if (type != null)
                                                    {
                                                        MethodInfo methodInfo = type.GetMethod("ValidarUsuarioDominio");

                                                        if (methodInfo != null)
                                                        {
                                                            object result = null;
                                                            ParameterInfo[] parameters = methodInfo.GetParameters();

                                                            if (parameters.Length == 0)
                                                            {
                                                                result = methodInfo.Invoke(classInstance, null);
                                                            }
                                                            else
                                                            {
                                                                string sClave = Util.DecryptKey(oConexion.Clave);
                                                                object[] parametersArray = new object[] { oConexion.Usuario, sClave, oConexion.Servidor };
                                                                result = methodInfo.Invoke(classInstance, parametersArray);

                                                                if (result != null && (bool)result == true)
                                                                {
                                                                    oIntg.Activo = true;
                                                                    cIntegraciones.UpdateItem(oIntg);
                                                                }
                                                                else
                                                                {
                                                                    listaConexiones = cConex.getListaConexiones((long)oDato.IntegracionID);

                                                                    if (listaConexiones != null && listaConexiones.Count == 1)
                                                                    {
                                                                        storeToolConexiones.RemoveAll();
                                                                        btnToggleLDAP.Pressed = false;
                                                                        btnAnadirLDAP.Disable();
                                                                        btnEditarLDAP.Disable();
                                                                        btnEliminarLDAP.Disable();
                                                                        btnRefrescarLDAP.Disable();
                                                                        btnDescargarLDAP.Disable();
                                                                        MensajeBox(GetGlobalResource("jsError"), GetGlobalResource("strMsgUsuarioClaveIncorrectos"), MessageBox.Icon.ERROR, null);

                                                                        direct.Success = true;
                                                                        direct.Result = "error";
                                                                        return direct;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    MensajeBox(GetGlobalResource("jsError"), GetGlobalResource("jsIntegracionNoEncontrada"), MessageBox.Icon.ERROR, null);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            trans.Complete();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            trans.Dispose();
                        }
                    }
                }
                else
                {
                    using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
                    {
                        try
                        {
                            oConexion = new Data.ToolConexiones();
                            oConexion.IntegracionID = (long)oDato.IntegracionID;
                            oConexion.ClienteID = long.Parse(hdCliID.Value.ToString());
                            oConexion.Activo = true;
                            oConexion.IsReporting = false;
                            oConexion.Privada = false;
                            oConexion.Defecto = false;
                            oConexion.ConexionPrincipal = false;

                            if (cConex.RegistroDuplicadoLogin(txtServer.Text, txtUsuario.Text))
                            {
                                log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                            }
                            else
                            {
                                if (txtServer.Text != "")
                                {
                                    oConexion.Servidor = txtServer.Text;
                                }

                                if (txtUsuario.Text != "")
                                {
                                    oConexion.Usuario = txtUsuario.Text;
                                }

                                if (txtPasswordField.Text != "" && txtPasswordConfirm.Text != "" && txtPasswordConfirm.Text == txtPasswordField.Text)
                                {
                                    oConexion.Clave = Util.EncryptKey(txtPasswordField.Text);
                                }
                                else
                                {
                                    MensajeBox(GetGlobalResource("jsError"), GetGlobalResource("jsClaveDiferente"), MessageBox.Icon.ERROR, null);
                                    trans.Complete();

                                    direct.Success = true;
                                    direct.Result = "error";
                                    return direct;
                                }

                                if (cConex.AddItem(oConexion) != null)
                                {
                                    log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));

                                    if (btnToggleLDAP.Pressed)
                                    {
                                        Data.ToolIntegracionesServiciosMetodos oIntg;

                                        if (oDato != null)
                                        {
                                            oIntg = cIntegraciones.getItemByIntegracionID(oDato.IntegracionID);

                                            if (oIntg != null)
                                            {
                                                if (type != null)
                                                {
                                                    MethodInfo methodInfo = type.GetMethod("ValidarUsuarioDominio");

                                                    if (methodInfo != null)
                                                    {
                                                        object result = null;
                                                        ParameterInfo[] parameters = methodInfo.GetParameters();

                                                        if (parameters.Length == 0)
                                                        {
                                                            result = methodInfo.Invoke(classInstance, null);
                                                        }
                                                        else
                                                        {
                                                            string sClave = Util.DecryptKey(oConexion.Clave);
                                                            object[] parametersArray = new object[] { oConexion.Usuario, sClave, oConexion.Servidor };
                                                            result = methodInfo.Invoke(classInstance, parametersArray);

                                                            if (result != null && (bool)result == true)
                                                            {
                                                                oIntg.Activo = true;
                                                                cIntegraciones.UpdateItem(oIntg);
                                                            }
                                                            else
                                                            {
                                                                listaConexiones = cConex.getListaConexiones((long)oDato.IntegracionID);

                                                                if (listaConexiones != null && listaConexiones.Count > 1)
                                                                {
                                                                    btnToggleLDAP.Pressed = true;
                                                                    MensajeBox(GetGlobalResource("jsError"), GetGlobalResource("strMsgUsuarioClaveIncorrectos"), MessageBox.Icon.ERROR, null);

                                                                    direct.Success = true;
                                                                    direct.Result = "error";
                                                                    return direct;
                                                                }
                                                                else
                                                                {
                                                                    btnToggleLDAP.Pressed = false;
                                                                    btnAnadirLDAP.Disable();
                                                                    btnEditarLDAP.Disable();
                                                                    btnEliminarLDAP.Disable();
                                                                    btnRefrescarLDAP.Disable();
                                                                    btnDescargarLDAP.Disable();
                                                                    storeToolConexiones.RemoveAll();
                                                                    MensajeBox(GetGlobalResource("jsError"), GetGlobalResource("strMsgUsuarioClaveIncorrectos"), MessageBox.Icon.ERROR, null);

                                                                    direct.Success = true;
                                                                    direct.Result = "error";
                                                                    return direct;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                MensajeBox(GetGlobalResource("jsError"), GetGlobalResource("jsIntegracionNoEncontrada"), MessageBox.Icon.ERROR, null);
                                            }
                                        }
                                    }
                                }
                            }

                            trans.Complete();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            trans.Dispose();
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
        public DirectResponse MostrarEditarLDAP()
        {
            DirectResponse direct = new DirectResponse();
            ToolConexionesController cConex = new ToolConexionesController();

            try
            {
                long lS = long.Parse(GridRowSelectLDAP.SelectedRecordID);
                Data.ToolConexiones oConexion = cConex.GetItem(lS);

                txtServer.Text = oConexion.Servidor;
                txtUsuario.Text = oConexion.Usuario;

                string sClave = Util.DecryptKey(oConexion.Clave);
                txtPasswordField.Text = sClave;
                txtPasswordConfirm.Text = sClave;

                winGestionLDAP.Show();
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
        public DirectResponse EliminarLDAP()
        {
            DirectResponse direct = new DirectResponse();
            List<Data.ToolConexiones> listaConexiones;
            ToolIntegracionesServiciosMetodosController cIntegraciones = new ToolIntegracionesServiciosMetodosController();
            ToolConexionesController cConex = new ToolConexionesController();

            long lID = long.Parse(GridRowSelectLDAP.SelectedRecordID);

            try
            {

                if (cConex.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));

                    Data.Vw_ToolServicios oDato = cIntegraciones.GetIntegracionByNombreServicio(Comun.INTEGRACION_SERVICIO_IDENTIFICACION);
                    listaConexiones = cConex.getListaConexiones((long)oDato.IntegracionID);

                    if (listaConexiones != null && listaConexiones.Count > 2)
                    {
                        btnToggleLDAP.Pressed = true;

                        direct.Success = true;
                        direct.Result = "";
                        return direct;
                    }
                    else
                    {
                        btnToggleLDAP.Pressed = false;
                        btnAnadirLDAP.Disable();
                        btnEditarLDAP.Disable();
                        btnEliminarLDAP.Disable();
                        btnRefrescarLDAP.Disable();
                        btnDescargarLDAP.Disable();
                        storeToolConexiones.RemoveAll();

                        direct.Success = true;
                        direct.Result = "";
                        return direct;
                    }
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

        #endregion

        #region FUNCTIONS

        [DirectMethod()]
        public DirectResponse CambiarParametro(string sParametro, string sValor)
        {
            DirectResponse direct = new DirectResponse();
            ParametrosController cParam = new ParametrosController();
            Data.Parametros oParametro;

            try
            {
                switch (sParametro)
                {
                    case "FACTOR":

                        oParametro = cParam.GetItemByName(Comun.DOBLE_VALIDACION);

                        if (btnToggleFactor.Pressed)
                        {
                            oParametro.Valor = "SI";
                        }
                        else
                        {
                            oParametro.Valor = "NO";
                        }

                        cParam.UpdateItem(oParametro);

                        break;

                    case "USUARIO":

                        oParametro = cParam.GetItemByName(Comun.LDAP_CREAR_USUARIO);

                        if (sValor != "")
                        {
                            if (sValor == "true")
                            {
                                oParametro.Valor = "SI";
                            }
                            else
                            {
                                oParametro.Valor = "NO";
                            }
                        }

                        cParam.UpdateItem(oParametro);

                        break;

                    case "LDAP":

                        if (!btnToggleLDAP.Pressed)
                        {
                            ToolIntegracionesServiciosMetodosController cIntegraciones = new ToolIntegracionesServiciosMetodosController();
                            Data.Vw_ToolServicios oDato = null;
                            Data.ToolIntegracionesServiciosMetodos oIntg;

                            direct.Success = true;
                            direct.Result = "NO";

                            oDato = cIntegraciones.GetIntegracionByNombreServicio(Comun.INTEGRACION_SERVICIO_IDENTIFICACION);
                            oIntg = cIntegraciones.getItemByIntegracionID(oDato.IntegracionID);

                            if (oIntg != null)
                            {
                                oIntg.Activo = false;
                                cIntegraciones.UpdateItem(oIntg);
                            }
                            else
                            {
                                MensajeBox(GetGlobalResource("jsError"), GetGlobalResource("jsIntegracionNoEncontrada"), MessageBox.Icon.ERROR, null);
                            }
                        }
                        else
                        {
                            ToolIntegracionesServiciosMetodosController cIntegraciones = new ToolIntegracionesServiciosMetodosController();
                            ToolConexionesController cConex = new ToolConexionesController();
                            Data.Vw_ToolServicios oDato = null;
                            Data.ToolIntegracionesServiciosMetodos oIntg;
                            List<Data.ToolConexiones> listaConexiones;
                            string sDominioABuscar = "";

                            oDato = cIntegraciones.GetIntegracionByNombreServicio(Comun.INTEGRACION_SERVICIO_IDENTIFICACION);

                            if (oDato != null)
                            {
                                oIntg = cIntegraciones.getItemByIntegracionID(oDato.IntegracionID);
                                listaConexiones = cConex.getListaConexiones((long)oDato.IntegracionID);

                                if (oIntg != null && listaConexiones != null)
                                {
                                    foreach (Data.ToolConexiones oConexion in listaConexiones)
                                    {
                                        if (oConexion.Usuario.Contains("\\"))
                                        {
                                            sDominioABuscar = oConexion.Usuario.Substring(0, oConexion.Usuario.LastIndexOf("\\"));
                                        }

                                        string sIntegracion = oDato.Integracion;
                                        string sNombreClase = oDato.NombreClase;
                                        Type type = Type.GetType(sNombreClase);

                                        if (type != null)
                                        {
                                            MethodInfo methodInfo = type.GetMethod("ValidarUsuarioDominio");

                                            if (methodInfo != null)
                                            {
                                                object result = null;
                                                ParameterInfo[] parameters = methodInfo.GetParameters();
                                                object classInstance = Activator.CreateInstance(type, sDominioABuscar);

                                                if (parameters.Length == 0)
                                                {
                                                    result = methodInfo.Invoke(classInstance, null);
                                                }
                                                else
                                                {
                                                    string sClave = Util.DecryptKey(oConexion.Clave);
                                                    object[] parametersArray = new object[] { oConexion.Usuario, sClave, oConexion.Servidor };
                                                    result = methodInfo.Invoke(classInstance, parametersArray);

                                                    if (result != null && (bool)result == true)
                                                    {
                                                        oIntg.Activo = true;
                                                        cIntegraciones.UpdateItem(oIntg);
                                                    }
                                                    else
                                                    {
                                                        btnToggleLDAP.Pressed = false;
                                                        btnAnadirLDAP.Disable();
                                                        btnEditarLDAP.Disable();
                                                        btnEliminarLDAP.Disable();
                                                        btnRefrescarLDAP.Disable();
                                                        btnDescargarLDAP.Disable();
                                                        storeToolConexiones.RemoveAll();
                                                        MensajeBox(GetGlobalResource("jsError"), GetGlobalResource("strMsgUsuarioClaveIncorrectos"), MessageBox.Icon.ERROR, null);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    MensajeBox(GetGlobalResource("jsError"), GetGlobalResource("jsIntegracionNoEncontrada"), MessageBox.Icon.ERROR, null);
                                }
                            }

                            direct.Success = true;
                            direct.Result = "SI";
                        }

                        break;
                    case "MULTIHOMING":

                        oParametro = cParam.GetItemByName(Comun.ACCESO_MULTIHOMING);

                        if (btnToggleMultihoming.Pressed)
                        {
                            oParametro.Valor = "SI";

                            direct.Success = true;
                            direct.Result = "SI";
                        }
                        else
                        {
                            oParametro.Valor = "NO";

                            direct.Success = true;
                            direct.Result = "NO";
                        }

                        cParam.UpdateItem(oParametro);

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
                return direct;
            }

            return direct;
        }

        #endregion
    }
}