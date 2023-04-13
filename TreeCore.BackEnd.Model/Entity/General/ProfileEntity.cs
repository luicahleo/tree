using System;

namespace TreeCore.BackEnd.Model.Entity.General
{
    public class ProfileEntity : BaseEntity
    {
        public readonly int? PerfilID;
        public readonly int? ClienteID;
        public readonly string Perfil_esES;
        public readonly string Descripcion;
        public readonly bool Activo;
        public readonly string CodigoModulo;
        public readonly string JsonUserFunctionalities;

        public ProfileEntity(int? perfilID, int? clienteID, string perfil_esES, string descripcion, bool activo, string codigoModulo, string jsonUserFunctionalities)
        {
            PerfilID = perfilID;
            ClienteID = clienteID;
            Perfil_esES = perfil_esES;
            Descripcion = descripcion;
            Activo = activo;
            CodigoModulo = codigoModulo;
            JsonUserFunctionalities = jsonUserFunctionalities;
        }

        protected ProfileEntity()
        {
            
        }

        public static ProfileEntity Create(int clienteID, string codigo, string descripcion, bool activo, string codigoModulo, string jsonUserFunctionalities)
            => new ProfileEntity(null, clienteID, codigo, descripcion, activo, codigoModulo, jsonUserFunctionalities);
        public static ProfileEntity UpdateId(ProfileEntity profile, int id) =>
            new ProfileEntity(id, profile.ClienteID, profile.Perfil_esES, profile.Descripcion, profile.Activo, profile.CodigoModulo, profile.JsonUserFunctionalities);
    }
}
