using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Contracts
{
    public class DeleteContractType : DeleteObjectService<ContractTypeDTO, ContractTypeEntity, ContractTypeDTOMapper>
    {
        GetDependencies<ContractTypeDTO, ContractTypeEntity> _getDependencies;
        public DeleteContractType(DeleteDependencies<ContractTypeEntity> dependencies, GetDependencies<ContractTypeDTO, ContractTypeEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ContractTypeDTO>> Delete(string sCode, int Client)
        {
            var ContractTypeIdentty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<ContractTypeEntity> contractType = (ContractTypeIdentty == null || ContractTypeIdentty.AlquilerTipoContratoID == null ?
                Result.Failure<ContractTypeEntity>(Error.Create(_errorTraduccion.NotFound))
                : ContractTypeIdentty);
            if (contractType.Success)
            {
                if (contractType.Valor.Defecto)
                {
                    return Result.Failure<ContractTypeDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(contractType.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ContractTypeDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await contractType.Async()
                .MapAsync(x => _mapper.Map(x));
        }


    }
}
