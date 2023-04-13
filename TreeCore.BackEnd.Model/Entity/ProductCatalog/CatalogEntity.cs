using System;
using System.Collections.Generic;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.ValueObject;

namespace TreeCore.BackEnd.Model.Entity.ProductCatalog
{
    public class CatalogEntity : BaseEntity
    {
        public readonly int? CoreProductCatalogID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly int? ClienteID;
        public readonly string Descripcion;
        public CurrencyEntity Monedas;
        public CatalogTypeEntity CoreProductCatalogTipos;
        public DateTime FechaInicioVigencia;
        public DateTime FechaFinVigencia;
        public DateTime? FechaCreacion;
        public DateTime? FechaUltimaModificacion;
        public UserEntity UsuariosCreador;
        public UserEntity UsuariosModificador;
        public PriceReadjustment PricesReadjustment;
        public CompanyEntity Entidades;
        public CatalogLifecycleStatusEntity CoreProductCatalogEstadosGlobales;
        public IEnumerable<CatalogAssignedProductsEntity> ServiciosVinculados;
        public IEnumerable<CompanyEntity> EntidadesVinculadas;

        public CatalogEntity(int? coreProductCatalogID, string codigo, string nombre, int? clienteID, 
            CurrencyEntity monedas, CatalogTypeEntity coreProductCatalogTipos, DateTime fechaInicioVigencia, DateTime fechaFinVigencia,
            DateTime? fechaCreacion, DateTime? fechaUltimaModificacion, UserEntity usuariosCreador, UserEntity usuariosModificador,
            string descripcion, PriceReadjustment pricesReadjustment, CatalogLifecycleStatusEntity coreProductCatalogEstadosGlobales,
            CompanyEntity entidades, IEnumerable<CatalogAssignedProductsEntity> serviciosVinculados,
            IEnumerable<CompanyEntity> entidadesVinculadas)
        {
            CoreProductCatalogID = coreProductCatalogID;
            ClienteID = clienteID;
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo)); ;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre)); ;
            Monedas = monedas;
            CoreProductCatalogTipos = coreProductCatalogTipos;
            FechaInicioVigencia = fechaInicioVigencia;
            FechaFinVigencia = fechaFinVigencia;
            FechaCreacion = fechaCreacion;
            FechaUltimaModificacion = fechaUltimaModificacion;
            UsuariosCreador = usuariosCreador;
            UsuariosModificador = usuariosModificador;
            Descripcion = descripcion;
            PricesReadjustment = pricesReadjustment;
            Entidades = entidades;
            CoreProductCatalogEstadosGlobales = coreProductCatalogEstadosGlobales;
            ServiciosVinculados = serviciosVinculados;
            EntidadesVinculadas = entidadesVinculadas;
        }

        protected CatalogEntity() { }

        public static CatalogEntity UpdateId(CatalogEntity catalog, int id) =>
            new CatalogEntity(id, catalog.Codigo, catalog.Nombre, catalog.ClienteID, catalog.Monedas, catalog.CoreProductCatalogTipos,
                catalog.FechaInicioVigencia, catalog.FechaFinVigencia, catalog.FechaCreacion, catalog.FechaUltimaModificacion, catalog.UsuariosCreador, 
                catalog.UsuariosModificador, catalog.Descripcion, catalog.PricesReadjustment, catalog.CoreProductCatalogEstadosGlobales, catalog.Entidades, 
                catalog.ServiciosVinculados, catalog.EntidadesVinculadas);
    }
}

