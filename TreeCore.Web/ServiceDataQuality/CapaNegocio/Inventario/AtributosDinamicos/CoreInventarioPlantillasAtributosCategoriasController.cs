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


namespace CapaNegocio
{
    public class CoreInventarioPlantillasAtributosCategoriasController : GeneralBaseController<CoreInventarioPlantillasAtributosCategorias, TreeCoreContext>
    {
        public CoreInventarioPlantillasAtributosCategoriasController()
            : base()
        { }

        public List<CoreInventarioPlantillasAtributosCategorias> GetPlantillasConf(long lCatConfID)
        {
            List<CoreInventarioPlantillasAtributosCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreInventarioPlantillasAtributosCategorias where c.CoreInventarioCategoriaAtributoCategoriaConfiguracionID == lCatConfID select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public bool PlantillaAplicada(long PlantillaID, long CatConf)
        {
            bool bUsada = false;
            string query = "select c.InventarioElementoID from InventarioElementos c" +
" cross apply openjson(c.jsonPlantillas)" +
" with(IDPlantilla int '$.\""+ CatConf + "\".\"PlantillaID\"') as jsonValues" +
" where IDPlantilla = " + PlantillaID;
            try
            {
                if (EjecutarQuery(query).Rows.Count > 0)
                {
                    bUsada = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                bUsada = false;
            }
            return bUsada;
        }

        public bool ComprobarDuplicadoNombre(string strNombre, long lCatConf)
        {
            bool Duplicado = false;
            List<CoreInventarioPlantillasAtributosCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreInventarioPlantillasAtributosCategorias
                              where c.CoreInventarioCategoriaAtributoCategoriaConfiguracionID == lCatConf
                              && c.Nombre == strNombre
                              select c).ToList();

                if (listaDatos.Count > 0)
                {
                    Duplicado = true;
                }
            }
            catch (Exception ex)
            {
                Duplicado = true;
                log.Error(ex.Message);
            }

            return Duplicado;
        }

