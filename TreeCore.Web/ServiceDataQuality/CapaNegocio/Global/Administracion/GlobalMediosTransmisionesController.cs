using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalMediosTransmisionesController : GeneralBaseController<GlobalMediosTransmisiones, TreeCoreContext>, IBasica<GlobalMediosTransmisiones>
    {
        public GlobalMediosTransmisionesController()
            : base()
        { }

        public bool RegistroVinculado(long MedioTransmisionID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string MedioTransmision, long clienteID)
        {
            bool isExiste = false;
            List<GlobalMediosTransmisiones> datos = new List<GlobalMediosTransmisiones>();


            datos = (from c in Context.GlobalMediosTransmisiones where (c.MedioTransmision == MedioTransmision && c.ClienteID == clienteID) select c).ToList<GlobalMediosTransmisiones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long MedioTransmisionID)
        {
            GlobalMediosTransmisiones dato = new GlobalMediosTransmisiones();
            GlobalMediosTransmisionesController cController = new GlobalMediosTransmisionesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && MedioTransmisionID == " + MedioTransmisionID.ToString());

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
    }
}