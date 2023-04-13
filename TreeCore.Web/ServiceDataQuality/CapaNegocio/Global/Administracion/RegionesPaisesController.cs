using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class RegionesPaisesController : GeneralBaseController<RegionesPaises, TreeCoreContext>, IBasica<RegionesPaises>
    {
        public RegionesPaisesController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string RegionPais, long clienteID)
        {
            bool isExiste = false;
            List<RegionesPaises> datos = new List<RegionesPaises>();


            datos = (from c in Context.RegionesPaises where (c.RegionPais == RegionPais/* && c.ClienteID == clienteID*/) select c).ToList<RegionesPaises>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long RegionPaisID)
        {
            RegionesPaises dato = new RegionesPaises();
            RegionesPaisesController cController = new RegionesPaisesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && RegionPaisID == " + RegionPaisID.ToString());

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

        public List<RegionesPaises> GetListRegionPaisActivoByPaisID(long PaisID)
        {
            List<RegionesPaises> listaRegionesPaises = null;

            try
            {
                listaRegionesPaises = (from c 
                                       in Context.RegionesPaises 
                                       where c.PaisID == PaisID && 
                                            c.Activo 
                                       orderby c.RegionPais 
                                       select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaRegionesPaises = null;
            }

            return listaRegionesPaises;
        }

        public List<RegionesPaises> GetListRegionPaisActivoByPaisID(long PaisID, long clienteID)
        {
            List<RegionesPaises> listaRegionesPaises = null;

            try
            {
                listaRegionesPaises = (from c 
                                       in Context.RegionesPaises 
                                       where c.PaisID == PaisID && 
                                            c.Activo &&
                                            c.ClienteID == clienteID
                                       orderby c.RegionPais 
                                       select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaRegionesPaises = null;
            }

            return listaRegionesPaises;
        }

        public long getPaisByRegionID(long? RegionID)
        {
            long Pais = 0;
            try
            {
                Pais = (from c in Context.RegionesPaises where c.RegionPaisID == RegionID select c.PaisID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Pais = 0;
            }

            return Pais;
        }
        public String GetRegionByID(long RegionPaisID)
        {
            // Local variables 
            String dato = null;
            try
            {
                dato = (from c in Context.RegionesPaises where c.RegionPaisID == RegionPaisID select c.RegionPais).First();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }
            return dato;
        }
        public bool getRegionesPaisesRepetido(long? PaisID, string Region)
        {
            // Local variables  
            List<RegionesPaises> lista = null;
            bool dato = false;
            try
            {
                lista = (from c in Context.RegionesPaises where c.PaisID == PaisID && c.RegionPais == Region select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return dato;
        }
        public bool getRegionesPaises_CodigoRepetido(long? PaisID, string Codigo)
        {
            // Local variables
            List<RegionesPaises> lista = null;
            bool dato = false;
            try
            {
                lista = (from c in Context.RegionesPaises where c.PaisID == PaisID && c.Codigo == Codigo select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return dato;
        }
        public List<RegionesPaises> getAllRegionPaisByPaisID(long PaisID)
        {
            List<RegionesPaises> listaRegionesPaises = null;
            try
            {
                listaRegionesPaises = (from c in Context.RegionesPaises where c.PaisID == PaisID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaRegionesPaises = null;
            }

            return listaRegionesPaises;
        }

        public RegionesPaises GetByNombre(string nombre)
        {
            RegionesPaises regionPais;
            try
            {
                regionPais = (from c 
                              in Context.RegionesPaises
                              where c.Activo && c.RegionPais == nombre
                              select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                regionPais = null;
            }

            return regionPais;
        }

        public RegionesPaises GetByNombre(long paisID, string sRegionPais)
        {
            RegionesPaises regionPais;
            try
            {
                regionPais = (from c
                              in Context.RegionesPaises
                              where c.Activo && 
                                      c.RegionPais == sRegionPais && 
                                      c.PaisID==paisID
                              select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                regionPais = null;
            }

            return regionPais;
        }

        public string GetRegionIdByRegionPaisID(long regionid)
        {
            string regionPais;
            try 
            {
                regionPais = (from c 
                              in Context.RegionesPaises
                              where c.Activo && 
                                    c.RegionPaisID == regionid 
                              orderby c.RegionPais 
                              select c.RegionPais).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                regionPais = null;
            }
            return regionPais;
        }
        
        public long GetIdActivoByNombre(string regionPais)
        {
            long id = 0;

            try
            {
                id = (from d 
                      in Context.RegionesPaises
                      where regionPais == d.RegionPais 
                      select d.RegionPaisID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                id = -1;
            }

            return id;
        }
        public long GetRegionIDByNombre(string Nombre, long paisID)
        {
            // Local variables
            List<RegionesPaises> lista = null;
            long regionPaisID = 0;
            // takes the information
            lista = (from c in Context.RegionesPaises where c.RegionPais.Equals(Nombre) && c.PaisID == paisID select c).ToList();

            if (lista != null && lista.Count > 0)
            {
                regionPaisID = lista.ElementAt(0).RegionPaisID;
            }
            else
            {
                regionPaisID = 0;
            }

            return regionPaisID;
        }
        public bool RegistroDuplicado(string RegionPais, long clienteID, long paisID)
        {
            bool isExiste = false;
            List<RegionesPaises> datos = new List<RegionesPaises>();


            datos = (from c in Context.RegionesPaises where (c.RegionPais == RegionPais && c.ClienteID == clienteID && c.PaisID == paisID) select c).ToList<RegionesPaises>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public RegionesPaises GetDefault(long paisID) {
            RegionesPaises oRegionesPaises;
            try
            {
                oRegionesPaises = (from c in Context.RegionesPaises where c.Defecto && c.PaisID == paisID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oRegionesPaises = null;
            }
            return oRegionesPaises;
        }

        public void EliminarDefecto(long paisID) {
            List<RegionesPaises> oRegionesPaises;
            ProvinciasController cProvincias = new ProvinciasController();
            try
            {
                oRegionesPaises = (from c in Context.RegionesPaises where c.Defecto && c.PaisID == paisID select c).ToList();
                foreach (var item in oRegionesPaises)
                {
                    item.Defecto = false;
                    cProvincias.EliminarDefecto(item.RegionPaisID);
                    UpdateItem(item);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public List<RegionesPaises> GetActivos()
        {
            List<RegionesPaises> lista;

            try
            {
                lista = (from c
                         in Context.RegionesPaises
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

        public RegionesPaises GetRegionCompletaByNombre(string Nombre, long paisID)
        {
            // Local variables
            List<RegionesPaises> lista = null;
            RegionesPaises dato = null;
            // takes the information
            lista = (from c in Context.RegionesPaises where c.RegionPais.Equals(Nombre) && c.PaisID == paisID select c).ToList();

            if (lista != null && lista.Count > 0)
            {
                dato = lista.ElementAt(0);
            }
            else
            {
                dato = null;
            }

            return dato;
        }
    }
}