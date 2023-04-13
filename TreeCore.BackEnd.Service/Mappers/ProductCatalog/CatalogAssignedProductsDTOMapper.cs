using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.Service.Mappers.ProductCatalog
{
    public class CatalogAssignedProductsDTOMapper : BaseMapper<CatalogAssignedProductsDTO, CatalogAssignedProductsEntity>
    {
        public override Task<CatalogAssignedProductsDTO> Map(CatalogAssignedProductsEntity oEntity)
        {
            CatalogAssignedProductsDTO dto = new CatalogAssignedProductsDTO()
            {
                ProductCode = (oEntity.CoreProductCatalogServicios != null) ? oEntity.CoreProductCatalogServicios.Codigo : null,
                Price = oEntity.Precio
            };
            return Task.FromResult(dto);
        }
    }
}
