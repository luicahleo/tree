using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.General
{
    public class BankRepository : BaseRepository<BankEntity>
    {
        public override Table Table => TableNames.Bank;

        public BankRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<BankEntity> InsertSingle(BankEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(BankEntity.ClienteID)},{nameof(BankEntity.CodigoBanco)}, " +
                $"{nameof(BankEntity.Banco)}, {nameof(BankEntity.Descripcion)}, {nameof(BankEntity.Activo)}, " +
                $"{ nameof(BankEntity.Defecto)})" +
                $"values (@{nameof(BankEntity.ClienteID)},@{nameof(BankEntity.CodigoBanco)}, @{nameof(BankEntity.Banco)}, " +
                $"@{nameof(BankEntity.Descripcion)}, @{nameof(BankEntity.Activo)}, " +
                $" @{ nameof(BankEntity.Defecto)});" +
                $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                CodigoBanco = obj.CodigoBanco,
                Banco = obj.Banco,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto
            }, sqlTran)).First();

            return BankEntity.UpdateId(obj, newId);
        }
        public override async Task<BankEntity> UpdateSingle(BankEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(BankEntity.CodigoBanco)} = @{nameof(BankEntity.CodigoBanco)}, " +
                $" {nameof(BankEntity.Banco)} =   @{nameof(BankEntity.Banco)}, " +
                $" {nameof(BankEntity.Descripcion)} =  @{nameof(BankEntity.Descripcion)}, " +
                $" {nameof(BankEntity.Activo)} = @{nameof(BankEntity.Activo)}, " +
                $" {nameof(BankEntity.Defecto)} =  @{ nameof(BankEntity.Defecto)} " +
                $" where {nameof(BankEntity.BancoID)} = @{nameof(BankEntity.BancoID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                CodigoBanco = obj.CodigoBanco,
                Banco = obj.Banco,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                BancoID = obj.BancoID
            }, sqlTran));
            return obj;
        }

        public override async Task<BankEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<BankEntity>($"select * from {Table.Name} where {Table.Code} = @code AND {nameof(BankEntity.ClienteID)} = @{nameof(BankEntity.ClienteID)}", new
            {
                code = code,
                ClienteID = Client
            }, sqlTran);
        }

        public override async Task<int> Delete(BankEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key AND {nameof(BankEntity.ClienteID)} = @{nameof(BankEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.BancoID,
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
