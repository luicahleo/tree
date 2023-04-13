using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class WorkFlowStatusGroupValidation : BasicValidation<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity>
    {
        public override Result<WorkFlowStatusGroupEntity> ValidateEntity(WorkFlowStatusGroupEntity statusGroup, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (statusGroup.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (statusGroup.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (statusGroup.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<WorkFlowStatusGroupEntity>(listaErrores.ToImmutableArray())
                : statusGroup;
        }

        public override Result<WorkFlowStatusGroupDTO> ValidateDTO(WorkFlowStatusGroupDTO statusGroup, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (statusGroup.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (statusGroup.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (statusGroup.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<WorkFlowStatusGroupDTO>(listaErrores.ToImmutableArray())
                : statusGroup;
        }
    }
}
