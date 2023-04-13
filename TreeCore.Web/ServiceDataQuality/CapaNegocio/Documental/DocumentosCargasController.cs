using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class DocumentosCargasController : GeneralBaseController<DocumentosCargas, TreeCoreContext>
    {
        public DocumentosCargasController()
            : base()
        { }


        public List<DocumentosCargas> GetAllDocumentosNOProcesados()
        {

            List<DocumentosCargas> lista = null;

            try
            {
                lista = (from c in Context.DocumentosCargas where !c.Procesado && c.Activo orderby c.DocumentoCargaID ascending select c).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }



            return lista;
        }

        public List<Vw_DocumentosCargas> GetAllDocumentosNOProcesadosVw()
        {
            List<Vw_DocumentosCargas> lista;

            try
            {
                List<DocumentosCargas> listaNoProcesados = GetAllDocumentosNOProcesados();
                List<long> ids = listaNoProcesados.Select(c => c.DocumentoCargaID).ToList();

                lista = (from c in Context.Vw_DocumentosCargas
                         where ids.Contains(c.DocumentoCargaID)
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<Vw_DocumentosCargas> GetListaByFiltro(string sFiltro, long lClienteID)
        {
            List<Vw_DocumentosCargas> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.Vw_DocumentosCargas
                              where (c.DocumentoCarga.Contains(sFiltro) || c.Resultado.Contains(sFiltro)
                                || c.RutaDocumento.Contains(sFiltro) || c.RutaLog.Contains(sFiltro)
                                || c.Operador.Contains(sFiltro) || c.Proyecto.Contains(sFiltro)) && c.ClienteID == lClienteID
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        public bool RegistroDuplicado(string sNombre)
        {
            bool isExiste = false;
            List<DocumentosCargas> listaDatos = new List<DocumentosCargas>();


            listaDatos = (from c in Context.DocumentosCargas
                          where c.DocumentoCarga == sNombre
                          select c).ToList<DocumentosCargas>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

    }
}