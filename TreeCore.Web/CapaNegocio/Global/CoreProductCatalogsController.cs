using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;
using Ext.Net;
using TreeCore.Clases;
using Newtonsoft.Json;
using System.Globalization;

namespace CapaNegocio
{
    public class CoreProductCatalogsController : GeneralBaseController<CoreProductCatalogs, TreeCoreContext>, IGestionBasica<CoreProductCatalogs>
    {
        public CoreProductCatalogsController()
            : base()
        { }

        public InfoResponse Add(CoreProductCatalogs oCatalogo)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oCatalogo))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.LogRegistroExistente,
                        Data = null
                    };
                }
                else
                {
                    oResponse = AddEntity(oCatalogo);
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

        public InfoResponse Update(CoreProductCatalogs oCatalogo)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oCatalogo))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.LogRegistroExistente,
                        Data = null
                    };
                }
                else
                {
                    oResponse = UpdateEntity(oCatalogo);
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

        public InfoResponse Delete(CoreProductCatalogs oCatalogo)
        {
            InfoResponse oResponse;
            try
            {

                oResponse = DeleteEntity(oCatalogo);

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

        public bool RegistroDuplicado(CoreProductCatalogs oCatalogo)
        {
            bool isExiste = false;
            List<CoreProductCatalogs> datos;

            datos = (from c in Context.CoreProductCatalogs where (c.Codigo == oCatalogo.Codigo && c.ClienteID == oCatalogo.ClienteID && c.CoreProductCatalogID != oCatalogo.CoreProductCatalogID) select c).ToList<CoreProductCatalogs>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public InfoResponse EditarCatalogo(CoreProductCatalogs catalogo, bool control)
        {
            InfoResponse oResponse = new InfoResponse();

            if (oResponse.Result)
            {

                oResponse = Update(catalogo);
                if (oResponse.Result)
                {
                    oResponse = SubmitChanges();
                }
                else
                {
                    DiscardChanges();
                }

            }
            else
            {
                DiscardChanges();
            }

            return oResponse;
        }

        public InfoResponse AnadirCatalogo(CoreProductCatalogs catalogo)
        {
            InfoResponse oResponse = new InfoResponse();

            if (oResponse.Result)
            {

                oResponse = Add(catalogo);
                if (oResponse.Result)
                {
                    oResponse = SubmitChanges();
                }
                else
                {
                    DiscardChanges();
                }

            }
            else
            {
                oResponse = DiscardChanges();
            }

            return oResponse;
        }

        public InfoResponse EliminarCatalogo(CoreProductCatalogs catalogo)
        {
            InfoResponse oResponse = new InfoResponse();

            oResponse = Delete(catalogo);
            if (oResponse.Result)
            {
                oResponse = SubmitChanges();
            }
            else
            {
                DiscardChanges();
            }
            return oResponse;

            if (oResponse.Result)
            {
                oResponse = Delete(catalogo);
                if (oResponse.Result)
                {
                    oResponse = SubmitChanges();
                }
                else
                {
                    DiscardChanges();
                }

            }
            else
            {
                DiscardChanges();
            }

            return oResponse;
        }
        public bool RegistroDuplicadoNombre(string Nombre, long clienteID)
        {
            bool isExiste = false;
            List<CoreProductCatalogs> datos;

            datos = (from c in Context.CoreProductCatalogs where c.Nombre == Nombre && c.ClienteID == clienteID select c).ToList<CoreProductCatalogs>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public bool RegistroDuplicadoCodigo(string Codigo, long clienteID)
        {
            bool isExiste = false;
            List<CoreProductCatalogs> datos;

            datos = (from c in Context.CoreProductCatalogs where c.Codigo == Codigo && c.ClienteID == clienteID select c).ToList<CoreProductCatalogs>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public Vw_CoreProductCatalogs getVistaByID(long lID)
        {
            Vw_CoreProductCatalogs oDato;

            try
            {
                oDato = (from c in Context.Vw_CoreProductCatalogs where c.CoreProductCatalogID == lID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        #region FILTROS, ORDEN Y PAGINACION

        public List<JsonObject> AplicarFiltroInterno(int pageSize, int curPage, out int total, DataSorter[] sorters, string s,
            Hidden hdStringBuscador, Hidden hdIDServicioBuscador, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos)
        {
            total = 0;
            Vw_CoreProductCatalogs oServicio = new Vw_CoreProductCatalogs();
            JsonObject oJson = new JsonObject();
            List<JsonObject> listaServicios = new List<JsonObject>();
            List<Vw_CoreProductCatalogs> listaResultados = new List<Vw_CoreProductCatalogs>();

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDServicioBuscador.IsEmpty) ? Convert.ToInt64(hdIDServicioBuscador.Value) : new long?();

            if (textoBuscado != null || IdBuscado != null)
            {
                if (IdBuscado != null)
                {
                    oServicio = GetItem<Vw_CoreProductCatalogs>("CoreProductCatalogID=" + IdBuscado);
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
                        string sServicio = JsonConvert.SerializeObject(oServicio);
                        oJson = JSON.Deserialize<JsonObject>(sServicio);
                    }

                    listaServicios.Add(oJson);
                }
                else if (textoBuscado != null)
                {
                    listaResultados = getListaByFiltro(textoBuscado);
                }
            }
            else
            {
                listaResultados = GetItemsList<Vw_CoreProductCatalogs>();
            }

            #region DESCARGA

            foreach (Vw_CoreProductCatalogs oDato in listaResultados)
            {
                oJson = new JsonObject();

                if (bDescarga && columnModel != null)
                {
                    for (int i = 0; i < columnModel.Columns.Count; i++)
                    {
                        if (!listaExcluidos.Contains(columnModel.Columns[i].DataIndex))
                        {
                            string sValor = columnModel.Columns[i].DataIndex;
                            System.Reflection.PropertyInfo propiedad = oDato.GetType().GetProperty(sValor);

                            if (propiedad.GetValue(oDato, null) != null)
                            {
                                if ((propiedad.GetValue(oDato, null)).GetType().ToString() == "System.DateTime")
                                {
                                    string sContenido = ((DateTime)(propiedad.GetValue(oDato, null))).ToString(Comun.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                    oJson.Add(columnModel.Columns[i].DataIndex, sContenido);
                                }
                                else
                                {
                                    oJson.Add(columnModel.Columns[i].DataIndex, propiedad.GetValue(oDato, null));
                                }
                            }
                        }
                    }
                }
                else
                {
                    string sServicio = JsonConvert.SerializeObject(oDato);
                    oJson = JSON.Deserialize<JsonObject>(sServicio);
                }

                listaServicios.Add(oJson);
            }

            #endregion

            listaServicios = Filtro(s, listaServicios);
            total = listaServicios.Count;
            listaServicios = PaginacionOrdenacion(listaServicios, sorters, curPage, pageSize);

            return listaServicios;
        }

        public List<JsonObject> AplicarFiltroInternoByEntidad(int pageSize, int curPage, out int total, DataSorter[] sorters, string s,
            Hidden hdStringBuscador, Hidden hdIDServicioBuscador, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos)
        {
            total = 0;
            Vw_CoreProductCatalogs oServicio = new Vw_CoreProductCatalogs();
            JsonObject oJson = new JsonObject();
            List<JsonObject> listaServicios = new List<JsonObject>();
            List<Vw_CoreProductCatalogs> listaResultados = new List<Vw_CoreProductCatalogs>();

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDServicioBuscador.IsEmpty) ? Convert.ToInt64(hdIDServicioBuscador.Value) : new long?();

            if (textoBuscado != null || IdBuscado != null)
            {
                if (IdBuscado != null)
                {
                    oServicio = GetItem<Vw_CoreProductCatalogs>("CoreProductCatalogID=" + IdBuscado);

                    string sServicio = JsonConvert.SerializeObject(oServicio);
                    oJson = JSON.Deserialize<JsonObject>(sServicio);
                    listaServicios.Add(oJson);
                }
                else if (textoBuscado != null)
                {
                    listaResultados = getListaByFiltroByEntidad(textoBuscado);
                }
            }
            else
            {
                listaResultados = GetItemsList<Vw_CoreProductCatalogs>();
            }

            #region DESCARGA

            foreach (Vw_CoreProductCatalogs oDato in listaResultados)
            {
                oJson = new JsonObject();

                if (bDescarga && columnModel != null)
                {
                    for (int i = 0; i < columnModel.Columns.Count; i++)
                    {
                        if (!listaExcluidos.Contains(columnModel.Columns[i].DataIndex))
                        {
                            string sValor = columnModel.Columns[i].DataIndex;
                            System.Reflection.PropertyInfo propiedad = oDato.GetType().GetProperty(sValor);

                            if (propiedad.GetValue(oDato, null) != null)
                            {
                                if ((propiedad.GetValue(oDato, null)).GetType().ToString() == "System.DateTime")
                                {
                                    string sContenido = ((DateTime)(propiedad.GetValue(oDato, null))).ToString(Comun.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                    oJson.Add(columnModel.Columns[i].DataIndex, sContenido);
                                }
                                else
                                {
                                    oJson.Add(columnModel.Columns[i].DataIndex, propiedad.GetValue(oDato, null));
                                }
                            }
                        }
                    }
                }
                else
                {
                    string sServicio = JsonConvert.SerializeObject(oDato);
                    oJson = JSON.Deserialize<JsonObject>(sServicio);
                }

                listaServicios.Add(oJson);
            }

            #endregion

            listaServicios = Filtro(s, listaServicios);
            total = listaServicios.Count;
            listaServicios = PaginacionOrdenacion(listaServicios, sorters, curPage, pageSize);

            return listaServicios;
        }

        public List<Vw_CoreProductCatalogs> getListaByFiltro(string sTexto)
        {
            List<Vw_CoreProductCatalogs> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.Vw_CoreProductCatalogs where c.Codigo.ToString().Contains(sTexto) || c.NombreProductCatalog.ToString().Contains(sTexto) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<Vw_CoreProductCatalogs> getListaByFiltroByEntidad(string sTexto)
        {
            List<Vw_CoreProductCatalogs> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.Vw_CoreProductCatalogs
                              where
(c.Codigo.ToString().Contains(sTexto))
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

        #endregion

    }
}