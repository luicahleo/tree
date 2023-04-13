using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class HistoricosCoreEmplazamientosController : GeneralBaseController<HistoricosCoreEmplazamientos, TreeCoreContext>
    {
        public HistoricosCoreEmplazamientosController()
            : base()
        {

        }

        public HistoricosCoreEmplazamientos getHistoricoByID(long emplazamientoID)
        {
            HistoricosCoreEmplazamientos res = new HistoricosCoreEmplazamientos();

            try
            {
                res = (from c in Context.HistoricosCoreEmplazamientos
                       where c.EmplazamientoID == emplazamientoID
                       orderby c.FechaModificacion descending
                       select c).First();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                res = null;
            }            

            return res;
        }

        internal List<Vw_HistoricosCoreEmplazamientos> GetVwHistoricosByID(long emplazamientoID)
        {
            List<Vw_HistoricosCoreEmplazamientos> res;

            res = (from c in Context.Vw_HistoricosCoreEmplazamientos
                   where c.EmplazamientoID == emplazamientoID
                   select c).ToList();

            return res;
        }

        internal List<Vw_HistoricosCoreEmplazamientos> GetVwHistoricosByByHistoricosIDs(List<long> listaIDS)
        {
            List<Vw_HistoricosCoreEmplazamientos> res;

            res = (from c in Context.Vw_HistoricosCoreEmplazamientos
                   where listaIDS.Contains(c.HistoricoCoreEmplazamientoID)
                   orderby c.FechaModificacion descending
                   select c).ToList();

            return res;
        }

        public List<long> GetLastModified(int days, long clienteID)
        {
            List<long> IDs;
            DateTime fecha = DateTime.Today;

            fecha = fecha.AddDays(-days);

            try
            {
                IDs = (from c in Context.HistoricosCoreEmplazamientos
                       where c.FechaModificacion > fecha
                       select c.EmplazamientoID).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                IDs = new List<long>();
            }

            return IDs;
        }

    }
}
