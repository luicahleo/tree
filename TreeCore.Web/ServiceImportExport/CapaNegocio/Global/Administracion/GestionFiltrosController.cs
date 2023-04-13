using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class GestionFiltrosController : GeneralBaseController<GestionFiltros, TreeCoreContext>
    {
        public GestionFiltrosController()
            : base()
        { }

        public List<GestionFiltros> GetFiltros(long usuarioID, string pagina)
        {
            List<GestionFiltros> filtros;

            try
            {
                filtros = (from c
                           in Context.GestionFiltros
                           where c.UsuarioID == usuarioID
                                && c.Pagina == pagina
                           select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                filtros = null;
            }
            return filtros;
        }

        public bool DuplicidadFiltro(string nombre, string pagina, long usuarioID) {
            GestionFiltros filtro;
            bool existe = false;
            try
            {
                filtro = (from c in Context.GestionFiltros where c.Pagina == pagina && c.NombreFiltro == nombre && c.UsuarioID == usuarioID select c).FirstOrDefault();
                if (filtro != null)
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

        public bool DuplicidadFiltroInventario(string nombre, string pagina, long usuarioID, long CatID) {
            DataTable filtro;
            string query;
            bool existe = false;
            try
            {
                query = "select GestionFiltroID, UsuarioID, NombreFiltro, JsonItemsFiltro, Pagina from GestionFiltros where Pagina = '"+pagina+"' and UsuarioID = " + usuarioID + " and Json_Value(JsonItemsFiltro, '$.\"InventarioCategoriaID\"') = " + CatID + " and NombreFiltro = '" + nombre + "'";
                filtro = EjecutarQuery(query);
                if (filtro.Rows.Count > 0)
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