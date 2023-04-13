using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using CapaNegocio;
using TreeCore.Data;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TreeCore.Integraciones.Comarch
{
    public class ComarchConnection
    {
        // Global variables
        private string servidor = null;
        private string usuario = null;
        private string clave = null;
        //private Alteda.Utiles.Logs debug = new Alteda.Utiles.Logs(Properties.Settings.Default.DirectorioLog, "IFRS16", false, true);
        Vw_ToolIntegracionesServiciosMetodosConexiones conexion = null;
        private bool conectado = false;
        static string key = "ABCDEFG54669525PQRSTUVWXYZabcdef852846opqrstuvwxyz";

        #region BASE

        public ComarchConnection(string metodo)
            : base()
        {
            ToolConexionesController cConexion = new ToolConexionesController();
            conexion = GetConexion(metodo);
            if (conexion != null)
            {
                //SetClave(conexion.Clave);
                SetClave(DecryptKey(conexion.Clave));
                SetServidor(conexion.URLCompleta);
                SetUsuario(conexion.Usuario);
                SetConectado(true);
            }
        }

        private Vw_ToolIntegracionesServiciosMetodosConexiones GetConexion(string metodo)
        {
            try
            {
                Vw_ToolIntegracionesServiciosMetodosConexiones conexionLocal = null;
                if (conexion == null)
                {
                    ToolConexionesController cConexion = new ToolConexionesController();
                    conexionLocal = cConexion.GetConexionPorMetodo(metodo);
                }
                else
                {
                    conexionLocal = conexion;
                }
                return conexionLocal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool IsConectado()
        {
            return conectado;
        }

        public void SetConectado(bool conectaLocal)
        {
            conectado = conectaLocal;
        }

        public string GetUsuario()
        {
            return usuario;
        }

        public void SetUsuario(string usuarioLocal)
        {
            usuario = usuarioLocal;
        }

        public string GetClave()
        {
            return clave;
        }

        public void SetClave(string claveLocal)
        {
            clave = claveLocal;
        }

        public string GetServidor()
        {
            return servidor;
        }

        public void SetServidor(string servidorLocal)
        {
            servidor = servidorLocal;
        }

        #endregion

        #region DESENCRIPTAR

        public string DecryptKey(string clave)
        {
            byte[] keyArray;
            //convierte el texto en una secuencia de bytes
            byte[] Array_a_Descifrar =
            Convert.FromBase64String(clave);

            //se llama a las clases que tienen los algoritmos
            //de encriptación se le aplica hashing
            //algoritmo MD5
            MD5CryptoServiceProvider hashmd5 =
            new MD5CryptoServiceProvider();

            keyArray = hashmd5.ComputeHash(
            UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes =
            new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform =
             tdes.CreateDecryptor();
            byte[] resultArray;
            try
            {
                resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);
            }

            catch (Exception)
            {
                resultArray = null;
            }
            tdes.Clear();
            //se regresa en forma de cadena
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        #endregion

        #region CREATE REQUEST

        /// <summary>
        /// Create a soap webrequest to [Url]
        /// </summary>
        /// <returns></returns>
        public HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(GetServidor());
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Headers.Add("SOAPAction", @"http://sap.com/xi/WebService/soap1.1");
            webRequest.KeepAlive = true;
            webRequest.UserAgent = "Apache-HttpClient/4.1.1 (java 1.5)";
            webRequest.Credentials = new System.Net.NetworkCredential(GetUsuario(), GetClave());
            webRequest.Method = "POST";
            return webRequest;
        }

        #endregion

        #region MENSAJES


        #region MESSAGE REGION

        /// <summary>
        /// Creates the xml message from the Comarch object
        /// </summary>
        /// <param name="comarchRegion">Objeto para la region en comarch</param>
        /// <returns>XML Message for calling Comarch</returns>
        public XmlDocument CreateMessageAddressItemRegion(AddressItemRegionRequest comarchRegion)
        {
            // Local variables
            XmlDocument soapEnvelopeXml = null;

            // Local variables
            string sCadena = null;

            #region CREATES THE XML


            // Creates the envelop
            soapEnvelopeXml = new XmlDocument();
            sCadena = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tree=""http://tree.ws.oss.comarch.com/"">" + System.Environment.NewLine;
            sCadena = sCadena + "<soapenv:Header/>" + System.Environment.NewLine;
            sCadena = sCadena + "<soapenv:Body>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:AddressItemRequest>" + System.Environment.NewLine;

            #region CUERPO XML


            #region ITEM REGION

            sCadena = sCadena + "<tree:AddressItemRegionRequest operation = \"" + comarchRegion.OPERATION + "\" abreviationRegion = \"" + comarchRegion.ABBREVIATION + "\">" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:regionName>" + comarchRegion.REGION_NAME + "</tree:regionName>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:mastercommercialclass>" + comarchRegion.MASTER_COMMERCIAL_CLASS + "</tree:mastercommercialclass>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterMinistryArea>" + comarchRegion.MASTER_COMMERCIAL_CLASS + "</tree:masterMinistryArea>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterRegionalTx>" + comarchRegion.MASTER_REGIONAL_TX + "</tree:masterRegionalTx>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterResponsibleTx>" + comarchRegion.MASTER_RESPONSIBLE_TX + "</tree:masterResponsibleTx>" + System.Environment.NewLine;
            sCadena = sCadena + " </tree:AddressItemRegionRequest>" + System.Environment.NewLine;

            #endregion


            #endregion

            sCadena = sCadena + "</tree:AddressItemRequest>" + System.Environment.NewLine;
            sCadena = sCadena + "</soapenv:Body>" + System.Environment.NewLine;
            sCadena = sCadena + "</soapenv:Envelope>" + System.Environment.NewLine;

            // Eliminamos las comas y reemplazamos por puntos
            sCadena = sCadena.Replace(",", ".");

            // Creates the document
            soapEnvelopeXml.LoadXml(sCadena);
            #endregion

            // Returns the result
            return soapEnvelopeXml;
        }

        #endregion

        #region MESSAGE CITY

        /// <summary>
        /// Creates the xml message from the Comarch object
        /// </summary>
        /// <param name="comarchCity">Objeto para la ciudad en comarch</param>
        /// <returns>XML Message for calling Comarch</returns>
        public XmlDocument CreateMessageAddressItemCity(AddressItemCityRequest comarchCity)
        {
            // Local variables
            XmlDocument soapEnvelopeXml = null;

            // Local variables
            string sCadena = null;

            #region CREATES THE XML


            // Creates the envelop
            soapEnvelopeXml = new XmlDocument();
            sCadena = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tree=""http://tree.ws.oss.comarch.com/"">" + System.Environment.NewLine;
            sCadena = sCadena + "<soapenv:Header/>" + System.Environment.NewLine;
            sCadena = sCadena + "<soapenv:Body>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:AddressItemRequest>" + System.Environment.NewLine;

            #region CUERPO XML

            #region ITEM CITY

            sCadena = sCadena + "<tree:AddressItemCityRequest operation = \"" + comarchCity.OPERATION + "\" abbreviationCity = \"" + comarchCity.ABBREVIATION + "\">" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:abbreviationRegion>" + comarchCity.ABBREVIATION_REGION + "</tree:abbreviationRegion>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:cityName>" + comarchCity.CITY_NAME + "</tree:cityName>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterCrc>" + comarchCity.MASTER_CRC_AREA + "</tree:masterCrc>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterCapitalCode>" + comarchCity.MASTER_CAPITAL_CODE + "</tree:masterCapitalCode>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterMCImain>" + comarchCity.MASTER_MCI_MAIN + "</tree:masterMCImain>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterMCIgroup>" + comarchCity.MASTER_MCI_GROUP + "</tree:masterMCIgroup>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:populationProjection>" + comarchCity.POPULATION_PROJECTION + "</tree:populationProjection>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterCityGroup>" + comarchCity.MASTER_MCI_GROUP + "</tree:masterCityGroup>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterCategory617>" + comarchCity.MASTER_CATEGORY_617 + "</tree:masterCategory617>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterscope>" + comarchCity.MASTER_SCOPE + "</tree:masterscope>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:LTECoverage>" + comarchCity.LTE_COVERAGE + "</tree:LTECoverage>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:market>" + comarchCity.MARKET + "</tree:market>" + System.Environment.NewLine;
            sCadena = sCadena + " </tree:AddressItemCityRequest>" + System.Environment.NewLine;

            #endregion

            #endregion

            sCadena = sCadena + "</tree:AddressItemRequest>" + System.Environment.NewLine;
            sCadena = sCadena + "</soapenv:Body>" + System.Environment.NewLine;
            sCadena = sCadena + "</soapenv:Envelope>" + System.Environment.NewLine;

            // Eliminamos las comas y reemplazamos por puntos
            sCadena = sCadena.Replace(",", ".");

            // Creates the document
            soapEnvelopeXml.LoadXml(sCadena);
            #endregion

            // Returns the result
            return soapEnvelopeXml;
        }

        #endregion

        #region MESSAGE DISTRICT

        /// <summary>
        /// Creates the xml message from the Comarch object
        /// </summary>
        /// <param name="comarchDistrict">Objeto para el distrito en comarch</param>
        /// <returns>XML Message for calling Comarch</returns>
        public XmlDocument CreateMessageAddressItemDistrict(AddressItemDistrictRequest comarchDistrict)
        {
            // Local variables
            XmlDocument soapEnvelopeXml = null;

            // Local variables
            string sCadena = null;

            #region CREATES THE XML


            // Creates the envelop
            soapEnvelopeXml = new XmlDocument();
            sCadena = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tree=""http://tree.ws.oss.comarch.com/"">" + System.Environment.NewLine;
            sCadena = sCadena + "<soapenv:Header/>" + System.Environment.NewLine;
            sCadena = sCadena + "<soapenv:Body>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:AddressItemRequest>" + System.Environment.NewLine;

            #region CUERPO XML

            #region ITEM DISTRICT

            sCadena = sCadena + "<tree:AddressItemDistrict operation = \"" + comarchDistrict.OPERATION + "\" abbreviationCity=\"" + comarchDistrict.ABBREVIATION_CITY + "\" abbreviationDistrict = \"" + comarchDistrict.ABBREVIATION_DISTRICT + "\">" + System.Environment.NewLine;
            //sCadena = sCadena + "<tree:citymasterabreviation>" + comarchDistrict.MASTER_ABBREVIATION + "</tree:abbreviationRegion>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:name>" + comarchDistrict.NAME + "</tree:name>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterDIVIPOLcategory>" + comarchDistrict.MASTER_DIVPOL_CATEGORY + "</tree:masterDIVIPOLcategory>" + System.Environment.NewLine;
            //sCadena = sCadena + "<tree:masterdistricttype>" + comarchDistrict.MASTER_DISTRICT_TYPE + "</tree:masterdistricttype>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:latitude>" + comarchDistrict.LATITUDE + "</tree:latitude>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:longitude>" + comarchDistrict.LONGITUDE + "</tree:longitude>" + System.Environment.NewLine;
            sCadena = sCadena + " </tree:AddressItemDistrict>" + System.Environment.NewLine;

            #endregion


            #endregion

            sCadena = sCadena + "</tree:AddressItemRequest>" + System.Environment.NewLine;
            sCadena = sCadena + "</soapenv:Body>" + System.Environment.NewLine;
            sCadena = sCadena + "</soapenv:Envelope>" + System.Environment.NewLine;

            // Eliminamos las comas y reemplazamos por puntos
            sCadena = sCadena.Replace(",", ".");

            // Creates the document
            soapEnvelopeXml.LoadXml(sCadena);
            #endregion

            // Returns the result
            return soapEnvelopeXml;
        }

        #endregion

        #region MESSAGE COMMUNE

        /// <summary>
        /// Creates the xml message from the Comarch object
        /// </summary>
        /// <param name="comarchDistrict">Objeto para la comuna en comarch</param>
        /// <returns>XML Message for calling Comarch</returns>
        public XmlDocument CreateMessageAddressItemCommune(AddressItemCommuneRequest comarchDistrict)
        {
            // Local variables
            XmlDocument soapEnvelopeXml = null;

            // Local variables
            string sCadena = null;

            #region CREATES THE XML


            // Creates the envelop
            soapEnvelopeXml = new XmlDocument();
            sCadena = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tree=""http://tree.ws.oss.comarch.com/"">" + System.Environment.NewLine;
            sCadena = sCadena + "<soapenv:Header/>" + System.Environment.NewLine;
            sCadena = sCadena + "<soapenv:Body>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:AddressItemRequest>" + System.Environment.NewLine;

            #region CUERPO XML

            #region ITEM DISTRICT

            sCadena = sCadena + "<tree:AddresItemCommune operation = \"" + comarchDistrict.OPERATION + "\" abbreviationDistrict=\"" + comarchDistrict.ABBREVIATION_DISTRICT + "\" abbreviationCommune = \"" + comarchDistrict.ABBREVIATION_COMMUNE + "\">" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:name>" + comarchDistrict.NAME + "</tree:name>" + System.Environment.NewLine;
            sCadena = sCadena + " </tree:AddresItemCommune>" + System.Environment.NewLine;

            #endregion


            #endregion

            sCadena = sCadena + "</tree:AddressItemRequest>" + System.Environment.NewLine;
            sCadena = sCadena + "</soapenv:Body>" + System.Environment.NewLine;
            sCadena = sCadena + "</soapenv:Envelope>" + System.Environment.NewLine;

            // Eliminamos las comas y reemplazamos por puntos
            sCadena = sCadena.Replace(",", ".");

            // Creates the document
            soapEnvelopeXml.LoadXml(sCadena);
            #endregion

            // Returns the result
            return soapEnvelopeXml;
        }

        #endregion

        #region MESSAGE LOCATION    

        /// <summary>
        /// Creates the xml message from the Comarch object
        /// </summary>
        /// <param name="comarchLocation">Objeto para la ciudad en comarch</param>
        /// <returns>XML Message for calling Comarch</returns>
        public XmlDocument CreateMessageLocation(LocationRequest comarchLocation)
        {
            // Local variables
            XmlDocument soapEnvelopeXml = null;

            // Local variables
            string sCadena = null;

            #region CREATES THE XML


            // Creates the envelop
            soapEnvelopeXml = new XmlDocument();
            sCadena = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tree=""http://tree.ws.oss.comarch.com/"">" + System.Environment.NewLine;
            sCadena = sCadena + "<soapenv:Header/>" + System.Environment.NewLine;
            sCadena = sCadena + "<soapenv:Body>" + System.Environment.NewLine;

            #region CUERPO XML

            #region ITEM CITY

            sCadena = sCadena + "<tree:LocationRequest operation = \"" + comarchLocation.OPERATION + "\" locationLegacyId = \"" + comarchLocation.LEGACY_ID + "\" abbreviationLocation = \"" + comarchLocation.NAME + "\" abbreviationCity = \"" + comarchLocation.NAME + "\" abbreviationRegion = \"" + comarchLocation.NAME + "\">" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:name>" + comarchLocation.NAME + "</tree:name>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:LocationAddress>" + comarchLocation.MASTER_ADDRESS + "</tree:LocationAddress>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:AccessInstructions>" + comarchLocation.ACCESS_INSTRUCTIONS + "</tree:AccessInstructions>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:OperationalStatus>" + comarchLocation.OPERATIONAL_STATUS + "</tree:OperationalStatus>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:elevation>" + comarchLocation.ELEVATION + "</tree:elevation>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:PEPCodeYear>" + comarchLocation.PEP_CODE_YEAR + "</tree:PEPCodeYear>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:PEPCodesociety>" + comarchLocation.PEP_CODE_SOCIETY + "</tree:PEPCodesociety>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:PEPcodeN1>" + comarchLocation.PEP_CODE_N1 + "</tree:PEPcodeN1>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:PEPCodeLetter>" + comarchLocation.PEP_CODE_LETTER + "</tree:PEPCodeLetter>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:InstallDate>" + comarchLocation.INSTALL_DATE + "</tree:InstallDate>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:DisposalDate>" + comarchLocation.DIPOSAL_DATE + "</tree:DisposalDate>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:DistinguishName>" + comarchLocation.DISTINGUISH_NAME + "</tree:DistinguishName>" + System.Environment.NewLine;

            sCadena = sCadena + "<tree:VisualImpact>" + comarchLocation.MASTER_VISUAL_IMPACT + "</tree:VisualImpact>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:latitude>" + comarchLocation.LATITUDE + "</tree:latitude>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:longitude>" + comarchLocation.LONGITUDE + "</tree:longitude>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:SiteType>" + comarchLocation.MASTER_SITE_TYPE_MIGRATION_IDENTIFIER + "</tree:SiteType>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterSitetype>" + comarchLocation.MASTER_SITE_TYPE + "</tree:masterSitetype>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterTxType>" + comarchLocation.MASTER_TX_TYPE + "</tree:masterTxType>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:locationDetailedType>" + comarchLocation.MASTER_LOCATION_DETAILED_TYPE_NAME + "</tree:locationDetailedType>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterConstructionareaType>" + comarchLocation.MASTER_CONSTRUCTION_AREA_TYPE + "</tree:masterConstructionareaType>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:requiredAttentionTime>" + comarchLocation.REQUIRED_ATTENTION_TIME + "</tree:requiredAttentionTime>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterNeighborhood>" + comarchLocation.MASTER_NEIHGBORHOOD + "</tree:masterNeighborhood>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:masterVisualImpact>" + comarchLocation.MASTER_VISUAL_IMPACT_2 + "</tree:masterVisualImpact>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:debrisAmount>" + comarchLocation.DEBRIS_AMOUNT + "</tree:debrisAmount>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:priority>" + comarchLocation.ENVIRONMENT_PRIORITY + "</tree:priority>" + System.Environment.NewLine;
            sCadena = sCadena + "<tree:cosite>" + comarchLocation.COSITE + "</tree:cosite>" + System.Environment.NewLine;


            sCadena = sCadena + "<tree:locationInterestRelations>" + System.Environment.NewLine;
            //sCadena = sCadena + "<tree:tag label=\"" + comarchLocation.relatio + "\">" + comarchLocation.DETAIL_TO_CONTRACT_RELATION_MASTERCONTRACT_CONTRACTTYPE + "</tree:tag>" + System.Environment.NewLine;
            sCadena = sCadena + " </tree:locationInterestRelations>" + System.Environment.NewLine;

            sCadena = sCadena + "<tree:locationContractRelations>" + System.Environment.NewLine;
            //sCadena = sCadena + "<tree:tag label=\"" + comarchLocation.relatio + "\">" + comarchLocation.DETAIL_TO_CONTRACT_RELATION_MASTERCONTRACT_CONTRACTTYPE + "</tree:tag>" + System.Environment.NewLine;
            sCadena = sCadena + " </tree:locationContractRelations>" + System.Environment.NewLine;

            sCadena = sCadena + "<tree:locationPoPRelations>" + System.Environment.NewLine;
            //sCadena = sCadena + "<tree:tag label=\"" + comarchLocation.relatio + "\">" + comarchLocation.DETAIL_TO_CONTRACT_RELATION_MASTERCONTRACT_CONTRACTTYPE + "</tree:tag>" + System.Environment.NewLine;
            sCadena = sCadena + " </tree:locationPoPRelations>" + System.Environment.NewLine;

            sCadena = sCadena + " </tree:LocationRequest>" + System.Environment.NewLine;

            #endregion

            #endregion

            sCadena = sCadena + "</soapenv:Body>" + System.Environment.NewLine;
            sCadena = sCadena + "</soapenv:Envelope>" + System.Environment.NewLine;

            // Eliminamos las comas y reemplazamos por puntos
            sCadena = sCadena.Replace(",", ".");

            // Creates the document
            soapEnvelopeXml.LoadXml(sCadena);
            #endregion

            // Returns the result
            return soapEnvelopeXml;
        }

        #endregion

        #endregion

        #region XML PROCESSING

        #region PROCESAR RESULTADO REGION

        private AddressItemRegionResponse ProcesarResultadoRegion(string sResultado)
        {
            // Local variables
            AddressItemRegionResponse respuesta = null;
            string sMessage = null;
            string sCodigo = null;


            // Process the input
            try
            {
                XmlDocument documento = new XmlDocument();

                documento.LoadXml(sResultado);

                respuesta = new AddressItemRegionResponse();


                // CODE
                XmlNodeList listaCode = documento.GetElementsByTagName("code");
                if (listaCode != null && listaCode.Count > 0)
                {
                    sCodigo = listaCode[0].InnerText;
                    respuesta.CODE_RESPUESTA = sCodigo;
                }
                // MESSAGE
                XmlNodeList listaMessage = documento.GetElementsByTagName("message");
                if (listaMessage != null && listaMessage.Count > 0)
                {
                    sMessage = listaMessage[0].InnerText;
                    respuesta.MENSAJE_RESPUESTA = sMessage;
                }

            }
            catch (Exception)
            { }

            // Returns the result
            return respuesta;
        }

        #endregion

        #region PROCESAR RESULTADO CITY

        private AddressItemCityResponse ProcesarResultadoCity(string sResultado)
        {
            // Local variables
            AddressItemCityResponse respuesta = null;
            string sMessage = null;
            string sCodigo = null;


            // Process the input
            try
            {
                XmlDocument documento = new XmlDocument();

                documento.LoadXml(sResultado);

                respuesta = new AddressItemCityResponse();


                // CODE
                XmlNodeList listaCode = documento.GetElementsByTagName("code");
                if (listaCode != null && listaCode.Count > 0)
                {
                    sCodigo = listaCode[0].InnerText;
                    respuesta.CODE_RESPUESTA = sCodigo;
                }
                // MESSAGE
                XmlNodeList listaMessage = documento.GetElementsByTagName("message");
                if (listaMessage != null && listaMessage.Count > 0)
                {
                    sMessage = listaMessage[0].InnerText;
                    respuesta.MENSAJE_RESPUESTA = sMessage;
                }

            }
            catch (Exception)
            { }
            // Returns the result
            return respuesta;
        }

        #endregion

        #region PROCESAR RESULTADO DISTRICT

        private AddressItemDistrictResponse ProcesarResultadoDistrict(string sResultado)
        {
            // Local variables
            AddressItemDistrictResponse respuesta = null;
            string sMessage = null;
            string sCodigo = null;


            // Process the input
            try
            {
                XmlDocument documento = new XmlDocument();

                documento.LoadXml(sResultado);

                respuesta = new AddressItemDistrictResponse();


                // CODE
                XmlNodeList listaCode = documento.GetElementsByTagName("code");
                if (listaCode != null && listaCode.Count > 0)
                {
                    sCodigo = listaCode[0].InnerText;
                    respuesta.CODE_RESPUESTA = sCodigo;
                }
                // MESSAGE
                XmlNodeList listaMessage = documento.GetElementsByTagName("message");
                if (listaMessage != null && listaMessage.Count > 0)
                {
                    sMessage = listaMessage[0].InnerText;
                    respuesta.MENSAJE_RESPUESTA = sMessage;
                }

            }
            catch (Exception)
            { }
            // Returns the result
            return respuesta;
        }

        #endregion

        #region PROCESAR RESULTADO COMMUNE

        private AddressItemCommuneResponse ProcesarResultadoCommune(string sResultado)
        {
            // Local variables
            AddressItemCommuneResponse respuesta = null;
            string sMessage = null;
            string sCodigo = null;


            // Process the input
            try
            {
                XmlDocument documento = new XmlDocument();

                documento.LoadXml(sResultado);

                respuesta = new AddressItemCommuneResponse();


                // CODE
                XmlNodeList listaCode = documento.GetElementsByTagName("code");
                if (listaCode != null && listaCode.Count > 0)
                {
                    sCodigo = listaCode[0].InnerText;
                    respuesta.CODE_RESPUESTA = sCodigo;
                }
                // MESSAGE
                XmlNodeList listaMessage = documento.GetElementsByTagName("message");
                if (listaMessage != null && listaMessage.Count > 0)
                {
                    sMessage = listaMessage[0].InnerText;
                    respuesta.MENSAJE_RESPUESTA = sMessage;
                }

            }
            catch (Exception)
            { }
            // Returns the result
            return respuesta;
        }

        #endregion

        #region PROCESAR RESULTADO LOCATION

        private LocationResponse ProcesarResultadoLocation(string sResultado)
        {
            // Local variables
            LocationResponse respuesta = null;
            string sMessage = null;
            string sCodigo = null;


            // Process the input
            try
            {
                XmlDocument documento = new XmlDocument();

                documento.LoadXml(sResultado);

                respuesta = new LocationResponse();


                // CODE
                XmlNodeList listaCode = documento.GetElementsByTagName("code");
                if (listaCode != null && listaCode.Count > 0)
                {
                    sCodigo = listaCode[0].InnerText;
                    respuesta.CODE_RESPUESTA = sCodigo;
                }
                // MESSAGE
                XmlNodeList listaMessage = documento.GetElementsByTagName("message");
                if (listaMessage != null && listaMessage.Count > 0)
                {
                    sMessage = listaMessage[0].InnerText;
                    respuesta.MENSAJE_RESPUESTA = sMessage;
                }

            }
            catch (Exception)
            {
            }
            // Returns the result
            return respuesta;
        }

        #endregion

        #endregion


        #region CREAR/ACTUALIZAR REGION
        public AddressItemRegionResponse ComarchAddressItemRegion(bool bConexionSegura, AddressItemRegionRequest region, string sTipoProyecto, string sComentarios, long userID, long? clienteID)
        {
            // Local variables
            string sResultado = null;
            XmlDocument soapEnvelop = null;
            AddressItemRegionResponse respuesta = null;
            List<object> listaResultado = null;
            // Monitorizacion del servicio
            MonitoringWSRegistrosController cMonitoring = new MonitoringWSRegistrosController();
            string sServicio = null;
            string sMetodo = null;
            bool bExito = false;
            bool bPropio = false;
            string sParametroEntrada = null;
            string sParametroSalida = null;
            long? monitoringAlquilerID = null;
            long? monitoringEmplazamientoID = null;


            try
            {

                // Monitoring information                
                sServicio = Comun.INTEGRACION_SERVICIO_COMARCH;
                sMetodo = "ComarchAddressItemRegion";

                // Creates the message                
                soapEnvelop = CreateMessageAddressItemRegion(region);
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(soapEnvelop.InnerXml);
                sParametroEntrada = soapEnvelop.InnerXml;
                // Creamos la conexion
                HttpWebRequest request = CreateWebRequest();





                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Flush();

                    //soapEnvelop.Save(stream);
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();
                        if (soapResult != null)
                        {
                            listaResultado = new List<object>();
                            sResultado = soapResult;
                            sParametroSalida = sResultado;
                            respuesta = ProcesarResultadoRegion(sResultado);
                            cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, soapResult, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);
                        }

                    }
                }

            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                else
                {

                }
                if (sParametroSalida is null)
                {
                    sParametroSalida = ex.Message;
                }
                cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);

            }
            catch (Exception ex)
            {
                if (sParametroSalida is null)
                {
                    sParametroSalida = ex.Message;
                }
                cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);

            }


            // Returns the result
            return respuesta;
        }
        #endregion

        #region CREAR/ACTUALIZAR CITY
        public AddressItemCityResponse ComarchAddressItemCity(bool bConexionSegura, AddressItemCityRequest city, string sTipoProyecto, string sComentarios, long userID, long? clienteID)
        {
            // Local variables
            string sResultado = null;
            XmlDocument soapEnvelop = null;
            AddressItemCityResponse respuesta = null;
            List<object> listaResultado = null;
            // Monitorizacion del servicio
            MonitoringWSRegistrosController cMonitoring = new MonitoringWSRegistrosController();
            string sServicio = null;
            string sMetodo = null;
            bool bExito = false;
            bool bPropio = false;
            string sParametroEntrada = null;
            string sParametroSalida = null;
            long? monitoringAlquilerID = null;
            long? monitoringEmplazamientoID = null;



            try
            {

                // Monitoring information                
                sServicio = Comun.INTEGRACION_SERVICIO_COMARCH;
                sMetodo = "ComarchAddressItemCity";

                // Creates the message                
                soapEnvelop = CreateMessageAddressItemCity(city);
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(soapEnvelop.InnerXml);
                sParametroEntrada = soapEnvelop.InnerXml;
                // Creamos la conexion
                HttpWebRequest request = CreateWebRequest();





                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Flush();
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();
                        if (soapResult != null)
                        {
                            listaResultado = new List<object>();
                            sResultado = soapResult;
                            sParametroSalida = sResultado;
                            respuesta = ProcesarResultadoCity(sResultado);

                            cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, soapResult, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);
                        }

                    }
                }

            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                else
                {
                }
                if (sParametroSalida is null)
                {
                    sParametroSalida = ex.Message;
                }
                cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);

            }
            catch (Exception ex)
            {
                if (sParametroSalida is null)
                {
                    sParametroSalida = ex.Message;
                }
                cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);

            }


            // Returns the result
            return respuesta;
        }
        #endregion

        #region CREAR/ACTUALIZAR DISTRICT
        public AddressItemDistrictResponse ComarchAddressItemDistrict(bool bConexionSegura, AddressItemDistrictRequest district, string sTipoProyecto, string sComentarios, long userID, long? clienteID)
        {
            // Local variables
            string sResultado = null;
            XmlDocument soapEnvelop = null;
            AddressItemDistrictResponse respuesta = null;
            List<object> listaResultado = null;
            // Monitorizacion del servicio
            MonitoringWSRegistrosController cMonitoring = new MonitoringWSRegistrosController();
            string sServicio = null;
            string sMetodo = null;
            bool bExito = false;
            bool bPropio = false;
            string sParametroEntrada = null;
            string sParametroSalida = null;
            long? monitoringAlquilerID = null;
            long? monitoringEmplazamientoID = null;


            try
            {

                // Monitoring information                
                sServicio = Comun.INTEGRACION_SERVICIO_COMARCH;
                sMetodo = "ComarchAddressItemDistrict";

                // Creates the message                
                soapEnvelop = CreateMessageAddressItemDistrict(district);
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(soapEnvelop.InnerXml);
                sParametroEntrada = soapEnvelop.InnerXml;
                // Creamos la conexion
                HttpWebRequest request = CreateWebRequest();





                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Flush();

                    //soapEnvelop.Save(stream);
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();
                        if (soapResult != null)
                        {
                            listaResultado = new List<object>();
                            sResultado = soapResult;
                            sParametroSalida = sResultado;
                            respuesta = ProcesarResultadoDistrict(sResultado);

                            cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, soapResult, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);
                        }

                    }
                }

            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                else
                {

                }
                if (sParametroSalida is null)
                {
                    sParametroSalida = ex.Message;
                }
                cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);

            }
            catch (Exception ex)
            {
                if (sParametroSalida is null)
                {
                    sParametroSalida = ex.Message;
                }
                cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);

            }


            // Returns the result
            return respuesta;
        }
        #endregion

        #region CREAR/ACTUALIZAR COMMUNE
        public AddressItemCommuneResponse ComarchAddressItemCommune(bool bConexionSegura, AddressItemCommuneRequest commune, string sTipoProyecto, string sComentarios, long userID, long? clienteID)
        {
            // Local variables
            string sResultado = null;
            XmlDocument soapEnvelop = null;
            AddressItemCommuneResponse respuesta = null;
            List<object> listaResultado = null;
            // Monitorizacion del servicio
            MonitoringWSRegistrosController cMonitoring = new MonitoringWSRegistrosController();
            string sServicio = null;
            string sMetodo = null;
            bool bExito = false;
            bool bPropio = false;
            string sParametroEntrada = null;
            string sParametroSalida = null;
            long? monitoringAlquilerID = null;
            long? monitoringEmplazamientoID = null;


            try
            {

                // Monitoring information                
                sServicio = Comun.INTEGRACION_SERVICIO_COMARCH;
                sMetodo = "ComarchAddressItemCommune";

                // Creates the message                
                soapEnvelop = CreateMessageAddressItemCommune(commune);
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(soapEnvelop.InnerXml);
                sParametroEntrada = soapEnvelop.InnerXml;
                // Creamos la conexion
                HttpWebRequest request = CreateWebRequest();





                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Flush();
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();
                        if (soapResult != null)
                        {
                            listaResultado = new List<object>();
                            sResultado = soapResult;
                            sParametroSalida = sResultado;
                            respuesta = ProcesarResultadoCommune(sResultado);

                            cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, soapResult, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);
                        }

                    }
                }

            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                else
                {

                }
                if (sParametroSalida is null)
                {
                    sParametroSalida = ex.Message;
                }
                cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);

            }
            catch (Exception ex)
            {
                if (sParametroSalida is null)
                {
                    sParametroSalida = ex.Message;
                }
                cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);

            }


            // Returns the result
            return respuesta;
        }
        #endregion

        #region CREAR/ACTUALIZAR LOCATION
        public List<object> ComarchLocation(bool bConexionSegura, LocationRequest location, string sTipoProyecto, string sComentarios, long userID, long? clienteID)
        {
            // Local variables
            string sResultado = null;
            XmlDocument soapEnvelop = null;
            LocationResponse respuesta = null;
            List<object> listaResultado = null;
            // Monitorizacion del servicio
            MonitoringWSRegistrosController cMonitoring = new MonitoringWSRegistrosController();
            string sServicio = null;
            string sMetodo = null;
            bool bExito = false;
            bool bPropio = false;
            string sParametroEntrada = null;
            string sParametroSalida = null;
            long? monitoringAlquilerID = null;
            long? monitoringEmplazamientoID = null;


            try
            {

                // Monitoring information                
                sServicio = Comun.INTEGRACION_SERVICIO_COMARCH;
                sMetodo = "ComarchLocation";

                // Creates the message                
                soapEnvelop = CreateMessageLocation(location);
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(soapEnvelop.InnerXml);
                sParametroEntrada = soapEnvelop.InnerXml;
                // Creamos la conexion
                HttpWebRequest request = CreateWebRequest();

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Flush();

                    //soapEnvelop.Save(stream);
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();
                        if (soapResult != null)
                        {
                            listaResultado = new List<object>();
                            sResultado = soapResult;
                            sParametroSalida = sResultado;
                            respuesta = ProcesarResultadoLocation(sResultado);
                            if (respuesta != null)
                            {
                                if (respuesta.MENSAJE_RESPUESTA.Equals(Comun.INTEGRACION_COMARCH_CREATE_ADDRESS_SUCCESS_MESSAGE))
                                {
                                    listaResultado.Add(Comun.INTEGRACION_COMARCH_CREATE_ADDRESS_SUCCESS_MESSAGE);
                                    bExito = true;
                                }
                                else
                                {
                                    listaResultado.Add(Comun.INTEGRACION_COMARCH_CREATE_ADDRESS_ERROR_MESSAGE);
                                    bExito = false;
                                }
                                listaResultado.Add(respuesta);
                                listaResultado.Add(sResultado);
                                listaResultado.Add(sParametroEntrada);

                            }
                            else
                            {
                                listaResultado.Add(Comun.INTEGRACION_COMARCH_CREATE_ADDRESS_ERROR_MESSAGE);
                                listaResultado.Add(respuesta);
                                listaResultado.Add(sResultado);
                                listaResultado.Add(sParametroEntrada);
                                bExito = false;
                            }
                            cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, soapResult, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);
                        }

                    }
                }

            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                else
                {

                }
                if (sParametroSalida is null)
                {
                    sParametroSalida = ex.Message;
                }
                cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);

            }
            catch (Exception ex)
            {
                if (sParametroSalida is null)
                {
                    sParametroSalida = ex.Message;
                }
                cMonitoring.AgregarRegistro(sTipoProyecto, userID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, clienteID, bExito, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);

            }


            // Returns the result
            return listaResultado;
        }
        #endregion
    }
}