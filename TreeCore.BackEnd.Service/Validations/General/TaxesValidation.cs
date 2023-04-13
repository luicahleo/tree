using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class TaxesValidation : BasicValidation<TaxesDTO, TaxesEntity>
    {
        public override Result<TaxesEntity> ValidateEntity(TaxesEntity taxes, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (taxes.Impuesto.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<TaxesEntity>(listaErrores.ToImmutableArray())
                : taxes;
        }

        public override Result<TaxesDTO> ValidateDTO(TaxesDTO taxes, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (taxes.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<TaxesDTO>(listaErrores.ToImmutableArray())
                : taxes;
        }
    }
}

