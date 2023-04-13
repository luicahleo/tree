using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class AvisosController : GeneralBaseController<Avisos, TreeCoreContext>, IBasica<Avisos>
    {
        public AvisosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string Aviso, long clienteID)
        {
            bool isExiste = false;
            List<Avisos> datos = new List<Avisos>();


            datos = (from c in Context.Avisos where (c.Aviso == Aviso /*&& c.ClienteID == clienteID*/) select c).ToList<Avisos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long AvisoID)
        {
            Avisos dato = new Avisos();
            AvisosController cController = new AvisosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && AvisoID == " + AvisoID.ToString());

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