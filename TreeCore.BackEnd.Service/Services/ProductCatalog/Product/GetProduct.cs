using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers.ProductCatalog;
using TreeCore.BackEnd.Service.Services.Companies;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{
    public class GetProduct : GetObjectService<ProductDTO, ProductEntity, ProductDTOMapper>
    {
        GetDependencies<ProductDetailsDTO, ProductEntity> _getDetailsDependencies;
        GetCompany _getCompany;
        GetProductType _getProductType;

        public GetProduct(GetDependencies<ProductDTO, ProductEntity> getDependencies, IHttpContextAccessor httpcontextAccessor,
            GetDependencies<ProductDetailsDTO, ProductEntity> getDetailsDependencies, GetCompany getCompany, GetProductType getProductType) :
            base(httpcontextAccessor, getDependencies)
        {
            _getDetailsDependencies = getDetailsDependencies;
            _getCompany = getCompany;
            _getProductType = getProductType;
        }

        public async Task<Result<ProductDetailsDTO>> GetItemDetailsByCode(string code, int Client)
        {
            var ProductIdentty = await _getDetailsDependencies.GetItemByCode(code, Client);
            Result<ProductEntity> Product = (ProductIdentty == null || ProductIdentty.CoreProductCatalogServicioID == null ?
                Result.Failure<ProductEntity>(Error.Create(_traduccion.NotFound))
                : ProductIdentty);
            
            return await Product.Async()
                .MapAsync(x => new ProductDetailDTOMapper().Map(x));
        }
    }
}

