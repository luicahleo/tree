using System;

namespace TreeCore.BackEnd.Model.Entity.BusinessProcess
{
    public class BusinessProcessTypeEntity : BaseEntity
    {
        public readonly int? CoreBusinessProcessTipoID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly bool Activo;
        public readonly string Descripcion;
        public readonly bool Defecto;
        public readonly int? ClienteID;


        public BusinessProcessTypeEntity(int? coreBusinessProcessTipoID, string codigo, string nombre, bool activo, string descripcion, bool defecto, int? clienteID)
        {
            this.CoreBusinessProcessTipoID = coreBusinessProcessTipoID;
            this.Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            this.Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            this.Activo = activo;
            this.Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            this.Defecto = defecto;
            this.ClienteID = clienteID;

        }

        protected BusinessProcessTypeEntity() { }
        public static BusinessProcessTypeEntity Create(int id, string codigo, string nombre, bool activo, string descripcion, bool defecto, int clienteID) /*BusinessProcessTypeEntity coreBusinessProcessTipos*/
                => new BusinessProcessTypeEntity(id, codigo, nombre, activo, descripcion, defecto, clienteID); /*coreBusinessProcessTipos*/

        public static BusinessProcessTypeEntity UpdateId(BusinessProcessTypeEntity ProductType, int id) =>
            new BusinessProcessTypeEntity(id, ProductType.Codigo, ProductType.Nombre, ProductType.Activo, ProductType.Descripcion, ProductType.Defecto, ProductType.ClienteID);
    }
}
