using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Drawing;

using TreeAPI;
using TreeCore.Data;
using CapaNegocio;

using Newtonsoft.Json;

using TreeAPI.API.Mobile.Crypto;
using TreeAPI.API.Mobile.General.JSON;
using TreeAPI.API.Mobile.General.SOAP;

namespace TreeAPI.API.Mobile.General
{
    public class SitesCloseByExtra
    {
        public string Information;
        public bool Shared;
        public string Operator;
    }

    public class MapDataRequestPackage
    {
        public bool RequestResult { get; set; }
        public string RequestResultString { get; set; }

        public long BlockSize { get; set; }
        public string BlockData { get; set; }

    }

    public class MapRequestHandler
    {
        // type string from server XML (always make sure these names match between server and clients)
        public const string MAP_REQUEST_SITESCLOSEBY = "SitesCloseBy";

        public string ProcessRequestSitesCloseBy(System.Web.SessionState.HttpSessionState Session, CryptoHandler cryptoHandler, MapRequestSOAP req)
        {
            string jsonOut = "";
            Locale appLocale = new Locale();

            string selectedLocale = appLocale.MobileLocale(req.Platform, req.Locale);

            // set the session locale
            Session["LOCALE"] = selectedLocale;

            MobileResourceRequester ResHelper = new MobileResourceRequester(Resources.MobileResources.ResourceManager, selectedLocale);

            // path the service is currently running on
            string servicePath = AppDomain.CurrentDomain.BaseDirectory;

            MapDataRequestPackage jsonMapDataRequestPackage = new MapDataRequestPackage();
            jsonMapDataRequestPackage.RequestResult = false;
            jsonMapDataRequestPackage.RequestResultString = ResHelper.GetString("srv_error");
            jsonMapDataRequestPackage.BlockSize = 0;
            jsonMapDataRequestPackage.BlockData = "";

            MapElements DataBlock = new MapElements();

            // need at least the user and project to filter
            if (req.UserID == -1)
            {
                jsonMapDataRequestPackage.RequestResult = false;
                jsonMapDataRequestPackage.RequestResultString = ResHelper.GetString("srv_invalid_user");
            }
            else
            {
                Usuarios us = null;
                UsuariosController userController = new UsuariosController();

                try
                {
                    List<Usuarios> userList = userController.GetItemsList<Usuarios>("UsuarioID == " + req.UserID.ToString());
                    if (userList.Count > 0)
                    {
                        us = userList[0];
                    }

                    if (us != null &&
                        us.Activo)
                    {
                        double vLat = 0.0;
                        double vLong = 0.0;

                        double dblLat = double.Parse(req.CenterLatitude, CultureInfo.InvariantCulture);
                        double dblLong = double.Parse(req.CenterLongitude, CultureInfo.InvariantCulture);
                        double dblDistance = double.Parse(req.Distance, CultureInfo.InvariantCulture);

                        // to match the web, divide the distance by 1000
                        dblDistance /= 1000.0;

                        EmplazamientosController cEmplazamientos = new EmplazamientosController();
                        List<Sp_EmplazamientosCercanos_GetResult> pointList = new List<Sp_EmplazamientosCercanos_GetResult>();

                        pointList = cEmplazamientos.GetAllPuntosCercanos(dblLat, dblLong, dblDistance).ToList();

                        // allocate ready for population
                        long imageIndex = 0;
                        string dictKey = "";
                        Dictionary<string, long> imgDictionary = new Dictionary<string, long>();

                        DataBlock.IconImages = new List<MapIconImage>();

                        DataBlock.Circles = new List<MapCircle>();
                        DataBlock.Markers = new List<MapMarker>();
                        DataBlock.Polygons = new List<MapPolygon>();

                        // add the center as a marker
                        MapMarker m = new MapMarker();
                        m.Latitude = dblLat.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        m.Longitude = dblLong.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        m.Label = "";

                        // center object
                        Vw_Emplazamientos Centro;
                        Centro = cEmplazamientos.GetPuntoCentro(dblLat, dblLong);
                        if (Centro != null)
                        {
                            SitesCloseByExtra extra = new SitesCloseByExtra();
                            extra.Information = Centro.NombreSitio;
                            extra.Operator = Centro.Operador;
                            extra.Shared = (bool)Centro.Compartido;

                            if (Centro.Compartido)
                                dictKey = "ima/" + Centro.Operador + "64c.png";
                            else
                                dictKey = "ima/" + Centro.Operador + "64.png";

                            if (imgDictionary.ContainsKey(dictKey))
                            {
                                long value = imgDictionary[dictKey];
                                m.ImageIdentifier = value;
                            }
                            else
                            {
                                imgDictionary.Add(dictKey, imageIndex);
                                m.ImageIdentifier = imageIndex;

                                imageIndex++;
                            }

                            m.ExtraData = JsonConvert.SerializeObject(extra, Formatting.Indented);
                            DataBlock.Markers.Add(m);
                        }

                        // convert the point list to a map marker
                        int pointIndex = 0;
                        foreach (Sp_EmplazamientosCercanos_GetResult val in pointList)
                        {
                            SitesCloseByExtra extra = new SitesCloseByExtra();
                            extra.Information = val.Informacion;
                            extra.Operator = val.Operador;
                            extra.Shared = (bool)val.Compartido;

                            m = new MapMarker();
                            vLat = (double)val.Latitud;
                            vLong = (double)val.Longitud;
                            m.Latitude = vLat.ToString(System.Globalization.CultureInfo.InvariantCulture);
                            m.Longitude = vLong.ToString(System.Globalization.CultureInfo.InvariantCulture);

                            // 
                            m.IconBase64 = "";

                            // try and make sure there's always a center object
                            if (Centro == null &&
                                pointIndex == 0)
                            {
                                if (extra.Shared)
                                    dictKey = "ima/" + val.Operador + "64c.png";
                                else
                                    dictKey = "ima/" + val.Operador + "64.png";
                            }
                            else
                            {
                                if (extra.Shared)
                                    dictKey = "ima/" + val.Operador + "32c.png";
                                else
                                    dictKey = "ima/" + val.Operador + "32.png";
                            }
                            if (imgDictionary.ContainsKey(dictKey))
                            {
                                long value = imgDictionary[dictKey];
                                m.ImageIdentifier = value;
                            }
                            else
                            {
                                imgDictionary.Add(dictKey, imageIndex);
                                m.ImageIdentifier = imageIndex;

                                imageIndex++;
                            }
                            

                            m.ExtraData = JsonConvert.SerializeObject(extra, Formatting.Indented);
                            DataBlock.Markers.Add(m);

                            pointIndex++;
                        }

                        // create a circle for the area
                        MapCircle areaCircle = new MapCircle();
                        areaCircle.Latitude = req.CenterLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        areaCircle.Longitude = req.CenterLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        areaCircle.Radius = (float)Double.Parse(req.Distance.ToString(System.Globalization.CultureInfo.InvariantCulture));
                        areaCircle.BorderWidth = 1;
                        areaCircle.BorderColor = "#80c3fc49";
                        areaCircle.FillColor = "#80c3fc49";

                        DataBlock.Circles.Add(areaCircle);

                        // now write the image cache
                        foreach (KeyValuePair<string, long> pair in imgDictionary)
                        {
                            MapIconImage icon = new MapIconImage();
                            icon.Identifier = pair.Value;

                            // load the image file and convert to base64
                            try
                            {
                                Bitmap img = (Bitmap)Image.FromFile(servicePath + pair.Key, true);
                                if (img != null)
                                {
                                    icon.ImageWidth = img.Width;
                                    icon.ImageHeight = img.Height;

                                    try
                                    {
                                        System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                        byte[] byteImage = ms.ToArray();
                                        icon.Base64Image = Convert.ToBase64String(byteImage);
                                    }
                                    catch( System.Exception ex )
                                    {
                                        System.Console.Write(ex);

                                        icon.Base64Image = "";
                                    }

                                    DataBlock.IconImages.Add(icon);
                                }
                            }
                            catch (System.IO.FileNotFoundException ex)
                            {
                                System.Console.Write(ex);
                            }
                        }

                        // all ok
                        jsonMapDataRequestPackage.RequestResult = true;
                        jsonMapDataRequestPackage.RequestResultString = ResHelper.GetString("srv_ok"); 

                        // now encrypt the data block
                        string rawData = JsonConvert.SerializeObject(DataBlock, Formatting.Indented);
                        if (rawData != null &
                            rawData.Length > 0)
                        {
                            AESCrypto.StringEncryption strEnc = cryptoHandler.EncryptString(rawData);

                            if (strEnc.encryptedString != null &&
                                strEnc.encryptedString.Length > 0)
                            {
                                jsonMapDataRequestPackage.BlockSize = strEnc.originalByteLength;
                                jsonMapDataRequestPackage.BlockData = strEnc.encryptedString;
                            }
                            else
                            {
                                jsonMapDataRequestPackage.RequestResult = false;
                                jsonMapDataRequestPackage.RequestResultString = ResHelper.GetString("srv_auth_error");
                            }
                        }

                    }
                    else
                    {
                        // user not found
                        jsonMapDataRequestPackage.RequestResult = false;
                        jsonMapDataRequestPackage.RequestResultString = ResHelper.GetString("srv_user_invalid_inactive");
                    }
                }
                catch (Exception ex)
                {
                    System.Console.Write(ex);

                    // general error
                    jsonMapDataRequestPackage.RequestResult = false;
                    jsonMapDataRequestPackage.RequestResultString = ResHelper.GetString("srv_exception");
                }
            }

            jsonOut = JsonConvert.SerializeObject(jsonMapDataRequestPackage, Formatting.Indented);

            return jsonOut;
        }

        public string Process(System.Web.SessionState.HttpSessionState Session, CryptoHandler cryptoHandler, MapRequestSOAP req)
        {
            string output = "";
            
            switch (req.RequestType)
            {
                case MAP_REQUEST_SITESCLOSEBY:
                    {
                        output = ProcessRequestSitesCloseBy(Session, cryptoHandler, req);
                    }break;
                default:
                    break;
            }

            return output;
        }
    }
}
