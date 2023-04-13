using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TreeCore.APIClient;
using TreeCore.Page;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.WorkFlows;

namespace TreeCore.ModWorkFlow
{
    public partial class WorkFlows : BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        BaseAPIClient<WorkflowDTO> ApiClient;

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            ApiClient = new BaseAPIClient<WorkflowDTO>(TOKEN_API);
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);
                
                List<string> listaIgnore = new List<string>()
                { };

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> {  }},
                { "Post", new List<ComponentBase> {  }},
                { "Put", new List<ComponentBase> { }},
                { "Delete", new List<ComponentBase> {  }}
            };
        }

        #endregion

        #region STORES

        #region WORKFLOWS

        protected void storeWorkFlow_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    int curPage = e.Page;
                    int pageSize = Convert.ToInt32(e.Limit);
                    var sSort = e.Sort;

                    QueryDTO query = new QueryDTO(sSort.First().Property, sSort.First().Direction.ToString(), pageSize, curPage);
                    if (!txtFiltroWorkflows.IsEmpty)
                        query.AddFilter(new FilterDTO(nameof(WorkflowDTO.Name), nameof(Operators.like), $"%{txtFiltroWorkflows.Value.ToString()}%", null));
                    foreach (var oFilter in e.Filter)
                    {
                        query.AddFilter(new FilterDTO(oFilter.Property, nameof(Operators.like), $"%{oFilter.Value}%", null));
                    }
                    var lista = ApiClient.GetList(query).Result;
                    if (lista != null)
                    {
                        e.Total = lista.TotalItems;
                        storeWorkFlow.DataSource = lista.Value;
                        storeWorkFlow.DataBind();
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

        [DirectMethod()]
        public DirectResponse DeleteWorkflow(string code)
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };
            try
            {
                var Result = ApiClient.DeleteEntity(code).Result;

                if (Result.Success)
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = Result.Errors[0].Message;
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
    }
}

