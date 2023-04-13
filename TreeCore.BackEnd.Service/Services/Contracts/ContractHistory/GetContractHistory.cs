using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.Language.Extensions;
using TreeCore.Shared.ROP;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Mappers.Contracts;
using Microsoft.AspNetCore.Http;

namespace TreeCore.BackEnd.Service.Services.Contracts.ContractHistory
{

    public class GetContractHistory : GetObjectService<ContractHistoryDTO, ContractHistoryEntity, ContractHistoryDTOMapper>
    {
        public GetContractHistory(GetDependencies<ContractHistoryDTO, ContractHistoryEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {

        }


    }
}
