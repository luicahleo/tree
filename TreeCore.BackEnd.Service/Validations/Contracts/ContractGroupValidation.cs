using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations.Contracts
{

    public class ContractGroupValidation : BasicValidation<ContractGroupDTO, ContractGroupEntity>
    {
        public override Result<ContractGroupEntity> ValidateEntity(ContractGroupEntity contractGroup, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (contractGroup.codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractGroup.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractGroup.TipoContratacion.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ContractGroupEntity>(listaErrores.ToImmutableArray())
                : contractGroup;
        }

        public override Result<ContractGroupDTO> ValidateDTO(ContractGroupDTO contractGroup, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (contractGroup.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractGroup.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (contractGroup.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<ContractGroupDTO>(listaErrores.ToImmutableArray())
                : contractGroup;
        }
    }
}
