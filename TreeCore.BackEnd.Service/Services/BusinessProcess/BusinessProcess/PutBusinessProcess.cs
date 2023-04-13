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
    public class PutBusinessProcess : PutObjectService<BusinessProcessDTO, BusinessProcessEntity, BusinessProcessDTOMapper>
    {
        private readonly GetDependencies<BusinessProcessDTO, BusinessProcessEntity> _getDependency;
        private readonly GetDependencies<BusinessProcessTypeDTO, BusinessProcessTypeEntity> _getBusinessProcessTypeDependency;
        private readonly GetDependencies<WorkflowDTO, WorkflowEntity> _getWorkflowsDependency;
        private readonly GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> _getWorkflowStatusDependency;

        public PutBusinessProcess(PutDependencies<BusinessProcessEntity> putDependency,
            GetDependencies<BusinessProcessDTO, BusinessProcessEntity> getDependency,
            GetDependencies<WorkflowDTO, WorkflowEntity> getWorkflowsDependency,
            GetDependencies<BusinessProcessTypeDTO, BusinessProcessTypeEntity> getBusinessProcessTypeDependency,
            GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> getWorkflowStatusDependency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new BusinessProcessValidation())
        {
            _getDependency = getDependency;
            _getWorkflowsDependency = getWorkflowsDependency;
            _getWorkflowStatusDependency = getWorkflowStatusDependency;
            _getBusinessProcessTypeDependency = getBusinessProcessTypeDependency;
        }

        public override async Task<Result<BusinessProcessEntity>> ValidateEntity(BusinessProcessDTO businessProcess, int Client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<BusinessProcessEntity>> listBusinessProcess;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            BusinessProcessEntity businessProcessFinal = null;

            BusinessProcessEntity? bProcess = await _getDependency.GetItemByCode(code, Client);
            if (bProcess == null)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.BusinessProcess + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
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

                businessProcessFinal = new BusinessProcessEntity(bProcess.CoreBusinessProcessID, Client, businessProcess.Code, businessProcess.Name, businessProcess.Description, 
                    businessProcess.Active, WorkflowStatus, linkedWorkflows, businessProcessType);

                filter = new Filter(nameof(BusinessProcessDTO.Code), Operators.eq, businessProcess.Code);
                listFilters.Add(filter);

                listBusinessProcess = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
                if (listBusinessProcess.Result != null && listBusinessProcess.Result.ListOrEmpty().Count > 0 &&
                    listBusinessProcess.Result.ListOrEmpty()[0].CoreBusinessProcessID != businessProcessFinal.CoreBusinessProcessID)
                {
                    lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.BusinessProcess + " " + $"{businessProcess.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<BusinessProcessEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return businessProcessFinal;
            }
        }
    }
}
