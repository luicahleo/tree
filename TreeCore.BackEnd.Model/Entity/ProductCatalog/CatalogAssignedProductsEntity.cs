namespace TreeCore.BackEnd.Model.Entity.ProductCatalog
{
    public class CatalogAssignedProductsEntity: BaseEntity
    {
        public int? CoreProductCatalogServicioAsignadoID;
        public CatalogEntity CoreProductCatalogs;
        public float Precio;
        public ProductEntity CoreProductCatalogServicios;

        public CatalogAssignedProductsEntity(int? coreProductCatalogServicioAsignadoID, float precio, ProductEntity coreProductCatalogServicios,
            CatalogEntity coreProductCatalogs)
        {
            CoreProductCatalogServicioAsignadoID = coreProductCatalogServicioAsignadoID;
            Precio = precio;
            CoreProductCatalogs = coreProductCatalogs;
            CoreProductCatalogServicios = coreProductCatalogServicios;
        }

        protected CatalogAssignedProductsEntity() { }

        public static CatalogAssignedProductsEntity UpdateId(CatalogAssignedProductsEntity ProductsAssigned, int id) =>
            new CatalogAssignedProductsEntity(id, ProductsAssigned.Precio, ProductsAssigned.CoreProductCatalogServicios, ProductsAssigned.CoreProductCatalogs);
    }
}
