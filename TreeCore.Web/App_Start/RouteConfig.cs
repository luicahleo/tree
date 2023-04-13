using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace TreeCore
{
    public class RouteConfig
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("Login",
                "Login/",
                "~/General/Login.aspx",
                true
            );

            routes.MapPageRoute(
                "Home",
                string.Empty,
                "~/General/Default.aspx",
                true,
                new RouteValueDictionary(new
                {
                    modulo = "Global",
                    pagina = ""
                })
            );

            routes.MapPageRoute("Default",
               "{modulo}",
               "~/General/Default.aspx",
               true
           );

            routes.MapPageRoute("Doc",
                "Doc/{action}/{slug}",
                "~/PaginasComunes/DocumentosVista.aspx",
                true,
                new RouteValueDictionary(new
                {
                    action = Comun.VirtualPath.Shared.Action.show,
                    tipoDoc = "",
                    slug = ""
                })
            );

            //redirección a pagina no encontrada
            routes.MapPageRoute("404",
                 "{404}",
                 "~/General/" + Comun.PaginaNoEncontrada,
                 true,
                 new RouteValueDictionary(new
                 {
                     NotFound = "404"
                 })
            );




        }

    }
}