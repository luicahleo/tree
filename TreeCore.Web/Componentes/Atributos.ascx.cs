using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using TreeCore.Page;
using System.Data.SqlClient;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Security;
using System.Collections;
using System.Linq;

namespace TreeCore.Componentes
{
    public partial class Atributos : BaseUserControl
    {
        protected long _Orden;
        protected long _Modulo;
        protected string _TipoAtributo;
        protected long _CategoriaAtributoID;
        protected long _CategoriaAtributoAsignacionID;
        protected long _AtributoID;
        protected ComponentBase ControlAtributo;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string nombreComponente;

        public string Nombre
        {
            get
            {
                return lbNombreAtr.Text;
            }
            set
            {
                lbNombreAtr.Text = value;
            }
        }

        public string TipoAtributo
        {
            get { return _TipoAtributo; }
            set { _TipoAtributo = value; }
        }

        public long Orden
        {
            get { return _Orden; }
            set { _Orden = value; }
        }

        public long Modulo
        {
            get { return _Modulo; }
            set { _Modulo = value; }
        }

        public long CategoriaAtributoID
        {
            get { return _CategoriaAtributoID; }
            set
            {
                _CategoriaAtributoID = value;
            }
        }
        public long CategoriaAtributoAsignacionID
        {
            get { return _CategoriaAtributoAsignacionID; }
            set
            {
                _CategoriaAtributoAsignacionID = value;
                hdCategoriaAtributoID.Value = value;
            }
        }

        public long AtributoID
        {
            get { return _AtributoID; }
            set
            {
                _AtributoID = value;
                hdAtributoID.Value = value;
            }
        }

        public bool EsPlantilla
        {
            set
            {
                if (value)
                {
                    lbNombreAtr.ReadOnly = true;
                    btnMoverAtributo.Hidden = true;
                    btnOpciones.Hidden = true;
                }
            }
        }

        public bool EsSoloLectura
        {
            set
            {
                if (value)
                {
                    lbNombreAtr.ReadOnly = true;
                    btnMoverAtributo.Hidden = true;
                    btnOpciones.Hidden = true;
                }
            }
        }

