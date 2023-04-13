using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository
{
    public abstract class BaseRepository<T>
        where T : class
    {
        protected readonly TransactionalWrapper _conexionWrapper;
        public BaseRepository(TransactionalWrapper conexion)
        {
            _conexionWrapper = conexion;
        }
        public abstract Table Table { get; }


        public abstract Task<T> InsertSingle(T obj);
        public abstract Task<T> UpdateSingle(T obj);
        public abstract Task<int> Delete(T obj);
        public abstract Task<T> GetItemByCode(string code, int Client);

        public async Task<List<T>> InsertList(List<T> obj)
        {
            return (await Task.WhenAll(obj.Select(a => InsertSingle(a)))).ToList();
        }
        public async Task<List<T>> UpdateList(List<T> obj)
        {
            return (await Task.WhenAll(obj.Select(a => UpdateSingle(a)))).ToList();
        }
        public async Task CommitTransaction()
        {
            await _conexionWrapper.CommitTransactionAsync();
        }
        public virtual async Task<IEnumerable<T>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;

            string sqlFilter = "";
            string sType = "";
            if (filters != null && filters.Count > 0 && filters[0] != null)
            {
                sqlFilter = " ";

                if (filters[0].Filters!= null && filters[0].Filters[0].Type != null && filters[0].Filters.Count > 1)
                {
                    sType = filters[0].Filters[0].Type;
                }
                else if (filters[0].Type != null)
                {
                    sType = filters[0].Type;
                }
                else
                {
                    sType = Filter.Types.AND;
                }
                sqlFilter += Filter.BuilFilters(filters, sType, countFilter, out countFilter, dicParam, out dicParam,"");
            }
            else if (filters != null && filters.Count > 0)
            {
                sqlFilter = " ";

                string filtros = Filter.BuilFilters(filters, Filter.Types.AND, countFilter, out countFilter, dicParam, out dicParam,"");

                if (filtros != " ")
                {
                    sqlFilter += filtros;
                }
                else
                {
                    sqlFilter = " ";
                }
            }

            if (string.IsNullOrWhiteSpace(sqlFilter))
            {
                sqlFilter = $" WHERE ClienteID = {Client}";
            }
            else 
            {
                sqlFilter += $" {Filter.Types.AND} ClienteID = {Client}";
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
            return await connection.QueryAsync<T?>(query, dicParam, await _conexionWrapper.GetTransactionAsync());
        }

    }

}
