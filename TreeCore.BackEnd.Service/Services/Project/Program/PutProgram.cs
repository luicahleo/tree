using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Project;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Program
{

    public class PutProgram : PutObjectService<ProgramDTO, ProgramEntity, ProgramDTOMapper>
    {
        private readonly GetDependencies<ProgramDTO, ProgramEntity> _getDependency;

        public PutProgram(PutDependencies<ProgramEntity> putDependency,
            GetDependencies<ProgramDTO, ProgramEntity> getDependency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new ProgramValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<ProgramEntity>> ValidateEntity(ProgramDTO oEntidad, int client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<ProgramEntity>> listprograms;
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            ProgramEntity? bProcessType = await _getDependency.GetItemByCode(code, client);
            ProgramEntity bProcessTypeFinal = null;

            if (bProcessType == null)
            {
                lErrors.Add(Error.Create(_traduccion.Program + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                bProcessTypeFinal = new ProgramEntity(bProcessType.CoreProgramID, oEntidad.Code, oEntidad.Name, oEntidad.Description, oEntidad.Active, client);

                filter = new Filter(nameof(ProgramDTO.Code), Operators.eq, oEntidad.Code);
                listFilters.Add(filter);

                listprograms = _getDependency.GetList(client, listFilters, null, null, -1, -1);
                if (listprograms.Result != null && listprograms.Result.ListOrEmpty().Count > 0 &&
                    listprograms.Result.ListOrEmpty()[0].CoreProgramID != bProcessTypeFinal.CoreProgramID)
                {
                    lErrors.Add(Error.Create(_traduccion.Program + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }

                if (listprograms.Result != null && listprograms.Result.ListOrEmpty().Count > 0)
                {
                    ProgramEntity pType = new ProgramEntity(listprograms.Result.ListOrEmpty()[0].CoreProgramID.Value, listprograms.Result.ListOrEmpty()[0].Codigo,
                        listprograms.Result.ListOrEmpty()[0].Nombre, listprograms.Result.ListOrEmpty()[0].Descripcion, listprograms.Result.ListOrEmpty()[0].Activo, client);
                    await _putDependencies.Update(pType);
                }
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<ProgramEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return bProcessTypeFinal;
            }
        }
    }
}
