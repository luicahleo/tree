using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class CatalogTypeValidation : BasicValidation<CatalogTypeDTO, CatalogTypeEntity>
    {
        public override Result<CatalogTypeEntity> ValidateEntity(CatalogTypeEntity catalogType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (catalogType.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalogType.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalogType.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<CatalogTypeEntity>(listaErrores.ToImmutableArray())
                : catalogType;
        }

        public override Result<CatalogTypeDTO> ValidateDTO(CatalogTypeDTO catalogType, ErrorTranslations _traduccion)
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
                Result.Failure<CatalogTypeDTO>(listaErrores.ToImmutableArray())
                : catalogType;
        }
    }
}
