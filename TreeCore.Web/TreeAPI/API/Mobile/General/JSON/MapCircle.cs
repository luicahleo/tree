namespace TreeAPI.API.Mobile.General.JSON
{
    // THIS MUST MATCH THE CLIENT
    public class MapCircle
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public float Radius { get; set; }
        public string FillColor { get; set; }
        public float BorderWidth { get; set; }
        public string BorderColor { get; set; }
    }
}
