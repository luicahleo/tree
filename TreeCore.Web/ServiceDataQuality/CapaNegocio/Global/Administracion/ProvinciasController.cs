using System;
using System.Collections.Generic;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ProvinciasController : GeneralBaseController<Provincias, TreeCoreContext>, IBasica<Provincias>
    {
        public ProvinciasController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string Provincia, long clienteID)
        {
            bool isExiste = false;
            List<Provincias> datos = new List<Provincias>();


            datos = (from c in Context.Provincias where (c.Provincia == Provincia/* && c.ClienteID == clienteID*/) select c).ToList<Provincias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ProvinciaID)
        {
            Provincias dato;
            ProvinciasController cController = new ProvinciasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ProvinciaID == " + ProvinciaID.ToString());

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

        public List<Provincias> GetListProvinciaActivaByRegionPaisID(long RegionPaisID)
        {
            List<Provincias> listaProvincias = null;
            try
            {
                listaProvincias = (from c 
                                   in Context.Provincias 
                                   where c.RegionPaisID == RegionPaisID && 
                                        c.Activo 
                                   orderby c.Provincia 
                                   select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaProvincias = null;
            }

            return listaProvincias;
        }

        public Provincias GetProvinciaByNombreObjeto(string Nombre)
        {
            // Local variables 
            Provincias dato = new Provincias();
            List<Provincias> lista = null;
            try
            {
                lista = (from c in Context.Provincias where c.Provincia.Equals(Nombre) select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = new Provincias();
            }
            return dato;
        }

        public long GetprovinciaByNombre(string Nombre)
        {
            // Local variables 
            long dato = 0;
            List<long> lista = null;
            try
            {
                lista = (from c in Context.Provincias where c.Provincia.Equals(Nombre) select c.ProvinciaID).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = 0;
            }
            return dato;
        }

        public List<Provincias> getAllProvinciasByRegionPaisID(long RegionPaisID)
        {
            List<Provincias> listaProvincias = null;
            try
            {
                listaProvincias = (from c in Context.Provincias where c.RegionPaisID == RegionPaisID orderby c.Provincia select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaProvincias = null;
            }

            return listaProvincias;
        }

        public long getRegionByProvinciaID(long? ProvinciaID)
        {
            long Region = 0;
            try
            {
                Region = (from c in Context.Provincias where c.ProvinciaID == ProvinciaID select c.RegionPaisID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Region = 0;
            }

            return Region;
        }

        public String GetprovinciaByID(long ProvinciaID)
        {
            // Local variables 
            String dato = null;
            try
            {
                dato = (from c in Context.Provincias where c.ProvinciaID == ProvinciaID select c.Provincia).First();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }
            return dato;
        }

        public Provincias GetActivaByNombre(string nombre)
        {
            Provincias provincia;
            try
            {
                provincia = (from c
                             in Context.Provincias
                             where c.Provincia.Equals(nombre) &&
                                    c.Activo
                             select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                provincia = null;
            }
            return provincia;
        }

        public Provincias GetActivaByNombre(long regionPaisID, string nombre)
        {
            Provincias provincia;
            try
            {
                provincia = (from c
                             in Context.Provincias
                             where c.Provincia.Equals(nombre) &&
                                    c.RegionPaisID== regionPaisID &&
                                    c.Activo
                             select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                provincia = null;
            }
            return provincia;
        }

        public string GetNameProvinciaActivaByID(long provinciaid)
        {
            string provincia;
            try
            {
                provincia = (from c 
                             in Context.Provincias 
                             where  c.Activo && 
                                    c.ProvinciaID == provinciaid 
                             orderby c.Provincia 
                             select c.Provincia).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                provincia = null;
            }
            return provincia;
        }
        public bool RegistroDuplicadoByRegionID(string Provincia, long RegionID)
        {
            bool isExiste = false;
            List<Provincias> datos = new List<Provincias>();


            datos = (from c in Context.Provincias where c.Provincia == Provincia && c.RegionPaisID == RegionID select c).ToList<Provincias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public Provincias GetDefaultRegion(long lRegionID)
        {
            Provincias oEstado;

            try
            {
                oEstado = (from c in Context.Provincias where c.Defecto && c.RegionPaisID == lRegionID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstado = null;
            }

            return oEstado;
        }

        public void EliminarDefecto(long regionID)
        {
            List<Provincias> oProvincias;
            MunicipiosController cMunicipios = new MunicipiosController();
            try
            {
                oProvincias = (from c in Context.Provincias where c.Defecto && c.RegionPaisID == regionID select c).ToList();
                foreach (var item in oProvincias)
                {
                    item.Defecto = false;
                    cMunicipios.EliminarDefecto(item.RegionPaisID);
                    UpdateItem(item);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public List<Provincias> GetActivos()
        {
            List<Provincias> lista;

            try
            {
                lista = (from c
                         in Context.Provincias
                         where c.Activo
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public Provincias GetProvinciaByNombre(string Nombre, long RegionPaisID)
        {
            // Local variables
            List<Provincias> lista = null;
            Provincias provincia = null;
            // takes the information
            lista = (from c in Context.Provincias where c.Provincia.Equals(Nombre) && c.RegionPaisID == RegionPaisID select c).ToList();

            if (lista != null && lista.Count > 0)
            {
                provincia = new Provincias();
                provincia = lista.ElementAt(0);
            }
            else
            {
                provincia = null;
            }

            return provincia;
        }
        public long GetProvinciaIDByNombre(string Nombre, long RegionPaisID)
        {
            // Local variables
            List<Provincias> lista = null;
            long provinciaID = 0;
            // takes the information
            lista = (from c in Context.Provincias where c.Provincia.Equals(Nombre) && c.RegionPaisID == RegionPaisID select c).ToList();

            if (lista != null && lista.Count > 0)
            {
                provinciaID = lista.ElementAt(0).ProvinciaID;
            }
            else
            {
                provinciaID = 0;
            }

            return provinciaID;
        }
    }
}