using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeCore.Shared.DTO.ValueObject;

namespace TreeCore.Shared.DTO.Companies
{
    public class CompanyAddressDTO : BaseDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Default { get; set; }

        public AddressDTO Address { get; set; }

        public CompanyAddressDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Default).ToLower(), "defecto");
            map.Add(nameof(Address).ToLower(), "direccionjson");
        }
    }
}
