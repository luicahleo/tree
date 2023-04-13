using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.Contracts
{
    public class FunctionalAreaRepository : BaseRepository<FunctionalAreaEntity>
    {
        public override Table Table => TableNames.FunctionalArea;

        public FunctionalAreaRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<FunctionalAreaEntity> InsertSingle(FunctionalAreaEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(FunctionalAreaEntity.ClienteID)}, {nameof(FunctionalAreaEntity.Codigo)}, " +
                $"{nameof(FunctionalAreaEntity.AreaFuncional)}, {nameof(FunctionalAreaEntity.Descripcion)}, {nameof(FunctionalAreaEntity.Activo)}, " +
                $"{ nameof(FunctionalAreaEntity.Defecto)})" +
                $"values (@{nameof(FunctionalAreaEntity.ClienteID)},@{nameof(FunctionalAreaEntity.Codigo)}, " +
                $"@{nameof(FunctionalAreaEntity.AreaFuncional)}, @{nameof(FunctionalAreaEntity.Descripcion)}, " +
                $"@{nameof(FunctionalAreaEntity.Activo)}, @{ nameof(FunctionalAreaEntity.Defecto)});" +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.Codigo,
                AreaFuncional = obj.AreaFuncional,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto
            }, sqlTran)).First();

            return FunctionalAreaEntity.UpdateId(obj, newId);
        }
        public override async Task<FunctionalAreaEntity> UpdateSingle(FunctionalAreaEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(FunctionalAreaEntity.Codigo)} = @{nameof(FunctionalAreaEntity.Codigo)}, " +
                $" {nameof(FunctionalAreaEntity.AreaFuncional)} =   @{nameof(FunctionalAreaEntity.AreaFuncional)}, " +
                $" {nameof(FunctionalAreaEntity.Descripcion)} =  @{nameof(FunctionalAreaEntity.Descripcion)}, " +
                $" {nameof(FunctionalAreaEntity.Activo)} = @{nameof(FunctionalAreaEntity.Activo)}, " +
                $" {nameof(FunctionalAreaEntity.Defecto)} =  @{ nameof(FunctionalAreaEntity.Defecto)} " +
                $" where {nameof(FunctionalAreaEntity.AreaFuncionalID)} = @{nameof(FunctionalAreaEntity.AreaFuncionalID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                AreaFuncional = obj.AreaFuncional,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                AreaFuncionalID = obj.AreaFuncionalID
            }, sqlTran));
            return obj;
        }

        public override async Task<FunctionalAreaEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<FunctionalAreaEntity>($"select * from {Table.Name} where Codigo = @code AND {nameof(FunctionalAreaEntity.ClienteID)} = @{nameof(FunctionalAreaEntity.ClienteID)}", new
            {
                code = code,
                ClientID = Client
            }, sqlTran);

        }


        public override async Task<int> Delete(FunctionalAreaEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key AND {nameof(FunctionalAreaEntity.ClienteID)} = @{nameof(FunctionalAreaEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.AreaFuncionalID,
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
