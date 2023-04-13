using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Mappers.Companies;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Companies
{
    public class GetCompanyAssignedPaymentMethods : GetObjectService<CompanyAssignedPaymentMethodsDTO, CompanyAssignedPaymentMethodsEntity, CompanyAssignedPaymentMethodsDTOMapper>
    {
        public GetCompanyAssignedPaymentMethods(GetDependencies<CompanyAssignedPaymentMethodsDTO, CompanyAssignedPaymentMethodsEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {
            
        }

    }
}
