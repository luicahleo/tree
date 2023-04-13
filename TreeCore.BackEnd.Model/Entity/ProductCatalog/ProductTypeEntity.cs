using System;

namespace TreeCore.BackEnd.Model.Entity.ProductCatalog
{
    public class ProductTypeEntity: BaseEntity
    {
        public readonly int? CoreProductCatalogServicioTipoID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;

        public ProductTypeEntity(int? coreProductCatalogServicioTipoID, string codigo, string nombre, string descripcion, bool activo, bool defecto)
        {
            CoreProductCatalogServicioTipoID = coreProductCatalogServicioTipoID;
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            Activo = activo;
            Defecto = defecto;
        }

        protected ProductTypeEntity() { }

        public static ProductTypeEntity UpdateId(ProductTypeEntity ProductType, int id) =>
            new ProductTypeEntity(id, ProductType.Codigo, ProductType.Nombre, ProductType.Descripcion, ProductType.Activo, ProductType.Defecto);
    }
}
