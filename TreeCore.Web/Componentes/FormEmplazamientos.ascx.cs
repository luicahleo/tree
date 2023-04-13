using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TreeCore.Clases;
using TreeCore.Page;

namespace TreeCore.Componentes
{
    public partial class FormEmplazamientos : BaseUserControl
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        List<Componentes.GestionCategoriasAtributos> listaCategorias;

        #region OBTENER DATOS

        public long? OperadorID
        {
            get
            {
                if (cmbOperadores.SelectedItem.Value != null && cmbOperadores.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbOperadores.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.cmbOperadores.SetValue(value);
                this.hdOperador.SetValue(value);
                this.cmbOperadores.ShowTrigger(0);
            }
        }

        public long? EstadoGlobalID
        {
            get
            {
                if (cmbEstadosGlobales.SelectedItem.Value != null && cmbEstadosGlobales.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbEstadosGlobales.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.cmbEstadosGlobales.SetValue(value);
                this.hdEstadoGlobal.SetValue(value);
                this.cmbEstadosGlobales.ShowTrigger(0);
            }
        }

        public long? EmplazamientoCategoriaSitioID
        {
            get
            {
                if (cmbCategorias.SelectedItem.Value != null && cmbCategorias.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbCategorias.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.cmbCategorias.SetValue(value);
                this.hdCategoria.SetValue(value);
                this.cmbCategorias.ShowTrigger(0);
            }
        }

        public long? EmplazamientoTamanoID
        {
            get
            {
                if (cmbTamanos.SelectedItem.Value != null && cmbTamanos.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbTamanos.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    this.cmbTamanos.SetValue(value);
                    this.hdTamanos.SetValue(value);
                    this.cmbTamanos.ShowTrigger(0);
                }
                else
                {
                    this.hdTamanos.SetValue(0);
                    this.cmbTamanos.ClearValue();
                }
            }
        }

        public long? EmplazamientoTipoEstructuraID
        {
            get
            {
                if (cmbTiposEstructuras.SelectedItem.Value != null && cmbTiposEstructuras.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbTiposEstructuras.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    this.cmbTiposEstructuras.SetValue(value);
                    this.hdTipoEstructura.SetValue(value);
                    this.cmbTiposEstructuras.ShowTrigger(0);
                }
                else
                {
                    this.hdTipoEstructura.SetValue(0);
                    this.cmbTiposEstructuras.ClearValue();
                }
            }
        }

        public long? EmplazamientoTipoEdificioID
        {
            get
            {
                if (cmbTiposEdificios.SelectedItem.Value != null && cmbTiposEdificios.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbTiposEdificios.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.cmbTiposEdificios.SetValue(value);
                this.hdTipoEdificio.SetValue(value);
                this.cmbTiposEdificios.ShowTrigger(0);
            }
        }

        public long? MonedaID
        {
            get
            {
                if (cmbMonedas.SelectedItem.Value != null && cmbMonedas.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbMonedas.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.cmbMonedas.SetValue(value);
                this.hdMoneda.SetValue(value);
                this.cmbMonedas.ShowTrigger(0);
            }
        }

        public long? EmplazamientoTipoID
        {
            get
            {
                if (cmbTipos.SelectedItem.Value != null && cmbTipos.SelectedItem.Value.ToString() != "")
                {
                    return long.Parse(this.cmbTipos.SelectedItem.Value);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.cmbTipos.SetValue(value);
                this.hdTipos.SetValue(value);
                this.cmbTipos.ShowTrigger(0);
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            UsuariosController cUsuarios = new UsuariosController();
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
            Data.Usuarios oUser = cUsuarios.GetItem(oUsuario.UsuarioID);
            PintarCategorias(false);
        }

        #region STORES

        #region OPERADORES

        protected void storeOperadores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    EntidadesController cOperadores = new EntidadesController();
                    List<Data.Entidades> listaOperadores = ListaOperadores();

                    if (listaOperadores != null)
                    {
                        this.storeOperadores.DataSource = listaOperadores;
                        this.storeOperadores.DataBind();
                    }

                    string sOperador;
                    if (hdOperador.Value != null)
                    {
                        sOperador = hdOperador.Value.ToString();
                    }
                    else
                    {
                        sOperador = "";
                    }

                    if (sOperador.Equals(""))
                    {
                        this.OperadorID = cOperadores.getOperadorCliente(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Entidades> ListaOperadores()
        {
            List<Data.Entidades> listaOperadores;
            EntidadesController cOperadores = new EntidadesController();

            try
            {
                listaOperadores = cOperadores.getEntidadesOperadores(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaOperadores = null;
            }

            return listaOperadores;
        }

        #endregion

        #region ESTADOS GLOBALES

        protected void storeEstadosGlobales_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EstadosGlobales> listaEstadosGlobales = ListaEstadosGlobales();

                    if (listaEstadosGlobales != null)
                    {
                        this.storeEstadosGlobales.DataSource = listaEstadosGlobales;
                        this.storeEstadosGlobales.DataBind();
                    }

                    string sEstadoGlobal;
                    if (hdEstadoGlobal.Value != null)
                    {
                        sEstadoGlobal = hdEstadoGlobal.Value.ToString();
                    }
                    else
                    {
                        sEstadoGlobal = "";
                    }

                    if (sEstadoGlobal.Equals(""))
                    {
                        EstadosGlobalesController cEstados = new EstadosGlobalesController();

                        long lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

                        if (lCliID != 0)
                        {
                            Data.EstadosGlobales oEstado = cEstados.GetDefault(lCliID);

                            if (oEstado != null)
                            {
                                this.EstadoGlobalID = oEstado.EstadoGlobalID;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.EstadosGlobales> ListaEstadosGlobales()
        {
            List<Data.EstadosGlobales> listaEstadosGlobales;
            EstadosGlobalesController cEstados = new EstadosGlobalesController();

            try
            {
                listaEstadosGlobales = cEstados.GetEstadosGlobalesActivos(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaEstadosGlobales = null;
            }

            return listaEstadosGlobales;
        }

        #endregion

        #region CATEGORIAS

        protected void storeCategorias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EmplazamientosCategoriasSitios> listaCategorias = ListaCategorias();

                    if (listaCategorias != null)
                    {
                        this.storeCategorias.DataSource = listaCategorias;
                        this.storeCategorias.DataBind();
                    }

                    string sCategoria;
                    if (hdCategoria.Value != null)
                    {
                        sCategoria = hdCategoria.Value.ToString();
                    }
                    else
                    {
                        sCategoria = "";
                    }

                    if (sCategoria.Equals(""))
                    {
                        EmplazamientosCategoriasSitiosController cCategorias = new EmplazamientosCategoriasSitiosController();

                        long lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

                        if (lCliID != 0)
                        {
                            Data.EmplazamientosCategoriasSitios oCategoria = cCategorias.GetDefault(lCliID);

                            if (oCategoria != null)
                            {
                                this.EmplazamientoCategoriaSitioID = oCategoria.EmplazamientoCategoriaSitioID;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.EmplazamientosCategoriasSitios> ListaCategorias()
        {
            List<Data.EmplazamientosCategoriasSitios> listaCategorias;
            EmplazamientosCategoriasSitiosController cCategorias = new EmplazamientosCategoriasSitiosController();

            try
            {
                listaCategorias = cCategorias.GetCategoriasSitiosActivas(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaCategorias = null;
            }

            return listaCategorias;
        }

        #endregion

        #region TIPOS

        protected void storeTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EmplazamientosTipos> listaTipos = ListaTipos();

                    if (listaTipos != null)
                    {
                        this.storeTipos.DataSource = listaTipos;
                        this.storeTipos.DataBind();
                    }

                    string sTipo;
                    if (hdTipos.Value != null)
                    {
                        sTipo = hdTipos.Value.ToString();
                    }
                    else
                    {
                        sTipo = "";
                    }

                    if (sTipo.Equals(""))
                    {
                        EmplazamientosTiposController cTipos = new EmplazamientosTiposController();

                        long lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

                        if (lCliID != 0)
                        {
                            Data.EmplazamientosTipos oTipo = cTipos.GetDefault(lCliID);

                            if (oTipo != null)
                            {
                                this.EmplazamientoTipoID = oTipo.EmplazamientoTipoID;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.EmplazamientosTipos> ListaTipos()
        {
            List<Data.EmplazamientosTipos> listaTipos;
            EmplazamientosTiposController cTipos = new EmplazamientosTiposController();

            try
            {
                listaTipos = cTipos.GetEmplazamientosTiposActivos(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTipos = null;
            }

            return listaTipos;
        }

        #endregion

        #region TAMAÑOS

        protected void storeTamanos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EmplazamientosTamanos> listaTamanos = ListaTamanos();

                    if (listaTamanos != null)
                    {
                        this.storeTamanos.DataSource = listaTamanos;
                        this.storeTamanos.DataBind();
                    }

                    string sTamano;
                    if (hdTamanos.Value != null)
                    {
                        sTamano = hdTamanos.Value.ToString();
                    }
                    else
                    {
                        sTamano = "";
                    }

                    if (sTamano.Equals(""))
                    {
                        EmplazamientosTamanosController cTamanos = new EmplazamientosTamanosController();

                        long lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

                        if (lCliID != 0)
                        {
                            Data.EmplazamientosTamanos oTamano = cTamanos.GetDefault(lCliID);

                            if (oTamano != null)
                            {
                                this.EmplazamientoTamanoID = oTamano.EmplazamientoTamanoID;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.EmplazamientosTamanos> ListaTamanos()
        {
            List<Data.EmplazamientosTamanos> listaTamanos;
            EmplazamientosTamanosController cTamanos = new EmplazamientosTamanosController();

            try
            {
                listaTamanos = cTamanos.GetEmplazamientosTamanosActivos(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTamanos = null;
            }

            return listaTamanos;
        }

        #endregion

        #region TIPOS ESTRUCTURAS

        protected void storeTiposEstructuras_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EmplazamientosTiposEstructuras> listaTiposEstructuras = ListaTiposEstructuras();

                    if (listaTiposEstructuras != null)
                    {
                        this.storeTiposEstructuras.DataSource = listaTiposEstructuras;
                        this.storeTiposEstructuras.DataBind();
                    }

                    string sTipoEstructura;
                    if (hdTipoEstructura.Value != null)
                    {
                        sTipoEstructura = hdTipoEstructura.Value.ToString();
                    }
                    else
                    {
                        sTipoEstructura = "";
                    }

                    if (sTipoEstructura.Equals(""))
                    {
                        EmplazamientosTiposEstructurasController cEstructuras = new EmplazamientosTiposEstructurasController();

                        long lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

                        if (lCliID != 0)
                        {
                            Data.EmplazamientosTiposEstructuras oEstructura = cEstructuras.GetDefault(lCliID);

                            if (oEstructura != null)
                            {
                                this.EmplazamientoTipoEstructuraID = oEstructura.EmplazamientoTipoEstructuraID;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.EmplazamientosTiposEstructuras> ListaTiposEstructuras()
        {
            List<Data.EmplazamientosTiposEstructuras> listaTiposEstructuras;
            EmplazamientosTiposEstructurasController cTiposEstructuras = new EmplazamientosTiposEstructurasController();

            try
            {
                listaTiposEstructuras = cTiposEstructuras.GetActivos(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTiposEstructuras = null;
            }

            return listaTiposEstructuras;
        }

        #endregion

        #region TIPOS EDIFICIOS

        protected void storeTiposEdificios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EmplazamientosTiposEdificios> listaTiposEdificios = ListaTiposEdificios();

                    if (listaTiposEdificios != null)
                    {
                        this.storeTiposEdificios.DataSource = listaTiposEdificios;
                        this.storeTiposEdificios.DataBind();
                    }

                    string sTipoEdificio;
                    if (hdTipoEdificio.Value != null)
                    {
                        sTipoEdificio = hdTipoEdificio.Value.ToString();
                    }
                    else
                    {
                        sTipoEdificio = "";
                    }

                    if (sTipoEdificio.Equals(""))
                    {
                        EmplazamientosTiposEdificiosController cEdificios = new EmplazamientosTiposEdificiosController();

                        long lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

                        if (lCliID != 0)
                        {
                            Data.EmplazamientosTiposEdificios oEdificio = cEdificios.GetDefault(lCliID);

                            if (oEdificio != null)
                            {
                                this.EmplazamientoTipoEdificioID = oEdificio.EmplazamientoTipoEdificioID;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.EmplazamientosTiposEdificios> ListaTiposEdificios()
        {
            List<Data.EmplazamientosTiposEdificios> listaTiposEdificios;
            EmplazamientosTiposEdificiosController cTiposEdificios = new EmplazamientosTiposEdificiosController();

            try
            {
                listaTiposEdificios = cTiposEdificios.GetActivos(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTiposEdificios = null;
            }

            return listaTiposEdificios;
        }

        #endregion

        #region MONEDAS

        protected void storeMonedas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Monedas> listaMonedas = ListaMonedas();

                    if (listaMonedas != null)
                    {
                        this.storeMonedas.DataSource = listaMonedas;
                        this.storeMonedas.DataBind();
                    }

                    string sMoneda;
                    if (hdMoneda.Value != null)
                    {
                        sMoneda = hdMoneda.Value.ToString();
                    }
                    else
                    {
                        sMoneda = "";
                    }

                    if (sMoneda.Equals(""))
                    {
                        MonedasController cMonedas = new MonedasController();

                        long lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

                        if (lCliID != 0)
                        {
                            Data.Monedas oMoneda = cMonedas.GetDefault(lCliID);

                            if (oMoneda != null)
                            {
                                this.MonedaID = oMoneda.MonedaID;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Monedas> ListaMonedas()
        {
            List<Data.Monedas> listaMonedas;
            MonedasController cMonedas = new MonedasController();

            try
            {
                listaMonedas = cMonedas.GetActivosCliente(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaMonedas = null;
            }

            return listaMonedas;
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool SobrescribirEdicion)

        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            List<Object> listaAtributos = new List<object>();
            MunicipiosController cMunicipios = new MunicipiosController();
            Data.Emplazamientos oDato;
            UsuariosController cUsuarios = new UsuariosController();
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];

            HistoricosCoreEmplazamientosController cHistorico = new HistoricosCoreEmplazamientosController();
            Data.HistoricosCoreEmplazamientos DatoHistorico;

            long lCliID = 0;
            long lMunicipioID = 0;
            long? lPaisID = 0;
            direct.Success = true;
            direct.Result = "";

            try
            {

                lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());
                Data.Usuarios oUser = cUsuarios.GetItem(oUsuario.UsuarioID);

                if (hdEmplazamientoID.Value != null && hdEmplazamientoID.Value.ToString() != "")
                {
                    oDato = cEmplazamientos.GetItem(long.Parse(hdEmplazamientoID.Value.ToString()));

                    DatoHistorico = cHistorico.getHistoricoByID(oDato.EmplazamientoID);


                    if (oDato.Codigo != txtCodigo.Text || oDato.OperadorID != long.Parse(cmbOperadores.SelectedItem.Value))
                    {
                        if (!cEmplazamientos.EmplazamientoDuplicadoCodigoOperador(long.Parse(cmbOperadores.SelectedItem.Value), txtCodigo.Text))
                        {
                            if (SobrescribirEdicion || (hdHistoricoEmplazamiento.Value.ToString() == DatoHistorico.HistoricoCoreEmplazamientoID.ToString()))
                            {
                                #region UPDATE
                                DateTime? dateDeactivationSave = null;
                                if (dateDeactivation.SelectedDate > DateTime.MinValue)
                                {
                                    dateDeactivationSave = dateDeactivation.SelectedDate;
                                }

                                foreach (var item in listaCategorias)
                                {
                                    if (!item.GuardarValor(listaAtributos, cEmplazamientos.Context))
                                    {
                                        direct.Success = false;
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }

                                if (geoEmplazamiento.Municipio != "")
                                {
                                    lPaisID = cMunicipios.getPaisByMunicipio(geoEmplazamiento.Municipio.Split(',')[0]);

                                    if (lPaisID != 0)
                                    {
                                        lMunicipioID = cMunicipios.GetMunicipioByNombre(geoEmplazamiento.Municipio.Split(',')[0]);
                                    }
                                }

                                ResponseCreateController responseCreate = cEmplazamientos.CreateSite(true, oDato.EmplazamientoID, oUser, lCliID,
                                    txtCodigo.Text,
                                    txtNombre.Text,
                                    Convert.ToInt64(cmbOperadores.SelectedItem.Value),
                                    Convert.ToInt64(cmbEstadosGlobales.SelectedItem.Value),
                                    Convert.ToInt64(cmbMonedas.SelectedItem.Value),
                                    Convert.ToInt64(cmbCategorias.SelectedItem.Value),
                                    Convert.ToInt64(cmbTipos.SelectedItem.Value),
                                    (cmbTiposEstructuras.Value != null && cmbTiposEstructuras.Value.ToString() != "") ? Convert.ToInt64(cmbTiposEstructuras.Value) : 0,
                                    Convert.ToInt64(cmbTiposEdificios.SelectedItem.Value),
                                    (cmbTamanos.Value != null && cmbTamanos.Value.ToString() != "") ? Convert.ToInt64(cmbTamanos.Value) : 0,
                                    dateActivation.SelectedDate,
                                    dateDeactivationSave,
                                    (long)lPaisID,
                                    lMunicipioID,
                                    geoEmplazamiento.Direccion,
                                    geoEmplazamiento.Barrio,
                                    geoEmplazamiento.CodigoPostal,
                                    Convert.ToDouble(geoEmplazamiento.Longitud),
                                    Convert.ToDouble(geoEmplazamiento.Latitud),
                                    listaAtributos);

                                if (responseCreate.Data == null)
                                {
                                    direct.Success = false;
                                    direct.Result = GetLocalResourceObject("strErrorGenerarHistorico").ToString();
                                    return direct;
                                }
                                #endregion
                            }
                            else
                            {
                                direct.Success = false;
                                direct.Result = "Editado";
                            }
                        }
                        else
                        {
                            hdCodigoDuplicado.SetValue("Duplicado");
                            direct.Success = false;
                            direct.Result = "Codigo";
                            return direct;
                        }
                    }
                    else
                    {
                        if (SobrescribirEdicion || (hdHistoricoEmplazamiento.Value.ToString() == DatoHistorico.HistoricoCoreEmplazamientoID.ToString()))
                        {
                            #region UPDATE

                            DateTime? dateDeactivationSave = null;
                            if (dateDeactivation.SelectedDate > DateTime.MinValue)
                            {
                                dateDeactivationSave = dateDeactivation.SelectedDate;
                            }

                            foreach (var item in listaCategorias)
                            {
                                if (!item.GuardarValor(listaAtributos, cEmplazamientos.Context))
                                {
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                    return direct;
                                }
                            }

                            if (geoEmplazamiento.Municipio != "" && geoEmplazamiento.Municipio != null)
                            {
                                lPaisID = cMunicipios.getPaisByMunicipio(geoEmplazamiento.Municipio.Split(',')[0]);

                                if (lPaisID != 0)
                                {
                                    lMunicipioID = cMunicipios.GetMunicipioByNombre(geoEmplazamiento.Municipio.Split(',')[0]);
                                }

                            }

                            ResponseCreateController responseCreate = cEmplazamientos.CreateSite(true, oDato.EmplazamientoID, oUser, lCliID,
                                txtCodigo.Text,
                                txtNombre.Text,
                                Convert.ToInt64(cmbOperadores.SelectedItem.Value),
                                Convert.ToInt64(cmbEstadosGlobales.SelectedItem.Value),
                                Convert.ToInt64(cmbMonedas.SelectedItem.Value),
                                Convert.ToInt64(cmbCategorias.SelectedItem.Value),
                                Convert.ToInt64(cmbTipos.SelectedItem.Value),
                                (cmbTiposEstructuras.Value != null && cmbTiposEstructuras.Value.ToString() != "") ? Convert.ToInt64(cmbTiposEstructuras.Value) : 0,
                                Convert.ToInt64(cmbTiposEdificios.SelectedItem.Value),
                                (cmbTamanos.Value != null && cmbTamanos.Value.ToString() != "") ? Convert.ToInt64(cmbTamanos.Value) : 0,
                                dateActivation.SelectedDate,
                                dateDeactivationSave,
                                (long)lPaisID,
                                lMunicipioID,
                                geoEmplazamiento.Direccion,
                                geoEmplazamiento.Barrio,
                                geoEmplazamiento.CodigoPostal,
                                Convert.ToDouble(geoEmplazamiento.Longitud),
                                Convert.ToDouble(geoEmplazamiento.Latitud),
                                listaAtributos);

                            if (responseCreate.Data == null)
                            {
                                direct.Success = false;
                                direct.Result = GetLocalResourceObject("strErrorGenerarHistorico").ToString();
                                return direct;
                            }
                            #endregion
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = "Editado";
                        }
                    }
                }
                else
                {
                    if (!cEmplazamientos.EmplazamientoDuplicadoCodigoOperador(long.Parse(cmbOperadores.SelectedItem.Value), txtCodigo.Text))
                    {
                        #region ADD
                        DateTime? dateDeactivationSave = null;
                        if (dateDeactivation.SelectedDate > DateTime.MinValue)
                        {
                            dateDeactivationSave = dateDeactivation.SelectedDate;
                        }

                        foreach (var item in listaCategorias)
                        {
                            if (!item.GuardarValor(listaAtributos, cEmplazamientos.Context))
                            {
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                return direct;
                            }
                        }

                        if (geoEmplazamiento.Municipio != "")
                        {
                            lPaisID = cMunicipios.getPaisByMunicipio(geoEmplazamiento.Municipio.Split(',')[0]);

                            if (lPaisID != 0)
                            {
                                lMunicipioID = cMunicipios.GetMunicipioByNombre(geoEmplazamiento.Municipio.Split(',')[0]);
                            }
                        }

                        ResponseCreateController responseCreate = cEmplazamientos.CreateSite(false, null, oUser, lCliID,
                            txtCodigo.Text,
                            txtNombre.Text,
                            Convert.ToInt64(cmbOperadores.SelectedItem.Value),
                            Convert.ToInt64(cmbEstadosGlobales.SelectedItem.Value),
                            Convert.ToInt64(cmbMonedas.SelectedItem.Value),
                            Convert.ToInt64(cmbCategorias.SelectedItem.Value),
                            Convert.ToInt64(cmbTipos.SelectedItem.Value),
                            (cmbTiposEstructuras.Value != null && cmbTiposEstructuras.Value.ToString() != "") ? Convert.ToInt64(cmbTiposEstructuras.Value) : 0,
                            Convert.ToInt64(cmbTiposEdificios.SelectedItem.Value),
                            (cmbTamanos.Value != null && cmbTamanos.Value.ToString() != "") ? Convert.ToInt64(cmbTamanos.Value) : 0,
                            dateActivation.SelectedDate,
                            dateDeactivationSave,
                            (long)lPaisID,
                            lMunicipioID,
                            geoEmplazamiento.Direccion,
                            geoEmplazamiento.Barrio,
                            geoEmplazamiento.CodigoPostal,
                            Convert.ToDouble(geoEmplazamiento.Longitud),
                            Convert.ToDouble(geoEmplazamiento.Latitud),
                            listaAtributos);

                        if (responseCreate.Data == null)
                        {
                            direct.Success = false;
                            direct.Result = GetLocalResourceObject("strErrorGenerarHistorico").ToString();
                            return direct;
                        }
                        else
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));

                            if (hdCondicionReglaID.Value.ToString() != "")
                            {
                                GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
                                if (!cCondicionesConfiguraciones.ActualizarUltimoCodigoByReglaID(long.Parse(hdCondicionReglaID.Value.ToString()), txtCodigo.Text))
                                {
                                    direct.Success = false;
                                    direct.Result = GetLocalResourceObject("strErrorActualizarCodigoAutomatico").ToString();
                                    return direct;
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        hdCodigoDuplicado.SetValue("Duplicado");
                        direct.Success = false;
                        direct.Result = "Codigo";
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


            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar(long lSeleccionado)
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            PaisesController cPaises = new PaisesController();
            MunicipiosController cMunicipios = new MunicipiosController();

            HistoricosCoreEmplazamientosController cHistorico = new HistoricosCoreEmplazamientosController();
            Data.HistoricosCoreEmplazamientos DatoHistorico;
            Data.Paises oPais;
            string sMunicipio = "";
            string sProvincia = "";

            try
            {
                direct.Success = true;

                Data.Emplazamientos oDato = cEmplazamientos.GetItem(lSeleccionado);
                hdEmplazamientoID.Value = oDato.EmplazamientoID.ToString();
                DatoHistorico = cHistorico.getHistoricoByID(oDato.EmplazamientoID);
                hdHistoricoEmplazamiento.Value = DatoHistorico.HistoricoCoreEmplazamientoID.ToString();

                #region DATOS PRINCIPALES

                txtCodigo.Text = oDato.Codigo;
                txtNombre.Text = oDato.NombreSitio;
                this.OperadorID = oDato.OperadorID;
                this.EstadoGlobalID = oDato.EstadoGlobalID;
                this.EmplazamientoCategoriaSitioID = oDato.CategoriaEmplazamientoID;
                this.EmplazamientoTipoID = oDato.EmplazamientoTipoID;
                this.EmplazamientoTamanoID = oDato.EmplazamientoTamanoID;
                this.EmplazamientoTipoEstructuraID = oDato.EmplazamientoTipoEstructuraID;
                this.EmplazamientoTipoEdificioID = oDato.TipoEdificacionID;
                this.MonedaID = oDato.MonedaID;
                dateActivation.SelectedDate = (DateTime)oDato.FechaActivacion;

                if (oDato.FechaDesactivacion != null && oDato.FechaDesactivacion.Value != DateTime.MinValue)
                {
                    dateDeactivation.SelectedDate = (DateTime)oDato.FechaDesactivacion;
                }
                #endregion

                #region DATOS LOCALIZACIONES

                if (oDato.Latitud != 0 || oDato.Longitud != 0 || oDato.Direccion != ""
                    || oDato.Barrio != "" || oDato.CodigoPostal != "" || oDato.MunicipioID != null
                    || oDato.MunicipioID != 0)
                {
                    geoEmplazamiento.Latitud = oDato.Latitud.ToString();
                    geoEmplazamiento.Longitud = oDato.Longitud.ToString();
                    geoEmplazamiento.Direccion = oDato.Direccion;
                    geoEmplazamiento.Barrio = oDato.Barrio;
                    geoEmplazamiento.CodigoPostal = oDato.CodigoPostal;

                    oPais = cPaises.GetPaisByMunicipioID((long)oDato.MunicipioID);
                    sMunicipio = cMunicipios.GetMunicipioByID(oDato.MunicipioID);
                    sProvincia = cMunicipios.getNombreProvinciaByMunicipioID(sMunicipio);
                    geoEmplazamiento.Municipio = sMunicipio + ", " + sProvincia + " (" + oPais.PaisCod + ")";
                }
                #endregion

                direct.Result = MostrarEditarAtributos(lSeleccionado);
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
        public DirectResponse Eliminar(long lSeleccionado)
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();

            try
            {
                if (cEmplazamientos.DeleteItem(lSeleccionado))
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

        #region GENERACION AUTOMÁTICA CÓDIGO


        [DirectMethod()]
        public DirectResponse ComprobarCodigoEmplazamientoDuplicado()
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            Data.GlobalCondicionesReglas aplicarRegla;
            List<Data.GlobalCondicionesReglasConfiguraciones> configuraciones;


            try
            {
                #region COMPROBAR CODIGO
                if (cEmplazamientos.CodigoDuplicadoGeneradorCodigos(hdCodigoEmplazamientoAutogenerado.Value.ToString()))
                {

                    hdCodigoDuplicado.Value = "Duplicado";
                }
                else
                {
                    hdCodigoDuplicado.Value = "No_Duplicado";
                }
                #endregion

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
        public DirectResponse GenerarCodigoEmplazamiento()
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            GlobalCondicionesReglasController cCondicionesReglasController = new GlobalCondicionesReglasController();
            EmplazamientosCategoriasSitiosController cCategorias = new EmplazamientosCategoriasSitiosController();
            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
            List<Data.GlobalCondicionesReglasConfiguraciones> configuraciones;

            long lCliID = 0;

            try
            {
                lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

                Data.GlobalCondicionesReglas aplicarRegla = cCondicionesReglasController.GetReglaByCampoDestino("CODIGO_EMPLAZAMIENTO", (long)Comun.Modulos.GLOBAL);

                if (aplicarRegla != null)
                {
                    configuraciones = cCondicionesConfiguraciones.GlobalCondicionesReglasConfiguracionesBySeleccionadoID(aplicarRegla.GlobalCondicionReglaID);

                    //hdCondicionReglaID.Value = aplicarRegla.GlobalCondicionReglaID;
                    //direct.Result = cCondicionesReglasController.getConfiguracionRegla(aplicarRegla.GlobalCondicionReglaID);

                    if (configuraciones != null && configuraciones.Count > 0)
                    {
                        string siguienteCodigo;
                        long CategoriaID = long.Parse(((Hidden)X.GetCmp("hdCategoria")).Value.ToString());
                        var CategoriaActiva = cCategorias.GetItem(CategoriaID);
                        siguienteCodigo = cCondicionesConfiguraciones.GetSiguienteByListaCondicionesReglasConfiguraciones(configuraciones, aplicarRegla.UltimoGenerado, aplicarRegla.Modificada, (long)CategoriaActiva.ClienteID);

                        if (siguienteCodigo == null)
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strGeneracionCodigoFallida);
                            return direct;
                        }
                        else
                        {
                            txtCodigo.SetValue(siguienteCodigo);

                            hdCodigoEmplazamientoAutogenerado.SetValue(siguienteCodigo);
                            hdCondicionReglaID.SetValue(aplicarRegla.GlobalCondicionReglaID);

                            JsonObject listaIDs = new JsonObject();
                            //listaIDs.Add("Emplazamientos", long.Parse(hdEmplazamientoID.Value.ToString()));
                            direct.Result = cCondicionesReglasController.getConfiguracionRegla(aplicarRegla.GlobalCondicionReglaID, listaIDs);
                        }
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strGeneracionSinRegla);
                        return direct;
                    }

                    //if (!(hdVistaPlantilla.Value != null && hdVistaPlantilla.Value.ToString() != ""))
                    //{
                    //    JsonObject listaIDs = new JsonObject();
                    //    listaIDs.Add("Emplazamientos", long.Parse(hdEmplazamientoID.Value.ToString()));
                    //    direct.Result = cCondicionesReglasController.getConfiguracionRegla(aplicarRegla.GlobalCondicionReglaID, listaIDs);
                    //}
                }
                //else
                //{
                //    direct.Success = false;
                //    direct.Result = GetGlobalResource(Comun.strGeneracionSinRegla);
                //    return direct;
                //}
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strGeneracionCodigoFallida);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        #endregion

        #endregion

        #region FUNCTIONS

        #region BOTONES NEXT PREV
        protected void Next_Click(object sender, DirectEventArgs e)
        {
            int index = int.Parse(e.ExtraParams["index"]);

            if ((index + 1) < this.pnVistasFormEmplazamiento.Items.Count)
            {
                this.pnVistasFormEmplazamiento.ActiveIndex = index + 1;
            }

            this.CheckButtons();
        }
        protected void Activate_Link_Click(object sender, DirectEventArgs e)
        {
            int index = int.Parse(e.ExtraParams["index"]);

            if ((index) < this.pnVistasFormEmplazamiento.Items.Count)
            {
                this.pnVistasFormEmplazamiento.ActiveIndex = index;
            }

            this.CheckButtons();
        }

        protected void Prev_Click(object sender, DirectEventArgs e)
        {
            int index = int.Parse(e.ExtraParams["index"]);

            if ((index - 1) >= 0)
            {
                this.pnVistasFormEmplazamiento.ActiveIndex = index - 1;
            }

            this.CheckButtons();
        }

        private void CheckButtons()
        {
            int index = this.pnVistasFormEmplazamiento.ActiveIndex;

            this.btnNextEmplazamiento.Disabled = index == this.pnVistasFormEmplazamiento.Items.Count;
            this.btnPrevEmplazamiento.Disabled = index == 3;
        }

        #endregion

        [DirectMethod()]
        public DirectResponse PintarCategorias(bool Update)
        {
            EmplazamientosAtributosCategoriasController cCategorias = new EmplazamientosAtributosCategoriasController();
            TreeCore.Componentes.GestionCategoriasAtributos oComponente;
            Data.EmplazamientosAtributosCategorias oDato;
            long cliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                if (listaCategorias == null || listaCategorias.Count == 0 || Update)
                {
                    listaCategorias = new List<GestionCategoriasAtributos>();
                    List<Data.EmplazamientosAtributosCategorias> listaAtributos = cCategorias.getCategoriasSeleccionadas(cliID);
                    foreach (var idCate in listaAtributos)
                    {
                        oComponente = (Componentes.GestionCategoriasAtributos)this.LoadControl("/Componentes/GestionCategoriasAtributos.ascx");
                        oComponente.ID = "CAT" + idCate.EmplazamientoAtributoCategoriaID;
                        oComponente.CategoriaAtributoID = idCate.EmplazamientoAtributoCategoriaID;
                        oComponente.Nombre = idCate.Nombre;
                        oComponente.Orden = cCategorias.GetOrdenCategoria(idCate.EmplazamientoAtributoCategoriaID, cliID);
                        oComponente.Modulo = (long)Comun.Modulos.GLOBAL;
                        listaCategorias.Add(oComponente);
                    }
                    listaCategorias = listaCategorias.OrderBy(it => it.Orden).ToList();
                    if (contenedorCategorias != null && contenedorCategorias.ContentControls != null && contenedorCategorias.ContentControls.Count > 0)
                    {
                        contenedorCategorias.ContentControls.Clear();
                    }
                    if (contenedorCategorias != null && contenedorCategorias.ContentControls != null)
                    {
                        foreach (var item in listaCategorias)
                        {
                            contenedorCategorias.ContentControls.Add(item);
                        }
                    }
                }
                else
                {
                    if (listaCategorias != null)
                    {
                        listaCategorias.Clear();
                    }
                    if (contenedorCategorias != null)
                    {
                        contenedorCategorias.ContentControls.Clear();
                    }
                }
                if (Update)
                {
                    contenedorCategorias.UpdateContent();

                    if (listaCategorias != null && listaCategorias.Count != 0)
                    {
                        foreach (var item in listaCategorias)
                        {
                            item.PintarAtributos(true);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }
            return direct;
        }

        public JsonObject MostrarEditarAtributos(long EmplazamientoID)
        {
            List<Data.EmplazamientosAtributos> listaAtributos;
            EmplazamientosAtributosController cAtributos = new EmplazamientosAtributosController();
            List<object> listaValoresAtributos = new List<object>();
            JsonObject jsDatos = new JsonObject();

            listaAtributos = cAtributos.GetAtributosEmplazamientos(EmplazamientoID);

            foreach (var item in listaAtributos)
            {
                listaValoresAtributos.Add(new
                {
                    AtributoID = item.EmplazamientoAtributoConfiguracionID,
                    Valor = item.Valor
                });
            }

            //PintarCategorias(true);

            if (listaValoresAtributos.Count > 0)
            {
                foreach (var item in listaCategorias)
                {
                    item.MostrarEditar(listaValoresAtributos, jsDatos);
                }
            }
            return jsDatos;
        }

        #endregion
    }
}