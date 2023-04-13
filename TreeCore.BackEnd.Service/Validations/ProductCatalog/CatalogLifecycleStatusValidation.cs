using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class CatalogLifecycleStatusValidation : BasicValidation<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity>
    {
        public override Result<CatalogLifecycleStatusEntity> ValidateEntity(CatalogLifecycleStatusEntity catalogLifecycleStatus, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (catalogLifecycleStatus.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalogLifecycleStatus.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalogLifecycleStatus.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<CatalogLifecycleStatusEntity>(listaErrores.ToImmutableArray())
                : catalogLifecycleStatus;
        }

        public override Result<CatalogLifecycleStatusDTO> ValidateDTO(CatalogLifecycleStatusDTO catalogLifecycleStatus, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (catalogLifecycleStatus.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalogLifecycleStatus.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalogLifecycleStatus.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<CatalogLifecycleStatusDTO>(listaErrores.ToImmutableArray())
                : catalogLifecycleStatus;
        }
    }
}

