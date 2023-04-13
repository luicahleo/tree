using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace CapaNegocio
{
    public sealed class MantenimientoEmplazamientosSeguimientosController : GeneralBaseController<MantenimientoEmplazamientosSeguimientos, TreeCoreContext>
    {
        public MantenimientoEmplazamientosSeguimientosController()
            : base()
        {

        }
       


        public List<Vw_MantenimientoEmplazamientosSeguimientos> GetSeguimientoByDates(DateTime FechaInicial, DateTime FechaFinal, long? usuario, string proyectotipo)
        {
            MantenimientoEmplazamientosSeguimientosController cSeguimiento = new MantenimientoEmplazamientosSeguimientosController();
            List<Vw_MantenimientoEmplazamientosSeguimientos> listaSeguimiento = new List<Vw_MantenimientoEmplazamientosSeguimientos>();

  
            // Extraemos los elementos en las fechas seleccinadas
            if (usuario == null && proyectotipo == null)
            {
                listaSeguimiento = (from c in Context.Vw_MantenimientoEmplazamientosSeguimientos where (c.Fecha.Date >= FechaInicial.Date && c.Fecha.Date <= FechaFinal) select c).ToList();
            }

            if (usuario != null && proyectotipo == null)
            {

                listaSeguimiento = (from c in Context.Vw_MantenimientoEmplazamientosSeguimientos where (c.Fecha.Date >= FechaInicial.Date && c.Fecha.Date <= FechaFinal && c.UsuarioID == usuario) select c).ToList();

            }

            if (usuario == null && proyectotipo != null)
            {

                listaSeguimiento = (from c in Context.Vw_MantenimientoEmplazamientosSeguimientos where (c.Fecha.Date >= FechaInicial.Date && c.Fecha.Date <= FechaFinal && c.ProyectoTipo == proyectotipo) select c).ToList();

            }

            if (usuario != null && proyectotipo != null)
            {

                listaSeguimiento = (from c in Context.Vw_MantenimientoEmplazamientosSeguimientos where (c.Fecha.Date >= FechaInicial.Date && c.Fecha.Date <= FechaFinal && c.ProyectoTipo == proyectotipo && c.UsuarioID == usuario) select c).ToList();

            }



            return listaSeguimiento;
        }


        



    }
}

