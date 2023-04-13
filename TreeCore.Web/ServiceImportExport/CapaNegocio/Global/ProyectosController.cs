using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;


namespace CapaNegocio
{
    public class ProyectosController : GeneralBaseController<Proyectos, TreeCoreContext>
    {
        public ProyectosController()
            : base()
        { }

        public long? GetProyectoID(string sNombre)
        {
            long? lDatos = new long?();

            try
            {
                lDatos = (from c in Context.Proyectos where c.Proyecto == sNombre select c.ProyectoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lDatos = null;
            }

            return lDatos;
        }

        public long getIDByProyectoTipoID(long lProyectoTipoID)
        {
            long lDatos = new long();

            try
            {
                lDatos = (from c in Context.Proyectos where c.ProyectoTipoID == lProyectoTipoID select c.ProyectoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lDatos = 0;
            }

            return lDatos;
        }

        public Int64 ProyectoGestionFlujos(String ProyectoDesc)
        {
            Int64 proyectoid = 0;
            try
            {
                proyectoid = (from c in Context.Proyectos where c.Proyecto == ProyectoDesc select c.ProyectoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                proyectoid = 0;
            }
            return proyectoid;
        }

        public List<Proyectos> GetAll()
        {
            List<Proyectos> listaProyectos;
            ProyectosController cProyectos = new ProyectosController();

            try
            {
                listaProyectos = cProyectos.GetItemsList("");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaProyectos = null;
            }

            return listaProyectos;
        }

        public List<Proyectos> GetProyectosByClienteAgrupacionTipo(long ClienteID, long? TipoProyectoID, long? AgrupacionID)
        {
            List<Proyectos> datos = new List<Proyectos>();
            string filtro = "";
            filtro = "Activo && ClienteID == " + ClienteID.ToString();
            if (TipoProyectoID.HasValue)
                filtro += " && ProyectoTipoID == " + TipoProyectoID.Value.ToString();
            if (AgrupacionID.HasValue)
                filtro += " && ProyectoAgrupacionID == " + AgrupacionID.Value.ToString();

            datos = GetItemsList(filtro);
            return datos;
        }

        public List<Proyectos> GetProyectosByClienteAgrupacionTipo(long ClienteID, long? TipoProyectoID, long? AgrupacionID, string sort)
        {
            List<Proyectos> datos = new List<Proyectos>();
            string filtro = "";
            filtro = "Activo && ClienteID == " + ClienteID.ToString();
            if (TipoProyectoID.HasValue)
                filtro += " && ProyectoTipoID == " + TipoProyectoID.Value.ToString();
            if (AgrupacionID.HasValue)
                filtro += " && ProyectoAgrupacionID == " + AgrupacionID.Value.ToString();

            datos = GetItemsList(filtro, sort);
            return datos;
        }
        public Proyectos GetProyectoByNombreCliente(string sNombre, long? clienteID, long tipoProyecto)
        {
            List<Proyectos> datos = null;
            Proyectos sProyecto = null;

            if (clienteID != null)
            {
                datos = (from c
                         in Context.Proyectos
                         where c.Proyecto.ToUpper() == sNombre.ToUpper() &&
                                c.Activo &&
                             c.ClienteID == clienteID &&
                             c.ProyectoTipoID == tipoProyecto
                         select c).ToList();
            }
            else
            {
                datos = (from c
                         in Context.Proyectos
                         where c.Proyecto.ToUpper() == sNombre.ToUpper() &&
                                c.ProyectoTipoID == tipoProyecto
                         select c).ToList();
            }

            if (datos != null && datos.Count > 0)
            {
                sProyecto = datos.ElementAt(0);
            }

            return sProyecto;
        }

        public long GetPryectoByNombre(string Nombre)
        {

            long proyectoID = -1;
            List<long> lista = null;
            try
            {

                lista = (from c in Context.Proyectos where c.Proyecto.Equals(Nombre) select c.ProyectoID).ToList();
                if (lista != null && lista.Count > 0)
                {
                    proyectoID = lista.ElementAt(0)
;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                proyectoID = -1;

            }
            return proyectoID;
        }

        public List<long?> GetProyectosAsignadosByUsuario(long usuarioID)
        {
            List<long?> datos = new List<long?>();
            datos = (from c in Context.Vw_ClientesProyectosZonas where c.UsuarioID.Equals(usuarioID) && !c.AlquilerTipoContratacionID.HasValue && !c.AlquilerTipoContratoID.HasValue select c.ProyectoID).ToList();
            return datos;
        }
        public List<Proyectos> GetProyectosNoAsignadosByClienteAgrupacionListaProyectosTipos(long ClienteID, long? AgrupacionID, List<long?> lTiposProyectosID, List<long?> lProyectosAsignadosID)
        {
            List<Proyectos> datos = new List<Proyectos>();
            if (AgrupacionID != null && AgrupacionID != 0)
            {
                datos = (from c in Context.Proyectos where c.Activo && c.ClienteID == ClienteID && c.ProyectoAgrupacionID == AgrupacionID && lTiposProyectosID.Contains(c.ProyectoTipoID) && !lProyectosAsignadosID.Contains(c.ProyectoID) select c).ToList();
            }
            else
            {
                datos = (from c in Context.Proyectos where c.Activo && c.ClienteID == ClienteID && lTiposProyectosID.Contains(c.ProyectoTipoID) && !lProyectosAsignadosID.Contains(c.ProyectoID) select c).ToList();
            }

            return datos;
        }


        public List<Proyectos> GetAllProyectos()
        {
            List<Proyectos> listaProyectos;
            ProyectosController cProyectos = new ProyectosController();

            try
            {
                listaProyectos = (from c in Context.Proyectos where c.Activo == true orderby c.Proyecto select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaProyectos = null;
            }

            return listaProyectos;
        }
        public List<Vw_Proyectos> GetAllProyectosClienteID(long clienteID)
        {
            List<Vw_Proyectos> listaProyectos;

            try
            {
                listaProyectos = (from c in Context.Vw_Proyectos where c.ClienteID == clienteID && c.Activo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaProyectos = null;
            }

            return listaProyectos;
        }

        public List<ProyectosFases> GetAllProyectosFases(long proyectoID)
        {
            List<ProyectosFases> listaProyectos;

            try
            {
                listaProyectos = (from c in Context.ProyectosFases where c.ProyectoID == proyectoID orderby c.Fase select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaProyectos = null;
            }

            return listaProyectos;
        }


        public long getProyectoTipoByNombre(string Nombre)
        {

            long lProyectoTipoID = 0;

            try
            {
                lProyectoTipoID = (from c in Context.Proyectos where c.Proyecto.Equals(Nombre) select c.ProyectoTipoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lProyectoTipoID = 0;
            }

            return lProyectoTipoID;
        }

        public bool controlDuplicidad(string nombre, string codigo, long clienteID)
        {
            bool control = false;
            List<Proyectos> listaProyectos=null;
            try
            {
                listaProyectos = (from c in Context.Proyectos where c.Proyecto == nombre || c.Referencia==codigo && c.ClienteID==clienteID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            if (listaProyectos.Count > 0)
            {
                control = true;
            }
            return control;
        }

    }
}