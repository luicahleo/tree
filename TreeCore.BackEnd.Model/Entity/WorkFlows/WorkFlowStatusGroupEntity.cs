using System;
namespace TreeCore.BackEnd.Model.Entity.WorkFlows
{
    public class WorkFlowStatusGroupEntity : BaseEntity
    {
        public readonly int? EstadoAgrupacionID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;



        public WorkFlowStatusGroupEntity(int? estadoAgrupacionID, int? clienteID, string codigo, string nombre, string descripcion,bool activo, bool defecto)
        {
            EstadoAgrupacionID = estadoAgrupacionID;
            ClienteID = clienteID;
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Descripcion = descripcion;
            Activo = activo;
            Defecto = defecto;

        }

        protected WorkFlowStatusGroupEntity()
        {
        }

        public static WorkFlowStatusGroupEntity Create(int id, int clienteID, string codigo, string nombre, string descripcion,
            bool activo, bool defecto)
            => new WorkFlowStatusGroupEntity(id, clienteID, codigo, nombre, descripcion, activo, defecto);

        public static WorkFlowStatusGroupEntity UpdateId(WorkFlowStatusGroupEntity statusGroup, int id) =>
            new WorkFlowStatusGroupEntity(id, statusGroup.ClienteID, statusGroup.Codigo, statusGroup.Nombre, statusGroup.Descripcion, statusGroup.Activo, statusGroup.Defecto);
    }
}
