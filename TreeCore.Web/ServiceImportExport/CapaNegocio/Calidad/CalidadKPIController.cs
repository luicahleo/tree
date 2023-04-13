using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class CalidadKPIController : GeneralBaseController<DQKpis, TreeCoreContext>
    {
        public CalidadKPIController()
             : base()
        {

        }

        public bool RegistroDuplicado(string sCalidad)
        {
            bool isExiste = false;
            List<DQKpis> datos = new List<DQKpis>();


            datos = (from c in Context.DQKpis where (c.DQKpi == sCalidad) select c).ToList<DQKpis>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}