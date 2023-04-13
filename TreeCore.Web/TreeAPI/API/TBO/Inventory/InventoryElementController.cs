using CapaNegocio;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Script.Serialization;
using TreeAPI.API.TBO.Interfaces;
using TreeAPI.DTO.Interfaces;
using TreeAPI.DTO.Salida.Inventory;
using TreeCore.Clases;
using TreeCore.Data;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq;
using Ext.Net;



namespace TreeAPI.Controllers
{
    [RoutePrefix("api/InventoryElement")]
    public class InventoryElementController : ApiBaseController/*, ICollective*/
    {
        #region CONSTANTES
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string NAME_CLASS = "InventoryElementController";
        #endregion

        #region COMUN INTERFACE

        //#region _CI_Create
        ///// <summary>
        ///// Create or update Inventory Elements
        ///// </summary>
        ///// <param name="Element">Data of InventarioElemento</param>
        ///// <param name="sUser">User email</param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("")]
        ////[ActionName("_CI_Create")]
        //public TBOResponse _CI_Create(DTO.Entrada.Inventory.InventoryElements Element, string sUser, bool bUpdate)
        //{
        //    // Local variables
        //    TBOResponse response = null;
        //    DTO.Entrada.Inventory.InventoryElements nuevo = null;
        //    InventarioCategorias categoria = null;
        //    InventarioElementos elementoInventario = new InventarioElementos();
        //    InventarioElementos elementoInventarioCat;
        //    List<Object> listaAtributos = new List<object>();
        //    InventarioElementosController cInventarioElementos;
        //    EmplazamientosController cEmplazamientos;
        //    InventarioCategoriasController cCategorias = new InventarioCategoriasController();
        //    EntidadesController cEntidades = new EntidadesController();
        //    InventarioAtributosController inventarioAtributos = new InventarioAtributosController();
        //    InventarioElementosAtributosEstadosController cInventarioElementosAtributosEstados = new InventarioElementosAtributosEstadosController();
        //    bool exitoCamposVacios = true;
        //    bool exitoLongitudCampos = true;
        //    bool exitoCamposRelacionados = true;
        //    bool fechasValidas = true;
        //    string sMissing = "";
        //    string cambosExcedidos = "";
        //    string fechasNoValidas = "";

        //    // Controllers
        //    InventarioElementosController cInventario = null;
        //    long? emplazamientoID = null;

        //    #region MONITORING_INICIAL
        //    MonitoringWSRegistrosController cMonitoring = new MonitoringWSRegistrosController();
        //    string sTipoProyecto = Comun.MODULOINVENTARIO;
        //    string sServicio = Comun.INTEGRACION_SERVICIO_API;
        //    string sMetodo = Comun.TBO.Metodo.TBO_METODO_INVENTORY_ELEMENTS_CI_CREATE;
        //    string sComentarios = "Invocacion del método _CI_Create del TBO de Inventory Elements";

        //    bool bPropio = true;
        //    string UsuarioEmail = (Element != null) ? sUser : string.Empty;
        //    string sParametroEntrada = GetInputParameter(sMetodo, UsuarioEmail, Element, "", "", "", "", "");
        //    string sParametroSalida = "";
        //    long? monitoringAlquilerID = null;
        //    long? monitoringEmplazamientoID = null;
        //    long? monitoringUsuarioID = null;
        //    long? monitoringClienteID = null;
        //    #endregion


        //    try
        //    {
        //        nuevo = Element;

        //        if (nuevo != null)
        //        {
        //            cInventario = new InventarioElementosController();
        //            cInventarioElementos = new InventarioElementosController();
        //            cEmplazamientos = new EmplazamientosController();

        //            #region COMPROBAR CAMPOS VACIOS
        //            if (string.IsNullOrEmpty(sUser))
        //            {
        //                exitoCamposVacios = false;
        //                sMissing = sMissing + " - " + nameof(sUser) + " ";
        //            }

        //            if (string.IsNullOrEmpty(nuevo.sName))
        //            {
        //                exitoCamposVacios = false;
        //                sMissing = sMissing + " - " + nameof(nuevo.sName) + " ";
        //            }

        //            if (string.IsNullOrEmpty(nuevo.sCategory))
        //            {
        //                exitoCamposVacios = false;
        //                sMissing = sMissing + " - " + nameof(nuevo.sCategory) + " ";
        //            }

        //            if (string.IsNullOrEmpty(nuevo.sElementCode))
        //            {
        //                exitoCamposVacios = false;
        //                sMissing = sMissing + " - " + nameof(nuevo.sElementCode) + " ";
        //            }

        //            if (string.IsNullOrEmpty(nuevo.sSiteCode))
        //            {
        //                exitoCamposVacios = false;
        //                sMissing = sMissing + " - " + nameof(nuevo.sSiteCode) + " ";
        //            }
        //            if (string.IsNullOrEmpty(nuevo.sCustomer))
        //            {
        //                exitoCamposVacios = false;
        //                sMissing = sMissing + " - " + nameof(nuevo.sCustomer) + " ";
        //            }
        //            if (string.IsNullOrEmpty(nuevo.sStatus))
        //            {
        //                exitoCamposVacios = false;
        //                sMissing = sMissing + " - " + nameof(nuevo.sStatus) + " ";
        //            }

        //            if (nuevo.bTemplate && string.IsNullOrEmpty(nuevo.sTemplate))
        //            {
        //                exitoCamposVacios = false;
        //                sMissing = sMissing + " - " + nameof(nuevo.sTemplate) + " ";
        //            }

        //            if (nuevo.Attributes != null)
        //            {
        //                if (nuevo.Attributes != null)
        //                {
        //                    foreach (var a in nuevo.Attributes)
        //                    {
        //                        if (string.IsNullOrEmpty(a.sAttributeName))
        //                        {
        //                            exitoCamposVacios = false;
        //                            sMissing = sMissing + " - " + nameof(a.sAttributeName) + " ";
        //                        }
        //                        if (string.IsNullOrEmpty(a.sAttributeValue))
        //                        {
        //                            exitoCamposVacios = false;
        //                            sMissing = sMissing + " - " + nameof(a.sAttributeValue) + " ";
        //                        }
        //                    }
        //                }
        //            }

        //            #endregion

        //            if (exitoCamposVacios)
        //            {



        //                if (fechasValidas)
        //                {
        //                    #region COMPROBAR LONGITUD CAMPOS
        //                    if (sUser != null && sUser.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.emailUsuario)
        //                    {
        //                        exitoLongitudCampos = false;
        //                        cambosExcedidos += " - " + nameof(sUser) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.emailUsuario + ") ";
        //                    }
        //                    if (nuevo.sName != null && nuevo.sName.Length > Comun.TBO.LENGTH_CAMPOS.INVENTORY.nombreInventario)
        //                    {
        //                        exitoLongitudCampos = false;
        //                        cambosExcedidos += " - " + nameof(nuevo.sName) + "(" + Comun.TBO.LENGTH_CAMPOS.INVENTORY.nombreInventario + ") ";
        //                    }
        //                    if (nuevo.sElementCode != null && nuevo.sElementCode.Length > Comun.TBO.LENGTH_CAMPOS.INVENTORY.numeroInventario)
        //                    {
        //                        exitoLongitudCampos = false;
        //                        cambosExcedidos += " - " + nameof(nuevo.sElementCode) + "(" + Comun.TBO.LENGTH_CAMPOS.INVENTORY.numeroInventario + ") ";
        //                    }
        //                    if (nuevo.sSiteCode != null && nuevo.sSiteCode.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.codigoEmplazamiento)
        //                    {
        //                        exitoLongitudCampos = false;
        //                        cambosExcedidos += " - " + nameof(nuevo.sSiteCode) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.codigoEmplazamiento + ") ";
        //                    }
        //                    if (nuevo.sCategory != null && nuevo.sCategory.Length > Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.mantenimientoIncidenciasTipo)
        //                    {
        //                        exitoLongitudCampos = false;
        //                        cambosExcedidos += " - " + nameof(nuevo.sCategory) + "(" + Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.mantenimientoIncidenciasTipo + ") ";
        //                    }


        //                    #endregion

        //                    if (exitoLongitudCampos)
        //                    {
        //                        this.SetUsuario(sUser);

        //                        if (user != null)
        //                        {
        //                            categoria = cCategorias.GetInventarioCategoriaIDByNombre(nuevo.sCategory, user.ClienteID.Value);

        //                            if (categoria != null)
        //                            {

        //                                if (nuevo.Attributes != null)
        //                                {
        //                                    List<Object> ListaAtributos = new List<object>();
        //                                    if (nuevo.Attributes != null)
        //                                    {
        //                                        foreach (var a in nuevo.Attributes)
        //                                        {
        //                                            long resultado = inventarioAtributos.GetAtributoByNombreCategoriaAPI(a.sAttributeName, categoria.InventarioCategoriaID);
        //                                            if (!(resultado > 0))
        //                                            {
        //                                                exitoCamposRelacionados = false;
        //                                            }
        //                                            else
        //                                            {
        //                                                a.sAttributeCode = resultado.ToString();
        //                                                listaAtributos.Add(a); //Si el parametro pertenece a la categoría 
        //                                            }
        //                                        }
        //                                    }
        //                                }

        //                                #region MONITORING
        //                                monitoringUsuarioID = (user == null) ? null : (long?)user.UsuarioID;
        //                                monitoringAlquilerID = null;
        //                                monitoringEmplazamientoID = emplazamientoID;
        //                                #endregion

