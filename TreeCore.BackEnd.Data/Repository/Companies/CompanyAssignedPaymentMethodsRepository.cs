using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;
namespace TreeCore.BackEnd.Data.Repository.Companies
{
    public class CompanyAssignedPaymentMethodsRepository : BaseRepository<CompanyAssignedPaymentMethodsEntity>
    {
        public override Table Table => TableNames.CompanyPaymentMethods;

        public Table tableCompany => TableNames.Company;
        public Table tablePaymentMethods => TableNames.PaymentMethods;

        public CompanyAssignedPaymentMethodsRepository(TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<CompanyAssignedPaymentMethodsEntity> InsertSingle(CompanyAssignedPaymentMethodsEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(CompanyAssignedPaymentMethodsEntity.CoreCompany.EntidadID)}," +
                $"{nameof(CompanyAssignedPaymentMethodsEntity.Defecto)}," +
                $" {nameof(CompanyAssignedPaymentMethodsEntity.CoreMetodosPagos.MetodoPagoID)} )" +
                $"values (" +
                $"@{nameof(CompanyAssignedPaymentMethodsEntity.CoreCompany.EntidadID)}, " +
                $"@{nameof(CompanyAssignedPaymentMethodsEntity.Defecto)}, " +
                $"@{nameof(CompanyAssignedPaymentMethodsEntity.CoreMetodosPagos.MetodoPagoID)} );" +
                $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                EntidadID = obj.CoreCompany.EntidadID,
                Defecto = obj.Defecto,
                MetodoPagoID = obj.CoreMetodosPagos.MetodoPagoID
            }, sqlTran)).First();

            return CompanyAssignedPaymentMethodsEntity.UpdateId(obj, newId);
        }
        public async Task<CompanyAssignedPaymentMethodsEntity> InsertSingleWithCompany(CompanyAssignedPaymentMethodsEntity obj, string entidadID)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(CompanyAssignedPaymentMethodsEntity.CoreCompany.EntidadID)}," +
                $"{nameof(CompanyAssignedPaymentMethodsEntity.Defecto)}," +
                $" {nameof(CompanyAssignedPaymentMethodsEntity.CoreMetodosPagos.MetodoPagoID)} )" +
                $"values (" +
                $"@{nameof(CompanyAssignedPaymentMethodsEntity.CoreCompany.EntidadID)}, " +
                $"@{nameof(CompanyAssignedPaymentMethodsEntity.Defecto)}, " +
                $"@{nameof(CompanyAssignedPaymentMethodsEntity.CoreMetodosPagos.MetodoPagoID)} );" +
                $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                EntidadID = entidadID,
                Defecto = obj.Defecto,
                MetodoPagoID = obj.CoreMetodosPagos.MetodoPagoID
            }, sqlTran)).First();

            return CompanyAssignedPaymentMethodsEntity.UpdateId(obj, newId);
        }

        public async Task<IEnumerable<CompanyAssignedPaymentMethodsEntity>> InsertList(IEnumerable<CompanyAssignedPaymentMethodsEntity> obj, string entidadID)
        {
            foreach (CompanyAssignedPaymentMethodsEntity paymentMethodAsociado in obj)
            {
                var result = await InsertSingleWithCompany(paymentMethodAsociado, entidadID);
            };
            return obj;
        }

        public override async Task<CompanyAssignedPaymentMethodsEntity> UpdateSingle(CompanyAssignedPaymentMethodsEntity obj)
        {
            string sql;

            sql = $"update {Table.Name} set " +
               $" {nameof(CompanyAssignedPaymentMethodsEntity.CoreCompany.EntidadID)} = @{nameof(CompanyAssignedPaymentMethodsEntity.CoreCompany.EntidadID)}, " +
               $" {nameof(CompanyAssignedPaymentMethodsEntity.Defecto)} = @{nameof(CompanyAssignedPaymentMethodsEntity.Defecto)}, " +
               $" {nameof(CompanyAssignedPaymentMethodsEntity.CoreMetodosPagos.MetodoPagoID)} = @{nameof(CompanyAssignedPaymentMethodsEntity.CoreMetodosPagos.MetodoPagoID)} " +
               $" where {nameof(CompanyAssignedPaymentMethodsEntity.EntidadMetodoPagoID)} = @{nameof(CompanyAssignedPaymentMethodsEntity.EntidadMetodoPagoID)} ";


            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                EntidadID = obj.CoreCompany.EntidadID,
                Defecto = obj.Defecto,
                MetodoPagoID = obj.CoreMetodosPagos.MetodoPagoID,
                EntidadMetodoPagoID = obj.EntidadMetodoPagoID
            }, sqlTran));

            return obj;
        }
        public async Task<CompanyAssignedPaymentMethodsEntity> UpdateSingleWithList(CompanyAssignedPaymentMethodsEntity obj, int companyMetodoPagoID)
        {
            string sql;

            sql = $"update {Table.Name} set " +
               $" {nameof(CompanyAssignedPaymentMethodsEntity.CoreCompany.EntidadID)} = @{nameof(CompanyAssignedPaymentMethodsEntity.CoreCompany.EntidadID)}, " +
               $" {nameof(CompanyAssignedPaymentMethodsEntity.Defecto)} = @{nameof(CompanyAssignedPaymentMethodsEntity.Defecto)}, " +
               $" {nameof(CompanyAssignedPaymentMethodsEntity.CoreMetodosPagos.MetodoPagoID)} = @{nameof(CompanyAssignedPaymentMethodsEntity.CoreMetodosPagos.MetodoPagoID)} " +
               $" where {nameof(CompanyAssignedPaymentMethodsEntity.EntidadMetodoPagoID)} = @{nameof(CompanyAssignedPaymentMethodsEntity.EntidadMetodoPagoID)} ";


            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                EntidadID = obj.CoreCompany.EntidadID,
                Defecto = obj.Defecto,
                MetodoPagoID = obj.CoreMetodosPagos.MetodoPagoID,
                EntidadMetodoPagoID = companyMetodoPagoID
            }, sqlTran));

            return obj;
        }

        public override async Task<int> Delete(CompanyAssignedPaymentMethodsEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.EntidadMetodoPagoID
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

        public override async Task<CompanyAssignedPaymentMethodsEntity> GetItemByCode(string Metodopago, int EntidadID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} CP " +
                $" inner join {tablePaymentMethods.Name} p on p.{tablePaymentMethods.ID} = CP.{tablePaymentMethods.ID}" +
                 $" inner join {tableCompany.Name} E on E.{tableCompany.ID} = CP.{tableCompany.ID}" +
                $" where p.{tablePaymentMethods.Code} = @metodopago" +
                $" and CP.{tableCompany.ID}=@entidadID";

            var result = await connection.QueryAsync<CompanyAssignedPaymentMethodsEntity, PaymentMethodsEntity, CompanyEntity, CompanyAssignedPaymentMethodsEntity>(sql,
                (companyAssignedPaymentMethodsEntity, paymentMethodsEntity, companyEntity) =>
                {
                    companyAssignedPaymentMethodsEntity.CoreMetodosPagos = paymentMethodsEntity;
                    companyAssignedPaymentMethodsEntity.CoreCompany = companyEntity;
                    return companyAssignedPaymentMethodsEntity;

                }, new
                {

                    metodopago = Metodopago,
                    entidadID = EntidadID
                }, sqlTran, true, splitOn: $"{tablePaymentMethods.ID},{tableCompany.ID}");
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<CompanyAssignedPaymentMethodsEntity>> GetListbyCompany(int EntidadID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql2 = $"select * from {Table.Name} C " +
                $"inner join {tablePaymentMethods.Name} p on p.{tablePaymentMethods.ID} = C.{tablePaymentMethods.ID} " +
                $"inner join {tableCompany.Name} E on E.{tableCompany.ID} = C.{tableCompany.ID}" +
                $" where C.{tableCompany.ID} = @entidadID";

            var result = await connection.QueryAsync<CompanyAssignedPaymentMethodsEntity, PaymentMethodsEntity, CompanyEntity, CompanyAssignedPaymentMethodsEntity>(sql2,
                (CompanypaymentMethodEntity, PaymentMethods, Company) =>
                {
                    CompanypaymentMethodEntity.CoreMetodosPagos = PaymentMethods;
                    CompanypaymentMethodEntity.CoreCompany = Company;
                    return CompanypaymentMethodEntity;
                },
                new
                {
                    entidadID = EntidadID
                }, sqlTran, true, splitOn: $"{tablePaymentMethods.ID}, {tableCompany.ID}");

            return result.ToList();
        }


        public async Task<IEnumerable<CompanyEntity>> GetListPaymentMethods(IEnumerable<CompanyEntity> result, string IDs)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql2 = $"select * from {Table.Name} C " +
                $"inner join {tablePaymentMethods.Name} p on p.{tablePaymentMethods.ID} = C.{tablePaymentMethods.ID} " +
                $"inner join {tableCompany.Name} E on E.{tableCompany.ID} = C.{tableCompany.ID}" +
                $" where C.{tableCompany.ID} in ({IDs})";
            List<CompanyAssignedPaymentMethodsEntity> listAssigned = new List<CompanyAssignedPaymentMethodsEntity>();
            var result2 = await connection.QueryAsync<CompanyAssignedPaymentMethodsEntity, PaymentMethodsEntity, CompanyEntity, CompanyAssignedPaymentMethodsEntity>(sql2,
                (CompanypaymentMethodEntity, PaymentMethods, Company) =>
                {
                    if (CompanypaymentMethodEntity != null)
                    {
                        
                        CompanypaymentMethodEntity.CoreMetodosPagos = PaymentMethods;
                        CompanypaymentMethodEntity.CoreCompany = Company;

                        
                        result.Where(pEnt => pEnt.EntidadID.Value == CompanypaymentMethodEntity.CoreCompany.EntidadID).ToList().ForEach(lk =>
                        {
                            if (lk.MetodosPagos == null)
                            {
                                listAssigned = new List<CompanyAssignedPaymentMethodsEntity>();
                                //lk.MetodosPagos = listAssigned;
                            }
                            listAssigned.Add(CompanypaymentMethodEntity);
                            lk.MetodosPagos = listAssigned;
                        });
                    }
                    
                    return CompanypaymentMethodEntity;
                },
                new { }, sqlTran, true, splitOn: $"{tablePaymentMethods.ID}, {tableCompany.ID}");

            return result.ToList();
        }

        public async Task<IEnumerable<CompanyAssignedPaymentMethodsEntity>> UpdateList(IEnumerable<CompanyAssignedPaymentMethodsEntity> obj, string entidadID)
        {
            IEnumerable<CompanyAssignedPaymentMethodsEntity> listaExiste = GetListbyCompany(int.Parse(entidadID)).Result;

            #region Remove linked Products

            foreach (CompanyAssignedPaymentMethodsEntity paymentMethod in listaExiste)
            {
                int idBorrar = compruebaExiste(obj, paymentMethod);
                if (idBorrar != 0)
                {
                    var result = await Delete(paymentMethod);
                }

            }

            #endregion

            #region Add linked Products
            if (obj != null)
            {

                var companyMetodoPagoID = 0;
                foreach (CompanyAssignedPaymentMethodsEntity paymentMethodAsociado in obj.ToList())
                {
                    companyMetodoPagoID = contieneNombre(listaExiste, paymentMethodAsociado);
                    if (companyMetodoPagoID == 0)
                    {

                        var result = await InsertSingleWithCompany(paymentMethodAsociado, entidadID);
                    }
                    else
                    {
                        var result = await UpdateSingleWithList(paymentMethodAsociado, companyMetodoPagoID);
                    }


                };
            }
            #endregion
            return obj;
        }

        private int contieneNombre(IEnumerable<CompanyAssignedPaymentMethodsEntity> listaExiste, CompanyAssignedPaymentMethodsEntity obj)
        {
            foreach (CompanyAssignedPaymentMethodsEntity paymentMethod in listaExiste)
            {
                if (paymentMethod.CoreMetodosPagos.MetodoPagoID == obj.CoreMetodosPagos.MetodoPagoID && paymentMethod.CoreCompany.EntidadID == obj.CoreCompany.EntidadID)
                {
                    return paymentMethod.EntidadMetodoPagoID.Value;
                }
            }
            return 0;
        }
        private int compruebaExiste(IEnumerable<CompanyAssignedPaymentMethodsEntity> listaExiste, CompanyAssignedPaymentMethodsEntity obj)
        {
            foreach (CompanyAssignedPaymentMethodsEntity paymentMethod in listaExiste)
            {
                if (paymentMethod.CoreMetodosPagos.MetodoPagoID == obj.CoreMetodosPagos.MetodoPagoID && paymentMethod.CoreCompany.EntidadID == obj.CoreCompany.EntidadID)
                {
                    return 0;
                }
            }

            return obj.EntidadMetodoPagoID.Value;
        }
        public async Task<CompanyAssignedPaymentMethodsEntity> GetItemByID( int CompanyPaymentAssignedMethodID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} CP " +
                $" inner join {tablePaymentMethods.Name} p on p.{tablePaymentMethods.ID} = CP.{tablePaymentMethods.ID}" +
                 $" inner join {tableCompany.Name} E on E.{tableCompany.ID} = CP.{tableCompany.ID}" +
                $" where CP.{Table.ID}=@companyPaymentAssignedMethodID";

            var result = await connection.QueryAsync<CompanyAssignedPaymentMethodsEntity, PaymentMethodsEntity, CompanyEntity, CompanyAssignedPaymentMethodsEntity>(sql,
                (companyAssignedPaymentMethodsEntity, paymentMethodsEntity, companyEntity) =>
                {
                    companyAssignedPaymentMethodsEntity.CoreMetodosPagos = paymentMethodsEntity;
                    companyAssignedPaymentMethodsEntity.CoreCompany = companyEntity;
                    return companyAssignedPaymentMethodsEntity;

                }, new
                {
                    companyPaymentAssignedMethodID = CompanyPaymentAssignedMethodID
                }, sqlTran, true, splitOn: $"{tablePaymentMethods.ID},{tableCompany.ID}");
            return result.FirstOrDefault();
        }

    }
}
