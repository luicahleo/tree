using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
using System.Linq;

namespace CapaNegocio
{
    public class EmplazamientosAtributosCategoriasController : GeneralBaseController<EmplazamientosAtributosCategorias, TreeCoreContext>
    {
        public EmplazamientosAtributosCategoriasController()
            : base()
        {

        }
        public bool RegistroDuplicado(string sNombre, long lClienteID)
        {
            bool bExiste = false;
            List<EmplazamientosAtributosCategorias> listaDatos = new List<EmplazamientosAtributosCategorias>();

            listaDatos = (from c in Context.EmplazamientosAtributosCategorias
                          where (c.Nombre == sNombre) && c.ClienteID == lClienteID
                          select c).ToList<EmplazamientosAtributosCategorias>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public long GetOrdenCategoria(long lCatID, long lClienteID) {
            long orden = 0;
            try
            {
                orden = (from c in Context.EmplazamientosAtributosConfiguracion where c.EmplazamientoAtributoCategoriaID == lCatID && c.ClienteID == lClienteID select c).First().OrdenCategoria;
            }
            catch (Exception)
            {
                orden = 0;
            }
            return orden;
        }

        public List<EmplazamientosAtributosCategorias> getCategoriasNoSeleccionadas(long lClienteID) {
            List<EmplazamientosAtributosCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.EmplazamientosAtributosCategorias
                              where c.ClienteID == lClienteID && !(from x in Context.EmplazamientosAtributosConfiguracion where x.ClienteID == lClienteID select x.EmplazamientoAtributoCategoriaID).Contains(c.EmplazamientoAtributoCategoriaID)
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<EmplazamientosAtributosCategorias> getCategoriasSeleccionadas(long lClienteID) {
            List<EmplazamientosAtributosCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.EmplazamientosAtributosCategorias
                              where c.ClienteID == lClienteID && (from x in Context.EmplazamientosAtributosConfiguracion where x.ClienteID == lClienteID select x.EmplazamientoAtributoCategoriaID).Contains(c.EmplazamientoAtributoCategoriaID)
                              select c).ToList();
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