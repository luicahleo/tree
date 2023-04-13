using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using System.Reflection;
using System.Data.SqlClient;
using log4net;

namespace TreeCore.ModGlobal
{
    public partial class GlobalDireccionesAccesos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> ListaFuncionalidades = new List<long>();
        long lMaestroID = 0;

        #region EVENTOS PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {

                ListaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                    cmbClientes.Hidden = false;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }

                #region FILTROS

                List<string> ListaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, gridMaestro.ColumnModel, ListaIgnore, _Locale);

                List<string> ListaIgnoreDetalle = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters2, storeDetalle, GridDetalle.ColumnModel, ListaIgnoreDetalle, _Locale);

                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));
                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(gridMaestro, storePrincipal, gridMaestro.ColumnModel, ListaIgnore, _Locale);
                Comun.Seleccionable(GridDetalle, storeDetalle, GridDetalle.ColumnModel, ListaIgnoreDetalle, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));


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
                        string sModuloID = Request.QueryString["aux"].ToString();
                        long lCliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        #region MAESTRO
                        if (sModuloID == null || sModuloID == "" || sModuloID == "-1")
                        {

                            List<Data.GlobalDireccionesAccesos> ListaDatos;
                            ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, lCliID);

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridMaestro.ColumnModel, ListaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                        }
                        #endregion

                        #region DETALLE
                        else
                        {
                            List<Data.GlobalDireccionesAccesosJornadas> ListaDatosDetalle;
                            ListaDatosDetalle = ListaDetalle(0, 0, sOrden, sDir, ref iCount, sFiltro, long.Parse(sModuloID));

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(GridDetalle.ColumnModel, ListaDatosDetalle, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            }
                            catch (Exception ex)
                            {
                                LogController lController = new LogController();
                                lController.EscribeLog(Ip, Usuario.UsuarioID, ex.Message);
                                log.Error(ex.Message);
                            }
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
                ResourceManagerTreeCore.RegisterIcon(Icon.ChartCurve);
            }
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ListaFuncionalidades.Contains((long)Comun.ModFun.GLO_GlobalDireccionesAccesos_Lectura))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;

                btnAnadirDetalle.Hidden = true;
                btnEditarDetalle.Hidden = true;
                btnEliminarDetalle.Hidden = true;
                btnActivarDetalle.Hidden = true;
                btnRefrescarDetalle.Hidden = false;
                btnDescargarDetalle.Hidden = true;
            }
            else if (ListaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_DIRECCIONES_ACCESOS))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;

                btnAnadirDetalle.Hidden = false;
                btnEditarDetalle.Hidden = false;
                btnEliminarDetalle.Hidden = false;
                btnActivarDetalle.Hidden = false;
                btnRefrescarDetalle.Hidden = false;
                btnDescargarDetalle.Hidden = false;
            }
        }

        #endregion

        #region STORES

        #region MAESTRO

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort, sDir;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];

                    var vLista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()));

                    if (vLista != null)
                    {
                        storePrincipal.DataSource = vLista;

                        PageProxy temp;
                        temp = (PageProxy)storePrincipal.Proxy[0];
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

        private List<Data.GlobalDireccionesAccesos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.GlobalDireccionesAccesos> ListaDatos;
            GlobalDireccionesAccesosController cGlobalDireccionesAccesos = new GlobalDireccionesAccesosController();

            try
            {
                if (lClienteID.HasValue)
                {
                    ListaDatos = cGlobalDireccionesAccesos.GetItemsWithExtNetFilterList<Data.GlobalDireccionesAccesos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
                }
                else
                {
                    ListaDatos = null;
                }
            }

            catch (Exception ex)
            {
                ListaDatos = null;
                log.Error(ex.Message);
            }

            return ListaDatos;
        }

        #endregion

        #region DETALLE

        protected void storeDetalle_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort, sDir;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters2"];

                    if (!ModuloID.Value.Equals(""))
                    {
                        lMaestroID = Convert.ToInt64(ModuloID.Value);
                    }

                    var vLista = ListaDetalle(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, lMaestroID);

                    if (vLista != null)
                    {
                        storeDetalle.DataSource = vLista;

                        PageProxy temp;
                        temp = (PageProxy)storeDetalle.Proxy[0];
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

        private List<Data.GlobalDireccionesAccesosJornadas> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        {
            List<Data.GlobalDireccionesAccesosJornadas> ListaDatos;

            try
            {
                GlobalDireccionesAccesosJornadasController cAccesosJornadas = new GlobalDireccionesAccesosJornadasController();

                ListaDatos = cAccesosJornadas.GetItemsWithExtNetFilterList<Data.GlobalDireccionesAccesosJornadas>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "GlobalDireccionAccesoID == " + lMaestroID);

            }
            catch (Exception ex)
            {
                ListaDatos = null;
                log.Error(ex.Message);
            }

            return ListaDatos;
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
                    string codTit = Util.ExceptionHandler(ex);
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

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            GlobalDireccionesAccesosController cGlobalDireccionesAccesos = new GlobalDireccionesAccesosController();
            long lCliID = 0;

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.GlobalDireccionesAccesos oDato;
                    oDato = cGlobalDireccionesAccesos.GetItem(lS);

                    if (oDato.Nombre == txtNombre.Text)
                    {
                        oDato.Nombre = txtNombre.Text;
                    }
                    else
                    {
                        lCliID = long.Parse(hdCliID.Value.ToString());

                        if (cGlobalDireccionesAccesos.RegistroDuplicado(txtNombre.Text, lCliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.Nombre = txtNombre.Text;
                        }
                    }

                    if (cGlobalDireccionesAccesos.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    if (cGlobalDireccionesAccesos.RegistroDuplicado(txtNombre.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.GlobalDireccionesAccesos oDato = new Data.GlobalDireccionesAccesos
                        {
                            Nombre = txtNombre.Text,
                            Activo = true,
                            ClienteID = Convert.ToInt32(ClienteID)
                        };

                        if (cGlobalDireccionesAccesos.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();
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
            GlobalDireccionesAccesosController cGlobalDireccionesAccesos = new GlobalDireccionesAccesosController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GlobalDireccionesAccesos oDato;
                oDato = cGlobalDireccionesAccesos.GetItem(lS);

                txtNombre.Text = oDato.Nombre;
                winGestion.Show();
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                direct.Result = "";
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
            GlobalDireccionesAccesosController cGlobalDireccionesAccesos = new GlobalDireccionesAccesosController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cGlobalDireccionesAccesos.RegistroDefecto(lID))
                {
                    direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                    direct.Success = false;
                }
                else if (cGlobalDireccionesAccesos.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
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
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            GlobalDireccionesAccesosController cDireccionesAccesos = new GlobalDireccionesAccesosController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.GlobalDireccionesAccesos oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cDireccionesAccesos.GetDefault(Convert.ToInt32(ClienteID));

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.Defecto)
                    {
                        oDato.Defecto = !oDato.Defecto;
                        cDireccionesAccesos.UpdateItem(oDato);
                    }

                    oDato = cDireccionesAccesos.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cDireccionesAccesos.UpdateItem(oDato);
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cDireccionesAccesos.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cDireccionesAccesos.UpdateItem(oDato);
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
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            GlobalDireccionesAccesosController cDireccionesAccesos = new GlobalDireccionesAccesosController();
            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.GlobalDireccionesAccesos oDato;

                oDato = cDireccionesAccesos.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cDireccionesAccesos.UpdateItem(oDato))
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

        #endregion

        #region DIRECT METHOD DETALLE

        [DirectMethod()]
        public DirectResponse AgregarEditarDetalle(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            GlobalDireccionesAccesosJornadasController cGlobalDireccionesAccesosJornadas = new GlobalDireccionesAccesosJornadasController();

            long lAccesoID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                    Data.GlobalDireccionesAccesosJornadas oDato;
                    oDato = cGlobalDireccionesAccesosJornadas.GetItem(lS);

                    oDato.Lunes = chkLunes.Checked;
                    oDato.Martes = chkMartes.Checked;
                    oDato.Miercoles = chkMiercoles.Checked;
                    oDato.Jueves = chkJueves.Checked;
                    oDato.Viernes = chkViernes.Checked;
                    oDato.Sabado = chkSabado.Checked;
                    oDato.Domingo = chkDomingo.Checked;
                    oDato.Festivos = chkFestivo.Checked;

                    oDato.GlobalDireccionAccesoID = lAccesoID;
                    oDato.HoraInicio = tfHoraInicio.SelectedTime;
                    oDato.HoraFin = tfHoraFin.SelectedTime;

                    if (cGlobalDireccionesAccesosJornadas.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storeDetalle.DataBind();
                    }
                }
                else
                {
                    Data.GlobalDireccionesAccesosJornadas oDato = new Data.GlobalDireccionesAccesosJornadas
                    {
                        Lunes = chkLunes.Checked,
                        Martes = chkMartes.Checked,
                        Miercoles = chkMiercoles.Checked,
                        Jueves = chkJueves.Checked,
                        Viernes = chkViernes.Checked,
                        Sabado = chkSabado.Checked,
                        Domingo = chkDomingo.Checked,
                        Festivos = chkFestivo.Checked,

                        HoraInicio = tfHoraInicio.SelectedTime,
                        HoraFin = tfHoraFin.SelectedTime,
                        GlobalDireccionAccesoID = lAccesoID,
                        Activo = true
                    };

                    if (cGlobalDireccionesAccesosJornadas.AddItem(oDato) != null)
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storeDetalle.DataBind();
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
        public DirectResponse MostrarEditarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalDireccionesAccesosJornadasController cGlobalDireccionesAccesosJornadas = new GlobalDireccionesAccesosJornadasController();


            try
            {
                long lS = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.GlobalDireccionesAccesosJornadas oDato;
                oDato = cGlobalDireccionesAccesosJornadas.GetItem(lS);

                chkLunes.Checked = oDato.Lunes;
                chkMartes.Checked = oDato.Martes;
                chkMiercoles.Checked = oDato.Miercoles;
                chkJueves.Checked = oDato.Jueves;
                chkViernes.Checked = oDato.Viernes;
                chkSabado.Checked = oDato.Sabado;
                chkDomingo.Checked = oDato.Domingo;
                chkFestivo.Checked = oDato.Festivos;


                tfHoraInicio.SelectedTime = oDato.HoraInicio;
                tfHoraFin.SelectedTime = oDato.HoraFin;

                winGestionDetalle.Show();
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
        public DirectResponse EliminarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalDireccionesAccesosJornadasController cAccesosJornadas = new GlobalDireccionesAccesosJornadasController();

            long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

            try
            {
                if (cAccesosJornadas.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
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
        public DirectResponse ActivarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalDireccionesAccesosJornadasController cAccesosJornadas = new GlobalDireccionesAccesosJornadasController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.GlobalDireccionesAccesosJornadas oDato;
                oDato = cAccesosJornadas.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cAccesosJornadas.UpdateItem(oDato))
                {
                    storeDetalle.DataBind();
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

        #region FUNCTIONS

        #endregion

    }
}