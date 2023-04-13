using System;
using System.Data;
using Ext.Net;
using System.Data.SqlClient;
using log4net;
using System.Reflection;

namespace CapaNegocio
{
    public class SQLServerModelController
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static SqlConnection connection = null;

        #region CONSTRUCTORES

        public SQLServerModelController()
                : base()
        {
            
        }

        public SQLServerModelController(string sCadena)
                : base()
        {
            Conectar(sCadena);
        }

        #endregion

        #region CONEXION

        private bool Conectar(string serverName, string sDatabase, string sUser, string sClave)
        {
            // Local variables
            bool bConectado = false;
            string connetionString = "Data Source=" + serverName + ";Initial Catalog=" + sDatabase + ";User ID=" + sUser + ";Password=" + sClave + "";
            try
            {
                connection = new SqlConnection(connetionString);

                connection.Open();
                bConectado = true;

            }
            catch (SqlException ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return bConectado;
        }

        private bool Conectar(string sCadenaConexion)
        {
            // Local variables
            bool bConectado = false;
            string connetionString = sCadenaConexion;
            try
            {
                connection = new SqlConnection(connetionString);

                connection.ConnectionString = connetionString;
                connection.Open();
                bConectado = true;


            }
            catch (SqlException ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return bConectado;
        }

        #endregion

        #region ARBOL MODELO

        /// <summary>
        /// Obtiene la lista de nodos del arbol convertida a JSON
        /// </summary>
        /// <param name="nodes">Nodo a Obtener</param>
        /// <returns>Cadena en JSON con los nodos</returns>
        public string GetItemsArbolString(NodeCollection nodes, string sNombreConexion)
        {
            string res = "";
            res = GetItemsArbol(nodes, sNombreConexion).ToJson();
            return res;
        }

        /// <summary>
        /// Devuelve la carga completa de nodos para el árbol
        /// </summary>
        /// <param name="nodes">Estructura de Nodos a Cargar</param>
        /// <returns>Colección de Nodos</returns>
        public NodeCollection GetItemsArbol(NodeCollection nodes, string sNombreConexion)
        {
            try
            {
                if ((nodes == null))
                {
                    nodes = new NodeCollection();
                }
                nodes.Clear();
                Node root = new Node();
                root.NodeID = "ROOT";
                root.Text = sNombreConexion;
                root.Qtip = "ROOT";
                root.Expanded = true;
                root.IconCls = "icon-folderdatabase";
                root.Expandable = false;

                //TreeNode nodo = new TreeNode();
                //nodo = GetAllTreeNodes(ref root);

                #region CREATES TREE

                try
                {
                    if (connection != null && connection.State.ToString().ToUpper().Equals("OPEN"))
                    {
                        //connection.Open();
                        DataTable dtTablas = connection.GetSchema("Tables", new String[] { null, null, null, "BASE TABLE" });
                        //DataTable dtTablas = connection.GetSchema("Tables");
                        DataTable dtVistas = connection.GetSchema("Views");
                        #region TABLAS

                        if (dtTablas != null && dtTablas.Rows != null && dtTablas.Rows.Count > 0)
                        {
                            Node nodo = new Node();
                            nodo.NodeID = "CARPETA1";
                            nodo.Expanded = true;
                            nodo.IconCls = "icon-folder";
                            nodo.Text = "Tables";
                            nodo.Expanded = false;
                            root.Children.Add(nodo);
                            Node rama = null;
                            Node ramaColumna = null;
                            int iTabla = 0;
                            ConfigItem itemTabla = null;
                            ConfigItem itemTablaTipoDato = null;


                            foreach (System.Data.DataRow fila in dtTablas.Select("", "TABLE_NAME"))
                            {
                                QTipCfg config = new QTipCfg();
                                rama = new Node();
                                rama.NodeID = "CARPETA1" + iTabla.ToString();
                                rama.Expanded = true;
                                rama.IconCls = "icon-table";
                                rama.Text = ProcesarNombreTabla(fila[2].ToString());
                                rama.Qtip = fila[1].ToString() + "." + fila[2].ToString();
                                itemTabla = new ConfigItem();
                                itemTabla.Name = "data";
                                itemTabla.Value = "\"" + fila[1].ToString() + "." + fila[2].ToString() + "\"";
                                rama.CustomAttributes.Add(itemTabla);
                                rama.Expanded = false;
                                nodo.Children.Add(rama);
                                iTabla = iTabla + 1;

                                DataTable dtColumnas = connection.GetSchema("Columns", new String[] { null, null, fila[2].ToString(), null });

                                if (dtColumnas != null && dtColumnas.Rows != null && dtColumnas.Rows.Count > 0)
                                {
                                    int iColumna = 0;

                                    foreach (System.Data.DataRow columna in dtColumnas.Select("", "COLUMN_NAME"))
                                    {
                                        ramaColumna = new Node();
                                        ramaColumna.NodeID = rama.NodeID + "-" + iColumna.ToString();
                                        ramaColumna.Expanded = true;
                                        ramaColumna.IconCls = "icon-bulletblue";
                                        ramaColumna.Text = ProcesarNombreCampo(columna[3].ToString());
                                        ramaColumna.Qtip = columna[1].ToString() + "." + fila[2].ToString() + "." + columna[3].ToString();
                                        ConfigItem nodoType = new ConfigItem();
                                        nodoType.Name = "type";
                                        nodoType.Mode = ParameterMode.Value;
                                        nodoType.Value = columna[7].ToString();
                                        ramaColumna.CustomAttributes.Add(nodoType);
                                        itemTabla = new ConfigItem();
                                        itemTabla.Name = "data";
                                        itemTabla.Value = "\"" + columna[1].ToString() + "." + fila[2].ToString() + "." + columna[3].ToString() + "\"";
                                        ramaColumna.CustomAttributes.Add(itemTabla);
                                        itemTablaTipoDato = new ConfigItem();
                                        itemTablaTipoDato.Name = "tipo";
                                        itemTablaTipoDato.Value = "\"" + columna[7].ToString() + "\"";
                                        rama.CustomAttributes.Add(itemTablaTipoDato);
                                        ramaColumna.Expanded = false;
                                        rama.Children.Add(ramaColumna);
                                        iColumna = iColumna + 1;

                                    }
                                }
                            }
                        }

                        #endregion

                        #region VISTAS

                        if (dtVistas != null && dtVistas.Rows != null && dtVistas.Rows.Count > 0)
                        {
                            Node nodoVista = new Node();
                            nodoVista.NodeID = "CARPETA2";
                            nodoVista.Expanded = true;
                            nodoVista.IconCls = "icon-folder";
                            nodoVista.Text = "Views";
                            nodoVista.Qtip = "Views";
                            nodoVista.Expanded = false;
                            root.Children.Add(nodoVista);
                            //NewParentNode = nodoVista;
                            Node ramaVista = null;
                            Node ramaVistaColumna = null;
                            int iTablaVista = 0;
                            ConfigItem itemVista = null;
                            ConfigItem itemVistaTipoDato = null;

                            foreach (System.Data.DataRow filaVista in dtVistas.Select("", "TABLE_NAME"))
                            {
                                ramaVista = new Node();
                                ramaVista.NodeID = "CARPETA2" + iTablaVista.ToString();
                                ramaVista.Expanded = true;
                                ramaVista.IconCls = "icon-tableconnect";
                                ramaVista.Text = ProcesarNombreTabla(filaVista[2].ToString());
                                ramaVista.Qtip = filaVista[1].ToString() + "." + filaVista[2].ToString();
                                itemVista = new ConfigItem();
                                itemVista.Name = "data";
                                itemVista.Value = "\"" + filaVista[1].ToString() + "." + filaVista[2].ToString() + "\"";
                                ramaVista.CustomAttributes.Add(itemVista);
                                ramaVista.Expanded = false;
                                nodoVista.Children.Add(ramaVista);
                                iTablaVista = iTablaVista + 1;

                                DataTable dtColumnasVistas = connection.GetSchema("Columns", new String[] { null, null, filaVista[2].ToString(), null });

                                if (dtColumnasVistas != null && dtColumnasVistas.Rows != null && dtColumnasVistas.Rows.Count > 0)
                                {
                                    int iColumnaVista = 0;
                                    foreach (System.Data.DataRow columnaVista in dtColumnasVistas.Select("", "COLUMN_NAME"))
                                    {
                                        ramaVistaColumna = new Node();
                                        ramaVistaColumna.NodeID = ramaVista.NodeID + "-" + iColumnaVista.ToString();
                                        ramaVistaColumna.Expanded = true;
                                        ramaVistaColumna.IconCls = "icon-bulletorange";
                                        ramaVistaColumna.Text = ProcesarNombreCampo(columnaVista[3].ToString());
                                        ramaVistaColumna.Qtip = columnaVista[1].ToString() + "." + filaVista[2].ToString() + "." + columnaVista[3].ToString();
                                        ConfigItem nodoType = new ConfigItem();
                                        nodoType.Name = "type";
                                        nodoType.Mode = ParameterMode.Value;
                                        nodoType.Value = columnaVista[7].ToString();
                                        ramaVistaColumna.CustomAttributes.Add(nodoType);
                                        itemVista = new ConfigItem();
                                        itemVista.Name = "data";
                                        itemVista.Value = "\"" + columnaVista[1].ToString() + "." + filaVista[2].ToString() + "." + columnaVista[3].ToString() + "\"";
                                        ramaVistaColumna.CustomAttributes.Add(itemVista);
                                        itemVistaTipoDato = new ConfigItem();
                                        itemVistaTipoDato.Name = "tipo";
                                        itemVistaTipoDato.Value = "\"" + columnaVista[7].ToString() + "\"";
                                        ramaVistaColumna.CustomAttributes.Add(itemVistaTipoDato);
                                        ramaVistaColumna.Expanded = false;
                                        ramaVista.Children.Add(ramaVistaColumna);
                                        iColumnaVista = iColumnaVista + 1;
                                    }
                                }
                            }
                        }

                        #endregion

                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    throw new Exception(ex.Message);
                }

                #endregion

                nodes.Add(root);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

            return nodes;

        }

        /// <summary>
        /// Función Recursiva que obtiene todos los nodos de un padre concreto
        /// </summary>        
        /// <param name="ParentNode">Nodo Padre</param>
        /// <returns>Arbol de nodos</returns>
        public Node GetAllTreeNodes(ref Node ParentNode)
        {
            Node NewParentNode = new Node();
            try
            {
                if (connection != null && connection.State.ToString().ToUpper().Equals("OPEN"))
                {
                    //connection.Open();
                    DataTable dtTablas = connection.GetSchema("Tables", new String[] { null, null, null, "BASE TABLE" });
                    //DataTable dtTablas = connection.GetSchema("Tables");
                    DataTable dtVistas = connection.GetSchema("Views");
                    #region TABLAS

                    if (dtTablas != null && dtTablas.Rows != null && dtTablas.Rows.Count > 0)
                    {
                        Node nodo = new Node();
                        nodo.NodeID = "CARPETA1";
                        nodo.Expanded = true;
                        nodo.IconCls = "icon-folder";
                        nodo.Text = "Tables";
                        nodo.Expanded = false;
                        ParentNode.Children.Add(nodo);
                        NewParentNode = nodo;
                        Node rama = null;
                        Node ramaColumna = null;
                        int iTabla = 0;


                        foreach (System.Data.DataRow fila in dtTablas.Select("", "TABLE_NAME"))
                        {
                            QTipCfg config = new QTipCfg();
                            rama = new Node();
                            rama.NodeID = "CARPETA1" + iTabla.ToString();
                            rama.Expanded = true;
                            rama.IconCls = "icon-table";
                            rama.Text = fila[1].ToString() + "." + fila[2].ToString();
                            rama.Expanded = false;
                            nodo.Children.Add(rama);
                            iTabla = iTabla + 1;

                            DataTable dtColumnas = connection.GetSchema("Columns", new String[] { null, null, fila[2].ToString(), null });

                            if (dtColumnas != null && dtColumnas.Rows != null && dtColumnas.Rows.Count > 0)
                            {
                                int iColumna = 0;

                                foreach (System.Data.DataRow columna in dtColumnas.Select("", "COLUMN_NAME"))
                                {
                                    ramaColumna = new Node();
                                    ramaColumna.NodeID = rama.NodeID + "-" + iColumna.ToString();
                                    ramaColumna.Expanded = true;
                                    ramaColumna.IconCls = "icon-bulletblue";
                                    ramaColumna.Text = columna[1].ToString() + "." + fila[2].ToString() + "." + columna[3].ToString();
                                    ramaColumna.Expanded = false;
                                    rama.Children.Add(ramaColumna);
                                    iColumna = iColumna + 1;

                                }
                            }
                        }
                    }

                    #endregion

                    #region VISTAS

                    if (dtVistas != null && dtVistas.Rows != null && dtVistas.Rows.Count > 0)
                    {
                        Node nodoVista = new Node();
                        nodoVista.NodeID = "CARPETA2";
                        nodoVista.Expanded = true;
                        nodoVista.IconCls = "icon-folder";
                        nodoVista.Text = "Views";
                        nodoVista.Expanded = false;
                        ParentNode.Children.Add(nodoVista);
                        //NewParentNode = nodoVista;
                        Node ramaVista = null;
                        Node ramaVistaColumna = null;
                        int iTablaVista = 0;

                        foreach (System.Data.DataRow filaVista in dtVistas.Select("", "TABLE_NAME"))
                        {
                            ramaVista = new Node();
                            ramaVista.NodeID = "CARPETA2" + iTablaVista.ToString();
                            ramaVista.Expanded = true;
                            ramaVista.IconCls = "icon-tableconnect";
                            ramaVista.Text = filaVista[1].ToString() + "." + filaVista[2].ToString();
                            ramaVista.Expanded = false;
                            nodoVista.Children.Add(ramaVista);
                            iTablaVista = iTablaVista + 1;

                            DataTable dtColumnasVistas = connection.GetSchema("Columns", new String[] { null, null, filaVista[2].ToString(), null });

                            if (dtColumnasVistas != null && dtColumnasVistas.Rows != null && dtColumnasVistas.Rows.Count > 0)
                            {
                                int iColumnaVista = 0;
                                foreach (System.Data.DataRow columnaVista in dtColumnasVistas.Select("", "COLUMN_NAME"))
                                {
                                    ramaVistaColumna = new Node();
                                    ramaVistaColumna.NodeID = ramaVista.NodeID + "-" + iColumnaVista.ToString();
                                    ramaVistaColumna.Expanded = true;
                                    ramaVistaColumna.IconCls = "icon-bulletorange";
                                    ramaVistaColumna.Text = columnaVista[1].ToString() + "." + filaVista[2].ToString() + "." + columnaVista[3].ToString();
                                    ramaVistaColumna.Expanded = false;
                                    ramaVista.Children.Add(ramaVistaColumna);
                                    iColumnaVista = iColumnaVista + 1;
                                }
                            }
                        }
                    }

                    #endregion

                    #region FUNCIONES

                    //TreeNode nodoFuncion = new TreeNode();
                    //nodoFuncion.NodeID = "CARPETA3";
                    //nodoFuncion.Expanded = true;
                    //nodoFuncion.IconCls = "icon-sum";
                    //nodoFuncion.Text = "Functions";
                    //nodoFuncion.Draggable = false;
                    //nodoFuncion.Expanded = false;
                    //ParentNode.Nodes.Add(nodoFuncion);
                    //NewParentNode = nodoFuncion;
                    //TreeNode ramaFuncion = null;
                    //int iTablaFuncion = 0;

                    //#region APPROX_COUNT_DISTINCT

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_APPROX_COUNT_DISTINCT_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion

                    //#region AVG

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_AVG_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion

                    //#region CHECKSUM_AGG_FUNCTION

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_CHECKSUM_AGG_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion

                    //#region COUNT

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_COUNT_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion

                    //#region COUNT_BIG_FUNCTION

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_COUNT_BIG_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion

                    //#region GROUPING

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_GROUPING_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion

                    //#region GROUPING_ID

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_GROUPING_ID_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion

                    //#region MAX

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_MAX_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion

                    //#region MIN

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_MIN_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion

                    //#region STDEVP

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_STDEVP_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion

                    //#region STDEV

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_STDEV_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion

                    //#region SUM

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_SUM_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion

                    //#region VAR

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_VAR_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion

                    //#region VARP

                    //ramaFuncion = new TreeNode();
                    //ramaFuncion.NodeID = "CARPETA3" + iTablaFuncion.ToString();
                    //ramaFuncion.Expanded = true;
                    //ramaFuncion.IconCls = "icon-bulletred";
                    //ramaFuncion.Text = Comun.SQL_SERVER_VARP_FUNCTION;
                    //ramaFuncion.Draggable = false;
                    //ramaFuncion.Expanded = false;
                    //nodoFuncion.Nodes.Add(ramaFuncion);
                    //iTablaFuncion = iTablaFuncion + 1;

                    //#endregion



                    #endregion
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
            return NewParentNode;
        }

        private string ProcesarNombreTabla(string sTabla)
        {
            // Local variables
            string sResultado = null;

            if (sTabla != null && sTabla.Length > 25)
            {
                sResultado = sTabla.Substring(0, 22) + "...";
            }
            else
            {
                sResultado = sTabla;
            }

            // Returns the result
            return sResultado;
        }

        private string ProcesarNombreCampo(string sCampo)
        {
            // Local variables
            string sResultado = null;

            if (sCampo != null && sCampo.Length > 25)
            {
                sResultado = "..." + sCampo.Substring(sCampo.Length - 22);
            }
            else
            {
                sResultado = sCampo;
            }

            // Returns the result
            return sResultado;
        }

        #endregion

    }
}
