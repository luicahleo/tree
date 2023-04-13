
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;
using TreeCore.BackEnd.Service.Mappers;

namespace TreeCore.BackEnd.Service.Services.Contracts
{

    public class DeleteContractGroup: DeleteObjectService<ContractGroupDTO, ContractGroupEntity,ContractGroupDTOMapper>
    {
        GetDependencies<ContractGroupDTO, ContractGroupEntity> _getDependencies;
        public DeleteContractGroup(DeleteDependencies<ContractGroupEntity> dependencies, GetDependencies<ContractGroupDTO, ContractGroupEntity> getDepencies, IHttpContextAccessor httpcontextAccessor):
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ContractGroupDTO>> Delete(string sCode, int client)
        {
            var ContractGroupIdentty = await _getDependencies.GetItemByCode(sCode, client);
            Result<ContractGroupEntity> contractGroup = (ContractGroupIdentty == null || ContractGroupIdentty.AlquilerTipoContratacionID == null ?
                Result.Failure<ContractGroupEntity>(Error.Create(_errorTraduccion.NotFound))
                : ContractGroupIdentty);
            if (contractGroup.Success)
            {
                if (contractGroup.Valor.Defecto)
                {
                    return Result.Failure<ContractGroupDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(contractGroup.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ContractGroupDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await contractGroup.Async()
               .MapAsync(x => _mapper.Map(x));
        }

     
    }
}
