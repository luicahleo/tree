using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.WorkOrders;

namespace TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrder
{
    public class GetWorkOrder : GetObjectService<WorkOrderDTO, WorkOrderEntity, WorkOrderDTOMapper>
    {
        public GetWorkOrder(GetDependencies<WorkOrderDTO, WorkOrderEntity> getDependencies, 
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {

        }
    }
}
