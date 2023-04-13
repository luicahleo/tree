using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.General
{
    public class ProfileRepository : BaseRepository<ProfileEntity>
    {
        public override Table Table => TableNames.Profile;
        public Table TableRol => TableNames.Rol;
        public Table TableRolProfiles => TableNames.RolProfiles;

        public ProfileRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<ProfileEntity> InsertSingle(ProfileEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(ProfileEntity.ClienteID)},{nameof(ProfileEntity.Perfil_esES)}, " +
                $"{nameof(ProfileEntity.Descripcion)}, {nameof(ProfileEntity.CodigoModulo)}, {nameof(ProfileEntity.Activo)}, " +
                $"{ nameof(ProfileEntity.JsonUserFunctionalities)})" +
                $"values (@{nameof(ProfileEntity.ClienteID)},@{nameof(ProfileEntity.Perfil_esES)}, @{nameof(ProfileEntity.Descripcion)}, " +
                $"@{nameof(ProfileEntity.CodigoModulo)}, @{nameof(ProfileEntity.Activo)}, " +
                $" @{ nameof(ProfileEntity.JsonUserFunctionalities)});" +
                $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Perfil_esES = obj.Perfil_esES,
                Descripcion = obj.Descripcion,
                CodigoModulo = obj.CodigoModulo,
                Activo = obj.Activo,
                JsonUserFunctionalities = obj.JsonUserFunctionalities
            }, sqlTran)).First();

            return ProfileEntity.UpdateId(obj, newId);
        }
        public override async Task<ProfileEntity> UpdateSingle(ProfileEntity obj)
        {
            string sql = $"update {Table.Name} set " +
                $" {nameof(ProfileEntity.Perfil_esES)} = @{nameof(ProfileEntity.Perfil_esES)}, " +
                $" {nameof(ProfileEntity.Descripcion)} =   @{nameof(ProfileEntity.Descripcion)}, " +
                $" {nameof(ProfileEntity.CodigoModulo)} =  @{nameof(ProfileEntity.CodigoModulo)}, " +
                $" {nameof(ProfileEntity.Activo)} = @{nameof(ProfileEntity.Activo)}, " +
                $" {nameof(ProfileEntity.JsonUserFunctionalities)} =  @{ nameof(ProfileEntity.JsonUserFunctionalities)} " +
                $" where {nameof(ProfileEntity.PerfilID)} = @{nameof(ProfileEntity.PerfilID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Perfil_esES = obj.Perfil_esES,
                Descripcion = obj.Descripcion,
                CodigoModulo = obj.CodigoModulo,
                Activo = obj.Activo,
                JsonUserFunctionalities = obj.JsonUserFunctionalities,
                PerfilID = obj.PerfilID
            }, sqlTran));
            return obj;
        }

        public override async Task<ProfileEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<ProfileEntity>($"select * from {Table.Name} where {Table.Code} = @code AND {nameof(ProfileEntity.ClienteID)} = @{nameof(ProfileEntity.ClienteID)}", new
            {
                code = code,
                ClienteID = Client
            }, sqlTran);
        }

        public async Task<IEnumerable<ProfileEntity>> GetItemsByRol(string Rol, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"select P.* from {Table.Name} P " +
                $"left join {TableRolProfiles.Name} RP on P.{Table.ID} = RP.{Table.ID} " +
                $"left join {TableRol.Name} R on R.{TableRol.ID} = RP.{TableRol.ID} " +
                $"where {TableRol.Code} = @code AND R.{nameof(ProfileEntity.ClienteID)} = @{nameof(ProfileEntity.ClienteID)}";

            return await connection.QueryAsync<ProfileEntity>(sql, new
            {
                code = Rol,
                ClienteID = Client
            }, sqlTran);
        }

        public override async Task<int> Delete(ProfileEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key AND {nameof(ProfileEntity.ClienteID)} = @{nameof(ProfileEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.PerfilID,
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
