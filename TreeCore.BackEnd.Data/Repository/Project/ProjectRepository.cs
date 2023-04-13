using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.General;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.Project;
using TreeCore.BackEnd.Model.ValueObject;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;

namespace TreeCore.BackEnd.Data.Repository.WorkOrders
{
    public class ProjectRepository : BaseRepository<ProjectEntity>
    {
        public override Table Table => TableNames.Project;
        public Table TableBusinessProcess => TableNames.BusinessProcess;
        public Table TableProgram => TableNames.Program;
        public Table TableProjectLifeCycleStatus => TableNames.ProjectLifeCycleStatus;
        public Table TableCurrency => TableNames.Currency;
        public Table TableUser => TableNames.User;

        private UserRepository userRepository;

        private WorkOrderRepository woRepository;

        public ProjectRepository(TransactionalWrapper conexion) : base(conexion)
        {
            userRepository = new UserRepository(conexion);
            woRepository = new WorkOrderRepository(conexion);
        }

        public override async Task<ProjectEntity> InsertSingle(ProjectEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(ProjectEntity.ClienteID)}, " +
                $"{nameof(ProjectEntity.Codigo)}, " +
                $"{nameof(ProjectEntity.Nombre)}, " +
                $"{nameof(ProjectEntity.Descripcion)}, " +
                $"{nameof(ProjectEntity.Activo)}, " +
                $"{nameof(ProjectEntity.CoreBusinessProcess.CoreBusinessProcessID)}, " +
                $"{nameof(ProjectEntity.CoreProgram.CoreProgramID)}, " +
                $"{nameof(ProjectEntity.CoreProjectLifeCycleStatus.CoreProjectLifeCycleStatusID)}, " +
                $"Budget{nameof(ProjectEntity.Budget.BudgetMoneda.MonedaID)}, " +
                $"{nameof(ProjectEntity.Budget.BudgetValor)}, " +
               // $"{nameof(ProjectEntity.Activos)}, " +
                $"{nameof(ProjectEntity.FechaCreacion)}, " +
                $"UsuarioCreadorID, " +
                $"{nameof(ProjectEntity.FechaUltimaModificacion)}, " +
                $"UsuarioModificadorID, " +
                $"{nameof(ProjectEntity.FechaFin)} " +
                $") " +
                $"values (" +
                $"@{nameof(ProjectEntity.ClienteID)}, " +
                $"@{nameof(ProjectEntity.Codigo)}, " +
                $"@{nameof(ProjectEntity.Nombre)}, " +
                $"@{nameof(ProjectEntity.Descripcion)}, " +
                $"@{nameof(ProjectEntity.Activo)}, " +
                $"@{nameof(ProjectEntity.CoreBusinessProcess.CoreBusinessProcessID)}, " +
                $"@{nameof(ProjectEntity.CoreProgram.CoreProgramID)}, " +
                $"@{nameof(ProjectEntity.CoreProjectLifeCycleStatus.CoreProjectLifeCycleStatusID)}, " +
                $"@Budget{nameof(ProjectEntity.Budget.BudgetMoneda.MonedaID)}, " +
                $"@{nameof(ProjectEntity.Budget.BudgetValor)}, " +
                //$"@{nameof(ProjectEntity.Activos)}, " +
                $"@{nameof(ProjectEntity.FechaCreacion)}, " +
                $"@UsuarioCreadorID, " +
                $"@{nameof(ProjectEntity.FechaUltimaModificacion)}, " +
                $"@UsuarioModificadorID, " +
                $"@{nameof(ProjectEntity.FechaFin)} " +
                $"); " +
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
                CoreBusinessProcessID = obj.CoreBusinessProcess.CoreBusinessProcessID,
                CoreProgramID = obj.CoreProgram.CoreProgramID,
                CoreProjectLifeCycleStatusID = obj.CoreProjectLifeCycleStatus.CoreProjectLifeCycleStatusID,
                BudgetMonedaID = obj.Budget.BudgetMoneda.MonedaID,
                BudgetValor = obj.Budget.BudgetValor,
                //Activos = obj.Activos,
                FechaCreacion = obj.FechaCreacion,
                UsuarioCreadorID = obj.UsuarioCreador.UsuarioID,
                FechaUltimaModificacion = obj.FechaUltimaModificacion,
                UsuarioModificadorID = obj.UsuarioModificador.UsuarioID,
                FechaFin = obj.FechaFin
            }, sqlTran)).First();

            return ProjectEntity.UpdateId(obj, newId);
        }
        public override async Task<ProjectEntity> UpdateSingle(ProjectEntity obj)
        {
            string sql = $"update {Table.Name} set " +
                $" {nameof(ProjectEntity.Codigo)} = @{nameof(ProjectEntity.Codigo)}, " +
                $" {nameof(ProjectEntity.Nombre)} =   @{nameof(ProjectEntity.Nombre)}, " +
                $" {nameof(ProjectEntity.Descripcion)} =  @{nameof(ProjectEntity.Descripcion)}, " +
                $" {nameof(ProjectEntity.Activo)} = @{nameof(ProjectEntity.Activo)}, " +
                $" {nameof(ProjectEntity.CoreBusinessProcess.CoreBusinessProcessID)} = @{nameof(ProjectEntity.CoreBusinessProcess.CoreBusinessProcessID)}, " +
                $" {nameof(ProjectEntity.CoreProgram.CoreProgramID)} = @{nameof(ProjectEntity.CoreProgram.CoreProgramID)}, " +
                $" {nameof(ProjectEntity.CoreProjectLifeCycleStatus.CoreProjectLifeCycleStatusID)} = @{nameof(ProjectEntity.CoreProjectLifeCycleStatus.CoreProjectLifeCycleStatusID)}, " +
                $" BudgetMonedaID = @BudgetMonedaID, " +
                $" {nameof(ProjectEntity.Budget.BudgetValor)} = @{nameof(ProjectEntity.Budget.BudgetValor)}, " +
                //$" {nameof(ProjectEntity.Activos)} = @{nameof(ProjectEntity.Activos)}, " +
                $" {nameof(ProjectEntity.FechaFin)} = @{nameof(ProjectEntity.FechaFin)}, " +
                $" {nameof(ProjectEntity.FechaUltimaModificacion)} = @{nameof(ProjectEntity.FechaUltimaModificacion)}, " +
                $" UsuarioModificadorID =  @UsuarioModificadorID " +
                $" where {nameof(ProjectEntity.CoreProjectID)} = @{nameof(ProjectEntity.CoreProjectID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Nombre = obj.Nombre,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                CoreBusinessProcessID = obj.CoreBusinessProcess.CoreBusinessProcessID,
                CoreProgramID = obj.CoreProgram.CoreProgramID,
                CoreProjectLifeCycleStatusID = obj.CoreProjectLifeCycleStatus.CoreProjectLifeCycleStatusID,
                BudgetMonedaID = obj.Budget.BudgetMoneda.MonedaID,
                BudgetValor = obj.Budget.BudgetValor,
                //Activos = obj.Activos,
                FechaUltimaModificacion = obj.FechaUltimaModificacion,
                UsuarioModificadorID = obj.UsuarioModificador.UsuarioID,
                FechaFin = obj.FechaFin,
                CoreProjectID = obj.CoreProjectID
            }, sqlTran));
            return obj;
        }

        public override async Task<ProjectEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"select * from {Table.Name} project " +
                $"inner join {TableCurrency.Name} currency on project.BudgetMonedaID = currency.{TableCurrency.ID} " +
                $"inner join {TableBusinessProcess.Name} businessProcess on project.{TableBusinessProcess.ID} = businessProcess.{TableBusinessProcess.ID} " +
                $"inner join {TableProgram.Name} program on project.{TableProgram.ID} = program.{TableProgram.ID} " +
                $"inner join {TableProjectLifeCycleStatus.Name} status on project.{TableProjectLifeCycleStatus.ID} = status.{TableProjectLifeCycleStatus.ID} " +
                $"where project.{Table.Code} = @code and project.{nameof(ProjectEntity.ClienteID)} = @{nameof(ProjectEntity.ClienteID)}";

            var result = await connection.QueryAsync<ProjectEntity, Budget, CurrencyEntity, BusinessProcessEntity, ProgramEntity, ProjectLifeCycleStatusEntity, ProjectEntity>(sql,
                (project, budget, currency, businessprocess, program, status) =>
                {
                    budget.BudgetMoneda = currency;
                    project.Budget = budget;
                    project.CoreBusinessProcess = businessprocess;
                    project.CoreProgram = program;
                    project.CoreProjectLifeCycleStatus = status;
                    return project;
                }, new { code = code, ClienteID = client }, sqlTran, true,
                splitOn: $"BudgetMonedaID,{TableCurrency.ID},{TableBusinessProcess.ID},{TableProgram.ID},{TableProjectLifeCycleStatus.ID}");
            var item = result.FirstOrDefault();
            if (item != null)
            {
                item.UsuarioCreador = await userRepository.GetItemByCompany(item.CoreProjectID.ToString(), Table, "UsuarioCreadorID");
                item.UsuarioModificador = await userRepository.GetItemByCompany(item.CoreProjectID.ToString(), Table, "UsuarioModificadorID");
                item.WorkOrders = await woRepository.GetItemsByProject(item.Codigo, client);
            }
            return item;
        }

        public override async Task<int> Delete(ProjectEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key" +
                $" AND {nameof(ProjectEntity.ClienteID)} = @{nameof(ProjectEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreProjectID,
                    ClienteID = obj.ClienteID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

        public override async Task<IEnumerable<ProjectEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            int countFilter = 0;

            string sqlFilter = "";
            string sType = "";
            if (filters != null && filters.Count > 0 && filters[0] != null)
            {
                sqlFilter = " ";

                if (filters[0].Filters != null && filters[0].Filters[0].Type != null && filters[0].Filters.Count > 1)
                {
                    sType = filters[0].Filters[0].Type;
                }
                else if (filters[0].Type != null)
                {
                    sType = filters[0].Type;
                }
                else
                {
                    sType = Filter.Types.AND;
                }
                sqlFilter += Filter.BuilFilters(filters, sType, countFilter, out countFilter, dicParam, out dicParam, "");
            }
            else if (filters != null && filters.Count > 0)
            {
                sqlFilter = " ";

                string filtros = Filter.BuilFilters(filters, Filter.Types.AND, countFilter, out countFilter, dicParam, out dicParam, "");

                if (filtros != " ")
                {
                    sqlFilter += filtros;
                }
                else
                {
                    sqlFilter = " ";
                }
            }

            if (string.IsNullOrWhiteSpace(sqlFilter))
            {
                sqlFilter = $" WHERE project.ClienteID = {Client}";
            }
            else
            {
                sqlFilter += $" {Filter.Types.AND} project.ClienteID = {Client}";
            }

            string sqlOrder = "";
            if (orders != null && orders.Count > 0)
            {
                foreach (string column in orders)
                {
                    sqlOrder += $"{(string.IsNullOrEmpty(sqlOrder) ? " order by project." : ", project.")} {column}";
                }
            }
            else
            {
                sqlOrder = $" order by  project.{Table.Code} ";
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


            string query = $"select TotalItems = COUNT(*) OVER(), * from {Table.Name} project " +
                $"left join {TableCurrency.Name} currency on project.BudgetMonedaID = currency.{TableCurrency.ID} " +
                $"left join {TableBusinessProcess.Name} businessProcess on project.{TableBusinessProcess.ID} = businessProcess.{TableBusinessProcess.ID} " +
                $"left join {TableProgram.Name} program on project.{TableProgram.ID} = program.{TableProgram.ID} " +
                $"left join {TableProjectLifeCycleStatus.Name} status on project.{TableProjectLifeCycleStatus.ID} = status.{TableProjectLifeCycleStatus.ID} {sqlFilter} {sqlOrder} {direction} {sqlPagination}";
            var result = await connection.QueryAsync<ProjectEntity, Budget, CurrencyEntity, BusinessProcessEntity, ProgramEntity, ProjectLifeCycleStatusEntity, ProjectEntity>(query,
                (project, budget, currency, businessprocess, program, status) =>
                {
                    budget.BudgetMoneda = currency;
                    project.Budget = budget;
                    project.CoreBusinessProcess = businessprocess;
                    project.CoreProgram = program;
                    project.CoreProjectLifeCycleStatus = status;
                    return project;
                }, dicParam, sqlTran, true,
                splitOn: $"BudgetMonedaID,{TableCurrency.ID},{TableBusinessProcess.ID},{TableProgram.ID},{TableProjectLifeCycleStatus.ID}");
            foreach (var item in result)
            {
                item.UsuarioCreador = await userRepository.GetItemByCompany(item.CoreProjectID.ToString(), Table, "UsuarioCreadorID");
                item.UsuarioModificador = await userRepository.GetItemByCompany(item.CoreProjectID.ToString(), Table, "UsuarioModificadorID");
                item.WorkOrders = await woRepository.GetItemsByProject(item.Codigo, Client);
            }
            return result;
        }

    }
}
