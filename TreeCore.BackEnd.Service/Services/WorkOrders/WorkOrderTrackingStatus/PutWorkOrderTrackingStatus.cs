using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.Service.Validations.WorkOrders;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrderTrackingStatus
{
    public class PutWorkOrderTrackingStatus : PutObjectService<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity, WorkOrderTrackingStatusDTOMapper>
    {
        private readonly GetDependencies<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity> _getDependency;
        private readonly GetDependencies<UserDTO, UserEntity> _getUser;

        public PutWorkOrderTrackingStatus(PutDependencies<WorkOrderTrackingStatusEntity> putDependency,
            GetDependencies<WorkOrderTrackingStatusDTO, WorkOrderTrackingStatusEntity> getDependency,
            GetDependencies<UserDTO, UserEntity> getUser,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new WorkOrderTrackingStatusValidation())
        {
            _getDependency = getDependency;
            _getUser = getUser;
        }
        public override async Task<Result<WorkOrderTrackingStatusEntity>> ValidateEntity(WorkOrderTrackingStatusDTO workOrder, int Client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();

            WorkOrderTrackingStatusEntity? workO = await _getDependency.GetItemByCode(code, Client);
            if (workO == null)
            {
                lErrors.Add(Error.Create(_traduccion.Code + " " + _traduccion.WorkOrderTrackingStatus + " " + $"{code}" + " " + _errorTraduccion.NotFound + "."));
            }
            else
            {
                UserEntity assignedUser = await _getUser.GetItemByCode(workOrder.PreviusWorkOrderTrackingStatusCode, Client);
                if (assignedUser == null)
                {
                    lErrors.Add(Error.Create(_traduccion.Email + " " + _traduccion.User + " " + $"{workOrder.AssignedUserEmail}" + " " + _errorTraduccion.NotFound + "."));
                }

                workO.AssignedUsuario = assignedUser;

            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<WorkOrderTrackingStatusEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return workO;
            }
        }
    }
}
