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
using TreeCore.Integraciones.Comarch;

namespace TreeCore.ModGlobal
{
    public partial class GlobalLocalidades : TreeCore.Page.BasePageExtNet
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

                List<string> listaIgnoreAreaOYM = new List<string>()
                { };

                Comun.CreateGridFilters(gridFiltersAreaOYM, storeAreasOYM, gridAreasOYM.ColumnModel, listaIgnoreAreaOYM, _Locale);

                List<string> listaIgnoreAreaLibres = new List<string>()
                { };

                Comun.CreateGridFilters(gridFiltersAreasOYMLibres, storeAreasOYMLibres, gridAreasOYMLibres.ColumnModel, listaIgnoreAreaLibres, _Locale);

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
                else {
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
                        List<Data.GlobalLocalidades> listaDatos = new List<Data.GlobalLocalidades>();
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        string param = Request.QueryString["aux"].ToString();




                        long id = 0;
                        if (param != "" && param != "null")
                            id = long.Parse(param);


                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro/*, ClienteID*/, id);

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
            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_GLOBAL_LOCALIDADES))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;
                btnActivar.Hidden = false;
                btnDefecto.Hidden = false;
            }
            if (listaFuncionalidades.Contains((int)Comun.ModFun.ACCESO_TOTAL_GLOBAL_LOCALIDADES_DATOS_ADICIONALES))
            {
                btnAgregarInformacionAdicional.Hidden = false;
            }
            if (listaFuncionalidades.Contains((int)Comun.ModFun.ACCESO_GLOBAL_LOCALIDADES_ASIGNAR_AREASOYM))
            {
                btnAreasOYM.Hidden = false;
            }
            if (listaFuncionalidades.Contains((int)Comun.ModFun.ACCESO_GLOBAL_LOCALIDADES_VISUALIZAR_AREASOYM))
            {
                btnAreasOYM.Hidden = false;
            }

            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_REGIONES_RADIO))
            {
                btnRadio.Hidden = false;
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
                    //long? lCliID = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];
                    long PartidoID = 0;

                    //if (ClienteID != null)
                    //{
                    //    lCliID = ClienteID.Value;
                    //}
                    //else
                    //{
                    //    if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                    //    {
                    //        lCliID = long.Parse(hdCliID.Value.ToString());
                    //    }
                    //}

                    //if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                    //{
                    //    lCliID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                    //}

                    if (cmbPartido.SelectedItem.Value != null && cmbPartido.SelectedItem.Value != "")
                    {
                        PartidoID = Convert.ToInt32(cmbPartido.SelectedItem.Value);
                    }
                    else if (hdPartidoID.Value != null && hdPartidoID.Value.ToString() != "")
                    {
                        PartidoID = long.Parse(hdPartidoID.Value.ToString());
                    }

                    if (PartidoID != 0)
                    {
                        var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro/*, lCliID*/, PartidoID);
                        if (lista != null)
                        {
                            storePrincipal.DataSource = lista;

                            PageProxy temp = (PageProxy)storePrincipal.Proxy[0];
                            temp.Total = iCount;
                        }
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

        private List<Data.GlobalLocalidades> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro/*, long? lClienteID*/, long PartidoID)
        {
            List<Data.GlobalLocalidades> listaDatos = new List<Data.GlobalLocalidades>();
            GlobalLocalidadesController cLocalidades = new GlobalLocalidadesController();

            try
            {
                //if (lClienteID.HasValue)
                //{
                //    listaDatos = cLocalidades.GetItemsWithExtNetFilterList<Data.GlobalLocalidades>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
                //}
                //else
                //{
                listaDatos = cLocalidades.GetItemsWithExtNetFilterList<Data.GlobalLocalidades>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "GlobalPartidoID==" + PartidoID);
                //}
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

        #region STORE PAISES

        private List<Data.Paises> ListaPaises()
        {
            List<Data.Paises> listaDatos = new List<Data.Paises>();
            List<Data.Paises> datosAux = new List<Data.Paises>();
            PaisesController cPaises = new PaisesController();
            try
            {
                long? RegionID = null;

                if (cmbRegion.SelectedItem.Value != null && cmbRegion.SelectedItem.Value != "")
                {
                    RegionID = long.Parse(cmbRegion.SelectedItem.Value);
                }
                else
                {
                    if (hdRegionID.Value != null && hdRegionID.Value.ToString() != "")

                    {
                        RegionID = long.Parse(hdRegionID.Value.ToString());
                    }
                }
                //long? RegionID = null;

                //if (hdRegionID.Value != null && hdRegionID.Value.ToString() != "")
                //{
                //    RegionID = long.Parse(hdRegionID.Value.ToString());
                //}

                if (RegionID != null)
                    listaDatos = cPaises.GetItemsList("RegionID==" + RegionID + "&& Activo==true");

                cPaises = null;
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        protected void storePaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                // devolvemos todos los clientes activos
                try
                {

                    var lista = ListaPaises();
                    if (lista != null)
                        storePaises.DataSource = lista;

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

        #region STORE REGIONES PAISES

        private List<Data.RegionesPaises> ListaRegionesPaises()
        {
            List<Data.RegionesPaises> listaDatos = new List<Data.RegionesPaises>();
            List<Data.RegionesPaises> datosAux = new List<Data.RegionesPaises>();
            RegionesPaisesController cRegionesPaises = new RegionesPaisesController();
            try
            {

                long? paisID = null;

                if (cmbPais.SelectedItem.Value != null && cmbPais.SelectedItem.Value != "")
                {
                    paisID = long.Parse(cmbPais.SelectedItem.Value);
                }
                else
                {
                    if (hdPaisID.Value != null && hdPaisID.Value.ToString() != "")

                    {
                        paisID = long.Parse(hdPaisID.Value.ToString());
                    }
                }
                if (paisID != null)
                    listaDatos = cRegionesPaises.GetItemsList("PaisID ==" + paisID + " && Activo== true");

                cRegionesPaises = null;
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        protected void storeRegionesPaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                // devolvemos todos los clientes activos
                try
                {
                    var lista = ListaRegionesPaises();
                    if (lista != null)
                        storeRegionesPaises.DataSource = lista;

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

        #region STORE REGIONES 

        private List<Data.Regiones> ListaRegiones()
        {
            List<Data.Regiones> listaDatos = new List<Data.Regiones>();
            List<Data.Regiones> datosAux = new List<Data.Regiones>();
            RegionesController cRegiones = new RegionesController();
            try
            {
                listaDatos = cRegiones.GetItemsList(" Activo== true");

                cRegiones = null;
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        protected void storeRegiones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                // devolvemos todos los clientes activos
                try
                {
                    var lista = ListaRegiones();
                    if (lista != null)
                        storeRegiones.DataSource = lista;

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

        #region STORE PROVINCIA

        private List<Data.Provincias> ListaProvincias()
        {
            List<Data.Provincias> listaDatos = new List<Data.Provincias>();
            List<Data.Provincias> datosAux = new List<Data.Provincias>();
            ProvinciasController cprovinvias = new ProvinciasController();
            try
            {
                long? RegionPaisID = null;

                if (cmbRegionPais.SelectedItem.Value != null && cmbRegionPais.SelectedItem.Value != "")
                {
                    RegionPaisID = long.Parse(cmbRegionPais.SelectedItem.Value);

                }
                else
                {
                    if (hdRegionPaisID.Value != null && hdRegionPaisID.Value.ToString() != "")
                    {
                        RegionPaisID = long.Parse(hdPaisID.Value.ToString());

                    }

                }
                if (RegionPaisID != null)
                    listaDatos = cprovinvias.GetItemsList("RegionPaisID ==" + RegionPaisID + " && Activo== true");

                cprovinvias = null;
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        protected void storeProvincias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                // devolvemos todos los clientes activos
                try
                {
                    var lista = ListaProvincias();
                    if (lista != null)
                        storeProvincias.DataSource = lista;

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

        #region STORE MUNICIPIO

        private List<Data.Municipios> ListaMunicipios()
        {
            List<Data.Municipios> listaDatos = new List<Data.Municipios>();
            List<Data.Municipios> datosAux = new List<Data.Municipios>();
            MunicipiosController cMunicipios = new MunicipiosController();

            try
            {
                long? ProvinciaID = null;

                if (cmbProvincia.SelectedItem.Value != null && cmbProvincia.SelectedItem.Value != "")
                {
                    ProvinciaID = long.Parse(cmbProvincia.SelectedItem.Value);
                }
                else
                {
                    if (hdProvinciaID.Value != null && hdProvinciaID.Value.ToString() != "")
                    {
                        ProvinciaID = long.Parse(hdPaisID.Value.ToString());

                    }
                }
                if (ProvinciaID != null)
                    listaDatos = cMunicipios.GetItemsList("ProvinciaID ==" + ProvinciaID + " && Activo== true");

                cMunicipios = null;
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        protected void storeMunicipios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                // devolvemos todos los clientes activos
                try
                {
                    var lista = ListaMunicipios();
                    if (lista != null)
                        storeMunicipios.DataSource = lista;

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

        #region STORE MUNICIPALIDAD

        private List<Data.GlobalMunicipalidades> ListaMunicipalidad()
        {
            List<Data.GlobalMunicipalidades> listaDatos = new List<Data.GlobalMunicipalidades>();
            List<Data.GlobalMunicipalidades> datosAux = new List<Data.GlobalMunicipalidades>();
            GlobalMunicipalidadesController cMunicipalidad = new GlobalMunicipalidadesController();
            try
            {
                long? MunicipioID = null;

                if (cmbMunicipio.SelectedItem.Value != null && cmbMunicipio.SelectedItem.Value != "")

                {
                    MunicipioID = long.Parse(cmbMunicipio.SelectedItem.Value);

                }
                else
                {
                    if (hdMunicipioID.Value != null && hdMunicipioID.Value.ToString() != "")

                    {
                        MunicipioID = long.Parse(hdMunicipioID.Value.ToString());

                    }

                }
                if (MunicipioID != null)
                    listaDatos = cMunicipalidad.GetItemsList("MunicipioID ==" + MunicipioID + " && Activo== true");

                cMunicipalidad = null;
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        protected void storeMunicipalidad_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                // devolvemos todos los clientes activos
                try
                {
                    var lista = ListaMunicipalidad();
                    if (lista != null)
                        storeMunicipalidad.DataSource = lista;

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

        #region STORE PARTIDO

        private List<Data.GlobalPartidos> ListaPartido()
        {
            List<Data.GlobalPartidos> listaDatos = new List<Data.GlobalPartidos>();
            List<Data.GlobalPartidos> datosAux = new List<Data.GlobalPartidos>();
            GlobalPartidosController cPartido = new GlobalPartidosController();
            try
            {
                long? MunicipalidadID = null;

                if (cmbMunicipalidad.SelectedItem.Value != null && cmbMunicipalidad.SelectedItem.Value != "")

                {
                    MunicipalidadID = long.Parse(cmbMunicipalidad.SelectedItem.Value);

                }
                else
                {
                    if (hdMunicipalidadID.Value != null && hdMunicipalidadID.Value.ToString() != "")

                    {
                        MunicipalidadID = long.Parse(hdMunicipalidadID.Value.ToString());

                    }

                }
                if (MunicipalidadID != null)
                    listaDatos = cPartido.GetItemsList("GlobalMunicipalidadID ==" + MunicipalidadID + " && Activo== true");

                cPartido = null;
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        protected void storePartidos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                // devolvemos todos los clientes activos
                try
                {
                    var lista = ListaPartido();
                    if (lista != null)
                        storePartidos.DataSource = lista;

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

        #region STORE AREAS OYM

        private List<Data.GlobalAreasOYM> ListaAreasOYM()
        {
            long LocalidadID = long.Parse(GridRowSelect.SelectedRecordID);
            GlobalAreasOYMController cGlobalAreasOYM = new GlobalAreasOYMController();
            List<Data.GlobalAreasOYM> listaDatos = new List<Data.GlobalAreasOYM>();

            try
            {
                listaDatos = cGlobalAreasOYM.GetAreasLibresByLocalidad(LocalidadID);

            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
            ;
        }


        private List<Data.Vw_LocalidadesAreasOYM> ListaAreasOYMLocalidad()
        {
            long LocalidadID = long.Parse(GridRowSelect.SelectedRecordID);
            GlobalAreasOYMController cGlobalAreasOYM = new GlobalAreasOYMController();
            List<Data.Vw_LocalidadesAreasOYM> listaDatos = new List<Data.Vw_LocalidadesAreasOYM>();
            try
            {
                listaDatos = cGlobalAreasOYM.GetAreasByLocalidad(LocalidadID);

            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        protected void storeAreasOYMLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.GlobalAreasOYM> datos = ListaAreasOYM();
                    if (datos != null)
                    {
                        storeAreasOYMLibres.DataSource = datos;
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

        protected void storeAreasOYM_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_LocalidadesAreasOYM> lista = ListaAreasOYMLocalidad();
                    if (lista != null)
                    {
                        storeAreasOYM.DataSource = lista;
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

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)

        {
            DirectResponse direct = new DirectResponse();
            GlobalLocalidadesController cLocalidades = new GlobalLocalidadesController();

            #region COMARCH_VARIABLES

            bool bIntegracionComarch = false;
            TreeCore.Integraciones.Comarch.ComarchConnection comarchConnect = null;
            bool conexionSegura = false;
            AddressItemDistrictRequest districtRequest = null;
            bool warning = false;
            bool error = false;
            string tipoAviso = "ERROR";

            #endregion

            try
            {

                #region INTEGRACION_COMARCH

                try
                {
                    if (comarchConnect != null && comarchConnect.IsConectado())
                    {
                        bIntegracionComarch = true;
                    }

                }
                catch (Exception)
                {
                    bIntegracionComarch = false;

                }

                #endregion

                if (!agregar)
                {

                    long S = long.Parse(GridRowSelect.SelectedRecordID);
                    bool Localidad_Modificado = false;
                    bool Codigo_Modificado = false;

                    Data.GlobalLocalidades dato = new Data.GlobalLocalidades();
                    dato = cLocalidades.GetItem(S);


                    if (!txtLocalidad.Text.Equals(dato.Localidad))
                    {
                        Localidad_Modificado = true;
                    }
                    if (txtCodigo.Text != "" && !txtCodigo.Text.Equals(dato.Codigo))
                    {
                        Codigo_Modificado = true;
                    }
                    if (txtLocalidad.Text != "")
                    {
                        dato.Localidad = txtLocalidad.Text;
                    }

                    dato.LocalidadCentroProblado = txtLocalidadCentroPoblado.Text;


                    dato.Codigo = txtCodigo.Text;

                    if (cLocalidades.getLocalidadRepetido(Convert.ToInt32(cmbPartido.SelectedItem.Value), dato.Localidad) && Localidad_Modificado)
                    {
                        direct.Result = GetGlobalResource(Comun.jsYaExiste);//"Este Municipalidad ya está agregado";
                        direct.Success = false;
                        return direct;
                    }
                    else
                    {
                        if (cLocalidades.getLocalidades_CodigoRepetido(Convert.ToInt32(cmbPartido.SelectedItem.Value), dato.Codigo) && Codigo_Modificado)
                        {
                            direct.Result = GetGlobalResource(Comun.jsYaExiste);
                            direct.Success = false;
                            return direct;
                        }
                        else
                        {

                            #region INTEGRACION COMARCH

                            if (bIntegracionComarch)
                            {

                                AddressItemDistrictResponse res = null;
                                Data.GlobalMunicipalidades municipalidad = new Data.GlobalMunicipalidades();
                                GlobalMunicipalidadesController cGlobalMunicipalidades = new GlobalMunicipalidadesController();

                                Data.GlobalPartidos partido = new Data.GlobalPartidos();
                                GlobalPartidosController cPartidos = new GlobalPartidosController();

                                partido = cPartidos.GetItem(dato.GlobalPartidoID);

                                municipalidad = cGlobalMunicipalidades.GetItem(partido.GlobalMunicipalidadID);

                                //Agregamos los datos a enviar.
                                districtRequest = new AddressItemDistrictRequest();

                                districtRequest.OPERATION = Comun.INTEGRACION_SERVICIO_COMARCH_MODIFY;
                                //districtRequest.ADDRESS_ITEM_TYPE = Comun.INTEGRACION_SERVICIO_COMARCH_CREATE_COMUNA;
                                districtRequest.ABBREVIATION_CITY = "";
                                districtRequest.ABBREVIATION_DISTRICT = dato.Codigo;
                                districtRequest.NAME = dato.Localidad;
                                districtRequest.MASTER_DIVPOL_CATEGORY = "";
                                //districtRequest.MASTER_DISTRICT_TYPE = "";
                                districtRequest.LATITUDE = "";
                                districtRequest.LONGITUDE = "";
                                res = comarchConnect.ComarchAddressItemDistrict(conexionSegura, districtRequest, Comun.MODULOGLOBAL, "", Usuario.UsuarioID, null);

                                //Se recibe respuesta
                                if (res != null)
                                {

                                    switch (res.CODE_RESPUESTA)
                                    {
                                        //OSS Internal Error -- Error Interno de OSS
                                        case "0":
                                            tipoAviso = "ERROR";
                                            error = true;

                                            break;

                                        //Invalid credentials for this operation -- Credenciales inválidas para esta operación.
                                        case "101":
                                            tipoAviso = "WARNING";
                                            warning = true;

                                            break;

                                        //District already created. -- Localidad / comuna enviado ya se encuentra creado.
                                        case "109":
                                            tipoAviso = "WARNING";
                                            warning = true;

                                            break;

                                        //Mandatory attribute is missing – addressitemtype
                                        case "110":
                                            tipoAviso = "WARNING";
                                            warning = true;

                                            break;

                                        //Mandatory attribute is missing – masterabbreviation
                                        case "111":
                                            tipoAviso = "WARNING";
                                            warning = true;

                                            break;

                                        //Mandatory attribute is missing – abbreviationdistrict
                                        case "112":
                                            tipoAviso = "WARNING";
                                            warning = true;

                                            break;

                                        //Mandatory attribute is missing – MasterDistrict
                                        case "113":
                                            tipoAviso = "WARNING";
                                            warning = true;

                                            break;

                                        //OK -- Operación exitosa
                                        case "200":
                                            tipoAviso = "INFO";

                                            break;

                                    }

                                    MensajeBox(tipoAviso, res.CODE_RESPUESTA + "-" + res.MENSAJE_RESPUESTA, Ext.Net.MessageBox.Icon.ERROR, null);
                                    direct.Success = true;
                                    if (error || warning)
                                    {

                                        return direct;
                                    }


                                }
                                else
                                {
                                    MensajeBox(tipoAviso, "Error de conexión con el servidor ", Ext.Net.MessageBox.Icon.ERROR, null);
                                    direct.Success = true;
                                    return direct;

                                }

                            }

                            comarchConnect = null;

                            #endregion

                            cLocalidades.UpdateItem(dato);
                        }
                    }

                }
                else
                {
                    Data.GlobalLocalidades dato = new Data.GlobalLocalidades();
                    List<Data.GlobalLocalidades> lista = new List<Data.GlobalLocalidades>();
                    lista = cLocalidades.GetItemsList("GlobalPartidoID==" + Convert.ToInt32(cmbPartido.SelectedItem.Value));
                    dato.Localidad = txtLocalidad.Text;
                    dato.Codigo = txtCodigo.Text;
                    dato.GlobalPartidoID = Convert.ToInt32(cmbPartido.SelectedItem.Value);
                    dato.Activo = true;
                    if (txtLocalidadCentroPoblado.Text != "")
                    {
                        dato.LocalidadCentroProblado = txtLocalidadCentroPoblado.Text;
                    }

                    if (cLocalidades.getLocalidadRepetido(Convert.ToInt32(cmbPartido.SelectedItem.Value), dato.Localidad))
                    {
                        direct.Result = GetGlobalResource(Comun.jsYaExiste);//"Este Municipalidad ya está agregado"
                        direct.Success = false;
                        return direct;
                    }
                    else
                    {
                        if (cLocalidades.getLocalidades_CodigoRepetido(Convert.ToInt32(cmbPartido.SelectedItem.Value), dato.Codigo))
                        {
                            direct.Result = GetGlobalResource(Comun.jsYaExiste);
                            direct.Success = false;
                            return direct;
                        }
                        else
                        {

                            #region INTEGRACION COMARCH

                            if (bIntegracionComarch)
                            {

                                AddressItemDistrictResponse res = null;
                                Data.GlobalMunicipalidades municipalidad = new Data.GlobalMunicipalidades();
                                GlobalMunicipalidadesController cGlobalMunicipalidades = new GlobalMunicipalidadesController();

                                Data.GlobalPartidos partido = new Data.GlobalPartidos();
                                GlobalPartidosController cPartidos = new GlobalPartidosController();

                                partido = cPartidos.GetItem(dato.GlobalPartidoID);

                                municipalidad = cGlobalMunicipalidades.GetItem(partido.GlobalMunicipalidadID);

                                //Agregamos los datos a enviar.
                                districtRequest = new AddressItemDistrictRequest();

                                districtRequest.OPERATION = Comun.INTEGRACION_SERVICIO_COMARCH_CREATE;
                                //districtRequest.ADDRESS_ITEM_TYPE = Comun.INTEGRACION_SERVICIO_COMARCH_CREATE_COMUNA;
                                districtRequest.ABBREVIATION_CITY = "";
                                districtRequest.ABBREVIATION_DISTRICT = dato.Codigo;
                                districtRequest.NAME = dato.Localidad;
                                districtRequest.MASTER_DIVPOL_CATEGORY = "";
                                //districtRequest.MASTER_DISTRICT_TYPE = "";
                                districtRequest.LATITUDE = "";
                                districtRequest.LONGITUDE = "";
                                res = comarchConnect.ComarchAddressItemDistrict(conexionSegura, districtRequest, Comun.MODULOGLOBAL, "", Usuario.UsuarioID, null);

                                //Se recibe respuesta
                                if (res != null)
                                {

                                    switch (res.CODE_RESPUESTA)
                                    {
                                        //OSS Internal Error -- Error Interno de OSS
                                        case "0":
                                            tipoAviso = "ERROR";
                                            error = true;

                                            break;

                                        //Invalid credentials for this operation -- Credenciales inválidas para esta operación.
                                        case "101":
                                            tipoAviso = "WARNING";
                                            warning = true;

                                            break;

                                        //District already created. -- Localidad / comuna enviado ya se encuentra creado.
                                        case "109":
                                            tipoAviso = "WARNING";
                                            warning = true;

                                            break;

                                        //Mandatory attribute is missing – addressitemtype
                                        case "110":
                                            tipoAviso = "WARNING";
                                            warning = true;

                                            break;

                                        //Mandatory attribute is missing – masterabbreviation
                                        case "111":
                                            tipoAviso = "WARNING";
                                            warning = true;

                                            break;

                                        //Mandatory attribute is missing – abbreviationdistrict
                                        case "112":
                                            tipoAviso = "WARNING";
                                            warning = true;

                                            break;

                                        //Mandatory attribute is missing – MasterDistrict
                                        case "113":
                                            tipoAviso = "WARNING";
                                            warning = true;

                                            break;

                                        //OK -- Operación exitosa
                                        case "200":
                                            tipoAviso = "INFO";

                                            break;

                                    }

                                    MensajeBox(tipoAviso, res.CODE_RESPUESTA + "-" + res.MENSAJE_RESPUESTA, Ext.Net.MessageBox.Icon.ERROR, null);
                                    direct.Success = true;
                                    if (error || warning)
                                    {

                                        return direct;
                                    }


                                }
                                else
                                {
                                    MensajeBox(tipoAviso, "Error de conexión con el servidor ", Ext.Net.MessageBox.Icon.ERROR, null);
                                    direct.Success = true;
                                    return direct;

                                }

                            }

                            comarchConnect = null;

                            #endregion

                            cLocalidades.AddItem(dato);
                        }
                    }

                }
                cLocalidades = null;
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
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            GlobalLocalidadesController cController = new GlobalLocalidadesController();

            try
            {
                long ID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GlobalLocalidades dato = new Data.GlobalLocalidades();
                dato = cController.GetItem(ID);
                txtLocalidad.Text = dato.Localidad;
                txtCodigo.Text = dato.Codigo;
                if (dato.LocalidadCentroProblado != null)
                {
                    txtLocalidadCentroPoblado.Text = dato.LocalidadCentroProblado;
                }
                winGestion.Show();
                cController = null;
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
            GlobalLocalidadesController cController = new GlobalLocalidadesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.GlobalLocalidades oDato = new Data.GlobalLocalidades();

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cController.GetItem("Defecto == True");

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.GlobalLocalidadID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cController.UpdateItem(oDato);
                        }

                        oDato = cController.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        cController.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cController.GetItem(lID);
                    oDato.Defecto = true;
                    cController.UpdateItem(oDato);
                }

                log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));

                cController = null;
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
            GlobalLocalidadesController cController = new GlobalLocalidadesController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cController.RegistroDefecto(lID))
                {
                    direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                    direct.Success = false;
                }
                else if (cController.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }

                cController = null;
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
            GlobalLocalidadesController cController = new GlobalLocalidadesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GlobalLocalidades oDato = new Data.GlobalLocalidades();
                oDato = cController.GetItem(lID);
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

            cController = null;
            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion

        #region GESTION RADIO

        [DirectMethod()]
        public DirectResponse AsignarRadio()
        {
            DirectResponse direct = new DirectResponse();
            GlobalLocalidadesController cLocalidades = new GlobalLocalidadesController();
            try
            {
                if (GridRowSelect.SelectedRows.Count > 0)
                {
                    foreach (SelectedRow selec in GridRowSelect.SelectedRows)
                    {
                        Data.GlobalLocalidades dato = new Data.GlobalLocalidades();
                        dato = cLocalidades.GetItem(long.Parse(selec.RecordID));
                        if (dato != null)
                        {
                            dato.Radio = numRadio.Value != null ? (double?)numRadio.Value : null;
                            cLocalidades.UpdateItem(dato);
                        }
                    }
                    cLocalidades = null;
                }
                else
                {
                    long partidoID = Convert.ToInt64(cmbPartido.SelectedItem.Value);
                    //List<Data.RegionesPaises> Municipalidades = cLocalidades.getAllRegionPaisByPaisID(paisID);

                    List<Data.GlobalLocalidades> Municipalidades = cLocalidades.getAllLocalidadesByPartidoID(partidoID);


                    foreach (Data.GlobalLocalidades dato in Municipalidades)
                    {
                        using (GlobalLocalidadesController controller = new GlobalLocalidadesController())
                        {
                            //if (numRadio.Value != null && numRadio.Value.ToString() != "")
                            //{
                            Data.GlobalLocalidades aux = controller.GetItem(dato.GlobalLocalidadID);
                            aux.Radio = numRadio.Value != null ? (double?)numRadio.Value : null; //numRadio.Number;
                            controller.UpdateItem(aux);
                            //}
                        }
                    }
                }

                storePrincipal.DataBind();

            }
            catch (Exception)
            {
            }

            direct.Success = true;
            direct.Result = "";
            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditarRadio()
        {
            DirectResponse direct = new DirectResponse();
            GlobalLocalidadesController cMunicipalidad = new GlobalLocalidadesController();

            try
            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GlobalLocalidades dato = new Data.GlobalLocalidades();
                dato = cMunicipalidad.GetItem(S);

                if (dato != null && dato.Radio != null)
                {
                    numRadio.Number = (double)dato.Radio;
                }

                winRadio.Show();
                cMunicipalidad = null;
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

        #region AREAS O&M

        [DirectMethod()]
        public DirectResponse AgregarArea()
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                long LocalidadID = long.Parse(GridRowSelect.SelectedRecordID);

                foreach (SelectedRow selec in SeleccionarAreaOYMLIbresRowSelection.SelectedRows)
                {
                    long AreaID = long.Parse(selec.RecordID);

                    LocalidadesAreasOYMController cLocalidadesAreasOYM = new LocalidadesAreasOYMController();
                    Data.LocalidadesAreasOYM localidadesAreasOYM = new Data.LocalidadesAreasOYM();
                    localidadesAreasOYM.GlobalLocalidadID = LocalidadID;
                    localidadesAreasOYM.GlobalAreaOYMID = AreaID;

                    cLocalidadesAreasOYM.AddItem(localidadesAreasOYM);
                }
            }
            catch (Exception)
            {

            }
            return direct;
        }

        [DirectMethod()]
        public DirectResponse QuitarArea()
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                foreach (SelectedRow selec in SeleccionarAreaOYMRowSelection.SelectedRows)
                {
                    long localidadAreaOYMId = Convert.ToInt64(selec.RecordID);

                    LocalidadesAreasOYMController cLocalidadesAreasOYM = new LocalidadesAreasOYMController();
                    Data.LocalidadesAreasOYM a = cLocalidadesAreasOYM.GetItem(localidadAreaOYMId);
                    if (cLocalidadesAreasOYM.DeleteItem(localidadAreaOYMId))
                    {
                        storeAreasOYM.DataBind();
                    }
                }

            }
            catch (Exception)
            {

            }
            return direct;
        }

        #endregion

        #region Gestion Adicional

        /// <summary>
        /// MOSTRAR EDITAR INFORMACION COMARCH
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public DirectResponse MostrarEditarAgregarAdicional()
        {
            DirectResponse ajax = new DirectResponse();
            ajax.Result = "";
            ajax.Success = true;

            try
            {
                long ID = Int64.Parse(GridRowSelect.SelectedRecordID);
                Data.GlobalLocalidades GlobalLocalidad = new Data.GlobalLocalidades();
                GlobalLocalidadesController cGlobalLocalidades = new GlobalLocalidadesController();
                GlobalLocalidad = cGlobalLocalidades.GetItem(ID);

                #region DATOS COMARCH

                if (GlobalLocalidad.DDN != null)
                {
                    txtDDN.Text = GlobalLocalidad.DDN;
                }
                else
                {
                    txtDDN.Text = "";
                }
                if (GlobalLocalidad.Subalm != null)
                {
                    txtSubalm.Text = GlobalLocalidad.Subalm;
                }
                else
                {
                    txtSubalm.Text = "";
                }
                if (GlobalLocalidad.DescSubalm != null)
                {
                    txtDescSubalm.Text = GlobalLocalidad.DescSubalm;
                }
                else
                {
                    txtDescSubalm.Text = "";
                }

                if (GlobalLocalidad.CabeceraComercial != null)
                {
                    txtCabeceraComercial.Text = GlobalLocalidad.CabeceraComercial;
                }
                else
                {
                    txtCabeceraComercial.Text = "";
                }

                if (GlobalLocalidad.RegionComercial != null)
                {
                    txtRegionComercial.Text = GlobalLocalidad.RegionComercial;
                }
                else
                {
                    txtRegionComercial.Text = "";
                }
                if (GlobalLocalidad.SubregionComercial != null)
                {
                    txtSubregionComercial.Text = GlobalLocalidad.SubregionComercial;
                }
                else
                {
                    txtSubregionComercial.Text = "";
                }
                if (GlobalLocalidad.ZIC != null)
                {
                    txtZIC.Text = GlobalLocalidad.ZIC;
                }
                else
                {
                    txtZIC.Text = "";
                }

                if (GlobalLocalidad.CabeceraOYM != null)
                {
                    txtCabeceraOM.Text = GlobalLocalidad.CabeceraOYM;
                }
                else
                {
                    txtCabeceraOM.Text = "";
                }

                if (GlobalLocalidad.Zona != null)
                {
                    txtZona.Text = GlobalLocalidad.Zona;
                }
                else
                {
                    txtZona.Text = "";
                }

                if (GlobalLocalidad.DimIntegral != null)
                {
                    chkdimIntegral.Checked = (bool)GlobalLocalidad.DimIntegral;
                }
                else
                {
                    chkdimIntegral.Checked = false;
                }

                if (GlobalLocalidad.CodigoEnacom != null)
                {
                    txtcodigoenacom.Text = GlobalLocalidad.CodigoEnacom;
                }
                else
                {
                    txtcodigoenacom.Text = "";
                }

                if (GlobalLocalidad.Anio != null)
                {
                    nbrAno.Number = Convert.ToInt32(GlobalLocalidad.Anio);
                }
                else
                {
                    nbrAno.SetRawValue(""); 
                }

                if (GlobalLocalidad.EsEstimado != null)
                {
                    chkEstimado.Checked = (bool)GlobalLocalidad.EsEstimado;
                }
                else
                {
                    chkEstimado.Checked = false;
                }

                if (GlobalLocalidad.CantidadHabitantes != null)
                {
                    nbrNumeroHabitnates.Number = (int)GlobalLocalidad.CantidadHabitantes;
                }
                else
                {
                    nbrNumeroHabitnates.SetRawValue("");
                }
                if (GlobalLocalidad.NBI != null)
                {
                    PorcentajeNBI.Number = (int)GlobalLocalidad.NBI;
                }
                else
                {
                    PorcentajeNBI.SetRawValue("");
                }

                if (GlobalLocalidad.RangoEtario != null)
                {
                    nbrRangoEtario.Number = (int)GlobalLocalidad.RangoEtario;
                }
                else
                {
                    nbrRangoEtario.SetRawValue("");
                }

                if (GlobalLocalidad.Latitud != null)
                {
                    nbrLatitud.Number = (int)GlobalLocalidad.Latitud;
                }
                else
                {
                    nbrLatitud.SetRawValue("");
                }

                if (GlobalLocalidad.RangoEtario != null)
                {
                    nbrRangoEtario.Number = (int)GlobalLocalidad.RangoEtario;
                }
                else
                {
                    nbrRangoEtario.SetRawValue("");
                }

                if (GlobalLocalidad.Latitud != null)
                {
                    nbrLatitud.Number = (int)GlobalLocalidad.Latitud;
                }
                else
                {
                    nbrLatitud.SetRawValue("");
                }
                if (GlobalLocalidad.Longitud != null)
                {
                    nbrLongitud.Number = (int)GlobalLocalidad.Longitud;
                }
                else
                {
                    nbrLongitud.SetRawValue("");
                }

                #endregion

                winGestionAdicional.Show();

                cGlobalLocalidades = null;
            }
            catch (Exception)
            {

            }
            return ajax;
        }

        /// <summary>
        /// AGREGAR EDITAR INFORMACION COMARCH
        /// </summary>
        /// <param name="agregar"></param>
        /// <returns></returns>
        [DirectMethod]
        public DirectResponse AgregarEditarAdicional()
        {
            DirectResponse direct = new DirectResponse();
            direct.Result = "";
            direct.Success = true;

            try
            {

                long ID = Int64.Parse(GridRowSelect.SelectedRecordID);
                Data.GlobalLocalidades GlobalLocalidad = new Data.GlobalLocalidades();
                GlobalLocalidadesController cGlobalLocalidades = new GlobalLocalidadesController();
                GlobalLocalidad = cGlobalLocalidades.GetItem(ID);

                GlobalLocalidad.DDN = txtDDN.Text;
                GlobalLocalidad.Subalm = txtSubalm.Text;
                GlobalLocalidad.DescSubalm = txtDescSubalm.Text;
                GlobalLocalidad.CabeceraComercial = txtCabeceraComercial.Text;
                GlobalLocalidad.RegionComercial = txtRegionComercial.Text;
                GlobalLocalidad.SubregionComercial = txtSubregionComercial.Text;
                GlobalLocalidad.ZIC = txtZIC.Text;
                GlobalLocalidad.CabeceraOYM = txtCabeceraOM.Text;
                GlobalLocalidad.Zona = txtZona.Text;
                GlobalLocalidad.DimIntegral = chkdimIntegral.Checked;
                GlobalLocalidad.CodigoEnacom = txtcodigoenacom.Text;
                if (nbrAno.Text != "")
                {
                    GlobalLocalidad.Anio = (int?)nbrAno.Number;
                }
                else
                {
                    GlobalLocalidad.Anio = null;
                }

                GlobalLocalidad.EsEstimado = chkEstimado.Checked;

                if (nbrNumeroHabitnates.Text != "")
                {
                    GlobalLocalidad.CantidadHabitantes = (int?)nbrNumeroHabitnates.Number;
                }
                else
                {
                    GlobalLocalidad.CantidadHabitantes = null;
                }
                if (PorcentajeNBI.Text != "")
                {
                    GlobalLocalidad.NBI = (int?)PorcentajeNBI.Number;
                }
                else
                {
                    GlobalLocalidad.NBI = null;
                }
                if (nbrRangoEtario.Text != "")
                {
                    GlobalLocalidad.RangoEtario = (int?)nbrRangoEtario.Number;
                }
                else
                {
                    GlobalLocalidad.RangoEtario = null;
                }
                if (nbrLatitud.Text != "")
                {
                    GlobalLocalidad.Latitud = (int?)nbrLatitud.Number;
                }
                else
                {
                    GlobalLocalidad.Latitud = null;
                }
                if (nbrLongitud.Text != "")
                {
                    GlobalLocalidad.Longitud = (int?)nbrLongitud.Number;
                }
                else
                {
                    GlobalLocalidad.Longitud = null;
                }
                if (cGlobalLocalidades.UpdateItem(GlobalLocalidad))
                {
                    storePrincipal.DataBind();
                }
                cGlobalLocalidades = null;
            }
            catch (Exception)
            {

            }
            return direct;
        }
        #endregion
    }
}