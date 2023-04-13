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
    public class CoreProductCatalogsPacksController : GeneralBaseController<CoreProductCatalogPacks, TreeCoreContext>, IGestionBasica<CoreProductCatalogPacks>
    {
        public CoreProductCatalogsPacksController()
            : base()
        { }

        public InfoResponse Add(CoreProductCatalogPacks oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (RegistroDuplicado(oEntidad))
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
                    oResponse = AddEntity(oEntidad);
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

        public InfoResponse Update(CoreProductCatalogPacks oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (RegistroDuplicado(oEntidad))
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
                    oResponse = UpdateEntity(oEntidad);
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

        public InfoResponse Delete(CoreProductCatalogPacks oEntidad)
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
        public bool RegistroDuplicado(CoreProductCatalogPacks oBanco)
        {
            bool isExiste = false;
            List<CoreProductCatalogPacks> datos;

            datos = (from c in Context.CoreProductCatalogPacks where (c.Codigo == oBanco.Codigo && c.CoreProductCatalogPackID != oBanco.CoreProductCatalogPackID) select c).ToList<CoreProductCatalogPacks>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public InfoResponse EliminarPacks(CoreProductCatalogPacks pack)
        {
            CoreProductCatalogServiciosPacksAsignadosController cServiciosPacks = new CoreProductCatalogServiciosPacksAsignadosController();
            InfoResponse oResponse = new InfoResponse();
            cServiciosPacks.SetDataContext(this.Context);
            List<CoreProductCatalogServiciosPacksAsignados> lista = new List<CoreProductCatalogServiciosPacksAsignados>();
            lista = cServiciosPacks.GetAllServiciosByPack(pack.CoreProductCatalogPackID);
            if (lista != null)
            {
                foreach (CoreProductCatalogServiciosPacksAsignados oDato in lista)
                {
                    cServiciosPacks.Delete(oDato);
                }
            }


            if (oResponse.Result)
            {
                oResponse = Delete(pack);
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



        public bool RegistroDuplicado(string sCodigo, string sNombre)
        {
            bool isExiste = false;
            List<CoreProductCatalogPacks> listaDatos = new List<CoreProductCatalogPacks>();

            listaDatos = (from c in Context.CoreProductCatalogPacks where (c.Nombre == sNombre || c.Codigo == sCodigo) select c).ToList<CoreProductCatalogPacks>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public bool RegistroDuplicadoNombre(string Nombre)
        {
            bool isExiste = false;
            List<CoreProductCatalogPacks> datos;

            datos = (from c in Context.CoreProductCatalogPacks where c.Nombre == Nombre select c).ToList<CoreProductCatalogPacks>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public bool RegistroDuplicadoCodigo(string Codigo)
        {
            bool isExiste = false;
            List<CoreProductCatalogPacks> datos;

            datos = (from c in Context.CoreProductCatalogPacks where c.Codigo == Codigo select c).ToList<CoreProductCatalogPacks>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }


        #region FILTROS, ORDEN Y PAGINACION

        public List<JsonObject> AplicarFiltroInterno(int pageSize, int curPage, out int total, DataSorter[] sorters, string s,
            Hidden hdStringBuscador, Hidden hdIDServicioBuscador, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos)
        {
            total = 0;
            Vw_CoreProductCatalogPacks oServicio = new Vw_CoreProductCatalogPacks();
            JsonObject oJson = new JsonObject();
            List<JsonObject> listaServicios = new List<JsonObject>();
            List<Vw_CoreProductCatalogPacks> listaResultados = new List<Vw_CoreProductCatalogPacks>();

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDServicioBuscador.IsEmpty) ? Convert.ToInt64(hdIDServicioBuscador.Value) : new long?();

            if (textoBuscado != null || IdBuscado != null)
            {
                if (IdBuscado != null)
                {
                    oServicio = GetItem<Vw_CoreProductCatalogPacks>("CoreProductCatalogPackID=" + IdBuscado);
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
                    listaResultados = getListaByFiltroByServicio(textoBuscado);
                }
            }
            else
            {
                listaResultados = (from c in Context.Vw_CoreProductCatalogPacks select c).OrderBy(x => x.Nombre).ToList();
            }

            #region DESCARGA

            foreach (Vw_CoreProductCatalogPacks oDato in listaResultados)
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
            Hidden hdStringBuscador, Hidden hdIDServicioBuscador, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos, long catalogoID)
        {
            total = 0;
            CoreProductCatalogPacks oServicio = new CoreProductCatalogPacks();
            JsonObject oJson = new JsonObject();
            List<JsonObject> listaServicios = new List<JsonObject>();
            List<CoreProductCatalogPacks> listaResultados = new List<CoreProductCatalogPacks>();

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDServicioBuscador.IsEmpty) ? Convert.ToInt64(hdIDServicioBuscador.Value) : new long?();


            if (textoBuscado != null || IdBuscado != null)
            {
                if (IdBuscado != null)
                {
                    oServicio = GetItem<CoreProductCatalogPacks>("CoreProductCatalogPackID=" + IdBuscado);
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
                //else if (textoBuscado != null)
                //{
                //    listaResultados = getListaByFiltroyCatalogoID(textoBuscado, catalogoID);
                //    foreach (Vw_CoreProductCatalogServiciosAsignados dato in listaResultados)
                //    {
                //        dato.Simbolo = dato.Simbolo + "/" + dato.Identificador;
                //    }
                //}
            }
            //else
            //{
            //    listaResultados = GetItemsList<ProductCatalogServiciosPacks>("ProductCatalogServiciosPacksID=" + catalogoID);
            //    foreach (ProductCatalogServiciosPacks dato in listaResultados)
            //    {
            //        dato.Simbolo = dato.Simbolo + "/" + dato.Identificador;
            //    }
            //}

            #region DESCARGA

            foreach (CoreProductCatalogPacks oDato in listaResultados)
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

        public List<Vw_CoreProductCatalogPacks> AplicarFiltroInternoPack(int pageSize, int curPage, out int total, DataSorter[] sorters, string s,
            Hidden hdStringBuscador, Hidden hdIDServicioBuscador, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos)
        {
            total = 0;
            Vw_CoreProductCatalogPacks oServicio = new Vw_CoreProductCatalogPacks();
            JsonObject oJson = new JsonObject();
            List<JsonObject> listaServicios = new List<JsonObject>();
            List<Vw_CoreProductCatalogPacks> listaResultados = new List<Vw_CoreProductCatalogPacks>();

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDServicioBuscador.IsEmpty) ? Convert.ToInt64(hdIDServicioBuscador.Value) : new long?();

            if (textoBuscado != null || IdBuscado != null)
            {
                if (IdBuscado != null)
                {
                    oServicio = GetItem<Vw_CoreProductCatalogPacks>("CoreProductCatalogPackID=" + IdBuscado);
                    listaResultados.Add(oServicio);
                }
                else if (textoBuscado != null)
                {
                    listaResultados = getListaByFiltroByServicio(textoBuscado);
                }
            }
            else
            {
                listaResultados = GetItemsList<Vw_CoreProductCatalogPacks>();
            }

            listaResultados = listaResultados.OrderBy(c => c.Nombre).ToList();

            return listaResultados;
        }

        public List<Vw_CoreProductCatalogPacks> getListaByFiltroByServicio(string sTexto)
        {
            List<Vw_CoreProductCatalogPacks> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.Vw_CoreProductCatalogPacks
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

        public Vw_CoreProductCatalogPacks getVistaByID(long lID)
        {
            Vw_CoreProductCatalogPacks oDato;

            try
            {
                oDato = (from c in Context.Vw_CoreProductCatalogPacks where c.CoreProductCatalogPackID == lID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }


        #endregion



    }
}