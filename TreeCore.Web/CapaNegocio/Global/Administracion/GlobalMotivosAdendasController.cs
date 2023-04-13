using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalMotivosAdendasController : GeneralBaseController<GlobalMotivosAdendas, TreeCoreContext>, IBasica<GlobalMotivosAdendas>
    {
        public GlobalMotivosAdendasController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalMotivoAdendaID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string MotivoAdenda, long clienteID)
        {
            bool isExiste = false;
            List<GlobalMotivosAdendas> datos = new List<GlobalMotivosAdendas>();


            datos = (from c in Context.GlobalMotivosAdendas where (c.MotivoAdenda == MotivoAdenda && c.ClienteID == clienteID) select c).ToList<GlobalMotivosAdendas>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalMotivoAdendaID)
        {
            GlobalMotivosAdendas dato = new GlobalMotivosAdendas();
            GlobalMotivosAdendasController cController = new GlobalMotivosAdendasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalMotivoAdendaID == " + GlobalMotivoAdendaID.ToString());

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

        public GlobalMotivosAdendas GetDefault(long clienteid)
        {
            GlobalMotivosAdendas oGlobalMotivosAdendas;
            try
            {
                oGlobalMotivosAdendas = (from c in Context.GlobalMotivosAdendas where c.Defecto && c.ClienteID == clienteid select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGlobalMotivosAdendas = null;
            }
            return oGlobalMotivosAdendas;
        }
    }
}