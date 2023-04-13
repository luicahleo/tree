using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.ValueObject
{
    public class AddressDTO : BaseDTO
    {

        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string PostalCode { get; set;}
        [Required]
        public string Locality { get; set;}
        [Required]
        public string Sublocality { get; set;}
        [Required]
        public string Country { get; set; }

        public AddressDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Address1).ToLower(), "direccionjson");
            map.Add(nameof(Address2).ToLower(), "direccionjson");
            map.Add(nameof(PostalCode).ToLower(), "direccionjson");
            map.Add(nameof(Locality).ToLower(), "direccionjson");
            map.Add(nameof(Sublocality).ToLower(), "direccionjson");
            map.Add(nameof(Country).ToLower(), "direccionjson");

        }
    }
}
