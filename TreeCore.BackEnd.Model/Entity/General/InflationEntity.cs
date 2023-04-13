using System;

namespace TreeCore.BackEnd.Model.Entity.General
{
    public class InflationEntity : BaseEntity
    {
        public readonly int? InflacionID;
        public readonly string Inflacion;
        public readonly string Codigo;
        public readonly string Descripcion;
        public readonly bool Estandar;
        public readonly bool Activo;
        public CountryEntity Paises;

        public InflationEntity(int? inflacionID, string inflacion, string codigo, string descripcion, bool estandar, bool activo, CountryEntity paises)
        {
            InflacionID = inflacionID;
            Inflacion = inflacion ?? throw new ArgumentNullException(nameof(inflacion)); ;
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo)); ;
            Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion)); ;
            Estandar = estandar;
            Activo = activo;
            Paises = paises;
        }

        protected InflationEntity() { }

        public static InflationEntity UpdateId(InflationEntity Inflation, int id) =>
            new InflationEntity(id, Inflation.Inflacion, Inflation.Codigo, Inflation.Descripcion, Inflation.Estandar, Inflation.Activo, Inflation.Paises);
    }
}
