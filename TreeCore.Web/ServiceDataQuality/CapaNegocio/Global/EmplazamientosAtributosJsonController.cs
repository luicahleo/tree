using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using System.Configuration;
using Tree.Linq;
using TreeCore.Data;
using System.Data.Linq;
using System.Reflection;
using log4net;
using System.Data.SqlClient;
using Tree.Linq.Dynamic;
using System.Text.RegularExpressions;

namespace CapaNegocio
{

    public class EmplazamientosAtributosJsonController
    {
        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected DataTable EjecutarQuery(string queryString)
        {
            ILog Log = LogManager.GetLogger("");
            //Log.Info("EXECUTED_QUERY: " + queryString);
            #region CADENA CONEXIÓN
#if SERVICESETTINGS
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string connectionString = TreeAPI.Properties.Settings.Default.Conexion;
#else
            string connectionString = TreeCore.Properties.Settings.Default.Conexion;
#endif
            #endregion

            DataTable result = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                log.Error(ex.Message);
            }

            return result;
        }

        //protected List<List<InventarioElementosAtributosJson>> DeserializacionDataTable(DataTable dataTable)
        //{
        //    List<List<InventarioElementosAtributosJson>> listaDatos;
        //    List<InventarioElementosAtributosJson> listaAtributos;
        //    try
        //    {
        //        listaDatos = new List<List<InventarioElementosAtributosJson>>();
        //        foreach (System.Data.DataRow fila in dataTable.Rows)
        //        {
        //            listaDatos.Add(JsonSerializer.Deserialize<List<InventarioElementosAtributosJson>>(fila.ItemArray[0].ToString()));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        listaDatos = null;
        //    }
        //    return listaDatos;
        //}

        public List<EmplazamientosAtributosJson> Deserializacion(string datos)
        {
            List<EmplazamientosAtributosJson> listaAtributos;
            try
            {
                if (datos.StartsWith("["))
                {
                    listaAtributos = JsonConvert.DeserializeObject<List<EmplazamientosAtributosJson>>(datos);
                }
                else
                {
                    datos = Regex.Replace(datos, "\"[0-9]*\":", "");
                    datos = "[" +datos.Remove(0,1).Remove(datos.Length-2, 1) + "]";
                    listaAtributos = JsonConvert.DeserializeObject<List<EmplazamientosAtributosJson>>(datos);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaAtributos = null;
            }
            return listaAtributos;
        }

        public string Serializacion(List<EmplazamientosAtributosJson> listaDatos)
        {
            string oDato;
            try
            {
                oDato = "{";
                foreach (var oAtr in listaDatos)
                {
                    if (oDato != "{")
                    {
                        oDato += ",";
                    }
                    oDato += "\"" + oAtr.AtributoID + "\":";
                    oDato += JsonConvert.SerializeObject(oAtr);
                }
                oDato += "}";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }
    }

    public class EmplazamientosAtributosJsonControllerComparer : IEqualityComparer<EmplazamientosAtributosJson>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(EmplazamientosAtributosJson x, EmplazamientosAtributosJson y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.AtributoID == y.AtributoID;
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(EmplazamientosAtributosJson product)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(product, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashAtributoID = product.AtributoID.GetHashCode();

            //Calculate the hash code for the product.
            return hashAtributoID;
        }
    }

}

namespace TreeCore.Data
{

    public class EmplazamientosAtributosJson
    {
        public long AtributoID { get; set; }
        public string NombreAtributo { get; set; }
        public string Valor { get; set; }
        public string TextLista { get; set; }
        public string TipoDato { get; set; }
    }

}