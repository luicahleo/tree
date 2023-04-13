using System.Collections.Generic;

namespace TreeAPI.API.Mobile.General.JSON
{
    // THIS MUST MATCH THE CLIENT
    public class MapPolyPoint
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    // THIS MUST MATCH THE CLIENT
    public class MapPolygon
    {
        public string Name { get; set; }
        public string FillColor { get; set; }
        public float BorderWidth { get; set; }
        public string BorderColor { get; set; }

        public List<MapPolyPoint> PolyPoints { get; set; }
    }
}
