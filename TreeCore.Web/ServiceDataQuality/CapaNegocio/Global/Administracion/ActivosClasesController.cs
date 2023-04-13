using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ActivosClasesController : GeneralBaseController<ActivosClases, TreeCoreContext>, IBasica<ActivosClases>
    {
        public ActivosClasesController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string ActivoClase, long clienteID)
        {
            bool isExiste = false;
            List<ActivosClases> datos = new List<ActivosClases>();


            datos = (from c in Context.ActivosClases where (c.ActivoClase == ActivoClase && c.ClienteID == clienteID) select c).ToList<ActivosClases>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ActivoClaseID)
        {
            ActivosClases dato = new ActivosClases();
            ActivosClasesController cController = new ActivosClasesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ActivoClaseID == " + ActivoClaseID.ToString());

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

        public ActivosClases GetDefault(long clienteID) {
            ActivosClases oActivosClases;
            try
            {
                oActivosClases = (from c in Context.ActivosClases where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oActivosClases = null;
            }
            return oActivosClases;
        }
    }
}