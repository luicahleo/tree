using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services;
using TreeCore.BackEnd.Service.Services.ProductCatalog;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.ProductCatalog
{
    /// <summary>
    /// CatalogTypeController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CatalogTypeController : ApiControllerBase<CatalogTypeDTO, CatalogTypeEntity, CatalogTypeDTOMapper>, IDeleteController<CatalogTypeDTO>, IModController<CatalogTypeDTO>
    {

        private readonly PutCatalogType _putCatalogType;
        private readonly PostCatalogType _postCatalogType;
        private readonly DeleteCatalogType _deleteCatalogType;

        public CatalogTypeController(GetObjectService<CatalogTypeDTO, CatalogTypeEntity, CatalogTypeDTOMapper> getObjectService, PutCatalogType putCatalogType, PostCatalogType postCatalogType, DeleteCatalogType deleteCatalogType) 
            : base(getObjectService)
        {
            _putCatalogType = putCatalogType;
            _postCatalogType = postCatalogType;
            _deleteCatalogType = deleteCatalogType;
        }

        /// <summary>
        /// Post Product Type
        /// </summary>
        /// <returns>List of Product Types</returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResultDto<CatalogTypeDTO>> Post(CatalogTypeDTO catalogType)
        {
            return (await _postCatalogType.Create(catalogType, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put object of Catalog Types
        /// </summary>
        /// <param name="code">Code of Catalog Type</param>
        /// <returns>List of Catalog Types</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<CatalogTypeDTO>> Put(CatalogTypeDTO catalogTypeDTO, string code)
        {
            return (await _putCatalogType.Update(catalogTypeDTO, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Catalog Types
        /// </summary>
        /// <param name="code">Code of Catalog Type</param>
        /// <returns>Catalog type</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<CatalogTypeDTO>> Delete(string code)
        {
            return (await _deleteCatalogType.Delete(code, Client)).MapDto(x => x);
        }

    }
}
