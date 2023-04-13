using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.Companies
{
    public class CompanyTypeRepository : BaseRepository<CompanyTypeEntity>
    {
        public override Table Table => TableNames.CompanyType;

        public CompanyTypeRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<CompanyTypeEntity> InsertSingle(CompanyTypeEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(CompanyTypeEntity.ClienteID)},{nameof(CompanyTypeEntity.Codigo)}, {nameof(CompanyTypeEntity.EntidadTipo)}, {nameof(CompanyTypeEntity.Descripcion)}, {nameof(CompanyTypeEntity.Activo)}, " +
                $"{ nameof(CompanyTypeEntity.Defecto)})" +
                $"values (@{nameof(CompanyTypeEntity.ClienteID)},@{nameof(CompanyTypeEntity.Codigo)}, @{nameof(CompanyTypeEntity.EntidadTipo)}, @{nameof(CompanyTypeEntity.Descripcion)}, @{nameof(CompanyTypeEntity.Activo)}, " +
                $" @{ nameof(CompanyTypeEntity.Defecto)});" +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.Codigo,
                EntidadTipo = obj.EntidadTipo,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto
            }, sqlTran)).First();

            return CompanyTypeEntity.UpdateId(obj, newId);
        }
        public override async Task<CompanyTypeEntity> UpdateSingle(CompanyTypeEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(CompanyTypeEntity.Codigo)} = @{nameof(CompanyTypeEntity.Codigo)}, " +
                $" {nameof(CompanyTypeEntity.EntidadTipo)} = @{nameof(CompanyTypeEntity.EntidadTipo)}, " +
                $" {nameof(CompanyTypeEntity.Descripcion)} = @{nameof(CompanyTypeEntity.Descripcion)}, " +
                $" {nameof(CompanyTypeEntity.Activo)} = @{nameof(CompanyTypeEntity.Activo)}, " +
                $" {nameof(CompanyTypeEntity.Defecto)} =  @{ nameof(CompanyTypeEntity.Defecto)}, " +
                $" {nameof(CompanyTypeEntity.ClienteID)} =  @{ nameof(CompanyTypeEntity.ClienteID)} " +
                $" where {nameof(CompanyTypeEntity.EntidadTipoID)} = @{nameof(CompanyTypeEntity.EntidadTipoID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                EntidadTipo = obj.EntidadTipo,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                EntidadTipoID = obj.EntidadTipoID,
                ClienteID = obj.ClienteID
            }, sqlTran));
            return obj;
        }

        public override async Task<CompanyTypeEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<CompanyTypeEntity>($"select * from {Table.Name} where Codigo = @code AND {nameof(CompanyTypeEntity.ClienteID)} = @{nameof(CompanyTypeEntity.ClienteID)}", new
            {
                code = code,
                ClienteID = Client
            }, sqlTran);

        }
        public override async Task<int> Delete(CompanyTypeEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key AND {nameof(CompanyTypeEntity.ClienteID)} = @{nameof(CompanyTypeEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.EntidadTipoID,
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
