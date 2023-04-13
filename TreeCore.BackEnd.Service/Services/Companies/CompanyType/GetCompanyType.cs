using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Service.Mappers.Companies;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.Language.Extensions;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Companies
{
    public class GetCompanyType : GetObjectService<CompanyTypeDTO, CompanyTypeEntity, CompanyTypeDTOMapper>
    {

        public GetCompanyType(GetDependencies<CompanyTypeDTO, CompanyTypeEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {
        }
        


    }
}
