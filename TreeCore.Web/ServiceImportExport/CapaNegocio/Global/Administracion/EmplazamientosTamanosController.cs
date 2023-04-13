using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class EmplazamientosTamanosController : GeneralBaseController<EmplazamientosTamanos, TreeCoreContext>, IBasica<EmplazamientosTamanos>
    {
        public EmplazamientosTamanosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool HasActiveTamano(string tamano, long clienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.EmplazamientosTamanos 
                          where c.Activo && 
                                  c.Tamano == tamano && 
                                  c.ClienteID==clienteID
                          select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }

            return existe;
        }
        public EmplazamientosTamanos GetActivoTamano(string tamano, long clienteID)
        {
            EmplazamientosTamanos existe;

            try
            {
                existe = (from c in Context.EmplazamientosTamanos
                          where c.Activo &&
                                  c.Tamano == tamano &&
                                  c.ClienteID == clienteID
                          select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = null;
            }

            return existe;
        }

        public bool RegistroDuplicado(string EmplazamientoTamano, long clienteID)
        {
            bool isExiste = false;
            List<EmplazamientosTamanos> datos = new List<EmplazamientosTamanos>();


            datos = (from c in Context.EmplazamientosTamanos where (c.Tamano == EmplazamientoTamano && c.ClienteID == clienteID) select c).ToList<EmplazamientosTamanos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long EmplazamientoTamanoID)
        {
            EmplazamientosTamanos dato = new EmplazamientosTamanos();
            EmplazamientosTamanosController cController = new EmplazamientosTamanosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && EmplazamientoTamanoID == " + EmplazamientoTamanoID.ToString());

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

        public List<EmplazamientosTamanos> GetEmplazamientosTamanosActivos(long ClienteID)
        {
            List<EmplazamientosTamanos> lista = null;

            try
            {
                lista = (from c in Context.EmplazamientosTamanos where c.Activo && c.ClienteID == ClienteID orderby c.Tamano select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<EmplazamientosTamanos>();
            }

            return lista;
        }

        public EmplazamientosTamanos GetDefault(long clienteID)
        {
            EmplazamientosTamanos emplazamientoTamano;
            try
            {
                emplazamientoTamano = (from c in Context.EmplazamientosTamanos where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                emplazamientoTamano = null;
            }
            return emplazamientoTamano;
        }

        public List<EmplazamientosTamanos> GetActivos(long clienteID)
        {
            List<EmplazamientosTamanos> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosTamanos
                         where c.Activo &&
                                c.ClienteID == clienteID
                         orderby c.Tamano
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<string> GetTamanosNombre(long ClienteID)
        {
            List<string> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosTamanos
                         where c.Activo && c.ClienteID == ClienteID
                         orderby c.Tamano
                         select c.Tamano).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public long GetTamanoByNombreAll(string Nombre)
        {

            long tamanoID = 0;
            try
            {

                tamanoID = (from c in Context.EmplazamientosTamanos where c.Tamano.Equals(Nombre) select c.EmplazamientoTamanoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tamanoID = -1;

            }
            return tamanoID;
        }

    }
}