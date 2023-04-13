using System;
using System.Collections.Generic;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.WorkFlows;

namespace TreeCore.BackEnd.Model.Entity.WorkOrders
{
    public class WorkOrderEntity : BaseEntity
    {
        public readonly int? CoreWorkOrderID;
        public readonly string Codigo;
        public readonly string Activos;
        public readonly DateTime FechaInicio;
        public readonly DateTime FechaFin;
        public readonly float Porcentaje;
        public readonly DateTime FechaCreacion;
        public readonly DateTime FechaUltimaModificacion;
        public readonly UserEntity UsuariosCreador;
        public readonly UserEntity UsuariosModificador;
        public readonly UserEntity UsuariosSupplier;
        public readonly UserEntity UsuariosCustomer;
        public readonly WorkflowEntity CoreWorkflows;
        public readonly CompanyEntity EntidadesCustomer;
        public readonly CompanyEntity EntidadesSupplier;
        public readonly WorkOrderLifecycleStatusEntity CoreWorkOrderLifeCycleStatus;
        public IEnumerable<WorkOrderTrackingStatusEntity> WorkOrderTrackingStatus;
        public readonly int CoreProjectID;

        public WorkOrderEntity(int? coreWorkOrderID, string codigo, string activos, DateTime fechaInicio, DateTime fechaFin, float porcentaje, 
            DateTime fechaCreacion, DateTime fechaUltimaModificacion, UserEntity usuariosCreador, UserEntity usuariosModificador, UserEntity usuariosSupplier, 
            UserEntity usuariosCustomer, WorkflowEntity coreWorkflows, CompanyEntity entidadesCustomer, CompanyEntity entidadesSupplier, 
            WorkOrderLifecycleStatusEntity coreWorkOrderLifeCycleStatus, IEnumerable<WorkOrderTrackingStatusEntity> workOrderTrackingStatus, int coreProjectID)
        {
            CoreWorkOrderID = coreWorkOrderID;
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            Activos = activos;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Porcentaje = porcentaje;
            FechaCreacion = fechaCreacion;
            FechaUltimaModificacion = fechaUltimaModificacion;
            UsuariosCreador = usuariosCreador;
            UsuariosModificador = usuariosModificador;
            UsuariosSupplier = usuariosSupplier;
            UsuariosCustomer = usuariosCustomer;
            CoreWorkflows = coreWorkflows;
            EntidadesCustomer = entidadesCustomer;
            EntidadesSupplier = entidadesSupplier;
            CoreWorkOrderLifeCycleStatus = coreWorkOrderLifeCycleStatus;
            WorkOrderTrackingStatus = workOrderTrackingStatus;
            CoreProjectID = coreProjectID;
        }

        protected WorkOrderEntity()
        {
        }

        public static WorkOrderEntity Create(int id, string codigo, string activos, DateTime fechaInicio, DateTime fechaFin, float porcentaje,
            DateTime fechaCreacion, DateTime fechaUltimaModificacion, UserEntity usuariosCreador, UserEntity usuariosModificador, UserEntity usuariosSupplier,
            UserEntity usuariosCustomer, WorkflowEntity coreWorkflows, CompanyEntity entidadesCustomer, CompanyEntity entidadesSupplier,
            WorkOrderLifecycleStatusEntity coreWorkOrderLifeCycleStatus, IEnumerable<WorkOrderTrackingStatusEntity> workOrderTrackingStatus, int coreProjectID)
            => new WorkOrderEntity(id, codigo, activos, fechaInicio, fechaFin, porcentaje, fechaCreacion, fechaUltimaModificacion,
            usuariosCreador, usuariosModificador, usuariosSupplier, usuariosCustomer, coreWorkflows, entidadesCustomer, entidadesSupplier,
            coreWorkOrderLifeCycleStatus, workOrderTrackingStatus, coreProjectID);

        public static WorkOrderEntity UpdateId(WorkOrderEntity workOrder, int id) =>
            new WorkOrderEntity(id, workOrder.Codigo, workOrder.Activos, workOrder.FechaInicio, workOrder.FechaFin, workOrder.Porcentaje, workOrder.FechaCreacion,
                workOrder.FechaUltimaModificacion, workOrder.UsuariosCreador, workOrder.UsuariosModificador, workOrder.UsuariosSupplier, workOrder.UsuariosCustomer,
                workOrder.CoreWorkflows, workOrder.EntidadesCustomer, workOrder.EntidadesSupplier, workOrder.CoreWorkOrderLifeCycleStatus,
                workOrder.WorkOrderTrackingStatus, workOrder.CoreProjectID);

        public static WorkOrderEntity UpdateProjectId(WorkOrderEntity workOrder, int projectID) =>
            new WorkOrderEntity(workOrder.CoreWorkOrderID, workOrder.Codigo, workOrder.Activos, workOrder.FechaInicio, workOrder.FechaFin, workOrder.Porcentaje, workOrder.FechaCreacion,
                workOrder.FechaUltimaModificacion, workOrder.UsuariosCreador, workOrder.UsuariosModificador, workOrder.UsuariosSupplier, workOrder.UsuariosCustomer,
                workOrder.CoreWorkflows, workOrder.EntidadesCustomer, workOrder.EntidadesSupplier, workOrder.CoreWorkOrderLifeCycleStatus,
                workOrder.WorkOrderTrackingStatus, projectID);
    }
}

