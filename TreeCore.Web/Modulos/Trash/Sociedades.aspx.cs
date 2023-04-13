using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace TreeCore.ModGlobal
{
    public partial class Sociedades : TreeCore.Page.BasePageExtNet
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
                        List<Data.Vw_Sociedades> listaDatos;
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
            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_SOCIEDADES))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;
                btnActivar.Hidden = true;
                btnDefecto.Hidden = true;
            }
            else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_SOCIEDADES))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;
                btnActivar.Hidden = false;
                btnDefecto.Hidden = false;
            }
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

        private List<Data.Vw_Sociedades> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.Vw_Sociedades> listaDatos;
            SociedadesController CSociedades = new SociedadesController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CSociedades.GetItemsWithExtNetFilterList<Data.Vw_Sociedades>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        #region MONEDAS

        protected void storeMonedas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                MonedasController cMonedas = new MonedasController();

                try
                {
                    var ls = cMonedas.GetAllActivos();
                    if (ls != null)
                        storeMonedas.DataSource = ls;

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
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            SociedadesController cSociedades = new SociedadesController();
            RegionesPaisesController cRegionPais = new RegionesPaisesController();
            ProvinciasController cProvincia = new ProvinciasController();
            MunicipiosController cMunicipios = new MunicipiosController();
            long lCliID = 0;

            try
            {
                if (!bAgregar) //update
                {
                    long lIDSelect = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.Sociedades oDato;

                    oDato = cSociedades.GetItem(lIDSelect);

                    if (oDato.Sociedad == txtSociedad.Text)
                    {
                        oDato.Sociedad = txtSociedad.Text;
                        oDato.NIF = txtNIF.Text;
                        oDato.Direccion = txtDireccion.Text;
                        oDato.CodigoPostal = txtCodigoPostal.Text;
                        oDato.Municipio = cMunicipios.GetMunicipioByID((long)locSociedades.MunicipioID);
                        oDato.Provincia = cProvincia.GetNameProvinciaActivaByID((long)locSociedades.ProvinciaID);
                        oDato.PaisID = locSociedades.PaisID;
                        oDato.Region = cRegionPais.GetRegionByID((long)locSociedades.RegionPaisID);
                        oDato.CodigoSociedad = txtCodigoSociedad.Text;
                        oDato.SociedadCECO = chkSocidedadCECO.Checked;

                        if (cmbMoneda.SelectedItem.Value != null && cmbMoneda.SelectedItem.Value != "")
                        {
                            oDato.MonedaID = long.Parse(cmbMoneda.SelectedItem.Value.ToString());
                        }

                        if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                        {
                            oDato.ClienteID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.ClienteID = long.Parse(hdCliID.Value.ToString());
                        }
                    }
                    else
                    {
                        lCliID = long.Parse(hdCliID.Value.ToString());

                        if (cSociedades.RegistroDuplicado(txtSociedad.Text, lCliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato = new Data.Sociedades
                            {
                                Sociedad = txtSociedad.Text,
                                NIF = txtNIF.Text,
                                Direccion = txtDireccion.Text,
                                CodigoPostal = txtCodigoPostal.Text,
                                Municipio = cMunicipios.GetMunicipioByID((long)locSociedades.MunicipioID),
                                Provincia = cProvincia.GetNameProvinciaActivaByID((long)locSociedades.ProvinciaID),
                                PaisID = locSociedades.PaisID,
                                Region = cRegionPais.GetRegionByID((long)locSociedades.RegionPaisID),
                                CodigoSociedad = txtCodigoSociedad.Text,
                                SociedadCECO = chkSocidedadCECO.Checked
                            };

                            if (cmbMoneda.SelectedItem.Value != null && cmbMoneda.SelectedItem.Value != "")
                            {
                                oDato.MonedaID = long.Parse(cmbMoneda.SelectedItem.Value.ToString());
                            }

                            if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                            {
                                oDato.ClienteID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                            }
                            else
                            {
                                oDato.ClienteID = lCliID;
                            }
                        }
                    }
                    if (cSociedades.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                { //insert
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    if (cSociedades.RegistroDuplicado(txtSociedad.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.Sociedades oDato = new Data.Sociedades
                        {
                            Sociedad = txtSociedad.Text,
                            NIF = txtNIF.Text,
                            Direccion = txtDireccion.Text,
                            CodigoPostal = txtCodigoPostal.Text,
                            Municipio = cMunicipios.GetMunicipioByID((long)locSociedades.MunicipioID),
                            Provincia = cProvincia.GetNameProvinciaActivaByID((long)locSociedades.ProvinciaID),
                            PaisID = locSociedades.PaisID,
                            Region = cRegionPais.GetRegionByID((long)locSociedades.RegionPaisID),
                            CodigoSociedad = txtCodigoSociedad.Text,
                            Activa = true,
                            SociedadCECO = chkSocidedadCECO.Checked
                        };

                        if (cmbMoneda.SelectedItem.Value != null)
                        {
                            oDato.MonedaID = long.Parse(cmbMoneda.SelectedItem.Value.ToString());
                        }

                        if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                        {
                            oDato.ClienteID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.ClienteID = lCliID;
                        }

                        if (cSociedades.AddItem(oDato) != null)
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
            SociedadesController cSociedades = new SociedadesController();
            ProvinciasController cProvincias = new ProvinciasController();
            MunicipiosController cMunicipios = new MunicipiosController();
            RegionesPaisesController cRegionPais = new RegionesPaisesController();

            try
            {
                long lIDSelect = Convert.ToInt64(GridRowSelect.SelectedRecordID);

                Data.Sociedades oDato = cSociedades.GetItem(lIDSelect);

                txtSociedad.Text = oDato.Sociedad;
                txtNIF.Text = oDato.NIF;
                txtDireccion.Text = oDato.Direccion;
                txtCodigoPostal.Text = oDato.CodigoPostal;

                cmbClientes.SetValue(oDato.ClienteID);
                txtCodigoSociedad.Text = oDato.CodigoSociedad;
                cmbMoneda.SetValue(oDato.MonedaID);

                if (oDato.SociedadCECO.HasValue)
                {
                    chkSocidedadCECO.Checked = Convert.ToBoolean(oDato.SociedadCECO);
                }
                else
                {
                    chkSocidedadCECO.Checked = false;
                }
                locSociedades.PaisID = oDato.PaisID;
                locSociedades.ProvinciaID = cProvincias.GetprovinciaByNombre(oDato.Provincia);
                locSociedades.MunicipioID = cMunicipios.GetMunicipioByNombre(oDato.Municipio);
                locSociedades.RegionPaisID = cRegionPais.GetByNombre(oDato.Region).RegionPaisID;
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
            SociedadesController cSociedades = new SociedadesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.Sociedades oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cSociedades.GetDefault(Convert.ToInt32(ClienteID));

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.SociedadID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cSociedades.UpdateItem(oDato);
                        }

                        oDato = cSociedades.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activa = true;
                        cSociedades.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cSociedades.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activa = true;
                    cSociedades.UpdateItem(oDato);
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
            SociedadesController cSociedades = new SociedadesController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cSociedades.RegistroDefecto(lID))
                {
                    direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                    direct.Success = false;
                }
                else if (cSociedades.DeleteItem(lID))
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
            SociedadesController cController = new SociedadesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.Sociedades oDato;
                oDato = cController.GetItem(lID);
                oDato.Activa = !oDato.Activa;

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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion

        #region FUNCTIONS
        #endregion

    }
}