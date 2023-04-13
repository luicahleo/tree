using System;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.Sites;
using TreeCore.BackEnd.Model.ValueObject;
using System.Collections.Generic;


namespace TreeCore.BackEnd.Model.Entity.Contracts
{
    public class ContractLineEntity : BaseEntity
    {
        public readonly int? AlquilerDetalleID;
        public readonly string CodigoDetalle;
        public ContractEntity oAlquiler;
        public ContractLineTypeEntity oAlquilerConcepto;
        public readonly int Periodicidad;
        public readonly float Importe;
        public CurrencyEntity oMoneda;
        public readonly string Descripcion;
        public readonly DateTime FechaPrimerPago;
        public readonly DateTime FechaUltimoPago;
        public readonly DateTime FechaProximoPago;
        public readonly int NumeroCuotas;
        public readonly float TotalImporte;
        public readonly float TotalConProrrogas;
        public readonly bool AplicaProrrogaAutomatica;
        public readonly bool Prorrateo;
        public readonly bool PagoAnticipado;
        public PriceReadjustment oReajuste;
        public IEnumerable<ContractLineTaxesEntity> oAlquileresDetallesImpuestos;
        public IEnumerable<ContractLineEntidadEntity> oAlquileresDetallesEntidades;








        public ContractLineEntity(int? AlquilerDetalleID,
                               string CodigoDetalle,
                               ContractEntity oAlquiler,
                               ContractLineTypeEntity oAlquilerConcepto,
                               int Periodicidad,
                               float Importe,
                               CurrencyEntity oMoneda,
                               string Descripcion,
                               DateTime FechaPrimerPago,
                               DateTime FechaUltimoPago,
                               DateTime FechaProximoPago,
                               int NumeroCuotas,
                               float TotalImporte,
                               float TotalConProrrogas,
                               bool AplicaProrrogaAutomatica,
                               bool Prorrateo,
                               bool PagoAnticipado,
                               PriceReadjustment oReajuste,
                               IEnumerable<ContractLineTaxesEntity> oAlquileresDetallesImpuestos,
                               IEnumerable<ContractLineEntidadEntity> oAlquileresDetallesEntidades)
        {
            this.AlquilerDetalleID = AlquilerDetalleID;
            this.CodigoDetalle = CodigoDetalle;
            this.oAlquiler = oAlquiler;
            this.oAlquilerConcepto = oAlquilerConcepto;
            this.Periodicidad = Periodicidad;
            this.Importe = Importe;
            this.oMoneda = oMoneda;
            this.Descripcion = Descripcion;
            this.FechaPrimerPago = FechaPrimerPago;
            this.FechaUltimoPago = FechaUltimoPago;
            this.FechaProximoPago = FechaProximoPago;
            this.NumeroCuotas = NumeroCuotas;
            this.TotalImporte = TotalImporte;
            this.TotalConProrrogas = TotalConProrrogas;
            this.AplicaProrrogaAutomatica = AplicaProrrogaAutomatica;
            this.Prorrateo = Prorrateo;
            this.PagoAnticipado = PagoAnticipado;
            this.oReajuste = oReajuste;
            this.oAlquileresDetallesImpuestos = oAlquileresDetallesImpuestos;
            this.oAlquileresDetallesEntidades = oAlquileresDetallesEntidades;


        }

        protected ContractLineEntity()
        {

        }

        public static ContractLineEntity Create(int? AlquilerDetalleID,
                               string codigoDetalle,
                               ContractEntity oAlquiler,
                               ContractLineTypeEntity oAlquilerConcepto,
                               int Periodicidad,
                               float Importe,
                               CurrencyEntity oMoneda,
                               string Descripcion,
                               DateTime FechaPrimerPago,
                               DateTime FechaUltimoPago,
                               DateTime FechaProximoPago,
                               int NumeroCuotas,
                               float TotalImporte,
                               float TotalConProrrogas,
                               bool AplicaProrrogaAutomatica,
                               bool Prorrateo,
                               bool PagoAnticipado,
                               PriceReadjustment oReajuste, IEnumerable<ContractLineTaxesEntity> oAlquileresDetallesImpuestos, 
                                                            IEnumerable<ContractLineEntidadEntity> oAlquileresDetallesEntidades) => new ContractLineEntity(AlquilerDetalleID, codigoDetalle,
                                                                                                    oAlquiler,
                                                                                                    oAlquilerConcepto,
                                                                                                    Periodicidad,
                                                                                                    Importe,
                                                                                                    oMoneda,
                                                                                                    Descripcion,
                                                                                                    FechaPrimerPago,
                                                                                                    FechaUltimoPago,
                                                                                                    FechaProximoPago,
                                                                                                    NumeroCuotas,
                                                                                                    TotalImporte,
                                                                                                    TotalConProrrogas,
                                                                                                    AplicaProrrogaAutomatica,
                                                                                                    Prorrateo,
                                                                                                    PagoAnticipado, oReajuste,oAlquileresDetallesImpuestos,oAlquileresDetallesEntidades);
        public static ContractLineEntity UpdateId(ContractLineEntity Contractline, int id) => new ContractLineEntity(id, Contractline.CodigoDetalle,
                                                                                                    Contractline.oAlquiler,
                                                                                                    Contractline.oAlquilerConcepto,
                                                                                                    Contractline.Periodicidad,
                                                                                                    Contractline.Importe,
                                                                                                    Contractline.oMoneda,
                                                                                                    Contractline.Descripcion,
                                                                                                    Contractline.FechaPrimerPago,
                                                                                                    Contractline.FechaUltimoPago,
                                                                                                    Contractline.FechaProximoPago,
                                                                                                    Contractline.NumeroCuotas,
                                                                                                    Contractline.TotalImporte,
                                                                                                    Contractline.TotalConProrrogas,
                                                                                                    Contractline.AplicaProrrogaAutomatica,
                                                                                                    Contractline.Prorrateo,
                                                                                                    Contractline.PagoAnticipado,
                                                                                                    Contractline.oReajuste,
                                                                                                    Contractline.oAlquileresDetallesImpuestos,
                                                                                                    Contractline.oAlquileresDetallesEntidades);

    }
}
