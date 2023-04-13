using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.Threading.Tasks;

using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.Shared.DTO.Project;
using TreeCore.BackEnd.Service.Mappers.Project;
using TreeCore.BackEnd.Service.Validations.Project;
using System.Collections.Immutable;

namespace TreeCore.BackEnd.Service.Services.Project.ProjectLifeCycleStatus
{
    public class PostProjectLifeCycleStatus : PostObjectService<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity, ProjectLifeCycleStatusDTOMapper>
    {
        private readonly GetDependencies<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity> _getDependency;
        private readonly PutDependencies<ProjectLifeCycleStatusEntity> _putDependency;

        public PostProjectLifeCycleStatus(PostDependencies<ProjectLifeCycleStatusEntity> postDependency, PutDependencies<ProjectLifeCycleStatusEntity> putDependency, GetDependencies<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity> getDependency, 
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, postDependency, new ProjectLifeCycleStatusValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<ProjectLifeCycleStatusEntity>> ValidateEntity(ProjectLifeCycleStatusDTO oEntidad, int client, string email, string code = "")
        {

            List<Error> lErrors = new List<Error>();


            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            ProjectLifeCycleStatusEntity projectLifeCycleStatusEntity = new ProjectLifeCycleStatusEntity(null, oEntidad.Code, oEntidad.Name, oEntidad.Description,
                oEntidad.Active, oEntidad.Colour, client);

            filter = new Filter(nameof(ProjectLifeCycleStatusDTO.Code).ToLower(), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<ProjectLifeCycleStatusEntity>> listProjectLifeCycleStatus = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listProjectLifeCycleStatus.Result != null && listProjectLifeCycleStatus.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.ProjectLifeCycleStatus + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<ProjectLifeCycleStatusEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return projectLifeCycleStatusEntity;
            }

        }
    }
}
