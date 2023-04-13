using System.Collections.Generic;

namespace TreeCore.Shared.DTO.ProductCatalog
{
    public class CatalogAssignedProductsDTO : BaseDTO
    {
        public string ProductCode { get; set; }
        public float Price { get; set; }

        public CatalogAssignedProductsDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(ProductCode).ToLower(), "coreproductcatalogservicioid");
            map.Add(nameof(Price).ToLower(), "precio");
        }
    }
}