        public bool EsTarea
        {
            set
            {
                if (value)
                {
                    btnMoverAtributo.Hidden = true;
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            nombreComponente = this.ID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PintarControl(false);
        }


        #region STORES

        #region COLORES

        protected void storeColores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreSemaforosColoresController cColores = new CoreSemaforosColoresController();
                try
                {
                    List<Data.CoreSemaforosColores> listaDatos;

                    listaDatos = cColores.GetColoresByClienteID(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));

                    if (listaDatos != null)
                    {
                        storeColores.DataSource = listaDatos;
                        storeColores.DataBind();
                    }
                    else
                    {
                        storeColores.DataSource = new List<Data.Roles>();
                        storeColores.DataBind();
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

        #region ATRIBUTOS

        [DirectMethod]
        public DirectResponse EliminarAtributo()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                switch (this.TipoAtributo)
                {
                    case Comun.MODULOINVENTARIO:
                        CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController cInvAtr = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController();
                        CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
                        if (cInvAtr.AtributoUsado(this.AtributoID))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                            return direct;

                        }
                        if (cAtributos.EliminarAtributo(this.AtributoID))
                        {
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            return direct;
                        }
                        break;
                    case Comun.EMPLAZAMIENTOS:
                        EmplazamientosAtributosConfiguracionController cEmplazamientosAtributos = new EmplazamientosAtributosConfiguracionController();
                        if (cEmplazamientosAtributos.DeleteItem(this.AtributoID))
                        {
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            direct.Success = true;
                            direct.Result = "";
                        }
                        break;
                    case Comun.PRODUCT_CATALOG_SERVICIOS:
                        CoreAtributosConfiguracionesController cAtributosConfig = new CoreAtributosConfiguracionesController();
                        CoreProductCatalogServiciosAtributosConfiguracionesController cConfig = new CoreProductCatalogServiciosAtributosConfiguracionesController();
                        CoreProductCatalogServiciosTareasController cTareas = new CoreProductCatalogServiciosTareasController();

                        Data.CoreProductCatalogServiciosAtributosConfiguraciones oConfig = cConfig.getDataByAtributoID(this.AtributoID);

                        if (cConfig.DeleteItem(oConfig.CoreProductCatalogServicioAtributoConfiguracionID))
                        {
                            if (cAtributosConfig.DeleteItem(this.AtributoID))
                            {
                                log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                                direct.Success = true;
                                direct.Result = "";
                            }
                        }
                        break;
                    default:
                        break;
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

        [DirectMethod]
        public DirectResponse CambiarNombre()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                direct.Success = true;
                direct.Result = "";

                CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
                Data.CoreAtributosConfiguraciones oAtributo;

                switch (this.TipoAtributo)
                {
                    case Comun.MODULOINVENTARIO:

                        oAtributo = cAtributos.GetItem(this.AtributoID);

                        if (lbNombreAtr.Text == null || lbNombreAtr.Text == "" || cAtributos.GetItem(this.AtributoID).Codigo == lbNombreAtr.Text)
                        {
                            direct.Success = true;
                            direct.Result = "";
                        }

                        #region Comprobación Formulario

                        else if (lbNombreAtr.Text == GetGlobalResource("strNombre") ||
                            lbNombreAtr.Text == GetGlobalResource("strCodigo") ||
                            lbNombreAtr.Text == GetGlobalResource("strEstado") ||
                            lbNombreAtr.Text == GetGlobalResource("strCategoria") ||
                            lbNombreAtr.Text == GetGlobalResource("strOperador") ||
                            lbNombreAtr.Text == GetGlobalResource("strPlantilla") ||
                            Comun.CamposValidosInventario.Contains(lbNombreAtr.Text))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource("jsYaExisteFormOriginal");
                            lbNombreAtr.SetText(cAtributos.GetItem(this.AtributoID).Codigo);
                        }

                        #endregion

                        else if (cAtributos.NombreDuplicado(lbNombreAtr.Text, this.AtributoID))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource("strCodigoDuplicadoCategoria");
                            lbNombreAtr.SetText(cAtributos.GetItem(this.AtributoID).Codigo);
                        }
                        else
                        {
                            oAtributo.Nombre = oAtributo.Nombre.Replace(oAtributo.Codigo, lbNombreAtr.Text);
                            oAtributo.Codigo = lbNombreAtr.Text;

                            if (cAtributos.UpdateItem(oAtributo))
                            {
                                log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                            }
                            else
                            {
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            }
                        }
                        break;

                    case Comun.EMPLAZAMIENTOS:

                        EmplazamientosAtributosConfiguracionController cEmplazamientosAtributos = new EmplazamientosAtributosConfiguracionController();
                        Data.EmplazamientosAtributosConfiguracion oEmplazamientosAtributo;
                        oEmplazamientosAtributo = cEmplazamientosAtributos.GetItem(this.AtributoID);

                        if (lbNombreAtr.Text == null || lbNombreAtr.Text == ""
                            || cEmplazamientosAtributos.GetItem(this.AtributoID).NombreAtributo == lbNombreAtr.Text)
                        {
                            direct.Success = true;
                            direct.Result = "";
                        }

                        #region Comprobación Formulario

                        else if (lbNombreAtr.Text == GetGlobalResource("strEmplazamientoCodigo") ||
                            lbNombreAtr.Text == GetGlobalResource("strNombreSitio") ||
                            lbNombreAtr.Text == GetGlobalResource("strOperador") ||
                            lbNombreAtr.Text == GetGlobalResource("strMoneda") ||
                            lbNombreAtr.Text == GetGlobalResource("strEstadoGlobal") ||
                            lbNombreAtr.Text == GetGlobalResource("strCategoriaSitio") ||
                            lbNombreAtr.Text == GetGlobalResource("strTiposEmplazamientos") ||
                            lbNombreAtr.Text == GetGlobalResource("strTipoEdificio") ||
                            lbNombreAtr.Text == GetGlobalResource("strTipoEstructura") ||
                            lbNombreAtr.Text == GetGlobalResource("strEmplazamientosTamanos") ||
                            lbNombreAtr.Text == GetGlobalResource("strFechaActivacion") ||
                            lbNombreAtr.Text == GetGlobalResource("strFechaDesactivacion") ||
                            lbNombreAtr.Text == GetGlobalResource("strMunicipio") ||
                            lbNombreAtr.Text == GetGlobalResource("strBarrio") ||
                            lbNombreAtr.Text == GetGlobalResource("strDireccion") ||
                            lbNombreAtr.Text == GetGlobalResource("strCodigoPostal") ||
                            lbNombreAtr.Text == GetGlobalResource("strLatitud") ||
                            lbNombreAtr.Text == GetGlobalResource("strLongitud") || Comun.CamposValidosEmplazamientos.Contains(lbNombreAtr.Text))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource("jsYaExisteFormOriginal");
                            lbNombreAtr.SetText(cEmplazamientosAtributos.GetItem(this.AtributoID).NombreAtributo);
                        }

                        #endregion

                        else if (cEmplazamientosAtributos.NombreDuplicado(lbNombreAtr.Text, oEmplazamientosAtributo.ClienteID))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsYaExiste);
                            lbNombreAtr.SetText(cEmplazamientosAtributos.GetItem(this.AtributoID).NombreAtributo);
                        }
                        else
                        {
                            oEmplazamientosAtributo.NombreAtributo = lbNombreAtr.Text;
                            oEmplazamientosAtributo.CodigoAtributo = lbNombreAtr.Text;

                            if (cEmplazamientosAtributos.UpdateItem(oEmplazamientosAtributo))
                            {
                                log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                            }
                            else
                            {
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            }
                        }
                        break;

                    case Comun.PRODUCT_CATALOG_SERVICIOS:

                        oAtributo = cAtributos.GetItem(this.AtributoID);

                        if (lbNombreAtr.Text == null || lbNombreAtr.Text == "" || cAtributos.GetItem(this.AtributoID).Codigo == lbNombreAtr.Text)
                        {
                            direct.Success = true;
                            direct.Result = "";
                        }

                        #region Comprobación Formulario

