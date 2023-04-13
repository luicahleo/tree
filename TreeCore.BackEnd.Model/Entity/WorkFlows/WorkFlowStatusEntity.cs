using System;
using System.Collections.Generic;
using TreeCore.BackEnd.Model.Entity.General;

namespace TreeCore.BackEnd.Model.Entity.WorkFlows
{
    public class WorkFlowStatusEntity : BaseEntity
    {
        public int? CoreEstadoID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Descripcion;
        public readonly int Tiempo;
        public readonly bool Completado;
        public readonly bool PublicoLectura;
        public readonly bool PublicoEscritura;
        public WorkFlowStatusGroupEntity EstadosAgrupaciones;
        public readonly bool Activo;
        public readonly bool Defecto;
        public IEnumerable<WorkFlowNextStatusEntity> EstadosSiguientes;
        public IEnumerable<RolEntity> EstadosRolesEscritura;
        public IEnumerable<RolEntity> EstadosRolesLectura;
        public WorkflowEntity WorkFlow;

        public WorkFlowStatusEntity(int? coreEstadoID, string codigo, string nombre, string descripcion,int tiempo,
            bool completado,bool publicoLectura, bool publicoEscritura, WorkFlowStatusGroupEntity estadoAgrupacion, bool activo, 
            bool defecto, WorkflowEntity? workflow, IEnumerable<WorkFlowNextStatusEntity> estadosSiguientes,
            IEnumerable<RolEntity> estadosRolesEscritura, IEnumerable<RolEntity> estadosRolesLectura)
        {
            CoreEstadoID = coreEstadoID;
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Descripcion = descripcion;
            Tiempo = tiempo;
            Completado = completado;
            PublicoLectura = publicoLectura;
            PublicoEscritura = publicoEscritura;
            EstadosAgrupaciones = estadoAgrupacion;
            Activo = activo;
            Defecto = defecto;
            WorkFlow = workflow;
            EstadosSiguientes = estadosSiguientes;
            EstadosRolesEscritura = estadosRolesEscritura;
            EstadosRolesLectura = estadosRolesLectura;
        }

        protected WorkFlowStatusEntity()
        {
        }

        public static WorkFlowStatusEntity Create(int id, string codigo, string nombre, string descripcion, int tiempo,
            bool completado, bool publicoLectura, bool publicoEscritura, WorkFlowStatusGroupEntity estadoAgrupacion, bool activo, 
            bool defecto, WorkflowEntity workflow, IEnumerable<WorkFlowNextStatusEntity> estadosSiguientes,
            IEnumerable<RolEntity> estadosRolesEscritura, IEnumerable<RolEntity> estadosRolesLectura)
            => new WorkFlowStatusEntity(id, codigo, nombre, descripcion,tiempo,completado,publicoLectura,publicoEscritura,
                estadoAgrupacion, activo, defecto, workflow, estadosSiguientes, estadosRolesEscritura, estadosRolesLectura);

        public static WorkFlowStatusEntity UpdateId(WorkFlowStatusEntity status, int id) =>
            new WorkFlowStatusEntity(id, status.Codigo, status.Nombre, status.Descripcion,status.Tiempo,status.Completado,status.PublicoLectura,
                status.PublicoEscritura,status.EstadosAgrupaciones, status.Activo, status.Defecto, status.WorkFlow, status.EstadosSiguientes,
                status.EstadosRolesEscritura, status.EstadosRolesLectura);
    }
}
