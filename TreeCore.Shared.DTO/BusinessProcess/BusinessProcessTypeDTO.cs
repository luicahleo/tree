using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.BusinessProcess
{
    public class BusinessProcessTypeDTO : BaseDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public bool Active { get; set; }

        public string Description { get; set; }
       
        [Required]
        public bool Default { get; set; }


        public BusinessProcessTypeDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(Default).ToLower(), "defecto");
        }
    }
}
