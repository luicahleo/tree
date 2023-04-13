using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class RolesController : GeneralBaseController<Roles, TreeCoreContext>
    {
        public RolesController()
            : base()
        { }

        public List<Roles> GetActivos(long? lClienteID)
        {
            List<Roles> roles = new List<Roles>();
            roles = (from c in Context.Roles where c.ClienteID == lClienteID && c.Activo select c).ToList();
            return roles;
        }
        public List<Roles> GetByClienteID(long? lClienteID)
        {
            List<Roles> roles = new List<Roles>();
            roles = (from c in Context.Roles where c.ClienteID == lClienteID select c).ToList();
            return roles;
        }

        public List<Roles> GetActivosNoAsignados(long lClienteID, long DocumentoTipo)
        {
            List<long?> roles;

            roles = (from c in Context.DocumentosTiposRoles where c.TipoDocumentoID == DocumentoTipo select c.RolID).ToList<long?>();

            return (from c in Context.Roles where (!(roles.Contains(c.RolID))) && c.ClienteID == lClienteID && c.Activo == true select c).OrderBy("Nombre").ToList<Roles>();
        }

        public bool registroDuplicado(string nombre, string codigo, long lClienteID)
        {
            List<Roles> roles = new List<Roles>();
            bool control = false;
            roles = (from c in Context.Roles where c.ClienteID == lClienteID && (c.Nombre == nombre || c.Codigo == codigo) select c).ToList();

            if (roles.Count > 0)
            {
                control = true;
            }
            return control;
        }

        public bool registroDuplicadoNombre(string nombre, long lClienteID)
        {
            List<Roles> roles = new List<Roles>();
            bool control = false;
            roles = (from c in Context.Roles where c.ClienteID == lClienteID && (c.Nombre == nombre) select c).ToList();

            if (roles.Count > 0)
            {
                control = true;
            }
            return control;
        }

        public bool registroDuplicadoCodigo(string codigo, long lClienteID)
        {
            List<Roles> roles = new List<Roles>();
            bool control = false;
            roles = (from c in Context.Roles where c.ClienteID == lClienteID && (c.Codigo == codigo) select c).ToList();

            if (roles.Count > 0)
            {
                control = true;
            }
            return control;
        }

        public List<Roles> GetRolesFromUsuario(long UsuarioID)
        {
            List<Roles> roles = new List<Roles>();
            try
            {
                roles = (from c in Context.Roles join usu in Context.UsuariosRoles on c.RolID equals usu.RolID where usu.UsuarioID == UsuarioID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                roles = null;
            }
            return roles;
        }

        public List<Vw_UsuariosRoles> GetAllRolesByUsuario(long UsuarioID)
        {
            List<Vw_UsuariosRoles> roles = new List<Vw_UsuariosRoles>();
            try
            {
                roles = (from c in Context.Vw_UsuariosRoles where c.UsuarioID == UsuarioID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                roles = null;
            }
            return roles;
        }

        public List<Roles> GetRolesLibresInventarioAtributosRestringidos(long atributoID)
        {
            List<Roles> perfiles;
            try
            {
                List<long> asignados = (from c in Context.InventarioAtributosRolesRestringidos where c.InventarioAtributoID == atributoID && c.RolID != null select c.RolID.Value).ToList();
                perfiles = (from c in Context.Roles where !(asignados.Contains(c.RolID)) orderby c.Nombre select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                perfiles = null;
            }
            return perfiles;
        }

        public List<Roles> GetRolesLibresEmplazamientosAtributosRestringidos(long atributoID)
        {
            List<Roles> perfiles;
            try
            {
                List<long> asignados = (from c in Context.EmplazamientosAtributosRolesRestringidos where c.EmplazamientoAtributoConfiguracionID == atributoID && c.RolID != null select c.RolID.Value).ToList();
                perfiles = (from c in Context.Roles where !(asignados.Contains(c.RolID)) orderby c.Nombre select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                perfiles = null;
            }
            return perfiles;
        }
        public List<Roles> GetRolesLibresAtributosRestringidos(long atributoID)
        {
            List<Roles> perfiles;
            try
            {
                List<long?> asignados = (from c in Context.CoreAtributosConfiguracionRolesRestringidos where c.CoreAtributoConfiguracionID == atributoID && c.RolID != null select c.RolID).ToList();
                perfiles = (from c in Context.Roles
                            join atr in Context.CoreAtributosConfiguraciones on c.ClienteID equals atr.ClienteID
                            where !(asignados.Contains(c.RolID)) && atr.CoreAtributoConfiguracionID == atributoID
                            orderby c.Nombre
                            select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                perfiles = null;
            }
            return perfiles;
        }

        public List<Roles> GetRolesLibres(long lEstadoID)
        {
            List<Roles> perfiles;

            try
            {
                List<long> asignados = (from c in Context.CoreEstadosRoles where c.CoreEstadoID == lEstadoID && c.RolID != null select c.RolID.Value).ToList();
                perfiles = (from c in Context.Roles where !(asignados.Contains(c.RolID)) orderby c.Codigo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                perfiles = null;
            }

            return perfiles;
        }

        public long getIDByCodigo(string sCodigo)
        {
            long lRolID;

            try
            {
                lRolID = (from c in Context.Roles where c.Codigo == sCodigo select c.RolID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lRolID = 0;
            }

            return lRolID;
        }

        public string getCodigoByID (long lRolID)
        {
            string sCodigo;

            try
            {
                sCodigo = (from c in Context.Roles where c.RolID == lRolID select c.Codigo).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sCodigo = null;
            }

            return sCodigo;
        }

        public List<Roles> getRolesAsignados (List<long> listaIDs)
        {
            List<Roles> listaDatos = new List<Roles>();
            Roles oDato;

            try
            {
                if (listaIDs != null)
                {
                    for (int i = 0; i < listaIDs.Count; i++)
                    {
                        oDato = (from c in Context.Roles where c.RolID == listaIDs[i] select c).First();
                        listaDatos.Add(oDato);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }
    }
}