using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.Sites;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;
using TreeCore.BackEnd.Model.ValueObject;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Data.Repository.Companies;

namespace TreeCore.BackEnd.Data.Repository.Contracts
{
    public class ContractLineEntidadRepository : BaseRepository<ContractLineEntidadEntity>
    {
        public override Table Table => TableNames.ContractLineEntidades;
        public Table ContractLine => TableNames.ContractLine;
        public Table BankAcount => TableNames.BankAccount;
        public Table Currency => TableNames.Currency;
        public Table CompanyAssignedPayment => TableNames.CompanyPaymentMethods;
        public Table Company => TableNames.Company;
        private CompanyAssignedPaymentMethodsRepository companyAssignedPaymentMethodsRepository;

        public ContractLineEntidadRepository(TransactionalWrapper conexion) : base(conexion)
        {
            companyAssignedPaymentMethodsRepository = new CompanyAssignedPaymentMethodsRepository(conexion);
        }
        public override async Task<int> Delete(ContractLineEntidadEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.AlquilerDetalleEntidadID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public override async Task<ContractLineEntidadEntity> GetItemByCode(string code, int Cliente)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} p inner join {ContractLine.Name} c on p.{ContractLine.ID} = c.{ContractLine.ID}" +
                $" inner join {BankAcount.Name} pt on p.{BankAcount.ID} = pt.{BankAcount.ID}" +
                  $" inner join {Currency.Name} pt on p.{Currency.ID} = pt.{Currency.ID}" +
                    $" inner join {CompanyAssignedPayment.Name} pt on p.{CompanyAssignedPayment.ID} = pt.{CompanyAssignedPayment.ID}" +
                $" where p.{Table.Code} = @code";

            var result = await connection.QueryAsync<ContractLineEntidadEntity, ContractLineEntity, BankAccountEntity, CurrencyEntity, CompanyAssignedPaymentMethodsEntity, ContractLineEntidadEntity>(sql,
            (contractLineEntiddadEntity, contractLineEntity, bankAcount, currency, companyAsignedPaymentMethode) =>
            {

                contractLineEntiddadEntity.oAlquilerDetalle = contractLineEntity;
                contractLineEntiddadEntity.oEntidadCuentaBankaria = bankAcount;
                contractLineEntiddadEntity.oMoneda = currency;
                contractLineEntiddadEntity.oMetodoPagoEntidad = companyAsignedPaymentMethode;
                return contractLineEntiddadEntity;
            }, new { code = code }, sqlTran, true, splitOn: $"{ContractLine.ID},{BankAcount.ID},{Currency.ID},{CompanyAssignedPayment.ID}");

            return result.FirstOrDefault();
        }

