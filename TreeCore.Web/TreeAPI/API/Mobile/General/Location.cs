using Newtonsoft.Json;
using System;

namespace TreeAPI.API.Mobile.General
{
    public class Location
    {
        // THIS MUST MATCH THE CLIENT
        public class LocationResponseValidation
        {
            public bool IsValid { get; set; }
            public String SrcLatitude { get; set; }
            public String SrcLongitude { get; set; }

            public String DestLatitude { get; set; }
            public String DestLongitude { get; set; }

            public String Distance { get; set; }
        }

        public double Deg2Rad(double deg)
        {
            return deg * (Math.PI / 180d);
        }

        /// <summary>
        /// Will get the distance in MILES between two lat/long points
        /// </summary>
        /// <param name="srcLat"></param>
        /// <param name="srcLong"></param>
        /// <param name="destLat"></param>
        /// <param name="destLong"></param>
        /// <returns></returns>
        public double GetDistanceFromLatLonInMiles(double srcLat, double srcLong, double destLat, double destLong)
        {
            double radiusOfEarthMILES = 3959d; // approx radius of the earth in miles

            // convert difference to radians
            double dLat = Deg2Rad(destLat - srcLat);
            double dLon = Deg2Rad(destLong - srcLong);

            double halfLat = Math.Sin(dLat * 0.5d);
            double halfLong = Math.Sin(dLon * 0.5d);

            double a =
              halfLat * halfLat +
              Math.Cos(Deg2Rad(srcLat)) * Math.Cos(Deg2Rad(destLat)) *
              halfLong * halfLong;

            double c = 2d * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1d - a));

            double d = radiusOfEarthMILES * c; // Distance in miles

            return Math.Abs(d);
        }

        /// <summary>
        /// Will get the distance in KM between two lat/long points
        /// </summary>
        /// <param name="srcLat"></param>
        /// <param name="srcLong"></param>
        /// <param name="destLat"></param>
        /// <param name="destLong"></param>
        /// <returns></returns>
        public double GetDistanceFromLatLonInKm(double srcLat, double srcLong, double destLat, double destLong)
        {
            double radiusOfEarthKM = 6371d; // approx radius of the earth in km

            // convert difference to radians
            double dLat = Deg2Rad(destLat - srcLat);
            double dLon = Deg2Rad(destLong - srcLong);

            double halfLat = Math.Sin(dLat * 0.5d);
            double halfLong = Math.Sin(dLon * 0.5d);

            double a =
              halfLat * halfLat +
              Math.Cos(Deg2Rad(srcLat)) * Math.Cos(Deg2Rad(destLat)) *
              halfLong * halfLong;

            double c = 2d * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1d - a));

            double d = radiusOfEarthKM * c; // Distance in km

            return Math.Abs(d);
        }

        public string LocationRequestValidateHandler(System.Web.SessionState.HttpSessionState Session, SOAP.LocationRequestValidationSOAP req)
        {
            string jsonOut = "";
            Location newLocation = new Location();

            Location.LocationResponseValidation jsonLocationValidateResponse = new Location.LocationResponseValidation();

            jsonLocationValidateResponse.IsValid = false;

            try
            {
                Locale appLocale = new Locale();

                string selectedLocale = appLocale.MobileLocale(req.Platform, req.Locale);

                // set the session locale
                Session["LOCALE"] = selectedLocale;

                // copy the original data
                jsonLocationValidateResponse.SrcLatitude = req.SrcLatitude;
                jsonLocationValidateResponse.SrcLongitude = req.SrcLongitude;
                jsonLocationValidateResponse.DestLatitude = req.DestLatitude;
                jsonLocationValidateResponse.DestLongitude = req.DestLongitude;

                // now calculate if the validation is correct
                jsonLocationValidateResponse.Distance = "9999";

                // convert to doubles
                double srcLat = double.Parse(req.SrcLatitude, System.Globalization.CultureInfo.InvariantCulture);
                double srcLong = double.Parse(req.SrcLongitude, System.Globalization.CultureInfo.InvariantCulture);

                double destLat = double.Parse(req.DestLatitude, System.Globalization.CultureInfo.InvariantCulture);
                double destLong = double.Parse(req.DestLongitude, System.Globalization.CultureInfo.InvariantCulture);

                double reqDistanceInside = double.Parse(req.DistanceInside, System.Globalization.CultureInfo.InvariantCulture);

                double calculatedDistanceKM = newLocation.GetDistanceFromLatLonInKm(srcLat, srcLong, destLat, destLong);
                calculatedDistanceKM = Math.Round(calculatedDistanceKM, 3);

                double calculatedDistanceMiles = newLocation.GetDistanceFromLatLonInMiles(srcLat, srcLong, destLat, destLong);
                calculatedDistanceMiles = Math.Round(calculatedDistanceMiles, 3);

                jsonLocationValidateResponse.Distance = calculatedDistanceKM.ToString();

                if (calculatedDistanceKM <= reqDistanceInside)
                    jsonLocationValidateResponse.IsValid = true;

            }
            catch (Exception ex)
            {
                System.Console.Write(ex);
            }

            jsonOut = JsonConvert.SerializeObject(jsonLocationValidateResponse, Formatting.Indented);

            return jsonOut;
        }
    }
}
