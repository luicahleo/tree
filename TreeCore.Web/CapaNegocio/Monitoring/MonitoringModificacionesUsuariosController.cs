using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class MonitoringModificacionesUsuariosController : GeneralBaseController<MonitoringModificacionesUsuarios, TreeCoreContext>, IGestionBasica<MonitoringModificacionesUsuarios>
    {
        public MonitoringModificacionesUsuariosController()
            : base()
        { }

        public InfoResponse Add(MonitoringModificacionesUsuarios oUser)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = AddEntity(oUser);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Update(MonitoringModificacionesUsuarios oUser)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = UpdateEntity(oUser);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Delete(MonitoringModificacionesUsuarios oUser)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = DeleteEntity(oUser);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }


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