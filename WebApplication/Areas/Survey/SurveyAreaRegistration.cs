﻿using System.Web.Mvc;

namespace WebApplication.Areas.Survey
{
    public class SurveyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Survey";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Survey_default",
                "Survey/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
                //, namespaces: new[] { "SWI.Areas.Survey.Controllers" }
            );
        }
    }
}