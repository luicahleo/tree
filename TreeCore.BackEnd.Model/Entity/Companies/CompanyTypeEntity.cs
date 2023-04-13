using System;

namespace TreeCore.BackEnd.Model.Entity.Companies
{
    public class CompanyTypeEntity : BaseEntity
    {
        public readonly int? EntidadTipoID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string EntidadTipo;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;



        public CompanyTypeEntity(int? EntidadTipoID, int? clienteID, string Codigo, string Nombre, string Descripcion, bool Activo, bool Defecto)
        {
            this.EntidadTipoID = EntidadTipoID;
            this.ClienteID = clienteID;
            this.Codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            this.EntidadTipo = Nombre ?? throw new ArgumentNullException(nameof(Nombre));
            this.Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            this.Activo = Activo;
            this.Defecto = Defecto;
        }

        public CompanyTypeEntity()
        {
            
        }

        public static CompanyTypeEntity Create(int id, int clienteID, string codigo, string nombre, string descripcion,
            bool activo, bool defecto)
            => new CompanyTypeEntity(id, clienteID, codigo, nombre, descripcion, activo, defecto);
        public static CompanyTypeEntity UpdateId(CompanyTypeEntity companyType, int id) =>
            new CompanyTypeEntity(id, companyType.ClienteID, companyType.Codigo, companyType.EntidadTipo, companyType.Descripcion, companyType.Activo, companyType.Defecto);
    }
}
