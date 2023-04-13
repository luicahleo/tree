using System;

namespace TreeCore.BackEnd.Model.Entity.ProductCatalog
{
    public class CatalogLifecycleStatusEntity : BaseEntity
    {
        public readonly int? CoreProductCatalogEstadoGlobalID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Defecto;
        public readonly int? ClienteID;

        public CatalogLifecycleStatusEntity(int? coreProductCatalogEstadoGlobalID, string codigo, string nombre, string descripcion, bool activo, bool defecto, int? clienteID)
        {
            CoreProductCatalogEstadoGlobalID = coreProductCatalogEstadoGlobalID;
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            Activo = activo;
            Defecto = defecto;
            ClienteID = clienteID;
        }

        protected CatalogLifecycleStatusEntity() { }

        public static CatalogLifecycleStatusEntity UpdateId(CatalogLifecycleStatusEntity CatalogLifecycleStatus, int id) =>
            new CatalogLifecycleStatusEntity(id, CatalogLifecycleStatus.Codigo, CatalogLifecycleStatus.Nombre, CatalogLifecycleStatus.Descripcion, 
                CatalogLifecycleStatus.Activo, CatalogLifecycleStatus.Defecto, CatalogLifecycleStatus.ClienteID);
    }
}

