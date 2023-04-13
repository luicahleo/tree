using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Tree.Linq;
using Tree.Linq.GenericExtensions;

namespace TreeCore.Clases
{
    public class LinqEngine
    {

        /// <summary>
        /// Método para filtrar y ordenar listas genéricas pasandole el filtro extnet
        /// </summary>
        /// <typeparam name="T">Tipo de datos de la lista</typeparam>
        /// <param name="dataList">La lista a filtrar y ordenar</param>
        /// <param name="ExtNetfilter">el filtro del ExtNet</param>
        /// <param name="filtro">otro Filtro</param>
        /// <param name="order">El Campo por el cula se realiza la Ordenación</param>
        /// <param name="dir">Direccions de la Ordenación</param>
        /// <param name="start">primer regístro </param>
        /// <param name="limit">limite de los registros</param>
        /// <param name="count">Cantidad total de los elementos de la lista</param>
        /// <returns></returns>
        public static List<T> PagingItemsListWithExtNetFilter<T>(List<T> dataList, string ExtNetfilter, string filtro, string order, string dir, int start, int limit, ref int count) where T : class
        {
            string linqfilter = null;

            IQueryable<T> query = default(IQueryable<T>);
            count = 0;
            if (dataList == null)
                return null;
            if (dataList.Count == 0)
                return dataList;
            query = dataList.AsQueryable();
            linqfilter = ParseExtNetFilter2LinqFilter(ExtNetfilter);
            if (!string.IsNullOrEmpty(filtro))
            {
                if (string.IsNullOrEmpty(linqfilter))
                {
                    linqfilter = filtro;
                }
                else
                {
                    linqfilter = "(" + linqfilter + ") && (" + filtro + ")";
                }
            }
            // a qui llamamos el método genérico pasandole el extNelFilter como filtro
            return ClassPagingFilterList.PagingItemsListWithFilter<T>(dataList, linqfilter, order, dir, start, limit, ref count);

        }

        /// <summary>
        /// Método que usa la libreria de ExtNet para realizar los filtros sobre las tablas
        /// </summary>
        /// <param name="ExtNetfilter"> el filtor de ExtNet</param>
        /// <returns></returns>
        /// 