                        else if (lbNombreAtr.Text == GetGlobalResource("strNombre") ||
                            lbNombreAtr.Text == GetGlobalResource("strCodigo") ||
                            lbNombreAtr.Text == GetGlobalResource("strEstado") ||
                            lbNombreAtr.Text == GetGlobalResource("strCategoria") ||
                            lbNombreAtr.Text == GetGlobalResource("strOperador") ||
                            lbNombreAtr.Text == GetGlobalResource("strPlantilla") || Comun.CamposValidosInventario.Contains(lbNombreAtr.Text))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource("jsYaExisteFormOriginal");
                            lbNombreAtr.SetText(cAtributos.GetItem(this.AtributoID).Nombre);
                        }

                        #endregion

                        else if (cAtributos.NombreDuplicado(lbNombreAtr.Text, this.AtributoID))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource("strCodigoDuplicadoCategoria");
                            lbNombreAtr.SetText(cAtributos.GetItem(this.AtributoID).Codigo);
                        }
                        else
                        {
                            oAtributo.Nombre = oAtributo.Nombre.Replace(oAtributo.Codigo, lbNombreAtr.Text);
                            oAtributo.Codigo = lbNombreAtr.Text;

                            if (cAtributos.UpdateItem(oAtributo))
                            {
                                log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                            }
                            else
                            {
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            }
                        }
                        break;

                    default:
                        break;
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

        [DirectMethod]
        public DirectResponse PintarControl(bool Update = false)
        {
            TiposDatosController cTipoDatos = new TiposDatosController();
            DirectResponse direct = new DirectResponse();

            Data.TiposDatos oTipoDato;
            try
            {
                direct.Success = true;
                direct.Result = "";
                if (Update && contenedorControl.Items != null)
                {
                    contenedorControl.Items.Clear();
                    ControlAtributo = null;
                }
                if (ControlAtributo == null)
                {
                    switch (this.TipoAtributo)
                    {
                        #region INVENTARIO
                        case Comun.MODULO_WORKFLOW:
                        case Comun.MODULOINVENTARIO:
                            CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
                            CoreAtributosConfiguracionTiposDatosPropiedadesController cPropiedades = new CoreAtributosConfiguracionTiposDatosPropiedadesController();
                            CoreAtributosConfiguracionRolesRestringidosController cRestriccionRoles = new CoreAtributosConfiguracionRolesRestringidosController();
                            List<Data.Vw_CoreAtributosConfiguracionTiposDatosPropiedades> listaPropiedades;
                            Data.CoreAtributosConfiguraciones oDato = cAtributos.GetItem(this.AtributoID);
                            oTipoDato = cTipoDatos.GetItem(oDato.TipoDatoID);
                            listaPropiedades = cPropiedades.GetVwPropiedadesFromAtributo(this.AtributoID);
                            switch (oTipoDato.Codigo)
                            {
                                case Comun.TIPODATO_CODIGO_TEXTO:
                                    ControlAtributo = new TextField
                                    {
                                        MaxLength = 200
                                    };
                                    break;
                                case Comun.TIPODATO_CODIGO_NUMERICO:
                                    ControlAtributo = new NumberField
                                    {
                                        AllowExponential = false,
                                        MaxLength = 200
                                    };
                                    break;
                                case Comun.TIPODATO_CODIGO_FECHA:
                                    ControlAtributo = new DateField
                                    {
                                        Format = Comun.FORMATO_FECHA,
                                        AriaHelp = GetGlobalResource("strFechaAyudaFormato"),
                                        InvalidText = GetGlobalResource("strFechaFormatoIncorrecto")
                                    };
                                    break;
                                case Comun.TIPODATO_CODIGO_LISTA:
                                    ControlAtributo = new ComboBox
                                    {
                                        ForceSelection = true,
                                        QueryMode = DataLoadMode.Local
                                    };
                                    if (oDato.TablaModeloDatoID != null)
                                    {
                                        List<Ext.Net.ListItem> listaItems;
                                        listaItems = cAtributos.GetItemsComboBox(this.AtributoID);
                                        if (listaItems != null)
                                        {
                                            ((ComboBox)ControlAtributo).Items.AddRange(listaItems);
                                        }
                                    }
                                    else if (oDato.ValoresPosibles != null && oDato.ValoresPosibles != "")
                                    {
                                        foreach (var item in (from c in oDato.ValoresPosibles.Split(';') orderby c select c))
                                        {
                                            ((ComboBox)ControlAtributo).Items.Add(new ListItem
                                            {
                                                Value = item,
                                                Text = item
                                            });
                                        }
                                    }
                                    break;
                                case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                                    ControlAtributo = new MultiCombo
                                    {
                                        ForceSelection = true,
                                        QueryMode = DataLoadMode.Local
                                    };
                                    if (oDato.TablaModeloDatoID != null)
                                    {
                                        List<Ext.Net.ListItem> listaItems;
                                        listaItems = cAtributos.GetItemsComboBox(this.AtributoID);
                                        if (listaItems != null)
                                        {
                                            ((MultiCombo)ControlAtributo).Items.AddRange(listaItems);
                                        }
                                    }
                                    else if (oDato.ValoresPosibles != null && oDato.ValoresPosibles != "")
                                    {
                                        foreach (var item in (from c in oDato.ValoresPosibles.Split(';') orderby c select c))
                                        {
                                            ((MultiCombo)ControlAtributo).Items.Add(new ListItem
                                            {
                                                Value = item,
                                                Text = item
                                            });
                                        }
                                    }
                                    break;
                                case Comun.TIPODATO_CODIGO_BOOLEAN:
                                    ControlAtributo = new Checkbox();
                                    break;
                                //case "ENTERO":

                                //    break;
                                //case "FLOTANTE":

                                //    break;
                                //case "MONEADA":

                                //    break;
                                //case "GEOPOSICION":

                                //    break;
                                case Comun.TIPODATO_CODIGO_TEXTAREA:
                                    ControlAtributo = new TextArea();
                                    break;
                                default:
                                    ControlAtributo = new TextField();
                                    break;
                            }
                            //ControlAtributo.ID = "Control" + oDato.InventarioAtributoID;
                            ControlAtributo.Cls = "txtContainerCategorias";

                            #region Propiedades

                            foreach (var item in listaPropiedades)
                            {
                                try
                                {
                                    if (item.CodigoTipoDatoPropiedad == "ToolTips")
                                    {
                                        var propertyInfo = ControlAtributo.GetType().GetProperty(item.CodigoTipoDatoPropiedad);
                                        ItemsCollection<ToolTip> listaTooltips = (ItemsCollection<ToolTip>)propertyInfo.GetValue(ControlAtributo);
                                        listaTooltips.Add(new ToolTip
                                        {
                                            Html = item.Valor,
                                            Anchor = "top",
                                            TrackMouse = true
                                        });
                                    }
                                    else if (item.CodigoTipoDatoPropiedad == "Value")
                                    {
                                        var propertyInfo = ControlAtributo.GetType().GetProperty(item.CodigoTipoDatoPropiedad);
                                        propertyInfo.SetValue(ControlAtributo, item.Valor);
                                    }
                                    else
                                    {
                                        var propertyInfo = ControlAtributo.GetType().GetProperty(item.CodigoTipoDatoPropiedad);
                                        if (propertyInfo != null)
                                        {
                                            switch (Type.GetTypeCode(propertyInfo.PropertyType))
                                            {
                                                case TypeCode.Boolean:
                                                    propertyInfo.SetValue(ControlAtributo, bool.Parse(item.Valor));
                                                    break;
                                                case TypeCode.Double:
                                                    propertyInfo.SetValue(ControlAtributo, Double.Parse(item.Valor));
                                                    break;
                                                case TypeCode.Int16:
                                                    propertyInfo.SetValue(ControlAtributo, int.Parse(item.Valor));
                                                    break;
                                                case TypeCode.Int32:
                                                    propertyInfo.SetValue(ControlAtributo, int.Parse(item.Valor));
                                                    break;
                                                case TypeCode.Int64:
                                                    propertyInfo.SetValue(ControlAtributo, long.Parse(item.Valor));
                                                    break;
                                                case TypeCode.String:
                                                    propertyInfo.SetValue(ControlAtributo, item.Valor);
                                                    break;
                                                case TypeCode.DateTime:
                                                    propertyInfo.SetValue(ControlAtributo, DateTime.Parse(item.Valor));
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            var cInvResDefe = cRestriccionRoles.GetDefault(this.AtributoID);
                            if (cInvResDefe != null)
                            {
                                switch (cInvResDefe.Restriccion)
                                {
                                    case (int)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                        ControlAtributo.Cls = ("txtContainerCategoriasHidden");
                                        break;
                                    case (int)Comun.RestriccionesAtributoDefecto.DISABLED:
                                        ControlAtributo.Cls = ("InvCategoryReadOnly");
                                        break;
                                    case (int)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                    default:
                                        break;
                                }

                            }

                            #endregion

                            break;

                        #endregion

                        #region EMPLAZAMIENTOS

                        case Comun.EMPLAZAMIENTOS:
                            EmplazamientosAtributosConfiguracionController cEmplazamientosAtributos = new EmplazamientosAtributosConfiguracionController();
                            EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController cEmplazamietosPropiedades = new EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController();
                            List<Data.Vw_EmplazamientosAtributosTiposDatosPropiedades> listaPropiedadesEmpl;
                            Data.EmplazamientosAtributosConfiguracion oEmplazamientoAtributo = cEmplazamientosAtributos.GetItem(this.AtributoID);
                            EmplazamientosAtributosConfiguracionRolesRestringidosController cEmplRestriccionRoles = new EmplazamientosAtributosConfiguracionRolesRestringidosController();
                            oTipoDato = cTipoDatos.GetItem(oEmplazamientoAtributo.TipoDatoID);
                            listaPropiedadesEmpl = cEmplazamietosPropiedades.GetPropiedadesFromAtributo(this.AtributoID);
                            switch (oTipoDato.Codigo)
                            {
                                case Comun.TIPODATO_CODIGO_TEXTO:
                                    ControlAtributo = new TextField
                                    {
                                        MaxLength = 200
                                    };
                                    break;
                                case Comun.TIPODATO_CODIGO_NUMERICO:
                                    ControlAtributo = new NumberField
                                    {
                                        AllowExponential = false,
                                        MaxLength = 200
                                    };
                                    break;
                                case Comun.TIPODATO_CODIGO_FECHA:
                                    ControlAtributo = new DateField
                                    {
                                        Format = Comun.FORMATO_FECHA,
                                        AriaHelp = GetGlobalResource("strFechaAyudaFormato"),
                                        InvalidText = GetGlobalResource("strFechaFormatoIncorrecto")
                                    };
                                    break;
                                case Comun.TIPODATO_CODIGO_LISTA:
                                    ControlAtributo = new ComboBox
                                    {
                                        ForceSelection = true,
                                        QueryMode = DataLoadMode.Local
                                    };
                                    if (oEmplazamientoAtributo.TablaModeloDatoID != null)
                                    {
                                        List<Ext.Net.ListItem> listaItems;
                                        listaItems = cEmplazamientosAtributos.GetItemsComboBoxByColumnaModeloDatosID(oEmplazamientoAtributo.EmplazamientoAtributoConfiguracionID);
                                        if (listaItems != null)
                                        {
                                            ((ComboBox)ControlAtributo).Items.AddRange(listaItems);
                                        }
                                    }
                                    else if (oEmplazamientoAtributo.ValoresPosibles != null && oEmplazamientoAtributo.ValoresPosibles != "")
                                    {
                                        foreach (var item in (from c in oEmplazamientoAtributo.ValoresPosibles.Split(';') orderby c select c))
                                        {
                                            ((ComboBox)ControlAtributo).Items.Add(new ListItem
                                            {
                                                Value = item,
                                                Text = item
                                            });
                                        }
                                    }
                                    break;
                                case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                                    ControlAtributo = new MultiCombo
                                    {
                                        ForceSelection = true,
                                        QueryMode = DataLoadMode.Local
                                    };
                                    if (oEmplazamientoAtributo.TablaModeloDatoID != null)
                                    {
                                        List<Ext.Net.ListItem> listaItems;
                                        listaItems = cEmplazamientosAtributos.GetItemsComboBoxByColumnaModeloDatosID(oEmplazamientoAtributo.EmplazamientoAtributoConfiguracionID);
                                        if (listaItems != null)
                                        {
                                            ((MultiCombo)ControlAtributo).Items.AddRange(listaItems);
                                        }
                                    }
                                    else if (oEmplazamientoAtributo.ValoresPosibles != null && oEmplazamientoAtributo.ValoresPosibles != "")
                                    {
                                        foreach (var item in (from c in oEmplazamientoAtributo.ValoresPosibles.Split(';') orderby c select c))
                                        {
                                            ((MultiCombo)ControlAtributo).Items.Add(new ListItem
                                            {
                                                Value = item,
                                                Text = item
                                            });
                                        }
                                    }
                                    break;
                                case Comun.TIPODATO_CODIGO_BOOLEAN:
                                    ControlAtributo = new Checkbox();
                                    break;
                                //case "ENTERO":

                                //    break;
                                //case "FLOTANTE":

                                //    break;
                                //case "MONEADA":

                                //    break;
                                //case "GEOPOSICION":

                                //    break;
                                case Comun.TIPODATO_CODIGO_TEXTAREA:
                                    ControlAtributo = new TextArea();
                                    break;
                                default:
                                    ControlAtributo = new TextField();
                                    break;
                            }
                            //ControlAtributo.ID = "Control" + oEmplazamientoAtributo.EmplazamientoAtributoConfiguracionID;
                            ControlAtributo.Cls = "txtContainerCategorias";

                            #region Propiedades

                            foreach (var item in listaPropiedadesEmpl)
                            {
                                try
                                {
                                    if (item.Codigo == "ToolTips")
                                    {
                                        var propertyInfo = ControlAtributo.GetType().GetProperty(item.Codigo);
                                        ItemsCollection<ToolTip> listaTooltips = (ItemsCollection<ToolTip>)propertyInfo.GetValue(ControlAtributo);
                                        listaTooltips.Add(new ToolTip
                                        {
                                            Html = item.Valor,
                                            Anchor = "top",
                                            TrackMouse = true
                                        });
                                    }
                                    else if (item.Codigo == "Value")
                                    {
                                        var propertyInfo = ControlAtributo.GetType().GetProperty(item.Codigo);
                                        propertyInfo.SetValue(ControlAtributo, item.Valor);
                                    }
                                    else
                                    {
                                        var propertyInfo = ControlAtributo.GetType().GetProperty(item.Codigo);
                                        if (propertyInfo != null)
                                        {
                                            switch (Type.GetTypeCode(propertyInfo.PropertyType))
                                            {
                                                case TypeCode.Boolean:
                                                    propertyInfo.SetValue(ControlAtributo, bool.Parse(item.Valor));
                                                    break;
                                                case TypeCode.Double:
                                                    propertyInfo.SetValue(ControlAtributo, Double.Parse(item.Valor));
                                                    break;
                                                case TypeCode.Int16:
                                                    propertyInfo.SetValue(ControlAtributo, int.Parse(item.Valor));
                                                    break;
                                                case TypeCode.Int32:
                                                    propertyInfo.SetValue(ControlAtributo, int.Parse(item.Valor));
                                                    break;
                                                case TypeCode.Int64:
                                                    propertyInfo.SetValue(ControlAtributo, long.Parse(item.Valor));
                                                    break;
                                                case TypeCode.String:
                                                    propertyInfo.SetValue(ControlAtributo, item.Valor);
                                                    break;
                                                case TypeCode.DateTime:
                                                    propertyInfo.SetValue(ControlAtributo, DateTime.Parse(item.Valor));
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            var cEmplResDefe = cEmplRestriccionRoles.GetDefault(this.AtributoID);
                            if (cEmplResDefe != null)
                            {
                                switch (cEmplResDefe.Restriccion)
                                {
                                    case (long)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                        ControlAtributo.Cls = ("txtContainerCategoriasHidden");
                                        break;
                                    case (long)Comun.RestriccionesAtributoDefecto.DISABLED:
                                        ControlAtributo.Cls = ("InvCategoryReadOnly");
                                        break;
                                    case (long)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                    default:
                                        break;
                                }

                            }

                            #endregion

                            break;

                        #endregion

                        #region PRODUCT CATALOG

                        case Comun.PRODUCT_CATALOG_SERVICIOS:

                            CoreAtributosConfiguracionesController cAtributosConfig = new CoreAtributosConfiguracionesController();
                            Data.CoreAtributosConfiguraciones oAtributo = cAtributosConfig.GetItem(this.AtributoID);
                            oTipoDato = cTipoDatos.GetItem(oAtributo.TipoDatoID);

                            switch (oTipoDato.Codigo)
                            {
                                case Comun.TIPODATO_CODIGO_TEXTO:
                                    ControlAtributo = new TextField
                                    {
                                        MaxLength = 200
                                    };
                                    break;
                                case Comun.TIPODATO_CODIGO_NUMERICO:
                                    ControlAtributo = new NumberField
                                    {
                                        AllowExponential = false,
                                        MaxLength = 200
                                    };
                                    break;
                                case Comun.TIPODATO_CODIGO_FECHA:
                                    ControlAtributo = new DateField
                                    {
                                        Format = Comun.FORMATO_FECHA,
                                        AriaHelp = GetGlobalResource("strFechaAyudaFormato"),
                                        InvalidText = GetGlobalResource("strFechaFormatoIncorrecto")
                                    };
                                    break;
                                case Comun.TIPODATO_CODIGO_LISTA:
                                    ControlAtributo = new ComboBox
                                    {
                                        ForceSelection = true,
                                        QueryMode = DataLoadMode.Local
                                    };
                                    if (oAtributo.ColumnaModeloDatoID != null)
                                    {
                                        List<Ext.Net.ListItem> listaItems;
                                        listaItems = cAtributosConfig.GetItemsComboBox(this.AtributoID);
                                        if (listaItems != null)
                                        {
                                            ((ComboBox)ControlAtributo).Items.AddRange(listaItems);
                                        }
                                    }
                                    else if (oAtributo.ValoresPosibles != null && oAtributo.ValoresPosibles != "")
                                    {
                                        foreach (var item in oAtributo.ValoresPosibles.Split(';'))
                                        {
                                            ((ComboBox)ControlAtributo).Items.Add(new ListItem
                                            {
                                                Value = item,
                                                Text = item
                                            });
                                        }
                                    }
                                    break;
                                case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                                    ControlAtributo = new MultiCombo
                                    {
                                        ForceSelection = true,
                                        QueryMode = DataLoadMode.Local
                                    };
                                    if (oAtributo.ColumnaModeloDatoID != null)
                                    {
                                        List<Ext.Net.ListItem> listaItems;
                                        listaItems = cAtributosConfig.GetItemsComboBox(this.AtributoID);
                                        if (listaItems != null)
                                        {
                                            ((MultiCombo)ControlAtributo).Items.AddRange(listaItems);
                                        }
                                    }
                                    else if (oAtributo.ValoresPosibles != null && oAtributo.ValoresPosibles != "")
                                    {
                                        foreach (var item in oAtributo.ValoresPosibles.Split(';'))
                                        {
                                            ((MultiCombo)ControlAtributo).Items.Add(new ListItem
                                            {
                                                Value = item,
                                                Text = item
                                            });
                                        }
                                    }
                                    break;
                                case Comun.TIPODATO_CODIGO_BOOLEAN:
                                    ControlAtributo = new Checkbox();
                                    break;
                                case Comun.TIPODATO_CODIGO_TEXTAREA:
                                    ControlAtributo = new TextArea();
                                    break;
                                default:
                                    ControlAtributo = new TextField();
                                    break;
                            }
                            ControlAtributo.Cls = "txtContainerCategorias";
                            break;
                        #endregion

                        default:
                            break;
                    }
                    contenedorControl.Items.Add(ControlAtributo);

                }

                if (Update)
                {
                    contenedorControl.UpdateContent();
                    hdTipoComponente.Value = ControlAtributo.GetType().Name;
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

        [DirectMethod]
        public void MoverElementoOrden(long Orden)
        {
            try
            {
                switch (this.TipoAtributo)
                {
                    case Comun.MODULOINVENTARIO:

                        CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController cInvCatVin = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController();
                        Data.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos oDato = cInvCatVin.GetVinculacion(this.CategoriaAtributoID, this.AtributoID);

                        if (oDato != null)
                        {
                            oDato.Orden = (int)Orden;
                            cInvCatVin.UpdateItem(oDato);
                        }

                        break;

                    case Comun.EMPLAZAMIENTOS:

                        EmplazamientosAtributosConfiguracionController cEmplazamientosAtributos = new EmplazamientosAtributosConfiguracionController();
                        cEmplazamientosAtributos.ActualizarOrdenAtributo(this.AtributoID, Orden);

                        break;

                    case Comun.PRODUCT_CATALOG_SERVICIOS:

                        CoreProductCatalogServiciosAtributosConfiguracionesController cAtrib = new CoreProductCatalogServiciosAtributosConfiguracionesController();
                        cAtrib.ActualizarOrdenAtributo(this.AtributoID, Orden);

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        #endregion

        #region SEMAFORO

        public class Rangos
        {
            public double value;
            public string type;
        }

        [DirectMethod()]
        public DirectResponse ObtenerDatosSemaforo()
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            Data.Vw_CoreAtributosConfiguracionTiposDatosPropiedades AtributoConfiguracion = new Data.Vw_CoreAtributosConfiguracionTiposDatosPropiedades();
            CoreAtributosConfiguracionTiposDatosPropiedadesController cAtributoConfiguracion = new CoreAtributosConfiguracionTiposDatosPropiedadesController();
            Data.CoreAtributosConfiguracionSemaforos ConfiguracionSemaforo = new Data.CoreAtributosConfiguracionSemaforos();
            CoreAtributosConfiguracionSemaforosController cConfiguracionSemaforo = new CoreAtributosConfiguracionSemaforosController();
            List<Data.CoreSemaforosRangosNumericos> listaRangosNumericos = new List<Data.CoreSemaforosRangosNumericos>();
            CoreSemaforosRangosNumericosController cRangosNumericos = new CoreSemaforosRangosNumericosController();
            CoreSemaforosColoresController cColores = new CoreSemaforosColoresController();
            Data.CoreSemaforos oSemaforo = new Data.CoreSemaforos();
            CoreSemaforosController cSemaforo = new CoreSemaforosController();


            string sMaxValue = null;
            string sMinValue = null;
            string sValores = "";
            bool bNoTieneValores = true;
            string sColorDefecto;
            try
            {
                ConfiguracionSemaforo = cConfiguracionSemaforo.GetItem("CoreAtributoConfiguracionID==" + _AtributoID);

                if (ConfiguracionSemaforo != null)
                {
                    oSemaforo = cSemaforo.GetItem(ConfiguracionSemaforo.CoreSemaforoID);
                    listaRangosNumericos = cRangosNumericos.GetItemsList("CoreSemaforoID==" + ConfiguracionSemaforo.CoreSemaforoID).OrderBy(c => c.CoreSemaforoRangoNumericoID).ToList();

                    if (listaRangosNumericos.Count > 0)
                    {
                        sValores = cColores.GetItem(oSemaforo.CoreSemaforoColorDefectoID).Nombre;
                        sValores = sValores + ";" + listaRangosNumericos[0].Rango.ToString();
                        sValores = sValores + ";" + listaRangosNumericos.Last().Rango;
                        List<Rangos> listaRangos = new List<Rangos>();
                        for (int i = 0; i < listaRangosNumericos.Count - 1; i++)
                        {
                            if (i != 0)
                            {
                                Rangos Rang = new Rangos();
                                Rang.value = listaRangosNumericos[i].Rango;
                                Rang.type = cColores.GetItem(listaRangosNumericos[i].CoreSemaforoColorID).Nombre;
                                listaRangos.Add(Rang);
                            }
                        }
                        if (listaRangos.Count > 0)
                        {
                            sValores = sValores + ";" + Newtonsoft.Json.JsonConvert.SerializeObject(listaRangos);
                        }
                        bNoTieneValores = false;
                    }

                }


                if (bNoTieneValores)
                {
                    sColorDefecto = cColores.GetColorDefecto(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
                    sMaxValue = cAtributoConfiguracion.GetMaxValue(_AtributoID);
                    sMinValue = cAtributoConfiguracion.GetMinValue(_AtributoID);

                    if (sMaxValue != null && sMinValue != null)
                    {
                        sValores = sColorDefecto + ";" + sMinValue + ";" + sMaxValue;
                    }
                    else
                    {
                        sValores = "No MaxMinValue" + ";" + sColorDefecto;
                    }
                }

                direct.Result = sValores;

            }
            catch (Exception)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }
            return direct;
        }

        [DirectMethod()]
        public DirectResponse GuardarSemaforo(string JsonRangos)
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            Random rnd = new Random();
            int aleatorio = rnd.Next(100000, 10000000);
            Data.CoreSemaforos oSemaforo = new Data.CoreSemaforos();
            CoreSemaforosController cSemaforo = new CoreSemaforosController();
            List<Data.CoreAtributosConfiguracionSemaforos> listConfiguracionSemaforo = new List<Data.CoreAtributosConfiguracionSemaforos>();
            Data.CoreAtributosConfiguracionSemaforos oConfiguracionSemaforo = new Data.CoreAtributosConfiguracionSemaforos();
            CoreAtributosConfiguracionSemaforosController cConfiguracionSemaforo = new CoreAtributosConfiguracionSemaforosController();
            CoreSemaforosColoresController cColor = new CoreSemaforosColoresController();
            Data.CoreSemaforosRangosNumericos oRangos = null;
            List<Data.CoreSemaforosRangosNumericos> listRangos = new List<Data.CoreSemaforosRangosNumericos>();
            CoreSemaforosRangosNumericosController cRangos = new CoreSemaforosRangosNumericosController();

            try
            {

                listConfiguracionSemaforo = cConfiguracionSemaforo.ExisteSemaforoByConfiguracion(_AtributoID);
                if (listConfiguracionSemaforo.Count > 0)
                {
                    //No se crea un semaforo pero se editan los valores de la tabla CoreSemaforosRangosNumericos
                    if (listConfiguracionSemaforo.Count == 1)
                    {
                        listRangos = cRangos.GetItemsList("CoreSemaforoID==" + listConfiguracionSemaforo[0].CoreSemaforoID).OrderByDescending(c => c.CoreSemaforoRangoNumericoID).ToList();

                        foreach (Data.CoreSemaforosRangosNumericos item in listRangos)
                        {
                            cRangos.DeleteItem(item.CoreSemaforoRangoNumericoID);
                        }
                        #region CREACION RANGOS NUMERICOS
                        List<Rangos> clRangos = new List<Rangos>();
                        clRangos = JSON.Deserialize<List<Rangos>>(JsonRangos);

                        long RangoID = 0;
                        foreach (Rangos item in clRangos)
                        {
                            oRangos = new Data.CoreSemaforosRangosNumericos();
                            oRangos.CoreSemaforoID = listConfiguracionSemaforo[0].CoreSemaforoID;
                            oRangos.Rango = item.value;
                            oRangos.CoreSemaforoColorID = cColor.GetItem("Nombre==\"" + item.type + "\"").CoreSemaforoColorID;
                            if (RangoID == 0)
                            {
                                oRangos.CoreSemaforoRangoNumericoAnteriorID = null;
                            }
                            else
                            {
                                oRangos.CoreSemaforoRangoNumericoAnteriorID = RangoID;
                            }

                            oRangos = cRangos.AddItem(oRangos);
                            RangoID = oRangos.CoreSemaforoRangoNumericoID;
                        }
                        #endregion
                    }
                    else
                    {
                        direct.Result = "The traffic light is being used in another configuration, it cannot be edited";
                        return direct;
                    }
                }
                else
                {
                    // Se crea un semaforo nuevo 
                    #region CREACION SEMAFORO
                    oSemaforo.Nombre = "Semaforo" + aleatorio;
                    oSemaforo.Codigo = "Semaforo" + aleatorio;
                    oSemaforo.Descripcion = "";
                    oSemaforo.CoreSemaforoColorDefectoID = cColor.GetColorDefectoID(long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString()));
                    oSemaforo.Activo = true;
                    oSemaforo.ClienteID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());
                    oSemaforo = cSemaforo.AddItem(oSemaforo);
                    #endregion

                    #region CREACION RANGOS NUMERICOS
                    List<Rangos> clRangos = new List<Rangos>();
                    clRangos = JSON.Deserialize<List<Rangos>>(JsonRangos);

                    long RangoID = 0;
                    foreach (Rangos item in clRangos)
                    {
                        oRangos = new Data.CoreSemaforosRangosNumericos();
                        oRangos.CoreSemaforoID = oSemaforo.CoreSemaforoID;
                        oRangos.Rango = item.value;
                        oRangos.CoreSemaforoColorID = cColor.GetItem("Nombre==\"" + item.type + "\"").CoreSemaforoColorID;
                        if (RangoID == 0)
                        {
                            oRangos.CoreSemaforoRangoNumericoAnteriorID = null;
                        }
                        else
                        {
                            oRangos.CoreSemaforoRangoNumericoAnteriorID = RangoID;
                        }

                        oRangos = cRangos.AddItem(oRangos);
                        RangoID = oRangos.CoreSemaforoRangoNumericoID;
                    }
                    #endregion

                    #region CREACION AtributosConfiguracionSemaforos
                    oConfiguracionSemaforo.CoreSemaforoID = oSemaforo.CoreSemaforoID;
                    oConfiguracionSemaforo.CoreAtributoConfiguracionID = _AtributoID;
                    cConfiguracionSemaforo.AddItem(oConfiguracionSemaforo);
                    #endregion
                }

            }
            catch (Exception)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }
            return direct;
        }
        #endregion

        #endregion

        #region FUNCTION

        //[DirectMethod()]
        //public DirectResponse MostrarDataSetting()
        //{
        //    DirectResponse direct = new DirectResponse();
        //    try
        //    {
        //        direct.Success = true;
        //        if (ControlAtributo != null && (ControlAtributo.GetType().Name == "ComboBox" || ControlAtributo.GetType().Name == "MultiCombo"))
        //        {
        //            direct.Result = true;
        //        }
        //        else
        //        {
        //            direct.Result = false;
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        direct.Success = false;
        //        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
        //    }
        //    return direct;
        //}

        public void SetAtributoUnico()
        {
            lbNombreAtr.SetEditable(false);
            menuItemEliminar.Hide();
            btnMoverAtributo.Hide();
            menuItemPerfiles.Hide();
            menuItemTrafficLight.Hide();
        }

        #endregion
    }
}