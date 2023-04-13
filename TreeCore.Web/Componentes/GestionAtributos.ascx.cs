using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using TreeCore.Page;
using TreeCore.Data;
using System.Linq;
using Newtonsoft.Json;

namespace TreeCore.Componentes
{
    public partial class GestionAtributos : BaseUserControl
    {
        protected long _Orden;
        protected long _Modulo;
        protected long _CategoriaAtributoID;
        protected long _AtributoID;
        protected bool _EsPlantilla = false;
        protected bool _EsSubcategoria = false;
        protected ComponentBase ControlAtributo;
        private objControl oObjControl;
        public static List<Object> listaTablas = null;
        public ILog log = LogManager.GetLogger("");

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
            set { _CategoriaAtributoID = value; }
        }

        public long AtributoID
        {
            get { return _AtributoID; }
            set { _AtributoID = value; }
        }

        public bool EsPlantilla
        {
            get { return _EsPlantilla; }
            set { _EsPlantilla = value; }
        }
        public bool EsSubcategoria
        {
            get { return _EsSubcategoria; }
            set { _EsSubcategoria = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PintarControl(false);
        }

        #region STORES

        #endregion

        #region DIRECT METHODS

        private class objControl
        {
            public objControl()
            {
                lProp = new List<objProp>();
                listaItems = new List<itemLista>();
            }
            public List<itemLista> listaItems;
            public string Nombre;
            public string TipoDato;
            public string ID;
            public List<objProp> lProp;
        }
        private class objProp
        {
            public string Propiedad;
            public object Valor;
        }
        private class itemLista
        {
            public string Display;
            public string Valor;
        }

        private void GenerarControl()
        {
            TiposDatosController cTipoDatos = new TiposDatosController();
            bool HiddenDef = true;
            bool DisabledDef = true;
            Data.TiposDatos oTipoDato;
            try
            {
                if (oObjControl == null)
                {
                    oObjControl = new objControl();
                }
                switch (this.Modulo)
                {
                    #region INVENTARIO

                    case (long)Comun.Modulos.INVENTARIO:

                        CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
                        CoreAtributosConfiguracionTiposDatosPropiedadesController cPropiedades = new CoreAtributosConfiguracionTiposDatosPropiedadesController();
                        CoreAtributosConfiguracionRolesRestringidosController cRestriccionRoles = new CoreAtributosConfiguracionRolesRestringidosController();
                        List<Data.Vw_CoreAtributosConfiguracionTiposDatosPropiedades> listaPropiedades;
                        Data.CoreAtributosConfiguraciones oDato = cAtributos.GetItem(this.AtributoID);
                        oTipoDato = cTipoDatos.GetItem(oDato.TipoDatoID);
                        List<Data.Vw_CoreAtributosConfiguracionRolesRestringidos> listaRestriccionRoles = cRestriccionRoles.GetVwRolesFromAtributoNoDefecto(this.AtributoID);
                        listaPropiedades = cPropiedades.GetVwPropiedadesFromAtributo(this.AtributoID);

                        if (ControlAtributo == null && contenedorControl != null && contenedorControl.Items.Count == 0)
                        {
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
                                        Format = "dd/MM/Y",
                                        AriaFormat = "",
                                        InvalidText = "<%$ Resources:Comun, strFechaFormatoIncorrecto %>"
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
                                        listaItems = cAtributos.GetItemsComboBox((long)oDato.CoreAtributoConfiguracionID);
                                        if (listaItems != null)
                                        {
                                            ((ComboBox)ControlAtributo).Items.AddRange(listaItems);
                                        }
                                        foreach (var item in listaItems)
                                        {
                                            oObjControl.listaItems.Add(new itemLista
                                            {
                                                Valor = item.Value,
                                                Display = item.Text
                                            });
                                        }
                                    }
                                    else if (oDato.ValoresPosibles != null && oDato.ValoresPosibles != "")
                                    {
                                        List<Ext.Net.ListItem> listaItems = new List<ListItem>();
                                        foreach (var item in (from c in oDato.ValoresPosibles.Split(';') orderby c select c))
                                        {
                                            listaItems.Add(new ListItem
                                            {
                                                Value = item,
                                                Text = item
                                            });
                                        }
                                        ((ComboBox)ControlAtributo).Items.AddRange(listaItems);
                                        foreach (var item in listaItems)
                                        {
                                            oObjControl.listaItems.Add(new itemLista
                                            {
                                                Valor = item.Value,
                                                Display = item.Text
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
                                        listaItems = cAtributos.GetItemsComboBox((long)oDato.CoreAtributoConfiguracionID);
                                        if (listaItems != null)
                                        {
                                            ((MultiCombo)ControlAtributo).Items.AddRange(listaItems);
                                        }
                                        foreach (var item in listaItems)
                                        {
                                            oObjControl.listaItems.Add(new itemLista
                                            {
                                                Valor = item.Value,
                                                Display = item.Text
                                            });
                                        }
                                    }
                                    else if (oDato.ValoresPosibles != null && oDato.ValoresPosibles != "")
                                    {
                                        List<Ext.Net.ListItem> listaItems = new List<ListItem>();
                                        foreach (var item in (from c in oDato.ValoresPosibles.Split(';') orderby c select c))
                                        {
                                            listaItems.Add(new ListItem
                                            {
                                                Value = item,
                                                Text = item
                                            });
                                        }
                                        ((MultiCombo)ControlAtributo).Items.AddRange(listaItems);
                                        foreach (var item in listaItems)
                                        {
                                            oObjControl.listaItems.Add(new itemLista
                                            {
                                                Valor = item.Value,
                                                Display = item.Text
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
                            ControlAtributo.ID = "Control" + oDato.CoreAtributoConfiguracionID;
                            ControlAtributo.Cls = "txtContainerCategorias";

                            oObjControl.Nombre = this.Nombre;
                            oObjControl.ID = ControlAtributo.ID;
                            oObjControl.TipoDato = oTipoDato.Codigo;

                            #region Propiedades

                            foreach (var item in listaPropiedades)
                            {
                                try
                                {
                                    oObjControl.lProp.Add(new objProp
                                    {
                                        Propiedad = item.CodigoTipoDatoPropiedad,
                                        Valor = item.Valor,
                                    });
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

                            if (X.GetCmp("hdVistaPlantilla") != null && ((Hidden)X.GetCmp("hdVistaPlantilla")).Value != null && ((Hidden)X.GetCmp("hdVistaPlantilla")).Value.ToString() != "")
                            {
                                var propertyInfo = ControlAtributo.GetType().GetProperty("AllowBlank");
                                if (propertyInfo != null)
                                {
                                    propertyInfo.SetValue(ControlAtributo, true);
                                    oObjControl.lProp.Add(new objProp
                                    {
                                        Propiedad = "AllowBlank",
                                        Valor = true,
                                    });

                                }
                            }

                            HiddenDef = true;
                            DisabledDef = true;

                            RolesController cRoles = new RolesController();
                            List<Data.Roles> listaRoles = cRoles.GetRolesFromUsuario(((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                            if (listaRestriccionRoles != null)
                            {
                                listaRestriccionRoles = listaRestriccionRoles.Where(res => listaRoles.Select(rol => rol.RolID).ToList().Contains((long)res.RolID)).ToList();
                                if (listaRestriccionRoles.Count > 0)
                                {
                                    foreach (var oRestriccionRol in listaRestriccionRoles)
                                    {
                                        if (oRestriccionRol.RolID != null)
                                        {
                                            switch (oRestriccionRol.Restriccion)
                                            {
                                                case (int)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                                    mainConstainer.Hidden = true;
                                                    oObjControl.lProp.Add(new objProp
                                                    {
                                                        Propiedad = "Hidden",
                                                        Valor = true,
                                                    });
                                                    HiddenDef = false;
                                                    break;
                                                case (int)Comun.RestriccionesAtributoDefecto.DISABLED:
                                                    ControlAtributo.Disabled = true;
                                                    oObjControl.lProp.Add(new objProp
                                                    {
                                                        Propiedad = "Disabled",
                                                        Valor = true,
                                                    });
                                                    DisabledDef = false;
                                                    break;
                                                case (int)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                                    if (HiddenDef)
                                                    {
                                                        mainConstainer.Hidden = false;
                                                        oObjControl.lProp.Add(new objProp
                                                        {
                                                            Propiedad = "Hidden",
                                                            Valor = false,
                                                        });
                                                    }
                                                    if (DisabledDef)
                                                    {
                                                        ControlAtributo.Disabled = false;
                                                        oObjControl.lProp.Add(new objProp
                                                        {
                                                            Propiedad = "Disabled",
                                                            Valor = false,
                                                        });
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var cInvResDefe = cRestriccionRoles.GetDefault(this.AtributoID);
                                    if (cInvResDefe != null)
                                    {
                                        switch (cInvResDefe.Restriccion)
                                        {
                                            case (int)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                                mainConstainer.Hidden = true;
                                                oObjControl.lProp.Add(new objProp
                                                {
                                                    Propiedad = "Hidden",
                                                    Valor = true,
                                                });
                                                break;
                                            case (int)Comun.RestriccionesAtributoDefecto.DISABLED:
                                                ControlAtributo.Disabled = true;
                                                oObjControl.lProp.Add(new objProp
                                                {
                                                    Propiedad = "Disabled",
                                                    Valor = true,
                                                });
                                                break;
                                            case (int)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                            default:
                                                break;
                                        }

                                    }
                                }
                            }
                            if (EsPlantilla)
                            {
                                ControlAtributo.Disabled = false;
                            }
                            if (EsSubcategoria && !EsPlantilla)
                            {
                                ControlAtributo.Disabled = true;
                            }
                            if (mainConstainer.Hidden)
                            {
                                var propertyInfo = ControlAtributo.GetType().GetProperty("AllowBlank");
                                if (propertyInfo != null)
                                {
                                    propertyInfo.SetValue(ControlAtributo, true);
                                    oObjControl.lProp.Add(new objProp
                                    {
                                        Propiedad = "AllowBlank",
                                        Valor = true,
                                    });
                                }
                            }

                            #endregion

                        }

                        break;

                    #endregion

                    #region EMPLAZAMIENTOS

                    case (long)Comun.Modulos.GLOBAL:

                        EmplazamientosAtributosConfiguracionController cEmplazamientosAtributos = new EmplazamientosAtributosConfiguracionController();
                        EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController cEmplazamietosPropiedades = new EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController();
                        List<Data.Vw_EmplazamientosAtributosTiposDatosPropiedades> listaPropiedadesEMP;
                        Data.EmplazamientosAtributosConfiguracion oEmplazamientoAtributo = cEmplazamientosAtributos.GetItem(this.AtributoID);
                        oTipoDato = cTipoDatos.GetItem(oEmplazamientoAtributo.TipoDatoID);
                        EmplazamientosAtributosConfiguracionRolesRestringidosController cEmplRestriccionRoles = new EmplazamientosAtributosConfiguracionRolesRestringidosController();
                        List<Data.Vw_EmplazamientosAtributosRolesRestringidos> listaEmplRestriccionRoles = cEmplRestriccionRoles.GetRolesRestringidosAtributo(oEmplazamientoAtributo.EmplazamientoAtributoConfiguracionID);
                        listaPropiedadesEMP = cEmplazamietosPropiedades.GetPropiedadesFromAtributo(this.AtributoID);
                        if (ControlAtributo == null && contenedorControl.Items.Count == 0)
                        {
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
                                        Format = "dd/MM/Y",
                                        InvalidText = "This is not a valid date - it must be in the format 'dd/mm/yyyy'"
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
                                        foreach (var item in listaItems)
                                        {
                                            oObjControl.listaItems.Add(new itemLista
                                            {
                                                Valor = item.Value,
                                                Display = item.Text
                                            });
                                        }
                                    }
                                    else if (oEmplazamientoAtributo.ValoresPosibles != null && oEmplazamientoAtributo.ValoresPosibles != "")
                                    {
                                        List<Ext.Net.ListItem> listaItems = new List<ListItem>();
                                        foreach (var item in (from c in oEmplazamientoAtributo.ValoresPosibles.Split(';') orderby c select c))
                                        {
                                            listaItems.Add(new ListItem
                                            {
                                                Value = item,
                                                Text = item
                                            });
                                        }
                                        ((ComboBox)ControlAtributo).Items.AddRange(listaItems);
                                        foreach (var item in listaItems)
                                        {
                                            oObjControl.listaItems.Add(new itemLista
                                            {
                                                Valor = item.Value,
                                                Display = item.Text
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
                                        foreach (var item in listaItems)
                                        {
                                            oObjControl.listaItems.Add(new itemLista
                                            {
                                                Valor = item.Value,
                                                Display = item.Text
                                            });
                                        }
                                    }
                                    else if (oEmplazamientoAtributo.ValoresPosibles != null && oEmplazamientoAtributo.ValoresPosibles != "")
                                    {
                                        List<Ext.Net.ListItem> listaItems = new List<ListItem>();
                                        foreach (var item in (from c in oEmplazamientoAtributo.ValoresPosibles.Split(';') orderby c select c))
                                        {
                                            listaItems.Add(new ListItem
                                            {
                                                Value = item,
                                                Text = item
                                            });
                                        }
                                        ((MultiCombo)ControlAtributo).Items.AddRange(listaItems);
                                        foreach (var item in listaItems)
                                        {
                                            oObjControl.listaItems.Add(new itemLista
                                            {
                                                Valor = item.Value,
                                                Display = item.Text
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
                            ControlAtributo.ID = "Control" + oEmplazamientoAtributo.EmplazamientoAtributoConfiguracionID;
                            ControlAtributo.Cls = "txtContainerCategorias";

                            oObjControl.Nombre = this.Nombre;
                            oObjControl.ID = ControlAtributo.ID;
                            oObjControl.TipoDato = oTipoDato.Codigo;

                            #region Propiedades

                            foreach (var item in listaPropiedadesEMP)
                            {
                                try
                                {
                                    oObjControl.lProp.Add(new objProp
                                    {
                                        Propiedad = item.Codigo,
                                        Valor = item.Valor,
                                    });
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


                            HiddenDef = true;
                            DisabledDef = true;



                            RolesController cRoles = new RolesController();
                            List<Data.Roles> listaRoles = cRoles.GetRolesFromUsuario(((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                            if (listaEmplRestriccionRoles != null)
                            {
                                listaEmplRestriccionRoles = listaEmplRestriccionRoles.Where(res => listaRoles.Select(rol => rol.RolID).ToList().Contains((long)res.RolID)).ToList();
                                if (listaEmplRestriccionRoles.Count > 0)
                                {
                                    foreach (var oRestriccionRol in listaEmplRestriccionRoles)
                                    {
                                        if (oRestriccionRol.RolID != null)
                                        {
                                            switch (oRestriccionRol.Restriccion)
                                            {
                                                case (int)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                                    mainConstainer.Hidden = true;
                                                    oObjControl.lProp.Add(new objProp
                                                    {
                                                        Propiedad = "Hidden",
                                                        Valor = true,
                                                    });
                                                    HiddenDef = false;
                                                    break;
                                                case (int)Comun.RestriccionesAtributoDefecto.DISABLED:
                                                    ControlAtributo.Disabled = true;
                                                    oObjControl.lProp.Add(new objProp
                                                    {
                                                        Propiedad = "Disabled",
                                                        Valor = true,
                                                    });
                                                    DisabledDef = false;
                                                    break;
                                                case (int)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                                    if (HiddenDef)
                                                    {
                                                        mainConstainer.Hidden = false;
                                                        oObjControl.lProp.Add(new objProp
                                                        {
                                                            Propiedad = "Hidden",
                                                            Valor = false,
                                                        });
                                                    }
                                                    if (DisabledDef)
                                                    {
                                                        ControlAtributo.Disabled = false;
                                                        oObjControl.lProp.Add(new objProp
                                                        {
                                                            Propiedad = "Disabled",
                                                            Valor = false,
                                                        });
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    var cEmplResDefe = cEmplRestriccionRoles.GetDefault(this.AtributoID);
                                    if (cEmplResDefe != null)
                                    {
                                        switch (cEmplResDefe.Restriccion)
                                        {
                                            case (long)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                                mainConstainer.Hidden = true;
                                                oObjControl.lProp.Add(new objProp
                                                {
                                                    Propiedad = "Hidden",
                                                    Valor = true,
                                                });
                                                break;
                                            case (long)Comun.RestriccionesAtributoDefecto.DISABLED:
                                                ControlAtributo.Disabled = true;
                                                oObjControl.lProp.Add(new objProp
                                                {
                                                    Propiedad = "Disabled",
                                                    Valor = true,
                                                });
                                                break;
                                            case (long)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                            default:
                                                break;
                                        }

                                    }
                                }
                            }
                            if (EsPlantilla)
                            {
                                ControlAtributo.Disabled = false;
                                oObjControl.lProp.Add(new objProp
                                {
                                    Propiedad = "Disabled",
                                    Valor = false,
                                });
                            }
                            if (EsSubcategoria && !EsPlantilla)
                            {
                                ControlAtributo.Disabled = true;
                                oObjControl.lProp.Add(new objProp
                                {
                                    Propiedad = "Disabled",
                                    Valor = true,
                                });
                            }
                            if (mainConstainer.Hidden)
                            {
                                var propertyInfo = ControlAtributo.GetType().GetProperty("AllowBlank");
                                if (propertyInfo != null)
                                {
                                    propertyInfo.SetValue(ControlAtributo, true);
                                    oObjControl.lProp.Add(new objProp
                                    {
                                        Propiedad = "AllowBlank",
                                        Valor = true,
                                    });
                                }
                            }

                            #endregion

                        }
                        break;

                    #endregion

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        [DirectMethod]
        public void PintarControl(bool Update = false)
        {
            try
            {
                if (contenedorControl != null)
                {
                    if (Update && contenedorControl.Items != null)
                    {
                        contenedorControl.Items.Clear();
                        ControlAtributo = null;
                    }
                    if (X.GetCmp("hdPageLoad") == null)
                    {
                        GenerarControl();
                    }
                    else
                    {
                        Hidden hdPL = (Hidden)X.GetCmp("hdPageLoad");
                        hdPageLoadController hdController = new hdPageLoadController(hdPL);
                        string oValor = hdController.GetValor("Atr" + this.AtributoID);
                        if (oValor == "")
                        {
                            GenerarControl();
                            oValor = JsonConvert.SerializeObject(oObjControl);
                            hdController.SetValor("Atr" + this.AtributoID, oValor);
                        }
                        else
                        {
                            oObjControl = JsonConvert.DeserializeObject<objControl>(oValor);
                            #region Cargar Objeto

                            switch (oObjControl.TipoDato)
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
                                        Format = "dd/MM/Y",
                                        InvalidText = "This is not a valid date - it must be in the format 'dd/mm/yyyy'"
                                    };
                                    break;
                                case Comun.TIPODATO_CODIGO_LISTA:
                                    ControlAtributo = new ComboBox
                                    {
                                        ForceSelection = true,
                                        QueryMode = DataLoadMode.Local
                                    };
                                    foreach (var item in oObjControl.listaItems)
                                    {
                                        ((ComboBox)ControlAtributo).Items.Add(new ListItem
                                        {
                                            Text = item.Display,
                                            Value = item.Valor
                                        });
                                    }

                                    break;
                                case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                                    ControlAtributo = new MultiCombo
                                    {
                                        ForceSelection = true,
                                        QueryMode = DataLoadMode.Local
                                    }; 
                                    foreach (var item in oObjControl.listaItems)
                                    {
                                        ((MultiCombo)ControlAtributo).Items.Add(new ListItem
                                        {
                                            Text = item.Display,
                                            Value = item.Valor
                                        });
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
                            ControlAtributo.ID = oObjControl.ID;
                            ControlAtributo.Cls = "txtContainerCategorias";

                            foreach (var oPro in oObjControl.lProp)
                            {
                                try
                                {
                                    if (oPro.Propiedad == "ToolTips")
                                    {
                                        var propertyInfo = ControlAtributo.GetType().GetProperty(oPro.Propiedad);
                                        ItemsCollection<ToolTip> listaTooltips = (ItemsCollection<ToolTip>)propertyInfo.GetValue(ControlAtributo);
                                        listaTooltips.Add(new ToolTip
                                        {
                                            Html = oPro.Valor.ToString(),
                                            Anchor = "top",
                                            TrackMouse = true
                                        });
                                    }
                                    else if (oPro.Propiedad == "Value")
                                    {
                                        var propertyInfo = ControlAtributo.GetType().GetProperty(oPro.Propiedad);
                                        propertyInfo.SetValue(ControlAtributo, oPro.Valor.ToString());
                                    }
                                    else if (oPro.Propiedad == "Hidden")
                                    {
                                        mainConstainer.Hidden = bool.Parse(oPro.Valor.ToString());
                                    }
                                    else
                                    {
                                        var propertyInfo = ControlAtributo.GetType().GetProperty(oPro.Propiedad);
                                        if (propertyInfo != null)
                                        {
                                            switch (Type.GetTypeCode(propertyInfo.PropertyType))
                                            {
                                                case TypeCode.Boolean:
                                                    propertyInfo.SetValue(ControlAtributo, bool.Parse(oPro.Valor.ToString()));
                                                    break;
                                                case TypeCode.Double:
                                                    propertyInfo.SetValue(ControlAtributo, Double.Parse(oPro.Valor.ToString()));
                                                    break;
                                                case TypeCode.Int16:
                                                    propertyInfo.SetValue(ControlAtributo, int.Parse(oPro.Valor.ToString()));
                                                    break;
                                                case TypeCode.Int32:
                                                    propertyInfo.SetValue(ControlAtributo, int.Parse(oPro.Valor.ToString()));
                                                    break;
                                                case TypeCode.Int64:
                                                    propertyInfo.SetValue(ControlAtributo, long.Parse(oPro.Valor.ToString()));
                                                    break;
                                                case TypeCode.String:
                                                    propertyInfo.SetValue(ControlAtributo, oPro.Valor.ToString());
                                                    break;
                                                case TypeCode.DateTime:
                                                    propertyInfo.SetValue(ControlAtributo, DateTime.Parse(oPro.Valor.ToString()));
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex.Message);
                                }
                            }
                            #endregion
                        }
                    }
                    contenedorControl.Items.Add(ControlAtributo);

                    if (Update)
                    {
                        //contenedorControl.UpdateContent();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }


        #endregion

        #region FUNCTIONS

        public bool GuardarValor(List<Object> listaAtributos, TreeCoreContext oContext)
        {
            string valor = ""; string prueba;
            TiposDatosController cTipoDatos = new TiposDatosController();
            cTipoDatos.SetDataContext(oContext);
            bool valido = true;

            Data.TiposDatos oTipoDato = new Data.TiposDatos();
            try
            {
                switch (this.Modulo)
                {
                    #region INVENTARIO

                    case (long)Comun.Modulos.INVENTARIO:
                        Data.CoreAtributosConfiguraciones oDato;
                        CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
                        cAtributos.SetDataContext(oContext);
                        oDato = cAtributos.GetItem(this.AtributoID);
                        oTipoDato = cTipoDatos.GetItem(oDato.TipoDatoID);
                        break;

                    #endregion

                    #region EMPLAZAMIENTOS

                    case (long)Comun.Modulos.GLOBAL:
                        EmplazamientosAtributosConfiguracionController cEmplazamientoAtributos = new EmplazamientosAtributosConfiguracionController();
                        cEmplazamientoAtributos.SetDataContext(oContext);
                        Data.EmplazamientosAtributosConfiguracion oEmplDato;
                        oEmplDato = cEmplazamientoAtributos.GetItem(this.AtributoID);
                        oTipoDato = cTipoDatos.GetItem(oEmplDato.TipoDatoID);
                        break;

                    #endregion

                    default:
                        break;
                }
                if (ControlAtributo == null)
                {
                    PintarControl(false);
                }

                string valorTextoLista = "";
                switch (oTipoDato.Codigo)
                {
                    case Comun.TIPODATO_CODIGO_TEXTO:
                        if (((TextField)contenedorControl.Items[0]).Value != null)
                        {
                            valor = ((TextField)contenedorControl.Items[0]).Value.ToString();
                        }
                        else
                        {
                            valor = "";
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_NUMERICO:
                        if (((NumberField)contenedorControl.Items[0]).Value != null)
                        {
                            if (((NumberField)contenedorControl.Items[0]).RawText != "")
                            {
                                valor = ((NumberField)contenedorControl.Items[0]).Value.ToString();
                            }
                            else
                            {
                                valor = "";
                            }
                        }
                        else
                        {
                            valor = "";
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_FECHA:
                        if (((DateField)contenedorControl.Items[0]).RawValue != null)
                        {
                            valor = DateTime.ParseExact(((DateField)contenedorControl.Items[0]).RawValue.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString();
                            valorTextoLista = ((DateField)contenedorControl.Items[0]).RawValue.ToString();
                        }
                        else
                        {
                            valor = "";
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_LISTA:
                        if (((ComboBox)contenedorControl.Items[0]).SelectedItem.Value != null)
                        {
                            valor = ((ComboBox)contenedorControl.Items[0]).SelectedItem.Value.ToString();
                            valorTextoLista = ((ComboBox)contenedorControl.Items[0]).SelectedItem.Text.ToString();
                        }
                        else
                        {
                            valor = "";
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                        if (((MultiCombo)contenedorControl.Items[0]).Value != null)
                        {
                            valor = "";
                            foreach (var item in ((MultiCombo)contenedorControl.Items[0]).SelectedItems)
                            {
                                if (valor != "")
                                {
                                    valor += ",";
                                    valorTextoLista += ",";
                                }
                                valor += item.Value.ToString();
                                valorTextoLista += item.Text.ToString();
                            }
                        }
                        else
                        {
                            valor = "";
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_BOOLEAN:
                        valor = ((Checkbox)contenedorControl.Items[0]).Checked.ToString();
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
                        if (((TextArea)contenedorControl.Items[0]).Text != null)
                        {
                            valor = ((TextArea)contenedorControl.Items[0]).Text.ToString();
                        }
                        else
                        {
                            valor = "";
                        }
                        break;
                    default:
                        if (((TextField)contenedorControl.Items[0]).Value != null)
                        {
                            valor = ((TextField)contenedorControl.Items[0]).Value.ToString();
                        }
                        else
                        {
                            valor = "";
                        }
                        break;
                }
                Object Atributo = new
                {
                    AtributoID = this.AtributoID,
                    NombreAtributo = this.Nombre,
                    Valor = valor,
                    TipoDato = oTipoDato.Codigo,
                    TextLista = valorTextoLista
                };

                if (!mainConstainer.Hidden)
                {
                    listaAtributos.Add(Atributo);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                valido = false;
            }
            return valido;
        }

        public void MostarEditar(List<Object> listaValoresAtributos, JsonObject jsDatos)
        {
            var AtributoID = listaValoresAtributos.First().GetType().GetProperty("AtributoID");
            var Valor = listaValoresAtributos.First().GetType().GetProperty("Valor");

            TiposDatosController cTipoDatos = new TiposDatosController();

            Data.TiposDatos oTipoDato = new Data.TiposDatos();
            try
            {
                string strValor;
                try
                {
                    strValor = Valor.GetValue(
                        listaValoresAtributos.FindAll(item =>
                        long.Parse(AtributoID.GetValue(item).ToString()) == this.AtributoID).First()
                        ).ToString();
                }
                catch (Exception)
                {
                    strValor = "";
                }

                switch (this.Modulo)
                {
                    #region INVENTARIO

                    case (long)Comun.Modulos.INVENTARIO:
                        CoreAtributosConfiguracionesController cInventariotributos = new CoreAtributosConfiguracionesController();
                        Data.CoreAtributosConfiguraciones oDato = cInventariotributos.GetItem(this.AtributoID);
                        oTipoDato = cTipoDatos.GetItem(oDato.TipoDatoID);
                        break;

                    #endregion

                    #region EMPLAZAMIENTOS

                    case (long)Comun.Modulos.GLOBAL:
                        EmplazamientosAtributosConfiguracionController cEmplazamientoAtributos = new EmplazamientosAtributosConfiguracionController();
                        Data.EmplazamientosAtributosConfiguracion oEmplDato = cEmplazamientoAtributos.GetItem(this.AtributoID);
                        oTipoDato = cTipoDatos.GetItem(oEmplDato.TipoDatoID);
                        break;

                    #endregion

                    default:
                        break;
                }

                if (strValor.Equals(""))
                {
                    jsDatos.Add(contenedorControl.Items[0].BaseClientID, strValor);
                }
                else
                {
                    switch (oTipoDato.Codigo)
                    {
                        case Comun.TIPODATO_CODIGO_TEXTO:
                            jsDatos.Add(contenedorControl.Items[0].BaseClientID, strValor);
                            break;
                        case Comun.TIPODATO_CODIGO_NUMERICO:
                            jsDatos.Add(contenedorControl.Items[0].BaseClientID, strValor);
                            break;
                        case Comun.TIPODATO_CODIGO_FECHA:
                            jsDatos.Add(contenedorControl.Items[0].BaseClientID, DateTime.Parse(strValor).ToString(Comun.FORMATO_FECHA));
                            break;
                        case Comun.TIPODATO_CODIGO_LISTA:
                            jsDatos.Add(contenedorControl.Items[0].BaseClientID, strValor);
                            break;
                        case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                            jsDatos.Add(contenedorControl.Items[0].BaseClientID, strValor.Split(',').ToList());
                            break;
                        case Comun.TIPODATO_CODIGO_BOOLEAN:
                            jsDatos.Add(contenedorControl.Items[0].BaseClientID, bool.Parse(strValor));
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
                            jsDatos.Add(contenedorControl.Items[0].BaseClientID, strValor);
                            break;
                        default:
                            jsDatos.Add(contenedorControl.Items[0].BaseClientID, strValor);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }


        #endregion
    }
}