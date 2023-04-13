using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeCore.Integraciones.LDAP
{
    public class UsuarioLDAP
    {
        public string usuario = null;
        public string path = null;
        public long usuarioLDAPID = 0;

        public UsuarioLDAP()
            : base()
        {

        }

        public long GetUsuarioLDAPID()
        {
            return usuarioLDAPID;
        }

        public void SetUsuarioLDAPID(long usuarioIDLocal)
        {
            usuarioLDAPID = usuarioIDLocal;
        }

        public string GetUsuario()
        {
            return usuario;
        }

        public void SetUsuario(string usuarioLocal)
        {
            usuario = usuarioLocal;
        }

        public string GetPath()
        {
            return path;
        }

        public void SetPath(string pathLocal)
        {
            path = pathLocal;
        }
    }
}