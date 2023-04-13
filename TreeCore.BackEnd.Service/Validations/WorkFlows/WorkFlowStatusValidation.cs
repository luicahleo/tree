using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class WorkFlowStatusValidation : BasicValidation<WorkFlowStatusDTO, WorkFlowStatusEntity>
    {
        public override Result<WorkFlowStatusEntity> ValidateEntity(WorkFlowStatusEntity status, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (status.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (status.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (status.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (status.Tiempo< 1)
            {
                listaErrores.Add(Error.Create(_traduccion.RangeInvalid));
            }

            return listaErrores.Any() ?
                Result.Failure<WorkFlowStatusEntity>(listaErrores.ToImmutableArray())
                : status;
        }

        public override Result<WorkFlowStatusDTO> ValidateDTO(WorkFlowStatusDTO status, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (status.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (status.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (status.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<WorkFlowStatusDTO>(listaErrores.ToImmutableArray())
                : status;
        }
    }
}