        //                                if (exitoCamposRelacionados)
        //                                {
        //                                    if (exitoCamposVacios)
        //                                    {
        //                                        Operadores operador = cEntidades.GetActivoOperador(Element.sCustomer, user.ClienteID.Value);
        //                                        if (operador != null)
        //                                        {

        //                                            emplazamientoID = cEmplazamientos.GetEmplazamientoIDByCodigo(nuevo.sSiteCode, user.ClienteID.Value);

        //                                            if (emplazamientoID > 0)
        //                                            {
        //                                                if (nuevo.bTemplate)
        //                                                {
        //                                                    elementoInventarioCat = cInventario.GetPlantillaCategoria(categoria.InventarioCategoriaID, nuevo.sTemplate);
        //                                                    if (elementoInventarioCat != null)
        //                                                    {
        //                                                        long PlantillaID = elementoInventario.InventarioElementoID;

        //                                                        long estatus = cInventarioElementosAtributosEstados.GetEstadoIDByNombre(user.ClienteID.Value, nuevo.sStatus);
        //                                                        if (estatus != null && estatus != 0)
        //                                                        {
        //                                                            bool existeInventario = cInventario.ComprobarDuplicadoCodigo(nuevo.sElementCode, user.ClienteID.Value); // Comprueba código duplicado
        //                                                            if (!existeInventario) //No existe el inventario
        //                                                            {
        //                                                                if (!bUpdate)
        //                                                                {


        //                                                                    //Validación que todos los campos tengan datos
        //                                                                    #region Validacion de campos
        //                                                                    if (nuevo.sName == null)
        //                                                                        nuevo.sName = "";
        //                                                                    if (PlantillaID == null || PlantillaID == 0)
        //                                                                    {
        //                                                                        PlantillaID = 0;
        //                                                                        nuevo.bTemplate = false;
        //                                                                    }
        //                                                                    else
        //                                                                    {
        //                                                                        nuevo.bTemplate = true;
        //                                                                    }
        //                                                                    if (estatus == null)
        //                                                                        estatus = 0;

        //                                                                    #region INVENTARIO PADRE
        //                                                                    long? elementoPadreID = null;
        //                                                                    if (nuevo.ParentElement != null && !string.IsNullOrEmpty(nuevo.ParentElement.sNumberInventory))
        //                                                                    {
        //                                                                        InventarioElementos intentarioPadre = cInventario.GetByNumberAndClienteID(nuevo.ParentElement.sNumberInventory, user.ClienteID.Value);
        //                                                                        if (intentarioPadre != null)
        //                                                                        {
        //                                                                            if (cCategorias.EsCategoriaPadrePermitida(categoria.InventarioCategoriaID, intentarioPadre.InventarioCategoriaID, (long)emplazamientoID))
        //                                                                            {
        //                                                                                elementoPadreID = intentarioPadre.InventarioElementoID;
        //                                                                            }
        //                                                                            else
        //                                                                            {
        //                                                                                response = new TBOResponse
        //                                                                                {
        //                                                                                    Result = false,
        //                                                                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_TEMPLATE_NOT_ALLOW_PARENT_CATEGORY_CODE,
        //                                                                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_TEMPLATE_NOT_ALLOW_PARENT_CATEGORY_DESCRIPTION
        //                                                                                };
        //                                                                            }
        //                                                                        }
        //                                                                    }

        //                                                                    #endregion
        //                                                                    if (emplazamientoID == null)
        //                                                                    {
        //                                                                        emplazamientoID = 0;
        //                                                                    }

        //                                                                    #endregion

        //                                                                    if (response == null)
        //                                                                    {
        //                                                                        ResponseCreateController responseCreate = cInventario.CreateInventary(bUpdate, user, user.ClienteID.Value, nuevo.sElementCode, nuevo.sName, operador, estatus, categoria, listaAtributos, nuevo.bTemplate, elementoPadreID,
        //                                                                                                                                                     Convert.ToInt64(emplazamientoID), PlantillaID);
        //                                                                        if (responseCreate.Data != null)
        //                                                                        {
        //                                                                            elementoInventario = (InventarioElementos)responseCreate.Data;

        //                                                                            elementoInventario = cInventario.GetItem(elementoInventario.InventarioElementoID);
        //                                                                            response = new TBOResponse
        //                                                                            {
        //                                                                                Result = true,
        //                                                                                Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
        //                                                                                Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
        //                                                                                Data = ConvertToInventoryElementCat(elementoInventario, true, categoria.InventarioCategoria)
        //                                                                            };

        //                                                                        }
        //                                                                        else
        //                                                                        {
        //                                                                            response = new TBOResponse
        //                                                                            {
        //                                                                                Result = false,
        //                                                                                Code = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_CODE,
        //                                                                                Description = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_DESCRIPTION
        //                                                                            };
        //                                                                        }
        //                                                                    }
        //                                                                }
        //                                                                else
        //                                                                {
        //                                                                    response = new TBOResponse
        //                                                                    {
        //                                                                        Result = false,
        //                                                                        Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_CODE_NOT_FOUND_CODE,
        //                                                                        Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_CODE_NOT_FOUND_DESCRIPTION
        //                                                                    };
        //                                                                }
        //                                                            }
        //                                                            else //Existe el inventario
        //                                                            {

        //                                                                if (bUpdate) //Modificación
        //                                                                {

        //                                                                    elementoInventario = cInventarioElementos.getElementoByNombreNumero(nuevo.sName, nuevo.sElementCode);
        //                                                                    if (categoria.InventarioCategoriaID == elementoInventario.InventarioCategoriaID) //Si tiene la misma categoria puede avanzar
        //                                                                    {
        //                                                                        //Validación que todos los campos tengan datos
        //                                                                        #region Validacion de campos
        //                                                                        if (nuevo.sName == null)
        //                                                                            nuevo.sName = "";
        //                                                                        if (PlantillaID == null || PlantillaID == 0)
        //                                                                        {
        //                                                                            PlantillaID = 0;
        //                                                                            nuevo.bTemplate = false;
        //                                                                        }
        //                                                                        else
        //                                                                        {
        //                                                                            nuevo.bTemplate = true;
        //                                                                        }
        //                                                                        if (estatus == null)
        //                                                                            estatus = 0;

        //                                                                        if (nuevo.ParentElement == null)
        //                                                                        {
        //                                                                            nuevo.ParentElement = new DTO.Entrada.Inventory.InventoryElementParent();
        //                                                                            nuevo.ParentElement.sNumberInventory = "0";
        //                                                                        }
        //                                                                        else
        //                                                                        {
        //                                                                            InventarioElementos intentarioPadre = cInventario.GetByNumberAndClienteID(nuevo.ParentElement.sNumberInventory, user.ClienteID.Value);
        //                                                                            if (intentarioPadre != null)
        //                                                                            {
        //                                                                                if (!cCategorias.EsCategoriaPadrePermitida(categoria.InventarioCategoriaID, intentarioPadre.InventarioCategoriaID, (long)emplazamientoID))
        //                                                                                {
        //                                                                                    response = new TBOResponse
        //                                                                                    {
        //                                                                                        Result = false,
        //                                                                                        Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_TEMPLATE_NOT_ALLOW_PARENT_CATEGORY_CODE,
        //                                                                                        Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_TEMPLATE_NOT_ALLOW_PARENT_CATEGORY_DESCRIPTION
        //                                                                                    };
        //                                                                                }
        //                                                                            }
        //                                                                        }
        //                                                                        if (emplazamientoID == null)
        //                                                                            emplazamientoID = 0;

        //                                                                        #endregion

        //                                                                        if (response == null)
        //                                                                        {
        //                                                                            ResponseCreateController responseCreate = cInventario.CreateInventary(bUpdate, user, user.ClienteID.Value, nuevo.sElementCode, nuevo.sName, operador, estatus, categoria, listaAtributos, nuevo.bTemplate, Convert.ToInt32(nuevo.ParentElement.sNumberInventory),
        //                                                                                                                                                         Convert.ToInt64(emplazamientoID), PlantillaID);
        //                                                                            if (responseCreate.Data != null)
        //                                                                            {
        //                                                                                elementoInventario = (InventarioElementos)responseCreate.Data;

        //                                                                                elementoInventario = cInventario.GetItem(elementoInventario.InventarioElementoID);
        //                                                                                response = new TBOResponse
        //                                                                                {
        //                                                                                    Result = true,
        //                                                                                    Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
        //                                                                                    Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
        //                                                                                    Data = ConvertToInventoryElementCat(elementoInventario, true, categoria.InventarioCategoria)
        //                                                                                };

