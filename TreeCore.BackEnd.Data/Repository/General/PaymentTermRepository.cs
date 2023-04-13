using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.General
{
    public class PaymentTermRepository : BaseRepository<PaymentTermEntity>
    {
        public override Table Table => TableNames.PaymentTerm;

        public PaymentTermRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<PaymentTermEntity> InsertSingle(PaymentTermEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(PaymentTermEntity.ClienteID)},{nameof(PaymentTermEntity.Codigo)}, {nameof(PaymentTermEntity.CondicionPago)}, {nameof(PaymentTermEntity.Descripcion)}, {nameof(PaymentTermEntity.Activo)}, " +
                $"{ nameof(PaymentTermEntity.Defecto)})" +
                $"values (@{nameof(PaymentTermEntity.ClienteID)},@{nameof(PaymentTermEntity.Codigo)}, @{nameof(PaymentTermEntity.CondicionPago)}, @{nameof(PaymentTermEntity.Descripcion)}, @{nameof(PaymentTermEntity.Activo)}, " +
                $" @{ nameof(PaymentTermEntity.Defecto)});" +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.Codigo,
                CondicionPago = obj.CondicionPago,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto
            }, sqlTran)).First();

            return PaymentTermEntity.UpdateId(obj, newId);
        }
        public override async Task<PaymentTermEntity> UpdateSingle(PaymentTermEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(PaymentTermEntity.Codigo)} = @{nameof(PaymentTermEntity.Codigo)}, " +
                $" {nameof(PaymentTermEntity.CondicionPago)} =   @{nameof(PaymentTermEntity.CondicionPago)}, " +
                $" {nameof(PaymentTermEntity.Descripcion)} =  @{nameof(PaymentTermEntity.Descripcion)}, " +
                $" {nameof(PaymentTermEntity.Activo)} = @{nameof(PaymentTermEntity.Activo)}, " +
                $" {nameof(PaymentTermEntity.Defecto)} =  @{ nameof(PaymentTermEntity.Defecto)} " +
                $" where {nameof(PaymentTermEntity.CondicionPagoID)} = @{nameof(PaymentTermEntity.CondicionPagoID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                CondicionPago = obj.CondicionPago,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                CondicionPagoID = obj.CondicionPagoID
            }, sqlTran));
            return obj;
        }

        public override async Task<PaymentTermEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string query = $"select * from {Table.Name} where {Table.Code} = @code" +
                $" AND {nameof(PaymentTermEntity.ClienteID)} = @{nameof(PaymentTermEntity.ClienteID)}";

            return await connection.QueryFirstOrDefaultAsync<PaymentTermEntity>(query, new
            {
                code = code.ToString(),
                ClienteID = client
            }, sqlTran);

        }

        public override async Task<int> Delete(PaymentTermEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key" +
                $" AND {nameof(PaymentTermEntity.ClienteID)} = @{nameof(PaymentTermEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CondicionPagoID,
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
