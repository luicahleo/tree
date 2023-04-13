using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalMunicipalidadesController : GeneralBaseController<GlobalMunicipalidades, TreeCoreContext>
    {
        public GlobalMunicipalidadesController()
            : base()
        { }
        public long GetMunicipalidadesByNombreMunicipioID(string Nombre, long municipioID)
        {
            // Local variables
            List<GlobalMunicipalidades> lista = null;
            long dato = -1;
            // takes the information
            try
            {
                lista = (from c in Context.GlobalMunicipalidades where c.Municipalidad.Equals(Nombre) && c.MunicipioID == municipioID select c).ToList();

                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0).GlobalMunicipalidadID;
                }
                else
                {
                    dato = -1;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return dato;
        }

        public bool RegistroDuplicadoByNombre(string Municipalidad, long MunicipioID)
        {
            bool isExiste = false;
            List<GlobalMunicipalidades> datos = new List<GlobalMunicipalidades>();


            datos = (from c in Context.GlobalMunicipalidades where (c.Municipalidad == Municipalidad && c.MunicipioID == MunicipioID) select c).ToList<GlobalMunicipalidades>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicadoByCodigo(string Codigo, long MunicipioID)
        {
            bool isExiste = false;
            List<GlobalMunicipalidades> datos = new List<GlobalMunicipalidades>();


            datos = (from c in Context.GlobalMunicipalidades where (c.Codigo == Codigo && c.MunicipioID == MunicipioID) select c).ToList<GlobalMunicipalidades>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public GlobalMunicipalidades GetDefault(long lMunicipalidadID)
        {
            GlobalMunicipalidades oEstado;

            try
            {
                oEstado = (from c in Context.GlobalMunicipalidades where c.Defecto && c.MunicipioID == lMunicipalidadID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstado = null;
            }

            return oEstado;
        }

        public void EliminarDefecto(long lMunicipalidadID)
        {
            List<GlobalMunicipalidades> oProvincias;
            GlobalMunicipalidadesController cMunicipios = new GlobalMunicipalidadesController();
            try
            {
                oProvincias = (from c in Context.GlobalMunicipalidades where c.Defecto && c.MunicipioID == lMunicipalidadID select c).ToList();
                foreach (var item in oProvincias)
                {
                    item.Defecto = false;
                    cMunicipios.EliminarDefecto(item.GlobalMunicipalidadID);
                    UpdateItem(item);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
    }
}