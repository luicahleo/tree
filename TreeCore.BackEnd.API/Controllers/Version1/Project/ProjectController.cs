using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.Project;
using TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrder;
using TreeCore.BackEnd.Service.Services.WorkOrders.WorkOrderTrackingStatus;
using TreeCore.Shared.DTO.Project;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.Version1.Project
{

    /// <summary>
    /// ProjectController
    /// </summary>
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [ApiController]
    //[Authorize]
    public class ProjectController : ApiControllerBase<ProjectDTO, ProjectEntity, ProjectDTOMapper>
    {
        private readonly PutProject _putProject;
        private readonly PostProject _postProject;
        private readonly DeleteProject _deleteProject;

        private readonly PutWorkOrder _putWorkOrder;
        private readonly GetWorkOrder _getWorkOrder;
        private readonly GetWorkOrderTrackingStatus _getWorkOrderTrackingStatus;
        private readonly PutWorkOrderTrackingStatus _putWorkOrderTrackingStatus;
        private readonly PostWorkOrderTrackingStatus _postWorkOrderTrackingStatus;

        public ProjectController(GetObjectService<ProjectDTO, ProjectEntity, ProjectDTOMapper> getObjectService,
            PutProject putProject, PostProject postProject, DeleteProject deleteProject, PutWorkOrder putWorkOrder,
            PutWorkOrderTrackingStatus putWorkOrderTrackingStatus, PostWorkOrderTrackingStatus postWorkOrderTrackingStatus,
            GetWorkOrder getWorkOrder, GetWorkOrderTrackingStatus getWorkOrderTrackingStatus)
            : base(getObjectService)
        {
            _putProject = putProject;
            _postProject = postProject;
            _deleteProject = deleteProject;
            _putWorkOrder = putWorkOrder;
            _getWorkOrder = getWorkOrder;
            _getWorkOrderTrackingStatus = getWorkOrderTrackingStatus;
            _putWorkOrderTrackingStatus = putWorkOrderTrackingStatus;
            _postWorkOrderTrackingStatus = postWorkOrderTrackingStatus;
        }

        /// <summary>
        /// Post Project
        /// </summary>
        /// <returns>List of Project</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<ProjectDTO>> Post(PostProjectDTO postProjectDTO)
        {
            return (await _postProject.Create(postProjectDTO.ProjectDTO, postProjectDTO.WorkOrderDTO, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Project 
        /// </summary>
        /// <param name="code">Code of Project</param>
        /// <returns>List of Project</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<ProjectDTO>> Put(ProjectDTO ProjectDTO, string code)
        {
            return (await _putProject.Update(ProjectDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of work order of Project 
        /// </summary>
        /// <param name="workOrderCode">Code of Work Order</param>
        /// <param name="projectCode">Code of Project</param>
        /// <returns>List of Work Order</returns>
        [HttpPut("{projectCode}/WorkOrder/{workOrderCode}")]
        public async Task<ResultDto<WorkOrderDTO>> Put(string projectCode, WorkOrderDTO workOrder, string workOrderCode)
        {
            return (await _putWorkOrder.Update(workOrder, workOrderCode, Client, EmailUser, projectCode)).MapDto(x => x);
        }

        ///// <summary>
        ///// Get Work Orders
        ///// </summary>
        ///// /// <param name="projectCode">Code of Project</param>
        ///// <returns>List of work orders of Project</returns>
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[HttpGet("{projectCode}/WorkOrder")]
        //public async Task<ResultDto<IEnumerable<WorkOrderDTO>>> GetListWorkOrders(string projectCode)
        //{
        //    return (await _getWorkOrder.GetListByProject(projectCode, Client)).MapDto(x => x);
        //}

        /// <summary>
        /// Post object of tracking status of work order
        /// </summary>
        /// <param name="projectCode">Code of Project</param>
        /// <returns>List of Tracking Status</returns>
        [HttpPost("{projectCode}/TrackingStatus/{trackingStatusCode}")]
        public async Task<ResultDto<WorkOrderTrackingStatusDTO>> Post(string projectCode, WorkOrderTrackingStatusDTO trackingStatus)
        {
            return (await _postWorkOrderTrackingStatus.Create(trackingStatus, Client, EmailUser, projectCode)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of tracking status of work order  
        /// </summary>
        /// <param name="trackingStatusCode">Code of Tracking Status of Work Order</param>
        /// <param name="projectCode">Code of Project</param>
        /// <returns>List of Work Order</returns>
        [HttpPut("{projectCode}/TrackingStatus/{trackingStatusCode}")]
        public async Task<ResultDto<WorkOrderTrackingStatusDTO>> Put(string projectCode,
            WorkOrderTrackingStatusDTO trackingStatus, string trackingStatusCode)
        {
            return (await _putWorkOrderTrackingStatus.Update(trackingStatus, trackingStatusCode, Client, EmailUser, projectCode)).MapDto(x => x);
        }

        ///// <summary>
        ///// Get Tracking Status
        ///// </summary>
        ///// /// <param name="projectCode">Code of Project</param>
        ///// <returns>List of tracking status of Project</returns>
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[HttpGet("{projectCode}/TrackingStatus")]
        //public async Task<ResultDto<IEnumerable<WorkOrderTrackingStatusDTO>>> GetListTrackingStatus(string projectCode)
        //{
        //    return (await _getWorkOrderTrackingStatus.GetListByProject(projectCode, Client)).MapDto(x => x);
        //}

        /// <summary>
        /// Post object of tracking of tracking status 
        /// </summary>
        /// <param name="projectCode">Code of Project</param>
        /// <returns>List of Tracking</returns>
        //[HttpPost("{projectCode}/Tracking/{trackingCode}")]
        //public async Task<ResultDto<WorkOrderTrackingDTO>> Post(string projectCode, WorkOrderTrackingDTO tracking)
        //{
        //    return (await _postWorkOrderTracking.Create(trackingStatus, Client, EmailUser, projectCode)).MapDto(x => x);
        //}

    }
}
