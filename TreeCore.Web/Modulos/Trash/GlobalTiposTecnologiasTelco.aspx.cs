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
using TreeCore.Data;
using System.Data;

namespace TreeCore.ModGlobal
{
    public partial class GlobalTiposTecnologiasTelco : TreeCore.Page.BasePageExtNet
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

                List<string> listaAsignados = new List<string>()
                { };

                Comun.CreateGridFilters(gridFiltersAsignados, storeFrecuenciasAsignadas, GridPanelFrecuenciasAsignadas.ColumnModel, listaAsignados, _Locale);

                List<string> listaLibres = new List<string>()
                { };

                Comun.CreateGridFilters(gridFiltersLibres, storeFrecuenciasLibres, gridFrecuenciasLibres.ColumnModel, listaLibres, _Locale);

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
                        List<Data.GlobalTiposTecnologiasTelco> listaDatos = new List<Data.GlobalTiposTecnologiasTelco>();
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
            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_RESTRINGIDO_GLOBAL_TIPOS_TECNOLOGIAS_TELCO))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;
                btnActivar.Hidden = true;
                btnAsignarFrecuencias.Hidden = true;
            }
            else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_GLOBAL_TIPOS_TECNOLOGIAS_TELCO))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;
                btnActivar.Hidden = false;
                btnAsignarFrecuencias.Hidden = false;
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

        private List<Data.GlobalTiposTecnologiasTelco> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.GlobalTiposTecnologiasTelco> listaDatos;
            GlobalTiposTecnologiasTelcoController CGlobalTiposTecnologiasTelco = new GlobalTiposTecnologiasTelcoController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CGlobalTiposTecnologiasTelco.GetItemsWithExtNetFilterList<Data.GlobalTiposTecnologiasTelco>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        #region FRECUENCIAS

        protected void storeFrecuenciasLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                GlobalTiposTecnologiasTelcoController cFrecuencias = new GlobalTiposTecnologiasTelcoController();
                try
                {
                    List<Data.GlobalFrecuenciasTelco> lista = cFrecuencias.frecuenciasNoAsignadas(Int64.Parse(GridRowSelect.SelectedRecordID));

                    if (lista != null)
                    {
                        storeFrecuenciasLibres.DataSource = lista;
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

        protected void storeFrecuenciasAsignadas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                GlobalTiposTecnologiasTelcoController cFrecuencias = new GlobalTiposTecnologiasTelcoController();
                try
                {
                    List<Data.GlobalFrecuenciasTelco> lista;
                    lista = cFrecuencias.frecuenciasAsignadas(Int64.Parse(GridRowSelect.SelectedRecordID));

                    if (lista != null)
                        storeFrecuenciasAsignadas.DataSource = lista;

                    cFrecuencias = null;
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

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)

        {
            DirectResponse direct = new DirectResponse();

            GlobalTiposTecnologiasTelcoController cGlobalTiposTecnologiasTelco = new GlobalTiposTecnologiasTelcoController();

            try
            {
                long IdCl = long.Parse(hdCliID.Value.ToString());
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.GlobalTiposTecnologiasTelco dato = cGlobalTiposTecnologiasTelco.GetItem(S);
                    if (dato.NombreTipoTecnologia == txtNombre.Text)
                    {
                        dato.NombreTipoTecnologia = txtNombre.Text;
                    }
                    else
                    {
                        if (cGlobalTiposTecnologiasTelco.RegistroDuplicado(txtNombre.Text, IdCl))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }

                        else
                        {
                            dato.NombreTipoTecnologia = txtNombre.Text;
                        }
                    }

                    if (cGlobalTiposTecnologiasTelco.UpdateItem(dato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }

                }
                else
                {
                    if (cGlobalTiposTecnologiasTelco.RegistroDuplicado(txtNombre.Text, IdCl))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.GlobalTiposTecnologiasTelco dato = new Data.GlobalTiposTecnologiasTelco();
                        dato.NombreTipoTecnologia = txtNombre.Text;
                        dato.Activo = true;
                        dato.ClienteID = IdCl;

                        if (cGlobalTiposTecnologiasTelco.AddItem(dato) != null)
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

            GlobalTiposTecnologiasTelcoController cGlobalTiposTecnologiasTelco = new GlobalTiposTecnologiasTelcoController();

            try

            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GlobalTiposTecnologiasTelco dato = cGlobalTiposTecnologiasTelco.GetItem(S);
                txtNombre.Text = dato.NombreTipoTecnologia;
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
            GlobalTiposTecnologiasTelcoController cGlobalTiposTecnologiasTelco = new GlobalTiposTecnologiasTelcoController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.GlobalTiposTecnologiasTelco oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cGlobalTiposTecnologiasTelco.GetDefault(long.Parse(hdCliID.Value.ToString()));

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.ClienteID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cGlobalTiposTecnologiasTelco.UpdateItem(oDato);
                        }

                        oDato = cGlobalTiposTecnologiasTelco.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        cGlobalTiposTecnologiasTelco.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cGlobalTiposTecnologiasTelco.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cGlobalTiposTecnologiasTelco.UpdateItem(oDato);
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
            GlobalTiposTecnologiasTelcoController CGlobalTiposTecnologiasTelco = new GlobalTiposTecnologiasTelcoController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CGlobalTiposTecnologiasTelco.DeleteItem(lID))
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
            GlobalTiposTecnologiasTelcoController cController = new GlobalTiposTecnologiasTelcoController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GlobalTiposTecnologiasTelco oDato;
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

        [DirectMethod()]
        public DirectResponse AgregarFrecuenciaATipoTecTelco()
        {
            DirectResponse direct = new DirectResponse();
            GlobalTiposTecnologiasTelcoFrecuenciasTelcoController cGlobalTiposTecnologiasTelcoFrecuenciasTelco = new GlobalTiposTecnologiasTelcoFrecuenciasTelcoController();

            try
            {
                foreach (SelectedRow selec in GridRowSelectFrecuenciasLibresRowSelection.SelectedRows)
                {
                    Data.GlobalTiposTecnologiasTelcoFrecuenciasTelco dato = new Data.GlobalTiposTecnologiasTelcoFrecuenciasTelco();
                    dato.GlobalTipoTecnologiaTelcoID = Int64.Parse(GridRowSelect.SelectedRecordID);
                    dato.GlobalFrecuenciaTelcoID = Int64.Parse(selec.RecordID);

                    cGlobalTiposTecnologiasTelcoFrecuenciasTelco.AddItem(dato);
                }

                storeFrecuenciasAsignadas.DataBind();
                storeFrecuenciasLibres.DataBind();
                direct.Success = true;
                direct.Result = "";
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
        public DirectResponse EliminarFrecuenciaATipoTecTelco()
        {
            DirectResponse direct = new DirectResponse();
            GlobalTiposTecnologiasTelcoFrecuenciasTelcoController cGlobalTiposTecnologiasTelcoFrecuenciasTelco = new GlobalTiposTecnologiasTelcoFrecuenciasTelcoController();
            //GridRowSelectFrecuenciasAsignadasRowSelection
            try
            {
                foreach (SelectedRow selec in GridRowSelectFrecuenciasAsignadasRowSelection.SelectedRows)
                {
                    cGlobalTiposTecnologiasTelcoFrecuenciasTelco.EliminarByTipoTecnologiaTelcoYFrecuencia(Int64.Parse(GridRowSelect.SelectedRecordID), Int64.Parse(selec.RecordID));
                }
                direct.Success = true;
                direct.Result = "";
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