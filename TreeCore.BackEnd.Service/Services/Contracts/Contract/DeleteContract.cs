using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Contracts
{

    public class DeleteContract: DeleteObjectService<ContractDTO, ContractEntity,ContractDTOMapper>
    {
        GetDependencies<ContractDTO, ContractEntity> _getDependencies;
        public DeleteContract(DeleteDependencies<ContractEntity> dependencies, GetDependencies<ContractDTO, ContractEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ContractDTO>> Delete(string sCode, int Client)
        {
            var ContractIdentty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<ContractEntity> contract = (ContractIdentty == null || ContractIdentty.AlquilerID == null ?
                Result.Failure<ContractEntity>(Error.Create(_errorTraduccion.NotFound))
                : ContractIdentty);
            if (contract.Success)
            {
                var iResult = await DeleteItem(contract.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ContractDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await contract.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
