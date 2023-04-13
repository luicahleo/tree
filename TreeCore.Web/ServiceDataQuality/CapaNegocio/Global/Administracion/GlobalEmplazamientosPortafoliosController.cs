using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{

    public class GlobalEmplazamientosPortafoliosController : GeneralBaseController<GlobalEmplazamientosPortafolios, TreeCoreContext>
    {
        public GlobalEmplazamientosPortafoliosController()
            : base()
        {

        }


        public GlobalEmplazamientosPortafolios GetPortafolioByNombre(string nombre)
        {
            GlobalEmplazamientosPortafolios dato = null;
            List<GlobalEmplazamientosPortafolios> lista = new List<GlobalEmplazamientosPortafolios>();

            lista = (from c in Context.GlobalEmplazamientosPortafolios where c.Portafolio == nombre && c.Activo select c).ToList<GlobalEmplazamientosPortafolios>();

            if (lista != null && lista.Count > 0)
            {
                dato = lista.ElementAt(0);
            }

            return dato;
        }

    }

}