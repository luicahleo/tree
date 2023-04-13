﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.Language.Extensions;
using TreeCore.Shared.ROP;
using TreeCore.BackEnd.Service.Mappers;

namespace TreeCore.BackEnd.Service.Services.Contracts
{
    public class GetContractLineType : GetObjectService<ContractLineTypeDTO, ContractLineTypeEntity,ContractLineTypeDTOMapper>
    {
        public GetContractLineType(GetDependencies<ContractLineTypeDTO, ContractLineTypeEntity> getDependencies, IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, getDependencies)
        {

        }

      
    }
}
