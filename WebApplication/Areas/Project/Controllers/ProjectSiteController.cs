using AirView.DBLayer.Project.Model;
using Library.SWI.Project.BLL;
using Newtonsoft.Json;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Project.Controllers
{
    [IsLogin]
    public class ProjectSiteController : Controller
    {
        PM_ProjectBL bal = new PM_ProjectBL();
        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult Update(PM_ProjectSite pm_projectsite)
        {
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            var Status = HttpContext.Request["Status"];
            var File = HttpContext.Request["file"];
            pm_projectsite = JsonConvert.DeserializeObject<PM_ProjectSite>(Status);


            HttpFileCollectionBase files = HttpContext.Request.Files;
            if (Request.Files != null)
            {
                if (Request.Files.Count > 0)
                {


                    var file = Request.Files[0];
                    actualFileName = file.FileName;
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    int size = file.ContentLength;

                    try
                    {
                        string FilePath = Path.Combine(Server.MapPath("~/Content/Files/") + fileName).ToString();
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/Files/"), fileName));
                        pm_projectsite.FilePath = FilePath;


                    }
                    catch (Exception)
                    {
                        Message = "File upload failed! Please try again";
                    }


                }
            }

            

            pm_projectsite.CreatedBy = ViewBag.UserId;
            var result = bal.UpdateMsWindow("Update_MSWindowId", pm_projectsite);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult ToList(string Filter,Int64 ProjectId=0,Int64 SiteId=0,int UserId=0,string Value1 = null)
        {
            var result = bal.ProjectSiteList(Filter, ProjectId, SiteId, UserId, Value1);
                return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}