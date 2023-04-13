using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCore.Shared.DTO.Query
{
    public class FilterDTO
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        //public string Linked { get; set; }
        public List<FilterDTO> Filters { get; set; }

        public FilterDTO()
        {
        }

        public FilterDTO(string field, string @operator, string value, string linked)
        {
            Field = field;
            Operator = @operator;
            Value = value;
            //Linked = linked;
        }
    }
}
