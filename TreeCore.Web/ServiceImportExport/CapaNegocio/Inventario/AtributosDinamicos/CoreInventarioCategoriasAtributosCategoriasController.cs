using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using Ext.Net;
using System.Data.SqlClient;
using System.Transactions;

namespace CapaNegocio
{
    public class CoreInventarioCategoriasAtributosCategoriasController : GeneralBaseController<CoreInventarioCategoriasAtributosCategorias, TreeCoreContext>
    {
        public CoreInventarioCategoriasAtributosCategoriasController()
            : base()
        { }

        public DirectResponse EliminarCategoriaAtributosInventarioCategoria(long? CategoriaID, long CategoriaConfID, out string sException)
        {

            CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
            cAtributos.SetDataContext(this.Context);
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController cCategoriasAtributos = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController();
            cCategoriasAtributos.SetDataContext(this.Context);
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCategoriasConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            cCategoriasConf.SetDataContext(this.Context);

            CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos oCategoriaAtributo;

            DirectResponse direct = new DirectResponse();

            sException = "";

            try
            {
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
                {
                    try
                    {
                        if (CategoriaID != null && CategoriaID != 0)
                        {
                            CoreInventarioCategoriasAtributosCategorias oCategoriaAso = GetRelacion((long)CategoriaID, CategoriaConfID);
                            CoreInventarioCategoriasAtributosCategoriasConfiguraciones oCategoriaConf = cCategoriasConf.GetItem(CategoriaConfID);
                            List<CoreAtributosConfiguraciones> listaAtributos = cCategoriasConf.GetListaAtributos(oCategoriaConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID);
                            foreach (var oAtr in listaAtributos)
                            {
                                oCategoriaAtributo = cCategoriasAtributos.GetVinculacion(oCategoriaConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID, oAtr.CoreAtributoConfiguracionID);
                                if (!cCategoriasAtributos.DeleteItem(oCategoriaAtributo.CoreInventarioCategoriaAtributoCategoriaConfiguracionAtributoID))
                                {
                                    trans.Dispose();
                                    direct.Success = false;
                                    direct.Result = (Comun.strMensajeGenerico);
                                    return direct;
                                }
                                if (!cAtributos.EliminarAtributo(oAtr.CoreAtributoConfiguracionID))
                                {
                                    trans.Dispose();
                                    direct.Success = false;
                                    direct.Result = (Comun.strMensajeGenerico);
                                    return direct;
                                }
                            }
                            if (!DeleteItem(oCategoriaAso.CoreInventarioCategoriaAtributoCategoriaID))
                            {
                                trans.Dispose();
                                direct.Success = false;
                                direct.Result = (Comun.strMensajeGenerico);
                                return direct;
                            }
                            else if (!cCategoriasConf.DeleteItem(CategoriaConfID))
                            {
                                trans.Dispose();
                                direct.Success = false;
                                direct.Result = (Comun.strMensajeGenerico);
                                return direct;
                            }
                        }
                        else
                        {
                            CoreInventarioCategoriasAtributosCategoriasConfiguraciones oCategoriaConf = cCategoriasConf.GetItem(CategoriaConfID);
                            List<CoreAtributosConfiguraciones> listaAtributos = cCategoriasConf.GetListaAtributos(oCategoriaConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID);
                            foreach (var oAtr in listaAtributos)
                            {
                                oCategoriaAtributo = cCategoriasAtributos.GetVinculacion(oCategoriaConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID, oAtr.CoreAtributoConfiguracionID);
                                if (!cCategoriasAtributos.DeleteItem(oCategoriaAtributo.CoreInventarioCategoriaAtributoCategoriaConfiguracionAtributoID))
                                {
                                    trans.Dispose();
                                    direct.Success = false;
                                    direct.Result = (Comun.strMensajeGenerico);
                                    return direct;
                                }
                                if (!cAtributos.EliminarAtributo(oAtr.CoreAtributoConfiguracionID))
                                {
                                    trans.Dispose();
                                    direct.Success = false;
                                    direct.Result = (Comun.strMensajeGenerico);
                                    return direct;
                                }
                                if (!cCategoriasConf.DeleteItem(CategoriaConfID))
                                {
                                    trans.Dispose();
                                    direct.Success = false;
                                    direct.Result = (Comun.strMensajeGenerico);
                                    return direct;
                                }
                            }
                        }

                        trans.Complete();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        if (ex is SqlException Sql)
                        {
                            trans.Dispose();
                            direct.Success = false;
                            direct.Result = (Comun.jsTieneRegistros);
                            sException = (Sql.Message);
                            return direct;
                        }
                        else
                        {
                            trans.Dispose();
                            direct.Success = false;
                            direct.Result = (Comun.strMensajeGenerico);
                            sException = (ex.Message);
                            return direct;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = (Comun.strMensajeGenerico);
                sException = (ex.Message);
                return direct;
            }

            return direct;
        }
        public DirectResponse EliminarRelacionCategoriaAtributosInventarioCategoria(long CategoriaID, long CategoriaConfID, out string sException)
        {
            DirectResponse direct = new DirectResponse();
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController cAtrVin = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributosController();
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCatConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            sException = "";

            try
            {
                if (CategoriaUsada(CategoriaConfID, CategoriaID))
                {
                    direct.Success = false;
                    direct.Result = (Comun.jsTieneRegistros);
                    return direct;
                }
                foreach (var oAtr in cCatConf.GetListaAtributos(CategoriaConfID))
                {
                    if (cAtrVin.AtributoUsadoCategoria(oAtr.CoreAtributoConfiguracionID, CategoriaID))
                    {
                        direct.Success = false;
                        direct.Result = (Comun.jsTieneRegistros);
                        return direct;
                    }
                }
                CoreInventarioCategoriasAtributosCategorias oCategoriaAso = GetRelacion(CategoriaID, CategoriaConfID);
                if (!DeleteItem(oCategoriaAso.CoreInventarioCategoriaAtributoCategoriaID))
                {
                    direct.Success = false;
                    direct.Result = (Comun.strMensajeGenerico);
                    return direct;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                if (ex is SqlException Sql)
                {
                    direct.Success = false;
                    direct.Result = (Comun.jsTieneRegistros);
                    sException = (Sql.Message);
                    return direct;
                }
                else
                {
                    direct.Success = false;
                    direct.Result = (Comun.strMensajeGenerico);
                    sException = (ex.Message);
                    return direct;
                }
            }

            return direct;
        }

        public List<CoreInventarioCategoriasAtributosCategorias> GetCategoriasAtributosVinculadas(long CategoriaID)
        {
            List<CoreInventarioCategoriasAtributosCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreInventarioCategoriasAtributosCategorias where c.InventarioCategoriaID == CategoriaID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public CoreInventarioCategoriasAtributosCategorias GetRelacion(long CategoriaID, long CatConfID)
        {
            CoreInventarioCategoriasAtributosCategorias oDato;
            try
            {
                oDato = (from c in Context.CoreInventarioCategoriasAtributosCategorias where c.InventarioCategoriaID == CategoriaID && c.CoreInventarioCategoriaAtributoCategoriaConfiguracionID == CatConfID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public List<CoreAtributosConfiguraciones> GetAtributosByInventarioCategoriaID(long lInvCatID)
        {
            List<CoreAtributosConfiguraciones> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreAtributosConfiguraciones
                              join atrVin in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c.CoreAtributoConfiguracionID equals atrVin.CoreAtributoConfiguracionID
                              join catConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on atrVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              join catVin in Context.CoreInventarioCategoriasAtributosCategorias on catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              where catVin.InventarioCategoriaID == lInvCatID
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<AtributosConfigWidthCategoria> GetAtributosByInventarioCategoriasIDs(List<long> lInvCatID)
        {
            List<AtributosConfigWidthCategoria> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreAtributosConfiguraciones
                              join atrVin in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c.CoreAtributoConfiguracionID equals atrVin.CoreAtributoConfiguracionID
                              join catConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on atrVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              join catVin in Context.CoreInventarioCategoriasAtributosCategorias on catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              join cat in Context.InventarioCategorias on catVin.InventarioCategoriaID equals cat.InventarioCategoriaID
                              where lInvCatID.Contains(catVin.InventarioCategoriaID)
                              select new AtributosConfigWidthCategoria(cat.InventarioCategoria, c)).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<CoreAtributosConfiguraciones> GetAtributosByInventarioCategoriaIDSinPlantillas(long lInvCatID)
        {
            List<CoreAtributosConfiguraciones> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreAtributosConfiguraciones
                              join atrVin in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c.CoreAtributoConfiguracionID equals atrVin.CoreAtributoConfiguracionID
                              join catConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on atrVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              join atrCat in Context.InventarioAtributosCategorias on catConf.InventarioAtributoCategoriaID equals atrCat.InventarioAtributoCategoriaID
                              join catVin in Context.CoreInventarioCategoriasAtributosCategorias on catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              where catVin.InventarioCategoriaID == lInvCatID && !atrCat.EsPlantilla
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<CoreInventarioCategoriasAtributosCategoriasConfiguraciones> GetSubcategoriaPlantillasValores(long lInvCatID)
        {
            List<CoreInventarioCategoriasAtributosCategoriasConfiguraciones> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioAtributosCategorias
                              join catConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on c.InventarioAtributoCategoriaID equals catConf.InventarioAtributoCategoriaID
                              join catVin in Context.CoreInventarioCategoriasAtributosCategorias on catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              where catVin.InventarioCategoriaID == lInvCatID && c.EsPlantilla
                              select catConf).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<CoreAtributosConfiguraciones> GetAtributosVisiblesByInventarioCategoriaID(long lInvCatID, long lUsuarioID)
        {
            RolesController cRoles = new RolesController();
            List<long> listaRolesID;

            CoreAtributosConfiguracionRolesRestringidosController cAtrRoles = new CoreAtributosConfiguracionRolesRestringidosController();
            List<Vw_CoreAtributosConfiguracionRolesRestringidos> listaRestriccionRoles;

            List<CoreAtributosConfiguraciones> listaDatos, listaFinal;
            try
            {
                listaDatos = (from c in Context.CoreAtributosConfiguraciones
                              join atrVin in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c.CoreAtributoConfiguracionID equals atrVin.CoreAtributoConfiguracionID
                              join catConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on atrVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              join catVin in Context.CoreInventarioCategoriasAtributosCategorias on catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              where catVin.InventarioCategoriaID == lInvCatID
                              orderby catVin.Orden, atrVin.Orden
                              select c).ToList();

                List<Roles> listaRoles = cRoles.GetRolesFromUsuario(lUsuarioID);
                List<long> listaRolesIDs = new List<long>();
                foreach (var oRol in listaRoles) { listaRolesIDs.Add(oRol.RolID); }
                listaFinal = new List<CoreAtributosConfiguraciones>();
                listaFinal.AddRange(listaDatos.GetRange(0, listaDatos.Count));
                foreach (var item in listaDatos)
                {
                    listaRestriccionRoles = cAtrRoles.GetVwRolesFromAtributoNoDefecto(item.CoreAtributoConfiguracionID);
                    if (listaRestriccionRoles != null && listaRestriccionRoles.Where(c => listaRoles.Select(r => r.RolID).ToList().Contains((long)c.RolID)).ToList().Count > 0)
                    {
                        foreach (var oRestriccionRol in listaRestriccionRoles)
                        {
                            if (oRestriccionRol.RolID != null)
                            {
                                if (listaRolesIDs.Contains(oRestriccionRol.RolID.Value))
                                {
                                    switch (oRestriccionRol.Restriccion)
                                    {
                                        case (int)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                            listaFinal.Remove(item);
                                            break;
                                        case (int)Comun.RestriccionesAtributoDefecto.DISABLED:
                                        case (int)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    //mainConstainer.Hidden = true;
                                    //ControlAtributo.Disabled = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        var cInvResDefe = cAtrRoles.GetDefault(item.CoreAtributoConfiguracionID);
                        if (cInvResDefe != null)
                        {
                            switch (cInvResDefe.Restriccion)
                            {
                                case (int)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                    listaFinal.Remove(item);
                                    break;
                                case (int)Comun.RestriccionesAtributoDefecto.DISABLED:
                                case (int)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                default:
                                    break;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaFinal = null;
            }
            return listaFinal;
        }


        public List<CoreAtributosConfiguraciones> GetAtributosVisiblesSinPlantillasByInventarioCategoriaID(long lInvCatID, long lUsuarioID)
        {
            RolesController cRoles = new RolesController();
            List<long> listaRolesID;

            CoreAtributosConfiguracionRolesRestringidosController cAtrRoles = new CoreAtributosConfiguracionRolesRestringidosController();
            List<Vw_CoreAtributosConfiguracionRolesRestringidos> listaRestriccionRoles;

            List<CoreAtributosConfiguraciones> listaDatos, listaFinal;
            try
            {
                listaDatos = (from c in Context.CoreAtributosConfiguraciones
                              join atrVin in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c.CoreAtributoConfiguracionID equals atrVin.CoreAtributoConfiguracionID
                              join catConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on atrVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              join catVin in Context.CoreInventarioCategoriasAtributosCategorias on catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              join catAtr in Context.InventarioAtributosCategorias on catConf.InventarioAtributoCategoriaID equals catAtr.InventarioAtributoCategoriaID
                              where catVin.InventarioCategoriaID == lInvCatID && !catAtr.EsPlantilla
                              orderby catVin.Orden, atrVin.Orden
                              select c).ToList();

                List<Roles> listaRoles = cRoles.GetRolesFromUsuario(lUsuarioID);
                List<long> listaRolesIDs = new List<long>();
                foreach (var oRol in listaRoles) { listaRolesIDs.Add(oRol.RolID); }
                listaFinal = new List<CoreAtributosConfiguraciones>();
                listaFinal.AddRange(listaDatos.GetRange(0, listaDatos.Count));
                foreach (var item in listaDatos)
                {
                    listaRestriccionRoles = cAtrRoles.GetVwRolesFromAtributoNoDefecto(item.CoreAtributoConfiguracionID);
                    if (listaRestriccionRoles != null)
                    {
                        if (listaRestriccionRoles.Count > 0)
                        {
                            foreach (var oRestriccionRol in listaRestriccionRoles)
                            {
                                if (oRestriccionRol.RolID != null)
                                {
                                    if (listaRolesIDs.Contains(oRestriccionRol.RolID.Value))
                                    {
                                        switch (oRestriccionRol.Restriccion)
                                        {
                                            case (int)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                                listaFinal.Remove(item);
                                                break;
                                            case (int)Comun.RestriccionesAtributoDefecto.DISABLED:
                                            case (int)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        //mainConstainer.Hidden = true;
                                        //ControlAtributo.Disabled = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var cInvResDefe = cAtrRoles.GetDefault(item.CoreAtributoConfiguracionID);
                            if (cInvResDefe != null)
                            {
                                switch (cInvResDefe.Restriccion)
                                {
                                    case (int)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                        listaFinal.Remove(item);
                                        break;
                                    case (int)Comun.RestriccionesAtributoDefecto.DISABLED:
                                    case (int)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                    default:
                                        break;
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaFinal = null;
            }
            return listaFinal;
        }
        public List<CoreAtributosConfiguraciones> GetAtributosVisiblesPlantillasByInventarioCategoriaID(long lInvCatID, long lUsuarioID)
        {
            RolesController cRoles = new RolesController();
            List<long> listaRolesID;

            CoreAtributosConfiguracionRolesRestringidosController cAtrRoles = new CoreAtributosConfiguracionRolesRestringidosController();
            List<Vw_CoreAtributosConfiguracionRolesRestringidos> listaRestriccionRoles;

            List<CoreAtributosConfiguraciones> listaDatos, listaFinal;
            try
            {
                listaDatos = (from c in Context.CoreAtributosConfiguraciones
                              join atrVin in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on c.CoreAtributoConfiguracionID equals atrVin.CoreAtributoConfiguracionID
                              join catConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on atrVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              join catVin in Context.CoreInventarioCategoriasAtributosCategorias on catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              join catAtr in Context.InventarioAtributosCategorias on catConf.InventarioAtributoCategoriaID equals catAtr.InventarioAtributoCategoriaID
                              where catVin.InventarioCategoriaID == lInvCatID && catAtr.EsPlantilla
                              orderby catVin.Orden, atrVin.Orden
                              select c).ToList();

                List<Roles> listaRoles = cRoles.GetRolesFromUsuario(lUsuarioID);
                List<long> listaRolesIDs = new List<long>();
                foreach (var oRol in listaRoles) { listaRolesIDs.Add(oRol.RolID); }
                listaFinal = new List<CoreAtributosConfiguraciones>();
                listaFinal.AddRange(listaDatos.GetRange(0, listaDatos.Count));
                foreach (var item in listaDatos)
                {
                    listaRestriccionRoles = cAtrRoles.GetVwRolesFromAtributoNoDefecto(item.CoreAtributoConfiguracionID);
                    if (listaRestriccionRoles != null)
                    {
                        if (listaRestriccionRoles.Count > 0)
                        {
                            foreach (var oRestriccionRol in listaRestriccionRoles)
                            {
                                if (oRestriccionRol.RolID != null)
                                {
                                    if (listaRolesIDs.Contains(oRestriccionRol.RolID.Value))
                                    {
                                        switch (oRestriccionRol.Restriccion)
                                        {
                                            case (int)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                                listaFinal.Remove(item);
                                                break;
                                            case (int)Comun.RestriccionesAtributoDefecto.DISABLED:
                                            case (int)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        //mainConstainer.Hidden = true;
                                        //ControlAtributo.Disabled = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var cInvResDefe = cAtrRoles.GetDefault(item.CoreAtributoConfiguracionID);
                            if (cInvResDefe != null)
                            {
                                switch (cInvResDefe.Restriccion)
                                {
                                    case (int)Comun.RestriccionesAtributoDefecto.HIDDEN:
                                        listaFinal.Remove(item);
                                        break;
                                    case (int)Comun.RestriccionesAtributoDefecto.DISABLED:
                                    case (int)Comun.RestriccionesAtributoDefecto.ACTIVE:
                                    default:
                                        break;
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaFinal = null;
            }
            return listaFinal;
        }

        public Vw_ConfCoreInventarioCategoriasAtributosCategorias GetdatoMigrador(string inventarioCategoria, string inventarioAtributoCategoria, string Codigo)
        {
            Vw_ConfCoreInventarioCategoriasAtributosCategorias dato = new Vw_ConfCoreInventarioCategoriasAtributosCategorias();
            try
            {
                if (inventarioCategoria.Equals(""))
                {
                    dato = (from c in Context.Vw_ConfCoreInventarioCategoriasAtributosCategorias
                            where c.InventarioCategoria == null && c.InventarioAtributoCategoria == inventarioAtributoCategoria
                            && c.Codigo == Codigo
                            select c).First();
                }
                else
                {
                    dato = (from c in Context.Vw_ConfCoreInventarioCategoriasAtributosCategorias
                            where c.InventarioCategoria == inventarioCategoria && c.InventarioAtributoCategoria == inventarioAtributoCategoria
                            && c.Codigo == Codigo
                            select c).First();
                }
                return dato;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }

        public List<long> GetListaConfiguracionesIDAtributosObligatorios(long lCatID)
        {
            List<long> listaIDs;
            try
            {
                listaIDs = (from catVin in Context.CoreInventarioCategoriasAtributosCategorias
                            join catConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals catConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                            join atrConf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on catVin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals atrConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                            join catAtr in Context.InventarioAtributosCategorias on catConf.InventarioAtributoCategoriaID equals catAtr.InventarioAtributoCategoriaID
                            join atr in Context.CoreAtributosConfiguraciones on atrConf.CoreAtributoConfiguracionID equals atr.CoreAtributoConfiguracionID
                            join vwAtrProp in Context.Vw_CoreAtributosConfiguracionTiposDatosPropiedades on atr.CoreAtributoConfiguracionID equals vwAtrProp.CoreAtributoConfiguracionID
                            where vwAtrProp.CodigoTipoDatoPropiedad == "AllowBlank" && vwAtrProp.Valor == "False" && !catAtr.EsPlantilla && catVin.InventarioCategoriaID == lCatID
                            select atr.CoreAtributoConfiguracionID).Distinct().ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaIDs = null;
            }
            return listaIDs;
        }

        public bool CategoriaUsada(long CatConfID, long InvCatID)
        {
            bool bUsada = false;
            string query1 = "SELECT c.InventarioElementoID FROM InventarioElementos c CROSS APPLY OPENJSON(c.jsonPlantillas) WITH (InvCatConfID int '$.\""+ CatConfID + "\".InvCatConfID') AS jsonValues WHERE jsonValues.InvCatConfID = "+ CatConfID+ " AND c.InventarioCategoriaID = "+ InvCatID;
            try
            {
                if (EjecutarQuery(query1).Rows.Count > 0)
                {
                    bUsada = true;
                }
            }
            catch (Exception ex)
            {
                bUsada = true;
                log.Error(ex.Message);
            }
            return bUsada;
        }

    }

    public class AtributosConfigWidthCategoria 
    {
        public string categoria { get; set; }
        public CoreAtributosConfiguraciones coreAtributoConfiguracion { get; set; }

        public AtributosConfigWidthCategoria(CoreAtributosConfiguraciones coreAtributoConfiguracion)
        {
            this.categoria = null;
            this.coreAtributoConfiguracion = coreAtributoConfiguracion;
        }

        public AtributosConfigWidthCategoria(string categoria, CoreAtributosConfiguraciones coreAtributoConfiguracion) {
            this.categoria = categoria;
            this.coreAtributoConfiguracion = coreAtributoConfiguracion;
        }
    }
}