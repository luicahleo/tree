using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class DocumentosExtensionesController : GeneralBaseController<DocumentosExtensiones, TreeCoreContext>, IGestionBasica<DocumentosExtensiones>
    {
        public DocumentosExtensionesController()
            : base()
        { }

        public InfoResponse Add(DocumentosExtensiones oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                if (!RegistroDuplicado(oEntidad))
                {
                    oResponse = AddEntity(oEntidad);
                }
                else
                {
                    oResponse = new InfoResponse()
                    {
                        Description = GetGlobalResource("jsYaExiste"),
                        Result = false
                    };
                }
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

        public InfoResponse Update(DocumentosExtensiones oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                if (!RegistroDuplicado(oEntidad))
                {
                    oResponse = UpdateEntity(oEntidad);
                }
                else
                {
                    oResponse = new InfoResponse()
                    {
                        Description = GetGlobalResource("jsYaExiste"),
                        Result = false
                    };
                }
                
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

        public InfoResponse Delete(DocumentosExtensiones oEntidad)
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

        public bool RegistroVinculado(long DocumentoExtensionID)
        {
            bool bExiste = true;
         

            return bExiste;
        }

        public bool RegistroDuplicado(DocumentosExtensiones DocumentoExtension)
        {
            bool bExiste = false;
            List<DocumentosExtensiones> listaDatos;


            listaDatos = (from c in Context.DocumentosExtensiones 
                          where (
                            c.Extension == DocumentoExtension.Extension && 
                            c.ClienteID == DocumentoExtension.ClienteID &&
                            c.DocumentoExtensionID != DocumentoExtension.DocumentoExtensionID) 
                          select c).ToList<DocumentosExtensiones>();

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