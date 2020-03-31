using Library.SWI.Survey.BLL;
using Library.SWI.Survey.Model;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
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
    public class QuestionController : Controller
    {
        // GET: Survey/Question
        public ActionResult Index()
        {
            return View();
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult New()
        {
            AD_DefinationBL db = new AD_DefinationBL();
            ViewBag.QuestionType = db.SelectedList("byDefinationType", "Question Type", "-Question Type-");
            return PartialView("~/Areas/Survey/Views/Question/_New.cshtml");
        }

        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult New(TSS_Question q)
        {
            //if (Request.Files.Count > 0)
            //{

            //}

            Response res = new Response();
            TSS_QuestionBL qb = new TSS_QuestionBL();
            dbDataTable dbtd = new dbDataTable();
            try
            {
                DataTable dt = dbtd.List();
                foreach (var item in q.Responses)
                {
                    //if (item.ResponseText != null)
                    //{
                    myDataTable.AddRow(dt, "Value1", q.QuestionId, "Value2", item.ResponseText, "Value3", item.ResponseValue, "Value4", item.SortOrder, "Value5", item.IsPassed,
                               "Value6", item.MinValue, "Value7", item.MaxValue, "Value8", item.IsGps, "Value9", true, "Value10", item.IsReadOnly, "Value11", item.UserValues);
                    //}
                }
                q.CreatedOn = DateTime.Now;
                q.CreatedBy = ViewBag.UserId;

                if (q.QuestionId == 0)
                {
                    res.Value = qb.Manage("Insert", q, dt);
                }
                else
                {
                    res.Value = qb.Manage("Update", q, dt);
                }

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

        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult IsActive(TSS_Question q)
        {
            Response res = new Response();
            TSS_QuestionBL qb = new TSS_QuestionBL();
            dbDataTable dbtd = new dbDataTable();
            try
            {
                DataTable dt = dbtd.List();
                foreach (var item in q.Responses)
                {
                    if (item.ResponseText != null)
                    {
                        myDataTable.AddRow(dt, "Value1", q.QuestionId, "Value2", item.ResponseText, "Value3", item.ResponseValue, "Value4", item.SortOrder, "Value5", item.IsPassed,
                                   "Value6", item.MinValue, "Value7", item.MaxValue, "Value8", item.IsGps, "Value9", true);
                    }
                }
                q.CreatedOn = DateTime.Now;
                q.CreatedBy = ViewBag.UserId;

                if (q.QuestionId > 0)
                {
                    res.Value = qb.Manage("UpdateStatus", q, dt);
                }
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

        [IsLogin(CheckPermission = false)]
        public ActionResult Delete(Int64 Id)
        {
            Response res = new Response();

            try
            {
                TSS_QuestionBL qb = new TSS_QuestionBL();
                TSS_Question q = new TSS_Question();
                q.QuestionId = Id;
                qb.Manage("Delete", q, null);
                res.Status = "success";
                res.Message = "Delete successfully";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult Single(Int64 Id)
        {
            TSS_QuestionBL qb = new TSS_QuestionBL();
            TSS_Question q = qb.ToSingle("GET_BY_QUESTIONID", Id.ToString());
            if (q != null)
            {
                TSS_ResponseBL rb = new TSS_ResponseBL();
                q.Responses = rb.ToList("GET_BY_QUESTIONID", Id.ToString());
            }



            return Json(q, JsonRequestBehavior.AllowGet);
        }




        [IsLogin(CheckPermission = false)]
        public JsonResult ToList(string Filter, string Value)
        {
            TSS_QuestionBL qb = new TSS_QuestionBL();
            var rec = qb.ToList(Filter, Value);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }
    }
}