using System;
using System.Collections.Generic;
using TreeCore.BackEnd.Model.Entity.WorkFlows;

namespace TreeCore.BackEnd.Model.Entity.BusinessProcess
{
    public class BusinessProcessEntity : BaseEntity
    {
        public readonly int? CoreBusinessProcessID;
        public readonly int? ClienteID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Descripcion;
        public readonly bool Activo;
        public WorkFlowStatusEntity CoreEstados;
        public IEnumerable<WorkflowEntity> WorkflowsVinculados;
        public BusinessProcessTypeEntity CoreBusinessProcessTipos;

        public BusinessProcessEntity(int? coreBusinessProcessID, int? clienteID, string codigo, string nombre,
            string descripcion, bool activo, WorkFlowStatusEntity coreEstados, IEnumerable<WorkflowEntity> workflowsVinculados,
            BusinessProcessTypeEntity coreBusinessProcessTipos)
        {
            this.CoreBusinessProcessID = coreBusinessProcessID;
            this.ClienteID = clienteID;
            this.Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            this.Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            this.Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            this.Activo = activo;
            this.CoreEstados = coreEstados;
            this.WorkflowsVinculados = workflowsVinculados;
            this.CoreBusinessProcessTipos = coreBusinessProcessTipos;
        }

        protected BusinessProcessEntity() { }

        public static BusinessProcessEntity Create(int id, int clienteID, string codigo, string nombre, string descripcion,
            bool activo, WorkFlowStatusEntity coreEstados, IEnumerable<WorkflowEntity> workflowsVinculados, BusinessProcessTypeEntity coreBusinessProcessTipos)
            => new BusinessProcessEntity(id, clienteID, codigo, nombre, descripcion, activo, coreEstados, workflowsVinculados, coreBusinessProcessTipos);
        public static BusinessProcessEntity UpdateId(BusinessProcessEntity businessProcess, int id) =>
            new BusinessProcessEntity(id, businessProcess.ClienteID, businessProcess.Codigo, businessProcess.Nombre, businessProcess.Descripcion,
                businessProcess.Activo, businessProcess.CoreEstados, businessProcess.WorkflowsVinculados, businessProcess.CoreBusinessProcessTipos);
    }
}
