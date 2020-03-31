using SWI.Libraries.Security.Entities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace AirView.Filters.MobileAuth
{
    public class MobileAuth : ActionFilterAttribute
    {
        private string AuthKey { get { return WebConfigurationManager.AppSettings["AuthKey"]; } }
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (AuthKey != HttpContext.Current.Request.QueryString["Key"])
                actionContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
            base.OnActionExecuting(actionContext);
        }
    }
}