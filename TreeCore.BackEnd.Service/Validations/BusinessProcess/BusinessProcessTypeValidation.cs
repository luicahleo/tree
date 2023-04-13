using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.BusinessProcess;
using TreeCore.Shared.ROP;


namespace TreeCore.BackEnd.Service.Validations.BusinessProcess
{
    class BusinessProcessTypeValidation : BasicValidation<BusinessProcessTypeDTO, BusinessProcessTypeEntity>
    {
        public override Result<BusinessProcessTypeEntity> ValidateEntity(BusinessProcessTypeEntity businessProcessType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (businessProcessType.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (businessProcessType.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (businessProcessType.Descripcion.Length > 400)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<BusinessProcessTypeEntity>(listaErrores.ToImmutableArray())
                : businessProcessType;
        }

        public override Result<BusinessProcessTypeDTO> ValidateDTO(BusinessProcessTypeDTO businessProcessType, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (businessProcessType.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (businessProcessType.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (businessProcessType.Description.Length > 400)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
           
            return listaErrores.Any() ?
                Result.Failure<BusinessProcessTypeDTO>(listaErrores.ToImmutableArray())
                : businessProcessType;
        }

    }
}
