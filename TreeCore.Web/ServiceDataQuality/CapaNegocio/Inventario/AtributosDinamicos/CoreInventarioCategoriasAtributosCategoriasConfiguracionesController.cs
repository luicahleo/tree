using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace CapaNegocio
{
    public class CoreInventarioCategoriasAtributosCategoriasConfiguracionesController : GeneralBaseController<CoreInventarioCategoriasAtributosCategoriasConfiguraciones, TreeCoreContext>
    {
        public CoreInventarioCategoriasAtributosCategoriasConfiguracionesController()
            : base()
        { }

        public List<CoreAtributosConfiguraciones> GetListaAtributos(long lCatConfID)
        {
            List<CoreAtributosConfiguraciones> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreAtributosConfiguraciones
                              join invCateAtr in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c.CoreAtributoConfiguracionID equals invCateAtr.CoreAtributoConfiguracionID
                              where invCateAtr.CoreInventarioCategoriaAtributoCategoriaConfiguracionID == lCatConfID
                              select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public List<Vw_CoreInventarioAtributos> GetListaVwAtributos(long lCatConfID)
        {
            List<Vw_CoreInventarioAtributos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_CoreInventarioAtributos
                              where c.CoreInventarioCategoriaAtributoCategoriaConfiguracionID == lCatConfID
                              select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public CoreInventarioCategoriasAtributosCategoriasConfiguraciones GetPlantilla(long lCatAtrID)
        {
            CoreInventarioCategoriasAtributosCategoriasConfiguraciones oDato;
            try
            {
                oDato = (from c in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones
                         where c.InventarioAtributoCategoriaID == lCatAtrID && !(c.InventarioCategoriaID.HasValue)
                         select c).First();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }
            return oDato;
        }

        public CoreInventarioCategoriasAtributosCategoriasConfiguraciones GetPlantillaByNombre(string sNombre, long lCliID)
        {
            CoreInventarioCategoriasAtributosCategoriasConfiguraciones oDato;
            try
            {
                oDato = (from c in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones
                         join atrCat in Context.InventarioAtributosCategorias on c.InventarioAtributoCategoriaID equals atrCat.InventarioAtributoCategoriaID
                         where atrCat.InventarioAtributoCategoria == sNombre && atrCat.ClienteID == lCliID && !(c.InventarioCategoriaID.HasValue)
                         select c).First();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }
            return oDato;
        }

        public CoreInventarioCategoriasAtributosCategoriasConfiguraciones GetPlantillaValoresByNombreCategoria(string sNombre, long lCategoria)
        {
            CoreInventarioCategoriasAtributosCategoriasConfiguraciones oDato;
            try
            {
                oDato = (from c in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones
                         join atrCat in Context.InventarioAtributosCategorias on c.InventarioAtributoCategoriaID equals atrCat.InventarioAtributoCategoriaID
                         join catVin in Context.CoreInventarioCategoriasAtributosCategorias on c.CoreInventarioCategoriaAtributoCategoriaConfiguracionID  equals catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                         where atrCat.InventarioAtributoCategoria == sNombre && atrCat.EsPlantilla && catVin.InventarioCategoriaID == lCategoria
                         select c).First();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }
            return oDato;
        }

        public CoreAtributosConfiguraciones CrearAtributo(long lCatConfID, long lTipoDatoID, string sNombre, string sCodigo, string sValoresPosibles, long? lColumnaModeloDatoID, int iOrden, long lModulo)
        {
            CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
            cAtributos.SetDataContext(this.Context);
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController cAtributosConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController();
            cAtributosConf.SetDataContext(this.Context);
            CoreAtributosConfiguracionRolesRestringidosController cRestrinccion = new CoreAtributosConfiguracionRolesRestringidosController();
            cRestrinccion.SetDataContext(this.Context);

            CoreInventarioCategoriasAtributosCategoriasConfiguraciones oConf;

            CoreAtributosConfiguraciones oDato;
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos oAsociacion;
            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    oConf = GetItem(lCatConfID);

                    if (oConf == null)
                    {
                        trans.Dispose();
                        return null;
                    }

                    oDato = new CoreAtributosConfiguraciones
                    {
                        TipoDatoID = lTipoDatoID,
                        Nombre = sNombre,
                        Codigo = sCodigo,
                        ValoresPosibles = sValoresPosibles,
                        ColumnaModeloDatoID = lColumnaModeloDatoID,
                        Activo = true,
                        ClienteID = oConf.InventarioAtributosCategorias.ClienteID
                    };
                    oDato = cAtributos.AddItem(oDato);
                    if (oDato != null)
                    {
                        oAsociacion = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos
                        {
                            CoreInventarioCategoriaAtributoCategoriaConfiguracionID = lCatConfID,
                            CoreAtributoConfiguracionID = oDato.CoreAtributoConfiguracionID,
                            Orden = iOrden
                        };
                        if ((oAsociacion = cAtributosConf.AddItem(oAsociacion)) == null)
                        {
                            trans.Dispose();
                            return null;
                        }
                        if (oAsociacion.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioCategoriaID == null)
                        {
                            oDato.Nombre = oAsociacion.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.InventarioAtributoCategoria + "(" + oDato.Nombre + ")";
                        }
                        else
                        {
                            oDato.Nombre = oAsociacion.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioCategorias.Codigo + "(" + oDato.Nombre + ")";
                        }
                        cAtributos.UpdateItem(oDato);
                    }
                    else
                    {
                        trans.Dispose();
                        return null;
                    }


                    #region RESTRICCION POR DEFECTO

                    ParametrosController cParametros = new ParametrosController();
                    cParametros.SetDataContext(this.Context);
                    CoreAtributosConfiguracionRolesRestringidos oRestriccion = new CoreAtributosConfiguracionRolesRestringidos
                    {
                        CoreAtributoConfiguracionID = oDato.CoreAtributoConfiguracionID,
                        RolID = null,
                        Restriccion = int.Parse(cParametros.GetItemByName(Comun.RESTRICCION_DEFECTO_INVENTARIO).Valor)
                    };

                    if (cRestrinccion.AddItem(oRestriccion) == null)
                    {
                        trans.Dispose();
                        return null;
                    }
                    #endregion

                    trans.Complete();
                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    oDato = null;
                    log.Error(ex.Message);
                }
            }
            return oDato;
        }

        public List<CoreInventarioCategoriasAtributosCategoriasConfiguraciones> GetPlantillas(long lClienteID, bool Activo)
        {
            List<CoreInventarioCategoriasAtributosCategoriasConfiguraciones> listaDatos;
            try
            {
                if (Activo)
                {
                    listaDatos = (from c in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones
                                  join catAtr in Context.InventarioAtributosCategorias on c.InventarioAtributoCategoriaID equals catAtr.InventarioAtributoCategoriaID
                                  where !(c.InventarioCategoriaID.HasValue) && catAtr.ClienteID == lClienteID && catAtr.EsPlantilla && catAtr.Activo
                                  select c).ToList();
                }
                else
                {
                    listaDatos = (from c in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones
                                  join catAtr in Context.InventarioAtributosCategorias on c.InventarioAtributoCategoriaID equals catAtr.InventarioAtributoCategoriaID
                                  where !(c.InventarioCategoriaID.HasValue) && catAtr.ClienteID == lClienteID && catAtr.EsPlantilla
                                  select c).ToList();
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }
        public List<CoreInventarioCategoriasAtributosCategoriasConfiguraciones> GetPlantillasCategoriaID(long lCategoriaID)
        {
            List<CoreInventarioCategoriasAtributosCategoriasConfiguraciones> listaDatos;
            try
            {
                    listaDatos = (from c in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones
                                  join catVin in Context.CoreInventarioCategoriasAtributosCategorias on c.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                                  join catAtr in Context.InventarioAtributosCategorias on c.InventarioAtributoCategoriaID equals catAtr.InventarioAtributoCategoriaID
                                  where !(c.InventarioCategoriaID.HasValue) && catAtr.EsPlantilla && catVin.InventarioCategoriaID == lCategoriaID
                                  select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguraciones GetdatoMigrador(string inventarioCategoria, string inventarioAtributoCategoria)
        {
            Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguraciones dato = new Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguraciones();
            try
            {
                if (inventarioCategoria.Equals(""))
                {
                    dato = (from c in Context.Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguraciones
                            where c.InventarioCategoria == null && c.InventarioAtributoCategoria == inventarioAtributoCategoria
                            select c).First();
                }
                else
                {
                    dato = (from c in Context.Vw_ConfCoreInventarioCategoriasAtributosCategoriasConfiguraciones
                            where c.InventarioCategoria == inventarioCategoria
    && c.InventarioAtributoCategoria == inventarioAtributoCategoria
                            select c).First();
                }

                return dato;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public bool ComprobarDuplicidadNombresCategorias(long lInvCatID, long lCatConfID, string NombreSubCategoria)
        {
            bool bDuplicado = false;
            try
            {
                List<CoreAtributosConfiguraciones> listaDuplicados = (from c in Context.CoreAtributosConfiguraciones
                                                                      join invCateAtr in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c.CoreAtributoConfiguracionID equals invCateAtr.CoreAtributoConfiguracionID
                                                                      where invCateAtr.CoreInventarioCategoriaAtributoCategoriaConfiguracionID == lCatConfID
                                                                      && ((from c2 in Context.CoreAtributosConfiguraciones
                                                                          join atrVin in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c2.CoreAtributoConfiguracionID equals atrVin.CoreAtributoConfiguracionID
                                                                          join catVin in Context.CoreInventarioCategoriasAtributosCategorias on atrVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                                                                          where catVin.InventarioCategoriaID == lInvCatID
                                                                          select c2.Codigo).ToList().Contains(c.Codigo) || (from c2 in Context.CoreAtributosConfiguraciones
                                                                                                                            join atrVin in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c2.CoreAtributoConfiguracionID equals atrVin.CoreAtributoConfiguracionID
                                                                                                                            join catVin in Context.CoreInventarioCategoriasAtributosCategorias on atrVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                                                                                                                            where catVin.InventarioCategoriaID == lInvCatID
                                                                                                                            select c2.Codigo).ToList().Contains(NombreSubCategoria))
                                                                      select c).ToList();
                if (listaDuplicados.Count > 0)
                {
                    bDuplicado = true;
                }
            }
            catch (Exception ex)
            {
                bDuplicado = true;
                log.Error(ex.Message);
            }
            return bDuplicado;
        }

        public List<long> GetListaConfiguracionesIDAtributosObligatorios(long lCatID)
        {
            List<long> listaIDs;
            try
            {
                listaIDs = (from  catConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones 
                            join atrConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals atrConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                            join catAtr in Context.InventarioAtributosCategorias on catConf.InventarioAtributoCategoriaID equals catAtr.InventarioAtributoCategoriaID
                            join atr in Context.CoreAtributosConfiguraciones on atrConf.CoreAtributoConfiguracionID equals atr.CoreAtributoConfiguracionID
                            join vwAtrProp in Context.Vw_CoreAtributosConfiguracionTiposDatosPropiedades on atr.CoreAtributoConfiguracionID equals vwAtrProp.CoreAtributoConfiguracionID
                            where vwAtrProp.CodigoTipoDatoPropiedad == "AllowBlank" && vwAtrProp.Valor == "False" && !catAtr.EsPlantilla && catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID == lCatID
                            select atr.CoreAtributoConfiguracionID).Distinct().ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaIDs = null;
            }
            return listaIDs;
        }

    }

}