using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SWI.Libraries.AirView.BLL
{
    public class AV_OptimizationBL
    {
        AV_RfOptimizationDL dal = new AV_RfOptimizationDL();

        public List<AV_Sector> GetSector(string Filter, Int64 SiteId, string SiteCode, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 ScopeId) {
            List<AV_Sector> secList = new List<AV_Sector>();
            AV_SectorDL sectorDL = new AV_SectorDL();
            var data = sectorDL.Get("GetSectors", SiteId.ToString(), NetworkModeId.ToString(), BandId.ToString(), CarrierId.ToString(), ScopeId.ToString());

             foreach (DataRow item in data.Rows)
            {
                AV_Sector s = new AV_Sector();
                //s.Id = Convert.ToInt64(item["SectorId"].ToString());
                s.SectorCode = item["SectorCode"].ToString();
                s.sectorColor = item["sectorColor"].ToString();
                s.BeamWidth = Convert.ToInt32(item["BeamWidth"].ToString());
                s.Azimuth = Convert.ToInt32(item["Azimuth"].ToString());
                s.Latitude = Convert.ToDouble(item["SectorLatitude"].ToString());
                s.Longitude = Convert.ToDouble(item["SectorLongitude"].ToString());
                s.PCI = Convert.ToInt32(item["PCI"].ToString());
                secList.Add(s);

            }
            return secList;
        }

        public List<AV_RFPlotLegends> GetRfLegend(Int64 siteId,Int64 networkModeId)
        {
            var query = dal.GetrFPlot("RFLegends_by_NetMode", siteId, networkModeId);
            var rfList = new List<AV_RFPlotLegends>();

            foreach (DataRow item in query.Rows)
            {
                AV_RFPlotLegends rf = new AV_RFPlotLegends();
                rf.PlotType = item["PlotType"].ToString();
                rf.PlotType = rf.PlotType.Replace("_PLOT", "");
                rf.rangeFrom = float.Parse(item["rangeFrom"].ToString());
                rf.rangeTo = float.Parse(item["rangeTo"].ToString());
                rf.rangeColor = item["rangeColor"].ToString();
                rfList.Add(rf);
            }

            return rfList;
        }


        public List<AV_Test> GetAllTest(string siteId,string networModeId,string BandId,string CarrierId)
        {
            AV_SitesBL siteBL = new AV_SitesBL();
            List<AV_Test> listTest = new List<AV_Test>();
            var data = siteBL.ToDataTable("GET_TEST_BY_LAYER", null, networModeId, BandId, CarrierId, null, null, null);

            foreach (DataRow item in data.Rows)
            {
                AV_Test test = new AV_Test();
                //test.Id = Convert.ToInt64(item["DefinationId"].ToString());
                test.Name = item["DefinationName"].ToString();
                test.UEId = item["UEId"].ToString();
                listTest.Add(test);
            }

            return listTest;
        }

        public List<SiteReportPlotVM> GetPCI(Int64 siteId )
        {
            var q = dal.GetPci(siteId);
            var pciList = new List<SiteReportPlotVM>();
            foreach (DataRow item in q.Rows)
            {
                SiteReportPlotVM pci = new SiteReportPlotVM();
                pci.PciId = item["PciId"].ToString();
                pci.pciColor = item["pciColor"].ToString();
                pciList.Add(pci);
            }

            return pciList;
        }

        public void CreateKml(string Filter, Int64 SiteId, string SiteCode, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 ScopeId,string address)
        {

            DataTable q = dal.CreateKml("NetLayerReport", SiteId, "CH92486A", NetworkModeId, BandId, CarrierId, 36);
            string pci = null;
            string ho = null;
            string cw = null;
            string rsrp = null;
            string ch = null;
            string rsrq = null;
            string ccw = null;
            string cinr = null;
            foreach (DataRow rows in q.Rows)
            {
                pci = rows["pciPlot"].ToString();
                ho = rows["hoPlot"].ToString();
                cw = rows["cwPlot"].ToString();
                rsrp = rows["rsrpPlot"].ToString();
                ch = rows["chPlot"].ToString();
                rsrq = rows["rsrqPlot"].ToString();
                ccw = rows["ccwPlot"].ToString();
                cinr = rows["sinrPlot"].ToString();


            }
            List<string> list = new List<string> { pci, ho, cw, rsrp, ch };

            System.IO.File.WriteAllText(address+"/PCI.kml", pci);
            System.IO.File.WriteAllText(address+"/CW.kml", cw);
            System.IO.File.WriteAllText(address+"/CCW.kml", ccw);
            System.IO.File.WriteAllText(address+"/HANDOVER.kml", rsrp);
            System.IO.File.WriteAllText(address+"/RSRP.kml", rsrp);
            System.IO.File.WriteAllText(address+"/RSRQ.kml", rsrq);
            System.IO.File.WriteAllText(address+"/CINR.kml", cinr);
            System.IO.File.WriteAllText(address+"/CH.kml", ch);

        }

        public List<RFOptimize> GetOptimizeData(string Filter, DataTable siteSector,long siteId,Int64 NetworkLayerId,string whereClause)
        {
            var query = dal.GetSiteSector(Filter, siteSector,siteId,NetworkLayerId,whereClause);
            return null;
        }
      
    }
    }

