using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Mappers.General
{
    public class TaxpayerTypeDTOMapper : BaseMapper<TaxpayerTypeDTO, TaxpayerTypeEntity>
    {
        public override Task<TaxpayerTypeDTO> Map(TaxpayerTypeEntity oEntity)
        {
            TaxpayerTypeDTO dto = new TaxpayerTypeDTO()
            {
                Active = oEntity.Activo,
                Code = oEntity.Codigo,
                Default = oEntity.Defecto,
                Description = oEntity.Descripcion,
                Name = oEntity.TipoContribuyente
            };
            return Task.FromResult(dto);
        }
    }
}
