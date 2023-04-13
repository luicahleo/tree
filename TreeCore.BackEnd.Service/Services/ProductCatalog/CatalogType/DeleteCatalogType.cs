using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{

    public class DeleteCatalogType : DeleteObjectService<CatalogTypeDTO, CatalogTypeEntity, CatalogTypeDTOMapper>
    {
        private readonly GetDependencies<CatalogTypeDTO, CatalogTypeEntity> _getDependencies;
        public DeleteCatalogType(DeleteDependencies<CatalogTypeEntity> dependencies, GetDependencies<CatalogTypeDTO, CatalogTypeEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<CatalogTypeDTO>> Delete(string sCode, int client)
        {
            var CatalogTypeIdentty = await _getDependencies.GetItemByCode(sCode, client);
            Result<CatalogTypeEntity> catalogType = (CatalogTypeIdentty == null || CatalogTypeIdentty.CoreProductCatalogTipoID == null ?
                Result.Failure<CatalogTypeEntity>(Error.Create(_errorTraduccion.NotFound))
                : CatalogTypeIdentty);
            if (catalogType.Success)
            {
                if (catalogType.Valor.Defecto)
                {
                    return Result.Failure<CatalogTypeDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(catalogType.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<CatalogTypeDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await catalogType.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
