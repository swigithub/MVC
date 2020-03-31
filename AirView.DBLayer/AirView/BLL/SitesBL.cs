using System;
using System.Collections.Generic;
using System.Linq;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using System.Data;
using System.Drawing;
using SWI.Libraries.Common;
using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.Fleet.DAL;
using AirView.DBLayer.Fleet.BLL;

namespace SWI.Libraries.AirView.BLL
{
    public class SitesBL
    {
        Conversion cnvtr = new Conversion();
        SitesDL sd = new SitesDL();
        public Int64 SaveSiteAndGetId(string Filter, string SiteId, string SiteCode, double Latitude, double Longitude, Int64 ClusterId, string ClientId, string Description, string Status, Int64 SubmittedById,string Market,DateTime ReceivedOn,string Scope,DataTable dt)
        {
            return sd.SaveNewSite(Filter,SiteId,SiteCode, Latitude, Longitude, ClusterId, ClientId, Description, Status, SubmittedById, Market, ReceivedOn, Scope, dt);
        }
        public Int64 SaveClustorAndGetId(string ClustorCode, string Region, string Market, string Client)
        {
            return sd.SaveNewClustor(ClustorCode, Region, Market, Client);
        }
       
        public List<NetworkMode> GetAllNetworkModesBandsCarriers()
        {
            List<NetworkMode> lstNetworkModes = new List<NetworkMode>();
            DataTable dt = sd.GetAllNetworkModesBandsCarriers();
            DataView view = new DataView(dt);

            DataTable tblNetworkModes = view.ToTable(true, new string[] { "NetworkModeId", "NetworkModeName" });

            NetworkMode nm;
            for (int i = 0; i < tblNetworkModes.Rows.Count; i++)
            {
                nm = new NetworkMode();
                List<Band> lstBands = new List<Band>();
                DataRow[] rows = dt.Select("NetworkModeId = '" + tblNetworkModes.Rows[i]["NetworkModeId"] + "'");
                Band loBand;
                for (int j = 0; j < rows.Count(); j++)
                {
                    List<Carrier> lstCarrier = new List<Carrier>();
                    loBand = new Band();
                    loBand.BandId = cnvtr.Int64(rows[j]["BandId"].ToString());
                    loBand.BandName = rows[j]["BandName"].ToString();

                    Carrier loCarrier;
                    for (int k = 0; k < rows.Count(); k++)
                    {
                        loCarrier = new Carrier();
                        loCarrier.CarrierId = cnvtr.Int64(rows[k]["CarrierId"].ToString());
                        loCarrier.CarrierName = rows[k]["Carrier"].ToString();
                        loBand.Carriers.Add(loCarrier);
                    }

                    lstBands.Add(loBand);
                }
                nm.NetworkModeId = cnvtr.Int64(tblNetworkModes.Rows[i]["NetworkModeId"].ToString());
                nm.NetworkModeName = tblNetworkModes.Rows[i]["NetworkModeName"].ToString();
                nm.Bands = lstBands;
                lstNetworkModes.Add(nm);

            }


            return lstNetworkModes;
        }

