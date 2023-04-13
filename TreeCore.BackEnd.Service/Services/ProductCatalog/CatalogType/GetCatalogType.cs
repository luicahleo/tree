using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{
    public class GetCatalogType : GetObjectService<CatalogTypeDTO, CatalogTypeEntity, CatalogTypeDTOMapper>
    {
        public GetCatalogType(GetDependencies<CatalogTypeDTO, CatalogTypeEntity> getDependencies, IHttpContextAccessor httpcontextAccessor):base(httpcontextAccessor, getDependencies)
        {

        }
    }
}
