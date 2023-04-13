using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Model.ValueObject;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.ValueObject;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{
    public class PostCatalog : PostObjectService<CatalogDTO, CatalogEntity, CatalogDTOMapper>
    {
        private readonly GetDependencies<CatalogDTO, CatalogEntity> _getDependency;

        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;
        private readonly GetDependencies<CurrencyDTO, CurrencyEntity> _getCurrencyDependency;
        private readonly GetDependencies<CatalogTypeDTO, CatalogTypeEntity> _getCatalogTypeDependency;
        private readonly GetDependencies<CompanyDTO, CompanyEntity> _getCompanyDependency;
        private readonly GetDependencies<InflationDTO, InflationEntity> _getInflationDependency;
        private readonly GetDependencies<ProductDTO, ProductEntity> _getProductDependency;
        private readonly GetDependencies<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity> _getLifecycleStatusDependency;

        public PostCatalog(PostDependencies<CatalogEntity> postDependency,
            GetDependencies<CatalogDTO, CatalogEntity> getDependency, IHttpContextAccessor httpcontextAccessor,
            GetDependencies<CurrencyDTO, CurrencyEntity> getCurrencyDependency, GetDependencies<CatalogTypeDTO, CatalogTypeEntity> getCatalogTypeDependency,
            GetDependencies<InflationDTO, InflationEntity> getInflationDependency, GetDependencies<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity> getLifecycleStatusDependency,
            GetDependencies<ProductDTO, ProductEntity> getProductDependency, GetDependencies<UserDTO, UserEntity> getUserDependency,
            GetDependencies<CompanyDTO, CompanyEntity> getCompanyDependency) :
            base(httpcontextAccessor, postDependency, new CatalogValidation())
        {
            _getDependency = getDependency;

            _getCurrencyDependency = getCurrencyDependency;
            _getInflationDependency = getInflationDependency;
            _getCatalogTypeDependency = getCatalogTypeDependency;
            _getProductDependency = getProductDependency;
            _getUserDependency = getUserDependency;
            _getCompanyDependency = getCompanyDependency;
            _getLifecycleStatusDependency = getLifecycleStatusDependency;
        }

        public override async Task<Result<CatalogEntity>> ValidateEntity(CatalogDTO catalog, int client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Error> lErrorsReadjustment = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> filtersCompany = new List<Filter>();
            List<Filter> filtersFinalCompany = new List<Filter>();
            InflationEntity inflation = null;
            CompanyEntity company = null;
            Filter filter;

            string tipo;
            float? fixedAmount;
            float? fixedPercentage;
            float? frequency;
            DateTime? startDate;
            DateTime? nextDate;
            DateTime? endDate;

            CurrencyEntity currency = await _getCurrencyDependency.GetItemByCode(catalog.CurrencyCode, client);
            if (currency == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeCurrency + " " + $"{catalog.CurrencyCode}" + " " + _errorTraduccion.NotFound + "."));
            }

            CatalogTypeEntity catalogType = await _getCatalogTypeDependency.GetItemByCode(catalog.CatalogTypeCode, client);
            if (catalogType == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeCatalogType + " " + $"{catalog.CatalogTypeCode}" + " " + _errorTraduccion.NotFound + "."));
            }

            UserEntity user = await _getUserDependency.GetItemByCode(email, client);
            if (user == null)
            {
                lErrors.Add(Error.Create(_traduccion.Email + " " + $"{email}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                catalog.CreationDate = DateTime.Now;
                catalog.LastModificationDate = DateTime.Now;
                catalog.CreationUserEmail = user.EMail;
                catalog.LastModificationUserEmail = user.EMail;
            }

            CatalogLifecycleStatusEntity lifecycleStatus = await _getLifecycleStatusDependency.GetItemByCode(catalog.LifecycleStatusCode, client);
            if (lifecycleStatus == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeCatalogLifecycleStatus + " " + $"{catalog.LifecycleStatusCode}" + " " + _errorTraduccion.NotFound + "."));
            }

            filtersCompany.Add(new Filter(nameof(CompanyDTO.Code).ToLower(), Operators.eq, catalog.SupplierCompanyCode));
            filtersCompany.Add(new Filter(nameof(CompanyDTO.Supplier).ToLower(), Operators.eq, true));
            filtersFinalCompany.Add(new Filter(Filter.Types.AND, filtersCompany));

            IEnumerable<CompanyEntity> listCompany = _getCompanyDependency.GetList(client, filtersFinalCompany, null, null, -1, -1).Result;
            if (listCompany.Count() == 0)
            {
                company = null;
                lErrors.Add(Error.Create(_traduccion.CodeCompany + " " + $"{catalog.SupplierCompanyCode}" + " " + _errorTraduccion.IsnotSupplier + "."));
            }
            else
            {
                company = listCompany.First();
            }

            if (catalog.PricesReadjustment != null)
            {
                inflation = await _getInflationDependency.GetItemByCode(catalog.PricesReadjustment.CodeInflation, client);
                tipo = catalog.PricesReadjustment.Type;
                fixedAmount = catalog.PricesReadjustment.FixedAmount;
                fixedPercentage = catalog.PricesReadjustment.FixedPercentage;
                frequency = catalog.PricesReadjustment.Frequency;
                startDate = catalog.PricesReadjustment.StartDate;
                nextDate = catalog.PricesReadjustment.NextDate;
                endDate = catalog.PricesReadjustment.EndDate;

                PriceReadjustmentDTO.Readjustment pType = PriceReadjustmentDTO.Readjustment.GetByCode<PriceReadjustmentDTO.Readjustment>(tipo);
                PriceReadjustmentDTO prices = new PriceReadjustmentDTO();
                prices.Type = (pType != null) ? pType.Name : "null";
                prices.CodeInflation = (inflation != null) ? inflation.Codigo : null;
                prices.FixedAmount = fixedAmount;
                prices.FixedPercentage = fixedPercentage;
                prices.Frequency = frequency;
                prices.StartDate = startDate;
                prices.NextDate = nextDate;
                prices.EndDate = endDate;
                lErrorsReadjustment = pType.ValidateObject(prices);

                if (lErrorsReadjustment.Count > 0)
                {
                    foreach (Error err in lErrorsReadjustment)
                    {
                        lErrors.Add(err);
                    }
                }
            }
            else
            {
                tipo = null;
                fixedAmount = null;
                fixedPercentage = null;
                frequency = null;
                startDate = null;
                nextDate = null;
                endDate = null;
                inflation = null;
            }

            IEnumerable<CatalogAssignedProductsEntity> linkedProducts = new CatalogAssignedProductsEntity[] { null };
            IEnumerable<ProductEntity> Products = null;
            if (catalog.LinkedProducts != null && catalog.LinkedProducts.Count > 0)
            {
                List<string> listaP = new List<string>();
                List<float> listaPrices = new List<float>();
                List<Filter> filters = new List<Filter>();
                List<Filter> filtersCodes = new List<Filter>();
                foreach (CatalogAssignedProductsDTO link in catalog.LinkedProducts)
                {
                    filtersCodes.Add(new Filter(nameof(CatalogDTO.Code).ToLower(), Operators.eq, link.ProductCode, Filter.Types.OR, null));
                    listaP.Add(link.ProductCode);
                    listaPrices.Add(link.Price);
                }
                //filters.Add(new Filter(Filter.Types.OR, filtersCodes));

                Products = await _getProductDependency.GetList(client, filtersCodes, null, null, -1, -1);
                IEnumerable<string> iEcodes = Products.Select(lp => lp.Codigo);
                IEnumerable<string> lp = listaP;

                IEnumerable<string> intersect = iEcodes.Union(lp).Except(iEcodes.Intersect(lp));

                if (intersect.Count() > 0)
                {
                    foreach (string scode in intersect.ToList())
                    {
                        lErrors.Add(Error.Create($"{nameof(CatalogDTO.Code)} '" + scode + $"' {_errorTraduccion.NotFound}.", null));
                    };
                }

                if (Products != null && Products.Count() > 0)
                {
                    int i = 0;
                    foreach (ProductEntity prod in Products)
                    {
                        var linkProd = new CatalogAssignedProductsEntity(null, listaPrices[i], prod, null);
                        linkedProducts = linkedProducts.Append(linkProd);
                        i++;
                    }
                }
            }

            IEnumerable<CompanyEntity> linkedCompanies = null;
            if (catalogType != null && catalogType.EsVenta && 
                catalog.LinkedCompanies != null && catalog.LinkedCompanies.Count > 0)
            {
                List<Filter> filtersCompanies = new List<Filter>();
                List<Filter> filtersCodesCompany = new List<Filter>();

                foreach (string codeLinkedCompany in catalog.LinkedCompanies)
                {
                    filtersCodesCompany = new List<Filter>();
                    filtersCodesCompany.Add(new Filter(nameof(CompanyDTO.Code).ToLower(), Operators.eq, codeLinkedCompany, Filter.Types.AND, null));
                    filtersCodesCompany.Add(new Filter(nameof(CompanyDTO.Customer).ToLower(), Operators.eq, true, Filter.Types.AND, null));
                    filtersCompanies.Add(new Filter(Filter.Types.OR, filtersCodesCompany));
                }
                

                linkedCompanies = await _getCompanyDependency.GetList(client, filtersCompanies, null, null, -1, -1);
                IEnumerable<string> iEcodesCompany = linkedCompanies.Select(lp => lp.Codigo);
                IEnumerable<string> lpCompany = catalog.LinkedCompanies;

                IEnumerable<string> intersectCompany = iEcodesCompany.Union(lpCompany).Except(iEcodesCompany.Intersect(lpCompany));

                if (intersectCompany.Count() > 0)
                {
                    foreach (string scodeCompany in intersectCompany.ToList())
                    {
                        lErrors.Add(Error.Create($"{nameof(CompanyDTO.Code)} '" + scodeCompany + $"' {_errorTraduccion.NotFound}.", null));
                    };
                }
            }

            PriceReadjustment pRead = new PriceReadjustment(tipo, inflation, fixedAmount, fixedPercentage, frequency, startDate, endDate, nextDate);

            CatalogEntity catalogEntity = new CatalogEntity(null, catalog.Code, catalog.Name, client, currency, catalogType, catalog.StartDate, 
                catalog.EndDate, DateTime.Now, DateTime.Now, user, user, catalog.Description, pRead, lifecycleStatus, company, 
                linkedProducts, linkedCompanies);
            filter = new Filter(nameof(CatalogDTO.Code), Operators.eq, catalog.Code);
            listFilters.Add(filter);

            Task<IEnumerable<CatalogEntity>> listCatalogs = _getDependency.GetList(client, listFilters, null, null, -1, -1);
            if (listCatalogs.Result != null && listCatalogs.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodeCatalog + " " + $"{catalog.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<CatalogEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return catalogEntity;
            }
        }
    }
}
