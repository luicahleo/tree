using System;
using System.Collections.Generic;
using System.Text;

namespace TreeCore.Shared.DTO.ValueObject
{
    public class FileDTO : BaseDTO
    {
        public string Document { get; set; }
        public byte[] DocumentData { get; set; }

        public FileDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Document).ToLower(), "documento");
        }
    }
}
