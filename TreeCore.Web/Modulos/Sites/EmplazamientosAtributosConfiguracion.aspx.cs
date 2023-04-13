using Ext.Net;
using TreeCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CapaNegocio;
using log4net;
using System.Reflection;
using System.Data.SqlClient;
using TreeCore.Clases;

namespace TreeCore.ModGlobal
{
    public partial class EmplazamientosAtributosConfiguracion : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        public List<TreeCore.Componentes.CategoriasAtributos> listaCategorias;
        private List<long> listaIDsCategorias;
        protected bool SoloLectura = false;

        #region GESTIÓN DE PÁGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                

                ResourceManagerOperaciones(ResourceManagerTreeCore);


                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion


                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                    cmbClientes.Hidden = false;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }

                switch (Comun.GetRestriccionDefectoEmplazamientos())
                {
                    case (long)Comun.RestriccionesAtributoDefecto.ACTIVE:
                        btnRestriccionActive.Disable();
                        break;
                    case (long)Comun.RestriccionesAtributoDefecto.DISABLED:
                        btnRestriccionDisabled.Disable();
                        break;
                    case (long)Comun.RestriccionesAtributoDefecto.HIDDEN:
                        btnRestriccionHidden.Disable();
                        break;
                    default:
                        break;
                }
                //CargarPagina();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SoloLectura = true;

            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            List<string> listFun = ((List<string>)(this.Session["FUNTIONALITIES"]));
            var UserInterface = ModulesController.GetUserInterfaces().FirstOrDefault(x => x.Page.ToLower() == sPagina.ToLower());
            var listFunPag = listFun.Where(x => $"{x.Split('@')[0]}" == UserInterface.Code);

            if (listFunPag.Where(x => x.Split('@')[1] == "Put").ToList().Count > 0)
            {
                SoloLectura = false;
            }
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { }},
            { "Post", new List<ComponentBase> { }},
            { "Put", new List<ComponentBase> {btnRestriccionActive, btnRestriccionDisabled, btnRestriccionHidden }},
            { "Delete", new List<ComponentBase> { }}
        };
            AtributosConfiguracion.TipoAtributo = Comun.EMPLAZAMIENTOS;
            PintarCategorias(false);
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                CargarPagina();
            }
        }

        #endregion 

        #region STORES

        #region CLIENTES

        protected void storeClientes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Clientes> listaClientes = ListaClientes();

                    if (listaClientes != null)
                    {
                        storeClientes.DataSource = listaClientes;
                    }
                    if (ClienteID.HasValue)
                    {
                        cmbClientes.SelectedItem.Value = ClienteID.Value.ToString();
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

        private List<Data.Clientes> ListaClientes()
        {
            List<Data.Clientes> listaDatos;
            ClientesController cClientes = new ClientesController();

            try
            {
                listaDatos = cClientes.GetActivos();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region CATEGORIAS LIBRES
        protected void storeCategoriasLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Data.EmplazamientosAtributosCategorias> listaFinal = new List<Data.EmplazamientosAtributosCategorias>();
                EmplazamientosAtributosCategoriasController cCategorias = new EmplazamientosAtributosCategoriasController();
                try
                {

                    if (listaIDsCategorias.Count == 0)
                    {
                        foreach (var item in listaCategorias)
                        {
                            listaIDsCategorias.Add(item.CategoriaAtributoID);
                        }
                    }
                    List<Data.EmplazamientosAtributosCategorias> listaCategoriasN = cCategorias.getCategoriasNoSeleccionadas(long.Parse(hdCliID.Value.ToString()));
                    foreach (var item in listaCategoriasN)
                    {
                        if (!listaIDsCategorias.Contains(item.EmplazamientoAtributoCategoriaID))
                        {
                            listaFinal.Add(item);
                        }
                    }
                    if (listaFinal != null && listaFinal.Count > 0)
                    {
                        storeCategoriasLibres.DataSource = listaFinal;
                    }
                    else
                    {
                        storeCategoriasLibres.DataSource = new List<Data.EmplazamientosAtributosCategorias>();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        #endregion

        #endregion

        #region CATEGORIAS

        [DirectMethod()]
        public DirectResponse CambiarRestriccionDefecto(string sBtn)
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                switch (sBtn)
                {
                    case "Active":
                        Comun.SetRestriccionDefectoEmplazamientos((long)Comun.RestriccionesAtributoDefecto.ACTIVE);
                        break;
                    case "Disabled":
                        Comun.SetRestriccionDefectoEmplazamientos((long)Comun.RestriccionesAtributoDefecto.DISABLED);
                        break;
                    case "Hidden":
                        Comun.SetRestriccionDefectoEmplazamientos((long)Comun.RestriccionesAtributoDefecto.HIDDEN);
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

        [DirectMethod(Timeout = 120000)]
        public DirectResponse CargarPagina()
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                listaCategorias = null;
                EmplazamientosAtributosConfiguracionController cAtr = new EmplazamientosAtributosConfiguracionController();
                List<Data.EmplazamientosAtributosConfiguracion> listaDatos = cAtr.GetAtributosFromCliente(long.Parse(hdCliID.Value.ToString()));
                List<long> listaID = listaDatos.GroupBy(item => item.EmplazamientoAtributoCategoriaID).Select(item => item.Key).ToList();
                listaID.ForEach(id =>
                {
                    if (hdListaCategorias.Value == null || hdListaCategorias.Value.ToString() == "")
                    {
                        hdListaCategorias.SetValue(id.ToString());
                    }
                    else
                    {
                        hdListaCategorias.SetValue(hdListaCategorias.Value + "," + id.ToString());
                    }
                });
                PintarCategorias(false, true);
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex);
            }
            return direct;
        }

        [DirectMethod(Timeout = 120000)]
        public DirectResponse PintarCategorias(bool Update, bool Ordenar = false)
        {
            EmplazamientosAtributosCategoriasController cCategorias = new EmplazamientosAtributosCategoriasController();
            TreeCore.Componentes.CategoriasAtributos oComponente;
            Data.EmplazamientosAtributosCategorias oDato;
            long cliID = long.Parse(hdCliID.Value.ToString());
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            if (listaCategorias == null)
            {
                try
                {
                    listaCategorias = new List<TreeCore.Componentes.CategoriasAtributos>();
                    if (hdListaCategorias.Value != null && hdListaCategorias.Value.ToString() != "")
                    {
                        foreach (var idCate in hdListaCategorias.Value.ToString().Split(','))
                        {
                            oDato = cCategorias.GetItem(long.Parse(idCate));
                            oComponente = (TreeCore.Componentes.CategoriasAtributos)this.LoadControl("../../Componentes/CategoriasAtributos.ascx");
                            oComponente.ID = "CAT" + oDato.EmplazamientoAtributoCategoriaID;
                            oComponente.CategoriaAtributoID = oDato.EmplazamientoAtributoCategoriaID;
                            oComponente.CategoriaAtributoAsignacionID = oDato.EmplazamientoAtributoCategoriaID;
                            oComponente.Nombre = oDato.Nombre;
                            try
                            {
                                oComponente.Orden = cCategorias.GetOrdenCategoria(oDato.EmplazamientoAtributoCategoriaID, cliID);
                            }
                            catch (Exception)
                            {
                                oComponente.Orden = 0;
                            }
                            oComponente.Modulo = (long)Comun.Modulos.GLOBAL;
                            oComponente.EsSoloLectura = SoloLectura;
                            listaCategorias.Add(oComponente);
                        }
                    }
                }
                catch (Exception ex)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                }
            }
            try
            {
                listaIDsCategorias = new List<long>();

                if (Ordenar)
                {
                    foreach (var item in listaCategorias)
                    {
                        item.Orden = cCategorias.GetOrdenCategoria(item.CategoriaAtributoID, cliID);
                    }
                }

                listaCategorias = listaCategorias.OrderBy(it => it.Orden).ToList();

                foreach (var item in listaCategorias)
                {
                    pnConfigurador.ContentControls.Add(item);
                    listaIDsCategorias.Add(item.CategoriaAtributoID);
                }

                if (Update)
                {
                    string categorias = "";
                    foreach (var item in listaCategorias)
                    {
                        item.PintarAtributos(false);
                        if (categorias != "")
                            categorias += ",";
                        categorias += item.CategoriaAtributoID.ToString();
                    }

                    if (listaCategorias.Count > 0)
                    {
                        pnConfigurador.UpdateContent();
                    }
                    else
                    {
                        pnConfigurador.Render();
                    }

                    hdListaCategorias.SetValue(categorias);
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

        [DirectMethod]
        public DirectResponse SeleccionarNuevaCategoria(long lCatID)
        {
            EmplazamientosAtributosCategoriasController cCategoriasAtributos = new EmplazamientosAtributosCategoriasController();
            Data.EmplazamientosAtributosCategorias oEmplazamientosAtributosCategorias;
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Result = "";
                direct.Success = true;
                oEmplazamientosAtributosCategorias = cCategoriasAtributos.GetItem(lCatID);
                TreeCore.Componentes.CategoriasAtributos Comp = (TreeCore.Componentes.CategoriasAtributos)this.LoadControl("../../Componentes/CategoriasAtributos.ascx");
                Comp.ID = "CAT" + oEmplazamientosAtributosCategorias.EmplazamientoAtributoCategoriaID;
                Comp.CategoriaAtributoID = oEmplazamientosAtributosCategorias.EmplazamientoAtributoCategoriaID;
                Comp.CategoriaAtributoAsignacionID = oEmplazamientosAtributosCategorias.EmplazamientoAtributoCategoriaID;
                Comp.Nombre = oEmplazamientosAtributosCategorias.Nombre;
                Comp.Orden = listaCategorias.Count;
                Comp.Modulo = (long)Comun.Modulos.GLOBAL;
                Comp.EsSoloLectura = SoloLectura;
                listaCategorias.Add(Comp);
                listaCategorias = listaCategorias.OrderBy(it => it.Orden).ToList();
                PintarCategorias(true);
            }
            catch (Exception ex)
            {
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                direct.Success = false;
                log.Error(ex);
            }
            return direct;
        }

        #endregion
    }





}