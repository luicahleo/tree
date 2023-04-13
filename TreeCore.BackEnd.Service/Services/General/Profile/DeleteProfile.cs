using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{

    public class DeleteProfile : DeleteObjectService<ProfileDTO, ProfileEntity, ProfileDTOMapper>
    {
        GetDependencies<ProfileDTO, ProfileEntity> _getDependencies;
        public DeleteProfile(DeleteDependencies<ProfileEntity> dependencies, GetDependencies<ProfileDTO, ProfileEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ProfileDTO>> Delete(string sCode, int Client)
        {
            var ProfileIdenty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<ProfileEntity> Profile = (ProfileIdenty == null || ProfileIdenty.PerfilID == null ?
                Result.Failure<ProfileEntity>(Error.Create(_errorTraduccion.NotFound))
                : ProfileIdenty);
            if (Profile.Success)
            {
                var iResult = await DeleteItem(Profile.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ProfileDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await Profile.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
