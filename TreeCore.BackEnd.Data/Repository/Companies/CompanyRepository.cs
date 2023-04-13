using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.General;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Data.Repository.Companies
{
    public class CompanyRepository : BaseRepository<CompanyEntity>
    {
        public override Table Table => TableNames.Company;
        public Table TableCompanyType => TableNames.CompanyType;
        public Table TableTaxpayerType => TableNames.TaxpayerType;
        public Table TableTaxIdentificationNumberCategory => TableNames.TaxIdentificationNumberCategory;
        public Table TablePaymentTerm => TableNames.PaymentTerm;
        public Table TableCurrency => TableNames.Currency;
        public Table TableBankAccount => TableNames.BankAccount;
        public Table TableBank => TableNames.Bank;
        public Table TablePaymentMethod => TableNames.PaymentMethods;

        private BankAccountRepository bankAccountRepository;

        private UserRepository userRepository;

        private CompanyAssignedPaymentMethodsRepository paymentMethodRepository;

        private CompanyAddressRepository companyAddressRepository;
        public CompanyRepository(TransactionalWrapper conexion) : base(conexion)
        {
            bankAccountRepository = new BankAccountRepository(conexion);
            paymentMethodRepository = new CompanyAssignedPaymentMethodsRepository(conexion);
            companyAddressRepository = new CompanyAddressRepository(conexion);
            userRepository = new UserRepository(conexion);
        }

        public override async Task<CompanyEntity> InsertSingle(CompanyEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                    $"{nameof(CompanyEntity.ClienteID)},{nameof(CompanyEntity.Codigo)}, {nameof(CompanyEntity.Nombre)}, " +
                    $"{nameof(CompanyEntity.Alias)}, {nameof(CompanyEntity.Email)}, {nameof(CompanyEntity.Telefono)}, {nameof(CompanyEntity.Activo)},  " +
                    $"{nameof(CompanyEntity.NumIdentContribuyente)}, {nameof(CompanyEntity.EsPropietario)},{nameof(CompanyEntity.EsProveedor)},{nameof(CompanyEntity.EsBeneficiario)},{nameof(CompanyEntity.EsOperador)}," +
                    $"{ nameof(CompanyEntity.EntidadesTipos.EntidadTipoID)},{ nameof(CompanyEntity.EntidadCliente)}," +
                    $"{ nameof(CompanyEntity.TiposContribuyentes.TipoContribuyenteID)},{ nameof(CompanyEntity.SAPTipoNIF.SAPTipoNIFID)}," +
                    $"{ nameof(CompanyEntity.CondicionPago.CondicionPagoID)},{ nameof(CompanyEntity.Moneda.MonedaID)}," +
                    $"UsuarioCreadorID,UsuarioModificadorID," +
                    $"{ nameof(CompanyEntity.FechaCreaccion)},{ nameof(CompanyEntity.FechaUltimaModificacion)}" +
                $")" +
                $"values (" +
                    $"@{nameof(CompanyEntity.ClienteID)},@{nameof(CompanyEntity.Codigo)}, @{nameof(CompanyEntity.Nombre)}, " +
                    $"@{nameof(CompanyEntity.Alias)},@{nameof(CompanyEntity.Email)}, @{nameof(CompanyEntity.Telefono)}," +
                    $"@{nameof(CompanyEntity.Activo)},@{nameof(CompanyEntity.NumIdentContribuyente)}, @{nameof(CompanyEntity.EsPropietario)},@{nameof(CompanyEntity.EsProveedor)},@{nameof(CompanyEntity.EsBeneficiario)},@{nameof(CompanyEntity.EsOperador)}," +
                    $"@{ nameof(CompanyEntity.EntidadesTipos.EntidadTipoID)},@{ nameof(CompanyEntity.EntidadCliente)}," +
                    $"@{ nameof(CompanyEntity.TiposContribuyentes.TipoContribuyenteID)},@{ nameof(CompanyEntity.SAPTipoNIF.SAPTipoNIFID)}," +
                    $"@{ nameof(CompanyEntity.CondicionPago.CondicionPagoID)},@{ nameof(CompanyEntity.Moneda.MonedaID)}," +
                    $"@UsuarioCreadorID,@UsuarioModificadorID," +
                    $"@{ nameof(CompanyEntity.FechaCreaccion)},@{ nameof(CompanyEntity.FechaUltimaModificacion)}" +
                $");" +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Alias = obj.Alias,
                Email = obj.Email,
                Telefono = obj.Telefono,
                Activo = obj.Activo,
                NumIdentContribuyente = obj.NumIdentContribuyente,
                EsPropietario = obj.EsPropietario,
                EsProveedor = obj.EsProveedor,
                EsBeneficiario = obj.EsBeneficiario,
                EsOperador = obj.EsOperador,
                EntidadTipoID = obj.EntidadesTipos.EntidadTipoID,
                EntidadCliente = false,
                TipoContribuyenteID = obj.TiposContribuyentes.TipoContribuyenteID,
                SAPTipoNIFID = obj.SAPTipoNIF.SAPTipoNIFID,
                CondicionPagoID = obj.CondicionPago.CondicionPagoID,
                MonedaID = obj.Moneda.MonedaID,
                UsuarioCreadorID = (obj.UsuariosCreadores != null) ? obj.UsuariosCreadores.UsuarioID : null,
                UsuarioModificadorID = (obj.UsuariosModificadores != null) ? obj.UsuariosModificadores.UsuarioID : null,
                FechaCreaccion = obj.FechaCreaccion,
                FechaUltimaModificacion = obj.FechaUltimaModificacion
            }, sqlTran)).First();

            IEnumerable<BankAccountEntity> result = await bankAccountRepository.InsertList(obj.CuentasBancarias, newId.ToString());

            IEnumerable<CompanyAssignedPaymentMethodsEntity> resultPaymentMethods = await paymentMethodRepository.InsertList(obj.MetodosPagos, newId.ToString());

            IEnumerable<CompanyAddressEntity> resultCompanyAddress = await companyAddressRepository.InsertList(obj.Direcciones, newId.ToString());

            return CompanyEntity.UpdateId(obj, newId);
        }
        public override async Task<CompanyEntity> UpdateSingle(CompanyEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            IEnumerable<BankAccountEntity> result = await bankAccountRepository.UpdateList(obj.CuentasBancarias, obj.EntidadID.ToString());
            IEnumerable<CompanyAssignedPaymentMethodsEntity> resultPaymentMethods = await paymentMethodRepository.UpdateList(obj.MetodosPagos, obj.EntidadID.ToString());
            IEnumerable<CompanyAddressEntity> resultCompanyAddress = await companyAddressRepository.UpdateList(obj.Direcciones, obj.EntidadID.ToString());

            string sql = $"update {Table.Name} set {nameof(CompanyEntity.Codigo)} = @{nameof(CompanyEntity.Codigo)}, " +
               $" {nameof(CompanyEntity.Nombre)} = @{nameof(CompanyEntity.Nombre)}, " +
               $" {nameof(CompanyEntity.Alias)} = @{nameof(CompanyEntity.Alias)}, " +
               $" {nameof(CompanyEntity.Email)} = @{nameof(CompanyEntity.Email)}, " +
               $" {nameof(CompanyEntity.Telefono)} = @{nameof(CompanyEntity.Telefono)}, " +
               $" {nameof(CompanyEntity.Activo)} = @{nameof(CompanyEntity.Activo)}, " +
               $" {nameof(CompanyEntity.EsPropietario)} = @{nameof(CompanyEntity.EsPropietario)}, " +
               $" {nameof(CompanyEntity.EsProveedor)} = @{nameof(CompanyEntity.EsProveedor)}, " +
               $" {nameof(CompanyEntity.EsBeneficiario)} = @{nameof(CompanyEntity.EsBeneficiario)}, " +
               $" {nameof(CompanyEntity.EsOperador)} = @{nameof(CompanyEntity.EsOperador)}, " +
               $" {nameof(CompanyEntity.NumIdentContribuyente)} = @{nameof(CompanyEntity.NumIdentContribuyente)}, " +
               $" {nameof(CompanyEntity.EntidadesTipos.EntidadTipoID)} =  @{ nameof(CompanyEntity.EntidadesTipos.EntidadTipoID)}, " +
               $" {nameof(CompanyEntity.TiposContribuyentes.TipoContribuyenteID)} =  @{ nameof(CompanyEntity.TiposContribuyentes.TipoContribuyenteID)}, " +
               $" {nameof(CompanyEntity.SAPTipoNIF.SAPTipoNIFID)} =  @{ nameof(CompanyEntity.SAPTipoNIF.SAPTipoNIFID)}, " +
               $" {nameof(CompanyEntity.CondicionPago.CondicionPagoID)} =  @{ nameof(CompanyEntity.CondicionPago.CondicionPagoID)}, " +
               $" {nameof(CompanyEntity.Moneda.MonedaID)} =  @{ nameof(CompanyEntity.Moneda.MonedaID)}, " +
               $" UsuarioModificadorID =  @UsuarioModificadorID," +
               $" {nameof(CompanyEntity.FechaUltimaModificacion)} =  @{ nameof(CompanyEntity.FechaUltimaModificacion)} " +
               $" where {nameof(CompanyEntity.EntidadID)} = @{nameof(CompanyEntity.EntidadID)} ";


            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Alias = obj.Alias,
                Email = obj.Email,
                Telefono = obj.Telefono,
                Activo = obj.Activo,
                NumIdentContribuyente = obj.NumIdentContribuyente,
                EsPropietario = obj.EsPropietario,
                EsProveedor = obj.EsProveedor,
                EsBeneficiario = obj.EsBeneficiario,
                EsOperador = obj.EsOperador,
                EntidadTipoID = obj.EntidadesTipos.EntidadTipoID,
                EntidadID = obj.EntidadID,
                TipoContribuyenteID = obj.TiposContribuyentes.TipoContribuyenteID,
                SAPTipoNIFID = obj.SAPTipoNIF.SAPTipoNIFID,
                CondicionPagoID = obj.CondicionPago.CondicionPagoID,
                MonedaID = obj.Moneda.MonedaID,
                UsuarioModificadorID = (obj.UsuariosModificadores != null) ? obj.UsuariosModificadores.UsuarioID : null,
                FechaUltimaModificacion = obj.FechaUltimaModificacion
            }, sqlTran));
            return obj;
        }

        public override async Task<CompanyEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"select * from {Table.Name} C " +
                $"inner join {TableCompanyType.Name} CT on C.{TableCompanyType.ID} = CT.{TableCompanyType.ID} " +
                $"left join {TableTaxpayerType.Name} TP on C.{TableTaxpayerType.ID} = TP.{TableTaxpayerType.ID} " +
                $"left join {TableTaxIdentificationNumberCategory.Name} TI on C.{TableTaxIdentificationNumberCategory.ID}= TI.{TableTaxIdentificationNumberCategory.ID} " +
                $"left join {TablePaymentTerm.Name} PT on C.{TablePaymentTerm.ID}= PT.{TablePaymentTerm.ID} " +
                $"inner join {TableCurrency.Name} CU on C.{TableCurrency.ID}= CU.{TableCurrency.ID} " +
                $" where C.{Table.Code} = @code AND C.{nameof(CompanyEntity.ClienteID)} = @{nameof(CompanyEntity.ClienteID)}";

            var result = await connection.QueryAsync<CompanyEntity, CompanyTypeEntity, TaxpayerTypeEntity, TaxIdentificationNumberCategoryEntity, PaymentTermEntity, CurrencyEntity, CompanyEntity>(sql,
                (companyEntity, companyTypeEntity, taxpayerTypeEntity, taxIdentificationNumberCategoryEntity, paymentTermEntity, CurrencyEntity) =>
                        {
                            companyEntity.EntidadesTipos = companyTypeEntity;
                            companyEntity.TiposContribuyentes = taxpayerTypeEntity;
                            companyEntity.SAPTipoNIF = taxIdentificationNumberCategoryEntity;
                            companyEntity.CondicionPago = paymentTermEntity;
                            companyEntity.Moneda = CurrencyEntity;
                            return companyEntity;
                        },
                new
                {
                    code = code,
                    ClienteID = Client
                }, sqlTran, true, splitOn: $"{TableCompanyType.ID}, {TableTaxpayerType.ID}, {TableTaxIdentificationNumberCategory.ID}, {TablePaymentTerm.ID}, {TableCurrency.ID}");
            if (result.FirstOrDefault() != null)
            {
                result.FirstOrDefault().CuentasBancarias = bankAccountRepository.GetListbyCompanyDetails(result.FirstOrDefault().EntidadID.Value).Result;
                result.FirstOrDefault().MetodosPagos = paymentMethodRepository.GetListbyCompany(result.FirstOrDefault().EntidadID.Value).Result;
                result.FirstOrDefault().UsuariosCreadores = userRepository.GetItemByCompany(result.FirstOrDefault().EntidadID.ToString(), Table, "UsuarioCreadorID").Result;
                result.FirstOrDefault().UsuariosModificadores = userRepository.GetItemByCompany(result.FirstOrDefault().EntidadID.ToString(), Table, "UsuarioModificadorID").Result;
                result.FirstOrDefault().Direcciones = companyAddressRepository.GetListbyCompany(result.FirstOrDefault().EntidadID.ToString()).Result;
            }


            return result.FirstOrDefault();

        }
        public override async Task<int> Delete(CompanyEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var resultBankAccount = await bankAccountRepository.DeleteByCompany(obj.EntidadID.Value);
            var resultPaymentMethods = await paymentMethodRepository.DeleteByCompany(obj.EntidadID.Value);
            var resultCompanyAddress = await companyAddressRepository.DeleteByCompany(obj.EntidadID.Value);

            string sql = $"delete from {Table.Name} Where {Table.ID} = @key AND {nameof(CompanyEntity.ClienteID)} = @{nameof(CompanyEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.EntidadID,
                    ClienteID = obj.ClienteID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public override async Task<IEnumerable<CompanyEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;

            string sqlRelation = $"p inner join {TableCompanyType.Name} c on p.{TableCompanyType.ID} = c.{TableCompanyType.ID} " +
                $"left join {TableTaxpayerType.Name} TT on p.{TableTaxpayerType.ID} = TT.{TableTaxpayerType.ID} " +
                $"left join {TableTaxIdentificationNumberCategory.Name} TI on p.{TableTaxIdentificationNumberCategory.ID}= TI.{TableTaxIdentificationNumberCategory.ID} " +
                $"left join {TablePaymentTerm.Name} PT on p.{TablePaymentTerm.ID}= PT.{TablePaymentTerm.ID} " +
                $"inner join {TableCurrency.Name} CU on p.{TableCurrency.ID}= CU.{TableCurrency.ID} ";

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



                sqlFilter += Filter.BuilFilters(filters, sType, countFilter, out countFilter, dicParam, out dicParam, "p");
            }
            else if (filters != null && filters.Count > 0)
            {
                sqlFilter = " WHERE ";

                string filtros = Filter.BuilFilters(filters, Filter.Types.AND, countFilter, out countFilter, dicParam, out dicParam, "p");

                if (filtros != " ")
                {
                    sqlFilter += filtros;
                }
                else
                {
                    sqlFilter = " ";
                }


            }

            if (string.IsNullOrEmpty(sqlFilter))
            {
                sqlFilter = $" WHERE p.{nameof(CompanyEntity.ClienteID)} = {Client} ";
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

            if (string.IsNullOrEmpty(direction) || (direction.ToLower() != "asc" && direction.ToLower() != "desc"))
            {
                direction = "ASC";
            }

            string sqlPagination = "";
            if (pageSize != -1 && pageIndex != -1)
            {
                sqlPagination = $"OFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            }


            string query = $"select TotalItems = COUNT(*) OVER(), * from {Table.Name} {sqlRelation} {sqlFilter} {sqlOrder} {direction} {sqlPagination}";


            var result = await connection.QueryAsync<CompanyEntity, CompanyTypeEntity, TaxpayerTypeEntity, TaxIdentificationNumberCategoryEntity, PaymentTermEntity, CurrencyEntity, CompanyEntity>(query,
                (companyEntity, companyTypeEntity, taxpayerTypeEntity, taxIdentificationNumberCategoryEntity, paymentTermEntity, currencyEntity) =>
                {
                    companyEntity.EntidadesTipos = companyTypeEntity;
                    companyEntity.TiposContribuyentes = taxpayerTypeEntity;
                    companyEntity.SAPTipoNIF = taxIdentificationNumberCategoryEntity;
                    companyEntity.CondicionPago = paymentTermEntity;
                    companyEntity.Moneda = currencyEntity;
                    return companyEntity;
                },
                 dicParam, sqlTran, true, splitOn: $"{TableCompanyType.ID}, {TableTaxpayerType.ID}, {TableTaxIdentificationNumberCategory.ID}, {TablePaymentTerm.ID}, {TableCurrency.ID}");
            if (result.Count() > 0)
            {
                IEnumerable<BankAccountEntity> bankAccounts;
                IEnumerable<CompanyAssignedPaymentMethodsEntity> paymentMethods;
                IEnumerable<CompanyAddressEntity> companyAddress;
                UserEntity userCreate;
                UserEntity userModify;
                string ids = "";
                result.ToList().ForEach(x =>
                {
                    ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.EntidadID}";

                });

                result = await bankAccountRepository.GetListBankAccount(result,ids);

                result = await paymentMethodRepository.GetListPaymentMethods(result, ids);

                result = await companyAddressRepository.GetListbyCompanyAddress(result, ids);

                foreach (CompanyEntity company in result)
                {


                    userCreate = await userRepository.GetItemByCompany(company.EntidadID.ToString(), Table, "UsuarioCreadorID");
                    userModify = await userRepository.GetItemByCompany(company.EntidadID.ToString(), Table, "UsuarioModificadorID");
                    if (userCreate != null)
                    {
                        company.UsuariosCreadores = userCreate;

                    }
                    if (userModify != null)
                    {
                        company.UsuariosModificadores = userModify;
                    }


                }
            }


            return result.ToList();
        }




    }
}
