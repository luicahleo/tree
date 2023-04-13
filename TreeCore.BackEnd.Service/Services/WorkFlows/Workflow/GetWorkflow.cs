using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.WorkFlows;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.WorkFlows;

namespace TreeCore.BackEnd.Service.Services.WorkFlows
{
    public class GetWorkflow : GetObjectService<WorkflowDTO, WorkflowEntity, WorkflowDTOMapper>
    {
        public GetWorkflow(GetDependencies<WorkflowDTO, WorkflowEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : 
            base(httpcontextAccessor, getDependencies)
        {

        }
    }
}
