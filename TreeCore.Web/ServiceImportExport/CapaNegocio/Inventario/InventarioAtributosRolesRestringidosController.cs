using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class InventarioAtributosRolesRestringidosController : GeneralBaseController<InventarioAtributosRolesRestringidos, TreeCoreContext>
    {
        public InventarioAtributosRolesRestringidosController()
            : base()
        { }

        public List<Vw_InventarioAtributosRolesRestringidos> GetAllRolesRestringidosAtributo(long atributoID)
        {
            List<Vw_InventarioAtributosRolesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_InventarioAtributosRolesRestringidos where c.InventarioAtributoID == atributoID orderby c.Restriccion descending select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }
        public List<Vw_InventarioAtributosRolesRestringidos> GetRolesRestringidosAtributo(long atributoID)
        {
            List<Vw_InventarioAtributosRolesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_InventarioAtributosRolesRestringidos where c.InventarioAtributoID == atributoID && c.RolID != null orderby c.Restriccion descending select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public InventarioAtributosRolesRestringidos GetPerfilRestringidoAtributo(long atributoID, long RolID)
        {
            InventarioAtributosRolesRestringidos oDato;
            try
            {
                oDato = (from c in Context.InventarioAtributosRolesRestringidos where c.InventarioAtributoID == atributoID && c.RolID == RolID select c).First();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }
            return oDato;
        }

        public bool ExisteDefaultAtributo(long atributoID)
        {
            bool existe = false;
            List<InventarioAtributosRolesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioAtributosRolesRestringidos where c.InventarioAtributoID == atributoID && c.RolID == null select c).ToList();
                if (listaDatos != null && listaDatos.Count > 0)
                {
                    existe = true;
                }
            }
            catch (Exception ex)
            {
                existe = true;
                log.Error(ex.Message);
            }
            return existe;
        }

        public InventarioAtributosRolesRestringidos GetDefault(long atributoID)
        {
            InventarioAtributosRolesRestringidos oDato;
            try
            {
                oDato = (from c in Context.InventarioAtributosRolesRestringidos where c.InventarioAtributoID == atributoID && c.RolID == null select c).First();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }
            return oDato;
        }

        public List<CoreAtributosConfiguracionRolesRestringidos> GetRolesByRolID(long rolID)
        {
            return (from c in Context.CoreAtributosConfiguracionRolesRestringidos where c.RolID == rolID select c).ToList();
        }

    }
}