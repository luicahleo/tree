using System;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.WorkFlows;

namespace TreeCore.BackEnd.Model.Entity.WorkOrders
{
    public class WorkOrderTrackingStatusEntity : BaseEntity
    {
        public int? CoreWorkOrderTrackingStatusID;
        public readonly string Codigo;
        public WorkOrderEntity WorkOrders;
        public WorkOrderTrackingStatusEntity? PreviusCoreWorkOrderTrackingStatus;
        public UserEntity AssignedUsuario;
        public WorkFlowStatusEntity Estado;
        public readonly DateTime FechaCreaccion;
        public UserEntity UsuarioCreador;



        public WorkOrderTrackingStatusEntity(int? coreWorkOrderEstadoID, string codigo, WorkOrderEntity workOrders, WorkOrderTrackingStatusEntity? previusCoreWorkOrderTrackingStatus, UserEntity assignedUsuario, WorkFlowStatusEntity estado, DateTime fechaCreaccion, UserEntity usuarioCreador)
        {
            CoreWorkOrderTrackingStatusID = coreWorkOrderEstadoID;
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            WorkOrders = workOrders;
            PreviusCoreWorkOrderTrackingStatus = previusCoreWorkOrderTrackingStatus;
            AssignedUsuario = assignedUsuario;
            Estado = estado;
            FechaCreaccion = fechaCreaccion;
            UsuarioCreador = usuarioCreador;

        }

        protected WorkOrderTrackingStatusEntity()
        {
        }

        public static WorkOrderTrackingStatusEntity Create(int id, string codigo, WorkOrderEntity workOrders, WorkOrderTrackingStatusEntity? previusCoreWorkOrderTrackingStatus, UserEntity assignedUsuario, WorkFlowStatusEntity estado, DateTime fechaCreaccion, UserEntity usuarioCreador)
            => new WorkOrderTrackingStatusEntity(id, codigo, workOrders, previusCoreWorkOrderTrackingStatus, assignedUsuario, estado, fechaCreaccion, usuarioCreador);

        public static WorkOrderTrackingStatusEntity UpdateId(WorkOrderTrackingStatusEntity trackingStatus, int id) =>
            new WorkOrderTrackingStatusEntity(id, trackingStatus.Codigo, trackingStatus.WorkOrders, trackingStatus.PreviusCoreWorkOrderTrackingStatus, trackingStatus.AssignedUsuario, trackingStatus.Estado, trackingStatus.FechaCreaccion, trackingStatus.UsuarioCreador);
    }
}
