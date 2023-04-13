using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.Config
{
    public class ColumnDTO : BaseDTO
    {
        [Required]
        public int Order { get; set; }
        [Required]
        public string ColumnName { get; set; }
        [Required]
        public string Column { get; set; }
        [Required]
        public bool Visible { get; set; }

        public ColumnDTO()
        {
            map = new Dictionary<string, string>();
        }
    }
}
