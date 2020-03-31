using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.DAL;
using SWI.Security.Filters;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    [IsLogin, ErrorHandling]
    public class AdminDashboardController : Controller
    {
        // GET: AdminDashboard
        public ActionResult Index()
        {
            try
            {
                AV_WidgetsBL wb = new AV_WidgetsBL();
                ViewBag.Widgets = wb.ToList("GetAll");
            }
            catch (System.Exception)
            {

               // throw;
            }
           
            return View();
        }

        public ActionResult TableResult(string Filter,string Value)
        {

            AD_GetQueryResultDL gr = new AD_GetQueryResultDL();
            ViewBag.TableResult = gr.Get(Filter, Value,null);
            return PartialView("~/Views/AdminDashboard/_TableResult.cshtml");
        }

        public ActionResult Chart(string Filter, string Value)
        {
            AD_GetQueryResultDL gr = new AD_GetQueryResultDL();
            ViewBag.Result = gr.Get(Filter, Value,null);
            //var dt= gr.Get(Filter, Value);
            //if (dt.Columns.Contains("x"))
            //{

            //}

            AV_WidgetsBL wb = new AV_WidgetsBL();
            var rec = wb.ToSingle("ByWidgetId", Value);
            return PartialView("~/Views/AdminDashboard/_Chart.cshtml", rec);
        }
    }
}