using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SqlClient;
using TreeCore.Clases;


namespace TreeCore.ModGlobal
{
    public partial class SAPIFRS16TratamientosPrevios : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));


                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
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
                        List<Data.IFRS16TratamientosPrevios> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        string[] param = Request.QueryString["aux"].ToString().Split(';');
                        int iCount = 0;
                        btnActivos.Pressed = Convert.ToBoolean(param[0]);
                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID, btnActivos.Pressed);



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
            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_SAP_TRATAMIENTOS_PREVIOS))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnActivos.Hidden = false;
                btnDescargar.Hidden = true;
                btnActivar.Hidden = true;
                btnDefecto.Hidden = true;
            }
            else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_SAP_TRATAMIENTOS_PREVIOS))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnActivos.Hidden = false;
                btnDescargar.Hidden = false;
                btnActivar.Hidden = false;
                btnDefecto.Hidden = false;
            }
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



                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()), btnActivos.Pressed);

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

        private List<Data.IFRS16TratamientosPrevios> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID, bool Activos)
        {
            List<Data.IFRS16TratamientosPrevios> listaDatos;
            IFRS16TratamientosPreviosController CIFRS16TratamientosPrevios = new IFRS16TratamientosPreviosController();

            try
            {
                if (lClienteID.HasValue && lClienteID != 0)
                {
                    if (!Activos)
                    {
                        listaDatos = CIFRS16TratamientosPrevios.GetItemsWithExtNetFilterList<Data.IFRS16TratamientosPrevios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
                    }
                    else
                    {
                        listaDatos = CIFRS16TratamientosPrevios.GetItemsWithExtNetFilterList<Data.IFRS16TratamientosPrevios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID + " && Activo");
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
                    List<Data.Clientes> listaClientes = ListaClientes();

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
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            IFRS16TratamientosPreviosController cTratamientosPrevios = new IFRS16TratamientosPreviosController();
            long cliID;
            InfoResponse oResponse;

            try
            {

                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.IFRS16TratamientosPrevios oDato = cTratamientosPrevios.GetItem(lS);

                    if (oDato.IFRS16TratamientoPrevio == txtTratamientosPrevios.Text)
                    {
                        oDato.IFRS16TratamientoPrevio = txtTratamientosPrevios.Text;
                        oDato.Descripcion = txtDescripcion.Text;
                    }
                    else
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());

                        oDato.IFRS16TratamientoPrevio = txtTratamientosPrevios.Text;
                        oDato.Descripcion = txtDescripcion.Text;
                    }

                    oResponse = cTratamientosPrevios.Update(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cTratamientosPrevios.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cTratamientosPrevios.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cTratamientosPrevios.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cliID = long.Parse(hdCliID.Value.ToString());

                    Data.IFRS16TratamientosPrevios oDato = new Data.IFRS16TratamientosPrevios
                    {
                        IFRS16TratamientoPrevio = txtTratamientosPrevios.Text,
                        Descripcion = txtDescripcion.Text,
                        Activo = true,
                        ClienteID = cliID
                    };

                    oResponse = cTratamientosPrevios.Add(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cTratamientosPrevios.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cTratamientosPrevios.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cTratamientosPrevios.DiscardChanges();
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
            IFRS16TratamientosPreviosController cTratamientosPrevios = new IFRS16TratamientosPreviosController();

            try
            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.IFRS16TratamientosPrevios oDato;
                oDato = cTratamientosPrevios.GetItem(S);
                txtTratamientosPrevios.Text = oDato.IFRS16TratamientoPrevio;
                txtDescripcion.Text = oDato.Descripcion;

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
            IFRS16TratamientosPreviosController CIFRS16TratamientosPrevios = new IFRS16TratamientosPreviosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.IFRS16TratamientosPrevios oDato = CIFRS16TratamientosPrevios.GetItem(lID);
                oResponse = CIFRS16TratamientosPrevios.SetDefecto(oDato);

                if (oResponse.Result)
                {
                    oResponse = CIFRS16TratamientosPrevios.SubmitChanges();

                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        CIFRS16TratamientosPrevios.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    CIFRS16TratamientosPrevios.DiscardChanges();
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
            IFRS16TratamientosPreviosController CIFRS16TratamientosPrevios = new IFRS16TratamientosPreviosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.IFRS16TratamientosPrevios oDato = CIFRS16TratamientosPrevios.GetItem(lID);
                oResponse = CIFRS16TratamientosPrevios.Delete(oDato);

                if (oResponse.Result)
                {
                    oResponse = CIFRS16TratamientosPrevios.SubmitChanges();

                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        CIFRS16TratamientosPrevios.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    CIFRS16TratamientosPrevios.DiscardChanges();
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
            IFRS16TratamientosPreviosController cController = new IFRS16TratamientosPreviosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.IFRS16TratamientosPrevios oDato = cController.GetItem(lID);
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

        #endregion

        #region FUNCTIONS
        protected void btnActivos_Toggle(Object sender, DirectEventArgs e)
        {
            storePrincipal.DataBind();
        }
        #endregion
    }
}