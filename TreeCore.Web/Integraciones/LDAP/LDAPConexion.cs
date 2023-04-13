using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeCore.Integraciones.LDAP
{
    public class LDAPConexion
    {
        public LDAPConexion() : base()
        {
            dominio = "";
            servidor = "";
            usuario = "";
            clave = "";
            conexionid = 0;
        }


        public string dominio { get; set; }


        public string servidor { get; set; }


        public string usuario { get; set; }


        public string clave { get; set; }

        public long conexionid { get; set; }
    }
}