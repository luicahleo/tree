using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.DTO.Contracts;


namespace TreeCore.BackEnd.Service.Mappers
{
    public class ContractGroupDTOMapper : BaseMapper<ContractGroupDTO, ContractGroupEntity>
    {
        public override Task<ContractGroupDTO> Map(ContractGroupEntity contractGroup)
        {
            ContractGroupDTO dto = new ContractGroupDTO()
            {
                Active = contractGroup.Activo,
                Code = contractGroup.codigo,
                Default = contractGroup.Defecto,
                Description = contractGroup.Descripcion,
                Name = contractGroup.TipoContratacion

            };
            return Task.FromResult(dto);
            
        }
    }
}
