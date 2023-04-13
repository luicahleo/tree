using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.WorkFlows;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkFlows
{

    public class DeleteWorkFlowStatusGroup : DeleteObjectService<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity, WorkFlowStatusGroupDTOMapper>
    {
        GetDependencies<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity> _getDependencies;
        public DeleteWorkFlowStatusGroup(DeleteDependencies<WorkFlowStatusGroupEntity> dependencies, GetDependencies<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<WorkFlowStatusGroupDTO>> Delete(string sCode, int Client)
        {
            var WorkFlowStatusGroupIdenty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<WorkFlowStatusGroupEntity> statusGroup = (WorkFlowStatusGroupIdenty == null || WorkFlowStatusGroupIdenty.EstadoAgrupacionID == null ?
                Result.Failure<WorkFlowStatusGroupEntity>(Error.Create(_errorTraduccion.NotFound))
                : WorkFlowStatusGroupIdenty);
            if (statusGroup.Success)
            {
                if (statusGroup.Valor.Defecto)
                {
                    return Result.Failure<WorkFlowStatusGroupDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(statusGroup.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<WorkFlowStatusGroupDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await statusGroup.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
