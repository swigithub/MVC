using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.AirView.Entities;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    [IsLogin, ErrorHandling, HandleError]
    public class HelpController : Controller
    {

        public ActionResult Index(string id)
        {
            return RedirectToAction("all");
        }
        // GET: Help
        public ActionResult All()
        {
            return View();
        }
        //ListJson for All List
        //Associated with All()
        [IsLogin(CheckPermission = false)]
        public JsonResult ListHelpJson(int id)
        {
            bool state = false;
            if (id != 0)
            {
                state = true;
            }

            AD_HelpBL help = new AD_HelpBL();
            var jsonData = help.List("List_Help", state).ToList();
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        //Request to Publish & Draft Content in All List
        //Associated with All()
        [IsLogin(CheckPermission = false)]
        public JsonResult ListRow(int FeatureID, int id)
        {
            bool state = false;
            if (id != 0)
            {
                state = true;
            }

            AD_HelpBL help = new AD_HelpBL();
            help.ListRow("ListRow_Help", state, FeatureID);

            return Json("Success!", JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title, ComponentId, ModuleId, FeatureId, Description")] AD_Help post)
        {
            if (ModelState.IsValid)
            {
                AD_HelpBL help = new AD_HelpBL();
                help.Create("Create_Help", post.ComponentId, post.ModuleId, post.FeatureId, post.Title, post.Description, false);
                return RedirectToAction("all");
            }

            return View(post);
        }

        //It will load ComponentId in create help
        //Associated with CreateHelp()
        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult HelpComponent()
        {
            AD_HelpBL help = new AD_HelpBL();
            var jsonData = help.Read("Get_Component_list").ToList();
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //It will load ModuleId in create help
        //Associated with CreateHelp()
        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult HelpModule(int id)
        {
            AD_HelpBL help = new AD_HelpBL();
            var jsonData = help.Read("Get_Module_list", id).ToList();
            List<AD_Help> response = new List<AD_Help>();
            foreach (AD_Help model in jsonData)
            {
                response.Add(model);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //It will load FeatureId in create help
        //Associated with CreateHelp()
        [IsLogin(CheckPermission = false)]
        public JsonResult HelpFeature(int MID, int CID)
        {
            AD_HelpBL help = new AD_HelpBL();
            var jsonData = help.Read("Get_Feature_list", CID, MID).ToList();
            List<AD_Help> response = new List<AD_Help>();
            foreach (AD_Help model in jsonData)
            {

                response.Add(model);

            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AD_HelpBL post = new AD_HelpBL();

            List<AD_Help> data = post.ReadPost("Read_Help", id).ToList();

            String code = data[0].Description;

            ViewBag.code = code.Replace("'", "");
            ViewBag.HelpId = data[0].HelpId;
            ViewBag.ComponentId = data[0].ComponentId;
            ViewBag.ComponentName = data[0].ComponentName;
            ViewBag.ModuleId = data[0].ModuleId;
            ViewBag.ModuleName = data[0].ModuleName;
            ViewBag.FeatureId = data[0].FeatureId;
            ViewBag.FeatureName = data[0].FeatureName;

            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        [HttpPost,ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HelpId, Title, Description")] AD_Help post)
        {
            if (ModelState.IsValid)
            {
                AD_HelpBL help = new AD_HelpBL();
                help.EditPost("Edit_Help", post.HelpId, post.Title, post.Description);
                return RedirectToAction("all");
            }

            return View(post);
        }

        public ActionResult Read(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AD_HelpBL post = new AD_HelpBL();

            List<AD_Help> data = post.ReadPost("Read_Help", id).ToList();

            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

    }
}