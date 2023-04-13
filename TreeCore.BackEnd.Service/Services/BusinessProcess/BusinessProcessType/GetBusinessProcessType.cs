using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.BusinessProcess;

namespace TreeCore.BackEnd.Service.Services.BusinessProcess
{
    public class GetBusinessProcessType : GetObjectService<BusinessProcessTypeDTO, BusinessProcessTypeEntity, BusinessProcessTypeDTOMapper>
    {
        public GetBusinessProcessType(GetDependencies<BusinessProcessTypeDTO, BusinessProcessTypeEntity> getDependencies, IHttpContextAccessor httpcontextAccessor):base(httpcontextAccessor, getDependencies)
        {

        }
    }
}
