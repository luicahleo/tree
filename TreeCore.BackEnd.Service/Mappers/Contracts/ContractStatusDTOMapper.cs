using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.DTO.Contracts;


namespace TreeCore.BackEnd.Service.Mappers
{
    public class ContractStatusDTOMapper : BaseMapper<ContractStatusDTO, ContractStatusEntity>
    {
        public override Task<ContractStatusDTO> Map(ContractStatusEntity contractState)
        {
            ContractStatusDTO dto = new ContractStatusDTO()
            {
                Active = contractState.Activo,
                Code = contractState.codigo,
                Default = contractState.Defecto,
                Description = contractState.Descripcion,
                Name = contractState.Estado,

            };
            return Task.FromResult(dto);
        }
    }
}
