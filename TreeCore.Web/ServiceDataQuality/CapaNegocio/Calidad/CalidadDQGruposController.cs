using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public class DQGroupsController : GeneralBaseController<DQGroups, TreeCoreContext>
    {
        public DQGroupsController()
            : base()
        { }

        public bool RegistroVinculado(long DQGroupID)
        {
            bool bExiste = true;
         

            return bExiste;
        }

        public bool RegistroDuplicado(string DQGroup, long lClienteID)
        {
            bool bExiste = false;
            List<DQGroups> listaDatos;


            listaDatos = (from c in Context.DQGroups where (c.DQGroup == DQGroup && c.ClienteID == lClienteID) select c).ToList<DQGroups>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public List<Vw_DQGroups> getAllGroupActivos()
        {
            List<Vw_DQGroups> listaDatos = null;

            listaDatos = (from c in Context.Vw_DQGroups where c.Activo == true select c).ToList();

            return listaDatos;
        }

        public long getIDByName (string sGrupo)
        {
            long lGroupID = 0;

            lGroupID = (from c in Context.Vw_DQGroups where c.DQGroup == sGrupo select c.DQGroupID).First();

            return lGroupID;
        }

        public List<long> GetGroupsActivos()
        {
            List<long> listaID;

            try
            {
                listaID = (from c in Context.DQGroups where c.Activo select c.DQGroupID).ToList();
            }
            catch (Exception ex)
            {
                listaID = null;
                log.Error(ex.Message);
            }

            return listaID;
        }
    }
}