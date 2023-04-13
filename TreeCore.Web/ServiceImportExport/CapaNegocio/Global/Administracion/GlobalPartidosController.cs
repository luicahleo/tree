using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalPartidosController : GeneralBaseController<GlobalPartidos, TreeCoreContext>
    {
        public GlobalPartidosController()
            : base()
        { }
        public long GetPartidosByNombreMunicipalidadID(string Nombre, long municipalidadID)
        {
            // Local variables
            List<GlobalPartidos> lista = null;
            long dato = -1;
            // takes the information
            try
            {
                lista = (from c in Context.GlobalPartidos where c.Partido.Equals(Nombre) && c.GlobalMunicipalidadID == municipalidadID select c).ToList();

                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0).GlobalPartidoID;
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

        public bool RegistroDuplicadoByNombre(string NombrePartido, long MunicipalidadID)
        {
            bool isExiste = false;
            List<GlobalPartidos> datos = new List<GlobalPartidos>();


            datos = (from c in Context.GlobalPartidos where (c.Partido == NombrePartido && c.GlobalMunicipalidadID == MunicipalidadID) select c).ToList<GlobalPartidos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicadoByCodigo(string Codigo, long MunicipalidadID)
        {
            bool isExiste = false;
            List<GlobalPartidos> datos = new List<GlobalPartidos>();


            datos = (from c in Context.GlobalPartidos where (c.Codigo == Codigo && c.GlobalMunicipalidadID == MunicipalidadID) select c).ToList<GlobalPartidos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long IDGlobalPartido)
        {
            GlobalPartidos oDato;
            bool defecto = false;
            try
            {
                oDato = (from c in Context.GlobalPartidos where c.Defecto && c.GlobalPartidoID == IDGlobalPartido select c).First();
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

        public List<GlobalPartidos> getAllMunicipalidadesByMunID(long MunicipalidadID)
        {
            List<GlobalPartidos> listaGlobalPartidos = null;
            try
            {
                listaGlobalPartidos = (from c in Context.GlobalPartidos where c.GlobalMunicipalidadID == MunicipalidadID orderby c.Partido select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaGlobalPartidos = null;
            }

            return listaGlobalPartidos;
        }

        public GlobalPartidos GetDefault(long lGlobalPartidoID)
        {
            GlobalPartidos oEstado;

            try
            {
                oEstado = (from c in Context.GlobalPartidos where c.Defecto && c.GlobalMunicipalidadID == lGlobalPartidoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstado = null;
            }

            return oEstado;
        }
    }
}