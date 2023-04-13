using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.General;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{
    public class GetPaymentMethods : GetObjectService<PaymentMethodsDTO, PaymentMethodsEntity, PaymentMethodsDTOMapper>
    {
        
        PaymentMethodsRepository repo;
        public GetPaymentMethods(
            GetDependencies<PaymentMethodsDTO, PaymentMethodsEntity> getDependencies,
            IHttpContextAccessor httpcontextAccessor):base(httpcontextAccessor, getDependencies)
        {
        }
        public async Task<Result<IEnumerable<PaymentMethodsDTO>>> GetItemByCompany(string entidadID)
        {
            var CompanyIdentty = await repo.GetListbyCompany(int.Parse(entidadID));
            

            return null;
        }

    }
}
