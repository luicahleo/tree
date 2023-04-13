using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ProyectosAgrupacionesController : GeneralBaseController<ProyectosAgrupaciones, TreeCoreContext>, IBasica<ProyectosAgrupaciones>
    {
        public ProyectosAgrupacionesController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string ProyectoAgrupacion, long clienteID)
        {
            bool isExiste = false;
            List<ProyectosAgrupaciones> datos;

            datos = (from c
                     in Context.ProyectosAgrupaciones
                     where c.ProyectoAgrupacion.Equals(ProyectoAgrupacion) &&
                            c.ClienteID == clienteID
                     select c).ToList<ProyectosAgrupaciones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ProyectoAgrupacionID)
        {
            ProyectosAgrupaciones dato;

            dato = (from c
                    in Context.ProyectosAgrupaciones
                    where c.Defecto &&
                        c.ProyectoAgrupacionID == ProyectoAgrupacionID
                    select c).First();

            return (dato != null);
        }

        public ProyectosAgrupaciones GetDefault(long ClienteID)
        {
            ProyectosAgrupaciones proyectoAgrupacione;
            try
            {
                proyectoAgrupacione = (from c in Context.ProyectosAgrupaciones where c.Defecto && c.ClienteID == ClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                proyectoAgrupacione = null;
            }
            return proyectoAgrupacione;
        }

        /// <summary>
        /// Devuelve Las Agrupaciones pertenencientes a Un Cliente
        /// </summary>
        /// <param name="clienteID"></param>
        /// <returns></returns>
        public List<ProyectosAgrupaciones> getClientesProyectosAgrupaciones(long clienteID)
        {
            List<ProyectosAgrupaciones> agrupaciones = new List<ProyectosAgrupaciones>();
            agrupaciones = GetItemsList("ClienteID == " + clienteID.ToString());
            return agrupaciones.OrderBy(x => x.ProyectoAgrupacion).ToList();
        }

        public List<ProyectosAgrupaciones> getClientesProyectosAgrupacionesPermisosNoAsignados(long clienteID, List<long?> lProyAgrupAsignadosID)
        {
            List<ProyectosAgrupaciones> agrupaciones = new List<ProyectosAgrupaciones>();
            agrupaciones = (from c in Context.ProyectosAgrupaciones where c.ClienteID == clienteID && c.Activo == true && !lProyAgrupAsignadosID.Contains(c.ProyectoAgrupacionID) select c).ToList();
            return agrupaciones.OrderBy(x => x.ProyectoAgrupacion).ToList();
        }

        public List<long?> GetAgrupacionesAsignadasByUsuario(long usuarioID)
        {
            List<long?> datos = new List<long?>();
            datos = (from c in Context.Vw_ClientesProyectosZonas where c.UsuarioID.Equals(usuarioID) && !c.AlquilerTipoContratacionID.HasValue && !c.AlquilerTipoContratoID.HasValue select c.ProyectoAgrupacionID).ToList();
            return datos;
        }

    }
}