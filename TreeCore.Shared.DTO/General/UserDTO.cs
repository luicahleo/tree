using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TreeCore.Shared.DTO.General
{
    public class UserDTO : BaseDTO
    {
        [Required] public string Name;
        [Required] public string Surnames;
        [Required] public string Email;
        [Required] public string Password;
        [Required] public bool Active;
        //public bool ChangeKey CambiarClave;
        public DateTime DateLastChange;
        //public string UltimasClaves;
        [Required] public string Phone;
        //public int Client ClienteID;
        //public bool LDAP;
        public DateTime ExpiryUserDate;
        public DateTime ExpiryPasswordDate;
        //public bool Internal;
        public DateTime LastAccessDate;
        //public string MacDispositivo;
        public string FullName;
        public DateTime CreationDate;
        public DateTime DeactivationDate;
        //public int? EntidadID;
        //public int? DepartamentoID;


        public UserDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Surnames).ToLower(), "Apellidos");
            map.Add(nameof(Email).ToLower(), "EMail");
            map.Add(nameof(Password).ToLower(), "Clave");
            map.Add(nameof(Active).ToLower(), "Activo");
            map.Add(nameof(DateLastChange).ToLower(), "FechaUltimoCambio");
            map.Add(nameof(Phone).ToLower(), "Telefono");
            map.Add(nameof(ExpiryUserDate).ToLower(), "FechaCaducidadUsuario");
            map.Add(nameof(ExpiryPasswordDate).ToLower(), "FechaCaducidadClave");
            //map.Add(nameof(Internal).ToLower(), "Interno");
            map.Add(nameof(LastAccessDate).ToLower(), "FechaUltimoAcceso");
            map.Add(nameof(FullName).ToLower(), "NombreCompleto");
            map.Add(nameof(CreationDate).ToLower(), "FechaCreacion");
            map.Add(nameof(DeactivationDate).ToLower(), "FechaDesactivacion");
        }
    }
}
