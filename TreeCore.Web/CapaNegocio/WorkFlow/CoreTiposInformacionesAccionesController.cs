using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using System.Transactions;

namespace CapaNegocio
{
    public class CoreTiposInformacionesAccionesController : GeneralBaseController<CoreTiposInformacionesAcciones, TreeCoreContext>
    {
        public CoreTiposInformacionesAccionesController()
            : base()
        { }

        public List<CoreTareasAcciones> getLista (long lTipoInfoID)
        {
            List<CoreTareasAcciones> listaAcciones;

            try
            {
                listaAcciones = (from i in Context.CoreTiposInformacionesAcciones
                             join a in Context.CoreTiposInformaciones on i.CoreTipoInformacionID equals a.CoreTipoInformacionID
                             join b in Context.CoreTareasAcciones on i.CoreTareaAccionID equals b.CoreTareaAccionID
                                 where i.CoreTipoInformacionID == lTipoInfoID 
                             select b).ToList();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                listaAcciones = null;
            }

            return listaAcciones;
        }

        public CoreTiposInformacionesAcciones getIDByParametros (long lAccionID, long lTipoID)
        {
            CoreTiposInformacionesAcciones oTipoAccion;

            try
            {
                oTipoAccion = (from c in Context.CoreTiposInformacionesAcciones where c.CoreTareaAccionID == lAccionID && c.CoreTipoInformacionID == lTipoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oTipoAccion = null;
            }

            return oTipoAccion;
        }

    }
}