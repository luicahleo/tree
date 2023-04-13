using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Mappers.ProductCatalog;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.DTO.Project;

namespace TreeCore.BackEnd.Service.Services.Project
{
    public class GetProject : GetObjectService<ProjectDTO, ProjectEntity, ProjectDTOMapper>
    {
        public GetProject(GetDependencies<ProjectDTO, ProjectEntity> getDependencies, IHttpContextAccessor httpcontextAccessor)
            : base(httpcontextAccessor, getDependencies) 
        {}
    }
}
