using System.Threading.Tasks;

using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.Shared.DTO.BusinessProcess;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class BusinessProcessTypeDTOMapper : BaseMapper<BusinessProcessTypeDTO, BusinessProcessTypeEntity>
    {
        public override Task<BusinessProcessTypeDTO> Map(BusinessProcessTypeEntity oEntity)
        {
            BusinessProcessTypeDTO dto = new BusinessProcessTypeDTO()
            {
                Code = oEntity.Codigo,
                Name = oEntity.Nombre,
                Active = oEntity.Activo,
                Description = oEntity.Descripcion,
                Default = oEntity.Defecto,
            };
            return Task.FromResult(dto);
        }
    }
}
