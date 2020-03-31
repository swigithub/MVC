using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("");
            routes.LowercaseUrls = true;

            routes.MapRoute(
                  name: "NetLayerReport",
                  url: "NetLayerReport/{Site}/{BandId}/{Carrier}/{NetworkMode}/{CircleRadios}/{AzmuthRadius}/{Auto}",
                  defaults: new { controller = "site", action = "netlayerreport2", Site = "0", BandId=0, Carrier=0, NetworkMode=0, CircleRadios=17 , AzmuthRadius =200,Auto="0"}
                  //  constraints: new { url = "[A-Za-z]*" }
                  , namespaces: new[] { "SWI.AirView.Controllers" }
              );

            routes.MapRoute(
                 name: "EditNetLayerReport",
                 url: "site/EditNetLayerReport/{SiteId}/{BandId}/{Carrier}/{NetworkMode}",
                 defaults: new { controller = "site", action = "EditNetLayerReport", SiteId = 0, BandId = 0, Carrier = 0, NetworkMode = "" }
                 //  constraints: new { url = "[A-Za-z]*" }
                 , namespaces: new[] { "SWI.AirView.Controllers" }
             );

            routes.MapRoute(
                name: "Help",
                url: "help/{action}/{id}",
                defaults: new { controller = "Help", action = "All", id = UrlParameter.Optional }
                 , namespaces: new[] { "SWI.AirView.Controllers" }

            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
                 , namespaces: new[] { "SWI.AirView.Controllers" }
                
            );



        }
    }
}
