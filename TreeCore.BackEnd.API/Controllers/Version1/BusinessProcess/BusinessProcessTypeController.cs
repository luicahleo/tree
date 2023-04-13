using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services.BusinessProcess;
using TreeCore.Shared.ROP;
using TreeCore.Shared.DTO;
using TreeCore.Shared.DTO.BusinessProcess;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Service.Services;

namespace TreeCore.BackEnd.API.Controllers.Version1.BusinessProcess
{
    /// <summary>
    /// BusinessProcessTypeController
    /// </summary>
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [ApiController]
    //[Authorize]


    public class BusinessProcessTypeController : ApiControllerBase<BusinessProcessTypeDTO,BusinessProcessTypeEntity,BusinessProcessTypeDTOMapper>, IDeleteController<BusinessProcessTypeDTO>, IModController<BusinessProcessTypeDTO>
    {

        private readonly PutBusinessProcessType _putBusinessProcessType;
        private readonly PostBusinessProcessType _postBusinessProcessType;
        private readonly DeleteBusinessProcessType _deleteBusinessProcessType;

        public BusinessProcessTypeController(GetObjectService<BusinessProcessTypeDTO, BusinessProcessTypeEntity, BusinessProcessTypeDTOMapper> getObjectService, PutBusinessProcessType putBusinessProcessType, PostBusinessProcessType postBusinessProcessType, DeleteBusinessProcessType deleteBusinessProcessType)
            : base(getObjectService)
        {
            _putBusinessProcessType = putBusinessProcessType;
            _postBusinessProcessType = postBusinessProcessType;
            _deleteBusinessProcessType = deleteBusinessProcessType;
        }

        /// <summary>
        /// Post Business Process Type
        /// </summary>
        /// <returns>List of Business Process Types</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<BusinessProcessTypeDTO>> Post(BusinessProcessTypeDTO businessProcessType)
        {
            return (await _postBusinessProcessType.Create(businessProcessType, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Business Process Types
        /// </summary>
        /// <param name="code">Code of Business Process Type</param>
        /// <returns>List of Business Process Types</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<BusinessProcessTypeDTO>> Put(BusinessProcessTypeDTO BusinessProcessTypeDTO, string code)
        {
            return (await _putBusinessProcessType.Update(BusinessProcessTypeDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Business Process Types
        /// </summary>
        /// <param name="code">Code of Business Process Type</param>
        /// <returns>Business Process type</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<BusinessProcessTypeDTO>> Delete(string code)
        {
            return (await _deleteBusinessProcessType.Delete(code, Client)).MapDto(x => x);
        }
    }
}
