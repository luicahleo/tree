using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MunicipiosController : GeneralBaseController<Municipios, TreeCoreContext>, IBasica<Municipios>
    {
        public MunicipiosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string Municipio, long clienteID)
        {
            bool isExiste = false;
            List<Municipios> datos = new List<Municipios>();


            datos = (from c in Context.Municipios where (c.Municipio == Municipio) select c).ToList<Municipios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long MunicipioID)
        {
            Municipios oDato;
            bool defecto = false;
            try
            {
                oDato = (from c in Context.Municipios where c.Defecto && c.MunicipioID == MunicipioID select c).First();
                if (oDato != null)
                {
                    defecto = true;
                }
                else
                {
                    defecto = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                defecto = false;
            }

            return defecto;
        }

        public List<Municipios> GetMunicipiosActivosByProvID(long ProvinciaID)
        {
            List<Municipios> listaMunicipios = null;
            try
            {
                listaMunicipios = (from c
                                   in Context.Municipios
                                   where c.ProvinciaID == ProvinciaID &&
                                        c.Activo
                                   orderby c.Municipio
                                   select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaMunicipios = null;
            }

            return listaMunicipios;
        }

        public Municipios GetByNombre(string nombre)
        {
            Municipios municipio;
            try
            {
                municipio = (from c
                             in Context.Municipios
                             where c.Municipio == nombre &&
                                    c.Activo
                             select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                municipio = null;
            }
            return municipio;
        }
        public Municipios GetActivoByNombre(long ProvinciaID, string sMunicipalidad)
        {
            Municipios municipio;
            try
            {
                municipio = (from c
                             in Context.Municipios
                             where c.Municipio == sMunicipalidad &&
                                    c.ProvinciaID == ProvinciaID &&
                                    c.Activo
                             select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                municipio = null;
            }
            return municipio;
        }

        public long GetMunicipioByNombre(string Nombre)
        {
            // Local variables
            long dato = 0;

            try
            {
                dato = GetByNombre(Nombre).MunicipioID;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return dato;
        }
        public Municipios GetMunicipioByCodigo(string Codigo, long lClienteID)
        {
            // Local variables
            Municipios dato;

            try
            {
                dato = (from c in Context.Municipios
                        where
/*join Prov in Context.Provincias on c.ProvinciaID equals Prov.ProvinciaID
join RegPa in Context.RegionesPaises on Prov.RegionPaisID equals RegPa.RegionPaisID
join Pais in Context.Paises on RegPa.PaisID equals Pais.PaisID
where Pais.ClienteID == lClienteID &&*/ c.Codigo == Codigo
                        select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }
            return dato;
        }

        public bool getMunicipioRepetido(long ProvinciaID, string Municipio)
        {
            // Local variables
            List<Municipios> lista = null;
            bool dato = false;
            try
            {
                lista = (from c in Context.Municipios where c.ProvinciaID == ProvinciaID && c.Municipio == Municipio select c).ToList();
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
        public bool getMunicipio_CodigoRepetido(long ProvinciaID, string Codigo)
        {
            // Local variables
            List<Municipios> lista = null;
            bool dato = false;
            try
            {
                lista = (from c in Context.Municipios where c.ProvinciaID == ProvinciaID && c.Codigo == Codigo select c).ToList();
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

        public List<Municipios> getAllMunicipiosByProvID(long ProvinciaID)
        {
            List<Municipios> listaMunicipios = null;
            try
            {
                listaMunicipios = (from c in Context.Municipios where c.ProvinciaID == ProvinciaID orderby c.Municipio select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaMunicipios = null;
            }

            return listaMunicipios;
        }

        public long getProvinciaByMunicipioID(long? MunicipioID)
        {
            long Provincia = 0;
            try
            {
                Provincia = (from c in Context.Municipios where c.MunicipioID == MunicipioID select c.ProvinciaID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Provincia = 0;
            }

            return Provincia;
        }

        public String GetMunicipioByID(long? MunicipioID)
        {
            // Local variables 
            String dato = null;
            try
            {
                dato = (from c in Context.Municipios where c.MunicipioID == MunicipioID select c.Municipio).First();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }
            return dato;
        }

        public String GetMunicipioActivoByID(long MunicipioID)
        {
            string municipio;
            try
            {
                municipio = (from c
                 in Context.Municipios
                             where c.Activo &&
                                   c.MunicipioID == MunicipioID
                             orderby c.Municipio
                             select c.Municipio).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                municipio = null;
            }
            return municipio;
        }

        public bool RegistroDuplicado(string Municipio, string Codigo, long provinciaID, long lMunicipioID)
        {
            bool isExiste = false;
            List<Municipios> datos = new List<Municipios>();

            if (lMunicipioID != 0)
            {
                datos = (from c in Context.Municipios where (c.Municipio == Municipio || c.Codigo == Codigo) && c.ProvinciaID == provinciaID && c.MunicipioID != lMunicipioID select c).ToList<Municipios>();
            }
            else
            {
                datos = (from c in Context.Municipios where (c.Municipio == Municipio || c.Codigo == Codigo) && c.ProvinciaID == provinciaID select c).ToList<Municipios>();
            }
            

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public Municipios GetDefaultProvincia(long lProvinciaID)
        {
            Municipios oEstado;

            try
            {
                oEstado = (from c in Context.Municipios where c.Defecto && c.ProvinciaID == lProvinciaID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstado = null;
            }

            return oEstado;
        }

        public void EliminarDefecto(long provinciaID)
        {
            List<Municipios> oMunicipios;
            try
            {
                oMunicipios = (from c in Context.Municipios where c.Defecto && c.ProvinciaID == provinciaID select c).ToList();
                foreach (var item in oMunicipios)
                {
                    item.Defecto = false;
                    UpdateItem(item);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public List<Municipios> GetActivos(long ClienteID)
        {
            List<Municipios> listaMunicipios;

            try
            {
                listaMunicipios = (from c
                                   in Context.Municipios
                                   where c.Activo
                                   select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaMunicipios = null;
            }

            return listaMunicipios;
        }
        public long GetMunicipioIDByNombre(string Nombre, long ProvinciaID)
        {
            // Local variables
            List<Municipios> lista = null;
            long municipioID = 0;
            // takes the information
            lista = (from c in Context.Municipios where c.Municipio.Equals(Nombre) && c.ProvinciaID == ProvinciaID select c).ToList();

            if (lista != null && lista.Count > 0)
            {
                municipioID = lista.ElementAt(0).MunicipioID;
            }
            else
            {
                municipioID = 0;
            }

            return municipioID;
        }

        public List<Vw_CoreMunicipios> getMunicipiosByClienteID(long lClienteID)
        {
            List<Vw_CoreMunicipios> listaMunicipios = null;

            try
            {
                listaMunicipios = (from c in Context.Vw_CoreMunicipios where c.ClienteID == lClienteID orderby c.NombreMunicipio select c).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaMunicipios = null;
            }

            return listaMunicipios;
        }

        public Vw_CoreMunicipios GetCoreMunicipioByMunicipioID(long municipioID)
        {
            Vw_CoreMunicipios coreMunicipio = null;

            coreMunicipio = (from c in Context.Vw_CoreMunicipios
                             where c.MunicipioID == municipioID
                             select c).FirstOrDefault();

            return coreMunicipio;
        }

        public long? getPaisByMunicipio(string sMunicipio)
        {
            long? lPaisID = 0;
            long? lRegionPaisID = 0;

            try
            {
                lRegionPaisID = (from c in Context.Vw_Municipios where c.Municipio == sMunicipio select c.RegionPaisID).First();
                lPaisID = (from c in Context.RegionesPaises where c.RegionPaisID == lRegionPaisID select c.PaisID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lPaisID = 0;
            }

            return lPaisID;
        }

        public string getNombreProvinciaByMunicipioID(string sMunicipio)
        {
            string sProvincia = "";

            try
            {
                sProvincia = (from c in Context.Vw_Municipios where c.Municipio == sMunicipio select c.Provincia).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sProvincia = "";
            }

            return sProvincia;
        }
        public List<string> getCodigosMunicipioCliente(long lClienteID)
        {
            List<string> listaDatos = new List<string>();

            try
            {
                listaDatos = (from c in Context.Municipios
                              join Prov in Context.Provincias on c.ProvinciaID equals Prov.ProvinciaID
                              join RegPa in Context.RegionesPaises on Prov.RegionPaisID equals RegPa.RegionPaisID
                              join Pais in Context.Paises on RegPa.PaisID equals Pais.PaisID
                              where Pais.ClienteID == lClienteID
                              select c.Codigo).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = new List<string>();
            }

            return listaDatos;
        }
    }
}