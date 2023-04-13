using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TreeCore.Shared.DTO.Contracts
{
    public class ContractLineTypeDTO : BaseDTO
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
        [Required]
        public bool Single { get; set; }
        [Required]
        public bool Recurrent { get; set; }
        [Required]
        public bool Income { get; set; }

        public ContractLineTypeDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "alquilerconcepto");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(Default).ToLower(), "defecto");
            map.Add(nameof(Single).ToLower(), "espagounico");
            map.Add(nameof(Recurrent).ToLower(), "esalquilerbase");
            map.Add(nameof(Income).ToLower(), "escobro");
        }
    }
}
