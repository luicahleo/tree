using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TreeCore.Page;
using TreeCore.Componentes;
using log4net;
using System.Reflection;
using TreeCore.Data;
using CapaNegocio;
using Newtonsoft.Json.Linq;

namespace TreeCore.ModGlobal.pages
{
    public partial class EmplazamientosHistoricos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        private static readonly List<string> lRecursosTraducciones = new List<string>()
        {
            // PRINCIPAL
            "strCodigo",
            "strNombreSitio",
            "strUsuario",
            "strFechaModificacion",
            "strOperador",
            "strEstadoGlobal",
            "strMoneda",
            "strCategoriaSitio",
            "strTipo",
            "strTipoEdificio",
            "strTipoEstructura",
            "strTamano",
            "strFechaActivacion",
            "strFechaDesactivacion",

            // LOCALIZACION
            "strDireccion",
            "strMunicipalidad",
            "strBarrio",
            "strCodigoPostal",
            "strLatitud",
            "strLongitud"
        };

        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                List<string> listaIgnore = new List<string>() { };

                Comun.CreateGridFilters(gridFiltersPrincipal, storePrincipal, gridPrincipal.ColumnModel, listaIgnore, _Locale);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            lbltituloPrincipal.Text = GetGlobalResource("strHistorico") + " " + GetGlobalResource("strEmplazamiento");

            if (this.Session["LOCALE"] != null)
            {
                hdLocale.Value = this.Session["LOCALE"].ToString();
            }

