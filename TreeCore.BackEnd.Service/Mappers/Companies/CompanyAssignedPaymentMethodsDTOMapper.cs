using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Mappers.Companies
{
    public class CompanyAssignedPaymentMethodsDTOMapper : BaseMapper<CompanyAssignedPaymentMethodsDTO, CompanyAssignedPaymentMethodsEntity>
    {
        public override Task<CompanyAssignedPaymentMethodsDTO> Map(CompanyAssignedPaymentMethodsEntity CompanyAssignedPaymentMethods)
        {
            CompanyAssignedPaymentMethodsDTO dto = new CompanyAssignedPaymentMethodsDTO()
            {
                Default = CompanyAssignedPaymentMethods.Defecto,
                PaymentMethodCode = CompanyAssignedPaymentMethods.CoreMetodosPagos.CodigoMetodoPago
            };

            
            return Task.FromResult(dto);
        }
    }
}
