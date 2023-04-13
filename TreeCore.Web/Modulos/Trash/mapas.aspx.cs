using Ext.Net;
using System;
using System.Collections.Generic;
using CapaNegocio;
using System.Linq;
using Ext.Net.Utilities;
using System.Data;
using System.Web.UI.WebControls;
using log4net;
using Tree.Linq.GenericExtensions;

namespace TreeCore.PaginasComunes
{
    public partial class Mapas : TreeCore.Page.BasePageExtNet
    {
        private ILog log = LogManager.GetLogger("");
        private List<long> lFuncionalidades = new List<long>();

        #region Gestión Página (Init/Load)

        protected void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack & !RequestManager.IsAjaxRequest)
            {
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

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!RequestManager.IsAjaxRequest)
            {
                lFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
            }
        }

        #endregion

        #region Stores

        #region Store Emplazamientos

        protected void storeEmplazamientosRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string operadoresID = "";
                    string estadosglobalesID = "";
                    string categoriassitiosID = "";
                    string tiposemplazamientosID = "";
                    string tamanosID = "";

                    if (!e.IsNull())
                    {
                        operadoresID = e.Parameters["Operadores"];
                        estadosglobalesID = e.Parameters["EstadosGlobales"];
                        categoriassitiosID = e.Parameters["CategoriasSitios"];
                        tiposemplazamientosID = e.Parameters["TiposEmplazamientos"];
                        tamanosID = e.Parameters["Tamanos"];
                    }

                    List<Data.Sp_EmplazamientosCercanosFotosMapa_GetResult> vLista = null;

                    string sEmplazamientoID = Request.QueryString["EmplazamientoID"];
                    if (sEmplazamientoID != null && sEmplazamientoID != "")
                    {

                        EmplazamientosController cEmp = new EmplazamientosController();
                        Data.Emplazamientos emp = cEmp.GetItem(Convert.ToInt64(sEmplazamientoID));

                        double dLatitud = emp.Latitud;
                        double dLongitud= emp.Longitud;

                        vLista = ListaEmplazamientos(operadoresID, estadosglobalesID, categoriassitiosID, tiposemplazamientosID, tamanosID, dLatitud, dLongitud);
                    }
                    else
                    {
                        vLista = ListaEmplazamientos(operadoresID, estadosglobalesID, categoriassitiosID, tiposemplazamientosID, tamanosID);
                    }

                    if (vLista != null)
                    {
                        storeEmplazamientos.DataSource = vLista;
                        PageProxy temp;
                        temp = (PageProxy)storeEmplazamientos.Proxy[0];
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

        private List<Data.Sp_EmplazamientosCercanosFotosMapa_GetResult> ListaEmplazamientos(string operadoresID, string estadosglobalesID, string categoriassitiosID, string tiposemplazamientosID, string tamanosID, double Latitud = 37.457981, double Longitud = -6.057108)
        {
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            List<Data.Sp_EmplazamientosCercanosFotosMapa_GetResult> puntos = new List<Data.Sp_EmplazamientosCercanosFotosMapa_GetResult>();

            try
            {
                int Radio = Convert.ToInt32(numRadio.Number);
                puntos = cEmplazamientos.GetAllPuntosCercanosConFotos(Latitud, Longitud, Radio).ToList();

                if (operadoresID.IsNotEmpty())
                {
                    var lID = operadoresID.Split(',').Select(long.Parse).ToList();
                    puntos = (from c in puntos where c.OperadorID != null && lID.Contains((long)c.OperadorID) select c).ToList();
                }

                if (estadosglobalesID.IsNotEmpty())
                {
                    var lID = estadosglobalesID.Split(',').Select(long.Parse).ToList();
                    puntos = (from c in puntos where c.EstadoGlobalID != null && lID.Contains((long)c.EstadoGlobalID) select c).ToList();
                }

                if (categoriassitiosID.IsNotEmpty())
                {
                    var lID = categoriassitiosID.Split(',').Select(long.Parse).ToList();
                    puntos = (from c in puntos where c.CategoriaEmplazamientoID != null && lID.Contains((long)c.CategoriaEmplazamientoID) select c).ToList();
                }

                if (tiposemplazamientosID.IsNotEmpty())
                {
                    var lID = tiposemplazamientosID.Split(',').Select(long.Parse).ToList();
                    puntos = (from c in puntos where c.EmplazamientoTipoID != null && lID.Contains((long)c.EmplazamientoTipoID) select c).ToList();
                }

                if (tamanosID.IsNotEmpty())
                {
                    var lID = tamanosID.Split(',').Select(long.Parse).ToList();
                    puntos = (from c in puntos where c.EmplazamientoTamanoID != null && lID.Contains((long)c.EmplazamientoTamanoID) select c).ToList();
                }

            }
            catch (Exception)
            {
                puntos = null;
            }

            return puntos;
        }


        #endregion

        #region Store Clientes
        protected void storeClientesRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    ClientesController cClientes = new ClientesController();

                    if (ClienteID != null)
                    {
                        cmbClientes.SelectedItem.Value = ClienteID.Value.ToString();
                        cmbClientes.Hidden = true;
                    }
                    else
                    {
                        List<Data.Clientes> lClientes = new List<Data.Clientes>();
                        lClientes = cClientes.GetItemsList<Data.Clientes>("Activo", "Cliente");

                        if (lClientes != null)
                        {
                            storeClientes.DataSource = lClientes;

                            PageProxy temp;
                            temp = (PageProxy)storeClientes.Proxy[0];
                        }

                        cClientes = null;
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

        #region Store Operadores
        protected void storeOperadoresRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    OperadoresController cOperadores = new OperadoresController();

                    long CliID = 0;
                    if (ClienteID != null)
                    {
                        CliID = ClienteID.Value;
                    }
                    else if (cmbClientes.SelectedItem.Value.IsNotEmpty())
                    {
                        CliID = Convert.ToInt32(cmbClientes.SelectedItem.Value, null);
                    }

                    if (!CliID.Equals(0))
                    {
                        var lOperadores = cOperadores.GetOperadoresActivos(CliID);

                        if (lOperadores != null)
                        {
                            storeOperadores.DataSource = lOperadores;

                            PageProxy temp;
                            temp = (PageProxy)storeOperadores.Proxy[0];
                        }

                        cOperadores = null;
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

        #region Store Estados Globales
        protected void storeEstadosGlobalesRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    EstadosGlobalesController cEstadosGlobales = new EstadosGlobalesController();

                    long CliID = 0;
                    if (ClienteID != null)
                    {
                        CliID = ClienteID.Value;
                    }
                    else if (cmbClientes.SelectedItem.Value.IsNotEmpty())
                    {
                        CliID = Convert.ToInt32(cmbClientes.SelectedItem.Value, null);
                    }

                    if (!CliID.Equals(0))
                    {
                        var lEstadosGlobales = cEstadosGlobales.GetEstadosGlobalesActivos(CliID);

                        if (lEstadosGlobales != null)
                        {
                            storeEstadosGlobales.DataSource = lEstadosGlobales;

                            PageProxy temp;
                            temp = (PageProxy)storeEstadosGlobales.Proxy[0];
                        }

                        cEstadosGlobales = null;
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

        #region Store Categorias Sitios
        protected void storeCategoriasSitiosRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    EmplazamientosCategoriasSitiosController cCategoriasSitios = new EmplazamientosCategoriasSitiosController();

                    long CliID = 0;
                    if (ClienteID != null)
                    {
                        CliID = ClienteID.Value;
                    }
                    else if (cmbClientes.SelectedItem.Value.IsNotEmpty())
                    {
                        CliID = Convert.ToInt32(cmbClientes.SelectedItem.Value, null);
                    }

                    if (!CliID.Equals(0))
                    {
                        var lCategoriasSitios = cCategoriasSitios.GetCategoriasSitiosActivas(CliID);

                        if (lCategoriasSitios != null)
                        {
                            storeCategoriasSitios.DataSource = lCategoriasSitios;

                            PageProxy temp;
                            temp = (PageProxy)storeCategoriasSitios.Proxy[0];
                        }

                        cCategoriasSitios = null;
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

        #region Store Emplazamientos Tipos
        protected void storeEmplazamientosTiposRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    EmplazamientosTiposController cEmplazamientosTipos = new EmplazamientosTiposController();

                    long CliID = 0;
                    if (ClienteID != null)
                    {
                        CliID = ClienteID.Value;
                    }
                    else if (cmbClientes.SelectedItem.Value.IsNotEmpty())
                    {
                        CliID = Convert.ToInt32(cmbClientes.SelectedItem.Value, null);
                    }

                    if (!CliID.Equals(0))
                    {
                        var lTipos = cEmplazamientosTipos.GetEmplazamientosTiposActivos(CliID);

                        if (lTipos != null)
                        {
                            storeEmplazamientosTipos.DataSource = lTipos;

                            PageProxy temp;
                            temp = (PageProxy)storeEmplazamientosTipos.Proxy[0];
                        }

                        cEmplazamientosTipos = null;
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

        #region Store Emplazamientos Tamaños
        protected void storeEmplazamientosTamanosRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    EmplazamientosTamanosController cEmplazamientosTamanos = new EmplazamientosTamanosController();

                    long CliID = 0;
                    if (ClienteID != null)
                    {
                        CliID = ClienteID.Value;
                    }
                    else if (cmbClientes.SelectedItem.Value.IsNotEmpty())
                    {
                        CliID = Convert.ToInt32(cmbClientes.SelectedItem.Value, null);
                    }

                    if (!CliID.Equals(0))
                    {
                        var lTamanos = cEmplazamientosTamanos.GetEmplazamientosTamanosActivos(CliID);

                        if (lTamanos != null)
                        {
                            storeEmplazamientosTamanos.DataSource = lTamanos;

                            PageProxy temp;
                            temp = (PageProxy)storeEmplazamientosTamanos.Proxy[0];
                        }

                        cEmplazamientosTamanos = null;
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

        #region Store Clusters

        protected void storeClustersRefresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var Clusters = new object[]        {

                        new object[] { "OFF", "0", "0"},
                        new object[] { "SMALL", "7", "30"},
                        new object[] { "MEDIUM", "10", "40"},
                        new object[] { "LARGE", "14", "50"}

                    };

                    storeClusters.DataSource = Clusters;

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

        #endregion

    }
}