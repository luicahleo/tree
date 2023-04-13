using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.BusinessProcess;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.BusinessProcess;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.BusinessProcess
{
    public class PostBusinessProcess : PostObjectService<BusinessProcessDTO, BusinessProcessEntity, BusinessProcessDTOMapper>
    {
        private readonly GetDependencies<BusinessProcessDTO, BusinessProcessEntity> _getDependency;
        private readonly GetDependencies<BusinessProcessTypeDTO, BusinessProcessTypeEntity> _getBusinessProcessTypeDependency;
        private readonly GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> _getWorkflowStatusDependency;
        private readonly GetDependencies<WorkflowDTO, WorkflowEntity> _getWorkflowsDependency;

        public PostBusinessProcess(PostDependencies<BusinessProcessEntity> postDependency,
            GetDependencies<BusinessProcessDTO, BusinessProcessEntity> getDependency,
            GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> getWorkflowStatusDependency,
            GetDependencies<BusinessProcessTypeDTO, BusinessProcessTypeEntity> getBusinessProcessTypeDependency,
            GetDependencies<WorkflowDTO, WorkflowEntity> getWorkflowsDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new BusinessProcessValidation())
        {
            _getDependency = getDependency;
            _getWorkflowsDependency = getWorkflowsDependency;
            _getWorkflowStatusDependency = getWorkflowStatusDependency;
            _getBusinessProcessTypeDependency = getBusinessProcessTypeDependency;
        }

        public override async Task<Result<BusinessProcessEntity>> ValidateEntity(BusinessProcessDTO businessProcess, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            WorkFlowStatusEntity WorkflowStatus = await _getWorkflowStatusDependency.GetItemByCode(businessProcess.InitialWorkflowStatusCode, Client);
            if (WorkflowStatus == null)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.CodeWorkFlowStatus + " " + $"{businessProcess.InitialWorkflowStatusCode}" + " " + _errorTraduccion.NotFound + "."));
            }

            BusinessProcessTypeEntity businessProcessType = await _getBusinessProcessTypeDependency.GetItemByCode(businessProcess.BusinessProcessTypeCode, Client);
            if (businessProcessType == null)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.BusinessProcessType + " " + $"{businessProcess.BusinessProcessTypeCode}" + " " + _errorTraduccion.NotFound + "."));
            }

            IEnumerable<WorkflowEntity> linkedWorkflows = null;
            if (businessProcess != null && businessProcess.LinkedWorkflows != null && businessProcess.LinkedWorkflows.Count > 0)
            {
                if (!businessProcess.LinkedWorkflows.Contains(businessProcess.InitialWorkflowCode))
                {
                    lErrors.Add(Error.Create($"{nameof(BusinessProcessDTO.Code)} '" + businessProcess.InitialWorkflowCode + $"' is not in the list of possible workflows.", null));
                }
                else
                {
                    List<Filter> filtersCodesWorkflows = new List<Filter>();

                    foreach (string codeLinkedWorkflow in businessProcess.LinkedWorkflows)
                    {
                        filtersCodesWorkflows.Add(new Filter(nameof(WorkflowDTO.Code).ToLower(), Operators.eq, codeLinkedWorkflow, Filter.Types.OR, null));
                    }

                    linkedWorkflows = await _getWorkflowsDependency.GetList(Client, filtersCodesWorkflows, null, null, -1, -1);
                    IEnumerable<string> iEcodesWorkflows = linkedWorkflows.Select(lp => lp.Codigo);
                    IEnumerable<string> lpWorkflow = businessProcess.LinkedWorkflows;

                    IEnumerable<string> intersectWorkflow = iEcodesWorkflows.Union(lpWorkflow).Except(iEcodesWorkflows.Intersect(lpWorkflow));

                    if (intersectWorkflow.Count() > 0)
                    {
                        foreach (string scodeWorkflow in intersectWorkflow.ToList())
                        {
                            lErrors.Add(Error.Create($"{nameof(WorkflowDTO.Code)} '" + scodeWorkflow + $"' {_errorTraduccion.NotFound}.", null));
                        };
                    }
                }
            }

            BusinessProcessEntity bProcess = new BusinessProcessEntity(null, Client, businessProcess.Code, businessProcess.Name, businessProcess.Description, businessProcess.Active,
                WorkflowStatus, linkedWorkflows, businessProcessType);

            filter = new Filter(nameof(BusinessProcessDTO.Code), Operators.eq, businessProcess.Code);
            listFilters.Add(filter);

            Task<IEnumerable<BusinessProcessEntity>> listBusinessProcess = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listBusinessProcess.Result != null && listBusinessProcess.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.BusinessProcess + " " + $"{businessProcess.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<BusinessProcessEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return bProcess;
            }
        }
    }
}
