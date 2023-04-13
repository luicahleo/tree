using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Project;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Program
{
    public class PostProgram : PostObjectService<ProgramDTO, ProgramEntity, ProgramDTOMapper>
    {

        private readonly GetDependencies<ProgramDTO, ProgramEntity> _getDependency;
        private readonly PutDependencies<ProgramEntity> _putDependency;

        public PostProgram(PostDependencies<ProgramEntity> postDependency, GetDependencies<ProgramDTO, ProgramEntity> getDependency, PutDependencies<ProgramEntity> putDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new ProgramValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
        }

        public override async Task<Result<ProgramEntity>> ValidateEntity(ProgramDTO oEntidad, int client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            ProgramEntity programEntity = new ProgramEntity(null, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, client);

            filter = new Filter(nameof(ProgramDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<ProgramEntity>> listPrograms = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listPrograms.Result != null && listPrograms.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.Program + " " + _traduccion.Code + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<ProgramEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return programEntity;
            }
        }
    }
}