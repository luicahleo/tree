using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreeCore.Data;

namespace TreeCore.CapaNegocio.Global.Administracion
{
    public sealed class EmplazamientosTiposEdificiosEmplazamientosTiposEstructurasController : GeneralBaseController<EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras, TreeCoreContext>
    {
        public EmplazamientosTiposEdificiosEmplazamientosTiposEstructurasController()
            : base()
        {

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

        public List<long> GetTipoEstructuraID (long lTipoEdificioID, long lTipoEstructuraID)
        {
            List<long> listaID;

            listaID = (from d in Context.EmplazamientosTiposEdificiosEmplazamientosTiposEstructuras
             where d.EmplazamientoTipoEdificioID == lTipoEdificioID && d.EmplazamientoTipoEstructuraID == lTipoEstructuraID
             select d.EmplazamientoTipoEdificioEmplazamientoTipoEstructuraID).ToList();

            return listaID;
        }
    }
}