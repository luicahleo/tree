using System;
using System.Collections.Generic;
using CapaNegocio;
using Ext.Net;
using System.IO;
using com.mxgraph;
using log4net;
using System.Reflection;
using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using TreeCore.Data;


namespace TreeCore.ModInventario
{
    public partial class InventarioCategoriaDiagrama : TreeCore.Page.BasePageExtNet
    {
        public List<long> Funcionalidades = new List<long>();
        public static long EmplazamientoID = 0;
        public static long AdquisicionSARFID = 0;
        public static long AdquisicionSARFEmplazamientoID = 0;
        public static long InstallTecnicaEmplazamientoID = 0;
        public static string sCodigoEmplazamiento = null;
        public static long pElementoID_Seleccionado = 0;
        public static long EmplazamientoElementoID = 0;
        public static long lTCEmplaID = 0;
        public static bool Templates = false;
        public static string RutaExportacion = "";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        public List<string> tipoValidacionesID = new List<string>();

        #region Gestión Página (Init/Load)

        protected void Page_Load(object sender, EventArgs e)
        {

            ProyectosTiposController cProyTip = new ProyectosTiposController();
            Data.ProyectosTipos ptip = new Data.ProyectosTipos();


            ptip = cProyTip.GetProyectosTiposByNombre(Comun.MODULOINVENTARIO);
            //Util.EscribeEstadistica(Usuario.UsuarioID, ptip.ProyectoTipoID, Request.Url.Segments[Request.Url.Segments.Length - 1], TreeCore.Properties.Settings.Default.Estadistica, "");
            ResourceManager1.Theme = Tema;

            hdInventarioImagenes.Value = "Earth_globe,Empty_Folder";

            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            Data.Emplazamientos emplazmiento = null;
            string sFichero = null;
            mxCodec codec = new mxCodec();
            string sTipoEmplazamiento = cmbTipoEmplazamientos.SelectedItem.Text;
            //1 = Convert.ToInt32(sEmplazamiento);
            /*CheckBox cbxN = new CheckBox();
            cbxN.Text = "Prueba";
            cbxN.Visible = true;
            hdCliID.Value = ClienteID;
            Toolbar2.Controls.Add(cbxN);*/

            List<Data.InventarioTiposVinculaciones> linventarioTiposVinculaciones = new List<Data.InventarioTiposVinculaciones>();
            InventarioTiposVinculacionesController cinventarioTiposVinculaciones = new InventarioTiposVinculacionesController();
            linventarioTiposVinculaciones = cinventarioTiposVinculaciones.GetActivos(Convert.ToInt32(hdCliID.Value));
            if (linventarioTiposVinculaciones.Count > 0)
            {
                if (linventarioTiposVinculaciones != null && linventarioTiposVinculaciones[0] != null)
                {
                    btnToggle.Text = linventarioTiposVinculaciones[0].Nombre;
                    tipoValidacionesID.Add(btnToggle.Text);

                }
                else
                {
                    btnToggle.Hidden = true;
                    btnToggle.Pressed = false;

                }
            }
            if (linventarioTiposVinculaciones.Count > 1)
            {
                if (linventarioTiposVinculaciones != null && linventarioTiposVinculaciones[1] != null)
                {
                    btnToggle1.Text = linventarioTiposVinculaciones[1].Nombre;
                    tipoValidacionesID.Add(btnToggle1.Text);

                }
               
            }
            else
            {
                btnToggle1.Hidden = true;
                btnToggle1.Pressed = false;

            }
            if (linventarioTiposVinculaciones.Count > 2)
            {
                if (linventarioTiposVinculaciones != null && linventarioTiposVinculaciones[2] != null)
                {
                    btnToggle2.Text = linventarioTiposVinculaciones[2].Nombre;
                    tipoValidacionesID.Add(btnToggle2.Text);
                }
                
            }
            else
            {
                btnToggle2.Hidden = true;
                btnToggle2.Pressed = false;

            }
            if (linventarioTiposVinculaciones.Count > 3)
            {
                if (linventarioTiposVinculaciones != null && linventarioTiposVinculaciones[3] != null)
                {
                    btnToggle3.Text = linventarioTiposVinculaciones[3].Nombre;
                    tipoValidacionesID.Add(btnToggle3.Text);
                }
                
            }
            else
            {
                btnToggle3.Hidden = true;
                btnToggle3.Pressed = false;

            }
            if (linventarioTiposVinculaciones.Count > 4)
            {
                if (linventarioTiposVinculaciones != null && linventarioTiposVinculaciones[4] != null)
                {
                    btnToggle4.Text = linventarioTiposVinculaciones[4].Nombre;
                    tipoValidacionesID.Add(btnToggle4.Text);
                }
                
            }
            else
            {
                btnToggle4.Hidden = true;
                btnToggle4.Pressed = false;

            }
            if (linventarioTiposVinculaciones.Count > 5)
            {
                if (linventarioTiposVinculaciones != null && linventarioTiposVinculaciones[5] != null)
                {
                    btnToggle5.Text = linventarioTiposVinculaciones[5].Nombre;
                    tipoValidacionesID.Add(btnToggle5.Text);
                }
                
            }
            else
            {
                btnToggle5.Hidden = true;
                btnToggle5.Pressed = false;

            }
            if (linventarioTiposVinculaciones.Count > 6)
            {
                if (linventarioTiposVinculaciones != null && linventarioTiposVinculaciones[6] != null)
                {
                    btnToggle6.Text = linventarioTiposVinculaciones[6].Nombre;
                    tipoValidacionesID.Add(btnToggle6.Text);

                }
                
            }
            else
            {
                btnToggle6.Hidden = true;
                btnToggle6.Pressed = false;

            }



            string fileName = "";
            fileName = "MF-INV-Global" + ".xml";
            if (sTipoEmplazamiento == null)
            {

                fileName = fileName.Replace(" ", "");
                sFichero = GetXMLFilePath("Global");
                /*if (!System.IO.File.Exists(sFichero))
                {*/
                fileName = CreatesXML().ToString();
                fileName = "MF-INV-Global" + ".xml";
                //}
            }
            else
            {
                fileName = "MF-INV-" + sTipoEmplazamiento + ".xml";
                fileName = fileName.Replace(" ", "");
                sFichero = GetXMLFilePath(sTipoEmplazamiento);
                fileName = CreatesXML().ToString();
                fileName = "MF-INV-" + sTipoEmplazamiento + ".xml";
                fileName = fileName.Replace(" ", "");

            }


            if (fileName != null && fileName != "")
            {
                hdFichero.Value = /*@*/"/documentos/Inventory/xml/" + fileName; //"MF-INV-1.xml";
            }
            else
            {
                hdFichero.Value = "";
            }


        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack & !RequestManager.IsAjaxRequest)
            {
                Funcionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;

                }
                else
                {
                   
                    hdCliID.Value = ClienteID;
                    
                }