        public List<SectorsVM> GetSectors(string Filter, Int64 SiteId = 0, Int64 NetworkModeId = 0, Int64 BandId = 0, Int64 CarrierId = 0, Int64 ScopeId = 0)
        {
            List<SectorsVM> lstSectors = new List<SectorsVM>();

            DataTable dt = sd.GetSectors(Filter,  SiteId ,  NetworkModeId,  BandId ,  CarrierId , ScopeId);

            if (dt != null && dt.Rows.Count > 0)
            {
                SectorsVM Sec;
                foreach (DataRow dr in dt.Rows)
                {
                    Sec = new SectorsVM();
                    Sec.SectorCode = (dt.Columns.Contains("SectorCode")) ? dr["SectorCode"].ToString() : "";
                    Sec.Azimuth = (dt.Columns.Contains("Azimuth")) ? cnvtr.Double(dr["Azimuth"].ToString()) : 0;
                    Sec.PCI = (dt.Columns.Contains("PCI")) ? Convert.ToInt32(dr["PCI"]) : 0;
                    Sec.SectorId = (dt.Columns.Contains("SectorId")) ? Convert.ToInt32(dr["SectorId"]) : 0;
                    Sec.BeamWidth = (dt.Columns.Contains("BeamWidth")) ? Convert.ToInt32(dr["BeamWidth"]) : 0;
                    Sec.Antenna = (dt.Columns.Contains("Antenna")) ? Convert.ToString(dr["Antenna"]) : "";
                    Sec.NetworkMode.DefinationId= (dt.Columns.Contains("NetworkModeId")) ? Convert.ToInt32(dr["NetworkModeId"]) : 0;
                    Sec.PingKpi = Convert.ToDecimal(dt.Columns.Contains("PingKpi"));
                    Sec.DLKpi = Convert.ToDecimal(dt.Columns.Contains("DLKpi"));
                    Sec.ULKpi = Convert.ToDecimal(dt.Columns.Contains("ULKpi"));
                    Sec.AvgPing = Convert.ToDecimal(dt.Columns.Contains("AvgPing"));
                    Sec.MaxDL = Convert.ToDecimal(dt.Columns.Contains("MaxDL"));
                    Sec.MaxUL = Convert.ToDecimal(dt.Columns.Contains("MaxUL"));
                    Sec.NetworkMode.DefinationName = (dt.Columns.Contains("NetworkMode")) ? Convert.ToString(dr["NetworkMode"]) : "";
                    Sec.Scope.DefinationName = (dt.Columns.Contains("Scope")) ? Convert.ToString(dr["Scope"]) : "";


                    Sec.Carrier.DefinationId = (dt.Columns.Contains("CarrierId")) ? Convert.ToInt32(dr["CarrierId"]) : 0;
                    Sec.Carrier.DefinationName = (dt.Columns.Contains("Carrier")) ? Convert.ToString(dr["Carrier"]) : "";

                    Sec.Band.DefinationId = (dt.Columns.Contains("BandId")) ? Convert.ToInt32(dr["BandId"]) : 0;
                    Sec.Band.DefinationName = (dt.Columns.Contains("Band")) ? Convert.ToString(dr["Band"]) : "";

                    Sec.SiteCode = (ColumnExist(dt, "SiteCode")) ? dr["SiteCode"].ToString() : "";
                    Sec.ClientPrefix = (ColumnExist(dt, "ClientPrefix")) ? dr["ClientPrefix"].ToString() : "";
                    if (dt.Columns.Contains("ClientId") && !string.IsNullOrEmpty(dr["ClientId"].ToString()))
                    {
                        Sec.ClientId = Int64.Parse(dr["ClientId"].ToString());
                    }
                    if (dt.Columns.Contains("CityId") && !string.IsNullOrEmpty(dr["CityId"].ToString()))
                    {
                        Sec.CityId = Int64.Parse(dr["CityId"].ToString());
                    }



                    if (dt.Columns.Contains("PingStatus") &&  string.IsNullOrEmpty(dr["PingStatus"].ToString())==false)
                    {
                        Sec.TestResult.PingStatus = bool.Parse( dr["PingStatus"].ToString());
                    }
                    if (dt.Columns.Contains("DownlinkStatus") && !string.IsNullOrEmpty(dr["DownlinkStatus"].ToString()) )
                    {
                        Sec.TestResult.DownlinkStatus = bool.Parse(dr["DownlinkStatus"].ToString());
                    }

                    if (dt.Columns.Contains("UplinkStatus") && !string.IsNullOrEmpty(dr["UplinkStatus"].ToString()) )
                    {
                        Sec.TestResult.UplinkStatus = bool.Parse(dr["UplinkStatus"].ToString());
                    }
                    if (dt.Columns.Contains("MoStatus") && !string.IsNullOrEmpty(dr["MoStatus"].ToString()) )
                    {
                        Sec.TestResult.MoStatus = bool.Parse(dr["MoStatus"].ToString());
                    }
                    if (dt.Columns.Contains("MtStatus") && !string.IsNullOrEmpty(dr["MtStatus"].ToString()))
                    {
                        Sec.TestResult.MtStatus = bool.Parse(dr["MoStatus"].ToString());
                    }
                    if (ColumnExist(dt, "CwHandoverStatus") && !string.IsNullOrEmpty(dr["CwHandoverStatus"].ToString()))
                    {
                        Sec.TestResult.CwHandoverStatus = bool.Parse(dr["CwHandoverStatus"].ToString());
                    }
                    if (ColumnExist( dt, "Ccwhandoverstatus") && !string.IsNullOrEmpty(dr["Ccwhandoverstatus"].ToString()))
                    {
                        Sec.TestResult.Ccwhandoverstatus = bool.Parse(dr["Ccwhandoverstatus"].ToString());
                    }

                    if (dt.Columns.Contains("VMoStatus") && !string.IsNullOrEmpty(dr["VMoStatus"].ToString()))
                    {
                        Sec.TestResult.VMoStatus = bool.Parse(dr["VMoStatus"].ToString());
                    }
                    if (dt.Columns.Contains("VMtStatus") && !string.IsNullOrEmpty(dr["VMtStatus"].ToString()))
                    {
                        Sec.TestResult.VMtStatus = bool.Parse(dr["VMtStatus"].ToString());
                    }

                    if (dt.Columns.Contains("SMoStatus") && !string.IsNullOrEmpty(dr["SMoStatus"].ToString()))
                    {
                        Sec.TestResult.SMoStatus = bool.Parse(dr["SMoStatus"].ToString());
                    }

                    if (dt.Columns.Contains("SMtStatus") && !string.IsNullOrEmpty(dr["SMtStatus"].ToString()))
                    {
                        Sec.TestResult.SMtStatus = bool.Parse(dr["SMtStatus"].ToString());
                    }

                    if (dt.Columns.Contains("CarrierAggregationStatus") && !string.IsNullOrEmpty(dr["CarrierAggregationStatus"].ToString()))
                    {
                        Sec.TestResult.CarrierAggregationStatus = bool.Parse(dr["CarrierAggregationStatus"].ToString());
                    }

                    if (dt.Columns.Contains("E911Status") && !string.IsNullOrEmpty(dr["E911Status"].ToString()))
                    {
                        Sec.TestResult.E911Status = bool.Parse(dr["E911Status"].ToString());
                    }

                    if (dt.Columns.Contains("IRATHandover") && !string.IsNullOrEmpty(dr["IRATHandover"].ToString()))
                    {
                        Sec.TestResult.IRATHandover = bool.Parse(dr["IRATHandover"].ToString());
                    }






                    lstSectors.Add(Sec);
                }
            }


            return lstSectors;
        }


