using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class InventarioAtributosPerfilesRestringidosController : GeneralBaseController<InventarioAtributosPerfilesRestringidos, TreeCoreContext>
    {
        public InventarioAtributosPerfilesRestringidosController()
            : base()
        { }

        public List<Vw_InventarioAtributosPerfilesRestringidos> GetAllPerfilesRestringidosAtributo(long atributoID)
        {
            List<Vw_InventarioAtributosPerfilesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_InventarioAtributosPerfilesRestringidos where c.InventariosAtributoID == atributoID orderby c.Oculto descending select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }
        public List<Vw_InventarioAtributosPerfilesRestringidos> GetPerfilesRestringidosAtributo(long atributoID)
        {
            List<Vw_InventarioAtributosPerfilesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_InventarioAtributosPerfilesRestringidos where c.InventariosAtributoID == atributoID && c.PerfilID != null orderby c.Oculto descending select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public InventarioAtributosPerfilesRestringidos GetPerfilRestringidoAtributo(long atributoID, long perfilID)
        {
            InventarioAtributosPerfilesRestringidos oDato;
            try
            {
                oDato = (from c in Context.InventarioAtributosPerfilesRestringidos where c.InventariosAtributoID == atributoID && c.PerfilID == perfilID select c).First();
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
            List<InventarioAtributosPerfilesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioAtributosPerfilesRestringidos where c.InventariosAtributoID == atributoID && c.PerfilID == null select c).ToList();
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

        public InventarioAtributosPerfilesRestringidos GetDefault(long atributoID)
        {
            InventarioAtributosPerfilesRestringidos oDato;
            try
            {
                oDato = (from c in Context.InventarioAtributosPerfilesRestringidos where c.InventariosAtributoID == atributoID && c.PerfilID == null select c).First();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }
            return oDato;
        }

    }
}