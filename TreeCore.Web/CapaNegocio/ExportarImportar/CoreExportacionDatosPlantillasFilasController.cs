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
    public class CoreExportacionDatosPlantillasFilasController : GeneralBaseController<CoreExportacionDatosPlantillasFilas, TreeCoreContext>
    {
        public CoreExportacionDatosPlantillasFilasController()
            : base()
        { }

        public List<CoreExportacionDatosPlantillasFilas> GetByPlantillaID(long plantillaID)
        {
            List<CoreExportacionDatosPlantillasFilas> lista;

            try
            {
                lista = (from c in Context.CoreExportacionDatosPlantillasFilas 
                         where c.CoreExportacionDatosPlantillaID == plantillaID 
                         select c).ToList();
            }
            catch (Exception ex)
            {
                lista = new List<CoreExportacionDatosPlantillasFilas>();
            }
            return lista;
        }

        public CoreExportacionDatosPlantillasFilas GetByCeldaID(long celdaID)
        {
            CoreExportacionDatosPlantillasFilas fila;

            try
            {
                fila = (from row in Context.CoreExportacionDatosPlantillasFilas 
                            join celda in Context.CoreExportacionDatosPlantillasCeldas on row.CoreExportacionDatosPlantillaFilaID equals celda.CoreExportacionDatosPlantillaFilaID
                        where celda.CoreExportacionDatosPlantillasCeldasID==celdaID
                        select row).First();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                fila = null;
            }

            return fila;
        }

    }
}