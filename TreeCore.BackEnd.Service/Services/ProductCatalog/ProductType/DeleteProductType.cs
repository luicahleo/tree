using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{
    public class DeleteProductType : DeleteObjectService<ProductTypeDTO, ProductTypeEntity, ProductTypeDTOMapper>
    {
        GetDependencies<ProductTypeDTO, ProductTypeEntity> _getDependencies;
        public DeleteProductType(DeleteDependencies<ProductTypeEntity> dependencies, GetDependencies<ProductTypeDTO, ProductTypeEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ProductTypeDTO>> Delete(string code, int client)
        {
            var ProductTypeIdentty = await _getDependencies.GetItemByCode(code, client);
            Result<ProductTypeEntity> ProductType = (ProductTypeIdentty == null || ProductTypeIdentty.CoreProductCatalogServicioTipoID == null ?
                Result.Failure<ProductTypeEntity>(Error.Create(_errorTraduccion.NotFound))
                : ProductTypeIdentty);
            if (ProductType.Success)
            {
                if (ProductType.Valor.Defecto)
                {
                    return Result.Failure<ProductTypeDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(ProductType.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ProductTypeDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(ProductTypeIdentty);
            }
            return await ProductType.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
