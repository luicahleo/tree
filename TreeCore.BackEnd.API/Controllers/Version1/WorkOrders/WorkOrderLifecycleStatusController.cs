using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.WorkOrders;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.General
{
    /// <summary>
    /// WorkOrderTrackingStatus
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class WorkOrderTrackingStatus : ApiControllerBase<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity, WorkOrderLifecycleStatusDTOMapper>, IDeleteController<WorkOrderLifecycleStatusDTO>, IModController<WorkOrderLifecycleStatusDTO>
    {

        private readonly PutWorkOrderLifecycleStatus _putWorkOrderTrackingStatus;
        private readonly PostWorkOrderLifecycleStatus _postWorkOrderTrackingStatus;
        private readonly DeleteWorkOrderLifecycleStatus _deleteWorkOrderTrackingStatus;

        public WorkOrderTrackingStatus(GetObjectService<WorkOrderLifecycleStatusDTO, WorkOrderLifecycleStatusEntity, WorkOrderLifecycleStatusDTOMapper> getObjectService, PutWorkOrderLifecycleStatus putWorkOrderTrackingStatus, PostWorkOrderLifecycleStatus postWorkOrderTrackingStatus, DeleteWorkOrderLifecycleStatus deleteWorkOrderTrackingStatus) 
            : base(getObjectService)
        {
            _putWorkOrderTrackingStatus = putWorkOrderTrackingStatus;
            _postWorkOrderTrackingStatus = postWorkOrderTrackingStatus;
            _deleteWorkOrderTrackingStatus = deleteWorkOrderTrackingStatus;
        }

        /// <summary>
        /// Post WorkOrderTrackingStatus
        /// </summary>
        /// <returns>List of WorkOrderTrackingStatus</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<WorkOrderLifecycleStatusDTO>> Post(WorkOrderLifecycleStatusDTO trackingStatus)
        {
            return (await _postWorkOrderTrackingStatus.Create(trackingStatus, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of WorkOrderTrackingStatus
        /// </summary>
        /// <param name="code">Code of WorkOrderTrackingStatus</param>
        /// <returns>List of WorkOrderTrackingStatus</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<WorkOrderLifecycleStatusDTO>> Put(WorkOrderLifecycleStatusDTO trackingStatusDTO, string code)
        {
            return (await _putWorkOrderTrackingStatus.Update(trackingStatusDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of WorkOrderTrackingStatus
        /// </summary>
        /// <param name="code">Code of WorkOrderTrackingStatus</param>
        /// <returns>WorkOrderTrackingStatus</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<WorkOrderLifecycleStatusDTO>> Delete(string code)
        {
            return (await _deleteWorkOrderTrackingStatus.Delete(code, Client)).MapDto(x => x);
        }

    }
}
