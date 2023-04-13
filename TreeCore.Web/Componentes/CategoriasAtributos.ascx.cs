using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using TreeCore.Page;
using System.Reflection;
using System.Linq;
using System.Transactions;
using System.Data.SqlClient;

namespace TreeCore.Componentes
{
    public partial class CategoriasAtributos : BaseUserControl
    {
        protected long _Orden;
        protected long _Modulo;
        protected long _CategoriaAtributoID;
        protected long _CategoriaID = 0;
        protected bool _EsPlantilla = false;
        protected bool _EsSoloLectura = false;
        protected long _CategoriaAtributoAsignacionID;
        protected List<Componentes.Atributos> listaAtributos;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string Nombre
        {
            get
            {
                return lbNombreCategoria.Text;
            }
            set
            {
                lbNombreCategoria.Text = value.ToString();
            }
        }

        public long Orden
        {
            get { return _Orden; }
            set
            {
                _Orden = value;
                containerAttributes.CustomConfig.Add(new ConfigItem("Orden", value));
            }
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

        public long CategoriaID
        {
            get { return _CategoriaID; }
            set { _CategoriaID = value; }
        }

        public long CategoriaAtributoAsignacionID
        {
            get { return _CategoriaAtributoAsignacionID; }
            set
            {
                _CategoriaAtributoAsignacionID = value;
            }
        }

        public bool ActivarBotonEliminarCategoria
        {
            get { return !btnEliminarCategorias.Hidden; }
            set { btnEliminarCategorias.Hidden = !value; }
        }
        public bool EsPlantilla
        {
            get { return _EsPlantilla; }
            set
            {
                _EsPlantilla = value;
                if (value)
                {
                    btnAddAttribute.Hidden = true;
                    lbNombreCategoria.Cls = "btnCategory btnCategoryNoButton";
                }
            }
        }
        public bool EsSoloLectura
        {
            get { return _EsSoloLectura; }
            set
            {
                _EsSoloLectura = value;
                if (value)
                {
                    btnAddAttribute.Hidden = true;
                    btnEliminarCategorias.Hidden = true;
                    btnMoverCategoria.Hidden = true;
                    lbNombreCategoria.Cls = "btnCategory btnCategoryNoButton";
                }
            }
        }

        public string TipoCategoria
        {
            set
            {
                if (value == "Seccion")
                {
                    lbNombreCategoria.IconCls = "ico-seccion-gr";
                }
                else if (value == "Subcategoria")
                {
                    lbNombreCategoria.IconCls = "ico-subcategoria-gr";
                }
                else if (value == "SubcategoriaPlantilla")
                {
                    lbNombreCategoria.IconCls = "ico-subcategoriaplantilla-gr";
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            PintarAtributos(false);
        }


        #region STORES



        #endregion

        #region DIRECT METHOD

        [DirectMethod]
        public string CargarTiposDatos()
        {
            TiposDatosController cDatos = new TiposDatosController();
            List<Ext.Net.MenuItem> items = new List<Ext.Net.MenuItem>();
            long cliID;

            try
            {
                switch (Modulo)
                {
                    case (long)Comun.Modulos.INVENTARIO:
                        CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCat = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
                        cliID = (long)cCat.GetItem(CategoriaAtributoID).InventarioAtributosCategorias.ClienteID;
                        break;
                    case (long)Comun.Modulos.GLOBAL:
                        EmplazamientosAtributosCategoriasController cEmplCat = new EmplazamientosAtributosCategoriasController();
                        cliID = (long)cEmplCat.GetItem(this.CategoriaAtributoID).ClienteID;
                        break;
                    default:
                        cliID = 0;
                        break;
                }
                List<Data.TiposDatos> listaDatos = cDatos.GetActivos(cliID);
                foreach (var item in listaDatos)
                {
                    items.Add(new Ext.Net.MenuItem(Nombre + item.TipoDato.ToString())
                    {
                        ID = Nombre + '_' + item.TipoDatoID.ToString(),
                        Text = item.TipoDato
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return ComponentLoader.ToConfig(items);
        }

        [DirectMethod]
        public void MoverElementoOrden(long Orden)
        {
            try
            {
                switch (Modulo)
                {
                    case (long)Comun.Modulos.INVENTARIO:
                        CoreInventarioCategoriasAtributosCategoriasController cInvCat = new CoreInventarioCategoriasAtributosCategoriasController();
                        Data.CoreInventarioCategoriasAtributosCategorias oInvCat = cInvCat.GetRelacion(CategoriaID, CategoriaAtributoID);
                        oInvCat.Orden = (int)Orden;
                        cInvCat.UpdateItem(oInvCat);
                        break;
                    case (long)Comun.Modulos.GLOBAL:
                        EmplazamientosAtributosConfiguracionController cEmpCat = new EmplazamientosAtributosConfiguracionController();
                        cEmpCat.ActualizarOrdenCategorias(CategoriaAtributoID, Orden);
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


        [DirectMethod]
        public DirectResponse EliminarCategoria()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                direct.Success = true;
                direct.Result = "";
                switch (Modulo)
                {
                    case (long)Comun.Modulos.INVENTARIO:
                        CoreInventarioCategoriasAtributosCategoriasController cCategoriasRe = new CoreInventarioCategoriasAtributosCategoriasController();
                        string sError;

                        if (EsPlantilla)
                        {
                            direct = cCategoriasRe.EliminarRelacionCategoriaAtributosInventarioCategoria(CategoriaID, CategoriaAtributoID, out sError);
                        }
                        else
                        {
                            direct = cCategoriasRe.EliminarCategoriaAtributosInventarioCategoria(CategoriaID, CategoriaAtributoID, out sError);
                        }
                        if (sError != "")
                        {
                            log.Error(sError);
                        }
                        if (direct.Result != null && direct.Result.ToString() != "")
                        {
                            direct.Result = GetGlobalResource(direct.Result.ToString());
                        }
                        break;
                    case (long)Comun.Modulos.GLOBAL:
                        EmplazamientosAtributosConfiguracionController cEmplAtr = new EmplazamientosAtributosConfiguracionController();
                        using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
                        {
                            try
                            {
                                foreach (var Atributo in listaAtributos)
                                {
                                    if (!cEmplAtr.DeleteItem(Atributo.AtributoID))
                                    {
                                        trans.Dispose();
                                        direct.Success = false;
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                                if (X.GetCmp("hdListaCategorias") != null)
                                {
                                    var hd = (Hidden)X.GetCmp("hdListaCategorias");
                                    List<string> lista = hd.Value.ToString().Split(',').ToList();
                                    lista.Remove(CategoriaAtributoID.ToString());
                                    hd.SetValue(String.Join(",", lista.ToArray()));
                                }
                                trans.Complete();
                            }
                            catch (Exception ex)
                            {
                                if (ex is SqlException Sql)
                                {
                                    trans.Dispose();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                                    log.Error(Sql.Message);
                                    return direct;
                                }
                                else
                                {
                                    trans.Dispose();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                    log.Error(ex.Message);
                                    return direct;
                                }
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
            }

            return direct;
        }

        [DirectMethod]
        public DirectResponse AñadirNuevoAtributo(int idTipoDato)
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                TiposDatosController cTipoDato = new TiposDatosController();
                Data.TiposDatos oTipoDato = cTipoDato.GetItem(idTipoDato);
                long cliID;
                int Orden = listaAtributos.Count;
                switch (Modulo)
                {
                    case (long)Comun.Modulos.INVENTARIO:
                        CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cAtr = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
                        CoreInventarioCategoriasAtributosCategoriasController cCatVin = new CoreInventarioCategoriasAtributosCategoriasController();
                        Data.CoreAtributosConfiguraciones atr = cAtr.CrearAtributo(CategoriaAtributoID, oTipoDato.TipoDatoID, GetGlobalResource(Comun.strNuevoAtributo) + this.CategoriaAtributoAsignacionID + Orden.ToString(), GetGlobalResource(Comun.strNuevoAtributo) + this.CategoriaAtributoAsignacionID + Orden.ToString(), "", null, Orden, Modulo);

                        Componentes.Atributos CompAtr = (Componentes.Atributos)this.LoadControl("Atributos.ascx");
                        CompAtr.ID = "ATR" + atr.CoreAtributoConfiguracionID.ToString();
                        CompAtr.Nombre = atr.Codigo;
                        CompAtr.Orden = Orden;
                        CompAtr.Modulo = this.Modulo;
                        CompAtr.TipoAtributo = Comun.MODULOINVENTARIO;
                        CompAtr.CategoriaAtributoID = this.CategoriaAtributoID;
                        CompAtr.CategoriaAtributoAsignacionID = this.CategoriaAtributoAsignacionID;
                        CompAtr.AtributoID = atr.CoreAtributoConfiguracionID;
                        CompAtr.EsPlantilla = EsPlantilla;
                        CompAtr.EsSoloLectura = EsSoloLectura;
                        listaAtributos.Add(CompAtr);

                        break;
                    case (long)Comun.Modulos.GLOBAL:

                        EmplazamientosAtributosCategoriasController cEmplCat = new EmplazamientosAtributosCategoriasController();
                        cliID = (long)cEmplCat.GetItem(this.CategoriaAtributoID).ClienteID;
                        EmplazamientosAtributosConfiguracionController cEmplAtr = new EmplazamientosAtributosConfiguracionController();
                        Data.EmplazamientosAtributosConfiguracion oEmplatr = new Data.EmplazamientosAtributosConfiguracion
                        {
                            EmplazamientoAtributoCategoriaID = CategoriaAtributoID,
                            TipoDatoID = oTipoDato.TipoDatoID,
                            NombreAtributo = GetGlobalResource(Comun.strNuevoAtributo) + this.CategoriaAtributoAsignacionID + listaAtributos.Count.ToString(),
                            CodigoAtributo = GetGlobalResource(Comun.strNuevoAtributo) + this.CategoriaAtributoAsignacionID + listaAtributos.Count.ToString(),
                            ValoresPosibles = "",
                            NombreTabla = "",
                            TablaIndice = "",
                            TablaValor = "",
                            TablaControlador = "",
                            FuncionControlador = "",
                            Activo = true,
                            Orden = listaAtributos.Count,
                            OrdenCategoria = int.Parse(this.Orden.ToString()),
                            ClienteID = cliID
                        };
                        oEmplatr = cEmplAtr.AddItem(oEmplatr);

                        #region RESTRICCION POR DEFECTO

                        EmplazamientosAtributosConfiguracionRolesRestringidosController cEmplRestrinccion = new EmplazamientosAtributosConfiguracionRolesRestringidosController();
                        Data.EmplazamientosAtributosRolesRestringidos cEmplRest = new Data.EmplazamientosAtributosRolesRestringidos
                        {
                            EmplazamientoAtributoConfiguracionID = oEmplatr.EmplazamientoAtributoConfiguracionID,
                            RolID = null,
                            Restriccion = (long)Comun.GetRestriccionDefectoEmplazamientos()
                        };

                        cEmplRestrinccion.AddItem(cEmplRest);

                        #endregion

                        Componentes.Atributos EmplCompAtr = (Componentes.Atributos)this.LoadControl("Atributos.ascx");
                        EmplCompAtr.ID = "ATR" + oEmplatr.EmplazamientoAtributoConfiguracionID.ToString();
                        EmplCompAtr.Nombre = oEmplatr.NombreAtributo;
                        EmplCompAtr.Orden = oEmplatr.Orden;
                        EmplCompAtr.Modulo = this.Modulo;
                        EmplCompAtr.TipoAtributo = Comun.EMPLAZAMIENTOS;
                        EmplCompAtr.CategoriaAtributoID = this.CategoriaAtributoID;
                        EmplCompAtr.CategoriaAtributoAsignacionID = this.CategoriaAtributoAsignacionID;
                        EmplCompAtr.AtributoID = oEmplatr.EmplazamientoAtributoConfiguracionID;
                        EmplCompAtr.EsPlantilla = EsPlantilla;
                        EmplCompAtr.EsSoloLectura = EsSoloLectura;
                        listaAtributos.Add(EmplCompAtr);
                        break;
                    default:
                        break;
                }
                PintarAtributos(true);
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
        public void PintarAtributos(bool Update, bool Ordenar = false)
        {
            CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cInvCategorias = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController cInvAtributos = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController();
            EmplazamientosAtributosConfiguracionController cEmplAtributos = new EmplazamientosAtributosConfiguracionController();
            if (listaAtributos == null)
            {
                Componentes.Atributos oComponente;
                switch (Modulo)
                {
                    case (long)Comun.Modulos.INVENTARIO:

                        try
                        {
                            listaAtributos = new List<Componentes.Atributos>();
                            foreach (var oDato in cInvCategorias.GetListaVwAtributos(this.CategoriaAtributoID))
                            {
                                oComponente = (Componentes.Atributos)this.LoadControl("Atributos.ascx");
                                oComponente.ID = "ATR" + oDato.CoreAtributoConfiguracionID.ToString();
                                oComponente.Nombre = oDato.CodigoCoreAtributoConfg;
                                oComponente.Orden = oDato.Orden;
                                oComponente.Modulo = this.Modulo;
                                oComponente.TipoAtributo = Comun.MODULOINVENTARIO;
                                oComponente.CategoriaAtributoID = this.CategoriaAtributoID;
                                oComponente.CategoriaAtributoAsignacionID = this.CategoriaAtributoAsignacionID;
                                oComponente.AtributoID = oDato.CoreAtributoConfiguracionID;
                                oComponente.EsPlantilla = EsPlantilla;
                                oComponente.EsSoloLectura = EsSoloLectura;
                                listaAtributos.Add(oComponente);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }

                        break;
                    case (long)Comun.Modulos.GLOBAL:
                        try
                        {
                            listaAtributos = new List<Componentes.Atributos>();
                            foreach (var oDato in cEmplAtributos.GetAtributosFromCategoriaAtributo(this.CategoriaAtributoID))
                            {
                                oComponente = (Componentes.Atributos)this.LoadControl("Atributos.ascx");
                                oComponente.ID = "ATR" + oDato.EmplazamientoAtributoConfiguracionID.ToString();
                                oComponente.Nombre = oDato.NombreAtributo;
                                oComponente.Orden = oDato.Orden;
                                oComponente.Modulo = this.Modulo;
                                oComponente.TipoAtributo = Comun.EMPLAZAMIENTOS;
                                oComponente.CategoriaAtributoID = this.CategoriaAtributoID;
                                oComponente.CategoriaAtributoAsignacionID = this.CategoriaAtributoAsignacionID;
                                oComponente.AtributoID = oDato.EmplazamientoAtributoConfiguracionID;
                                oComponente.EsPlantilla = EsPlantilla;
                                oComponente.EsSoloLectura = EsSoloLectura;
                                listaAtributos.Add(oComponente);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }
                        break;
                    default:
                        break;
                }
            }
            try
            {
                if (Ordenar)
                {
                    switch (this.Modulo)
                    {
                        case (long)Comun.Modulos.INVENTARIO:
                            foreach (var item in listaAtributos)
                            {
                                item.Orden = cInvAtributos.GetVinculacion(this.CategoriaAtributoID, item.AtributoID).Orden;
                            }
                            break;

                        case (long)Comun.Modulos.GLOBAL:

                            foreach (var item in listaAtributos)
                            {
                                item.Orden = cEmplAtributos.GetItem(item.AtributoID).Orden;
                            }
                            break;


                        default:
                            break;
                    }
                }
                listaAtributos = listaAtributos.OrderBy(item => item.Orden).ToList();
                foreach (var item in listaAtributos)
                {
                    flexContainer.ContentControls.Add(item);
                }
                if (Update)
                {
                    if (listaAtributos.Count > 0)
                    {
                        foreach (var item in listaAtributos)
                        {
                            item.PintarControl(false);
                        }
                        flexContainer.UpdateContent();
                    }
                    else
                    {
                        flexContainer.Render();
                    }
                }


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            cInvAtributos.Dispose();
            cEmplAtributos.Dispose();
        }

        #endregion

        #region FUNCTION



        #endregion
    }
}