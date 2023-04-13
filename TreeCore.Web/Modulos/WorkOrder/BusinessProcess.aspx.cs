using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using System.Data.SqlClient;
using log4net;
using System.Reflection;
using System.Transactions;
using Newtonsoft.Json;
using System.Linq;
using TreeCore.Clases;

namespace TreeCore.ModWorkFlow.pages
{
    public partial class BusinessProcess : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                //#region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                //#endregion

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_RESTRINGIDO_GLOBAL_BANCOS)) { }
        }

        //[DirectMethod()]
        //public DirectResponse VaciarCombo()
        //{
        //    DirectResponse direct = new DirectResponse();

        //    try
        //    {
        //        cmbWorkflow.Items.Clear();
        //    }
        //    catch (Exception ex)
        //    {
        //        direct.Success = false;
        //        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
        //        log.Error(ex.Message);
        //        return direct;
        //    }

        //    direct.Success = true;
        //    direct.Result = "";

        //    return direct;
        //}

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            long RegID = long.Parse(GridRowSelectBusinessProcess.SelectedRecordID);
            BusinessProcessController cBusiness = new BusinessProcessController();
            CoreWorkflowsController cWorkFlow = new CoreWorkflowsController();
            try
            {
                var oDato = cBusiness.GetItem(RegID);
                string idWorkflow = cWorkFlow.getWorkFlowByEstadoID(oDato.EstadoInicialID);
                var oWorkFlow = cWorkFlow.GetItem(long.Parse(idWorkflow));

                txtName.SetValue(oDato.Nombre);
                txtCode.SetValue(oDato.Codigo);

                cmbType.SetValue(oDato.CoreBusinessProcessTipoID);
                cmbWorkflow.SetValue(oWorkFlow.Nombre);
                cmbStatus.SetValue(oDato.EstadoInicialID);

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
        public DirectResponse AgregarEditar(bool bAgregar, List<string> listaWorkFlowsIDs)
        {
            DirectResponse direct = new DirectResponse();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            BusinessProcessController cBusiness = new BusinessProcessController();
            BusinessProcessWorkFlowController cWorkFlow = new BusinessProcessWorkFlowController();
            InfoResponse oResponse;
            InfoResponse oResponseWorkFlow;

            try
            {
                if (bAgregar)
                {
                    Data.CoreBusinessProcess oDato = new Data.CoreBusinessProcess
                    {
                        Nombre = txtName.Value.ToString(),
                        Codigo = txtCode.Value.ToString(),
                        CoreBusinessProcessTipoID = long.Parse(cmbType.Value.ToString()),
                        EstadoInicialID = long.Parse(cmbStatus.Value.ToString()),
                        Activo = true,
                        ClienteID = lCliID
                    };

                    oResponse = cBusiness.Add(oDato);
                    if (oResponse.Result)
                    {
                        oResponse = cBusiness.SubmitChanges();

                        if (oResponse.Result)
                        {
                            foreach (string item in listaWorkFlowsIDs)
                            {
                                Data.CoreBusinessProcessWorkflows oData = new Data.CoreBusinessProcessWorkflows();
                                oData.CoreBusinessProcessID = oDato.CoreBusinessProcessID;
                                oData.CoreWorkflowID = long.Parse(item);
                                oResponseWorkFlow = cWorkFlow.Add(oData);

                                if (oResponseWorkFlow.Result)
                                {
                                    cWorkFlow.SubmitChanges();

                                    log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                                    direct.Success = true;
                                    direct.Result = "";
                                }
                                else
                                {
                                    cWorkFlow.DiscardChanges();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(oResponse.Description);
                                }
                            }
                        }
                        else
                        {
                            cBusiness.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cBusiness.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    long RegID = long.Parse(GridRowSelectBusinessProcess.SelectedRecordID);
                    var oDato = cBusiness.GetItem(RegID);
                    oDato.Nombre = txtName.Value.ToString();
                    oDato.Codigo = txtCode.Value.ToString();
                    oDato.CoreBusinessProcessTipoID = long.Parse(cmbType.Value.ToString());
                    oDato.EstadoInicialID = long.Parse(cmbStatus.Value.ToString());

                    List<Data.CoreBusinessProcessWorkflows> listaBorrar = new List<Data.CoreBusinessProcessWorkflows>();
                    listaBorrar = cWorkFlow.getAllByBusinessID(RegID);

                    foreach (Data.CoreBusinessProcessWorkflows item in listaBorrar)
                    {
                        cWorkFlow.Delete(item);
                    }

                    foreach (string item in listaWorkFlowsIDs)
                    {
                        Data.CoreBusinessProcessWorkflows oData = new Data.CoreBusinessProcessWorkflows();
                        oData.CoreBusinessProcessID = oDato.CoreBusinessProcessID;
                        oData.CoreWorkflowID = long.Parse(item);
                        cWorkFlow.Add(oData);
                    }

                    oResponse = cBusiness.Update(oDato);
                    if (!oResponse.Result)
                    {
                        cBusiness.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                    else 
                    {
                        cBusiness.SubmitChanges();

                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                        direct.Success = true;
                        direct.Result = "";
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

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            long RegID = long.Parse(GridRowSelectBusinessProcess.SelectedRecordID);
            BusinessProcessController cBusiness = new BusinessProcessController();
            BusinessProcessWorkFlowController cWorkFlow = new BusinessProcessWorkFlowController();
            InfoResponse oResponse;
            InfoResponse oResponseWorkFlow;

            try
            {

                List<Data.CoreBusinessProcessWorkflows> listaBorrar = new List<Data.CoreBusinessProcessWorkflows>();
                listaBorrar = cWorkFlow.getAllByBusinessID(RegID);

                foreach (Data.CoreBusinessProcessWorkflows item in listaBorrar)
                {
                    oResponseWorkFlow = cWorkFlow.Delete(item);
                    if (oResponseWorkFlow.Result)
                    {
                        cWorkFlow.SubmitChanges();
                    }
                    else
                    {
                        cWorkFlow.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponseWorkFlow.Description);
                    }
                }

                Data.CoreBusinessProcess oDato = cBusiness.GetItem(RegID);
                oResponse = cBusiness.Delete(oDato);
                if (oResponse.Result)
                {
                    oResponse = cBusiness.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cBusiness.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cBusiness.DiscardChanges();
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
            return direct;
        }

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            long RegID = long.Parse(GridRowSelectBusinessProcess.SelectedRecordID);
            BusinessProcessController cController = new BusinessProcessController();
            try
            {
                var oDato = cController.GetItem(RegID);
                oDato.Activo = !oDato.Activo;
                if (!cController.UpdateItem(oDato))
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    return direct;
                }
                log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
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

        #region WorkFLow

        protected void storeWorkFlows_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CoreWorkflows> listaWorkFlows;
                    CoreWorkflowsController cController = new CoreWorkflowsController();

                    listaWorkFlows = cController.GetActivos(long.Parse(hdCliID.Value.ToString()));

                    if (listaWorkFlows != null)
                    {
                        storeWorkFlows.DataSource = listaWorkFlows;
                        storeWorkFlows.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion


        #region WorkFLowEstados

        protected void storeWorkFlowsEstados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_CoreEstados> listaWorkFlowsEstados;
                    EstadosController cController = new EstadosController();
                    CoreWorkflowsController cWorkFlow = new CoreWorkflowsController();

                    string idWorkFlow;

                    if (cmbWorkflow.Value != null)
                    {
                        idWorkFlow = cWorkFlow.getIDByName(cmbWorkflow.Value.ToString());

                        listaWorkFlowsEstados = cController.GetCoreEstadosFromWorkflowID(long.Parse(idWorkFlow));

                        if (listaWorkFlowsEstados != null)
                        {
                            storeWorkFlowsEstados.DataSource = listaWorkFlowsEstados;
                            storeWorkFlowsEstados.DataBind();
                        }

                    }
                    else
                    {
                        storeWorkFlowsEstados.DataSource = null;
                        storeWorkFlowsEstados.DataBind();
                    }



                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion


        #region BusinessProcessTipos

        protected void storeBusinessProcessTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CoreBusinessProcessTipos> listaTipos;
                    BusinessProcessTipoController cController = new BusinessProcessTipoController();

                    listaTipos = cController.getAllActivos();

                    if (listaTipos != null)
                    {
                        storeBusinessProcessTipos.DataSource = listaTipos;
                        storeBusinessProcessTipos.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion


        #region BusinessProcessWorkFlowsAdd

        protected void storeBusinessProcessWorkFlowsAdd_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CoreBusinessProcessWorkflows> listaTipos;
                    BusinessProcessWorkFlowController cController = new BusinessProcessWorkFlowController();
                    long RegID = long.Parse(GridRowSelectBusinessProcess.SelectedRecordID);

                    listaTipos = cController.getAllByBusinessID(RegID);

                    if (listaTipos != null)
                    {
                        storeBusinessProcessWorkFlowsAdd.DataSource = listaTipos;
                        storeBusinessProcessWorkFlowsAdd.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion


        #region Store Principal

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CoreBusinessProcess> lista;
                    BusinessProcessController cController = new BusinessProcessController();

                    lista = cController.GetActivos(long.Parse(hdCliID.Value.ToString()));

                    if (lista != null)
                    {
                        storePrincipal.DataSource = lista;
                        storePrincipal.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region Store WorkFlow

        protected void storeWorkflowBP_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CoreWorkflows> lista;
                    CoreWorkflowsController cController = new CoreWorkflowsController();

                    lista = cController.getWorkFlowsByBusinessID(long.Parse(hdBusinessProcess.Value.ToString()));

                    if (lista != null)
                    {
                        storeWorkflowBP.DataSource = lista;
                        storeWorkflowBP.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

    }
}