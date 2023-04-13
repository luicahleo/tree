using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.General
{
    public class CurrencyDTO : BaseDTO
    {
        [Required] public string Code { get; set; }
        public string Symbol { get; set; }
        public float DollarChange { get; set; }
        public float EuroChange { get; set; }
        [Required] public bool Active { get; set; }
        [Required] public bool Default { get; set; }

        public CurrencyDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "moneda");
            map.Add(nameof(Symbol).ToLower(), "simbolo");
            map.Add(nameof(DollarChange).ToLower(), "cambiodollarus");
            map.Add(nameof(EuroChange).ToLower(), "cambioeuro");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(Default).ToLower(), "defecto");
        }
    }
}
