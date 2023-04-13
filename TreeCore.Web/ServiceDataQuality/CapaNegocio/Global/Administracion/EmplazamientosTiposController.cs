using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class EmplazamientosTiposController : GeneralBaseController<EmplazamientosTipos, TreeCoreContext>, IBasica<EmplazamientosTipos>
    {
        public EmplazamientosTiposController()
            : base()
        { }

        public List<EmplazamientosTipos> GetEmplazamientosTiposActivos(long ClienteID)
        {
            List<EmplazamientosTipos> lista = null;

            try
            {
                lista = (from c in Context.EmplazamientosTipos where c.Activo && c.ClienteID == ClienteID orderby c.Tipo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<EmplazamientosTipos>();
            }

            return lista;
        }
        public bool RegistroVinculado(long EmplazamientoTipoID)
        {
            bool existe = true;


            return existe;
        }

        public bool HasActiveEmplazamientoTipo(string tipo, long clienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.EmplazamientosTipos
                          where c.Activo && 
                                c.Tipo==tipo && 
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

        public EmplazamientosTipos GetActivoEmplazamientoTipo(string tipo, long clienteID)
        {
            EmplazamientosTipos result;

            try
            {
                result = (from c in Context.EmplazamientosTipos
                          where c.Activo &&
                                c.Tipo == tipo &&
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

        public bool RegistroDuplicado(string EmplazamientoTipo, long clienteID)
        {
            bool isExiste = false;
            List<EmplazamientosTipos> datos = new List<EmplazamientosTipos>();


            datos = (from c in Context.EmplazamientosTipos where (c.Tipo == EmplazamientoTipo && c.ClienteID == clienteID) select c).ToList<EmplazamientosTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long EmplazamientoTipoID)
        {
            EmplazamientosTipos dato = new EmplazamientosTipos();
            EmplazamientosTiposController cController = new EmplazamientosTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && EmplazamientoTipoID == " + EmplazamientoTipoID.ToString());

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

        public EmplazamientosTipos GetDefault(long clienteID) {
            EmplazamientosTipos oEmplazamientosTipos;
            try
            {
                oEmplazamientosTipos = (from c in Context.EmplazamientosTipos where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEmplazamientosTipos = null;
            }
            return oEmplazamientosTipos;
        }

        public List<EmplazamientosTipos> GetActivos(long clienteID)
        {
            List<EmplazamientosTipos> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosTipos
                         where c.Activo && 
                                c.ClienteID == clienteID
                         orderby c.Tipo
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<string> GetTiposNombre(long ClienteID)
        {
            List<string> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosTipos
                         where c.Activo && c.ClienteID == ClienteID
                         orderby c.Tipo
                         select c.Tipo).ToList();
            }
            catch (Exception)
            {
                lista = null;
            }

            return lista;
        }

        public long GetTipoByNombreAll(string Nombre)
        {

            long tipoID = 0;
            try
            {

                tipoID = (from c in Context.EmplazamientosTipos where c.Tipo.Equals(Nombre) select c.EmplazamientoTipoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoID = -1;

            }
            return tipoID;
        }

    }
}