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
    public class ProjectLifeCycleStatusRepository : BaseRepository<ProjectLifeCycleStatusEntity>
    {
        public override Table Table => TableNames.ProjectLifeCycleStatus;

        public ProjectLifeCycleStatusRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }
        //TODO::
        public override async Task<ProjectLifeCycleStatusEntity> InsertSingle(ProjectLifeCycleStatusEntity obj)
        {
            string sql = $"insert into {Table.Name} " +
                $"({nameof(ProjectLifeCycleStatusEntity.Codigo)}, " +
                $"{nameof(ProjectLifeCycleStatusEntity.Nombre)}, " +
                $"{nameof(ProjectLifeCycleStatusEntity.Descripcion)}, " +
                $"{nameof(ProjectLifeCycleStatusEntity.Activo)}, " +
                $"{nameof(ProjectLifeCycleStatusEntity.Color)}, " +
                $"{nameof(ProjectLifeCycleStatusEntity.ClienteID)}) " +
                $"values (@{nameof(ProjectLifeCycleStatusEntity.Codigo)}, " +
                $"@{nameof(ProjectLifeCycleStatusEntity.Nombre)}, " +
                $"@{nameof(ProjectLifeCycleStatusEntity.Descripcion)}, " +
                $"@{nameof(ProjectLifeCycleStatusEntity.Activo)}, " +
                $"@{nameof(ProjectLifeCycleStatusEntity.Color)}, " +
                $"@{nameof(ProjectLifeCycleStatusEntity.ClienteID)});" +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Color = obj.Color,
                ClienteID = obj.ClienteID,
            }, sqlTran)).First();

            return ProjectLifeCycleStatusEntity.UpdateId(obj, newId);
        }
        public override async Task<ProjectLifeCycleStatusEntity> UpdateSingle(ProjectLifeCycleStatusEntity obj)
        {
            string sql = $"update {Table.Name} " + $"set {nameof(ProjectLifeCycleStatusEntity.Codigo)} = @{nameof(ProjectLifeCycleStatusEntity.Codigo)}, " +
                $" {nameof(ProjectLifeCycleStatusEntity.Nombre)} =   @{nameof(ProjectLifeCycleStatusEntity.Nombre)}, " +
                $" {nameof(ProjectLifeCycleStatusEntity.Descripcion)} =  @{nameof(ProjectLifeCycleStatusEntity.Descripcion)}, " +
                $" {nameof(ProjectLifeCycleStatusEntity.Activo)} = @{nameof(ProjectLifeCycleStatusEntity.Activo)}, " +
                $" {nameof(ProjectLifeCycleStatusEntity.Color)} =  @{nameof(ProjectLifeCycleStatusEntity.Color)}, " +
                $" {nameof(ProjectLifeCycleStatusEntity.ClienteID)} =  @{ nameof(ProjectLifeCycleStatusEntity.ClienteID)} " +
                $" where {nameof(ProjectLifeCycleStatusEntity.CoreProjectLifeCycleStatusID)} = @{nameof(ProjectLifeCycleStatusEntity.CoreProjectLifeCycleStatusID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Color = obj.Color,
                ClienteID = obj.ClienteID,
                CoreProjectLifeCycleStatusID = obj.CoreProjectLifeCycleStatusID
            }, sqlTran));
            return obj;
        }

        public override async Task<ProjectLifeCycleStatusEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<ProjectLifeCycleStatusEntity>($"select * from {Table.Name} where Codigo = @code" +
                $" AND {nameof(ProjectLifeCycleStatusEntity.ClienteID)} = @{nameof(ProjectLifeCycleStatusEntity.ClienteID)}", new
                {
                    code = code,
                    ClienteID = client
                }, sqlTran);

        }

        public override async Task<int> Delete(ProjectLifeCycleStatusEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key" +
                $" AND {nameof(ProjectLifeCycleStatusEntity.ClienteID)} = @{nameof(ProjectLifeCycleStatusEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreProjectLifeCycleStatusID,
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
