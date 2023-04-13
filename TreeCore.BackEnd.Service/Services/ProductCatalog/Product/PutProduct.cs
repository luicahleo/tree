using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers.ProductCatalog;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{
    public class PutProduct : PutObjectService<ProductDTO, ProductEntity, ProductDTOMapper>
    {
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;
        private readonly GetDependencies<ProductDTO, ProductEntity> _getProductDependencies;
        private readonly GetDependencies<CompanyDTO, CompanyEntity> _getCompanyDependency;
        private readonly GetDependencies<ProductTypeDTO, ProductTypeEntity> _getProductTypeDependency;

        public PutProduct(PutDependencies<ProductEntity> putDependency, IHttpContextAccessor httpcontextAccessor, 
            GetDependencies<ProductDTO, ProductEntity> getDependencies,
            GetDependencies<CompanyDTO, CompanyEntity> getCompanyDependency, GetDependencies<ProductTypeDTO, ProductTypeEntity> getProductTypeDependency,
            GetDependencies<UserDTO, UserEntity> getUserDependency):
           base(httpcontextAccessor, putDependency, new ProductValidation())
        {
            _getProductDependencies = getDependencies;
            _getCompanyDependency = getCompanyDependency;
            _getUserDependency = getUserDependency;
            _getProductTypeDependency = getProductTypeDependency;
        }

        public override async Task<Result<ProductEntity>> ValidateEntity(ProductDTO product, int Client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            Filter filter;

            ProductTypeEntity productType = await _getProductTypeDependency.GetItemByCode(product.ProductTypeCode, Client);
            if (productType == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeProductType + " " + $"{product.ProductTypeCode}" + " " + _errorTraduccion.NotFound + "."));
            }

            UserEntity user = await _getUserDependency.GetItemByCode(email, Client);
            if (user == null)
            {
                lErrors.Add(Error.Create(_traduccion.Email + " " + $"{email}" + " " + _errorTraduccion.NotFound + "."));
            }

            ProductEntity? prod = await _getProductDependencies.GetItemByCode(code, Client);
            ProductEntity prodFinal = new ProductEntity(prod.CoreProductCatalogServicioID, product.Code, product.Name, productType, product.Amount,
                null, product.Unit, user.UsuarioID, null, null, DateTime.Now, DateTime.Now, user.UsuarioID, null, null, product.Public, product.Description);

            IEnumerable<ProductEntity> linkedProducts = null;
            if (product.LinkedProducts != null && product.LinkedProducts.Count > 0)
            {
                List<Filter> filters = new List<Filter>();
                List<Filter> filtersCodes = new List<Filter>();
                foreach (string codeLinkedProduct in product.LinkedProducts)
                {
                    filtersCodes.Add(new Filter(nameof(ProductDTO.Code).ToLower(), Operators.eq, codeLinkedProduct));
                }
                filters.Add(new Filter(Filter.Types.OR, filtersCodes));

                linkedProducts = await _getProductDependencies.GetList(Client, filters, null, null, -1, -1);
                IEnumerable<string> iEcodes = linkedProducts.Select(lp => lp.Codigo);
                IEnumerable<string> lp = product.LinkedProducts;

                IEnumerable<string> intersect = iEcodes.Union(lp).Except(iEcodes.Intersect(lp));

                if (intersect.Count() > 0)
                {
                    foreach (string scode in intersect.ToList())
                    {
                        lErrors.Add(Error.Create($"{nameof(ProductDTO.Code)} '" + scode + $"' {_errorTraduccion.NotFound}.", null));
                    };
                }

                foreach (ProductEntity lkp in linkedProducts.Where(x => x.EsPack))
                {
                    lErrors.Add(Error.Create($"{_traduccion.ProductWithCode} '{lkp.Codigo}' {_traduccion.IsPack}.", null));
                }
            }

            filter = new Filter(nameof(ProductDTO.Code), Operators.eq, product.Code);
            listFilters.Add(filter);

            Task<IEnumerable<ProductEntity>> listProducts = _getProductDependencies.GetList(Client, listFilters, null, null, -1, -1);
            if (listProducts.Result != null && listProducts.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.Product + " " + $"{product.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<ProductEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return prodFinal;
            }
        }
    }
}

