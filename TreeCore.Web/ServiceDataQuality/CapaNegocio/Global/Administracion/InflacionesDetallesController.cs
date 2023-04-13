using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class InflacionesDetallesController : GeneralBaseController<InflacionesDetalles, TreeCoreContext>
    {
        public InflacionesDetallesController()
            : base()
        {

        }

        public bool hasDuplicadosNuevoByFecha (long InflacionID, DateTime FechaDesde, DateTime FechaHasta, int Anualidad, int? Mes)
        {
            bool existe = false;
            List<InflacionesDetalles> detalles = new List<InflacionesDetalles>();

            detalles = (from c in Context.InflacionesDetalles where c.InflacionID == InflacionID && (c.FechaDesde != null && c.FechaDesde == FechaDesde) && (c.FechaHasta != null && c.FechaHasta == FechaHasta) select c).ToList();

            if (detalles.Count > 0)
            {
                existe = true;
            }
            else
            {
                detalles = (from c in Context.InflacionesDetalles where c.InflacionID == InflacionID && c.Anualidad == Anualidad && c.Mes == Mes select c).ToList();

                if (detalles.Count > 0)
                {
                    existe = true;
                }
            }

            return existe;
        }
    }
}
