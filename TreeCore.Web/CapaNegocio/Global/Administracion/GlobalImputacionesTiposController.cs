using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{

    public class GlobalImputacionesTiposController : GeneralBaseController<GlobalImputacionesTipos, TreeCoreContext>
    {
        public GlobalImputacionesTiposController()
            : base()
        {

        }

        public GlobalImputacionesTipos GetImputacionByNombre(string nombre)
        {
            GlobalImputacionesTipos dato = null;
            List<GlobalImputacionesTipos> lista = new List<GlobalImputacionesTipos>();

            lista = (from c in Context.GlobalImputacionesTipos where c.ImputacionTipo == nombre && c.Activo select c).ToList<GlobalImputacionesTipos>();

            if (lista != null && lista.Count > 0)
            {
                dato = lista.ElementAt(0);
            }

            return dato;
        }

    }

}