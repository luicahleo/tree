using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class ContractTypeValidation : BasicValidation<ContractTypeDTO, ContractTypeEntity>
    {
        public override Result<ContractTypeEntity> ValidateEntity(ContractTypeEntity contractType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (contractType.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractType.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractType.TipoContrato.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ContractTypeEntity>(listaErrores.ToImmutableArray())
                : contractType;
        }

        public override Result<ContractTypeDTO> ValidateDTO(ContractTypeDTO contractType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (contractType.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractType.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractType.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ContractTypeDTO>(listaErrores.ToImmutableArray())
                : contractType;
        }
    }
}
