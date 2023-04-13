using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Tree.Linq.GenericExtensions;
using TreeCore.Clases;
using TreeCore.Data;
using Ext.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Globalization;

namespace CapaNegocio
{
    public class EmplazamientosController : GeneralBaseController<Emplazamientos, TreeCoreContext>
    {
        public EmplazamientosController()
            : base()
        { }

        #region CONSTANTES
        private const string operatorAND = " AND ";
        public static string SITES = "Sites";
        public static string LOCALIZACIONES = "Localizaciones";

        public List<string> lCamposValidosEmplazamientos = new List<string>
        {
            // PRINCIPAL
            "Codigo",
            "NombreSitio",
            "Operador",
            "EstadoGlobal",
            "Moneda",
            "CategoriaSitio",
            "Tipo",
            "TipoEdificio",
            "TipoEstructura",
            "Tamano",
            "FechaActivacion",
            "FechaDesactivacion",

            // LOCALIZACION
            "Direccion",
            "Municipio",
            "Barrio",
            "CodigoPostal",
            "Latitud",
            "Longitud"
        };

        #endregion

        //public List<Vw_CoreEmplazamientosFiltros> GetActivos(long ClienteID)
        //{
        //    List<Vw_CoreEmplazamientosFiltros> lista = (from c in Context.Vw_CoreEmplazamientosFiltros select c).ToList();

        //    return lista;
        //}
        public bool CodigoDuplicadoGeneradorCodigos(string codigo)
        {
            List<Emplazamientos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Emplazamientos where c.Codigo == codigo /*&& (c.EmplazamientoTipoID == EmplazamientoTipo || c.EmplazamientoTipoID == null) && c.ClienteID == clienteID*/ select c).ToList();

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

