using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Reflection;
using System.Data.SqlClient;
using TreeCore.CapaNegocio.Global.Administracion;
using static TreeCore.Global;
using TreeCore.Clases;

namespace TreeCore.ModGlobal
{
    public partial class EmplazamientosTiposEdificios : TreeCore.Page.BasePageExtNet
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
                Comun.CreateGridFilters(gridFiltersEdificiosPaises, storeTiposEdificiosPaises, GridTipoEdificiosPaises.ColumnModel, listaIgnore, _Locale);
                Comun.CreateGridFilters(gridFiltersTiposEstructuras, storeTiposEstructuras, gridTiposEstructuras.ColumnModel, listaIgnore, _Locale);
                Comun.CreateGridFilters(gridFiltersTiposEstructurasLibre, storeTiposEstructurasLibres, gridTiposEstructurasLibres.ColumnModel, listaIgnore, _Locale);
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
                        List<Data.EmplazamientosTiposEdificios> listaDatos;
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
            { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnDefecto, btnCosteDesmantelamiento }},
            { "Delete", new List<ComponentBase> { btnEliminar }}
        };
        }

        #endregion

        #region STORES

        #region PAISES

        protected void storePaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var ls = ListaPaises();
                    if (ls != null)
                    {
                        storePaises.DataSource = ls;
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

        private List<Data.Paises> ListaPaises()
        {
            List<Data.Paises> datos;
            PaisesController cPaises = new PaisesController();
            try
            {
                datos = cPaises.GetActivos(long.Parse(hdCliID.Value.ToString()));
            }
            catch (Exception)
            {
                datos = null;
            }
            return datos;
        }

        #endregion

        #region MONEDAS

        protected void storeMonedas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var ls = ListaMonedas();

                    if (ls != null)
                    {
                        storeMonedas.DataSource = ls;
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

        private List<Data.Monedas> ListaMonedas()
        {
            List<Data.Monedas> datos;
            MonedasController cMonedas = new MonedasController();

            try
            {
                datos = cMonedas.GetAllMonedas();
            }
            catch (Exception)
            {
                datos = null;
            }

            return datos;
        }

        #endregion

        #region COSTES DESMANTELAMIENTO

        protected void storeTiposEdificiosPaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    long lProID = long.Parse(GridRowSelect.SelectedRecordID);
                    var ls = ListaProveedoresCostes(lProID);

                    if (ls != null)
                    {
                        storeTiposEdificiosPaises.DataSource = ls;
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

        private List<Data.Vw_EmplazamientosTiposEdificiosPaises> ListaProveedoresCostes(long lTipoEdificioID)
        {
            List<Data.Vw_EmplazamientosTiposEdificiosPaises> listaDatos;
            EmplazamientosTiposEdificiosPaisesController cProCC = new EmplazamientosTiposEdificiosPaisesController();

            try
            {
                listaDatos = cProCC.GetTipoEdificioID(lTipoEdificioID);
            }
            catch (Exception)
            {
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region AÑOS

        protected void storeAnualidad_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Anyos> lAnyos = new List<Anyos>();
                    Anyos anyo;

                    int iRangoSuperior = DateTime.Now.Date.Year + 20;
                    int iRangoInferior = DateTime.Now.Date.Year - 20;

                    for (int i = iRangoSuperior; i > iRangoInferior; i--)
                    {
                        anyo = new Anyos();
                        anyo.Anualidad = i.ToString();
                        anyo.AnualidadID = i;

                        lAnyos.Add(anyo);
                    }

                    if (lAnyos != null)
                    {
                        storeAnualidad.DataSource = lAnyos;
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

        #region TIPOS ESTRUCTURAS

        protected void storeTiposEstructuras_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                EmplazamientosTiposEdificiosEmplazamientosTiposEstructurasController cEdificiosEstructuras = new EmplazamientosTiposEdificiosEmplazamientosTiposEstructurasController();

                try
                {
                    List<Data.EmplazamientosTiposEstructuras> listaDatos;
                    listaDatos = cEdificiosEstructuras.tiposEstructurasAsignadas(Int64.Parse(GridRowSelect.SelectedRecordID));

                    if (listaDatos != null)
                    {
                        storeTiposEstructuras.DataSource = listaDatos;
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

        #region TIPOS ESTRUCTURAS LIBRES

        protected void storeTiposEstructurasLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                EmplazamientosTiposEdificiosEmplazamientosTiposEstructurasController cEdificiosEstructuras = new EmplazamientosTiposEdificiosEmplazamientosTiposEstructurasController();

                try
                {
                    List<Data.EmplazamientosTiposEstructuras> listaDatos;
                    listaDatos = cEdificiosEstructuras.tiposEstructurasNoAsignadas(Int64.Parse(GridRowSelect.SelectedRecordID));

                    if (listaDatos != null)
                    {
                        storeTiposEstructurasLibres.DataSource = listaDatos;
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

        #region PRINCIPAL

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

        private List<Data.EmplazamientosTiposEdificios> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.EmplazamientosTiposEdificios> listaDatos;
            EmplazamientosTiposEdificiosController CEmplazamientosTiposEdificios = new EmplazamientosTiposEdificiosController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CEmplazamientosTiposEdificios.GetItemsWithExtNetFilterList<Data.EmplazamientosTiposEdificios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosTiposEdificiosController cTipos = new EmplazamientosTiposEdificiosController();
            long lCliID = 0;
            InfoResponse oResponse;

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.EmplazamientosTiposEdificios oDato;
                    oDato = cTipos.GetItem(lS);
                    oDato.TipoEdificio = txtTipoEdificio.Text;
                    oDato.CostoPorDesmantelamiento = Convert.ToDouble(txtCostoDesmantelamiento.Text);

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
                    Data.EmplazamientosTiposEdificios oDato = new Data.EmplazamientosTiposEdificios();
                    oDato.TipoEdificio = txtTipoEdificio.Text;
                    oDato.CostoPorDesmantelamiento = Convert.ToDouble(txtCostoDesmantelamiento.Text);
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

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosTiposEdificiosController cTipos = new EmplazamientosTiposEdificiosController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);
                Data.EmplazamientosTiposEdificios oDato;

                oDato = cTipos.GetItem(lS);
                txtTipoEdificio.Text = oDato.TipoEdificio;

                if (oDato.CostoPorDesmantelamiento != null)
                {
                    txtCostoDesmantelamiento.Text = ((double)oDato.CostoPorDesmantelamiento).ToString();
                }
                else
                {
                    txtCostoDesmantelamiento.Text = "0";
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
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosTiposEdificiosController cEmplazamientosTiposEdificios = new EmplazamientosTiposEdificiosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.EmplazamientosTiposEdificios oDato = cEmplazamientosTiposEdificios.GetItem(lID);
                oResponse = cEmplazamientosTiposEdificios.SetDefecto(oDato);
                if (oResponse.Result)
                {
                    oResponse = cEmplazamientosTiposEdificios.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cEmplazamientosTiposEdificios.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cEmplazamientosTiposEdificios.DiscardChanges();
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
            EmplazamientosTiposEdificiosController cEmplazamientosTiposEdificios = new EmplazamientosTiposEdificiosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.EmplazamientosTiposEdificios oDato = cEmplazamientosTiposEdificios.GetItem(lID);
                oResponse = cEmplazamientosTiposEdificios.Delete(oDato);
                if (oResponse.Result)
                {
                    oResponse = cEmplazamientosTiposEdificios.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cEmplazamientosTiposEdificios.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cEmplazamientosTiposEdificios.DiscardChanges();
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
            EmplazamientosTiposEdificiosController cController = new EmplazamientosTiposEdificiosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.EmplazamientosTiposEdificios oDato = cController.GetItem(lID);
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

        #region DIRECT METHOD TIPO ESTRUCTURA

        [DirectMethod]
        public DirectResponse AgregarTiposEstructuras()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosTiposEdificiosEmplazamientosTiposEstructurasController cEdificiosEstructuras = new EmplazamientosTiposEdificiosEmplazamientosTiposEstructurasController();
            InfoResponse oResponse;

            try
            {
                foreach (SelectedRow selec in GridRowSelectTiposEstructurasLibre.SelectedRows)
                {
                    Data.EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras oDato = new Data.EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras();

                    oDato.EmplazamientoTipoEstructuraID = Int64.Parse(selec.RecordID);
                    oDato.EmplazamientoTipoEdificioID = Int64.Parse(GridRowSelect.SelectedRecordID);
                    oDato.Activo = true;

                    oResponse = cEdificiosEstructuras.Add(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cEdificiosEstructuras.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storeTiposEstructuras.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cEdificiosEstructuras.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cEdificiosEstructuras.DiscardChanges();
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
        public DirectResponse QuitarTipoEstructura()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosTiposEdificiosEmplazamientosTiposEstructurasController cEdificiosEstructuras = new EmplazamientosTiposEdificiosEmplazamientosTiposEstructurasController();
            InfoResponse oResponse;

            try
            {
                foreach (SelectedRow selec in GridRowSelectTiposEstructuras.SelectedRows)
                {
                    Data.EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras Borrado = cEdificiosEstructuras.GetTipoEstructuraID(Int64.Parse(GridRowSelect.SelectedRecordID), Int64.Parse(selec.RecordID));

                    oResponse = cEdificiosEstructuras.Delete(Borrado);
                    if (oResponse.Result)
                    {
                        oResponse = cEdificiosEstructuras.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                            storeTiposEstructuras.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cEdificiosEstructuras.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cEdificiosEstructuras.DiscardChanges();
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

        #endregion

        #region DIRECT METHOD COSTES

        [DirectMethod()]
        public DirectResponse GurardarCostesDesmantelamiento(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosTiposEdificiosPaisesController cTiposEdificiosPaises = new EmplazamientosTiposEdificiosPaisesController();
            Data.EmplazamientosTiposEdificiosPaises oDato;
            long lCliID = 0;
            InfoResponse oResponse;

            try
            {
                if (!bAgregar)
                {
                    Data.Vw_EmplazamientosTiposEdificiosPaises oDatoAnt;
                    long lProCCID = long.Parse(TipoEdificiosPaisesRowSelection.SelectedRecordID);

                    oDato = cTiposEdificiosPaises.GetItem(lProCCID);
                    oDatoAnt = cTiposEdificiosPaises.GetItem<Data.Vw_EmplazamientosTiposEdificiosPaises>(lProCCID);
                    long lProID = long.Parse(GridRowSelect.SelectedRecordID);

                    oDato.CostoPorDesmantelamiento = Convert.ToDouble(txtCoste.Text);
                    oDato.EmplazamientoTipoEdificioID = lProID;
                    oDato.Anyo = Convert.ToInt16(cmbAnualidad.SelectedItem.Value);
                    oDato.TasaDescuento = Convert.ToDouble(txtTasaDescuento.Text);
                    oDato.TasaInflacion = Convert.ToDouble(txtTasaInflacion.Text);
                    oDato.Anyo = Convert.ToInt16(cmbAnualidad.SelectedItem.Value);
                    oDato.TasaDescuento = Convert.ToDouble(txtTasaDescuento.Text);
                    oDato.TasaInflacion = Convert.ToDouble(txtTasaInflacion.Text);

                    if (cmbPais.SelectedItem.Value != null && cmbPais.SelectedItem.Value != "")
                    {
                        oDato.PaisID = Convert.ToInt32(cmbPais.SelectedItem.Value.ToString());
                    }

                    lCliID = long.Parse(hdCliID.Value.ToString());

                    if (cmbMoneda.SelectedItem.Value != null && cmbMoneda.SelectedItem.Value != "")
                    {
                        oDato.MonedaID = Convert.ToInt32(cmbMoneda.SelectedItem.Value.ToString());
                    }

                    oResponse = cTiposEdificiosPaises.Update(oDato);
                    if (oResponse.Result)
                    {
                        oResponse = cTiposEdificiosPaises.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storeTiposEdificiosPaises.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cTiposEdificiosPaises.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cTiposEdificiosPaises.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());
                    oDato = new Data.EmplazamientosTiposEdificiosPaises();
                    long lProID = long.Parse(GridRowSelect.SelectedRecordID);

                    oDato.CostoPorDesmantelamiento = Convert.ToDouble(txtCoste.Text);
                    oDato.EmplazamientoTipoEdificioID = lProID;

                    if (cmbPais.SelectedItem.Value != null && cmbPais.SelectedItem.Value != "")
                    {
                        oDato.PaisID = Convert.ToInt32(cmbPais.SelectedItem.Value.ToString());
                    }

                    if (cmbMoneda.SelectedItem.Value != null && cmbMoneda.SelectedItem.Value != "")
                    {
                        oDato.MonedaID = Convert.ToInt32(cmbMoneda.SelectedItem.Value.ToString());
                    }

                    oDato.Anyo = Convert.ToInt16(cmbAnualidad.SelectedItem.Value);
                    oDato.TasaDescuento = Convert.ToDouble(txtTasaDescuento.Text);
                    oDato.TasaInflacion = Convert.ToDouble(txtTasaInflacion.Text);
                    oDato.Anyo = Convert.ToInt16(cmbAnualidad.SelectedItem.Value);
                    oDato.TasaDescuento = Convert.ToDouble(txtTasaDescuento.Text);
                    oDato.TasaInflacion = Convert.ToDouble(txtTasaInflacion.Text);
                    oDato.Activo = true;
                    oDato.ClienteID = lCliID;

                    oResponse = cTiposEdificiosPaises.Add(oDato);
                    if (oResponse.Result)
                    {
                        oResponse = cTiposEdificiosPaises.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storeTiposEdificiosPaises.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cTiposEdificiosPaises.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cTiposEdificiosPaises.DiscardChanges();
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
        public DirectResponse MostrarEditarCoste()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosTiposEdificiosPaisesController cTiposEdificiosPaises = new EmplazamientosTiposEdificiosPaisesController();

            try
            {
                long lS = long.Parse(TipoEdificiosPaisesRowSelection.SelectedRecordID);
                Data.EmplazamientosTiposEdificiosPaises oDato;
                oDato = cTiposEdificiosPaises.GetItem(lS);

                if (oDato.CostoPorDesmantelamiento != null)
                {
                    txtCoste.Text = oDato.CostoPorDesmantelamiento.ToString();
                }

                if (oDato.CostoPorDesmantelamiento != null)
                {
                    txtCoste.Text = oDato.CostoPorDesmantelamiento.ToString();
                }

                if (oDato.TasaDescuento != null)
                {
                    txtTasaDescuento.Text = oDato.TasaDescuento.ToString();
                }

                if (oDato.TasaInflacion != null)
                {
                    txtTasaInflacion.Text = oDato.TasaInflacion.ToString();
                }

                if (oDato.Anyo != null)
                {
                    cmbAnualidad.SetValue(oDato.Anyo.ToString());
                }

                cmbPais.SetValue(oDato.PaisID);
                cmbMoneda.SetValue(oDato.MonedaID.ToString());

                winCostesDesmantelamiento.Show();

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
        public DirectResponse QuitarCoste()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosTiposEdificiosPaisesController cTiposEdificiosPaises = new EmplazamientosTiposEdificiosPaisesController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(TipoEdificiosPaisesRowSelection.SelectedRecordID);
                Data.EmplazamientosTiposEdificiosPaises oDato = cTiposEdificiosPaises.GetItem(lID);
                oResponse = cTiposEdificiosPaises.Delete(oDato);
                if (oResponse.Result)
                {
                    oResponse = cTiposEdificiosPaises.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storeTiposEdificiosPaises.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cTiposEdificiosPaises.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cTiposEdificiosPaises.DiscardChanges();
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
        public DirectResponse AsignarPorDefectoCoste()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosTiposEdificiosPaisesController cTiposEdificiosPaises = new EmplazamientosTiposEdificiosPaisesController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(TipoEdificiosPaisesRowSelection.SelectedRecordID);
                Data.EmplazamientosTiposEdificiosPaises oDato = cTiposEdificiosPaises.GetItem(lID);
                oResponse = cTiposEdificiosPaises.SetDefecto(oDato);
                if (oResponse.Result)
                {
                    oResponse = cTiposEdificiosPaises.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storeTiposEdificiosPaises.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cTiposEdificiosPaises.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cTiposEdificiosPaises.DiscardChanges();
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
        public DirectResponse ActivarDesactivarCostes()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosTiposEdificiosPaisesController cTiposEdificiosPaises = new EmplazamientosTiposEdificiosPaisesController();
            InfoResponse oResponse;
            try
            {
                long lID = long.Parse(TipoEdificiosPaisesRowSelection.SelectedRecordID);
                Data.EmplazamientosTiposEdificiosPaises oDato = cTiposEdificiosPaises.GetItem(lID);
                oResponse = cTiposEdificiosPaises.ModificarActivar(oDato);
                if (oResponse.Result)
                {
                    oResponse = cTiposEdificiosPaises.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storeTiposEdificiosPaises.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cTiposEdificiosPaises.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cTiposEdificiosPaises.DiscardChanges();
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

        public class Anyos
        {
            int _AnualidadID;
            string _Anualidad;

            public int AnualidadID
            {
                get { return _AnualidadID; }
                set { _AnualidadID = value; }
            }

            public string Anualidad
            {
                get { return _Anualidad; }
                set { _Anualidad = value; }
            }
        }

        #endregion

    }
}