using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations.Contracts
{

    public class ContractLineTaxesValidation : BasicValidation<ContractLineTaxesDTO, ContractLineTaxesEntity>
    {
        public override Result<ContractLineTaxesEntity> ValidateEntity(ContractLineTaxesEntity contractGroup, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            

            return listaErrores.Any() ?
                Result.Failure<ContractLineTaxesEntity>(listaErrores.ToImmutableArray())
                : contractGroup;
        }

        public override Result<ContractLineTaxesDTO> ValidateDTO(ContractLineTaxesDTO contractGroup, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

           

            return listaErrores.Any() ?
                Result.Failure<ContractLineTaxesDTO>(listaErrores.ToImmutableArray())
                : contractGroup;
        }
    }
}
