using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class VexCriticidadesController : GeneralBaseController<VexCriticidades, TreeCoreContext>, IBasica<VexCriticidades>
    {
        public VexCriticidadesController()
            : base()
        { }

        public bool RegistroVinculado(long VexCriticidadID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string VexCriticidad, long clienteID)
        {
            bool isExiste = false;
            List<VexCriticidades> datos = new List<VexCriticidades>();


            datos = (from c in Context.VexCriticidades where (c.VexCriticidad == VexCriticidad && c.ClienteID == clienteID) select c).ToList<VexCriticidades>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long VexCriticidadID)
        {
            VexCriticidades dato = new VexCriticidades();
            VexCriticidadesController cController = new VexCriticidadesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && VexCriticidadID == " + VexCriticidadID.ToString());

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

        public VexCriticidades GetDefault(long clienteID) {
            VexCriticidades oVexCriticidades;
            try
            {
                oVexCriticidades = (from c in Context.VexCriticidades where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oVexCriticidades = null;
            }
            return oVexCriticidades;
        }
    }
}