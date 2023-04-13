using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class InventarioElementosVinculacionesController : GeneralBaseController<InventarioElementosVinculaciones, TreeCoreContext>
    {
        public InventarioElementosVinculacionesController()
            : base()
        {

        }

        public List<InventarioElementosVinculaciones> GetVinculacionesFromElemento(long lInventarioElementoID)
        {
            List<InventarioElementosVinculaciones> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioElementosVinculaciones where c.InventarioElementoID == lInventarioElementoID select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public List<InventarioElementos> GetElementosPadres(long lInventarioElementoID)
        {
            List<InventarioElementos> listaDatos;
            try
            {
                listaDatos = (from ele in Context.InventarioElementos join c in Context.InventarioElementosVinculaciones on ele.InventarioElementoID equals c.InventarioElementoPadreID where c.InventarioElementoID == lInventarioElementoID select ele).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public List<InventarioElementos> GetElementosPadresCategoria(long lInventarioElementoID, long lInventarioCategoriaID)
        {
            List<InventarioElementos> listaDatos;
            try
            {
                listaDatos = (from ele in Context.InventarioElementos
                              join c in Context.InventarioElementosVinculaciones on ele.InventarioElementoID equals c.InventarioElementoPadreID
                              where c.InventarioElementoID == lInventarioElementoID && ele.InventarioCategoriaID == lInventarioCategoriaID
                              select ele).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public List<Vw_CoreElementosVinculacionesReducida> GetElementosPadresCategoria(long lInventarioElementoID, long lInventarioCategoriaID, List<Vw_CoreElementosVinculacionesReducida> listaElementos, List<InventarioElementosVinculaciones> listaVinculaciones)
        {
            List<Vw_CoreElementosVinculacionesReducida> listaDatos;
            try
            {
                listaDatos = (from ele in listaElementos
                              join c in listaVinculaciones on ele.InventarioElementoID equals c.InventarioElementoPadreID
                              where c.InventarioElementoID == lInventarioElementoID && ele.InventarioCategoriaID == lInventarioCategoriaID
                              select ele).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public List<InventarioElementos> GetElementosHijos(long lInventarioElementoID)
        {
            List<InventarioElementos> listaDatos;
            try
            {
                listaDatos = (from ele in Context.InventarioElementos join c in Context.InventarioElementosVinculaciones on ele.InventarioElementoID equals c.InventarioElementoID where c.InventarioElementoPadreID == lInventarioElementoID select ele).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public List<InventarioElementos> GetElementosHijosCategoria(long? lInventarioElementoID, long lCategoriaID, long lEmplazamientoID)
        {
            List<InventarioElementos> listaDatos;
            try
            {
                listaDatos = (from ele in Context.InventarioElementos
                              join c in Context.InventarioElementosVinculaciones on ele.InventarioElementoID equals c.InventarioElementoID
                              where ((lInventarioElementoID.HasValue) ? c.InventarioElementoPadreID == lInventarioElementoID : !c.InventarioElementoPadreID.HasValue) && ele.InventarioCategoriaID == lCategoriaID && ele.EmplazamientoID == lEmplazamientoID
                              select ele).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public List<Vw_CoreElementosVinculacionesReducida> GetElementosHijosCategoria(long? lInventarioElementoID, long lCategoriaID, List<Vw_CoreElementosVinculacionesReducida> listaElementos, List<InventarioElementosVinculaciones> listaVinculaciones, long lEmplazamientoID)
        {
            List<Vw_CoreElementosVinculacionesReducida> listaDatos;
            try
            {
                listaDatos = (from ele in listaElementos
                              join c in listaVinculaciones on ele.InventarioElementoID equals c.InventarioElementoID
                              where ((lInventarioElementoID.HasValue) ? c.InventarioElementoPadreID == lInventarioElementoID : !c.InventarioElementoPadreID.HasValue) && ele.InventarioCategoriaID == lCategoriaID && ele.EmplazamientoID == lEmplazamientoID
                              select ele).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public List<Vw_InventarioElementosVinculaciones> GetVwVinculacionesFromCategoriaPadre(long? lInventarioEleID, long? EmplazamientoTipoID, bool Activos)
        {
            List<Vw_InventarioElementosVinculaciones> listaDatos;
            try
            {
                if (Activos)
                {
                    listaDatos = (from c in Context.Vw_InventarioElementosVinculaciones where (lInventarioEleID != null) ? c.InventarioElementoPadreID == lInventarioEleID : !c.InventarioElementoPadreID.HasValue select c).ToList();

                }
                else
                {
                    listaDatos = (from c in Context.Vw_InventarioElementosVinculaciones where (lInventarioEleID != null) ? c.InventarioElementoPadreID == lInventarioEleID : !c.InventarioElementoPadreID.HasValue select c).ToList();

                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public List<Vw_InventarioElementosVinculaciones> GetVwVinculacionesFromEmplazamiento(long lEmplazamientoID, long? ElePadreID)
        {
            List<Vw_InventarioElementosVinculaciones> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_InventarioElementosVinculaciones where ((ElePadreID != null) ? c.InventarioElementoPadreID == ElePadreID : !c.InventarioElementoPadreID.HasValue) && c.EmplazamientoID == lEmplazamientoID select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public List<Vw_InventarioElementosVinculaciones> GetVwVinculacionesFromEmplazamiento(long lEmplazamientoID, long? ElePadreID, List<long> TiposVinculaciones)
        {
            List<Vw_InventarioElementosVinculaciones> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_InventarioElementosVinculaciones
                              join vin in Context.InventarioCategoriasVinculacionesTiposVinculaciones on c.InventarioCategoriaVinculacionID equals vin.InventarioCategoriaVinculacionID
                              where ((ElePadreID != null) ? c.InventarioElementoPadreID == ElePadreID : !c.InventarioElementoPadreID.HasValue) && c.EmplazamientoID == lEmplazamientoID && TiposVinculaciones.Contains(vin.InventarioTipoVinculacionID)
                              select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public InventarioElementosVinculaciones GetVinculacionFromElementos(long lElementoID, long? lElementoPadreID)
        {
            InventarioElementosVinculaciones oDato;
            try
            {
                if (lElementoPadreID.HasValue)
                {
                    oDato = (from c in Context.InventarioElementosVinculaciones where c.InventarioElementoPadreID == lElementoPadreID && c.InventarioElementoID == lElementoID select c).FirstOrDefault();
                }
                else
                {
                    oDato = (from c in Context.InventarioElementosVinculaciones where !c.InventarioElementoPadreID.HasValue && c.InventarioElementoID == lElementoID select c).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }
            return oDato;
        }

        public InventarioElementosVinculaciones GetVinculacionFromElementos(long lElementoID, long? lElementoPadreID, List<InventarioElementosVinculaciones> listaVinculaciones)
        {
            InventarioElementosVinculaciones oDato;
            try
            {
                if (lElementoPadreID.HasValue)
                {
                    oDato = (from c in listaVinculaciones where c.InventarioElementoPadreID == lElementoPadreID && c.InventarioElementoID == lElementoID select c).FirstOrDefault();
                }
                else
                {
                    oDato = (from c in listaVinculaciones where !c.InventarioElementoPadreID.HasValue && c.InventarioElementoID == lElementoID select c).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }
            return oDato;
        }

        public List<InventarioTiposVinculaciones> GetTiposVinculacionFromElementos(long lElementoID, long? lElementoPadreID)
        {
            List<InventarioTiposVinculaciones> oDato;
            try
            {
                if (lElementoPadreID.HasValue)
                {
                    oDato = (from c in Context.InventarioElementosVinculaciones
                             join catVin in Context.InventarioCategoriasVinculacionesTiposVinculaciones on c.InventarioCategoriaVinculacionID equals catVin.InventarioCategoriaVinculacionID
                             join vin in Context.InventarioTiposVinculaciones on catVin.InventarioTipoVinculacionID equals vin.InventarioTipoVinculacionID
                             where c.InventarioElementoPadreID == lElementoPadreID && c.InventarioElementoID == lElementoID
                             select vin).ToList();
                }
                else
                {
                    oDato = (from c in Context.InventarioElementosVinculaciones
                             join catVin in Context.InventarioCategoriasVinculacionesTiposVinculaciones on c.InventarioCategoriaVinculacionID equals catVin.InventarioCategoriaVinculacionID
                             join vin in Context.InventarioTiposVinculaciones on catVin.InventarioTipoVinculacionID equals vin.InventarioTipoVinculacionID
                             where !c.InventarioElementoPadreID.HasValue && c.InventarioElementoID == lElementoID
                             select vin).ToList();
                }
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }
            return oDato;
        }

        public bool TienenVinculacion(long lElementoID, long? lElementoPadreID)
        {
            bool existe = false;

            try
            {
                IQueryable<InventarioElementosVinculaciones> query = (from c in Context.InventarioElementosVinculaciones
                                                                      where
                                                                         c.InventarioElementoID == lElementoID
                                                                      select c);
                if (lElementoPadreID.HasValue)
                {
                    query = query.Where(c => c.InventarioElementoPadreID.Value == lElementoPadreID.Value);
                }
                else
                {
                    query = query.Where(c => c.InventarioElementoPadreID == null);
                }

                existe = query.FirstOrDefault() != null;
            }
            catch (Exception ex)
            {
                existe = false;
                log.Error(ex.Message);
            }

            return existe;
        }

        public ResponseCreateController SaveUpdateVinculacion(Vw_CoreElementosVinculacionesReducida oElePadre,
            Vw_CoreElementosVinculacionesReducida oEleHijo,
            List<Vw_CoreElementosVinculacionesReducida> listaEle,
            List<InventarioElementosVinculaciones> listaVinEle,
            List<InventarioCategoriasVinculaciones> listaVinCat)
        {

            ResponseCreateController result;
            InfoResponse response;

            InventarioElementosVinculaciones oDato = null;
            InventarioCategoriasVinculacionesController cCateVinculaciones = new InventarioCategoriasVinculacionesController();
            cCateVinculaciones.SetDataContext(this.Context);
            try
            {
                InventarioCategoriasVinculaciones oCatVin = null;
                if (oElePadre != null)
                {
                    if ((long) oEleHijo.EmplazamientoID == oElePadre.EmplazamientoID)
                    {
                        oCatVin = cCateVinculaciones.GetVinculacionDefecto(oEleHijo.InventarioCategoriaID, oElePadre.InventarioCategoriaID, oEleHijo.EmplazamientoTipoID, listaVinCat);
                        if (oCatVin == null)
                        {
                            oDato = null;
                            response = new InfoResponse
                            {
                                Result = false,
                                Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_LINKAGE_NOT_ALLOWED_CODE,
                                Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_LINKAGE_NOT_ALLOWED_DESCRIPTION
                            };
                            result = new ResponseCreateController(response, oDato);
                            return result;
                        }

                        oDato = GetVinculacionFromElementos(oEleHijo.InventarioElementoID, oElePadre.InventarioElementoID, listaVinEle);
                    }
                    else
                    {
                        oDato = null;
                        response = new InfoResponse
                        {
                            Result = false,
                            Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                        };
                        result = new ResponseCreateController(response, oDato);
                        return result;
                    }
                }
                else
                {
                    oCatVin = cCateVinculaciones.GetVinculacionDefecto(oEleHijo.InventarioCategoriaID, null, oEleHijo.EmplazamientoTipoID, listaVinCat);
                    if (oCatVin == null)
                    {
                        oDato = null;
                        response = new InfoResponse
                        {
                            Result = false,
                            Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                        };
                        result = new ResponseCreateController(response, oDato);
                        return result;
                    }
                    oDato = GetVinculacionFromElementos(oEleHijo.InventarioElementoID, null, listaVinEle);
                }
                if (oDato == null)
                {
                    switch (int.Parse(oCatVin.TipoRelacion))
                    {
                        case (int)Comun.TiposVinculaciones.Rel_1_1:
                            #region 1-1

                            if ((oElePadre != null) && GetElementosPadresCategoria(oEleHijo.InventarioElementoID, oElePadre.InventarioCategoriaID, listaEle, listaVinEle).Count > 0)
                            {
                                oDato = null;
                                response = new InfoResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_CODE,
                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_DESCRIPTION + ", Relationship with Element : " + GetElementosPadresCategoria(oEleHijo.InventarioElementoID, oElePadre.InventarioCategoriaID, listaEle, listaVinEle).FirstOrDefault().NumeroInventario
                                };
                                result = new ResponseCreateController(response, oDato);
                                return result;
                            }
                            else if ((oElePadre != null) ? (GetElementosHijosCategoria(oElePadre.InventarioElementoID, oEleHijo.InventarioCategoriaID, listaEle, listaVinEle, (long) oEleHijo.EmplazamientoID).Count > 0) : (GetElementosHijosCategoria(null, oEleHijo.InventarioCategoriaID, listaEle, listaVinEle, (long) oEleHijo.EmplazamientoID).Count > 0))
                            {
                                oDato = null;
                                response = new InfoResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_CODE,
                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_DESCRIPTION + ", Relationship with Element : " + ((oElePadre != null) ? (GetElementosHijosCategoria(oElePadre.InventarioElementoID, oEleHijo.InventarioCategoriaID, listaEle, listaVinEle, (long) oEleHijo.EmplazamientoID).FirstOrDefault().NumeroInventario) : (GetElementosHijosCategoria(null, oEleHijo.InventarioCategoriaID, listaEle, listaVinEle, (long) oEleHijo.EmplazamientoID).FirstOrDefault().NumeroInventario))
                                };
                                result = new ResponseCreateController(response, oDato);
                                return result;
                            }
                            else
                            {
                                oDato = new InventarioElementosVinculaciones
                                {
                                    InventarioElementoID = oEleHijo.InventarioElementoID,
                                    InventarioElementoPadreID = (oElePadre != null) ? ((long?)oElePadre.InventarioElementoID) : null,
                                    InventarioCategoriaVinculacionID = oCatVin.InventarioCategoriaVinculacionID
                                };
                                if ((oDato = AddItem(oDato)) == null)
                                {
                                    oDato = null;
                                    response = new InfoResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                                        Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                                    };
                                    result = new ResponseCreateController(response, oDato);
                                    return result;
                                }
                            }

                            #endregion
                            break;
                        case (int)Comun.TiposVinculaciones.Rel_1_N:
                            #region 1-N

                            if ((oElePadre != null) ? GetElementosPadresCategoria(oEleHijo.InventarioElementoID, oElePadre.InventarioCategoriaID, listaEle, listaVinEle).Count > 0 : false)
                            {
                                oDato = null;
                                response = new InfoResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_CODE,
                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_DESCRIPTION + ", Relationship with Element : " + GetElementosPadresCategoria(oEleHijo.InventarioElementoID, oElePadre.InventarioCategoriaID, listaEle, listaVinEle).FirstOrDefault().NumeroInventario
                                };
                                result = new ResponseCreateController(response, oDato);
                                return result;
                            }
                            else
                            {
                                oDato = new InventarioElementosVinculaciones
                                {
                                    InventarioElementoID = oEleHijo.InventarioElementoID,
                                    InventarioElementoPadreID = (oElePadre != null) ? ((long?)oElePadre.InventarioElementoID) : null,
                                    InventarioCategoriaVinculacionID = oCatVin.InventarioCategoriaVinculacionID
                                };
                                if ((oDato = AddItem(oDato)) == null)
                                {
                                    oDato = null;
                                    response = new InfoResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                                        Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                                    };
                                    result = new ResponseCreateController(response, oDato);
                                    return result;
                                }
                            }

                            #endregion
                            break;
                        case (int)Comun.TiposVinculaciones.Rel_N_1:
                            #region N-1

                            if (((oElePadre != null) ? (GetElementosHijosCategoria(oElePadre.InventarioElementoID, oEleHijo.InventarioCategoriaID, listaEle, listaVinEle, (long) oEleHijo.EmplazamientoID).Count > 0)
                                : (GetElementosHijosCategoria(null, oEleHijo.InventarioCategoriaID, listaEle, listaVinEle, (long) oEleHijo.EmplazamientoID).Count > 0)))
                            {
                                oDato = null;
                                response = new InfoResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_CODE,
                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_DESCRIPTION + ", Relationship with Element : " + ((oElePadre != null) ? (GetElementosHijosCategoria(oElePadre.InventarioElementoID, oEleHijo.InventarioCategoriaID, listaEle, listaVinEle, (long) oEleHijo.EmplazamientoID).FirstOrDefault().NumeroInventario)
                                : (GetElementosHijosCategoria(null, oEleHijo.InventarioCategoriaID, listaEle, listaVinEle, (long) oEleHijo.EmplazamientoID).FirstOrDefault().NumeroInventario))
                                };
                                result = new ResponseCreateController(response, oDato);
                                return result;
                            }
                            else
                            {
                                oDato = new InventarioElementosVinculaciones
                                {
                                    InventarioElementoID = oEleHijo.InventarioElementoID,
                                    InventarioElementoPadreID = (oElePadre != null) ? ((long?)oElePadre.InventarioElementoID) : null,
                                    InventarioCategoriaVinculacionID = oCatVin.InventarioCategoriaVinculacionID
                                };
                                if ((oDato = AddItem(oDato)) == null)
                                {
                                    oDato = null;
                                    response = new InfoResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                                        Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                                    };
                                    result = new ResponseCreateController(response, oDato);
                                    return result;
                                }
                            }

                            #endregion
                            break;
                        case (int)Comun.TiposVinculaciones.Rel_N_M:
                            #region N-M
                            oDato = new InventarioElementosVinculaciones
                            {
                                InventarioElementoID = oEleHijo.InventarioElementoID,
                                InventarioElementoPadreID = (oElePadre != null) ? ((long?)oElePadre.InventarioElementoID) : null,
                                InventarioCategoriaVinculacionID = oCatVin.InventarioCategoriaVinculacionID
                            };
                            if ((oDato = AddItem(oDato)) == null)
                            {
                                oDato = null;
                                response = new InfoResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                                };
                                result = new ResponseCreateController(response, oDato);
                                return result;
                            }

                            #endregion
                            break;
                    }
                }
                response = new InfoResponse
                {
                    Result = true,
                    Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION
                };
                result = new ResponseCreateController(response, oDato);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
                response = new InfoResponse
                {
                    Result = false,
                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                };
                result = new ResponseCreateController(response, oDato);
            }
            return result;
        }
        public ResponseCreateController SaveUpdateVinculacion(InventarioElementos oElePadre, InventarioElementos oEleHijo, Emplazamientos oEmplazamiento)
        {

            ResponseCreateController result;
            InfoResponse response;

            InventarioElementosVinculaciones oDato = null;
            InventarioCategoriasVinculacionesController cCateVinculaciones = new InventarioCategoriasVinculacionesController();
            cCateVinculaciones.SetDataContext(this.Context);
            try
            {
                InventarioCategoriasVinculaciones oCatVin = null;
                if (oElePadre != null)
                {
                    oCatVin = cCateVinculaciones.GetVinculacionDefecto(oEleHijo.InventarioCategoriaID, oElePadre.InventarioCategoriaID, oEmplazamiento.EmplazamientoTipoID);
                    if (oCatVin == null)
                    {
                        oDato = null;
                        response = new InfoResponse
                        {
                            Result = false,
                            Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_LINKAGE_NOT_ALLOWED_CODE,
                            Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_LINKAGE_NOT_ALLOWED_DESCRIPTION
                        };
                        result = new ResponseCreateController(response, oDato);
                        return result;
                    }

                    oDato = GetVinculacionFromElementos(oEleHijo.InventarioElementoID, oElePadre.InventarioElementoID);
                }
                else
                {
                    oCatVin = cCateVinculaciones.GetVinculacionDefecto(oEleHijo.InventarioCategoriaID, null, oEmplazamiento.EmplazamientoTipoID);
                    if (oCatVin == null)
                    {
                        oDato = null;
                        response = new InfoResponse
                        {
                            Result = false,
                            Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_LINKAGE_NOT_ALLOWED_CODE,
                            Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_LINKAGE_NOT_ALLOWED_DESCRIPTION
                        };
                        result = new ResponseCreateController(response, oDato);
                        return result;
                    }
                    oDato = GetVinculacionFromElementos(oEleHijo.InventarioElementoID, null);
                }
                if (oDato == null)
                {
                    switch (int.Parse(oCatVin.TipoRelacion))
                    {
                        case (int)Comun.TiposVinculaciones.Rel_1_1:
                            #region 1-1

                            if ((oElePadre != null) && GetElementosPadresCategoria(oEleHijo.InventarioElementoID, oElePadre.InventarioCategoriaID).Count > 0)
                            {
                                oDato = null;
                                response = new InfoResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_CODE,
                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_DESCRIPTION + ", Relationship with Element : " + GetElementosPadresCategoria(oEleHijo.InventarioElementoID, oElePadre.InventarioCategoriaID).FirstOrDefault().NumeroInventario
                                };
                                result = new ResponseCreateController(response, oDato);
                                return result;
                            }
                            else if ((oElePadre != null) ? (GetElementosHijosCategoria(oElePadre.InventarioElementoID, oEleHijo.InventarioCategoriaID, oEmplazamiento.EmplazamientoID).Count > 0) : (GetElementosHijosCategoria(null, oEleHijo.InventarioCategoriaID, oEmplazamiento.EmplazamientoID).Count > 0))
                            {
                                oDato = null;
                                response = new InfoResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_CODE,
                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_DESCRIPTION + ", Relationship with Element : " + ((oElePadre != null) ? (GetElementosHijosCategoria(oElePadre.InventarioElementoID, oEleHijo.InventarioCategoriaID, oEmplazamiento.EmplazamientoID).FirstOrDefault().NumeroInventario) : (GetElementosHijosCategoria(null, oEleHijo.InventarioCategoriaID, oEmplazamiento.EmplazamientoID).FirstOrDefault().NumeroInventario))
                                };
                                result = new ResponseCreateController(response, oDato);
                                return result;
                            }
                            else
                            {
                                oDato = new InventarioElementosVinculaciones
                                {
                                    InventarioElementoID = oEleHijo.InventarioElementoID,
                                    InventarioElementoPadreID = (oElePadre != null) ? ((long?)oElePadre.InventarioElementoID) : null,
                                    InventarioCategoriaVinculacionID = oCatVin.InventarioCategoriaVinculacionID
                                };
                                if ((oDato = AddItem(oDato)) == null)
                                {
                                    oDato = null;
                                    response = new InfoResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                                        Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                                    };
                                    result = new ResponseCreateController(response, oDato);
                                    return result;
                                }
                            }

                            #endregion
                            break;
                        case (int)Comun.TiposVinculaciones.Rel_1_N:
                            #region 1-N

                            if ((oElePadre != null) ? GetElementosPadresCategoria(oEleHijo.InventarioElementoID, oElePadre.InventarioCategoriaID).Count > 0 : false)
                            {
                                oDato = null;
                                response = new InfoResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_CODE,
                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_DESCRIPTION + ", Relationship with Element : " + GetElementosPadresCategoria(oEleHijo.InventarioElementoID, oElePadre.InventarioCategoriaID).FirstOrDefault().NumeroInventario
                                };
                                result = new ResponseCreateController(response, oDato);
                                return result;
                            }
                            else
                            {
                                oDato = new InventarioElementosVinculaciones
                                {
                                    InventarioElementoID = oEleHijo.InventarioElementoID,
                                    InventarioElementoPadreID = (oElePadre != null) ? ((long?)oElePadre.InventarioElementoID) : null,
                                    InventarioCategoriaVinculacionID = oCatVin.InventarioCategoriaVinculacionID
                                };
                                if ((oDato = AddItem(oDato)) == null)
                                {
                                    oDato = null;
                                    response = new InfoResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                                        Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                                    };
                                    result = new ResponseCreateController(response, oDato);
                                    return result;
                                }
                            }

                            #endregion
                            break;
                        case (int)Comun.TiposVinculaciones.Rel_N_1:
                            #region N-1

                            if (((oElePadre != null) ? (GetElementosHijosCategoria(oElePadre.InventarioElementoID, oEleHijo.InventarioCategoriaID, oEmplazamiento.EmplazamientoID).Count > 0) : (GetElementosHijosCategoria(null, oEleHijo.InventarioCategoriaID, oEmplazamiento.EmplazamientoID).Count > 0)))
                            {
                                oDato = null;
                                response = new InfoResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_CODE,
                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ERROR_CARDINALITY_DESCRIPTION + ", Relationship with Element : " + ((oElePadre != null) ? (GetElementosHijosCategoria(oElePadre.InventarioElementoID, oEleHijo.InventarioCategoriaID, oEmplazamiento.EmplazamientoID).FirstOrDefault().NumeroInventario) : (GetElementosHijosCategoria(null, oEleHijo.InventarioCategoriaID, oEmplazamiento.EmplazamientoID).FirstOrDefault().NumeroInventario))
                                };
                                result = new ResponseCreateController(response, oDato);
                                return result;
                            }
                            else
                            {
                                oDato = new InventarioElementosVinculaciones
                                {
                                    InventarioElementoID = oEleHijo.InventarioElementoID,
                                    InventarioElementoPadreID = (oElePadre != null) ? ((long?)oElePadre.InventarioElementoID) : null,
                                    InventarioCategoriaVinculacionID = oCatVin.InventarioCategoriaVinculacionID
                                };
                                if ((oDato = AddItem(oDato)) == null)
                                {
                                    oDato = null;
                                    response = new InfoResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                                        Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                                    };
                                    result = new ResponseCreateController(response, oDato);
                                    return result;
                                }
                            }

                            #endregion
                            break;
                        case (int)Comun.TiposVinculaciones.Rel_N_M:
                            #region N-M
                            oDato = new InventarioElementosVinculaciones
                            {
                                InventarioElementoID = oEleHijo.InventarioElementoID,
                                InventarioElementoPadreID = (oElePadre != null) ? ((long?)oElePadre.InventarioElementoID) : null,
                                InventarioCategoriaVinculacionID = oCatVin.InventarioCategoriaVinculacionID
                            };
                            if ((oDato = AddItem(oDato)) == null)
                            {
                                oDato = null;
                                response = new InfoResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                                };
                                result = new ResponseCreateController(response, oDato);
                                return result;
                            }

                            #endregion
                            break;
                    }
                }
                response = new InfoResponse
                {
                    Result = true,
                    Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION
                };
                result = new ResponseCreateController(response, oDato);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
                response = new InfoResponse
                {
                    Result = false,
                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                };
                result = new ResponseCreateController(response, oDato);
            }
            return result;
        }

    }
}