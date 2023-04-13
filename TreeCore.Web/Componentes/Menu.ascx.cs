using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using TreeCore.Data;
using System.Reflection;
using TreeCore.Page;
using Microsoft.Extensions.Configuration;
using TreeCore.Clases;
using System.Linq;
using TreeCore.Shared.DTO.General;
using TreeCore.APIClient;
using TreeCore.Shared.DTO.Query;

namespace TreeCore.Componentes
{
    public partial class Menu : BaseUserControl
    {
        private string _NombreModulo;
        public List<long> listaFuncionalidades;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Hidden CliID = null;



        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

                if (listaFuncionalidades == null && this.Session.IsNewSession == true)
                {
                    UsuariosController cUsuarios = new UsuariosController();
                    Usuarios us = new Usuarios();
                    if (UsuariosSesiones != null)
                    {
                        us = cUsuarios.GetItem(UsuariosSesiones.UsuarioID);

                        FuncionalidadesController fConntrol = new FuncionalidadesController();

                        if (us.EMail == Comun.TREE_SUPER_USER)
                        {
                            listaFuncionalidades = fConntrol.getFuncionalidadesSuperUsuario(us.UsuarioID);
                        }
                        else
                        {
                            listaFuncionalidades = fConntrol.getFuncionalidades(us.UsuarioID);
                        }
                    }
                    else
                    {
                        us = null;
                        listaFuncionalidades = new List<long>();
                    }

                }


                //CargarMenu();
                string version = "V " + TreeCore.Properties.Settings.Default.Version;
                lblVersion.Text = version;
                lblEntorno.Text = TreeCore.Properties.Settings.Default.Nombre_Entorno;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!RequestManager.IsAjaxRequest)
            {
                CliID = (Hidden)X.GetCmp("hdCliID");
                CargarDatosCliente();
            }
        }

        private void CargarDatosCliente()
        {
            try
            {
                ClientesController cClientes = new ClientesController();

                #region Imagen Defecto
                ParametrosController cParametros = new ParametrosController();
                string pathImagen = cParametros.GetItemValor(Comun.RUTA_LOGO_CLIENTE_DEFECTO);
                string imagenClienteDefecto = pathImagen;
                #endregion


                if (CliID != null && CliID.Value != null && !CliID.Value.ToString().Equals(""))
                {
                    Clientes cliente = cClientes.GetItem(long.Parse(CliID.Value.ToString()));

                    if (cliente != null)
                    {
                        //lblNombreCliente.Text = cliente.Cliente;

                        if (!string.IsNullOrEmpty(cliente.Imagen))
                        {
                            logoCliente.ImageUrl = ImagenCargada();
                        }
                        else
                        {
                            logoCliente.Src = imagenClienteDefecto;
                        }
                    }
                    else
                    {
                        //lblNombreCliente.Text = string.Empty;
                        logoCliente.Src = imagenClienteDefecto;
                    }
                }
                else
                {
                    //lblNombreCliente.Text = string.Empty;
                    logoCliente.Src = imagenClienteDefecto;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
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
                imagenCargada = "/ima/clientes/LOGOAtrebo.png";
            }

            return imagenCargada;
        }

        public void CargarMenu()
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                Node nodoRaiz = new Node
                {
                    Text = "root",
                    Expanded = true
                };

                nodes.Add(nodoRaiz);

                nodoRaiz.Children.AddRange(ConstruirArbol());

                Tree.SetRootNode(nodoRaiz);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private NodeCollection ConstruirArbol()
        {
            MenusController cMenus = new MenusController();
            NodeCollection oNodes = new NodeCollection();

            try
            {
                oNodes = GetNodosHijos(Global.Configuration.GetSection($"Menus:{_NombreModulo}:UserInterfaces").GetChildren());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oNodes = null;
            }

            return oNodes;
        }

        private NodeCollection GetNodosHijos(IEnumerable<IConfigurationSection> listaPaginas)
        {

            DirectResponse direct = new DirectResponse();
            List<JsonObject> lMenusModulos = new List<JsonObject>();

            var Modulos = ModulesController.GetModules();
            var UserInterfaces = ModulesController.GetUserInterfaces();

            List<string> listFun = ((List<string>)(this.Session["FUNTIONALITIES"]));
            var listaInterfaces = ModulesController.GetUserInterfaces().Where(x => listFun.Where(c => c.Split('@')[1] == "Read").Select(c => c.Split('@')[0]).Contains(x.Code)).ToList();

            NodeCollection oMenu = new NodeCollection(false);
            try
            {
                foreach (var pag in listaPaginas.Where(x => (x.GetSection("UserInterface").Value == null) || listaInterfaces.Select(c => c.Code).Contains(x.GetSection("UserInterface").Value)))
                {
                    string sTraduccion = "";

                    #region Carpeta

                    if (pag.GetSection("UserInterface").Value == null)
                    {
                        foreach (var oTrad in pag.GetSection("Recurso").Get<List<string>>())
                        {
                            if (sTraduccion != "")
                                sTraduccion += " ";
                            sTraduccion += GetGlobalResource(oTrad);
                        }
                        Node oNodo = new Node
                        {
                            Text = (pag.GetSection("LocalName").Value != "") ? pag.GetSection("LocalName").Value : sTraduccion,
                            Expanded = false,
                            Expandable = true,
                            Leaf = false,
                            IconFile = "/" + Comun.rutaIconoWeb(pag.GetSection("Icono").Value)
                        };
                        oNodo.Children.AddRange(GetNodosHijos(pag.GetSection($"UserInterfaces").GetChildren()));
                        if (oNodo.Children.Count > 0)
                        {
                            oMenu.Add(oNodo);
                        }
                    }

                    #endregion

                    #region Pagina

                    else
                    {
                        var Modulo = Modulos.FirstOrDefault(x => x.UserInterfaces.Select(c => c.Code).Contains(pag.GetSection("UserInterface").Value));
                        var UserInterface = UserInterfaces.FirstOrDefault(x => x.Code == pag.GetSection("UserInterface").Value);
                        foreach (var oTrad in UserInterface.Resource)
                        {
                            if (sTraduccion != "")
                                sTraduccion += " ";
                            sTraduccion += GetGlobalResource(oTrad);
                        }
                        Node oNodo = new Node
                        {
                            Text = (pag.GetSection("LocalName").Value != "") ? pag.GetSection("LocalName").Value : sTraduccion,
                            Expanded = true,
                            Expandable = false,
                            Leaf = true,

                            NodeID = UserInterface.Page,
                            IconFile = "/" + Comun.rutaIconoWeb(UserInterface.Icon)
                        };

                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.RutaPagina, $"{Modulo.Route}{UserInterface.Page}"));
                        oNodo.CustomAttributes.Add(new ConfigItem(Comun.PARAMETROS, $"{pag.GetSection("Parametros")}"));
                        oMenu.Add(oNodo);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oMenu = null;
            }
            oMenu.Sort((x, y) => x.Text.CompareTo(y.Text));
            return oMenu;
        }

        public string NombreModulo
        {
            get { return _NombreModulo; }
            set { this._NombreModulo = value; }
        }

    }
}