using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.Contracts
{

    public class ContractGroupRepository : BaseRepository<ContractGroupEntity>
    {
        public override Table Table => TableNames.ContractGroup;

        public ContractGroupRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<ContractGroupEntity> InsertSingle(ContractGroupEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(ContractGroupEntity.ClienteID)},{nameof(ContractGroupEntity.codigo)}, {nameof(ContractGroupEntity.TipoContratacion)}, {nameof(ContractGroupEntity.Descripcion)}, {nameof(ContractGroupEntity.Activo)}, " +
                $"{ nameof(ContractGroupEntity.Defecto)})" +
                $"values (@{nameof(ContractGroupEntity.ClienteID)},@{nameof(ContractGroupEntity.codigo)}, @{nameof(ContractGroupEntity.TipoContratacion)}, @{nameof(ContractGroupEntity.Descripcion)}, @{nameof(ContractGroupEntity.Activo)}, " +
                $" @{ nameof(ContractGroupEntity.Defecto)});" +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.codigo,
                TipoContratacion = obj.TipoContratacion,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto
            }, sqlTran)).First();

            return ContractGroupEntity.UpdateId(obj, newId);
        }
        public override async Task<ContractGroupEntity> UpdateSingle(ContractGroupEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(ContractGroupEntity.codigo)} = @{nameof(ContractGroupEntity.codigo)}, " +
                $" {nameof(ContractGroupEntity.TipoContratacion)} =   @{nameof(ContractGroupEntity.TipoContratacion)}, " +
                $" {nameof(ContractGroupEntity.Descripcion)} =  @{nameof(ContractGroupEntity.Descripcion)}, " +
                $" {nameof(ContractGroupEntity.Activo)} = @{nameof(ContractGroupEntity.Activo)}, " +
                $" {nameof(ContractGroupEntity.Defecto)} =  @{ nameof(ContractGroupEntity.Defecto)} " +
                $" where {nameof(ContractGroupEntity.AlquilerTipoContratacionID)} = @{nameof(ContractGroupEntity.AlquilerTipoContratacionID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.codigo,
                TipoContratacion = obj.TipoContratacion,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                AlquilerTipoContratacionID = obj.AlquilerTipoContratacionID
            }, sqlTran));
            return obj;
        }

        public override async Task<ContractGroupEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<ContractGroupEntity>($"select * from {Table.Name} where Codigo = @code AND {nameof(ContractGroupEntity.ClienteID)} = @{nameof(ContractGroupEntity.ClienteID)}", new
            {
                code = code,
                ClienteID = Client
            }, sqlTran);

        }

        public override async Task<int> Delete(ContractGroupEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key AND {nameof(ContractGroupEntity.ClienteID)} = @{nameof(ContractGroupEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.AlquilerTipoContratacionID,
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
