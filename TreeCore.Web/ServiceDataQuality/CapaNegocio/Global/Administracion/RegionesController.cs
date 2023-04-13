using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class RegionesController : GeneralBaseController<Regiones, TreeCoreContext>, IBasica<Regiones>
    {
        public RegionesController()
            : base()
        { }

        public bool RegistroVinculado(long RegionID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string Region, long clienteID)
        {
            bool isExiste = false;
            List<Regiones> datos = new List<Regiones>();


            datos = (from c in Context.Regiones where (c.Region == Region && c.ClienteID == clienteID) select c).ToList<Regiones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long RegionID)
        {
            Regiones dato = new Regiones();
            RegionesController cController = new RegionesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && RegionID == " + RegionID.ToString());

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }

        public Regiones GetRegionCompletaByNombre(string Nombre)
        {
            // Local variables
            List<Regiones> lista = null;
            Regiones dato = null;
            // takes the information
            lista = (from c in Context.Regiones where c.Region.Equals(Nombre) select c).ToList();

            if (lista != null && lista.Count > 0)
            {
                dato = lista.ElementAt(0);
            }
            else
            {
                dato = new Regiones();
            }

            return dato;
        }


        public Regiones GetRegionActivaByNombre(string Nombre)
        {
            Regiones dato;

            try
            {
                dato = (from c in Context.Regiones 
                        where c.Region.Equals(Nombre) 
                        select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }

            return dato;
        }

        public Regiones GetDefault()
        {
            Regiones oRegion;

            try
            {
                oRegion = (from c in Context.Regiones where c.Defecto select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oRegion = null;
            }

            return oRegion;
        }

        public List<Regiones> GetActivos() {
            List<Regiones> listadatos;
            try
            {
                listadatos = (from c 
                              in Context.Regiones 
                              where c.Activo
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }

        public Regiones GetRegionByID(long lID)
        {
            Regiones oDato = null;

            oDato = (from c in Context.Regiones where c.RegionID == lID select c).First();

            return oDato;
        }

        public List<Regiones> GetActivosByClienteID(long lClienteID)
        {
            List<Regiones> listadatos;
            try
            {
                listadatos = (from c in Context.Regiones where c.Activo && c.ClienteID == lClienteID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }

    }
}