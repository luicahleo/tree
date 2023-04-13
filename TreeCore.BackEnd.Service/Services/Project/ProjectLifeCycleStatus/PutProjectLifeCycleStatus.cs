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
    public class PutProjectLifeCycleStatus : PutObjectService<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity, ProjectLifeCycleStatusDTOMapper>
    {
        private readonly GetDependencies<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity> _getDependency;

        public PutProjectLifeCycleStatus(PutDependencies<ProjectLifeCycleStatusEntity> putDependency, GetDependencies<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity> getDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, putDependency, new ProjectLifeCycleStatusValidation())
        {
            _getDependency = getDependency;
        }

        public override async Task<Result<ProjectLifeCycleStatusEntity>> ValidateEntity(ProjectLifeCycleStatusDTO oEntidad, int client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<ProjectLifeCycleStatusEntity>> listProjectLifeCycleStatus;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            ProjectLifeCycleStatusEntity? pLifeCycleStatus = await _getDependency.GetItemByCode(code, client);
            ProjectLifeCycleStatusEntity pLifeCycleStatusFinal = null;

            if (pLifeCycleStatus == null)
            {
                lErrors.Add(Error.Create(_traduccion.ProjectLifeCycleStatus + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                pLifeCycleStatusFinal = new ProjectLifeCycleStatusEntity(pLifeCycleStatus.CoreProjectLifeCycleStatusID, oEntidad.Code, oEntidad.Name,
                 oEntidad.Description, oEntidad.Active, oEntidad.Colour, client);

                filter = new Filter(nameof(ProjectLifeCycleStatusDTO.Code), Operators.eq, oEntidad.Code);
                listFilters.Add(filter);

                listProjectLifeCycleStatus = _getDependency.GetList(client, listFilters, null, null, -1, -1);
                if (listProjectLifeCycleStatus.Result != null && listProjectLifeCycleStatus.Result.ListOrEmpty().Count > 0 &&
                    listProjectLifeCycleStatus.Result.ListOrEmpty()[0].CoreProjectLifeCycleStatusID != pLifeCycleStatusFinal.CoreProjectLifeCycleStatusID)
                {
                    lErrors.Add(Error.Create(_traduccion.ProjectLifeCycleStatus + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                listProjectLifeCycleStatus = _getDependency.GetList(client, listFiltersDefault, null, null, -1, -1);
                if (listProjectLifeCycleStatus.Result != null && listProjectLifeCycleStatus.Result.ListOrEmpty().Count > 0)
                {
                    ProjectLifeCycleStatusEntity pType = new ProjectLifeCycleStatusEntity(listProjectLifeCycleStatus.Result.ListOrEmpty()[0].CoreProjectLifeCycleStatusID.Value, listProjectLifeCycleStatus.Result.ListOrEmpty()[0].Codigo,
                        listProjectLifeCycleStatus.Result.ListOrEmpty()[0].Nombre, listProjectLifeCycleStatus.Result.ListOrEmpty()[0].Descripcion, listProjectLifeCycleStatus.Result.ListOrEmpty()[0].Activo, listProjectLifeCycleStatus.Result.ListOrEmpty()[0].Color, client);
                    await _putDependencies.Update(pType);
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<ProjectLifeCycleStatusEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return pLifeCycleStatusFinal;
            }
        }
    }
}