        public CoreInventarioPlantillasAtributosCategorias GetPlantillaInventarioCategoria(string sNombre, string sNombreCategoria, long lCat)
        {
            CoreInventarioPlantillasAtributosCategorias listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreInventarioPlantillasAtributosCategorias
                              join conf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on c.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals conf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              join atrC in Context.InventarioAtributosCategorias on conf.InventarioAtributoCategoriaID equals atrC.InventarioAtributoCategoriaID
                              join vin in Context.CoreInventarioCategoriasAtributosCategorias on c.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals vin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              where vin.InventarioCategoriaID == lCat
                              && c.Nombre == sNombre
                              && atrC.InventarioAtributoCategoria == sNombreCategoria
                              select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public CoreInventarioPlantillasAtributosCategorias GetPlantillaSubcategoria(string sNombre, long lCatConf)
        {
            CoreInventarioPlantillasAtributosCategorias listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreInventarioPlantillasAtributosCategorias
                              join conf in Context.CoreInventarioCategoriasAtributosCategoriasConfiguraciones on c.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals conf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                              where c.Nombre == sNombre
                              && conf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID == lCatConf
                              select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public List<CoreInventarioPlantillasAtributosCategorias> GetPlantillas(List<long> listaIDsPlantillas)
        {
            List<CoreInventarioPlantillasAtributosCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreInventarioPlantillasAtributosCategorias
                              where listaIDsPlantillas.Contains(c.CoreInventarioPlantillaAtributoCategoriaID)
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public ResponseCreateController CreateUpdatePlantillaCategorias(string sNombre, string sCodigo, long lCreador,
            List<object> listaAtributos,
            long lCategoriaConfiguracionID,
            long? lPlantillaID,
            JsonObject listasCargadas = null,
            JsonObject listasRest = null,
            JsonObject listaObj = null)
        {

            CoreInventarioPlantillasAtributosCategoriasAtributosController cAtributos = new CoreInventarioPlantillasAtributosCategoriasAtributosController();
            cAtributos.SetDataContext(Context);
            InventarioCategoriasController cCategorias = new InventarioCategoriasController();
            cCategorias.SetDataContext(Context);

            InventarioPlantillasAtributosJson oPlantillaAtr;
            List<InventarioPlantillasAtributosJson> listaAtributosJson;
            InventarioPlantillasAtributosJsonController cPlantillaJson = new InventarioPlantillasAtributosJsonController();
            CoreInventarioElementosPlantillasAtributosCategoriasAtributosController cPlantillas = new CoreInventarioElementosPlantillasAtributosCategoriasAtributosController();
            cPlantillas.SetDataContext(Context);
            CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
            cAtributosConf.SetDataContext(Context);
            InventarioElementosHistoricosController cHistorico = new InventarioElementosHistoricosController();
            cHistorico.SetDataContext(Context);

            CoreInventarioPlantillasAtributosCategorias oDato, oDatoAux;

            ResponseCreateController result;
            InfoResponse response;

            JsonObject listas = new JsonObject();
            if (listasCargadas != null)
            {
                listas = listasCargadas;
            }
            else
            {
                foreach (var oAtr in listaAtributos)
                {
                    var AtributoID = oAtr.GetType().GetProperty("AtributoID");
                    var NombreAtributo = oAtr.GetType().GetProperty("NombreAtributo");
                    var Valor = oAtr.GetType().GetProperty("Valor");
                    var TipoDato = oAtr.GetType().GetProperty("TipoDato");
                    if (TipoDato.GetValue(oAtr).ToString() == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE || TipoDato.GetValue(oAtr).ToString() == Comun.TIPODATO_CODIGO_LISTA)
                    {
                        listas.Add(NombreAtributo.GetValue(oAtr).ToString(), cAtributosConf.GetJsonItems(long.Parse(AtributoID.GetValue(oAtr).ToString())));
                    }
                }
            }
            listaAtributosJson = new List<InventarioPlantillasAtributosJson>();
            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    bool Obli;

                    #region Atributos

                    foreach (var oAtr in listaAtributos)
                    {
                        var AtributoID = oAtr.GetType().GetProperty("AtributoID");
                        var NombreAtributo = oAtr.GetType().GetProperty("NombreAtributo");
                        var Valor = oAtr.GetType().GetProperty("Valor");
                        var TipoDato = oAtr.GetType().GetProperty("TipoDato");
                        string descError = "";
                        oPlantillaAtr = cAtributos.SaveUpdateAtributo(
                            long.Parse(AtributoID.GetValue(oAtr).ToString()),
                            Valor.GetValue(oAtr).ToString(),
                            listas,
                            out Obli,
                            out descError,
                            listasRest,
                            listaObj);
                        if (oPlantillaAtr == null)
                        {
                            trans.Dispose();
                            oDato = null;
                            response = new InfoResponse
                            {
                                Result = false,
                                Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ATRIBUTTE_INVALID_FORMAT_CODE,
                                Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ATRIBUTTE_INVALID_FORMAT_DESCRIPTION + ": " + NombreAtributo.GetValue(oAtr).ToString() + ". " + descError
                            };
                            result = new ResponseCreateController(response, oDato);
                            return result;
                        }
                        else
                        {
                            listaAtributosJson.Add(oPlantillaAtr);
                        }
                    }

                    #endregion

                    if (!lPlantillaID.HasValue)
                    {
                        oDato = new CoreInventarioPlantillasAtributosCategorias
                        {
                            Nombre = sNombre,
                            Codigo = sCodigo,
                            FechaCreacion = DateTime.Now,
                            FechaModificacion = DateTime.Now,
                            CoreInventarioCategoriaAtributoCategoriaConfiguracionID = lCategoriaConfiguracionID,
                            CreadorID = lCreador,
                            JsonAtributosDinamicos = cPlantillaJson.Serializacion(listaAtributosJson)
                        };

                        Context.CoreInventarioPlantillasAtributosCategorias.InsertOnSubmit(oDato);
                    }
                    else
                    {
                        oDato = GetItem((long)lPlantillaID);
                        if (oDato.Codigo == sCodigo || !ComprobarDuplicadoNombre(sCodigo, lCategoriaConfiguracionID))
                        {
                            oDato.Nombre = sNombre;
                            oDato.Codigo = sCodigo;
                            oDato.FechaModificacion = DateTime.Now;
                            oDato.JsonAtributosDinamicos = cPlantillaJson.Serializacion(listaAtributosJson);
                            if (!UpdateItem(oDato))
                            {
                                trans.Dispose();
                                oDato = null;
                                response = new InfoResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                                };
                                result = new ResponseCreateController(response, oDato);
                            }
                        }
                        else
                        {
                            trans.Dispose();
                            oDato = null;
                            response = new InfoResponse
                            {
                                Result = false,
                                Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_ELEMENT_CODE_DUPLICATE,
                                Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_ELEMENT_DUPLICATE_DESCRIPTION
                            };
                            result = new ResponseCreateController(response, oDato);
                        }
                    }
                    response = new InfoResponse
                    {
                        Result = true,
                        Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                        Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION
                    };
                    result = new ResponseCreateController(response, oDato);
                    trans.Complete();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    trans.Dispose();
                    oDato = null;
                    response = new InfoResponse
                    {
                        Result = false,
                        Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                        Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION + " " + ex.Message
                    };
                    result = new ResponseCreateController(response, oDato);
                }
            }
            return result;
        }

    }
}