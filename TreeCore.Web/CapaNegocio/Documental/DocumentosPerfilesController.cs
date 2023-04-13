using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class DocumentosPerfilesController : GeneralBaseController<DocumentosPerfiles, TreeCoreContext>
    {
        public DocumentosPerfilesController()
            : base()
        { }

        public List<Vw_DocumentosPerfiles> GetPerfilesByTipoDocumentoID(long tipoDocumentoID)
        {
            return (from c in Context.Vw_DocumentosPerfiles where c.TipoDocumentoID == tipoDocumentoID select c).ToList();
        }
        public List<DocumentosPerfiles> GetPerfilesByTipoDocumentoIDDefecto(long tipoDocumentoID)
        {
            return (from c in Context.DocumentosPerfiles where c.TipoDocumentoID == tipoDocumentoID && c.PerfilID == null select c).ToList();
        }

    }
}