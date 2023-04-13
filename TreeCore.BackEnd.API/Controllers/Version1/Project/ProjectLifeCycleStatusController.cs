using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.Shared.ROP;
using TreeCore.Shared.DTO;
using TreeCore.BackEnd.Service.Services;

using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.Shared.DTO.Project;
using TreeCore.BackEnd.Service.Mappers.Project;
using TreeCore.BackEnd.Service.Validations.Project;
using System.Collections.Immutable;
using TreeCore.BackEnd.Service.Services.Project.ProjectLifeCycleStatus;

namespace TreeCore.BackEnd.API.Controllers.Version1.Project
{

    /// <summary>
    /// ProjectLifeCycleStatusController
    /// </summary>
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [ApiController]
    //[Authorize]
    public class ProjectLifeCycleStatusController : ApiControllerBase<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity, ProjectLifeCycleStatusDTOMapper>, IDeleteController<ProjectLifeCycleStatusDTO>, IModController<ProjectLifeCycleStatusDTO>
    {
        private readonly PutProjectLifeCycleStatus _putProjectLifeCycleStatus;
        private readonly PostProjectLifeCycleStatus _postProjectLifeCycleStatus;
        private readonly DeleteProjectLifeCycleStatus _deleteProjectLifeCycleStatus;

        public ProjectLifeCycleStatusController(GetObjectService<ProjectLifeCycleStatusDTO, ProjectLifeCycleStatusEntity, ProjectLifeCycleStatusDTOMapper> getObjectService, PutProjectLifeCycleStatus putProjectLifeCycleStatus, PostProjectLifeCycleStatus postProjectLifeCycleStatus, DeleteProjectLifeCycleStatus deleteProjectLifeCycleStatus)
            : base(getObjectService)
        {
            _putProjectLifeCycleStatus = putProjectLifeCycleStatus;
            _postProjectLifeCycleStatus = postProjectLifeCycleStatus;
            _deleteProjectLifeCycleStatus = deleteProjectLifeCycleStatus;
        }

        /// <summary>
        /// Post Project Life Cycle Status
        /// </summary>
        /// <returns>List of Project Life Cycle Status</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<ProjectLifeCycleStatusDTO>> Post(ProjectLifeCycleStatusDTO projectLifeCycleStatus)
        {
            return (await _postProjectLifeCycleStatus.Create(projectLifeCycleStatus, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Project Life Cycle Status
        /// </summary>
        /// <param name="code">Code of Project Life Cycle Status</param>
        /// <returns>List of Project Life Cycle Status</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<ProjectLifeCycleStatusDTO>> Put(ProjectLifeCycleStatusDTO ProjectLifeCycleStatusDTO, string code)
        {
            return (await _putProjectLifeCycleStatus.Update(ProjectLifeCycleStatusDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Project Life Cycle Status
        /// </summary>
        /// <param name="code">Code of Project Life Cycle Status</param>
        /// <returns>Project Life Cycle Status</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<ProjectLifeCycleStatusDTO>> Delete(string code)
        {
            return (await _deleteProjectLifeCycleStatus.Delete(code, Client)).MapDto(x => x);
        }
    }
}
