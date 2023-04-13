using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalDireccionesAccesosController : GeneralBaseController<GlobalDireccionesAccesos, TreeCoreContext>
    {
        public GlobalDireccionesAccesosController()
            : base()
        { }
        public bool RegistroDuplicado(string sNombre, long lClienteID)
        {
            bool bExiste = false;
            List<GlobalDireccionesAccesos> listaDatos;

            listaDatos = (from c in Context.GlobalDireccionesAccesos where (c.Nombre == sNombre && c.ClienteID == lClienteID) select c).ToList<GlobalDireccionesAccesos>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }
        public bool RegistroDefecto(long GlobalDireccionAccesoID)
        {
            GlobalDireccionesAccesos dato = new GlobalDireccionesAccesos();
            GlobalDireccionesAccesosController cController = new GlobalDireccionesAccesosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalDireccionAccesoID == " + GlobalDireccionAccesoID.ToString());

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

        public GlobalDireccionesAccesos GetDefault(long lClienteID)
        {
            GlobalDireccionesAccesos oAcceso;

            try
            {
                oAcceso = (from c in Context.GlobalDireccionesAccesos where c.Defecto && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oAcceso = null;
            }

            return oAcceso;
        }
    }
}