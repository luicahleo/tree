using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TreeCore.Data;
using System.Reflection;

namespace TreeCore.ModGlobal
{
    public partial class VexProyectosSecciones : TreeCore.Page.BasePageExtNet
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
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        //long? cliID = null;
                        int iCount = 0;

                        #region MAESTRO
                        if (sModuloID == null || sModuloID == "" || sModuloID == "-1")
                        {

                            List<Data.VexProyectos> ListaDatos = null;
                            ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);

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
                            List<Data.VexSecciones> ListaDatosDetalle = null;

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
            if (ListaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_VEX_PROYECTOS_SECCIONES))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnActivar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;

                btnAnadirDetalle.Hidden = true;
                btnEditarDetalle.Hidden = true;
                btnEliminarDetalle.Hidden = true;
                btnActivarDetalle.Hidden = true;
                btnRefrescarDetalle.Hidden = false;
                btnDescargarDetalle.Hidden = true;
            }
            else if (ListaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_VEX_PROYECTOS_SECCIONES))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnActivar.Hidden = false;
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

        private List<Data.VexProyectos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lClienteID)
        {
            List<Data.VexProyectos> ListaDatos;
            VexProyectosController CVexProyectos = new VexProyectosController();

            try
            {
                if (ClienteID != 0)
                {
                    ListaDatos = CVexProyectos.GetItemsWithExtNetFilterList<Data.VexProyectos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        private List<Data.VexSecciones> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        {
            List<Data.VexSecciones> ListaDatos;

            try
            {
                VexSeccionesController CVexSecciones = new VexSeccionesController();

                ListaDatos = CVexSecciones.GetItemsWithExtNetFilterList<Data.VexSecciones>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "VexProyectoID == " + lMaestroID);

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
        public DirectResponse AgregarEditar(bool agregar)
        {
            DirectResponse direct = new DirectResponse();

            VexProyectosController cVexProyectos = new VexProyectosController();

            try

            {
                long cliID = long.Parse(hdCliID.Value.ToString());
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.VexProyectos oDato;
                    oDato = cVexProyectos.GetItem(S);

                    if (oDato.Codigo == txtCodigo.Text && oDato.VexProyecto == txtVexProyecto.Text)
                    {
                        oDato.VexProyecto = txtVexProyecto.Text;
                        oDato.Codigo = txtCodigo.Text;
                    }
                    else
                    {
                        if (cVexProyectos.RegistroDuplicado(txtCodigo.Text, txtVexProyecto.Text, cliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.VexProyecto = txtVexProyecto.Text;
                            oDato.Codigo = txtCodigo.Text;
                        }
                    }
                    if (cVexProyectos.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }

                }
                else
                {
                    if (cVexProyectos.RegistroDuplicado(txtCodigo.Text, txtVexProyecto.Text, cliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.VexProyectos oDato = new Data.VexProyectos();
                        oDato.VexProyecto = txtVexProyecto.Text;
                        oDato.Codigo = txtCodigo.Text;
                        oDato.Activo = true;

                        if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                        {
                            oDato.ClienteID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.ClienteID = cliID;
                        }

                        if (cVexProyectos.AddItem(oDato) != null)
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

            VexProyectosController cVexProyectos = new VexProyectosController();


            try

            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.VexProyectos oDato = cVexProyectos.GetItem(S);

                txtVexProyecto.Text = oDato.VexProyecto;
                txtCodigo.Text = oDato.Codigo;

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
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            VexProyectosController CVexProyectos = new VexProyectosController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.VexProyectos oDato = CVexProyectos.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (CVexProyectos.UpdateItem(oDato))
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

            CVexProyectos = null;
            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            VexProyectosController CVexProyectos = new VexProyectosController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CVexProyectos.DeleteItem(lID))
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

        #endregion

        #region GESTION DETALLE

        [DirectMethod()]
        public DirectResponse mostrarDetalle(long moduloID)
        {
            DirectResponse direct = new DirectResponse();
            direct.Result = "";
            direct.Success = true;

            try
            {
                storeDetalle.DataBind();
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
        public DirectResponse AgregarEditarDetalle(bool agregar)
        {
            DirectResponse direct = new DirectResponse();
            VexSecciones oDato = new VexSecciones();
            VexSeccionesController cController = new VexSeccionesController();

            try

            {
                long MaestroID = Convert.ToInt64(ModuloID.Value);
                long cliID = long.Parse(hdCliID.Value.ToString()); ;
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelectDetalle.SelectedRecordID);
                    oDato = cController.GetItem(S);



                    if (oDato.CodigoSeccion == txtCodigoSeccion.Text && oDato.VexSeccion == txtVexSeccion.Text)
                    {
                        oDato.VexSeccion = txtVexSeccion.Text;
                        oDato.CodigoSeccion = txtCodigoSeccion.Text;
                    }
                    else
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());
                        if (cController.RegistroDuplicado(txtCodigoSeccion.Text, txtVexSeccion.Text, MaestroID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.VexSeccion = txtVexSeccion.Text;
                            oDato.CodigoSeccion = txtCodigoSeccion.Text;
                        }
                    }
                    if (cController.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));

                    }

                }
                else
                {
                    cliID = long.Parse(hdCliID.Value.ToString());

                    if (cController.RegistroDuplicado(txtCodigoSeccion.Text, txtVexSeccion.Text, MaestroID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        oDato.VexProyectoID = Convert.ToInt64(GridRowSelect.SelectedRecordID);
                        oDato.VexSeccion = txtVexSeccion.Text;
                        oDato.CodigoSeccion = txtCodigoSeccion.Text;
                        oDato.Activo = true;
                        oDato.ClienteID = cliID;

                        if (cController.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));

                        }

                    }
                }
                storeDetalle.DataBind();

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
            DirectResponse ajax = new DirectResponse();

            try

            {
                long ID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                Data.VexSecciones oDato;
                VexSeccionesController fController = new VexSeccionesController();

                oDato = fController.GetItem(ID);

                txtVexSeccion.Text = oDato.VexSeccion;
                txtCodigoSeccion.Text = oDato.CodigoSeccion;

                winGestionDetalle.Show();
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return ajax;
            }

            ajax.Success = true;
            ajax.Result = "";

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse EliminarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            VexSeccionesController CVexSecciones = new VexSeccionesController();

            direct.Result = "";
            direct.Success = true;


            long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

            try
            {
                if (CVexSecciones.DeleteItem(lID))
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
            VexSeccionesController CVexSecciones = new VexSeccionesController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.VexSecciones oDato = CVexSecciones.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (CVexSecciones.UpdateItem(oDato))
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

    }
}