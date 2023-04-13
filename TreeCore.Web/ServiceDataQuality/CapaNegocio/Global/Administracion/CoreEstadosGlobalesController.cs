using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreEstadosGlobalesController : GeneralBaseController<CoreEstadosGlobales, TreeCoreContext>
    {
        public CoreEstadosGlobalesController()
            : base()
        { }

        public List<CoreEstadosGlobales> getCoreEstadosGlobales(long lEstadoID)
        {
            List<CoreEstadosGlobales> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.CoreEstadosGlobales select c).ToList();
                listaDatos = (from EstGlo in Context.CoreEstadosGlobales
                              join Est in Context.Vw_CoreEstados on EstGlo.CoreEstadoID equals Est.CoreEstadoID
                              where EstGlo.CoreEstadoID == lEstadoID
                              select EstGlo).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public bool RegistroDuplicado(long lEstadoID, long? lEstadoGlobalID, long? lInvID, long? lDocID)
        {
            bool isExiste = false;
            List<CoreEstadosGlobales> datos = new List<CoreEstadosGlobales>();

            //if (lEstadoGlobalID != null)
            //{
            //    datos = (from c in Context.CoreEstadosGlobales
            //             where c.CoreEstadoID == lEstadoID && c.EstadoGlobalID == lEstadoGlobalID select c).ToList<CoreEstadosGlobales>();
            //}
            //else if (lInvID != null)
            //{
            //    datos = (from c in Context.CoreEstadosGlobales
            //             where c.CoreEstadoID == lEstadoID && c.InventarioElementoAtributoEstadoID == lInvID select c).ToList<CoreEstadosGlobales>();
            //}
            //else if (lDocID != null)
            //{
            //    datos = (from c in Context.CoreEstadosGlobales
            //             where c.CoreEstadoID == lEstadoID && c.DocumentoEstadoID == lDocID select c).ToList<CoreEstadosGlobales>();
            //}

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}