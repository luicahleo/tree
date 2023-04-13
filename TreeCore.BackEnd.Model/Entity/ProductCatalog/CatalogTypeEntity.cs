using System;

namespace TreeCore.BackEnd.Model.Entity.ProductCatalog
{
    public class CatalogTypeEntity : BaseEntity
    {
        public readonly int? CoreProductCatalogTipoID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;
        public readonly bool EsVenta;
        public readonly bool EsCompra;

        public CatalogTypeEntity(int? CoreProductCatalogTipoID, int? clienteID, string Codigo, string Nombre, string Descripcion, bool Activo, bool Defecto,
            bool EsVenta, bool EsCompra)
        {
            this.CoreProductCatalogTipoID = CoreProductCatalogTipoID;
            this.ClienteID = clienteID;
            this.Codigo = Codigo ?? throw new ArgumentNullException(nameof(Codigo));
            this.Nombre = Nombre ?? throw new ArgumentNullException(nameof(Nombre));
            this.Descripcion = Descripcion ?? throw new ArgumentNullException(nameof(Descripcion));
            this.Activo = Activo;
            this.Defecto = Defecto;
            this.EsCompra = EsCompra;
            this.EsVenta = EsVenta;
        }

        protected CatalogTypeEntity() {}

        public static CatalogTypeEntity Create(int id, int clienteID, string codigo, string nombre, string descripcion,
            bool activo, bool defecto, bool esCompra, bool esVenta)
            => new CatalogTypeEntity(id, clienteID, codigo, nombre, descripcion, activo, defecto, esCompra, esVenta);
        public static CatalogTypeEntity UpdateId(CatalogTypeEntity CatalogType, int id) =>
            new CatalogTypeEntity(id, CatalogType.ClienteID, CatalogType.Codigo, CatalogType.Nombre, CatalogType.Descripcion, CatalogType.Activo, CatalogType.Defecto,
                CatalogType.EsCompra, CatalogType.EsVenta);
    }
}
