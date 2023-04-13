using System;
using System.Collections.Generic;
using log4net;
using Ext.Net;
using CapaNegocio;
using System.Reflection;
using TreeCore.Page;
using TreeCore.Data;
using System.IO;

namespace TreeCore.Componentes
{
    public partial class Mapas : BaseUserControl
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private List<long> lFuncionalidades = new List<long>();

        #region GESTION PAGINAS

        protected void Page_Load(object sender, EventArgs e)
        {
            lFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
            

            SetHiddenValues();
        }

        #endregion

        #region STORES

        #region EMPLAZAMIENTOS

        protected void storeEmplazamientos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var vLista = ListaEmplazamientos();
                    SetDataSourceGridMapas(vLista);
                }
                catch (Exception ex)
                {
                    string sCodTit = Util.ExceptionHandler(ex);
                    log.Error(ex.Message);
                }
            }
        }

        public List<Vw_EmplazamientosMapasCercanos> ListaEmplazamientos()
        {
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            List<Data.Vw_EmplazamientosMapasCercanos> listaPuntos;

            try
            {
                if (Request.QueryString["EmplazamientoID"] != null)
                {
                    Hidden Longitud = this.hdLongitudMapa;
                    double dLongitud = Convert.ToDouble(Longitud.Value);

                    Hidden Latitud = this.hdLatitudMapa;
                    double dLatitud = Convert.ToDouble(Latitud.Value);

                    double dRadio = Convert.ToDouble(numRadio.Value);

                    listaPuntos = cEmplazamientos.GetEmplazamientosMapasCercanosByRadio(dLatitud, dLongitud, dRadio);
                    long emplazamientoID = long.Parse(Request.QueryString["EmplazamientoID"].ToString());
                    listaPuntos.Add(cEmplazamientos.GetEmplazamientobyID(emplazamientoID));

                }
                else
                {
                    Hidden CliID = (Hidden)X.GetCmp("hdCliID");

                    if (!CliID.Value.ToString().Equals(""))
                    {
                        listaPuntos = cEmplazamientos.GetEmplazamientosMapasCercanosByClienteID(long.Parse(CliID.Value.ToString()));
                    }
                    else
                    {
                        listaPuntos = new List<Data.Vw_EmplazamientosMapasCercanos>();
                    }
                }
            }
            catch (Exception)
            {
                listaPuntos = null;
            }

            return listaPuntos;
        }

        #endregion

        #region CLUSTERS

        protected void storeClusters_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sOff = GetGlobalResourceObject("Comun", "strOff").ToString();
                    string sSmall = GetGlobalResourceObject("Comun", "strSmall").ToString();
                    string sMedium = GetGlobalResourceObject("Comun", "strMedium").ToString();
                    string sLarge = GetGlobalResourceObject("Comun", "strLarge").ToString();

                    var Clusters = new object[] {
                        new object[] { sOff, "0", "0"},
                        new object[] { sSmall, "7", "30"},
                        new object[] { sMedium, "10", "40"},
                        new object[] { sLarge, "14", "50"}
                    };

                    storeClusters.DataSource = Clusters;


                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion


        public void SetDataSourceGridMapas(List<Vw_EmplazamientosMapasCercanos> lista)
        {
            DocumentosController cDocumentos = new DocumentosController();

            try
            {
                List<object> listaObj = new List<object>();
                foreach (Vw_EmplazamientosMapasCercanos empl in lista)
                {
                    string path = "";

                    string tempPath = TreeCore.DirectoryMapping.GetIconoOperadorTempDirectory();
                    tempPath = Path.Combine(tempPath, cDocumentos.getFileNameIconoOperador((long)empl.OperadorID));

                    if (!File.Exists(tempPath))
                    {
                        string originalPath = TreeCore.DirectoryMapping.GetIconoOperadorDirectory();
                        originalPath = Path.Combine(originalPath, cDocumentos.getFileNameIconoOperador((long)empl.OperadorID));

                        if (File.Exists(originalPath))
                        {
                            File.Copy(originalPath, tempPath);
                            path = "/" + Path.Combine(DirectoryMapping.GetIconoOperadorTempDirectoryRelative(), cDocumentos.getFileNameIconoOperador((long)empl.OperadorID));
                        }
                        else
                        {
                            path = "/ima/mapicons/ico-noOperator-map.svg";
                        }

                    }
                    else
                    {
                        path = "/" + Path.Combine(DirectoryMapping.GetIconoOperadorTempDirectoryRelative(), cDocumentos.getFileNameIconoOperador((long)empl.OperadorID));
                    }

                    listaObj.Add(new { site = empl, image = path });
                }

                storeEmplazamientos.SetData(listaObj);

                PageProxy temp = (PageProxy)storeEmplazamientos.Proxy[0];
                temp.Total = lista.Count;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void SetHiddenValues()
        {
            try
            {
                hdClienteID = (Hidden)X.GetCmp("hdCliID");

                PaisesController cPais = new PaisesController();
                Data.Paises oDato = cPais.GetDefault(long.Parse(hdClienteID.Value.ToString()));
                this.hdZoom.SetValue(oDato.Zoom);

                double Latitud;
                double Longitud;

                if (Request.QueryString["EmplazamientoID"] != null)
                {
                    long lEmplazamientoID = Convert.ToInt64(Request.QueryString["EmplazamientoID"]);
                    EmplazamientosController cEmplazamiento = new EmplazamientosController();

                    Data.Emplazamientos Emplazamiento = cEmplazamiento.GetItem(lEmplazamientoID);

                    Latitud = Emplazamiento.Latitud;
                    Longitud = Emplazamiento.Longitud;
                    this.hdEmplazamientoID.SetValue(lEmplazamientoID);
                    this.hdCercanos.SetValue(true);
                    this.hdZoom.SetValue("16");
                }
                else
                {
                    Latitud = (double)oDato.Latitud;
                    Longitud = (double)oDato.Longitud;
                    this.hdCercanos.SetValue(false);

                    this.pnFiltersMap.Hidden = true;
                }

                if (string.IsNullOrEmpty((string)this.hdLatitudMapa.Value) || string.IsNullOrEmpty((string)this.hdLongitudMapa.Value))
                {
                    this.hdLatitudMapa.SetValue(Latitud);
                    this.hdLongitudMapa.SetValue(Longitud);
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
        }

        #endregion

        //#region PAISES

        //protected void storePaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        try
        //        {
        //            List<Data.Paises> listaPaises = ListaPaises();

        //            if (listaPaises != null)
        //            {
        //                this.storePaises.DataSource = listaPaises;
        //            }

        //            //if (PaisDefecto)
        //            //{
        //            //    PaisesController cPais = new PaisesController();
        //            //    Data.Paises oPais = cPais.GetDefault(long.Parse(hdClienteID.Value.ToString()));
        //            //    this.PaisID = oPais.PaisID;
        //            //}

        //        }

        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //            string sCodTit = Util.ExceptionHandler(ex);
        //        }
        //    }
        //}

        //private List<Data.Paises> ListaPaises()
        //{
        //    List<Data.Paises> listaDatos;
        //    PaisesController cPaises = new PaisesController();

        //    try
        //    {
        //        listaDatos = cPaises.GetActivos(long.Parse(hdClienteID.Value.ToString()));
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        listaDatos = null;
        //    }

        //    return listaDatos;
        //}

        //#endregion

        //#region OPERADORES
        //protected void storeOperadoresRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        try
        //        {
        //            OperadoresController cOperadores = new OperadoresController();
        //            Hidden CliID = (Hidden)X.GetCmp("hdCliID");
        //            if (!CliID.Equals(0))
        //            {
        //                var lOperadores = cOperadores.GetOperadoresActivos(long.Parse(CliID.Value.ToString()));

        //                if (lOperadores != null)
        //                {
        //                    storeOperadores.DataSource = lOperadores;

        //                    PageProxy temp;
        //                    temp = (PageProxy)storeOperadores.Proxy[0];
        //                }

        //                cOperadores = null;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //            string codTit = Util.ExceptionHandler(ex);
        //        }
        //    }
        //}

        //#endregion

        //#region ESTADOS GLOBALES
        //protected void storeEstadosGlobalesRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        try
        //        {
        //            EstadosGlobalesController cEstadosGlobales = new EstadosGlobalesController();
        //            Hidden CliID = (Hidden)X.GetCmp("hdCliID");

        //            if (!CliID.Equals(0))
        //            {
        //                var lEstadosGlobales = cEstadosGlobales.GetEstadosGlobalesActivos(long.Parse(CliID.Value.ToString()));

        //                if (lEstadosGlobales != null)
        //                {
        //                    storeEstadosGlobales.DataSource = lEstadosGlobales;

        //                    PageProxy temp;
        //                    temp = (PageProxy)storeEstadosGlobales.Proxy[0];
        //                }

        //                cEstadosGlobales = null;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //            string codTit = Util.ExceptionHandler(ex);
        //        }
        //    }
        //}

        //#endregion

        //#region CATEGORIAS SITIOS
        //protected void storeCategoriasSitiosRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        try
        //        {
        //            EmplazamientosCategoriasSitiosController cCategoriasSitios = new EmplazamientosCategoriasSitiosController();
        //            Hidden CliID = (Hidden)X.GetCmp("hdCliID");

        //            if (!CliID.Equals(0))
        //            {
        //                var lCategoriasSitios = cCategoriasSitios.GetCategoriasSitiosActivas(long.Parse(CliID.Value.ToString()));

        //                if (lCategoriasSitios != null)
        //                {
        //                    storeCategoriasSitios.DataSource = lCategoriasSitios;

        //                    PageProxy temp;
        //                    temp = (PageProxy)storeCategoriasSitios.Proxy[0];
        //                }

        //                cCategoriasSitios = null;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //            string codTit = Util.ExceptionHandler(ex);
        //        }
        //    }
        //}

        //#endregion

        //#region EMPLAZAMIENTOS TIPOS
        //protected void storeEmplazamientosTiposRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        try
        //        {
        //            EmplazamientosTiposController cEmplazamientosTipos = new EmplazamientosTiposController();
        //            Hidden CliID = (Hidden)X.GetCmp("hdCliID");

        //            if (!CliID.Equals(0))
        //            {
        //                var lTipos = cEmplazamientosTipos.GetEmplazamientosTiposActivos(long.Parse(CliID.Value.ToString()));

        //                if (lTipos != null)
        //                {
        //                    storeEmplazamientosTipos.DataSource = lTipos;

        //                    PageProxy temp;
        //                    temp = (PageProxy)storeEmplazamientosTipos.Proxy[0];
        //                }

        //                cEmplazamientosTipos = null;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //            string codTit = Util.ExceptionHandler(ex);
        //        }
        //    }
        //}

        //#endregion

        //#region EMPLAZAMIENTOS TAMAÑOS
        //protected void storeEmplazamientosTamanosRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        try
        //        {
        //            EmplazamientosTamanosController cEmplazamientosTamanos = new EmplazamientosTamanosController();
        //            Hidden CliID = (Hidden)X.GetCmp("hdCliID");

        //            if (!CliID.Equals(0))
        //            {
        //                var lTamanos = cEmplazamientosTamanos.GetEmplazamientosTamanosActivos(long.Parse(CliID.Value.ToString()));

        //                if (lTamanos != null)
        //                {
        //                    storeEmplazamientosTamanos.DataSource = lTamanos;

        //                    PageProxy temp;
        //                    temp = (PageProxy)storeEmplazamientosTamanos.Proxy[0];
        //                }

        //                cEmplazamientosTamanos = null;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //            string codTit = Util.ExceptionHandler(ex);
        //        }
        //    }
        //}

        //#endregion

        //#region CLUSTERS

        //protected void storeClustersRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        try
        //        {
        //            var Clusters = new object[]        {

        //                new object[] { "OFF", "0", "0"},
        //                new object[] { "SMALL", "7", "30"},
        //                new object[] { "MEDIUM", "10", "40"},
        //                new object[] { "LARGE", "14", "50"}

        //            };

        //            storeClusters.DataSource = Clusters;

        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //            string codTit = Util.ExceptionHandler(ex);
        //        }
        //    }
        //}

        //#endregion

    }


}
