using System;

namespace TreeCore.BackEnd.Model.Entity.General
{
    public class PaymentTermEntity : BaseEntity
    {
        public readonly int? CondicionPagoID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string CondicionPago;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;



        public PaymentTermEntity(int? CondicionPagoID, int? clienteID, string Codigo, string Nombre, string Descripcion, bool Activo, bool Defecto)
        {
            this.CondicionPagoID = CondicionPagoID;
            this.ClienteID = clienteID;
            this.Codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            this.CondicionPago = Nombre ?? throw new ArgumentNullException(nameof(Nombre));
            this.Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            this.Activo = Activo;
            this.Defecto = Defecto;
        }

        public PaymentTermEntity()
        {
            
        }

        public static PaymentTermEntity Create(int id, int clienteID, string codigo, string nombre, string descripcion,
            bool activo, bool defecto)
            => new PaymentTermEntity(id, clienteID, codigo, nombre, descripcion, activo, defecto);
        public static PaymentTermEntity UpdateId(PaymentTermEntity CondicionPagoID, int id) =>
            new PaymentTermEntity(id, CondicionPagoID.ClienteID, CondicionPagoID.Codigo, CondicionPagoID.CondicionPago, CondicionPagoID.Descripcion, CondicionPagoID.Activo, CondicionPagoID.Defecto);
    }
}
