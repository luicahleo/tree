using System;
using System.Collections.Generic;
using System.Linq;
using TreeCore.BackEnd.Model.Entity.Companies;

namespace TreeCore.BackEnd.Model.Entity.ProductCatalog
{
    public class ProductEntity : BaseEntity
    {
        public readonly int? CoreProductCatalogServicioID;
        public readonly string Codigo;
        public readonly string Nombre;
        public ProductTypeEntity CoreProductCatalogServiciosTipos;
        public readonly float Cantidad;
        public readonly string CronFormat;
        public readonly string Unidad;
        public readonly int? UsuarioID;
        public readonly int? CoreObjetoNegocioTipoID;
        public readonly int? CoreFormulaID;
        public readonly DateTime? FechaModificacion;
        public readonly DateTime? FechaCreacion;
        public readonly int? UsuarioCreadorID;
        public bool EsPack;
        public bool Publico;
        public IEnumerable<ProductEntity> ServiciosVinculados;
        public IEnumerable<CompanyEntity> EntidadesVinculadas;
        public readonly string Descripcion;

        public ProductEntity(int? coreProductCatalogServicioID, string codigo, string nombre, ProductTypeEntity coreProductCatalogServiciosTipos, float cantidad, string cronFormat, 
            string unidad, int? usuarioID, int? coreObjetoNegocioTipoID, int? coreFormulaID, 
            DateTime? fechaModificacion, DateTime? fechaCreacion, int? usuarioCreadorID, IEnumerable<ProductEntity> serviciosVinculados, IEnumerable<CompanyEntity> entidadesVinculadas, bool publico,
            string descripcion)
        {
            CoreProductCatalogServicioID = coreProductCatalogServicioID;
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo)); ;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre)); ;
            CoreProductCatalogServiciosTipos = coreProductCatalogServiciosTipos;
            Cantidad = cantidad;
            CronFormat = cronFormat;
            Unidad = unidad;
            UsuarioID = usuarioID;
            CoreObjetoNegocioTipoID = coreObjetoNegocioTipoID;
            CoreFormulaID = coreFormulaID;
            FechaModificacion = fechaModificacion;
            FechaCreacion = fechaCreacion;
            UsuarioCreadorID = usuarioCreadorID;
            ServiciosVinculados = serviciosVinculados;
            EntidadesVinculadas = entidadesVinculadas;
            EsPack = (ServiciosVinculados != null && ServiciosVinculados.ToList().Count > 0);
            Publico = publico;
            Descripcion = descripcion;
        }

        protected ProductEntity() { }

        public static ProductEntity UpdateId(ProductEntity Product, int id) =>
            new ProductEntity(id, Product.Codigo, Product.Nombre, 
                Product.CoreProductCatalogServiciosTipos, Product.Cantidad, Product.CronFormat, Product.Unidad,
                Product.UsuarioID, Product.CoreObjetoNegocioTipoID, Product.CoreFormulaID, Product.FechaModificacion, 
                Product.FechaCreacion, Product.UsuarioCreadorID, Product.ServiciosVinculados, Product.EntidadesVinculadas, Product.Publico, Product.Descripcion);
    }
}

