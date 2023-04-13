using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Config;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.Config
{
    public class ViewRepository : BaseRepository<ViewEntity>
    {
        public override Table Table => TableNames.View;
        public Table TableUsuarios => TableNames.User;

        public ViewRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<ViewEntity> InsertSingle(ViewEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(ViewEntity.Nombre)},{nameof(ViewEntity.UsuarioID)}, " +
                $"{nameof(ViewEntity.JsonColumnas)}, {nameof(ViewEntity.JsonFiltros)}, {nameof(ViewEntity.Pagina)}, " +
                $"{ nameof(ViewEntity.Defecto)}, {nameof(ViewEntity.Icono)}, {nameof(ViewEntity.FechaCreacion)}, {nameof(ViewEntity.FechaUltimaModificacion)})" +
                $"values (@{nameof(ViewEntity.Nombre)},@{nameof(ViewEntity.UsuarioID)}, @{nameof(ViewEntity.JsonColumnas)}, " +
                $"@{nameof(ViewEntity.JsonFiltros)}, @{nameof(ViewEntity.Pagina)}, " +
                $" @{ nameof(ViewEntity.Defecto)}, @{nameof(ViewEntity.Icono)}, @{nameof(ViewEntity.FechaCreacion)}, @{nameof(ViewEntity.FechaUltimaModificacion)});" +
                $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Nombre = obj.Nombre,
                UsuarioID = obj.UsuarioID,
                JsonColumnas = obj.JsonColumnas,
                JsonFiltros = obj.JsonFiltros,
                Pagina = obj.Pagina,
                Defecto = obj.Defecto,
                Icono = obj.Icono,
                FechaCreacion = obj.FechaCreacion,
                FechaUltimaModificacion= obj.FechaUltimaModificacion
            }, sqlTran)).First();

            return ViewEntity.UpdateId(obj, newId);
        }
        public override async Task<ViewEntity> UpdateSingle(ViewEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(ViewEntity.Nombre)} = @{nameof(ViewEntity.Nombre)}, " +
                $" {nameof(ViewEntity.JsonColumnas)} =  @{nameof(ViewEntity.JsonColumnas)}, " +
                $" {nameof(ViewEntity.JsonFiltros)} = @{nameof(ViewEntity.JsonFiltros)}, " +
                $" {nameof(ViewEntity.Defecto)} =  @{ nameof(ViewEntity.Defecto)}, " +
                $" {nameof(ViewEntity.Icono)} =  @{ nameof(ViewEntity.Icono)}, " +
                $" {nameof(ViewEntity.FechaUltimaModificacion)} =  @{ nameof(ViewEntity.FechaUltimaModificacion)} " +
                $" where {nameof(ViewEntity.CoreGestionVistaID)} = @{nameof(ViewEntity.CoreGestionVistaID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            try
            {
                var filasafectadas = (await connection.ExecuteAsync(sql, new
                {
                    Nombre = obj.Nombre,
                    JsonColumnas = obj.JsonColumnas,
                    JsonFiltros = obj.JsonFiltros,
                    Defecto = obj.Defecto,
                    CoreGestionVistaID = obj.CoreGestionVistaID,
                    Icono = obj.Icono,
                    FechaUltimaModificacion = obj.FechaUltimaModificacion
                }, sqlTran));
            }
            catch(System.Exception ex) {
                throw;
            }
            return obj;
        }

        public override async Task<ViewEntity> GetItemByCode(string code, int UsuarioID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<ViewEntity>($"select * from {Table.Name} left join {TableUsuarios.Name} on {Table.Name}.{TableUsuarios.ID} = {TableUsuarios.Name}.{TableUsuarios.ID} where {Table.Name}.{Table.Code} = @code AND {TableUsuarios.ID} = @{TableUsuarios.ID}", new
            {
                code = code,
                UsuarioID = UsuarioID
            }, sqlTran);
        }

        public override async Task<int> Delete(ViewEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreGestionVistaID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public override async Task<IEnumerable<ViewEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;

            string sqlFilter = "";
            if (filters != null && filters.Count > 0)
            {
                sqlFilter = " WHERE ";
                sqlFilter += Filter.BuilFilters(filters, Filter.Types.AND, countFilter, out countFilter, dicParam, out dicParam, "");
            }

            string sqlOrder = "";
            if (orders != null && orders.Count > 0)
            {
                foreach (string column in orders)
                {
                    sqlOrder += $"{(string.IsNullOrEmpty(sqlOrder) ? " order by " : ", ")} {column}";
                }
            }
            else
            {
                sqlOrder = $" order by {Table.Code} ";
            }

            if (string.IsNullOrEmpty(direction) || (direction.ToLower() != "asc" && direction.ToLower() != "desc"))
            {
                direction = "ASC";
            }

            string sqlPagination = "";
            if (pageSize != -1 && pageIndex != -1)
            {
                sqlPagination = $"OFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            }


            string query = $"select TotalItems = COUNT(*) OVER(), * from {Table.Name} {sqlFilter} {sqlOrder} {direction} {sqlPagination}";
            return await connection.QueryAsync<ViewEntity?>(query, dicParam, await _conexionWrapper.GetTransactionAsync());
        }

    }
}
