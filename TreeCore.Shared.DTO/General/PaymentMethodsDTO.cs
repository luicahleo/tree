using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.General
{
    public class PaymentMethodsDTO : BaseDTO
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
        public bool RequiresBankAccount { get; set; }

        public PaymentMethodsDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigoMetodoPago");
            map.Add(nameof(Name).ToLower(), "metodoPago");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(Default).ToLower(), "defecto");
            map.Add(nameof(RequiresBankAccount).ToLower(), "requiereBanco");
        }
    }
}
