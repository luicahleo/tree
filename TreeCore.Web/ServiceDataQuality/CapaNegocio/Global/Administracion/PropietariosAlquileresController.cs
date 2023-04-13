using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public sealed class PropietariosAlquileresController : GeneralBaseController<PropietariosAlquileres, TreeCoreContext>
    {
        public PropietariosAlquileresController()
            : base()
        {

        }

        public PropietariosAlquileres GetPropietariosAlquilerByID(long propietarioID, long alquilerID)
        {

            List<PropietariosAlquileres> lista = null;
            PropietariosAlquileres dato = null;

            try
            {
                lista = (from c in Context.PropietariosAlquileres where c.AlquilerID == alquilerID && c.PropietarioID == propietarioID select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }
            return dato;
        }


        public PropietariosAlquileres GetPropietariosPrincipalByAlquilerID(long alquilerID)
        {

            List<PropietariosAlquileres> lista = null;
            PropietariosAlquileres dato = null;

            try
            {
                lista = (from c in Context.PropietariosAlquileres where c.AlquilerID == alquilerID && c.Principal == true select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }
            return dato;
        }

        public Vw_Propietarios GetPropietarioPrincipalByalquilerID(long alquilerID)
        {

            List<Vw_Propietarios> lista = null;
            Vw_Propietarios dato = null;
            try
            {
                lista = (from c in Context.Vw_Propietarios where c.AlquilerID == alquilerID && c.Principal == true select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }
            return dato;
        }


    }
}
