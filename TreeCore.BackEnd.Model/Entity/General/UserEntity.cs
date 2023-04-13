using System;

namespace TreeCore.BackEnd.Model.Entity.General
{
    public class UserEntity : BaseEntity
    {
        public readonly int? UsuarioID;
        public readonly string Nombre;
        public readonly string Apellidos;
        public readonly string EMail;
        public readonly string Clave;
        public readonly bool Activo;
        public readonly bool CambiarClave;
        public readonly DateTime FechaUltimoCambio;
        public readonly string UltimasClaves;
        public readonly string Telefono;
        public readonly int? ClienteID;
        public readonly bool LDAP;
        public readonly DateTime FechaCaducidadUsuario;
        public readonly DateTime FechaCaducidadClave;
        public readonly bool Interno;
        public readonly DateTime FechaUltimoAcceso;
        public readonly string MacDispositivo;
        public readonly string NombreCompleto;
        public readonly DateTime FechaCreacion;
        public readonly DateTime FechaDesactivacion;
        public readonly int? EntidadID;
        public readonly int? DepartamentoID;

        public UserEntity(int? usuarioID, string nombre, string apellidos, string eMail, string clave, bool activo, bool cambiarClave, DateTime fechaUltimoCambio, string ultimasClaves, string telefono, int? clienteID, bool lDAP, DateTime fechaCaducidadUsuario, DateTime fechaCaducidadClave, bool interno, DateTime fechaUltimoAcceso, string macDispositivo, string nombreCompleto, DateTime fechaCreacion, DateTime fechaDesactivacion, int? entidadID, int? departamentoID)
        {
            UsuarioID = usuarioID;
            Nombre = nombre;
            Apellidos = apellidos;
            EMail = eMail;
            Clave = clave;
            Activo = activo;
            CambiarClave = cambiarClave;
            FechaUltimoCambio = fechaUltimoCambio;
            UltimasClaves = ultimasClaves;
            Telefono = telefono;
            ClienteID = clienteID;
            LDAP = lDAP;
            FechaCaducidadUsuario = fechaCaducidadUsuario;
            FechaCaducidadClave = fechaCaducidadClave;
            Interno = interno;
            FechaUltimoAcceso = fechaUltimoAcceso;
            MacDispositivo = macDispositivo;
            NombreCompleto = nombreCompleto;
            FechaCreacion = fechaCreacion;
            FechaDesactivacion = fechaDesactivacion;
            EntidadID = entidadID;
            DepartamentoID = departamentoID;
        }

        protected UserEntity()
        {
        }

        public static UserEntity Create(int? usuarioID, string nombre, string apellidos, string eMail, string clave, 
            bool activo, bool cambiarClave, DateTime fechaUltimoCambio, string ultimasClaves, string telefono, 
            int? clienteID, bool lDAP, DateTime fechaCaducidadUsuario, DateTime fechaCaducidadClave, bool interno, 
            DateTime fechaUltimoAcceso, string macDispositivo, string nombreCompleto, DateTime fechaCreacion, 
            DateTime fechaDesactivacion, int? entidadID, int? departamentoID)
            => new UserEntity(usuarioID, nombre, apellidos, eMail, clave, activo, cambiarClave, fechaUltimoCambio, 
                ultimasClaves, telefono, clienteID, lDAP, fechaCaducidadUsuario, fechaCaducidadClave, interno, 
                fechaUltimoAcceso, macDispositivo, nombreCompleto, fechaCreacion, fechaDesactivacion, entidadID, departamentoID);
        
        public static UserEntity UpdateId(UserEntity user, int id) =>
            new UserEntity(id, user.Nombre, user.Apellidos, user.EMail, user.Clave, user.Activo, user.CambiarClave, user.FechaUltimoCambio,
                user.UltimasClaves, user.Telefono, user.ClienteID, user.LDAP, user.FechaCaducidadUsuario, user.FechaCaducidadClave, user.Interno,
                user.FechaUltimoAcceso, user.MacDispositivo, user.NombreCompleto, user.FechaCreacion, user.FechaDesactivacion, user.EntidadID, user.DepartamentoID);
    }
}
