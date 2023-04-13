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


namespace TreeCore.ModInventario
{
    public partial class InventarioElementosAtributosEstados : TreeCore.Page.BasePageExtNet
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
                        List<Data.InventarioElementosAtributosEstados> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long lCliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, lCliID);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                            log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            EstadisticasController cEstadisticas = new EstadisticasController();
                            cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.INVENTARIO), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
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
                }
            }
        }

        private List<Data.InventarioElementosAtributosEstados> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.InventarioElementosAtributosEstados> listaDatos;
            InventarioElementosAtributosEstadosController cInventarioElementosAtributosEstados = new InventarioElementosAtributosEstadosController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = cInventarioElementosAtributosEstados.GetItemsWithExtNetFilterList<Data.InventarioElementosAtributosEstados>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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
                        storePrincipal.DataBind();
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
            InventarioElementosAtributosEstadosController cInventarioElementosAtributosEstados = new InventarioElementosAtributosEstadosController();

            try
            {
                long lCliID = long.Parse(hdCliID.Value.ToString());

                if (!bAgregar)
                {
                    long lS = Int64.Parse(GridRowSelect.SelectedRecordID);


                    Data.InventarioElementosAtributosEstados oDato;
                    List<Data.InventarioElementosAtributosEstados> listaDatos = cInventarioElementosAtributosEstados.GetItemList();
                    oDato = cInventarioElementosAtributosEstados.GetItem(lS);

                    if (oDato.Nombre == txtNombre.Text && oDato.Codigo == txtCodigo.Text)
                    {
                        if (ClienteID.HasValue)
                        {
                            lCliID = long.Parse(hdCliID.Value.ToString());
                        }

                        foreach (var i in listaDatos)
                        {
                            if (txtCodigo.Text == i.Codigo & oDato.InventarioElementoAtributoEstadoID != i.InventarioElementoAtributoEstadoID)
                            {
                                MensajeBox(GetGlobalResource(Comun.jsControlDuplicidad), GetGlobalResource(Comun.jsCodigoExiste), MessageBox.Icon.INFO, null);
                                direct.Success = false;
                                return direct;
                            }
                            else
                            {
                                oDato.Codigo = txtCodigo.Text;
                            }
                        }

                        if (cInventarioElementosAtributosEstados.UpdateItem(oDato))
                        {
                            log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();
                        }
                    }
                    else
                    {
                        if (cInventarioElementosAtributosEstados.RegistroDuplicado(txtNombre.Text, txtCodigo.Text, lCliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.Nombre = txtNombre.Text;
                            if (ClienteID.HasValue)
                            {
                                lCliID = long.Parse(hdCliID.Value.ToString());
                            }

                            foreach (var i in listaDatos)
                            {
                                if (txtCodigo.Text == i.Codigo & oDato.InventarioElementoAtributoEstadoID != i.InventarioElementoAtributoEstadoID)
                                {
                                    MensajeBox(GetGlobalResource(Comun.jsControlDuplicidad), GetGlobalResource(Comun.jsCodigoExiste), MessageBox.Icon.INFO, null);
                                    direct.Success = false;
                                    return direct;
                                }
                                else
                                {
                                    oDato.Codigo = txtCodigo.Text;
                                }
                            }

                            if (cInventarioElementosAtributosEstados.UpdateItem(oDato))
                            {
                                log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                                storePrincipal.DataBind();
                            }
                        }
                    }

                }
                else
                {
                    if (cInventarioElementosAtributosEstados.RegistroDuplicado(txtNombre.Text, txtCodigo.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.InventarioElementosAtributosEstados oDato = new Data.InventarioElementosAtributosEstados();
                        oDato.Nombre = txtNombre.Text;
                        oDato.Codigo = txtCodigo.Text;
                        oDato.ClienteID = lCliID;


                        oDato.Activo = true;
                        oDato = cInventarioElementosAtributosEstados.AddItem(oDato);

                        if (oDato != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();
                        }
                        storePrincipal.DataBind();
                        cInventarioElementosAtributosEstados = null;
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
            InventarioElementosAtributosEstadosController cInventarioElementosAtributosEstados = new InventarioElementosAtributosEstadosController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);
                Data.InventarioElementosAtributosEstados oDato = cInventarioElementosAtributosEstados.GetItem(lS);

                txtNombre.Text = oDato.Nombre;
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
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            InventarioElementosAtributosEstadosController cInventarioElementosAtributosEstados = new InventarioElementosAtributosEstadosController();
            long lCliID = 0;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.InventarioElementosAtributosEstados oDato;

                if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                {
                    lCliID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                }
                else if (ClienteID.HasValue)
                {
                    lCliID = Convert.ToInt32(ClienteID);
                }

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cInventarioElementosAtributosEstados.GetDefault(lCliID);

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.InventarioElementoAtributoEstadoID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cInventarioElementosAtributosEstados.UpdateItem(oDato);
                        }
                    }

                    oDato = cInventarioElementosAtributosEstados.GetItem(lID);
                    oDato.Defecto = !oDato.Defecto;
                    oDato.Activo = true;
                    cInventarioElementosAtributosEstados.UpdateItem(oDato);
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cInventarioElementosAtributosEstados.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cInventarioElementosAtributosEstados.UpdateItem(oDato);
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
            InventarioElementosAtributosEstadosController cInventarioElementosAtributosEstados = new InventarioElementosAtributosEstadosController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cInventarioElementosAtributosEstados.RegistroDefecto(lID))
                {
                    direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                    direct.Success = false;
                }
                else if (cInventarioElementosAtributosEstados.DeleteItem(lID))
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
            InventarioElementosAtributosEstadosController cController = new InventarioElementosAtributosEstadosController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.InventarioElementosAtributosEstados oDato = cController.GetItem(lID);

                if (!oDato.Defecto)
                {
                    oDato.Activo = !oDato.Activo;

                    if (cController.UpdateItem(oDato))
                    {
                        storePrincipal.DataBind();
                        log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                    }
                }
                else
                {
                    direct.Result = GetGlobalResource(Comun.jsDesactivarPorDefecto);
                    direct.Success = false;
                    return direct;
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