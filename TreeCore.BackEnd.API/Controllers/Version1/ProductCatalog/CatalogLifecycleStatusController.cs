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
    /// CatalogLifecycleStatusController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class CatalogLifecycleStatusController : ApiControllerBase<CatalogLifecycleStatusDTO, CatalogLifecycleStatusEntity, CatalogLifecycleStatusDTOMapper>, 
        IDeleteController<CatalogLifecycleStatusDTO>, IModController<CatalogLifecycleStatusDTO>
    {
        private readonly PostCatalogLifecycleStatus _postCatalogLifecycleStatus;
        private readonly PutCatalogLifecycleStatus _putCatalogLifecycleStatus;
        private readonly DeleteCatalogLifecycleStatus _deleteCatalogLifecycleStatus;

        public CatalogLifecycleStatusController(GetCatalogLifecycleStatus getObjectService, PutCatalogLifecycleStatus putCatalogLifecycleStatus, 
            PostCatalogLifecycleStatus postCatalogLifecycleStatus, DeleteCatalogLifecycleStatus deleteCatalogLifecycleStatus) : base(getObjectService)
        {
            _postCatalogLifecycleStatus = postCatalogLifecycleStatus;
            _putCatalogLifecycleStatus = putCatalogLifecycleStatus;
            _deleteCatalogLifecycleStatus = deleteCatalogLifecycleStatus;
        }

        /// <summary>
        /// Post Catalog Lifecycle Status
        /// </summary>
        /// <returns>List of Catalog Lifecycle Status</returns>
        [HttpPost("")]
        public async Task<ResultDto<CatalogLifecycleStatusDTO>> Post(CatalogLifecycleStatusDTO catalogLifecycleStatus)
        {
            return (await _postCatalogLifecycleStatus.Create(catalogLifecycleStatus, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put Catalog Lifecycle Status
        /// </summary>
        /// <param name="code">Code of Catalog Lifecycle Status</param>
        /// <returns>Catalog Lifecycle Status</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<CatalogLifecycleStatusDTO>> Put(CatalogLifecycleStatusDTO catalogLifecycleStatus, string code)
        {
            return (await _putCatalogLifecycleStatus.Update(catalogLifecycleStatus, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Catalog Lifecycle Status
        /// </summary>
        /// <param name="code">Code of Catalog Lifecycle Status</param>
        /// <returns>Catalog Lifecycle Status</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<CatalogLifecycleStatusDTO>> Delete(string code)
        {
            return (await _deleteCatalogLifecycleStatus.Delete(code, Client)).MapDto(x => x);
        }
    }
}
