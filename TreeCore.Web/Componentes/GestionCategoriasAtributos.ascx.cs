using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using TreeCore.Page;
using System.Linq;
using System.Threading;
using System.Data.Linq;
using Newtonsoft.Json;

namespace TreeCore.Componentes
{
    public partial class GestionCategoriasAtributos : BaseUserControl
    {
        protected long _Orden;
        protected long _Modulo;
        protected long _CategoriaAtributoID;
        protected bool _EsPlantilla = false;
        protected bool _EsSubCategoria = false;
        protected List<Componentes.GestionAtributos> listaAtributos;
        protected ILog log = LogManager.GetLogger("");

        public string Nombre
        {
            get
            {
                return lbNombreCategoria.Text;
            }
            set
            {
                lbNombreCategoria.Text = value;
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

        public bool MostrarPlantillas
        {
            get { return !cmbPlantilla.Hidden; }
            set
            {
                cmbPlantilla.Hidden = !value;
                _EsPlantilla = !value;
            }
        }

        public bool EsPlantilla
        {
            get { return _EsPlantilla; }
            set
            {
                if (value)
                {
                    CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
                    var vLista = cPlantillas.GetPlantillasConf(this.CategoriaAtributoID);

                    if (vLista != null && vLista.Count > 0)
                    {
                        _EsPlantilla = !value;
                    }
                    else
                    {
                        _EsPlantilla = value;
                    }
                }
                else
                {
                    _EsPlantilla = !value;
                }

            }
        }

        public bool EsSubCategoria
        {
            get { return _EsSubCategoria; }
            set
            {
                if (value)
                {
                    hdEsPlantilla.Value = 1;
                }
                _EsSubCategoria = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PintarAtributos(false);
        }


        #region STORES

        #region PLANTILLAS

        protected void storePlantillas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();

                try
                {

                    var vLista = cPlantillas.GetPlantillasConf(this.CategoriaAtributoID);

                    if (vLista != null && vLista.Count > 0)
                    {
                        storePlantillas.DataSource = vLista;
                        storePlantillas.DataBind();
                    }
                    else
                    {
                        storePlantillas.DataSource = new List<Data.CoreInventarioPlantillasAtributosCategorias>();
                        storePlantillas.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    storePlantillas.DataSource = new List<Data.CoreInventarioPlantillasAtributosCategorias>();
                    storePlantillas.DataBind();
                }
            }
        }

        #endregion

        #endregion

        #region DIRECT METHODS

        protected class objCategoria
        {
            public bool cmbPlantillaHidden;
            public List<objAtributo> listaAtributos;
        }
        protected class objAtributo
        {
            public string ID;
            public string Nombre;
            public long Orden;
            public long Modulo;
            public long CategoriaAtributoID;
            public long AtributoID;
            public bool EsPlantilla;
            public bool EsSubcategoria;
        }

        private void CargarAtributos()
        {
            Componentes.GestionAtributos oComponente;
            try
            {
                switch (this.Modulo)
                {
                    #region INVENTARIO

                    case (long)Comun.Modulos.INVENTARIO:
                        CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cInvCategorias = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
                                                
                        if (cInvCategorias.GetItem(this.CategoriaAtributoID).InventarioAtributosCategorias.EsPlantilla)
                        {
                            cmbPlantilla.Hidden = false;
                        }

                        listaAtributos = new List<Componentes.GestionAtributos>();
                        List<Data.Vw_CoreInventarioAtributos> listaAtr = cInvCategorias.GetListaVwAtributos(this.CategoriaAtributoID).ToList();
                        foreach (var oDato in listaAtr)
                        {
                            oComponente = (Componentes.GestionAtributos)this.LoadControl("GestionAtributos.ascx");
                            oComponente.ID = "ATR" + oDato.CoreAtributoConfiguracionID.ToString();
                            oComponente.Nombre = oDato.CodigoCoreAtributoConfg;
                            oComponente.Orden = oDato.Orden;
                            oComponente.Modulo = this.Modulo;
                            oComponente.CategoriaAtributoID = this.CategoriaAtributoID;
                            oComponente.AtributoID = oDato.CoreAtributoConfiguracionID;
                            oComponente.EsPlantilla = EsPlantilla;
                            oComponente.EsSubcategoria = EsSubCategoria;
                            listaAtributos.Add(oComponente);
                        }
                        break;

                    #endregion

                    #region EMPLAZAMIENTOS

                    case (long)Comun.Modulos.GLOBAL:
                        EmplazamientosAtributosConfiguracionController cEmplazamientoAtributos = new EmplazamientosAtributosConfiguracionController();
                        listaAtributos = new List<Componentes.GestionAtributos>();
                        foreach (var oDato in cEmplazamientoAtributos.GetAtributosFromCategoriaAtributo(this.CategoriaAtributoID))
                        {
                            oComponente = (Componentes.GestionAtributos)this.LoadControl("GestionAtributos.ascx");
                            oComponente.ID = "ATR" + oDato.EmplazamientoAtributoConfiguracionID.ToString();
                            oComponente.Nombre = oDato.NombreAtributo;
                            oComponente.Orden = oDato.Orden;
                            oComponente.Modulo = this.Modulo;
                            oComponente.CategoriaAtributoID = this.CategoriaAtributoID;
                            oComponente.AtributoID = oDato.EmplazamientoAtributoConfiguracionID;
                            listaAtributos.Add(oComponente);
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
        public void PintarAtributos(bool Update, bool Ordenar = false)
        {
            Componentes.GestionAtributos oComponente;
            try
            {


                if (listaAtributos == null || listaAtributos.Count == 0)
                {
                    if (X.GetCmp("hdPageLoad") == null)
                    {
                        CargarAtributos();
                    }
                    else
                    {
                        Hidden hdPL = (Hidden)X.GetCmp("hdPageLoad");
                        hdPageLoadController hdController = new hdPageLoadController(hdPL);
                        string oValor = hdController.GetValor("Cat" + this.CategoriaAtributoID);
                        if (oValor == "")
                        {
                            CargarAtributos();
                            var lAtr = new List<objAtributo>();
                            foreach (var oAtr in listaAtributos)
                            {
                                lAtr.Add(new objAtributo
                                {
                                    ID = oAtr.ID,
                                    Nombre = oAtr.Nombre,
                                    Orden = oAtr.Orden,
                                    Modulo = oAtr.Modulo,
                                    CategoriaAtributoID = oAtr.CategoriaAtributoID,
                                    AtributoID = oAtr.AtributoID,
                                    EsPlantilla = oAtr.EsPlantilla,
                                    EsSubcategoria = oAtr.EsSubcategoria
                                });
                            }
                            var objCat = new objCategoria
                            {
                                cmbPlantillaHidden = cmbPlantilla.Hidden,
                                listaAtributos = lAtr
                            };
                            Hidden hdError = (Hidden)X.GetCmp("hdErrorCarga");
                            oValor = JsonConvert.SerializeObject(objCat);
                            hdController.SetValor("Cat" + this.CategoriaAtributoID, oValor);
                        }
                        else
                        {
                            var settings = new JsonSerializerSettings
                            {
                                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                MissingMemberHandling = MissingMemberHandling.Ignore,
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore,
                                DefaultValueHandling = DefaultValueHandling.Ignore,
                                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                            };
                            objCategoria oCat = JsonConvert.DeserializeObject<objCategoria>(oValor, settings);
                            cmbPlantilla.Hidden = oCat.cmbPlantillaHidden;

                            flexContainer.ContentControls.Clear();
                            flexContainer.Dispose();

                            listaAtributos = new List<Componentes.GestionAtributos>();
                            var listaAtributosObj = oCat.listaAtributos.ToList();

                            //foreach (var oAtr in listaAtributosObj)
                            for (int i = 0; i < listaAtributosObj.Count; i++)
                            {
                                oComponente = (Componentes.GestionAtributos)this.LoadControl("GestionAtributos.ascx");
                                oComponente.ID = listaAtributosObj[i].ID;
                                oComponente.Nombre = listaAtributosObj[i].Nombre;
                                oComponente.Orden = listaAtributosObj[i].Orden;
                                oComponente.Modulo = listaAtributosObj[i].Modulo;
                                oComponente.CategoriaAtributoID = listaAtributosObj[i].CategoriaAtributoID;
                                oComponente.AtributoID = listaAtributosObj[i].AtributoID;
                                oComponente.EsPlantilla = listaAtributosObj[i].EsPlantilla;
                                oComponente.EsSubcategoria = listaAtributosObj[i].EsSubcategoria;
                                listaAtributos.Add(oComponente);
                            }

                            GC.KeepAlive(listaAtributosObj);
                        }
                    }
                }
                listaAtributos = listaAtributos.OrderBy(item => item.Orden).ToList();
                foreach (var item in listaAtributos)
                {
                    flexContainer.ContentControls.Add(item);
                }
                if (Update)
                {
                    //flexContainer.Render();
                    //flexContainer.UpdateContent();
                    foreach (var item in listaAtributos)
                    {
                        item.PintarControl(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Hidden hdError = (Hidden)X.GetCmp("hdErrorCarga");
                if (hdError != null)
                {
                    hdError.Value = true;
                }
                log.Error(ex.Message);
            }
        }

        [DirectMethod]
        public DirectResponse SeleccionarPlantilla()
        {
            DirectResponse direct = new DirectResponse();
            CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
            InventarioPlantillasAtributosJsonController cPlsJson = new InventarioPlantillasAtributosJsonController();
            List<Data.CoreInventarioPlantillasAtributosCategoriasAtributos> listaAtributosVal;
            Data.CoreInventarioPlantillasAtributosCategorias oDato;

            List<object> listaValoresAtributos = new List<object>();
            JsonObject jsDatos = new JsonObject();

            try
            {
                long lPlantillaID = long.Parse(cmbPlantilla.Value.ToString());
                oDato = cPlantillas.GetItem(lPlantillaID);

                if (oDato != null)
                {
                    foreach (var item in cPlsJson.Deserializacion(oDato.JsonAtributosDinamicos))
                    {
                        listaValoresAtributos.Add(new
                        {
                            AtributoID = item.AtributoID,
                            Valor = item.Valor
                        });
                    }
                    if (listaValoresAtributos.Count > 0)
                    {
                        foreach (GestionAtributos item in listaAtributos)
                        {
                            item.MostarEditar(listaValoresAtributos, jsDatos);
                        }
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

            direct.Success = true;
            direct.Result = jsDatos;

            return direct;
        }

        #endregion

        #region FUNCTIONS
        public bool GuardarValor(List<Object> listaAtributosValor, TreeCore.Data.TreeCoreContext oContext, List<Object> listaPlantillasValor = null)
        {
            try
            {
                foreach (GestionAtributos item in listaAtributos)
                {
                    if (!item.GuardarValor(listaAtributosValor, oContext))
                    {
                        return false;
                    }

                }
                if (listaPlantillasValor != null && cmbPlantilla.Value != null && cmbPlantilla.Value.ToString() != "")
                {
                    Object plantilla = new
                    {
                        PlantillaID = long.Parse(cmbPlantilla.Value.ToString()),
                        InvCatConfID = this.CategoriaAtributoID,
                        NombrePlantilla = cmbPlantilla.RawText
                    };
                    listaPlantillasValor.Add(plantilla);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
            return true;
        }

        public void MostrarEditar(List<Object> listaValoresAtributos, JsonObject jsDatos, List<Object> listaValoresPlantillas = null)
        {
            foreach (GestionAtributos item in listaAtributos)
            {
                item.MostarEditar(listaValoresAtributos, jsDatos);
            }
            if (listaValoresPlantillas != null && listaValoresPlantillas.Count > 0)
            {
                var PlantillaID = listaValoresPlantillas.First().GetType().GetProperty("PlantillaID");
                var CategoriaID = listaValoresPlantillas.First().GetType().GetProperty("CategoriaID");
                string strValor;
                try
                {
                    strValor = PlantillaID.GetValue(
                        listaValoresPlantillas.FindAll(item =>
                        long.Parse(CategoriaID.GetValue(item).ToString()) == this.CategoriaAtributoID).First()
                        ).ToString();

                    jsDatos.Add(cmbPlantilla.BaseClientID, long.Parse(strValor));
                }
                catch (Exception)
                {
                    strValor = "";
                }

            }
        }

        #endregion
    }
}