using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class DocumentalTiposController : GeneralBaseController<DocumentTipos, TreeCoreContext>
    {
        public DocumentalTiposController()
            : base()
        { }

        public long getTipologiaID(string sTipo)
        {
            long lDatos;

            try
            {
                lDatos = (from c in Context.DocumentTipos where c.DocumentTipo == sTipo select c.DocumentTipoID).First();
            }
            catch (Exception ex)
            {
                lDatos = new long();
                log.Error(ex.Message);
            }

            return lDatos;
        }

        public List<DocumentTipos> GetAllTipologia(bool bActivo)
        {

            List<DocumentTipos> lista = null;

            try
            {
                if (bActivo)
                {
                    lista = (from c in Context.DocumentTipos where c.Activo == bActivo select c).ToList();
                }
                else
                {
                    lista = (from c in Context.DocumentTipos select c).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }



            return lista;
        }
        public long? GetDocumentTipoByNombre(String nombre)
        {
            long? lTipoID = -1;
            List<long> lTipos = null;
            try
            {
                lTipos = (from c in Context.DocumentTipos where c.DocumentTipo == nombre select c.DocumentTipoID).ToList<long>();

                if (lTipos != null && lTipos.Count > 0)
                {
                    lTipoID = lTipos.ElementAt(0);
                }
            }
            //tipos = (from c in Context.ClientesProyectosTipos where c.ClienteID == clienteID select c.ProyectoTipoID).ToList<long>();

            catch (Exception ex)
            {
                lTipoID = -1;
                log.Error(ex.Message);
            }
            return lTipoID;

        }
    }
}