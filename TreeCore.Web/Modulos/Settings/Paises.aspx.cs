using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data.SqlClient;
using System.Linq;

namespace TreeCore.ModGlobal
{
    public partial class Paises : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();
        public List<object> ListaIconos;

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
                        List<Data.Vw_Paises> listaDatos;
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
                        lista.ForEach(p => {
                            try
                            {
                                Icon icono = (Icon)Enum.Parse(typeof(Icon), p.Icono);
                                p.Icono = ResourceManager.GetIconClassName(icono);
                                ResourceManagerTreeCore.RegisterIcon(icono);
                            }
                            catch(Exception ex)
                            {

                            }
                        });

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

        private List<Data.Vw_Paises> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.Vw_Paises> listaDatos;
            PaisesController cPaises = new PaisesController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = cPaises.GetItemsWithExtNetFilterList<Data.Vw_Paises>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
                }
                else
                {
                    listaDatos = null;
                }

                //Filtro resultados KPI
                if (listaDatos != null && listIdsResultadosKPI != null)
                {
                    listaDatos = cPaises.FiltroListaPrincipalByIDs(listaDatos.Cast<object>().ToList(), listIdsResultadosKPI, nameIndiceID).Cast<Data.Vw_Paises>().ToList();
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

        #region STORE REGIONES


        protected void storeRegiones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {

            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sort, dir;
                    //Recupera los parámetros para obtener los datos de la grilla
                    sort = e.Sort.ToString();
                    dir = e.Sort.ToString();
                    int count = 0;
                    string filtro = e.Parameters["gridFilters"];

                    //Recupera los datos y los establece
                    storeRegiones.DataSource = ListaRegiones(e.Start, e.Limit, sort, dir, ref count, filtro);

                    PageProxy temp;
                    temp = (PageProxy)storeRegiones.Proxy[0];
                    temp.Total = count;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    MensajeErrorGenerico(ex);
                }
            }
        }

        private List<Data.Regiones> ListaRegiones(int start, int limit, string sort, string dir, ref int count, string filtro)
        {
            List<Data.Regiones> datos = null;

            try
            {
                RegionesController mRegiones = new RegionesController();

                datos = mRegiones.GetActivos();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return datos;
        }

        private void RefreshStoreRegiones()
        {
            storeRegiones.DataBind();
        }

        #endregion

        #region ICONOS

        protected void storeIconos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {

            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    if (ListaIconos == null || ListaIconos.Count == 0)
                    {
                        Icon icono;
                        List<string> icons = Enum.GetNames(typeof(Icon)).ToList<string>();
                        icons.Remove("None");
                        List<object> internalImages = new List<object>(icons.Count);
                        List<string> banderas = (from c in icons where c.StartsWith("Flag") select c).ToList();
                        foreach (var item in banderas)
                        {
                            icono = (Icon)Enum.Parse(typeof(Icon), item);
                            internalImages.Add(
                            new
                            {
                                ImagenID = icono,
                                Imagen = /*ResourceManager.GetIconClassName(icono)*/ icono.ToString(),
                                IconCls = ResourceManager.GetIconClassName(icono)
                            });
                            ResourceManagerTreeCore.RegisterIcon(icono);
                        }

                        storeIconos.DataSource = internalImages;
                        storeIconos.DataBind();
                        ListaIconos = internalImages;
                    }
                    else
                    {
                        storeIconos.DataSource = ListaIconos;
                        storeIconos.DataBind();
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
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();

            PaisesController cControl = new PaisesController();

            long cliID = long.Parse(hdCliID.Value.ToString());

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.Paises oDato;
                    oDato = cControl.GetItem(lS);

                    if (oDato.Pais == txtNombre.Text && oDato.PaisCod == txtCodigo.Text)
                    {
                        oDato.Pais = txtNombre.Text;
                        oDato.PaisCod = txtCodigo.Text;
                        oDato.RegionID = long.Parse(cmbRegion.SelectedItem.Value.ToString());
                        oDato.Pais_En = "";
                        oDato.Pais_Fr = "";
                        oDato.Prefijo = txtPrefijo.Text;
                        oDato.Icono = cmbIconos.Value.ToString();


                        if (numbLatitud.Value != null)
                        {
                            oDato.Latitud = double.Parse(numbLatitud.Value.ToString());
                        }
                        else
                        {
                            oDato.Latitud = null;
                        }

                        if (numbLongitud.Value != null)
                        {
                            oDato.Longitud = double.Parse(numbLongitud.Value.ToString());
                        }
                        else
                        {
                            oDato.Longitud = null;
                        }

                        if (numbZoom.Value != null)
                        {
                            oDato.Zoom = Convert.ToInt32(numbZoom.Value.ToString());
                        }
                        else
                        {
                            oDato.Zoom = null;
                        }
                    }
                    else
                    {
                        if (cControl.RegistroDuplicado(txtNombre.Text, txtCodigo.Text, cliID, lS))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            if (cmbRegion.SelectedItem.Value != null)
                            {
                                oDato.RegionID = cControl.GetregionID_Activo(cmbRegion.SelectedItem.Text);
                            }

                            oDato.Pais = txtNombre.Text;
                            oDato.PaisCod = txtCodigo.Text;
                            oDato.RegionID = long.Parse(cmbRegion.SelectedItem.Value.ToString());
                            oDato.Pais_En = "";
                            oDato.Pais_Fr = "";
                            oDato.Prefijo = txtPrefijo.Text;
                            oDato.Icono = cmbIconos.Value.ToString();

                            if (numbLatitud.Value != null)
                            {
                                oDato.Latitud = double.Parse(numbLatitud.Value.ToString());
                            }
                            else
                            {
                                oDato.Latitud = null;
                            }

                            if (numbLongitud.Value != null)
                            {
                                oDato.Longitud = double.Parse(numbLongitud.Value.ToString());
                            }
                            else
                            {
                                oDato.Longitud = null;
                            }

                            if (numbZoom.Value != null)
                            {
                                oDato.Zoom = Convert.ToInt32(numbZoom.Value.ToString());
                            }
                            else
                            {
                                oDato.Zoom = null;
                            }
                        }
                    }
                    if (cControl.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    if (cControl.RegistroDuplicado(txtNombre.Text, txtCodigo.Text, cliID, 0))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.Paises dato = new Data.Paises();
                        dato.Pais = txtNombre.Text;
                        dato.PaisCod = txtCodigo.Text;
                        dato.Pais_En = "";
                        dato.Pais_Fr = "";
                        dato.Prefijo = txtPrefijo.Text;
                        dato.Icono = cmbIconos.Value.ToString();

                        if (numbLatitud.Value != null)
                        {
                            dato.Latitud = double.Parse(numbLatitud.Value.ToString());
                        }
                        else
                        {
                            dato.Latitud = null;
                        }

                        if (numbLongitud.Value != null)
                        {
                            dato.Longitud = double.Parse(numbLongitud.Value.ToString());
                        }
                        else
                        {
                            dato.Longitud = null;
                        }

                        if (numbZoom.Value != null)
                        {
                            dato.Zoom = Convert.ToInt32(numbZoom.Value.ToString());
                        }
                        else
                        {
                            dato.Zoom = null;
                        }

                        if (cmbRegion.SelectedItem.Value != null)
                        {
                            dato.RegionID = cControl.GetregionID_Activo(cmbRegion.SelectedItem.Text);
                        }

                        dato.Activo = true;
                        dato.ClienteID = cliID;

                        if (cControl.AddItem(dato) != null)
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

            PaisesController cControl = new PaisesController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.Paises dato;
                dato = cControl.GetItem<Data.Paises>(lS);

                if (dato.RegionID != 0)
                {
                    cmbRegion.SetValue(dato.RegionID);
                }
                txtNombre.Text = dato.Pais;
                txtCodigo.Text = dato.PaisCod;
                numbLatitud.Value = dato.Latitud;
                numbLongitud.Value = dato.Longitud;
                numbZoom.Value = dato.Zoom;
                txtPrefijo.Text = dato.Prefijo;
                cmbIconos.SetValue(dato.Icono);
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
            PaisesController CPaises = new PaisesController();
            RegionesPaisesController cRegiones = new RegionesPaisesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.Paises oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = CPaises.GetDefault(long.Parse(hdCliID.Value.ToString()));

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.PaisID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cRegiones.EliminarDefecto(oDato.PaisID);
                            CPaises.UpdateItem(oDato);
                        }

                        oDato = CPaises.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        CPaises.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = CPaises.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    CPaises.UpdateItem(oDato);
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
            PaisesController CPaises = new PaisesController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CPaises.RegistroDefecto(lID))
                {
                    direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                    direct.Success = false;
                }
                else if (CPaises.DeleteItem(lID))
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
            PaisesController cController = new PaisesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.Paises oDato;
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion

        #region FUNCTIONS
        #endregion

    }
}