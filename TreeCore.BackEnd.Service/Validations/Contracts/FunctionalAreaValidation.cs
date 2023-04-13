using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations.Contracts
{
    public class FunctionalAreaValidation : BasicValidation<FunctionalAreaDTO, FunctionalAreaEntity>
    {
        public override Result<FunctionalAreaEntity> ValidateEntity(FunctionalAreaEntity functionalArea, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (functionalArea.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (functionalArea.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (functionalArea.AreaFuncional.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<FunctionalAreaEntity>(listaErrores.ToImmutableArray())
                : functionalArea;
        }

        public override Result<FunctionalAreaDTO> ValidateDTO(FunctionalAreaDTO functionalArea, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (functionalArea.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (functionalArea.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (functionalArea.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<FunctionalAreaDTO>(listaErrores.ToImmutableArray())
                : functionalArea;
        }
    }
}
