using System;
using System.Data;
using System.Configuration;
using System.Linq;
using Tree.Linq;
using System.Collections.Generic;
using TreeCore.Data;
using System.Data.Linq;
using System.Reflection;
using log4net;
using System.Data.SqlClient;
using Tree.Linq.Dynamic;

namespace CapaNegocio
{
    /// <summary>
    /// Clase Base de Negocio con Conexión a General
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class GeneralBaseController<TEntity, TContext> : TreeBaseControllerExtNet<TEntity, TContext>
        where TEntity : class, new()
        where TContext : DataContext
    {
        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public GeneralBaseController()

#if SERVICESETTINGS
        : base(System.Configuration.ConfigurationManager.AppSettings["Conexion"])
#elif TREEAPI
        : base(ServerConnectionChooser.getServerString(TreeAPI.Properties.Settings.Default.Conexion))
#else
        : base(ServerConnectionChooser.getServerString(TreeCore.Properties.Settings.Default.Conexion))
#endif
        {
            //string srv = ServerConnectionChooser.getStoredString(Sites.Properties.Settings.Default.SMTP_Servidor);
            //string mailAddr = ServerConnectionChooser.getStoredString(Sites.Properties.Settings.Default.SMTP_Mail);
            
        }

        //Filtro resultados KPI
        public List<object> FiltroListaPrincipalByIDs(List<object> listaIn, List<long> ListIDs, string nameIndiceID)
        {
            List<object> listaOut = new List<object>();
            try
            {
                listaIn.ForEach(obj => {
                    
                    Type tipo = obj.GetType();
                    PropertyInfo propName = tipo.GetProperty(nameIndiceID);
                    long id = (long)propName.GetValue(obj, null);


                    if (ListIDs.Contains(id))
                    {
                        listaOut.Add(obj);
                    }
                });

            }
            catch (Exception ex)
            {
                listaOut = null;
                log.Error(ex.Message);
            }

            return listaOut;
        }

        public List<TEntity> GetActivos(long clienteID)
        {
            List<TEntity> lista;
            try
            {
                lista = GetItemsList<TEntity>("ClienteID==" + clienteID.ToString(), "").ToList();
            }
            catch (ParseException ex)
            {
                lista = GetItemsList<TEntity>().ToList();
                //lista = null;
            }catch (Exception ex)
            {
                lista = null;
                log.Error(ex.Message);
            }
            return lista;
        }

        public DataTable EjecutarQuery(string queryString)
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
    }

    interface IBasica<T> {
        bool RegistroVinculado(long id);
        bool RegistroDefecto(long id);
        bool RegistroDuplicado(string nombre, long clienteID);
    }
}
