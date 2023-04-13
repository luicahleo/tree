using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Contracts
{

    public class DeleteContractStatus : DeleteObjectService<ContractStatusDTO, ContractStatusEntity, ContractStatusDTOMapper>
    {
        GetDependencies<ContractStatusDTO, ContractStatusEntity> _getDependencies;
        public DeleteContractStatus(DeleteDependencies<ContractStatusEntity> dependencies, GetDependencies<ContractStatusDTO, ContractStatusEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ContractStatusDTO>> Delete(string sCode, int Client)
        {
            var ContractStateIdentty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<ContractStatusEntity> contractState = (ContractStateIdentty == null || ContractStateIdentty.AlquilerEstadoID == null ?
                Result.Failure<ContractStatusEntity>(Error.Create(_errorTraduccion.NotFound))
                : ContractStateIdentty);
            if (contractState.Success)
            {
                if (contractState.Valor.Defecto)
                {
                    return Result.Failure<ContractStatusDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(contractState.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ContractStatusDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await contractState.Async()
                .MapAsync(x => _mapper.Map(x));
        }


    }
}
