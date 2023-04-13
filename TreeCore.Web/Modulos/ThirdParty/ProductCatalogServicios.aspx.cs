using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Data.SqlClient;
using TreeCore.Componentes;
using TreeCore.Data;
using TreeCore.Page;
using TreeCore.APIClient;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.Query;
using Newtonsoft.Json;

namespace TreeCore.ModGlobal
{
    public partial class ProductCatalogServicios : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region GESTION DE PAGINA

        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));


                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>() { };

                Comun.CreateGridFilters(gridFilters, storeCoreProductCatalogServicios, gridServicios.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }

            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];
                string sEntidad = Request.QueryString["aux3"].ToString();

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<ProductDTO> listaDatos = null;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        int iCount = 0;
                        bool bActivo = Request.QueryString["aux"] == "true";

                        List<FilterDTO> filtros = new List<FilterDTO>();
                        if (sFiltro != "")
                        {
                            foreach (var oFiltro in JsonConvert.DeserializeObject<List<FilterExtNet>>(sFiltro))
                            {
                                filtros.Add(new FilterDTO
                                {
                                    Field = oFiltro.property,
                                    Value = oFiltro.value,
                                    Operator = oFiltro.@operator
                                });
                            }
                        }
                        List<string> orders = new List<string>();
                        if (sOrden != "")
                        {
                            orders.Add(sOrden);
                        }

                        QueryDTO queryDTO = new QueryDTO(filtros, orders, sDir);

                        BaseAPIClient<ProductDTO> aPIClient = new BaseAPIClient<ProductDTO>(TOKEN_API);

                        listaDatos = aPIClient.GetList(queryDTO).Result.Value;

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombreTask<ProductDTO>(gridServicios.ColumnModel, listaDatos, Response, "", GetGlobalResource("strProductCatalogServicios").ToString(), _Locale).Wait();
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
                //storePrincipal.Reload();
                ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);
            }
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = "ProductCatalogServiciosContenedor.aspx";
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { btnAnadir }},
            { "Put", new List<ComponentBase> { btnEditar}},
            { "Delete", new List<ComponentBase> { btnEliminar }}
        };
        }

        #endregion

        #region STORES

        #region PRINCIPAL

        protected void storeCoreProductCatalogServicios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<ProductDTO> ApiClient = new BaseAPIClient<ProductDTO>(TOKEN_API);
                    var lista = ApiClient.GetList().Result;
                    if (lista != null)
                    {
                        store.DataSource = lista.Value;
                        store.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region ENTIDADES

        protected void storeEntidades_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<CompanyDTO> ApiClient = new BaseAPIClient<CompanyDTO>(TOKEN_API);
                    var lista = ApiClient.GetList().Result;
                    if (lista != null)
                    {
                        store.DataSource = lista.Value;
                        store.DataBind();
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

        #endregion

        #region CORE SERVICIOS TIPOS

        protected void storeCoreProductCatalogServiciosTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<ProductTypeDTO> ApiClient = new BaseAPIClient<ProductTypeDTO>(TOKEN_API);
                    var lista = ApiClient.GetList().Result;
                    if (lista != null)
                    {
                        store.DataSource = lista.Value;
                        store.DataBind();
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

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            BaseAPIClient<ProductDTO> ApiClient = new BaseAPIClient<ProductDTO>(TOKEN_API);
            ProductDTO oDato;

            try
            {
                if (!bAgregar)
                {
                    string oCode = GridRowSelect.SelectedRows[0].RecordID;

                    oDato = ApiClient.GetByCode(oCode).Result.Value;

                    var originalCode = oDato.Code;

                    oDato.Name = txtServicio.Text;
                    oDato.Code = txtCodigo.Text;
                    oDato.Amount = (float)numCantidad.Number;
                    oDato.ProductTypeCode = cmbTipos.Value.ToString();

                    var Result = ApiClient.UpdateEntity(originalCode, oDato).Result;

                    if (Result.Success)
                    {
                        log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                              //  oDato.EntidadID = 0;
                                  //  oDato.CoreFrecuenciaID = long.Parse(cmbFrecuencias.SelectedItem.Value.ToString());
                                    //oDato.CoreFrecuenciaID = 0;
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = Result.Errors[0].Message;
                        return direct;
                    }
                }
                else
                {
                    oDato = new ProductDTO
                    {
                        Name = txtServicio.Text,
                        Code = txtCodigo.Text,
                        Description = txtDescripcion.Text,
                        Amount = (float)numCantidad.Number,
                        ProductTypeCode = cmbTipos.Value.ToString()
                    };

                    var Result = ApiClient.AddEntity(oDato).Result;

                    if (Result.Success)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = Result.Errors[0].Message;
                        return direct;
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
            hdEditando.SetValue("Editar");
            try
            {
                string oCode = GridRowSelect.SelectedRows[0].RecordID;
                BaseAPIClient<ProductDTO> ApiClient = new BaseAPIClient<ProductDTO>(TOKEN_API);
                var oDato = ApiClient.GetByCode(oCode).Result;
                txtServicio.Text = oDato.Value.Name;
                txtCodigo.Text = oDato.Value.Code;
                txtDescripcion.Text = oDato.Value.Description;
                numCantidad.Number = oDato.Value.Amount;
                cmbTipos.SetValue(oDato.Value.ProductTypeCode);
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
            BaseAPIClient<ProductDTO> ApiClient = new BaseAPIClient<ProductDTO>(TOKEN_API);

            var lID = GridRowSelect.SelectedRecordID;

            try
            {
                var Result = ApiClient.DeleteEntity(lID).Result;

                if (Result.Success)
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = Result.Errors[0].Message;
                    return direct;
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
        public DirectResponse ExportarFlujo()
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                //#region CONVERSION KEYS

                //string sEstados = Comun.ConvertKeyXML(GetGlobalResource("strEstados"));
                //string sCodigo = Comun.ConvertKeyXML(GetGlobalResource("strCodigo"));
                //string sNombre = Comun.ConvertKeyXML(GetGlobalResource("strNombre"));
                //string sPorcentaje = Comun.ConvertKeyXML(GetGlobalResource("strPorcentaje"));
                //string sDefecto = Comun.ConvertKeyXML(GetGlobalResource("strDefecto"));
                //string sCompletado = Comun.ConvertKeyXML(GetGlobalResource("strCompletado"));
                //string sGrupos = Comun.ConvertKeyXML(GetGlobalResource("strGrupos"));
                //string sDepartamento = Comun.ConvertKeyXML(GetGlobalResource("strDepartamento"));
                //string sEstadosSiguientes = Comun.ConvertKeyXML(GetGlobalResource("strEstadosSiguientes"));
                //string sEstado = Comun.ConvertKeyXML(GetGlobalResource("strEstado"));
                //string sEstadoSiguiente = Comun.ConvertKeyXML(GetGlobalResource("strEstadoSiguiente"));
                //string sTiposDocumentos = Comun.ConvertKeyXML(GetGlobalResource("strTiposDocumentos"));
                //string sDocumentoTipo = Comun.ConvertKeyXML(GetGlobalResource("strDocumentoTipo"));
                //string sRequerido = Comun.ConvertKeyXML(GetGlobalResource("strRequerido"));
                //string sValidado = Comun.ConvertKeyXML(GetGlobalResource("strValidado"));
                //string sObligatorio = Comun.ConvertKeyXML(GetGlobalResource("strObligatorio"));
                //string sEstadosGlobales = Comun.ConvertKeyXML(GetGlobalResource("strEstadosGlobales"));
                //string sEstadoGlobal = Comun.ConvertKeyXML(GetGlobalResource("strEstadoGlobal"));
                //string sInventarioElemento = Comun.ConvertKeyXML(GetGlobalResource("strInventarioElemento"));
                //string sDocumento = Comun.ConvertKeyXML(GetGlobalResource("strDocumento"));
                //string sRoles = Comun.ConvertKeyXML(GetGlobalResource("strRoles"));

                //#endregion

                //TipologiasController cTipologias = new TipologiasController();
                //string sNombreTipologia = "";
                //long lTipologiaID = 0;

                //if (cmbWorkflows.SelectedItem.Value != null && cmbWorkflows.SelectedItem.Value.ToString() != "")
                //{
                //    lTipologiaID = Convert.ToInt32(cmbWorkflows.SelectedItem.Value);
                //    sNombreTipologia = cTipologias.getCodigoByID(lTipologiaID);
                //}

                //log.Info(GetGlobalResource("strExportarFlujo"));
                //log.Info(GetGlobalResource("strComienzoExportacion"));

                //List<Data.Vw_CoreEstados> listaEstados = new List<Data.Vw_CoreEstados>();
                //EstadosController cEstados = new EstadosController();
                //listaEstados = cEstados.GetVistaCoreEstadosFromTipologia(lTipologiaID);

                ////
                //// Descargo la tabla de BBDD sobre un DataTale
                ////
                //DataTable EstadosDt = new DataTable();
                //EstadosDt.TableName = sNombreTipologia;

                //EstadosDt.Columns.Add(sCodigo);
                //EstadosDt.Columns.Add(sNombre);
                //EstadosDt.Columns.Add(sPorcentaje);
                //EstadosDt.Columns.Add(sDefecto);
                //EstadosDt.Columns.Add(sCompletado);
                //EstadosDt.Columns.Add(sGrupos);
                //EstadosDt.Columns.Add(sDepartamento);

                //foreach (var item in listaEstados)
                //{
                //    EstadosDt.Rows.Add();

                //    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sCodigo] = item.Codigo;
                //    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sPorcentaje] = item.Porcentaje;
                //    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sNombre] = item.NombreEstado;
                //    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sCompletado] = item.Completado;
                //    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sDefecto] = item.Defecto;
                //    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sGrupos] = item.NombreAgrupacion;
                //    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sDepartamento] = item.Departamento;

                //}

                //#region ESTADOS SIGUIENTES 

                //List<Data.CoreEstadosSiguientes> listaEstadosSig = new List<Data.CoreEstadosSiguientes>();
                //EstadosSiguientesController cEstadosSiguientes = new EstadosSiguientesController();
                //EstadosSiguientesController cEstSig = new EstadosSiguientesController();

                //foreach (var item in listaEstados)
                //{
                //    List<Data.CoreEstadosSiguientes> aux;
                //    aux = cEstadosSiguientes.getEstadosSiguientesByEstadoID(item.CoreEstadoID);
                //    listaEstadosSig.AddRange(aux);
                //}

                //#region DATA TABLE

                //DataTable dt3 = new DataTable();
                //dt3.TableName = sEstadosSiguientes;

                //dt3.Columns.Add(sEstado);
                //dt3.Columns.Add(sEstadoSiguiente);
                //dt3.Columns.Add(sDefecto);

                //#endregion

                //#region ESTADOS

                //foreach (var item in listaEstadosSig)
                //{
                //    dt3.Rows.Add();

                //    if (item.CoreEstadoID != 0 && item.CoreEstadoID != null && !DBNull.Value.Equals(item.CoreEstadoID))
                //    {
                //        Data.CoreEstados oEst = new Data.CoreEstados();

                //        oEst = cEstados.GetItem((long)item.CoreEstadoID);
                //        dt3.Rows[dt3.Rows.Count - 1][sEstado] = oEst.Codigo;
                //    }

                //    if (item.CoreEstadoPosibleID != 0 && item.CoreEstadoPosibleID != null && !DBNull.Value.Equals(item.CoreEstadoPosibleID))
                //    {
                //        Data.CoreEstados oEst = new Data.CoreEstados();

                //        oEst = cEstados.GetItem((long)item.CoreEstadoPosibleID);
                //        dt3.Rows[dt3.Rows.Count - 1][sEstadoSiguiente] = oEst.Codigo;
                //    }

                //    dt3.Rows[dt3.Rows.Count - 1][sDefecto] = item.Defecto;
                //}

                //#endregion

                //#endregion

                //#region TIPOS DOCUMENTOS

                //List<Data.CoreEstadosDocumentosTipos> listaDocumentos = new List<Data.CoreEstadosDocumentosTipos>();
                //EstadosDocumentosTiposController cTiposDocumentos = new EstadosDocumentosTiposController();
                //EstadosDocumentosTiposController cTipDocument = new EstadosDocumentosTiposController();

                //foreach (var item in listaEstados)
                //{
                //    List<Data.CoreEstadosDocumentosTipos> aux = new List<Data.CoreEstadosDocumentosTipos>();
                //    aux = cTiposDocumentos.getDocumentosByEstadoID(item.CoreEstadoID);
                //    listaDocumentos.AddRange(aux);
                //}

                //#region DATA TABLE

                //DataTable dt4 = new DataTable();
                //dt4.TableName = sTiposDocumentos;

                //dt4.Columns.Add(sEstado);
                //dt4.Columns.Add(sDocumentoTipo);
                //dt4.Columns.Add(sRequerido);
                //dt4.Columns.Add(sObligatorio);
                //dt4.Columns.Add(sValidado);

                //#endregion

                //#region ESTADOS

                //foreach (var item in listaDocumentos)
                //{
                //    dt4.Rows.Add();

                //    dt4.Rows[dt4.Rows.Count - 1][sRequerido] = item.RequiereFirma;
                //    dt4.Rows[dt4.Rows.Count - 1][sObligatorio] = item.Obligatorio;
                //    dt4.Rows[dt4.Rows.Count - 1][sValidado] = item.RequiereValidacion;

                //    if (item.CoreEstadoID != 0 && item.CoreEstadoID != null && !DBNull.Value.Equals(item.CoreEstadoID))
                //    {
                //        Data.CoreEstados oEst = new Data.CoreEstados();

                //        oEst = cEstados.GetItem((long)item.CoreEstadoID);
                //        dt4.Rows[dt4.Rows.Count - 1][sEstado] = oEst.Codigo;
                //    }

                //    if (item.DocumentoTipoID != 0 && item.DocumentoTipoID != null && !DBNull.Value.Equals(item.DocumentoTipoID))
                //    {
                //        Data.DocumentTipos oDoc = new Data.DocumentTipos();
                //        DocumentTiposController cDocs = new DocumentTiposController();

                //        oDoc = cDocs.GetItem((long)item.DocumentoTipoID);
                //        dt4.Rows[dt4.Rows.Count - 1][sDocumentoTipo] = oDoc.DocumentTipo;
                //    }
                //}

                //#endregion

                //#endregion

                //#region ESTADOS GLOBALES

                //List<Data.CoreEstadosGlobales> listaEstadosGlobales = new List<Data.CoreEstadosGlobales>();
                //CoreEstadosGlobalesController cEstadosGlobales = new CoreEstadosGlobalesController();
                //CoreEstadosGlobalesController cEstGlob = new CoreEstadosGlobalesController();

                //foreach (var item in listaEstados)
                //{
                //    List<Data.CoreEstadosGlobales> aux = new List<Data.CoreEstadosGlobales>();
                //    aux = cEstadosGlobales.getCoreEstadosGlobales(item.CoreEstadoID);
                //    listaEstadosGlobales.AddRange(aux);
                //}

                //#region DATA TABLE

                //DataTable dt5 = new DataTable();
                //dt5.TableName = sEstadosGlobales;

                //dt5.Columns.Add(sEstado);
                //dt5.Columns.Add(sEstadoGlobal);
                //dt5.Columns.Add(sInventarioElemento);
                //dt5.Columns.Add(sDocumento);

                //#endregion

                //#region ESTADOS

                //foreach (var item in listaEstadosGlobales)
                //{
                //    dt5.Rows.Add();

                //    if (item.CoreEstadoID != 0 && item.CoreEstadoID != null && !DBNull.Value.Equals(item.CoreEstadoID))
                //    {
                //        Data.CoreEstados oEst = new Data.CoreEstados();

                //        oEst = cEstados.GetItem((long)item.CoreEstadoID);
                //        dt5.Rows[dt5.Rows.Count - 1][sEstado] = oEst.Codigo;
                //    }

                //    if (item.DocumentoEstadoID != 0 && item.DocumentoEstadoID != null && !DBNull.Value.Equals(item.DocumentoEstadoID))
                //    {
                //        Data.DocumentosEstados oEst = new Data.DocumentosEstados();
                //        DocumentosEstadosController cDocEst = new DocumentosEstadosController();

                //        oEst = cDocEst.GetItem((long)item.DocumentoEstadoID);
                //        dt5.Rows[dt5.Rows.Count - 1][sDocumento] = oEst.Codigo;
                //    }

                //    if (item.EstadoGlobalID != 0 && item.EstadoGlobalID != null && !DBNull.Value.Equals(item.EstadoGlobalID))
                //    {
                //        Data.EstadosGlobales oEst = new Data.EstadosGlobales();
                //        EstadosGlobalesController cEstGlobal = new EstadosGlobalesController();

                //        oEst = cEstGlobal.GetItem((long)item.EstadoGlobalID);
                //        dt5.Rows[dt5.Rows.Count - 1][sEstadoGlobal] = oEst.EstadoGlobal;
                //    }

                //    if (item.InventarioElementoAtributoEstadoID != 0 && item.InventarioElementoAtributoEstadoID != null && !DBNull.Value.Equals(item.InventarioElementoAtributoEstadoID))
                //    {
                //        Data.InventarioElementosAtributosEstados oEst = new Data.InventarioElementosAtributosEstados();
                //        InventarioElementosAtributosEstadosController cInv = new InventarioElementosAtributosEstadosController();

                //        oEst = cInv.GetItem((long)item.InventarioElementoAtributoEstadoID);
                //        dt5.Rows[dt5.Rows.Count - 1][sInventarioElemento] = oEst.Codigo;
                //    }
                //}

                //#endregion

                //#endregion

                //#region ROLES

                //List<Data.CoreEstadosRoles> listaEstadosRoles = new List<Data.CoreEstadosRoles>();
                //CoreEstadosRolesController cEstadosRoles = new CoreEstadosRolesController();
                //CoreEstadosRolesController cRoles = new CoreEstadosRolesController();

                //foreach (var item in listaEstados)
                //{
                //    List<Data.CoreEstadosRoles> aux = new List<Data.CoreEstadosRoles>();
                //    aux = cEstadosRoles.getTablaRolesByEstadoID(item.CoreEstadoID);
                //    listaEstadosRoles.AddRange(aux);
                //}

                //#region DATA TABLE

                //DataTable dt6 = new DataTable();
                //dt6.TableName = sRoles;

                //dt6.Columns.Add(sEstado);
                //dt6.Columns.Add("Rol");

                //#endregion

                //#region ESTADOS

                //foreach (var item in listaEstadosRoles)
                //{
                //    dt6.Rows.Add();

                //    if (item.CoreEstadoID != 0 && item.CoreEstadoID != null && !DBNull.Value.Equals(item.CoreEstadoID))
                //    {
                //        Data.CoreEstados oEst = new Data.CoreEstados();

                //        oEst = cEstados.GetItem((long)item.CoreEstadoID);
                //        dt6.Rows[dt6.Rows.Count - 1][sEstado] = oEst.Codigo;
                //    }

                //    if (item.RolID != 0 && item.RolID != null && !DBNull.Value.Equals(item.RolID))
                //    {
                //        Data.Roles oEst = new Data.Roles();
                //        RolesController cRol = new RolesController();

                //        oEst = cRol.GetItem((long)item.RolID);
                //        dt6.Rows[dt6.Rows.Count - 1]["Rol"] = oEst.Codigo;
                //    }
                //}

                //#endregion

                //#endregion

                //string Filepath = "";
                //if (Properties.Settings.Default.RutaDocumentos[2] == '\\')
                //{
                //    Filepath = Properties.Settings.Default.RutaDocumentos;
                //}
                //else
                //{
                //    Filepath = Server.MapPath("~/" + Properties.Settings.Default.RutaDocumentos);
                //}

                //DateTime fecha = DateTime.Today;
                //string fileName = "";

                //if (sNombreTipologia != null)
                //{
                //    fileName = sEstados + " - " + sNombreTipologia + " - " + fecha.Day + fecha.Month + fecha.Year + ".xml";
                //    fileName = fileName.Replace(" ", "");
                //}
                //else
                //{
                //    fileName = sEstados + " - " + fecha.Day + fecha.Month + fecha.Year + ".xml";
                //}

                //string fullpath = Path.Combine(Filepath, fileName);

                //if (!System.IO.Directory.Exists(Filepath))
                //{ System.IO.Directory.CreateDirectory(Filepath); }

                //if (System.IO.File.Exists(fullpath))
                //{
                //    System.IO.File.Delete(fullpath);
                //}

                //DataTable[] tablas = { EstadosDt, dt3, dt4, dt5, dt6 };

                //XmlSerializer serializer = new XmlSerializer(typeof(DataTable[]));
                //TextWriter writer = new StreamWriter(fullpath);
                //serializer.Serialize(writer, tablas);
                //writer.Close();

                //cEstados = null;

            }
            catch (Exception ex)
            {
                log.Error(GetGlobalResource("strExportarFlujo"));
                log.Error(ex.Message);

                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }

            direct.Success = true;
            direct.Result = GetGlobalResource("strDescargaCorrecta");

            return direct;
        }

        [DirectMethod()]
        public DirectResponse ImportarFlujo()
        {
            DirectResponse direct = new DirectResponse();
            string sEstadosRepetidos = "";
            List<Object> listaMensajes = new List<Object>();
            Object oDato;
            var lista = "";

            #region CONVERSION KEYS

            string sCodigo = Comun.ConvertKeyXML(GetGlobalResource("strCodigo"));
            string sNombre = Comun.ConvertKeyXML(GetGlobalResource("strNombre"));
            string sPorcentaje = Comun.ConvertKeyXML(GetGlobalResource("strPorcentaje"));
            string sDefecto = Comun.ConvertKeyXML(GetGlobalResource("strDefecto"));
            string sCompletado = Comun.ConvertKeyXML(GetGlobalResource("strCompletado"));
            string sGrupos = Comun.ConvertKeyXML(GetGlobalResource("strGrupos"));
            string sDepartamento = Comun.ConvertKeyXML(GetGlobalResource("strDepartamento"));
            string sEstado = Comun.ConvertKeyXML(GetGlobalResource("strEstado"));
            string sEstadoSiguiente = Comun.ConvertKeyXML(GetGlobalResource("strEstadoSiguiente"));
            string sDocumentoTipo = Comun.ConvertKeyXML(GetGlobalResource("strDocumentoTipo"));
            string sRequerido = Comun.ConvertKeyXML(GetGlobalResource("strRequerido"));
            string sValidado = Comun.ConvertKeyXML(GetGlobalResource("strValidado"));
            string sObligatorio = Comun.ConvertKeyXML(GetGlobalResource("strObligatorio"));
            string sEstadoGlobal = Comun.ConvertKeyXML(GetGlobalResource("strEstadoGlobal"));
            string sInventarioElemento = Comun.ConvertKeyXML(GetGlobalResource("strInventarioElemento"));
            string sDocumento = Comun.ConvertKeyXML(GetGlobalResource("strDocumento"));

            #endregion

            try
            {
                //TipologiasController cTipologias = new TipologiasController();
                //string sNombreTipologia = "";
                //long lTipologiaID = 0;


                //if (cmbWorkflows.SelectedItem.Value != null && cmbWorkflows.SelectedItem.Value.ToString() != "")
                //{
                //    lTipologiaID = Convert.ToInt32(cmbWorkflows.SelectedItem.Value);
                //    sNombreTipologia = cTipologias.getCodigoByID(lTipologiaID);
                //}

                //string fileName = Path.GetFileName(FileUploadImportar.PostedFile.FileName);
                //string ext = Path.GetExtension(FileUploadImportar.PostedFile.FileName);

                //if (ext != ".xml")
                //{
                //    log.Error(GetGlobalResource("strFormatoIncorrecto"));

                //    oDato = cargarMensajes(GetGlobalResource("strFormatoIncorrecto"), "", false, "General");
                //    listaMensajes.Add(oDato);
                //}
                //else
                //{
                //    string fileDirectory = "";

                //    if (Properties.Settings.Default.RutaDocumentos[2] == '\\')
                //    {
                //        fileDirectory = Properties.Settings.Default.RutaDocumentos;
                //    }
                //    else
                //    {
                //        fileDirectory = Server.MapPath("~/" + Properties.Settings.Default.RutaDocumentos);
                //    }

                //    if (!System.IO.Directory.Exists(fileDirectory))
                //    { System.IO.Directory.CreateDirectory(fileDirectory); }

                //    string fullpath = Path.Combine(fileDirectory, fileName);

                //    if (!System.IO.File.Exists(fullpath))
                //    {
                //        System.IO.File.Delete(fullpath);
                //    }

                //    if (FileUploadImportar.HasFile)
                //    {
                //        FileUploadImportar.PostedFile.SaveAs(fullpath);

                //        // Deserializo y paso a formato Datatable el documento xml, guardo el fichero provisional
                //        XmlSerializer serializer = new XmlSerializer(typeof(DataTable[]));
                //        FileStream loadStream = new FileStream(fullpath, FileMode.Open, FileAccess.Read);
                //        DataTable[] tablas = (DataTable[])serializer.Deserialize(loadStream);
                //        loadStream.Close();
                //        int iEstadoAñadido = 0;
                //        int iEstadoSiguienteAñadido = 0;
                //        int iEstadoGlobalAñadido = 0;
                //        int iDocumentoAñadido = 0;
                //        int iRolAñadido = 0;

                //        EstadosController cEstados = new EstadosController();

                //        for (int k = 0; k < tablas[0].Rows.Count; k++)
                //        {
                //            Data.CoreEstados oEstado = new Data.CoreEstados();
                //            DataRow row = tablas[0].Rows[k];

                //            oEstado.Codigo = Convert.ToString(row[sCodigo]);
                //            oEstado.Porcentaje = Convert.ToInt32(row[sPorcentaje]);
                //            oEstado.Nombre = Convert.ToString(row[sNombre]);
                //            oEstado.Completado = Convert.ToBoolean(row[sCompletado]);
                //            oEstado.Defecto = Convert.ToBoolean(row[sDefecto]);
                //            oEstado.CoreTipologiaID = lTipologiaID;

                //            if (row[sGrupos] != null && !DBNull.Value.Equals(row[sGrupos]))
                //            {
                //                AgrupacionEstadosController cGrup = new AgrupacionEstadosController();
                //                oEstado.EstadoAgrupacionID = cGrup.GetEstadosAgrupacionesByNombre(row[sGrupos].ToString()).EstadoAgrupacionID;
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sGrupos, false, oEstado.Codigo);
                //                listaMensajes.Add(oDato);
                //            }

                //            if (row[sDepartamento] != null && !DBNull.Value.Equals(row[sDepartamento]))
                //            {
                //                DepartamentosController cDep = new DepartamentosController();
                //                oEstado.DepartamentoID = cDep.GetDepartamentoID(row[sDepartamento].ToString());
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sDepartamento, false, oEstado.Codigo);
                //                listaMensajes.Add(oDato);
                //            }

                //            if (!cEstados.ExisteEstadoByTipologia((long)oEstado.CoreTipologiaID, oEstado.Codigo))
                //            {
                //                oEstado = cEstados.AddItem(oEstado);
                //                iEstadoAñadido++;

                //                oDato = cargarMensajes(GetGlobalResource("strEstadoAñadido"), oEstado.Codigo, true, oEstado.Codigo);
                //                listaMensajes.Add(oDato);
                //            }
                //            else
                //            {
                //                if (sEstadosRepetidos.Equals(""))
                //                {
                //                    sEstadosRepetidos = Convert.ToString(row[sCodigo]);
                //                }
                //                else
                //                {
                //                    sEstadosRepetidos += (", " + Convert.ToString(row[sCodigo]));
                //                }
                //            }
                //        }

                //        #region ESTADOS SIGUIENTES

                //        EstadosSiguientesController cEstadosSiguientes = new EstadosSiguientesController();

                //        for (int k = 0; k < tablas[1].Rows.Count; k++)
                //        {
                //            Data.CoreEstadosSiguientes oEstSig = new Data.CoreEstadosSiguientes();
                //            DataRow row = tablas[1].Rows[k];

                //            if (row[sEstado] != null && !DBNull.Value.Equals(row[sEstado]))
                //            {
                //                oEstSig.CoreEstadoID = (long)cEstados.GetEstadoIDByTipologia(row[sEstado].ToString(), lTipologiaID);
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstado, false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }

                //            if (row[sEstadoSiguiente] != null && !DBNull.Value.Equals(row[sEstadoSiguiente]))
                //            {
                //                oEstSig.CoreEstadoPosibleID = (long)cEstados.GetEstadoIDByTipologia(row[sEstadoSiguiente].ToString(), lTipologiaID);
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstadoSiguiente, false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }

                //            oEstSig.Defecto = Convert.ToBoolean(row[sDefecto]);

                //            if (!cEstadosSiguientes.RegistroDuplicado(oEstSig.CoreEstadoID, oEstSig.CoreEstadoPosibleID))
                //            {
                //                cEstadosSiguientes.AddItem(oEstSig);
                //                iEstadoSiguienteAñadido++;

                //                oDato = cargarMensajes(GetGlobalResource("strEstadoSiguienteAñadido"), Convert.ToString(row[sEstadoSiguiente]), true, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strEstadoSiguienteDuplicado"), Convert.ToString(row[sEstadoSiguiente]), false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }
                //        }

                //        #endregion

                //        #region TIPOS DOCUMENTOS 

                //        EstadosDocumentosTiposController cEstadosDocumentos = new EstadosDocumentosTiposController();
                //        DocumentTiposController cDoc = new DocumentTiposController();

                //        for (int k = 0; k < tablas[2].Rows.Count; k++)
                //        {
                //            Data.CoreEstadosDocumentosTipos oEstDoc = new Data.CoreEstadosDocumentosTipos();
                //            DataRow row = tablas[2].Rows[k];

                //            if (row[sEstado] != null && !DBNull.Value.Equals(row[sEstado]))
                //            {
                //                oEstDoc.CoreEstadoID = (long)cEstados.GetEstadoIDByTipologia(row[sEstado].ToString(), lTipologiaID);
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstado, false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }

                //            if (row[sDocumentoTipo] != null && !DBNull.Value.Equals(row[sDocumentoTipo]))
                //            {
                //                oEstDoc.DocumentoTipoID = cDoc.GetDocTipo(row[sDocumentoTipo].ToString());
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sDocumentoTipo, false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }

                //            oEstDoc.Obligatorio = Convert.ToBoolean(row[sObligatorio]);
                //            oEstDoc.RequiereFirma = Convert.ToBoolean(row[sRequerido]);
                //            oEstDoc.RequiereValidacion = Convert.ToBoolean(row[sValidado]);

                //            if (!cEstadosDocumentos.RegistroDuplicado(oEstDoc.CoreEstadoID, oEstDoc.DocumentoTipoID))
                //            {
                //                cEstadosDocumentos.AddItem(oEstDoc);
                //                iDocumentoAñadido++;

                //                oDato = cargarMensajes(GetGlobalResource("strDocumentoAñadido"), Convert.ToString(row[sDocumentoTipo]), true, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strDocumentoDuplicado"), Convert.ToString(row[sDocumentoTipo]), false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }
                //        }

                //        #endregion

                //        #region ESTADOS GLOBALES 

                //        CoreEstadosGlobalesController cEstadosGlobales = new CoreEstadosGlobalesController();
                //        EstadosGlobalesController cGlobales = new EstadosGlobalesController();
                //        InventarioElementosAtributosEstadosController cInv = new InventarioElementosAtributosEstadosController();
                //        DocumentosEstadosController cDocEstados = new DocumentosEstadosController();

                //        for (int k = 0; k < tablas[3].Rows.Count; k++)
                //        {
                //            Data.CoreEstadosGlobales oEstGlobal = new Data.CoreEstadosGlobales();
                //            DataRow row = tablas[3].Rows[k];
                //            string sEstGlobal = "";

                //            if (row[sEstado] != null && !DBNull.Value.Equals(row[sEstado]))
                //            {
                //                oEstGlobal.CoreEstadoID = (long)cEstados.GetEstadoIDByTipologia(row[sEstado].ToString(), lTipologiaID);
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstado, false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }

                //            if (row[sEstadoGlobal] != null && !DBNull.Value.Equals(row[sEstadoGlobal]))
                //            {
                //                oEstGlobal.EstadoGlobalID = cGlobales.GetEstadoGlobalByNombre(row[sEstadoGlobal].ToString());
                //                sEstGlobal = Convert.ToString(row[sEstadoGlobal]);
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstadoGlobal, false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }

                //            if (row[sInventarioElemento] != null && !DBNull.Value.Equals(row[sInventarioElemento]))
                //            {
                //                oEstGlobal.InventarioElementoAtributoEstadoID = cInv.GetEstadoIDByCodigo(long.Parse(hdCliID.Value.ToString()), row[sInventarioElemento].ToString());
                //                sEstGlobal = Convert.ToString(row[sInventarioElemento]);
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sInventarioElemento, false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }

                //            if (row[sDocumento] != null && !DBNull.Value.Equals(row[sDocumento]))
                //            {
                //                oEstGlobal.DocumentoEstadoID = cDocEstados.getIDByCodigo(row[sDocumento].ToString());
                //                sEstGlobal = Convert.ToString(row[sDocumento]);
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sDocumento, false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }

                //            if (!cEstadosGlobales.RegistroDuplicado((long)oEstGlobal.CoreEstadoID, oEstGlobal.EstadoGlobalID, oEstGlobal.InventarioElementoAtributoEstadoID, oEstGlobal.DocumentoEstadoID))
                //            {
                //                cEstadosGlobales.AddItem(oEstGlobal);
                //                iEstadoGlobalAñadido++;

                //                oDato = cargarMensajes(GetGlobalResource("strEstadoGlobalAñadido"), sEstGlobal, true, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strEstadoGlobalDuplicado"), sEstGlobal, false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }
                //        }

                //        #endregion

                //        #region ROLES 

                //        CoreEstadosRolesController cEstadosRoles = new CoreEstadosRolesController();
                //        RolesController cRoles = new RolesController();

                //        for (int k = 0; k < tablas[4].Rows.Count; k++)
                //        {
                //            Data.CoreEstadosRoles oEstRol = new Data.CoreEstadosRoles();
                //            DataRow row = tablas[4].Rows[k];

                //            if (row[sEstado] != null && !DBNull.Value.Equals(row[sEstado]))
                //            {
                //                oEstRol.CoreEstadoID = (long)cEstados.GetEstadoIDByTipologia(row[sEstado].ToString(), lTipologiaID);
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstado, false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }

                //            if (row["Rol"] != null && !DBNull.Value.Equals(row["Rol"]))
                //            {
                //                oEstRol.RolID = cRoles.getIDByCodigo(row["Rol"].ToString());
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), "Rol", false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }

                //            if (!cEstadosRoles.RegistroDuplicado((long)oEstRol.CoreEstadoID, (long)oEstRol.RolID))
                //            {
                //                cEstadosRoles.AddItem(oEstRol);
                //                iRolAñadido++;

                //                oDato = cargarMensajes(GetGlobalResource("strRolAñadido"), Convert.ToString(row["Rol"]), true, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }
                //            else
                //            {
                //                oDato = cargarMensajes(GetGlobalResource("strRolDuplicado"), Convert.ToString(row["Rol"]), false, Convert.ToString(row[sEstado]));
                //                listaMensajes.Add(oDato);
                //            }
                //        }

                //        #endregion

                //        if (sEstadosRepetidos.Equals(""))
                //        {
                //            log.Info(GetGlobalResource("strSubidaCorrecta"));
                //        }
                //        else
                //        {
                //            log.Error(GetGlobalResource("strSubidaFallo"));

                //            oDato = cargarMensajes(GetGlobalResource("strEstadosDuplicados"), sEstadosRepetidos, false, "Resumen");
                //            listaMensajes.Add(oDato);
                //        }

                //        lista = Newtonsoft.Json.JsonConvert.SerializeObject(listaMensajes);
                //    }
                //}
            }
            catch (Exception)
            {
                log.Error(GetGlobalResource("strImportarFlujo"));
                log.Error(GetGlobalResource("strErrorImportacion"));

                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }

            //storeCoreEstados.DataBind();
            FileUploadImportar.PostedFile.InputStream.Dispose();
            FileUploadImportar.Clear();

            direct.Success = true;
            direct.Result = lista;

            return direct;
        }

        #endregion
    }
}