using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Contracts
{
    public class DeleteContractLineType : DeleteObjectService<ContractLineTypeDTO, ContractLineTypeEntity,ContractLineTypeDTOMapper>
    {
        GetDependencies<ContractLineTypeDTO, ContractLineTypeEntity> _getDependencies;
        public DeleteContractLineType(DeleteDependencies<ContractLineTypeEntity> dependencies, GetDependencies<ContractLineTypeDTO, ContractLineTypeEntity> getDepencies, IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, dependencies)
        {
            _getDependencies = getDepencies;
        }

        public override async Task<Result<ContractLineTypeDTO>> Delete(string sCode, int Client)
        {
            var ContractLineTypeIdentty = await _getDependencies.GetItemByCode(sCode, Client);
            Result<ContractLineTypeEntity> contractLineType = (ContractLineTypeIdentty == null || ContractLineTypeIdentty.AlquilerConceptoID == null ?
                Result.Failure<ContractLineTypeEntity>(Error.Create(_errorTraduccion.NotFound))
                : ContractLineTypeIdentty);
            if (contractLineType.Success)
            {
                if (contractLineType.Valor.Defecto)
                {
                    return Result.Failure<ContractLineTypeDTO>(Error.Create(_errorTraduccion.DeleteDefault));
                }
                var iResult = await DeleteItem(contractLineType.Valor);
                if (iResult.Valor == 0)
                {
                    return Result.Failure<ContractLineTypeDTO>(Error.Create(_errorTraduccion.ErrorFK));
                }
                await CommitTransaction(null);
            }
            return await contractLineType.Async()
                .MapAsync(x => _mapper.Map(x));
        }

      
    }
}