        public bool ColumnExist(DataTable dt,string Column) {
            return dt.Columns.Contains(Column);
        }

        public List<BandVM> GetSiteBands(string Filter, Int64 SiteId)
        {
            List<BandVM> lstBands = new List<BandVM>();
            SitesDL sd = new SitesDL();
            DataTable dt = sd.GetSiteBands(Filter,SiteId);

            if (dt.Rows.Count > 0)
            {
                BandVM loBand;
                foreach (DataRow dr in dt.Rows)
                {
                    loBand = new BandVM();
                    loBand.BandName = dr["BandName"].ToString();
                    loBand.BandId = cnvtr.Int64(dr["BandId"].ToString());
                    loBand.NetworkMode = dr["NetworkMode"].ToString();
                    loBand.NetworkModeId = (dt.Columns.Contains("NetworkModeId"))?DataType.ToInt32(dr["NetworkModeId"].ToString()):0;
                    loBand.Carrier = dr["Carrier"].ToString();
                    loBand.CarrierId =Convert.ToInt64( dr["CarrierId"].ToString());
                    loBand.Scope = dr["Scope"].ToString();
                    loBand.ScopeId =Convert.ToInt32( dr["ScopeId"].ToString());
                    
                    if (!string.IsNullOrEmpty(dr["ReceivedOn"].ToString()))
                    {loBand.ReceivedOn = Convert.ToDateTime(dr["ReceivedOn"].ToString());}

                    if (!string.IsNullOrEmpty(dr["SubmittedOn"].ToString()))
                    { loBand.SubmittedOn = Convert.ToDateTime(dr["SubmittedOn"].ToString()); }

                    if (!string.IsNullOrEmpty(dr["ScheduledOn"].ToString()))
                    { loBand.ScheduledOn = Convert.ToDateTime(dr["ScheduledOn"].ToString()); }

                    if (!string.IsNullOrEmpty(dr["DriveCompletedOn"].ToString()))
                    { loBand.DriveCompletedOn = Convert.ToDateTime(dr["DriveCompletedOn"].ToString()); }

                    if (!string.IsNullOrEmpty(dr["CompletedOn"].ToString()))
                    { loBand.CompletedOn = Convert.ToDateTime(dr["CompletedOn"].ToString()); }
                    loBand.Status = dr["StatusName"].ToString();
                    loBand.TesterId =Convert.ToInt32( dr["TesterId"].ToString());
                    loBand.StatusColor = dr["StatusColor"].ToString();
                    loBand.TesterName = dr["TesterName"].ToString();
                    loBand.RedriveType = dr["RedriveType"].ToString();
                    loBand.RedriveReason = dr["RedriveReason"].ToString();
                    loBand.PWoRefID = dr["PWoRefID"].ToString();
                    loBand.isReDrive =Convert.ToBoolean( dr["isReDrive"].ToString());
                    loBand.SiteCode = dr["SiteCode"].ToString();
                    loBand.City = dr["City"].ToString();
                    loBand.Region = dr["Region"].ToString();
                    loBand.IsActive =Convert.ToBoolean( dr["IsActive"].ToString());
                    loBand.LayerStatusId = Convert.ToInt64( dr["LayerStatusId"].ToString());
                    loBand.ClientPrefix = (dt.Columns.Contains("ClientPrefix")) ? dr["ClientPrefix"].ToString():null;

                    lstBands.Add(loBand);
                }
            }


            return lstBands;
        }

        public bool AssignTester(Int64 SiteId, Int64 TesterId, Int64 TesterAssignedById, DateTime SchduledDate, string Status,int NetworkMode, int Band, int Carrier, int UserDeviceId, string TestTypes,int? SequenceId,long? LayerStatusId=null)
        {
            SitesDL sd = new SitesDL();
            return sd.AssignTester(SiteId, TesterId, TesterAssignedById, SchduledDate, Status,  NetworkMode,  Band, Carrier,UserDeviceId, TestTypes,SequenceId, LayerStatusId);
        }
        /* == IF Vehcile Module is Active == */
        public bool AssignTester(int VehicleId, Int64 SiteId, Int64 TesterId, Int64 TesterAssignedById, DateTime SchduledDate, string Status, int NetworkMode, int Band, int Carrier, int UserDeviceId, string TestTypes, int? SequenceId,long? LayerStatusId=null)
        {
            SitesDL sd = new SitesDL();
            FM_VehicleDL model = new FM_VehicleDL();
            model.VehicleAssignTesterCLS("VehicleAssignTesterCLS", Convert.ToInt32(TesterId), VehicleId, Convert.ToInt32(SiteId), NetworkMode, Band, Carrier);
            return sd.AssignTester(SiteId, TesterId, TesterAssignedById, SchduledDate, Status, NetworkMode, Band, Carrier, UserDeviceId, TestTypes, SequenceId, LayerStatusId);
        }
        public bool AssignTesterCLS(Int64 SiteId, Int64 TesterId, Int64 TesterAssignedById, DateTime SchduledDate, string Status, int NetworkMode, int Band, int Carrier, int UserDeviceId, string TestTypes, int? SequenceId,int Layer,int? DeviceScheduleId=0,bool IsMaster=false)
        {
            SitesDL sd = new SitesDL();
            return sd.AssignTesterCLS(SiteId, TesterId, TesterAssignedById, SchduledDate, Status, NetworkMode, Band, Carrier, UserDeviceId, TestTypes, SequenceId,Layer, DeviceScheduleId, IsMaster);
        }
        /* == IF Vehcile Module is Active == */     
        public bool AssignTesterCLS(int? VehicleId, Int64 SiteId, Int64 TesterId, Int64 TesterAssignedById, DateTime SchduledDate, string Status, int NetworkMode, int Band, int Carrier, int UserDeviceId, string TestTypes, int? SequenceId, int Layer, int? DeviceScheduleId = 0, bool IsMaster = false)
        {
            SitesDL sd = new SitesDL();
            FM_VehicleDL model = new FM_VehicleDL();
            model.VehicleAssignTesterCLS("VehicleAssignTesterCLS", Convert.ToInt32(TesterId), VehicleId, Convert.ToInt32(SiteId), NetworkMode, Band, Carrier);
            return sd.AssignTesterCLS(SiteId, TesterId, TesterAssignedById, SchduledDate, Status, NetworkMode, Band, Carrier, UserDeviceId, TestTypes, SequenceId, Layer, DeviceScheduleId, IsMaster);
        }

