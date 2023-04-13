using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using System.Data;
using Ext.Net;
using Newtonsoft.Json;

namespace CapaNegocio
{
    public class EmplazamientosAtributosConfiguracionController : GeneralBaseController<EmplazamientosAtributosConfiguracion, TreeCoreContext>
    {
        public EmplazamientosAtributosConfiguracionController()
            : base()
        { }

        public void ActualizarOrdenCategorias(long catAtrID, bool Orden)
        {
            List<EmplazamientosAtributosConfiguracion> listaEjecutor;
            List<EmplazamientosAtributosConfiguracion> listaVictima;
            try
            {
                listaEjecutor = (from c in Context.EmplazamientosAtributosConfiguracion where c.EmplazamientoAtributoCategoriaID == catAtrID select c).ToList();
                if (Orden)
                {
                    listaVictima = (from c in Context.EmplazamientosAtributosConfiguracion where c.OrdenCategoria == (listaEjecutor.First().OrdenCategoria - 1) select c).ToList();
                    if (listaVictima.Count > 0)
                    {
                        listaVictima.ForEach(x => x.OrdenCategoria++);
                        Context.SubmitChanges();
                    }
                    if ((listaEjecutor.First().OrdenCategoria - 1) >= 0)
                    {
                        listaEjecutor.ForEach(x => x.OrdenCategoria--);
                        Context.SubmitChanges();
                    }
                }
                else
                {
                    listaVictima = (from c in Context.EmplazamientosAtributosConfiguracion where c.OrdenCategoria == (listaEjecutor.First().OrdenCategoria + 1) select c).ToList();
                    if (listaVictima.Count > 0)
                    {
                        listaEjecutor.ForEach(x => x.OrdenCategoria++);
                        listaVictima.ForEach(x => x.OrdenCategoria--);
                        Context.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        public void ActualizarOrdenCategorias(long catAtrID, long Orden)
        {
            List<EmplazamientosAtributosConfiguracion> listaEjecutor;
            List<EmplazamientosAtributosConfiguracion> listaVictima;
            try
            {
                listaEjecutor = (from c in Context.EmplazamientosAtributosConfiguracion where c.EmplazamientoAtributoCategoriaID == catAtrID select c).ToList();
                if (listaEjecutor != null && listaEjecutor.Count > 0)
                {
                    listaEjecutor.ForEach(x => x.OrdenCategoria = (int)Orden);
                    Context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public void ActualizarOrdenAtributo(long atrID, bool Orden)
        {
            EmplazamientosAtributosConfiguracion oAtrEjecutor;
            EmplazamientosAtributosConfiguracion oAtrVictima;
            try
            {
                oAtrEjecutor = (from c in Context.EmplazamientosAtributosConfiguracion where c.EmplazamientoAtributoConfiguracionID == atrID select c).First();
                if (Orden)
                {
                    oAtrVictima = (from c in Context.EmplazamientosAtributosConfiguracion where c.ClienteID == oAtrEjecutor.ClienteID && c.EmplazamientoAtributoCategoriaID == oAtrEjecutor.EmplazamientoAtributoCategoriaID && c.Orden == (oAtrEjecutor.Orden - 1) select c).First();
                    if (oAtrVictima != null)
                    {
                        oAtrEjecutor.Orden--;
                        oAtrVictima.Orden++;
                        Context.SubmitChanges();
                    }
                }
                else
                {
                    oAtrVictima = (from c in Context.EmplazamientosAtributosConfiguracion where c.ClienteID == oAtrEjecutor.ClienteID && c.EmplazamientoAtributoCategoriaID == oAtrEjecutor.EmplazamientoAtributoCategoriaID && c.Orden == (oAtrEjecutor.Orden + 1) select c).First();
                    if (oAtrVictima != null)
                    {
                        oAtrEjecutor.Orden++;
                        oAtrVictima.Orden--;
                        Context.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        public void ActualizarOrdenAtributo(long atrID, long NewOrden)
        {
            EmplazamientosAtributosConfiguracion oAtrEjecutor;
            EmplazamientosAtributosConfiguracion oAtrVictima;
            try
            {
                oAtrEjecutor = (from c in Context.EmplazamientosAtributosConfiguracion where c.EmplazamientoAtributoConfiguracionID == atrID select c).First();
                if (oAtrEjecutor != null)
                {
                    oAtrEjecutor.Orden = (int)NewOrden;
                    Context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        public EmplazamientosAtributosConfiguracion GetAtributoByCodigo(long lClienteID, string sCodigo)
        {
            EmplazamientosAtributosConfiguracion oDato;
            try
            {
                oDato = (from c in Context.EmplazamientosAtributosConfiguracion
                         where c.ClienteID == lClienteID && c.CodigoAtributo == sCodigo
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public List<EmplazamientosAtributosConfiguracion> GetAtributosFromCliente(long idCliente)
        {
            List<EmplazamientosAtributosConfiguracion> listaDatos;
            try
            {
                listaDatos = (from c in Context.EmplazamientosAtributosConfiguracion where c.ClienteID == idCliente select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }
        public List<EmplazamientosAtributosConfiguracion> GetAtributosFromCategoriaAtributo(long idCategoria)
        {
            List<EmplazamientosAtributosConfiguracion> listaDatos;
            try
            {
                listaDatos = (from c in Context.EmplazamientosAtributosConfiguracion where c.EmplazamientoAtributoCategoriaID == idCategoria select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public bool NombreDuplicado(string nombre, long clienteID)
        {
            bool bDuplicado = false;
            try
            {
                List<EmplazamientosAtributosConfiguracion> listaDatos = (from c in Context.EmplazamientosAtributosConfiguracion where c.NombreAtributo == nombre && c.ClienteID == clienteID select c).ToList();
                if (listaDatos.Count > 0)
                {
                    bDuplicado = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                bDuplicado = true;
            }
            return bDuplicado;
        }

        public List<EmplazamientosAtributosConfiguracion> GetAtributosVisibles(long CliID, long lUsuarioID)
        {
            RolesController cRoles = new RolesController();
            List<long> listaRolesID;

            EmplazamientosAtributosConfiguracionRolesRestringidosController cAtrRoles = new EmplazamientosAtributosConfiguracionRolesRestringidosController();
            List<Vw_EmplazamientosAtributosRolesRestringidos> listaRestriccionRoles;

            List<EmplazamientosAtributosConfiguracion> listaDatos, listaFinal;
            try
            {
                listaDatos = (from c in Context.EmplazamientosAtributosConfiguracion
                              where c.ClienteID == CliID
                              orderby c.OrdenCategoria, c.Orden
                              select c).ToList();

                List<Roles> listaRoles = cRoles.GetRolesFromUsuario(lUsuarioID);
                List<long> listaRolesIDs = new List<long>();
                foreach (var oRol in listaRoles) { listaRolesIDs.Add(oRol.RolID); }
                listaFinal = new List<EmplazamientosAtributosConfiguracion>();
                listaFinal.AddRange(listaDatos.GetRange(0, listaDatos.Count));
                foreach (var item in listaDatos)
                {
                    listaRestriccionRoles = cAtrRoles.GetRolesRestringidosAtributo(item.EmplazamientoAtributoConfiguracionID);
                    if (listaRestriccionRoles != null && listaRestriccionRoles.Where(c => listaRoles.Select(r => r.RolID).ToList().Contains((long)c.RolID)).ToList().Count > 0)
                    {
                        foreach (var oRestriccionRol in listaRestriccionRoles)
                        {
                            if (oRestriccionRol.RolID != null)
                            {
                                if (listaRolesIDs.Contains(oRestriccionRol.RolID.Value))
                                {
                                    switch (oRestriccionRol.Restriccion)
                                    {
                                        case (int)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                            listaFinal.Remove(item);
                                            break;
                                        case (int)Comun.RestriccionesAtributoDefecto.DISABLED:
                                        case (int)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    //mainConstainer.Hidden = true;
                                    //ControlAtributo.Disabled = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        var cInvResDefe = cAtrRoles.GetDefault(item.EmplazamientoAtributoConfiguracionID);
                        if (cInvResDefe != null)
                        {
                            switch (cInvResDefe.Restriccion)
                            {
                                case (int)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                    listaFinal.Remove(item);
                                    break;
                                case (int)Comun.RestriccionesAtributoDefecto.DISABLED:
                                case (int)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                default:
                                    break;
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaFinal = null;
            }
            return listaFinal;
        }

        #region ColumnaModeloDatoID

        protected class TablaAtributo
        {
            public string text { get; set; }
            public long TablaID { get; set; }
            public long ColumnaID { get; set; }
            public string IndiceColumna { get; set; }
        }

        protected string GenerarQuery(long lAtributoID)
        {
            JsonObject listaDatos = new JsonObject();
            EmplazamientosAtributosConfiguracionListasColumnasAdicionalesController cColumnas = new EmplazamientosAtributosConfiguracionListasColumnasAdicionalesController();
            EmplazamientosAtributosConfiguracion oAtributo;
            List<TablaAtributo> listaTablasAtr;
            List<EmplazamientosAtributosConfiguracionListasColumnasAdicionales> listaColumnas;
            ColumnasModeloDatosController cColumnasConf = new ColumnasModeloDatosController();
            cColumnasConf.SetDataContext(this.Context);
            TablasModeloDatosController cTablas = new TablasModeloDatosController();
            TablasModeloDatos oTabla;
            JsonObject auxJson;
            string query = "", orderby = "";
            try
            {
                oAtributo = GetItem(lAtributoID);
                List<TablasModeloDatos> listaTablasCargadas = cTablas.GetActivos((long)oAtributo.ClienteID);
                listaColumnas = cColumnas.GetColumnasFromAtributo(lAtributoID);

                string sTablas = "";

                query = "SELECT ";
                if (listaColumnas != null)
                {

                    query += " " + oAtributo.TablasModeloDatos.NombreTabla + "." + oAtributo.TablasModeloDatos.Indice;
                    sTablas += oAtributo.TablasModeloDatos.NombreTabla;
                    int cont = 0;
                    foreach (var item in listaColumnas)
                    {
                        listaTablasAtr = JsonConvert.DeserializeObject<List<TablaAtributo>>(item.JsonRutaColumna);
                        if (listaTablasAtr.Count == 0)
                        {
                            query += ", " + item.ColumnasModeloDatos.TablasModeloDatos.NombreTabla + "." + item.ColumnasModeloDatos.NombreColumna;
                            if (sTablas == "")
                            {
                                sTablas = item.ColumnasModeloDatos.TablasModeloDatos.NombreTabla;
                            }
                            if (item == listaColumnas.First())
                            {
                                orderby = " ORDER BY " + item.ColumnasModeloDatos.TablasModeloDatos.NombreTabla + "." + item.ColumnasModeloDatos.NombreColumna;
                            }
                        }
                        else
                        {
                            string refAnterior = oAtributo.TablasModeloDatos.NombreTabla;
                            foreach (var oTbl in listaTablasAtr)
                            {
                                oTabla = (from c in listaTablasCargadas where c.TablaModeloDatosID == oTbl.TablaID select c).FirstOrDefault();
                                if (oTabla != null)
                                {
                                    if (sTablas == "")
                                    {
                                        sTablas = oTabla.NombreTabla;
                                    }
                                    sTablas += " JOIN " + oTabla.NombreTabla + " Tbl" + cont + " ON " + refAnterior + "." + oTbl.IndiceColumna + " = " + " Tbl" + cont + "." + item.ColumnasModeloDatos.TablasModeloDatos.Indice;
                                    refAnterior = "Tbl" + cont;
                                    cont++;
                                }
                            }
                            query += ", " + refAnterior + "." + item.ColumnasModeloDatos.NombreColumna;
                            if (item == listaColumnas.First())
                            {
                                orderby = " ORDER BY " + refAnterior + "." + item.ColumnasModeloDatos.NombreColumna;
                            }
                        }
                    }
                }
                query += " FROM " + sTablas + orderby;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                query = "";
            }
            return query;
        }

        public List<Ext.Net.ListItem> GetItemsComboBoxByColumnaModeloDatosID(long lAtributoID)
        {
            List<Ext.Net.ListItem> listaDatos = new List<Ext.Net.ListItem>();
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            ColumnasModeloDatos oColumna, oColumnaAux;
            List<ColumnasModeloDatos> listaColumnas;
            try
            {
                string query = GenerarQuery(lAtributoID);
                DataTable result = this.EjecutarQuery(query);

                Ext.Net.ListItem newListItem;
                if (result != null)
                {
                    foreach (System.Data.DataRow fila in result.Rows)
                    {
                        string text = "";
                        string textAux = "";
                        newListItem = new Ext.Net.ListItem();

                        newListItem.Value = fila.ItemArray[0].ToString();
                        //text = fila[0].ToString();
                        for (int i = 1; i < fila.ItemArray.Length; i++)
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
                        newListItem.Text = textAux;
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

        public JsonObject GetJsonItemsComboBoxByColumnaModeloDatosID(long lAtributoID)
        {
            JsonObject listaDatos = new JsonObject();
            try
            {
                string query = GenerarQuery(lAtributoID);
                DataTable result = this.EjecutarQuery(query);

                JsonObject auxJson;
                if (result != null)
                {
                    foreach (System.Data.DataRow fila in result.Rows)
                    {
                        string text = "";
                        string textAux = "";
                        auxJson = new JsonObject();

                        //text = fila[1].ToString();
                        for (int i = 1; i < fila.ItemArray.Length; i++)
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
                        auxJson.Add("Value", fila.ItemArray[0].ToString());
                        auxJson.Add("Text", textAux);
                        if (!listaDatos.ContainsKey(fila.ItemArray[0].ToString()))
                        {
                            listaDatos.Add(fila.ItemArray[0].ToString(), auxJson);
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

        public JsonObject GetJsonItemsByColumnaModeloDatosID(long lAtributoID)
        {
            JsonObject listaDatos = new JsonObject();
            try
            {
                string query = GenerarQuery(lAtributoID);

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

                        text = fila[1].ToString();
                        auxJson.Add("Value", fila.ItemArray[0].ToString());
                        auxJson.Add("Text", text);
                        if (!(listaDatos.TryGetValue(fila.ItemArray[0].ToString(), out oAux) && oAux != null))
                        {
                            listaDatos.Add(fila.ItemArray[0].ToString(), auxJson);
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

        public JsonObject GetJsonItemsByColumnaModeloDatosIDServicio(long lAtributoID)
        {
            JsonObject listaDatos = new JsonObject();
            try
            {
                string query = GenerarQuery(lAtributoID);
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

                        text = fila[1].ToString();
                        auxJson.Add("Value", fila.ItemArray[0].ToString());
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

        public List<string> GetValoresListStringComboBoxByColumnaModeloDatosID(long lAtributoID)
        {
            List<string> ValoresPosibles = new List<string>();
            try
            {
                string query = GenerarQuery(lAtributoID);
                DataTable result = this.EjecutarQuery(query);

                if (result != null)
                {
                    foreach (System.Data.DataRow fila in result.Rows)
                    {
                        ValoresPosibles.Add(fila[1].ToString());
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

        public string GetValoresStringComboBoxByColumnaModeloDatosID(long lAtributoID)
        {
            string ValoresPosibles = "";
            try
            {
                string query = GenerarQuery(lAtributoID);
                DataTable result = this.EjecutarQuery(query);
                if (result != null)
                {
                    foreach (System.Data.DataRow fila in result.Rows)
                    {
                        if (ValoresPosibles == "")
                        {
                            ValoresPosibles = fila[1].ToString();
                        }
                        else
                        {
                            ValoresPosibles += ";" + fila[1].ToString();
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

        public Dictionary<string, List<string>> GetAtributosObligatorios(long clienteID)
        {
            Dictionary<string, List<string>> diccionario = new Dictionary<string, List<string>>();

            try
            {
                var lista = (from tipDat in Context.TiposDatosPropiedades
                             join attTipDat in Context.EmplazamientosAtributosTiposDatosPropiedades on tipDat.TipoDatoPropiedadID equals attTipDat.TipoDatoPropiedadID
                             join attConfig in Context.EmplazamientosAtributosConfiguracion on attTipDat.EmplazamientoAtributoConfiguracionID equals attConfig.EmplazamientoAtributoConfiguracionID
                             join attCategory in Context.EmplazamientosAtributosCategorias on attConfig.EmplazamientoAtributoCategoriaID equals attCategory.EmplazamientoAtributoCategoriaID
                             where
                                attConfig.ClienteID == clienteID &&
                                tipDat.Codigo == "AllowBlank" &&
                                attTipDat.Valor == false.ToString() &&
                                attConfig.Activo
                             select new
                             {
                                 category = attCategory.Nombre,
                                 attributo = attConfig.CodigoAtributo
                             }).ToList();

                if (lista != null)
                {
                    lista.ForEach(elemento =>
                    {
                        if (diccionario.ContainsKey(elemento.category))
                        {
                            diccionario[elemento.category].Add(elemento.attributo);
                        }
                        else
                        {
                            List<string> atts = new List<string>();
                            atts.Add(elemento.attributo);
                            diccionario.Add(elemento.category, atts);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                diccionario = new Dictionary<string, List<string>>();
            }
            return diccionario;
        }


        public EmplazamientosAtributosConfiguracion GetByCodeAndCategory(string codigo, string category, long clienteID)
        {
            EmplazamientosAtributosConfiguracion emplAtrConf;
            try
            {
                emplAtrConf = (from conf in Context.EmplazamientosAtributosConfiguracion
                               join cat in Context.EmplazamientosAtributosCategorias on conf.EmplazamientoAtributoCategoriaID equals cat.EmplazamientoAtributoCategoriaID
                               where
                                  conf.CodigoAtributo == codigo &&
                                  cat.Nombre == category &&
                                  conf.ClienteID == clienteID
                               select conf).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                emplAtrConf = null;
            }

            return emplAtrConf;
        }

        public List<EmplazamientosAtributosConfiguracion> GetActivos(long clienteID)
        {
            List<EmplazamientosAtributosConfiguracion> lista;
            try
            {
                lista = (from c in Context.EmplazamientosAtributosConfiguracion
                         where c.ClienteID == clienteID
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }
            return lista;
        }

        public List<long> GetAtributosObligatoriosIDs(long clienteID)
        {
            List<long> listaDatos;
            try
            {
                listaDatos = (from c in Context.EmplazamientosAtributosConfiguracion
                              join atrProp in Context.EmplazamientosAtributosTiposDatosPropiedades on c.EmplazamientoAtributoConfiguracionID equals atrProp.EmplazamientoAtributoConfiguracionID
                              join prop in Context.TiposDatosPropiedades on atrProp.TipoDatoPropiedadID equals prop.TipoDatoPropiedadID
                              where c.ClienteID == clienteID && prop.Codigo == "AllowBlank" && atrProp.Valor == "False"
                              select c.EmplazamientoAtributoConfiguracionID).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public bool AtributoVisible(long AtributoID, long UsuarioID)
        {
            List<EmplazamientosAtributosConfiguracion> listaDatos;
            bool bVisible = true;
            try
            {
                listaDatos = (from atr in Context.EmplazamientosAtributosConfiguracion
                              join atrRol in Context.EmplazamientosAtributosRolesRestringidos on atr.EmplazamientoAtributoConfiguracionID equals atrRol.EmplazamientoAtributoConfiguracionID
                              join usuRol in Context.UsuariosRoles on atrRol.RolID equals usuRol.RolID
                              where atr.EmplazamientoAtributoConfiguracionID == AtributoID && usuRol.UsuarioID == UsuarioID && atrRol.Restriccion == (int)Comun.RestriccionesAtributoDefecto.HIDDEN
                              select atr).ToList();
                if (listaDatos.Count > 0)
                {
                    return false;
                }
                listaDatos = (from atr in Context.EmplazamientosAtributosConfiguracion
                              join atrRol in Context.EmplazamientosAtributosRolesRestringidos on atr.EmplazamientoAtributoConfiguracionID equals atrRol.EmplazamientoAtributoConfiguracionID
                              join usuRol in Context.UsuariosRoles on atrRol.RolID equals usuRol.RolID
                              where atr.EmplazamientoAtributoConfiguracionID == AtributoID && usuRol.UsuarioID == UsuarioID && atrRol.Restriccion == (int)Comun.RestriccionesAtributoDefecto.ACTIVE
                              select atr).ToList();
                if (listaDatos.Count > 0)
                {
                    return true;
                }
                listaDatos = (from atr in Context.EmplazamientosAtributosConfiguracion
                              join atrRol in Context.EmplazamientosAtributosRolesRestringidos on atr.EmplazamientoAtributoConfiguracionID equals atrRol.EmplazamientoAtributoConfiguracionID
                              where atr.EmplazamientoAtributoConfiguracionID == AtributoID && atrRol.Restriccion == (int)Comun.RestriccionesAtributoDefecto.HIDDEN && !atrRol.RolID.HasValue
                              select atr).ToList();
                if (listaDatos.Count > 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
            return bVisible;
        }

    }
}