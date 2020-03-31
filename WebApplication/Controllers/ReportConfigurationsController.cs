using SWI.AirView.Common;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace SWI.AirView.Controllers
{
    /*----MoB!----*/
    [IsLogin]
    public class ReportConfigurationsController : Controller
    {
        // GET: ReportConfigurations
        public ActionResult New(string id="0")
        {
            

            SelectedList sl = new SelectedList();
            UserClientsBL ucb = new UserClientsBL();
            ViewBag.UserClients = ucb.SelectedList("byUserId",Convert.ToString( ViewBag.UserId));

            AD_DefinationBL db = new AD_DefinationBL();
            ViewBag.Cities = db.SelectedList("UserCities", Convert.ToString(ViewBag.UserId));
            var ReportTypes = db.SelectedList("ReportTypes");
           
            ViewBag.ReportTypes = ReportTypes;
            if (ReportTypes.Count>0 && Convert.ToInt32(id)==0)
            {
                id = ReportTypes.FirstOrDefault().Value;
            }
           
            ViewBag.SelectedReport = id;

            ViewBag.Scopes = db.SelectedList("byDefinationType", "Scope");

            return View();
        }

        [HttpPost]
        public ActionResult New(Int64 ReportId,string CityId, Int64 ClientId, Int64 ScopeId, int[] KeyId, string[] KeyValue,string[] fontColor,long ProjectId=0)
        {
            try
            {
                List<string> cities = CityId.Split(',').ToList();
                foreach (var city in cities)
                {

                    DataTable Conf = new DataTable();
                    #region Datatable Columns
                    Conf.Columns.AddRange(new DataColumn[9]
                    {
                    new DataColumn("ReportId", typeof (Int64)),
                    new DataColumn("ClientId", typeof (Int64)),
                    new DataColumn("CityId", typeof (Int64)),
                    new DataColumn("KeyValue", typeof (string)),
                    new DataColumn("KeyId", typeof (string)),
                    new DataColumn("isActive", typeof (bool)),
                    new DataColumn("fontColor", typeof (string)),
                    new DataColumn("ScopeId", typeof (Int64)),
                    new DataColumn("ProjectId", typeof (Int64)),
                    });
                    #endregion
                    #region Add Row In Conf
                    for (int i = 0; i < KeyId.Length; i++)
                    {
                        DataRow row;
                        row = Conf.NewRow();
                        row["ReportId"] = ReportId;
                        row["ClientId"] = ClientId;
                        row["CityId"] = Convert.ToInt64(city);
                        row["KeyId"] = KeyId[i];
                        row["KeyValue"] = (KeyValue[i] == "true") ? string.Empty : KeyValue[i];
                        row["isActive"] = (KeyValue[i] == "true") ? true : false;
                        row["fontColor"] = "000000";
                        row["ScopeId"] = ScopeId;
                        row["ProjectId"] = ProjectId;
                        Conf.Rows.Add(row);
                    }
                    #endregion


                    AD_ReportConfigurationDL rcd = new AD_ReportConfigurationDL();
                    rcd.Save(Conf);
                }
                TempData["msg_success"] = "Report configurations save successfully.";
            }
            catch (Exception ex)
            {

               TempData["msg_error"] =ex.Message;
            }
            
            return RedirectToAction("new");
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult NewTemplate(int id = 0) {

            
            AD_ReportConfigurationBL rcb = new AD_ReportConfigurationBL();
            List<AD_ReportConfiguration> rec = rcb.ToList("Definations", id.ToString());
            ViewBag.sections = rec.GroupBy(test => test.SectionId)
                              .Select(grp => grp.First())
                              .ToList();

            return PartialView("/Views/ReportConfigurations/_NewTemplate.cshtml", rec);
        }
    }
}