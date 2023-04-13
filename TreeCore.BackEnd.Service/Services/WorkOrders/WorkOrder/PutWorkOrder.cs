using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.Service.Validations.WorkOrders;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrder
{
    public class PutWorkOrder : PutObjectService<WorkOrderDTO, WorkOrderEntity, WorkOrderDTOMapper>
    {
        private readonly GetDependencies<WorkOrderDTO, WorkOrderEntity> _getDependency;

        public PutWorkOrder(PutDependencies<WorkOrderEntity> putDependency,
            GetDependencies<WorkOrderDTO, WorkOrderEntity> getDependency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new WorkOrderValidation())
        {
            _getDependency = getDependency;
        }
        public override async Task<Result<WorkOrderEntity>> ValidateEntity(WorkOrderDTO workOrder, int Client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            Task<IEnumerable<WorkOrderEntity>> listWorkOrders;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            WorkOrderEntity workOrderFinal = null;

            WorkOrderEntity? workO = await _getDependency.GetItemByCode(code, Client);
            if (workO == null)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.WorkOrder + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                workOrderFinal = new WorkOrderEntity(workO.CoreWorkOrderID, workOrder.Code, null, workOrder.StartDate, (DateTime)workOrder.EndDate,
                    0, (DateTime)workOrder.CreationDate, (DateTime)workOrder.LastModificationDate, null, null, null,
                    null, null, null, null, null, null, 0);

                filter = new Filter(nameof(WorkOrderDTO.Code), Operators.eq, workOrder.Code);
                listFilters.Add(filter);

                listWorkOrders = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
                if (listWorkOrders.Result != null && listWorkOrders.Result.ListOrEmpty().Count > 0 &&
                    listWorkOrders.Result.ListOrEmpty()[0].CoreWorkOrderID != workOrderFinal.CoreWorkOrderID)
                {
                    lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.WorkOrder + " " + $"{workOrder.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
                }
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkOrderEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return workOrderFinal;
            }
        }
    }
}
