using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.mxgraph;
using CapaNegocio;
using System.Web.UI.WebControls;
using Ext.Net;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace CapaNegocio
{
    public class MyFlowController
    {
        #region CONSTRUCTOR

        public MyFlowController()
        {

        }

        #endregion

        #region CREACION DE FLUJOS

        public bool CreateWorkFlow(string proyectoTipo, int tipoFlujoID, long tipologiaID, mxGraph grafico, List<mxCell> lista)
        {
            // Local variables
            bool bResultado = false;
            string sID = null;
            string sEstilo = null;
            string sNombre = null;
            List<mxCell> listaCeldas = null;
            List<mxCell> listaEstados = null;
            int iOrden = 0;

            switch (proyectoTipo)
            {
                /*case Comun.MODULOGIS:

                    #region GIS

                    #region DBI

                    if (tipoFlujoID == Comun.TIPOFLUJO_FTTH_DBI)
                    {
                        List<GISDBIEstados> listaEstadosTREE = null;
                        GISDBIEstadosController cGISDBIEstados = new GISDBIEstadosController();
                        GISDBIEstadosController cGISDBISiguientes = new GISDBIEstadosController();
                        GISDBIEstados estado = null;
                        GISDBIEstados siguiente = null;

                        if (lista != null && lista.Count > 0)
                        {
                            listaEstados = new List<mxCell>();
                            foreach (mxCell celda in lista)
                            {
                                sID = celda.Id;
                                sEstilo = celda.Style;
                                if (sEstilo.Contains("shape=ext"))
                                {
                                    listaEstados.Add(celda);
                                }
                            }
                        }

                        #region STATE CREATION

                        // Create the basic states
                        if (listaEstados != null && listaEstados.Count > 0)
                        {
                            listaEstadosTREE = new List<GISDBIEstados>();
                            foreach (mxCell celdaEstados in listaEstados)
                            {
                                iOrden = iOrden + 1;
                                estado = new GISDBIEstados();
                                estado.Agrupacion = celdaEstados.GetAttribute("Agrupacion");
                                estado.Agrupacion_enUS = celdaEstados.GetAttribute("Agrupacion");
                                estado.Agrupacion_esES = celdaEstados.GetAttribute("Agrupacion");
                                estado.Agrupacion_frFR = celdaEstados.GetAttribute("Agrupacion");
                                if (celdaEstados.GetAttribute("AplicaDevengo") != null && celdaEstados.GetAttribute("AplicaDevengo") != "")
                                {
                                    estado.AplicaDevengo = Convert.ToBoolean(celdaEstados.GetAttribute("AplicaDevengo"));
                                }
                                else
                                {
                                    estado.AplicaDevengo = false;
                                }
                                if (celdaEstados.GetAttribute("AplicaPago") != null && celdaEstados.GetAttribute("AplicaPago") != "")
                                {
                                    estado.AplicaPago = Convert.ToBoolean(celdaEstados.GetAttribute("AplicaPago"));
                                }
                                else
                                {
                                    estado.AplicaPago = false;
                                }
                                estado.Codigo = celdaEstados.GetAttribute("Codigo");

                                if (celdaEstados.GetAttribute("ColorNaranja") != null && celdaEstados.GetAttribute("ColorNaranja") != "")
                                {
                                    estado.ColorNaranja = Convert.ToInt32(celdaEstados.GetAttribute("ColorNaranja"));
                                }
                                else
                                {
                                    estado.ColorNaranja = 10;
                                }

                                if (celdaEstados.GetAttribute("ColorRojo") != null && celdaEstados.GetAttribute("ColorRojo") != "")
                                {
                                    estado.ColorRojo = Convert.ToInt32(celdaEstados.GetAttribute("ColorRojo"));
                                }
                                else
                                {
                                    estado.ColorRojo = 10;
                                }

                                if (celdaEstados.GetAttribute("Completado") != null && celdaEstados.GetAttribute("Completado") != "")
                                {
                                    estado.Completado = Convert.ToBoolean(celdaEstados.GetAttribute("Completado"));
                                }
                                else
                                {
                                    estado.Completado = false;
                                }
                                estado.CorreoContenido = celdaEstados.GetAttribute("CorreoContenido");
                                estado.CorreoLista = celdaEstados.GetAttribute("CorreoLista");

                                if (celdaEstados.GetAttribute("Defecto") != null && celdaEstados.GetAttribute("Defecto") != "")
                                {
                                    estado.Defecto = Convert.ToBoolean(celdaEstados.GetAttribute("Defecto"));
                                }
                                else
                                {
                                    estado.Defecto = false;
                                }

                                if (celdaEstados.GetAttribute("DepartamentoID") != null && celdaEstados.GetAttribute("DepartamentoID") != "")
                                {
                                    estado.DepartamentoID = Convert.ToInt32(celdaEstados.GetAttribute("DepartamentoID"));
                                }

                                if (celdaEstados.GetAttribute("Editable") != null && celdaEstados.GetAttribute("Editable") != "")
                                {
                                    estado.Editable = Convert.ToBoolean(celdaEstados.GetAttribute("Editable"));
                                }
                                else
                                {
                                    estado.Editable = false;
                                }

                                if (celdaEstados.GetAttribute("Enviar") != null && celdaEstados.GetAttribute("Enviar") != "")
                                {
                                    estado.Enviar = Convert.ToBoolean(celdaEstados.GetAttribute("Enviar"));
                                }
                                else
                                {
                                    estado.Enviar = false;
                                }

                                if (celdaEstados.GetAttribute("EstadoGlobalID") != null && celdaEstados.GetAttribute("EstadoGlobalID") != "")
                                {
                                    estado.EstadoGlobalID = Convert.ToInt32(celdaEstados.GetAttribute("EstadoGlobalID"));
                                }

                                if (celdaEstados.GetAttribute("EstadoPadreID") != null && celdaEstados.GetAttribute("EstadoPadreID") != "")
                                {
                                    estado.EstadoPadreID = Convert.ToInt32(celdaEstados.GetAttribute("EstadoPadreID"));
                                }

                                if (celdaEstados.GetAttribute("FuncionalidadID") != null && celdaEstados.GetAttribute("FuncionalidadID") != "")
                                {
                                    estado.FuncionalidadID = Convert.ToInt32(celdaEstados.GetAttribute("FuncionalidadID"));
                                    estado.TieneFuncionalidad = true;
                                }
                                else
                                {
                                    estado.TieneFuncionalidad = false;
                                }

                                if (celdaEstados.Value != null)
                                {
                                    estado.GISDBIEstado_esES = celdaEstados.Value.ToString();
                                    estado.GISDBIEstado_enUS = celdaEstados.Value.ToString();
                                    estado.GISDBIEstado_frFR = celdaEstados.Value.ToString();
                                }

                                if (celdaEstados.GetAttribute("GISEstadoSLAID") != null && celdaEstados.GetAttribute("GISEstadoSLAID") != "")
                                {
                                    estado.GISEstadoSLAID = Convert.ToInt32(celdaEstados.GetAttribute("GISEstadoSLAID"));
                                }
                                estado.GISTipologiaID = tipologiaID;

                                if (celdaEstados.GetAttribute("GlobalLimiteID") != null && celdaEstados.GetAttribute("GlobalLimiteID") != "")
                                {
                                    estado.GlobalLimiteID = Convert.ToInt32(celdaEstados.GetAttribute("GlobalLimiteID"));
                                }


                                estado.IconoMapa = celdaEstados.GetAttribute("IconoMapa");
                                estado.IconoMenu = celdaEstados.GetAttribute("IconoMenu");

                                if (celdaEstados.GetAttribute("InicioPresupuesto") != null && celdaEstados.GetAttribute("InicioPresupuesto") != "")
                                {
                                    estado.InicioPresupuesto = Convert.ToBoolean(celdaEstados.GetAttribute("InicioPresupuesto"));
                                }
                                else
                                {
                                    estado.InicioPresupuesto = false;
                                }

                                if (celdaEstados.GetAttribute("InventarioElementoAtributoEstadoID") != null && celdaEstados.GetAttribute("InventarioElementoAtributoEstadoID") != "")
                                {
                                    estado.InventarioElementoAtributoEstadoID = Convert.ToInt32(celdaEstados.GetAttribute("InventarioElementoAtributoEstadoID"));
                                }

                                if (celdaEstados.GetAttribute("Limite") != null && celdaEstados.GetAttribute("Limite") != "")
                                {
                                    estado.Limite = Convert.ToBoolean(celdaEstados.GetAttribute("Limite"));
                                }
                                else
                                {
                                    estado.Limite = false;
                                }
                                estado.LiteralMenu = celdaEstados.GetAttribute("LiteralMenu");

                                if (celdaEstados.GetAttribute("MuestraPresupuesto") != null && celdaEstados.GetAttribute("MuestraPresupuesto") != "")
                                {
                                    estado.MuestraPresupuesto = Convert.ToBoolean(celdaEstados.GetAttribute("MuestraPresupuesto"));
                                }
                                else
                                {
                                    estado.MuestraPresupuesto = false;
                                }

                                if (celdaEstados.GetAttribute("NotificaAgencia") != null && celdaEstados.GetAttribute("NotificaAgencia") != "")
                                {
                                    estado.NotificaAgencia = Convert.ToBoolean(celdaEstados.GetAttribute("NotificaAgencia"));
                                }
                                else
                                {
                                    estado.NotificaAgencia = false;
                                }

                                estado.Orden = iOrden;

                                if (celdaEstados.GetAttribute("Porcentaje") != null && celdaEstados.GetAttribute("Porcentaje") != "")
                                {
                                    estado.Porcentaje = Convert.ToInt32(celdaEstados.GetAttribute("Porcentaje"));
                                }
                                else
                                {
                                    estado.Porcentaje = 0;
                                }

                                if (celdaEstados.GetAttribute("RequierePoligono") != null && celdaEstados.GetAttribute("RequierePoligono") != "")
                                {
                                    estado.RequierePoligono = Convert.ToBoolean(celdaEstados.GetAttribute("RequierePoligono"));
                                }
                                else
                                {
                                    estado.RequierePoligono = false;
                                }

                                if (celdaEstados.GetAttribute("Visible") != null && celdaEstados.GetAttribute("Visible") != "")
                                {
                                    estado.Visible = Convert.ToBoolean(celdaEstados.GetAttribute("Visible"));
                                }
                                else
                                {
                                    estado.Visible = true;
                                }

                                estado = cGISDBIEstados.AddItem(estado);
                                listaEstadosTREE.Add(estado);
                            }

                        }

                        #endregion

                        #region NEXT STATES

                        if (listaEstadosTREE != null && listaEstadosTREE.Count > 0)
                        {
                            GISDBIEstadosSiguientesController cSiguientes = new GISDBIEstadosSiguientesController();
                            GISDBIEstadosSiguientes sig = null;
                            int k = 0;
                            for (k = 0; k < listaEstados.Count; k = k + 1)
                            {
                                listaCeldas = GetNextStates(listaEstados.ElementAt(k).Id, lista);
                                if (listaCeldas != null && listaCeldas.Count > 0)
                                {
                                    estado = listaEstadosTREE.ElementAt(k);
                                    foreach (mxCell celda in listaCeldas)
                                    {
                                        sNombre = celda.Value.ToString();
                                        siguiente = cGISDBISiguientes.GetEstadoByCodigov2(sNombre, tipologiaID);
                                        if (siguiente != null)
                                        {
                                            sig = new GISDBIEstadosSiguientes();
                                            sig.Defecto = false;
                                            sig.GISDBIEstadoID = estado.GISDBIEstadoID;
                                            sig.GISDBIEstadoPosibleID = siguiente.GISDBIEstadoID;
                                            sig.Limite = false;
                                            sig.LimiteNoCumple = false;
                                            sig.LimiteSiCumple = false;
                                            sig.SoloAdministrador = false;
                                            cSiguientes.AddItem(sig);
                                        }
                                    }

                                }
                            }
                        }

                        #endregion
                    }

                    #endregion

                    #region TRAMOS

                    else if (tipoFlujoID == Comun.TIPOFLUJO_FTTH_TRAMOS)
                    {

                    }

                    #endregion

                    #region TAREAS

                    else if (tipoFlujoID == Comun.TIPOFLUJO_FTTH_TAREAS)
                    {

                    }

                    #endregion

                    #region NO FLOW

                    else
                    {

                    }

                    #endregion

                    #endregion
            
                    break;*/
            }


            // Returns the result
            return bResultado;

        }

        /// <summary>
        /// Recursive function to find the next states
        /// </summary>
        /// <param name="lista">Cell list</param>
        /// <returns></returns>
        private List<mxCell> GetNextStates(string sID, List<mxCell> lista)
        {
            // Local variables
            List<mxCell> listaCeldas = null;
            List<mxCell> listaResultado = null;
            string sIDLocal = null;
            string sEstilo = null;

            try
            {
                if (sID != null)
                {
                    listaCeldas = (from c in lista
                                   where c.Target.Id == sID
                                   select c
                                        ).ToList();
                    if (listaCeldas != null && listaCeldas.Count > 0)
                    {
                        foreach (mxCell celda in listaCeldas)
                        {
                            sIDLocal = celda.Id;
                            sEstilo = celda.Style;
                            if (sEstilo.Contains("shape=ext"))
                            {
                                listaResultado.Add(celda);
                            }
                            else
                            {
                                listaResultado.AddRange(GetNextStates(sIDLocal, lista));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message);
            }

            // Returns the result
            return listaResultado;
        }

        #endregion

        #region CODIFICACION

        public string Encode(mxGraph grafico, Dictionary<long, mxCell> dtCeldas, List<mxCell> lista)
        {
            // Local variables
            string sRes = null;
            mxCell celda = null;

            if (grafico != null)
            {
                sRes = "<mxGraphModel dx=\"0\" dy=\"0\" grid=\"0\" gridSize=\"10\" guides=\"1\" tooltips=\"1\" connect=\"1\" arrows=\"1\" fold=\"1\" page=\"0\" pageScale=\"0\" pageView=\"0\"  pageWidth=\"827\" pageHeight=\"1169\"    >" + Environment.NewLine;
                sRes = sRes + "<root>" + Environment.NewLine;
                if (dtCeldas != null && dtCeldas.Count > 0)
                {
                    foreach (long key in dtCeldas.Keys)
                    {
                        celda = dtCeldas[key];
                        sRes = sRes + "<mxCell id=\"" + celda.Id + "\" ";
                        if (celda.Value != null && celda.Value.ToString() != "")
                        {
                            sRes = sRes + "value=\"" + celda.Value + "\" ";
                        }
                        if (celda.Style != null && celda.Style != "")
                        {
                            sRes = sRes + "style=\"" + celda.Style + "\" ";
                        }
                        if (celda.Parent != null)
                        {
                            sRes = sRes + "parent=\"" + celda.Parent.Id + "\" ";
                        }
                        if (celda.Vertex)
                        {
                            sRes = sRes + "vertex=\"1\" ";
                        }

                        sRes = sRes + ">";
                        if (celda.Vertex)
                        {
                            sRes = sRes + "<mxGeometry  x=\"" + celda.Geometry.X + "\" y=\"" + celda.Geometry.Y + "\" width =\"" + celda.Geometry.Width + "\" height=\"" + celda.Geometry.Height +  "\" as=\"geometry\" />" + Environment.NewLine;
                        }
                        sRes = sRes + "</mxCell>";
                    }
                }
                if (lista != null && lista.Count > 0)
                {
                    foreach (mxCell conector in lista)
                    {
                        sRes = sRes + "<mxCell id=\"" + conector.Id + "\" ";
                        if (conector.Value != null && conector.Value.ToString() != "")
                        {
                            sRes = sRes + "value=\"" + conector.Value + "\" ";
                        }
                        if (conector.Style != null && conector.Style != "")
                        {
                            sRes = sRes + "style=\"" + conector.Style + "\" ";
                        }
                        if (conector.Parent != null)
                        {
                            sRes = sRes + "parent=\"" + conector.Parent.Id + "\" ";
                        }
                        if (conector.Edge)
                        {
                            sRes = sRes + "edge=\"1\" ";
                        }
                        if (conector.Source != null)
                        {
                            sRes = sRes + "source=\"" + conector.Source.Id + "\" ";
                        }
                        if (conector.Target != null)
                        {
                            sRes = sRes + "target=\"" + conector.Target.Id + "\" ";
                        }


                        sRes = sRes + ">";
                        if (celda.Vertex)
                        {
                            sRes = sRes + "<mxGeometry relative=\"1\" as=\"geometry\"/>" + Environment.NewLine;
                        }
                        sRes = sRes + "</mxCell>" + Environment.NewLine;
                    }
                }
                // Ends the coding
                sRes = sRes + "</root>" + Environment.NewLine;
                sRes = sRes + "</mxGraphModel>" + Environment.NewLine;
            }

            // Returns the result
            return sRes;
        }

        #endregion

        #region SAVE DIAGRAM

        [DirectMethod()]
        public DirectResponse SaveDiagram(string sDiagrama)
        {

            DirectResponse direct = new DirectResponse();

            try
            {
                #region FILE SAVING

                //se define la ruta para el archivo provisional y se crea el directorio de destino si no existe
                string Filepath = GetXMLDirectoryPath();

                string fullpath = "";;// GetXMLFilePath(lEmplazamientoID);


                if (!System.IO.Directory.Exists(Filepath))
                { System.IO.Directory.CreateDirectory(Filepath); }

                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }

                File.WriteAllText(fullpath, sDiagrama);

                #endregion

                #region UPDATES THE FLOW

                mxCodec codec = new mxCodec();
                // mxGraph grafico = null;
                mxCell celda = null;
                List<mxCell> listaCeldas = null;
                XmlDocument doc = mxUtils.ParseXml(sDiagrama);
                XmlNodeList listaNodos = doc.ChildNodes;
                if (listaNodos != null && listaNodos.Count > 0)
                {
                    listaCeldas = new List<mxCell>();

                    XmlNode nodo = listaNodos[0];

                    if (nodo != null)
                    {
                        doc = mxUtils.ParseXml(nodo.InnerXml);
                        listaNodos = doc.ChildNodes;
                        if (listaNodos != null && listaNodos.Count > 0)
                        {
                            XmlNode raiz = listaNodos[0];

                            if (raiz != null)
                            {
                                //doc = mxUtils.ParseXml(raiz.InnerXml);
                                //listaNodos = doc.ChildNodes;
                                if (raiz.ChildNodes != null && raiz.ChildNodes.Count > 0)
                                {
                                    foreach (XmlNode forma in raiz.ChildNodes)
                                    {
                                        celda = (mxCell)codec.Decode(forma);
                                        listaCeldas.Add(celda);
                                    }
                                }
                            }
                        }
                    }

                }


                XmlSerializer serializer = new XmlSerializer(typeof(mxGraphModel));
                StringReader rdr = new StringReader(sDiagrama);
                mxGraphModel modelo = (mxGraphModel)serializer.Deserialize(rdr);

                if (modelo != null)
                {

                }

                #endregion



            }
            catch (Exception ex)
            {
                //log.Error(ex.Message);
            }


            // Returns the result
            direct.Success = true;
            direct.Result = "";
            return direct;
        }

        #endregion

        #region XML

        private string GetXMLFilePath(long emplazamientoID)
        {
            //se define la ruta para el archivo provisional y se crea el directorio de destino si no existe
            string Filepath = GetXMLDirectoryPath();

            string fileName = "";
           /* if (lEmplazamientoID > 0)
            {
                fileName = "MF-INV-" + lEmplazamientoID.ToString() + ".xml";
                fileName = fileName.Replace(" ", "");
            }
            else
            {
                fileName = "";
            }*/
            string fullpath = Path.Combine(Filepath, fileName);

            // Returns the result
            return fullpath;
        }

        /// <summary>
        /// Gets the path to read/write the xml file
        /// </summary>
        /// <returns>XML path</returns>
        private string GetXMLDirectoryPath()
        {
            string Filepath = TreeCore.DirectoryMapping.GetDocumentDirectory();
            
            Filepath = Filepath + "\\Inventory\\xml\\";

            // Returns the result
            return Filepath;
        }

        #endregion

        #region CREATION XML FILE

      /*  private string CreatesXML()
        {
            // Local variables
            string sNombreFichero = null;
            InventarioElementosVinculacionesController cVincula = null;
            InventarioElementosController cElementos = null;
            List<Vw_InventarioElementos> listaElementos = null;
            List<InventarioElementosVinculaciones> listaVincula = null;
            mxCell celda = null;
            mxCell root = null;
            mxCell raiz = null;
            //mxCell capa = null;
            mxGeometry geometria = null;
            mxGraph grafico = null;
            mxGraphModel modelo = null;
            int i = 0;
            //int iCategorias = 0;
            Dictionary<string, int> dtCapas = null;
            Dictionary<long, mxCell> dtCeldas = null;
            List<mxCell> listaConexiones = null;
            string sEstilo = null;
            //mxCodec codec = null;



            try
            {
                cElementos = new InventarioElementosController();
                listaElementos = cElementos.GetListaElementosByEmplazamiento(lEmplazamientoID);
                if (listaElementos != null && listaElementos.Count > 0)
                {
                    // Creates the root
                    grafico = new mxGraph();
                    grafico.GridSize = 10;
                    grafico.GridEnabled = true;
                    grafico.View.Scale = 1;
                    grafico.Model.BeginUpdate();

                    raiz = new mxCell();
                    raiz.Id = i.ToString();
                    i = i + 1;
                    root = new mxCell();
                    root.Id = i.ToString();
                    root.Parent = raiz;
                    raiz.Insert(root);
                    // Creates the layer
                    dtCapas = new Dictionary<string, int>();
                    dtCeldas = new Dictionary<long, mxCell>();
                    dtCeldas.Add(-1, raiz);
                    dtCeldas.Add(-2, root);

                    foreach (Vw_InventarioElementos elemento in listaElementos)
                    {
                        celda = new mxCell();
                        celda.Id = "EL-" + i.ToString();
                        celda.Value = elemento.NombreElemento;
                        sEstilo = "image;html=1;image=" + elemento.Imagen;
                        celda.Style = sEstilo;
                        celda.Parent = root;
                        celda.Vertex = true;
                        geometria = new mxGeometry();
                        geometria.Width = 80;
                        geometria.Height = 80;
                        celda.Geometry = geometria;
                        celda.SetAttribute("ElementID", elemento.InventarioElementoID.ToString());
                        dtCeldas.Add(elemento.InventarioElementoID, celda);
                        root.Insert(celda);
                        i = i + 1;

                    }
                    listaConexiones = new List<mxCell>();
                    foreach (Data.Vw_InventarioElementos elemento in listaElementos)
                    {
                        cVincula = new InventarioElementosVinculacionesController();
                        listaVincula = cVincula.GetAllVinculacionesByElementoID(elemento.InventarioElementoID);
                        if (listaVincula != null && listaVincula.Count > 0)
                        {

                            foreach (Data.InventarioElementosVinculaciones vincula in listaVincula)
                            {
                                celda = new mxCell();
                                celda.Id = "XC-" + i.ToString();
                                //celda.Value = elemento.NombreElemento;
                                sEstilo = "edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;";
                                celda.Style = sEstilo;
                                celda.Parent = root;
                                celda.Edge = true;
                                geometria = new mxGeometry();
                                geometria.Relative = true;
                                celda.Geometry = geometria;
                                celda.Source = dtCeldas[vincula.InventarioElementoID];
                                celda.Target = dtCeldas[(long)vincula.InventarioElementoPadreID];
                                root.Insert(celda);
                                listaConexiones.Add(celda);
                                i = i + 1;
                            }
                        }

                    }



                    modelo = new mxGraphModel(raiz);
                    grafico = new mxGraph(modelo);

                    // Creates the file
                    string Filepath = "";
                    string sDiagrama = null;
                    if (Properties.Settings.Default.RutaDocumentos[2] == '\\')
                    {
                        Filepath = Properties.Settings.Default.RutaDocumentos;
                    }
                    else
                    {
                        Filepath = Server.MapPath("~/" + Properties.Settings.Default.RutaDocumentos);
                    }

                    Filepath = Filepath + "\\Inventory\\xml\\";

                    string fileName = "";
                    if (lEmplazamientoID > 0)
                    {
                        fileName = "MF-INV-" + lEmplazamientoID.ToString() + ".xml";
                        fileName = fileName.Replace(" ", "");
                    }
                    else
                    {
                        fileName = "MF-INV-Default.xml";
                    }
                    string fullpath = Path.Combine(Filepath, fileName);

                    if (!System.IO.Directory.Exists(Filepath))
                    { System.IO.Directory.CreateDirectory(Filepath); }

                    if (System.IO.File.Exists(fullpath))
                    {
                        System.IO.File.Delete(fullpath);
                    }
                    //codec = new mxCodec();
                    //var result = codec.Encode(grafico.Model);
                    //sDiagrama = mxUtils.GetXml(result);
                    MyFlow.MyFlowController cFlow = new MyFlow.MyFlowController();
                    sDiagrama = cFlow.Encode(grafico, dtCeldas, listaConexiones);

                    File.WriteAllText(fullpath, sDiagrama);

                    sNombreFichero = "MF-INV-" + lEmplazamientoID.ToString() + ".xml";
                    sNombreFichero = sNombreFichero.Replace(" ", "");
                }
            }
            catch (Exception ex)
            {
                Comun.cLog.EscribirLog("InventarioGraficos.aspx.cs - CreateXML(): " + ex.Message);
                sNombreFichero = null;
            }

            // Returns the result
            return sNombreFichero;
        }*/

        #endregion
    }
}