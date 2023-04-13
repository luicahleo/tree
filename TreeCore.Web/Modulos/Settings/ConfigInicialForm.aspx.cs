using System;
using System.IO;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using log4net;
using log4net.Repository.Hierarchy;
using System.Reflection;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TreeCore.ModGlobal
{
    public partial class ConfigInicialForm : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        public string sCarpeta;

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                
                ResourceManagerOperaciones(ResourceManagerTreeCore);
                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }

                hdUsuarioID.SetValue(Usuario.UsuarioID);
                MostrarEditarUsuario();
                storeIdiomas.Reload();
                storePais.Reload();
                MostrarEditarEntidad();
                storeMonedas.Reload();



            }


            #region EXCEL

            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> { }},
                { "Post", new List<ComponentBase> { }},
                { "Put", new List<ComponentBase> { }},
                { "Delete", new List<ComponentBase> { }}
            };
        }

        #endregion

        #region STORES

        #region STORE IDIOMAS

        protected void storeIdiomas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var ls = ListaIdiomas();
                    if (ls != null)
                    {
                        storeIdiomas.DataSource = ls;
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

        private List<Data.Idiomas> ListaIdiomas()
        {
            List<Data.Idiomas> listadatos;
            try
            {
                IdiomasController mControl = new IdiomasController();
                long lCliID = long.Parse(hdCliID.Value.ToString());

                listadatos = mControl.GetActivos(lCliID);
                Data.Idiomas defecto = mControl.GetDefault(lCliID);
                cmbIdiomas.Value = defecto.IdiomaID;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }
        #endregion

        #region STORE PAISES

        protected void storePais_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var ls = ListaPaises();
                    if (ls != null)
                    {
                        storePais.DataSource = ls;
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

        private List<Data.Paises> ListaPaises()
        {
            List<Data.Paises> listadatos;
            try
            {
                PaisesController mControl = new PaisesController();
                long lCliID = long.Parse(hdCliID.Value.ToString());

                listadatos = mControl.GetActivos(lCliID);
                Data.Paises defecto = mControl.GetDefault(lCliID);
                cmbPais.Value = defecto.PaisID;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }
        #endregion

        #region STORE MUNICIPIOS

        protected void storeMunicipios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var ls = ListaMunicipios();
                    if (ls != null)
                    {
                        storeMunicipios.DataSource = ls;
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

        private List<Data.Municipios> ListaMunicipios()
        {
            List<Data.Municipios> listadatos;
            try
            {
                MunicipiosController mControl = new MunicipiosController();
                long lCliID = long.Parse(hdCliID.Value.ToString());

                listadatos = mControl.GetActivos(lCliID);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }
        #endregion

        #region STORE MONEDAS

        protected void storeMonedas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var ls = ListaMonedas();
                    if (ls != null)
                    {
                        storeMonedas.DataSource = ls;
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

        private List<Data.Monedas> ListaMonedas()
        {
            List<Data.Monedas> listadatos;
            try
            {
                MonedasController mControl = new MonedasController();
                long lCliID = long.Parse(hdCliID.Value.ToString());

                listadatos = mControl.GetActivos(lCliID);
                Data.Monedas defecto = mControl.GetDefault(lCliID);
                cmbMonedas.Value = defecto.MonedaID;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }
        #endregion

        #endregion

        #region DIRECT METHODS

        [DirectMethod()]
        public DirectResponse EditarUsuario()
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cUsuarios = new UsuariosController();

            try
            {
                long lS = long.Parse(hdUsuarioID.Value.ToString());

                Data.Usuarios oDato;
                oDato = cUsuarios.GetItem<Data.Usuarios>(lS);

                if (oDato != null)
                {
                    oDato.Nombre = txtNombre.Text;
                    oDato.Apellidos = txtApellidos.Text;
                    oDato.EMail = txtEMail.Text;
                    oDato.Clave = Tree.Utiles.md5.MD5String(txtClaveRepite.Text);
                }
                cUsuarios.UpdateItem(oDato);
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
        public DirectResponse MostrarEditarUsuario()
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cUsuarios = new UsuariosController();

            try
            {
                long lS = long.Parse(hdUsuarioID.Value.ToString());

                Data.Vw_Usuarios oDato;
                oDato = cUsuarios.GetItem<Data.Vw_Usuarios>(lS);

                if (oDato != null)
                {
                    txtNombre.Text = oDato.Nombre;
                    txtApellidos.Text = oDato.Apellidos;
                    txtEMail.Text = oDato.EMail;
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
        public DirectResponse EditarLenguajeIdioma()
        {
            DirectResponse direct = new DirectResponse();
            IdiomasController cIdiomas = new IdiomasController();
            PaisesController cPaises = new PaisesController();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            try
            {
                Data.Idiomas idiomaDefecto = cIdiomas.GetDefault(lCliID);
                if(idiomaDefecto.IdiomaID!= long.Parse(cmbIdiomas.Value.ToString()))
                {
                    idiomaDefecto.Defecto = false;
                    cIdiomas.UpdateItem(idiomaDefecto);

                    Data.Idiomas idioma = cIdiomas.GetItem(long.Parse(cmbIdiomas.Value.ToString()));
                    idioma.Defecto = true;
                    cIdiomas.UpdateItem(idioma);
                }

                Data.Paises paisDefecto = cPaises.GetDefault(lCliID);
                if (paisDefecto.PaisID != long.Parse(cmbPais.Value.ToString()))
                {
                    paisDefecto.Defecto = false;
                    cPaises.UpdateItem(paisDefecto);

                    Data.Paises pais = cPaises.GetItem(long.Parse(cmbPais.Value.ToString()));
                    pais.Defecto = true;
                    cPaises.UpdateItem(pais);
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
        public DirectResponse EditarEntidad()
        {
            DirectResponse direct = new DirectResponse();
            EntidadesController cEntidades = new EntidadesController();
            ClientesController cClientes = new ClientesController();
            OperadoresController cOperadores = new OperadoresController();

            try
            {
                long lS = long.Parse(hdEntidadID.Value.ToString());
                long lCliID = long.Parse(hdCliID.Value.ToString());

                Data.Entidades oDato;
                oDato = cEntidades.GetItem(lS);
                Data.Clientes cliente = cClientes.GetItem(lCliID);
                cliente.Cliente = txtNombreEntidad.Text;
                cliente.CIF = txtCodigo.Text;

                if (oDato != null)
                {
                    oDato.Nombre = txtNombreEntidad.Text;
                    oDato.Direccion = txtDireccion.Text;
                    oDato.Codigo = txtCodigo.Text;
                    oDato.MunicipioID = long.Parse(cmbMunicipio.Value.ToString());

                    if (FileImagen.HasFile)
                    {
                        
                        cliente.Imagen = (FileImagen.HasFile) ? FormatoImagenCorrecto(FileImagen) : "";
                        GuardarImagenRuta();
                    }

                    if(oDato.OperadorID != null)
                    {
                        Data.Operadores operador = cOperadores.GetItem(oDato.OperadorID.Value);
                        operador.Operador = txtNombreEntidad.Text;
                        operador.CIF = txtCodigo.Text;
                        cOperadores.UpdateItem(operador);
                    }


                }
                cClientes.UpdateItem(cliente);
                cEntidades.UpdateItem(oDato);
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
        public DirectResponse MostrarEditarEntidad()
        {
            DirectResponse direct = new DirectResponse();
            UsuariosController cUsuarios = new UsuariosController();
            EntidadesController cEntidades = new EntidadesController();

            try
            {
                long lS = long.Parse(hdUsuarioID.Value.ToString());

                long entidadID = cUsuarios.GetEntidadIDByUsuario(lS);
                hdEntidadID.SetValue(entidadID);
                Data.Entidades oDato = cEntidades.GetItem(entidadID);

                if (oDato != null)
                {
                    txtNombreEntidad.Text = oDato.Nombre;
                    txtCodigo.Text = oDato.Codigo;
                    txtDireccion.Text = oDato.Direccion;
                    storeMunicipios.Reload();
                    cmbMunicipio.SetValue(oDato.MunicipioID);
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
        public DirectResponse EditarMonedas()
        {
            DirectResponse direct = new DirectResponse();
            MonedasController cMonedas = new MonedasController();
            ClientesController cClientes = new ClientesController();
            long lCliID = long.Parse(hdCliID.Value.ToString());
            try
            {
                
                Data.Monedas monedaDefecto = cMonedas.GetDefault(lCliID);

                Data.Clientes cliente = cClientes.GetItem(lCliID);
                if (chkControlMoneda.Checked == false)
                {
                    if (monedaDefecto.MonedaID != long.Parse(cmbMonedas.Value.ToString()))
                    {
                        monedaDefecto.Defecto = false;
                        cMonedas.UpdateItem(monedaDefecto);

                        Data.Monedas moneda = cMonedas.GetItem(long.Parse(cmbMonedas.Value.ToString()));
                        moneda.Defecto = true;
                        cliente.MonedaID = moneda.MonedaID;
                        cClientes.UpdateItem(cliente);
                        cMonedas.UpdateItem(moneda);
                    }
                }
                else
                {
                    monedaDefecto.Defecto = false;
                    cMonedas.UpdateItem(monedaDefecto);
                    Data.Monedas moneda = new Data.Monedas();
                    moneda.Moneda = txtMoneda.Text;
                    moneda.Simbolo = txtSimbolo.Text;
                    moneda.CambioDollarUS = double.Parse(txtDolar.Text);
                    moneda.CambioEuro = double.Parse(txtEuro.Text);
                    moneda.Defecto = true;
                    moneda.Activo = true;
                    moneda.ClienteID = lCliID;
                    moneda=cMonedas.AddItem(moneda);
                    cliente.MonedaID = moneda.MonedaID;
                    cClientes.UpdateItem(cliente);
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

            cambiarParametro();

            return direct;
        }

        #endregion

        #region IMAGENES

        [DirectMethod()]
        public string GuardarImagenRuta()
        {

            try
            {
                if (FileImagen.HasFile)
                {

                    Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];

                    string[] extension = FileImagen.FileName.Split('.');

                    string nombreArchivo = oUsuario.ClienteID.ToString() + "." + extension[1];
                    string path = TreeCore.DirectoryMapping.GetImagenClienteDirectory();
                    string ruta = Path.Combine(path, nombreArchivo);
                    FileImagen.PostedFile.SaveAs(ruta);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }

            return FileImagen.FileName;
        }

        public string ImagenCargada()
        {
            Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
            DocumentosController cDocumentos = new DocumentosController();
            string imagenCargada = "";

            if (oUsuario.ClienteID != null)
            {

                var files = Directory.GetFiles(TreeCore.DirectoryMapping.GetImagenClienteDirectory(), oUsuario.ClienteID.ToString() + ".*");

                if (files.Length > 0)
                {
                    var tempFiles = Directory.GetFiles(TreeCore.DirectoryMapping.GetImagenClienteTempDirectory(), oUsuario.ClienteID.ToString() + ".*");
                    string extension = files[0].Split('.')[1];
                    string rutaTemp = Path.Combine(DirectoryMapping.GetImagenClienteTempDirectory(), oUsuario.ClienteID.ToString() + '.' + extension);

                    if (tempFiles.Length.Equals(0))
                    {
                        File.Copy(files[0], rutaTemp);
                    }

                    imagenCargada = "/" + Path.Combine(DirectoryMapping.GetImagenClienteTempDirectoryRelative(), oUsuario.ClienteID.ToString() + '.' + extension); ;

                }
            }
            else
            {
                imagenCargada = "";
            }

            if (imagenCargada == "")
            {
                imagenCargada = "../ima/clientes/LOGOAtrebo.svg";
            }

            return imagenCargada;
        }

        #endregion

        public string FormatoImagenCorrecto(FileUploadField file)
        {
            string nombreArchivo = file.FileName;
            return nombreArchivo;
        }

        public void cambiarParametro()
        {
            ParametrosController pController = new ParametrosController();
            Data.Parametros instalacion = pController.GetItemByName("DESPLIEGUE_INICIAL");
            instalacion.Valor = "false";
            pController.UpdateItem(instalacion);
        }
    }
}