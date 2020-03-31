using OfficeOpenXml;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWI.AirView.Controllers
{
    [IsLogin, ErrorHandling]
    public class SiteTestLogController : Controller
    {
        // GET: SiteTestLog

        public ActionResult Export(Int64 SiteId, Int64 NetworkmodeId, Int64 BandId, Int64 CarrierId, Int64 ScopeId)
        {
            MemoryStream memStream=new MemoryStream();
            string FileName = DateTime.Now.ToString();

            AV_SiteTestLogDL stld = new AV_SiteTestLogDL();
            DataTable dt = stld.Get("Export", SiteId, NetworkmodeId, BandId, CarrierId, ScopeId);
            if (dt!=null && dt.Rows.Count > 0)
            {

                var temp = dt.AsEnumerable()
                                  .GroupBy(r => new { Col1 = r["TestType"] })
                                  .Select(g => g.OrderBy(r => r["TestType"]).First())
                                  .CopyToDataTable();
                dt.Columns.Remove("ActualNetMode");
                dt.Columns.Remove("ActualBand");
                dt.Columns.Remove("ActualCarrier");
                using (var package = new ExcelPackage())
                {
                    FileName = dt.Rows[0][4] + "_" + dt.Rows[0][2] + "_" + dt.Rows[0][3] + "_" + dt.Rows[0][5] + "_" + DateTime.Now.ToString();
                    for (int t = 0; t < temp.Rows.Count; t++)
                    {
                        string TestType = (temp.Columns.Contains("TestType")) ? temp.Rows[t]["TestType"].ToString() : "";


                        dt.DefaultView.RowFilter = "TestType='"+ TestType + "'";
                        DataTable CCW = (dt.DefaultView).ToTable();
                        CCW.Columns.Remove("TestType");
                        var Sheet1 = package.Workbook.Worksheets.Add(TestType);
                        for (int i = 0; i < CCW.Columns.Count; i++)
                        {
                            Sheet1.Cells[1, i + 1].Value = CCW.Columns[i].ColumnName;
                        }

                        for (int i = 0; i < CCW.Rows.Count; i++)
                        {
                            for (int j = 0; j < CCW.Columns.Count; j++)
                            {
                                Sheet1.Cells[i + 2, j + 1].Value = CCW.Rows[i][j].ToString();
                            }
                        }

                    }
                   
                        memStream = new MemoryStream(package.GetAsByteArray());
                   
                }
                return File(memStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
            }
            else
            {
                ViewBag.error = true;
                TempData["error"] = ViewBag.error;
                return Redirect("/Dashboard/Index"); //Redirect("/Dashboard/Index");
            }
            
            //using (var package = new ExcelPackage())
            //{
            //    // var CCW = rec.Where(m => m.TestType == "CCW").ToList().ToDataTable();



            //    if (dt.Rows.Count > 0)
            //    {
            //        FileName = dt.Rows[0][4] + "_" + dt.Rows[0][2] + "_" + dt.Rows[0][3] + "_" + dt.Rows[0][5] + "_" + DateTime.Now.ToString();


            //        #region CCW
            //        dt.DefaultView.RowFilter = "TestType='CCW'";
            //        DataTable CCW = (dt.DefaultView).ToTable();
            //        CCW.Columns.Remove("TestType");
            //        CCW.Columns.Remove("ActualNetMode");
            //        CCW.Columns.Remove("ActualBand");
            //        CCW.Columns.Remove("ActualCarrier");
            //        var Sheet1 = package.Workbook.Worksheets.Add("CCW");
            //        for (int i = 0; i < CCW.Columns.Count; i++)
            //        {
            //            Sheet1.Cells[1, i + 1].Value = CCW.Columns[i].ColumnName;
            //        }

            //        for (int i = 0; i < CCW.Rows.Count; i++)
            //        {
            //            for (int j = 0; j < CCW.Columns.Count; j++)
            //            {
            //                Sheet1.Cells[i + 2, j + 1].Value = CCW.Rows[i][j].ToString();
            //            }
            //        }
            //        #endregion

            //        #region CW
            //        dt.DefaultView.RowFilter = "TestType='CW'";
            //        DataTable CW = (dt.DefaultView).ToTable();
            //        CW.Columns.Remove("TestType");
            //        CW.Columns.Remove("ActualNetMode");
            //        CW.Columns.Remove("ActualBand");
            //        CW.Columns.Remove("ActualCarrier");
            //        // var CW = rec.Where(m => m.TestType == "CW").ToList().ToDataTable();
            //        var Sheet2 = package.Workbook.Worksheets.Add("CW");

            //        for (int i = 0; i < CW.Columns.Count; i++)
            //        {
            //            Sheet2.Cells[1, i + 1].Value = CW.Columns[i].ColumnName;
            //        }

            //        for (int i = 0; i < CW.Rows.Count; i++)
            //        {
            //            for (int j = 0; j < CW.Columns.Count; j++)
            //            {
            //                Sheet2.Cells[i + 2, j + 1].Value = CW.Rows[i][j].ToString();
            //            }
            //        }
            //        #endregion


            //        #region DL
            //        dt.DefaultView.RowFilter = "TestType='DL'";
            //        DataTable DL = (dt.DefaultView).ToTable();
            //        DL.Columns.Remove("TestType");
            //        DL.Columns.Remove("ActualNetMode");
            //        DL.Columns.Remove("ActualBand");
            //        DL.Columns.Remove("ActualCarrier");
            //        // var DL = rec.Where(m => m.TestType == "DL").ToList().ToDataTable();
            //        var Sheet3 = package.Workbook.Worksheets.Add("DL");


            //        for (int i = 0; i < DL.Columns.Count; i++)
            //        {
            //            Sheet3.Cells[1, i + 1].Value = DL.Columns[i].ColumnName;
            //        }

            //        for (int i = 0; i < DL.Rows.Count; i++)
            //        {
            //            for (int j = 0; j < DL.Columns.Count; j++)
            //            {
            //                Sheet3.Cells[i + 2, j + 1].Value = DL.Rows[i][j].ToString();
            //            }
            //        }
            //        #endregion

            //        #region UL
            //        dt.DefaultView.RowFilter = "TestType='UL'";
            //        DataTable UL = (dt.DefaultView).ToTable();
            //        UL.Columns.Remove("TestType");
            //        UL.Columns.Remove("ActualNetMode");
            //        UL.Columns.Remove("ActualBand");
            //        UL.Columns.Remove("ActualCarrier");
            //        //var UL = rec.Where(m => m.TestType == "UL").ToList().ToDataTable();
            //        var Sheet4 = package.Workbook.Worksheets.Add("UL");
            //        for (int i = 0; i < UL.Columns.Count; i++)
            //        {
            //            Sheet4.Cells[1, i + 1].Value = UL.Columns[i].ColumnName;
            //        }

            //        for (int i = 0; i < UL.Rows.Count; i++)
            //        {
            //            for (int j = 0; j < UL.Columns.Count; j++)
            //            {
            //                Sheet4.Cells[i + 2, j + 1].Value = UL.Rows[i][j].ToString();
            //            }
            //        }
            //        #endregion

            //        #region UL
            //        dt.DefaultView.RowFilter = "TestType='Ping'";
            //        DataTable Ping = (dt.DefaultView).ToTable();
            //        Ping.Columns.Remove("TestType");
            //        Ping.Columns.Remove("ActualNetMode");
            //        Ping.Columns.Remove("ActualBand");
            //        Ping.Columns.Remove("ActualCarrier");
            //        // var Ping = rec.Where(m => m.TestType == "Ping").ToList().ToDataTable();
            //        var Sheet5 = package.Workbook.Worksheets.Add("Ping");

            //        for (int i = 0; i < Ping.Columns.Count; i++)
            //        {
            //            Sheet5.Cells[1, i + 1].Value = Ping.Columns[i].ColumnName;
            //        }

            //        for (int i = 0; i < Ping.Rows.Count; i++)
            //        {
            //            for (int j = 0; j < Ping.Columns.Count; j++)
            //            {
            //                Sheet5.Cells[i + 2, j + 1].Value = Ping.Rows[i][j].ToString();
            //            }
            //        }
            //        #endregion
            //    }

            //    memStream = new MemoryStream(package.GetAsByteArray());
            //}

            //return File(memStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
        }

        public ActionResult TranferLogs(Int64 Id)
        {
            SitesBL sb = new SitesBL();
            List<BandVM> Bands = sb.GetSiteBands("", Id);
            ViewBag.Bands = Bands.Select(m => new { m.BandId, m.NetworkMode, m.LayerStatusId, m.BandName, m.Carrier, Text = m.NetworkMode + " " + m.BandName + " " + m.Carrier, Value = m.SiteCode + "_" + m.NetworkMode + "_" + m.BandName + "_" + m.Carrier }).ToList();
            AV_SectorBL secb = new AV_SectorBL();
            ViewBag.Sectors = secb.ToList("BySiteId", Id.ToString(), "0", "0", "0", "0").GroupBy(m => m.SectorCode).Select(grp => grp.First()).ToList();

            AD_DefinationBL db = new AD_DefinationBL();
            ViewBag.Tests = db.SelectedList("byDefinationType", "Test Type", null);
            AV_TestDL t = new AV_TestDL();
              //  ViewBag.count = t.CountTestsofsite("Count_Testslogs", Id, 0,"","","","");
            ViewBag.count = null;
            ViewBag.Tasks = new List<SelectListItem>() { new SelectListItem {Value="Summary",Text="Summary" }, new SelectListItem { Value = "Logs", Text = "Logs" }, new SelectListItem { Value = "Files", Text = "Files" } };
            ViewBag.SiteId = Id;
            return View();
        }

        [HttpPost]
        public ActionResult TranferLogs(Int64 SiteLayerId, Int64[] Sectors, string[] Tests, string[] Tasks, string SiteCode,int SiteId)
        {
            Response res = new Response();
            try
            {
                TransferSiteLogsDL tsd = new TransferSiteLogsDL();
                AV_SiteTestLogBL st = new AV_SiteTestLogBL();
                var Sector = string.Join(",", Sectors);
                AD_DefinationBL db = new AD_DefinationBL();
                ViewBag.Tests = db.SelectedList("byDefinationType", "Test Type", null);
                var Test = string.Join(",", Tests);
                var Task = string.Join(",", Tasks);
                st.TransferSiteLogs("Transfer_Testslogs", SiteId, SiteLayerId, Sector, Test, SiteCode, Task);
               foreach (var item in Tasks) {
                    if(item == "Files") { 
                     st.ChangeFolderName(SiteLayerId,SiteCode);
                    }
                }
                res.Status = "success";
                res.Message = "save successfully";
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