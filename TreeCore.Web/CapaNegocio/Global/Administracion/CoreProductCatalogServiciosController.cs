using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;
using Newtonsoft.Json;
using System.Globalization;
using Tree.Linq.GenericExtensions;
using TreeCore.Clases;
using Ext.Net;

namespace CapaNegocio
{
    public class CoreProductCatalogServiciosController : GeneralBaseController<CoreProductCatalogServicios, TreeCoreContext>, IGestionBasica<CoreProductCatalogServicios>
    {
        public CoreProductCatalogServiciosController()
            : base()
        { }

        private const string operatorAND = " AND ";

        public InfoResponse Add(CoreProductCatalogServicios oServicio)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicado(oServicio))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "Codigo",
                        Description = Comun.LogRegistroExistente,
                        Data = null
                    };
                }
                else
                {
                    oResponse = AddEntity(oServicio);
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

        public InfoResponse Update(CoreProductCatalogServicios oServicio)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDuplicado(oServicio))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "Codigo",
                        Description = Comun.LogRegistroExistente,
                        Data = null
                    };
                }
                else
                {
                    oResponse = UpdateEntity(oServicio);
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

        public InfoResponse Delete(CoreProductCatalogServicios oServicio)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = DeleteEntity(oServicio);
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

        public bool RegistroDuplicado(CoreProductCatalogServicios oServicio)
        {
            bool isExiste = false;
            List<CoreProductCatalogServicios> datos;

            datos = (from c in Context.CoreProductCatalogServicios where (c.Codigo == oServicio.Codigo && c.CoreProductCatalogServicioID != oServicio.CoreProductCatalogServicioID) select c).ToList<CoreProductCatalogServicios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        //public string getIdentificador(long lServicioID)
        //{
        //    string sIdentificador = null;

        //    try
        //    {
        //        sIdentificador = (from c in Context.Vw_CoreProductCatalogServicios where c.CoreProductCatalogServicioID == lServicioID select c.Identificador).First();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        sIdentificador = null;
        //    }

        //    return sIdentificador;
        //}

        public List<Vw_CoreProductCatalogServicios> getListaByFiltro(string sTexto)
        {
            List<Vw_CoreProductCatalogServicios> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.Vw_CoreProductCatalogServicios
                              where c.Codigo.Contains(sTexto) || c.Nombre.Contains(sTexto)
                              select c).OrderBy(x => x.Nombre).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }
        public List<Vw_CoreProductCatalogServiciosAsignados> getListaByFiltroyCatalogoID(string sTexto, long catalogoID)
        {
            List<Vw_CoreProductCatalogServiciosAsignados> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.Vw_CoreProductCatalogServiciosAsignados where (c.CodigoProductCatalog.ToString().Contains(sTexto) || c.NombreCatalogServicio.ToString().Contains(sTexto)) && c.CoreProductCatalogID == catalogoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }
        public List<Vw_CoreProductCatalogServiciosPacksAsignados> getListaByFiltroyPackID(string sTexto, long packID)
        {
            List<Vw_CoreProductCatalogServiciosPacksAsignados> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.Vw_CoreProductCatalogServiciosPacksAsignados where (c.NombreProductCatalogServicio.ToString().Contains(sTexto) || c.Codigo.ToString().Contains(sTexto)) && c.CoreProductCatalogPackID == packID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public bool CodigoDuplicadoGeneradorCodigos(string codigo)
        {
            List<Vw_CoreProductCatalogServicios> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_CoreProductCatalogServicios where c.Codigo == codigo select c).ToList();

                if (listaDatos.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return true;
            }
        }

        public CoreProductCatalogServicios getServicioByNombre(string sNombre)
        {
            CoreProductCatalogServicios oDato = null;

            try
            {
                oDato = (from c in Context.CoreProductCatalogServicios where c.Nombre == sNombre select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

            }

            return oDato;
        }

        #region FILTROS, ORDEN Y PAGINACION

        public List<JsonObject> AplicarFiltroInterno(int pageSize, int curPage, out int total, DataSorter[] sorters, string s,
            Hidden hdStringBuscador, Hidden hdIDServicioBuscador, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos)
        {
            total = 0;
            Vw_CoreProductCatalogServicios oServicio = new Vw_CoreProductCatalogServicios();
            JsonObject oJson = new JsonObject();
            List<JsonObject> listaServicios = new List<JsonObject>();
            List<Vw_CoreProductCatalogServicios> listaResultados = new List<Vw_CoreProductCatalogServicios>();

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDServicioBuscador.IsEmpty) ? Convert.ToInt64(hdIDServicioBuscador.Value) : new long?();

            if (textoBuscado != null || IdBuscado != null)
            {
                if (IdBuscado != null)
                {
                    oServicio = GetItem<Vw_CoreProductCatalogServicios>("CoreProductCatalogServicioID=" + IdBuscado);
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
                            else
                            {
                                oJson.Add(columnModel.Columns[i].DataIndex, null);
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
                listaResultados = (from c in Context.Vw_CoreProductCatalogServicios select c).OrderBy(x => x.Nombre).ToList();
            }

            #region DESCARGA

            foreach (Vw_CoreProductCatalogServicios oDato in listaResultados)
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
                            else
                            {
                                oJson.Add(columnModel.Columns[i].DataIndex, null);
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
        public List<JsonObject> AplicarFiltroInternoByCatalogoID(int pageSize, int curPage, out int total, DataSorter[] sorters, string s,
            Hidden hdStringBuscador, Hidden hdIDServicioBuscador, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos, long packID)
        {
            total = 0;
            Vw_CoreProductCatalogServiciosAsignados oServicio = new Vw_CoreProductCatalogServiciosAsignados();
            JsonObject oJson = new JsonObject();
            List<JsonObject> listaServicios = new List<JsonObject>();
            List<Vw_CoreProductCatalogServiciosAsignados> listaResultados = new List<Vw_CoreProductCatalogServiciosAsignados>();

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDServicioBuscador.IsEmpty) ? Convert.ToInt64(hdIDServicioBuscador.Value) : new long?();


            if (textoBuscado != null || IdBuscado != null)
            {
                if (IdBuscado != null)
                {
                    oServicio = GetItem<Vw_CoreProductCatalogServiciosAsignados>("CoreProductCatalogServicioID=" + IdBuscado);

                    if (oServicio != null)
                    {
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
                            //oServicio.Simbolo = oServicio.Simbolo + "/" + oServicio.Identificador;
                            string sServicio = JsonConvert.SerializeObject(oServicio);
                            oJson = JSON.Deserialize<JsonObject>(sServicio);

                        }

                        listaServicios.Add(oJson);
                    }
                }
                else if (textoBuscado != null)
                {
                    listaResultados = getListaByFiltroyCatalogoID(textoBuscado, packID);
                    foreach (Vw_CoreProductCatalogServiciosAsignados dato in listaResultados)
                    {
                        //dato.Simbolo = dato.Simbolo + "/" + dato.Identificador;
                    }
                }
            }
            else
            {
                listaResultados = GetItemsList<Vw_CoreProductCatalogServiciosAsignados>("CoreProductCatalogID=" + packID);
                foreach (Vw_CoreProductCatalogServiciosAsignados dato in listaResultados)
                {
                    //dato.Simbolo = dato.Simbolo + "/" + dato.Identificador;
                }
            }

            #region DESCARGA

            foreach (Vw_CoreProductCatalogServiciosAsignados oDato in listaResultados)
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

        public List<JsonObject> AplicarFiltroInternoByPackID(int pageSize, int curPage, out int total, DataSorter[] sorters, string s,
            Hidden hdStringBuscador, Hidden hdIDServicioBuscador, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos, long packID)
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
                    oServicio = GetItem<Vw_CoreProductCatalogServiciosPacksAsignados>("!CoreProductCatalogServicioID=" + IdBuscado);
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
                        //oServicio.IdentificadorProductCatalog = oServicio.IdentificadorProductCatalog + "/" + oServicio.IdentificadorProductCatalogServicio;
                        string sServicio = JsonConvert.SerializeObject(oServicio);
                        oJson = JSON.Deserialize<JsonObject>(sServicio);

                    }

                    listaServicios.Add(oJson);
                }
                else if (textoBuscado != null)
                {
                    listaResultados = getListaByFiltroyPackID(textoBuscado, packID);
                    foreach (Vw_CoreProductCatalogServiciosPacksAsignados dato in listaResultados)
                    {
                        //dato.IdentificadorProductCatalog = dato.IdentificadorProductCatalog + "/" + dato.IdentificadorProductCatalogServicio;
                    }
                }
            }
            else
            {
                listaResultados = GetItemsList<Vw_CoreProductCatalogServiciosPacksAsignados>("CoreProductCatalogPackID=" + packID);
                foreach (Vw_CoreProductCatalogServiciosPacksAsignados dato in listaResultados)
                {
                    //dato.IdentificadorProductCatalog = dato.IdentificadorProductCatalog + "/" + dato.IdentificadorProductCatalogServicio;
                }
            }

            #region DESCARGA

            foreach (Vw_CoreProductCatalogServiciosPacksAsignados oDato in listaResultados)
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

        public List<Vw_CoreProductCatalogServicios> AplicarFiltroInternoCatalogo(int pageSize, int curPage, out int total, DataSorter[] sorters, string s,
            Hidden hdStringBuscador, Hidden hdIDServicioBuscador, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos)
        {
            total = 0;
            Vw_CoreProductCatalogServicios oServicio = new Vw_CoreProductCatalogServicios();
            JsonObject oJson = new JsonObject();
            List<JsonObject> listaServicios = new List<JsonObject>();
            List<Vw_CoreProductCatalogServicios> listaResultados = new List<Vw_CoreProductCatalogServicios>();

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDServicioBuscador.IsEmpty) ? Convert.ToInt64(hdIDServicioBuscador.Value) : new long?();

            if (textoBuscado != null || IdBuscado != null)
            {
                if (IdBuscado != null)
                {
                    oServicio = GetItem<Vw_CoreProductCatalogServicios>("CoreProductCatalogServicioID=" + IdBuscado);
                    listaResultados.Add(oServicio);
                }
                else if (textoBuscado != null)
                {
                    listaResultados = getListaByFiltroByServicio(textoBuscado);
                }
            }
            else
            {
                listaResultados = GetItemsList<Vw_CoreProductCatalogServicios>();
            }

            listaResultados = listaResultados.OrderBy(c => c.Nombre).OrderBy(c => c.CodigoTipo).ToList();

            return listaResultados;
        }

        public List<Vw_CoreProductCatalogServicios> AplicarFiltroInternoPack(int pageSize, int curPage, out int total, DataSorter[] sorters, string s,
            Hidden hdStringBuscador, Hidden hdIDServicioBuscador, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos)
        {
            total = 0;
            Vw_CoreProductCatalogServicios oServicio = new Vw_CoreProductCatalogServicios();
            JsonObject oJson = new JsonObject();
            List<JsonObject> listaServicios = new List<JsonObject>();
            List<Vw_CoreProductCatalogServicios> listaResultados = new List<Vw_CoreProductCatalogServicios>();

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDServicioBuscador.IsEmpty) ? Convert.ToInt64(hdIDServicioBuscador.Value) : new long?();

            if (textoBuscado != null || IdBuscado != null)
            {
                if (IdBuscado != null)
                {
                    oServicio = GetItem<Vw_CoreProductCatalogServicios>("CoreProductCatalogServicioID=" + IdBuscado);
                    listaResultados.Add(oServicio);
                }
                else if (textoBuscado != null)
                {
                    listaResultados = getListaByFiltroByServicio(textoBuscado);
                }
            }
            else
            {
                listaResultados = GetItemsList<Vw_CoreProductCatalogServicios>();
            }

            listaResultados = listaResultados.OrderBy(c => c.Nombre).OrderBy(c => c.CodigoTipo).ToList();

            return listaResultados;
        }

        public List<Vw_CoreProductCatalogServicios> getListaByFiltroByServicio(string sTexto)
        {
            List<Vw_CoreProductCatalogServicios> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.Vw_CoreProductCatalogServicios
                              where c.Nombre.ToString().Contains(sTexto)
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion
    }
}

