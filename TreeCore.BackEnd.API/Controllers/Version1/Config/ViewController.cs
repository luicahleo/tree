using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Config;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.Config;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.Config;
using TreeCore.BackEnd.Service.Services.General;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Config;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.Config
{
    /// <summary>
    /// ViewController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class ViewController : ApiControllerBase<ViewDTO, ViewEntity, ViewDTOMapper>, IDeleteController<ViewDTO>, IModController<ViewDTO>
    {

        private readonly PutView _putView;
        private readonly PostView _postView;
        private readonly DeleteView _deleteView;

        public ViewController(GetObjectService<ViewDTO, ViewEntity, ViewDTOMapper> getObjectService, PutView putView, PostView postView, DeleteView deleteView) 
            : base(getObjectService)
        {
            _putView = putView;
            _postView = postView;
            _deleteView = deleteView;
        }

        /// <summary>
        /// Post View
        /// </summary>
        /// <returns>List of View</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<ViewDTO>> Post(ViewDTO View)
        {
            return (await _postView.Create(View, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of View
        /// </summary>
        /// <param name="code">Code of View</param>
        /// <returns>List of View</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<ViewDTO>> Put(ViewDTO ViewDTO, string code)
        {
            return (await _putView.Update(ViewDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of View
        /// </summary>
        /// <param name="code">Code of View</param>
        /// <returns>View</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<ViewDTO>> Delete(string code)
        {
            return (await _deleteView.Delete(code, Client)).MapDto(x => x);
        }

    }
}
