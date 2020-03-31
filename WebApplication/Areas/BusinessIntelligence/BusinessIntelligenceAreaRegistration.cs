using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.BusinessIntelligence
{
    public class BusinessIntelligenceAreaRegistration: AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BusinessIntelligence";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BusinessIntelligence_default",
                "BusinessIntelligence/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
              "BusinessIntelligence_GenericTemplate",
              "BusinessIntelligence/{controller}/{action}/{moduleType}/{moduleId}/{renderId}",
              new { action = "Index", moduleType = UrlParameter.Optional, moduleId= UrlParameter.Optional, renderId= UrlParameter.Optional }
          );
            context.MapRoute(
              name: "DashboardReport",
              url: "dsrpt/{controller}/{action}/{dirName}/{fileName}",
              defaults: new { controller = "Home", action = "Index" }
          );
        }
    }
}