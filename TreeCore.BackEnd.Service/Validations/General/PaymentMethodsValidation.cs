using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class PaymentMethodsValidation : BasicValidation<PaymentMethodsDTO, PaymentMethodsEntity>
    {
        public override Result<PaymentMethodsEntity> ValidateEntity(PaymentMethodsEntity paymentMethods, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (paymentMethods.CodigoMetodoPago.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (paymentMethods.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (paymentMethods.MetodoPago.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<PaymentMethodsEntity>(listaErrores.ToImmutableArray())
                : paymentMethods;
        }

        public override Result<PaymentMethodsDTO> ValidateDTO(PaymentMethodsDTO paymentMethods, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (paymentMethods.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (paymentMethods.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (paymentMethods.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<PaymentMethodsDTO>(listaErrores.ToImmutableArray())
                : paymentMethods;
        }
    }
}
