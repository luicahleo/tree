using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TreeCore.Shared.DTO.Inventory
{
    public class InventoryDTO:BaseDTO
    {
        [Required]
        public string Code { get; set; }
        

        public InventoryDTO()
        {
            map = new Dictionary<string, string>();
            map.Add("Code", "Codigo");
           
           
        }
    }
}
