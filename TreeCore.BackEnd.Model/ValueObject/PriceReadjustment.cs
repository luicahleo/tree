using System;
using System.Collections.Generic;
using TreeCore.BackEnd.Model.Entity.General;

namespace TreeCore.BackEnd.Model.ValueObject
{
    public class PriceReadjustment : BaseValueObject
    {
        public string Tipo;
        public InflationEntity Inflacion;
        public float? CantidadFija;
        public float? PorcentajeFijo;
        public float? Periodicidad;
        public DateTime? FechaFinReajuste;
        public DateTime? FechaInicioReajuste;
        public DateTime? FechaProximaReajuste; 

        public PriceReadjustment(string tipo, InflationEntity inflacion, float? cantidadFija, float? porcentajeFijo, 
            float? periodicidad, DateTime? fechaFin, DateTime? fechaInicio, DateTime? fechaProxima)
        {
            Tipo = tipo;
            Inflacion = inflacion;
            CantidadFija = cantidadFija;
            PorcentajeFijo = porcentajeFijo;
            Periodicidad = periodicidad;
            FechaFinReajuste = fechaFin;
            FechaInicioReajuste = fechaInicio;
            FechaProximaReajuste = fechaProxima;
        }

        protected PriceReadjustment() { }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Tipo;
            yield return Inflacion;
            yield return CantidadFija;
            yield return PorcentajeFijo;
            yield return Periodicidad;
            yield return FechaFinReajuste;
            yield return FechaInicioReajuste;
            yield return FechaProximaReajuste;
        }
    }
}
