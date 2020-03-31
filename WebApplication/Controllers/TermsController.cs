
using SWI.Libraries.AD.BLL;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class TermsController : Controller
    {
        // GET: Terms
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Colors()
        {
            AD_DefinationBL db = new AD_DefinationBL();
            return View(db.ToList("Colors"));
        }
    }
}