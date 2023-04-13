using Dapper;

using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.BusinessProcess
{
    public class BusinessProcessTypeRepository : BaseRepository<BusinessProcessTypeEntity>
    {
        public override Table Table => TableNames.BusinessProcessType;

        public BusinessProcessTypeRepository(TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<BusinessProcessTypeEntity> InsertSingle(BusinessProcessTypeEntity obj)
        {
            string sql = $"insert into {Table.Name} " +
                $"({nameof(BusinessProcessTypeEntity.Codigo)}, " +
                $"{nameof(BusinessProcessTypeEntity.Nombre)}, " +
                $"{nameof(BusinessProcessTypeEntity.Activo)}, " +
                $"{nameof(BusinessProcessTypeEntity.Descripcion)}, " +
                $"{nameof(BusinessProcessTypeEntity.Defecto)}, " +
                $"{nameof(BusinessProcessTypeEntity.ClienteID)}) " +
                $"values (@{nameof(BusinessProcessTypeEntity.Codigo)}, " +
                $"@{nameof(BusinessProcessTypeEntity.Nombre)}, " +
                $"@{nameof(BusinessProcessTypeEntity.Activo)}, " +
                $"@{nameof(BusinessProcessTypeEntity.Descripcion)}," +
                $" @{ nameof(BusinessProcessTypeEntity.Defecto)}," +
                $" @{ nameof(BusinessProcessTypeEntity.ClienteID)});" +
                 $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Activo = obj.Activo,
                Descripcion = obj.Descripcion,
                Defecto = obj.Defecto,
                ClienteID = obj.ClienteID,
            }, sqlTran)).First();

            return BusinessProcessTypeEntity.UpdateId(obj, newId);
        }

        public override async Task<BusinessProcessTypeEntity> UpdateSingle(BusinessProcessTypeEntity obj)
        {
            string sql = $"update {Table.Name} " + $"set {nameof(BusinessProcessTypeEntity.Codigo)} = @{nameof(BusinessProcessTypeEntity.Codigo)}, " +
                $" {nameof(BusinessProcessTypeEntity.Nombre)} =   @{nameof(BusinessProcessTypeEntity.Nombre)}, " +
                $" {nameof(BusinessProcessTypeEntity.Activo)} = @{nameof(BusinessProcessTypeEntity.Activo)}, " +
                $" {nameof(BusinessProcessTypeEntity.Descripcion)} =  @{nameof(BusinessProcessTypeEntity.Descripcion)}, " +
                $" {nameof(BusinessProcessTypeEntity.Defecto)} =  @{ nameof(BusinessProcessTypeEntity.Defecto)} " +
                $" where {nameof(BusinessProcessTypeEntity.CoreBusinessProcessTipoID)} = @{nameof(BusinessProcessTypeEntity.CoreBusinessProcessTipoID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Activo = obj.Activo,
                Descripcion = obj.Descripcion,
                Defecto = obj.Defecto,
                CoreBusinessProcessTipoID = obj.CoreBusinessProcessTipoID

            }, sqlTran));

            return obj;
        }

        public override async Task<BusinessProcessTypeEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<BusinessProcessTypeEntity>($"select * from {Table.Name} where {Table.Code} = @code " +
               $"AND {nameof(BusinessProcessTypeEntity.ClienteID)} = @{nameof(BusinessProcessTypeEntity.ClienteID)}", new
               {
                   code = code,
                   ClienteID = client

            }, sqlTran);

        }

        public override async Task<int> Delete(BusinessProcessTypeEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"delete from {Table.Name} Where {Table.ID} = @key" +
                $" AND {nameof(BusinessProcessTypeEntity.ClienteID)} = @{nameof(BusinessProcessTypeEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreBusinessProcessTipoID,
                    ClienteID = obj.ClienteID
                }, sqlTran);
            }
            catch (Exception e)
            {
                numReg = 0;
            }
            return numReg;
        }
    }
}
