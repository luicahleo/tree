using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class MonedasEvolucionesController : GeneralBaseController<MonedasEvoluciones, TreeCoreContext>, IGestionBasica<MonedasEvoluciones>
    {
        public MonedasEvolucionesController()
            : base()
        { }

        public InfoResponse Add(MonedasEvoluciones oEntidad)
        {
            MonedasEvolucionesController cEvolucion = new MonedasEvolucionesController();
            cEvolucion.SetDataContext(this.Context);

            InfoResponse oResponse;

            try
            {
                oResponse = AddEntity(oEntidad);

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

        public InfoResponse Update(MonedasEvoluciones oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = UpdateEntity(oEntidad);
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

        public InfoResponse Delete(MonedasEvoluciones oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = DeleteEntity(oEntidad);
                if (oResponse.Result)
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                }
                
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
    }
}