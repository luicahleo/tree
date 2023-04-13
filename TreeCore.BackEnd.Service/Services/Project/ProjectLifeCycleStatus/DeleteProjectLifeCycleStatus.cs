using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.ROP;

using TreeCore.Shared.DTO.Project;
using TreeCore.BackEnd.Service.Mappers.Project;
using TreeCore.BackEnd.Model.Entity.Project;

namespace TreeCore.BackEnd.Service.Services.Project.ProjectLifeCycleStatus
{
    public class DeleteProjectLifeCycleStatus : DeleteObjectService<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity, ProjectLifeCycleStatusDTOMapper>
    {
        private readonly GetDependencies<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity> _getDependencies;
        public DeleteProjectLifeCycleStatus(DeleteDependencies<ProjectLifeCycleStatusEntity> dependencies,
            GetDependencies<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ProjectLifeCycleStatusDTO>> Delete(string code, int client)
        {
            var projectLifeCycleStatusIndentity = await _getDependencies.GetItemByCode(code, client);
            Result<ProjectLifeCycleStatusEntity> ProjectLifeCycleStatus = (projectLifeCycleStatusIndentity == null || projectLifeCycleStatusIndentity.CoreProjectLifeCycleStatusID == null ?
                Result.Failure<ProjectLifeCycleStatusEntity>(Error.Create(_errorTraduccion.NotFound))
                : projectLifeCycleStatusIndentity);
            if (ProjectLifeCycleStatus.Success)
            {
                var iResult = await DeleteItem(ProjectLifeCycleStatus.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ProjectLifeCycleStatusDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(projectLifeCycleStatusIndentity);
            }
            return await ProjectLifeCycleStatus.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
