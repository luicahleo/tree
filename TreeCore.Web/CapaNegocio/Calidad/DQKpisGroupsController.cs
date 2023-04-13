using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;
using System.Reflection;

namespace CapaNegocio
{
    public class DQKpisGroupsController : GeneralBaseController<DQKpisGroups, TreeCoreContext>
    {
        public DQKpisGroupsController()
            : base()
        { }

        public List<Vw_DQKpisGroups> GetGruposActivosByKPI(long DQKpiID, long DQGroupID)
        {
            List<Vw_DQKpisGroups> grupos;
            try
            {
                grupos = (from c in Context.Vw_DQKpisGroups
                          where
                              c.Activo &&
                              c.DQKpiID == DQKpiID &&
                              DQGroupID == c.DQGroupID
                              
                          select c).ToList();
            }
            catch (Exception ex)
            {
                grupos = null;
                log.Error(ex.Message);
            }
            return grupos;
        }

        public List<DQKpisGroups> GetAllGroupsByKPI(long lKpi, bool bActivo)
        {
            List<DQKpisGroups> lista = null;

            try
            {
                if (bActivo)
                {
                    lista = (from c in Context.DQKpisGroups where c.Activo && c.DQKpiID == lKpi orderby c.DQKpiGroupID select c).ToList();
                }
                else
                {
                    lista = (from c in Context.DQKpisGroups where c.DQKpiID == lKpi orderby c.DQKpiGroupID select c).ToList();
                }
            }
            catch (Exception ex)
            {
                return lista;
                log.Error(ex.Message);
            }

            return lista;
        }

        public List<long> GetAllGroupsIdsByKPI(long lKpi, bool bActivo)
        {
            List<long> lista = null;

            try
            {
                if (bActivo)
                {
                    lista = (from c in Context.DQKpisGroups where c.Activo && c.DQKpiID == lKpi orderby c.DQKpiGroupID select c.DQKpiGroupID).ToList();
                }
                else
                {
                    lista = (from c in Context.DQKpisGroups where c.DQKpiID == lKpi orderby c.DQKpiGroupID select c.DQKpiGroupID).ToList();
                }
            }
            catch (Exception ex)
            {
                return lista;
                log.Error(ex.Message);
            }

            return lista;
        }

        public List<long> GetGroupsActivos()
        {
            List<long> listaID;

            try
            {
                listaID = (from c in Context.DQKpisGroups where c.Activo select c.DQKpiGroupID).ToList();
            }
            catch (Exception ex)
            {
                listaID = null;
                log.Error(ex.Message);
            }

            return listaID;
        }

        public bool RegistroDuplicado(long lKPI, string sGrupo, string sNombre)
        {
            bool isExiste = false;
            List<Vw_DQKpisGroups> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_DQKpisGroups
                              where c.DQKpiID == lKPI && c.DQGroup == sGrupo && c.NombreCondicion == sNombre 
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