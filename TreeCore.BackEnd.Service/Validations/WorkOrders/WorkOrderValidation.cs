using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Translations;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Validations.WorkOrders
{
    public class WorkOrderValidation : BasicValidation<WorkOrderDTO, WorkOrderEntity>
    {
        public override Result<WorkOrderEntity> ValidateEntity(WorkOrderEntity workOrder, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (workOrder.Codigo.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (workOrder.FechaFin > workOrder.FechaInicio)
            {
                listaErrores.Add(Error.Create(_traduccion.ValidFromDateVsFirstEndDate));
            }

            return listaErrores.Any() ?
                Result.Failure<WorkOrderEntity>(listaErrores.ToImmutableArray())
                : workOrder;
        }

        public override Result<WorkOrderDTO> ValidateDTO(WorkOrderDTO workOrder, ErrorTranslations _traduccion)
        {
            List<Error> listaErrores = new List<Error>();

            if (workOrder.Code.Length > 50)
            {
                listaErrores.Add(Error.Create(_traduccion.MaxLengthText));
            }

            if (workOrder.EndDate > workOrder.StartDate)
            {
                listaErrores.Add(Error.Create(_traduccion.ValidFromDateVsFirstEndDate));
            }

            return listaErrores.Any() ?
                Result.Failure<WorkOrderDTO>(listaErrores.ToImmutableArray())
                : workOrder;
        }
    }
}
