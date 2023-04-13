using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreAtributosConfiguracionRolesRestringidosController : GeneralBaseController<CoreAtributosConfiguracionRolesRestringidos, TreeCoreContext>
    {
        public CoreAtributosConfiguracionRolesRestringidosController()
            : base()
        { }

        public List<Vw_CoreAtributosConfiguracionRolesRestringidos> GetVwRolesFromAtributo(long lAtributoID) {
            List<Vw_CoreAtributosConfiguracionRolesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_CoreAtributosConfiguracionRolesRestringidos where c.CoreAtributoConfiguracionID == lAtributoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<Vw_CoreAtributosConfiguracionRolesRestringidos> GetVwRolesFromAtributoNoDefecto(long lAtributoID) {
            List<Vw_CoreAtributosConfiguracionRolesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_CoreAtributosConfiguracionRolesRestringidos where c.CoreAtributoConfiguracionID == lAtributoID && c.RolID != null select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public CoreAtributosConfiguracionRolesRestringidos GetDefault(long atributoID)
        {
            CoreAtributosConfiguracionRolesRestringidos oDato;
            try
            {
                oDato = (from c in Context.CoreAtributosConfiguracionRolesRestringidos where c.CoreAtributoConfiguracionID == atributoID && c.RolID == null select c).FirstOrDefault();
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
            List<CoreAtributosConfiguracionRolesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreAtributosConfiguracionRolesRestringidos where c.CoreAtributoConfiguracionID == atributoID && c.RolID == null select c).ToList();
                if (listaDatos != null && listaDatos.Count > 0)
                {
                    existe = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = true;
            }
            return existe;
        }

    }
}