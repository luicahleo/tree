using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class DocumentosRolesController : GeneralBaseController<DocumentosTiposRoles, TreeCoreContext>, IGestionBasica<DocumentosTiposRoles>
    {
        public DocumentosRolesController()
            : base()
        { }

        public InfoResponse Add(DocumentosTiposRoles oEntidad)
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

        public InfoResponse Update(DocumentosTiposRoles oEntidad)
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
                    Description = GetGlobalResource(Comun.strMensajeGenerico),
                    Data = null
                };
            }

            return oResponse;
        }

        public InfoResponse Delete(DocumentosTiposRoles oEntidad)
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

        public List<Vw_DocumentosTiposRoles> GetRolesByTipoDocumentoID(long tipoDocumentoID)
        {
            return (from c in Context.Vw_DocumentosTiposRoles where c.TipoDocumentoID == tipoDocumentoID select c).ToList();
        }
        public List<DocumentosTiposRoles> GetRolesByTipoDocumentoIDDefecto(long tipoDocumentoID)
        {
            return (from c in Context.DocumentosTiposRoles where c.TipoDocumentoID == tipoDocumentoID && c.RolID == null select c).ToList();
        }
        public List<Vw_DocumentosTiposRoles> GetRolesByRolID(long rolID)
        {
            return (from c in Context.Vw_DocumentosTiposRoles where c.RolID == rolID && c.DocumentTipo != null select c).ToList();
        }
        public DocumentosTiposRoles GetDocRol(long docTipoID)
        {
            return (from c in Context.DocumentosTiposRoles where c.DocumentoTipoRoleID == docTipoID select c).First();
        }

        public DocumentosTiposRoles GetRolByTipoDocumento(long tipoDocumentoID)
        {
            return (from c in Context.DocumentosTiposRoles where c.TipoDocumentoID == tipoDocumentoID select c).First();
        }

        public DocumentosTiposRoles getIDByDatos(long lRolID, long lTipoDocumentoID)
        {
            DocumentosTiposRoles oDato = new DocumentosTiposRoles();
            oDato = (from c in Context.DocumentosTiposRoles where c.TipoDocumentoID == lTipoDocumentoID && c.RolID == lRolID select c).First();

            return oDato;
        }
    }
}