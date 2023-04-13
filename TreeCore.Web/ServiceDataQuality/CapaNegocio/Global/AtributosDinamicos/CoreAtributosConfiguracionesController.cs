using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using System.Transactions;
using System.Collections;
using System.Data;
using System.Reflection;
using Tree.Linq.GenericExtensions;
using System.Text.RegularExpressions;
using Ext.Net;
using Newtonsoft.Json;


namespace CapaNegocio
{
    public class CoreAtributosConfiguracionesController : GeneralBaseController<CoreAtributosConfiguraciones, TreeCoreContext>
    {
        public CoreAtributosConfiguracionesController()
            : base()
        { }

        public CoreAtributosConfiguraciones GetAtributoByCodigo(string sCodigo)
        {
            CoreAtributosConfiguraciones oDato;
            try
            {
                oDato = (from c in Context.CoreAtributosConfiguraciones where c.Codigo == sCodigo select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public CoreAtributosConfiguraciones GetAtributoByCodigo(string sCodigo, long lClienteID)
        {
            CoreAtributosConfiguraciones oDato;
            try
            {
                oDato = (from c in Context.CoreAtributosConfiguraciones where c.Codigo == sCodigo && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public CoreAtributosConfiguraciones GetAtributoByCodigoyCategoria(string sCodigo, long lCategoriaID)
        {
            CoreAtributosConfiguraciones oDato;
            try
            {
                oDato = (from c in Context.CoreAtributosConfiguraciones
                         join confAtr in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c.CoreAtributoConfiguracionID equals confAtr.CoreAtributoConfiguracionID
                         join vin in Context.CoreInventarioCategoriasAtributosCategorias on confAtr.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals vin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                         where c.Codigo == sCodigo && vin.InventarioCategoriaID == lCategoriaID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public CoreAtributosConfiguraciones GetAtributoByCodigoyCategoriaV2(string sCodigo, long lCategoriaID)
        {
            CoreAtributosConfiguraciones oDato;
            try
            {
                oDato = (from c in Context.CoreAtributosConfiguraciones
                         join confAtr in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c.CoreAtributoConfiguracionID equals confAtr.CoreAtributoConfiguracionID
                         join confCat in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on confAtr.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals confCat.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                         join catAtr in Context.InventarioAtributosCategorias on confCat.InventarioAtributoCategoriaID equals catAtr.InventarioAtributoCategoriaID
                         join vin in Context.CoreInventarioCategoriasAtributosCategorias on confAtr.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals vin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                         where c.Codigo == sCodigo && vin.InventarioCategoriaID == lCategoriaID && !catAtr.EsPlantilla
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public CoreAtributosConfiguraciones GetAtributoByCodigoyCategoriaConfiguracion(string sCodigo, long lCategoriaConfID)
        {
            CoreAtributosConfiguraciones oDato;
            try
            {
                oDato = (from c in Context.CoreAtributosConfiguraciones
                         join confAtr in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c.CoreAtributoConfiguracionID equals confAtr.CoreAtributoConfiguracionID
                         where c.Codigo == sCodigo && confAtr.CoreInventarioCategoriaAtributoCategoriaConfiguracionID == lCategoriaConfID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public CoreAtributosConfiguraciones GetAtributoByNombreyCategoria(string sNombre, long lCategoriaID)
        {
            CoreAtributosConfiguraciones oDato;
            try
            {
                oDato = (from c in Context.CoreAtributosConfiguraciones
                         join confAtr in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c.CoreAtributoConfiguracionID equals confAtr.CoreAtributoConfiguracionID
                         join vin in Context.CoreInventarioCategoriasAtributosCategorias on confAtr.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals vin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                         where c.Nombre == sNombre && vin.InventarioCategoriaID == lCategoriaID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public bool EliminarAtributo(long lAtributoID)
        {
            CoreAtributosConfiguracionListasColumnasAdicionalesController cColumnas = new CoreAtributosConfiguracionListasColumnasAdicionalesController();
            cColumnas.SetDataContext(this.Context);
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController cAtrVin = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController();
            cAtrVin.SetDataContext(this.Context);

            bool bCorrecto = true;
            if (cAtrVin.AtributoUsado(lAtributoID))
            {
                return false;
            }
            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    List<CoreAtributosConfiguracionListasColumnasAdicionales> listaColumnas = cColumnas.GetColumnasAtributos(lAtributoID);
                    if (listaColumnas != null)
                    {
                        foreach (var oCol in listaColumnas)
                        {
                            if (!cColumnas.DeleteItem(oCol.CoreAtributoConfiguracionListaColumnaAdicionalID))
                            {
                                trans.Dispose();
                                return false;
                            }
                        }
                    }
                    var listaVin = cAtrVin.GetVinculaciones(lAtributoID);
                    if (listaVin != null)
                    {
                        foreach (var item in listaVin)
                        {
                            if (!cAtrVin.DeleteItem(item.CoreInventarioCategoriaAtributoCategoriaConfiguracionAtributoID))
                            {
                                trans.Dispose();
                                return false;
                            }
                        }
                    }
                    if (!DeleteItem(lAtributoID))
                    {
                        trans.Dispose();
                        return false;
                    }
                    trans.Complete();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    trans.Dispose();
                    bCorrecto = false;
                }
            }
            return bCorrecto;
        }

        public bool NombreDuplicado(string nombre, long AtributoID)
        {
            List<string> listaNombreAsignados;
            List<string> listaCategoriasAsignadas;
            bool bDuplicado = false;
            try
            {
                listaNombreAsignados = (from atr in Context.CoreAtributosConfiguraciones
                                        join atrConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on atr.CoreAtributoConfiguracionID equals atrConf.CoreAtributoConfiguracionID
                                        join catVin in Context.CoreInventarioCategoriasAtributosCategorias on atrConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                                        where (from atr2 in Context.CoreAtributosConfiguraciones
                                               join atrConf2 in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on atr2.CoreAtributoConfiguracionID equals atrConf2.CoreAtributoConfiguracionID
                                               join catVin2 in Context.CoreInventarioCategoriasAtributosCategorias on atrConf2.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin2.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                                               where atr2.CoreAtributoConfiguracionID == AtributoID
                                               select catVin2.InventarioCategoriaID).ToList().Contains(catVin.InventarioCategoriaID)
                                        select atr.Codigo).ToList();
                if (listaNombreAsignados.Count == 0)
                {
                    listaNombreAsignados = (from atr in Context.CoreAtributosConfiguraciones
                                            join atrConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on atr.CoreAtributoConfiguracionID equals atrConf.CoreAtributoConfiguracionID
                                            where (from atr2 in Context.CoreAtributosConfiguraciones
                                                   join atrConf2 in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on atr2.CoreAtributoConfiguracionID equals atrConf2.CoreAtributoConfiguracionID
                                                   where atr2.CoreAtributoConfiguracionID == AtributoID
                                                   select atrConf2.CoreInventarioCategoriaAtributoCategoriaConfiguracionID).ToList().Contains(atrConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID)
                                            select atr.Codigo).ToList();
                }
                listaCategoriasAsignadas = (from atr in Context.InventarioAtributosCategorias
                                            join conf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on atr.InventarioAtributoCategoriaID equals conf.InventarioAtributoCategoriaID
                                            join atrConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on conf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals atrConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                                            join catVin in Context.CoreInventarioCategoriasAtributosCategorias on atrConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                                            where (from atr2 in Context.CoreAtributosConfiguraciones
                                                   join atrConf2 in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on atr2.CoreAtributoConfiguracionID equals atrConf2.CoreAtributoConfiguracionID
                                                   join catVin2 in Context.CoreInventarioCategoriasAtributosCategorias on atrConf2.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin2.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                                                   where atr2.CoreAtributoConfiguracionID == AtributoID
                                                   select catVin2.InventarioCategoriaID).ToList().Contains(catVin.InventarioCategoriaID)
                                            select atr.InventarioAtributoCategoria).ToList();
                if (listaCategoriasAsignadas.Count == 0)
                {
                    listaCategoriasAsignadas = (from atr in Context.InventarioAtributosCategorias
                                                join conf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on atr.InventarioAtributoCategoriaID equals conf.InventarioAtributoCategoriaID
                                                join atrConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on conf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals atrConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                                                where atrConf.CoreAtributoConfiguracionID == AtributoID
                                                select atr.InventarioAtributoCategoria).ToList();
                }
                listaNombreAsignados.AddRange(listaCategoriasAsignadas);
                return listaNombreAsignados.Contains(nombre);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                bDuplicado = true;
            }


            return bDuplicado;
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
            CoreAtributosConfiguracionListasColumnasAdicionalesController cColumnas = new CoreAtributosConfiguracionListasColumnasAdicionalesController();
            CoreAtributosConfiguraciones oAtributo;
            List<TablaAtributo> listaTablasAtr;
            List<CoreAtributosConfiguracionListasColumnasAdicionales> listaColumnas;
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
                listaColumnas = cColumnas.GetColumnasAtributos(lAtributoID);

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
                query += " FROM " + sTablas;

                #region ORDER

                query += orderby;

                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                query = "";
            }
            return query;
        }

        public List<Ext.Net.ListItem> GetItemsComboBox(long lAtributoID)
        {
            List<Ext.Net.ListItem> listaDatos = new List<Ext.Net.ListItem>();
            TablasModeloDatosController cTablas = new TablasModeloDatosController();
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

        public JsonObject GetJsonItemsComboBox(long lAtributoID)
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

        public JsonObject GetJsonItems(long lAtributo)
        {
            JsonObject listaDatos = new JsonObject();
            CoreAtributosConfiguraciones oAtributo;
            JsonObject auxJson;
            try
            {
                oAtributo = GetItem(lAtributo);
                if (oAtributo.TablaModeloDatoID != null)
                {
                    string query = GenerarQuery(lAtributo);
                    DataTable result = this.EjecutarQuery(query);

                    if (result != null)
                    {
                        foreach (System.Data.DataRow fila in result.Rows)
                        {
                            string text = "";
                            string textAux = "";
                            object oAux = null;

                            auxJson = new JsonObject();
                            text = fila[1].ToString();
                            auxJson.Add("Value", fila.ItemArray[0].ToString());
                            auxJson.Add("Text", text);
                            if (!(listaDatos.TryGetValue(text, out oAux) && oAux != null))
                            {
                                listaDatos.Add(fila.ItemArray[0].ToString(), auxJson);
                            }
                        }
                    }
                }
                else
                {
                    object oAux;
                    foreach (var item in oAtributo.ValoresPosibles.Split(';').ToList())
                    {
                        if (!listaDatos.TryGetValue(item, out oAux))
                        {
                            auxJson = new JsonObject();
                            auxJson.Add("Text", item);
                            auxJson.Add("Value", item);
                            listaDatos.Add(item, auxJson);
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

        public string GetQueryItemsFiltros(long lAtributo)
        {
            CoreAtributosConfiguraciones oAtributo;
            string query = "";
            try
            {
                oAtributo = GetItem(lAtributo);

                if (oAtributo.TablaModeloDatoID != null)
                {
                    query = GenerarQuery(lAtributo);
                }
                else
                {
                    query = "Select distinct AuxLista.Texto, AuxLista.Texto From ( VALUES ";
                    foreach (var item in oAtributo.ValoresPosibles.Split(';').ToList())
                    {
                        if (query == "Select distinct AuxLista.Texto, AuxLista.Texto From ( VALUES ")
                        {
                            query += "('" + item + "')";
                        }
                        else
                        {
                            query += ",('" + item + "')";
                        }
                    }
                    query += ") AS AuxLista (Texto)";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            return query;
        }

        public JsonObject GetJsonItemsServicio(long lAtributo)
        {
            JsonObject listaDatos = new JsonObject();
            CoreAtributosConfiguraciones oAtributo;
            JsonObject auxJson;
            try
            {
                oAtributo = GetItem(lAtributo);

                if (oAtributo.TablaModeloDatoID != null)
                {

                    string query = GenerarQuery(lAtributo);
                    DataTable result = this.EjecutarQuery(query);

                    if (result != null)
                    {
                        foreach (System.Data.DataRow fila in result.Rows)
                        {
                            string text = "";
                            string textAux = "";
                            object oAux = null;

                            auxJson = new JsonObject();
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
                else
                {
                    object oAux;
                    foreach (var item in oAtributo.ValoresPosibles.Split(';').ToList())
                    {
                        if (!listaDatos.TryGetValue(item, out oAux))
                        {
                            auxJson = new JsonObject();
                            auxJson.Add("Text", item);
                            auxJson.Add("Value", item);
                            listaDatos.Add(item, auxJson);
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

        public JsonObject GetJsonItemsExport(long lAtributo)
        {
            JsonObject listaDatos = new JsonObject();
            CoreAtributosConfiguraciones oAtributo;
            try
            {
                oAtributo = GetItem(lAtributo);

                List<string> listaTablas = new List<string>();

                if (oAtributo.TablaModeloDatoID != null)
                {

                    string query = GenerarQuery(lAtributo);
                    DataTable result = this.EjecutarQuery(query);

                    if (result != null)
                    {
                        foreach (System.Data.DataRow fila in result.Rows)
                        {
                            string text = "";
                            string textAux = "";
                            string value = fila.ItemArray[0].ToString();
                            object oAux = null;

                            text = fila[1].ToString();

                            if (!(listaDatos.TryGetValue(text, out oAux) && oAux != null))
                            {
                                listaDatos.Add(value, text);
                            }
                        }
                    }
                }
                else
                {
                    object oAux;
                    foreach (var item in oAtributo.ValoresPosibles.Split(';').ToList())
                    {
                        if (!listaDatos.TryGetValue(item, out oAux))
                        {
                            listaDatos.Add(item, item);
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

        public List<string> GetValoresListStringComboBox(long lAtributoID)
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

        public string GetValoresStringComboBox(long lAtributoID)
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


        public Vw_ConfCoreAtributosConfiguraciones DatosInventarioMigrador(string Nombre)
        {
            Vw_ConfCoreAtributosConfiguraciones datos = new Vw_ConfCoreAtributosConfiguraciones();
            try
            {
                datos = (from c in Context.Vw_ConfCoreAtributosConfiguraciones where c.Nombre == Nombre select c).First();
                return datos;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public bool AtributoVisible(long AtributoID, long UsuarioID)
        {
            List<CoreAtributosConfiguraciones> listaDatos;
            bool bVisible = true;
            try
            {
                listaDatos = (from atr in Context.CoreAtributosConfiguraciones
                              join atrRol in Context.CoreAtributosConfiguracionRolesRestringidos on atr.CoreAtributoConfiguracionID equals atrRol.CoreAtributoConfiguracionID
                              join usuRol in Context.UsuariosRoles on atrRol.RolID equals usuRol.RolID
                              where atr.CoreAtributoConfiguracionID == AtributoID && usuRol.UsuarioID == UsuarioID && atrRol.Restriccion == (int)Comun.RestriccionesAtributoDefecto.HIDDEN
                              select atr).ToList();
                if (listaDatos.Count > 0)
                {
                    return false;
                }
                listaDatos = (from atr in Context.CoreAtributosConfiguraciones
                              join atrRol in Context.CoreAtributosConfiguracionRolesRestringidos on atr.CoreAtributoConfiguracionID equals atrRol.CoreAtributoConfiguracionID
                              join usuRol in Context.UsuariosRoles on atrRol.RolID equals usuRol.RolID
                              where atr.CoreAtributoConfiguracionID == AtributoID && usuRol.UsuarioID == UsuarioID && atrRol.Restriccion == (int)Comun.RestriccionesAtributoDefecto.ACTIVE
                              select atr).ToList();
                if (listaDatos.Count > 0)
                {
                    return true;
                }
                listaDatos = (from atr in Context.CoreAtributosConfiguraciones
                              join atrRol in Context.CoreAtributosConfiguracionRolesRestringidos on atr.CoreAtributoConfiguracionID equals atrRol.CoreAtributoConfiguracionID
                              where atr.CoreAtributoConfiguracionID == AtributoID && atrRol.Restriccion == (int)Comun.RestriccionesAtributoDefecto.HIDDEN && !atrRol.RolID.HasValue
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