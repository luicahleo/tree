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
    public class TaxesRepository : BaseRepository<TaxesEntity>
    {
        public override Table Table => TableNames.Taxes;
        public Table tableCountry => TableNames.Country;

        public TaxesRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<TaxesEntity> InsertSingle(TaxesEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(TaxesEntity.ClienteID)}, " +
                $"{nameof(TaxesEntity.Codigo)}, " +
                $"{nameof(TaxesEntity.Impuesto)}, " +
                $"{nameof(TaxesEntity.Paises.PaisID)}," +
                $"{nameof(TaxesEntity.Descripcion)}, " +
                $"{nameof(TaxesEntity.FechaActualizacion)}, " +
                $"{nameof(TaxesEntity.Valor)}," +
                $"{nameof(TaxesEntity.Activo)}, " +
                $"{nameof(TaxesEntity.Defecto)}) " +
                $"values (" +
                $"@{nameof(TaxesEntity.ClienteID)}," +
                $"@{nameof(TaxesEntity.Codigo)}, " +
                $"@{nameof(TaxesEntity.Impuesto)}, " +
                $"@{nameof(TaxesEntity.Paises.PaisID)}, " +
                $"@{nameof(TaxesEntity.Descripcion)}, " +
                $"@{nameof(TaxesEntity.FechaActualizacion)}, " +
                $"@{nameof(TaxesEntity.Valor)}, " +
                $"@{nameof(TaxesEntity.Activo)}, " +
                $"@{nameof(TaxesEntity.Defecto)}); " +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.Codigo,
                Impuesto = obj.Impuesto,
                PaisID = obj.Paises.PaisID,
                Descripcion = obj.Descripcion,
                FechaActualizacion = obj.FechaActualizacion,
                Valor = obj.Valor,
                Activo = obj.Activo,
                Defecto = obj.Defecto
            }, sqlTran)).First();

            return TaxesEntity.UpdateId(obj, newId);
        }
        public override async Task<TaxesEntity> UpdateSingle(TaxesEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(TaxesEntity.Codigo)} = @{nameof(TaxesEntity.Codigo)}, " +
                $" {nameof(TaxesEntity.Impuesto)} =   @{nameof(TaxesEntity.Impuesto)}, " +
                $" {nameof(TaxesEntity.Descripcion)} =  @{nameof(TaxesEntity.Descripcion)}, " +
                $" {nameof(TaxesEntity.FechaActualizacion)} =  @{nameof(TaxesEntity.FechaActualizacion)}, " +
                $" {nameof(TaxesEntity.Valor)} =  @{nameof(TaxesEntity.Valor)}, " +
                $" {nameof(TaxesEntity.Paises.PaisID)} =  @{nameof(TaxesEntity.Paises.PaisID)}, " +
                $" {nameof(TaxesEntity.Activo)} = @{nameof(TaxesEntity.Activo)}, " +
                $" {nameof(TaxesEntity.Defecto)} =  @{ nameof(TaxesEntity.Defecto)} " +
                $" where {nameof(TaxesEntity.ImpuestoID)} = @{nameof(TaxesEntity.ImpuestoID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Impuesto = obj.Impuesto,
                Descripcion = obj.Descripcion,
                FechaActualizacion = obj.FechaActualizacion,
                Valor = obj.Valor,
                PaisID = obj.Paises.PaisID,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                ImpuestoID = obj.ImpuestoID
            }, sqlTran));
            return obj;
        }

        public override async Task<TaxesEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<TaxesEntity>($"select * from {Table.Name} where Codigo = @code" +
                $" AND {nameof(TaxesEntity.ClienteID)} = @{nameof(TaxesEntity.ClienteID)}", new
                {
                    code = code,
                    ClienteID = client
                }, sqlTran);

        }



        public override async Task<IEnumerable<TaxesEntity>> GetList(int client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;


            string sqlRelation = $"i inner join {tableCountry.Name} c on i.{tableCountry.ID} = c.{tableCountry.ID} ";

            string sqlFilter = $" WHERE i.{nameof(TaxesEntity.ClienteID)} = {client} ";
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

            var result = await connection.QueryAsync<TaxesEntity, CountryEntity, TaxesEntity>(query,
                (taxesEntity, countryEntity) =>
                {
                    taxesEntity.Paises = countryEntity;
                    return taxesEntity;
                }
                , dicParam, sqlTran, true, splitOn: "PaisID");

            return result.ToList();
        }


        public override async Task<int> Delete(TaxesEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key" +
                $" AND {nameof(TaxesEntity.ClienteID)} = @{nameof(TaxesEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.ImpuestoID,
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
