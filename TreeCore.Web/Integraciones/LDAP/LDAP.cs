using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace TreeCore.Integraciones.LDAP
{
    public class LDAP
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string dominio = null;
        private string servidor = null;
        private string usuario = null;
        private string clave = null;
        private bool bConexionMultiple = false;
        Vw_ToolIntegracionesServiciosMetodosConexiones conexion = null;

        public LDAP(string sDominio)
            : base()
        {
            ToolConexionesController cConexion = new ToolConexionesController();
            int numConexiones = GetNumConexiones();

            if (numConexiones > 0)
            {
                if (numConexiones == 1)
                {
                    SetConexionMultiple(false);
                    conexion = GetConexion();
                    if (conexion != null)
                    {
                        string claveDesencriptada = DesencriptarClave(conexion.Clave);
                        SetClave(claveDesencriptada);
                        SetClave(claveDesencriptada);
                        SetServidor(conexion.Servidor);
                        SetUsuario(conexion.Usuario);
                        SetDominio(conexion.Servidor);
                    }
                }
                else
                {
                    SetConexionMultiple(true);
                    conexion = GetConexionByDominio(sDominio);
                    if (conexion != null)
                    {
                        string claveDesencriptada = DesencriptarClave(conexion.Clave);
                        SetClave(claveDesencriptada);
                        SetClave(claveDesencriptada);
                        SetServidor(conexion.Servidor);
                        SetUsuario(conexion.Usuario);
                        SetDominio(conexion.Servidor);
                    }
                }
            }


        }

        private Vw_ToolIntegracionesServiciosMetodosConexiones GetConexion()
        {
            try
            {
                Vw_ToolIntegracionesServiciosMetodosConexiones conexionLocal = null;
                if (conexion == null)
                {
                    ToolConexionesController cConexion = new ToolConexionesController();
                    conexionLocal = cConexion.GetConexionPorServicio(Comun.INTEGRACION_SERVICIO_IDENTIFICACION);
                }
                else
                {
                    conexionLocal = conexion;
                }
                return conexionLocal;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        private Vw_ToolIntegracionesServiciosMetodosConexiones GetConexionByDominio(string sDominio)
        {
            try
            {
                Vw_ToolIntegracionesServiciosMetodosConexiones conexionLocal = null;
                if (conexion == null)
                {
                    if (sDominio != null && sDominio != "")
                    {
                        sDominio = "://DC=" + sDominio;
                        ToolConexionesController cConexion = new ToolConexionesController();
                        conexionLocal = cConexion.GetConexionPorServicioYParametro(Comun.INTEGRACION_SERVICIO_IDENTIFICACION, sDominio);

                    }

                }
                else
                {
                    conexionLocal = conexion;
                }
                return conexionLocal;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        private int GetNumConexiones()
        {
            int iNumConexiones = 0;
            List<Vw_ToolIntegracionesServiciosMetodosConexiones> listaConexionesLocal = new List<Vw_ToolIntegracionesServiciosMetodosConexiones>();
            ToolConexionesController cConexion = new ToolConexionesController();
            listaConexionesLocal = cConexion.GetConexionesListaPorServicio(Comun.INTEGRACION_SERVICIO_IDENTIFICACION);

            if (listaConexionesLocal != null)
            {
                iNumConexiones = listaConexionesLocal.Count;
            }
            return iNumConexiones;
        }

        public string GetDominio()
        {
            return dominio;
        }

        public void SetDominio(string dominioLocal)
        {
            dominio = dominioLocal;
        }

        public string GetUsuario()
        {
            return usuario;
        }

        public void SetUsuario(string usuarioLocal)
        {
            usuario = usuarioLocal;
        }
        public string GetClave()
        {
            return clave;
        }
        public void SetClave(string claveLocal)
        {
            clave = claveLocal;
        }
        public string GetServidor()
        {
            return servidor;
        }

        public void SetServidor(string servidorLocal)
        {
            servidor = servidorLocal;
        }

        private string DesencriptarClave(string claveEncriptada)
        {
            string claveDesencriptada = "";
            claveDesencriptada = Util.DecryptKey(claveEncriptada);
            return claveDesencriptada;
        }

        public void SetConexionMultiple(bool bConexionMultipleLocal)
        {
            bConexionMultiple = bConexionMultipleLocal;
        }

        public bool GetConexionMultiple()
        {
            return bConexionMultiple;
        }

        #region "GRUPOS GENERAL"

        public string GetPathFromGroup(string groupName)
        {

            string sRes = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            SearchResult resultado = null;

            try
            {
                string[] aServer = null;
                string LDAP_Server, LDAP_User, LDAP_Pass, LDAP_Server2;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();

                string sAux = LDAP_Server.Substring(7);
                aServer = sAux.Split(',');
                LDAP_Server2 = "";
                int i = 0;
                foreach (string cad in aServer)
                {
                    if (i == 0)
                    {
                        LDAP_Server2 = cad.Substring(3);
                        i++;
                    }
                    else
                    {
                        LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                    }

                }

                LDAP_Server2 = "LDAP://" + LDAP_Server2;

                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(LDAP_Server2, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = "(&(objectClass=group)(CN=" + groupName + "))";

                // create results objects from search object  
                resultado = search.FindOne();

                sRes = resultado.Path;
                sRes = sRes.Substring(sRes.IndexOf("//") + 2);

                // Returns the result
                return sRes;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return sRes;
            }
        }

        public List<string> GetAllPathsFromGroup(string groupName)
        {

            List<string> lista = null;
            string sRes = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            SearchResultCollection coleccion = null;

            try
            {
                string LDAP_Server, LDAP_User, LDAP_Pass;//, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();

                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(LDAP_Server, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = "(&(objectClass=group)(CN=" + groupName + "))";

                // create results objects from search object  
                coleccion = search.FindAll();
                lista = new List<string>();

                foreach (SearchResult found in coleccion)
                {
                    sRes = found.Path;
                    sRes = sRes.Substring(sRes.IndexOf("//") + 2);
                    lista.Add(sRes);
                }

                return lista;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return lista;
            }
        }

        public List<string> GetGruposByUnidadOrganizativa(string sUnidad)
        {
            // Local variables
            List<string> lista = null;

            try
            {
                string[] aServer = null;
                string LDAP_Server, LDAP_User, LDAP_Pass, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();
                string nivelUO = "";

                aux = LDAP_Server.Substring(7);
                aServer = aux.Split(',');
                LDAP_Server2 = "";
                int i = 0;
                foreach (string cad in aServer)
                {
                    if (i == 0)
                    {
                        LDAP_Server2 = cad.Substring(3);
                        i++;
                    }
                    else
                    {
                        LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                    }
                }

                nivelUO = sUnidad;

                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);

                lista = new List<string>();
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, LDAP_Server2, nivelUO, LDAP_User, LDAP_Pass);
                GroupPrincipal qUser = new GroupPrincipal(ctx);
                PrincipalSearcher srch = new PrincipalSearcher(qUser);

                foreach (var found in srch.FindAll())
                {
                    lista.Add(found.SamAccountName);
                }

                // Returns the result
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return lista;
            }
        }

        #endregion

        #region OU

        public string GetPathFromOU(string ouName)
        {
            string sRes = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            SearchResult resultado = null;

            try
            {
                string LDAP_Server, LDAP_User, LDAP_Pass;//, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();

                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(LDAP_Server, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = "(&(objectClass=organizationalUnit)(OU=" + ouName + "))";

                // create results objects from search object  
                resultado = search.FindOne();
                sRes = resultado.Path;
                sRes = sRes.Substring(sRes.IndexOf("//") + 2);

                // Returns the result
                return sRes;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return sRes;
            }
        }

        public List<string> GetAllPathsFromOU(string ouName)
        {
            // Local variables
            List<string> lista = null;
            string sRes = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            SearchResultCollection coleccion = null;

            try
            {
                string LDAP_Server, LDAP_User, LDAP_Pass;//, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();

                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(LDAP_Server, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = "(&(objectClass=organizationalUnit)(OU=" + ouName + "))";

                // create results objects from search object  

                coleccion = search.FindAll();
                lista = new List<string>();

                foreach (SearchResult found in coleccion)
                {
                    sRes = found.Path;
                    sRes = sRes.Substring(sRes.IndexOf("//") + 2);
                    lista.Add(sRes);
                }


                // Returns the result
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return lista;
            }
        }

        #endregion

        bool PerteneceAlDirectorio(string user)
        {
            // Local variables
            bool bPertenece = false;

            // Returns the result
            return bPertenece;
        }

        #region "Arbol"

        /// <summary>
        /// Returns the root node of the LDAP
        /// </summary>
        /// <returns></returns>
        public Node GetNodoRaiz()
        {
            // Local variables
            Node raiz = null;
            string sDominio = null;
            string sNombre = null;
            string[] cadenas = null;
            int i = 0;
            string[] stringSeparators = new string[] { "DC=" };

            // Takes the information of the domain
            sDominio = GetServidor();
            sNombre = "";

            try
            {

                cadenas = sDominio.Split(stringSeparators, StringSplitOptions.None);
                for (i = 1; i < cadenas.Length; i = i + 1)
                {
                    if (i != cadenas.Length - 1)
                    {
                        sNombre = sNombre + cadenas[i].Substring(0, cadenas[i].Length - 1);
                    }
                    else
                    {
                        sNombre = sNombre + cadenas[i];
                    }
                    sNombre = sNombre + ".";
                }

                raiz = new Node();
                raiz.Text = sNombre.Substring(0, sNombre.Length - 1);
                raiz.Icon = Icon.FolderDatabase;
                raiz.NodeID = "ROOT-" + raiz.Text;

            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }

            // Returns the result
            return raiz;
        }

        /// <summary>
        /// Returns the root node of the LDAP
        /// </summary>
        /// <returns></returns>
        Node GetNodoRaizByDominio(string sDominioLocal)
        {
            // Local variables
            Node raiz = null;
            string sDominio = null;
            string sNombre = null;
            string[] cadenas = null;
            int i = 0;
            string[] stringSeparators = new string[] { "DC=" };

            // Takes the information of the domain
            sDominio = sDominioLocal;
            sNombre = "";

            try
            {

                cadenas = sDominio.Split(stringSeparators, StringSplitOptions.None);
                for (i = 1; i < cadenas.Length; i = i + 1)
                {
                    if (i != cadenas.Length - 1)
                    {
                        sNombre = sNombre + cadenas[i].Substring(0, cadenas[i].Length - 1);
                    }
                    else
                    {
                        sNombre = sNombre + cadenas[i];
                    }
                    sNombre = sNombre + ".";
                }

                raiz = new Node();
                raiz.Text = sNombre.Substring(0, sNombre.Length - 1);
                raiz.Icon = Icon.FolderDatabase;
                raiz.NodeID = "ROOT-" + raiz.Text;

            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }

            // Returns the result
            return raiz;
        }

        #region "UNIDADES ORGANIZATIVAS"

        public List<string> GetUnidadesOrganizativas(string path)
        {
            // Local variables
            List<string> resultado = null;
            string[] cadenas = null;
            int i = 0;
            string[] stringSeparators = new string[] { "OU=" };
            string sAux = null;

            // Takes out all the organizational units
            sAux = path.Substring(0, path.IndexOf("DC="));
            cadenas = sAux.Split(stringSeparators, StringSplitOptions.None);
            resultado = new List<string>();
            for (i = 1; i < cadenas.Length; i = i + 1)
            {

                resultado.Add(cadenas[i].Substring(0, cadenas[i].Length - 1));

            }

            // Returns the result
            return resultado;
        }

        /// <summary>
        /// Obtiene la lista de nodos del arbol convertida a JSON
        /// </summary>
        /// <param name="Children">Nodo a Obtener</param>
        /// <returns>Cadena en JSON con los nodos</returns>
        public string GetItemsArbolStringUnidadesOrganizativas()
        {
            // Local variables
            List<string> lista = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            SearchResultCollection coleccion = null;
            Node node = null;
            NodeCollection arbol = null;
            Node raiz = null;
            string sPath = null;
            SortedDictionary<string, Node> tabla = null;
            List<string> listaUnidades = null;
            Node nodeAdicional = null;
            Node ultimoNodo = null;
            NodeCollection nodos = null;
            int iContador = 0;
            string sResultado = null;
            int j = 0;
            Hashtable dominiosTabla = null;
            List<string> listaDominios = null;
            string sRutaParcial = null;

            try
            {
                string LDAP_Server, LDAP_User, LDAP_Pass;//, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();


                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(LDAP_Server, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = "(&(objectClass=organizationalUnit))";

                search.Sort = new SortOption("name", System.DirectoryServices.SortDirection.Descending);

                // create results objects from search object  

                coleccion = search.FindAll();
                lista = new List<string>();
                arbol = new NodeCollection();
                tabla = new SortedDictionary<string, Node>();
                dominiosTabla = new Hashtable();

                // Creates the root of the tree
                raiz = GetNodoRaiz();


                foreach (SearchResult found in coleccion)
                {
                    sPath = found.Path;
                    // Obtains the domain information
                    sRutaParcial = sPath.Substring(7);
                    sRutaParcial = sRutaParcial.Replace(",DC=", "$");
                    sRutaParcial = sRutaParcial.Replace(",OU=", "$");
                    sRutaParcial = sRutaParcial.Replace("OU=", "$");
                    sRutaParcial = sRutaParcial.Substring(1);
                    sRutaParcial = sRutaParcial.Substring(sRutaParcial.IndexOf('$'));
                    listaUnidades = GetUnidadesOrganizativas(sPath);
                    j = listaUnidades.Count - 1;
                    string sNombreUnidad = listaUnidades.ElementAt(j);

                    if (!tabla.ContainsKey(sNombreUnidad))
                    {
                        j = 0;
                        while (j < listaUnidades.Count)
                        {
                            if (j != 0)
                            {
                                sRutaParcial = sRutaParcial.Substring(1);
                                sRutaParcial = sRutaParcial.Substring(sRutaParcial.IndexOf("$"));
                            }

                            nodeAdicional = new Node();
                            nodeAdicional.Text = listaUnidades.ElementAt(j);
                            nodeAdicional.NodeID = "OU-" + listaUnidades.ElementAt(j) + sRutaParcial;
                            nodeAdicional.Icon = Icon.Folder;

                            if (ultimoNodo != null)
                            {
                                nodeAdicional.Children.Add(ultimoNodo);
                                ultimoNodo = nodeAdicional;
                            }
                            j = j + 1;
                        }
                        if (ultimoNodo != null)
                        {
                            tabla.Add(sNombreUnidad, ultimoNodo);
                            ultimoNodo = null;
                        }
                        else
                        {
                            tabla.Add(sNombreUnidad, nodeAdicional);
                            nodeAdicional = null;
                        }

                    }
                    else
                    {
                        Node nodoIntermedio = null;
                        Node nodoActual = null;
                        List<Object> listaObjetos = null;
                        int indice = -1;
                        int indiceAnterior = -1;
                        node = (Node)tabla[sNombreUnidad];
                        j = listaUnidades.Count - 1;
                        ultimoNodo = null;
                        nodoActual = node;
                        List<int> listadoIndices = new List<int>();
                        List<Node> nodosAnteriores = new List<Node>();
                        int iNodo = 0;
                        Node nodoPrevio = null;

                        while (j > 0)
                        {
                            nodos = nodoActual.Children;
                            listaObjetos = GetNodoByNombre(nodos, listaUnidades.ElementAt(j - 1));
                            if (listaObjetos != null)
                            {
                                nodoIntermedio = (Node)listaObjetos.ElementAt(0);
                                indice = Convert.ToInt32((string)listaObjetos.ElementAt(1));
                            }
                            else
                            {
                                nodoIntermedio = null;
                                indice = -1;
                            }

                            if (nodoIntermedio != null)
                            {
                                j = j - 1;
                                nodosAnteriores.Add(nodoIntermedio);
                                nodoActual = nodoIntermedio;
                                indiceAnterior = indice;
                                listadoIndices.Add(indice);

                            }
                            else
                            {
                                iContador = 0;
                                while (iContador < j)
                                {
                                    if (iContador != 0)
                                    {
                                        sRutaParcial = sRutaParcial.Substring(1);
                                        sRutaParcial = sRutaParcial.Substring(sRutaParcial.IndexOf("$"));
                                    }
                                    nodeAdicional = new Node();
                                    nodeAdicional.Text = listaUnidades.ElementAt(iContador);
                                    nodeAdicional.NodeID = "OU-" + listaUnidades.ElementAt(iContador) + sRutaParcial;
                                    nodeAdicional.Icon = Icon.Folder;
                                    if (ultimoNodo != null)
                                    {
                                        nodeAdicional.Children.Add(ultimoNodo);
                                        ultimoNodo = nodeAdicional;
                                        iContador = iContador + 1;
                                    }
                                    else
                                    {
                                        ultimoNodo = nodeAdicional;
                                        iContador = iContador + 1;
                                    }
                                }
                                if (ultimoNodo != null)
                                {
                                    nodoActual.Children.Add(ultimoNodo);
                                    ultimoNodo = null;
                                    int k = listadoIndices.Count;
                                    while (k > 0)
                                    {
                                        iNodo = listadoIndices.ElementAt(k - 1);
                                        if (k - 1 > 0)
                                        {
                                            nodoPrevio = nodosAnteriores.ElementAt(k - 2);
                                        }
                                        else
                                        {
                                            nodoPrevio = node;
                                        }
                                        nodoPrevio.Children[iNodo] = nodoActual;
                                        nodoActual = nodoPrevio;
                                        k = k - 1;

                                    }
                                }
                                else
                                {
                                    nodoActual.Children.Add(nodeAdicional);
                                    nodeAdicional = null;
                                    int k = listadoIndices.Count;
                                    while (k > 0)
                                    {
                                        iNodo = listadoIndices.ElementAt(k - 1);
                                        if (k - 1 > 0)
                                        {
                                            nodoPrevio = nodosAnteriores.ElementAt(k - 2);
                                        }
                                        else
                                        {
                                            nodoPrevio = node;
                                        }
                                        nodoPrevio.Children[iNodo] = nodoActual;
                                        nodoActual = nodoPrevio;
                                        k = k - 1;

                                    }
                                }
                                tabla[sNombreUnidad] = nodoActual;
                                j = -1;
                            }

                        }
                    }



                }

                // The hashtable has been update.
                j = 0;
                //bool bContinua = true;
                var ordena = tabla.Keys.ToList();
                ordena.Sort();
                foreach (var clave in ordena)
                {
                    raiz.Children.Add((Node)tabla[clave]);
                }

                // Obtains the domain information
                // Takes the domain information
                Node dominioAdicional = null;
                listaDominios = GetDominios();
                if (listaDominios != null)
                {
                    listaDominios.Sort();
                    foreach (string rutaDominio in listaDominios)
                    {
                        dominioAdicional = GetItemsArbolStringUnidadesOrganizativasByDominio(rutaDominio);
                        if (dominioAdicional != null)
                        {

                            dominioAdicional.Text = dominioAdicional.Text.Replace('$', '.');
                            raiz.Children.Add(dominioAdicional);
                        }
                    }
                }
                arbol.Add(raiz);

                // Returns the result
                sResultado = arbol.ToJson();
                tabla = null;
                nodeAdicional = null;
                node = null;
                ultimoNodo = null;
                return sResultado;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Obtiene la lista de nodos del arbol convertida a JSON
        /// </summary>
        /// <param name="Children">Nodo a Obtener</param>
        /// <returns>Cadena en JSON con los nodos</returns>
        public Node GetItemsArbolStringUnidadesOrganizativasByDominio(string sDominio)
        {
            // Local variables
            List<string> lista = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            SearchResultCollection coleccion = null;
            Node node = null;
            NodeCollection arbol = null;
            Node raiz = null;
            string sPath = null;
            SortedDictionary<string, Node> tabla = null;
            List<string> listaUnidades = null;
            Node nodeAdicional = null;
            Node ultimoNodo = null;
            NodeCollection nodos = null;
            int iContador = 0;
            //string sResultado = null;
            int j = 0;
            Hashtable dominiosTabla = null;
            List<string> listaDominios = null;
            string sRutaParcial = null;
            //Tree.Utiles.Logs debug = new Tree.Utiles.Logs(Sites.Properties.Settings.Default.DirectorioLogs, "LDAP", false, true);

            try
            {
                //string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass;//, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();


                // Creates the connection to the ldap server
                string sNombreDominio = sDominio.Replace('$', '.');
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(sNombreDominio, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = "(&(objectClass=organizationalUnit))";
                search.Sort = new SortOption("name", System.DirectoryServices.SortDirection.Descending);
                // create results objects from search object  

                coleccion = search.FindAll();
                lista = new List<string>();
                arbol = new NodeCollection();
                tabla = new SortedDictionary<string, Node>();
                dominiosTabla = new Hashtable();

                // Creates the root of the tree
                raiz = GetNodoRaizByDominio(sDominio);

                foreach (SearchResult found in coleccion)
                {
                    sPath = found.Path;
                    // Obtains the domain information
                    sRutaParcial = sPath.Substring(7);
                    sRutaParcial = sRutaParcial.Replace(",DC=", "$");
                    sRutaParcial = sRutaParcial.Replace(",OU=", "$");
                    sRutaParcial = sRutaParcial.Replace("OU=", "$");
                    sRutaParcial = sRutaParcial.Substring(1);
                    sRutaParcial = sRutaParcial.Substring(sRutaParcial.IndexOf('$'));

                    listaUnidades = GetUnidadesOrganizativas(sPath);
                    j = listaUnidades.Count - 1;
                    string sNombreUnidad = listaUnidades.ElementAt(j);
                    if (!tabla.ContainsKey(sNombreUnidad))
                    {
                        j = 0;
                        while (j < listaUnidades.Count)
                        {
                            if (j != 0)
                            {
                                sRutaParcial = sRutaParcial.Substring(1);
                                sRutaParcial = sRutaParcial.Substring(sRutaParcial.IndexOf("$"));
                            }
                            nodeAdicional = new Node();
                            nodeAdicional.Text = listaUnidades.ElementAt(j);
                            nodeAdicional.NodeID = "OU-" + listaUnidades.ElementAt(j) + sRutaParcial;
                            nodeAdicional.Icon = Icon.Folder;
                            if (ultimoNodo != null)
                            {
                                nodeAdicional.Children.Add(ultimoNodo);
                                ultimoNodo = nodeAdicional;
                            }
                            j = j + 1;
                        }
                        if (ultimoNodo != null)
                        {
                            tabla.Add(sNombreUnidad, ultimoNodo);
                            ultimoNodo = null;
                        }
                        else
                        {
                            tabla.Add(sNombreUnidad, nodeAdicional);
                            nodeAdicional = null;
                        }
                    }
                    else
                    {
                        Node nodoIntermedio = null;
                        Node nodoActual = null;
                        List<Object> listaObjetos = null;
                        int indice = -1;
                        int indiceAnterior = -1;
                        node = (Node)tabla[sNombreUnidad];
                        j = listaUnidades.Count - 1;
                        ultimoNodo = null;
                        nodoActual = node;
                        List<int> listadoIndices = new List<int>();
                        List<Node> nodosAnteriores = new List<Node>();
                        int iNodo = 0;
                        Node nodoPrevio = null;

                        while (j > 0)
                        {
                            nodos = nodoActual.Children;
                            listaObjetos = GetNodoByNombre(nodos, listaUnidades.ElementAt(j - 1));
                            if (listaObjetos != null)
                            {
                                nodoIntermedio = (Node)listaObjetos.ElementAt(0);
                                indice = Convert.ToInt32((string)listaObjetos.ElementAt(1));
                            }
                            else
                            {
                                nodoIntermedio = null;
                                indice = -1;
                            }


                            if (nodoIntermedio != null)
                            {
                                j = j - 1;
                                nodosAnteriores.Add(nodoIntermedio);
                                nodoActual = nodoIntermedio;
                                indiceAnterior = indice;
                                listadoIndices.Add(indice);

                            }
                            else
                            {
                                iContador = 0;
                                while (iContador < j)
                                {
                                    if (iContador != 0)
                                    {
                                        sRutaParcial = sRutaParcial.Substring(1);
                                        sRutaParcial = sRutaParcial.Substring(sRutaParcial.IndexOf("$"));
                                    }
                                    nodeAdicional = new Node();
                                    nodeAdicional.Text = listaUnidades.ElementAt(iContador);
                                    nodeAdicional.NodeID = "OU-" + listaUnidades.ElementAt(iContador) + sRutaParcial;
                                    nodeAdicional.Icon = Icon.Folder;
                                    if (ultimoNodo != null)
                                    {
                                        nodeAdicional.Children.Add(ultimoNodo);
                                        ultimoNodo = nodeAdicional;
                                        iContador = iContador + 1;
                                    }
                                    else
                                    {
                                        ultimoNodo = nodeAdicional;
                                        iContador = iContador + 1;
                                    }
                                }
                                if (ultimoNodo != null)
                                {
                                    nodoActual.Children.Add(ultimoNodo);
                                    ultimoNodo = null;
                                    int k = listadoIndices.Count;
                                    while (k > 0)
                                    {
                                        iNodo = listadoIndices.ElementAt(k - 1);
                                        if (k - 1 > 0)
                                        {
                                            nodoPrevio = nodosAnteriores.ElementAt(k - 2);
                                        }
                                        else
                                        {
                                            nodoPrevio = node;
                                        }

                                        nodoPrevio.Children[iNodo] = nodoActual;
                                        nodoActual = nodoPrevio;
                                        k = k - 1;

                                    }
                                }
                                else
                                {
                                    nodoActual.Children.Add(nodeAdicional);
                                    nodeAdicional = null;
                                    int k = listadoIndices.Count;
                                    while (k > 0)
                                    {
                                        iNodo = listadoIndices.ElementAt(k - 1);
                                        if (k - 1 > 0)
                                        {
                                            nodoPrevio = nodosAnteriores.ElementAt(k - 2);
                                        }
                                        else
                                        {
                                            nodoPrevio = node;
                                        }
                                        nodoPrevio.Children[iNodo] = nodoActual;
                                        nodoActual = nodoPrevio;
                                        k = k - 1;

                                    }
                                }
                                tabla[sNombreUnidad] = nodoActual;
                                j = -1;
                            }

                        }
                    }



                }
                // The hashtable has been update.
                j = 0;
                //bool bContinua = true;
                var ordena = tabla.Keys.ToList();
                ordena.Sort();
                foreach (var clave in ordena)
                {
                    raiz.Children.Add((Node)tabla[clave]);
                }

                // Creates the looping condition
                listaDominios = GetDominiosBySubdominio(sDominio);
                Node subNodo = null;
                if (listaDominios != null)
                {
                    listaDominios.Sort();
                    foreach (string subDominio in listaDominios)
                    {
                        subNodo = GetItemsArbolStringUnidadesOrganizativasByDominio(subDominio);
                        if (subNodo != null)
                        {
                            raiz.Children.Add(subNodo);
                        }
                    }
                }


                // Returns the result            
                tabla = null;
                nodeAdicional = null;
                node = null;
                ultimoNodo = null;
                return raiz;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return null;
            }
        }

        public List<Object> GetNodoByNombre(NodeCollection coleccionLocal, string sNombre)
        {
            //Local variables
            Node nodo = null;
            List<Object> lista = null;
            int i = 0;
            bool bControl = true;
            int iRes = 0;

            // Find initialy the node
            while (bControl && i < coleccionLocal.Count)
            {
                if (coleccionLocal.ElementAt(i).Text.Equals(sNombre))
                {
                    nodo = (Node)coleccionLocal.ElementAt(i);
                    iRes = i;
                    bControl = false;
                }
                i = i + 1;
            }

            if (nodo != null)
            {
                lista = new List<object>();
                lista.Add(nodo);
                lista.Add(iRes.ToString());
            }

            // Returns the result
            return lista;
        }
        #endregion

        #region "USUARIOS"

        public List<string> GetUsuariosByUnidadOrganizativa(string sUnidad)
        {
            // Local variables
            List<string> lista = null;

            try
            {
                string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();
                string nivelUO = "";

                aux = LDAP_Server.Substring(7);
                aServer = aux.Split(',');
                LDAP_Server2 = "";
                int i = 0;
                foreach (string cad in aServer)
                {
                    if (i == 0)
                    {
                        LDAP_Server2 = cad.Substring(3);
                        i++;
                    }
                    else
                    {
                        LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                    }
                }

                nivelUO = sUnidad;

                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);

                lista = new List<string>();

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, LDAP_Server2, nivelUO, LDAP_User, LDAP_Pass);
                UserPrincipal qUser = new UserPrincipal(ctx);
                PrincipalSearcher srch = new PrincipalSearcher(qUser);

                foreach (var found in srch.FindAll())
                {

                    lista.Add(found.SamAccountName);
                }


                // Returns the result
                return lista;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return lista;
            }
        }


        public List<string> GetUsuariosEmailsByUnidadOrganizativa(string sUnidad)
        {
            // Local variables
            List<string> lista = null;

            //int res = -1;
            //int? auxres = 0;
            //string resul = "";
            //int j = 0;
            try
            {
                string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();
                string nivelUO = "";

                aux = LDAP_Server.Substring(7);
                aServer = aux.Split(',');
                LDAP_Server2 = "";
                int i = 0;
                foreach (string cad in aServer)
                {
                    if (i == 0)
                    {
                        LDAP_Server2 = cad.Substring(3);
                        i++;
                    }
                    else
                    {
                        LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                    }


                }

                nivelUO = sUnidad;

                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);

                lista = new List<string>();

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, LDAP_Server2, nivelUO, LDAP_User, LDAP_Pass);
                UserPrincipal qUser = new UserPrincipal(ctx);

                PrincipalSearcher srch = new PrincipalSearcher(qUser);

                DirectoryEntry de = null;
                foreach (Principal found in srch.FindAll())
                {
                    de = (DirectoryEntry)found.GetUnderlyingObject();
                    if (de.Properties["mail"] != null && de.Properties["mail"].Value != null && !de.Properties["mail"].Value.ToString().Equals(""))
                    {
                        lista.Add(de.Properties["mail"].Value.ToString());
                    }
                }


                // Returns the result
                return lista;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return lista;
            }
        }



        public List<List<string>> GetUsuariosCompletosByUnidadOrganizativa(string sUnidad)
        {
            // Local variables
            List<string> lista = null;
            List<List<string>> listaFinal = null;

            //int res = -1;
            //int? auxres = 0;
            //string resul = "";
            //int j = 0;
            try
            {
                string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();
                string nivelUO = "";

                aux = LDAP_Server.Substring(7);
                aServer = aux.Split(',');
                LDAP_Server2 = "";
                int i = 0;
                foreach (string cad in aServer)
                {
                    if (i == 0)
                    {
                        LDAP_Server2 = cad.Substring(3);
                        i++;
                    }
                    else
                    {
                        LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                    }


                }

                nivelUO = sUnidad;

                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);

                lista = new List<string>();

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, LDAP_Server2, nivelUO, LDAP_User, LDAP_Pass);
                UserPrincipal qUser = new UserPrincipal(ctx);

                PrincipalSearcher srch = new PrincipalSearcher(qUser);

                listaFinal = new List<List<string>>();

                DirectoryEntry de = null;
                foreach (Principal found in srch.FindAll())
                {
                    lista = new List<string>();
                    de = (DirectoryEntry)found.GetUnderlyingObject();
                    if (de.Properties["mail"] != null && de.Properties["mail"].Value != null && !de.Properties["mail"].Value.ToString().Equals(""))
                    {
                        lista.Add(de.Properties["mail"].Value.ToString());
                        lista.Add(de.Properties["sAMAccountName"].Value.ToString());
                    }
                    listaFinal.Add(lista);
                }


                // Returns the result
                return listaFinal;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return listaFinal;
            }
        }


        public List<string> GetUsuariosByGrupo(string sGrupo)
        {
            // Local variables
            List<string> lista = null;
            //int res = -1;
            //int? auxres = 0;
            //string resul = "";
            //int j = 0;
            try
            {
                string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();
                string nivelUO = "";


                aux = LDAP_Server.Substring(7);
                aServer = aux.Split(',');
                LDAP_Server2 = "";
                int i = 0;
                foreach (string cad in aServer)
                {
                    if (i == 0)
                    {
                        LDAP_Server2 = cad.Substring(3);
                        i++;
                    }
                    else
                    {
                        LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                    }


                }
                string sGrupoInicial = sGrupo.Substring(0, sGrupo.IndexOf(','));
                string sNombreGrupo = sGrupoInicial.Substring(3);
                sGrupo = sGrupo.Substring(sGrupo.LastIndexOf("CN="), sGrupo.Length - sGrupo.LastIndexOf("CN="));
                sGrupo = sGrupo.Substring(sGrupo.IndexOf(',') + 1, sGrupo.Length - sGrupo.IndexOf(',') - 1);
                nivelUO = sGrupo;

                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);

                lista = new List<string>();

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, LDAP_Server2, nivelUO, LDAP_User, LDAP_Pass);


                // get the group you're interested in
                GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, sNombreGrupo);

                // iterate over its members
                foreach (Principal p in group.Members)
                {
                    lista.Add(p.SamAccountName);

                }
                return lista;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return lista;
            }
        }

        /// <summary>
        /// Obtiene la lista de nodos del arbol convertida a JSON
        /// </summary>
        /// <param name="Children">Nodo a Obtener</param>
        /// <returns>Cadena en JSON con los nodos</returns>
        public string GetItemsArbolStringUsuarios()
        {
            // Local variables
            List<string> lista = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            SearchResultCollection coleccion = null;
            Node node = null;
            NodeCollection arbol = null;
            Node raiz = null;
            string sPath = null;
            Hashtable tabla = null;
            List<string> listaUnidades = null;
            Node nodeAdicional = null;
            Node ultimoNodo = null;
            NodeCollection nodos = null;
            int iContador = 0;
            string sResultado = null;
            int j = 0;
            List<string> listaUsuarios = null;
            string sRutaOU = null;
            Node nodoUsuario = null;

            try
            {
                //string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass;//, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();

                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(LDAP_Server, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = "(&(objectClass=organizationalUnit))";

                // create results objects from search object  

                coleccion = search.FindAll();
                lista = new List<string>();
                arbol = new NodeCollection();
                tabla = new Hashtable();

                // Creates the root of the tree
                raiz = GetNodoRaiz();
                //tabla.Add(raiz.Text, raiz);

                foreach (SearchResult found in coleccion)
                {
                    //node = new Node(found.Properties["name"][0].ToString());
                    sPath = found.Path;
                    listaUnidades = GetUnidadesOrganizativas(sPath);
                    j = listaUnidades.Count - 1;
                    string sNombreUnidad = listaUnidades.ElementAt(j);
                    sRutaOU = sPath.Substring(7);
                    listaUsuarios = GetUsuariosByUnidadOrganizativa(sRutaOU);

                    if (!tabla.ContainsKey(sNombreUnidad))
                    {
                        j = 0;
                        while (j < listaUnidades.Count)
                        {
                            nodeAdicional = new Node();
                            nodeAdicional.Icon = Icon.Folder;
                            if (j == 0)
                            {
                                if (listaUsuarios != null && listaUsuarios.Count > 0)
                                {
                                    foreach (string sUsuarioLocal in listaUsuarios)
                                    {
                                        nodoUsuario = new Node();
                                        nodoUsuario.Text = sUsuarioLocal;
                                        nodoUsuario.Icon = Icon.User;
                                        nodoUsuario.NodeID = "USUARIO-" + sUsuarioLocal;
                                        nodeAdicional.Children.Add(nodoUsuario);
                                    }
                                }
                            }
                            if (ultimoNodo != null)
                            {
                                nodeAdicional.Children.Add(ultimoNodo);
                                ultimoNodo = nodeAdicional;
                            }
                            j = j + 1;
                        }
                        if (ultimoNodo != null)
                        {
                            // Adds the users for the given ou.

                            tabla.Add(found.Properties["name"][0].ToString(), ultimoNodo);
                        }
                        else
                        {
                            tabla.Add(found.Properties["name"][0].ToString(), nodeAdicional);
                        }
                    }
                    else
                    {
                        Node nodoIntermedio = null;
                        node = (Node)tabla[sNombreUnidad];
                        j = listaUnidades.Count - 1;
                        ultimoNodo = null;
                        while (j > 0)
                        {
                            nodos = node.Children;
                            //nodoIntermedio = GetNodoByNombre(nodos, listaUnidades.ElementAt(j - 1));
                            if (nodoIntermedio != null)
                            {
                                node = nodoIntermedio;
                                j = j - 1;
                            }
                            else
                            {
                                iContador = 0;
                                while (iContador < j)
                                {
                                    nodeAdicional = new Node();
                                    nodeAdicional.Text = listaUnidades.ElementAt(iContador);
                                    nodeAdicional.Icon = Icon.Folder;
                                    if (iContador == 0)
                                    {
                                        if (listaUsuarios != null && listaUsuarios.Count > 0)
                                        {
                                            foreach (string sUsuarioLocal in listaUsuarios)
                                            {
                                                nodoUsuario = new Node();
                                                nodoUsuario.Text = sUsuarioLocal;
                                                nodoUsuario.Icon = Icon.User;
                                                nodoUsuario.NodeID = "USUARIO-" + sUsuarioLocal;
                                                nodeAdicional.Children.Add(nodoUsuario);
                                            }
                                        }
                                    }
                                    if (ultimoNodo != null)
                                    {
                                        nodeAdicional.Children.Add(ultimoNodo);
                                        ultimoNodo = nodeAdicional;
                                        iContador = iContador + 1;
                                    }
                                    else
                                    {
                                        ultimoNodo = nodeAdicional;
                                        iContador = iContador + 1;
                                    }
                                }
                                if (ultimoNodo != null)
                                {
                                    node.Children.Add(ultimoNodo);
                                }
                                else
                                {
                                    node.Children.Add(nodeAdicional);
                                }
                                tabla[sNombreUnidad] = node;
                                j = -1;
                            }

                        }
                    }



                }
                // The hashtable has been update.
                j = 0;
                //bool bContinua = true;
                foreach (DictionaryEntry elementos in tabla)
                {
                    raiz.Children.Add((Node)tabla[elementos.Key]);
                }
                arbol.Add(raiz);

                // Returns the result
                sResultado = arbol.ToJson();
                tabla = null;
                nodeAdicional = null;
                node = null;
                ultimoNodo = null;
                return sResultado;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return null;
            }
        }


        private string LDAPEscape(string s)
        {
            var replacements = new Dictionary<string, string>() { {@"\", @"\5C"},
                                                          {@"*", @"\2A"},
                                                          {@"(", @"\2B"},
                                                          {@")", @"\29"},
                                                          {@"/", @"\3C"}};
            string ret = s;

            //escape the chars that need to be escaped
            foreach (var pair in replacements)
            {
                ret = ret.Replace(pair.Key, pair.Value);
            }

            var whiteSpaceEscapeChars = @"\";
            //escape leading white space
            int whiteSpaceCount = 0;
            while (whiteSpaceCount < ret.Count() && Char.IsWhiteSpace(ret[whiteSpaceCount]))
            {
                ret = String.Format("{0}{1}{2}", ret.Substring(0, whiteSpaceCount), whiteSpaceEscapeChars,
                    ret.Substring(whiteSpaceCount));
                whiteSpaceCount += 1 + whiteSpaceEscapeChars.Length;
            }

            //escape trailing whitespace
            if (whiteSpaceCount < ret.Count())
            {
                whiteSpaceCount = ret.Count() - 1;
                while (whiteSpaceCount >= 0 && Char.IsWhiteSpace(ret[whiteSpaceCount]))
                {
                    ret = String.Format("{0}{1}{2}", ret.Substring(0, whiteSpaceCount), whiteSpaceEscapeChars,
                        ret.Substring(whiteSpaceCount));
                    whiteSpaceCount--;
                }
            }

            return ret;
        }

        public List<Object> BuscarUsuarios(string busqueda, string sOrden)
        {
            busqueda = LDAPEscape(busqueda);
            // Local variables
            List<Object> lista = null;
            string LDAP_Pass = null;
            string LDAP_Server = null;
            string LDAP_User = null;
            //UsuarioLDAP userLDAP = null;
            string LDAP_Server2 = "";
            string sAux = null;
            string[] aServer = null;
            int iContador = 0;
            Object objeto = null;
            SortedDictionary<string, object> listaUsuarios = null;

            // Makes the connection to the LDAP
            try
            {
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();
                lista = new List<Object>();

                sAux = LDAP_Server.Substring(7);
                aServer = sAux.Split(',');
                LDAP_Server2 = "";
                int i = 0;
                foreach (string cad in aServer)
                {
                    if (i == 0)
                    {
                        LDAP_Server2 = cad.Substring(3);
                        i++;
                    }
                    else
                    {
                        LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                    }


                }


                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);


                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, LDAP_Server2, LDAP_User, LDAP_Pass);
                UserPrincipal qUser = new UserPrincipal(ctx);
                qUser.SamAccountName = ("*" + busqueda + "*").Replace("**", "*");
                listaUsuarios = new SortedDictionary<string, object>();


                PrincipalSearcher srch = new PrincipalSearcher(qUser);

                foreach (var found in srch.FindAll())
                {
                    iContador = iContador + 1;
                    objeto = new { UsuarioLDAPID = found.SamAccountName, Usuario = found.SamAccountName, Path = found.DistinguishedName };
                    //lista.Add(objeto);
                    listaUsuarios.Add(found.DistinguishedName, objeto);
                }

                qUser = new UserPrincipal(ctx);
                qUser.Name = ("*" + busqueda + "*").Replace("**", "*");
                srch = new PrincipalSearcher(qUser);
                foreach (var found in srch.FindAll())
                {
                    iContador = iContador + 1;
                    objeto = new { UsuarioLDAPID = found.SamAccountName, Usuario = found.SamAccountName, Path = found.DistinguishedName };
                    //lista.Add(objeto);
                    if (!listaUsuarios.ContainsKey(found.DistinguishedName))
                        listaUsuarios.Add(found.DistinguishedName, objeto);
                }

                if (sOrden.Equals("DESC"))
                {
                    string sClaveDescendente = null;
                    for (int k = 0; k < listaUsuarios.Keys.Count; k = k + 1)
                    {
                        sClaveDescendente = listaUsuarios.Keys.ElementAt(listaUsuarios.Keys.Count - k - 1);
                        lista.Add(listaUsuarios[sClaveDescendente]);
                    }
                }
                else
                {
                    foreach (string sClave in listaUsuarios.Keys)
                    {
                        lista.Add(listaUsuarios[sClave]);
                    }
                }

                return lista;
            }
            catch (Exception e)
            {
                // Returns the result
                log.Error(e.Message);
                return lista;
            }

        }

        public List<string> GetUsuariosByEmail(string sEmail)
        {
            // Local variables
            List<string> lista = null;
            string sRes = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            //SearchResult resultado = null;
            SearchResultCollection coleccion = null;
            //ResultPropertyCollection propiedades = null;



            //int res = -1;
            //int? auxres = 0;
            //string resul = "";
            //int j = 0;
            try
            {
                //string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass;//, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();

                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(LDAP_Server, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = string.Format("(&(mail={0}))", sEmail);

                // create results objects from search object  

                coleccion = search.FindAll();
                lista = new List<string>();

                foreach (SearchResult found in coleccion)
                {
                    sRes = found.Properties["sAMAccountName"][0].ToString();
                    lista.Add(sRes);
                }

                // Returns the result
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return lista;
            }
        }

        #endregion

        #region "GRUPOS"

        public class NodeData : IComparable
        {
            public string NodeName { get; set; }
            public string NodeID { get; set; }
            public string NodeType { get; set; }

            public List<NodeData> ChildChildren = new List<NodeData>();

            #region Miembros de IComparable

            public int CompareTo(object obj)
            {
                return NodeName.CompareTo(((NodeData)obj).NodeName);
            }

            public override string ToString()
            {
                return NodeType + "_" + NodeName;
            }

            #endregion
        }

        public string GetItemsArbolStringGrupos_v2()
        {
            NodeData NodeData = GetItemsArbolStringGrupos_v2Data();

            string rez = GetItemsArbolStringGrupos_v2Children(NodeData);
            return rez;
        }

        public string GetItemsArbolStringGrupos_v2Children(NodeData NodeData)
        {
            NodeCollection tnc = new NodeCollection();
            Node Node = new Node();
            tnc.Add(Node);
            GetItemsArbolStringGrupos_v2AddChildren(Node, NodeData.ChildChildren[0]);

            return tnc.ToJson();
        }

        public void GetItemsArbolStringGrupos_v2AddChildren(Node Node, NodeData NodeData)
        {
            Node.Text = NodeData.NodeName;
            Node.NodeID = NodeData.NodeID;
            switch (NodeData.NodeType)
            {
                case "Group":
                    Node.Icon = Icon.Group;
                    break;
                case "Folder":
                    Node.Icon = Icon.Folder;
                    break;
            }
            NodeData.ChildChildren.Sort();
            foreach (NodeData NodeDataChild in NodeData.ChildChildren)
            {
                Node NodeChild = new Node();
                Node.Children.Add(NodeChild);
                GetItemsArbolStringGrupos_v2AddChildren(NodeChild, NodeDataChild);
            }
        }

        public List<string> GetItemsArbolStringGruposUsarios_v2(string sGrupo)
        {
            string[] aServer = null;
            string LDAP_Server, LDAP_User, LDAP_Pass, LDAP_Server2, aux;
            LDAP_Pass = GetClave();
            LDAP_Server = GetServidor();
            LDAP_User = GetUsuario();

            aux = LDAP_Server.Substring(7);
            aServer = aux.Split(',');
            LDAP_Server2 = "";
            int i = 0;
            foreach (string cad in aServer)
            {
                if (i == 0)
                {
                    LDAP_Server2 = cad.Substring(3);
                    i++;
                }
                else
                {
                    LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                }
            }

            // Creates the connection to the ldap server
            LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);


            DirectoryContext DirectoryContext = new DirectoryContext(DirectoryContextType.Forest,
                LDAP_Server2, LDAP_User, LDAP_Pass);

            Forest arbol = Forest.GetForest(DirectoryContext);

            GlobalCatalog globalCatalog = arbol.FindGlobalCatalog();
            DirectorySearcher search = globalCatalog.GetDirectorySearcher();

            string query = "(&(objectCategory=person)(objectClass=user)(memberOf=" + sGrupo + "))";
            search.Filter = query;
            search.PropertiesToLoad.Add("memberOf");
            search.PropertiesToLoad.Add("name");
            //search.Sort = new SortOption("givenname", System.DirectoryServices.SortDirection.Descending);

            SearchResultCollection coleccion = search.FindAll();

            List<string> usarios = new List<string>();
            foreach (SearchResult searchresult in coleccion)
            {
                DirectoryEntry UserDirectoryEntry = searchresult.GetDirectoryEntry();
                string displayName = UserDirectoryEntry.Username;
            }

            return usarios;
        }


        public NodeData GetItemsArbolStringGrupos_v2Data()
        {
            NodeData NodeData = new NodeData();
            string[] aServer = null;
            string LDAP_Server, LDAP_User, LDAP_Pass, LDAP_Server2, aux;
            LDAP_Pass = GetClave();
            LDAP_Server = GetServidor();
            LDAP_User = GetUsuario();

            aux = LDAP_Server.Substring(7);
            aServer = aux.Split(',');
            LDAP_Server2 = "";
            int i = 0;
            foreach (string cad in aServer)
            {
                if (i == 0)
                {
                    LDAP_Server2 = cad.Substring(3);
                    i++;
                }
                else
                {
                    LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                }
            }

            // Creates the connection to the ldap server
            LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);


            DirectoryContext DirectoryContext = new DirectoryContext(DirectoryContextType.Forest,
                LDAP_Server2, LDAP_User, LDAP_Pass);

            Forest arbol = Forest.GetForest(DirectoryContext);

            GlobalCatalog globalCatalog = arbol.FindGlobalCatalog();
            DirectorySearcher search = globalCatalog.GetDirectorySearcher();

            search.Filter = @"(&(objectClass=group)(!CN=*MICROSOFT*SSEE*))";
            search.Sort = new SortOption("givenname", System.DirectoryServices.SortDirection.Descending);

            SearchResultCollection coleccion = search.FindAll();

            foreach (SearchResult searchresult in coleccion)
            {
                //Comun.Log("LDAP", "GetItemsArbolStringGrupos_v2Data", "searchresult: " + searchresult.Path, false);

                List<string> split = searchresult.Path.Split(',').ToList();
                split.Reverse();
                NodeData CurrentNodeData = NodeData;
                string NodePath = "";
                foreach (string pair in split)
                {
                    string[] keyValue = pair.Split('=');
                    if (keyValue.Length < 2)
                        continue;
                    if (keyValue[1] == "local")
                        continue;
                    NodePath += "_" + keyValue[1];
                    NodeData FoundNodeData = null;

                    foreach (NodeData tnd in CurrentNodeData.ChildChildren)
                    {
                        if (keyValue[1] == tnd.NodeName)
                        {
                            FoundNodeData = tnd;
                        }
                    }
                    if (FoundNodeData == null)
                    {
                        NodeData data = new NodeData();
                        CurrentNodeData.ChildChildren.Add(NodeData);
                        data.NodeName = keyValue[1];

                        if (split.IndexOf(pair) == split.Count - 1)
                        {
                            data.NodeType = "Group";
                            data.NodeID = "GRUPO-" + searchresult.Path.Substring(searchresult.Path.IndexOf("//") + 2);
                        }
                        else
                        {
                            data.NodeType = "Folder";
                            data.NodeID = "OU-" + NodePath;
                        }
                        CurrentNodeData = NodeData;
                    }
                    else
                    {
                        CurrentNodeData = FoundNodeData;
                    }
                }
            }


            return NodeData;

        }
        /// <summary>
        /// Obtiene la lista de grupos del arbol convertida a JSON
        /// </summary>
        /// <param name="Children">Nodo a Obtener</param>
        /// <returns>Cadena en JSON con los nodos</returns>
        public string GetItemsArbolStringGrupos()
        {
            // Local variables
            List<string> lista = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            SearchResultCollection coleccion = null;
            Node node = null;
            NodeCollection arbol = null;
            Node raiz = null;
            string sPath = null;
            SortedDictionary<string, Node> tabla = null;
            List<string> listaUnidades = null;
            Node nodeAdicional = null;
            Node ultimoNodo = null;
            NodeCollection nodos = null;
            int iContador = 0;
            string sResultado = null;
            int j = 0;
            Hashtable dominiosTabla = null;
            List<string> listaDominios = null;
            string sRutaParcial = null;
            string sRutaOU = null;
            List<string> listaGrupos = null;
            Node nodoUsuario = null;
            string sPathOU = null;



            try
            {
                string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass, LDAP_Server2;//, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                //LDAP_Server = "LDAP://Tree.local";
                LDAP_User = GetUsuario();

                string sAux = LDAP_Server.Substring(7);
                aServer = sAux.Split(',');
                LDAP_Server2 = "";
                int i = 0;
                foreach (string cad in aServer)
                {
                    if (i == 0)
                    {
                        LDAP_Server2 = cad.Substring(3);
                        i++;
                    }
                    else
                    {
                        LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                    }

                }

                LDAP_Server2 = "LDAP://" + LDAP_Server2;

                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(LDAP_Server2, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = "(&(objectClass=organizationalUnit))";

                search.Sort = new SortOption("name", System.DirectoryServices.SortDirection.Descending);

                // create results objects from search object  

                coleccion = search.FindAll();
                lista = new List<string>();
                arbol = new NodeCollection();
                tabla = new SortedDictionary<string, Node>();
                dominiosTabla = new Hashtable();

                // Creates the root of the tree
                raiz = GetNodoRaiz();


                foreach (SearchResult found in coleccion)
                {
                    sPathOU = found.Properties["distinguishedname"][0].ToString();
                    sPath = found.Path;
                    // Obtains the domain information
                    sRutaParcial = sPath.Substring(7);
                    // sRutaParcial = sRutaParcial.Substring(sRutaParcial.IndexOf('/')+1);
                    sRutaParcial = sRutaParcial.Replace(",DC=", ".");
                    sRutaParcial = sRutaParcial.Replace(",OU=", ".");
                    sRutaParcial = sRutaParcial.Replace("OU=", ".");
                    sRutaParcial = sRutaParcial.Substring(1);
                    sRutaParcial = sRutaParcial.Substring(sRutaParcial.IndexOf('.'));
                    listaUnidades = GetUnidadesOrganizativas(sPath);
                    j = listaUnidades.Count - 1;
                    string sNombreUnidad = listaUnidades.ElementAt(j);
                    sRutaOU = sPath.Substring(7);
                    listaGrupos = GetGruposByUnidadOrganizativa(sPathOU);

                    if (!tabla.ContainsKey(sNombreUnidad))
                    {
                        j = 0;
                        while (j < listaUnidades.Count)
                        {
                            nodeAdicional = new Node();
                            nodeAdicional.Text = listaUnidades.ElementAt(j);
                            nodeAdicional.NodeID = "OU-" + listaUnidades.ElementAt(j) + sRutaParcial;
                            nodeAdicional.Icon = Icon.Folder;
                            if (j == 0)
                            {
                                if (listaGrupos != null && listaGrupos.Count > 0)
                                {
                                    foreach (string sGrupoLocal in listaGrupos)
                                    {
                                        nodoUsuario = new Node();
                                        nodoUsuario.Text = sGrupoLocal;
                                        nodoUsuario.Icon = Icon.Group;
                                        nodoUsuario.NodeID = "GRUPO-" + GetPathFromGroup(sGrupoLocal);
                                        nodeAdicional.Children.Add(nodoUsuario);
                                    }
                                }
                            }
                            if (ultimoNodo != null)
                            {
                                nodeAdicional.Children.Add(ultimoNodo);
                                ultimoNodo = nodeAdicional;
                            }
                            j = j + 1;
                        }

                        if (ultimoNodo != null)
                        {
                            tabla.Add(sNombreUnidad, ultimoNodo);
                            ultimoNodo = null;
                        }
                        else
                        {
                            tabla.Add(sNombreUnidad, nodeAdicional);
                            nodeAdicional = null;
                        }

                    }
                    else
                    {
                        Node nodoIntermedio = null;
                        Node nodoActual = null;
                        List<Object> listaObjetos = null;
                        int indice = -1;
                        int indiceAnterior = -1;
                        node = (Node)tabla[sNombreUnidad];
                        j = listaUnidades.Count - 1;
                        ultimoNodo = null;
                        nodoActual = node;
                        List<int> listadoIndices = new List<int>();
                        List<Node> nodosAnteriores = new List<Node>();
                        int iNodo = 0;
                        Node nodoPrevio = null;

                        while (j > 0)
                        {
                            nodos = nodoActual.Children;
                            listaObjetos = GetNodoByNombre(nodos, listaUnidades.ElementAt(j - 1));
                            if (listaObjetos != null)
                            {
                                nodoIntermedio = (Node)listaObjetos.ElementAt(0);
                                indice = Convert.ToInt32((string)listaObjetos.ElementAt(1));
                            }
                            else
                            {
                                nodoIntermedio = null;
                                indice = -1;
                            }

                            if (nodoIntermedio != null)
                            {
                                j = j - 1;
                                nodosAnteriores.Add(nodoIntermedio);
                                nodoActual = nodoIntermedio;
                                indiceAnterior = indice;
                                listadoIndices.Add(indice);

                            }
                            else
                            {
                                iContador = 0;
                                while (iContador < j)
                                {
                                    nodeAdicional = new Node();
                                    nodeAdicional.Text = listaUnidades.ElementAt(iContador);
                                    nodeAdicional.NodeID = "OU-" + listaUnidades.ElementAt(iContador) + sRutaParcial;
                                    nodeAdicional.Icon = Icon.Folder;
                                    if (iContador == 0)
                                    {
                                        if (listaGrupos != null && listaGrupos.Count > 0)
                                        {
                                            foreach (string sGrupoLocal in listaGrupos)
                                            {
                                                nodoUsuario = new Node();
                                                nodoUsuario.Text = sGrupoLocal;
                                                nodoUsuario.Icon = Icon.Group;
                                                nodoUsuario.NodeID = "GRUPO-" + GetPathFromGroup(sGrupoLocal);
                                                nodeAdicional.Children.Add(nodoUsuario);
                                            }
                                        }
                                    }
                                    if (ultimoNodo != null)
                                    {
                                        nodeAdicional.Children.Add(ultimoNodo);
                                        ultimoNodo = nodeAdicional;
                                        iContador = iContador + 1;
                                    }
                                    else
                                    {
                                        ultimoNodo = nodeAdicional;
                                        iContador = iContador + 1;
                                    }
                                }
                                if (ultimoNodo != null)
                                {
                                    nodoActual.Children.Add(ultimoNodo);
                                    ultimoNodo = null;
                                    int k = listadoIndices.Count;
                                    while (k > 0)
                                    {
                                        iNodo = listadoIndices.ElementAt(k - 1);
                                        if (k - 1 > 0)
                                        {
                                            nodoPrevio = nodosAnteriores.ElementAt(k - 2);
                                        }
                                        else
                                        {
                                            nodoPrevio = node;
                                        }
                                        nodoPrevio.Children[iNodo] = nodoActual;
                                        nodoActual = nodoPrevio;
                                        k = k - 1;

                                    }
                                }
                                else
                                {
                                    nodoActual.Children.Add(nodeAdicional);
                                    nodeAdicional = null;
                                    int k = listadoIndices.Count;
                                    while (k > 0)
                                    {
                                        iNodo = listadoIndices.ElementAt(k - 1);
                                        if (k - 1 > 0)
                                        {
                                            nodoPrevio = nodosAnteriores.ElementAt(k - 2);
                                        }
                                        else
                                        {
                                            nodoPrevio = node;
                                        }
                                        nodoPrevio.Children[iNodo] = nodoActual;
                                        nodoActual = nodoPrevio;
                                        k = k - 1;

                                    }
                                }
                                tabla[sNombreUnidad] = nodoActual;
                                j = -1;
                            }

                        }
                    }



                }
                // The hashtable has been update.
                j = 0;
                var ordena = tabla.Keys.ToList();
                ordena.Sort();
                foreach (var clave in ordena)
                {
                    raiz.Children.Add((Node)tabla[clave]);
                }

                // Obtains the domain information
                // Takes the domain information
                Node dominioAdicional = null;
                listaDominios = GetDominios();
                if (listaDominios != null)
                {
                    listaDominios.Sort();
                    foreach (string rutaDominio in listaDominios)
                    {
                        dominioAdicional = GetItemsArbolStringGruposByDominio(rutaDominio);
                        if (dominioAdicional != null)
                        {
                            raiz.Children.Add(dominioAdicional);
                        }
                    }
                }
                arbol.Add(raiz);

                // Returns the result
                sResultado = arbol.ToJson();
                tabla = null;
                nodeAdicional = null;
                node = null;
                ultimoNodo = null;
                return sResultado;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Obtiene la lista de nodos del arbol convertida a JSON
        /// </summary>
        /// <param name="Children">Nodo a Obtener</param>
        /// <returns>Cadena en JSON con los nodos</returns>
        public Node GetItemsArbolStringGruposByDominio(string sDominio)
        {
            // Local variables
            List<string> lista = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            SearchResultCollection coleccion = null;
            Node node = null;
            NodeCollection arbol = null;
            Node raiz = null;
            string sPath = null;
            SortedDictionary<string, Node> tabla = null;
            List<string> listaUnidades = null;
            Node nodeAdicional = null;
            Node ultimoNodo = null;
            NodeCollection nodos = null;
            int iContador = 0;
            //string sResultado = null;
            int j = 0;
            Hashtable dominiosTabla = null;
            List<string> listaDominios = null;
            string sRutaParcial = null;
            string sRutaOU = null;
            List<string> listaGrupos = null;
            Node nodoUsuario = null;
            string sPathOU = null;



            try
            {
                string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass, LDAP_Server2;//, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();

                string sAux = LDAP_Server.Substring(7);
                aServer = sAux.Split(',');
                LDAP_Server2 = "";
                int i = 0;
                foreach (string cad in aServer)
                {
                    if (i == 0)
                    {
                        LDAP_Server2 = cad.Substring(3);
                        i++;
                    }
                    else
                    {
                        LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                    }

                }

                LDAP_Server2 = "LDAP://" + LDAP_Server2;


                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(sDominio, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = "(&(objectClass=organizationalUnit))";
                search.Sort = new SortOption("name", System.DirectoryServices.SortDirection.Descending);
                // create results objects from search object  

                coleccion = search.FindAll();
                lista = new List<string>();
                arbol = new NodeCollection();
                tabla = new SortedDictionary<string, Node>();
                dominiosTabla = new Hashtable();

                // Creates the root of the tree
                raiz = GetNodoRaizByDominio(sDominio);


                foreach (SearchResult found in coleccion)
                {
                    sPathOU = found.Properties["distinguishedname"][0].ToString();
                    sPath = found.Path;
                    // Obtains the domain information
                    sRutaParcial = sPath.Substring(7);
                    sRutaParcial = sRutaParcial.Replace(",DC=", ".");
                    sRutaParcial = sRutaParcial.Replace(",OU=", ".");
                    sRutaParcial = sRutaParcial.Replace("OU=", ".");
                    sRutaParcial = sRutaParcial.Substring(1);
                    sRutaParcial = sRutaParcial.Substring(sRutaParcial.IndexOf('.'));
                    sRutaOU = sPath.Substring(7);
                    listaGrupos = GetGruposByUnidadOrganizativa(sPathOU);

                    listaUnidades = GetUnidadesOrganizativas(sPath);
                    j = listaUnidades.Count - 1;
                    string sNombreUnidad = listaUnidades.ElementAt(j);
                    if (!tabla.ContainsKey(sNombreUnidad))
                    {
                        j = 0;
                        while (j < listaUnidades.Count)
                        {

                            nodeAdicional = new Node();
                            nodeAdicional.Text = listaUnidades.ElementAt(j);
                            nodeAdicional.NodeID = "OU-" + listaUnidades.ElementAt(j) + sRutaParcial;
                            nodeAdicional.Icon = Icon.Folder;
                            if (j == 0)
                            {
                                if (listaGrupos != null && listaGrupos.Count > 0)
                                {
                                    foreach (string sGrupoLocal in listaGrupos)
                                    {
                                        nodoUsuario = new Node();
                                        nodoUsuario.Text = sGrupoLocal;
                                        nodoUsuario.Icon = Icon.Group;
                                        nodoUsuario.NodeID = "GRUPO-" + GetPathFromGroup(sGrupoLocal);
                                        nodeAdicional.Children.Add(nodoUsuario);
                                    }
                                }
                            }
                            if (ultimoNodo != null)
                            {
                                nodeAdicional.Children.Add(ultimoNodo);
                                ultimoNodo = nodeAdicional;
                            }
                            j = j + 1;
                        }
                        if (ultimoNodo != null)
                        {
                            tabla.Add(sNombreUnidad, ultimoNodo);
                        }
                        else
                        {
                            tabla.Add(sNombreUnidad, nodeAdicional);
                        }
                    }
                    else
                    {
                        Node nodoIntermedio = null;
                        Node nodoActual = null;
                        List<Object> listaObjetos = null;
                        int indice = -1;
                        int indiceAnterior = -1;
                        node = (Node)tabla[sNombreUnidad];
                        j = listaUnidades.Count - 1;
                        ultimoNodo = null;
                        nodoActual = node;
                        List<int> listadoIndices = new List<int>();
                        List<Node> nodosAnteriores = new List<Node>();
                        int iNodo = 0;
                        Node nodoPrevio = null;

                        while (j > 0)
                        {
                            nodos = nodoActual.Children;
                            listaObjetos = GetNodoByNombre(nodos, listaUnidades.ElementAt(j - 1));
                            if (listaObjetos != null)
                            {
                                nodoIntermedio = (Node)listaObjetos.ElementAt(0);
                                indice = Convert.ToInt32((string)listaObjetos.ElementAt(1));
                            }
                            else
                            {
                                nodoIntermedio = null;
                                indice = -1;
                            }


                            if (nodoIntermedio != null)
                            {
                                j = j - 1;
                                nodosAnteriores.Add(nodoIntermedio);
                                nodoActual = nodoIntermedio;
                                indiceAnterior = indice;
                                listadoIndices.Add(indice);

                            }
                            else
                            {
                                iContador = 0;
                                while (iContador < j)
                                {
                                    nodeAdicional = new Node();
                                    nodeAdicional.Text = listaUnidades.ElementAt(iContador);
                                    nodeAdicional.NodeID = "OU-" + listaUnidades.ElementAt(iContador) + sRutaParcial;
                                    nodeAdicional.Icon = Icon.Folder;
                                    if (iContador == 0)
                                    {
                                        if (listaGrupos != null && listaGrupos.Count > 0)
                                        {
                                            foreach (string sGrupoLocal in listaGrupos)
                                            {
                                                nodoUsuario = new Node();
                                                nodoUsuario.Text = sGrupoLocal;
                                                nodoUsuario.Icon = Icon.Group;
                                                nodoUsuario.NodeID = "GRUPO-" + GetPathFromGroup(sGrupoLocal);
                                                nodeAdicional.Children.Add(nodoUsuario);
                                            }
                                        }
                                    }
                                    if (ultimoNodo != null)
                                    {
                                        nodeAdicional.Children.Add(ultimoNodo);
                                        ultimoNodo = nodeAdicional;
                                        iContador = iContador + 1;
                                    }
                                    else
                                    {
                                        ultimoNodo = nodeAdicional;
                                        iContador = iContador + 1;
                                    }
                                }
                                if (ultimoNodo != null)
                                {
                                    nodoActual.Children.Add(ultimoNodo);
                                    ultimoNodo = null;
                                    int k = listadoIndices.Count;
                                    while (k > 0)
                                    {
                                        iNodo = listadoIndices.ElementAt(k - 1);
                                        if (k - 1 > 0)
                                        {
                                            nodoPrevio = nodosAnteriores.ElementAt(k - 2);
                                        }
                                        else
                                        {
                                            nodoPrevio = node;
                                        }

                                        nodoPrevio.Children[iNodo] = nodoActual;
                                        nodoActual = nodoPrevio;
                                        k = k - 1;

                                    }
                                }
                                else
                                {
                                    nodoActual.Children.Add(nodeAdicional);
                                    nodeAdicional = null;
                                    int k = listadoIndices.Count;
                                    while (k > 0)
                                    {
                                        iNodo = listadoIndices.ElementAt(k - 1);
                                        if (k - 1 > 0)
                                        {
                                            nodoPrevio = nodosAnteriores.ElementAt(k - 2);
                                        }
                                        else
                                        {
                                            nodoPrevio = node;
                                        }
                                        nodoPrevio.Children[iNodo] = nodoActual;
                                        nodoActual = nodoPrevio;
                                        k = k - 1;

                                    }
                                }
                                tabla[sNombreUnidad] = node;
                                j = -1;
                            }

                        }
                    }



                }
                // The hashtable has been update.
                j = 0;
                var ordena = tabla.Keys.ToList();
                ordena.Sort();
                foreach (var clave in ordena)
                {
                    raiz.Children.Add((Node)tabla[clave]);
                }

                // Creates the looping condition
                listaDominios = GetDominiosBySubdominio(sDominio);
                Node subNodo = null;
                if (listaDominios != null)
                {
                    listaDominios.Sort();
                    foreach (string subDominio in listaDominios)
                    {
                        subNodo = GetItemsArbolStringGruposByDominio(subDominio);
                        if (subNodo != null)
                        {
                            raiz.Children.Add(subNodo);
                        }
                    }
                }


                // Returns the result            
                tabla = null;
                nodeAdicional = null;
                node = null;
                ultimoNodo = null;
                return raiz;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return null;
            }
        }


        #endregion
        #endregion

        #region "DOMINIOS"

        public string GetDominioPadre()
        {
            // Local variables
            string sRes = null;
            string LDAP_Server = null;

            // Takes the parameter information
            LDAP_Server = GetServidor();

            sRes = LDAP_Server.Substring(7);
            sRes = sRes.Replace(",DC=", "$");
            sRes = sRes.Replace("DC=", "$");
            sRes = sRes.Substring(1);


            // Returns the result
            return sRes;
        }

        public List<string> GetDominios()
        {
            // Local variables
            List<string> listaDominios = null;
            List<string> resultado = null;
            string[] aServer = null;
            string LDAP_Server, LDAP_User, LDAP_Pass, LDAP_Server2, aux;
            LDAP_Pass = GetClave();
            LDAP_Server = GetServidor();
            LDAP_User = GetUsuario();
            aux = LDAP_Server.Substring(7);
            aServer = aux.Split(',');
            LDAP_Server2 = "";
            int i = 0;
            foreach (string cad in aServer)
            {
                if (i == 0)
                {
                    LDAP_Server2 = cad.Substring(3);
                    i++;
                }
                else
                {
                    LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                }


            }

            listaDominios = new List<string>();

            LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);

            Tree.LDAP.ActiveDirectory.LDAPSearchResult res2 = new Tree.LDAP.ActiveDirectory.LDAPSearchResult();


            // Connects to the forest
            using (System.DirectoryServices.ActiveDirectory.Forest arbol = System.DirectoryServices.ActiveDirectory.Forest.GetForest(new System.DirectoryServices.ActiveDirectory.DirectoryContext(System.DirectoryServices.ActiveDirectory.DirectoryContextType.Forest, LDAP_Server2, LDAP_User, LDAP_Pass)))
            {

                foreach (System.DirectoryServices.ActiveDirectory.Domain dominio in arbol.Domains)
                {
                    if (!dominio.Name.Equals(arbol.Name))
                    {
                        if (dominio.Name.Contains("."))
                        {
                            listaDominios.Add(dominio.Name.Split('.')[0]);
                        }
                        else
                        {
                            listaDominios.Add(dominio.Name);
                        }
                        dominio.Dispose();
                    }
                }

                string sDirectoryString = null;
                resultado = new List<string>();

                if (listaDominios != null && listaDominios.Count > 0)
                {


                    foreach (string domi in listaDominios)
                    {

                        sDirectoryString = "";
                        sDirectoryString = string.Format("LDAP://DC={0}.{1}", domi, arbol.Name);
                        sDirectoryString = sDirectoryString.Replace(".", ",DC=");
                        resultado.Add(sDirectoryString);
                    }

                }

            }

            // Returns the result
            return resultado;

        }

        public List<string> GetDominiosBySubdominio(string subdominio)
        {
            // Local variables
            List<string> lista = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            SearchResultCollection coleccion = null;
            List<string> listaDominios = null;
            string sDirectoryString = "";


            try
            {
                //string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass;//, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();


                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(subdominio, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = "(&(objectClass=domainDNS))";
                search.Sort = new SortOption("name", System.DirectoryServices.SortDirection.Descending);
                // create results objects from search object  

                coleccion = search.FindAll();
                lista = new List<string>();
                listaDominios = new List<string>();
                string sCompara = subdominio.Substring(7);
                sCompara = sCompara.Replace(",DC=", "$");
                sCompara = sCompara.Replace("DC=", "$");
                sCompara = sCompara.Substring(1);
                string[] division = sCompara.Split('$');
                int divisionLongitud = division.Length;
                string[] partes = null;
                string sRuta = null;

                foreach (SearchResult found in coleccion)
                {
                    sRuta = found.Path;
                    sRuta = sRuta.Substring(7);
                    sRuta = sRuta.Replace(",DC=", "$");
                    sRuta = sRuta.Replace("DC=", "$");
                    sRuta = sRuta.Substring(1);
                    if (!sRuta.Equals(sCompara))
                    {
                        if (sRuta.Contains("$"))
                        {
                            partes = sRuta.Split('$');
                            if (partes.Length - divisionLongitud - 1 >= 0)
                            {
                                listaDominios.Add(partes[partes.Length - divisionLongitud - 1]);
                            }
                        }
                        else
                        {
                            listaDominios.Add(found.Properties["name"][0].ToString());
                        }
                    }

                }

                if (listaDominios != null && listaDominios.Count > 0)
                {


                    foreach (string domi in listaDominios)
                    {

                        sDirectoryString = "";
                        sDirectoryString = string.Format("LDAP://DC={0}.{1}", domi, subdominio.Substring(7));
                        lista.Add(sDirectoryString);
                    }

                }
                return lista;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return lista;
            }
        }

        public List<string> GetDominiosBosque()
        {
            // Local variables
            List<string> lista = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            SearchResultCollection coleccion = null;
            List<string> listaDominios = null;
            string sDirectoryString = "";


            try
            {
                //string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass;//, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();


                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(LDAP_Server, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = "(&(objectClass=domainDNS))";
                search.Sort = new SortOption("name", System.DirectoryServices.SortDirection.Descending);
                // create results objects from search object  

                coleccion = search.FindAll();
                lista = new List<string>();
                listaDominios = new List<string>();
                string sCompara = LDAP_Server.Substring(7);
                sCompara = sCompara.Replace(",DC=", "$");
                sCompara = sCompara.Replace("DC=", "$");
                sCompara = sCompara.Substring(1);
                string[] division = sCompara.Split('$');
                int divisionLongitud = division.Length;
                string[] partes = null;
                string sRuta = null;

                foreach (SearchResult found in coleccion)
                {
                    sRuta = found.Path;
                    sRuta = sRuta.Substring(7);
                    sRuta = sRuta.Replace(",DC=", "$");
                    sRuta = sRuta.Replace("DC=", "$");
                    sRuta = sRuta.Substring(1);
                    if (!sRuta.Equals(sCompara))
                    {
                        if (sRuta.Contains("$"))
                        {
                            partes = sRuta.Split('$');
                            listaDominios.Add(partes[partes.Length - divisionLongitud - 1]);
                        }
                        else
                        {
                            listaDominios.Add(found.Properties["name"][0].ToString().ToUpper());
                        }
                    }

                }

                if (listaDominios != null && listaDominios.Count > 0)
                {


                    foreach (string domi in listaDominios)
                    {

                        sDirectoryString = "";
                        sDirectoryString = string.Format("LDAP://DC={0}.{1}", domi, LDAP_Server.Substring(7));
                        lista.Add(sDirectoryString);
                    }

                }
                return lista;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return lista;
            }
        }



        #endregion

        #region ASINCRONO

        /// <summary>
        /// Obtiene la lista de nodos del arbol convertida a JSON
        /// </summary>
        /// <param name="Children">Nodo a Obtener</param>
        /// <returns>Cadena en JSON con los nodos</returns>
        public List<Node> getUnidadesByID(long unidadID)
        {

            // Local variables        
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            //SearchResultCollection coleccion = null;
            //List<string> listaDominios = null;
            //string sDirectoryString = "";
            List<Node> lista = null;
            Node nodo = null;
            string sPath = null;
            //List<string> listaUnidades = null;
            string sNombreOU = "";
            string sRutaParcial = "";

            try
            {
                //string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass;//, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();


                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(LDAP_Server, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // retrieve only cn and distinguishedname properties
                //string[] props = { "cn", "distinguishedname" };
                //src.PropertiesToLoad.AddRange(props);
                search.SearchRoot = myLdapConnection;
                search.SearchScope = SearchScope.Subtree;
                // search only object category user
                search.Filter = "(&(objectClass=organizationalUnit))";
                search.Sort = new SortOption("name", System.DirectoryServices.SortDirection.Descending);
                // use a paged search
                //src.PageSize = 500;
                SearchResultCollection res = search.FindAll();
                if (res != null)
                {
                    lista = new List<Node>();
                    foreach (SearchResult found in res)
                    {
                        sPath = found.Path;
                        sRutaParcial = sPath.Substring(7);
                        sRutaParcial = sRutaParcial.Replace(",DC=", "$");
                        sRutaParcial = sRutaParcial.Replace(",OU=", "$");
                        sRutaParcial = sRutaParcial.Replace("OU=", "$");
                        sRutaParcial = sRutaParcial.Substring(1);
                        sNombreOU = sRutaParcial.Substring(0, sRutaParcial.IndexOf("$"));
                        //sRutaParcial = sRutaParcial.Substring(sRutaParcial.IndexOf('$'));                    
                        nodo = new Node();
                        nodo.Text = sNombreOU;
                        nodo.NodeID = "OU-" + sRutaParcial;
                        nodo.Icon = Icon.Folder;
                        lista.Add(nodo);

                    }
                }
                return lista;

            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return lista;
            }
        }

        #endregion

        #region "LOGIN EXTERNO"

        public Usuarios UsuariosLoginLDAP(string Usuario, string Clave, string sDominio)
        {
            Usuarios res = null;
            string[] aServer = null;
            string LDAP_Server, LDAP_User, LDAP_Pass, LDAP_Server2, aux;
            LDAP_Pass = GetClave();
            LDAP_Server = GetServidor();
            LDAP_User = GetUsuario();

            aux = LDAP_Server.Substring(7);
            aServer = aux.Split(',');
            LDAP_Server2 = "";
            int i = 0;
            foreach (string cad in aServer)
            {
                if (i == 0)
                {
                    LDAP_Server2 = cad.Substring(3);
                    i++;
                }
                else
                {
                    LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                }


            }

            //LDAP_Server2 = "Tree.local";
            //LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);

            Tree.LDAP.ActiveDirectory.LDAPSearchResult res2 = new Tree.LDAP.ActiveDirectory.LDAPSearchResult();
            Tree.LDAP.ActiveDirectory.LDAPQuery Consulta = null;
            List<String> dominios = new List<string>();
            // using (System.DirectoryServices.ActiveDirectory.Forest arbol = System.DirectoryServices.ActiveDirectory.Forest.GetCurrentForest())

            bool bErrorForest = false;

            try
            {
                using (System.DirectoryServices.ActiveDirectory.Forest arbol = System.DirectoryServices.ActiveDirectory.Forest.GetForest(new System.DirectoryServices.ActiveDirectory.DirectoryContext(System.DirectoryServices.ActiveDirectory.DirectoryContextType.Forest, LDAP_Server2, LDAP_User, LDAP_Pass)))
                {

                    foreach (System.DirectoryServices.ActiveDirectory.Domain dominio in arbol.Domains)
                    {

                        if (dominio.Name.Contains("."))
                        {
                            dominios.Add(dominio.Name.Split('.')[0].ToUpper());
                        }
                        else
                        {
                            dominios.Add(dominio.Name.ToUpper());
                        }
                        dominio.Dispose();
                    }



                    if (dominios.Count > 1)
                    {
                        //Recorremos el bosque para comprobar si existe el usuario
                        string DirectoryString = string.Format("LDAP://{0}", arbol.Name);

                        Consulta = new Tree.LDAP.ActiveDirectory.LDAPQuery(DirectoryString, LDAP_User, LDAP_Pass);

                        res2 = Consulta.AuthenticateUserObject(Usuario, Clave);

                        if (res2 == null)
                        {
                            foreach (string domi in dominios)
                            {

                                DirectoryString = "";
                                DirectoryString = string.Format("LDAP://{0}.{1}", domi, arbol.Name);

                                Consulta = new Tree.LDAP.ActiveDirectory.LDAPQuery(DirectoryString, LDAP_User, LDAP_Pass);

                                res2 = Consulta.AuthenticateUserObject(Usuario, Clave);

                            }
                        }




                    }
                    else
                    {
                        Consulta = new Tree.LDAP.ActiveDirectory.LDAPQuery(LDAP_Server, LDAP_User, LDAP_Pass);

                        res2 = Consulta.AuthenticateUserObject(Usuario, Clave);

                    }

                }

            }
            catch (Exception ex)
            {
                res2 = null;
                bErrorForest = true;
                log.Error(ex.Message);
            }


            if (res2 != null)
            {
                res = new Usuarios();
                res.EMail = Usuario;
                res.Nombre = res2.firstName;
                res.Apellidos = res2.lastName;
                res.Telefono = res2.telephoneNumber;
                res.EMail = res2.EmailAddress;
            }
            else
            {
                if (bErrorForest)
                {
                    res = GetUsuarioFromLDAP(Usuario, Clave, sDominio);

                }
                else
                {
                    res = GetUsuarioFromLDAP(Usuario, Clave, sDominio);
                }

            }
            Consulta = null;
            return res;
        }
        #endregion


        #region "PRINTERS"

        public string GetPathFromPrinterName(string printerName)
        {

            // Local variables
            //List<string> lista = null;
            string sRes = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            SearchResult resultado = null;
            //SearchResultCollection coleccion = null;
            //ResultPropertyCollection propiedades = null;



            //int res = -1;
            //int? auxres = 0;
            //string resul = "";
            //int j = 0;
            try
            {
                //string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass;//, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();

                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(LDAP_Server, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = "(&(objectCategory=printQueue)(name=" + printerName + "))";

                // create results objects from search object  

                resultado = search.FindOne();

                sRes = resultado.Path;
                sRes = sRes.Substring(sRes.IndexOf("//") + 2);

                // Returns the result
                return sRes;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return sRes;
            }
        }


        #endregion

        #region "SERVERS"

        public string GetPathFromServerName(string serverName)
        {

            // Local variables
            //List<string> lista = null;
            string sRes = null;
            DirectoryEntry myLdapConnection = null;
            DirectorySearcher search = null;
            SearchResult resultado = null;
            //SearchResultCollection coleccion = null;
            //ResultPropertyCollection propiedades = null;



            //int res = -1;
            //int? auxres = 0;
            //string resul = "";
            //int j = 0;
            try
            {
                //string[] aServer = null;
                //string[] aOU = null;
                string LDAP_Server, LDAP_User, LDAP_Pass;//, LDAP_Server2, aux;
                LDAP_Pass = GetClave();
                LDAP_Server = GetServidor();
                LDAP_User = GetUsuario();

                // Creates the connection to the ldap server
                LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                myLdapConnection = new DirectoryEntry(LDAP_Server, LDAP_User, LDAP_Pass);
                myLdapConnection.AuthenticationType = AuthenticationTypes.Secure;

                // Once connected we create the searcher object
                search = new DirectorySearcher(myLdapConnection);

                // Prepates the filter
                search.Filter = "(&(objectCategory=computer)(name=" + serverName + "))";

                // create results objects from search object  

                resultado = search.FindOne();

                sRes = resultado.Path;
                sRes = sRes.Substring(sRes.IndexOf("//") + 2);

                // Returns the result
                return sRes;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return sRes;
            }
        }


        #endregion

        #region LOGIN

        public Usuarios Login(string user, string pass)
        {
            // Local variables
            Usuarios usuarioLocal = null;
            UsuariosController cUsuario = new UsuariosController();
            //Clientes clienteLocal = null;
            ClientesController cCliente = new ClientesController();
            Vw_ToolIntegracionesServiciosMetodosConexiones conexionLocal = null;
            //List<LDAPConexion> conexionesListaLocal = null;
            string sDominioABuscar = null;
            string LDAP_Server = null;
            string LDAP_User = null;
            string LDAP_Pass = null;
            bool bMultidominio = false;

            ParametrosController cParametros = new ParametrosController();
            Usuarios usuarioLocalMod = null;
            UsuariosController cUsuarioMod = new UsuariosController();

            usuarioLocalMod = cUsuarioMod.getUsuarioByEmail(Comun.TREE_SERVICES_USER);// Sirve para monitorear quien ha realizado los cambios en el usuario

            try
            {

                ToolConexionesController cConexion = new ToolConexionesController();
                int numConexiones = GetNumConexiones();

                if (numConexiones > 1)
                {
                    bMultidominio = true;
                    /*//Si el usuario introducido lleva la barra invertida entonces se separa el dominio por un sitio y el usuario por otro
                    if (user.Contains("\\"))
                    {
                        sDominioABuscar = user.Substring(0, user.LastIndexOf("\\"));
                        user = user.Substring(user.LastIndexOf("\\") + 1, user.Length - (user.LastIndexOf("\\") + 1));
                    }*/

                }

                //Si el usuario introducido lleva la barra invertida entonces se separa el dominio por un sitio y el usuario por otro
                if (user.Contains("\\"))
                {
                    sDominioABuscar = user.Substring(0, user.LastIndexOf("\\"));
                    user = user.Substring(user.LastIndexOf("\\") + 1, user.Length - (user.LastIndexOf("\\") + 1));
                }

                //ANTIGUO QUE FUNCIONA ANTES DE AÑADIR MULTIPLES DOMINIOS
                // Performs the connection
                if (conexion == null)
                {
                    if (bMultidominio)
                    {
                        if (sDominioABuscar != null)
                        {
                            conexionLocal = GetConexionByDominio(sDominioABuscar);

                        }
                        else
                        {
                            return usuarioLocal;
                        }

                    }
                    else
                    {
                        conexionLocal = GetConexion();
                    }

                    if (conexionLocal != null)
                    {

                        LDAP_Pass = DesencriptarClave(conexionLocal.Clave);
                        LDAP_Server = conexionLocal.Servidor;
                        LDAP_User = conexionLocal.Usuario;
                    }
                }
                else
                {
                    conexionLocal = conexion;
                    LDAP_Pass = GetClave();
                    LDAP_Server = GetServidor();
                    LDAP_User = GetUsuario();
                }

                #region MULTIPLESCONEXIONES_BORRAR
                /*if (conexion == null && conexionesLista == null)
                {
                    ToolConexionesController cConexion = new ToolConexionesController();
                    int numConexiones = GetNumConexiones();

                    if (numConexiones > 0)
                    {
                        if (numConexiones == 1)
                        {
                            conexionLocal = GetConexion();
                            LDAP_Pass = DesencriptarClave(conexionLocal.Clave);
                            LDAP_Server = conexionLocal.Servidor;
                            LDAP_User = conexionLocal.Usuario;

                        }
                        else
                        {
                            conexionesLista = GetConexionesListaVista();
                            if (conexionesLista != null && conexionesLista.Count > 0)
                            {
                                SetConexionMultiple(true);
                                List<LDAPConexion> oConexionesListaLocal = new List<LDAPConexion>();

                                foreach (Vw_ToolIntegracionesServiciosMetodosConexiones dato in conexionesLista)
                                {
                                    LDAPConexion nuevaConexion = new LDAPConexion();

                                    string claveDesencriptada2 = DesencriptarClave(dato.Clave);
                                    nuevaConexion.clave = claveDesencriptada2;
                                    nuevaConexion.servidor = dato.Servidor;
                                    nuevaConexion.usuario = dato.Usuario;

                                    oConexionesListaLocal.Add(nuevaConexion);
                                }

                                SetListaConexiones(oConexionesListaLocal);

                            }
                            LDAP_Pass = "";
                            LDAP_Server = "";
                            LDAP_User = "";
                        }
                    }
                    else
                    {
                        LDAP_Pass = "";
                        LDAP_Server = "";
                        LDAP_User = "";
                    }
                }
                else
                {
                    if (!bConexionMultiple)
                    {
                        conexionLocal = conexion;
                        LDAP_Pass = GetClave();
                        LDAP_Server = GetServidor();
                        LDAP_User = GetUsuario();

                    }
                    else
                    {
                        conexionesListaLocal = GetListaConexiones();
                        LDAP_Pass = "";
                        LDAP_Server = "";
                        LDAP_User = "";
                    }

                }
                */
                #endregion

                #region ANTIGUO
                /*aux = LDAP_Server.Substring(7);
                aServer = aux.Split(',');
                LDAP_Server2 = "";
                int i = 0;
                foreach (string cad in aServer)
                {
                    if (i == 0)
                    {
                        LDAP_Server2 = cad.Substring(3);
                        i++;
                    }
                    else
                    {
                        LDAP_Server2 = LDAP_Server2 + "." + cad.Substring(3);
                    }


                }

                //LDAP_Server2 = "Tree.local";

                //LDAP_Pass = Tree.Seguridad.Encriptar.DecryptString(LDAP_Pass, Comun.gClaveEncriptacion);
                LDAP_Pass = Util.DecryptKey(LDAP_Pass);

                Tree.LDAP.ActiveDirectory.LDAPSearchResult res2 = new Tree.LDAP.ActiveDirectory.LDAPSearchResult();
                Tree.LDAP.ActiveDirectory.LDAPQuery Consulta = null;
                List<String> dominios = new List<string>();


                // using (System.DirectoryServices.ActiveDirectory.Forest arbol = System.DirectoryServices.ActiveDirectory.Forest.GetCurrentForest())
                using (System.DirectoryServices.ActiveDirectory.Forest arbol = System.DirectoryServices.ActiveDirectory.Forest.GetForest(new System.DirectoryServices.ActiveDirectory.DirectoryContext(System.DirectoryServices.ActiveDirectory.DirectoryContextType.Forest, LDAP_Server2, LDAP_User, LDAP_Pass)))
                {

                    foreach (System.DirectoryServices.ActiveDirectory.Domain dominio in arbol.Domains)
                    {
                        if (dominio.Name.Contains(".")) { dominios.Add(dominio.Name.Split('.')[0].ToUpper()); }
                        else { dominios.Add(dominio.Name.ToUpper()); }
                        dominio.Dispose();
                    }



                    if (dominios.Count > 1)
                    {


                        //Recorremos el bosque para comprobar si existe el usuario

                        string DirectoryString = string.Format("LDAP://{0}", arbol.Name);



                        Consulta = new Tree.LDAP.ActiveDirectory.LDAPQuery(DirectoryString, LDAP_User, LDAP_Pass);
                        res2 = Consulta.AuthenticateUserObject(user, pass);



                        if (res2 == null)
                        {

                            foreach (string domi in dominios)
                            {

                                DirectoryString = "";
                                DirectoryString = string.Format("LDAP://{0}.{1}", domi, arbol.Name);

                                Consulta = new Tree.LDAP.ActiveDirectory.LDAPQuery(DirectoryString, LDAP_User, LDAP_Pass);

                                res2 = Consulta.AuthenticateUserObject(user, pass);

                                if (res2 != null)
                                {
                                    break;
                                }
                            }
                        }




                    }
                    else
                    {

                        Consulta = new Tree.LDAP.ActiveDirectory.LDAPQuery(LDAP_Server, LDAP_User, LDAP_Pass);

                        res2 = Consulta.AuthenticateUserObject(user, pass);

                    }

                }
                */
                #endregion

                if (LDAP_Server != null && LDAP_Pass != null && LDAP_User != null)
                {

                    //txaLog.AppendText("Conexion establecida: Servidor - " + LDAP_Server);


                    bool res = false;

                    res = ValidarUsuario(user, pass, LDAP_Server);

                    #region MULTIPLES_CONEXIONES_BORRAR
                    /*
                    if (!bConexionMultiple)
                    {
                        res = ValidarUsuario(user, pass, LDAP_Server);
                        Comun.cLog.EscribirLog(Comun.MensajeLog("EjecutarLogin", "Después de validar  - res: " + res + Comun.NuevaLinea, ""));
                    }
                    else
                    {

                        res = ValidarUsuarioMultiplesConexiones(user, pass, conexionesListaLocal);
                        if (res)
                        {
                            LDAP_Pass = GetClave();
                            LDAP_Server = GetServidor();
                            LDAP_User = GetUsuario();

                            conexionLocal = conexion;
                        }
                        Comun.cLog.EscribirLog(Comun.MensajeLog("EjecutarLogin", "Multiples dominios - Después de validar   - res: " + res + ", SERVIDOR: " + LDAP_Server + Comun.NuevaLinea, ""));
                    }*/
                    #endregion

                    string sValidacion;
                    string sServidor = ConvertirLDAPServer(LDAP_Server, true);
                    string sDomconDominio = ConvertirLDAPServerConDominio(LDAP_Server);
                    string sServidorDom = ConvertirLDAPServer(LDAP_Server, false);
                    string sUsuarioDom = sServidorDom + "\\" + GetUsuario();

                    log.Info("Connects to server: " + LDAP_Server + ".The domain in .domain.server format is: " + sDomconDominio + ".");
                    

                    if (res)
                    {
                        sValidacion = "OK";
                        log.Info("The connection " + sDomconDominio + " is established and user " + user + " is validated.");

                        string email = GetEmail(user, pass, LDAP_Server);

                        if (email != null)
                        {
                            string sUserEncontrado = "no";
                            string sNegacion = "";
                            usuarioLocal = new Usuarios();

                            // Searches for the given user
                            usuarioLocal = cUsuario.getUserByEmail(email);

                            if (usuarioLocal == null)
                            {
                                #region USUARIO_NO_EXISTE

                                // Verificamos si se deja acceder sólo con los datos del LDAP
                                Parametros ldapParametro = cParametros.GetItemByName("LDAP_CREAR_USUARIO");

                                if (ldapParametro != null && (ldapParametro.Valor == "SI" || ldapParametro.Valor == "YES"))
                                {
                                    usuarioLocal = UsuariosLoginLDAP(user, pass, LDAP_Server);

                                    if (usuarioLocal != null)
                                    {
                                        usuarioLocal.Activo = true;
                                        usuarioLocal.CambiarClave = false;
                                        usuarioLocal.ClienteID = conexionLocal.ClienteID;

                                        try
                                        {
                                            usuarioLocal.Clave = Util.EncryptKey(pass);
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex.Message);
                                            usuarioLocal.Clave = "error";
                                        }

                                        if (usuarioLocal.Apellidos == "" || usuarioLocal.Apellidos == null)
                                        {
                                            string apellidos = GetLastname(user, pass, LDAP_Server);

                                            if (apellidos != null)
                                            {
                                                usuarioLocal.Apellidos = apellidos;
                                            }
                                        }

                                        string company = GetCompany(user, pass, LDAP_Server);

                                        if (company != null)
                                        {

                                            long entidadID = 0;
                                            EntidadesController cEntidades = new EntidadesController();
                                            entidadID = cEntidades.GetEntidadIDByNombre(company, (long)usuarioLocal.ClienteID);

                                            if (entidadID != 0)
                                            {
                                                usuarioLocal.EntidadID = entidadID;
                                                usuarioLocal.Interno = false;
                                            }
                                            else
                                            {
                                                log.Info("La entidad no existe");
                                            }

                                        }

                                        if (usuarioLocal.Telefono == "" || usuarioLocal.Telefono == null)
                                        {
                                            string telephone = GetTelephoneNumber(user, pass, LDAP_Server);
                                            if (telephone != null)
                                            {
                                                usuarioLocal.Telefono = telephone;
                                            }
                                        }

                                        usuarioLocal.Interno = true;
                                        usuarioLocal.OperadorID = null;
                                        usuarioLocal.EntidadID = null;
                                        usuarioLocal.SAPTipoNIFID = null;
                                        usuarioLocal.DepartamentoID = null;
                                        usuarioLocal.EmpresaProveedoraID = null;
                                        usuarioLocal.LDAP = true;

                                        UsuariosController cUsuarioLocal = new UsuariosController();
                                        usuarioLocal = cUsuarioLocal.AddItem(usuarioLocal);

                                        #region Generar Histórico de Monitoring

                                        Data.MonitoringModificacionesUsuarios HistoricosDel = new Data.MonitoringModificacionesUsuarios();
                                        MonitoringModificacionesUsuariosController cMonitoringModificacionesUsuariosDel = new MonitoringModificacionesUsuariosController();
                                        HistoricosDel.UsuarioID = Convert.ToInt64(usuarioLocal.UsuarioID);

                                        if (usuarioLocalMod.UsuarioID.ToString() != "")
                                            HistoricosDel.UsuarioModificadorID = usuarioLocalMod.UsuarioID;
                                        else
                                            HistoricosDel.UsuarioModificadorID = usuarioLocal.UsuarioID;

                                        HistoricosDel.OperacionRealizada = "Crear Usuario";
                                        HistoricosDel.CambioEfectuado = "Crear Usuario";
                                        HistoricosDel.ProyectoTipoID = Convert.ToInt32(Comun.Modulos.GLOBAL);
                                        HistoricosDel.FechaModificacion = DateTime.Now;

                                        cMonitoringModificacionesUsuariosDel.AddItem(HistoricosDel);
                                        cMonitoringModificacionesUsuariosDel = null;

                                        #endregion

                                        if (usuarioLocal != null)
                                        {
                                            List<PerfilesGrupos> listaPerfiles = null;
                                            UsuariosPerfilesController cUsuariosPerfiles = new UsuariosPerfilesController();
                                            PerfilesGruposController cPerfiles = new PerfilesGruposController();

                                            List<string> listaGrupos = GetGruposByUsuario(user, LDAP_Server, LDAP_User, LDAP_Pass);

                                            foreach (string sGrupo in listaGrupos)
                                            {
                                                listaPerfiles = cPerfiles.GetPerfilesByGrupo(sGrupo);

                                                foreach (PerfilesGrupos perfil in listaPerfiles)
                                                {
                                                    UsuariosPerfiles nuevoPerfil = new UsuariosPerfiles();
                                                    nuevoPerfil.PerfilID = perfil.PerfilID;
                                                    nuevoPerfil.UsuarioID = usuarioLocal.UsuarioID;
                                                    cUsuariosPerfiles.AddItem(nuevoPerfil);
                                                }
                                            }
                                        }
                                    }

                                }

                                #endregion
                            }
                            else
                            {
                                sUserEncontrado = "si";
                                if (!usuarioLocal.Activo)
                                {

                                    // Usuario existe en Tree pero está desactivo (Validar el parametro si se activa el usuario con LDAP_CREAR_USUARIO)
                                    Parametros ldapParametro = cParametros.GetItemByName("LDAP_CREAR_USUARIO");

                                    if (ldapParametro != null && (ldapParametro.Valor == "SI" || ldapParametro.Valor == "YES"))
                                    {
                                        // Debe activar el usuario y quitar los perfiles
                                        usuarioLocal.Activo = true;
                                        cUsuario.UpdateItem(usuarioLocal);

                                        #region Generar Histórico de Monitoring
                                        Data.MonitoringModificacionesUsuarios Historicos = new Data.MonitoringModificacionesUsuarios();
                                        MonitoringModificacionesUsuariosController cMonitoringModificacionesUsuarios = new MonitoringModificacionesUsuariosController();

                                        Historicos.UsuarioID = Convert.ToInt64(usuarioLocal.UsuarioID);
                                        if (usuarioLocalMod.UsuarioID.ToString() != "")
                                            Historicos.UsuarioModificadorID = usuarioLocalMod.UsuarioID;
                                        else
                                            Historicos.UsuarioModificadorID = usuarioLocal.UsuarioID;

                                        Historicos.OperacionRealizada = "Activar Usuario";
                                        Historicos.CambioEfectuado = "Activar Usuario";
                                        Historicos.ProyectoTipoID = Convert.ToInt32(Comun.Modulos.GLOBAL);
                                        Historicos.FechaModificacion = DateTime.Now;

                                        string EstadoJson = JsonConvert.SerializeObject(Historicos);
                                        cMonitoringModificacionesUsuarios.AddItem(Historicos);
                                        cMonitoringModificacionesUsuarios = null;

                                        #endregion

                                        UsuariosPerfilesController cUsuariosPerfiles = new UsuariosPerfilesController();
                                        List<long> PerfilesUsuario = null;
                                        PerfilesUsuario = cUsuariosPerfiles.UsuarioperfilesAsignadosIDs(usuarioLocal.UsuarioID);

                                        foreach (long selec in PerfilesUsuario)
                                        {
                                            cUsuariosPerfiles.DeleteItem(Convert.ToInt64(selec));

                                            #region Generar Histórico de Monitoring

                                            Data.MonitoringModificacionesUsuarios HistoricosDel = new Data.MonitoringModificacionesUsuarios();

                                            MonitoringModificacionesUsuariosController cMonitoringModificacionesUsuariosDel = new MonitoringModificacionesUsuariosController();

                                            HistoricosDel.UsuarioID = Convert.ToInt64(usuarioLocal.UsuarioID);
                                            if (usuarioLocalMod.UsuarioID.ToString() != "")
                                                HistoricosDel.UsuarioModificadorID = usuarioLocalMod.UsuarioID;
                                            else
                                                HistoricosDel.UsuarioModificadorID = usuarioLocal.UsuarioID;

                                            HistoricosDel.OperacionRealizada = "Quitar Perfil";
                                            HistoricosDel.CambioEfectuado = "Quitar Perfil";
                                            HistoricosDel.ProyectoTipoID = Convert.ToInt32(Comun.Modulos.GLOBAL);
                                            HistoricosDel.FechaModificacion = DateTime.Now;



                                            //Historicos.JSON = EstadoJson;

                                            cMonitoringModificacionesUsuariosDel.AddItem(HistoricosDel);

                                            cMonitoringModificacionesUsuariosDel = null;

                                            #endregion

                                            cUsuariosPerfiles = null;
                                        }

                                        //AgregarPerfiles
                                        if (usuarioLocal != null)
                                        {
                                            List<PerfilesGrupos> listaPerfiles = null;
                                            PerfilesGruposController cPerfiles = new PerfilesGruposController();
                                            List<string> listaGrupos = GetGruposByUsuario(user, LDAP_Server, LDAP_User, LDAP_Pass);

                                            foreach (string sGrupo in listaGrupos)
                                            {
                                                listaPerfiles = cPerfiles.GetPerfilesByGrupo(sGrupo);

                                                foreach (PerfilesGrupos perfil in listaPerfiles)
                                                {
                                                    UsuariosPerfiles nuevoPerfil = new UsuariosPerfiles();
                                                    nuevoPerfil.PerfilID = perfil.PerfilID;
                                                    nuevoPerfil.UsuarioID = usuarioLocal.UsuarioID;
                                                    cUsuariosPerfiles.AddItem(nuevoPerfil);
                                                }

                                                #region Generar Histórico de Monitoring
                                                Data.MonitoringModificacionesUsuarios HistoricosAdd = new Data.MonitoringModificacionesUsuarios();

                                                MonitoringModificacionesUsuariosController cMonitoringModificacionesUsuariosAdd = new MonitoringModificacionesUsuariosController();

                                                HistoricosAdd.UsuarioID = Convert.ToInt64(usuarioLocal.UsuarioID);
                                                if (usuarioLocalMod.UsuarioID.ToString() != "")
                                                    HistoricosAdd.UsuarioModificadorID = usuarioLocalMod.UsuarioID;
                                                else
                                                    HistoricosAdd.UsuarioModificadorID = usuarioLocal.UsuarioID;

                                                HistoricosAdd.OperacionRealizada = "Agregar Perfil";
                                                HistoricosAdd.CambioEfectuado = "Agregar Perfil";
                                                HistoricosAdd.ProyectoTipoID = Convert.ToInt32(Comun.Modulos.GLOBAL);
                                                HistoricosAdd.FechaModificacion = DateTime.Now;



                                                //Historicos.JSON = EstadoJson;

                                                cMonitoringModificacionesUsuariosAdd.AddItem(Historicos);

                                                cMonitoringModificacionesUsuariosAdd = null;

                                                #endregion

                                                cUsuariosPerfiles = null;
                                            }
                                        }
                                    }

                                    return usuarioLocal;
                                }
                            }

                            if (sUserEncontrado == "no")
                            {
                                sNegacion = " doesn't";
                            }

                            log.Info("User " + user + " is validated in the domain: " + sValidacion + " and it goes to look for the parameter 'mail', it returns: " + email + " and in the database it" + sNegacion + " finds the user.");
                            
                            if (sNegacion == "")
                            {
                                log.Info("Log in: " + sValidacion);
                            }
                            else if (sNegacion == " doesn't")
                            {
                                sValidacion = "NOK";
                                log.Info("Log in: " + sValidacion);
                            }
                            
                        }
                    }
                    else
                    {
                        sValidacion = "NOK";
                        log.Info("The connection " + sDomconDominio + " is established and user " + user + " is not validated.");
                        log.Info("Log in: " + sValidacion);
                    }
                    
                }
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            // Checks for the groups

            return usuarioLocal;
        }

        public List<string> GetGruposByUsuario(string user, string servidorLocal, string userLocal, string claveLocal)
        {
            // Local variables
            List<string> lista = null;

            // Obtains the information of the groups from a given user.
            try
            {
                lista = new List<string>();
                // set up domain context
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, servidorLocal, userLocal, claveLocal))
                {
                    // find a user
                    UserPrincipal userioDominio = UserPrincipal.FindByIdentity(ctx, user);

                    if (user != null)
                    {
                        // get the user's groups
                        var groups = userioDominio.GetAuthorizationGroups();
                        lista = new List<string>();

                        foreach (GroupPrincipal group in groups)
                        {
                            // Creates the list with the different groups
                            lista.Add(group.Name);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<string>();
            }

            // Returns the result
            return lista;

        }

        #endregion

        #region LDAP
        /// <summary>
        /// Convierte la url del servidor LDAP de formato LDAP:// a fomato domain.subdomain
        /// </summary>
        /// <param name="sServer"></param>
        /// <returns></returns>
        private string ConvertirLDAPServer(string sServer, bool bConPuerto)
        {
            // Local variables
            string sResultado = null;

            string sServerReducido = null;
            if (sServer.IndexOf('#') >= 0)
            {
                sServerReducido = sServer.Substring(sServer.IndexOf('#') + 1, sServer.Length - (sServer.IndexOf('#') + 1));
            }
            else
            {
                sServerReducido = sServer;
            }
            // Process the input
            sResultado = sServerReducido.Replace("DC=", "");
            sResultado = sResultado.Replace("OU=", "");


            sResultado = sResultado.Replace(",", ".");

            if (sResultado.Contains("LDAPS://"))
            {
                sResultado = sResultado.Replace("LDAPS://", "");
                if (bConPuerto)
                {
                    sResultado = sResultado + ":636";
                }

            }
            else
            {
                if (sResultado.Contains("LDAP://"))
                {
                    sResultado = sResultado.Replace("LDAP://", "");
                }

            }
            // Returns the result
            return sResultado;
        }

        private string ObtenerGrupoDeCadena(string sServer)
        {
            // Local variables

            string sGrupo = null;

            if (sServer.IndexOf('#') >= 0)
            {
                sGrupo = sServer.Substring(0, sServer.IndexOf('#'));
            }

            // Returns the result
            return sGrupo;
        }
        private string ConvertirLDAPServerConDominio(string sServer)
        {
            // Local variables
            string sResultado = null;
            string sDominioConPuntos = ConvertirLDAPServer(sServer, true);
            //"LDAP://HMLREDECORP.BR/DC=HMLREDECORP,DC=BR"
            string sInicio = "";
            string sFinal = "";
            int iNumInicial = 0;
            int iNumFinal = 7;

            if (sServer.Contains("LDAPS://"))
            {
                sServer = sServer.Replace("LDAPS", "LDAP");

            }


            if (sServer.IndexOf('#') > 0)
            {
                iNumInicial = iNumInicial + sServer.IndexOf('#') + 1;
                iNumFinal = iNumFinal + sServer.IndexOf('#') + 1;
            }

            sInicio = sServer.Substring(iNumInicial, iNumFinal);
            sFinal = sServer.Substring(iNumFinal, sServer.Length - iNumFinal);

            sResultado = sInicio + sDominioConPuntos + "/" + sFinal;


            // Returns the result
            return sResultado;
        }

        private string GetEmail(string userName, string password, string domain)
        {
            try
            {
                //Comun.cLog.EscribirLog(Comun.MensajeLog("LDAP - GetEmail", "GetEmail: usuario de la bbdd:  " + GetUsuario() + Comun.NuevaLinea, ""));
                //Comun.cLog.EscribirLog(Comun.MensajeLog("LDAP - GetEmail", "GetEmail: clave de la bbdd:  " + GetClave() + Comun.NuevaLinea, ""));
                //"LDAP://HMLREDECORP.BR/DC=HMLREDECORP,DC=BR"
                string dominioConDominio = ConvertirLDAPServerConDominio(domain);
                string dominioConPuntos = ConvertirLDAPServer(domain, false);
                string usuarioConDominio = dominioConPuntos + "\\" + GetUsuario();
                DirectoryEntry adsEntry = new System.DirectoryServices.DirectoryEntry(dominioConDominio, usuarioConDominio, GetClave());
                DirectorySearcher deSearch = new DirectorySearcher(adsEntry);

                deSearch.SearchScope = SearchScope.Subtree;

                deSearch.ReferralChasing = ReferralChasingOption.All;

                //deSearch.Filter = "(&(objectClass=user)(anr=" + userName + "))";
                deSearch.Filter = "(&(objectClass=user)(sAMAccountName=" + userName + "))";
                SearchResult result = deSearch.FindOne();

                if (result != null)
                {
                    DirectoryEntry directoryEntry = new System.DirectoryServices.DirectoryEntry();

                    directoryEntry = result.GetDirectoryEntry();
                    try
                    {
                        string email = directoryEntry.Properties["mail"][0].ToString();
                        return email;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        return null;
                    }


                }
                else
                {
                    return null;
                }
            }
            catch (Exception Ex2)
            {
                log.Error(Ex2.Message);
                return null;

            }



        }

        private string GetCompany(string userName, string password, string domain)
        {

            //"LDAP://HMLREDECORP.BR/DC=HMLREDECORP,DC=BR"
            string dominioConDominio = ConvertirLDAPServerConDominio(domain);
            string dominioConPuntos = ConvertirLDAPServer(domain, false);
            string usuarioConDominio = dominioConPuntos + "\\" + GetUsuario();
            DirectoryEntry adsEntry = new System.DirectoryServices.DirectoryEntry(dominioConDominio, usuarioConDominio, GetClave());
            DirectorySearcher deSearch = new DirectorySearcher(adsEntry);

            deSearch.SearchScope = SearchScope.Subtree;

            deSearch.ReferralChasing = ReferralChasingOption.All;

            deSearch.Filter = "(&(objectClass=user)(anr=" + userName + "))";
            SearchResult result = deSearch.FindOne();

            if (result != null)
            {
                DirectoryEntry directoryEntry = new System.DirectoryServices.DirectoryEntry();

                directoryEntry = result.GetDirectoryEntry();
                try
                {
                    string company = directoryEntry.Properties["company"][0].ToString();
                    return company;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        private string GetLastname(string userName, string password, string domain)
        {

            //"LDAP://HMLREDECORP.BR/DC=HMLREDECORP,DC=BR"
            string dominioConDominio = ConvertirLDAPServerConDominio(domain);
            string dominioConPuntos = ConvertirLDAPServer(domain, false);
            string usuarioConDominio = dominioConPuntos + "\\" + GetUsuario();
            DirectoryEntry adsEntry = new System.DirectoryServices.DirectoryEntry(dominioConDominio, usuarioConDominio, GetClave());
            DirectorySearcher deSearch = new DirectorySearcher(adsEntry);

            deSearch.SearchScope = SearchScope.Subtree;

            deSearch.ReferralChasing = ReferralChasingOption.All;

            deSearch.Filter = "(&(objectClass=user)(anr=" + userName + "))";
            SearchResult result = deSearch.FindOne();

            if (result != null)
            {
                DirectoryEntry directoryEntry = new System.DirectoryServices.DirectoryEntry();

                directoryEntry = result.GetDirectoryEntry();
                try
                {
                    string lastname = directoryEntry.Properties["sn"][0].ToString();
                    return lastname;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return null;
                }

            }
            else
            {
                return null;
            }

        }

        private string GetTelephoneNumber(string userName, string password, string domain)
        {

            //"LDAP://HMLREDECORP.BR/DC=HMLREDECORP,DC=BR"
            string dominioConDominio = ConvertirLDAPServerConDominio(domain);
            string dominioConPuntos = ConvertirLDAPServer(domain, false);
            string usuarioConDominio = dominioConPuntos + "\\" + GetUsuario();
            DirectoryEntry adsEntry = new System.DirectoryServices.DirectoryEntry(dominioConDominio, usuarioConDominio, GetClave());
            DirectorySearcher deSearch = new DirectorySearcher(adsEntry);

            deSearch.SearchScope = SearchScope.Subtree;

            deSearch.ReferralChasing = ReferralChasingOption.All;

            deSearch.Filter = "(&(objectClass=user)(anr=" + userName + "))";
            SearchResult result = deSearch.FindOne();

            if (result != null)
            {
                DirectoryEntry directoryEntry = new System.DirectoryServices.DirectoryEntry();

                directoryEntry = result.GetDirectoryEntry();
                try
                {
                    string telephone = directoryEntry.Properties["telephoneNumber"][0].ToString();
                    return telephone;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return null;
                }

            }
            else
            {
                return null;
            }

        }

        private Usuarios GetUsuarioFromLDAP(string userName, string password, string domain)
        {
            Usuarios oUsuario = new Usuarios();


            string dominioConDominio = ConvertirLDAPServerConDominio(domain);

            string dominioConPuntos = ConvertirLDAPServer(domain, false);
            string usuarioConDominio = dominioConPuntos + "\\" + GetUsuario();

            DirectoryEntry adsEntry = new System.DirectoryServices.DirectoryEntry(dominioConDominio, usuarioConDominio, GetClave());

            DirectorySearcher deSearch = new DirectorySearcher(adsEntry);

            deSearch.SearchScope = SearchScope.Subtree;

            deSearch.ReferralChasing = ReferralChasingOption.All;

            deSearch.Filter = "(&(objectClass=user)(anr=" + userName + "))";

            SearchResult result = deSearch.FindOne();

            if (result != null)
            {
                DirectoryEntry directoryEntry = new System.DirectoryServices.DirectoryEntry();

                directoryEntry = result.GetDirectoryEntry();
                string email = "";
                string nombre = "";
                string apellidos = "";
                string sTelefono = "";
                bool devuelveNulo = false;

                try
                {
                    email = directoryEntry.Properties["mail"][0].ToString();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    devuelveNulo = true;
                }

                try
                {
                    nombre = directoryEntry.Properties["givenName"][0].ToString();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    devuelveNulo = true;
                }

                try
                {
                    apellidos = directoryEntry.Properties["sn"][0].ToString();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    devuelveNulo = true;
                }

                try
                {
                    sTelefono = directoryEntry.Properties["telephoneNumber"][0].ToString();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
                //string email = directoryEntry.Properties["mail"][0].ToString();
                //string nombre = directoryEntry.Properties["givenName"][0].ToString();
                //string apellidos = directoryEntry.Properties["sn"][0].ToString();
                //string sTelefono = directoryEntry.Properties["facsimileTelephoneNumber"][0].ToString();


                oUsuario.EMail = email;
                oUsuario.Nombre = nombre;
                oUsuario.Apellidos = apellidos;
                oUsuario.Telefono = sTelefono;

                if (!devuelveNulo)
                {
                    return oUsuario;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }



        }


        private bool ValidarUsuario(string userName, string password, string domain)
        {
            bool authentic = false;
            try
            {
                string sServer = ConvertirLDAPServer(domain, true);
                string sGrupo = ObtenerGrupoDeCadena(domain);
                using (var context = new PrincipalContext(ContextType.Domain, sServer, GetUsuario(), GetClave()))
                {
                    //Username and password for authentication.
                    authentic = context.ValidateCredentials(userName, password);
                    if (sGrupo != null)
                    {
                        List<string> listaGrupos = GetGruposByUsuario(userName, sServer, GetUsuario(), GetClave());
                        bool bEncontrado = false;
                        foreach (string grupo in listaGrupos)
                        {
                            if (!bEncontrado)
                            {
                                if (grupo == sGrupo)
                                {
                                    bEncontrado = true;
                                }
                            }

                        }
                        if (!bEncontrado)
                        {
                            authentic = false;
                        }
                    }
                }
            }
            catch (DirectoryServicesCOMException ex)
            {
                log.Error(ex.Message);
            }
            return authentic;
        }

        public bool ValidarUsuarioDominio(string userName, string password, string domain)
        {
            bool authentic = false;
            try
            {
                string sServer = ConvertirLDAPServer(domain, true);

                try
                {
                    using (var context = new PrincipalContext(ContextType.Domain, sServer, userName, password))
                    {
                        //Username and password for authentication.
                        authentic = context.ValidateCredentials(userName, password);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

            }
            catch (DirectoryServicesCOMException ex)
            {
                log.Error(ex.Message);
            }
            return authentic;
        }


        private bool ValidarUsuarioMultiplesConexiones(string userName, string password, List<LDAPConexion> listaConexiones)
        {
            bool authentic = false;

            try
            {
                foreach (LDAPConexion dato in listaConexiones)
                {
                    //Si ya se ha encontrado el usuario, ya no se busca en el resto de dominios
                    if (!authentic)
                    {
                        string sServer = ConvertirLDAPServer(dato.dominio, true);

                        try
                        {
                            using (var context = new PrincipalContext(ContextType.Domain, sServer, dato.usuario, dato.clave))
                            {
                                //Username and password for authentication.
                                authentic = context.ValidateCredentials(userName, password);


                                //Si lo encuentra, entonces se guarda en las variables de la clase los valores del dominio donde está el usuario
                                if (authentic)
                                {


                                    SetClave(dato.clave);
                                    SetServidor(dato.servidor);
                                    SetUsuario(dato.usuario);
                                    SetDominio(dato.servidor);

                                    Vw_ToolIntegracionesServiciosMetodosConexiones conexionLocal = null;
                                    ToolConexionesController cConexion = new ToolConexionesController();
                                    if (dato.conexionid > 0)
                                    {
                                        conexionLocal = cConexion.GetItem<Vw_ToolIntegracionesServiciosMetodosConexiones>(dato.conexionid);
                                        if (conexionLocal != null)
                                        {
                                            conexion = conexionLocal;
                                        }
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

            }
            catch (DirectoryServicesCOMException ex)
            {
                log.Error(ex.Message);
            }
            return authentic;
        }

        #endregion

    }
}