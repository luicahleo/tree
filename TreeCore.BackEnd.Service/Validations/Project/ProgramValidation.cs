using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Config;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Config;
using TreeCore.Shared.DTO.Project;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class ProgramValidation : BasicValidation<ProgramDTO, ProgramEntity>
    {
        public override Result<ProgramEntity> ValidateEntity(ProgramEntity program, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (program.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (program.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (program.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ProgramEntity>(listaErrores.ToImmutableArray())
                : program;
        }

        public override Result<ProgramDTO> ValidateDTO(ProgramDTO program, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (program.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (program.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (program.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ProgramDTO>(listaErrores.ToImmutableArray())
                : program;
        }
    }
}
