using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Service.Mappers.BusinessProcess;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.BusinessProcess;

namespace TreeCore.BackEnd.Service.Services.BusinessProcess
{
    public class GetBusinessProcess : GetObjectService<BusinessProcessDTO, BusinessProcessEntity, BusinessProcessDTOMapper>
    {
        public GetBusinessProcess(GetDependencies<BusinessProcessDTO, BusinessProcessEntity> getDependencies, IHttpContextAccessor httpcontextAccessor)
            : base(httpcontextAccessor, getDependencies)
        {

        }

    }
}

