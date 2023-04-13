using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.WorkOrders;

namespace TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrderTrackingStatus
{
    public class GetWorkOrderTrackingStatus : GetObjectService<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity, WorkOrderTrackingStatusDTOMapper>
    {
        public GetWorkOrderTrackingStatus(GetDependencies<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity> getDependencies,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {

        }
    }
}
