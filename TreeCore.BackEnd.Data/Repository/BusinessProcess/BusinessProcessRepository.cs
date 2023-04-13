using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.BusinessProcess
{
    public class BusinessProcessRepository : BaseRepository<BusinessProcessEntity>
    {
        public override Table Table => TableNames.BusinessProcess;
        public Table tableBusinessProcessType => TableNames.BusinessProcessType;
        public Table tableWorkflows => TableNames.Workflow;
        public Table tableWorkflowStatus => TableNames.WorkFlowStatus;
        public Table tableWorkflowAssigned => TableNames.BusinessProcessAssignedWorkflows;

        public BusinessProcessRepository(TransactionalWrapper conexion) : base(conexion) { }

        public override async Task<BusinessProcessEntity> InsertSingle(BusinessProcessEntity obj)
        {
            string sql = $"insert into {Table.Name} " +
                $"({nameof(BusinessProcessEntity.ClienteID)},{nameof(BusinessProcessEntity.Codigo)}, " +
                $"{nameof(BusinessProcessEntity.Nombre)}, {nameof(BusinessProcessEntity.Descripcion)}, {nameof(BusinessProcessEntity.Activo)}, " +
                $"{nameof(BusinessProcessEntity.CoreBusinessProcessTipos.CoreBusinessProcessTipoID)}, EstadoInicialID)" +
                $"values (@{nameof(BusinessProcessEntity.ClienteID)},@{nameof(BusinessProcessEntity.Codigo)}, @{nameof(BusinessProcessEntity.Nombre)}, " +
                $"@{nameof(BusinessProcessEntity.Descripcion)}, @{nameof(BusinessProcessEntity.Activo)}, " +
                $"@{nameof(BusinessProcessEntity.CoreBusinessProcessTipos.CoreBusinessProcessTipoID)}, @{nameof(BusinessProcessEntity.CoreEstados.CoreEstadoID)});" +
                $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                CoreEstadoID = (obj.CoreEstados != null) ? obj.CoreEstados.CoreEstadoID : null,
                CoreBusinessProcessTipoID = (obj.CoreBusinessProcessTipos != null) ? obj.CoreBusinessProcessTipos.CoreBusinessProcessTipoID : null
            }, sqlTran)).First();

            if (obj.WorkflowsVinculados != null)
            {
                foreach (WorkflowEntity workflowVinculado in obj.WorkflowsVinculados.ToList())
                {
                    sql = $"INSERT INTO {tableWorkflowAssigned.Name} ({nameof(BusinessProcessLinkedWorkflowsEntity.CoreBusinessProcessID)}, " +
                    $"{nameof(BusinessProcessLinkedWorkflowsEntity.CoreWorkflowID)})" +
                    $"values (@{nameof(newId)}, " +
                    $"@{nameof(BusinessProcessLinkedWorkflowsEntity.CoreWorkflowID)});" +
                    $"SELECT SCOPE_IDENTITY();";

                    var idLinkedProduct = (await connection.QueryAsync<int>(sql, new
                    {
                        newId = newId,
                        CoreWorkflowID = workflowVinculado.CoreWorkFlowID
                    }, sqlTran)).First();
                }
            }

            return BusinessProcessEntity.UpdateId(obj, newId);
        }

        public override async Task<BusinessProcessEntity> UpdateSingle(BusinessProcessEntity obj)
        {
            var resultDelete = await DeleteWorkflows(obj);
            var resultInsert = await InsertWorkflows(obj);

            string sql = $"update {Table.Name} " +
                $"set {nameof(BusinessProcessEntity.Codigo)} = @{nameof(BusinessProcessEntity.Codigo)}, " +
                $" {nameof(BusinessProcessEntity.Nombre)} =   @{nameof(BusinessProcessEntity.Nombre)}, " +
                $" {nameof(BusinessProcessEntity.Descripcion)} =  @{nameof(BusinessProcessEntity.Descripcion)}, " +
                $" {nameof(BusinessProcessEntity.Activo)} = @{nameof(BusinessProcessEntity.Activo)}, " +
                $" EstadoInicialID =  @{ nameof(BusinessProcessEntity.CoreEstados.CoreEstadoID)}, " +
                $" {nameof(BusinessProcessEntity.CoreBusinessProcessTipos.CoreBusinessProcessTipoID)} = @{nameof(BusinessProcessEntity.CoreBusinessProcessTipos.CoreBusinessProcessTipoID)} " +
                $" where {nameof(BusinessProcessEntity.CoreBusinessProcessID)} = @{nameof(BusinessProcessEntity.CoreBusinessProcessID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                CoreBusinessProcessID = obj.CoreBusinessProcessID,
                CoreEstadoID = (obj.CoreEstados != null) ? obj.CoreEstados.CoreEstadoID : null,
                CoreBusinessProcessTipoID = (obj.CoreBusinessProcessTipos != null) ? obj.CoreBusinessProcessTipos.CoreBusinessProcessTipoID : null
            }, sqlTran));
            return obj;
        }

        public override async Task<BusinessProcessEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            var sql = $"select * from {Table.Name} b " +
                $"left join {tableWorkflowStatus.Name} ws on b.EstadoInicialID = ws.{tableWorkflowStatus.ID} " + 
                $"left join {tableWorkflows.Name} w on ws.{tableWorkflows.ID} = w.{tableWorkflows.ID} " +
                $"left join {tableBusinessProcessType.Name} bt on b.{tableBusinessProcessType.ID} = bt.{tableBusinessProcessType.ID} " +
                $"where b.{Table.Code} = @code AND b.{nameof(BusinessProcessEntity.ClienteID)} = @{nameof(BusinessProcessEntity.ClienteID)}";

            var result = await connection.QueryAsync<BusinessProcessEntity, WorkFlowStatusEntity, WorkflowEntity, BusinessProcessTypeEntity, BusinessProcessEntity>(sql,
            (businessProcessEntity, workflowStatusEntity, workflowEntity, businessProcessTypeEntity) =>
            {
                businessProcessEntity.CoreBusinessProcessTipos = businessProcessTypeEntity;
                businessProcessEntity.CoreEstados = workflowStatusEntity;
                businessProcessEntity.CoreEstados.WorkFlow = workflowEntity;

                return businessProcessEntity;
            }, new { code = code, ClienteID = Client }, sqlTran, true, 
            splitOn: $"{tableWorkflowStatus.ID},{tableWorkflows.ID},{tableBusinessProcessType.ID}");

            if (result.ToList() != null && result.ToList().Count > 0)
            {
                string ids = "";
                result.ToList().ForEach(x =>
                {
                    ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.CoreBusinessProcessID}";
                });

                string query = $"SELECT * FROM {Table.Name} b " +
                    $"inner join {tableWorkflowAssigned.Name} workflowAssigned on b.{nameof(BusinessProcessEntity.CoreBusinessProcessID)} = workflowAssigned.{nameof(BusinessProcessLinkedWorkflowsEntity.CoreBusinessProcessID)} " +
                    $"inner join {tableWorkflows.Name} workflow on workflowAssigned.{nameof(BusinessProcessLinkedWorkflowsEntity.CoreWorkflowID)} = workflow.{nameof(BusinessProcessLinkedWorkflowsEntity.CoreWorkflowID)} " +
                    $"WHERE b.{nameof(BusinessProcessEntity.CoreBusinessProcessID)} IN ({ids})";

                var result2 = await connection.QueryAsync<BusinessProcessEntity, BusinessProcessLinkedWorkflowsEntity, WorkflowEntity, BusinessProcessEntity>(query,
                (businessProcessEntity, businessProcessLinkedWorkflowsEntity, workflowsEntity) => {
                    result.Where(pEnt => pEnt.CoreBusinessProcessID.Value == businessProcessLinkedWorkflowsEntity.CoreBusinessProcessID).ToList().ForEach(lk =>
                    {
                        if (lk.WorkflowsVinculados == null)
                        {
                            lk.WorkflowsVinculados = new List<WorkflowEntity>();
                        }
                        ((List<WorkflowEntity>)lk.WorkflowsVinculados).Add(workflowsEntity);
                    });

                    return businessProcessEntity;

                }, new { }, sqlTran, true, splitOn: $"{tableWorkflowAssigned.ID},{tableWorkflows.ID}");
            }

            return result.FirstOrDefault();
        }

        public override async Task<IEnumerable<BusinessProcessEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;

            string sqlRelation = $"b left join {tableWorkflowStatus.Name} ws on b.EstadoInicialID = ws.{tableWorkflowStatus.ID} " +
                $" left join {tableWorkflows.Name} w on ws.{tableWorkflows.ID} = w.{tableWorkflows.ID} " +
                $" left join {tableBusinessProcessType.Name} bt on b.{tableBusinessProcessType.ID} = bt.{tableBusinessProcessType.ID} ";

            string sqlFilter = "";
            if (filters != null && filters.Count > 0)
            {
                sqlFilter += Filter.BuilFilters(filters, Filter.Types.AND, countFilter, out countFilter, dicParam, out dicParam, "b");
            }

            string sqlOrder = "";
            if (orders != null && orders.Count > 0)
            {
                foreach (string column in orders)
                {
                    sqlOrder += $"{(string.IsNullOrEmpty(sqlOrder) ? " order by b." : ", ")} {column}";
                }
            }
            else
            {
                sqlOrder = $" order by b.{Table.Code} ";
            }

            string sqlPagination = "";
            if (pageSize != -1 && pageIndex != -1)
            {
                sqlPagination = $"OFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            }

            if (string.IsNullOrEmpty(direction) || (direction.ToLower() != "asc" && direction.ToLower() != "desc"))
            {
                direction = "ASC";
            }

            string query = $"select TotalItems = COUNT(*) OVER(), * from {Table.Name} {sqlRelation} {sqlFilter} {sqlOrder} {direction} {sqlPagination}";

            var result = await connection.QueryAsync<BusinessProcessEntity, WorkFlowStatusEntity, WorkflowEntity, BusinessProcessTypeEntity, BusinessProcessEntity>(query,
            (businessProcessEntity, workflowStatusEntity, workflowEntity, businessProcessTypeEntity) =>
            {
                businessProcessEntity.CoreBusinessProcessTipos = businessProcessTypeEntity;
                businessProcessEntity.CoreEstados = workflowStatusEntity;
                businessProcessEntity.CoreEstados.WorkFlow = workflowEntity;

                return businessProcessEntity;
            },
            dicParam, sqlTran, true, splitOn: $"{tableWorkflowStatus.ID},{tableWorkflows.ID},{tableBusinessProcessType.ID}");

            result = result.Distinct();

            if (result.ToList() != null && result.ToList().Count > 0)
            {
                string ids = "";
                result.ToList().ForEach(x =>
                {
                    ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.CoreBusinessProcessID}";
                });

                string query2 = $"SELECT * FROM {Table.Name} b " +
                    $"inner join {tableWorkflowAssigned.Name} workflowAssigned on b.{nameof(BusinessProcessEntity.CoreBusinessProcessID)} = workflowAssigned.{nameof(BusinessProcessLinkedWorkflowsEntity.CoreBusinessProcessID)} " +
                    $"inner join {tableWorkflows.Name} workflow on workflowAssigned.{nameof(BusinessProcessLinkedWorkflowsEntity.CoreWorkflowID)} = workflow.{nameof(BusinessProcessLinkedWorkflowsEntity.CoreWorkflowID)} " +
                    $"WHERE b.{nameof(BusinessProcessEntity.CoreBusinessProcessID)} IN ({ids})";

                var result2 = await connection.QueryAsync<BusinessProcessEntity, BusinessProcessLinkedWorkflowsEntity, WorkflowEntity, BusinessProcessEntity>(query2,
                (businessProcessEntity, businessProcessLinkedWorkflowsEntity, workflowsEntity) => {
                    result.Where(pEnt => pEnt.CoreBusinessProcessID.Value == businessProcessLinkedWorkflowsEntity.CoreBusinessProcessID).ToList().ForEach(lk =>
                    {
                        if (lk.WorkflowsVinculados == null)
                        {
                            lk.WorkflowsVinculados = new List<WorkflowEntity>();
                        }
                        ((List<WorkflowEntity>)lk.WorkflowsVinculados).Add(workflowsEntity);
                    });

                    return businessProcessEntity;

                }, new { }, sqlTran, true, splitOn: $"{tableWorkflowAssigned.ID},{tableWorkflows.ID}");
            }

            return result.ToList();
        }

        public override async Task<int> Delete(BusinessProcessEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"delete from {Table.Name} Where {Table.ID} = @key " +
                $"AND {nameof(BusinessProcessEntity.ClienteID)} = @{nameof(BusinessProcessEntity.ClienteID)}";
            int numReg;

            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreBusinessProcessID,
                    ClienteID = obj.ClienteID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }

            return numReg;
        }

        public async Task<int> DeleteWorkflows(BusinessProcessEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"delete from {tableWorkflowAssigned.Name} Where " +
                $"{nameof(BusinessProcessEntity.CoreBusinessProcessID)} = @{nameof(BusinessProcessEntity.CoreBusinessProcessID)}";
            int numReg;

            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    CoreBusinessProcessID = obj.CoreBusinessProcessID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }

            return numReg;
        }

        public async Task<int> InsertWorkflows(BusinessProcessEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();            
            int numReg = 0;

            try
            {
                if (obj.WorkflowsVinculados != null)
                {
                    foreach (WorkflowEntity workflowVinculada in obj.WorkflowsVinculados.ToList())
                    {
                        string sql = $"INSERT INTO {tableWorkflowAssigned.Name} ({nameof(BusinessProcessLinkedWorkflowsEntity.CoreBusinessProcessID)}, " +
                                $"{nameof(BusinessProcessLinkedWorkflowsEntity.CoreWorkflowID)})" +
                                $"values (@{nameof(BusinessProcessLinkedWorkflowsEntity.CoreBusinessProcessID)}, " +
                                $"@{nameof(workflowVinculada.CoreWorkFlowID)});" +
                                $"SELECT SCOPE_IDENTITY();";

                        numReg = (await connection.QueryAsync<int>(sql, new
                        {
                            CoreBusinessProcessID = obj.CoreBusinessProcessID,
                            CoreWorkflowID = workflowVinculada.CoreWorkFlowID
                        }, sqlTran)).First();
                    }
                }
                
            }
            catch (System.Exception)
            {
                numReg = 0;
            }

            return numReg;
        }

        private class BusinessProcessLinkedWorkflowsEntity
        {
            public readonly int? CoreBusinessProcessWorkflowID;
            public readonly int CoreBusinessProcessID;
            public readonly int CoreWorkflowID;
        }
    }
}
