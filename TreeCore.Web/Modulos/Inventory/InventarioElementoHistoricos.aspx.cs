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
using System.Globalization;

namespace TreeCore.ModGlobal.pages
{
    public partial class InventarioElementoHistoricos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        private static readonly List<string> lRecursosTraducciones = new List<string>()
        {

            "strCodigo",
            "strNombre",
            "strFechaCreacion"/*,
            "strFechaAlta"*/
        };

        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                List<string> listaIgnore = new List<string>() { };

                Comun.CreateGridFilters(gridFiltersPrincipal, storePrincipal, gridPrincipal.ColumnModel, listaIgnore, _Locale);
                hdElementoID.Value = Request.QueryString["InventarioElementoID"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblTituloPrincipal.Text = GetGlobalResource("strHistorico") + " " + GetGlobalResource("strInventario");

            if (this.Session["LOCALE"] != null)
            {
                hdLocale.Value = this.Session["LOCALE"].ToString();
            }

            sPagina = "InventarioGestionContenedor.aspx";
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

        private List<Data.Vw_InventarioElementosHistoricos> ListaPrincipal()
        {
            List<Data.Vw_InventarioElementosHistoricos> listaDatos;
            InventarioElementosHistoricosController cInvElemHistoricos = new InventarioElementosHistoricosController();

            try
            {
                listaDatos = cInvElemHistoricos.getVwHistoricosByID(long.Parse(hdElementoID.Value.ToString()));
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


            List<JsonObject> listaDatos = new List<JsonObject>();

            try
            {
                JsonObject elementoVacio = new JsonObject();
                elementoVacio.Add("Traduccion", GetGlobalResource("strElementos"));
                listaDatos.Add(elementoVacio);
                JsonObject temp;

                InventarioElementosAtributosJsonController cAtrJson = new InventarioElementosAtributosJsonController();
                InventarioElementosPlantillasJsonController cPlaJson = new InventarioElementosPlantillasJsonController();
                InventarioPlantillasAtributosJsonController cPlaAtrJson = new InventarioPlantillasAtributosJsonController();
                CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
                CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
                InventarioElementosController cInvElementos = new InventarioElementosController();
                long lInvElemID = Convert.ToInt64(Request.QueryString["InventarioElementoID"]);
                List<long> listaPlaIds;
                long UsuarioID = ((TreeCore.Data.Usuarios)this.Session["USUARIO"]).UsuarioID;

                Data.InventarioElementos invElemento = cInvElementos.GetItem("InventarioElementoID=" + lInvElemID);
                temp = new JsonObject();
                temp.Add("Traduccion", GetGlobalResource("strCodigo"));
                listaDatos.Add(temp);
                temp = new JsonObject();
                temp.Add("Traduccion", GetGlobalResource("strNombre"));
                listaDatos.Add(temp);
                temp = new JsonObject();
                temp.Add("Traduccion", GetGlobalResource("strUsuario"));
                listaDatos.Add(temp);
                temp = new JsonObject();
                temp.Add("Traduccion", GetGlobalResource("strFechaModificacion"));
                listaDatos.Add(temp);
                temp = new JsonObject();
                temp.Add("Traduccion", GetGlobalResource("strEstado"));
                listaDatos.Add(temp);
                temp = new JsonObject();
                temp.Add("Traduccion", GetGlobalResource("strOperador"));
                listaDatos.Add(temp);
                foreach (var item in cAtrJson.Deserializacion(invElemento.JsonAtributosDinamicos))
                {
                    temp = new JsonObject();
                    temp.Add("Traduccion", item.NombreAtributo);
                    if (cAtributos.AtributoVisible(item.AtributoID, UsuarioID))
                    {
                        listaDatos.Add(temp);
                    }
                }
                listaPlaIds = new List<long>();
                foreach (var item in cPlaJson.Deserializacion(invElemento.JsonPlantillas))
                {
                    temp = new JsonObject();
                    temp.Add("Traduccion", item.NombreCategoria);
                    listaDatos.Add(temp);
                    listaPlaIds.Add(item.PlantillaID);
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
        public DirectResponse GetEstadoActualInvElementoJSON()
        {

            DirectResponse direct = new DirectResponse();
            InventarioElementosController cInvElementos = new InventarioElementosController();
            InventarioElementosAtributosJsonController cAtrJson = new InventarioElementosAtributosJsonController();
            InventarioElementosPlantillasJsonController cPlaJson = new InventarioElementosPlantillasJsonController();
            InventarioPlantillasAtributosJsonController cPlaAtrJson = new InventarioPlantillasAtributosJsonController();
            CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
            CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
            long lInvElemID = Convert.ToInt64(Request.QueryString["InventarioElementoID"]);
            List<long> listaPlaIds;
            long UsuarioID = ((TreeCore.Data.Usuarios)this.Session["USUARIO"]).UsuarioID;

            try
            {
                JObject oJsonInvElementoOrdenado = new JObject();
                Data.InventarioElementos invElemento = cInvElementos.GetItem("InventarioElementoID=" + lInvElemID);
                oJsonInvElementoOrdenado.Add("NumeroInventario", invElemento.NumeroInventario);
                oJsonInvElementoOrdenado.Add("Nombre", invElemento.Nombre);
                oJsonInvElementoOrdenado.Add("strUsuario", invElemento.Usuarios.NombreCompleto);
                oJsonInvElementoOrdenado.Add("strFechaModificacion", ((DateTime)invElemento.UltimaModificacionFecha).ToString(Comun.FORMATO_FECHA));
                oJsonInvElementoOrdenado.Add("strEstado", invElemento.InventarioElementosAtributosEstados.Nombre);
                oJsonInvElementoOrdenado.Add("strOperador", invElemento.Operadores.Operador);
                foreach (var item in cAtrJson.Deserializacion(invElemento.JsonAtributosDinamicos))
                {
                    if (cAtributos.AtributoVisible(item.AtributoID, UsuarioID))
                    {
                        if (oJsonInvElementoOrdenado.GetValue("Atr" + item.AtributoID) == null)
                        {
                            oJsonInvElementoOrdenado.Add("Atr" + item.AtributoID, item.TextLista);
                        }
                    }
                }
                listaPlaIds = new List<long>();
                foreach (var item in cPlaJson.Deserializacion(invElemento.JsonPlantillas))
                {
                    if (oJsonInvElementoOrdenado.GetValue("Pla" + item.InvCatConfID) == null)
                    {
                        oJsonInvElementoOrdenado.Add("Pla" + item.InvCatConfID, item.NombrePlantilla);
                    }
                    listaPlaIds.Add(item.PlantillaID);
                }
                /*InventarioElementosHistoricos historico = cInvElementos.GetHistoricoByInvElemID(invElemento.InventarioElementoID);

                hdEstadoActual.Value = historico.InventarioElementoHistoricoID.ToString();
                string[] campos = historico.HistoricoAtributosDinamicos.Trim('{', '}').Split(',');
                SepararDatosInvElemHistorico(campos, Comun.CamposValidosInventario.Last(), out string estaticos, out string dinamicos);

                JObject oJsonInvElemento = JObject.Parse(estaticos);

                // ORDENAR VALORES JSON PARA QUE CUADREN EN LA TABLA TRADUCCIONES
                JObject oJsonInvElementoOrdenado = new JObject();
                foreach (string sCampoValido in Comun.CamposValidosInventario)
                {

                    oJsonInvElementoOrdenado.Add(sCampoValido, oJsonInvElemento[sCampoValido]);
                }

                JObject oJsonAtributosDinamicos = JObject.Parse(dinamicos);
                foreach (dynamic atributo in oJsonAtributosDinamicos)
                {

                    dynamic NombreAtributo = atributo.Key;
                    dynamic Valor = atributo.Value;

                    oJsonInvElementoOrdenado.Add(NombreAtributo, Valor);
                }*/

                direct.Success = true;
                direct.Result = oJsonInvElementoOrdenado;
            }
            catch (Exception ex)
            {

                direct.Success = false;
                direct.Result = null;
                log.Error(ex.Message);
            }

            return direct;
        }

        [DirectMethod]
        public DirectResponse ExportarInventario(long[] listaIDs)
        {
            List<string> listaString;
            List<List<string>> listaListas;
            InventarioElementosController cInvElementos = new InventarioElementosController();
            InventarioElementosHistoricosController cHistoricos = new InventarioElementosHistoricosController();
            InventarioElementosAtributosJsonController cAtributosJson = new InventarioElementosAtributosJsonController();
            List<InventarioElementosAtributosJson> listaAtributosValores;
            InventarioElementosAtributosJson oAtributo;
            InventarioElementosPlantillasJsonController cPlantillasJson = new InventarioElementosPlantillasJsonController();
            List<InventarioElementosPlantillasJson> listaPlantillasValores;
            InventarioElementosPlantillasJson oPlaValor;
            CoreInventarioCategoriasAtributosCategoriasController cCategoriasVin = new CoreInventarioCategoriasAtributosCategoriasController();
            List<CoreAtributosConfiguraciones> listaAtributos;
            List<CoreInventarioCategoriasAtributosCategoriasConfiguraciones> listaPlantillas;
            CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
            JObject json, jsonAux;
            object oAux;
            List<long> listaPlaIds;
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                listaListas = new List<List<string>>();
                Data.InventarioElementos invElemento = cInvElementos.GetItem(long.Parse(hdElementoID.Value.ToString()));

                List<Data.Vw_InventarioElementosHistoricos> listaHistoricos = cHistoricos.getVwHistoricosByHistoricosIDs(listaIDs.ToList());

                if (invElemento != null)
                {
                    string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                    string fileName = "";
                    fileName = GetGlobalResource("strHistorico") + "_" + invElemento.NumeroInventario.Replace("/", "") + "_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
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
                    listaString.Add(GetGlobalResource("strFechaModificacion"));
                    listaString.Add(GetGlobalResource("strCodigo"));
                    listaString.Add(GetGlobalResource("strNombre"));
                    listaString.Add(GetGlobalResource("strEstado"));
                    listaString.Add(GetGlobalResource("strOperador"));

                    listaAtributos = cCategoriasVin.GetAtributosVisiblesByInventarioCategoriaID(invElemento.InventarioCategoriaID, Usuario.UsuarioID);

                    if (listaAtributos != null && listaAtributos.Count > 0)
                    {
                        foreach (var oAtr in listaAtributos)
                        {
                            listaString.Add(oAtr.Codigo);
                        }
                    }

                    listaPlantillas = cCategoriasVin.GetSubcategoriaPlantillasValores(invElemento.InventarioCategoriaID);

                    if (listaPlantillas != null && listaPlantillas.Count > 0)
                    {
                        foreach (var oPla in listaPlantillas)
                        {
                            listaString.Add(oPla.InventarioAtributosCategorias.InventarioAtributoCategoria);
                        }
                    }

                    listaListas.Add(listaString);

                    #endregion

                    #region ESTADO ACTUAL

                    listaString = new List<string>();
                    listaAtributosValores = cAtributosJson.Deserializacion(invElemento.JsonAtributosDinamicos);
                    listaPlantillasValores = cPlantillasJson.Deserializacion(invElemento.JsonPlantillas);

                    listaString.Add(invElemento.Usuarios.NombreCompleto);
                    listaString.Add(((DateTime)invElemento.UltimaModificacionFecha).ToString(Comun.FORMATO_FECHA));
                    listaString.Add(invElemento.NumeroInventario);
                    listaString.Add(invElemento.Nombre);
                    listaString.Add(invElemento.InventarioElementosAtributosEstados.Nombre);
                    listaString.Add(invElemento.Operadores.Operador);


                    listaPlaIds = new List<long>();
                    foreach (var item in listaPlantillas)
                    {
                        oPlaValor = (from c in listaPlantillasValores where c.InvCatConfID == item.CoreInventarioCategoriaAtributoCategoriaConfiguracionID select c).FirstOrDefault();
                        if (oPlaValor != null)
                        {
                            listaPlaIds.Add(oPlaValor.PlantillaID);
                        }
                    }
                    var listaAtrPlantillas = cPlantillas.GetPlantillas(listaPlaIds);
                    foreach (var atr in listaAtrPlantillas)
                    {
                        listaAtributosValores.AddRange(cAtributosJson.Deserializacion(atr.JsonAtributosDinamicos));
                    }

                    foreach (var item in listaAtributos)
                    {
                        oAtributo = (from c in listaAtributosValores where c.AtributoID == item.CoreAtributoConfiguracionID select c).FirstOrDefault();
                        if (oAtributo != null)
                        {
                            listaString.Add(oAtributo.TextLista);
                        }
                        else
                        {
                            listaString.Add("");
                        }
                    }


                    foreach (var item in listaPlantillas)
                    {
                        oPlaValor = (from c in listaPlantillasValores where c.InvCatConfID == item.CoreInventarioCategoriaAtributoCategoriaConfiguracionID select c).FirstOrDefault();
                        if (oPlaValor != null)
                        {
                            listaString.Add(oPlaValor.NombrePlantilla);
                            listaPlaIds.Add(oPlaValor.PlantillaID);
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
                        json = JObject.Parse(oHis.HistoricoAtributosDinamicos);
                        listaString = new List<string>();
                        listaString.Add(oHis.NombreCompleto);
                        listaString.Add(((DateTime)oHis.FechaModificacion).ToString(Comun.FORMATO_FECHA));
                        listaString.Add(oHis.NumeroElemento);
                        listaString.Add(oHis.NombreElemento);
                        if (json.GetValue("strEstado") != null)
                        {
                            listaString.Add(json.GetValue("strEstado").ToString());
                        }
                        else
                        {
                            listaString.Add("");
                        }
                        if (json.GetValue("strOperador") != null)
                        {
                            listaString.Add(json.GetValue("strOperador").ToString());
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
                                    if (json.GetValue("Atr" + oAtr.CoreAtributoConfiguracionID) != null)
                                    {
                                        listaString.Add(json.GetValue("Atr" + oAtr.CoreAtributoConfiguracionID).ToString());
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

                        if (listaPlantillas != null && listaPlantillas.Count > 0)
                        {
                            foreach (var oPla in listaPlantillas)
                            {
                                try
                                {
                                    if (json.GetValue("Pla"+oPla.CoreInventarioCategoriaAtributoCategoriaConfiguracionID) != null)
                                    {
                                        listaString.Add(json.GetValue("Pla" + oPla.CoreInventarioCategoriaAtributoCategoriaConfiguracionID).ToString());
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
                    Comun.ExportarModeloDatosDinamicoFilas(saveAs, invElemento.NumeroInventario, listaListas, 0);

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

        #region LOADER PARA NAV BAR SUPERIOR

        #endregion

        #endregion

        private void SepararDatosInvElemHistorico(string[] campos, string ultimoElementoEstatico, out string estaticos, out string dinamicos)
        {

            bool bDinamicos = false;

            List<string> camposEstaticos = new List<string>();
            List<string> camposDinamicos = new List<string>();


            foreach (string campo in campos)
            {

                if (bDinamicos)
                {

                    camposDinamicos.Add(campo);
                }
                else
                {

                    camposEstaticos.Add(campo);
                }

                if (campo.Contains(ultimoElementoEstatico))
                {

                    bDinamicos = true;
                }
            }

            string tempEstaticos = string.Join(",", camposEstaticos);
            string tempDinamicos = string.Join(",", camposDinamicos);

            estaticos = "{" + tempEstaticos + "}";
            dinamicos = "{" + tempDinamicos + "}";
        }
    }
}