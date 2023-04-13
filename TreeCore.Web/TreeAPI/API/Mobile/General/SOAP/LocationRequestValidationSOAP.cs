using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeAPI.API.Mobile.General.SOAP
{
    // THIS MUST MATCH THE CLIENT
    public class LocationRequestValidationSOAP
    {
        public string Platform;
        public string Locale;
        public long UserID;

        public string SrcLongitude;
        public string SrcLatitude;

        public string DestLongitude;
        public string DestLatitude;

        public string DistanceInside;
        public string DistanceOutside;
    }
}
