using Microsoft.AspNetCore.Http;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.Service.Services.General
{

    public class GetUser : GetObjectService<UserDTO, UserEntity, UserDTOMapper>
    {
        public GetUser(GetDependencies<UserDTO, UserEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {

        }
    }
}
