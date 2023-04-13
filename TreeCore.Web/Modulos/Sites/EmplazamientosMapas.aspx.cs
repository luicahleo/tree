using System;
using System.Collections.Generic;
using log4net;
using Ext.Net;
using CapaNegocio;
using System.Reflection;
using TreeCore.Page;
using TreeCore.Data;
using System.IO;
using System.Linq;

namespace TreeCore.ModGlobal.pages
{
    public partial class EmplazamientosMapas : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private List<long> lFuncionalidades = new List<long>();
        Data.Usuarios oUser;

        #region GESTION PAGINAS
        protected void Page_Load(object sender, EventArgs e)
        {
            lFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
            
        }

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                if (Request.QueryString["cliID"] != null)
                {
                    hdCliID.Value = Request.QueryString["cliID"];
                }
                if (Request.QueryString["StringBuscador"] != null)
                {
                    hdStringBuscador.Value = Request.QueryString["StringBuscador"];
                }
                if (Request.QueryString["IDEmplazamientoBuscador"] != null)
                {
                    hdIDEmplazamientoBuscador.Value = Request.QueryString["IDEmplazamientoBuscador"];
                }

                Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
                UsuariosController cUsuarios = new UsuariosController();
                if (oUsuario != null)
                {
                    oUser = cUsuarios.GetItem(oUsuario.UsuarioID);
                }

            }

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
                    int total;
                    EmplazamientosController cEmplazamientos = new EmplazamientosController();
                    List<string> listaVacia = new List<string>();
                    List<JsonObject> lista = cEmplazamientos.AplicarFiltroInterno(true, hdFiltrosAplicados.Value.ToString(), -1, -1, out total, null, hdBounds.Value.ToString(), hdCliID.Value.ToString(), hdStringBuscador, hdIDEmplazamientoBuscador, sResultadoKPIid, false, null, "");

                    if (lista.Count > 250)
                    {
                        lista = lista.OrderBy(x => Guid.NewGuid()).Take(250).ToList();
                    }

                    storeEmplazamientos.DataSource = lista;
                    storeEmplazamientos.DataBind();

                    PageProxy temp = (PageProxy)storeEmplazamientos.Proxy[0];
                    temp.Total = lista.Count;
                }
                catch (Exception ex)
                {
                    string sCodTit = Util.ExceptionHandler(ex);
                    log.Error(ex.Message);
                }
            }
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

        #endregion


        [DirectMethod]
        public DirectResponse SetHiddenValues()
        {
            DirectResponse direct = new DirectResponse();

            PaisesController cPais = new PaisesController();
            OperadoresController cOperadores = new OperadoresController();

            double Latitud;
            double Longitud;

            try
            {
                Data.Paises oDato = cPais.GetDefault(long.Parse(hdCliID.Value.ToString()));

                if (oDato != null)
                {
                    this.hdZoom.SetValue(oDato.Zoom);
                }
                else
                {
                    this.hdZoom.SetValue(6);
                }

                hdOperadoresConIcono.SetValue(cOperadores.getEntidadesOperadoresConIcono(long.Parse(hdCliID.Value.ToString())));
                hdpath.SetValue(Path.Combine(DirectoryMapping.GetIconoOperadorTempDirectoryRelative()));

                if (Request.QueryString["EmplazamientoID"] != null)
                {
                    long lEmplazamientoID = Convert.ToInt64(Request.QueryString["EmplazamientoID"]);
                    EmplazamientosController cEmplazamiento = new EmplazamientosController();

                    Data.Emplazamientos Emplazamiento = cEmplazamiento.GetItem(lEmplazamientoID);

                    Latitud = Emplazamiento.Latitud;
                    Longitud = Emplazamiento.Longitud;
                    this.hdCercanos.SetValue(false);
                }
                else
                {
                    Latitud = (double)oDato.Latitud;
                    Longitud = (double)oDato.Longitud;
                    this.hdCercanos.SetValue(false);
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
                direct.Result = "";
                return direct;
            }

            direct.Result = "";
            direct.Success = true;
            return direct;
        }


    }
}