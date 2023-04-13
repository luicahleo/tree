using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalFrecuenciasTelcoController : GeneralBaseController<GlobalFrecuenciasTelco, TreeCoreContext>, IBasica<GlobalFrecuenciasTelco>
    {
        public GlobalFrecuenciasTelcoController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalFrecuenciaTelcoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string GlobalFrecuenciaTelco, long clienteID)
        {
            bool isExiste = false;
            List<GlobalFrecuenciasTelco> datos = new List<GlobalFrecuenciasTelco>();


            datos = (from c in Context.GlobalFrecuenciasTelco where (c.NombreFrecuenciaTelco == GlobalFrecuenciaTelco) select c).ToList<GlobalFrecuenciasTelco>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalFrecuenciaTelcoID)
        {
            GlobalFrecuenciasTelco dato = new GlobalFrecuenciasTelco();
            GlobalFrecuenciasTelcoController cController = new GlobalFrecuenciasTelcoController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalFrecuenciaTelcoID == " + GlobalFrecuenciaTelcoID.ToString());

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

        public GlobalFrecuenciasTelco GetDefault(long clienteID) {
            GlobalFrecuenciasTelco oGlobalFrecuenciasTelco;
            try
            {
                oGlobalFrecuenciasTelco = (from c in Context.GlobalFrecuenciasTelco where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGlobalFrecuenciasTelco = null;
            }
            return oGlobalFrecuenciasTelco;
        }
    }
}