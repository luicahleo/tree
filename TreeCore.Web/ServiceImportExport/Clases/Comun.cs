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
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using log4net;
using System.Reflection;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Data;


public static class Comun
{
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


#if SERVICESETTINGS
    public static string Version = System.Configuration.ConfigurationManager.AppSettings["Version"];
#elif TREEAPI
    public static string Version = TreeAPI.Properties.Settings.Default.Version;
#else
    public static string Version = TreeCore.Properties.Settings.Default.Version;

#endif


    #region PERFIL SUPER

    public const string PERFIL_SUPERUSUARIO = "SUPERUSUARIO";

    #endregion

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

    #region TIPOS FICHEROS EXPORTACION
    public const string TIPO_FICHERO_CSV = "csv";
    public const string TIPO_FICHERO_XLSX = "xlsx";
    #endregion

    #region FUNCIONES

    public static DateTime GetFechaExcel(string sDate)
    {
        DateTime oDate = new DateTime();
        try
        {
            if (sDate.Length == 8)
            {
                oDate = new DateTime(int.Parse(sDate.Substring(0, 4)), int.Parse(sDate.Substring(4, 2)), int.Parse(sDate.Substring(6, 2)));
            }
            else
            {
                oDate = new DateTime();
            }
        }
        catch (Exception)
        {
            oDate = new DateTime();
        }
        return oDate;
    }

    public static DateTime AtrFechaToDateTime(string sDate)
    {
        DateTime oDate;
        try
        {
            string dia, mes, ano;
            List<string> listaDatos = sDate.Split(' ')[0].Split('/').ToList();
            mes = (listaDatos[0].Length == 1) ? "0" + listaDatos[0] : listaDatos[0];
            dia = (listaDatos[1].Length == 1) ? "0" + listaDatos[1] : listaDatos[1];
            ano = listaDatos[2];
            oDate = new DateTime(int.Parse(ano), int.Parse(mes), int.Parse(dia));
        }
        catch (Exception)
        {
            oDate = new DateTime();
        }
        return oDate;
    }

    public static string SafeResourceLookup(dynamic currentPageClass, string key)
    {
        if (key == null ||
            key.Length == 0)
            return "SafeResourceLookup: Recurso no encontrado";

        try
        {
            return currentPageClass.GetLocalResourceObject(key).ToString();
        }
        catch (Exception)
        {
            try
            {
                return currentPageClass.SafeResourceLookup(key).ToString();
            }
            catch (Exception)
            {

            }
        }

        return key + " no existe";
    }

