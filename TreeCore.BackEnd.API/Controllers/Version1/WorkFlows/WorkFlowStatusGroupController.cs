using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.BackEnd.Service.Mappers.WorkFlows;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.WorkFlows;
using TreeCore.Shared.DTO.WorkFlows;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.General
{
    /// <summary>
    /// WorkFlowStatusGroupController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class WorkFlowStatusGroupController : ApiControllerBase<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity, WorkFlowStatusGroupDTOMapper>, IDeleteController<WorkFlowStatusGroupDTO>, IModController<WorkFlowStatusGroupDTO>
    {

        private readonly PutWorkFlowStatusGroup _putWorkFlowStatusGroup;
        private readonly PostWorkFlowStatusGroup _postWorkFlowStatusGroup;
        private readonly DeleteWorkFlowStatusGroup _deleteWorkFlowStatusGroup;

        public WorkFlowStatusGroupController(GetObjectService<WorkFlowStatusGroupDTO, WorkFlowStatusGroupEntity, WorkFlowStatusGroupDTOMapper> getObjectService, PutWorkFlowStatusGroup putWorkFlowStatusGroup, PostWorkFlowStatusGroup postWorkFlowStatusGroup, DeleteWorkFlowStatusGroup deleteWorkFlowStatusGroup) 
            : base(getObjectService)
        {
            _putWorkFlowStatusGroup = putWorkFlowStatusGroup;
            _postWorkFlowStatusGroup = postWorkFlowStatusGroup;
            _deleteWorkFlowStatusGroup = deleteWorkFlowStatusGroup;
        }

        /// <summary>
        /// Post WorkFlowStatusGroup
        /// </summary>
        /// <returns>List of WorkFlowStatusGroup</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<WorkFlowStatusGroupDTO>> Post(WorkFlowStatusGroupDTO statusGroup)
        {
            return (await _postWorkFlowStatusGroup.Create(statusGroup, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of WorkFlowStatusGroup
        /// </summary>
        /// <param name="code">Code of WorkFlowStatusGroup</param>
        /// <returns>List of WorkFlowStatusGroup</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<WorkFlowStatusGroupDTO>> Put(WorkFlowStatusGroupDTO statusGroupDTO, string code)
        {
            return (await _putWorkFlowStatusGroup.Update(statusGroupDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of WorkFlowStatusGroup
        /// </summary>
        /// <param name="code">Code of WorkFlowStatusGroup</param>
        /// <returns>WorkFlowStatusGroup</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<WorkFlowStatusGroupDTO>> Delete(string code)
        {
            return (await _deleteWorkFlowStatusGroup.Delete(code, Client)).MapDto(x => x);
        }

    }
}
