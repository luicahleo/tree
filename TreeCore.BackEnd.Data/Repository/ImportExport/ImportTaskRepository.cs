using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.ImportExport;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.ImportExport
{
    public class ImportTaskRepository : BaseRepository<ImportTaskEntity>
    {
        public override Table Table => TableNames.ImportTask;
        public Table TableType => TableNames.ImportType;
        public Table TableCron => TableNames.Cron;

        public ImportTaskRepository(TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<ImportTaskEntity> InsertSingle(ImportTaskEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
               $"{nameof(ImportTaskEntity.DocumentoCarga)}," +
                $"{nameof(ImportTaskEntity.RutaDocumento)}," +
                $"{nameof(ImportTaskEntity.FechaSubida)}," +
                $"{nameof(ImportTaskEntity.FechaEstimadaSubida)}," +
                $"{nameof(ImportTaskEntity.ClienteID)}," +
                $"{nameof(ImportTaskEntity.Procesado)}, " +
                $"{nameof(ImportTaskEntity.Activo)}," +
                $"{nameof(ImportTaskEntity.Exito)}," +
                $"{nameof(ImportTaskEntity.DocumentosCargasPlantillas.DocumentoCargaPlantillaID)}, " +
                $"{nameof(ImportTaskEntity.UsuarioID)}"+
                $")" +
                $"values (" +
                $"@{nameof(ImportTaskEntity.DocumentoCarga)}," +
                $"@{nameof(ImportTaskEntity.RutaDocumento)}," +
                $"@{nameof(ImportTaskEntity.FechaSubida)}," +
                $"@{nameof(ImportTaskEntity.FechaEstimadaSubida)}," +
                $"@{nameof(ImportTaskEntity.ClienteID)}," +
                $"@{nameof(ImportTaskEntity.Procesado)}, " +
                $"@{nameof(ImportTaskEntity.Activo)}," +
                $"@{nameof(ImportTaskEntity.Exito)}," +
                $"@{nameof(ImportTaskEntity.DocumentosCargasPlantillas.DocumentoCargaPlantillaID)}, " +
                $"@{nameof(ImportTaskEntity.UsuarioID)}" +
                $");" +
                 $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                DocumentoCarga = obj.DocumentoCarga,
                DocumentoCargaPlantillaID = obj.DocumentosCargasPlantillas.DocumentoCargaPlantillaID,
                RutaDocumento = obj.RutaDocumento,
                FechaSubida = obj.FechaSubida,
                FechaEstimadaSubida = obj.FechaEstimadaSubida,
                ClienteID = obj.ClienteID,
                Procesado = obj.Procesado,
                Activo = obj.Activo,
                UsuarioID = obj.UsuarioID,
                Exito = obj.Exito
            }, sqlTran)).First();

            return ImportTaskEntity.UpdateId(obj, newId);
        }

        public override async Task<ImportTaskEntity> UpdateSingle(ImportTaskEntity obj)
        {
            string sql = $"update {Table.Name} set" +
                $" {nameof(ImportTaskEntity.DocumentoCarga)} = @{nameof(ImportTaskEntity.DocumentoCarga)}, " +
                $" {nameof(ImportTaskEntity.RutaDocumento)} = @{nameof(ImportTaskEntity.RutaDocumento)}, " +
                $" {nameof(ImportTaskEntity.FechaSubida)} = @{nameof(ImportTaskEntity.FechaSubida)}, " +
                $" {nameof(ImportTaskEntity.FechaEstimadaSubida)} = @{nameof(ImportTaskEntity.FechaEstimadaSubida)}, " +
                $" {nameof(ImportTaskEntity.ClienteID)} = @{nameof(ImportTaskEntity.ClienteID)}, " +
                $" {nameof(ImportTaskEntity.Procesado)} = @{nameof(ImportTaskEntity.Procesado)}, " +
                $" {nameof(ImportTaskEntity.Activo)} = @{nameof(ImportTaskEntity.Activo)}, " +
                $" {nameof(ImportTaskEntity.Exito)} = @{nameof(ImportTaskEntity.Exito)}, " +
                $" {nameof(ImportTaskEntity.UsuarioID)} = @{nameof(ImportTaskEntity.UsuarioID)}, " +
                $" {nameof(ImportTaskEntity.RutaLog)} = @{nameof(ImportTaskEntity.RutaLog)}, " +
                $" {nameof(ImportTaskEntity.DocumentosCargasPlantillas.DocumentoCargaPlantillaID)} = @{nameof(ImportTaskEntity.DocumentosCargasPlantillas.DocumentoCargaPlantillaID)} " +
                $" where {nameof(ImportTaskEntity.DocumentoCargaID)} = @{nameof(ImportTaskEntity.DocumentoCargaID)}  ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                DocumentoCargaID = obj.DocumentoCargaID,
                DocumentoCargaPlantillaID = obj.DocumentosCargasPlantillas.DocumentoCargaPlantillaID,
                DocumentoCarga = obj.DocumentoCarga,
                RutaDocumento = obj.RutaDocumento,
                FechaSubida = obj.FechaSubida,
                FechaEstimadaSubida = obj.FechaEstimadaSubida,
                ClienteID = obj.ClienteID,
                Procesado = obj.Procesado,
                Activo = obj.Activo,
                UsuarioID = obj.UsuarioID,
                Exito = obj.Exito,
                RutaLog = obj.RutaLog
            }, sqlTran));

            return obj;
        }

        public override async Task<ImportTaskEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} p" +
                $" inner join {TableType.Name} c on p.{TableType.ID} = c.{TableType.ID}" +
                $" where p.{Table.Code} = @code AND p.{nameof(ImportTaskEntity.ClienteID)} = @{nameof(ImportTaskEntity.ClienteID)}";

            var result = await connection.QueryAsync<ImportTaskEntity, ImportTypeEntity, ImportTaskEntity>(sql,
                (importTask, importType) =>
                {
                    importTask.DocumentosCargasPlantillas = importType;
                    return importTask;
                }, new { code = code, ClienteID = client }, sqlTran, true, splitOn: $"{TableType.ID}, {TableCron.ID}");


            return result.FirstOrDefault();
        }

        public override async Task<IEnumerable<ImportTaskEntity>> GetList(int client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;


            string sqlRelation = $"p inner join {TableType.Name} c on p.{TableType.ID} = c.{TableType.ID}";

            string sqlFilter = $" WHERE {nameof(ImportTaskEntity.ClienteID)} = {client} ";
            if (filters != null)
            {
                foreach (Filter filter in filters)
                {
                    int iterator = countFilter++;
                    sqlFilter += $" AND p.{filter.Column} {filter.Operator} @{filter.Column}{iterator} ";
                    string nameVar = $"{filter.Column}{iterator}";
                    dicParam.Add(nameVar, filter.Value);
                }
            }

            string sqlOrder = "";
            if (orders != null && orders.Count > 0)
            {
                foreach (string column in orders)
                {
                    sqlOrder += $"{(string.IsNullOrEmpty(sqlOrder) ? " order by p." : ", ")} {column}";
                }
            }
            else
            {
                sqlOrder = $" order by p.{Table.Code} ";
            }

            string sqlPagination = "";
            if (pageSize != -1 && pageIndex != -1)
            {
                sqlPagination = $"OFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            }


            string query = $"select TotalItems = COUNT(*) OVER(), * from {Table.Name} {sqlRelation} {sqlFilter} {sqlOrder} {sqlPagination}";

            var result = await connection.QueryAsync<ImportTaskEntity, ImportTypeEntity, ImportTaskEntity>(query,
                 (importTask, importType) =>
                 {
                     importTask.DocumentosCargasPlantillas = importType;
                     return importTask;
                 }, dicParam, sqlTran, true, splitOn: $"{TableType.ID}, {TableCron.ID}");

            return result.ToList();
        }

        public override async Task<int> Delete(ImportTaskEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key" +
                $" AND {nameof(ImportTaskEntity.ClienteID)} = @{nameof(ImportTaskEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.DocumentoCargaID,
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
