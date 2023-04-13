using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class GlobalDireccionesTiposLlavesAccesosController : GeneralBaseController<GlobalDireccionesTiposLlavesAccesos, TreeCoreContext>, IBasica<GlobalDireccionesTiposLlavesAccesos>
    {
        public GlobalDireccionesTiposLlavesAccesosController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalDireccionTipoLlaveAccesoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string GlobalDireccionTipoLlaveAcceso, long clienteID)
        {
            bool isExiste = false;
            List<GlobalDireccionesTiposLlavesAccesos> datos = new List<GlobalDireccionesTiposLlavesAccesos>();


            datos = (from c in Context.GlobalDireccionesTiposLlavesAccesos where (c.Nombre == GlobalDireccionTipoLlaveAcceso && c.ClienteID == clienteID) select c).ToList<GlobalDireccionesTiposLlavesAccesos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalDireccionTipoLlaveAccesoID)
        {
            GlobalDireccionesTiposLlavesAccesos dato;
            GlobalDireccionesTiposLlavesAccesosController cController = new GlobalDireccionesTiposLlavesAccesosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalDireccionTipoLlaveAccesoID == " + GlobalDireccionTipoLlaveAccesoID.ToString());

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

        public GlobalDireccionesTiposLlavesAccesos GetDefault(long ClienteID)
        {
            GlobalDireccionesTiposLlavesAccesos direccionTipoLlaveAcceso;
            try
            {
                direccionTipoLlaveAcceso = (from c
                         in Context.GlobalDireccionesTiposLlavesAccesos
                         where c.Defecto &&
                                c.ClienteID == ClienteID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direccionTipoLlaveAcceso = null;
            }
            return direccionTipoLlaveAcceso;
        }
    }
}