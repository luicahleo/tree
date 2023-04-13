using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
using Newtonsoft.Json;
using System.Data.Linq.Mapping;
using System.Web.UI.WebControls;
using Ext.Net;
using System.Reflection;
using System.Data.SqlClient;
using System.Globalization;






namespace CapaNegocio
{
    public class MigradorController : GeneralBaseController<MigradorTablas, TreeCoreContext>


    {
        public MigradorController()
            : base()
        { }

        public string[] GetTabla(string Cambio, string dato)
        {
            string[] separador = { "[", "]" };
            string[] datos = Cambio.Split(separador, StringSplitOptions.None);

            return datos;
        }
        public string GetCampo(string Campo)
        {
            string[] separador = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string[] datos = Campo.Split(separador, StringSplitOptions.None);

            return datos[0];
        }



        public List<MigradorTablas> GetItemTablas(bool Configuracion, bool Datos, long proyectoTipoID, bool Activo)
        {//Dependiendo de los parámetros se extrae la información de la tabla MigradorTablas
            List<MigradorTablas> migradorTablas = new List<MigradorTablas>();
            migradorTablas = (from c in Context.MigradorTablas
                              where c.Configuracion == Configuracion && c.Datos == Datos &&
c.ProyectoTipoID == proyectoTipoID && c.Activo == true
                              orderby c.Orden
                              select c).ToList();

            return migradorTablas;
        }

