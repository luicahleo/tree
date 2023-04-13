using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.WorkFlows;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkFlows
{
    public class PutWorkflow : PutObjectService<WorkflowDTO, WorkflowEntity, WorkflowDTOMapper>
    {
        private readonly GetDependencies<WorkflowDTO, WorkflowEntity> _getDependency;
        private readonly PutWorkFlowStatus _putWorkflowStatusDependency;
        private readonly GetDependencies<RolDTO, RolEntity> _getRolesDependency;

        public PutWorkflow(PutDependencies<WorkflowEntity> putDependency,
            GetDependencies<WorkflowDTO, WorkflowEntity> getDependency,
            GetDependencies<RolDTO, RolEntity> getRolesDependency,
            PutWorkFlowStatus putWorkflowStatusDependency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new WorkflowValidation())
        {
            _getDependency = getDependency;
            _getRolesDependency = getRolesDependency;
            _putWorkflowStatusDependency = putWorkflowStatusDependency;
        }

        public override async Task<Result<WorkflowEntity>> ValidateEntity (WorkflowDTO workflow, int Client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            IEnumerable<WorkflowEntity> listWorkflows;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            WorkflowEntity workflowFinal = null;

            List<WorkFlowStatusEntity> listStatus = new List<WorkFlowStatusEntity>();

            if (workflow != null && workflow.LinkedStatus != null && workflow.LinkedStatus.Count > 0 && workflow.LinkedStatus[0] != null)
            {
                Result<List<WorkFlowStatusEntity>> listStatusValidity = await _putWorkflowStatusDependency.ValidateEntity(workflow.LinkedStatus, Client, workflow);
                if (listStatusValidity.Success)
                {
                    listStatus = listStatusValidity.Value;
                }
                else
                {
                    lErrors.AddRange(listStatusValidity.Errors);
                }
            }

            WorkflowEntity? workF = await _getDependency.GetItemByCode(code, Client);
            if (workF == null)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.Workflow + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                IEnumerable<RolEntity> linkedRoles = null;
                if (workflow != null && workflow.LinkedRoles != null && workflow.LinkedRoles.Count > 0 && workflow.LinkedRoles[0] != null)
                {
                    List<Filter> filtersRoles = new List<Filter>();

                    foreach (string codeLinkedRoles in workflow.LinkedRoles)
                    {
                        filtersRoles.Add(new Filter(nameof(RolDTO.Code).ToLower(), Operators.eq, codeLinkedRoles, Filter.Types.OR, null));
                    }

                    linkedRoles = await _getRolesDependency.GetList(Client, filtersRoles, null, null, -1, -1);
                    IEnumerable<string> iEcodesRoles = linkedRoles.Select(lp => lp.Codigo);
                    IEnumerable<string> lpRoles = workflow.LinkedRoles;

                    IEnumerable<string> intersectRoles = iEcodesRoles.Union(lpRoles).Except(iEcodesRoles.Intersect(lpRoles));

                    if (intersectRoles.Count() > 0)
                    {
                        foreach (string scodeRoles in intersectRoles.ToList())
                        {
                            lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRoles + $"' {_errorTraduccion.NotFound}.", null));
                        };
                    }
                }

                workflowFinal = new WorkflowEntity(workF.CoreWorkFlowID, Client, workflow.Code, workflow.Name, workflow.Description, workflow.Active, workflow.Public, listStatus, linkedRoles);

                filter = new Filter(nameof(WorkflowDTO.Code), Operators.eq, workflow.Code);
                listFilters.Add(filter);

                listWorkflows = await _getDependency.GetList(Client, listFilters, null, null, -1, -1);
                if (listWorkflows != null && listWorkflows.ListOrEmpty().Count > 0 &&
                    listWorkflows.ListOrEmpty()[0].CoreWorkFlowID != workF.CoreWorkFlowID)
                {
                    lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.Workflow + " " + $"{workflow.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkflowEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return workflowFinal;
            }
        }
    }
}

