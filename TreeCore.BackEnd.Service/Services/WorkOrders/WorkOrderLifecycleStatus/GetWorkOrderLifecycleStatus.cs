using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.WorkOrders;

namespace TreeCore.BackEnd.Service.Services.WorkOrders
{
    public class GetWorkOrderLifecycleStatus : GetObjectService<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity, WorkOrderLifecycleStatusDTOMapper>
    {
        public GetWorkOrderLifecycleStatus(GetDependencies<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {

        }
    }
}
