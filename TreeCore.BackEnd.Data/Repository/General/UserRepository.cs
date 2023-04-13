using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.General
{
    public class UserRepository : BaseRepository<UserEntity>
    {
        public override Table Table => TableNames.User;

        public UserRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public async override Task<UserEntity> InsertSingle(UserEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(UserEntity.Nombre)}, {nameof(UserEntity.Apellidos)}, " +
                $"{nameof(UserEntity.EMail)}, {nameof(UserEntity.Clave)}, {nameof(UserEntity.Activo)}, {nameof(UserEntity.CambiarClave)}, " +
                $"{nameof(UserEntity.FechaUltimoCambio)}, {nameof(UserEntity.UltimasClaves)}, {nameof(UserEntity.Telefono)}, " +
                $"{nameof(UserEntity.ClienteID)}, {nameof(UserEntity.LDAP)}, {nameof(UserEntity.FechaCaducidadUsuario)}, " +
                $"{nameof(UserEntity.FechaCaducidadClave)}, {nameof(UserEntity.Interno)}, {nameof(UserEntity.FechaUltimoAcceso)}," +
                $"{nameof(UserEntity.MacDispositivo)}, {nameof(UserEntity.NombreCompleto)}, {nameof(UserEntity.FechaCreacion)}, " +
                $"{nameof(UserEntity.FechaDesactivacion)}, {nameof(UserEntity.EntidadID)}, {nameof(UserEntity.DepartamentoID)})" +
                $"values (@{ nameof(UserEntity.Nombre)}, @{ nameof(UserEntity.Apellidos)}, " +
                $"@{ nameof(UserEntity.EMail)}, @{ nameof(UserEntity.Clave)}, @{ nameof(UserEntity.Activo)}, @{ nameof(UserEntity.CambiarClave)}, " +
                $"@{ nameof(UserEntity.FechaUltimoCambio)}, @{ nameof(UserEntity.UltimasClaves)}, @{ nameof(UserEntity.Telefono)}, " +
                $"@{ nameof(UserEntity.ClienteID)}, @{ nameof(UserEntity.LDAP)}, @{ nameof(UserEntity.FechaCaducidadUsuario)}, " +
                $"@{ nameof(UserEntity.FechaCaducidadClave)}, @{ nameof(UserEntity.Interno)}, @{ nameof(UserEntity.FechaUltimoAcceso)}, " +
                $"@{ nameof(UserEntity.MacDispositivo)}, @{ nameof(UserEntity.NombreCompleto)}, @{ nameof(UserEntity.FechaCreacion)}, " +
                $"@{ nameof(UserEntity.FechaDesactivacion)}, @{ nameof(UserEntity.EntidadID)}, @{ nameof(UserEntity.DepartamentoID)}); " +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Nombre = obj.Nombre,
                Apellidos = obj.Apellidos,
                EMail = obj.EMail,
                Clave = obj.Clave,
                Activo = obj.Activo,
                CambiarClave = obj.CambiarClave,
                FechaUltimoCambio = obj.FechaUltimoCambio,
                UltimasClaves = obj.UltimasClaves,
                Telefono = obj.Telefono,
                ClienteID = obj.ClienteID,
                LDAP = obj.LDAP,
                FechaCaducidadUsuario = obj.FechaCaducidadUsuario,
                FechaCaducidadClave = obj.FechaCaducidadClave,
                Interno = obj.Interno,
                FechaUltimoAcceso = obj.FechaUltimoAcceso,
                MacDispositivo = obj.MacDispositivo,
                NombreCompleto = obj.NombreCompleto,
                FechaCreacion = obj.FechaCreacion,
                FechaDesactivacion = obj.FechaDesactivacion,
                EntidadID = obj.EntidadID,
                DepartamentoID = obj.DepartamentoID
            }, sqlTran)).First();

            return UserEntity.UpdateId(obj, newId);
        }

        public override async Task<UserEntity> UpdateSingle(UserEntity obj)
        {
            string sql = $"update {Table.Name} set { nameof(UserEntity.Nombre)} = @{ nameof(UserEntity.Nombre)}, " +
                $"{ nameof(UserEntity.Apellidos)} = @{ nameof(UserEntity.Apellidos)}," +
                $"{ nameof(UserEntity.EMail)} = @{ nameof(UserEntity.EMail)}, " +
                $"{ nameof(UserEntity.Clave)} = @{ nameof(UserEntity.Clave)}, " +
                $"{ nameof(UserEntity.Activo)} = @{ nameof(UserEntity.Activo)}," +
                $"{ nameof(UserEntity.CambiarClave)} = @{ nameof(UserEntity.CambiarClave)}, " +
                $"{ nameof(UserEntity.FechaUltimoCambio)} = @{ nameof(UserEntity.FechaUltimoCambio)}," +
                $"{ nameof(UserEntity.UltimasClaves)} = @{ nameof(UserEntity.UltimasClaves)}," +
                $"{ nameof(UserEntity.Telefono)} = @{ nameof(UserEntity.Telefono)}," +
                $"{ nameof(UserEntity.ClienteID)} = @{ nameof(UserEntity.ClienteID)}," +
                $"{ nameof(UserEntity.LDAP)} = @{ nameof(UserEntity.LDAP)}," +
                $"{ nameof(UserEntity.FechaCaducidadUsuario)} = @{ nameof(UserEntity.FechaCaducidadUsuario)}," +
                $"{ nameof(UserEntity.FechaCaducidadClave)} = @{ nameof(UserEntity.FechaCaducidadClave)}," +
                $"{ nameof(UserEntity.Interno)} = @{ nameof(UserEntity.Interno)}," +
                $"{ nameof(UserEntity.FechaUltimoAcceso)} = @{ nameof(UserEntity.FechaUltimoAcceso)}," +
                $"{ nameof(UserEntity.MacDispositivo)} = @{ nameof(UserEntity.MacDispositivo)}," +
                $"{ nameof(UserEntity.NombreCompleto)} = @{ nameof(UserEntity.NombreCompleto)}," +
                $"{ nameof(UserEntity.FechaCreacion)} = @{ nameof(UserEntity.FechaCreacion)}," +
                $"{ nameof(UserEntity.FechaDesactivacion)} = @{ nameof(UserEntity.FechaDesactivacion)}," +
                $"{ nameof(UserEntity.EntidadID)} = @{ nameof(UserEntity.EntidadID)}," +
                $"{ nameof(UserEntity.DepartamentoID)} = @{ nameof(UserEntity.DepartamentoID)} " +
                $" where {nameof(UserEntity.UsuarioID)} = @{nameof(UserEntity.UsuarioID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Nombre = obj.Nombre,
                Apellidos = obj.Apellidos,
                EMail = obj.EMail,
                Clave = obj.Clave,
                Activo = obj.Activo,
                CambiarClave = obj.CambiarClave,
                FechaUltimoCambio = obj.FechaUltimoCambio,
                UltimasClaves = obj.UltimasClaves,
                Telefono = obj.Telefono,
                ClienteID = obj.ClienteID,
                LDAP = obj.LDAP,
                FechaCaducidadUsuario = obj.FechaCaducidadUsuario,
                FechaCaducidadClave = obj.FechaCaducidadClave,
                Interno = obj.Interno,
                FechaUltimoAcceso = obj.FechaUltimoAcceso,
                MacDispositivo = obj.MacDispositivo,
                NombreCompleto = obj.NombreCompleto,
                FechaCreacion = obj.FechaCreacion,
                FechaDesactivacion = obj.FechaDesactivacion,
                EntidadID = obj.EntidadID,
                DepartamentoID = obj.DepartamentoID
            }, sqlTran));
            return obj;
        }

        public override async Task<UserEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<UserEntity>($"select * from {Table.Name} where EMail = @code" +
                $" AND {nameof(UserEntity.ClienteID)} = @{nameof(UserEntity.ClienteID)}", new
            {
                code = code,
                ClienteID = client
            }, sqlTran);
        }

        public async Task<UserEntity> GetItemByCode(string code)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<UserEntity>($"select * from {Table.Name} where EMail = @code" +
                $" AND ClienteID IS NOT NULL", new
                {
                    code = code
                }, sqlTran);
        }
        public async Task<UserEntity> GetItemByCompany(string ID, Table tabla,string columna)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"select * from {Table.Name} where {Table.ID} = (select {columna} from {tabla.Name} where {tabla.ID} = @ID)";
            var result = await connection.QueryFirstOrDefaultAsync<UserEntity>(sql, new { ID = ID }, sqlTran);

            return result;
        }

        public override async Task<int> Delete(UserEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key" +
                $" AND {nameof(UserEntity.ClienteID)} = @{nameof(UserEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.UsuarioID,
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
