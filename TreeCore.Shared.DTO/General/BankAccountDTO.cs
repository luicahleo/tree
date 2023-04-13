using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.General
{
    public class BankAccountDTO : BaseDTO
    {
        [Required]
        public string BankCode { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string IBAN { get; set; }
        
        public string Description { get; set; }
        [Required]
        public string SWIFT { get; set; }

        public BankAccountDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(BankCode).ToLower(), "bancoid");
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(IBAN).ToLower(), "iban");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(SWIFT).ToLower(), "swift");
        }
    }
}
