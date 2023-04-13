using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations
{
    public class WorkOrderLifecycleStatusValidation : BasicValidation<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity>
    {
        public override Result<WorkOrderLifecycleStatusEntity> ValidateEntity(WorkOrderLifecycleStatusEntity trackingStatus, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (trackingStatus.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (trackingStatus.Descripcion.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (trackingStatus.Nombre.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            if (trackingStatus.Color.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<WorkOrderLifecycleStatusEntity>(listaErrores.ToImmutableArray())
                : trackingStatus;
        }

        public override Result<WorkOrderLifecycleStatusDTO> ValidateDTO(WorkOrderLifecycleStatusDTO trackingStatus, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (trackingStatus.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (trackingStatus.Description.Length > 200)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (trackingStatus.Name.Length > 100)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<WorkOrderLifecycleStatusDTO>(listaErrores.ToImmutableArray())
                : trackingStatus;
        }
    }
}
