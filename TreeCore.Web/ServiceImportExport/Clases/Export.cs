using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using TreeCore.Data;

namespace TreeCore
{
    public class Export
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const string DINAMICO = "DINAMICO";
        public const string ESTATICO = "ESTATICO";
        private static CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
        private static string dateTimePattern = cultureInfo.DateTimeFormat.SortableDateTimePattern;

        #region GetBodyExcel
        public static List<JsonObject> GetBodyExcel(CoreExportacionDatosPlantillas plantilla, List<JsonObject> listaIn, List<CoreExportacionDatosPlantillasColumnas> columnas,
            List<CoreExportacionDatosPlantillasCeldas> celdas, List<CoreExportacionDatosPlantillasFilas> filas, 
            Dictionary<string, string> formatosFecha, out Dictionary<string, string> outformatosFecha, CoreExportacionDatosPlantillasFilas filaID,
            Dictionary<long, ColumnasModeloDatos> dicColumnasModeloDatos, Dictionary<long, string> dicNombresTablas,
            Dictionary<long, string> dicControladores, Dictionary<long, string> dicDisplay, Dictionary<long, TiposDatos> dicTiposDatos,
            List<Vw_CoreExportacionDatosPlantillasReglasCeldas> reglasCelda)
        {
            ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();
            List<JsonObject> listaOut = new List<JsonObject>();


            try
            {
                if (listaIn != null)
                {
                    int index = 0;
                    foreach (JsonObject item in listaIn)
                    {
                        index++;
                        log.Info("Procesing row: " + index + "/" + listaIn.Count);
                        JsonObject itemOut = new JsonObject();

                        if (filas.Count == 1 && !plantilla.ColumnaModeloDatoID.HasValue)
                        {
                            #region Una sola fila sin categoría
                            itemOut = buildColumnsExcel(columnas, celdas, filas[0], item, itemOut, dicNombresTablas, dicControladores, dicDisplay, dicTiposDatos, formatosFecha, out formatosFecha, reglasCelda);
                            #endregion
                        }
                        else if (plantilla.ColumnaModeloDatoID.HasValue)
                        {
                            #region Una o varias filas por categoría
                            ColumnasModeloDatos colModDatFiltro = null;
                            if (plantilla.ColumnaModeloDatoID.HasValue && !dicColumnasModeloDatos.ContainsKey(plantilla.ColumnaModeloDatoID.Value))
                            {
                                colModDatFiltro = cColumnasModeloDatos.GetItem(plantilla.ColumnaModeloDatoID.Value);
                                if (colModDatFiltro != null)
                                {
                                    dicColumnasModeloDatos.Add(plantilla.ColumnaModeloDatoID.Value, colModDatFiltro);
                                }
                            }
                            else if (plantilla.ColumnaModeloDatoID.HasValue)
                            {
                                colModDatFiltro = dicColumnasModeloDatos[plantilla.ColumnaModeloDatoID.Value];
                            }

                            if (filaID.TipoFiltroID.HasValue && item.ContainsKey(colModDatFiltro.NombreColumna) &&
                                item[colModDatFiltro.NombreColumna].ToString() == filaID.TipoFiltroID.Value.ToString())
                            {
                                itemOut = buildColumnsExcel(columnas, celdas, filaID, item, itemOut, dicNombresTablas, dicControladores, dicDisplay, dicTiposDatos, formatosFecha, out formatosFecha, reglasCelda);
                            }
                            #endregion
                        }

                        if (itemOut != null && itemOut.Count > 0)
                        {
                            log.Info("Item added");
                            listaOut.Add(itemOut);
                        }
                        else
                        {
                            log.Info("Item not added");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                listaOut = new List<JsonObject>();
            }
            outformatosFecha = formatosFecha;

            return listaOut;
        }
        #endregion

        #region buildColumnsExcel
        public static JsonObject buildColumnsExcel(List<CoreExportacionDatosPlantillasColumnas> columnas, List<CoreExportacionDatosPlantillasCeldas> celdas,
            CoreExportacionDatosPlantillasFilas fila, JsonObject item, JsonObject itemOut,
            Dictionary<long, string> dicNombresTablas,
            Dictionary<long, string> dicControladores,
            Dictionary<long, string> dicDisplay,
            Dictionary<long, TiposDatos> dicTiposDatos,
            Dictionary<string, string> formatosFecha, out Dictionary<string, string> outFormatosFecha,
            List<Vw_CoreExportacionDatosPlantillasReglasCeldas> reglasCelda)
        {
            ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();
            TiposDatosController cTiposDatos = new TiposDatosController();

            Dictionary<long, CoreAtributosConfiguraciones> dicCoreAtributosConfiguraciones = new Dictionary<long, CoreAtributosConfiguraciones>();
            
            JsonObject jsonListas = new JsonObject();

            try
            {
                foreach (CoreExportacionDatosPlantillasColumnas col in columnas)
                {
                    List<CoreExportacionDatosPlantillasCeldas> celdasTmp = celdas.FindAll(c => c.CoreExportacionDatosPlantillaColumnaID == col.CoreExportacionDatosPlantillaColumnaID && c.CoreExportacionDatosPlantillaFilaID == fila.CoreExportacionDatosPlantillaFilaID);
                    string valorCelda = "";
                    string separador = "";
                    List<long> idsOrdenados = new List<long>();
                    string colIdentificador = col.CoreExportacionDatosPlantillaColumnaID.ToString();

                    idsOrdenados = ordenarCeldasConcatenadas(celdasTmp, null, idsOrdenados);

                    foreach (long id in idsOrdenados)
                    {
                        CoreExportacionDatosPlantillasCeldas tmpCeld = celdasTmp.Find(c => c.CoreExportacionDatosPlantillasCeldasID == id);
                        
                        string valorCeldaTmp = separador;

                        if (tmpCeld.TextoFijo != null)
                        {
                            valorCeldaTmp += tmpCeld.TextoFijo;
                        }
                        else if (tmpCeld.ColumnasModeloDatoID.HasValue)
                        {
                            #region ColumnasModeloDatoID
                            ColumnasModeloDatos colModDat = cColumnasModeloDatos.GetItem(tmpCeld.ColumnasModeloDatoID.Value);
                            if (colModDat != null)
                            {
                                if (item.ContainsKey(colModDat.NombreColumna))
                                {
                                    TiposDatos tipoDato = null;
                                    if (colModDat.TipoDatoID.HasValue)
                                    {
                                        if (!dicTiposDatos.ContainsKey(colModDat.TipoDatoID.Value))
                                        {
                                            tipoDato = cTiposDatos.GetItem(colModDat.TipoDatoID.Value);
                                            if (tipoDato != null)
                                            {
                                                dicTiposDatos.Add(colModDat.TipoDatoID.Value, tipoDato);
                                            }
                                        }
                                        else
                                        {
                                            tipoDato = dicTiposDatos[colModDat.TipoDatoID.Value];
                                        }
                                    }

                                    if (colModDat.ForeignKeyID.HasValue)
                                    {
                                        #region Get info to execute query
                                        string sNombreTabla;
                                        if (dicNombresTablas.ContainsKey(colModDat.ForeignKeyID.Value))
                                        {
                                            sNombreTabla = dicNombresTablas[colModDat.ForeignKeyID.Value];
                                        }
                                        else
                                        {
                                            sNombreTabla = cColumnasModeloDatos.getDataSourceTablaColumna(colModDat.ForeignKeyID.Value);
                                            dicNombresTablas.Add(colModDat.ForeignKeyID.Value, sNombreTabla);
                                        }

                                        string sControlador;
                                        if (dicControladores.ContainsKey(colModDat.ForeignKeyID.Value))
                                        {
                                            sControlador = dicControladores[colModDat.ForeignKeyID.Value];
                                        }
                                        else
                                        {
                                            sControlador = cColumnasModeloDatos.getControllerColumna(colModDat.ForeignKeyID.Value);
                                            dicControladores.Add(colModDat.ForeignKeyID.Value, sControlador);
                                        }

                                        string sDisplay;
                                        if (dicDisplay.ContainsKey(colModDat.ColumnaModeloDatosID))
                                        {
                                            sDisplay = dicDisplay[colModDat.ColumnaModeloDatosID];
                                        }
                                        else
                                        {
                                            sDisplay = cColumnasModeloDatos.getDisplay(colModDat.ColumnaModeloDatosID);
                                            dicDisplay.Add(colModDat.ColumnaModeloDatosID, sDisplay);
                                        }
                                        #endregion

                                        #region Obtener valor desde foreing key
                                        Type tipo = Type.GetType("TreeCore.Data." + sNombreTabla.Split('.')[1]);
                                        Type tipoControlador = Type.GetType("CapaNegocio." + sControlador);

                                        if (tipo == null)
                                        {
                                            tipo = Type.GetType("TreeCore.Data.Vw_" + sNombreTabla.Split('_')[1]);
                                        }

                                        if (tipoControlador.BaseType.GenericTypeArguments[0].FullName != tipo.FullName)
                                        {
                                            tipo = Type.GetType(tipoControlador.BaseType.GenericTypeArguments[0].FullName);
                                        }

                                        var instance = Activator.CreateInstance(tipoControlador);
                                        MethodInfo[] methodos = tipoControlador.GetMethods();

                                        foreach (MethodInfo met in methodos)
                                        {
                                            if (met.Name == "GetItem" && met.ReturnType == tipo)
                                            {
                                                var Params = met.GetParameters();
                                                if (Params[0].ParameterType.Name == "Int64")
                                                {
                                                    if (!string.IsNullOrEmpty(item[colModDat.NombreColumna].ToString()))
                                                    {
                                                        object result = met.Invoke(instance, new object[] { long.Parse(item[colModDat.NombreColumna].ToString()) });
                                                        PropertyInfo propName = tipo.GetProperty(sDisplay);

                                                        if (propName != null)
                                                        {
                                                            string value = propName.GetValue(result, null).ToString();
                                                            
                                                            if (tipoDato != null && tipoDato.Codigo == Comun.TIPODATO_CODIGO_FECHA)
                                                            {
                                                                #region TIPOS FECHA
                                                                string sFecha = ParseDate(value, null);
                                                                sFecha = AplicarReglasTransformacion(sFecha, tmpCeld, tipoDato.Codigo, reglasCelda);

                                                                DateTime date;
                                                                //DateTime.TryParse(sFecha, out date);
                                                                DateTime.TryParseExact(sFecha, dateTimePattern, cultureInfo, DateTimeStyles.None, out date);

                                                                if (date != null && DateTime.MinValue != date)
                                                                {
                                                                    sFecha = ParseDate(sFecha, tmpCeld.FormatoFecha);
                                                                    if (!formatosFecha.ContainsKey(colIdentificador))
                                                                    {
                                                                        formatosFecha.Add(colIdentificador, tmpCeld.FormatoFecha);
                                                                    }
                                                                }

                                                                valorCeldaTmp += sFecha;
                                                                #endregion
                                                            }
                                                            else
                                                            {
                                                                #region OTROS TIPOS (TEXTO)
                                                                valorCeldaTmp += AplicarReglasTransformacion(value, tmpCeld, tipoDato.Codigo, reglasCelda);
                                                                #endregion
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        if (tipoDato != null && tipoDato.Codigo == Comun.TIPODATO_CODIGO_FECHA)
                                        {
                                            #region TIPOS FECHA
                                            string sFecha = ParseDate(item[colModDat.NombreColumna].ToString(), null);
                                            sFecha = AplicarReglasTransformacion(sFecha, tmpCeld, tipoDato.Codigo, reglasCelda);
                                            DateTime date;
                                            //DateTime.TryParse(sFecha, out date);
                                            DateTime.TryParseExact(sFecha, "d/M/yyyy", cultureInfo, DateTimeStyles.None, out date);

                                            if (date != null && DateTime.MinValue != date)
                                            {
                                                sFecha = ParseDate(date.ToString(), tmpCeld.FormatoFecha);
                                                if (!formatosFecha.ContainsKey(colIdentificador))
                                                {
                                                    formatosFecha.Add(colIdentificador, tmpCeld.FormatoFecha);
                                                }
                                            }

                                            valorCeldaTmp += sFecha;
                                            #endregion
                                        }
                                        else
                                        {
                                            #region OTROS TIPOS (TEXTO)
                                            valorCeldaTmp += AplicarReglasTransformacion(item[colModDat.NombreColumna].ToString(), tmpCeld, tipoDato.Codigo, reglasCelda);
                                            #endregion
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (tmpCeld.CoreAtributosConfiguracionID.HasValue && item.ContainsKey(tmpCeld.CoreAtributosConfiguracionID.Value.ToString()))
                        {
                            #region CoreAtributosConfiguracionID

                            valorCeldaTmp += GetValorCeldaDeTipoDinamico(tmpCeld.CoreAtributosConfiguracionID.Value, tmpCeld.CoreAtributosConfiguracionID.Value.ToString(), item, dicCoreAtributosConfiguraciones, dicTiposDatos, tmpCeld, jsonListas, formatosFecha, out formatosFecha, reglasCelda);

                            #endregion
                        }
                        else if (!string.IsNullOrEmpty(tmpCeld.CampoVinculado))
                        {
                            #region Campo vinculado
                            CampoVinculado campoVinculado = JsonConvert.DeserializeObject<CampoVinculado>(tmpCeld.CampoVinculado);
                            
                            if (campoVinculado.EsDinamico)
                            {

                                valorCeldaTmp += GetValorCeldaDeTipoDinamico(campoVinculado.CampoVinculadoID, campoVinculado.IdRandom, item, dicCoreAtributosConfiguraciones, dicTiposDatos, tmpCeld, jsonListas, formatosFecha, out formatosFecha, reglasCelda);

                            }
                            else
                            {
                                if (campoVinculado.TipoDato != null && campoVinculado.TipoDato == Comun.TIPODATO_CODIGO_FECHA)
                                {
                                    #region TIPOS FECHA
                                    string sFecha = ParseDate(item[campoVinculado.IdRandom].ToString(), null);
                                    sFecha = AplicarReglasTransformacion(sFecha, tmpCeld, campoVinculado.TipoDato, reglasCelda);
                                    DateTime date;
                                    //DateTime.TryParse(sFecha, out date);
                                    DateTime.TryParseExact(sFecha, dateTimePattern, cultureInfo, DateTimeStyles.None, out date);

                                    if (date != null && DateTime.MinValue != date)
                                    {
                                        sFecha = ParseDate(sFecha, tmpCeld.FormatoFecha);
                                        if (!formatosFecha.ContainsKey(colIdentificador))
                                        {
                                            formatosFecha.Add(colIdentificador, tmpCeld.FormatoFecha);
                                        }
                                    }

                                    valorCeldaTmp += sFecha;
                                    #endregion
                                }
                                else
                                {
                                    #region OTROS TIPOS (TEXTO)
                                    object ob;
                                    if (item.TryGetValue(campoVinculado.IdRandom, out ob))
                                    {
                                        valorCeldaTmp += AplicarReglasTransformacion(item[campoVinculado.IdRandom].ToString(), tmpCeld, campoVinculado.TipoDato, reglasCelda);
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }

                        if (!valorCeldaTmp.Equals(separador))
                        {
                            valorCelda += valorCeldaTmp;
                        }

                        if (!string.IsNullOrEmpty(tmpCeld.Separador))
                        {
                            separador = tmpCeld.Separador;
                        }
                    }

                    object obj;
                    if (itemOut.TryGetValue(colIdentificador, out obj))
                    {
                        itemOut[colIdentificador] = valorCelda;
                    }
                    else
                    {
                        itemOut.Add(colIdentificador, valorCelda);
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                itemOut = null;
            }

            #region Return Dictionaries
            outFormatosFecha = formatosFecha;
            #endregion

            return itemOut;
        }
        #endregion

        #region GetValorCeldaDeTipoDinamico
        private static string GetValorCeldaDeTipoDinamico(long CampoVinculadoID, string identificadorColumna, JsonObject item, 
                                                            Dictionary<long, CoreAtributosConfiguraciones> dicCoreAtributosConfiguraciones,
                                                            Dictionary<long, TiposDatos> dicTiposDatos, CoreExportacionDatosPlantillasCeldas tmpCeld,
                                                            JsonObject jsonListas, Dictionary<string, string> formatosFecha, out Dictionary<string, string> outFormatosFecha, 
                                                            List<Vw_CoreExportacionDatosPlantillasReglasCeldas> reglasCelda)
        {
            string valorCelda;
            JsonObject jsonListaValores;

            #region CONTROLLERS
            CoreAtributosConfiguracionesController cCoreAtributosConfiguraciones = new CoreAtributosConfiguracionesController();
            TiposDatosController cTiposDatos = new TiposDatosController();
            #endregion

            try
            {
                
                #region Obtener datos
                CoreAtributosConfiguraciones oAttrConfig;
                if (!dicCoreAtributosConfiguraciones.ContainsKey(CampoVinculadoID))
                {
                    oAttrConfig = cCoreAtributosConfiguraciones.GetItem(CampoVinculadoID);
                    if (oAttrConfig != null)
                    {
                        dicCoreAtributosConfiguraciones.Add(CampoVinculadoID, oAttrConfig);
                    }
                }
                else
                {
                    oAttrConfig = dicCoreAtributosConfiguraciones[CampoVinculadoID];
                }

                TiposDatos tipoDato;
                if (!dicTiposDatos.ContainsKey(oAttrConfig.TipoDatoID))
                {
                    tipoDato = cTiposDatos.GetItem(oAttrConfig.TipoDatoID);
                    if (tipoDato != null)
                    {
                        dicTiposDatos.Add(oAttrConfig.TipoDatoID, tipoDato);
                    }
                }
                else
                {
                    tipoDato = dicTiposDatos[oAttrConfig.TipoDatoID];
                }
                #endregion

                if (tipoDato.Codigo == Comun.TIPODATO_CODIGO_LISTA || tipoDato.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                {
                    #region TIPOS LISTA
                    jsonListaValores = new JsonObject();

                    #region jsonListaValores
                    if (!jsonListas.ContainsKey(oAttrConfig.Codigo))
                    {
                        if (oAttrConfig.ColumnaModeloDatoID.HasValue)
                        {
                            jsonListaValores = cCoreAtributosConfiguraciones.GetJsonItemsExport((long)oAttrConfig.CoreAtributoConfiguracionID);
                        }
                        else if (oAttrConfig.ValoresPosibles != null && oAttrConfig.ValoresPosibles != "")
                        {
                            foreach (var it in oAttrConfig.ValoresPosibles.Split(';').ToList())
                            {
                                object oAux;
                                if (!jsonListaValores.TryGetValue(it, out oAux))
                                {
                                    jsonListaValores.Add(it, it);
                                }
                            }
                        }
                        jsonListas.Add(oAttrConfig.Codigo, jsonListaValores);
                    }
                    else
                    {
                        jsonListaValores = (JsonObject)jsonListas[oAttrConfig.Codigo];
                    }
                    #endregion

                    string[] valoresItems = item[identificadorColumna].ToString().Split(',');
                    string valorCeldaTemp = "";
                    foreach (string valorItem in valoresItems)
                    {
                        valorCeldaTemp += string.IsNullOrEmpty(valorCeldaTemp) ? "" : ", ";
                        object obj;
                        if (!string.IsNullOrEmpty(identificadorColumna) &&
                            item.TryGetValue(identificadorColumna, out obj) &&
                            jsonListaValores.TryGetValue(valorItem, out obj))
                        {
                            object value = "";

                            jsonListaValores.TryGetValue(valorItem, out value);

                            if (value != null)
                            {
                                valorCeldaTemp += AplicarReglasTransformacion(value.ToString(), tmpCeld, tipoDato.Codigo, reglasCelda);
                            }
                            else
                            {
                                valorCeldaTemp += valorItem;
                            }
                        }
                        else
                        {
                            valorCeldaTemp += AplicarReglasTransformacion("", tmpCeld, tipoDato.Codigo, reglasCelda);
                        }
                    }
                    valorCelda = valorCeldaTemp;
                    #endregion
                }
                else if (tipoDato.Codigo == Comun.TIPODATO_CODIGO_FECHA)
                {
                    #region TIPOS FECHA
                    string sFecha = ParseDate(item[identificadorColumna].ToString(), null);
                    sFecha = AplicarReglasTransformacion(sFecha, tmpCeld, tipoDato.Codigo, reglasCelda);

                    DateTime date;
                    //DateTime.TryParse(sFecha, out date);
                    DateTime.TryParseExact(sFecha, "d/M/yyyy", cultureInfo, DateTimeStyles.None, out date);

                    if (date != null && DateTime.MinValue != date)
                    {
                        sFecha = ParseDate(date.ToString(), tmpCeld.FormatoFecha); 
                        if (!formatosFecha.ContainsKey(identificadorColumna))
                        {
                            formatosFecha.Add(identificadorColumna, tmpCeld.FormatoFecha);
                        }
                    }

                    valorCelda = sFecha;
                    #endregion
                }
                else
                {
                    #region OTROS TIPOS (TEXTO)
                    valorCelda = AplicarReglasTransformacion(item[identificadorColumna].ToString(), tmpCeld, tipoDato.Codigo, reglasCelda);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                valorCelda = "";
            }

            outFormatosFecha = formatosFecha;

            return valorCelda;
        }
        #endregion

        #region Aplicar reglas de transformación
        public static string AplicarReglasTransformacion(string valor, CoreExportacionDatosPlantillasCeldas celda, string tipoDato, List<Vw_CoreExportacionDatosPlantillasReglasCeldas> reglasCelda)
        {
            bool transformacrionRealizada = false;
            string valorTransformado = valor;




            reglasCelda = reglasCelda.Where(rc => rc.CoreExportacionDatosPlantillasCeldaID == celda.CoreExportacionDatosPlantillasCeldasID).ToList();

            reglasCelda = ordenarListaReglasDeTransformacion(reglasCelda);
            

            foreach (Vw_CoreExportacionDatosPlantillasReglasCeldas regla in reglasCelda)
            {
                if (!transformacrionRealizada)
                {
                    if (regla.CheckValorDefecto)
                    {
                        valorTransformado = regla.ValorDefecto;
                        transformacrionRealizada = true;
                        log.Info("Apply default value(" + tipoDato + "): '" + ((regla.ValorDefecto != null) ? regla.ValorDefecto : "") + "'");
                    }
                    else
                    {
                        log.Info("Apply rule trnasformation(" + tipoDato + "): If '" + valor + "' " + regla.Nombre + " '" + regla.ValorCelda + "', '" + regla.ValorIf + "' else '" + ((regla.ValorDefecto != null) ? regla.ValorDefecto : "") + "'");
                        switch (tipoDato)
                        {
                            case "TEXTO":
                                #region TEXTO
                                switch (regla.Operador)
                                {
                                    case "=":
                                        if (valor == regla.ValorCelda)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "LIKE":
                                        if (valor.Contains(regla.ValorCelda))
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "IS NULL":
                                        if (valor == null)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "= ''":
                                        if (valor == "")
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "IS NOT NULL":
                                        if (valor != null)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "!=":
                                        if (valor != regla.ValorCelda)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "!= ''":
                                        if (valor != "")
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                }
                                break;
                            #endregion
                            case "FECHA":
                                #region FECHA
                                DateTime fechaValor;
                                DateTime.TryParseExact(valor, dateTimePattern, cultureInfo, DateTimeStyles.None, out fechaValor);
                                DateTime fechaCondicion;
                                DateTime.TryParse(regla.ValorCelda, out fechaCondicion);

                                switch (regla.Operador)
                                {
                                    case ">":
                                        if (fechaValor > fechaCondicion)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "<":
                                        if (fechaValor < fechaCondicion)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "=":
                                        if (fechaValor == fechaCondicion)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "IS NULL":
                                        if (fechaValor == null)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                }
                                break;
                            #endregion
                            case "BOOLEAN":
                                #region BOOLEAN
                                bool boolValor;
                                bool.TryParse(valor, out boolValor);
                                bool boolCondicion;
                                bool.TryParse(regla.ValorCelda, out boolCondicion);

                                if (valor == regla.ValorCelda)
                                {
                                    valorTransformado = regla.ValorIf;
                                    transformacrionRealizada = true;
                                }
                                break;
                            #endregion
                            //case "MONEDA":
                            //case "FLOTANTE":
                            case "NUMERICO":
                            case "ENTERO":
                                #region NUMERICO
                                float numValor;
                                float.TryParse(valor, out numValor);
                                float numCondicion;
                                float.TryParse(regla.ValorCelda, out numCondicion);

                                switch (regla.Operador)
                                {
                                    case "=":
                                        if (numValor == numCondicion)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case ">":
                                        if (numValor > numCondicion)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "<":
                                        if (numValor < numCondicion)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "!=":
                                        if (numValor != numCondicion)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case ">=":
                                        if (numValor >= numCondicion)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "<=":
                                        if (numValor <= numCondicion)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "IS NULL":
                                        if (string.IsNullOrEmpty(valor))
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "IS NOT NULL":
                                        if (!string.IsNullOrEmpty(valor))
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                }
                                break;
                            #endregion
                            case "LISTAMULTIPLE":
                            case "LISTA":
                                #region LISTA
                                switch (regla.Operador)
                                {
                                    case "=":
                                        if (valor == regla.ValorCelda)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "IS NULL":
                                        if (valor == regla.ValorCelda)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "IN":
                                        if (regla.ValorCelda.Contains(valor))
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "NOT IN":
                                        if (!regla.ValorCelda.Contains(valor))
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                    case "IS NOT NULL":
                                        if (valor != null)
                                        {
                                            valorTransformado = regla.ValorIf;
                                            transformacrionRealizada = true;
                                        }
                                        break;
                                }
                                break;
                                #endregion
                        }
                    }
                }
                else
                {
                    log.Info("The transformation has already been executed");
                }
            }

            return valorTransformado;
        }
        #endregion

        #region GenerateQuery
        public static string GenerateQuery(CoreExportacionDatosPlantillas plantilla, List<CoreExportacionDatosPlantillasCeldas> celdas, long TablaModeloDatosID, 
            long? ColumnaModeloDatoID, long? limiteRegistros, long? TipoFiltroID, long? TipoFiltroDinamicoID)
        {
            #region CONTROLLERS
            TablasModeloDatosController cTablasModeloDatos = new TablasModeloDatosController();
            ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();
            FiltroInventarioElementosController cFiltroInventarioElementosController = new FiltroInventarioElementosController();
            InventarioCategoriasController cInventarioCategorias = new InventarioCategoriasController();
            CoreAtributosConfiguracionesController cCoreAtributosConfiguraciones = new CoreAtributosConfiguracionesController();
            InventarioCategoriasVinculacionesController cInventarioCategoriasVinculaciones = new InventarioCategoriasVinculacionesController();
            #endregion

            string query = "";
            string select = "SELECT ";
            string top = "TOP";
            string sNombreColumnas = "";
            string from = "FROM";
            string nombreTabla = "";
            string aliasTabla = "c";
            string JOINs = "";
            long incrementalAlias = 0;
            string tablaInventarioElementos = "";
            string where = "";
            List<string> nombreColumnas = new List<string>();
            List<string> nombreColumnasAtributos = new List<string>();
            string nombreColumnaCategoriaID = null;
            List<long> idsCategorias = new List<long>();
            List<InventarioCategoriasVinculaciones> inventarioCategoriasVinculaciones;

            try
            {
                TablasModeloDatos tablaModeloDato = cTablasModeloDatos.GetItem(TablaModeloDatosID);

                if (tablaModeloDato != null)
                {
                    nombreTabla = tablaModeloDato.NombreTabla;
                    sNombreColumnas += aliasTabla + "." + tablaModeloDato.Indice;
                }

                if (ColumnaModeloDatoID.HasValue)
                {
                    ColumnasModeloDatos colModDat = cColumnasModeloDatos.GetItem(ColumnaModeloDatoID.Value);
                    if (colModDat != null && !nombreColumnas.Contains(colModDat.NombreColumna) && !nombreColumnasAtributos.Contains(colModDat.NombreColumna))
                    {
                        nombreColumnas.Add(aliasTabla + "." + colModDat.NombreColumna);
                        nombreColumnaCategoriaID = colModDat.NombreColumna;
                    }

                    if (tablaModeloDato.Indice == "InventarioElementoID")
                    {
                        tablaInventarioElementos = "LEFT JOIN dbo.InventarioElementos i ON " + aliasTabla + ".InventarioElementoID=i.InventarioElementoID";
                    }
                }

                inventarioCategoriasVinculaciones = cInventarioCategoriasVinculaciones.GetItemList();

                foreach (CoreExportacionDatosPlantillasCeldas celda in celdas)
                {
                    if (celda.ColumnasModeloDatoID.HasValue)
                    {
                        ColumnasModeloDatos colModDat = cColumnasModeloDatos.GetItem(celda.ColumnasModeloDatoID.Value);
                        string cname = aliasTabla + "." + colModDat.NombreColumna;

                        if (colModDat != null && !nombreColumnas.Contains(cname) && !nombreColumnasAtributos.Contains(cname))
                        {
                            nombreColumnas.Add(cname);
                        }
                    }
                    else if (celda.CoreAtributosConfiguracionID.HasValue && tablaModeloDato.Indice == "InventarioElementoID")
                    {
                        string sCol = "JSON_VALUE(i.jsonAtributosDinamicos, '$.\"" + celda.CoreAtributosConfiguracionID.Value + "\".\"Valor\"') as '" + celda.CoreAtributosConfiguracionID.Value + "'";
                        if (!nombreColumnasAtributos.Contains(sCol) && !nombreColumnas.Contains(sCol))
                        {
                            nombreColumnasAtributos.Add(sCol);
                        }
                    }
                    else if (!string.IsNullOrEmpty(celda.CampoVinculado))
                    {
                        try
                        {
                            CampoVinculado campoVinculado = JsonConvert.DeserializeObject<CampoVinculado>(celda.CampoVinculado);

                            List<long> categories = new List<long>();
                            List<long> lTablas = new List<long>();
                            List<string> outNombreColumnasAtributos;
                            long outIncrementalAlias = 0;
                            string aliasTablaAnterior = aliasTabla;
                            long idTablaAnterior = plantilla.TablaModeloDatosID;

                            if (campoVinculado.Ruta != null)
                            {
                                campoVinculado.Ruta.path.ForEach(i =>
                                {
                                    if (i.tipo == DINAMICO)
                                    {
                                        categories.Add(i.id);
                                    }
                                    else if (i.tipo == ESTATICO)
                                    {
                                        lTablas.Add(i.id);
                                    }
                                });
                            }

                            List<InventarioCategorias> categorias = cInventarioCategorias.GetCategoriasByCategoriasIDs(categories);
                            List<TablasModeloDatos> tablas = cColumnasModeloDatos.GetTablasByIds(lTablas);

                            if (campoVinculado.Ruta != null)
                            {
                                string aliasInvenElemAnterior = aliasTabla;
                                string aliasTablaAcutal = aliasTablaAnterior;
                                long? idCategoriaAnterior = TipoFiltroID.Value;

                                foreach (ItemPath i in campoVinculado.Ruta.path)
                                {
                                    if (i.tipo == DINAMICO)
                                    {
                                        CoreAtributosConfiguraciones coreAtributoConfiguracion = cCoreAtributosConfiguraciones.GetItem(campoVinculado.CampoVinculadoID);



                                        InventarioCategorias categoria = categorias.Find(cat => cat.InventarioCategoriaID == i.id);
                                        List<InventarioCategoriasVinculaciones> catVinculadasHijos = inventarioCategoriasVinculaciones.FindAll(cat => cat.InventarioCategoriaID == i.id && cat.InventarioCategoriaPadreID == idCategoriaAnterior);
                                        List<InventarioCategoriasVinculaciones> catVinculadasPadre = inventarioCategoriasVinculaciones.FindAll(cat => cat.InventarioCategoriaPadreID == i.id && cat.InventarioCategoriaID == idCategoriaAnterior);

                                        //catVinculadasHijos = catVinculadasHijos.FindAll(x => int.Parse(x.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_1_1 || int.Parse(x.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_1_N || int.Parse(x.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_N_1);
                                        //catVinculadasPadre = catVinculadasPadre.FindAll(x => int.Parse(x.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_1_1 || int.Parse(x.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_1_N || int.Parse(x.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_N_1);

                                        string aliasInvEleVinc = aliasInvenElemAnterior + i.id + incrementalAlias++;
                                        string aliasInvEle = aliasInvenElemAnterior + i.id + incrementalAlias++;

                                        string onJoin1 = "";
                                        string onJoin2 = "";
                                        if (catVinculadasHijos != null && catVinculadasHijos.Count > 0)
                                        {
                                            onJoin1 = aliasInvenElemAnterior + ".InventarioElementoID=" + aliasInvEleVinc + ".InventarioElementoPadreID " + " AND " + aliasInvEleVinc + ".InventarioCategoriaVinculacionID=" + catVinculadasHijos[0].InventarioCategoriaVinculacionID;
                                            onJoin2 = aliasInvEleVinc + ".InventarioElementoID = " + aliasInvEle + ".InventarioElementoID ";
                                        }
                                        else if (catVinculadasPadre != null && catVinculadasPadre.Count > 0)
                                        {
                                            onJoin1 = aliasInvenElemAnterior + ".InventarioElementoID=" + aliasInvEleVinc + ".InventarioElementoID " + " AND "+ aliasInvEleVinc + ".InventarioCategoriaVinculacionID="+ catVinculadasPadre[0].InventarioCategoriaVinculacionID;
                                            onJoin2 = aliasInvEleVinc + ".InventarioElementoPadreID = " + aliasInvEle + ".InventarioElementoID ";
                                        }


                                        JOINs += " LEFT JOIN InventarioElementosVinculaciones " + aliasInvEleVinc + " ON " + onJoin1 +
                                                " LEFT JOIN InventarioElementos " + aliasInvEle + " ON " + onJoin2;

                                        ItemPath last = campoVinculado.Ruta.path.Last();
                                        if (last.id == i.id && last.tipo == i.tipo && coreAtributoConfiguracion != null)
                                        {
                                            string sCol = "JSON_VALUE(" + aliasInvEle + ".jsonAtributosDinamicos, '$.\"" + coreAtributoConfiguracion.CoreAtributoConfiguracionID + "\".\"Valor\"') as '" + campoVinculado.IdRandom + "'";
                                            if (!nombreColumnasAtributos.Contains(sCol) && !nombreColumnas.Contains(sCol))
                                            {
                                                nombreColumnasAtributos.Add(sCol);
                                            }
                                        }

                                        aliasInvenElemAnterior = aliasInvEle;
                                        idCategoriaAnterior = i.id;
                                    }
                                    else if (i.tipo == ESTATICO)
                                    {
                                        idCategoriaAnterior = null;
                                        tablas.Add(tablaModeloDato);
                                        ColumnasModeloDatos columna = cColumnasModeloDatos.GetItem(campoVinculado.CampoVinculadoID);
                                        if (campoVinculado.Ruta.path.Count > 0)
                                        {

                                            JOINs = buildQueryOfCampoVinculado(idTablaAnterior, i.id, tablas, aliasTabla, incrementalAlias,
                                                out outIncrementalAlias, aliasTablaAnterior, out aliasTablaAcutal, JOINs, columna, campoVinculado, nombreColumnasAtributos,
                                                out outNombreColumnasAtributos, nombreColumnas);
                                            idTablaAnterior = i.id;
                                            incrementalAlias = outIncrementalAlias;
                                            nombreColumnasAtributos = outNombreColumnasAtributos;

                                        }
                                        else
                                        {
                                            JOINs = buildQueryOfCampoVinculado(idTablaAnterior, plantilla.TablaModeloDatosID, tablas, aliasTabla, incrementalAlias,
                                                out outIncrementalAlias, aliasTablaAnterior, out aliasTablaAcutal, JOINs, columna, campoVinculado, nombreColumnasAtributos,
                                                    out outNombreColumnasAtributos, nombreColumnas);
                                            idTablaAnterior = plantilla.TablaModeloDatosID;
                                            incrementalAlias = outIncrementalAlias;
                                            nombreColumnasAtributos = outNombreColumnasAtributos;
                                        }
                                        aliasTablaAnterior = aliasTablaAcutal;
                                    }
                                }
                            }
                            else
                            {
                                if (campoVinculado.EsDinamico)
                                {
                                    CoreAtributosConfiguraciones coreAtributoConfiguracion = cCoreAtributosConfiguraciones.GetItem(campoVinculado.CampoVinculadoID);

                                    string sCol = "JSON_VALUE(" + aliasTabla + ".jsonAtributosDinamicos, '$.\"" + coreAtributoConfiguracion.CoreAtributoConfiguracionID + "\".\"Valor\"') as '" + campoVinculado.IdRandom + "'";
                                    if (!nombreColumnasAtributos.Contains(sCol) && !nombreColumnas.Contains(sCol))
                                    {
                                        nombreColumnasAtributos.Add(sCol);
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

                #region Filtro por categoría
                
                if (TipoFiltroID.HasValue)
                {
                    where = aliasTabla + "." + nombreColumnaCategoriaID + " = " + TipoFiltroID.Value;
                }
                else if (TipoFiltroDinamicoID.HasValue)
                {
                    where = aliasTabla + "." + nombreColumnaCategoriaID + " = " + TipoFiltroDinamicoID.Value;
                }

                #endregion

                if (string.IsNullOrEmpty(where))
                {
                    where = "";
                }

                nombreColumnas.ForEach(nmCol =>
                {
                    sNombreColumnas += (string.IsNullOrEmpty(sNombreColumnas) ? "" : ", ") + nmCol;
                });
                nombreColumnasAtributos.ForEach(nmCol =>
                {
                    sNombreColumnas += (string.IsNullOrEmpty(sNombreColumnas) ? "" : ", ") + nmCol;
                });

                #region Filtro
                string whereFiltro = string.Empty;
                if (!string.IsNullOrEmpty(plantilla.Filtro))
                {
                    string listaFiltros = "";
                    JObject jFiltro = JObject.Parse(plantilla.Filtro);
                    if (jFiltro.ContainsKey("listaFiltros"))
                    {
                        listaFiltros = jFiltro["listaFiltros"].ToString();
                    }

                    List<FiltroInventarioElementos> filtros = cFiltroInventarioElementosController.DeserializacionFiltros(listaFiltros);

                    if (filtros != null)
                    {
                        foreach (FiltroInventarioElementos filtro in filtros)
                        {
                            whereFiltro += ((string.IsNullOrEmpty(whereFiltro)) ? "" : " AND ");

                            string campoFiltro;
                            string sValueFiltro = null;
                            if (filtro.TipoCampo == "Atributo")
                            {
                                campoFiltro = "JSON_VALUE(i.jsonAtributosDinamicos, '$.\"" + filtro.Campo + "\".\"Valor\"')";
                                sValueFiltro = "'" + filtro.Value + "'";
                            }
                            else
                            {
                                campoFiltro = aliasTabla + "." + filtro.Campo;
                                sValueFiltro = filtro.Value;
                            }

                            if (filtro.TypeData == Comun.TIPODATO_CODIGO_LISTA || filtro.TypeData == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                            {
                                whereFiltro += campoFiltro + " IN ( " + sValueFiltro + ")";
                            }
                            else if (filtro.TypeData == Comun.TIPODATO_CODIGO_TEXTO || filtro.TypeData == Comun.TIPODATO_CODIGO_TEXTAREA)
                            {
                                whereFiltro += campoFiltro + " LIKE '%" + sValueFiltro + "%'";
                            }
                            else if (filtro.TypeData == Comun.TIPODATO_CODIGO_FECHA)
                            {
                                string[] elements = { "CAST(", campoFiltro, " AS DATE) ", filtro.Operador, " CAST(CONVERT(DATETIME, '", sValueFiltro, "') AS DATE)" };
                                whereFiltro += string.Concat(elements);
                            }
                            else if (filtro.TypeData == Comun.TIPODATO_CODIGO_BOOLEAN)
                            {
                                if (filtro.TipoCampo == "Atributo")
                                {
                                    whereFiltro += campoFiltro + " = " + (bool.Parse(sValueFiltro) ? "True" : "False");
                                }
                                else
                                {
                                    whereFiltro += campoFiltro + " = " + (bool.Parse(sValueFiltro) ? "1" : "0");
                                }
                            }
                            else if (filtro.TypeData == Comun.TIPODATO_CODIGO_NUMERICO || filtro.TypeData == Comun.TIPODATO_CODIGO_NUMERICO_ENTERO || filtro.TypeData == Comun.TIPODATO_CODIGO_NUMERICO_FLOTANTE)
                            {
                                whereFiltro += campoFiltro + " = " + sValueFiltro;
                            }
                        }

                        whereFiltro = ((string.IsNullOrEmpty(where)) ? "" : " AND ") + whereFiltro;
                        where += " " + whereFiltro;
                    }
                }
                #endregion

                #region Formar Query
                if (!string.IsNullOrEmpty(where))
                {
                    where = "WHERE " + where;
                }

                sNombreColumnas = (string.IsNullOrEmpty(sNombreColumnas) ? "*" : sNombreColumnas);

                if (limiteRegistros.HasValue)
                {
                    if (string.IsNullOrEmpty(nombreColumnaCategoriaID))
                    {
                        query = select + " " + top + " " + limiteRegistros.ToString() + " " + sNombreColumnas + " " + from + " " + nombreTabla + " " + aliasTabla + " " + JOINs + " " + tablaInventarioElementos + " " + where;
                    }
                    else
                    {
                        query = "SELECT * FROM(" +
                            select + " " + sNombreColumnas + ", ROW_NUMBER() OVER(PARTITION BY " + aliasTabla + "." + nombreColumnaCategoriaID + " ORDER BY " + aliasTabla + "." + nombreColumnaCategoriaID + ") rn FROM " + nombreTabla + " " + aliasTabla + " " + JOINs + " " + tablaInventarioElementos + " " + where +
                            ") x WHERE   x.rn" + " <= " + limiteRegistros.ToString();
                    }
                }
                else
                {
                    query = select + " " + sNombreColumnas + " " + from + " " + nombreTabla + " " + aliasTabla + " " + JOINs + " " + tablaInventarioElementos + " " + where;
                }
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return query;
        }
        #endregion

        #region BuildQueryOfCampoVinculado
        private static string buildQueryOfCampoVinculado(long idTablaAnterior, long idTabla, List<TablasModeloDatos> tablas, string aliasTabla, 
            long incrementalAlias, out long outIncrementalAlias, string aliasTablaAnterior, out string outAliasTablaAcutal, string JOINs, ColumnasModeloDatos columna,
            CampoVinculado campoVinculado, List<string> nombreColumnasAtributos, out List<string> outNombreColumnasAtributos, List<string> nombreColumnas) {


            TablasModeloDatosController cTablasModeloDatos = new TablasModeloDatosController();
            ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();

            TablasModeloDatos tabla = tablas.Find(tb => tb.TablaModeloDatosID == idTabla);
            string aliasTablaActual = aliasTabla + idTabla + incrementalAlias++;
            outAliasTablaAcutal = aliasTablaActual;
            string nombreColumnaFk;
            string nombreColumnaPk;
            cColumnasModeloDatos.GetNombreColumnasRelacionadas(out nombreColumnaFk, out nombreColumnaPk, idTablaAnterior, idTabla);

            if (string.IsNullOrEmpty(nombreColumnaFk))
            {
                nombreColumnaFk = tabla.Indice;
            }

            JOINs += " LEFT JOIN " + tabla.NombreTabla + " " + aliasTablaActual + " ON " + aliasTablaAnterior + "." + nombreColumnaFk + " = " + aliasTablaActual + "." + tabla.Indice;

            if (columna.TablaModeloDatosID == idTabla)
            {
                string nombreColumnaVinculada = aliasTablaActual + "." + columna.NombreColumna;
                string sCol = nombreColumnaVinculada + " AS '" + campoVinculado.IdRandom + "'";
                if (!nombreColumnas.Contains(sCol) && !nombreColumnasAtributos.Contains(sCol))
                {
                    nombreColumnasAtributos.Add(sCol);
                }
            }

            idTablaAnterior = idTabla;
            aliasTablaAnterior = aliasTablaActual;

            outIncrementalAlias = incrementalAlias;
            outNombreColumnasAtributos = nombreColumnasAtributos;

            return JOINs;
        }
        #endregion

        #region GenerateAndSaveFile
        public static string GenerateAndSaveFile(List<string> cabecera, IList lista, string rutaFichero, string nombreFichero, string TipoFichero, Dictionary<string, string> formatosFechas)
        {
            string archivo = "";
            string path = "";
            if (lista != null)
            {
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    path = rutaFichero + "\\" + nombreFichero;

                    #region TipoFichero
                    if (TipoFichero.Equals(Comun.TIPO_FICHERO_CSV))
                    {
                        path += Comun.DOCUMENTO_EXTENSION_EXCEL_CSV;
                        archivo = nombreFichero + Comun.DOCUMENTO_EXTENSION_EXCEL_CSV;
                    }
                    else
                    {
                        path += Comun.DOCUMENTO_EXTENSION_EXCEL_XLSX;
                        archivo = nombreFichero + Comun.DOCUMENTO_EXTENSION_EXCEL_XLSX;
                    }
                    #endregion

                    #region Delete file if exists
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    #endregion

                    FileInfo file = new FileInfo(path);

                    string ServiceName = DirectoryMapping.GetExportDirectory();

                    ExcelPackage pagkage = new ExcelPackage(file);
                    pagkage.Workbook.Properties.Author = ServiceName;
                    pagkage.Workbook.Properties.Title = nombreFichero;
                    pagkage.Workbook.Properties.Created = DateTime.Now;
                    

                    ExcelWorksheet ws = pagkage.Workbook.Worksheets.Add(nombreFichero);

                    int i = 1;
                    cabecera.ForEach(col =>
                    {
                        ws.Cells[1, i++].Value = col;
                    });

                    if (lista != null && lista.Count > 0)
                    {
                        ExcelRangeBase range = ws.Cells["A2"].LoadFromCollection((IEnumerable<JsonObject>)lista);
                        
                        if (lista.Count > 0)
                        {
                            range.AutoFitColumns();
                        }

                        #region Asignación de formato a las celdas
                        ExcelCellAddress start = range.Start;
                        ExcelCellAddress end = range.End;
                        for (int row = start.Row; row <= end.Row; row++)
                        {
                            for (int col = start.Column; col <= end.Column; col++)
                            {
                                ExcelRange celd = ws.Cells[row, col];
                                string valueOfCeld = celd.Text;
                                //IFormatProvider culture = new CultureInfo("en-US", true);
                                //string keyFormato = ((IEnumerable<JsonObject>)lista).ElementAt(row - 2).ElementAt(col - 1).Key;

                                //DateTime date;
                                float ndecimal;
                                long nEntero;

                                //if (formatosFechas.ContainsKey(keyFormato))
                                //{
                                //    string dateFormat = formatosFechas[keyFormato];

                                //    if (DateTime.TryParseExact(valueOfCeld, dateFormat, culture, DateTimeStyles.None, out date))
                                //    {
                                //        celd.Style.Numberformat.Format = dateFormat;

                                //        celd.Value = date;
                                //    }
                                //}
                                //else 
                                if (long.TryParse(valueOfCeld, out nEntero))
                                {
                                    celd.Style.Numberformat.Format = "0";
                                    celd.Value = nEntero;
                                }
                                else if (float.TryParse(valueOfCeld, out ndecimal))
                                {
                                    celd.Style.Numberformat.Format = "0.00";
                                    celd.Value = ndecimal;
                                }
                            }
                        }
                        #endregion

                    }

                    if (TipoFichero.Equals(Comun.TIPO_FICHERO_CSV))
                    {
                        ExcelOutputTextFormat excOutTextFormat = new ExcelOutputTextFormat()
                        {
                            Delimiter = ';',
                            Encoding = System.Text.Encoding.UTF8
                        };
                        string content;
                        if (ws.Cells.Worksheet.Dimension != null)
                        {
                            string localAddres = ws.Cells.Worksheet.Dimension.LocalAddress;
                            content = ws.Cells[localAddres].ToText(excOutTextFormat);
                        }
                        else
                        {
                            content = ws.Cells.ToText(excOutTextFormat);
                        }

                        File.WriteAllText(path, content);
                    }
                    else
                    {
                        pagkage.Save();
                    }

                    log.Info("File is saved: " + path);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    log.Info("File not saved: " + path);
                }
            }

            return archivo;
        }
        #endregion

        #region GenerateHistory
        public static void GenerateHistory(long CoreExportacionDatoPlantillaID, string archivo, DateTime fechaEjecucion)
        {
            CoreExportacionDatosPlantillasHistoricosController cCoreExportacionDatosPlantillasHistoricos = new CoreExportacionDatosPlantillasHistoricosController();

            try
            {
                long version = cCoreExportacionDatosPlantillasHistoricos.GetNewNumberVersion(CoreExportacionDatoPlantillaID);
                CoreExportacionDatosPlantillasHistoricos newHistorico = new CoreExportacionDatosPlantillasHistoricos() { 
                    Activo=true,
                    CoreExportacionDatosPlantillaID = CoreExportacionDatoPlantillaID,
                    FechaEjecucion = fechaEjecucion,
                    Version = version,
                    Archivo = archivo
                };

                cCoreExportacionDatosPlantillasHistoricos.AddItem(newHistorico);


            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }
        #endregion

        #region ordenarCeldasConcatenadas
        public static List<long> ordenarCeldasConcatenadas(List<CoreExportacionDatosPlantillasCeldas> celdas, long? celdaPadreID, List<long> celdasIDsOrdenados)
        {
            CoreExportacionDatosPlantillasCeldas celdaHija = celdas.Find(c => c.CeldaPadreID == celdaPadreID);

            if (celdaPadreID.HasValue)
            {
                celdasIDsOrdenados.Add(celdaPadreID.Value);
            }

            if (celdaHija != null)
            {
                celdasIDsOrdenados = ordenarCeldasConcatenadas(celdas, celdaHija.CoreExportacionDatosPlantillasCeldasID, celdasIDsOrdenados);
            }

            return celdasIDsOrdenados;
        }
        #endregion

        #region GetStrFecha
        public static string GetStrFecha()
        {
            DateTime fecha = DateTime.Now;
            string str = fecha.ToString("yyyyMMdd");

            return str;
        }
        #endregion

        #region ParseDate
        /**
         https://docs.microsoft.com/es-es/dotnet/standard/base-types/custom-date-and-time-format-strings

            "d" 	El día del mes, de 1 a 31.
            "dd" 	El día del mes, de 01 a 31.
            "ddd" 	El nombre abreviado del día de la semana.
            "dddd" 	El nombre completo del día de la semana.
            "f" 	Las décimas de segundo de un valor de fecha y hora.
            "ff" 	Las centésimas de segundo de un valor de fecha y hora.
            "fff" 	Los milisegundos de un valor de fecha y hora.
            "ffff" 	Las diezmilésimas de segundo de un valor de fecha y hora.
            "fffff" 	Las cienmilésimas de segundo de un valor de fecha y hora.
            "ffffff" 	Las millonésimas de segundo de un valor de fecha y hora.
            "fffffff" 	Las diezmillonésimas de segundo de un valor de fecha y hora.
            "F" 	Si es distinto de cero, las décimas de segundo de un valor de fecha y hora.
            "FF" 	Si es distinto de cero, las centésimas de segundo de un valor de fecha y hora.
            "FFF" 	Si es distinto de cero, los milisegundos de un valor de fecha y hora.
            "FFFF" 	Si es distinto de cero, las diezmilésimas de segundo de un valor de fecha y hora.
            "FFFFF" 	Si es distinto de cero, las cienmilésimas de segundo de un valor de fecha y hora.
            "FFFFFF" 	Si es distinto de cero, las millonésimas de segundo de un valor de fecha y hora.
            "FFFFFFF" 	Si es distinto de cero, las diezmillonésimas de segundo de un valor de fecha y hora.
            "g", "gg" 	El período o la era.
            "h" 	La hora, usando un reloj de 12 horas de 1 a 12.
            "hh" 	La hora, usando un reloj de 12 horas de 01 a 12.
            "H" 	La hora, usando un reloj de 24 horas de 0 a 23.
            "HH" 	La hora, usando un reloj de 24 horas de 00 a 23.
            "K" 	Información de la zona horaria.
            "m" 	Minutos, de 0 a 59.
            "mm" 	El minuto, de 00 a 59.
            "M" 	El mes, de 1 a 12.
            "MM" 	El mes, de 01 a 12.
            "MMM" 	El nombre abreviado del mes.
            "MMMM" 	El nombre completo del mes.
            "s" 	El segundo, de 0 a 59.
            "ss" 	El segundo, de 00 a 59.
            "t" 	El primer carácter del designador AM/PM.
            "tt" 	El designador AM/PM.
            "y" 	El año, de 0 a 99.
            "yy" 	El año, de 00 a 99.
            "yyy" 	El año, con un mínimo de tres dígitos.
            "yyyy" 	El año como un número de cuatro dígitos.
            "yyyyy" 	El año como un número de cinco dígitos.
            "z" 	Desfase de horas con respecto a la hora UTC, sin ceros iniciales.
            "zz" 	Desfase de horas con respecto a la hora UTC, con un cero inicial para un valor de un solo dígito.
            "zzz" 	Desfase de horas y minutos con respecto a la hora UTC.
            ":" 	El separador de hora.
            "/" 	El separador de fecha.
            'cadena' 	Delimitador de cadena literal.
            % 	Define el siguiente carácter como un especificador de formato personalizado.
            \ 	El carácter de escape.
         */
        private static string ParseDate(string sDate, string formato)
        {
            string sFecha;

            if (!string.IsNullOrEmpty(sDate))
            {
                try
                {
                    if (string.IsNullOrEmpty(formato))
                    {
                        formato = Comun.FORMATO_FECHA;
                    }

                    DateTime fecha;// = DateTime.Parse(sDate);
                                   //DateTime.ParseExact(sDate, "M/d/yyyy h:mm:ss ", new CultureInfo("en-US"));
                    DateTime.TryParseExact(sDate, dateTimePattern, cultureInfo, DateTimeStyles.None, out fecha);
                    if (fecha == null || fecha == DateTime.MinValue)
                    {
                        fecha = DateTime.Parse(sDate);
                    }

                    sFecha = fecha.ToString(formato);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    sFecha = null;
                }
            }
            else
            {
                sFecha = sDate;
            }

            return sFecha;
        }
        #endregion

        #region CrearRutaSiNoExiste
        public static void CrearRutaSiNoExiste(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            // Try to create the directory.
            DirectoryInfo di = Directory.CreateDirectory(path);
        }
        #endregion

        #region Campo vinculado
        public class CampoVinculado
        {
            public long CampoVinculadoID { get; set; }
            public bool EsDinamico { get; set; }
            public Path Ruta { get; set; }
            public string IdRandom { get; set; }
            public string TipoDato { get; set; }

            public CampoVinculado(long CampoVinculadoID, bool EsDinamico, Path ruta, string TipoDato)
            {
                this.CampoVinculadoID = CampoVinculadoID;
                this.EsDinamico = EsDinamico;
                this.Ruta = ruta;
                this.IdRandom = DateTime.Now.Ticks.ToString();
                this.TipoDato = TipoDato;
            }
        }

        public class Path
        {
            public List<ItemPath> path  { get; set; }
        }

        public class ItemPath
        {
            public long id { get; set; }
            public string uID { get; set; }
            public string tipo { get; set; }
        }
        #endregion

        #region ordenarListaReglasDeTransformacion
        public static List<Vw_CoreExportacionDatosPlantillasReglasCeldas> ordenarListaReglasDeTransformacion(List<Vw_CoreExportacionDatosPlantillasReglasCeldas> reglasCelda)
        {
            reglasCelda.Sort((x, y) => ComparerCoreExportacionDatosPlantillasReglasCeldas.CompareVw(x, y));

            return reglasCelda;
        }
        #endregion

    }



}
