using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations.Contracts
{

    public class ContractStatusValidation : BasicValidation<ContractStatusDTO, ContractStatusEntity>
    {
        public override Result<ContractStatusEntity> ValidateEntity(ContractStatusEntity contractState, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (contractState.codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractState.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractState.Estado.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ContractStatusEntity>(listaErrores.ToImmutableArray())
                : contractState;
        }

        public override Result<ContractStatusDTO> ValidateDTO(ContractStatusDTO contractState, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (contractState.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractState.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractState.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ContractStatusDTO>(listaErrores.ToImmutableArray())
                : contractState;
        }
    }
}
