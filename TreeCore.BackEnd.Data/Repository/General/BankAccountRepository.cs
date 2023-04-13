using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.General
{
    public class BankAccountRepository : BaseRepository<BankAccountEntity>
    {
        public override Table Table => TableNames.BankAccount;
        public Table tableCompany => TableNames.Company;
        public Table tableBank => TableNames.Bank;

        public BankAccountRepository(TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<BankAccountEntity> InsertSingle(BankAccountEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(BankAccountEntity.Codigo)}, {nameof(BankAccountEntity.IBAN)}," +
                $"{nameof(BankAccountEntity.Descripcion)}, {nameof(BankAccountEntity.SWIFT)},{nameof(BankAccountEntity.Company.EntidadID)}," +
                $"{nameof(BankAccountEntity.Bank.BancoID)} )" +
                $"values (@{nameof(BankAccountEntity.Codigo)}, @{nameof(BankAccountEntity.IBAN)}, @{nameof(BankAccountEntity.Descripcion)}, " +
                $" @{nameof(BankAccountEntity.SWIFT)}, @{nameof(BankAccountEntity.Company.EntidadID)},@{nameof(BankAccountEntity.Bank.BancoID)} );" +
                $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            int newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                IBAN = obj.IBAN,
                EntidadID = obj.Company.EntidadID,
                SWIFT = obj.SWIFT,
                BancoID = obj.Bank.BancoID,
                Descripcion = obj.Descripcion
            }, sqlTran)).First();


            return BankAccountEntity.UpdateId(obj, newId);
        }
        public async Task<BankAccountEntity> InsertSingleWithCompany(BankAccountEntity obj, string entidadID)
        {
            string sql = $"insert into {Table.Name} ({nameof(BankAccountEntity.Codigo)}, {nameof(BankAccountEntity.IBAN)}," +
                $"{nameof(BankAccountEntity.Descripcion)}, {nameof(BankAccountEntity.SWIFT)},{nameof(BankAccountEntity.Company.EntidadID)}," +
                $"{nameof(BankAccountEntity.Bank.BancoID)} )" +
                $"values (@{nameof(BankAccountEntity.Codigo)}, @{nameof(BankAccountEntity.IBAN)}, @{nameof(BankAccountEntity.Descripcion)}, " +
                $" @{nameof(BankAccountEntity.SWIFT)}, @{nameof(BankAccountEntity.Company.EntidadID)},@{nameof(BankAccountEntity.Bank.BancoID)} );" +
                $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            int newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                IBAN = obj.IBAN,
                EntidadID = entidadID,
                SWIFT = obj.SWIFT,
                BancoID = obj.Bank.BancoID,
                Descripcion = obj.Descripcion
            }, sqlTran)).First();


            return BankAccountEntity.UpdateId(obj, newId);
        }

        public override async Task<BankAccountEntity> UpdateSingle(BankAccountEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql;

            sql = $"update {Table.Name} set " +
               $" {nameof(BankAccountEntity.IBAN)} = @{nameof(BankAccountEntity.IBAN)}, " +
               $" {nameof(BankAccountEntity.Descripcion)} = @{nameof(BankAccountEntity.Descripcion)}, " +
               $" {nameof(BankAccountEntity.SWIFT)} = @{nameof(BankAccountEntity.SWIFT)}, " +
               $" {nameof(BankAccountEntity.Bank.BancoID)} = @{nameof(BankAccountEntity.Bank.BancoID)} " +
               $" where {nameof(BankAccountEntity.Codigo)} = @{nameof(BankAccountEntity.Codigo)} ";

            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                IBAN = obj.IBAN,
                Descripcion = obj.Descripcion,
                SWIFT = obj.SWIFT,
                BancoID = obj.Bank.BancoID,
                Codigo = obj.Codigo
            }, sqlTran));

            return obj;
        }

        public override async Task<BankAccountEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} p inner join {tableCompany.Name} c on p.{tableCompany.ID} = c.{tableCompany.ID}" +
                $" inner join {tableBank.Name} pt on p.{tableBank.ID} = pt.{tableBank.ID}" +
                $" where p.{Table.Code} = @code";

            var result = await connection.QueryAsync<BankAccountEntity, CompanyEntity, BankEntity, BankAccountEntity>(sql,
            (bankAccountEntity, companyEntity, bankEntity) =>
            {
                bankAccountEntity.Company = companyEntity;
                bankAccountEntity.Bank = bankEntity;
                return bankAccountEntity;
            }, new { code = code }, sqlTran, true, splitOn: $"{tableCompany.ID},{tableBank.ID}");

            return result.FirstOrDefault();
        }

        public override async Task<IEnumerable<BankAccountEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;


            string sqlRelation = $"p inner join {tableCompany.Name} c on p.{tableCompany.ID} = c.{tableCompany.ID} " +
                $" inner join {tableBank.Name} pt on p.{tableBank.ID} = pt.{tableBank.ID} ";

            string sqlFilter = "";
            if (filters != null && filters.Count > 0)
            {
                sqlFilter = " WHERE ";
                sqlFilter += Filter.BuilFilters(filters, Filter.Types.AND, countFilter, out countFilter, dicParam, out dicParam, "p");
            }

            string sqlOrder = "";
            if (orders != null && orders.Count > 0)
            {
                foreach (string column in orders)
                {
                    sqlOrder += $"{(string.IsNullOrEmpty(sqlOrder) ? " order by p." : ", ")} {column}";
                }
            }
            else
            {
                sqlOrder = $" order by p.{Table.Code} ";
            }

            string sqlPagination = "";
            if (pageSize != -1 && pageIndex != -1)
            {
                sqlPagination = $"OFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            }

            string query = $"select TotalItems = COUNT(*) OVER(), * from {Table.Name} {sqlRelation} {sqlFilter} {sqlOrder} {sqlPagination}";

            var result = await connection.QueryAsync<BankAccountEntity, CompanyEntity, BankEntity, BankAccountEntity>(query,
            (bankAccountEntity, companyEntity, bankEntity) =>
            {
                bankAccountEntity.Company = companyEntity;
                bankAccountEntity.Bank = bankEntity;

                return bankAccountEntity;
            },
            dicParam, sqlTran, true, splitOn: $"{tableCompany.ID},{tableBank.ID}");

            result = result.Distinct();


            return result.ToList();
        }
        public async Task<IEnumerable<BankAccountEntity>> GetListbyCompany(int EntidadID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string query = $"select * from {Table.Name} where {tableCompany.ID} = @entidadID";

            var result = await connection.QueryAsync<BankAccountEntity>(query, new { entidadID = EntidadID }, sqlTran);

            return result.ToList();
        }
        public async Task<IEnumerable<BankAccountEntity>> GetListbyCompanyDetails(int EntidadID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql2 = $"select * from {Table.Name} C " +
                $"inner join {tableBank.Name} CT on C.{tableBank.ID} = CT.{tableBank.ID} " +
                $"inner join {tableCompany.Name} TP on C.{tableCompany.ID} = TP.{tableCompany.ID} " +
                $" where C.{tableCompany.ID} = @entidadID";

            var result = await connection.QueryAsync<BankAccountEntity, BankEntity, CompanyEntity, BankAccountEntity>(sql2,
                (bankAccountEntity, bankEntity, companyEntity) =>
                {
                    bankAccountEntity.Bank = bankEntity;
                    bankAccountEntity.Company = companyEntity;
                    return bankAccountEntity;
                },
                new
                {
                    entidadID = EntidadID
                }, sqlTran, true, splitOn: $"{tableBank.ID},{tableCompany.ID}");

            return result.ToList();
        }

        public async Task<IEnumerable<CompanyEntity>> GetListBankAccount(IEnumerable<CompanyEntity> result, string IDs)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql2 = $"select * from {Table.Name} C " +
                $"inner join {tableBank.Name} CT on C.{tableBank.ID} = CT.{tableBank.ID} " +
                $"inner join {tableCompany.Name} TP on C.{tableCompany.ID} = TP.{tableCompany.ID} " +
                $" where C.{tableCompany.ID} in ({IDs})";
            List<BankAccountEntity> listAssigned = new List<BankAccountEntity>();
            var result2 = await connection.QueryAsync<BankAccountEntity, BankEntity, CompanyEntity, BankAccountEntity>(sql2,
                (bankAccountEntity, bankEntity, companyEntity) =>
                {
                    if (bankAccountEntity != null)
                    {
                        bankAccountEntity.Bank = bankEntity;
                        bankAccountEntity.Company = companyEntity;

                        result.Where(pEnt => pEnt.EntidadID == bankAccountEntity.Company.EntidadID).ToList().ForEach(lk =>
                        {
                            if(lk.CuentasBancarias == null)
                            {
                                listAssigned = new List<BankAccountEntity>();
                            }
                            listAssigned.Add(bankAccountEntity);
                            lk.CuentasBancarias = listAssigned;
                        }
                        );
                    }
                    return bankAccountEntity;
                }, new { }, sqlTran, true, splitOn: $"{tableBank.ID},{tableCompany.ID}");

            return result.ToList();
        }

        public override async Task<int> Delete(BankAccountEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.EntidadCuentaBancariaID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }
        public async Task<int> DeleteByCompany(int EntidadID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {tableCompany.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = EntidadID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public async Task<IEnumerable<BankAccountEntity>> InsertList(IEnumerable<BankAccountEntity> obj, string entidadID)
        {
            foreach (BankAccountEntity servicioVinculado in obj)
            {
                var result = await InsertSingleWithCompany(servicioVinculado, entidadID);
            };
            return obj;
        }
        public async Task<IEnumerable<BankAccountEntity>> UpdateList(IEnumerable<BankAccountEntity> obj, string entidadID)
        {
            IEnumerable<BankAccountEntity> listaExiste = GetListbyCompany(int.Parse(entidadID)).Result;

            #region Remove linked Products

            foreach (BankAccountEntity bankAccount in listaExiste)
            {
                int idBorrar = compruebaExiste(obj, bankAccount);
                if (idBorrar != 0)
                {
                    var result = await Delete(bankAccount);
                }

            }

            #endregion

            #region Add linked Products
            if (obj != null)
            {
                foreach (BankAccountEntity servicioVinculado in obj.ToList())
                {
                    if (contieneNombre(listaExiste, servicioVinculado) == 0)
                    {

                        var result = await InsertSingleWithCompany(servicioVinculado, entidadID);
                    }
                    else
                    {
                        var result = await UpdateSingle(servicioVinculado);
                    }


                };
            }
            #endregion
            return obj;
        }

        private int contieneNombre(IEnumerable<BankAccountEntity> listaExiste, BankAccountEntity obj)
        {
            foreach (BankAccountEntity bankAccount in listaExiste)
            {
                if (bankAccount.Codigo == obj.Codigo)
                {
                    return bankAccount.EntidadCuentaBancariaID.Value;
                }
            }
            return 0;
        }
        private int compruebaExiste(IEnumerable<BankAccountEntity> listaExiste, BankAccountEntity obj)
        {
            foreach (BankAccountEntity bankAccount in listaExiste)
            {
                if (bankAccount.Codigo == obj.Codigo)
                {
                    return 0;
                }
            }

            return obj.EntidadCuentaBancariaID.Value;
        }

    }
}

