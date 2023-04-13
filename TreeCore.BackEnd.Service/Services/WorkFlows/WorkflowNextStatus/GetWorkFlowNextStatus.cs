using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.WorkFlows;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.WorkFlows;

namespace TreeCore.BackEnd.Service.Services.WorkFlows
{
    public class GetWorkFlowNextStatus : GetObjectService<WorkFlowNextStatusDTO, WorkFlowNextStatusEntity, WorkFlowNextStatusDTOMapper>
    {
        public GetWorkFlowNextStatus(GetDependencies<WorkFlowNextStatusDTO, WorkFlowNextStatusEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {
            
        }

    }
}
