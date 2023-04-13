using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;
using System.Reflection;

namespace CapaNegocio
{
    public class DQKpisGroupsRulesController : GeneralBaseController<DQKpisGroupsRules, TreeCoreContext>
    {
        public DQKpisGroupsRulesController()
            : base()
        { }

        public List<Vw_DQKpisGroupsRules> GetReglasByGrupo(long DQKpiGroupID)
        {
            List<Vw_DQKpisGroupsRules> reglas;
            try
            {
                reglas = (from c in Context.Vw_DQKpisGroupsRules
                          where 
                              c.Activo && 
                              c.DQKpiGroupID == DQKpiGroupID
                          select c).ToList();
            }
            catch (Exception ex)
            {
                reglas = null;
                log.Error(ex.Message);
            }
            return reglas;
        }

        public bool RegistroDuplicado(long lKpiGroupID, long lColumna, long lOperador, string sValor, 
            string sFecha, string sNumber, bool bCheck, string sDinamic)
        {
            bool isExiste = false;
            string sValue = "0";
            List<Vw_DQKpisGroupsRules> listaDatos;

            try
            {
                if (bCheck)
                {
                    sValue = "1";
                }
                listaDatos = (from c in Context.Vw_DQKpisGroupsRules
                              where (c.DQKpiGroupID == lKpiGroupID && c.ColumnaModeloDatosID == lColumna && c.TipoDatoOperadorID == lOperador &&
                                (c.Valor == sValor || c.Valor == sFecha || c.Valor == sNumber || c.Valor == sValue || c.Valor == sDinamic))
                              select c).ToList();

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