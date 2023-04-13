using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.WorkFlows;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkFlows
{
    public class DeleteWorkflow : DeleteObjectService<WorkflowDTO, WorkflowEntity, WorkflowDTOMapper>
    {
        GetDependencies<WorkflowDTO, WorkflowEntity> _getDependencies;

        public DeleteWorkflow(DeleteDependencies<WorkflowEntity> dependencies, GetDependencies<WorkflowDTO, WorkflowEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<WorkflowDTO>> Delete(string sCode, int Client)
        {
            var workflowIdentity = await _getDependencies.GetItemByCode(sCode, Client);
            Result<WorkflowEntity> workflow = (workflowIdentity == null || workflowIdentity.CoreWorkFlowID == null ?
                Result.Failure<WorkflowEntity>(Error.Create(_errorTraduccion.NotFound))
                : workflowIdentity);

            if (workflow.Success)
            {
                var iResult = await DeleteItem(workflow.Valor);

                if (iResult.Valor == 0)
                {
                    return Result.Failure<WorkflowDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }

                await CommitTransaction(null);
            }

            return await workflow.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}

