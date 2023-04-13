using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.General
{
    public class CountryRepository : BaseRepository<CountryEntity>
    {
        public override Table Table => TableNames.Country;

        public CountryRepository(TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<CountryEntity> InsertSingle(CountryEntity obj)
        {
            string sql = $"";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Pais = obj.Pais,
                PaisCod = obj.PaisCod,
                Activo = obj.Activo
            }, sqlTran)).First();

            return CountryEntity.UpdateId(obj, newId);
        }

        public override async Task<CountryEntity> UpdateSingle(CountryEntity obj)
        {
            string sql = $"";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                PaisID = obj.PaisID,
                Pais = obj.Pais,
                PaisCod = obj.PaisCod,
                Activo = obj.Activo
            }, sqlTran));

            return obj;
        }

        public override async Task<CountryEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<CountryEntity>($"select * from {Table.Name} where {Table.Code} = @code", new
            {
                code = code
            }, sqlTran);

        }

        public override async Task<int> Delete(CountryEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.PaisID
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

