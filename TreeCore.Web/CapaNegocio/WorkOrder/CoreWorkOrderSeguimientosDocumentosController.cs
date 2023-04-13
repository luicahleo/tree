using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Clases;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreWorkOrderSeguimientosDocumentosController : GeneralBaseController<CoreWorkOrderSeguimientosDocumentos, TreeCoreContext>, IGestionBasica<CoreWorkOrderSeguimientosDocumentos>
    {
        public CoreWorkOrderSeguimientosDocumentosController()
            : base()
        { }

        public InfoResponse Add(CoreWorkOrderSeguimientosDocumentos oEntidad)
        {
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

        public InfoResponse Update(CoreWorkOrderSeguimientosDocumentos oEntidad)
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

        public InfoResponse Delete(CoreWorkOrderSeguimientosDocumentos oEntidad)
        {
            InfoResponse oResponse;

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

        public List<Documentos> GetDocumentosFromSeguimiento(long lSeguimientoID) {
            List<Documentos> listaDatos;
            try
            {
                listaDatos = (from doc in Context.Documentos
                              join
                                segDoc in Context.CoreWorkOrderSeguimientosDocumentos on doc.DocumentoID equals segDoc.DocumentoID
                              where segDoc.CoreWorkOrderSeguimientoID == lSeguimientoID && doc.Activo
                              select doc).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

    }
}