namespace TreeAPI.API.Mobile.DireccionAPI.JSON
{
    public class DireccionAPIMobile
    {
        public class DireccionAPISession
        {
            public string URL { get; set; }
            public long AppDireccionID { get; set; }
            public string CodigoCliente { get; set; }
            public string DescripcionCliente { get; set; }   
            public bool Activo { get; set; }
            public string UsuarioAPI { get; set; }
            public string ContraseñaAPI { get; set; }

            public string Locale { get; set; }

        }

        public class DireccionAPISessionPackage
        {
            public bool RequestResult { get; set; }
            public string RequestResultString { get; set; }
            public long BlockSize { get; set; }
            public string BlockData { get; set; }

        }
    }
}