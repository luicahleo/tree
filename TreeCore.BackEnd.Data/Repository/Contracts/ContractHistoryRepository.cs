using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.Contracts
{
    public class ContractHistoryRepository : BaseRepository<ContractHistoryEntity>
    {
        public override Table Table => TableNames.ContractHistroy;
        public Table TableUser => TableNames.User;

        public ContractHistoryRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }
        public override Task<int> Delete(ContractHistoryEntity obj)
        {
            throw new System.NotImplementedException();
        }

        public override Task<ContractHistoryEntity> GetItemByCode(string code, int Client)
        {
            throw new System.NotImplementedException();
        }

        public override async Task<ContractHistoryEntity> InsertSingle(ContractHistoryEntity obj)
        {
            throw new System.NotImplementedException();
        }

        public override Task<ContractHistoryEntity> UpdateSingle(ContractHistoryEntity obj)
        {
            throw new System.NotImplementedException();
        }

        public  async Task<ContractHistoryEntity> InsertSingleByContractID(ContractHistoryEntity obj,string alquilerID)
        {
            string sql = $"insert into {Table.Name} " +
               $"({nameof(ContractHistoryEntity.FechaCreacion)},{nameof(ContractHistoryEntity.CreadorID)}, {nameof(ContractHistoryEntity.Activo)}," +
               $" {nameof(ContractHistoryEntity.Datos)}, {nameof(ContractHistoryEntity.ViaModificacion)},  { nameof(ContractHistoryEntity.AlquilerID)}," +
               $" {nameof(ContractHistoryEntity.ObtenerDatosHistoricos)}, {nameof(ContractHistoryEntity.VersionXML)})" +
               $" values " +
               $"(@{nameof(ContractHistoryEntity.FechaCreacion)},@{nameof(ContractHistoryEntity.CreadorID)}, @{nameof(ContractHistoryEntity.Activo)}," +
               $"@{nameof(ContractHistoryEntity.Datos)},@{nameof(ContractHistoryEntity.ViaModificacion)}, @{nameof(ContractHistoryEntity.AlquilerID)}," +
               $"@{nameof(ContractHistoryEntity.ObtenerDatosHistoricos)},@{nameof(ContractHistoryEntity.VersionXML)})" +
               $" SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                FechaCreacion = obj.FechaCreacion,
                CreadorID = obj.CreadorID,
                Activo = obj.Activo,
                Datos = obj.Datos,
                ViaModificacion = obj.ViaModificacion,
                AlquilerID = alquilerID,
                ObtenerDatosHistoricos = obj.ObtenerDatosHistoricos,
                VersionXML = obj.VersionXML
            }, sqlTran)).First();

            return ContractHistoryEntity.UpdateId(obj, newId);
        }

        public override async  Task<IEnumerable<ContractHistoryEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;

            string sqlFilter = "";
            string sType = "";
            if (filters != null && filters.Count > 0 && filters[0] != null)
            {
                sqlFilter = " WHERE ";

                if (filters[0].Filters != null && filters[0].Filters[0].Type != null && filters[0].Filters.Count > 1)
                {
                    sType = filters[0].Filters[0].Type;
                }
                else
                {
                    sType = Filter.Types.AND;
                }
                sqlFilter += Filter.BuilFilters(filters, sType, countFilter, out countFilter, dicParam, out dicParam, "");
            }
            else if (filters != null && filters.Count > 0)
            {
                sqlFilter = " WHERE ";

                string filtros = Filter.BuilFilters(filters, Filter.Types.AND, countFilter, out countFilter, dicParam, out dicParam, "");

                if (filtros != " ")
                {
                    sqlFilter += filtros;
                }
                else
                {
                    sqlFilter = " ";
                }
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
            return await connection.QueryAsync<ContractHistoryEntity>(query, dicParam, await _conexionWrapper.GetTransactionAsync());
        }
    }
}
