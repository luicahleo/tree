using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class CatalogLifecycleStatusDTOMapper : BaseMapper<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity>
    {
        public override Task<CatalogLifecycleStatusDTO> Map(CatalogLifecycleStatusEntity catalogLifecycleStatus)
        {
            CatalogLifecycleStatusDTO dto = new CatalogLifecycleStatusDTO()
            {
                Active = catalogLifecycleStatus.Activo,
                Code = catalogLifecycleStatus.Codigo,
                Default = catalogLifecycleStatus.Defecto,
                Description = catalogLifecycleStatus.Descripcion,
                Name = catalogLifecycleStatus.Nombre
            };
            return Task.FromResult(dto);
        }
    }
}