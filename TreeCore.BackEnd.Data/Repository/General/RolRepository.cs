using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.General
{
    public class RolRepository : BaseRepository<RolEntity>
    {
        public override Table Table => TableNames.Rol;
        public Table TableProfiles => TableNames.Profile;
        public Table TableRolProfiles => TableNames.RolProfiles;

        private readonly ProfileRepository profileRepository;

        public RolRepository(TransactionalWrapper conexion) : base(conexion)
        {
            profileRepository = new ProfileRepository(conexion);
        }

        public override async Task<RolEntity> InsertSingle(RolEntity obj)
        {
            string sql = $"insert into {Table.Name} " +
                $"({nameof(RolEntity.ClienteID)}," +
                $"{nameof(RolEntity.Codigo)}, " +
                $"{nameof(RolEntity.Nombre)}, " +
                $"{nameof(RolEntity.Descripcion)}, " +
                $"{nameof(RolEntity.Activo)}) " +
                $"values " +
                $"(@{nameof(RolEntity.ClienteID)}," +
                $"@{nameof(RolEntity.Codigo)}, " +
                $"@{nameof(RolEntity.Nombre)}, " +
                $"@{nameof(RolEntity.Descripcion)}, " +
                $"@{nameof(RolEntity.Activo)}); " +
                $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo
            }, sqlTran)).First();

            if (obj.Profiles != null)
            {
                foreach (ProfileEntity profile in obj.Profiles.ToList())
                {
                    sql = $"INSERT INTO {TableRolProfiles.Name} (" +
                        $"{nameof(RolEntity.RolID)}, " +
                        $"{nameof(ProfileEntity.PerfilID)}) " +
                        $"values (" +
                        $"@{nameof(RolEntity.RolID)}, " +
                        $"@{nameof(ProfileEntity.PerfilID)}); " +
                        $"SELECT SCOPE_IDENTITY();";

                    await connection.QueryAsync<int>(sql, new
                    {
                        RolID = newId,
                        PerfilID = profile.PerfilID
                    }, sqlTran);
                };
            }

            return RolEntity.UpdateId(obj, newId);
        }
        public override async Task<RolEntity> UpdateSingle(RolEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql;

            #region Remove Profiles

            sql = $"delete from {TableRolProfiles.Name} Where {nameof(RolEntity.RolID)} = @{nameof(RolEntity.RolID)}";
            await connection.ExecuteAsync(sql, new
            {
                RolID = obj.RolID
            }, sqlTran);

            #endregion

            #region Add Profiles

            if (obj.Profiles != null)
            {
                foreach (ProfileEntity profile in obj.Profiles.ToList())
                {
                    sql = $"INSERT INTO {TableRolProfiles.Name} (" +
                        $"{nameof(RolEntity.RolID)}, " +
                        $"{nameof(ProfileEntity.PerfilID)}) " +
                        $"values (" +
                        $"@{nameof(RolEntity.RolID)}, " +
                        $"@{nameof(ProfileEntity.PerfilID)}); " +
                        $"SELECT SCOPE_IDENTITY();";

                    await connection.QueryAsync<int>(sql, new
                    {
                        RolID = obj.RolID,
                        PerfilID = profile.PerfilID
                    }, sqlTran);
                };
            }

            #endregion

            sql = $"update {Table.Name} set " +
                $" {nameof(RolEntity.Codigo)} = @{nameof(RolEntity.Codigo)}, " +
                $" {nameof(RolEntity.Nombre)} =   @{nameof(RolEntity.Nombre)}, " +
                $" {nameof(RolEntity.Descripcion)} =  @{nameof(RolEntity.Descripcion)}, " +
                $" {nameof(RolEntity.Activo)} = @{nameof(RolEntity.Activo)}" +
                $" where {nameof(RolEntity.RolID)} = @{nameof(RolEntity.RolID)} ";

            await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                RolID = obj.RolID
            }, sqlTran);
            return obj;
        }

        public override async Task<RolEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"select * from {Table.Name} R " +
                $"where R.{Table.Code} = @code AND R.{nameof(RolEntity.ClienteID)} = @{nameof(RolEntity.ClienteID)}";

            var result = await connection.QueryFirstOrDefaultAsync<RolEntity>(sql, new
            {
                code = code,
                ClienteID = Client
            }, sqlTran);
            if (result != null)
            {
                result.Profiles = await profileRepository.GetItemsByRol(code, Client);
            }
            return result;
        }

        public override async Task<int> Delete(RolEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql;
            int numReg;
            try
            {
                #region Remove Profiles

                sql = $"delete from {TableRolProfiles.Name} Where {nameof(RolEntity.RolID)} = @{nameof(RolEntity.RolID)}";
                await connection.ExecuteAsync(sql, new
                {
                    RolID = obj.RolID
                }, sqlTran);

                #endregion

                sql = $"delete from {Table.Name} Where {Table.ID} = @key AND {nameof(RolEntity.ClienteID)} = @{nameof(RolEntity.ClienteID)}";

                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.RolID,
                    ClienteID = obj.ClienteID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public override async Task<IEnumerable<RolEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            var listaRoles = await base.GetList(Client, filters, orders, direction, pageSize, pageIndex);
            foreach (var rol in listaRoles)
            {
                rol.Profiles = await profileRepository.GetItemsByRol(rol.Codigo, Client);
            }
            return listaRoles;
        }
    }
}
