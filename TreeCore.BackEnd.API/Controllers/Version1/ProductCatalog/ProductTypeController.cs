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
    /// ProductTypeController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class ProductTypeController : ApiControllerBase<ProductTypeDTO, ProductTypeEntity, ProductTypeDTOMapper>, IDeleteController<ProductTypeDTO>, IModController<ProductTypeDTO>
    {
        private readonly PostProductType _postProductType;
        private readonly PutProductType _putProductType;
        private readonly DeleteProductType _deleteProductType;

        public ProductTypeController(GetObjectService<ProductTypeDTO, ProductTypeEntity, ProductTypeDTOMapper> getObjectService, PutProductType putCatalogType, PostProductType postCatalogType, DeleteProductType deleteCatalogType) : base(getObjectService)
        {
            _postProductType = postCatalogType;
            _putProductType = putCatalogType;
            _deleteProductType = deleteCatalogType;
        }

        /// <summary>
        /// Post Product Type
        /// </summary>
        /// <returns>List of Product Types</returns>
        [HttpPost("")]
        public async Task<ResultDto<ProductTypeDTO>> Post(ProductTypeDTO productType)
        {
            return (await _postProductType.Create(productType, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Put Product Type
        /// </summary>
        /// <param name="code">Code of Product Type</param>
        /// <returns>List of Product Types</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<ProductTypeDTO>> Put(ProductTypeDTO productType, string code)
        {
            return (await _putProductType.Update(productType, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Product Type
        /// </summary>
        /// <param name="code">Code of Product Type</param>
        /// <returns>Product Type</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<ProductTypeDTO>> Delete(string code)
        {
            return (await _deleteProductType.Delete(code, Client)).MapDto(x => x);
        }
    }
}