    public class Parametroslist
    {
        string _Codigo;
        string _Valor;

        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        public string Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }
    }

    public static double ConvertPuntosEnComasFloat(string mensaje)
    {
        string mensajeComas = "";
        double resultado = 0;

        if (mensaje == "")
        {
            resultado = 0;
        }

        else
        {
            mensajeComas = mensaje.Replace(".", ",");
            resultado = Convert.ToDouble(String.Format("{0:0.00}", mensajeComas));
        }

        return resultado;
    }

    public static string ConvertKeyXML(string sNombre)
    {
        string sKey = "";

        try
        {
            if (sNombre.Contains(" "))
            {
                sKey = sNombre.Replace(" ", "_");
            }
            else
            {
                sKey = sNombre;
            }
        }
        catch (Exception)
        {
            sKey = null;
        }

        return sKey;
    }

    public static double ConvertCambioMoneda(string sMensaje)
    {
        string sMensajeComas = "";
        double dResultado = 0;

        if (sMensaje == "")
        {
            dResultado = 0;
        }
        else
        {
            sMensajeComas = sMensaje.Replace(".", ",");
            dResultado = Convert.ToDouble(String.Format("{0:0.00}", sMensajeComas));
        }

        return dResultado;

    }

    public static string rutaIconoWeb(string archivo)
    {
        string ruta = string.Empty;
        try
        {
            ruta = string.Concat(GetDirectorioICono(), archivo);
        }
        catch (Exception)
        {
            ruta = archivo;
        }

        return ruta;
    }

    public static string rutaIconoWebInventario(string archivo)
    {
        string ruta = string.Empty;
        try
        {
            ruta = string.Concat(GetDirectorioIConoInventario(), archivo);
        }
        catch (Exception)
        {
            ruta = archivo;
        }

        return ruta;
    }

    public static string rutaIconoSystem()
    {
        string ruta = string.Empty;
        try
        {
            string parametro = GetDirectorioICono();
            ruta = HttpContext.Current.Server.MapPath(parametro);
        }
        catch (Exception)
        {
            ruta = string.Empty;
        }

        return ruta;
    }
    public static string rutaIconoInventario()
    {
        string ruta = string.Empty;
        try
        {
            string parametro = GetDirectorioIConoInventario();
            ruta = HttpContext.Current.Server.MapPath(parametro);
        }
        catch (Exception)
        {
            ruta = string.Empty;
        }

        return ruta;
    }

    public static string GetDirectorioICono()
    {
        ParametrosController cParametros = new ParametrosController();
        return cParametros.GetItemByName(Comun.RUTA_DIRECTORIO_ICONOS).Valor;
    }
    public static string GetDirectorioIConoInventario()
    {
        ParametrosController cParametros = new ParametrosController();
        return cParametros.GetItemByName(Comun.RUTA_DIRECTORIO_ICONOS_INVENTARIO).Valor;
    }
    public static long GetRestriccionDefectoInventario()
    {
        ParametrosController cParametros = new ParametrosController();
        return long.Parse(cParametros.GetItemByName(Comun.RESTRICCION_DEFECTO_INVENTARIO).Valor);
    }
    public static long GetRestriccionDefectoEmplazamientos()
    {
        ParametrosController cParametros = new ParametrosController();
        return long.Parse(cParametros.GetItemByName(Comun.RESTRICCION_DEFECTO_EMPLAZAMIENTO).Valor);
    }
    public static void SetRestriccionDefectoInventario(long lRest)
    {
        ParametrosController cParametros = new ParametrosController();
        TreeCore.Data.Parametros oParamentro = cParametros.GetItemByName(Comun.RESTRICCION_DEFECTO_INVENTARIO);
        oParamentro.Valor = lRest.ToString();
        cParametros.UpdateItem(oParamentro);
    }
    public static void SetRestriccionDefectoEmplazamientos(long lRest)
    {
        ParametrosController cParametros = new ParametrosController();
        TreeCore.Data.Parametros oParamentro = cParametros.GetItemByName(Comun.RESTRICCION_DEFECTO_EMPLAZAMIENTO);
        oParamentro.Valor = lRest.ToString();
        cParametros.UpdateItem(oParamentro);
    }

    #endregion

    #region ENCRYPTATION
    public const int KEY_MAX_LENGTH = 72;
    public const string KEY_ACTIVATION_DAY_CODE = "LT";
    public const string KEY_ACTIVATION_MONTH_CODE = "HY";
    public const string KEY_ACTIVATION_YEAR_CODE = "3W";
    public const string KEY_CANCEL_DAY_CODE = "PS";
    public const string KEY_CANCEL_MONTH_CODE = "JZ";
    public const string KEY_CANCEL_YEAR_CODE = "ER";
    public const string KEY_LICENSES_NUMBER_CODE = "FC";
    public const string KEY_CLIENT_CODE = "BM";
    public const string KEY_LICENSE_TYPE_CODE = "RR";
    public const int KEY_ACTIVATION_DATE_DELAY = 21;
    public const int KEY_CANCEL_DATE_DELAY = 13;
    public const int KEY_LICENSES_NUMBER_DELAY = 48;
    public const int KEY_CLIENT_CODE_DELAY = 29;
    public const int KEY_LICENSE_TYPE_DELAY = 40;
    public const int KEY_LICENSE_CRC_MOD = 71;
    public const string KEY_LICENSE_CRC_SUPPLEMENT = "V";
    public const string KEY_SEPARATOR = "-";

    public const string gClaveEncriptacion = "5a5c24429eb5c23b7829b9eba78da1b3b3e93150fb54d625ee3d0cd80975d725";

    #endregion

    #region EXCEPCIONES GENERALES

    /// <summary>
    /// CONSTANTES DE LAS EXCEPCIONES CONTROLADAS
    /// </summary>
    public static string cod100 = Resources.Comun.strCodigoExcepcion100;
    public static string cod110 = Resources.Comun.strCodigoExcepcion110;
    public static string cod111 = Resources.Comun.strCodigoExcepcion111;
    public static string cod112 = Resources.Comun.strCodigoExcepcion112;
    public static string cod113 = Resources.Comun.strCodigoExcepcion113;
    public static string cod114 = Resources.Comun.strCodigoExcepcion114;
    public static string cod120 = Resources.Comun.strCodigoExcepcion120;
    public static string cod121 = Resources.Comun.strCodigoExcepcion121;
    public static string cod122 = Resources.Comun.strCodigoExcepcion122;
    public static string cod130 = Resources.Comun.strCodigoExcepcion130;
    public static string cod131 = Resources.Comun.strCodigoExcepcion131;
    public static string cod132 = Resources.Comun.strCodigoExcepcion132;
    public static string cod140 = Resources.Comun.strCodigoExcepcion140;

    #endregion

    #region CONSTANTES

    /// <summary>
    /// CONTADOR DE INTENTO EN LOGIN
    /// </summary>
    public static int IntentosClave = 0;

    /// <summary>
    /// CONSTANTES DE LICENCIAS
    /// </summary>
    public const string MODULOGLOBAL = "GLOBAL";
    public const string MODULOMANTENIMIENTO = "MANTENIMIENTO";
    public const string MODULOSPACE = "SPACE";
    public const string MODULOADQUISICIONESBUSQUEDA = "ADQUISICIONES_BUSQUEDA";
    public const string MODULOACCESS = "ACCESS";
    public const string MODULOADQUISICIONES = "ADQUISICIONES";
    public const string MODULOADQUISICIONESSARF = "ADQUISICIONES_SARF";
    public const string MODULOAMBIENTAL = "AMBIENTAL";
    public const string MODULOAMPLIACIONES = "AMPLIACIONES";
    public const string MODULOASSETSPURCHASE = "ASSETSPURCHASE";
    public const string MODULOAUDIT = "AUDIT";
    public const string MODULOCITY = "CITY";
    public const string MODULODESPLIEGUE = "DESPLIEGUE";
    public const string MODULODOCUMENTAL = "DOCUMENTAL";
    public const string MODULOENERGY = "ENERGY";
    public const string MODULOFINANCIERO = "FINANCIERO";
    public const string MODULOFIRMADIGITAL = "FIRMA_DIGITAL";
    public const string MODULOINDOOR = "INDOOR";
    public const string MODULOINSTALLOBRACIVIL = "INSTALL_OBRA_CIVIL";
    public const string MODULOINSTALLTECNICA = "INSTALL_TECNICA";
    public const string MODULOINVENTARIO = "INVENTARIO";
    public const string MODULOLEGAL = "LEGAL";
    public const string MODULOPLANNING = "PLANNING";
    public const string MODULOSAVING = "SAVING";
    public const string MODULOSHARING = "SHARING";
    public const string MODULOSSRR = "SSRR";
    public const string MODULOSWAPPING = "SWAPPING";
    public const string MODULOTORREROSARF = "TORRERO_SARF";
    public const string MODULOTORRERO = "TORRERO";
    public const string MODULOUNINSTALLADMIN = "UNINSTALL_ADMIN";
    public const string MODULOUNINSTALLTECNICA = "UNINSTALL_TECNICA";
    public const string MODULOUNINSTALLELECTRICA = "UNINSTALL_ELECTRICA";
    public const string MODULOVANDALISMO = "VANDALISMO";
    public const string MODIFICAR_PERFILES_USUARIO = "MODIFICAR PERFILES USUARIO";
    public const string MODIFICAR_PROYECTOS_TIPOS = "MODIFICAR_PROYECTOS_TIPOS";
    public const string MODULOMONITORING = "MONITORING";
    public const string MODULOIMPORT_EXPORT = "EXPORTAR_IMPORTAR";
    public const string MODULOCALIDAD = "CALIDAD";

    /// <summary>
    /// Codigos Acción
    /// </summary>
    public const string VISITAR_PAGINA = "VISITAR_PAGINA";

    // Nueva linea
    public const char NuevaLinea = '\n';
    /// <summary>
    /// Idioma por defecto del sitio web
    /// </summary>
    public const string DefaultLocale = "es-ES";

    public static string CultureBBDD = "";

    /// <summary>
    /// Mensaje para Sesiones Concurrentes
    /// </summary>
    public static string MsgSesionConcurrente = Resources.Comun.strMsgSesionConcurrente;

    /// <summary>
    /// Titulo mensaje de control de código de seguridad
    /// </summary>
    /// 
    public static string MsgPaginaNoPermitida = Resources.Comun.strMsgPaginaNoPermitida;

    /// <summary
    /// Error ajax que se mostrará por defecto
    /// </summary>
    public static string ERRORAJAXGENERICO = Resources.Comun.strErrorGenerico;

    /// <summary>
    /// Mensaje para clave o usuario Incorrectos
    /// </summary>
    /// 
    public static string MsgTituloDublicado = Resources.Comun.strSesionDuplicada;

    /// <summary>
    /// Mensaje para clave o usuario Incorrectos
    /// </summary>
    /// 
    public static string MsgDublicado = Resources.Comun.strMsgSesionDuplicada;

    #endregion

    #region RECURSOS

    public static string NOMBRE_FICHERO_RECURSOS = "Comun";

    #region LOG

    public static string LogFiltrosPerfilesCreados = "LogFiltrosPerfilesCreados";
    public static string LogFiltrosInventarioGestion = "LogFiltrosInventarioGestion";
    public static string LogFiltrosEntidades = "LogFiltrosEntidades";
    public static string LogSeleccionElementoGrilla = "LogSeleccionElementoGrilla";
    public static string LogEstadisticasAgregadas = "LogEstadisticasAgregadas";
    public static string LogExcelExportado = "LogExcelExportado";
    public static string LogRegistroExistente = "LogRegistroExistente";
    public static string LogActualizacionRealizada = "LogActualizacionRealizada";
    public static string LogAgregacionRealizada = "LogAgregacionRealizada";
    public static string LogCambioRegistroPorDefecto = "LogCambioRegistroPorDefecto";
    public static string LogEliminacionRealizada = "LogEliminacionRealizada";
    public static string LogActivacionRealizada = "LogActivacionRealizada";
    public static string LogDispositivoDesvinculado = "LogDispositivoDesvinculado";
    public static string LogCambioAlmacen = "LogCambioAlmacen";
    public static string LogCambioBaja = "LogCambioBaja";
    public static string LogCambioMantenimiento = "LogCambioMantenimiento";
    public static string LogCambioReparacion = "LogCambioReparacion";
    public static string LogCambioTraslado = "LogCambioTraslado";
    public static string LogActualizacionNombre = "LogActualizacionNombre";
    public static string LogActualizacionApellidos = "LogActualizacionApellidos";
    public static string LogActualizacionEmail = "LogActualizacionEmail";
    public static string LogActualizacionClave = "LogActualizacionClave";
    public static string LogEstructuraActualizada = "LogEstructuraActualizada";
    // Import-Export Migrador
    //Export
    public static string LogTablaMigrada = "LogTablaMigrada";
    public static string LogTablaMigradaWith = "LogTablaMigradaWith";
    public static string LogTablaMigradaRows = "LogTablaMigradaRows";
    //Import
    public static string LogTablaImport = "LogTablaImport";
    public static string LogTablaImportRow = "LogTablaImportRow";
    public static string LogTablaProblem = "LogTabla";

    #endregion

    #region ESTADISTICA
    public static string CommentEstadisticaPageInit = "CommentEstadisticaPageInit";
    public static string CommentEstadisticasAgregarProyectoTipo = "CommentEstadisticasAgregarProyectoTipo";
    #endregion

    #region CALIDAD
    public static string PARAM_IDS_RESULTADOS = "idsResultados";
    public static string PARAM_NAME_INDICE_ID = "nameIndiceID";
    #endregion

    #region JS

    public static string jsInfo = "jsInfo";
    public static string jsYaExiste = "jsYaExiste";
    public static string jsEmailExiste = "jsEmailExiste";
    public static string jsTelefonoExiste = "jsTelefonoExiste";
    public static string jsPorDefecto = "jsPorDefecto";
    public static string jsEntidadCliente = "jsEntidadCliente";
    public static string jsTieneRegistros = "jsTieneRegistros";
    public static string jsTieneRegistrosDeseaEliminar = "jsTieneRegistrosDeseaEliminar";
    public static string jsActTieneRegistros = "jsActTieneRegistros";
    public static string jsControlDuplicidad = "jsControlDuplicidad";
    public static string jsCodigoExiste = "jsCodigoExiste";
    public static string jsCategoriaExiste = "jsCategoriaExiste";
    public static string jsOrdenExiste = "jsOrdenExiste";
    public static string jsDesactivarPorDefecto = "jsDesactivarPorDefecto";
    public static string jsReplicarTitulo = "jsReplicarTitulo";
    public static string jsReplicarExitomsg = "jsReplicarExitomsg";
    public static string jsRegistroDuplicado = "jsRegistroDuplicado";
    public static string jsTituloModulo = "jsTituloModulo";
    public static string jsFechaIncorrecta = "jsFechaIncorrecta";
    public static string jsFechaCaducidadClave = "jsFechaCaducidadClave";
    public static string jsModificarUsuario = "jsModificarUsuario";
    public static string jsNivelMenuNoPermitido = "jsNivelMenuNoPermitido";
    public static string jsAgregarEnPaginaNoPermitido = "jsAgregarEnPaginaNoPermitido";
    public static string jsStatusBar = "jsStatusBar";
    public static string jsStatusBarNothingFound = "jsStatusBarNothingFound";
    public static string jsClaveDiferente = "jsClaveDiferente";
    public static string jsContactoInactivo = "jsContactoInactivo";
    public static string jsInicio = "jsInicio";
    public static string jsMunicipalidades = "jsMunicipalidades";
    public static string jsTipologias = "jsTipologias";
    public static string jsInventarioTiposVinculaciones = "jsInventarioTiposVinculaciones";
    public static string jsPerfiles = "jsPerfiles";
    public static string jsProyectoEstado = "jsProyectoEstado";
    public static string jsServiciosFrecuencias = "jsServiciosFrecuencias";
    public static string jsCatalogosTiposServicios = "jsCatalogosTiposServicios";
    public static string jsCatalogosTipos = "jsCatalogosTipos";
    public static string jsAgrupacionEstados = "jsAgrupacionEstados";
    public static string jsTablasModeloDatos = "jsTablasModeloDatos";
    public static string jsFuenteDatos = "jsFuenteDatos";
    public static string jsExtensionNoPermitida = "jsExtensionNoPermitida";
    public static string jsNumeroMaximoCaracteres = "jsNumeroMaximoCaracteres";
    public static string jsFrecuencia = "jsFrecuencia";
    public static string jsUnidades = "jsUnidades";
    public static string jsSubiendo = "jsSubiendo";
    public static string jsEspera = "jsEspera";
    public static string jsMensajeGenerico = "jsMensajeGenerico";
    public static string jsActivo = "jsActivo";
    public static string jsTamanoDocumentoExcedido = "jsTamanoDocumentoExcedido";
    public static string strFechaFormatoIncorrecto = "strFechaFormatoIncorrecto";
    public static string jsTamanoCodigoExcedido = "jsTamanoCodigoExcedido";
    public static string jsCaracteres = "jsCaracteres";
    public static string jsNombreColumna = "jsNombreColumna";


    #endregion

    #region STR

    public static string strMensajeGenerico = "strMensajeGenerico";
    public static string strFabricante = "strFabricante";
    public static string strActivo = "strActivo";
    public static string strModelo = "strModelo";
    public static string strModificado = "strModificado";
    public static string strDefecto = "strDefecto";
    public static string strPais = "strPais";
    public static string strDimensiones = "strDimensiones";
    public static string strAEV = "strAEV";
    public static string strAEVCA = "strAEVCA";
    public static string strCA = "strCA";
    public static string strRaiz = "strRaiz";
    public static string strNuevo = "strNuevo";
    public static string strActualizado = "strActualizado";
    public static string strCOLUMNA_NO_TRADUCIDA = "strCOLUMNA_NO_TRADUCIDA";
    public static string strNuevoAtributo = "strNuevoAtributo";
    public static string strMes = "strMes";
    public static string strValor = "strValor";
    public static string strLogs = "strLogs";
    public static string strLogsServicio = "strLogsServicio";
    public static string strContactos = "strContactos";
    public static string strEmplazamientos = "strEmplazamientos";
    public static string strExportarLog = "strExportarLog";
    public static string strArchivoInexistente = "strArchivoInexistente";
    public static string strPlantillas = "strPlantillas";
    public static string strTipoArchivo = "strTipoArchivo";
    public static string strInventario = "strInventario";
    public static string strLocalizacion = "strLocalizacion";
    public static string strDocumentos = "strDocumentos";
    public static string strRegexSeparador = "strRegexSeparador";
    public static string regexNombreText = "regexNombreText";
    public static string strRegexNumerico = "strRegexNumerico";
    public static string strRegexCaracter = "strRegexCaracter";
    public static string strCalidadKPI = "strCalidadKPI";
    public static string strCalidadKPIResults = "strCalidadKPIResults";
    public static string strDocumentoEstadoPorDefectoNoEncontrado = "strDocumentoEstadoPorDefectoNoEncontrado";
    public static string strNombreYaExisteParaElObjeto = "strNombreYaExisteParaElObjeto";
    public static string strDocTipoExistente = "strDocTipoExistente";
    public static string strAccesoDenegadoMultihomming = "strAccesoDenegadoMultihomming";
    public static string strProyectos = "strProyectos";
    public static string strOperacionRealizada = "strOperacionRealizada";
    public static string strDocumentosMovidosSatisfactorio = "strDocumentosMovidosSatisfactorio";
    public static string strDocumentosNoMovidosSatisfactorio = "strDocumentosNoMovidosSatisfactorio";
    public static string strGeneracionNombreFallida = "strGeneracionNombreFallida";
    public static string strGeneracionCodigoFallida = "strGeneracionCodigoFallida";
    public static string strGeneracionSinRegla = "strGeneracionSinRegla";
    public static string strLogVacio = "strLogVacio";
    public static string jsLogVacio = "jsLogVacio";
    public static string strUsuarioSinRoles = "strUsuarioSinRoles";

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
        public const string Boolean = "Boolean";
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

    #region OPERADORES FILTROS
    public const string OPERADOR_IGUAL = "IGUAL";
    public const string OPERADOR_MAYOR = "MAYOR";
    public const string OPERADOR_MENOR = "MENOR";
    #endregion

    #region TiposDinamicosFiltro

    public class TABS_EMPLAZAMIENTO
    {
        public const string TAB_LOCALIZACIONES = "Localizaciones";
        public const string TAB_CONTACTO = "Contactos";
        public const string TAB_MAP = "Maps";
        public const string TAB_SITE = "Sites";
        public const string TAB_INVENTARIO = "Inventario";
        public const string TAB_DOCUMENTOS = "Documentos";
        public const string TAB_ATRIBUTOS = "Atributos";
    }

    public class VISTAS_EMPLAZAMIENTOS
    {
        public const string GENERAL = "GENERAL";
        public const string INVENTARIO = "INVENTARIO";
        public const string DOCUMENTOS = "DOCUMENTOS";
        public const string CONTACTOS = "CONTACTOS";
    }

    public class TiposDinamicosFiltro
    {
        public TiposDinamicosFiltro(string tabla, string tagTraduction, string nameTableId, string name, string tab, string nameHeaderVwFilter, string nameHeaderVw, string vista)
        {
            this.tabla = tabla;
            this.tagTraduction = tagTraduction;
            this.nameTableId = nameTableId;
            this.name = name;
            this.tab = tab;
            this.nameHeaderVwFilter = nameHeaderVwFilter;
            this.nameHeaderVwTab = nameHeaderVw;
            this.vista = vista;
        }

        public TiposDinamicosFiltro(string tagTraduction, string tab, string nameHeaderVwFilter, string nameHeaderVw, string vista)
        {
            this.tagTraduction = tagTraduction;
            this.tab = tab;
            this.nameHeaderVwFilter = nameHeaderVwFilter;
            this.nameHeaderVwTab = nameHeaderVw;
            this.vista = vista;
        }

        public string tabla;
        public string tagTraduction;
        public string nameTableId;
        public string name;
        public string tab;
        public string nameHeaderVwFilter;
        public string nameHeaderVwTab;
        public string vista;
    }

    public static Dictionary<string, TiposDinamicosFiltro> columTagCoreEmplazamientosFiltros = new Dictionary<string, TiposDinamicosFiltro>() {
        {"Codigo", new TiposDinamicosFiltro("strCodigo", TABS_EMPLAZAMIENTO.TAB_SITE, "Codigo", "Codigo", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"NombreSitio", new TiposDinamicosFiltro("strNombreSitio", TABS_EMPLAZAMIENTO.TAB_SITE, "NombreSitio", "NombreSitio", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"Proyectos", new TiposDinamicosFiltro("strProyectos", TABS_EMPLAZAMIENTO.TAB_SITE, "Proyectos", "Proyectos", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"Contratos", new TiposDinamicosFiltro("strContratos", TABS_EMPLAZAMIENTO.TAB_SITE, "Contratos", "Contratos", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"Operador", new TiposDinamicosFiltro("Vw_EntidadesOperadores", "strOperador", "OperadorID", "Nombre", TABS_EMPLAZAMIENTO.TAB_SITE, "OperadorID", "OperadorID", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"MonedaID", new TiposDinamicosFiltro("Monedas", "strMoneda", "MonedaID", "Moneda", TABS_EMPLAZAMIENTO.TAB_SITE, "MonedaID", "Moneda", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"EstadoGlobalID", new TiposDinamicosFiltro("EstadosGlobales", "strEstadoGlobal", "EstadoGlobalID", "EstadoGlobal", TABS_EMPLAZAMIENTO.TAB_SITE, "EstadoGlobalID", "EstadoGlobal", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"EmplazamientoCategoriaSitioID", new TiposDinamicosFiltro("EmplazamientosCategoriasSitios", "strCategoriaSitio", "EmplazamientoCategoriaSitioID", "CategoriaSitio", TABS_EMPLAZAMIENTO.TAB_SITE, "EmplazamientoCategoriaSitioID", "CategoriaSitio", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"EmplazamientoTipoID", new TiposDinamicosFiltro("EmplazamientosTipos", "strTipo", "EmplazamientoTipoID", "Tipo", TABS_EMPLAZAMIENTO.TAB_SITE, "EmplazamientoTipoID", "Tipo", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"EmplazamientoTipoEdificioID", new TiposDinamicosFiltro("EmplazamientosTiposEdificios", "strTipoEdificio", "EmplazamientoTipoEdificioID", "TipoEdificio", TABS_EMPLAZAMIENTO.TAB_SITE, "EmplazamientoTipoEdificioID", "TipoEdificio", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"EmplazamientoTipoEstructuraID", new TiposDinamicosFiltro("EmplazamientosTiposEstructuras", "strTipoEstructura", "EmplazamientoTipoEstructuraID", "TipoEstructura", TABS_EMPLAZAMIENTO.TAB_SITE, "EmplazamientoTipoEstructuraID", "TipoEstructura", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"EmplazamientoTamanoID", new TiposDinamicosFiltro("EmplazamientosTamanos", "strTamano", "EmplazamientoTamanoID", "Tamano", TABS_EMPLAZAMIENTO.TAB_SITE, "EmplazamientoTamanoID", "Tamano", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"FechaActivacion", new TiposDinamicosFiltro("strFechaActivacion", TABS_EMPLAZAMIENTO.TAB_SITE, "FechaActivacion", "FechaActivacion", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"FechaDesactivacion", new TiposDinamicosFiltro("strFechaDesactivacion", TABS_EMPLAZAMIENTO.TAB_SITE, "FechaDesactivacion", "FechaDesactivacion", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"RegionID", new TiposDinamicosFiltro("Regiones", "strRegion", "RegionID", "Region", TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES, "RegionID", "Region", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"PaisID", new TiposDinamicosFiltro("Paises", "strPais", "PaisID", "Pais", TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES, "PaisID", "Pais", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"RegionPaisID", new TiposDinamicosFiltro("RegionesPaises", "strRegionPais", "RegionPaisID", "RegionPais", TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES, "RegionPaisID", "RegionPais", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"ProvinciaID", new TiposDinamicosFiltro("Provincias", "strProvincia", "ProvinciaID", "Provincia", TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES, "ProvinciaID", "Provincia", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"MunicipioID", new TiposDinamicosFiltro("Municipios", "strMunicipio", "MunicipioID", "Municipio", TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES, "MunicipioID", "Municipio", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"Barrio", new TiposDinamicosFiltro("strBarrio", TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES, "Barrio", "Barrio", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"Direccion", new TiposDinamicosFiltro("strDireccion", TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES, "Direccion", "Direccion", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"CodigoPostal", new TiposDinamicosFiltro("strCodigoPostal", TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES, "CodigoPostal", "CodigoPostal", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"Latitud", new TiposDinamicosFiltro("strLatitud", TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES, "Latitud", "Latitud", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"Longitud", new TiposDinamicosFiltro("strLongitud", TABS_EMPLAZAMIENTO.TAB_LOCALIZACIONES, "Longitud", "Longitud", VISTAS_EMPLAZAMIENTOS.GENERAL)},
        {"Email", new TiposDinamicosFiltro("strEmailContacto", TABS_EMPLAZAMIENTO.TAB_CONTACTO, "Email", "Email", VISTAS_EMPLAZAMIENTOS.CONTACTOS)},
        {"Telefono", new TiposDinamicosFiltro("strTelefonoContacto", TABS_EMPLAZAMIENTO.TAB_CONTACTO, "Telefono", "Telefono", VISTAS_EMPLAZAMIENTOS.CONTACTOS)},
        {"ContactoTipoID", new TiposDinamicosFiltro("ContactosTipos", "strContactoTipo", "ContactoTipoID", "ContactoTipo", TABS_EMPLAZAMIENTO.TAB_CONTACTO, "ContactoTipoID", "ContactoTipoID", VISTAS_EMPLAZAMIENTOS.CONTACTOS)},
        {"InventarioCategoriaID", new TiposDinamicosFiltro("InventarioCategorias", "strCategoriaInventario", "InventarioCategoriaID", "InventarioCategoria", TABS_EMPLAZAMIENTO.TAB_INVENTARIO, "InventarioCategoriaID", "InventarioCategoriaID", VISTAS_EMPLAZAMIENTOS.INVENTARIO)},
        {"OperadorInventarioID", new TiposDinamicosFiltro("Vw_EntidadesOperadores", "strOperadorInventario", "OperadorID", "Nombre", TABS_EMPLAZAMIENTO.TAB_INVENTARIO, "OperadorInventarioID", "OperadorID", VISTAS_EMPLAZAMIENTOS.INVENTARIO)},
        {"InventarioElementoAtributoEstadoID", new TiposDinamicosFiltro("InventarioElementosAtributosEstados", "strEstadoInventario", "InventarioElementoAtributoEstadoID", "Nombre", TABS_EMPLAZAMIENTO.TAB_INVENTARIO, "InventarioElementoAtributoEstadoID", "InventarioElementoAtributoEstadoID", VISTAS_EMPLAZAMIENTOS.INVENTARIO)},
        {"DocumentTipoID", new TiposDinamicosFiltro("DocumentTipos", "strTipoDocumento", "DocumentTipoID", "DocumentTipo", TABS_EMPLAZAMIENTO.TAB_DOCUMENTOS, "DocumentTipoID", "DocumentTipoID", VISTAS_EMPLAZAMIENTOS.DOCUMENTOS)}
    };

    #endregion

    #region Documentos
    public static class ObjetosTipos
    {
        public const string Emplazamiento = "Emplazamiento";
        public const string InventarioElemento = "InventarioElemento";
        public const string Usuario = "Usuario";
    }
    #endregion

    #endregion

    #region Menu
    public static string ModuloID = "ModuloID";
    public static string ACTIVO = "Activo";
    public static string NUEVO = "Nuevo";
    public static string ACTUALIZADO = "Actualizado";
    public static string MenuModulo = "MenuModulo";
    public static string MenuID = "MenuID";
    public static string Nombre = "Nombre";
    public static string EXPANDIDO = "Expandido";
    public static string ICONO = "Icono";
    public static string ALIAS = "Alias";
    public static string NOMBRE_MODULO = "NombreModulo";
    public static string PAGINA = "Pagina";
    public static string PARAMETROS = "Parametros";
    public static string RutaPagina = "RutaPagina";
    public static string RutaPaginaPadre = "RutaPaginaPadre";
    public static string PaginasComunes = "/PaginasComunes/";
    public static string NIVEL_SELECCIONADO = "NivelSeleccionado";
    public static string ROOT_VACIO = "rootVacio";
    public static string ROOT = "root";
    public static string TRUE = "true";
    public static string FALSE = "false";
    public static string MENU_NIVEL_MAXIMO = "MENU_NIVEL_MAXIMO";
    public static string RUTA_DIRECTORIO_ICONOS = "RUTA_DIRECTORIO_ICONOS";
    public static string RUTA_DIRECTORIO_ICONOS_INVENTARIO = "RUTA_DIRECTORIO_ICONOS_INVENTARIO";
    public static string RUTA_LOGO_CLIENTE_DEFECTO = "RUTA_LOGO_CLIENTE_DEFECTO";
    #endregion

    #region CLASES ICONOS
    public class ICONS
    {
        public static string X_RED = "ico-x-16-red";
        public static string CHECKED = "ico-checked-16-gr";
        public static string VISIBLE = "ico-visible-16";
        public static string EDIT = "ico-edit-16";
        public static string DOWNLOAD = "ico-download-16";
    }
    #endregion

    #region INVENTARIOS
    public static string NOMBRE = "Nombre";
    public static string CODIGO = "Codigo";
    public static string CATEGORIA = "Categoria";
    public static string ESTADO = "Estado";
    public static string FECHACREACION = "FechaCreacion";
    public static string RESTRICCION_DEFECTO_INVENTARIO = "RESTRICCION_DEFECTO_INVENTARIO";
    public static string RESTRICCION_DEFECTO_EMPLAZAMIENTO = "RESTRICCION_DEFECTO_EMPLAZAMIENTO";
    #endregion

    #region SERVICIOS

    public const string SERVICIO_IMPORT_EXPORT = "IMPORT_EXPORT";
    public const string SERVICIO_DATA_QUALITY = "DATA_QUALITY";
    public const string SERVICIO_CARGA_DISTRIBUCION_REGIONAL = "DISTRIBUCION_REGIONAL";
    //Sirve para la gestión de los servicios
    public const int CODIGO_SERVICIO_IMPORT_EXPORT = 601;
    public const int CODIGO_SERVICIO_DATA_QUALITY = 602;
    public const int CODIGO_SERVICIO_CARGA_DISTRIBUCION_REGIONAL = 603;
    #endregion

    #region DOCUMENTOS
    public class Documentos
    {
        public class Permisos
        {
            public const string LECTURA = "Lectura";
            public const string ESCRITURA = "Escritura";
            public const string DESCARGA = "Descarga";
        }

        public class Extensiones
        {
            public const string PDF = "pdf";
            public const string DOC = "doc";
            public const string DOCX = "docx";
            public const string PPT = "ppt";
            public const string XLS = "xls";
            public const string XSLX = "xslx";
        }

        public class classCSS
        {
            public const string PDF = "DocPDF";
            public const string WORD = "DocWord";
            public const string POWERPOINT = "DocPowerPoint";
            public const string EXCEL = "DocExcel";
            public const string OTHER = "DocOther";
        }

        public class IconFile
        {
            public const string PDF = "ico-pdf.svg";
            public const string WORD = "ico-word.svg";
            public const string POWERPOINT = "ico-powerpoint.svg";
            public const string EXCEL = "ico-excel.svg";
            public const string OTHER = "ico-otherdocument.svg";
        }
    }
    #endregion

    #region PARAMETROS
    public class Parametros
    {
        public const string EXTENSIONES_VISOR = "EXTENSIONES_VISOR";
        public const string TAMANO_MAXIMO_DESCARGA_DOCUMENTOS = "TAMANO_MAXIMO_DESCARGA_DOCUMENTOS";
    }
    #endregion

    #region Compartir url
    public class Shared
    {
        public const string tipoDoc = "tipoDoc";
        public const string slug = "slug";
        public const string action = "action";

        public class Action
        {
            public const string download = "download";
            public const string show = "show";
        }
    }
    #endregion

    public class SQL
    {
        public const string AND = "AND";
        public const string OR = "OR";
        public const string INTERSECT = "INTERSECT";
        public const string UNION = "UNION";
    }

    /// <summary>
    /// Título a usar en las páginas donde sea necesario
    /// </summary>
    public const string TituloPaginas = "TREE";

    /// <summary>
    /// CONSTANTES LOGIN
    /// </summary>
    public static int IntentoSeguridad = 0;
    public const string DOBLE_VALIDACION = "DOBLE_VALIDACION";
    public const string ACCESO_MULTIHOMING = "ACCESO MULTIHOMING";
    public const string LDAP_CREAR_USUARIO = "LDAP_CREAR_USUARIO";


    public static string _emailServiceUser = "treeservices@atrebo.com";
    public static string TREE_SERVICES_USER
    {
        get
        {
            UsuariosController cUsuarios = new UsuariosController();
            ClientesController cClientes = new ClientesController();
            long? cliID = cClientes.GetSingleClientID();

            if (cUsuarios.checkSystemUser("Service", "User", _emailServiceUser, cliID))
            {
                return _emailServiceUser;
            }
            else
            {
                return "";
            }
        }
    }


    public static string _emailSuperUser = "super@atrebo.com";
    public static string TREE_SUPER_USER
    {
        get
        {
            UsuariosController cUsuarios = new UsuariosController();
            if (cUsuarios.checkSystemUser("Super", "User", _emailSuperUser, null))
            {
                return _emailSuperUser;
            }
            else
            {
                return "";
            }
        }
    }


    /// <summary>
    /// Constantes para la exportacion de Excels 
    /// </summary>
    public const string EXCEL = "EXCEL";
    public const string EXCEL_CONTACTOS = "EXCEL_CONTACTOS";
    public const string EXCEL_GLOBAL = "EXCEL_GLOBAL";
    public const string EXCEL_IFRS16 = "EXCEL_IFRS16";
    public const string EXCEL_ANALYTICS = "EXCEL_Analytics";
    public const string EXCEL_SIN_FORMATO = "EXCEL_SIN_FORMATO";
    public const string EXCEL_CDM_BT = "EXCEL_CdM_BT";
    public const string EXCEL_CDM_MT = "EXCEL_CdM_MT";
    public const string EXCEL_CDM_MER = "EXCEL_CdM_MERCADOS";
    public const string EXCEL_CDM_CENA = "EXCEL_CdM_CENA";

    public const string DOCUMENTO_EXTENSION_EXCEL_SIN_P = "xls";
    public const string DOCUMENTO_EXTENSION_EXCEL = ".xls";
    public const string DOCUMENTO_EXTENSION_EXCEL_XLSX = ".xlsx";
    public const string DOCUMENTO_EXTENSION_EXCEL_XLSX_SIN_P = "xlsx";
    public const string DOCUMENTO_EXTENSION_EXCEL_CSV = ".csv";
    public const char EXCEL_SEP_CARACTER = ';';
    public const char EXCEL_SEP_CARACTER_SAFE_REPLACE = ':';
    public static string EXCEL_SEP_HEADER = "\"sep=" + EXCEL_SEP_CARACTER + "\"";

    /// <summary>
    /// Nombre de servicios
    /// </summary>
    ///     
    public static string INTEGRACION_SERVICIO_IDENTIFICACION = "LOGIN";
    public static string INTEGRACION_SERVICIO_COMARCH = "COMARCH";
    public static string INTEGRACION_COMARCH_CREATE_ADDRESS_SUCCESS_MESSAGE = "OK";
    public static string INTEGRACION_COMARCH_CREATE_ADDRESS_ERROR_MESSAGE = "NOK";
    public static string INTEGRACION_SERVICIO_COMARCH_CREATE = "Create";
    public static string INTEGRACION_SERVICIO_COMARCH_MODIFY = "Modify";

    /// <summary>
    /// Codigos Historicos
    /// </summary>
    ///     
    public static string COD_HISTORICO_ESTADO_AGREGAR = "COD_AGREGAR"; //NUEVO
    public static string COD_HISTORICO_ESTADO_AGREGAR_FUNCIONALIDAD = "COD_AGREGAR_FUNCIONALIDAD"; //AGREGAR FUNCIONALIDAD
    public static string COD_HISTORICO_ESTADO_ELIMINAR_FUNCIONALIDAD = "COD_ELIMINADA_FUNCIONALIDAD"; //ELIMINAR FUNCIONALIDAD
    public static string COD_HISTORICO_ESTADO_AGREGAR_DOCUMENTO = "COD_AGREGAR_DOCUMENTO"; //AGREGAR DOCUMENTO
    public static string COD_HISTORICO_ESTADO_ELIMINAR_DOCUMENTO = "COD_ELIMINADO_DOCUMENTO"; //ElIMINAR DOCUMENTO
    public static string COD_HISTORICO_ESTADO_AGREGAR_VINCULO = "COD_AGREGAR_VINCULO"; //NUEVO VINCULO
    public static string COD_HISTORICO_ESTADO_ELIMINAR_VINCULO = "COD_ELIMINADO_VINCULO"; //ELIMINAR VINCULO
    public static string COD_HISTORICO_ESTADO_ESTADO_SIGUIENTE_DEFECTO = "COD_ESTADO_SIGUIENTE_DEFECTO"; //NUEVO ESTADO SIGUIENTE POR DEFECTO
    public static string COD_HISTORICO_ESTADO_MODIFICAR = "COD_MODIFICADO"; //MODIFICADO
    public static string COD_HISTORICO_ESTADO_DEFECTO = "COD_ESTADO_DEFECTO"; //ESTADO POR DEFECTO
    public static string COD_HISTORICO_ESTADO_ELIMINAR = "COD_ELIMINADO"; //ELIMINADO
    public static string COD_HISTORICO_ESTADO_EDITAR = "COD_EDITADO"; //EDITADO
    public static string COD_HISTORICO_ESTADO_FORMULARIO_EDITABLE = "COD_FORMULARIO_EDITABLE"; // CAMBIO EN EL BIT FORMULARIO EDITABLE
    public static string COD_HISTORICO_ESTADO_AGREGAR_ESTADO_SIGUIENTE = "COD_AGREGAR_ESTADO_SIGUIENTE"; //AGREGAR ESTADO SIGUIENTE
    public static string COD_HISTORICO_ESTADO_ELIMINAR_SUBESTADO = "COD_ELIMINADO_SUBESTADO"; //ELIMINAR SUBESTADO 
    public static string COD_HISTORICO_ESTADO_ELIMINAR_ESTADO_SIGUIENTE = "COD_ELIMINADO_ESTADO_SIGUIENTE"; // ELIMINAR ESTADO SIGUIENTE

    /// </summary>
    /// Nombre de los modulos
    /// </summary>
    /// 

    ///</ summary>
    ///Integraciones
    ///</ summary>
    ///
    public static string INTEGRACION_SERVICIO_ERP = "SAP";
    public const string INTEGRACION_SERVICIO_API = "API";

    #region TBO 
    public static class TBO
    {

        #region METODOS
        public static class Metodo
        {

            #region FINANCIAL

            #region INVOICES
            public const string TBO_METODO_INVOICES_CI_CREATE = "_CI_Create";
            public const string TBO_METODO_INVOICES_CI_UPDATE = "_CI_Update";
            public const string TBO_METODO_INVOICES_CI_ENABLE = "_CI_Enable";
            public const string TBO_METODO_INVOICES_CI_DISABLE = "_CI_Disable";
            public const string TBO_METODO_INVOICES_CI_FIND_BY_PRIMARY_KEY = "_CI_FindByPrimaryKey";
            public const string TBO_METODO_INVOICES_CI_FIND_BY_PRIMARY_CODE = "_CI_FindByPrimaryCode";
            public const string TBO_METODO_INVOICES_CI_FIND_ALL = "_CI_FindAll";
            public const string TBO_METODO_INVOICES_CI_REMOVE = "_CI_Remove";
            public const string TBO_METODO_INVOICES_SI_CREATE_OBJECT = "_SI_CreateObject";
            public const string TBO_METODO_INVOICES_SI_FIND_PENDING_INVOICES_BY_DATE = "_SI_FindPendingInvoicesByDate";
            public const string TBO_METODO_INVOICES_SI_FIND_PAID_INVOICES_BY_DATE = "_SI_FindPaidInvoicesByDate";
            public const string TBO_METODO_INVOICES_SI_CANCEL_INVOICE = "_SI_Cancellnvoice";
            public const string TBO_METODO_INVOICES_SI_PAY_INVOICE = "_SI_PayInvoice";
            public const string TBO_METODO_INVOICES_SI_GET_ACCOUNTING_ACCOUNT = "_SI_GetAccountingAccount";
            #endregion

            #region INVOICES_RECEIVED
            public const string TBO_METODO_INVOICES_RECEIVED_CI_CREATE = "_CI_Create";
            public const string TBO_METODO_INVOICES_RECEIVED_CI_UPDATE = "_CI_Update";
            public const string TBO_METODO_INVOICES_RECEIVED_CI_ENABLE = "_CI_Enable";
            public const string TBO_METODO_INVOICES_RECEIVED_CI_DISABLE = "_CI_Disable";
            public const string TBO_METODO_INVOICES_RECEIVED_CI_FIND_BY_PRIMARY_KEY = "_CI_FindByPrimaryKey";
            public const string TBO_METODO_INVOICES_RECEIVED_CI_FIND_BY_PRIMARY_CODE = "_CI_FindByPrimaryCode";
            public const string TBO_METODO_INVOICES_RECEIVED_CI_FIND_ALL = "_CI_FindAll";
            public const string TBO_METODO_INVOICES_RECEIVED_CI_REMOVE = "_CI_Remove";
            #endregion

            #endregion

            #region GLOBAL

            #region LANDLORDS
            public const string TBO_METODO_LANDLORDS_CI_CREATE = "_CI_Create";
            public const string TBO_METODO_LANDLORDS_CI_UPDATE = "_CI_Update";
            public const string TBO_METODO_LANDLORDS_CI_ENABLE = "_CI_Enable";
            public const string TBO_METODO_LANDLORDS_CI_DISABLE = "_CI_Disable";
            public const string TBO_METODO_LANDLORDS_CI_FIND_BY_PRIMARY_KEY = "_CI_FindByPrimaryKey";
            public const string TBO_METODO_LANDLORDS_CI_FIND_BY_PRIMARY_CODE = "_CI_FindByPrimaryCode";
            public const string TBO_METODO_LANDLORDS_CI_FIND_ALL = "_CI_FindAll";
            public const string TBO_METODO_LANDLORDS_CI_REMOVE = "_CI_Remove";
            #endregion

            #region SITE_ACQUISITION
            public const string TBO_METODO_SITE_ACQUISITION_CI_CREATE = "_CI_Create";
            public const string TBO_METODO_SITE_ACQUISITION_CI_UPDATE = "_CI_Update";
            public const string TBO_METODO_SITE_ACQUISITION_CI_ENABLE = "_CI_Enable";
            public const string TBO_METODO_SITE_ACQUISITION_CI_DISABLE = "_CI_Disable";
            public const string TBO_METODO_SITE_ACQUISITION_CI_FIND_BY_PRIMARY_KEY = "_CI_FindByPrimaryKey";
            public const string TBO_METODO_SITE_ACQUISITION_CI_FIND_BY_PRIMARY_CODE = "_CI_FindByPrimaryCode";
            public const string TBO_METODO_SITE_ACQUISITION_CI_FIND_ALL = "_CI_FindAll";
            public const string TBO_METODO_SITE_ACQUISITION_CI_REMOVE = "_CI_Remove";
            public const string TBO_METODO_SITE_ACQUISITION_SI_ASSIGN_CONTRACTOR_TO_SARF = "_SI_AssignContractorToSARF";
            public const string TBO_METODO_SITE_ACQUISITION_SI_CHANGE_STATUS = "_SI_ChangeStatus";
            public const string TBO_METODO_SITE_ACQUISITION_SI_ADD_CANDIDATE = "_SI_AddCandidate";
            public const string TBO_METODO_SITE_ACQUISITION_SI_ADD_COMMENT_WITH_ATTACHMENT = "_SI_AddCommentWithAttachment";
            public const string TBO_METODO_SITE_ACQUISITION_SI_ADD_COMMENT = "_SI_AddComment";
            public const string TBO_METODO_SITE_ACQUISITION_SI_GET_STATUS = "_SI_GetStatus";
            #endregion

            #endregion

            #region INSTALL

            #region INSTALL_CIVIL_WORK
            public const string TBO_METODO_INSTALL_CIVIL_WORK_CI_CREATE = "_CI_Create";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_CI_UPDATE = "_CI_Update";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_CI_ENABLE = "_CI_Enable";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_CI_DISABLE = "_CI_Disable";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_CI_FIND_BY_PRIMARY_KEY = "_CI_FindByPrimaryKey";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_CI_FIND_BY_PRIMARY_CODE = "_CI_FindByPrimaryCode";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_CI_FIND_ALL = "_CI_FindAll";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_CI_REMOVE = "_CI_Remove";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_SI_CREATE_BUDGET = "_SI_CreateBudget";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_SI_VALIDATE_BUDGET = "_SI_ValidateBudget";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_SI_CHANGE_STATUS = "_SI_ChangeStatus";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_SI_GET_STATUS = "_SI_GetStatus";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_SI_ADD_COMMENT = "_SI_AddComment";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_SI_ADD_COMMENT_WITH_ATTACHMENT = "_SI_AddCommentWithAttachment";
            public const string TBO_METODO_INSTALL_CIVIL_WORK_SI_ASSIGN_CONTRACTOR = "_SI_AssignContractor";
            #endregion

            #region INSTALL_TECNICAL
            public const string TBO_METODO_INSTALL_TECNICAL_CI_CREATE = "_CI_Create";
            public const string TBO_METODO_INSTALL_TECNICAL_CI_UPDATE = "_CI_Update";
            public const string TBO_METODO_INSTALL_TECNICAL_CI_ENABLE = "_CI_Enable";
            public const string TBO_METODO_INSTALL_TECNICAL_CI_DISABLE = "_CI_Disable";
            public const string TBO_METODO_INSTALL_TECNICAL_CI_FIND_BY_PRIMARY_KEY = "_CI_FindByPrimaryKey";
            public const string TBO_METODO_INSTALL_TECNICAL_CI_FIND_BY_PRIMARY_CODE = "_CI_FindByPrimaryCode";
            public const string TBO_METODO_INSTALL_TECNICAL_CI_FIND_ALL = "_CI_FindAll";
            public const string TBO_METODO_INSTALL_TECNICAL_CI_REMOVE = "_CI_Remove";
            public const string TBO_METODO_INSTALL_TECNICAL_SI_CREATE_BUDGET = "_SI_CreateBudget";
            public const string TBO_METODO_INSTALL_TECNICAL_SI_ADD_COMMENT = "_SI_AddComment";
            public const string TBO_METODO_INSTALL_TECNICAL_SI_ASSIGN_CONTRACTOR = "_SI_AssignContractor";
            public const string TBO_METODO_INSTALL_TECNICAL_SI_CHANGE_STATUS = "_SI_ChangeStatus";
            #endregion

            #endregion

            #region INVENTORY

            #region INVENTORY ELEMENTS
            public const string TBO_METODO_INVENTORY_ELEMENTS_CI_CREATE = "_CI_Create";
            public const string TBO_METODO_INVENTORY_ELEMENTS_CI_UPDATE = "_CI_Update";
            public const string TBO_METODO_INVENTORY_ELEMENTS_CI_ENABLE = "_CI_Enable";
            public const string TBO_METODO_INVENTORY_ELEMENTS_CI_DISABLE = "_CI_Disable";
            public const string TBO_METODO_INVENTORY_ELEMENTS_CI_FIND_BY_PRIMARY_KEY = "_CI_FindByPrimaryKey";
            public const string TBO_METODO_INVENTORY_ELEMENTS_CI_FIND_BY_PRIMARY_CODE = "_CI_FindByPrimaryCode";
            public const string TBO_METODO_INVENTORY_ELEMENTS_CI_FIND_ALL = "_CI_FindAll";
            public const string TBO_METODO_INVENTORY_ELEMENTS_CI_REMOVE = "_CI_Remove";
            public const string TBO_METODO_INVENTORY_ELEMENTS_SI_CREATE = "_SI_Create";
            public const string TBO_METODO_INVENTORY_ELEMENTS_SI_GET_STATUS = "_SI_GetStatus";
            public const string TBO_METODO_INVENTORY_ELEMENTS_SI_IsSolved = "_SI_IsSolved";
            public const string TBO_METODO_INVENTORY_ELEMENTS_SI_CANCEL = "_SI_Cancel";
            public const string TBO_METODO_INVENTORY_ELEMENTS_SI_BLOCK = "_SI_Block";
            public const string TBO_METODO_INVENTORY_ELEMENTS_SI_ACCEPT = "_SI_Accept";



            #endregion

            #endregion

            #region LEGALIZATIONS

            #region LICENSES
            public const string TBO_METODO_LICENSES_CI_CREATE = "_CI_Create";
            public const string TBO_METODO_LICENSES_CI_UPDATE = "_CI_Update";
            public const string TBO_METODO_LICENSES_CI_ENABLE = "_CI_Enable";
            public const string TBO_METODO_LICENSES_CI_DISABLE = "_CI_Disable";
            public const string TBO_METODO_LICENSES_CI_FIND_BY_PRIMARY_KEY = "_CI_FindByPrimaryKey";
            public const string TBO_METODO_LICENSES_CI_FIND_BY_PRIMARY_CODE = "_CI_FindByPrimaryCode";
            public const string TBO_METODO_LICENSES_CI_FIND_ALL = "_CI_FindAll";
            public const string TBO_METODO_LICENSES_CI_REMOVE = "_CI_Remove";
            public const string TBO_METODO_LICENSES_SI_ADD_COMMENT = "_SI_AddComment";
            public const string TBO_METODO_LICENSES_SI_GET_STATUS = "_SI_GetStatus";
            public const string TBO_METODO_LICENSES_SI_FIND_ALL_BY_CONTRACTOR = "_SI_FindAllByContrator";
            #endregion

            #endregion

            #region MAINTENANCE

            #region CORRECTIVE_MAINTENANCE
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_CI_CREATE = "_CI_Create";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_CI_UPDATE = "_CI_Update";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_CI_ENABLE = "_CI_Enable";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_CI_DISABLE = "_CI_Disable";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_CI_FIND_BY_PRIMARY_KEY = "_CI_FindByPrimaryKey";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_CI_FIND_BY_PRIMARY_CODE = "_CI_FindByPrimaryCode";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_CI_FIND_ALL = "_CI_FindAll";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_CI_REMOVE = "_CI_Remove";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_CHANGE_STATUS = "_SI_ChangeStatus";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_ASSIGN_CONTRACTOR = "_SI_AssignContractor";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_ADD_COMMENT = "_SI_AddComment";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_CREATE_BUDGET = "_SI_CreateBudget";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_VALIDATE_BUDGET = "_SI_ValidateBudget";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_APPROVE_BUDGET = "_SI_ApproveBudget";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_GET_STATUS = "_SI_GetStatus";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_GET_SCHEDULED_DATE = "_SI_GetScheduledDate";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_IS_SOLVED = "_SI_IsSolved";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_CANCEL = "_SI_Cancel";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_BLOCK = "_SI_Block";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_GET_PENDING_TASKS = "_SI_GetPendingTasks";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_GET_PENDING_TASKS_BY_SITE = "_SI_GetPendingTasksBySite";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_GET_ACUMULATIVE_TIME = "_SI_GetAcumulativeTime";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_FIND_ALL_BY_CONTRACTOR = "_SI_FindAllByContractor";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_GET_TIME_TO_LIMIT = "_SI_GetTimeToLimit";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_GET_OUT_DATED_TASKS = "_SI_GetOutDatedTasks";
            public const string TBO_METODO_CORRECTIVE_MAINTENANCE_SI_GET_NEXT_TO_OUT_DATED_TASKS = "_SI_GetNextToOutDatedTasks";
            #endregion

            #region MAINTENANCE_INCIDENTS
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_CI_CREATE = "_CI_Create";
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_CI_UPDATE = "_CI_Update";
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_CI_ENABLE = "_CI_Enable";
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_CI_DISABLE = "_CI_Disable";
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_CI_FIND_BY_PRIMARY_KEY = "_CI_FindByPrimaryKey";
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_CI_FIND_BY_PRIMARY_CODE = "_CI_FindByPrimaryCode";
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_CI_FIND_ALL = "_CI_FindAll";
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_CI_REMOVE = "_CI_Remove";
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_SI_CREATE = "_SI_Create";
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_SI_GET_STATUS = "_SI_GetStatus";
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_SI_IsSolved = "_SI_IsSolved";
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_SI_CANCEL = "_SI_Cancel";
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_SI_BLOCK = "_SI_Block";
            public const string TBO_METODO_MAINTENANCE_INCIDENTS_SI_ACCEPT = "_SI_Accept";
            #endregion

            #endregion

            #region SPACEREQUEST

            #region SPACE_REQUEST
            public const string TBO_METODO_SPACE_REQUEST_CI_CREATE = "_CI_Create";
            public const string TBO_METODO_SPACE_REQUEST_CI_UPDATE = "_CI_Update";
            public const string TBO_METODO_SPACE_REQUEST_CI_ENABLE = "_CI_Enable";
            public const string TBO_METODO_SPACE_REQUEST_CI_DISABLE = "_CI_Disable";
            public const string TBO_METODO_SPACE_REQUEST_CI_FIND_BY_PRIMARY_KEY = "_CI_FindByPrimaryKey";
            public const string TBO_METODO_SPACE_REQUEST_CI_FIND_BY_PRIMARY_CODE = "_CI_FindByPrimaryCode";
            public const string TBO_METODO_SPACE_REQUEST_CI_FIND_ALL = "_CI_FindAll";
            public const string TBO_METODO_SPACE_REQUEST_CI_REMOVE = "_CI_Remove";
            public const string TBO_METODO_SPACE_REQUEST_SI_SEND_DOCUMENTATION = "_SI_SendDocumentation";
            public const string TBO_METODO_SPACE_REQUEST_SI_ACCEPT_PROPOSAL = "_SI_AcceptProposal";
            public const string TBO_METODO_SPACE_REQUEST_SI_REQUEST_PROPOSAL = "_SI_RequestProposal";
            public const string TBO_METODO_SPACE_REQUEST_SI_SIGN_PROPOSAL = "_SI_SignProposal";
            public const string TBO_METODO_SPACE_REQUEST_SI_GET_STATUS = "_SI_GetStatus";
            public const string TBO_METODO_SPACE_REQUEST_SI_ADD_COMMENT = "_SI_AddComment";
            public const string TBO_METODO_SPACE_REQUEST_SI_CHANGE_STATUS = "_SI_ChangeStatus";
            #endregion

            #endregion

            #region SELF SERVICE

            #region TOWER_CUSTOMER
            public const string TBO_METODO_TOWER_CUSTOMER_CI_CREATE = "_CI_Create";
            public const string TBO_METODO_TOWER_CUSTOMER_CI_UPDATE = "_CI_Update";
            public const string TBO_METODO_TOWER_CUSTOMER_CI_ENABLE = "_CI_Enable";
            public const string TBO_METODO_TOWER_CUSTOMER_CI_DISABLE = "_CI_Disable";
            public const string TBO_METODO_TOWER_CUSTOMER_CI_FIND_BY_PRIMARY_KEY = "_CI_FindByPrimaryKey";
            public const string TBO_METODO_TOWER_CUSTOMER_CI_FIND_BY_PRIMARY_CODE = "_CI_FindByPrimaryCode";
            public const string TBO_METODO_TOWER_CUSTOMER_CI_FIND_ALL = "_CI_FindAll";
            public const string TBO_METODO_TOWER_CUSTOMER_CI_REMOVE = "_CI_Remove";
            public const string TBO_METODO_TOWER_CUSTOMER_SI_CREATE_SEND = "_SI_CreateSend";
            public const string TBO_METODO_TOWER_CUSTOMER_SI_SEND = "_SI_Send";
            public const string TBO_METODO_TOWER_CUSTOMER_SI_FIND_ALL_SITE_BY_CUSTOMER = "_SI_FindAllSiteByCustomer";
            public const string TBO_METODO_TOWER_CUSTOMER_SI_FIND_ALL_SITE_REQUEST_BY_CUSTOMER = "_SI_FindAllSiteRequestByCustomer";
            public const string TBO_METODO_TOWER_CUSTOMER_SI_FIND_ALL_INVENTORY_BY_SITE = "_SI_FindAllInventoryBySite";
            public const string TBO_METODO_TOWER_CUSTOMER_SI_FIND_ALL_INVENTORY_BY_REQUEST = "_SI_FindAllInventoryByRequest";
            #endregion

            #endregion

        }
        #endregion

        #region TAG RECURSOS 
        public static class TAG_RECURSO
        {

            public const string NotaEstadoCancelado = "NotaEstadoCancelado";
            public const string NotaEstadoBloqueado = "NotaEstadoBloqueado";
            public const string strTreeAPI = "strTreeAPI";
            public const string strDescripcionAPI = "strDescripcionAPI";
        }
        #endregion

        #region LONGITUD CAMPOS
        public static class LENGTH_CAMPOS
        {
            public static class GENERAL
            {
                public static int emailUsuario = 200;
                public static int codigoEmplazamiento = 50;
                public static int proyecto = 400;
                public static int moneda = 50;
                public static int operador = 150;
                public static int estadoGlobal = 150;
                public static int emplazamientosCategorias = 50;
                public static int tipoEmplazamiento = 50;
                public static int tipoEstructura = 50;
                public static int tipoEdificio = 50;
                public static int pais = 500;
                public static int region = 50;
                public static int regionPais = 500;
                public static int municipio = 500;
                public static int provincia = 500;
            }

            public static class MAINTENANCE
            {
                public static int nombreIncidencia = 50;
                public static int descripcionIncidencia = 500;
                public static int mantenimientoIncidenciasTipo = 100;
                public static int agencia = 100;
                public static int criticidad = 100;
                public static int tipologia = 50;
                public static int conflictividad = 50;
            }

            public static class SITE
            {
                public static int codigo = 50;
                public static int nombreSitio = 500;
                public static int barrio = 150;
                public static int direccion = int.MaxValue;
                public static int codigoPostal = 50;
                public static int tamano = 150;
            }

            public static class INVENTORY
            {
                public static int numeroInventario = 100;
                public static int nombreInventario = 100;
            }
        }
        #endregion

        public const int TBO_LIST_MAXIMUM_SIZE = 50;
        public const int MAX_DAYS_VALID_LAST_MODIFIED = 31;
    }
    #endregion


    public const string FORMATO_FECHA = "dd/MM/yyyy";
    public const string Form_fecha = "yyyyMMdd";

    public static string DateTimeToString(DateTime date)
    {
        string f = date.Day + "/" + ((date.Month < 10) ? "0" : "") + date.Month + "/" + date.Year;

        return f;
    }

    public static string DateTimeToStringConHora(DateTime date)
    {
        string hora = ((date.Hour < 10) ? "0" : "") + date.Hour + ":" + ((date.Minute < 10) ? "0" : "") + date.Minute + ":" + ((date.Second < 10) ? "0" : "") + date.Second;
        string f = date.Day + "/" + ((date.Month < 10) ? "0" : "") + date.Month + "/" + date.Year + " " + hora;

        return f;
    }

    /// <summary>
    /// Constantes de componentes
    /// </summary>
    /// 
    public const long COMPONENTE_CAMPO_TEXTO = 1;
    public const long COMPONENTE_CAMPO_DESPLEGABLE = 2;
    public const long COMPONENTE_CAMPO_CHECK = 3;
    public const long COMPONENTE_CAMPO_RADIO = 4;

    public enum RestriccionesAtributoDefecto
    {
        ACTIVE = 1,
        DISABLED = 2,
        HIDDEN = 3
    }

    public enum Modulos
    {
        GLOBAL = 45,
        MANTENIMIENTO = 29,
        ACCESS = 43,
        ADQUISICIONES = 27,
        ADQUISICIONES_SARF = 69,
        AMBIENTAL = 85,
        AMPLIACIONES = 97,
        ASSETS_PURCHASE = 99,
        AUDIT = 42,
        CALIDAD = 81,
        FINANCIERO = 59,
        CITY = 34,
        DESPLIEGUE = 88,
        DOCUMENTAL = 41,
        ENERGY = 36,
        FIRMA_DIGITAL = 100,
        INDOOR = 73,
        INSTALL_OBRA_CIVIL = 52,
        INSTALL_TECNICA = 51,
        INVENTARIO = 78,
        LEGAL = 26,
        MONITORING = 79,
        PLANNING = 80,
        SAVING = 24,
        SHARING = 30,
        SPACE = 57,
        SSRR = 92,
        SWAPPING = 61,
        TOWER_SARF = 65,
        TOWER = 62,
        UNINSTALL_ADMIN = 37,
        UNINSTALL_TECNICA = 40,
        UNINSTALL_ELECTRICA = 53,
        VANDALISMO = 95,
        EXPORTAR_IMPORTAR = 104
    }

    /// <summary>
    /// Funcionalidades definidas por modulos
    /// </summary>
    /// 
        
    public enum ModFun
    {
        #region ACCESO MODULOS

        ACCESO_A_ENERGY_MODULO = 1704,
        ACCESO_A_INDOOR_MODULO = 1706,
        ACCESO_A_SAVING_MODULO = 1710,
        ACCESO_A_SHARING_MODULO = 1711,
        ACCESO_A_DESPLIEGUE_MODULO = 1725,
        ACCESO_A_SSRR_MODULO = 1728,
        ACCESO_A_LEGAL_MODULO = 1708,
        ACCESO_A_PLANNING_MODULO = 1720,
        ACCESO_A_MONITORIZACION_MODULO = 1717,
        ACCESO_A_INVENTARIO_MODULO = 1716,
        ACCESO_A_ACCESS_MODULO = 1700,
        ACCESO_A_ADQUISICIONES_MODULO = 1157,
        ACCESO_A_INSTALL_MODULO = 1707,
        ACCESO_A_UNINSTALL_MODULO = 1715,
        ACCESO_A_DOCUMENTAL_MODULO = 1703,
        ACCESO_A_SPACE_MODULO = 1712,
        ACCESO_A_AMBIENTAL_MODULO = 1721,
        ACCESO_A_AMPLIACIONES_MODULO = 1731,
        ACCESO_A_COMPRA_ACTIVOS_MODULO = 1732,
        ACCESO_A_AUDIT_MODULO = 1701,
        ACCESO_A_BILLING_MODULO = 1702,
        ACCESO_A_CALIDAD_MODULO = 1722,
        ACCESO_A_CITY_MODULO = 1729,
        ACCESO_A_GIS_MODULO = 1724,
        ACCESO_A_PROPERTY_INCIDENCIAS_MODULO = 1705,
        ACCESO_A_MANTENIMIENTO_MODULO = 1709,
        ACCESO_A_REPORTING_MODULO = 1726,
        ACCESO_A_SWAPPING_MODULO = 1713,
        ACCESO_A_TOWER_MODULO = 1714,

        #endregion

        #region GLOBAL

        ACCESO_TOTAL_EMPLAZAMIENTOS = 16,
        ACCESO_SOLO_LECTURA_A_EMPLAZAMIENTOS = 2037,

        ACCESO_TOTAL_LOCALIZACIONES = 2,
        ACCESO_TOTAL_CONFLICTIVIDAD = 3,
        ACCESO_TOTAL_MODULOS = 5,
        ACCESO_TOTAL_PARAMETROS = 7,
        ACCESO_TOTAL_CLIENTES = 10,
        ACCESO_TOTAL_ESTADOSGLOBALES = 11,
        ACCESO_TOTAL_PAISES = 12,
        ACCESO_TOTAL_LICENCIAS = 21,
        ACCESO_CLIENTE_MONEDAS = 22,
        ACCESO_TOTAL_MONEDAS = 23,
        ACCESO_CLIENTE_INFLACIONES = 29,
        ACCESO_TOTAL_INFLACIONES = 30,
        ACCESO_TOTAL_INFRAESTRUCTURAS = 32,
        ACCESO_TOTAL_COLUMNAS = 34,
        ACCESO_TOTAL_OPERADORES = 36,
        ACCESO_TOTAL_GLOBAL_ACTA_TECNICAS_FABRICANTES_MODELOS = 41,
        ACCESO_RESTRINGIDO_GLOBAL_ACTA_TECNICAS_FABRICANTES_MODELOS = 42,
        ACCESO_TOTAL_GLOBAL_TIPOS_TECNOLOGIAS_TELCO = 45,
        ACCESO_RESTRINGIDO_GLOBAL_TIPOS_TECNOLOGIAS_TELCO = 47,
        ACCESO_TOTAL_GLOBAL_FRECUENCIAS_TELCO = 46,
        ACCESO_RESTRINGIDO_GLOBAL_FRECUENCIAS_TELCO = 48,
        ACCESO_TOTAL_COMPONENTES = 386,
        ACCESO_TOTAL_TIPOS_DATOS = 388,
        ACCESO_TOTAL_IMPUESTOS = 2000,
        GLO_Impuestos_Lectura = 500036,
        ACCESO_TOTAL_EMPLAZAMIENTOS_TIPOS = 2002,
        ACCESO_TOTAL_EMPLAZAMIENTOS_CATEGORIAS_SITIOS = 2004,
        ACCESO_TOTAL_CALIDAD = 2005,
        ACCESO_TOTAL_PROVEEDORES = 2007,
        ACCESO_TOTAL_A_PROYECTOS_AGRUPACIONES = 2012,
        ACCESO_TOTAL_ALQUILERES_ESTADOS = 2013,
        ACCESO_TOTAL_ALQUILERES_TIPOS_CONTRATACIONES = 2014,
        ACCESO_TOTAL_ALQUILERES_TIPOS_CONTRATOS = 2015,
        ACCESO_TOTAL_ALQUILERES_TIPOS_PROPIETARIOS = 2016,
        ACCESO_TOTAL_ALQUILERES_TIPOS_CONTRIBUYENTES = 2017,
        ACCESO_TOTAL_EMPLAZAMIENTOS_TAMANOS = 2018,
        ACCESO_TOTAL_CONCEPTOS_PAGOS = 2019,
        ACCESO_TOTAL_NOTIFICACIONES_CADENCIAS = 2038,
        ACCESO_TOTAL_NOTIFICACIONES_CRITERIOS = 2040,
        ACCESO_RESTRINGIDO_NOTIFICACIONES_CRITERIOS = 2041,
        ACCESO_TOTAL_NOTIFICACIONES_GESTION = 2042,
        ACCESO_RESTRINGIDO_NOTIFICACIONES_GESTION = 2043,
        ACCESO_TOTAL_GLOBAL_INFRAESTRUCTURAS = 2047,
        ACCESO_TOTAL_METODOS_PAGOS = 2054,
        ACCESO_TOTAL_ALQUILERES_METODOS_PAGOS = 2057,
        ACCESO_TOTAL_PROVINCIAS_MUNICIPIOS = 2060,
        ACCESO_RESTRINGIDO_PROVINCIAS_MUNICIPIOS = 2062,
        ACCESO_TOTAL_REGIONES = 2061,
        ACCESO_RESTRINGIDO_REGIONES = 2063,
        ACCESO_TOTAL_SERVICIOS = 2070,
        ACCESO_TOTAL_METODOS = 2072,
        ACCESO_TOTAL_CONEXIONES = 2074,
        ACCESO_TOTAL_RIESGOS_EMPLAZAMIENTOS = 2081,
        ACCESO_RESTRINGIDO_RIESGOS_EMPLAZAMIENTOS = 2082,
        ACCESO_TOTAL_DEPARTAMENTOS = 2089,
        ACCESO_TOTAL_ALQUILERES_CLASES_ACTIVOS = 2091,
        ACCESO_TOTAL_SAP_TIPOS_NIF = 2093,
        ACCESO_TOTAL_SAP_TRATAMIENTOS = 2095,
        ACCESO_TOTAL_SAP_CLAVES_CLASIFICACIONES = 2097,
        ACCESO_TOTAL_SAP_CUENTAS_ASOCIADAS = 2099,
        ACCESO_TOTAL_SAP_GRUPOS_CUENTAS = 2101,
        ACCESO_TOTAL_SAP_GRUPO_TESORERIAS = 2103,
        ACCESO_TOTAL_ZONAS = 2105,
        ACCESO_TOTAL_SAP_TRATAMIENTOS_PREVIOS = 2106,
        ACCESO_TOTAL_A_GLOBAL_CENTROS_OPERATIVOS = 2201,
        ACCESO_RESTRINGIDO_A_GLOBAL_CENTROS_OPERATIVOS = 2202,
        ACCESO_TOTAL_A_GLOBAL_AREASOYM = 3134,
        ACCESO_RESTRINGIDO_A_GLOBAL_AREASOYM = 3135,
        ACCESO_GLOBAL_LOCALIDADES_ASIGNAR_AREASOYM = 3136,
        ACCESO_GLOBAL_LOCALIDADES_VISUALIZAR_AREASOYM = 3137,
        ACCESO_TOTAL_INFRAESTRUCTURA_GESTION = 5503,
        ACCESO_TOTAL_ACTIVOS_CLASES = 5555,
        ACCESO_SOLO_LECTURA_A_ACTIVOS_CLASES = 5556,
        ACCESO_TOTAL_ACTIVOS_ESTADOS = 5557,
        ACCESO_TOTAL_GLOBAL_BANCOS = 5562,
        ACCESO_TOTAL_SOCIEDADES = 5565,
        ACCESO_TOTAL_CENTROCOSTES = 5567,
        ACCESO_TOTAL_GLOBAL_PROPIETARIOS = 5569,
        ACCESO_TOTAL_EMPRESASPROVEEDORAS = 5575,
        ACCESO_SOLO_LECTURA_EMPRESASPROVEEDORAS = 5576,
        ACCESO_TOTAL_ALQUILERES_HISTORICAS_ESTADOS = 5577,
        ACCESO_TOTAL_ALQUILERES_CONCEPTOS = 5580,
        ACCESO_ANONIMIZAR = 5583,
        ACCESO_CREAR_PROVEEDOR = 5584,
        ACCESO_TOTAL_ALQUILERES_TIPOS_CONTRATOS_SAP = 6103,
        ACCESO_TOTAL_AREASFUNCIONALES = 6106,
        ACCESO_TOTAL_LIMITES = 6108,
        ACCESO_SOLO_LECTURA_LIMITES = 6109,
        ACCESO_TOTAL_VEX_CRITICIDADES = 6113,
        ACCESO_TOTAL_VEX_PROYECTOS_SECCIONES = 6115,
        ACCESO_SOLO_LECTURA_VEX_PROYECTOS_SECCIONES = 6116,
        ACCESO_TOTAL_CONDICIONESPAGOS = 6121,
        ACCESO_TOTAL_TIPOSSERVICIOS = 6123,
        ACCESO_TOTAL_GLOBAL_INFRAESTRUCTURA_SUBSERVICIOS = 6136,
        ACCESO_TOTAL_PARTESRESCISORAS = 6140,
        ACCESO_TOTAL_GRUPOSAUTORIZACIONES = 6146,
        ACCESO_TOTAL_MOTIVOS_ADENDAS = 6148,
        ACCESO_TOTAL_GLOBAL_LOCALIDADES = 6157,
        ACCESO_TOTAL_TIPOS_DISPOSITIVOS = 6190,
        ACCESO_TOTAL_ESTADOS_DISPOSITIVOS = 6193,
        ACCESO_TOTAL_FABRICANTES_DISPOSITIVOS = 6199,
        ACCESO_TOTAL_UNIDADES_DISPOSITIVOS = 6202,
        ACCESO_TOTAL_CATEGORIAS_DISPOSITIVOS = 6205,
        ACCESO_TOTAL_INFLACIONES_TIPOS = 6244,
        ACCESO_TOTAL_GLOBAL_ZONAS_CLUSTER = 6255,
        ACCESO_TOTAL_EMPLAZAMIENTOS_TIPOS_ESTRUCTURAS = 6314,
        ACCESO_TOTAL_TOKENS_ACCESO = 6320,
        ACCESO_TOTAL_USUARIOS_ACCESO = 6321,
        ACCESO_TOTAL_MULTIUSUARIOS_ACCESO = 6322,
        ACCESO_TOTAL_EMPRESASPROVEEDORAS_PROVEEDORES = 6327,
        ACCESO_TOTAL_GLOBAL_LOCALIDADES_DATOS_ADICIONALES = 6359,
        ACCESO_GLOBAL_ADMINISTRACION_IDIOMAS = 6360,
        ACCESO_TOTAL_REGIONES_RADIO = 6375,
        ACCESO_GESTION_SUBCONTRATAS_GLOBAL = 6404,
        ACCESO_TOTAL_GRUPOS_ACCESOS_WEB = 6421,
        ACCESO_SOLO_LECTURA_GRUPOS_ACCESOS_WEB = 6422,
        ACCESO_TOTAL_DIRECCIONES_TIPOS = 9580,
        ACCESO_TOTAL_DIRECCIONES_TIPOS_CAMINOS = 9581,
        ACCESO_TOTAL_DIRECCIONES_ESTADOS_CAMINOS = 9852,
        ACCESO_TOTAL_DIRECCIONES_TIPOS_LLAVES_ACCESOS = 9853,
        ACCESO_TOTAL_DIRECCIONES_LUGARES_RETIROS_ACCESOS = 9854,
        ACCESO_TOTAL_PROYECTOS_TIPOS = 9990,
        ACCESO_TOTAL_CONFIGURACION_MAXIMO = 22067,
        GLO_ToolIntregracionesAtributosMapping_Lectura = 500037,
        ACCESO_TOTAL_DIRECCIONES_ACCESOS = 22084,
        ACCESO_TOTAL_INTEGRACIONES = 2068,

        ACCESO_TOTAL_USUARIOS = 2,
        ACCESO_SOLO_LECTURA_USUARIOS = 22079,
        GLO_Perfiles_Lectura = 500024,
        ACCESO_TOTAL_PERFILES = 1,
        ACCESO_TOTAL_PROYECTOSTIPOS = 4,
        GLO_ProyectosTipos_Lectura = 500026,
        GLO_Clientes_Lectura = 500030,
        GLO_Parametros_Lectura = 500008,
        GLO_EstadosGlobales_Lectura = 500018,
        GLO_Paises_Lectura = 500031,
        GLO_Proyectos_Lectura = 500025,
        ACCESO_TOTAL_PROYECTOS = 14,
        ACCESO_CLIENTE_LICENCIAS = 20,
        ACCESO_RESTRINGIDO_A_PROYECTOS_AGRUPACIONES = 496,
        GLO_EmplazamientosTipos_Lectura = 500017,
        ACCESO_TOTAL_EMPLAZAMIENTOS_TIPOS_EDIFICIOS = 2003,
        GLO_EmplazamientosTiposEdificios_Lectura = 500016,
        GLO_EmplazamientosCategoriasSitios_Lectura = 500002,
        ACCESO_RESTRINGIDO_CALIDAD = 2006,
        ACCESO_RESTRINGIDO_PROVEEDORES = 2008,
        ACCESO_TOTAL_A_ALQUILERES_ESTADOS = 2013,
        GLO_AlquileresEstados_Lectura = 500013,
        GLO_AlquileresTiposContrataciones_Lectura = 500014,
        GLO_AlquileresTiposContratos_Lectura = 500015,
        GLO_AlquileresTiposPropietarios_Lectura = 500001,
        GLO_TiposContribuyentes_Lectura = 500009,
        GLO_EmplazamientosTamanos_Lectura = 500003,
        GLO_ConceptosPagos_Lectura = 500005,

        GLO_AgrupacionEstados_Lectura = 500106,
        GLO_AgrupacionEstados_Total = 500107,

        ACCESO_RESTRINGIDO_NOTIFICACIONES_CADENCIAS = 2039,
        ACCESO_RESTRINGIDO_METODOS_PAGOS = 2056,
        ACCESO_RESTRINGIDO_ALQUILERES_METODOS_PAGOS = 2059,
        ACCESO_RESTRINGIDO_SERVICIOS = 2071,
        ACCESO_RESTRINGIDO_CONEXIONES = 2075,
        ACCESO_RESTRINGIDO_DEPARTAMENTOS = 2090,
        GLO_VerColumnas_Total = 500027,
        GLO_VerColumnas_Lectura = 500028,
        GLO_VerColumnasAlquileres_Lectura = 500029,
        ACCESO_TOTAL_A_GLOBAL_ALQUILERES_COLUMNAS = 1530,
        ACCESO_SOLO_LECTURA_A_ACTIVOS_ESTADOS = 5558,
        ACCESO_RESTRINGIDO_GLOBAL_BANCOS = 5563,
        ACCESO_SOLO_LECTURA_SOCIEDADES = 5566,
        ACCESO_SOLO_LECTURA_CENTROCOSTES = 5568,
        ACCESO_RESTRINGIDO_GLOBAL_PROPIETARIOS = 5570,
        ACCESO_RESTRINGIDO_ALQUILERES_HISTORICAS_ESTADOS = 5579,
        ACCESO_RESTRINGIDO_ALQUILERES_CONCEPTOS = 6000,
        ACCESO_RESTRINGIDO_ALQUILERES_TIPOS_CONTRATOS_SAP = 6105,
        ACCESO_RESTRINGIDO_ALQUILERES_CLASES_ACTIVOS = 2092,
        ACCESO_RESTRINGIDO_SAP_TIPOS_NIF = 2094,
        ACCESO_RESTRINGIDOS_SAP_TRATAMIENTOS = 2096,
        ACCESO_SOLO_LECTURA_AREASFUNCIONALES = 6107,
        ACCESO_SOLO_LECTURA_VEX_CRITICIDADES = 6114,
        ACCESO_SOLO_LECTURA_CONDICIONESPAGOS = 6122,
        ACCESO_SOLO_LECTURA_SAP_CLAVES_CLASIFICACIONES = 2098,
        ACCESO_SOLO_LECTURA_SAP_CUENTAS_ASOCIADAS = 2100,
        ACCESO_SOLO_LECTURA_SAP_GRUPOS_CUENTAS = 2102,
        ACCESO_SOLO_LECTURA_SAP_GRUPO_TESORERIAS = 2104,
        ACCESO_SOLO_LECTURA_TIPOSSERVICIOS = 6124,
        GLO_Zonas_Lectura = 500011,
        ACCESO_SOLO_LECTURA_SAP_TRATAMIENTOS_PREVIOS = 2107,
        ACCESO_SOLO_LECTURA_PARTESRESCISORAS = 6141,
        ACCESO_RESTRINGIDO_GLOBAL_INFRAESTRUCTURA_SUBSERVICIOS = 6138,
        ACCESO_SOLO_LECTURA_GRUPOSAUTORIZACIONES = 6147,
        ACCESO_SOLO_LECTURA_MOTIVOS_ADENDAS = 6149,
        ACCESO_RESTRINGIDO_TIPOS_DISPOSITIVOS = 6192,
        ACCESO_RESTRINGIDO_ESTADOS_DISPOSITIVOS = 6195,
        ACCESO_RESTRINGIDO_FABRICANTES_DISPOSITIVOS = 6201,
        ACCESO_RESTRINGIDO_CATEGORIAS_DISPOSITIVOS = 6207,
        ACCESO_RESTRINGIDO_UNIDADES_DISPOSITIVOS = 6204,
        ACCESO_TOTAL_FLUJOS = 6218,
        ACCESO_RESTRINGIDO_FLUJOS = 6219,
        ACCESO_SOLO_LECTURA_INFLACIONES_TIPOS = 6243,
        ACCESO_TOTAL_GLOBAL_DIRECCIONES_TIPOS = 6280,
        GLO_GlobalDireccionesTipos_Lectura = 500019,
        GLO_GlobalDireccionesTiposCaminos_Lectura = 500020,
        ACCESO_TOTAL_GLOBAL_TIPOS_CAMINOS = 6281,
        ACCESO_TOTAL_GLOBAL_ESTADOS_CAMINOS = 6282,
        GLO_GlobalDireccionesEstadosCaminos_Lectura = 500021,
        GLO_GlobalDireccionesTiposLlavesAccesos_Lectura = 500022,
        ACCESO_TOTAL_GLOBAL_TIPOS_LLAVES_ACCESOS = 6283,
        ACCESO_TOTAL_GLOBAL_LUGARES_RETIROS_ACCESOS = 6284,
        GLO_GlobalDireccionesLugaresRetirosAccesos_Lectura = 500023,
        ACCESO_RESTRINGIDO_EMPLAZAMIENTOS_TIPOS_ESTRUCTURAS = 6315,
        GLO_Componentes_Lectura = 500004,
        GLO_TiposDatos_Lectura = 500010,
        GLO_Idiomas_Lectura = 500012,
        GLO_LicenciasTipos_Total = 500006,
        GLO_LicenciasTipos_Lectura = 500007,
        GLO_GlobalDireccionesAccesos_Lectura = 500008,
        GLO_GlobalSAPInterfazIndices_Lectura = 500035,
        GLO_GlobalSAPInterfazIndices_Total = 500034,
        GLO_MenuLateral_Total = 500041,
        GLO_MenuLateral_Lectura = 500040,
        GLO_MenusModulos_Total = 500038,
        GLO_MenusModulos_Lectura = 500039,
        GLO_GestionUsuario_Total = 500050,
        GLO_Entidades_Total = 500048,
        GLO_Entidades_Lectura = 500049,
        GLO_Contactos_Total = 500055,
        GLO_Contactos_Lectura = 500056,
        GLO_MapaEmplazamientosCercanos_Total = 500057,
        GLO_MapaEmplazamientosCercanos_Lectura = 500058,
        GLO_GENERADORCODIGOS_LECTURA = 500061,
        GLO_GENERADORCODIGOS_TOTAL = 500062,
        GLO_EntidadesTipos_Total = 500071,
        GLO_EntidadesTipos_Lectura = 500072,

        GLO_ContactosTipos_Total = 500073,
        GLO_ContactosTipos_Lectura = 500074,
        GLO_FuncionalidadesTipos_Total = 500075,
        GLO_FuncionalidadesTipos_Lectura = 500076,
        GLO_Municipalidades_Total = 500077,
        GLO_Municipalidades_Lectura = 500078,


        GLO_DocumentosVista_Total = 500081,
        //GLO_DocumentosVista_Lectura = 500082,
        GLO_EmplazamientosAtributosConfiguracion_Total = 500082,
        GLO_EmplazamientosAtributosConfiguracion_Lectura = 500083,
        GLO_EmplazamientosAtributosCategorias_Total = 500084,
        GLO_EmplazamientosAtributosCategorias_Lectura = 500085,
        GLO_DocumentosVista_Lectura = 500087,
        GLO_Entidades_AsignarImagenOperador = 500086,

        GLO_TablasModeloDatos_Total = 500507,
        GLO_TablasModeloDatos_Lectura = 500506,
        GLO_EmplazamientosHistoricos_Total = 500100,
        GLO_EmplazamientosHistoricos_Lectura = 500101,

        ACCESO_TOTAL_GLOBAL_DQTablasPaginas = 500508,
        ACCESO_RESTRINGIDO_GLOBAL_DQTablasPaginas = 500509,

        GLO_ProyectosEstados_Total = 500515,
        GLO_ProyectosEstados_Lectura = 500514,
        GLO_Proyecto_Total = 500516,
        GLO_Proyecto_Lectura = 500517,


        GLO_Tipologias_Total = 500519,
        GLO_Tipologias_Lectura = 500520,

        GLO_Estados_Total = 500527,
        GLO_Estados_Lectura = 500528,

        GLO_ProductCatalog_Total = 500535,
        GLO_ProductCatalog_Lectura = 500536,

        GLO_ProductCatalogServiciosContenedor_Total = 500539,
        GLO_ProductCatalogServiciosContenedor_Lectura = 500540,
        GLO_ProductCatalogTipos_Lectura = 500537,
        GLO_ProductCatalogTiposLectura_Total = 500538,

        GLO_ProductCatalogTiposServicios_Lectura = 500541,
        GLO_ProductCatalogTiposServicios_Total = 500542,

        GLO_ProductCatalogUnidades_Lectura = 500543,
        GLO_ProductCatalogUnidades_Total = 500544,

        GLO_ProductCatalogFrecuencias_Lectura = 500545,
        GLO_ProductCatalogFrecuencias_Total = 500546,

        GLO_ProductCatalogPrecios_Lectura = 500547,
        GLO_ProductCatalogPrecios_Total = 500548,

        GLO_LoginSettings_Lectura = 500549,
        GLO_LoginSettings_Total = 500550,
        GLO_LoginSettings_Descarga = 500634,

        GLO_ServicesSettings_Total = 500553,
        GLO_ServicesSettings_Lectura = 500554,
        GLO_ServicesSettings_Descarga = 500635,

        GLO_GlobalSettingsContenedor_Total = 500640,
        GLO_GlobalSettingsContenedor_Lectura = 500641,
        GLO_GlobalSettingsContenedor_Descarga = 500642,


        #endregion

        #region ACCESS

        ACCESO_TOTAL_ACCESS_EMPLAZAMIENTOS = 354,
        ACCESO_USUARIO_A_ACCESS_EMPLAZAMIENTOS = 356,
        ACCESO_CLIENTE_A_ACCESS_EMPLAZAMIENTOS = 355,
        ACCESO_SOLO_LECTURA_A_ACCESS = 373,

        #endregion

        #region ADQUISICIONES

        ACCESO_TOTAL_A_ADQUISICIONES_ESTADOS = 131,
        ACCESO_TOTAL_ADQUISICIONES_EMPLAZAMIENTOS = 125,
        ACCESO_TOTAL_A_ADQUISICIONES_DBI = 1121,
        ACCESO_CLIENTE_A_ADQUISICIONES_EMPLAZAMIENTOS = 126,
        ACCESO_USUARIO_A_ADQUISICIONES_EMPLAZAMIENTOS = 127,
        ACCESO_SOLO_LECTURA_A_ADQUISICIONES = 156,

        #endregion

        #region ADQUISICIONES SARF

        ACCESO_TOTAL_A_ADQUISICIONES_ESTADOS_SARF = 1129,

        #endregion

        #region AMBIENTAL

        ACCESO_TOTAL_AMBIENTAL_ESTADOS = 7133,

        #endregion

        #region AMPLIACIONES

        ACCESO_TOTAL_A_AMPLIACIONES_ESTADOS = 8213,

        #endregion

        #region ASSETS PURCHASE

        ACCESO_TOTAL_A_ASSETS_PURCHASE_ESTADOS = 8322,

        #endregion

        #region AUDIT

        ACCESO_TOTAL_AUDIT_EMPLAZAMIENTOS = 304,
        ACCESO_USUARIO_A_AUDIT_EMPLAZAMIENTOS = 306,
        ACCESO_SOLO_LECTURA_A_AUDIT = 336,

        #endregion

        #region BILLING

        ACCESO_TOTAL_A_BILLING_ESTADOS = 810,
        ACCESO_TOTAL_A_BILLING_IMPUESTOS = 836,
        ACCESO_TOTAL_BILLING_EMPLAZAMIENTOS = 807,
        ACCESO_USUARIO_A_BILLING_EMPLAZAMIENTOS = 809,
        ACCESO_SOLO_LECTURA_A_BILLING_EMPLAZAMIENTOS = 825,
        ACCESO_SOLO_LECTURA_A_BILLING = 825,

        #endregion

        #region CALIDAD
        ACCESO_TOTAL_CALIDAD_DQCATEGORIAS = 7423,
        ACCESO_TOTAL_CALIDAD_DQGRUPOS = 7330,
        ACCESO_SOLO_LECTURA_A_CALIDAD_DQGRUPOS = 7331,
        CLD_CalidadKPI_Total = 500102,
        CLD_CalidadKPI_Lectura = 500103,
        ACCESO_TOTAL_CALIDAD_DQSEMAFOROS = 7329,
        ACCESO_SOLO_LECTURA_A_CALIDAD_DQSEMAFOROS = 7420,

        #endregion

        #region CITY

        ACCESO_TOTAL_A_CITY_ESTADOS = 101,

        #endregion

        #region DESPLIEGUE

        ACCESO_TOTAL_DESPLIEGUE_ESTADOS = 7529,

        #endregion

        #region DOCUMENTAL

        ACCESO_TOTAL_A_DOCUMENTAL_ESTADOS = 260,
        ACCESO_TOTAL_DOCUMENTAL_EMPLAZAMIENTOS = 254,
        ACCESO_USUARIO_A_DOCUMENTAL_EMPLAZAMIENTOS = 256,
        ACCESO_TOTAL_A_DOCUMENTAL_TIPOS_DOCUMENTOS = 268,
        ACCESO_RESTRINGIDO_A_DOCUMENTAL_TIPOS_DOCUMENTOS = 269,
        ACCESO_SOLO_LECTURA_A_DOCUMENTAL = 270,
        ACCESO_TOTAL_DOCUMENTAL_EXTENSIONES = 271,
        ACCESO_RESTRINGIDO_A_DOCUMENTAL_EXTENSIONES = 272,
        ACCESO_SOLO_LECTURA_A_DOCUMENTAL_METADATOS = 290,
        ACCESO_TOTAL_DOCUMENTAL_METADATOS = 291,
        ACCESO_TOTAL_DOCUMENTOS_ESTADOS = 292,
        ACCESO_LECTURA_DOCUMENTOS_ESTADOS = 293,
        GLO_DocumentosVista_Usuario = 500108,
        DOC_DocumentosHistoricos_Total = 500547,
        DOC_DocumentosHistoricos_Lectura = 500548,
        GLO_ServiciosFrecuencias_Lectura = 500554,
        GLO_ServiciosFrecuencias_Total = 500553,

        #endregion

        #region ENERGY

        ACCESO_TOTAL_ENERGY_EMPLAZAMIENTOS = 214,
        ACCESO_CLIENTE_A_ENERGY_EMPLAZAMIENTOS = 215,
        ACCESO_USUARIO_A_ENERGY_EMPLAZAMIENTOS = 216,
        ACCESO_SOLO_LECTURA_A_ENERGY = 236,

        #endregion

        #region EXPORTAR/IMPORTAR

        GLO_DataUpload_Lectura = 500067,
        GLO_DataUpload_Total = 500066,
        GLO_ExportImport = 500063,
        GLO_AdminTemplate_Total = 500068,
        GLO_AdminTemplate_Lectura = 500069,
        GLO_DataUploadGrid_Lectura = 500079,
        GLO_DataUploadGrid_Total = 500080,
        MEI_ExpInicio_Lectura = 500090,
        MEI_ExpInicio_Total = 500091,
        MEI_ExportacionDatosPlantillas_Total = 500551,
        MEI_ExportacionDatosPlantillas_Lectura = 500552,

        #endregion

        #region FIRMA DIGITAL

        ACCESO_TOTAL_A_FIRMA_DIGITAL_ESTADOS = 1734,

        #endregion

        #region INCIDENCIAS

        ACCESO_TOTAL_PROPERTY_EMPLAZAMIENTOS = 614,
        ACCESO_SOLO_LECTURA_PROPERTY_INCIDENCIAS = 625,
        ACCESO_SOLO_LECTURA_A_PROPERTY_EMPLAZAMIENTOS = 644,

        #endregion

        #region INDOOR

        ACCESO_TOTAL_A_INDOOR_ESTADOS = 4510,
        ACCESO_TOTAL_INDOOR_EMPLAZAMIENTOS = 4504,
        ACCESO_USUARIO_A_INDOOR_EMPLAZAMIENTOS = 4506,
        ACCESO_SOLO_LECTURA_A_INDOOR_EMPLAZAMIENTOS = 4531,
        ACCESO_TOTAL_INVENTARIO_EMPLAZAMIENTOS = 4702,
        ACCESO_CLIENTE_A_INVENTARIO_EMPLAZAMIENTOS = 4703,
        ACCESO_USUARIO_A_INVENTARIO_EMPLAZAMIENTOS = 4704,

        #endregion

        #region INSTALL OBRA CIVIL

        ACCESO_TOTAL_A_INSTALL_OBRA_CIVIL_ESTADOS = 457,
        ACCESO_TOTAL_A_INSTALL_OBRA_CIVIL_EMPLAZAMIENTOS = 464,
        ACCESO_RESTRINGIDA_A_INSTALL_OBRA_CIVIL_EMPLAZAMIENTOS = 466,
        ACCESO_USUARIO_A_INSTALL_OBRA_CIVIL_EMPLAZAMIENTOS = 1100,
        ACCESO_CLIENTE_A_INSTALL_OBRA_CIVIL_EMPLAZAMIENTOS = 465,
        ACCESO_SOLO_LECTURA_A_INSTALL_OBRA_CIVIL = 467,

        #endregion

        #region INSTALL TECNICA

        ACCESO_TOTAL_A_INSTALL_TECNICA_ESTADOS = 445,
        ACCESO_TOTAL_INSTALL_TECNICA_EMPLAZAMIENTOS = 452,
        ACCESO_USUARIO_INSTALL_TECNICA_EMPLAZAMIENTOS = 439,
        ACCESO_CLIENTE_INSTALL_TECNICA_EMPLAZAMIENTOS = 453,
        ACCESO_SOLO_LECTURA_A_INSTALL_TECNICA = 455,

        #endregion

        #region INVENTARIO

        ACCESO_TOTAL_A_INVENTARIO_CATEGORIAS = 4731,
        ACCESO_SOLO_LECTURAL_A_INVENTARIO_CATEGORIAS = 4732,
        ACCESO_TOTAL_A_INVENTARIO_ACTIVOS_REPOSITORIOS = 4752,
        ACCESO_RESTRINGIDO_A_INVENTARIO_ACTIVOS_REPOSITORIOS = 4753,
        ACCESO_TOTAL_INVENTARIO_ELEMENTOS_ATRIBUTOS_ESTADOS = 4763,
        ACCESO_LECTURA_INVENTARIO_ELEMENTOS_ATRIBUTOS_ESTADOS = 500531,
        ACCESO_TOTAL_A_INVENTARIO_ATRIBUTOS_PREDEFINIDOS = 4761,
        ACCESO_RESTRINGIDO_A_INVENTARIO_ATRIBUTOS_PREDEFINIDOS = 4762,
        IN_InventarioCategoriasAtributos_Lectura = 500032,
        IN_InventarioCategoriasAtributos_Total = 500033,
        INV_InventarioCategoriasContenedor_Total = 500529,
        INV_InventarioCategoriasContenedor_Lectura = 500530,
        INV_InventarioCategoryViewVistaCategoria_Total = 500059,
        INV_InventarioCategoryViewVistaCategoria_Lectura = 500060,
        INV_InventarioCategoriasAtributosPlantillas_Total = 500555,
        INV_InventarioCategoriasAtributosPlantillas_Lectura = 500556,

        INV_InventarioCategoryView_Total = 500046,
        INV_InventarioCategoryView_Lectura = 500047,
        INV_InventarioGestion_Content_Total = 500042,
        INV_InventarioGestion_Content_Lectura = 500043,
        INV_InventarioGestionContenedor_Lectura = 500044,
        INV_InventarioGestionContenedor_Total = 500045,
        INV_InventarioGestionContenedor_Plantilla = 500532,
        INV_InventarioElementoHistoricos_Lectura = 500510,
        INV_InventarioElementoHistoricos_Total = 500511,
        INV_ElementoHistoricos_Lectura = 500512,
        INV_ElementoHistoricos_Total = 500513,
        INV_InventarioCategoriasVinculaciones_Total = 500523,
        INV_InventarioCategoriasVinculaciones_Lectura = 500524,

        ACCESO_TOTAL_A_INVENTARIO_PLANTILLAS = 4740,
        ACCESO_SOLO_LECTURA_A_INVENTARIO_PLANTILLAS = 4741,
        ACCESO_TOTAL_A_INVENTARIO_INVENTARIO = 4759,
        ACCESO_RESTRINGIDO_A_INVENTARIO_INVENTARIO = 4760,


        INV_InventarioTiposVinculaciones_Lectura = 500525,
        INV_InventarioTiposVinculaciones_Total = 500526,
        INV_InventarioInicio_Total = 500648,
        #endregion

        #region LEGAL

        ACCESO_TOTAL_A_LEGALIZACIONES_ESTADOS = 508,
        ACCESO_TOTAL_LEGALIZACIONES_EMPLAZAMIENTOS = 502,
        ACCESO_CLIENTE_A_LEGALIZACIONES_EMPLAZAMIENTOS = 503,
        ACCESO_USUARIO_A_LEGALIZACIONES_EMPLAZAMIENTOS = 504,
        ACCESO_SOLO_LECTURA_A_LEGALIZACIONES = 529,

        #endregion

        #region MANTENIMIENTO

        ACCESO_CLIENTE_A_MANTENIMIENTO_EMPLAZAMIENTOS = 412,
        ACCESO_TOTAL_MANTENIMIENTO_EMPLAZAMIENTOS = 413,
        ACCESO_SOLO_LECTURA_A_MANTENIMIENTO = 416,
        ACCESO_USUARIO_MANTENIMIENTO_EMPLAZAMIENTOS = 417,
        ACCESO_TOTAL_A_MANTENIMIENTO_EMPLAZAMIENTOS_CORRECTIVOS = 418,
        ACCESO_USUARIO_MANTENIMIENTO_EMPLAZAMIENTOS_CORRECTIVOS = 419,
        ACCESO_CLIENTE_A_MANTENIMIENTO_EMPLAZAMIENTOS_CORRECTIVOS = 420,


        MTO_MantenimientoIncidenciasTipo_Lectura = 500051,
        MTO_MantenimientoIncidenciasTipo_Total = 500052,
        MTO_MantenimientoIncidencias_Lectura = 500053,
        MTO_MantenimientoIncidencias_Total = 500054,

        #endregion

        #region MONITORING

        ACCESO_TOTAL_ESTADISTICAS = 2022,
        ACCESO_TOTAL_A_MONITORING_DIARIO = 4911,
        ACCESO_TOTAL_A_MONITORING_LOGS = 4921,
        ACCESO_TOTAL_A_MONITORING_SERVICIOS_WEB = 4925,
        ACCESO_TOTAL_A_MONITORING_MODIFICACIONES_USUARIOS = 4941,
        MON_MonitoringLogServicio_Total = 500521,
        MON_MonitoringLogServicio_Lectura = 500522,
        #endregion

        #region PLANNING

        ACCESO_TOTAL_PLANNING_ESTADOS = 4833,

        #endregion

        #region SAVING

        ACCESO_TOTAL_A_SAVING_ESTADOS = 480,
        ACCESO_TOTAL_SAVING_EMPLAZAMIENTOS = 474,
        ACCESO_CLIENTE_A_SAVING_EMPLAZAMIENTOS = 475,
        ACCESO_USUARIO_A_SAVING_EMPLAZAMIENTOS = 476,
        ACCESO_SOLO_LECTURA_A_SAVING = 469,

        #endregion

        #region SHARING

        ACCESO_TOTAL_A_SHARING_ESTADOS = 71,
        ACCESO_TOTAL_SHARING_EMPLAZAMIENTOS = 65,
        ACCESO_CLIENTE_A_SHARING_EMPLAZAMIENTOS = 66,
        ACCESO_USUARIO_A_SHARING_EMPLAZAMIENTOS = 67,
        ACCESO_SOLO_LECTURA_A_SHARING = 88,

        #endregion

        #region SPACE

        ACCESO_TOTAL_A_SPACE_ESTADOS = 715,
        ACCESO_TOTAL_SPACE_EMPLAZAMIENTOS = 709,
        ACCESO_USUARIO_A_SPACE_EMPLAZAMIENTOS = 711,
        ACCESO_SOLO_LECTURA_A_SPACE_EMPLAZAMIENTOS = 732,

        #endregion

        #region SSRR

        ACCESO_TOTAL_A_SSRR_ESTADOS = 7801,

        #endregion

        #region SWAPPING

        ACCESO_TOTAL_A_SWAPPING_EMPLAZAMIENTOS_ESTADOS = 908,

        #endregion

        #region TOWER

        ACCESO_TOTAL_A_TOWER_ESTADOS = 1310,
        ACCESO_TOTAL_TOWER_EMPLAZAMIENTOS = 1304,
        ACCESO_USUARIO_A_TOWER_EMPLAZAMIENTOS = 1306,
        ACCESO_SOLO_LECTURA_A_TOWER_EMPLAZAMIENTOS = 1335,

        #endregion

        #region TOWER SARF

        ACCESO_TOTAL_A_TOWER_ESTADOS_SARF = 1348,

        #endregion

        #region UNINSTALL ADMIN

        ACCESO_TOTAL_A_UNINSTALL_ADMIN_ESTADOS = 174,
        ACCESO_TOTAL_UNINSTALL_ADMIN_EMPLAZAMIENTOS = 165,
        ACCESO_CLIENTE_A_UNINSTALL_ADMIN_EMPLAZAMIENTOS = 166,
        ACCESO_USUARIO_A_UNINSTALL_ADMIN_EMPLAZAMIENTOS = 167,
        ACCESO_SOLO_LECTURA_UNINSTALL_ADMIN = 208,

        #endregion

        #region UNINSTALL ELECTRICA

        ACCESO_TOTAL_A_UNINSTALL_ELECTRICA_ESTADOS = 195,

        #endregion

        #region UNINSTALL TECNICA

        ACCESO_TOTAL_A_UNINSTALL_TECNICA_ESTADOS = 176,

        #endregion

        #region VANDALISMO

        ACCESO_TOTAL_VANDALISMO_ESTADOS = 7935,

        #endregion

        #region APP
        APP_ACCESO_VALIDACIONES = 11000,
        APP_ACCESO_DOBLE_FACTOR = 11001,
        #endregion
    }

    public static string VISTA_GENERAL_MODULO_GLOBAL = "dbo.vw_Emplazamientos";

    #region GESTION ACCESO

    internal static string BloqueoUsuario(string txtUsername)
    {
        Usuarios usuario = new Usuarios();
        string bloqueado = "";
        UsuariosController us = new UsuariosController();

        if (us.GetItemsList("EMail == \"" + txtUsername + "\"").Count > 0)
        {
            usuario = us.GetItem("EMail == \"" + txtUsername + "\"");
            usuario.Activo = false;
            us.UpdateItem(usuario);

            IntentosClave = 0;
        }

        return bloqueado;
    }

    #endregion

    #region TEMAS

    public static Ext.Net.Theme Tema(string tema)
    {

        switch (tema)
        {
            case "0":
                return Ext.Net.Theme.Aria;

            case "1":
                return Ext.Net.Theme.Classic;

            case "2":
                return Ext.Net.Theme.Crisp;

            case "3":
                return Ext.Net.Theme.CrispTouch;

            case "4":
                return Ext.Net.Theme.Graphite;

            case "5":
                return Ext.Net.Theme.Gray;

            case "6":
                return Ext.Net.Theme.Material;

            case "7":
                return Ext.Net.Theme.Neptune;

            case "8":
                return Ext.Net.Theme.NeptuneTouch;

            case "9":
                return Ext.Net.Theme.None;

            case "10":
                return Ext.Net.Theme.Triton;


            default:
                return Ext.Net.Theme.Triton;
        }
    }

    #endregion

    #region CULTURE

    /// <summary>
    /// Cambia el idioma de la página al especificado como argumento
    /// </summary>
    /// <param name="locale">Idioma a establecer</param>
    /// <remarks>
    /// </remarks>
    public static void SetCulture(string locale)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(locale);
        System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(locale);
        System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

    }

    /// <summary>
    /// Agregar las Variables JS a la Peticion antes de mandarla al Cliente
    /// </summary>
    /// <param name="res"></param>
    /// <param name="culture"></param>
    /// <param name="Modulo"></param>
    public static void AddCurrentCultureJavascript(ResourceManager res, string culture, string Modulo)
    {

#if SERVICESETTINGS
        double TiempoSesionMinutos = Double.Parse(System.Configuration.ConfigurationManager.AppSettings["TiempoSesionMinutos"]);
#elif TREEAPI
            double TiempoSesionMinutos = TreeAPI.Properties.Settings.Default.TiempoSesionMinutos;
#else
        double TiempoSesionMinutos = TreeCore.Properties.Settings.Default.TiempoSesionMinutos;
#endif


        string locale = CultureName(culture);
        res.RegisterClientScriptBlock("Comun", CargarVariablesJavascript("Comun", locale, false));
        res.RegisterClientScriptBlock("Current", CargarVariablesJavascript(Modulo + ".aspx", locale, true));
        res.RegisterClientScriptBlock("Sesion", "TiempoSesion = " + TimeSpan.FromMinutes(TiempoSesionMinutos).TotalMilliseconds.ToString() + ";");
        res.Locale = locale;
    }

    
    public static string GetCultureBBDD()
    {
        SqlConnection connection = null;
        string sQuery = "select @@language";
        string result = "";
        #if SERVICESETTINGS
                    string conexion = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
                    DataTable dtResultado = new DataTable();
#elif TREEAPI
                    string conexion = TreeAPI.Properties.Settings.Default.Conexion;
                    DataTable dtResultado = new DataTable();
#else
        string conexion = TreeCore.Properties.Settings.Default.Conexion;
        DataTable dtResultado = new DataTable();
#endif
        try
        {
            using (connection = new SqlConnection(conexion))
            {
                //connection.Open();
                if (sQuery != null)
                {
                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    if (connection != null && connection.State == ConnectionState.Open)
                    {
                        SqlCommand command = new SqlCommand(sQuery, connection);
                        int pru = command.ExecuteNonQuery();

                        SqlDataAdapter sda = new SqlDataAdapter(command);
                        sda.Fill(dtResultado);
                        result = dtResultado.Rows[0][0].ToString();
                        CultureBBDD = result;
                        connection.Close();
                       
                        
                    }
                }

            }
            return result;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
          
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return result;
        }
    }

    /// <summary>
    /// Devuelve el idioma en formato idioma-variante
    /// </summary>
    /// <param name="valor">Culture a buscar</param>
    /// <returns>Idioma en el Formato idioma-variante (por ejemplo, es-ES)</returns>
    public static string CultureName(string valor)
    {
        CultureInfo[] cultures = System.Globalization.CultureInfo.GetCultures
                     (CultureTypes.SpecificCultures);
        string cultureBBDD = "";

        try
        {
            CultureInfo selectCulture = (from p in cultures where p.DisplayName == valor select p).First();
            cultureBBDD = GetCultureBBDD();
            return selectCulture.Name;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return DefaultLocale;
        }
    }

    public static string CargarVariablesJavascript(string Pagina, string Locale, bool Local)
    {
        System.Text.StringBuilder res = new System.Text.StringBuilder();

        try
        {
            string[] aux = Locale.Split('-');
            string idioma = aux[1].ToLower();
            string archivo = "";


            string directorio = "/App_GlobalResources";
            if (Local)
            {
                directorio = "App_LocalResources";
            }

            if (aux[1].ToLower().Equals(aux[0].ToLower()))
            {
                archivo = HttpContext.Current.Server.MapPath(directorio + "/" + Pagina + "." + idioma + ".resx");
            }
            else if (aux[0].ToLower() == "en" && aux[1] == "US")
            {
                archivo = HttpContext.Current.Server.MapPath(directorio + "/" + Pagina + "." + aux[0].ToLower() + ".resx");
            }
            else
            {
                archivo = HttpContext.Current.Server.MapPath(directorio + "/" + Pagina + "." + Locale.ToLower() + ".resx");
            }


            if (!System.IO.File.Exists(archivo))
            {
                // obtenemos el archivo por defecto si no tenemos el del idioma seleccionado
                archivo = HttpContext.Current.Server.MapPath(directorio + "/" + Pagina + ".resx");
            }

            if (System.IO.File.Exists(archivo))
            {

                System.Resources.ResXResourceReader resReader = new System.Resources.ResXResourceReader(archivo);

                DictionaryEntry entry = default(DictionaryEntry);
                string key = null;
                string valor = null;

                foreach (DictionaryEntry entry_loopVariable in resReader)
                {
                    entry = entry_loopVariable;
                    key = entry.Key.ToString();
                    if (key.StartsWith("js"))
                    {
                        valor = entry.Value.ToString();
                        res.AppendLine("var " + key + " = '" + valor + "';");
                    }

                }
                resReader.Close();
                resReader = null;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }


        return res.ToString();

    }

    public static string cargarVariablesSTRJavascript(string Pagina, string Locale, bool Local)
    {
        System.Text.StringBuilder res = new System.Text.StringBuilder();

        try
        {
            string sIdioma = Locale;
            string sArchivo = "";
            string sDirectorio = "/App_GlobalResources";

            if (sIdioma == "en" || sIdioma == "US")
            {
                sArchivo = HttpContext.Current.Server.MapPath(sDirectorio + "/" + Pagina + "." + sIdioma + ".resx");
            }
            else
            {
                sArchivo = HttpContext.Current.Server.MapPath(sDirectorio + "/" + Pagina + "." + Locale.ToLower() + ".resx");
            }


            if (!System.IO.File.Exists(sArchivo))
            {
                sArchivo = HttpContext.Current.Server.MapPath(sDirectorio + "/" + Pagina + ".resx");
            }

            if (System.IO.File.Exists(sArchivo))
            {

                System.Resources.ResXResourceReader resReader = new System.Resources.ResXResourceReader(sArchivo);

                DictionaryEntry entry = default(DictionaryEntry);
                string key = null;
                string valor = null;

                res.AppendLine("var array = [");

                foreach (DictionaryEntry entry_loopVariable in resReader)
                {
                    entry = entry_loopVariable;
                    key = entry.Key.ToString();
                    if (key.StartsWith("str") || key.StartsWith("js"))
                    {

                        valor = entry.Value.ToString();
                        valor = valor.Replace("'", "\\'");
                        res.AppendLine("{key: '" + key + "', valor: '" + valor + "'},");
                    }

                }

                res.AppendLine("];");
                resReader.Close();
                resReader = null;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }


        return res.ToString();

    }

    public static string CabeceraDeJavascriptYCSSVersionOrig(string Pagina, string Directorio, bool interno, bool NoPrincipal)
    {
        string dir_js = "/JS";
        string cadenaComun = "<script type=\"text/javascript\" src=\"" + dir_js + "/common.js?Version=" + Version + "\"></script>";
        string cadenaPagina = "<script type=\"text/javascript\" src=\"" + Directorio + "/" + Pagina + ".js?Version=" + Version + "\"></script>";

        string dir_css = "/CSS";

        if (Directorio != null)
        {
            if (Directorio.Contains("pages"))
            {
                dir_css = "/CSS";
            }
            //if (interno)
            //{
            //    dir_ima = "../../ima";
            //}
            //if (NoPrincipal)
            //{
            //    dir_ima = "../ima";
            //}
            string nl = System.Environment.NewLine;
            string cadenaCSS = "<link href=\"" + dir_css + "/tCore.min.css?Version=" + Version + "\" rel=\"stylesheet\" type=\"text/css\" />" + System.Environment.NewLine;
            cadenaCSS = cadenaCSS + "<link href = \"https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap\" rel =\"stylesheet\"/>" + System.Environment.NewLine;
            string cssProp = Directorio + "/css/style" + Pagina + ".css";
            string cadenaFavIcon = "<link runat=\"server\" type=\"image/x-icon\" rel=\"shortcut icon\" href=\"/ima/favicon.ico\"/>";
            string jquery = "<script type=\"text/javascript\" src=\"/Scripts/jquery-3.5.1.min.js\" ></script>" +
                "<script type=\"text/javascript\" src=\"/Scripts/jquery-ui-1.12.1.custom/jquery-ui.js\"></script>" +
                "<script type=\"text/javascript\" src=\"/Scripts/bootstrap.min.js\"></script>" +
                "<script src=\"/Scripts/js.cookie.min.js\"></script>" +
                "<script type=\"text/javascript\" src=\"/Scripts/jquery-sortable.js\"></script>" +
                "<script type=\"text/javascript\" src=\"/Scripts/bootstrap-toaster.js\"></script>" +
                "<script type=\"text/javascript\" src=\"/Scripts/jquery.inactivity.min.js\"></script>" + System.Environment.NewLine +
                "<link href = \"" + dir_css + "/bootstrap-toaster.css\" rel =\"stylesheet\"/>" + System.Environment.NewLine /*+
                "<link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta3/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-eOJMYsd53ii+scO/bJGFsiCZc+5NDVN2yr8+0RDqr0Ql0h+rP48ckxlpbzKgwra6\" crossorigin=\"anonymous\">" + System.Environment.NewLine +
                "<link rel=\"stylesheet\" href=\"https://cdn.jsdelivr.net/npm/bootstrap-icons@1.4.1/font/bootstrap-icons.css\">"*/;




            return jquery + cadenaComun + nl + cadenaPagina + nl + cadenaCSS + nl + cadenaFavIcon + nl + nl;
        }
        else
        {
            return "";
        }



    }


    public static string CabeceraDeJavascriptYCSS(string Pagina, string Directorio, bool interno, bool NoPrincipal)
    {
        string connectionURI = HttpContext.Current.Request.Url.AbsoluteUri;

        var uri = new Uri(connectionURI);
        string url = connectionURI = uri.GetLeftPart(UriPartial.Authority);

        ParametrosController cParametros = new ParametrosController();
        TreeCore.Data.Parametros param = new TreeCore.Data.Parametros();
        param = cParametros.GetItemByName("CERTIFICADO_SSL");

        if (param != null &&
            (param.Valor == "SI" || param.Valor == "YES"))
        {
            if (connectionURI.Contains("http:"))
                url = connectionURI.Replace("http:", "https:");

        }
        else
        {
            if (connectionURI.Contains("https:"))
                url = connectionURI.Replace("https:", "http:");

        }


        string cadenaComun = "<script type=\"text/javascript\" src=\"" + url + "/" + Directorio + "/common.js?Version=" + Version + "\"></script>";
        string cadenaPagina = "<script type=\"text/javascript\" src=\"" + url + "/" + Directorio + "/" + Pagina + ".js?Version=" + Version + "\"></script>";

        string dir_ima = "../ima";
        if (Directorio.Contains("pages"))
        {
            dir_ima = url + "/" + "ima";
        }
        if (interno)
        {
            dir_ima = "../../ima";
        }
        if (NoPrincipal)
        {
            dir_ima = "../ima";
            dir_ima = url + "/" + "ima";
        }


        string nl = System.Environment.NewLine;
        string cadenaCSS = "<link href=\"" + dir_ima + "/generic.css?Version=" + Version + "\" rel=\"stylesheet\" type=\"text/css\" />" + System.Environment.NewLine;
        string cadenaFavIcon = "<link runat=\"server\" type=\"image/x-icon\" rel=\"shortcut icon\" href=\"/ima/favicon.ico\"/>";

        //string cadenaGoogle = "<script type=\"text/javascript\">" + "var _gaq = _gaq || [];" + "_gaq.push(['_setAccount', 'UA-45128600-1']);" + "_gaq.push(['_trackPageview']);" + "(function() {" + "var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;" + "ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';" + "var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);" + "})();" + "</script>";

        //se ha quitado la cadenaGoogle 0903207 AHP para que se tenga que conectar con google analitic

        return cadenaComun + nl + cadenaPagina + nl + cadenaCSS + nl + cadenaFavIcon + nl;

    }


    #endregion

    #region GRID FILTERS

    public static string GetBooleanAsString(string strValue, string localeStr, bool preferYesNo)
    {
        System.Resources.ResourceManager rm = Resources.Comun.ResourceManager;

        string result = strValue;
        try
        {
            if (rm != null)
            {
                if (preferYesNo)
                {
                    if (strValue.ToUpper().Equals("TRUE"))
                        result = rm.GetString("BOOL_STR_YES", new System.Globalization.CultureInfo(localeStr));
                    else if (strValue.ToUpper().Equals("FALSE"))
                        result = rm.GetString("BOOL_STR_NO", new System.Globalization.CultureInfo(localeStr));
                    else
                        result = strValue;
                }
                else
                {
                    if (strValue.ToUpper().Equals("TRUE"))
                        result = rm.GetString("BOOL_STR_TRUE", new System.Globalization.CultureInfo(localeStr));
                    else if (strValue.ToUpper().Equals("FALSE"))
                        result = rm.GetString("BOOL_STR_FALSE", new System.Globalization.CultureInfo(localeStr));
                    else
                        result = strValue;
                }
            }
        }
        catch (SystemException ex)
        {
            System.Console.Write(ex);

            result = strValue;
        }

        return result;
    }
    public static string GetComunString(string keyValue, string localeStr)
    {
        System.Resources.ResourceManager rm = Resources.Comun.ResourceManager;

        string result = keyValue;
        try
        {
            if (rm != null)
            {
                result = rm.GetString(keyValue, new System.Globalization.CultureInfo(localeStr));
            }
        }
        catch (SystemException ex)
        {
            System.Console.Write(ex);

            result = keyValue;
        }

        return result;
    }
    public static void CreateGridFilters(Ext.Net.GridFilters gridFilter, Store gridStore, Ext.Net.GridHeaderContainer columnModel, List<string> ignoreList, string currentLocale)
    {
        Dictionary<string, ModelField> dataFields = new Dictionary<string, ModelField>();

        if (gridStore.Model.Count == 1)
        {
            Ext.Net.Model model = gridStore.Model[0];
            ModelFieldCollection fieldsCollection = model.Fields;

            foreach (ModelField f in fieldsCollection)
            {
                if (!dataFields.ContainsKey(f.Name))
                {
                    dataFields.Add(f.Name, f);
                }
            }
        }

        for (int k = 0; k < columnModel.Columns.Count; k = k + 1)
        {
            if (columnModel.Columns[k].DataIndex != null &&
                !columnModel.Columns[k].DataIndex.Equals("") &&
                !ignoreList.Contains(columnModel.Columns[k].DataIndex) &&
                dataFields.ContainsKey(columnModel.Columns[k].DataIndex))
            {
                ModelField f = dataFields[columnModel.Columns[k].DataIndex];
                GridFilter filter = new StringFilter();

                switch (f.Type)
                {
                    case ModelFieldType.Boolean:
                        filter = new BooleanFilter();
                        BooleanFilter bf = (BooleanFilter)filter;
                        bf.YesText = Resources.Comun.strYes;
                        bf.NoText = Resources.Comun.strNo;
                        break;
                    case ModelFieldType.Date:
                        filter = new DateFilter();
                        DateFilter df = (DateFilter)filter;
                        df.SubmitFormat = Resources.Comun.FormatFecha;
                        df.BeforeText = Resources.Comun.strBeforeText;
                        df.AfterText = Resources.Comun.strAfterText;
                        df.OnText = Resources.Comun.strOnText;
                        break;
                    case ModelFieldType.Float:
                        filter = new NumberFilter();
                        NumberFilter flof = (NumberFilter)filter;
                        flof.EmptyText = Resources.Comun.strNumber;
                        break;
                    case ModelFieldType.Int:
                        filter = new NumberFilter();
                        NumberFilter nf = (NumberFilter)filter;
                        nf.EmptyText = Resources.Comun.strNumber;
                        break;
                    case ModelFieldType.Auto:
                    case ModelFieldType.String:
                        filter = new StringFilter();
                        StringFilter sf = (StringFilter)filter;
                        sf.EmptyText = Resources.Comun.strFiltro;
                        break;
                    default:
                        break;
                }

                filter.DataIndex = columnModel.Columns[k].DataIndex;
                gridFilter.AddFilter(filter);
            }
        }
    }

    #endregion

    #region EXPORTACION

    public static void ExportacionDesdeListaNombre(Ext.Net.GridHeaderContainer columnModel, IList lista, HttpResponse response, [Optional, DefaultParameterValue("")] string ListaCamposExcluidos, string nombreFichero, string lenguaje)
    {
        string strHtml = "";

        if (lista != null)
        {
            try
            {
                strHtml += "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                strHtml += "<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">";
                strHtml += "<head>";
                strHtml += "<title></title>";
                strHtml += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />";
                strHtml += "</head>";
                strHtml += "<body>";
                strHtml += "<p>";
                strHtml += "<table border=\"1px\">";
                strHtml += EscribeCabecera(columnModel, ListaCamposExcluidos, lenguaje);
                foreach (object nodo in lista)
                {
                    strHtml += EscribeLinea(nodo, columnModel, ListaCamposExcluidos);
                }
                strHtml += " </table>";
                strHtml += "</p>";
                strHtml += " </body>";
                strHtml += "</html>";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        response.Clear();
        response.ContentType = Comun.GetMimeType(Comun.DOCUMENTO_EXTENSION_EXCEL_SIN_P);
        response.Charset = "";

        if (Path.GetExtension(nombreFichero) != String.Empty)
        {
            response.AddHeader("Content-Disposition", "attachment; filename=" + nombreFichero);
        }
        else
        {
            response.AddHeader("Content-Disposition", "attachment; filename=" + nombreFichero + Comun.DOCUMENTO_EXTENSION_EXCEL);
        }

        response.AddHeader("Content-Length", strHtml.Length.ToString());
        response.Write(strHtml);
        response.Flush();
        response.SuppressContent = true;

        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    public static void ExportacionDesdeListaNombreParalela(Ext.Net.GridHeaderContainer columnModel, IList lista, HttpResponse response, [Optional, DefaultParameterValue("")] string ListaCamposExcluidos, string nombreFichero, string lenguaje)
    {
        string strHtml = "";

        if (lista != null)
        {
            try
            {
                strHtml += "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                strHtml += "<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">";
                strHtml += "<head>";
                strHtml += "<title></title>";
                strHtml += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />";
                strHtml += "</head>";
                strHtml += "<body>";
                strHtml += "<p>";
                strHtml += "<table border=\"1px\">";
                strHtml += EscribeCabecera(columnModel, ListaCamposExcluidos, lenguaje);

                object key = new object(); // Empty object serves lightest for locks

                Parallel.ForEach<object>(lista.OfType<object>(), (object nodo) =>
                {
                    string linea = EscribeLinea(nodo, columnModel, ListaCamposExcluidos);

                    lock (key)
                    {
                        strHtml += linea;
                    }
                });


                strHtml += " </table>";
                strHtml += "</p>";
                strHtml += " </body>";
                strHtml += "</html>";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        response.Clear();
        response.ContentType = Comun.GetMimeType(Comun.DOCUMENTO_EXTENSION_EXCEL_SIN_P);
        response.Charset = "";

        if (Path.GetExtension(nombreFichero) != String.Empty)
        {
            response.AddHeader("Content-Disposition", "attachment; filename=" + nombreFichero);
        }
        else
        {
            response.AddHeader("Content-Disposition", "attachment; filename=" + nombreFichero + Comun.DOCUMENTO_EXTENSION_EXCEL);
        }

        response.AddHeader("Content-Length", strHtml.Length.ToString());
        response.Write(strHtml);
        response.Flush();
        response.SuppressContent = true;

        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    public async static System.Threading.Tasks.Task ExportacionDesdeListaNombreTask(Ext.Net.GridHeaderContainer columnModel, IList lista, HttpResponse response, [Optional, DefaultParameterValue("")] string ListaCamposExcluidos, string nombreFichero, string lenguaje)
    {
        byte[] bytes = new byte[] { };

        if (lista != null)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                CultureInfo culture = CultureInfo.GetCultureInfo(lenguaje);
                string columnaNoTraducida = HttpContext.GetGlobalResourceObject("Comun", strCOLUMNA_NO_TRADUCIDA, culture).ToString();

                var pagkage = new ExcelPackage();

                var ws = pagkage.Workbook.Worksheets.Add(nombreFichero);

                int i = 1;
                columnModel.Columns.ForEach(col =>
                {
                    if (col.DataIndex != null && !ListaCamposExcluidos.Contains(col.DataIndex))
                    {
                        string cabecera = !string.IsNullOrEmpty(col.Text) ? col.Text : ((!string.IsNullOrEmpty(col.ToolTip)) ? col.ToolTip : columnaNoTraducida).ToString();
                        ws.Cells[1, i++].Value = cabecera;
                    }
                });

                var range = ws.Cells["A2"].LoadFromCollection((IEnumerable<JsonObject>)lista);
                if (lista != null && lista.Count > 0)
                {
                    range.AutoFitColumns();
                }

                bytes = await pagkage.GetAsByteArrayAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        response.Clear();
        response.ContentType = Comun.GetMimeType(Comun.DOCUMENTO_EXTENSION_EXCEL_XLSX_SIN_P);
        response.Charset = "";

        if (Path.GetExtension(nombreFichero) != String.Empty)
        {
            response.AddHeader("Content-Disposition", "attachment; filename=" + nombreFichero);
        }
        else
        {
            response.AddHeader("Content-Disposition", "attachment; filename=" + nombreFichero + Comun.DOCUMENTO_EXTENSION_EXCEL_XLSX);
        }

        response.AddHeader("Content-Length", bytes.Count().ToString());
        HttpContext.Current.Response.BinaryWrite(bytes);
        HttpContext.Current.Response.Flush();
        //response.SuppressContent = true;

        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    private static string EscribeCabecera(Ext.Net.GridHeaderContainer colmodel, string CamposExcluidos, string lenguaje)
    {
        StringBuilder html = new StringBuilder();

        CultureInfo culture = CultureInfo.GetCultureInfo(lenguaje);
        string columnaNoTraducida = HttpContext.GetGlobalResourceObject("Comun", strCOLUMNA_NO_TRADUCIDA, culture).ToString();

        html.Append("<tr style=\"color: black; font:Arial, Helvetica, sans-serif\">");
        for (int i = 0; i < colmodel.Columns.Count; i++)
        {
            if (colmodel.Columns[i] is Ext.Net.Column)
            {
                Column col = (Column)(colmodel.Columns[i]);
                if (!col.Hidden)
                {
                    //if (!CamposExcluidos.Contains(col.DataIndex))
                    //{
                    html.Append(string.Format("<th style=\"font-weight: bold; font-size: 13px;\">{0}</th>", !string.IsNullOrEmpty(col.Text) ? col.Text : ((!string.IsNullOrEmpty(col.ToolTip)) ? col.ToolTip : columnaNoTraducida).ToString()));
                    //}
                }
            }
            else if (colmodel.Columns[i] is Ext.Net.NumberColumn)
            {
                NumberColumn col = (NumberColumn)(colmodel.Columns[i]);
                if (!col.Hidden)
                {
                    if (!CamposExcluidos.Contains(col.DataIndex))
                    {
                        html.Append(string.Format("<th style=\"font-weight: bold; font-size: 13px;\">{0}</th>", !string.IsNullOrEmpty(col.Text) ? col.Text : ((!string.IsNullOrEmpty(col.ToolTip)) ? col.ToolTip : columnaNoTraducida).ToString()));
                    }
                }
            }
            else if (colmodel.Columns[i] is Ext.Net.DateColumn)
            {
                DateColumn col = (DateColumn)(colmodel.Columns[i]);
                if (!col.Hidden)
                {
                    if (!CamposExcluidos.Contains(col.DataIndex))
                    {
                        html.Append(string.Format("<th style=\"font-weight: bold; font-size: 13px;\">{0}</th>", !string.IsNullOrEmpty(col.Text) ? col.Text : ((!string.IsNullOrEmpty(col.ToolTip)) ? col.ToolTip : columnaNoTraducida).ToString()));
                    }
                }
            }
            else if (colmodel.Columns[i] is Ext.Net.TreeColumn)
            {
                TreeColumn col = (TreeColumn)(colmodel.Columns[i]);
                if (!col.Hidden)
                {
                    if (!CamposExcluidos.Contains(col.DataIndex))
                    {
                        html.Append(string.Format("<th style=\"font-weight: bold; font-size: 13px;\">{0}</th>", !string.IsNullOrEmpty(col.Text) ? col.Text : ((!string.IsNullOrEmpty(col.ToolTip)) ? col.ToolTip : columnaNoTraducida).ToString()));
                    }
                }
            }
        }
        html.Append("</tr>");
        return (html.ToString());
    }

    private static string EscribeLinea(object nodo, Ext.Net.GridHeaderContainer colmodel, string CamposExcluidos)
    {
        StringBuilder html = new StringBuilder();
        string valor = "";
        object elem = null;
        string tipoelem = null;
        html.Append("<tr>");

        for (int i = 0; i < colmodel.Columns.Count; i++)
        {
            if (!colmodel.Columns[i].Hidden)
            {
                if (colmodel.Columns[i] is Ext.Net.Column)
                {
                    Column col = (Column)(colmodel.Columns[i]);

                    if (!col.Hidden)
                    {
                        //if (!CamposExcluidos.Contains(col.DataIndex))
                        //{
                        valor = "";
                        try
                        {
                            Type tipo = nodo.GetType();
                            System.Reflection.PropertyInfo propiedad = tipo.GetProperty(col.DataIndex);

                            if (propiedad != null)
                            {
                                elem = propiedad.GetValue(nodo, null);
                                if (elem != null)
                                {
                                    tipoelem = elem.GetType().ToString();
                                    switch (tipoelem)
                                    {
                                        case "System.String":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            break;
                                        case "Integer":
                                            if (elem != null)
                                            {
                                                valor = "&nbsp;" + elem;
                                            }
                                            else
                                            {
                                                valor = "";
                                            }
                                            break;
                                        case "System.DateTime":
                                            if (elem != null)
                                            {
                                                valor = ((DateTime)(elem)).ToString(Comun.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                            }
                                            else
                                            {
                                                valor = "";
                                            }
                                            break;
                                        case "System.Decimal":
                                        case "System.Double":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            else
                                            {
                                                valor = "0";
                                            }
                                            break;
                                        case "System.Int32":
                                        case "System.Int64":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            else
                                            {
                                                valor = "0";
                                            }
                                            break;
                                        case "System.Boolean":
                                            if ((Boolean)elem == true)
                                            {
                                                //valor = Resources.Comun.strSi;
                                                valor = "TRUE";
                                            }
                                            else
                                            {
                                                //valor = Resources.Comun.strNo;
                                                valor = "FALSE";
                                            }
                                            break;
                                        case "System.TimeSpan":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            break;
                                        default:
                                            valor = "";

                                            break;
                                    }
                                }
                                else
                                {
                                    System.Console.Write("");
                                    valor = "";
                                }
                            }
                            else if (tipo.Name == "JsonObject")
                            {
                                if (((JsonObject)nodo)[col.DataIndex] != null)
                                {
                                    valor = ((JsonObject)nodo)[col.DataIndex].ToString();
                                }
                                else
                                {
                                    valor = "";
                                }
                            }
                            else if (tipo.Name == "JObject")
                            {
                                if (((JObject)nodo)[col.DataIndex] != null)
                                {
                                    valor = ((JObject)nodo)[col.DataIndex].ToString();
                                }
                                else
                                {
                                    valor = "";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }

                        // remove invalid escape characters
                        valor.Replace("\\", "/");

                        // replace any separator chars to something safe
                        if (valor.Contains(Comun.EXCEL_SEP_CARACTER))
                            valor = valor.Replace(Comun.EXCEL_SEP_CARACTER, Comun.EXCEL_SEP_CARACTER_SAFE_REPLACE);

                        valor = HttpUtility.HtmlEncode(valor);

                        html.Append(string.Format("<td style=\"font-size:12px\">{0}</td>", valor));
                        //}
                    }
                }
                else if (colmodel.Columns[i] is Ext.Net.NumberColumn)
                {
                    NumberColumn col = (NumberColumn)(colmodel.Columns[i]);

                    if (!col.Hidden)
                    {
                        //if (!CamposExcluidos.Contains(col.DataIndex))
                        //{
                        valor = "";
                        try
                        {
                            Type tipo = nodo.GetType();
                            System.Reflection.PropertyInfo propiedad = tipo.GetProperty(col.DataIndex);

                            if (propiedad != null)
                            {
                                elem = propiedad.GetValue(nodo, null);
                                if (elem != null)
                                {
                                    tipoelem = elem.GetType().ToString();
                                    switch (tipoelem)
                                    {
                                        case "System.String":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            break;
                                        case "Integer":
                                            if (elem != null)
                                            {
                                                valor = "&nbsp;" + elem;
                                            }
                                            else
                                            {
                                                valor = "0";
                                            }
                                            break;
                                        case "System.DateTime":
                                            if (elem != null)
                                            {
                                                valor = ((DateTime)(elem)).ToString(Comun.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                            }
                                            else
                                            {
                                                valor = "";
                                            }
                                            break;
                                        case "System.Decimal":
                                        case "System.Double":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            else
                                            {
                                                valor = "0";
                                            }
                                            break;
                                        case "System.Int32":
                                        case "System.Int64":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            else
                                            {
                                                valor = "0";
                                            }
                                            break;
                                        case "System.Boolean":
                                            if ((Boolean)elem == true)
                                            {
                                                //valor = Resources.Comun.strSi;
                                                valor = "TRUE";
                                            }
                                            else
                                            {
                                                //valor = Resources.Comun.strNo;
                                                valor = "FALSE";
                                            }
                                            break;
                                        case "System.TimeSpan":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            break;
                                        default:
                                            valor = "";

                                            break;
                                    }
                                }
                                else
                                {
                                    System.Console.Write("");
                                    valor = "";
                                }
                            }
                            else if (tipo.Name == "JsonObject")
                            {
                                if (((JsonObject)nodo)[col.DataIndex] != null)
                                {
                                    valor = ((JsonObject)nodo)[col.DataIndex].ToString();
                                }
                                else
                                {
                                    valor = "";
                                }
                            }
                            else if (tipo.Name == "JObject")
                            {
                                if (((JObject)nodo)[col.DataIndex] != null)
                                {
                                    valor = ((JObject)nodo)[col.DataIndex].ToString();
                                }
                                else
                                {
                                    valor = "";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }

                        // remove invalid escape characters
                        valor.Replace("\\", "/");

                        // replace any separator chars to something safe
                        if (valor.Contains(Comun.EXCEL_SEP_CARACTER))
                            valor = valor.Replace(Comun.EXCEL_SEP_CARACTER, Comun.EXCEL_SEP_CARACTER_SAFE_REPLACE);

                        valor = HttpUtility.HtmlEncode(valor);

                        html.Append(string.Format("<td style=\"font-size:12px\">{0}</td>", valor));
                        //}
                    }
                }
                else if (colmodel.Columns[i] is Ext.Net.DateColumn)
                {
                    DateColumn col = (DateColumn)(colmodel.Columns[i]);

                    if (!col.Hidden)
                    {
                        //if (!CamposExcluidos.Contains(col.DataIndex))
                        //{
                        valor = "";
                        try
                        {
                            Type tipo = nodo.GetType();
                            System.Reflection.PropertyInfo propiedad = tipo.GetProperty(col.DataIndex);
                            if (propiedad != null)
                            {
                                elem = propiedad.GetValue(nodo, null);
                                if (elem != null)
                                {
                                    tipoelem = elem.GetType().ToString();
                                    switch (tipoelem)
                                    {
                                        case "System.String":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            break;
                                        case "Integer":
                                            if (elem != null)
                                            {
                                                valor = "&nbsp;" + elem;
                                            }
                                            else
                                            {
                                                valor = "";
                                            }
                                            break;
                                        case "System.DateTime":
                                            if (elem != null)
                                            {
                                                valor = ((DateTime)(elem)).ToString(Comun.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                            }
                                            else
                                            {
                                                valor = "";
                                            }
                                            break;
                                        case "System.Decimal":
                                        case "System.Double":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            else
                                            {
                                                valor = "0";
                                            }
                                            break;
                                        case "System.Int32":
                                        case "System.Int64":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            else
                                            {
                                                valor = "0";
                                            }
                                            break;
                                        case "System.Boolean":
                                            if ((Boolean)elem == true)
                                            {
                                                //valor = Resources.Comun.strSi;
                                                valor = "TRUE";
                                            }
                                            else
                                            {
                                                //valor = Resources.Comun.strNo;
                                                valor = "FALSE";
                                            }
                                            break;
                                        case "System.TimeSpan":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            break;
                                        default:
                                            valor = "";

                                            break;
                                    }
                                }
                                else
                                {
                                    System.Console.Write("");
                                    valor = "";
                                }
                            }
                            else if (tipo.Name == "JsonObject")
                            {
                                if (((JsonObject)nodo)[col.DataIndex] != null)
                                {
                                    valor = ((JsonObject)nodo)[col.DataIndex].ToString();
                                }
                                else
                                {
                                    valor = "";
                                }
                            }
                            else if (tipo.Name == "JObject")
                            {
                                if (((JObject)nodo)[col.DataIndex] != null)
                                {
                                    valor = ((JObject)nodo)[col.DataIndex].ToString();
                                }
                                else
                                {
                                    valor = "";
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }

                        // remove invalid escape characters
                        valor.Replace("\\", "/");

                        // replace any separator chars to something safe
                        if (valor.Contains(Comun.EXCEL_SEP_CARACTER))
                            valor = valor.Replace(Comun.EXCEL_SEP_CARACTER, Comun.EXCEL_SEP_CARACTER_SAFE_REPLACE);

                        valor = HttpUtility.HtmlEncode(valor);

                        html.Append(string.Format("<td style=\"font-size:12px\">{0}</td>", valor));
                        //}
                    }
                }
                else if (colmodel.Columns[i] is Ext.Net.TreeColumn)
                {
                    TreeColumn col = (TreeColumn)(colmodel.Columns[i]);

                    if (!col.Hidden)
                    {
                        //if (!CamposExcluidos.Contains(col.DataIndex))
                        //{
                        valor = "";
                        try
                        {
                            Type tipo = nodo.GetType();
                            System.Reflection.PropertyInfo propiedad = tipo.GetProperty(col.DataIndex);

                            if (propiedad != null)
                            {
                                elem = propiedad.GetValue(nodo, null);
                                if (elem != null)
                                {
                                    tipoelem = elem.GetType().ToString();
                                    switch (tipoelem)
                                    {
                                        case "System.String":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            break;
                                        case "Integer":
                                            if (elem != null)
                                            {
                                                valor = "&nbsp;" + elem;
                                            }
                                            else
                                            {
                                                valor = "";
                                            }
                                            break;
                                        case "System.DateTime":
                                            if (elem != null)
                                            {
                                                valor = ((DateTime)(elem)).ToString(Comun.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                            }
                                            else
                                            {
                                                valor = "";
                                            }
                                            break;
                                        case "System.Decimal":
                                        case "System.Double":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            else
                                            {
                                                valor = "0";
                                            }
                                            break;
                                        case "System.Int32":
                                        case "System.Int64":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            else
                                            {
                                                valor = "0";
                                            }
                                            break;
                                        case "System.Boolean":
                                            if ((Boolean)elem == true)
                                            {
                                                //valor = Resources.Comun.strSi;
                                                valor = "TRUE";
                                            }
                                            else
                                            {
                                                //valor = Resources.Comun.strNo;
                                                valor = "FALSE";
                                            }
                                            break;
                                        case "System.TimeSpan":
                                            if (elem != null)
                                            {
                                                valor = elem.ToString();
                                            }
                                            break;
                                        default:
                                            valor = "";

                                            break;
                                    }
                                }
                                else
                                {
                                    System.Console.Write("");
                                    valor = "";
                                }
                            }
                            else if (tipo.Name == "JsonObject")
                            {
                                valor = ((JsonObject)nodo)[col.DataIndex].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }

                        // remove invalid escape characters
                        valor.Replace("\\", "/");

                        // replace any separator chars to something safe
                        if (valor.Contains(Comun.EXCEL_SEP_CARACTER))
                            valor = valor.Replace(Comun.EXCEL_SEP_CARACTER, Comun.EXCEL_SEP_CARACTER_SAFE_REPLACE);

                        valor = HttpUtility.HtmlEncode(valor);

                        html.Append(string.Format("<td style=\"font-size:12px\">{0}</td>", valor));
                        //}
                    }
                }
            }
        }

        html.Append("</tr>");
        return (html.ToString());
    }

    public static void ExportacionDesdeListaNombreTemplate(Ext.Net.GridHeaderContainer columnModel, IList lista, HttpResponse response, [Optional, DefaultParameterValue("")] string ListaCamposExcluidos, string nombreFichero, string lenguaje)
    {
        string strHtml = "";

        if (lista != null)
        {
            try
            {
                strHtml += "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                strHtml += "<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">";
                strHtml += "<head>";
                strHtml += "<title></title>";
                strHtml += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />";
                strHtml += "</head>";
                strHtml += "<body>";
                strHtml += "<p>";
                strHtml += "<table border=\"1px\">";
                strHtml += EscribeCabeceraTemplate(columnModel, ListaCamposExcluidos, lenguaje);
                foreach (object nodo in lista)
                {
                    strHtml += EscribeLineaTemplate(nodo, columnModel, ListaCamposExcluidos);
                }
                strHtml += " </table>";
                strHtml += "</p>";
                strHtml += " </body>";
                strHtml += "</html>";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        response.Clear();
        response.ContentType = Comun.GetMimeType(Comun.DOCUMENTO_EXTENSION_EXCEL_SIN_P);
        response.Charset = "";

        if (Path.GetExtension(nombreFichero) != String.Empty)
        {
            response.AddHeader("Content-Disposition", "attachment; filename=" + nombreFichero);
        }
        else
        {
            response.AddHeader("Content-Disposition", "attachment; filename=" + nombreFichero + Comun.DOCUMENTO_EXTENSION_EXCEL);
        }

        response.AddHeader("Content-Length", strHtml.Length.ToString());
        response.Write(strHtml);
        response.Flush();
        response.SuppressContent = true;

        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    private static string EscribeCabeceraTemplate(Ext.Net.GridHeaderContainer colmodel, string CamposExcluidos, string lenguaje)
    {
        StringBuilder html = new StringBuilder();

        CultureInfo culture = CultureInfo.GetCultureInfo(lenguaje);
        string columnaNoTraducida = HttpContext.GetGlobalResourceObject("Comun", strCOLUMNA_NO_TRADUCIDA, culture).ToString();

        html.Append("<tr style=\"color: black; font:Arial, Helvetica, sans-serif\">");
        for (int i = 0; i < colmodel.Columns.Count; i++)
        {
            if (colmodel.Columns[i] is Ext.Net.Column)
            {
                Column col = (Column)(colmodel.Columns[i]);
                html.Append(string.Format("<th style=\"font-weight: bold; font-size: 13px;\">{0}</th>", !string.IsNullOrEmpty(col.Text) ? col.Text : ((!string.IsNullOrEmpty(col.ToolTip)) ? col.ToolTip : columnaNoTraducida).ToString()));
            }
            else if (colmodel.Columns[i] is Ext.Net.NumberColumn)
            {
                NumberColumn col = (NumberColumn)(colmodel.Columns[i]);

                if (!CamposExcluidos.Contains(col.DataIndex))
                {
                    html.Append(string.Format("<th style=\"font-weight: bold; font-size: 13px;\">{0}</th>", !string.IsNullOrEmpty(col.Text) ? col.Text : ((!string.IsNullOrEmpty(col.ToolTip)) ? col.ToolTip : columnaNoTraducida).ToString()));
                }
            }
            else if (colmodel.Columns[i] is Ext.Net.DateColumn)
            {
                DateColumn col = (DateColumn)(colmodel.Columns[i]);

                if (!CamposExcluidos.Contains(col.DataIndex))
                {
                    html.Append(string.Format("<th style=\"font-weight: bold; font-size: 13px;\">{0}</th>", !string.IsNullOrEmpty(col.Text) ? col.Text : ((!string.IsNullOrEmpty(col.ToolTip)) ? col.ToolTip : columnaNoTraducida).ToString()));
                }
            }
            else if (colmodel.Columns[i] is Ext.Net.TreeColumn)
            {
                TreeColumn col = (TreeColumn)(colmodel.Columns[i]);

                if (!CamposExcluidos.Contains(col.DataIndex))
                {
                    html.Append(string.Format("<th style=\"font-weight: bold; font-size: 13px;\">{0}</th>", !string.IsNullOrEmpty(col.Text) ? col.Text : ((!string.IsNullOrEmpty(col.ToolTip)) ? col.ToolTip : columnaNoTraducida).ToString()));
                }
            }
        }
        html.Append("</tr>");
        return (html.ToString());
    }

    private static string EscribeLineaTemplate(object nodo, Ext.Net.GridHeaderContainer colmodel, string CamposExcluidos)
    {
        StringBuilder html = new StringBuilder();
        string valor = "";
        object elem = null;
        string tipoelem = null;
        html.Append("<tr>");

        for (int i = 0; i < colmodel.Columns.Count; i++)
        {
            if (colmodel.Columns[i] is Ext.Net.Column)
            {
                Column col = (Column)(colmodel.Columns[i]);

                valor = "";
                try
                {
                    Type tipo = nodo.GetType();
                    System.Reflection.PropertyInfo propiedad = tipo.GetProperty(col.DataIndex);

                    if (propiedad != null)
                    {
                        elem = propiedad.GetValue(nodo, null);
                        if (elem != null)
                        {
                            tipoelem = elem.GetType().ToString();
                            switch (tipoelem)
                            {
                                case "System.String":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    break;
                                case "Integer":
                                    if (elem != null)
                                    {
                                        valor = "&nbsp;" + elem;
                                    }
                                    else
                                    {
                                        valor = "";
                                    }
                                    break;
                                case "System.DateTime":
                                    if (elem != null)
                                    {
                                        valor = ((DateTime)(elem)).ToString(Comun.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        valor = "";
                                    }
                                    break;
                                case "System.Decimal":
                                case "System.Double":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    else
                                    {
                                        valor = "0";
                                    }
                                    break;
                                case "System.Int32":
                                case "System.Int64":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    else
                                    {
                                        valor = "0";
                                    }
                                    break;
                                case "System.Boolean":
                                    if ((Boolean)elem == true)
                                    {
                                        //valor = Resources.Comun.strSi;
                                        valor = "TRUE";
                                    }
                                    else
                                    {
                                        //valor = Resources.Comun.strNo;
                                        valor = "FALSE";
                                    }
                                    break;
                                case "System.TimeSpan":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    break;
                                default:
                                    valor = "";

                                    break;
                            }
                        }
                        else
                        {
                            System.Console.Write("");
                            valor = "";
                        }
                    }
                    else if (tipo.Name == "JsonObject")
                    {
                        if (((JsonObject)nodo)[col.DataIndex] != null)
                        {
                            valor = ((JsonObject)nodo)[col.DataIndex].ToString();
                        }
                        else
                        {
                            valor = "";
                        }
                    }
                    else if (tipo.Name == "JObject")
                    {
                        if (((JObject)nodo)[col.DataIndex] != null)
                        {
                            valor = ((JObject)nodo)[col.DataIndex].ToString();
                        }
                        else
                        {
                            valor = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                // remove invalid escape characters
                valor.Replace("\\", "/");

                // replace any separator chars to something safe
                if (valor.Contains(Comun.EXCEL_SEP_CARACTER))
                    valor = valor.Replace(Comun.EXCEL_SEP_CARACTER, Comun.EXCEL_SEP_CARACTER_SAFE_REPLACE);

                valor = HttpUtility.HtmlEncode(valor);

                html.Append(string.Format("<td style=\"font-size:12px\">{0}</td>", valor));
            }
            else if (colmodel.Columns[i] is Ext.Net.NumberColumn)
            {
                NumberColumn col = (NumberColumn)(colmodel.Columns[i]);

                valor = "";
                try
                {
                    Type tipo = nodo.GetType();
                    System.Reflection.PropertyInfo propiedad = tipo.GetProperty(col.DataIndex);

                    elem = propiedad.GetValue(nodo, null);
                    if (elem != null)
                    {
                        tipoelem = elem.GetType().ToString();
                        switch (tipoelem)
                        {
                            case "System.String":
                                if (elem != null)
                                {
                                    valor = elem.ToString();
                                }
                                break;
                            case "Integer":
                                if (elem != null)
                                {
                                    valor = "&nbsp;" + elem;
                                }
                                else
                                {
                                    valor = "0";
                                }
                                break;
                            case "System.DateTime":
                                if (elem != null)
                                {
                                    valor = ((DateTime)(elem)).ToString(Comun.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    valor = "";
                                }
                                break;
                            case "System.Decimal":
                            case "System.Double":
                                if (elem != null)
                                {
                                    valor = elem.ToString();
                                }
                                else
                                {
                                    valor = "0";
                                }
                                break;
                            case "System.Int32":
                            case "System.Int64":
                                if (elem != null)
                                {
                                    valor = elem.ToString();
                                }
                                else
                                {
                                    valor = "0";
                                }
                                break;
                            case "System.Boolean":
                                if ((Boolean)elem == true)
                                {
                                    //valor = Resources.Comun.strSi;
                                    valor = "TRUE";
                                }
                                else
                                {
                                    //valor = Resources.Comun.strNo;
                                    valor = "FALSE";
                                }
                                break;
                            case "System.TimeSpan":
                                if (elem != null)
                                {
                                    valor = elem.ToString();
                                }
                                break;
                            default:
                                valor = "";

                                break;
                        }
                    }
                    else
                    {
                        System.Console.Write("");
                        valor = "";
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                // remove invalid escape characters
                valor.Replace("\\", "/");

                // replace any separator chars to something safe
                if (valor.Contains(Comun.EXCEL_SEP_CARACTER))
                    valor = valor.Replace(Comun.EXCEL_SEP_CARACTER, Comun.EXCEL_SEP_CARACTER_SAFE_REPLACE);

                valor = HttpUtility.HtmlEncode(valor);

                html.Append(string.Format("<td style=\"font-size:12px\">{0}</td>", valor));

            }
            else if (colmodel.Columns[i] is Ext.Net.DateColumn)
            {
                DateColumn col = (DateColumn)(colmodel.Columns[i]);

                valor = "";
                try
                {
                    Type tipo = nodo.GetType();
                    System.Reflection.PropertyInfo propiedad = tipo.GetProperty(col.DataIndex);

                    elem = propiedad.GetValue(nodo, null);
                    if (elem != null)
                    {
                        tipoelem = elem.GetType().ToString();
                        switch (tipoelem)
                        {
                            case "System.String":
                                if (elem != null)
                                {
                                    valor = elem.ToString();
                                }
                                break;
                            case "Integer":
                                if (elem != null)
                                {
                                    valor = "&nbsp;" + elem;
                                }
                                else
                                {
                                    valor = "";
                                }
                                break;
                            case "System.DateTime":
                                if (elem != null)
                                {
                                    valor = ((DateTime)(elem)).ToString(Comun.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    valor = "";
                                }
                                break;
                            case "System.Decimal":
                            case "System.Double":
                                if (elem != null)
                                {
                                    valor = elem.ToString();
                                }
                                else
                                {
                                    valor = "0";
                                }
                                break;
                            case "System.Int32":
                            case "System.Int64":
                                if (elem != null)
                                {
                                    valor = elem.ToString();
                                }
                                else
                                {
                                    valor = "0";
                                }
                                break;
                            case "System.Boolean":
                                if ((Boolean)elem == true)
                                {
                                    //valor = Resources.Comun.strSi;
                                    valor = "TRUE";
                                }
                                else
                                {
                                    //valor = Resources.Comun.strNo;
                                    valor = "FALSE";
                                }
                                break;
                            case "System.TimeSpan":
                                if (elem != null)
                                {
                                    valor = elem.ToString();
                                }
                                break;
                            default:
                                valor = "";

                                break;
                        }
                    }
                    else
                    {
                        System.Console.Write("");
                        valor = "";
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                // remove invalid escape characters
                valor.Replace("\\", "/");

                // replace any separator chars to something safe
                if (valor.Contains(Comun.EXCEL_SEP_CARACTER))
                    valor = valor.Replace(Comun.EXCEL_SEP_CARACTER, Comun.EXCEL_SEP_CARACTER_SAFE_REPLACE);

                valor = HttpUtility.HtmlEncode(valor);

                html.Append(string.Format("<td style=\"font-size:12px\">{0}</td>", valor));
            }
            else if (colmodel.Columns[i] is Ext.Net.TreeColumn)
            {
                TreeColumn col = (TreeColumn)(colmodel.Columns[i]);

                valor = "";
                try
                {
                    Type tipo = nodo.GetType();
                    System.Reflection.PropertyInfo propiedad = tipo.GetProperty(col.DataIndex);

                    if (propiedad != null)
                    {
                        elem = propiedad.GetValue(nodo, null);
                        if (elem != null)
                        {
                            tipoelem = elem.GetType().ToString();
                            switch (tipoelem)
                            {
                                case "System.String":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    break;
                                case "Integer":
                                    if (elem != null)
                                    {
                                        valor = "&nbsp;" + elem;
                                    }
                                    else
                                    {
                                        valor = "";
                                    }
                                    break;
                                case "System.DateTime":
                                    if (elem != null)
                                    {
                                        valor = ((DateTime)(elem)).ToString(Comun.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        valor = "";
                                    }
                                    break;
                                case "System.Decimal":
                                case "System.Double":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    else
                                    {
                                        valor = "0";
                                    }
                                    break;
                                case "System.Int32":
                                case "System.Int64":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    else
                                    {
                                        valor = "0";
                                    }
                                    break;
                                case "System.Boolean":
                                    if ((Boolean)elem == true)
                                    {
                                        //valor = Resources.Comun.strSi;
                                        valor = "TRUE";
                                    }
                                    else
                                    {
                                        //valor = Resources.Comun.strNo;
                                        valor = "FALSE";
                                    }
                                    break;
                                case "System.TimeSpan":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    break;
                                default:
                                    valor = "";

                                    break;
                            }
                        }
                        else
                        {
                            System.Console.Write("");
                            valor = "";
                        }
                    }
                    else if (tipo.Name == "JsonObject")
                    {
                        valor = ((JsonObject)nodo)[col.DataIndex].ToString();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                // remove invalid escape characters
                valor.Replace("\\", "/");

                // replace any separator chars to something safe
                if (valor.Contains(Comun.EXCEL_SEP_CARACTER))
                    valor = valor.Replace(Comun.EXCEL_SEP_CARACTER, Comun.EXCEL_SEP_CARACTER_SAFE_REPLACE);

                valor = HttpUtility.HtmlEncode(valor);

                html.Append(string.Format("<td style=\"font-size:12px\">{0}</td>", valor));
            }
        }

        html.Append("</tr>");
        return (html.ToString());
    }

    public static void ExportacionDesdeListaNombreDinamico(Ext.Net.GridHeaderContainer columnModel, IList lista, HttpResponse response, [Optional, DefaultParameterValue("")] string ListaCamposExcluidos, string nombreFichero)
    {
        string strHtml = "";

        if (lista != null)
        {
            try
            {
                strHtml += "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                strHtml += "<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">";
                strHtml += "<head>";
                strHtml += "<title></title>";
                strHtml += "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />";
                strHtml += "</head>";
                strHtml += "<body>";
                strHtml += "<p>";
                strHtml += "<table border=\"1px\">";
                strHtml += EscribeCabeceraDinamico(columnModel, ListaCamposExcluidos);
                foreach (object nodo in lista)
                    strHtml += EscribeLineaDinamico(nodo, columnModel, ListaCamposExcluidos);

                strHtml += " </table>";
                strHtml += "</p>";
                strHtml += " </body>";
                strHtml += "</html>";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        response.Clear();
        response.ContentType = Comun.GetMimeType(Comun.DOCUMENTO_EXTENSION_EXCEL_SIN_P);
        response.Charset = "";

        if (Path.GetExtension(nombreFichero) != String.Empty)
        {
            response.AddHeader("Content-Disposition", "attachment; filename=" + nombreFichero);
        }
        else
        {
            response.AddHeader("Content-Disposition", "attachment; filename=" + nombreFichero + Comun.DOCUMENTO_EXTENSION_EXCEL);
        }

        response.AddHeader("Content-Length", strHtml.Length.ToString());
        response.Write(strHtml);
        response.Flush();
        response.SuppressContent = true;

        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    private static string EscribeCabeceraDinamico(Ext.Net.GridHeaderContainer colmodel, string CamposExcluidos)
    {
        StringBuilder html = new StringBuilder();

        html.Append("<tr style=\"color: black; font:Arial, Helvetica, sans-serif\">");
        for (int i = 0; i < colmodel.Columns.Count; i++)
        {
            if (colmodel.Columns[i] is Ext.Net.Column)
            {
                Column col = (Column)(colmodel.Columns[i]);
                if (!col.Hidden)
                {
                    html.Append(string.Format("<th style=\"font-weight: bold; font-size: 13px;\">{0}</th>", col.ID));
                }
            }
            else if (colmodel.Columns[i] is Ext.Net.NumberColumn)
            {
                NumberColumn col = (NumberColumn)(colmodel.Columns[i]);
                if (!col.Hidden)
                {
                    html.Append(string.Format("<th style=\"font-weight: bold; font-size: 13px;\">{0}</th>", col.ID));

                }
            }
            else if (colmodel.Columns[i] is Ext.Net.DateColumn)
            {
                DateColumn col = (DateColumn)(colmodel.Columns[i]);
                if (!col.Hidden)
                {
                    html.Append(string.Format("<th style=\"font-weight: bold; font-size: 13px;\">{0}</th>", col.ID));
                }
            }
            else if (colmodel.Columns[i] is Ext.Net.TreeColumn)
            {
                TreeColumn col = (TreeColumn)(colmodel.Columns[i]);
                if (!col.Hidden)
                {
                    html.Append(string.Format("<th style=\"font-weight: bold; font-size: 13px;\">{0}</th>", col.ID));
                }
            }
        }
        html.Append("</tr>");
        return (html.ToString());
    }

    private static string EscribeLineaDinamico(object nodo, Ext.Net.GridHeaderContainer colmodel, string CamposExcluidos)
    {
        StringBuilder html = new StringBuilder();
        string valor = "";
        object elem = null;
        string tipoelem = null;
        html.Append("<tr>");

        for (int i = 0; i < colmodel.Columns.Count; i++)
        {
            if (colmodel.Columns[i] is Ext.Net.Column)
            {
                Column col = (Column)(colmodel.Columns[i]);

                if (!col.Hidden)
                {
                    valor = "";
                    try
                    {
                        Type tipo = nodo.GetType();
                        System.Reflection.PropertyInfo propiedad = tipo.GetProperty(col.DataIndex);

                        if (propiedad != null)
                        {
                            elem = propiedad.GetValue(nodo, null);
                            if (elem != null)
                            {
                                tipoelem = elem.GetType().ToString();
                                switch (tipoelem)
                                {
                                    case "System.String":
                                        if (elem != null)
                                        {
                                            valor = elem.ToString();
                                        }
                                        break;
                                    case "Integer":
                                        if (elem != null)
                                        {
                                            valor = "&nbsp;" + elem;
                                        }
                                        else
                                        {
                                            valor = "";
                                        }
                                        break;
                                    case "System.DateTime":
                                        if (elem != null)
                                        {
                                            valor = ((DateTime)elem).ToShortDateString();
                                        }
                                        else
                                        {
                                            valor = "";
                                        }
                                        break;
                                    case "System.Decimal":
                                    case "System.Double":
                                        if (elem != null)
                                        {
                                            valor = elem.ToString();
                                        }
                                        else
                                        {
                                            valor = "0";
                                        }
                                        break;
                                    case "System.Int32":
                                    case "System.Int64":
                                        if (elem != null)
                                        {
                                            valor = elem.ToString();
                                        }
                                        else
                                        {
                                            valor = "0";
                                        }
                                        break;
                                    case "System.Boolean":
                                        if ((Boolean)elem == true)
                                        {
                                            //valor = Resources.Comun.strSi;
                                            valor = "TRUE";
                                        }
                                        else
                                        {
                                            //valor = Resources.Comun.strNo;
                                            valor = "FALSE";
                                        }
                                        break;
                                    case "System.TimeSpan":
                                        if (elem != null)
                                        {
                                            valor = elem.ToString();
                                        }
                                        break;
                                    default:
                                        valor = "";

                                        break;
                                }
                            }
                            else
                            {
                                System.Console.Write("");
                                valor = "";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }

                    // remove invalid escape characters
                    valor.Replace("\\", "/");

                    // replace any separator chars to something safe
                    if (valor.Contains(Comun.EXCEL_SEP_CARACTER))
                        valor = valor.Replace(Comun.EXCEL_SEP_CARACTER, Comun.EXCEL_SEP_CARACTER_SAFE_REPLACE);

                    valor = HttpUtility.HtmlEncode(valor);

                    html.Append(string.Format("<td style=\"font-size:12px\">{0}</td>", valor));
                }
            }
            else if (colmodel.Columns[i] is Ext.Net.NumberColumn)
            {
                NumberColumn col = (NumberColumn)(colmodel.Columns[i]);

                if (!col.Hidden)
                {
                    valor = "";
                    try
                    {
                        Type tipo = nodo.GetType();
                        System.Reflection.PropertyInfo propiedad = tipo.GetProperty(col.DataIndex);

                        elem = propiedad.GetValue(nodo, null);
                        if (elem != null)
                        {
                            tipoelem = elem.GetType().ToString();
                            switch (tipoelem)
                            {
                                case "System.String":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    break;
                                case "Integer":
                                    if (elem != null)
                                    {
                                        valor = "&nbsp;" + elem;
                                    }
                                    else
                                    {
                                        valor = "0";
                                    }
                                    break;
                                case "System.DateTime":
                                    if (elem != null)
                                    {
                                        valor = ((DateTime)elem).ToShortDateString();
                                    }
                                    else
                                    {
                                        valor = "";
                                    }
                                    break;
                                case "System.Decimal":
                                case "System.Double":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    else
                                    {
                                        valor = "0";
                                    }
                                    break;
                                case "System.Int32":
                                case "System.Int64":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    else
                                    {
                                        valor = "0";
                                    }
                                    break;
                                case "System.Boolean":
                                    if ((Boolean)elem == true)
                                    {
                                        //valor = Resources.Comun.strSi;
                                        valor = "TRUE";
                                    }
                                    else
                                    {
                                        //valor = Resources.Comun.strNo;
                                        valor = "FALSE";
                                    }
                                    break;
                                case "System.TimeSpan":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    break;
                                default:
                                    valor = "";

                                    break;
                            }
                        }
                        else
                        {
                            System.Console.Write("");
                            valor = "";
                        }

                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }

                    // remove invalid escape characters
                    valor.Replace("\\", "/");

                    // replace any separator chars to something safe
                    if (valor.Contains(Comun.EXCEL_SEP_CARACTER))
                        valor = valor.Replace(Comun.EXCEL_SEP_CARACTER, Comun.EXCEL_SEP_CARACTER_SAFE_REPLACE);

                    valor = HttpUtility.HtmlEncode(valor);

                    html.Append(string.Format("<td style=\"font-size:12px\">{0}</td>", valor));
                }
            }
            else if (colmodel.Columns[i] is Ext.Net.DateColumn)
            {
                DateColumn col = (DateColumn)(colmodel.Columns[i]);

                if (!col.Hidden)
                {
                    valor = "";
                    try
                    {
                        Type tipo = nodo.GetType();
                        System.Reflection.PropertyInfo propiedad = tipo.GetProperty(col.DataIndex);

                        elem = propiedad.GetValue(nodo, null);
                        if (elem != null)
                        {
                            tipoelem = elem.GetType().ToString();
                            switch (tipoelem)
                            {
                                case "System.String":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    break;
                                case "Integer":
                                    if (elem != null)
                                    {
                                        valor = "&nbsp;" + elem;
                                    }
                                    else
                                    {
                                        valor = "";
                                    }
                                    break;
                                case "System.DateTime":
                                    if (elem != null)
                                    {
                                        valor = ((DateTime)elem).ToShortDateString();
                                    }
                                    else
                                    {
                                        valor = "";
                                    }
                                    break;
                                case "System.Decimal":
                                case "System.Double":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    else
                                    {
                                        valor = "0";
                                    }
                                    break;
                                case "System.Int32":
                                case "System.Int64":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    else
                                    {
                                        valor = "0";
                                    }
                                    break;
                                case "System.Boolean":
                                    if ((Boolean)elem == true)
                                    {
                                        //valor = Resources.Comun.strSi;
                                        valor = "TRUE";
                                    }
                                    else
                                    {
                                        //valor = Resources.Comun.strNo;
                                        valor = "FALSE";
                                    }
                                    break;
                                case "System.TimeSpan":
                                    if (elem != null)
                                    {
                                        valor = elem.ToString();
                                    }
                                    break;
                                default:
                                    valor = "";

                                    break;
                            }
                        }
                        else
                        {
                            System.Console.Write("");
                            valor = "";
                        }

                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }

                    // remove invalid escape characters
                    valor.Replace("\\", "/");

                    // replace any separator chars to something safe
                    if (valor.Contains(Comun.EXCEL_SEP_CARACTER))
                        valor = valor.Replace(Comun.EXCEL_SEP_CARACTER, Comun.EXCEL_SEP_CARACTER_SAFE_REPLACE);

                    valor = HttpUtility.HtmlEncode(valor);

                    html.Append(string.Format("<td style=\"font-size:12px\">{0}</td>", valor));
                }
            }
        }

        html.Append("</tr>");
        return (html.ToString());
    }

    public static string GetMimeType(string Extension)
    {
        string res = "";
        switch (Extension)
        {
            case "pdf":
            case ".pdf":
                res = "application/pdf";
                break;
            case "doc":
            case ".doc":
                res = "application/msword";
                break;
            case "docx":
            case ".docx":
                res = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                break;
            case "xls":
            case ".xls":
                res = "application/vnd.ms-excel";
                break;
            case "xlsx":
            case ".xlsx":
                res = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                break;
            case "txt":
            case ".txt":
            case ".log":
                res = "text/plain";
                break;
            case "ppt":
            case ".ppt":
                res = "application/vnd.ms-powerpoint";
                break;
            case "pptx":
            case ".pptx":
                res = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                break;
            case "jpg":
            case ".jpg":
            case "jpeg":
            case ".jpeg":
                res = "image/jpeg";
                break;
            case "png":
            case ".png":
                res = "image/png";
                break;
            case "kml":
            case ".kml":
                res = "application/vnd.google-earth.kml+xml";
                break;
            case "kmz":
            case ".kmz":
                res = "application/vnd.google-earth.kmz";
                break;
            default:
                res = "application/octet-stream";
                break;

        }
        return res;
    }



    #region PARALLEL EXPORT

    public static void ExportacionDesdeListaNombreParallelConLocale(GridHeaderContainer colmodel, IList lista, HttpResponse response, [Optional, DefaultParameterValue("")] string ListaCamposExcluidos, string NombreFichero, string currentLocale)
    {
        MemoryStream ms = new MemoryStream();
        StreamWriter sw = new StreamWriter(ms, System.Text.Encoding.Unicode);

        if (lista != null)
        {
            try
            {
                sw.WriteLine(Comun.EXCEL_SEP_HEADER);

                string cabeceraStr = EscribeCabeceraConLocale(colmodel, ListaCamposExcluidos, currentLocale);

                int totalSemiColons = cabeceraStr.Split(Comun.EXCEL_SEP_CARACTER).Length - 1;

                sw.WriteLine(cabeceraStr);

                object key = new object(); // Empty object serves lightest for locks

                Parallel.ForEach<object>(lista.OfType<object>(), (object nodo) =>
                {
                    string val = EscribeLineaConLocale(nodo, colmodel, ListaCamposExcluidos, currentLocale);
                    int totalSemiColonsLine = val.Split(Comun.EXCEL_SEP_CARACTER).Length - 1;

                    lock (key)
                    {
                        if (totalSemiColonsLine != totalSemiColons)
                        {
                            // there is a mismatch between the number of columns and the number of elements in the line just written
                        }
                        sw.WriteLine(val);
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        sw.Flush();

        response.Clear();
        response.ContentType = Comun.GetMimeType(Comun.DOCUMENTO_EXTENSION_EXCEL_SIN_P);
        response.Charset = "";

        // add the extension if there isn't one - otherwise keep the name
        if (Path.GetExtension(NombreFichero) != String.Empty)
            response.AddHeader("content-disposition", "attachment; filename=" + NombreFichero);
        else
            response.AddHeader("content-disposition", "attachment; filename=" + NombreFichero + Comun.DOCUMENTO_EXTENSION_EXCEL);

        response.AddHeader("Content-Length", ms.Length.ToString());

        // done, write it out
        response.BinaryWrite(ms.ToArray());
        //ms.WriteTo(response.OutputStream);
        response.Flush();

        // Prevents any other content from being sent to the browser
        response.SuppressContent = true;

        // Directs the thread to finish, bypassing additional processing
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    /// <summary>
    /// Cabecera de la Grilla para Exportación a Excel
    /// </summary>
    /// <param name="colmodel">Columna</param>
    /// <param name="CamposExcluidos">Lista de Campos </param>
    /// <returns>Cabecera de la Grilla en formato excel</returns>
    private static string EscribeCabeceraConLocale(GridHeaderContainer colmodel, string CamposExcluidos, string currentLocale)
    {
        StringBuilder outStr = new StringBuilder();
        //html.Append("<tr style=\"color: black; font:Arial, Helvetica, sans-serif\">");

        for (int i = 0; i < colmodel.Columns.Count; i++)
        {
            if (colmodel.Columns[i] is Ext.Net.Column || colmodel.Columns[i] is Ext.Net.ComponentColumn)
            {
                if (colmodel.Columns[i] is Ext.Net.ComponentColumn)
                {

                    ComponentColumn col = (ComponentColumn)(colmodel.Columns[i]);
                    if (!col.Hidden)
                    {
                        if (!CamposExcluidos.Contains(col.DataIndex))
                        {
                            // change if the name contains the excel separator
                            if (col.Text.Contains(Comun.EXCEL_SEP_CARACTER))
                                col.Text.Replace(Comun.EXCEL_SEP_CARACTER, EXCEL_SEP_CARACTER_SAFE_REPLACE);

                            outStr.Append(col.Text + ";");
                        }
                    }
                }
                else
                {

                    Column col = (Column)(colmodel.Columns[i]);
                    if (!col.Hidden)
                    {
                        if (!CamposExcluidos.Contains(col.DataIndex))
                        {
                            // change if the name contains the excel separator
                            if (col.Text.Contains(Comun.EXCEL_SEP_CARACTER))
                                col.Text.Replace(Comun.EXCEL_SEP_CARACTER, EXCEL_SEP_CARACTER_SAFE_REPLACE);

                            outStr.Append(col.Text + ";");
                        }
                    }
                }

            }
        }

        return outStr.ToString();
    }

    /// <summary>
    /// Línea en la exportación a excel de cada fila de la grilla
    /// </summary>
    /// <param name="nodo"></param>
    /// <param name="colmodel">Modelo de la cabecera</param>
    /// <param name="CamposExcluidos">Lista de campos excluidos</param>
    /// <returns>Cadena con toda la exportación a excel de todas las líneas</returns>
    private static string EscribeLineaConLocale(object nodo, GridHeaderContainer colmodel, string CamposExcluidos, string currentLocale)
    {
        StringBuilder outStr = new StringBuilder();
        string valor = "";
        object elem = null;
        string tipoelem = null;

        System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(currentLocale);

        System.Resources.ResourceManager rm = Resources.Comun.ResourceManager;

        string langStr = rm.GetString("GEO_LATITUD", new System.Globalization.CultureInfo(currentLocale));
        if (langStr != null)
        {
            langStr = langStr.ToLower();
        }


        string longStr = rm.GetString("GEO_LONGITUD", new System.Globalization.CultureInfo(currentLocale));
        if (longStr != null)
        {
            longStr = longStr.ToLower();
        }


        string geoposicionadoStr = rm.GetString("GEOPOSICIONADO", new System.Globalization.CultureInfo(currentLocale));

        if (geoposicionadoStr != null)
        {
            geoposicionadoStr = geoposicionadoStr.ToLower();
        }

        string ImporteAlquilerStr = rm.GetString("IMPORTEALQUILER", new System.Globalization.CultureInfo(currentLocale));
        if (ImporteAlquilerStr != null)
        {
            ImporteAlquilerStr = ImporteAlquilerStr.ToLower();
        }

        for (int i = 0; i < colmodel.Columns.Count; i++)
        {
            if (colmodel.Columns[i] is Ext.Net.Column || colmodel.Columns[i] is Ext.Net.ComponentColumn)
            {
                if (colmodel.Columns[i] is Ext.Net.ComponentColumn)
                {
                    ComponentColumn col = (ComponentColumn)(colmodel.Columns[i]);

                    if (!col.Hidden)
                    {
                        if (!CamposExcluidos.Contains(col.DataIndex))
                        {
                            valor = "";
                            try
                            {
                                Type tipo = nodo.GetType();

                                if (tipo != null)
                                {
                                    System.Reflection.PropertyInfo propiedad = tipo.GetProperty(col.DataIndex);

                                    if (propiedad != null)
                                    {
                                        elem = propiedad.GetValue(nodo, null);
                                        if (elem != null)
                                        {
                                            tipoelem = elem.GetType().ToString();
                                            switch (tipoelem)
                                            {
                                                case "System.String":
                                                    if (elem != null)
                                                    {
                                                        valor = elem.ToString();
                                                        if (valor.Contains("\t") || valor.Contains("\n") || valor.Contains("\r") || valor.Contains(";"))
                                                        {

                                                            valor = valor.Replace('\t', ' ');
                                                            valor = valor.Replace('\n', ' ');
                                                            valor = valor.Replace('\r', ' ');
                                                            valor = valor.Replace(';', ' ');
                                                        }



                                                    }


                                                    break;
                                                case "Integer":
                                                    if (elem != null)
                                                    {
                                                        valor = elem.ToString();
                                                    }
                                                    else
                                                    {
                                                        valor = "";
                                                    }
                                                    break;
                                                case "System.DateTime":
                                                    if (elem != null)
                                                    {
                                                        DateTime dt = (DateTime)elem;

                                                        valor = TreeCore.Util.GetDateToLocalizedShortString(dt, currentLocale);
                                                    }
                                                    else
                                                    {
                                                        valor = "";
                                                    }
                                                    break;
                                                case "System.Decimal":
                                                case "System.Double":
                                                    if (elem != null)
                                                    {

                                                        if ((col.DataIndex.ToLower() == langStr ||
                                                            col.DataIndex.ToLower() == longStr) && currentLocale != "es-PE")
                                                        {
                                                            // always convert to decimal point first
                                                            double val = (double)elem;

                                                            string dblStr = val.ToString();
                                                            dblStr.Replace(",", ".");

                                                            // now convert it to the locale
                                                            if (ci.NumberFormat.NumberDecimalSeparator != ".")
                                                                dblStr.Replace(".", ci.NumberFormat.NumberDecimalSeparator);

                                                            valor = dblStr;
                                                        }
                                                        else if ((col.DataIndex.ToLower() == langStr ||
                                                            col.DataIndex.ToLower() == longStr || col.DataIndex.ToLower() == geoposicionadoStr || col.DataIndex.ToLower() == ImporteAlquilerStr) && currentLocale == "es-PE")
                                                        {

                                                            // always convert to decimal point first
                                                            double val = (double)elem;

                                                            string dblStr = val.ToString();
                                                            dblStr = dblStr.Replace(".", ",");

                                                            // now convert it to the locale
                                                            if (ci.NumberFormat.NumberDecimalSeparator != ".")
                                                                dblStr.Replace(".", ci.NumberFormat.NumberDecimalSeparator);

                                                            valor = dblStr;
                                                        }

                                                        else
                                                        {
                                                            valor = elem.ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        valor = "0";
                                                    }
                                                    break;
                                                case "System.Int32":
                                                case "System.Int64":
                                                    if (elem != null)
                                                    {
                                                        valor = elem.ToString();
                                                    }
                                                    else
                                                    {
                                                        valor = "0";
                                                    }
                                                    break;
                                                case "System.Boolean":
                                                    if ((Boolean)elem == true)
                                                    {
                                                        valor = Resources.Comun.strSi;
                                                    }
                                                    else
                                                    {
                                                        valor = Resources.Comun.strNo;
                                                    }
                                                    break;
                                                default:
                                                    valor = "";

                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            valor = "";
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }

                            // remove invalid escape characters
                            valor.Replace("\\", "/");

                            // replace any separator chars to something safe
                            if (valor.Contains(Comun.EXCEL_SEP_CARACTER))
                                valor = valor.Replace(Comun.EXCEL_SEP_CARACTER, Comun.EXCEL_SEP_CARACTER_SAFE_REPLACE);

                            outStr.Append(valor + ";");
                        }
                    }
                }
                else
                {

                    Column col = (Column)(colmodel.Columns[i]);

                    if (!col.Hidden)
                    {
                        if (!CamposExcluidos.Contains(col.DataIndex))
                        {
                            valor = "";
                            try
                            {
                                Type tipo = nodo.GetType();

                                if (tipo != null)
                                {
                                    System.Reflection.PropertyInfo propiedad = tipo.GetProperty(col.DataIndex);

                                    if (propiedad != null)
                                    {
                                        elem = propiedad.GetValue(nodo, null);
                                        if (elem != null)
                                        {
                                            tipoelem = elem.GetType().ToString();
                                            switch (tipoelem)
                                            {
                                                case "System.String":
                                                    if (elem != null)
                                                    {
                                                        valor = elem.ToString();
                                                        if (valor.Contains("\t") || valor.Contains("\n") || valor.Contains("\r") || valor.Contains(";"))
                                                        {

                                                            valor = valor.Replace('\t', ' ');
                                                            valor = valor.Replace('\n', ' ');
                                                            valor = valor.Replace('\r', ' ');
                                                            valor = valor.Replace(';', ' ');
                                                        }



                                                    }


                                                    break;
                                                case "Integer":
                                                    if (elem != null)
                                                    {
                                                        valor = elem.ToString();
                                                    }
                                                    else
                                                    {
                                                        valor = "";
                                                    }
                                                    break;
                                                case "System.DateTime":
                                                    if (elem != null)
                                                    {
                                                        DateTime dt = (DateTime)elem;

                                                        valor = TreeCore.Util.GetDateToLocalizedShortString(dt, currentLocale);
                                                    }
                                                    else
                                                    {
                                                        valor = "";
                                                    }
                                                    break;
                                                case "System.Decimal":
                                                case "System.Double":
                                                    if (elem != null)
                                                    {

                                                        if ((col.DataIndex.ToLower() == langStr ||
                                                            col.DataIndex.ToLower() == longStr) && currentLocale != "es-PE")
                                                        {
                                                            // always convert to decimal point first
                                                            double val = (double)elem;

                                                            string dblStr = val.ToString();
                                                            dblStr.Replace(",", ".");

                                                            // now convert it to the locale
                                                            if (ci.NumberFormat.NumberDecimalSeparator != ".")
                                                                dblStr.Replace(".", ci.NumberFormat.NumberDecimalSeparator);

                                                            valor = dblStr;
                                                        }
                                                        else if ((col.DataIndex.ToLower() == langStr ||
                                                            col.DataIndex.ToLower() == longStr || col.DataIndex.ToLower() == geoposicionadoStr || col.DataIndex.ToLower() == ImporteAlquilerStr) && currentLocale == "es-PE")
                                                        {

                                                            // always convert to decimal point first
                                                            double val = (double)elem;

                                                            string dblStr = val.ToString();
                                                            dblStr = dblStr.Replace(".", ",");

                                                            // now convert it to the locale
                                                            if (ci.NumberFormat.NumberDecimalSeparator != ".")
                                                                dblStr.Replace(".", ci.NumberFormat.NumberDecimalSeparator);

                                                            valor = dblStr;
                                                        }

                                                        else
                                                        {
                                                            valor = elem.ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        valor = "0";
                                                    }
                                                    break;
                                                case "System.Int32":
                                                case "System.Int64":
                                                    if (elem != null)
                                                    {
                                                        valor = elem.ToString();
                                                    }
                                                    else
                                                    {
                                                        valor = "0";
                                                    }
                                                    break;
                                                case "System.Boolean":
                                                    if ((Boolean)elem == true)
                                                    {
                                                        valor = Resources.Comun.strSi;
                                                    }
                                                    else
                                                    {
                                                        valor = Resources.Comun.strNo;
                                                    }
                                                    break;
                                                default:
                                                    valor = "";

                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            valor = "";
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }

                            // remove invalid escape characters
                            valor.Replace("\\", "/");

                            // replace any separator chars to something safe
                            if (valor.Contains(Comun.EXCEL_SEP_CARACTER))
                                valor = valor.Replace(Comun.EXCEL_SEP_CARACTER, Comun.EXCEL_SEP_CARACTER_SAFE_REPLACE);

                            outStr.Append(valor + ";");
                        }
                    }
                }



            }
        }

        return outStr.ToString();
    }

    #endregion


    #endregion

    #region SELECCIONABLE

    public static void Seleccionable(Ext.Net.GridPanel grid, Store gridStore, Ext.Net.GridHeaderContainer columnModel, List<string> ignoreList, string currentLocale)
    {
        Dictionary<string, ModelField> dataFields = new Dictionary<string, ModelField>();

        if (gridStore.Model.Count == 1)
        {
            Ext.Net.Model model = gridStore.Model[0];
            ModelFieldCollection fieldsCollection = model.Fields;

            foreach (ModelField f in fieldsCollection)
            {
                if (!dataFields.ContainsKey(f.Name))
                {
                    dataFields.Add(f.Name, f);
                }
            }

            for (int k = 0; k < columnModel.Columns.Count; k = k + 1)
            {
                if (columnModel.Columns[k].DataIndex != null &&
                    !columnModel.Columns[k].DataIndex.Equals("") &&
                    !ignoreList.Contains(columnModel.Columns[k].DataIndex) &&
                    dataFields.ContainsKey(columnModel.Columns[k].DataIndex))
                {
                    ModelField f = dataFields[columnModel.Columns[k].DataIndex];
                    grid.ColumnModel.Columns[k].Editor.Clear();

                    Field item = new Ext.Net.TextField();

                    switch (f.Type)
                    {
                        case ModelFieldType.Date:
                            item = new DateField();
                            DateField df = (DateField)item;
                            df.Editable = false;
                            Ext.Net.DateColumn dc = (Ext.Net.DateColumn)columnModel.Columns[k];

                            if (dc.Format != null && dc.Format.Length > 0)
                                df.Format = dc.Format;
                            break;
                        case ModelFieldType.Float:
                        case ModelFieldType.Int:
                            item = new NumberField();
                            NumberField nf = (NumberField)item;
                            break;
                        case ModelFieldType.Auto:
                        case ModelFieldType.String:
                            break;
                        default:
                            break;
                    }

                    //grid.ColumnModel.Columns[k].SetEditor(item);
                    item.ReadOnly = true;
                    item.CausesValidation = false;
                    grid.ColumnModel.Columns[k].Editor.Add(item);
                }
            }
        }
    }

    #endregion

    #region IDIOMAS

    const string CamposTraducibles = "EmplazamientoEstado,Conflictividad,SavingEmplazamientoEstado";
    public const string Espanol = "es-ES";
    public const string Ingles = "en-US";
    public const string Frances = "fr-FR";
    public const string Italiano = "it-IT";
    public const string Chileno = "es-CL";
    public const string Ecuatoriano = "es-EC";
    public const string Panama = "es-PA";

    public static string AjustarCampo(string sCampo, string sLocale)
    {
        string sSufijo = "es_ES";

        switch (sLocale)
        {
            case Comun.Ingles:
                sSufijo = "enUS";
                break;
            case Comun.Frances:
                sSufijo = "frFR";
                break;
            case Comun.Espanol:
                sSufijo = "esES";
                break;
            case Comun.Italiano:
                sSufijo = "itIT";
                break;
            case Comun.Chileno:
                sSufijo = "esCL";
                break;
            case Comun.Panama:
                sSufijo = "esPA";
                break;
            case Comun.Ecuatoriano:
                sSufijo = "esEC";
                break;
        }

        if (sCampo != "EmplazamientoEstado")
        {
            sCampo = sCampo + "_" + sSufijo;
        }

        return sCampo;
    }

    public static string AjustarFiltro(string sFiltro, string sLocale)
    {
        if (sFiltro != null)
        {
            string[] aux = CamposTraducibles.Split(',');

            foreach (String sDato in aux)
            {
                if (sFiltro.Contains(sDato))
                {
                    sFiltro = sFiltro.Replace(sDato, AjustarCampo(sDato, sLocale));
                }
            }
        }

        return sFiltro;
    }

    #endregion

    #region TIPOS VALORES

    public enum TiposValores
    {
        String = 0,
        Boolean = 1,
        Integer = 2,
        Date = 3
    }

    #endregion

    #region TIPOS PROPIEDADES

    public enum TiposPropiedades
    {
        AllowBlank = 1,
        AllowDecimals = 2,
        AllowExponential = 3,
        CausesValidation = 4,
        Checked = 5,
        DecimalPrecision = 6,
        DecimalSeparator = 7,
        Disabled = 8,
        Editable = 9,
        EmptyNumber = 10,
        EmptyText = 11,
        Enabled = 12,
        EnforceMaxLength = 13,
        Hidden = 14,
        MaxDate = 15,
        MaxLength = 16,
        MaxLengthText = 17,
        MinDate = 18,
        MinLenght = 19,
        MinLenghtText = 20,
        Format = 21,
        Text = 22,
        Value = 23,
        Visible = 24,
        MinValue = 25,
        MaxValue = 26,
        Regex = 27,
        RegexText = 28,
        ToolTips = 29,
    }

    #endregion

    #region FUENTES DE DATOS

    /// <summary>
    /// FUENTES DE DATOS
    /// </summary>

    public const string REPORTING_SOURCE_TREE = "TREE";
    public const string REPORTING_SOURCE_SQL_SERVER = "SQLSRV";
    public const string REPORTING_SOURCE_EXCEL = "EXCEL";
    public const string REPORTING_SOURCE_ORACLE = "ORACLE";
    public const string REPORTING_SOURCE_MYSQL = "MYSQL";
    public const string REPORTING_SOURCE_DB2 = "DB2";
    public const string REPORTING_SOURCE_POSTGRE = "POSTGRE";
    public const string REPORTING_SOURCE_CSV = "CSV";

    /// <summary>
    /// FUENTES DE DATOS
    /// </summary>
    /// 
    public const string REPORTING_SQL_DIRECCION_ORDENACION_DESC = "DESC";
    public const string REPORTING_SQL_DIRECCION_ORDENACION_ASC = "ASC";

    #endregion

    #region LISTAS CAMPOS VALIDOS HISTORICOS

    public static JObject ObjectToJSON(Object oJson, string nombre)
    {

        JObject historicoJSON = new JObject();
        var listaCamposValidos = Comun.GetCamposValidosPorNombre(nombre);

        try
        {
            if (listaCamposValidos != null)
            {
                if (oJson != null)
                {
                    foreach (System.Reflection.PropertyInfo propiedad in oJson.GetType().GetProperties())
                    {

                        if (listaCamposValidos.Contains(propiedad.Name) && !historicoJSON.ContainsKey(propiedad.Name))
                        {

                            string temp = "";
                            if (propiedad.GetValue(oJson, null) != null)
                            {
                                if (propiedad.PropertyType == typeof(DateTime) || propiedad.PropertyType == typeof(System.Nullable<System.DateTime>))
                                {
                                    temp = Comun.DateTimeToString((DateTime)propiedad.GetValue(oJson, null));
                                }
                                else
                                {
                                    temp = propiedad.GetValue(oJson, null).ToString();
                                }
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
        }
        catch (Exception ex)
        {

            historicoJSON = null;
            log.Error(ex.Message);
        }

        return historicoJSON;
    }

    #region ObjectToJSON
    public static JObject ObjectToJSON(Object oJson)
    {

        JObject objJSON = new JObject();

        try
        {
            if (oJson != null)
            {
                foreach (System.Reflection.PropertyInfo propiedad in oJson.GetType().GetProperties())
                {
                    if (!objJSON.ContainsKey(propiedad.Name))
                    {

                        string temp = "";
                        if (propiedad.GetValue(oJson, null) != null)
                        {
                            if (propiedad.PropertyType == typeof(DateTime) || propiedad.PropertyType == typeof(System.Nullable<System.DateTime>))
                            {
                                temp = Comun.DateTimeToString((DateTime)propiedad.GetValue(oJson, null));
                            }
                            else
                            {
                                temp = propiedad.GetValue(oJson, null).ToString();
                            }
                        }

                        objJSON.Add(propiedad.Name, temp);
                    }
                }
            }
        }
        catch (Exception ex)
        {

            objJSON = null;
            log.Error(ex.Message);
        }

        return objJSON;
    }
    #endregion

    public static List<string> GetCamposValidosPorNombre(string nombre)
    {
        List<string> listaValidos;

        switch (nombre)
        {

            case "Emplazamientos":
                listaValidos = CamposValidosEmplazamientos;
                break;
            case "InventarioElementos":
                listaValidos = CamposValidosInventario;
                break;
            case "Documentos":
                listaValidos = CamposValidosDocumentos;
                break;
            case "CoreProductCatalogServicios":
                listaValidos = CamposValidosProductCatalog;
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

    public static readonly List<string> CamposValidosDocumentos = new List<string>
    {

        // PRINCIPAL
        "Identificador",
        "NombreObjeto",
        "Documento",
        "Extension",
        "Creador",
        "FechaDocumento",
        "NombreEstado",
        "Version",
        "Tamano",
        //"Slug",
        "DocumentTipo",
        "Observaciones",
        "Activo"
    };

    public static readonly List<string> CamposValidosInventario = new List<string>
    {

        "NumeroInventario",
        "Nombre"/*,
        "FechaCreacion",
        "FechaAlta"*/

        // LOS SIGUIENTES CAMPOS SE METEN COMO ATRIBUTOS DINAMICOS
        // PORQUE AL CREAR EL HISTORICO SE GUARDAN COMO ID:

        // "InventarioElementoPadre"
        // "Plantilla"
        // "Estado"
    };

    public static readonly List<string> CamposValidosProductCatalog = new List<string>
    {
        "Precio",
    };

    #endregion

    #region EXPORTACION

    public static void ExportarModeloDatosDinamicoFilas(string archivo, string tab, List<List<string>> filas, long cont)
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
    public static void ExportarModeloDatosDinamicoColumnas(string archivo, string tab, List<List<string>> Filas, long cont)
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


    #endregion

    #region NOMBRES MODULOS

    public static Dictionary<string, string> NombresModulos = new Dictionary<string, string>
    {
        ["strCalidad"] = "ModCalidad",
        ["strDocumental"] = "ModDocumental",
        ["strExportarImportar"] = "ModExportarImportar",
        //["strGlobal"] = "ModGlobal", // Si se incluye esta linea puede no funcionar el menú de favoritos para los modulos de Global
        ["strInventario"] = "ModInventario",
        ["strMantenimiento"] = "ModMantenimiento",
        ["strMonitoring"] = "ModMonitoring"

    };

    #endregion

    #region Acceso no permitido
    public const string PaginaNoEncontrada = "PaginaNoEncontrada.aspx";
    public const string AccesoNoPermitido = "AccesoNoPermitido.aspx";

    public static void AccesoNoPermitidoAModulos(HttpResponse Response, Usuarios usuario, string modulo)
    {
        UsuariosRolesController cUsuariosRoles = new UsuariosRolesController();

        if (usuario.ClienteID != null)
        {
            List<string> listaModulos = cUsuariosRoles.modulosByRolesAsignados(usuario.UsuarioID);
            if (!listaModulos.Contains(modulo))
            {
                Response.Redirect("~/" + Comun.AccesoNoPermitido);
            }
        }
    }

    #endregion

    #region TIPOS VINCULACIONES

    public enum TiposVinculaciones
    {
        Rel_1_1 = 1,
        Rel_1_N = 2,
        Rel_N_1 = 3,
        Rel_N_M = 4
    }

    #endregion

    #region FUNCIONALIDADES POR TIPO
    public static bool ComprobarFuncionalidadDescarga(string sPagina, List<Vw_Funcionalidades> listaFuncionalidades, List<TreeCore.Data.Modulos> listaModulos = null)
    {
        bool existe = false;
        List<Vw_Funcionalidades> listaFuncionalidadesDescarga;
        TreeCore.Data.Modulos oMod;
        try
        {
            if (listaModulos != null)
            {
                oMod = (from c in listaModulos where c.Pagina == sPagina.ToLower() select c).FirstOrDefault();
                if (oMod != null && oMod.ModuloPadreID.HasValue)
                {
                    listaFuncionalidadesDescarga = (from c in listaFuncionalidades
                                                    where
                     c.ModuloID == oMod.ModuloPadreID && c.Exportar.HasValue && c.Exportar == true
                                                    select c).ToList();
                    if (listaFuncionalidadesDescarga != null && listaFuncionalidadesDescarga.Count > 0)
                    {
                        existe = true;
                    }
                }
                else
                {
                    listaFuncionalidadesDescarga = (from c in listaFuncionalidades
                                                    where
                     c.Pagina == sPagina.ToLower() && c.Exportar.HasValue && c.Exportar == true
                                                    select c).ToList();
                    if (listaFuncionalidadesDescarga != null && listaFuncionalidadesDescarga.Count > 0)
                    {
                        existe = true;
                    }
                }
            }
            else
            {
                listaFuncionalidadesDescarga = (from c in listaFuncionalidades
                                                where
                 c.Pagina == sPagina.ToLower() && c.Exportar.HasValue && c.Exportar == true
                                                select c).ToList();
                if (listaFuncionalidadesDescarga != null && listaFuncionalidadesDescarga.Count > 0)
                {
                    existe = true;
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            existe = false;
        }
        return existe;
    }
    public static bool ComprobarFuncionalidadTotal(string sPagina, List<Vw_Funcionalidades> listaFuncionalidades, List<TreeCore.Data.Modulos> listaModulos = null)
    {
        bool existe = false;
        List<Vw_Funcionalidades> listaFuncionalidadesDescarga;
        TreeCore.Data.Modulos oMod;
        try
        {
            if (listaModulos != null)
            {
                oMod = (from c in listaModulos where c.Pagina == sPagina.ToLower() select c).FirstOrDefault();
                if (oMod != null && oMod.ModuloPadreID.HasValue)
                {
                    listaFuncionalidadesDescarga = (from c in listaFuncionalidades
                                                    where
                     c.ModuloID == oMod.ModuloPadreID && c.Total.HasValue && c.Total == true
                                                    select c).ToList();
                    if (listaFuncionalidadesDescarga != null && listaFuncionalidadesDescarga.Count > 0)
                    {
                        existe = true;
                    }
                }
                else
                {
                    listaFuncionalidadesDescarga = (from c in listaFuncionalidades
                                                    where
                     c.Pagina == sPagina.ToLower() && c.Total.HasValue && c.Total == true
                                                    select c).ToList();
                    if (listaFuncionalidadesDescarga != null && listaFuncionalidadesDescarga.Count > 0)
                    {
                        existe = true;
                    }
                }
            }
            else
            {
                listaFuncionalidadesDescarga = (from c in listaFuncionalidades
                                                where
                 c.Pagina == sPagina.ToLower() && c.Total.HasValue && c.Total == true
                                                select c).ToList();
                if (listaFuncionalidadesDescarga != null && listaFuncionalidadesDescarga.Count > 0)
                {
                    existe = true;
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            existe = false;
        }
        return existe;
    }
    public static bool ComprobarFuncionalidadSoloLectura(string sPagina, List<Vw_Funcionalidades> listaFuncionalidades, List<TreeCore.Data.Modulos> listaModulos = null)
    {
        bool existe = false;
        List<Vw_Funcionalidades> listaFuncionalidadesDescarga;
        TreeCore.Data.Modulos oMod;
        try
        {
            if (listaModulos != null)
            {
                oMod = (from c in listaModulos where c.Pagina == sPagina.ToLower() select c).FirstOrDefault();
                if (oMod != null && oMod.ModuloPadreID.HasValue)
                {
                    listaFuncionalidadesDescarga = (from c in listaFuncionalidades
                                                    where
                     c.ModuloID == oMod.ModuloPadreID && c.Lectura.HasValue && c.Lectura == true
                                                    select c).ToList();
                    if (listaFuncionalidadesDescarga != null && listaFuncionalidadesDescarga.Count > 0)
                    {
                        existe = true;
                    }
                }
                else
                {
                    listaFuncionalidadesDescarga = (from c in listaFuncionalidades
                                                    where
                     c.Pagina == sPagina.ToLower() && c.Lectura.HasValue && c.Lectura == true
                                                    select c).ToList();
                    if (listaFuncionalidadesDescarga != null && listaFuncionalidadesDescarga.Count > 0)
                    {
                        existe = true;
                    }
                }
            }
            else
            {
                listaFuncionalidadesDescarga = (from c in listaFuncionalidades
                                                where
                 c.Pagina == sPagina.ToLower() && c.Lectura.HasValue && c.Lectura == true
                                                select c).ToList();
                if (listaFuncionalidadesDescarga != null && listaFuncionalidadesDescarga.Count > 0)
                {
                    existe = true;
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            existe = false;
        }
        return existe;
    }

    #endregion

    #region Services

    #endregion
}