using System.Collections.Generic;

namespace TreeAPI.API.Mobile.General.JSON
{
    // THIS MUST MATCH THE CLIENT
    public class MapElements
    {
        public List<MapIconImage> IconImages { get; set; }

        public List<MapPolygon> Polygons { get; set; }
        public List<MapCircle> Circles { get; set; }
        public List<MapMarker> Markers { get; set; }
    }
}
