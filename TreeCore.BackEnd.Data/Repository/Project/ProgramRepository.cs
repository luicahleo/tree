using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.Project
{
    public class ProgramRepository : BaseRepository<ProgramEntity>
    {
        public override Table Table => TableNames.Program;

        public ProgramRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<ProgramEntity> InsertSingle(ProgramEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(ProgramEntity.Codigo)}, " +
                $"{nameof(ProgramEntity.Nombre)}, " +
                $"{nameof(ProgramEntity.Descripcion)}, " +
                $"{nameof(ProgramEntity.Activo)}, " +
                $"{nameof(ProgramEntity.ClienteID)}) " +
                $"values (" +
                $"@{nameof(ProgramEntity.Codigo)}, " +
                $"@{nameof(ProgramEntity.Nombre)}, " +
                $"@{nameof(ProgramEntity.Descripcion)}, " +
                $"@{nameof(ProgramEntity.Activo)}, " +
                $"@{nameof(ProgramEntity.ClienteID)});" +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                ClienteID = obj.ClienteID,
            }, sqlTran)).First();

            return ProgramEntity.UpdateId(obj, newId);
        }
        public override async Task<ProgramEntity> UpdateSingle(ProgramEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(ProgramEntity.Codigo)} = @{nameof(ProgramEntity.Codigo)}, " +
                $" {nameof(ProgramEntity.Nombre)} =   @{nameof(ProgramEntity.Nombre)}, " +
                $" {nameof(ProgramEntity.Descripcion)} =  @{nameof(ProgramEntity.Descripcion)}, " +
                $" {nameof(ProgramEntity.Activo)} = @{nameof(ProgramEntity.Activo)} " +
                $" where {nameof(ProgramEntity.CoreProgramID)} = @{nameof(ProgramEntity.CoreProgramID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                CoreProgramID = obj.CoreProgramID
            }, sqlTran));
            return obj;
        }

        public override async Task<ProgramEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var result = await connection.QueryFirstOrDefaultAsync<ProgramEntity>($"select * from {Table.Name} where Codigo = @code" +
                $" AND {nameof(ProgramEntity.ClienteID)} = @{nameof(ProgramEntity.ClienteID)}", new
                {
                    code = code,
                    ClienteID = client
                }, sqlTran);
            return result;

        }

        public override async Task<int> Delete(ProgramEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"delete from {Table.Name} Where {Table.ID} = @key" +
                $" AND {nameof(ProgramEntity.ClienteID)} = @{nameof(ProgramEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreProgramID,
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
