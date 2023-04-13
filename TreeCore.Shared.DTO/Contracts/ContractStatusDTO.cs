using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TreeCore.Shared.DTO.Contracts
{
    public class ContractStatusDTO:BaseDTO
    {

        [Required] public string Code { get; set; }
       
        [Required] public string Name { get; set; }
       
        [Required] public string Description { get; set; }

        public bool Active { get; set; }

        [Required] public bool Default { get; set; }


       

       
        public ContractStatusDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "Estado");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(Default).ToLower(), "defecto");
          
            
        }
    }
}
