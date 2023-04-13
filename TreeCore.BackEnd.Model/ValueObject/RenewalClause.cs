using System;
using System.Collections.Generic;
using TreeCore.BackEnd.Model.Entity.General;

namespace TreeCore.BackEnd.Model.ValueObject
{
    public class RenewalClause : BaseValueObject
    {
        public bool ProrrogaAutomatica { get; set; }
        public int? CadenciaProrrogaAutomatica { get; set; }
        public int? NumProrrogasMaximas { get; set; }

        public int? ProrrogasConsumidas { get; set; }
        public int? NumdiasNotificacion { get; set; }
        public DateTime? FechaFinContratoAuxiliar { get; set; }
        public DateTime? FechaNotificacionRenovacion { get; set; }

        public DateTime? FechaEfectivaFinContrato { get; set; }




        public RenewalClause(bool tipo, int? cadenciaprorroga, int? totalprorrogas, int? prorrogasConsumidas, int? diasNotificacion,
            DateTime? fechaProrroga, DateTime? fechaNotificacionProrroga, DateTime? fechaEfectivaFinContrato)
        {
            ProrrogaAutomatica = tipo;
            CadenciaProrrogaAutomatica = cadenciaprorroga;
            NumProrrogasMaximas = totalprorrogas;
            ProrrogasConsumidas = prorrogasConsumidas;
            NumdiasNotificacion = diasNotificacion;
            FechaFinContratoAuxiliar = fechaProrroga;
            FechaNotificacionRenovacion = fechaNotificacionProrroga;
            FechaEfectivaFinContrato = fechaEfectivaFinContrato;


        }
        protected RenewalClause() { }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ProrrogaAutomatica;
            yield return CadenciaProrrogaAutomatica;
            yield return NumProrrogasMaximas;
            yield return ProrrogasConsumidas;
            yield return NumdiasNotificacion;
            yield return FechaFinContratoAuxiliar;
            yield return FechaNotificacionRenovacion;
            yield return FechaEfectivaFinContrato;


        }
    }
}
