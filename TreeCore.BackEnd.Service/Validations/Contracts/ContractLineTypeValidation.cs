using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class ContractLineTypeValidation : BasicValidation<ContractLineTypeDTO, ContractLineTypeEntity>
    {
        public override Result<ContractLineTypeEntity> ValidateEntity(ContractLineTypeEntity contractLineType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (contractLineType.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractLineType.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractLineType.AlquilerConcepto.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ContractLineTypeEntity>(listaErrores.ToImmutableArray())
                : contractLineType;
        }

        public override Result<ContractLineTypeDTO> ValidateDTO(ContractLineTypeDTO contractLineType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (contractLineType.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractLineType.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractLineType.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ContractLineTypeDTO>(listaErrores.ToImmutableArray())
                : contractLineType;
        }
    }
}
