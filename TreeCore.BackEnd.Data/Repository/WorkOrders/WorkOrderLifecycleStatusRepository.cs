using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.WorkOrders
{
    public class WorkOrderLifecycleStatusRepository : BaseRepository<WorkOrderLifecycleStatusEntity>
    {
        public override Table Table => TableNames.WorkOrderLifecycleStatus;

        public WorkOrderLifecycleStatusRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<WorkOrderLifecycleStatusEntity> InsertSingle(WorkOrderLifecycleStatusEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(WorkOrderLifecycleStatusEntity.ClienteID)}, " +
                $"{nameof(WorkOrderLifecycleStatusEntity.Codigo)}, " +
                $"{nameof(WorkOrderLifecycleStatusEntity.Nombre)}, " +
                $"{nameof(WorkOrderLifecycleStatusEntity.Descripcion)}, " +
                $"{nameof(WorkOrderLifecycleStatusEntity.Color)}, " +
                $"{nameof(WorkOrderLifecycleStatusEntity.Activo)}, " +
                $"{nameof(WorkOrderLifecycleStatusEntity.Defecto)}) " +
                $"values (" +
                $"@{nameof(WorkOrderLifecycleStatusEntity.ClienteID)}," +
                $"@{nameof(WorkOrderLifecycleStatusEntity.Codigo)}, " +
                $"@{nameof(WorkOrderLifecycleStatusEntity.Nombre)}, " +
                $"@{nameof(WorkOrderLifecycleStatusEntity.Descripcion)}, " +
                $"@{nameof(WorkOrderLifecycleStatusEntity.Color)}, " +
                $"@{nameof(WorkOrderLifecycleStatusEntity.Activo)}, " +
                $"@{nameof(WorkOrderLifecycleStatusEntity.Defecto)}); " +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Color = obj.Color,
                Activo = obj.Activo,
                Defecto = obj.Defecto
            }, sqlTran)).First();

            return WorkOrderLifecycleStatusEntity.UpdateId(obj, newId);
        }
        public override async Task<WorkOrderLifecycleStatusEntity> UpdateSingle(WorkOrderLifecycleStatusEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(WorkOrderLifecycleStatusEntity.Codigo)} = @{nameof(WorkOrderLifecycleStatusEntity.Codigo)}, " +
                $" {nameof(WorkOrderLifecycleStatusEntity.Nombre)} =   @{nameof(WorkOrderLifecycleStatusEntity.Nombre)}, " +
                $" {nameof(WorkOrderLifecycleStatusEntity.Descripcion)} =  @{nameof(WorkOrderLifecycleStatusEntity.Descripcion)}, " +
                $" {nameof(WorkOrderLifecycleStatusEntity.Color)} =  @{nameof(WorkOrderLifecycleStatusEntity.Color)}, " +
                $" {nameof(WorkOrderLifecycleStatusEntity.Activo)} = @{nameof(WorkOrderLifecycleStatusEntity.Activo)}, " +
                $" {nameof(WorkOrderLifecycleStatusEntity.Defecto)} =  @{ nameof(WorkOrderLifecycleStatusEntity.Defecto)} " +
                $" where {nameof(WorkOrderLifecycleStatusEntity.CoreWorkOrderEstadoID)} = @{nameof(WorkOrderLifecycleStatusEntity.CoreWorkOrderEstadoID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Color = obj.Color,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                CoreWorkOrderEstadoID = obj.CoreWorkOrderEstadoID
            }, sqlTran));
            return obj;
        }

        public override async Task<WorkOrderLifecycleStatusEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<WorkOrderLifecycleStatusEntity>($"select * from {Table.Name} where Codigo = @code" +
                $" AND {nameof(WorkOrderLifecycleStatusEntity.ClienteID)} = @{nameof(WorkOrderLifecycleStatusEntity.ClienteID)}", new
                {
                    code = code,
                    ClienteID = client
                }, sqlTran);

        }

        public override async Task<int> Delete(WorkOrderLifecycleStatusEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key" +
                $" AND {nameof(WorkOrderLifecycleStatusEntity.ClienteID)} = @{nameof(WorkOrderLifecycleStatusEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreWorkOrderEstadoID,
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
