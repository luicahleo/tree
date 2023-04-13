using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class InfraestructurasSubserviciosController : GeneralBaseController<GlobalSubElementosMarcos, TreeCoreContext>, IBasica<GlobalSubElementosMarcos>
    {
        public InfraestructurasSubserviciosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string GlobalSubElementoMarco, long clienteID)
        {
            bool isExiste = false;
            List<GlobalSubElementosMarcos> datos = new List<GlobalSubElementosMarcos>();


            datos = (from c in Context.GlobalSubElementosMarcos where (c.GlobalSubElemento == GlobalSubElementoMarco/* && c.ClienteID == clienteID*/) select c).ToList<GlobalSubElementosMarcos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicado2(string GlobalSubElementoMarco)
        {
            bool isExiste = false;
            List<GlobalSubElementosMarcos> datos = new List<GlobalSubElementosMarcos>();


            datos = (from c in Context.GlobalSubElementosMarcos where (c.GlobalSubElemento == GlobalSubElementoMarco) select c).ToList<GlobalSubElementosMarcos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalSubElementoMarcoID)
        {
            GlobalSubElementosMarcos dato = new GlobalSubElementosMarcos();
            InfraestructurasSubserviciosController cController = new InfraestructurasSubserviciosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalSubElementoMarcoID == " + GlobalSubElementoMarcoID.ToString());

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

        public GlobalSubElementosMarcos GetDefault(long lClienteID)
        {
            GlobalSubElementosMarcos oSubElemento;
            try
            {
                oSubElemento = (from c in Context.GlobalSubElementosMarcos where c.Defecto /*&& c.ClienteID == lClienteID*/select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oSubElemento = null;
            }

            return oSubElemento;
        }
    }
}