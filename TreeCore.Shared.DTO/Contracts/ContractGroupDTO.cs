using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TreeCore.Shared.DTO.Contracts
{
    public class ContractGroupDTO : BaseDTO
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

        public ContractGroupDTO()
        {
            map = new Dictionary<string, string>();
            map.Add("code", "codigo");
            map.Add("name", "TipoContratacion");
            map.Add("description", "descripcion");
            map.Add("active", "activo");
            map.Add("default", "defecto");



        }
    }
}
