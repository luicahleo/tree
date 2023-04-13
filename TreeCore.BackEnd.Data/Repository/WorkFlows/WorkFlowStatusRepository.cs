using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.WorkFlows;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.WorkFlows
{
    public class WorkFlowStatusRepository : BaseRepository<WorkFlowStatusEntity>
    {
        public override Table Table => TableNames.WorkFlowStatus;
        public Table TableStatusGroup => TableNames.WorkFlowStatusGroup;
        public Table tableWorkflow => TableNames.Workflow;
        public Table tableRol => TableNames.Rol;
        public Table tableRolWriting => TableNames.StatusRolWriting;
        public Table tableRolReading => TableNames.StatusRolReading;

        private WorkFlowNextStatusRepository workflowNextStatusRepository;

        public WorkFlowStatusRepository(TransactionalWrapper conexion) : base(conexion)
        {
            workflowNextStatusRepository = new WorkFlowNextStatusRepository(conexion);
        }

        public override async Task<WorkFlowStatusEntity> InsertSingle(WorkFlowStatusEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(WorkFlowStatusEntity.Codigo)}, " +
                $"{nameof(WorkFlowStatusEntity.Nombre)}, " +
                $"{nameof(WorkFlowStatusEntity.Descripcion)}, " +
                $"{nameof(WorkFlowStatusEntity.Tiempo)}, " +
                $"{nameof(WorkFlowStatusEntity.Completado)}, " +
                $"{nameof(WorkFlowStatusEntity.PublicoLectura)}, " +
                $"{nameof(WorkFlowStatusEntity.PublicoEscritura)}, " +
                $"{nameof(WorkFlowStatusEntity.EstadosAgrupaciones.EstadoAgrupacionID)}, " +
                $"{nameof(WorkFlowStatusEntity.Activo)}, " +
                $"{nameof(WorkFlowStatusEntity.WorkFlow.CoreWorkFlowID)}, " +
                $"{nameof(WorkFlowStatusEntity.Defecto)}) " +
                $"values (" +
                $"@{nameof(WorkFlowStatusEntity.Codigo)}, " +
                $"@{nameof(WorkFlowStatusEntity.Nombre)}, " +
                $"@{nameof(WorkFlowStatusEntity.Descripcion)}, " +
                $"@{nameof(WorkFlowStatusEntity.Tiempo)}, " +
                $"@{nameof(WorkFlowStatusEntity.Completado)}, " +
                $"@{nameof(WorkFlowStatusEntity.PublicoLectura)}, " +
                $"@{nameof(WorkFlowStatusEntity.PublicoEscritura)}, " +
                $"@{nameof(WorkFlowStatusEntity.EstadosAgrupaciones.EstadoAgrupacionID)}, " +
                $"@{nameof(WorkFlowStatusEntity.Activo)}, " +
                $"@{nameof(WorkFlowStatusEntity.WorkFlow.CoreWorkFlowID)}, " +
                $"@{nameof(WorkFlowStatusEntity.Defecto)}); " +
                 $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Porcentaje = obj.Tiempo,
                Completado = obj.Completado,
                PublicoLectura = obj.PublicoLectura,
                PublicoEscritura = obj.PublicoEscritura,
                EstadoAgrupacionID = (obj.EstadosAgrupaciones != null) ? obj.EstadosAgrupaciones.EstadoAgrupacionID : null,
                Activo = obj.Activo,
                CoreWorkFlowID = (obj.WorkFlow != null) ? obj.WorkFlow.CoreWorkFlowID : null,
                Defecto = obj.Defecto
            }, sqlTran)).First();
            obj = WorkFlowStatusEntity.UpdateId(obj, newId);
            foreach (WorkFlowNextStatusEntity estadoVinculado in obj.EstadosSiguientes)
            {
                estadoVinculado.WorkFlowStatus = obj;
                estadoVinculado.WorkFlowStatus.CoreEstadoID = newId;
                var result2 = await workflowNextStatusRepository.InsertSingle(estadoVinculado);
            };

            var resultAdd = await InsertRoles(obj);

            return WorkFlowStatusEntity.UpdateId(obj, newId);
        }

        public async Task<WorkFlowStatusEntity> InsertSingleWithWorkflow(WorkFlowStatusEntity obj, string sWorkflowID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"insert into {Table.Name} (" +
                $"{nameof(WorkFlowStatusEntity.Codigo)}, " +
                $"{nameof(WorkFlowStatusEntity.Nombre)}, " +
                $"{nameof(WorkFlowStatusEntity.Descripcion)}, " +
                $"{nameof(WorkFlowStatusEntity.Tiempo)}, " +
                $"{nameof(WorkFlowStatusEntity.Completado)}, " +
                $"{nameof(WorkFlowStatusEntity.PublicoLectura)}, " +
                $"{nameof(WorkFlowStatusEntity.PublicoEscritura)}, " +
                $"{nameof(WorkFlowStatusEntity.EstadosAgrupaciones.EstadoAgrupacionID)}, " +
                $"{nameof(WorkFlowStatusEntity.Activo)}, " +
                $"{nameof(WorkFlowStatusEntity.Defecto)}, " +
                $"{nameof(WorkFlowStatusEntity.WorkFlow.CoreWorkFlowID)}) " +
                $"values (" +
                $"@{nameof(WorkFlowStatusEntity.Codigo)}, " +
                $"@{nameof(WorkFlowStatusEntity.Nombre)}, " +
                $"@{nameof(WorkFlowStatusEntity.Descripcion)}, " +
                $"@{nameof(WorkFlowStatusEntity.Tiempo)}, " +
                $"@{nameof(WorkFlowStatusEntity.Completado)}, " +
                $"@{nameof(WorkFlowStatusEntity.PublicoLectura)}, " +
                $"@{nameof(WorkFlowStatusEntity.PublicoEscritura)}, " +
                $"@{nameof(WorkFlowStatusEntity.EstadosAgrupaciones.EstadoAgrupacionID)}, " +
                $"@{nameof(WorkFlowStatusEntity.Activo)}, " +
                $"@{nameof(WorkFlowStatusEntity.Defecto)}, " +
                $"@{nameof(WorkFlowStatusEntity.WorkFlow.CoreWorkFlowID)}); " +
                 $"SELECT SCOPE_IDENTITY();";

            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Tiempo = obj.Tiempo,
                Completado = obj.Completado,
                PublicoLectura = obj.PublicoLectura,
                PublicoEscritura = obj.PublicoEscritura,
                EstadoAgrupacionID = (obj.EstadosAgrupaciones != null) ? obj.EstadosAgrupaciones.EstadoAgrupacionID : null,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                CoreWorkFlowID = sWorkflowID
            }, sqlTran)).First();

            return WorkFlowStatusEntity.UpdateId(obj, newId);
        }

        public override async Task<WorkFlowStatusEntity> UpdateSingle(WorkFlowStatusEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            var resultDelete = await DeleteRoles(obj);
            var resultAdd = await InsertRoles(obj);

            var resultStatusDelete = await workflowNextStatusRepository.DeleteByWorkFlowStatus(obj.CoreEstadoID.Value);
            foreach (WorkFlowNextStatusEntity estadoVinculado in obj.EstadosSiguientes)
            {
                estadoVinculado.WorkFlowStatus = obj;
                estadoVinculado.WorkFlowStatus.CoreEstadoID = obj.CoreEstadoID;
                var result2 = await workflowNextStatusRepository.InsertSingle(estadoVinculado);
            };

            string sql = $"update {Table.Name} set {nameof(WorkFlowStatusEntity.Codigo)} = @{nameof(WorkFlowStatusEntity.Codigo)}, " +
                $" {nameof(WorkFlowStatusEntity.Nombre)} =   @{nameof(WorkFlowStatusEntity.Nombre)}, " +
                $" {nameof(WorkFlowStatusEntity.Descripcion)} =  @{nameof(WorkFlowStatusEntity.Descripcion)}, " +
                $" {nameof(WorkFlowStatusEntity.Tiempo)} =  @{nameof(WorkFlowStatusEntity.Tiempo)}, " +
                $" {nameof(WorkFlowStatusEntity.Completado)} =  @{nameof(WorkFlowStatusEntity.Completado)}, " +
                $" {nameof(WorkFlowStatusEntity.PublicoLectura)} =  @{nameof(WorkFlowStatusEntity.PublicoLectura)}, " +
                $" {nameof(WorkFlowStatusEntity.PublicoEscritura)} =  @{nameof(WorkFlowStatusEntity.PublicoEscritura)}, " +
                $" {nameof(WorkFlowStatusEntity.EstadosAgrupaciones.EstadoAgrupacionID)} =  @{nameof(WorkFlowStatusEntity.EstadosAgrupaciones.EstadoAgrupacionID)}, " +
                $" {nameof(WorkFlowStatusEntity.Activo)} = @{nameof(WorkFlowStatusEntity.Activo)}, " +
                $" {nameof(WorkFlowStatusEntity.Defecto)} =  @{ nameof(WorkFlowStatusEntity.Defecto)} " +
                $" where {nameof(WorkFlowStatusEntity.CoreEstadoID)} = @{nameof(WorkFlowStatusEntity.CoreEstadoID)} ";

            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Tiempo = obj.Tiempo,
                Completado = obj.Completado,
                PublicoLectura = obj.PublicoLectura,
                PublicoEscritura = obj.PublicoEscritura,
                EstadoAgrupacionID = (obj.EstadosAgrupaciones != null) ? obj.EstadosAgrupaciones.EstadoAgrupacionID : null,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                CoreEstadoID = obj.CoreEstadoID
            }, sqlTran));
           

            return obj;
        }

        public override async Task<WorkFlowStatusEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} p " +
                $" inner join {TableStatusGroup.Name} pt on p.{TableStatusGroup.ID} = pt.{TableStatusGroup.ID}" +
                $" where p.{Table.Code} = @code";

            var result = await connection.QueryAsync<WorkFlowStatusEntity, WorkFlowStatusGroupEntity, WorkFlowStatusEntity>(sql,
            (statusEntity, statusGroupEntity) =>
            {
                statusEntity.EstadosAgrupaciones = statusGroupEntity;
                return statusEntity;
            }, new { code = code }, sqlTran, true, splitOn: $"{Table.ID},{TableStatusGroup.ID}");

            if (result.ToList() != null && result.ToList().Count > 0)
            {
                string ids = "";
                result.ToList().ForEach(x =>
                {
                    ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.CoreEstadoID}";
                });

                string query = $"select * from {Table.Name} Status " +
                $"left join {tableRolWriting.Name} RolesWriting on Status.{Table.ID} = RolesWriting.{Table.ID} " +
                $"left join {tableRol.Name} Roles on RolesWriting.{tableRol.ID} = Roles.{tableRol.ID} " +
                $" where Status.{nameof(WorkFlowStatusEntity.CoreEstadoID)} IN ({ids})";

                var result2 = await connection.QueryAsync<WorkFlowStatusEntity, WorkflowStatusAssignedRolesWritingEntity, RolEntity, WorkFlowStatusEntity>(query,
                    (workflowStatusEntity, workflowStatusAssignedRolesWritingEntity, rolEntity) =>
                    {
                        if (workflowStatusAssignedRolesWritingEntity != null)
                        {
                            workflowStatusAssignedRolesWritingEntity.CoreWorkflowsEstados = workflowStatusEntity;
                            workflowStatusAssignedRolesWritingEntity.Roles = rolEntity;

                            result.Where(pEnt => pEnt.CoreEstadoID.Value == workflowStatusAssignedRolesWritingEntity.CoreWorkflowsEstados.CoreEstadoID).ToList().ForEach(lk =>
                            {
                                if (lk.EstadosRolesEscritura == null)
                                {
                                    lk.EstadosRolesEscritura = new List<RolEntity>();
                                }

                                ((List<RolEntity>)lk.EstadosRolesEscritura).Add(rolEntity);
                            });
                        }

                        return workflowStatusEntity;
                    }, new { }, sqlTran, true, splitOn: $"{tableRolWriting.ID},{tableRol.ID}");

                string query2 = $"select * from {Table.Name} Status " +
                $"left join {tableRolReading.Name} RolesReading on Status.{Table.ID} = RolesReading.{Table.ID} " +
                $"left join {tableRol.Name} Roles on RolesReading.{tableRol.ID} = Roles.{tableRol.ID} " +
                $" where Status.{nameof(WorkFlowStatusEntity.CoreEstadoID)} IN ({ids})";

                var result3 = await connection.QueryAsync<WorkFlowStatusEntity, WorkflowStatusAssignedRolesReadingEntity, RolEntity, WorkFlowStatusEntity>(query2,
                    (workflowStatusEntity, workflowStatusAssignedRolesReadingEntity, rolEntity) =>
                    {
                        if (workflowStatusAssignedRolesReadingEntity != null)
                        {
                            workflowStatusAssignedRolesReadingEntity.CoreWorkflowsEstados = workflowStatusEntity;
                            workflowStatusAssignedRolesReadingEntity.Roles = rolEntity;

                            result.Where(pEnt => pEnt.CoreEstadoID.Value == workflowStatusAssignedRolesReadingEntity.CoreWorkflowsEstados.CoreEstadoID).ToList().ForEach(lk =>
                            {
                                if (lk.EstadosRolesLectura == null)
                                {
                                    lk.EstadosRolesLectura = new List<RolEntity>();
                                }

                                ((List<RolEntity>)lk.EstadosRolesLectura).Add(rolEntity);
                            });
                        }

                        return workflowStatusEntity;
                    }, new { }, sqlTran, true, splitOn: $"{tableRolReading.ID},{tableRol.ID}");
            }

            return result.FirstOrDefault();

        }

        public async Task<int?> GetItemIDByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var sql = $"select * from {Table.Name} p " +
                $" where p.{Table.Code} = @code";

            var result = await connection.QueryAsync<WorkFlowStatusEntity>(sql, sqlTran);

            return result.FirstOrDefault().CoreEstadoID;

        }

        public override async Task<IEnumerable<WorkFlowStatusEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;

            var sqlRelation = $"p inner join {TableStatusGroup.Name} pt on p.{TableStatusGroup.ID} = pt.{TableStatusGroup.ID} " +
                $" left join {tableWorkflow.Name} wf on p.{tableWorkflow.ID} = wf.{tableWorkflow.ID}";

            string sqlFilter = "";
            string sType = "";

            if (filters != null && filters.Count > 0 && filters[0] != null)
            {
                sqlFilter = " ";

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
                sqlFilter = " ";

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
            var result = await connection.QueryAsync<WorkFlowStatusEntity, WorkFlowStatusGroupEntity, WorkflowEntity, WorkFlowStatusEntity>(query,
                (workFlowStatusEntity, workflowStatusGroupEntity, workflow) =>
                {
                    workFlowStatusEntity.EstadosAgrupaciones = workflowStatusGroupEntity;
                    workFlowStatusEntity.WorkFlow = workflow;
                    return workFlowStatusEntity;
                }, dicParam, sqlTran, true, splitOn: $"{Table.ID},{TableStatusGroup.ID},{tableWorkflow.ID}");

            if (result.Count() > 0)
            {
                IEnumerable<WorkFlowNextStatusEntity> workflowNextStatus;

                foreach (WorkFlowStatusEntity status in result)
                {
                    workflowNextStatus = await workflowNextStatusRepository.GetListbyWorkFlowStatus(status.CoreEstadoID.Value);

                    if (workflowNextStatus != null)
                    {
                        status.EstadosSiguientes = workflowNextStatus;
                    }
                }

                string ids = "";
                result.ToList().ForEach(x =>
                {
                    ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.CoreEstadoID}";
                });

                string query2 = $"select * from {Table.Name} Status " +
                $"left join {tableRolWriting.Name} RolesWriting on Status.{Table.ID} = RolesWriting.{Table.ID} " +
                $"left join {tableRol.Name} Roles on RolesWriting.{tableRol.ID} = Roles.{tableRol.ID} " +
                $" where Status.{nameof(WorkFlowStatusEntity.CoreEstadoID)} IN ({ids})";

                var result2 = await connection.QueryAsync<WorkFlowStatusEntity, WorkflowStatusAssignedRolesWritingEntity, RolEntity, WorkFlowStatusEntity>(query2,
                    (workflowStatusEntity, workflowStatusAssignedRolesWritingEntity, rolEntity) =>
                    {
                        if (workflowStatusAssignedRolesWritingEntity != null)
                        {
                            workflowStatusAssignedRolesWritingEntity.CoreWorkflowsEstados = workflowStatusEntity;
                            workflowStatusAssignedRolesWritingEntity.Roles = rolEntity;

                            result.Where(pEnt => pEnt.CoreEstadoID.Value == workflowStatusAssignedRolesWritingEntity.CoreWorkflowsEstados.CoreEstadoID).ToList().ForEach(lk =>
                            {
                                if (lk.EstadosRolesEscritura == null)
                                {
                                    lk.EstadosRolesEscritura = new List<RolEntity>();
                                }

                                ((List<RolEntity>)lk.EstadosRolesEscritura).Add(rolEntity);
                            });
                        }

                        return workflowStatusEntity;
                    }, new { }, sqlTran, true, splitOn: $"{tableRolWriting.ID},{tableRol.ID}");

                string query3 = $"select * from {Table.Name} Status " +
                $"left join {tableRolReading.Name} RolesReading on Status.{Table.ID} = RolesReading.{Table.ID} " +
                $"left join {tableRol.Name} Roles on RolesReading.{tableRol.ID} = Roles.{tableRol.ID} " +
                $" where Status.{nameof(WorkFlowStatusEntity.CoreEstadoID)} IN ({ids})";

                var result3 = await connection.QueryAsync<WorkFlowStatusEntity, WorkflowStatusAssignedRolesReadingEntity, RolEntity, WorkFlowStatusEntity>(query3,
                    (workflowStatusEntity, workflowStatusAssignedRolesReadingEntity, rolEntity) =>
                    {
                        if (workflowStatusAssignedRolesReadingEntity != null)
                        {
                            workflowStatusAssignedRolesReadingEntity.CoreWorkflowsEstados = workflowStatusEntity;
                            workflowStatusAssignedRolesReadingEntity.Roles = rolEntity;

                            result.Where(pEnt => pEnt.CoreEstadoID.Value == workflowStatusAssignedRolesReadingEntity.CoreWorkflowsEstados.CoreEstadoID).ToList().ForEach(lk =>
                            {
                                if (lk.EstadosRolesLectura == null)
                                {
                                    lk.EstadosRolesLectura = new List<RolEntity>();
                                }

                                ((List<RolEntity>)lk.EstadosRolesLectura).Add(rolEntity);
                            });
                        }

                        return workflowStatusEntity;
                    }, new { }, sqlTran, true, splitOn: $"{tableRolReading.ID},{tableRol.ID}");
            }

            return result.ToList();
        }

        public override async Task<int> Delete(WorkFlowStatusEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreEstadoID,
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public async Task<int> DeleteRoles(WorkFlowStatusEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sqlReading = $"delete from {tableRolReading.Name} Where " +
                $"{nameof(WorkFlowStatusEntity.CoreEstadoID)} = @{nameof(WorkFlowStatusEntity.CoreEstadoID)}";
            var numReg = await connection.ExecuteAsync(sqlReading, new
            {
                CoreEstadoID = obj.CoreEstadoID
            }, sqlTran);

            string sqlWriting = $"delete from {tableRolWriting.Name} Where " +
                $"{nameof(WorkFlowStatusEntity.CoreEstadoID)} = @{nameof(WorkFlowStatusEntity.CoreEstadoID)}";
            numReg = await connection.ExecuteAsync(sqlWriting, new
            {
                CoreEstadoID = obj.CoreEstadoID
            }, sqlTran);

            return numReg;
        }

        public async Task<int> InsertRoles(WorkFlowStatusEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var idLinkedRol = 0;

            if (obj.EstadosRolesLectura != null)
            {
                foreach (RolEntity rolVinculada in obj.EstadosRolesLectura.ToList())
                {
                    if (rolVinculada != null)
                    {
                        string sqlReading = $"INSERT INTO {tableRolReading.Name} " +
                                $"({nameof(WorkFlowStatusEntity.CoreEstadoID)}, " +
                                $"{nameof(RolEntity.RolID)})" +
                                $"values (@{nameof(WorkFlowStatusEntity.CoreEstadoID)}, " +
                                $"@{nameof(rolVinculada.RolID)});" +
                                $"SELECT SCOPE_IDENTITY();";

                        idLinkedRol = (await connection.QueryAsync<int>(sqlReading, new
                        {
                            CoreEstadoID = obj.CoreEstadoID,
                            RolID = rolVinculada.RolID
                        }, sqlTran)).First();
                    }
                };
            }
            
            if (obj.EstadosRolesEscritura != null)
            {
                foreach (RolEntity rolVinculada in obj.EstadosRolesEscritura.ToList())
                {
                    if (rolVinculada != null)
                    {
                        string sqlWriting = $"INSERT INTO {tableRolWriting.Name} " +
                                $"({nameof(WorkFlowStatusEntity.CoreEstadoID)}, " +
                                $"{nameof(RolEntity.RolID)})" +
                                $"values (@{nameof(WorkFlowStatusEntity.CoreEstadoID)}, " +
                                $"@{nameof(rolVinculada.RolID)});" +
                                $"SELECT SCOPE_IDENTITY();";

                        idLinkedRol = (await connection.QueryAsync<int>(sqlWriting, new
                        {
                            CoreEstadoID = obj.CoreEstadoID,
                            RolID = rolVinculada.RolID
                        }, sqlTran)).First();
                    }
                };
            }            

            return idLinkedRol;
        }

        public async Task<int> DeleteByWorkflow(int WorkflowID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"delete from {Table.Name} Where {tableWorkflow.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = WorkflowID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public async Task<IEnumerable<WorkFlowStatusEntity>> InsertList(IEnumerable<WorkFlowStatusEntity> obj, string workflowid)
        {
            foreach (WorkFlowStatusEntity estadoVinculado in obj)
            {
                WorkFlowStatusEntity result = await InsertSingleWithWorkflow(estadoVinculado, workflowid);
                estadoVinculado.CoreEstadoID = result.CoreEstadoID;

                var numRegReading = await DeleteRoles(estadoVinculado);
                var resultAdd = await InsertRoles(estadoVinculado);
            };
            foreach (WorkFlowStatusEntity estadoVinculado in obj)
            {
                if (estadoVinculado.EstadosSiguientes.Count() > 0)
                {
                    var result2 = await workflowNextStatusRepository.InsertList(estadoVinculado.EstadosSiguientes, obj, estadoVinculado.CoreEstadoID.Value);
                }
            };

            return obj;
        }

        public async Task<IEnumerable<WorkFlowStatusEntity>> GetListByWorkflow(int workflowid)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string query = $"select * from {Table.Name} p " +
                $"inner join {TableStatusGroup.Name} pt on p.{TableStatusGroup.ID} = pt.{TableStatusGroup.ID} " +
                $" where {tableWorkflow.ID} = @workflowid order by p.{Table.Code}";

            var result = await connection.QueryAsync<WorkFlowStatusEntity, WorkFlowStatusGroupEntity, WorkFlowStatusEntity>(query,
           (statusEntity, statusGroupEntity) =>
           {
               statusEntity.EstadosAgrupaciones = statusGroupEntity;
               return statusEntity;
           }, new { workflowid = workflowid }, sqlTran, true, splitOn: $"{Table.ID},{TableStatusGroup.ID}");

            if (result.Count() > 0)
            {
                IEnumerable<WorkFlowNextStatusEntity> workflowStatus;

                foreach (WorkFlowStatusEntity workflow in result)
                {
                    workflowStatus = await workflowNextStatusRepository.GetListbyWorkFlowStatus(workflow.CoreEstadoID.Value);

                    if (workflowStatus != null)
                    {
                        workflow.EstadosSiguientes = workflowStatus;
                    }
                }

                string ids = "";
                result.ToList().ForEach(x =>
                {
                    ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.CoreEstadoID}";
                });

                string query2 = $"select * from {Table.Name} Status " +
                $"left join {tableRolWriting.Name} RolesWriting on Status.{Table.ID} = RolesWriting.{Table.ID} " +
                $"left join {tableRol.Name} Roles on RolesWriting.{tableRol.ID} = Roles.{tableRol.ID} " +
                $" where Status.{nameof(WorkFlowStatusEntity.CoreEstadoID)} IN ({ids})";

                var result2 = await connection.QueryAsync<WorkFlowStatusEntity, WorkflowStatusAssignedRolesWritingEntity, RolEntity, WorkFlowStatusEntity>(query2,
                    (workflowStatusEntity, workflowStatusAssignedRolesWritingEntity, rolEntity) =>
                    {
                        if (workflowStatusAssignedRolesWritingEntity != null)
                        {
                            workflowStatusAssignedRolesWritingEntity.CoreWorkflowsEstados = workflowStatusEntity;
                            workflowStatusAssignedRolesWritingEntity.Roles = rolEntity;

                            result.Where(pEnt => pEnt.CoreEstadoID.Value == workflowStatusAssignedRolesWritingEntity.CoreWorkflowsEstados.CoreEstadoID).ToList().ForEach(lk =>
                            {
                                if (lk.EstadosRolesEscritura == null)
                                {
                                    lk.EstadosRolesEscritura = new List<RolEntity>();
                                }

                                ((List<RolEntity>)lk.EstadosRolesEscritura).Add(rolEntity);
                            });
                        }

                        return workflowStatusEntity;
                    }, new { }, sqlTran, true, splitOn: $"{tableRolWriting.ID},{tableRol.ID}");

                string query3 = $"select * from {Table.Name} Status " +
                $"left join {tableRolReading.Name} RolesReading on Status.{Table.ID} = RolesReading.{Table.ID} " +
                $"left join {tableRol.Name} Roles on RolesReading.{tableRol.ID} = Roles.{tableRol.ID} " +
                $" where Status.{nameof(WorkFlowStatusEntity.CoreEstadoID)} IN ({ids})";

                var result3 = await connection.QueryAsync<WorkFlowStatusEntity, WorkflowStatusAssignedRolesReadingEntity, RolEntity, WorkFlowStatusEntity>(query3,
                    (workflowStatusEntity, workflowStatusAssignedRolesReadingEntity, rolEntity) =>
                    {
                        if (workflowStatusAssignedRolesReadingEntity != null)
                        {
                            workflowStatusAssignedRolesReadingEntity.CoreWorkflowsEstados = workflowStatusEntity;
                            workflowStatusAssignedRolesReadingEntity.Roles = rolEntity;

                            result.Where(pEnt => pEnt.CoreEstadoID.Value == workflowStatusAssignedRolesReadingEntity.CoreWorkflowsEstados.CoreEstadoID).ToList().ForEach(lk =>
                            {
                                if (lk.EstadosRolesLectura == null)
                                {
                                    lk.EstadosRolesLectura = new List<RolEntity>();
                                }

                                ((List<RolEntity>)lk.EstadosRolesLectura).Add(rolEntity);
                            });
                        }

                        return workflowStatusEntity;
                    }, new { }, sqlTran, true, splitOn: $"{tableRolReading.ID},{tableRol.ID}");
            }


            return result.ToList();
        }

        public async Task<IEnumerable<WorkFlowStatusEntity>> UpdateList(IEnumerable<WorkFlowStatusEntity> obj, string workflowid)
        {
            IEnumerable<WorkFlowStatusEntity> listaExiste = await GetListByWorkflow(int.Parse(workflowid));

            #region REMOVE LINKED STATUS

            foreach (WorkFlowStatusEntity estadoVinculado in listaExiste)
            {
                int idBorrar = compruebaExiste(obj, estadoVinculado);
                if (idBorrar != 0)
                {
                    var result = await Delete(estadoVinculado);
                    var resultRoles = await DeleteRoles(estadoVinculado);
                }
            }

            #endregion

            #region ADD LINKED STATUS

            if (obj != null)
            {

                foreach (WorkFlowStatusEntity estadoVinculado in obj.ToList())
                {
                    estadoVinculado.CoreEstadoID = contieneNombre(listaExiste, estadoVinculado);

                    if (estadoVinculado.CoreEstadoID == 0)
                    {
                        var result = await InsertSingleWithWorkflow(estadoVinculado, workflowid);
                        estadoVinculado.CoreEstadoID = result.CoreEstadoID;
                    }
                    else
                    {
                        var result = await UpdateSingle(estadoVinculado);
                    }
                };

                foreach (WorkFlowStatusEntity estadoVinculado in obj)
                {
                    if (estadoVinculado.EstadosSiguientes.Count() > 0)
                    {
                        var result2 = await workflowNextStatusRepository.InsertList(estadoVinculado.EstadosSiguientes, obj, estadoVinculado.CoreEstadoID.Value);
                    }
                };
            }
            #endregion

            return obj;
        }

        private int compruebaExiste(IEnumerable<WorkFlowStatusEntity> listaExiste, WorkFlowStatusEntity obj)
        {
            foreach (WorkFlowStatusEntity workflowStatus in listaExiste)
            {
                if (workflowStatus.Codigo == obj.Codigo)
                {
                    return 0;
                }
            }

            return obj.CoreEstadoID.Value;
        }

        private int contieneNombre(IEnumerable<WorkFlowStatusEntity> listaExiste, WorkFlowStatusEntity obj)
        {
            foreach (WorkFlowStatusEntity workflowStatus in listaExiste)
            {
                if (workflowStatus.Codigo == obj.Codigo)
                {
                    return workflowStatus.CoreEstadoID.Value;
                }
            }
            return 0;
        }
    }
}
