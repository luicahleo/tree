using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.General
{
    public class PaymentMethodsRepository : BaseRepository<PaymentMethodsEntity>
    {
        public override Table Table => TableNames.PaymentMethods;

        public PaymentMethodsRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<PaymentMethodsEntity> InsertSingle(PaymentMethodsEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(PaymentMethodsEntity.ClienteID)},{nameof(PaymentMethodsEntity.CodigoMetodoPago)}, {nameof(PaymentMethodsEntity.MetodoPago)}, {nameof(PaymentMethodsEntity.Descripcion)}, {nameof(PaymentMethodsEntity.Activo)}, {nameof(PaymentMethodsEntity.RequiereBanco)}, " +
                $"{ nameof(PaymentMethodsEntity.Defecto)})" +
                $"values (@{nameof(PaymentMethodsEntity.ClienteID)},@{nameof(PaymentMethodsEntity.CodigoMetodoPago)}, @{nameof(PaymentMethodsEntity.MetodoPago)}, @{nameof(PaymentMethodsEntity.Descripcion)}, @{nameof(PaymentMethodsEntity.Activo)}, @{nameof(PaymentMethodsEntity.RequiereBanco)}, " +
                $" @{ nameof(PaymentMethodsEntity.Defecto)});" +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                CodigoMetodoPago = obj.CodigoMetodoPago,
                MetodoPago = obj.MetodoPago,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                RequiereBanco = obj.RequiereBanco,
                Defecto = obj.Defecto
            }, sqlTran)).First();

            return PaymentMethodsEntity.UpdateId(obj, newId);
        }
        public override async Task<PaymentMethodsEntity> UpdateSingle(PaymentMethodsEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(PaymentMethodsEntity.CodigoMetodoPago)} = @{nameof(PaymentMethodsEntity.CodigoMetodoPago)}, " +
                $" {nameof(PaymentMethodsEntity.MetodoPago)} =   @{nameof(PaymentMethodsEntity.MetodoPago)}, " +
                $" {nameof(PaymentMethodsEntity.Descripcion)} =  @{nameof(PaymentMethodsEntity.Descripcion)}, " +
                $" {nameof(PaymentMethodsEntity.Activo)} = @{nameof(PaymentMethodsEntity.Activo)}, " +
                $" {nameof(PaymentMethodsEntity.RequiereBanco)} = @{nameof(PaymentMethodsEntity.RequiereBanco)}, " +
                $" {nameof(PaymentMethodsEntity.Defecto)} =  @{ nameof(PaymentMethodsEntity.Defecto)} " +
                $" where {nameof(PaymentMethodsEntity.MetodoPagoID)} = @{nameof(PaymentMethodsEntity.MetodoPagoID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                CodigoMetodoPago = obj.CodigoMetodoPago,
                MetodoPago = obj.MetodoPago,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                RequiereBanco = obj.RequiereBanco,
                MetodoPagoID = obj.MetodoPagoID
            }, sqlTran));
            return obj;
        }

        public async Task<IEnumerable<PaymentMethodsEntity>> GetListbyCompany(int EntidadID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string query = $"select * from {Table.Name} where {Table.ID} = @entidadID";

            var result = await connection.QueryAsync<PaymentMethodsEntity>(query, new { entidadID = EntidadID }, sqlTran);

            return result.ToList();
        }

        public override async Task<PaymentMethodsEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<PaymentMethodsEntity>($"select * from {Table.Name} where {Table.Code} = @code" +
                $" AND {nameof(PaymentMethodsEntity.ClienteID)} = @{nameof(PaymentMethodsEntity.ClienteID)}", new
            {
                code = code,
                ClienteID = client
            }, sqlTran);

        }

        public override async Task<int> Delete(PaymentMethodsEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key " +
                $" AND {nameof(PaymentMethodsEntity.ClienteID)} = @{nameof(PaymentMethodsEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.MetodoPagoID,
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
