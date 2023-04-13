using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TreeCore.Shared.DTO.Sites
{
    public class SiteDTO:BaseDTO
    {
        [Required]
        public string Code { get; set; }
        

        public SiteDTO()
        {
            map = new Dictionary<string, string>();
            map.Add("Code", "Codigo");
           
           
        }
    }
}
