using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class EmplazamientosAtributosConfiguracionPerfilesRestringidosController : GeneralBaseController<EmplazamientosAtributosPerfilesRestringidos, TreeCoreContext>
    {
        public EmplazamientosAtributosConfiguracionPerfilesRestringidosController()
            : base()
        { }

        public List<Vw_EmplazamientosAtributosPerfilesRestringidos> GetAllPerfilesRestringidosAtributo(long atributoID)
        {
            List<Vw_EmplazamientosAtributosPerfilesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_EmplazamientosAtributosPerfilesRestringidos where c.EmplazamientoAtributoConfiguracionID == atributoID orderby c.Oculto descending select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }
        public List<Vw_EmplazamientosAtributosPerfilesRestringidos> GetPerfilesRestringidosAtributo(long atributoID)
        {
            List<Vw_EmplazamientosAtributosPerfilesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_EmplazamientosAtributosPerfilesRestringidos where c.EmplazamientoAtributoConfiguracionID == atributoID && c.PerfilID != null orderby c.Oculto descending select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public EmplazamientosAtributosPerfilesRestringidos GetPerfilRestringidoAtributo(long atributoID, long perfilID)
        {
            EmplazamientosAtributosPerfilesRestringidos oDato;
            try
            {
                oDato = (from c in Context.EmplazamientosAtributosPerfilesRestringidos where c.EmplazamientoAtributoConfiguracionID == atributoID && c.EmplazamientoAtributoPerfilRestringidoID == perfilID select c).First();
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
            List<EmplazamientosAtributosPerfilesRestringidos> listaDatos;
            try
            {
                listaDatos = (from c in Context.EmplazamientosAtributosPerfilesRestringidos where c.EmplazamientoAtributoConfiguracionID == atributoID && c.PerfilID == null select c).ToList();
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
        public EmplazamientosAtributosPerfilesRestringidos GetDefault(long atributoID)
        {
            EmplazamientosAtributosPerfilesRestringidos oDato;
            try
            {
                oDato = (from c in Context.EmplazamientosAtributosPerfilesRestringidos where c.EmplazamientoAtributoConfiguracionID == atributoID && c.PerfilID == null select c).First();
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