namespace TreeAPI.API.Mobile.General.SOAP
{
    // THIS MUST MATCH THE CLIENT
    public class MapRequestSOAP
    {
        public string Platform;
        public string Locale;
        public long UserID;

        public string RequestType;

        public string CenterLatitude;
        public string CenterLongitude;

        public string Distance;
    }
}