        public SitesVM GetSiteDataSingle(Int64 SiteId)
        {
            SitesDL sd = new SitesDL();
            DataTable dt = sd.GetSiteData("ById", SiteId.ToString());
            if (dt.Rows.Count > 0)
            {
                SitesVM loSite = new SitesVM();
                loSite.SiteId = cnvtr.Int64(dt.Rows[0]["SiteId"].ToString());
                loSite.SiteCode = dt.Rows[0]["SiteCode"].ToString();
                loSite.Latitude = Convert.ToDouble(dt.Rows[0]["Latitude"]);
                loSite.Longitude = Convert.ToDouble(dt.Rows[0]["Longitude"]);
                loSite.SubmittedOn = Convert.ToDateTime(dt.Rows[0]["SubmittedOn"]);

                if (dt.Rows[0]["ScheduledOn"] != DBNull.Value)
                { loSite.ScheduledOn = Convert.ToDateTime(dt.Rows[0]["ScheduledOn"]); }
                if (dt.Rows[0]["AssignedOn"] != DBNull.Value)
                { loSite.AssignedOn = Convert.ToDateTime(dt.Rows[0]["AssignedOn"]); }
                if (dt.Rows[0]["CompletedOn"] != DBNull.Value)
                { loSite.CompletedOn = Convert.ToDateTime(dt.Rows[0]["CompletedOn"]); }

                

                loSite.Status = dt.Rows[0]["Status"].ToString();
                loSite.Market = dt.Rows[0]["Market"].ToString();
                loSite.Region = dt.Rows[0]["Region"].ToString();

                loSite.NetworkMode = dt.Rows[0]["NetworkMode"].ToString();
                loSite.Band = dt.Rows[0]["Band"].ToString();
                loSite.Carrier = dt.Rows[0]["Carrier"].ToString();

                if (dt.Columns.Contains("ClientId"))
                {
                    loSite.ClientId=(!string.IsNullOrEmpty(dt.Rows[0]["ClientId"].ToString()))?Convert.ToInt64( dt.Rows[0]["ClientId"].ToString()):0;
                }

                if (dt.Columns.Contains("CityId"))
                {
                    loSite.CityId = (!string.IsNullOrEmpty(dt.Rows[0]["CityId"].ToString())) ? Convert.ToInt64(dt.Rows[0]["CityId"].ToString()) : 0;
                }

                if (dt.Columns.Contains("ScopeId"))
                {
                    loSite.ScopeId = (!string.IsNullOrEmpty(dt.Rows[0]["ScopeId"].ToString())) ? Convert.ToInt64(dt.Rows[0]["ScopeId"].ToString()) : 0;
                }

                if (dt.Columns.Contains("Scope"))
                {
                    loSite.Scope = dt.Rows[0]["Scope"].ToString();
                }

                return loSite;
            }
            return null;
        }

        public List<SitesVM> GetSiteDataList(Int64 SiteId)
        {
            List<SitesVM> loSiteList = new List<SitesVM>();
            SitesDL sd = new SitesDL();
            DataTable dt = sd.GetSiteData("ById", SiteId.ToString());
            if (dt.Rows.Count > 0)
            {
                SitesVM loSite;
                foreach (DataRow dr in dt.Rows)
                {
                    loSite = new SitesVM();
                    loSite.SiteId = cnvtr.Int64(dr["SiteId"].ToString());
                    loSite.SiteCode = dr["SiteCode"].ToString();
                    loSite.Latitude = Convert.ToDouble(dr["Latitude"]);
                    loSite.Longitude = Convert.ToDouble(dr["Longitude"]);
                    loSite.SubmittedOn = Convert.ToDateTime(dr["SubmittedOn"]);

                    if (dr["ScheduledOn"] != DBNull.Value)
                    { loSite.ScheduledOn = Convert.ToDateTime(dr["ScheduledOn"]); }
                    if (dr["AssignedOn"] != DBNull.Value)
                    { loSite.AssignedOn = Convert.ToDateTime(dr["AssignedOn"]); }
                    if (dr["CompletedOn"] != DBNull.Value)
                    { loSite.CompletedOn = Convert.ToDateTime(dr["CompletedOn"]); }

                    loSite.Status = dr["Status"].ToString();
                    loSite.Market = dr["Market"].ToString();
                    loSite.Region = dr["Region"].ToString();

                    loSite.NetworkMode = dr["NetworkMode"].ToString();
                    loSite.Band = dr["Band"].ToString();
                    loSite.Carrier = dr["Carrier"].ToString();

                    if (dt.Columns.Contains("CityId"))
                    {
                        loSite.CityId =(!string.IsNullOrEmpty(dr["CityId"].ToString()))? Convert.ToInt64( dr["CityId"].ToString()):0;
                    }

                    loSiteList.Add(loSite);
                }

            }
            return loSiteList;
        }

