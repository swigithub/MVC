using Library.SWI.Survey.DAL;
using Library.SWI.Survey.Model;
using SWI.AirView.Models;
using SWI.Libraries.Common;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Survey.Controllers
{
    [IsLogin]
    public class TemplateController : Controller
    {
        // GET: Survey/Template
        [IsLogin(CheckPermission = false)]
        public ActionResult Index(Int64 Id,Int64 SectionId=0,string IdType="Survey")
        {
            ViewBag.SurveyId = Id;
            ViewBag.SectionId = SectionId;
            ViewBag.IdType=IdType;
            return View();
        }
        [IsLogin(CheckPermission = false),HttpPost]
        public ActionResult Index(List< TSS_Template> Temp)
        {
            Response res = new Response();
            TSS_TemplateDL td = new TSS_TemplateDL();
            try
            {
                dbDataTable dbdt = new dbDataTable();
                DataTable dt = dbdt.List();
                foreach (var item in Temp)
                {
                    myDataTable.AddRow(dt, "Value1", item.SurveyId, "Value2", item.SectionId, "Value3", item.QuestionId,"Value4",item.IdType);
                }
                td.Manage("ADD_QUESTION_FROM_TEMPLATE", dt);
                res.Status = "success";
                res.Message = "Save successfully";

            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}