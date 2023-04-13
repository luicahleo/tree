using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class CatalogTypeDTOMapper : BaseMapper<CatalogTypeDTO, CatalogTypeEntity>
    {
        public override Task<CatalogTypeDTO> Map(CatalogTypeEntity oEntity)
        {
            CatalogTypeDTO dto = new CatalogTypeDTO()
            {
                Active = oEntity.Activo,
                Code = oEntity.Codigo,
                Default = oEntity.Defecto,
                Description = oEntity.Descripcion,
                Name = oEntity.Nombre,
                IsOffering = oEntity.EsVenta,
                IsPurchasing = oEntity.EsCompra
            };
            return Task.FromResult(dto);
        }
    }
}
