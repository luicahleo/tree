using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace TreeCore
{
    public class VirtualPath
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            /*routes.MapPageRoute(
                "Home",
                string.Empty,
                "~/Default.aspx"
            );*/

            /*routes.MapPageRoute("",
                "Category/{action}/{categoryName}",
                "~/PaginaNoEncontrada.aspx", true, new RouteValueDictionary(new { categoryName = "food", action = "show" }));*/

            routes.MapPageRoute("Doc",
                "Doc/{action}/{slug}",
                "~/PaginasComunes/DocumentosVista.aspx", 
                true, 
                new RouteValueDictionary(new { 
                    action = Comun.Shared.Action.show, 
                    tipoDoc = "", 
                    slug = "" 
                })
            );



            //redirección a pagina no encontrada
            routes.MapPageRoute("404",
                 "{404}",
                 "~/" + Comun.PaginaNoEncontrada, true, new RouteValueDictionary(new { NotFound = "404" }));
        }

    }
}