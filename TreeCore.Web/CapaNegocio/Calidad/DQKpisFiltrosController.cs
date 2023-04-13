using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;
using System.Reflection;

namespace CapaNegocio
{
    public class DQKpisFiltrosController : GeneralBaseController<DQKpisFiltros, TreeCoreContext>
    {
        public DQKpisFiltrosController()
            : base()
        { }

        public List<Vw_DQKpisFiltros> GetFiltrosActivosPorKPI(long DQKpiID)
        {
            List<Vw_DQKpisFiltros> filtros;
            try
            {
                filtros = (from c in Context.Vw_DQKpisFiltros
                           where c.DQKpiID == DQKpiID 
                           select c).ToList();
            }
            catch (Exception ex)
            {
                filtros = null;
                log.Error(ex.Message);
            }
            return filtros;
        }

        public bool RegistroDuplicado (long lKpiID, long lColumna, long lOperador, string sValor, 
            string sFecha, string sNumber, bool bCheck, string sDinamic)
        {
            bool isExiste = false;
            string sValue = "0";
            List<Vw_DQKpisFiltros> listaDatos;

            try
            {
                if (bCheck)
                {
                    sValue = "1";
                }
                listaDatos = (from c in Context.Vw_DQKpisFiltros where (c.DQKpiID == lKpiID && c.ColumnaModeloDatoID == lColumna && c.TipoDatoOperadorID == lOperador &&
                              (c.Valor == sValor || c.Valor == sFecha || c.Valor == sNumber || c.Valor == sValue || c.Valor == sDinamic)) select c).ToList();
            
                if (listaDatos.Count > 0)
                {
                    isExiste = true;
                }
            }
            catch (Exception ex)
            {
                isExiste = false;
                log.Error(ex.Message);
            }

            return isExiste;
        }
    }
}