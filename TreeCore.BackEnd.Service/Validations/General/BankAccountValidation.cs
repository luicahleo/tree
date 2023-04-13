using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class BankAccountValidation : BasicValidation<BankAccountDTO, BankAccountEntity>
    {
        public override Result<BankAccountEntity> ValidateEntity(BankAccountEntity company, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (company.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (company.IBAN.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<BankAccountEntity>(listaErrores.ToImmutableArray())
                : company;
        }

        public override Result<BankAccountDTO> ValidateDTO(BankAccountDTO company, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (company.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (company.IBAN.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (company.Description.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            return listaErrores.Any() ?
                Result.Failure<BankAccountDTO>(listaErrores.ToImmutableArray())
                : company;
        }
    }
}
