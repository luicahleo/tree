using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TreeCore.Shared.DTO.General
{
    public class AssetDTO : BaseDTO
    {
        [Required]
        public string ObjectCode { get; set; }
        [Required]
        public string AssetCode { get; set; }
        public AssetDTO()
        {
            map = new Dictionary<string, string>();
        }
    }
}
