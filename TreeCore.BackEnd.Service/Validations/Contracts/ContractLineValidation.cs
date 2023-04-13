using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations.Contracts
{

    public class ContractLineValidation : BasicValidation<ContractLineDTO, ContractLineEntity>
    {
        public override Result<ContractLineEntity> ValidateEntity(ContractLineEntity contract, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (contract.CodigoDetalle.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            
            
          


            return listaErrores.Any() ?
                Result.Failure<ContractLineEntity>(listaErrores.ToImmutableArray())
                : contract;
        }
      

        public override Result<ContractLineDTO> ValidateDTO(ContractLineDTO contract, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (contract.Code.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (contract.Code.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }


            return listaErrores.Any() ?
                Result.Failure<ContractLineDTO>(listaErrores.ToImmutableArray())
                : contract;
        }
    }
}