        public bool RegistroDuplicado(string sCodigo, string sNombre, long lClienteID)
        {
            bool bExiste = false;
            List<Emplazamientos> listaDatos = new List<Emplazamientos>();

            listaDatos = (from c in Context.Emplazamientos
                          where (c.Codigo == sCodigo || c.NombreSitio == sNombre) && c.ClienteID == lClienteID
                          select c).ToList<Emplazamientos>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        //Crea y actualiza un emplazamiento
        internal ResponseCreateController CreateSite(bool bUpdate, long? emplazamientoID, Usuarios user, long clienteID, string codigo, string name, long operadorID, long estadoGlobalID,
                                                    long monedaID, long categoriaSitioID, long emplazamientoTipoID,
                                                    long? tipoEstructuraID, long tipoEdificioID,
                                                    long? tamanoID, DateTime activationDate, DateTime? deactivationDate,
                                                    long paisID, long municipioID, string address, string neighborhood,
                                                    string postalCode, double longitude, double latitude, List<object> listaAtributos = null, JsonObject listasCargadas = null)
        {
            Emplazamientos obj;
            ResponseCreateController result;
            InfoResponse response = null;
            object oAux;
            try
            {

                MunicipiosController cMunicipio = new MunicipiosController();
                EmplazamientosAtributosController cAtributos = new EmplazamientosAtributosController();
                EmplazamientosAtributosConfiguracionController cAtributosConf = new EmplazamientosAtributosConfiguracionController();
                EmplazamientosAtributosJsonController cAtrJson = new EmplazamientosAtributosJsonController();
                List<EmplazamientosAtributosJson> listaAtrJson = new List<EmplazamientosAtributosJson>();
                EmplazamientosAtributosJson oAtrJosn;
                Vw_CoreMunicipios municipio = cMunicipio.GetCoreMunicipioByMunicipioID(municipioID);
                string municipalidad = municipio.NombreMunicipio + ", " + municipio.NombreProvincia + ", (" + municipio.PaisCod + ")";
                cMunicipio = null;

                if (bUpdate)
                {
                    if (emplazamientoID.HasValue)
                    {
                        obj = GetItem(emplazamientoID.Value);
                        ActualizarHistorico(obj, user);
                    }
                    else
                    {
                        obj = null;
                    }
                }
                else
                {
                    obj = new Emplazamientos();
                    obj.ClienteID = clienteID;
                }

                #region JSON ATRIBUTOS

                if (listasCargadas == null)
                {
                    listasCargadas = new JsonObject();
                    foreach (var oAtr in cAtributosConf.GetAtributosFromCliente(clienteID))
                    {
                        if (oAtr.TiposDatos.Codigo == "LISTA" || oAtr.TiposDatos.Codigo == "LISTAMULTIPLE")
                        {
                            JsonObject jsonListaValores = new JsonObject();
                            if (oAtr.TablaModeloDatoID != null)
                            {
                                jsonListaValores = cAtributosConf.GetJsonItemsByColumnaModeloDatosID(oAtr.EmplazamientoAtributoConfiguracionID);
                            }
                            //else if (oConfAtributo.FuncionControlador != null && oConfAtributo.FuncionControlador != "")
                            //{
                            //    listaValores = cConfAtributo.GetListItemsComboBoxByFuncion(oConfAtributo.FuncionControlador, null, null, oConfAtributo.EmplazamientoAtributoConfiguracionID);
                            //}
                            else if (oAtr.ValoresPosibles != null && oAtr.ValoresPosibles != "")
                            {
                                foreach (var item in oAtr.ValoresPosibles.Split(';').ToList())
                                {
                                    if (!jsonListaValores.TryGetValue(item, out oAux))
                                    {
                                        JsonObject oJsonAux = new JsonObject();
                                        oJsonAux.Add("Text", item);
                                        oJsonAux.Add("Value", item);
                                        jsonListaValores.Add(item, oJsonAux);
                                    }
                                }
                            }
                            listasCargadas.Add(oAtr.NombreAtributo, jsonListaValores);
                        }
                    }
                }

                JsonObject json = new JsonObject();
                JsonObject jsonAux;
                try
                {
                    string descError;
                    bool bObligatorio;
                    foreach (var oAtr in listaAtributos)
                    {
                        var AtributoID = oAtr.GetType().GetProperty("AtributoID");
                        var NombreAtributo = oAtr.GetType().GetProperty("NombreAtributo");
                        var Valor = oAtr.GetType().GetProperty("Valor");
                        var TextLista = oAtr.GetType().GetProperty("TextLista");
                        var TipoDato = oAtr.GetType().GetProperty("TipoDato");
                        if ((oAtrJosn = cAtributos.SaveUpdateAtributo(out descError, clienteID, NombreAtributo.GetValue(oAtr).ToString(), Valor.GetValue(oAtr).ToString(), listasCargadas, out bObligatorio)) != null)
                        {
                            listaAtrJson.Add(oAtrJosn);
                        }
                        else
                        {
                            response = new InfoResponse
                            {
                                Result = false,
                                Code = ServicesCodes.SITES.COD_TBO_ATTRIBUTES_INVALID_ATTRIBUTES_CODE,
                                Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ATRIBUTTE_INVALID_FORMAT_DESCRIPTION + ": " + NombreAtributo.GetValue(oAtr).ToString() + ". " + descError
                            };
                            result = new ResponseCreateController(response, obj);
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    response = new InfoResponse
                    {
                        Result = false,
                        Code = ServicesCodes.SITES.COD_TBO_ATTRIBUTES_INVALID_ATTRIBUTES_CODE,
                        Description = ServicesCodes.SITES.COD_TBO_ATTRIBUTES_INVALID_ATTRIBUTES_DESCRIPTION
                    };
                    result = new ResponseCreateController(response, obj);
                    return result;
                }

                #endregion

                if (obj != null)
                {
                    obj.Codigo = codigo;
                    obj.NombreSitio = name;
                    obj.OperadorID = operadorID;
                    obj.EstadoGlobalID = estadoGlobalID;
                    obj.MonedaID = monedaID;
                    obj.CategoriaEmplazamientoID = categoriaSitioID;
                    obj.EmplazamientoTipoID = emplazamientoTipoID;
                    obj.EmplazamientoTipoEstructuraID = (tipoEstructuraID != 0) ? tipoEstructuraID : null;
                    obj.TipoEdificacionID = tipoEdificioID;
                    obj.EmplazamientoTamanoID = (tamanoID != 0) ? tamanoID : null;
                    obj.FechaActivacion = activationDate;
                    obj.FechaDesactivacion = deactivationDate;
                    obj.PaisID = paisID;
                    obj.MunicipioID = municipioID;
                    obj.Municipio = municipalidad;
                    obj.Direccion = address;
                    obj.Barrio = neighborhood;
                    obj.CodigoPostal = postalCode;
                    obj.Longitud = longitude;
                    obj.Latitud = latitude;
                    obj.JsonAtributosDinamicos = cAtrJson.Serializacion(listaAtrJson);
                    obj.UltimoUsuarioModificadorID = user.UsuarioID;
                    obj.FechaUltimaModificacion = DateTime.Now;

                    #region VALORES POR DEFECTO
                    obj.Propietario = "";
                    obj.Region = "";
                    obj.Provincia = "";
                    #endregion


                    if (bUpdate)
                    {
                        if (!UpdateItem(obj))
                        {
                            //FALLO AL ACTUALIZAR
                            response = new InfoResponse
                            {
                                Result = true,
                                Code = ServicesCodes.SITES.COD_TBO_ERROR_WHEN_UPDATING_CODE,
                                Description = ServicesCodes.SITES.COD_TBO_ERROR_WHEN_UPDATING_DESCRIPTION
                            };
                        }
                    }
                    else
                    {
                        obj.CreadorID = user.UsuarioID;
                        obj.FechaCreacion = DateTime.Now;
                        obj = AddItem(obj);
                        if (obj == null)
                        {
                            //FALLO AL GUARDAR
                            response = new InfoResponse
                            {
                                Result = true,
                                Code = ServicesCodes.SITES.COD_TBO_ERROR_WHEN_SAVING_CODE,
                                Description = ServicesCodes.SITES.COD_TBO_ERROR_WHEN_SAVING_DESCRIPTION
                            };
                        }
                    }

                    if (response == null)
                    {
                        response = new InfoResponse
                        {
                            Result = true,
                            Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                            Data = obj
                        };
                    }
                }
                else
                {
                    response = new InfoResponse()
                    {
                        Result = false,
                        Code = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_CODE,
                        Description = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_DESCRIPTION,
                        Data = null
                    };
                }

                result = new ResponseCreateController(response, obj);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                obj = null;

                response = new InfoResponse
                {
                    Result = false,
                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION + " " + ex.Message
                };
                result = new ResponseCreateController(response, obj);
            }

            return result;
        }
        internal ResponseCreateController CreateSite(bool bUpdate, long? emplazamientoID, Usuarios user, long clienteID, string codigo, string name, long operadorID, long estadoGlobalID,
                                                    long monedaID, long categoriaSitioID, long emplazamientoTipoID,
                                                    long? tipoEstructuraID, long tipoEdificioID,
                                                    long? tamanoID, DateTime activationDate, DateTime? deactivationDate,
                                                    long paisID, long municipioID, string address, string neighborhood,
                                                    string postalCode, double longitude, double latitude, List<object> listaAtributos, JsonObject listasCargadas, List<EmplazamientosAtributosConfiguracion> listaAtrConf, JsonObject listaRestri)
        {
            Emplazamientos obj;
            ResponseCreateController result;
            InfoResponse response = null;
            object oAux;
            try
            {

                MunicipiosController cMunicipio = new MunicipiosController();
                EmplazamientosAtributosController cAtributos = new EmplazamientosAtributosController();
                EmplazamientosAtributosConfiguracionController cAtributosConf = new EmplazamientosAtributosConfiguracionController();
                EmplazamientosAtributosJsonController cAtrJson = new EmplazamientosAtributosJsonController();
                List<EmplazamientosAtributosJson> listaAtrJson = new List<EmplazamientosAtributosJson>();
                EmplazamientosAtributosJson oAtrJosn;
                Vw_CoreMunicipios municipio = cMunicipio.GetCoreMunicipioByMunicipioID(municipioID);
                string municipalidad = municipio.NombreMunicipio + ", " + municipio.NombreProvincia + ", (" + municipio.PaisCod + ")";
                cMunicipio = null;

                if (bUpdate)
                {
                    if (emplazamientoID.HasValue)
                    {
                        obj = GetItem(emplazamientoID.Value);
                        ActualizarHistorico(obj, user);
                    }
                    else
                    {
                        obj = null;
                    }
                }
                else
                {
                    obj = new Emplazamientos();
                    obj.ClienteID = clienteID;
                }

                #region JSON ATRIBUTOS

                if (listasCargadas == null)
                {
                    listasCargadas = new JsonObject();
                    foreach (var oAtr in cAtributosConf.GetAtributosFromCliente(clienteID))
                    {
                        if (oAtr.TiposDatos.Codigo == "LISTA" || oAtr.TiposDatos.Codigo == "LISTAMULTIPLE")
                        {
                            JsonObject jsonListaValores = new JsonObject();
                            if (oAtr.TablaModeloDatoID != null)
                            {
                                jsonListaValores = cAtributosConf.GetJsonItemsByColumnaModeloDatosID(oAtr.EmplazamientoAtributoConfiguracionID);
                            }
                            //else if (oConfAtributo.FuncionControlador != null && oConfAtributo.FuncionControlador != "")
                            //{
                            //    listaValores = cConfAtributo.GetListItemsComboBoxByFuncion(oConfAtributo.FuncionControlador, null, null, oConfAtributo.EmplazamientoAtributoConfiguracionID);
                            //}
                            else if (oAtr.ValoresPosibles != null && oAtr.ValoresPosibles != "")
                            {
                                foreach (var item in oAtr.ValoresPosibles.Split(';').ToList())
                                {
                                    if (!jsonListaValores.TryGetValue(item, out oAux))
                                    {
                                        JsonObject oJsonAux = new JsonObject();
                                        oJsonAux.Add("Text", item);
                                        oJsonAux.Add("Value", item);
                                        jsonListaValores.Add(item, oJsonAux);
                                    }
                                }
                            }
                            listasCargadas.Add(oAtr.NombreAtributo, jsonListaValores);
                        }
                    }
                }

                JsonObject json = new JsonObject();
                JsonObject jsonAux;
                try
                {
                    bool bObligatorio;
                    string descError;
                    foreach (var oAtr in listaAtributos)
                    {
                        var AtributoID = oAtr.GetType().GetProperty("AtributoID");
                        var NombreAtributo = oAtr.GetType().GetProperty("NombreAtributo");
                        var Valor = oAtr.GetType().GetProperty("Valor");
                        var TextLista = oAtr.GetType().GetProperty("TextLista");
                        var TipoDato = oAtr.GetType().GetProperty("TipoDato");
                        if ((oAtrJosn = cAtributos.SaveUpdateAtributo(out descError, clienteID, NombreAtributo.GetValue(oAtr).ToString(), Valor.GetValue(oAtr).ToString(), listasCargadas, out bObligatorio)) != null)
                        {
                            listaAtrJson.Add(oAtrJosn);
                        }
                        else
                        {
                            response = new InfoResponse
                            {
                                Result = false,
                                Code = ServicesCodes.SITES.COD_TBO_ATTRIBUTES_INVALID_ATTRIBUTES_CODE,
                                Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ATRIBUTTE_INVALID_FORMAT_DESCRIPTION + ": " + NombreAtributo.GetValue(oAtr).ToString() + ". " + descError
                            };
                            result = new ResponseCreateController(response, obj);
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    response = new InfoResponse
                    {
                        Result = false,
                        Code = ServicesCodes.SITES.COD_TBO_ATTRIBUTES_INVALID_ATTRIBUTES_CODE,
                        Description = ServicesCodes.SITES.COD_TBO_ATTRIBUTES_INVALID_ATTRIBUTES_DESCRIPTION
                    };
                    result = new ResponseCreateController(response, obj);
                    return result;
                }

                #endregion

                if (obj != null)
                {
                    obj.Codigo = codigo;
                    obj.NombreSitio = name;
                    obj.OperadorID = operadorID;
                    obj.EstadoGlobalID = estadoGlobalID;
                    obj.MonedaID = monedaID;
                    obj.CategoriaEmplazamientoID = categoriaSitioID;
                    obj.EmplazamientoTipoID = emplazamientoTipoID;
                    obj.EmplazamientoTipoEstructuraID = (tipoEstructuraID != 0) ? tipoEstructuraID : null;
                    obj.TipoEdificacionID = tipoEdificioID;
                    obj.EmplazamientoTamanoID = (tamanoID != 0) ? tamanoID : null;
                    obj.FechaActivacion = activationDate;
                    obj.FechaDesactivacion = deactivationDate;
                    obj.PaisID = paisID;
                    obj.MunicipioID = municipioID;
                    obj.Municipio = municipalidad;
                    obj.Direccion = address;
                    obj.Barrio = neighborhood;
                    obj.CodigoPostal = postalCode;
                    obj.Longitud = longitude;
                    obj.Latitud = latitude;
                    obj.UltimoUsuarioModificadorID = user.UsuarioID;
                    obj.FechaUltimaModificacion = DateTime.Now;

                    if (bUpdate)
                    {
                        obj.JsonAtributosDinamicos = cAtrJson.Serializacion(listaAtrJson.Concat(cAtrJson.Deserializacion(obj.JsonAtributosDinamicos)).Distinct(new EmplazamientosAtributosJsonControllerComparer()).ToList());
                    }
                    else
                    {
                        obj.JsonAtributosDinamicos = cAtrJson.Serializacion(listaAtrJson);
                    }

                    #region VALORES POR DEFECTO
                    obj.Propietario = "";
                    obj.Region = "";
                    obj.Provincia = "";
                    #endregion


                    if (bUpdate)
                    {
                        if (!UpdateItem(obj))
                        {
                            //FALLO AL ACTUALIZAR
                            response = new InfoResponse
                            {
                                Result = true,
                                Code = ServicesCodes.SITES.COD_TBO_ERROR_WHEN_UPDATING_CODE,
                                Description = ServicesCodes.SITES.COD_TBO_ERROR_WHEN_UPDATING_DESCRIPTION
                            };
                        }
                    }
                    else
                    {
                        obj.CreadorID = user.UsuarioID;
                        obj.FechaCreacion = DateTime.Now;
                        obj = AddItem(obj);
                        if (obj != null)
                        {
                            //FALLO AL GUARDAR
                            response = new InfoResponse
                            {
                                Result = true,
                                Code = ServicesCodes.SITES.COD_TBO_ERROR_WHEN_SAVING_CODE,
                                Description = ServicesCodes.SITES.COD_TBO_ERROR_WHEN_SAVING_DESCRIPTION
                            };
                        }
                    }

                    if (response == null)
                    {
                        response = new InfoResponse
                        {
                            Result = true,
                            Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                            Data = obj
                        };
                    }
                }
                else
                {
                    response = new InfoResponse()
                    {
                        Result = false,
                        Code = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_CODE,
                        Description = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_DESCRIPTION,
                        Data = null
                    };
                }

                result = new ResponseCreateController(response, obj);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                obj = null;

                response = new InfoResponse
                {
                    Result = false,
                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION + " " + ex.Message
                };
                result = new ResponseCreateController(response, obj);
            }

            return result;
        }

        #region NOTIFICACIONES INTERFACE

        public string GetListaByFiltro(string sFiltro, string sVisualiza)
        {
            // Local variables
            List<Vw_Emplazamientos> lista = null;
            string sResultado = null;

            // Invokes the method
            try
            {
                lista = GetItemsList<Vw_Emplazamientos>(sFiltro);

                if (lista != null && lista.Count > 0)
                {

                    // Creates the header
                    object propertyValue = null;
                    List<string> cabecera = null;
                    cabecera = sVisualiza.Split(';').ToList();
                    sResultado = "<table style=\"border: 1px solid #999\">" + Comun.NuevaLinea;
                    sResultado = sResultado + "<tr>" + Comun.NuevaLinea;
                    if (cabecera != null && cabecera.Count > 0)
                    {
                        foreach (string nuevaCabecera in cabecera)
                        {
                            sResultado = sResultado + "<th style=\"font-weight: bold; background: #0061ac; color: white\">" + nuevaCabecera + "</th>" + Comun.NuevaLinea;
                        }

                        sResultado = sResultado + "</tr>" + Comun.NuevaLinea;

                        foreach (Vw_Emplazamientos nuevoObjeto in lista)
                        {
                            sResultado = sResultado + "<tr>" + Comun.NuevaLinea;
                            foreach (string nuevaCelda in cabecera)
                            {
                                sResultado = sResultado + "<th style=\"font-weight: bold; background: #0061ac; color: white\">";
                                // Reads the information
                                propertyValue = nuevoObjeto.GetType().GetProperty(nuevaCelda).GetValue(nuevoObjeto, null);
                                if (propertyValue != null)
                                {
                                    sResultado = sResultado + propertyValue.ToString();
                                }

                                sResultado = sResultado + "</th>" + Comun.NuevaLinea;
                            }
                            sResultado = sResultado + "</tr>" + Comun.NuevaLinea;
                        }
                    }
                    sResultado = sResultado + "</table>" + Comun.NuevaLinea;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sResultado = "";
            }

            // Returns the result
            return sResultado;

        }

        public List<Vw_EmplazamientosMapasCercanos> GetEmplazamientosMapasCercanosByRadio(double dLatitud, double dLongitud, double dRadio)
        {
            List<Vw_EmplazamientosMapasCercanos> lEmplazamientos = new List<Vw_EmplazamientosMapasCercanos>();
            List<Sp_EmplazamientosCercanosFotosMapa_GetResult> lEmplazamientosCercanos = GetAllPuntosCercanosConFotos(dLatitud, dLongitud, dRadio).ToList();

            foreach (Sp_EmplazamientosCercanosFotosMapa_GetResult empCerc in lEmplazamientosCercanos)
            {
                Vw_EmplazamientosMapasCercanos temp = new Vw_EmplazamientosMapasCercanos();

                foreach (System.Reflection.PropertyInfo propiedad in temp.GetType().GetProperties())
                {
                    dynamic valor = empCerc.GetType().GetProperty(propiedad.Name).GetValue(empCerc);
                    temp.GetType().GetProperty(propiedad.Name).SetValue(temp, valor, null);
                }

                lEmplazamientos.Add(temp);
            }

            return lEmplazamientos;
        }

        public Vw_EmplazamientosMapasCercanos GetEmplazamientobyID(long emplazamientoID)
        {
            Vw_EmplazamientosMapasCercanos lEmplazamientos = new Vw_EmplazamientosMapasCercanos();

            lEmplazamientos = (from c in Context.Vw_EmplazamientosMapasCercanos where c.EmplazamientoID == emplazamientoID select c).First();

            return lEmplazamientos;
        }

        #endregion

        public Vw_Emplazamientos GetPuntoCentro(double dLatitud, double dLongitud)
        {
            List<Vw_Emplazamientos> lEmplazamientosLista;
            Vw_Emplazamientos vCentro = new Vw_Emplazamientos();

            lEmplazamientosLista = (from c in Context.Vw_Emplazamientos where (c.Latitud == dLatitud && c.Longitud == dLongitud) select c).ToList();

            if (lEmplazamientosLista.Count > 0)
            {
                vCentro = lEmplazamientosLista.First();
            }

            return vCentro;
        }

        public List<Sp_EmplazamientosCercanosFotosMapa_GetResult> GetAllPuntosCercanosConFotos(double dLatitud, double dLongitud, double dDistancia)
        {
            List<Sp_EmplazamientosCercanosFotosMapa_GetResult> lEmplazamientosLista;

            lEmplazamientosLista = Context.Sp_EmplazamientosCercanosFotosMapa_Get(dLatitud, dLongitud, dDistancia).ToList();

            return lEmplazamientosLista;
        }

        public List<Sp_VisualizacionEmplazamientosResult1> VisualizacionEmplazamientos(string path, string filtros)
        {
            List<Sp_VisualizacionEmplazamientosResult1> lista;

            try
            {
                lista = Context.Sp_VisualizacionEmplazamientos(path, filtros).GetResult<Sp_VisualizacionEmplazamientosResult1>().ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }
            return lista;
        }

        public List<Sp_VisualizacionEmplazamientosResult2> VisualizacionEmplazamientosLocalizaciones(string path, string filtros)
        {
            List<Sp_VisualizacionEmplazamientosResult2> lista;

            try
            {
                lista = Context.Sp_VisualizacionEmplazamientos(path, filtros).GetResult<Sp_VisualizacionEmplazamientosResult2>().ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }
            return lista;
        }

        public List<Vw_EmplazamientosMapasCercanos> GetEmplazamientosMapasCercanosByClienteID(long clienteID)
        {
            List<Vw_EmplazamientosMapasCercanos> lista;
            try
            {
                lista = (from c
                         in Context.Vw_EmplazamientosMapasCercanos
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

        public long GetEmplazamientoTipoID(long emplazamientoID)
        {
            long emplazamientoTipoID;

            emplazamientoTipoID = (from c in Context.Emplazamientos where (c.EmplazamientoID == emplazamientoID) select c.EmplazamientoTipoID.Value).First();


            return emplazamientoTipoID;
        }

        public bool EmplazamientoDuplicadoCodigoOperador(long OperadorID, string strCodigo)
        {
            bool Existe = false;
            List<Emplazamientos> listaDatos;
            try
            {
                listaDatos = (from c in Context.Emplazamientos where c.Codigo == strCodigo && c.OperadorID == OperadorID select c).ToList();
                if (listaDatos != null && listaDatos.Count > 0)
                {
                    Existe = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Existe = true;
            }
            return Existe;
        }

        //OLD
        /*public DataTable Sp_VisualizacionEmplazamientos2(string tab, string where, long clienteID)
        {
            DataTable result;
            try
            {
#if SERVICESETTINGS
                using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]))
#elif TREEAPI
                using (var conn = new SqlConnection(TreeAPI.Properties.Settings.Default.Conexion))
#else
                using (var conn = new SqlConnection(TreeCore.Properties.Settings.Default.Conexion))
#endif

                using (var cmd = new SqlCommand("Sp_VisualizacionEmplazamientos2", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@tab", tab);
                    cmd.Parameters.AddWithValue("@filtros", where);
                    cmd.Parameters.AddWithValue("@clienteID", clienteID);

                    var da = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    da.Fill(ds);
                    result = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }*/


        public DataTable Sp_VisualizacionEmplazamientosNuevo(string tab, string whereGeneral, string whereInventario, string whereDocumentos, string filtroContactos, long clienteID,
            string colOrdenacion,
            int numPage,
            int tmPage,
            string dirOrd)
        {
            DataTable result;
            try
            {
#if SERVICESETTINGS
                using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["Conexion"]))
#elif TREEAPI
                using (var conn = new SqlConnection(TreeAPI.Properties.Settings.Default.Conexion))
#else
                using (var conn = new SqlConnection(TreeCore.Properties.Settings.Default.Conexion))
#endif

                using (var cmd = new SqlCommand("Sp_VisualizacionEmplazamientosNuevo", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@tab", tab);
                    cmd.Parameters.AddWithValue("@filtroGeneral", whereGeneral);
                    cmd.Parameters.AddWithValue("@filtroInventario", whereInventario);
                    cmd.Parameters.AddWithValue("@filtroDocumentos", whereDocumentos);
                    cmd.Parameters.AddWithValue("@filtroContactos", filtroContactos);
                    cmd.Parameters.AddWithValue("@clienteID", clienteID);
                    cmd.Parameters.AddWithValue("@colOrdenacion", colOrdenacion);
                    cmd.Parameters.AddWithValue("@numPagina", numPage);
                    cmd.Parameters.AddWithValue("@numRegistros", tmPage);
                    cmd.Parameters.AddWithValue("@DirOrd", dirOrd);

                    var da = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    da.Fill(ds);
                    result = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = null;
            }

            return result;
        }

        public Emplazamientos GetEmplazamientoByCodigo(string codigo, long ClienteID)
        {
            Emplazamientos emplazamiento = null;

            try
            {
                emplazamiento = (from c
                        in Context.Emplazamientos
                                 where c.ClienteID == ClienteID &&
                                         c.Codigo == codigo
                                 select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                emplazamiento = null;
            }

            return emplazamiento;
        }

        public bool ExistsSite(string codigo, long ClienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c
                            in Context.Emplazamientos
                          where c.ClienteID == ClienteID &&
                                  c.Codigo == codigo
                          select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }

            return existe;
        }

        public Emplazamientos GetByCodigo(string Codigo)
        {
            Emplazamientos emplazamiento;
            try
            {
                emplazamiento = (from c
                                 in Context.Emplazamientos
                                 where c.Codigo == Codigo
                                 select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                emplazamiento = null;
            }

            return emplazamiento;
        }

        public void ExportarModeloDatos(string archivo, string tab, List<string> filaTipoDato, List<string> filaValoresPosibles, List<string> filaCabecera)
        {
            using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(archivo, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                //Sheets
                var sheetPart = workbook.WorkbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
                var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = workbook.WorkbookPart.GetIdOfPart(sheetPart),
                    SheetId = 1,
                    Name = tab
                };

                sheets.Append(sheet);

                //Heading
                DocumentFormat.OpenXml.Spreadsheet.Row FilaTipoDato = new DocumentFormat.OpenXml.Spreadsheet.Row();
                DocumentFormat.OpenXml.Spreadsheet.Row FilaValoresPosibles = new DocumentFormat.OpenXml.Spreadsheet.Row();
                DocumentFormat.OpenXml.Spreadsheet.Row FilaCabecera = new DocumentFormat.OpenXml.Spreadsheet.Row();

                /*//Primera fila
                foreach (String tipoDato in filaTipoDato)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(tipoDato);
                    FilaTipoDato.AppendChild(cell);
                }

                //Segunda fila
                foreach (String valorPosible in filaValoresPosibles)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(valorPosible);
                    FilaValoresPosibles.AppendChild(cell);
                }*/

                //Segunda fila
                foreach (String cabecera in filaCabecera)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(cabecera);
                    FilaCabecera.AppendChild(cell);
                }

                //sheetData.AppendChild(FilaTipoDato);
                //sheetData.AppendChild(FilaValoresPosibles);
                sheetData.AppendChild(FilaCabecera);

            }
        }

        public void ExportarModeloDatosDinamicoFilas(string archivo, string tab, List<List<string>> filas, long cont)
        {
            using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(archivo, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                //Sheets
                var sheetPart = workbook.WorkbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
                var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = workbook.WorkbookPart.GetIdOfPart(sheetPart),
                    SheetId = (DocumentFormat.OpenXml.UInt32Value)cont,
                    Name = tab
                };

                sheets.Append(sheet);

                DocumentFormat.OpenXml.Spreadsheet.Row Fila;

                foreach (var lista in filas)
                {
                    Fila = new DocumentFormat.OpenXml.Spreadsheet.Row();
                    foreach (String valor in lista)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(valor);
                        Fila.AppendChild(cell);
                    }
                    sheetData.AppendChild(Fila);
                }

            }
        }
        public void ExportarModeloDatosDinamicoColumnas(string archivo, string tab, List<List<string>> Filas, long cont)
        {
            using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(archivo, true))
            {
                //Sheets
                var sheetPart = workbook.WorkbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
                var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = workbook.WorkbookPart.GetIdOfPart(sheetPart),
                    SheetId = (DocumentFormat.OpenXml.UInt32Value)cont,
                    Name = tab
                };

                sheets.Append(sheet);

                DocumentFormat.OpenXml.Spreadsheet.Row Fila;
                foreach (var col in Filas)
                {
                    Fila = new DocumentFormat.OpenXml.Spreadsheet.Row();
                    foreach (String valor in col)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(valor);
                        Fila.AppendChild(cell);
                    }
                    sheetData.AppendChild(Fila);
                }

            }
        }

        public void ExportarModeloDatosOpen(string archivo, string tab, List<string> filaTipoDato, List<string> filaValoresPosibles, List<string> filaCabecera, long cont)
        {
            using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(archivo, true))
            {
                //Sheets
                var sheetPart = workbook.WorkbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
                var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = workbook.WorkbookPart.GetIdOfPart(sheetPart),
                    SheetId = (DocumentFormat.OpenXml.UInt32Value)cont,
                    Name = tab
                };

                sheets.Append(sheet);

                //Heading
                DocumentFormat.OpenXml.Spreadsheet.Row FilaTipoDato = new DocumentFormat.OpenXml.Spreadsheet.Row();
                DocumentFormat.OpenXml.Spreadsheet.Row FilaValoresPosibles = new DocumentFormat.OpenXml.Spreadsheet.Row();
                DocumentFormat.OpenXml.Spreadsheet.Row FilaCabecera = new DocumentFormat.OpenXml.Spreadsheet.Row();

                //Primera fila
                foreach (String tipoDato in filaTipoDato)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(tipoDato);
                    FilaTipoDato.AppendChild(cell);
                }

                //Segunda fila
                foreach (String valorPosible in filaValoresPosibles)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(valorPosible);
                    FilaValoresPosibles.AppendChild(cell);
                }

                //Segunda fila
                foreach (String cabecera in filaCabecera)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(cabecera);
                    FilaCabecera.AppendChild(cell);
                }

                sheetData.AppendChild(FilaTipoDato);
                sheetData.AppendChild(FilaValoresPosibles);
                sheetData.AppendChild(FilaCabecera);

            }
        }

