using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{
    public class DeleteCatalogLifecycleStatus : DeleteObjectService<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity, CatalogLifecycleStatusDTOMapper>
    {
        GetDependencies<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity> _getDependencies;
        public DeleteCatalogLifecycleStatus(DeleteDependencies<CatalogLifecycleStatusEntity> dependencies, 
            GetDependencies<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<CatalogLifecycleStatusDTO>> Delete(string code, int client)
        {
            var catalogLifecycleStatusIdentty = await _getDependencies.GetItemByCode(code, client);
            Result<CatalogLifecycleStatusEntity> CatalogLifecycleStatus = (catalogLifecycleStatusIdentty == null || catalogLifecycleStatusIdentty.CoreProductCatalogEstadoGlobalID == null ?
                Result.Failure<CatalogLifecycleStatusEntity>(Error.Create(_errorTraduccion.NotFound))
                : catalogLifecycleStatusIdentty);
            if (CatalogLifecycleStatus.Success)
            {
                if (CatalogLifecycleStatus.Valor.Defecto)
                {
                    return Result.Failure<CatalogLifecycleStatusDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(CatalogLifecycleStatus.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<CatalogLifecycleStatusDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(catalogLifecycleStatusIdentty);
            }
            return await CatalogLifecycleStatus.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
