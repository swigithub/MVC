
namespace SWI.Libraries.AirView.Entities
{
    public class TestResultVm
    {
        public string PciId { get; set; }
        public int GsmRssi { get; set; }
        public int GsmRxQual { get; set; }
        public int WcdmaRssi { get; set; }
        public int WcdmaRscp { get; set; }
        public double WcdmaEcio { get; set; }
        public int LteRssi { get; set; }
        public int LteRsrp { get; set; }
        public int LteRsrq { get; set; }
        public int LteRsnr { get; set; }
        public double LteCqi { get; set; }
        public bool? FtpStatus { get; set; }
        public string PingHost { get; set; }
        public string LatencyRate { get; set; }
        public string PingIterations { get; set; }
        public double PingMinResult { get; set; }
        public string PingMaxResult { get; set; }
        public double PingAverageResult { get; set; }
        public bool? PingStatus { get; set; }
        public string DownlinkRate { get; set; }
        public double DownlinkMinResult { get; set; }
        public double DownlinkMaxResult { get; set; }
        public double DownlinkAvgResult { get; set; }
        public bool? DownlinkStatus { get; set; }
        public double UplinkRate { get; set; }
        public double UplinkMinResult { get; set; }
        public double UplinkMaxResult { get; set; }
        public double UplinkAvgResult { get; set; }
        public bool? UplinkStatus { get; set; }
        public string ConnectionSetupTime { get; set; }
        public bool? ConnectionSetupStatus { get; set; }
        public string MoMtCallNo { get; set; }
        public string MoMtCallDuration { get; set; }
        public bool? MoStatus { get; set; }
        public bool? MtStatus { get; set; }
        public string VMoMtCallno { get; set; }
        public string VMoMtCallDuration { get; set; }
        public bool? VMoStatus { get; set; }
        public bool? VMtStatus { get; set; }
        public bool? CwHandoverStatus { get; set; }
        public bool? Ccwhandoverstatus { get; set; }
        public string FtpServerIp { get; set; }
        public string FtpServerPort { get; set; }
        public string FtpServerPath { get; set; }
        public string FtpDownlinkFile { get; set; }

        public string StationaryTestFilePath { get; set; }
        public string CwTestFilePath { get; set; }
        public string CcwTestFilePath { get; set; }
        public double OoklaDownLinkResult { get; set; }
        public double OoklaUplinkResult { get; set; }

        public string OoklaTestFilePath { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public double TestLatitude { get; set; }
        public double TestLongitude { get; set; }
        public string Band { get; set; }

        public bool? SMoStatus { get; set; }
        public bool? SMtStatus { get; set; }

        public bool? CarrierAggregationStatus { get; set; }
        public bool? E911Status { get; set; }
        public bool? IRATHandover { get; set; }

    }
}
