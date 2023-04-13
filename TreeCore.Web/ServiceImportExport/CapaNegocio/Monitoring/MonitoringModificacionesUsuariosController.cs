using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MonitoringModificacionesUsuariosController : GeneralBaseController<MonitoringModificacionesUsuarios, TreeCoreContext>
    {
        public MonitoringModificacionesUsuariosController()
            : base()
        { }

        public bool RegistroVinculado(long MonitoringModificacionUsuarioID)
        {
            bool bExiste = true;
         

            return bExiste;
        }

        

        public bool RegistroDefecto(long MonitoringModificacionUsuarioID)
        {
            MonitoringModificacionesUsuarios oDato;
            MonitoringModificacionesUsuariosController cController = new MonitoringModificacionesUsuariosController();
            bool bDefecto = false;

            oDato = cController.GetItem("Defecto == true && MonitoringModificacionUsuarioID == " + MonitoringModificacionUsuarioID.ToString());

            if (oDato != null)
            {
                bDefecto = true;
            }
            else
            {
                bDefecto = false;
            }

            return bDefecto;
        }

        
    }
}