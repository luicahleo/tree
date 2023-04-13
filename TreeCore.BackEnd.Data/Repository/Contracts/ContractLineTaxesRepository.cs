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

namespace TreeCore.BackEnd.Data.Repository.Contracts
{
    public class ContractLineTaxesRepository : BaseRepository<ContractLineTaxesEntity>
    {
        public override Table Table => TableNames.ContractLineTaxes;
        public Table ContractLine => TableNames.ContractLine;
        public Table Taxes => TableNames.Taxes;

        public ContractLineTaxesRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }
        public override async Task<int> Delete(ContractLineTaxesEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.AlquilerDetalleImpuestoID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public override async Task<ContractLineTaxesEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} p inner join {ContractLine.Name} c on p.{ContractLine.ID} = c.{ContractLine.ID}" +
                $" inner join {Taxes.Name} pt on p.{Taxes.ID} = pt.{Taxes.ID}" +
                $" where p.{Table.Code} = @code";

            var result = await connection.QueryAsync<ContractLineTaxesEntity, ContractLineEntity, TaxesEntity, ContractLineTaxesEntity>(sql,
            (contractLineTaxesEntity, contractLineEntity, taxesEntity) =>
            {
                contractLineTaxesEntity.oImpuesto = taxesEntity;
                contractLineTaxesEntity.oAlquilerDetalle = contractLineEntity;
                return contractLineTaxesEntity;
            }, new { code = code }, sqlTran, true, splitOn: $"{ContractLine.ID},{Taxes.ID}");

