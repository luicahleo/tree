using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public class DQKpisMonitoringController : GeneralBaseController<DQKpisMonitoring, TreeCoreContext>
    {
        public DQKpisMonitoringController()
            : base()
        { }

        public DQKpisMonitoring GetUltimoKPI(long DQKpiID)
        {
            DQKpisMonitoring ultimo;

            try
            {
                ultimo = (from c in Context.DQKpisMonitoring
                          where 
                            c.DQKpiID == DQKpiID && 
                            c.Activa && 
                            c.Ultima
                          select c).First();
            }
            catch (Exception ex)
            {
                ultimo = null;
                log.Error(ex.Message);
            }

            return ultimo;
        }

        public List<Vw_DQKpisMonitoring> GetUltimosKPIs(long?  DQCategoriaID)
        {
            List<Vw_DQKpisMonitoring> lista;

            try {
                IQueryable<Vw_DQKpisMonitoring> query = (from monitoring in Context.Vw_DQKpisMonitoring
                                                         join kpi in Context.DQKpis on monitoring.DQKpiID equals kpi.DQKpiID
                                                         where
                                                            monitoring.Ultima &&
                                                            monitoring.Activa &&
                                                            kpi.Activo
                                                         orderby monitoring.DQKpi
                                                         select monitoring);
                if ( DQCategoriaID.HasValue)
                {
                    query = query.Where(c => c.DQCategoriaID==DQCategoriaID);
                }

                lista = query.ToList();
            }
            catch (Exception ex)
            {
                lista = new List<Vw_DQKpisMonitoring>();
                log.Error(ex.Message);
            }

            return lista;
        }

        public Vw_DQKpisMonitoring GetUltimoKPIVista(long DQKpiID)
        {
            Vw_DQKpisMonitoring ultimo;

            try
            {
                ultimo = (from c in Context.Vw_DQKpisMonitoring
                          where
                            c.DQKpiID == DQKpiID &&
                            c.Activa &&
                            c.Ultima
                          select c).First();
            }
            catch (Exception ex)
            {
                ultimo = null;
                log.Error(ex.Message);
            }

            return ultimo;
        }

        public List<Vw_DQKpisMonitoring> getListaKPI(long lCategoriaID)
        {
            List<Vw_DQKpisMonitoring> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_DQKpisMonitoring where c.DQCategoriaID == lCategoriaID && c.Activa orderby c.Version descending select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        public List<Vw_DQKpisMonitoring> getAll()
        {
            List<Vw_DQKpisMonitoring> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.Vw_DQKpisMonitoring where c.Activa orderby c.Version descending select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }


        public List<Vw_DQKpisMonitoring> getByKPI(long DQKpiID)
        {
            List<Vw_DQKpisMonitoring> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_DQKpisMonitoring 
                              where  c.DQKpiID== DQKpiID 
                              orderby c.Version descending 
                              select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        public List<Vw_DQKpisMonitoring> getByKPIActivos(long DQKpiID)
        {
            List<Vw_DQKpisMonitoring> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_DQKpisMonitoring
                              where c.Activa && c.DQKpiID == DQKpiID
                              orderby c.Version descending
                              select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }


        public List<Vw_DQKpisMonitoring> GetUltimosKPIs(long DQKpiID)
        {
            List<Vw_DQKpisMonitoring> lista;

            try
            {
                IQueryable<Vw_DQKpisMonitoring> query = (from c in Context.Vw_DQKpisMonitoring
                                                         where
                                                            c.Activa &&
                                                            c.DQKpiID==DQKpiID
                                                         orderby c.Version descending
                                                         select c);
                lista = query.Take(3).ToList();
            }
            catch (Exception ex)
            {
                lista = new List<Vw_DQKpisMonitoring>();
                log.Error(ex.Message);
            }

            return lista;
        }


    }
}