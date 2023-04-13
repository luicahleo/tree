using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.ProductCatalog
{
    public class CatalogTypeRepository : BaseRepository<CatalogTypeEntity>
    {
        public override Table Table => TableNames.CatalogType;

        public CatalogTypeRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<CatalogTypeEntity> InsertSingle(CatalogTypeEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(CatalogTypeEntity.ClienteID)},{nameof(CatalogTypeEntity.Codigo)}, {nameof(CatalogTypeEntity.Nombre)}, " +
                $"{nameof(CatalogTypeEntity.Descripcion)}, {nameof(CatalogTypeEntity.Activo)}, " +
                $"{nameof(CatalogTypeEntity.EsCompra)}, {nameof(CatalogTypeEntity.EsVenta)}, " +
                $"{ nameof(CatalogTypeEntity.Defecto)})" +
                $"values (@{nameof(CatalogTypeEntity.ClienteID)},@{nameof(CatalogTypeEntity.Codigo)}, @{nameof(CatalogTypeEntity.Nombre)}, " +
                $"@{nameof(CatalogTypeEntity.Descripcion)}, @{nameof(CatalogTypeEntity.Activo)}, " +
                $"@{nameof(CatalogTypeEntity.EsCompra)}, @{nameof(CatalogTypeEntity.EsVenta)}, " +
                $"@{ nameof(CatalogTypeEntity.Defecto)});" +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                EsCompra = obj.EsCompra,
                EsVenta = obj.EsVenta
            }, sqlTran)).First();

            return CatalogTypeEntity.UpdateId(obj, newId);
        }
        public override async Task<CatalogTypeEntity> UpdateSingle(CatalogTypeEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(CatalogTypeEntity.Codigo)} = @{nameof(CatalogTypeEntity.Codigo)}, " +
                $" {nameof(CatalogTypeEntity.Nombre)} =   @{nameof(CatalogTypeEntity.Nombre)}, " +
                $" {nameof(CatalogTypeEntity.Descripcion)} =  @{nameof(CatalogTypeEntity.Descripcion)}, " +
                $" {nameof(CatalogTypeEntity.Activo)} = @{nameof(CatalogTypeEntity.Activo)}, " +
                $" {nameof(CatalogTypeEntity.Defecto)} =  @{nameof(CatalogTypeEntity.Defecto)}, " +
                $" {nameof(CatalogTypeEntity.EsCompra)} = @{nameof(CatalogTypeEntity.EsCompra)}, " +
                $" {nameof(CatalogTypeEntity.EsVenta)} = @{nameof(CatalogTypeEntity.EsVenta)} " +
                $" where {nameof(CatalogTypeEntity.CoreProductCatalogTipoID)} = @{nameof(CatalogTypeEntity.CoreProductCatalogTipoID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                CoreProductCatalogTipoID = obj.CoreProductCatalogTipoID,
                EsCompra = obj.EsCompra,
                EsVenta = obj.EsVenta
            }, sqlTran));
            return obj;
        }

        public override async Task<CatalogTypeEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<CatalogTypeEntity>($"select * from {Table.Name} where {Table.Code} = @code" +
                $" AND {nameof(CatalogEntity.ClienteID)} = @{nameof(CatalogEntity.ClienteID)}", new
            {
                code = code,
                ClienteID = client
            }, sqlTran);

        }

        public override async Task<int> Delete(CatalogTypeEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key AND {nameof(CatalogEntity.ClienteID)} = @{nameof(CatalogEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreProductCatalogTipoID,
                    ClienteID = obj.ClienteID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }       
    }
}
