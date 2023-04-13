using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TreeCore.Shared.DTO.Contracts
{
    public class ContractLineTaxesDTO : BaseDTO
    {


        [Required] public string TaxCode { get; set; }

       
       
       

        public ContractLineTaxesDTO()
        {
            map = new Dictionary<string, string>();
        
            map.Add(nameof(TaxCode).ToLower(), "codigo");
            
            
        }
    }
}