            return result.FirstOrDefault();
        }

        public override async Task<ContractLineTaxesEntity> InsertSingle(ContractLineTaxesEntity obj)
        {
            string sql = $"insert into {Table.Name} " +
               $"({nameof(ContractLineTaxesEntity.oAlquilerDetalle.AlquilerDetalleID)}," +
               $"{nameof(ContractLineTaxesEntity.oImpuesto.ImpuestoID)}, " +
               $"{nameof(ContractLineTaxesEntity.Cantidad)}) " +

               $"values(@{nameof(ContractLineTaxesEntity.oAlquilerDetalle.AlquilerDetalleID)}," +
               $"@{nameof(ContractLineTaxesEntity.oImpuesto.ImpuestoID)}," +
               $"@{nameof(ContractLineTaxesEntity.Cantidad)})" +

              $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                AlquilerDetalleID = obj.oAlquilerDetalle,
                //  NombreContrato = obj.Nombre,
                ImpuestoID = obj.oImpuesto.ImpuestoID,
                Cantidad = obj.Cantidad,


            }, sqlTran)).First();


            return ContractLineTaxesEntity.UpdateId(obj, newId);
        }

        public override async Task<ContractLineTaxesEntity> UpdateSingle(ContractLineTaxesEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(ContractLineTaxesEntity.oImpuesto.ImpuestoID)} = @{nameof(ContractLineTaxesEntity.oImpuesto.ImpuestoID)}, " +
                 $" {nameof(ContractLineTaxesEntity.oAlquilerDetalle.AlquilerDetalleID)} =   @{nameof(ContractLineTaxesEntity.oAlquilerDetalle.AlquilerDetalleID)}, " +
                $" {nameof(ContractLineTaxesEntity.Cantidad)} =   @{nameof(ContractLineTaxesEntity.Cantidad)} " +
                 $" where {nameof(ContractLineTaxesEntity.AlquilerDetalleImpuestoID)} = @{nameof(ContractLineTaxesEntity.AlquilerDetalleImpuestoID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Cantidad = obj.Cantidad,
                AlquilerDetalleID = obj.oAlquilerDetalle.AlquilerDetalleID,
                oImpuesto = obj.oImpuesto,

            }, sqlTran));
            return obj;
        }
        public override async Task<IEnumerable<ContractLineTaxesEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            throw new System.NotImplementedException();
        }

        #region CONTRACT LINE TAXES BY CONTRACT LINE AND TAXES
        public async Task<IEnumerable<ContractLineTaxesEntity>> InsertList(IEnumerable<ContractLineTaxesEntity> obj, string alquilerDetalleID)
        {
            foreach (ContractLineTaxesEntity item in obj)
            {
                var result = await InsertSingleWithContractLine(item, alquilerDetalleID);
            };
            return obj;
        }

        public async Task<ContractLineTaxesEntity> InsertSingleWithContractLine(ContractLineTaxesEntity obj, string alquilerDetalleID)
        {
            string sql = $"insert into {Table.Name} " +
               $"({nameof(ContractLineTaxesEntity.oAlquilerDetalle.AlquilerDetalleID)}," +
               $"{nameof(ContractLineTaxesEntity.oImpuesto.ImpuestoID)}, " +
               $"{nameof(ContractLineTaxesEntity.Cantidad)}) " +

               $"values(@{nameof(ContractLineTaxesEntity.oAlquilerDetalle.AlquilerDetalleID)}," +
               $"@{nameof(ContractLineTaxesEntity.oImpuesto.ImpuestoID)}," +
               $"@{nameof(ContractLineTaxesEntity.Cantidad)})" +

              $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                AlquilerDetalleID = alquilerDetalleID,
                //  NombreContrato = obj.Nombre,
                ImpuestoID = obj.oImpuesto.ImpuestoID,
                Cantidad = obj.Cantidad,


            }, sqlTran)).First();


            return ContractLineTaxesEntity.UpdateId(obj, newId);
        }

        public async Task<IEnumerable<ContractLineTaxesEntity>> GetListbyContractLineDetails(int AlquilerDetalleID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"select * from {Table.Name} C " +
                $"inner join {ContractLine.Name} CT on C.{ContractLine.ID} = CT.{ContractLine.ID} " +
                $"inner join {Taxes.Name} TP on C.{Taxes.ID} = TP.{Taxes.ID} " +
                $" where C.{ContractLine.ID} = @AlquilerDetalleID";


            var result = await connection.QueryAsync<ContractLineTaxesEntity, ContractLineEntity, TaxesEntity, ContractLineTaxesEntity>(sql,
                (contractlineTaxesEntity, contractLineEntity, taxesEntity) =>
                {
                    contractlineTaxesEntity.oAlquilerDetalle = contractLineEntity;


                    contractlineTaxesEntity.oImpuesto = taxesEntity;

                    return contractlineTaxesEntity;
                },
                 new { AlquilerDetalleID = AlquilerDetalleID }, sqlTran, true, splitOn: $"{ContractLine.ID},{Taxes.ID}");

            return result.ToList();
        }

        public async Task<IEnumerable<ContractLineEntity>> GetListbyContractLineDetails(IEnumerable<ContractLineEntity> result, string IDS)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"select * from {Table.Name} C " +
                $"inner join {ContractLine.Name} CT on C.{ContractLine.ID} = CT.{ContractLine.ID} " +
                $"inner join {Taxes.Name} TP on C.{Taxes.ID} = TP.{Taxes.ID} " +
                $" where C.{ContractLine.ID} in ({IDS})";

            List<ContractLineTaxesEntity> listAssigned = new List<ContractLineTaxesEntity>();
            var result2 = await connection.QueryAsync<ContractLineTaxesEntity, ContractLineEntity, TaxesEntity, ContractLineTaxesEntity>(sql,
                (contractlineTaxesEntity, contractLineEntity, taxesEntity) =>
                {
                    //listAssigned = new List<ContractLineTaxesEntity>();
                    if (contractlineTaxesEntity != null)
                    {
                        contractlineTaxesEntity.oAlquilerDetalle = contractLineEntity;
                        contractlineTaxesEntity.oImpuesto = taxesEntity;
                        result.Where(contractlineTEnt => contractlineTEnt.AlquilerDetalleID == contractlineTaxesEntity.oAlquilerDetalle.AlquilerDetalleID).ToList().ForEach(lk =>
                        {
                             if (lk.oAlquileresDetallesImpuestos == null)
                             {
                                listAssigned = new List<ContractLineTaxesEntity>();
                            }
                             listAssigned.Add(contractlineTaxesEntity);
                             lk.oAlquileresDetallesImpuestos = listAssigned;
                         }
                         );                         
                      }
                    return contractlineTaxesEntity;
                  },
                 new {  }, sqlTran, true, splitOn: $"{ContractLine.ID},{Taxes.ID}");

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


        public async Task<IEnumerable<ContractLineTaxesEntity>> UpdateList(IEnumerable<ContractLineTaxesEntity> obj, string alquilerDetalleID)
        {
            IEnumerable<ContractLineTaxesEntity> listaExiste = GetListbyContractLineDetails(int.Parse(alquilerDetalleID)).Result;

            #region Remove linked ContractLine

            foreach (ContractLineTaxesEntity contractLinetaxe in listaExiste)
            {
                int idBorrar = compruebaExiste(obj, contractLinetaxe);
                if (idBorrar != 0)
                {
                    var result = await Delete(contractLinetaxe);
                }

            }

            #endregion

            #region Add linked Contract Line Taxes
            if (obj != null)
            {
                foreach (ContractLineTaxesEntity contractlinetaxe in obj.ToList())
                {
                    if (contieneNombre(listaExiste, contractlinetaxe) == 0)
                    {

                        var result = await InsertSingleWithContractLine(contractlinetaxe, alquilerDetalleID);
                    }



                };
            }
            #endregion
            return obj;
        }

        private int contieneNombre(IEnumerable<ContractLineTaxesEntity> listaExiste, ContractLineTaxesEntity obj)
        {
            foreach (ContractLineTaxesEntity contractlinetaxe in listaExiste)
            {
                if (contractlinetaxe.oImpuesto.Codigo == obj.oImpuesto.Codigo)
                {
                    return 0;
                }
            }
            return obj.AlquilerDetalleImpuestoID.Value;
        }
        private int compruebaExiste(IEnumerable<ContractLineTaxesEntity> listaExiste, ContractLineTaxesEntity obj)
        {
            foreach (ContractLineTaxesEntity contractlinetaxe in listaExiste)
            {
                if (contractlinetaxe.oImpuesto.Codigo == obj.oImpuesto.Codigo)
                {
                    return obj.AlquilerDetalleImpuestoID.Value;
                }
            }

            return 0;
        }
        #endregion
    }
}
