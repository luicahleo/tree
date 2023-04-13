using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.General;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Model.ValueObject;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.ProductCatalog
{
    public class CatalogRepository : BaseRepository<CatalogEntity>
    {
        public override Table Table => TableNames.Catalog;
        public Table tableCompany => TableNames.Company;
        public Table tableCurrency => TableNames.Currency;
        public Table tableCatalogType => TableNames.CatalogType;
        public Table tableCompanyAssigned => TableNames.CatalogAssignedCompanies;
        public Table tableInflation => TableNames.Inflation;
        public Table tableLifecycleStatus => TableNames.CatalogLifecycleStatus;
        public Table tableProduct => TableNames.Product;
        public Table tableProductAssigned => TableNames.CatalogAssignedProducts;
        public Table tableUser => TableNames.User;
        private UserRepository userRepository;

        public CatalogRepository(TransactionalWrapper conexion) : base(conexion) 
        {
            userRepository = new UserRepository(conexion);
        }

        public override async Task<CatalogEntity> InsertSingle(CatalogEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(CatalogEntity.Codigo)}, {nameof(CatalogEntity.Nombre)}, {nameof(CatalogEntity.ClienteID)}, " +
                $"{nameof(CatalogEntity.Monedas.MonedaID)}, {nameof(CatalogEntity.CoreProductCatalogTipos.CoreProductCatalogTipoID)}, {nameof(CatalogEntity.Entidades.EntidadID)}, " +
                $"{nameof(CatalogEntity.PricesReadjustment.Tipo)}, {nameof(CatalogEntity.PricesReadjustment.Inflacion.InflacionID)}, {nameof(CatalogEntity.PricesReadjustment.CantidadFija)}," +
                $"{nameof(CatalogEntity.PricesReadjustment.PorcentajeFijo)}, {nameof(CatalogEntity.PricesReadjustment.Periodicidad)}, {nameof(CatalogEntity.PricesReadjustment.FechaFinReajuste)}," +
                $"{nameof(CatalogEntity.PricesReadjustment.FechaInicioReajuste)}, {nameof(CatalogEntity.PricesReadjustment.FechaProximaReajuste)}, " +
                $"{nameof(CatalogEntity.FechaInicioVigencia)}, {nameof(CatalogEntity.FechaFinVigencia)}, {nameof(CatalogEntity.FechaCreacion)}, {nameof(CatalogEntity.FechaUltimaModificacion)}, " +
                $" UsuarioCreadorID, UsuarioModificadorID, {nameof(CatalogEntity.Descripcion)}, {nameof(CatalogEntity.CoreProductCatalogEstadosGlobales.CoreProductCatalogEstadoGlobalID)}) " +
                $"values (@{nameof(CatalogEntity.Codigo)}, @{nameof(CatalogEntity.Nombre)}, @{nameof(CatalogEntity.ClienteID)}, " +
                $" @{nameof(CatalogEntity.Monedas.MonedaID)}, @{nameof(CatalogEntity.CoreProductCatalogTipos.CoreProductCatalogTipoID)}, @{nameof(CatalogEntity.Entidades.EntidadID)}, " +
                $" @{nameof(CatalogEntity.PricesReadjustment.Tipo)}, @{nameof(CatalogEntity.PricesReadjustment.Inflacion.InflacionID)}, @{nameof(CatalogEntity.PricesReadjustment.CantidadFija)}," +
                $" @{nameof(CatalogEntity.PricesReadjustment.PorcentajeFijo)}, @{nameof(CatalogEntity.PricesReadjustment.Periodicidad)}, @{nameof(CatalogEntity.PricesReadjustment.FechaFinReajuste)}," +
                $" @{nameof(CatalogEntity.PricesReadjustment.FechaInicioReajuste)}, @{nameof(CatalogEntity.PricesReadjustment.FechaProximaReajuste)}, " +
                $" @{nameof(CatalogEntity.FechaInicioVigencia)}, @{nameof(CatalogEntity.FechaFinVigencia)}, @{nameof(CatalogEntity.FechaCreacion)}, " +
                $" @{nameof(CatalogEntity.FechaUltimaModificacion)}, @UsuarioCreadorID, @UsuarioModificadorID, " +
                $"@{nameof(CatalogEntity.Descripcion)}, @{nameof(CatalogEntity.CoreProductCatalogEstadosGlobales.CoreProductCatalogEstadoGlobalID)});" +
                 $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                ClienteID = obj.ClienteID,
                MonedaID = (obj.Monedas != null) ? obj.Monedas.MonedaID : null,
                CoreProductCatalogTipoID = (obj.CoreProductCatalogTipos != null) ? obj.CoreProductCatalogTipos.CoreProductCatalogTipoID : null,
                EntidadID = obj.Entidades.EntidadID,
                Tipo = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.Tipo : null,
                InflacionID = (obj.PricesReadjustment.Inflacion != null) ? obj.PricesReadjustment.Inflacion.InflacionID : null,
                CantidadFija = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.CantidadFija : null,
                PorcentajeFijo = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.PorcentajeFijo : null,
                Periodicidad = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.Periodicidad : null,
                FechaFinReajuste = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.FechaFinReajuste : null,
                FechaInicioReajuste = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.FechaInicioReajuste : null,
                FechaProximaReajuste = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.FechaProximaReajuste : null,
                FechaInicioVigencia = obj.FechaInicioVigencia,
                FechaFinVigencia = obj.FechaFinVigencia,
                FechaCreacion = obj.FechaCreacion,
                FechaUltimaModificacion = obj.FechaUltimaModificacion,
                UsuarioCreadorID = (obj.UsuariosCreador != null) ? obj.UsuariosCreador.UsuarioID : null,
                UsuarioModificadorID = (obj.UsuariosModificador != null) ? obj.UsuariosModificador.UsuarioID : null,
                Descripcion = obj.Descripcion,
                CoreProductCatalogEstadoGlobalID = (obj.CoreProductCatalogEstadosGlobales != null) ? obj.CoreProductCatalogEstadosGlobales.CoreProductCatalogEstadoGlobalID : null
            }, sqlTran)).First();

            if (obj.ServiciosVinculados != null)
            {
                foreach (CatalogAssignedProductsEntity servicioVinculado in obj.ServiciosVinculados.ToList())
                {
                    if (servicioVinculado != null)
                    {
                        sql = $"INSERT INTO {tableProductAssigned.Name} ({nameof(CatalogEntity.CoreProductCatalogID)}, " +
                            $"{nameof(ProductEntity.CoreProductCatalogServicioID)}, {nameof(servicioVinculado.Precio)})" +
                            $"values (@{nameof(newId)}, " +
                            $"@{nameof(servicioVinculado.CoreProductCatalogServicios.CoreProductCatalogServicioID)}, @{nameof(servicioVinculado.Precio)});" +
                            $"SELECT SCOPE_IDENTITY();";

                        var idLinkedProduct = (await connection.QueryAsync<int>(sql, new
                        {
                            newId = newId,
                            CoreProductCatalogServicioID = servicioVinculado.CoreProductCatalogServicios.CoreProductCatalogServicioID,
                            Precio = servicioVinculado.Precio
                        }, sqlTran)).First();
                    }
                };
            }

            if (obj.EntidadesVinculadas != null)
            {
                foreach (CompanyEntity entidadVinculada in obj.EntidadesVinculadas.ToList())
                {
                    sql = $"INSERT INTO {tableCompanyAssigned.Name} ({nameof(CatalogLinkedCompanyEntity.CoreProductCatalogID)}, " +
                    $"{nameof(CatalogLinkedCompanyEntity.EntidadID)})" +
                    $"values (@{nameof(newId)}, " +
                    $"@{nameof(CatalogLinkedCompanyEntity.EntidadID)});" +
                    $"SELECT SCOPE_IDENTITY();";

                    var idLinkedProduct = (await connection.QueryAsync<int>(sql, new
                    {
                        newId = newId,
                        EntidadID = entidadVinculada.EntidadID
                    }, sqlTran)).First();
                };
            }

            return CatalogEntity.UpdateId(obj, newId);
        }

        public override async Task<CatalogEntity> UpdateSingle(CatalogEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            #region REMOVE ASSIGNED PRODUCTS
            string sql = $"delete from {tableProductAssigned.Name} Where " +
                $"{nameof(CatalogEntity.CoreProductCatalogID)} = @{nameof(CatalogEntity.CoreProductCatalogID)}";
            var numReg = await connection.ExecuteAsync(sql, new
            {
                CoreProductCatalogID = obj.CoreProductCatalogID
            }, sqlTran);
            #endregion

            #region ADD ASSIGNED PRODUCTS
            if (obj.ServiciosVinculados != null)
            {
                foreach (CatalogAssignedProductsEntity servicioVinculado in obj.ServiciosVinculados.ToList())
                {
                    if (servicioVinculado != null)
                    {
                        sql = $"INSERT INTO {tableProductAssigned.Name} ({nameof(CatalogEntity.CoreProductCatalogID)}, " +
                    $"{nameof(ProductEntity.CoreProductCatalogServicioID)}, {nameof(servicioVinculado.Precio)})" +
                    $"values (@{nameof(CatalogEntity.CoreProductCatalogID)}, " +
                    $"@{nameof(servicioVinculado.CoreProductCatalogServicios.CoreProductCatalogServicioID)}, @{nameof(servicioVinculado.Precio)});" +
                    $"SELECT SCOPE_IDENTITY();";

                        var idLinkedProduct = (await connection.QueryAsync<int>(sql, new
                        {
                            CoreProductCatalogID = obj.CoreProductCatalogID,
                            CoreProductCatalogServicioID = servicioVinculado.CoreProductCatalogServicios.CoreProductCatalogServicioID,
                            Precio = servicioVinculado.Precio
                        }, sqlTran)).First();
                    }
                };
            }
            #endregion

            #region REMOVE ASSIGNED COMPANIES
            sql = $"delete from {tableCompanyAssigned.Name} Where " +
                $"{nameof(CatalogEntity.CoreProductCatalogID)} = @{nameof(CatalogEntity.CoreProductCatalogID)}";
            var numRegCompany = await connection.ExecuteAsync(sql, new
            {
                CoreProductCatalogID = obj.CoreProductCatalogID
            }, sqlTran);
            #endregion

            #region ADD ASSIGNED COMPANIES
            if (obj.EntidadesVinculadas != null)
            {
                foreach (CompanyEntity entidadVinculada in obj.EntidadesVinculadas.ToList())
                {
                    if (entidadVinculada != null)
                    {
                        sql = $"INSERT INTO {tableCompanyAssigned.Name} ({nameof(CatalogLinkedCompanyEntity.CoreProductCatalogID)}, " +
                                $"{nameof(CatalogLinkedCompanyEntity.EntidadID)})" +
                                $"values (@{nameof(CatalogLinkedCompanyEntity.CoreProductCatalogID)}, " +
                                $"@{nameof(entidadVinculada.EntidadID)});" +
                                $"SELECT SCOPE_IDENTITY();";

                        var idLinkedProduct = (await connection.QueryAsync<int>(sql, new
                        {
                            CoreProductCatalogID = obj.CoreProductCatalogID,
                            EntidadID = entidadVinculada.EntidadID
                        }, sqlTran)).First();
                    }
                };
            }
            #endregion

            sql = $"update {Table.Name} set {nameof(CatalogEntity.Codigo)} = @{nameof(CatalogEntity.Codigo)}, " +
                $" {nameof(CatalogEntity.Nombre)} =   @{nameof(CatalogEntity.Nombre)}, " +
                $" {nameof(CatalogEntity.ClienteID)} =   @{nameof(CatalogEntity.ClienteID)}, " +
                $" {nameof(CatalogEntity.Monedas.MonedaID)} =   @{nameof(CatalogEntity.Monedas.MonedaID)}, " +
                $" {nameof(CatalogEntity.CoreProductCatalogTipos.CoreProductCatalogTipoID)} =   @{nameof(CatalogEntity.CoreProductCatalogTipos.CoreProductCatalogTipoID)}, " +
                $" {nameof(CatalogEntity.Entidades.EntidadID)} = @{nameof(CatalogEntity.Entidades.EntidadID)}, " +
                $" {nameof(CatalogEntity.PricesReadjustment.Tipo)} =   @{nameof(CatalogEntity.PricesReadjustment.Tipo)}, " +
                $" {nameof(CatalogEntity.PricesReadjustment.Inflacion.InflacionID)} =   @{nameof(CatalogEntity.PricesReadjustment.Inflacion.InflacionID)}, " +
                $" {nameof(CatalogEntity.PricesReadjustment.CantidadFija)} =   @{nameof(CatalogEntity.PricesReadjustment.CantidadFija)}, " +
                $" {nameof(CatalogEntity.PricesReadjustment.PorcentajeFijo)} =   @{nameof(CatalogEntity.PricesReadjustment.PorcentajeFijo)}, " +
                $" {nameof(CatalogEntity.PricesReadjustment.Periodicidad)} =   @{nameof(CatalogEntity.PricesReadjustment.Periodicidad)}, " +
                $" {nameof(CatalogEntity.PricesReadjustment.FechaFinReajuste)} =   @{nameof(CatalogEntity.PricesReadjustment.FechaFinReajuste)}, " +
                $" {nameof(CatalogEntity.PricesReadjustment.FechaInicioReajuste)} =   @{nameof(CatalogEntity.PricesReadjustment.FechaInicioReajuste)}, " +
                $" {nameof(CatalogEntity.PricesReadjustment.FechaProximaReajuste)} =   @{nameof(CatalogEntity.PricesReadjustment.FechaProximaReajuste)}, " +
                $" {nameof(CatalogEntity.FechaInicioVigencia)} =   @{nameof(CatalogEntity.FechaInicioVigencia)}, " +
                $" {nameof(CatalogEntity.FechaFinVigencia)} =   @{nameof(CatalogEntity.FechaFinVigencia)}, " +
                $" {nameof(CatalogEntity.FechaUltimaModificacion)} =   @{nameof(CatalogEntity.FechaUltimaModificacion)}, " +
                $" UsuarioModificadorID = @UsuarioModificadorID, " +
                $" {nameof(CatalogEntity.Descripcion)} =   @{nameof(CatalogEntity.Descripcion)}, " +
                $" {nameof(CatalogEntity.CoreProductCatalogEstadosGlobales.CoreProductCatalogEstadoGlobalID)} =   @{nameof(CatalogEntity.CoreProductCatalogEstadosGlobales.CoreProductCatalogEstadoGlobalID)} " +
                $" where {nameof(CatalogEntity.CoreProductCatalogID)} = @{nameof(CatalogEntity.CoreProductCatalogID)} ";

            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                CoreProductCatalogID = obj.CoreProductCatalogID,
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                ClienteID = obj.ClienteID,
                EntidadID = obj.Entidades.EntidadID,
                MonedaID = (obj.Monedas != null) ? obj.Monedas.MonedaID : null,
                CoreProductCatalogTipoID = (obj.CoreProductCatalogTipos != null) ? obj.CoreProductCatalogTipos.CoreProductCatalogTipoID : null,
                Tipo = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.Tipo : null,
                InflacionID = (obj.PricesReadjustment.Inflacion != null) ? obj.PricesReadjustment.Inflacion.InflacionID : null,
                CantidadFija = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.CantidadFija : null,
                PorcentajeFijo = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.PorcentajeFijo : null,
                Periodicidad = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.Periodicidad : null,
                FechaFinReajuste = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.FechaFinReajuste : null,
                FechaInicioReajuste = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.FechaInicioReajuste : null,
                FechaProximaReajuste = (obj.PricesReadjustment != null) ? obj.PricesReadjustment.FechaProximaReajuste : null,
                FechaInicioVigencia = obj.FechaInicioVigencia,
                FechaFinVigencia = obj.FechaFinVigencia,
                FechaUltimaModificacion = obj.FechaUltimaModificacion,
                UsuarioModificadorID = (obj.UsuariosModificador != null) ? obj.UsuariosModificador.UsuarioID : null,
                Descripcion = obj.Descripcion,
                CoreProductCatalogEstadoGlobalID = (obj.CoreProductCatalogEstadosGlobales != null) ? obj.CoreProductCatalogEstadosGlobales.CoreProductCatalogEstadoGlobalID : null
            }, sqlTran));

            return obj;
        }

        public override async Task<CatalogEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} Catalog " +
                $" left join {tableCompany.Name} Company on Catalog.{tableCompany.ID} = Company.{tableCompany.ID}" +
                $" inner join {tableCurrency.Name} Currency on Catalog.{tableCurrency.ID} = Currency.{tableCurrency.ID}" +
                $" inner join {tableCatalogType.Name} CatalogType on Catalog.{tableCatalogType.ID} = CatalogType.{tableCatalogType.ID}" +                
                $" left join {tableInflation.Name} Inflation on Catalog.{tableInflation.ID} = Inflation.{tableInflation.ID}" +
                $" left join {tableLifecycleStatus.Name} LifecycleStatus on Catalog.{tableLifecycleStatus.ID} = LifecycleStatus.{tableLifecycleStatus.ID}" +
                $" where Catalog.{Table.Code} = @code AND Catalog.{nameof(CatalogEntity.ClienteID)} = @{nameof(CatalogEntity.ClienteID)}";

            var result = await connection.QueryAsync<CatalogEntity, PriceReadjustment, CompanyEntity, CurrencyEntity, CatalogTypeEntity, 
                InflationEntity, CatalogLifecycleStatusEntity, CatalogEntity>(sql,
                (catalogEntity, priceReadjustment, companyEntity, currencyEntity, catalogTypeEntity, inflationEntity, catalogLifecycleStatusEntity) =>
                {
                    catalogEntity.Monedas = currencyEntity;
                    catalogEntity.Entidades = companyEntity;
                    catalogEntity.CoreProductCatalogTipos = catalogTypeEntity;
                    catalogEntity.CoreProductCatalogEstadosGlobales = catalogLifecycleStatusEntity;

                    if (priceReadjustment != null)
                    {
                        catalogEntity.PricesReadjustment = priceReadjustment;
                        catalogEntity.PricesReadjustment.Inflacion = (inflationEntity != null) ? inflationEntity : null;
                    }
                    else
                    {
                        catalogEntity.PricesReadjustment = null;
                    }

                    return catalogEntity;
                }, new { code = code, ClienteID = Client }, sqlTran, true,
                splitOn: $"Tipo,{tableCompany.ID},{tableCurrency.ID},{tableCatalogType.ID},{tableInflation.ID},{tableLifecycleStatus.ID}");

            if (result.ToList() != null && result.ToList().Count > 0)
            {
                string ids = "";
                result.ToList().ForEach(x =>
                {
                    ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.CoreProductCatalogID}";
                });

                List<CatalogAssignedProductsEntity> listAssigned = new List<CatalogAssignedProductsEntity>();

                string query2 = $"SELECT * FROM {Table.Name} Catalog " +
                    $"inner join {tableProductAssigned.Name} ProductAssigned on Catalog.{nameof(CatalogEntity.CoreProductCatalogID)} = ProductAssigned.{nameof(CatalogEntity.CoreProductCatalogID)} " +
                    $"inner join {tableProduct.Name} Product on ProductAssigned.{nameof(CatalogAssignedProductsEntity.CoreProductCatalogServicios.CoreProductCatalogServicioID)} = Product.{nameof(ProductEntity.CoreProductCatalogServicioID)} " +
                    $"WHERE Catalog.{nameof(CatalogEntity.CoreProductCatalogID)} IN ({ids})";
                var result2 = await connection.QueryAsync<CatalogEntity, CatalogAssignedProductsEntity, ProductEntity, CatalogEntity>(query2,
                (catalogEntity, catalogAssignedProductsEntity, productEntity) =>
                {
                    if (catalogAssignedProductsEntity != null)
                    {
                        catalogAssignedProductsEntity.CoreProductCatalogs = catalogEntity;
                        catalogAssignedProductsEntity.CoreProductCatalogServicios = productEntity;

                        result.Where(pEnt => pEnt.CoreProductCatalogID.Value == catalogAssignedProductsEntity.CoreProductCatalogs.CoreProductCatalogID).ToList().ForEach(lk =>
                        {
                            if (lk.ServiciosVinculados == null)
                            {
                                lk.ServiciosVinculados = listAssigned;
                            }

                            listAssigned.Add(catalogAssignedProductsEntity);
                            lk.ServiciosVinculados = listAssigned;
                        });
                    }

                    return catalogEntity;
                }, new { }, sqlTran, true, splitOn: $"{tableProductAssigned.ID},{tableProduct.ID}");

                string query3 = $"SELECT * FROM {Table.Name} Catalog " +
                    $"inner join {tableCompanyAssigned.Name} CompanyAssigned on Catalog.{nameof(CatalogEntity.CoreProductCatalogID)} = CompanyAssigned.{nameof(CatalogLinkedCompanyEntity.CoreProductCatalogID)} " +
                    $"inner join {tableCompany.Name} Company on CompanyAssigned.{nameof(CatalogLinkedCompanyEntity.EntidadID)} = Company.{nameof(CompanyEntity.EntidadID)} " +
                    $"WHERE Catalog.{nameof(CatalogEntity.CoreProductCatalogID)} IN ({ids})";
                var result3 = await connection.QueryAsync<CatalogEntity, CatalogLinkedCompanyEntity, CompanyEntity, CatalogEntity>(query3,
                (catalogEntity, catalogLinkedCompanyEntity, companyEntity) => {
                    result.Where(pEnt => pEnt.CoreProductCatalogID.Value == catalogLinkedCompanyEntity.CoreProductCatalogID).ToList().ForEach(lk =>
                    {
                        if (lk.EntidadesVinculadas == null)
                        {
                            lk.EntidadesVinculadas = new List<CompanyEntity>();
                        }
                        ((List<CompanyEntity>)lk.EntidadesVinculadas).Add(companyEntity);
                    });

                    return catalogEntity;
                }, new { }, sqlTran, true, splitOn: $"{tableCompanyAssigned.ID},{tableCompany.ID}");

                result.FirstOrDefault().UsuariosCreador = userRepository.GetItemByCompany(result.FirstOrDefault().CoreProductCatalogID.ToString(), Table, "UsuarioCreadorID").Result;
                result.FirstOrDefault().UsuariosModificador = userRepository.GetItemByCompany(result.FirstOrDefault().CoreProductCatalogID.ToString(), Table, "UsuarioModificadorID").Result;
            }

            return result.FirstOrDefault();
        }

        public override async Task<IEnumerable<CatalogEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;

            string sqlRelation = $"Catalog " +
                $"left join {tableCompany.Name} Company on Catalog.{tableCompany.ID} = Company.{tableCompany.ID} " +
                $"inner join {tableCurrency.Name} Currency on Catalog.{tableCurrency.ID} = Currency.{tableCurrency.ID} " +
                $"inner join {tableCatalogType.Name} CatalogType on Catalog.{tableCatalogType.ID} = CatalogType.{tableCatalogType.ID} " +
                $"left join {tableInflation.Name} Inflation on Catalog.{tableInflation.ID} = Inflation.{tableInflation.ID} " +
                $"left join {tableLifecycleStatus.Name} LifecycleStatus on Catalog.{tableLifecycleStatus.ID} = LifecycleStatus.{tableLifecycleStatus.ID}";

            string sqlFilter = $" WHERE Catalog.{nameof(CatalogEntity.ClienteID)} = {Client} ";
            if (filters != null)
            {
                foreach (Filter filter in filters)
                {
                    int iterator = countFilter++;
                    sqlFilter += $" AND Catalog.{filter.Column} {filter.Operator} @{filter.Column}{iterator} ";
                    string nameVar = $"{filter.Column}{iterator}";
                    dicParam.Add(nameVar, filter.Value);
                }
            }

            string sqlOrder = "";
            if (orders != null && orders.Count > 0)
            {
                foreach (string column in orders)
                {
                    sqlOrder += $"{(string.IsNullOrEmpty(sqlOrder) ? " order by Catalog." : ", ")} {column}";
                }
            }
            else
            {
                sqlOrder = $" order by Catalog.{Table.Code} ";
            }

            if (string.IsNullOrEmpty(direction) || (direction.ToLower() != "asc" && direction.ToLower() != "desc"))
            {
                direction = "ASC";
            }

            string sqlPagination = "";
            if (pageSize != -1 && pageIndex != -1)
            {
                sqlPagination = $"OFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            }

            string query = $"select TotalItems = COUNT(*) OVER(), * from {Table.Name} {sqlRelation} {sqlFilter} {sqlOrder} {sqlPagination}";

            var result = await connection.QueryAsync<CatalogEntity, PriceReadjustment, CompanyEntity, CurrencyEntity, CatalogTypeEntity,
                InflationEntity, CatalogLifecycleStatusEntity, CatalogEntity>(query,
                (catalogEntity, priceReadjustment, companyEntity, currencyEntity, catalogTypeEntity, inflationEntity, catalogLifecycleStatusEntity) =>
                {
                    catalogEntity.Monedas = currencyEntity;
                    catalogEntity.Entidades = companyEntity;
                    catalogEntity.CoreProductCatalogTipos = catalogTypeEntity;
                    catalogEntity.CoreProductCatalogEstadosGlobales = catalogLifecycleStatusEntity;

                    if (priceReadjustment != null)
                    {
                        catalogEntity.PricesReadjustment = priceReadjustment;
                        catalogEntity.PricesReadjustment.Inflacion = (inflationEntity != null) ? inflationEntity : null;
                    }
                    else
                    {
                        catalogEntity.PricesReadjustment = null;
                    }

                    return catalogEntity;
                }
                , dicParam, sqlTran, true, splitOn: $"Tipo,{tableCompany.ID},{tableCurrency.ID},{tableCatalogType.ID},{tableInflation.ID},{tableLifecycleStatus.ID}");

            result = result.Distinct();

            if (result.ToList() != null && result.ToList().Count > 0)
            {
                string ids = "";
                result.ToList().ForEach(x =>
                {
                    ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.CoreProductCatalogID}";
                });

                List<CatalogAssignedProductsEntity> listAssigned = new List<CatalogAssignedProductsEntity>();

                string query2 = $"SELECT * FROM {Table.Name} Catalog " +
                    $"inner join {tableProductAssigned.Name} ProductAssigned on Catalog.{nameof(CatalogEntity.CoreProductCatalogID)} = ProductAssigned.{nameof(CatalogEntity.CoreProductCatalogID)} " +
                    $"inner join {tableProduct.Name} Product on ProductAssigned.{nameof(CatalogAssignedProductsEntity.CoreProductCatalogServicios.CoreProductCatalogServicioID)} = Product.{nameof(ProductEntity.CoreProductCatalogServicioID)} " +
                    $"WHERE Catalog.{nameof(CatalogEntity.CoreProductCatalogID)} IN ({ids})";
                var result2 = await connection.QueryAsync<CatalogEntity, CatalogAssignedProductsEntity, ProductEntity, CatalogEntity>(query2,
                (catalogEntity, catalogAssignedProductsEntity, productEntity) =>
                {
                    if (catalogAssignedProductsEntity != null)
                    {
                        catalogAssignedProductsEntity.CoreProductCatalogs = catalogEntity;
                        catalogAssignedProductsEntity.CoreProductCatalogServicios = productEntity;

                        result.Where(pEnt => pEnt.CoreProductCatalogID.Value == catalogAssignedProductsEntity.CoreProductCatalogs.CoreProductCatalogID).ToList().ForEach(lk =>
                        {
                            if (lk.ServiciosVinculados == null)
                            {
                                lk.ServiciosVinculados = listAssigned;
                            }

                            listAssigned.Add(catalogAssignedProductsEntity);
                            lk.ServiciosVinculados = listAssigned;
                        });
                    }

                    return catalogEntity;
                }, new { }, sqlTran, true, splitOn: $"{tableProductAssigned.ID},{tableProduct.ID}");

                string query3 = $"SELECT * FROM {Table.Name} Catalog " +
                    $"inner join {tableCompanyAssigned.Name} CompanyAssigned on Catalog.{nameof(CatalogEntity.CoreProductCatalogID)} = CompanyAssigned.{nameof(CatalogLinkedCompanyEntity.CoreProductCatalogID)} " +
                    $"inner join {tableCompany.Name} Company on CompanyAssigned.{nameof(CatalogLinkedCompanyEntity.EntidadID)} = Company.{nameof(CompanyEntity.EntidadID)} " +
                    $"WHERE Catalog.{nameof(CatalogEntity.CoreProductCatalogID)} IN ({ids})";
                var result3 = await connection.QueryAsync<CatalogEntity, CatalogLinkedCompanyEntity, CompanyEntity, CatalogEntity>(query3,
                (catalogEntity, catalogLinkedCompanyEntity, companyEntity) => {
                    result.Where(pEnt => pEnt.CoreProductCatalogID.Value == catalogLinkedCompanyEntity.CoreProductCatalogID).ToList().ForEach(lk =>
                    {
                        if (lk.EntidadesVinculadas == null)
                        {
                            lk.EntidadesVinculadas = new List<CompanyEntity>();
                        }
                        ((List<CompanyEntity>)lk.EntidadesVinculadas).Add(companyEntity);
                    });

                    return catalogEntity;
                }, new { }, sqlTran, true, splitOn: $"{tableCompanyAssigned.ID},{tableCompany.ID}");

                result.FirstOrDefault().UsuariosCreador = userRepository.GetItemByCompany(result.FirstOrDefault().CoreProductCatalogID.ToString(), Table, "UsuarioCreadorID").Result;
                result.FirstOrDefault().UsuariosModificador = userRepository.GetItemByCompany(result.FirstOrDefault().CoreProductCatalogID.ToString(), Table, "UsuarioModificadorID").Result;
            }

            return result.ToList();
        }

        public override async Task<int> Delete(CatalogEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key AND {nameof(CatalogEntity.ClienteID)} = @{nameof(CatalogEntity.ClienteID)}";
            int numReg = 0;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreProductCatalogID,
                    ClienteID = obj.ClienteID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        private class CatalogLinkedCompanyEntity
        {
            public readonly int? CoreProductCatalogEntidadAsignadaID;
            public readonly int CoreProductCatalogID;
            public readonly int EntidadID;
        }
    }
}

