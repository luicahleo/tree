using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.Sites;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.Sites
{

    public class SiteRepository : BaseRepository<SiteEntity>
    {
        public override Table Table => TableNames.Sites;

        public SiteRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<SiteEntity> InsertSingle(SiteEntity obj)
        {
            int newId = 0;

            return SiteEntity.UpdateId(obj, newId);
        }
        public override async Task<SiteEntity> UpdateSingle(SiteEntity obj)
        {
           
            return obj;
        }

        public override async Task<SiteEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<SiteEntity>($"select * from {Table.Name} where Codigo = @code", new
            {
                code = code
            }, sqlTran);

        }

        public override async Task<int> Delete(SiteEntity obj)
        {
           
            int numReg=0;
          
            return numReg;
        }
    }
}
