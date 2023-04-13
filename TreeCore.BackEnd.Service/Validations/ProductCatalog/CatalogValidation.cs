using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class CatalogValidation : BasicValidation<CatalogDTO, CatalogEntity>
    {
        public override Result<CatalogEntity> ValidateEntity(CatalogEntity catalog, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (catalog.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalog.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalog.FechaFinVigencia < catalog.FechaInicioVigencia)
            {
                listaErrores.Add(Error.Create(_traduccion.DateIncorrect));
            }

            if ((catalog.PricesReadjustment.FechaFinReajuste != null && catalog.PricesReadjustment.FechaProximaReajuste != null) &&
                (catalog.PricesReadjustment.FechaFinReajuste < catalog.PricesReadjustment.FechaProximaReajuste))
            {
                listaErrores.Add(Error.Create(_traduccion.DateIncorrect));
            }

            if ((catalog.PricesReadjustment.FechaProximaReajuste != null && catalog.PricesReadjustment.FechaInicioReajuste != null) &&
                (catalog.PricesReadjustment.FechaProximaReajuste < catalog.PricesReadjustment.FechaInicioReajuste))
            {
                listaErrores.Add(Error.Create(_traduccion.DateIncorrect));
            }

            return listaErrores.Any() ?
                Result.Failure<CatalogEntity>(listaErrores.ToImmutableArray())
                : catalog;
        }

        public override Result<CatalogDTO> ValidateDTO(CatalogDTO catalog, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (catalog.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalog.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalog.CurrencyCode.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalog.CatalogTypeCode.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (catalog.EndDate < catalog.StartDate)
            {
                listaErrores.Add(Error.Create(_traduccion.DateIncorrect));
            }

            if ((catalog.PricesReadjustment.EndDate != null && catalog.PricesReadjustment.NextDate != null) &&
                (catalog.PricesReadjustment.EndDate < catalog.PricesReadjustment.NextDate))
            {
                listaErrores.Add(Error.Create(_traduccion.DateIncorrect));
            }

            if ((catalog.PricesReadjustment.NextDate != null && catalog.PricesReadjustment.StartDate != null) &&
                (catalog.PricesReadjustment.NextDate < catalog.PricesReadjustment.StartDate))
            {
                listaErrores.Add(Error.Create(_traduccion.DateIncorrect));
            }

            return listaErrores.Any() ?
                Result.Failure<CatalogDTO>(listaErrores.ToImmutableArray())
                : catalog;
        }
    }
}
