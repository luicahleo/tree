using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{

    public class AlquileresDetallesEquiposAdicionalesController : GeneralBaseController<AlquileresDetallesEquiposAdicionales, TreeCoreContext>
    {
        public AlquileresDetallesEquiposAdicionalesController()
            : base()
        {

        }

        public AlquileresDetallesEquiposAdicionales GetEquipoAdicionalByNombre(string nombre)
        {
            AlquileresDetallesEquiposAdicionales dato = null;
            List<AlquileresDetallesEquiposAdicionales> lista = new List<AlquileresDetallesEquiposAdicionales>();

            lista = (from c in Context.AlquileresDetallesEquiposAdicionales where c.AlquilerDetalleEquipoAdiciona == nombre && c.Activo select c).ToList<AlquileresDetallesEquiposAdicionales>();

            if (lista != null && lista.Count > 0)
            {
                dato = lista.ElementAt(0);
            }

            return dato;
        }

    }

}