using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.Companies
{
    public class CompanyDetailsDTO : BaseDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Alias { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public string CompanyTypeCode { get; set; }
        
        [Required]
        public string CompanyTypeName { get; set; }
        [Required]
        public string CompanyTypeDescription { get; set; }
        
        public string TaxpayerTypeCode { get; set; }
        public string TaxpayerTypeName { get; set; }
        public string TaxpayerTypeDescription { get; set; }



        public CompanyDetailsDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Alias).ToLower(), "alias");
            map.Add(nameof(Phone).ToLower(), "telefono");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(CompanyTypeCode).ToLower(), "entidadtipocodigo");
            map.Add(nameof(CompanyTypeName).ToLower(), "entidadtiponombre");
            map.Add(nameof(CompanyTypeDescription).ToLower(), "entidadtipodescripcion");
            map.Add(nameof(TaxpayerTypeCode).ToLower(), "contribuyentetipocodigo");
            map.Add(nameof(TaxpayerTypeName).ToLower(), "contribuyentetiponombre");
            map.Add(nameof(TaxpayerTypeDescription).ToLower(), "contribuyentetipodescripcion");
        }
    }
}
