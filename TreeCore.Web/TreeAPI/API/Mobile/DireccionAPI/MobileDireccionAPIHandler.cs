using CapaNegocio;
using Newtonsoft.Json;
using System;
using TreeAPI.API.Mobile.Crypto;
using TreeAPI.API.Mobile.General;
using TreeCore;
using TreeCore.Data;
using static TreeAPI.API.Mobile.DireccionAPI.JSON.DireccionAPIMobile;

namespace TreeAPI
{
    public class MobileDireccionAPIHandler
    {
        public class MobileDireccionAPIRequestSOAP
        {
            public string CodigoCliente;
            public string Platform;
            public string Locale;            

            public long CLLength;
        }

        public string Process(System.Web.SessionState.HttpSessionState Session, CryptoHandler cryptoHandler, MobileDireccionAPIRequestSOAP req)
        {
            Locale appLocale = new Locale();

            string contrasenia = null;
            string selectedLocale = appLocale.MobileLocale(req.Platform, req.Locale);

            // set the session locale
            Session["LOCALE"] = selectedLocale;

            MobileResourceRequester ResHelper = new MobileResourceRequester(Resources.MobileResources.ResourceManager, selectedLocale);

            AppDirecciones appDirecciones;
            AppDireccionesController cAppDirecciones = new AppDireccionesController();

            DireccionAPISessionPackage jsonDireccionAPIPackage = new DireccionAPISessionPackage();
            jsonDireccionAPIPackage.RequestResult = false;
            jsonDireccionAPIPackage.RequestResultString = ResHelper.GetString("srv_error");
            jsonDireccionAPIPackage.BlockSize = 0;
            jsonDireccionAPIPackage.BlockData = "";

            DireccionAPISession DataBlock;
            AESCrypto.StringDecryption strOut = cryptoHandler.DecryptString(req.CodigoCliente, req.CLLength);
            if (strOut.decryptedString != null &&
                strOut.decryptedString.Length > 0)
                req.CodigoCliente = strOut.decryptedString;

            try
            {
                appDirecciones = cAppDirecciones.GetDireccionByCodigo(req.CodigoCliente);
                if (appDirecciones != null && appDirecciones.Activo)
                {
                    if (appDirecciones.Contrasenia != null)
                    {
                        contrasenia = Util.DecryptKey(appDirecciones.Contrasenia);
                    }
                    
                    DataBlock = new DireccionAPISession
                    {
                        Locale = req.Locale,
                        DescripcionCliente = appDirecciones.DescripcionCliente,
                        URL = appDirecciones.URL,
                        AppDireccionID = appDirecciones.AppDireccionID,
                        Activo = appDirecciones.Activo,
                        UsuarioAPI = appDirecciones.Usuario,
                        ContraseñaAPI = contrasenia
                    };
                    jsonDireccionAPIPackage.RequestResult = true;
                    jsonDireccionAPIPackage.RequestResultString = ResHelper.GetString("srv_ok");

                    // now encrypt the data block
                    string rawData = JsonConvert.SerializeObject(DataBlock, Formatting.Indented);
                    if (rawData != null &
                        rawData.Length > 0)
                    {
                        AESCrypto.StringEncryption strEnc = cryptoHandler.EncryptString(rawData);

                        if (strEnc.encryptedString != null &&
                            strEnc.encryptedString.Length > 0)
                        {
                            jsonDireccionAPIPackage.BlockSize = strEnc.originalByteLength;
                            jsonDireccionAPIPackage.BlockData = strEnc.encryptedString;
                        }
                        else
                        {
                            jsonDireccionAPIPackage.RequestResult = false;
                            jsonDireccionAPIPackage.RequestResultString = ResHelper.GetString("srv_cod_error");
                        }
                    }
                }
                else
                {
                    jsonDireccionAPIPackage.RequestResult = false;
                    jsonDireccionAPIPackage.RequestResultString = ResHelper.GetString("srv_codigo_no_valido");
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex);

                jsonDireccionAPIPackage.RequestResult = false;
                jsonDireccionAPIPackage.RequestResultString = ResHelper.GetString("srv_exception");
            }

            string jsonOut = JsonConvert.SerializeObject(jsonDireccionAPIPackage, Formatting.Indented);

            return jsonOut;
        }

    }
}