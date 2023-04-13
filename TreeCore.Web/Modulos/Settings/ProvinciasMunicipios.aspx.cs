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
    public partial class ProvinciasMunicipios : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> ListaFuncionalidades = new List<Data.Vw_Funcionalidades>();
        long lMaestroID = 0;

        #region EVENTOS PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
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
                        string sModuloID = Request.QueryString["aux"].ToString();
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;
                        string[] param = Request.QueryString["aux"].ToString().Split(';');
                        string sGrid = Request.QueryString["aux3"];
                        long regionID = 0;
                        RegionesPaisesController cRegionesPaises = new RegionesPaisesController();


                        #region MAESTRO

                        if (sGrid == "-1")
                        {
                            List<Data.Provincias> ListaoDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, long.Parse(sModuloID), CliID);

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridMaestro.ColumnModel, ListaoDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
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
                            List<Data.Municipios> ListaoDatosDetalle = ListaDetalle(0, 0, sOrden, sDir, ref iCount, sFiltro, long.Parse(sModuloID), CliID);

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(GridDetalle.ColumnModel, ListaoDatosDetalle, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
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
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> { btnDescargar, btnDescargarDetalle }},
                { "Post", new List<ComponentBase> { btnAnadir, btnAgregarInformacionComarch, btnAnadirDetalle, btnAgregarInformacionComarchDetalle }},
                { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnDefecto, btnRadio, btnEditarDetalle, btnActivarDetalle, btnRadioDetalle, btnDefectoDetalle }},
                { "Delete", new List<ComponentBase> { btnEliminar, btnEliminarDetalle }}
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
                    string sSort, sDir;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];
                    long regionID = 0;
                    RegionesPaisesController cRegionesPaises = new RegionesPaisesController();
                    if (cmbRegiones.SelectedItem.Value != null && cmbRegiones.SelectedItem.Value != "")
                    {
                        regionID = long.Parse(cmbRegiones.SelectedItem.Value);
                    }
                    else
                    {
                        regionID = 0;
                    }

                    //Recupera los oDatos y los establece
                    var ls = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, regionID, long.Parse(hdCliID.Value.ToString()));
                    if (ls != null)
                    {
                        storePrincipal.DataSource = ls;

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

        private List<Data.Provincias> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long regionID, long ClienteID)
        {
            List<Data.Provincias> ListaoDatos;
            ProvinciasController CProvincias = new ProvinciasController();


            try
            {
                if (regionID != 0 && ClienteID != 0)
                {
                    ListaoDatos = CProvincias.GetItemsWithExtNetFilterList<Data.Provincias>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "RegionPaisID == " + regionID);
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
                    else
                    {
                        lMaestroID = 0;
                    }

                    var vLista = ListaDetalle(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, lMaestroID, long.Parse(hdCliID.Value.ToString()));
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

        private List<Data.Municipios> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID, long ClienteID)
        {
            List<Data.Municipios> ListaoDatos;
            MunicipiosController CMunicipios = new MunicipiosController();

            try
            {
                if (lMaestroID != 0 && ClienteID != 0)
                {
                    ListaoDatos = CMunicipios.GetItemsWithExtNetFilterList<Data.Municipios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ProvinciaID == " + lMaestroID);
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

        #region PAISES

        protected void storePaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                PaisesController cPaises = new PaisesController();

                try
                {
                    var ls = cPaises.GetActivos(long.Parse(hdCliID.Value.ToString()));
                    if (ls != null)
                        storePaises.DataSource = ls;


                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        #endregion

        #region REGIONES

        protected void storeRegiones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                PaisesController cPaises = new PaisesController();
                RegionesPaisesController cRegionesPaises = new RegionesPaisesController();
                List<Data.RegionesPaises> Lista_RegionesPaises = new List<Data.RegionesPaises>();
                try
                {
                    if (cmbPais.SelectedItem.Value != null)
                        Lista_RegionesPaises = cRegionesPaises.GetListRegionPaisActivoByPaisID(long.Parse(cmbPais.Value.ToString()));
                    if (Lista_RegionesPaises != null)
                        storeRegiones.DataSource = Lista_RegionesPaises;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        #endregion

        #endregion

        #region DIRECT METHOD

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)
        {
            DirectResponse direct = new DirectResponse();

            ProvinciasController cProvincia = new ProvinciasController();
            RegionesPaisesController cRegionesPaises = new RegionesPaisesController();
            long regionID = long.Parse(cmbRegiones.Value.ToString());
            #region COMARCH_VARIABLES
            TreeCore.Integraciones.Comarch.ComarchConnection comarchConnect = null;

            #endregion

            #region INTEGRACION_COMARCH
            try
            {
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.Provincias oDato = cProvincia.GetItem(S);

                    if (oDato.Provincia == txtProvincia.Text && oDato.Codigo == txtCodigoProvincia.Text)
                    {
                        oDato.Provincia = txtProvincia.Text;
                        oDato.Codigo = txtCodigoProvincia.Text;
                    }
                    else
                    {
                        if (cProvincia.RegistroDuplicadoByRegionID(txtProvincia.Text, regionID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.Provincia = txtProvincia.Text;
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

                    if (cProvincia.RegistroDuplicadoByRegionID(txtProvincia.Text, regionID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.Provincias oDato = new Data.Provincias();
                        oDato.Provincia = txtProvincia.Text;
                        oDato.Codigo = txtCodigoProvincia.Text;
                        oDato.Activo = true;
                        oDato.RegionPaisID = regionID;



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

            ProvinciasController cProvincias = new ProvinciasController();


            try

            {
                long S = long.Parse(GridRowSelect.SelectedRows[0].RecordID);

                Data.Provincias oDato = cProvincias.GetItem(S);
                txtProvincia.Text = oDato.Provincia;
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
            ProvinciasController cProvincias = new ProvinciasController();
            MunicipiosController cMunicipios = new MunicipiosController();
            Data.Provincias oDato;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRows[0].RecordID);
                long lregionID = long.Parse(cmbRegiones.Value.ToString());
                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cProvincias.GetDefaultRegion(lregionID);

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.ProvinciaID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cMunicipios.EliminarDefecto(oDato.ProvinciaID);
                            cProvincias.UpdateItem(oDato);
                        }

                        oDato = cProvincias.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
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
            ProvinciasController CProvincias = new ProvinciasController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CProvincias.DeleteItem(lID))
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

        #region DIRECT METHOD DETALLE

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
            DirectResponse ajax = new DirectResponse();

            MunicipiosController cMun = new MunicipiosController();
            long provinciaID = long.Parse(GridRowSelect.SelectedRecordID);

            long cliID = 0;
            try
            {
                if (!agregar)
                {
                   long S = Int64.Parse(GridRowSelectDetalle.SelectedRows[0].RecordID);
                    Data.Municipios oDato = cMun.GetItem(S);

                    if (oDato.Municipio == txtMunicipio.Text && oDato.Codigo == txtCodigoMunicipio.Text)
                    {
                        oDato.Municipio = txtMunicipio.Text;
                        oDato.Codigo = txtCodigoMunicipio.Text;
                        oDato.FactorZona = Convert.ToDouble(txtFactorZona.Text);
                        oDato.FactorComuna = Convert.ToDouble(txtFactorMunicipio.Text);
                        oDato.Factor = Convert.ToDouble(txtFactor.Text);
                    }
                    else
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());
                        if (cMun.RegistroDuplicado(txtMunicipio.Text, txtCodigoMunicipio.Text, provinciaID, S))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.Municipio = txtMunicipio.Text;
                            oDato.Codigo = txtCodigoMunicipio.Text;
                            oDato.FactorZona = Convert.ToDouble(txtFactorZona.Text);
                            oDato.FactorComuna = Convert.ToDouble(txtFactorMunicipio.Text);
                            oDato.Factor = Convert.ToDouble(txtFactor.Text);
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

                    if (cMun.RegistroDuplicado(txtMunicipio.Text, txtCodigoMunicipio.Text, provinciaID, 0))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.Municipios oDato = new Data.Municipios();
                        oDato.Municipio = txtMunicipio.Text;
                        oDato.Codigo = txtCodigoMunicipio.Text;
                        oDato.FactorZona = Convert.ToDouble(txtFactorZona.Text);
                        oDato.FactorComuna = Convert.ToDouble(txtFactorMunicipio.Text);
                        oDato.Factor = Convert.ToDouble(txtFactor.Text);
                        oDato.Activo = true;
                        oDato.ProvinciaID = provinciaID;
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
                long ID = Int64.Parse(GridRowSelectDetalle.SelectedRows[0].RecordID);
                MunicipiosController cMunicipios = new MunicipiosController();
                Data.Municipios oDato = cMunicipios.GetItem(ID);

                txtMunicipio.Text = oDato.Municipio;
                txtCodigoMunicipio.Text = oDato.Codigo;
                if (oDato.FactorZona != null)
                {
                    txtFactorZona.Text = oDato.FactorZona.ToString();
                }
                else
                {
                    txtFactorZona.Text = "0";
                }
                if (oDato.FactorComuna != null)
                {
                    txtFactorMunicipio.Text = oDato.FactorComuna.ToString();
                }
                else
                {
                    txtFactorMunicipio.Text = "0";
                }
                if (oDato.Factor != null)
                {
                    txtFactor.Text = oDato.Factor.ToString();
                }
                else
                {
                    txtFactor.Text = "0";
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
        public DirectResponse AsignarPorDefectoDetalle()
        {
            DirectResponse direct = new DirectResponse();
            MunicipiosController cMunicipios = new MunicipiosController();

            try
            {
                long lID = Int64.Parse(GridRowSelectDetalle.SelectedRows[0].RecordID);
                long lProvinciaID = long.Parse(GridRowSelect.SelectedRows[0].RecordID);
                Data.Municipios oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cMunicipios.GetDefaultProvincia(lProvinciaID);

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.MunicipioID != lID)
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
            MunicipiosController CMunicipios = new MunicipiosController();

            direct.Result = "";
            direct.Success = true;

            long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

            try
            {
                if (CMunicipios.DeleteItem(lID))
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
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            ProvinciasController cProvincias = new ProvinciasController();
            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.Provincias oDato = cProvincias.GetItem(lID);
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
            MunicipiosController CMunicipios = new MunicipiosController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.Municipios oDato = CMunicipios.GetItem(lID);
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
            ProvinciasController cPaises = new ProvinciasController();
            try
            {
                if (GridRowSelect.SelectedRows.Count > 0)
                {
                    foreach (SelectedRow selec in GridRowSelect.SelectedRows)
                    {
                        Data.Provincias oDato = cPaises.GetItem(long.Parse(selec.RecordID));
                        if (oDato != null)
                        {
                            oDato.Radio = numRadio.Value != null ? (double?)numRadio.Value : null;
                            cPaises.UpdateItem(oDato);
                        }
                    }
                }
                else
                {
                    long municipioID = Convert.ToInt64(cmbRegiones.SelectedItem.Value);
                    List<Data.Provincias> Municipalidades = cPaises.getAllProvinciasByRegionPaisID(municipioID);

                    foreach (Data.Provincias oDato in Municipalidades)
                    {
                        using (ProvinciasController controller = new ProvinciasController())
                        {
                            Data.Provincias aux = controller.GetItem(oDato.ProvinciaID);
                            aux.Radio = numRadio.Value != null ? (double?)numRadio.Value : null;
                            controller.UpdateItem(aux);
                        }
                    }
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
            ProvinciasController cMunicipalidad = new ProvinciasController();

            try
            {
                long S = long.Parse(GridRowSelect.SelectedRows[0].RecordID);

                Data.Provincias oDato = cMunicipalidad.GetItem(S);

                if (oDato != null && oDato.Radio != null)
                {
                    numRadio.Number = (double)oDato.Radio;
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
            MunicipiosController cMunicipios = new MunicipiosController();
            try
            {
                if (GridRowSelectDetalle.SelectedRows.Count > 0)
                {
                    foreach (SelectedRow selec in GridRowSelectDetalle.SelectedRows)
                    {
                        Data.Municipios oDato = cMunicipios.GetItem(long.Parse(selec.RecordID));
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
                    List<Data.Municipios> Municipios = cMunicipios.getAllMunicipiosByProvID(provinciaID);

                    foreach (Data.Municipios oDato in Municipios)
                    {
                        using (MunicipiosController controller = new MunicipiosController())
                        {
                            Data.Municipios aux = controller.GetItem(oDato.MunicipioID);
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
            MunicipiosController cMunicipalidades = new MunicipiosController();

            try
            {
                long S = long.Parse(GridRowSelectDetalle.SelectedRows[0].RecordID);

                Data.Municipios oDato = cMunicipalidades.GetItem(S);

                if (oDato != null && oDato.Radio != null)
                {
                    numRadioDetalle.Number = (double)oDato.Radio;
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
                Data.Provincias provincia;
                ProvinciasController cProvincias = new ProvinciasController();
                provincia = cProvincias.GetItem(ID);

                #region oDatoS COMARCH

                if (provincia.RegionComercial != null)
                {
                    txtRegionComercial.Text = provincia.RegionComercial;
                }
                else
                {
                    txtRegionComercial.Text = "";
                }
                if (provincia.ZonaMinisterio != null)
                {
                    txtZonaMinisterio.Text = provincia.ZonaMinisterio;
                }
                else
                {
                    txtZonaMinisterio.Text = "";
                }
                if (provincia.RegionalTX != null)
                {
                    txtRegionalTX.Text = provincia.RegionalTX;
                }
                else
                {
                    txtRegionalTX.Text = "";
                }
                if (provincia.ResponsableTX != null)
                {
                    txtResponsableTX.Text = provincia.ResponsableTX;
                }
                else
                {
                    txtResponsableTX.Text = "";
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
                    bIntegracionComarch = false;
                }

                #endregion

                long ID = Int64.Parse(GridRowSelect.SelectedRows[0].RecordID);
                Data.Provincias provinciaoDato = new Data.Provincias();
                ProvinciasController cProvinciasoDatos = new ProvinciasController();
                provinciaoDato = cProvinciasoDatos.GetItem(ID);

                provinciaoDato.RegionComercial = txtRegionComercial.Text;
                provinciaoDato.ZonaMinisterio = txtZonaMinisterio.Text;
                provinciaoDato.RegionalTX = txtRegionalTX.Text;
                provinciaoDato.ResponsableTX = txtResponsableTX.Text;


                #region INTEGRACION COMARCH

                if (bIntegracionComarch)
                {

                    AddressItemRegionResponse res = null;

                    ////Agregamos los oDatos a enviar.
                    regionRequest = new AddressItemRegionRequest();

                    if ((provinciaoDato.RegionComercial != null && provinciaoDato.RegionComercial != "")
                        || (provinciaoDato.ZonaMinisterio != null && provinciaoDato.ZonaMinisterio != "")
                        || (provinciaoDato.RegionalTX != null && provinciaoDato.RegionalTX != "")
                        || (provinciaoDato.ResponsableTX != null && provinciaoDato.ResponsableTX != ""))

                    {
                        regionRequest.OPERATION = Comun.INTEGRACION_SERVICIO_COMARCH_MODIFY;
                    }
                    else
                    {

                        regionRequest.OPERATION = Comun.INTEGRACION_SERVICIO_COMARCH_CREATE;
                    }

                    regionRequest.ABBREVIATION = provinciaoDato.Codigo;
                    regionRequest.MASTER_COMMERCIAL_CLASS = provinciaoDato.RegionComercial;
                    regionRequest.MASTER_MINISTRY_AREA = provinciaoDato.ZonaMinisterio;
                    regionRequest.MASTER_REGIONAL_TX = provinciaoDato.RegionalTX;
                    regionRequest.REGION_NAME = provinciaoDato.Provincia;
                    regionRequest.MASTER_RESPONSIBLE_TX = provinciaoDato.ResponsableTX;
                    res = comarchConnect.ComarchAddressItemRegion(conexionSegura, regionRequest, Comun.MODULOGLOBAL, "", Usuario.UsuarioID, null);

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

                            //Region already created. -- Departamento enviado ya se encuentra creado.
                            case "103":
                                tipoAviso = "WARNING";
                                warning = true;

                                break;

                            //Missing manoDatory attribute – operation
                            case "104":
                                tipoAviso = "WARNING";
                                warning = true;

                                break;

                            //Missing manoDatory attribute – abbreviation
                            case "105":
                                tipoAviso = "WARNING";
                                warning = true;

                                break;

                            //Missing manoDatory attribute – abbreviationregion
                            case "106":
                                tipoAviso = "WARNING";
                                warning = true;

                                break;

                            //Missing manoDatory attribute – masterregionname
                            case "107":
                                tipoAviso = "WARNING";
                                warning = true;

                                break;

                            //Missing attribute - name
                            case "108":
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



                if (cProvinciasoDatos.UpdateItem(provinciaoDato))
                {

                    storeDetalle.DataBind();

                }
                cProvinciasoDatos = null;


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

        #region INFORMACION COMARCH DETALLE

        /// <summary>
        /// MOSTRAR EDITAR INFORMACION COMARCH
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public DirectResponse MostrarEditarAgregarComarchDetalle()
        {
            DirectResponse ajax = new DirectResponse();
            ajax.Result = "";
            ajax.Success = true;

            try
            {

                long ID = Int64.Parse(GridRowSelectDetalle.SelectedRows[0].RecordID);
                Data.Municipios municipio = new Data.Municipios();
                MunicipiosController cMunicipios = new MunicipiosController();
                municipio = cMunicipios.GetItem(ID);

                #region oDatoS COMARCH

                if (municipio.ZonaCrc != null)
                {
                    txtZonaCrc.Text = municipio.ZonaCrc;
                }
                else
                {
                    txtZonaCrc.Text = "";
                }
                if (municipio.CiudadCapital != null)
                {
                    txtCiudadCapital.Text = municipio.CiudadCapital;
                }
                else
                {
                    txtCiudadCapital.Text = "";
                }
                if (municipio.MciCiudadPrincipal != null)
                {
                    txtMciCiudadPrincipal.Text = municipio.MciCiudadPrincipal;
                }
                else
                {
                    txtMciCiudadPrincipal.Text = "";
                }
                if (municipio.MciCiudadGrupo != null)
                {
                    txtMciCiudadGrupo.Text = municipio.MciCiudadGrupo;
                }
                else
                {
                    txtMciCiudadGrupo.Text = "";
                }
                if (municipio.ProyeccionPoblacion != null)
                {
                    txtProyeccionPoblacion.Text = municipio.ProyeccionPoblacion;
                }
                else
                {
                    txtProyeccionPoblacion.Text = "";
                }
                if (municipio.GrupoMpio != null)
                {
                    txtGrupoMpio.Text = municipio.GrupoMpio;
                }
                else
                {
                    txtGrupoMpio.Text = "";
                }
                if (municipio.Categorization != null)
                {
                    txtCategorization.Text = municipio.Categorization;
                }
                else
                {
                    txtCategorization.Text = "";
                }
                if (municipio.Ambito != null)
                {
                    txtAmbito.Text = municipio.Ambito;
                }
                else
                {
                    txtAmbito.Text = "";
                }
                if (municipio.ProcentajeCoberturaLTE != null)
                {
                    txtProcentajeCoberturaLTE.Text = municipio.ProcentajeCoberturaLTE;
                }
                else
                {
                    txtProcentajeCoberturaLTE.Text = "";
                }
                if (municipio.Mercado != null)
                {
                    txtMercado.Text = municipio.Mercado;
                }
                else
                {
                    txtMercado.Text = "";
                }
                if (municipio.CiudadCapital != null)
                {
                    txtCiudadCapital.Text = municipio.CiudadCapital;
                }
                else
                {
                    txtCiudadCapital.Text = "";
                }


                #endregion



                winGestionComarchDetalle.Show();

                cMunicipios = null;
            }
            catch (Exception ex)
            {
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

            }
            return ajax;
        }

        /// <summary>
        /// AGREGAR EDITAR INFORMACION COMARCH
        /// </summary>
        /// <param name="agregar"></param>
        /// <returns></returns>
        [DirectMethod]
        public DirectResponse AgregarEditarComarchDetalle()
        {
            DirectResponse direct = new DirectResponse();
            direct.Result = "";
            direct.Success = true;

            #region COMARCH_VARIABLES

            bool bIntegracionComarch = false;
            TreeCore.Integraciones.Comarch.ComarchConnection comarchConnect = null;
            bool conexionSegura = false;
            AddressItemCityRequest cityRequest = null;
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
                catch (Exception)
                {
                    bIntegracionComarch = false;
                }

                #endregion

                long ID = Int64.Parse(GridRowSelectDetalle.SelectedRows[0].RecordID);
                Data.Municipios municipio = new Data.Municipios();
                MunicipiosController cMunicipios = new MunicipiosController();
                municipio = cMunicipios.GetItem(ID);

                municipio.ZonaCrc = txtZonaCrc.Text;
                municipio.CiudadCapital = txtCiudadCapital.Text;
                municipio.MciCiudadPrincipal = txtMciCiudadPrincipal.Text;
                municipio.MciCiudadGrupo = txtMciCiudadGrupo.Text;
                municipio.ProyeccionPoblacion = txtProyeccionPoblacion.Text;
                municipio.GrupoMpio = txtGrupoMpio.Text;
                municipio.Categorization = txtCategorization.Text;
                municipio.Ambito = txtAmbito.Text;
                municipio.ProcentajeCoberturaLTE = txtProcentajeCoberturaLTE.Text;
                municipio.Mercado = txtMercado.Text;

                #region INTEGRACION COMARCH

                if (bIntegracionComarch)
                {

                    AddressItemCityResponse res = null;
                    Data.Provincias provincia = new Data.Provincias();
                    ProvinciasController cProvincias = new ProvinciasController();

                    provincia = cProvincias.GetItem(municipio.ProvinciaID);

                    //Agregamos los oDatos a enviar.
                    cityRequest = new AddressItemCityRequest();

                    if ((municipio.ZonaCrc != null && municipio.ZonaCrc != "")
                        || (municipio.CiudadCapital != null && municipio.CiudadCapital != "")
                        || (municipio.CiudadCapital != null && municipio.CiudadCapital != "")
                        || (municipio.MciCiudadPrincipal != null && municipio.MciCiudadPrincipal != "")
                        || (municipio.MciCiudadGrupo != null && municipio.MciCiudadGrupo != "")
                        || (municipio.ProyeccionPoblacion != null && municipio.ProyeccionPoblacion != "")
                        || (municipio.GrupoMpio != null && municipio.GrupoMpio != "")
                        || (municipio.Categorization != null && municipio.Categorization != "")
                        || (municipio.Ambito != null && municipio.Ambito != "")
                        || (municipio.ProcentajeCoberturaLTE != null && municipio.ProcentajeCoberturaLTE != "")
                        || (municipio.Mercado != null && municipio.Mercado != ""))
                    {
                        cityRequest.OPERATION = Comun.INTEGRACION_SERVICIO_COMARCH_MODIFY;
                    }
                    else
                    {

                        cityRequest.OPERATION = Comun.INTEGRACION_SERVICIO_COMARCH_CREATE;
                    }

                    cityRequest.ABBREVIATION = municipio.Codigo;
                    cityRequest.ABBREVIATION_REGION = provincia.Codigo;
                    cityRequest.CITY_NAME = municipio.Municipio;
                    cityRequest.MASTER_CRC_AREA = municipio.ZonaCrc;
                    cityRequest.MASTER_CAPITAL_CODE = municipio.CiudadCapital;
                    cityRequest.MASTER_MCI_MAIN = municipio.MciCiudadPrincipal;
                    cityRequest.MASTER_MCI_GROUP = municipio.MciCiudadGrupo;
                    cityRequest.POPULATION_PROJECTION = municipio.ProyeccionPoblacion;
                    cityRequest.MASTER_CITY_GROUP = municipio.GrupoMpio;
                    cityRequest.MASTER_CATEGORY_617 = municipio.Categorization;
                    cityRequest.MASTER_SCOPE = municipio.Ambito;
                    cityRequest.LTE_COVERAGE = municipio.ProcentajeCoberturaLTE;
                    cityRequest.MARKET = municipio.Mercado;
                    res = comarchConnect.ComarchAddressItemCity(conexionSegura, cityRequest, Comun.MODULOGLOBAL, "", Usuario.UsuarioID, null);
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

                            //Region already created. -- Departamento enviado ya se encuentra creado.
                            case "103":
                                tipoAviso = "WARNING";
                                warning = true;

                                break;

                            //Missing manoDatory attribute – operation
                            case "104":
                                tipoAviso = "WARNING";
                                warning = true;

                                break;

                            //Missing manoDatory attribute – abbreviation
                            case "105":
                                tipoAviso = "WARNING";
                                warning = true;

                                break;

                            //Missing manoDatory attribute – abbreviationregion
                            case "106":
                                tipoAviso = "WARNING";
                                warning = true;

                                break;

                            //Missing manoDatory attribute – masterregionname
                            case "107":
                                tipoAviso = "WARNING";
                                warning = true;

                                break;

                            //Missing attribute - name
                            case "108":
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



                if (cMunicipios.UpdateItem(municipio))
                {

                    storeDetalle.DataBind();

                }
                cMunicipios = null;


            }
            catch (Exception ex)
            {
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

            }
            storeDetalle.Reload();
            return direct;
        }




        #endregion

        #endregion

    }

}