using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrder
{
    public class DeleteWorkOrder : DeleteObjectService<WorkOrderDTO, WorkOrderEntity, WorkOrderDTOMapper>
    {
        GetDependencies<WorkOrderDTO, WorkOrderEntity> _getDependencies;

        public DeleteWorkOrder(DeleteDependencies<WorkOrderEntity> dependencies, GetDependencies<WorkOrderDTO, WorkOrderEntity> getDepencies,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<WorkOrderDTO>> Delete(string sCode, int Client)
        {
            var WorkOrderIdenty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<WorkOrderEntity> workOrder = (WorkOrderIdenty == null || WorkOrderIdenty.CoreWorkOrderID == null ?
                Result.Failure<WorkOrderEntity>(Error.Create(_errorTraduccion.NotFound))
                : WorkOrderIdenty);

            if (workOrder.Success)
            {
                var iResult = await DeleteItem(workOrder.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<WorkOrderDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await workOrder.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
