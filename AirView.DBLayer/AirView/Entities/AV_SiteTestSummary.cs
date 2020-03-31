using AirView.DBLayer.AirView.Entities;
using System;
using System.Collections.Generic;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_SiteTestSummary
    {
        public AV_SiteTestSummary()
        {
            CW_CCWHOPlot = new List<CW_CCWHOPlot>();
            MOMTCallPlot = new List<MOMTCallPlot>();
            TestWiseSiteData = new List<TestWiseSiteData>();
            Locations = new List<TestLocations>();
            Observations = new List<AV_NetLayerObservation>();
            BeamTestLog = new List<AV_BeamTestLog>();
            BTLegend = new List<BeamTestLegend>();
            RDACounts = new List<RDACounts>();
            OoklaDLSiteLevels = new List<OoklaDLSiteLevels>();
            SiteTestLog = new List<AV_SiteTestLog>();
            SectorsOoklaTest = new List<AV_SectorsOoklaTest>();
        }
        public long Testcount { get; set; }
        public decimal TestDuration { get; set; }

        public decimal NLatitude { get; set; }
        public decimal NLongitude { get; set; }
        public decimal SummaryId { get; set; }

        public decimal RegionId { get; set; }
        public string Region { get; set; }
        public long SampleCount { get; set; }
        public bool FastReturnStatus { get; set; }
        public double CoverageDistance { get; set; }
        public string TCH { get; set; }
        public decimal CityId { get; set; }
        public decimal ClientId { get; set; }
        public string City { get; set; }
        public decimal ClusterId { get; set; }
        public string Cluster { get; set; }
        public decimal SiteId { get; set; }
        public string Site { get; set; }
        public string ActualSiteCode { get; set; }
        public DateTime? SiteScheduleDate { get; set; }
        public DateTime SiteUploadDate { get; set; }
        public decimal SectorId { get; set; }
        public string Sector { get; set; }
        public decimal ScopeId { get; set; }
        public string Scope { get; set; }
        public decimal NetworkModeId { get; set; }
        public string NetworkMode { get; set; }
        public decimal BandId { get; set; }
        public string Band { get; set; }
        public string BandNo { get; set; }
        public decimal CarrierId { get; set; }
        public string Carrier { get; set; }
        public string Antenna { get; set; }
        public double Azimuth { get; set; }
        public decimal PciId { get; set; }
        public double BeamWidth { get; set; }
        public int BandWidth { get; set; }

        public int GsmRssi { get; set; }
        public int GsmRxQual { get; set; }
        public int WcdmaRssi { get; set; }
        public int WcdmaRscp { get; set; }
        public double WcdmaEcio { get; set; }

        public double DistanceFromSite { get; set; }
        public double AngleToSite { get; set; }
        public string FtpStatus { get; set; }
        public string PingHost { get; set; }
        public double LatencyRate { get; set; }
        public double PingIterations { get; set; }
        public double PingMinResult { get; set; }
        public double PingMaxResult { get; set; }
        public double PingAverageResult { get; set; }
        public double CCWHoInterruptionTime { get; set; }

        public bool? PingStatus { get; set; }
        public bool? PhyDLStatus { get; set; }
        public bool? PhyULStatus { get; set; }

        public double DownlinkRate { get; set; }
        public double DownlinkMinResult { get; set; }
        public double DownlinkMaxResult { get; set; }
        public double DownlinkAvgResult { get; set; }
        public bool? DownlinkStatus { get; set; }
        public double UplinkRate { get; set; }
        public double UplinkMinResult { get; set; }
        public double UplinkMaxResult { get; set; }
        public double UplinkAvgResult { get; set; }
        public bool? UplinkStatus { get; set; }
        public double ConnectionSetupTime { get; set; }
        public bool? ConnectionSetupStatus { get; set; }
        public double CADLSpeed { get; set; }
        public double CAULSpeed { get; set; }

        public int LteRssi { get; set; }
        public string FloorPath { get; set; }
        public int LteRsrp { get; set; }
        public int LteRsrq { get; set; }
        public double LteRsnr { get; set; }
        public double LteCqi { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }


        public string MoMtCallNo { get; set; }
        public int MoMtCallDuration { get; set; }
        public bool? MoStatus { get; set; }
        public bool? MobHandoverStatus { get; set; }

        public bool? MtStatus { get; set; }
        public string VMoMtCallno { get; set; }
        public int VMoMtCallDuration { get; set; }
        public bool? VMoStatus { get; set; }
        public bool? VMtStatus { get; set; }
        public bool? MimoStatus { get; set; }
        public bool? CwHandoverStatus { get; set; }
        public bool? Ccwhandoverstatus { get; set; }
        public string FtpServerIp { get; set; }
        public string FtpServerPort { get; set; }
        public string FtpServerPath { get; set; }
        public string FtpDownlinkFile { get; set; }
        public string OoklaTestFilePath { get; set; }
        public double OoklaUplinkResult { get; set; }
        public double OoklaDownlinkResult { get; set; }
        public double OoklaPingResult { get; set; }
        public string ClientLogo { get; set; }
        public string VendorLogo { get; set; }

        public double TestLatitude { get; set; }
        public double TestLongitude { get; set; }
        public bool? ICwHandoverStatus { get; set; }
        public bool? ICcwhandoverstatus { get; set; }
        public double OoklaRssi { get; set; }
        public double OoklaSinr { get; set; }
        public double TestRssi { get; set; }
        public double TestSinr { get; set; }

        public string SiteName { get; set; }
        public string SiteType { get; set; }
        public string SiteClass { get; set; }
        public string SiteAddress { get; set; }
        public string LayerStatus { get; set; }
        public double RFHeight { get; set; }
        public int ETilt { get; set; }
        public int MTilt { get; set; }
        public string CellId { get; set; }
        public string Project { get; set; }
        public string ClientPrefix { get; set; }
        public string CwTestFilePath { get; set; }
        public string StationaryTestFilePath { get; set; }
        public string SpeedTestFilePath { get; set; }
        public string CaActiveTestFilePath { get; set; }
        public string CaDeavticeTestFilePath { get; set; }
        public string LaaSpeedTestFilePath { get; set; }
        public string CaSpeedTestFilePath { get; set; }
        public string LaaSmTestFilePath{ get; set; }
        public string CcwTestFilePath { get; set; }
        public string MimoTestFilePath { get; set; }

        public bool? SMoStatus { get; set; }
        public bool? SMtStatus { get; set; }

        public bool? CarrierAggregationStatus { get; set; }
        public bool? E911Status { get; set; }
        public bool? IsE911Performed { get; set; }
        public string SectorColor { get; set; }
        public string MRBTS { get; set; }

        public double SectorLatitude { get; set; }
        public double SectorLongitude { get; set; }

        public double MinConSetupTime { get; set; }

        public double MaxConSetupTime { get; set; }

        public double AvgConSetupTime { get; set; }

        public double HoInterruptionTime { get; set; }
        public double InterHOInteruptTime { get; set; }
        public double IntraHOInteruptTime { get; set; }
        public double IntreHOInteruptTime { get; set; }
        public double callSetupTime { get; set; }
        public double PhyULSpeedAvg { get; set; }
        public double PhyULSpeedMax { get; set; }
        public double PhyULSpeedMin { get; set; }
        public double PhyDLSpeedAvg { get; set; }
        public double PhyDLSpeedMax { get; set; }
        public double PhyDLSpeedMin { get; set; }


        public bool? IRATHandover { get; set; }



        public List<ReportTimeStamp> reportTimeStamps { get; set; }

        //------Siddique------------------
        public double DLLat { get; set; }
        public double DLLng { get; set; }
        public double DLSignalStrength { get; set; }
        public double DLSignalPower { get; set; }
        public double DLSignalQuality { get; set; }
        public double DLSignalNoise { get; set; }

        public double CSLat { get; set; }
        public double CSLng { get; set; }
        public double CSSignalStrength { get; set; }
        public double CSSignalPower { get; set; }
        public double CSSignalQuality { get; set; }
        public double CSSignalNoise { get; set; }

        public double RttLat { get; set; }
        public double RttLng { get; set; }
        public double RttSignalStrength { get; set; }
        public double RttSignalPower { get; set; }
        public double RttSignalQuality { get; set; }
        public double RttSignalNoise { get; set; }


        public List<CW_CCWHOPlot> CW_CCWHOPlot { get; set; }
        public List<MOMTCallPlot> MOMTCallPlot { get; set; }
        public List<TestWiseSiteData> TestWiseSiteData { get; set; }
        public List<AV_FloorPlan> FloorPlans { get; set; }
        public List<TestLocations> Locations { get; set; }
        public List<AV_NetLayerObservation> Observations { get; set; }

        public List<AV_BeamTestLog> BeamTestLog { get; set; }
        public List<BeamTestLegend> BTLegend { get; set; }
        public List<RDACounts> RDACounts { get; set; }
        public List<OoklaDLSiteLevels> OoklaDLSiteLevels { get; set; }
        public List<AV_SiteTestLog> SiteTestLog { get; set; }
        public List<AV_SectorsOoklaTest> SectorsOoklaTest { get; set; }


    }

    public class ReportTimeStamp
    {
        public ReportTimeStamp()
        {

        }
        public Int64 SiteId { get; set; }
        public DateTime serverTimeStamp { get; set; }
        public string NetworkModeId { get; set; }
        public string NetworkMode { get; set; }
        public string BandId { get; set; }
        public string Band { get; set; }
        public string NetLayerTitle { get; set; }
        public string CarrierId { get; set; }
        public string Carrier { get; set; }

        public string SectorId { get; set; }
        public string NetLayerId { get; set; }
        public string NetLayer { get; set; }
        public string PCI { get; set; }
        public Int64 PciId { get; set; }

        public string SectorCode { get; set; }
        public string SignalStrength { get; set; }
        public string ColorCode { get; set; }

        public List<SectorTables> Tables { get; set; }
    }

    public class SectorTables
    {
        public string NetLayer { get; set; }

        public Int64 SectorId { get; set; }

        public Int64 SiteId { get; set; }

        public string Sector { get; set; }

        public string PCI { get; set; }

        public Int64 SampleCount { get; set; }
        public double SamplePercentage { get; set; }

        public Int64 ReselectionCount { get; set; }
    }
    public class TestLocations
    {
        public int SiteId { get; set; }
        public int NetworkModeId { get; set; }
        public int BandId { get; set; }
        public int CarrierId { get; set; }
        public int ScopeId { get; set; }

        public int SectorId { get; set; }

        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string TestType { get; set; }
        public string TestResult { get; set; }
        
        public string TestCategory { get; set; }
        public float SignalStrength { get; set; }
        public float SignalQuality { get; set; }
        public float SignalNoiseRatio { get; set; }
        public string TestComments { get; set; }
        public DateTime TestDate { get; set; }
        public bool TestStatus { get; set; }
        public float TestKPI { get; set; }
        public string SourceCell { get; set; }
        public string TargetCell { get; set; }

        public long TestLocationId { get; set; }
        public float PRBPercent { get; set; }
        public string MCS { get; set; }
        public int RB { get; set; }
        public string Modulation { get; set; }
        public string ModPercent { get; set; }
        public string TM { get; set; }
        public int RI { get; set; }
        public float PDSCH { get; set; }
        public float CAPDSCH { get; set; }
        public float PUSCH { get; set; }
        public float CAPU { get; set; }

    }
    public class CW_CCWHOPlot
    {
        public int siteID { get; set; }
        public int networkModeID { get; set; }
        public int bandID { get; set; }
        public int carrierID { get; set; }
        public int scopeID { get; set; }
        public int sectorID { get; set; }
        public string TestType { get; set; }
        public string Timestamp { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string HandoverType { get; set; }

        public string FromCarrier { get; set; }
        public string FromBand { get; set; }
        public string ToCarrier { get; set; }
        public string ToBand { get; set; }
        public string ToPCI { get; set; }
        public string HoDuration { get; set; }
        public string HoInterruptionTime { get; set; }
        public string FromPCI { get; set; }
        public string ToChannel { get; set; }
        public string FromRAT { get; set; }
        public string ToRAT { get; set; }

    }
    public class MOMTCallPlot
    {
        public int siteID { get; set; }
        public int networkModeID { get; set; }
        public int bandID { get; set; }
        public int carrierID { get; set; }
        public int scopeID { get; set; }
        public int sectorID { get; set; }
        public string TestType { get; set; }
        public string Timestamp { get; set; }
        public string Event { get; set; }
        public string DirectionIN { get; set; }
        public string DirectionOUT { get; set; }
        public string Description { get; set; }
    }

    public class TestWiseSiteData { 
    
        public int siteID { get; set; }
        public int NetworkModeId { get; set; }
        public int BandId { get; set; }
        public int CarrierId { get; set; }

        public int ScopeId { get; set; }
        public int SectorId { get; set; }
        public string TestType { get; set; }
        public string TestLatitude { get; set; }

        public string TestLongitude { get; set; }
        public string SignalStrength { get; set; }
        public string SignalPower { get; set; }
        public string SignalQuality { get; set; }

        public string SignalNoise { get; set; }
    }

    public class AV_NetLayerObservation
    {
        public int Id { get; set; }
        public long LayerStatusId { get; set; }
        public string CWComments { get; set; }
        public string CCWComments { get; set; }
        public string PDSCHComments { get; set; }
        public string PUSCHComments { get; set; }
    }
    public class AV_SectorsOoklaTest
    {
        public int Id { get; set; }
        public long SiteId { get; set; }
        public long NetworkModeId { get; set; }
        public long BandId { get; set; }
        public long ScopeId { get; set; }
        public long SectorId { get; set; }
        public long CarrierId { get; set; }

        public string OoklaFilePath { get; set; }
        public string NetWorkMode { get; set; }
        public double DownLinkSpeed { get; set; }
        public double UpLinkSpeed { get; set; }
        public bool isManual { get; set; }
    }
}
