using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.Companies
{
    public class CompanyAddressRepository : BaseRepository<CompanyAddressEntity>
    {
        public override Table Table => TableNames.CompanyAddress;
        public Table tableCompany => TableNames.Company;

        public CompanyAddressRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<CompanyAddressEntity> InsertSingle(CompanyAddressEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(CompanyAddressEntity.Codigo)}, {nameof(CompanyAddressEntity.EntidadDireccion)}, {nameof(CompanyAddressEntity.DireccionJSON)}, " +
                $"{ nameof(CompanyAddressEntity.Defecto)})" +
                $"values (@{nameof(CompanyAddressEntity.Codigo)}, @{nameof(CompanyAddressEntity.EntidadDireccion)}, @{nameof(CompanyAddressEntity.DireccionJSON)} " +
                $" @{ nameof(CompanyAddressEntity.Defecto)});" +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                EntidadDireccion = obj.EntidadDireccion,
                DireccionJSON = obj.DireccionJSON,
                Defecto = obj.Defecto,
            }, sqlTran)).First();

            return CompanyAddressEntity.UpdateId(obj, newId);
        }
        public override async Task<CompanyAddressEntity> UpdateSingle(CompanyAddressEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(CompanyAddressEntity.Codigo)} = @{nameof(CompanyAddressEntity.Codigo)}, " +
                $" {nameof(CompanyAddressEntity.EntidadDireccion)} = @{nameof(CompanyAddressEntity.EntidadDireccion)}, " +
                $" {nameof(CompanyAddressEntity.DireccionJSON)} = @{nameof(CompanyAddressEntity.DireccionJSON)}, " +
                $" {nameof(CompanyAddressEntity.Defecto)} =  @{ nameof(CompanyAddressEntity.Defecto)} " +
                $" where {nameof(CompanyAddressEntity.EntidadDireccionID)} = @{nameof(CompanyAddressEntity.EntidadDireccionID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                EntidadDireccion = obj.EntidadDireccion,
                DireccionJSON = obj.DireccionJSON,
                Defecto = obj.Defecto,
                EntidadDireccionID = obj.EntidadDireccionID,
            }, sqlTran));
            return obj;
        }

        public override async Task<CompanyAddressEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<CompanyAddressEntity>($"select * from {Table.Name} where Codigo = @code", new
            {
                code = code,
            }, sqlTran);

        }
        public override async Task<int> Delete(CompanyAddressEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key ";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.EntidadDireccionID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }
        public async Task<IEnumerable<CompanyAddressEntity>> InsertList(IEnumerable<CompanyAddressEntity> obj, string entidadID)
        {
            foreach (CompanyAddressEntity paymentMethodAsociado in obj)
            {
                var result = await InsertSingleWithCompany(paymentMethodAsociado, entidadID);
            };
            return obj;
        }

        public async Task<CompanyAddressEntity> InsertSingleWithCompany(CompanyAddressEntity obj, string entidadID)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(CompanyAddressEntity.CoreCompany.EntidadID)}," +
                $"{nameof(CompanyAddressEntity.Codigo)}," +
                $"{nameof(CompanyAddressEntity.EntidadDireccion)}," +
                $"{nameof(CompanyAddressEntity.DireccionJSON)}," +
                $"{nameof(CompanyAddressEntity.Defecto)} )" +
                $"values (" +
                $"@{nameof(CompanyAddressEntity.CoreCompany.EntidadID)}, " +
                $"@{nameof(CompanyAddressEntity.Codigo)}, " +
                $"@{nameof(CompanyAddressEntity.EntidadDireccion)}, " +
                $"@{nameof(CompanyAddressEntity.DireccionJSON)}, " +
                $"@{nameof(CompanyAddressEntity.Defecto)}) " +
                $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                EntidadID = entidadID,
                Codigo = obj.Codigo,
                EntidadDireccion = obj.EntidadDireccion,
                DireccionJSON = obj.DireccionJSON,
                Defecto = obj.Defecto,
            }, sqlTran)).First();

            return CompanyAddressEntity.UpdateId(obj, newId);
        }

        public async Task<IEnumerable<CompanyAddressEntity>> GetListbyCompany(string EntidadID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string query = $"select * from {Table.Name} where {tableCompany.ID} = @entidadID";

            var result = await connection.QueryAsync<CompanyAddressEntity>(query, new { entidadID = EntidadID }, sqlTran);

            return result.ToList();
        }

        public async Task<IEnumerable<CompanyEntity>> GetListbyCompanyAddress(IEnumerable<CompanyEntity> result, string IDs)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string query = $"select * from {Table.Name} C " +
                  $"inner join {tableCompany.Name} E on E.{tableCompany.ID} = C.{tableCompany.ID} " +
                     $" where C.{tableCompany.ID} IN ({IDs})";
            List<CompanyAddressEntity> listAssigned = new List<CompanyAddressEntity>();

            var result2 = await connection.QueryAsync<CompanyAddressEntity, CompanyEntity, CompanyAddressEntity>(query,
                (companyAddressEntity, companyEntity) =>
                {
                    if (companyAddressEntity != null)
                    {
                        
                        companyAddressEntity.CoreCompany = companyEntity;

                        result.Where(pEnt => pEnt.EntidadID.Value == companyAddressEntity.CoreCompany.EntidadID).ToList().ForEach(lk =>
                         {
                             if (lk.Direcciones == null)
                             {
                                 listAssigned = new List<CompanyAddressEntity>();
                                 //lk.Direcciones = listAssigned;
                             }
                             listAssigned.Add(companyAddressEntity);
                             lk.Direcciones = listAssigned;
                         }
                        );
                    }
                    return companyAddressEntity;

                }, new { }, sqlTran, true, splitOn: $"{Table.ID}, {tableCompany.ID}");

            return result.ToList();
        }

        public async Task<IEnumerable<CompanyAddressEntity>> UpdateList(IEnumerable<CompanyAddressEntity> obj, string entidadID)
        {
            IEnumerable<CompanyAddressEntity> listaExiste = GetListbyCompany(entidadID).Result;

            #region Remove linked Products

            foreach (CompanyAddressEntity companyAddress in listaExiste)
            {
                int idBorrar = compruebaExiste(obj, companyAddress);
                if (idBorrar != 0)
                {
                    var result = await Delete(companyAddress);
                }

            }

            #endregion

            #region Add linked Products
            if (obj != null)
            {
                foreach (CompanyAddressEntity servicioVinculado in obj.ToList())
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

        private int contieneNombre(IEnumerable<CompanyAddressEntity> listaExiste, CompanyAddressEntity obj)
        {
            foreach (CompanyAddressEntity companyAddress in listaExiste)
            {
                if (companyAddress.Codigo == obj.Codigo)
                {
                    return companyAddress.EntidadDireccionID.Value;
                }
            }
            return 0;
        }
        private int compruebaExiste(IEnumerable<CompanyAddressEntity> listaExiste, CompanyAddressEntity obj)
        {
            foreach (CompanyAddressEntity companyAddress in listaExiste)
            {
                if (companyAddress.Codigo == obj.Codigo)
                {
                    return 0;
                }
            }

            return obj.EntidadDireccionID.Value;
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

    }
}
