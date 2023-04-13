using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{
    public class GetCatalogLifecycleStatus : GetObjectService<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity, CatalogLifecycleStatusDTOMapper>
    {
        public GetCatalogLifecycleStatus(GetDependencies<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity> getDependencies, 
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {

        }
    }
}

