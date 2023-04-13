using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations.WorkOrders
{
    public class WorkOrderTrackingStatusValidation : BasicValidation<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity>
    {
        public override Result<WorkOrderTrackingStatusEntity> ValidateEntity(WorkOrderTrackingStatusEntity workOrder, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (workOrder.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }
            return listaErrores.Any() ?
                Result.Failure<WorkOrderTrackingStatusEntity>(listaErrores.ToImmutableArray())
                : workOrder;
        }

        public override Result<WorkOrderTrackingStatusDTO> ValidateDTO(WorkOrderTrackingStatusDTO workOrder, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (workOrder.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            return listaErrores.Any() ?
                Result.Failure<WorkOrderTrackingStatusDTO>(listaErrores.ToImmutableArray())
                : workOrder;
        }
    }
}
