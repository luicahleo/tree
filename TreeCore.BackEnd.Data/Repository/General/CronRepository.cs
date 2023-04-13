using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;
using System.Collections.Generic;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.General
{
    public class CronRepository : BaseRepository<CronEntity>
    {
        public override Table Table => TableNames.Cron;

        public CronRepository(TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<CronEntity> InsertSingle(CronEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(CronEntity.Nombre)}," +
                $"{nameof(CronEntity.CronFormat)}," +
                $"{nameof(CronEntity.Activo)}," +
                $"{nameof(CronEntity.FechaInicio)}," +
                $"{nameof(CronEntity.FechaFin)}," +
                $"{nameof(CronEntity.TipoFrecuencia)}" +
                $")" +
                $"values (" +
                $"@{nameof(CronEntity.Nombre)}," +
                $"@{nameof(CronEntity.CronFormat)}," +
                $"@{nameof(CronEntity.Activo)}," +
                $"@{nameof(CronEntity.FechaInicio)}," +
                $"@{nameof(CronEntity.FechaFin)}," +
                $"@{nameof(CronEntity.TipoFrecuencia)}" +
                $");" +
                $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Nombre = obj.Nombre,
                CronFormat = obj.CronFormat,
                Activo = obj.Activo,
                FechaInicio = obj.FechaInicio,
                FechaFin = obj.FechaFin,
                TipoFrecuencia = obj.TipoFrecuencia
            }, sqlTran)).First();

            return CronEntity.UpdateId(obj, newId);
        }

        public override async Task<CronEntity> UpdateSingle(CronEntity obj)
        {
            string sql = $"update {Table.Name} set " +
                $"{nameof(CronEntity.Nombre)} = @{nameof(CronEntity.Nombre)}," +
                $"{nameof(CronEntity.CronFormat)} = @{nameof(CronEntity.CronFormat)}," +
                $"{nameof(CronEntity.Activo)} = @{nameof(CronEntity.Activo)}," +
                $"{nameof(CronEntity.FechaInicio)} = @{nameof(CronEntity.FechaInicio)}," +
                $"{nameof(CronEntity.FechaFin)} = @{nameof(CronEntity.FechaFin)}," +
                $"{nameof(CronEntity.TipoFrecuencia)} = @{nameof(CronEntity.TipoFrecuencia)}" +
                $" where {nameof(CronEntity.CoreServicioFrecuenciaID)} = @{nameof(CronEntity.CoreServicioFrecuenciaID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                CoreServicioFrecuenciaID = obj.CoreServicioFrecuenciaID,
                CronFormat = obj.CronFormat,
                Nombre = obj.Nombre,
                Activo = obj.Activo,
                FechaInicio = obj.FechaInicio,
                FechaFin = obj.FechaFin,
                TipoFrecuencia = obj.TipoFrecuencia
            }, sqlTran));

            return obj;
        }

        public override async Task<CronEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} where p.Codigo = @code";

            return await connection.QueryFirstOrDefaultAsync<CronEntity>($"select * from {Table.Name} where {Table.Code} = @code", new
            {
                code = code
            }, sqlTran);
        }

        public override async Task<int> Delete(CronEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreServicioFrecuenciaID
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
