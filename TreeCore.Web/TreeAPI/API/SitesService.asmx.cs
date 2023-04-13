using Newtonsoft.Json;
using Sites.API.Mobile.General.SOAP;
using System.Web.Services;
using TreeAPI.API.Mobile.Crypto;
using TreeAPI.API.Mobile.Login;

namespace TreeAPI.API
{
    /// <summary>
    /// Descripción breve de SitesService
    /// </summary>
    [WebService(Namespace = "http://sites.alteda.es/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class SitesService : System.Web.Services.WebService
    {

        #region CONFIG
        private CryptoHandler CryptoHandler = new CryptoHandler();

        public class CountryServer
        {
            public CountryServer(string addr, long p, string code)
            {
                address = addr;
                port = p;
                country_code = code;
            }

            public string address { get; set; }
            public long port { get; set; }
            public string country_code { get; set; }
        }

        public class CountryServerPackage
        {
            public bool RequestResult { get; set; }
            public string RequestResultString { get; set; }
            public long BlockSize { get; set; }
            public string BlockData { get; set; }
        }

        public SitesService()
        {
            GenerateEncryptedServerList();

            /*try
            {
                //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"ima\Telefonica32.png");
                //string currentDir = Environment.CurrentDirectory;
                //string fullpath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            }
            catch (System.Exception ex)
            {
                System.Console.Write(ex);
            }*/
        }

        public void GenerateEncryptedServerList()
        {
            // using this to generate the encrypted country server locations
            CountryServerPackage jsonCountryServerPackage = new CountryServerPackage();
            jsonCountryServerPackage.RequestResult = true;
            jsonCountryServerPackage.RequestResultString = "OK";
            jsonCountryServerPackage.BlockSize = 0;
            jsonCountryServerPackage.BlockData = "";

            CountryServer[] servers = new CountryServer[] {
                new CountryServer( "http://www.spain.com", -1, "es_ES"),
                new CountryServer( "http://www.brasil.com", -1, "pt_BR"),
                new CountryServer( "http://www.ecuador.com", -1, "es_EC"),
                new CountryServer( "http://www.panama.com", -1, "es_PA"),
                new CountryServer( "http://www.chile.com", -1, "es_CL"),
                new CountryServer( "http://www.colombia.com", -1, "es_CO"),
                new CountryServer( "http://www.peru.com", -1, "es_PE"),
                new CountryServer( "http://www.venezuela.com", -1, "es_VE"),
                new CountryServer( "http://www.argentina.com", -1, "es_AR"),
                new CountryServer( "http://www.unitedstates.com", -1, "en_US"),
            };

            // now encrypt the data block
            string rawData = JsonConvert.SerializeObject(servers, Formatting.Indented);
            if (rawData != null &
                rawData.Length > 0)
            {
                AESCrypto.StringEncryption strEnc = CryptoHandler.EncryptString(rawData);

                if (strEnc.encryptedString != null &&
                    strEnc.encryptedString.Length > 0)
                {
                    jsonCountryServerPackage.BlockSize = strEnc.originalByteLength;
                    jsonCountryServerPackage.BlockData = strEnc.encryptedString;
                }
                else
                {
                    jsonCountryServerPackage.RequestResult = false;
                    jsonCountryServerPackage.RequestResultString = "ERROR";
                }
            }

            // output string contains the encrypted json
            // this should be copied to the server and also copied as servers.json on the device in 'assets'
            string jsonOut = JsonConvert.SerializeObject(jsonCountryServerPackage, Formatting.Indented);


            //System.Console.Write(jsonOut);
        }
        #endregion

        #region LOGIN
        /// <summary>
        /// Mobile Login handler para la aplicacion de verificacion de Xamarin
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="locale"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="uniqueDeviceID"></param>
        /// <param name="fcmKey"></param>
        /// <param name="mac"></param>
        /// <returns>JSON login data</returns>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public string MobileLoginV2(LoginRequestSOAP req)
        {
            MobileLoginV2Handler handler = new MobileLoginV2Handler();

            return handler.Process(Session, CryptoHandler, req);
        }

        /// <summary>
        /// Mobile Login handler para la aplicacion de verificacion de Xamarin
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="locale"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="uniqueDeviceID"></param>
        /// <param name="fcmKey"></param>
        /// <param name="mac"></param>
        /// <returns>JSON login data</returns>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public string MobileLoginV3(LoginRequestSOAPV3 req)
        {
            MobileLoginV3Handler handler = new MobileLoginV3Handler();

            return handler.Process(Session, CryptoHandler, req);
        }
        #endregion

        #region DIRECTION API
        /// <summary>
        /// Mobile Get Direccion URL API
        /// </summary>       
        /// <param name="codigoCliente"></param>
        /// <returns>JSON login data</returns>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public string MobileDireccionAPI(MobileDireccionAPIHandler.MobileDireccionAPIRequestSOAP req)
        {
            MobileDireccionAPIHandler handler = new MobileDireccionAPIHandler();

            return handler.Process(Session, CryptoHandler, req);
        } 
        #endregion

        #region METODOS BASURA DE TREE 4

        /// <summary>
        /// Datos de los empalzamientos a validar
        /// </summary>       
        /// <param name="req"></param>
        /// <returns>objects</returns>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public string ModuloEmplazamientosForValidation(ModuloEmplazamientosSOAP req)
        {
            return "";
        }

        /// <summary>
        /// Actualizar el seguimiento segun lo que devuelve la App
        /// </summary>       
        /// <param name="req"></param>
        /// <returns>true o false</returns>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public string ModuloEmplazamientosActualizarSeguimiento(ModuloEmplazamientosActualizarSeguimientoSOAP req)
        {
            return "";
        }

        #endregion

    }
}
