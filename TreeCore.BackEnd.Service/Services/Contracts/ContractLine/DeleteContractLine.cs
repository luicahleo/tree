using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Contracts
{

    public class DeleteContractLine: DeleteObjectService<ContractLineDTO, ContractLineEntity,ContractLineDTOMapper>
    {
        GetDependencies<ContractLineDTO, ContractLineEntity> _getDependencies;
        public DeleteContractLine(DeleteDependencies<ContractLineEntity> dependencies, GetDependencies<ContractLineDTO, ContractLineEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ContractLineDTO>> Delete(string sCode, int Client)
        {
            var ContractLineIdentty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<ContractLineEntity> ContractLine = (ContractLineIdentty == null || ContractLineIdentty.AlquilerDetalleID == null ?
                Result.Failure<ContractLineEntity>(Error.Create(_errorTraduccion.NotFound))
                : ContractLineIdentty);
            if (ContractLine.Success)
            {
                var iResult = await DeleteItem(ContractLine.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ContractLineDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await ContractLine.Async()
                .MapAsync(x => _mapper.Map(x));
        }
    }
}
