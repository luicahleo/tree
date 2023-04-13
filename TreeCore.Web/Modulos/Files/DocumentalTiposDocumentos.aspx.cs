using System;
using System.Collections.Generic;
using CapaNegocio;
using Ext.Net;
using TreeCore.Data;
using log4net;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using TreeCore.Clases;
using System.Linq;

namespace TreeCore.ModDocumental.pages
{
    public partial class DocumentalTiposDocumentos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static List<long> storeExtensionesPorTipoDoc = new List<long>();
        public static List<long> StoreRolesPorTipoDoc = new List<long>();
        public static List<string> roles = new List<string>();
        private static string ROOT = "ROOT";
        private bool permitirDragAndDrop = false;

        #region Gestión Página (Init/Load)

        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                
                ResourceManagerOperaciones(ResourceManagerTreeCore);
                Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
                UsuariosController cUsuarios = new UsuariosController();
                Data.Usuarios oUser = cUsuarios.GetItem(oUsuario.UsuarioID);
                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.SetValue(ClienteID);
                }

                List<long> listaFuncionalidades = ((List<long>)this.Session["FUNCIONALIDADES"]);
                if (listaFuncionalidades != null)
                {
                    if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_RESTRINGIDO_A_DOCUMENTAL_TIPOS_DOCUMENTOS))
                    {
                        permitirDragAndDrop = false;
                    }
                    else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_A_DOCUMENTAL_TIPOS_DOCUMENTOS))
                    {
                        permitirDragAndDrop = true;
                    }
                }
                CargarMenu();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);

            List<string> listFun = ((List<string>)(this.Session["FUNTIONALITIES"]));
            var UserInterface = ModulesController.GetUserInterfaces().FirstOrDefault(x => x.Page.ToLower() == sPagina.ToLower());
            var listFunPag = listFun.Where(x => $"{x.Split('@')[0]}" == UserInterface.Code);

            if (listFunPag.Where(x => x.Split('@')[1] == "Put").ToList().Count > 0)
            {
                permitirDragAndDrop = true;
            }

            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> { btnDescargar }},
                { "Post", new List<ComponentBase> { btnAnadir, AddFolderMainPanel, AddDocuMainPanel, btnAnadirRoles, btnAnadirExtension }},
                { "Put", new List<ComponentBase> { btnEditarDocumento, btnActivar, lblToggle }},
                { "Delete", new List<ComponentBase> { btnEliminarDocumento, btnEliminarRoles, btnEliminarExtension }}
            };
        }

        #endregion

        #region CLASES

        public class gext
        {
            public long DocumentoExtensionID;
            public string Extension;
            public gext(long docExtID, string extension)
            {
                DocumentoExtensionID = docExtID;
                Extension = extension;
            }
        }

        #endregion

        #region DIRECT METHODS

        #region CARGA TREEPANEL

        [DirectMethod()]
        public DirectResponse CargarMenu()
        {
            DirectResponse result = new DirectResponse();
            result.Success = true;
            result.Result = "";

            try
            {

                NodeCollection nodes = new NodeCollection();
                Node nodoRaiz;
                nodoRaiz = new Node
                {
                    Text = GetGlobalResource("strRaiz"),
                    Expanded = true,
                    Qtip = GetGlobalResource("strRaiz")
                };
                nodoRaiz.CustomAttributes.Add(new ConfigItem("DocumentTipo", GetGlobalResource("strRaiz")));
                nodoRaiz.CustomAttributes.Add(new ConfigItem("EsCarpeta", true));
                nodoRaiz.AllowDrop = (permitirDragAndDrop);
                
                nodoRaiz.NodeID = ROOT;
                nodes.Add(nodoRaiz);
                nodoRaiz.Children.AddRange(ConstruirArbol(null));
                TreePanelV1.SetRootNode(nodoRaiz);
                hdCarpetaID.SetValue("");

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result.Success = false;
                result.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }

            return result;

        }

        private NodeCollection ConstruirArbol(long? lPadre)
        {
            DocumentTiposController cDocTipos = new DocumentTiposController();
            NodeCollection oNodes;
            try
            {
                if (btnActivo.Pressed)
                {
                    List<DocumentTipos> listaDocTipos = cDocTipos.GetSortedActivos();
                    oNodes = GetNodosHijos(lPadre, listaDocTipos, 0);
                }
                else
                {
                    List<DocumentTipos> listaDocTipos = cDocTipos.GetSortedTodos();
                    oNodes = GetNodosHijos(lPadre, listaDocTipos, 0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oNodes = null;
            }
            return oNodes;
        }

        private NodeCollection GetNodosHijos(long? lPadre, List<DocumentTipos> listaCat, long nivel)
        {
            NodeCollection oMenu = new NodeCollection(false);
            nivel++;
            try
            {
                listaCat.ForEach(oItem =>
                {
                    if (oItem.SuperDocumentTipoID == lPadre)
                    {
                        Node oNodo = new Node
                        {
                            NodeID = oItem.DocumentTipoID.ToString(),
                            Text = oItem.DocumentTipo,
                            Expanded = true,
                            Expandable = true
                        };
                        if (!oItem.Activo)
                        {
                            oNodo.Cls = "itemArbolDesactivo";
                        }
                        oNodo.CustomAttributes.Add(new ConfigItem("DocumentTipo", oItem.DocumentTipo));
                        oNodo.CustomAttributes.Add(new ConfigItem("EsCarpeta", oItem.EsCarpeta));
                        if (permitirDragAndDrop)
                        {
                            oNodo.AllowDrag = (!oItem.EsCarpeta);
                            oNodo.AllowDrop = (oItem.EsCarpeta);
                        }
                        else
                        {
                            oNodo.AllowDrag = false;
                            oNodo.AllowDrop = false;
                        }
                        NodeCollection nodoshijos = GetNodosHijos(oItem.DocumentTipoID, listaCat, nivel);
                        if (nodoshijos != null && nodoshijos.Count > 0)
                        {
                            oNodo.Children.AddRange(nodoshijos);
                        }
                        oNodo.Leaf = !oItem.EsCarpeta;
                        oMenu.Add(oNodo);
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oMenu = null;
            }
            return oMenu;
        }

        #endregion

        #region AÑADIR/EDITAR NUEVA CARPETA

        [DirectMethod()]
        public DirectResponse AñadirNuevaCarpeta()
        {
            DocumentTiposController cDocTipos = new DocumentTiposController();
            DirectResponse direct = new DirectResponse();
            long cliID = long.Parse(hdCliID.Value.ToString());
            try
            {
                DocumentTipos newDocumentoTipo = new DocumentTipos();
                newDocumentoTipo.DocumentTipo = txtNombreCarpeta.Text;
                if (hdCarpetaID.Value.ToString() != "")
                {
                    long carpetaID = long.Parse(hdCarpetaID.Value.ToString());
                    newDocumentoTipo.SuperDocumentTipoID = carpetaID;
                }
                else
                {
                    newDocumentoTipo.SuperDocumentTipoID = null;
                }
                newDocumentoTipo.ClienteID = cliID;
                newDocumentoTipo.Activo = true;
                newDocumentoTipo.TipoElectrico = false;
                newDocumentoTipo.TipoComercial = false;
                newDocumentoTipo.ExencionRetencion = false;
                newDocumentoTipo.EsCarpeta = true;
                InfoResponse infoResponse = cDocTipos.Add(newDocumentoTipo);
                if (infoResponse.Result)
                {
                    direct.Success = true;
                    direct.Result = "";
                    cDocTipos.SubmitChanges();
                }
                else
                {
                    direct.Success = false;
                    direct.Result = infoResponse.Description;
                    cDocTipos.DiscardChanges();
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            CargarMenu();
            return direct;
        }


        [DirectMethod()]
        public void EditarVentanaCarpeta()
        {
            DocumentTiposController cDocTipos = new DocumentTiposController();
            try
            {
                RowSelectionModel sm = TreePanelV1.GetSelectionModel() as RowSelectionModel;
                if (sm.SelectedRow != null && Convert.ToBoolean(hdAnadirCarpeta.Value) == false)
                {
                    if (cDocTipos.GetItem(long.Parse(sm.SelectedRow.RecordID)).EsCarpeta)
                    {
                        winNewFolder.Title = GetGlobalResource("strEditarCarpeta");
                        txtNombreCarpeta.Text = cDocTipos.GetItem(long.Parse(sm.SelectedRow.RecordID)).DocumentTipo;
                        btnAnadirCarpeta.Text = GetGlobalResource("jsEditar");
                    }
                }
                else if (Convert.ToBoolean(hdAnadirCarpeta.Value) == true)
                {
                    winNewFolder.Title = GetGlobalResource("strNuevaCarpeta");
                    txtNombreCarpeta.Text = String.Empty;
                    btnAnadirCarpeta.Text = GetGlobalResource("strAnadir");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        [DirectMethod()]
        public void EditarCarpeta()
        {
            DocumentTiposController cDocTipos = new DocumentTiposController();
            try
            {
                RowSelectionModel sm = TreePanelV1.GetSelectionModel() as RowSelectionModel;
                DocumentTipos docTipo = cDocTipos.GetItem(long.Parse(sm.SelectedRow.RecordID));
                docTipo.DocumentTipo = txtNombreCarpeta.Text;
                    
                InfoResponse infoResponse = cDocTipos.Update(docTipo);
                if (infoResponse.Result)
                {
                    cDocTipos.SubmitChanges();
                    log.Error(infoResponse.Description);
                }
                else
                {
                    cDocTipos.DiscardChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        #endregion

        #region CREAR/EDITAR NUEVOS TIPOS DOCUMENTOS

        [DirectMethod()]
        public DirectResponse AgregarEditarDocumento(bool agregar)
        {
            DirectResponse direct = new DirectResponse();
            DocumentTiposController cDocTipos = new DocumentTiposController();

            InfoResponse infoResponse;
            try
            {
                long cliID = long.Parse(hdCliID.Value.ToString());

                if (!agregar)
                {
                    long tipoDocumentoID = long.Parse(hdTipoDocSeleccionado.Value.ToString());
                    DocumentTipos docTipo = cDocTipos.GetItem(tipoDocumentoID);

                    string NombreDocumentoAntiguo = docTipo.DocumentTipo;
                    docTipo.DocumentTipo = txtNombreTipoDocumento.Text;

                    List<string> recordsIDs = new List<string>();
                    RowSelectionModel extensionesSeleccionadas = winGestionDocumentosExtensiones.GetSelectionModel() as RowSelectionModel;
                    foreach (SelectedRow row in extensionesSeleccionadas.SelectedRows) {
                        recordsIDs.Add(row.RecordID);
                    }

                    bool PermisoLectura = chkPermisoLectura.Checked;
                    bool PermisoEscritura = chkPermisoEscritura.Checked;
                    bool PermisoDescarga = chkPermisoDescarga.Checked;

                    infoResponse = cDocTipos.EditarDocumentTipos(docTipo, NombreDocumentoAntiguo, recordsIDs, PermisoLectura, PermisoEscritura, PermisoDescarga);
                    
                }
                else
                {
                    DocumentTipos docTipo = new DocumentTipos()
                    {
                        DocumentTipo = txtNombreTipoDocumento.Text,
                        Activo = true,
                        TipoElectrico = false,
                        TipoComercial = false,
                        ExencionRetencion = false,
                        EsCarpeta = false,
                        ClienteID = cliID,
                    };


                    if (hdCarpetaID.Value.ToString() != "")
                    {
                        long carpetaID = long.Parse(hdCarpetaID.Value.ToString());
                        docTipo.SuperDocumentTipoID = carpetaID;
                    }
                    else
                    {
                        docTipo.SuperDocumentTipoID = null;
                    }

                    List<string> recordsIDs = new List<string>();
                    RowSelectionModel extensionesSeleccionadas = winGestionDocumentosExtensiones.GetSelectionModel() as RowSelectionModel;
                    foreach (SelectedRow row in extensionesSeleccionadas.SelectedRows)
                    {
                        recordsIDs.Add(row.RecordID);
                    }

                    bool PermisoLectura = chkPermisoLectura.Checked;
                    bool PermisoEscritura = chkPermisoEscritura.Checked;
                    bool PermisoDescarga = chkPermisoDescarga.Checked;

                    infoResponse = cDocTipos.CrearDocumentTipos(docTipo, recordsIDs, PermisoLectura, PermisoEscritura, PermisoDescarga);
                    
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            if (infoResponse.Result)
            {
                direct.Success = true;
                direct.Result = "";
            }
            else
            {
                direct.Success = false;
                direct.Result = infoResponse.Description;
            }
            
            return direct;
        }


        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            DocumentTiposController cDocTipos = new DocumentTiposController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                DocumentTipos oDato;
                oDato = cDocTipos.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                InfoResponse infoResponse = cDocTipos.Update(oDato);
                if (infoResponse.Result)
                {
                    cDocTipos.SubmitChanges();
                    CargarMenu();
                    log.Info(GetGlobalResource(Comun.LogActivacionRealizada));

                    direct.Success = true;
                    direct.Result = "";
                }
                else
                {
                    cDocTipos.DiscardChanges();
                    direct.Success = false;
                    direct.Result = infoResponse.Description;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }


            return direct;
        }


        [DirectMethod()]
        public DirectResponse MostrarEditarDocumento()
        {
            DirectResponse direct = new DirectResponse();
            DocumentTiposController cDocTipos = new DocumentTiposController();
            DocumentosRolesController cDocTipRoles = new DocumentosRolesController();
            long tipoDocumentoID = long.Parse(hdTipoDocSeleccionado.Value.ToString());
            try
            {
                RowSelectionModel smExt = winGestionDocumentosExtensiones.GetSelectionModel() as RowSelectionModel;
                DocumentTipos documentoTipo = cDocTipos.GetItem(tipoDocumentoID);

                txtNombreTipoDocumento.Value = documentoTipo.DocumentTipo;

                cDocTipos.GetDocExtIDByTipoDocID(tipoDocumentoID).ForEach(ext =>
                {
                    smExt.SelectedRows.Add(new SelectedRow(storeExtensionesPorTipoDoc.IndexOf(ext)));
                });
                smExt.UpdateSelection();

                List<DocumentosTiposRoles> dato = cDocTipRoles.GetRolesByTipoDocumentoIDDefecto(tipoDocumentoID);
                if (dato.Count > 0)
                {
                    chkPermisoLectura.Value = dato[0].PermisoLectura;
                    chkPermisoEscritura.Value = dato[0].PermisoEscritura;
                    chkPermisoDescarga.Value = dato[0].PermisoDescarga;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";
            return direct;
        }

        [DirectMethod()]
        public DirectResponse BeforeDropNodo(string targetsString, string destinationID, string destinationJSON)
        {

            DirectResponse direct = new DirectResponse();
            DocumentTiposController cDocumentTipos = new DocumentTiposController();


            bool docNoMovidos = true;

            try
            {
                if (permitirDragAndDrop)
                {
                    long? idDestino;
                    if (destinationID != ROOT)
                    {
                        idDestino = long.Parse(destinationID);
                    }
                    else
                    {
                        idDestino = null;
                    }

                    JObject targetsJSON = JObject.Parse(targetsString);
                    JArray arrayRecords = (JArray)targetsJSON["records"];
                    foreach (JObject jobj in arrayRecords)
                    {
                        long id = 0;
                        if (jobj.TryGetValue("id", out JToken iDToken))
                        {
                            id = long.Parse(iDToken.ToString());
                        }

                        DocumentTipos target = cDocumentTipos.GetItem(id);
                        if (target != null)
                        {
                            target.SuperDocumentTipoID = idDestino;

                            InfoResponse infoResponse = cDocumentTipos.Update(target);
                            if (infoResponse.Result)
                            {

                            }
                            else
                            {
                                docNoMovidos = true;
                            }
                        }
                    }


                    direct.Success = true;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        #region ELIMINAR TIPOS DE DOCUMENTOS
        [DirectMethod()]
        public DirectResponse EliminarDocumento()
        {
            DirectResponse direct = new DirectResponse();
            DocumentTiposController cDocTipos = new DocumentTiposController();

            try
            {
                RowSelectionModel doc = TreePanelV1.GetSelectionModel() as RowSelectionModel;
                long tipoDocumentoID = long.Parse(doc.SelectedRecordID.ToString());
                DocumentTipos dato = cDocTipos.GetItem(tipoDocumentoID);

                InfoResponse infoResponse = cDocTipos.Delete(dato);
                if (infoResponse.Result)
                {
                    EliminarRutaDocumento(dato);
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                    return direct;
                }
                else
                {
                    direct.Success = false;
                    direct.Result = infoResponse.Description;
                    return direct;
                }
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException Sql)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                    log.Error(Sql.Message);
                    return direct;
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }
        #endregion

        #region AÑADIR Y ELIMINAR PROYECTOS, EXTENSIONES Y ROLES DE TIPO DOCUMENTO


        [DirectMethod()]
        public void añadirExtensionDeTipoDoc()
        {
            DocumentTiposController cDocTipos = new DocumentTiposController();
            DocumentosTiposExtensionesController cDocTiposExt = new DocumentosTiposExtensionesController();
            cDocTiposExt.SetDataContext(cDocTipos.Context);
            DocumentosExtensionesController cDocExt = new DocumentosExtensionesController();
            cDocExt.SetDataContext(cDocTipos.Context);

            InfoResponse inforesponse;
            bool saveChanges = true;
            string message = "";

            try
            {
                RowSelectionModel doc = TreePanelV1.GetSelectionModel() as RowSelectionModel;
                RowSelectionModel extensionesSeleccionadas = GridOnlyExtensiones.GetSelectionModel() as RowSelectionModel;
                foreach (SelectedRow row in extensionesSeleccionadas.SelectedRows)
                {
                    DocumentosTiposExtensiones newDocTiposExt = new DocumentosTiposExtensiones();
                    newDocTiposExt.DocumentTipoID = long.Parse(doc.SelectedRow.RecordID);
                    newDocTiposExt.DocumentoExtensionID = cDocTipos.GetDocExtensionesByClienteIDYExtension(Usuario.ClienteID.Value, cDocExt.GetItem(long.Parse(row.RecordID)).Extension).DocumentoExtensionID;
                    inforesponse = cDocTiposExt.Add(newDocTiposExt);
                    if (!inforesponse.Result)
                    {
                        saveChanges = false;
                        message = inforesponse.Description;
                    }
                }

                if (saveChanges)
                {
                    cDocTiposExt.SubmitChanges();
                }
                else
                {
                    cDocTiposExt.DiscardChanges();
                    log.Error(message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        [DirectMethod()]
        public void eliminarExtensionDeTipoDoc()
        {
            DocumentTiposController cDocTipos = new DocumentTiposController();
            try
            {
                RowSelectionModel doc = TreePanelV1.GetSelectionModel() as RowSelectionModel;
                RowSelectionModel ext = GridP3Top.GetSelectionModel() as RowSelectionModel;

                InfoResponse infoResponse = cDocTipos.eliminarExtensionDeTipoDoc(long.Parse(doc.SelectedRow.RecordID), long.Parse(ext.SelectedRow.RecordID));
                if (infoResponse.Result)
                {
                    cDocTipos.SubmitChanges();
                }
                else
                {
                    cDocTipos.DiscardChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        [DirectMethod()]
        public void añadirPerfilDeTipoDoc()
        {
            DocumentosRolesController cDocRoles = new DocumentosRolesController();
            try
            {
                bool saveChanges = true;
                string message = "";

                RowSelectionModel doc = TreePanelV1.GetSelectionModel() as RowSelectionModel;
                RowSelectionModel rolesSeleccionados = GridOnlyRoles.GetSelectionModel() as RowSelectionModel;
                foreach (SelectedRow row in rolesSeleccionados.SelectedRows)
                {
                    DocumentosTiposRoles newDocRoles = new DocumentosTiposRoles();
                    newDocRoles.TipoDocumentoID = long.Parse(doc.SelectedRow.RecordID);
                    newDocRoles.RolID = long.Parse(row.RecordID);
                    newDocRoles.Activo = true;
                    newDocRoles.PermisoLectura = chkPermisoLecturaRoles.Checked;
                    newDocRoles.PermisoEscritura = chkPermisoEscrituraRoles.Checked;
                    newDocRoles.PermisoDescarga = chkPermisoDescargaRoles.Checked;
                    InfoResponse inforesponse = cDocRoles.Add(newDocRoles);

                    if (!inforesponse.Result)
                    {
                        saveChanges = false;
                        message = inforesponse.Description;
                    }
                }

                if (saveChanges)
                {
                    cDocRoles.SubmitChanges();
                }
                else
                {
                    cDocRoles.DiscardChanges();
                    log.Error(message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        [DirectMethod()]
        public DirectResponse eliminarPerfilDeTipoDoc()
        {
            DirectResponse direct = new DirectResponse();
            DocumentosRolesController cDocRoles = new DocumentosRolesController();

            InfoResponse infoResponse;

            try
            {
                RowSelectionModel perfil = GridP2Bot.GetSelectionModel() as RowSelectionModel;

                infoResponse = cDocRoles.Delete(cDocRoles.GetItem(long.Parse(perfil.SelectedRow.RecordID)));
                if (infoResponse.Result)
                {
                    infoResponse = cDocRoles.SubmitChanges();
                }
                else
                {
                    infoResponse = cDocRoles.DiscardChanges();
                }

                if (infoResponse.Result)
                {
                    direct.Success = true;
                    direct.Result = "";
                }
                else
                {
                    direct.Success = false;
                    direct.Result = infoResponse.Description;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }


            return direct;
        }
        #endregion

        #endregion

        #region STORES
        protected void StoreRoles_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                DocumentTiposController cDocTipos = new DocumentTiposController();
                DocumentosRolesController cDocRoles = new DocumentosRolesController();
                try
                {
                    roles = new List<string>();
                    RowSelectionModel sm = TreePanelV1.GetSelectionModel() as RowSelectionModel;
                    long tipoDocumentoID = long.Parse(hdTipoDocSeleccionado.Value.ToString());
                    foreach (SelectedRow row in sm.SelectedRows)
                    {
                        if (row.RecordID != "root" && row.RecordID != "")
                        {
                            List<object> datosPerfiles = new List<object>();
                            List<Vw_DocumentosTiposRoles> listPerfiles = cDocRoles.GetRolesByTipoDocumentoID(long.Parse(row.RecordID));
                            listPerfiles.ForEach(perf =>
                            {
                                if (perf != null && !datosPerfiles.Contains(new { perf.DocumentoTipoRoleID, perf.Nombre, perf.PermisoLectura, perf.PermisoEscritura, perf.PermisoDescarga }))
                                {
                                    datosPerfiles.Add(new { perf.DocumentoTipoRoleID, perf.Nombre, perf.PermisoLectura, perf.PermisoEscritura, perf.PermisoDescarga });
                                    if (!roles.Contains(perf.Nombre)) { roles.Add(perf.Nombre); }
                                }
                            });

                            List<DocumentosTiposRoles> lDocRoles = cDocRoles.GetRolesByTipoDocumentoIDDefecto(tipoDocumentoID);
                            DocumentosTiposRoles docRoles;
                            if (lDocRoles.Count > 0)
                            {
                                docRoles = lDocRoles[0];
                                string Nombre = GetGlobalResource("strDefecto");
                                datosPerfiles.Add(new { docRoles.DocumentoTipoRoleID, Nombre, docRoles.PermisoLectura, docRoles.PermisoEscritura, docRoles.PermisoDescarga });
                            }
                            StoreRoles.DataSource = datosPerfiles;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        protected void StoreExtensiones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                DocumentTiposController cDocTipos = new DocumentTiposController();
                DocumentosExtensionesController cDocExt = new DocumentosExtensionesController();
                try
                {
                    RowSelectionModel sm = TreePanelV1.GetSelectionModel() as RowSelectionModel;
                    foreach (SelectedRow row in sm.SelectedRows)
                    {
                        if (row.RecordID != "root" && row.RecordID != "")
                        {
                            List<object> datosExtensiones = new List<object>();
                            cDocTipos.GetDocExtByDocTipoID(long.Parse(row.RecordID)).ForEach(ext =>
                            {
                                datosExtensiones.Add(new { DocumentoExtensionID = cDocExt.GetItem(ext).DocumentoExtensionID, Extension = cDocExt.GetItem(ext).Extension });
                            });
                            StoreExtensiones.DataSource = datosExtensiones;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        protected void StoreGridExtensiones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                DocumentTiposController cDocTipos = new DocumentTiposController();
                try
                {
                    winGestionDocumentosExtensiones.GetSelectionModel().DeselectAll();
                    storeExtensionesPorTipoDoc = new List<long>();
                    List<gext> sortedList = new List<gext>();
                    List<object> datosGridExtensiones = new List<object>();
                    //Obtener los documentosExtensiones por ClienteID
                    cDocTipos.GetDocExtensionesByClienteID(Usuario.ClienteID.Value).ForEach(ext =>
                    {
                        sortedList.Add(new gext(ext.DocumentoExtensionID, ext.Extension));
                    });
                    sortedList.Sort((x, y) => x.Extension.CompareTo(y.Extension));
                    sortedList.ForEach(ex => { storeExtensionesPorTipoDoc.Add(ex.DocumentoExtensionID); datosGridExtensiones.Add(new { Extension = ex.Extension }); });
                    StoreGridExtensiones.DataSource = datosGridExtensiones;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }


        protected void StoreOnlyExtensiones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                DocumentTiposController cDocTipos = new DocumentTiposController();
                DocumentosExtensionesController cDocExt = new DocumentosExtensionesController();
                try
                {
                    #region STORE EXTENSIONES
                    RowSelectionModel sm = TreePanelV1.GetSelectionModel() as RowSelectionModel;
                    List<object> datosExtensiones = new List<object>();
                    foreach (SelectedRow row in sm.SelectedRows)
                    {
                        if (row.RecordID != "root" && row.RecordID != "")
                        {
                            cDocTipos.GetDocExtByDocTipoID(long.Parse(row.RecordID)).ForEach(ext =>
                            {
                                datosExtensiones.Add(new { DocumentoExtensionID = cDocExt.GetItem(ext).DocumentoExtensionID, Extension = cDocExt.GetItem(ext).Extension });
                            });
                        }
                    }
                    #endregion

                    #region STORE ONLY EXTENSIONES
                    List<object> datosOnlyExtensiones = new List<object>();
                    cDocTipos.GetDocExtensionesByClienteID(Usuario.ClienteID.Value).ForEach(ext =>
                    {
                        if (!datosExtensiones.Contains(new { DocumentoExtensionID = ext.DocumentoExtensionID, Extension = ext.Extension }))
                        {
                            datosOnlyExtensiones.Add(new { DocumentoExtensionID = ext.DocumentoExtensionID, Extension = ext.Extension });
                        }
                    });
                    StoreOnlyExtensiones.DataSource = datosOnlyExtensiones;
                    #endregion
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        protected void StoreOnlyRoles_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {


                DocumentTiposController cDocTipos = new DocumentTiposController();
                DocumentosRolesController cDocRoles = new DocumentosRolesController();
                RolesController cRoles = new RolesController();
                try
                {
                    List<Data.Roles> listaRoles = new List<Data.Roles>();

                    listaRoles = cRoles.GetActivos(Usuario.ClienteID.Value);

                    StoreOnlyRoles.DataSource = listaRoles;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        protected void StoreRolesLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {


                DocumentTiposController cDocTipos = new DocumentTiposController();
                DocumentosRolesController cDocRoles = new DocumentosRolesController();
                RolesController cRoles = new RolesController();
                try
                {
                    List<Data.Roles> listaRoles = new List<Data.Roles>();

                    long lS = long.Parse(GridRowSelect.SelectedRecordID);

                    listaRoles = cRoles.GetActivosNoAsignados(Usuario.ClienteID.Value, lS);

                    if (listaRoles != null)
                    {
                        StoreRolesLibres.DataSource = listaRoles;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #endregion

        #region FUNCIONES

        public string EliminarRutaDocumento(DocumentTipos dato)
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
                        Directory.Delete(path);
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
        #endregion

    }
}