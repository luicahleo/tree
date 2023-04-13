using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers.ProductCatalog;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{
    public class DeleteProduct : DeleteObjectService<ProductDTO, ProductEntity, ProductDTOMapper>
    {
        GetDependencies<ProductDTO, ProductEntity> _getDependencies;

        public DeleteProduct(DeleteDependencies<ProductEntity> dependencies, GetDependencies<ProductDTO, ProductEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ProductDTO>> Delete (string code, int Client)
        {
            var ProductIdentty = await _getDependencies.GetItemByCode(code, Client);
            Result<ProductEntity> product = (ProductIdentty == null || ProductIdentty.CoreProductCatalogServicioID == null ?
                Result.Failure<ProductEntity>(Error.Create(_errorTraduccion.NotFound))
                : ProductIdentty);
            if (product.Success)
            {
                var iResult = await DeleteItem(product.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ProductDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(ProductIdentty);
            }
            return await product.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}

