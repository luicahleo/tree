
using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using TreeCore.Page;
using System.Linq;
using System.Transactions;

namespace TreeCore.Componentes
{
    public partial class FormGestionElementos : BaseUserControl
    {
        public ILog log = LogManager.GetLogger("");
        private bool _MostrarCategorias;
        private bool _EsPlantilla;
        Data.Emplazamientos EmplazamientoActivo;
        Data.InventarioCategorias CategoriaActiva;
        List<Componentes.GestionCategoriasAtributos> listaCategorias;

        public bool MostrarCategorias
        {
            get { return !cmbCategoriaElemento.Disabled; }
            set
            {
                this.cmbCategoriaElemento.Disabled = !value;
                this.cmbCategoriaElemento.AllowBlank = !value;
            }
        }
        public long CategoriaID
        {
            get { return long.Parse(hdCatID.Value.ToString()); }
            set
            {
                this.hdCatID.SetValue(value);
            }
        }
        public long ElementoPadreID
        {
            get { return long.Parse(hdElementoPadreID.Value.ToString()); }
            set { this.hdElementoPadreID.SetValue(value); }
        }
        public long ElementoID
        {
            get { return long.Parse(hdElementoID.Value.ToString()); }
            set { this.hdElementoID.SetValue(value); }
        }
        public bool EsPlantilla
        {
            get { return _EsPlantilla; }
            set
            {
                _EsPlantilla = value;
                if (value)
                {
                    cmbEstado.Hide();
                    cmbOperador.Hide();
                }
            }
        }
        public long? Operador
        {
            set
            {
                cmbOperador.SetValue(value);
                cmbOperador.Triggers[0].Hidden = false;
                hdOperadorID.SetValue(value);
            }
        }

