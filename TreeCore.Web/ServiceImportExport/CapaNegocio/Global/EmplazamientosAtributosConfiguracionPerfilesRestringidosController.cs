using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class EmplazamientosAtributosConfiguracionRolesRestringidosController : GeneralBaseController<EmplazamientosAtributosRolesRestringidos, TreeCoreContext>
    {
        public EmplazamientosAtributosConfiguracionRolesRestringidosController()
            : base()
        { }

        public List<Vw_EmplazamientosAtributosRolesRestringidos> GetAllRolesRestringidosAtributo(long atributoID)
        {
            List<Vw_EmplazamientosAtributosRolesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_EmplazamientosAtributosRolesRestringidos where c.EmplazamientoAtributoConfiguracionID == atributoID orderby c.Restriccion descending select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }
        public List<Vw_EmplazamientosAtributosRolesRestringidos> GetRolesRestringidosAtributo(long atributoID)
        {
            List<Vw_EmplazamientosAtributosRolesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_EmplazamientosAtributosRolesRestringidos where c.EmplazamientoAtributoConfiguracionID == atributoID && c.RolID != null orderby c.Restriccion descending select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public EmplazamientosAtributosRolesRestringidos GetPerfilRestringidoAtributo(long atributoID, long RolID)
        {
            EmplazamientosAtributosRolesRestringidos oDato;
            try
            {
                oDato = (from c in Context.EmplazamientosAtributosRolesRestringidos where c.EmplazamientoAtributoConfiguracionID == atributoID && c.EmplazamientoAtributoRolRestringidoID == RolID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }
        public bool ExisteDefaultAtributo(long atributoID)
        {
            bool existe = false;
            List<EmplazamientosAtributosRolesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.EmplazamientosAtributosRolesRestringidos where c.EmplazamientoAtributoConfiguracionID == atributoID && c.RolID == null select c).ToList();
                if (listaDatos != null && listaDatos.Count > 0)
                {
                    existe = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = true;
                throw;
            }
            return existe;
        }
        public EmplazamientosAtributosRolesRestringidos GetDefault(long atributoID)
        {
            List<EmplazamientosAtributosRolesRestringidos> listaDatos;
            EmplazamientosAtributosRolesRestringidos oDato;
            try
            {
                listaDatos = (from c in Context.EmplazamientosAtributosRolesRestringidos where c.EmplazamientoAtributoConfiguracionID == atributoID && c.RolID == null select c).ToList();
                if (listaDatos.Count > 0)
                {
                    oDato = listaDatos.First();
                }
                else
                {
                    oDato = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

    }
}