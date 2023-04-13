using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TreeCore.Shared.DTO.Validation;

namespace TreeCore.Shared.DTO.General
{
    public class CronDTO : BaseDTO
    {
        [Required]
        [CronValidation(ErrorMessage = "Invalid Cron")]
        public string Cron { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public CronDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Cron).ToLower(), "cronformat");
            map.Add(nameof(StartDate).ToLower(), "fechainicio");
            map.Add(nameof(EndDate).ToLower(), "fechafin");
        }
    }
}
