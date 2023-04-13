using System;
using System.Collections;

namespace TreeCore.BackEnd.API
{
    public class ErrorMessage
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public IEnumerable Error { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
