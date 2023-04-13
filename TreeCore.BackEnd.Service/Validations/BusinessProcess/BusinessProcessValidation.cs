using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.BusinessProcess;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class BusinessProcessValidation : BasicValidation<BusinessProcessDTO, BusinessProcessEntity>
    {
        public override Result<BusinessProcessEntity> ValidateEntity(BusinessProcessEntity businessProcess, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (businessProcess.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (businessProcess.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (businessProcess.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<BusinessProcessEntity>(listaErrores.ToImmutableArray())
                : businessProcess;
        }

        public override Result<BusinessProcessDTO> ValidateDTO(BusinessProcessDTO businessProcess, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (businessProcess.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (businessProcess.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (businessProcess.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<BusinessProcessDTO>(listaErrores.ToImmutableArray())
                : businessProcess;
        }
    }
}
