using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.Program;
using TreeCore.Shared.DTO.Project;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.General
{
    /// <summary>
    /// Program
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class Program : ApiControllerBase<ProgramDTO, ProgramEntity, ProgramDTOMapper>, IDeleteController<ProgramDTO>, IModController<ProgramDTO>
    {

        private readonly PutProgram _putProgram;
        private readonly PostProgram _postProgram;
        private readonly DeleteProgram _deleteProgram;

        public Program(GetObjectService<ProgramDTO, ProgramEntity, ProgramDTOMapper> getObjectService, PutProgram putProgram, PostProgram postProgram, DeleteProgram deleteProgram) 
            : base(getObjectService)
        {
            _putProgram = putProgram;
            _postProgram = postProgram;
            _deleteProgram = deleteProgram;
        }

        /// <summary>
        /// Post Program
        /// </summary>
        /// <returns>List of Program</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<ProgramDTO>> Post(ProgramDTO trackingStatus)
        {
            return (await _postProgram.Create(trackingStatus, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Program
        /// </summary>
        /// <param name="code">Code of Program</param>
        /// <returns>List of Program</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<ProgramDTO>> Put(ProgramDTO trackingStatusDTO, string code)
        {
            return (await _putProgram.Update(trackingStatusDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Program
        /// </summary>
        /// <param name="code">Code of Program</param>
        /// <returns>Program</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<ProgramDTO>> Delete(string code)
        {
            return (await _deleteProgram.Delete(code, Client)).MapDto(x => x);
        }

    }
}