                List<Data.InventarioTiposVinculaciones> linventarioTiposVinculaciones = new List<Data.InventarioTiposVinculaciones>();
                InventarioTiposVinculacionesController cinventarioTiposVinculaciones = new InventarioTiposVinculacionesController();
                linventarioTiposVinculaciones = cinventarioTiposVinculaciones.GetActivos(Convert.ToInt32(hdCliID.Value));

                if (linventarioTiposVinculaciones != null && linventarioTiposVinculaciones[0] != null)
                {
                    btnToggle.Text = linventarioTiposVinculaciones[0].Nombre;
                    tipoValidacionesID.Add(btnToggle.Text);

                }
                if (linventarioTiposVinculaciones != null && linventarioTiposVinculaciones[1] != null)
                {
                    btnToggle1.Text = linventarioTiposVinculaciones[1].Nombre;
                    tipoValidacionesID.Add(btnToggle1.Text);

                }
                if (linventarioTiposVinculaciones != null && linventarioTiposVinculaciones[2] != null)
                {
                    btnToggle2.Text = linventarioTiposVinculaciones[2].Nombre;
                    tipoValidacionesID.Add(btnToggle2.Text);
                }
            }
        }

        #endregion

        #region XML

        private string GetXMLFilePath(string tipoEmplazamiento)
        {
            //se define la ruta para el archivo provisional y se crea el directorio de destino si no existe
            string Filepath = /*@*/"/documentos/Inventory/xml/"; //GetXMLDirectoryPath();

            string fileName = "";
            if (tipoEmplazamiento != null)
            {
                fileName = "MF -INV-" + tipoEmplazamiento + ".xml";
                fileName = fileName.Replace(" ", "");
            }
            else
            {
                fileName = "";
            }
            string fullpath = Path.Combine(Filepath, fileName);
            hdFichero.Value = fullpath;

            // Returns the result
            return fullpath;
        }

        /// <summary>
        /// Gets the path to read/write the xml file
        /// </summary>
        /// <returns>XML path</returns>
        private string GetXMLDirectoryPath()
        {
            // Local variables
            string Filepath = null;
            Filepath = TreeCore.DirectoryMapping.GetDocumentDirectory();
            Filepath = Filepath + "\\Inventory\\xml\\";

            // Returns the result
            return Filepath;
        }
        #endregion

        #region CREATION XML FILE
        [DirectMethod()]
        public DirectResponse CreatesXML()
        {
            DirectResponse direct = new DirectResponse();
            // Local variables
            string sNombreFichero = null;
            InventarioCategoriasVinculacionesController cVincula = null;
            InventarioCategoriasController cCategorias = null;
            List<Data.InventarioCategorias> listaElementos = null;
            List<Data.InventarioCategorias> listaElementosCompleta = null;
            List<Data.InventarioCategorias> listaElementosPadre = null;
            List<Data.InventarioCategoriasVinculaciones> listaVincula = null;
            List<long> ListaIdsCompleta = new List<long>();
           
            mxCell celda = null;
            mxCell cImagen = null;
            mxCell cShape = null;
            mxCell root = null;
            mxCell rootImage = null;
            mxCell raiz = null;
            mxCell celdaInterna = null;
            //mxCell capa = null;
            mxGeometry geometria = null;
            mxGraph grafico = null;
            mxGraphModel modelo = null;
            int i = 0;
            int j = 1000;
            int p = 1;
            int y = i;
            int iX = 0;
           
            //int iCategorias = 0;
            Dictionary<string, int> dtCapas = null;
            Dictionary<long, mxCell> dtCeldas = null;
            List<mxCell> listaConexiones = null;
            string sEstilo = null;
            //mxCodec codec = null;


            listaConexiones = new List<mxCell>();
            try
            {
                cCategorias = new InventarioCategoriasController();
                List<Data.InventarioCategoriasVinculaciones> ListaInventarioCategorias = new List<Data.InventarioCategoriasVinculaciones>();
                InventarioCategoriasVinculacionesController cCategoriasVinculacionesController = new InventarioCategoriasVinculacionesController();
                InventarioTiposVinculacionesController cTiposVinculaciones = new InventarioTiposVinculacionesController();
                ListaInventarioCategorias = cCategoriasVinculacionesController.GetVinculacionesFromCategoriaEmplazamientoTipoDiagrama(Convert.ToInt64(cmbTipoEmplazamientos.SelectedItem.Value));
                listaElementosCompleta = cCategorias.GetInventarioCategoriasByTipoEmplazamiento2(Convert.ToInt64(cmbTipoEmplazamientos.SelectedItem.Value), Convert.ToInt64(ClienteID));
                /*List<long> lista = cCategoriasVinculacionesController.GetListTipoVinculacionesDiagrama( "3");
                if (lista != null && lista.Count > 0)
                {
                    cVincula = new InventarioCategoriasVinculacionesController();
                    listaVincula = cVincula.GetListByVinculacionesDiagrama(lista);*/
                //Guardando Lista de Ids
                foreach (Data.InventarioCategoriasVinculaciones categoriaVinculaciones in ListaInventarioCategorias)
                {
                    if (!ListaIdsCompleta.Contains(categoriaVinculaciones.InventarioCategoriaID))
                    {
                        ListaIdsCompleta.Add(categoriaVinculaciones.InventarioCategoriaID);
                    }
                    if (!ListaIdsCompleta.Contains(Convert.ToInt64(categoriaVinculaciones.InventarioCategoriaPadreID)) && categoriaVinculaciones.InventarioCategoriaPadreID != null)
                    {
                        ListaIdsCompleta.Add(Convert.ToInt64(categoriaVinculaciones.InventarioCategoriaPadreID));
                    }
                }
                listaElementosCompleta = cCategorias.GetInventarioCategoriasByTipoEmplazamientoDiagramaPadre(ListaIdsCompleta);
                listaElementos = cCategorias.GetInventarioCategoriasByTipoEmplazamientoDiagrama(cmbTipoEmplazamientos.SelectedItem.Value, Convert.ToInt64(ClienteID));
                if (listaElementos != null && listaElementos.Count > 0)
                {
                    // Creates the root
                    grafico = new mxGraph();
                    grafico.GridSize = 0;
                    //grafico.Stylesheet = 
                    grafico.GridEnabled = false;
                    grafico.View.Scale = 0;
                    grafico.Model.BeginUpdate();

                    raiz = new mxCell();
                    raiz.Id = i.ToString();
                    i = i + 1;
                    root = new mxCell();
                    root.Id = i.ToString();
                    string idRoot = root.Id;
                    root.Parent = raiz;
                    raiz.Insert(root);
                    // Creates the layer
                    dtCapas = new Dictionary<string, int>();
                    dtCeldas = new Dictionary<long, mxCell>();
                    dtCeldas.Add(-1, raiz);
                    dtCeldas.Add(-2, root);
                    rootImage = new mxCell();
                    sEstilo = "image;html=1;image=/ima/infricons/ico-rootInventory.svg;rotatable=0;deletable=0;connectable=0;";
                    rootImage.Style = sEstilo;
                    rootImage.Parent = root;
                    rootImage.Vertex = true;

                    geometria = new mxGeometry();
                    geometria.Width = 40;
                    geometria.Height = 30;
                    geometria.X = 20;
                    geometria.Y = 10;
                    rootImage.Geometry = geometria;
                    /*root.Insert(rootImage);
                   
                    dtCeldas.Add(123, rootImage);*/
                    rootImage.SetAttribute("InventarioCategoriaID", 123.ToString());
                    dtCeldas.Add(123, rootImage);
                    root.Insert(rootImage);

                    List<long> Padres = new List<long>();
                    foreach (Data.InventarioCategorias categoria in listaElementos)
                    {
                        if (ListaIdsCompleta.Contains(categoria.InventarioCategoriaID))
                        {
                            Padres.Add(categoria.InventarioCategoriaID);
                            p++;
                            /////Group
                            celda = new mxCell();
                            j = j + i;
                            celda.Id = "EL-" + j.ToString();
                            //celda.Value = categoria.InventarioCategoria;
                            sEstilo = "group;deletable=0;connectable=0;rotatable=0;";// + categoria.Icono;
                            celda.Style = sEstilo;
                            celda.Parent = rootImage;
                            celda.Vertex = true;
                            geometria = new mxGeometry();
                            geometria.Width = 190;
                            geometria.Height = 40;
                            geometria.X = 150;
                            y = 40 * (i + 1); ;
                            geometria.Y = y;
                            celda.Geometry = geometria;
                            int idCelda = Convert.ToInt32(categoria.InventarioCategoriaID) + i + 100000000;
                            /*cShape.SetAttribute("InventarioCategoriaID", idShape.ToString());
                            dtCeldas.Add(idShape, cShape);*/
                            celda.SetAttribute("InventarioCategoriaID", idCelda.ToString());
                            dtCeldas.Add(idCelda, celda);

                            root.Insert(celda);
                            i++;
                            ///////// Shape
                            ////
                            cShape = new mxCell();
                            cShape.Id = "EL-" + i.ToString();
                            //celda.Value = categoria.InventarioCategoria;
                            sEstilo = "rounded=1;whiteSpace=wrap;html=1;strokeColor=#06A086;fontSize=10;verticalLabelPosition=middle;arcSize=50;fillColor=#FFFFFF;movable=0;editable=0;connectable=0;deletable=0;rotatable=0;resizable=0;";
                            cShape.Style = sEstilo;
                            cShape.Value = categoria.InventarioCategoria;
                            cShape.Parent = celda;
                            cShape.Vertex = true;

                            geometria = new mxGeometry();
                            geometria.X = 10;
                            geometria.Y = 10;
                            geometria.Width = 180;
                            geometria.Height = 26;
                            cShape.Geometry = geometria;
                            cShape.Geometry.X = geometria.X;
                            cShape.Geometry.Y = geometria.Y;
                            cShape.SetAttribute("InventarioCategoriaID", categoria.InventarioCategoria.ToString());
                            dtCeldas.Add(categoria.InventarioCategoriaID, cShape);
                            celda.Insert(cShape);
                            ListaIdsCompleta.Remove(categoria.InventarioCategoriaID);

                            i = i++;



                            cImagen = new mxCell();
                            cImagen.Id = "EL-" + i.ToString();
                            //celda.Value = categoria.InventarioCategoria;
                            sEstilo = "rounded=1;whiteSpace=wrap;html=1;fillColor=#F6F6F6;arcSize=50;strokeColor=none;movable=0;editable=0;connectable=0;deletable=0;rotatable=0;resizable=0;";
                            cImagen.Style = sEstilo;
                            cImagen.Parent = celda;
                            cImagen.Vertex = true;

                            geometria = new mxGeometry();
                            geometria.Width = 28;
                            geometria.Height = 24;
                            geometria.X = 11;
                            geometria.Y = 11;
                            cImagen.Geometry = geometria;

                            int idI = Convert.ToInt32(categoria.InventarioCategoriaID) + j * 15987;

                            cImagen.SetAttribute("InventarioCategoriaID", idI.ToString());
                            dtCeldas.Add(idI, cImagen);
                            celda.Insert(cImagen);
                            i = i++;
                            /////
                            ///
                            /////// Imagen
                            ///


                            cImagen = new mxCell();
                            cImagen.Id = "EL-" + i.ToString();
                            //celda.Value = categoria.InventarioCategoria;
                            sEstilo = "image;html=1;image=/ima/infricons/" + categoria.Icono + ";imageBackground=#F6F6F6;arcSize=50;movable=0;resizable=0;rotatable=0;deletable=0;editable=1;connectable=0;";
                            cImagen.Style = sEstilo;
                            cImagen.Parent = celda;
                            cImagen.Vertex = true;

                            geometria = new mxGeometry();
                            geometria.Width = 20;
                            geometria.Height = 16;
                            geometria.X = 15;
                            geometria.Y = 15;
                            cImagen.Geometry = geometria;

                            int id = Convert.ToInt32(categoria.InventarioCategoriaID) + j;

                            cImagen.SetAttribute("InventarioCategoriaID", id.ToString());
                            dtCeldas.Add(id, cImagen);
                            celda.Insert(cImagen);
                            i = i++;

                            celdaInterna = new mxCell();
                            celdaInterna.Id = "XC-" + i.ToString();
                            //sEstilo = "rounded=1;startArrow=oval;endArrow=oval;startFill=1;endFill=0;deletable=0;movable=0;editable=0;entryX=0;entryY=0.5;entryDx=0;entryDy=0;exitX=1;exitY=0.5;exitDx=0;exitDy=0;";
                            //sEstilo = "curved=1;startArrow=oval;endArrow=oval;startFill=1;endFill=0;deletable=0;movable=0;editable=0;entryX=0;entryY=0;entryDx=0;entryDy=0;exitX=0;exitY=0;exitDx=0;exitDy=0;";
                            sEstilo = "curved=1;startArrow=oval;startFill=1;endArrow=classic;html=1;edgeStyle=entityRelationEdgeStyle;elbow=vertical;jettySize=auto;deletable=0;movable=0;editable=0;";
                            celdaInterna.Style = sEstilo;
                            celdaInterna.Parent = root;
                            celdaInterna.Edge = true;
                            geometria = new mxGeometry();
                            geometria.Relative = true;
                            celdaInterna.Geometry = geometria;
                            celdaInterna.Source = dtCeldas[123];
                            celdaInterna.Target = dtCeldas[(long)categoria.InventarioCategoriaID];
                            celda.Insert(celdaInterna);
                            listaConexiones.Add(celdaInterna);

                        }



                    }


                    ////Revisando categorias hijas
                    List<long> listaDatos = null;
                    listaDatos = cCategorias.GetListByTipoEmplazamientoDiagramaPadre(cmbTipoEmplazamientos.SelectedItem.Value, Convert.ToInt64(ClienteID), Padres);
                    while (listaDatos.Count > 0)
                    {
                        listaElementosPadre = cCategorias.GetInventarioCategoriasByTipoEmplazamientoDiagramaPadre(listaDatos);
                        Padres = new List<long>();
                        foreach (Data.InventarioCategorias categoria in listaElementosPadre)
                        {

                            if (ListaIdsCompleta.Contains(categoria.InventarioCategoriaID))
                            {
                                Padres.Add(categoria.InventarioCategoriaID);
                                /////Group
                                celda = new mxCell();
                                j = j + i;
                                celda.Id = "EL-" + j.ToString();
                                sEstilo = "group;deletable=0;connectable=0;rotatable=0;editable=0;";// + categoria.Icono;
                                celda.Style = sEstilo;
                                celda.Parent = root;
                                celda.Vertex = true;
                                geometria = new mxGeometry();
                                geometria.Width = 190;
                                geometria.Height = 40;
                                geometria.X = 450 + (iX * 250);
                                y = 40 * (i + 1);
                                geometria.Y = y;
                                celda.Geometry = geometria;
                                int idCelda = Convert.ToInt32(categoria.InventarioCategoriaID) + 100000000 + 87562;
                                celda.SetAttribute("InventarioCategoriaID", idCelda.ToString());
                                dtCeldas.Add(idCelda, celda);
                                root.Insert(celda);
                                i++;

                                /////
                                ///////// Shape
                                ////
                                cShape = new mxCell();
                                cShape.Id = "EL-" + i.ToString();
                                sEstilo = "rounded=1;whiteSpace=wrap;html=1;strokeColor=#06A086;fontSize=10;verticalLabelPosition=middle;arcSize=50;fillColor=#FFFFFF;movable=0;editable=0;connectable=0;deletable=0;rotatable=0;resizable=0;";
                                cShape.Style = sEstilo;
                                cShape.Value = categoria.InventarioCategoria;
                                cShape.Parent = celda;
                                cShape.Vertex = true;
                                geometria = new mxGeometry();
                                geometria.X = 10;
                                geometria.Y = 10;
                                geometria.Width = 180;
                                geometria.Height = 26;
                                cShape.Geometry = geometria;
                                cShape.Geometry.X = geometria.X;
                                cShape.Geometry.Y = geometria.Y;
                                cShape.SetAttribute("InventarioCategoriaID", categoria.InventarioCategoria.ToString());
                                dtCeldas.Add(categoria.InventarioCategoriaID, cShape);
                                celda.Insert(cShape);
                                ListaIdsCompleta.Remove(categoria.InventarioCategoriaID);

                                cImagen = new mxCell();
                                cImagen.Id = "EL-" + i.ToString();
                                //celda.Value = categoria.InventarioCategoria;
                                sEstilo = "rounded=1;whiteSpace=wrap;html=1;fillColor=#F6F6F6;arcSize=50;strokeColor=none;movable=0;editable=0;connectable=0;deletable=0;rotatable=0;resizable=0;";
                                cImagen.Style = sEstilo;
                                cImagen.Parent = celda;
                                cImagen.Vertex = true;

                                geometria = new mxGeometry();
                                geometria.Width = 28;
                                geometria.Height = 24;
                                geometria.X = 11;
                                geometria.Y = 11;
                                cImagen.Geometry = geometria;

                                int idI = Convert.ToInt32(categoria.InventarioCategoriaID) + j * 98562;

                                cImagen.SetAttribute("InventarioCategoriaID", idI.ToString());
                                dtCeldas.Add(idI, cImagen);
                                celda.Insert(cImagen);
                                i = i++;

                                /////// Imagen
                                ///
                                cImagen = new mxCell();
                                cImagen.Id = "EL-" + i.ToString();
                                sEstilo = "image;html=1;image=/ima/infricons/" + categoria.Icono + ";imageBackground=#F6F6F6;movable=0;resizable=0;rotatable=0;deletable=0;editable=1;connectable=0;";
                                cImagen.Style = sEstilo;
                                cImagen.Parent = celda;
                                cImagen.Vertex = true;
                                geometria = new mxGeometry();
                                geometria.Width = 20;
                                geometria.Height = 20;
                                geometria.X = 15;
                                geometria.Y = 15;
                                cImagen.Geometry = geometria;
                                int id = Convert.ToInt32(categoria.InventarioCategoriaID) + j + 2000389;
                                cImagen.SetAttribute("InventarioCategoriaID", id.ToString());
                                dtCeldas.Add(id, cImagen);
                                celda.Insert(cImagen);
                                i = i++;

                            }
                            i = i++;

                            ///////
                        }
                        iX++;
                        listaDatos = cCategorias.GetListByTipoEmplazamientoDiagramaPadre(cmbTipoEmplazamientos.SelectedItem.Value, Convert.ToInt64(ClienteID), Padres);
                        if (!(listaDatos.Count > 0))
                        {
                            listaDatos = cCategorias.GetListByTipoEmplazamientoDiagramaPadre(cmbTipoEmplazamientos.SelectedItem.Value, Convert.ToInt64(ClienteID), ListaIdsCompleta);
                            if ((listaDatos.Count == 0) && (ListaIdsCompleta.Count > 0))
                            {
                                listaDatos = ListaIdsCompleta;

                                //c = 0;
                            }
                        }

                    }

                    List<long> TiposVinculaciones = new List<long>();
                    long idtiposVinculaciones;
                    if (btnToggle.Pressed == true)
                    {
                        idtiposVinculaciones = cTiposVinculaciones.getidTipoVinculaciones(tipoValidacionesID[0], Convert.ToInt64(ClienteID));
                        TiposVinculaciones.Add(idtiposVinculaciones);
                    }
                    if (btnToggle1.Pressed == true)
                    {
                        idtiposVinculaciones = cTiposVinculaciones.getidTipoVinculaciones(tipoValidacionesID[1], Convert.ToInt64(ClienteID));
                        TiposVinculaciones.Add(idtiposVinculaciones);
                    }
                    if (btnToggle2.Pressed == true)
                    {
                        idtiposVinculaciones = cTiposVinculaciones.getidTipoVinculaciones(tipoValidacionesID[2], Convert.ToInt64(ClienteID));
                        TiposVinculaciones.Add(idtiposVinculaciones);
                    }
                   // foreach (Data.InventarioCategorias elemento in listaElementosCompleta) //Imprime las flechas
                    //{
                        cVincula = new InventarioCategoriasVinculacionesController();
                        
                        listaDatos = cVincula.GetListByTipoEmplazamientoTipoVinculacionesDiagrama(cmbTipoEmplazamientos.SelectedItem.Value, Convert.ToInt64(ClienteID), TiposVinculaciones);
                        //listaVincula = cVincula.GetVinculacionesFromCategoriaEmplazamientoTipo(elemento.InventarioCategoriaID, Convert.ToInt32(cmbTipoEmplazamientos.SelectedItem.Value));
                        listaVincula = cVincula.GetListByVinculacionesDiagrama(listaDatos);
                            if (listaVincula != null && listaVincula.Count > 0)
                        {

                            foreach (Data.InventarioCategoriasVinculaciones vincula in listaVincula)
                            {
                                if (vincula.InventarioCategoriaPadreID != null)
                                {
                                    celdaInterna = new mxCell();
                                    celdaInterna.Id = "XC-" + i.ToString();

                                    celdaInterna.Parent = root;
                                    celdaInterna.Edge = true;
                                    geometria = new mxGeometry();
                                    geometria.Relative = true;
                                    celdaInterna.Geometry = geometria;
                                    celdaInterna.Source = dtCeldas[(long)vincula.InventarioCategoriaPadreID];
                                    celdaInterna.Target = dtCeldas[(long)vincula.InventarioCategoriaID];
                                    /*if (celdaInterna.Source == celdaInterna.Target)
                                    {*/
                                    //sEstilo = "rounded=1;startArrow=oval;endArrow=oval;startFill=1;endFill=0;deletable=0;movable=0;editable=0;entryX=0;entryY=0.5;entryDx=0;entryDy=0;exitX=1;exitY=0.5;exitDx=0;exitDy=0; "; 
                                    //sEstilo = "rounded=1;startArrow=oval;endArrow=oval;startFill=1;endFill=0;deletable=0;movable=0;editable=0;";
                                    sEstilo = "curved=1;startArrow=oval;startFill=1;endArrow=classic;html=1;edgeStyle=elbowEdgeStyle;exitX=1;exitY=0.5;exitDx=0;exitDy=0;jumpStyle=arc;deletable=0;editable=0;";
                                    //sEstilo = "startArrow=oval;startFill=0;endArrow=block;html=1;edgeStyle=segmentEdgeStyle;orthogonalLoop=1;elbow=horizontal;deletable=0;movable=0;editable=0;exitX=1;exitY=0.5;exitDx=0;exitDy=0;entryX=0;entryY=0;entryDx=0;entryDy=0;";
                                    /*}
                                    else
                                    {
                                        //sEstilo = "curved=1;startArrow=oval;endArrow=block;html=1;edgeStyle=entityRelationEdgeStyle;elbow=vertical;deletable=0;movable=0;editable=0;"; 
                                        sEstilo = "startArrow=oval;startFill=0;endArrow=block;html=1;edgeStyle=segmentEdgeStyle;elbow=horizontal;deletable=0;movable=0;editable=0;exitX=1;exitY=0.5;exitDx=0;exitDy=0;entryX=0;entryY=0;entryDx=0;entryDy=0;";
                                    }*/
                                    celdaInterna.Style = sEstilo;
                                    celda.Insert(celdaInterna);
                                    listaConexiones.Add(celdaInterna);
                                    i = i + 1;
                                }
                            }
                        }

                    
                    modelo = new mxGraphModel(raiz);
                    grafico = new mxGraph(modelo);

                    // Creates the file
                    string Filepath = "";
                    string sDiagrama = null;
                    /*if (Properties.Settings.Default.RutaDocumentos[2] == '\\')
                    {
                        Filepath = "documentos";//Properties.Settings.Default.RutaDocumentos;
                    }
                    else
                    {*/
                    Filepath = Server.MapPath("~/" + "documentos"); //Properties.Settings.Default.RutaDocumentos) ;
                    //}

                    Filepath = Filepath + "\\Inventory\\xml\\";

                    string fileName = "";
                    if (Convert.ToInt32(cmbTipoEmplazamientos.SelectedItem.Value) > 0)
                    {
                        fileName = "MF-INV-" + cmbTipoEmplazamientos.SelectedItem.Text + ".xml";
                        fileName = fileName.Replace(" ", "");
                    }
                    else
                    {
                        fileName = "MF-INV-Global.xml";
                    }
                    string fullpath = Path.Combine(Filepath, fileName);// hdFichero.Value.ToString(); // 

                    if (!System.IO.Directory.Exists(Filepath))
                    { System.IO.Directory.CreateDirectory(Filepath); }

                    if (System.IO.File.Exists(fullpath))
                    {
                        System.IO.File.Delete(fullpath);
                    }
                    MyFlowController cFlow = new MyFlowController();
                    sDiagrama = cFlow.Encode(grafico, dtCeldas, listaConexiones);

                    File.WriteAllText(fullpath, sDiagrama);
                    sNombreFichero = fileName;
                    sNombreFichero = sNombreFichero.Replace(" ", "");
                    direct.Success = true;
                    direct.Result = sNombreFichero;
                }
                else
                {

                    string Filepath = Server.MapPath("~/" + "documentos"); //Properties.Settings.Default.RutaDocumentos) ;
                    //}

                    Filepath = Filepath + "\\Inventory\\xml\\";

                    string fileName = "";
                    if (Convert.ToInt32(cmbTipoEmplazamientos.SelectedItem.Value) > 0)
                    {
                        fileName = "MF-INV-" + cmbTipoEmplazamientos.SelectedItem.Text + ".xml";
                        fileName = fileName.Replace(" ", "");
                    }
                    else
                    {
                        fileName = "MF-INV-Global.xml";
                    }
                    string fullpath = Path.Combine(Filepath, fileName);// hdFichero.Value.ToString(); // 

                    if (!System.IO.Directory.Exists(Filepath))
                    { System.IO.Directory.CreateDirectory(Filepath); }

                    if (System.IO.File.Exists(fullpath))
                    {
                        System.IO.File.Delete(fullpath);
                    }
                    MyFlowController cFlow = new MyFlowController();
                    string sDiagrama = cFlow.Encode(grafico, dtCeldas, listaConexiones);

                    File.WriteAllText(fullpath, sDiagrama);
                    sNombreFichero = fileName;
                    sNombreFichero = sNombreFichero.Replace(" ", "");
                    direct.Success = true;
                    direct.Result = sNombreFichero;
                }
           /* }
                else
                {
                    string Filepath = Server.MapPath("~/" + "documentos"); //Properties.Settings.Default.RutaDocumentos) ;
                    //}

                    Filepath = Filepath + "\\Inventory\\xml\\";

                    string fileName = "";
                    if (Convert.ToInt32(cmbTipoEmplazamientos.SelectedItem.Value) > 0)
                    {
                        fileName = "MF-INV-" + cmbTipoEmplazamientos.SelectedItem.Text + ".xml";
                        fileName = fileName.Replace(" ", "");
                    }
                    else
                    {
                        fileName = "MF-INV-Global.xml";
                    }
                    string fullpath = Path.Combine(Filepath, fileName);// hdFichero.Value.ToString(); // 

                    if (!System.IO.Directory.Exists(Filepath))
                    { System.IO.Directory.CreateDirectory(Filepath); }

                    if (System.IO.File.Exists(fullpath))
                    {
                        System.IO.File.Delete(fullpath);
                    }
                    MyFlowController cFlow = new MyFlowController();
                    string sDiagrama = cFlow.Encode(grafico, dtCeldas, listaConexiones);

                    File.WriteAllText(fullpath, sDiagrama);
                    sNombreFichero = fileName;
                    sNombreFichero = sNombreFichero.Replace(" ", "");
                    direct.Success = true;
                    direct.Result = sNombreFichero;
                }*/
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sNombreFichero = null;
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }

            // Returns the result
            return direct;
        }


        #endregion

        #region  Store
        protected void storeTipoEmplazamientos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                EmplazamientosTiposController cEmplazamientosTipos = new EmplazamientosTiposController();
                try
                {

                    var lista = cEmplazamientosTipos.GetEmplazamientosTiposActivos(long.Parse(hdCliID.Value.ToString()));

                    if (lista != null)
                    {
                        storeTipoEmplazamientos.DataSource = lista;
                        storeTipoEmplazamientos.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        
        #endregion
    }
}