        //                                                                            }
        //                                                                            else
        //                                                                            {
        //                                                                                response = new TBOResponse
        //                                                                                {
        //                                                                                    Result = false,
        //                                                                                    Code = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_CODE,
        //                                                                                    Description = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_DESCRIPTION
        //                                                                                };
        //                                                                            }
        //                                                                        }
        //                                                                    }
        //                                                                    else
        //                                                                    {
        //                                                                        response = new TBOResponse
        //                                                                        {
        //                                                                            Result = false,
        //                                                                            Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_NOT_FOUND_CODE,
        //                                                                            Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_NOT_FOUND_DESCRIPTION
        //                                                                        };
        //                                                                    }
        //                                                                }
        //                                                                else
        //                                                                {
        //                                                                    response = new TBOResponse
        //                                                                    {
        //                                                                        Result = false,
        //                                                                        Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_ELEMENT_CODE_DUPLICATE,
        //                                                                        Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_ELEMENT_DUPLICATE_DESCRIPTION
        //                                                                    };
        //                                                                }
        //                                                            }



        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            response = new TBOResponse
        //                                                            {
        //                                                                Result = false,
        //                                                                Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_STATUS_NOT_FOUND_CODE,
        //                                                                Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_STATUS_NOT_FOUND_DESCRIPTION
        //                                                            };
        //                                                        }
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        response = new TBOResponse
        //                                                        {
        //                                                            Result = false,
        //                                                            Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_TEMPLATE_NOT_FOUND_CODE,
        //                                                            Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_TEMPLATE_NOT_FOUND_DESCRIPTION
        //                                                        };
        //                                                    }
        //                                                }
        //                                                else
        //                                                {
        //                                                    long estatus = cInventarioElementosAtributosEstados.GetEstadoIDByNombre(user.ClienteID.Value, nuevo.sStatus);
        //                                                    if (estatus != null && estatus != 0)
        //                                                    {
        //                                                        bool existeInventario = cInventario.ComprobarDuplicadoCodigo(nuevo.sElementCode, user.ClienteID.Value); // Comprueba código duplicado
        //                                                        if (!existeInventario) //No existe el inventario
        //                                                        {
        //                                                            if (!bUpdate)
        //                                                            {

        //                                                                long PlantillaID = 0;
        //                                                                //Validación que todos los campos tengan datos
        //                                                                #region Validacion de campos
        //                                                                if (nuevo.sName == null)
        //                                                                    nuevo.sName = "";
        //                                                                if (PlantillaID == null || PlantillaID == 0)
        //                                                                {
        //                                                                    PlantillaID = 0;
        //                                                                    nuevo.bTemplate = false;
        //                                                                }
        //                                                                else
        //                                                                {
        //                                                                    nuevo.bTemplate = true;
        //                                                                }
        //                                                                if (estatus == null)
        //                                                                    estatus = 0;

        //                                                                #region INVENTARIO PADRE
        //                                                                long? elementoPadreID = null;
        //                                                                if (nuevo.ParentElement != null && !string.IsNullOrEmpty(nuevo.ParentElement.sNumberInventory))
        //                                                                {
        //                                                                    InventarioElementos intentarioPadre = cInventario.GetByNumberAndClienteID(nuevo.ParentElement.sNumberInventory, user.ClienteID.Value);
        //                                                                    if (intentarioPadre != null)
        //                                                                    {
        //                                                                        if (cCategorias.EsCategoriaPadrePermitida(categoria.InventarioCategoriaID, intentarioPadre.InventarioCategoriaID, (long)emplazamientoID))
        //                                                                        {
        //                                                                            elementoPadreID = intentarioPadre.InventarioElementoID;
        //                                                                        }
        //                                                                        else
        //                                                                        {
        //                                                                            response = new TBOResponse
        //                                                                            {
        //                                                                                Result = false,
        //                                                                                Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_TEMPLATE_NOT_ALLOW_PARENT_CATEGORY_CODE,
        //                                                                                Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_TEMPLATE_NOT_ALLOW_PARENT_CATEGORY_DESCRIPTION
        //                                                                            };
        //                                                                        }
        //                                                                    }
        //                                                                }

        //                                                                #endregion
        //                                                                if (emplazamientoID == null)
        //                                                                {
        //                                                                    emplazamientoID = 0;
        //                                                                }
        //                                                                #endregion

        //                                                                if (response == null)
        //                                                                {
        //                                                                    ResponseCreateController responseCreate = cInventario.CreateInventary(bUpdate, user, user.ClienteID.Value, nuevo.sElementCode, nuevo.sName, operador, estatus, categoria, listaAtributos, nuevo.bTemplate, elementoPadreID,
        //                                                                                                                                                     Convert.ToInt64(emplazamientoID), PlantillaID);
        //                                                                    if (responseCreate.Data != null)
        //                                                                    {
        //                                                                        elementoInventario = (InventarioElementos)responseCreate.Data;

        //                                                                        elementoInventario = cInventario.GetItem(elementoInventario.InventarioElementoID);
        //                                                                        response = new TBOResponse
        //                                                                        {
        //                                                                            Result = true,
        //                                                                            Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
        //                                                                            Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
        //                                                                            Data = ConvertToInventoryElementCat(elementoInventario, true, categoria.InventarioCategoria)
        //                                                                        };

        //                                                                    }
        //                                                                    else
        //                                                                    {
        //                                                                        response = new TBOResponse
        //                                                                        {
        //                                                                            Result = false,
        //                                                                            Code = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_CODE,
        //                                                                            Description = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_DESCRIPTION
        //                                                                        };
        //                                                                    }
        //                                                                }
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                response = new TBOResponse
        //                                                                {
        //                                                                    Result = false,
        //                                                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_CODE_NOT_FOUND_CODE,
        //                                                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_CODE_NOT_FOUND_DESCRIPTION
        //                                                                };
        //                                                            }
        //                                                        }
        //                                                        else //Existe el inventario
        //                                                        {

        //                                                            if (bUpdate) //Modificación
        //                                                            {

        //                                                                elementoInventario = cInventarioElementos.getElementoByNombreNumero(nuevo.sName, nuevo.sElementCode);
        //                                                                if (categoria.InventarioCategoriaID == elementoInventario.InventarioCategoriaID) //Si tiene la misma categoria puede avanzar
        //                                                                {
        //                                                                    long PlantillaID = 0;
        //                                                                    //Validación que todos los campos tengan datos
        //                                                                    #region Validacion de campos
        //                                                                    if (nuevo.sName == null)
        //                                                                        nuevo.sName = "";
        //                                                                    if (PlantillaID == null || PlantillaID == 0)
        //                                                                    {
        //                                                                        PlantillaID = 0;
        //                                                                        nuevo.bTemplate = false;
        //                                                                    }
        //                                                                    else
        //                                                                    {
        //                                                                        nuevo.bTemplate = true;
        //                                                                    }
        //                                                                    if (estatus == null)
        //                                                                        estatus = 0;

        //                                                                    if (nuevo.ParentElement == null)
        //                                                                    {
        //                                                                        nuevo.ParentElement = new DTO.Entrada.Inventory.InventoryElementParent();
        //                                                                        nuevo.ParentElement.sNumberInventory = "0";
        //                                                                    }
        //                                                                    else
        //                                                                    {
        //                                                                        InventarioElementos intentarioPadre = cInventario.GetByNumberAndClienteID(nuevo.ParentElement.sNumberInventory, user.ClienteID.Value);
        //                                                                        if (intentarioPadre != null)
        //                                                                        {
        //                                                                            if (!cCategorias.EsCategoriaPadrePermitida(categoria.InventarioCategoriaID, intentarioPadre.InventarioCategoriaID, (long)emplazamientoID))
        //                                                                            {
        //                                                                                response = new TBOResponse
        //                                                                                {
        //                                                                                    Result = false,
        //                                                                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_TEMPLATE_NOT_ALLOW_PARENT_CATEGORY_CODE,
        //                                                                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_TEMPLATE_NOT_ALLOW_PARENT_CATEGORY_DESCRIPTION
        //                                                                                };
        //                                                                            }
        //                                                                        }
        //                                                                    }
        //                                                                    if (emplazamientoID == null)
        //                                                                        emplazamientoID = 0;

        //                                                                    #endregion

        //                                                                    if (response == null)
        //                                                                    {
        //                                                                        ResponseCreateController responseCreate = cInventario.CreateInventary(bUpdate, user, user.ClienteID.Value, nuevo.sElementCode, nuevo.sName, operador, estatus, categoria, listaAtributos, nuevo.bTemplate, Convert.ToInt32(nuevo.ParentElement.sNumberInventory),
        //                                                                                                                                                     Convert.ToInt64(emplazamientoID), PlantillaID);
        //                                                                        if (responseCreate.Data != null)
        //                                                                        {
        //                                                                            elementoInventario = (InventarioElementos)responseCreate.Data;

        //                                                                            elementoInventario = cInventario.GetItem(elementoInventario.InventarioElementoID);
        //                                                                            response = new TBOResponse
        //                                                                            {
        //                                                                                Result = true,
        //                                                                                Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
        //                                                                                Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
        //                                                                                Data = ConvertToInventoryElementCat(elementoInventario, true, categoria.InventarioCategoria)
        //                                                                            };

        //                                                                        }
        //                                                                        else
        //                                                                        {
        //                                                                            response = new TBOResponse
        //                                                                            {
        //                                                                                Result = false,
        //                                                                                Code = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_CODE,
        //                                                                                Description = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_DESCRIPTION
        //                                                                            };
        //                                                                        }
        //                                                                    }
        //                                                                }
        //                                                                else
        //                                                                {
        //                                                                    response = new TBOResponse
        //                                                                    {
        //                                                                        Result = false,
        //                                                                        Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_NOT_FOUND_CODE,
        //                                                                        Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_NOT_FOUND_DESCRIPTION
        //                                                                    };
        //                                                                }
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                response = new TBOResponse
        //                                                                {
        //                                                                    Result = false,
        //                                                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_ELEMENT_CODE_DUPLICATE,
        //                                                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_ELEMENT_DUPLICATE_DESCRIPTION
        //                                                                };
        //                                                            }
        //                                                        }



        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        response = new TBOResponse
        //                                                        {
        //                                                            Result = false,
        //                                                            Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_STATUS_NOT_FOUND_CODE,
        //                                                            Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_STATUS_NOT_FOUND_DESCRIPTION
        //                                                        };
        //                                                    }
        //                                                }

