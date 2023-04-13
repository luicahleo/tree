using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services.ProductCatalog;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.ProductCatalog
{
    /// <summary>
    /// CatalogController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class CatalogController : ApiControllerBase<CatalogDTO, CatalogEntity, CatalogDTOMapper>, IDeleteController<CatalogDTO>, IModController<CatalogDTO>
    {
        private readonly PostCatalog _postCatalog;
        private readonly PutCatalog _putCatalog;
        private readonly GetCatalog _getCatalog;
        private readonly DeleteCatalog _deleteCatalog;

        public CatalogController(GetCatalog getCatalog, PutCatalog putCatalog,
            PostCatalog postCatalog, DeleteCatalog deleteCatalog) : base(getCatalog)
        {
            _postCatalog = postCatalog;
            _putCatalog = putCatalog;
            _getCatalog = getCatalog;
            _deleteCatalog = deleteCatalog;
        }

        /// <summary>
        /// Get Details Catalog By Code
        /// </summary>
        /// <param name="codeProduct">Code of Product</param>
        /// <param name="codeCatalog">Code of Catalog</param>
        /// <returns>Details Catalog</returns>
        //[HttpGet("Assigned/{codeProduct}{codeCatalog}")]
        //public async Task<ResultDto<CatalogAssignedProductsDTO>> GetDetails(string codeProduct, string codeCatalog)
        //{
        //    return (await _getCatalog.GetItemDetailsByCode(codeProduct, codeCatalog)).MapDto(x => x);
        //}

        /// <summary>
        /// Create Catalog
        /// </summary>
        /// <returns>Catalog</returns>
        [HttpPost("")]
        public async Task<ResultDto<CatalogDTO>> Post(CatalogDTO catalog)
        {
            return (await _postCatalog.Create(catalog, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Update Catalog
        /// </summary>
        /// <param name="catalog">Catalog</param>
        /// <param name="code">Code of Catalog</param>
        /// <returns>Catalog</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<CatalogDTO>> Put(CatalogDTO catalog, string code)
        {
            return (await _putCatalog.Update(catalog, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Catalog
        /// </summary>
        /// <param name="code">Code of Catalog</param>
        /// <returns>Catalog</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<CatalogDTO>> Delete(string code)
        {
            return (await _deleteCatalog.Delete(code, Client)).MapDto(x => x);
        }
    }
}

