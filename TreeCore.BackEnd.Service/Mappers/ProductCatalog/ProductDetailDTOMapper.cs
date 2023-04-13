using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Mappers.Companies;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.Service.Mappers.ProductCatalog
{
    public class ProductDetailDTOMapper : BaseMapper<ProductDetailsDTO, ProductEntity>
    {
        public override Task<ProductDetailsDTO> Map(ProductEntity Product)
        {
            var _getCompany = new CompanyDTOMapper();
            var _getProductType = new ProductTypeDTOMapper();
            var productMapper = new ProductDTOMapper();


            ProductDetailsDTO dto = new ProductDetailsDTO()
            {
                Code = Product.Codigo,
                Name = Product.Nombre,
                Amount = Product.Cantidad,
                ProductType = (Product.CoreProductCatalogServiciosTipos != null) ? _getProductType.Map(Product.CoreProductCatalogServiciosTipos).Result : null,
                IsPack = Product.EsPack,
                LinkedProducts = new System.Collections.Generic.List<ProductDTO>()
            };

            if (Product.ServiciosVinculados != null && Product.ServiciosVinculados.ToList().Count > 0)
            {
                foreach (var productLinked in Product.ServiciosVinculados)
                {
                    dto.LinkedProducts.Add(productMapper.Map(productLinked).Result);
                }
            }

            return Task.FromResult(dto);
        }
    }
}