        //                                            }
        //                                            else
        //                                            {
        //                                                response = new TBOResponse
        //                                                {
        //                                                    Result = false,
        //                                                    Code = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_CODE,
        //                                                    Description = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_DESCRIPTION
        //                                                };
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            response = new TBOResponse
        //                                            {
        //                                                Result = false,
        //                                                Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_OPERATOR_NOT_FOUND_CODE,
        //                                                Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_OPERATOR_NOT_FOUND_DESCRIPTION
        //                                            };

        //                                        }
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    response = new TBOResponse
        //                                    {
        //                                        Result = false,
        //                                        Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ELEMENT_ATTRIBUTE_NOT_FOUND_CODE,
        //                                        Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ELEMENT_ATTRIBUTE__NOT_FOUND_DESCRIPTION
        //                                    };
        //                                }
        //                            }
        //                            else
        //                            {
        //                                response = new TBOResponse
        //                                {
        //                                    Result = false,
        //                                    Code = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_CATEGORY_NOT_FOUND_CODE,
        //                                    Description = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_CATEGORY_NOT_FOUND_DESCRIPTION
        //                                };
        //                            }
        //                        }
        //                        else
        //                        {
        //                            response = new TBOResponse
        //                            {
        //                                Result = false,
        //                                Code = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_CODE,
        //                                Description = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_DESCRIPTION
        //                            };
        //                        }
        //                    }
        //                    else
        //                    {

        //                        response = new TBOResponse
        //                        {
        //                            Result = false,
        //                            Code = ServicesCodes.GENERIC.COD_TBO_LENGTH_DATA_EXCEEDS_CODE,
        //                            Description = ServicesCodes.GENERIC.COD_TBO_LENGTH_DATA_EXCEEDS_DESCRIPTION + cambosExcedidos
        //                        };

        //                    }
        //                }
        //                else
        //                {
        //                    if (response == null)
        //                    {

        //                        response = new TBOResponse
        //                        {
        //                            Result = false,
        //                            Code = ServicesCodes.GENERIC.COD_TBO_INCORRECT_DATE_FORMAT_CODE,
        //                            Description = ServicesCodes.GENERIC.COD_TBO_INCORRECT_DATE_FORMAT_DESCRIPTION + fechasNoValidas
        //                        };
        //                    }


        //                }

        //            }
        //            else
        //            {

        //                response = new TBOResponse
        //                {
        //                    Result = false,
        //                    Code = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_CODE,
        //                    Description = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_DESCRIPTION + sMissing
        //                };

        //            }
        //        }
        //        else
        //        {
        //            response = new TBOResponse
        //            {
        //                Result = false,
        //                Code = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_CODE,
        //                Description = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_DESCRIPTION + " - " + nameof(Element)
        //            };
        //        }
        //    }



        //    catch (Exception ex)
        //    {
        //        Comun.cLog.EscribirLog(GetMessajeLog(NAME_CLASS, "Create()", ex.Message));
        //        log.Error(ex.Message);
        //        response = new TBOResponse
        //        {
        //            Result = false,
        //            Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
        //            Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION + " " + ex.Message
        //        };
        //    }

        //    #region MONITORING_FINAL
        //    sParametroSalida = GetOutputParameter(response);
        //    sComentarios = response.Description;
        //    cMonitoring.AgregarRegistro(sTipoProyecto, monitoringUsuarioID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, monitoringClienteID, response.Result, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);
        //    #endregion

        //    // Returns the result
        //    return response;
        //}
        //#endregion

        #region _CI_Create
        /// <summary>
        /// Create or update Inventory Elements
        /// </summary>
        /// <param name="Element">Data of InventarioElemento</param>
        /// <param name="sUser">User email</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        //[ActionName("_CI_Create")]
        public TBOResponse _CI_Create(DTO.Entrada.Inventory.InventoryElements Element, string sUser, bool bUpdate)
        {
            // Local variables
            TBOResponse response = null;
            DTO.Entrada.Inventory.InventoryElements nuevo = null;
            InventarioCategorias categoria = null;
            InventarioElementos elementoInventario = new InventarioElementos();
            InventarioElementos elementoInventarioCat;
            List<Object> listaAtributos = new List<object>();
            List<Object> listaPlantillas = new List<object>();
            InventarioElementosController cInventarioElementos;
            EmplazamientosController cEmplazamientos;
            InventarioCategoriasController cCategorias = new InventarioCategoriasController();
            InventarioAtributosCategoriasController cAtrCategorias = new InventarioAtributosCategoriasController();
            EntidadesController cEntidades = new EntidadesController();
            CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
            CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
            InventarioElementosAtributosEstadosController cInventarioElementosAtributosEstados = new InventarioElementosAtributosEstadosController();
            bool exitoCamposVacios = true;
            bool exitoLongitudCampos = true;
            bool exitoCamposRelacionados = true;
            bool fechasValidas = true;
            string sMissing = "";
            string cambosExcedidos = "";
            string fechasNoValidas = "";

            // Controllers
            InventarioElementosController cInventario = null;
            long? emplazamientoID = null;

            #region MONITORING_INICIAL
            MonitoringWSRegistrosController cMonitoring = new MonitoringWSRegistrosController();
            string sTipoProyecto = Comun.MODULOINVENTARIO;
            string sServicio = Comun.INTEGRACION_SERVICIO_API;
            string sMetodo = Comun.TBO.Metodo.TBO_METODO_INVENTORY_ELEMENTS_CI_CREATE;
            string sComentarios = "Invocacion del método _CI_Create del TBO de Inventory Elements";

            bool bPropio = true;
            string UsuarioEmail = (Element != null) ? sUser : string.Empty;
            string sParametroEntrada = GetInputParameter(sMetodo, UsuarioEmail, Element, "", "", "", "", "");
            string sParametroSalida = "";
            long? monitoringAlquilerID = null;
            long? monitoringEmplazamientoID = null;
            long? monitoringUsuarioID = null;
            long? monitoringClienteID = null;
            #endregion


            try
            {
                nuevo = Element;

                if (nuevo != null)
                {
                    cInventario = new InventarioElementosController();
                    cInventarioElementos = new InventarioElementosController();
                    cEmplazamientos = new EmplazamientosController();

                    #region COMPROBAR CAMPOS VACIOS
                    if (string.IsNullOrEmpty(sUser))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(sUser) + " ";
                    }

                    if (string.IsNullOrEmpty(nuevo.sName))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(nuevo.sName) + " ";
                    }

                    if (string.IsNullOrEmpty(nuevo.sCategory))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(nuevo.sCategory) + " ";
                    }

                    if (string.IsNullOrEmpty(nuevo.sElementCode))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(nuevo.sElementCode) + " ";
                    }

