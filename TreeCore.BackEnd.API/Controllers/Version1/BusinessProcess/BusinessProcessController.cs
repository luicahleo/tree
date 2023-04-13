using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Service.Mappers.BusinessProcess;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.BusinessProcess;
using TreeCore.Shared.DTO.BusinessProcess;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.Version1.BusinessProcess
{
    /// <summary>
    /// BusinessProcessController
    /// </summary>
    [ApiController]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class BusinessProcessController : ApiControllerBase<BusinessProcessDTO, BusinessProcessEntity, BusinessProcessDTOMapper>, IDeleteController<BusinessProcessDTO>, 
        IModController<BusinessProcessDTO>
    {
        private readonly PutBusinessProcess _putBusinessProcess;
        private readonly PostBusinessProcess _postBusinessProcess;
        private readonly DeleteBusinessProcess _deleteBusinessProcess;

        public BusinessProcessController(GetObjectService<BusinessProcessDTO, BusinessProcessEntity, BusinessProcessDTOMapper> getObjectService, PutBusinessProcess putBusinessProcess,
            PostBusinessProcess postBusinessProcess, DeleteBusinessProcess deleteBusinessProcess)
            : base(getObjectService)
        {
            _putBusinessProcess = putBusinessProcess;
            _postBusinessProcess = postBusinessProcess;
            _deleteBusinessProcess = deleteBusinessProcess;
        }

        /// <summary>
        /// Post BusinessProcess
        /// </summary>
        /// <returns>List of BusinessProcess</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<BusinessProcessDTO>> Post(BusinessProcessDTO businessProcess)
        {
            return (await _postBusinessProcess.Create(businessProcess, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of BusinessProcess
        /// </summary>
        /// <param name="code">Code of BusinessProcess</param>
        /// <returns>List of BusinessProcess</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<BusinessProcessDTO>> Put(BusinessProcessDTO businessProcessDTO, string code)
        {
            return (await _putBusinessProcess.Update(businessProcessDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of BusinessProcess
        /// </summary>
        /// <param name="code">Code of BusinessProcess</param>
        /// <returns>BusinessProcess</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<BusinessProcessDTO>> Delete(string code)
        {
            return (await _deleteBusinessProcess.Delete(code, Client)).MapDto(x => x);
        }

    }
}

