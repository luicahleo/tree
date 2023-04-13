using log4net;
using NCrontab;
using System;
using System.Reflection;

namespace TreeCore
{
    public static class Cron
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region SiguienteFechaEsHoy
        public static bool SiguienteFechaEsHoy(string CronFormat, DateTime FechaInicio, DateTime? FechaFin)
        {
            bool result = false;
            DateTime today = DateTime.Today;
            DateTime yesterday = DateTime.Today.AddDays(-1);
            DateTime occurrence;

            try
            {
                if (FechaInicio <= today && (!FechaFin.HasValue || today <= FechaFin.Value))
                {
                    if(CronFormat != "_")
                    {
                        CrontabSchedule s = CrontabSchedule.Parse(CronFormat);

                        if (FechaFin.HasValue)
                        {
                            occurrence = s.GetNextOccurrence(yesterday, FechaFin.Value);
                        }
                        else
                        {
                            occurrence = s.GetNextOccurrence(yesterday);
                        }

                        if (occurrence == today)
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
                log.Error(ex);
                result = false;
            }
            return result;
        }
        #endregion

    }
}