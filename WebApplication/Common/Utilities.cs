using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SWI.AirView.Common
{
    public static class Utilities
    {
        public static DateTime StartOfWeek()
        {
            DateTime baseDate = DateTime.Today;
            return baseDate.AddDays(-(int)baseDate.DayOfWeek);
        }
        public static DateTime EndOfWeek()
        {
            DateTime baseDate = DateTime.Today;
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            return thisWeekStart.AddDays(7).AddSeconds(-1);
        }
        public static DateTime StartOfMonth()
        {
            DateTime baseDate = DateTime.Today;
            return baseDate.AddDays(1 - baseDate.Day);
        }
        public static DateTime EndOfMonth()
        {
            DateTime baseDate = DateTime.Today;
            var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
            return thisMonthStart.AddMonths(1).AddSeconds(-1);
        }
        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }

        public static string RenderToString(this PartialViewResult partialView)
        {
            var httpContext = HttpContext.Current;

            if (httpContext == null)
            {
                throw new NotSupportedException("An HTTP context is required to render the partial view to a string");
            }

            var controllerName = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();

            var controller = (ControllerBase)ControllerBuilder.Current.GetControllerFactory().CreateController(httpContext.Request.RequestContext, controllerName);

            var controllerContext = new ControllerContext(httpContext.Request.RequestContext, controller);

            var view = ViewEngines.Engines.FindPartialView(controllerContext, partialView.ViewName).View;

            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using (var tw = new HtmlTextWriter(sw))
                {
                    view.Render(new ViewContext(controllerContext, view, partialView.ViewData, partialView.TempData, tw), tw);
                }
            }

            return sb.ToString();
        }

        static public string ElementAtOrDefault(string[] Array, int index)
        {
            return (index >= 0 && index < Array.Length && Array != null) ? Array[index] : null;
        }
    }
}