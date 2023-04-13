using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreExportacionDatosPlantillasColumnasController : GeneralBaseController<CoreExportacionDatosPlantillasColumnas, TreeCoreContext>
    {
        public CoreExportacionDatosPlantillasColumnasController()
            : base()
        { }

        public List<CoreExportacionDatosPlantillasColumnas> GetByPlantillaID(long plantillaID)
        {
            List<CoreExportacionDatosPlantillasColumnas> lista;

            try
            {
                lista = (from col in Context.CoreExportacionDatosPlantillasColumnas
                         join celda in Context.CoreExportacionDatosPlantillasCeldas on col.CoreExportacionDatosPlantillaColumnaID equals celda.CoreExportacionDatosPlantillaColumnaID
                         join fila in Context.CoreExportacionDatosPlantillasFilas on celda.CoreExportacionDatosPlantillaFilaID equals fila.CoreExportacionDatosPlantillaFilaID
                         where fila.CoreExportacionDatosPlantillaID == plantillaID
                         select col).Distinct().ToList();
            }
            catch(Exception ex)
            {
                lista = new List<CoreExportacionDatosPlantillasColumnas>();
            }
            return lista;
        }

    }
}