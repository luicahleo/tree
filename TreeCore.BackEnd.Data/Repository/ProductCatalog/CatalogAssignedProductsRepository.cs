using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.Shared.Data.Db;
namespace TreeCore.BackEnd.Data.Repository.ProductCatalog
{
    public class CatalogAssignedProductsRepository : BaseRepository<CatalogAssignedProductsEntity>
    {
        public override Table Table => TableNames.CatalogAssignedProducts;
        public Table tableProduct => TableNames.Product;
        public Table tableCatalog => TableNames.Catalog;

        public CatalogAssignedProductsRepository (TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<CatalogAssignedProductsEntity> InsertSingle(CatalogAssignedProductsEntity obj)
        {
            string sql = $"";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {}, sqlTran)).First();

            return CatalogAssignedProductsEntity.UpdateId(obj, newId);
        }

        public override async Task<CatalogAssignedProductsEntity> UpdateSingle(CatalogAssignedProductsEntity obj)
        {
            string sql = $"";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {}, sqlTran));

            return obj;
        }

        public override async Task<CatalogAssignedProductsEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<CatalogAssignedProductsEntity>($"", new
            {}, sqlTran);

        }

        public async Task<CatalogAssignedProductsEntity> GetItemByCodes(string codeProduct, string codeCatalog)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} c " +
                $"inner join {tableProduct.Name} p on c.{tableProduct.ID} = p.{tableProduct.ID} " +
                $"inner join {tableCatalog.Name} cat on c.{tableCatalog.ID} = cat.{tableCatalog.ID} " +
                $"where p.{tableProduct.Code} = @codeProduct and cat.{tableCatalog.Code} = @codeCatalog";

            var result = await connection.QueryAsync<CatalogAssignedProductsEntity, ProductEntity, CatalogEntity, CatalogAssignedProductsEntity>(sql,
                (catalogAssignedProductsEntity, productEntity, catalogEntity) =>
                {
                    catalogAssignedProductsEntity.CoreProductCatalogServicios = productEntity;
                    catalogAssignedProductsEntity.CoreProductCatalogs = catalogEntity;
                    return catalogAssignedProductsEntity;
                }, new { codeProduct = codeProduct, codeCatalog = codeCatalog }, sqlTran, true, splitOn: $"{tableProduct.ID},{tableCatalog.ID}");

            return result.FirstOrDefault();
        }

        public override async Task<int> Delete(CatalogAssignedProductsEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {}, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }
    }
}
