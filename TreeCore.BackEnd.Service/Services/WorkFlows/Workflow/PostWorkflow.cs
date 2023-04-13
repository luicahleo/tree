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
    public class PostWorkflow : PostObjectService<WorkflowDTO, WorkflowEntity, WorkflowDTOMapper>
    {

        private readonly GetDependencies<WorkflowDTO, WorkflowEntity> _getDependency;
        private readonly PutDependencies<WorkflowEntity> _putDependency;
        private readonly PostWorkFlowStatus _postWorkflowStatusDependency;
        private readonly GetDependencies<RolDTO, RolEntity> _getRolesDependency;

        public PostWorkflow(PostDependencies<WorkflowEntity> postDependency,
            GetDependencies<WorkflowDTO, WorkflowEntity> getDependency,
            GetDependencies<RolDTO, RolEntity> getRolesDependency,
            PutDependencies<WorkflowEntity> putDependency,
            PostWorkFlowStatus postWorkFlowStatus,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new WorkflowValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
            _getRolesDependency = getRolesDependency;
            _postWorkflowStatusDependency = postWorkFlowStatus;
        }

        public override async Task<Result<WorkflowEntity>> ValidateEntity (WorkflowDTO oEntidad, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            List<WorkFlowStatusEntity> listStatus = new List<WorkFlowStatusEntity>();
            if (oEntidad != null && oEntidad.LinkedStatus != null && oEntidad.LinkedStatus.Count > 0)
            {
                Result<List<WorkFlowStatusEntity>> listStatusValidity = await _postWorkflowStatusDependency.ValidateEntity(oEntidad.LinkedStatus, Client, oEntidad, oEntidad.Code);
                if (listStatusValidity.Success)
                {
                    listStatus = listStatusValidity.Value;
                }
                else
                {
                    lErrors.AddRange(listStatusValidity.Errors);
                }

            }

            IEnumerable<RolEntity> linkedRoles = null;
            if (oEntidad != null && oEntidad.LinkedRoles != null && oEntidad.LinkedRoles.Count > 0)
            {
                List<Filter> filtersRoles = new List<Filter>();

                foreach (string codeLinkedRoles in oEntidad.LinkedRoles)
                {
                    filtersRoles.Add(new Filter(nameof(RolDTO.Code).ToLower(), Operators.eq, codeLinkedRoles, Filter.Types.OR, null));
                }

                linkedRoles = await _getRolesDependency.GetList(Client, filtersRoles, null, null, -1, -1);
                IEnumerable<string> iEcodesRoles = linkedRoles.Select(lp => lp.Codigo);
                IEnumerable<string> lpRoles = oEntidad.LinkedRoles;

                IEnumerable<string> intersectRoles = iEcodesRoles.Union(lpRoles).Except(iEcodesRoles.Intersect(lpRoles));

                if (intersectRoles.Count() > 0)
                {
                    foreach (string scodeRoles in intersectRoles.ToList())
                    {
                        lErrors.Add(Error.Create($"{nameof(RolDTO.Code)} '" + scodeRoles + $"' {_errorTraduccion.NotFound}.", null));
                    };
                }
            }

            WorkflowEntity workflow = new WorkflowEntity(null, Client, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, oEntidad.Public, listStatus, linkedRoles);

            filter = new Filter(nameof(WorkflowDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);
            Task<IEnumerable<WorkflowEntity>> listWorkflows = _getDependency.GetList(Client, listFilters, null, null, -1, -1);

            if (listWorkflows.Result != null && listWorkflows.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.Workflow + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkflowEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return workflow;
            }
        }
    }
}