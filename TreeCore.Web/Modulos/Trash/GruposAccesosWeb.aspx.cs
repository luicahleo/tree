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
using TreeCore.Data;

namespace TreeCore.ModGlobal
{
    public partial class GruposAccesosWeb : TreeCore.Page.BasePageExtNet
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
                Comun.CreateGridFilters(gridFiltersUsuarios, storeUsuarios, GridUsuarios.ColumnModel, listaIgnore, _Locale);
                Comun.CreateGridFilters(gridFiltersUsuariosLibres, storeUsuariosLibres, GridPanelUsuariosLibres.ColumnModel, listaIgnore, _Locale);
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
                        List<Data.GruposAccesosWeb> listaDatos = new List<Data.GruposAccesosWeb>();
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
            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_GRUPOS_ACCESOS_WEB))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;
                btnActivar.Hidden = true;
                btnDefecto.Hidden = true;
            }
            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_GRUPOS_ACCESOS_WEB))
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

        private List<Data.GruposAccesosWeb> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.GruposAccesosWeb> listaDatos;
            GruposAccesosWebController CGruposAccesosWeb = new GruposAccesosWebController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CGruposAccesosWeb.GetItemsWithExtNetFilterList<Data.GruposAccesosWeb>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        #region USUARIOS


        protected void storeUsuarios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sort, dir;
                    dir = e.Sort[0].Direction.ToString();
                    sort = e.Sort[0].Property.ToString();

                    int count = 0;
                    string filtro = e.Parameters["gridFilters"];

                    long idGrupo = 0;

                    if (GridRowSelect.SelectedRecordID != "")
                        idGrupo = Convert.ToInt32(GridRowSelect.SelectedRecordID);

                    var ls = ListaUsuarios(e.Start, e.Limit, sort, dir, ref count, filtro, idGrupo, long.Parse(hdCliID.Value.ToString()));
                    if (ls != null)
                    {
                        storeUsuarios.DataSource = ls;

                        PageProxy temp = (PageProxy)storeUsuarios.Proxy[0];
                        temp.Total = count;
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

        private List<Data.Vw_GruposAccesoWebUsuarios> ListaUsuarios(int start, int limit, string sort, string dir, ref int count, string filtro, long IdGrupo, long clienteID)
        {
            List<Data.Vw_GruposAccesoWebUsuarios> listaDatos = null;
            GruposAccesosWebUsuariosController cUsuarios = new GruposAccesosWebUsuariosController();
            try
            {
                listaDatos = cUsuarios.getUsuariosGrupoAccesoWeb(IdGrupo);
                listaDatos = Clases.LinqEngine.PagingItemsListWithExtNetFilter(listaDatos, filtro, "", sort, dir, start, limit, ref count);
            }

            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        #endregion

        #region USUARIOS

        protected void storeUsuariosLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sort, dir;
                    sort = e.Sort[0].Property.ToString();
                    dir = e.Sort[0].Direction.ToString();
                    int count = 0;
                    string filtro = e.Parameters["gridFilters"];

                    long idGrupo = 0;

                    if (GridRowSelect.SelectedRecordID != "")
                        idGrupo = Convert.ToInt32(GridRowSelect.SelectedRecordID);

                    var ls = ListaUsuariosLibres(e.Start, e.Limit, sort, dir, ref count, filtro, idGrupo, long.Parse(hdCliID.Value.ToString()));
                    if (ls != null)
                    {
                        storeUsuariosLibres.DataSource = ls;

                        PageProxy temp = (PageProxy)storeUsuariosLibres.Proxy[0];
                        temp.Total = count;
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

        private List<Data.Usuarios> ListaUsuariosLibres(int start, int limit, string sort, string dir, ref int count, string filtro, long IdGrupo, long clienteID)
        {
            List<Data.Usuarios> listadatos = null;
            GruposAccesosWebUsuariosController cGruposAccesosWebUsuarios = new GruposAccesosWebUsuariosController();

            try
            {

                listadatos = cGruposAccesosWebUsuarios.UsuariosAsignados(IdGrupo, clienteID);
                listadatos = Clases.LinqEngine.PagingItemsListWithExtNetFilter(listadatos, filtro, "", sort, dir, start, limit, ref count);

            }

            catch (Exception ex)
            {
                listadatos = null;
                log.Error(ex.Message);
            }
            return listadatos;
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)

        {
            DirectResponse direct = new DirectResponse();

            GruposAccesosWebController cController = new GruposAccesosWebController();
            long cliID = 0;
            try

            {
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.GruposAccesosWeb dato = cController.GetItem(S);


                    if (dato.GrupoAcceso == txtGrupoAcceso.Text)
                    {
                        dato.GrupoAcceso = txtGrupoAcceso.Text;
                        dato.URL = txtURL.Text;
                    }
                    else
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());
                        if (cController.RegistroDuplicado(txtGrupoAcceso.Text, cliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            dato.GrupoAcceso = txtGrupoAcceso.Text;
                            dato.URL = txtURL.Text;
                        }
                    }


                    if (cController.UpdateItem(dato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    cliID = long.Parse(hdCliID.Value.ToString());

                    if (cController.RegistroDuplicado(txtGrupoAcceso.Text, cliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.GruposAccesosWeb dato = new Data.GruposAccesosWeb();
                        dato.GrupoAcceso = txtGrupoAcceso.Text;
                        dato.URL = txtURL.Text;
                        dato.Activo = true;
                        dato.ClienteID = ClienteID.Value;

                        if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                        {
                            dato.ClienteID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            dato.ClienteID = cliID;
                        }
                        if (cController.AddItem(dato) != null)
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


            GruposAccesosWebController cGruposAccesosWeb = new GruposAccesosWebController();

            try

            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GruposAccesosWeb dato = cGruposAccesosWeb.GetItem(S);
                txtGrupoAcceso.Text = dato.GrupoAcceso;
                txtURL.Text = dato.URL;
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
            GruposAccesosWebController CGruposAccesosWeb = new GruposAccesosWebController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.GruposAccesosWeb oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = CGruposAccesosWeb.GetDefault(Convert.ToInt32(ClienteID));

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.ClienteID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            CGruposAccesosWeb.UpdateItem(oDato);
                        }

                        oDato = CGruposAccesosWeb.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        CGruposAccesosWeb.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = CGruposAccesosWeb.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    CGruposAccesosWeb.UpdateItem(oDato);
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
            GruposAccesosWebController CGruposAccesosWeb = new GruposAccesosWebController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CGruposAccesosWeb.DeleteItem(lID))
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
            GruposAccesosWebController cController = new GruposAccesosWebController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GruposAccesosWeb oDato = cController.GetItem(lID);
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

        #region StoreGruposAccesosWebUsuarios

        #endregion
        #region UsuariosGruposAccesos

        [DirectMethod()]
        public DirectResponse EliminarUsuario()
        {
            DirectResponse direct = new DirectResponse();
            GruposAccesosWebUsuariosController cGruposAccesosWeb = new GruposAccesosWebUsuariosController();
            GruposAccesosWebUsuarios reg;
            try
            {
                foreach (var item in RowSelectionModelUsuario.SelectedRows)
                {
                    reg = cGruposAccesosWeb.getRegistro(long.Parse(item.RecordID), long.Parse(GridRowSelect.SelectedRecordID));
                    if (cGruposAccesosWeb.DeleteItem(reg.GrupoAccesoWebUsuarioID))
                    {

                    }
                }
                storeUsuarios.DataBind();
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            cGruposAccesosWeb = null;
            direct.Success = true;
            direct.Result = "";
            return direct;
        }

        [DirectMethod()]
        public DirectResponse AgregarUsuario()
        {
            DirectResponse direct = new DirectResponse();
            GruposAccesosWebUsuariosController cController = new GruposAccesosWebUsuariosController();

            try
            {
                Data.GruposAccesosWebUsuarios dato;
                foreach (var item in RowSelectionModelUsuariosLibres.SelectedRows)
                {
                    dato = new Data.GruposAccesosWebUsuarios();
                    dato.GrupoAccesoWebID = Convert.ToInt32(GridRowSelect.SelectedRecordID);
                    dato.UsuarioID = Convert.ToInt32(item.RecordID);
                    if (cController.AddItem(dato) == null)
                    {
                        throw new Exception();
                    }
                }
                winUsuariosLibres.Hide();
                storeUsuarios.DataBind();
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
    }
}