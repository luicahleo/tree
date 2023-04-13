using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class EstadosDocumentosTiposController : GeneralBaseController<CoreEstadosDocumentosTipos, TreeCoreContext>
    {
        public EstadosDocumentosTiposController()
               : base()
        { }

        public List<CoreEstadosDocumentosTipos> getDocumentosByEstadoID(long lEstadoID)
        {
            List<CoreEstadosDocumentosTipos> listaDatos;

            try
            {
                listaDatos = (from c in Context.CoreEstadosDocumentosTipos where c.CoreEstadoID == lEstadoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public bool RegistroDuplicado(long lEstadoID, long lDocumentoTipoID)
        {
            bool isExiste = false;
            List<CoreEstadosDocumentosTipos> datos = new List<CoreEstadosDocumentosTipos>();

            datos = (from c in Context.CoreEstadosDocumentosTipos  where (c.CoreEstadoID == lEstadoID && c.DocumentoTipoID == lDocumentoTipoID) select c).ToList<CoreEstadosDocumentosTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

    }
}