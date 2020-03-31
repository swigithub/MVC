using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_SiteScript
    {
        public Int64 SrId { get; set; }

        public Int64 ScriptId { get; set; }
        public Int64 EventValue1 { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 NetLayerId { get; set; }
        public Int64 RevisionId { get; set; }
        public Int64 EventTypeId { get; set; }
        public string Event { get; set; }
        public string EventValue { get; set; }
        public bool IsValue { get; set; }
        public bool IsL3Enabled { get; set; }
        public string Color { get; set; }
        public string MapColumn { get; set; }
        public string DisplayType { get; set; }
        public string DefinationName { get; set; }
        public string pDefinationName { get; set; }
        public string ScannerManufecturerId { get; set; }
        public string ScannerModelId { get; set; }
        public string Options { get; set; }
        public string OptionId { get; set; }
        public string EventCommand { get; set; }
        public string pDefinationId { get; set; }
        public Int64 PEventId { get; set; }

        public Int64 CEventId { get; set; }

        public string CEvent { get; set; }

        public string PEvent { get; set; }

        public int SequenceId { get; set; }
        public DateTime ScheduledOn { get; set; }
        public Int64 DeviceScheduleId { get; set; }
        
        public Int64 BandId { get; set; }
        public Int64 CarrierId { get; set; }
        public Int64 NetworkModeId { get; set; }
        public Int64 ScopeId { get; set; }

        /////Parent device

        public Int64 TesterId { get; set; }
        public Int64 UserDeviceId { get; set; }
        public string TestKpi { get; set; }
        public int SortOrder { get; set; }
        public List<AV_NodesProperties> ActionDialogue { get; set; }
        public string ScannerConfig { get; set; }
        //public MeasurementRSSI BlindScanConfig { get; set; }
        //public MeasurementRSSI NRTopNSignalConfig { get; set; }
    }

    public class AV_NodesProperties
    {
        public decimal FormId { get; set; }

        public decimal EntryId { get; set; }
        public int IsDeleted { get; set; }
        public decimal NodeTypeId { get; set; }
        public string Title { get; set; }
        public string DataType { get; set; }
        public string DefaultValue { get; set; }

        public string ActualValue { get; set; }
        public string MaxLength { get; set; }
        public string Required { get; set; }

        public int SortOrder { get; set; }
     
      


    }
    public class AV_ScriptScannerConfigurations
    {
        public int KpiId { get; set; }
        public int pKpiId { get; set; }
        public string KpiName { get; set; }
        public string KpiCode { get; set; }
        public string KpiValue { get; set; }
    }

    public class ScannerConfig
    {
        public string SCAN_TYPE { get; set; }
        public string SCAN_REQUEST_BODY { get; set; }
        //public MeasurementRSSI SCAN_REQUEST_BODY { get; set; } = new MeasurementRSSI();
    }
    //RSSI
    public class MeasurementRSSI
    {
        public int SCAN_MODE { get; set; }
        public CHANNEL_LIST CHANNEL_LIST { get; set; } = new CHANNEL_LIST();
        public SCAN_SAMPLING SCAN_SAMPLING { get; set; } = new SCAN_SAMPLING();
    }
    public class CHANNEL_LIST
    {
        public long PROTOCOL_CODE { get; set; }
        public long BAND_CODE { get; set; }
        public List<CHANNEL_ARRAY> CHANNEL_ARRAY { get; set; } = new List<Entities.CHANNEL_ARRAY>();
    }
    public class CHANNEL_ARRAY
    {
        public long CHANNEL_NUMBER { get; set; }
        public long CHANNEL_STYLE { get; set; }
    }
    public class SCAN_SAMPLING
    {
        public long SCAN_SAMPLING_TRIGGER_TYPE { get; set; }
        public long SCAN_SAMPLING_TRIGGER_VALUE { get; set; }
        public long[] SCAN_SAMPLING_MODE_ARRAY { get; set; }
    }
    //public class SCAN_SAMPLING_MODE_ARRAY
    //{
    //    public long value { get; set; }
    //}

    //Blind Scan
    public class MeasurementBlindScan
    {
        public string SCAN_MODE { get; set; }
        public List<BLIND_SCAN_REQUEST_ELEMENT_LIST> BLIND_SCAN_REQUEST_ELEMENT_LIST { get; set; } = new List<Entities.BLIND_SCAN_REQUEST_ELEMENT_LIST>();
        public List<SCAN_SAMPLING> SCAN_SAMPLING { get; set; } = new List<SCAN_SAMPLING>();
    }
    public class BLIND_SCAN_REQUEST_ELEMENT_LIST
    {
        public string PROTOCOL_CODE { get; set; }
        public string NUMBER_OF_IDS { get; set; }
        public List<BLIND_SCAN_REQUEST_BAND_ELEMENT_LIST> BLIND_SCAN_REQUEST_BAND_ELEMENT_LIST { get; set; } = new List<Entities.BLIND_SCAN_REQUEST_BAND_ELEMENT_LIST>();
        public List<DATA_MODE_LIST> DATA_MODE_LIST { get; set; } = new List<Entities.DATA_MODE_LIST>();
    }
    public class BLIND_SCAN_REQUEST_BAND_ELEMENT_LIST
    {
        public string BAND_CODE { get; set; }
    }
    public class DATA_MODE_LIST
    {
        public string value { get; set; }
    }

    // NR Top N Signal
    public class MeasurementNRTopNSignal
    {
        public string SCAN_MODE { get; set; }
        public List<CHANNEL_OR_FREQUENCY> CHANNEL_OR_FREQUENCY { get; set; } = new List<Entities.CHANNEL_OR_FREQUENCY>();
        public List<SCAN_SAMPLING> SCAN_SAMPLING { get; set; } = new List<SCAN_SAMPLING>();
    }
    public class CHANNEL_OR_FREQUENCY
    {
        public string PROTOCOL_CODE { get; set; }
        public string BAND_CODE { get; set; }
        public List<CHANNEL_OR_FREQUENCYSPECIED> CHANNEL_OR_FREQUENCYs { get; set; }
    }
    public class CHANNEL_OR_FREQUENCYSPECIED
    {
        public string CHANNEL_SPECIFIED { get; set; }
        public List<CHANNEL_ARRAY> CHANNEL { get; set; } = new List<Entities.CHANNEL_ARRAY>();
    }
    public class FREQUENCYs
    {
        public string FREQUENCY { get; set; }
        public string BAND_WIDTH { get; set; }
    }

}