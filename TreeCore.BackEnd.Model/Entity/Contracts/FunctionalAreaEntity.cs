using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCore.BackEnd.Model.Entity.Contracts
{
    public class FunctionalAreaEntity : BaseEntity
    {
        public readonly int? AreaFuncionalID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string AreaFuncional;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;



        public FunctionalAreaEntity(int? AreaFuncionalID, int? clienteID, string Codigo, string AreaFuncional, string Descripcion, bool Activo, bool Defecto)
        {
            this.AreaFuncionalID = AreaFuncionalID;
            this.ClienteID = clienteID;
            this.Codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            this.AreaFuncional = AreaFuncional ?? throw new ArgumentNullException(nameof(AreaFuncional));
            this.Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            this.Activo = Activo;
            this.Defecto = Defecto;
        }

        protected FunctionalAreaEntity()
        {
            //AreaFuncionalID = AreaFuncionalID;
            //ClienteID = ClienteID;
            //Codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            //AreaFuncional = AreaFuncional ?? throw new ArgumentNullException(nameof(AreaFuncional));
            //Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            //Activo = Activo;
            //Defecto = Defecto;
        }

        public static FunctionalAreaEntity Create(int id, int clienteID, string codigo, string areaFuncional, string descripcion,
            bool activo, bool defecto)
            => new FunctionalAreaEntity(id, clienteID, codigo, areaFuncional, descripcion, activo, defecto);
        public static FunctionalAreaEntity UpdateId(FunctionalAreaEntity FunctionalArea, int id) =>
            new FunctionalAreaEntity(id, FunctionalArea.ClienteID, FunctionalArea.Codigo, FunctionalArea.AreaFuncional, FunctionalArea.Descripcion, FunctionalArea.Activo, FunctionalArea.Defecto);
    }
}