            sPagina = "EmplazamientosContenedor.aspx";
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { }},
            { "Put", new List<ComponentBase> { }},
            { "Delete", new List<ComponentBase> { }}
        };
        }

        #region STORES

        #region PRINCIPAL

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {

            if (RequestManager.IsAjaxRequest)
            {

                try
                {
                    var lista = ListaPrincipal();

                    if (lista != null)
                    {

                        storePrincipal.DataSource = lista;
                        storePrincipal.DataBind();
                    }
                }
                catch (Exception ex)
                {

                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_HistoricosCoreEmplazamientos> ListaPrincipal()
        {

            List<Data.Vw_HistoricosCoreEmplazamientos> listaDatos;
            HistoricosCoreEmplazamientosController cEmplazamientosHistoricos = new HistoricosCoreEmplazamientosController();

            try
            {
                listaDatos = cEmplazamientosHistoricos.GetVwHistoricosByID(long.Parse(Request.QueryString["EmplazamientoID"]));
            }
            catch (Exception ex)
            {

                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        #endregion

        #region TRADUCCIONES 

        protected void storeTraducciones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {

                try
                {

                    int iCount = 0;
                    var lista = ListaTraducciones();
                    if (lista != null)
                    {

                        storeTraducciones.DataSource = lista;
                        PageProxy temp = (PageProxy)storeTraducciones.Proxy[0];
                        temp.Total = iCount;
                    }
                }
                catch (Exception ex)
                {

                    log.Error(ex.Message);
                }
            }
        }

        private List<JsonObject> ListaTraducciones()
        {
            EmplazamientosAtributosConfiguracionController cAtributos = new EmplazamientosAtributosConfiguracionController();
            EmplazamientosAtributosJsonController cEmplAtr = new EmplazamientosAtributosJsonController();
            List<JsonObject> listaDatos = new List<JsonObject>();
            long UsuarioID = ((TreeCore.Data.Usuarios)this.Session["USUARIO"]).UsuarioID;

            try
            {
                JsonObject elementoVacio = new JsonObject();
                elementoVacio.Add("Traduccion", GetGlobalResource("strElementos"));
                listaDatos.Add(elementoVacio);

                foreach (string recurso in lRecursosTraducciones)
                {

                    JsonObject temp = new JsonObject();
                    temp.Add("Traduccion", GetGlobalResource(recurso));
                    listaDatos.Add(temp);
                }

                string sEmplazamientoID = Request.QueryString["EmplazamientoID"];
                EmplazamientosController cEmplazamientos = new EmplazamientosController();
                foreach (var item in cEmplAtr.Deserializacion(cEmplazamientos.GetItem("EmplazamientoID=" + sEmplazamientoID).JsonAtributosDinamicos))
                {

                    JsonObject temp = new JsonObject();
                    temp.Add("Traduccion", item.NombreAtributo);
                    if (cAtributos.AtributoVisible(item.AtributoID, UsuarioID))
                    {
                        listaDatos.Add(temp);
                    }
                }
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

        #region DIRECT METHODS

        [DirectMethod]
        public DirectResponse GetEstadoActualEmplazamientoJSON()
        {

            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            EmplazamientosAtributosJsonController cEmplAtr = new EmplazamientosAtributosJsonController();
            long lEmplazamientoID = Convert.ToInt64(Request.QueryString["EmplazamientoID"]);
            EmplazamientosAtributosConfiguracionController cAtributos = new EmplazamientosAtributosConfiguracionController();
            long UsuarioID = ((TreeCore.Data.Usuarios)this.Session["USUARIO"]).UsuarioID;

            try
            {

                Data.Emplazamientos emplazamiento = cEmplazamientos.GetItem("EmplazamientoID=" + lEmplazamientoID);
                JObject oJsonEmplazamientoOrdenado = new JObject();
                oJsonEmplazamientoOrdenado.Add("Codigo", emplazamiento.Codigo);
                oJsonEmplazamientoOrdenado.Add("NombreSitio", emplazamiento.NombreSitio);
                if (emplazamiento.UltimoUsuarioModificadorID != null)
                {
                    oJsonEmplazamientoOrdenado.Add("UsuarioModificador", emplazamiento.UltimoUsuarioModificador.NombreCompleto);
                }
                else
                {
                    oJsonEmplazamientoOrdenado.Add("UsuarioModificador", "");
                }
                if (emplazamiento.FechaUltimaModificacion == null || emplazamiento.FechaUltimaModificacion.ToString() == "")
                {
                    oJsonEmplazamientoOrdenado.Add("FechaModificacion", "");
                }
                else
                {
                    oJsonEmplazamientoOrdenado.Add("FechaModificacion", ((DateTime)emplazamiento.FechaUltimaModificacion).ToString(Comun.FORMATO_FECHA));
                }
                oJsonEmplazamientoOrdenado.Add("Operador", emplazamiento.Operadores.Operador);
                oJsonEmplazamientoOrdenado.Add("EstadoGlobal", emplazamiento.EstadosGlobales.EstadoGlobal);
                oJsonEmplazamientoOrdenado.Add("Moneda", emplazamiento.Monedas.Moneda);
                oJsonEmplazamientoOrdenado.Add("CategoriaSitio", emplazamiento.EmplazamientosCategoriasSitios.CategoriaSitio);
                oJsonEmplazamientoOrdenado.Add("Tipo", emplazamiento.EmplazamientosTipos.Tipo);
                oJsonEmplazamientoOrdenado.Add("TipoEdificio", emplazamiento.EmplazamientosTiposEdificios.TipoEdificio);
                oJsonEmplazamientoOrdenado.Add("TipoEstructura", emplazamiento.EmplazamientosTiposEstructuras.TipoEstructura);
                oJsonEmplazamientoOrdenado.Add("Tamano", emplazamiento.EmplazamientosTamanos.Tamano);
                oJsonEmplazamientoOrdenado.Add("FechaActivacion", ((DateTime)emplazamiento.FechaActivacion).ToString(Comun.FORMATO_FECHA));
                if (emplazamiento.FechaDesactivacion == null || emplazamiento.FechaDesactivacion.ToString() == "")
                {
                    oJsonEmplazamientoOrdenado.Add("FechaDesactivacion", "");
                }
                else
                {
                    oJsonEmplazamientoOrdenado.Add("FechaDesactivacion", ((DateTime)emplazamiento.FechaDesactivacion).ToString(Comun.FORMATO_FECHA));
                }
                oJsonEmplazamientoOrdenado.Add("Direccion", emplazamiento.Direccion);
                oJsonEmplazamientoOrdenado.Add("Municipio", emplazamiento.Municipios.Municipio);
                oJsonEmplazamientoOrdenado.Add("Barrio", emplazamiento.Barrio);
                oJsonEmplazamientoOrdenado.Add("CodigoPostal", emplazamiento.CodigoPostal);
                oJsonEmplazamientoOrdenado.Add("Latitud", emplazamiento.Latitud);
                oJsonEmplazamientoOrdenado.Add("Longitud", emplazamiento.Longitud);

                foreach (var item in cEmplAtr.Deserializacion(emplazamiento.JsonAtributosDinamicos))
                {
                    if (cAtributos.AtributoVisible(item.AtributoID, UsuarioID))
                    {
                        if (oJsonEmplazamientoOrdenado.GetValue("Atr" + item.AtributoID) == null)
                        {
                            oJsonEmplazamientoOrdenado.Add("Atr" + item.AtributoID, item.TextLista);
                        }
                    }
                }

                direct.Success = true;
                direct.Result = oJsonEmplazamientoOrdenado;
            }
            catch (Exception ex)
            {

                direct.Success = false;
                direct.Result = null;
                log.Error(ex.Message);
            }

            return direct;
        }

        #region LOADER PARA NAV BAR SUPERIOR

        #endregion

        [DirectMethod]
        public DirectResponse ExportarEmplazamientos(long[] listaIDs)
        {
            List<string> listaString;
            List<List<string>> listaListas;
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            HistoricosCoreEmplazamientosController cEmplazamientosHistoricos = new HistoricosCoreEmplazamientosController();
            EmplazamientosAtributosConfiguracionController cEmplAtrConf = new EmplazamientosAtributosConfiguracionController();
            EmplazamientosAtributosJsonController cAtributos = new EmplazamientosAtributosJsonController();
            List<Data.EmplazamientosAtributosJson> listaAtrValores;
            Data.EmplazamientosAtributosJson oAtrValor;
            List<Data.EmplazamientosAtributosConfiguracion> listaAtributos;
            JObject json, jsonAux;
            object oAux;
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                listaListas = new List<List<string>>();
                Data.Emplazamientos oEmpla = cEmplazamientos.GetItem(long.Parse(Request.QueryString["EmplazamientoID"]));
                List<Data.Vw_HistoricosCoreEmplazamientos> listaHistoricos = cEmplazamientosHistoricos.GetVwHistoricosByByHistoricosIDs(listaIDs.ToList());
                if (oEmpla != null)
                {
                    string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                    string fileName = "";
                    fileName = GetGlobalResource("strHistorico") + "_" + oEmpla.NombreSitio.Replace("/", "") + "_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
                    string saveAs = directorio + fileName;

                    using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(saveAs, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                    {
                        var workbookPart = workbook.AddWorkbookPart();
                        workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                        workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                    }

                    #region CABECERAS

                    listaString = new List<string>();

                    listaString.Add(GetGlobalResource("strUsuario"));
                    listaString.Add(GetGlobalResource("strFecha"));
                    listaString.Add(GetGlobalResource("strCodigo"));
                    listaString.Add(GetGlobalResource("strNombreSitio"));
                    listaString.Add(GetGlobalResource("strOperador"));
                    listaString.Add(GetGlobalResource("strEstadoGlobal"));
                    listaString.Add(GetGlobalResource("strMoneda"));
                    listaString.Add(GetGlobalResource("strCategoriaSitio"));
                    listaString.Add(GetGlobalResource("strTipo"));
                    listaString.Add(GetGlobalResource("strTipoEdificio"));
                    listaString.Add(GetGlobalResource("strTipoEstructura"));
                    listaString.Add(GetGlobalResource("strTamano"));
                    listaString.Add(GetGlobalResource("strFechaActivacion"));
                    listaString.Add(GetGlobalResource("strFechaDesactivacion"));
                    listaString.Add(GetGlobalResource("strDireccion"));
                    listaString.Add(GetGlobalResource("strMunicipalidad"));
                    listaString.Add(GetGlobalResource("strBarrio"));
                    listaString.Add(GetGlobalResource("strCodigoPostal"));
                    listaString.Add(GetGlobalResource("strLatitud"));
                    listaString.Add(GetGlobalResource("strLongitud"));


                    listaAtributos = cEmplAtrConf.GetAtributosVisibles(oEmpla.ClienteID, Usuario.UsuarioID);

                    if (listaAtributos != null && listaAtributos.Count > 0)
                    {
                        foreach (var oAtr in listaAtributos)
                        {
                            listaString.Add(oAtr.NombreAtributo);
                        }
                    }

                    listaListas.Add(listaString);

                    #endregion

                    #region ESTADO ACTUAL

                    listaAtrValores = cAtributos.Deserializacion(oEmpla.JsonAtributosDinamicos);

                    listaString = new List<string>();

                    if (oEmpla.UltimoUsuarioModificadorID != null)
                    {
                        listaString.Add(oEmpla.UltimoUsuarioModificador.NombreCompleto);
                    }
                    else
                    {
                        listaString.Add("");
                    }
                    if (oEmpla.FechaUltimaModificacion != null)
                    {
                        listaString.Add(((DateTime)oEmpla.FechaUltimaModificacion).ToString(Comun.FORMATO_FECHA));
                    }
                    else
                    {
                        listaString.Add("");
                    }
                    listaString.Add(oEmpla.Codigo);
                    listaString.Add(oEmpla.NombreSitio);
                    listaString.Add(oEmpla.Operadores.Operador);
                    listaString.Add(oEmpla.EstadosGlobales.EstadoGlobal);
                    listaString.Add(oEmpla.Monedas.Moneda);
                    listaString.Add(oEmpla.EmplazamientosCategoriasSitios.CategoriaSitio);
                    listaString.Add(oEmpla.EmplazamientosTipos.Tipo);
                    listaString.Add(oEmpla.EmplazamientosTiposEdificios.TipoEdificio);
                    listaString.Add(oEmpla.EmplazamientosTiposEstructuras.TipoEstructura);
                    listaString.Add(oEmpla.EmplazamientosTamanos.Tamano);
                    listaString.Add(((DateTime)oEmpla.FechaCreacion).ToString(Comun.FORMATO_FECHA));
                    if (oEmpla.FechaDesactivacion != null)
                    {
                        listaString.Add(((DateTime)oEmpla.FechaDesactivacion).ToString(Comun.FORMATO_FECHA));
                    }
                    else
                    {
                        listaString.Add("");
                    }
                    listaString.Add(oEmpla.Direccion);
                    listaString.Add(oEmpla.Municipios.Municipio);
                    listaString.Add(oEmpla.Barrio);
                    listaString.Add(oEmpla.Latitud.ToString());
                    listaString.Add(oEmpla.Longitud.ToString());

                    foreach (var item in listaAtributos)
                    {
                        oAtrValor = (from c in listaAtrValores where c.AtributoID == item.EmplazamientoAtributoConfiguracionID select c).FirstOrDefault();
                        if (oAtrValor != null)
                        {
                            listaString.Add(oAtrValor.TextLista);
                        }
                        else
                        {
                            listaString.Add("");
                        }
                    }

                    listaListas.Add(listaString);

                    #endregion

                    foreach (var oHis in listaHistoricos)
                    {
                        json = JObject.Parse(oHis.DatosJSON);
                        listaString = new List<string>();
                        listaString.Add(oHis.NombreCompleto);
                        listaString.Add(oHis.FechaModificacion.ToString(Comun.FORMATO_FECHA));
                        listaString.Add(oHis.Codigo);
                        if (json.GetValue("NombreSitio") != null)
                        {
                            listaString.Add(json.GetValue("NombreSitio").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("Operador") != null)
                        {
                            listaString.Add(json.GetValue("Operador").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("EstadoGlobal") != null)
                        {
                            listaString.Add(json.GetValue("EstadoGlobal").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("Moneda") != null)
                        {
                            listaString.Add(json.GetValue("Moneda").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("CategoriaSitio") != null)
                        {
                            listaString.Add(json.GetValue("CategoriaSitio").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("Tipo") != null)
                        {
                            listaString.Add(json.GetValue("Tipo").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("TipoEdificio") != null)
                        {
                            listaString.Add(json.GetValue("TipoEdificio").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("TipoEstructura") != null)
                        {
                            listaString.Add(json.GetValue("TipoEstructura").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("Tamano") != null)
                        {
                            listaString.Add(json.GetValue("Tamano").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("FechaActivacion") != null)
                        {
                            listaString.Add(json.GetValue("FechaActivacion").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("FechaDesactivacion") != null)
                        {
                            listaString.Add(json.GetValue("FechaDesactivacion").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("Direccion") != null)
                        {
                            listaString.Add(json.GetValue("Direccion").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("Municipio") != null)
                        {
                            listaString.Add(json.GetValue("Municipio").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("Barrio") != null)
                        {
                            listaString.Add(json.GetValue("Barrio").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("Latitud") != null)
                        {
                            listaString.Add(json.GetValue("Latitud").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("Longitud") != null)
                        {
                            listaString.Add(json.GetValue("Longitud").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (listaAtributos != null && listaAtributos.Count > 0)
                        {
                            foreach (var oAtr in listaAtributos)
                            {
                                try
                                {
                                    if (json.GetValue("Atr" + oAtr.EmplazamientoAtributoConfiguracionID) != null)
                                    {
                                        listaString.Add(json.GetValue("Atr" + oAtr.EmplazamientoAtributoConfiguracionID).ToString());
                                    }
                                    else
                                    {
                                        listaString.Add("");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    listaString.Add("");
                                }
                            }
                        }
                        listaListas.Add(listaString);
                    }
                    Comun.ExportarModeloDatosDinamicoFilas(saveAs, oEmpla.Codigo, listaListas, 0);

                    Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = null;
                log.Error(ex.Message);
            }

            return direct;
        }

        #endregion
    }
}