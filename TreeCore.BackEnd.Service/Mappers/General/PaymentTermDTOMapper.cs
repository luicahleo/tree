using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Mappers.General
{
    public class PaymentTermDTOMapper : BaseMapper<PaymentTermDTO, PaymentTermEntity>
    {
        public override Task<PaymentTermDTO> Map(PaymentTermEntity oEntity)
        {
            PaymentTermDTO dto = new PaymentTermDTO()
            {
                Active = oEntity.Activo,
                Code = oEntity.Codigo,
                Default = oEntity.Defecto,
                Description = oEntity.Descripcion,
                Name = oEntity.CondicionPago
            };
            return Task.FromResult(dto);
        }
    }
}
