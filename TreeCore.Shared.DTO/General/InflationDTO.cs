using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TreeCore.Shared.DTO.General
{
    public class InflationDTO : BaseDTO
    {
        [Required] public string Code { get; set; }
        [Required] public string Name { get; set; }
        [Required] public bool Estandar { get; set; }
        public string Description { get; set; }
        public string CountryName { get; set; }
        public bool Active { get; set; }

        public InflationDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "inflacion");
            map.Add(nameof(Estandar).ToLower(), "estandar");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(CountryName).ToLower(), "paisid");
        }
    }
}
