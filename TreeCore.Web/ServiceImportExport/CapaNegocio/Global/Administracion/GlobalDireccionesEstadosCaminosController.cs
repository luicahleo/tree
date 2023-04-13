using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalDireccionesEstadosCaminosController : GeneralBaseController<GlobalDireccionesEstadosCaminos, TreeCoreContext>, IBasica<GlobalDireccionesEstadosCaminos>
    {
        public GlobalDireccionesEstadosCaminosController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalDireccionEstadoCaminoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string GlobalDireccionEstadoCamino, long clienteID)
        {
            bool isExiste = false;
            List<GlobalDireccionesEstadosCaminos> datos = new List<GlobalDireccionesEstadosCaminos>();


            datos = (from c in Context.GlobalDireccionesEstadosCaminos where (c.Nombre == GlobalDireccionEstadoCamino && c.ClienteID == clienteID) select c).ToList<GlobalDireccionesEstadosCaminos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalDireccionEstadoCaminoID)
        {
            GlobalDireccionesEstadosCaminos dato = new GlobalDireccionesEstadosCaminos();
            GlobalDireccionesEstadosCaminosController cController = new GlobalDireccionesEstadosCaminosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalDireccionEstadoCaminoID == " + GlobalDireccionEstadoCaminoID.ToString());

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

        public GlobalDireccionesEstadosCaminos GetDefault(long clienteID) {
            GlobalDireccionesEstadosCaminos oGlobalDireccionesEstadosCaminos;
            try
            {
                oGlobalDireccionesEstadosCaminos = (from c in Context.GlobalDireccionesEstadosCaminos where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGlobalDireccionesEstadosCaminos = null;
            }
            return oGlobalDireccionesEstadosCaminos;
        }
    }
}