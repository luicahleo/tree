using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TreeCore.Shared.DTO.General
{
    public class TaxesDTO : BaseDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Valor { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public bool Default { get; set; }

        public TaxesDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "impuesto");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(Valor).ToLower(), "valor");
            map.Add(nameof(CountryCode).ToLower(), "paisid");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(Default).ToLower(), "defecto");
        }
    }
}
