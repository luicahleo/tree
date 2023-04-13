using System;

namespace TreeCore.BackEnd.Model.Entity.General
{
    public class CurrencyEntity : BaseEntity
    {
        public readonly int? MonedaID;
        public readonly int? ClienteID;
        public readonly string Moneda;
        public readonly string Simbolo;
        public readonly float CambioDollarUS;
        public readonly float CambioEuro;
        public readonly DateTime? FechaActualizacion;
        public readonly bool Activo;
        public readonly bool Defecto;

        public CurrencyEntity(int? monedaID, int? clienteID, string moneda, string simbolo, float cambioDollarUS, float cambioEuro, DateTime? fechaActualizacion, bool activo, bool defecto)
        {
            MonedaID = monedaID;
            ClienteID = clienteID;
            Moneda = moneda ?? throw new ArgumentNullException(nameof(moneda)); ;
            Simbolo = simbolo ?? throw new ArgumentNullException(nameof(simbolo)); ;
            CambioDollarUS = cambioDollarUS;
            CambioEuro = cambioEuro;
            FechaActualizacion = fechaActualizacion;
            Activo = activo;
            Defecto = defecto;
        }

        public CurrencyEntity() { }

        public static CurrencyEntity UpdateId(CurrencyEntity currency, int id) =>
            new CurrencyEntity(id, null, currency.Moneda, currency.Simbolo, currency.CambioDollarUS, currency.CambioEuro, currency.FechaActualizacion, currency.Activo, currency.Defecto);
    }
}