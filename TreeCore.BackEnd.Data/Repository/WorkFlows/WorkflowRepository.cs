using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.General;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.WorkFlows
{
    public class WorkflowRepository : BaseRepository<WorkflowEntity>
    {
        public override Table Table => TableNames.Workflow;
        public Table tableRolAssigned => TableNames.WorkflowAssignedRoles;
        public Table tableRol => TableNames.Rol;
        public Table tableRolWriting => TableNames.StatusRolWriting;
        public Table tableRolReading => TableNames.StatusRolReading;

        private WorkFlowStatusRepository workflowStatusRepository;

        public WorkflowRepository(TransactionalWrapper conexion) : base(conexion)
        {
            workflowStatusRepository = new WorkFlowStatusRepository(conexion);
        }

        public override async Task<WorkflowEntity> InsertSingle(WorkflowEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(WorkflowEntity.ClienteID)}, " +
                $"{nameof(WorkflowEntity.Codigo)}, " +
                $"{nameof(WorkflowEntity.Nombre)}, " +
                $"{nameof(WorkflowEntity.Descripcion)}, " +
                $"{nameof(WorkflowEntity.Activo)}, " +
                $"{nameof(WorkflowEntity.Publico)}) " +
                $"values (" +
                $"@{nameof(WorkflowEntity.ClienteID)}," +
                $"@{nameof(WorkflowEntity.Codigo)}, " +
                $"@{nameof(WorkflowEntity.Nombre)}, " +
                $"@{nameof(WorkflowEntity.Descripcion)}, " +
                $"@{nameof(WorkflowEntity.Activo)}, " +
                $"@{nameof(WorkflowEntity.Publico)}); " +
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
                Publico = obj.Publico
            }, sqlTran)).First();

            IEnumerable<WorkFlowStatusEntity> resultStatus = await workflowStatusRepository.InsertList(obj.WorkflowsEstados, newId.ToString());

            obj = WorkflowEntity.UpdateId(obj, newId);
            var resultAdd = await InsertRoles(obj);

            return obj;
        }

        public override async Task<WorkflowEntity> UpdateSingle(WorkflowEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            IEnumerable<WorkFlowStatusEntity> resultStatus = await workflowStatusRepository.UpdateList(obj.WorkflowsEstados, obj.CoreWorkFlowID.ToString());

            var resultDelete = await DeleteRoles(obj);
            var resultAdd = await InsertRoles(obj);

            string sql = $"update {Table.Name} set {nameof(WorkflowEntity.Codigo)} = @{nameof(WorkflowEntity.Codigo)}, " +
                $" {nameof(WorkflowEntity.Nombre)} =   @{nameof(WorkflowEntity.Nombre)}, " +
                $" {nameof(WorkflowEntity.Descripcion)} =  @{nameof(WorkflowEntity.Descripcion)}, " +
                $" {nameof(WorkflowEntity.Activo)} = @{nameof(WorkflowEntity.Activo)}, " +
                $" {nameof(WorkflowEntity.Publico)} =  @{ nameof(WorkflowEntity.Publico)} " +
                $" where {nameof(WorkflowEntity.CoreWorkFlowID)} = @{nameof(WorkflowEntity.CoreWorkFlowID)} ";

            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Publico = obj.Publico,
                CoreWorkFlowID = obj.CoreWorkFlowID
            }, sqlTran));

            #region REMOVE ASSIGNED ROLES STATUS

            if (resultStatus != null && resultStatus.Count() > 0)
            {
                foreach (WorkFlowStatusEntity status in resultStatus)
                {
                    if (obj.WorkflowsRoles == null &&
                        (status.EstadosRolesEscritura != null || status.EstadosRolesLectura != null))
                    {
                        resultDelete = await workflowStatusRepository.DeleteRoles(status);
                    }
                }
            }

            #endregion

            return obj;
        }

        public override async Task<WorkflowEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            var result = await connection.QueryAsync<WorkflowEntity>($"select * from {Table.Name} where {Table.Code} = @code" +
                $" AND {nameof(WorkflowEntity.ClienteID)} = @{nameof(WorkflowEntity.ClienteID)}", new
                {
                    code = code,
                    ClienteID = client
                }, sqlTran);

            if (result.ToList() != null && result.ToList().Count > 0)
            {
                string ids = "";
                result.ToList().ForEach(x =>
                {
                    ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.CoreWorkFlowID}";
                });

                result.FirstOrDefault().WorkflowsEstados = workflowStatusRepository.GetListByWorkflow(result.FirstOrDefault().CoreWorkFlowID.Value).Result;

                string query = $"select * from {Table.Name} WorkFlow " +
                $"left join {tableRolAssigned.Name} RolesAssigned on WorkFlow.{Table.ID} = RolesAssigned.{Table.ID} " +
                $"left join {tableRol.Name} Roles on RolesAssigned.{tableRol.ID} = Roles.{tableRol.ID} " +
                $" where WorkFlow.{nameof(WorkflowEntity.CoreWorkFlowID)} IN ({ids})";

                var result2 = await connection.QueryAsync<WorkflowEntity, WorkflowAssignedRolesEntity, RolEntity, WorkflowEntity>(query,
                    (workflowEntity, workflowAssignedRolesEntity, rolEntity) =>
                    {
                        if (workflowAssignedRolesEntity != null)
                        {
                            workflowAssignedRolesEntity.CoreWorkflows = workflowEntity;
                            workflowAssignedRolesEntity.Roles = rolEntity;

                            result.Where(pEnt => pEnt.CoreWorkFlowID.Value == workflowAssignedRolesEntity.CoreWorkflows.CoreWorkFlowID).ToList().ForEach(lk =>
                            {
                                if (lk.WorkflowsRoles == null)
                                {
                                    lk.WorkflowsRoles = new List<RolEntity>();
                                }

                                ((List<RolEntity>)lk.WorkflowsRoles).Add(rolEntity);
                            });
                        }

                        return workflowEntity;
                    }, new { }, sqlTran, true, splitOn: $"{tableRolAssigned.ID},{tableRol.ID}");
            }

            return result.FirstOrDefault();
        }

        public override async Task<IEnumerable<WorkflowEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            var result = await base.GetList(Client, filters, orders, direction, pageSize, pageIndex);

            if (result.Count() > 0)
            {
                IEnumerable<WorkFlowStatusEntity> workflowStatus;

                foreach (WorkflowEntity workflow in result)
                {
                    workflowStatus = await workflowStatusRepository.GetListByWorkflow(workflow.CoreWorkFlowID.Value);

                    if (workflowStatus != null)
                    {
                        workflow.WorkflowsEstados = workflowStatus;
                    }
                }

                string ids = "";
                result.ToList().ForEach(x =>
                {
                    ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.CoreWorkFlowID}";
                });

                string query2 = $"select * from {Table.Name} WorkFlow " +
                $"left join {tableRolAssigned.Name} RolesAssigned on WorkFlow.{Table.ID} = RolesAssigned.{Table.ID} " +
                $"left join {tableRol.Name} Roles on RolesAssigned.{tableRol.ID} = Roles.{tableRol.ID} " +
                $" where WorkFlow.{nameof(WorkflowEntity.CoreWorkFlowID)} IN ({ids})";

                var result2 = await connection.QueryAsync<WorkflowEntity, WorkflowAssignedRolesEntity, RolEntity, WorkflowEntity>(query2,
                    (workflowEntity, workflowAssignedRolesEntity, rolEntity) =>
                    {
                        if (workflowAssignedRolesEntity != null)
                        {
                            workflowAssignedRolesEntity.CoreWorkflows = workflowEntity;
                            workflowAssignedRolesEntity.Roles = rolEntity;

                            result.Where(pEnt => pEnt.CoreWorkFlowID.Value == workflowAssignedRolesEntity.CoreWorkflows.CoreWorkFlowID).ToList().ForEach(lk =>
                            {
                                if (lk.WorkflowsRoles == null)
                                {
                                    lk.WorkflowsRoles = new List<RolEntity>();
                                }

                                ((List<RolEntity>)lk.WorkflowsRoles).Add(rolEntity);
                            });
                        }

                        return workflowEntity;
                    }, new { }, sqlTran, true, splitOn: $"{tableRolAssigned.ID},{tableRol.ID}");
            }

            return result.ToList();
        }

        public override async Task<int> Delete(WorkflowEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            var resultStatus = await workflowStatusRepository.DeleteByWorkflow(obj.CoreWorkFlowID.Value);
            var resultDelete = await DeleteRoles(obj);

            string sql = $"delete from {Table.Name} Where {Table.ID} = @key" +
                $" AND {nameof(WorkflowEntity.ClienteID)} = @{nameof(WorkflowEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreWorkFlowID,
                    ClienteID = obj.ClienteID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public async Task<int> DeleteRoles(WorkflowEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"delete from {tableRolAssigned.Name} Where " +
                $"{nameof(WorkflowEntity.CoreWorkFlowID)} = @{nameof(WorkflowEntity.CoreWorkFlowID)}";

            var numReg = await connection.ExecuteAsync(sql, new
            {
                CoreWorkFlowID = obj.CoreWorkFlowID
            }, sqlTran);

            return numReg;
        }

        public async Task<int> InsertRoles(WorkflowEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var idLinkedRol = 0;

            if (obj.WorkflowsRoles != null)
            {
                foreach (RolEntity rolVinculada in obj.WorkflowsRoles.ToList())
                {
                    if (rolVinculada != null)
                    {
                        string sql = $"INSERT INTO {tableRolAssigned.Name} " +
                                $"({nameof(WorkflowEntity.CoreWorkFlowID)}, " +
                                $"{nameof(RolEntity.RolID)})" +
                                $"values (@{nameof(WorkflowEntity.CoreWorkFlowID)}, " +
                                $"@{nameof(rolVinculada.RolID)});" +
                                $"SELECT SCOPE_IDENTITY();";

                        idLinkedRol = (await connection.QueryAsync<int>(sql, new
                        {
                            CoreWorkFlowID = obj.CoreWorkFlowID,
                            RolID = rolVinculada.RolID
                        }, sqlTran)).First();
                    }
                };
            }

            return idLinkedRol;
        }

    }
}
