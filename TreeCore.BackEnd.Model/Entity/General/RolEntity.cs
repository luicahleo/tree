using System;
using System.Collections.Generic;

namespace TreeCore.BackEnd.Model.Entity.General
{
    public class RolEntity : BaseEntity
    {
        public readonly int? RolID;
        public readonly int ClienteID;
        public readonly string Codigo;
        public readonly string Nombre;
        public readonly string Descripcion;
        public readonly bool Activo;
        public IEnumerable<ProfileEntity> Profiles;

        public RolEntity(int? rolID, int clienteID, string codigo, string nombre, string descripcion, bool activo, IEnumerable<ProfileEntity> profiles)
        {
            RolID = rolID;
            ClienteID = clienteID;
            Codigo = codigo;
            Nombre = nombre;
            Descripcion = descripcion;
            Activo = activo;
            Profiles = profiles;
        }

        protected RolEntity()
        {
            
        }

        public static RolEntity Create(int clienteID, string codigo, string nombre, string descripcion, bool activo, IEnumerable<ProfileEntity> profiles)
            => new RolEntity(null, clienteID, codigo, nombre, descripcion, activo, profiles);
        public static RolEntity UpdateId(RolEntity rol, int id) =>
            new RolEntity(id, rol.ClienteID, rol.Codigo, rol.Nombre, rol.Descripcion, rol.Activo, rol.Profiles);
    }
}
