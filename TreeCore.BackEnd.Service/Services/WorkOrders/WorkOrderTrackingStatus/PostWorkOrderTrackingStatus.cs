using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.Service.Validations.WorkOrders;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrderTrackingStatus
{
    public class PostWorkOrderTrackingStatus : PostObjectService<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity, WorkOrderTrackingStatusDTOMapper>
    {

        private readonly GetDependencies<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity> _getDependency;
        private readonly GetDependencies<WorkOrderDTO, WorkOrderEntity> _getWorkOrders;
        private readonly GetDependencies<UserDTO, UserEntity> _getUser;
        private readonly GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> _getStatus;

        public PostWorkOrderTrackingStatus(PostDependencies<WorkOrderTrackingStatusEntity> postDependency,
            GetDependencies<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity> getDependency,
            GetDependencies<WorkOrderDTO, WorkOrderEntity> getWorkOrders,
            GetDependencies<UserDTO, UserEntity> getUser,
            GetDependencies<WorkFlowStatusDTO, WorkFlowStatusEntity> getStatus,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new WorkOrderTrackingStatusValidation())
        {
            _getDependency = getDependency;
            _getWorkOrders = getWorkOrders;
            _getUser = getUser;
            _getStatus = getStatus;
        }

        public override async Task<Result<WorkOrderTrackingStatusEntity>> ValidateEntity(WorkOrderTrackingStatusDTO oEntidad, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            WorkOrderEntity workOrders = await _getWorkOrders.GetItemByCode(code, Client);
            if (workOrders == null)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.WorkOrder + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }

            WorkOrderTrackingStatusEntity previusTrackingStatus = await _getDependency.GetItemByCode(oEntidad.PreviusWorkOrderTrackingStatusCode, Client);
            if (previusTrackingStatus == null)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.WorkOrderTrackingStatus + " " + $"{oEntidad.PreviusWorkOrderTrackingStatusCode}" + " " + _errorTraduccion.NotFound + "."));
            }
            
            UserEntity assignedUser = await _getUser.GetItemByCode(oEntidad.AssignedUserEmail, Client);
            if (assignedUser == null)
            {
                lErrors.Add(Error.Create(_traduccion.Email + " " + _traduccion.User + " " + $"{oEntidad.AssignedUserEmail}" + " " + _errorTraduccion.NotFound + "."));
            }
            
            WorkFlowStatusEntity status = await _getStatus.GetItemByCode(oEntidad.StatusCode, Client);
            if (status == null)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.CodeWorkFlowStatus + " " + $"{oEntidad.StatusCode}" + " " + _errorTraduccion.NotFound + "."));
            }

            UserEntity createUser = await _getUser.GetItemByCode(email, Client);
            


            WorkOrderTrackingStatusEntity trackingStatus = new WorkOrderTrackingStatusEntity(null, oEntidad.Code, workOrders, previusTrackingStatus, assignedUser, status, DateTime.Now, createUser);

            filter = new Filter(nameof(WorkOrderTrackingStatusDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);

            Task<IEnumerable<WorkOrderTrackingStatusEntity>> lisWorkOrderTrackingStatus = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (lisWorkOrderTrackingStatus.Result != null && lisWorkOrderTrackingStatus.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.WorkOrderTrackingStatus + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkOrderTrackingStatusEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return trackingStatus;
            }
        }
    }
}
