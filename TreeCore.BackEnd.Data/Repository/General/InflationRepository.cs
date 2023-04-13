using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;
using System.Collections.Generic;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.General
{
    public class InflationRepository : BaseRepository<InflationEntity>
    {
        public override Table Table => TableNames.Inflation;
        public Table tableCountry => TableNames.Country;

        public InflationRepository(TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<InflationEntity> InsertSingle(InflationEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(InflationEntity.Inflacion)}, {nameof(InflationEntity.Descripcion)}, {nameof(InflationEntity.Codigo)}," +
                $"{nameof(InflationEntity.Estandar)}, {nameof(InflationEntity.Paises.PaisID)}, {nameof(InflationEntity.Activo)}) " +
                $"values (@{nameof(InflationEntity.Inflacion)}, @{nameof(InflationEntity.Descripcion)}, @{nameof(InflationEntity.Codigo)}, " +
                $"@{nameof(InflationEntity.Estandar)}, @{nameof(InflationEntity.Paises.PaisID)}, @{nameof(InflationEntity.Activo)});" +
                 $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Inflacion = obj.Inflacion,
                Codigo = obj.Codigo,
                Descripcion = obj.Descripcion,
                Estandar = obj.Estandar,
                Activo = obj.Activo,
                PaisID = obj.Paises.PaisID
            }, sqlTran)).First();

            return InflationEntity.UpdateId(obj, newId);
        }

        public override async Task<InflationEntity> UpdateSingle(InflationEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(InflationEntity.Inflacion)} = @{nameof(InflationEntity.Inflacion)}, " +
                $" {nameof(InflationEntity.Descripcion)} =   @{nameof(InflationEntity.Descripcion)}, " +
                $" {nameof(InflationEntity.Codigo)} =   @{nameof(InflationEntity.Codigo)}, " +
                $" {nameof(InflationEntity.Activo)} =   @{nameof(InflationEntity.Activo)}, " +
                $" {nameof(InflationEntity.Estandar)} =  @{nameof(InflationEntity.Estandar)}, " +
                $" {nameof(InflationEntity.Paises.PaisID)} = @{nameof(InflationEntity.Paises.PaisID)}" +
                $" where {nameof(InflationEntity.InflacionID)} = @{nameof(InflationEntity.InflacionID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                InflacionID = obj.InflacionID,
                Inflacion = obj.Inflacion,
                Codigo = obj.Codigo,
                Descripcion = obj.Descripcion,
                Estandar = obj.Estandar,
                Activo = obj.Activo,
                PaisID = obj.Paises.PaisID
            }, sqlTran));

            return obj;
        }

        public override async Task<InflationEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} i inner join {tableCountry.Name} c on i.{tableCountry.ID} = c.{tableCountry.ID} " +
                $"where i.{Table.Code} = @code";

            var result = await connection.QueryAsync<InflationEntity, CountryEntity, InflationEntity>(sql,
                (inflationEntity, countryEntity) =>
                {
                    inflationEntity.Paises = countryEntity;
                    return inflationEntity;
                }, new { code = code }, sqlTran, true, splitOn: "PaisID");

            return result.FirstOrDefault();
        }

        public override async Task<IEnumerable<InflationEntity>> GetList(int client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;


            string sqlRelation = $"i inner join {tableCountry.Name} c on i.{tableCountry.ID} = c.{tableCountry.ID} ";

            string sqlFilter = " WHERE 1 = 1 ";
            if (filters != null)
            {
                foreach (Filter filter in filters)
                {
                    int iterator = countFilter++;
                    sqlFilter += $" AND i.{filter.Column} {filter.Operator} @{filter.Column}{iterator} ";
                    string nameVar = $"{filter.Column}{iterator}";
                    dicParam.Add(nameVar, filter.Value);
                }
            }

            string sqlOrder = "";
            if (orders != null && orders.Count > 0)
            {
                foreach (string column in orders)
                {
                    sqlOrder += $"{(string.IsNullOrEmpty(sqlOrder) ? " order by i." : ", ")} {column}";
                }
            }
            else
            {
                sqlOrder = $" order by i.{Table.Code} ";
            }

            string sqlPagination = "";
            if (pageSize != -1 && pageIndex != -1)
            {
                sqlPagination = $"OFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            }


            string query = $"select TotalItems = COUNT(*) OVER(), * from {Table.Name} {sqlRelation} {sqlFilter} {sqlOrder} {sqlPagination}";

            var result = await connection.QueryAsync<InflationEntity, CountryEntity, InflationEntity>(query,
                (inflationEntity, countryEntity) =>
                {
                    inflationEntity.Paises = countryEntity;
                    return inflationEntity;
                }
                , dicParam, sqlTran, true, splitOn: "PaisID");

            return result.ToList();
        }

        public override async Task<int> Delete(InflationEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.InflacionID
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
