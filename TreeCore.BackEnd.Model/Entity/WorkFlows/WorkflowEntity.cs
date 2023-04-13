using System;
using System.Collections.Generic;
using TreeCore.BackEnd.Model.Entity.General;

namespace TreeCore.BackEnd.Model.Entity.WorkFlows
{
    public class WorkflowEntity : BaseEntity
    {
        public readonly int? CoreWorkFlowID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly bool Publico;
        public IEnumerable<WorkFlowStatusEntity> WorkflowsEstados;
        public IEnumerable<RolEntity> WorkflowsRoles;

        public WorkflowEntity(int? coreWorkFlowID, int? clienteID, string codigo, string nombre, string descripcion, bool activo, bool publico,
            IEnumerable<WorkFlowStatusEntity> workflowsEstados, IEnumerable<RolEntity> workflowsRoles)
        {
            CoreWorkFlowID = coreWorkFlowID;
            ClienteID = clienteID;
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Descripcion = descripcion;
            Activo = activo;
            Publico = publico;
            WorkflowsEstados = workflowsEstados;
            WorkflowsRoles = workflowsRoles;
        }

        protected WorkflowEntity()
        {
        }

        public static WorkflowEntity Create(int id, int clienteID, string codigo, string nombre, string descripcion,
            bool activo, bool publico, IEnumerable<WorkFlowStatusEntity> workflowsEstados, IEnumerable<RolEntity> workflowsRoles)
            => new WorkflowEntity(id, clienteID, codigo, nombre, descripcion, activo, publico, workflowsEstados, workflowsRoles);

        public static WorkflowEntity UpdateId(WorkflowEntity workflow, int id) =>
            new WorkflowEntity(id, workflow.ClienteID, workflow.Codigo, workflow.Nombre, workflow.Descripcion, workflow.Activo, workflow.Publico,
                workflow.WorkflowsEstados, workflow.WorkflowsRoles);
    }
}
