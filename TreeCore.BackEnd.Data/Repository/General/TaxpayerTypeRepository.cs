using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.General
{
    public class TaxpayerTypeRepository : BaseRepository<TaxpayerTypeEntity>
    {
        public override Table Table => TableNames.TaxpayerType;

        public TaxpayerTypeRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<TaxpayerTypeEntity> InsertSingle(TaxpayerTypeEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(TaxpayerTypeEntity.ClienteID)},{nameof(TaxpayerTypeEntity.Codigo)}, {nameof(TaxpayerTypeEntity.TipoContribuyente)}, {nameof(TaxpayerTypeEntity.Descripcion)}, {nameof(TaxpayerTypeEntity.Activo)}, " +
                $"{ nameof(TaxpayerTypeEntity.Defecto)})" +
                $"values (@{nameof(TaxpayerTypeEntity.ClienteID)},@{nameof(TaxpayerTypeEntity.Codigo)}, @{nameof(TaxpayerTypeEntity.TipoContribuyente)}, @{nameof(TaxpayerTypeEntity.Descripcion)}, @{nameof(TaxpayerTypeEntity.Activo)}, " +
                $" @{ nameof(TaxpayerTypeEntity.Defecto)});" +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.Codigo,
                TipoContribuyente = obj.TipoContribuyente,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto
            }, sqlTran)).First();

            return TaxpayerTypeEntity.UpdateId(obj, newId);
        }
        public override async Task<TaxpayerTypeEntity> UpdateSingle(TaxpayerTypeEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(TaxpayerTypeEntity.Codigo)} = @{nameof(TaxpayerTypeEntity.Codigo)}, " +
                $" {nameof(TaxpayerTypeEntity.TipoContribuyente)} =   @{nameof(TaxpayerTypeEntity.TipoContribuyente)}, " +
                $" {nameof(TaxpayerTypeEntity.Descripcion)} =  @{nameof(TaxpayerTypeEntity.Descripcion)}, " +
                $" {nameof(TaxpayerTypeEntity.Activo)} = @{nameof(TaxpayerTypeEntity.Activo)}, " +
                $" {nameof(TaxpayerTypeEntity.Defecto)} =  @{ nameof(TaxpayerTypeEntity.Defecto)} " +
                $" where {nameof(TaxpayerTypeEntity.TipoContribuyenteID)} = @{nameof(TaxpayerTypeEntity.TipoContribuyenteID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                TipoContribuyente = obj.TipoContribuyente,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                TipoContribuyenteID = obj.TipoContribuyenteID
            }, sqlTran));
            return obj;
        }

        public override async Task<TaxpayerTypeEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<TaxpayerTypeEntity>($"select * from {Table.Name} where {Table.Code} = @code" +
                $" AND {nameof(TaxpayerTypeEntity.ClienteID)} = @{nameof(TaxpayerTypeEntity.ClienteID)}", new
            {
                code = code,
                ClienteID = client
            }, sqlTran);

        }

        public override async Task<int> Delete(TaxpayerTypeEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key " +
                $" AND {nameof(TaxpayerTypeEntity.ClienteID)} = @{nameof(TaxpayerTypeEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.TipoContribuyenteID,
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
