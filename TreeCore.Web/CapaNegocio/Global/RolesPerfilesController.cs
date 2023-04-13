using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class RolesPerfilesController : GeneralBaseController<RolesPerfiles, TreeCoreContext>
    {
        public RolesPerfilesController()
            : base()
        { }

        public RolesPerfiles GetRolesPerfilesByPerfilRol(long rolID, long perfilID)
        {
            List<RolesPerfiles> roles;
            RolesPerfiles dato=null;

            roles = (from c in Context.RolesPerfiles where c.RolID == rolID && c.PerfilID== perfilID select c).ToList<RolesPerfiles>();

            if (roles.Count > 0)
            {
                dato = roles.First();
            }

            return dato;
        }

        public bool RegistroDuplicado(long RolID, long PerfilID)
        {
            bool isExiste = false;
            List<RolesPerfiles> datos;

            datos = (from c in Context.RolesPerfiles where (c.RolID == RolID && c.PerfilID == PerfilID) select c).ToList<RolesPerfiles>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

    }
}