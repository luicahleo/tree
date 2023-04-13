using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class TaxIdentificationNumberCategoryValidation : BasicValidation<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity>
    {
        public override Result<TaxIdentificationNumberCategoryEntity> ValidateEntity(TaxIdentificationNumberCategoryEntity taxIdentificationNumberCategory, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (taxIdentificationNumberCategory.Codigo.Length > 6)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (taxIdentificationNumberCategory.Descripcion.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (taxIdentificationNumberCategory.Nombre.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<TaxIdentificationNumberCategoryEntity>(listaErrores.ToImmutableArray())
                : taxIdentificationNumberCategory;
        }

        public override Result<TaxIdentificationNumberCategoryDTO> ValidateDTO(TaxIdentificationNumberCategoryDTO taxIdentificationNumberCategory, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (taxIdentificationNumberCategory.Code.Length > 6)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (taxIdentificationNumberCategory.Description.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (taxIdentificationNumberCategory.Name.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<TaxIdentificationNumberCategoryDTO>(listaErrores.ToImmutableArray())
                : taxIdentificationNumberCategory;
        }
    }
}
