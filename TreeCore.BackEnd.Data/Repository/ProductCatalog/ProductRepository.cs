using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.Shared.Data.Db;
using System.Collections.Generic;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.ProductCatalog
{
    public class ProductRepository : BaseRepository<ProductEntity>
    {
        public override Table Table => TableNames.Product;
        public Table tableCompany => TableNames.Company;
        public Table tableProductType => TableNames.ProductType;
        public Table tableProductLinked => TableNames.ProductProductLinked;
        public Table tableProductEntityLinked => TableNames.ProductEntityLinked;

        public ProductRepository(TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<ProductEntity> InsertSingle(ProductEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(ProductEntity.Codigo)}, {nameof(ProductEntity.Nombre)}," +
                $"{nameof(ProductEntity.Cantidad)}, {nameof(ProductEntity.CoreFormulaID)}, {nameof(ProductEntity.CronFormat)}, " +
                $"{nameof(ProductEntity.CoreObjetoNegocioTipoID)}, {nameof(ProductEntity.CoreProductCatalogServiciosTipos.CoreProductCatalogServicioTipoID)}, { nameof(ProductEntity.Unidad)}, " +
                $"{nameof(ProductEntity.FechaCreacion)}, {nameof(ProductEntity.UsuarioCreadorID)}, " +
                $"{nameof(ProductEntity.FechaModificacion)}, {nameof(ProductEntity.UsuarioID)}," +
                $"{nameof(ProductEntity.EsPack)}, {nameof(ProductEntity.Publico)}, {nameof(ProductEntity.Descripcion)})" +
                $"values (@{nameof(ProductEntity.Codigo)}, @{nameof(ProductEntity.Nombre)}, @{nameof(ProductEntity.Cantidad)}, " +
                $" @{nameof(ProductEntity.CoreFormulaID)}, @{nameof(ProductEntity.CronFormat)}, @{nameof(ProductEntity.CoreObjetoNegocioTipoID)}, " +
                $" @{nameof(ProductEntity.CoreProductCatalogServiciosTipos.CoreProductCatalogServicioTipoID)}, @{nameof(ProductEntity.Unidad)}, @{nameof(ProductEntity.FechaCreacion)}, " +
                $" @{nameof(ProductEntity.UsuarioCreadorID)}, @{nameof(ProductEntity.FechaModificacion)}, " +
                $" @{nameof(ProductEntity.UsuarioID)}, @{nameof(ProductEntity.EsPack)}, @{nameof(ProductEntity.Publico)}, @{nameof(ProductEntity.Descripcion)});" +
                $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            int newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                CronFormat = obj.CronFormat,
                Unidad = obj.Unidad,
                FechaModificacion = obj.FechaModificacion,
                UsuarioID = obj.UsuarioID,
                CoreObjetoNegocioTipoID = obj.CoreObjetoNegocioTipoID,
                CoreFormulaID = obj.CoreFormulaID,
                CoreProductCatalogServicioTipoID = obj.CoreProductCatalogServiciosTipos.CoreProductCatalogServicioTipoID,
                FechaCreacion = obj.FechaCreacion,
                UsuarioCreadorID = obj.UsuarioCreadorID,
                Cantidad = obj.Cantidad,
                EsPack = obj.EsPack,
                Publico = obj.Publico,
                Descripcion = obj.Descripcion
            }, sqlTran)).First();

            if (obj.ServiciosVinculados != null)
            {
                foreach (ProductEntity servicioVinculado in obj.ServiciosVinculados.ToList())
                {
                    sql = $"INSERT INTO {tableProductLinked.Name} ({nameof(ProductProductLinkedEntity.CoreProductCatalogServicioPadreID)}, " +
                    $"{nameof(ProductProductLinkedEntity.CoreProductCatalogServicioHijoID)})" +
                    $"values (@{nameof(newId)}, " +
                    $"@{nameof(ProductProductLinkedEntity.CoreProductCatalogServicioHijoID)});" +
                    $"SELECT SCOPE_IDENTITY();";

                    var idLinkedProduct = (await connection.QueryAsync<int>(sql, new
                    {
                        newId = newId,
                        CoreProductCatalogServicioHijoID = servicioVinculado.CoreProductCatalogServicioID
                    }, sqlTran)).First();
                };
            }

            if (obj.EntidadesVinculadas != null)
            {
                foreach (CompanyEntity entidadVinculada in obj.EntidadesVinculadas.ToList())
                {
                    sql = $"INSERT INTO {tableProductEntityLinked.Name} ({nameof(ProductLinkedEntity.EntidadID)}, " +
                    $"{nameof(ProductLinkedEntity.CoreProductCatalogServicioID)})" +
                    $"values (@{nameof(newId)}, " +
                    $"@{nameof(ProductLinkedEntity.CoreProductCatalogServicioID)});" +
                    $"SELECT SCOPE_IDENTITY();";

                    var idLinkedProduct = (await connection.QueryAsync<int>(sql, new
                    {
                        newId = newId,
                        EntidadID = entidadVinculada.EntidadID
                    }, sqlTran)).First();
                };
            }

            return ProductEntity.UpdateId(obj, newId);
        }

        public override async Task<ProductEntity> UpdateSingle(ProductEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            #region Remove linked Products
            string sql = $"delete from {tableProductLinked.Name} Where {nameof(ProductProductLinkedEntity.CoreProductCatalogServicioPadreID)} = @{nameof(ProductEntity.CoreProductCatalogServicioID)}";
            var numReg = await connection.ExecuteAsync(sql, new
            {
                CoreProductCatalogServicioID = obj.CoreProductCatalogServicioID
            }, sqlTran);
            #endregion

            #region Add linked Products
            if (obj.ServiciosVinculados != null)
            {
                foreach (ProductEntity servicioVinculado in obj.ServiciosVinculados.ToList())
                {
                    sql = $"INSERT INTO {tableProductLinked.Name} ({nameof(ProductProductLinkedEntity.CoreProductCatalogServicioPadreID)}, " +
                    $"{nameof(ProductProductLinkedEntity.CoreProductCatalogServicioHijoID)})" +
                    $"values (@{nameof(ProductEntity.CoreProductCatalogServicioID)}, " +
                    $"@CoreProductCatalogServicioChild);" +
                    $"SELECT SCOPE_IDENTITY();";

                    var idLinkedProduct = (await connection.QueryAsync<int>(sql, new
                    {
                        CoreProductCatalogServicioID = obj.CoreProductCatalogServicioID,
                        CoreProductCatalogServicioChild = servicioVinculado.CoreProductCatalogServicioID
                    }, sqlTran)).First();
                };
            }
            #endregion

            sql = $"update {Table.Name} set {nameof(ProductEntity.Codigo)} = @{nameof(ProductEntity.Codigo)}, " +
                $" {nameof(ProductEntity.Nombre)} =   @{nameof(ProductEntity.Nombre)}, " +
                $" {nameof(ProductEntity.Cantidad)} =  @{nameof(ProductEntity.Cantidad)}, " +
                $" {nameof(ProductEntity.CoreFormulaID)} = @{nameof(ProductEntity.CoreFormulaID)}, " +
                $" {nameof(ProductEntity.CronFormat)} = @{nameof(ProductEntity.CronFormat)}, " +
                $" {nameof(ProductEntity.CoreObjetoNegocioTipoID)} =  @{ nameof(ProductEntity.CoreObjetoNegocioTipoID)}, " +
                $" {nameof(ProductEntity.CoreProductCatalogServiciosTipos.CoreProductCatalogServicioTipoID)} =  @{ nameof(ProductEntity.CoreProductCatalogServiciosTipos.CoreProductCatalogServicioTipoID)}, " +
                $" {nameof(ProductEntity.Unidad)} =  @{ nameof(ProductEntity.Unidad)}, " +
                $" {nameof(ProductEntity.FechaModificacion)} =  @{ nameof(ProductEntity.FechaModificacion)}, " +
                $" {nameof(ProductEntity.FechaCreacion)} =  @{ nameof(ProductEntity.FechaCreacion)}, " +
                $" {nameof(ProductEntity.UsuarioCreadorID)} =  @{ nameof(ProductEntity.UsuarioCreadorID)}, " +
                $" {nameof(ProductEntity.UsuarioID)} =  @{ nameof(ProductEntity.UsuarioID)}, " +
                $" {nameof(ProductEntity.EsPack)} =  @{ nameof(ProductEntity.EsPack)}, " +
                $" {nameof(ProductEntity.Publico)} =  @{ nameof(ProductEntity.Publico)}, " +
                $" {nameof(ProductEntity.Descripcion)} = @{nameof(ProductEntity.Descripcion)}" +
                $" where {nameof(ProductEntity.CoreProductCatalogServicioID)} = @{nameof(ProductEntity.CoreProductCatalogServicioID)} ";

            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                CoreProductCatalogServicioID = obj.CoreProductCatalogServicioID,
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                CronFormat = obj.CronFormat,
                Unidad = obj.Unidad,
                FechaModificacion = obj.FechaModificacion,
                UsuarioID = obj.UsuarioID,
                CoreObjetoNegocioTipoID = obj.CoreObjetoNegocioTipoID,
                CoreFormulaID = obj.CoreFormulaID,
                CoreProductCatalogServicioTipoID = obj.CoreProductCatalogServiciosTipos.CoreProductCatalogServicioTipoID,
                FechaCreacion = obj.FechaCreacion,
                UsuarioCreadorID = obj.UsuarioCreadorID,
                Cantidad = obj.Cantidad,
                EsPack = obj.EsPack,
                Publico = obj.Publico,
                Descripcion = obj.Descripcion
            }, sqlTran));

            return obj;
        }

        public override async Task<ProductEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} p " +
                $" inner join {tableProductType.Name} pt on p.{tableProductType.ID} = pt.{tableProductType.ID}" +
                $" where p.{Table.Code} = @code";

            var result = await connection.QueryAsync<ProductEntity, ProductTypeEntity, ProductEntity>(sql,
            (productEntity, productTypeEntity) =>
            {
                productEntity.CoreProductCatalogServiciosTipos = productTypeEntity;
                return productEntity;
            }, new { code = code }, sqlTran, true, splitOn: $"{tableProductType.ID}");

            if (result.ToList() != null && result.ToList().Count > 0)
            {
                string ids = "";
                result.ToList().ForEach(x =>
                {
                    ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.CoreProductCatalogServicioID}";
                });

                string query2 = $"SELECT * FROM {Table.Name} p " +
                    $"inner join {tableProductLinked.Name} lk on p.{nameof(ProductEntity.CoreProductCatalogServicioID)} = lk.{nameof(ProductProductLinkedEntity.CoreProductCatalogServicioPadreID)} " +
                    $"inner join {Table.Name} linkP on lk.{nameof(ProductProductLinkedEntity.CoreProductCatalogServicioHijoID)} = linkp.{nameof(ProductEntity.CoreProductCatalogServicioID)} " +
                    $"WHERE p.{nameof(ProductEntity.CoreProductCatalogServicioID)} IN ({ids})";
                var result2 = await connection.QueryAsync<ProductEntity, ProductProductLinkedEntity, ProductEntity, ProductEntity>(query2,
                (productEntity, productProductLinkedEntity, productEntityLink) =>
                {
                    result.Where(pEnt => pEnt.CoreProductCatalogServicioID.Value == productProductLinkedEntity.CoreProductCatalogServicioPadreID).ToList().ForEach(lk =>
                    {
                        if (lk.ServiciosVinculados == null)
                        {
                            lk.ServiciosVinculados = new List<ProductEntity>();
                            lk.EsPack = true;
                        }
                        ((List<ProductEntity>)lk.ServiciosVinculados).Add(productEntityLink);
                    });

                    return productEntity;
                }, new { }, sqlTran, true, splitOn: $"{tableProductLinked.ID},{Table.ID}");

                string query3 = $"SELECT * FROM {Table.Name} p " +
                    $"inner join {tableProductEntityLinked.Name} lk on p.{nameof(ProductEntity.CoreProductCatalogServicioID)} = lk.{nameof(ProductLinkedEntity.CoreProductCatalogServicioID)} " +
                    $"inner join {tableCompany.Name} linkP on lk.{nameof(ProductLinkedEntity.EntidadID)} = linkp.{nameof(CompanyEntity.EntidadID)} " +
                    $"WHERE p.{nameof(ProductEntity.CoreProductCatalogServicioID)} IN ({ids})";
                var result3 = await connection.QueryAsync<CompanyEntity, ProductLinkedEntity, CompanyEntity, CompanyEntity>(query3,
                (companyEntity, productLinkedEntity, companyEntityLink) =>
                {
                    result.Where(pEnt => pEnt.CoreProductCatalogServicioID.Value == productLinkedEntity.CoreProductCatalogServicioID).ToList().ForEach(lk =>
                    {
                        if (lk.EntidadesVinculadas == null)
                        {
                            lk.EntidadesVinculadas = new List<CompanyEntity>();
                        }
                        ((List<CompanyEntity>)lk.EntidadesVinculadas).Add(companyEntityLink);
                    });

                    return companyEntity;
                }, new { }, sqlTran, true, splitOn: $"{tableProductEntityLinked.ID},{Table.ID}");
            }

            return result.FirstOrDefault();
        }

        public override async Task<IEnumerable<ProductEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;
            string sType = "";

            string sqlRelation = $"p inner join {tableProductType.Name} pt on p.{tableProductType.ID} = pt.{tableProductType.ID} ";

            string sqlFilter = "";
            if (filters != null && filters.Count > 0)
            {
                sqlFilter = " WHERE ";

                if (filters[0].Type != null && filters.Count > 1)
                {
                    sType = filters[0].Type;
                }
                else
                {
                    sType = Filter.Types.AND;
                }

                sqlFilter += Filter.BuilFilters(filters, sType, countFilter, out countFilter, dicParam, out dicParam, "p");
            }

            string sqlOrder = "";
            if (orders != null && orders.Count > 0)
            {
                foreach (string column in orders)
                {
                    sqlOrder += $"{(string.IsNullOrEmpty(sqlOrder) ? " order by p." : ", ")} {column}";
                }
            }
            else
            {
                sqlOrder = $" order by p.{Table.Code} ";
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

            string query = $"select TotalItems = COUNT(*) OVER(), * from {Table.Name} {sqlRelation} {sqlFilter} {sqlOrder} {direction} {sqlPagination}";

            var result = await connection.QueryAsync<ProductEntity, ProductTypeEntity, ProductEntity>(query,
            (productEntity, productTypeEntity) =>
            {
                productEntity.CoreProductCatalogServiciosTipos = productTypeEntity;

                return productEntity;
            },
            dicParam, sqlTran, true, splitOn: $"{tableProductType.ID}");

            result = result.Distinct();

            if (result.ToList() != null && result.ToList().Count > 0)
            {
                string ids = "";
                result.ToList().ForEach(x =>
                {
                    ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.CoreProductCatalogServicioID}";
                });

                string query2 = $"SELECT * FROM {Table.Name} p " +
                    $"inner join {tableProductLinked.Name} lk on p.{nameof(ProductEntity.CoreProductCatalogServicioID)} = lk.{nameof(ProductProductLinkedEntity.CoreProductCatalogServicioPadreID)} " +
                    $"inner join {Table.Name} linkP on lk.{nameof(ProductProductLinkedEntity.CoreProductCatalogServicioHijoID)} = linkp.{nameof(ProductEntity.CoreProductCatalogServicioID)} " +
                    $"WHERE p.{nameof(ProductEntity.CoreProductCatalogServicioID)} IN ({ids})";
                var result2 = await connection.QueryAsync<ProductEntity, ProductProductLinkedEntity, ProductEntity, ProductEntity>(query2,
                (productEntity, productProductLinkedEntity, productEntityLink) =>
                {
                    result.Where(pEnt => pEnt.CoreProductCatalogServicioID.Value == productProductLinkedEntity.CoreProductCatalogServicioPadreID).ToList().ForEach(lk =>
                    {
                        if (lk.ServiciosVinculados == null)
                        {
                            lk.ServiciosVinculados = new List<ProductEntity>();
                            lk.EsPack = true;
                        }
                        ((List<ProductEntity>)lk.ServiciosVinculados).Add(productEntityLink);
                    });

                    return productEntity;
                }, new { }, sqlTran, true, splitOn: $"{tableProductLinked.ID},{Table.ID}");

                string query3 = $"SELECT * FROM {Table.Name} p " +
                    $"inner join {tableProductEntityLinked.Name} lk on p.{nameof(ProductEntity.CoreProductCatalogServicioID)} = lk.{nameof(ProductLinkedEntity.CoreProductCatalogServicioID)} " +
                    $"inner join {tableCompany.Name} linkP on lk.{nameof(ProductLinkedEntity.EntidadID)} = linkp.{nameof(CompanyEntity.EntidadID)} " +
                    $"WHERE p.{nameof(ProductEntity.CoreProductCatalogServicioID)} IN ({ids})";
                var result3 = await connection.QueryAsync<CompanyEntity, ProductLinkedEntity, CompanyEntity, CompanyEntity>(query3,
                (companyEntity, productLinkedEntity, companyEntityLink) =>
                {
                    result.Where(pEnt => pEnt.CoreProductCatalogServicioID.Value == productLinkedEntity.CoreProductCatalogServicioID).ToList().ForEach(lk =>
                    {
                        if (lk.EntidadesVinculadas == null)
                        {
                            lk.EntidadesVinculadas = new List<CompanyEntity>();
                        }
                        ((List<CompanyEntity>)lk.EntidadesVinculadas).Add(companyEntityLink);
                    });

                    return companyEntity;
                }, new { }, sqlTran, true, splitOn: $"{tableProductEntityLinked.ID},{Table.ID}");
            }



            return result.ToList();
        }

        public override async Task<int> Delete(ProductEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreProductCatalogServicioID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        private class ProductProductLinkedEntity
        {
            public readonly int? CoreProductCatalogServiciosServicioAsignadoID;
            public readonly int CoreProductCatalogServicioPadreID;
            public readonly int CoreProductCatalogServicioHijoID;
        }

        private class ProductLinkedEntity
        {
            public readonly int? CoreProductCatalogServicioEntidadID;
            public readonly int CoreProductCatalogServicioID;
            public readonly int EntidadID;
        }
    }
}

