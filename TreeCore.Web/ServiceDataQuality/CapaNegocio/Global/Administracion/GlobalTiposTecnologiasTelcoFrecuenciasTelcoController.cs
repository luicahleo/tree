using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalTiposTecnologiasTelcoFrecuenciasTelcoController : GeneralBaseController<GlobalTiposTecnologiasTelcoFrecuenciasTelco, TreeCoreContext>
    {
        public GlobalTiposTecnologiasTelcoFrecuenciasTelcoController()
            : base()
        { }

        public void EliminarByTipoTecnologiaTelcoYFrecuencia(long TipoTecTelcoID, long FrecuenciaTelcoID)
        {
            GlobalTiposTecnologiasTelcoFrecuenciasTelcoController cGlobalTiposTecnologiasTelcoFrecuenciasTelco = new GlobalTiposTecnologiasTelcoFrecuenciasTelcoController();
            List<GlobalTiposTecnologiasTelcoFrecuenciasTelco> vinculados = (from c in Context.GlobalTiposTecnologiasTelcoFrecuenciasTelco where c.GlobalTipoTecnologiaTelcoID == TipoTecTelcoID && c.GlobalFrecuenciaTelcoID == FrecuenciaTelcoID select c).ToList<GlobalTiposTecnologiasTelcoFrecuenciasTelco>();

            foreach (GlobalTiposTecnologiasTelcoFrecuenciasTelco item in vinculados)
            {
                cGlobalTiposTecnologiasTelcoFrecuenciasTelco.DeleteItem(item.GlobalTipoTecnologiaTelcoFrecuenciaTelcoID);
            }
        }
    }
}