using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TreeCore.APIClient;
using TreeCore.Shared.DTO.Config;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.ModWorkOrders.pages
{
    public partial class FormularioWorkFlow : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        BaseAPIClient<WorkflowDTO> ApiClient;
        private WorkflowDTO WFDTO;

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            ApiClient = new BaseAPIClient<WorkflowDTO>(TOKEN_API);
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (Request.QueryString["WFCode"] != null && Request.QueryString["WFCode"] != "")
                {
                    hdWFCode.SetValue(Request.QueryString["WFCode"]);
                    WFDTO = ApiClient.GetByCode(Request.QueryString["WFCode"]).Result.Value;
                    pnSumWorkFlow.Show();
                    tblEditWF.Show();
                }
                else
                {
                    hdWFCode.SetValue("");
                    WFDTO = new WorkflowDTO();
                    WFDTO.Active = true;
                    WFDTO.LinkedStatus = new List<WorkFlowStatusDTO>();
                    WFDTO.LinkedRoles = new List<string>();
                    pnConfWorkFlow.Show();
                }
                hdObjeto.Value = JsonConvert.SerializeObject(WFDTO);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = "WorkFlows.aspx";
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> { }},
                { "Post", new List<ComponentBase> { }},
                { "Put", new List<ComponentBase> { }},
                { "Delete", new List<ComponentBase> { }}
            };

        }


        #endregion

        #region STORES

        #region Roles

        protected void storeRoles_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<RolDTO> apiCl = new BaseAPIClient<RolDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeRoles.DataSource = lista.Value;
                        storeRoles.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        protected void storeStatusGroup_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<WorkFlowStatusGroupDTO> apiCl = new BaseAPIClient<WorkFlowStatusGroupDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeStatusGroup.DataSource = lista.Value;
                        storeStatusGroup.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod]
        public DirectResponse UploadWF()
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };
            try
            {
                ResultDto<WorkflowDTO> result;
                WFDTO = JsonConvert.DeserializeObject<WorkflowDTO>(hdObjeto.Value.ToString());
                if (hdWFCode.Value.ToString() == "")
                {
                    result = ApiClient.AddEntity(WFDTO).Result;
                }
                else
                {
                    result = ApiClient.UpdateEntity(hdWFCode.Value.ToString(), WFDTO).Result;
                }
                if (result.Success)
                {
                    if (hdWFCode.Value.ToString() == "")
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                    }
                    else
                    {
                        log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                    }
                    hdWFCode.SetValue(result.Value.Code);
                }
                else
                {
                    direct.Success = false;
                    direct.Result = result.Errors[0].Message;
                    return direct;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        #endregion

        #region FUNCTIONS



        #endregion
    }
}