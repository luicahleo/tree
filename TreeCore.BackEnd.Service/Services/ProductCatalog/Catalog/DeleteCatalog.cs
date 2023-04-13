using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{
    public class DeleteCatalog : DeleteObjectService<CatalogDTO, CatalogEntity, CatalogDTOMapper>
    {
        GetDependencies<CatalogDTO, CatalogEntity> _getDependencies;

        public DeleteCatalog(DeleteDependencies<CatalogEntity> dependencies, GetDependencies<CatalogDTO, CatalogEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<CatalogDTO>> Delete(string code, int Client)
        {
            var catalogIdentty = await _getDependencies.GetItemByCode(code, Client);
            Result<CatalogEntity> catalog = (catalogIdentty == null || catalogIdentty.CoreProductCatalogID == null ?
                Result.Failure<CatalogEntity>(Error.Create(_errorTraduccion.NotFound))
                : catalogIdentty);
            if (catalog.Success)
            {
                var iResult = await DeleteItem(catalog.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<CatalogDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(catalogIdentty);
            }
            return await catalog.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
