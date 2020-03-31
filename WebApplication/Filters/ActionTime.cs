using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace SWI.Security.Filters
{
    /*----MoB!----*/
    public class ActionTime : ActionFilterAttribute
    {
        Stopwatch stopwatch = new Stopwatch();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            stopwatch.Restart();
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            stopwatch.Stop();
            TimeSpan timeTakenByActionMethod = stopwatch.Elapsed;
            filterContext.Controller.ViewBag.ActionTime = timeTakenByActionMethod.ToString();
            //save this time to database for tunning purpose or any other.
        }
    }
}