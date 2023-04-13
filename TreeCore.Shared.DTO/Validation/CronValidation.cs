using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using NCrontab.Advanced;

namespace TreeCore.Shared.DTO.Validation
{
    class CronValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return (CrontabSchedule.TryParse(value.ToString()) != null);
        }
    }
}
