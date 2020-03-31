using AirView.DBLayer.Project.Model;
using SWI.Libraries.AD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Project.Controllers
{
    public class KPIController : Controller
    {
        // GET: Project/KPI
        AirView.DBLayer.Project.BLL.PM_KPI_BL KPI_BL = new AirView.DBLayer.Project.BLL.PM_KPI_BL();

        #region Definations
        public ActionResult GetDefinations()
        {

            try
            {
                var result = KPI_BL.DefinationList("ByDefinationTypeId");
                return Json(new { Status = true, error = "none", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, error = ex }, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion 
        #region KPI
        public ActionResult GetKPI(Int64 Id = 0, string Type = "")
        {
            try
            {
                AirView.DBLayer.Project.BLL.PM_KPI_BL KPI_BL = new AirView.DBLayer.Project.BLL.PM_KPI_BL();
                List<AD_Defination> Technology = new List<AD_Defination>();
                List<AD_Defination> Level = new List<AD_Defination>();
                List<AD_Defination> DataType = new List<AD_Defination>();
                List<AD_Defination> Kpi_Type = new List<AD_Defination>();
                List<AD_Defination> Band = new List<AD_Defination>();
                List<AD_Defination> Definations = KPI_BL.DefinationList("ByDefinationTypeId");
                for (int i = 0; i < Definations.Count; i++)
                {
                    if (Definations[i].DefinationTypeId == 8)
                    {
                        Technology.Add(Definations[i]);
                    }

                    else if (Definations[i].DefinationTypeId == 120064)
                    {
                        DataType.Add(Definations[i]);
                    }
                    else if (Definations[i].DefinationTypeId == 90064)
                    {
                        Level.Add(Definations[i]);
                    }
                    else if (Definations[i].DefinationTypeId == 90065)
                    {
                        Kpi_Type.Add(Definations[i]);
                    }
                    else if (Definations[i].DefinationTypeId == 10)
                    {
                        Band.Add(Definations[i]);
                    }
                }
                ViewBag.Band = Band;
                ViewBag.Technologies = Technology;
                ViewBag.Level = Level;
                ViewBag.DataType = DataType;
                ViewBag.KpiType = Kpi_Type;
                List<PM_KPI> result = KPI_BL.KPIList("Get_KPI", Id);
                foreach (var item in result)
                {
                    item.Bands = Definations.Where(x => x.PDefinationId == item.Technology).ToList();
                }
                return PartialView("~/Areas/Project/Views/Defination/_KPI.cshtml", result);
            }
            catch (Exception ex)
            {
                return PartialView("<b>" + ex + "</b>");
            }
        }
        public ActionResult GetKPIJson(Int64 Id)
        {

            try
            {
                List<PM_KPI> result = KPI_BL.KPIList("Get_KPI", Id);
                return Json(new { Status = true, error = "none", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, error = ex }, JsonRequestBehavior.DenyGet);
            }
        }
        [HttpPost]
        public ActionResult Save(List<PM_KPI> kpis)
        {
            try
            {
                bool result = KPI_BL.Insert("Insert_KPI", kpis, Convert.ToString(ViewBag.UserId));
                return Json(new { Status = result, error = "none" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Status = false, error = ex }, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion
        #region Threshold
        public ActionResult GetThreshold(string Id)
        {
            try
            {
                AirView.DBLayer.Project.BLL.PM_KPI_BL KPI_BL = new AirView.DBLayer.Project.BLL.PM_KPI_BL();
                List<AD_Defination> Condition = new List<AD_Defination>();
                List<AD_Defination> Action = new List<AD_Defination>();
                List<AD_Defination> Definations = KPI_BL.DefinationList("ByDefinationTypeId");
                for (int i = 0; i < Definations.Count; i++)
                {
                    if (Definations[i].DefinationTypeId == 40039)
                    {
                        Condition.Add(Definations[i]);
                    }
                    else if (Definations[i].DefinationTypeId == 90066)
                    {
                        Action.Add(Definations[i]);
                    }
                }
                ViewBag.Condition = Condition;
                ViewBag.Action = Action;
                var result = KPI_BL.ThresholdList("Get_Threshold", Convert.ToInt64(Id));
                return PartialView("~/Areas/Project/Views/Defination/_Threshold.cshtml", result);
            }
            catch (Exception ex)
            {
                return PartialView("<b>" + ex + "</b>");
            }
        }
        [HttpPost]
        public ActionResult SaveThreshold(List<PM_Threshold> thr)
        {
            try
            {
                bool result = KPI_BL.Insert("Insert_Threshold", thr, Convert.ToString(ViewBag.UserId));
                return Json(new { Status = result, error = "none" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Status = false, error = ex }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
       
    }
}