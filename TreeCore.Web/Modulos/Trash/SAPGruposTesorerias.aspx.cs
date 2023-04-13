using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SqlClient;
using TreeCore.Clases;


namespace TreeCore.ModGlobal
{
    public partial class SAPGruposTesorerias : TreeCore.Page.BasePageExtNet
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
                        List<Data.SAPGruposTesorerias> listaDatos;
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

        private List<Data.SAPGruposTesorerias> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.SAPGruposTesorerias> listaDatos;
            SAPGruposTesoreriasController CSAPGruposTesorerias = new SAPGruposTesoreriasController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CSAPGruposTesorerias.GetItemsWithExtNetFilterList<Data.SAPGruposTesorerias>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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
            SAPGruposTesoreriasController cSAP = new SAPGruposTesoreriasController();
            long lCliID = 0;
            InfoResponse oResponse;
            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.SAPGruposTesorerias oDato;
                    oDato = cSAP.GetItem(lS);

                    
                        oDato.SAPGrupoTesoreria = txtSAPGrupoTesoreria.Text;
                        oDato.Descripcion = txtDescripcion.Text;
                    
                        lCliID = long.Parse(hdCliID.Value.ToString());

                 
                    
                    oResponse = cSAP.Update(oDato);
                    if (oResponse.Result)
                    {
                        oResponse = cSAP.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        string error = oResponse.Description;
                        oResponse = cSAP.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(error);
                        return direct;
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    Data.SAPGruposTesorerias oDato = new Data.SAPGruposTesorerias();

                    oDato.SAPGrupoTesoreria = txtSAPGrupoTesoreria.Text;
                    oDato.Descripcion = txtDescripcion.Text;
                    oDato.Activo = true;

                    oDato.ClienteID = lCliID;
                   
                        oResponse = cSAP.Add(oDato);
                    if (oResponse.Result)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        storePrincipal.DataBind();
                        oResponse = cSAP.SubmitChanges();
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        string error = oResponse.Description;
                        oResponse = cSAP.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(error);
                        return direct;
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
            SAPGruposTesoreriasController cSAP = new SAPGruposTesoreriasController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.SAPGruposTesorerias oDato;
                oDato = cSAP.GetItem(lS);

                txtSAPGrupoTesoreria.Text = oDato.SAPGrupoTesoreria;
                txtDescripcion.Text = oDato.Descripcion;

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
            SAPGruposTesoreriasController cSAPGruposTesorerias = new SAPGruposTesoreriasController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.SAPGruposTesorerias oDato;
                InfoResponse oResponse;
                long lCliID = 0;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cSAPGruposTesorerias.GetItem(lID);
                oResponse = cSAPGruposTesorerias.SetDefecto(oDato);
                if (oResponse.Result)
                {
                    oResponse = cSAPGruposTesorerias.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    oResponse = cSAPGruposTesorerias.DiscardChanges();
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            SAPGruposTesoreriasController cSAPGruposTesorerias = new SAPGruposTesoreriasController();
            InfoResponse oResponse;
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {

                Data.SAPGruposTesorerias oDato = cSAPGruposTesorerias.GetItem(lID);
                oResponse = cSAPGruposTesorerias.Delete(oDato);
                if (oResponse.Result)
                {
                    oResponse = cSAPGruposTesorerias.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        return direct;
                    }
                }
                else
                {
                    string error = oResponse.Description;
                    oResponse = cSAPGruposTesorerias.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(error);
                    return direct;

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
            SAPGruposTesoreriasController cController = new SAPGruposTesoreriasController();
            InfoResponse oResponse;
            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.SAPGruposTesorerias oDato = cController.GetItem(lID);
                oDato.Activo = !oDato.Activo;
                oResponse = cController.Update(oDato);
                if (oResponse.Result)
                {
                    oResponse = cController.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    oResponse = cController.DiscardChanges();
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion


        #region FUNCTIONS

        #endregion
    }
}