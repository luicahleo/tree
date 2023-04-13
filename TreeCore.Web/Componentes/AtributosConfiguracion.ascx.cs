using CapaNegocio;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Data.SqlClient;
using TreeCore.Page;
using System.Reflection;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace TreeCore.Componentes
{

    public partial class AtributosConfiguracion : BaseUserControl
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected string _TipoAtributo;
        protected long _AtributoID;
        protected ComponentBase ControlAtributo;

        public string TipoAtributo
        {
            get { return _TipoAtributo; }
            set { _TipoAtributo = value; }
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

        protected void Page_Init(object sender, EventArgs e)
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            PintarControlFormat(false);
        }

        #region STORES

        #region TABLAS

        protected void storeTablas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Object> lista = new List<object>();
                    Object objeto = null;

                    TablasModeloDatosController cTablas = new TablasModeloDatosController();
                    List<Data.TablasModeloDatos> listaTabla;
                    listaTabla = cTablas.GetTablas();

                    foreach (var mt in listaTabla)
                    {
                        objeto = new { TablaID = mt.TablaModeloDatosID, TablaNombre = (GetGlobalResource(mt.ClaveRecurso) != "") ? GetGlobalResource(mt.ClaveRecurso) : mt.NombreTabla };
                        lista.Add(objeto);
                    }

                    if (lista.Count > 0)
                    {
                        storeTablas.DataSource = lista;
                        storeTablas.DataBind();
                        if (hdDatabase.Value != null && hdDatabase.Value != "")
                        {
                            cmbTable.SetValue(hdDatabase.Value);
                            cmbTable.Triggers[0].Hidden = false;
                            hdDatabase.SetValue("");
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

        #region COLUMNAS
        protected void storeColumnas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Object> lista = new List<object>();
                Object objeto;

                try
                {
                    if (cmbTable.Value != null && cmbTable.Value != "")
                    {
                        ColumnasModeloDatosController cTablas = new ColumnasModeloDatosController();
                        List<Data.ColumnasModeloDatos> listaColumnas = cTablas.GetColumnasTablas(long.Parse(cmbTable.Value.ToString()));
                        foreach (var dm in listaColumnas)
                        {
                            objeto = new { ColumnaTablaID = dm.ColumnaModeloDatosID, ColumnaNombre = (GetGlobalResource(dm.ClaveRecurso) != "") ? GetGlobalResource(dm.ClaveRecurso) : dm.NombreColumna };
                            lista.Add(objeto);
                        }

                        if (lista.Count > 0)
                        {
                            storeColumnas.DataSource = lista;
                            storeColumnas.DataBind();
                            if (hdValue.Value != null && hdValue.Value.ToString() != "")
                            {
                                cmbValue.SetValue(hdValue.Value);
                                cmbValue.Triggers[0].Hidden = false;
                                hdValue.SetValue("");
                            }
                            if (hdTooltip.Value != null && hdTooltip.Value.ToString() != "")
                            {
                                List<long> listaTooltip = new List<long>();
                                foreach (var item in hdTooltip.Value.ToString().Split(','))
                                {
                                    listaTooltip.Add(long.Parse(item));
                                }
                                cmbToolTip.SetValue(listaTooltip);
                                cmbToolTip.Triggers[0].Hidden = false;
                                hdValue.SetValue("");
                            }
                        }
                    }
                    else if (hdDatabase.Value != null && hdDatabase.Value.ToString() != "")
                    {
                        ColumnasModeloDatosController cTablas = new ColumnasModeloDatosController();
                        List<Data.ColumnasModeloDatos> listaColumnas = cTablas.GetColumnasTablas(long.Parse(hdDatabase.Value.ToString()));
                        foreach (var dm in listaColumnas)
                        {
                            objeto = new { ColumnaTablaID = dm.ColumnaModeloDatosID, ColumnaNombre = (GetGlobalResource(dm.ClaveRecurso) != "") ? GetGlobalResource(dm.ClaveRecurso) : dm.NombreColumna };
                            lista.Add(objeto);
                        }

                        if (lista.Count > 0)
                        {
                            storeColumnas.DataSource = lista;
                            storeColumnas.DataBind();
                            if (hdValue.Value != null && hdValue.Value.ToString() != "")
                            {
                                cmbValue.SetValue(hdValue.Value);
                                cmbValue.Triggers[0].Hidden = false;
                                hdValue.SetValue("");
                            }
                            if (hdTooltip.Value != null && hdTooltip.Value.ToString() != "")
                            {
                                List<long> listaTooltip = new List<long>();
                                foreach (var item in hdTooltip.Value.ToString().Split(','))
                                {
                                    listaTooltip.Add(long.Parse(item));
                                }
                                cmbToolTip.SetValue(listaTooltip);
                                cmbToolTip.Triggers[0].Hidden = false;
                                hdTooltip.SetValue("");
                            }
                        }
                    }
                    else
                    {
                        storeColumnas.DataSource = new List<object>();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        protected class objectColAso
        {
            public long ID { get; set; }
            public string Name { get; set; }
            public long AtributoID { get; set; }
            public long ColumnaModeloDatoID { get; set; }
            public int Orden { get; set; }
        }
        protected void storeColumnasVinculadas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            CoreAtributosConfiguracionListasColumnasAdicionalesController cListas = new CoreAtributosConfiguracionListasColumnasAdicionalesController();
            EmplazamientosAtributosConfiguracionListasColumnasAdicionalesController cEmplListas = new EmplazamientosAtributosConfiguracionListasColumnasAdicionalesController();
            List<objectColAso> listaColumnas = new List<objectColAso>();
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    /*switch (Modulo)
                    {
                        case (long)Comun.Modulos.INVENTARIO:
                            var listaCols = cListas.GetColumnasAtributos(long.Parse(hdAtributoID.Value.ToString()));
                            if (listaCols != null && listaCols.Count > 0)
                            {
                                foreach (var oCol in listaCols)
                                {
                                    listaColumnas.Add(new objectColAso
                                    {
                                        ID = oCol.CoreAtributoConfiguracionListaColumnaAdicionalID,
                                        Name = GetGlobalResource(oCol.ColumnasModeloDatos.ClaveRecurso),
                                        AtributoID = oCol.CoreAtributoConfiguracionID,
                                        ColumnaModeloDatoID = oCol.ColumnaModeloDatoID,
                                        Orden = (int)oCol.Orden
                                    });
                                }
                            }
                            break;
                        case (long)Comun.Modulos.GLOBAL:
                            var listaColsEmpl = cEmplListas.GetColumnasFromAtributo(long.Parse(hdAtributoID.Value.ToString()));
                            if (listaColsEmpl != null && listaColsEmpl.Count > 0)
                            {
                                foreach (var oCol in listaColsEmpl)
                                {
                                    listaColumnas.Add(new objectColAso
                                    {
                                        ID = oCol.EmplazamientoAtributoConfiguracionListaColumnaAdicionalID,
                                        Name = GetGlobalResource(oCol.ColumnasModeloDatos.ClaveRecurso),
                                        AtributoID = oCol.EmplazamientosAtributoConfiguracionID,
                                        ColumnaModeloDatoID = oCol.ColumnaModeloDatoID,
                                        Orden = oCol.Orden
                                    });
                                }
                            }
                            break;
                        default:
                            break;
                    }*/
                    storeColumnasVinculadas.DataSource = listaColumnas;
                    storeColumnasVinculadas.DataBind();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        protected class objectCol
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public long ObjectID { get; set; }
            public long? ColumnaID { get; set; }
            public string IndiceColumna { get; set; }
            public bool esCarpeta { get; set; }
        }
        protected void storeSelectCamposVinculados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
                List<objectCol> listaColumnas = new List<objectCol>();
                try
                {
                    var TablaID = long.Parse(hdTablaActual.Value.ToString());
                    var listaCol = cColumnas.GetColumnasTablas(TablaID);

                    if (listaCol != null && listaCol.Count > 0)
                    {
                        foreach (var oCol in listaCol)
                        {
                            if (oCol.ForeignKeyID != null)
                            {
                                listaColumnas.Add(new objectCol
                                {
                                    ID = "Car" + oCol.ForeignKey.TablaModeloDatosID,
                                    Name = GetGlobalResource(oCol.ForeignKey.TablasModeloDatos.ClaveRecurso),
                                    ObjectID = oCol.ForeignKey.TablaModeloDatosID,
                                    ColumnaID = oCol.ColumnaModeloDatosID,
                                    IndiceColumna = oCol.NombreColumna,
                                    esCarpeta = true
                                });
                            }
                            else
                            {
                                listaColumnas.Add(new objectCol
                                {
                                    ID = "Col" + oCol.ColumnaModeloDatosID,
                                    Name = GetGlobalResource(oCol.ClaveRecurso),
                                    ObjectID = oCol.ColumnaModeloDatosID,
                                    esCarpeta = false
                                });
                            }
                        }
                    }

                    storeSelectCamposVinculados.DataSource = listaColumnas;
                    storeSelectCamposVinculados.DataBind();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }
        #endregion

        #region ROLES

        protected void storeRoles_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                RolesController cRoles = new RolesController();
                try
                {
                    List<Data.Roles> listaDatos;
                    switch (this.TipoAtributo)
                    {
                        case Comun.MODULO_WORKFLOW:
                        case Comun.PRODUCT_CATALOG_SERVICIOS:
                        case Comun.MODULOINVENTARIO:
                            listaDatos = cRoles.GetRolesLibresAtributosRestringidos(long.Parse(hdAtributoID.Value.ToString()));
                            break;
                        case Comun.EMPLAZAMIENTOS:
                            listaDatos = cRoles.GetRolesLibresEmplazamientosAtributosRestringidos(long.Parse(hdAtributoID.Value.ToString()));
                            break;
                        default:
                            listaDatos = new List<Data.Roles>();
                            break;
                    }

                    if (listaDatos != null)
                    {
                        storeRoles.DataSource = listaDatos;
                        storeRoles.DataBind();
                    }
                    else
                    {
                        storeRoles.DataSource = new List<Data.Roles>();
                        storeRoles.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region ROLES RESTRINGIDOS

        protected void storeRolesRestringidos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {

                try
                {
                    List<object> listaDatos = new List<object>();

                    switch (this.TipoAtributo)
                    {
                        case Comun.MODULO_WORKFLOW:
                        case Comun.PRODUCT_CATALOG_SERVICIOS:
                        case Comun.MODULOINVENTARIO:
                            CoreAtributosConfiguracionRolesRestringidosController cRoles = new CoreAtributosConfiguracionRolesRestringidosController();
                            List<Data.Vw_CoreAtributosConfiguracionRolesRestringidos> listaRoles;
                            listaRoles = cRoles.GetVwRolesFromAtributo(long.Parse(hdAtributoID.Value.ToString()));

                            if (listaRoles != null)
                            {
                                foreach (var item in listaRoles)
                                {
                                    listaDatos.Add(new
                                    {
                                        AtributoRolRestringidoID = item.CoreAtributoConfiguracionRolRestringidoID,
                                        Nombre = item.NombreRol,
                                        Restriccion = item.Restriccion
                                    });
                                }
                            }
                            break;

                        #region EMPLAZAMIENTOS

                        case Comun.EMPLAZAMIENTOS:
                            EmplazamientosAtributosConfiguracionRolesRestringidosController cRolesEmpl = new EmplazamientosAtributosConfiguracionRolesRestringidosController();
                            List<Data.Vw_EmplazamientosAtributosRolesRestringidos> listaDatosEmp;
                            listaDatosEmp = cRolesEmpl.GetAllRolesRestringidosAtributo(long.Parse(hdAtributoID.Value.ToString()));

                            if (listaDatosEmp != null)
                            {
                                foreach (var item in listaDatosEmp)
                                {
                                    listaDatos.Add(new
                                    {
                                        AtributoRolRestringidoID = item.EmplazamientoAtributoRolRestringidoID,
                                        Nombre = item.Nombre,
                                        Restriccion = item.Restriccion
                                    });
                                }
                            }
                            break;

                        #endregion

                        default:
                            break;
                    }

                    if (listaDatos != null)
                    {
                        storeRolesRestringidos.DataSource = listaDatos;
                        storeRolesRestringidos.DataBind();
                    }
                    else
                    {
                        storeRolesRestringidos.DataSource = new List<string>();
                        storeRolesRestringidos.DataBind();
                    }

                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region FORMATOS

        protected void storeFomatos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {

                try
                {
                    List<object> listaDatos = new List<object>();
                    switch (this.TipoAtributo)
                    {
                        case Comun.MODULO_WORKFLOW:
                        case Comun.PRODUCT_CATALOG_SERVICIOS:
                        case Comun.MODULOINVENTARIO:
                            CoreAtributosConfiguracionTiposDatosPropiedadesController cPropiedades = new CoreAtributosConfiguracionTiposDatosPropiedadesController();
                            List<Data.Vw_CoreAtributosConfiguracionTiposDatosPropiedades> listaDatosINV;
                            listaDatosINV = cPropiedades.GetVwPropiedadesFromAtributo(long.Parse(hdAtributoID.Value.ToString()));

                            if (listaDatosINV != null)
                            {
                                foreach (var item in listaDatosINV)
                                {
                                    if (item.TipoValor == "Date")
                                    {
                                        listaDatos.Add(new
                                        {
                                            AtributoTipoDatoPropiedadID = item.CoreAtributoConfiguracionTipoDatoPropiedadID,
                                            Nombre = item.NombreTipoDatoPropiedad,
                                            Valor = DateTime.Parse(item.Valor).ToString(Comun.FORMATO_FECHA)
                                        });
                                    }
                                    else
                                    {
                                        listaDatos.Add(new
                                        {
                                            AtributoTipoDatoPropiedadID = item.CoreAtributoConfiguracionTipoDatoPropiedadID,
                                            Nombre = item.NombreTipoDatoPropiedad,
                                            Valor = item.Valor
                                        });
                                    }
                                }
                            }
                            break;

                        #region EMPLAZAMIENTOS

                        case Comun.EMPLAZAMIENTOS:
                            EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController cEmplazamientosPropiedades = new EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController();
                            List<Data.Vw_EmplazamientosAtributosTiposDatosPropiedades> listaDatosEMP;
                            listaDatosEMP = cEmplazamientosPropiedades.GetPropiedadesFromAtributo(long.Parse(hdAtributoID.Value.ToString()));

                            if (listaDatosEMP != null)
                            {
                                foreach (var item in listaDatosEMP)
                                {
                                    listaDatos.Add(new
                                    {
                                        AtributoTipoDatoPropiedadID = item.EmplazamientoAtributoTipoDatoPropiedadID,
                                        Nombre = item.Nombre,
                                        Valor = item.Valor
                                    });
                                }
                            }
                            break;

                        #endregion

                        default:
                            break;
                    }
                    if (listaDatos != null)
                    {
                        storeFomatos.DataSource = listaDatos;
                        storeFomatos.DataBind();
                    }
                    else
                    {
                        storeFomatos.DataSource = new List<string>();
                        storeFomatos.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region PROPIEDADESTIPODATOS

        protected void storeTiposPropiedades_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {

                try
                {
                    List<Data.TiposDatosPropiedades> listaDatos;

                    switch (this.TipoAtributo)
                    {
                        case Comun.MODULO_WORKFLOW:
                        case Comun.PRODUCT_CATALOG_SERVICIOS:
                        case Comun.MODULOINVENTARIO:
                            CoreAtributosConfiguracionTiposDatosPropiedadesController cPropiedades = new CoreAtributosConfiguracionTiposDatosPropiedadesController();
                            CoreAtributosConfiguracionesController cAtributo = new CoreAtributosConfiguracionesController();
                            listaDatos = cPropiedades.GetPropiedadesLibresAtributo(long.Parse(hdAtributoID.Value.ToString()), cAtributo.GetItem(long.Parse(hdAtributoID.Value.ToString())).TipoDatoID);
                            break;
                        case Comun.EMPLAZAMIENTOS:
                            EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController cEmplazamientosPropiedades = new EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController();
                            EmplazamientosAtributosConfiguracionController cEmplazamientoAtr = new EmplazamientosAtributosConfiguracionController();
                            listaDatos = cEmplazamientosPropiedades.GetPropiedadesLibresEmplazamientpAtributo(long.Parse(hdAtributoID.Value.ToString()), cEmplazamientoAtr.GetItem(long.Parse(hdAtributoID.Value.ToString())).TipoDatoID);
                            break;
                        default:
                            listaDatos = new List<Data.TiposDatosPropiedades>();
                            break;
                    }


                    if (listaDatos != null)
                    {
                        storeTiposPropiedades.DataSource = listaDatos;
                        storeTiposPropiedades.DataBind();
                    }
                    else
                    {
                        storeTiposPropiedades.DataSource = new List<string>();
                        storeTiposPropiedades.DataBind();
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

        #region Direct Methods

        #region FORMAT

        [DirectMethod()]
        public DirectResponse CambiarValorFormat()
        {
            DirectResponse direct = new DirectResponse();
            TiposDatosPropiedadesController cPropiedades = new TiposDatosPropiedadesController();
            Data.TiposDatosPropiedades oPropiedad;

            try
            {
                if (cmbTiposPropiedades.SelectedItem.Value != null && cmbTiposPropiedades.SelectedItem.Value != "")
                {
                    ContenedorCampoValueFormat.Items.Clear();
                    ContenedorCampoValueFormat.Show();
                    oPropiedad = cPropiedades.GetItem(long.Parse(cmbTiposPropiedades.SelectedItem.Value));

                    #region GetPropertyInfo

                    Data.TiposDatos oTipoDato = new Data.TiposDatos();
                    CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
                    TiposDatosController cTipoDatos = new TiposDatosController();
                    EmplazamientosAtributosConfiguracionController cEmplAtr = new EmplazamientosAtributosConfiguracionController();

                    switch (this.TipoAtributo)
                    {
                        case Comun.MODULO_WORKFLOW:
                        case Comun.PRODUCT_CATALOG_SERVICIOS:
                        case Comun.MODULOINVENTARIO:
                            Data.CoreAtributosConfiguraciones oDato = cAtributos.GetItem(long.Parse(hdAtributoID.Value.ToString()));
                            oTipoDato = cTipoDatos.GetItem(oDato.TipoDatoID);
                            break;

                        case Comun.EMPLAZAMIENTOS:
                            Data.EmplazamientosAtributosConfiguracion oDatoEmpl = cEmplAtr.GetItem(long.Parse(hdAtributoID.Value.ToString()));
                            oTipoDato = cTipoDatos.GetItem(oDatoEmpl.TipoDatoID);
                            break;
                    }
                    switch (oTipoDato.Codigo)
                    {
                        case Comun.TIPODATO_CODIGO_TEXTO:
                            ControlAtributo = new TextField();
                            break;
                        case Comun.TIPODATO_CODIGO_NUMERICO:
                            ControlAtributo = new NumberField();
                            break;
                        case Comun.TIPODATO_CODIGO_FECHA:
                            ControlAtributo = new DateField();
                            break;
                        case Comun.TIPODATO_CODIGO_LISTA:
                            ControlAtributo = new ComboBox();
                            break;
                        case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                            ControlAtributo = new MultiCombo();
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

                    #endregion

                    var propertyInfo = ControlAtributo.GetType().GetProperty(oPropiedad.Codigo);
                    if (oPropiedad.Codigo == "Value")
                    {
                        ContenedorCampoValueFormat.Items.Add(new NumberField
                        {
                            ID = "numCampoValueFormat",
                            AllowBlank = false,
                            Value = 0,
                            FieldLabel = GetGlobalResource("strValor"),
                            LabelAlign = LabelAlign.Top
                        });
                    }
                    else if (propertyInfo != null)
                    {
                        switch (Type.GetTypeCode(propertyInfo.PropertyType))
                        {
                            case TypeCode.Boolean:
                                ContenedorCampoValueFormat.Items.Add(new Checkbox
                                {
                                    ID = "chkCampoValueFormat",
                                    BoxLabel = GetGlobalResource("strValor"),
                                    BoxLabelAlign = BoxLabelAlign.After
                                });
                                break;
                            case TypeCode.Double:
                                ContenedorCampoValueFormat.Items.Add(new NumberField
                                {
                                    ID = "numCampoValueFormat",
                                    AllowBlank = false,
                                    FieldLabel = GetGlobalResource("strValor"),
                                    LabelAlign = LabelAlign.Top
                                });
                                break;
                            case TypeCode.Int16:
                                ContenedorCampoValueFormat.Items.Add(new NumberField
                                {
                                    ID = "numCampoValueFormat",
                                    AllowBlank = false,
                                    FieldLabel = GetGlobalResource("strValor"),
                                    LabelAlign = LabelAlign.Top
                                });
                                break;
                            case TypeCode.Int32:
                                ContenedorCampoValueFormat.Items.Add(new NumberField
                                {
                                    ID = "numCampoValueFormat",
                                    AllowBlank = false,
                                    FieldLabel = GetGlobalResource("strValor"),
                                    LabelAlign = LabelAlign.Top
                                });
                                break;
                            case TypeCode.Int64:
                                ContenedorCampoValueFormat.Items.Add(new NumberField
                                {
                                    ID = "numCampoValueFormat",
                                    AllowBlank = false,
                                    FieldLabel = GetGlobalResource("strValor"),
                                    LabelAlign = LabelAlign.Top
                                });
                                break;
                            case TypeCode.UInt16:
                                ContenedorCampoValueFormat.Items.Add(new NumberField
                                {
                                    ID = "numCampoValueFormat",
                                    AllowBlank = false,
                                    FieldLabel = GetGlobalResource("strValor"),
                                    LabelAlign = LabelAlign.Top
                                });
                                break;
                            case TypeCode.UInt32:
                                ContenedorCampoValueFormat.Items.Add(new NumberField
                                {
                                    ID = "numCampoValueFormat",
                                    AllowBlank = false,
                                    FieldLabel = GetGlobalResource("strValor"),
                                    LabelAlign = LabelAlign.Top
                                });
                                break;
                            case TypeCode.UInt64:
                                ContenedorCampoValueFormat.Items.Add(new NumberField
                                {
                                    ID = "numCampoValueFormat",
                                    AllowBlank = false,
                                    FieldLabel = GetGlobalResource("strValor"),
                                    LabelAlign = LabelAlign.Top
                                });
                                break;
                            case TypeCode.String:
                                ContenedorCampoValueFormat.Items.Add(new TextField
                                {
                                    ID = "txtCampoValueFormat",
                                    AllowBlank = false,
                                    Text = "",
                                    MaxLength = 100,
                                    FieldLabel = GetGlobalResource("strValor"),
                                    LabelAlign = LabelAlign.Top
                                });
                                break;
                            case TypeCode.DateTime:
                                ContenedorCampoValueFormat.Items.Add(new DateField
                                {
                                    ID = "datCampoValueFormat",
                                    AllowBlank = false,
                                    FieldLabel = GetGlobalResource("strValor"),
                                    Format = "dd/MM/Y",
                                    LabelAlign = LabelAlign.Top
                                });
                                break;
                            default:
                                ContenedorCampoValueFormat.Items.Add(new TextField
                                {
                                    ID = "txtCampoValueFormat",
                                    AllowBlank = false,
                                    Text = "",
                                    MaxLength = 50,
                                    FieldLabel = GetGlobalResource("strValor"),
                                    LabelAlign = LabelAlign.Top
                                });
                                break;
                        }
                        if (oPropiedad.AplicaReglas)
                        {
                            switch (this.TipoAtributo)
                            {
                                case Comun.MODULO_WORKFLOW:
                                case Comun.PRODUCT_CATALOG_SERVICIOS:
                                case Comun.MODULOINVENTARIO:
                                    CoreAtributosConfiguracionTiposDatosPropiedadesController cProdPropiedades = new CoreAtributosConfiguracionTiposDatosPropiedadesController();
                                    List<Data.Vw_CoreAtributosConfiguracionTiposDatosPropiedades> listaProdPropiedades = cProdPropiedades.GetVwPropiedadesFromAtributo(long.Parse(hdAtributoID.Value.ToString()));

                                    #region Propiedades

                                    foreach (var item in listaProdPropiedades)
                                    {
                                        try
                                        {
                                            propertyInfo = ContenedorCampoValueFormat.Items[0].GetType().GetProperty(item.CodigoTipoDatoPropiedad);
                                            if (propertyInfo != null)
                                            {
                                                switch (Type.GetTypeCode(propertyInfo.PropertyType))
                                                {
                                                    case TypeCode.Boolean:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], bool.Parse(item.Valor));
                                                        break;
                                                    case TypeCode.Double:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], Double.Parse(item.Valor));
                                                        break;
                                                    case TypeCode.Int16:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], int.Parse(item.Valor));
                                                        break;
                                                    case TypeCode.Int32:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], int.Parse(item.Valor));
                                                        break;
                                                    case TypeCode.Int64:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], long.Parse(item.Valor));
                                                        break;
                                                    case TypeCode.String:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], item.Valor);
                                                        break;
                                                    case TypeCode.DateTime:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], DateTime.Parse(item.Valor));
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }

                                    propertyInfo = ContenedorCampoValueFormat.Items[0].GetType().GetProperty("AllowBlank");
                                    propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], false);

                                    propertyInfo = ContenedorCampoValueFormat.Items[0].GetType().GetProperty("Disabled");
                                    propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], false);

                                    #endregion

                                    break;
                                #region EMPLAZAMIENTOS

                                case Comun.EMPLAZAMIENTOS:
                                    EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController cEmplazamietosPropiedades = new EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController();
                                    List<Data.Vw_EmplazamientosAtributosTiposDatosPropiedades> listaPropiedadesEmpl = cEmplazamietosPropiedades.GetPropiedadesFromAtributo(long.Parse(hdAtributoID.Value.ToString()));

                                    #region Propiedades

                                    foreach (var item in listaPropiedadesEmpl)
                                    {
                                        try
                                        {
                                            propertyInfo = ContenedorCampoValueFormat.Items[0].GetType().GetProperty(item.Codigo);
                                            if (propertyInfo != null)
                                            {
                                                switch (Type.GetTypeCode(propertyInfo.PropertyType))
                                                {
                                                    case TypeCode.Boolean:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], bool.Parse(item.Valor));
                                                        break;
                                                    case TypeCode.Double:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], Double.Parse(item.Valor));
                                                        break;
                                                    case TypeCode.Int16:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], int.Parse(item.Valor));
                                                        break;
                                                    case TypeCode.Int32:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], int.Parse(item.Valor));
                                                        break;
                                                    case TypeCode.Int64:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], long.Parse(item.Valor));
                                                        break;
                                                    case TypeCode.String:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], item.Valor);
                                                        break;
                                                    case TypeCode.DateTime:
                                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], DateTime.Parse(item.Valor));
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }

                                    propertyInfo = ContenedorCampoValueFormat.Items[0].GetType().GetProperty("AllowBlank");
                                    propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], false);

                                    propertyInfo = ContenedorCampoValueFormat.Items[0].GetType().GetProperty("Disabled");
                                    propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], false);

                                    #endregion

                                    break;

                                #endregion

                                default:
                                    break;
                            }
                        }
                    }
                    ContenedorCampoValueFormat.UpdateContent();
                }
                else
                {
                    ContenedorCampoValueFormat.Hide();
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
        public DirectResponse GuardarFormat()
        {
            DirectResponse direct = new DirectResponse();
            TiposDatosPropiedadesController cPropiedades = new TiposDatosPropiedadesController();
            Data.TiposDatosPropiedades oPropiedad;
            string sValor = "";
            try
            {
                direct.Success = true;
                direct.Result = "";

                oPropiedad = cPropiedades.GetItem(long.Parse(cmbTiposPropiedades.SelectedItem.Value));
                PintarControlFormat(false);
                if (oPropiedad.Codigo == "ToolTips")
                {
                    TextField txt = (TextField)X.GetCmp("txtCampoValueFormat");
                    sValor = txt.Value.ToString();
                    txt.Reset();
                }
                else if (oPropiedad.Codigo == "Value")
                {
                    NumberField num = (NumberField)X.GetCmp("numCampoValueFormat");
                    sValor = num.Value.ToString();
                    num.Reset();
                }
                else
                {
                    #region GetPropertyInfo

                    Data.TiposDatos oTipoDato = new Data.TiposDatos();
                    CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
                    TiposDatosController cTipoDatos = new TiposDatosController();
                    EmplazamientosAtributosConfiguracionController cEmplAtr = new EmplazamientosAtributosConfiguracionController();

                    switch (this.TipoAtributo)
                    {
                        case Comun.MODULO_WORKFLOW:
                        case Comun.PRODUCT_CATALOG_SERVICIOS:
                        case Comun.MODULOINVENTARIO:
                            Data.CoreAtributosConfiguraciones oDato = cAtributos.GetItem(long.Parse(hdAtributoID.Value.ToString()));
                            oTipoDato = cTipoDatos.GetItem(oDato.TipoDatoID);
                            break;

                        case Comun.EMPLAZAMIENTOS:
                            Data.EmplazamientosAtributosConfiguracion oDatoEmpl = cEmplAtr.GetItem(long.Parse(hdAtributoID.Value.ToString()));
                            oTipoDato = cTipoDatos.GetItem(oDatoEmpl.TipoDatoID);
                            break;
                    }
                    switch (oTipoDato.Codigo)
                    {
                        case Comun.TIPODATO_CODIGO_TEXTO:
                            ControlAtributo = new TextField();
                            break;
                        case Comun.TIPODATO_CODIGO_NUMERICO:
                            ControlAtributo = new NumberField();
                            break;
                        case Comun.TIPODATO_CODIGO_FECHA:
                            ControlAtributo = new DateField();
                            break;
                        case Comun.TIPODATO_CODIGO_LISTA:
                            ControlAtributo = new ComboBox();
                            break;
                        case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                            ControlAtributo = new MultiCombo();
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

                    #endregion

                    var propertyInfo = ControlAtributo.GetType().GetProperty(oPropiedad.Codigo);
                    if (propertyInfo != null)
                    {
                        switch (Type.GetTypeCode(propertyInfo.PropertyType))
                        {
                            case TypeCode.Boolean:
                                Checkbox chk = (Checkbox)X.GetCmp("chkCampoValueFormat");
                                sValor = chk.Checked.ToString();
                                chk.Reset();
                                break;
                            case TypeCode.Double:
                                NumberField num = (NumberField)X.GetCmp("numCampoValueFormat");
                                sValor = num.Value.ToString();
                                num.Reset();
                                break;
                            case TypeCode.Int16:
                                NumberField num0 = (NumberField)X.GetCmp("numCampoValueFormat");
                                sValor = num0.Value.ToString();
                                num0.Reset();
                                break;
                            case TypeCode.Int32:
                                NumberField num1 = (NumberField)X.GetCmp("numCampoValueFormat");
                                sValor = num1.Value.ToString();
                                num1.Reset();
                                break;
                            case TypeCode.Int64:
                                NumberField num2 = (NumberField)X.GetCmp("numCampoValueFormat");
                                sValor = num2.Value.ToString();
                                num2.Reset();
                                break;
                            case TypeCode.String:
                                TextField txt = (TextField)X.GetCmp("txtCampoValueFormat");
                                sValor = txt.Value.ToString();
                                txt.Reset();
                                break;
                            case TypeCode.DateTime:
                                DateField dat = (DateField)X.GetCmp("datCampoValueFormat");
                                sValor = DateTime.ParseExact(dat.RawValue.ToString(), Comun.FORMATO_FECHA, null).ToString();
                                dat.Reset();
                                break;
                            default:
                                sValor = "";
                                break;
                        }
                    }
                }
                switch (this.TipoAtributo)
                {
                    case Comun.MODULO_WORKFLOW:
                    case Comun.PRODUCT_CATALOG_SERVICIOS:
                    case Comun.MODULOINVENTARIO:
                        CoreAtributosConfiguracionTiposDatosPropiedadesController cAtributoPropiedad = new CoreAtributosConfiguracionTiposDatosPropiedadesController();
                        Data.CoreAtributosConfiguracionTiposDatosPropiedades oAtributoPropiedad = new Data.CoreAtributosConfiguracionTiposDatosPropiedades
                        {
                            CoreAtributoConfiguracionID = long.Parse(hdAtributoID.Value.ToString()),
                            TipoDatoPropiedadID = oPropiedad.TipoDatoPropiedadID,
                            Valor = sValor
                        };
                        if (cAtributoPropiedad.AddItem(oAtributoPropiedad) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource("LogErrorRule");
                            return direct;
                        }
                        break;

                    case Comun.EMPLAZAMIENTOS:
                        EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController cEmplazamientoPropiedad = new EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController();
                        Data.EmplazamientosAtributosTiposDatosPropiedades oEmplazamientoPropiedad = new Data.EmplazamientosAtributosTiposDatosPropiedades
                        {
                            EmplazamientoAtributoConfiguracionID = long.Parse(hdAtributoID.Value.ToString()),
                            TipoDatoPropiedadID = oPropiedad.TipoDatoPropiedadID,
                            Valor = sValor
                        };
                        if (cEmplazamientoPropiedad.ComprobarPropiedad(oPropiedad, oEmplazamientoPropiedad))
                        {
                            if (cEmplazamientoPropiedad.AddItem(oEmplazamientoPropiedad) != null)
                            {
                                log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            }
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource("LogErrorRule");
                            return direct;
                        }
                        break;

                    default:
                        break;
                }

                direct.Result = "CAT" + hdCategoriaAtributoID.Value + "_ATR" + hdAtributoID.Value;
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
        public DirectResponse EliminarFormato(long idFormatoAtributo)
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                switch (this.TipoAtributo)
                {
                    case Comun.MODULO_WORKFLOW:
                    case Comun.PRODUCT_CATALOG_SERVICIOS:
                    case Comun.MODULOINVENTARIO:
                        CoreAtributosConfiguracionTiposDatosPropiedadesController cFormatosAtributos = new CoreAtributosConfiguracionTiposDatosPropiedadesController();
                        if (cFormatosAtributos.DeleteItem(idFormatoAtributo))
                        {
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            direct.Success = true;
                            direct.Result = "";
                        }
                        break;
                    case Comun.EMPLAZAMIENTOS:
                        EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController cEmplazamientosFormatosAtributos = new EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController();
                        if (cEmplazamientosFormatosAtributos.DeleteItem(idFormatoAtributo))
                        {
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            direct.Success = true;
                            direct.Result = "";
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

            direct.Success = true;
            direct.Result = "CAT" + hdCategoriaAtributoID.Value + "_ATR" + hdAtributoID.Value;

            return direct;
        }

        [DirectMethod]
        public void LimpiarContenedorFormat()
        {
            try
            {
                ContenedorCampoValueFormat.Render();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        [DirectMethod]
        public void PintarControlFormat(bool Update)
        {
            TiposDatosPropiedadesController cPropiedades = new TiposDatosPropiedadesController();
            Data.TiposDatosPropiedades oPropiedad;
            Data.TiposDatos oTipoDato = new Data.TiposDatos();
            CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
            EmplazamientosAtributosConfiguracionController cEmplAtr = new EmplazamientosAtributosConfiguracionController();
            TiposDatosController cTipoDatos = new TiposDatosController();
            try
            {
                if (ContenedorCampoValueFormat != null && ContenedorCampoValueFormat.Items.Count == 0)
                {
                    if (cmbTiposPropiedades != null && cmbTiposPropiedades.SelectedItem.Value != null && cmbTiposPropiedades.SelectedItem.Value != "")
                    {
                        oPropiedad = cPropiedades.GetItem(long.Parse(cmbTiposPropiedades.SelectedItem.Value));

                        #region GetPropertyInfo

                        switch (this.TipoAtributo)
                        {
                            case Comun.MODULO_WORKFLOW:
                            case Comun.PRODUCT_CATALOG_SERVICIOS:
                            case Comun.MODULOINVENTARIO:
                                Data.CoreAtributosConfiguraciones oDato = cAtributos.GetItem(long.Parse(hdAtributoID.Value.ToString()));
                                if (oTipoDato != null)
                                {
                                    oTipoDato = cTipoDatos.GetItem(oDato.TipoDatoID);
                                }
                                break;

                            case Comun.EMPLAZAMIENTOS:
                                Data.EmplazamientosAtributosConfiguracion oDatoEmpl = cEmplAtr.GetItem(long.Parse(hdAtributoID.Value.ToString()));
                                oTipoDato = cTipoDatos.GetItem(oDatoEmpl.TipoDatoID);
                                break;
                        }
                        switch (oTipoDato.Codigo)
                        {
                            case Comun.TIPODATO_CODIGO_TEXTO:
                                ControlAtributo = new TextField();
                                break;
                            case Comun.TIPODATO_CODIGO_NUMERICO:
                                ControlAtributo = new NumberField();
                                break;
                            case Comun.TIPODATO_CODIGO_FECHA:
                                ControlAtributo = new DateField();
                                break;
                            case Comun.TIPODATO_CODIGO_LISTA:
                                ControlAtributo = new ComboBox();
                                break;
                            case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                                ControlAtributo = new MultiCombo();
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

                        #endregion

                        var propertyInfo = ControlAtributo.GetType().GetProperty(oPropiedad.Codigo);
                        if (oPropiedad.Codigo == "Value")
                        {
                            ContenedorCampoValueFormat.Items.Add(new NumberField
                            {
                                ID = "numCampoValueFormat",
                                AllowBlank = false,
                                Value = 0,
                                FieldLabel = GetGlobalResource("strValor"),
                                LabelAlign = LabelAlign.Top
                            });
                        }
                        else if (propertyInfo != null)
                        {
                            switch (Type.GetTypeCode(propertyInfo.PropertyType))
                            {
                                case TypeCode.Boolean:
                                    ContenedorCampoValueFormat.Items.Add(new Checkbox
                                    {
                                        ID = "chkCampoValueFormat",
                                        BoxLabel = GetGlobalResource("strValor"),
                                        BoxLabelAlign = BoxLabelAlign.After
                                    });
                                    break;
                                case TypeCode.Double:
                                    ContenedorCampoValueFormat.Items.Add(new NumberField
                                    {
                                        ID = "numCampoValueFormat",
                                        AllowBlank = false,
                                        Value = 0,
                                        FieldLabel = GetGlobalResource("strValor"),
                                        LabelAlign = LabelAlign.Top,
                                        MinValue = Double.NegativeInfinity,
                                        MaxLength = 100
                                    });
                                    break;
                                case TypeCode.Int16:
                                    ContenedorCampoValueFormat.Items.Add(new NumberField
                                    {
                                        ID = "numCampoValueFormat",
                                        AllowBlank = false,
                                        Value = 0,
                                        FieldLabel = GetGlobalResource("strValor"),
                                        LabelAlign = LabelAlign.Top,
                                        MinValue = Double.NegativeInfinity,
                                        MaxLength = 100
                                    });
                                    break;
                                case TypeCode.Int32:
                                    ContenedorCampoValueFormat.Items.Add(new NumberField
                                    {
                                        ID = "numCampoValueFormat",
                                        AllowBlank = false,
                                        Value = 0,
                                        FieldLabel = GetGlobalResource("strValor"),
                                        LabelAlign = LabelAlign.Top,
                                        MinValue = Double.NegativeInfinity,
                                        MaxLength = 100
                                    });
                                    break;
                                case TypeCode.Int64:
                                    ContenedorCampoValueFormat.Items.Add(new NumberField
                                    {
                                        ID = "numCampoValueFormat",
                                        AllowBlank = false,
                                        Value = 0,
                                        FieldLabel = GetGlobalResource("strValor"),
                                        LabelAlign = LabelAlign.Top,
                                        MinValue = Double.NegativeInfinity,
                                        MaxLength = 100
                                    });
                                    break;
                                case TypeCode.UInt16:
                                    ContenedorCampoValueFormat.Items.Add(new NumberField
                                    {
                                        ID = "numCampoValueFormat",
                                        AllowBlank = false,
                                        Value = 0,
                                        FieldLabel = GetGlobalResource("strValor"),
                                        LabelAlign = LabelAlign.Top,
                                        MinValue = Double.NegativeInfinity,
                                        MaxLength = 100
                                    });
                                    break;
                                case TypeCode.UInt32:
                                    ContenedorCampoValueFormat.Items.Add(new NumberField
                                    {
                                        ID = "numCampoValueFormat",
                                        AllowBlank = false,
                                        Value = 0,
                                        FieldLabel = GetGlobalResource("strValor"),
                                        LabelAlign = LabelAlign.Top,
                                        MinValue = Double.NegativeInfinity,
                                        MaxLength = 100
                                    });
                                    break;
                                case TypeCode.UInt64:
                                    ContenedorCampoValueFormat.Items.Add(new NumberField
                                    {
                                        ID = "numCampoValueFormat",
                                        AllowBlank = false,
                                        Value = 0,
                                        FieldLabel = GetGlobalResource("strValor"),
                                        LabelAlign = LabelAlign.Top,
                                        MinValue = Double.NegativeInfinity,
                                        MaxLength = 100
                                    });
                                    break;
                                case TypeCode.String:
                                    ContenedorCampoValueFormat.Items.Add(new TextField
                                    {
                                        ID = "txtCampoValueFormat",
                                        AllowBlank = false,
                                        Text = "",
                                        FieldLabel = GetGlobalResource("strValor"),
                                        LabelAlign = LabelAlign.Top,
                                        MaxLength = 100
                                    });
                                    break;
                                case TypeCode.DateTime:
                                    ContenedorCampoValueFormat.Items.Add(new DateField
                                    {
                                        ID = "datCampoValueFormat",
                                        AllowBlank = false,
                                        FieldLabel = GetGlobalResource("strValor"),
                                        Format = "dd/MM/Y",
                                        LabelAlign = LabelAlign.Top
                                    });
                                    break;
                                default:
                                    ContenedorCampoValueFormat.Items.Add(new TextField
                                    {
                                        ID = "txtCampoValueFormat",
                                        AllowBlank = false,
                                        Text = "",
                                        MaxLength = 100,
                                        FieldLabel = GetGlobalResource("strValor"),
                                        LabelAlign = LabelAlign.Top
                                    });
                                    break;
                            }
                            if (oPropiedad.AplicaReglas)
                            {
                                switch (this.TipoAtributo)
                                {
                                    #region INVETARIO

                                    case Comun.MODULO_WORKFLOW:
                                    case Comun.PRODUCT_CATALOG_SERVICIOS:
                                    case Comun.MODULOINVENTARIO:
                                        CoreAtributosConfiguracionTiposDatosPropiedadesController cCorePropiedades = new CoreAtributosConfiguracionTiposDatosPropiedadesController();
                                        List<Data.Vw_CoreAtributosConfiguracionTiposDatosPropiedades> listaPropiedades = cCorePropiedades.GetVwPropiedadesFromAtributo(long.Parse(hdAtributoID.Value.ToString()));

                                        #region Propiedades

                                        foreach (var item in listaPropiedades)
                                        {
                                            try
                                            {
                                                propertyInfo = ContenedorCampoValueFormat.Items[0].GetType().GetProperty(item.CodigoTipoDatoPropiedad);
                                                if (propertyInfo != null)
                                                {
                                                    switch (Type.GetTypeCode(propertyInfo.PropertyType))
                                                    {
                                                        case TypeCode.Boolean:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], bool.Parse(item.Valor));
                                                            break;
                                                        case TypeCode.Double:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], Double.Parse(item.Valor));
                                                            break;
                                                        case TypeCode.Int16:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], int.Parse(item.Valor));
                                                            break;
                                                        case TypeCode.Int32:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], int.Parse(item.Valor));
                                                            break;
                                                        case TypeCode.Int64:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], long.Parse(item.Valor));
                                                            break;
                                                        case TypeCode.String:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], item.Valor);
                                                            break;
                                                        case TypeCode.DateTime:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], DateTime.Parse(item.Valor));
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }

                                        propertyInfo = ContenedorCampoValueFormat.Items[0].GetType().GetProperty("AllowBlank");
                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], false);

                                        propertyInfo = ContenedorCampoValueFormat.Items[0].GetType().GetProperty("Disabled");
                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], false);

                                        #endregion

                                        break;

                                    #endregion

                                    #region EMPLAZAMIENTOS

                                    case Comun.EMPLAZAMIENTOS:
                                        EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController cEmplazamietosPropiedades = new EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController();
                                        List<Data.Vw_EmplazamientosAtributosTiposDatosPropiedades> listaPropiedadesEmpl = cEmplazamietosPropiedades.GetPropiedadesFromAtributo(long.Parse(hdAtributoID.Value.ToString()));

                                        #region Propiedades

                                        foreach (var item in listaPropiedadesEmpl)
                                        {
                                            try
                                            {
                                                propertyInfo = ContenedorCampoValueFormat.Items[0].GetType().GetProperty(item.Codigo);
                                                if (propertyInfo != null)
                                                {
                                                    switch (Type.GetTypeCode(propertyInfo.PropertyType))
                                                    {
                                                        case TypeCode.Boolean:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], bool.Parse(item.Valor));
                                                            break;
                                                        case TypeCode.Double:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], Double.Parse(item.Valor));
                                                            break;
                                                        case TypeCode.Int16:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], int.Parse(item.Valor));
                                                            break;
                                                        case TypeCode.Int32:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], int.Parse(item.Valor));
                                                            break;
                                                        case TypeCode.Int64:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], long.Parse(item.Valor));
                                                            break;
                                                        case TypeCode.String:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], item.Valor);
                                                            break;
                                                        case TypeCode.DateTime:
                                                            propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], DateTime.Parse(item.Valor));
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }

                                        propertyInfo = ContenedorCampoValueFormat.Items[0].GetType().GetProperty("AllowBlank");
                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], false);

                                        propertyInfo = ContenedorCampoValueFormat.Items[0].GetType().GetProperty("Disabled");
                                        propertyInfo.SetValue(ContenedorCampoValueFormat.Items[0], false);

                                        #endregion

                                        break;

                                    #endregion

                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
                if (Update)
                {
                    ContenedorCampoValueFormat.UpdateContent();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        #endregion

        #region SETTINGS

        [DirectMethod()]
        public DirectResponse GuardarSetting(string JsonColumnas)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                long AtributoID = long.Parse(hdAtributoID.Value.ToString());
                switch (this.TipoAtributo)
                {
                    case Comun.MODULO_WORKFLOW:
                    case Comun.PRODUCT_CATALOG_SERVICIOS:
                    case Comun.MODULOINVENTARIO:
                        CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
                        CoreAtributosConfiguracionListasColumnasAdicionalesController cColumnas = new CoreAtributosConfiguracionListasColumnasAdicionalesController();
                        Data.CoreAtributosConfiguracionListasColumnasAdicionales oColumna = new Data.CoreAtributosConfiguracionListasColumnasAdicionales();
                        Data.CoreAtributosConfiguraciones oAtributo = cAtributos.GetItem(long.Parse(hdAtributoID.Value.ToString()));
                        switch (long.Parse(cmbTipoLista.SelectedItem.Value))
                        {
                            case 1:
                                if (txtaSetting.RawValue.ToString().Length <= 500)
                                {
                                    if (cColumnas.EliminarColumnasAtributos(AtributoID))
                                    {
                                        oAtributo.ValoresPosibles = txtaSetting.RawValue.ToString().Replace(",", ";");
                                        oAtributo.ColumnaModeloDatoID = null;
                                        oAtributo.TablasModeloDatos = null;
                                    }
                                }
                                else
                                {
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                    return direct;
                                }
                                break;
                            case 2:
                                oAtributo.ValoresPosibles = "";
                                if (cColumnas.EliminarColumnasAtributos(AtributoID))
                                {
                                    JObject json = JObject.Parse(JsonColumnas);
                                    int cont = 0;
                                    foreach (var ojsonCol in json)
                                    {
                                        JObject oColjs = JObject.Parse(ojsonCol.Value.ToString());
                                        cColumnas.AddItem(new Data.CoreAtributosConfiguracionListasColumnasAdicionales
                                        {
                                            ColumnaModeloDatoID = long.Parse(oColjs.GetValue("ColumnaID").ToString()),
                                            CoreAtributoConfiguracionID = AtributoID,
                                            JsonRutaColumna = oColjs.GetValue("JsonRuta").ToString(),
                                            Orden = cont
                                        });
                                        cont++;
                                        oAtributo.ColumnaModeloDatoID = long.Parse(oColjs.GetValue("ColumnaID").ToString());
                                    }
                                    oAtributo.TablaModeloDatoID = long.Parse(cmbTable.Value.ToString());
                                }
                                break;
                            default:
                                break;
                        }
                        cAtributos.UpdateItem(oAtributo);
                        break;
                    case Comun.EMPLAZAMIENTOS:
                        EmplazamientosAtributosConfiguracionController cEmplazamientosAtributos = new EmplazamientosAtributosConfiguracionController();
                        Data.EmplazamientosAtributosConfiguracion oEmplazamientoAtributo = cEmplazamientosAtributos.GetItem(long.Parse(hdAtributoID.Value.ToString()));
                        EmplazamientosAtributosConfiguracionListasColumnasAdicionalesController cListasEmplazamientos = new EmplazamientosAtributosConfiguracionListasColumnasAdicionalesController();
                        switch (long.Parse(cmbTipoLista.SelectedItem.Value))
                        {
                            case 1:
                                if (txtaSetting.RawValue.ToString().Length <= 500)
                                {
                                    if (cListasEmplazamientos.EliminarColumnasAtributos(AtributoID))
                                    {
                                        oEmplazamientoAtributo.ValoresPosibles = txtaSetting.RawValue.ToString().Replace(",", ";");
                                        oEmplazamientoAtributo.ColumnaModeloDatoID = null;
                                        oEmplazamientoAtributo.TablasModeloDatos = null;
                                    }
                                }
                                else
                                {
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                    return direct;
                                }
                                break;
                            case 2:
                                oEmplazamientoAtributo.ValoresPosibles = "";
                                if (cListasEmplazamientos.EliminarColumnasAtributos(AtributoID))
                                {
                                    JObject json = JObject.Parse(JsonColumnas);
                                    int cont = 0;
                                    foreach (var ojsonCol in json)
                                    {
                                        JObject oColjs = JObject.Parse(ojsonCol.Value.ToString());
                                        cListasEmplazamientos.AddItem(new Data.EmplazamientosAtributosConfiguracionListasColumnasAdicionales
                                        {
                                            ColumnaModeloDatoID = long.Parse(oColjs.GetValue("ColumnaID").ToString()),
                                            EmplazamientosAtributoConfiguracionID = AtributoID,
                                            JsonRutaColumna = oColjs.GetValue("JsonRuta").ToString(),
                                            Orden = cont
                                        });
                                        cont++;
                                        oEmplazamientoAtributo.ColumnaModeloDatoID = long.Parse(oColjs.GetValue("ColumnaID").ToString());
                                    }
                                    oEmplazamientoAtributo.TablaModeloDatoID = long.Parse(cmbTable.Value.ToString());
                                }
                                break;
                            default:
                                break;
                        }
                        cEmplazamientosAtributos.UpdateItem(oEmplazamientoAtributo);
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

            direct.Success = true;
            direct.Result = "";
            direct.Result = "CAT" + hdCategoriaAtributoID.Value + "_ATR" + hdAtributoID.Value;

            return direct;
        }

        protected class ColumnasVinculadas
        {
            public long ID { get; set; }
            public string Name { get; set; }
            public long AtributoID { get; set; }
            public long ColumnaModeloDatoID { get; set; }
            public string JsonRuta { get; set; }
            public int Orden { get; set; }
        }

        [DirectMethod()]
        public DirectResponse MostrarSetting()
        {
            DirectResponse direct = new DirectResponse();
            TablasModeloDatosController cTablas = new TablasModeloDatosController();
            Data.TablasModeloDatos oTabla;

            try
            {
                List<ColumnasVinculadas> listaColumnas = new List<ColumnasVinculadas>();
                switch (this.TipoAtributo)
                {
                    case Comun.MODULO_WORKFLOW:
                    case Comun.PRODUCT_CATALOG_SERVICIOS:
                    case Comun.MODULOINVENTARIO:
                        CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
                        CoreAtributosConfiguracionListasColumnasAdicionalesController cColumnasAtr = new CoreAtributosConfiguracionListasColumnasAdicionalesController();
                        Data.CoreAtributosConfiguraciones oAtributo = cAtributos.GetItem(long.Parse(hdAtributoID.Value.ToString()));
                        txtaSetting.SetText("");
                        cmbTable.ClearValue();
                        cmbValue.ClearValue();
                        cmbToolTip.ClearValue();
                        cmbTable.Triggers[0].Hidden = true;
                        cmbValue.Triggers[0].Hidden = true;

                        if (oAtributo.TablaModeloDatoID != null)
                        {
                            cmbTipoLista.Select(1);
                            containerTable.Show();
                            containerValue.Show();
                            containerAux.Show();
                            oTabla = cTablas.GetItem((long)oAtributo.TablaModeloDatoID);
                            if (oTabla != null)
                            {
                                hdDatabase.SetValue(oTabla.TablaModeloDatosID);
                                hdTablaActual.SetValue(oTabla.TablaModeloDatosID);
                                //hdValue.SetValue(oColumnas.ColumnaModeloDatosID);
                                //string valorTooltit = "";
                                foreach (var item in cColumnasAtr.GetColumnasAtributos(long.Parse(hdAtributoID.Value.ToString())))
                                {
                                    listaColumnas.Add(new ColumnasVinculadas
                                    {
                                        ID = item.ColumnaModeloDatoID,
                                        Name = GetGlobalResource(item.ColumnasModeloDatos.ClaveRecurso),
                                        AtributoID = oAtributo.CoreAtributoConfiguracionID,
                                        ColumnaModeloDatoID = item.ColumnaModeloDatoID,
                                        JsonRuta = item.JsonRutaColumna,
                                        Orden = (int)item.Orden
                                    });
                                }
                                cmbTable.GetStore().Reload();
                                direct.Result = JsonConvert.SerializeObject(listaColumnas);
                            }
                        }
                        else if (oAtributo.ValoresPosibles != "" && oAtributo.ValoresPosibles != null)
                        {
                            containerTxtaSetting.Show();
                            cmbTipoLista.Select(0);
                            direct.Result = oAtributo.ValoresPosibles.Split(';');
                        }
                        break;
                    case Comun.EMPLAZAMIENTOS:
                        EmplazamientosAtributosConfiguracionController cEmplazamientosAtributos = new EmplazamientosAtributosConfiguracionController();
                        Data.EmplazamientosAtributosConfiguracion oEmplazamientoAtributo = cEmplazamientosAtributos.GetItem(long.Parse(hdAtributoID.Value.ToString()));
                        EmplazamientosAtributosConfiguracionListasColumnasAdicionalesController cEmplazamientosColumnas = new EmplazamientosAtributosConfiguracionListasColumnasAdicionalesController();
                        txtaSetting.SetText("");
                        cmbTable.ClearValue();
                        cmbValue.ClearValue();
                        cmbTable.Triggers[0].Hidden = true;
                        cmbValue.Triggers[0].Hidden = true;

                        if (oEmplazamientoAtributo.TablaModeloDatoID != null)
                        {
                            cmbTipoLista.Select(1);
                            containerTable.Show();
                            containerValue.Show();
                            containerAux.Show();
                            oTabla = cTablas.GetItem((long)oEmplazamientoAtributo.TablaModeloDatoID);
                            if (oTabla != null)
                            {
                                hdDatabase.SetValue(oTabla.TablaModeloDatosID);
                                hdTablaActual.SetValue(oTabla.TablaModeloDatosID);
                                //hdFunciones.SetValue(oEmplazamientoAtributo.FuncionControlador);
                                //hdTooltip.SetValue(oEmplazamientoAtributo.TablaValor);
                                foreach (var item in cEmplazamientosColumnas.GetColumnasFromAtributo(long.Parse(hdAtributoID.Value.ToString())))
                                {
                                    listaColumnas.Add(new ColumnasVinculadas
                                    {
                                        ID = item.ColumnaModeloDatoID,
                                        Name = GetGlobalResource(item.ColumnasModeloDatos.ClaveRecurso),
                                        AtributoID = oEmplazamientoAtributo.EmplazamientoAtributoConfiguracionID,
                                        ColumnaModeloDatoID = item.ColumnaModeloDatoID,
                                        JsonRuta = item.JsonRutaColumna,
                                        Orden = (int)item.Orden
                                    });
                                }
                                cmbTable.GetStore().Reload();
                                direct.Result = JsonConvert.SerializeObject(listaColumnas);
                            }
                        }
                        else if (oEmplazamientoAtributo.ValoresPosibles != "" && oEmplazamientoAtributo.ValoresPosibles != null)
                        {
                            containerTxtaSetting.Show();
                            cmbTipoLista.Select(0);
                            direct.Result = oEmplazamientoAtributo.ValoresPosibles.Split(';');
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

            direct.Success = true;
            //direct.Result = "";

            return direct;
        }

        #endregion

        #region RESTRICCIONES ROLES

        [DirectMethod()]
        public DirectResponse GuardarRestriction(long lRestrinccion)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                switch (this.TipoAtributo)
                {
                    case Comun.MODULO_WORKFLOW:
                    case Comun.PRODUCT_CATALOG_SERVICIOS:
                    case Comun.MODULOINVENTARIO:
                        CoreAtributosConfiguracionRolesRestringidosController cRolesRestringidos = new CoreAtributosConfiguracionRolesRestringidosController();
                        Data.CoreAtributosConfiguracionRolesRestringidos oRolesRestringidos;
                        if (cmbRoles.SelectedItems != null && cmbRoles.SelectedItems.Count > 0)
                        {
                            foreach (var item in cmbRoles.SelectedItems)
                            {
                                oRolesRestringidos = new Data.CoreAtributosConfiguracionRolesRestringidos
                                {
                                    CoreAtributoConfiguracionID = long.Parse(hdAtributoID.Value.ToString()),
                                    RolID = long.Parse(item.Value),
                                };
                                switch (lRestrinccion)
                                {
                                    case (long)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                        oRolesRestringidos.Restriccion = (int)Comun.RestriccionesAtributoDefecto.ACTIVE;
                                        break;
                                    case (long)Comun.RestriccionesAtributoDefecto.DISABLED:
                                        oRolesRestringidos.Restriccion = (int)Comun.RestriccionesAtributoDefecto.DISABLED;
                                        break;
                                    case (long)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                        oRolesRestringidos.Restriccion = (int)Comun.RestriccionesAtributoDefecto.HIDDEN;
                                        break;
                                }
                                if (cRolesRestringidos.AddItem(oRolesRestringidos) != null)
                                {
                                    log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                }
                            }
                        }
                        else if (!cRolesRestringidos.ExisteDefaultAtributo(long.Parse(hdAtributoID.Value.ToString())))
                        {
                            oRolesRestringidos = new Data.CoreAtributosConfiguracionRolesRestringidos
                            {
                                CoreAtributoConfiguracionID = long.Parse(hdAtributoID.Value.ToString()),
                                RolID = null
                            };
                            switch (lRestrinccion)
                            {
                                case (long)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                    oRolesRestringidos.Restriccion = (int)Comun.RestriccionesAtributoDefecto.ACTIVE;
                                    break;
                                case (long)Comun.RestriccionesAtributoDefecto.DISABLED:
                                    oRolesRestringidos.Restriccion = (int)Comun.RestriccionesAtributoDefecto.DISABLED;
                                    break;
                                case (long)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                    oRolesRestringidos.Restriccion = (int)Comun.RestriccionesAtributoDefecto.HIDDEN;
                                    break;
                            }
                            if (cRolesRestringidos.AddItem(oRolesRestringidos) != null)
                            {
                                log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            }
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource("strYaExisteRestriccion");
                            return direct;
                        }
                        break;
                    case Comun.EMPLAZAMIENTOS:
                        EmplazamientosAtributosConfiguracionRolesRestringidosController cEmplazamientosRolesRestringidos = new EmplazamientosAtributosConfiguracionRolesRestringidosController();
                        Data.EmplazamientosAtributosRolesRestringidos oEmplazamientosRolesRestringidos;
                        switch (lRestrinccion)
                        {
                            case (long)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                if (cmbRoles.SelectedItems != null && cmbRoles.SelectedItems.Count > 0)
                                {
                                    foreach (var item in cmbRoles.SelectedItems)
                                    {
                                        oEmplazamientosRolesRestringidos = new Data.EmplazamientosAtributosRolesRestringidos
                                        {
                                            EmplazamientoAtributoConfiguracionID = long.Parse(hdAtributoID.Value.ToString()),
                                            RolID = long.Parse(item.Value),
                                            Restriccion = (long)Comun.RestriccionesAtributoDefecto.ACTIVE
                                        };
                                        if (cEmplazamientosRolesRestringidos.AddItem(oEmplazamientosRolesRestringidos) != null)
                                        {
                                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                        }
                                    }
                                }
                                else if (!cEmplazamientosRolesRestringidos.ExisteDefaultAtributo(long.Parse(hdAtributoID.Value.ToString())))
                                {
                                    oEmplazamientosRolesRestringidos = new Data.EmplazamientosAtributosRolesRestringidos
                                    {
                                        EmplazamientoAtributoConfiguracionID = long.Parse(hdAtributoID.Value.ToString()),
                                        RolID = null,
                                        Restriccion = (long)Comun.RestriccionesAtributoDefecto.ACTIVE
                                    };
                                    if (cEmplazamientosRolesRestringidos.AddItem(oEmplazamientosRolesRestringidos) != null)
                                    {
                                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                    }
                                }
                                else
                                {
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource("strYaExisteRestriccion");
                                    return direct;
                                }

                                break;
                            case (long)Comun.RestriccionesAtributoDefecto.DISABLED:

                                if (cmbRoles.SelectedItems != null && cmbRoles.SelectedItems.Count > 0)
                                {
                                    foreach (var item in cmbRoles.SelectedItems)
                                    {
                                        oEmplazamientosRolesRestringidos = new Data.EmplazamientosAtributosRolesRestringidos
                                        {
                                            EmplazamientoAtributoConfiguracionID = long.Parse(hdAtributoID.Value.ToString()),
                                            RolID = long.Parse(item.Value),
                                            Restriccion = (long)Comun.RestriccionesAtributoDefecto.DISABLED
                                        };
                                        if (cEmplazamientosRolesRestringidos.AddItem(oEmplazamientosRolesRestringidos) != null)
                                        {
                                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                        }
                                    }
                                }
                                else if (!cEmplazamientosRolesRestringidos.ExisteDefaultAtributo(long.Parse(hdAtributoID.Value.ToString())))
                                {
                                    oEmplazamientosRolesRestringidos = new Data.EmplazamientosAtributosRolesRestringidos
                                    {
                                        EmplazamientoAtributoConfiguracionID = long.Parse(hdAtributoID.Value.ToString()),
                                        RolID = null,
                                        Restriccion = (long)Comun.RestriccionesAtributoDefecto.DISABLED
                                    };
                                    if (cEmplazamientosRolesRestringidos.AddItem(oEmplazamientosRolesRestringidos) != null)
                                    {
                                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                    }
                                }
                                else
                                {
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource("strYaExisteRestriccion");
                                    return direct;
                                }

                                break;
                            case (long)Comun.RestriccionesAtributoDefecto.HIDDEN:

                                if (cmbRoles.SelectedItems != null && cmbRoles.SelectedItems.Count > 0)
                                {
                                    foreach (var item in cmbRoles.SelectedItems)
                                    {
                                        oEmplazamientosRolesRestringidos = new Data.EmplazamientosAtributosRolesRestringidos
                                        {
                                            EmplazamientoAtributoConfiguracionID = long.Parse(hdAtributoID.Value.ToString()),
                                            RolID = long.Parse(item.Value),
                                            Restriccion = (long)Comun.RestriccionesAtributoDefecto.HIDDEN
                                        };
                                        if (cEmplazamientosRolesRestringidos.AddItem(oEmplazamientosRolesRestringidos) != null)
                                        {
                                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                        }
                                    }
                                }
                                else if (!cEmplazamientosRolesRestringidos.ExisteDefaultAtributo(long.Parse(hdAtributoID.Value.ToString())))
                                {
                                    oEmplazamientosRolesRestringidos = new Data.EmplazamientosAtributosRolesRestringidos
                                    {
                                        EmplazamientoAtributoConfiguracionID = long.Parse(hdAtributoID.Value.ToString()),
                                        RolID = null,
                                        Restriccion = (long)Comun.RestriccionesAtributoDefecto.HIDDEN
                                    };
                                    if (cEmplazamientosRolesRestringidos.AddItem(oEmplazamientosRolesRestringidos) != null)
                                    {
                                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                    }
                                }
                                else
                                {
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource("strYaExisteRestriccion");
                                    return direct;
                                }

                                break;
                            default:
                                break;
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

            direct.Success = true;
            direct.Result = "CAT" + hdCategoriaAtributoID.Value + "_ATR" + hdAtributoID.Value;

            return direct;
        }

        [DirectMethod()]
        public DirectResponse EliminarRestriction(long idRolesRestringidos)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                switch (this.TipoAtributo)
                {
                    case Comun.MODULO_WORKFLOW:
                    case Comun.PRODUCT_CATALOG_SERVICIOS:
                    case Comun.MODULOINVENTARIO:
                        CoreAtributosConfiguracionRolesRestringidosController cRoles = new CoreAtributosConfiguracionRolesRestringidosController();
                        if (cRoles.DeleteItem(idRolesRestringidos))
                        {
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            direct.Success = true;
                            direct.Result = "";
                        }
                        break;
                    case Comun.EMPLAZAMIENTOS:
                        EmplazamientosAtributosConfiguracionRolesRestringidosController cEmplazamientoRoles = new EmplazamientosAtributosConfiguracionRolesRestringidosController();
                        if (cEmplazamientoRoles.DeleteItem(idRolesRestringidos))
                        {
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            direct.Success = true;
                            direct.Result = "";
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

            direct.Success = true;
            direct.Result = "CAT" + hdCategoriaAtributoID.Value + "_ATR" + hdAtributoID.Value;

            return direct;
        }

        #endregion

        #endregion

    }

}