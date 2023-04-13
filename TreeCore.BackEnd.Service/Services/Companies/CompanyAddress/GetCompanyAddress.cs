using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Service.Mappers.Companies;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Companies;

namespace TreeCore.BackEnd.Service.Services.Companies
{
    public class GetCompanyAddress : GetObjectService<CompanyAddressDTO, CompanyAddressEntity, CompanyAddressDTOMapper>
    {

        public GetCompanyAddress(GetDependencies<CompanyAddressDTO, CompanyAddressEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {
        }
        


    }
}