                    if (string.IsNullOrEmpty(nuevo.sSiteCode))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(nuevo.sSiteCode) + " ";
                    }
                    if (string.IsNullOrEmpty(nuevo.sCustomer))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(nuevo.sCustomer) + " ";
                    }
                    if (string.IsNullOrEmpty(nuevo.sStatus))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(nuevo.sStatus) + " ";
                    }

                    if (nuevo.Attributes != null)
                    {
                        foreach (var a in nuevo.Attributes)
                        {
                            if (string.IsNullOrEmpty(a.sAttributeName))
                            {
                                exitoCamposVacios = false;
                                sMissing = sMissing + " - " + nameof(a.sAttributeName) + " ";
                            }
                            if (string.IsNullOrEmpty(a.sAttributeValue))
                            {
                                exitoCamposVacios = false;
                                sMissing = sMissing + " - " + nameof(a.sAttributeValue) + " ";
                            }
                        }
                    }

                    #endregion

                    if (exitoCamposVacios)
                    {
                        #region COMPROBAR LONGITUD CAMPOS

                        if (sUser != null && sUser.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.emailUsuario)
                        {
                            exitoLongitudCampos = false;
                            cambosExcedidos += " - " + nameof(sUser) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.emailUsuario + ") ";
                        }
                        if (nuevo.sName != null && nuevo.sName.Length > Comun.TBO.LENGTH_CAMPOS.INVENTORY.nombreInventario)
                        {
                            exitoLongitudCampos = false;
                            cambosExcedidos += " - " + nameof(nuevo.sName) + "(" + Comun.TBO.LENGTH_CAMPOS.INVENTORY.nombreInventario + ") ";
                        }
                        if (nuevo.sElementCode != null && nuevo.sElementCode.Length > Comun.TBO.LENGTH_CAMPOS.INVENTORY.numeroInventario)
                        {
                            exitoLongitudCampos = false;
                            cambosExcedidos += " - " + nameof(nuevo.sElementCode) + "(" + Comun.TBO.LENGTH_CAMPOS.INVENTORY.numeroInventario + ") ";
                        }
                        if (nuevo.sSiteCode != null && nuevo.sSiteCode.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.codigoEmplazamiento)
                        {
                            exitoLongitudCampos = false;
                            cambosExcedidos += " - " + nameof(nuevo.sSiteCode) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.codigoEmplazamiento + ") ";
                        }
                        if (nuevo.sCategory != null && nuevo.sCategory.Length > Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.mantenimientoIncidenciasTipo)
                        {
                            exitoLongitudCampos = false;
                            cambosExcedidos += " - " + nameof(nuevo.sCategory) + "(" + Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.mantenimientoIncidenciasTipo + ") ";
                        }

                        #endregion

                        if (exitoLongitudCampos)
                        {
                            this.SetUsuario(sUser);

                            if (user != null)
                            {
                                categoria = cCategorias.GetInventarioCategoriaIDByNombre(nuevo.sCategory, user.ClienteID.Value);
                                if (categoria == null)
                                {
                                    response = new TBOResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_CATEGORY_NOT_FOUND_CODE,
                                        Description = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_CATEGORY_NOT_FOUND_DESCRIPTION
                                    };
                                    return response;
                                }

                                Emplazamientos oEmplazamiento = cEmplazamientos.GetEmplazamientoByCodigoyCliente(nuevo.sSiteCode, user.ClienteID.Value);
                                if (oEmplazamiento == null)
                                {
                                    response = new TBOResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_CODE,
                                        Description = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_DESCRIPTION
                                    };
                                    return response;
                                }

                                Operadores oOperador = cEntidades.GetActivoOperador(Element.sCustomer, user.ClienteID.Value);
                                if (oOperador == null)
                                {
                                    response = new TBOResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_OPERATOR_NOT_FOUND_CODE,
                                        Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_OPERATOR_NOT_FOUND_DESCRIPTION
                                    };
                                    return response;
                                }

                                InventarioElementosAtributosEstados oEstado = cInventarioElementosAtributosEstados.GetEstadoByNombre(user.ClienteID.Value, Element.sStatus);
                                if (oEstado == null)
                                {
                                    response = new TBOResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_STATUS_NOT_FOUND_CODE,
                                        Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_STATUS_NOT_FOUND_DESCRIPTION
                                    };
                                    return response;
                                }

                                JsonObject jsonListas = new JsonObject();
                                JsonObject jsonListaValores, oJsonAux;
                                List<int> listaColumnasInvalidas = new List<int>();

                                if (nuevo.Attributes != null)
                                {
                                    object oAux;

                                    foreach (var a in nuevo.Attributes)
                                    {
                                        CoreAtributosConfiguraciones oAtr = cAtributos.GetAtributoByCodigoyCategoriaV2(a.sAttributeName, categoria.InventarioCategoriaID);
                                        if (oAtr != null)
                                        {
                                            if (oAtr.TiposDatos.Codigo == "LISTA" || oAtr.TiposDatos.Codigo == "LISTAMULTIPLE")
                                            {
                                                jsonListaValores = new JsonObject();
                                                if (oAtr.TablaModeloDatoID != null)
                                                {
                                                    jsonListaValores = cAtributos.GetJsonItemsServicio((long)oAtr.CoreAtributoConfiguracionID);
                                                }
                                                else if (oAtr.ValoresPosibles != null && oAtr.ValoresPosibles != "")
                                                {
                                                    foreach (var item in oAtr.ValoresPosibles.Split(';').ToList())
                                                    {
                                                        if (!jsonListaValores.TryGetValue(item, out oAux))
                                                        {
                                                            oJsonAux = new JsonObject();
                                                            oJsonAux.Add("Text", item);
                                                            oJsonAux.Add("Value", item);
                                                            jsonListaValores.Add(item, oJsonAux);
                                                        }
                                                    }
                                                }
                                                jsonListas.Add(oAtr.Nombre, jsonListaValores);
                                            }
                                            if (oAtr.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Fecha.ToUpper() && a.sAttributeValue != "")
                                            {
                                                a.sAttributeValue = DateTime.ParseExact(a.sAttributeValue, Comun.FORMATO_FECHA, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                                            }
                                            Object Atributo = new
                                            {
                                                AtributoID = oAtr.CoreAtributoConfiguracionID,
                                                NombreAtributo = oAtr.Nombre,
                                                Valor = a.sAttributeValue,
                                                TipoDato = oAtr.TiposDatos.Codigo,
                                                TextLista = a.sAttributeValue
                                            };
                                            listaAtributos.Add(Atributo);
                                        }
                                        else
                                        {
                                            response = new TBOResponse
                                            {
                                                Result = false,
                                                Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ELEMENT_ATTRIBUTE_NOT_FOUND_CODE,
                                                Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ELEMENT_ATTRIBUTE__NOT_FOUND_DESCRIPTION + ": " + a.sAttributeName
                                            };
                                            return response;
                                        }
                                    }
                                }
                                else
                                {
                                    response = new TBOResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ELEMENT_ATTRIBUTE_NOT_FOUND_CODE,
                                        Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ELEMENT_ATTRIBUTE__NOT_FOUND_DESCRIPTION
                                    };
                                    return response;
                                }

                                if (nuevo.Templates != null)
                                {
                                    foreach (var a in nuevo.Templates)
                                    {
                                        if (cAtrCategorias.ExistsCategory(a.sAttributeCategory, (long)categoria.ClienteID))
                                        {
                                            CoreInventarioPlantillasAtributosCategorias oPla = cPlantillas.GetPlantillaInventarioCategoria(a.sTemplateName, a.sAttributeCategory, categoria.InventarioCategoriaID);
                                            if (oPla != null)
                                            {
                                                Object plantilla = new
                                                {
                                                    PlantillaID = oPla.CoreInventarioPlantillaAtributoCategoriaID,
                                                    InvCatConfID = oPla.CoreInventarioCategoriaAtributoCategoriaConfiguracionID,
                                                    NombrePlantilla = oPla.Nombre
                                                };
                                                listaPlantillas.Add(plantilla);
                                            }
                                            else
                                            {
                                                response = new TBOResponse
                                                {
                                                    Result = false,
                                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_TEMPLATE_NOT_FOUND_CODE,
                                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_TEMPLATE_NOT_FOUND_DESCRIPTION + ": " + a.sTemplateName
                                                };
                                                return response;
                                            }
                                        }
                                        else
                                        {
                                            response = new TBOResponse
                                            {
                                                Result = false,
                                                Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ATRIBUTTE_NOT_FOUND_CODE,
                                                Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ATRIBUTTE_NOT_FOUND_DESCRIPTION + ": " + a.sAttributeCategory
                                            };
                                            return response;
                                        }
                                    }
                                }

                                List<string> listaCamposWarn;

                                if (bUpdate)
                                {
                                    elementoInventario = cInventarioElementos.GetByNumberAndClienteID(nuevo.sElementCode, user.ClienteID.Value);
                                    if (elementoInventario != null)
                                    {
                                        elementoInventario.NumeroInventario = nuevo.sElementCode;
                                        elementoInventario.Nombre = nuevo.sName;
                                        elementoInventario.OperadorID = oOperador.OperadorID;
                                        elementoInventario.InventarioCategoriaID = categoria.InventarioCategoriaID;
                                        elementoInventario.EmplazamientoID = oEmplazamiento.EmplazamientoID;
                                        elementoInventario.UltimaModificacionFecha = DateTime.Now;
                                        elementoInventario.UltimaModificacionUsuarioID = user.UsuarioID;
                                        InfoResponse responseCreate = cInventarioElementos.AddUpdateInventarioElemento(elementoInventario,
                                            listaAtributos,
                                            listaPlantillas,
                                            jsonListas
                                            );
                                        if (responseCreate.Result)
                                        {
                                            InventarioElementos inv = (InventarioElementos)responseCreate.Data;

                                            response = new TBOResponse()
                                            {
                                                Result = true,
                                                Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                                                Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                                                Data = ConvertToInventoryElementCat(inv, true, categoria.InventarioCategoria)
                                            };
                                        }
                                        else
                                        {
                                            response = new TBOResponse()
                                            {
                                                Result = responseCreate.Result,
                                                Code = responseCreate.Code,
                                                Description = responseCreate.Description
                                            };
                                            return response;
                                        }
                                    }
                                    else
                                    {
                                        response = new TBOResponse
                                        {
                                            Result = false,
                                            Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_CODE_NOT_FOUND_CODE,
                                            Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_CODE_NOT_FOUND_DESCRIPTION
                                        };
                                        return response;
                                    }
                                }
                                else
                                {
                                    elementoInventario = new InventarioElementos
                                    {
                                        InventarioCategoriaID = categoria.InventarioCategoriaID,
                                        Activo = true,
                                        Plantilla = false,
                                        FechaCreacion = DateTime.Now,
                                        FechaAlta = DateTime.Now,
                                        UltimaModificacionFecha = DateTime.Now,
                                        UltimaModificacionUsuarioID = user.UsuarioID,
                                        CreadorID = user.UsuarioID,
                                        NumeroInventario = nuevo.sElementCode,
                                        Nombre = nuevo.sName,
                                        OperadorID = oOperador.OperadorID,
                                        Emplazamientos = oEmplazamiento
                                    };
                                    InfoResponse responseCreate = cInventarioElementos.AddUpdateInventarioElemento(elementoInventario,
                                            listaAtributos,
                                            listaPlantillas,
                                            jsonListas
                                            );
                                    if (responseCreate.Result)
                                    {
                                        InventarioElementos inv = (InventarioElementos)responseCreate.Data;

                                        response = new TBOResponse()
                                        {
                                            Result = true,
                                            Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                                            Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                                            Data = ConvertToInventoryElementCat(inv, true, categoria.InventarioCategoria)
                                        };
                                    }
                                    else
                                    {
                                        response = new TBOResponse()
                                        {
                                            Result = responseCreate.Result,
                                            Code = responseCreate.Code,
                                            Description = responseCreate.Description
                                        };
                                        return response;
                                    }
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
                                return response;
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
                            return response;
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
                        return response;
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
                    return response;
                }
            }



            catch (Exception ex)
            {
                log.Error(ex.Message);
                response = new TBOResponse
                {
                    Result = false,
                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION + " " + ex.Message
                };
            }

            #region MONITORING_FINAL

            sParametroSalida = GetOutputParameter(response);
            sComentarios = response.Description;
            cMonitoring.AgregarRegistro(sTipoProyecto, monitoringUsuarioID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, monitoringClienteID, response.Result, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);

            #endregion

            // Returns the result
            return response;
        }
        #endregion

        #endregion

        #region SINGULAR INTERFACE

        #region _SI_GetElements
        /// <summary>
        /// Get inventory elements by customer
        /// </summary>
        /// <param name="sCustomer">Entity</param>
        /// <param name="sUser">User email</param>
        /// <param name="sConnectionCode">Conection code</param>
        /// <param name="sSiteCode">Site code</param>
        /// <param name="sCategoryAttribute">Category attribute</param>
        /// <param name="sCategoryElement">Category element</param>
        /// <param name="iPage">Page</param>
        /// <param name="iPageSize">Page size</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public TBOResponse _SI_GetElements(string sCustomer, string sUser, string sConnectionCode, [FromUri] string sSiteCode = "",
            [FromUri] string sCategoryAttribute = "", [FromUri] string sCategoryElement = "", [FromUri] int? iPage = 0, [FromUri] int? iPageSize = Comun.TBO.TBO_LIST_MAXIMUM_SIZE)
        {
            // Local variables
            TBOResponse response = null;
            List<InventarioElementos> listaInventarioElementos;
            bool bExito = true;
            string sMissing = string.Empty;

            //Controllers
            InventarioElementosController cInventariElementos = new InventarioElementosController();
            OperadoresController cOperadores = new OperadoresController();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            InventarioAtributosCategoriasController cInventarioAtributosCategorias = new InventarioAtributosCategoriasController();
            InventarioCategoriasController cInventarioCategorias = new InventarioCategoriasController();

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
                    else if (!string.IsNullOrEmpty(sSiteCode) && !cEmplazamientos.ExistsSite(sSiteCode, user.ClienteID.Value))
                    {
                        response = new TBOResponse()
                        {
                            Result = false,
                            Code = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_DESCRIPTION
                        };
                    }
                    else if (!string.IsNullOrEmpty(sCategoryAttribute) && !cInventarioAtributosCategorias.ExistsCategory(sCategoryAttribute, user.ClienteID.Value))
                    {
                        response = new TBOResponse()
                        {
                            Result = false,
                            Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ATTRIBUTE_NOT_FOUND_CODE,
                            Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ATTRIBUTE_NOT_FOUND_DESCRIPTION
                        };
                    }
                    else if (!string.IsNullOrEmpty(sCategoryElement) && !cInventarioCategorias.ExistsCategory(sCategoryElement, user.ClienteID.Value))
                    {
                        response = new TBOResponse()
                        {
                            Result = false,
                            Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ELEMENT_NOT_FOUND_CODE,
                            Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ELEMENT_NOT_FOUND_DESCRIPTION
                        };
                    }
                    #endregion

                    if (response == null)
                    {
                        InventarioCategorias categoriaElemento = cInventarioCategorias.GetInventarioCategoriaIDByNombre(sCategoryElement, user.ClienteID.Value);
                        int totalSize;

                        if (!string.IsNullOrEmpty(sSiteCode))
                        {
                            listaInventarioElementos = cInventariElementos.GetElementsByOperadorAndSite(sCustomer, sSiteCode, user.ClienteID.Value, categoriaElemento, iPageSize.Value, iPage.Value);
                            totalSize = cInventariElementos.CountElementsByOperadorAndSite(sCustomer, sSiteCode, user.ClienteID.Value, categoriaElemento);
                        }
                        else
                        {
                            //Parametro sSiteCode vacio
                            listaInventarioElementos = cInventariElementos.GetElementsByOperador(sCustomer, user.ClienteID.Value, categoriaElemento, iPageSize.Value, iPage.Value);
                            totalSize = cInventariElementos.CountElementsByOperador(sCustomer, user.ClienteID.Value, categoriaElemento);
                        }

                        List<InventoryElements> listResult = new List<InventoryElements>();

                        if (listaInventarioElementos != null && listaInventarioElementos.Count > 0)
                        {
                            for (int i = 0; i < listaInventarioElementos.Count; i++)
                            {
                                try
                                {
                                    InventarioElementos InventarioElementos = listaInventarioElementos[i];
                                    listResult.Add(ConvertToInventoryElement(InventarioElementos, true, sCategoryAttribute));
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex.Message);
                                }
                            }
                        }

                        response = new TBOResponse
                        {
                            Result = true,
                            Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                            Data = new ListReturnTreeObject<InventoryElements>()
                            {
                                iPage = iPage.Value,
                                iPageSize = iPageSize.Value,
                                iTotalSize = totalSize,
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
        /// Get inventory element by code
        /// </summary>
        /// <param name="sCustomer">Entity</param>
        /// <param name="sUser">User email</param>
        /// <param name="sConnectionCode">Conection code</param>
        /// <param name="sInventoryCode">Inventory code</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{sInventoryCode}")]
        public TBOResponse _SI_GetByCode(string sCustomer, string sUser, string sConnectionCode, string sInventoryCode, [FromUri] string sCategoryAttribute = "")
        {
            //Variables
            TBOResponse response = null;
            bool bExito = true;
            string sMissing = string.Empty;

            //Controllers
            OperadoresController cOperadores = new OperadoresController();
            InventarioElementosController cInventarioElementos = new InventarioElementosController();
            InventarioAtributosCategoriasController cInventarioAtributosCategorias = new InventarioAtributosCategoriasController();

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
                if (string.IsNullOrEmpty(sInventoryCode))
                {
                    bExito = false;
                    sMissing = sMissing + " - " + nameof(sInventoryCode) + " ";
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
                    else if (!string.IsNullOrEmpty(sCategoryAttribute) && !cInventarioAtributosCategorias.ExistsCategory(sCategoryAttribute, user.ClienteID.Value))
                    {
                        response = new TBOResponse()
                        {
                            Result = false,
                            Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ATTRIBUTE_NOT_FOUND_CODE,
                            Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_CATEGORY_ATTRIBUTE_NOT_FOUND_DESCRIPTION
                        };
                    }
                    #endregion

                    if (response == null)
                    {
                        InventarioElementos elemento = cInventarioElementos.GetElementsByOperadorAndElementCode(sCustomer, sInventoryCode, user.ClienteID.Value);

                        if (elemento != null)
                        {
                            response = new TBOResponse()
                            {
                                Result = true,
                                Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                                Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                                Data = ConvertToInventoryElement(elemento, true, sCategoryAttribute)
                            };
                        }
                        else
                        {
                            response = new TBOResponse()
                            {
                                Result = false,
                                Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_CODE_NOT_FOUND_CODE,
                                Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_CODE_NOT_FOUND_DESCRIPTION
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
        /// Get the latest modified elements
        /// </summary>
        /// <param name="iDays">Days ago</param>
        /// <param name="sCustomer">Entity</param>
        /// <param name="sUser">User email</param>
        /// <param name="iPage">Page</param>
        /// <param name="iPageSize">Page size</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLastModified")]
        public TBOResponse _SI_GetLastModified(int? iDays, string sUser, [FromUri] string sCustomer = "", [FromUri] int? iPage = 0, [FromUri] int? iPageSize = Comun.TBO.TBO_LIST_MAXIMUM_SIZE)
        {
            // Local variables
            TBOResponse response = null;
            List<InventarioElementos> listaInventarioElementos;
            bool bExito = true;
            string sMissing = string.Empty;

            //Controllers
            EntidadesController cEntidades = new EntidadesController();
            InventarioElementosController cInventarioElementos = new InventarioElementosController();
            InventarioElementosHistoricosController cInventarioElementosHistoricos = new InventarioElementosHistoricosController();

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
                            #endregion

                            if (response == null)
                            {
                                List<long> IDs = cInventarioElementosHistoricos.GetLastModified(iDays.Value, user.ClienteID.Value);

                                listaInventarioElementos = cInventarioElementos.GetInventarioElentosByIDs(IDs, sCustomer, user.ClienteID.Value, iPageSize.Value, iPage.Value);

                                List<InventoryElements> listResult = new List<InventoryElements>();

                                if (listaInventarioElementos != null && listaInventarioElementos.Count > 0)
                                {
                                    for (int i = 0; i < listaInventarioElementos.Count; i++)
                                    {
                                        try
                                        {
                                            InventarioElementos inventarioElemento = listaInventarioElementos[i];
                                            listResult.Add(ConvertToInventoryElementCodeName(inventarioElemento));
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex.Message);
                                        }
                                    }
                                }

                                response = new TBOResponse
                                {
                                    Result = true,
                                    Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                                    Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                                    Data = new ListReturnTreeObject<InventoryElements>()
                                    {
                                        iPage = iPage.Value,
                                        iPageSize = iPageSize.Value,
                                        iTotalSize = cInventarioElementos.CountGetInventarioElentosByIDs(IDs, sCustomer, user.ClienteID.Value),
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

        #region _SI_CreateRelationship
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sUser">User email</param>
        /// <param name="sParentInventoryCode">Parent inventory number</param>
        /// <param name="sChildInventoryCode">Child inventory number</param>
        /// <returns>Allows to establish linkages between inventory elements</returns>
        [HttpGet]
        [Route("CreateRelationship")]
        public TBOResponse _SI_CreateRelationship(string sUser, string sChildInventoryCode, string sParentInventoryCode = null)
        {
            // Local variables
            TBOResponse response = null;
            bool bExito = true;
            string sMissing = string.Empty;

            //Controllers
            InventarioElementosController cInventarioElementos = new InventarioElementosController();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            EmplazamientosTiposController cEmplazamientosTipos = new EmplazamientosTiposController();
            InventarioElementosVinculacionesController cInventarioElementosVinculaciones = new InventarioElementosVinculacionesController();
            InventarioCategoriasVinculacionesController cInventarioCategoriasVinculaciones = new InventarioCategoriasVinculacionesController();

            try
            {
                #region COMPROBAR CAMPOS VACIOS
                if (string.IsNullOrEmpty(sUser))
                {
                    bExito = false;
                    sMissing = sMissing + " - " + nameof(sUser) + " ";
                }
                if (string.IsNullOrEmpty(sChildInventoryCode))
                {
                    bExito = false;
                    sMissing = sMissing + " - " + nameof(sChildInventoryCode) + " ";
                }
                #endregion

                if (bExito)
                {
                    this.SetUsuario(sUser);

                    #region Comprobar campos en BBDD

                    if (this.user == null)
                    {
                        response = new TBOResponse()
                        {
                            Result = false,
                            Code = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_DESCRIPTION
                        };
                    }
                    else if (!cInventarioElementos.HasInventoryByNumberAndClienteID(sChildInventoryCode, this.user.ClienteID.Value))
                    {
                        response = new TBOResponse()
                        {
                            Result = false,
                            Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_CODE_NOT_FOUND_CODE,
                            Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_CODE_NOT_FOUND_DESCRIPTION + " - " + sChildInventoryCode
                        };
                    }
                    else if (!string.IsNullOrEmpty(sParentInventoryCode) && !cInventarioElementos.HasInventoryByNumberAndClienteID(sParentInventoryCode, this.user.ClienteID.Value))
                    {
                        response = new TBOResponse()
                        {
                            Result = false,
                            Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_CODE_NOT_FOUND_CODE,
                            Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_CODE_NOT_FOUND_DESCRIPTION + " - " + sParentInventoryCode
                        };
                    }
                    #endregion

                    if (response == null)
                    {
                        InventarioElementos elementoPadre = cInventarioElementos.GetByNumberAndClienteID(sParentInventoryCode, this.user.ClienteID.Value);
                        InventarioElementos elementoHijo = cInventarioElementos.GetByNumberAndClienteID(sChildInventoryCode, this.user.ClienteID.Value);

                        #region RECUPERAR EMPLAZAMIENTO TIPO
                        Emplazamientos emp = cEmplazamientos.GetItem(elementoHijo.EmplazamientoID.Value);
                        EmplazamientosTipos empTipo;
                        long? empTipoID = null;
                        if (emp.EmplazamientoTipoID.HasValue)
                        {
                            empTipo = cEmplazamientosTipos.GetItem(emp.EmplazamientoTipoID.Value);
                            if (empTipo != null)
                            {
                                empTipoID = empTipo.EmplazamientoTipoID;
                            }
                        }
                        #endregion

                        if (elementoPadre == null || (elementoPadre.EmplazamientoID == elementoHijo.EmplazamientoID))
                        {
                            long? elementoPadreID = null;
                            long? categoriaPadreID = null;
                            if (elementoPadre != null)
                            {
                                elementoPadreID = elementoPadre.InventarioElementoID;
                                categoriaPadreID = elementoPadre.InventarioCategoriaID;
                            }
                            long elementoHijoID = elementoHijo.InventarioElementoID;
                            long categoriaHijoID = elementoHijo.InventarioCategoriaID;

                            if (!cInventarioElementosVinculaciones.TienenVinculacion(elementoHijoID, elementoPadreID))
                            {
                                InventarioCategoriasVinculaciones invCatVinc = cInventarioCategoriasVinculaciones.GetVinculacionDefecto(categoriaHijoID, categoriaPadreID, empTipoID);

                                if (invCatVinc != null)
                                {
                                    #region CREAR VINCULACION
                                    InventarioElementosVinculaciones invElemVinc = new InventarioElementosVinculaciones()
                                    {
                                        InventarioCategoriaVinculacionID = invCatVinc.InventarioCategoriaVinculacionID,
                                        InventarioElementoID = elementoHijoID,
                                        InventarioElementoPadreID = elementoPadreID
                                    };

                                    invElemVinc = cInventarioElementosVinculaciones.AddItem(invElemVinc);
                                    if (invElemVinc != null)
                                    {
                                        response = new TBOResponse
                                        {
                                            Result = true,
                                            Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                                            Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION
                                        };
                                    }
                                    else
                                    {
                                        response = new TBOResponse
                                        {
                                            Result = false,
                                            Code = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_CODE,
                                            Description = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_DESCRIPTION
                                        };
                                    }
                                    #endregion
                                }
                                else
                                {
                                    response = new TBOResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_LINKAGE_NOT_ALLOWED_CODE,
                                        Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_LINKAGE_NOT_ALLOWED_DESCRIPTION
                                    };
                                }
                            }
                            else
                            {
                                response = new TBOResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ALREADY_LINK_BETWEEN_ELEMENTS_CODE,
                                    Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ALREADY_LINK_BETWEEN_ELEMENTS_DESCRIPTION
                                };
                            }
                        }
                        else
                        {
                            response = new TBOResponse
                            {
                                Result = false,
                                Code = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ELEMENTS_IN_DIFFERENT_SITES_CODE,
                                Description = ServicesCodes.INVENTORY_ELEMENT.COD_TBO_ELEMENTS_IN_DIFFERENT_SITES_DESCRIPTION
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

        #endregion

        #region Functions

        #region ConvertToInventoryElement
        public static InventoryElements ConvertToInventoryElement(InventarioElementos inventarioElemento, bool reducido, string sCategoryAttribute)
        {
            //Local variables
            InventoryElements result = new InventoryElements();

            //Controlls
            UsuariosController cUsuarios = new UsuariosController();
            InventarioCategoriasController cInventarioCategorias = new InventarioCategoriasController();

            try
            {
                if (inventarioElemento.InventarioElementoID > 0)
                {
                    result.lIdentifierLong = inventarioElemento.InventarioElementoID;
                }
                result.sElementCode = inventarioElemento.NumeroInventario;

                if (inventarioElemento.EmplazamientoID.HasValue)
                {
                    EmplazamientosController cEmplazamientos = new EmplazamientosController();
                    Emplazamientos empla = cEmplazamientos.GetItem(inventarioElemento.EmplazamientoID.Value);
                    if (empla != null)
                    {
                        result.sSiteCode = empla.Codigo;
                    }
                }

                if (inventarioElemento.InventarioElementoPadreID.HasValue)
                {
                    InventarioElementosController cInventarioElementos = new InventarioElementosController();
                    InventarioElementos inventarioElementos = cInventarioElementos.GetItem(inventarioElemento.InventarioElementoPadreID.Value);
                    if (inventarioElementos != null)
                    {
                        InventoryElementParent padre = new InventoryElementParent
                        {
                            sElementType = inventarioElementos.InventarioCategorias.InventarioCategoria,
                            sNumberInventory = inventarioElementos.NumeroInventario
                        };

                        //result.ParentElement = padre;
                    }
                }

                result.sCategory = cInventarioCategorias.GetItem(inventarioElemento.InventarioCategoriaID).InventarioCategoria;
                result.sCreator = cUsuarios.GetItem(inventarioElemento.CreadorID).EMail;
                if (!reducido)
                {
                    //result.dPurchaseOrder = inventarioElemento.NumPedido;
                    result.dAcquisitionDate = (inventarioElemento.FechaAdquisicion.HasValue) ? inventarioElemento.FechaAdquisicion.ToString() : string.Empty;
                }

                result.Attributes = new List<InventoryAttributes>();

                foreach (InventarioElementosAtributos atributo in inventarioElemento.InventarioElementosAtributos)
                {
                    Usuarios creador = cUsuarios.GetItem(atributo.CreadorID);
                    InventarioAtributosController cInventarioAtributos = new InventarioAtributosController();
                    InventarioAtributosController cInventarioAtributos2 = new InventarioAtributosController();
                    Vw_InventarioAtributos inventarioAtributo = cInventarioAtributos.GetItem<Vw_InventarioAtributos>(atributo.InventarioAtributoID);

                    bool returnAttribute = true;
                    if (!string.IsNullOrEmpty(sCategoryAttribute) && !(sCategoryAttribute == inventarioAtributo.InventarioAtributoCategoria))
                    {
                        returnAttribute = false;
                    }

                    if (returnAttribute)
                    {
                        InventoryAttributes attribute = new InventoryAttributes
                        {
                            sAttributeName = inventarioAtributo.NombreAtributo,
                            // sCategoryAttribute = inventarioAtributo.InventarioAtributoCategoria,
                            sAttributeValue = cInventarioAtributos2.GetLabelByAtributo(
                                                inventarioAtributo.InventarioAtributoID,
                                                inventarioAtributo.CodigoTipoDato,
                                                inventarioAtributo.NombreAtributo,
                                                inventarioAtributo.ValorDefecto,
                                                inventarioAtributo.ValoresPosibles,
                                                inventarioAtributo.Obligatorio,
                                                inventarioAtributo.NombreTabla,
                                                inventarioAtributo.TablaIndice,
                                                inventarioAtributo.TablaValor,
                                                inventarioAtributo.TablaControlador,
                                                inventarioAtributo.FuncionControlador,
                                                inventarioElemento.InventarioElementoID,
                                                atributo.Valor)
                        };

                        if (!reducido)
                        {
                            attribute.sAttributeCode = inventarioAtributo.CodigoAtributo;
                            attribute.dCreationDate = atributo.FechaCreacion.ToString();
                            attribute.sCreator = "";
                            attribute.sElementCode = result.sElementCode;
                        }

                        result.Attributes.Add(attribute);
                    }

                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = null;
            }

            return result;
        }
        #endregion

        #region ConvertToInventoryElementCat
        public static InventoryElements ConvertToInventoryElementCat(InventarioElementos inventarioElemento, bool reducido, string sCategoryAttribute)
        {
            //Local variables
            InventoryElements result = new InventoryElements();

            //Controlls
            UsuariosController cUsuarios = new UsuariosController();
            InventarioCategoriasController cInventarioCategorias = new InventarioCategoriasController();

            try
            {
                if (inventarioElemento.InventarioElementoID > 0)
                {
                    result.lIdentifierLong = inventarioElemento.InventarioElementoID;
                }
                result.sElementCode = inventarioElemento.NumeroInventario;

                if (inventarioElemento.EmplazamientoID.HasValue)
                {
                    EmplazamientosController cEmplazamientos = new EmplazamientosController();
                    Emplazamientos empla = cEmplazamientos.GetItem(inventarioElemento.EmplazamientoID.Value);
                    if (empla != null)
                    {
                        result.sSiteCode = empla.Codigo;
                    }
                }

                result.sCategory = cInventarioCategorias.GetItem(inventarioElemento.InventarioCategoriaID).InventarioCategoria;
                result.sCreator = cUsuarios.GetItem(inventarioElemento.CreadorID).EMail;
                if (!reducido)
                {
                    //result.dPurchaseOrder = inventarioElemento.NumPedido;
                    result.dAcquisitionDate = (inventarioElemento.FechaAdquisicion.HasValue) ? inventarioElemento.FechaAdquisicion.ToString() : string.Empty;
                }

                result.Attributes = new List<InventoryAttributes>();
                result.Templates = new List<InventoryTemplates>();

                if (reducido)
                {
                    var oJson = JObject.Parse(inventarioElemento.JsonAtributosDinamicos);
                    foreach (dynamic oAtr in oJson)
                    {
                        dynamic valor = oAtr.Value;
                        if (valor.TipoDato != null && valor.TipoDato == Comun.TIPODATO_CODIGO_FECHA)
                        {
                            if (valor.NombreAtributo != null)
                            {
                                InventoryAttributes attribute = new InventoryAttributes
                                {
                                    sAttributeName = valor.NombreAtributo,
                                    sAttributeValue = DateTime.Parse(valor.Valor.ToString()).ToString(Comun.FORMATO_FECHA)
                                };
                                result.Attributes.Add(attribute);
                            }
                        }
                        else
                        {
                            if (valor.NombreAtributo != null)
                            {
                                InventoryAttributes attribute = new InventoryAttributes
                                {
                                    sAttributeName = valor.NombreAtributo,
                                    sAttributeValue = valor.Valor.ToString()
                                };
                                result.Attributes.Add(attribute);
                            }
                        }
                    }

                    var oJsonPla = JObject.Parse(inventarioElemento.JsonPlantillas);
                    foreach (dynamic oPla in oJsonPla)
                    {
                        dynamic valor = oPla.Value;
                        InventoryTemplates template = new InventoryTemplates
                        {
                            sTemplateName = valor.NombrePlantilla,
                        };
                        result.Templates.Add(template);
                    }
                }
                else
                {
                    var oJson = JObject.Parse(inventarioElemento.JsonAtributosDinamicos);
                    CoreInventarioElementosAtributosController cElAtr = new CoreInventarioElementosAtributosController();
                    foreach (dynamic oAtr in oJson)
                    {
                        dynamic valor = oAtr.Value;
                        CoreInventarioElementosAtributos oEle = cElAtr.GetAtributo(inventarioElemento.InventarioElementoID, valor.AtributoID);
                        if (valor.TipoDato != null && valor.TipoDato == Comun.TIPODATO_CODIGO_FECHA)
                        {
                            if (valor.NombreAtributo != null)
                            {
                                InventoryAttributes attribute = new InventoryAttributes
                                {
                                    sAttributeName = valor.NombreAtributo,
                                    sAttributeValue = DateTime.Parse(valor.Valor.ToString()).ToString(Comun.FORMATO_FECHA),
                                    sAttributeCode = oEle.CoreAtributosConfiguraciones.Codigo,
                                    dCreationDate = oEle.FechaCreacion.ToString(),
                                    sCreator = oEle.CoreAtributosConfiguraciones.Codigo
                                };
                                result.Attributes.Add(attribute);
                            }
                        }
                        else
                        {
                            if (valor.NombreAtributo != null)
                            {
                                InventoryAttributes attribute = new InventoryAttributes
                                {
                                    sAttributeName = valor.NombreAtributo,
                                    sAttributeValue = valor.Valor.ToString(),
                                    sAttributeCode = oEle.CoreAtributosConfiguraciones.Codigo,
                                    dCreationDate = oEle.FechaCreacion.ToString(),
                                    sCreator = oEle.CoreAtributosConfiguraciones.Codigo
                                };
                                result.Attributes.Add(attribute);
                            }
                        }
                    }
                    var oJsonPla = JObject.Parse(inventarioElemento.JsonPlantillas);
                    foreach (dynamic oPla in oJsonPla)
                    {
                        dynamic valor = oPla.Value;
                        InventoryTemplates template = new InventoryTemplates
                        {
                            sTemplateName = valor.NombrePlantilla,
                        };
                        result.Templates.Add(template);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = null;
            }

            return result;
        }
        #endregion

        #region ConvertToInventoryElementCodeName
        public DTO.Salida.Inventory.InventoryElements ConvertToInventoryElementCodeName(InventarioElementos inventarioElemento)
        {
            //Variables
            InventoryElements result = new InventoryElements();

            //Controllers
            InventarioCategoriasController cInventarioCategorias = new InventarioCategoriasController();

            result.sElementCode = inventarioElemento.NumeroInventario;
            result.sCategory = cInventarioCategorias.GetItem(inventarioElemento.InventarioCategoriaID).InventarioCategoria;

            if (inventarioElemento.EmplazamientoID.HasValue)
            {
                EmplazamientosController cEmplazamientos = new EmplazamientosController();
                Emplazamientos empla = cEmplazamientos.GetItem(inventarioElemento.EmplazamientoID.Value);
                if (empla != null)
                {
                    result.sSiteCode = empla.Codigo;
                }
            }

            if (inventarioElemento.InventarioElementoPadreID.HasValue)
            {
                InventarioElementosController cInventarioElementos = new InventarioElementosController();
                InventarioElementos inventarioElementos = cInventarioElementos.GetItem(inventarioElemento.InventarioElementoPadreID.Value);
                if (inventarioElementos != null)
                {
                    InventoryElementParent padre = new InventoryElementParent
                    {
                        sElementType = inventarioElementos.InventarioCategorias.InventarioCategoria,
                        sNumberInventory = inventarioElementos.NumeroInventario
                    };

                    //result.ParentElement = padre;
                }
            }

            return result;
        }
        #endregion

        #region MONITORING

        #region GetInputParameter
        private string GetInputParameter(string Metodo, string sUser, DTO.Entrada.Inventory.InventoryElements element, string sProject, string sAgency, string sSeverity, string sTypology, string sConflictLevel)
        {
            string result = string.Empty;

            string url = this.Request.RequestUri.ToString(); //url con parametros
            string method = this.Request.Method.Method; //GET | POST | ...
            //string MediaType = this.Request.Content.Headers.ContentType.MediaType; //json:aplication

            result += method + Comun.NuevaLinea;
            result += url + Comun.NuevaLinea;


            //string lObjectType = (element != null) ? element.ToString() : string.Empty;
            //string Date = (element != null && element.dCreationDate != null) ? element.GetCreationDate().ToString() : string.Empty;

            switch (Metodo)
            {
                #region TBO_METODO_INVENTORY_ELEMENTS_CI_CREATE
                case Comun.TBO.Metodo.TBO_METODO_INVENTORY_ELEMENTS_CI_CREATE:

                    result += new JavaScriptSerializer().Serialize(element);

                    break;
                #endregion

                #region TBO_METODO_INVENTORY_ELEMENTS_SI_ACCEPT
                case Comun.TBO.Metodo.TBO_METODO_INVENTORY_ELEMENTS_SI_ACCEPT:
                #endregion
                default:
                    break;
            }

            return result;
        }
        #endregion

        #region GetOutputParameter
        private string GetOutputParameter(TBOResponse response)
        {
            string result = new JavaScriptSerializer().Serialize(response);


            return result;
        }
        #endregion

        #endregion

        #region ConvertToInventoryElement
        private DTO.Salida.Global.Sites ConvertToInventoryElement(Emplazamientos emplazamiento, bool reducido)
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
            }
            #endregion

            return site;
        }
        #endregion 

        #endregion

    }
}