        public string JsonTabla(string Tabla)
        {
            string EstadoJson = "";
            List<Object> lista = new List<object>();

            string sNombreTabla = "dbo." + Tabla;
            string sNombreTablaMinuscula = null;
            string sNombreTablaMayuscula = null;
            string sQuery = "Select * from " + sNombreTabla;
            int i = 1;
            bool bTabla = true;
            SqlConnection connection = null;


#if SERVICESETTINGS
            string conexion = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string conexion = TreeAPI.Properties.Settings.Default.Conexion;
#else
            string conexion = TreeCore.Properties.Settings.Default.Conexion;
#endif



            try
            {
                if (!sNombreTabla.Contains("vw_") && !sNombreTabla.Contains("Vw_"))
                {
                    sNombreTablaMayuscula = sNombreTabla;//sNombreTabla.Substring(0, 3) + "." + "Vw_" + sNombreTabla.Substring(4, sNombreTabla.Length - 4);
                    sNombreTablaMinuscula = sNombreTabla;//sNombreTabla.Substring(0, 3) + "." + "vw_" + sNombreTabla.Substring(4, sNombreTabla.Length - 4);
                    bTabla = true;
                }
                else
                {
                    sNombreTablaMayuscula = sNombreTabla.Replace("vw_", "Vw_");
                    sNombreTablaMinuscula = sNombreTabla.Replace("Vw_", "vw_");
                    bTabla = false;
                }
                DataTable dtResultado = new DataTable();

                using (connection = new SqlConnection(conexion))
                {
                    //connection.Open();
                    int iNumFilas = 0;
                    if (sQuery != null)
                    {
                        if (connection != null && connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        if (connection != null && connection.State == ConnectionState.Open)
                        {
                            SqlCommand command = new SqlCommand(sQuery, connection);

                            SqlDataAdapter sda = new SqlDataAdapter(command);

                            iNumFilas = sda.Fill(dtResultado);

                        }
                    }

                }
                EstadoJson = JsonConvert.SerializeObject(dtResultado);
                EstadoJson = EstadoJson.Replace("\"[", "\"/");
                EstadoJson = EstadoJson.Replace("]\"", "&\"");
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                string prueba = ex.Message;

            }

            return EstadoJson;
        }

        public long RegistrosTabla(string Tabla)
        {

            List<Object> lista = new List<object>();

            string sNombreTabla = "dbo." + Tabla;
            string sNombreTablaMinuscula = null;
            string sNombreTablaMayuscula = null;
            string sQuery = "Select * from " + sNombreTabla;
            int i = 1;
            int iNumFilas = 0;
            bool bTabla = true;

            SqlConnection connection = null;
#if SERVICESETTINGS
            string conexion = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string conexion = TreeAPI.Properties.Settings.Default.Conexion;
#else
            string conexion = TreeCore.Properties.Settings.Default.Conexion;
#endif



            try
            {
                if (!sNombreTabla.Contains("vw_") && !sNombreTabla.Contains("Vw_"))
                {
                    sNombreTablaMayuscula = sNombreTabla;//sNombreTabla.Substring(0, 3) + "." + "Vw_" + sNombreTabla.Substring(4, sNombreTabla.Length - 4);
                    sNombreTablaMinuscula = sNombreTabla;//sNombreTabla.Substring(0, 3) + "." + "vw_" + sNombreTabla.Substring(4, sNombreTabla.Length - 4);
                    bTabla = true;
                }
                else
                {
                    sNombreTablaMayuscula = sNombreTabla.Replace("vw_", "Vw_");
                    sNombreTablaMinuscula = sNombreTabla.Replace("Vw_", "vw_");
                    bTabla = false;
                }
                DataTable dtResultado = new DataTable();

                using (connection = new SqlConnection(conexion))
                {
                    //connection.Open();

                    if (sQuery != null)
                    {
                        if (connection != null && connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        if (connection != null && connection.State == ConnectionState.Open)
                        {
                            SqlCommand command = new SqlCommand(sQuery, connection);

                            SqlDataAdapter sda = new SqlDataAdapter(command);

                            iNumFilas = sda.Fill(dtResultado);

                        }
                    }

                }


            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                string prueba = ex.Message;

            }

            return iNumFilas;
        }

        public string consultaBD(string Tabla, string Campo, string valor)
        {
            string respuesta = "";
            List<Object> lista = new List<object>();

            string sNombreTabla = "dbo." + Tabla;
            string sNombreTablaMinuscula = null;
            string sNombreTablaMayuscula = null;
            string sQuery = "Select * from " + sNombreTabla + " where " + Campo + " = '" + valor + "'";
            int i = 1;
            bool bTabla = true;
            string usuariodefault = Comun.TREE_SERVICES_USER;


#if SERVICESETTINGS
            string conexion = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string conexion = TreeAPI.Properties.Settings.Default.Conexion;
#else
            string conexion = TreeCore.Properties.Settings.Default.Conexion;
#endif


            SqlConnection connection = new SqlConnection();


            try
            {
                if (!sNombreTabla.Contains("vw_") && !sNombreTabla.Contains("Vw_"))
                {
                    sNombreTablaMayuscula = sNombreTabla;//sNombreTabla.Substring(0, 3) + "." + "Vw_" + sNombreTabla.Substring(4, sNombreTabla.Length - 4);
                    sNombreTablaMinuscula = sNombreTabla;//sNombreTabla.Substring(0, 3) + "." + "vw_" + sNombreTabla.Substring(4, sNombreTabla.Length - 4);
                    bTabla = true;
                }
                else
                {
                    sNombreTablaMayuscula = sNombreTabla.Replace("vw_", "Vw_");
                    sNombreTablaMinuscula = sNombreTabla.Replace("Vw_", "vw_");
                    bTabla = false;
                }

                DataTable dtResultado = new DataTable();
                //connection = new SqlConnection();
                using (connection = new SqlConnection(conexion))
                {
                    //connection.Open();
                    int iNumFilas = 0;
                    if (sQuery != null)
                    {
                        if (connection != null && connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        if (connection != null && connection.State == ConnectionState.Open)
                        {
                            SqlCommand command = new SqlCommand(sQuery, connection);

                            SqlDataAdapter sda = new SqlDataAdapter(command);

                            iNumFilas = sda.Fill(dtResultado);
                            if (iNumFilas == 0 && Campo.Equals("EMail"))
                            {
                                command = new SqlCommand("Select * from usuarios where Email = '" + usuariodefault + "' ", connection);
                                sda = new SqlDataAdapter(command);
                                iNumFilas = sda.Fill(dtResultado);
                            }
                            connection.Close();
                        }
                    }

                }
                respuesta = dtResultado.Rows[0][0].ToString();
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                string prueba = ex.Message;
            }
            return respuesta;
        }

        public string InsertarTabla(DataTable dt, string nombreTabla, string CamposClave, string resultado) //Función para insertar los datos de una tabla 
        {
            string queryColumna = ""; // Se llena los valores de las columnas
            string queryValues = "'"; // Se llena los valores de los registros
            string sQuery = "INSERT INTO " + nombreTabla + " (";
            SqlConnection connection = null;

#if SERVICESETTINGS
            string conexion = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string conexion = TreeAPI.Properties.Settings.Default.Conexion;
#else
            string conexion = TreeCore.Properties.Settings.Default.Conexion;

#endif

            string where = "";
            int contador = 0; // Contador de registros
            int contadorok = 0; // Contador de registros
            int contadorerror = 0; // Contador de registros
            string rowserror = "";
            string datoserror = "";
            string logerror = "";
            resultado = "";
            string datatype = "";


            try
            {

                DataTable dtResultado = new DataTable();

                //Buscar los campos de las columnas para insertarlos en la BD
                contador = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++) // Se irá guardando los datos por cada fila
                {

                    where = existeRegistro(dt, nombreTabla, CamposClave, i);
                    if (where.Equals(""))
                    {
                        sQuery = "INSERT INTO " + nombreTabla + " (";
                        queryColumna = "";
                        queryValues = "'";

                        for (int j = 1; j < dt.Columns.Count; j++)
                        {

                            datatype = dt.Columns[j].DataType.ToString();
                            queryColumna += dt.Columns[j].ColumnName + ", "; //Recuperar los nombres de columna
                            if (!datatype.Equals("System.DateTime"))
                            {
                                queryValues += dt.Rows[i][j] + "', '"; //Recuperar los valores 
                            }
                            else
                            {
                               /* string fecha = dt.Rows[i][j].ToString().Substring(0, 9);
                                //DateTime fechaformato = Convert.ToDateTime(fecha);
                                string Valor = DateTime.Parse(fecha, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                                queryValues += Valor + "', '";*/

                                string fecha = dt.Rows[i][j].ToString().Substring(0, 9);
                                string[] meses = fecha.Split('/');
                                string mes = meses[1];
                                DateTime temp;
                                string Valor = "";
                                if (!(Convert.ToInt32(mes) > 12))
                                {

                                    Valor = DateTime.Parse(fecha, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                                }
                                else
                                {

                                    Valor = "01/01/2021";
                                }
                                queryValues += Valor + "', '";
                                //sQuery += dt.Columns[j].ColumnName + " = '" + Valor + "', ";


                            }
                        }
                        if (queryColumna.Substring(queryColumna.Length - 2).Equals(", "))
                        {
                            queryColumna = queryColumna.Substring(0, queryColumna.Length - 2); //Se elimina los últimos dos caracteres
                        }
                        if (queryValues.Substring(queryValues.Length - 3).Equals(", '"))
                        {
                            queryValues = queryValues.Substring(0, queryValues.Length - 3); //Se elimina los últimos tres caracteres
                        }
                        sQuery += queryColumna + ") VALUES ( " + queryValues + " )"; // Se une todo el comando para ser ejecutado
                    }
                    else
                    {
                        sQuery = "Update " + nombreTabla + " Set ";
                        queryColumna = "";
                        queryValues = "'";

                        //Debe ser el update
                        for (int j = 1; j < dt.Columns.Count; j++)
                        {
                            
                            datatype = dt.Columns[j].DataType.ToString();
                            if (!datatype.Equals("System.DateTime"))
                            {
                                sQuery += dt.Columns[j].ColumnName + " = '" + dt.Rows[i][j].ToString() + "', ";
                            }
                            else
                            {
                                string fecha = dt.Rows[i][j].ToString().Substring(0, 9);
                                string[] meses = fecha.Split('/');
                                string mes = meses[1];
                                DateTime temp;
                                string Valor = "";
                                if (!(Convert.ToInt32(mes) > 12))
                                {
                                    
                                    Valor  = DateTime.Parse(fecha, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    
                                    Valor = "01/01/2021";
                                }
                                
                                sQuery += dt.Columns[j].ColumnName + " = '" + Valor + "', ";
                                
                            }
                        }
                        if (sQuery.Substring(sQuery.Length - 2).Equals(", "))
                        {
                            sQuery = sQuery.Substring(0, sQuery.Length - 2); //Se elimina los últimos dos caracteres
                        }
                        sQuery += where; // Se une todo el comando para ser ejecutado

                    }
                    try
                    {
                        using (connection = new SqlConnection(conexion))
                        {
                            //connection.Open();
                            if (sQuery != null)
                            {
                                if (connection != null && connection.State == ConnectionState.Closed)
                                {
                                    connection.Open();
                                }

                                if (connection != null && connection.State == ConnectionState.Open)
                                {
                                    SqlCommand command = new SqlCommand(sQuery, connection);
                                    int pru = command.ExecuteNonQuery();

                                    SqlDataAdapter sda = new SqlDataAdapter(command);


                                    connection.Close();
                                    contadorok++;
                                    queryColumna = ""; // Se resetea variable
                                    queryValues = "'"; // Se resetea variable
                                    sQuery = "INSERT INTO " + nombreTabla + " ("; //Se resetea variable
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        resultado += "There was a problem with the query " + sQuery + " " + System.Environment.NewLine;
                        if (where != "")
                        {
                            if (where.Substring(0, 6).Equals(" where"))
                            {
                                datoserror = where.Substring(7, where.Length - 7); //Se elimina la palabra where para saber que registro estoy eliminando
                            }
                            contadorerror++;
                            logerror += "There was a problem with the item  " + datoserror + " " + System.Environment.NewLine;
                            logerror += ex.Message + System.Environment.NewLine;
                            datoserror = "";

                        }
                        else
                        {
                            contadorerror++;
                            logerror += "There was a problem with the query  " + sQuery + " " + System.Environment.NewLine;
                            logerror += ex.Message + System.Environment.NewLine;
                            datoserror = "";
                        }
                        if (connection != null && connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
                resultado = nombreTabla + " was successfully upload " + contador.ToString() + " rows " + System.Environment.NewLine +
                    "Rows ok " + contadorok.ToString() + System.Environment.NewLine +
                    "Rows Nok " + contadorerror.ToString() + System.Environment.NewLine;
                if (logerror != "")
                {
                    resultado += "Errors:" + System.Environment.NewLine +
                    logerror + System.Environment.NewLine;
                }

            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                string prueba = ex.Message;
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return resultado;
        }

        public DataTable DTabla(string Tabla)
        {

            List<Object> lista = new List<object>();

            string sNombreTabla = "dbo." + Tabla;
            string sNombreTablaMinuscula = null;
            string sNombreTablaMayuscula = null;
            string sQuery = "Select * from " + sNombreTabla;

#if SERVICESETTINGS
            string conexion = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string conexion = TreeAPI.Properties.Settings.Default.Conexion;
#else
            string conexion = TreeCore.Properties.Settings.Default.Conexion;
#endif


            bool bTabla = true;
            SqlConnection connection = null;
            DataTable dtResultado = new DataTable();


            try
            {
                if (!sNombreTabla.Contains("vw_") && !sNombreTabla.Contains("Vw_"))
                {
                    sNombreTablaMayuscula = sNombreTabla;//sNombreTabla.Substring(0, 3) + "." + "Vw_" + sNombreTabla.Substring(4, sNombreTabla.Length - 4);
                    sNombreTablaMinuscula = sNombreTabla;//sNombreTabla.Substring(0, 3) + "." + "vw_" + sNombreTabla.Substring(4, sNombreTabla.Length - 4);
                    bTabla = true;
                }
                else
                {
                    sNombreTablaMayuscula = sNombreTabla.Replace("vw_", "Vw_");
                    sNombreTablaMinuscula = sNombreTabla.Replace("Vw_", "vw_");
                    bTabla = false;
                }


                try
                {

                    using (connection = new SqlConnection(conexion))
                    {
                        //connection.Open();
                        int iNumFilas = 0;
                        if (sQuery != null)
                        {
                            if (connection != null && connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                            }

                            if (connection != null && connection.State == ConnectionState.Open)
                            {
                                SqlCommand command = new SqlCommand(sQuery, connection);

                                SqlDataAdapter sda = new SqlDataAdapter(command);

                                iNumFilas = sda.Fill(dtResultado);
                                connection.Close();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);

                }




            }



            catch (Exception ex)
            {
                log.Error(ex.Message);
                string prueba = ex.Message;


            }

            return dtResultado;
        }

        public String InsertarTablaVista(long proyectoTipoID, DataTable dt, string nombreTabla, DataTable dtVista, string Vista, string BuscarTabla, string CamposClaves, string resultado) //Función para insertar los datos de una tabla 
        {

            string queryColumna = ""; // Se llena los valores de las columnas
            string queryValues = "'"; // Se llena los valores de los registros
            string sQuery = "";
            SqlConnection connection = null;
            string[] ListaBuscarTabla = null;

#if SERVICESETTINGS
            string conexion = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string conexion = TreeAPI.Properties.Settings.Default.Conexion;
#else
            string conexion = TreeCore.Properties.Settings.Default.Conexion;


#endif
            int contador = dtVista.Rows.Count; // Contador de registros
            int contadorok = 0; // Contador de registros
            int contadorerror = 0; // Contador de registros
            string logerror = ""; // Log de errores
            string datatype = "";

            try
            {
                if (proyectoTipoID == Convert.ToInt32 (Comun.Modulos.GLOBAL))
                {
                    if (!(nombreTabla == "Menus"))
                    {
                        DataTable dtResultado = new DataTable();
                        string datoID = "";
                        int iTabla = 0;
                        string where = "";
                        string datos = "";
                        string datoserror = "";
                        resultado = "";
                        string dato = "";
                        if (BuscarTabla != null)
                        {
                            ListaBuscarTabla = BuscarTabla.Split(';');
                        }
                        string prueba = null;
                        //Buscar los campos de las columnas para insertarlos en la BD

                        for (int i = 0; i < dtVista.Rows.Count; i++) // Se irá guardando los datos por cada fila
                        {
                            // Primero se debe validar si es un registro repetido
                            /*if (nombreTabla.Equals("Menus"))
                            {
                                where = existeRegistro(dtVista, dt, nombreTabla, CamposClaves, i, "MenusModulos;MenusModulos;Modulos");
                            }
                            else*/
                            
                            where = existeRegistro(dtVista, dt, nombreTabla, CamposClaves, i, BuscarTabla);
                             if (where.Equals("")) //Si encuentro where es que debo hacer update
                            {
                             
                                    sQuery = "INSERT INTO " + nombreTabla + " (";
                                    queryColumna = "";
                                    queryValues = "'";
                                    for (int j = 1; j < dtVista.Columns.Count; j++)
                                    {


                                        queryColumna += dt.Columns[j].ColumnName + ", "; //Recuperar los nombres de columna
                                        datatype = dt.Columns[j].DataType.ToString();
                                        if (dt.Columns[j].ColumnName.Equals(dtVista.Columns[j].ColumnName))
                                        {
                                            string datoquery = dtVista.Rows[i][j].ToString();
                                            if (!datoquery.Equals(""))
                                            {
                                                if (!datatype.Equals("System.DateTime"))
                                                {
                                                dato = dtVista.Rows[i][j].ToString().Replace("'", "´");
                                                    queryValues += dato + "', '"; //Recuperar los valores 
                                                dato = "";
                                                }
                                                else
                                                {

                                                    string fecha = dtVista.Rows[i][j].ToString().Substring(0, 9);
                                                    string[] meses = fecha.Split('/');
                                                    string mes = meses[1];
                                                    DateTime temp;
                                                    string Valor = "";
                                                    if (!(Convert.ToInt32(mes) > 12))
                                                    {

                                                        Valor = DateTime.Parse(fecha, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                                                    }
                                                    else
                                                    {

                                                        Valor = "01/01/2021";
                                                    }
                                                                                                  
                                                    queryValues += Valor + "', '";
                                                  
                                                }
                                                if (i == 0 && j == 1)
                                                {
                                                    datos = dt.Columns[j].ColumnName + ": " + dtVista.Rows[i][j]; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                                }
                                            }
                                            else
                                            {
                                                if (dt.Columns[j].ColumnName.Equals("Nombre") || dt.Columns[j].ColumnName.Equals("Pais_En") || dt.Columns[j].ColumnName.Equals("Pais_Fr")
                                                    || dt.Columns[j].ColumnName.Equals("ClaveRecurso") || dt.Columns[j].ColumnName.Equals("EmpresaProveedora") || dt.Columns[j].ColumnName.Equals("CIF")
                                                    || dt.Columns[j].ColumnName.Equals("Perfil_esES") || dt.Columns[j].ColumnName.Equals("Perfil_enUS") || dt.Columns[j].ColumnName.Equals("Perfil_frFR")
                                                     || dt.Columns[j].ColumnName.Equals("Perfil_itIT") || dt.Columns[j].ColumnName.Equals("Descripcion") || dt.Columns[j].ColumnName.Equals("ExtensionURL") || dt.Columns[j].ColumnName.Equals("Codigo"))
                                                {
                                                    queryValues += dtVista.Rows[i][j] + "', '";
                                                }
                                                else
                                                {
                                                    queryValues = queryValues.Substring(0, queryValues.Length - 1);
                                                    queryValues += "null, '";
                                                    if (i == 0 && j == 1)
                                                    {
                                                        datos = dt.Columns[j].ColumnName + ": " + dtVista.Rows[i][j]; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                                    }
                                                }
                                            }
                                        }
                                        else //Debe validar el valor en el campo 
                                        {
                                            if (dtVista.Columns[j].ColumnName.Equals("TipoEmpresaProveedora1") && nombreTabla.Equals("EmpresasProveedoras"))
                                            {
                                                queryValues += dtVista.Rows[i][j] + "', '";
                                            }
                                            else
                                            {
                                                if (ListaBuscarTabla[iTabla] != null)
                                                {
                                                    if (dtVista.Columns[j].ColumnName.Equals("CodigoAtributo1") && nombreTabla.Equals("InventarioAtributos") ||
                                                        dtVista.Columns[j].ColumnName.Equals("TipoEmpresaProveedora1") && nombreTabla.Equals("EmpresasProveedoras"))
                                                    {
                                                        queryValues += dtVista.Rows[i][j] + "', '";
                                                    }
                                                    else
                                                    {
                                                        string campo = GetCampo(dtVista.Columns[j].ColumnName);
                                                        if (campo != null) // revisar si es un campo igual en la misma tabla
                                                        {
                                                            if (!dtVista.Rows[i][j].ToString().Equals(""))
                                                            {
                                                                datoID = consultaBD(ListaBuscarTabla[iTabla], campo, dtVista.Rows[i][j].ToString()); //Hace la consulta de 
                                                                if (datoID != "")
                                                                {
                                                                    queryValues += datoID + "', '";
                                                                    iTabla++;
                                                                    if (i == 0 && j == 1)
                                                                    {
                                                                        datos = dt.Columns[j].ColumnName + ": " + datoID; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    queryValues = queryValues.Substring(0, queryValues.Length - 1);
                                                                    queryValues += "null, '";
                                                                    if (i == 0 && j == 1)
                                                                    {
                                                                        datos = dt.Columns[j].ColumnName + ": " + null; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                                                    }
                                                                    iTabla++;

                                                                }
                                                            }
                                                            else
                                                            {
                                                                queryValues = queryValues.Substring(0, queryValues.Length - 1);
                                                                queryValues += "null, '";
                                                                if (i == 0 && j == 1)
                                                                {
                                                                    datos = dt.Columns[j].ColumnName + ": " + null; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                                                }
                                                                iTabla++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }

                                    //Hacemos la parte del insert
                                    iTabla = 0;
                                    if (queryColumna.Substring(queryColumna.Length - 2).Equals(", "))
                                    {
                                        queryColumna = queryColumna.Substring(0, queryColumna.Length - 2); //Se elimina los últimos dos caracteres
                                    }
                                    if (queryValues.Substring(queryValues.Length - 3).Equals(", '"))
                                    {
                                        queryValues = queryValues.Substring(0, queryValues.Length - 3); //Se elimina los últimos tres caracteres
                                    }
                                    sQuery += queryColumna + ") VALUES ( " + queryValues + " )"; // Se une todo el comando para ser ejecutado
                                
                          
                            }
                            else //Ingresa cuando el regitro está repetido, solo debe cambiar los datos no ingresar un nuevo registro
                            {
                                  sQuery = "Update " + nombreTabla + " Set ";
                                    queryColumna = "";
                                    queryValues = "'";
                                    //Debe ser el update
                                    for (int j = 1; j < dt.Columns.Count; j++)
                                    {


                                        sQuery += dt.Columns[j].ColumnName + " = '"; //Recuperar los nombres de columna
                                        datatype = dt.Columns[j].DataType.ToString();
                                        if (dt.Columns[j].ColumnName.Equals(dtVista.Columns[j].ColumnName))
                                        {
                                            if (!dtVista.Rows[i][j].ToString().Equals(""))
                                            {

                                                if (!datatype.Equals("System.DateTime"))
                                                {
                                                dato = dtVista.Rows[i][j].ToString().Replace("'", "´");
                                                sQuery += dato + "', "; //Recuperar los valores 
                                                dato = "";
                                                }
                                                else
                                                {
                                                    string fecha = dt.Rows[i][j].ToString().Substring(0, 9);
                                                    string[] meses = fecha.Split('/');
                                                    string mes = meses[1];
                                                    DateTime temp;
                                                    string Valor = "";
                                                    if (!(Convert.ToInt32(mes) > 12))
                                                    {

                                                        Valor = DateTime.Parse(fecha, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                                                    }
                                                    else
                                                    {

                                                        Valor = "01/01/2021";
                                                    }

                                                    sQuery += Valor + "', ";




                                                }

                                                if (i == 0 && j == 1)
                                                {
                                                    datos = dt.Columns[j].ColumnName + ": " + dtVista.Rows[i][j]; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                                }
                                            }
                                            else
                                            {
                                                prueba = dtVista.Rows[i][j].ToString();
                                                //string dato = dt.Rows[i][j].ToString();
                                                if (!prueba.Equals(""))
                                                {
                                                    sQuery += dtVista.Rows[i][j].ToString() + "', ";
                                                    if (i == 0 && j == 1)
                                                    {
                                                        datos = dt.Columns[j].ColumnName + ": " + dtVista.Rows[i][j]; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                                    }
                                                }
                                                else
                                                {
                                                    /*if (prueba.Equals(""))
                                                    {
                                                        sQuery += dtVista.Rows[i][j].ToString() + "', ";
                                                    }
                                                    else
                                                    {*/
                                                    if (dt.Columns[j].ColumnName.Equals("Nombre") || dt.Columns[j].ColumnName.Equals("Pais_En") || dt.Columns[j].ColumnName.Equals("Pais_Fr")
                                                        || dt.Columns[j].ColumnName.Equals("ClaveRecurso") || dt.Columns[j].ColumnName.Equals("EmpresaProveedora") || dt.Columns[j].ColumnName.Equals("CIF")
                                                        || dt.Columns[j].ColumnName.Equals("Perfil_esES") || dt.Columns[j].ColumnName.Equals("Perfil_enUS") || dt.Columns[j].ColumnName.Equals("Perfil_frFR")
                                                         || dt.Columns[j].ColumnName.Equals("Perfil_itIT") || dt.Columns[j].ColumnName.Equals("Descripcion") || dt.Columns[j].ColumnName.Equals("ExtensionURL") || dt.Columns[j].ColumnName.Equals("Codigo"))
                                                    {
                                                        sQuery += dtVista.Rows[i][j] + "', ";
                                                    }


                                                    else
                                                    {
                                                        if (sQuery.Substring(sQuery.Length - 1).Equals("'"))
                                                        {
                                                            sQuery = sQuery.Substring(0, sQuery.Length - 1); //Se elimina los últimos dos caracteres
                                                        }
                                                        sQuery += queryValues.Substring(0, queryValues.Length - 1);
                                                        sQuery += "null, ";
                                                        //}
                                                    }
                                                }
                                            }
                                        }
                                        else //Debe validar el valor en el campo 
                                        {
                                            if (ListaBuscarTabla[iTabla] != null)
                                            {
                                                string campo = GetCampo(dtVista.Columns[j].ColumnName);
                                                if (campo != null) // revisar si es un campo igual en la misma tabla
                                                {
                                                    if (!dtVista.Rows[i][j].ToString().Equals(""))
                                                    {
                                                        if (!campo.Equals("ClienteCIF") && !campo.Equals("NombreTablaTablasModeloDatos"))
                                                        {
                                                            datoID = consultaBD(ListaBuscarTabla[iTabla], campo, dtVista.Rows[i][j].ToString()); //Hace la consulta de 
                                                            sQuery += datoID + "', ";
                                                            iTabla++;
                                                        }
                                                    else if (campo.Equals("NombreTablaTablasModeloDatos"))
                                                    {
                                                        datoID = consultaBD(ListaBuscarTabla[iTabla], "NombreTabla", dtVista.Rows[i][j].ToString()); //Hace la consulta de 
                                                        sQuery += datoID + "', ";
                                                        iTabla++;
                                                    }
                                                    else
                                                        {
                                                            datoID = consultaBD(ListaBuscarTabla[iTabla], "CIF", dtVista.Rows[i][j].ToString()); //Hace la consulta de 
                                                            sQuery += datoID + "', ";
                                                            iTabla++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (sQuery.Substring(sQuery.Length - 1).Equals("'"))
                                                        {
                                                            sQuery = sQuery.Substring(0, sQuery.Length - 1); //Se elimina los últimos dos caracteres
                                                        }
                                                        sQuery += queryValues.Substring(0, queryValues.Length - 1);
                                                        sQuery += "null, ";
                                                        iTabla++;
                                                    }
                                                }

                                            }

                                        }
                                    }
                                    iTabla = 0;
                                    if (sQuery.Substring(sQuery.Length - 2).Equals(", "))
                                    {
                                        sQuery = sQuery.Substring(0, sQuery.Length - 2); //Se elimina los últimos dos caracteres
                                    }

                                    sQuery += where; // Se une todo el comando para ser ejecutado
                                
                          
                            }

                            
                                try
                                {
                                    using (connection = new SqlConnection(conexion))
                                    {
                                        //connection.Open();
                                        if (sQuery != null)
                                        {
                                            if (connection != null && connection.State == ConnectionState.Closed)
                                            {
                                                connection.Open();
                                            }

                                            if (connection != null && connection.State == ConnectionState.Open)
                                            {
                                                SqlCommand command = new SqlCommand(sQuery, connection);
                                                int pru = command.ExecuteNonQuery();
                                                SqlDataAdapter sda = new SqlDataAdapter(command);

                                                contadorok++;
                                                connection.Close();
                                                queryColumna = ""; // Se resetea variable
                                                queryValues = "'"; // Se resetea variable
                                                                   //sQuery = "INSERT INTO " + nombreTabla + " ("; //Se resetea variable

                                            }
                                        }

                                    }

                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex.Message);
                                    resultado += "There was a problem with the query " + sQuery + " " + System.Environment.NewLine;
                                    if (where != "")
                                    {
                                        if (where.Substring(0, 6).Equals(" where"))
                                        {
                                            datoserror = where.Substring(7, where.Length - 7); //Se elimina la palabra where para saber que registro estoy eliminando
                                        }
                                        contadorerror++;
                                        logerror += "There was a problem with the item  " + datoserror + " " + System.Environment.NewLine;
                                        logerror += ex.Message + System.Environment.NewLine;
                                        datoserror = "";

                                    }
                                    else
                                    {
                                        contadorerror++;
                                        logerror += "There was a problem with the query  " + sQuery + " " + System.Environment.NewLine;
                                        logerror += ex.Message + System.Environment.NewLine;
                                        datoserror = "";
                                    }
                                    if (connection != null && connection.State == ConnectionState.Open)
                                    {
                                        connection.Close();
                                    }
                                }
                            

                        }
                    }
                }
                else
                {
                    DataTable dtResultado = new DataTable();
                    string datoID = "";
                    int iTabla = 0;
                    string where = "";
                    string datos = "";
                    string datoserror = "";
                    resultado = "";
                    Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos dato = new Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos();
                    CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController ccoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController();
                    Vw_ConfCoreInventarioCategoriasAtributosCategorias datoAtributoCategoria = new Vw_ConfCoreInventarioCategoriasAtributosCategorias();
                    CoreInventarioCategoriasAtributosCategoriasController cdatoAtributoCategoria = new CoreInventarioCategoriasAtributosCategoriasController();

                    if (BuscarTabla != null)
                    {
                        ListaBuscarTabla = BuscarTabla.Split(';');
                    }
                    string prueba = null;
                    //Buscar los campos de las columnas para insertarlos en la BD

                    for (int i = 0; i < dtVista.Rows.Count; i++) // Se irá guardando los datos por cada fila
                    {
                        // Primero se debe validar si es un registro repetido
                        /*if (nombreTabla.Equals("Menus"))
                        {
                            where = existeRegistro(dtVista, dt, nombreTabla, CamposClaves, i, "MenusModulos;MenusModulos;Modulos");
                        }
                        else*/
                        if (nombreTabla.Equals("CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos"))
                        {
                            dato = ccoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos.getConfInventarioCategoriasAtributosConfiguracion(dtVista.Rows[i]["InventarioCategoria"].ToString(),
                               dtVista.Rows[i]["InventarioAtributoCategoria"].ToString(), dtVista.Rows[i]["Nombre"].ToString());
                            if (dato != null)
                            { where = "HacerUpdate"; }
                            else
                            {
                                where = "";
                            }


                        }
                        else if (nombreTabla.Equals("CoreInventarioCategoriasAtributosCategorias"))
                        {
                            datoAtributoCategoria = cdatoAtributoCategoria.GetdatoMigrador(dtVista.Rows[i]["InventarioCategoria"].ToString(),
                               dtVista.Rows[i]["InventarioAtributoCategoria"].ToString(), dtVista.Rows[i]["Codigo"].ToString());
                            if (datoAtributoCategoria != null)
                            { where = "HacerUpdate"; }
                            else
                            {
                                where = "";
                            }


                        }
                        else
                        {
                            where = existeRegistro(dtVista, dt, nombreTabla, CamposClaves, i, BuscarTabla);

                        }

                        if (where.Equals("")) //Si encuentro where es que debo hacer update
                        {
                            if (!nombreTabla.Equals("CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos") && !nombreTabla.Equals("CoreInventarioCategoriasAtributosCategorias"))
                            {

                                sQuery = "INSERT INTO " + nombreTabla + " (";
                                queryColumna = "";
                                queryValues = "'";
                                for (int j = 1; j < dtVista.Columns.Count; j++)
                                {


                                    queryColumna += dt.Columns[j].ColumnName + ", "; //Recuperar los nombres de columna
                                    datatype = dt.Columns[j].DataType.ToString();
                                    if (dt.Columns[j].ColumnName.Equals(dtVista.Columns[j].ColumnName))
                                    {
                                        string datoquery = dtVista.Rows[i][j].ToString();
                                        if (!datoquery.Equals(""))
                                        {
                                            if (!datatype.Equals("System.DateTime"))
                                            {
                                                queryValues += dtVista.Rows[i][j].ToString() + "', '"; //Recuperar los valores 
                                            }
                                            else
                                            {

                                                string fecha = dtVista.Rows[i][j].ToString().Substring(0, 9);
                                                string[] meses = fecha.Split('/');
                                                string mes = meses[1];
                                                DateTime temp;
                                                string Valor = "";
                                                if (!(Convert.ToInt32(mes) > 12))
                                                {

                                                    Valor = DateTime.Parse(fecha, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                                                }
                                                else
                                                {

                                                    Valor = "01/01/2021";
                                                }

                                                //sQuery += dt.Columns[j].ColumnName + " = '" + Valor + "', ";
                                                /*string fecha = dtVista.Rows[i][j].ToString().Substring(0, 9);
                                                //DateTime fechaformato = Convert.ToDateTime(fecha);
                                                string Valor = DateTime.Parse(fecha, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);*/
                                                queryValues += Valor + "', '";
                                                //queryValues += fechaformato.ToString("MM-dd-yyyy") + "', '";



                                            }
                                            if (i == 0 && j == 1)
                                            {
                                                datos = dt.Columns[j].ColumnName + ": " + dtVista.Rows[i][j]; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                            }
                                        }
                                        else
                                        {
                                            if (dt.Columns[j].ColumnName.Equals("Nombre") || dt.Columns[j].ColumnName.Equals("Pais_En") || dt.Columns[j].ColumnName.Equals("Pais_Fr")
                                                || dt.Columns[j].ColumnName.Equals("ClaveRecurso") || dt.Columns[j].ColumnName.Equals("EmpresaProveedora") || dt.Columns[j].ColumnName.Equals("CIF")
                                                || dt.Columns[j].ColumnName.Equals("Perfil_esES") || dt.Columns[j].ColumnName.Equals("Perfil_enUS") || dt.Columns[j].ColumnName.Equals("Perfil_frFR")
                                                || dt.Columns[j].ColumnName.Equals("Perfil_itIT") || dt.Columns[j].ColumnName.Equals("Descripcion") || dt.Columns[j].ColumnName.Equals("ExtensionURL")
                                                 || dt.Columns[j].ColumnName.Equals("Perfil_itIT") || dt.Columns[j].ColumnName.Equals("Descripcion") || dt.Columns[j].ColumnName.Equals("ExtensionURL") || dt.Columns[j].ColumnName.Equals("Codigo"))
                                                
                                            {
                                                queryValues += dtVista.Rows[i][j] + "', '";
                                            }
                                            else
                                            {
                                                queryValues = queryValues.Substring(0, queryValues.Length - 1);
                                                queryValues += "null, '";
                                                if (i == 0 && j == 1)
                                                {
                                                    datos = dt.Columns[j].ColumnName + ": " + dtVista.Rows[i][j]; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                                }
                                            }
                                        }
                                    }
                                    else //Debe validar el valor en el campo 
                                    {
                                        if (dtVista.Columns[j].ColumnName.Equals("TipoEmpresaProveedora1") && nombreTabla.Equals("EmpresasProveedoras"))
                                        {
                                            queryValues += dtVista.Rows[i][j] + "', '";
                                        }
                                        else
                                        {
                                            if (ListaBuscarTabla[iTabla] != null)
                                            {
                                                if (dtVista.Columns[j].ColumnName.Equals("CodigoAtributo1") && nombreTabla.Equals("InventarioAtributos") ||
                                                    dtVista.Columns[j].ColumnName.Equals("TipoEmpresaProveedora1") && nombreTabla.Equals("EmpresasProveedoras"))
                                                {
                                                    queryValues += dtVista.Rows[i][j] + "', '";
                                                }
                                                else
                                                {
                                                    string campo = GetCampo(dtVista.Columns[j].ColumnName);
                                                    if (campo != null) // revisar si es un campo igual en la misma tabla
                                                    {
                                                        if (!dtVista.Rows[i][j].ToString().Equals(""))
                                                        {
                                                            datoID = consultaBD(ListaBuscarTabla[iTabla], campo, dtVista.Rows[i][j].ToString()); //Hace la consulta de 
                                                            if (datoID != "")
                                                            {
                                                                queryValues += datoID + "', '";
                                                                iTabla++;
                                                                if (i == 0 && j == 1)
                                                                {
                                                                    datos = dt.Columns[j].ColumnName + ": " + datoID; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                                                }
                                                            }
                                                            else
                                                            {
                                                                queryValues = queryValues.Substring(0, queryValues.Length - 1);
                                                                queryValues += "null, '";
                                                                if (i == 0 && j == 1)
                                                                {
                                                                    datos = dt.Columns[j].ColumnName + ": " + null; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                                                }
                                                                iTabla++;

                                                            }
                                                        }
                                                        else
                                                        {
                                                            queryValues = queryValues.Substring(0, queryValues.Length - 1);
                                                            queryValues += "null, '";
                                                            if (i == 0 && j == 1)
                                                            {
                                                                datos = dt.Columns[j].ColumnName + ": " + null; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                                            }
                                                            iTabla++;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }

                                //Hacemos la parte del insert
                                iTabla = 0;
                                if (queryColumna.Substring(queryColumna.Length - 2).Equals(", "))
                                {
                                    queryColumna = queryColumna.Substring(0, queryColumna.Length - 2); //Se elimina los últimos dos caracteres
                                }
                                if (queryValues.Substring(queryValues.Length - 3).Equals(", '"))
                                {
                                    queryValues = queryValues.Substring(0, queryValues.Length - 3); //Se elimina los últimos tres caracteres
                                }
                                sQuery += queryColumna + ") VALUES ( " + queryValues + " )"; // Se une todo el comando para ser ejecutado
                            }
                            else if (nombreTabla.Equals("CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos"))
                            {
                                Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguraciones datoInventarioCategoriaAtributoConfiguracion = new Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguraciones();
                                CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cInventarioAtributosCategoriasConfiguracion = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
                                Vw_ConfCoreAtributosConfiguraciones datoAtributosConfiguracion = new Vw_ConfCoreAtributosConfiguraciones();
                                CoreAtributosConfiguracionesController cdatoAtributosConfiguracion = new CoreAtributosConfiguracionesController();
                                CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos datosingresar = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos();
                                CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController cdatosingresar = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController();
                                datoInventarioCategoriaAtributoConfiguracion = cInventarioAtributosCategoriasConfiguracion.GetdatoMigrador(dtVista.Rows[i]["InventarioCategoria"].ToString(), dtVista.Rows[i]["InventarioAtributoCategoria"].ToString());
                                datoAtributosConfiguracion = cdatoAtributosConfiguracion.DatosInventarioMigrador(dtVista.Rows[i]["Nombre"].ToString());
                                if (datoInventarioCategoriaAtributoConfiguracion != null && datoAtributosConfiguracion != null)
                                {
                                    datosingresar.CoreInventarioCategoriaAtributoCategoriaConfiguracionID = datoInventarioCategoriaAtributoConfiguracion.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                                    datosingresar.CoreAtributoConfiguracionID = datoAtributosConfiguracion.CoreAtributoConfiguracionID;
                                    datosingresar.Orden = Convert.ToInt32(dtVista.Rows[i]["Orden"]);
                                    cdatosingresar.AddItem(datosingresar);
                                    contadorok++;
                                }
                                else
                                {
                                    contadorerror++;
                                    logerror += "There was a problem with the item  " + dtVista.Rows[i]["InventarioCategoria"].ToString() + " " + dtVista.Rows[i]["InventarioAtributoCategoria"].ToString() + " " + System.Environment.NewLine;
                                    logerror += "Item no found" + System.Environment.NewLine;
                                    datoserror = "";
                                }


                            }
                            else if (nombreTabla.Equals("CoreInventarioCategoriasAtributosCategorias"))
                            {
                                InventarioCategorias inventarioCategoria = new InventarioCategorias();
                                InventarioCategoriasController cInventarioCategoria = new InventarioCategoriasController();
                                Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguraciones datoInventarioCategoriaAtributoConfiguracion = new Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguraciones();
                                CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cInventarioAtributosCategoriasConfiguracion = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
                                datoInventarioCategoriaAtributoConfiguracion = cInventarioAtributosCategoriasConfiguracion.GetdatoMigrador(dtVista.Rows[i]["InventarioCategoria"].ToString(), dtVista.Rows[i]["InventarioAtributoCategoria"].ToString());
                                inventarioCategoria = cInventarioCategoria.GetCategoriaByCodigo(dtVista.Rows[i]["Codigo"].ToString());
                                CoreInventarioCategoriasAtributosCategorias datosIngresar = new CoreInventarioCategoriasAtributosCategorias();
                                CoreInventarioCategoriasAtributosCategoriasController cDatosIngresar = new CoreInventarioCategoriasAtributosCategoriasController();
                                if (datoInventarioCategoriaAtributoConfiguracion != null && inventarioCategoria != null)
                                {
                                    datosIngresar.CoreInventarioCategoriaAtributoCategoriaConfiguracionID = datoInventarioCategoriaAtributoConfiguracion.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                                    datosIngresar.InventarioCategoriaID = inventarioCategoria.InventarioCategoriaID;
                                    datosIngresar.Orden = Convert.ToInt32(dtVista.Rows[i]["Orden"]);
                                    cDatosIngresar.AddItem(datosIngresar);
                                    contadorok++;
                                }
                                else
                                {
                                    contadorerror++;
                                    logerror += "There was a problem with the item  " + dtVista.Rows[i]["Codigo"].ToString() + " " + dtVista.Rows[i]["InventarioCategoria"].ToString() + " " + dtVista.Rows[i]["InventarioAtributoCategoria"].ToString() + " " + System.Environment.NewLine;
                                    logerror += "Item no found" + System.Environment.NewLine;
                                    datoserror = "";
                                }
                            }
                            else
                            {

                            }
                        }
                        else //Ingresa cuando el regitro está repetido, solo debe cambiar los datos no ingresar un nuevo registro
                        {
                            if (!nombreTabla.Equals("CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos") && !nombreTabla.Equals("CoreInventarioCategoriasAtributosCategorias"))
                            {
                                sQuery = "Update " + nombreTabla + " Set ";
                                queryColumna = "";
                                queryValues = "'";
                                //Debe ser el update
                                for (int j = 1; j < dt.Columns.Count; j++)
                                {


                                    sQuery += dt.Columns[j].ColumnName + " = '"; //Recuperar los nombres de columna
                                    datatype = dt.Columns[j].DataType.ToString();
                                    if (dt.Columns[j].ColumnName.Equals(dtVista.Columns[j].ColumnName))
                                    {
                                        if (!dtVista.Rows[i][j].ToString().Equals(""))
                                        {

                                            if (!datatype.Equals("System.DateTime"))
                                            {
                                                sQuery += dtVista.Rows[i][j] + "', "; //Recuperar los valores 
                                            }
                                            else
                                            {
                                                string fecha = dt.Rows[i][j].ToString().Substring(0, 9);
                                                string[] meses = fecha.Split('/');
                                                string mes = meses[1];
                                                DateTime temp;
                                                string Valor = "";
                                                if (!(Convert.ToInt32(mes) > 12))
                                                {

                                                    Valor = DateTime.Parse(fecha, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                                                }
                                                else
                                                {

                                                    Valor = "01/01/2021";
                                                }

                                                sQuery += Valor + "', ";




                                            }

                                            if (i == 0 && j == 1)
                                            {
                                                datos = dt.Columns[j].ColumnName + ": " + dtVista.Rows[i][j]; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                            }
                                        }
                                        else
                                        {
                                            prueba = dtVista.Rows[i][j].ToString();
                                            //string dato = dt.Rows[i][j].ToString();
                                            if (!prueba.Equals(""))
                                            {
                                                sQuery += dtVista.Rows[i][j].ToString() + "', ";
                                                if (i == 0 && j == 1)
                                                {
                                                    datos = dt.Columns[j].ColumnName + ": " + dtVista.Rows[i][j]; //Recupero el  valor de la columna y el dato que tiene la columna para usarlo en el log
                                                }
                                            }
                                            else
                                            {
                                                /*if (prueba.Equals(""))
                                                {
                                                    sQuery += dtVista.Rows[i][j].ToString() + "', ";
                                                }
                                                else
                                                {*/
                                                if (dt.Columns[j].ColumnName.Equals("Nombre") || dt.Columns[j].ColumnName.Equals("Pais_En") || dt.Columns[j].ColumnName.Equals("Pais_Fr")
                                                    || dt.Columns[j].ColumnName.Equals("ClaveRecurso") || dt.Columns[j].ColumnName.Equals("EmpresaProveedora") || dt.Columns[j].ColumnName.Equals("CIF")
                                                    || dt.Columns[j].ColumnName.Equals("Perfil_esES") || dt.Columns[j].ColumnName.Equals("Perfil_enUS") || dt.Columns[j].ColumnName.Equals("Perfil_frFR")
                                                     || dt.Columns[j].ColumnName.Equals("Perfil_itIT") || dt.Columns[j].ColumnName.Equals("Descripcion") || dt.Columns[j].ColumnName.Equals("ExtensionURL") || dt.Columns[j].ColumnName.Equals("Codigo"))
                                                {
                                                    sQuery += dtVista.Rows[i][j] + "', ";
                                                }


                                                else
                                                {
                                                    if (sQuery.Substring(sQuery.Length - 1).Equals("'"))
                                                    {
                                                        sQuery = sQuery.Substring(0, sQuery.Length - 1); //Se elimina los últimos dos caracteres
                                                    }
                                                    sQuery += queryValues.Substring(0, queryValues.Length - 1);
                                                    sQuery += "null, ";
                                                    //}
                                                }
                                            }
                                        }
                                    }
                                    else //Debe validar el valor en el campo 
                                    {
                                        if (ListaBuscarTabla[iTabla] != null)
                                        {
                                            string campo = GetCampo(dtVista.Columns[j].ColumnName);
                                            if (campo != null) // revisar si es un campo igual en la misma tabla
                                            {
                                                if (!dtVista.Rows[i][j].ToString().Equals(""))
                                                {
                                                    if (!campo.Equals("ClienteCIF") && !campo.Equals("CodigoCoreModulos") && !campo.Equals("NombreTablaTablasModeloDatos"))
                                                    {
                                                        datoID = consultaBD(ListaBuscarTabla[iTabla], campo, dtVista.Rows[i][j].ToString()); //Hace la consulta de 
                                                        sQuery += datoID + "', ";
                                                        iTabla++;
                                                    }
                                                    else if (campo.Equals("CodigoCoreModulos"))
                                                    {
                                                        datoID = consultaBD(ListaBuscarTabla[iTabla], "Codigo", dtVista.Rows[i][j].ToString()); //Hace la consulta de 
                                                        sQuery += datoID + "', ";
                                                        iTabla++;
                                                    }
                                                    else if (campo.Equals("ClienteCIF"))
                                                    {
                                                        datoID = consultaBD(ListaBuscarTabla[iTabla], "CIF", dtVista.Rows[i][j].ToString()); //Hace la consulta de 
                                                        sQuery += datoID + "', ";
                                                        iTabla++;
                                                    }
                                                    else if (campo.Equals("NombreTablaTablasModeloDatos"))
                                                    {
                                                        datoID = consultaBD(ListaBuscarTabla[iTabla], "NombreTabla", dtVista.Rows[i][j].ToString()); //Hace la consulta de 
                                                        sQuery += datoID + "', ";
                                                        iTabla++;
                                                    }
                                                }
                                                else
                                                {
                                                    if (sQuery.Substring(sQuery.Length - 1).Equals("'"))
                                                    {
                                                        sQuery = sQuery.Substring(0, sQuery.Length - 1); //Se elimina los últimos dos caracteres
                                                    }
                                                    sQuery += queryValues.Substring(0, queryValues.Length - 1);
                                                    sQuery += "null, ";
                                                    iTabla++;
                                                }
                                            }

                                        }

                                    }
                                }
                                iTabla = 0;
                                if (sQuery.Substring(sQuery.Length - 2).Equals(", "))
                                {
                                    sQuery = sQuery.Substring(0, sQuery.Length - 2); //Se elimina los últimos dos caracteres
                                }

                                sQuery += where; // Se une todo el comando para ser ejecutado
                            }
                            else if (nombreTabla.Equals("CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos"))
                            {
                                Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguraciones datoInventarioCategoriaAtributoConfiguracion = new Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguraciones();
                                CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cInventarioAtributosCategoriasConfiguracion = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
                                Vw_ConfCoreAtributosConfiguraciones datoAtributosConfiguracion = new Vw_ConfCoreAtributosConfiguraciones();
                                CoreAtributosConfiguracionesController cdatoAtributosConfiguracion = new CoreAtributosConfiguracionesController();
                                CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos datosingresar = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos();
                                CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController cdatosingresar = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController();
                                datoInventarioCategoriaAtributoConfiguracion = cInventarioAtributosCategoriasConfiguracion.GetdatoMigrador(dtVista.Rows[i]["InventarioCategoria"].ToString(), dtVista.Rows[i]["InventarioAtributoCategoria"].ToString());
                                datoAtributosConfiguracion = cdatoAtributosConfiguracion.DatosInventarioMigrador(dtVista.Rows[i]["Nombre"].ToString());
                                if (datoInventarioCategoriaAtributoConfiguracion != null && datoAtributosConfiguracion != null)
                                {
                                    datosingresar = cdatosingresar.GetItem(dato.CoreInventarioCategoriaAtributoCategoriaConfiguracionAtributoID);
                                    if (datosingresar != null)
                                    {
                                        datosingresar.CoreInventarioCategoriaAtributoCategoriaConfiguracionID = datoInventarioCategoriaAtributoConfiguracion.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                                        datosingresar.CoreAtributoConfiguracionID = datoAtributosConfiguracion.CoreAtributoConfiguracionID;
                                        datosingresar.Orden = Convert.ToInt32(dtVista.Rows[i]["Orden"]);
                                        cdatosingresar.UpdateItem(datosingresar);
                                        contadorok++;
                                    }
                                }
                                else
                                {
                                    contadorerror++;
                                    logerror += "There was a problem with the item  " + dtVista.Rows[i]["InventarioCategoria"].ToString() + " " + dtVista.Rows[i]["InventarioAtributoCategoria"].ToString() + " " + System.Environment.NewLine;
                                    logerror += "Item no found" + System.Environment.NewLine;
                                    datoserror = "";
                                }


                            }
                            else if (nombreTabla.Equals("CoreInventarioCategoriasAtributosCategorias"))
                            {
                                InventarioCategorias inventarioCategoria = new InventarioCategorias();
                                InventarioCategoriasController cInventarioCategoria = new InventarioCategoriasController();
                                Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguraciones datoInventarioCategoriaAtributoConfiguracion = new Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguraciones();
                                CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cInventarioAtributosCategoriasConfiguracion = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
                                datoInventarioCategoriaAtributoConfiguracion = cInventarioAtributosCategoriasConfiguracion.GetdatoMigrador(dtVista.Rows[i]["InventarioCategoria"].ToString(), dtVista.Rows[i]["InventarioAtributoCategoria"].ToString());
                                inventarioCategoria = cInventarioCategoria.GetCategoriaByCodigo(dtVista.Rows[i]["Codigo"].ToString());
                                CoreInventarioCategoriasAtributosCategorias datosIngresar = new CoreInventarioCategoriasAtributosCategorias();
                                CoreInventarioCategoriasAtributosCategoriasController cDatosIngresar = new CoreInventarioCategoriasAtributosCategoriasController();

                                if (datoInventarioCategoriaAtributoConfiguracion != null && inventarioCategoria != null)
                                {
                                    datosIngresar = cDatosIngresar.GetItem(datoAtributoCategoria.CoreInventarioCategoriaAtributoCategoriaID);
                                    if (datosIngresar != null)
                                    {
                                        datosIngresar.CoreInventarioCategoriaAtributoCategoriaConfiguracionID = datoInventarioCategoriaAtributoConfiguracion.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                                        datosIngresar.InventarioCategoriaID = inventarioCategoria.InventarioCategoriaID;
                                        datosIngresar.Orden = Convert.ToInt32(dtVista.Rows[i]["Orden"]);
                                        cDatosIngresar.UpdateItem(datosIngresar);
                                        contadorok++;
                                    }
                                }
                                else
                                {
                                    contadorerror++;
                                    logerror += "There was a problem with the item  " + dtVista.Rows[i]["Codigo"].ToString() + " " + dtVista.Rows[i]["InventarioCategoria"].ToString() + " " + dtVista.Rows[i]["InventarioAtributoCategoria"].ToString() + " " + System.Environment.NewLine;
                                    logerror += "Item no found" + System.Environment.NewLine;
                                    datoserror = "";
                                }
                            }
                            else
                            {

                            }
                        }

                        if (!nombreTabla.Equals("CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos") && !nombreTabla.Equals("CoreInventarioCategoriasAtributosCategorias"))
                        {
                            try
                            {
                                using (connection = new SqlConnection(conexion))
                                {
                                    sQuery = sQuery.Replace("/&", "[]");
                                    //connection.Open();
                                    if (sQuery != null)
                                    {
                                        if (connection != null && connection.State == ConnectionState.Closed)
                                        {
                                            connection.Open();
                                        }

                                        if (connection != null && connection.State == ConnectionState.Open)
                                        {
                                            SqlCommand command = new SqlCommand(sQuery, connection);
                                            int pru = command.ExecuteNonQuery();
                                            SqlDataAdapter sda = new SqlDataAdapter(command);

                                            contadorok++;
                                            connection.Close();
                                            queryColumna = ""; // Se resetea variable
                                            queryValues = "'"; // Se resetea variable
                                                               //sQuery = "INSERT INTO " + nombreTabla + " ("; //Se resetea variable

                                        }
                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                resultado += "There was a problem with the query " + sQuery + " " + System.Environment.NewLine;
                                if (where != "")
                                {
                                    if (where.Substring(0, 6).Equals(" where"))
                                    {
                                        datoserror = where.Substring(7, where.Length - 7); //Se elimina la palabra where para saber que registro estoy eliminando
                                    }
                                    contadorerror++;
                                    logerror += "There was a problem with the item  " + datoserror + " " + System.Environment.NewLine;
                                    logerror += ex.Message + System.Environment.NewLine;
                                    datoserror = "";

                                }
                                else
                                {
                                    contadorerror++;
                                    logerror += "There was a problem with the query  " + sQuery + " " + System.Environment.NewLine;
                                    logerror += ex.Message + System.Environment.NewLine;
                                    datoserror = "";
                                }
                                if (connection != null && connection.State == ConnectionState.Open)
                                {
                                    connection.Close();
                                }
                            }
                        }

                    }
                } //Termina el if de Menus
                resultado = nombreTabla + " was successfully upload " + contador.ToString() + " rows " + System.Environment.NewLine +
                    "Rows ok " + contadorok.ToString() + System.Environment.NewLine +
                    "Rows Nok " + contadorerror.ToString() + System.Environment.NewLine;
                if (logerror != "")
                {
                    resultado += "Errors:" + System.Environment.NewLine +
                    logerror + System.Environment.NewLine;
                }
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                string prueba = ex.Message;
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return resultado;
        }

        public string existeRegistro(DataTable dtVista, DataTable dt, string nombreTabla, string CamposLlave, int fila, string buscarTabla) //Función para buscar registros duplicados 
        {
            string[] sNombreTabla = null; // 
            string queryValues = "'"; // Se llena los valores de los registros
            string sQuery = "Select * from  " + nombreTabla;
            string where = " where ";
            SqlConnection connection = null;

#if SERVICESETTINGS
            string conexion = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string conexion = TreeAPI.Properties.Settings.Default.Conexion;
#else
            string conexion = TreeCore.Properties.Settings.Default.Conexion;
#endif

            string[] Campos = null;
            int iTabla = 0;
            string datoID = "";
            string valor = "";

            try
            {

                DataTable dtResultado = new DataTable();
                if (CamposLlave != null) // Se saca los campos llaves por tabla que debemos validar
                {
                    Campos = CamposLlave.Split(';');
                }
                if (buscarTabla != null) // Se saca los campos llaves por tabla que debemos validar
                {
                    sNombreTabla = buscarTabla.Split(';');
                }
                //Buscar los campos de las columnas para insertarlos en la BD


                for (int j = 1; j < dt.Columns.Count; j++)
                {
                    for (int a = 0; a < Campos.Count(); a++)
                    {
                        if (dtVista.Columns[j].ColumnName.Equals(Campos[a]))
                        {
                            ////Primero se debe convertir el valor del campo de la vista al valor de la tabla
                            if (dt.Columns[j].ColumnName.Equals(dtVista.Columns[j].ColumnName.ToString()))
                            {
                                valor = dtVista.Rows[fila][j].ToString().Replace("'", "´");
                                where += Campos[a] + "= '" + valor + "' and "; //Recuperar los nombres de columna

                            }
                            else
                            {
                                if (sNombreTabla[iTabla] != null)
                                {
                                    string campo = GetCampo(dtVista.Columns[j].ColumnName);
                                    datoID = consultaBD(sNombreTabla[iTabla], campo, dtVista.Rows[fila][j].ToString()); //Hace la consulta de 
                                    if (!datoID.Equals(""))
                                    {
                                        where += dt.Columns[j].ColumnName.ToString() + "= '" + datoID + "' and ";

                                    }
                                    else
                                    {
                                        where += dt.Columns[j].ColumnName.ToString() + " is null and ";

                                    }
                                    iTabla++;
                                }
                            }

                        }

                    }

                }
                iTabla = 0;
                if (where.Substring(where.Length - 5).Equals(" and "))
                {
                    where = where.Substring(0, where.Length - 5); //Se elimina los últimos dos caracteres
                }
                // Se une todo el comando para ser ejecutado
                sQuery += where;
                using (connection = new SqlConnection(conexion))
                {
                    //connection.Open();
                    if (sQuery != null)
                    {
                        if (connection != null && connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        if (connection != null && connection.State == ConnectionState.Open)
                        {
                            SqlCommand command = new SqlCommand(sQuery, connection);
                            int pru = command.ExecuteNonQuery();

                            SqlDataAdapter sda = new SqlDataAdapter(command);
                            fila = sda.Fill(dtResultado);

                            connection.Close();

                            if (dtResultado.Rows.Count > 0)
                                return where;
                            
                        }
                    }

                }

                return "";
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                string prueba = ex.Message;
                return "";
            }
        }

        #region ExisteRegistroInventario
        public string existeRegistroInventario(DataTable dtVista, DataTable dt, string nombreTabla, string CamposLlave, int fila, string buscarTabla) //Función para buscar registros duplicados 
        {
            string[] sNombreTabla = null; // 
            string queryValues = "'"; // Se llena los valores de los registros
            string sQuery = "Select * from  " + nombreTabla;
            string where = " where ";
            SqlConnection connection = null;
            Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos dato = new Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos();
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController coreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController();

#if SERVICESETTINGS
            string conexion = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string conexion = TreeAPI.Properties.Settings.Default.Conexion;
#else
            string conexion = TreeCore.Properties.Settings.Default.Conexion;
#endif

            string[] Campos = null;
            int iTabla = 0;
            string datoID = "";

            try
            {

                DataTable dtResultado = new DataTable();
                if (CamposLlave != null) // Se saca los campos llaves por tabla que debemos validar
                {
                    Campos = CamposLlave.Split(';');
                }
                if (buscarTabla != null) // Se saca los campos llaves por tabla que debemos validar
                {
                    sNombreTabla = buscarTabla.Split(';');
                }
                //Buscar los campos de las columnas para insertarlos en la BD





                dato = coreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos.getConfInventarioCategoriasAtributosConfiguracion(dtVista.Rows[fila]["InventarioCategoria"].ToString(),
                    dtVista.Rows[fila]["InventarioAtributoCategoria"].ToString(), dtVista.Rows[fila]["Nombre"].ToString());




                return "";
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                string prueba = ex.Message;
                return "";
            }
        }
        #endregion

        public string existeRegistro(DataTable dt, string nombreTabla, string CamposLlave, int fila) //Función para buscar registros duplicados cuando no tiene vista relacionada
        {
            string queryColumna = ""; // Se llena los valores de las columnas
            string queryValues = "'"; // Se llena los valores de los registros
            string sQuery = "Select * from  " + nombreTabla;
            string where = " where ";
            SqlConnection connection = null;

#if SERVICESETTINGS
            string conexion = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string conexion = TreeAPI.Properties.Settings.Default.Conexion;
#else
            string conexion = TreeCore.Properties.Settings.Default.Conexion;
#endif


            string[] Campos = null;
            int iTabla = 0;
            string datoID = "";

            try
            {

                DataTable dtResultado = new DataTable();
                if (CamposLlave != null) // Se saca los campos llaves por tabla que debemos validar
                {
                    Campos = CamposLlave.Split(';');
                }
                //Buscar los campos de las columnas para insertarlos en la BD
                int camposcount = Campos.Count();

                for (int j = 1; j < dt.Columns.Count; j++)
                {
                    for (int a = 0; a < Campos.Count(); a++)
                    {
                        if (dt.Columns[j].ColumnName.Equals(Campos[a]))
                        {
                            ////Primero se debe convertir el valor del campo de la vista al valor de la tabla
                            object datosprueba = null;
                            datosprueba = dt.Rows[fila][j];
                            if (!(datosprueba == null))
                            {
                                where += Campos[a] + "= '" + dt.Rows[fila][j] + "' and "; //Recuperar los nombres de columna

                            }
                            else
                            {
                                where += Campos[a] + " is null and ";

                            }

                        }
                    }





                }

                if (where.Substring(where.Length - 5).Equals(" and "))
                {
                    where = where.Substring(0, where.Length - 5); //Se elimina los últimos dos caracteres
                }
                // Se une todo el comando para ser ejecutado
                sQuery += where;
                using (connection = new SqlConnection(conexion))
                {
                    //connection.Open();
                    if (sQuery != null)
                    {
                        if (connection != null && connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        if (connection != null && connection.State == ConnectionState.Open)
                        {
                            SqlCommand command = new SqlCommand(sQuery, connection);
                            int pru = command.ExecuteNonQuery();

                            SqlDataAdapter sda = new SqlDataAdapter(command);
                            fila = sda.Fill(dtResultado);

                            connection.Close();

                            if (dtResultado.Rows.Count > 0)
                                return where;
                        }
                    }

                }

                return "";
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                string prueba = ex.Message;
                return "";
            }
        }
    }


}