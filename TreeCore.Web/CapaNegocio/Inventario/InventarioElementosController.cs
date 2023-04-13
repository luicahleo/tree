using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using TreeCore.Data;
using Ext.Net;
using Tree.Linq.GenericExtensions;
using Newtonsoft.Json.Linq;
using TreeCore.Clases;
using System.Globalization;


namespace CapaNegocio
{
    public class InventarioElementosController : GeneralBaseController<InventarioElementos, TreeCoreContext>, IGestionBasica<InventarioElementos>
    {
        public InventarioElementosController()
            : base()
        { }

        public bool RegistroDuplicado(InventarioElementos oEntidad)
        {
            bool isExiste = false;
            List<Vw_InventarioElementosReducida> datos;

            datos = (from c in Context.Vw_InventarioElementosReducida where (c.NumeroInventario == oEntidad.NumeroInventario && c.ClienteID == oEntidad.Emplazamientos.ClienteID && c.InventarioElementoID != oEntidad.InventarioElementoID) select c).ToList();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public InfoResponse Add(InventarioElementos oEntidad)
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
                        Description = GetGlobalResource(Comun.LogRegistroExistente),
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
                    Description = GetGlobalResource(Comun.strMensajeGenerico),
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Update(InventarioElementos oEntidad)
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
                        Description = GetGlobalResource(Comun.LogRegistroExistente),
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
                    Description = GetGlobalResource(Comun.strMensajeGenerico),
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Delete(InventarioElementos oEntidad)
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
                    Description = GetGlobalResource(Comun.strMensajeGenerico),
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse AddUpdateInventarioElemento(InventarioElementos oEntidad,
            List<object> listaAtributos,
            List<object> listaPlantillas,
            JsonObject listasCargadas = null,
            List<long> listasObligatorias = null,
            JsonObject listaRestricciones = null,
            JsonObject listaAtributosCar = null,
            List<InventarioElementosAtributosJson> listaAtributosPlantillas = null)
        {
            CoreInventarioElementosAtributosController cAtributos = new CoreInventarioElementosAtributosController();
            cAtributos.SetDataContext(Context);
            CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
            cAtributosConf.SetDataContext(Context);
            CoreInventarioElementosPlantillasAtributosCategoriasAtributosController cPlantillas = new CoreInventarioElementosPlantillasAtributosCategoriasAtributosController();
            cPlantillas.SetDataContext(Context);
            InventarioElementosHistoricosController cHistorico = new InventarioElementosHistoricosController();
            cHistorico.SetDataContext(Context);
            InventarioElementosAtributosJsonController cAtrJson = new InventarioElementosAtributosJsonController();
            InventarioElementosPlantillasJsonController cPlaJson = new InventarioElementosPlantillasJsonController();

            InfoResponse oResponse = new InfoResponse();
            try
            {
                #region LISTAS

                JsonObject listas;
                JsonObject listasHistoricos = new JsonObject();
                foreach (var oAtr in listaAtributos)
                {
                    var AtributoID = oAtr.GetType().GetProperty("AtributoID");
                    var NombreAtributo = oAtr.GetType().GetProperty("NombreAtributo");
                    var Valor = oAtr.GetType().GetProperty("Valor");
                    var TipoDato = oAtr.GetType().GetProperty("TipoDato");
                    if (TipoDato.GetValue(oAtr).ToString() == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE || TipoDato.GetValue(oAtr).ToString() == Comun.TIPODATO_CODIGO_LISTA)
                    {
                        listasHistoricos.Add(NombreAtributo.GetValue(oAtr).ToString(), cAtributosConf.GetJsonItems(long.Parse(AtributoID.GetValue(oAtr).ToString())));
                    }
                }
                if (listasCargadas != null)
                {
                    listas = listasCargadas;
                }
                else
                {
                    listas = listasHistoricos;
                }

                #endregion

                #region ATRIBUTOS

                List<InventarioElementosAtributosJson> listaAtr = new List<InventarioElementosAtributosJson>();
                InventarioElementosAtributosJson oEleAtr;

                foreach (var oAtr in listaAtributos)
                {
                    var AtributoID = oAtr.GetType().GetProperty("AtributoID");
                    var NombreAtributo = oAtr.GetType().GetProperty("NombreAtributo");
                    var Valor = oAtr.GetType().GetProperty("Valor");
                    var TipoDato = oAtr.GetType().GetProperty("TipoDato");
                    string descError = "";
                    oEleAtr = cAtributos.SaveUpdateAtributo(out descError, oEntidad.InventarioCategoriaID,
                        long.Parse(AtributoID.GetValue(oAtr).ToString()),
                        Valor.GetValue(oAtr).ToString(),
                        listas,
                        listaRestricciones,
                        listaAtributosCar);
                    if (oEleAtr == null)
                    {
                        oResponse = new InfoResponse
                        {
                            Result = false,
                            Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ATRIBUTTE_INVALID_FORMAT_CODE,
                            Description = descError,
                            Data = null
                        };
                        return oResponse;
                    }
                    else
                    {
                        listaAtr.Add(oEleAtr);
                    }
                }

                if (listaAtributosPlantillas != null && listaAtributosPlantillas.Count > 0)
                {
                    listaAtr.AddRange(listaAtributosPlantillas);
                }


                #endregion

                #region PLANTILLAS

                List<InventarioElementosPlantillasJson> listaPlaJson = new List<InventarioElementosPlantillasJson>();
                bool Obli;

                if (listaPlantillas != null)
                {
                    listaPlaJson = cPlantillas.AplicarPlantillas(listaPlantillas, oEntidad.InventarioCategoriaID, out Obli, listasObligatorias);
                    if (!Obli)
                    {
                        oResponse = new InfoResponse
                        {
                            Result = false,
                            Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_REQUIRED_TEMPLATE_CODE,
#if SERVICESETTINGS
                            Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_REQUIRED_TEMPLATE_CODE,
#elif TREEAPI
                            Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_REQUIRED_TEMPLATE_CODE,
#else
                            Description = GetGlobalResource(Comun.strMensajeGenerico),
#endif
                            Data = null
                        };
                        return oResponse;
                    }
                }

                #endregion

                if (oEntidad.InventarioElementoID == 0)
                {
                    oEntidad.JsonAtributosDinamicos = cAtrJson.Serializacion(listaAtr);
                    oEntidad.JsonPlantillas = cPlaJson.Serializacion(listaPlaJson);
                    oResponse = Add(oEntidad);
                }
                else
                {
                    var oElementoHistorico = GetItem(oEntidad.InventarioElementoID);
                    if (!cHistorico.CrearHistorico(oElementoHistorico))
                    {
                        oResponse = new InfoResponse
                        {
                            Result = false,
                            Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
#if SERVICESETTINGS
                            Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION,
#elif TREEAPI
                            Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION,
#else
                            Description = GetGlobalResource(Comun.strMensajeGenerico),
#endif
                            Data = null
                        };
                        return oResponse;
                    }
                    oEntidad.JsonAtributosDinamicos = cAtrJson.Serializacion(listaAtr.Concat(cAtrJson.Deserializacion(oEntidad.JsonAtributosDinamicos)).Distinct(new InventarioElementosAtributosJsonComparer()).ToList());
                    oEntidad.JsonPlantillas = cPlaJson.Serializacion(listaPlaJson.Concat(cPlaJson.Deserializacion(oEntidad.JsonPlantillas)).Distinct(new InventarioElementosPlantillasJsonComparer()).ToList());
                    oResponse = Update(oEntidad);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
#if SERVICESETTINGS
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION,
#elif TREEAPI
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION,
#else
                    Description = GetGlobalResource(Comun.strMensajeGenerico),
#endif
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse MoverElementos(List<InventarioElementos> listaElementos, Emplazamientos oEmplazamiento)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                InventarioElementosVinculacionesController cVinculaciones = new InventarioElementosVinculacionesController();
                cVinculaciones.SetDataContext(Context);
                foreach (var Inv in listaElementos)
                {
                    if (cVinculaciones.GetVinculacionesFromElemento(Inv.InventarioElementoID).Count > 0)
                    {
                        oResponse = new InfoResponse
                        {
                            Result = false,
                            Code = "",
                            Description = GetGlobalResource("jsTieneVinculaciones"),
                            Data = null
                        };
                        return oResponse;
                    }
                }
                foreach (var Inv in listaElementos)
                {
                    Inv.EmplazamientoID = oEmplazamiento.EmplazamientoID;
                    if (!(oResponse = Update(Inv)).Result)
                    {
                        return oResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = GetGlobalResource(Comun.strMensajeGenerico),
                    Data = null
                };
            }
            return oResponse;
        }

        public InventarioElementos GetElementoByCodigo(string sCodigo, long lClienteID)
        {
            InventarioElementos ElementoID = null;
            List<InventarioElementos> datos = new List<InventarioElementos>();

            datos = (from c in Context.InventarioElementos
                     join
cat in Context.InventarioCategorias on c.InventarioCategoriaID equals cat.InventarioCategoriaID
                     where c.NumeroInventario == sCodigo && cat.ClienteID == lClienteID
                     select c).ToList();

            if (datos.Count > 0)
            {
                ElementoID = datos[0];
            }

            return ElementoID;
        }
        public List<InventarioElementos> GetElementoByCategoria(long lCategoriaID)
        {
            List<InventarioElementos> lista = new List<InventarioElementos>();
            try
            {
                lista = (from c in Context.InventarioElementos
                         where c.InventarioCategoriaID == lCategoriaID
                         select c).ToList();
            }
            catch (Exception ex)
            {
                lista = null;
                log.Error(ex.Message);
            }
            return lista;
        }
        public Vw_InventarioElementosReducida GetElementoVistaReducida(long idElemento)
        {
            try
            {
                return (from c in Context.Vw_InventarioElementosReducida where c.InventarioElementoID == idElemento select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public List<Vw_InventarioElementosReducida> GetInventarioElementosByEmplazamientoFiltroCategorias(long lEmplazamientoID, long lElementoID, string sFiltroCategorias)
        {
            List<Vw_InventarioElementosReducida> lista;

            try
            {
                if (lElementoID != 0)
                {
                    if (sFiltroCategorias == "")
                    {
                        lista = (from c in Context.Vw_InventarioElementosReducida where c.Activo == true && c.EmplazamientoID == lEmplazamientoID && c.InventarioElementoID != lElementoID select c).ToList();
                    }
                    else
                    {
                        string[] lCategorias = sFiltroCategorias.Split(';');
                        lista = (from c in Context.Vw_InventarioElementosReducida where c.Activo == true && c.EmplazamientoID == lEmplazamientoID && c.InventarioElementoID != lElementoID && lCategorias.Contains(c.InventarioCategoria) select c).ToList();
                    }
                }
                else
                {
                    if (sFiltroCategorias == "")
                    {
                        lista = (from c in Context.Vw_InventarioElementosReducida where c.Activo == true && c.EmplazamientoID == lEmplazamientoID && c.InventarioElementoID != lElementoID select c).ToList();
                    }
                    else
                    {
                        string[] lCategorias = sFiltroCategorias.Split(';');
                        lista = (from c in Context.Vw_InventarioElementosReducida where c.Activo == true && c.EmplazamientoID == lEmplazamientoID && lCategorias.Contains(c.InventarioCategoria) select c).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                lista = null;
                log.Error(ex.Message);
            }

            return lista;
        }

        public List<Vw_CoreElementosVinculacionesReducida> GetVinculaciones(long clienteID)
        {
            List<Vw_CoreElementosVinculacionesReducida> lista;

            try
            {
                lista = (from c in Context.Vw_CoreElementosVinculacionesReducida
                         where c.ClienteID == clienteID
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }
        public List<Vw_InventarioElementosReducida> GetVwByEmplazamientoID(long emplazamientoID)
        {
            List<Vw_InventarioElementosReducida> inventario;
            try
            {
                inventario = (from c
                         in Context.Vw_InventarioElementosReducida
                              where c.EmplazamientoID == emplazamientoID
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                inventario = null;
            }
            return inventario;
        }

        public List<InventarioElementos> GetElementosByEmplazamientoID(long emplazamientoID)
        {
            List<InventarioElementos> inventario;
            try
            {
                inventario = (from c
                         in Context.InventarioElementos
                              where c.EmplazamientoID == emplazamientoID
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                inventario = null;
            }
            return inventario;
        }

        public List<Vw_CoreInventarioElementos> GetVWElementosByEmplazamientoID(long emplazamientoID)
        {
            List<Vw_CoreInventarioElementos> inventario;
            try
            {
                inventario = (from c
                         in Context.Vw_CoreInventarioElementos
                              where c.EmplazamientoID == emplazamientoID
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                inventario = null;
            }
            return inventario;
        }


        #region INICIO INVENTARIO
        public List<ItemInvInicio> GetElementosByTopCategorias(int top, List<long> categoriasValidas)
        {
            List<ItemInvInicio> inventario;
            List<ItemInvInicio> categoriasTop;

            try
            {
                if (categoriasValidas.Count > 0)
                {
                    categoriasTop = new List<ItemInvInicio>();
                    categoriasValidas.ForEach(c =>
                    {
                        categoriasTop.Add(new ItemInvInicio()
                        {
                            InventarioCategoriaID = c
                        });
                    });
                }
                else
                {
                    categoriasTop = GetTopCategorias(top);
                }

                List<long> listIDCategories = new List<long>();
                categoriasTop.ForEach(x => listIDCategories.Add(x.InventarioCategoriaID));

                inventario = (from c in Context.InventarioElementos
                              join cat in Context.InventarioCategorias on c.InventarioCategoriaID equals cat.InventarioCategoriaID
                              where listIDCategories.Contains(c.InventarioCategoriaID)
                              select new ItemInvInicio()
                              {
                                  InventarioElementoID = c.InventarioElementoID,
                                  Nombre = c.Nombre,
                                  InventarioCategoria = cat.InventarioCategoria,
                                  InventarioCategoriaID = cat.InventarioCategoriaID,
                                  FechaCreacion = c.FechaCreacion
                              }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                inventario = null;
            }
            return inventario;
        }

        public List<ItemInvInicio> GetElementosByTopOperadores(int top, List<long> operadoresValidos)
        {
            List<ItemInvInicio> operadoresTop;

            try
            {
                IQueryable<ItemInvInicio> query = (from c in Context.InventarioElementos
                                                   join op in Context.Operadores on c.OperadorID equals op.OperadorID
                                                   group op by op.OperadorID into g
                                                   select new ItemInvInicio()
                                                   {
                                                       OperadorID = g.First().OperadorID,
                                                       Operador = g.First().Operador,
                                                       ocurrencies = g.Count()
                                                   }).OrderByDescending(x => x.ocurrencies);

                if (operadoresValidos.Count() > 0)
                {
                    query = query.Where(x => operadoresValidos.Contains(x.OperadorID));
                }

                operadoresTop = query.Take(top).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                operadoresTop = null;
            }
            return operadoresTop;
        }

        public List<ItemInvInicio> GetElementosByTopEstadosOperacionales(int top, List<long> estadosValidos)
        {
            List<ItemInvInicio> estadosOperacionalesTop;

            try
            {
                IQueryable<ItemInvInicio> query = (from c in Context.InventarioElementos
                                                   join op in Context.InventarioElementosAtributosEstados on c.InventarioElementoAtributoEstadoID equals op.InventarioElementoAtributoEstadoID
                                                   group op by op.InventarioElementoAtributoEstadoID into g
                                                   select new ItemInvInicio()
                                                   {
                                                       InventarioElementoAtributoEstado = g.First().InventarioElementoAtributoEstadoID,
                                                       NombreAtributoEstado = g.First().Nombre,
                                                       CodigoAtributoEstado = g.First().Codigo,
                                                       ocurrencies = g.Count()
                                                   }).OrderByDescending(x => x.ocurrencies);

                if (estadosValidos.Count() > 0)
                {
                    query = query.Where(x => estadosValidos.Contains(x.InventarioElementoAtributoEstado));
                }

                estadosOperacionalesTop = query.Take(top).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                estadosOperacionalesTop = null;
            }
            return estadosOperacionalesTop;
        }

        public List<ItemInvInicio> GetElementosTotales(DateTime fechaIncio)
        {
            List<ItemInvInicio> inventario;
            try
            {
                inventario = (from c in Context.InventarioElementos
                              where c.FechaCreacion >= fechaIncio
                              select new ItemInvInicio()
                              {
                                  InventarioElementoID = c.InventarioElementoID,
                                  FechaCreacion = c.FechaCreacion
                              }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                inventario = null;
            }
            return inventario;
        }

        public List<ItemInvInicio> GetTopCategorias(int top)
        {
            List<ItemInvInicio> categoriasTop;
            try
            {
                categoriasTop = (from c in Context.InventarioElementos
                                 join cat in Context.InventarioCategorias on c.InventarioCategoriaID equals cat.InventarioCategoriaID
                                 group cat by cat.InventarioCategoriaID into g
                                 select new ItemInvInicio()
                                 {
                                     InventarioCategoriaID = g.First().InventarioCategoriaID,
                                     InventarioCategoria = g.First().InventarioCategoria,
                                     ocurrencies = g.Count()
                                 }).OrderByDescending(x => x.ocurrencies).Take(top).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                categoriasTop = null;
            }

            return categoriasTop;
        }

        public List<long> GetTopCategories(int top, List<long> categoriasValidas)
        {
            List<ItemInvInicio> categoriasTop;
            List<long> ids = new List<long>();

            try
            {
                if (categoriasValidas.Count > 0)
                {
                    categoriasTop = new List<ItemInvInicio>();
                    categoriasValidas.ForEach(c =>
                    {
                        categoriasTop.Add(new ItemInvInicio()
                        {
                            InventarioCategoriaID = c
                        });
                    });
                }
                else
                {
                    categoriasTop = GetTopCategorias(top);
                }

                categoriasTop.ForEach(c => ids.Add(c.InventarioCategoriaID));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                ids = new List<long>();
            }

            return ids;
        }

        public List<ItemInvInicio> GetElementosByTopCategories(DateTime fechaInicio, long categoryID)
        {
            List<ItemInvInicio> inventario;

            try
            {
                inventario = (from c in Context.InventarioElementos
                              join cat in Context.InventarioCategorias on c.InventarioCategoriaID equals cat.InventarioCategoriaID
                              where c.InventarioCategoriaID == categoryID &&
                                c.FechaCreacion >= fechaInicio
                              select new ItemInvInicio()
                              {
                                  InventarioElementoID = c.InventarioElementoID,
                                  InventarioCategoriaID = c.InventarioCategoriaID,
                                  FechaCreacion = c.FechaCreacion,
                                  InventarioCategoria = cat.InventarioCategoria
                              }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                inventario = null;
            }
            return inventario;
        }

        #endregion

        public List<InventarioElementos> GetElementosNoAsignadosByEmplazamientoID(long? lCatID, long emplazamientoID)
        {
            List<long> listaVin;
            List<InventarioElementos> inventario;
            try
            {
                listaVin = (from vin
                         in Context.InventarioElementosVinculaciones
                            join ele in Context.InventarioElementos on vin.InventarioElementoID equals ele.InventarioElementoID
                            where ele.EmplazamientoID == emplazamientoID && vin.InventarioElementoPadreID == lCatID
                            select vin.InventarioElementoID).ToList();
                inventario = (from c in Context.InventarioElementos where !listaVin.Contains(c.InventarioElementoID) && c.EmplazamientoID == emplazamientoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string s = ex.Message;
                inventario = null;
            }
            return inventario;
        }

        public List<InventarioElementos> GetElementosNoAsignadosByEmplazamientoIDDisponible(long? lEle, long emplazamientoID, long? lCatID, long? lEmplazamientoTipoID)
        {
            List<long> listaVin;
            List<long> listaCats;
            List<InventarioElementos> inventario;
            try
            {
                listaCats = (from c in Context.InventarioCategoriasVinculaciones
                             where (!c.EmplazamientoTipoID.HasValue || c.EmplazamientoTipoID == lEmplazamientoTipoID) && ((lCatID.HasValue) ? c.InventarioCategoriaPadreID == lCatID : !c.InventarioCategoriaPadreID.HasValue) && c.Activo
                             select c.InventarioCategoriaID).ToList();

                listaVin = (from c in Context.Vw_InventarioElementosVinculaciones
                            where c.EmplazamientoID == emplazamientoID && ((lEle.HasValue) ? c.InventarioElementoPadreID == lEle : !c.InventarioElementoPadreID.HasValue)
                            select c.InventarioElementoID).ToList();

                inventario = (from c in Context.InventarioElementos
                              where c.EmplazamientoID == emplazamientoID && !listaVin.Contains(c.InventarioElementoID) && listaCats.Contains(c.InventarioCategoriaID)
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                inventario = null;
            }
            return inventario;
        }

        public InventarioElementosHistoricos GetHistoricoByInvElemID(long invElemID)
        {
            List<InventarioElementosHistoricos> lHistorico;

            lHistorico = (from c in Context.InventarioElementosHistoricos
                          where c.ElementoID == invElemID
                          select c).ToList();

            return lHistorico.LastOrDefault();
        }

        public bool ComprobarDuplicadoNombre(string strNombre, long clienteID)
        {
            bool Duplicado = false;
            List<Vw_InventarioElementosReducida> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_InventarioElementosReducida where (c.ClienteID == clienteID || c.ClienteID == null) && c.Nombre == strNombre select c).ToList();
                if (listaDatos.Count > 0)
                {
                    Duplicado = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Duplicado = true;
            }

            return Duplicado;
        }

        public bool ComprobarDuplicadoCodigo(string strCodigo, long clienteID)
        {
            bool Duplicado = false;
            List<Vw_InventarioElementosReducida> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_InventarioElementosReducida
                              where (c.ClienteID == clienteID || c.ClienteID == null)
                              && c.NumeroInventario == strCodigo
                              select c).ToList();

                if (listaDatos.Count > 0)
                {

                    Duplicado = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Duplicado = true;
            }

            return Duplicado;
        }

        #region GRID INVENTARIO
        public DataTable GetGridDinamicoInventariov4(long CategoriaID, long EmplazamientoID, int nPAg, int nFilas)
        {
            DataTable listaDatos;

            #region Plantillas

            string select = " select InvEle.InventarioElementoID, " +
" InvEle.NumeroInventario, " +
" InvEle.Nombre, " +
" InvEle.InventarioCategoriaID, " +
" InvEle.InventarioCategoria, " +
" InvEle.EmplazamientoID, " +
" InvEle.Codigo, " +
" InvEle.OperadorID, " +
" InvEle.Nombre, " +
" InvEle.InventarioElementoAtributoEstadoID, " +
" InvEle.NombreAtributoEstado, " +
" InvEle.CreadorID, " +
" InvEle.NombreCompleto, " +
" InvEle.FechaCreacion, " +
" InvEle.UltimaModificacionFecha, " +
" InvEle.JsonInvetarioElemento, " +
" InvEle.jsonPlantillas, " +
" Count(InvEle.InventarioElementoID) Over() as totalReg " +
" From vw_CoreInventarioElementos InvEle ";
            string paginacion = " order by InvEle.NumeroInventario OFFSET " + (nPAg * nFilas) + " ROWS FETCH NEXT " + nFilas + " ROWS ONLY";


            #endregion

            try
            {
                string query = select;
                if (CategoriaID != 0)
                {
                    query += " where ";
                    query += " InvEle.InventarioCategoriaID = " + CategoriaID;
                    if (EmplazamientoID != 0)
                    {
                        query += " and InvEle.EmplazamientoID = " + EmplazamientoID;
                    }
                }
                else if (EmplazamientoID != 0)
                {
                    query += " where ";
                    query += " InvEle.EmplazamientoID = " + EmplazamientoID;
                }
                query += paginacion;
                listaDatos = EjecutarQuery(query);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;

        }

        public DataTable GetGridDinamicoInventariov4(long CategoriaID, long EmplazamientoID, int nPAg, int nFilas,
            string listaIDsOperadores,
            string listaIDsEstados,
            string listaIDsUsuarios,
            string sFechaCreacionMin,
            string sFechaCreacionMax,
            string sFechaModMin,
            string sFechaModMax,
            string sFiltros,
            string Ordenacion = "NumeroInventario",
            bool bAsc = true,
            string TipoDato = null)
        {
            FiltroInventarioElementosController cFiltros = new FiltroInventarioElementosController();
            DataTable listaDatos;


            #region Filtros

            string filtros = "";
            string Having = "";
            string campo = "";
            string condicion = "";

            if (sFiltros != null && sFiltros != "")
            {
                foreach (var item in cFiltros.Deserializacion(sFiltros))
                {
                    campo = "";
                    condicion = "";
                    foreach (var oFiltro in item.listaFiltros)
                    {
                        switch (oFiltro.TipoCampo)
                        {
                            case CamposFiltroInventario.TipoCampoElemento:
                                campo = "InvEle." + oFiltro.Campo;
                                break;
                            case CamposFiltroInventario.TipoCampoAtributo:
                                campo = "Json_Value(InvEle.JsonInvetarioElemento,  '$.\"" + oFiltro.Campo + "\".\"Valor\"')";
                                break;
                            case CamposFiltroInventario.TipoCampoPlantilla:
                                campo = "Json_Value(InvEle.jsonPlantillas,  '$.\"" + oFiltro.Campo + "\".\"PlantillaID\"')";
                                break;
                        }
                        switch (oFiltro.TypeData)
                        {
                            case Comun.TIPODATO_CODIGO_TEXTAREA:
                            case Comun.TIPODATO_CODIGO_TEXTO:
                                condicion = campo + " Like '%" + oFiltro.Value + "%'";
                                break;
                            case Comun.TIPODATO_CODIGO_NUMERICO:
                                condicion = "CONVERT(real, (Replace(" + campo + ", ',', '.'))) " + oFiltro.Operador + " " + oFiltro.Value;
                                break;
                            case Comun.TIPODATO_CODIGO_FECHA:
                                string BBDD = Comun.CultureBBDD;
                                if (BBDD.Contains("english"))
                                {
                                    campo = "Json_Value(InvEle.JsonInvetarioElemento,  '$.\"" + oFiltro.Campo + "\".\"Valor\"')";
                                    condicion = "CONVERT(varchar, " + campo + ", 103) " + oFiltro.Operador + " CONVERT(datetime, '" + oFiltro.Value + "', 103)";
                                }
                                else
                                {
                                    campo = "Json_Value(InvEle.JsonInvetarioElemento,  '$.\"" + oFiltro.Campo + "\".\"TextLista\"')";
                                    condicion = "TRY_CONVERT(datetime, " + campo + ", 103) " + oFiltro.Operador + " FORMAT(CONVERT(datetime, '" + oFiltro.Value + "', 103), 'dd/MM/yyyy')";

                                }
                                break;
                            case Comun.TIPODATO_CODIGO_LISTA:
                                condicion = campo + " in ('" + oFiltro.Value.Replace(",", "','") + "')";
                                break;
                            case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                                condicion = "(";
                                foreach (var valor in oFiltro.Value.Split(','))
                                {
                                    condicion += ((condicion == "(") ? "" : " or ") + campo + " Like '%" + valor + "%'";
                                }
                                condicion += ")";
                                break;
                            case Comun.TIPODATO_CODIGO_BOOLEAN:
                                condicion = "Upper(" + campo + ") = Upper('" + oFiltro.Value + "')";
                                break;
                        }
                        filtros += " and " + condicion + " ";
                    }
                }
            }

            #endregion

            #region Plantillas
            string select;
            select = " select InvEle.InventarioElementoID, " +
" InvEle.NumeroInventario, " +
" InvEle.Nombre, " +
" InvEle.InventarioCategoriaID, " +
" InvEle.InventarioCategoria, " +
" InvEle.EmplazamientoID, " +
" InvEle.Codigo, " +
" InvEle.OperadorID, " +
" InvEle.Operador, " +
" InvEle.InventarioElementoAtributoEstadoID, " +
" InvEle.NombreAtributoEstado, " +
" InvEle.CreadorID, " +
" InvEle.NombreCompleto, " +
" InvEle.FechaCreacion, " +
" InvEle.UltimaModificacionFecha, " +
" InvEle.JsonInvetarioElemento, " +
" InvEle.jsonPlantillas, " +
" Count(InvEle.InventarioElementoID) Over() as totalReg " +
" From vw_CoreInventarioElementos InvEle ";
            string paginacion = "";
            if (Ordenacion.StartsWith("Atr"))
            {
                switch (TipoDato)
                {
                    case Comun.TIPODATO_CODIGO_NUMERICO:
                        paginacion = " order by Cast(Json_Value( InvEle.JsonInvetarioElemento , '$.\"" + Ordenacion.Replace("Atr", "") + "\".\"TextLista\"') as float) " + ((bAsc) ? "Asc" : "Desc") + " OFFSET " + (nPAg * nFilas) + " ROWS FETCH NEXT " + nFilas + " ROWS ONLY";
                        if (nPAg == 0 && nFilas == 0)
                        {
                            paginacion = " order by Cast(Json_Value( InvEle.JsonInvetarioElemento , '$.\"" + Ordenacion.Replace("Atr", "") + "\".\"TextLista\"') as float) " + ((bAsc) ? "Asc" : "Desc");
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_FECHA:
                        paginacion = " order by TRY_CONVERT(datetime, Json_Value( InvEle.JsonInvetarioElemento , '$.\"" + Ordenacion.Replace("Atr", "") + "\".\"TextLista\"'), 103) " + ((bAsc) ? "Asc" : "Desc") + " OFFSET " + (nPAg * nFilas) + " ROWS FETCH NEXT " + nFilas + " ROWS ONLY";
                        if (nPAg == 0 && nFilas == 0)
                        {
                            paginacion = " order by TRY_CONVERT(datetime, Json_Value( InvEle.JsonInvetarioElemento , '$.\"" + Ordenacion.Replace("Atr", "") + "\".\"TextLista\"'), 103) " + ((bAsc) ? "Asc" : "Desc");
                        }
                        break;
                    default:
                        paginacion = " order by Json_Value( InvEle.JsonInvetarioElemento , '$.\"" + Ordenacion.Replace("Atr", "") + "\".\"TextLista\"') " + ((bAsc) ? "Asc" : "Desc") + " OFFSET " + (nPAg * nFilas) + " ROWS FETCH NEXT " + nFilas + " ROWS ONLY";
                        if (nPAg == 0 && nFilas == 0)
                        {
                            paginacion = " order by Json_Value( InvEle.JsonInvetarioElemento , '$.\"" + Ordenacion.Replace("Atr", "") + "\".\"TextLista\"') " + ((bAsc) ? "Asc" : "Desc");
                        }
                        break;
                }
            }
            else if (Ordenacion.StartsWith("Pla"))
            {
                paginacion = " order by Json_Value( InvEle.jsonPlantillas , '$.\"" + Ordenacion.Replace("Pla", "") + "\".\"NombrePlantilla\"') " + ((bAsc) ? "Asc" : "Desc") + " OFFSET " + (nPAg * nFilas) + " ROWS FETCH NEXT " + nFilas + " ROWS ONLY";
                if (nPAg == 0 && nFilas == 0)
                {
                    paginacion = " order by Json_Value( InvEle.jsonPlantillas , '$.\"" + Ordenacion.Replace("Pla", "") + "\".\"NombrePlantilla\"') " + ((bAsc) ? "Asc" : "Desc");
                }
            }
            else
            {
                paginacion = " order by InvEle." + Ordenacion + " " + ((bAsc) ? "Asc" : "Desc") + " OFFSET " + (nPAg * nFilas) + " ROWS FETCH NEXT " + nFilas + " ROWS ONLY";
                if (nPAg == 0 && nFilas == 0)
                {
                    paginacion = " order by InvEle." + Ordenacion + " " + ((bAsc) ? "Asc" : "Desc");
                }
            }

            //string agrupacion = " group by InvEle.InventarioElementoID,  InvEle.NumeroInventario,  InvEle.Nombre,  InvEle.InventarioCategoriaID,  InvEle.InventarioCategoria,  InvEle.EmplazamientoID,  InvEle.Codigo,  InvEle.OperadorID,  InvEle.Operador,  InvEle.InventarioElementoAtributoEstadoID,  InvEle.NombreAtributoEstado,  InvEle.CreadorID,  InvEle.NombreCompleto,  InvEle.FechaCreacion,  InvEle.UltimaModificacionFecha,  InvEle.JsonInvetarioElemento,  InvEle.jsonPlantillas ";

            #endregion

            try
            {
                string query = select;
                if (CategoriaID != 0)
                {
                    query += " where ";
                    query += " InvEle.InventarioCategoriaID = " + CategoriaID;
                    if (EmplazamientoID != 0)
                    {
                        query += " and InvEle.EmplazamientoID = " + EmplazamientoID;
                    }
                }
                else if (EmplazamientoID != 0)
                {
                    query += " where ";
                    query += " InvEle.EmplazamientoID = " + EmplazamientoID;
                }

                #region QuickFilters

                if (listaIDsOperadores != "")
                {
                    query += " and InvEle.OperadorID in (" + listaIDsOperadores + ") ";
                }

                if (listaIDsEstados != "")
                {
                    query += " and InvEle.InventarioElementoAtributoEstadoID in (" + listaIDsEstados + ") ";
                }

                if (listaIDsUsuarios != "")
                {
                    query += " and InvEle.CreadorID in (" + listaIDsUsuarios + ") ";
                }

                if (sFechaCreacionMin != "")
                {
                    query += " and InvEle.FechaCreacion >= CONVERT(datetime, '" + sFechaCreacionMin + "', 103) ";
                }

                if (sFechaCreacionMax != "")
                {
                    query += " and InvEle.FechaCreacion <= CONVERT(datetime, '" + sFechaCreacionMax + "', 103) ";
                }

                if (sFechaModMin != "")
                {
                    query += " and InvEle.UltimaModificacionFecha >= CONVERT(datetime, '" + sFechaModMin + "', 103) ";
                }

                if (sFechaModMax != "")
                {
                    query += " and InvEle.UltimaModificacionFecha <= CONVERT(datetime, '" + sFechaModMax + "', 103) ";
                }

                #endregion

                query += filtros;
                query += paginacion;
                listaDatos = EjecutarQuery(query);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;

        }

        public DataTable GetGridDinamicoInventariov4AtributosPlantillas(long CategoriaID, long EmplazamientoID, int nPAg, int nFilas,
            string listaIDsOperadores,
            string listaIDsEstados,
            string listaIDsUsuarios,
            string sFechaCreacionMin,
            string sFechaCreacionMax,
            string sFechaModMin,
            string sFechaModMax,
            string sFiltros,
            string Ordenacion = "NumeroInventario",
            bool bAsc = true,
            string TipoDato = null)
        {
            FiltroInventarioElementosController cFiltros = new FiltroInventarioElementosController();
            DataTable listaDatos;


            #region Filtros

            string filtros = "";
            string Having = "";
            string campo = "";
            string condicion = "";

            if (sFiltros != null && sFiltros != "")
            {
                foreach (var item in cFiltros.Deserializacion(sFiltros))
                {
                    campo = "";
                    condicion = "";
                    foreach (var oFiltro in item.listaFiltros)
                    {
                        switch (oFiltro.TipoCampo)
                        {
                            case CamposFiltroInventario.TipoCampoElemento:
                                campo = "InvEle." + oFiltro.Campo;
                                break;
                            case CamposFiltroInventario.TipoCampoAtributo:
                                campo = "Json_Value(InvEle.JsonInvetarioElemento,  '$.\"" + oFiltro.Campo + "\".\"Valor\"')";
                                break;
                            case CamposFiltroInventario.TipoCampoPlantilla:
                                campo = "Json_Value(InvEle.jsonPlantillas,  '$.\"" + oFiltro.Campo + "\".\"PlantillaID\"')";
                                break;
                        }
                        switch (oFiltro.TypeData)
                        {
                            case Comun.TIPODATO_CODIGO_TEXTAREA:
                            case Comun.TIPODATO_CODIGO_TEXTO:
                                condicion = campo + " Like '%" + oFiltro.Value + "%'";
                                break;
                            case Comun.TIPODATO_CODIGO_NUMERICO:
                                condicion = "CONVERT(real, (Replace(" + campo + ", ',', '.'))) " + oFiltro.Operador + " " + oFiltro.Value;
                                break;
                            case Comun.TIPODATO_CODIGO_FECHA:
                                condicion = "CONVERT(datetime, " + campo + ", 120) " + oFiltro.Operador + " CONVERT(datetime, '" + oFiltro.Value + "', 103)";
                                break;
                            case Comun.TIPODATO_CODIGO_LISTA:
                                condicion = campo + " in ('" + oFiltro.Value.Replace(",", "','") + "')";
                                break;
                            case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                                condicion = "(";
                                foreach (var valor in oFiltro.Value.Split(','))
                                {
                                    condicion += ((condicion == "(") ? "" : " or ") + campo + " Like '%" + valor + "%'";
                                }
                                condicion += ")";
                                break;
                            case Comun.TIPODATO_CODIGO_BOOLEAN:
                                condicion = "Upper(" + campo + ") = Upper('" + oFiltro.Value + "')";
                                break;
                        }
                        filtros += " and " + condicion + " ";
                    }
                }
            }

            #endregion

            #region Plantillas
            string select;
            select = " select InvEle.InventarioElementoID, " +
" InvEle.NumeroInventario, " +
" InvEle.Nombre, " +
" InvEle.InventarioCategoriaID, " +
" InvEle.InventarioCategoria, " +
" InvEle.EmplazamientoID, " +
" InvEle.Codigo, " +
" InvEle.OperadorID, " +
" InvEle.Operador, " +
" InvEle.InventarioElementoAtributoEstadoID, " +
" InvEle.NombreAtributoEstado, " +
" InvEle.CreadorID, " +
" InvEle.NombreCompleto, " +
" InvEle.FechaCreacion, " +
" InvEle.UltimaModificacionFecha, " +
" InvEle.JsonInvetarioElemento, " +
" InvEle.jsonPlantillas, " +
" Count(InvEle.InventarioElementoID) Over() as totalReg " +
" From vw_CoreInventarioElementos InvEle ";
            string paginacion = "";
            if (Ordenacion.StartsWith("Atr"))
            {
                switch (TipoDato)
                {
                    case Comun.TIPODATO_CODIGO_NUMERICO:
                        paginacion = " order by Cast(Json_Value( InvEle.JsonInvetarioElemento , '$.\"" + Ordenacion.Replace("Atr", "") + "\".\"TextLista\"') as float) " + ((bAsc) ? "Asc" : "Desc") + " OFFSET " + (nPAg * nFilas) + " ROWS FETCH NEXT " + nFilas + " ROWS ONLY";
                        if (nPAg == 0 && nFilas == 0)
                        {
                            paginacion = " order by Cast(Json_Value( InvEle.JsonInvetarioElemento , '$.\"" + Ordenacion.Replace("Atr", "") + "\".\"TextLista\"') as float) " + ((bAsc) ? "Asc" : "Desc");
                        }
                        break;
                    case Comun.TIPODATO_CODIGO_FECHA:
                        paginacion = " order by TRY_CONVERT(datetime, Json_Value( InvEle.JsonInvetarioElemento , '$.\"" + Ordenacion.Replace("Atr", "") + "\".\"TextLista\"'), 103) " + ((bAsc) ? "Asc" : "Desc") + " OFFSET " + (nPAg * nFilas) + " ROWS FETCH NEXT " + nFilas + " ROWS ONLY";
                        if (nPAg == 0 && nFilas == 0)
                        {
                            paginacion = " order by TRY_CONVERT(datetime, Json_Value( InvEle.JsonInvetarioElemento , '$.\"" + Ordenacion.Replace("Atr", "") + "\".\"TextLista\"'), 103) " + ((bAsc) ? "Asc" : "Desc");
                        }
                        break;
                    default:
                        paginacion = " order by Json_Value( InvEle.JsonInvetarioElemento , '$.\"" + Ordenacion.Replace("Atr", "") + "\".\"TextLista\"') " + ((bAsc) ? "Asc" : "Desc") + " OFFSET " + (nPAg * nFilas) + " ROWS FETCH NEXT " + nFilas + " ROWS ONLY";
                        if (nPAg == 0 && nFilas == 0)
                        {
                            paginacion = " order by Json_Value( InvEle.JsonInvetarioElemento , '$.\"" + Ordenacion.Replace("Atr", "") + "\".\"TextLista\"') " + ((bAsc) ? "Asc" : "Desc");
                        }
                        break;
                }
            }
            else if (Ordenacion.StartsWith("Pla"))
            {
                paginacion = " order by Json_Value( InvEle.jsonPlantillas , '$.\"" + Ordenacion.Replace("Pla", "") + "\".\"NombrePlantilla\"') " + ((bAsc) ? "Asc" : "Desc") + " OFFSET " + (nPAg * nFilas) + " ROWS FETCH NEXT " + nFilas + " ROWS ONLY";
                if (nPAg == 0 && nFilas == 0)
                {
                    paginacion = " order by Json_Value( InvEle.jsonPlantillas , '$.\"" + Ordenacion.Replace("Pla", "") + "\".\"NombrePlantilla\"') " + ((bAsc) ? "Asc" : "Desc");
                }
            }
            else
            {
                paginacion = " order by InvEle." + Ordenacion + " " + ((bAsc) ? "Asc" : "Desc") + " OFFSET " + (nPAg * nFilas) + " ROWS FETCH NEXT " + nFilas + " ROWS ONLY";
                if (nPAg == 0 && nFilas == 0)
                {
                    paginacion = " order by InvEle." + Ordenacion + " " + ((bAsc) ? "Asc" : "Desc");
                }
            }
            string agrupacion = " group by InvEle.InventarioElementoID,  InvEle.NumeroInventario,  InvEle.Nombre,  InvEle.InventarioCategoriaID,  InvEle.InventarioCategoria,  InvEle.EmplazamientoID,  InvEle.Codigo,  InvEle.OperadorID,  InvEle.Operador,  InvEle.InventarioElementoAtributoEstadoID,  InvEle.NombreAtributoEstado,  InvEle.CreadorID,  InvEle.NombreCompleto,  InvEle.FechaCreacion,  InvEle.UltimaModificacionFecha,  InvEle.JsonInvetarioElemento,  InvEle.jsonPlantillas ";

            #endregion

            try
            {
                string query = select;
                if (CategoriaID != 0)
                {
                    query += " where ";
                    query += " InvEle.InventarioCategoriaID = " + CategoriaID;
                    if (EmplazamientoID != 0)
                    {
                        query += " and InvEle.EmplazamientoID = " + EmplazamientoID;
                    }
                }
                else if (EmplazamientoID != 0)
                {
                    query += " where ";
                    query += " InvEle.EmplazamientoID = " + EmplazamientoID;
                }

                #region QuickFilters

                if (listaIDsOperadores != "")
                {
                    query += " and InvEle.OperadorID in (" + listaIDsOperadores + ") ";
                }

                if (listaIDsEstados != "")
                {
                    query += " and InvEle.InventarioElementoAtributoEstadoID in (" + listaIDsEstados + ") ";
                }

                if (listaIDsUsuarios != "")
                {
                    query += " and InvEle.CreadorID in (" + listaIDsUsuarios + ") ";
                }

                if (sFechaCreacionMin != "")
                {
                    query += " and InvEle.FechaCreacion >= CONVERT(datetime, '" + sFechaCreacionMin + "', 103) ";
                }

                if (sFechaCreacionMax != "")
                {
                    query += " and InvEle.FechaCreacion <= CONVERT(datetime, '" + sFechaCreacionMax + "', 103) ";
                }

                if (sFechaModMin != "")
                {
                    query += " and InvEle.UltimaModificacionFecha >= CONVERT(datetime, '" + sFechaModMin + "', 103) ";
                }

                if (sFechaModMax != "")
                {
                    query += " and InvEle.UltimaModificacionFecha <= CONVERT(datetime, '" + sFechaModMax + "', 103) ";
                }

                #endregion

                query += filtros;
                query += paginacion;
                listaDatos = EjecutarQuery(query);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;

        }

        #endregion

        public void ExportarInventarioEmplazamiento(long EmplazamientoID, string sArchivo, long lUsuarioID, List<long> listaCatIDs, JsonObject listaTraducciones,
            string listaIDsOperadores,
            string listaIDsEstados,
            string listaIDsUsuarios,
            string sFechaCreacionMin,
            string sFechaCreacionMax,
            string sFechaModMin,
            string sFechaModMax,
            string sFiltros)
        {


            InventarioElementosAtributosJsonController cAtrJson = new InventarioElementosAtributosJsonController();
            InventarioElementosPlantillasJsonController cPlaJson = new InventarioElementosPlantillasJsonController();
            InventarioPlantillasAtributosJsonController cPlaAtrJson = new InventarioPlantillasAtributosJsonController();


            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            cEmplazamientos.SetDataContext(Context);
            Emplazamientos oEmpl;

            InventarioCategoriasController cCategorias = new InventarioCategoriasController();
            cCategorias.SetDataContext(Context);
            List<InventarioCategorias> listaCategorias;

            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCatConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            cCatConf.SetDataContext(Context);

            CoreInventarioCategoriasAtributosCategoriasController cCategoriasVin = new CoreInventarioCategoriasAtributosCategoriasController();
            cCategoriasVin.SetDataContext(Context);

            CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
            cAtributosConf.SetDataContext(Context);

            List<CoreAtributosConfiguraciones> listaAtributos;

            InventarioElementosController cElementos = new InventarioElementosController();
            cElementos.SetDataContext(Context);
            DataTable listaElementos;

            JObject json, jsonAux;
            object oAux;

            try
            {
                oEmpl = cEmplazamientos.GetItem(EmplazamientoID);
                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(sArchivo, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                }
                uint sheetId = 0;
                if (listaCatIDs != null)
                {
                    listaCategorias = cCategorias.GetCategoriasByCategoriasIDs(listaCatIDs);
                }
                else
                {
                    listaCategorias = cCategorias.GetInventarioCategoriasByTipoEmplazamiento(oEmpl.EmplazamientoTipoID.ToString(), oEmpl.ClienteID);
                }


                FiltroInventarioElementosController cFiltros = new FiltroInventarioElementosController();
                List<JsonItemsFiltroInventario> listaFiltros = cFiltros.Deserializacion(sFiltros);


                foreach (var oCat in listaCategorias)
                {
                    List<List<string>> listasExcel = new List<List<string>>();
                    List<string> filaCabecera = new List<string>();
                    List<string> filaValores;

                    #region Carga de Listas

                    JsonObject listasItems = new JsonObject();
                    JsonObject listaItems = new JsonObject();
                    JsonObject auxJson;


                    foreach (var oAtr in cCategoriasVin.GetAtributosByInventarioCategoriaID(oCat.InventarioCategoriaID))
                    {
                        if (oAtr.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_LISTA || oAtr.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                        {
                            if (oAtr.TablaModeloDatoID != null)
                            {
                                listaItems = cAtributosConf.GetJsonItemsComboBox((long)oAtr.CoreAtributoConfiguracionID);

                            }
                            else if (oAtr.ValoresPosibles != null && oAtr.ValoresPosibles != "")
                            {
                                listaItems = new JsonObject();
                                foreach (var val in oAtr.ValoresPosibles.Split(';'))
                                {
                                    try
                                    {
                                        auxJson = new JsonObject();
                                        auxJson.Add("Value", val);
                                        auxJson.Add("Text", val);
                                        listaItems.Add(val, auxJson);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                            listasItems.Add(oAtr.CoreAtributoConfiguracionID.ToString(), listaItems);
                        }
                    }

                    #endregion

                    #region CABECERAS

                    #region Campos estaticos

                    if (listaTraducciones.TryGetValue("Codigo", out oAux))
                    {
                        filaCabecera.Add(oAux.ToString());
                    }
                    else
                    {
                        filaCabecera.Add("");
                    }
                    if (listaTraducciones.TryGetValue("Nombre", out oAux))
                    {
                        filaCabecera.Add(oAux.ToString());
                    }
                    else
                    {
                        filaCabecera.Add("");
                    }

                    if (listaTraducciones.TryGetValue("Operador", out oAux))
                    {
                        filaCabecera.Add(oAux.ToString());
                    }
                    else
                    {
                        filaCabecera.Add("");
                    }
                    if (listaTraducciones.TryGetValue("Estado", out oAux))
                    {
                        filaCabecera.Add(oAux.ToString());
                    }
                    else
                    {
                        filaCabecera.Add("");
                    }

                    #endregion

                    #region CamposDinamicos

                    List<long> listaIDsPlantillas = new List<long>();
                    List<long> listaIDsAtributos = new List<long>();

                    List<CoreInventarioCategoriasAtributosCategoriasConfiguraciones> listaPlantillas = cCatConf.GetPlantillasCategoriaID(oCat.InventarioCategoriaID);
                    foreach (var oPla in listaPlantillas)
                    {
                        filaCabecera.Add(oPla.InventarioAtributosCategorias.InventarioAtributoCategoria);
                        listaIDsPlantillas.Add(oPla.CoreInventarioCategoriaAtributoCategoriaConfiguracionID);
                    }

                    listaAtributos = cCategoriasVin.GetAtributosVisiblesByInventarioCategoriaID(oCat.InventarioCategoriaID, lUsuarioID);
                    if (listaAtributos != null && listaAtributos.Count > 0)
                    {
                        foreach (var oAtr in listaAtributos)
                        {
                            filaCabecera.Add(oAtr.Codigo);
                            listaIDsAtributos.Add(oAtr.CoreAtributoConfiguracionID);
                        }
                    }

                    #endregion

                    listasExcel.Add(filaCabecera);
                    #endregion

                    #region VALORES

                    string FiltrosCat = "";
                    if (listaFiltros != null && listaFiltros.Count > 0)
                    {
                        List<JsonItemsFiltroInventario> filtrosCategoria = (from c in listaFiltros where c.InventarioCategoriaID == oCat.InventarioCategoriaID select c).ToList();
                        if (filtrosCategoria.Count > 0)
                        {
                            FiltrosCat = cFiltros.Serializacion(filtrosCategoria);
                        }
                    }

                    listaElementos = cElementos.GetGridDinamicoInventariov4AtributosPlantillas(oCat.InventarioCategoriaID, EmplazamientoID, 0, 0,
            listaIDsOperadores,
            listaIDsEstados,
            listaIDsUsuarios,
            sFechaCreacionMin,
            sFechaCreacionMax,
            sFechaModMin,
            sFechaModMax,
            FiltrosCat);

                    foreach (DataRow oEle in listaElementos.Rows)
                    {
                        filaValores = new List<string>();

                        filaValores.Add(oEle[1].ToString());
                        filaValores.Add(oEle[2].ToString());
                        filaValores.Add(oEle[8].ToString());
                        filaValores.Add(oEle[10].ToString());

                        List<InventarioElementosAtributosJson> listaAtributosVal = new List<InventarioElementosAtributosJson>();

                        List<InventarioElementosPlantillasJson> listaPlantillasVal = new List<InventarioElementosPlantillasJson>();

                        if (oEle[15] != null && oEle[15].ToString() != "")
                        {
                            listaAtributosVal.AddRange(cAtrJson.Deserializacion(oEle[15].ToString()));
                        }
                        if (oEle.ItemArray.Length > 18 && oEle[18] != null && oEle[18].ToString() != "")
                        {
                            listaAtributosVal.AddRange(cAtrJson.Deserializacion(oEle[18].ToString()));
                        }
                        if (oEle[16] != null && oEle[16].ToString() != "")
                        {
                            listaPlantillasVal.AddRange(cPlaJson.Deserializacion(oEle[16].ToString()));
                        }

                        foreach (var PlaID in listaIDsPlantillas)
                        {
                            var oPla = (from c in listaPlantillasVal where c.InvCatConfID == PlaID select c).FirstOrDefault();
                            if (oPla != null)
                            {
                                filaValores.Add(oPla.NombrePlantilla);
                            }
                            else
                            {
                                filaValores.Add("");
                            }
                        }

                        foreach (var AtrID in listaIDsAtributos)
                        {
                            var oAtr = (from c in listaAtributosVal where c.AtributoID == AtrID select c).FirstOrDefault();
                            if (oAtr != null)
                            {
                                if (oAtr.TipoDato != null && (oAtr.TipoDato == Comun.TIPODATO_CODIGO_LISTA || oAtr.TipoDato == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE))
                                {
                                    string sValor = "";
                                    dynamic auxDina;
                                    if (listasItems.TryGetValue(oAtr.AtributoID.ToString(), out auxDina))
                                    {
                                        listaItems = (JsonObject)auxDina;
                                        string Auxstr;
                                        if (oAtr.Valor.ToString() != "")
                                        {
                                            foreach (var val in oAtr.Valor.ToString().Split(','))
                                            {
                                                if (listaItems.TryGetValue(val, out auxDina))
                                                {
                                                    foreach (var aux in auxDina)
                                                    {
                                                        if (aux.Key == "Text")
                                                        {
                                                            sValor += ", " + aux.Value;
                                                        }
                                                    }
                                                }
                                            }
                                            sValor = sValor.Remove(0, 2);
                                        }
                                        else
                                        {
                                            sValor = oAtr.Valor.ToString();
                                        }
                                    }
                                    else
                                    {
                                        sValor = oAtr.Valor.ToString();
                                    }
                                    filaValores.Add(sValor);
                                }
                                else if (oAtr.TipoDato != null && oAtr.TipoDato == Comun.TIPODATO_CODIGO_FECHA)
                                {
                                    if (oAtr.NombreAtributo != null)
                                    {
                                        if (oAtr.Valor.ToString() != "")
                                        {
                                            try
                                            {
                                                filaValores.Add(DateTime.Parse(oAtr.Valor.ToString(), CultureInfo.InvariantCulture).ToString(Comun.FORMATO_FECHA));
                                            }
                                            catch (Exception ex)
                                            {
                                                filaValores.Add(oAtr.Valor.ToString());
                                            }
                                        }
                                        else
                                        {
                                            filaValores.Add("");
                                        }
                                    }
                                }
                                else
                                {
                                    if (oAtr.NombreAtributo != null)
                                    {
                                        filaValores.Add(oAtr.Valor.ToString());
                                    }
                                }
                            }
                            else
                            {
                                filaValores.Add("");
                            }
                        }

                        listasExcel.Add(filaValores);
                    }

                    #endregion

                    Comun.ExportarModeloDatosDinamicoFilas(sArchivo, oCat.InventarioCategoria, listasExcel, sheetId);
                    sheetId++;

                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        #region API

        public int CountGetInventarioElentosByIDs(List<long> IDs, string sCustomer, long clienteID)
        {
            int elements;
            IQueryable<InventarioElementos> query;

            try
            {
                if (string.IsNullOrEmpty(sCustomer))
                {
                    query = (from inventEle in Context.InventarioElementos
                             join empl in Context.Emplazamientos on inventEle.EmplazamientoID equals empl.EmplazamientoID
                             where
                                IDs.Contains(inventEle.InventarioElementoID) &&
                                empl.ClienteID == clienteID
                             select inventEle);
                }
                else
                {
                    query = (from inventEle in Context.InventarioElementos
                             join empl in Context.Emplazamientos on inventEle.EmplazamientoID equals empl.EmplazamientoID
                             join enti in Context.Entidades on inventEle.OperadorID equals enti.OperadorID
                             join oper in Context.Operadores on inventEle.OperadorID equals oper.OperadorID
                             where
                                IDs.Contains(inventEle.InventarioElementoID) &&
                                empl.ClienteID == clienteID &&
                                enti.Codigo == sCustomer
                             select inventEle);
                }

                elements = query.Count();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                elements = -1;
            }
            return elements;
        }

        public List<InventarioElementos> GetInventarioElentosByIDs(List<long> IDs, string sCustomer, long clienteID, int iPageSize, int iPage)
        {
            List<InventarioElementos> elements;
            IQueryable<InventarioElementos> query;

            try
            {
                if (string.IsNullOrEmpty(sCustomer))
                {
                    query = (from inventEle in Context.InventarioElementos
                             join empl in Context.Emplazamientos on inventEle.EmplazamientoID equals empl.EmplazamientoID
                             where
                                IDs.Contains(inventEle.InventarioElementoID) &&
                                empl.ClienteID == clienteID
                             select inventEle);
                }
                else
                {
                    query = (from inventEle in Context.InventarioElementos
                             join empl in Context.Emplazamientos on inventEle.EmplazamientoID equals empl.EmplazamientoID
                             join enti in Context.Entidades on inventEle.OperadorID equals enti.OperadorID
                             join oper in Context.Operadores on inventEle.OperadorID equals oper.OperadorID
                             where
                                IDs.Contains(inventEle.InventarioElementoID) &&
                                empl.ClienteID == clienteID &&
                                enti.Codigo == sCustomer
                             select inventEle);
                }

                elements = query.Skip(iPageSize * iPage).Take(iPageSize).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                elements = new List<InventarioElementos>();
            }
            return elements;
        }


        public List<InventarioElementos> GetElementsByOperador(string codigo, long clienteID, InventarioCategorias categoriaElemento, int pageSize, int curPage)
        {
            List<InventarioElementos> list;
            IQueryable<InventarioElementos> query;

            OperadoresController cOperadores = new OperadoresController();

            try
            {
                bool EsClienteOperador = cOperadores.EsClienteOperador(codigo, clienteID);

                if (EsClienteOperador)
                {
                    query = (from inv in Context.InventarioElementos
                             join vw_iEr in Context.Vw_InventarioElementosReducida on inv.InventarioElementoID equals vw_iEr.InventarioElementoID
                             where inv.Activo
                             select inv);
                }
                else
                {
                    query = (from inv in Context.InventarioElementos
                             join vw_iEr in Context.Vw_InventarioElementosReducida on inv.InventarioElementoID equals vw_iEr.InventarioElementoID
                             where inv.Activo &&
                                     vw_iEr.CodigoEntidad.Equals(codigo)
                             select inv);
                }

                if (categoriaElemento != null)
                {
                    query = query.Where(c => c.InventarioCategoriaID == categoriaElemento.InventarioCategoriaID);
                }

                list = query.Skip(pageSize * curPage).Take(pageSize).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                list = new List<InventarioElementos>();
            }

            return list;
        }

        public int CountElementsByOperador(string codigo, long clienteID, InventarioCategorias categoriaElemento)
        {
            int size;
            IQueryable<InventarioElementos> query;

            OperadoresController cOperadores = new OperadoresController();

            try
            {
                bool EsClienteOperador = cOperadores.EsClienteOperador(codigo, clienteID);

                if (EsClienteOperador)
                {
                    query = (from inv in Context.InventarioElementos
                             join vw_iEr in Context.Vw_InventarioElementosReducida on inv.InventarioElementoID equals vw_iEr.InventarioElementoID
                             where inv.Activo
                             select inv);
                }
                else
                {
                    query = (from inv in Context.InventarioElementos
                             join vw_iEr in Context.Vw_InventarioElementosReducida on inv.InventarioElementoID equals vw_iEr.InventarioElementoID
                             where inv.Activo &&
                                     vw_iEr.CodigoEntidad.Equals(codigo)
                             select inv);
                }

                if (categoriaElemento != null)
                {
                    query = query.Where(c => c.InventarioCategoriaID == categoriaElemento.InventarioCategoriaID);
                }

                size = query.Count();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                size = -1;
            }

            return size;
        }

        public List<InventarioElementos> GetElementsByOperadorAndSite(string codigo, string siteCode, long clienteID, InventarioCategorias categoriaElemento, int pageSize, int curPage)
        {
            List<InventarioElementos> list;
            IQueryable<InventarioElementos> query;

            OperadoresController cOperadores = new OperadoresController();

            try
            {
                bool EsClienteOperador = cOperadores.EsClienteOperador(codigo, clienteID);

                if (EsClienteOperador)
                {
                    query = (from inv in Context.InventarioElementos
                             join vw_iEr in Context.Vw_InventarioElementosReducida on inv.InventarioElementoID equals vw_iEr.InventarioElementoID
                             join emp in Context.Emplazamientos on inv.EmplazamientoID equals emp.EmplazamientoID
                             where inv.Activo &&
                                     emp.Codigo == siteCode
                             select inv);
                }
                else
                {
                    query = (from inv in Context.InventarioElementos
                             join vw_iEr in Context.Vw_InventarioElementosReducida on inv.InventarioElementoID equals vw_iEr.InventarioElementoID
                             join emp in Context.Emplazamientos on inv.EmplazamientoID equals emp.EmplazamientoID
                             where inv.Activo &&
                                     emp.Codigo == siteCode &&
                                     vw_iEr.CodigoEntidad.Equals(codigo)
                             select inv);
                }

                if (categoriaElemento != null)
                {
                    query = query.Where(c => c.InventarioCategoriaID == categoriaElemento.InventarioCategoriaID);
                }

                list = query.Skip(pageSize * curPage).Take(pageSize).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                list = new List<InventarioElementos>();
            }

            return list;
        }

        public int CountElementsByOperadorAndSite(string codigo, string siteCode, long clienteID, InventarioCategorias categoriaElemento)
        {
            int size;
            IQueryable<InventarioElementos> query;

            OperadoresController cOperadores = new OperadoresController();

            try
            {
                bool EsClienteOperador = cOperadores.EsClienteOperador(codigo, clienteID);

                if (EsClienteOperador)
                {
                    query = (from inv in Context.InventarioElementos
                             join vw_iEr in Context.Vw_InventarioElementosReducida on inv.InventarioElementoID equals vw_iEr.InventarioElementoID
                             join emp in Context.Emplazamientos on inv.EmplazamientoID equals emp.EmplazamientoID
                             where inv.Activo &&
                                     emp.Codigo == siteCode
                             select inv);
                }
                else
                {
                    query = (from inv in Context.InventarioElementos
                             join vw_iEr in Context.Vw_InventarioElementosReducida on inv.InventarioElementoID equals vw_iEr.InventarioElementoID
                             join emp in Context.Emplazamientos on inv.EmplazamientoID equals emp.EmplazamientoID
                             where inv.Activo &&
                                     emp.Codigo == siteCode &&
                                     vw_iEr.CodigoEntidad.Equals(codigo)
                             select inv);
                }

                if (categoriaElemento != null)
                {
                    query = query.Where(c => c.InventarioCategoriaID == categoriaElemento.InventarioCategoriaID);
                }

                size = query.Count();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                size = -1;
            }

            return size;
        }
        public InventarioElementos GetByNumberAndClienteID(string numeroInventario, long clienteID)
        {
            InventarioElementos result;
            try
            {
                result = (from iE in Context.InventarioElementos
                          join emp in Context.Emplazamientos on iE.NumeroInventario equals numeroInventario
                          where iE.EmplazamientoID == emp.EmplazamientoID
                          select iE).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = null;
            }
            return result;
        }

        public bool HasInventoryByNumberAndClienteID(string numeroInventario, long clienteID)
        {
            bool result = false;
            InventarioElementos elemento = GetByNumberAndClienteID(numeroInventario, clienteID);

            if (elemento != null)
            {
                result = true;
            }

            return result;
        }
        public InventarioElementos GetElementsByOperadorAndElementCode(string codigo, string elementoryCode, long clienteID)
        {
            InventarioElementos list;

            OperadoresController cOperadores = new OperadoresController();

            try
            {
                bool EsClienteOperador = cOperadores.EsClienteOperador(codigo, clienteID);

                if (EsClienteOperador)
                {
                    list = (from inv in Context.InventarioElementos
                            join vw_iEr in Context.Vw_InventarioElementosReducida on inv.InventarioElementoID equals vw_iEr.InventarioElementoID
                            join emp in Context.Emplazamientos on inv.EmplazamientoID equals emp.EmplazamientoID
                            where inv.Activo &&
                                    inv.NumeroInventario == elementoryCode
                            select inv).First();
                }
                else
                {
                    list = (from inv in Context.InventarioElementos
                            join vw_iEr in Context.Vw_InventarioElementosReducida on inv.InventarioElementoID equals vw_iEr.InventarioElementoID
                            join emp in Context.Emplazamientos on inv.EmplazamientoID equals emp.EmplazamientoID
                            where inv.Activo &&
                                    inv.NumeroInventario == elementoryCode &&
                                    vw_iEr.CodigoEntidad.Equals(codigo)
                            select inv).First();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                list = null;
            }

            return list;
        }


        #endregion

    }


}