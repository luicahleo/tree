using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public class DocumentosExtensionesController : GeneralBaseController<DocumentosExtensiones, TreeCoreContext>
    {
        public DocumentosExtensionesController()
            : base()
        { }

        public bool RegistroVinculado(long DocumentoExtensionID)
        {
            bool bExiste = true;
         

            return bExiste;
        }

        public bool RegistroDuplicado(string DocumentoExtension, long lClienteID)
        {
            bool bExiste = false;
            List<DocumentosExtensiones> listaDatos;


            listaDatos = (from c in Context.DocumentosExtensiones where (c.Extension == DocumentoExtension && c.ClienteID == lClienteID) select c).ToList<DocumentosExtensiones>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public DocumentosExtensiones GetExtensionbyExtension(string DocumentoExtension, long lClienteID)
        {
            DocumentosExtensiones oDato;
            try
            {
                oDato = (from c in Context.DocumentosExtensiones where (c.Extension == DocumentoExtension && c.ClienteID == lClienteID) select c).First();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }
            return oDato;
        }

        
    }
}