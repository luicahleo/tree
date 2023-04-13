using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class CurrencyValidation : BasicValidation<CurrencyDTO, CurrencyEntity>
    {
        public override Result<CurrencyEntity> ValidateEntity(CurrencyEntity currency, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (currency.Moneda.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (currency.Simbolo.Length > 10)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<CurrencyEntity>(listaErrores.ToImmutableArray())
                : currency;
        }

        public override Result<CurrencyDTO> ValidateDTO(CurrencyDTO currency, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (currency.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<CurrencyDTO>(listaErrores.ToImmutableArray())
                : currency;
        }
    }
}

