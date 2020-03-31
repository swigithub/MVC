using Library.SWI.Survey.BLL;
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
    public class QuestionLogicController : Controller
    {
        // GET: Survey/QuestionLogic

        [IsLogin(CheckPermission = false)]
        public ActionResult New()
        {
            return PartialView("~/Areas/Survey/Views/QuestionLogic/_New.cshtml");
        }

        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult New(TSS_QuestionLogic ql, Int64[] ToQuestions)
        {

            string ToQuestionId = string.Join(",", ToQuestions);

            Response res = new Response();
            TSS_QuestionLogicBL qlb = new TSS_QuestionLogicBL();
            dbDataTable dbdt = new dbDataTable();
            try
            {
                DataTable dt = dbdt.List();

                myDataTable.AddRow(dt, "Value1", ql.SurveyId, "Value2", ql.SectionId, "Value3", ql.FromQuestionId, "Value4", ToQuestionId, "Value5", ql.ConditionId,
                                       "Value6", ql.ResponseId, "Value7", ql.ActionId, "Value8", true);

                res.Value = qlb.Manage("Insert", "", dt);
                res.Status = "success";
                res.Message = "Update successfully";

                //+++++++++++++++++++++++++++++++++++
                //deleteExistingRows(ql.FromQuestionId, ToQuestions);E:\TFS Projects\SWI\WebApplication\Areas\Survey\Views\QuestionLogic\

                //for (int i = 0; i < ToQuestions.Length; i++)
                //{
                //    DataTable dt = dbdt.List();

                //    myDataTable.AddRow(dt, "Value1", ql.SurveyId, "Value2", ql.SectionId, "Value3", ql.FromQuestionId, "Value4", ToQuestions[i], "Value5", ql.ConditionId,
                //                           "Value6", ql.ResponseId, "Value7", ql.ActionId, "Value8", true);
                //    res.Value = qlb.Manage("Insert", "", dt);
                //}
                //res.Status = "success";
                //res.Message = "Update successfully";
                //------------------------------------
            }

            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult Delete(Int64 Id)
        {
            Response res = new Response();

            try
            {
                TSS_QuestionLogicBL sb = new TSS_QuestionLogicBL();

                sb.Manage("Delete", Id.ToString(), null);
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
        public JsonResult ToList(string Filter, string Value)
        {
            TSS_QuestionLogicBL qlb = new TSS_QuestionLogicBL();
            var rec = qlb.ToList(Filter, Value);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }



        //public List<TSS_Section> BuildTree(List<TSS_Section> list)
        //{
        //    List<TSS_Section> returnList = new List<TSS_Section>();
        //    var topLevels = list.Where(a => a.PSectionId == list.OrderBy(b => b.PSectionId).FirstOrDefault().PSectionId);
        //    returnList.AddRange(topLevels);
        //    foreach (var i in topLevels)
        //    {
        //        //GetTreeView(list, i, ref returnList);
        //    }

        //    return returnList;
        //}



        [IsLogin(CheckPermission = false), HttpGet]
        public JsonResult ToQuestionIds(string Filter, string Value)
        {
            TSS_QuestionLogicBL qlb = new TSS_QuestionLogicBL();
            var rec = qlb.ToQuestionIds(Filter, Value);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

    }
}