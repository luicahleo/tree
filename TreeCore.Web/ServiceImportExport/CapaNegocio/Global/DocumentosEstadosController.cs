using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class DocumentosEstadosController : GeneralBaseController<DocumentosEstados, TreeCoreContext>
    {

        public DocumentosEstados GetDefecto()
        {
            DocumentosEstados estado;

            try
            {
                estado = (from c in Context.DocumentosEstados 
                          where c.Defecto 
                          select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                estado = null;
            }

            return estado;
        }
        public DocumentosEstados GetEstadobyNombre(string sNombre, long lClienteID)
        {
            DocumentosEstados estado;

            try
            {
                estado = (from c in Context.DocumentosEstados 
                          where c.Nombre == sNombre && c.ClienteID == lClienteID 
                          select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                estado = null;
            }

            return estado;
        }

        public long getIDByCodigo (string sCodigo)
        {
            long lDocumentoID;

            try
            {
                lDocumentoID = (from c in Context.DocumentosEstados where c.Codigo == sCodigo select c.DocumentoEstadoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lDocumentoID = 0;
            }

            return lDocumentoID;
        }

        //public List<DocumentosEstados> GetActivosLibres(long lEstadoID)
        //{
        //    List<DocumentosEstados> listaDatos;
        //    List<long?> listaIDs;

        //    try
        //    {
        //        listaIDs = (from c in Context.CoreEstadosGlobales where c.CoreEstadoID == lEstadoID select c.DocumentoEstadoID).ToList();
        //        listaDatos = (from c in Context.DocumentosEstados where c.Activo && !listaIDs.Contains(c.DocumentoEstadoID) select c).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        listaDatos = null;
        //    }

        //    return listaDatos;
        //}
        public List<DocumentosEstados> GetActivos(long clienteID)
        {
            List<DocumentosEstados> lista;

            try
            {
                lista = (from c in Context.DocumentosEstados 
                         where 
                            c.ClienteID == clienteID && 
                            c.Activo 
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<DocumentosEstados>();
            }

            return lista;
        }
    }
}