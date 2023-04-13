using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.Contracts
{
    public class ContractTypeRepository : BaseRepository<ContractTypeEntity>
    {
        public override Table Table => TableNames.ContractType;

        public ContractTypeRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<ContractTypeEntity> InsertSingle(ContractTypeEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(ContractTypeEntity.ClienteID)},{nameof(ContractTypeEntity.Codigo)}, {nameof(ContractTypeEntity.TipoContrato)}, {nameof(ContractTypeEntity.Descripcion)}, {nameof(ContractTypeEntity.Activo)}, " +
                $"{ nameof(ContractTypeEntity.Defecto)})" +
                $"values (@{nameof(ContractTypeEntity.ClienteID)},@{nameof(ContractTypeEntity.Codigo)}, @{nameof(ContractTypeEntity.TipoContrato)}, @{nameof(ContractTypeEntity.Descripcion)}, @{nameof(ContractTypeEntity.Activo)}, " +
                $" @{ nameof(ContractTypeEntity.Defecto)});" +
                $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.Codigo,
                TipoContrato = obj.TipoContrato,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto
            }, sqlTran)).First();

            return ContractTypeEntity.UpdateId(obj, newId);
        }
        public override async Task<ContractTypeEntity> UpdateSingle(ContractTypeEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(ContractTypeEntity.Codigo)} = @{nameof(ContractTypeEntity.Codigo)}, " +
                $" {nameof(ContractTypeEntity.TipoContrato)} =   @{nameof(ContractTypeEntity.TipoContrato)}, " +
                $" {nameof(ContractTypeEntity.Descripcion)} =  @{nameof(ContractTypeEntity.Descripcion)}, " +
                $" {nameof(ContractTypeEntity.Activo)} = @{nameof(ContractTypeEntity.Activo)}, " +
                $" {nameof(ContractTypeEntity.Defecto)} =  @{ nameof(ContractTypeEntity.Defecto)} " +
                $" where {nameof(ContractTypeEntity.AlquilerTipoContratoID)} = @{nameof(ContractTypeEntity.AlquilerTipoContratoID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                TipoContrato = obj.TipoContrato,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                AlquilerTipoContratoID = obj.AlquilerTipoContratoID
            }, sqlTran));
            return obj;
        }

        public override async Task<ContractTypeEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<ContractTypeEntity>($"select * from {Table.Name} where Codigo = @code AND {nameof(ContractTypeEntity.ClienteID)} = @{nameof(ContractTypeEntity.ClienteID)}", new
            {
                code = code,
                ClienteID = Client
            }, sqlTran);

        }

        public override async Task<int> Delete(ContractTypeEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key AND {nameof(ContractTypeEntity.ClienteID)} = @{nameof(ContractTypeEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.AlquilerTipoContratoID,
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
