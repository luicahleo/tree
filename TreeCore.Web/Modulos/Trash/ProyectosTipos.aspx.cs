using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using TreeCore.Data;

namespace TreeCore.ModGlobal
{
    public partial class ProyectosTipos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grid, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<Data.ProyectosTipos> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                            log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            EstadisticasController cEstadisticas = new EstadisticasController();
                            cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        Response.Write("ERROR: " + ex.Message);
                    }

                    Response.End();
                }

                ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);
            }
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #endregion

        #region STORES

        #region PRINCIPAL
        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro);

                    if (lista != null)
                    {
                        storePrincipal.DataSource = lista;

                        PageProxy temp = (PageProxy)storePrincipal.Proxy[0];
                        temp.Total = iCount;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.ProyectosTipos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.ProyectosTipos> listaDatos;
            ProyectosTiposController cProyectosTipos = new ProyectosTiposController();

            try
            {
                listaDatos = cProyectosTipos.GetItemsWithExtNetFilterList<Data.ProyectosTipos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            ProyectosTiposController cProyectosTipos = new ProyectosTiposController();

            #region SUBIR iMAGENES

            string sArchivos = "";
            string sExtension = "";
            string sNombreArchivo = "";

            string sFileNameOfbigIcon = Path.GetFileName(FileUploadField2.PostedFile.FileName);
            string sFileNameOfSmallIcon = Path.GetFileName(FileUploadField1.PostedFile.FileName);
            string sExtImagenGrande = Path.GetExtension(FileUploadField2.PostedFile.FileName).ToLower();
            string sExtImagenPequeña = Path.GetExtension(FileUploadField1.PostedFile.FileName).ToLower();
            string sImagenGrande = Path.GetFileNameWithoutExtension(FileUploadField2.PostedFile.FileName);
            string sImagenPequeña = Path.GetFileNameWithoutExtension(FileUploadField1.PostedFile.FileName);

            string sFileDirectory = "";
            sFileDirectory = TreeCore.DirectoryMapping.GetDocumentDirectory();

            if (!System.IO.Directory.Exists(sFileDirectory))
            {
                System.IO.Directory.CreateDirectory(sFileDirectory);
            }

            string sFullpath = Path.Combine(sFileDirectory, sImagenGrande);
            string sFullpath1 = Path.Combine(sFileDirectory, sImagenPequeña);
            string sFullpath2 = sFullpath1 + ";" + sFullpath;

            if (sFileNameOfbigIcon != "" && sExtImagenGrande != "" && sFileNameOfSmallIcon != "" && sExtImagenPequeña != "")
            {
                FileUploadField1.PostedFile.SaveAs(sFullpath);
                FileUploadField2.PostedFile.SaveAs(sFullpath1);
                sExtImagenGrande = sExtImagenGrande.Substring(1, sExtImagenGrande.Length - 1);
                sExtImagenPequeña = sExtImagenPequeña.Substring(1, sExtImagenPequeña.Length - 1);
                sArchivos = sFullpath2;
                sExtension = sExtImagenGrande + ";" + sExtImagenPequeña;
                sNombreArchivo = sFileNameOfbigIcon + "/" + sFileNameOfSmallIcon;
            }
            else if (sFileNameOfSmallIcon != "" && sExtImagenPequeña != "" && sExtImagenPequeña.StartsWith("."))
            {
                FileUploadField1.PostedFile.SaveAs(sFullpath1);
                sExtImagenPequeña = sExtImagenPequeña.Substring(1, sExtImagenPequeña.Length - 1);
                sArchivos = sFullpath1;
                sExtension = sExtImagenPequeña;
                sNombreArchivo = "/" + sFileNameOfSmallIcon;
            }
            else if (sFileNameOfbigIcon != "" && sExtImagenGrande != "" && sExtImagenGrande.StartsWith("."))
            {
                FileUploadField2.PostedFile.SaveAs(sFullpath);
                sExtImagenGrande = sExtImagenGrande.Substring(1, sExtImagenGrande.Length - 1);
                sArchivos = sFullpath;
                sExtension = sExtImagenGrande;
                sNombreArchivo = sFileNameOfbigIcon + "/";
            }

            #endregion

            try
            {
                if (!bAgregar)
                {
                    long lIDSelect = Convert.ToInt64(GridRowSelect.SelectedRecordID);
                    Data.ProyectosTipos oDato;
                    oDato = cProyectosTipos.GetItem(lIDSelect);

                    if (oDato.ProyectoTipo == txtProyectoTipo.Text)
                    {
                        oDato.ProyectoTipo = txtProyectoTipo.Text;
                        oDato.AliasKey = txtKey.Text;

                        if (sArchivos != "")
                        {
                            oDato.Archivo = sArchivos;
                        }

                        if (sExtension != "")
                        {
                            oDato.Extension = sExtension;
                        }
                        oDato.NombreArchivo = sNombreArchivo;
                        if (chkExisteZona.Checked)
                        {
                            oDato.ExisteZona = true;
                        }
                        else
                        {
                            oDato.ExisteZona = false;
                        }
                        if (oDato.Alias != "")
                        {
                            oDato.Alias = txtAlias.Text;
                        }

                        if (chkIsReporting.Checked)
                        {
                            oDato.IsReporting = true;
                        }
                        else
                        {
                            oDato.IsReporting = false;
                        }
                        if (chkSoloClientes.Checked)
                        {
                            oDato.SoloClientes = true;
                        }
                        else
                        {
                            oDato.SoloClientes = false;
                        }
                    }
                    else
                    {
                        if (cProyectosTipos.RegistroDuplicado2(txtProyectoTipo.Text))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.ProyectoTipo = txtProyectoTipo.Text;
                            oDato.AliasKey = txtKey.Text;
                            oDato = cProyectosTipos.GetItem(lIDSelect);

                            if (chkExisteZona.Checked)
                            {
                                oDato.ExisteZona = true;
                            }
                            else
                            {
                                oDato.ExisteZona = false;
                            }
                            if (oDato.Alias != "")
                            {
                                oDato.Alias = txtAlias.Text;
                            }

                            if (chkIsReporting.Checked)
                            {
                                oDato.IsReporting = true;
                            }
                            else
                            {
                                oDato.IsReporting = false;
                            }

                            if (sArchivos != "")
                            {
                                oDato.Archivo = sArchivos;
                            }
                            oDato.NombreArchivo = sNombreArchivo;

                            if (sExtension != "")
                            {
                                oDato.Extension = sExtension;
                            }
                            if (chkSoloClientes.Checked)
                            {
                                oDato.SoloClientes = true;
                            }
                            else
                            {
                                oDato.SoloClientes = false;
                            }
                        }
                    }
                    if (cProyectosTipos.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    if (cProyectosTipos.RegistroDuplicado2(txtProyectoTipo.Text))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.ProyectosTipos oDato = new Data.ProyectosTipos
                        {
                            ProyectoTipo = txtProyectoTipo.Text,
                            AliasKey = txtKey.Text,
                            Activo = true
                        };

                        if (sArchivos != "")
                        {
                            oDato.Archivo = sArchivos;
                        }

                        if (sExtension != "")
                        {
                            oDato.Extension = sExtension;
                        }

                        oDato.NombreArchivo = sNombreArchivo;

                        if (chkExisteZona.Checked)
                        {
                            oDato.ExisteZona = true;
                        }
                        else
                        {
                            oDato.ExisteZona = false;
                        }
                        if (oDato.Alias != "")
                        {
                            oDato.Alias = txtAlias.Text;
                        }
                        if (chkIsReporting.Checked)
                        {
                            oDato.IsReporting = true;
                        }
                        else
                        {
                            oDato.IsReporting = false;
                        }
                        if (chkSoloClientes.Checked)
                        {
                            oDato.SoloClientes = true;
                        }
                        else
                        {
                            oDato.SoloClientes = false;
                        }

                        if (cProyectosTipos.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosTiposController cProyectosTipos = new ProyectosTiposController();

            try
            {
                long lIDSelect = Convert.ToInt64(GridRowSelect.SelectedRecordID);

                Data.ProyectosTipos oDato;
                oDato = cProyectosTipos.GetItem(lIDSelect);

                txtProyectoTipo.Text = oDato.ProyectoTipo;
                txtAlias.Text = oDato.Alias;
                txtKey.Text = oDato.AliasKey;

                if (oDato.ExisteZona == true)
                {
                    chkExisteZona.Checked = true;
                }
                else
                {
                    chkExisteZona.Checked = false;
                }
                if (oDato.Alias != "")
                {
                    oDato.Alias = txtAlias.Text;
                }
                if (oDato.IsReporting)
                {
                    chkIsReporting.Checked = true;
                }
                else
                {
                    chkIsReporting.Checked = false;
                }
                if (oDato.SoloClientes)
                {
                    chkSoloClientes.Checked = true;
                }
                else
                {
                    chkSoloClientes.Checked = false;
                }

                winGestion.Show();
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosTiposController cProyectosTipos = new ProyectosTiposController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cProyectosTipos.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }

            }
            catch (Exception ex)
            {
                if (ex is SqlException Sql)
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

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            ProyectosTiposController cController = new ProyectosTiposController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ProyectosTipos oDato = cController.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cController.UpdateItem(oDato))
                {
                    storePrincipal.DataBind();
                    log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse ExtensionesValidas(long lTipoDocumento)
        {
            DirectResponse direct = new DirectResponse();
            direct.Result = "";

            DocumentosTiposExtensionesController cTiposExtensiones = new DocumentosTiposExtensionesController();
            List<Vw_DocumentosTiposExtensiones> listaTiposextensionespermitidas;
            listaTiposextensionespermitidas = cTiposExtensiones.ExtensionesPermitidasByTipoDocumento(lTipoDocumento);

            string sExtensionesvalidas = "";

            foreach (Vw_DocumentosTiposExtensiones extension in listaTiposextensionespermitidas)
            {
                sExtensionesvalidas += $"{extension.Extension},";
            }

            ExtensionesPermitidas.Value = sExtensionesvalidas;
            direct.Success = true;

            return direct;
        }

        #endregion

        #region FUNCTIONS

        #endregion
    }
}