namespace TreeAPI.API.Mobile.General.JSON
{
    // THIS MUST MATCH THE CLIENT
    public class MapMarker
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Label { get; set; }
        public string ExtraData { get; set; }
        public long ImageIdentifier { get; set; }
        public string IconBase64 { get; set; }
    }
}
