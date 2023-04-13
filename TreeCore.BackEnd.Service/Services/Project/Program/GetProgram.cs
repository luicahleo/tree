using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Project;

namespace TreeCore.BackEnd.Service.Services.Program
{
     public class GetProgram : GetObjectService<ProgramDTO, ProgramEntity, ProgramDTOMapper>
    {
        public GetProgram(GetDependencies<ProgramDTO, ProgramEntity> getDependencies, IHttpContextAccessor httpcontextAccessor)
            : base(httpcontextAccessor, getDependencies) 
        {}
    }
}
