using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.Inventory;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.Inventory
{

    public class InventoryRepository : BaseRepository<InventoryEntity>
    {
        public override Table Table => TableNames.Inventory;

        public InventoryRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<InventoryEntity> InsertSingle(InventoryEntity obj)
        {
            int newId = 0;

            return InventoryEntity.UpdateId(obj, newId);
        }
        public override async Task<InventoryEntity> UpdateSingle(InventoryEntity obj)
        {
           
            return obj;
        }

        public override async Task<InventoryEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<InventoryEntity>($"select * from {Table.Name} where Codigo = @code and ClienteID = @client", new
            {
                code = code,
                client = client
            }, sqlTran);

        }

        public override async Task<int> Delete(InventoryEntity obj)
        {
           
            int numReg=0;
          
            return numReg;
        }
    }
}
