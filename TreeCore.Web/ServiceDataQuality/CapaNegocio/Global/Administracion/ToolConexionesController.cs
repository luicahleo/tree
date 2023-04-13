using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ToolConexionesController : GeneralBaseController<ToolConexiones, TreeCoreContext>
    {
        public ToolConexionesController()
            : base()
        { }

        public bool RegistroVinculado(long ClienteID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string sConexion, long clienteID)
        {
            bool isExiste = false;
            List<ToolConexiones> listaDatos = new List<ToolConexiones>();

            listaDatos = (from c in Context.ToolConexiones where (c.Servidor == sConexion && c.ClienteID == clienteID) select c).ToList<ToolConexiones>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        
        public Vw_ToolConexiones GetConexionByServicioID (long lServicioID)
        {
            Vw_ToolConexiones oDato = null;

            try
            {
                oDato = (from c in Context.Vw_ToolConexiones where c.ServicioID == lServicioID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public ToolConexiones GetTablaConexionByServicioID(long lIntegracionID)
        {
            ToolConexiones oDato = null;

            try
            {
                oDato = (from c in Context.ToolConexiones where c.IntegracionID == lIntegracionID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public bool RegistroDefecto(long ToolConexionID)
        {
            ToolConexiones dato = new ToolConexiones();
            ToolConexionesController cController = new ToolConexionesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ToolConexionID == " + ToolConexionID.ToString());

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

        public Vw_ToolIntegracionesServiciosMetodosConexiones GetConexionPorMetodo(string metodo)
        {
            // Local variables
            List<Vw_ToolIntegracionesServiciosMetodosConexiones> lista = null;
            Vw_ToolIntegracionesServiciosMetodosConexiones datos = null;

            // Obtains the information
            lista = (from c in Context.Vw_ToolIntegracionesServiciosMetodosConexiones where (c.Metodo == metodo && (bool)c.ActivoIntegracionServicioMetodo && (bool)c.ActivoIntegracion && (bool)c.ActivoServicio && (bool)c.ActivoMetodo && (bool)c.ActivoConexion) select c).ToList();

            if (lista.Count > 0)
            {
                datos = lista.ElementAt(0);
            }
            return datos;

        }

        public List<Vw_ToolConexionesIntegraciones> GetConexionesPorClienteID(long? clienteID)
        {
            // Local variables
            List<Vw_ToolConexionesIntegraciones> lista = null;

            // Obtains the information
            if (clienteID != null)
            {
                lista = (from c in Context.Vw_ToolConexionesIntegraciones where (c.ClienteID == clienteID && (bool)c.ActivaConexion) select c).ToList();
            }
            else
            {
                lista = (from c in Context.Vw_ToolConexionesIntegraciones where ((bool)c.ActivaConexion) select c).ToList();
            }

            return lista;

        }

        public ToolConexiones GetDefault(long lClienteID)
        {
            ToolConexiones oConexion;

            try
            {
                oConexion = (from c in Context.ToolConexiones where c.Defecto && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oConexion = null;
            }

            return oConexion;
        }

        public List<Vw_ToolIntegracionesServiciosMetodosConexiones> GetConexionesListaPorServicio(string servicio)
        {
            // Local variables
            List<Vw_ToolIntegracionesServiciosMetodosConexiones> lista = null;

            // Obtains the information
            lista = (from c in Context.Vw_ToolIntegracionesServiciosMetodosConexiones where (c.Servicio == servicio && (bool)c.ActivoIntegracionServicioMetodo && (bool)c.ActivoIntegracion && (bool)c.ActivoServicio && (bool)c.ActivoMetodo && (bool)c.ActivoConexion) select c).ToList();

            return lista;

        }

        public Vw_ToolIntegracionesServiciosMetodosConexiones GetConexionPorServicio(string servicio)
        {
            // Local variables
            List<Vw_ToolIntegracionesServiciosMetodosConexiones> lista = null;
            Vw_ToolIntegracionesServiciosMetodosConexiones datos = null;

            // Obtains the information
            lista = (from c in Context.Vw_ToolIntegracionesServiciosMetodosConexiones 
                     where 
                         (c.Servicio == servicio && 
                         (bool)c.ActivoIntegracionServicioMetodo && 
                         (bool)c.ActivoIntegracion && 
                         (bool)c.ActivoServicio && 
                         (bool)c.ActivoMetodo && 
                         (bool)c.ActivoConexion) 
                     select c).ToList();

            if (lista.Count > 0)
            {
                datos = lista.ElementAt(0);
            }
            return datos;

        }

        public Vw_ToolIntegracionesServiciosMetodosConexiones GetConexionPorServicioYParametro(string servicio, string sDatoExistenteEnCadena)
        {
            // Local variables
            List<Vw_ToolIntegracionesServiciosMetodosConexiones> lista = null;
            Vw_ToolIntegracionesServiciosMetodosConexiones datos = null;

            // Obtains the information
            lista = (from c in Context.Vw_ToolIntegracionesServiciosMetodosConexiones where (c.Servicio == servicio && (bool)c.ActivoIntegracionServicioMetodo && (bool)c.ActivoIntegracion && (bool)c.ActivoServicio && (bool)c.ActivoMetodo && (bool)c.ActivoConexion && c.Servidor.Contains(sDatoExistenteEnCadena)) select c).ToList();

            if (lista.Count > 0)
            {
                datos = lista.ElementAt(0);
            }
            return datos;

        }

        public List<ToolConexiones> getListaConexiones(long lIntegracionID)
        {
            List<ToolConexiones> listaConexiones;

            try
            {
                listaConexiones = (from c in Context.ToolConexiones where c.IntegracionID == lIntegracionID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaConexiones = null;
            }

            return listaConexiones;
        }

        public bool RegistroDuplicadoLogin (string sServidor, string sUsuario)
        {
            bool isExiste = false;
            List<Vw_ToolConexiones> listaDatos = new List<Vw_ToolConexiones>();

            listaDatos = (from c in Context.Vw_ToolConexiones where (c.Servidor == sServidor && c.Usuario == sUsuario) select c).ToList<Vw_ToolConexiones>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}