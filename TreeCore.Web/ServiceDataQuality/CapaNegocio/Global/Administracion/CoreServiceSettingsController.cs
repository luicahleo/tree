using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreServiceSettingsController : GeneralBaseController<CoreServiceSettings, TreeCoreContext>, IBasica<CoreServiceSettings>
    {
        public CoreServiceSettingsController()
            : base()
        { }

        public bool RegistroVinculado(long EstadoGlobalID)
        {
            bool existe = true;


            return existe;
        }

        public bool HasActiveEstadoGlobal(string estadoGlobal, long clienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.EstadosGlobales
                          where c.Activo == true &&
                                c.EstadoGlobal == estadoGlobal &&
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
        /*
        public EstadosGlobales GetActivoEstadoGlobal(string estadoGlobal, long clienteID)
        {
            EstadosGlobales result;

            try
            {
                result = (from c in Context.EstadosGlobales
                          where c.Activo == true &&
                                c.EstadoGlobal == estadoGlobal &&
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
        */
        public bool RegistroDuplicado(string EstadoGlobal, long clienteID)
        {
            bool isExiste = false;
            List<EstadosGlobales> datos = new List<EstadosGlobales>();


            datos = (from c in Context.EstadosGlobales where (c.EstadoGlobal == EstadoGlobal && c.ClienteID == clienteID) select c).ToList<EstadosGlobales>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long EstadoGlobalID)
        {
            EstadosGlobales dato = new EstadosGlobales();
            EstadosGlobalesController cController = new EstadosGlobalesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && EstadoGlobalID == " + EstadoGlobalID.ToString());

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
        /*
        public long GetGlobalID(string sNombre)
        {
            long lDato = new long();

            try
            {
                lDato = (from c in Context.EstadosGlobales where c.EstadoGlobal == sNombre select c.EstadoGlobalID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lDato = new long();
            }

            return lDato;
        }

        public List<EstadosGlobales> GetEstadosGlobalesActivos(long ClienteID)
        {
            List<EstadosGlobales> lista = null;

            try
            {
                lista = (from c in Context.EstadosGlobales where c.ClienteID == ClienteID && c.Activo == true orderby c.EstadoGlobal select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<EstadosGlobales>();
            }

            return lista;
        }

        public List<EstadosGlobales> GetAllEstados(bool bActivo)
        {
            // Local variables
            List<EstadosGlobales> lista = null;

            try
            {
                if (bActivo)
                {
                    lista = (from c in Context.EstadosGlobales where c.Activo == true select c).ToList<EstadosGlobales>();
                }
                else
                {
                    lista = (from c in Context.EstadosGlobales select c).ToList<EstadosGlobales>();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

            }
            return lista;
        }

        public EstadosGlobales GetDefault(long clienteID)
        {
            EstadosGlobales oEstadosGlobales;
            try
            {
                oEstadosGlobales = (from c in Context.EstadosGlobales where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstadosGlobales = null;
            }
            return oEstadosGlobales;
        }

        public EstadosGlobales GetDesactivo(long clienteID)
        {
            EstadosGlobales oEstadosGlobales;
            try
            {
                oEstadosGlobales = (from c in Context.EstadosGlobales where c.Desactivo && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstadosGlobales = null;
            }
            return oEstadosGlobales;
        }
        public EstadosGlobales GetDesinstalado(long clienteID)
        {
            EstadosGlobales oEstadosGlobales;
            try
            {
                oEstadosGlobales = (from c in Context.EstadosGlobales where c.Desinstalado && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstadosGlobales = null;
            }
            return oEstadosGlobales;
        }

        public List<EstadosGlobales> GetActivos(long clienteID)
        {
            List<EstadosGlobales> lista;

            try
            {
                lista = (from c
                         in Context.EstadosGlobales
                         where c.Activo == true &&
                                c.ClienteID == clienteID
                         orderby c.EstadoGlobal
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        */
      

        public CoreServiceSettings GetCoreServiceSettingsByNombre(string Nombre)
        {

            CoreServiceSettings estGLobalID = new CoreServiceSettings();
            try
            {

                estGLobalID = (from c in Context.CoreServiceSettings where c.Nombre.Equals(Nombre) select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                estGLobalID = null;

            }
            return estGLobalID;
        }

        public CoreServiceSettings GetCoreServiceSettings(long Codigo)
        {
            CoreServiceSettings listaDatos = new CoreServiceSettings();
            
            try
            {
               
                listaDatos = (from c in Context.CoreServiceSettings where c.CodigoServiceSettings == Codigo  select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }
    }
}