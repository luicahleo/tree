using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.DTO.ValueObject;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class CatalogDTOMapper : BaseMapper<CatalogDTO, CatalogEntity>
    {
        public override Task<CatalogDTO> Map(CatalogEntity catalog)
        {
            PriceReadjustmentDTO pricesDTO = new PriceReadjustmentDTO();
            pricesDTO.Type = (catalog.PricesReadjustment != null) ? catalog.PricesReadjustment.Tipo: "null";
            pricesDTO.CodeInflation = ((catalog.PricesReadjustment != null) && (catalog.PricesReadjustment.Inflacion != null)) ? catalog.PricesReadjustment.Inflacion.Codigo : "";
            pricesDTO.FixedAmount = (catalog.PricesReadjustment != null) ? catalog.PricesReadjustment.CantidadFija : null;
            pricesDTO.FixedPercentage = (catalog.PricesReadjustment != null) ? catalog.PricesReadjustment.PorcentajeFijo : null;
            pricesDTO.Frequency = (catalog.PricesReadjustment != null) ? catalog.PricesReadjustment.Periodicidad : null;
            pricesDTO.StartDate = (catalog.PricesReadjustment != null) ? catalog.PricesReadjustment.FechaInicioReajuste : null;
            pricesDTO.NextDate = (catalog.PricesReadjustment != null) ? catalog.PricesReadjustment.FechaProximaReajuste : null;
            pricesDTO.EndDate = (catalog.PricesReadjustment != null) ? catalog.PricesReadjustment.FechaFinReajuste : null;

            List<CatalogAssignedProductsDTO> listaAssign = new List<CatalogAssignedProductsDTO>();
            if (catalog.ServiciosVinculados != null)
            {
                foreach (CatalogAssignedProductsEntity oEntity in catalog.ServiciosVinculados)
                {
                    CatalogAssignedProductsDTO assignedDTO = new CatalogAssignedProductsDTO()
                    {
                        ProductCode = (oEntity.CoreProductCatalogServicios != null) ? oEntity.CoreProductCatalogServicios.Codigo : null,
                        Price = oEntity.Precio
                    };
                    listaAssign.Add(assignedDTO);
                }
            }

            List<string> listaCompanies = new List<string>();
            if (catalog.EntidadesVinculadas != null)
            {
                foreach (CompanyEntity oEntity in catalog.EntidadesVinculadas)
                {
                    listaCompanies.Add(oEntity.Codigo);
                }
            }

            CatalogDTO dto = new CatalogDTO()
            {
                Code = catalog.Codigo,
                Name = catalog.Nombre,
                CurrencyCode = (catalog.Monedas != null) ? catalog.Monedas.Moneda : "",
                Description = catalog.Descripcion,
                StartDate = (DateTime)catalog.FechaInicioVigencia,
                EndDate = (DateTime)catalog.FechaFinVigencia,
                CatalogTypeCode = (catalog.CoreProductCatalogTipos != null) ? catalog.CoreProductCatalogTipos.Codigo : "",
                PricesReadjustment = pricesDTO,
                SupplierCompanyCode = (catalog.Entidades != null) ? catalog.Entidades.Codigo : "",
                LifecycleStatusCode = (catalog.CoreProductCatalogEstadosGlobales != null) ? catalog.CoreProductCatalogEstadosGlobales.Codigo : "",
                LinkedProducts = listaAssign,
                CreationDate = catalog.FechaCreacion,
                LastModificationDate = catalog.FechaUltimaModificacion,
                CreationUserEmail = (catalog.UsuariosCreador != null) ? catalog.UsuariosCreador.EMail : null,
                LastModificationUserEmail = (catalog.UsuariosModificador != null) ? catalog.UsuariosModificador.EMail : null,
                LinkedCompanies = listaCompanies
            };
            return Task.FromResult(dto);
        }
    }
}