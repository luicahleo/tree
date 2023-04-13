using System;
using System.Collections.Generic;

namespace TreeCore.Shared.DTO.General
{
    public class CountryDTO : BaseDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Default { get; set; }

        public CountryDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "paiscod");
            map.Add(nameof(Name).ToLower(), "pais");
            map.Add(nameof(Default).ToLower(), "defecto");
        }
    }
}