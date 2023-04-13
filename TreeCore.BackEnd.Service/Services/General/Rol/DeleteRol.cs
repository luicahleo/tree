using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.General
{

    public class DeleteRol : DeleteObjectService<RolDTO, RolEntity, RolDTOMapper>
    {
        GetDependencies<RolDTO, RolEntity> _getDependencies;
        public DeleteRol(DeleteDependencies<RolEntity> dependencies, GetDependencies<RolDTO, RolEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<RolDTO>> Delete(string sCode, int Client)
        {
            var RolIdenty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<RolEntity> Rol = (RolIdenty == null || RolIdenty.RolID == null ?
                Result.Failure<RolEntity>(Error.Create(_errorTraduccion.NotFound))
                : RolIdenty);
            if (Rol.Success)
            {
                var iResult = await DeleteItem(Rol.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<RolDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await Rol.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
