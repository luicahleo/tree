using TreeCore.BackEnd.Model.Entity.General;

namespace TreeCore.BackEnd.Model.Entity.WorkFlows
{
    public class WorkflowAssignedRolesEntity : BaseEntity
    {
        public int? CoreWorkFlowRolID;
        public RolEntity Roles;
        public WorkflowEntity CoreWorkflows;

        public WorkflowAssignedRolesEntity(int? coreworkflowrolid, RolEntity roles,
            WorkflowEntity coreworkflows)
        {
            CoreWorkFlowRolID = coreworkflowrolid;
            Roles = roles;
            CoreWorkflows = coreworkflows;
        }

        protected WorkflowAssignedRolesEntity() { }

        public static WorkflowAssignedRolesEntity UpdateId(WorkflowAssignedRolesEntity RolesAssigned, int id) =>
            new WorkflowAssignedRolesEntity(id, RolesAssigned.Roles, RolesAssigned.CoreWorkflows);
    }
}
