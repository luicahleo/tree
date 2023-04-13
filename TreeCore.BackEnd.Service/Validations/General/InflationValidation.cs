using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations.General
{
    public class InflationValidation : BasicValidation<InflationDTO, InflationEntity>
    {
        public override Result<InflationEntity> ValidateEntity(InflationEntity Inflation, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (Inflation.Inflacion.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (Inflation.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (Inflation.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<InflationEntity>(listaErrores.ToImmutableArray())
                : Inflation;
        }

        public override Result<InflationDTO> ValidateDTO(InflationDTO Inflation, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (Inflation.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (Inflation.Name.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (Inflation.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<InflationDTO>(listaErrores.ToImmutableArray())
                : Inflation;
        }
    }
}
