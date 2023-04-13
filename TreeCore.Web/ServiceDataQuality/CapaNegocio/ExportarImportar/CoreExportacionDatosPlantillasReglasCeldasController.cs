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
    public class CoreExportacionDatosPlantillasReglasCeldasController : GeneralBaseController<CoreExportacionDatosPlantillasReglasCeldas, TreeCoreContext>
    {
        public CoreExportacionDatosPlantillasReglasCeldasController()
            : base()
        { }

        public List<Vw_CoreExportacionDatosPlantillasReglasCeldas> GetByCeldaID(long celdaID)
        {
            List<Vw_CoreExportacionDatosPlantillasReglasCeldas> reglasCeldas;

            try
            {
                reglasCeldas = (from c in Context.Vw_CoreExportacionDatosPlantillasReglasCeldas
                                where c.CoreExportacionDatosPlantillasCeldaID == celdaID
                                select c).ToList();
            }
            catch (Exception ex) {
                log.Error(ex.Message);
                reglasCeldas = new List<Vw_CoreExportacionDatosPlantillasReglasCeldas>();
            }

            return reglasCeldas;
        }

        public List<Vw_CoreExportacionDatosPlantillasReglasCeldas> GetByPlantilla(long PlantillaID)
        {
            List<Vw_CoreExportacionDatosPlantillasReglasCeldas> reglasCeldas;

            try
            {
                reglasCeldas = (from c in Context.Vw_CoreExportacionDatosPlantillasReglasCeldas
                                join cel in Context.CoreExportacionDatosPlantillasCeldas on c.CoreExportacionDatosPlantillasCeldaID equals cel.CoreExportacionDatosPlantillasCeldasID
                                join f in Context.CoreExportacionDatosPlantillasFilas on cel.CoreExportacionDatosPlantillaFilaID equals f.CoreExportacionDatosPlantillaFilaID
                                where f.CoreExportacionDatosPlantillaID == PlantillaID
                                select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                reglasCeldas = new List<Vw_CoreExportacionDatosPlantillasReglasCeldas>();
            }

            return reglasCeldas;
        }

        public Vw_CoreExportacionDatosPlantillasReglasCeldas GetItemVw(long reglaID)
        {
            Vw_CoreExportacionDatosPlantillasReglasCeldas regla;
            try
            {
                regla = (from c in Context.Vw_CoreExportacionDatosPlantillasReglasCeldas
                         where c.CoreExportacionDatosPlantillasReglaCeldaID == reglaID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                regla = null;
            }

            return regla;
        }

        public int GetNuevoNumeroOrden(long celdaID)
        {
            int orden = 0;

            try
            {
                orden = (from c in Context.CoreExportacionDatosPlantillasReglasCeldas
                         where c.CoreExportacionDatosPlantillasCeldaID == celdaID
                         select c).ToList().OrderByDescending(s => s.Orden).First().Orden.Value;
                orden  = orden + 1;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            return orden;
        }

        public void ReordenarReglasByCelda(long celdaID)
        {
            List<CoreExportacionDatosPlantillasReglasCeldas> reglas;
            try
            {
                reglas = (from c in Context.CoreExportacionDatosPlantillasReglasCeldas
                          where c.CoreExportacionDatosPlantillasCeldaID.Value == celdaID
                          select c).ToList();

                reglas.Sort((x, y) => ComparerCoreExportacionDatosPlantillasReglasCeldas.Compare(x, y));

                for (int i = 0; i < reglas.Count; i++)
                {
                    reglas[i].Orden = i;
                    UpdateItem(reglas[i]);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                reglas = null;
            }

        }
    }

    class ComparerCoreExportacionDatosPlantillasReglasCeldas
    {
        public static int CompareVw(Vw_CoreExportacionDatosPlantillasReglasCeldas x, Vw_CoreExportacionDatosPlantillasReglasCeldas y)
        {
            if (x.CheckValorDefecto)
            {
                return 1;
            }
            else if (y.CheckValorDefecto)
            {
                return -1;
            }
            
            if (x.Orden < y.Orden)
            {
                return -1;
            }
            else if (x.Orden > y.Orden)
            {
                return 1;
            }

            return 0;
        }
        public static int Compare(CoreExportacionDatosPlantillasReglasCeldas x, CoreExportacionDatosPlantillasReglasCeldas y)
        {
            if (x.CheckValorDefecto)
            {
                return 1;
            }
            else if (y.CheckValorDefecto)
            {
                return -1;
            }

            if (x.Orden < y.Orden)
            {
                return -1;
            }
            else if (x.Orden > y.Orden)
            {
                return 1;
            }

            return 0;
        }
    }
}