using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using TreeAPI.API.TBO.Interfaces;
using TreeAPI.DTO.Interfaces;
using TreeAPI.DTO.Salida.Global;
using TreeCore.Clases;
using TreeCore.Data;
using System.Reflection;


namespace TreeAPI.Controllers
{
    [RoutePrefix("api/Sites")]
    public class SitesController : ApiBaseController/*, ICollective*/
    {

        #region CONSTANTES
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string NAME_CLASS = "SitesController";
        #endregion

        #region COLLECTIVE INTERFACE

        #region _CI_Create
        /// <summary>
        /// Create or update a Site
        /// </summary>
        /// <param name="Element">Site</param>
        /// <param name="sUser">User email</param>
        /// <param name="bUpdate">False=Create the site. True=Update the site</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public TBOResponse _CI_Create(DTO.Entrada.Global.Sites Element, string sUser, bool bUpdate)
        {
            // Local variables
            TBOResponse response = null;
            bool exitoCamposVacios = true;
            bool exitoLongitudCampos = true;
            bool fechasValidas = true;
            string sMissing = "";
            string cambosExcedidos = "";
            string fechasNoValidas = "";
            string sAttrNoExists = "";

            //Controllers
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            EntidadesController cEntidades = new EntidadesController();
            EstadosGlobalesController cEstadosGlobales = new EstadosGlobalesController();
            MonedasController cMonedas = new MonedasController();
            EmplazamientosTiposController cEmplazamientosTipos = new EmplazamientosTiposController();
            EmplazamientosCategoriasSitiosController cEmplazamientosCategorias = new EmplazamientosCategoriasSitiosController();
            EmplazamientosTiposEstructurasController cEmplazamientosTiposEstructuras = new EmplazamientosTiposEstructurasController();
            EmplazamientosTiposEdificiosController cEmplazamientosTiposEdificios = new EmplazamientosTiposEdificiosController();
            EmplazamientosTamanosController cEmplazamientosTamanos = new EmplazamientosTamanosController();
            EmplazamientosAtributosController cEmplazamientosAtributos = new EmplazamientosAtributosController();
            EmplazamientosAtributosConfiguracionController cEmplazamientosAtributosConfiguracion = new EmplazamientosAtributosConfiguracionController();

            #region MONITORING_INICIAL
            MonitoringWSRegistrosController cMonitoring = new MonitoringWSRegistrosController();
            string sTipoProyecto = Comun.MODULOMANTENIMIENTO;
            string sServicio = Comun.INTEGRACION_SERVICIO_API;
            string sMetodo = Comun.TBO.Metodo.TBO_METODO_MAINTENANCE_INCIDENTS_CI_CREATE;
            string sComentarios = "Invocacion del método _CI_Create del TBO de Sites";

            bool bPropio = true;
            string UsuarioEmail = (Element != null) ? sUser : string.Empty;
            //string sParametroEntrada = GetInputParameter(sMetodo, UsuarioEmail, Element, "", "", "", "", "");
            string sParametroSalida = "";
            long? monitoringAlquilerID = null;
            long? monitoringEmplazamientoID = null;
            long? monitoringUsuarioID = null;
            long? monitoringClienteID = null;
            #endregion

            try
            {
                if (Element != null)
                {
                    #region COMPROBAR CAMPOS VACIOS

                    #region SITE
                    if (string.IsNullOrEmpty(sUser))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(sUser) + " ";
                    }
                    if (string.IsNullOrEmpty(Element.sCode))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(Element.sCode) + " ";
                    }
                    if (string.IsNullOrEmpty(Element.sName))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(Element.sName) + " ";
                    }
                    if (string.IsNullOrEmpty(Element.sCustomer))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(Element.sCustomer) + " ";
                    }
                    if (string.IsNullOrEmpty(Element.sGlobalState))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(Element.sGlobalState) + " ";
                    }
                    if (string.IsNullOrEmpty(Element.sCurrency))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(Element.sCurrency) + " ";
                    }
                    if (string.IsNullOrEmpty(Element.sCategory))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(Element.sCategory) + " ";
                    }
                    if (string.IsNullOrEmpty(Element.sSitesType))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(Element.sSitesType) + " ";
                    }
                    if (string.IsNullOrEmpty(Element.sStructureType))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(Element.sStructureType) + " ";
                    }
                    if (string.IsNullOrEmpty(Element.sBuildingType))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(Element.sBuildingType) + " ";
                    }
                    if (string.IsNullOrEmpty(Element.sSize))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(Element.sSize) + " ";
                    }
                    if (string.IsNullOrEmpty(Element.sActivationDate))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(Element.dActivationDate) + " ";
                    }
                    #endregion

                    #region ATRIBUTOS DINÁMICOS
                    this.SetUsuario(sUser);

                    if (user != null)
                    {
                        Dictionary<string, List<string>> attrObligatorios = cEmplazamientosAtributosConfiguracion.GetAtributosObligatorios(user.ClienteID.Value);

                        if (Element.Attributes != null)
                        {
                            foreach (DTO.Entrada.Global.SiteAttributes atributo in Element.Attributes)
                            {
                                if (string.IsNullOrEmpty(atributo.sAttributeCode))
                                {
                                    exitoCamposVacios = false;
                                    sMissing = sMissing + " - " + nameof(atributo.sAttributeCode) + " ";
                                }
                                if (string.IsNullOrEmpty(atributo.sAttributeValue))
                                {
                                    exitoCamposVacios = false;
                                    sMissing = sMissing + " - " + nameof(atributo.sAttributeValue) + " ";
                                }
                                if (string.IsNullOrEmpty(atributo.sAttributeCategory))
                                {
                                    exitoCamposVacios = false;
                                    sMissing = sMissing + " - " + nameof(atributo.sAttributeCategory) + " ";
                                }

                                if (!string.IsNullOrEmpty(atributo.sAttributeCode) &&
                                    !string.IsNullOrEmpty(atributo.sAttributeCategory) &&
                                    attrObligatorios.ContainsKey(atributo.sAttributeCategory))
                                {
                                    attrObligatorios[atributo.sAttributeCategory].Remove(atributo.sAttributeCode);

                                    if (attrObligatorios[atributo.sAttributeCategory].Count == 0)
                                    {
                                        attrObligatorios.Remove(atributo.sAttributeCategory);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region LOCALIZATION
                        if (string.IsNullOrEmpty(Element.sCountry))
                        {
                            exitoCamposVacios = false;
                            sMissing = sMissing + " - " + nameof(Element.sCountry) + " ";
                        }
                        if (string.IsNullOrEmpty(Element.sCountryRegion))
                        {
                            exitoCamposVacios = false;
                            sMissing = sMissing + " - " + nameof(Element.sCountryRegion) + " ";
                        }
                        if (string.IsNullOrEmpty(Element.sProvince))
                        {
                            exitoCamposVacios = false;
                            sMissing = sMissing + " - " + nameof(Element.sProvince) + " ";
                        }
                        if (string.IsNullOrEmpty(Element.sMunicipality))
                        {
                            exitoCamposVacios = false;
                            sMissing = sMissing + " - " + nameof(Element.sMunicipality) + " ";
                        }
                        if (string.IsNullOrEmpty(Element.sAddress))
                        {
                            exitoCamposVacios = false;
                            sMissing = sMissing + " - " + nameof(Element.sAddress) + " ";
                        }
                        if (!Element.dLatitude.HasValue)
                        {
                            exitoCamposVacios = false;
                            sMissing = sMissing + " - " + nameof(Element.dLatitude) + " ";
                        }
                        if (!Element.dLongitude.HasValue)
                        {
                            exitoCamposVacios = false;
                            sMissing = sMissing + " - " + nameof(Element.dLongitude) + " ";
                        }
                        #endregion

                        #endregion

                        if (exitoCamposVacios)
                        {

                            if (attrObligatorios.Count > 0)
                            {
                                string sAtributosObligatorios = "";
                                List<string> keyList = new List<string>(attrObligatorios.Keys);
                                keyList.ForEach(key =>
                                {
                                    attrObligatorios[key].ForEach(value =>
                                    {
                                        sAtributosObligatorios += (string.IsNullOrEmpty(sAtributosObligatorios) ? "'" : ", '") + value + "'('" + key + "')";

                                    });
                                });

                                response = new TBOResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.SITES.COD_TBO_MANDATORY_ATTRIBUTES_MISSING_CODE,
                                    Description = ServicesCodes.SITES.COD_TBO_MANDATORY_ATTRIBUTES_MISSING_DESCRIPTION + sAtributosObligatorios
                                };
                            }
                            else
                            {

                                #region COMPROBAR FECHAS
                                if (!DateIsValid(Element.dActivationDate))
                                {
                                    fechasValidas = false;
                                    fechasNoValidas = fechasNoValidas + " - " + nameof(Element.dActivationDate) + " ";
                                }
                                if ((Element.DeactivationDateDate.HasValue && Element.DeactivationDateDate == DateTime.MinValue) ||
                                    (!DateIsValid(Element.dDeactivationDate) && !string.IsNullOrEmpty(Element.dDeactivationDate)))
                                {
                                    fechasNoValidas = fechasNoValidas + " - " + nameof(Element.dDeactivationDate) + " ";
                                    fechasValidas = false;
                                }
                                #endregion

                                if (fechasValidas)
                                {

                                    #region COMPROBAR ATRIBUTOS DINÁMICOS

                                    if (Element.Attributes != null)
                                    {
                                        bool existenAtributos = true;

                                        #region COMPROBAR QUE LOS ATRIBUTOS EXISTAN
                                        Element.Attributes.ForEach(attr =>
                                        {
                                            bool codigoAttrValido = cEmplazamientosAtributosConfiguracion.GetAtributoByCodigo(user.ClienteID.Value, attr.sAttributeCode) != null;
                                            if (!codigoAttrValido)
                                            {
                                                sAttrNoExists += (string.IsNullOrEmpty(sAttrNoExists) ? "" : ", ") + attr.sAttributeCode;
                                                existenAtributos = false;
                                            }
                                        });
                                        #endregion

                                        if (existenAtributos)
                                        {
                                            #region ATRIBUTOS
                                            List<object> emplazamientosAtributos = new List<object>();
                                            string attrNoValidos = "";

                                            Element.Attributes.ForEach(attr =>
                                            {
                                                EmplazamientosAtributosConfiguracion empAtrConf = cEmplazamientosAtributosConfiguracion.GetByCodeAndCategory(attr.sAttributeCode, attr.sAttributeCategory, user.ClienteID.Value);
                                                if (empAtrConf != null)
                                                {
                                                    if (empAtrConf.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_FECHA && attr.sAttributeValue != "")
                                                    {
                                                        emplazamientosAtributos.Add(new
                                                        {
                                                            AtributoID = empAtrConf.EmplazamientoAtributoConfiguracionID,
                                                            NombreAtributo = empAtrConf.NombreAtributo,
                                                            Valor = DateTime.ParseExact(attr.sAttributeValue, Comun.FORMATO_FECHA, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture),
                                                            TextLista = attr.sAttributeValue
                                                        });

                                                    }
                                                    else
                                                    {
                                                        emplazamientosAtributos.Add(new
                                                        {
                                                            AtributoID = empAtrConf.EmplazamientoAtributoConfiguracionID,
                                                            NombreAtributo = empAtrConf.NombreAtributo,
                                                            Valor = attr.sAttributeValue,
                                                            TextLista = attr.sAttributeValue
                                                        });

                                                    }
                                                }
                                                //else
                                                //{
                                                //    attrNoValidos += (string.IsNullOrEmpty(attrNoValidos) ? "" : ", '") + attr.sAttributeCode + "'";
                                                //}
                                            });
                                            #endregion

                                            if (string.IsNullOrEmpty(attrNoValidos))
                                            {
                                                #region COMPROBAR LONGITUD CAMPOS

                                                #region SITE
                                                if (sUser != null && sUser.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.emailUsuario)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(sUser) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.emailUsuario + ") ";
                                                }
                                                if (Element.sCode != null && Element.sCode.Length > Comun.TBO.LENGTH_CAMPOS.SITE.codigo)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sCode) + "(" + Comun.TBO.LENGTH_CAMPOS.SITE.codigo + ") ";
                                                }
                                                if (Element.sName != null && Element.sName.Length > Comun.TBO.LENGTH_CAMPOS.SITE.nombreSitio)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sName) + "(" + Comun.TBO.LENGTH_CAMPOS.SITE.nombreSitio + ") ";
                                                }
                                                if (Element.sCustomer != null && Element.sCustomer.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.operador)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sCustomer) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.operador + ") ";
                                                }
                                                if (Element.sGlobalState != null && Element.sGlobalState.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.estadoGlobal)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sGlobalState) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.estadoGlobal + ") ";
                                                }
                                                if (Element.sCurrency != null && Element.sCurrency.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.moneda)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sCurrency) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.moneda + ") ";
                                                }
                                                if (Element.sCurrency != null && Element.sCategory.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.emplazamientosCategorias)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sCategory) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.emplazamientosCategorias + ") ";
                                                }
                                                if (Element.sSitesType != null && Element.sSitesType.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.tipoEmplazamiento)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sSitesType) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.tipoEmplazamiento + ") ";
                                                }
                                                if (Element.sStructureType != null && Element.sStructureType.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.tipoEstructura)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sStructureType) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.tipoEstructura + ") ";
                                                }
                                                if (Element.sBuildingType != null && Element.sBuildingType.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.tipoEdificio)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sBuildingType) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.tipoEdificio + ") ";
                                                }
                                                if (Element.sSize != null && Element.sSize.Length > Comun.TBO.LENGTH_CAMPOS.SITE.tamano)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sSize) + "(" + Comun.TBO.LENGTH_CAMPOS.SITE.tamano + ") ";
                                                }
                                                #endregion

                                                #region LOCATION
                                                if (Element.sCountry != null && Element.sCountry.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.pais)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sCountry) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.pais + ") ";
                                                }
                                                if (Element.sCountryRegion != null && Element.sCountryRegion.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.regionPais)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sCountryRegion) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.regionPais + ") ";
                                                }
                                                if (Element.sProvince != null && Element.sProvince.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.provincia)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sProvince) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.provincia + ") ";
                                                }
                                                if (Element.sMunicipality != null && Element.sMunicipality.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.municipio)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sMunicipality) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.municipio + ") ";
                                                }
                                                if (Element.sAddress != null && Element.sAddress.Length > Comun.TBO.LENGTH_CAMPOS.SITE.direccion)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sAddress) + "(" + Comun.TBO.LENGTH_CAMPOS.SITE.direccion + ") ";
                                                }
                                                if (Element.sNeighborhood != null && Element.sNeighborhood.Length > Comun.TBO.LENGTH_CAMPOS.SITE.barrio)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sNeighborhood) + "(" + Comun.TBO.LENGTH_CAMPOS.SITE.barrio + ") ";
                                                }
                                                if (Element.sPostalCode != null && Element.sPostalCode.Length > Comun.TBO.LENGTH_CAMPOS.SITE.codigoPostal)
                                                {
                                                    exitoLongitudCampos = false;
                                                    cambosExcedidos += " - " + nameof(Element.sPostalCode) + "(" + Comun.TBO.LENGTH_CAMPOS.SITE.codigoPostal + ") ";
                                                }
                                                #endregion

                                                #endregion

                                                if (exitoLongitudCampos)
                                                {

                                                    #region COMPROBACIÓN CAMPOS BBDD
                                                    Operadores operador = cEntidades.GetActivoOperador(Element.sCustomer, user.ClienteID.Value);
                                                    EstadosGlobales EstadoGlobal = cEstadosGlobales.GetActivoEstadoGlobal(Element.sGlobalState, user.ClienteID.Value);
                                                    Monedas moneda = cMonedas.GetActivoMoneda(Element.sCurrency, user.ClienteID.Value);
                                                    EmplazamientosCategoriasSitios categoria = cEmplazamientosCategorias.GetActivoCategoria(Element.sCategory, user.ClienteID.Value);
                                                    EmplazamientosTipos EmplazamientoTipo = cEmplazamientosTipos.GetActivoEmplazamientoTipo(Element.sSitesType, user.ClienteID.Value);
                                                    EmplazamientosTiposEstructuras TipoEstructura = cEmplazamientosTiposEstructuras.GetActivoTipoEstructura(Element.sStructureType, user.ClienteID.Value);
                                                    EmplazamientosTiposEdificios TipoEdificio = cEmplazamientosTiposEdificios.GetActivoTipoEdificio(Element.sBuildingType, user.ClienteID.Value);
                                                    EmplazamientosTamanos tamano = cEmplazamientosTamanos.GetActivoTamano(Element.sSize, user.ClienteID.Value);

                                                    bool hasLocalizacion = this.ComprobarLocalizacion(user.ClienteID.Value, Element.sRegion, Element.sCountry, Element.sCountryRegion, Element.sProvince, Element.sMunicipality);

                                                    if (operador == null)
                                                    {
                                                        response = new TBOResponse
                                                        {
                                                            Result = false,
                                                            Code = ServicesCodes.GENERIC.COD_TBO_CUSTOMER_NOT_FOUND_CODE,
                                                            Description = ServicesCodes.GENERIC.COD_TBO_CUSTOMER_NOT_FOUND_DESCRIPTION
                                                        };
                                                    }
                                                    else if (EstadoGlobal == null)
                                                    {
                                                        response = new TBOResponse
                                                        {
                                                            Result = false,
                                                            Code = ServicesCodes.SITES.COD_TBO_GLOBAL_STATE_NOT_FOUND_CODE,
                                                            Description = ServicesCodes.SITES.COD_TBO_GLOBAL_STATE_NOT_FOUND_DESCRIPTION
                                                        };
                                                    }
                                                    else if (moneda == null)
                                                    {
                                                        response = new TBOResponse
                                                        {
                                                            Result = false,
                                                            Code = ServicesCodes.GENERIC.COD_TBO_CURRENCY_NOT_FOUND_CODE,
                                                            Description = ServicesCodes.GENERIC.COD_TBO_CURRENCY_NOT_FOUND_DESCRIPTION
                                                        };
                                                    }
                                                    else if (categoria == null)
                                                    {
                                                        response = new TBOResponse
                                                        {
                                                            Result = false,
                                                            Code = ServicesCodes.SITES.COD_TBO_CATEGORY_NOT_FOUND_CODE,
                                                            Description = ServicesCodes.SITES.COD_TBO_CATEGORY_NOT_FOUND_DESCRIPTION
                                                        };
                                                    }
                                                    else if (EmplazamientoTipo == null)
                                                    {
                                                        response = new TBOResponse
                                                        {
                                                            Result = false,
                                                            Code = ServicesCodes.SITES.COD_TBO_SITE_TYPE_NOT_FOUND_CODE,
                                                            Description = ServicesCodes.SITES.COD_TBO_SITE_TYPE_NOT_FOUND_DESCRIPTION
                                                        };
                                                    }
                                                    else if (TipoEstructura == null)
                                                    {
                                                        response = new TBOResponse
                                                        {
                                                            Result = false,
                                                            Code = ServicesCodes.SITES.COD_TBO_STRUCTURE_TYPE_NOT_FOUND_CODE,
                                                            Description = ServicesCodes.SITES.COD_TBO_STRUCTURE_TYPE_NOT_FOUND_DESCRIPTION
                                                        };
                                                    }
                                                    else if (TipoEdificio == null)
                                                    {
                                                        response = new TBOResponse
                                                        {
                                                            Result = false,
                                                            Code = ServicesCodes.SITES.COD_TBO_BUILDING_TYPE_NOT_FOUND_CODE,
                                                            Description = ServicesCodes.SITES.COD_TBO_BUILDING_TYPE_NOT_FOUND_DESCRIPTION
                                                        };
                                                    }
                                                    else if (tamano == null)
                                                    {
                                                        response = new TBOResponse
                                                        {
                                                            Result = false,
                                                            Code = ServicesCodes.SITES.COD_TBO_SIZE_NOT_FOUND_CODE,
                                                            Description = ServicesCodes.SITES.COD_TBO_SIZE_NOT_FOUND_DESCRIPTION
                                                        };
                                                    }
                                                    else if (!hasLocalizacion)
                                                    {
                                                        response = new TBOResponse
                                                        {
                                                            Result = false,
                                                            Code = ServicesCodes.GENERIC.COD_TBO_LOCATION_NOT_FOUND_CODE,
                                                            Description = ServicesCodes.GENERIC.COD_TBO_LOCATION_NOT_FOUND_DESCRIPTION + concatLocalizacion(Element.sRegion, Element.sCountry, Element.sCountryRegion, Element.sProvince, Element.sMunicipality)
                                                        };
                                                    }
                                                    #endregion

                                                    if (response == null)
                                                    {
                                                        bool existeSite = cEmplazamientos.ExistsSite(Element.sCode, user.ClienteID.Value);
                                                        if (bUpdate)
                                                        {
                                                            if (existeSite)
                                                            {
                                                                //Actualizar emplazamiento
                                                                Emplazamientos emp = cEmplazamientos.GetEmplazamientoByCodigo(Element.sCode, user.ClienteID.Value);

                                                                ResponseCreateController responseCreate = cEmplazamientos.CreateSite(bUpdate, emp.EmplazamientoID, user, user.ClienteID.Value, Element.sCode, Element.sName, operador.OperadorID,
                                                                                            EstadoGlobal.EstadoGlobalID, moneda.MonedaID, categoria.EmplazamientoCategoriaSitioID,
                                                                                            EmplazamientoTipo.EmplazamientoTipoID, TipoEstructura.EmplazamientoTipoEstructuraID, TipoEdificio.EmplazamientoTipoEdificioID,
                                                                                            tamano.EmplazamientoTamanoID, Element.ActivationDateDate.Value,
                                                                                            Element.DeactivationDateDate, pais.PaisID, municipio.MunicipioID, Element.sAddress,
                                                                                            Element.sNeighborhood, Element.sPostalCode, Element.dLongitude.Value, Element.dLatitude.Value, emplazamientosAtributos);

                                                                if (responseCreate.Data != null)
                                                                {
                                                                    Emplazamientos empl = (Emplazamientos)responseCreate.Data;

                                                                    response = new TBOResponse()
                                                                    {
                                                                        Result = true,
                                                                        Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                                                                        Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                                                                        Data = ConvertToSite((Emplazamientos)responseCreate.Data, false, "")
                                                                    };
                                                                }
                                                                else
                                                                {
                                                                    response = new TBOResponse()
                                                                    {
                                                                        Result = responseCreate.infoResponse.Result,
                                                                        Code = responseCreate.infoResponse.Code,
                                                                        Description = responseCreate.infoResponse.Description
                                                                    };
                                                                }
                                                            }
                                                            else if (!existeSite)
                                                            {
                                                                //ERROR No existe el emplazamieto
                                                                response = new TBOResponse
                                                                {
                                                                    Result = false,
                                                                    Code = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_CODE,
                                                                    Description = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_DESCRIPTION
                                                                };
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (!existeSite)
                                                            {
                                                                //Crear emplazamiento
                                                                ResponseCreateController responseCreate = cEmplazamientos.CreateSite(bUpdate, null, user, user.ClienteID.Value, Element.sCode, Element.sName, operador.OperadorID,
                                                                                            EstadoGlobal.EstadoGlobalID, moneda.MonedaID, categoria.EmplazamientoCategoriaSitioID,
                                                                                            EmplazamientoTipo.EmplazamientoTipoID, TipoEstructura.EmplazamientoTipoEstructuraID, TipoEdificio.EmplazamientoTipoEdificioID,
                                                                                            tamano.EmplazamientoTamanoID, Element.ActivationDateDate.Value,
                                                                                            Element.DeactivationDateDate, pais.PaisID, municipio.MunicipioID, Element.sAddress,
                                                                                            Element.sNeighborhood, Element.sPostalCode, Element.dLongitude.Value, Element.dLatitude.Value, emplazamientosAtributos);

                                                                if (responseCreate.Data != null)
                                                                {
                                                                    response = new TBOResponse()
                                                                    {
                                                                        Result = true,
                                                                        Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                                                                        Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                                                                        Data = ConvertToSite((Emplazamientos)responseCreate.Data, false, "")
                                                                    };
                                                                }
                                                                else
                                                                {
                                                                    response = new TBOResponse()
                                                                    {
                                                                        Result = responseCreate.infoResponse.Result,
                                                                        Code = responseCreate.infoResponse.Code,
                                                                        Description = responseCreate.infoResponse.Description
                                                                    };
                                                                }
                                                            }
                                                            else if (existeSite)
                                                            {
                                                                //ERROR El emplazamiento ya existe
                                                                response = new TBOResponse
                                                                {
                                                                    Result = false,
                                                                    Code = ServicesCodes.SITES.COD_TBO_SITE_EXISTS_CODE,
                                                                    Description = ServicesCodes.SITES.COD_TBO_SITE_EXISTS_DESCRIPTION
                                                                };
                                                            }
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    response = new TBOResponse
                                                    {
                                                        Result = false,
                                                        Code = ServicesCodes.GENERIC.COD_TBO_LENGTH_DATA_EXCEEDS_CODE,
                                                        Description = ServicesCodes.GENERIC.COD_TBO_LENGTH_DATA_EXCEEDS_DESCRIPTION + cambosExcedidos
                                                    };
                                                }
                                            }
                                            else
                                            {
                                                response = new TBOResponse
                                                {
                                                    Result = false,
                                                    Code = ServicesCodes.SITES.COD_TBO_ATTRIBUTES_INVALID_ATTRIBUTES_CODE,
                                                    Description = ServicesCodes.SITES.COD_TBO_ATTRIBUTES_INVALID_ATTRIBUTES_DESCRIPTION + attrNoValidos
                                                };
                                            }
                                        }
                                        else
                                        {
                                            response = new TBOResponse
                                            {
                                                Result = false,
                                                Code = ServicesCodes.SITES.COD_TBO_ATTRIBUTES_NOT_FOUND_CODE,
                                                Description = ServicesCodes.SITES.COD_TBO_ATTRIBUTES_NOT_FOUND_DESCRIPTION + sAttrNoExists
                                            };
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    response = new TBOResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.GENERIC.COD_TBO_INCORRECT_DATE_FORMAT_CODE,
                                        Description = ServicesCodes.GENERIC.COD_TBO_INCORRECT_DATE_FORMAT_DESCRIPTION + fechasNoValidas
                                    };
                                }
                            }
                        }
                        else
                        {
                            response = new TBOResponse
                            {
                                Result = false,
                                Code = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_CODE,
                                Description = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_DESCRIPTION + sMissing
                            };
                        }
                    }
                    else
                    {
                        response = new TBOResponse
                        {
                            Result = false,
                            Code = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_DESCRIPTION
                        };
                    }
                }
                else
                {
                    response = new TBOResponse
                    {
                        Result = false,
                        Code = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_CODE,
                        Description = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_DESCRIPTION + " - " + nameof(Element)
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                response = new TBOResponse
                {
                    Result = false,
                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                };
            }

            return response;
        }
        #endregion

        #endregion

        #region COLLECTIVE INTERFACE

        #region _SI_GetElements
        /// <summary>
        /// Get sites by customer
        /// </summary>
        /// <param name="sCustomer">Entity</param>
        /// <param name="sUser">User email</param>
        /// <param name="sConnectionCode">Conection code</param>
        /// <param name="iPage">Page</param>
        /// <param name="iPageSize">Page size</param>
        /// <param name="sCategoryAttribute">Category attribute</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public TBOResponse _SI_GetAllByOperator(string sCustomer, string sUser, string sConnectionCode, [FromUri] int? iPage = 0,
            [FromUri] int? iPageSize = Comun.TBO.TBO_LIST_MAXIMUM_SIZE, [FromUri] string sCategoryAttribute = "")
        {
            // Local variables
            TBOResponse response = null;
            List<Emplazamientos> listaEmplazamientos;
            bool bExito = true;
            string sMissing = string.Empty;

            //Controllers
            OperadoresController cOperadores = new OperadoresController();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            EmplazamientosAtributosController cEmplazamientosAtributos = new EmplazamientosAtributosController();

            #region ASIGNA VALORES POR DEFECTO A NULOS
            if (!iPage.HasValue)
            {
                iPage = 0;
            }
            if (!iPageSize.HasValue)
            {
                iPageSize = Comun.TBO.TBO_LIST_MAXIMUM_SIZE;
            }
            #endregion

            try
            {
                #region COMPROBAR CAMPOS VACIOS
                if (string.IsNullOrEmpty(sCustomer))
                {
                    bExito = false;
                    sMissing = sMissing + " - " + nameof(sCustomer) + " ";
                }
                if (string.IsNullOrEmpty(sUser))
                {
                    bExito = false;
                    sMissing = sMissing + " - " + nameof(sUser) + " ";
                }
                if (string.IsNullOrEmpty(sConnectionCode))
                {
                    bExito = false;
                    sMissing = sMissing + " - " + nameof(sConnectionCode) + " ";
                }
                #endregion

                if (bExito)
                {
                    this.SetUsuario(sUser);

                    #region Comprobar campos en BBDD
                    if (!cOperadores.HasOperadorCodigoConexion(sCustomer, sConnectionCode))
                    {
                        response = new TBOResponse()
                        {
                            Result = false,
                            Code = ServicesCodes.GENERIC.COD_TBO_CUSTOMER_WITH_CONEXION_CODE_NOT_FOUND_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_CUSTOMER_WITH_CONEXION_CODE_NOT_FOUND_DESCRIPTION
                        };
                    }
                    else if (this.user == null)
                    {
                        response = new TBOResponse()
                        {
                            Result = false,
                            Code = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_DESCRIPTION
                        };
                    }
                    else if (!string.IsNullOrEmpty(sCategoryAttribute) && !cEmplazamientosAtributos.HasByNombre(sCategoryAttribute, user.ClienteID.Value))
                    {
                        response = new TBOResponse()
                        {
                            Result = false,
                            Code = ServicesCodes.SITES.COD_TBO_ATTRIBUTE_CATEGORY_NOT_FOUND_CODE,
                            Description = ServicesCodes.SITES.COD_TBO_ATTRIBUTE_CATEGORY_NOT_FOUND_DESCRIPTION
                        };
                    }
                    #endregion

                    if (response == null)
                    {
                        listaEmplazamientos = cEmplazamientos.GetEmplazamientoByOperadorAndCliente(sCustomer, user.ClienteID.Value, iPageSize.Value, iPage.Value, sCategoryAttribute);

                        List<DTO.Salida.Global.Sites> listResult = new List<DTO.Salida.Global.Sites>();

                        if (listaEmplazamientos != null && listaEmplazamientos.Count > 0)
                        {
                            for (int i = 0; i < listaEmplazamientos.Count; i++)
                            {
                                try
                                {
                                    Emplazamientos emplazamiento = listaEmplazamientos[i];
                                    listResult.Add(ConvertToSite(emplazamiento, true, sCategoryAttribute));
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }

                        response = new TBOResponse
                        {
                            Result = true,
                            Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                            Data = new ListReturnTreeObject<DTO.Salida.Global.Sites>()
                            {
                                iPage = iPage.Value,
                                iPageSize = iPageSize.Value,
                                iTotalSize = cEmplazamientos.CountEmplazamientoByOperadorAndCliente(sCustomer, user.ClienteID.Value, sCategoryAttribute),
                                list = listResult
                            }
                        };
                    }
                }
                else
                {
                    response = new TBOResponse
                    {
                        Result = false,
                        Code = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_CODE,
                        Description = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_DESCRIPTION + sMissing
                    };
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                response = new TBOResponse
                {
                    Result = false,
                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                };
            }

            return response;
        }
        #endregion

        #region _SI_GetByCode
        /// <summary>
        /// Get site by code
        /// </summary>
        /// <param name="sCustomer">Entity</param>
        /// <param name="sUser">User email</param>
        /// <param name="sConnectionCode">Conection code</param>
        /// <param name="sSiteCode">Site code</param>
        /// <param name="sCategoryAttribute">Category attribute</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{sSiteCode}")]
        public TBOResponse _SI_GetByCode(string sCustomer, string sUser, string sConnectionCode, string sSiteCode,
            [FromUri] string sCategoryAttribute = "")
        {
            //Variables
            TBOResponse response = null;
            bool bExito = true;
            string sMissing = string.Empty;

            //Controllers
            OperadoresController cOperadores = new OperadoresController();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            EmplazamientosAtributosController cEmplazamientosAtributos = new EmplazamientosAtributosController();

            try
            {
                #region COMPROBAR CAMPOS VACIOS
                if (string.IsNullOrEmpty(sCustomer))
                {
                    bExito = false;
                    sMissing = sMissing + " - " + nameof(sCustomer) + " ";
                }
                if (string.IsNullOrEmpty(sUser))
                {
                    bExito = false;
                    sMissing = sMissing + " - " + nameof(sUser) + " ";
                }
                if (string.IsNullOrEmpty(sConnectionCode))
                {
                    bExito = false;
                    sMissing = sMissing + " - " + nameof(sConnectionCode) + " ";
                }
                if (string.IsNullOrEmpty(sSiteCode))
                {
                    bExito = false;
                    sMissing = sMissing + " - " + nameof(sSiteCode) + " ";
                }
                #endregion

                if (bExito)
                {
                    this.SetUsuario(sUser);

                    #region Comprobar campos en BBDD
                    if (!cOperadores.HasOperadorCodigoConexion(sCustomer, sConnectionCode))
                    {
                        response = new TBOResponse()
                        {
                            Result = false,
                            Code = ServicesCodes.GENERIC.COD_TBO_CUSTOMER_WITH_CONEXION_CODE_NOT_FOUND_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_CUSTOMER_WITH_CONEXION_CODE_NOT_FOUND_DESCRIPTION
                        };
                    }
                    else if (this.user == null)
                    {
                        response = new TBOResponse()
                        {
                            Result = false,
                            Code = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_DESCRIPTION
                        };
                    }
                    else if (!string.IsNullOrEmpty(sCategoryAttribute) && !cEmplazamientosAtributos.HasByNombre(sCategoryAttribute, user.ClienteID.Value))
                    {
                        response = new TBOResponse()
                        {
                            Result = false,
                            Code = ServicesCodes.SITES.COD_TBO_ATTRIBUTE_CATEGORY_NOT_FOUND_CODE,
                            Description = ServicesCodes.SITES.COD_TBO_ATTRIBUTE_CATEGORY_NOT_FOUND_DESCRIPTION
                        };
                    }
                    #endregion

                    if (response == null)
                    {
                        Emplazamientos emplazamiento = cEmplazamientos.GetSiteByOperadorAndSiteCode(sCustomer, sSiteCode, user.ClienteID.Value, sCategoryAttribute);

                        if (emplazamiento != null)
                        {
                            response = new TBOResponse()
                            {
                                Result = true,
                                Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                                Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                                Data = ConvertToSite(emplazamiento, false, sCategoryAttribute)
                            };
                        }
                        else
                        {
                            response = new TBOResponse()
                            {
                                Result = false,
                                Code = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_CODE,
                                Description = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_DESCRIPTION
                            };
                        }
                    }
                }
                else
                {
                    response = new TBOResponse
                    {
                        Result = false,
                        Code = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_CODE,
                        Description = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_DESCRIPTION + sMissing
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                response = new TBOResponse
                {
                    Result = false,
                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                };
            }
            return response;
        }
        #endregion

        #region _SI_GetLastModified
        /// <summary>
        /// Get the latest modified sites
        /// </summary>
        /// <param name="iDays">Days ago</param>
        /// <param name="sCustomer">Entity</param>
        /// <param name="sUser">User email</param>
        /// <param name="iPage">Page</param>
        /// <param name="iPageSize">Page size</param>
        /// <param name="sCategoryAttribute">Category Attribute</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLastModified")]
        public TBOResponse _SI_GetLastModified(int? iDays, string sUser, [FromUri] string sCustomer = "", [FromUri] int? iPage = 0,
            [FromUri] int? iPageSize = Comun.TBO.TBO_LIST_MAXIMUM_SIZE, [FromUri] string sCategoryAttribute = "")
        {
            // Local variables
            TBOResponse response = null;
            List<Emplazamientos> listaEmplazamientos;
            bool bExito = true;
            string sMissing = string.Empty;

            //Controllers
            EntidadesController cEntidades = new EntidadesController();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            HistoricosCoreEmplazamientosController cHistoricosCoreEmplazamientos = new HistoricosCoreEmplazamientosController();
            EmplazamientosAtributosController cEmplazamientosAtributos = new EmplazamientosAtributosController();

            #region ASIGNA VALORES POR DEFECTO A NULOS
            if (!iPage.HasValue)
            {
                iPage = 0;
            }
            if (!iPageSize.HasValue)
            {
                iPageSize = Comun.TBO.TBO_LIST_MAXIMUM_SIZE;
            }
            #endregion

            try
            {
                #region COMPROBAR CAMPOS VACIOS
                if (!iDays.HasValue)
                {
                    bExito = false;
                    sMissing = sMissing + " - " + nameof(iDays) + " ";
                }
                if (string.IsNullOrEmpty(sUser))
                {
                    bExito = false;
                    sMissing = sMissing + " - " + nameof(sUser) + " ";
                }
                #endregion

                if (bExito)
                {
                    this.SetUsuario(sUser);

                    if (this.user != null)
                    {
                        if (iDays.HasValue && iDays.Value <= Comun.TBO.MAX_DAYS_VALID_LAST_MODIFIED)
                        {

                            #region Comprobar campos en BBDD
                            if (!string.IsNullOrEmpty(sCustomer) && cEntidades.GetActivoOperador(sCustomer, user.ClienteID.Value) == null)
                            {
                                response = new TBOResponse()
                                {
                                    Result = false,
                                    Code = ServicesCodes.GENERIC.COD_TBO_CUSTOMER_NOT_FOUND_CODE,
                                    Description = ServicesCodes.GENERIC.COD_TBO_CUSTOMER_NOT_FOUND_DESCRIPTION
                                };
                            }
                            else if (this.user == null)
                            {
                                response = new TBOResponse()
                                {
                                    Result = false,
                                    Code = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_CODE,
                                    Description = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_DESCRIPTION
                                };
                            }
                            else if (!string.IsNullOrEmpty(sCategoryAttribute) && !cEmplazamientosAtributos.HasByNombre(sCategoryAttribute, user.ClienteID.Value))
                            {
                                response = new TBOResponse()
                                {
                                    Result = false,
                                    Code = ServicesCodes.SITES.COD_TBO_ATTRIBUTE_CATEGORY_NOT_FOUND_CODE,
                                    Description = ServicesCodes.SITES.COD_TBO_ATTRIBUTE_CATEGORY_NOT_FOUND_DESCRIPTION
                                };
                            }
                            #endregion

                            if (response == null)
                            {
                                List<long> IDs = cHistoricosCoreEmplazamientos.GetLastModified(iDays.Value, user.ClienteID.Value);

                                listaEmplazamientos = cEmplazamientos.GetEmplazamientosByIDs(IDs, sCustomer, user.ClienteID.Value, iPageSize.Value, iPage.Value, sCategoryAttribute);

                                List<DTO.Salida.Global.Sites> listResult = new List<DTO.Salida.Global.Sites>();

                                if (listaEmplazamientos != null && listaEmplazamientos.Count > 0)
                                {
                                    for (int i = 0; i < listaEmplazamientos.Count; i++)
                                    {
                                        try
                                        {
                                            Emplazamientos emplazamiento = listaEmplazamientos[i];
                                            listResult.Add(ConvertToSiteCodeName(emplazamiento));
                                        }
                                        catch (Exception)
                                        {

                                        }
                                    }
                                }

                                response = new TBOResponse
                                {
                                    Result = true,
                                    Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                                    Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                                    Data = new ListReturnTreeObject<DTO.Salida.Global.Sites>()
                                    {
                                        iPage = iPage.Value,
                                        iPageSize = iPageSize.Value,
                                        iTotalSize = cEmplazamientos.CountGetEmplazamientosByIDs(IDs, sCustomer, user.ClienteID.Value, sCategoryAttribute),
                                        list = listResult
                                    }
                                };
                            }
                        }
                        else
                        {
                            response = new TBOResponse
                            {
                                Result = false,
                                Code = ServicesCodes.GENERIC.COD_TBO_RANGE_DAYS_NOT_VALID_CODE,
                                Description = ServicesCodes.GENERIC.COD_TBO_RANGE_DAYS_NOT_VALID_DESCRIPTION + "(" + Comun.TBO.MAX_DAYS_VALID_LAST_MODIFIED + ")"
                            };
                        }
                    }
                    else
                    {
                        response = new TBOResponse
                        {
                            Result = false,
                            Code = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_DESCRIPTION
                        };
                    }
                }
                else
                {
                    response = new TBOResponse
                    {
                        Result = false,
                        Code = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_CODE,
                        Description = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_DESCRIPTION + sMissing
                    };
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                response = new TBOResponse
                {
                    Result = false,
                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION
                };
            }

            return response;
        }
        #endregion

        #endregion

        #region SINGULAR INTERFACE

        #region ConvertToSite
        private DTO.Salida.Global.Sites ConvertToSite(Emplazamientos emplazamiento, bool reducido, string sCategoryAttribute)
        {
            DTO.Salida.Global.Sites site = new DTO.Salida.Global.Sites();

            PaisesController cPaises = new PaisesController();
            OperadoresController cOperadores = new OperadoresController();
            MonedasController cMonedas = new MonedasController();
            EmplazamientosTiposController cEmplazamientosTipos = new EmplazamientosTiposController();
            EmplazamientosCategoriasSitiosController cEmplazamientosCategoriasSitios = new EmplazamientosCategoriasSitiosController();
            EmplazamientosTiposEdificiosController cEmplazamientosTiposEdificios = new EmplazamientosTiposEdificiosController();
            EmplazamientosTamanosController cEmplazamientosTamanos = new EmplazamientosTamanosController();
            EmplazamientosTiposEstructurasController cEmplazamientosTiposEstructuras = new EmplazamientosTiposEstructurasController();
            EstadosGlobalesController cEstadosGlobales = new EstadosGlobalesController();
            EmplazamientosAtributosController cEmplazamientosAtributos = new EmplazamientosAtributosController();

            #region GET PROPERTIES



            site.sName = emplazamiento.NombreSitio;
            site.sCode = emplazamiento.Codigo;

            if (emplazamiento.EstadoGlobalID != null)
            {
                EstadosGlobales estadoGlobal = cEstadosGlobales.GetItem(Convert.ToInt64(emplazamiento.EstadoGlobalID));
                if (estadoGlobal != null)
                {
                    site.sGlobalState = estadoGlobal.EstadoGlobal;
                }
            }

            Operadores operador = cOperadores.GetItem(emplazamiento.OperadorID);
            site.sCustomer = operador.Operador;

            if (emplazamiento.Longitud < float.MaxValue && emplazamiento.Longitud > float.MinValue)
            {
                site.dLongitude = Convert.ToSingle(emplazamiento.Longitud);
            }

            if (emplazamiento.Latitud < float.MaxValue && emplazamiento.Latitud > float.MinValue)
            {
                site.dLatitude = Convert.ToSingle(emplazamiento.Latitud);
            }

            if (!reducido)
            {

                if (emplazamiento.MunicipioID.HasValue)
                {

                    Paises paises = new PaisesController().GetItem(emplazamiento.PaisID);
                    Regiones regiones = new RegionesController().GetItem(paises.RegionID);
                    Municipios municipio = new MunicipiosController().GetItem(emplazamiento.MunicipioID.Value);
                    Provincias provincias = new ProvinciasController().GetItem(municipio.ProvinciaID);
                    RegionesPaises regionesPaises = new RegionesPaisesController().GetItem(provincias.RegionPaisID);

                    site.sRegion = regiones.Region;
                    site.sMunicipality = municipio.Municipio;
                    site.sProvince = provincias.Provincia;
                    site.sCountryRegion = regionesPaises.RegionPais;
                }
                else
                {
                    site.sRegion = emplazamiento.Region;
                    site.sMunicipality = emplazamiento.Municipio;
                    site.sProvince = emplazamiento.Provincia;
                    site.sCountryRegion = emplazamiento.RegionPais;
                }

                site.sPostalCode = emplazamiento.CodigoPostal;
                site.sAddress = emplazamiento.Direccion;


                Paises pais = cPaises.GetItem(emplazamiento.PaisID);
                if (pais != null)
                {
                    site.sCountry = pais.Pais;
                }

                if (emplazamiento.MonedaID != null)
                {
                    Monedas moneda = cMonedas.GetItem(Convert.ToInt64(emplazamiento.MonedaID));
                    if (moneda != null)
                    {
                        site.sCurrency = moneda.Simbolo;
                    }
                }

                if (emplazamiento.EmplazamientoTipoID != null)
                {
                    EmplazamientosTipos emplazamientosTipos = cEmplazamientosTipos.GetItem(Convert.ToInt64(emplazamiento.EmplazamientoTipoID));
                    if (emplazamientosTipos != null)
                    {
                        site.sSitesType = emplazamientosTipos.Tipo;
                    }
                }

                if (emplazamiento.CategoriaEmplazamientoID != null)
                {
                    EmplazamientosCategoriasSitios emplazamientosCategorias = cEmplazamientosCategoriasSitios.GetItem(Convert.ToInt64(emplazamiento.CategoriaEmplazamientoID));
                    if (emplazamientosCategorias != null)
                    {
                        site.sCategory = emplazamientosCategorias.CategoriaSitio;
                    }
                }

                if (emplazamiento.TipoEdificacionID != null)
                {
                    EmplazamientosTiposEdificios emplazamientosTiposEdificios = cEmplazamientosTiposEdificios.GetItem(Convert.ToInt64(emplazamiento.TipoEdificacionID));
                    if (emplazamientosTiposEdificios != null)
                    {
                        site.sStructureType = emplazamientosTiposEdificios.TipoEdificio;
                    }
                }

                if (emplazamiento.EmplazamientoTamanoID != null)
                {
                    EmplazamientosTamanos emplazamientosTamanos = cEmplazamientosTamanos.GetItem(Convert.ToInt64(emplazamiento.EmplazamientoTamanoID));
                    if (emplazamientosTamanos != null)
                    {
                        site.sSize = emplazamientosTamanos.Tamano;
                    }
                }

                if (emplazamiento.EmplazamientoTipoEstructuraID != null)
                {
                    EmplazamientosTiposEstructuras emplazamientosTiposEstructuras = cEmplazamientosTiposEstructuras.GetItem(Convert.ToInt64(emplazamiento.EmplazamientoTipoEstructuraID));
                    if (emplazamientosTiposEstructuras != null)
                    {
                        site.sBuildingType = emplazamientosTiposEstructuras.TipoEstructura;
                    }
                }

                site.ActivationDateDate = emplazamiento.FechaActivacion;
                site.DeactivationDateDate = emplazamiento.FechaDesactivacion;
                site.sNeighborhood = emplazamiento.Barrio;

                site.Attributes = new List<SiteAttributes>();

                #region Atributos
                foreach (EmplazamientosAtributos atributo in cEmplazamientosAtributos.GetAtributosEmplazamientos(emplazamiento.EmplazamientoID))
                {
                    EmplazamientosAtributosCategoriasController cEmplazamientosAtributosCategorias = new EmplazamientosAtributosCategoriasController();

                    Vw_EmplazamientosAtributos vw_empAtt = cEmplazamientosAtributos.GetItemVista(atributo.EmplazamientoAtributoID);
                    EmplazamientosAtributosCategorias categoria = cEmplazamientosAtributosCategorias.GetItem(vw_empAtt.EmplazamientoAtributoCategoriaID);

                    bool returnAttribute = true;
                    if (!string.IsNullOrEmpty(sCategoryAttribute) && !(sCategoryAttribute == categoria.Nombre))
                    {
                        returnAttribute = false;
                    }

                    if (returnAttribute)
                    {
                        TiposDatosController cTiposDatos = new TiposDatosController();
                        TiposDatos tipoDato = cTiposDatos.GetItem(vw_empAtt.TipoDatoID);

                        string value = vw_empAtt.Valor;
                        if (tipoDato != null && tipoDato.Codigo == Comun.TIPODATO_CODIGO_FECHA)
                        {
                            try
                            {                                           //4/19/2021 12:00:00 AM
                                DateTime dt = DateTime.ParseExact(value, "M/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                value = dt.ToString(Comun.FORMATO_FECHA);

                                //value = dt.ToString("MM/dd/yyyy");
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        SiteAttributes siteAttribute = new SiteAttributes()
                        {
                            sAttributeCode = vw_empAtt.CodigoAtributo,
                            sAttributeName = vw_empAtt.NombreAtributo,
                            sAttributeValue = value,
                            sCategoryAttribute = categoria.Nombre,
                            sSiteCode = emplazamiento.Codigo
                        };

                        site.Attributes.Add(siteAttribute);
                    }
                }
                #endregion
            }
            #endregion

            return site;
        }
        #endregion

        #region ConvertToSite Only CodeAndName
        private DTO.Salida.Global.Sites ConvertToSiteCodeName(Emplazamientos emplazamiento)
        {
            DTO.Salida.Global.Sites site = new DTO.Salida.Global.Sites();

            site.sName = emplazamiento.NombreSitio;
            site.sCode = emplazamiento.Codigo;

            return site;
        }
        #endregion

        #endregion

    }
}