        public static string ParseExtNetFilter2LinqFilter(string ExtNetfilter)
        {
            FilterConditions filterCondition = default(FilterConditions);
            string linqfilter = null;
            // Si el ExtNetfilter viene Vacio entonces no filtramos
            if (string.IsNullOrEmpty(ExtNetfilter))
            {
                return null;
            }
            else
            {
                Comparison compardor = default(Comparison);
                FilterType filtertype = default(FilterType);
                string atomo = "";
                string conjuncion = "";

                filterCondition = new FilterConditions(ExtNetfilter);
                linqfilter = "";

                foreach (FilterCondition condition in filterCondition.Conditions)
                {
                    compardor = condition.Comparison;
                    filtertype = condition.Type;
                    atomo = "";
                    if (filtertype == Ext.Net.FilterType.List)
                    {
                        List<String> collection = condition.List;
                        IEnumerator<string> iter = collection.GetEnumerator();
                        try
                        {
                            if (collection.Count > 0)
                            {
                                string disyuncion = "";
                                atomo = "(";
                                while (iter.MoveNext())
                                {
                                    atomo += disyuncion + "(" + condition.Field + "!= null && ";
                                    atomo += condition.Field + " = \"" + iter.Current + "\"" + ")";
                                    //atomo += "&&" + condition.Name + "!= null)";
                                    disyuncion = " || ";
                                }
                                atomo += ")";
                            }
                        }
                        catch (Exception)
                        {
                            atomo = "";
                        }
                    }
                    else
                    {
                        try
                        {
                            atomo = condition.Field;

                            switch (compardor)
                            {
                                case Comparison.Lt:
                                    atomo += " < {0}";
                                    break;
                                case Comparison.Gt:
                                    atomo += " > {0}";
                                    break;
                                case Comparison.Eq:
                                case Comparison.Like:

                                    if (filtertype == Ext.Net.FilterType.String)
                                    {
                                        atomo += " != null " + "&& " + condition.Field + ".toString().ToLower().Contains(\"" + condition.Property.Value + "\".toString().ToLower()) ";
                                    }
                                    else
                                    {
                                        atomo += " = {0}";
                                    }

                                    break;
                                
                            }
                            switch (filtertype)
                            {
                                case Ext.Net.FilterType.Boolean:
                                    if (condition.Equals(true))
                                    {
                                        atomo = string.Format(atomo, "true");
                                    }
                                    else
                                    {
                                        atomo = string.Format(atomo, "false");
                                    }

                                    break;
                                case Ext.Net.FilterType.Date:
                                    try
                                    {
                                        string[] aux;
                                        aux = condition.Property.Value.ToString().Split('-');
                                        if (aux.Length < 2)
                                        {
                                            aux = condition.Property.Value.ToString().Split('/');
                                        }
                                        DateTime fecha;
                                        fecha = new DateTime(Convert.ToInt32(aux[2]), Convert.ToInt32(aux[0]), Convert.ToInt32(aux[1]));

                                        long ticks = fecha.Ticks;
                                        //long ticks = condition.ValueAsDate.Ticks;
                                        atomo = string.Format(atomo, "DateTime(" + ticks + ")");
                                    }
                                    catch (Exception ex)
                                    {
                                        atomo = "";
                                    }

                                    break;

                                case Ext.Net.FilterType.String:
                                    atomo = string.Format(atomo, "\"" + condition.Field + "\"");
                                    break;
                                case Ext.Net.FilterType.Number:
                                    atomo = string.Format(atomo, condition.Field);
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            atomo = "";

                        }
                    }
                    if (!string.IsNullOrEmpty(atomo))
                    {
                        linqfilter += conjuncion + atomo;
                        conjuncion = " && ";

                    }
                }
                if (linqfilter.Length > 0)
                {
                    return linqfilter;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<object> filtro(List<object> data, string filterConditions)
        {
            try 
            { 
                FilterConditions fc = new FilterConditions(filterConditions);

                foreach (FilterCondition condition in fc.Conditions)
                {
                    Comparison comparison = condition.Comparison;
                    string field = condition.Field;
                    FilterType type = condition.Type;

                    object value;
                    switch (condition.Type)
                    {
                        case FilterType.Boolean:
                            value = condition.Value<bool>();
                            break;
                        case FilterType.Date:
                            value = condition.Value<DateTime>();
                            break;
                        case FilterType.List:
                            value = condition.List;
                            break;
                        case FilterType.Number:
                            if (data.Count > 0 && data[0].GetType().GetProperty(field).PropertyType == typeof(int))
                            {
                                value = condition.Value<int>();
                            }
                            else
                            {
                                value = condition.Value<double>();
                            }

                            break;
                        case FilterType.String:
                            value = condition.Value<string>();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    data.RemoveAll(
                        item =>
                        {
                            object oValue = item.GetType().GetProperty(field).GetValue(item, null);
                            IComparable cItem = oValue as IComparable;

                            switch (comparison)
                            {
                                case Comparison.Eq:
                                case Comparison.Like:
                                case Comparison.In:
                                    switch (type)
                                    {
                                        case FilterType.List:
                                            return !(value as List<string>).Contains(oValue.ToString());
                                        case FilterType.String:
                                            return !oValue.ToString().StartsWith(value.ToString());
                                        default:
                                            return !cItem.Equals(value);
                                    }

                                case Comparison.Gt:
                                    return cItem.CompareTo(value) < 1;
                                case Comparison.Lt:
                                    return cItem.CompareTo(value) > -1;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                    );
                }
            }
            catch (Exception) 
            {
                data = null;
            }
            return data;
        }

        //Filtrado
        public static List<JsonObject> filtroJson(List<JsonObject> data, string filterConditions)
        {
            try
            {
                if (filterConditions != null)
                {
                    FilterConditions fc = new FilterConditions(filterConditions);

                    foreach (FilterCondition condition in fc.Conditions)
                    {
                        Comparison comparison = condition.Comparison;
                        string field = condition.Field;
                        FilterType type = condition.Type;

                        object value;
                        switch (condition.Type)
                        {
                            case FilterType.Boolean:
                                value = condition.Value<bool>();
                                break;
                            case FilterType.Date:
                                //value = condition.Value<DateTime>();
                                //value = Convert.ToDateTime(condition.Property.Value);
                                string timeString = condition.Property.Value.ToString();
                                IFormatProvider culture = new CultureInfo("en-US", true);
                                value = DateTime.ParseExact(timeString, "dd/MM/yyyy", culture).ToShortDateString();
                                //value = Convert.ToDateTime(data[0][field]);

                                break;
                            case FilterType.List:
                                value = condition.List;
                                break;
                            case FilterType.Number:
                                value = Convert.ToDouble(data[0][field]);
                                /*if (data.Count > 0 && data[0][field].PropertyType == typeof(int))
                                {
                                    value = condition.Value<int>();
                                }
                                else
                                {
                                    value = condition.Value<double>();
                                }*/

                                break;
                            case FilterType.String:
                                value = condition.Value<string>();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        data.RemoveAll(
                            item =>
                            {
                                if (item.ContainsKey(field))
                                {
                                    object oValue = item[field];

                                    if (field.Contains("Fecha"))
                                    {
                                        oValue = Convert.ToDateTime(oValue).ToShortDateString();
                                    }

                                    IComparable cItem = oValue as IComparable;

                                    switch (comparison)
                                    {
                                        case Comparison.Eq:
                                        case Comparison.Like:
                                        case Comparison.In:
                                            switch (type)
                                            {
                                                case FilterType.List:
                                                    return !(value as List<string>).Contains(oValue.ToString());
                                                case FilterType.String:
                                                    return !oValue.ToString().ToLower().Contains(value.ToString().ToLower());
                                                default:
                                                    return !cItem.Equals(value);
                                            }

                                        case Comparison.Gt:
                                            if (cItem is DateTime) {
                                                return ((DateTime)cItem).CompareTo((DateTime)value) < 1;
                                            }
                                            else if (cItem != null)
                                            {
                                                return cItem.CompareTo(value) < 1;
                                            }
                                            else
                                            {
                                                return true;
                                            }
                                        case Comparison.Lt:
                                            if (cItem is DateTime)
                                            {
                                                return ((DateTime)cItem).CompareTo((DateTime)value) > -1;
                                            }
                                            else if (cItem != null)
                                            {
                                                return cItem.CompareTo(value) > -1;
                                            }
                                            else
                                            {
                                                return true;
                                            }
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                data = null;
            }
            return data;
        }

        //Ordenación
        public static List<JsonObject> SortJson(List<JsonObject> listaJson, DataSorter[] sorters)
        {
            try
            {
                if (sorters.Length > 0)
                {
                    listaJson.Sort((JsonObject x, JsonObject y) =>
                    {
                        object a;
                        object b;

                        int direction = sorters[0].Direction == Ext.Net.SortDirection.DESC ? -1 : 1;

                        a = (x.ContainsKey(sorters[0].Property)) ? x[sorters[0].Property] : null;
                        b = (y.ContainsKey(sorters[0].Property)) ? y[sorters[0].Property] : null;

                        try
                        {
                            return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                        }
                        catch(Exception ex)
                        {
                            return -1 * direction;
                        }
                    });
                }
            }
            catch(Exception ex)
            {
                listaJson = null;
            }

            return listaJson;
        }

    }
}