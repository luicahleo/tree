using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.Contracts
{

    public class ContractStateRepository : BaseRepository<ContractStatusEntity>
    {
        public override Table Table => TableNames.ContractState;

        public ContractStateRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<ContractStatusEntity> InsertSingle(ContractStatusEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(ContractStatusEntity.ClienteID)},{nameof(ContractStatusEntity.codigo)}, " +
                $"{nameof(ContractStatusEntity.Estado)}, {nameof(ContractStatusEntity.Descripcion)}, " +
                $"{nameof(ContractStatusEntity.Activo)},{nameof(ContractStatusEntity.UsoPagar)}, " +
                $"{ nameof(ContractStatusEntity.Defecto)})" +
                $"values (@{nameof(ContractStatusEntity.ClienteID)},@{nameof(ContractStatusEntity.codigo)}," +
                $" @{nameof(ContractStatusEntity.Estado)}, @{nameof(ContractStatusEntity.Descripcion)}," +
                $" @{nameof(ContractStatusEntity.Activo)}, 0, " +
                $" @{ nameof(ContractStatusEntity.Defecto)});" +
                $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.codigo,
                Estado = obj.Estado,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto
            }, sqlTran)).First();

            return ContractStatusEntity.UpdateId(obj, newId);
        }
        public override async Task<ContractStatusEntity> UpdateSingle(ContractStatusEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(ContractStatusEntity.codigo)} = @{nameof(ContractStatusEntity.codigo)}, " +
                $" {nameof(ContractStatusEntity.Estado)} =   @{nameof(ContractStatusEntity.Estado)}, " +
                $" {nameof(ContractStatusEntity.Descripcion)} =  @{nameof(ContractStatusEntity.Descripcion)}, " +
                $" {nameof(ContractStatusEntity.Activo)} = @{nameof(ContractStatusEntity.Activo)}, " +
                $" {nameof(ContractStatusEntity.Defecto)} =  @{ nameof(ContractStatusEntity.Defecto)}, " +
                 $" {nameof(ContractStatusEntity.UsoPagar)} = 0 " +
                $" where {nameof(ContractStatusEntity.AlquilerEstadoID)} = @{nameof(ContractStatusEntity.AlquilerEstadoID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.codigo,
                Estado = obj.Estado,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                AlquilerEstadoID = obj.AlquilerEstadoID
            }, sqlTran));
            return obj;
        }

        public override async Task<ContractStatusEntity> GetItemByCode(string code, int Client)
        {

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<ContractStatusEntity>($"select * from {Table.Name} where Codigo = @code AND {nameof(ContractStatusEntity.ClienteID)} = @{nameof(ContractStatusEntity.ClienteID)}", new
            {
                code = code,
                ClienteID = Client
            }, sqlTran);
        }

        public override async Task<int> Delete(ContractStatusEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key AND {nameof(ContractStatusEntity.ClienteID)} = @{nameof(ContractStatusEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.AlquilerEstadoID,
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
