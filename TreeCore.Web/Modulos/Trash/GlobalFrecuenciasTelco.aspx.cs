using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace TreeCore.ModGlobal
{
    public partial class GlobalFrecuenciasTelco : TreeCore.Page.BasePageExtNet
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
                        List<Data.GlobalFrecuenciasTelco> listaDatos = new List<Data.GlobalFrecuenciasTelco>();
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
            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_RESTRINGIDO_GLOBAL_FRECUENCIAS_TELCO))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;
                btnActivar.Hidden = true;
            }
            else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_GLOBAL_FRECUENCIAS_TELCO))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;
                btnActivar.Hidden = false;
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

                     

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro,  long.Parse(hdCliID.Value.ToString()));

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

        private List<Data.GlobalFrecuenciasTelco> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.GlobalFrecuenciasTelco> listaDatos = new List<Data.GlobalFrecuenciasTelco>();
            GlobalFrecuenciasTelcoController CGlobalFrecuenciasTelco = new GlobalFrecuenciasTelcoController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CGlobalFrecuenciasTelco.GetItemsWithExtNetFilterList<Data.GlobalFrecuenciasTelco>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)

        {
            DirectResponse direct = new DirectResponse();

            GlobalFrecuenciasTelcoController cGlobalFrecuenciasTelco = new GlobalFrecuenciasTelcoController();

            try
            {
                long IdCl = long.Parse(hdCliID.Value.ToString());
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.GlobalFrecuenciasTelco oDato = cGlobalFrecuenciasTelco.GetItem(S);
                    if (oDato.NombreFrecuenciaTelco == txtNombre.Text)
                    {
                        oDato.NombreFrecuenciaTelco = txtNombre.Text;
                    }
                    else
                    {
                        if (cGlobalFrecuenciasTelco.RegistroDuplicado(txtNombre.Text, IdCl))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }

                        else
                        {
                            oDato.NombreFrecuenciaTelco = txtNombre.Text;
                        }
                    }

                    if (cGlobalFrecuenciasTelco.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }

                }
                else
                {
                    if (cGlobalFrecuenciasTelco.RegistroDuplicado(txtNombre.Text, IdCl))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.GlobalFrecuenciasTelco oDato = new Data.GlobalFrecuenciasTelco();
                        oDato.NombreFrecuenciaTelco = txtNombre.Text;
                        oDato.Activo = true;
                        oDato.ClienteID = IdCl;

                        if (cGlobalFrecuenciasTelco.AddItem(oDato) != null)
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

            GlobalFrecuenciasTelcoController cGlobalFrecuenciasTelcoController = new GlobalFrecuenciasTelcoController();

            try

            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GlobalFrecuenciasTelco oDato = cGlobalFrecuenciasTelcoController.GetItem(S);
                txtNombre.Text = oDato.NombreFrecuenciaTelco;
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
            GlobalFrecuenciasTelcoController cGlobalFrecuenciasTelcoController = new GlobalFrecuenciasTelcoController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.GlobalFrecuenciasTelco oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cGlobalFrecuenciasTelcoController.GetDefault(long.Parse(hdCliID.Value.ToString()));

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.ClienteID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cGlobalFrecuenciasTelcoController.UpdateItem(oDato);
                        }

                        oDato = cGlobalFrecuenciasTelcoController.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        cGlobalFrecuenciasTelcoController.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cGlobalFrecuenciasTelcoController.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cGlobalFrecuenciasTelcoController.UpdateItem(oDato);
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
            GlobalFrecuenciasTelcoController CGlobalFrecuenciasTelco = new GlobalFrecuenciasTelcoController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CGlobalFrecuenciasTelco.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }

                CGlobalFrecuenciasTelco = null;
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
            GlobalFrecuenciasTelcoController cController = new GlobalFrecuenciasTelcoController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GlobalFrecuenciasTelco oDato = cController.GetItem(lID);
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


    }
}