using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrderTrackingStatus
{
    public class DeleteWorkOrderTrackingStatus : DeleteObjectService<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity, WorkOrderTrackingStatusDTOMapper>
    {
        GetDependencies<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity> _getDependencies;

        public DeleteWorkOrderTrackingStatus(DeleteDependencies<WorkOrderTrackingStatusEntity> dependencies, GetDependencies<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity> getDepencies, 
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<WorkOrderTrackingStatusDTO>> Delete(string sCode, int Client)
        {
            var WorkOrderTrackingStatusIdenty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<WorkOrderTrackingStatusEntity> workOrder = (WorkOrderTrackingStatusIdenty == null || WorkOrderTrackingStatusIdenty.CoreWorkOrderTrackingStatusID == null ?
                Result.Failure<WorkOrderTrackingStatusEntity>(Error.Create(_errorTraduccion.NotFound))
                : WorkOrderTrackingStatusIdenty);

            if (workOrder.Success)
            {
                var iResult = await DeleteItem(workOrder.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<WorkOrderTrackingStatusDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await workOrder.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