        public long ProyectoTipo
        {
            get
            {
                return long.Parse(hdProyectoTipo.Value.ToString());
            }
            set
            {
                hdProyectoTipo.SetValue(value);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PintarCategorias(false);
        }

        #region STORES

        #region CATEGORIAS

        protected void storeCategorias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                EmplazamientosController cEmplazamientos = new EmplazamientosController();
                try
                {
                    if (hdCatPadreID.Value != null && hdCatPadreID.Value.ToString() != "")
                    {
                        long lCatPadreID = long.Parse(hdCatPadreID.Value.ToString());
                        long EmplazamientoID = long.Parse(((Hidden)X.GetCmp("hdEmplazamientoID")).Value.ToString());
                        EmplazamientoActivo = cEmplazamientos.GetItem(EmplazamientoID);
                        List<Data.InventarioCategorias> listaDatos;
                        if (EmplazamientoActivo != null)
                        {
                            if (CategoriaID == 0)
                            {
                                listaDatos = cCategorias.GetCategoriasHijas(lCatPadreID, EmplazamientoActivo.EmplazamientoTipoID);
                            }
                            else
                            {
                                listaDatos = new List<Data.InventarioCategorias>();
                                listaDatos.Add(cCategorias.GetItem(CategoriaID));
                            }
                        }
                        else
                        {
                            long CategoriaID = long.Parse(((Hidden)X.GetCmp("hdCategoriaID")).Value.ToString());
                            listaDatos = new List<Data.InventarioCategorias>();
                            listaDatos.Add(cCategorias.GetItem(CategoriaID));
                        }
                        storeCategoriasElementos.DataSource = listaDatos;
                        storeCategoriasElementos.DataBind();
                        if (listaDatos == null || listaDatos.Count == 0)
                        {
                            cmbCategoriaElemento.EmptyText = GetGlobalResource("strNoCategorias");
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

        #region ESTADOS

        protected void storeEstados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioElementosAtributosEstadosController cEstados = new InventarioElementosAtributosEstadosController();
                EmplazamientosController cEmplazamientos = new EmplazamientosController();
                InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                List<Data.InventarioElementosAtributosEstados> listaDatos;
                try
                {

                    long EmplazamientoID = long.Parse(((Hidden)X.GetCmp("hdEmplazamientoID")).Value.ToString());
                    EmplazamientoActivo = cEmplazamientos.GetItem(EmplazamientoID);
                    Data.InventarioElementosAtributosEstados oDato;
                    if (EmplazamientoActivo != null)
                    {
                        listaDatos = cEstados.GetActivos(EmplazamientoActivo.ClienteID);
                        oDato = cEstados.GetDefault(EmplazamientoActivo.ClienteID);
                    }
                    else
                    {
                        long CategoriaID = long.Parse(((Hidden)X.GetCmp("hdCategoriaID")).Value.ToString());
                        CategoriaActiva = cCategorias.GetItem(CategoriaID);
                        listaDatos = cEstados.GetActivos((long)CategoriaActiva.ClienteID);
                        oDato = cEstados.GetDefault((long)CategoriaActiva.ClienteID);
                    }
                    storeEstados.DataSource = listaDatos;
                    storeEstados.DataBind();

                    if (oDato != null)
                    {
                        cmbEstado.SetValue(oDato.InventarioElementoAtributoEstadoID);
                        cmbEstado.Triggers[0].Hidden = false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

        }

        #endregion

        #region OPERADORES

        protected void storeOperadores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_EntidadesOperadores> listaOperadores;
                    InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                    OperadoresController cOperador = new OperadoresController();
                    long EmplazamientoID = long.Parse(((Hidden)X.GetCmp("hdEmplazamientoID")).Value.ToString());
                    EmplazamientosController cEmplazamientos = new EmplazamientosController();
                    EmplazamientoActivo = cEmplazamientos.GetItem(EmplazamientoID);
                    Data.InventarioElementosAtributosEstados oDato;
                    if (EmplazamientoActivo != null)
                    {
                        listaOperadores = cOperador.getEntidadesOperadores(EmplazamientoActivo.ClienteID);
                    }
                    else
                    {
                        long CategoriaID = long.Parse(((Hidden)X.GetCmp("hdCategoriaID")).Value.ToString());
                        CategoriaActiva = cCategorias.GetItem(CategoriaID);
                        listaOperadores = cOperador.getEntidadesOperadores((long)CategoriaActiva.ClienteID);
                    }
                    storeOperadores.DataSource = listaOperadores;
                    storeOperadores.DataBind();
                    string sOperador;
                    if (hdOperadorID.Value != null)
                    {
                        sOperador = hdOperadorID.Value.ToString();
                    }
                    else
                    {
                        sOperador = "";
                    }

                    if (sOperador.Equals(""))
                    {
                        Data.Vw_EntidadesOperadores oOperador;
                        Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
                        oOperador = cOperador.getOperadorUsuario(oUsuario.UsuarioID);
                        if (oOperador != null)
                        {
                            this.Operador = oOperador.OperadorID;
                        }
                        else
                        {
                            if (EmplazamientoActivo != null)
                            {
                                oOperador = cOperador.getOperadorCliente(EmplazamientoActivo.ClienteID);
                            }
                            else
                            {
                                long CategoriaID = long.Parse(((Hidden)X.GetCmp("hdCategoriaID")).Value.ToString());
                                CategoriaActiva = cCategorias.GetItem(CategoriaID);
                                oOperador = cOperador.getOperadorCliente((long)CategoriaActiva.ClienteID);
                            }
                            this.Operador = oOperador.OperadorID;
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

        #region PLANTILLAS

        protected void storePlantillas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    if (CategoriaID != 0)
                    {
                        EmplazamientosController cEmplazamientos = new EmplazamientosController();
                        InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                        long EmplazamientoID = long.Parse(((Hidden)X.GetCmp("hdEmplazamientoID")).Value.ToString());
                        EmplazamientoActivo = cEmplazamientos.GetItem(EmplazamientoID);
                        InventarioElementosController cElementos = new InventarioElementosController();
                        List<Data.InventarioElementos> listaDatos = new List<Data.InventarioElementos>();
                        long catID;
                        if (CategoriaID != 0)
                        {
                            catID = CategoriaID;
                        }
                        else
                        {
                            catID = long.Parse(cmbCategoriaElemento.SelectedItem.Value);
                        }
                        if (EmplazamientoActivo != null)
                        {
                            //listaDatos = cElementos.GetPlantillasCategoria(catID, EmplazamientoActivo.ClienteID);
                        }
                        else
                        {
                            long CategoriaID = long.Parse(((Hidden)X.GetCmp("hdCategoriaID")).Value.ToString());
                            CategoriaActiva = cCategorias.GetItem(CategoriaID);
                            //listaDatos = cElementos.GetPlantillasCategoria(catID, (long)CategoriaActiva.ClienteID);
                        }
                        storePlantillas.DataSource = listaDatos;
                        storePlantillas.DataBind();
                        if (hdPlantillaID.Value != null && hdPlantillaID.Value != "")
                        {
                            cmbPlantilla.SetValue(hdPlantillaID.Value);
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

        #endregion

        #region DIRECT METHODS

        [DirectMethod()]
        public DirectResponse PintarCategorias(bool Update)
        {
            CoreInventarioCategoriasAtributosCategoriasController cCategorias = new CoreInventarioCategoriasAtributosCategoriasController();
            Componentes.GestionCategoriasAtributos oComponente;
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                if (Update && contenedorCategorias != null && contenedorCategorias.ContentControls.Count > 0)
                {
                    listaCategorias = new List<GestionCategoriasAtributos>();
                    contenedorCategorias.ContentControls.Clear();
                    contenedorCategorias.Dispose();
                }
                if (hdCatID.Value != null && hdCatID.Value.ToString() != "" && long.Parse(hdCatID.Value.ToString()) != 0)
                {
                    if (listaCategorias == null || listaCategorias.Count == 0)
                    {
                        listaCategorias = new List<Componentes.GestionCategoriasAtributos>();
                        long lCatID = long.Parse(hdCatID.Value.ToString());
                        List<Data.CoreInventarioCategoriasAtributosCategorias> listaInventarioAtributosCategorias;
                        listaInventarioAtributosCategorias = cCategorias.GetCategoriasAtributosVinculadas(lCatID);
                        foreach (var idCate in listaInventarioAtributosCategorias)
                        {
                            oComponente = (Componentes.GestionCategoriasAtributos)this.LoadControl("GestionCategoriasAtributos.ascx");
                            oComponente.ID = "CAT" + idCate.CoreInventarioCategoriaAtributoCategoriaID;
                            oComponente.CategoriaAtributoID = idCate.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                            oComponente.Nombre = idCate.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.InventarioAtributoCategoria;
                            oComponente.Orden = idCate.Orden;
                            oComponente.Modulo = (long)Comun.Modulos.INVENTARIO;
                            listaCategorias.Add(oComponente);
                        }
                    }
                    listaCategorias = listaCategorias.OrderBy(it => it.Orden).ToList();
                    foreach (var item in listaCategorias)
                    {
                        contenedorCategorias.ContentControls.Add(item);
                    }
                }
                if (Update)
                {
                    //contenedorCategorias.Render();
                    if (listaCategorias != null && listaCategorias.Count != 0)
                    {
                        foreach (var item in listaCategorias)
                        {
                            item.PintarAtributos(true);
                        }
                    }
                    contenedorCategorias.UpdateContent();
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

        /*[DirectMethod()]
        public DirectResponse GuardarValor(bool SobrescribirEdicion)
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            List<Object> listaAtributos = new List<object>();
            Data.InventarioElementosAtributos oInventarioElementosAtributos;
            InventarioElementosController cInventarioElementos = new InventarioElementosController();
            InventarioElementosAtributosController cInventarioElementosAtributos = new InventarioElementosAtributosController();
            cInventarioElementosAtributos.SetDataContext(cInventarioElementos.Context);
            Data.InventarioElementos oInventarioElementos;
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            cEmplazamientos.SetDataContext(cInventarioElementos.Context);
            InventarioCategoriasController cCategorias = new InventarioCategoriasController();
            cCategorias.SetDataContext(cInventarioElementos.Context);
            JsonObject json = new JsonObject();
            InventarioElementosHistoricosController cHistorico = new InventarioElementosHistoricosController();
            cHistorico.SetDataContext(cInventarioElementos.Context);
            Data.InventarioElementosHistoricos DatoHistorico;

            long lCLienteID;

            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {

                long EmplazamientoID = long.Parse(((Hidden)X.GetCmp("hdEmplazamientoID")).Value.ToString());
                EmplazamientoActivo = cEmplazamientos.GetItem(EmplazamientoID);

                if (EmplazamientoActivo != null)
                {
                    lCLienteID = EmplazamientoActivo.ClienteID;
                }
                else
                {
                    long CategoriaID = long.Parse(((Hidden)X.GetCmp("hdCategoriaID")).Value.ToString());
                    CategoriaActiva = cCategorias.GetItem(CategoriaID);
                    lCLienteID = (long)CategoriaActiva.ClienteID;
                }

                if (hdElementoID.Value != null && hdElementoID.Value.ToString() != "" && hdElementoID.Value.ToString() != "0")
                {
                    try
                    {
                        oInventarioElementos = cInventarioElementos.GetItem(long.Parse(hdElementoID.Value.ToString()));
                        if (txtCodigoElemento.Text == oInventarioElementos.NumeroInventario)
                        {

                            DatoHistorico = cHistorico.getHistoricoByID(oInventarioElementos.InventarioElementoID);

                            if (SobrescribirEdicion || (hdHistoricoInventario.Value.ToString() == DatoHistorico.InventarioElementoHistoricoID.ToString()))
                            {
                                #region UPDATE

                                oInventarioElementos.Nombre = txtNombreElemento.Text;
                                oInventarioElementos.Plantilla = (((Hidden)X.GetCmp("hdVistaPlantilla")).Value != null && ((Hidden)X.GetCmp("hdVistaPlantilla")).Value.ToString() != "");
                                if (!oInventarioElementos.Plantilla)
                                {
                                    oInventarioElementos.InventarioElementoAtributoEstadoID = long.Parse(cmbEstado.SelectedItem.Value);
                                    oInventarioElementos.OperadorID = long.Parse(cmbOperador.SelectedItem.Value.ToString());
                                }
                                if (cmbPlantilla.SelectedItem.Value != null && cmbPlantilla.SelectedItem.Value != "")
                                {
                                    oInventarioElementos.PlantillaID = long.Parse(cmbPlantilla.SelectedItem.Value);
                                }
                                else
                                {
                                    oInventarioElementos.PlantillaID = null;
                                }
                                oInventarioElementos.UltimaModificacionFecha = DateTime.Now;
                                oInventarioElementos.UltimaModificacionUsuarioID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID;
                                foreach (var item in listaCategorias)
                                {
                                    if (!item.GuardarValor(listaAtributos, cInventarioElementos.Context))
                                    {
                                        trans.Dispose();
                                        direct.Success = false;
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                                foreach (var item in listaAtributos)
                                {
                                    var jsonAux = new JsonObject();
                                    var AtributoID = item.GetType().GetProperty("AtributoID");
                                    var NombreAtributo = item.GetType().GetProperty("NombreAtributo");
                                    var Valor = item.GetType().GetProperty("Valor");
                                    var TextLista = item.GetType().GetProperty("TextLista");
                                    var TipoDato = item.GetType().GetProperty("TipoDato");
                                    jsonAux.Add("AtributoID", AtributoID.GetValue(item));
                                    jsonAux.Add("NombreAtributo", NombreAtributo.GetValue(item));
                                    jsonAux.Add("Valor", Valor.GetValue(item));
                                    jsonAux.Add("TextLista", TextLista.GetValue(item));
                                    jsonAux.Add("TipoDato", TipoDato.GetValue(item));
                                    json.Add(AtributoID.GetValue(item).ToString(), jsonAux);
                                }
                                oInventarioElementos.JsonAtributosDinamicos = json.ToJsonString();
                                foreach (var item in listaAtributos)
                                {
                                    var AtributoID = item.GetType().GetProperty("AtributoID");
                                    var Valor = item.GetType().GetProperty("Valor");
                                    oInventarioElementosAtributos = cInventarioElementosAtributos.GetAtributoElemento(oInventarioElementos.InventarioElementoID, long.Parse(AtributoID.GetValue(item).ToString()));
                                    if (oInventarioElementosAtributos == null)
                                    {
                                        oInventarioElementosAtributos = new Data.InventarioElementosAtributos();
                                        oInventarioElementosAtributos.InventarioElementoID = oInventarioElementos.InventarioElementoID;
                                        oInventarioElementosAtributos.InventarioAtributoID = long.Parse(AtributoID.GetValue(item).ToString());
                                        oInventarioElementosAtributos.CreadorID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID;
                                        oInventarioElementosAtributos.Valor = Valor.GetValue(item).ToString();
                                        oInventarioElementosAtributos.Activo = true;
                                        oInventarioElementosAtributos.FechaCreacion = DateTime.Now;
                                        cInventarioElementosAtributos.AddItem(oInventarioElementosAtributos);
                                    }
                                    else
                                    {
                                        oInventarioElementosAtributos.Valor = Valor.GetValue(item).ToString();
                                        cInventarioElementosAtributos.UpdateItem(oInventarioElementosAtributos);
                                    }
                                }
                                cInventarioElementos.UpdateItem(oInventarioElementos);
                                //if (!cHistorico.CrearHistorico(oInventarioElementos, listaAtributos, listaPlatillas))
                                //{
                                //    trans.Dispose();
                                //    direct.Success = false;
                                //    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                //}
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
                            if (!cInventarioElementos.ComprobarDuplicadoCodigo(txtCodigoElemento.Text, lCLienteID))
                            {
                                oInventarioElementos.NumeroInventario = txtCodigoElemento.Text;
                                oInventarioElementos.Nombre = txtNombreElemento.Text;
                                oInventarioElementos.Plantilla = (((Hidden)X.GetCmp("hdVistaPlantilla")).Value != null && ((Hidden)X.GetCmp("hdVistaPlantilla")).Value.ToString() != "");
                                if (!oInventarioElementos.Plantilla)
                                {
                                    oInventarioElementos.InventarioElementoAtributoEstadoID = long.Parse(cmbEstado.SelectedItem.Value);
                                    oInventarioElementos.OperadorID = long.Parse(cmbOperador.SelectedItem.Value.ToString());
                                }
                                if (cmbPlantilla.SelectedItem.Value != null && cmbPlantilla.SelectedItem.Value != "")
                                {
                                    oInventarioElementos.PlantillaID = long.Parse(cmbPlantilla.SelectedItem.Value);
                                }
                                else
                                {
                                    oInventarioElementos.PlantillaID = null;
                                }
                                oInventarioElementos.UltimaModificacionFecha = DateTime.Now;
                                oInventarioElementos.UltimaModificacionUsuarioID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID;
                                foreach (var item in listaCategorias)
                                {
                                    if (!item.GuardarValor(listaAtributos, cInventarioElementos.Context))
                                    {
                                        trans.Dispose();
                                        direct.Success = false;
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                                foreach (var item in listaAtributos)
                                {
                                    var jsonAux = new JsonObject();
                                    var AtributoID = item.GetType().GetProperty("AtributoID");
                                    var NombreAtributo = item.GetType().GetProperty("NombreAtributo");
                                    var Valor = item.GetType().GetProperty("Valor");
                                    var TextLista = item.GetType().GetProperty("TextLista");
                                    var TipoDato = item.GetType().GetProperty("TipoDato");
                                    jsonAux.Add("AtributoID", AtributoID.GetValue(item));
                                    jsonAux.Add("NombreAtributo", NombreAtributo.GetValue(item));
                                    jsonAux.Add("Valor", Valor.GetValue(item));
                                    jsonAux.Add("TextLista", TextLista.GetValue(item));
                                    jsonAux.Add("TipoDato", TipoDato.GetValue(item));
                                    json.Add(AtributoID.GetValue(item).ToString(), jsonAux);
                                }
                                oInventarioElementos.JsonAtributosDinamicos = json.ToJsonString();
                                cInventarioElementos.UpdateItem(oInventarioElementos);
                                //if (!cHistorico.CrearHistorico(oInventarioElementos, listaAtributos, listaPlatillas))
                                //{
                                //    trans.Dispose();
                                //    direct.Success = false;
                                //    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                //}
                            }
                            else
                            {
                                log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.jsCodigoExiste);
                                trans.Dispose();
                                return direct;
                            }
                        }
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        trans.Complete();
                    }
                    catch (Exception ex)
                    {
                        trans.Dispose();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    }
                }
                else
                {
                    if (!cInventarioElementos.ComprobarDuplicadoCodigo(txtCodigoElemento.Text, lCLienteID))
                    {
                        try
                        {
                            foreach (var item in listaCategorias)
                            {
                                if (!item.GuardarValor(listaAtributos, cInventarioElementos.Context))
                                {
                                    trans.Dispose();
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                    return direct;
                                }
                            }
                            oInventarioElementos = new Data.InventarioElementos();
                            oInventarioElementos.InventarioCategoriaID = long.Parse(hdCatID.Value.ToString());
                            if (hdElementoPadreID.Value != null && hdElementoPadreID.Value.ToString() != "" && long.Parse(hdElementoPadreID.Value.ToString()) != 0)
                            {
                                oInventarioElementos.InventarioElementoPadreID = long.Parse(hdElementoPadreID.Value.ToString());
                            }
                            else
                            {
                                oInventarioElementos.InventarioElementoPadreID = null;
                            }
                            oInventarioElementos.CreadorID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID;
                            oInventarioElementos.NumeroInventario = txtCodigoElemento.Text;
                            oInventarioElementos.Nombre = txtNombreElemento.Text;
                            oInventarioElementos.FechaCreacion = DateTime.Now;
                            oInventarioElementos.FechaAlta = DateTime.Now;
                            oInventarioElementos.Plantilla = (((Hidden)X.GetCmp("hdVistaPlantilla")).Value != null && ((Hidden)X.GetCmp("hdVistaPlantilla")).Value.ToString() != "");
                            oInventarioElementos.UltimaModificacionUsuarioID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID;
                            oInventarioElementos.Activo = true;
                            if (!oInventarioElementos.Plantilla)
                            {
                                oInventarioElementos.InventarioElementoAtributoEstadoID = long.Parse(cmbEstado.SelectedItem.Value);
                                oInventarioElementos.OperadorID = long.Parse(cmbOperador.SelectedItem.Value.ToString());
                                oInventarioElementos.EmplazamientoID = (long.Parse(((Hidden)X.GetCmp("hdEmplazamientoID")).Value.ToString()) == 0) ? null : (long?)long.Parse(((Hidden)X.GetCmp("hdEmplazamientoID")).Value.ToString());
                            }
                            if (cmbPlantilla.SelectedItem.Value != null && cmbPlantilla.SelectedItem.Value != "")
                            {
                                oInventarioElementos.PlantillaID = long.Parse(cmbPlantilla.SelectedItem.Value);
                            }
                            else
                            {
                                oInventarioElementos.PlantillaID = null;
                            }
                            foreach (var item in listaAtributos)
                            {
                                var jsonAux = new JsonObject();
                                var AtributoID = item.GetType().GetProperty("AtributoID");
                                var NombreAtributo = item.GetType().GetProperty("NombreAtributo");
                                var Valor = item.GetType().GetProperty("Valor");
                                var TextLista = item.GetType().GetProperty("TextLista");
                                var TipoDato = item.GetType().GetProperty("TipoDato");
                                jsonAux.Add("AtributoID", AtributoID.GetValue(item));
                                jsonAux.Add("NombreAtributo", NombreAtributo.GetValue(item));
                                jsonAux.Add("Valor", Valor.GetValue(item));
                                jsonAux.Add("TextLista", TextLista.GetValue(item));
                                jsonAux.Add("TipoDato", TipoDato.GetValue(item));
                                json.Add(AtributoID.GetValue(item).ToString(), jsonAux);
                            }
                            oInventarioElementos.JsonAtributosDinamicos = json.ToJsonString();
                            oInventarioElementos = cInventarioElementos.AddItem(oInventarioElementos);
                            foreach (var item in listaAtributos)
                            {
                                var AtributoID = item.GetType().GetProperty("AtributoID");
                                var Valor = item.GetType().GetProperty("Valor");
                                oInventarioElementosAtributos = new Data.InventarioElementosAtributos();
                                oInventarioElementosAtributos.InventarioElementoID = oInventarioElementos.InventarioElementoID;
                                oInventarioElementosAtributos.InventarioAtributoID = long.Parse(AtributoID.GetValue(item).ToString());
                                oInventarioElementosAtributos.CreadorID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID;
                                oInventarioElementosAtributos.Valor = Valor.GetValue(item).ToString();
                                oInventarioElementosAtributos.Activo = true;
                                oInventarioElementosAtributos.FechaCreacion = DateTime.Now;
                                cInventarioElementosAtributos.AddItem(oInventarioElementosAtributos);
                            }

                            #region GENERAR HISTORICO

                            //if (!cHistorico.CrearHistorico(oInventarioElementos, listaAtributos, listaPlatillas))
                            //{
                            //    direct.Success = false;
                            //    direct.Result = GetLocalResourceObject(Comun.strMensajeGenerico).ToString();
                            //    return direct;
                            //}

                            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
                            cCondicionesConfiguraciones.SetDataContext(cInventarioElementos.Context);

                            if (hdCondicionCodigoReglaID.Value.ToString() != "" && hdCodigoAutogenerado.Value.ToString() == txtCodigoElemento.Text.Trim())
                            {
                                if (!cCondicionesConfiguraciones.ActualizarUltimoCodigoByReglaID(long.Parse(hdCondicionCodigoReglaID.Value.ToString()), txtCodigoElemento.Text))
                                {
                                    direct.Success = false;
                                    direct.Result = GetLocalResourceObject("strErrorActualizarCodigoAutomatico").ToString();
                                    return direct;
                                }
                            }

                            if (hdCondicionNombreReglaID.Value.ToString() != "" && hdNombreAutogenerado.Value.ToString() == txtNombreElemento.Text.Trim())
                            {
                                if (!cCondicionesConfiguraciones.ActualizarUltimoCodigoByReglaID(long.Parse(hdCondicionNombreReglaID.Value.ToString()), txtNombreElemento.Text))
                                {
                                    direct.Success = false;
                                    direct.Result = GetLocalResourceObject("strErrorActualizarCodigoAutomatico").ToString();
                                    return direct;
                                }
                            }

                            #endregion

                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            trans.Complete();
                        }
                        catch (Exception ex)
                        {
                            trans.Dispose();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        }
                    }
                    else
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsCodigoExiste);
                        trans.Dispose();
                        return direct;
                    }
                }
            }
            return direct;
        }*/

        [DirectMethod]
        public DirectResponse SeleccionarPlantilla()
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                List<Data.Vw_InventarioElementosAtributos> listaInventarioElementosAtributos;
                InventarioElementosAtributosController cInventarioElementosAtributos = new InventarioElementosAtributosController();
                JsonObject jsDatos = new JsonObject();
                InventarioElementosController cInventarioElementos = new InventarioElementosController();
                Data.InventarioElementos oPlantilla;
                List<object> listaValoresAtributos = new List<object>();
                oPlantilla = cInventarioElementos.GetItem(long.Parse(cmbPlantilla.SelectedItem.Value));
                listaInventarioElementosAtributos = cInventarioElementosAtributos.GetElementosAtributosByElementoID(oPlantilla.InventarioElementoID);

                foreach (var item in listaInventarioElementosAtributos)
                {
                    listaValoresAtributos.Add(new
                    {
                        AtributoID = item.InventarioAtributoID,
                        Valor = item.Valor
                    });
                }

                foreach (var item in listaCategorias)
                {
                    item.MostrarEditar(listaValoresAtributos, jsDatos);
                }
                direct.Result = jsDatos;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }
            return direct;
        }

        #region GENERACION AUTOMÁTICA CÓDIGO

        [DirectMethod()]
        public DirectResponse GenerarCodigoInventario()
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            GlobalCondicionesReglasController cCondicionesReglasController = new GlobalCondicionesReglasController();
            InventarioCategoriasController cCategorias = new InventarioCategoriasController();
            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            Data.GlobalCondicionesReglas aplicarRegla;
            List<Data.GlobalCondicionesReglasConfiguraciones> configuraciones;

            try
            {
                #region CODIGO

                aplicarRegla = cCondicionesReglasController.GetReglaByCampoDestino("CODIGO_INVENTARIO", this.ProyectoTipo);

                if (aplicarRegla != null)
                {
                    configuraciones = cCondicionesConfiguraciones.GlobalCondicionesReglasConfiguracionesBySeleccionadoID(aplicarRegla.GlobalCondicionReglaID);

                    if (configuraciones != null && configuraciones.Count > 0)
                    {
                        string siguienteCodigo;
                        long EmplazamientoID = long.Parse(((Hidden)X.GetCmp("hdEmplazamientoID")).Value.ToString());
                        EmplazamientoActivo = cEmplazamientos.GetItem(EmplazamientoID);
                        if (EmplazamientoActivo != null)
                        {
                            siguienteCodigo = cCondicionesConfiguraciones.GetSiguienteByListaCondicionesReglasConfiguraciones(configuraciones, aplicarRegla.UltimoGenerado, aplicarRegla.Modificada, EmplazamientoActivo.ClienteID);
                        }
                        else
                        {
                            long CategoriaID = long.Parse(((Hidden)X.GetCmp("hdCategoriaID")).Value.ToString());
                            CategoriaActiva = cCategorias.GetItem(CategoriaID);
                            siguienteCodigo = cCondicionesConfiguraciones.GetSiguienteByListaCondicionesReglasConfiguraciones(configuraciones, aplicarRegla.UltimoGenerado, aplicarRegla.Modificada, (long)CategoriaActiva.ClienteID);
                        }

                        if (siguienteCodigo == null)
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strGeneracionCodigoFallida);
                            return direct;
                        }
                        else
                        {
                            txtCodigoElemento.SetValue(siguienteCodigo);

                            hdCodigoAutogenerado.SetValue(siguienteCodigo);
                            hdCondicionCodigoReglaID.SetValue(aplicarRegla.GlobalCondicionReglaID);

                            direct.Result = cCondicionesReglasController.getConfiguracionRegla(aplicarRegla.GlobalCondicionReglaID);
                        }
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strGeneracionSinRegla);
                        return direct;
                    }
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
        public DirectResponse GenerarNombreInventario()
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            GlobalCondicionesReglasController cCondicionesReglasController = new GlobalCondicionesReglasController();
            InventarioCategoriasController cCategorias = new InventarioCategoriasController();
            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            Data.GlobalCondicionesReglas aplicarRegla;
            List<Data.GlobalCondicionesReglasConfiguraciones> configuraciones;

            try
            {

                #region NOMBRE

                aplicarRegla = cCondicionesReglasController.GetReglaByCampoDestino("NOMBRE_INVENTARIO", this.ProyectoTipo);

                if (aplicarRegla != null)
                {
                    configuraciones = cCondicionesConfiguraciones.GlobalCondicionesReglasConfiguracionesBySeleccionadoID(aplicarRegla.GlobalCondicionReglaID);

                    if (configuraciones != null && configuraciones.Count > 0)
                    {
                        string siguienteNombre;
                        long EmplazamientoID = long.Parse(((Hidden)X.GetCmp("hdEmplazamientoID")).Value.ToString());
                        EmplazamientoActivo = cEmplazamientos.GetItem(EmplazamientoID);
                        if (EmplazamientoActivo != null)
                        {
                            siguienteNombre = cCondicionesConfiguraciones.GetSiguienteByListaCondicionesReglasConfiguraciones(configuraciones, aplicarRegla.UltimoGenerado, aplicarRegla.Modificada, EmplazamientoActivo.ClienteID);
                        }
                        else
                        {
                            long CategoriaID = long.Parse(((Hidden)X.GetCmp("hdCategoriaID")).Value.ToString());
                            CategoriaActiva = cCategorias.GetItem(CategoriaID);
                            siguienteNombre = cCondicionesConfiguraciones.GetSiguienteByListaCondicionesReglasConfiguraciones(configuraciones, aplicarRegla.UltimoGenerado, aplicarRegla.Modificada, (long)CategoriaActiva.ClienteID);
                        }

                        if (siguienteNombre == null)
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strGeneracionNombreFallida);
                            return direct;
                        }
                        else
                        {
                            txtNombreElemento.SetValue(siguienteNombre);

                            hdNombreAutogenerado.SetValue(siguienteNombre);
                            hdCondicionNombreReglaID.SetValue(aplicarRegla.GlobalCondicionReglaID);
                        }
                    }
                    direct.Result = cCondicionesReglasController.getConfiguracionRegla(aplicarRegla.GlobalCondicionReglaID);
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

        #endregion

        #endregion

        #region FUNCTIONS

        public JsonObject MostrarEditar(string lElementoID)
        {
            long ElementoID = long.Parse(lElementoID);
            List<Data.Vw_InventarioElementosAtributos> listaInventarioElementosAtributos;
            InventarioElementosAtributosController cInventarioElementosAtributos = new InventarioElementosAtributosController();
            List<object> listaValoresAtributos = new List<object>();
            Data.InventarioElementos oInventarioElementos;
            InventarioElementosController cInventarioElementos = new InventarioElementosController();
            JsonObject jsDatos = new JsonObject();

            oInventarioElementos = cInventarioElementos.GetItem(ElementoID);
            listaInventarioElementosAtributos = cInventarioElementosAtributos.GetElementosAtributosByElementoID(oInventarioElementos.InventarioElementoID);


            InventarioElementosHistoricosController cHistorico = new InventarioElementosHistoricosController();
            Data.InventarioElementosHistoricos DatoHistorico;

            DatoHistorico = cHistorico.getHistoricoByID(ElementoID);
            hdHistoricoInventario.Value = DatoHistorico.InventarioElementoHistoricoID.ToString();

            foreach (var item in listaInventarioElementosAtributos)
            {
                listaValoresAtributos.Add(new
                {
                    AtributoID = item.InventarioAtributoID,
                    Valor = item.Valor
                });
            }

            this.CategoriaID = oInventarioElementos.InventarioCategoriaID;
            this.Operador = oInventarioElementos.OperadorID;
            cmbOperador.SetValue(oInventarioElementos.OperadorID);
            this.cmbCategoriaElemento.Disabled = true;
            this.cmbCategoriaElemento.AllowBlank = true;

            PintarCategorias(true);

            if (listaValoresAtributos.Count > 0)
            {
                foreach (var item in listaCategorias)
                {
                    item.MostrarEditar(listaValoresAtributos, jsDatos);
                }
            }
            //contenedorCategorias.Render();


            cmbCategoriaElemento.SetValue(oInventarioElementos.InventarioCategoriaID);
            cmbEstado.SetValue(oInventarioElementos.InventarioElementoAtributoEstadoID);
            txtCodigoElemento.SetText(oInventarioElementos.NumeroInventario);
            txtNombreElemento.SetText(oInventarioElementos.Nombre);
            if (oInventarioElementos.PlantillaID != null)
            {
                hdPlantillaID.SetValue(oInventarioElementos.PlantillaID);
            }
            this.EsPlantilla = oInventarioElementos.Plantilla;
            return jsDatos;
        }

        #endregion
    }
}