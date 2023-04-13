using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalMotorGeneradorTipoController : GeneralBaseController<GlobalMotorGeneradorTipo, TreeCoreContext>, IBasica<GlobalMotorGeneradorTipo>
    {
        public GlobalMotorGeneradorTipoController()
            : base()
        { }

        public bool RegistroVinculado(long TipoMotorGeneradorID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string TipoMotorGenerador, long clienteID)
        {
            bool isExiste = false;
            List<GlobalMotorGeneradorTipo> datos = new List<GlobalMotorGeneradorTipo>();


            datos = (from c in Context.GlobalMotorGeneradorTipo where (c.TipoMotorGenerador == TipoMotorGenerador && c.ClienteID == clienteID) select c).ToList<GlobalMotorGeneradorTipo>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long TipoMotorGeneradorID)
        {
            GlobalMotorGeneradorTipo dato = new GlobalMotorGeneradorTipo();
            GlobalMotorGeneradorTipoController cController = new GlobalMotorGeneradorTipoController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && TipoMotorGeneradorID == " + TipoMotorGeneradorID.ToString());

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