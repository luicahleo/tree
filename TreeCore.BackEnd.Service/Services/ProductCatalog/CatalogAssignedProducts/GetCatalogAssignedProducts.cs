using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers.ProductCatalog;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{
    public class GetCatalogAssignedProducts : GetObjectService<CatalogAssignedProductsDTO, CatalogAssignedProductsEntity, CatalogAssignedProductsDTOMapper>
    {
        public GetCatalogAssignedProducts(GetDependencies<CatalogAssignedProductsDTO, CatalogAssignedProductsEntity> getDependencies, IHttpContextAccessor httpcontextAccessor)
            : base(httpcontextAccessor, getDependencies) 
        {}
    }
}
