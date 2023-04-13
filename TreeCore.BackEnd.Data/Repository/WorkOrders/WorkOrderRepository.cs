using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.WorkOrders
{
    public class WorkOrderRepository : BaseRepository<WorkOrderEntity>
    {
        public override Table Table => TableNames.WorkOrders;
        public  Table TableProject => TableNames.Project;

        public WorkOrderRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<WorkOrderEntity> InsertSingle(WorkOrderEntity obj)
        {
            string sql = $"insert into {Table.Name} (" +
                $"{nameof(WorkOrderEntity.Codigo)}, " +
                $"{nameof(WorkOrderEntity.Activos)}, " +
                $"{nameof(WorkOrderEntity.FechaInicio)}, " +
                $"{nameof(WorkOrderEntity.FechaFin)}, " +
                $"{nameof(WorkOrderEntity.Porcentaje)}, " +
                $"{nameof(WorkOrderEntity.FechaCreacion)}, " +
                $"{nameof(WorkOrderEntity.FechaUltimaModificacion)}, " +
                $"{nameof(WorkOrderEntity.UsuariosCreador.UsuarioID)}, " +
                $"{nameof(WorkOrderEntity.UsuariosModificador.UsuarioID)}, " +
                $"{nameof(WorkOrderEntity.UsuariosSupplier.UsuarioID)}, " +
                $"{nameof(WorkOrderEntity.UsuariosCustomer.UsuarioID)}, " +
                $"{nameof(WorkOrderEntity.CoreWorkflows.CoreWorkFlowID)}, " +
                $"{nameof(WorkOrderEntity.EntidadesCustomer.EntidadID)}, " +
                $"{nameof(WorkOrderEntity.EntidadesSupplier.EntidadID)}, " +
                $"{nameof(WorkOrderEntity.CoreWorkOrderLifeCycleStatus.CoreWorkOrderEstadoID)}) " +
                $"values (" +
                $"@{nameof(WorkOrderEntity.Codigo)}, " +
                $"@{nameof(WorkOrderEntity.Activos)}, " +
                $"@{nameof(WorkOrderEntity.FechaInicio)}, " +
                $"@{nameof(WorkOrderEntity.FechaFin)}, " +
                $"@{nameof(WorkOrderEntity.Porcentaje)}, " +
                $"@{nameof(WorkOrderEntity.FechaCreacion)}, " +
                $"@{nameof(WorkOrderEntity.FechaUltimaModificacion)}, " +
                $"@{nameof(WorkOrderEntity.UsuariosCreador.UsuarioID)}, " +
                $"@{nameof(WorkOrderEntity.UsuariosModificador.UsuarioID)}, " +
                $"@{nameof(WorkOrderEntity.UsuariosSupplier.UsuarioID)}, " +
                $"@{nameof(WorkOrderEntity.UsuariosCustomer.UsuarioID)}, " +
                $"@{nameof(WorkOrderEntity.CoreWorkflows.CoreWorkFlowID)}, " +
                $"@{nameof(WorkOrderEntity.EntidadesCustomer.EntidadID)}, " +
                $"@{nameof(WorkOrderEntity.EntidadesSupplier.EntidadID)}, " +
                $"@{nameof(WorkOrderEntity.CoreWorkOrderLifeCycleStatus.CoreWorkOrderEstadoID)}); " +
                 $"SELECT SCOPE_IDENTITY();";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                Codigo = obj.Codigo,
                Activos = obj.Activos,
                FechaInicio = obj.FechaInicio,
                FechaFin = obj.FechaFin,
                Porcentaje = obj.Porcentaje,
                FechaCreacion = obj.FechaCreacion,
                FechaUltimaModificacion = obj.FechaUltimaModificacion,
                UsuariosCreador = obj.UsuariosCreador,
                UsuariosSupplier = obj.UsuariosSupplier,
                UsuariosCustomer = obj.UsuariosCustomer,
                UsuariosModificador = obj.UsuariosModificador,
                CoreWorkflows = obj.CoreWorkflows,
                EntidadesCustomer = obj.EntidadesCustomer,
                EntidadesSupplier = obj.EntidadesSupplier,
                CoreWorkOrderLifeCycleStatus = obj.CoreWorkOrderLifeCycleStatus
            }, sqlTran)).First();

            return WorkOrderEntity.UpdateId(obj, newId);
        }
        public override async Task<WorkOrderEntity> UpdateSingle(WorkOrderEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(WorkOrderEntity.Codigo)} = @{nameof(WorkOrderEntity.Codigo)}, " +
                $" {nameof(WorkOrderEntity.Activos)} = @{nameof(WorkOrderEntity.Activos)}, " +
                $" {nameof(WorkOrderEntity.FechaInicio)} =  @{nameof(WorkOrderEntity.FechaInicio)}, " +
                $" {nameof(WorkOrderEntity.FechaFin)} =  @{nameof(WorkOrderEntity.FechaFin)}, " +
                $" {nameof(WorkOrderEntity.Porcentaje)} =  @{nameof(WorkOrderEntity.Porcentaje)}, " +
                $" {nameof(WorkOrderEntity.FechaUltimaModificacion)} =  @{nameof(WorkOrderEntity.FechaUltimaModificacion)}, " +
                $" {nameof(WorkOrderEntity.UsuariosSupplier.UsuarioID)} =  @{nameof(WorkOrderEntity.UsuariosSupplier.UsuarioID)}, " +
                $" {nameof(WorkOrderEntity.UsuariosCustomer.UsuarioID)} =  @{nameof(WorkOrderEntity.UsuariosCustomer.UsuarioID)}, " +
                $" {nameof(WorkOrderEntity.UsuariosModificador.UsuarioID)} =  @{nameof(WorkOrderEntity.UsuariosModificador.UsuarioID)}, " +
                $" {nameof(WorkOrderEntity.CoreWorkflows.CoreWorkFlowID)} =  @{nameof(WorkOrderEntity.CoreWorkflows.CoreWorkFlowID)}, " +
                $" {nameof(WorkOrderEntity.EntidadesCustomer.EntidadID)} =  @{nameof(WorkOrderEntity.EntidadesCustomer.EntidadID)}, " +
                $" {nameof(WorkOrderEntity.EntidadesSupplier.EntidadID)} =  @{nameof(WorkOrderEntity.EntidadesSupplier.EntidadID)}, " +
                $" {nameof(WorkOrderEntity.CoreWorkOrderLifeCycleStatus.CoreWorkOrderEstadoID)} =  @{nameof(WorkOrderEntity.CoreWorkOrderLifeCycleStatus.CoreWorkOrderEstadoID)}, " +
                $" where {nameof(WorkOrderEntity.CoreWorkOrderID)} = @{nameof(WorkOrderEntity.CoreWorkOrderID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                Activos = obj.Activos,
                FechaInicio = obj.FechaInicio,
                FechaFin = obj.FechaFin,
                Porcentaje = obj.Porcentaje,
                FechaUltimaModificacion = obj.FechaUltimaModificacion,
                UsuariosSupplier = obj.UsuariosSupplier,
                UsuariosCustomer = obj.UsuariosCustomer,
                UsuariosModificador = obj.UsuariosModificador,
                CoreWorkflows = obj.CoreWorkflows,
                EntidadesCustomer = obj.EntidadesCustomer,
                EntidadesSupplier = obj.EntidadesSupplier,
                CoreWorkOrderLifeCycleStatus = obj.CoreWorkOrderLifeCycleStatus,
                CoreWorkOrderID = obj.CoreWorkOrderID
            }, sqlTran));
            return obj;
        }

        public override async Task<WorkOrderEntity> GetItemByCode(string code, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<WorkOrderEntity>($"select * from {Table.Name} where {Table.Code} = @code", new
            {
                code = code
            }, sqlTran);

        }

        public async Task<IEnumerable<WorkOrderEntity>> GetItemsByProject(string woCode, int client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            var sql = $"select wo.* from {Table.Name} wo " +
                $"left join {TableProject.Name} p on wo.{TableProject.ID} = p.{TableProject.ID} " +
                $"where p.{TableProject.Code} = @code";

            return await connection.QueryAsync<WorkOrderEntity>(sql, new
            {
                code = woCode
            }, sqlTran);

        }

        public override async Task<int> Delete(WorkOrderEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.CoreWorkOrderID
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
