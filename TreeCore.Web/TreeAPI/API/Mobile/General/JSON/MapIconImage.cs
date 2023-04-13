namespace TreeAPI.API.Mobile.General.JSON
{
    // THIS MUST MATCH THE CLIENT
    public class MapIconImage
    {
        public long Identifier { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public string Base64Image { get; set; }
    }
}
