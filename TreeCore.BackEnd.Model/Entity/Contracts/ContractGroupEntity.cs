using System;


namespace TreeCore.BackEnd.Model.Entity.Contracts
{
    public class ContractGroupEntity : BaseEntity
    {
        public readonly int? AlquilerTipoContratacionID;
        public readonly string TipoContratacion;
        public readonly bool Activo;
        public readonly bool Defecto;
        public readonly int? ClienteID;
        public readonly string codigo;
        public readonly string Descripcion;



        public ContractGroupEntity(int? AlquilerTipoContratacionID, int? clienteID, string Codigo, string TipoContratacion, string Descripcion, bool Activo, bool Defecto)
        {
            this.AlquilerTipoContratacionID = AlquilerTipoContratacionID;
            this.ClienteID = clienteID;
            this.codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            this.TipoContratacion = TipoContratacion ?? throw new ArgumentNullException(nameof(TipoContratacion));
            this.Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            this.Activo = Activo;
            this.Defecto = Defecto;
        }

        protected ContractGroupEntity()
        {
            //CoreProductCatalogTipoID = CoreProductCatalogTipoID;
            //ClienteID = ClienteID;
            //Codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            //Nombre = Nombre ?? throw new ArgumentNullException(nameof(Nombre));
            //Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            //Activo = Activo;
            //Defecto = Defecto;
        }

        public static ContractGroupEntity Create(int id, int clienteID, string codigo, string nombre, string descripcion,
            bool activo, bool defecto)
            => new ContractGroupEntity(id, clienteID, codigo, nombre, descripcion, activo, defecto);
        public static ContractGroupEntity UpdateId(ContractGroupEntity ContractGroup, int id) =>
            new ContractGroupEntity(id, ContractGroup.ClienteID, ContractGroup.codigo, ContractGroup.TipoContratacion, ContractGroup.Descripcion, ContractGroup.Activo, ContractGroup.Defecto);

    }
}
