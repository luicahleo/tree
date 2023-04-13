using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreExportacionDatosPlantillasHistoricosController : GeneralBaseController<CoreExportacionDatosPlantillasHistoricos, TreeCoreContext>
    {
        public CoreExportacionDatosPlantillasHistoricosController()
            : base()
        { }

        public long GetNewNumberVersion(long plantillaID)
        {
            long newVersion = 1;

            try
            {
                long? oldVersion = (from c in Context.CoreExportacionDatosPlantillasHistoricos
                                    where c.CoreExportacionDatosPlantillaID == plantillaID
                                    select c).OrderByDescending(history => history.Version).Select(history => history.Version).First();

                if (oldVersion.HasValue)
                {
                    newVersion = oldVersion.Value + 1;
                }

            }
            catch (Exception ex)
            {
                newVersion = 1;
                log.Error(ex.Message);
            }


            return newVersion;
        }

        public List<CoreExportacionDatosPlantillasHistoricos> GetActivosByPlantillaID(long plantillaID)
        {
            List<CoreExportacionDatosPlantillasHistoricos> historicos;

            try
            {
                historicos = (from c in Context.CoreExportacionDatosPlantillasHistoricos
                              where 
                                c.CoreExportacionDatosPlantillaID == plantillaID && 
                                c.Activo
                              select c).ToList();
            }
            catch(Exception ex)
            {
                historicos = null;
                log.Error(ex.Message);
            }

            return historicos;
        }

    }
}