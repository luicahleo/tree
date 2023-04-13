using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class PaymentTermValidation : BasicValidation<PaymentTermDTO, PaymentTermEntity>
    {
        public override Result<PaymentTermEntity> ValidateEntity(PaymentTermEntity catalogType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (catalogType.Codigo.Length > 6)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalogType.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalogType.CondicionPago.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<PaymentTermEntity>(listaErrores.ToImmutableArray())
                : catalogType;
        }

        public override Result<PaymentTermDTO> ValidateDTO(PaymentTermDTO catalogType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (catalogType.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalogType.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalogType.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<PaymentTermDTO>(listaErrores.ToImmutableArray())
                : catalogType;
        }
    }
}
