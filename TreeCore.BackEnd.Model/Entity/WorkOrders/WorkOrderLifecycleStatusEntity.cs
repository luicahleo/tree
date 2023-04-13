using System;
namespace TreeCore.BackEnd.Model.Entity.WorkOrders
{
    public class WorkOrderLifecycleStatusEntity : BaseEntity
    {
        public readonly int? CoreWorkOrderEstadoID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Descripcion;
        public readonly string Color;
        public readonly bool Activo;
        public readonly bool Defecto;



        public WorkOrderLifecycleStatusEntity(int? coreWorkOrderEstadoID, int? clienteID, string codigo, string nombre, string descripcion,string color,bool activo, bool defecto)
        {
            CoreWorkOrderEstadoID = coreWorkOrderEstadoID;
            ClienteID = clienteID;
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Color = color ?? throw new ArgumentNullException(nameof(color));
            Descripcion = descripcion;
            Activo = activo;
            Defecto = defecto;

        }

        protected WorkOrderLifecycleStatusEntity()
        {
        }

        public static WorkOrderLifecycleStatusEntity Create(int id, int clienteID, string codigo, string nombre, string descripcion, string color,
            bool activo, bool defecto)
            => new WorkOrderLifecycleStatusEntity(id, clienteID, codigo, nombre, descripcion,color, activo, defecto);

        public static WorkOrderLifecycleStatusEntity UpdateId(WorkOrderLifecycleStatusEntity trackingStatus, int id) =>
            new WorkOrderLifecycleStatusEntity(id, trackingStatus.ClienteID, trackingStatus.Codigo, trackingStatus.Nombre, trackingStatus.Descripcion,trackingStatus.Color, trackingStatus.Activo, trackingStatus.Defecto);
    }
}
