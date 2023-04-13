using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CategoriasTablasController : GeneralBaseController<CategoriasTablas, TreeCoreContext>
    {
        public CategoriasTablasController()
            : base()
        { }


        public CategoriasTablas GetCategoriaByNombre(string sNombre)
        {
            List<CategoriasTablas> lista = null;
            CategoriasTablas dato = null;
            lista = (from c in Context.CategoriasTablas where (c.CategoriaNombre == sNombre) select c).ToList();
            if (lista != null && lista.Count > 0)
            {
                dato = lista.ElementAt(0);
            }

            return dato;
        }

        public bool RegistroDuplicado(string categoria)
        {
            bool isExiste = false;
            List<CategoriasTablas> datos = new List<CategoriasTablas>();


            datos = (from c in Context.CategoriasTablas where (c.CategoriaNombre == categoria) select c).ToList<CategoriasTablas>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}