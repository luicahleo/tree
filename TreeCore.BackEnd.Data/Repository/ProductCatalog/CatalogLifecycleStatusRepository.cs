using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.ProductCatalog
{
    public class CatalogLifecycleStatusRepository : BaseRepository<CatalogLifecycleStatusEntity>
    {
        public override Table Table => TableNames.CatalogLifecycleStatus;

        public CatalogLifecycleStatusRepository(TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<CatalogLifecycleStatusEntity> InsertSingle(CatalogLifecycleStatusEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(CatalogLifecycleStatusEntity.Codigo)}, {nameof(CatalogLifecycleStatusEntity.Nombre)}, " +
                $"{nameof(CatalogLifecycleStatusEntity.Descripcion)}, {nameof(CatalogLifecycleStatusEntity.Activo)}, " +
                $"{ nameof(CatalogLifecycleStatusEntity.Defecto)}, {nameof(CatalogLifecycleStatusEntity.ClienteID)})" +
                $"values (@{nameof(CatalogLifecycleStatusEntity.Codigo)}, @{nameof(CatalogLifecycleStatusEntity.Nombre)}, " +
                $"@{nameof(CatalogLifecycleStatusEntity.Descripcion)}, @{nameof(CatalogLifecycleStatusEntity.Activo)}, " +
                $" @{ nameof(CatalogLifecycleStatusEntity.Defecto)}, @{nameof(CatalogLifecycleStatusEntity.ClienteID)});" +
                 $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                EntidadCliente = false,
                ClienteID = obj.ClienteID
            }, sqlTran)).First();

            return CatalogLifecycleStatusEntity.UpdateId(obj, newId);
        }

        public override async Task<CatalogLifecycleStatusEntity> UpdateSingle(CatalogLifecycleStatusEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(CatalogLifecycleStatusEntity.Codigo)} = @{nameof(CatalogLifecycleStatusEntity.Codigo)}, " +
                $" {nameof(CatalogLifecycleStatusEntity.Nombre)} =   @{nameof(CatalogLifecycleStatusEntity.Nombre)}, " +
                $" {nameof(CatalogLifecycleStatusEntity.Descripcion)} =  @{nameof(CatalogLifecycleStatusEntity.Descripcion)}, " +
                $" {nameof(CatalogLifecycleStatusEntity.Activo)} = @{nameof(CatalogLifecycleStatusEntity.Activo)}, " +
                $" {nameof(CatalogLifecycleStatusEntity.ClienteID)} = @{nameof(CatalogLifecycleStatusEntity.ClienteID)}, " +
                $" {nameof(CatalogLifecycleStatusEntity.Defecto)} =  @{ nameof(CatalogLifecycleStatusEntity.Defecto)} " +
                $" where {nameof(CatalogLifecycleStatusEntity.CoreProductCatalogEstadoGlobalID)} = @{nameof(CatalogLifecycleStatusEntity.CoreProductCatalogEstadoGlobalID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                CoreProductCatalogEstadoGlobalID = obj.CoreProductCatalogEstadoGlobalID,
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                ClienteID = obj.ClienteID
            }, sqlTran));

            return obj;
        }

        public override async Task<CatalogLifecycleStatusEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<CatalogLifecycleStatusEntity>($"select * from {Table.Name} where {Table.Code} = @code", new
            {
                code = code
            }, sqlTran);

        }

        public override async Task<int> Delete(CatalogLifecycleStatusEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreProductCatalogEstadoGlobalID
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

