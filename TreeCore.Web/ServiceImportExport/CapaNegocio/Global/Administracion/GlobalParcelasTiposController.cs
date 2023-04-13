using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalParcelasTiposController : GeneralBaseController<GlobalParcelasTipos, TreeCoreContext>, IBasica<GlobalParcelasTipos>
    {
        public GlobalParcelasTiposController()
            : base()
        { }

        public bool RegistroVinculado(long ParcelaTipoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string ParcelaTipo, long clienteID)
        {
            bool isExiste = false;
            List<GlobalParcelasTipos> datos = new List<GlobalParcelasTipos>();


            datos = (from c in Context.GlobalParcelasTipos where (c.ParcelaTipo == ParcelaTipo && c.ClienteID == clienteID) select c).ToList<GlobalParcelasTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ParcelaTipoID)
        {
            GlobalParcelasTipos dato = new GlobalParcelasTipos();
            GlobalParcelasTiposController cController = new GlobalParcelasTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ParcelaTipoID == " + ParcelaTipoID.ToString());

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