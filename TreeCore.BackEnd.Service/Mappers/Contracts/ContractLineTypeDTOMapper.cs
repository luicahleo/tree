using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.DTO.Contracts;


namespace TreeCore.BackEnd.Service.Mappers
{
    public class ContractLineTypeDTOMapper : BaseMapper<ContractLineTypeDTO, ContractLineTypeEntity>
    {
        public override Task<ContractLineTypeDTO> Map(ContractLineTypeEntity contractLineType)
        {
            ContractLineTypeDTO dto = new ContractLineTypeDTO()
            {
                Active = contractLineType.Activo,
                Code = contractLineType.Codigo,
                Default = contractLineType.Defecto,
                Description = contractLineType.Descripcion,
                Name = contractLineType.AlquilerConcepto,
                Single = contractLineType.EsPagoUnico,
                Recurrent = contractLineType.EsAlquilerBase,
                Income = contractLineType.EsCobro
            };
            return Task.FromResult(dto);
        }
    }
}
