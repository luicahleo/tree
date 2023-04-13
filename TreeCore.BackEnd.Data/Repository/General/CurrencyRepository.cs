using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.General
{
    public class CurrencyRepository : BaseRepository<CurrencyEntity>
    {
        public override Table Table => TableNames.Currency;

        public CurrencyRepository(TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<CurrencyEntity> InsertSingle(CurrencyEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(CurrencyEntity.Moneda)}, {nameof(CurrencyEntity.Simbolo)}, {nameof(CurrencyEntity.CambioDollarUS)}, {nameof(CurrencyEntity.CambioEuro)}, " +
                $"{nameof(CurrencyEntity.FechaActualizacion)}, { nameof(CurrencyEntity.ClienteID)}, { nameof(CurrencyEntity.Activo)}, {nameof(CurrencyEntity.Defecto)})" +
                $"values (@{nameof(CurrencyEntity.Moneda)}, @{nameof(CurrencyEntity.Simbolo)}, @{nameof(CurrencyEntity.CambioDollarUS)}, @{nameof(CurrencyEntity.CambioEuro)}, " +
                $" @{nameof(CurrencyEntity.FechaActualizacion)}, @{nameof(CurrencyEntity.ClienteID)}, @{nameof(CurrencyEntity.Activo)}, @{nameof(CurrencyEntity.Defecto)});" +
                 $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Moneda = obj.Moneda,
                Simbolo = obj.Simbolo,
                CambioDollarUS = obj.CambioDollarUS,
                CambioEuro = obj.CambioEuro,
                FechaActualizacion = obj.FechaActualizacion,
                Activo = obj.Activo,
                ClienteID = obj.ClienteID,
                Defecto = obj.Defecto
            }, sqlTran)).First();

            return CurrencyEntity.UpdateId(obj, newId);
        }

        public override async Task<CurrencyEntity> UpdateSingle(CurrencyEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(CurrencyEntity.Moneda)} = @{nameof(CurrencyEntity.Moneda)}, " +
                $" {nameof(CurrencyEntity.Simbolo)} =   @{nameof(CurrencyEntity.Simbolo)}, " +
                $" {nameof(CurrencyEntity.CambioDollarUS)} =  @{nameof(CurrencyEntity.CambioDollarUS)}, " +
                $" {nameof(CurrencyEntity.CambioEuro)} =  @{nameof(CurrencyEntity.CambioEuro)}, " +
                $" {nameof(CurrencyEntity.FechaActualizacion)} =  @{nameof(CurrencyEntity.FechaActualizacion)}, " +
                $" {nameof(CurrencyEntity.Activo)} = @{nameof(CurrencyEntity.Activo)}, " +
                $" {nameof(CurrencyEntity.Defecto)} =  @{ nameof(CurrencyEntity.Defecto)} " +
                $" where {nameof(CurrencyEntity.MonedaID)} = @{nameof(CurrencyEntity.MonedaID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                MonedaID = obj.MonedaID,
                Moneda = obj.Moneda,
                Simbolo = obj.Simbolo,
                CambioDollarUS = obj.CambioDollarUS,
                CambioEuro = obj.CambioEuro,
                FechaActualizacion = obj.FechaActualizacion,
                Activo = obj.Activo,
                Defecto = obj.Defecto
            }, sqlTran));

            return obj;
        }

        public override async Task<CurrencyEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<CurrencyEntity>($"select * from {Table.Name} where {Table.Code} = @code", new
            {
                code = code
            }, sqlTran);

        }

        public override async Task<int> Delete(CurrencyEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.MonedaID
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
