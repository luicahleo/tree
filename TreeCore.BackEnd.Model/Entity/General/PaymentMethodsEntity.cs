using System;

namespace TreeCore.BackEnd.Model.Entity.General
{
    public class PaymentMethodsEntity : BaseEntity
    {
        public readonly int? MetodoPagoID;
        public readonly int? ClienteID;
        public readonly string CodigoMetodoPago;
        public readonly string MetodoPago;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;
        public readonly bool RequiereBanco;

        public PaymentMethodsEntity(int? MetodoPagoID, int? clienteID, string CodigoMetodoPago, string MetodoPago, string Descripcion, bool Activo, bool Defecto, bool RequiereBanco)
        {
            this.MetodoPagoID = MetodoPagoID;
            this.ClienteID = clienteID;
            this.CodigoMetodoPago = CodigoMetodoPago ?? throw new ArgumentNullException(nameof(CodigoMetodoPago));
            this.MetodoPago = MetodoPago ?? throw new ArgumentNullException(nameof(MetodoPago));
            this.Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            this.Activo = Activo;
            this.Defecto = Defecto;
            this.RequiereBanco = RequiereBanco;
        }

        protected PaymentMethodsEntity()
        {
            
        }

        public static PaymentMethodsEntity Create(int id, int clienteID, string CodigoMetodoPago, string nombre, string descripcion,
            bool activo, bool defecto, bool requiereBanco)
            => new PaymentMethodsEntity(id, clienteID, CodigoMetodoPago, nombre, descripcion, activo, defecto, requiereBanco);
        public static PaymentMethodsEntity UpdateId(PaymentMethodsEntity MetodoPagoID, int id) =>
            new PaymentMethodsEntity(id, MetodoPagoID.ClienteID, MetodoPagoID.CodigoMetodoPago, MetodoPagoID.MetodoPago, MetodoPagoID.Descripcion, MetodoPagoID.Activo, MetodoPagoID.Defecto, MetodoPagoID.RequiereBanco);
    }
}
