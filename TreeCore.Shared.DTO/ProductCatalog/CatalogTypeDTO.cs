using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.ProductCatalog
{
    public class CatalogTypeDTO : BaseDTO
    {
        [Required] public string Code { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public bool Active { get; set; }
        [Required] public bool Default { get; set; }
        [Required] public bool IsOffering { get; set; }
        [Required] public bool IsPurchasing { get; set; }

        public CatalogTypeDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(Default).ToLower(), "defecto");
            map.Add(nameof(IsOffering).ToLower(), "esventa");
            map.Add(nameof(IsPurchasing).ToLower(), "escompra");
        }
    }
}
