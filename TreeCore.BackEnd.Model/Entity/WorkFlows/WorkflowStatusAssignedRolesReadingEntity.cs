using TreeCore.BackEnd.Model.Entity.General;

namespace TreeCore.BackEnd.Model.Entity.WorkFlows
{
    public class WorkflowStatusAssignedRolesReadingEntity : BaseEntity
    {
        public int? CoreEstadoRolLecturaID;
        public RolEntity Roles;
        public WorkFlowStatusEntity CoreWorkflowsEstados;

        public WorkflowStatusAssignedRolesReadingEntity(int? coreEstadoRolLecturaID, RolEntity roles,
            WorkFlowStatusEntity coreWorkflowsEstados)
        {
            CoreEstadoRolLecturaID = coreEstadoRolLecturaID;
            Roles = roles;
            CoreWorkflowsEstados = coreWorkflowsEstados;
        }

        protected WorkflowStatusAssignedRolesReadingEntity() { }

        public static WorkflowStatusAssignedRolesReadingEntity UpdateId(WorkflowStatusAssignedRolesReadingEntity RolesAssigned, int id) =>
            new WorkflowStatusAssignedRolesReadingEntity(id, RolesAssigned.Roles, RolesAssigned.CoreWorkflowsEstados);
    }
}

