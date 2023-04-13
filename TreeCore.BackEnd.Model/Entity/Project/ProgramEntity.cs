using System;
using System.Collections.Generic;
using TreeCore.BackEnd.Model.Entity.Project;
//using TreeCore.BackEnd.Model.Entity.WorkFlows;

namespace TreeCore.BackEnd.Model.Entity.Project
{
    public class ProgramEntity : BaseEntity
    {
        public readonly int? CoreProgramID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly int? ClienteID;

        public ProgramEntity(int? programID, string codigo, string nombre, string descripcion, bool activo, int? clienteID)
        {
            this.CoreProgramID = programID;
            this.Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            this.Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            this.Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            this.Activo = activo;
            this.ClienteID = clienteID;

        }

        protected ProgramEntity() { }

        public static ProgramEntity Create(int id, string codigo, string nombre, string descripcion, bool activo, int clienteID) 
            => new ProgramEntity(id, codigo, nombre, descripcion, activo, clienteID);
        public static ProgramEntity UpdateId(ProgramEntity businessProcess, int id) =>
            new ProgramEntity(id, businessProcess.Codigo, businessProcess.Nombre, businessProcess.Descripcion, businessProcess.Activo, businessProcess.ClienteID);
    }
}
