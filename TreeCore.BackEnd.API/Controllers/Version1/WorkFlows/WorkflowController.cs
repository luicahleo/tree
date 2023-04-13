using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.WorkFlows;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.WorkFlows;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.WorkFlows
{
    /// <summary>
    /// WorkflowController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class WorkflowController : ApiControllerBase<WorkflowDTO, WorkflowEntity, WorkflowDTOMapper>, IDeleteController<WorkflowDTO>, IModController<WorkflowDTO>
    {
        private readonly PutWorkflow _putWorkflows;
        private readonly PostWorkflow _postWorkflows;
        private readonly DeleteWorkflow _deleteWorkflows;

        private readonly GetWorkFlowStatus _getWorkflowStatus;
        private readonly PutWorkFlowStatus _putWorkflowStatus;
        private readonly PostWorkFlowStatus _postWorkflowStatus;
        private readonly DeleteWorkFlowStatus _deleteWorkflowStatus;

        public WorkflowController(GetObjectService<WorkflowDTO, WorkflowEntity, WorkflowDTOMapper> getObjectService, 
            PutWorkflow putWorkflows, PostWorkflow postWorkflows, DeleteWorkflow deleteWorkflows, PutWorkFlowStatus putWorkflowStatus,
            PostWorkFlowStatus postWorkflowStatus, DeleteWorkFlowStatus deleteWorkflowStatus, GetWorkFlowStatus getWorkflowStatus)
            : base(getObjectService)
        {
            _putWorkflows = putWorkflows;
            _postWorkflows = postWorkflows;
            _deleteWorkflows = deleteWorkflows;

            _getWorkflowStatus = getWorkflowStatus;
            _putWorkflowStatus = putWorkflowStatus;
            _postWorkflowStatus = postWorkflowStatus;
            _deleteWorkflowStatus = deleteWorkflowStatus;
        }

        /// <summary>
        /// Post Workflows
        /// </summary>
        /// <returns>List of Workflows</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<WorkflowDTO>> Post(WorkflowDTO workflow)
        {
            return (await _postWorkflows.Create(workflow, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Workflows
        /// </summary>
        /// <param name="code">Code of Workflow</param>
        /// <returns>List of Workflows</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<WorkflowDTO>> Put(WorkflowDTO workflow, string code)
        {
            return (await _putWorkflows.Update(workflow, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Workflows
        /// </summary>
        /// <param name="code">Code of Workflow</param>
        /// <returns>Workflow</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<WorkflowDTO>> Delete(string code)
        {
            return (await _deleteWorkflows.Delete(code, Client)).MapDto(x => x);
        }

        /// <summary>
        /// Post Status
        /// </summary>
        /// /// <param name="WorkflowCode">Code of Workflow</param>
        /// <returns>List of status of Workflow</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{WorkflowCode}/Status")]
        public async Task<ResultDto<IEnumerable<WorkFlowStatusDTO>>> GetListStatus(string WorkflowCode)
        {
            return (await _getWorkflowStatus.GetListByWorkFlow(WorkflowCode, Client)).MapDto(x => x);
        }

        /// <summary>
        /// Post Status
        /// </summary>
        /// /// <param name="WorkflowCode">Code of Workflow</param>
        /// <returns>List of status of Workflow</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("{WorkflowCode}/Status")]
        public async Task<ResultDto<WorkFlowStatusDTO>> Post(string WorkflowCode, WorkFlowStatusDTO status)
        {
            return (await _postWorkflowStatus.Create(status, Client, EmailUser, WorkflowCode)).MapDto(x => x);
        }

        /// <summary>
        /// Put object status of Workflows
        /// </summary>
        /// <param name="StatusCode">Code of status of Workflow</param>
        /// <param name="WorkflowCode">Code of Workflow</param>
        /// <returns>List of status of Workflows</returns>
        [HttpPut("{WorkflowCode}/Status/{StatusCode}")]
        public async Task<ResultDto<WorkFlowStatusDTO>> Put(string WorkflowCode, WorkFlowStatusDTO status, string StatusCode)
        {
            return (await _putWorkflowStatus.Update(status, StatusCode, Client, EmailUser, WorkflowCode)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Status of Workflows
        /// </summary>
        /// <param name="StatusCode">Code of Status of Workflow</param>
        /// <param name="WorkflowCode">Code of Workflow</param>
        /// <returns>Status of Workflow</returns>
        [HttpDelete("{WorkflowCode}/Status/{StatusCode}")]
        public async Task<ResultDto<WorkFlowStatusDTO>> Delete(string StatusCode, string WorkflowCode)
        {
            return (await _deleteWorkflowStatus.Delete(StatusCode, Client)).MapDto(x => x);
        }
    }
}