        #region HISTORICOS

        public Emplazamientos AddItemWithHistorical(Emplazamientos emplazamiento, Usuarios user, List<object> listaAtributos = null)
        {
            // Local variables
            Emplazamientos emplazamientoLocal = null;

            using (TransactionScope trans = new TransactionScope())
            {
                // Performs the operation
                try
                {
                    emplazamientoLocal = AddItem(emplazamiento);

                    if (emplazamientoLocal != null)
                    {
                        //#region ATRIBUTOS

                        //EmplazamientosAtributos oAtributos;
                        //EmplazamientosAtributosController cAtributos = new EmplazamientosAtributosController();
                        //cAtributos.SetDataContext(this.Context);

                        //if (listaAtributos != null)
                        //{
                        //    foreach (var item in listaAtributos)
                        //    {
                        //        var AtributoID = item.GetType().GetProperty("AtributoID");
                        //        var Valor = item.GetType().GetProperty("Valor");
                        //        oAtributos = cAtributos.GetAtributo(emplazamientoLocal.EmplazamientoID, long.Parse(AtributoID.GetValue(item).ToString()));
                        //        if (oAtributos == null)
                        //        {
                        //            oAtributos = new EmplazamientosAtributos
                        //            {
                        //                EmplazamientoID = emplazamientoLocal.EmplazamientoID,
                        //                EmplazamientoAtributoConfiguracionID = long.Parse(AtributoID.GetValue(item).ToString()),
                        //                Valor = Valor.GetValue(item).ToString()
                        //            };
                        //            cAtributos.AddItem(oAtributos);
                        //        }
                        //        else
                        //        {
                        //            oAtributos.Valor = Valor.GetValue(item).ToString();
                        //            cAtributos.UpdateItem(oAtributos);
                        //        }
                        //    }
                        //}

                        //#endregion

                        #region ADD HISTORICAL INFO

                        ActualizarHistorico(emplazamientoLocal, user);

                        #endregion

                        trans.Complete();
                    }
                    else
                    {
                        trans.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    trans.Dispose();
                    //Comun.cLog.EscribirLog("EmplazamientosController - AddItemWithHistorical: " + ex.Message);
                }

            }
            // Returns the result
            return emplazamientoLocal;
        }

        private bool ActualizarHistorico(Emplazamientos emplazamiento, Usuarios usuario)
        {
            bool bRes = false;

            try
            {
                HistoricosCoreEmplazamientosController cHistoricoEmplazamientos = new HistoricosCoreEmplazamientosController();
                cHistoricoEmplazamientos.SetDataContext(this.Context);
                HistoricosCoreEmplazamientos historico = new HistoricosCoreEmplazamientos();


                historico.EmplazamientoID = emplazamiento.EmplazamientoID;
                if (emplazamiento.UltimoUsuarioModificadorID.HasValue)
                {
                    historico.UsuarioID = (long)emplazamiento.UltimoUsuarioModificadorID;
                }
                else
                {
                    historico.UsuarioID = usuario.UsuarioID;
                }
                if (emplazamiento.FechaUltimaModificacion.HasValue)
                {
                    historico.FechaModificacion = (DateTime)emplazamiento.FechaUltimaModificacion;
                }
                else
                {
                    historico.FechaModificacion = DateTime.Now;
                }
                historico.DatosJSON = EmplazamientoToJSON(emplazamiento).ToString();
                if (cHistoricoEmplazamientos.AddItem(historico) != null)
                {
                    bRes = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

                bRes = false;

            }

            return bRes;
        }

        public bool UpdateItemWithHistorical(Emplazamientos emplazamiento, Usuarios user, List<object> listaAtributos = null)
        {
            // Local variables
            bool bResultado = false;

            // Performs the operation
            try
            {
                bResultado = UpdateItem(emplazamiento);
                if (bResultado)
                {
                    #region ADD HISTORICAL INFO

                    ActualizarHistorico(emplazamiento, user);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return bResultado;
        }

        private static string DateTimeToString(DateTime date)
        {
            string f = date.Day + "/" + ((date.Month < 10) ? "0" : "") + date.Month + "/" + date.Year;

            return f;
        }

        private JObject EmplazamientoToJSON(Emplazamientos emplazamiento)
        {
            EmplazamientosAtributosJsonController cEmplAtr = new EmplazamientosAtributosJsonController();
            JObject historicoJSON = new JObject();
            JObject historicoOrdenadoJSON = new JObject();
            List<EmplazamientosAtributosJson> atributosJSON = cEmplAtr.Deserializacion(emplazamiento.JsonAtributosDinamicos);
            UsuariosController cUsuarios = new UsuariosController();
            cUsuarios.SetDataContext(this.Context);


            Vw_CoreEmplazamientos vwEmplazamiento = null;
            vwEmplazamiento = GetVwEmplzamientoByID(emplazamiento.EmplazamientoID);

            Vw_CoreLocalizaciones vwEmplazamientoLoc = null;
            vwEmplazamientoLoc = GetVwEmplzamientoLocByID(emplazamiento.EmplazamientoID);

            try
            {
                if (emplazamiento.UltimoUsuarioModificadorID.HasValue)
                {
                    historicoJSON.Add("UsuarioModificador", cUsuarios.GetItem((long)emplazamiento.UltimoUsuarioModificadorID).NombreCompleto);
                }
                if (emplazamiento.FechaUltimaModificacion.HasValue)
                {
                    historicoJSON.Add("FechaModificacion", ((DateTime)emplazamiento.FechaUltimaModificacion).ToString(Comun.FORMATO_FECHA));
                }

                foreach (System.Reflection.PropertyInfo propiedad in vwEmplazamiento.GetType().GetProperties())
                {
#if SERVICES
                    if (Comun.CamposValidosEmplazamientosPrincipal.Contains(propiedad.Name) && !historicoJSON.ContainsKey(propiedad.Name))
#elif SERVICEIMPORTEXPORT
                    if (ComunServicios.CamposValidosEmplazamientosPrincipal.Contains(propiedad.Name) && !historicoJSON.ContainsKey(propiedad.Name))
#elif TREEAPI
                    if (Comun.CamposValidosEmplazamientosPrincipal.Contains(propiedad.Name) && !historicoJSON.ContainsKey(propiedad.Name))
#else
                    if (Comun.CamposValidosEmplazamientosPrincipal.Contains(propiedad.Name) && !historicoJSON.ContainsKey(propiedad.Name))
#endif
                    {

                        string temp = "";
                        if (propiedad.GetValue(vwEmplazamiento, null) != null)
                        {
                            if (propiedad.PropertyType == typeof(DateTime) || propiedad.PropertyType == typeof(System.Nullable<System.DateTime>))
                            {
                                temp = ((DateTime)propiedad.GetValue(vwEmplazamiento, null)).ToString(Comun.FORMATO_FECHA);
                            }
                            else
                            {
                                temp = propiedad.GetValue(vwEmplazamiento, null).ToString();
                            }
                        }

                        historicoJSON.Add(propiedad.Name, temp);
                    }
                }


                foreach (System.Reflection.PropertyInfo propiedad in vwEmplazamientoLoc.GetType().GetProperties())
                {
#if SERVICES
                    if (Comun.CamposValidosEmplazamientosLocalizacion.Contains(propiedad.Name) && !historicoJSON.ContainsKey(propiedad.Name))
#elif SERVICEIMPORTEXPORT
                    if (ComunServicios.CamposValidosEmplazamientosLocalizacion.Contains(propiedad.Name) && !historicoJSON.ContainsKey(propiedad.Name))
#elif TREEAPI
                    if (Comun.CamposValidosEmplazamientosLocalizacion.Contains(propiedad.Name) && !historicoJSON.ContainsKey(propiedad.Name))
#else
                    if (Comun.CamposValidosEmplazamientosLocalizacion.Contains(propiedad.Name) && !historicoJSON.ContainsKey(propiedad.Name))
#endif
                    {

                        string temp = "";
                        if (propiedad.GetValue(vwEmplazamientoLoc, null) != null)
                        {

                            if (propiedad.PropertyType == typeof(DateTime) || propiedad.PropertyType == typeof(System.Nullable<System.DateTime>))
                            {
                                temp = DateTimeToString((DateTime)propiedad.GetValue(vwEmplazamientoLoc, null));
                            }
                            else
                            {
                                temp = propiedad.GetValue(vwEmplazamientoLoc, null).ToString();
                            }
                        }

                        historicoJSON.Add(propiedad.Name, temp);
                    }
                }


                // ORDENAR VALORES JSON PARA QUE CUADREN EN LA TABLA TRADUCCIONES

#if SERVICES
                foreach (string sCampoValido in Comun.CamposValidosEmplazamientos)
                {

                    historicoOrdenadoJSON.Add(sCampoValido, historicoJSON[sCampoValido]);
                }
#elif SERVICEIMPORTEXPORT
                foreach (string sCampoValido in ComunServicios.CamposValidosEmplazamientos)
                {

                    historicoOrdenadoJSON.Add(sCampoValido, historicoJSON[sCampoValido]);
                }
#elif TREEAPI
                foreach (string sCampoValido in Comun.CamposValidosEmplazamientos)
                {

                    historicoOrdenadoJSON.Add(sCampoValido, historicoJSON[sCampoValido]);
                }
#else
                foreach (string sCampoValido in Comun.CamposValidosEmplazamientos)
                {

                    historicoOrdenadoJSON.Add(sCampoValido, historicoJSON[sCampoValido]);
                }
#endif

                foreach (var atributo in atributosJSON)
                {
                    historicoOrdenadoJSON.Add("Atr" + atributo.AtributoID, atributo.TextLista);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                historicoOrdenadoJSON = null;
            }

            return historicoOrdenadoJSON;
        }

        public Vw_CoreEmplazamientos GetVwEmplzamientoByID(long emplazamientoID)
        {
            Vw_CoreEmplazamientos res;
            try
            {
                res = (from c in Context.Vw_CoreEmplazamientos
                       where c.EmplazamientoID == emplazamientoID
                       select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res = null;
            }

            return res;
        }

        public Vw_CoreLocalizaciones GetVwEmplzamientoLocByID(long emplazamientoID)
        {
            Vw_CoreLocalizaciones res;
            try
            {
                res = (from c in Context.Vw_CoreLocalizaciones
                       where c.EmplazamientoID == emplazamientoID
                       select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res = null;
            }

            return res;
        }

        public List<String> ListContactosTipos()
        {
            return (from c in Context.ContactosTipos select c.ContactoTipo).ToList();
        }
        #endregion

        public Emplazamientos GetEmplazamientoByCodigoyOperadoryCliente(string codigo, string operador, long clienteID)
        {
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            Emplazamientos emplazamiento = new Emplazamientos();
            List<Emplazamientos> lEmplazamiento = new List<Emplazamientos>();

            OperadoresController cOperadores = new OperadoresController();
            long OperadorID = cOperadores.GetOperadorByNombre(operador);

            lEmplazamiento = (from c in Context.Emplazamientos where c.Codigo == codigo && c.OperadorID == OperadorID && c.ClienteID == clienteID select c).ToList();

            if (lEmplazamiento.Count > 0)
            {
                emplazamiento = lEmplazamiento[0];
            }
            else
            {
                emplazamiento = null;
            }


            return emplazamiento;

        }
        public Emplazamientos GetEmplazamientoByCodigoyCliente(string codigo, long clienteID)
        {
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            Emplazamientos emplazamiento = new Emplazamientos();
            List<Emplazamientos> lEmplazamiento = new List<Emplazamientos>();

            lEmplazamiento = (from c in Context.Emplazamientos where c.Codigo == codigo && c.ClienteID == clienteID select c).ToList();

            if (lEmplazamiento.Count > 0)
            {
                emplazamiento = lEmplazamiento[0];
            }
            else
            {
                emplazamiento = null;
            }


            return emplazamiento;

        }

        public EmplazamientosCamposAdicionales GetEmplazamientosCamposAdicionales(long EmplazamientoID)
        {
            EmplazamientosCamposAdicionales empAd = null;
            List<EmplazamientosCamposAdicionales> lista = null;

            lista = (from c in Context.EmplazamientosCamposAdicionales where c.EmplazamientoID == EmplazamientoID select c).ToList<EmplazamientosCamposAdicionales>();

            if (lista != null && lista.Count > 0)
            {
                empAd = lista.ElementAt(0);
            }

            return empAd;
        }

        public double ConvertirGrados2Decimal(string valor)
        {
            double res = 0;
            //normalmente viene: 41-23-25.839N	02-08-38.589E  (uno de los dos)
            try
            {

                if (valor.Contains("."))
                {
                    if (valor.StartsWith("-"))
                    {
                        //  tiene punto, pero no tiene guiones, es un valor decimal
                        valor = valor.Replace(".", ","); //para que al convertirlo a decimal tengamos la coma bien

                    }
                    else
                    {
                        if (!valor.Contains("-"))
                        {

                            valor = valor.Replace(".", ","); //para que al convertirlo a decimal tengamos la coma bien
                        }

                    }
                }

                if (!double.TryParse(valor, out res))
                {

                    //si no puede convertirlo es que son grados

                    string direccion = "";
                    double grados = 0;
                    double minutos = 0;
                    double segundos = 0;

                    direccion = valor.Substring(valor.Length - 1, 1);
                    string[] aux = valor.Split('.');
                    string[] aux2 = aux[0].Split('-');

                    grados = Convert.ToInt32(aux2[0]);
                    minutos = Convert.ToInt32(aux2[1]);
                    segundos = Convert.ToInt32(aux2[2]);

                    int signo = 1;

                    switch (direccion.ToUpper())
                    {
                        case "W":
                            signo = -1;
                            break;
                        case "S":
                            signo = -1;
                            break;
                        default:
                            signo = 1;
                            break;

                    }


                    double aux10 = (minutos * 60) / 3600;

                    double dato = (grados + aux10) * 1000000;

                    res = Math.Round(dato) / 1000000 * signo;

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                // no hace nada, devuelve 0
                res = 0;
            }

            return res;
        }
        public double ConvertirGrados2DecimalError(string valor)
        {
            double res = 0;
            //normalmente viene: 41-23-25.839N	02-08-38.589E  (uno de los dos)
            try
            {

                if (valor.Contains("."))
                {
                    if (valor.StartsWith("-"))
                    {
                        //  tiene punto, pero no tiene guiones, es un valor decimal
                        valor = valor.Replace(".", ","); //para que al convertirlo a decimal tengamos la coma bien

                    }
                    else
                    {
                        if (!valor.Contains("-"))
                        {

                            valor = valor.Replace(".", ","); //para que al convertirlo a decimal tengamos la coma bien
                        }

                    }
                }

                if (!double.TryParse(valor, out res))
                {

                    //si no puede convertirlo es que son grados

                    string direccion = "";
                    double grados = 0;
                    double minutos = 0;
                    double segundos = 0;

                    direccion = valor.Substring(valor.Length - 1, 1);
                    string[] aux = valor.Split('.');
                    string[] aux2 = aux[0].Split('-');

                    grados = Convert.ToInt32(aux2[0]);
                    minutos = Convert.ToInt32(aux2[1]);
                    segundos = Convert.ToInt32(aux2[2]);

                    int signo = 1;

                    switch (direccion.ToUpper())
                    {
                        case "W":
                            signo = -1;
                            break;
                        case "S":
                            signo = -1;
                            break;
                        default:
                            signo = 1;
                            break;

                    }


                    double aux10 = (minutos * 60) / 3600;

                    double dato = (grados + aux10) * 1000000;

                    res = Math.Round(dato) / 1000000 * signo;

                }
                else
                {
                    res = Convert.ToDouble(valor.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                // no hace nada, devuelve 0
                res = -200;
            }

            return res;
        }

        public double actualizarTotalRenegociable(long emplazamientoID)
        {
            double TotalRenegociable = 0;

            if (emplazamientoID > 0)
            {
                TotalRenegociable = (double)Context.Fn_GlobalRentaTotalRenegociable((int)emplazamientoID);
            }

            return TotalRenegociable;
        }

        public double actualizarTotalAreaRenegociable(long emplazamientoID)
        {
            double TotalAreaRenegociable = 0;

            if (emplazamientoID > 0)
            {
                TotalAreaRenegociable = (double)Context.Fn_GlobalAreaTotalRenegociable((int)emplazamientoID);
            }

            return TotalAreaRenegociable;
        }

        public double actualizarTotalValorAreaRenegociable(long emplazamientoID)
        {
            double TotalValorAreaRenegociable = 0;

            if (emplazamientoID > 0)
            {
                TotalValorAreaRenegociable = (double)Context.Fn_GlobalValorAreaTotalRenegociable((int)emplazamientoID);
            }

            return TotalValorAreaRenegociable;
        }

        public long GetEmplazamientoIDByCodigo(string codigo, long ClienteID)
        {
            long empID = 0;

            try
            {
                Emplazamientos dato = default(Emplazamientos);
                dato = GetItem("ClienteID ==" + ClienteID + " && Codigo ==\"" + codigo + "\"");

                if (dato != null)
                {
                    empID = dato.EmplazamientoID;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }


            return empID;
        }

        public List<Emplazamientos> GetEmplazamientoByOperadorAndCliente(string codigo, long clienteID, int pageSize, int curPage, string sCategoryAttribute)
        {
            List<Emplazamientos> list;

            OperadoresController cOperadores = new OperadoresController();

            try
            {
                bool EsClienteOperador = cOperadores.EsClienteOperador(codigo, clienteID);
                IQueryable<Emplazamientos> query;
                bool comprobarCategoria = (!string.IsNullOrEmpty(sCategoryAttribute));

                if (EsClienteOperador)
                {
                    query = (from empl in Context.Emplazamientos
                             join attrEmpl in Context.Vw_EmplazamientosAtributos on empl.EmplazamientoID equals attrEmpl.EmplazamientoID
                             join attCat in Context.EmplazamientosAtributosCategorias on attrEmpl.EmplazamientoAtributoCategoriaID equals attCat.EmplazamientoAtributoCategoriaID
                             where empl.ClienteID == clienteID && ((comprobarCategoria) ? attCat.Nombre == sCategoryAttribute : true)
                             select empl);
                }
                else
                {
                    query = (from empl in Context.Emplazamientos
                             join enti in Context.Entidades on empl.OperadorID equals enti.OperadorID
                             join oper in Context.Operadores on empl.OperadorID equals oper.OperadorID
                             join attrEmpl in Context.Vw_EmplazamientosAtributos on empl.EmplazamientoID equals attrEmpl.EmplazamientoID
                             join attCat in Context.EmplazamientosAtributosCategorias on attrEmpl.EmplazamientoAtributoCategoriaID equals attCat.EmplazamientoAtributoCategoriaID
                             where enti.Codigo == codigo && ((comprobarCategoria) ? attCat.Nombre == sCategoryAttribute : true)
                             select empl);
                }

                list = query.Skip(pageSize * curPage).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                list = new List<Emplazamientos>();
            }

            return list;
        }

        public int CountEmplazamientoByOperadorAndCliente(string codigo, long clienteID, string sCategoryAttribute)
        {
            int numItems;

            OperadoresController cOperadores = new OperadoresController();

            try
            {
                bool EsClienteOperador = cOperadores.EsClienteOperador(codigo, clienteID);
                IQueryable<Emplazamientos> query;
                bool comprobarCategoria = (!string.IsNullOrEmpty(sCategoryAttribute));

                if (EsClienteOperador)
                {
                    query = (from empl in Context.Emplazamientos
                             join attrEmpl in Context.Vw_EmplazamientosAtributos on empl.EmplazamientoID equals attrEmpl.EmplazamientoID
                             join attCat in Context.EmplazamientosAtributosCategorias on attrEmpl.EmplazamientoAtributoCategoriaID equals attCat.EmplazamientoAtributoCategoriaID
                             where
                                empl.ClienteID == clienteID &&
                                ((comprobarCategoria) ? attCat.Nombre == sCategoryAttribute : true)
                             select empl);
                }
                else
                {
                    query = (from empl in Context.Emplazamientos
                             join enti in Context.Entidades on empl.OperadorID equals enti.OperadorID
                             join oper in Context.Operadores on empl.OperadorID equals oper.OperadorID
                             join attrEmpl in Context.Vw_EmplazamientosAtributos on empl.EmplazamientoID equals attrEmpl.EmplazamientoID
                             join attCat in Context.EmplazamientosAtributosCategorias on attrEmpl.EmplazamientoAtributoCategoriaID equals attCat.EmplazamientoAtributoCategoriaID
                             where
                                empl.ClienteID == clienteID &&
                                enti.Codigo == codigo &&
                                ((comprobarCategoria) ? attCat.Nombre == sCategoryAttribute : true)
                             select empl);
                }

                numItems = query.Count();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                numItems = -1;
            }

            return numItems;
        }

        public Emplazamientos GetSiteByOperadorAndSiteCode(string codigo, string codigoEmplazamiento, long clienteID, string sCategoryAttribute)
        {
            Emplazamientos site;

            OperadoresController cOperadores = new OperadoresController();

            try
            {
                bool EsClienteOperador = cOperadores.EsClienteOperador(codigo, clienteID);
                bool comprobarCategoria = (!string.IsNullOrEmpty(sCategoryAttribute));

                if (EsClienteOperador)
                {
                    site = (from empl in Context.Emplazamientos
                            join attrEmpl in Context.Vw_EmplazamientosAtributos on empl.EmplazamientoID equals attrEmpl.EmplazamientoID
                            join attCat in Context.EmplazamientosAtributosCategorias on attrEmpl.EmplazamientoAtributoCategoriaID equals attCat.EmplazamientoAtributoCategoriaID
                            where empl.ClienteID == clienteID &&
                                    empl.Codigo == codigoEmplazamiento &&
                                    ((comprobarCategoria) ? attCat.Nombre == sCategoryAttribute : true)
                            select empl).First();
                }
                else
                {
                    site = (from empl in Context.Emplazamientos
                            join enti in Context.Entidades on empl.OperadorID equals enti.OperadorID
                            join oper in Context.Operadores on empl.OperadorID equals oper.OperadorID
                            join attrEmpl in Context.Vw_EmplazamientosAtributos on empl.EmplazamientoID equals attrEmpl.EmplazamientoID
                            join attCat in Context.EmplazamientosAtributosCategorias on attrEmpl.EmplazamientoAtributoCategoriaID equals attCat.EmplazamientoAtributoCategoriaID
                            where enti.Codigo == codigo &&
                                    empl.ClienteID == clienteID &&
                                    empl.Codigo == codigoEmplazamiento &&
                                    ((comprobarCategoria) ? attCat.Nombre == sCategoryAttribute : true)
                            select empl).First();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                site = null;
            }

            return site;
        }

        public int CountGetEmplazamientosByIDs(List<long> IDs, string sCustomer, long clienteID, string sCategoryAttribute)
        {
            int count;
            IQueryable<Emplazamientos> query;
            bool comprobarCategoria = (!string.IsNullOrEmpty(sCategoryAttribute));

            try
            {
                if (string.IsNullOrEmpty(sCustomer))
                {
                    query = (from empl in Context.Emplazamientos
                             where IDs.Contains(empl.EmplazamientoID) && empl.ClienteID == clienteID
                             join attrEmpl in Context.Vw_EmplazamientosAtributos on empl.EmplazamientoID equals attrEmpl.EmplazamientoID
                             join attCat in Context.EmplazamientosAtributosCategorias on attrEmpl.EmplazamientoAtributoCategoriaID equals attCat.EmplazamientoAtributoCategoriaID
                             where ((comprobarCategoria) ? attCat.Nombre == sCategoryAttribute : true)
                             select empl);
                }
                else
                {
                    query = (from empl in Context.Emplazamientos
                             join enti in Context.Entidades on empl.OperadorID equals enti.OperadorID
                             join oper in Context.Operadores on empl.OperadorID equals oper.OperadorID
                             join attrEmpl in Context.Vw_EmplazamientosAtributos on empl.EmplazamientoID equals attrEmpl.EmplazamientoID
                             join attCat in Context.EmplazamientosAtributosCategorias on attrEmpl.EmplazamientoAtributoCategoriaID equals attCat.EmplazamientoAtributoCategoriaID
                             where IDs.Contains(empl.EmplazamientoID) &&
                             empl.ClienteID == clienteID &&
                             enti.Codigo == sCustomer &&
                                    ((comprobarCategoria) ? attCat.Nombre == sCategoryAttribute : true)
                             select empl);
                }

                count = query.Count();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                count = -1;
            }
            return count;
        }

        public List<Emplazamientos> GetEmplazamientosByIDs(List<long> IDs, string sCustomer, long clienteID, int iPageSize, int iPage, string sCategoryAttribute)
        {
            List<Emplazamientos> sites;
            IQueryable<Emplazamientos> query;
            bool comprobarCategoria = (!string.IsNullOrEmpty(sCategoryAttribute));

            try
            {
                if (string.IsNullOrEmpty(sCustomer))
                {
                    query = (from empl in Context.Emplazamientos
                             where IDs.Contains(empl.EmplazamientoID) && empl.ClienteID == clienteID
                             join attrEmpl in Context.Vw_EmplazamientosAtributos on empl.EmplazamientoID equals attrEmpl.EmplazamientoID
                             join attCat in Context.EmplazamientosAtributosCategorias on attrEmpl.EmplazamientoAtributoCategoriaID equals attCat.EmplazamientoAtributoCategoriaID
                             where ((comprobarCategoria) ? attCat.Nombre == sCategoryAttribute : true)
                             select empl);
                }
                else
                {
                    query = (from empl in Context.Emplazamientos
                             join enti in Context.Entidades on empl.OperadorID equals enti.OperadorID
                             join oper in Context.Operadores on empl.OperadorID equals oper.OperadorID
                             join attrEmpl in Context.Vw_EmplazamientosAtributos on empl.EmplazamientoID equals attrEmpl.EmplazamientoID
                             join attCat in Context.EmplazamientosAtributosCategorias on attrEmpl.EmplazamientoAtributoCategoriaID equals attCat.EmplazamientoAtributoCategoriaID
                             where IDs.Contains(empl.EmplazamientoID) &&
                             empl.ClienteID == clienteID &&
                             enti.Codigo == sCustomer &&
                             ((comprobarCategoria) ? attCat.Nombre == sCategoryAttribute : true)
                             select empl);
                }

                sites = query.Skip(iPageSize * iPage).Take(iPageSize).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sites = new List<Emplazamientos>();
            }
            return sites;
        }

        #region POSICION
        public List<Sp_EmplazamientosCercanos_GetResult> GetAllPuntosCercanos(double latitud, double longitud, double distancia)
        {
            List<Sp_EmplazamientosCercanos_GetResult> EmplazamientosLista = new List<Sp_EmplazamientosCercanos_GetResult>();
            //Hay que Cambiar Sp_EmplazamientosByPrpyectos_Result Por Vw_Emplazamientos para homogeñar los resultados dentro de la Capa de Datos
            EmplazamientosLista = Context.Sp_EmplazamientosCercanos_Get(latitud, longitud, distancia).ToList();
            return EmplazamientosLista;
        }
        #endregion

        public JObject CrearJsonAtributo(EmplazamientosAtributos oAtr)
        {
            JObject json;
            try
            {
                json = new JObject();
                json.Add("AtributoID", oAtr.EmplazamientoAtributoID);
                json.Add("NombreAtributo", oAtr.EmplazamientosAtributosConfiguracion.NombreAtributo);
                json.Add("Valor", oAtr.Valor);
                json.Add("TextLista", oAtr.Valor);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                json = null;
            }
            return json;
        }

        #region Métodos Carga Información Grids

        public List<JsonObject> AplicarFiltroInterno(bool interno, string filtrosAplicados, int pageSize, int curPage, out int total, DataSorter[] sorters, string s, string clienteID,
            Hidden hdStringBuscador, Hidden hdIDEmplazamientoBuscador, string sResultadoKPIid, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos)
        {
            List<JsonObject> items = new List<JsonObject>();
            List<string> listaVacia = new List<string>();
            total = 0;

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDEmplazamientoBuscador.IsEmpty) ? Convert.ToInt64(hdIDEmplazamientoBuscador.Value) : new long?();

            JObject json = JObject.Parse(filtrosAplicados);
            string tabSelected = (string)json["tab"];
            bool visible = (bool)json["visible"];

            DataTable dt = RealizarConsulta(filtrosAplicados, clienteID, visible, textoBuscado, IdBuscado, sResultadoKPIid, s, ((sorters != null && sorters.Length > 0) ? sorters[0].Property : "Codigo"), curPage, pageSize, ((sorters != null && sorters.Length > 0) ? sorters[0].Direction.ToString() : "ASC"));

            #region Parsear segun tab

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    total = int.Parse(dt.Rows[0][dt.Columns.Count - 1].ToString());
                }
                else
                {
                    total = 0;

                }
            }
            else
            {
                total = 0;

            }

            switch (tabSelected)
            {
                case Comun.TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES:
                    List<JsonObject> localizaciones = new List<JsonObject>();
                    if (dt != null)
                    {
                        localizaciones = getListJsonObject(dt, bDescarga, columnModel, listaExcluidos);
                    }

                    #region Paginación, filtro y ordenación
                    items = localizaciones;
                    #endregion

                    break;
                case Comun.TABS_EMPLAZAMIENTO.TAB_CONTACTO:
                    List<JsonObject> contactos = new List<JsonObject>();
                    if (dt != null)
                    {
                        contactos = getListJsonObject(dt, bDescarga, columnModel, listaExcluidos);
                    }

                    #region Paginación, filtro y ordenación
                    items = contactos;
                    #endregion

                    break;
                case Comun.TABS_EMPLAZAMIENTO.TAB_MAP:
                    List<Vw_EmplazamientosMapasCercanos> maps = new List<Vw_EmplazamientosMapasCercanos>();
                    if (dt != null)
                    {
                        items = getListJsonObject(dt, bDescarga, columnModel, listaExcluidos);
                    }

                    #region Filtrado límites del mapa



                    #endregion

                    break;
                case Comun.TABS_EMPLAZAMIENTO.TAB_INVENTARIO:
                    List<JsonObject> inventario = new List<JsonObject>();
                    if (dt != null)
                    {
                        inventario = getListJsonObject(dt, bDescarga, columnModel, listaExcluidos);
                    }

                    #region Paginación, filtro y ordenación
                    items = inventario;
                    #endregion

                    break;
                case Comun.TABS_EMPLAZAMIENTO.TAB_DOCUMENTOS:
                    List<JsonObject> documentos = new List<JsonObject>();
                    if (dt != null)
                    {
                        documentos = getListJsonObjectDocumentosActivosYUltimaVersion(dt, bDescarga, columnModel, listaExcluidos);
                    }

                    #region Paginación, filtro y ordenación
                    items = documentos;
                    #endregion

                    break;
                case Comun.TABS_EMPLAZAMIENTO.TAB_ATRIBUTOS:
                    List<JsonObject> listaAtributos = new List<JsonObject>();
                    if (dt != null)
                    {
                        listaAtributos = getListAtributos(dt, long.Parse(clienteID), bDescarga, columnModel, listaExcluidos);
                    }

                    #region Paginación, filtro y ordenación
                    items = listaAtributos;
                    #endregion

                    break;
                case Comun.TABS_EMPLAZAMIENTO.TAB_SITE:
                default:
                    List<JsonObject> sites = new List<JsonObject>();
                    if (dt != null)
                    {
                        sites = getListJsonObject(dt, bDescarga, columnModel, listaExcluidos);
                    }

                    #region Paginación, filtro y ordenación
                    items = sites;
                    #endregion

                    break;
            }
            #endregion

            return items;
        }

        private static List<JsonObject> Filtro(string s, List<JsonObject> lista)
        {
            if (!string.IsNullOrEmpty(s))
            {
                lista = LinqEngine.filtroJson(lista, s);
            }

            return lista;
        }

        private static T getElementOfView<T>(T temp, DataRow row)
        {

            PropertyInfo[] propiedades = temp.GetType().GetProperties();
            foreach (PropertyInfo propiedad in propiedades)
            {
                try
                {
                    dynamic valor = row.Field<dynamic>(propiedad.Name);
                    temp.GetType().GetProperty(propiedad.Name).SetValue(temp, valor, null);
                }
                catch (Exception) { }
            }

            return temp;
        }

        private static List<JsonObject> getListJsonObject(DataTable oDataTable, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos)
        {
            List<JsonObject> listaDatos = new List<JsonObject>();
            JsonObject oJson;
            try
            {
                foreach (DataRow row in oDataTable.Rows)
                {
                    oJson = new JsonObject();

                    if (bDescarga && columnModel != null)
                    {
                        for (int i = 0; i < columnModel.Columns.Count; i++)
                        {
                            if (!listaExcluidos.Contains(columnModel.Columns[i].DataIndex))
                            {
                                if (columnModel.Columns[i].XType == "datecolumn")
                                {
                                    DateTime? fecha = (DateTime?)row.Field<DateTime?>(columnModel.Columns[i].DataIndex);
                                    if (fecha.HasValue)
                                    {
                                        oJson.Add(columnModel.Columns[i].DataIndex, Comun.DateTimeToString(fecha.Value));
                                    }
                                    else
                                    {
                                        oJson.Add(columnModel.Columns[i].DataIndex, row.Field<string>(columnModel.Columns[i].DataIndex));
                                    }
                                }
                                else
                                {
                                    oJson.Add(columnModel.Columns[i].DataIndex, row.Field<object>(columnModel.Columns[i].DataIndex));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < oDataTable.Columns.Count; i++)
                        {
                            oJson.Add(oDataTable.Columns[i].ToString(), row[i]);
                        }
                    }

                    listaDatos.Add(oJson);
                }

            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
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

        private static List<JsonObject> getListAtributos(DataTable oDataTable, long lClienteID, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos)
        {
            List<JsonObject> listaDatos = new List<JsonObject>();
            EmplazamientosAtributosConfiguracionController cAtributosConf = new EmplazamientosAtributosConfiguracionController();
            EmplazamientosAtributosJsonController cAtrJson = new EmplazamientosAtributosJsonController();
            JsonObject oJson;
            JObject jsonAux;
            object nombreAtributo, valorAtributo = "";
            EmplazamientosAtributosConfiguracion atr;
            try
            {

                #region Cargas Listas

                JsonObject listasItems = new JsonObject();
                JsonObject listaItems = new JsonObject();
                JsonObject auxJson;

                foreach (var oAtr in cAtributosConf.GetAtributosFromCliente(lClienteID))
                {
                    listaItems = new JsonObject();
                    if (oAtr.TablaModeloDatoID != null)
                    {
                        listaItems = cAtributosConf.GetJsonItemsComboBoxByColumnaModeloDatosID(oAtr.EmplazamientoAtributoConfiguracionID);
                    }
                    else if (oAtr.FuncionControlador != null && oAtr.FuncionControlador != "")
                    {
                        listaItems = cAtributosConf.GetJsonItemsComboBoxByFuncion(oAtr.FuncionControlador, null, null, oAtr.EmplazamientoAtributoConfiguracionID);
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
                    listasItems.Add(oAtr.EmplazamientoAtributoConfiguracionID.ToString(), listaItems);
                }

                #endregion

                foreach (DataRow row in oDataTable.Rows)
                {

                    oJson = new JsonObject();
                    if (bDescarga && columnModel != null)
                    {
                        for (int i = 0; i < columnModel.Columns.Count; i++)
                        {
                            if (!listaExcluidos.Contains(columnModel.Columns[i].DataIndex))
                            {
                                if (oDataTable.Columns.Contains(columnModel.Columns[i].DataIndex))
                                {
                                    oJson.Add(columnModel.Columns[i].DataIndex, row.Field<string>(columnModel.Columns[i].DataIndex));
                                }
                                else
                                {
                                    oJson.Add(columnModel.Columns[i].DataIndex, null);

                                    string sJson = row.Field<string>("jsonAtributosDinamicos");
                                    if (!string.IsNullOrEmpty(sJson))
                                    {
                                        foreach (var valor in cAtrJson.Deserializacion(sJson))
                                        {
                                            try
                                            {

                                                if (valor.TipoDato != null && (valor.TipoDato == Comun.TIPODATO_CODIGO_LISTA || valor.TipoDato == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE))
                                                {
                                                    if (columnModel.Columns[i].Text.Equals(valor.NombreAtributo))
                                                    {
                                                        string sValor = "";
                                                        dynamic auxDina;
                                                        if (listasItems.TryGetValue(valor.AtributoID.ToString(), out auxDina))
                                                        {
                                                            listaItems = (JsonObject)auxDina;
                                                            string Auxstr;
                                                            if (valor.Valor.ToString() != "")
                                                            {
                                                                foreach (var val in valor.Valor.ToString().Split(','))
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
                                                                sValor = valor.Valor.ToString();
                                                            }
                                                        }

                                                        oJson[columnModel.Columns[i].DataIndex] = sValor;
                                                    }
                                                }
                                                else if (valor.TipoDato != null && valor.TipoDato == Comun.TIPODATO_CODIGO_FECHA)
                                                {
                                                    if (columnModel.Columns[i].Text.Equals(valor.NombreAtributo))
                                                    {
                                                        oJson[columnModel.Columns[i].DataIndex] = Comun.DateTimeToString(DateTime.Parse(valor.Valor.ToString()));
                                                    }
                                                }
                                                else
                                                {
                                                    if (columnModel.Columns[i].Text.Equals(valor.NombreAtributo))
                                                    {
                                                        oJson[columnModel.Columns[i].DataIndex] = valor.Valor;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                log.Error(ex.Message);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < oDataTable.Columns.Count; i++)
                        {
                            if (oDataTable.Columns[i].ToString() != "jsonAtributosDinamicos")
                            {
                                oJson.Add(oDataTable.Columns[i].ToString(), row[i]);
                            }
                            else
                            {
                                if (row[i] != null && row[i].ToString() != "")
                                {
                                    foreach (var valor in cAtrJson.Deserializacion(row[i].ToString()))
                                    {
                                        try
                                        {
                                            if (valor.TipoDato != null && (valor.TipoDato == Comun.TIPODATO_CODIGO_LISTA || valor.TipoDato == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE))
                                            {
                                                string sValor = "";
                                                dynamic auxDina;
                                                if (listasItems.TryGetValue(valor.AtributoID.ToString(), out auxDina))
                                                {
                                                    listaItems = (JsonObject)auxDina;
                                                    string Auxstr;
                                                    if (valor.Valor.ToString() != "")
                                                    {
                                                        foreach (var val in valor.Valor.ToString().Split(','))
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
                                                        sValor = valor.Valor.ToString();
                                                    }
                                                }
                                                oJson.Add("Atr" + valor.AtributoID, sValor);
                                            }
                                            else if (valor.TipoDato != null && valor.TipoDato == Comun.TIPODATO_CODIGO_FECHA)
                                            {
                                                if (valor.NombreAtributo != null)
                                                {
                                                    oJson.Add("Atr" + valor.AtributoID, DateTime.Parse(valor.Valor.ToString(), CultureInfo.InvariantCulture));
                                                }
                                            }
                                            else
                                            {
                                                if (valor.NombreAtributo != null)
                                                {
                                                    if (valor != null)
                                                    {
                                                        oJson.Add("Atr" + valor.AtributoID, valor.Valor.ToString());
                                                    }
                                                    else
                                                    {
                                                        oJson.Add("Atr" + valor.AtributoID, valor.Valor.ToString());
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //jsonAux.Add(item.Key, item.Value);
                                            oJson.Add("Atr" + valor.AtributoID, valor.Valor.ToString());
                                        }
                                        //oJson.Add(valor.NombreAtributo.ToString(), valor.Valor.ToString());
                                    }
                                }
                            }
                        }
                    }
                    listaDatos.Add(oJson);
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
            }
            return listaDatos;
        }

        private List<JsonObject> getListJsonObjectDocumentosActivosYUltimaVersion(DataTable oDataTable, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos)
        {
            List<JsonObject> listaDatos = new List<JsonObject>();
            JsonObject oJson;
            try
            {
                foreach (DataRow row in oDataTable.Rows)
                {
                    oJson = new JsonObject();

                    if (bDescarga && columnModel.Columns != null)
                    {
                        for (int i = 0; i < columnModel.Columns.Count; i++)
                        {
                            if (!listaExcluidos.Contains(columnModel.Columns[i].DataIndex))
                            {
                                if (columnModel.Columns[i].XType == "datecolumn")
                                {
                                    DateTime? fecha = (DateTime?)row.Field<DateTime?>(columnModel.Columns[i].DataIndex);
                                    if (fecha.HasValue)
                                    {
                                        oJson.Add(columnModel.Columns[i].DataIndex, Comun.DateTimeToString(fecha.Value));
                                    }
                                    else
                                    {
                                        oJson.Add(columnModel.Columns[i].DataIndex, row.Field<string>(columnModel.Columns[i].DataIndex));
                                    }
                                }
                                else
                                {
                                    oJson.Add(columnModel.Columns[i].DataIndex, row.Field<object>(columnModel.Columns[i].DataIndex));
                                }
                            }
                        }

                        if (row.Field<bool?>("UltimaVersion").HasValue && row.Field<bool?>("UltimaVersion").Value &&
                            row.Field<bool?>("Activo").HasValue && row.Field<bool?>("Activo").Value)
                        {
                            listaDatos.Add(oJson);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < oDataTable.Columns.Count; i++)
                        {
                            oJson.Add(oDataTable.Columns[i].ToString(), row[i]);
                        }

                        if (oJson.ContainsKey("Activo") && oJson.ContainsKey("UltimaVersion") && oJson["Activo"].ToString().ToLower() == "true" && oJson["UltimaVersion"].ToString().ToLower() == "true")
                        {
                            listaDatos.Add(oJson);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        private static DataTable RealizarConsulta(string filtrosAplicados, string sClienteID, bool visible, string textoBuscado, long? IdBuscado, string sResultadoKPIid, string mapBounds,
            string colOrdenacion,
            int numPage,
            int tmPage,
            string dirOrd)
        {
            DataTable dt;
            try
            {
                string whereGeneral = string.Empty;
                string whereInventario = string.Empty;
                string whereDocumentos = string.Empty;
                string whereContactos = string.Empty;

                JObject json = JObject.Parse(filtrosAplicados);
                JArray arrayFiltrosAplicados = (JArray)json["items"];
                string tabSelected = (string)json["tab"];

                bool primerBloqueFiltro = true;

                #region Montar Where
                foreach (JObject filtroAplicado in arrayFiltrosAplicados)
                {
                    JArray arrayItemsFiltro = (JArray)filtroAplicado["filters"];
                    string queryTmpGeneral = string.Empty;
                    string queryTmpInventario = string.Empty;
                    string queryTmpDocumentos = string.Empty;
                    string queryTmpContactos = string.Empty;
                    bool primeraVueltaGeneral = true;
                    bool primeraVueltaInventario = true;
                    bool primeraVueltaDocumentos = true;
                    bool primeraVueltaContactos = true;

                    #region AND entre filtros
                    if (primerBloqueFiltro)
                    {
                        primerBloqueFiltro = false;
                    }
                    else 
                    {
                        if (!string.IsNullOrEmpty(whereGeneral))
                        {
                            queryTmpGeneral += " AND ";
                        }
                        if (!string.IsNullOrEmpty(whereInventario))
                        {
                            queryTmpInventario += " AND ";
                        }
                        if (!string.IsNullOrEmpty(whereDocumentos))
                        {
                            queryTmpDocumentos += " AND ";
                        }
                        if (!string.IsNullOrEmpty(whereContactos))
                        {
                            queryTmpContactos += " AND ";
                        }
                    }
                    #endregion

                    foreach (JObject itemFiltro in arrayItemsFiltro)
                    {
                        string id = (string)itemFiltro["Id"];
                        //string name = (string)itemFiltro["Name"];
                        string value = (string)itemFiltro["Value"];
                        bool multi = (bool)itemFiltro["multi"];
                        string operador = (string)itemFiltro["operator"];
                        string typeData = (string)itemFiltro["typeData"];
                        //string valueLabel = (string)itemFiltro["valueLabel"];


                        string tab = Comun.columTagCoreEmplazamientosFiltros[id].tab;
                        string nameHeaderVw = Comun.columTagCoreEmplazamientosFiltros[id].nameHeaderVwTab;
                        string nameHeaderVwFilter = Comun.columTagCoreEmplazamientosFiltros[id].nameHeaderVwFilter;
                        string vista = Comun.columTagCoreEmplazamientosFiltros[id].vista;

                        #region concatenación de filtros
                        switch (vista)
                        {
                            case Comun.VISTAS_EMPLAZAMIENTOS.INVENTARIO:
                                queryTmpInventario += (primeraVueltaInventario) ? "(" : " AND ";
                                primeraVueltaInventario = false;
                                break;
                            case Comun.VISTAS_EMPLAZAMIENTOS.CONTACTOS:
                                queryTmpContactos += (primeraVueltaContactos) ? "(" : " AND ";
                                primeraVueltaContactos = false;
                                break;
                            case Comun.VISTAS_EMPLAZAMIENTOS.DOCUMENTOS:
                                queryTmpDocumentos += (primeraVueltaDocumentos) ? "(" : " AND ";
                                primeraVueltaDocumentos = false;
                                break;
                            case Comun.VISTAS_EMPLAZAMIENTOS.GENERAL:
                            default:
                                queryTmpGeneral += (primeraVueltaGeneral) ? "(" : " AND ";
                                primeraVueltaGeneral = false;
                                break;
                        }
                        #endregion

                        string queryTmp = string.Empty;
                        if (multi)
                        {
                            string IDs = value.Replace(';', ',');
                            if (IDs.EndsWith(",", StringComparison.Ordinal))
                            {
                                IDs = IDs.Remove(IDs.Length - 1);
                            }

                            queryTmp += GetSentenceQueryMulti(nameHeaderVwFilter, IDs);

                            if (tab != Comun.TABS_EMPLAZAMIENTO.TAB_SITE && tab != Comun.TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES && tab == tabSelected)
                            {
                                queryTmp += ") AND (" + GetSentenceQueryMulti(nameHeaderVw, IDs);
                            }
                        }
                        else
                        {
                            queryTmp += GetSentenceQuery(nameHeaderVwFilter, operador, value, typeData);

                            if (tab != Comun.TABS_EMPLAZAMIENTO.TAB_SITE && tab != Comun.TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES && tab == tabSelected)
                            {
                                queryTmp += ") AND (" + GetSentenceQuery(nameHeaderVw, operador, value, typeData);
                            }
                        }

                        #region Concatenación del filtro
                        switch (vista)
                        {
                            case Comun.VISTAS_EMPLAZAMIENTOS.INVENTARIO:
                                queryTmpInventario += queryTmp;
                                break;
                            case Comun.VISTAS_EMPLAZAMIENTOS.CONTACTOS:
                                queryTmpContactos += queryTmp;
                                break;
                            case Comun.VISTAS_EMPLAZAMIENTOS.DOCUMENTOS:
                                queryTmpDocumentos += queryTmp;
                                break;
                            case Comun.VISTAS_EMPLAZAMIENTOS.GENERAL:
                            default:
                                queryTmpGeneral += queryTmp;
                                break;
                        }
                        #endregion
                    }

                    if (arrayItemsFiltro.Count > 0)
                    {
                        #region Concatenación del filtro
                        if (!string.IsNullOrEmpty(queryTmpInventario))
                        {
                            queryTmpInventario += ")";
                            whereInventario += queryTmpInventario;
                        }
                        if (!string.IsNullOrEmpty(queryTmpContactos))
                        {
                            queryTmpContactos += ")";
                            whereContactos += queryTmpContactos;
                        }
                        if (!string.IsNullOrEmpty(queryTmpDocumentos))
                        {
                            queryTmpDocumentos += ")";
                            whereDocumentos += queryTmpDocumentos;
                        }
                        if (!string.IsNullOrEmpty(queryTmpGeneral))
                        {
                            queryTmpGeneral += ")";
                            whereGeneral += queryTmpGeneral;
                        }
                        #endregion
                    }
                }

                //whereGeneral = addClienteIdToWhere(whereGeneral, sClienteID);

                whereGeneral = addMapBoundsFilter(tabSelected, whereGeneral, mapBounds);

                whereGeneral = addActivoContacto(tabSelected, whereGeneral);

                whereGeneral = addSitesVisibles(whereGeneral, visible);

                whereGeneral = addIdsResultados(whereGeneral, sResultadoKPIid);

                //whereDocumentos = addFiltroDocumentos(tabSelected, whereDocumentos);

                #region Buscador
                string outWhereInventario, outWhereDocumentos, outWhereContactos;

                whereGeneral = filtroBuscador(whereGeneral, whereInventario, whereDocumentos, whereContactos, tabSelected, textoBuscado, IdBuscado, out outWhereInventario, out outWhereDocumentos, out outWhereContactos);

                whereInventario = outWhereInventario;
                whereDocumentos = outWhereDocumentos;
                whereContactos = outWhereContactos;
                #endregion

                #endregion

                #region Realizar consulta
                EmplazamientosController cEmplazamientos = new EmplazamientosController();
                dt = cEmplazamientos.Sp_VisualizacionEmplazamientosNuevo(tabSelected, whereGeneral, whereInventario, whereDocumentos, whereContactos, long.Parse(sClienteID), colOrdenacion, numPage, tmPage, dirOrd);
                #endregion

            }
            catch (Exception ex)
            {
                dt = null;
                log.Error(ex);
            }
            return dt;
        }

        private static string filtroBuscador(string whereGeneral, string whereInventario, string whereDocumentos, string whereContactos, string tab, string textoBuscado, long? IdBuscado,
                   out string outWhereInventario, out string outWhereDocumentos, out string outWhereContactos)
        {
            outWhereInventario = whereInventario;
            outWhereDocumentos = whereDocumentos;
            outWhereContactos = whereContactos;

            if (textoBuscado != null || IdBuscado != null)
            {
                string separadorWhereGeneral = (!string.IsNullOrEmpty(whereGeneral)) ? " AND " : "";



                if (IdBuscado != null)
                {
                    whereGeneral += separadorWhereGeneral + "EmplazamientoID=" + IdBuscado;
                }
                else if (textoBuscado != null)
                {
                    switch (tab)
                    {
                        case Comun.TABS_EMPLAZAMIENTO.TAB_SITE:
                            whereGeneral += separadorWhereGeneral + "(Codigo LIKE '%" + textoBuscado + "%' OR NombreSitio LIKE '%" + textoBuscado + "%')";
                            break;
                        case Comun.TABS_EMPLAZAMIENTO.TAB_ATRIBUTOS:
                            whereGeneral += separadorWhereGeneral + "(Codigo LIKE '%" + textoBuscado + "%' OR NombreSitio LIKE '%" + textoBuscado + "%')";
                            break;
                        case Comun.TABS_EMPLAZAMIENTO.TAB_CONTACTO:
                            outWhereContactos += (!string.IsNullOrEmpty(outWhereContactos)) ? " OR " : "";
                            outWhereContactos += "(Codigo LIKE '%" + textoBuscado + "%' OR NombreSitio LIKE '%" + textoBuscado + "%' OR " +
                                            "Nombre LIKE '%" + textoBuscado + "%' OR Apellidos LIKE '%" + textoBuscado + "%' OR Email LIKE '%" + textoBuscado +
                                            "%' OR Direccion LIKE '%" + textoBuscado + "%' OR CP LIKE '%" + textoBuscado + "%' OR Telefono LIKE '%" + textoBuscado +
                                            "%' OR Telefono2 LIKE '%" + textoBuscado + "%')";
                            break;
                        case Comun.TABS_EMPLAZAMIENTO.TAB_DOCUMENTOS:
                            outWhereDocumentos += (!string.IsNullOrEmpty(outWhereDocumentos)) ? " OR " : "";
                            outWhereDocumentos += "(Codigo LIKE '%" + textoBuscado + "%' OR NombreSitio LIKE '%" + textoBuscado + "%' OR " +
                                                    "Documento LIKE '%" + textoBuscado + "%')";
                            break;
                        case Comun.TABS_EMPLAZAMIENTO.TAB_INVENTARIO:
                            outWhereInventario += (!string.IsNullOrEmpty(outWhereInventario)) ? " OR " : "";
                            outWhereInventario += "(Codigo LIKE '%" + textoBuscado + "%' OR NombreEmplazamiento LIKE '%" + textoBuscado + "%' OR " +
                                                    "NombreElemento LIKE '%" + textoBuscado + "%' OR NumeroInventario LIKE '%" + textoBuscado + "%')";
                            break;
                        case Comun.TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES:
                            whereGeneral += separadorWhereGeneral + "(Codigo LIKE '%" + textoBuscado + "%' OR NombreSitio LIKE '%" + textoBuscado + "%' OR " +
                                                                    "Direccion LIKE '%" + textoBuscado + "%' OR CodigoPostal LIKE '%" + textoBuscado + "%')";
                            break;
                        case Comun.TABS_EMPLAZAMIENTO.TAB_MAP:
                            break;
                        default:
                            break;
                    }

                }
            }

            return whereGeneral;
        }


        #region CONSTRUCCIÓN DE SENTENCIA WHERE
        private static string addClienteIdToWhere(string whereQuery, string ClienteID)
        {
            string operadorAnd = (string.IsNullOrEmpty(whereQuery) ? "" : operatorAND);

            string[] elements = { whereQuery, operadorAnd, "ClienteID=", ClienteID };
            return string.Concat(elements);
        }

        private static string addActivoContacto(string tab, string whereQuery)
        {
            string query = "";
            if (tab == Comun.TABS_EMPLAZAMIENTO.TAB_CONTACTO)
            {
                string operadorAnd = (string.IsNullOrEmpty(whereQuery) ? "" : operatorAND);

                string[] elements = { whereQuery, operadorAnd, "Activo=1" };
                query = string.Concat(elements);
            }
            else
            {
                query = whereQuery;
            }

            return query;
        }

        private static string addFiltroDocumentos(string tab, string whereDocumentos) {
            string query = "";

            if (tab == Comun.TABS_EMPLAZAMIENTO.TAB_DOCUMENTOS)
            {
                query = whereDocumentos + ((string.IsNullOrEmpty(whereDocumentos)) ? " " : " AND ") + "UltimaVersion=1";
            }
            else
            {
                query = whereDocumentos;
            }

            return query;
        }

        private static string addMapBoundsFilter(string tab, string whereQuery, string Bounds)
        {
            string query = "";
            if (tab == Comun.TABS_EMPLAZAMIENTO.TAB_MAP)
            {
                string operadorAnd = (string.IsNullOrEmpty(whereQuery) ? "" : operatorAND);

                string[] elements = { whereQuery, operadorAnd, Bounds };
                query = string.Concat(elements);
            }
            else
            {
                query = whereQuery;
            }

            return query;
        }

        public static string addIdsResultados(string whereQuery, string sResultadoKPIid)
        {
            string query = "";

            if (!string.IsNullOrEmpty(sResultadoKPIid) && sResultadoKPIid != "undefined")
            {
                DQKpisMonitoringController cDQKpisMonitoring = new DQKpisMonitoringController();
                long idDQKpiMonitoring = long.Parse(sResultadoKPIid);
                DQKpisMonitoring DQKpiMonitoring = cDQKpisMonitoring.GetItem(idDQKpiMonitoring);


                if (DQKpiMonitoring != null)
                {
                    List<long> listaIDs;
                    DQKpisController cDQKpis = new DQKpisController();
                    cDQKpis.ejecutarConsulta(DQKpiMonitoring.Filtro, out listaIDs);

                    string sIDs = "";
                    listaIDs.ForEach(id =>
                    {
                        sIDs += (!string.IsNullOrEmpty(sIDs) ? ", " : "") + id;
                    });

                    string operadorAnd = (string.IsNullOrEmpty(whereQuery) ? "" : operatorAND);
                    string[] elements = { whereQuery, operadorAnd, "EmplazamientoID", " IN(", sIDs, ") " };
                    query = string.Concat(elements);
                }
            }
            else
            {
                query = whereQuery;
            }

            return query;
        }

        private static string addSitesVisibles(string whereQuery, bool visible)
        {
            string query = "";
            string operadorAnd = (string.IsNullOrEmpty(whereQuery) ? "" : operatorAND);

            if (visible)
            {
                string[] elements = { whereQuery, operadorAnd, "Visible=1" };
                query = string.Concat(elements);
            }
            else
            {
                query = whereQuery;
            }

            return query;
        }

        private static string GetSentenceQuery(string columName, string operador, string valor, string tipoValor)
        {
            string sentence = string.Empty;

            if (string.IsNullOrEmpty(operador))
            {
                operador = Comun.OPERADOR_IGUAL;
            }

            Type type = Type.GetType(tipoValor);

            if (type == typeof(System.String))
            {
                string[] elements = { columName, " ", "LIKE", " '%", valor, "%'" };
                sentence = string.Concat(elements);
            }
            else if (tipoValor.Contains(typeof(System.DateTime).FullName))
            {
                //FORMAT(FechaActivacion, 'dd/MM/yyyy') = FORMAT(CONVERT(DATETIME, '01/01/2007'), 'dd/MM/yyyy')
                //FechaActivacion > CONVERT(DATETIME, '1/6/2021')
                string[] elements = { columName, " ", GetOperadorByString(operador), " CONVERT(datetime, '", valor, "', 103)" };
                sentence = string.Concat(elements);
            }
            else if (tipoValor.Contains(typeof(System.Int64).FullName) || tipoValor.Contains(typeof(System.Double).FullName))
            {
                string[] elements = { columName, " ", GetOperadorByString(operador), " ", valor };
                sentence = string.Concat(elements);
            }

            return sentence;
        }

        private static string GetSentenceQueryMulti(string columName, string IDs)
        {
            string sentence = string.Empty;

            string[] elements = { columName, " IN (", IDs, ")" };
            sentence = string.Concat(elements);

            return sentence;
        }

        private static string GetOperadorByString(string operador)
        {
            string operadorResultado;
            switch (operador)
            {
                case Comun.OPERADOR_MAYOR:
                    operadorResultado = ">";
                    break;
                case Comun.OPERADOR_MENOR:
                    operadorResultado = "<";
                    break;
                case Comun.OPERADOR_IGUAL:
                default:
                    operadorResultado = "=";
                    break;
            }
            return operadorResultado;
        }
        #endregion

        #endregion

    }
}