using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using TreeCore.Data;

namespace TreeCore.ModGlobal.pages
{
    public partial class DocumentosHistoricos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        private static readonly List<string> lRecursosTraducciones = new List<string>()
        {
            // PRINCIPAL
            "strCodigoObjeto",
            "strNombreObjeto",
            "strNombre",
            "strExtension",
            "strCreador",
            "strFecha",
            "strDocumentosEstados",
            "strVersion",
            "strTamano",
            //"strSlug",
            "strDocumentoTipo",
            "strDescripcion",
            "strActivo"

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

        //protected override void OnPreRenderComplete(EventArgs e)
        //{
        //    base.OnPreRenderComplete(e);
        //    if (!IsPostBack && !RequestManager.IsAjaxRequest)
        //    {
        //        List<Data.Vw_Funcionalidades> listFun = ((List<Data.Vw_Funcionalidades>)(this.Session["LISTAFUNCIONALIDADES"]));

        //        btnRefrescar.Hidden = false;
        //        btnDescargar.Hidden = true;

        //        if (Comun.ComprobarFuncionalidadSoloLectura(System.IO.Path.GetFileName(Request.Url.AbsolutePath), listFun))
        //        {
        //            btnRefrescar.Hidden = false;
        //            btnDescargar.Hidden = true;
        //        }
        //        else if (Comun.ComprobarFuncionalidadTotal(System.IO.Path.GetFileName(Request.Url.AbsolutePath), listFun))
        //        {
        //            btnRefrescar.Hidden = false;
        //            btnDescargar.Hidden = false;
        //        }
        //        if (Comun.ComprobarFuncionalidadDescarga(System.IO.Path.GetFileName(Request.Url.AbsolutePath), listFun))
        //        {
        //            btnDescargar.Hidden = false;
        //        }
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            lbltituloPrincipal.Text = GetGlobalResource("strHistorico") + " " + GetGlobalResource("strDocumento");

            if (this.Session["LOCALE"] != null)
            {
                hdLocale.Value = this.Session["LOCALE"].ToString();
            }

            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
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
                        PageProxy temp = (PageProxy)storePrincipal.Proxy[0];
                        temp.Total = lista.Count;
                    }
                }
                catch (Exception ex)
                {

                    log.Error(ex.Message);
                }
            }
        }

        private List<Vw_HistoricoCoreDocumentos> ListaPrincipal()
        {

            List<Vw_HistoricoCoreDocumentos> listaDatos;
            HistoricoCoreDocumentosController cDocumentosHistoricos = new HistoricoCoreDocumentosController();

            try
            {

                string sDocumentoID = Request.QueryString["DocumentoID"];
                listaDatos = cDocumentosHistoricos.GetHistoricoByDocumentoID(long.Parse(sDocumentoID));
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

                foreach (string recurso in lRecursosTraducciones)
                {

                    JsonObject temp = new JsonObject();
                    temp.Add("Traduccion", GetGlobalResource(recurso));
                    listaDatos.Add(temp);
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
        public DirectResponse GetEstadoActualDocumentoJSON()
        {

            DirectResponse direct = new DirectResponse();
            DocumentosController cDocumentos = new DocumentosController();

            long lDocumentoID = Convert.ToInt64(Request.QueryString["DocumentoID"]);

            try
            {
                Documentos Documento = cDocumentos.getDocIsUltimaVersion(lDocumentoID);
                Vw_CoreDocumentos vwDocumento = cDocumentos.GetItem<Vw_CoreDocumentos>(Documento.DocumentoID);

                JObject oJsonDocumento = JObject.Parse(Comun.ObjectToJSON(vwDocumento, "Documentos").ToString());

                // ORDENAR VALORES JSON PARA QUE CUADREN EN LA TABLA TRADUCCIONES
                JObject oJsonDocumentoOrdenado = new JObject();
                foreach (string sCampoValido in Comun.CamposValidosDocumentos)
                {
                    oJsonDocumentoOrdenado.Add(sCampoValido, oJsonDocumento[sCampoValido]);
                }

                direct.Success = true;
                direct.Result = oJsonDocumentoOrdenado;
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