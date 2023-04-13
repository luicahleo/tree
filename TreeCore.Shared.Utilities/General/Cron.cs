using NCrontab;
using System;
using System.Reflection;

namespace TreeCore
{
    public static class Cron
    {

        public static DateTime getSiguienteFecha(string CronFormat)
        {
            try
            {
                CrontabSchedule s = CrontabSchedule.Parse(CronFormat);
                return s.GetNextOccurrence(DateTime.Now);
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }

        #region SiguienteFechaEsHoy
        public static bool SiguienteFechaEsHoy(string CronFormat, DateTime FechaInicio, DateTime? FechaFin)
        {
            bool result = false;
            DateTime today = DateTime.Today;
            DateTime yesterday = DateTime.Today.AddDays(-1);
            DateTime occurrence;

            try
            {
                if (FechaInicio.Date <= today && (!FechaFin.HasValue || today <= FechaFin.Value))
                {
                    if(CronFormat != "_")
                    {
                        CrontabSchedule s = CrontabSchedule.Parse(CronFormat);

                        if (FechaFin.HasValue)
                        {
                            occurrence = s.GetNextOccurrence(today, FechaFin.Value);
                        }
                        else
                        {
                            occurrence = s.GetNextOccurrence(today);
                        }

                        if (occurrence.Date == today)
                        {
                            result = true;
                        }
                    }
                    else if (FechaInicio == today)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        #endregion

    }
}