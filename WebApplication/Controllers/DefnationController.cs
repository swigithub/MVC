using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.Security.Entities;
using eSpares.Levity;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AD.DAL;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.Common;
using SWI.Libraries.Security.BLL;
using SWI.Libraries.Security.Entities;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace SWI.AirView.Controllers
{
    /*----MoB!----*/

    [IsLogin, ErrorHandling]
    public class DefnationController : Controller
    {
        // GET: Defnation
        [IsLogin(CheckPermission = false)]
        public ActionResult New()
        {


            AD_DefinationBL db = new AD_DefinationBL();
            ViewBag.Definations = db.ToList("AllActive");
            Sec_UserDefinationTypeBL dtb = new Sec_UserDefinationTypeBL();
            var User = Session["user"] as LoginInformation;
            ViewBag.DefinationTypes = dtb.ToListDefinations("GetDefinationTypeByUId", User.UserId.ToString());

            //AD_DefinationTypesBL dtb = new AD_DefinationTypesBL();
            //ViewBag.DefinationTypes = dtb.ToList("All");

            SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
            ViewBag.SelectedDefinationTypes = sl.UserDefinationTypes(User.UserId.ToString());
            //
            return View();
        }




        [HttpPost]
        public ActionResult New(List<AD_Defination> def)
        {
            Response res = new Response();
            try
            {
                dbDataTable ddt = new dbDataTable();
                DataTable dtdef = ddt.List();
                foreach (var item in def)
                {
                    myDataTable.AddRow(dtdef, "Value1", item.DefinationId, "Value2", item.DefinationName, "Value3", item.PDefinationId, "Value4", item.DefinationTypeId, "Value5", item.KeyCode,
                                        "Value6", item.DisplayType, "Value7", item.ColorCode, "Value8", item.InputType, "Value9", item.MaxLength, "Value10", item.SortOrder, "Value11", item.IsActive, "Value12", item.DisplayText);
                }
                AD_DefinationDL dd = new AD_DefinationDL();
                dd.Manage("Insert", dtdef);
                res.Status = "success";
                res.Message = "save successfully";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                res.Status = "danger";
                res.Message = ex.Message;
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult Grid(Int64 Id)
        {
            AD_DefinationBL db = new AD_DefinationBL();
            return PartialView("~/views/Defnation/Grid.cshtml", db.ToList("byDefinationTypeId", Id.ToString()));

        }


        [IsLogin(CheckPermission = false)]
        public JsonResult ToList(string filter, string value)
        {
            AD_DefinationBL db = new AD_DefinationBL();
            var rec = db.ToList(filter, value);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WorkGroup()
        {
            return View();
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult Group(List<Sec_Workgroup> modelBind)
        {
            AD_DefinationBL db = new AD_DefinationBL();

            for (int x = 0; x < modelBind.Count; x++)
            {
                if (ModelState.IsValid)
                {
                    if(modelBind[x].WorkgroupId > 0)
                    {
                        db.List_Work_Group1("UpdateWorkGroup", modelBind[x].WorkgroupName, modelBind[x].WorkgroupId,Convert.ToInt32(ViewBag.UserId));
                    }
                    else
                    {
                        db.List_Work_Group1("InsertWorkGroup", modelBind[x].WorkgroupName,0, Convert.ToInt32(ViewBag.UserId));
                    }
                    
                }
               
            }

            return View(modelBind);

        }
        [HttpGet,IsLogin(CheckPermission =false)]
        public JsonResult GetGroup()
        {
            AD_DefinationBL db = new AD_DefinationBL();
            var jsonReturn = db.List_Work_Group("GetGroup").ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult DeleteGroup(int id)
        {
            AD_DefinationBL db = new AD_DefinationBL();
            var jsonReturn = db.Delete_Group("DeleteGroup", id);
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, IsLogin(CheckPermission = false)]
        public JsonResult CheckDeleteGroup(int id)
        {
            AD_DefinationBL db = new AD_DefinationBL();
            var jsonReturn = db.List_Work_Group("CheckDelete", id).ToList();
            //var jsonReturn = db.Delete_Group("CheckDelete", id);
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

    }
}