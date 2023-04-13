using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.DTO.Contracts;
using Newtonsoft.Json;



namespace TreeCore.BackEnd.Service.Mappers
{
    public class ContractHistoryDTOMapper : BaseMapper<ContractHistoryDTO, ContractHistoryEntity>
    {
        public override Task<ContractHistoryDTO> Map(ContractHistoryEntity contractHistory)
        {

            
            ContractDTO aux2 = JsonConvert.DeserializeObject<ContractDTO>(contractHistory.Datos);

            ContractHistoryDTO dto = new ContractHistoryDTO()
            {
                
                
                CreationDate = contractHistory.FechaCreacion,
                Active = contractHistory.Activo,
                Contract = aux2,
                
            };
            return Task.FromResult(dto);

        }
    }
}
