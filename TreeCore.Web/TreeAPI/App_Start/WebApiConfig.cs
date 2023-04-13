using Newtonsoft.Json;
using System.Web.Http;

namespace TreeAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            //Elimina la respuesta en xml
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //No devuelve nulos en el json de salida
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;


            /*
            config.Routes.MapHttpRoute(
                name: "controller",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );*/




        }
    }
}
