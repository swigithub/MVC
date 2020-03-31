
using System.Web.Mvc;
using System.Web.Routing;
namespace SWI.Security.Filters
{
    public class ErrorHandling : System.Web.Mvc.ActionFilterAttribute, System.Web.Mvc.IExceptionFilter
    {
        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }


        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            // filterContext.Controller.ViewBag.OnExceptionError = "Exception filter called";
            


            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            // if the request is AJAX return JSON else view.
            if (IsAjax(filterContext))
            {
                //Because its a exception raised after ajax invocation
                //Lets return Json
                filterContext.Result = new JsonResult()
                {
                    Data = filterContext.Exception.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
            }
            else {
                filterContext.Controller.TempData["msg_error"] = filterContext.Exception.Message;
                filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary(
                          new { controller = "Error", action = "index" , type ="Filter"}));
            }
           

            //filterContext.Result = new ViewResult
            //{
            //    ViewName = "~/Views/Shared/Error.cshtml",
            //    MasterName = "~/Views/Shared/_Layout.cshtml",
            //    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
            //    TempData = filterContext.Controller.TempData
            //};
        }
    }
}