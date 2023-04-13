
namespace TreeCore.BackEnd.Model.Entity.WorkFlows
{
    public class WorkFlowNextStatusEntity: BaseEntity
    {
        public int? CoreEstadoSiguienteID;
        public WorkFlowStatusEntity? WorkFlowStatus;
        public bool Defecto;
        public WorkFlowStatusEntity WorkFlowNextStatus;

        public WorkFlowNextStatusEntity(int? coreEstadoSiguienteID, WorkFlowStatusEntity? workFlowStatus,
            WorkFlowStatusEntity workFlowNextStatus, bool defecto)
        {
            CoreEstadoSiguienteID = coreEstadoSiguienteID;
            WorkFlowStatus = workFlowStatus;
            WorkFlowNextStatus = workFlowNextStatus;
            Defecto = defecto;
        }

        protected WorkFlowNextStatusEntity() { }

        public static WorkFlowNextStatusEntity UpdateId(WorkFlowNextStatusEntity workFlowNextStatus, int id) =>
            new WorkFlowNextStatusEntity(id, workFlowNextStatus.WorkFlowStatus, workFlowNextStatus.WorkFlowNextStatus, workFlowNextStatus.Defecto);
    }
}
