using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class ProductTypeValidation : BasicValidation<ProductTypeDTO, ProductTypeEntity>
    {
        public override Result<ProductTypeEntity> ValidateEntity(ProductTypeEntity productType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (productType.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (productType.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (productType.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ProductTypeEntity>(listaErrores.ToImmutableArray())
                : productType;
        }

        public override Result<ProductTypeDTO> ValidateDTO(ProductTypeDTO productType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (productType.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (productType.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (productType.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ProductTypeDTO>(listaErrores.ToImmutableArray())
                : productType;
        }
    }
}
