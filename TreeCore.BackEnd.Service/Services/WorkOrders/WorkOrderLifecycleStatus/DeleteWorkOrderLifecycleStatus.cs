using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkOrders
{

    public class DeleteWorkOrderLifecycleStatus : DeleteObjectService<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity, WorkOrderLifecycleStatusDTOMapper>
    {
        GetDependencies<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity> _getDependencies;
        public DeleteWorkOrderLifecycleStatus(DeleteDependencies<WorkOrderLifecycleStatusEntity> dependencies, GetDependencies<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<WorkOrderLifecycleStatusDTO>> Delete(string sCode, int Client)
        {
            var WorkOrderTrackingStatusIdenty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<WorkOrderLifecycleStatusEntity> trackingStatus = (WorkOrderTrackingStatusIdenty == null || WorkOrderTrackingStatusIdenty.CoreWorkOrderEstadoID == null ?
                Result.Failure<WorkOrderLifecycleStatusEntity>(Error.Create(_errorTraduccion.NotFound))
                : WorkOrderTrackingStatusIdenty);
            if (trackingStatus.Success)
            {
                if (trackingStatus.Valor.Defecto)
                {
                    return Result.Failure<WorkOrderLifecycleStatusDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(trackingStatus.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<WorkOrderLifecycleStatusDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await trackingStatus.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
