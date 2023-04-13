using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TreeCore.Shared.DTO.Contracts
{
    public class ContractHistoryDTO : BaseDTO
    {


        
        [Required] public DateTime CreationDate { get; set; }
        [Required] public bool Active { get; set; }
        [Required] public ContractDTO Contract { get; set; }
        public ContractHistoryDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Active).ToLower(), "activo");
            map.Add(nameof(CreationDate).ToLower(), "fechacreacion");
            map.Add(nameof(Contract).ToLower(), "contract");
        }
    }
}