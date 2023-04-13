using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TreeCore.Shared.DTO.WorkOrders
{
    public class WorkOrderLifecycleStatusDTO : BaseDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public bool Default { get; set; }
        [Required]
        public string Colour { get; set; }

        public WorkOrderLifecycleStatusDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(Default).ToLower(), "defecto");
            map.Add(nameof(Colour).ToLower(), "color");
        }
    }
}
