using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.SWI.Survey.Model;
using Library.SWI.Survey.BLL;
using SWI.AirView.Models;
using SWI.Security.Filters;

namespace WebApplication.Areas.Survey.Controllers
{
    [IsLogin]
    public class SectionController : Controller
    {
        // GET: Survey/Section
        [IsLogin(CheckPermission = false)]
        public ActionResult New(Int64 Id)
        {
            ViewBag.SurveyId = Id;
            return PartialView("~/Areas/Survey/Views/Section/_New.cshtml");
        }


     
        [IsLogin(CheckPermission = false),HttpPost]
        public ActionResult New(TSS_Section sec)
        {
            Response res = new Response();
            TSS_SectionBL sb = new TSS_SectionBL();

            try
            {
                res.Status = "success";

                if (sec.SectionId>0)
                {
                    sec.CreatedOn = DateTime.MinValue;
                    res.Value = sb.Manage("Update", sec);
                    res.Message = "Update successfully";
                }
                else
                {
                    sec.CreatedBy = ViewBag.UserId;
                    sec.CreatedOn = DateTime.Now;
                    res.Value = sb.Manage("Insert", sec);
                    res.Message = "Save successfully";
                }
                

            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(Int64 Id)
        {
            Response res = new Response();

            try
            {
                TSS_SectionBL sb = new TSS_SectionBL();
                TSS_Section sec = new TSS_Section();
                sec.SectionId = Id;
                sb.Manage("Delete", sec);
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

        [IsLogin(CheckPermission =false)]
        public ActionResult List(string Value)
        {
            TSS_SectionBL sb = new TSS_SectionBL();
            var rec = (Value!="0")? sb.ToList("By_SurveyId", Value):null;
            var rec1 = (Value!="0")? sb.ToListWithoutTree("By_SurveyId", Value):null;
            var result = new { list1 = rec, list2 = rec1 };
            //return PartialView("~/Areas/Survey/Views/Section/_List.cshtml", rec);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [IsLogin(CheckPermission = false)]
        public ActionResult Tree(string filter, string value)
        {
            TSS_SectionBL sb = new TSS_SectionBL();
            var list = sb.ToList(filter, value);
            //var Parent = Sections.Where(m => m.PSectionId == 0).ToList();
   

            List<TSS_Section> treeList = new List<TSS_Section>();

            //foreach (var item in Sections)
            //{
            //    GetTreeView(item, Sections);
            //}
            //return Json(Parent, JsonRequestBehavior.AllowGet);

            if (list.Count > 0)
            {
                treeList = BuildTree(list);
            }
            return new JsonResult { Data = new { treeList = treeList }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [IsLogin(CheckPermission = false)]
        public List<TSS_Section> BuildTree(List<TSS_Section> list)
        {
            List<TSS_Section> returnList = new List<TSS_Section>();
            var topLevels = list.Where(a => a.PSectionId == list.OrderBy(b => b.PSectionId).FirstOrDefault().PSectionId);
            returnList.AddRange(topLevels);
            foreach (var i in topLevels)
            {
                GetTreeView(list, i, ref returnList);
            }

            return returnList;
        }


        private void GetTreeView(List<TSS_Section> list, TSS_Section current, ref List<TSS_Section> returnList)
        {
            var childs = list.Where(a => a.PSectionId == current.SectionId).ToList();
            current.Sections = new List<TSS_Section>();
            current.Sections.AddRange(childs);
            foreach (var i in childs)
            {
                GetTreeView(list, i, ref returnList);
            }
        }

        


        [IsLogin(CheckPermission = false)]
        public ActionResult WithQuestions(string Value)
        {
            List<TSS_Section> Sections = new List<TSS_Section>();
            TSS_SectionBL sb = new TSS_SectionBL();

            string[] val = Value.Split(',');
            foreach (var item in val)
            {
                TSS_Section sec = (item != "0") ? sb.ToSingle("By_SectionId", item) : null;
                if (sec!=null)
                {
                    TSS_QuestionBL qb = new TSS_QuestionBL();
                    sec.QuestionList = qb.ToList("GET_BY_SECTIONID", sec.SectionId.ToString());


                    Sections.Add(sec);
                }
               
            }
           
            return Json(Sections, JsonRequestBehavior.AllowGet);
        }
    }
}