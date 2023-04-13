using System;
using System.Data;
using ExcelDataReader;
using System.IO;
using Ext.Net;
using log4net;
using System.Reflection;

namespace CapaNegocio
{
    public partial class ExcelController : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        // Global variables     
        static StreamReader lector = null;
        static string sFichero = null;

        #region CONSTRUCTORES

        public ExcelController()
                : base()
        {
            
        }

        public ExcelController(string sNombreFichero, string sExtension)
                : base()
        {
            sFichero = sNombreFichero;
        }

        #endregion

        #region CONEXION        

        private bool Conectar()
        {
            // Local variables
            bool bConectado = false;
            try
            {
                lector = new System.IO.StreamReader(sFichero);

                bConectado = true;


            }
            catch (FileNotFoundException ex)
            {
                log.Error(ex.Message);
            }
            catch (IOException ex)
            {
                log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return bConectado;
        }

        public StreamReader GetLector()
        {
            return lector;
        }

        public void SetLector(StreamReader srLector)
        {
            lector = srLector;
        }

        #endregion       

        #region ARBOL MODELO

        /// <summary>
        /// Obtiene la lista de nodos del arbol convertida a JSON
        /// </summary>
        /// <param name="nodes">Nodo a Obtener</param>
        /// <returns>Cadena en JSON con los nodos</returns>
        public string GetItemsArbolString(NodeCollection nodes)
        {
            string res = "";
            res = GetItemsArbol(nodes).ToJson();
            return res;
        }

        /// <summary>
        /// Devuelve la carga completa de nodos para el árbol
        /// </summary>
        /// <param name="nodes">Estructura de Nodos a Cargar</param>
        /// <returns>Colección de Nodos</returns>
        public NodeCollection GetItemsArbol(NodeCollection nodes)
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
                root.Text = "TREE";
                root.Qtip = "ROOT";
                root.Expanded = true;
                root.IconCls = "icon-folderdatabase";
                root.Expandable = false;

                //TreeNode nodo = new TreeNode();
                //nodo = GetAllTreeNodes(ref root);

                #region CREATES TREE

                try
                {
                    if (Conectar())
                    {
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(GetLector().BaseStream);
                        System.Globalization.NumberFormatInfo prov = new System.Globalization.NumberFormatInfo();
                        prov.NumberDecimalSeparator = ".";

                        DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });

                        // Stores the information
                        int iInitialTab = 0;
                        int numTabs = result.Tables.Count;
                        int k = 0;

                        // Checks all the tabs
                        for (k = iInitialTab; k < numTabs; k = k + 1)
                        {
                            Node nodo = new Node();
                            nodo.NodeID = "CARPETA" + k.ToString();
                            nodo.Expanded = true;
                            nodo.IconCls = "icon-folder";
                            nodo.Text = ProcesarNombreTabla(result.Tables[k].TableName);
                            nodo.Expanded = false;
                            root.Children.Add(nodo);
                            Node rama = null;
                            int iTabla = 0;
                            ConfigItem itemTabla = null;


                            System.Data.DataRow fila = result.Tables[k].Rows[0];
                            for (int j = 0; j < fila.Table.Columns.Count; j = j + 1)
                            {
                                QTipCfg config = new QTipCfg();
                                rama = new Node();
                                rama.NodeID = "CARPETA" + k.ToString() + j.ToString();
                                rama.Expanded = true;
                                rama.IconCls = "icon-table";
                                rama.Text = ProcesarNombreTabla(fila.Table.Columns[j].ToString());
                                rama.Qtip = fila.Table.Columns[j].ToString();
                                itemTabla = new ConfigItem();
                                itemTabla.Name = "data";
                                itemTabla.Value = "\"" + fila.Table.Columns[j].ToString() + "\"";
                                rama.CustomAttributes.Add(itemTabla);
                                rama.Expanded = false;
                                nodo.Children.Add(rama);
                                iTabla = iTabla + 1;
                            }

                        }

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
