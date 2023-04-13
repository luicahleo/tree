using System;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
namespace TreeCore.BackEnd.Model.Entity.Contracts
{
    public class ContractLineEntidadEntity : BaseEntity
    {
        public readonly int? AlquilerDetalleEntidadID;
        public ContractLineEntity oAlquilerDetalle;
        public BankAccountEntity oEntidadCuentaBankaria;
        public CurrencyEntity oMoneda;
        public CompanyAssignedPaymentMethodsEntity oMetodoPagoEntidad;
        public double CantidadPorcentaje;




        public ContractLineEntidadEntity(int? alquilerDetalleEntidadID, ContractLineEntity oalquilerDetalle, 
            CurrencyEntity omoneda, BankAccountEntity oentidadCuentaBankaria,
            CompanyAssignedPaymentMethodsEntity ometodoPagoEntidad, double cantidadPorcentaje)
        {
            AlquilerDetalleEntidadID = alquilerDetalleEntidadID;
            oAlquilerDetalle = oalquilerDetalle;
            oEntidadCuentaBankaria = oentidadCuentaBankaria;
            oMetodoPagoEntidad = ometodoPagoEntidad;
            oMoneda = omoneda;
            CantidadPorcentaje = cantidadPorcentaje;
        }

        protected ContractLineEntidadEntity()
        {
        }

        public static ContractLineEntidadEntity Create(int? id, ContractLineEntity oalquilerDetalle,
            CurrencyEntity omoneda, BankAccountEntity oentidadCuentaBankaria,
            CompanyAssignedPaymentMethodsEntity ometodoPagoEntidad, double cantidadPorcentaje)
            => new ContractLineEntidadEntity(id,  oalquilerDetalle,
             omoneda,  oentidadCuentaBankaria,
             ometodoPagoEntidad, cantidadPorcentaje);

        public static ContractLineEntidadEntity UpdateId(ContractLineEntidadEntity contractLineEntidad, int id) =>
            new ContractLineEntidadEntity(id, contractLineEntidad.oAlquilerDetalle,
            contractLineEntidad.oMoneda, contractLineEntidad.oEntidadCuentaBankaria,
            contractLineEntidad.oMetodoPagoEntidad, contractLineEntidad.CantidadPorcentaje);
    }
}

