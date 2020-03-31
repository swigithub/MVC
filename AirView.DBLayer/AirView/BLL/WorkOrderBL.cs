using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/

    public class WorkOrderBL
    {


        WorkOrderDL wod = new WorkOrderDL();

        public bool Insert(string filter, Workorder wo, List<Workorder> wolst, Int64 UserId, string IMEI)
        {
            try
            {
              
                dbDataTable ddt = new dbDataTable();
                DataTable wodt = ddt.Tb_AV_Workorder();

            
                if (wolst != null)
                {
                    #region Add Rows

                    for (int i = 0; i < wolst.Count; i++)
                    {
                        DataRow row = wodt.NewRow();
                        row["clusterCode"] = wolst[i].clusterId;  //wo.clusterCode;
                        row["Region"] = wo.Region;
                        row["Market"] = wo.Market;
                        row["Client"] = wo.Client;
                        row["siteCode"] = wo.siteCode;
                        row["siteLatitude"] = wo.siteLatitude;
                        row["siteLongitude"] = wo.siteLongitude;
                        row["Description"] = wo.Description;
                        row["sectorCode"] = wolst[i].sectorCode;
                        row["networkMode"] = wolst[i].networkMode; //wolst[i].networkMode;
                        row["Scope"] = wo.Scope;
                        row["Band"] = wolst[i].Band;
                        row["Carrier"] = wolst[i].Carrier;
                        row["Antenna"] = wolst[i].Antenna;
                        row["BeamWidth"] = wolst[i].BeamWidth;
                        row["VerticalBeamWidth"] = wolst[i].VerticalBeamWidth;
                        row["Azimuth"] = wolst[i].Azimuth;
                        row["PCI"] = wolst[i].PCI;
                        row["ReceivedOn"] = wo.ReceivedOn;
                        row["BandWidth"] = wolst[i].BandWidth;
                        row["CellId"] = wolst[i].CellId;
                        row["RFHeight"] = wolst[i].RFHeight;
                        row["MTilt"] = wolst[i].MTilt;
                        row["ETilt"] = null;
                        row["SiteName"] = wo.SiteName;
                        row["SiteTypeId"] = wo.SiteTypeId;
                        row["SiteClassId"] = wo.SiteClassId;
                        row["SiteAddress"] = wo.SiteAddress;
                        //project
                        row["ProjectId"] = wo.ProjectId;
                        row["MRBTS"] = wolst[i].MRBTS;
                        row["RevisionId"] = wo.RevisionId;
                        row["RedriveCount"] = wo.RedriveCount;
                        row["SiteId"] = wo.SiteId;
                        row["SectorLatitude"] = wolst[i].SectorLatitude;
                        row["SectorLongitude"] = wolst[i].SectorLongitude;
                        row["SectorId"] = wolst[i].SectorId;
                        ///cluster custom
                        row["SiteClusterId"] = wolst[i].SiteClusterId;
                        row["ClusterName"] = "";
                        row["CellFilePath"] = "";
                        ///TSS   
                        row["SiteSurveyId"] = wolst[i].SiteSurveyId;
                        row["SurveyId"] = wolst[i].SurveyId;


                        wodt.Rows.Add(row);
                    }



                    #endregion
                }
              
                wod.Insert(filter, wodt, UserId, IMEI);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        #region clusterworkorder

        public bool Insert(string filter, Workorder wo, List<Workorder> wolst, Int64 UserId, string IMEI, string FilePath = "")
        {
            try
            {
                dbDataTable ddt = new dbDataTable();
                DataTable wodt = ddt.Tb_AV_Workorder();
                if (wolst != null)
                {
                    var wo2 = FilePath.Split(',');
                    #region Add Rows
                    for (int i = 0; i < wolst.Count; i++)
                    {
                        DataRow row = wodt.NewRow();
                        row["clusterCode"] = wolst[i].clusterId;  //wo.clusterCode;
                        row["Region"] = wo.Region;
                        row["Market"] = wo.Market;
                        row["Client"] = wo.Client;
                        row["siteCode"] = (wo.siteCode ?? string.Empty).ToString();
                        row["siteLatitude"] = wo.siteLatitude;
                        row["siteLongitude"] = wo.siteLongitude;
                        //project
                        row["ProjectId"] = wo.ProjectId;
                        row["Description"] = wo.Description;
                        row["sectorCode"] = wolst[i].sectorCode;
                        row["networkMode"] = wolst[i].networkMode; //wolst[i].networkMode;
                        row["Scope"] = wo.Scope;
                        row["Band"] = wolst[i].Band;
                        row["Carrier"] = wolst[i].Carrier;
                        row["Antenna"] = wolst[i].Antenna;
                        row["BeamWidth"] = wolst[i].BeamWidth;
                        row["VerticalBeamWidth"] = wolst[i].VerticalBeamWidth;
                        row["Azimuth"] = wolst[i].Azimuth;
                        row["PCI"] = wolst[i].PCI;
                        row["ReceivedOn"] = wo.ReceivedOn;
                        row["BandWidth"] = wolst[i].BandWidth;
                        row["CellId"] = wolst[i].CellId;
                        row["RFHeight"] = wolst[i].RFHeight;
                        row["MTilt"] = wolst[i].MTilt;
                        row["ETilt"] = wolst[i].ETilt;
                        row["SiteName"] = (wo.SiteName ?? string.Empty).ToString();
                        row["SiteTypeId"] = wo.SiteTypeId;
                        row["SiteClassId"] = wo.SiteClassId;
                        row["SiteAddress"] = wo.SiteAddress;
                        row["MRBTS"] = wolst[i].MRBTS;
                        row["RevisionId"] = wo.RevisionId;
                        row["RedriveCount"] = wo.RedriveCount;
                        row["SiteId"] = wo.SiteId;
                        row["SectorLatitude"] = wolst[i].SectorLatitude;
                        row["SectorLongitude"] = wolst[i].SectorLongitude;
                        row["SectorId"] = wolst[i].SectorId;
                        ///cluster custom
                        row["SiteClusterId"] = wolst[i].SiteClusterId;
                        row["ClusterName"] = wolst[i].clusterName;
                        row["CellFilePath"] = "/Content/AirViewLogs/" + wolst[0].ClientPrefix + "/" + wolst[i].clusterId + "/" + wolst[i].clusterName + "_" + wolst[i].networkmodename + ".csv";
                        ///TSS
                        row["SiteSurveyId"] = wolst[i].SiteSurveyId;
                        row["SurveyId"] = wolst[i].SurveyId;
                        wodt.Rows.Add(row);
                    }
                    #endregion
                }

                wod.Insert(filter, wodt, UserId, IMEI);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion
        public bool Insert(string filter, Int64 Count, string FilePath = "")
        {
            try
            {
                wod.Insert(filter, Count, FilePath);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<GetWorkOrder> Report(string Filter, string value1 = null, string value2 = null, string value3 = null)
        {
            DataTable dt = wod.Get(Filter, value1, value2, value3);
            List<GetWorkOrder> WorkOrders = new List<GetWorkOrder>();
            GetWorkOrder wo = new GetWorkOrder();

            if (dt != null && dt.Rows.Count > 0)
            {


                if (dt.Columns.Contains("SiteId"))
                {
                    DataTable TempDt = dt;
                    TempDt = dt.AsEnumerable()
                               .GroupBy(r => new { Col1 = r["SiteId"] })
                               .Select(g => g.OrderBy(r => r["SiteId"]).First())
                               .CopyToDataTable();
                    for (int i = 0; i < TempDt.Rows.Count; i++)
                    {
                        GetWorkOrderSite sit = new GetWorkOrderSite();

                        sit.SiteId = (TempDt.Columns.Contains("SiteId")) ? int.Parse(TempDt.Rows[i]["SiteId"].ToString()) : 0;

                        sit.SiteCode = (TempDt.Columns.Contains("SiteCode")) ? TempDt.Rows[i]["SiteCode"].ToString() : null;
                        sit.ClusterId = (TempDt.Columns.Contains("ClusterId")) ? int.Parse(TempDt.Rows[i]["ClusterId"].ToString()) : 0;

                        sit.TesterId = (TempDt.Columns.Contains("TesterId")) ? int.Parse(TempDt.Rows[i]["TesterId"].ToString()) : 0;
                        sit.Tester = (TempDt.Columns.Contains("Tester")) ? TempDt.Rows[i]["Tester"].ToString() : null;
                        sit.Latitude = (TempDt.Columns.Contains("Latitude")) ? double.Parse(TempDt.Rows[i]["Latitude"].ToString()) : 0;
                        sit.Longitude = (TempDt.Columns.Contains("Longitude")) ? double.Parse(TempDt.Rows[i]["Longitude"].ToString()) : 0;
                        sit.WoRefId = (TempDt.Columns.Contains("WoRefId")) ? TempDt.Rows[i]["WoRefId"].ToString() : null;
                        sit.WOStatus = (TempDt.Columns.Contains("WOStatus")) ? TempDt.Rows[i]["WOStatus"].ToString() : null;
                        sit.ColorCode = (TempDt.Columns.Contains("ColorCode")) ? TempDt.Rows[i]["ColorCode"].ToString() : null;

                        sit.ClientId = (dt.Columns.Contains("ClientId")) ? int.Parse(TempDt.Rows[i]["ClientId"].ToString()) : 0;
                        sit.Client = (dt.Columns.Contains("Client")) ? TempDt.Rows[i]["Client"].ToString() : null;
                        sit.Scope = (dt.Columns.Contains("Scope")) ? TempDt.Rows[i]["Scope"].ToString() : null;
                        sit.Market = (dt.Columns.Contains("Market")) ? TempDt.Rows[i]["Market"].ToString() : null;
                        sit.ScheduledOn = (dt.Columns.Contains("ScheduledOn")) ? TempDt.Rows[i]["ScheduledOn"].ToString() : null;

                        sit.SiteTypeId = (dt.Columns.Contains("SiteTypeId")) ? TempDt.Rows[i]["SiteTypeId"].ToString() : null;
                        sit.SiteType = (dt.Columns.Contains("SiteType")) ? TempDt.Rows[i]["SiteType"].ToString() : null;
                        sit.ProjectId = (dt.Columns.Contains("ProjectId")) ? TempDt.Rows[i]["ProjectId"].ToString() : null;

                        wo.Message = (TempDt.Columns.Contains("Message")) ? TempDt.Rows[i]["Message"].ToString() : null;

                        wo.Site.Add(sit);

                    }

                }
                else
                {

                    wo.Message = (dt.Columns.Contains("Message")) ? dt.Rows[0]["Message"].ToString() : null;
                    WorkOrders.Add(wo);
                }

                #region Sectors
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int SiteId = (dt.Columns.Contains("SiteId")) ? int.Parse(dt.Rows[i]["SiteId"].ToString()) : 0;
                    var Site = wo.Site.Where(m => m.SiteId == SiteId).FirstOrDefault();
                    if (Site != null)
                    {
                        GetWorkOrderSector sec = new GetWorkOrderSector();

                        sec.SectorId = (dt.Columns.Contains("SectorId")) ? int.Parse(dt.Rows[i]["SectorId"].ToString()) : 0;
                        sec.SectorCode = (dt.Columns.Contains("SectorCode")) ? dt.Rows[i]["SectorCode"].ToString() : null;

                        sec.NetTypeId = (dt.Columns.Contains("NetTypeId")) ? int.Parse(dt.Rows[i]["NetTypeId"].ToString()) : 0;
                        sec.NetType = (dt.Columns.Contains("NetType")) ? dt.Rows[i]["NetType"].ToString() : null;

                        sec.BandId = (dt.Columns.Contains("BandId")) ? int.Parse(dt.Rows[i]["BandId"].ToString()) : 0;
                        sec.Band = (dt.Columns.Contains("Band")) ? dt.Rows[i]["Band"].ToString() : null;


                        sec.CarrierId = (dt.Columns.Contains("CarrierId")) ? int.Parse(dt.Rows[i]["CarrierId"].ToString()) : 0;
                        sec.Carrier = (dt.Columns.Contains("Carrier")) ? dt.Rows[i]["Carrier"].ToString() : null;


                        sec.ScopeId = (dt.Columns.Contains("ScopeId")) ? int.Parse(dt.Rows[i]["ScopeId"].ToString()) : 0;
                        sec.Scope = (dt.Columns.Contains("Scope")) ? dt.Rows[i]["Scope"].ToString() : null;
                        sec.Antenna = (dt.Columns.Contains("Antenna")) ? dt.Rows[i]["Antenna"].ToString() : null;
                        sec.BeamWidth = (dt.Columns.Contains("BeamWidth")) ? dt.Rows[i]["BeamWidth"].ToString() : null;
                        sec.Azimuth = (dt.Columns.Contains("Azimuth")) ? dt.Rows[i]["Azimuth"].ToString() : null;
                        sec.PCI = (dt.Columns.Contains("PCI")) ? dt.Rows[i]["PCI"].ToString() : null;
                        sec.Latitude = (dt.Columns.Contains("SectorLatitude")) ? DataType.ToDouble(dt.Rows[i]["SectorLatitude"].ToString()) : 0;
                        sec.Longitude = (dt.Columns.Contains("SectorLongitude")) ? DataType.ToDouble(dt.Rows[i]["SectorLongitude"].ToString()) : 0;
                        sec.RecieverDistance = (dt.Columns.Contains("RecieverDistance")) ? DataType.ToDouble(dt.Rows[i]["RecieverDistance"].ToString()) : 0;
                        sec.InnerDistance = (dt.Columns.Contains("InnerDistance")) ? DataType.ToDouble(dt.Rows[i]["InnerDistance"].ToString()) : 0;
                        sec.OuterDistance = (dt.Columns.Contains("OuterDistance")) ? DataType.ToDouble(dt.Rows[i]["OuterDistance"].ToString()) : 0;

                        sec.CellId = (dt.Columns.Contains("CellId")) ? dt.Rows[i]["CellId"].ToString() : null;
                        sec.RFHeight = (dt.Columns.Contains("RFHeight")) ? DataType.ToDouble(dt.Rows[i]["RFHeight"].ToString()) : 0;
                        sec.AntennaDownTilt = (dt.Columns.Contains("AntennaDownTilt")) ? DataType.ToDouble(dt.Rows[i]["AntennaDownTilt"].ToString()) : 0;
                        sec.VerticalBeamwidth = (dt.Columns.Contains("VerticalBeamwidth")) ? DataType.ToDouble(dt.Rows[i]["VerticalBeamwidth"].ToString()) : 0;

                        Site.Sector.Add(sec);

                    }


                }


                #endregion


            }
            WorkOrders.Add(wo);
            return WorkOrders;
        }

        public List<AD_ClusterScheduleVM> Report1(string Filter, string value1, string value2 = null, string value3 = null)
        {
            DataTable dt = wod.Get(Filter, value1.ToString(), value2, value3);
            return dt.ToList<AD_ClusterScheduleVM>();
        }

        public WoStatusCheck StatusCheck(string value1 = null, string value2 = null, string value3 = null)
        {

            DataTable dt = wod.Get("Status_Check", value1, value2, value3);

            return dt.ToList<WoStatusCheck>().FirstOrDefault();


        }


        public List<Workorder> GetWO(string value1 = null, string value2 = null, string value3 = null)
        {
            AV_SitesDL sd = new AV_SitesDL();
            List<Workorder> wolst = new List<Workorder>();

            DataSet ds = sd.GetDataSet("SiteWithSectors", value1);
            if (ds.Tables.Count > 0)
            {
                DataTable Sit = ds.Tables[0];

                AV_Site s = new AV_Site();

                var rec = Sit.ToList<AV_Site>();
                if (rec.Count > 0)
                {
                    s = rec.FirstOrDefault();
                }

                DataTable sec = ds.Tables[1];
                Workorder wo;
                for (int i = 0; i < sec.Rows.Count; i++)
                {
                    wo = new Workorder();
                    wo.SiteId = s.SiteId;
                    wo.siteCode = s.SiteCode;
                    wo.siteLatitude = s.Latitude.ToString();
                    wo.siteLongitude = s.Longitude.ToString();
                    wo.Client = s.ClientId.ToString();
                    wo.Scope = s.ScopeId.ToString();
                    wo.SiteAddress = s.SiteAddress;
                    wo.SiteName = s.SiteName;
                    wo.SiteTypeId = s.SiteTypeId.ToString();
                    wo.SiteClassId = s.SiteClassId;
                    wo.Description = s.Description;
                    wo.clusterCode = "-";
                    wo.sectorCode = sec.Rows[i]["SectorCode"].ToString();
                    wo.networkMode = sec.Rows[i]["NetworkModeId"].ToString();
                    wo.Band = sec.Rows[i]["BandId"].ToString();
                    wo.Carrier = sec.Rows[i]["CarrierId"].ToString();
                    wo.BandWidth = (!string.IsNullOrEmpty(sec.Rows[i]["BandWidth"].ToString())) ? sec.Rows[i]["BandWidth"].ToString() : "0";
                    wo.Antenna = sec.Rows[i]["Antenna"].ToString();
                    wo.BeamWidth = sec.Rows[i]["BeamWidth"].ToString();
                    wo.Azimuth = sec.Rows[i]["Azimuth"].ToString();
                    wo.PCI = sec.Rows[i]["PCI"].ToString();
                    wo.MRBTS = sec.Rows[i]["MRBTS"].ToString();
                    wo.RFHeight = DataType.ToInt32(sec.Rows[i]["RFHeight"].ToString());
                    wo.MTilt = DataType.ToInt32(sec.Rows[i]["MTilt"].ToString());
                    wo.ETilt = DataType.ToInt32(sec.Rows[i]["ETilt"].ToString());
                    wo.CellId = sec.Rows[i]["CellId"].ToString();

                    wolst.Add(wo);
                }


            }

            return wolst;


        }


        public List<GetWorkExport> Export(string SiteId)
        {
            DataTable dt = wod.Get("WorkOrderExport", SiteId);
            List<GetWorkExport> WorkOrders = new List<GetWorkExport>();
            GetWorkExport wo;

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    wo = new GetWorkExport();
                    wo.clusterCode = dt.Rows[i]["clusterCode"].ToString();
                    wo.Region = dt.Rows[i]["Region"].ToString();
                    wo.Market = dt.Rows[i]["Market"].ToString();
                    wo.Client = dt.Rows[i]["Client"].ToString();
                    wo.siteCode = dt.Rows[i]["siteCode"].ToString();
                    wo.SiteName = dt.Rows[i]["SiteName"].ToString();
                    wo.Project = dt.Rows[i]["ProjectName"].ToString();
                    wo.SiteType = dt.Rows[i]["siteType"].ToString();
                    wo.SiteClass = dt.Rows[i]["SiteClass"].ToString();
                    wo.CellId = dt.Rows[i]["CellId"].ToString();
                    wo.siteLatitude = dt.Rows[i]["siteLatitude"].ToString();
                    wo.siteLongitude = dt.Rows[i]["siteLongitude"].ToString();
                    wo.Description = dt.Rows[i]["Description"].ToString();
                    wo.Address = dt.Rows[i]["SiteAddress"].ToString();
                    wo.sectorCode = dt.Rows[i]["sectorCode"].ToString();
                    wo.networkMode = dt.Rows[i]["networkMode"].ToString();
                    wo.Scope = dt.Rows[i]["Scope"].ToString();
                    wo.Band = dt.Rows[i]["Band"].ToString();
                    wo.Carrier = dt.Rows[i]["Carrier"].ToString();
                    wo.Antenna = dt.Rows[i]["Antenna"].ToString();
                    wo.RFHeight = dt.Rows[i]["RFHeight"].ToString();
                    wo.BeamWidth = dt.Rows[i]["BeamWidth"].ToString();
                    wo.VBeamWidth = dt.Rows[i]["VBeamWidth"].ToString();
                    wo.AntennaDowntilt = dt.Rows[i]["AntennaDowntilt"].ToString();
                    wo.Azimuth = dt.Rows[i]["Azimuth"].ToString();
                    wo.PCI = dt.Rows[i]["PCI"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["ReceivedOn"].ToString()))
                    {
                        wo.ReceivedOn = Convert.ToDateTime(dt.Rows[i]["ReceivedOn"].ToString());

                    }


                    WorkOrders.Add(wo);
                }
            }


            return WorkOrders;
        }



        public List<AV_Site> Search(string value1 = null, string value2 = null, string value3 = null)
        {
            DataTable dtbl = new DataTable();
            dtbl = wod.Get("Search", value1, value2, value3);
            if (dtbl != null)
            {
                List<AV_Site> WorkOrders = dtbl.ToList<AV_Site>();
                return WorkOrders;
            }
            return null;
        }

        //public List<AV_Site> ListIsActive(string value1 = null, string value2 = null, string value3 = null)
        //{
        //    DataTable dtbl = new DataTable();
        //    dtbl = wod.Get("Search", value1, value2, value3);
        //    List<AV_Site> WorkOrders = dtbl.ToList<AV_Site>();

        //    return WorkOrders;
        //}
   
        public bool ChangeFolderName(Workorder wo,List<Workorder>wolst)
        {
            try {
                List<string> Newpath = new List<string>();
                List<string> OldPath = new List<string>();
                AD_DefinationBL db = new AD_DefinationBL();
                int x = 0;
                string ClientPrefix = "";
                string networkmode = "" ;
                string carriors = "";
                string bands = "";
                var Newtworkmode= db.SelectedList("NetworkModes", null, "-NetworkMode-");
                var Bands = db.ToList("Bands");
                var Carriers = db.ToList("Carriers");
                List<AV_Sector> Sectors = new List<AV_Sector>();
              List<Changefoldername> t = new List<Changefoldername>();
                List<Changefoldername> oldt = new List<Changefoldername>();

                AV_SitesBL sb = new AV_SitesBL();
                AV_Site Site = new AV_Site();
                sb.SiteWithSectors(Convert.ToInt32(wo.SiteId), ref Site, ref Sectors);
               
                for(int i=0;i<1;i++)
                {
                        ClientPrefix = wolst[0].ClientPrefix;
                }



                    var dt = wod.GetSiteBands("Get_All", wo.SiteId);
                if (wolst != null)
                {

                        if (wo.siteCode.ToString() == Site.SiteCode)
                        {
                        foreach (var row in Sectors)
                        {
                            foreach (var netmod in Newtworkmode)
                            {
                                if (netmod.Value == row.NetworkModeId.ToString())
                                {
                                    networkmode = netmod.Text;
                                }
                            }
                            foreach (var b in Bands)
                            {
                                if (b.DefinationId == row.BandId)
                                {
                                    bands = b.DefinationName;
                                }
                            }
                            foreach (var c in Carriers)
                            {
                                if (c.DefinationId == row.CarrierId)
                                {
                                    carriors = c.DefinationName;
                                }
                            }
                            var oldp = "/Content/AirViewLogs/" + ClientPrefix + "/" + Site.SiteCode + '/' + networkmode + "_" + bands + "_" + carriors + "";
                            Changefoldername cf = new Changefoldername();
                            cf.Id = Convert.ToInt64(row.SectorId);
                            cf.Path = oldp;
                            oldt.Add(cf);
                        }
                        foreach (var item in wolst)
                            {


                                foreach (var netmod in Newtworkmode)
                                {
                                    if (netmod.Value == item.networkMode)
                                    {
                                        networkmode = netmod.Text;
                                    }
                                }
                                foreach (var b in Bands)
                                {
                                    if (b.DefinationId.ToString() == item.Band)
                                    {
                                        bands = b.DefinationName;
                                    }
                                }
                                foreach (var c in Carriers)
                                {
                                    if (c.DefinationId.ToString() == item.Carrier)
                                    {
                                        carriors = c.DefinationName;
                                    }
                                }


                                var p = "/Content/AirViewLogs/" + ClientPrefix + "/" + wo.siteCode.ToString() + '/' + networkmode + "_" + bands + "_" + carriors + "";
                                foreach (var itm in Sectors)
                                {
                                    if (itm.SectorId == item.SectorId)
                                    {
                                        Changefoldername cfn = new Changefoldername();
                                        cfn.Id = Convert.ToInt64(item.SectorId);
                                        cfn.Path = p;
                                        t.Add(cfn);
                                        break;
                                    }
                                }
                            }

                        List<long> RemoveLayesId = new List<long>();
                        foreach (var itme in t)
                        {
                            foreach (var tm in oldt)
                            {
                                if (itme.Id == tm.Id && itme.Path == tm.Path)
                                {
                                    RemoveLayesId.Add(itme.Id);
                                }
                            }
                        }
                        foreach (var r in RemoveLayesId)
                        {
                            var obj = t.Where(xx => xx.Id == r).FirstOrDefault();
                            var obj1 = oldt.Where(xx => xx.Id == r).FirstOrDefault();
                            t.Remove(obj);
                            oldt.Remove(obj1);
                        }
                        for (int i = 0; i < t.Count; i++)
                        {

                            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(oldt[i].Path));
                            if (exists)
                            {
                                bool exists1 = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(t[i].Path));
                                if (!exists1)
                                {
                                    t[i].Path = HttpContext.Current.Server.MapPath(t[i].Path);
                                    oldt[i].Path = HttpContext.Current.Server.MapPath(oldt[i].Path);
                                    Directory.CreateDirectory(t[i].Path);
                                    DirectoryInfo dirInfo = new DirectoryInfo(t[i].Path);
                                    int xx = 0;
                                    int y = 0;
                                    List<String> MyFiles = Directory
                                                       .GetFiles(oldt[i].Path, "*.*", SearchOption.AllDirectories).ToList();

                                    foreach (string file in MyFiles)
                                    {
                                        FileInfo mFile = new FileInfo(file);
                                        // to remove name collisions
                                        if (new FileInfo(dirInfo + "\\" + mFile.Name).Exists == false)
                                        {
                                            mFile.MoveTo(dirInfo + "\\" + mFile.Name);
                                            xx++;
                                        }
                                    }
                                    //Directory.CreateDirectory(HttpContext.Current.Server.MapPath(t[i].Path));
                                    //      }
                                    //  System.IO.Directory.Move(HttpContext.Current.Server.MapPath(oldt[i].Path), HttpContext.Current.Server.MapPath(t[i].Path));
                                }
                                else
                                {
                                    t[i].Path = HttpContext.Current.Server.MapPath(t[i].Path);
                                    oldt[i].Path = HttpContext.Current.Server.MapPath(oldt[i].Path);
                                    List<String> Files = Directory
                                                           .GetFiles(oldt[i].Path, "*.*", SearchOption.AllDirectories).ToList();



                                    if (Files.Count > 0)
                                    {
                                        var MyFiles = Directory
                                                   .GetFiles(oldt[i].Path, "*.*", SearchOption.AllDirectories).ToList();

                                        foreach (string file in MyFiles)
                                        {
                                            FileInfo mFile = new FileInfo(file);
                                            // to remove name collisions
                                            if (new FileInfo(t[i].Path + "\\" + mFile.Name).Exists == false)
                                            {
                                                mFile.MoveTo(t[i].Path + "\\" + mFile.Name);

                                            }
                                        }
                                    }
                                }

                            }
                        }


                    }
                    else
                        {                         
                            var oldp = "/Content/AirViewLogs/" + ClientPrefix + "/" + Site.SiteCode + '/' ;
                            var p = "/Content/AirViewLogs/" + ClientPrefix + "/" + wo.siteCode.ToString() + '/';
                            var newPath = HttpContext.Current.Server.MapPath(p);
                            var oldPath = HttpContext.Current.Server.MapPath(oldp);
                            DirectoryCopy(oldPath, newPath, true);
                        clearFolder(oldPath);
                        DeleteFolder(oldPath);
                    }
                }
                      
            return true;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public void DeleteFolder(string FolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(FolderName);
                dir.Delete();
            
        }
        public void clearFolder(string FolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(FolderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                clearFolder(di.FullName);
                di.Delete();
            }
        }
        public void DirectoryCopy(
            string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

    }
}
