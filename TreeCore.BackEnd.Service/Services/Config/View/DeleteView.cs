using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Config;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.Config;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Config;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Config
{

    public class DeleteView : DeleteObjectService<ViewDTO, ViewEntity, ViewDTOMapper>
    {
        GetDependencies<ViewDTO, ViewEntity> _getDependencies;
        public DeleteView(DeleteDependencies<ViewEntity> dependencies, GetDependencies<ViewDTO, ViewEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ViewDTO>> Delete(string sCode, int Client)
        {
            var ViewIdenty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<ViewEntity> View = (ViewIdenty == null || ViewIdenty.CoreGestionVistaID == null ?
                Result.Failure<ViewEntity>(Error.Create(_errorTraduccion.NotFound))
                : ViewIdenty);
            if (View.Success)
            {
                if (View.Valor.Defecto)
                {
                    return Result.Failure<ViewDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(View.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ViewDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await View.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
