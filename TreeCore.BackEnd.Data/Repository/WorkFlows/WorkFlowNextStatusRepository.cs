using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.Shared.Data.Db;
namespace TreeCore.BackEnd.Data.Repository.WorkFlows
{
    public class WorkFlowNextStatusRepository : BaseRepository<WorkFlowNextStatusEntity>
    {
        public override Table Table => TableNames.WorkFlowNextStatus;

        public Table tableWorkFlowStatus => TableNames.WorkFlowStatus;

        public WorkFlowNextStatusRepository(TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<WorkFlowNextStatusEntity> InsertSingle(WorkFlowNextStatusEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(WorkFlowNextStatusEntity.WorkFlowStatus.CoreEstadoID)}," +
                $"{nameof(WorkFlowNextStatusEntity.Defecto)}," +
                $" {nameof(WorkFlowNextStatusLinkedEntity.CoreEstadoPosibleID)} )" +
                $"values (" +
                $"@{nameof(WorkFlowNextStatusEntity.WorkFlowStatus.CoreEstadoID)}, " +
                $"@{nameof(WorkFlowNextStatusEntity.Defecto)}, " +
                $"@{nameof(WorkFlowNextStatusLinkedEntity.CoreEstadoPosibleID)} );" +
                $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                CoreEstadoID = obj.WorkFlowStatus.CoreEstadoID,
                Defecto = obj.Defecto,
                CoreEstadoPosibleID = obj.WorkFlowNextStatus.CoreEstadoID,
            }, sqlTran)).First();

            return WorkFlowNextStatusEntity.UpdateId(obj, newId);
        }
        public async Task<WorkFlowNextStatusEntity> InsertSingleWithWorkFlowStatus(WorkFlowNextStatusEntity obj, IEnumerable<WorkFlowStatusEntity> listStatus, int estadoID)
        {
            int id=0;
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(WorkFlowNextStatusEntity.WorkFlowStatus.CoreEstadoID)}," +
                $"{nameof(WorkFlowNextStatusEntity.Defecto)}," +
                $" {nameof(WorkFlowNextStatusLinkedEntity.CoreEstadoPosibleID)} )" +
                $"values (" +
                $"@{nameof(WorkFlowNextStatusEntity.WorkFlowStatus.CoreEstadoID)}, " +
                $"@{nameof(WorkFlowNextStatusEntity.Defecto)}, " +
                $"@{nameof(WorkFlowNextStatusLinkedEntity.CoreEstadoPosibleID)} );" +
                $"SELECT SCOPE_IDENTITY();";

            foreach(WorkFlowStatusEntity item in listStatus)
            {
                if(obj.WorkFlowNextStatus.Codigo == item.Codigo)
                {
                    id = item.CoreEstadoID.Value;
                }
            }

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                CoreEstadoID = estadoID,
                Defecto = obj.Defecto,
                CoreEstadoPosibleID = id,
            }, sqlTran)).First();

            return WorkFlowNextStatusEntity.UpdateId(obj, newId);
        }

        public async Task<IEnumerable<WorkFlowNextStatusEntity>> InsertList(IEnumerable<WorkFlowNextStatusEntity> obj, IEnumerable<WorkFlowStatusEntity> listStatus, int estadoID)
        {
            var result2 = await DeleteByWorkFlowStatus(estadoID);
            foreach (WorkFlowNextStatusEntity nextStatusAsociado in obj)
            {
                var result = await InsertSingleWithWorkFlowStatus(nextStatusAsociado,listStatus, estadoID);
            };
            return obj;
        }

        public override async Task<WorkFlowNextStatusEntity> UpdateSingle(WorkFlowNextStatusEntity obj)
        {
            string sql;

            sql = $"update {Table.Name} set " +
               $" {nameof(WorkFlowNextStatusEntity.WorkFlowStatus.CoreEstadoID)} = @{nameof(WorkFlowNextStatusEntity.WorkFlowStatus.CoreEstadoID)}, " +
               $" {nameof(WorkFlowNextStatusEntity.Defecto)} = @{nameof(WorkFlowNextStatusEntity.Defecto)}, " +
               $" {nameof(WorkFlowNextStatusEntity.WorkFlowNextStatus.CoreEstadoID)} = @{nameof(WorkFlowNextStatusEntity.WorkFlowNextStatus.CoreEstadoID)} " +
               $" where {nameof(WorkFlowNextStatusEntity.CoreEstadoSiguienteID)} = @{nameof(WorkFlowNextStatusEntity.CoreEstadoSiguienteID)} ";


            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                CoreEstadoID = obj.WorkFlowStatus.CoreEstadoID,
                Defecto = obj.Defecto,
                CoreEstadoSiguienteID = obj.CoreEstadoSiguienteID
            }, sqlTran));

            return obj;
        }
        public async Task<WorkFlowNextStatusEntity> UpdateSingleWithList(WorkFlowNextStatusEntity obj, IEnumerable<WorkFlowStatusEntity> listStatus, int statusCoreEstadoID)
        {
            string sql;
            int id = 0;
            sql = $"update {Table.Name} set " +
               $" {nameof(WorkFlowNextStatusEntity.WorkFlowStatus.CoreEstadoID)} = @{nameof(WorkFlowNextStatusEntity.WorkFlowStatus.CoreEstadoID)}, " +
               $" {nameof(WorkFlowNextStatusEntity.Defecto)} = @{nameof(WorkFlowNextStatusEntity.Defecto)}, " +
               $" {nameof(WorkFlowNextStatusLinkedEntity.CoreEstadoPosibleID)} = @{nameof(WorkFlowNextStatusLinkedEntity.CoreEstadoPosibleID)} " +
               $" where {nameof(WorkFlowNextStatusEntity.CoreEstadoSiguienteID)} = @{nameof(WorkFlowNextStatusEntity.CoreEstadoSiguienteID)} ";
            foreach (WorkFlowStatusEntity item in listStatus)
            {
                if (obj.WorkFlowNextStatus.Codigo == item.Codigo)
                {
                    id = item.CoreEstadoID.Value;
                }
            }

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                CoreEstadoID = obj.WorkFlowStatus.CoreEstadoID,
                Defecto = obj.Defecto,
                CoreEstadoPosibleID = id
            }, sqlTran));

            return obj;
        }

        public override async Task<int> Delete(WorkFlowNextStatusEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreEstadoSiguienteID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public async Task<int> DeleteByWorkFlowStatus(int CoreEstadoID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {tableWorkFlowStatus.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = CoreEstadoID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public override async Task<WorkFlowNextStatusEntity> GetItemByCode(string Metodopago, int CoreEstadoID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} CP " +
                $" inner join {tableWorkFlowStatus.Name} p on p.{tableWorkFlowStatus.ID} = CP.{tableWorkFlowStatus.ID}" +
                 $" inner join {tableWorkFlowStatus.Name} E on E.{tableWorkFlowStatus.ID} = CP.{tableWorkFlowStatus.ID}" +
                $" where p.{tableWorkFlowStatus.Code} = @nextstatus" +
                $" and CP.{tableWorkFlowStatus.ID}=@estadoID";

            var result = await connection.QueryAsync<WorkFlowNextStatusEntity, WorkFlowStatusEntity, WorkFlowStatusEntity, WorkFlowNextStatusEntity>(sql,
                (statusAssignedPaymentMethodsEntity, workFlowNextStatusEntity, statusEntity) =>
                {
                    statusAssignedPaymentMethodsEntity.WorkFlowNextStatus = workFlowNextStatusEntity;
                    statusAssignedPaymentMethodsEntity.WorkFlowStatus = statusEntity;
                    return statusAssignedPaymentMethodsEntity;

                }, new
                {

                    nextstatus = Metodopago,
                    estadoID = CoreEstadoID
                }, sqlTran, true, splitOn: $"{tableWorkFlowStatus.ID}");
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<WorkFlowNextStatusEntity>> GetListbyWorkFlowStatus(int CoreEstadoID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql2 = $"select * from {tableWorkFlowStatus.Name} W " +
                $"inner join {Table.Name} E on W.{nameof(WorkFlowStatusEntity.CoreEstadoID)} = E.{nameof(WorkFlowStatusEntity.CoreEstadoID)} " +
                $"inner join {tableWorkFlowStatus.Name} NE on E.{nameof(WorkFlowNextStatusLinkedEntity.CoreEstadoPosibleID)} = NE.{nameof(WorkFlowNextStatusLinkedEntity.CoreEstadoID)} " +
                $" where W.{tableWorkFlowStatus.ID} = @estadoID";

            var result = await connection.QueryAsync<WorkFlowNextStatusEntity, WorkFlowStatusEntity, WorkFlowNextStatusEntity>(sql2,
                (WorkFlowStatusnextStatusEntity, WorkFlowStatus) =>
                {
                    WorkFlowStatusnextStatusEntity.WorkFlowNextStatus = WorkFlowStatus;
                    WorkFlowStatusnextStatusEntity.WorkFlowStatus = WorkFlowStatus;
                    return WorkFlowStatusnextStatusEntity;
                },
                new
                {
                    estadoID = CoreEstadoID
                }, sqlTran, true, splitOn: $"{tableWorkFlowStatus.ID}, {tableWorkFlowStatus.ID}");

            return result.ToList();
        }
        
        public async Task<IEnumerable<WorkFlowStatusEntity>> GetList(IEnumerable<WorkFlowStatusEntity> result, string ids)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql2 = $"select * from {tableWorkFlowStatus.Name} C " +
                $"inner join {Table.Name} E on E.{nameof(WorkFlowStatusEntity.CoreEstadoID)} = C.{nameof(WorkFlowStatusEntity.CoreEstadoID)}" +
                $"inner join {tableWorkFlowStatus.Name} E on E.{nameof(WorkFlowNextStatusLinkedEntity.CoreEstadoPosibleID)} = C.{nameof(WorkFlowNextStatusLinkedEntity.CoreEstadoID)}" +
                $" where C.{tableWorkFlowStatus.ID} in ({ids})";

            var result2 = await connection.QueryAsync<WorkFlowStatusEntity, WorkFlowNextStatusEntity, WorkFlowStatusEntity, WorkFlowStatusEntity>(sql2,
                (WorkFlowStatusStatusEntity, WorkFlowNextStatus, workflowEntityLink) =>
                {
                    result.Where(pEnt => pEnt.CoreEstadoID.Value == WorkFlowStatusStatusEntity.CoreEstadoID).ToList().ForEach(lk =>
                    {
                        if (lk.EstadosSiguientes == null)
                        {
                            lk.EstadosSiguientes = new List<WorkFlowNextStatusEntity>();
                        }
                        ((List<WorkFlowNextStatusEntity>)lk.EstadosSiguientes).Add(WorkFlowNextStatus);
                    });
                    return WorkFlowStatusStatusEntity;
                },
                new
                {
                    
                }, sqlTran, true, splitOn: $"{tableWorkFlowStatus.ID}, {tableWorkFlowStatus.ID}");

            return result.ToList();
        }

        public async Task<IEnumerable<WorkFlowNextStatusEntity>> UpdateList(IEnumerable<WorkFlowNextStatusEntity> obj, IEnumerable<WorkFlowStatusEntity> listStatus, int estadoID)
        {
            IEnumerable<WorkFlowNextStatusEntity> listaExiste = GetListbyWorkFlowStatus(estadoID).Result;

            #region Remove linked Products

            foreach (WorkFlowNextStatusEntity nextStatus in listaExiste)
            {
                int idBorrar = compruebaExiste(obj, nextStatus);
                if (idBorrar != 0)
                {
                    var result = await Delete(nextStatus);
                }

            }

            #endregion

            #region Add linked Products
            if (obj != null)
            {
                int statusCoreEstadoID;
                foreach (WorkFlowNextStatusEntity nextStatusAsociado in obj.ToList())
                {
                    statusCoreEstadoID = contieneNombre(listaExiste, nextStatusAsociado);
                    if (statusCoreEstadoID == 0)
                    {

                        var result = await InsertSingleWithWorkFlowStatus(nextStatusAsociado, listStatus, estadoID);
                    }
                };
            }
            #endregion
            return obj;
        }

        private int contieneNombre(IEnumerable<WorkFlowNextStatusEntity> listaExiste, WorkFlowNextStatusEntity obj)
        {
            foreach (WorkFlowNextStatusEntity nextStatus in listaExiste)
            {
                if (nextStatus.WorkFlowNextStatus.CoreEstadoID == obj.WorkFlowNextStatus.CoreEstadoID && nextStatus.WorkFlowStatus.CoreEstadoID == obj.WorkFlowStatus.CoreEstadoID)
                {
                    return nextStatus.CoreEstadoSiguienteID.Value;
                }
            }
            return 0;
        }
        private int compruebaExiste(IEnumerable<WorkFlowNextStatusEntity> listaExiste, WorkFlowNextStatusEntity obj)
        {
            foreach (WorkFlowNextStatusEntity nextStatus in listaExiste)
            {
                if (nextStatus.WorkFlowNextStatus.CoreEstadoID == obj.WorkFlowNextStatus.CoreEstadoID && nextStatus.WorkFlowStatus.CoreEstadoID == obj.WorkFlowStatus.CoreEstadoID)
                {
                    return 0;
                }
            }

            return obj.CoreEstadoSiguienteID.Value;
        }
        public async Task<WorkFlowNextStatusEntity> GetItemByID( int CoreEstadoSiguienteID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} CP " +
                $" inner join {tableWorkFlowStatus.Name} p on p.{tableWorkFlowStatus.ID} = CP.{tableWorkFlowStatus.ID}" +
                 $" inner join {tableWorkFlowStatus.Name} E on E.{tableWorkFlowStatus.ID} = CP.{tableWorkFlowStatus.ID}" +
                $" where CP.{Table.ID}=@CoreEstadoSiguienteID";

            var result = await connection.QueryAsync<WorkFlowNextStatusEntity, WorkFlowStatusEntity, WorkFlowStatusEntity, WorkFlowNextStatusEntity>(sql,
                (statusAssignedPaymentMethodsEntity, workFlowNextStatusEntity, statusEntity) =>
                {
                    statusAssignedPaymentMethodsEntity.WorkFlowNextStatus = workFlowNextStatusEntity;
                    statusAssignedPaymentMethodsEntity.WorkFlowStatus = statusEntity;
                    return statusAssignedPaymentMethodsEntity;

                }, new
                {
                    CoreEstadoSiguienteID = CoreEstadoSiguienteID
                }, sqlTran, true, splitOn: $"{tableWorkFlowStatus.ID},{tableWorkFlowStatus.ID}");
            return result.FirstOrDefault();
        }
        private class WorkFlowNextStatusLinkedEntity
        {
            public readonly int? CoreEstadoSiguienteID;
            public readonly int CoreEstadoID;
            public readonly int CoreEstadoPosibleID;
        }
    }

    
}