        public SiteReportVM GetSiteReportSummary(Int64 SiteId)
        {
            SiteReportVM SiteReportvm = new SiteReportVM();
            try
            {
                DataSet ds = sd.GetSiteTestSummary(SiteId);
                DataTable dtTestSummary = ds.Tables[0];
                DataTable dtConfigurationSettigs = ds.Tables[1];
                DataTable dtPlots = ds.Tables[2];

                List<DataRow> drSite = dtTestSummary.Select("SiteId='" + SiteId + "'").Distinct().ToList();

                SitesVM loSite = new SitesVM();
                #region SitesVM

                loSite.SiteId = cnvtr.Int64(drSite[0]["SiteId"].ToString());
                loSite.SiteCode = drSite[0]["Site"] is DBNull ? "" : drSite[0]["Site"].ToString();
                loSite.Region = drSite[0]["Region"] is DBNull ? "" : drSite[0]["Region"].ToString();
                loSite.Market = drSite[0]["City"] is DBNull ? "" : drSite[0]["City"].ToString();
                loSite.Latitude = cnvtr.Double(drSite[0]["Latitude"].ToString());
                loSite.Longitude = cnvtr.Double(drSite[0]["Longitude"].ToString());


                loSite.ScheduledOn = drSite[0]["ScheduledOn"] is DBNull ? Convert.ToDateTime("11/15/2016 00:00") : Convert.ToDateTime(drSite[0]["ScheduledOn"].ToString());
                loSite.SubmittedOn = drSite[0]["SubmittedOn"] is DBNull ? Convert.ToDateTime("11/15/2016 00:00") : Convert.ToDateTime(drSite[0]["SubmittedOn"].ToString());
                loSite.CompletedOn = drSite[0]["CompletedOn"] is DBNull ? Convert.ToDateTime("11/15/2016 00:00") : Convert.ToDateTime(drSite[0]["CompletedOn"].ToString());

                loSite.ClientLogo = drSite[0]["ClientLogo"] is DBNull ? "" : drSite[0]["ClientLogo"].ToString();
                loSite.VendorLogo = drSite[0]["VendorLogo"] is DBNull ? "" : drSite[0]["VendorLogo"].ToString();
                #endregion


                DataView viewBands = new DataView(dtTestSummary);
                DataTable dtBands = viewBands.ToTable(true, "BandId", "Band", "NetworkMode", "Carrier", "Scope");


                #region lstBands

                List<BandVM> lstBands = new List<BandVM>();
                BandVM loBand;
                foreach (DataRow drBand in dtBands.Rows)
                {
                    loBand = new BandVM();
                    loBand.BandId = drBand["BandId"] is DBNull ? 0 : Convert.ToInt32(drBand["BandId"]);
                    loBand.BandName = drBand["Band"] is DBNull ? "" : Convert.ToString(drBand["Band"]);
                    loBand.NetworkMode = drBand["NetworkMode"] is DBNull ? "" : Convert.ToString(drBand["NetworkMode"]);
                    loBand.Carrier = drBand["Carrier"] is DBNull ? "" : Convert.ToString(drBand["Carrier"]);
                    loBand.Scope = drBand["Scope"] is DBNull ? "" : Convert.ToString(drBand["Scope"]);

                    DataView viewSectors = new DataView(dtTestSummary, "BandId = '" + loBand.BandId + "'", "SectorId", DataViewRowState.CurrentRows);
                    DataTable dtSectors = viewBands.ToTable(true);


                    #region TestResult And lstsectors 
                    List<SectorsVM> lstsectors = new List<SectorsVM>();
                    SectorsVM loSector;
                    TestResultVm TestResult;
                    foreach (DataRow drSector in dtSectors.Rows)
                    {
                        loSector = new SectorsVM();
                        loSector.SectorCode = drSector["Sector"] is DBNull ? "" : Convert.ToString(drSector["Sector"]);
                        loSector.PCI = drSector["PciId"] is DBNull ? 0 : Convert.ToInt32(drSector["PciId"]);

                        loSector.Azimuth = drSector["Azimuth"] is DBNull ? (float)0 : (float)Convert.ToDouble(drSector["Azimuth"]);
                        loSector.BeamWidth = drSector["BeamWidth"] is DBNull ? (float)0 : (float)Convert.ToDouble(drSector["BeamWidth"]);
                        TestResult = new TestResultVm();
                        TestResult.DownlinkAvgResult = cnvtr.Double(drSector["DownlinkAvgResult"].ToString());
                        TestResult.DownlinkMaxResult = cnvtr.Double(drSector["DownlinkMaxResult"].ToString());
                        TestResult.DownlinkMinResult = cnvtr.Double(drSector["DownlinkMinResult"].ToString());
                        TestResult.DownlinkRate = drSector["DownlinkRate"] is DBNull ? "" : Convert.ToString(drSector["DownlinkRate"]);
                        if (!string.IsNullOrEmpty(drSector["DownlinkStatus"].ToString()))
                        {
                            TestResult.DownlinkStatus = Convert.ToBoolean(drSector["DownlinkStatus"]);

                        }
                        if (!string.IsNullOrEmpty(drSector["FtpStatus"].ToString()))
                        {
                            TestResult.FtpStatus = cnvtr.Bool(drSector["FtpStatus"].ToString());

                        }
                        TestResult.GsmRssi = cnvtr.Int32(drSector["GsmRssi"].ToString());
                        TestResult.GsmRxQual = cnvtr.Int32(drSector["GsmRxQual"].ToString());
                        TestResult.LatencyRate = drSector["LatencyRate"] is DBNull ? "" : Convert.ToString(drSector["LatencyRate"]);
                        TestResult.LteCqi = cnvtr.Double(drSector["LteCqi"].ToString());
                        TestResult.LteRsnr = cnvtr.Int32(drSector["LteRsnr"].ToString());
                        TestResult.LteRsrp = cnvtr.Int32(drSector["LteRsrp"].ToString());
                        TestResult.LteRsrq = cnvtr.Int32(drSector["LteRsrq"].ToString());
                        TestResult.LteRssi = cnvtr.Int32(drSector["LteRssi"].ToString());
                        TestResult.PciId = drSector["DownlinkAvgResult"] is DBNull ? "" : Convert.ToString(drSector["PciId"]);
                        TestResult.PingAverageResult = cnvtr.Double(drSector["PingAverageResult"].ToString());
                        TestResult.PingHost = drSector["PingHost"] is DBNull ? "" : Convert.ToString(drSector["PingHost"]);
                        TestResult.PingIterations = drSector["PingIterations"] is DBNull ? "" : Convert.ToString(drSector["PingIterations"]);
                        TestResult.PingMaxResult = drSector["PingMaxResult"] is DBNull ? "" : Convert.ToString(drSector["PingMaxResult"]);
                        TestResult.PingMinResult = cnvtr.Double(drSector["PingMinResult"].ToString());
                        if (!string.IsNullOrEmpty(drSector["PingStatus"].ToString()))
                        {
                            TestResult.PingStatus = cnvtr.Bool(drSector["PingStatus"].ToString());
                        }
                        TestResult.UplinkMinResult = cnvtr.Double(drSector["UplinkMinResult"].ToString());
                        TestResult.UplinkRate = cnvtr.Double(drSector["UplinkRate"].ToString());
                        TestResult.WcdmaEcio = cnvtr.Double(drSector["WcdmaEcio"].ToString());
                        TestResult.WcdmaRscp = cnvtr.Int32(drSector["WcdmaRscp"].ToString());
                        TestResult.WcdmaRssi = cnvtr.Int32(drSector["WcdmaRssi"].ToString());
                        if (!string.IsNullOrEmpty(drSector["Ccwhandoverstatus"].ToString()))
                        {
                            TestResult.Ccwhandoverstatus = cnvtr.Bool(drSector["Ccwhandoverstatus"].ToString());
                        }
                        if (!string.IsNullOrEmpty(drSector["ConnectionSetupStatus"].ToString()))
                        {
                            TestResult.ConnectionSetupStatus = cnvtr.Bool(drSector["ConnectionSetupStatus"].ToString());
                            TestResult.MoStatus = cnvtr.Bool(drSector["MoStatus"].ToString());
                        }
                       
                        TestResult.ConnectionSetupTime = drSector["ConnectionSetupTime"] is DBNull ? "" : Convert.ToString(drSector["ConnectionSetupTime"]);
                        if (!string.IsNullOrEmpty(drSector["CwHandoverStatus"].ToString()))
                        {
                            TestResult.CwHandoverStatus = cnvtr.Bool(drSector["CwHandoverStatus"].ToString());
                        }

                        TestResult.FtpDownlinkFile = drSector["FtpDownlinkFile"] is DBNull ? "" : Convert.ToString(drSector["FtpDownlinkFile"]);
                        TestResult.FtpServerIp = drSector["FtpServerIp"] is DBNull ? "" : Convert.ToString(drSector["FtpServerIp"]);
                        TestResult.FtpServerPath = drSector["FtpServerPath"] is DBNull ? "" : Convert.ToString(drSector["FtpServerPath"]);
                        TestResult.FtpServerPort = drSector["FtpServerPort"] is DBNull ? "" : Convert.ToString(drSector["FtpServerPort"]);
                        TestResult.MoMtCallDuration = drSector["MoMtCallDuration"] is DBNull ? "" : Convert.ToString(drSector["MoMtCallDuration"]);
                        TestResult.MoMtCallNo = drSector["MoMtCallNo"] is DBNull ? "" : Convert.ToString(drSector["MoMtCallNo"]);
                        if (!string.IsNullOrEmpty(drSector["MoStatus"].ToString()))
                        {
                            TestResult.MoStatus = cnvtr.Bool(drSector["MoStatus"].ToString());
                        }
                        if (!string.IsNullOrEmpty(drSector["MtStatus"].ToString()))
                        {
                            TestResult.MtStatus = cnvtr.Bool(drSector["MtStatus"].ToString());
                        }
                        TestResult.UplinkAvgResult = cnvtr.Double(drSector["UplinkAvgResult"].ToString());
                        TestResult.UplinkMaxResult = cnvtr.Double(drSector["UplinkMaxResult"].ToString());

                        if (!string.IsNullOrEmpty(drSector["UplinkStatus"].ToString()))
                        {
                            TestResult.UplinkStatus = cnvtr.Bool(drSector["UplinkStatus"].ToString());
                        }
                        TestResult.VMoMtCallDuration = drSector["VMoMtCallDuration"] is DBNull ? "" : Convert.ToString(drSector["VMoMtCallDuration"]);
                        TestResult.VMoMtCallno = drSector["VMoMtCallno"] is DBNull ? "" : Convert.ToString(drSector["VMoMtCallno"]);
                        if (!string.IsNullOrEmpty(drSector["VMoStatus"].ToString()))
                        {
                            TestResult.VMoStatus = cnvtr.Bool(drSector["VMoStatus"].ToString());
                        }
                        if (!string.IsNullOrEmpty(drSector["VMtStatus"].ToString()))
                        {
                            TestResult.VMtStatus = cnvtr.Bool(drSector["VMtStatus"].ToString());
                        }
                        TestResult.StationaryTestFilePath = drSector["StationaryTestFilePath"] is DBNull ? "" : Convert.ToString(drSector["StationaryTestFilePath"]);
                        TestResult.CwTestFilePath = drSector["CwTestFilePath"] is DBNull ? "" : Convert.ToString(drSector["CwTestFilePath"]);
                        TestResult.CcwTestFilePath = drSector["CcwTestFilePath"] is DBNull ? "" : Convert.ToString(drSector["CcwTestFilePath"]);
                        TestResult.OoklaDownLinkResult = cnvtr.Double(drSector["OoklaDownlinkResult"].ToString());
                        TestResult.OoklaUplinkResult = cnvtr.Double(drSector["OoklaUplinkResult"].ToString());

                        TestResult.Latitude = drSector["Latitude"] is DBNull ? "" : Convert.ToString(drSector["Latitude"]);
                        TestResult.Longitude = drSector["Longitude"] is DBNull ? "" : Convert.ToString(drSector["Longitude"]);

                        TestResult.TestLatitude = cnvtr.Double(drSector["TestLatitude"].ToString());
                        TestResult.TestLongitude = cnvtr.Double(drSector["TestLongitude"].ToString());
                        TestResult.Band = drSector["Band"].ToString();
                        TestResult.IRATHandover = cnvtr.Bool(drSector["IRATHandover"].ToString());


                        loSector.TestResult = TestResult;

                        lstsectors.Add(loSector);

                    }
                    loBand.Sectors = lstsectors;
                    #endregion

                    DataView vSectors = new DataView(dtTestSummary, "SiteId = '" + loSite.SiteId + "'", "SectorId", DataViewRowState.CurrentRows);
                    DataTable tblSectors = vSectors.ToTable(true);

                    DataView viewPlots = new DataView(dtPlots);
                    DataTable dtPlot = viewPlots.ToTable(true);


                    #region SiteReportPlotVM
                    List<SiteReportPlotVM> lstPlots = new List<SiteReportPlotVM>();
                    SiteReportPlotVM plot;



                    foreach (DataRow drPlot in dtPlot.Rows)
                    {

                        plot = new SiteReportPlotVM();
                        plot.Latitude = Convert.ToDouble(drPlot["Latitude"] is DBNull ? 0 : drPlot["Latitude"]);
                        plot.Longitude = Convert.ToDouble(drPlot["Longitude"] is DBNull ? 0 : drPlot["Longitude"]);
                        plot.PCI = drPlot["PciId"] is DBNull ? "" : drPlot["PciId"].ToString();
                        plot.MarkerImagePath = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/0e/Ski_trail_rating_symbol-green_circle.svg/768px-Ski_trail_rating_symbol-green_circle.svg.png";
                        plot.siteLatitude = drSite[0]["Latitude"] is DBNull ? 0 : Convert.ToDouble(drSite[0]["Latitude"].ToString());
                        plot.siteLongitude = drSite[0]["Longitude"] is DBNull ? 0 : Convert.ToDouble(drSite[0]["Longitude"].ToString());
                        plot.IsHandover= Convert.ToBoolean(drPlot["IsHandover"].ToString());


                        ColorCollection cc = new ColorCollection();
                        #region LTE Color
                        DataRow[] drows = tblSectors.Select("PciId=" + "'" + plot.PCI + "'");
                        if (drows.Count() > 0)
                        {
                            int SelectedIndex = tblSectors.Rows.IndexOf(drows[0]);
                            plot.plotColor = cc.LTE_PciId(SelectedIndex);
                        }
                        if (loBand.NetworkMode.ToString() == "LTE")
                        {
                            int LteRsrp = drPlot["LteRsrp"] is DBNull ? 0 : Convert.ToInt32(drPlot["LteRsrp"]);
                            plot.rsrpColor = cc.LTERsrp(LteRsrp);



                            int LteRsrq = drPlot["LteRsrq"] is DBNull ? 0 : Convert.ToInt32(drPlot["LteRsrq"]);
                            plot.rsrqColor = cc.LTERsrq(LteRsrq);



                            int Ltesinr = drPlot["LteRsnr"] is DBNull ? 0 : Convert.ToInt32(drPlot["LteRsnr"]);
                            plot.rsnrColor = cc.LTEsinr(Ltesinr);



                        }


                        #endregion

                        #region   WCDMA Color
                        if (loBand.NetworkMode.ToString() == "WCDMA")
                        {
                            int WcdmaEcio = drPlot["WcdmaEcio"] is DBNull ? 0 : Convert.ToInt32(drPlot["WcdmaEcio"]);
                            plot.rsrpColor = cc.WCDMAEcio(WcdmaEcio);
                        }
                        #endregion


                        #region GSM Color
                        if (loBand.NetworkMode.ToString() == "GSM")
                        {
                            int GsmRssi = drPlot["GsmRssi"] is DBNull ? 0 : Convert.ToInt32(drPlot["GsmRssi"]);
                            plot.rsrpColor = cc.GsmRssi(GsmRssi);
                        }

                        #endregion

                        lstPlots.Add(plot);
                    }
                    #endregion
                    loBand.PciPlot = lstPlots;

                    lstBands.Add(loBand);
                }
                #endregion

                loSite.Bands = lstBands;

                DataView view = new DataView(dtConfigurationSettigs);
                DataTable distinctValues = view.ToTable(true, "TestCateoryId", "TestCategory");

                List<TestCategoryVM> lstTestCategories = new List<TestCategoryVM>();
                TestCategoryVM cat;
                foreach (DataRow dr in distinctValues.Rows)
                {
                    cat = new TestCategoryVM();
                    cat.TestCategoryId = cnvtr.Int64(dr["TestCateoryId"].ToString());
                    cat.TestCategoryName = dr["TestCategory"] is DBNull ? "" : dr["TestCategory"].ToString();

                    DataView v = new DataView(dtConfigurationSettigs, "TestCateoryId = '" + cat.TestCategoryId + "'", "TestTypeId", DataViewRowState.CurrentRows);
                    DataTable dtTypes = v.ToTable(true, "TestTypeId", "TestType");

                    List<TestTypeVM> lstTestTypes = new List<TestTypeVM>();
                    TestTypeVM type;
                    foreach (DataRow drtype in dtTypes.Rows)
                    {
                        type = new TestTypeVM();
                        type.TestTypeName = drtype["TestType"] is DBNull ? "" : drtype["TestType"].ToString();
                        type.TestTypeId = cnvtr.Int64(drtype["TestTypeId"].ToString());

                        DataView vkpis = new DataView(dtConfigurationSettigs, "TestTypeId = '" + type.TestTypeId + "'", "TestTypeId", DataViewRowState.CurrentRows);
                        DataTable dtKpi = vkpis.ToTable(true, "Kpi", "KpiValue");

                        List<TestKpiVM> lstKpi = new List<TestKpiVM>();
                        TestKpiVM kpi;
                        foreach (DataRow drkpi in dtKpi.Rows)
                        {
                            kpi = new TestKpiVM();
                            kpi.KpiName = drkpi["Kpi"] is DBNull ? "" : drkpi["Kpi"].ToString();
                            kpi.KpiValue = drkpi["KpiValue"] is DBNull ? "" : drkpi["KpiValue"].ToString();
                            lstKpi.Add(kpi);
                        }
                        type.TestKpi = lstKpi;

                        lstTestTypes.Add(type);
                    }
                    cat.TestTypes = lstTestTypes;

                    lstTestCategories.Add(cat);

                }



                SiteReportvm.Site = loSite;
                SiteReportvm.TestCategories = lstTestCategories;

            }
            catch (Exception)
            {
                // throw;

            }

            return SiteReportvm;
        }


        public string SectorColorsRGB(string Sector)
        {
                string rgb = null;

            Color c = ColorTranslator.FromHtml("#08bc01");
            if (("alpha" == Sector.ToLower()) || Sector=="1")
            {
                c = ColorTranslator.FromHtml("#f20202");

            }
            else if ("beta" == Sector.ToLower() || Sector == "2")
            {
                c = ColorTranslator.FromHtml("#021272");
            }
            else if ("gamma" == Sector.ToLower() || Sector == "3")
            {
                c = ColorTranslator.FromHtml("#008000");

            }
            else if ("delta" == Sector.ToLower() || Sector == "4")
            {
                c = ColorTranslator.FromHtml("#0AF5E9");
            }
            else if ("epsilon" == Sector.ToLower() || Sector == "5")
            {

                c = ColorTranslator.FromHtml("#EE82EE");
            }
            else if ("digamma" == Sector.ToLower() || Sector == "6")
            {
                c = ColorTranslator.FromHtml("#FFD700");
            }
            else
            {
                c = ColorTranslator.FromHtml("#08bc01");
            }

            rgb = "rgb(" + c.R + "," + c.G + ", " + c.B + ")";
            return rgb;
        }
    }
}