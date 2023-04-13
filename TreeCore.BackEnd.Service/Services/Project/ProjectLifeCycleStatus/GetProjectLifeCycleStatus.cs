using TreeCore.Shared.DTO.Project;
using TreeCore.BackEnd.Service.Mappers.Project;
using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.BackEnd.Model.Entity.Project;

namespace TreeCore.BackEnd.Service.Services.Project.ProjectLifeCycleStatus
{
    public class GetProjectLifeCycleStatus : GetObjectService<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity, ProjectLifeCycleStatusDTOMapper>
    {
        public GetProjectLifeCycleStatus(GetDependencies<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity> getDependencies,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {

        }
    }
}

