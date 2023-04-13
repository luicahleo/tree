using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public sealed class PerfilesGruposController : GeneralBaseController<PerfilesGrupos, TreeCoreContext>
    {
        public PerfilesGruposController()
            : base()
        {

        }

        public List<PerfilesGrupos> GetPerfilesByGrupo(string grupo)
        {
            // Local variables
            List<PerfilesGrupos> lista = null;


            // Obtains the information
            lista = (from c in Context.PerfilesGrupos where (c.Grupo == grupo) select c).ToList();

            if (lista == null)
            {
                lista = new List<PerfilesGrupos>();
            }
            return lista;

        }

        public PerfilesGrupos GetItemByPerfilID(long perfilID)
        {
            // Local variables
            List<PerfilesGrupos> lista = null;
            PerfilesGrupos dato = null;


            // Obtains the information
            lista = (from c in Context.PerfilesGrupos where (c.PerfilID == perfilID) select c).ToList();

            if (lista != null && lista.Count > 0)
            {
                dato = lista.ElementAt(0);
            }

            return dato;

        }
    }
}