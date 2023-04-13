using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Contracts
{

    public class DeleteFunctionalArea : DeleteObjectService<FunctionalAreaDTO, FunctionalAreaEntity,FuncionalAreaDTOMapper>
    {
        GetDependencies<FunctionalAreaDTO, FunctionalAreaEntity> _getDependencies;
        public DeleteFunctionalArea(DeleteDependencies<FunctionalAreaEntity> dependencies, GetDependencies<FunctionalAreaDTO, FunctionalAreaEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<FunctionalAreaDTO>> Delete(string sCode, int Client)
        {
            var FunctionalAreaIdentty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<FunctionalAreaEntity> functionalArea = (FunctionalAreaIdentty == null || FunctionalAreaIdentty.AreaFuncionalID == null ?
                Result.Failure<FunctionalAreaEntity>(Error.Create(_errorTraduccion.NotFound))
                : FunctionalAreaIdentty);
            if (functionalArea.Success)
            {
                if (functionalArea.Valor.Defecto)
                {
                    return Result.Failure<FunctionalAreaDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(functionalArea.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<FunctionalAreaDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await functionalArea.Async()
                .MapAsync(x =>_mapper.Map(x));
        }

    }
}
