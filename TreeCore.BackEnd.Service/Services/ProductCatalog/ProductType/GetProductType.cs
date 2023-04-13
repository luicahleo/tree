using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.Service.Services.ProductCatalog
{
    public class GetProductType : GetObjectService<ProductTypeDTO, ProductTypeEntity, ProductTypeDTOMapper>
    {
        public GetProductType(GetDependencies<ProductTypeDTO, ProductTypeEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {

        }
    }
}
