
using System;
namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_SiteTestLog
    {
        public decimal LogId { get; set; }
        public decimal RegionId { get; set; }
        public string Region { get; set; }
        public decimal CityId { get; set; }
        public string City { get; set; }
        public decimal TestCategoryId { get; set; }
        public string TestCategory { get; set; }
        public decimal TestTypeId { get; set; }
        public string TestType { get; set; }
        public string TimeStamp { get; set; }
        public decimal ClusterId { get; set; }
        public string Cluster { get; set; }
        public decimal SiteId { get; set; }
        public string Site { get; set; }
        public decimal SectorId { get; set; }
        public string Sector { get; set; }
        public string CellId { get; set; }
        public string LacId { get; set; }
        public string PciId { get; set; }
        public string MccId { get; set; }
        public string MncId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal ScopeId { get; set; }
        public string Scope { get; set; }
        public decimal NetworkModeId { get; set; }
        public string NetworkMode { get; set; }
        public decimal BandId { get; set; }
        public string Band { get; set; }
        public decimal CarrierId { get; set; }
        public string Carrier { get; set; }
        public float GsmRssi { get; set; }
        public float GsmRxQual { get; set; }
        public int WcdmaRssi { get; set; }
        public float WcdmaRscp { get; set; }
        public float WcdmaEcio { get; set; }
        public float LteRssi { get; set; }
        public float LteRsrp { get; set; }
        public float LteRsrq { get; set; }
        public float LteRsnr { get; set; }
        public float LteCqi { get; set; }
        public string NRBand { get; set; }
        public string NRCarrier { get; set; }
        public float NRRsrp { get; set; }
        public float NRRsrq { get; set; }
        public float NRRsnr { get; set; }
        public float NRCqi { get; set; }
        public int NRPci { get; set; }
        
        public double DistanceFromSite { get; set; }
        public double AngleToSite { get; set; }
        public string FtpStatus { get; set; }
        public string StackTrace { get; set; }
        public double TestResult { get; set; }
        public bool MoStatus { get; set; }
        public bool MtStatus { get; set; }
        public bool VolteMoStatus { get; set; }
        public bool VolteMtStatus { get; set; }
        public double ConnectionSetupTime { get; set; }
        public bool TestStatus { get; set; }
        public string SubNetworkMode { get; set; }
        public string ActualBand { get; set; }
        public string ActualCarrier { get; set; }
        public bool IsHandover { get; set; }
        public bool IsActive { get; set; }
        public int SiteRefId { get; set; }
        public int SectorRefId { get; set; }
        public string WoRefId { get; set; }
        public DateTime serverTimeStamp { get; set; }
        public string pciColor { get; set; }
        public string rsrpColor { get; set; }
        public string rsrqColor { get; set; }
        public string sinrColor { get; set; }
        public string dlColor { get; set; }
        public string ChColor { get; set; }

        //new addition of properties friday 5/4/2018

        public long FloorId { get; set; }
        public string RRCState { get; set; }
        public string NetMode1 { get; set; }
        public string Band1 { get; set; }
        public string CH1 { get; set; }

        public int PCI1 { get; set; }
        public double CI1 { get; set; }
        public double SS1 { get; set; }
        public double SP1 { get; set; }
        public double SQ1 { get; set; }
        public double SNR1 { get; set; }

        //2

        public string NetMode2 { get; set; }
        public string Band2 { get; set; }
        public string CH2 { get; set; }
        public int PCI2 { get; set; }
        public double CI2 { get; set; }
        public double SS2 { get; set; }
        public double SP2 { get; set; }
        public double SQ2 { get; set; }
        public double SNR2 { get; set; }

        //3

        public string NetMode3 { get; set; }
        public string Band3 { get; set; }
        public string CH3 { get; set; }
        public int PCI3 { get; set; }
        public double CI3 { get; set; }
        public double SS3 { get; set; }
        public double SP3 { get; set; }
        public double SQ3 { get; set; }
        public double SNR3 { get; set; }

        //4

        public string NetMode4 { get; set; }
        public string Band4 { get; set; }
        public string CH4 { get; set; }
        public int PCI4 { get; set; }
        public double CI4 { get; set; }
        public double SS4 { get; set; }
        public double SP4 { get; set; }
        public double SQ4 { get; set; }
        public double SNR4 { get; set; }

        //5

        public string NetMode5 { get; set; }
        public string Band5 { get; set; }
        public string CH5 { get; set; }
        public int PCI5 { get; set; }
        public double CI5 { get; set; }
        public double SS5 { get; set; }
        public double SP5 { get; set; }
        public double SQ5 { get; set; }
        public double SNR5 { get; set; }

        //6

        public string NetMode6 { get; set; }
        public string Band6 { get; set; }
        public string CH6 { get; set; }
        public int PCI6 { get; set; }
        public double CI6 { get; set; }
        public double SS6 { get; set; }
        public double SP6 { get; set; }
        public double SQ6 { get; set; }
        public double SNR6 { get; set; }

        //7

        public string NetMode7 { get; set; }
        public string Band7 { get; set; }
        public string CH7 { get; set; }
        public int PCI7 { get; set; }
        public double CI7 { get; set; }
        public double SS7 { get; set; }
        public double SP7 { get; set; }
        public double SQ7 { get; set; }
        public double SNR7 { get; set; }

        //8

        public string NetMode8 { get; set; }
        public string Band8 { get; set; }
        public string CH8 { get; set; }
        public int PCI8 { get; set; }
        public double CI8 { get; set; }
        public double SS8 { get; set; }
        public double SP8 { get; set; }
        public double SQ8 { get; set; }
        public double SNR8 { get; set; }

        //9
        public string TCH { get; set; }
        public string FromPCI { get; set; }
        public string ToPCI { get; set; }

        //10

        public float PRBPercent { get; set; }
        public string MCS { get; set; }
        public int RB { get; set; }
        public string Modulation { get; set; }
        public string ModPercent { get; set; }
        public string TM { get; set; }
        public int RI { get; set; }
        public float PCPDSCH { get; set; }
        public float PCPUSCH { get; set; }
        public float SCPDSCH { get; set; }
        public float SCPUSCH { get; set; }

    }

}
