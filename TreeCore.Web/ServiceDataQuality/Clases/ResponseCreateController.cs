namespace TreeCore.Clases
{
    public class ResponseCreateController
    {
        public InfoResponse infoResponse;
        public object Data;

        public ResponseCreateController(InfoResponse infoResponse, object Data)
        {
            this.infoResponse = infoResponse;
            this.Data = Data;
        }
    }
}