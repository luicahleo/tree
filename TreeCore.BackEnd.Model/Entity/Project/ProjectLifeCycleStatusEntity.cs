using System;
namespace TreeCore.BackEnd.Model.Entity.Project
{
    public class ProjectLifeCycleStatusEntity : BaseEntity
    {
        public readonly int? CoreProjectLifeCycleStatusID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly string Color;
        public readonly int? ClienteID;



        public ProjectLifeCycleStatusEntity(int? coreProjectLifeCycleStatusID, string codigo, string nombre, string descripcion,bool activo, string color, int? clienteID)
        {
            this.CoreProjectLifeCycleStatusID = coreProjectLifeCycleStatusID;
            this.Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            this.Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            this.Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            this.Activo = activo;
            this.Color = color ?? throw new ArgumentNullException(nameof(color));
            this.ClienteID = clienteID;

        }

        protected ProjectLifeCycleStatusEntity() { }

        public static ProjectLifeCycleStatusEntity Create(int id, string codigo, string nombre, string descripcion, bool activo, string color, int clienteID) /*ProjectLifeCycleStatusEntity coreProjectLifeCycleStatus*/
               => new ProjectLifeCycleStatusEntity(id, codigo, nombre, descripcion, activo, color, clienteID); /*coreProjectLifeCycleStatus*/

        public static ProjectLifeCycleStatusEntity UpdateId(ProjectLifeCycleStatusEntity ProductLifeCycleStatus, int id) =>
            new ProjectLifeCycleStatusEntity(id, ProductLifeCycleStatus.Codigo, ProductLifeCycleStatus.Nombre, ProductLifeCycleStatus.Descripcion, ProductLifeCycleStatus.Activo,  ProductLifeCycleStatus.Color, ProductLifeCycleStatus.ClienteID);
    }
}
