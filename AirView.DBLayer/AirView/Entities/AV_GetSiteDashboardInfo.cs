using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.Entities
{
    public class AV_GetSiteDashboardInfo
    {
        public AV_GetSiteDashboardInfo()
        {
            TeamMember = new List<SiteDashboardTeamMember>();
            ClientOrVendor = new List<SiteDashboardClientOrVendor>();
            PingThroughtput = new List<SiteDashboardThroughtputChart>();
            DLThroughtput = new List<SiteDashboardThroughtputChart>();
            ULThroughtput = new List<SiteDashboardThroughtputChart>();

            SiteSectorInfo = new List<SiteSectorInfo>();
            PciSignalStrength = new List<PciSignalStrength>();

            MOMTStatus = new List<MOMTStatus>();
            HandoverStatus = new List<HandoverStatus>();
            OoklaTestResult = new List<OoklaTestResult>();
            GraphDataMTMOSMOSMT = new List<groupByTestType>();


        }
        public List<SiteDashboardTeamMember> TeamMember { get; set; }
        public List<SiteDashboardClientOrVendor> ClientOrVendor { get; set; }
        public List<SiteDashboardThroughtputChart> PingThroughtput { get; set; }
        public List<SiteDashboardThroughtputChart> DLThroughtput { get; set; }
        public List<SiteDashboardThroughtputChart> ULThroughtput { get; set; }

        public List<SiteSectorInfo> SiteSectorInfo { get; set; }
        public List<PciSignalStrength> PciSignalStrength { get; set; }

        public List<MOMTStatus> MOMTStatus { get; set; }
        public List<HandoverStatus> HandoverStatus { get; set; }
        public List<OoklaTestResult> OoklaTestResult { get; set; }

        public List<groupByTestType> GraphDataMTMOSMOSMT { get; set; }

        //

    }

    public class SiteDashboardTeamMember
    {
        public decimal UserId { get; set; }
        public string Username { get; set; }
        public string UserType { get; set; }
        public string Picture { get; set; }
    }

    public class SiteDashboardClientOrVendor
    {
        public string ClientName { get; set; }
        public string Logo { get; set; }
        public string ClientType { get; set; }
    }

    public class SiteDashboardThroughtputChart
    {
        public string SiteCode { get; set; }
        public string TestType { get; set; }
        public DateTime? TimeStamp { get; set; }
        public double MinResult { get; set; }
        public double AvgResult { get; set; }
        public double MaxResult { get; set; }
        public string Sector { get; set; }
    }
    public class SiteDashboardThroughtputChartmomt
    {
        public string SiteCode { get; set; }
        public string TestType { get; set; }
        public DateTime? TimeStamp { get; set; }
        public int NetworkModeId { get; set; }
        public string NetworkMode { get; set; }
        public string Sector { get; set; }
        public int BandId { get; set; }

        public string Band { get; set; }
    }

    public class groupByTestType

    {
        public groupByTestType()
        {
            SiteDashboardThroughtputChartmomtList = new List<SiteDashboardThroughtputChartmomt>();
        }

        public string TestTypeGroup { get; set; }

        public List<SiteDashboardThroughtputChartmomt> SiteDashboardThroughtputChartmomtList { get; set; }
    }

    public class SiteSectorInfo // added by siddique 2018-05-04
    {
        public double TestLatitude { get; set; }
        public double TestLongitude { get; set; }
        public string NetworkMode { get; set; }
        public string Band { get; set; }
        public double Carrier { get; set; }
        public string SignalStrength { get; set; }
        public string SignalQuality { get; set; }
        public string SignalNoise { get; set; }

    }

    public class PciSignalStrength // added by siddique 2018-05-04
    {
        public DateTime TimeStamp { get; set; }
        public string PCI { get; set; }
        public string SignalStrength { get; set; }
    }

    public class MOMTStatus
    {
        public string SiteCode { get; set; }
        public string Sector { get; set; }
        public bool MoStatus { get; set; }
        public bool MtStatus { get; set; }
        public bool VMoStatus { get; set; }
        public bool VMtStatus { get; set; }
    }

    public class HandoverStatus
    {
        public string SiteCode { get; set; }
        public string Sector { get; set; }
        public decimal PciId { get; set; }
        public bool CwHandoverStatus { get; set; }
        public bool Ccwhandoverstatus { get; set; }
        public bool ICwHandoverStatus { get; set; }
        public bool ICcwhandoverstatus { get; set; }
    }

    public class OoklaTestResult
    {
        public string SectorCode { get; set; }
        public double OoklaPingResult { get; set; }
        public double OoklaDownlinkResult { get; set; }
        public double OoklaUplinkResult { get; set; }
        public string OoklaTestFilePath { get; set; }
    }

}
