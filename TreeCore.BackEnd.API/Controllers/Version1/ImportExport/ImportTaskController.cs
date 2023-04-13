using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ImportExport;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.ImportExport;
using TreeCore.Shared.DTO.ImportExport;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.ImportExport
{
    /// <summary>
    /// ImportTaskController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route (Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class ImportTaskController : ApiControllerBase<ImportTaskDTO, ImportTaskEntity, ImportTaskDTOMapper>, IDeleteController<ImportTaskDTO>, IModController<ImportTaskDTO>
    {
        private readonly PostImportTask _postImportTask;
        private readonly PutImportTask _putImportTask;
        private readonly DeleteImportTask _deleteImportTask;

        public ImportTaskController(GetObjectService<ImportTaskDTO, ImportTaskEntity, ImportTaskDTOMapper> getObjectService, PostImportTask postImportTask, 
            PutImportTask putImportTask, DeleteImportTask deleteImportTask) 
            : base(getObjectService)
        {
            _postImportTask = postImportTask;
            _putImportTask = putImportTask;
            _deleteImportTask = deleteImportTask;
        }

        /// <summary>
        /// Post ImportTask
        /// </summary>
        /// <returns>List of Currencies</returns>
        [HttpPost("")]
        public async Task<ResultDto<ImportTaskDTO>> Post(ImportTaskDTO ImportTask)
        {
            return (await _postImportTask.Create(ImportTask, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put ImportTask 
        /// </summary>
        /// <param name="codeImportTask">Code of ImportTask</param>
        /// <returns>List of Currencies</returns>
        [HttpPut("{codeImportTask}")]
        public async Task<ResultDto<ImportTaskDTO>> Put(ImportTaskDTO ImportTask, string codeImportTask)
        {
            return (await _putImportTask.Update(ImportTask, codeImportTask, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of ImportTask
        /// </summary>
        /// <param name="code">Code of ImportTask</param>
        /// <returns>ImportTask</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<ImportTaskDTO>> Delete(string code)
        {
            return (await _deleteImportTask.Delete(code, Client)).MapDto(x => x);
        }
    }
}
