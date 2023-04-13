using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Mappers.ProductCatalog;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{
    public class GetCatalog : GetObjectService<CatalogDTO, CatalogEntity, CatalogDTOMapper>
    {
        GetDependencies<CatalogAssignedProductsDTO, CatalogAssignedProductsEntity> _getDetailsDependencies;

        public GetCatalog(GetDependencies<CatalogDTO, CatalogEntity> getDependencies,
            GetDependencies<CatalogAssignedProductsDTO, CatalogAssignedProductsEntity> getDetailsDependencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, getDependencies) 
        {
            _getDetailsDependencies = getDetailsDependencies;
        }
    }
}

