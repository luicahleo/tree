using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.Service.Mappers.ProductCatalog
{
    public class ProductDTOMapper : BaseMapper<ProductDTO, ProductEntity>
    {
        public override Task<ProductDTO> Map(ProductEntity Product)
        {

            ProductDTO dto = new ProductDTO()
            {
                Code = Product.Codigo,
                Name = Product.Nombre,
                Unit = Product.Unidad,
                Amount = Product.Cantidad,
                Description = Product.Descripcion,
                IsPack = Product.EsPack,
                ProductTypeCode = (Product.CoreProductCatalogServiciosTipos != null) ? Product.CoreProductCatalogServiciosTipos.Codigo : ""
            };

            if (Product.ServiciosVinculados != null && Product.ServiciosVinculados.ToList().Count > 0)
            {
                dto.LinkedProducts = new List<string>();
                foreach (var linkproduc in Product.ServiciosVinculados.ToList())
                {
                    dto.LinkedProducts.Add(linkproduc.Codigo);
                }
            }

            if (Product.EntidadesVinculadas != null && Product.EntidadesVinculadas.ToList().Count > 0)
            {
                dto.SupplierCompany = new List<string>();
                foreach (var linkproduc in Product.EntidadesVinculadas.ToList())
                {
                    dto.SupplierCompany.Add(linkproduc.Codigo);
                }
            }
            return Task.FromResult(dto);
        }
    }
}
