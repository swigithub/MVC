using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "swi/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
             config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
