using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreeCore.Data;

namespace TreeCore.CapaNegocio.Global.Administracion
{
    public class ClientesProyectosTiposController : GeneralBaseController<ClientesProyectosTipos, TreeCoreContext>
    {
        public ClientesProyectosTiposController()
            : base()
        {

        }
        public ClientesProyectosTipos getClientesProyectosTipos(long clienteID, long proyectoTipoID)
        {
            return GetItem("ClienteID == " + clienteID.ToString() + "&& proyectoTipoID ==  " + proyectoTipoID.ToString());
        }


        public List<Vw_ClientesProyectosTipos> GetProyectosTiposMantenimiento(long clienteID)
        {
            return (from c in Context.Vw_ClientesProyectosTipos where (c.ProyectoTipo.Contains("Mantenimiento") && c.ClienteID == clienteID) select c).ToList<Vw_ClientesProyectosTipos>();

        }

        public List<Vw_ClientesProyectosTipos> GetProyectosTiposVandalismo(long clienteID)
        {
            return (from c in Context.Vw_ClientesProyectosTipos where (c.ProyectoTipo.Contains("Vandalismo") && c.ClienteID == clienteID) select c).ToList<Vw_ClientesProyectosTipos>();

        }

        public List<Vw_ClientesProyectosTipos> GetProyectosByCliente(long clienteID)
        {
            return (from c in Context.Vw_ClientesProyectosTipos where c.ClienteID == clienteID select c).ToList<Vw_ClientesProyectosTipos>();

        }
        public Vw_ClientesProyectosTipos GetProyectosByClienteYProyecto(long clienteID, long proyectoTipoID)
        {
            return (from c in Context.Vw_ClientesProyectosTipos where c.ClienteID == clienteID && c.ProyectoTipoID == proyectoTipoID select c).First();

        }

    }
}