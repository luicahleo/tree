using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.DTO.Contracts;


namespace TreeCore.BackEnd.Service.Mappers
{
    public class ContractTypeDTOMapper : BaseMapper<ContractTypeDTO, ContractTypeEntity>
    {
        public override Task<ContractTypeDTO> Map(ContractTypeEntity contractType)
        {
            ContractTypeDTO dto = new ContractTypeDTO()
            {
                Active = contractType.Activo,
                Code = contractType.Codigo,
                Default = contractType.Defecto,
                Description = contractType.Descripcion,
                Name = contractType.TipoContrato
            };
            return Task.FromResult(dto);
        }
    }
}
