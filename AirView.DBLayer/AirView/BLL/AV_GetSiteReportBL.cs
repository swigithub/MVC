using AirView.DBLayer.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.BLL
{
   public class AV_GetSiteReportBL
    {
        AV_GetSiteReportDL srd = new AV_GetSiteReportDL();
        public List<AV_SiteTestSummary> ToList(string filter, Int64 SiteId )
        {
            DataTable dt = srd.Get(filter, SiteId);
            List<AV_SiteTestSummary> conf = dt.ToList<AV_SiteTestSummary>();
            return conf;
        }


        public KeyValuePair<List<AV_SiteTestSummary>,DataTable> NIReport(Int64 SiteId,ref List<SiteReportPlotVM>  PlotData,ref List<AD_ReportConfiguration> rptConf,string kmlPath)
        {
            DataSet ds = srd.GetDs("NIReport", SiteId);
            List<AV_SiteTestSummary> smry = new List<AV_SiteTestSummary>();
            if (ds != null && ds.Tables.Count > 0)
            {
                AV_SiteTestSummaryBL stb = new AV_SiteTestSummaryBL();
                smry = stb.DataTableToList(ds.Tables[0]); // ds.Tables[0].ToList<AV_SiteTestSummary>();
               
                if (ds.Tables.Count>0)
                {
                    SiteReportPlotVM plot;
                    DataTable Lagend = ds.Tables[1];
                    for (int i = 0; i < Lagend.Rows.Count; i++)
                    {
                        plot = new SiteReportPlotVM();
                        plot.PCI = Lagend.Rows[i]["PciId"].ToString();
                        plot.plotColorName = Lagend.Rows[i]["pciColor"].ToString();
                        plot.TestType = Lagend.Rows[i]["TestType"].ToString();
                        PlotData.Add(plot);

                    }

                }
                if (ds.Tables.Count>1)
                {
                    DataTable conf = ds.Tables[2];
                    rptConf = conf.ToList<AD_ReportConfiguration>();
                }

                if (ds.Tables.Count >2)
                {
                    DataTable dtkml = ds.Tables[3];
                    KML km = new KML();
                    string kml = string.Empty;
                    string PciKml = string.Empty;

                    //var smrFirstRow = smry.Where(m=>m.NetworkMode=="LTE").FirstOrDefault();
                    var smrFirstRow = smry.FirstOrDefault();
                    if (smrFirstRow!=null)
                    {
                       // string Layer = smrFirstRow.Site + "\\" + smrFirstRow.NetworkMode + "_" + smrFirstRow.Band + "_" + smrFirstRow.Carrier;
                        for (int i = 0; i < dtkml.Rows.Count; i++)
                        {
                            string Layer= dtkml.Rows[i]["Site"].ToString() + "\\" + dtkml.Rows[i]["NetworkMode"].ToString() + "_" + dtkml.Rows[i]["Band"].ToString() + "_" + dtkml.Rows[i]["Carrier"].ToString();
                            km.SaveKml(dtkml.Rows[i]["pciPlot"].ToString(), "pci", kmlPath + smrFirstRow.ClientPrefix+ "\\" + Layer);
                        }
                    }

                    

                }
            }
                
            return new KeyValuePair<List<AV_SiteTestSummary>,DataTable>(smry,ds.Tables[4]) ;
        }

        public List<ReportTimeStamp> ToListNI(string Filter, Int64 SiteId = 0)
        {
            DataTable dt = srd.GetDataTable(Filter, SiteId);
            return dt.ToList<ReportTimeStamp>();
        }
        public List<ReportTimeStamp> ToListNIDataSet(string Filter, Int64 SiteId = 0)
        {
            try
            {
                DataSet dt = srd.GetDataSet(Filter, SiteId);
                List<ReportTimeStamp> ret = new List<ReportTimeStamp>();
                if (dt.Tables[0].Rows.Count > 0)
                {
                    ret = dt.Tables[0].ToList<ReportTimeStamp>();
                }

                ret[0].Tables = dt.Tables[1].ToList<SectorTables>();
                return ret;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
