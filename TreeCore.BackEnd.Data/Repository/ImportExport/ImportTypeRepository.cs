using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.ImportExport;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.ImportExport
{
    public class ImportTypeRepository : BaseRepository<ImportTypeEntity>
    {
        public override Table Table => TableNames.ImportType;

        public ImportTypeRepository(TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<ImportTypeEntity> InsertSingle(ImportTypeEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(ImportTypeEntity.DocumentoCargaPlantilla)}, {nameof(ImportTypeEntity.RutaDocumento)},{nameof(ImportTypeEntity.FechaSubida)},{nameof(ImportTypeEntity.ProyectoTipoID)}," +
                $"{nameof(ImportTypeEntity.Activo)}" +
                $")" +
                $"values (" +
                $"@{nameof(ImportTypeEntity.DocumentoCargaPlantilla)}, @{nameof(ImportTypeEntity.RutaDocumento)}, @{nameof(ImportTypeEntity.FechaSubida)}, @{nameof(ImportTypeEntity.ProyectoTipoID)}," +
                $"@{nameof(ImportTypeEntity.Activo)}" +
                $");" +
                 $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                DocumentoCargaPlantilla = obj.DocumentoCargaPlantilla,
                RutaDocumento = obj.RutaDocumento,
                FechaSubida = obj.FechaSubida,
                ProyectoTipoID = obj.ProyectoTipoID,
                Activo = obj.Activo
            }, sqlTran)).First();

            return ImportTypeEntity.UpdateId(obj, newId);
        }

        public override async Task<ImportTypeEntity> UpdateSingle(ImportTypeEntity obj)
        {
            string sql = $"update {Table.Name} set" +
                $" {nameof(ImportTypeEntity.DocumentoCargaPlantilla)} = @{nameof(ImportTypeEntity.DocumentoCargaPlantilla)}, " +
                $" {nameof(ImportTypeEntity.RutaDocumento)} = @{nameof(ImportTypeEntity.RutaDocumento)}, " +
                $" {nameof(ImportTypeEntity.FechaSubida)} = @{nameof(ImportTypeEntity.FechaSubida)}, " +
                $" {nameof(ImportTypeEntity.ProyectoTipoID)} = @{nameof(ImportTypeEntity.ProyectoTipoID)}, " +
                $" {nameof(ImportTypeEntity.Activo)} = @{nameof(ImportTypeEntity.Activo)} " +
                $" where {nameof(ImportTypeEntity.DocumentoCargaPlantillaID)} = @{nameof(ImportTypeEntity.DocumentoCargaPlantillaID)}  ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                DocumentoCargaPlantillaID = obj.DocumentoCargaPlantillaID,
                DocumentoCargaPlantilla = obj.DocumentoCargaPlantilla,
                RutaDocumento = obj.RutaDocumento,
                FechaSubida = obj.FechaSubida,
                ProyectoTipoID = obj.ProyectoTipoID,
                Activo = obj.Activo
            }, sqlTran));

            return obj;
        }

        public override async Task<ImportTypeEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<ImportTypeEntity>($"select * from {Table.Name} where {Table.Code} = @code", new
            {
                code = code
            }, sqlTran);

        }

        public override async Task<int> Delete(ImportTypeEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.DocumentoCargaPlantillaID
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
