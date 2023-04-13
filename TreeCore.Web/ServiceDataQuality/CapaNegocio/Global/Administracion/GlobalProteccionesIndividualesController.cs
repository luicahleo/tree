using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalProteccionesIndividualesController : GeneralBaseController<GlobalProteccionesIndividuales, TreeCoreContext>, IBasica<GlobalProteccionesIndividuales>
    {
        public GlobalProteccionesIndividualesController()
            : base()
        { }

        public bool RegistroVinculado(long ProteccionIndividualID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string ProteccionIndividual, long clienteID)
        {
            bool isExiste = false;
            List<GlobalProteccionesIndividuales> datos = new List<GlobalProteccionesIndividuales>();


            datos = (from c in Context.GlobalProteccionesIndividuales where (c.ProteccionIndividual == ProteccionIndividual && c.ClienteID == clienteID) select c).ToList<GlobalProteccionesIndividuales>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ProteccionIndividualID)
        {
            GlobalProteccionesIndividuales dato = new GlobalProteccionesIndividuales();
            GlobalProteccionesIndividualesController cController = new GlobalProteccionesIndividualesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ProteccionIndividualID == " + ProteccionIndividualID.ToString());

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