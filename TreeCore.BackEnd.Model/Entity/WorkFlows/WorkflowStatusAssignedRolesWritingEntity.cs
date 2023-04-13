using TreeCore.BackEnd.Model.Entity.General;

namespace TreeCore.BackEnd.Model.Entity.WorkFlows
{
    public class WorkflowStatusAssignedRolesWritingEntity : BaseEntity
    {
        public int? CoreEstadoRolEscrituraID;
        public RolEntity Roles;
        public WorkFlowStatusEntity CoreWorkflowsEstados;

        public WorkflowStatusAssignedRolesWritingEntity(int? coreEstadosRolEscrituraID, RolEntity roles,
            WorkFlowStatusEntity coreWorkflowsEstados)
        {
            CoreEstadoRolEscrituraID = coreEstadosRolEscrituraID;
            Roles = roles;
            CoreWorkflowsEstados = coreWorkflowsEstados;
        }

        protected WorkflowStatusAssignedRolesWritingEntity() { }

        public static WorkflowStatusAssignedRolesWritingEntity UpdateId(WorkflowStatusAssignedRolesWritingEntity RolesAssigned, int id) =>
            new WorkflowStatusAssignedRolesWritingEntity(id, RolesAssigned.Roles, RolesAssigned.CoreWorkflowsEstados);
    }
}
