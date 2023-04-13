using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class EmplazamientosCategoriasSitiosController : GeneralBaseController<EmplazamientosCategoriasSitios, TreeCoreContext>
    {
        public EmplazamientosCategoriasSitiosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool HasActiveCategoria(string categoria, long clienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.EmplazamientosCategoriasSitios
                          where c.Activo && 
                                c.CategoriaSitio == categoria && 
                                c.ClienteID == clienteID
                          select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }

            return existe;
        }

        public EmplazamientosCategoriasSitios GetActivoCategoria(string categoria, long clienteID)
        {
            EmplazamientosCategoriasSitios result;

            try
            {
                result = (from c in Context.EmplazamientosCategoriasSitios
                          where c.Activo &&
                                c.CategoriaSitio == categoria &&
                                c.ClienteID == clienteID
                          select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = null;
            }

            return result;
        }

        public bool RegistroDuplicado(string CategoriaSitio, long clienteID)
        {
            bool isExiste = false;
            List<EmplazamientosCategoriasSitios> datos = new List<EmplazamientosCategoriasSitios>();


            datos = (from c in Context.EmplazamientosCategoriasSitios where (c.CategoriaSitio == CategoriaSitio && c.ClienteID == clienteID) select c).ToList<EmplazamientosCategoriasSitios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long EmplazamientoCategoriaSitioID)
        {
            EmplazamientosCategoriasSitios dato = new EmplazamientosCategoriasSitios();
            EmplazamientosCategoriasSitiosController cController = new EmplazamientosCategoriasSitiosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && EmplazamientoCategoriaSitioID == " + EmplazamientoCategoriaSitioID.ToString());

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

        public List<EmplazamientosCategoriasSitios> GetCategoriasSitiosActivas(long ClienteID)
        {
            List<EmplazamientosCategoriasSitios> lista = null;

            try
            {
                lista = (from c in Context.EmplazamientosCategoriasSitios where c.Activo && c.ClienteID == ClienteID orderby c.CategoriaSitio select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<EmplazamientosCategoriasSitios>();
            }

            return lista;
        }
        public EmplazamientosCategoriasSitios GetDefault(long clienteID)
        {
            EmplazamientosCategoriasSitios categoriaSitio;
            try
            {
                categoriaSitio = (from c in Context.EmplazamientosCategoriasSitios where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                categoriaSitio = null;
            }
            return categoriaSitio;
        }

        public List<EmplazamientosCategoriasSitios> GetActivos(long clienteID)
        {
            List<EmplazamientosCategoriasSitios> lista;

            try
            {
                lista = (from c 
                         in Context.EmplazamientosCategoriasSitios 
                         where c.Activo && 
                                c.ClienteID == clienteID
                         orderby c.CategoriaSitio
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<string> GetCategoriasNombre(long ClienteID)
        {
            List<string> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosCategoriasSitios
                         where c.Activo == true && c.ClienteID == ClienteID
                         orderby c.CategoriaSitio
                         select c.CategoriaSitio).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public long GetCategoriaByNombreAll(string Nombre)
        {

            long categoriaSitioID = 0;
            try
            {

                categoriaSitioID = (from c in Context.EmplazamientosCategoriasSitios where c.CategoriaSitio.Equals(Nombre) select c.EmplazamientoCategoriaSitioID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                categoriaSitioID = -1;

            }
            return categoriaSitioID;
        }

    }
}