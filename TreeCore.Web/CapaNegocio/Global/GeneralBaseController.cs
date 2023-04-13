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
using TreeCore.Clases;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;
using Tree.Linq.GenericExtensions;
using System.Web;
using System.Globalization;

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
        private List<string> primaryKeys;
        private System.Type entityType;
        private CultureInfo cultureInfo;
        private static global::System.Resources.ResourceManager resourceMan;

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
            entityType = typeof(TEntity);
            primaryKeys = getPrimaryKeys();
        }
        
        #region Private method

        private List<string> getPrimaryKeys()
        {
            ReadOnlyCollection<MetaDataMember> members;
            List<string> primaryKeys = new List<string>();

            MetaTable model = Context.Mapping.GetTable(entityType);

            if (model == null) throw new Exception("La entidad no existe para el contexto de dato.");

            members = model.RowType.DataMembers;

            foreach (MetaDataMember member in members)
            {
                if (member.IsPrimaryKey)
                {
                    primaryKeys.Add(member.Name);
                }
            }
            return primaryKeys;
        }

        private void CloneEntity(TEntity currentEntity, ref TEntity originalEntity)
        {
            if (originalEntity == null)
            {
                ConstructorInfo ctorInfo = entityType.GetConstructor(Type.EmptyTypes);
                if (ctorInfo == null) return;

                originalEntity = (TEntity)ctorInfo.Invoke(new object[0]);
            }

            foreach (PropertyInfo propInfo in entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                object currentValue = null;
                object originalValue = null;
                if (propInfo.CanRead)
                {
                    originalValue = propInfo.GetValue(originalEntity, null);
                    currentValue = propInfo.GetValue(currentEntity, null);
                    try
                    {
                        if (propInfo.CanWrite)
                        {
                            if (currentValue == null)
                            {
                                propInfo.SetValue(originalEntity, null, null);
                            }
                            else
                            {
                                if (!currentValue.Equals(originalValue))
                                {
                                    propInfo.SetValue(originalEntity, currentValue, null);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        #endregion

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
                if(clienteID == 0)
                {
                    throw new ParseException("Client is 0", 0);
                }
                else
                {
                    lista = GetItemsList<TEntity>("ClienteID==" + clienteID.ToString(), "").ToList();
                }
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

        public InfoResponse SubmitChanges()
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                Context.SubmitChanges(); 
                oResponse = new InfoResponse
                {
                    Result = true,
                    Code = "",
                    Description = "",
                    Data = Context
                };
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Data = null
                };
                if (ex is SqlException Sql)
                {
                    oResponse.Description = GetGlobalResource(Comun.jsTieneRegistros);
                }
                else
                {
                    oResponse.Description = GetGlobalResource(Comun.strMensajeGenerico);
                }
            }
            return oResponse;
        }
        public InfoResponse DiscardChanges()
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                Context.GetChangeSet().Deletes.Clear();
                Context.GetChangeSet().Updates.Clear();
                Context.GetChangeSet().Inserts.Clear();
                oResponse = new InfoResponse
                {
                    Result = true,
                    Code = "",
                    Description = "",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = GetGlobalResource(Comun.strMensajeGenerico),
                    Data = Context
                };
            }
            return oResponse;
        }

        private TEntity AddWithoutContext(TEntity item)
        {
            if (item == null) throw new NullReferenceException();
            Table<TEntity> table = Context.GetTable<TEntity>();

            if (table != null)
            {
                table.InsertOnSubmit(item);
                //context.SubmitChanges();
                return item;
            }
            else
            {
                throw new ApplicationException("No hay un conjunto de entidad definido");
            }
        }

        private bool UpdateWithoutContext(TEntity item)
        {
            if (item == null) throw new NullReferenceException();

            PropertyInfo propInfo = default(PropertyInfo);
            List<object> valores = new List<object>();
            TEntity originalItem = default(TEntity);
            foreach (string key in primaryKeys)
            {
                propInfo = entityType.GetProperty(key, BindingFlags.Public | BindingFlags.Instance);
                if (propInfo != null)
                {
                    try
                    {
                        valores.Add(propInfo.GetValue(item, null));
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("No se ha podido obtener valor de uno de los clave primaria", ex);
                    }
                }
                else
                {
                    throw new Exception("Uno de los claves primarias no está definida");
                }
            }

            originalItem = GetItem(valores.ToArray());
            if (originalItem != null)
            {
                CloneEntity(item, ref originalItem);
                //context.SubmitChanges();
            }
            return true;
        }

        private bool DeleteWithoutContext(TEntity toDelete)
        {
            if (toDelete != null)
            {
                Context.GetTable<TEntity>().DeleteOnSubmit(toDelete);
                //context.SubmitChanges();
            }
            return true;
        }

        protected InfoResponse AddEntity(TEntity oDato) {
            TEntity oEntidad;
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if ((oEntidad = AddWithoutContext(oDato)) != null)
                {
                    oResponse = new InfoResponse
                    {
                        Result = true,
                        Code = "",
                        Description = "",
                        Data = oEntidad
                    };
                }
                else
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.strMensajeGenerico,
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        protected InfoResponse UpdateEntity(TEntity oDato)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (UpdateWithoutContext(oDato))
                {
                    oResponse = new InfoResponse
                    {
                        Result = true,
                        Code = "",
                        Description = "",
                        Data = oDato
                    };
                }
                else
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.strMensajeGenerico,
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        protected InfoResponse DeleteEntity(TEntity oDato)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (DeleteWithoutContext(oDato))
                {
                    oResponse = new InfoResponse
                    {
                        Result = true,
                        Code = "",
                        Description = "",
                        Data = null
                    };
                }
                else
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.strMensajeGenerico,
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message); 
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Data = null
                };
                if (ex is SqlException Sql)
                {
                    oResponse.Description = Comun.jsTieneRegistros;
                }
                else
                {
                    oResponse.Description = Comun.strMensajeGenerico;
                }
                
            }
            return oResponse;
        }

        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;
        }

        public string GetGlobalResource(string tag)
        {
            string resource;
            
            try
            {
                if (resourceMan == null)
                {
                    System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Comun", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }

                //object valor = HttpContext.GetGlobalResourceObject(Comun.NOMBRE_FICHERO_RECURSOS, tag, cultureInfo);
                object valor = resourceMan.GetString(tag, cultureInfo);

                if (valor != null)
                {
                    resource = valor.ToString();
                }
                else
                {
                    resource = "";
                }
            }
            catch (Exception)
            {
                resource = "";
            }


            return resource;
        }

    }

    interface IBasica<T> {
        bool RegistroVinculado(long id);
        bool RegistroDefecto(long id);
        bool RegistroDuplicado(string nombre, long clienteID);
    }

    interface IGestionBasica<TEntity> {
        InfoResponse Add(TEntity oEntidad);
        InfoResponse Update(TEntity oEntidad);
        InfoResponse Delete(TEntity oEntidad);
    }
}
