using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TreeCore.Clases;

namespace TreeCore.ModGlobal
{
    public partial class TiposDatos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();
        long lMaestroID = 0;

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
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                Comun.CreateGridFilters(gridFiltersDetalle, storeDetalle, gridDetalle.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));
                Comun.CreateGridFilters(gridFiltersOperadores, storeTiposDatosOperadores, gridOperadores.ColumnModel, listaIgnore, _Locale);
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

                #region COMBOS
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
                        bool bActivo = Request.QueryString["aux"] == "true";
                        string sFiltro = Request.QueryString["filtro"];
                        string sModuloID = Request.QueryString["aux"].ToString();
                        string ventanaDetalle = Request.QueryString["aux3"].ToString();
                        long CliID = long.Parse(Request.QueryString["cliente"]);

                        int iCount = 0;

                        #region MAESTRO
                        if (sModuloID == null || sModuloID == "" || sModuloID == "-1" || sModuloID == "true" || sModuloID == "false")
                        {

                            List<Data.Vw_TiposDatos> listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID, bActivo);

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }

                            #region ESTADISTICAS
                            try
                            {
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            #endregion
                        }
                        #endregion

                        #region DETALLE
                        else if (ventanaDetalle == "operador")
                        {
                            List<Data.TiposDatosOperadores> listaDatos = ListaOperadores(0, 0, sOrden, sDir, ref iCount, sFiltro, long.Parse(sModuloID));

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridOperadores.ColumnModel, listaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
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

                            #region ESTADISTICAS
                            try
                            {
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
                            List<Data.TiposDatosPropiedades> listaDatos = ListaDetalle(0, 0, sOrden, sDir, ref iCount, sFiltro, long.Parse(sModuloID));

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridDetalle.ColumnModel, listaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
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

                            #region ESTADISTICAS
                            try
                            {
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }

                            #endregion
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
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> { btnDescargar }},
                { "Post", new List<ComponentBase> { btnAnadir }},
                { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnDefecto }},
                { "Delete", new List<ComponentBase> { btnEliminar }}
            };
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
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()), btnActivo.Pressed);

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

        private List<Data.Vw_TiposDatos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID, bool bEstado)
        {
            List<Data.Vw_TiposDatos> listaDatos;
            TiposDatosController cTiposDatos = new TiposDatosController();

            try
            {
                if (lClienteID.HasValue)
                {
                    if (bEstado)
                    {
                        listaDatos = cTiposDatos.GetItemsWithExtNetFilterList<Data.Vw_TiposDatos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == true && ClienteID == " + lClienteID);
                    }
                    else
                    {
                        listaDatos = cTiposDatos.GetItemsWithExtNetFilterList<Data.Vw_TiposDatos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
                    }
                }
                else
                {
                    listaDatos = null;
                }

                //Filtro resultados KPI
                if (listaDatos != null && listIdsResultadosKPI != null)
                {
                    listaDatos = cTiposDatos.FiltroListaPrincipalByIDs(listaDatos.Cast<object>().ToList(), listIdsResultadosKPI, nameIndiceID).Cast<Data.Vw_TiposDatos>().ToList();
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        protected void storeTiposValores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<object> lObjetos = new List<object>();
                    foreach (var tipoValor in Enum.GetValues(typeof(Comun.TiposValores)))
                    {
                        lObjetos.Add(new { TipoValor = tipoValor.ToString(), TipoValorID = tipoValor.GetHashCode() });
                    }

                    storeTiposValores.DataSource = lObjetos;
                    storeTiposValores.DataBind();
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        protected void storeTiposPropiedades_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<object> lObjetos = new List<object>();
                    foreach (var tipoPropiedad in Enum.GetValues(typeof(Comun.TiposPropiedades)))
                    {
                        lObjetos.Add(new { Propiedad = tipoPropiedad.ToString(), PropiedadID = tipoPropiedad.GetHashCode() });
                    }

                    storeTiposPropiedades.DataSource = lObjetos;
                    storeTiposPropiedades.DataBind();
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        //protected void storeCodigos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        try
        //        {
        //            List<object> lObjetos = new List<object>();
        //            foreach (var codigo in Enum.GetValues(typeof(Comun.TiposCodigos)))
        //            {
        //                lObjetos.Add(new { Codigo = codigo.ToString(), CodigoID = codigo.GetHashCode() });
        //            }

        //            storeCodigos.DataSource = lObjetos;
        //            storeCodigos.DataBind();
        //        }

        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //            string codTit = Util.ExceptionHandler(ex);
        //            MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
        //        }
        //    }
        //}

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
                    string sFiltro = e.Parameters["gridFiltersDetalle"];

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

        private List<Data.TiposDatosPropiedades> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        {
            List<Data.TiposDatosPropiedades> ListaDatos = new List<Data.TiposDatosPropiedades>();
            TiposDatosPropiedadesController cDetalle = new TiposDatosPropiedadesController();

            try
            {
                ListaDatos = cDetalle.GetItemsWithExtNetFilterList<Data.TiposDatosPropiedades>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "TipoDatoID == " + lMaestroID);
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return ListaDatos;
        }
        protected void storeTiposDatosOperadores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort, sDir;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFiltersDetalle"];

                    if (!ModuloID.Value.Equals(""))
                    {
                        lMaestroID = Convert.ToInt64(ModuloID.Value);
                    }

                    var vLista = ListaOperadores(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, lMaestroID);

                    if (vLista != null)
                    {
                        storeTiposDatosOperadores.DataSource = vLista;

                        PageProxy temp;
                        temp = (PageProxy)storeTiposDatosOperadores.Proxy[0];
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

        private List<Data.TiposDatosOperadores> ListaOperadores(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        {
            List<Data.TiposDatosOperadores> ListaDatos = new List<Data.TiposDatosOperadores>();
            TiposDatosOperadoresController cDetalle = new TiposDatosOperadoresController();

            try
            {
                ListaDatos = cDetalle.GetItemsWithExtNetFilterList<Data.TiposDatosOperadores>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "TipoDatoID == " + lMaestroID);
            }

            catch (Exception ex)
            {
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

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            TiposDatosController cTipoDato = new TiposDatosController();
            InfoResponse oResponse;
            long cliID = 0;

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.TiposDatos oDato = cTipoDato.GetItem(lS);

                    if (oDato.TipoDato == txtTipoDato.Text)
                    {
                        oDato.Codigo = txtCodigo2.Text;
                        oDato.TipoDato = txtTipoDato.Text;
                    }
                    else
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());
                        oDato.Codigo = txtCodigo2.Text;
                        oDato.TipoDato = txtTipoDato.Text;
                    }

                    oResponse = cTipoDato.Update(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cTipoDato.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();
                            direct.Success = true;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                        else
                        {
                            cTipoDato.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cTipoDato.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cliID = long.Parse(hdCliID.Value.ToString());

                    Data.TiposDatos oDato = new Data.TiposDatos
                    {
                        TipoDato = txtTipoDato.Text,
                        Codigo = txtCodigo2.Text,
                        Activo = true,
                        ClienteID = cliID
                    };

                    oResponse = cTipoDato.Add(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cTipoDato.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();
                            direct.Success = true;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                        else
                        {
                            cTipoDato.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cTipoDato.DiscardChanges();
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

            TiposDatosController cTipoDato = new TiposDatosController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.TiposDatos oDato;
                oDato = cTipoDato.GetItem(lS);
                txtTipoDato.Text = oDato.TipoDato;
                txtCodigo2.Text = oDato.Codigo;

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
            TiposDatosController CTiposDatos = new TiposDatosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.TiposDatos oDato = CTiposDatos.GetItem(lID);

                oResponse = CTiposDatos.SetDefecto(oDato);

                if (oResponse.Result)
                {
                    oResponse = CTiposDatos.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        CTiposDatos.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    CTiposDatos.DiscardChanges();
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
            TiposDatosController CTiposDatos = new TiposDatosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.TiposDatos oDato = CTiposDatos.GetItem(lID);
                oResponse = CTiposDatos.Delete(oDato);

                if (oResponse.Result)
                {
                    oResponse = CTiposDatos.SubmitChanges();

                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        CTiposDatos.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    CTiposDatos.DiscardChanges();
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
            TiposDatosController cController = new TiposDatosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.TiposDatos oDato = cController.GetItem(lID);
                oResponse = cController.ModificarActivar(oDato);

                if (oResponse.Result)
                {
                    oResponse = cController.SubmitChanges();

                    if (oResponse.Result)
                    {
                        storePrincipal.DataBind();
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
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

        #region GESTION DETALLE

        [DirectMethod()]
        public DirectResponse AgregarEditarDetalle(bool bAgregar)
        {
            DirectResponse ajax = new DirectResponse();
            TiposDatosPropiedadesController cDetalle = new TiposDatosPropiedadesController();
            InfoResponse oResponse;

            try
            {
                long lIDMaestro = Int64.Parse(GridRowSelect.SelectedRecordID);

                if (!bAgregar)
                {
                    long lIDDetalle = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

                    Data.TiposDatosPropiedades oDato = cDetalle.GetItem(lIDDetalle);

                    if (txtValorDefecto.Text != "")
                    {
                        oDato.ValorDefecto = txtValorDefecto.Text;
                    }

                    if (cmbTiposValores.SelectedItem.Value != "")
                    {
                        oDato.TipoValor = Convert.ToString(cmbTiposValores.SelectedItem.Text);
                    }

                    if (txtNombre.Text != "")
                    {
                        oDato.Nombre = txtNombre.Text;
                    }

                    oDato.AplicaReglas = chkAplicaReglas.Checked;

                    if (oDato.Codigo == cmbTiposPropiedades.SelectedItem.Text)
                    {
                        oResponse = cDetalle.Update(oDato);

                        if (oResponse.Result)
                        {
                            oResponse = cDetalle.SubmitChanges();

                            if (oResponse.Result)
                            {
                                log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                                storeDetalle.DataBind();

                                ajax.Success = true;
                                ajax.Result = "";
                            }
                            else
                            {
                                cDetalle.DiscardChanges();
                                ajax.Success = false;
                                ajax.Result = GetGlobalResource(oResponse.Description);
                            }
                        }
                        else
                        {
                            cDetalle.DiscardChanges();
                            ajax.Success = false;
                            ajax.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        if (cmbTiposPropiedades.SelectedItem.Value != "")
                        {
                            oDato.Codigo = Convert.ToString(cmbTiposPropiedades.SelectedItem.Text);
                        }

                        oResponse = cDetalle.Update(oDato);

                        if (oResponse.Result)
                        {
                            oResponse = cDetalle.SubmitChanges();

                            if (oResponse.Result)
                            {
                                log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                                storeDetalle.DataBind();

                                ajax.Success = true;
                                ajax.Result = "";
                            }
                            else
                            {
                                cDetalle.DiscardChanges();
                                ajax.Success = false;
                                ajax.Result = GetGlobalResource(oResponse.Description);
                            }
                        }
                        else
                        {
                            cDetalle.DiscardChanges();
                            ajax.Success = false;
                            ajax.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                }
                else
                {
                    Data.TiposDatosPropiedades oDato = new Data.TiposDatosPropiedades();

                    oDato.TipoDatoID = lIDMaestro;

                    if (txtValorDefecto.Text != "")
                    {
                        oDato.ValorDefecto = txtValorDefecto.Text;
                    }

                    if (cmbTiposValores.SelectedItem.Value != "")
                    {
                        oDato.TipoValor = Convert.ToString(cmbTiposValores.SelectedItem.Text);
                    }
                    if (cmbTiposPropiedades.SelectedItem.Value != "")
                    {
                        oDato.Codigo = Convert.ToString(cmbTiposPropiedades.SelectedItem.Text);
                    }

                    if (txtNombre.Text != "")
                    {
                        oDato.Nombre = txtNombre.Text;
                    }

                    oDato.Activo = true;
                    oDato.AplicaReglas = chkAplicaReglas.Checked;

                    oResponse = cDetalle.Add(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cDetalle.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storeDetalle.DataBind();

                            ajax.Success = true;
                            ajax.Result = "";
                        }
                        else
                        {
                            cDetalle.DiscardChanges();
                            ajax.Success = false;
                            ajax.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cDetalle.DiscardChanges();
                        ajax.Success = false;
                        ajax.Result = GetGlobalResource(oResponse.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return ajax;
            }

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditarDetalle()
        {
            DirectResponse ajax = new DirectResponse();

            try
            {
                long lIDDetalle = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                TiposDatosPropiedadesController cDetalle = new TiposDatosPropiedadesController();

                Data.TiposDatosPropiedades oDato = cDetalle.GetItem(lIDDetalle);

                chkAplicaReglas.Checked = (oDato.AplicaReglas);

                if (oDato.ValorDefecto != null)
                {
                    txtValorDefecto.Text = oDato.ValorDefecto;
                }

                if (oDato.TipoValor != null)
                {
                    cmbTiposValores.Select(oDato.TipoValor);
                }

                if (oDato.Nombre != null)
                {
                    txtNombre.Text = oDato.Nombre;
                }

                if (oDato.Codigo != null)
                {
                    cmbTiposPropiedades.Select(oDato.Codigo);
                }

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
            TiposDatosPropiedadesController cDetalle = new TiposDatosPropiedadesController();
            InfoResponse oResponse;

            try
            {
                long lIDDetalle = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                Data.TiposDatosPropiedades oDato = cDetalle.GetItem(lIDDetalle);
                oResponse = cDetalle.Delete(oDato);

                if (oResponse.Result)
                {
                    oResponse = cDetalle.SubmitChanges();

                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storeDetalle.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cDetalle.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cDetalle.DiscardChanges();
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
        public DirectResponse ActivarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            TiposDatosPropiedadesController cDetalle = new TiposDatosPropiedadesController();
            InfoResponse oResponse;

            try
            {
                long lIDDetalle = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                Data.TiposDatosPropiedades oDato = cDetalle.GetItem(lIDDetalle);
                oResponse = cDetalle.ModificarActivar(oDato);

                if (oResponse.Result)
                {
                    oResponse = cDetalle.SubmitChanges();

                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storeDetalle.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cDetalle.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cDetalle.DiscardChanges();
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

        #region GESTION OPERADOR

        [DirectMethod()]
        public DirectResponse AgregarEditarOperador(bool bAgregar)
        {
            DirectResponse ajax = new DirectResponse();
            TiposDatosOperadoresController cDetalle = new TiposDatosOperadoresController();
            InfoResponse oResponse;

            try
            {
                long lIDMaestro = Int64.Parse(GridRowSelect.SelectedRecordID);

                if (!bAgregar)
                {
                    long lIDDetalle = Int64.Parse(GridRowSelectOperadores.SelectedRecordID);

                    Data.TiposDatosOperadores oDato;
                    oDato = cDetalle.GetItem(lIDDetalle);
                    if (oDato.Nombre != txtNombreDatoOperador.Text)
                    {
                        if (txtNombreDatoOperador.Text != "")
                        {
                            oDato.Nombre = txtNombreDatoOperador.Text;
                        }
                    }

                    if (txtClaveRecurso.Text != "")
                    {
                        oDato.ClaveRecurso = txtClaveRecurso.Text;
                    }
                    if (txtOperador.Text != "")
                    {
                        oDato.Operador = txtOperador.Text;
                    }
                    oDato.RequiereValor = (bool)chkRequiereValor.Value;

                    oResponse = cDetalle.Update(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cDetalle.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storeTiposDatosOperadores.DataBind();

                            ajax.Success = true;
                            ajax.Result = "";
                        }
                        else
                        {
                            cDetalle.DiscardChanges();
                            ajax.Success = false;
                            ajax.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cDetalle.DiscardChanges();
                        ajax.Success = false;
                        ajax.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    Data.TiposDatosOperadores oDato = new Data.TiposDatosOperadores();

                    oDato.TipoDatoID = lIDMaestro;
                    oDato.Nombre = txtNombreDatoOperador.Text;
                    oDato.ClaveRecurso = txtClaveRecurso.Text;
                    oDato.Operador = txtOperador.Text;
                    oDato.RequiereValor = (bool)chkRequiereValor.Value;
                    oDato.Activo = true;

                    oResponse = cDetalle.Add(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cDetalle.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storeTiposDatosOperadores.DataBind();

                            ajax.Success = true;
                            ajax.Result = "";
                        }
                        else
                        {
                            cDetalle.DiscardChanges();
                            ajax.Success = false;
                            ajax.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cDetalle.DiscardChanges();
                        ajax.Success = false;
                        ajax.Result = GetGlobalResource(oResponse.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return ajax;
            }

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditarOperador()
        {
            DirectResponse ajax = new DirectResponse();

            try
            {
                long lIDDetalle = Int64.Parse(GridRowSelectOperadores.SelectedRecordID);
                TiposDatosOperadoresController cDetalle = new TiposDatosOperadoresController();

                Data.TiposDatosOperadores oDato = cDetalle.GetItem(lIDDetalle);

                if (oDato.Nombre != null)
                {
                    txtNombreDatoOperador.Text = oDato.Nombre;
                }

                if (oDato.ClaveRecurso != null)
                {
                    txtClaveRecurso.Text = oDato.ClaveRecurso;
                }

                if (oDato.Operador != null)
                {
                    txtOperador.Text = oDato.Operador;
                }

                chkRequiereValor.SetValue(oDato.RequiereValor);

                winGestionOperador.Show();
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
        public DirectResponse EliminarOperador()
        {
            DirectResponse direct = new DirectResponse();
            TiposDatosOperadoresController cDetalle = new TiposDatosOperadoresController();
            InfoResponse oResponse;

            try
            {
                long lIDDetalle = Int64.Parse(GridRowSelectOperadores.SelectedRecordID);
                Data.TiposDatosOperadores oDato = cDetalle.GetItem(lIDDetalle);
                oResponse = cDetalle.Delete(oDato);

                if (oResponse.Result)
                {
                    oResponse = cDetalle.SubmitChanges();

                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storeTiposDatosOperadores.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cDetalle.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cDetalle.DiscardChanges();
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
        public DirectResponse ActivarOperador()
        {
            DirectResponse direct = new DirectResponse();
            TiposDatosOperadoresController cDetalle = new TiposDatosOperadoresController();
            InfoResponse oResponse;

            try
            {
                long lIDDetalle = Int64.Parse(GridRowSelectOperadores.SelectedRecordID);
                Data.TiposDatosOperadores oDato = cDetalle.GetItem(lIDDetalle);
                oResponse = cDetalle.ModificarActivar(oDato);

                if (oResponse.Result)
                {
                    oResponse = cDetalle.SubmitChanges();

                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storeTiposDatosOperadores.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cDetalle.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cDetalle.DiscardChanges();
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
    }
}