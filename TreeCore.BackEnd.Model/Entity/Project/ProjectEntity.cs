using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.BusinessProcess;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Model.ValueObject;

namespace TreeCore.BackEnd.Model.Entity.Project
{
    public class ProjectEntity : BaseEntity
    {
        public readonly int? CoreProjectID;
        public readonly int ClienteID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Descripcion;
        public readonly bool Activo;
        public BusinessProcessEntity CoreBusinessProcess;
        public ProgramEntity CoreProgram;
        public ProjectLifeCycleStatusEntity CoreProjectLifeCycleStatus;
        public Budget Budget;
        //public readonly string Activos;
        public readonly DateTime FechaCreacion;
        public UserEntity UsuarioCreador;
        public readonly DateTime FechaUltimaModificacion;
        public UserEntity UsuarioModificador;
        public readonly DateTime? FechaFin;
        public IEnumerable<WorkOrderEntity> WorkOrders;

        public ProjectEntity(int? coreProjectID, int clienteID, string codigo, string nombre, string descripcion, bool activo, BusinessProcessEntity coreBusinesProcess, ProgramEntity coreProgram, ProjectLifeCycleStatusEntity coreProjectLifeCycleStatus, Budget budget, IEnumerable<WorkOrderEntity> Wo, DateTime fechaCreacion, UserEntity usuarioCreador, DateTime fechaUltimaModificacion, UserEntity usuarioModificador, DateTime? fechaFin)
        {
            CoreProjectID = coreProjectID;
            ClienteID = clienteID;
            Codigo = codigo;
            Nombre = nombre;
            Descripcion = descripcion;
            Activo = activo;
            CoreBusinessProcess = coreBusinesProcess;
            CoreProgram = coreProgram;
            CoreProjectLifeCycleStatus = coreProjectLifeCycleStatus;
            Budget = budget;
            //Activos = activos;
            WorkOrders = Wo;
            FechaCreacion = fechaCreacion;
            UsuarioCreador = usuarioCreador;
            FechaUltimaModificacion = fechaUltimaModificacion;
            UsuarioModificador = usuarioModificador;
            FechaFin = fechaFin;
        }

        protected ProjectEntity()
        {
        }

        public static ProjectEntity Create(int clienteID, string codigo, string nombre, string descripcion, bool activo, BusinessProcessEntity coreBusinesProcess, ProgramEntity coreProgram, ProjectLifeCycleStatusEntity coreProjectLifeCycleStatus, Budget budget, IEnumerable<WorkOrderEntity> wo, DateTime fechaCreacion, UserEntity usuarioCreador, DateTime fechaUltimaModificacion, UserEntity usuarioModificador, DateTime? fechaFin)
            => new ProjectEntity(null, clienteID, codigo, nombre, descripcion, activo, coreBusinesProcess, coreProgram, coreProjectLifeCycleStatus, budget, wo, fechaCreacion, usuarioCreador, fechaUltimaModificacion, usuarioModificador, fechaFin);

        public static ProjectEntity UpdateId(ProjectEntity project, int id) =>
            new ProjectEntity(id, project.ClienteID, project.Codigo, project.Nombre, project.Descripcion, project.Activo, project.CoreBusinessProcess, project.CoreProgram, project.CoreProjectLifeCycleStatus, project.Budget, project.WorkOrders, project.FechaCreacion, project.UsuarioCreador, project.FechaUltimaModificacion, project.UsuarioModificador, project.FechaFin);
    }
}
