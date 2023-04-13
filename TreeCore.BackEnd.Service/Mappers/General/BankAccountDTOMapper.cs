using System;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ValueObject;

namespace TreeCore.BackEnd.Service.Mappers
{
    public class BankAccountDTOMapper : BaseMapper<BankAccountDTO, BankAccountEntity>
    {
        public override Task<BankAccountDTO> Map(BankAccountEntity bankAccount)
        {
            BankAccountDTO dto = new BankAccountDTO()
            {
                Code = bankAccount.Codigo,
                IBAN = bankAccount.IBAN,
                SWIFT = bankAccount.SWIFT,
                Description = bankAccount.Descripcion,
                BankCode = (bankAccount.Bank != null) ? bankAccount.Bank.CodigoBanco : ""
            };
            return Task.FromResult(dto);
        }
    }
}