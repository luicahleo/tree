using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreExportacionDatosPlantillasController : GeneralBaseController<CoreExportacionDatosPlantillas, TreeCoreContext>
    {
        public CoreExportacionDatosPlantillasController()
            : base()
        { }

        public bool RegistroDuplicado(string nombre)
        {
            bool duplicado = false;
            
            try
            {
                duplicado = (from c in Context.CoreExportacionDatosPlantillas
                             where c.Nombre == nombre
                             select c).Count() > 0;
            }
            catch(Exception ex)
            {
                duplicado = true;
                log.Error(ex.Message);
            }

            return duplicado;
        }


        public List<Vw_CoreExportacionDatosPlantillas> GetActivosVista()
        {
            List<Vw_CoreExportacionDatosPlantillas> plantillas;
            try
            {
                plantillas = (from c in Context.Vw_CoreExportacionDatosPlantillas
                              where c.Activo
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                plantillas = null;
            }

            return plantillas;
        }

        public List<CoreExportacionDatosPlantillas> GetActivos()
        {
            List<CoreExportacionDatosPlantillas> plantillas;
            try
            {
                plantillas = (from c in Context.CoreExportacionDatosPlantillas
                              where c.Activo
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                plantillas = null;
            }

            return plantillas;
        }

        #region Consulta exportación
        public List<JsonObject> ejecutarConsulta(string queryString)
        {
            ILog Log = LogManager.GetLogger("");
            Log.Info("EXECUTED_QUERY: " + queryString);
            #region CADENA CONEXIÓN
#if SERVICESETTINGS
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string connectionString = TreeAPI.Properties.Settings.Default.Conexion;
#else
            string connectionString = TreeCore.Properties.Settings.Default.Conexion;
#endif
            #endregion

            List<JsonObject> result;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        result = DataTableToListJsonObject(dataTable);
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

        private static List<JsonObject> DataTableToListJsonObject(DataTable oDataTable)
        {
            List<JsonObject> listaDatos = new List<JsonObject>();
            JsonObject oJson;
            try
            {
                foreach (DataRow row in oDataTable.Rows)
                {
                    oJson = new JsonObject();

                    for (int i = 0; i < oDataTable.Columns.Count; i++)
                    {
                        oJson.Add(oDataTable.Columns[i].ToString(), row[i]);
                    }

                    listaDatos.Add(oJson);
                }

            }
            catch (Exception ex)
            {
                listaDatos = null;
            }
            return listaDatos;
        }


        #endregion

        public CoreExportacionDatosPlantillas GetByCelda(long celdaID)
        {
            CoreExportacionDatosPlantillas plantilla;
            try
            {
                plantilla = (from pl in Context.CoreExportacionDatosPlantillas 
                                join fl in Context.CoreExportacionDatosPlantillasFilas on pl.CoreExportacionDatoPlantillaID equals fl.CoreExportacionDatosPlantillaID
                                join celd in Context.CoreExportacionDatosPlantillasCeldas on fl.CoreExportacionDatosPlantillaFilaID equals celd.CoreExportacionDatosPlantillaFilaID
                             where celd.CoreExportacionDatosPlantillasCeldasID==celdaID
                             select pl).First();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                plantilla = null;
            }

            return plantilla;
        }
    }
}