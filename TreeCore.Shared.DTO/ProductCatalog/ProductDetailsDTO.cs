using System.Collections.Generic;
using TreeCore.Shared.DTO.Companies;

namespace TreeCore.Shared.DTO.ProductCatalog
{
    public class ProductDetailsDTO : BaseDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
        public ProductTypeDTO ProductType { get; set; }
        public bool IsPack { get; set; }
        public List<ProductDTO> LinkedProducts { get; set; }

        public ProductDetailsDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Amount).ToLower(), "cantidad");
            map.Add(nameof(ProductType).ToLower(), "coreproductcatalogserviciotipoid");
            map.Add(nameof(IsPack), "espack");
            map.Add(nameof(LinkedProducts).ToLower(), "productosAsignados");
        }
    }
}
