using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalTiposTecnologiasTelcoController : GeneralBaseController<GlobalTiposTecnologiasTelco, TreeCoreContext>, IBasica<GlobalTiposTecnologiasTelco>
    {
        public GlobalTiposTecnologiasTelcoController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalTipoTecnologiaTelcoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string GlobalTipoTecnologiaTelco, long clienteID)
        {
            bool isExiste = false;
            List<GlobalTiposTecnologiasTelco> datos = new List<GlobalTiposTecnologiasTelco>();


            datos = (from c in Context.GlobalTiposTecnologiasTelco where (c.NombreTipoTecnologia == GlobalTipoTecnologiaTelco) select c).ToList<GlobalTiposTecnologiasTelco>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalTipoTecnologiaTelcoID)
        {
            GlobalTiposTecnologiasTelco dato = new GlobalTiposTecnologiasTelco();
            GlobalTiposTecnologiasTelcoController cController = new GlobalTiposTecnologiasTelcoController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalTipoTecnologiaTelcoID == " + GlobalTipoTecnologiaTelcoID.ToString());

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }

        public List<GlobalFrecuenciasTelco> frecuenciasNoAsignadas(long TipoTecTelcoID)
        {
            List<long> frecuencias;
            frecuencias = (from c in Context.GlobalTiposTecnologiasTelcoFrecuenciasTelco where c.GlobalTipoTecnologiaTelcoID == TipoTecTelcoID select c.GlobalFrecuenciaTelcoID).ToList<long>();
            return (from c in Context.GlobalFrecuenciasTelco where !(frecuencias.Contains(c.GlobalFrecuenciaTelcoID)) && c.Activo select c).ToList<GlobalFrecuenciasTelco>();

        }

        public List<GlobalFrecuenciasTelco> frecuenciasAsignadas(long TipoTecTelcoID)
        {
            List<long> frecuencias;
            frecuencias = (from c in Context.GlobalTiposTecnologiasTelcoFrecuenciasTelco where c.GlobalTipoTecnologiaTelcoID == TipoTecTelcoID select c.GlobalFrecuenciaTelcoID).ToList<long>();
            return (from c in Context.GlobalFrecuenciasTelco where (frecuencias.Contains(c.GlobalFrecuenciaTelcoID)) select c).ToList<GlobalFrecuenciasTelco>();

        }

        public GlobalTiposTecnologiasTelco GetDefault(long clienteID) {
            GlobalTiposTecnologiasTelco oGlobalTiposTecnologiasTelco;
            try
            {
                oGlobalTiposTecnologiasTelco = (from c in Context.GlobalTiposTecnologiasTelco where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGlobalTiposTecnologiasTelco = null;
            }
            return oGlobalTiposTecnologiasTelco;
        }
    }
}