using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.AirView.Entities;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.BLL;
using System.Threading;
using SWI.Libraries.Common;

namespace WebApplication.Controllers
{
    [IsLogin]
    public class OptimizationsController : Controller
    {
        AV_OptimizationBL optimizeBL = new AV_OptimizationBL();
        List<AV_SiteTestLog> list = new List<AV_SiteTestLog>();
        Thread th;

        [OutputCache(Duration = 20, VaryByParam = "none")]
        public ActionResult Index(Int64 Id)
        {
            HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoStore();


            return View();
        }

        [Route("getAllTest")]
        public ActionResult GetAllTest(string siteId, string networkModeId, string BandId, string CarrierId)
        {
            var listTest = optimizeBL.GetAllTest(siteId, networkModeId, BandId, CarrierId);
            //AV_SitesBL siteBL = new AV_SitesBL();
            //var data = siteBL.Dtable("GET_TEST_BY_LAYER", null, null, null, null, null, null, null);

            //foreach (DataRow item in data.Rows)
            //{
            //    AV_Test test = new AV_Test();

            //    test.Name = item["DefinationName"].ToString();
            //    test.UEId = item["UEId"].ToString();
            //    listTest.Add(test);
            //}
            return Json(new { listTest }, JsonRequestBehavior.AllowGet);
        }

        [Route("getSectors")]
        public ActionResult GetSectors(Int64 siteId, string sitCode, Int64 networkModeId, Int64 BandId, Int64 CarrierId, Int64 scopeId)
        {

            var q = optimizeBL.GetSector("", siteId, sitCode, networkModeId, BandId, CarrierId, scopeId);

            return Json(q, JsonRequestBehavior.AllowGet);
            //return Request.CreateResponse(HttpStatusCode.OK, q);
        }

        [HttpPost, Route("search")]
        public ActionResult Search(AV_Search serach)
        {
            string address = System.Web.HttpContext.Current.Server.MapPath("~/files");
            th = new Thread(() => optimizeBL.CreateKml("", serach.SiteId, serach.SiteCode, serach.NetworkModeId, serach.BandId, serach.CarrierId, serach.ScopeId, address));

            //optimizeBL.CreateKml("", 398366, "CH92486A", serach.NetworkModeId[0], serach.BandId[0], serach.CarrierId[0], 36);
            th.Start();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [Route("getPci")]
        public ActionResult GetPci(Int64 siteId)
        {
            var pciList = optimizeBL.GetPCI(siteId);

            return Json(pciList, JsonRequestBehavior.AllowGet);
            //return Request.CreateResponse(HttpStatusCode.OK, pciList);
        }
        #region this for change the color of kml
        //[HttpPost]
        //[Route("changeColor")]
        //public ActionResult ChangeColor(ChangeColor color)
        //{
        //    color.NewVlue = color.NewVlue.Replace("#", "ff");
        //    color.OldValue = color.OldValue.Replace("#", "ff");
        //    string path = System.Web.Hosting.HostingEnvironment.MapPath("~/files/cta.kml");
        //    //Path.(AppDomain.CurrentDomain.BaseDirectory, @"~/files/cta.kml");
        //    var myDocument = new XmlDocument();
        //    myDocument.Load(path);
        //    var nodes = myDocument.GetElementsByTagName("Style");
        //    var resultNodes = new List<XmlNode>();
        //    foreach (XmlNode node in nodes)
        //    {
        //        if (node.Attributes != null && node.Attributes["id"] != null && node.Attributes["id"].Value == color.Id)
        //        {
        //            XmlNode q = node.FirstChild.FirstChild;
        //            q.InnerText = color.NewVlue;
        //            myDocument.Save(path);

        //        }
        //    }

        //    return Json(12, JsonRequestBehavior.AllowGet);

        //}
        #endregion

        public ActionResult GetrfPlot(Int64 siteId, Int64 networkModeId)
        {

            var rfList = optimizeBL.GetRfLegend(siteId, networkModeId);


            return Json(rfList, JsonRequestBehavior.AllowGet);
            //return Request.CreateResponse(HttpStatusCode.OK, new { rfList });

        }

        [HttpPost]
        public ActionResult SiteSector(List<RFOptimize> sector, List<RFOptimize> pci)
        {
            dbDataTable dbdt = new dbDataTable();
            DataTable dt = dbdt.List();
            List<RFOptimize> data = new List<RFOptimize>();
            if (pci != null && sector != null)
            {
                foreach (var p in pci)
                {
                    data.Add(p);
                }

                foreach (var s in sector)
                {
                    data.Add(s);
                }


                foreach (var item in data)
                {
                    myDataTable.AddRow(dt, "Value1", item.PCI, "Value2", item.pciColor, "Value3", item.sectorColor, "Value4", sector[0].NetworkLayerId, "Value5", sector[0].SiteId);
                }
                data = optimizeBL.GetOptimizeData("", dt, pci[0].SiteId, pci[0].NetworkLayerId,"");
            }
            else
            {

                foreach (var item in sector)
                {
                    myDataTable.AddRow(dt, "Value1", item.PCI, "Value2", item.pciColor, "Value3", item.sectorColor);
                }
                data = optimizeBL.GetOptimizeData("", dt, sector[0].SiteId, sector[0].NetworkLayerId,"");
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CollectedPCI(List<RFOptimize> pci, List<RFOptimize> sec)
        {
            dbDataTable dbdt = new dbDataTable();
            DataTable dt = dbdt.List();
            List<RFOptimize> data = new List<RFOptimize>();
            if (pci != null && sec != null)
            {
                foreach (var p in pci)
                {
                    data.Add(p);
                }

                foreach (var s in sec)
                {
                    data.Add(s);
                }

                foreach (var item in data)
                {
                    myDataTable.AddRow(dt, "Value1", item.PCI, "Value2", item.pciColor, "Value3", item.sectorColor);
                }
                data = optimizeBL.GetOptimizeData("", dt, pci[0].SiteId, pci[0].NetworkLayerId,"");
            }
            else
            {


                foreach (var item in pci)
                {
                    myDataTable.AddRow(dt, "Value1", item.PciId, "Value2", item.pciColor);
                }
                data = optimizeBL.GetOptimizeData("", dt, pci[0].SiteId, pci[0].NetworkLayerId,"");
            }


            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RFLegends(List<RFOptimize> rfLegends)
        {
            dbDataTable dbdt = new dbDataTable();
            DataTable dt = dbdt.List();

            foreach (var item in rfLegends)
            {
                myDataTable.AddRow(dt, "Value1", item.PlotType, "Value2", item.rangeColor, "Value3", item.rangeFrom, "Value4", item.rangeTo);
            }
            var data = optimizeBL.GetOptimizeData("", dt, rfLegends[0].SiteId, rfLegends[0].NetworkLayerId,"");

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}