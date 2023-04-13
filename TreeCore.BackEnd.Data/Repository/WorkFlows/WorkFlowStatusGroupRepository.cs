using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.WorkFlows
{
    public class WorkFlowStatusGroupRepository : BaseRepository<WorkFlowStatusGroupEntity>
    {
        public override Table Table => TableNames.WorkFlowStatusGroup;

        public WorkFlowStatusGroupRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<WorkFlowStatusGroupEntity> InsertSingle(WorkFlowStatusGroupEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(WorkFlowStatusGroupEntity.ClienteID)}, " +
                $"{nameof(WorkFlowStatusGroupEntity.Codigo)}, " +
                $"{nameof(WorkFlowStatusGroupEntity.Nombre)}, " +
                $"{nameof(WorkFlowStatusGroupEntity.Descripcion)}, " +
                $"{nameof(WorkFlowStatusGroupEntity.Activo)}, " +
                $"{nameof(WorkFlowStatusGroupEntity.Defecto)}) " +
                $"values (" +
                $"@{nameof(WorkFlowStatusGroupEntity.ClienteID)}," +
                $"@{nameof(WorkFlowStatusGroupEntity.Codigo)}, " +
                $"@{nameof(WorkFlowStatusGroupEntity.Nombre)}, " +
                $"@{nameof(WorkFlowStatusGroupEntity.Descripcion)}, " +
                $"@{nameof(WorkFlowStatusGroupEntity.Activo)}, " +
                $"@{nameof(WorkFlowStatusGroupEntity.Defecto)}); " +
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

            return WorkFlowStatusGroupEntity.UpdateId(obj, newId);
        }
        public override async Task<WorkFlowStatusGroupEntity> UpdateSingle(WorkFlowStatusGroupEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(WorkFlowStatusGroupEntity.Codigo)} = @{nameof(WorkFlowStatusGroupEntity.Codigo)}, " +
                $" {nameof(WorkFlowStatusGroupEntity.Nombre)} =   @{nameof(WorkFlowStatusGroupEntity.Nombre)}, " +
                $" {nameof(WorkFlowStatusGroupEntity.Descripcion)} =  @{nameof(WorkFlowStatusGroupEntity.Descripcion)}, " +
                $" {nameof(WorkFlowStatusGroupEntity.Activo)} = @{nameof(WorkFlowStatusGroupEntity.Activo)}, " +
                $" {nameof(WorkFlowStatusGroupEntity.Defecto)} =  @{ nameof(WorkFlowStatusGroupEntity.Defecto)} " +
                $" where {nameof(WorkFlowStatusGroupEntity.EstadoAgrupacionID)} = @{nameof(WorkFlowStatusGroupEntity.EstadoAgrupacionID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                EstadoAgrupacionID = obj.EstadoAgrupacionID
            }, sqlTran));
            return obj;
        }

        public override async Task<WorkFlowStatusGroupEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<WorkFlowStatusGroupEntity>($"select * from {Table.Name} where Codigo = @code" +
                $" AND {nameof(WorkFlowStatusGroupEntity.ClienteID)} = @{nameof(WorkFlowStatusGroupEntity.ClienteID)}", new
                {
                    code = code,
                    ClienteID = client
                }, sqlTran);

        }

        public override async Task<int> Delete(WorkFlowStatusGroupEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key" +
                $" AND {nameof(WorkFlowStatusGroupEntity.ClienteID)} = @{nameof(WorkFlowStatusGroupEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.EstadoAgrupacionID,
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
