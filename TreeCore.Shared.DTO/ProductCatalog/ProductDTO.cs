using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.ProductCatalog
{
    public class ProductDTO : BaseDTO
    {
        [Required] public string Code { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Unit { get; set; }
        [Required] public float Amount { get; set; }
        public string Description { get; set; }
        public string ProductTypeCode { get; set; }
        [RegularExpression(@"^(?#second)(\*|(?:\*|(?:[0-9]|(?:[1-5][0-9])))\/(?:[0-9]|(?:[1-5][0-9]))|
(?:[0-9]|(?:[1-5][0-9]))(?:(?:\-[0-9]|\-(?:[1-5][0-9]))?|(?:\,(?:[0-9]|(?:[1-5][0-9])))*)) 
(?#minute)(\*|(?:\*|(?:[0-9]|(?:[1-5][0-9])))\/(?:[0-9]|(?:[1-5][0-9]))|(?:[0-9]|
(?:[1-5][0-9]))(?:(?:\-[0-9]|\-(?:[1-5][0-9]))?|(?:\,(?:[0-9]|(?:[1-5][0-9])))*)) 
(?#hour)(\*|(?:\*|(?:\*|(?:[0-9]|1[0-9]|2[0-3])))\/(?:[0-9]|1[0-9]|2[0-3])|
(?:[0-9]|1[0-9]|2[0-3])(?:(?:\-(?:[0-9]|1[0-9]|2[0-3]))?|(?:\,(?:[0-9]|1[0-9]|2[0-3]))*)) 
(?#day_of_month)(\*|\?|L(?:W|\-(?:[1-9]|(?:[12][0-9])|3[01]))?|(?:[1-9]|(?:[12][0-9])|
3[01])(?:W|\/(?:[1-9]|(?:[12][0-9])|3[01]))?|(?:[1-9]|(?:[12][0-9])|3[01])
(?:(?:\-(?:[1-9]|(?:[12][0-9])|3[01]))?|(?:\,(?:[1-9]|(?:[12][0-9])|3[01]))*)) 
(?#month)(\*|(?:[1-9]|1[012]|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)
(?:(?:\-(?:[1-9]|1[012]|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))?|
(?:\,(?:[1-9]|1[012]|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))*)) 
(?#day_of_week)(\*|\?|[0-6](?:L|\#[1-5])?|(?:[0-6]|SUN|MON|TUE|WED|THU|FRI|SAT)
(?:(?:\-(?:[0-6]|SUN|MON|TUE|WED|THU|FRI|SAT))?|(?:\,(?:[0-6]|SUN|MON|TUE|WED|THU|FRI|SAT))*)) 
(?#year)(\*|(?:[1-9][0-9]{3})(?:(?:\-[1-9][0-9]{3})?|(?:\,[1-9][0-9]{3})*))$",
         ErrorMessage = "Cron format not valid.")]
        public string Frecuency { get; set; }
        public bool IsPack { get; set; }
        public bool Public { get; set; }
        public List<string> LinkedProducts { get; set; }
        public List<string> SupplierCompany { get; set; }

        public ProductDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Unit).ToLower(), "unidad");
            map.Add(nameof(Amount).ToLower(), "cantidad");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(ProductTypeCode).ToLower(), "coreproductcatalogserviciotipoid");
            map.Add(nameof(Frecuency).ToLower(), "cronformat");
            map.Add(nameof(IsPack).ToLower(), "espack");
            map.Add(nameof(Public).ToLower(), "publico");
            map.Add(nameof(LinkedProducts).ToLower(), "productosAsignados");
            map.Add(nameof(SupplierCompany).ToLower(), "entidadesAsignadas");
        }
    }
}
