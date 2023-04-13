using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ValueObject;

namespace TreeCore.Shared.DTO.ImportExport
{
    public class ImportTaskDTO : BaseDTO
    {
        [Required] public string Code { get; set; }
        [Required] public FileDTO Document { get; set; }
        [Required] public string Type { get; set; }
        [Required] public DateTime UploadDate { get; set; }
        [Required] public DateTime ImportDate { get; set; }
        public bool Processed { get; set; }
        public bool Success { get; set; }
        public string LogFile { get; set; }

        public ImportTaskDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "documentocarga");
            map.Add(nameof(Document).ToLower(), "rutadocumento");
            map.Add(nameof(Type).ToLower(), "documentocargaplantilla");
            map.Add(nameof(UploadDate).ToLower(), "fechasubida");
            map.Add(nameof(ImportDate).ToLower(), "fechaestimadasubida");
            map.Add(nameof(Processed).ToLower(), "procesado");
            map.Add(nameof(Success).ToLower(), "exito");
            map.Add(nameof(LogFile).ToLower(), "rutalog");
        }
    }
}
