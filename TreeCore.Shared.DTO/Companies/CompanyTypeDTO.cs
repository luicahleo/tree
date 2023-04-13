using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.Companies
{
    public class CompanyTypeDTO : BaseDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public bool Default { get; set; }

        public CompanyTypeDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "entidadtipo");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(Default).ToLower(), "defecto");
        }
    }
}
