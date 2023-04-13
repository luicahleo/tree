using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Mappers.General
{
    public class TaxIdentificationNumberCategoryDTOMapper : BaseMapper<TaxIdentificationNumberCategoryDTO, TaxIdentificationNumberCategoryEntity>
    {
        public override Task<TaxIdentificationNumberCategoryDTO> Map(TaxIdentificationNumberCategoryEntity oEntity)
        {
            TaxIdentificationNumberCategoryDTO dto = new TaxIdentificationNumberCategoryDTO()
            {
                Active = oEntity.Activo,
                Code = oEntity.Codigo,
                Default = oEntity.Defecto,
                Description = oEntity.Descripcion,
                Name = oEntity.Nombre
            };
            return Task.FromResult(dto);
        }
    }
}