        public override async Task<ContractLineEntidadEntity> InsertSingle(ContractLineEntidadEntity obj)
        {
            string sql = $"insert into {Table.Name} " +
               $"({nameof(ContractLineEntidadEntity.oAlquilerDetalle.AlquilerDetalleID)}," +
               $"{nameof(ContractLineEntidadEntity.oEntidadCuentaBankaria.EntidadCuentaBancariaID)}, " +
               $"{nameof(ContractLineEntidadEntity.oMoneda.MonedaID)}, " +
               $"{nameof(ContractLineEntidadEntity.oMetodoPagoEntidad.EntidadMetodoPagoID)}, " +
               $"{nameof(ContractLineEntidadEntity.CantidadPorcentaje)}) " +

               $" values(@{nameof(ContractLineEntidadEntity.oAlquilerDetalle.AlquilerDetalleID)}," +
               $"@{nameof(ContractLineEntidadEntity.oEntidadCuentaBankaria.EntidadCuentaBancariaID)}," +
               $"@{nameof(ContractLineEntidadEntity.oMoneda.MonedaID)}," +
               $"@{nameof(ContractLineEntidadEntity.oMetodoPagoEntidad.EntidadMetodoPagoID)}," +
               $"@{nameof(ContractLineEntidadEntity.CantidadPorcentaje)})" +

              $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                AlquilerDetalleID = obj.oAlquilerDetalle,
                //  NombreContrato = obj.Nombre,
                EntidadCuentaBancariaID = obj.oEntidadCuentaBankaria.EntidadCuentaBancariaID,
                MonedaID = obj.oMoneda.MonedaID,
                EntidadMetodoPagoID = obj.oMetodoPagoEntidad.EntidadMetodoPagoID,
                PorcentajeCantidad = obj.CantidadPorcentaje,


            }, sqlTran)).First();


            return ContractLineEntidadEntity.UpdateId(obj, newId);
        }

        public override async Task<ContractLineEntidadEntity> UpdateSingle(ContractLineEntidadEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(ContractLineEntidadEntity.oMoneda.MonedaID)} = @{nameof(ContractLineEntidadEntity.oMoneda.MonedaID)}, " +
                 $" {nameof(ContractLineEntidadEntity.oAlquilerDetalle.AlquilerDetalleID)} =   @{nameof(ContractLineEntidadEntity.oAlquilerDetalle.AlquilerDetalleID)}, " +
                $" {nameof(ContractLineEntidadEntity.oEntidadCuentaBankaria.EntidadCuentaBancariaID)} =   @{nameof(ContractLineEntidadEntity.oEntidadCuentaBankaria.EntidadCuentaBancariaID)}, " +
                $" {nameof(ContractLineEntidadEntity.oMetodoPagoEntidad.EntidadMetodoPagoID)} =   @{nameof(ContractLineEntidadEntity.oMetodoPagoEntidad.EntidadMetodoPagoID)}, " +
                $" {nameof(ContractLineEntidadEntity.CantidadPorcentaje)} =   @{nameof(ContractLineEntidadEntity.CantidadPorcentaje)}, " +
                 $" where {nameof(ContractLineEntidadEntity.AlquilerDetalleEntidadID)} = @{nameof(ContractLineEntidadEntity.AlquilerDetalleEntidadID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                PorcentajeCantidad = obj.CantidadPorcentaje,
                EntidadCuentaBancariaID = obj.oEntidadCuentaBankaria.EntidadCuentaBancariaID,
                MonedaID = obj.oMoneda.MonedaID,
                EntidadMetodoPagoID = obj.oMetodoPagoEntidad.EntidadMetodoPagoID,

            }, sqlTran));
            return obj;
        }
        public override async Task<IEnumerable<ContractLineEntidadEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            throw new System.NotImplementedException();
        }

        #region CONTRACT LINE TAXES BY CONTRACT LINE AND TAXES
        public async Task<IEnumerable<ContractLineEntidadEntity>> InsertList(IEnumerable<ContractLineEntidadEntity> obj, string alquilerDetalleID)
        {
            foreach (ContractLineEntidadEntity item in obj)
            {
                var result = await InsertSingleWithContractLine(item, alquilerDetalleID);
            };
            return obj;
        }

        public async Task<ContractLineEntidadEntity> UpdateSingleWith(ContractLineEntidadEntity obj, int contractlineentidadID)
        {
            string sql = $"update {Table.Name} set {nameof(ContractLineEntidadEntity.oMoneda.MonedaID)} = @{nameof(ContractLineEntidadEntity.oMoneda.MonedaID)}, " +
                $" {nameof(ContractLineEntidadEntity.oEntidadCuentaBankaria.EntidadCuentaBancariaID)} =   @{nameof(ContractLineEntidadEntity.oEntidadCuentaBankaria.EntidadCuentaBancariaID)}, " +
                $" {nameof(ContractLineEntidadEntity.oMetodoPagoEntidad.EntidadMetodoPagoID)} =   @{nameof(ContractLineEntidadEntity.oMetodoPagoEntidad.EntidadMetodoPagoID)}, " +
                $" {nameof(ContractLineEntidadEntity.CantidadPorcentaje)} =   @{nameof(ContractLineEntidadEntity.CantidadPorcentaje)} " +
                 $" where {nameof(ContractLineEntidadEntity.AlquilerDetalleEntidadID)} = @{nameof(ContractLineEntidadEntity.AlquilerDetalleEntidadID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                PorcentajeCantidad = obj.CantidadPorcentaje,
                EntidadCuentaBancariaID = obj.oEntidadCuentaBankaria != null ? obj.oEntidadCuentaBankaria.EntidadCuentaBancariaID : null,
                MonedaID = obj.oMoneda.MonedaID,

                EntidadMetodoPagoID = obj.oMetodoPagoEntidad.EntidadMetodoPagoID,
                CantidadPorcentaje = obj.CantidadPorcentaje,
                AlquilerDetalleEntidadID = contractlineentidadID

            }, sqlTran));
            return obj;
        }

        public async Task<ContractLineEntidadEntity> InsertSingleWithContractLine(ContractLineEntidadEntity obj, string alquilerDetalleID)
        {
            string sql = $"insert into {Table.Name} " +
               $"({nameof(ContractLineEntidadEntity.oAlquilerDetalle.AlquilerDetalleID)}," +
               $"{nameof(ContractLineEntidadEntity.oEntidadCuentaBankaria.EntidadCuentaBancariaID)}, " +
               $"{nameof(ContractLineEntidadEntity.oMoneda.MonedaID)}, " +
               $"{nameof(ContractLineEntidadEntity.oMetodoPagoEntidad.EntidadMetodoPagoID)}, " +
               $"{nameof(ContractLineEntidadEntity.CantidadPorcentaje)}) " +

               $" values(@{nameof(ContractLineEntidadEntity.oAlquilerDetalle.AlquilerDetalleID)}," +
               $"@{nameof(ContractLineEntidadEntity.oEntidadCuentaBankaria.EntidadCuentaBancariaID)}," +
               $"@{nameof(ContractLineEntidadEntity.oMoneda.MonedaID)}," +
               $"@{nameof(ContractLineEntidadEntity.oMetodoPagoEntidad.EntidadMetodoPagoID)}," +
               $"@{nameof(ContractLineEntidadEntity.CantidadPorcentaje)})" +

              $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                AlquilerDetalleID = alquilerDetalleID,
                //  NombreContrato = obj.Nombre,
                EntidadCuentaBancariaID = obj.oEntidadCuentaBankaria != null ? obj.oEntidadCuentaBankaria.EntidadCuentaBancariaID : null,
                MonedaID = obj.oMoneda.MonedaID,
                EntidadMetodoPagoID = obj.oMetodoPagoEntidad.EntidadMetodoPagoID,
                CantidadPorcentaje = obj.CantidadPorcentaje,


            }, sqlTran)).First();


            return ContractLineEntidadEntity.UpdateId(obj, newId);
        }

        public async Task<IEnumerable<ContractLineEntidadEntity>> GetListbyContractLineDetails(int AlquilerDetalleID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"select * from {Table.Name} CLE " +
                $"left join {ContractLine.Name} CL on CLE.{ContractLine.ID} = CL.{ContractLine.ID} " +
                $"left join {BankAcount.Name} BA on CLE.{BankAcount.ID} = BA.{BankAcount.ID} " +
                $"left join {Currency.Name} M on CLE.{Currency.ID} = M.{Currency.ID} " +
                $"left join {CompanyAssignedPayment.Name} CAP on CLE.{CompanyAssignedPayment.ID} = CAP.{CompanyAssignedPayment.ID} " +
                $" where CLE.{ContractLine.ID} = @AlquilerDetalleID";

            var result = await connection.QueryAsync<ContractLineEntidadEntity, ContractLineEntity, BankAccountEntity, CurrencyEntity, CompanyAssignedPaymentMethodsEntity, ContractLineEntidadEntity>(sql,
                        (contractLineEntiddadEntity, contractLineEntity, bankAcount, currency, companyAsignedPaymentMethode) =>
                        {

                            contractLineEntiddadEntity.oAlquilerDetalle = contractLineEntity;
                            contractLineEntiddadEntity.oEntidadCuentaBankaria = bankAcount;
                            contractLineEntiddadEntity.oMoneda = currency;
                            contractLineEntiddadEntity.oMetodoPagoEntidad = companyAsignedPaymentMethode;


                            return contractLineEntiddadEntity;
                        }, new { AlquilerDetalleID = AlquilerDetalleID }, sqlTran, true, splitOn: $"{ContractLine.ID},{BankAcount.ID},{Currency.ID},{CompanyAssignedPayment.ID}");


            CompanyAssignedPaymentMethodsEntity companymethod;
            foreach (ContractLineEntidadEntity contractlineentidad in result)
            {
                companymethod = await companyAssignedPaymentMethodsRepository.GetItemByID((int)contractlineentidad.oMetodoPagoEntidad.EntidadMetodoPagoID);
                if (companymethod != null)
                {
                    contractlineentidad.oMetodoPagoEntidad = companymethod;
                }
            }
            return result.ToList();
        }

        public async Task<IEnumerable<ContractLineEntity>> GetListbyContractLineDetails(IEnumerable<ContractLineEntity> result, string IDS)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"select * from {Table.Name} CLE " +
                $"left join {ContractLine.Name} CL on CLE.{ContractLine.ID} = CL.{ContractLine.ID} " +
                $"left join {BankAcount.Name} BA on CLE.{BankAcount.ID} = BA.{BankAcount.ID} " +
                $"left join {Currency.Name} M on CLE.{Currency.ID} = M.{Currency.ID} " +
                $"left join {CompanyAssignedPayment.Name} CAP on CLE.{CompanyAssignedPayment.ID} = CAP.{CompanyAssignedPayment.ID} " +
                $" where CLE.{ContractLine.ID} in ({IDS})";

            List<ContractLineEntidadEntity> listAssigned = new List<ContractLineEntidadEntity>();

            var result2 = await connection.QueryAsync<ContractLineEntidadEntity, ContractLineEntity, BankAccountEntity, CurrencyEntity, CompanyAssignedPaymentMethodsEntity, ContractLineEntidadEntity>(sql,
                        (contractLineEntiddadEntity, contractLineEntity, bankAcount, currency, companyAsignedPaymentMethode) =>
                        {

                            //listAssigned = new List<ContractLineEntidadEntity>();
                            if (contractLineEntiddadEntity != null)
                            {
                                contractLineEntiddadEntity.oAlquilerDetalle = contractLineEntity;
                                contractLineEntiddadEntity.oEntidadCuentaBankaria = bankAcount;
                                contractLineEntiddadEntity.oMoneda = currency;
                                contractLineEntiddadEntity.oMetodoPagoEntidad = companyAsignedPaymentMethode;
                                result.Where(contlineconmpanie => contlineconmpanie.AlquilerDetalleID == contractLineEntiddadEntity.oAlquilerDetalle.AlquilerDetalleID).ToList().ForEach(lk =>
                                  {
                                      if (lk.oAlquileresDetallesEntidades == null)
                                      {
                                          listAssigned = new List<ContractLineEntidadEntity>();
                                      }
                                      listAssigned.Add(contractLineEntiddadEntity);
                                      lk.oAlquileresDetallesEntidades = listAssigned;
                                  }

                                    );
                               }
                               return contractLineEntiddadEntity;
                            

                        }, new { }, sqlTran, true, splitOn: $"{ContractLine.ID},{BankAcount.ID},{Currency.ID},{CompanyAssignedPayment.ID}");

          CompanyAssignedPaymentMethodsEntity companymethod;
            foreach (ContractLineEntidadEntity contractlineentidad in result2)
            {
                companymethod = await companyAssignedPaymentMethodsRepository.GetItemByID((int)contractlineentidad.oMetodoPagoEntidad.EntidadMetodoPagoID);
                if (companymethod != null)
                {
                    contractlineentidad.oMetodoPagoEntidad = companymethod;
                }
            }
            return result.ToList();
        }


        public async Task<int> DeleteByContractLine(int AlquilerDetalleID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"delete from {Table.Name} Where {ContractLine.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = AlquilerDetalleID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }


        public async Task<IEnumerable<ContractLineEntidadEntity>> UpdateList(IEnumerable<ContractLineEntidadEntity> obj, string alquilerDetalleID)
        {
            IEnumerable<ContractLineEntidadEntity> listaExiste = GetListbyContractLineDetails(int.Parse(alquilerDetalleID)).Result;

            #region Remove linked ContractLine

            foreach (ContractLineEntidadEntity contractLineentidad in listaExiste)
            {
                int idBorrar = compruebaExiste(obj, contractLineentidad);
                if (idBorrar != 0)
                {
                    var result = await Delete(contractLineentidad);
                }

            }

            #endregion

            #region Add linked Contract Line Taxes
            if (obj != null)
            {
                var contractlineentityID = 0;
                foreach (ContractLineEntidadEntity contractLineentidad in obj.ToList())
                {
                    contractlineentityID = contieneNombre(listaExiste, contractLineentidad);
                    if (contractlineentityID == 0)
                    {

                        var result = await InsertSingleWithContractLine(contractLineentidad, alquilerDetalleID);
                    }
                    else
                    {
                        var result = await UpdateSingleWith(contractLineentidad, contractlineentityID);
                    }



                };
            }
            #endregion
            return obj;
        }

        private int contieneNombre(IEnumerable<ContractLineEntidadEntity> listaExiste, ContractLineEntidadEntity obj)
        {
            foreach (ContractLineEntidadEntity contractLineentidad in listaExiste)
            {
                if (contractLineentidad.oMetodoPagoEntidad.CoreCompany.Codigo == obj.oMetodoPagoEntidad.CoreCompany.Codigo)
                {
                    return contractLineentidad.AlquilerDetalleEntidadID.Value;
                }
            }
            return 0;
        }
        private int compruebaExiste(IEnumerable<ContractLineEntidadEntity> listaExiste, ContractLineEntidadEntity obj)
        {
            foreach (ContractLineEntidadEntity contractLineentidad in listaExiste)
            {
                if (contractLineentidad.oMetodoPagoEntidad.CoreCompany.Codigo == obj.oMetodoPagoEntidad.CoreCompany.Codigo)
                {
                    return 0;
                }
            }

            return obj.AlquilerDetalleEntidadID.Value;
        }


    }
    #endregion
}


