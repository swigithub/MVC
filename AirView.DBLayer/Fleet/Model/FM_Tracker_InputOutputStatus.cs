using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.Model
{
    public class FM_Tracker_InputOutputStatus
    {
        public bool OutLockthedoor { get; set; }
        public bool OutSirenSound { get; set; }
        public bool OutUnlockthedoor { get; set; }
        public bool OutRelyToStopCar { get; set; }

        public bool InSOS { get; set; }
        public bool InAntiTemper { get; set; }
        public bool InDoorOpenClose { get; set; }
        public bool InUnlockDoor { get; set; }
        public bool InEngineOnOff { get; set; }
    }

    public class FM_Tracker_Alarms
    {
        public AlarmCodes AlrmCodes { get; set; }
        public string Description { get; set; }
        public float ThresholdVal { get; set; }
        public float CurrentVal { get; set; }
        public bool Status { get; set; }
    }
    public enum AlarmCodes
    {
        [Description("Exhaust Emission")]
        ExhstEmson = 0,
        [Description("Idle Engine")]
        IdlEngn = 1,
        [Description("Hard Acceleration")]
        HrdAcc = 2,
        [Description( "Hard Deceleration")]
        HrdDecel = 3,
        [Description("Engine Temperature")]
        EngnTmp = 4,
        [Description( "Speeding")]
        Spd = 5,
        [Description("Towing")]
        Twng = 6,
        [Description("Low Voltage")]
        LwVltg = 7,
        [Description("Temperature")]
        Tmpr = 8,
        [Description("Crash")]
        Crsh = 9,
        [Description("Emergency")]
        Emrgncy = 10,
        [Description("Fatigue Driving")]
        FatgDrv = 11,
        [Description("Sharp Turn")]
        ShrpTrn = 12,
        [Description("Quick Lane Change")]
        QkLnChg = 13,
        [Description("Power On")]
        PwrOn = 14,
        [Description("High RPM")]
        HghRPM = 15,
        [Description("MIL")]
        MIL = 16,
        [Description("OBD Communication Error")]
        OBDErr = 17,
        [Description( "Power Off")]
        PwrOf = 18,
        [Description("No GPS")]
        NoGPS = 19,
        [Description("Privacy Status")]
        PrvcyStatus = 20,
        [Description("Ignition On")]
        IgntOn = 21,
        [Description("Illegal lIgniton")]
        IllglIgnton = 22,
        [Description("Illegal Enter")]
        IllglEntr = 23,
        [Description("Vibration")]
        Vib = 24,
        [Description("Dangerous Driving")]
        DngrousDrvng = 25,
        [Description("No card")]
        NoCrd = 26,
        [Description("Un lock")]
        UnLk = 27,
        [Description("Geo-Fence Alarm")]
        GeoFnceAlrm = 28,
        [Description("Ignition Off")]
        IgntOff = 29,
        [Description("SOS Alarm")]
        SOSAlrm = 30,
        [Description("Door Open Alarm")]
        DoorOpnAlrm = 31,
        [Description("Fuel Loss Alarm")]
        FuelLosAlarm = 32
    }
}
