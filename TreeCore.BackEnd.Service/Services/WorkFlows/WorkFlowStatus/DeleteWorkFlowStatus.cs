using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.WorkFlows;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkFlows
{

    public class DeleteWorkFlowStatus : DeleteObjectService<WorkFlowStatusDTO, WorkFlowStatusEntity, WorkFlowStatusDTOMapper>
    {
        GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> _getDependencies;
        public DeleteWorkFlowStatus(DeleteDependencies<WorkFlowStatusEntity> dependencies, GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<WorkFlowStatusDTO>> Delete(string sCode, int Client)
        {
            var WorkFlowStatusIdenty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<WorkFlowStatusEntity> status = (WorkFlowStatusIdenty == null || WorkFlowStatusIdenty.CoreEstadoID == null ?
                Result.Failure<WorkFlowStatusEntity>(Error.Create(_errorTraduccion.NotFound))
                : WorkFlowStatusIdenty);
            if (status.Success)
            {
                if (status.Valor.Defecto)
                {
                    return Result.Failure<WorkFlowStatusDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(status.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<WorkFlowStatusDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await status.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
