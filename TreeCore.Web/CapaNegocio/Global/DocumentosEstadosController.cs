using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Clases;
using TreeCore.Data;

namespace CapaNegocio
{
    public class DocumentosEstadosController : GeneralBaseController<DocumentosEstados, TreeCoreContext>, IGestionBasica<DocumentosEstados>
    {

        public InfoResponse Add(DocumentosEstados oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                if(!RegistroDuplicado(oEntidad.Nombre, oEntidad.Codigo, oEntidad.ClienteID, oEntidad.DocumentoEstadoID))
                {
                    oResponse = AddEntity(oEntidad);
                }
                else
                {
                    oResponse = new InfoResponse()
                    {
                        Result = false,
                        Code = "",
                        Description = GetGlobalResource(Comun.LogRegistroExistente),
                        Data = null
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

        public InfoResponse Update(DocumentosEstados oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicadoNombre(oEntidad.Nombre, oEntidad.ClienteID, oEntidad.DocumentoEstadoID) || 
                    RegistroDuplicadoCodigo(oEntidad.Codigo, oEntidad.ClienteID, oEntidad.DocumentoEstadoID))
                {
                    oResponse = new InfoResponse()
                    {
                        Result = false,
                        Code = "",
                        Description = GetGlobalResource(Comun.LogRegistroExistente),
                        Data = null
                    };
                }
                else
                {
                    oResponse = UpdateEntity(oEntidad);
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

        public InfoResponse Delete(DocumentosEstados oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDefecto(oEntidad.DocumentoEstadoID))
                {
                    oResponse = new InfoResponse()
                    {
                        Description = GetGlobalResource(Comun.jsPorDefecto),
                        Result = false,
                        Data = null
                    };
                }
                else
                {
                    oResponse = DeleteEntity(oEntidad);
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

        public string getNombreByID(long lEstadoGlobalID)
        {
            string sNombre;

            try
            {
                sNombre = (from c in Context.DocumentEstados where c.DocumentEstadoID == lEstadoGlobalID select c.DocumentEstado).First();
            }
            catch (Exception)
            {
                sNombre = null;
            }

            return sNombre;
        }

        public bool RegistroVinculado(long DocumentoExtensionID)
        {
            bool bExiste = true;


            return bExiste;
        }

        public bool RegistroDuplicado(string Nombre, string Codigo, long lClienteID, long DocumentoEstadoID)
        {
            bool bExiste = false;
            List<DocumentosEstados> listaDatos;


            listaDatos = (from c in Context.DocumentosEstados 
                          where ((c.Nombre == Nombre || c.Codigo == Codigo) && c.ClienteID == lClienteID && c.DocumentoEstadoID != DocumentoEstadoID) 
                          select c).ToList<DocumentosEstados>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public bool RegistroDuplicadoNombre(string Nombre, long lClienteID, long DocumentoEstadoID)
        {
            bool bExiste = false;
            List<DocumentosEstados> listaDatos;


            listaDatos = (from c in Context.DocumentosEstados 
                          where (c.Nombre == Nombre && c.ClienteID == lClienteID && c.DocumentoEstadoID != DocumentoEstadoID) 
                          select c).ToList<DocumentosEstados>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }
        public bool RegistroDuplicadoCodigo(string Codigo, long lClienteID, long DocumentoEstadoID)
        {
            bool bExiste = false;
            List<DocumentosEstados> listaDatos;


            listaDatos = (from c in Context.DocumentosEstados 
                          where (c.Codigo == Codigo && c.ClienteID == lClienteID && c.DocumentoEstadoID != DocumentoEstadoID) 
                          select c).ToList<DocumentosEstados>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }
        public DocumentosEstados GetDefault(long clienteID)
        {
            DocumentosEstados docEstado;
            try
            {
                docEstado = (from c in Context.DocumentosEstados 
                             where 
                                c.Defecto && 
                                c.ClienteID == clienteID 
                             select c).First();
            }
            catch (Exception ex)
            {
                docEstado = null;
                log.Error(ex.Message);
            }
            return docEstado;
        }

        public bool RegistroDefecto(long DocumentoEstadoID)
        {
            DocumentosEstados dato = new DocumentosEstados();
            bool defecto = false;

            dato = GetItem("Defecto == true && DocumentoEstadoID == " + DocumentoEstadoID.ToString());

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }

        public InfoResponse AsignarPorDefecto(long IdDocumentoEstado, long clienteID)
        {
            DocumentosEstados oDato;
            InfoResponse infoResponse;

            // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
            oDato = GetDefault(clienteID);

            // SI HAY Y ES DISTINTO AL SELECCIONADO
            if (oDato != null)
            {  
                if (oDato.Defecto)
                {
                    oDato.Defecto = !oDato.Defecto;
                    infoResponse = Update(oDato);
                }

                oDato = GetItem(IdDocumentoEstado);
                oDato.Defecto = true;
                oDato.Activo = true;
                infoResponse = Update(oDato);
                SubmitChanges();
            }
            // SI NO HAY ELEMENTO POR DEFECTO
            else
            {
                oDato = GetItem(IdDocumentoEstado);
                oDato.Defecto = true;
                oDato.Activo = true;
                infoResponse = Update(oDato);
            }

            return infoResponse;
        }
    }
}