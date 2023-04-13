using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using TreeCore.Data;
using Tree.Linq.GenericExtensions;
using System.Text.RegularExpressions;
using Ext.Net;


namespace CapaNegocio
{
    public class InventarioAtributosController : GeneralBaseController<InventarioAtributos, TreeCoreContext>
    {
        public InventarioAtributosController()
            : base()
        {

        }

        public bool NombreDuplicado(string nombre, long lAtrID)
        {
            bool bDuplicado = false;
            try
            {
                List<InventarioAtributos> listaDatos = (from c in Context.InventarioAtributos
                                                        where c.NombreAtributo == nombre &&
                  c.InventarioCategoriaID == (from x in Context.InventarioAtributos where x.InventarioAtributoID == lAtrID select x).First().InventarioCategoriaID
                                                        select c).ToList();
                if (listaDatos.Count > 0)
                {
                    bDuplicado = true;
                }
            }
            catch (Exception ex)
            {
                bDuplicado = true;
                log.Error(ex.Message);
            }
            return bDuplicado;
        }

        public List<InventarioAtributos> GetAtributosFromCategoria(long idCategoria)
        {
            List<InventarioAtributos> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioAtributos where c.InventarioCategoriaID == idCategoria select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }
        public List<InventarioAtributos> GetAtributosFromCategoriaActivos(long idCategoria)
        {
            List<InventarioAtributos> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioAtributos where c.InventarioCategoriaID == idCategoria && c.Activo orderby c.Orden select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }
        public List<InventarioAtributos> GetAtributosFromCategoriaAtributo(long idCategoria, long idCategoriaAtributo)
        {
            List<InventarioAtributos> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioAtributos where c.InventarioCategoriaID == idCategoria && c.InventarioAtributoCategoriaID == idCategoriaAtributo select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        //#region Tabla

        //public List<Ext.Net.ListItem> GetItemsComboBoxByTabla(string sController, string sTabla, string sIndice, string sValor, string sMetodo)
        //{
        //    List<Ext.Net.ListItem> listaDatos = new List<Ext.Net.ListItem>();
        //    try
        //    {
        //        sTabla = sTabla.Replace("dbo.", "");
        //        sTabla = "TreeCore.Data." + sTabla;
        //        Type clase = Type.GetType(sTabla);
        //        Type controller = Type.GetType("CapaNegocio." + sController);

        //        ConstructorInfo constuctorController = controller.GetConstructor(Type.EmptyTypes);
        //        object objetoConstructor = constuctorController.Invoke(new object[] { });
        //        ConstructorInfo constuctorClase = clase.GetConstructor(Type.EmptyTypes);
        //        object objetoClase = constuctorClase.Invoke(new object[] { });

        //        sMetodo = "GetItemList";

        //        MethodInfo m = controller.GetMethod(sMetodo);
        //        Object invocacion = m.Invoke(objetoConstructor, null);
        //        IList collection = (IList)invocacion;

        //        if (invocacion != null)
        //        {
        //            for (int j = 0; j < collection.Count; j = j + 1)
        //            {
        //                Ext.Net.ListItem newListItem = new Ext.Net.ListItem();
        //                PropertyInfo[] properties = clase.GetProperties();
        //                foreach (PropertyInfo property in properties)
        //                {
        //                    if (property.Name.Equals(sIndice))
        //                    {
        //                        newListItem.Value = Convert.ToString(property.GetValue(collection[j]));
        //                    }

        //                    if (property.Name.Equals(sValor))
        //                    {
        //                        newListItem.Text = Convert.ToString(property.GetValue(collection[j]));
        //                    }
        //                }
        //                listaDatos.Add(newListItem);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    return listaDatos;
        //}

        //public JsonObject GetJsonItemsComboBoxByTabla(string sController, string sTabla, string sIndice, string sValor, string sMetodo)
        //{
        //    JsonObject listaDatos = new JsonObject();
        //    try
        //    {
        //        sTabla = sTabla.Replace("dbo.", "");
        //        sTabla = "TreeCore.Data." + sTabla;
        //        Type clase = Type.GetType(sTabla);
        //        Type controller = Type.GetType("CapaNegocio." + sController);

        //        ConstructorInfo constuctorController = controller.GetConstructor(Type.EmptyTypes);
        //        object objetoConstructor = constuctorController.Invoke(new object[] { });
        //        ConstructorInfo constuctorClase = clase.GetConstructor(Type.EmptyTypes);
        //        object objetoClase = constuctorClase.Invoke(new object[] { });

        //        sMetodo = "GetItemList";

        //        MethodInfo m = controller.GetMethod(sMetodo);
        //        Object invocacion = m.Invoke(objetoConstructor, null);
        //        IList collection = (IList)invocacion;

        //        if (invocacion != null)
        //        {
        //            for (int j = 0; j < collection.Count; j = j + 1)
        //            {
        //                JsonObject auxJson = new JsonObject();
        //                string indice = "";
        //                PropertyInfo[] properties = clase.GetProperties();
        //                foreach (PropertyInfo property in properties)
        //                {
        //                    if (property.Name.Equals(sIndice))
        //                    {
        //                        auxJson.Add("Value", Convert.ToString(property.GetValue(collection[j])));
        //                        indice = Convert.ToString(property.GetValue(collection[j]));
        //                    }

        //                    if (property.Name.Equals(sValor))
        //                    {
        //                        auxJson.Add("Text", Convert.ToString(property.GetValue(collection[j])));
        //                    }
        //                }
        //                listaDatos.Add(indice, auxJson);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    return listaDatos;
        //}

        //public string GetValoresStringComboBoxByTabla(string sNombre, string sController, string sTabla, string sIndice, string sValor)
        //{
        //    // Local variables
        //    string sMetodo = null;
        //    string ValoresPosibles = "";

        //    try
        //    {
        //        sTabla = sTabla.Replace("dbo.", "");
        //        sTabla = "TreeCore.Data." + sTabla;
        //        Type clase = Type.GetType(sTabla);
        //        Type controller = Type.GetType("CapaNegocio." + sController);

        //        if (controller != null)
        //        {
        //            ConstructorInfo constuctorController = controller.GetConstructor(Type.EmptyTypes);
        //            object objetoConstructor = constuctorController.Invoke(new object[] { });
        //            ConstructorInfo constuctorClase = clase.GetConstructor(Type.EmptyTypes);
        //            object objetoClase = constuctorClase.Invoke(new object[] { });

        //            sMetodo = "GetItemList";
        //            MethodInfo m = controller.GetMethod(sMetodo);

        //            Object invocacion = m.Invoke(objetoConstructor, null);
        //            IList collection = (IList)invocacion;


        //            if (invocacion != null)
        //            {
        //                for (int j = 0; j < collection.Count; j = j + 1)
        //                {
        //                    PropertyInfo[] properties = clase.GetProperties();
        //                    foreach (PropertyInfo property in properties)
        //                    {
        //                        if (property.Name.Equals(sValor))
        //                        {
        //                            if (j == 0)
        //                            {
        //                                ValoresPosibles = Convert.ToString(property.GetValue(collection[j]));
        //                            }
        //                            else
        //                            {
        //                                ValoresPosibles += ";" + Convert.ToString(property.GetValue(collection[j]));
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Comun.cLog.EscribirLog("InventarioAtributosController - GetComboBoxByTabla: " + ex.Message);
        //    }

        //    // Returns the result
        //    return ValoresPosibles;
        //}



        //#endregion

        #region ColumnaModeloDatoID

        public List<Ext.Net.ListItem> GetItemsComboBoxByColumnaModeloDatosID(long lColumnaModeloDatos, string sCamposAdicionales)
        {
            List<Ext.Net.ListItem> listaDatos = new List<Ext.Net.ListItem>();
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            ColumnasModeloDatos oColumna, oColumnaAux;
            List<ColumnasModeloDatos> listaColumnas;
            try
            {
                oColumna = cColumnas.GetItem(lColumnaModeloDatos);
                listaColumnas = new List<ColumnasModeloDatos>();
                if (sCamposAdicionales != "")
                {
                    foreach (var item in sCamposAdicionales.Split(','))
                    {
                        oColumnaAux = cColumnas.GetItem(long.Parse(item));
                        listaColumnas.Add(oColumnaAux);
                    };
                }

                string query = "select ";
                query += oColumna.NombreColumna;
                query += ", " + oColumna.TablasModeloDatos.Indice;
                foreach (var item in listaColumnas)
                {
                    query += ", " + item.NombreColumna;
                }
                query += " from " + oColumna.TablasModeloDatos.NombreTabla;

                DataTable result = this.EjecutarQuery(query);

                Ext.Net.ListItem newListItem;
                if (result != null)
                {
                    foreach (System.Data.DataRow fila in result.Rows)
                    {
                        string text = "";
                        string textAux = "";
                        newListItem = new Ext.Net.ListItem();

                        newListItem.Value = fila.ItemArray[1].ToString();
                        text = fila[0].ToString();
                        for (int i = 2; i < fila.ItemArray.Length; i++)
                        {
                            if (fila.ItemArray[i].ToString() != "")
                            {
                                if (textAux != "")
                                {
                                    textAux += " - " + fila.ItemArray[i].ToString();
                                }
                                else
                                {
                                    textAux += fila.ItemArray[i].ToString();
                                }
                            }
                        }
                        if (textAux != "")
                        {
                            newListItem.Text = text + ", " + textAux;
                        }
                        else
                        {
                            newListItem.Text = text;
                        }
                        listaDatos.Add(newListItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            return listaDatos;
        }

        public JsonObject GetJsonItemsComboBoxByColumnaModeloDatosID(long lColumnaModeloDatos, string sCamposAdicionales)
        {
            JsonObject listaDatos = new JsonObject();
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            ColumnasModeloDatos oColumna, oColumnaAux;
            List<ColumnasModeloDatos> listaColumnas;
            try
            {
                oColumna = cColumnas.GetItem(lColumnaModeloDatos);
                listaColumnas = new List<ColumnasModeloDatos>();
                if (sCamposAdicionales != "")
                {
                    foreach (var item in sCamposAdicionales.Split(','))
                    {
                        oColumnaAux = cColumnas.GetItem(long.Parse(item));
                        listaColumnas.Add(oColumnaAux);
                    };
                }

                string query = "select ";
                query += oColumna.NombreColumna;
                query += ", " + oColumna.TablasModeloDatos.Indice;
                foreach (var item in listaColumnas)
                {
                    query += ", " + item.NombreColumna;
                }
                query += " from " + oColumna.TablasModeloDatos.NombreTabla;

                DataTable result = this.EjecutarQuery(query);

                JsonObject auxJson;
                if (result != null)
                {
                    foreach (System.Data.DataRow fila in result.Rows)
                    {
                        string text = "";
                        string textAux = "";
                        auxJson = new JsonObject();

                        text = fila[0].ToString();
                        for (int i = 2; i < fila.ItemArray.Length; i++)
                        {
                            if (fila.ItemArray[i].ToString() != "")
                            {
                                if (textAux != "")
                                {
                                    textAux += " - " + fila.ItemArray[i].ToString();
                                }
                                else
                                {
                                    textAux += fila.ItemArray[i].ToString();
                                }
                            }
                        }
                        auxJson.Add("Value", fila.ItemArray[1].ToString());
                        if (textAux != "")
                        {
                            auxJson.Add("Text", text + ", " + textAux);
                        }
                        else
                        {
                            auxJson.Add("Text", text);
                        }
                        listaDatos.Add(fila.ItemArray[1].ToString(), auxJson);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            return listaDatos;
        }

        public JsonObject GetJsonItemsByColumnaModeloDatosID(long lColumnaModeloDatos)
        {
            JsonObject listaDatos = new JsonObject();
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            ColumnasModeloDatos oColumna, oColumnaAux;
            try
            {
                oColumna = cColumnas.GetItem(lColumnaModeloDatos);

                string query = "select ";
                query += oColumna.NombreColumna;
                query += ", " + oColumna.TablasModeloDatos.Indice;
                query += " from " + oColumna.TablasModeloDatos.NombreTabla;

                DataTable result = this.EjecutarQuery(query);

                JsonObject auxJson;
                if (result != null)
                {
                    foreach (System.Data.DataRow fila in result.Rows)
                    {
                        string text = "";
                        string textAux = "";
                        auxJson = new JsonObject();
                        object oAux = null;

                        text = fila[0].ToString();
                        auxJson.Add("Value", fila.ItemArray[1].ToString());
                        auxJson.Add("Text", text);
                        if (!(listaDatos.TryGetValue(text, out oAux) && oAux != null))
                        {
                            listaDatos.Add(text, auxJson);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            return listaDatos;
        }

        public List<string> GetValoresListStringComboBoxByColumnaModeloDatosID(long lColumnaModeloDatos, string sCamposAdicionales)
        {
            List<Ext.Net.ListItem> listaDatos = new List<Ext.Net.ListItem>();
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            ColumnasModeloDatos oColumna, oColumnaAux;
            List<ColumnasModeloDatos> listaColumnas;
            List<string> ValoresPosibles = new List<string>();
            try
            {
                oColumna = cColumnas.GetItem(lColumnaModeloDatos);
                listaColumnas = new List<ColumnasModeloDatos>();
                if (sCamposAdicionales != "")
                {
                    foreach (var item in sCamposAdicionales.Split(','))
                    {
                        oColumnaAux = cColumnas.GetItem(long.Parse(item));
                        listaColumnas.Add(oColumnaAux);
                    };
                }

                string query = "select ";
                query += oColumna.NombreColumna;
                query += ", " + oColumna.TablasModeloDatos.Indice;
                foreach (var item in listaColumnas)
                {
                    query += ", " + item.NombreColumna;
                }
                query += " from " + oColumna.TablasModeloDatos.NombreTabla;

                DataTable result = this.EjecutarQuery(query);

                Ext.Net.ListItem newListItem;
                if (result != null)
                {
                    foreach (System.Data.DataRow fila in result.Rows)
                    {
                        ValoresPosibles.Add(fila[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            // Returns the result
            return ValoresPosibles;
        }

        public string GetValoresStringComboBoxByColumnaModeloDatosID(long lColumnaModeloDatos, string sCamposAdicionales)
        {
            List<Ext.Net.ListItem> listaDatos = new List<Ext.Net.ListItem>();
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            ColumnasModeloDatos oColumna, oColumnaAux;
            List<ColumnasModeloDatos> listaColumnas;
            string ValoresPosibles = "";
            try
            {
                oColumna = cColumnas.GetItem(lColumnaModeloDatos);
                listaColumnas = new List<ColumnasModeloDatos>();

                if (sCamposAdicionales != "")
                {
                    foreach (var item in sCamposAdicionales.Split(','))
                    {
                        oColumnaAux = cColumnas.GetItem(long.Parse(item));
                        listaColumnas.Add(oColumnaAux);
                    };
                }

                string query = "select ";
                query += oColumna.NombreColumna;
                query += ", " + oColumna.TablasModeloDatos.Indice;
                foreach (var item in listaColumnas)
                {
                    query += ", " + item.NombreColumna;
                }
                query += " from " + oColumna.TablasModeloDatos.NombreTabla;

                DataTable result = this.EjecutarQuery(query);

                Ext.Net.ListItem newListItem;
                if (result != null)
                {
                    foreach (System.Data.DataRow fila in result.Rows)
                    {
                        if (ValoresPosibles == "")
                        {
                            ValoresPosibles = fila[0].ToString();
                        }
                        else
                        {
                            ValoresPosibles += ";" + fila[0].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            // Returns the result
            return ValoresPosibles;
        }

        #endregion

        #region Funciones

        public List<Ext.Net.ListItem> GetItemsComboBoxByFuncion(string sMetodo, long? EmplazamientoID, long? InventarioEmplazamientoID, long AtributoID)
        {
            List<Ext.Net.ListItem> listaDatos = new List<Ext.Net.ListItem>();
            try
            {
                Type controller = Type.GetType("TreeCore.Clases.InventarioFuncionesCargaStores");
                ConstructorInfo constuctorController = controller.GetConstructor(Type.EmptyTypes);
                object objetoConstructor = constuctorController.Invoke(new object[] { });

                MethodInfo m = controller.GetMethod(sMetodo);
                Object invocacion = m.Invoke(objetoConstructor, new object[] { EmplazamientoID, InventarioEmplazamientoID, AtributoID });
                IList collection = (IList)invocacion;

                if (invocacion != null)
                {
                    for (int j = 0; j < collection.Count; j = j + 1)
                    {
                        Ext.Net.ListItem newListItem = new Ext.Net.ListItem();
                        newListItem.Value = Convert.ToString(j);
                        newListItem.Text = Convert.ToString(collection[j]);
                        listaDatos.Add(newListItem);
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            return listaDatos;
        }

        public JsonObject GetJsonItemsComboBoxByFuncion(string sMetodo, long? EmplazamientoID, long? InventarioEmplazamientoID, long AtributoID)
        {
            JsonObject listaDatos = new JsonObject();
            try
            {
                Type controller = Type.GetType("TreeCore.Clases.InventarioFuncionesCargaStores");
                ConstructorInfo constuctorController = controller.GetConstructor(Type.EmptyTypes);
                object objetoConstructor = constuctorController.Invoke(new object[] { });

                MethodInfo m = controller.GetMethod(sMetodo);
                Object invocacion = m.Invoke(objetoConstructor, new object[] { EmplazamientoID, InventarioEmplazamientoID, AtributoID });
                IList collection = (IList)invocacion;

                if (invocacion != null)
                {
                    for (int j = 0; j < collection.Count; j = j + 1)
                    {
                        JsonObject auxJson = new JsonObject();
                        auxJson.Add("Value", j);
                        auxJson.Add("Text", Convert.ToString(collection[j]));
                        listaDatos.Add(j.ToString(), auxJson);
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            return listaDatos;
        }

        public List<string> GetListItemsComboBoxByFuncion(string sMetodo, long? EmplazamientoID, long? InventarioEmplazamientoID, long AtributoID)
        {
            List<string> listaDatos = new List<string>();
            try
            {
                Type controller = Type.GetType("TreeCore.Clases.InventarioFuncionesCargaStores");
                ConstructorInfo constuctorController = controller.GetConstructor(Type.EmptyTypes);
                object objetoConstructor = constuctorController.Invoke(new object[] { });

                MethodInfo m = controller.GetMethod(sMetodo);
                Object invocacion = m.Invoke(objetoConstructor, new object[] { EmplazamientoID, InventarioEmplazamientoID, AtributoID });
                IList collection = (IList)invocacion;

                if (invocacion != null)
                {
                    for (int j = 0; j < collection.Count; j = j + 1)
                    {
                        //Ext.Net.ListItem newListItem = new Ext.Net.ListItem();
                        //newListItem.Value = Convert.ToString(j);
                        //newListItem.Text = Convert.ToString(collection[j]);
                        listaDatos.Add(Convert.ToString(j));
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            return listaDatos;
        }

        #endregion

        #region OBTENCIÓN DE COMPONENTES
        public string GetLabelByAtributo(long AtributoID, string TipoDato, string NombreAtributo, string ValorDefecto, string ValoresPosibles, bool Obligatorio, string NombreTabla, string TablaIndice, string TablaValor, string TablaControlador, string FuncionControlador, long elementoID, string Resultado)
        {
            string lblValor = "";
            try
            {
                switch (TipoDato)
                {
                    case Comun.TIPODATO_CODIGO_TEXTO:
                        {
                            lblValor = Resultado;
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_NUMERICO:
                        {
                            lblValor = Resultado;
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_NUMERICO_FLOTANTE:
                        {
                            lblValor = Resultado;
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_NUMERICO_ENTERO:
                        {
                            lblValor = Resultado;
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_FECHA:
                        {
                            lblValor = Resultado;
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_GEOPOSICION:
                        {

                        }
                        break;
                    case Comun.TIPODATO_CODIGO_BOOLEAN:
                        {
                            lblValor = Resultado;
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_LISTA:
                        {
                            if (NombreTabla != null && TablaIndice != null && NombreTabla != "" && TablaIndice != "")
                            {
                                lblValor = GetLabelByTabla(TablaControlador, NombreTabla, TablaValor, Resultado);
                            }
                            else if (FuncionControlador != null && FuncionControlador != "")
                            {
                                lblValor = GetLabelByFuncion(NombreAtributo, FuncionControlador, elementoID, Resultado);
                            }
                            else
                            {
                                int indice = 0;
                                if (int.TryParse(Resultado, out indice) && ValoresPosibles != null && ValoresPosibles != "")
                                {
                                    string[] sElementos = ValoresPosibles.Split(';');
                                    indice = Convert.ToInt32(Resultado) - 1;
                                    lblValor = sElementos[indice];
                                }
                                else
                                {
                                    lblValor = Resultado;
                                }

                            }
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                        {
                            if (NombreTabla != null && TablaIndice != null && NombreTabla != "" && TablaIndice != "")
                            {
                                string[] elements = Resultado.Split(',');
                                for (int i = 0; i < elements.Length; i++)
                                {
                                    if (lblValor == "")
                                    {
                                        lblValor = GetLabelByTabla(TablaControlador, NombreTabla, TablaValor, elements[i]);
                                    }
                                    else
                                    {
                                        lblValor += ", " + GetLabelByTabla(TablaControlador, NombreTabla, TablaValor, elements[i]);
                                    }
                                }
                            }
                            else if (FuncionControlador != null && FuncionControlador != "")
                            {


                                string[] elements = Resultado.Split(',');
                                for (int i = 0; i < elements.Length; i++)
                                {
                                    if (lblValor == "")
                                    {
                                        lblValor = GetLabelByFuncion(NombreAtributo, FuncionControlador, elementoID, elements[i]);
                                    }
                                    else
                                    {
                                        lblValor += ", " + GetLabelByFuncion(NombreAtributo, FuncionControlador, elementoID, elements[i]);
                                    }
                                }

                            }
                            else
                            {

                                string sValores = ValoresPosibles;
                                if (sValores != null)
                                {
                                    string[] sElementos = sValores.Split(';');
                                    string[] elements = Resultado.Split(',');

                                    for (int i = 0; i < elements.Length; i++)
                                    {
                                        int ind = Array.IndexOf(sElementos, elements[i]);

                                        if (lblValor == "")
                                        {
                                            lblValor = sElementos[ind];
                                        }
                                        else
                                        {
                                            lblValor += ", " + sElementos[ind];
                                        }
                                    }

                                }
                            }
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string msd = ex.Message;
            }

            return lblValor;
        }

        public string GetLabelByTabla(string sController, string sTabla, string sCampo, string sIndice)
        {
            string lblValor = "";

            try
            {
                if (!string.IsNullOrEmpty(sController) && !string.IsNullOrEmpty(sTabla) && !string.IsNullOrEmpty(sCampo) && !string.IsNullOrEmpty(sIndice))
                {
                    sTabla = sTabla.Replace("dbo.", "");
                    sTabla = "TreeCore.Data." + sTabla;
                    Type clase = Type.GetType(sTabla);
                    Type controller = Type.GetType("CapaNegocio." + sController);

                    ConstructorInfo constuctorController = controller.GetConstructor(Type.EmptyTypes);
                    object objetoConstructor = constuctorController.Invoke(new object[] { });
                    ConstructorInfo constuctorClase = clase.GetConstructor(Type.EmptyTypes);
                    object objetoClase = constuctorClase.Invoke(new object[] { });

                    MethodInfo m = controller.GetMethods().First(s => s.Name == "GetItem");
                    Object invocacion = m.Invoke(objetoConstructor, new object[] { Convert.ToInt32(sIndice) });

                    if (invocacion != null)
                    {
                        PropertyInfo Property = clase.GetProperty(sCampo);
                        lblValor = Property.GetValue(invocacion).ToString();
                    }
                }


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return lblValor;
        }

        public string GetLabelByFuncion(string sNombre, string sMetodo, long elementoID, string sIndice)
        {
            string lblValor = "";

            try
            {
                if (!string.IsNullOrEmpty(sMetodo) && !string.IsNullOrEmpty(sIndice))
                {
                    Type controller = Type.GetType("TreeCore.Clases.InventarioFuncionesCargaStores");
                    ConstructorInfo constuctorController = controller.GetConstructor(Type.EmptyTypes);
                    object objetoConstructor = constuctorController.Invoke(new object[] { });

                    MethodInfo m = controller.GetMethod(sMetodo);
                    Object invocacion = m.Invoke(objetoConstructor, new object[] { elementoID });
                    IList collection = (IList)invocacion;

                    if (invocacion != null)
                    {
                        lblValor = collection[Convert.ToInt32(sIndice) - 1].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return lblValor;
        }
        #endregion
        public InventarioAtributos GetAtributoByNombreCategoria(string NombreAtributo, long CategoriaID)
        {
            InventarioAtributos atributo = null;
            try
            {
                atributo = (from c in Context.InventarioAtributos where c.Activo == true && c.InventarioCategoriaID == CategoriaID && c.NombreAtributo == NombreAtributo select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                atributo = null;
            }
            return atributo;
        }

        public long GetAtributoByNombreCategoriaAPI(string NombreAtributo, long CategoriaID)
        {
            long resultado = 0;
            Vw_InventarioAtributos inventarioAtributos = new Vw_InventarioAtributos();
            try
            {
                inventarioAtributos = (from c in Context.Vw_InventarioAtributos where c.Activo == true && c.InventarioCategoriaID == CategoriaID && c.NombreAtributo == NombreAtributo select c).First();
                if (inventarioAtributos != null)
                {
                    resultado = inventarioAtributos.InventarioAtributoID;
                }

            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                inventarioAtributos = null;
            }
            return resultado;
        }

        public Dictionary<long, string> GetIDValoresByTabla(long lColumnaModeloDatos)
        {
            List<Ext.Net.ListItem> listaDatos = new List<Ext.Net.ListItem>();
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            ColumnasModeloDatos oColumna, oColumnaAux;
            List<ColumnasModeloDatos> listaColumnas;
            List<string> ValoresPosibles = new List<string>();
            Dictionary<long, string> ListaIDValores = new Dictionary<long, string>();
            try
            {
                oColumna = cColumnas.GetItem(lColumnaModeloDatos);

                string query = "select ";
                query += oColumna.NombreColumna;
                query += ", " + oColumna.TablasModeloDatos.Indice;
                query += " from " + oColumna.TablasModeloDatos.NombreTabla;

                DataTable result = this.EjecutarQuery(query);

                Ext.Net.ListItem newListItem;
                if (result != null)
                {
                    foreach (System.Data.DataRow fila in result.Rows)
                    {
                        ListaIDValores.Add(long.Parse(fila[1].ToString()), fila[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            // Returns the result
            return ListaIDValores;
        }

        public string CheckTipoDatoCorrecto(string CodigoTipoDato, string Dato, string ListaValoresPosibles, long AtributoID, Dictionary<long, string> ListaIDValor)
        {
            string result = Dato;

            try
            {
                switch (CodigoTipoDato)
                {
                    case Comun.TIPODATO_CODIGO_TEXTO:
                        {

                        }
                        break;
                    case Comun.TIPODATO_CODIGO_NUMERICO_FLOTANTE:
                        {
                            Double num;
                            if (!double.TryParse(Dato, out num))
                            {
                                result = "%err%";
                            }
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_NUMERICO:
                        {
                            Double num;
                            if (!double.TryParse(Dato, out num))
                            {
                                result = "%err%";
                            }
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_NUMERICO_ENTERO:
                        {
                            Int32 num;
                            if (!Int32.TryParse(Dato, out num))
                            {
                                result = "%err%";
                            }
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_FECHA:
                        {
                            DateTime date;
                            result = Dato.Substring(0, 2) + "/" + Dato.Substring(2, 2) + "/" + Dato.Substring(4, 4);

                            if (!DateTime.TryParse(result, out date))
                            {
                                result = "%err%";
                            }
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_GEOPOSICION:
                        {

                        }
                        break;
                    case Comun.TIPODATO_CODIGO_BOOLEAN:
                        {
                            Boolean check;
                            if (!Boolean.TryParse(Dato, out check))
                            {
                                result = "%err%";
                            }
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_LISTA:
                    case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                        {

                            if (ListaValoresPosibles != null && ListaValoresPosibles != "")
                            {
                                string[] listaValoresPosibles = ListaValoresPosibles.Split(';');
                                string[] listaValores = Dato.Split(';');

                                for (int i = 0; i < listaValores.Length; i++)
                                {
                                    int index = Array.IndexOf(listaValoresPosibles, listaValores[i]);

                                    if (index < 0)
                                    {
                                        result = "%err%";
                                    }
                                    else
                                    {
                                        if (i == 0)
                                        {
                                            result = listaValores[i];
                                        }
                                        else
                                        {
                                            result += ";" + listaValores[i];
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string[] listaValores = Dato.Split(';');

                                for (int i = 0; i < listaValores.Length; i++)
                                {
                                    long myKey = ListaIDValor.FirstOrDefault(x => x.Value == listaValores[i]).Key;


                                    if (myKey == 0)
                                    {
                                        result = "%err%";
                                        break;
                                    }
                                    else
                                    {

                                        if (i == 0)
                                        {
                                            result = myKey.ToString();
                                        }
                                        else
                                        {
                                            result += ";" + myKey.ToString();
                                        }
                                    }

                                }

                            }

                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = "%err%";
            }

            return result;

        }


        /// <summary>
        /// CheckTipoDatoCorrectoServicio usar unicamente para el servicio Import Export
        /// </summary>
        /// <param name="CodigoTipoDato"></param>
        /// <param name="Dato"></param>
        /// <param name="ListaValoresPosibles"></param>
        /// <param name="AtributoID"></param>
        /// <param name="ListaIDValor"></param>
        /// <returns></returns>
        public string CheckTipoDatoCorrectoServicio(string CodigoTipoDato, string Dato, string ListaValoresPosibles, long AtributoID, Dictionary<long, string> ListaIDValor)
        {
            string result = Dato;

            try
            {
                switch (CodigoTipoDato)
                {
                    case "TEXTO":
                        {

                        }
                        break;
                    case "FLOTANTE":
                        {
                            /*Double num;
                            if (!double.TryParse(Dato, out num))
                            {
                                result = "%err%";
                            }*/
                        }
                        break;
                    case "NUMERICO":
                        {
                            /*Double num;
                            if (!double.TryParse(Dato, out num))
                            {
                                result = "%err%";
                            }*/
                        }
                        break;
                    case "ENTERO":
                        {
                            /*Int32 num;
                            if (!Int32.TryParse(Dato, out num))
                            {
                                result = "%err%";
                            }*/
                        }
                        break;
                    case "FECHA":
                        {
                            DateTime date;
                            result = Dato.Substring(0, 2) + "/" + Dato.Substring(2, 2) + "/" + Dato.Substring(4, 4);

                            if (!DateTime.TryParse(result, out date))
                            {
                                result = "%err%";
                            }
                        }
                        break;
                    case "GEOPOSICION":
                        {

                        }
                        break;
                    case "BOOLEAN":
                        {
                            Boolean check;
                            if (!Boolean.TryParse(Dato, out check))
                            {
                                result = "%err%";
                            }
                        }
                        break;
                    case "LISTA":
                    case "LISTAMULTIPLE":
                        {

                            if (ListaValoresPosibles != null && ListaValoresPosibles != "")
                            {
                                string[] listaValoresPosibles = ListaValoresPosibles.Split(';');
                                string[] listaValores = Dato.Split(';');
                                if (listaValores.Count() == 1 && listaValores[0] == "")
                                {
                                    result = "";
                                }
                                else
                                {
                                    for (int i = 0; i < listaValores.Length; i++)
                                    {
                                        int index = Array.IndexOf(listaValoresPosibles, listaValores[i]);

                                        if (index < 0)
                                        {
                                            result = "%err%";
                                        }
                                        else
                                        {
                                            if (i == 0)
                                            {
                                                result = listaValores[i];
                                            }
                                            else
                                            {
                                                result += ";" + listaValores[i];
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string[] listaValores = Dato.Split(';');
                                if (listaValores.Count() == 1 && listaValores[0] == "")
                                {
                                    result = "";
                                }
                                else
                                {
                                    for (int i = 0; i < listaValores.Length; i++)
                                    {
                                        long myKey = ListaIDValor.FirstOrDefault(x => x.Value == listaValores[i]).Key;


                                        if (myKey == null)
                                        {
                                            result = "%err%";
                                            break;
                                        }
                                        else
                                        {

                                            if (i == 0)
                                            {
                                                result = myKey.ToString();
                                            }
                                            else
                                            {
                                                result += ";" + myKey.ToString();
                                            }
                                        }

                                    }
                                }

                            }

                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = "%err%";
            }

            return result;

        }

        public List<InventarioAtributos> GetActivos(long clienteID)
        {
            List<InventarioAtributos> lista;

            try
            {
                lista = (from c in Context.InventarioAtributos
                         join cat in Context.InventarioAtributosCategorias on c.InventarioAtributoCategoriaID equals cat.InventarioAtributoCategoriaID
                         where cat.ClienteID == clienteID
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }


    }
}