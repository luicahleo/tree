using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeCore.Shared.DTO.ImportExport
{
    public class ImportTypeDTO : BaseDTO
    {
        [Required] public string Code { get; set; }
        [Required] public string Document { get; set; }

        public ImportTypeDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "documentocarga");
            map.Add(nameof(Document).ToLower(), "rutadocumento");
        }
    }
}
