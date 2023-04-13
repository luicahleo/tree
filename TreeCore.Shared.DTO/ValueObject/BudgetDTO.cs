using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using TreeCore.Shared.DTO.General;

namespace TreeCore.Shared.DTO.ValueObject
{
    public class BudgetDTO : BaseDTO
    {
        [Required]
        public string CurrencyCode { get; set; }
        [JsonIgnore]
        public CurrencyDTO CurrencyDTO { get; set; }
        [Required]
        public float Value { get; set; }

        public BudgetDTO()
        {
            map = new Dictionary<string, string>();
            //map.Add(nameof(Document).ToLower(), "documento");
        }
    }
}
