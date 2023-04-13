using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Mappers.General
{
    public class PaymentMethodsDTOMapper : BaseMapper<PaymentMethodsDTO, PaymentMethodsEntity>
    {
        public override Task<PaymentMethodsDTO> Map(PaymentMethodsEntity oEntity)
        {
            PaymentMethodsDTO dto = new PaymentMethodsDTO()
            {
                Active = oEntity.Activo,
                Code = oEntity.CodigoMetodoPago,
                Default = oEntity.Defecto,
                Description = oEntity.Descripcion,
                RequiresBankAccount = oEntity.RequiereBanco,
                Name = oEntity.MetodoPago
            };
            return Task.FromResult(dto);
        }
    }
}
