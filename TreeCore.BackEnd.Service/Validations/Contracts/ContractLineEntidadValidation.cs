using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations.Contracts
{

    public class ContractLineEntidadValidation : BasicValidation<ContractLineEntidadDTO, ContractLineEntidadEntity>
    {
        public override Result<ContractLineEntidadEntity> ValidateEntity(ContractLineEntidadEntity contractLineEntitad, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();



            return listaErrores.Any() ?
                Result.Failure<ContractLineEntidadEntity>(listaErrores.ToImmutableArray())
                : contractLineEntitad;
        }

        public override Result<ContractLineEntidadDTO> ValidateDTO(ContractLineEntidadDTO contractLineEntitad, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();



            return listaErrores.Any() ?
                Result.Failure<ContractLineEntidadDTO>(listaErrores.ToImmutableArray())
                : contractLineEntitad;
        }
    }
}