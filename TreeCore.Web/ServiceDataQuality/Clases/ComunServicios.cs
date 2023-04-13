using CapaNegocio;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using Newtonsoft.Json.Linq;

public static class ComunServicios
{

    #region TIPOS DATOS

    public const string TIPODATO_CODIGO_NUMERICO = "NUMERICO";
    public const string TIPODATO_CODIGO_TEXTO = "TEXTO";
    public const string TIPODATO_CODIGO_FECHA = "FECHA";
    public const string TIPODATO_CODIGO_BOOLEAN = "BOOLEAN";
    public const string TIPODATO_CODIGO_MONEDA = "MONEDA";
    public const string TIPODATO_CODIGO_TEXTAREA = "TEXTAREA";
    public const string TIPODATO_CODIGO_LISTA = "LISTA";
    public const string TIPODATO_CODIGO_NUMERICO_FLOTANTE = "FLOTANTE";
    public const string TIPODATO_CODIGO_NUMERICO_ENTERO = "ENTERO";
    public const string TIPODATO_CODIGO_GEOPOSICION = "GEOPOSICION";
    public const string TIPODATO_CODIGO_LISTA_MULTIPLE = "LISTAMULTIPLE";

    #endregion

    #region FUNCIONES

    public static DateTime GetFechaExcel(string sDate, out bool correcto)
    {
        correcto = true;
        DateTime oDate = new DateTime();
        try
        {
            if (sDate.Length == 8)
            {
                oDate = new DateTime(int.Parse(sDate.Substring(0, 4)), int.Parse(sDate.Substring(4, 2)), int.Parse(sDate.Substring(6, 2)));
            }
            else
            {
                correcto = false;
                oDate = new DateTime();
            }
        }
        catch (Exception ex)
        {
            correcto = false;
            oDate = new DateTime().AddDays(1);
        }
        return oDate;
    }

    #endregion

    #region ENCRYPTATION

    #endregion

    #region TIPOS DATOS
    public static class TiposDatos
    {
        public const string Texto = "Texto";
        public const string Numerico = "Numerico";
        public const string Entero = "Entero";
        public const string GeoPosicion = "GeoPosicion";
        public const string Moneda = "Moneda";
        public const string Decimal = "Decimal";
        public const string Booleano = "Booleano";
        public const string Fecha = "Fecha";
        public const string Lista = "Lista";
        public const string ListaMultiple = "ListaMultiple";

        public static class BoolValues
        {
            public const string True = "true";
            public const string False = "false";
        }
    }


    #endregion

    #region SERVICIOS

    public const string SERVICIO_IMPORT_EXPORT = "IMPORT_EXPORT";
    public const string SERVICIO_CARGA_DISTRIBUCION_REGIONAL = "REGIONAL_DISTRIBUTION";
    public const string SERVICIO_CARGA_PLANTILLA_V23 = "V23";
    public const string SERVICIO_CARGA_INVENTARIO = "INVENTORY";
    public const string SERVICIO_CARGA_DOCUMENTOS = "DOCUMENTS";
    public const string SERVICIO_CARGA_PRIPIETARIOS = "OWNERS";
    public const string SERVICIO_CARGA_PROVEEDORES = "PROVIDERS";
    public const string SERVICIO_CARGA_EMPLAZAMIENTOS = "SITES";
    public const string SERVICIO_CARGA_EMPLAZAMIENTOS_CONTACTOS = "CONTACTS SITES";
    public const string SERVICIO_CARGA_CONTACTOS = "CONTACTS";
    public const string SERVICIO_CARGA_ENTIDADES = "ENTITIES";
    public const string SERVICIO_CARGA_VINCULACIONES_ELEMENTOS= "ELEMENTS RELATIONSHIPS";
    public const string SERVICIO_CARGA_SITES = "SITES";
    public const string SERVICIO_CARGA_FORM_SECTIONS = "FORM SECTIONS";
    public const string SERVICIO_CARGA_USUARIOS = "USERS";

    #endregion

    #region LISTAS CAMPOS VALIDOS HISTORICOS

    public static JObject ObjectToJSON(Object oJson, string nombre)
    {

        JObject historicoJSON = new JObject();
        var listaCamposValidos = ComunServicios.GetCamposValidosPorNombre(nombre);

        try
        {
            if (listaCamposValidos != null)
            {

                foreach (System.Reflection.PropertyInfo propiedad in oJson.GetType().GetProperties())
                {

                    if (listaCamposValidos.Contains(propiedad.Name))
                    {

                        string temp = "";
                        if (propiedad.GetValue(oJson, null) != null)
                        {

                            temp = propiedad.GetValue(oJson, null).ToString();
                        }

                        historicoJSON.Add(propiedad.Name, temp);

                        if (historicoJSON.Count == listaCamposValidos.Count())
                        {

                            break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

            historicoJSON = null;
            //Comun.cLog.EscribirLog(Comun.MensajeLog("Comun", "ObjectToJSON", ex.Message));
        }

        return historicoJSON;
    }

    public static List<string> GetCamposValidosPorNombre(string nombre)
    {
        List<string> listaValidos = new List<string>();

        switch (nombre)
        {

            case "Emplazamientos":
                listaValidos = CamposValidosEmplazamientos;
                break;
            case "InventarioElementos":
                listaValidos = CamposValidosInventario;
                break;
            default:
                listaValidos = null;
                break;
        }

        return listaValidos;
    }

    public static readonly List<string> CamposValidosEmplazamientos = new List<string>
    {

        "Codigo",
        "NombreSitio",
        "UsuarioModificador",
        "FechaModificacion",
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
        "Direccion",
        "Municipio",
        "Barrio",
        "CodigoPostal",
        "Latitud",
        "Longitud",
    };

    public static readonly List<string> CamposValidosEmplazamientosPrincipal = new List<string>
    {
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
        "UltimoUsuarioModificadorID",
        "FechaUltimaModificacion"
    };

    public static readonly List<string> CamposValidosEmplazamientosLocalizacion = new List<string>
    {
        "Direccion",
        "Municipio",
        "Barrio",
        "CodigoPostal",
        "Latitud",
        "Longitud"
    };

    public static readonly List<string> CamposValidosInventario = new List<string>
    {

        "NumeroInventario",
        "Nombre",
        "FechaCreacion",
        "FechaAlta"

        // LOS SIGUIENTES CAMPOS SE METEN COMO ATRIBUTOS DINAMICOS
        // PORQUE AL CREAR EL HISTORICO SE GUARDAN COMO ID:

        // "InventarioElementoPadre"
        // "Plantilla"
        // "Estado"
    };

    #endregion

}