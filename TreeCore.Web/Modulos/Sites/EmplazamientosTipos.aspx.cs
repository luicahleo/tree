using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using TreeCore.Clases;

namespace TreeCore.ModGlobal
{
    public partial class EmplazamientosTipos : TreeCore.Page.BasePageExtNet
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
                Comun.CreateGridFilters(gridFiltersTiposEdificios, storeTiposEdificios, gridTiposEdificios.ColumnModel, listaIgnore, _Locale);
                Comun.CreateGridFilters(gridFiltersTiposEdificiosLbres, storeTiposEdificiosLibres, gridTiposEdificiosLibres.ColumnModel, listaIgnore, _Locale);
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
                        List<Data.EmplazamientosTipos> listaDatos;
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

        private List<Data.EmplazamientosTipos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.EmplazamientosTipos> listaDatos;
            EmplazamientosTiposController CEmplazamientosTipos = new EmplazamientosTiposController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CEmplazamientosTipos.GetItemsWithExtNetFilterList<Data.EmplazamientosTipos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        #region TIPOS EDIFICIOS

        protected void storeTiposEdificios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                EmplazamientosTiposEmplazamientosTiposEdificiosController cContratacionesEdificios = new EmplazamientosTiposEmplazamientosTiposEdificiosController();
                try
                {
                    List<Data.EmplazamientosTiposEdificios> lista;
                    lista = cContratacionesEdificios.tiposEdificiosAsignados(Int64.Parse(GridRowSelect.SelectedRecordID));
                    if (lista != null)
                    {
                        storeTiposEdificios.DataSource = lista;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region TIPOS EDIFICIOS LIBRES

        /// <summary>
        /// Cargar los Edificios Sin asignar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void storeTiposEdificiosLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                EmplazamientosTiposEmplazamientosTiposEdificiosController cContratacionesEdificios = new EmplazamientosTiposEmplazamientosTiposEdificiosController();
                try
                {
                    List<Data.EmplazamientosTiposEdificios> lista;
                    lista = cContratacionesEdificios.tiposEdificiosNoAsignado(Int64.Parse(GridRowSelect.SelectedRecordID));
                    if (lista != null)
                    {
                        storeTiposEdificiosLibres.DataSource = lista;
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
            EmplazamientosTiposController cTipos = new EmplazamientosTiposController();
            long lCliID = 0;
            InfoResponse oResponse;

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.EmplazamientosTipos oDato;
                    oDato = cTipos.GetItem(lS);
                    oDato.Tipo = txtTipo.Text;

                    oResponse = cTipos.Update(oDato);
                    if (oResponse.Result)
                    {
                        oResponse = cTipos.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cTipos.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cTipos.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());
                    Data.EmplazamientosTipos oDato = new Data.EmplazamientosTipos();
                    oDato.Tipo = txtTipo.Text;
                    oDato.Activo = true;
                    oDato.ClienteID = lCliID;

                    oResponse = cTipos.Add(oDato);
                    if (oResponse.Result)
                    {
                        oResponse = cTipos.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cTipos.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cTipos.DiscardChanges();
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

        [DirectMethod]
        public DirectResponse AgregarTiposEdificios()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosTiposEmplazamientosTiposEdificiosController cEmplazamientosEdificios = new EmplazamientosTiposEmplazamientosTiposEdificiosController();
            InfoResponse oResponse;

            try
            {
                foreach (SelectedRow selec in GridRowSelectTiposEdificiosLibres.SelectedRows)
                {
                    Data.EmplazamientosTiposEmplazamientosTiposEdificios oDato = new Data.EmplazamientosTiposEmplazamientosTiposEdificios
                    {
                        EmplazamientoTipoEdificioID = Int64.Parse(selec.RecordID),
                        EmplazamientoTipoID = Int64.Parse(GridRowSelect.SelectedRecordID),
                        Activo = true
                    };
                    oResponse = cEmplazamientosEdificios.Add(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cEmplazamientosEdificios.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storeTiposEdificios.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cEmplazamientosEdificios.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cEmplazamientosEdificios.DiscardChanges();
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
        /// Quitar tipo Edificios al tipo de contratacion
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public DirectResponse QuitarTipoEdificio()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosTiposEmplazamientosTiposEdificiosController cEmplazamientosEdificios = new EmplazamientosTiposEmplazamientosTiposEdificiosController();
            InfoResponse oResponse;

            try
            {
                foreach (SelectedRow selec in GridRowSelectTiposEdificios.SelectedRows)
                {

                    Data.EmplazamientosTiposEmplazamientosTiposEdificios EliminadoEdificio = cEmplazamientosEdificios.GetByTipoEdificioTipoID(long.Parse(selec.RecordID), long.Parse(GridRowSelect.SelectedRecordID));
                    oResponse = cEmplazamientosEdificios.Delete(EliminadoEdificio);

                    if (oResponse.Result)
                    {
                        oResponse = cEmplazamientosEdificios.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storeTiposEdificios.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cEmplazamientosEdificios.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cEmplazamientosEdificios.DiscardChanges();
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
        /// Agregar tipo Edificios al tipo de contratacion
        /// </summary>
        /// <returns></returns>

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();

            EmplazamientosTiposController cTipos = new EmplazamientosTiposController();

            try

            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.EmplazamientosTipos oDato;
                oDato = cTipos.GetItem(lS);
                txtTipo.Text = oDato.Tipo;
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
            EmplazamientosTiposController CEmplazamientosTipos = new EmplazamientosTiposController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.EmplazamientosTipos oDato = CEmplazamientosTipos.GetItem(lID);
                oResponse = CEmplazamientosTipos.SetDefecto(oDato);
                if (oResponse.Result)
                {
                    oResponse = CEmplazamientosTipos.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        CEmplazamientosTipos.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    CEmplazamientosTipos.DiscardChanges();
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
            EmplazamientosTiposController CEmplazamientosTipos = new EmplazamientosTiposController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.EmplazamientosTipos oDato = CEmplazamientosTipos.GetItem(lID);
                oResponse = CEmplazamientosTipos.Delete(oDato);
                if (oResponse.Result)
                {
                    oResponse = CEmplazamientosTipos.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        CEmplazamientosTipos.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    CEmplazamientosTipos.DiscardChanges();
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
            EmplazamientosTiposController cController = new EmplazamientosTiposController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.EmplazamientosTipos oDato = cController.GetItem(lID);
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


    }
}