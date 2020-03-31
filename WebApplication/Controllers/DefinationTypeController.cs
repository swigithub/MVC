using System;
using System.Web.Mvc;
using SWI.AirView.Common;
using SWI.Libraries.Security.Entities;
using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.BLL;
using SWI.Security.Filters;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.Entities;
using SWI.AirView.Models;
using System.Data;
using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.AD.BLL;
using AirView.DBLayer.AirView.BLL;

namespace WebApplication.Controllers
{
    [IsLogin, ErrorHandling]
    public class DefinationTypeController : Controller
    {
        //----King--Coder--Safi-UK-----//
        [IsLogin]
        public ActionResult Index()
        {
            SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
            ViewBag.PDefinationName = sl.DefinationTypes();
            return View();
        }

        public ActionResult New()
        {
            SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
            ViewBag.PDefinationName = sl.DefinationTypes();
            AD_DefinationTypes model = new AD_DefinationTypes();
          
            return PartialView("~/Views/DefinationType/_DefinationTypes.cshtml", model);
        }
    
        [IsLogin(CheckPermission = false), HttpPost]
    
        public ActionResult New(AD_DefinationTypes dt)
        {
            Response res = new Response();
            try
            {
                AD_DefinationTypesBL wb =new AD_DefinationTypesBL();
    
                if (dt.DefinationTypeId >0)
                {
                wb.Insert("Edite",dt);

                }
                else
                {
                   wb.Insert("New",dt);

                }
                res.Status = "success";
                res.Message = "save successfully";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(new { response = res }, JsonRequestBehavior.AllowGet);


        }
     
        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult Paging(int current, int rowCount, string searchPhrase)
        {
            AD_DefinationTypesBL pb = new AD_DefinationTypesBL();

            Int64 count = 0;

            current = (current == 0) ? 1 : current;
            rowCount = (rowCount == 0) ? 5 : rowCount;
            int offset = (current - 1) * rowCount;
            var rec = pb.Paging(offset, rowCount, searchPhrase, ref count);

            return Json(new { current = current, total = count, rows = rec, rowCount = rowCount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditDefinationType(int Id)
        {
            AD_DefinationTypesBL db = new AD_DefinationTypesBL();
            //List<AD_DefinationTypes> list = db.ToList("ALL");
            SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
            ViewBag.PDefinationName = sl.DefinationTypes();
            AD_DefinationTypes model = new AD_DefinationTypes();

            if (Id > 0)
            {
                AD_DefinationTypes dt = db.SingleDefinationType("Single", Id.ToString());
                model.DefinationType = dt.DefinationType;
                model.PDefinationTypeId =dt.PDefinationTypeId;
                model.DefinationTypeId = dt.DefinationTypeId;
                model.IsActive = dt.IsActive;
            }
            return PartialView("~/Views/DefinationType/_DefinationTypes.cshtml", model);
        }
        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult Delete(int Id)
        {
            bool result = false;
            if (Id > 0)
            {
                AD_DefinationTypesBL db = new AD_DefinationTypesBL();

                result = db.DeleteSingleDefination("Delete", Id.ToString());

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult DefinationTypeStatus(int Id)
        {
            bool res = false;
            try {
               
            if (Id > 0)
            {
                AD_DefinationTypesBL db = new AD_DefinationTypesBL();
                AD_DefinationTypesBL wb = new AD_DefinationTypesBL();
                var result = db.SingleDefinationType("ChanceStatus", Id.ToString());
               
                    res = true;

            }
            }
            catch (Exception)
            {
                res = false;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}