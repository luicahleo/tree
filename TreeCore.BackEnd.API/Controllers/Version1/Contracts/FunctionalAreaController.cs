using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.Contracts;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.Contracts
{
    /// <summary>
    /// FunctionalAreaController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class FunctionalAreaController : ApiControllerBase<FunctionalAreaDTO, FunctionalAreaEntity, FuncionalAreaDTOMapper>, IDeleteController<FunctionalAreaDTO>, IModController<FunctionalAreaDTO>
    {

        private readonly PutFunctionalArea _putFunctionalArea;
        private readonly PostFunctionalArea _postFunctionalArea;
        private readonly DeleteFunctionalArea _deleteFunctionalArea;

        public FunctionalAreaController(GetObjectService<FunctionalAreaDTO, FunctionalAreaEntity, FuncionalAreaDTOMapper> getObjectService, PutFunctionalArea putFunctionalArea, PostFunctionalArea postFunctionalArea, DeleteFunctionalArea deleteFunctionalArea)
            : base(getObjectService)
        {
            _putFunctionalArea = putFunctionalArea;
            _postFunctionalArea = postFunctionalArea;
            _deleteFunctionalArea = deleteFunctionalArea;
        }

        /// <summary>
        /// Post Product Type
        /// </summary>
        /// <returns>List of Product Types</returns>
        [HttpPost("")]
        public async Task<ResultDto<FunctionalAreaDTO>> Post(FunctionalAreaDTO functionalArea)
        {
            return (await _postFunctionalArea.Create(functionalArea, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Catalog Types
        /// </summary>
        /// <param name="code">Code of Catalog Type</param>
        /// <returns>List of Catalog Types</returns>
        [HttpPut("{id}")]
        public async Task<ResultDto<FunctionalAreaDTO>> Put(FunctionalAreaDTO functionalAreaDTO, string code)
        {
            return (await _putFunctionalArea.Update(functionalAreaDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Catalog Types
        /// </summary>
        /// <param name="code">Code of Catalog Type</param>
        /// <returns>Catalog type</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<FunctionalAreaDTO>> Delete(string code)
        {
            return (await _deleteFunctionalArea.Delete(code, Client)).MapDto(x => x);
        }

    }
}
