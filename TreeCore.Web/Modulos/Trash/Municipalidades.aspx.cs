using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using TreeCore.Integraciones.Comarch;

namespace TreeCore.ModGlobal
{
    public partial class Municipalidades : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> ListaFuncionalidades = new List<long>();
        long MaestroID = 0;

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

                List<string> ListaIgnore = new List<string>() { };
                Comun.CreateGridFilters(gridFilters, storePrincipal, gridMaestro.ColumnModel, ListaIgnore, _Locale);
                List<string> ListaIgnoreDetalle = new List<string>() { };
                Comun.CreateGridFilters(gridFiltersDetalle, storeDetalle, GridDetalle.ColumnModel, ListaIgnoreDetalle, _Locale);
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
                        string sMunicipioID = Request.QueryString["aux"].ToString();
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;
                        string[] param = Request.QueryString["aux"].ToString().Split(';');
                        string sGrid = Request.QueryString["aux3"];
                        long PartidoID = 0;
                        RegionesPaisesController cRegionesPaises = new RegionesPaisesController();


                        #region MAESTRO

                        if (sGrid == "-1")
                        {

                            List<Data.GlobalMunicipalidades> ListaoDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, long.Parse(sMunicipioID));

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridMaestro.ColumnModel, ListaoDatos, Response, "", GetGlobalResource(Comun.jsMunicipalidades), _Locale);
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
                            List<Data.GlobalPartidos> ListaoDatosDetalle = ListaDetalle(0, 0, sOrden, sDir, ref iCount, sFiltro, long.Parse(sMunicipioID));

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(GridDetalle.ColumnModel, ListaoDatosDetalle, Response, "", GetGlobalResource(Comun.jsMunicipalidades), _Locale);
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
            if (ListaFuncionalidades.Contains((long)Comun.ModFun.GLO_Municipalidades_Lectura))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnActivar.Hidden = true;
                btnRadio.Hidden = true;
                btnAgregarInformacionComarch.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;
                btnDefecto.Hidden = true;

                btnAnadirDetalle.Hidden = true;
                btnEditarDetalle.Hidden = true;
                btnEliminarDetalle.Hidden = true;
                btnActivarDetalle.Hidden = true;
                btnRefrescarDetalle.Hidden = false;
                btnDescargarDetalle.Hidden = true;
                btnRadioDetalle.Hidden = true;
                btnDefectoDetalle.Hidden = true;
            }
            if (ListaFuncionalidades.Contains((long)Comun.ModFun.GLO_Municipalidades_Total))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnActivar.Hidden = false;
                btnRadio.Hidden = false;
                btnAgregarInformacionComarch.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;
                btnDefecto.Hidden = false;

                btnAnadirDetalle.Hidden = false;
                btnEditarDetalle.Hidden = false;
                btnEliminarDetalle.Hidden = false;
                btnActivarDetalle.Hidden = false;
                btnRefrescarDetalle.Hidden = false;
                btnDescargarDetalle.Hidden = false;
                btnRadioDetalle.Hidden = false;
                btnDefectoDetalle.Hidden = false;
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
                    long MunicipioID = 0;

                    RegionesPaisesController cRegionesPaises = new RegionesPaisesController();

                    if (cmbMunicipio.Value != null)
                    {
                        MunicipioID = Convert.ToInt32(cmbMunicipio.Value.ToString());
                        hdMunicipio.SetValue(Convert.ToInt32(cmbMunicipio.Value.ToString()));
                    }

                    if (MunicipioID != 0)
                    {
                        //Recupera los oDatos y los establece
                        var ls = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, MunicipioID);
                        if (ls != null)
                        {
                            storePrincipal.DataSource = ls;

                            PageProxy temp;
                            temp = (PageProxy)storePrincipal.Proxy[0];
                            temp.Total = iCount;
                        }
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    //string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.GlobalMunicipalidades> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long MunicipioID)
        {
            List<Data.GlobalMunicipalidades> listaDatos;
            GlobalMunicipalidadesController cMunicipalidades = new GlobalMunicipalidadesController();


            try
            {
                if (MunicipioID != 0)
                {
                    listaDatos = cMunicipalidades.GetItemsWithExtNetFilterList(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "MunicipioID==" + MunicipioID);
                }
                else
                {
                    listaDatos = null;
                }

                //Filtro resultados KPI
                if (listaDatos != null && listIdsResultadosKPI != null)
                {
                    listaDatos = cMunicipalidades.FiltroListaPrincipalByIDs(listaDatos.Cast<object>().ToList(), listIdsResultadosKPI, nameIndiceID).Cast<Data.GlobalMunicipalidades>().ToList();
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


                    long MaestroID;
                    if (cmbMunicipio.Value != "")
                    {
                        if (!ModuloID.Value.Equals(""))
                        {
                            if (hdMaestroID.Value.ToString() != "1")
                            {
                                MaestroID = Convert.ToInt64(ModuloID.Value.ToString());
                            }
                            else
                            {
                                MaestroID = 0;
                            }
                        }
                        else
                        {
                            MaestroID = 0;
                        }
                    }
                    else
                    {
                        MaestroID = 0;
                    }


                    var vLista = ListaDetalle(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, MaestroID);
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

        private List<Data.GlobalPartidos> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long MaestroID)
        {
            List<Data.GlobalPartidos> ListaoDatos;
            GlobalPartidosController CMunicipios = new GlobalPartidosController();

            try

            {
                if (MaestroID != 0)
                {
                    ListaoDatos = CMunicipios.GetItemsWithExtNetFilterList<Data.GlobalPartidos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "GlobalMunicipalidadID == " + MaestroID.ToString());
                }
                else
                {
                    ListaoDatos = null;
                }
            }

            catch (Exception ex)
            {
                ListaoDatos = null;
                log.Error(ex.Message);
            }

            return ListaoDatos;
        }


        #endregion

        #region STORES LOCALIZACIONES

        #region REGION

        protected void storeRegionPaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {

                    List<Data.Regiones> listaRegiones = ListaRegionPaises();

                    if (listaRegiones != null)
                    {
                        this.storeRegiones.DataSource = listaRegiones;
                        this.storeRegiones.DataBind();
                    }


                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        private List<Data.Regiones> ListaRegionPaises()
        {
            List<Data.Regiones> listaDatos;
            RegionesController cRegiones = new RegionesController();

            try
            {
                listaDatos = cRegiones.GetActivos();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region PAISES

        protected void storePaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Paises> listaPaises = ListaPaises();

                    if (listaPaises != null)
                    {
                        this.storePaises.DataSource = listaPaises;
                        this.storePaises.DataBind();
                    }
                    string pais;
                    if (hdPais.Value != null)
                    {
                        pais = hdPais.Value.ToString();
                    }
                    else
                    {
                        pais = "";
                    }

                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        private List<Data.Paises> ListaPaises()
        {
            List<Data.Paises> listaDatos = new List<Data.Paises>();
            PaisesController cPaises = new PaisesController();

            try
            {
                if (hdCliID.Value != null)
                {
                    listaDatos = cPaises.GetActivos(long.Parse(hdCliID.Value.ToString()));

                }
                else
                {
                    if (this.hdRegionPaises.Value != null && this.hdRegionPaises.Value.ToString() != "")
                    {
                        listaDatos = cPaises.GetPaisesRegion(Convert.ToInt64(this.hdRegionPaises.Value));
                        hdRegionPaises.SetValue("");
                    }
                    else if (this.cmbRegiones.Value != null && this.cmbRegiones.Value.ToString() != "")
                    {
                        listaDatos = cPaises.GetPaisesRegion(Convert.ToInt64(this.cmbRegiones.Value));

                    }
                    else
                    {
                        listaDatos = null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region REGIONES PAISES

        protected void storeRegiones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.RegionesPaises> listaRegiones = ListaRegiones();

                    if (listaRegiones != null)
                    {
                        storeRegiones.DataSource = listaRegiones;
                        this.storeRegiones.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        private List<Data.RegionesPaises> ListaRegiones()
        {
            List<Data.RegionesPaises> listaDatos;
            RegionesPaisesController cRegiones = new RegionesPaisesController();

            try
            {
                if (this.cmbPais.Value != null && this.cmbPais.Value.ToString() != "")
                {
                    listaDatos = cRegiones.getAllRegionPaisByPaisID(Convert.ToInt64(this.cmbPais.Value));

                }
                else if (this.hdPais.Value != null && this.hdPais.Value.ToString() != "")
                {
                    listaDatos = cRegiones.getAllRegionPaisByPaisID(Convert.ToInt64(this.hdPais.Value));
                    hdPais.SetValue("");
                }

                else
                {
                    listaDatos = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region PROVINCIAS

        protected void storeProvincias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Provincias> listaProvincias = ListaProvincias();

                    if (listaProvincias != null)
                    {
                        storeProvincias.DataSource = listaProvincias;
                        this.storeProvincias.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        private List<Data.Provincias> ListaProvincias()
        {
            List<Data.Provincias> listaDatos;
            ProvinciasController cProvincias = new ProvinciasController();

            try
            {
                if (this.hdRegion.Value != null && this.hdRegion.Value.ToString() != "")
                {
                    listaDatos = cProvincias.getAllProvinciasByRegionPaisID(Convert.ToInt64(this.hdRegion.Value));
                    hdRegion.SetValue("");
                }
                else if (this.cmbRegiones.Value != null && this.cmbRegiones.Value.ToString() != "")
                {
                    listaDatos = cProvincias.getAllProvinciasByRegionPaisID(Convert.ToInt64(this.cmbRegiones.Value));
                }
                else
                {
                    listaDatos = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region MUNICIPIO

        protected void storeMunicipios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Municipios> listaMunicipios = ListaMunicipios();

                    if (listaMunicipios != null)
                    {
                        storeMunicipios.DataSource = listaMunicipios;
                        storeMunicipios.DataBind();
                    }

                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        private List<Data.Municipios> ListaMunicipios()
        {
            List<Data.Municipios> listaDatos;
            MunicipiosController cMunicipios = new MunicipiosController();

            try
            {
                if (this.hdProvincia.Value != null && this.hdProvincia.Value.ToString() != "")
                {
                    listaDatos = cMunicipios.getAllMunicipiosByProvID(Convert.ToInt64(this.hdProvincia.Value));
                    hdProvincia.SetValue("");
                }
                else if (this.cmbProvincia.Value != null && this.cmbProvincia.Value.ToString() != "")
                {
                    listaDatos = cMunicipios.getAllMunicipiosByProvID(Convert.ToInt64(this.cmbProvincia.Value));
                }
                else
                {
                    listaDatos = null;
                }
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
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.Clientes> ListaClientes()
        {
            List<Data.Clientes> listaoDatos;
            ClientesController cClientes = new ClientesController();

            try
            {
                listaoDatos = cClientes.GetActivos();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaoDatos = null;
            }

            return listaoDatos;
        }

        #endregion


        #endregion

        #region DIRECT METHOD

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)
        {
            DirectResponse direct = new DirectResponse();

            GlobalMunicipalidadesController cProvincia = new GlobalMunicipalidadesController();
            RegionesPaisesController cRegionesPaises = new RegionesPaisesController();
            long regionID = long.Parse(cmbRegiones.Value.ToString());
            long municipioID = long.Parse(cmbMunicipio.Value.ToString());
            #region COMARCH_VARIABLES
            TreeCore.Integraciones.Comarch.ComarchConnection comarchConnect = null;

            #endregion

            #region INTEGRACION_COMARCH
            try
            {
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.GlobalMunicipalidades oDato = cProvincia.GetItem(S);

                    if (oDato.Municipalidad == txtProvincia.Text && oDato.Codigo == txtCodigoProvincia.Text)
                    {
                        oDato.Municipalidad = txtProvincia.Text;
                        oDato.Activo = true;
                        oDato.Codigo = txtCodigoProvincia.Text;
                    }
                    else
                    {
                        long lMunicipioID = long.Parse(cmbMunicipio.Value.ToString());
                        if (cProvincia.RegistroDuplicadoByNombre(txtProvincia.Text, lMunicipioID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else if (cProvincia.RegistroDuplicadoByCodigo(txtCodigoProvincia.Text, lMunicipioID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.Municipalidad = txtProvincia.Text;
                            oDato.Activo = true;
                            oDato.Codigo = txtCodigoProvincia.Text;
                        }
                    }

                    if (cProvincia.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }

                }
                else
                {
                    long lMunicipioID = long.Parse(cmbMunicipio.Value.ToString());

                    if (cProvincia.RegistroDuplicadoByNombre(txtProvincia.Text, lMunicipioID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else if (cProvincia.RegistroDuplicadoByCodigo(txtCodigoProvincia.Text, lMunicipioID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.GlobalMunicipalidades oDato = new Data.GlobalMunicipalidades();
                        oDato.Municipalidad = txtProvincia.Text;
                        oDato.Codigo = txtCodigoProvincia.Text;
                        oDato.Activo = true;
                        oDato.MunicipioID = municipioID;




                        if (cProvincia.AddItem(oDato) != null)
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
            #endregion
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();

            GlobalMunicipalidadesController cProvincias = new GlobalMunicipalidadesController();


            try

            {
                long S = long.Parse(GridRowSelect.SelectedRows[0].RecordID);

                Data.GlobalMunicipalidades oDato = cProvincias.GetItem(S);
                txtProvincia.Text = oDato.Municipalidad;
                txtCodigoProvincia.Text = oDato.Codigo;
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
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            GlobalMunicipalidadesController cProvincias = new GlobalMunicipalidadesController();
            MunicipiosController cMunicipios = new MunicipiosController();
            Data.GlobalMunicipalidades oDato;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRows[0].RecordID);

                long lregionID = Convert.ToInt32(cmbMunicipio.Value);

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cProvincias.GetDefault(lregionID);

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.GlobalMunicipalidadID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cMunicipios.EliminarDefecto(oDato.GlobalMunicipalidadID);
                            cProvincias.UpdateItem(oDato);
                        }

                        oDato = cProvincias.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        cProvincias.UpdateItem(oDato);
                    }
                    else if (oDato.Defecto)
                    {
                        oDato.Defecto = !oDato.Defecto;
                        cMunicipios.EliminarDefecto(oDato.GlobalMunicipalidadID);
                        cProvincias.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cProvincias.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cProvincias.UpdateItem(oDato);
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
            GlobalMunicipalidadesController CProvincias = new GlobalMunicipalidadesController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {

                Data.GlobalMunicipalidades oDato;
                oDato = CProvincias.GetItem(lID);

                if (oDato.Defecto == true)
                {
                    log.Warn(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
                    MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsPorDefecto), Ext.Net.MessageBox.Icon.INFO, null);
                }
                else
                {
                    if (CProvincias.DeleteItem(lID))
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

        #endregion

        #region DIRECT METHOD DETALLE

        [DirectMethod()]
        public DirectResponse mostrarDetalle(long MaestroID)
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
            DirectResponse ajax = new DirectResponse();

            GlobalPartidosController cMun = new GlobalPartidosController();
            long municipalidadID = long.Parse(GridRowSelect.SelectedRecordID);

            long cliID = 0;
            try
            {
                if (!agregar)
                {
                    long S = Int64.Parse(GridRowSelectDetalle.SelectedRows[0].RecordID);
                    Data.GlobalPartidos oDato = cMun.GetItem(S);

                    if (oDato.Partido == txtMunicipio.Text && oDato.Codigo == txtCodigoMunicipio.Text)
                    {
                        oDato.Partido = txtMunicipio.Text;
                        oDato.Codigo = txtCodigoMunicipio.Text;
                    }
                    else
                    {
                        long MaestroID = Int64.Parse(GridRowSelect.SelectedRows[0].RecordID);
                        cliID = long.Parse(hdCliID.Value.ToString());
                        if (cMun.RegistroDuplicadoByNombre(txtMunicipio.Text, MaestroID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else if (cMun.RegistroDuplicadoByCodigo(txtCodigoMunicipio.Text, MaestroID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.Partido = txtMunicipio.Text;
                            oDato.Codigo = txtCodigoMunicipio.Text;
                        }
                    }

                    if (cMun.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));

                    }

                }
                else
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                    long MaestroID = Int64.Parse(GridRowSelect.SelectedRows[0].RecordID);

                    if (cMun.RegistroDuplicadoByNombre(txtMunicipio.Text, MaestroID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else if (cMun.RegistroDuplicadoByCodigo(txtCodigoMunicipio.Text, MaestroID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.GlobalPartidos oDato = new Data.GlobalPartidos();
                        oDato.Partido = txtMunicipio.Text;
                        oDato.Codigo = txtCodigoMunicipio.Text;
                        oDato.Activo = true;
                        oDato.Defecto = false;
                        oDato.GlobalMunicipalidadID = municipalidadID;
                        if (cMun.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));

                        }

                    }

                }
                storeDetalle.DataBind();
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
                long ID = long.Parse(hdGlopalPartidoID.Value.ToString());
                GlobalPartidosController cMunicipios = new GlobalPartidosController();
                Data.GlobalPartidos oDato = cMunicipios.GetItem(ID);

                txtMunicipio.Text = oDato.Partido;
                txtCodigoMunicipio.Text = oDato.Codigo;

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
        public DirectResponse AsignarPorDefectoDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalPartidosController cMunicipios = new GlobalPartidosController();

            try
            {
                long lID = Int64.Parse(GridRowSelectDetalle.SelectedRows[0].RecordID);
                long lGlobalPartidoID = long.Parse(GridRowSelect.SelectedRows[0].RecordID);
                Data.GlobalPartidos oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cMunicipios.GetDefault(lGlobalPartidoID);

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.GlobalPartidoID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cMunicipios.UpdateItem(oDato);
                        }

                        oDato = cMunicipios.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        cMunicipios.UpdateItem(oDato);
                    }
                    else if (oDato.Defecto)
                    {
                        oDato.Defecto = !oDato.Defecto;
                        cMunicipios.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cMunicipios.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cMunicipios.UpdateItem(oDato);
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
        public DirectResponse EliminarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalPartidosController CMunicipios = new GlobalPartidosController();

            direct.Result = "";
            direct.Success = true;

            long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

            try
            {
                Data.GlobalPartidos oDato;
                oDato = CMunicipios.GetItem(lID);
                if (oDato.Defecto == true)
                {
                    log.Warn(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
                    MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsPorDefecto), Ext.Net.MessageBox.Icon.INFO, null);
                }
                else
                {
                    if (CMunicipios.DeleteItem(lID))
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
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            GlobalMunicipalidadesController cProvincias = new GlobalMunicipalidadesController();
            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.GlobalMunicipalidades oDato = cProvincias.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cProvincias.UpdateItem(oDato))
                {
                    storePrincipal.DataBind();
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
        public DirectResponse ActivarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalPartidosController CMunicipios = new GlobalPartidosController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.GlobalPartidos oDato = CMunicipios.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (CMunicipios.UpdateItem(oDato))
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

        #region RADIO

        [DirectMethod()]
        public DirectResponse AsignarRadio()
        {
            DirectResponse direct = new DirectResponse();
            GlobalMunicipalidadesController cPaises = new GlobalMunicipalidadesController();
            try
            {
                if (GridRowSelect.SelectedRows.Count > 0)
                {
                    foreach (SelectedRow selec in GridRowSelect.SelectedRows)
                    {
                        Data.GlobalMunicipalidades oDato = cPaises.GetItem(long.Parse(selec.RecordID));
                        if (oDato != null)
                        {
                            oDato.Radio = numRadio.Value != null ? (double?)numRadio.Value : null;
                            cPaises.UpdateItem(oDato);
                        }
                    }
                }
                else
                {
                    //long municipioID = Convert.ToInt64(cmbRegiones.SelectedItem.Value);
                    //List<Data.Provincias> Municipalidades = cPaises.getAllProvinciasByRegionPaisID(municipioID);

                    //foreach (Data.Provincias oDato in Municipalidades)
                    //{
                    //    using (ProvinciasController controller = new ProvinciasController())
                    //    {
                    //        Data.Provincias aux = controller.GetItem(oDato.ProvinciaID);
                    //        aux.Radio = numRadio.Value != null ? (double?)numRadio.Value : null;
                    //        controller.UpdateItem(aux);
                    //    }
                    //}
                }

                storePrincipal.DataBind();

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
        public DirectResponse MostrarEditarRadio()
        {
            DirectResponse result = new DirectResponse();
            GlobalMunicipalidadesController cMunicipalidad = new GlobalMunicipalidadesController();

            try
            {
                long S = long.Parse(GridRowSelect.SelectedRows[0].RecordID);

                Data.GlobalMunicipalidades oDato = cMunicipalidad.GetItem(S);

                if (oDato != null && oDato.Radio != null)
                {
                    numRadio.Value = oDato.Radio.Value.ToString();
                }

                winRadio.Show();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return result;
            }

            result.Success = true;
            result.Result = "";
            return result;
        }

        [DirectMethod()]
        public DirectResponse AsignarRadioDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalPartidosController cMunicipios = new GlobalPartidosController();
            try
            {
                if (GridRowSelectDetalle.SelectedRows.Count > 0)
                {
                    foreach (SelectedRow selec in GridRowSelectDetalle.SelectedRows)
                    {
                        Data.GlobalPartidos oDato = cMunicipios.GetItem(long.Parse(selec.RecordID));
                        if (oDato != null)
                        {
                            oDato.Radio = numRadioDetalle.Value != null ? (double?)numRadioDetalle.Value : null;
                            cMunicipios.UpdateItem(oDato);
                        }
                    }
                }
                else
                {
                    long provinciaID = Convert.ToInt64(GridRowSelect.SelectedRows[0].RecordID);
                    List<Data.GlobalPartidos> Municipios = cMunicipios.getAllMunicipalidadesByMunID(provinciaID);

                    foreach (Data.GlobalPartidos oDato in Municipios)
                    {
                        using (GlobalPartidosController controller = new GlobalPartidosController())
                        {
                            Data.GlobalPartidos aux = controller.GetItem(oDato.GlobalPartidoID);
                            aux.Radio = numRadioDetalle.Value != null ? (double?)numRadioDetalle.Value : null;
                            controller.UpdateItem(aux);
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

            direct.Success = true;
            direct.Result = "";
            return direct;
        }
        [DirectMethod()]
        public DirectResponse MostrarEditarRadioDetalle()
        {
            DirectResponse result = new DirectResponse();
            GlobalPartidosController cMunicipalidades = new GlobalPartidosController();

            try
            {
                long S = long.Parse(GridRowSelectDetalle.SelectedRows[0].RecordID);

                Data.GlobalPartidos oDato = cMunicipalidades.GetItem(S);

                if (oDato != null && oDato.Radio != null)
                {
                    numRadioDetalle.Value = oDato.Radio.Value.ToString();
                }

                winRadioDetalle.Show();
                cMunicipalidades = null;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return result;
            }

            result.Success = true;
            result.Result = "";
            return result;
        }
        #endregion

        #region INFORMACION COMARCH

        /// <summary>
        /// MOSTRAR EDITAR INFORMACION COMARCH
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public DirectResponse MostrarEditarAgregarComarch()
        {
            DirectResponse ajax = new DirectResponse();
            ajax.Result = "";
            ajax.Success = true;

            try
            {
                long ID = Int64.Parse(GridRowSelect.SelectedRows[0].RecordID);
                Data.GlobalMunicipalidades provincia;
                GlobalMunicipalidadesController cProvincias = new GlobalMunicipalidadesController();
                provincia = cProvincias.GetItem(ID);

                #region oDatoS COMARCH

                if (provincia.CategoriaDivipola != null)
                {
                    txtCategoriaDivipola.Text = provincia.CategoriaDivipola;
                }
                else
                {
                    txtCategoriaDivipola.Text = "";
                }
                if (provincia.TipoPoblado != null)
                {
                    txtTipoPoblado.Text = provincia.TipoPoblado;
                }
                else
                {
                    txtTipoPoblado.Text = "";
                }
                if (provincia.Latitud != null)
                {
                    numbLatitud.Text = provincia.Latitud.Value.ToString();
                }
                else
                {
                    numbLatitud.Value = "";
                }
                if (provincia.Longitud != null)
                {
                    numbLongitud.Value = provincia.Longitud.Value.ToString();
                }
                else
                {
                    numbLongitud.Value = "";
                }



                #endregion



                winGestionComarch.Show();
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return ajax;
        }

        /// <summary>
        /// AGREGAR EDITAR INFORMACION COMARCH 
        /// </summary>
        /// <param name="agregar"></param>
        /// <returns></returns>
        [DirectMethod]
        public DirectResponse AgregarEditarComarch()
        {
            DirectResponse direct = new DirectResponse();
            direct.Result = "";
            direct.Success = true;

            #region COMARCH_VARIABLES

            bool bIntegracionComarch = false;
            TreeCore.Integraciones.Comarch.ComarchConnection comarchConnect = null;
            bool conexionSegura = false;
            AddressItemRegionRequest regionRequest = null;
            bool warning = false;
            bool error = false;
            string tipoAviso = "ERROR";

            #endregion

            try
            {

                #region INTEGRACION_COMARCH

                try
                {

                    comarchConnect = new TreeCore.Integraciones.Comarch.ComarchConnection(Comun.INTEGRACION_SERVICIO_COMARCH);

                    if (comarchConnect != null && comarchConnect.IsConectado())
                    {
                        bIntegracionComarch = true;
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    bIntegracionComarch = false;

                }

                #endregion

                long ID = Int64.Parse(GridRowSelect.SelectedRows[0].RecordID);
                Data.GlobalMunicipalidades MunicipalidadoDato = new Data.GlobalMunicipalidades();
                GlobalMunicipalidadesController cGlobalMunicipalidadesoDatos = new GlobalMunicipalidadesController();
                MunicipalidadoDato = cGlobalMunicipalidadesoDatos.GetItem(ID);

                MunicipalidadoDato.CategoriaDivipola = txtCategoriaDivipola.Text;
                MunicipalidadoDato.TipoPoblado = txtTipoPoblado.Text;
                MunicipalidadoDato.Latitud = long.Parse(numbLatitud.Value.ToString());
                MunicipalidadoDato.Longitud = long.Parse(numbLatitud.Value.ToString());


                //#region INTEGRACION COMARCH

                //if (bIntegracionComarch)
                //{

                //    AddressItemRegionResponse res = null;

                //    ////Agregamos los oDatos a enviar.
                //    regionRequest = new AddressItemRegionRequest();

                //    if ((provinciaoDato.RegionComercial != null && provinciaoDato.RegionComercial != "")
                //        || (provinciaoDato.ZonaMinisterio != null && provinciaoDato.ZonaMinisterio != "")
                //        || (provinciaoDato.RegionalTX != null && provinciaoDato.RegionalTX != "")
                //        || (provinciaoDato.ResponsableTX != null && provinciaoDato.ResponsableTX != ""))

                //    {
                //        regionRequest.OPERATION = Comun.INTEGRACION_SERVICIO_COMARCH_MODIFY;
                //    }
                //    else
                //    {

                //        regionRequest.OPERATION = Comun.INTEGRACION_SERVICIO_COMARCH_CREATE;
                //    }

                //    regionRequest.ABBREVIATION = provinciaoDato.Codigo;
                //    regionRequest.MASTER_COMMERCIAL_CLASS = provinciaoDato.RegionComercial;
                //    regionRequest.MASTER_MINISTRY_AREA = provinciaoDato.ZonaMinisterio;
                //    regionRequest.MASTER_REGIONAL_TX = provinciaoDato.RegionalTX;
                //    regionRequest.REGION_NAME = provinciaoDato.Provincia;
                //    regionRequest.MASTER_RESPONSIBLE_TX = provinciaoDato.ResponsableTX;





                //    Comun.cLog.EscribirLog(DateTime.Now, "COMARCH: Antes de llamar a AddressItemRegion. EndPoint: " + comarchConnect.GetServidor());
                //    res = comarchConnect.ComarchAddressItemRegion(conexionSegura, regionRequest, Comun.MODULOGLOBAL, "", Usuario.UsuarioID, null);
                //    Comun.cLog.EscribirLog(DateTime.Now, "COMARCH: Despues de crear AddressItemRegion");
                //    //Se recibe respuesta
                //    if (res != null)
                //    {

                //        switch (res.CODE_RESPUESTA)
                //        {
                //            //OSS Internal Error -- Error Interno de OSS
                //            case "0":
                //                tipoAviso = "ERROR";
                //                error = true;

                //                break;

                //            //Invalid credentials for this operation -- Credenciales inválidas para esta operación.
                //            case "101":
                //                tipoAviso = "WARNING";
                //                warning = true;

                //                break;

                //            //Region already created. -- Departamento enviado ya se encuentra creado.
                //            case "103":
                //                tipoAviso = "WARNING";
                //                warning = true;

                //                break;

                //            //Missing manoDatory attribute – operation
                //            case "104":
                //                tipoAviso = "WARNING";
                //                warning = true;

                //                break;

                //            //Missing manoDatory attribute – abbreviation
                //            case "105":
                //                tipoAviso = "WARNING";
                //                warning = true;

                //                break;

                //            //Missing manoDatory attribute – abbreviationregion
                //            case "106":
                //                tipoAviso = "WARNING";
                //                warning = true;

                //                break;

                //            //Missing manoDatory attribute – masterregionname
                //            case "107":
                //                tipoAviso = "WARNING";
                //                warning = true;

                //                break;

                //            //Missing attribute - name
                //            case "108":
                //                tipoAviso = "WARNING";
                //                warning = true;

                //                break;
                //            //OK -- Operación exitosa
                //            case "200":
                //                tipoAviso = "INFO";

                //                break;

                //        }

                //        MensajeBox(tipoAviso, res.CODE_RESPUESTA + "-" + res.MENSAJE_RESPUESTA, Ext.Net.MessageBox.Icon.ERROR, null);
                //        direct.Success = true;
                //        if (error || warning)
                //        {

                //            return direct;
                //        }


                //    }
                //    else
                //    {
                //        MensajeBox(tipoAviso, "Error de conexión con el servidor ", Ext.Net.MessageBox.Icon.ERROR, null);
                //        direct.Success = true;
                //        return direct;

                //    }

                //}

                //comarchConnect = null;

                //#endregion



                if (cGlobalMunicipalidadesoDatos.UpdateItem(MunicipalidadoDato))
                {

                    storeDetalle.DataBind();

                }
                cGlobalMunicipalidadesoDatos = null;


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

        //#region INFORMACION COMARCH DETALLE

        ///// <summary>
        ///// MOSTRAR EDITAR INFORMACION COMARCH
        ///// </summary>
        ///// <returns></returns>
        //[DirectMethod]
        //public DirectResponse MostrarEditarAgregarComarchDetalle()
        //{
        //    DirectResponse ajax = new DirectResponse();
        //    ajax.Result = "";
        //    ajax.Success = true;

        //    try
        //    {

        //        long ID = Int64.Parse(GridRowSelectDetalle.SelectedRows[0].RecordID);
        //        Data.Municipios municipio = new Data.Municipios();
        //        MunicipiosController cMunicipios = new MunicipiosController();
        //        municipio = cMunicipios.GetItem(ID);

        //        #region oDatoS COMARCH

        //        if (municipio.ZonaCrc != null)
        //        {
        //            txtZonaCrc.Text = municipio.ZonaCrc;
        //        }
        //        else
        //        {
        //            txtZonaCrc.Text = "";
        //        }
        //        if (municipio.CiudadCapital != null)
        //        {
        //            txtCiudadCapital.Text = municipio.CiudadCapital;
        //        }
        //        else
        //        {
        //            txtCiudadCapital.Text = "";
        //        }
        //        if (municipio.MciCiudadPrincipal != null)
        //        {
        //            txtMciCiudadPrincipal.Text = municipio.MciCiudadPrincipal;
        //        }
        //        else
        //        {
        //            txtMciCiudadPrincipal.Text = "";
        //        }
        //        if (municipio.MciCiudadGrupo != null)
        //        {
        //            txtMciCiudadGrupo.Text = municipio.MciCiudadGrupo;
        //        }
        //        else
        //        {
        //            txtMciCiudadGrupo.Text = "";
        //        }
        //        if (municipio.ProyeccionPoblacion != null)
        //        {
        //            txtProyeccionPoblacion.Text = municipio.ProyeccionPoblacion;
        //        }
        //        else
        //        {
        //            txtProyeccionPoblacion.Text = "";
        //        }
        //        if (municipio.GrupoMpio != null)
        //        {
        //            txtGrupoMpio.Text = municipio.GrupoMpio;
        //        }
        //        else
        //        {
        //            txtGrupoMpio.Text = "";
        //        }
        //        if (municipio.Categorization != null)
        //        {
        //            txtCategorization.Text = municipio.Categorization;
        //        }
        //        else
        //        {
        //            txtCategorization.Text = "";
        //        }
        //        if (municipio.Ambito != null)
        //        {
        //            txtAmbito.Text = municipio.Ambito;
        //        }
        //        else
        //        {
        //            txtAmbito.Text = "";
        //        }
        //        if (municipio.ProcentajeCoberturaLTE != null)
        //        {
        //            txtProcentajeCoberturaLTE.Text = municipio.ProcentajeCoberturaLTE;
        //        }
        //        else
        //        {
        //            txtProcentajeCoberturaLTE.Text = "";
        //        }
        //        if (municipio.Mercado != null)
        //        {
        //            txtMercado.Text = municipio.Mercado;
        //        }
        //        else
        //        {
        //            txtMercado.Text = "";
        //        }
        //        if (municipio.CiudadCapital != null)
        //        {
        //            txtCiudadCapital.Text = municipio.CiudadCapital;
        //        }
        //        else
        //        {
        //            txtCiudadCapital.Text = "";
        //        }


        //        #endregion



        //        winGestionComarchDetalle.Show();

        //        cMunicipios = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        //MensajeErrorGenerico(ex);
        //        Comun.MensajeLog(paginaJS, "MostrarEditarAgregarComarchDetalle", ex.Message);
        //        string codTit = "";
        //        codTit = Util.ExceptionHandler(ex);
        //        MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

        //    }
        //    return ajax;
        //}

        ///// <summary>
        ///// AGREGAR EDITAR INFORMACION COMARCH
        ///// </summary>
        ///// <param name="agregar"></param>
        ///// <returns></returns>
        //[DirectMethod]
        //public DirectResponse AgregarEditarComarchDetalle()
        //{
        //    DirectResponse direct = new DirectResponse();
        //    direct.Result = "";
        //    direct.Success = true;

        //    #region COMARCH_VARIABLES

        //    bool bIntegracionComarch = false;
        //    TreeCore.Integraciones.Comarch.ComarchConnection comarchConnect = null;
        //    bool conexionSegura = false;
        //    AddressItemCityRequest cityRequest = null;
        //    bool warning = false;
        //    bool error = false;
        //    string tipoAviso = "ERROR";

        //    #endregion

        //    try
        //    {

        //        #region INTEGRACION_COMARCH

        //        try
        //        {

        //            comarchConnect = new TreeCore.Integraciones.Comarch.ComarchConnection(Comun.INTEGRACION_SERVICIO_COMARCH);
        //            Comun.cLog.EscribirLog(DateTime.Now, "COMARCH: Después de crear las conexiones");

        //            if (comarchConnect != null && comarchConnect.IsConectado())
        //            {
        //                bIntegracionComarch = true;
        //                Comun.cLog.EscribirLog(DateTime.Now, "COMARCH: Conectado activo");
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Comun.MensajeLog("Municipalidades", "AgregarEditarComarch - Verifica integracion Comarch", ex.Message);
        //            Comun.cLog.EscribirLog("Comarch: Error producido al ver si el servicio está activo " + ex.Message);
        //            bIntegracionComarch = false;

        //        }

        //        #endregion

        //        long ID = Int64.Parse(GridRowSelectDetalle.SelectedRows[0].RecordID);
        //        Data.Municipios municipio = new Data.Municipios();
        //        MunicipiosController cMunicipios = new MunicipiosController();
        //        municipio = cMunicipios.GetItem(ID);

        //        municipio.ZonaCrc = txtZonaCrc.Text;
        //        municipio.CiudadCapital = txtCiudadCapital.Text;
        //        municipio.MciCiudadPrincipal = txtMciCiudadPrincipal.Text;
        //        municipio.MciCiudadGrupo = txtMciCiudadGrupo.Text;
        //        municipio.ProyeccionPoblacion = txtProyeccionPoblacion.Text;
        //        municipio.GrupoMpio = txtGrupoMpio.Text;
        //        municipio.Categorization = txtCategorization.Text;
        //        municipio.Ambito = txtAmbito.Text;
        //        municipio.ProcentajeCoberturaLTE = txtProcentajeCoberturaLTE.Text;
        //        municipio.Mercado = txtMercado.Text;

        //        #region INTEGRACION COMARCH

        //        if (bIntegracionComarch)
        //        {

        //            AddressItemCityResponse res = null;
        //            Data.Provincias provincia = new Data.Provincias();
        //            ProvinciasController cProvincias = new ProvinciasController();

        //            provincia = cProvincias.GetItem(municipio.ProvinciaID);

        //            //Agregamos los oDatos a enviar.
        //            cityRequest = new AddressItemCityRequest();

        //            if ((municipio.ZonaCrc != null && municipio.ZonaCrc != "")
        //                || (municipio.CiudadCapital != null && municipio.CiudadCapital != "")
        //                || (municipio.CiudadCapital != null && municipio.CiudadCapital != "")
        //                || (municipio.MciCiudadPrincipal != null && municipio.MciCiudadPrincipal != "")
        //                || (municipio.MciCiudadGrupo != null && municipio.MciCiudadGrupo != "")
        //                || (municipio.ProyeccionPoblacion != null && municipio.ProyeccionPoblacion != "")
        //                || (municipio.GrupoMpio != null && municipio.GrupoMpio != "")
        //                || (municipio.Categorization != null && municipio.Categorization != "")
        //                || (municipio.Ambito != null && municipio.Ambito != "")
        //                || (municipio.ProcentajeCoberturaLTE != null && municipio.ProcentajeCoberturaLTE != "")
        //                || (municipio.Mercado != null && municipio.Mercado != ""))
        //            {
        //                cityRequest.OPERATION = Comun.INTEGRACION_SERVICIO_COMARCH_MODIFY;
        //            }
        //            else
        //            {

        //                cityRequest.OPERATION = Comun.INTEGRACION_SERVICIO_COMARCH_CREATE;
        //            }

        //            cityRequest.ABBREVIATION = municipio.Codigo;
        //            cityRequest.ABBREVIATION_REGION = provincia.Codigo;
        //            cityRequest.CITY_NAME = municipio.Municipio;
        //            cityRequest.MASTER_CRC_AREA = municipio.ZonaCrc;
        //            cityRequest.MASTER_CAPITAL_CODE = municipio.CiudadCapital;
        //            cityRequest.MASTER_MCI_MAIN = municipio.MciCiudadPrincipal;
        //            cityRequest.MASTER_MCI_GROUP = municipio.MciCiudadGrupo;
        //            cityRequest.POPULATION_PROJECTION = municipio.ProyeccionPoblacion;
        //            cityRequest.MASTER_CITY_GROUP = municipio.GrupoMpio;
        //            cityRequest.MASTER_CATEGORY_617 = municipio.Categorization;
        //            cityRequest.MASTER_SCOPE = municipio.Ambito;
        //            cityRequest.LTE_COVERAGE = municipio.ProcentajeCoberturaLTE;
        //            cityRequest.MARKET = municipio.Mercado;


        //            Comun.cLog.EscribirLog(DateTime.Now, "COMARCH: Antes de llamar a AddressItemCity. EndPoint: " + comarchConnect.GetServidor());
        //            res = comarchConnect.ComarchAddressItemCity(conexionSegura, cityRequest, Comun.MODULOGLOBAL, "", Usuario.UsuarioID, null);
        //            Comun.cLog.EscribirLog(DateTime.Now, "COMARCH: Despues de crear AddressItemCity");
        //            //Se recibe respuesta
        //            if (res != null)
        //            {

        //                switch (res.CODE_RESPUESTA)
        //                {
        //                    //OSS Internal Error -- Error Interno de OSS
        //                    case "0":
        //                        tipoAviso = "ERROR";
        //                        error = true;

        //                        break;

        //                    //Invalid credentials for this operation -- Credenciales inválidas para esta operación.
        //                    case "101":
        //                        tipoAviso = "WARNING";
        //                        warning = true;

        //                        break;

        //                    //Region already created. -- Departamento enviado ya se encuentra creado.
        //                    case "103":
        //                        tipoAviso = "WARNING";
        //                        warning = true;

        //                        break;

        //                    //Missing manoDatory attribute – operation
        //                    case "104":
        //                        tipoAviso = "WARNING";
        //                        warning = true;

        //                        break;

        //                    //Missing manoDatory attribute – abbreviation
        //                    case "105":
        //                        tipoAviso = "WARNING";
        //                        warning = true;

        //                        break;

        //                    //Missing manoDatory attribute – abbreviationregion
        //                    case "106":
        //                        tipoAviso = "WARNING";
        //                        warning = true;

        //                        break;

        //                    //Missing manoDatory attribute – masterregionname
        //                    case "107":
        //                        tipoAviso = "WARNING";
        //                        warning = true;

        //                        break;

        //                    //Missing attribute - name
        //                    case "108":
        //                        tipoAviso = "WARNING";
        //                        warning = true;

        //                        break;
        //                    //OK -- Operación exitosa
        //                    case "200":
        //                        tipoAviso = "INFO";

        //                        break;

        //                }

        //                MensajeBox(tipoAviso, res.CODE_RESPUESTA + "-" + res.MENSAJE_RESPUESTA, Ext.Net.MessageBox.Icon.ERROR, null);
        //                direct.Success = true;
        //                if (error || warning)
        //                {

        //                    return direct;
        //                }


        //            }
        //            else
        //            {
        //                MensajeBox(tipoAviso, "Error de conexión con el servidor ", Ext.Net.MessageBox.Icon.ERROR, null);
        //                direct.Success = true;
        //                return direct;

        //            }

        //        }

        //        comarchConnect = null;

        //        #endregion



        //        if (cMunicipios.UpdateItem(municipio))
        //        {

        //            storeDetalle.DataBind();

        //        }
        //        cMunicipios = null;


        //    }
        //    catch (Exception ex)
        //    {
        //        Comun.cLog.EscribirLog(Comun.MensajeLog(paginaJS, "AgregarEditarInformacionComarchDetalle", ex.Message));
        //        string codTit = "";
        //        codTit = Util.ExceptionHandler(ex);
        //        MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

        //    }
        //    storeDetalle.Reload();
        //    return direct;
        //}




        //#endregion

        #endregion

    }

}