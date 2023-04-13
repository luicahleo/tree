using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class DocumentosRolesController : GeneralBaseController<DocumentosTiposRoles, TreeCoreContext>
    {
        public DocumentosRolesController()
            : base()
        { }

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