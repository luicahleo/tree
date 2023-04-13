using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;
using Ext.Net;
using TreeCore.Clases;
using System.IO;

namespace CapaNegocio
{
    public class DocumentTiposController : GeneralBaseController<DocumentTipos, TreeCoreContext>
    {
        public DocumentTiposController()
            : base()
        { }

        public InfoResponse Add(DocumentTipos oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                if (CheckExisteDocTipoMismoNombre(oEntidad.DocumentTipo, oEntidad.DocumentTipoID) == null)
                {
                    oResponse = AddEntity(oEntidad);
                }
                else
                {
                    oResponse = new InfoResponse()
                    {
                        Description = GetGlobalResource("strCarpetaExistente"),
                        Data = null,
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

        public InfoResponse Update(DocumentTipos oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                if (CheckExisteDocTipoMismoNombre(oEntidad.DocumentTipo, oEntidad.DocumentTipoID) == null)
                {
                    oResponse = UpdateEntity(oEntidad);
                }
                else
                {
                    oResponse = new InfoResponse()
                    {
                        Result = false,
                        Description = GetGlobalResource("strCarpetaExistente")
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

        public InfoResponse Delete(DocumentTipos oEntidad)
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

        public InfoResponse EditarDocumentTipos(DocumentTipos docTipo, string nombreAntiguo, List<string> recordsIDs, 
            bool chkPermisoLectura, bool chkPermisoEscritura, bool chkPermisoDescarga)
        {
            InfoResponse infoResponse;
            DocumentosTiposExtensionesController cDocTiposExt = new DocumentosTiposExtensionesController();
            DocumentosRolesController cDocTipRoles = new DocumentosRolesController();
            cDocTiposExt.SetDataContext(this.Context);
            cDocTipRoles.SetDataContext(this.Context);


            try
            {
                #region TIPO DOCUMENTO

                if (docTipo.DocumentTipo != docTipo.DocumentTipo)
                {
                    if (!CheckExisteTipoDocumentos(docTipo))
                    {
                        docTipo.DocumentTipo = docTipo.DocumentTipo;
                        infoResponse = Update(docTipo);
                        editarRutaDocumento(docTipo, nombreAntiguo);
                    }
                    else
                    {
                        DiscardChanges();

                        infoResponse = new InfoResponse()
                        {
                            Description = GetGlobalResource(Comun.strDocTipoExistente),
                            Result = false
                        };
                    }
                }

                #region DOCUMENTOS TIPOS EXTENSIONES
                GetDoctiposExtByDocTipoID(docTipo.DocumentTipoID).ForEach(ext => { cDocTiposExt.Delete(ext); });
                foreach (string row in recordsIDs)
                {
                    DocumentosTiposExtensiones newDocTiposExt = new DocumentosTiposExtensiones();
                    newDocTiposExt.DocumentTipos = docTipo;
                    newDocTiposExt.DocumentoExtensionID = GetDocExtensionesByClienteIDYExtension(docTipo.ClienteID.Value, row).DocumentoExtensionID;
                    infoResponse = cDocTiposExt.Add(newDocTiposExt);
                    if (infoResponse.Result)
                    {
                        newDocTiposExt = (DocumentosTiposExtensiones)infoResponse.Data;
                    }
                    else
                    {
                        log.Error(infoResponse.Description);
                    }
                }
                #endregion

                #region DOCUMENTOS ROLES

                List<DocumentosTiposRoles> lDocTipRoles = cDocTipRoles.GetRolesByTipoDocumentoIDDefecto(docTipo.DocumentTipoID);
                DocumentosTiposRoles docTipoRol;
                if (lDocTipRoles.Count > 0)
                {
                    docTipoRol = lDocTipRoles[0];
                    docTipoRol.PermisoLectura = chkPermisoLectura;
                    docTipoRol.PermisoEscritura = chkPermisoEscritura;
                    docTipoRol.PermisoDescarga = chkPermisoDescarga;
                    docTipoRol.Activo = true;
                    infoResponse = cDocTipRoles.Update(docTipoRol);
                }
                else
                {
                    docTipoRol = new DocumentosTiposRoles();
                    docTipoRol.TipoDocumentoID = docTipo.DocumentTipoID;
                    docTipoRol.PermisoLectura = chkPermisoLectura;
                    docTipoRol.PermisoEscritura = chkPermisoEscritura;
                    docTipoRol.PermisoDescarga = chkPermisoDescarga;
                    docTipoRol.Activo = true;
                    infoResponse = cDocTipRoles.Add(docTipoRol);
                }

                #endregion

                infoResponse = SubmitChanges();

                #endregion

            }
            catch (Exception ex)
            {
                DiscardChanges();
                log.Error(ex.Message);
                
                infoResponse = new InfoResponse() {
                    Description = GetGlobalResource(Comun.strMensajeGenerico),
                    Result = false
                };
            }

            return infoResponse;
        }

        public InfoResponse CrearDocumentTipos(DocumentTipos documentTipo, List<string> recordsIDs,
            bool chkPermisoLectura, bool chkPermisoEscritura, bool chkPermisoDescarga)
        {
            DocumentosTiposExtensionesController cDocTiposExt = new DocumentosTiposExtensionesController();
            DocumentosRolesController cDocTipRoles = new DocumentosRolesController();
            cDocTiposExt.SetDataContext(this.Context);
            cDocTipRoles.SetDataContext(this.Context);

            InfoResponse infoResponse;

            try
            {
                #region TIPO DOCUMENTO

                if (!CheckExisteTipoDocumentos(documentTipo))
                {
                    infoResponse = Add(documentTipo);

                    if (infoResponse.Result)
                    {
                        CrearRutaDocumento(documentTipo);
                    }

                    #region DOCUMENTOS TIPOS EXTENSIONES

                    foreach (string row in recordsIDs)
                    {
                        DocumentosTiposExtensiones newDocTiposExt = new DocumentosTiposExtensiones();
                        //newDocTiposExt.DocumentTipoID = documentTipo.DocumentTipoID;
                        newDocTiposExt.DocumentTipos = documentTipo;
                        newDocTiposExt.DocumentoExtensionID = GetDocExtensionesByClienteIDYExtension(documentTipo.ClienteID.Value, row).DocumentoExtensionID;
                        infoResponse = cDocTiposExt.Add(newDocTiposExt);
                        if (infoResponse.Result)
                        {

                        }
                        else
                        {
                            log.Error(infoResponse.Description);
                        }

                    }
                    #endregion

                    #region DOCUMENTOS ROLES DEFECTO
                    DocumentosTiposRoles docTiposRoles = new DocumentosTiposRoles();
                    docTiposRoles.DocumentTipos = documentTipo;
                    docTiposRoles.Activo = true;
                    docTiposRoles.PermisoLectura = chkPermisoLectura;
                    docTiposRoles.PermisoEscritura = chkPermisoEscritura;
                    docTiposRoles.PermisoDescarga = chkPermisoDescarga;
                    infoResponse = cDocTipRoles.Add(docTiposRoles);
                    if (infoResponse.Result)
                    {

                    }
                    else
                    {
                        log.Error(infoResponse.Description);
                    }
                    #endregion

                }
                else
                {
                    DiscardChanges();
                    infoResponse = new InfoResponse()
                    {
                        Result = false,
                        Description = GetGlobalResource(Comun.strDocTipoExistente)
                    };
                }

                infoResponse = SubmitChanges();

                #endregion
            }
            catch (Exception ex)
            {
                DiscardChanges();
                log.Error(ex.Message);
                infoResponse = new InfoResponse()
                {
                    Result = false,
                    Description = GetGlobalResource(Comun.strMensajeGenerico)
                };
            }

            return infoResponse;
        }

        public string editarRutaDocumento(DocumentTipos dato, String nombreAntiguo)
        {
            string path = "";
            string nuevaRuta = "";

            try
            {

                if (dato != null)
                {
                    path = TreeCore.DirectoryMapping.GetDocumentDirectory();
                    path = Path.Combine(path, nombreAntiguo);

                    nuevaRuta = TreeCore.DirectoryMapping.GetDocumentDirectory();
                    nuevaRuta = Path.Combine(nuevaRuta, dato.DocumentTipo);


                    if (Directory.Exists(path))
                    {
                        Directory.Move(path, nuevaRuta);

                    }
                    else
                    {
                        CrearRutaDocumento(dato);
                    }
                }

                return path;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public string CrearRutaDocumento(DocumentTipos dato)
        {
            string path = "";

            try
            {
                if (dato != null)
                {
                    path = TreeCore.DirectoryMapping.GetDocumentDirectory();
                    path = Path.Combine(path, dato.DocumentTipo);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }

                return path;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

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

        public List<Vw_AccessEmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAccess(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AccessEmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_AccessEmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.AccessEmplazamientosEstadosTiposDocumentos where c.AccessEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AccessEmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.AccessEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AccessEmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_AdquisicionesEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAdquisiciones(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AdquisicionesEmplazamientosEstadosDocumentos> listaResult = new List<Vw_AdquisicionesEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.AdquisicionesEmplazamientosEstadosTiposDocumentos where c.AdquisicionEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AdquisicionesEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.AdquisicionEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AdquisicionesEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_AdquisicionesSARFEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAdquisicionesSARF(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AdquisicionesSARFEmplazamientosEstadosDocumentos> listaResult = new List<Vw_AdquisicionesSARFEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.AdquisicionesSARFEmplazamientosEstadosTiposDocumentos where c.AdquisicionSARFEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AdquisicionesSARFEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.AdquisicionSARFEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AdquisicionesSARFEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_AmbientalEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAmbiental(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AmbientalEstadosDocumentos> listaResult = new List<Vw_AmbientalEstadosDocumentos>();

            listaTipos = (from c in Context.AmbientalEstadosTiposDocumentos where c.AmbientalEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AmbientalEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.AmbientalEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AmbientalEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_AmpliacionesEmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAmpliaciones(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AmpliacionesEmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_AmpliacionesEmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.AmpliacionesEmplazamientosEstadosTiposDocumentos where c.AmpliacionEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AmpliacionesEmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.AmpliacionEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AmpliacionesEmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_AssetsPurchaseEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAssetsPurchase(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AssetsPurchaseEmplazamientosEstadosDocumentos> listaResult = new List<Vw_AssetsPurchaseEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.AssetsPurchaseEmplazamientosEstadosTiposDocumentos where c.AssetPurchaseEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AssetsPurchaseEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.AssetPurchaseEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AssetsPurchaseEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_AuditEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAudit(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AuditEmplazamientosEstadosDocumentos> listaResult = new List<Vw_AuditEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.AuditEmplazamientosEstadosTiposDocumentos where c.AuditEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AuditEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.AuditEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AuditEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_CityEmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosCity(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_CityEmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_CityEmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.CityEmplazamientosEstadosTiposDocumentos where c.CityEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_CityEmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.CityEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_CityEmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_DesplieguesEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosDespliegues(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_DesplieguesEmplazamientosEstadosDocumentos> listaResult = new List<Vw_DesplieguesEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.DesplieguesEmplazamientosEstadosTiposDocumentos where c.DespliegueEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_DesplieguesEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.DespliegueEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_DesplieguesEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_EnergyEmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosEnergy(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_EnergyEmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_EnergyEmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.EnergyEmplazamientosEstadosTiposDocumentos where c.EnergyEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_EnergyEmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.EnergyEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_EnergyEmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_FinancieroAlquileresEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosFinanciero(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_FinancieroAlquileresEstadosDocumentos> listaResult = new List<Vw_FinancieroAlquileresEstadosDocumentos>();

            listaTipos = (from c in Context.FinancieroAlquileresEstadosTiposDocumentos where c.FinancieroAlquilerEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_FinancieroAlquileresEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.FinancieroAlquilerEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_FinancieroAlquileresEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_FirmaDigitalEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosFirmaDigital(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_FirmaDigitalEstadosDocumentos> listaResult = new List<Vw_FirmaDigitalEstadosDocumentos>();

            listaTipos = (from c in Context.FirmaDigitalEstadosTiposDocumentos where c.FirmaDigitalEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_FirmaDigitalEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.FirmaDigitalEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_FirmaDigitalEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_IndoorEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosIndoor(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_IndoorEmplazamientosEstadosDocumentos> listaResult = new List<Vw_IndoorEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.IndoorEmplazamientosEstadosTiposDocumentos where c.IndoorEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_IndoorEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.IndoorEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_IndoorEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_InstallObraCivilEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosInstallObraCivil(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_InstallObraCivilEmplazamientosEstadosDocumentos> listaResult = new List<Vw_InstallObraCivilEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.InstallObraCivilEmplazamientosEstadosTiposDocumentos where c.InstallObraCivilEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_InstallObraCivilEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.InstallObraCivilEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_InstallObraCivilEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_InstallTecnicaEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosInstallTecnica(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_InstallTecnicaEmplazamientosEstadosDocumentos> listaResult = new List<Vw_InstallTecnicaEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.InstallTecnicaEmplazamientosEstadosTiposDocumentos where c.InstallTecnicaEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_InstallTecnicaEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.InstallTecnicaEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_InstallTecnicaEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_LegalEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosLegal(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_LegalEmplazamientosEstadosDocumentos> listaResult = new List<Vw_LegalEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.LegalEmplazamientosEstadosTiposDocumentos where c.LegalEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_LegalEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.LegalEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_LegalEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_MantenimientoEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosMantenimiento(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_MantenimientoEmplazamientosEstadosDocumentos> listaResult = new List<Vw_MantenimientoEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.MantenimientoEmplazamientosEstadosTiposDocumentos where c.MantenimientoEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_MantenimientoEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.MantenimientoEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_MantenimientoEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_PlanningPlanificacionesEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosPlanning(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_PlanningPlanificacionesEstadosTiposDocumentos> listaResult = new List<Vw_PlanningPlanificacionesEstadosTiposDocumentos>();

            listaTipos = (from c in Context.PlanningPlanificacionesEstadosTiposDocumentos where c.PlanningPlanificacionEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_PlanningPlanificacionesEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.PlanningPlanificacionEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_PlanningPlanificacionesEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_SavingEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosSaving(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_SavingEmplazamientosEstadosDocumentos> listaResult = new List<Vw_SavingEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.SavingEmplazamientosEstadosTiposDocumentos where c.SavingEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_SavingEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.SavingEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_SavingEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_SharingEmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosSharing(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_SharingEmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_SharingEmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.SharingEmplazamientosEstadosTipoDocumentos where c.SharingEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_SharingEmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.SharingEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_SharingEmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_SpaceEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosSpace(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_SpaceEmplazamientosEstadosDocumentos> listaResult = new List<Vw_SpaceEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.SpaceEmplazamientosEstadosTiposDocumentos where c.SpaceEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_SpaceEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.SpaceEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_SpaceEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_SSRREmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosSSRR(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_SSRREmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_SSRREmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.SSRREmplazamientosEstadosTiposDocumentos where c.SSRREmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_SSRREmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.SSRREmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_SSRREmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_TorrerosSARFEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosTorrerosSARF(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_TorrerosSARFEmplazamientosEstadosDocumentos> listaResult = new List<Vw_TorrerosSARFEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.TorrerosSARFEmplazamientosEstadosTiposDocumentos where c.TorreroSARFEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_TorrerosSARFEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.TorreroSARFEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_TorrerosSARFEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_TorrerosEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosTorreros(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_TorrerosEmplazamientosEstadosDocumentos> listaResult = new List<Vw_TorrerosEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.TorrerosEmplazamientosEstadosTiposDocumentos where c.TorreroEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_TorrerosEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.TorreroEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_TorrerosEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_UninstallAdminEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosUninstallAdmin(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_UninstallAdminEmplazamientosEstadosDocumentos> listaResult = new List<Vw_UninstallAdminEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.UninstallAdminEmplazamientosEstadosTiposDocumentos where c.UninstallAdminEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_UninstallAdminEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.UninstallAdminEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_UninstallAdminEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_UninstallTecnicaEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosUninstallTecnica(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_UninstallTecnicaEmplazamientosEstadosDocumentos> listaResult = new List<Vw_UninstallTecnicaEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.UninstallTecnicaEmplazamientosEstadosTiposDocumentos where c.UninstallTecnicaEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_UninstallTecnicaEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.UninstallTecnicaEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_UninstallTecnicaEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_UninstallElectricaEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosUninstallElectrica(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_UninstallElectricaEmplazamientosEstadosDocumentos> listaResult = new List<Vw_UninstallElectricaEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.UninstallElectricaEmplazamientosEstadosTiposDocumentos where c.UninstallElectricaEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_UninstallElectricaEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.UninstallElectricaEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_UninstallElectricaEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_VandalismoEmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosVandalismo(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_VandalismoEmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_VandalismoEmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.VandalismoEmplazamientosEstadosTiposDocumentos where c.VandalismoEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_VandalismoEmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.VandalismoEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_VandalismoEmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_ProyectosTiposDocumentosTipos> GetDocumentosTiposNoAsignadosEstados(long lEstadoID, long lProyectoTipoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_ProyectosTiposDocumentosTipos> lista = null;

            listaTipos = (from c in Context.AdquisicionesSARFEstadosTiposDocumentos where c.AdquisicionSARFEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            lista = (from c in Context.Vw_ProyectosTiposDocumentosTipos where !(listaTiposNuevo.Contains(c.DocumentTipoID)) && c.ProyectoTipoID == lProyectoTipoID select c).OrderBy("DocumentTipo").ToList<Vw_ProyectosTiposDocumentosTipos>();

            return lista;
        }

        public long GetDocTipo(string sDocTipo)
        {
            long lTipos = new long();

            lTipos = (from c in Context.DocumentTipos where c.DocumentTipo == sDocTipo select c.DocumentTipoID).First();

            return lTipos;
        }
        public DocumentTipos GetDocumetoTipo(string sDocTipo)
        {
            DocumentTipos oTipos;
            try
            {
                oTipos = (from c in Context.DocumentTipos where c.DocumentTipo == sDocTipo select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oTipos = null;
            }
            return oTipos;
        }

        public DocumentosTiposRoles CompruebaPermisoDocumentoTipo(long DocumentoTipoID, long UsuarioID)
        {
            DocumentosTiposRoles resultado;

            try
            {
                List<DocumentosTiposRoles> listaPermisos = (from p in Context.Roles
                                                          join rolU in Context.UsuariosRoles on p.RolID equals rolU.RolID
                                                          join docRol in Context.DocumentosTiposRoles on p.RolID equals docRol.RolID
                                                          where
                                                             UsuarioID == rolU.UsuarioID &&
                                                             docRol.Activo &&
                                                             docRol.TipoDocumentoID == DocumentoTipoID
                                                          select docRol).ToList();



                if (listaPermisos == null || listaPermisos.Count == 0)
                {
                    //Comprobamos permisos por defecto si el usuario no tiene Rol asignado
                    Usuarios usuario = (from user in Context.Usuarios where user.UsuarioID == UsuarioID select user).First();

                    IQueryable<DocumentosTiposRoles> query = (from docRol in Context.DocumentosTiposRoles
                                                              join docTipo in Context.DocumentTipos on docRol.TipoDocumentoID equals docTipo.DocumentTipoID
                                                            where
                                                                docRol.RolID == null &&
                                                                docRol.Activo &&
                                                                docTipo.Activo &&
                                                                docTipo.ClienteID == usuario.ClienteID
                                                            orderby docTipo.DocumentTipo
                                                            select docRol);
                    listaPermisos = query.ToList();
                }


                if (listaPermisos == null || listaPermisos.Count == 0)
                {
                    resultado = null;
                }
                else if (listaPermisos.Count == 1)
                {
                    resultado = listaPermisos[0];
                }
                else
                {
                    DocumentosTiposRoles permisoMultiRol = new DocumentosTiposRoles();
                    permisoMultiRol.PermisoLectura = false;
                    permisoMultiRol.PermisoEscritura = false;
                    permisoMultiRol.PermisoDescarga = false;

                    foreach (DocumentosTiposRoles permiso in listaPermisos)
                    {
                        if (permiso.PermisoLectura)
                        {
                            permisoMultiRol.PermisoLectura = true;
                        }
                        if (permiso.PermisoEscritura)
                        {
                            permisoMultiRol.PermisoEscritura = true;
                        }
                        if (permiso.PermisoDescarga)
                        {
                            permisoMultiRol.PermisoDescarga = true;
                        }
                    }

                    resultado = permisoMultiRol;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                resultado = null;
            }

            return resultado;
        }

        public List<DocumentTipos> GetDocumentosTiposUsuario(long UsuarioID, long ProyectoTipoID)
        {
            List<DocumentTipos> documentosTipos = new List<DocumentTipos>();

            try
            {
                List<DocumentTipos> lDocumentosTipossAux;

                Usuarios usuario = (from user in Context.Usuarios where user.UsuarioID == UsuarioID select user).First();

                #region Tipos por Rol
                lDocumentosTipossAux = (
                    from uRol in Context.UsuariosRoles
                    join docRoles in Context.DocumentosTiposRoles on uRol.RolID equals docRoles.RolID
                    join docTipo in Context.DocumentTipos on docRoles.TipoDocumentoID equals docTipo.DocumentTipoID
                    join user in Context.Usuarios on uRol.UsuarioID equals user.UsuarioID
                    where
                        uRol.UsuarioID == UsuarioID &&
                        docRoles.Activo &&
                        docTipo.Activo &&
                        docTipo.ClienteID == user.ClienteID

                    orderby docTipo.DocumentTipo
                    select docTipo).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                #region Rol Defecto
                lDocumentosTipossAux = (
                    from docRoles in Context.DocumentosTiposRoles
                    join docTipo in Context.DocumentTipos on docRoles.TipoDocumentoID equals docTipo.DocumentTipoID
                    where
                        docRoles.RolID == null &&
                        docRoles.Activo &&
                        docTipo.Activo &&
                        docTipo.ClienteID == usuario.ClienteID

                    orderby docTipo.DocumentTipo
                    select docTipo).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                #region Carpetas
                lDocumentosTipossAux = (
                    from docTipo in Context.DocumentTipos
                    join c in Context.Clientes on docTipo.ClienteID equals c.ClienteID
                    join u in Context.Usuarios on c.ClienteID equals u.ClienteID
                    where
                        docTipo.EsCarpeta &&
                        docTipo.Activo &&
                        docTipo.ClienteID == u.ClienteID &&
                        u.UsuarioID == UsuarioID
                    orderby docTipo.DocumentTipo
                    select docTipo).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                #region Tipos por proyecto
                lDocumentosTipossAux = (
                    from c in Context.ProyectosTiposDocumentosTipos
                    join d in Context.DocumentTipos on c.DocumentTipoID equals d.DocumentTipoID
                    where c.ProyectoTipoID == ProyectoTipoID

                    select d).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                #region Ordenación
                documentosTipos = documentosTipos.OrderBy(x => x.DocumentTipo).ToList();
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                documentosTipos = null;
            }

            return documentosTipos;
        }

        public List<DocumentTipos> GetDocumentosTiposUsuario(long UsuarioID, string documentoTipo)
        {
            List<DocumentTipos> documentosTipos = new List<DocumentTipos>();

            try
            {
                List<DocumentTipos> lDocumentosTipossAux;

                Usuarios usuario = (from user in Context.Usuarios where user.UsuarioID == UsuarioID select user).First();

                #region Tipos por Roles
                lDocumentosTipossAux = (
                    from uRoles in Context.UsuariosRoles
                    join docRoles in Context.DocumentosTiposRoles on uRoles.RolID equals docRoles.RolID
                    join docTipo in Context.DocumentTipos on docRoles.TipoDocumentoID equals docTipo.DocumentTipoID
                    join user in Context.Usuarios on uRoles.UsuarioID equals user.UsuarioID
                    where
                        uRoles.UsuarioID == UsuarioID &&
                        docRoles.Activo &&
                        docTipo.Activo &&
                        docTipo.DocumentTipo == documentoTipo &&
                        docTipo.ClienteID == user.ClienteID

                    orderby docTipo.DocumentTipo
                    select docTipo).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                #region Rol Defecto
                lDocumentosTipossAux = (
                    from docRoles in Context.DocumentosTiposRoles
                    join docTipo in Context.DocumentTipos on docRoles.TipoDocumentoID equals docTipo.DocumentTipoID
                    where
                        docRoles.RolID == null &&
                        docRoles.Activo &&
                        docTipo.Activo &&
                        docTipo.DocumentTipo == documentoTipo &&
                        docTipo.ClienteID == usuario.ClienteID

                    orderby docTipo.DocumentTipo
                    select docTipo).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                #region Carpetas
                lDocumentosTipossAux = (
                    from docTipo in Context.DocumentTipos
                    join c in Context.Clientes on docTipo.ClienteID equals c.ClienteID
                    join u in Context.Usuarios on c.ClienteID equals u.ClienteID
                    where
                        docTipo.EsCarpeta &&
                        docTipo.Activo &&
                        docTipo.ClienteID == u.ClienteID &&
                        u.UsuarioID == UsuarioID
                    orderby docTipo.DocumentTipo
                    select docTipo).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                #region Ordenación
                documentosTipos = documentosTipos.OrderBy(x => x.DocumentTipo).ToList();
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                documentosTipos = null;
            }

            return documentosTipos;
        }

        public List<DocumentTipos> GetDocumentosTiposUsuario(long UsuarioID)
        {
            List<DocumentTipos> documentosTipos = new List<DocumentTipos>();

            try
            {
                List<DocumentTipos> lDocumentosTipossAux;

                Usuarios usuario = (from user in Context.Usuarios where user.UsuarioID == UsuarioID select user).First();

                #region Tipos por Roles
                lDocumentosTipossAux = (
                    from uRoles in Context.UsuariosRoles
                    join docRoles in Context.DocumentosTiposRoles on uRoles.RolID equals docRoles.RolID
                    join docTipo in Context.DocumentTipos on docRoles.TipoDocumentoID equals docTipo.DocumentTipoID
                    join user in Context.Usuarios on uRoles.UsuarioID equals user.UsuarioID
                    where
                        uRoles.UsuarioID == UsuarioID &&
                        docRoles.Activo &&
                        docTipo.ClienteID == user.ClienteID

                    orderby docTipo.DocumentTipo
                    select docTipo).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                #region Rol Defecto
                lDocumentosTipossAux = (
                    from docRoles in Context.DocumentosTiposRoles
                    join docTipo in Context.DocumentTipos on docRoles.TipoDocumentoID equals docTipo.DocumentTipoID
                    where
                        docRoles.RolID == null &&
                        docRoles.Activo &&
                        docTipo.ClienteID == usuario.ClienteID

                    orderby docTipo.DocumentTipo
                    select docTipo).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                #region Carpetas
                lDocumentosTipossAux = (
                    from docTipo in Context.DocumentTipos
                    join c in Context.Clientes on docTipo.ClienteID equals c.ClienteID
                    join u in Context.Usuarios on c.ClienteID equals u.ClienteID
                    where
                        docTipo.EsCarpeta &&
                        docTipo.ClienteID == u.ClienteID &&
                        u.UsuarioID == UsuarioID
                    orderby docTipo.DocumentTipo
                    select docTipo).ToList();

                if (lDocumentosTipossAux != null && lDocumentosTipossAux.Count > 0)
                {
                    documentosTipos.AddRange(lDocumentosTipossAux);
                }
                #endregion

                #region Ordenación
                documentosTipos = documentosTipos.OrderBy(x => x.DocumentTipo).ToList();
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                documentosTipos = null;
            }

            return documentosTipos;
        }


        public List<DocumentTipos> GetActivos(long ClienteID)
        {
            return (from c in Context.DocumentTipos
                    where
                        c.Activo &&
                        !c.EsCarpeta &&
                        c.ClienteID == ClienteID
                    select c).ToList();
        }

        public List<DocumentTipos> GetSortedActivos()
        {
            return (from c in Context.DocumentTipos where c.Activo == true orderby c.DocumentTipo select c).ToList();
        }
        public List<DocumentTipos> GetSortedTodos()
        {
            return (from c in Context.DocumentTipos orderby c.DocumentTipo select c).ToList();
        }

        public List<Vw_DocumentTipos> GetDocumentosTiposByProyectoTipoID(long ProyectoTipoID)
        {
            return (from c in Context.Vw_DocumentTipos where c.Activo == true && c.EsCarpeta == false && c.ProyectoTipoID == ProyectoTipoID select c).ToList();
        }

        public List<long> GetDocTipoIDByProyTipoID(long proyTipoID)
        {
            return (from c in Context.ProyectosTiposDocumentosTipos where c.ProyectoTipoID == proyTipoID select c.DocumentTipoID).ToList();
        }

        public List<Vw_DocumentosPerfiles> GetDocPerfilesByTipoDocumentoID(long tipoDocumentoID)
        {
            return (from c in Context.Vw_DocumentosPerfiles where c.TipoDocumentoID == tipoDocumentoID select c).ToList();
        }

        public List<long> GetProyectosTiposByClienteID(long clienteID)
        {
            return (from c in Context.ClientesProyectosTipos where c.ClienteID == clienteID select c.ProyectoTipoID).ToList();
        }

        public List<DocumentosExtensiones> GetDocExtensionesByClienteID(long clienteID)
        {
            return (from c in Context.DocumentosExtensiones where c.ClienteID == clienteID select c).ToList();
        }

        public List<long> GetDocExtIDByTipoDocID(long tipoDocID)
        {
            return (from c in Context.DocumentosTiposExtensiones where c.DocumentTipoID == tipoDocID select c.DocumentoExtensionID).ToList();
        }

        public DocumentosExtensiones GetDocExtensionesByClienteIDYExtension(long clienteID, string extension)
        {
            return (from c in Context.DocumentosExtensiones where c.ClienteID == clienteID && c.Extension.Equals(extension) select c).FirstOrDefault();
        }

        public List<long> GetDocExtByDocTipoID(long docTipoID)
        {
            return (from c in Context.DocumentosTiposExtensiones where c.DocumentTipoID == docTipoID select c.DocumentoExtensionID).ToList();
        }
        public List<DocumentosExtensiones> GetExtByDocTipoID(long docTipoID)
        {
            return (from c in Context.DocumentosTiposExtensiones join ext in Context.DocumentosExtensiones on c.DocumentoExtensionID equals ext.DocumentoExtensionID where c.DocumentTipoID == docTipoID select ext).ToList();
        }

        public List<DocumentosTiposExtensiones> GetDoctiposExtByDocTipoID(long docTipoID)
        {
            return (from c in Context.DocumentosTiposExtensiones 
                    where c.DocumentTipoID == docTipoID 
                    select c).ToList();
        }

        public long GetProyectosTipoIDByDocTipoIDAndProyID(long docTipoID, long ProyID)
        {
            return (from c in Context.ProyectosTiposDocumentosTipos where c.DocumentTipoID == docTipoID && c.ProyectoTipoID == ProyID select c.ProyectoTipoDocumentoTipoID).FirstOrDefault();
        }

        public List<ProyectosTiposDocumentosTipos> GetProyectosTipoByDocTipoID(long docTipoID)
        {
            return (from c in Context.ProyectosTiposDocumentosTipos where c.DocumentTipoID == docTipoID select c).ToList();
        }

        public List<long> GetProyectosTipoIDByDocTipoID(long docTipoID)
        {
            return (from c in Context.ProyectosTiposDocumentosTipos where c.DocumentTipoID == docTipoID select c.ProyectoTipoID).ToList();
        }

        public DocumentosTiposExtensiones GetDocExtIDByDocTipoIDAndProyID(long docTipoID, long docExtID)
        {
            DocumentosTiposExtensiones docTipExt;
            try
            {
                docTipExt = (from c in Context.DocumentosTiposExtensiones
                             where c.DocumentTipoID == docTipoID && c.DocumentoExtensionID == docExtID
                             select c).FirstOrDefault();
            }
            catch(Exception ex)
            {
                docTipExt = null;
                log.Error(ex.Message);
            }
            return docTipExt;
        }

        public List<long> GetDocPerfilesIDByDocTipoID(long docTipoID)
        {
            return (from c in Context.DocumentosPerfiles where c.TipoDocumentoID == docTipoID select c.DocumentoPerfilID).ToList();
        }

        public DocumentosPerfiles GetDocPerfilesDefectoIDByDocTipoID(long docTipoID)
        {
            return (from c in Context.DocumentosPerfiles where c.TipoDocumentoID == docTipoID && c.PerfilID == null select c).First();
        }

        public long GetDocPerfilIDByDocTipoIDAndProyID(long docTipoID, long perfilID)
        {
            return (from c in Context.DocumentosPerfiles where c.TipoDocumentoID == docTipoID && c.PerfilID == perfilID select c.DocumentoPerfilID).FirstOrDefault();
        }

        public List<Perfiles> GetPerfilesByProyTipoIDandClienteID(long clienteID)
        {
            return (from c in Context.Perfiles where c.ClienteID == clienteID select c).ToList();
        }

        public List<long?> GetPerfilesByTipoDocID(long tipoDocID)
        {
            return (from c in Context.DocumentosPerfiles where c.PerfilID != null && c.TipoDocumentoID == tipoDocID select c.PerfilID).ToList();
        }

        public DocumentTipos CheckExisteDocTipoMismoNombre(string nombre, long DocumentTipoID)
        {
            return (from c in Context.DocumentTipos 
                    where c.DocumentTipo == nombre && c.EsCarpeta && c.DocumentTipoID != DocumentTipoID
                    select c).FirstOrDefault();
        }

        public bool CheckExisteTipoDocumentos(DocumentTipos docTipo)
        {
            bool control = true;
            DocumentTipos dato = (from c in Context.DocumentTipos 
                                  where 
                                    c.DocumentTipo == docTipo.DocumentTipo && 
                                    c.ClienteID == docTipo.ClienteID && 
                                    c.EsCarpeta == false && 
                                    c.DocumentTipoID != docTipo.DocumentTipoID 
                                  select c).FirstOrDefault();
            if (dato == null)
            {
                control = false;
            }
            return control;
        }

        public List<DocumentTipos> GetBySuperDocumentTipo(long SuperDocumentTipoID)
        {
            List<DocumentTipos> lista;
            try
            {
                lista = (from c in Context.DocumentTipos
                         where
                            c.SuperDocumentTipoID == SuperDocumentTipoID
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<DocumentTipos>();
            }
            return lista;
        }

        public List<DocumentTipos> GetDocumentosTiposBySuperDocumentTipoPermisoEscritura(long? SuperDocumentTipoID, long usuarioID)
        {
            List<DocumentTipos> lista;
            try
            {
                IQueryable<DocumentTipos> query = (from docTyp in Context.DocumentTipos
                         join docRol in Context.DocumentosTiposRoles on docTyp.DocumentTipoID equals docRol.TipoDocumentoID into dp
                         from d in dp.DefaultIfEmpty()
                         join usRol in Context.UsuariosRoles on d.RolID equals usRol.RolID into up
                         from p in up.DefaultIfEmpty()
                         
                        where
                            (p.UsuarioID == usuarioID || d.RolID == default) &&
                            (d.Activo || d == default) &&
                            !docTyp.EsCarpeta &&
                            docTyp.Activo &&
                            d.PermisoEscritura
                         select docTyp);

                if (SuperDocumentTipoID.HasValue)
                {
                    query = query.Where(docTyp => docTyp.SuperDocumentTipoID == SuperDocumentTipoID);
                }
                else
                {
                    query = query.Where(docTyp => docTyp.SuperDocumentTipoID == null);
                }

                lista = query.ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<DocumentTipos>();
            }
            return lista;
        }

        public List<DocumentTipos> GetCarpetasByPadre(long clienteID, long? SuperDocumentTipoID, bool inactivos)
        {
            List<DocumentTipos> lista;
            try
            {
                IQueryable<DocumentTipos> query = (from c in Context.DocumentTipos
                         where
                            c.EsCarpeta &&
                            c.ClienteID == clienteID
                         select c);

                if (SuperDocumentTipoID.HasValue)
                {
                    query = query.Where(c => c.SuperDocumentTipoID.Value == SuperDocumentTipoID.Value);
                }
                else
                {
                    query = query.Where(c => c.SuperDocumentTipoID == null);
                }

                lista = query.ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<DocumentTipos>();
            }
            return lista;
        }

        public List<DocumentTipos> GetTiposPermisoDescarga(List<long> idsTiposDoc, long usuarioID)
        {
            List<DocumentTipos> lista;

            try {
                List<long> hijos = getTiposHijos(idsTiposDoc);


                lista = (from doctyp in Context.DocumentTipos
                         join docRol in Context.DocumentosTiposRoles on doctyp.DocumentTipoID equals docRol.TipoDocumentoID into dp
                            from d in dp.DefaultIfEmpty()
                         join usRol in Context.UsuariosRoles on d.RolID equals usRol.RolID into up
                            from p in up.DefaultIfEmpty()
                         where 
                            (p.UsuarioID==usuarioID || d.RolID == default) && 
                            (d.Activo || d == default) &&
                            doctyp.Activo &&
                            hijos.Contains(doctyp.DocumentTipoID) &&
                            (d.PermisoDescarga || doctyp.EsCarpeta)
                         select doctyp).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<long> getTiposHijos(List<long> idsTiposDoc)
        {
            List<long> tiposHijos = new List<long>();
            tiposHijos.AddRange(idsTiposDoc);

            try
            {
                idsTiposDoc.ForEach(idType => {
                    try {
                        List<long> tmp = (from c in Context.DocumentTipos
                                          where c.SuperDocumentTipoID == idType
                                          select c.DocumentTipoID).ToList();

                        tiposHijos.AddRange(getTiposHijos(tmp));
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        tiposHijos.Add(idType);
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tiposHijos = new List<long>();
            }

            return tiposHijos;
        }

        public List<DocumentTipos> getDocumentosTiposLibres(long lEstadoID)
        {
            List<DocumentTipos> listaDatos;
            List<long> listaIDs;

            try
            {
                listaIDs = (from c in Context.CoreEstadosDocumentosTipos where c.CoreEstadoID == lEstadoID select c.DocumentoTipoID).ToList();
                listaDatos = (from EstDoc in Context.CoreEstadosDocumentosTipos
                              join Doc in Context.DocumentTipos on EstDoc.CoreEstadoID equals lEstadoID
                              where Doc.DocumentTipoID != EstDoc.DocumentoTipoID && !listaIDs.Contains(Doc.DocumentTipoID) && !Doc.EsCarpeta
                              select Doc).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<CoreEstadosDocumentosTipos> getDocumentosTipos(long lEstadoID)
        {
            List<CoreEstadosDocumentosTipos> listaDatos;

            try
            {
                listaDatos = (from EstDoc in Context.CoreEstadosDocumentosTipos
                              join Est in Context.Vw_CoreEstados on EstDoc.CoreEstadoID equals Est.CoreEstadoID
                              where EstDoc.CoreEstadoID == lEstadoID
                              select EstDoc).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public string getNombreByTipoID (long lTipoID)
        {
            string sNombre;

            try
            {
                sNombre = (from c in Context.DocumentTipos where c.DocumentTipoID == lTipoID select c.DocumentTipo).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sNombre = null;
            }

            return sNombre;
        }

        public List<DocumentTipos> getCarpetaPadre(long carpetaActual)
        {
            List<DocumentTipos> carpetas;

            try
            {
                carpetas = (from carpeta in Context.DocumentTipos
                            join carpetaPadre in Context.DocumentTipos on carpeta.DocumentTipoID equals carpetaPadre.SuperDocumentTipoID
                            where carpeta.DocumentTipoID == carpetaActual
                            select carpeta).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                carpetas = new List<DocumentTipos>();
            }

            return carpetas;
        }

        public InfoResponse eliminarExtensionDeTipoDoc(long docID, long extID)
        {
            InfoResponse infoResponse;

            DocumentosTiposExtensionesController cDocExt = new DocumentosTiposExtensionesController();
            cDocExt.SetDataContext(this.Context);

            try
            {
                infoResponse = cDocExt.Delete(GetDocExtIDByDocTipoIDAndProyID(docID, extID));
            }
            catch(Exception ex)
            {
                infoResponse = new InfoResponse()
                {
                    Result = false,
                    Data = null,
                    Description = ex.Message
                };
                log.Error(ex.Message);
            }
            return infoResponse;
        }
    }
}