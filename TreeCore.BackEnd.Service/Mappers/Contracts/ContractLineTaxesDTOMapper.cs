using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.DTO.Contracts;

namespace TreeCore.BackEnd.Service.Mappers.Contracts
{
    
    public class ContractLineTaxesDTOMapper : BaseMapper<ContractLineTaxesDTO, ContractLineTaxesEntity>
    {
        public override Task<ContractLineTaxesDTO> Map(ContractLineTaxesEntity contractlineTaxes)
        {
            ContractLineTaxesDTO dto = new ContractLineTaxesDTO()
            {
                TaxCode = contractlineTaxes.oImpuesto.Codigo,
               


            };
            return Task.FromResult(dto);

        }
    }
}
