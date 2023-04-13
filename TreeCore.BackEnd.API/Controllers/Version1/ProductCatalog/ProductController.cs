using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers.ProductCatalog;
using TreeCore.BackEnd.Service.Services.ProductCatalog;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.ProductCatalog
{
    /// <summary>
    /// ProductController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]" + Routes.Carpeta + "[controller]")]
    public class ProductController : ApiControllerBase<ProductDTO, ProductEntity, ProductDTOMapper>, IDeleteController<ProductDTO>, IModController<ProductDTO>
    {
        private readonly PostProduct _postProduct;
        private readonly PutProduct _putProduct;
        private readonly GetProduct _getProduct;
        private readonly DeleteProduct _deleteProduct;

        public ProductController(GetProduct getProduct, PutProduct putProduct,
            PostProduct postProduct, DeleteProduct deleteProduct) : base(getProduct)
        {
            _postProduct = postProduct;
            _putProduct = putProduct;
            _getProduct = getProduct;
            _deleteProduct = deleteProduct;
        }

        /// <summary>
        /// Get Details Product By Code
        /// </summary>
        /// <param name="code">Code of Product</param>
        /// <returns>Details Product</returns>
        [HttpGet("Details/{code}")]
        public async Task<ResultDto<ProductDetailsDTO>> GetDetails(string code)
        {
            return (await _getProduct.GetItemDetailsByCode(code, Client)).MapDto(x => x);
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <returns>Product</returns>
        [HttpPost("")]
        public async Task<ResultDto<ProductDTO>> Post(ProductDTO product)
        {
            return (await _postProduct.Create(product, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="code">Code of Product</param>
        /// <returns>Product</returns>
        [HttpPut("{code}")]
        public async Task<ResultDto<ProductDTO>> Put(ProductDTO product, string code)
        {
            return (await _putProduct.Update(product, code, Client, EmailUser)).MapDto(x => x);
        }

        /// <summary>
        /// Delete object of Product
        /// </summary>
        /// <param name="code">Code of Product</param>
        /// <returns>Product</returns>
        [HttpDelete("{code}")]
        public async Task<ResultDto<ProductDTO>> Delete(string code)
        {
            return (await _deleteProduct.Delete(code, Client)).MapDto(x => x);
        }
    }
}
