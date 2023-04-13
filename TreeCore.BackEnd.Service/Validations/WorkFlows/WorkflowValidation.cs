using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class WorkflowValidation : BasicValidation<WorkflowDTO, WorkflowEntity>
    {
        public override Result<WorkflowEntity> ValidateEntity (WorkflowEntity workflow, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (workflow.Codigo.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (workflow.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (workflow.Nombre.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<WorkflowEntity>(listaErrores.ToImmutableArray())
                : workflow;
        }

        public override Result<WorkflowDTO> ValidateDTO (WorkflowDTO workflow, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (workflow.Code.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (workflow.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (workflow.Name.Length > 150)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<WorkflowDTO>(listaErrores.ToImmutableArray())
                : workflow;
        }
    }
}

