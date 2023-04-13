using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.WorkFlows;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.WorkFlows;

namespace TreeCore.BackEnd.Service.Services.WorkFlows
{
    public class GetWorkFlowStatusGroup : GetObjectService<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity, WorkFlowStatusGroupDTOMapper>
    {
        public GetWorkFlowStatusGroup(GetDependencies<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {

        }
    }
}
