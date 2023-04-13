using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.General;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.General
{
    /// <summary>
    /// RolController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class RolController : ApiControllerBase<RolDTO, RolEntity, RolDTOMapper>, IDeleteController<RolDTO>, IModController<RolDTO>
    {

        private readonly PutRol _putRol;
        private readonly PostRol _postRol;
        private readonly DeleteRol _deleteRol;

        public RolController(GetObjectService<RolDTO, RolEntity, RolDTOMapper> getObjectService, PutRol putRol, PostRol postRol, DeleteRol deleteRol) 
            : base(getObjectService)
        {
            _putRol = putRol;
            _postRol = postRol;
            _deleteRol = deleteRol;
        }

        /// <summary>
        /// Post Rol
        /// </summary>
        /// <returns>List of Rol</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<RolDTO>> Post(RolDTO Rol)
        {
            return (await _postRol.Create(Rol, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Rol
        /// </summary>
        /// <param name="code">Code of Rol</param>
        /// <returns>List of Rol</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<RolDTO>> Put(RolDTO RolDTO, string code)
        {
            return (await _putRol.Update(RolDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Rol
        /// </summary>
        /// <param name="code">Code of Rol</param>
        /// <returns>Rol</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<RolDTO>> Delete(string code)
        {
            return (await _deleteRol.Delete(code, Client)).MapDto(x => x);
        }

    }
}
