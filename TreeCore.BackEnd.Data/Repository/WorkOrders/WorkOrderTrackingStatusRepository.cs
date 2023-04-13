using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.WorkOrders
{
    public class WorkOrderTrackingStatusRepository : BaseRepository<WorkOrderTrackingStatusEntity>
    {
        public override Table Table => TableNames.WorkOrderTrackingStatus;
        public Table TableWorkOrder => TableNames.WorkOrders;
        public Table TableUser => TableNames.User;
        public Table TableStatus => TableNames.WorkFlowStatus;


        public WorkOrderTrackingStatusRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<WorkOrderTrackingStatusEntity> InsertSingle(WorkOrderTrackingStatusEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(WorkOrderTrackingStatusEntity.Codigo)}, " +
                $"{nameof(WorkOrderTrackingStatusTable.PreviusCoreWorkOrderTrackingStatusID)}, " +
                $"{nameof(WorkOrderTrackingStatusTable.AssignedUsuarioID)}, " +
                $"{nameof(WorkOrderTrackingStatusEntity.Estado.CoreEstadoID)}, " +
                $"{nameof(WorkOrderTrackingStatusEntity.FechaCreaccion)}, " +
                $"{nameof(WorkOrderTrackingStatusTable.UsuarioCreadorID)}) " +
                $"values (" +
                $"@{nameof(WorkOrderTrackingStatusEntity.Codigo)}, " +
                $"@{nameof(WorkOrderTrackingStatusTable.PreviusCoreWorkOrderTrackingStatusID)}, " +
                $"@{nameof(WorkOrderTrackingStatusTable.AssignedUsuarioID)}, " +
                $"@{nameof(WorkOrderTrackingStatusEntity.Estado.CoreEstadoID)}, " +
                $"@{nameof(WorkOrderTrackingStatusEntity.FechaCreaccion)}, " +
                $"@{nameof(WorkOrderTrackingStatusTable.UsuarioCreadorID)}); " +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                PreviusCoreWorkOrderTrackingStatusID = obj.PreviusCoreWorkOrderTrackingStatus.CoreWorkOrderTrackingStatusID,
                AssignedUsuarioID = obj.AssignedUsuario.UsuarioID,
                CoreEstadoID = obj.Estado.CoreEstadoID,
                FechaCreaccion = obj.FechaCreaccion,
                UsuarioCreadorID = obj.UsuarioCreador.UsuarioID
            }, sqlTran)).First();
            

            return WorkOrderTrackingStatusEntity.UpdateId(obj, newId);
        }

        public async Task<WorkOrderTrackingStatusEntity> InsertSingleWithWorkorder(WorkOrderTrackingStatusEntity obj, string sWorkorderID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"insert into {Table.Name} (" +
                $"{nameof(WorkOrderTrackingStatusEntity.Codigo)}, " +
                $"{nameof(WorkOrderTrackingStatusTable.PreviusCoreWorkOrderTrackingStatusID)}, " +
                $"{nameof(WorkOrderTrackingStatusTable.AssignedUsuarioID)}, " +
                $"{nameof(WorkOrderTrackingStatusEntity.Estado.CoreEstadoID)}, " +
                $"{nameof(WorkOrderTrackingStatusEntity.FechaCreaccion)}, " +
                $"{nameof(WorkOrderTrackingStatusTable.UsuarioCreadorID)}, " +
                $"{nameof(WorkOrderTrackingStatusEntity.WorkOrders.CoreWorkOrderID)}) " +
                $"values (" +
                $"@{nameof(WorkOrderTrackingStatusEntity.Codigo)}, " +
                $"@{nameof(WorkOrderTrackingStatusTable.PreviusCoreWorkOrderTrackingStatusID)}, " +
                $"@{nameof(WorkOrderTrackingStatusTable.AssignedUsuarioID)}, " +
                $"@{nameof(WorkOrderTrackingStatusEntity.Estado.CoreEstadoID)}, " +
                $"@{nameof(WorkOrderTrackingStatusEntity.FechaCreaccion)}, " +
                $"@{nameof(WorkOrderTrackingStatusTable.UsuarioCreadorID)}, " +
                $"@{nameof(WorkOrderTrackingStatusEntity.WorkOrders.CoreWorkOrderID)}); " +
                 $"SELECT SCOPE_IDENTITY();";

            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                PreviusCoreWorkOrderTrackingStatusID = obj.PreviusCoreWorkOrderTrackingStatus.CoreWorkOrderTrackingStatusID,
                AssignedUsuarioID = obj.AssignedUsuario.UsuarioID,
                CoreEstadoID = obj.Estado.CoreEstadoID,
                FechaCreaccion = obj.FechaCreaccion,
                UsuarioCreadorID = obj.UsuarioCreador.UsuarioID,
                CoreWorkOrderID = sWorkorderID
            }, sqlTran)).First();

            //return WorkOrderTrackingStatusEntity.UpdateId(obj, newId);
            return null;
        }

        public override async Task<WorkOrderTrackingStatusEntity> UpdateSingle(WorkOrderTrackingStatusEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"update {Table.Name} set " +
                $" {nameof(WorkOrderTrackingStatusTable.AssignedUsuarioID)} =  @{nameof(WorkOrderTrackingStatusTable.AssignedUsuarioID)} " +
                $" where {nameof(WorkOrderTrackingStatusEntity.CoreWorkOrderTrackingStatusID)} = @{nameof(WorkOrderTrackingStatusEntity.CoreWorkOrderTrackingStatusID)} ";

            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                AssignedUsuarioID = obj.AssignedUsuario.UsuarioID
            }, sqlTran));


            return obj;
        }

        public override async Task<WorkOrderTrackingStatusEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} p " +
                $" inner join {TableWorkOrder.Name} pt on p.{TableWorkOrder.ID} = pt.{TableWorkOrder.ID}, " +
                $" inner join {Table.Name} ts on p.{Table.ID} = ts.{nameof(WorkOrderTrackingStatusTable.PreviusCoreWorkOrderTrackingStatusID)}, " +
                $" inner join {TableUser.Name} au on p.{nameof(WorkOrderTrackingStatusTable.AssignedUsuarioID)} = au.{TableUser.ID}, " +
                $" inner join {TableUser.Name} cu on p.{nameof(WorkOrderTrackingStatusTable.UsuarioCreadorID)} = cu.{TableUser.ID}, " +
                $" where p.{Table.Code} = @code";

            var result = await connection.QueryAsync<WorkOrderTrackingStatusEntity, WorkOrderEntity, WorkOrderTrackingStatusEntity,UserEntity,UserEntity, WorkOrderTrackingStatusEntity>(sql,
            (workOrderTrackingStatus, workOrderEntity, previewTracking, assignedUser, usercreate) =>
            {
                workOrderTrackingStatus.WorkOrders = workOrderEntity;
                workOrderTrackingStatus.PreviusCoreWorkOrderTrackingStatus = previewTracking;
                workOrderTrackingStatus.AssignedUsuario = assignedUser;
                workOrderTrackingStatus.UsuarioCreador = usercreate;
                return workOrderTrackingStatus;
            }, new { code = code }, sqlTran, true, splitOn: $"{Table.ID},{TableWorkOrder.ID},{Table.ID},{TableUser.ID},{TableUser.ID}");

            return result.FirstOrDefault();
        }

        public async Task<int?> GetItemIDByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} p " +
                $" where p.{Table.Code} = @code";

            var result = await connection.QueryAsync<WorkOrderTrackingStatusEntity>(sql, sqlTran);

            return result.FirstOrDefault().CoreWorkOrderTrackingStatusID;

        }

        public override async Task<IEnumerable<WorkOrderTrackingStatusEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;

            var sqlRelation = $"p inner join {TableWorkOrder.Name} pt on p.{TableWorkOrder.ID} = pt.{TableWorkOrder.ID}, " +
                $" inner join {Table.Name} ts on p.{Table.ID} = ts.{nameof(WorkOrderTrackingStatusTable.PreviusCoreWorkOrderTrackingStatusID)}, " +
                $" inner join {TableUser.Name} au on p.{nameof(WorkOrderTrackingStatusTable.AssignedUsuarioID)} = au.{TableUser.ID}, " +
                $" inner join {TableUser.Name} cu on p.{nameof(WorkOrderTrackingStatusTable.UsuarioCreadorID)} = cu.{TableUser.ID} ";

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

            string sqlOrder = "";
            if (orders != null && orders.Count > 0)
            {
                foreach (string column in orders)
                {
                    sqlOrder += $"{(string.IsNullOrEmpty(sqlOrder) ? " order by " : ", ")} {column}";
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
           

            var result = await connection.QueryAsync<WorkOrderTrackingStatusEntity, WorkOrderEntity, WorkOrderTrackingStatusEntity, UserEntity, UserEntity, WorkOrderTrackingStatusEntity>(query,
            (workOrderTrackingStatus, workOrderEntity, previewTracking, assignedUser, usercreate) =>
            {
                workOrderTrackingStatus.WorkOrders = workOrderEntity;
                workOrderTrackingStatus.PreviusCoreWorkOrderTrackingStatus = previewTracking;
                workOrderTrackingStatus.AssignedUsuario = assignedUser;
                workOrderTrackingStatus.UsuarioCreador = usercreate;
                return workOrderTrackingStatus;
            }, dicParam, sqlTran, true, splitOn: $"{Table.ID},{TableWorkOrder.ID},{Table.ID},{TableUser.ID},{TableUser.ID}");


            return result.ToList();
        }

        public override async Task<int> Delete(WorkOrderTrackingStatusEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreWorkOrderTrackingStatusID,
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public async Task<int> DeleteByWorkorder(int WorkorderID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"delete from {Table.Name} Where {TableWorkOrder.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = WorkorderID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public async Task<IEnumerable<WorkOrderTrackingStatusEntity>> InsertList(IEnumerable<WorkOrderTrackingStatusEntity> obj, string workorderid)
        {
            foreach (WorkOrderTrackingStatusEntity trackingStatus in obj)
            {
                WorkOrderTrackingStatusEntity result = await InsertSingleWithWorkorder(trackingStatus, workorderid);
                trackingStatus.CoreWorkOrderTrackingStatusID = result.CoreWorkOrderTrackingStatusID;
            };

            return obj;
        }

        public async Task<IEnumerable<WorkOrderTrackingStatusEntity>> GetListByWorkorder(int workorderid)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();


            var sql = $"select * from {Table.Name} p " +
                $" inner join {TableWorkOrder.Name} pt on p.{TableWorkOrder.ID} = pt.{TableWorkOrder.ID}, " +
                $" inner join {Table.Name} ts on p.{Table.ID} = ts.{nameof(WorkOrderTrackingStatusTable.PreviusCoreWorkOrderTrackingStatusID)}, " +
                $" inner join {TableUser.Name} au on p.{nameof(WorkOrderTrackingStatusTable.AssignedUsuarioID)} = au.{TableUser.ID}, " +
                $" inner join {TableUser.Name} cu on p.{nameof(WorkOrderTrackingStatusTable.UsuarioCreadorID)} = cu.{TableUser.ID}, " +
                $" where p.{TableWorkOrder.Code} = @code";

            var result = await connection.QueryAsync<WorkOrderTrackingStatusEntity, WorkOrderEntity, WorkOrderTrackingStatusEntity, UserEntity, UserEntity, WorkOrderTrackingStatusEntity>(sql,
           (workOrderTrackingStatus, workOrderEntity, previewTracking, assignedUser, usercreate) =>
           {
               workOrderTrackingStatus.WorkOrders = workOrderEntity;
               workOrderTrackingStatus.PreviusCoreWorkOrderTrackingStatus = previewTracking;
               workOrderTrackingStatus.AssignedUsuario = assignedUser;
               workOrderTrackingStatus.UsuarioCreador = usercreate;
               return workOrderTrackingStatus;
           }, new { code = workorderid }, sqlTran, true, splitOn: $"{Table.ID},{TableWorkOrder.ID},{Table.ID},{TableUser.ID},{TableUser.ID}");


            return result.ToList();
        }

        public async Task<IEnumerable<WorkOrderTrackingStatusEntity>> UpdateList(IEnumerable<WorkOrderTrackingStatusEntity> obj, string workorderid)
        {
            IEnumerable<WorkOrderTrackingStatusEntity> listaExiste = await GetListByWorkorder(int.Parse(workorderid));

            #region REMOVE LINKED STATUS

            foreach (WorkOrderTrackingStatusEntity trackingStatus in listaExiste)
            {
                int idBorrar = compruebaExiste(obj, trackingStatus);
                if (idBorrar != 0)
                {
                    var result = await Delete(trackingStatus);
                }
            }

            #endregion

            #region ADD LINKED STATUS

            if (obj != null)
            {

                foreach (WorkOrderTrackingStatusEntity trackingStatus in obj.ToList())
                {
                    trackingStatus.CoreWorkOrderTrackingStatusID = contieneNombre(listaExiste, trackingStatus);

                    if (trackingStatus.CoreWorkOrderTrackingStatusID == 0)
                    {
                        var result = await InsertSingleWithWorkorder(trackingStatus, workorderid);
                        trackingStatus.CoreWorkOrderTrackingStatusID = result.CoreWorkOrderTrackingStatusID;
                    }
                    else 
                    {
                        var result = await UpdateSingle(trackingStatus);
                    }
                };
            }
            #endregion

            return obj;
        }

        private int compruebaExiste(IEnumerable<WorkOrderTrackingStatusEntity> listaExiste, WorkOrderTrackingStatusEntity obj)
        {
            foreach (WorkOrderTrackingStatusEntity trackingStatus in listaExiste)
            {
                if (trackingStatus.Codigo == obj.Codigo)
                {
                    return 0;
                }
            }

            return obj.CoreWorkOrderTrackingStatusID.Value;
        }

        private int contieneNombre(IEnumerable<WorkOrderTrackingStatusEntity> listaExiste, WorkOrderTrackingStatusEntity obj)
        {
            foreach (WorkOrderTrackingStatusEntity trackingStatus in listaExiste)
            {
                if (trackingStatus.Codigo == obj.Codigo)
                {
                    return trackingStatus.CoreWorkOrderTrackingStatusID.Value;
                }
            }
            return 0;
        }

        private class WorkOrderTrackingStatusTable
        {
            public readonly int PreviusCoreWorkOrderTrackingStatusID;
            public readonly int AssignedUsuarioID;
            public readonly int UsuarioCreadorID;
        }
    }
}
