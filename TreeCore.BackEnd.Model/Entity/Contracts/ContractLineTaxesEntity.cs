using System;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.General;
namespace TreeCore.BackEnd.Model.Entity.Contracts
{
    public class ContractLineTaxesEntity : BaseEntity
    {
        public readonly int? AlquilerDetalleImpuestoID;
        public ContractLineEntity oAlquilerDetalle;
        public TaxesEntity oImpuesto;
        public double? Cantidad;
        



        public ContractLineTaxesEntity(int? alquilerDetalleImpuestoID, ContractLineEntity oalquilerDetalle, TaxesEntity oimpuesto, double? cantidad)
        {
            AlquilerDetalleImpuestoID = alquilerDetalleImpuestoID;
            oAlquilerDetalle = oalquilerDetalle;
            oImpuesto = oimpuesto;
            Cantidad = cantidad;
        }

        protected ContractLineTaxesEntity()
        {
        }

        public static ContractLineTaxesEntity Create(int id,  ContractLineEntity oalquilerDetalle, TaxesEntity oimpuesto, double? cantidad)
            => new ContractLineTaxesEntity(id,  oalquilerDetalle, oimpuesto, cantidad);

        public static ContractLineTaxesEntity UpdateId(ContractLineTaxesEntity taxes, int id) =>
            new ContractLineTaxesEntity(id,  taxes.oAlquilerDetalle, taxes.oImpuesto, taxes.Cantidad);
    }
}
