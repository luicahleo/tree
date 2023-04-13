using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrderTrackingStatus;
using TreeCore.BackEnd.Service.Validations.WorkOrders;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrder
{
    public class PostWorkOrder : PostObjectService<WorkOrderDTO, WorkOrderEntity, WorkOrderDTOMapper>
    {
        private int projID;
        private readonly GetDependencies<WorkOrderDTO, WorkOrderEntity> _getDependency;
        private readonly PutDependencies<WorkOrderEntity> _putDependency;
        private readonly PostWorkOrderTrackingStatus _postTrackingStatus;

        public PostWorkOrder(PostDependencies<WorkOrderEntity> postDependency,
            GetDependencies<WorkOrderDTO, WorkOrderEntity> getDependency,
            PutDependencies<WorkOrderEntity> putDependency,
            PostWorkOrderTrackingStatus postTrackingStatus,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new WorkOrderValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
            _postTrackingStatus = postTrackingStatus;
        }

        public virtual async Task<Result<WorkOrderDTO>> Create(WorkOrderDTO oEntidad, int projecID, int clientID, string email, string code = "")
        {
            projID = projecID;

            var wo = await ValidateEntity(oEntidad, clientID, email, code)
                 .Bind(ValidateText)
                 .Bind(Validate)
                 .Bind(SaveItem);

            if (wo.Success)
            {
                WorkOrderTrackingStatusDTO oTracking = new WorkOrderTrackingStatusDTO();
                oTracking.Code = oEntidad.Code + 01;
                oTracking.StatusCode = wo.Value.CoreWorkflows.WorkflowsEstados.ListOrEmpty().FirstOrDefault(x => x.Defecto).Codigo;

                var trackingStatus = await _postTrackingStatus.Create(oTracking, clientID, email, code);

                if (trackingStatus.Success)
                {
                    var woDTO = _mapper.Map(wo.Value).Result;
                    woDTO.LinkedWorkOrderTrakingStatus.ListOrEmpty().Add(trackingStatus.Value);
                    return woDTO;
                }
                else
                {
                    return Result.Failure<WorkOrderDTO>(ImmutableArray.Create(trackingStatus.Errores.ToArray()));
                }
            }

            return _mapper.Map(wo.Value).Result;
        }

        public override async Task<Result<WorkOrderEntity>> ValidateEntity(WorkOrderDTO oEntidad, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            // FALTA AÑADIR LIST ASSETS
            // FALTA AÑADIR LIST WORKFLOWS

            WorkOrderEntity workOrder = new WorkOrderEntity(null, oEntidad.Code, null, oEntidad.StartDate, (DateTime)oEntidad.EndDate, oEntidad.Percentage,
                (DateTime)oEntidad.CreationDate, (DateTime)oEntidad.LastModificationDate, null, null, null, null,
                null, null, null, null, null, 0);

            filter = new Filter(nameof(WorkOrderDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<WorkOrderEntity>> lisWorkOrders = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (lisWorkOrders.Result != null && lisWorkOrders.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.WorkOrder + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkOrderEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return workOrder;
            }
        }

        public async Task<Result<WorkOrderEntity>> Validate (WorkOrderEntity workOrderEntity)
        {
            return await Task.FromResult(WorkOrderEntity.UpdateProjectId(workOrderEntity, projID));
        }
    }
}
