using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class ProductTypeDTOMapper : BaseMapper<ProductTypeDTO, ProductTypeEntity>
    {
        public override Task<ProductTypeDTO> Map(ProductTypeEntity ProductType)
        {
            ProductTypeDTO dto = new ProductTypeDTO()
            {
                Active = ProductType.Activo,
                Code = ProductType.Codigo,
                Default = ProductType.Defecto,
                Description = ProductType.Descripcion,
                Name = ProductType.Nombre
            };
            return Task.FromResult(dto);
        }
    }
}
