using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreeCore.Clases;
using TreeCore.Data;

namespace TreeCore.CapaNegocio.Global.Administracion
{
    public sealed class EmplazamientosTiposEdificiosEmplazamientosTiposEstructurasController : GeneralBaseController<EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras, TreeCoreContext>, IGestionBasica<EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras>
    {
        public EmplazamientosTiposEdificiosEmplazamientosTiposEstructurasController()
            : base()
        { }

        public InfoResponse Add(EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
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

        public InfoResponse Update(EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
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

        public InfoResponse Delete(EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oResponse = DeleteEntity(oEntidad);
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

        public List<EmplazamientosTiposEstructuras> tiposEstructurasAsignadas(long tipoedificioID)
        {
            List<long> tiposEstructurasID;
            List<EmplazamientosTiposEstructuras> listaTipos = new List<EmplazamientosTiposEstructuras>();

            try
            {
                tiposEstructurasID = (from c in Context.EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras where c.EmplazamientoTipoEdificioID == tipoedificioID && c.Activo == true select c.EmplazamientoTipoEstructuraID).ToList<long>();
                listaTipos = (from c in Context.EmplazamientosTiposEstructuras where (tiposEstructurasID.Contains(c.EmplazamientoTipoEstructuraID) && c.Activo) select c).ToList<EmplazamientosTiposEstructuras>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTipos = null;
            }

            return listaTipos;
        }

        public List<EmplazamientosTiposEstructuras> tiposEstructurasNoAsignadas(long tipoedificioID)
        {
            List<long> tiposID;
            List<EmplazamientosTiposEstructuras> listaTipos = new List<EmplazamientosTiposEstructuras>();

            try
            {
                tiposID = (from c in Context.EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras where c.EmplazamientoTipoEdificioID == tipoedificioID && c.Activo == true select c.EmplazamientoTipoEstructuraID).ToList<long>();
                listaTipos = (from c in Context.EmplazamientosTiposEstructuras where (!tiposID.Contains(c.EmplazamientoTipoEstructuraID) && c.Activo) select c).ToList<EmplazamientosTiposEstructuras>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTipos = null;
            }
            return listaTipos;

        }

        public EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras GetTipoEstructuraID(long lTipoEdificioID, long lTipoEstructuraID)
        {
            EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras listaID;

            listaID = (from d in Context.EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras
                       where d.EmplazamientoTipoEdificioID == lTipoEdificioID && d.EmplazamientoTipoEstructuraID == lTipoEstructuraID
                       select d).First();

            return listaID;
        }
    }
}