using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Reflection;
using System.Linq;
using TreeCore.Data;
using Tree.Linq.GenericExtensions;
using Aspose.Zip;
using Aspose.Zip.Saving;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Threading;
using System.Transactions;
using System.Web.Configuration;
using TreeCore.APIClient;
using TreeCore.Shared.DTO.ImportExport;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Sites;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.Utilities.Enum;

namespace TreeCore.ModExportarImportar
{
    public partial class DataUpload : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));


                ResourceManagerOperaciones(ResourceManagerTreeCore);

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region DESCARGA PLANTILLA

            if ((Request.QueryString["TipoProyecto"] != null))
            {
                string sProyectoTipo = Request.QueryString["TipoProyecto"];
            }

            if ((Request.QueryString["DescargarPlantilla"] != null))
            {
                string sProyectoTipo = Request.QueryString["TipoProyecto"];
                string sPlantilla = Request.QueryString["Plantilla"];

                DescargarPlantilla(sProyectoTipo, sPlantilla);
            }

            #endregion
        }

        #endregion

        #region STORES

        #region PLANTILLAS

        protected void storeDocumentosCargaPlantillas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.DocumentosCargasPlantillas> listaDatos = null;
                    DocumentosCargasPlantillasController cPlantillas = new DocumentosCargasPlantillasController();

                    listaDatos = cPlantillas.GetAllPlantillasSinAyuda(true);

                    if (listaDatos != null)
                    {
                        storeDocumentosCargaPlantillas.DataSource = listaDatos;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region AUXILIAR

        protected void storeAuxiliar_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    JsonObject jsonlista = new JsonObject();
                    List<JsonObject> jsonAux = new List<JsonObject>();

                    long cliID = 0;

                    if (ClienteID != null)
                    {
                        cliID = ClienteID.Value;
                    }
                    else
                    {
                        if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                        {
                            cliID = long.Parse(hdCliID.Value.ToString());
                        }
                    }

                    if (cmbPlantillas.Value.ToString() == "INVENTORY")
                    {
                        InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                        var listaCategorias = cCategorias.GetInventarioCategoriasActivas(cliID);
                        if (listaCategorias != null)
                        {
                            foreach (var item in listaCategorias)
                            {
                                jsonlista = new JsonObject();
                                jsonlista.Add("Nombre", item.InventarioCategoria);
                                jsonlista.Add("ElementoID", item.InventarioCategoriaID);
                                jsonAux.Add(jsonlista);
                            }
                            storeAuxiliar.DataSource = jsonAux;
                            storeAuxiliar.DataBind();
                        }
                    }
                    else if (cmbPlantillas.Value.ToString() == "FORM SECTIONS")
                    {
                        CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCatConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
                        var listaCategorias = cCatConf.GetPlantillas(cliID, true);
                        if (listaCategorias != null)
                        {
                            foreach (var item in listaCategorias)
                            {
                                jsonlista = new JsonObject();
                                jsonlista.Add("Nombre", item.InventarioAtributosCategorias.InventarioAtributoCategoria);
                                jsonlista.Add("ElementoID", item.CoreInventarioCategoriaAtributoCategoriaConfiguracionID);
                                jsonAux.Add(jsonlista);
                            }
                            storeAuxiliar.DataSource = jsonAux;
                            storeAuxiliar.DataBind();
                        }
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
        public DirectResponse ExistePlantilla(string sPlantilla)
        {

            DirectResponse direct = new DirectResponse();

            ProyectosTiposController cProyectos = new ProyectosTiposController();
            DocumentosCargasPlantillasController cPlantillas = new DocumentosCargasPlantillasController();

            try
            {
                Data.DocumentosCargasPlantillas oDato = cPlantillas.getPlantillasByName(sPlantilla);

                if (oDato != null && oDato.RutaDocumento != null && oDato.RutaDocumento != "")
                {
                    if (!oDato.RutaDocumento.Contains("."))
                    {
                        Exportacion(oDato.RutaDocumento);

                        #region Crear Zip
                        string pathZIP = DirectoryMapping.GetTempDirectory("ZIP");
                        pathZIP = pathZIP.Replace("\\", "/");
                        string nameFileZIP = DateTime.Now.Ticks + ".zip";
                        pathZIP += "/" + nameFileZIP;

                        using (FileStream zipFile = File.Open(pathZIP, FileMode.Create))
                        {
                            FileInfo fi1 = new FileInfo(hdRuta1.Value.ToString());
                            FileInfo fi2 = new FileInfo(hdRuta2.Value.ToString());

                            using (var archive = new Archive())
                            {
                                archive.CreateEntry(fi1.Name, fi1);
                                archive.CreateEntry(fi2.Name, fi2);
                                archive.Save(zipFile, new ArchiveSaveOptions() { Encoding = Encoding.ASCII });

                            }
                        }

                        #endregion

                        #region Descargar ZIP

                        Response.AddHeader("content-disposition", "attachment; filename=" + nameFileZIP);

                        FileInfo file = new FileInfo(pathZIP);

                        HttpContext.Current.Response.AppendHeader("Content-Length", file.Length.ToString());
                        HttpContext.Current.Response.TransmitFile(pathZIP);
                        HttpContext.Current.Response.Flush();

                        #endregion

                        direct.Success = true;
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsTamanoDocumentoExcedido);
                    }
                }
                else
                {
                    MensajeBox(GetGlobalResource(Comun.strExportarLog), GetGlobalResource(Comun.strArchivoInexistente), Ext.Net.MessageBox.Icon.WARNING, null);
                }

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse DescargarPlantilla(string sProyectoTipo, string sPlantilla)
        {
            DirectResponse direct = new DirectResponse();
            DocumentosCargasPlantillasController cPlantillas = new DocumentosCargasPlantillasController();
            ProyectosTiposController cProyectos = new ProyectosTiposController();
            string sRutaOrigen = "";

            try
            {
                if (sProyectoTipo != null && sPlantilla != null)
                {
                    long lProyectoTipoID = cProyectos.getidProyectoTipo(sProyectoTipo);
                    Data.DocumentosCargasPlantillas oDato = cPlantillas.getPlantillasByProyectoIDByName(lProyectoTipoID, sPlantilla);

                    if (oDato.RutaDocumento != "" && oDato.RutaDocumento != null)
                    {
                        sRutaOrigen = oDato.RutaDocumento;
                    }
                    else
                    {
                        MensajeBox(GetGlobalResource(Comun.strExportarLog), GetGlobalResource(Comun.strArchivoInexistente), Ext.Net.MessageBox.Icon.WARNING, null);
                    }
                }
                if (File.Exists(sRutaOrigen))
                {
                    string sFilepath = "";
                    string sNombrearchivo = sRutaOrigen.Substring(sRutaOrigen.LastIndexOf('\\') + 1);

                    sFilepath = TreeCore.DirectoryMapping.GetDocumentDirectory();

                    string sRutaDest = Path.Combine(sFilepath, sNombrearchivo);

                    if (File.Exists(sRutaOrigen))
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(sRutaOrigen);

                        Response.Clear();
                        Response.ContentType = Comun.GetMimeType(file.Extension);

                        Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);
                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.TransmitFile(sRutaOrigen);
                        Response.Flush();
                    }

                    Response.SuppressContent = true;
                    System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                else
                {
                    MensajeBox(GetGlobalResource(Comun.strExportarLog), GetGlobalResource(Comun.strArchivoInexistente), Ext.Net.MessageBox.Icon.WARNING, null);
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
            direct.Result = Response;

            return direct;
        }

        #endregion

        #region FUNCTIONS

        public string GetValoresStringComboBox(List<string> lista)
        {
            string valores = "";

            for (int j = 0; j < lista.Count; j = j + 1)
            {
                if (j == 0)
                {
                    valores = lista[j].ToString();
                }
                else
                {
                    valores += ", " + lista[j].ToString();
                }
            }

            return valores;
        }

        public List<string> GetValoresPosibles()
        {
            List<string> lista = new List<string>();

            EntidadesController cOperadores = new EntidadesController();
            MonedasController cMonedas = new MonedasController();
            EstadosGlobalesController cEstadosGlobales = new EstadosGlobalesController();
            EmplazamientosCategoriasSitiosController cCategorias = new EmplazamientosCategoriasSitiosController();
            EmplazamientosTiposController cTipos = new EmplazamientosTiposController();
            EmplazamientosTiposEdificiosController cEdificios = new EmplazamientosTiposEdificiosController();
            EmplazamientosTiposEstructurasController cEstructuras = new EmplazamientosTiposEstructurasController();
            EmplazamientosTamanosController cTamanos = new EmplazamientosTamanosController();

            long cliID = 0;

            if (ClienteID != null)
            {
                cliID = ClienteID.Value;
            }
            else
            {
                if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
            }

            lista.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
            lista.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
            lista.Add(GetValoresStringComboBox(cOperadores.getEntidadesOperadores(cliID).Select(c => c.Codigo).ToList()));
            lista.Add(GetValoresStringComboBox(cMonedas.GetMonedasCodigo(cliID)));
            lista.Add(GetValoresStringComboBox(cEstadosGlobales.GetEstadosGlobalesNombre(cliID)));
            lista.Add(GetValoresStringComboBox(cCategorias.GetCategoriasNombre(cliID)));
            lista.Add(GetValoresStringComboBox(cTipos.GetTiposNombre(cliID)));
            lista.Add(GetValoresStringComboBox(cEdificios.GetEdificiosNombre(cliID)));
            lista.Add(GetValoresStringComboBox(cEstructuras.GetEstructurasNombre(cliID)));
            lista.Add(GetValoresStringComboBox(cTamanos.GetTamanosNombre(cliID)));
            lista.Add("yyyyMMdd");
            lista.Add("yyyyMMdd");
            lista.Add(" ");
            lista.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "150"));
            lista.Add(GetGlobalResource("strMaximo"));
            lista.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
            lista.Add(GetGlobalResource("strEntero") + "-" + GetGlobalResource("strDecimal"));
            lista.Add(GetGlobalResource("strEntero") + "-" + GetGlobalResource("strDecimal"));

            return lista;
        }

        [DirectMethod()]
        public DirectResponse Exportacion(string sRuta)
        {
            DirectResponse direct = new DirectResponse();
            DocumentosCargasPlantillasController cPlantillas = new DocumentosCargasPlantillasController();
            ProyectosTiposController cProyectos = new ProyectosTiposController();

            try
            {
                if (sRuta != null && sRuta != "")
                {
                    if (sRuta == "ExportarModeloInventario")
                    {
                        string sTipoEmplazamiento = "0";
                        ExportarModeloInventario(sTipoEmplazamiento, long.Parse(hdCliID.Value.ToString()));

                        ExportarAyudaModeloInventario(sTipoEmplazamiento, long.Parse(hdCliID.Value.ToString()));
                    }
                    else if (sRuta == "ExportarModeloDocumental")
                    {
                        GenerarPlantillaModeloDocumental();

                        GenerarPlantillaAyudaModeloDocumental();
                    }
                    else if (sRuta == "ExportarModeloEmplazamiento")
                    {
                        ExportarModeloEmplazamiento();

                        ExportarAyudaModeloEmplazamiento();
                    }
                    else if (sRuta == "ExportarModeloEmpContactos")
                    {
                        ExportarModeloEmpContactos();

                        ExportarAyudaModeloEmpContactos();
                    }
                    else if (sRuta == "ExportarModeloEntidades")
                    {
                        ExportarModeloEntidades();

                        ExportarAyudaModeloEntidades();
                    }
                    else if (sRuta == "ExportarModeloRegional")
                    {
                        ExportarModeloRegional();

                        ExportarAyudaModeloRegional();
                    }
                    else if (sRuta == "ExportarModeloVinculacionesElementos")
                    {
                        GenerarPlantillaVinculacionesInventario();

                        GenerarPlantillaAyudaVinculacionesInventario();
                    }
                    else if (sRuta == "ExportarModeloSubcategorias")
                    {
                        GenerarPlantillaSubCategoriasInventario();

                        GenerarPlantillaAyudaSubCategoriasInventario();
                    }
                    else if (sRuta == "ExportarModeloUsuarios")
                    {
                        ExportarModeloUsuarios();

                        ExportarAyudaModeloUsuarios();
                    }
                    else if (sRuta == "ExportarModeloProductCatalogServicios")
                    {
                        ExportarModeloProductCatalogServicios();

                        ExportarAyudaModeloProductCatalogServicios();
                    }
                    else if (sRuta == "ExportarModeloProductCatalogCatalogos")
                    {
                        ExportarModeloProductCatalogCatalogos();

                        ExportarAyudaModeloProductCatalogCatalogos();
                    }
                    else if (sRuta == "ExportarModeloContracts")
                    {
                        ExportarModeloContracts();

                        ExportarAyudaModeloContracts();
                    }
                    else
                    {
                        MensajeBox(GetGlobalResource(Comun.strExportarLog), GetGlobalResource(Comun.strArchivoInexistente), Ext.Net.MessageBox.Icon.WARNING, null);
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
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

        #region CATALOGOS

        [DirectMethod]
        public DirectResponse ExportarModeloProductCatalogCatalogos()
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogsController cServicios = new CoreProductCatalogsController();

            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloDatosProductCatalogs") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta1.SetValue(saveAs);

                #region CLIENTE
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                List<string> filaCabecera;
                List<List<string>> Listas;

                int cont = 0;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region CATALOGOS

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();
                filaCabecera.Add(GetGlobalResource("strCodigo"));
                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add(GetGlobalResource("strDescripcion"));
                filaCabecera.Add($"{GetGlobalResource("strTipo")} {GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strMoneda")} {GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strFechaInicio"));
                filaCabecera.Add(GetGlobalResource("strFechaFin"));

                filaCabecera.Add($"{GetGlobalResource("strReajustes")} {GetGlobalResource("strTipo")}");
                filaCabecera.Add($"{GetGlobalResource("strInflacion")}");
                filaCabecera.Add($"{GetGlobalResource("strCantidadFija")}");
                filaCabecera.Add($"{GetGlobalResource("strPorcentajeFijo")}");
                filaCabecera.Add($"{GetGlobalResource("strPorcentajeFijo")}");
                filaCabecera.Add(GetGlobalResource("strFrecuencia"));
                filaCabecera.Add(GetGlobalResource("strFechaInicioReajuste"));
                filaCabecera.Add(GetGlobalResource("strFechaProximaRevision"));
                filaCabecera.Add(GetGlobalResource("strFechaFinReajuste"));

                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strProductCatalog"), Listas, cont);
                cont++;

                #endregion

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

        [DirectMethod]
        public DirectResponse ExportarAyudaModeloProductCatalogCatalogos()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloAyudaDatosProductCatalog") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta2.SetValue(saveAs);

                #region CLIENTE
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                int cont = 0;

                List<string> filaCabecera;
                List<string> filaRequerido;
                List<string> filaTipoDato;
                List<string> filaFormato;
                List<string> filaMaxValue;
                List<string> filaMinValue;
                List<string> filaRegex;

                List<List<string>> Listas;

                List<List<string>> ColumnasValores;
                List<string> colValores;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region CATALOGOS

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strCodigo"));
                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add(GetGlobalResource("strDescripcion"));
                filaCabecera.Add($"{GetGlobalResource("strTipo")}");
                filaCabecera.Add($"{GetGlobalResource("strMoneda")}");
                filaCabecera.Add(GetGlobalResource("strFechaInicio"));
                filaCabecera.Add(GetGlobalResource("strFechaFin"));

                filaCabecera.Add($"{GetGlobalResource("strReajustes")} {GetGlobalResource("strTipo")}");
                filaCabecera.Add($"{GetGlobalResource("strInflacion")}");
                filaCabecera.Add($"{GetGlobalResource("strCantidadFija")}");
                filaCabecera.Add($"{GetGlobalResource("strPorcentajeFijo")}");
                filaCabecera.Add(GetGlobalResource("strFrecuencia"));
                filaCabecera.Add(GetGlobalResource("strFechaInicioReajuste"));
                filaCabecera.Add(GetGlobalResource("strFechaProximaRevision"));
                filaCabecera.Add(GetGlobalResource("strFechaFinReajuste"));

                #endregion

                #region Reqerido

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("False");

                filaRequerido.Add("True");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");

                #endregion

                #region Fila Tipos de datos

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));

                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));

                #endregion

                #region Formato

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("yyyyMMdd");
                filaFormato.Add("yyyyMMdd");

                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("yyyyMMdd");
                filaFormato.Add("yyyyMMdd");
                filaFormato.Add("yyyyMMdd");

                #endregion

                #region Max Value

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region Min Value

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");

                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("0");
                filaMinValue.Add("0");
                filaMinValue.Add("0");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region Regex

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strCatalogos"), Listas, cont);
                cont++;

                #region Values

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigo"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strNombre"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strDescripcion"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strTipo"));
                BaseAPIClient<CatalogTypeDTO> cCatalogType = new BaseAPIClient<CatalogTypeDTO>(TOKEN_API);
                foreach (var valor in cCatalogType.GetList().Result.Value)
                {
                    colValores.Add(valor.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strMoneda"));
                BaseAPIClient<CurrencyDTO> cCurrency = new BaseAPIClient<CurrencyDTO>(TOKEN_API);
                foreach (var valor in cCurrency.GetList().Result.Value)
                {
                    colValores.Add(valor.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strTipo"));
                CoreProductCatalogTiposController cTipos = new CoreProductCatalogTiposController();
                foreach (var valor in cTipos.GetAllCoreProductCatalogTiposByClienteID(cliID))
                {
                    colValores.Add(valor.Nombre);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strFechaInicio"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strFechaFin"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strReajustes"));
                colValores.Add(GetGlobalResource("strSinIncremento"));
                colValores.Add(GetGlobalResource("strCPI"));
                colValores.Add(GetGlobalResource("strCantidadFija"));
                colValores.Add(GetGlobalResource("strPorcentajeFijo"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strFechaInicioReajuste"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strFechaProximaRevision"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strFechaFinReajuste"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strInflacion"));
                InflacionesController cInflaciones = new InflacionesController();
                foreach (var valor in cInflaciones.GetAllInflaciones())
                {
                    colValores.Add(valor.Inflacion);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strValor"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strPorcentaje"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCadencia"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCantidadFija"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strPorcentajeFijo"));
                ColumnasValores.Add(colValores);

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);


                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strCatalogos") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;


                ColumnasValores = new List<List<string>>();

                #region REAJUSTES PRECIOS

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strTipo"));
                colValores.Add(GetGlobalResource("strFechaInicio"));
                colValores.Add(GetGlobalResource("strFechaProximaRevision"));
                colValores.Add(GetGlobalResource("strFechaFin"));
                colValores.Add(GetGlobalResource("strInflacion"));
                colValores.Add(GetGlobalResource("strCadencia"));
                colValores.Add(GetGlobalResource("strCantidadFija"));
                colValores.Add(GetGlobalResource("strPorcentajeFijo"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strSinIncremento"));
                colValores.Add("");
                colValores.Add("");
                colValores.Add("");
                colValores.Add("");
                colValores.Add("");
                colValores.Add("");
                colValores.Add("");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCPI"));
                colValores.Add("X");
                colValores.Add("X");
                colValores.Add("X");
                colValores.Add("X");
                colValores.Add("");
                colValores.Add("");
                colValores.Add("");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCantidadFija"));
                colValores.Add("X");
                colValores.Add("X");
                colValores.Add("X");
                colValores.Add("");
                colValores.Add("X");
                colValores.Add("X");
                colValores.Add("");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strPorcentajeFijo"));
                colValores.Add("X");
                colValores.Add("X");
                colValores.Add("X");
                colValores.Add("");
                colValores.Add("X");
                colValores.Add("");
                colValores.Add("X");
                ColumnasValores.Add(colValores);


                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strReajustes"), ColumnasValores, cont);
                cont++;

                #endregion


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

        #endregion

        #region COMPANIES

        [DirectMethod]
        public DirectResponse ExportarAyudaModeloEntidades()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloAyudaDatosEntidades") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta2.SetValue(saveAs);

                #region CLIENTEID
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                int cont = 0;

                List<string> filaCabecera;
                List<string> filaRequerido;
                List<string> filaTipoDato;
                List<string> filaFormato;
                List<string> filaMaxValue;
                List<string> filaMinValue;
                List<string> filaRegex;

                List<List<string>> Listas;

                List<List<string>> ColumnasValores;
                List<string> colValores;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region COMPANIES

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region HEADER ROW

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strCodigo"));
                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add("Alias");
                filaCabecera.Add(GetGlobalResource("strEmail"));
                filaCabecera.Add(GetGlobalResource("strTelefono"));
                filaCabecera.Add(GetGlobalResource("strActivo"));
                filaCabecera.Add(GetGlobalResource("strEntidadPropietario"));
                filaCabecera.Add("Supplier");
                filaCabecera.Add(GetGlobalResource("strEsCliente"));
                filaCabecera.Add("Payee");
                filaCabecera.Add("TaxIdentificationNumber");
                filaCabecera.Add($"{GetGlobalResource("strEntidadesTipos")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strContribuyente")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strTipoNIF")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strCondicionPago")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strMoneda")}:{GetGlobalResource("strCodigo")}");

                #endregion

                #region REQUIRED

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("True");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("True");
                filaRequerido.Add("False");
                filaRequerido.Add("True");
                filaRequerido.Add("False");
                filaRequerido.Add("True");

                #endregion

                #region DATA TYPE

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));
                filaTipoDato.Add(GetGlobalResource("strBooleano"));
                filaTipoDato.Add(GetGlobalResource("strBooleano"));
                filaTipoDato.Add(GetGlobalResource("strBooleano"));
                filaTipoDato.Add(GetGlobalResource("strBooleano"));
                filaTipoDato.Add(GetGlobalResource("strBooleano"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));

                #endregion

                #region FORMAT

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region MAX VALUE

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region MIN VALUE

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region REGEX

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strEntidades"), Listas, cont);
                cont++;

                #region VALUES

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigo"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strNombre"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strAlias"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strEmail"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strTelefono"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strActivo"));
                colValores.Add("TRUE");
                colValores.Add("FALSE");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strEntidadPropietario"));
                colValores.Add("TRUE");
                colValores.Add("FALSE");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strProveedores"));
                colValores.Add("TRUE");
                colValores.Add("FALSE");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strEsCliente"));
                colValores.Add("TRUE");
                colValores.Add("FALSE");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add("Payee");
                colValores.Add("TRUE");
                colValores.Add("FALSE");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strNumIdentificacion"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strTipo")} {GetGlobalResource("strCodigo")}");
                BaseAPIClient<CompanyTypeDTO> companyType = new BaseAPIClient<CompanyTypeDTO>(TOKEN_API);
                foreach (var item in companyType.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strContribuyente")} {GetGlobalResource("strCodigo")}");
                BaseAPIClient<TaxpayerTypeDTO> taxPlayerType = new BaseAPIClient<TaxpayerTypeDTO>(TOKEN_API);
                foreach (var item in taxPlayerType.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strTipoNIF")} {GetGlobalResource("strCodigo")}");
                BaseAPIClient<TaxIdentificationNumberCategoryDTO> taxIdentaficationCategory = new BaseAPIClient<TaxIdentificationNumberCategoryDTO>(TOKEN_API);
                foreach (var item in taxIdentaficationCategory.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strCondicionPago")} {GetGlobalResource("strCodigo")}");
                BaseAPIClient<PaymentTermDTO> paymentTerm = new BaseAPIClient<PaymentTermDTO>(TOKEN_API);
                foreach (var item in paymentTerm.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strMoneda")} {GetGlobalResource("strCodigo")}");
                BaseAPIClient<CurrencyDTO> currency = new BaseAPIClient<CurrencyDTO>(TOKEN_API);
                foreach (var item in currency.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strEntidades") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                #region BANK ACCOUNT

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region HEADER ROW

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add($"{GetGlobalResource("strEntidades")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strBancos")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strCodigo"));
                filaCabecera.Add(GetGlobalResource("strIBAN"));
                filaCabecera.Add(GetGlobalResource("strDescripcion"));
                filaCabecera.Add(GetGlobalResource("strSWIFT"));

                #endregion

                #region REQUIRED

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("False");

                #endregion

                #region DATA TYPE

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));

                #endregion

                #region FORMAT

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region MAX VALUE

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region MIN VALUE

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region REGEX

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, "Bank Account", Listas, cont);
                cont++;

                #region VALUES

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strEntidades")} {GetGlobalResource("strCodigo")}");
                BaseAPIClient<CompanyDTO> company = new BaseAPIClient<CompanyDTO>(TOKEN_API);
                foreach (var item in company.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strBancos")} {GetGlobalResource("strCodigo")}");
                BaseAPIClient<BankDTO> bank = new BaseAPIClient<BankDTO>(TOKEN_API);
                foreach (var item in bank.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigo"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strIBAN"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strDescripcion"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strSWIFT"));
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, "Bank Account  " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                #region ADDRESS

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region HEADER ROW

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add($"{GetGlobalResource("strEntidades")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strCodigo"));
                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add(GetGlobalResource("strDefecto"));
                filaCabecera.Add(GetGlobalResource("strDireccion") + "1");
                filaCabecera.Add(GetGlobalResource("strDireccion") + "2");
                filaCabecera.Add(GetGlobalResource("strCodigoPostal"));
                filaCabecera.Add("Locality");
                filaCabecera.Add("Sublocality");
                filaCabecera.Add($"{GetGlobalResource("strPaises")}:{GetGlobalResource("strNombre")}");

                #endregion

                #region REQUIRED

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("False");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");

                #endregion

                #region DATA TYPE

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strBooleano"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));

                #endregion

                #region FORMAT

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region MAX VALUE

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region MIN VALUE

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region REGEX

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strDirecciones"), Listas, cont);
                cont++;

                #region VALUES

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigo"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strNombre"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strDefecto"));
                colValores.Add("TRUE");
                colValores.Add("FALSE");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strDireccion") + "1");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strDireccion") + "2");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigoPostal"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add("Locality");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add("Sublocality");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strPaises")} {GetGlobalResource("strNombre")}");
                BaseAPIClient<CountryDTO> country = new BaseAPIClient<CountryDTO>(TOKEN_API);
                foreach (var item in country.GetList().Result.Value)
                {
                    colValores.Add(item.Name);
                }
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strDirecciones") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                #region PAYMENT METHODS

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region HEADER ROW

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add($"{GetGlobalResource("strEntidades")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strMetodoPago")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strDefecto"));

                #endregion

                #region REQUIRED

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");

                #endregion

                #region DATA TYPE

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strBooleano"));

                #endregion

                #region FORMAT

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region MAX VALUE

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region MIN VALUE

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region REGEX

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strMetodoPago"), Listas, cont);
                cont++;

                #region VALUES

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strEntidades")} {GetGlobalResource("strCodigo")}");
                foreach (var item in company.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strMetodosPagos")} {GetGlobalResource("strCodigo")}");
                BaseAPIClient<PaymentMethodsDTO> paymentMethods = new BaseAPIClient<PaymentMethodsDTO>(TOKEN_API);
                foreach (var item in paymentMethods.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strDefecto"));
                colValores.Add("TRUE");
                colValores.Add("FALSE");
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strMetodosPagos") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

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

        [DirectMethod]
        public DirectResponse ExportarModeloEntidades()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloDatosEntidades") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta1.SetValue(saveAs);

                #region CLIENTEID
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                List<string> filaCabecera;
                List<List<string>> Listas;

                int cont = 0;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region COMPANIES

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add(GetGlobalResource("strCodigo"));
                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add("Alias");
                filaCabecera.Add(GetGlobalResource("strEmail"));
                filaCabecera.Add(GetGlobalResource("strTelefono"));
                filaCabecera.Add(GetGlobalResource("strActivo"));
                filaCabecera.Add(GetGlobalResource("strEntidadPropietario"));
                filaCabecera.Add("Supplier");
                filaCabecera.Add(GetGlobalResource("strEsCliente"));
                filaCabecera.Add("Payee");
                filaCabecera.Add("TaxIdentificationNumber");
                filaCabecera.Add($"{GetGlobalResource("strEntidadesTipos")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strContribuyente")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strTipoNIF")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strCondicionPago")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strMoneda")}:{GetGlobalResource("strCodigo")}");

                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strEntidades"), Listas, cont);
                cont++;

                #endregion

                #region BANK ACCOUNT

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add($"{GetGlobalResource("strEntidades")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strBancos")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strCodigo"));
                filaCabecera.Add(GetGlobalResource("strIBAN"));
                filaCabecera.Add(GetGlobalResource("strDescripcion"));
                filaCabecera.Add(GetGlobalResource("strSWIFT"));

                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strCuenta"), Listas, cont);
                cont++;

                #endregion

                #region ADDRESS

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add($"{GetGlobalResource("strEntidades")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strCodigo"));
                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add(GetGlobalResource("strDefecto"));
                filaCabecera.Add(GetGlobalResource("strDireccion") + "1");
                filaCabecera.Add(GetGlobalResource("strDireccion") + "2");
                filaCabecera.Add(GetGlobalResource("strCodigoPostal"));
                filaCabecera.Add("Locality");
                filaCabecera.Add("Sublocality");
                filaCabecera.Add($"{GetGlobalResource("strPaises")}:{GetGlobalResource("strNombre")}");

                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strDirecciones"), Listas, cont);
                cont++;

                #endregion

                #region PAYMENT METHODS

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add($"{GetGlobalResource("strEntidades")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strMetodoPago")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strDefecto"));

                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strMetodoPago"), Listas, cont);
                cont++;

                #endregion

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

        #endregion

        #region CONTRACTS

        [DirectMethod]
        public DirectResponse ExportarAyudaModeloContracts()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloAyudaDatosContracts") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta2.SetValue(saveAs);

                #region CLIENTEID
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                int cont = 0;

                List<string> filaCabecera;
                List<string> filaRequerido;
                List<string> filaTipoDato;
                List<string> filaFormato;
                List<string> filaMaxValue;
                List<string> filaMinValue;
                List<string> filaRegex;

                List<List<string>> Listas;

                List<List<string>> ColumnasValores;
                List<string> colValores;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region CONTRACTS

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region HEADER ROW

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strCodigo"));
                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add(GetGlobalResource("strDescripcion"));
                filaCabecera.Add(GetGlobalResource("strCodigoEmplazamiento"));
                filaCabecera.Add($"{GetGlobalResource("strEstado")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strMoneda")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strTipoContratacion")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strTiposContratos")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strContratoMarco")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strFechaEjecucion"));
                filaCabecera.Add(GetGlobalResource("jsFechaInicio"));
                filaCabecera.Add(GetGlobalResource("jsFechaFin"));
                //filaCabecera.Add(GetGlobalResource("strDuracion"));
                filaCabecera.Add(GetGlobalResource("strCerradoExpirar"));
                filaCabecera.Add(GetGlobalResource("strTipoProrroga"));
                filaCabecera.Add(GetGlobalResource("strFrecuencia"));
                filaCabecera.Add(GetGlobalResource("strNumeroProrrogas"));
                //filaCabecera.Add(GetGlobalResource("strNumeroRenovacion"));
                filaCabecera.Add(GetGlobalResource("strDiasNotificacionProrroga"));
                // filaCabecera.Add(GetGlobalResource("strFechaRenovacion"));
                filaCabecera.Add(GetGlobalResource("strFechaNotificacionProrroga"));
                // filaCabecera.Add(GetGlobalResource("strFechaAlta"));


                #endregion

                #region REQUIRED

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("False");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                //filaRequerido.Add("False");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                // filaRequerido.Add("False");
                filaRequerido.Add("False");
                //filaRequerido.Add("False");
                filaRequerido.Add("False");
                // filaRequerido.Add("False");



                #endregion

                #region DATA TYPE

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));
                // filaTipoDato.Add(GetGlobalResource("strNumerico"));
                filaTipoDato.Add(GetGlobalResource("strBooleano"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));
                //filaTipoDato.Add(GetGlobalResource("strNumerico"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));
                //filaTipoDato.Add(GetGlobalResource("strFecha"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));
                //filaTipoDato.Add(GetGlobalResource("strFecha"));



                #endregion

                #region FORMAT

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaTipoDato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                // filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                //filaFormato.Add("");
                filaFormato.Add("");
                // filaFormato.Add("");
                filaFormato.Add("");
                //filaFormato.Add("");


                #endregion

                #region MAX VALUE

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaTipoDato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                //filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                //filaFormato.Add("");
                filaFormato.Add("");
                //filaFormato.Add("");
                filaFormato.Add("");
                // filaFormato.Add("");

                #endregion

                #region MIN VALUE

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaTipoDato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                // filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                // filaFormato.Add("");
                filaFormato.Add("");
                // filaFormato.Add("");
                filaFormato.Add("");
                //filaFormato.Add("");

                #endregion

                #region REGEX

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaTipoDato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                // filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                //filaFormato.Add("");
                filaFormato.Add("");
                // filaFormato.Add("");
                filaFormato.Add("");
                //filaFormato.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strContracts"), Listas, cont);
                cont++;

                #region VALUES

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strNumContrato"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strNombreContratos"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strDescripcion"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigoEmplazamiento"));
                ColumnasValores.Add(colValores);


                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strEstadoAlquiler")} {GetGlobalResource("strCodigo")}");
                BaseAPIClient<ContractStatusDTO> contractStatus = new BaseAPIClient<ContractStatusDTO>(TOKEN_API);
                foreach (var item in contractStatus.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strMoneda")} {GetGlobalResource("strCodigo")}");
                BaseAPIClient<CurrencyDTO> currency = new BaseAPIClient<CurrencyDTO>(TOKEN_API);
                foreach (var item in currency.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strTipoContratacion")}:{GetGlobalResource("strCodigo")}");
                BaseAPIClient<ContractGroupDTO> contractGroup = new BaseAPIClient<ContractGroupDTO>(TOKEN_API);
                foreach (var item in contractGroup.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strTiposContratos")}:{GetGlobalResource("strCodigo")}");
                BaseAPIClient<ContractTypeDTO> contractType = new BaseAPIClient<ContractTypeDTO>(TOKEN_API);
                foreach (var item in contractType.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strContratoMarco")}:{GetGlobalResource("strCodigo")}");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strFechaEjecucion"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("jsFechaInicio"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("jsFechaFin"));
                ColumnasValores.Add(colValores);


                //colValores = new List<string>();
                //colValores.Add(GetGlobalResource("strDuracion"));
                //ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCerradoExpirar"));
                colValores.Add("TRUE");
                colValores.Add("FALSE");
                ColumnasValores.Add(colValores);


                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strTipoProrroga"));
                colValores.Add(RenewalReadjustment.sAuto);
                colValores.Add(RenewalReadjustment.sOptional);
                colValores.Add(RenewalReadjustment.sPreviousNegotiation);

                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strFrecuencia"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strNumeroProrrogas"));
                ColumnasValores.Add(colValores);


                //colValores = new List<string>();
                //colValores.Add(GetGlobalResource("strNumeroRenovacion"));
                //ColumnasValores.Add(colValores);


                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strDiasNotificacionProrroga"));
                ColumnasValores.Add(colValores);


                //colValores = new List<string>();
                //colValores.Add(GetGlobalResource("strFechaRenovacion"));
                //ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strFechaNotificacionProrroga"));
                ColumnasValores.Add(colValores);

                //colValores = new List<string>();
                //colValores.Add(GetGlobalResource("strFechaAlta"));
                //ColumnasValores.Add(colValores);



                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strContracts") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                #region CONTRACTS LINES

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region HEADER ROW

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add($"{GetGlobalResource("strContrato")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("jsLineaContrato")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strDescripcion")}");
                filaCabecera.Add($"{GetGlobalResource("jsLineaContrato")} {GetGlobalResource("strTipo")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("jsFrecuencia")}");
                filaCabecera.Add($"{GetGlobalResource("strValor")}");
                filaCabecera.Add($"{GetGlobalResource("strMonedas")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strValidoDesde")}");
                filaCabecera.Add($"{GetGlobalResource("strValidoHasta")}");
                filaCabecera.Add($"{GetGlobalResource("strFechaProximoPago")}");
                filaCabecera.Add($"{GetGlobalResource("strAplicaRenovacion")}");
                filaCabecera.Add($"{GetGlobalResource("strProrrateo")}");
                filaCabecera.Add($"{GetGlobalResource("strPagoAnticipado")}");
                filaCabecera.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("strTipo")}");
                filaCabecera.Add($"{GetGlobalResource("strInflaciones")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strCantidadFija")}");
                filaCabecera.Add($"{GetGlobalResource("strPorcentajeFijo")}");
                filaCabecera.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("jsFrecuencia")}");
                filaCabecera.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("jsFechaInicio")}");
                filaCabecera.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("strFechaProximaRevision")}");
                filaCabecera.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("jsFechaFin")}");

                #endregion

                #region REQUIRED

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("False");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");

                #endregion

                #region DATA TYPE

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));
                filaTipoDato.Add(GetGlobalResource("strBooleano"));
                filaTipoDato.Add(GetGlobalResource("strBooleano"));
                filaTipoDato.Add(GetGlobalResource("strBooleano"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));

                #endregion

                #region FORMAT

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region MAX VALUE

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region MIN VALUE

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region REGEX

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("jsLineaContrato"), Listas, cont);
                cont++;

                #region VALUES

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strContrato")}:{GetGlobalResource("strCodigo")}");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("jsLineaContrato")}:{GetGlobalResource("strCodigo")}");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strDescripcion"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("jsLineaContrato")}{GetGlobalResource("strTipo")}:{GetGlobalResource("strCodigo")}");
                BaseAPIClient<ContractLineTypeDTO> contractLinesTypes = new BaseAPIClient<ContractLineTypeDTO>(TOKEN_API);
                foreach (var item in contractLinesTypes.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("jsFrecuencia"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strValor"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strMonedas")}  {GetGlobalResource("strCodigo")}");
                foreach (var item in currency.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strValidoDesde"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strValidoHasta"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strFechaProximoPago"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strAplicaRenovacion"));
                colValores.Add("TRUE");
                colValores.Add("FALSE");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strProrrateo"));
                colValores.Add("TRUE");
                colValores.Add("FALSE");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strPagoAnticipado"));
                colValores.Add("TRUE");
                colValores.Add("FALSE");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("strTipo")}");
                colValores.Add(PReadjustment.sPCI);
                colValores.Add(PReadjustment.sFixedAmount);
                colValores.Add(PReadjustment.sFixedPercentege);
                colValores.Add(PReadjustment.sWithoutIncrements);
                ColumnasValores.Add(colValores);


                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strInflaciones")} {GetGlobalResource("strCodigo")}");
                BaseAPIClient<InflationDTO> inflation = new BaseAPIClient<InflationDTO>(TOKEN_API);
                foreach (var item in inflation.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCantidadFija"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strPorcentajeFijo"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("jsFrecuencia"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("strFechaInicio")}");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("strFechaProximaRevision")}");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("jsFechaFin")}");
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("jsLineaContrato") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                #region CONTRACTS LINES TAXES

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region HEADER ROW

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add($"{GetGlobalResource("strContratos")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("jsLineaContrato")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strImpuesto")}:{GetGlobalResource("strCodigo")}");

                #endregion

                #region REQUIRED

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");

                #endregion

                #region DATA TYPE

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));

                #endregion

                #region FORMAT

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region MAX VALUE

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region MIN VALUE

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region REGEX

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("jsLineaContrato") + " " + GetGlobalResource("strImpuestos"), Listas, cont);
                cont++;

                #region VALUES

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strContratos"));
                ColumnasValores.Add(colValores);


                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("jsLineaContrato")}  {GetGlobalResource("strCodigo")}");
                ColumnasValores.Add(colValores);



                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strImpuesto")} {GetGlobalResource("strCodigo")}");
                BaseAPIClient<TaxesDTO> tax = new BaseAPIClient<TaxesDTO>(TOKEN_API);
                foreach (var item in tax.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("jsLineaContrato") + " " + GetGlobalResource("strImpuestos") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                #region CONTRACTS LINES COMPANIES

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region HEADER ROW

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add($"{GetGlobalResource("strContratos")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("jsLineaContrato")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strEntidades")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strMetodosPagos")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strBankAccounts")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strMonedas")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strPorcentaje"));

                #endregion

                #region REQUIRED

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("False");
                filaRequerido.Add("True");
                filaRequerido.Add("True");

                #endregion

                #region DATA TYPE

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));

                #endregion

                #region FORMAT

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region MAX VALUE

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region MIN VALUE

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region REGEX

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strPayees"), Listas, cont);
                cont++;

                #region VALUES

                colValores = new List<string>();
                List<string> colBancos = new List<string>();
                List<string> colMetodosPagos = new List<string>();
                colValores.Add(GetGlobalResource("strContratos"));
                ColumnasValores.Add(colValores);


                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("jsLineaContrato")}  {GetGlobalResource("strCodigo")}");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strEntidades")}  {GetGlobalResource("strCodigo")}");
                BaseAPIClient<CompanyDTO> company = new BaseAPIClient<CompanyDTO>(TOKEN_API);
                colMetodosPagos.Add($"{GetGlobalResource("strMetodosPagos")}  {GetGlobalResource("strCodigo")}");
                colBancos.Add($"{GetGlobalResource("strBankAccounts")}  {GetGlobalResource("strCodigo")}");
                foreach (var item in company.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                    string banco = "";
                    if (item.LinkedBankAccount != null)
                    {
                        foreach (var b in item.LinkedBankAccount)
                        {
                            banco = b.Code + ";" + banco;

                        }
                        if (banco != "")
                        {
                            banco = banco.TrimEnd(';');
                        }

                    }

                    colBancos.Add(banco);
                    string metodopago = "";
                    if (item.LinkedPaymentMethodCode != null)
                    {
                        foreach (var m in item.LinkedPaymentMethodCode)
                        {

                            metodopago = m.PaymentMethodCode + ";" + metodopago;
                        }
                        if (metodopago != "")
                        {
                            metodopago = metodopago.TrimEnd(';');
                        }


                    }
                    colMetodosPagos.Add(metodopago);
                }
                ColumnasValores.Add(colValores);
                ColumnasValores.Add(colMetodosPagos);
                ColumnasValores.Add(colBancos);

                colValores = new List<string>();
                //colValores.Add($"{GetGlobalResource("strMetodosPagos")}  {GetGlobalResource("strCodigo")}");
                //BaseAPIClient<PaymentMethodsDTO> paymentMethod = new BaseAPIClient<PaymentMethodsDTO>(TOKEN_API);
                //foreach (var item in paymentMethod.GetList().Result.Value)
                //{
                //    colValores.Add(item.Code);
                //}

                //colValores = new List<string>();
                //colValores.Add($"{GetGlobalResource("strBankAccounts")}  {GetGlobalResource("strCodigo")}");



                colValores = new List<string>();
                colValores.Add($"{GetGlobalResource("strMonedas")}  {GetGlobalResource("strCodigo")}");
                foreach (var item in currency.GetList().Result.Value)
                {
                    colValores.Add(item.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strPorcentaje"));
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strPayees") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

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
        [DirectMethod]
        public DirectResponse ExportarModeloContracts()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = "CONTRACTS_DATAMODEL_" + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta1.SetValue(saveAs);

                #region CLIENTEID
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                List<string> filaCabecera;
                List<List<string>> Listas;

                int cont = 0;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region CONTRACTS

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add(GetGlobalResource("strNumContrato"));
                filaCabecera.Add(GetGlobalResource("strNombreContratos"));
                filaCabecera.Add(GetGlobalResource("strDescripcion"));
                filaCabecera.Add(GetGlobalResource("strCodigoEmplazamiento"));
                filaCabecera.Add($"{GetGlobalResource("strEstadoAlquiler")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strMoneda")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strTipoContratacion")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strTiposContratos")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strContratoMarco")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strFechaEjecucion"));
                filaCabecera.Add(GetGlobalResource("jsFechaInicio"));
                filaCabecera.Add(GetGlobalResource("jsFechaFin"));
                // filaCabecera.Add(GetGlobalResource("strDuracion"));
                filaCabecera.Add(GetGlobalResource("strCerradoExpirar"));
                filaCabecera.Add(GetGlobalResource("strTipoProrroga"));
                filaCabecera.Add(GetGlobalResource("strFrecuencia"));
                filaCabecera.Add(GetGlobalResource("strNumeroProrrogas"));
                // filaCabecera.Add(GetGlobalResource("strNumeroRenovacion"));
                filaCabecera.Add(GetGlobalResource("strDiasNotificacionProrroga"));
                // filaCabecera.Add(GetGlobalResource("strFechaRenovacion"));
                filaCabecera.Add(GetGlobalResource("strFechaNotificacionProrroga"));
                // filaCabecera.Add(GetGlobalResource("strFechaAlta"));

                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strContracts"), Listas, cont);
                cont++;

                #endregion

                #region CONTRACTS LINES

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add($"{GetGlobalResource("strContrato")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("jsLineaContrato")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strDescripcion"));
                filaCabecera.Add($"{GetGlobalResource("jsLineaContrato")} {GetGlobalResource("strTipo")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strFrecuencia"));
                filaCabecera.Add($"{GetGlobalResource("strValor")}");
                filaCabecera.Add($"{GetGlobalResource("strMoneda")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strValidoDesde"));
                filaCabecera.Add(GetGlobalResource("strValidoHasta"));
                filaCabecera.Add(GetGlobalResource("strFechaProximoPago"));
                filaCabecera.Add(GetGlobalResource("strAplicaRenovacion"));
                filaCabecera.Add(GetGlobalResource("strProrrateo"));
                filaCabecera.Add(GetGlobalResource("strPagoAnticipado"));
                filaCabecera.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("strTipo")}");
                filaCabecera.Add(GetGlobalResource("strInflacion"));
                filaCabecera.Add(GetGlobalResource("strCantidadFija"));
                filaCabecera.Add(GetGlobalResource("jsPorcentaje"));
                filaCabecera.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("jsFrecuencia")}");
                filaCabecera.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("jsFechaInicio")}");
                filaCabecera.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("strFechaProximaRevision")}");
                filaCabecera.Add($"{GetGlobalResource("strReajustes")}:{GetGlobalResource("jsFechaFin")}");



                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("jsLineaContrato"), Listas, cont);
                cont++;

                #endregion

                #region CONTRACT LINE TAXES

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add(GetGlobalResource("strNumContrato"));
                filaCabecera.Add($"{GetGlobalResource("jsLineaContrato")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strImpuesto")}:{GetGlobalResource("strCodigo")}");

                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("jsLineaContrato") + " " + GetGlobalResource("strImpuestos"), Listas, cont);
                cont++;

                #endregion


                #region CONTRACT LINE COMPANIES

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add(GetGlobalResource("strNumContrato"));
                filaCabecera.Add($"{GetGlobalResource("jsLineaContrato")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strEntidades")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strMetodosPagos")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strBankAccounts")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add($"{GetGlobalResource("strMoneda")}:{GetGlobalResource("strCodigo")}");
                filaCabecera.Add(GetGlobalResource("strPorcentaje"));



                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strPayees"), Listas, cont);
                cont++;

                #endregion



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

        #endregion

        #region DOCUMENTOS

        [DirectMethod]
        public DirectResponse GenerarPlantillaModeloDocumental()
        {
            DirectResponse direct = new DirectResponse();
            DocumentosController cDocumentos = new DocumentosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloCargaDocumento") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta1.SetValue(saveAs);
                string ValoresPosibles = string.Join(";", cDocumentos.ValoresDocumentosTipos());

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region Datos

                List<string> filaCabecera = new List<string>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strObjetoNegocio"));
                filaCabecera.Add(GetGlobalResource("strIdentificadorUnico"));
                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add(GetGlobalResource("strArchivo"));
                filaCabecera.Add(GetGlobalResource("strExtension"));
                filaCabecera.Add(GetGlobalResource("strTipo"));
                filaCabecera.Add(GetGlobalResource("strEstado"));
                //filaCabecera.Add(GetGlobalResource("strDocumentoInicial"));

                #endregion

                #endregion

                List<List<string>> Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strLocalizacion"), Listas, 0);

                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        [DirectMethod]
        public DirectResponse GenerarPlantillaAyudaModeloDocumental()
        {
            DirectResponse direct = new DirectResponse();
            DocumentosController cDocumentos = new DocumentosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloAyudaCargaDocumento") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta2.SetValue(saveAs);

                #region CLIENTEID
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                int cont = 0;

                List<string> filaCabecera;
                List<string> filaRequerido;
                List<string> filaTipoDato;
                List<string> filaFormato;
                List<string> filaMaxValue;
                List<string> filaMinValue;
                List<string> filaRegex;

                List<List<string>> Listas;

                List<List<string>> ColumnasValores;
                List<string> colValores;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region DOCUMENTAL

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strObjetoNegocio"));
                filaCabecera.Add(GetGlobalResource("strIdentificadorUnico"));
                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add(GetGlobalResource("strArchivo"));
                filaCabecera.Add(GetGlobalResource("strExtension"));
                filaCabecera.Add(GetGlobalResource("strTipo"));
                filaCabecera.Add(GetGlobalResource("strEstado"));
                //filaCabecera.Add(GetGlobalResource("strDocumentoInicial"));

                #endregion

                #region Reqerido

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                //filaRequerido.Add("False");

                #endregion

                #region Fila Tipos de datos

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                //filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));

                #endregion

                #region Formato

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                //filaFormato.Add("");

                #endregion

                #region Max Value

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                //filaMaxValue.Add("");

                #endregion

                #region Min Value

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                //filaMinValue.Add("");

                #endregion

                #region Regex

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                //filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strLocalizacion"), Listas, cont);
                cont++;

                #region Values

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCampo"));
                ColumnasValores.Add(colValores);


                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strObjetoNegocio"));
                colValores.Add("SITE");
                colValores.Add("INVENTORY_ELEMENT");
                colValores.Add("USER");
                colValores.Add("COMPANY");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strIdentificadorUnico"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strNombre"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strArchivo"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strExtension"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strTipo"));
                DocumentTiposController cTipos = new DocumentTiposController();
                foreach (var item in cTipos.GetActivos(ClienteID.Value))
                {
                    colValores.Add(item.DocumentTipo);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strEstado"));
                DocumentosEstadosController cDocEstados = new DocumentosEstadosController();
                foreach (var item in cDocEstados.GetActivos(ClienteID.Value))
                {
                    colValores.Add(item.Nombre);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strDocumentoInicial"));
                //ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strLocalizacion") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        #endregion

        #region EMPLAZAMIENTOS

        [DirectMethod]
        public DirectResponse ExportarModeloEmplazamiento()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloDatosEmplazamientos") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta1.SetValue(saveAs);

                List<string> lTabs = new List<string>();
                lTabs.Add(GetGlobalResource("strEmplazamientos"));
                //lTabs.Add(GetGlobalResource("strContactos"));

                #region Datos Emplazamientos
                List<string> filaTipoDatoEmplazamientos = new List<string>();
                List<string> filaValoresPosiblesEmplazamientos = new List<string>();
                List<string> filaCabeceraEmplazamientos = new List<string>();

                #region Fila Tipos de datos
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strFecha"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strFecha"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strNumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strNumerico"));
                #endregion

                #region Fila Valores Posibles
                filaValoresPosiblesEmplazamientos = GetValoresPosibles();
                #endregion

                #region Fila Cabecera
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strEmplazamientoCodigo"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strNombreSitio"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strOperador"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strMoneda"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strEstadoGlobal"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strCategoriaSitio"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strTiposEmplazamientos"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strTipoEdificio"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strTipoEstructura"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strEmplazamientosTamanos"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strFechaActivacion"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strFechaDesactivacion"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strMunicipio"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strBarrio"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strDireccion"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strCodigoPostal"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strLatitud"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strLongitud"));
                #endregion

                #region Datos Dinamicos

                List<EmplazamientosAtributosConfiguracion> listaAtributos;
                EmplazamientosAtributosConfiguracionController cAtributos = new EmplazamientosAtributosConfiguracionController();
                listaAtributos = cAtributos.GetAtributosFromCliente(long.Parse(hdCliID.Value.ToString()));
                if (listaAtributos != null)
                {
                    foreach (var oAtr in listaAtributos)
                    {
                        filaCabeceraEmplazamientos.Add(oAtr.NombreAtributo);
                        filaTipoDatoEmplazamientos.Add(oAtr.TiposDatos.TipoDato);

                        #region Valore Posibles

                        if (oAtr.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Lista.ToUpper() || oAtr.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.ListaMultiple.ToUpper())
                        {
                            if (oAtr.TablaModeloDatoID != null)
                            {
                                EmplazamientosAtributosConfiguracionController cInventarioComponentes = new EmplazamientosAtributosConfiguracionController();
                                string valoresCombo = cInventarioComponentes.GetValoresStringComboBoxByColumnaModeloDatosID(oAtr.EmplazamientoAtributoConfiguracionID);

                                filaValoresPosiblesEmplazamientos.Add(valoresCombo);
                            }
                            else
                            {
                                filaValoresPosiblesEmplazamientos.Add(oAtr.ValoresPosibles);
                            }
                        }
                        else if (oAtr.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Boolean.ToUpper())
                        {
                            filaValoresPosiblesEmplazamientos.Add("True;False");
                        }
                        else if (oAtr.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Fecha.ToUpper())
                        {
                            filaValoresPosiblesEmplazamientos.Add("yyyyMMdd");
                        }
                        else
                        {
                            filaValoresPosiblesEmplazamientos.Add("");
                        }

                        #endregion

                    }
                }

                #endregion

                #endregion

                #region Datos Contactos
                List<string> filaTipoDato = new List<string>();
                List<string> filaValoresPosibles = new List<string>();
                List<string> filaCabecera = new List<string>();

                #region Fila Tipos de datos
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                #endregion

                #region Fila Valores Posibles
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
                #endregion

                #region Fila Cabecera
                filaCabecera.Add(GetGlobalResource("strEmplazamientoCodigo") + "*");
                filaCabecera.Add(GetGlobalResource("strEmail") + "*");
                filaCabecera.Add(GetGlobalResource("strTelefono") + "*");
                #endregion

                #endregion

                long cont = 1;

                foreach (string elem in lTabs)
                {
                    if (cont == 1)
                    {
                        cEmplazamientos.ExportarModeloDatos(saveAs, elem, filaTipoDatoEmplazamientos, filaValoresPosiblesEmplazamientos, filaCabeceraEmplazamientos);
                    }
                    else
                    {
                        //cEmplazamientos.ExportarModeloDatosOpen(saveAs, elem, filaTipoDato, filaValoresPosibles, filaCabecera, cont);
                    }
                    cont++;
                }

                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        [DirectMethod]
        public DirectResponse ExportarAyudaModeloEmplazamiento()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloAyudaDatosEmplazamientos") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta2.SetValue(saveAs);

                List<string> lTabs = new List<string>();
                lTabs.Add(GetGlobalResource("strFormato"));
                lTabs.Add(GetGlobalResource("strValor"));

                #region Datos Emplazamientos

                List<string> filaCabeceraEmplazamientos = new List<string>();
                List<string> filaRequeridoEmplazamientos = new List<string>();
                List<string> filaTipoDatoEmplazamientos = new List<string>();
                List<string> filaFormatoEmplazamientos = new List<string>();
                List<string> filaMaxValueEmplazamientos = new List<string>();
                List<string> filaMinValueEmplazamientos = new List<string>();
                List<string> filaRegexEmplazamientos = new List<string>();

                List<List<string>> ColumnasValoresEmplazamientos = new List<List<string>>();
                List<string> colValoresEmplazamientos;

                #region Fila Cabecera
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strCampo"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strEmplazamientoCodigo"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strNombreSitio"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strOperador"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strMoneda"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strEstadoGlobal"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strCategoriaSitio"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strTiposEmplazamientos"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strTipoEdificio"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strTipoEstructura"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strEmplazamientosTamanos"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strFechaActivacion"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strFechaDesactivacion"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strMunicipio"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strBarrio"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strDireccion"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strCodigoPostal"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strLatitud"));
                filaCabeceraEmplazamientos.Add(GetGlobalResource("strLongitud"));
                #endregion

                #region Reqerido

                filaRequeridoEmplazamientos.Add(GetGlobalResource("strObligatorio"));
                filaRequeridoEmplazamientos.Add("True");
                filaRequeridoEmplazamientos.Add("True");
                filaRequeridoEmplazamientos.Add("True");
                filaRequeridoEmplazamientos.Add("True");
                filaRequeridoEmplazamientos.Add("True");
                filaRequeridoEmplazamientos.Add("True");
                filaRequeridoEmplazamientos.Add("True");
                filaRequeridoEmplazamientos.Add("True");
                filaRequeridoEmplazamientos.Add("False");
                filaRequeridoEmplazamientos.Add("False");
                filaRequeridoEmplazamientos.Add("True");
                filaRequeridoEmplazamientos.Add("False");
                filaRequeridoEmplazamientos.Add("True");
                filaRequeridoEmplazamientos.Add("False");
                filaRequeridoEmplazamientos.Add("True");
                filaRequeridoEmplazamientos.Add("False");
                filaRequeridoEmplazamientos.Add("False");
                filaRequeridoEmplazamientos.Add("False");

                #endregion

                #region Fila Tipos de datos

                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strTipoDato"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strFecha"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strFecha"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("jsLista"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strNumerico"));
                filaTipoDatoEmplazamientos.Add(GetGlobalResource("strNumerico"));

                #endregion

                #region Formato

                filaFormatoEmplazamientos.Add(GetGlobalResource("strFormato"));
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("yyyyMMdd");
                filaFormatoEmplazamientos.Add("yyyyMMdd");
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("");
                filaFormatoEmplazamientos.Add("00,000000");
                filaFormatoEmplazamientos.Add("00,000000");

                #endregion

                #region Max Value

                filaMaxValueEmplazamientos.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("");
                filaMaxValueEmplazamientos.Add("+90,000000");
                filaMaxValueEmplazamientos.Add("+180,000000");

                #endregion

                #region Min Value

                filaMinValueEmplazamientos.Add(GetGlobalResource("strValorMinimo"));
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("");
                filaMinValueEmplazamientos.Add("-90,000000");
                filaMinValueEmplazamientos.Add("-180,000000");

                #endregion

                #region Regex

                filaRegexEmplazamientos.Add(GetGlobalResource("strRegex"));
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");
                filaRegexEmplazamientos.Add("");

                #endregion

                //#region Fila Valores Posibles
                //filaValoresPosiblesEmplazamientos = GetValoresPosibles();
                //#endregion

                #region Valores Posibles

                long cliID = 0;

                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else
                {
                    if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());
                    }
                }

                EntidadesController cOperadores = new EntidadesController();
                MonedasController cMonedas = new MonedasController();
                EstadosGlobalesController cEstadosGlobales = new EstadosGlobalesController();
                EmplazamientosCategoriasSitiosController cCategorias = new EmplazamientosCategoriasSitiosController();
                EmplazamientosTiposController cTipos = new EmplazamientosTiposController();
                EmplazamientosTiposEdificiosController cEdificios = new EmplazamientosTiposEdificiosController();
                EmplazamientosTiposEstructurasController cEstructuras = new EmplazamientosTiposEstructurasController();
                EmplazamientosTamanosController cTamanos = new EmplazamientosTamanosController();
                MunicipiosController cMunicipios = new MunicipiosController();

                colValoresEmplazamientos = new List<string>();
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strEmplazamientoCodigo"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = new List<string>();
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strNombreSitio"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = cOperadores.getEntidadesOperadores(cliID).Select(c => c.Codigo).ToList();
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strOperador"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = cMonedas.GetMonedasCodigo(cliID);
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strMoneda"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = cEstadosGlobales.GetEstadosGlobalesNombre(cliID);
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strEstadoGlobal"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = cCategorias.GetCategoriasNombre(cliID);
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strCategoriaSitio"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = cTipos.GetTiposNombre(cliID);
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strTiposEmplazamientos"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = cEdificios.GetEdificiosNombre(cliID);
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strTipoEdificio"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = cEstructuras.GetEstructurasNombre(cliID);
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strTipoEstructura"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = cTamanos.GetTamanosNombre(cliID);
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strEmplazamientosTamanos"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = new List<string>();
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strFechaActivacion"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = new List<string>();
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strFechaDesactivacion"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = cMunicipios.getCodigosMunicipioCliente(cliID);
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strMunicipio"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = new List<string>();
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strBarrio"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = new List<string>();
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strDireccion"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = new List<string>();
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strCodigoPostal"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = new List<string>();
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strLatitud"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                colValoresEmplazamientos = new List<string>();
                colValoresEmplazamientos.Insert(0, GetGlobalResource("strLongitud"));
                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);

                #endregion

                #region Datos Dinamicos

                List<EmplazamientosAtributosConfiguracion> listaAtributos;
                EmplazamientosAtributosConfiguracionController cAtributos = new EmplazamientosAtributosConfiguracionController();
                listaAtributos = cAtributos.GetAtributosFromCliente(long.Parse(hdCliID.Value.ToString()));
                if (listaAtributos != null)
                {
                    foreach (var oAtr in listaAtributos)
                    {
                        filaCabeceraEmplazamientos.Add(oAtr.CodigoAtributo);
                        filaTipoDatoEmplazamientos.Add(oAtr.TiposDatos.TipoDato);

                        #region Propiedades

                        Vw_EmplazamientosAtributosTiposDatosPropiedades oPropiedades;
                        EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController cPropiedadesAtributos = new EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController();
                        List<Vw_EmplazamientosAtributosTiposDatosPropiedades> listaPropiedades = cPropiedadesAtributos.GetPropiedadesFromAtributo(oAtr.EmplazamientoAtributoConfiguracionID);

                        if (listaPropiedades != null && listaPropiedades.Count > 0)
                        {
                            #region Requerido

                            oPropiedades = null;
                            try
                            {
                                oPropiedades = listaPropiedades.FindAll(prop => prop.Codigo == "AllowBlank").FirstOrDefault();

                                if (oAtr.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_BOOLEAN || (oPropiedades != null && oPropiedades.Valor == "False"))
                                {
                                    filaRequeridoEmplazamientos.Add("True");
                                }
                                else
                                {
                                    filaRequeridoEmplazamientos.Add("False");
                                }
                            }
                            catch (Exception ex)
                            {
                                filaRequeridoEmplazamientos.Add("False");
                            }

                            #endregion

                            #region Max Value

                            oPropiedades = null;

                            try
                            {
                                oPropiedades = listaPropiedades.FindAll(prop => (prop.Codigo == "MaxDate" || prop.Codigo == "MaxValue")).FirstOrDefault();
                                if (oPropiedades != null)
                                {
                                    filaMaxValueEmplazamientos.Add(oPropiedades.Valor);
                                }
                                else
                                {
                                    filaMaxValueEmplazamientos.Add("");
                                }
                            }
                            catch (Exception ex)
                            {
                                filaMaxValueEmplazamientos.Add("");
                            }


                            #endregion

                            #region Min Value

                            oPropiedades = null;
                            try
                            {
                                oPropiedades = listaPropiedades.FindAll(prop => (prop.Codigo == "MinDate" || prop.Codigo == "MinValue")).FirstOrDefault();

                                if (oPropiedades != null)
                                {
                                    filaMinValueEmplazamientos.Add(oPropiedades.Valor);
                                }
                                else
                                {
                                    filaMinValueEmplazamientos.Add("");
                                }
                            }
                            catch (Exception ex)
                            {
                                filaMinValueEmplazamientos.Add("");
                            }

                            #endregion

                            #region Regex

                            oPropiedades = null;
                            try
                            {
                                oPropiedades = listaPropiedades.FindAll(prop => prop.Codigo == "Regex").FirstOrDefault();

                                if (oPropiedades != null)
                                {
                                    filaRegexEmplazamientos.Add(oPropiedades.Valor);
                                }
                                else
                                {
                                    filaRegexEmplazamientos.Add("");
                                }
                            }
                            catch (Exception ex)
                            {
                                filaRegexEmplazamientos.Add("");
                            }

                            #endregion

                            #region Format

                            oPropiedades = null;
                            try
                            {
                                oPropiedades = listaPropiedades.FindAll(prop => prop.Codigo == "AllowDecimals" && prop.Valor == "True").FirstOrDefault();

                                if (oPropiedades != null)
                                {
                                    try
                                    {
                                        oPropiedades = listaPropiedades.FindAll(prop => prop.Codigo == "DecimalPrecision").FirstOrDefault();

                                        if (oPropiedades != null)
                                        {
                                            filaFormatoEmplazamientos.Add("0," + string.Concat(Enumerable.Repeat("0", int.Parse(oPropiedades.Valor))));
                                        }
                                        else
                                        {
                                            filaFormatoEmplazamientos.Add("0,00");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        filaFormatoEmplazamientos.Add("0,00");
                                    }
                                }
                                else
                                {
                                    filaFormatoEmplazamientos.Add("");
                                }
                            }
                            catch (Exception ex)
                            {
                                filaFormatoEmplazamientos.Add("");
                            }

                            #endregion
                        }
                        else
                        {
                            if (oAtr.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_BOOLEAN)
                            {
                                filaRequeridoEmplazamientos.Add("True");
                            }
                            else
                            {
                                filaRequeridoEmplazamientos.Add("False");
                            }
                            filaMaxValueEmplazamientos.Add("");
                            filaFormatoEmplazamientos.Add("");
                            filaMinValueEmplazamientos.Add("");
                            filaRegexEmplazamientos.Add("");
                        }

                        if (oAtr.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_FECHA)
                        {
                            filaFormatoEmplazamientos[filaFormatoEmplazamientos.Count - 1] = "yyyyMMdd";
                        }
                        #endregion

                        #region Valore Posibles

                        if (oAtr.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Lista.ToUpper() || oAtr.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.ListaMultiple.ToUpper())
                        {
                            if (oAtr.TablaModeloDatoID != null)
                            {
                                EmplazamientosAtributosConfiguracionController cInventarioComponentes = new EmplazamientosAtributosConfiguracionController();

                                colValoresEmplazamientos = cInventarioComponentes.GetValoresListStringComboBoxByColumnaModeloDatosID(oAtr.EmplazamientoAtributoConfiguracionID);
                                colValoresEmplazamientos.Insert(0, oAtr.NombreAtributo);
                                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);
                            }
                            else
                            {
                                colValoresEmplazamientos = oAtr.ValoresPosibles.Split(';').ToList();
                                colValoresEmplazamientos.Insert(0, oAtr.NombreAtributo);
                                ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);
                            }
                        }
                        else if (oAtr.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Boolean.ToUpper())
                        {
                            colValoresEmplazamientos = new List<string>();
                            colValoresEmplazamientos.Insert(0, oAtr.NombreAtributo);
                            colValoresEmplazamientos.Add("True");
                            colValoresEmplazamientos.Add("False");
                            ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);
                        }
                        else if (oAtr.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Fecha.ToUpper())
                        {
                            colValoresEmplazamientos = new List<string>();
                            colValoresEmplazamientos.Insert(0, oAtr.NombreAtributo);
                            ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);
                        }
                        else
                        {
                            colValoresEmplazamientos = new List<string>();
                            colValoresEmplazamientos.Insert(0, oAtr.NombreAtributo);
                            ColumnasValoresEmplazamientos.Add(colValoresEmplazamientos);
                        }

                        #endregion

                    }
                }

                #endregion

                List<List<string>> Listas = new List<List<string>>();
                Listas.Add(filaCabeceraEmplazamientos);
                Listas.Add(filaRequeridoEmplazamientos);
                Listas.Add(filaTipoDatoEmplazamientos);
                Listas.Add(filaFormatoEmplazamientos);
                Listas.Add(filaMaxValueEmplazamientos);
                Listas.Add(filaMinValueEmplazamientos);
                Listas.Add(filaRegexEmplazamientos);

                #endregion

                //#region Datos Contactos
                //List<string> filaTipoDato = new List<string>();
                //List<string> filaValoresPosibles = new List<string>();
                //List<string> filaCabecera = new List<string>();

                //#region Fila Tipos de datos
                //filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                //filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                //filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                //#endregion

                //#region Fila Valores Posibles
                //filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                //filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
                //filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
                //#endregion

                //#region Fila Cabecera
                //filaCabecera.Add(GetGlobalResource("strEmplazamientoCodigo") + "*");
                //filaCabecera.Add(GetGlobalResource("strEmail") + "*");
                //filaCabecera.Add(GetGlobalResource("strTelefono") + "*");
                //#endregion

                //#endregion

                long cont = 1;

                //DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                foreach (string elem in lTabs)
                {
                    if (cont == 1)
                    {
                        Comun.ExportarModeloDatosDinamicoFilas(saveAs, elem, Listas, cont);
                    }
                    else
                    {
                        Comun.ExportarModeloDatosDinamicoColumnas(saveAs, elem, ColumnasValoresEmplazamientos, cont);
                    }
                    cont++;
                }

                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        [DirectMethod]
        public DirectResponse ExportarModeloEmpContactos()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strPlantillaEmpContactos") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta1.SetValue(saveAs);
                string tab = GetGlobalResource("strContactos");

                long cont = 0;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }


                List<string> filaCabecera;

                List<List<string>> Listas;

                #region CONTACTOS

                filaCabecera = new List<string>();

                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add(GetGlobalResource("strApellidos"));
                filaCabecera.Add(GetGlobalResource("strEmail"));
                filaCabecera.Add(GetGlobalResource("strPrefijo"));
                filaCabecera.Add(GetGlobalResource("strTelefono"));
                filaCabecera.Add(GetGlobalResource("strPrefijo"));
                filaCabecera.Add(GetGlobalResource("strTelefono2"));
                filaCabecera.Add(GetGlobalResource("strTipoContacto"));
                filaCabecera.Add(GetGlobalResource("strMunicipio"));
                filaCabecera.Add(GetGlobalResource("strCodigoPostal"));
                filaCabecera.Add(GetGlobalResource("strDireccion"));
                filaCabecera.Add(GetGlobalResource("strComentarios"));

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);

                #endregion

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strContactos"), Listas, cont);
                cont++;

                #region CONTACTOS-EMPLAZAMIENTOS

                filaCabecera = new List<string>();

                filaCabecera.Add(GetGlobalResource("strCodigoEmplazamiento"));
                filaCabecera.Add(GetGlobalResource("strEmailContacto"));

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);

                #endregion

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strEmplazamientos") + " " + GetGlobalResource("strContactos"), Listas, cont);
                cont++;

                #region CONTACTOS-ENTIDADES

                filaCabecera = new List<string>();

                filaCabecera.Add(GetGlobalResource("strCodigo"));
                filaCabecera.Add(GetGlobalResource("strEmailContacto"));

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);

                #endregion

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strEntidades") + " " + GetGlobalResource("strContactos"), Listas, cont);
                cont++;


                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        [DirectMethod]
        public DirectResponse ExportarAyudaModeloEmpContactos()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strPlantillaAyudaEmpContactos") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta2.SetValue(saveAs);
                string tab = GetGlobalResource("strContactos");

                #region CLIENTEID
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                int cont = 0;

                List<string> filaCabecera;
                List<string> filaRequerido;
                List<string> filaTipoDato;
                List<string> filaFormato;
                List<string> filaMaxValue;
                List<string> filaMinValue;
                List<string> filaRegex;

                List<List<string>> Listas;

                List<List<string>> ColumnasValores;
                List<string> colValores;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region CONTACTOS

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add(GetGlobalResource("strApellidos"));
                filaCabecera.Add(GetGlobalResource("strEmail"));
                filaCabecera.Add(GetGlobalResource("strPrefijo"));
                filaCabecera.Add(GetGlobalResource("strTelefono"));
                filaCabecera.Add(GetGlobalResource("strPrefijo"));
                filaCabecera.Add(GetGlobalResource("strTelefono2"));
                filaCabecera.Add(GetGlobalResource("strTipoContacto"));
                filaCabecera.Add(GetGlobalResource("strMunicipio"));
                filaCabecera.Add(GetGlobalResource("strCodigoPostal"));
                filaCabecera.Add(GetGlobalResource("strDireccion"));
                filaCabecera.Add(GetGlobalResource("strComentarios"));

                #endregion

                #region Reqerido

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");

                #endregion

                #region Fila Tipos de datos

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));

                #endregion

                #region Formato

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region Max Value

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region Min Value

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region Regex

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strContactos"), Listas, cont);
                cont++;

                #region Values

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strNombre"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strApellidos"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strEmail"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                PaisesController cPaises = new PaisesController();
                colValores.Add(GetGlobalResource("strPrefijo"));
                colValores.AddRange(cPaises.GetActivosConPrefijo(cliID).Select(c => c.Prefijo).ToList());
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strTelefono"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strPrefijo"));
                colValores.AddRange(cPaises.GetActivosConPrefijo(cliID).Select(c => c.Prefijo).ToList());
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strTelefono2"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strTipoContacto"));
                ContactosTiposController cContactosTipos = new ContactosTiposController();
                colValores.AddRange(cContactosTipos.ListContactosTipos());
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strMunicipio"));
                MunicipiosController cMunicipios = new MunicipiosController();
                colValores.AddRange(cMunicipios.getCodigosMunicipioCliente(cliID));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigoPostal"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strDireccion"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strComentarios"));
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strContactos") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                #region CONTACTOS-EMPLAZAMIENTOS

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strCodigoEmplazamiento"));
                filaCabecera.Add(GetGlobalResource("strEmailContacto"));

                #endregion

                #region Reqerido

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");

                #endregion

                #region Fila Tipos de datos

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));

                #endregion

                #region Formato

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region Max Value

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region Min Value

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region Regex

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strEmplazamientos") + " " + GetGlobalResource("strContactos"), Listas, cont);
                cont++;

                #region Values

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigoEmplazamiento"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strEmailContacto"));
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strEmplazamientos") + " " + GetGlobalResource("strContactos") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                #region CONTACTOS-ENTIDADES

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strCodigo"));
                filaCabecera.Add(GetGlobalResource("strEmailContacto"));

                #endregion

                #region Reqerido

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");

                #endregion

                #region Fila Tipos de datos

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));

                #endregion

                #region Formato

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region Max Value

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region Min Value

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region Regex

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strEntidades") + " " + GetGlobalResource("strContactos"), Listas, cont);
                cont++;

                #region Values

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigo"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strEmailContacto"));
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strEntidades") + " " + GetGlobalResource("strContactos") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        #endregion

        #region INVENTARIO

        [DirectMethod]
        public DirectResponse ExportarModeloInventario(string lTipo, long lClienteID)
        {
            DirectResponse direct = new DirectResponse();
            InventarioCategoriasController cController = new InventarioCategoriasController();
            List<Data.InventarioCategorias> listaCategorias = new List<Data.InventarioCategorias>();
            List<Data.CoreAtributosConfiguraciones> listaAtributos;
            List<Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones> listaPlantillas;
            CoreInventarioCategoriasAtributosCategoriasController cCategoriasVin = new CoreInventarioCategoriasAtributosCategoriasController();
            CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();

            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloDatosInventario") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta1.SetValue(saveAs);

                List<string> lTabs = new List<string>();

                long cliID = 0;

                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else
                {
                    if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());
                    }
                }
                List<long> listaCategoriasID = new List<long>();
                foreach (var item in cmbAuxiliar.SelectedItems)
                {
                    listaCategoriasID.Add(long.Parse(item.Value));
                }

                listaCategorias = cController.GetInventarioCategoriasLista(listaCategoriasID);

                List<string> filaCabecera;

                int cont = 0;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                foreach (var oCategoria in listaCategorias)
                {
                    filaCabecera = new List<string>();

                    #region Fila Cabecera

                    filaCabecera.Add(GetGlobalResource("strEmplazamientoCodigo"));
                    filaCabecera.Add(GetGlobalResource("strNombre"));
                    filaCabecera.Add(GetGlobalResource("strCodigoElemento"));
                    filaCabecera.Add(GetGlobalResource("strEstadoInventario"));
                    filaCabecera.Add(GetGlobalResource("strEntidad"));

                    #endregion

                    #region Atributos Dinamicos

                    listaAtributos = cCategoriasVin.GetAtributosByInventarioCategoriaIDSinPlantillas(oCategoria.InventarioCategoriaID);
                    if (listaAtributos != null && listaAtributos.Count > 0)
                    {
                        foreach (var oAtributo in listaAtributos)
                        {
                            filaCabecera.Add(oAtributo.Codigo);
                        }
                    }

                    #endregion

                    #region Subcategorias

                    listaPlantillas = cCategoriasVin.GetSubcategoriaPlantillasValores(oCategoria.InventarioCategoriaID);
                    if (listaPlantillas != null && listaPlantillas.Count > 0)
                    {
                        foreach (var oPla in listaPlantillas)
                        {
                            filaCabecera.Add("Key:" + oPla.InventarioAtributosCategorias.InventarioAtributoCategoria);
                        }
                    }
                    #endregion

                    List<List<string>> Listas = new List<List<string>>();
                    Listas.Add(filaCabecera);

                    Comun.ExportarModeloDatosDinamicoFilas(saveAs, oCategoria.InventarioCategoria, Listas, cont);
                    cont++;

                }
                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.INVENTARIO), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
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

        [DirectMethod]
        public DirectResponse ExportarAyudaModeloInventario(string lTipo, long lClienteID)
        {
            DirectResponse direct = new DirectResponse();
            InventarioCategoriasController cController = new InventarioCategoriasController();
            List<Data.InventarioCategorias> listaCategorias = new List<Data.InventarioCategorias>();
            List<Data.CoreAtributosConfiguraciones> listaAtributos;
            List<Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones> listaPlantillas;
            CoreInventarioCategoriasAtributosCategoriasController cCategoriasVin = new CoreInventarioCategoriasAtributosCategoriasController();
            CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
            List<Data.Vw_CoreAtributosConfiguracionTiposDatosPropiedades> listaPropiedades;
            CoreAtributosConfiguracionTiposDatosPropiedadesController cPropiedades = new CoreAtributosConfiguracionTiposDatosPropiedadesController();
            Vw_CoreAtributosConfiguracionTiposDatosPropiedades oPropiedades;
            CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
            List<CoreInventarioPlantillasAtributosCategorias> listaPlantillasVal;

            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloAyudaDatosInventario") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta2.SetValue(saveAs);

                List<string> lTabs = new List<string>();

                long cliID = 0;

                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else
                {
                    if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());
                    }
                }

                List<long> listaCategoriasID = new List<long>();
                foreach (var item in cmbAuxiliar.SelectedItems)
                {
                    listaCategoriasID.Add(long.Parse(item.Value));
                }

                listaCategorias = cController.GetInventarioCategoriasLista(listaCategoriasID);

                List<string> filaCabecera;
                List<string> filaRequerido;
                List<string> filaTipoDato;
                List<string> filaFormato;
                List<string> filaMaxValue;
                List<string> filaMinValue;
                List<string> filaRegex;

                List<List<string>> ColumnasValores;
                List<string> colValores;

                int cont = 0;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                foreach (var oCategoria in listaCategorias)
                {
                    filaCabecera = new List<string>();
                    filaRequerido = new List<string>();
                    filaTipoDato = new List<string>();
                    filaFormato = new List<string>();
                    filaMaxValue = new List<string>();
                    filaMinValue = new List<string>();
                    filaRegex = new List<string>();

                    ColumnasValores = new List<List<string>>();


                    #region Fila Cabecera

                    filaCabecera.Add(GetGlobalResource("strCampo"));
                    filaCabecera.Add(GetGlobalResource("strEmplazamientoCodigo"));
                    filaCabecera.Add(GetGlobalResource("strNombre"));
                    filaCabecera.Add(GetGlobalResource("strCodigoElemento"));
                    filaCabecera.Add(GetGlobalResource("strEstadoInventario"));
                    filaCabecera.Add(GetGlobalResource("strOperador"));

                    #endregion

                    #region Reqerido

                    filaRequerido.Add(GetGlobalResource("strObligatorio"));
                    filaRequerido.Add("True");
                    filaRequerido.Add("True");
                    filaRequerido.Add("True");
                    filaRequerido.Add("True");
                    filaRequerido.Add("True");

                    #endregion

                    #region Fila Tipos de datos

                    filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                    filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                    filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                    filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                    filaTipoDato.Add(GetGlobalResource("jsLista"));
                    filaTipoDato.Add(GetGlobalResource("jsLista"));

                    #endregion

                    #region Formato

                    filaFormato.Add(GetGlobalResource("strFormato"));
                    filaFormato.Add("");
                    filaFormato.Add("");
                    filaFormato.Add("");
                    filaFormato.Add("");
                    filaFormato.Add("");

                    #endregion

                    #region Max Value

                    filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                    filaMaxValue.Add("");
                    filaMaxValue.Add("");
                    filaMaxValue.Add("");
                    filaMaxValue.Add("");
                    filaMaxValue.Add("");

                    #endregion

                    #region Min Value

                    filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                    filaMinValue.Add("");
                    filaMinValue.Add("");
                    filaMinValue.Add("");
                    filaMinValue.Add("");
                    filaMinValue.Add("");

                    #endregion

                    #region Regex

                    filaRegex.Add(GetGlobalResource("strRegex"));
                    filaRegex.Add("");
                    filaRegex.Add("");
                    filaRegex.Add("");
                    filaRegex.Add("");
                    filaRegex.Add("");

                    #endregion

                    #region Valores Posibles

                    EntidadesController cOperadores = new EntidadesController();
                    InventarioElementosAtributosEstadosController cEstados = new InventarioElementosAtributosEstadosController();

                    colValores = new List<string>();
                    colValores.Insert(0, GetGlobalResource("strEmplazamientoCodigo"));
                    ColumnasValores.Add(colValores);

                    colValores = new List<string>();
                    colValores.Insert(0, GetGlobalResource("strNombre"));
                    ColumnasValores.Add(colValores);

                    colValores = new List<string>();
                    colValores.Insert(0, GetGlobalResource("strCodigoElemento"));
                    ColumnasValores.Add(colValores);

                    colValores = cEstados.GetActivos(cliID).Select(c => c.Codigo).ToList();
                    colValores.Insert(0, GetGlobalResource("strEstadoInventario"));
                    ColumnasValores.Add(colValores);

                    colValores = cOperadores.getEntidadesOperadores(cliID).Select(c => c.Codigo).ToList();
                    colValores.Insert(0, GetGlobalResource("strOperador"));
                    ColumnasValores.Add(colValores);

                    #endregion

                    #region Atributos Dinamicos

                    listaAtributos = cCategoriasVin.GetAtributosByInventarioCategoriaIDSinPlantillas(oCategoria.InventarioCategoriaID);
                    if (listaAtributos != null && listaAtributos.Count > 0)
                    {
                        foreach (var oAtributo in listaAtributos)
                        {
                            filaCabecera.Add(oAtributo.Codigo);
                            filaTipoDato.Add(oAtributo.TiposDatos.TipoDato);
                            listaPropiedades = cPropiedades.GetVwPropiedadesFromAtributo(oAtributo.CoreAtributoConfiguracionID);
                            if (listaPropiedades != null && listaPropiedades.Count > 0)
                            {
                                #region Requerido

                                oPropiedades = null;
                                try
                                {
                                    oPropiedades = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == "AllowBlank").FirstOrDefault();

                                    if (oAtributo.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_BOOLEAN || (oPropiedades != null && oPropiedades.Valor == "False"))
                                    {
                                        filaRequerido.Add("True");
                                    }
                                    else
                                    {
                                        filaRequerido.Add("False");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    filaRequerido.Add("False");
                                }

                                #endregion

                                #region Max Value

                                oPropiedades = null;

                                try
                                {
                                    oPropiedades = listaPropiedades.FindAll(prop => (prop.CodigoTipoDatoPropiedad == "MaxDate" || prop.CodigoTipoDatoPropiedad == "MaxValue")).FirstOrDefault();
                                    if (oPropiedades != null)
                                    {
                                        filaMaxValue.Add(oPropiedades.Valor);
                                    }
                                    else
                                    {
                                        filaMaxValue.Add("");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    filaMaxValue.Add("");
                                }


                                #endregion

                                #region Min Value

                                oPropiedades = null;
                                try
                                {
                                    oPropiedades = listaPropiedades.FindAll(prop => (prop.CodigoTipoDatoPropiedad == "MinDate" || prop.CodigoTipoDatoPropiedad == "MinValue")).FirstOrDefault();

                                    if (oPropiedades != null)
                                    {
                                        filaMinValue.Add(oPropiedades.Valor);
                                    }
                                    else
                                    {
                                        filaMinValue.Add("");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    filaMinValue.Add("");
                                }

                                #endregion

                                #region Regex

                                oPropiedades = null;
                                try
                                {
                                    oPropiedades = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == "Regex").FirstOrDefault();

                                    if (oPropiedades != null)
                                    {
                                        filaRegex.Add(oPropiedades.Valor);
                                    }
                                    else
                                    {
                                        filaRegex.Add("");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    filaRegex.Add("");
                                }

                                #endregion

                                #region Format

                                oPropiedades = null;
                                try
                                {
                                    oPropiedades = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == "AllowDecimals" && prop.Valor == "True").FirstOrDefault();

                                    if (oPropiedades != null)
                                    {
                                        try
                                        {
                                            oPropiedades = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == "DecimalPrecision").FirstOrDefault();

                                            if (oPropiedades != null)
                                            {
                                                filaFormato.Add("0," + string.Concat(Enumerable.Repeat("0", int.Parse(oPropiedades.Valor))));
                                            }
                                            else
                                            {
                                                filaFormato.Add("0,00");
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            filaFormato.Add("0,00");
                                        }
                                    }
                                    else
                                    {
                                        filaFormato.Add("");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    filaFormato.Add("");
                                }

                                #endregion

                            }
                            else
                            {
                                if (oAtributo.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_BOOLEAN)
                                {
                                    filaRequerido.Add("True");
                                }
                                else
                                {
                                    filaRequerido.Add("False");
                                }
                                filaMaxValue.Add("");
                                filaMinValue.Add("");
                                filaFormato.Add("");
                                filaRegex.Add("");
                            }
                            if (oAtributo.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_FECHA)
                            {
                                filaFormato[filaFormato.Count - 1] = "yyyyMMdd";
                            }

                            #region Valore Posibles

                            if (oAtributo.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Lista.ToUpper() || oAtributo.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.ListaMultiple.ToUpper())
                            {
                                if (oAtributo.TablaModeloDatoID != null)
                                {
                                    colValores = cAtributos.GetValoresListStringComboBox((long)oAtributo.CoreAtributoConfiguracionID);
                                    colValores.Insert(0, oAtributo.Nombre);
                                    ColumnasValores.Add(colValores);
                                }
                                else if (oAtributo.ValoresPosibles != null && oAtributo.ValoresPosibles != "")
                                {
                                    colValores = oAtributo.ValoresPosibles.Split(';').ToList();
                                    colValores.Insert(0, oAtributo.Nombre);
                                    ColumnasValores.Add(colValores);
                                }
                                else
                                {
                                    colValores = new List<string>();
                                    colValores.Insert(0, oAtributo.Nombre);
                                    ColumnasValores.Add(colValores);
                                }
                            }
                            else if (oAtributo.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Boolean.ToUpper())
                            {
                                colValores = new List<string>();
                                colValores.Insert(0, oAtributo.Nombre);
                                colValores.Add("True");
                                colValores.Add("False");
                                ColumnasValores.Add(colValores);
                            }
                            else if (oAtributo.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Fecha.ToUpper())
                            {
                                colValores = new List<string>();
                                colValores.Insert(0, oAtributo.Nombre);
                                ColumnasValores.Add(colValores);
                            }
                            else
                            {
                                colValores = new List<string>();
                                colValores.Insert(0, oAtributo.Nombre);
                                ColumnasValores.Add(colValores);
                            }

                            #endregion
                        }
                    }
                    #endregion

                    #region Subcategorias

                    listaPlantillas = cCategoriasVin.GetSubcategoriaPlantillasValores(oCategoria.InventarioCategoriaID);
                    if (listaPlantillas != null && listaPlantillas.Count > 0)
                    {
                        foreach (var oPla in listaPlantillas)
                        {
                            filaCabecera.Add("Key:" + oPla.InventarioAtributosCategorias.InventarioAtributoCategoria);
                            filaRequerido.Add(GetGlobalResource("strSubcategoriaPlantillas"));
                            filaTipoDato.Add("");
                            filaFormato.Add("");
                            filaMaxValue.Add("");
                            filaMinValue.Add("");
                            filaRegex.Add("");

                            #region Subcategorias Valores Plantillas

                            listaPlantillasVal = cPlantillas.GetPlantillasConf(oPla.CoreInventarioCategoriaAtributoCategoriaConfiguracionID);
                            if (listaPlantillasVal != null && listaPlantillasVal.Count > 0)
                            {
                                colValores = listaPlantillasVal.Select(c => c.Nombre).ToList();
                            }
                            else
                            {
                                colValores = new List<string>();
                            }
                            colValores.Insert(0, "Key:" + oPla.InventarioAtributosCategorias.InventarioAtributoCategoria);
                            ColumnasValores.Add(colValores);

                            #endregion

                        }
                    }
                    #endregion


                    List<List<string>> Listas = new List<List<string>>();
                    Listas.Add(filaCabecera);
                    Listas.Add(filaRequerido);
                    Listas.Add(filaTipoDato);
                    Listas.Add(filaFormato);
                    Listas.Add(filaMaxValue);
                    Listas.Add(filaMinValue);
                    Listas.Add(filaRegex);

                    Comun.ExportarModeloDatosDinamicoFilas(saveAs, oCategoria.InventarioCategoria, Listas, cont);
                    cont++;

                    if (oCategoria.InventarioCategoria.Length >= 20)
                    {
                        oCategoria.InventarioCategoria = oCategoria.InventarioCategoria.Substring(0, 20) + "... " + GetGlobalResource("strValor");
                        Comun.ExportarModeloDatosDinamicoColumnas(saveAs, oCategoria.InventarioCategoria, ColumnasValores, cont);
                        cont++;
                    }
                    else
                    {
                        Comun.ExportarModeloDatosDinamicoColumnas(saveAs, oCategoria.InventarioCategoria + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                        cont++;
                    }
                }
                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.INVENTARIO), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
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

        [DirectMethod]
        public DirectResponse GenerarPlantillaSubCategoriasInventario()
        {
            DirectResponse direct = new DirectResponse();
            DocumentosController cDocumentos = new DocumentosController();
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCatCont = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            try
            {
                int hoja = 0;
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloDatosSubcategorias") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta1.SetValue(saveAs);

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region Datos

                List<long> listaSubCategoriasID = new List<long>();
                foreach (var item in cmbAuxiliar.SelectedItems)
                {
                    listaSubCategoriasID.Add(long.Parse(item.Value));
                }

                foreach (var SubID in listaSubCategoriasID)
                {
                    List<string> filaCabecera = new List<string>();

                    #region Fila Cabecera

                    filaCabecera.Add(GetGlobalResource("strNombre"));

                    List<CoreAtributosConfiguraciones> listaAtributos = cCatCont.GetListaAtributos(SubID);

                    if (listaAtributos != null)
                    {
                        foreach (var oAtr in listaAtributos)
                        {
                            filaCabecera.Add(oAtr.Codigo);
                        }
                    }

                    #endregion

                    List<List<string>> Listas = new List<List<string>>();
                    Listas.Add(filaCabecera);
                    Comun.ExportarModeloDatosDinamicoFilas(saveAs, cCatCont.GetItem(SubID).InventarioAtributosCategorias.InventarioAtributoCategoria, Listas, hoja);
                    hoja++;
                }

                #endregion
                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        [DirectMethod]
        public DirectResponse GenerarPlantillaAyudaSubCategoriasInventario()
        {
            DirectResponse direct = new DirectResponse();
            InventarioCategoriasController cController = new InventarioCategoriasController();
            List<Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones> listaCategorias = new List<Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones>();
            List<Data.CoreAtributosConfiguraciones> listaAtributos;
            CoreInventarioCategoriasAtributosCategoriasController cCategoriasVin = new CoreInventarioCategoriasAtributosCategoriasController();
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCategoriasConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
            List<Data.Vw_CoreAtributosConfiguracionTiposDatosPropiedades> listaPropiedades;
            CoreAtributosConfiguracionTiposDatosPropiedadesController cPropiedades = new CoreAtributosConfiguracionTiposDatosPropiedadesController();
            Vw_CoreAtributosConfiguracionTiposDatosPropiedades oPropiedades;

            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloAyudaDatosSubcategorias") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta2.SetValue(saveAs);

                List<string> lTabs = new List<string>();

                long cliID = 0;

                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else
                {
                    if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());
                    }
                }

                List<long> listaCategoriasID = new List<long>();
                foreach (var item in cmbAuxiliar.SelectedItems)
                {
                    listaCategoriasID.Add(long.Parse(item.Value));
                }

                List<string> filaCabecera;
                List<string> filaRequerido;
                List<string> filaTipoDato;
                List<string> filaFormato;
                List<string> filaMaxValue;
                List<string> filaMinValue;
                List<string> filaRegex;

                List<List<string>> ColumnasValores;
                List<string> colValores;

                int cont = 0;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                foreach (var oCategoria in listaCategoriasID)
                {
                    filaCabecera = new List<string>();
                    filaRequerido = new List<string>();
                    filaTipoDato = new List<string>();
                    filaFormato = new List<string>();
                    filaMaxValue = new List<string>();
                    filaMinValue = new List<string>();
                    filaRegex = new List<string>();

                    ColumnasValores = new List<List<string>>();


                    #region Fila Cabecera

                    filaCabecera.Add(GetGlobalResource("strCampo"));
                    filaCabecera.Add(GetGlobalResource("strNombre"));

                    #endregion

                    #region Reqerido

                    filaRequerido.Add(GetGlobalResource("strObligatorio"));
                    filaRequerido.Add("True");

                    #endregion

                    #region Fila Tipos de datos

                    filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                    filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));

                    #endregion

                    #region Formato

                    filaFormato.Add(GetGlobalResource("strFormato"));
                    filaFormato.Add("");

                    #endregion

                    #region Max Value

                    filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                    filaMaxValue.Add("");

                    #endregion

                    #region Min Value

                    filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                    filaMinValue.Add("");

                    #endregion

                    #region Regex

                    filaRegex.Add(GetGlobalResource("strRegex"));
                    filaRegex.Add("");

                    #endregion

                    #region Valores Posibles

                    InventarioElementosAtributosEstadosController cEstados = new InventarioElementosAtributosEstadosController();

                    colValores = new List<string>();
                    colValores.Insert(0, GetGlobalResource("strNombre"));
                    ColumnasValores.Add(colValores);

                    #endregion

                    #region Atributos Dinamicos

                    listaAtributos = cCategoriasConf.GetListaAtributos(oCategoria);
                    if (listaAtributos != null && listaAtributos.Count > 0)
                    {
                        foreach (var oAtributo in listaAtributos)
                        {
                            filaCabecera.Add(oAtributo.Codigo);
                            filaTipoDato.Add(oAtributo.TiposDatos.TipoDato);
                            listaPropiedades = cPropiedades.GetVwPropiedadesFromAtributo(oAtributo.CoreAtributoConfiguracionID);
                            if (listaPropiedades != null && listaPropiedades.Count > 0)
                            {
                                #region Requerido

                                oPropiedades = null;
                                try
                                {
                                    oPropiedades = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == "AllowBlank").FirstOrDefault();

                                    if (oAtributo.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_BOOLEAN || (oPropiedades != null && oPropiedades.Valor == "False"))
                                    {
                                        filaRequerido.Add("True");
                                    }
                                    else
                                    {
                                        filaRequerido.Add("False");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    filaRequerido.Add("False");
                                }

                                #endregion

                                #region Max Value

                                oPropiedades = null;

                                try
                                {
                                    oPropiedades = listaPropiedades.FindAll(prop => (prop.CodigoTipoDatoPropiedad == "MaxDate" || prop.CodigoTipoDatoPropiedad == "MaxValue")).FirstOrDefault();
                                    if (oPropiedades != null)
                                    {
                                        filaMaxValue.Add(oPropiedades.Valor);
                                    }
                                    else
                                    {
                                        filaMaxValue.Add("");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    filaMaxValue.Add("");
                                }


                                #endregion

                                #region Min Value

                                oPropiedades = null;
                                try
                                {
                                    oPropiedades = listaPropiedades.FindAll(prop => (prop.CodigoTipoDatoPropiedad == "MinDate" || prop.CodigoTipoDatoPropiedad == "MinValue")).FirstOrDefault();

                                    if (oPropiedades != null)
                                    {
                                        filaMinValue.Add(oPropiedades.Valor);
                                    }
                                    else
                                    {
                                        filaMinValue.Add("");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    filaMinValue.Add("");
                                }

                                #endregion

                                #region Regex

                                oPropiedades = null;
                                try
                                {
                                    oPropiedades = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == "Regex").FirstOrDefault();

                                    if (oPropiedades != null)
                                    {
                                        filaRegex.Add(oPropiedades.Valor);
                                    }
                                    else
                                    {
                                        filaRegex.Add("");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    filaRegex.Add("");
                                }

                                #endregion

                                #region Format

                                oPropiedades = null;
                                try
                                {
                                    oPropiedades = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == "AllowDecimals" && prop.Valor == "True").FirstOrDefault();

                                    if (oPropiedades != null)
                                    {
                                        try
                                        {
                                            oPropiedades = listaPropiedades.FindAll(prop => prop.CodigoTipoDatoPropiedad == "DecimalPrecision").FirstOrDefault();

                                            if (oPropiedades != null)
                                            {
                                                filaFormato.Add("0," + string.Concat(Enumerable.Repeat("0", int.Parse(oPropiedades.Valor))));
                                            }
                                            else
                                            {
                                                filaFormato.Add("0,00");
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            filaFormato.Add("0,00");
                                        }
                                    }
                                    else
                                    {
                                        filaFormato.Add("");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    filaFormato.Add("");
                                }

                                #endregion

                            }
                            else
                            {
                                if (oAtributo.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_BOOLEAN)
                                {
                                    filaRequerido.Add("True");
                                }
                                else
                                {
                                    filaRequerido.Add("False");
                                }
                                filaMaxValue.Add("");
                                filaMinValue.Add("");
                                filaFormato.Add("");
                                filaRegex.Add("");
                            }
                            if (oAtributo.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_FECHA)
                            {
                                filaFormato[filaFormato.Count - 1] = "yyyyMMdd";
                            }

                            #region Valore Posibles

                            if (oAtributo.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Lista.ToUpper() || oAtributo.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.ListaMultiple.ToUpper())
                            {
                                if (oAtributo.TablaModeloDatoID != null)
                                {
                                    colValores = cAtributos.GetValoresListStringComboBox((long)oAtributo.CoreAtributoConfiguracionID);
                                    colValores.Insert(0, oAtributo.Nombre);
                                    ColumnasValores.Add(colValores);
                                }
                                else if (oAtributo.ValoresPosibles != null && oAtributo.ValoresPosibles != "")
                                {
                                    colValores = oAtributo.ValoresPosibles.Split(';').ToList();
                                    colValores.Insert(0, oAtributo.Nombre);
                                    ColumnasValores.Add(colValores);
                                }
                                else
                                {
                                    colValores = new List<string>();
                                    colValores.Insert(0, oAtributo.Nombre);
                                    ColumnasValores.Add(colValores);
                                }
                            }
                            else if (oAtributo.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Boolean.ToUpper())
                            {
                                colValores = new List<string>();
                                colValores.Insert(0, oAtributo.Nombre);
                                colValores.Add("True");
                                colValores.Add("False");
                                ColumnasValores.Add(colValores);
                            }
                            else if (oAtributo.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Fecha.ToUpper())
                            {
                                colValores = new List<string>();
                                colValores.Insert(0, oAtributo.Nombre);
                                ColumnasValores.Add(colValores);
                            }
                            else
                            {
                                colValores = new List<string>();
                                colValores.Insert(0, oAtributo.Nombre);
                                ColumnasValores.Add(colValores);
                            }

                            #endregion
                        }
                    }
                    #endregion

                    List<List<string>> Listas = new List<List<string>>();
                    Listas.Add(filaCabecera);
                    Listas.Add(filaRequerido);
                    Listas.Add(filaTipoDato);
                    Listas.Add(filaFormato);
                    Listas.Add(filaMaxValue);
                    Listas.Add(filaMinValue);
                    Listas.Add(filaRegex);

                    Comun.ExportarModeloDatosDinamicoFilas(saveAs, cCategoriasConf.GetItem(oCategoria).InventarioAtributosCategorias.InventarioAtributoCategoria, Listas, cont);
                    cont++;
                    Comun.ExportarModeloDatosDinamicoColumnas(saveAs, cCategoriasConf.GetItem(oCategoria).InventarioAtributosCategorias.InventarioAtributoCategoria + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                    cont++;

                }
                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.INVENTARIO), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
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

        [DirectMethod]
        public DirectResponse GenerarPlantillaVinculacionesInventario()
        {
            DirectResponse direct = new DirectResponse();
            DocumentosController cDocumentos = new DocumentosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloDatosVinculacionesElementos") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta1.SetValue(saveAs);
                string ValoresPosibles = string.Join(";", cDocumentos.ValoresDocumentosTipos());

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region Datos

                List<string> filaCabecera = new List<string>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCodigoElemento"));
                filaCabecera.Add(GetGlobalResource("strCodigoElementoPadre"));

                #endregion

                #endregion

                List<List<string>> Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strVinculacion"), Listas, 0);

                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        [DirectMethod]
        public DirectResponse GenerarPlantillaAyudaVinculacionesInventario()
        {
            DirectResponse direct = new DirectResponse();
            DocumentosController cDocumentos = new DocumentosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloAyudaDatosVinculacionesElementos") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta2.SetValue(saveAs);

                #region CLIENTEID
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                int cont = 0;

                List<string> filaCabecera;
                List<string> filaRequerido;
                List<string> filaTipoDato;
                List<string> filaFormato;
                List<string> filaMaxValue;
                List<string> filaMinValue;
                List<string> filaRegex;

                List<List<string>> Listas;

                List<List<string>> ColumnasValores;
                List<string> colValores;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region DOCUMENTAL

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strCodigoElemento"));
                filaCabecera.Add(GetGlobalResource("strCodigoElementoPadre"));

                #endregion

                #region Reqerido

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");

                #endregion

                #region Fila Tipos de datos

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));

                #endregion

                #region Formato

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region Max Value

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region Min Value

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region Regex

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strVinculacion"), Listas, cont);
                cont++;

                #region Values

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCampo"));
                ColumnasValores.Add(colValores);


                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigoElemento"));
                colValores.Add(GetGlobalResource("strCodigoElemento"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigoElementoPadre"));
                colValores.Add("*" + GetGlobalResource("strVacio") + " / " + GetGlobalResource("strCodigoElemento"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strAyudaCargaVinculaciones"));
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strVinculacion") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        #endregion

        #region LOCALIZACION

        [DirectMethod]
        public DirectResponse ExportarModeloRegional()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloDatosRegional") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta1.SetValue(saveAs);

                #region Datos
                List<string> filaTipoDato = new List<string>();
                List<string> filaValoresPosibles = new List<string>();
                List<string> filaCabecera = new List<string>();

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region Fila Tipos de datos
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strFloat"));
                filaTipoDato.Add(GetGlobalResource("strFloat"));
                filaTipoDato.Add(GetGlobalResource("strFloat"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                #endregion

                #region Fila Valores Posibles
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "150"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
                //FactorZona
                filaValoresPosibles.Add("");
                //FactorMunicipio
                filaValoresPosibles.Add("");
                //Factor
                filaValoresPosibles.Add("");
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "200"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "500"));
                filaValoresPosibles.Add(GetGlobalResource("strCaracteresMaximo").ToString().Replace("{0}", "50"));
                #endregion

                #region Fila Cabecera
                filaCabecera.Add(GetGlobalResource("strRegion"));
                filaCabecera.Add(GetGlobalResource("strPais"));
                filaCabecera.Add(GetGlobalResource("strRegionPais"));
                filaCabecera.Add(GetGlobalResource("strProvincia"));
                filaCabecera.Add(GetGlobalResource("strMunicipio"));
                filaCabecera.Add(GetGlobalResource("strFactorZona"));
                filaCabecera.Add(GetGlobalResource("strFactorMunicipio"));
                filaCabecera.Add(GetGlobalResource("strFactor"));
                filaCabecera.Add(GetGlobalResource("strCodigoRegionPais"));
                filaCabecera.Add(GetGlobalResource("strCodigoProvincia"));
                filaCabecera.Add(GetGlobalResource("strCodigoMunicipio"));
                filaCabecera.Add(GetGlobalResource("strMunicipalidad"));
                filaCabecera.Add(GetGlobalResource("strCodigoMunicipalidad"));
                filaCabecera.Add(GetGlobalResource("strPartido"));
                filaCabecera.Add(GetGlobalResource("strCodigoPartido"));
                filaCabecera.Add(GetGlobalResource("strLocalidad"));
                filaCabecera.Add(GetGlobalResource("strCodigoLocalidad"));
                #endregion

                #endregion

                List<List<string>> Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strLocalizacion"), Listas, 0);
                //cEmplazamientos.ExportarModeloDatos(saveAs, GetGlobalResource("strHoja"), filaTipoDato, filaValoresPosibles, filaCabecera);

                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        [DirectMethod]
        public DirectResponse ExportarAyudaModeloRegional()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloAyudaDatosRegional") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta2.SetValue(saveAs);


                #region CLIENTEID
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                int cont = 0;

                List<string> filaCabecera;
                List<string> filaRequerido;
                List<string> filaTipoDato;
                List<string> filaFormato;
                List<string> filaMaxValue;
                List<string> filaMinValue;
                List<string> filaRegex;

                List<List<string>> Listas;

                List<List<string>> ColumnasValores;
                List<string> colValores;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region REGIONAL

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strRegion"));
                filaCabecera.Add(GetGlobalResource("strPais"));
                filaCabecera.Add(GetGlobalResource("strRegionPais"));
                filaCabecera.Add(GetGlobalResource("strProvincia"));
                filaCabecera.Add(GetGlobalResource("strMunicipio"));
                filaCabecera.Add(GetGlobalResource("strFactorZona"));
                filaCabecera.Add(GetGlobalResource("strFactorMunicipio"));
                filaCabecera.Add(GetGlobalResource("strFactor"));
                filaCabecera.Add(GetGlobalResource("strCodigoRegionPais"));
                filaCabecera.Add(GetGlobalResource("strCodigoProvincia"));
                filaCabecera.Add(GetGlobalResource("strCodigoMunicipio"));
                filaCabecera.Add(GetGlobalResource("strMunicipalidad"));
                filaCabecera.Add(GetGlobalResource("strCodigoMunicipalidad"));
                filaCabecera.Add(GetGlobalResource("strPartido"));
                filaCabecera.Add(GetGlobalResource("strCodigoPartido"));
                filaCabecera.Add(GetGlobalResource("strLocalidad"));
                filaCabecera.Add(GetGlobalResource("strCodigoLocalidad"));

                #endregion

                #region Reqerido

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");

                #endregion

                #region Fila Tipos de datos

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strFloat"));
                filaTipoDato.Add(GetGlobalResource("strFloat"));
                filaTipoDato.Add(GetGlobalResource("strFloat"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));

                #endregion

                #region Formato

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region Max Value

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region Min Value

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region Regex

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strLocalizacion"), Listas, cont);
                cont++;

                #region Values

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCampo"));
                ColumnasValores.Add(colValores);


                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strRegion"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strPais"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strRegionPais"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strProvincia"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strMunicipio"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strFactorZona"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strFactorMunicipio"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strFactor"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigoRegionPais"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigoProvincia"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigoMunicipio"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strMunicipalidad"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigoMunicipalidad"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strPartido"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigoPartido"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strLocalidad"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigoLocalidad"));
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strLocalizacion") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        #endregion

        #region PRODUCTOS

        [DirectMethod]
        public DirectResponse ExportarAyudaModeloProductCatalogServicios()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloAyudaDatosProductCatalogServicios") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta2.SetValue(saveAs);

                #region CLIENTE
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                int cont = 0;

                List<string> filaCabecera;
                List<string> filaRequerido;
                List<string> filaTipoDato;
                List<string> filaFormato;
                List<string> filaMaxValue;
                List<string> filaMinValue;
                List<string> filaRegex;

                List<List<string>> Listas;

                List<List<string>> ColumnasValores;
                List<string> colValores;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region SERVICIOS

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strCodigo"));
                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add(GetGlobalResource("strUnidades"));
                filaCabecera.Add(GetGlobalResource("strEntidad"));
                filaCabecera.Add(GetGlobalResource("strTipo"));

                #endregion

                #region Reqerido

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");

                #endregion

                #region Fila Tipos de datos

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strNumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));

                #endregion

                #region Formato

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("0.00");
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region Max Value

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region Min Value

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("1");
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region Regex

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strProductCatalogServicios"), Listas, cont);
                cont++;

                #region Values

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strNombre"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strCodigo"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strUnidades"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strEntidad"));
                BaseAPIClient<CompanyDTO> cEntidades = new BaseAPIClient<CompanyDTO>(TOKEN_API);
                foreach (var valor in cEntidades.GetList().Result.Value)
                {
                    colValores.Add(valor.Code);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strTipo"));
                BaseAPIClient<ProductTypeDTO> cTipos = new BaseAPIClient<ProductTypeDTO>(TOKEN_API);
                foreach (var valor in cTipos.GetList().Result.Value)
                {
                    colValores.Add(valor.Code);
                }
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strProductCatalogServicios") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                #region SERVICIOS ASIGNADOS

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strCodigoPadre"));
                filaCabecera.Add(GetGlobalResource("strCodigo"));

                #endregion

                #region Reqerido

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");

                #endregion

                #region Fila Tipos de datos

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));

                #endregion

                #region Formato

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region Max Value

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region Min Value

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region Regex

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strProductCatalogServicios"), Listas, cont);
                cont++;

                #endregion

                /*#region CHECKLIST

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strServicio"));
                filaCabecera.Add(GetGlobalResource("strChecklist"));
                filaCabecera.Add(GetGlobalResource("strNumMinimoFotos"));

                #endregion

                #region Reqerido

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");

                #endregion

                #region Fila Tipos de datos

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strBooleano"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));

                #endregion

                #region Formato

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region Max Value

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("100");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region Min Value

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("0");

                #endregion

                #region Regex

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strChecklist"), Listas, cont);
                cont++;

                #region Values

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strServicio"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strChecklist"));
                colValores.Add("TRUE");
                colValores.Add("FALSE");
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strNumMinimoFotos"));
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strChecklist") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                #region ATRIBUTOS

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strServicio"));
                filaCabecera.Add(GetGlobalResource("strAtributo"));
                filaCabecera.Add(GetGlobalResource("strTipoDato"));

                #endregion

                #region Reqerido

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("False");
                filaRequerido.Add("False");
                filaRequerido.Add("False");

                #endregion

                #region Fila Tipos de datos

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));

                #endregion

                #region Formato

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region Max Value

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("100");
                filaMaxValue.Add("100");
                filaMaxValue.Add("");

                #endregion

                #region Min Value

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region Regex

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strAtributos"), Listas, cont);
                cont++;

                #region Values

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strServicio"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strAtributo"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strTipoDato"));
                TiposDatosController cTiposDatos = new TiposDatosController();
                foreach (var item in cTiposDatos.GetActivos(cliID))
                {
                    colValores.Add(item.TipoDato);
                }
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strAtributos") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion*/

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

        [DirectMethod]
        public DirectResponse ExportarModeloProductCatalogServicios()
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();

            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloDatosProductCatalogServicios") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta1.SetValue(saveAs);

                #region CLIENTE
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                List<string> filaCabecera;
                List<List<string>> Listas;

                int cont = 0;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region SERVICIOS

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add(GetGlobalResource("strCodigo"));
                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add(GetGlobalResource("strUnidades"));
                filaCabecera.Add(GetGlobalResource("strEntidad"));
                filaCabecera.Add(GetGlobalResource("strTipo"));

                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strProductCatalogServicios"), Listas, cont);
                cont++;

                #endregion

                #region SERVICIOS ASIGNACION

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add(GetGlobalResource("strCodigoPadre"));
                filaCabecera.Add(GetGlobalResource("strCodigo"));

                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strVinculacion"), Listas, cont);
                cont++;

                #endregion

                /*#region CHECKLIST

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add(GetGlobalResource("strServicio"));
                filaCabecera.Add(GetGlobalResource("strChecklist"));
                filaCabecera.Add(GetGlobalResource("strNumMinimoFotos"));

                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strChecklist"), Listas, cont);
                cont++;

                #endregion

                #region ATRIBUTOS

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add(GetGlobalResource("strServicio"));
                filaCabecera.Add(GetGlobalResource("strAtributo"));
                filaCabecera.Add(GetGlobalResource("strTipoDato"));

                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strAtributos"), Listas, cont);
                cont++;

                #endregion*/
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

        #endregion

        #region USUARIOS

        [DirectMethod]
        public DirectResponse ExportarAyudaModeloUsuarios()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloAyudaUsuarios") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta2.SetValue(saveAs);

                #region CLIENTEID
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                int cont = 0;

                List<string> filaCabecera;
                List<string> filaRequerido;
                List<string> filaTipoDato;
                List<string> filaFormato;
                List<string> filaMaxValue;
                List<string> filaMinValue;
                List<string> filaRegex;

                List<List<string>> Listas;

                List<List<string>> ColumnasValores;
                List<string> colValores;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region ENTIDADES

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add(GetGlobalResource("strApellidos"));
                filaCabecera.Add(GetGlobalResource("strPrefijo"));
                filaCabecera.Add(GetGlobalResource("strTelefono"));
                filaCabecera.Add(GetGlobalResource("strEmail"));
                filaCabecera.Add(GetGlobalResource("strEntidad"));
                filaCabecera.Add(GetGlobalResource("strFechaCaducidad"));

                #endregion

                #region Reqerido

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");
                filaRequerido.Add("True");

                #endregion

                #region Fila Tipos de datos

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));
                filaTipoDato.Add(GetGlobalResource("strFecha"));

                #endregion

                #region Formato

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("");
                filaFormato.Add("yyyyMMdd");

                #endregion

                #region Max Value

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region Min Value

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region Regex

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strUsuarios"), Listas, cont);
                cont++;

                #region Values

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strNombre"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strApellidos"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strPrefijo"));
                PaisesController cPaises = new PaisesController();
                foreach (var item in cPaises.GetActivos(cliID))
                {
                    colValores.Add(item.Prefijo);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strTelefono"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strEmail"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strEntidad"));
                EntidadesController cEntidades = new EntidadesController();
                foreach (var item in cEntidades.GetActivos(cliID))
                {
                    colValores.Add(item.Nombre);
                }
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strFechaCaducidad"));
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strUsuarios") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                #region USUARIOS ROLES

                filaCabecera = new List<string>();
                filaRequerido = new List<string>();
                filaTipoDato = new List<string>();
                filaFormato = new List<string>();
                filaMaxValue = new List<string>();
                filaMinValue = new List<string>();
                filaRegex = new List<string>();

                ColumnasValores = new List<List<string>>();

                #region Fila Cabecera

                filaCabecera.Add(GetGlobalResource("strCampo"));
                filaCabecera.Add(GetGlobalResource("strEmail"));
                filaCabecera.Add(GetGlobalResource("strRol"));

                #endregion

                #region Reqerido

                filaRequerido.Add(GetGlobalResource("strObligatorio"));
                filaRequerido.Add("True");
                filaRequerido.Add("True");

                #endregion

                #region Fila Tipos de datos

                filaTipoDato.Add(GetGlobalResource("strTipoDato"));
                filaTipoDato.Add(GetGlobalResource("strAlfanumerico"));
                filaTipoDato.Add(GetGlobalResource("jsLista"));

                #endregion

                #region Formato

                filaFormato.Add(GetGlobalResource("strFormato"));
                filaFormato.Add("");
                filaFormato.Add("");

                #endregion

                #region Max Value

                filaMaxValue.Add(GetGlobalResource("strValorMaximo"));
                filaMaxValue.Add("");
                filaMaxValue.Add("");

                #endregion

                #region Min Value

                filaMinValue.Add(GetGlobalResource("strValorMinimo"));
                filaMinValue.Add("");
                filaMinValue.Add("");

                #endregion

                #region Regex

                filaRegex.Add(GetGlobalResource("strRegex"));
                filaRegex.Add("");
                filaRegex.Add("");

                #endregion

                Listas = new List<List<string>>();
                Listas.Add(filaCabecera);
                Listas.Add(filaRequerido);
                Listas.Add(filaTipoDato);
                Listas.Add(filaFormato);
                Listas.Add(filaMaxValue);
                Listas.Add(filaMinValue);
                Listas.Add(filaRegex);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strRoles"), Listas, cont);
                cont++;

                #region Values

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strNombre"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strEmail"));
                ColumnasValores.Add(colValores);

                colValores = new List<string>();
                colValores.Add(GetGlobalResource("strRol"));
                RolesController cRoles = new RolesController();
                foreach (var item in cRoles.GetActivos(cliID))
                {
                    colValores.Add(item.Nombre);
                }
                ColumnasValores.Add(colValores);

                #endregion

                Comun.ExportarModeloDatosDinamicoColumnas(saveAs, GetGlobalResource("strRoles") + " " + GetGlobalResource("strValor"), ColumnasValores, cont);
                cont++;

                #endregion

                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        [DirectMethod]
        public DirectResponse ExportarModeloUsuarios()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            try
            {
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strModeloDatosUsuarios") + DateTime.Today.ToString("yyyyMMdd") + "-" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".xls";
                string saveAs = directorio + fileName;
                hdRuta1.SetValue(saveAs);

                #region CLIENTEID
                long cliID = 0;
                if (ClienteID != null)
                {
                    cliID = ClienteID.Value;
                }
                else if (hdCliID.Value != null && hdCliID.Value.ToString() != "")
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                }
                #endregion

                List<string> filaCabecera;
                List<List<string>> Listas;

                int cont = 0;

                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }

                #region Usuarios

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add(GetGlobalResource("strNombre"));
                filaCabecera.Add(GetGlobalResource("strApellidos"));
                filaCabecera.Add(GetGlobalResource("strPrefijo"));
                filaCabecera.Add(GetGlobalResource("strTelefono"));
                filaCabecera.Add(GetGlobalResource("strEmail"));
                filaCabecera.Add(GetGlobalResource("strEntidad"));
                filaCabecera.Add(GetGlobalResource("strFechaCaducidad"));

                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strUsuarios"), Listas, cont);
                cont++;

                #endregion

                #region Usuarios Roles

                Listas = new List<List<string>>();
                filaCabecera = new List<string>();

                filaCabecera.Add(GetGlobalResource("strEmail"));
                filaCabecera.Add(GetGlobalResource("strRol"));

                Listas.Add(filaCabecera);

                Comun.ExportarModeloDatosDinamicoFilas(saveAs, GetGlobalResource("strRoles"), Listas, cont);
                cont++;

                #endregion

                //Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
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

        #endregion

        #endregion
    }
}