using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{

    public class ProyectosTiposController : GeneralBaseController<ProyectosTipos, TreeCoreContext>, IBasica<ProyectosTipos>
    {
        public ProyectosTiposController()
            : base()
        {

        }

        public bool RegistroVinculado(long ProyectoTipoID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string ProyectoTipo, long clienteID)
        {
            bool isExiste = false;
            List<ProyectosTipos> datos = new List<ProyectosTipos>();

            datos = (from c in Context.ProyectosTipos where (c.ProyectoTipo == ProyectoTipo /*&& c.ClienteID == clienteID*/) select c).ToList<ProyectosTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicado2(string ProyectoTipo)
        {
            bool isExiste = false;
            List<ProyectosTipos> datos = new List<ProyectosTipos>();


            datos = (from c in Context.ProyectosTipos where (c.ProyectoTipo == ProyectoTipo) select c).ToList<ProyectosTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ProyectoTipoID)
        {
            ProyectosTipos dato = new ProyectosTipos();
            ProyectosTiposController cController = new ProyectosTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ProyectoTipoID == " + ProyectoTipoID.ToString());

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

        public List<ProyectosTipos> GetAllProyectosTipos()
        {
            // Local variables
            List<ProyectosTipos> lista = null;
            try
            {
                lista = (from c in Context.ProyectosTipos select c).OrderBy("ProyectoTipo").ToList<ProyectosTipos>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        public List<ProyectosTipos> ProyectosTiposNoAsignado(long clienteID)
        {
            List<long> tipos;
            tipos = (from c in Context.ClientesProyectosTipos where c.ClienteID == clienteID select c.ProyectoTipoID).ToList<long>();
            return (from c in Context.ProyectosTipos where !(tipos.Contains(c.ProyectoTipoID)) select c).OrderBy("ProyectoTipo").ToList<ProyectosTipos>();
        }


        public string ExisteZona(long proyectoTipoID)
        {
            bool? zonaExistente = false;

            zonaExistente = (from c in Context.ProyectosTipos where c.ProyectoTipoID == proyectoTipoID select c.ExisteZona).First();

            if (zonaExistente == true)
            {
                return "si";
            }
            else
            {
                return "no";
            }
        }

        public Vw_ClientesProyectosTipos GetItemVistaClientesProyectosTipos(long ProyectoTipoID, long ClienteID)
        {
            Vw_ClientesProyectosTipos oDato;
            try
            {
                oDato = (from c in Context.Vw_ClientesProyectosTipos where c.ProyectoTipoID == ProyectoTipoID && c.ClienteID == ClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        /// <summary>
        /// Obtiene los avisos asignados a un rol.
        /// </summary>
        /// <param name="perfilID">Perfil del que obtener los avisos asignados.</param>
        /// <returns>Lista de avisos asignados al rol</returns>
        public List<Vw_ClientesProyectosTipos> ProyectosTiposAsignados(long clienteID)
        {


            List<long> tipos;
            List<Vw_ClientesProyectosTipos> outList = new List<Vw_ClientesProyectosTipos>();

            tipos = (from c in Context.ClientesProyectosTipos where c.ClienteID == clienteID select c.ProyectoTipoID).ToList<long>();

            if (tipos != null)
            {
                outList = (from c in Context.Vw_ClientesProyectosTipos where (tipos.Contains(c.ProyectoTipoID) && clienteID == c.ClienteID) select c).OrderBy("ProyectoTipo").ToList<Vw_ClientesProyectosTipos>();
            }

            return outList;
        }
        public List<Vw_ClientesProyectosTipos> ProyectosTiposAsignadosInstall(string ModuloInstall, string ModuloObraCivil)
        {

            return (from c in Context.Vw_ClientesProyectosTipos where (c.ProyectoTipo == ModuloInstall) || (c.ProyectoTipo == ModuloObraCivil) select c).OrderBy("ProyectoTipo").ToList<Vw_ClientesProyectosTipos>();
        }
        public List<Vw_ClientesProyectosTipos> ProyectosTiposAsignadosMantenimiento(long clienteID, long PreventivoID, long CorrectivoID)
        {
            List<long> tipos;
            tipos = (from c in Context.ClientesProyectosTipos where (c.ClienteID == clienteID && (c.ProyectoTipoID == PreventivoID || c.ProyectoTipoID == CorrectivoID)) select c.ProyectoTipoID).ToList<long>();
            return (from c in Context.Vw_ClientesProyectosTipos where (tipos.Contains(c.ProyectoTipoID) && clienteID == c.ClienteID) select c).OrderBy("ProyectoTipo").ToList<Vw_ClientesProyectosTipos>();
        }

        public List<Vw_ClientesProyectosTipos> ProyectosTiposAsignados(long clienteID, string proyectoTipo)
        {
            List<long> tipos;
            tipos = (from c in Context.ClientesProyectosTipos where c.ClienteID == clienteID && c.ProyectosTipos.ProyectoTipo.StartsWith(proyectoTipo) select c.ProyectoTipoID).ToList<long>();
            return (from c in Context.Vw_ClientesProyectosTipos where (tipos.Contains(c.ProyectoTipoID) && clienteID == c.ClienteID) select c).OrderBy("ProyectoTipo").ToList<Vw_ClientesProyectosTipos>();
        }

        public List<Vw_ClientesProyectosTipos> ProyectosTiposAsignadosTorreros(long ModuloTorreros, long ModuloTorrerosCollo)
        {

            return (from c in Context.Vw_ClientesProyectosTipos where (c.ProyectoTipoID == ModuloTorreros) || (c.ProyectoTipoID == ModuloTorrerosCollo) select c).OrderBy("ProyectoTipo").ToList<Vw_ClientesProyectosTipos>();
        }

        public ProyectosTipos GetProyectosTiposByNombre(string nombre)
        {
            ProyectosTipos proyectoLocal;
            try
            {
                proyectoLocal = (from c
                                 in Context.ProyectosTipos
                                 where c.ProyectoTipo == nombre
                                 select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                proyectoLocal = null;
            }

            return proyectoLocal;
        }

        public long GetProyectosTiposIDByNombre(string nombre)
        {
            long lRes = 0;
            List<ProyectosTipos> tipos;
            ProyectosTipos proyectoLocal = null;
            tipos = (from c in Context.ProyectosTipos where c.ProyectoTipo == nombre select c).ToList<ProyectosTipos>();
            if (tipos != null && tipos.Count > 0)
            {
                proyectoLocal = tipos.ElementAt(0);
                lRes = proyectoLocal.ProyectoTipoID;
            }
            return lRes;
        }

        public List<Vw_ClientesProyectosTipos> GetProyectosTiposByClienteID(long clienteID)
        {
            return (from c in Context.Vw_ClientesProyectosTipos where (c.ClienteID == clienteID) select c).ToList<Vw_ClientesProyectosTipos>();
        }

        /// <summary>
        /// Devuelve el nombre del Proyecto Tipo
        /// </summary>
        public string GetProyectoTipo_nombre(long pProyectoTipoID)
        {
            string nombre = "";
            nombre = (from c in Context.ProyectosTipos where (c.ProyectoTipoID == pProyectoTipoID) select c).First().ProyectoTipo;

            return nombre;
        }

        public List<ProyectosTipos> GetProyectosTiposExceptNombre(string nombre)
        {
            List<ProyectosTipos> lista = null;
            ProyectosTipos proyectoLocal = null;
            try
            {
                lista = (from c in Context.ProyectosTipos where c.ProyectoTipo != nombre orderby c.ProyectoTipo select c).ToList<ProyectosTipos>();
                if (lista != null && lista.Count > 0)
                {
                    proyectoLocal = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        public long getidProyectoTipo(string ProyectoTipo)
        {

            long proyectoTipoid;
            try
            {
                proyectoTipoid = (from c in Context.ProyectosTipos where c.ProyectoTipo == ProyectoTipo select c.ProyectoTipoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                proyectoTipoid = new long();
            }
            return proyectoTipoid;
        }


        public long GetProyectosTiposIDByAlias(string nombre)
        {
            long lRes = 0;
            List<long> tipos;
            tipos = (from c in Context.ProyectosTipos where c.Alias == nombre select c.ProyectoTipoID).ToList();
            if (tipos != null && tipos.Count > 0)
            {
                lRes = tipos.ElementAt(0);

            }
            return lRes;
        }

        public ProyectosTipos GetProyectosTiposByID(long lID)
        {
            List<ProyectosTipos> tipos;
            ProyectosTipos proyectoLocal = null;
            tipos = (from c in Context.ProyectosTipos where c.ProyectoTipoID == lID select c).ToList<ProyectosTipos>();
            if (tipos != null && tipos.Count > 0)
            {
                proyectoLocal = tipos.ElementAt(0);
            }
            return proyectoLocal;
        }

        #region REPORTING

        public List<ProyectosTipos> GetAllProyectosTiposReporting()
        {
            // Local variables
            List<ProyectosTipos> lista = null;
            try
            {
                lista = (from c in Context.ProyectosTipos where c.IsReporting == true select c).OrderBy("ProyectoTipo").ToList<ProyectosTipos>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        #endregion

        public List<ProyectosTipos> GetListaOrderByProyectoTipo()
        {
            List<ProyectosTipos> proyectosTipos;

            try
            {
                proyectosTipos = (from c
                                  in Context.ProyectosTipos
                                  orderby c.ProyectoTipo
                                  select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                proyectosTipos = null;
            }

            return proyectosTipos;
        }

        public List<long> GetListProyectosTiposIDByAlias(string alias)
        {
            return (from c in Context.ProyectosTipos where c.Alias == alias select c.ProyectoTipoID).ToList();
        }

        public List<Vw_ClientesProyectosTipos> getProyectosByClienteID(long lClienteID)
        {
            List<Vw_ClientesProyectosTipos> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_ClientesProyectosTipos where (c.ClienteID == lClienteID) orderby c.ProyectoTipo select c).ToList<Vw_ClientesProyectosTipos>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<ProyectosTipos> GetAllProyectosTiposLibres(long proyectoID)
        {
            List<long> tipos;
            tipos = (from c in Context.ProyectosProyectosTipos where c.ProyectoID == proyectoID select c.ProyectoTipoID).ToList<long>();
            return (from c in Context.ProyectosTipos where !(tipos.Contains(c.ProyectoTipoID)) select c).OrderBy("ProyectoTipo").ToList<ProyectosTipos>();
        }

        public List<Vw_ClientesProyectosTipos> getProyectosByClienteIDMigrador(long lClienteID)
        {
            List<long> tiposProyectos;
            List<Vw_ClientesProyectosTipos> listaDatos = new List<Vw_ClientesProyectosTipos>();
            try
            {

                tiposProyectos = (from c in Context.MigradorTablas select c.ProyectoTipoID).ToList<long>();

                if (tiposProyectos != null)
                {
                    listaDatos = (from c in Context.Vw_ClientesProyectosTipos where (tiposProyectos.Contains(c.ProyectoTipoID) && lClienteID == c.ClienteID) select c).OrderBy("ProyectoTipo").ToList<Vw_ClientesProyectosTipos>();
                }
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