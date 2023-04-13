using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.General
{
    public class TaxIdentificationNumberCategoryRepository : BaseRepository<TaxIdentificationNumberCategoryEntity>
    {
        public override Table Table => TableNames.TaxIdentificationNumberCategory;

        public TaxIdentificationNumberCategoryRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<TaxIdentificationNumberCategoryEntity> InsertSingle(TaxIdentificationNumberCategoryEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(TaxIdentificationNumberCategoryEntity.ClienteID)},{nameof(TaxIdentificationNumberCategoryEntity.Codigo)}, {nameof(TaxIdentificationNumberCategoryEntity.Nombre)}, {nameof(TaxIdentificationNumberCategoryEntity.Descripcion)}, {nameof(TaxIdentificationNumberCategoryEntity.Activo)}, " +
                $"{ nameof(TaxIdentificationNumberCategoryEntity.Defecto)})" +
                $"values (@{nameof(TaxIdentificationNumberCategoryEntity.ClienteID)},@{nameof(TaxIdentificationNumberCategoryEntity.Codigo)}, @{nameof(TaxIdentificationNumberCategoryEntity.Nombre)}, @{nameof(TaxIdentificationNumberCategoryEntity.Descripcion)}, @{nameof(TaxIdentificationNumberCategoryEntity.Activo)}, " +
                $" @{ nameof(TaxIdentificationNumberCategoryEntity.Defecto)});" +
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
                Defecto = obj.Defecto
            }, sqlTran)).First();

            return TaxIdentificationNumberCategoryEntity.UpdateId(obj, newId);
        }
        public override async Task<TaxIdentificationNumberCategoryEntity> UpdateSingle(TaxIdentificationNumberCategoryEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(TaxIdentificationNumberCategoryEntity.Codigo)} = @{nameof(TaxIdentificationNumberCategoryEntity.Codigo)}, " +
                $" {nameof(TaxIdentificationNumberCategoryEntity.Nombre)} =   @{nameof(TaxIdentificationNumberCategoryEntity.Nombre)}, " +
                $" {nameof(TaxIdentificationNumberCategoryEntity.Descripcion)} =  @{nameof(TaxIdentificationNumberCategoryEntity.Descripcion)}, " +
                $" {nameof(TaxIdentificationNumberCategoryEntity.Activo)} = @{nameof(TaxIdentificationNumberCategoryEntity.Activo)}, " +
                $" {nameof(TaxIdentificationNumberCategoryEntity.Defecto)} =  @{ nameof(TaxIdentificationNumberCategoryEntity.Defecto)} " +
                $" where {nameof(TaxIdentificationNumberCategoryEntity.SAPTipoNIFID)} = @{nameof(TaxIdentificationNumberCategoryEntity.SAPTipoNIFID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                SAPTipoNIFID = obj.SAPTipoNIFID
            }, sqlTran));
            return obj;
        }

        public override async Task<TaxIdentificationNumberCategoryEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<TaxIdentificationNumberCategoryEntity>($"select * from {Table.Name} where {Table.Code} = @code" +
                $" AND {nameof(TaxIdentificationNumberCategoryEntity.ClienteID)} = @{nameof(TaxIdentificationNumberCategoryEntity.ClienteID)}", new
            {
                code = code,
                ClienteID = client
            }, sqlTran);

        }

        public override async Task<int> Delete(TaxIdentificationNumberCategoryEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key " +
                $" AND {nameof(TaxIdentificationNumberCategoryEntity.ClienteID)} = @{nameof(TaxIdentificationNumberCategoryEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.SAPTipoNIFID,
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
