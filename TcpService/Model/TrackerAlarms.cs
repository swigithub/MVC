using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpService
{
    class TrackerAlarms
    {
        public AlarmCodes AlrmCodes { get; set; }
        public double ThresholdVal {get; set;}
        public double CurrentVal { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }    

    enum AlarmCodes
    {
        ExhstEmson=0,
        IdlEngn = 1,
        HrdAcc =2,
        HrdDecel = 3,
        EngnTmp =4,
        Spd=5,
        Twng =6,
        LwVltg = 7,
        Tmpr = 8,
        Crsh = 9,
        Emrgncy =10,
        FatgDrv = 11,
        ShrpTrn = 12,
        QkLnChg = 13,
        PwrOn = 14,
        HghRPM  = 15,
        MIL = 16,
        OBDErr = 17,
        PwrOf = 18,
        NoGPS = 19,
        PrvcyStatus =20,
        IgntOn = 21,
        IllglIgnton = 22,
        IllglEntr = 23,
        Vib = 24,
        DngrousDrvng = 25,
        NoCrd = 26,
        UnLk = 27,
        GeoFnceAlrm = 28,
        IgntOff =29,
        SOSAlrm =30,
        DoorOpnAlrm=31,
        FuelLosAlarm =32,
        WifiEnable=33,
        WifiSSID=34,
        WifiPwd = 35

    }
}
