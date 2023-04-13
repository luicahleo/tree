using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class TaxpayerTypeValidation : BasicValidation<TaxpayerTypeDTO, TaxpayerTypeEntity>
    {
        public override Result<TaxpayerTypeEntity> ValidateEntity(TaxpayerTypeEntity taxpayerType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (taxpayerType.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (taxpayerType.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (taxpayerType.TipoContribuyente.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<TaxpayerTypeEntity>(listaErrores.ToImmutableArray())
                : taxpayerType;
        }

        public override Result<TaxpayerTypeDTO> ValidateDTO(TaxpayerTypeDTO taxpayerType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (taxpayerType.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (taxpayerType.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (taxpayerType.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<TaxpayerTypeDTO>(listaErrores.ToImmutableArray())
                : taxpayerType;
        }
    }
}
