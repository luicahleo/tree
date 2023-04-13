using Ext.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class PaisesController : GeneralBaseController<Paises, TreeCoreContext>, IBasica<Paises>
    {
        public PaisesController()
            : base()
        { }

        public bool RegistroVinculado(long PaisID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string Pais, long clienteID)
        {
            bool isExiste = false;
            List<Paises> datos = new List<Paises>();


            datos = (from c in Context.Paises where (c.Pais == Pais && c.ClienteID == clienteID) select c).ToList<Paises>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long PaisID)
        {
            Paises dato = new Paises();
            PaisesController cController = new PaisesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && PaisID == " + PaisID.ToString());

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

        public Paises GetPaisDefecto()
        {
            List<Paises> listaDatos = null;
            listaDatos = GetItemsList("Defecto");

            if (listaDatos.Count > 0)
            {
                return listaDatos[0];
            }
            else
            {
                listaDatos = GetItemsList("", "PaisID");

                if (listaDatos.Count > 0)
                {
                    return listaDatos[0];
                }
                else
                {
                }
            }

            return null;
        }

        public class dirGoogle
        {

            public string long_name { get; set; }
            public string short_name { get; set; }
            public string[] types { get; set; }
        }

        public long GetregionID_Activo(string pNombre)
        {
            long id = 0;
            try
            {
                id = (from c in Context.Regiones where c.Region == pNombre && c.Activo == true select c.RegionID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                id = -1;
            }
            return id;
        }
        public long GetPaisByNombre(string Nombre)
        {
            Paises pais;
            long paisID = 0;

            try
            {
                pais = (from c
                       in Context.Paises
                        where (c.Pais.Equals(Nombre) ||
                                 c.Pais_En.Equals(Nombre) ||
                                 c.Pais_Fr.Equals(Nombre))
                        select c).First();
                paisID = pais.PaisID;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                paisID = -1;
            }

            return paisID;
        }

        public String GetPaisByID(long PaisID)
        {
            // Local variables 
            String dato = null;
            try
            {
                dato = (from c in Context.Paises where c.PaisID == PaisID select c.Pais).First();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }
            return dato;
        }

        public List<Paises> GetActivos(long clienteID)
        {
            List<Paises> paises;
            try
            {
                paises = (from c
                          in Context.Paises
                          where c.Activo && c.ClienteID == clienteID
                          orderby c.Pais
                          select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                paises = null;
            }

            return paises;
        }

        public List<Paises> GetActivosConPrefijo(long clienteID)
        {
            List<Paises> paises;
            try
            {
                paises = (from c
                          in Context.Paises
                          where c.Activo && c.ClienteID == clienteID && c.Prefijo != ""
                          orderby c.Pais
                          select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                paises = null;
            }

            return paises;
        }

        public Paises GetDefault(long clienteID)
        {
            Paises oPaises;
            try
            {
                if (clienteID == 0)
                {
                    oPaises = (from c in Context.Paises where c.Defecto && c.ClienteID == null select c).First();
                }
                else
                {
                    oPaises = (from c in Context.Paises where c.Defecto && c.ClienteID == clienteID select c).First();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oPaises = null;
            }
            return oPaises;
        }

        public bool RegistroDuplicado(string Pais, string Codigo, long clienteID, long lPaisID)
        {
            bool isExiste = false;
            List<Paises> datos = new List<Paises>();

            if (lPaisID != 0)
            {
                datos = (from c in Context.Paises where (c.Pais == Pais || c.PaisCod == Codigo) && c.ClienteID == clienteID && c.PaisID != lPaisID select c).ToList<Paises>();
            }
            else
            {
                datos = (from c in Context.Paises where (c.Pais == Pais || c.PaisCod == Codigo) && c.ClienteID == clienteID select c).ToList<Paises>();
            }

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<Paises> GetPaisesRegion(long regionID) {
            List<Paises> listaDatos;
            try
            {
                listaDatos = (from c in Context.Paises where c.Activo && c.RegionID == regionID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<Paises> GetAllActivos()
        {
            List<Paises> lista;

            try
            {
                lista = (from c
                         in Context.Paises
                         where c.Activo
                         orderby c.Pais
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public long GetPaisByNombreRegion(string Nombre, long RegionID)
        {
            long id = 0;
            try
            {
                id = (from c in Context.Paises where ((c.Pais.Equals(Nombre) || c.Pais_En.Equals(Nombre) || c.Pais_Fr.Equals(Nombre)) && (c.RegionID == RegionID)) select c.PaisID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                id = -1;
            }
            return id;
        }
        public long GetPaisByNombreRegion(string Nombre)
        {
            long id = 0;
            try
            {
                id = (from c in Context.Paises where ((c.Pais.Equals(Nombre) || c.Pais_En.Equals(Nombre) || c.Pais_Fr.Equals(Nombre))) select c.PaisID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                id = -1;
            }
            return id;
        }

        public Paises GetPaisByNombreRegion(string nombre, long regionID, long clienteID)
        {
            Paises pais;

            try
            {
                pais = (from c in Context.Paises
                        where c.Activo &&
                            c.Pais == nombre &&
                            c.RegionID == regionID &&
                            c.ClienteID == clienteID
                        select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                pais = null;
            }
            return pais;
        }

        public string getMunicipioIDByPais(Paises oDato)
        {
            string sMunicipio;

            try
            {
                sMunicipio = (from c in Context.RegionesPaises
                              join paises in Context.Paises on c.PaisID equals paises.PaisID
                              join municipio in Context.Vw_Municipios on c.RegionPaisID equals municipio.RegionPaisID
                              where paises.PaisID == oDato.PaisID
                              select municipio.Municipio).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sMunicipio = "";
            }

            return sMunicipio;
        }

        public Paises GetPaisByMunicipioID(long lMunicipioID)
        {
            Paises oPais = null;
            long? lRegionPaisID;
            long lPaisID;

            try
            {
                if (lMunicipioID != 0)
                {
                    lRegionPaisID = (from c in Context.Vw_Municipios where c.MunicipioID == lMunicipioID select c.RegionPaisID).First();

                    if (lRegionPaisID != 0)
                    {
                        lPaisID = (from c in Context.RegionesPaises where c.RegionPaisID == lRegionPaisID select c.PaisID).First();

                        if (lPaisID != 0)
                        {
                            oPais = (from c in Context.Paises where c.PaisID == lPaisID select c).First();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oPais = null;
            }

            return oPais;
        }

        public List<Ext.Net.MenuItem> GetMenuItemsPrefijos(long ClienteID)
        {
            List<Ext.Net.MenuItem> items = new List<Ext.Net.MenuItem>();
            List<Paises> paises;

            try
            {
                paises = GetActivos(ClienteID);

                if (paises != null)
                {
                    foreach (Paises pais in paises)
                    {
                        if (pais.Icono != null && pais.Prefijo != null)
                        {
                            Icon icono = (Icon)Enum.Parse(typeof(Icon), pais.Icono);
                            items.Add(new Ext.Net.MenuItem(pais.Prefijo)
                            {
                                Icon = icono
                            });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

            return items;
        }

        public bool PrefijoValido(long ClienteID, string sPrefijo)
        {
            bool bValido = false;
            string sPrefijoFinal;

            try
            {
                var paises = GetActivos(ClienteID);

                if (!sPrefijo.Contains("+"))
                {
                    sPrefijoFinal = "+" + sPrefijo;
                }
                else
                {
                    sPrefijoFinal = sPrefijo;
                }

                if (paises.Select(c => c.Prefijo).Contains(sPrefijoFinal))
                {
                    bValido = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                bValido = false;
            }

            return bValido;
        }

    }
}