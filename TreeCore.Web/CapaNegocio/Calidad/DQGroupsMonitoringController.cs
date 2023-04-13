using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public class DQGroupsMonitoringController : GeneralBaseController<DQGroupsMonitoring, TreeCoreContext>
    {
        public DQGroupsMonitoringController()
            : base()
        { }

        public DQGroupsMonitoring GetUltimoByGrupo(long DQGroupID)
        {
            List<DQGroupsMonitoring> lista = null;
            DQGroupsMonitoring dato = null;

            try
            {
                lista = (from c in Context.DQGroupsMonitoring 
                         where 
                            c.Ultima && 
                            c.DQGroupID != null && 
                            c.DQGroupID == DQGroupID
                         select c).ToList();
                
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(lista.Count - 1);
                }

            }
            catch (Exception ex)
            {
                return dato;
                log.Error(ex.Message);
            }

            return dato;
        }

        public List<object> GetUltimosByKPI(long DQKpiID, int num)
        {
            List<object> lista;
            try
            {
                lista = (from grupo in Context.Vw_CoreDQGroupsMonitoring
                         join kpi in Context.Vw_DQKpisMonitoring on grupo.DQKpiID equals kpi.DQKpiID
                         join dqGroup in Context.DQGroups on grupo.DQGroupID equals dqGroup.DQGroupID
                         //join kpigrupo in Context.DQKpisGroups on grupoTabla.DQKpiGroupID equals kpigrupo.DQKpiGroupID
                         where 
                            kpi.Ultima &&
                            kpi.DQKpiID == DQKpiID &&
                            (grupo.Version == kpi.Version.ToString() || grupo.Version == (kpi.Version-1).ToString() || grupo.Version == (kpi.Version-2).ToString())
                         orderby grupo.Version descending
                         select new {
                             DQKpiID = grupo.DQKpiID,
                             Activa = grupo.Activa,
                             DQGroupMonitoringID = grupo.DQGroupMonitoringID,
                             FechaEjecucion = grupo.FechaEjecucion,
                             Total = grupo.Total,
                             Ultima = grupo.Ultima,
                             Version = grupo.Version,
                             DQGroup = grupo.DQGroup,
                             NombreCompleto = grupo.NombreCompleto,
                             NumeroElementos = grupo.NumeroElementos,
                             NombreTipoCondicion = dqGroup.DQGroup,
                             DQGroupID = dqGroup.DQGroupID
                         }
                         ).ToList<object>();
            }
            catch (Exception ex)
            {
                lista = null;
                log.Error(ex.Message);
            }

            return lista;
        }
    }
}