using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class EmplazamientosTiposEmplazamientosTiposEdificiosController : GeneralBaseController <EmplazamientosTiposEmplazamientosTiposEdificios, TreeCoreContext>
    {
        public EmplazamientosTiposEmplazamientosTiposEdificiosController()
            : base()
        { }
        public List<EmplazamientosTiposEdificios> tiposEdificiosAsignados(long tipoemplazamientoID)
        {
            List<long> tiposEdificiosID;
            List<EmplazamientosTiposEdificios> listaTipos = new List<EmplazamientosTiposEdificios>();

            try
            {
                tiposEdificiosID = (from c in Context.EmplazamientosTiposEmplazamientosTiposEdificios where c.EmplazamientoTipoID == tipoemplazamientoID && c.Activo == true select c.EmplazamientoTipoEdificioID).ToList<long>();
                listaTipos = (from c in Context.EmplazamientosTiposEdificios where (tiposEdificiosID.Contains(c.EmplazamientoTipoEdificioID) && c.Activo) select c).ToList<EmplazamientosTiposEdificios>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTipos = null;
            }

            return listaTipos;
        }



        public List<EmplazamientosTiposEdificios> tiposEdificiosNoAsignado(long tipoEmplazamientoID)
        {
            List<long> tiposID;
            List<EmplazamientosTiposEdificios> listaTipos = new List<EmplazamientosTiposEdificios>();

            try
            {
                tiposID = (from c in Context.EmplazamientosTiposEmplazamientosTiposEdificios where c.EmplazamientoTipoID == tipoEmplazamientoID && c.Activo == true select c.EmplazamientoTipoEdificioID).ToList<long>();
                listaTipos = (from c in Context.EmplazamientosTiposEdificios where (!(tiposID.Contains(c.EmplazamientoTipoEdificioID)) && c.Activo == true) select c).ToList<EmplazamientosTiposEdificios>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTipos = null;
            }
            return listaTipos;

        }

        public EmplazamientosTiposEmplazamientosTiposEdificios GetByTipoEdificioTipoID(long EmplazamientoTipoEdificioID, long EmplazamientoTipoID)
        {
            EmplazamientosTiposEmplazamientosTiposEdificios result;
            try
            {
                result = (from d in Context.EmplazamientosTiposEmplazamientosTiposEdificios
                          where
                             d.EmplazamientoTipoEdificioID == EmplazamientoTipoEdificioID &&
                             d.EmplazamientoTipoID == EmplazamientoTipoID
                          select d).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = null;
            }

            return result;
        }
    }
}
