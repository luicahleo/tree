using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class ProductValidation : BasicValidation<ProductDTO, ProductEntity>
    {
        public override Result<ProductEntity> ValidateEntity(ProductEntity product, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (product.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (product.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ProductEntity>(listaErrores.ToImmutableArray())
                : product;
        }

        public override Result<ProductDTO> ValidateDTO(ProductDTO product, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (product.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (product.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }


            if (product.ProductTypeCode.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (product.Frecuency.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ProductDTO>(listaErrores.ToImmutableArray())
                : product;
        }
    }
}
