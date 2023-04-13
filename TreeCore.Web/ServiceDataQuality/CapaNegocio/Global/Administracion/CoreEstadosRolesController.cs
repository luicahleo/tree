using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreEstadosRolesController : GeneralBaseController<CoreEstadosRoles, TreeCoreContext>
    {
        public CoreEstadosRolesController()
            : base()
        { }

        public List<Vw_CoreEstadosRoles> getRolesByEstadoID(long lEstadoID)
        {
            List<Vw_CoreEstadosRoles> listaDatos;
            List<long> lRolID;

            try
            {
                lRolID = (from c in Context.CoreEstadosRoles where c.CoreEstadoID == lEstadoID select c.CoreEstadoRolID).ToList();
                listaDatos = (from c in Context.Vw_CoreEstadosRoles where (lRolID.Contains(c.CoreEstadoRolID)) orderby c.NombreRol select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<CoreEstadosRoles> getTablaRolesByEstadoID(long lEstadoID)
        {
            List<CoreEstadosRoles> listaDatos;
            List<long> lRolID;

            try
            {
                lRolID = (from c in Context.CoreEstadosRoles where c.CoreEstadoID == lEstadoID select c.CoreEstadoRolID).ToList();
                listaDatos = (from c in Context.CoreEstadosRoles where (lRolID.Contains(c.CoreEstadoRolID)) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public bool RegistroDuplicado(long lEstadoID, long lRolID)
        {
            bool isExiste = false;
            List<CoreEstadosRoles> datos = new List<CoreEstadosRoles>();

            datos = (from c in Context.CoreEstadosRoles where (c.CoreEstadoID == lEstadoID && c.RolID == lRolID) select c).ToList<CoreEstadosRoles>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}