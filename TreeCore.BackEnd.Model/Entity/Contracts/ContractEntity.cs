using System;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.Sites;
using TreeCore.BackEnd.Model.ValueObject;
using System.Collections.Generic;


namespace TreeCore.BackEnd.Model.Entity.Contracts
{
    public class ContractEntity : BaseEntity
    {
        public readonly int? AlquilerID;
        public readonly string NumContrato;
        public readonly string NombreContrato;
        public ContractStatusEntity oAlquilerEstado;
        public ContractGroupEntity oAlquilerTipoContratacion;
        public SiteEntity oSite;
        public CurrencyEntity oMoneda;
        public ContractTypeEntity oAlquilerTipoContrato;
        public readonly string ComentariosGenerales;
        public readonly int? ContratoMarcoID;
        public readonly DateTime FechaFirmaContrato;
        public readonly DateTime FechaInicioContrato;
        public readonly DateTime FechaFinContrato;
        public readonly int Cadencias;
        public readonly DateTime FechaFinContratoAuxiliar;
        public readonly bool ExpirarAVencimiento;

        public readonly string Propietario;
        public RenewalClause oAjusteRenovaciones;
        public readonly bool TerminacionPrevistaPrimeraVentana;
        public IEnumerable<ContractLineEntity> oAlquileresDetalles;
        public DateTime? FechaCreacion;
        public DateTime? FechaModificacion;
        public UserEntity CreadoPor;
        public UserEntity ModificadoPor;
        public ContractHistoryEntity HistorialContrato;
        






        public ContractEntity(int? AlquilerID,
                               string NumContrato,
                               string NombreContrato, 
                               ContractStatusEntity oAlquilerEstado,
                               ContractGroupEntity oAlquilerTipoContratacion,
                               SiteEntity oSite,
                               CurrencyEntity oMoneda,
                               ContractTypeEntity oAlquilerTipoContrato,
                               string ComentariosGenerales,
                               int? ContratoMarcoID,
                               DateTime FechaFirmaContrato,
                               DateTime FechaInicioContrato,
                               DateTime FechaFinContrato,
                               int Cadencias,
                               DateTime FechaFinContratoAuxiliar,
                               bool ExpirarAVencimiento,
                               string Propietario,RenewalClause oAjusteRenovaciones,bool terminacionPrevistaPrimeraVentana, IEnumerable<ContractLineEntity> oAlquileresDetalles,
                               DateTime? FechaCreacion, DateTime? FechaModificacion, UserEntity CreadoPor, UserEntity ModificadoPor, ContractHistoryEntity HistorialContrato)
        {
            this.AlquilerID = AlquilerID;
            this.oAlquilerTipoContratacion = oAlquilerTipoContratacion;
            this.NumContrato = NumContrato;
            this.NombreContrato = NombreContrato;
            this.oAlquilerEstado = oAlquilerEstado;
            this.oSite = oSite;
            this.oMoneda = oMoneda;
            this.oAlquilerTipoContrato = oAlquilerTipoContrato;
            this.ComentariosGenerales = ComentariosGenerales;
            this.ContratoMarcoID = ContratoMarcoID;
            this.ComentariosGenerales = ComentariosGenerales;
            this.FechaFirmaContrato = FechaFirmaContrato;
            this.FechaInicioContrato = FechaInicioContrato;
            this.FechaFinContrato = FechaFinContrato;
            this.Cadencias = Cadencias;
            this.FechaFinContratoAuxiliar = FechaFinContratoAuxiliar;
            this.ExpirarAVencimiento = ExpirarAVencimiento;
            this.Propietario = Propietario;
            this.oAjusteRenovaciones = oAjusteRenovaciones;
            this.TerminacionPrevistaPrimeraVentana = terminacionPrevistaPrimeraVentana;
            this.oAlquileresDetalles = oAlquileresDetalles;
            this.FechaCreacion = FechaCreacion;
            this.FechaModificacion = FechaModificacion;
            this.CreadoPor = CreadoPor;
            this.ModificadoPor = ModificadoPor;
            this.HistorialContrato = HistorialContrato;

        }

        protected ContractEntity()
        {
           
        }

        public static ContractEntity Create(int? AlquilerID,
                               string NumContrato,
                               string NombreContrato,
                               ContractStatusEntity oAlquilerEstado,
                               ContractGroupEntity oAlquilerTipoContratacion,
                               SiteEntity oSite,
                               CurrencyEntity oMoneda,
                               ContractTypeEntity oAlquilerTipoContrato,
                               string ComentariosGenerales,
                               int? ContratoMarcoID,
                               DateTime FechaFirmaContrato,
                               DateTime FechaInicioContrato,
                               DateTime FechaFinContrato,
                               int Cadencias,
                               DateTime FechaFinContratoAuxiliar,
                               bool ExpirarAVencimiento,
                               string Propietario,
                               RenewalClause oAjusteRenovaciones,
                               bool TerminacionPrevistaPrimeraVentana,
                               IEnumerable<ContractLineEntity> oAlquileresDetalles,
                               DateTime? FechaCreacion, DateTime? FechaModificacion, UserEntity CreadoPor, UserEntity ModificadoPor, ContractHistoryEntity HistorialContrato)
            => new ContractEntity( AlquilerID,  NumContrato,  NombreContrato, oAlquilerEstado, oAlquilerTipoContratacion,
                oSite, oMoneda, oAlquilerTipoContrato,  ComentariosGenerales,ContratoMarcoID,
                FechaFirmaContrato,  FechaInicioContrato,  FechaFinContrato,Cadencias,  FechaFinContratoAuxiliar, ExpirarAVencimiento,Propietario, oAjusteRenovaciones,TerminacionPrevistaPrimeraVentana, oAlquileresDetalles,
                 FechaCreacion, FechaModificacion, CreadoPor, ModificadoPor,  HistorialContrato);
        public static ContractEntity UpdateId(ContractEntity Contract, int id) =>
            new ContractEntity(id, Contract.NumContrato, Contract.NombreContrato, Contract.oAlquilerEstado, Contract.oAlquilerTipoContratacion,
                Contract.oSite, Contract.oMoneda, Contract.oAlquilerTipoContrato, Contract.ComentariosGenerales, Contract.ContratoMarcoID,
                Contract.FechaFirmaContrato, Contract.FechaInicioContrato, Contract.FechaFinContrato, Contract.Cadencias, Contract.FechaFinContratoAuxiliar,Contract.ExpirarAVencimiento,Contract.Propietario,Contract.oAjusteRenovaciones,Contract.TerminacionPrevistaPrimeraVentana,Contract.oAlquileresDetalles,
                Contract.FechaCreacion, Contract.FechaModificacion, Contract.CreadoPor, Contract.ModificadoPor, Contract.HistorialContrato);

    }
}
