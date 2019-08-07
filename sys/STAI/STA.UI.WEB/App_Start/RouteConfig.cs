using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace STA.UI.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "DePara", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "DefaultLog",
                url: "{controller}/{action}/{nomeArquivo}",
                defaults: new { controller = "Log", action = "ExibirArquivoLog", nomeArquivo = UrlParameter.Optional }
            );
        }
    }
}