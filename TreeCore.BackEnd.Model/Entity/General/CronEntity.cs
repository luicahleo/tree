using System;

namespace TreeCore.BackEnd.Model.Entity.General
{
    public class CronEntity : BaseEntity
    {
        public readonly int? CoreServicioFrecuenciaID;
        public readonly string Nombre;
        public readonly string CronFormat;
        public readonly bool Activo;
        public readonly DateTime FechaInicio;
        public readonly DateTime? FechaFin;
        public readonly string TipoFrecuencia;

        public CronEntity(int? coreServicioFrecuenciaID, string nombre, string cronFormat, bool activo, DateTime fechaInicio, DateTime? fechaFin, string tipoFrecuencia)
        {
            CoreServicioFrecuenciaID = coreServicioFrecuenciaID;
            Nombre = nombre;
            CronFormat = cronFormat;
            Activo = activo;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            TipoFrecuencia = tipoFrecuencia;
        }

        public CronEntity(string nombre, string cronFormat, DateTime fechaInicio, DateTime? fechaFin)
        {
            Nombre = nombre;
            CronFormat = cronFormat;
            Activo = true;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            TipoFrecuencia = "";
        }

        public CronEntity(int? coreServicioFrecuenciaID, string nombre, string cronFormat, DateTime fechaInicio, DateTime? fechaFin)
        {
            CoreServicioFrecuenciaID = coreServicioFrecuenciaID;
            Nombre = nombre;
            CronFormat = cronFormat;
            Activo = true;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            TipoFrecuencia = "";
        }

        protected CronEntity() { }

        public static CronEntity UpdateId(CronEntity cron, int id) =>
            new CronEntity(id, cron.Nombre, cron.CronFormat, cron.Activo, cron.FechaInicio, cron.FechaFin, cron.TipoFrecuencia);
    }
}