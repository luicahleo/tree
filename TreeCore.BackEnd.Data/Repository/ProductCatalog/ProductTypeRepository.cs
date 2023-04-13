using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.ProductCatalog
{
    public class ProductTypeRepository: BaseRepository<ProductTypeEntity>
    {
        public override Table Table => TableNames.ProductType;

        public ProductTypeRepository(TransactionalWrapper conexion): base(conexion) { }

        public override async Task<ProductTypeEntity> InsertSingle (ProductTypeEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(ProductTypeEntity.Codigo)}, {nameof(ProductTypeEntity.Nombre)}, {nameof(ProductTypeEntity.Descripcion)}, {nameof(ProductTypeEntity.Activo)}, " +
                $"{ nameof(ProductTypeEntity.Defecto)})" +
                $"values (@{nameof(ProductTypeEntity.Codigo)}, @{nameof(ProductTypeEntity.Nombre)}, @{nameof(ProductTypeEntity.Descripcion)}, @{nameof(ProductTypeEntity.Activo)}, " +
                $" @{ nameof(ProductTypeEntity.Defecto)});" +
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
                EntidadCliente = false
            }, sqlTran)).First();

            return ProductTypeEntity.UpdateId(obj, newId);
        }

        public override async Task<ProductTypeEntity> UpdateSingle(ProductTypeEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(ProductTypeEntity.Codigo)} = @{nameof(ProductTypeEntity.Codigo)}, " +
                $" {nameof(ProductTypeEntity.Nombre)} =   @{nameof(ProductTypeEntity.Nombre)}, " +
                $" {nameof(ProductTypeEntity.Descripcion)} =  @{nameof(ProductTypeEntity.Descripcion)}, " +
                $" {nameof(ProductTypeEntity.Activo)} = @{nameof(ProductTypeEntity.Activo)}, " +
                $" {nameof(ProductTypeEntity.Defecto)} =  @{ nameof(ProductTypeEntity.Defecto)} " +
                $" where {nameof(ProductTypeEntity.CoreProductCatalogServicioTipoID)} = @{nameof(ProductTypeEntity.CoreProductCatalogServicioTipoID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                CoreProductCatalogServicioTipoID = obj.CoreProductCatalogServicioTipoID,
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto
            }, sqlTran));

            return obj;
        }

        public override async Task<ProductTypeEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<ProductTypeEntity>($"select * from {Table.Name} where {Table.Code} = @code", new
            {
                code = code
            }, sqlTran);

        }

        public override async Task<int> Delete(ProductTypeEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreProductCatalogServicioTipoID
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
