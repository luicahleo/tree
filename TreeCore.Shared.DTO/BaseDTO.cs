using Newtonsoft.Json;
using System.Collections.Generic;

namespace TreeCore.Shared.DTO
{
    public class BaseDTO
    {
        public BaseDTO() { 

        }
        [JsonIgnore]
        public Dictionary<string, string> map;
    }
}
