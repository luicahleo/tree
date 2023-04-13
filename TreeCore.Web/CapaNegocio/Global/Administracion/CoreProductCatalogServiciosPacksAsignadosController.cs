using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
using Ext.Net;
using System.IO;
using Tree.Linq.GenericExtensions;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using TreeCore.Clases;
using System.Globalization;
using Newtonsoft.Json;

namespace CapaNegocio
{
    public class CoreProductCatalogServiciosPacksAsignadosController : GeneralBaseController<CoreProductCatalogServiciosPacksAsignados, TreeCoreContext>
    {
        public CoreProductCatalogServiciosPacksAsignadosController()
            : base()
        { }

        public InfoResponse Add(CoreProductCatalogServiciosPacksAsignados oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {


                oResponse = AddEntity(oEntidad);

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

        public InfoResponse Update(CoreProductCatalogServiciosPacksAsignados oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {

                oResponse = UpdateEntity(oEntidad);

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

        public InfoResponse Delete(CoreProductCatalogServiciosPacksAsignados oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {

                oResponse = DeleteEntity(oEntidad);

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

       
        public Vw_CoreProductCatalogServiciosPacksAsignados getItemByServicioID(long lServicioID)
        {
            Vw_CoreProductCatalogServiciosPacksAsignados oDato = null;

            try
            {
                oDato = (from c in Context.Vw_CoreProductCatalogServiciosPacksAsignados
                         where c.CoreProductCatalogServicioID == lServicioID
                         select c).First();
            }
            catch (Exception)
            {
                oDato = null;
            }

            return oDato;
        }

        public List<Vw_CoreProductCatalogServiciosPacksAsignados> getItemsByServicioID(long lServicioID)
        {
            List<Vw_CoreProductCatalogServiciosPacksAsignados> oDato = null;

            try
            {
                oDato = (from c in Context.Vw_CoreProductCatalogServiciosPacksAsignados
                         where c.CoreProductCatalogServicioID == lServicioID
                         select c).ToList();
            }
            catch (Exception ex)
            {
                oDato = null;
            }

            return oDato;
        }

        public List<CoreProductCatalogServiciosPacksAsignados> GetAllServiciosByPack(long packID)
        {
            List<CoreProductCatalogServiciosPacksAsignados> listaDatos = new List<CoreProductCatalogServiciosPacksAsignados>();
            try
            {
                listaDatos = (from c in Context.CoreProductCatalogServiciosPacksAsignados
                              where c.CoreProductCatalogPackID == packID
                              select c).ToList();
            }

            catch (Exception ex)
            {
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<Vw_CoreProductCatalogServiciosPacksAsignados> GetAllServiciosByLongPack(long? packID)
        {
            List<Vw_CoreProductCatalogServiciosPacksAsignados> listaDatos = new List<Vw_CoreProductCatalogServiciosPacksAsignados>();
            try
            {
                if (packID != null)
                {
                    listaDatos = (from c in Context.Vw_CoreProductCatalogServiciosPacksAsignados
                                  where c.CoreProductCatalogPackID == packID
                                  select c).ToList();
                }
            }

            catch (Exception ex)
            {
                listaDatos = null;
            }

            return listaDatos;
        }

        public CoreProductCatalogServiciosPacksAsignados getServicioAsignadoIDByCatalogoIDYServicioID(long lCatalogoID, long servicioID)
        {
            CoreProductCatalogServiciosPacksAsignados oDato = null;

            try
            {
                oDato = (from c in Context.CoreProductCatalogServiciosPacksAsignados
                         where c.CoreProductCatalogPackID == lCatalogoID && c.CoreProductCatalogServicioID == servicioID
                         select c).First();
            }
            catch (Exception)
            {
                oDato = null;
            }

            return oDato;
        }

        public List<Vw_CoreProductCatalogServiciosPacksAsignados> GetAllServiciosByPackID(long catalogoID)
        {
            List<Vw_CoreProductCatalogServiciosPacksAsignados> listaDatos = new List<Vw_CoreProductCatalogServiciosPacksAsignados>();
            try
            {
                DataTable result;
                #region CADENA CONEXIÓN
#if SERVICESETTINGS
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string connectionString = TreeAPI.Properties.Settings.Default.Conexion;
#else
                string connectionString = TreeCore.Properties.Settings.Default.Conexion;
#endif
                #endregion
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("sp_CoreProductCatalogServiciosByProductCatalogID", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@ProductCatalogID", catalogoID);

                    var da = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    da.Fill(ds);
                    result = ds.Tables[0];
                }
                Vw_CoreProductCatalogServiciosPacksAsignados d;
                foreach (DataRow row in result.Rows)
                {
                    d = new Vw_CoreProductCatalogServiciosPacksAsignados();
                    d.CoreProductCatalogServicioID = (long)row[0];
                    if (row[2].GetType().Name != "DBNull")
                    {
                        d.NombreProductCatalogServicio = (string)row[2];
                    }
                    //if (row[3].GetType().Name != "DBNull")
                    //{
                    //    d.CantidadCatalogServicio = (double)row[3];
                    //}
                    if (row[4].GetType().Name != "DBNull")
                    {
                        d.CoreProductCatalogServicioTipoID = (long)row[4];

                    }
                    if (row[5].GetType().Name != "DBNull")
                    {
                        d.CoreProductCatalogServicioID = (long)row[5];
                    }
                    //if (row[6].GetType().Name != "DBNull")
                    //{
                    //    d.Precio = (double)row[6];
                    //}
                    if (row[7].GetType().Name != "DBNull")
                    {
                        d.NombreProductCatalogServicioTipo = (string)row[7];
                    }

                    listaDatos.Add(d);
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
            }

            return listaDatos;
        }


        public List<Vw_CoreProductCatalogServiciosPacksAsignados> AplicarFiltroInternoFormulario(int pageSize, int curPage, out int total, DataSorter[] sorters, string s,
            Hidden hdStringBuscador, Hidden hdIDServicioBuscador, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos, long catalogoID)
        {

            total = 0;
            Vw_CoreProductCatalogServiciosPacksAsignados oServicio = new Vw_CoreProductCatalogServiciosPacksAsignados();
            JsonObject oJson = new JsonObject();
            List<JsonObject> listaServicios = new List<JsonObject>();
            List<Vw_CoreProductCatalogServiciosPacksAsignados> listaResultados = new List<Vw_CoreProductCatalogServiciosPacksAsignados>();

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDServicioBuscador.IsEmpty) ? Convert.ToInt64(hdIDServicioBuscador.Value) : new long?();

            if (textoBuscado != null || IdBuscado != null)
            {
                if (IdBuscado != null)
                {
                    oServicio = GetItem<Vw_CoreProductCatalogServiciosPacksAsignados>("CoreProductCatalogServicioID=" + IdBuscado);
                    oJson = new JsonObject();

                    if (bDescarga && columnModel != null)
                    {
                        for (int i = 0; i < columnModel.Columns.Count; i++)
                        {
                            if (!listaExcluidos.Contains(columnModel.Columns[i].DataIndex))
                            {
                                string sValor = columnModel.Columns[i].DataIndex;
                                System.Reflection.PropertyInfo propiedad = oServicio.GetType().GetProperty(sValor);

                                if (propiedad.GetValue(oServicio, null) != null)
                                {
                                    if ((propiedad.GetValue(oServicio, null)).GetType().ToString() == "System.DateTime")
                                    {
                                        string sContenido = ((DateTime)(propiedad.GetValue(oServicio, null))).ToString(Comun.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                        oJson.Add(columnModel.Columns[i].DataIndex, sContenido);
                                    }
                                    else
                                    {
                                        oJson.Add(columnModel.Columns[i].DataIndex, propiedad.GetValue(oServicio, null));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                    }

                    listaResultados.Add(oServicio);
                }
                else if (textoBuscado != null)
                {
                    listaResultados = getListaByFiltro(textoBuscado);
                }
            }
            else
            {
                listaResultados = GetItemsList<Vw_CoreProductCatalogServiciosPacksAsignados>();
                listaResultados = GetAllServiciosByPackID(catalogoID);
                listaResultados = listaResultados.OrderBy(c => c.NombreProductCatalogServicio).OrderBy(c => c.NombreProductCatalogServicioTipo).ToList();
            }



            total = listaServicios.Count;

            return listaResultados;


        }
        public List<Vw_CoreProductCatalogServiciosPacksAsignados> getListaByFiltro(string sTexto)
        {
            List<Vw_CoreProductCatalogServiciosPacksAsignados> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.Vw_CoreProductCatalogServiciosPacksAsignados where c.CodigoProductCatalogServicio.ToString().Contains(sTexto) || c.NombreProductCatalogServicio.ToString().Contains(sTexto) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<Vw_CoreProductCatalogServiciosPacksAsignados> AplicarFiltroInternoByPackID(int pageSize, int curPage, out int total, DataSorter[] sorters, string s,
            Hidden hdStringBuscador, Hidden hdIDServicioBuscador, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos, Hidden hdPack)
        {
            total = 0;
            Vw_CoreProductCatalogServiciosPacksAsignados oServicio = new Vw_CoreProductCatalogServiciosPacksAsignados();
            JsonObject oJson = new JsonObject();
            List<JsonObject> listaServicios = new List<JsonObject>();
            List<Vw_CoreProductCatalogServiciosPacksAsignados> listaResultados = new List<Vw_CoreProductCatalogServiciosPacksAsignados>();

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDServicioBuscador.IsEmpty) ? Convert.ToInt64(hdIDServicioBuscador.Value) : new long?();
            long? IdPack = (!hdPack.IsEmpty) ? Convert.ToInt64(hdPack.Value) : new long?();

            if (textoBuscado != null || IdBuscado != null)
            {
                if (IdBuscado != null)
                {
                    oServicio = GetItem<Vw_CoreProductCatalogServiciosPacksAsignados>("CoreProductCatalogServicioPackAsignadoID=" + IdBuscado);
                    listaResultados.Add(oServicio);
                }
                else if (textoBuscado != null)
                {
                    listaResultados = getListaByFiltroByServicio(textoBuscado);
                }
            }
            else if (IdPack != null)
            {
                listaResultados = GetAllServiciosByLongPack(IdPack);
            }

            listaResultados = listaResultados.OrderBy(c => c.NombreProductCatalogServicio).ToList();

            return listaResultados;
        }

        public List<Vw_CoreProductCatalogServiciosPacksAsignados> getListaByFiltroByServicio(string sTexto)
        {
            List<Vw_CoreProductCatalogServiciosPacksAsignados> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.Vw_CoreProductCatalogServiciosPacksAsignados
                              where c.NombreProductCatalogServicio.ToString().Contains(sTexto)
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        private static List<JsonObject> Filtro(string s, List<JsonObject> lista)
        {
            if (!string.IsNullOrEmpty(s))
            {
                lista = LinqEngine.filtroJson(lista, s);
            }

            return lista;
        }

        private List<JsonObject> PaginacionOrdenacion(List<JsonObject> lista, DataSorter[] sorters, int curPage, int pageSize)
        {

            if (sorters != null)
            {
                lista = LinqEngine.SortJson(lista, sorters);
            }

            if (curPage != -1 && pageSize != -1)
            {
                lista = lista.Skip(curPage * pageSize).Take(pageSize).ToList();
            }

            return lista;
        }

        public List<long> getAllServiciosByID(long lPackID)
        {
            List<long> listaIDs;

            try
            {
                listaIDs = (from c in Context.CoreProductCatalogServiciosPacksAsignados where c.CoreProductCatalogPackID == lPackID select c.CoreProductCatalogServicioID).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaIDs = null;
            }

            return listaIDs;
        }

        public long getValorByValoresID(long lServicioID, long lPackID)
        {
            long lServicioAsignID;

            try
            {
                lServicioAsignID = (from c in Context.CoreProductCatalogServiciosPacksAsignados where c.CoreProductCatalogServicioID == lServicioID && c.CoreProductCatalogPackID == lPackID select c.CoreProductCatalogServicioPackAsignadoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lServicioAsignID = 0;
            }

            return lServicioAsignID;
        }

    }

}
