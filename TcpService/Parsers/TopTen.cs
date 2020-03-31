using AirViewTracker.DBLayer.Fleet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpService.Parsers
{
    internal class TopTen
    {
        CommonOperation ObjComnOp;
        string ExceptionLogPath = "";
        string LogFilePath = "";
        string SgnalRApi = "";
        List<ClientInfo> ClientLst;
        bool WriteLogFile;
        bool WriteExceptionLog;
        List<String> DeviceIDCollection;
        FM_VehicleBL ObjVehiclesBL;
        bool ChkIndividualTrackerFromDb;
        List<string> AlarmsWthFloatVal;
        private static readonly Object LockThread = new Object();
       
        public TopTen(string ExceptionLogPth, string FilePth, string SignalRAPi, ref List<ClientInfo> LstClntInfo, bool WriteLgFile, bool WriteExcetionLg, List<string> IMEICollect, string ConnectionStr, bool CheckIndividualTrackerFromDb)
        {
            ExceptionLogPath = ExceptionLogPth;
            LogFilePath = FilePth;
            SgnalRApi = SignalRAPi;
            ObjComnOp = new CommonOperation(LogFilePath, ExceptionLogPath, SignalRAPi);
            ClientLst = LstClntInfo;
            WriteLogFile = WriteLgFile;
            WriteExceptionLog = WriteExcetionLg;
            DeviceIDCollection = IMEICollect;
            ObjVehiclesBL = new FM_VehicleBL(ConnectionStr);
            ChkIndividualTrackerFromDb = CheckIndividualTrackerFromDb;           

        }


        internal void ParseAlarmPcket(string[] HexCollection, string TrackerHexaResponse, string TrackerId,DataPacket DtaObj)
        {
            int AlarmByte = 13;
            string AlarmCode = HexCollection[AlarmByte];
            TrackerAlarms Alrms = new TrackerAlarms();
            Alrms.Status = true;
            DtaObj.LstAlarms = new List<TrackerAlarms>();

            switch (AlarmCode)
            {
                case "01":
                    Alrms.AlrmCodes = AlarmCodes.SOSAlrm;
                    Alrms.Description = "SOS Alarm";
                    DtaObj.LstAlarms.Add(Alrms);
                    break;
                case "02":
                    //Alrms.AlrmCodes = AlarmCodes;
                    //Alrms.Description = "Speed (KM/H)";
                    break;
                case "03":
                    Alrms.AlrmCodes = AlarmCodes.DoorOpnAlrm;
                    Alrms.Description = "Door Open Alarm";
                    DtaObj.LstAlarms.Add(Alrms);
                    break;
                case "04":
                    Alrms.AlrmCodes = AlarmCodes.IgntOn;
                    Alrms.Description = "Ignition ON";
                    DtaObj.LstAlarms.Add(Alrms);
                    break;
                case "05":
                    //Alrms.AlrmCodes = AlarmCodes.Spd;
                    //Alrms.Description = "Speed (KM/H)";
                    break;
                case "10":
                    Alrms.AlrmCodes = AlarmCodes.LwVltg;
                    Alrms.Description = "Low Voltage (V)";
                    DtaObj.LstAlarms.Add(Alrms);
                    break;
                case "11":
                    Alrms.AlrmCodes = AlarmCodes.Spd;
                    Alrms.Description = "Speed (KM/H)";
                    DtaObj.LstAlarms.Add(Alrms);
                    break;
                case "12":
                    break;
                case "13":
                    break;
                case "30":
                    Alrms.AlrmCodes = AlarmCodes.Vib;
                    Alrms.Description = "Vibration";
                    DtaObj.LstAlarms.Add(Alrms);
                    break;
                case "50":
                    Alrms.AlrmCodes = AlarmCodes.Tmpr; //External Power Cut 
                    Alrms.Description = "Temper";
                    DtaObj.LstAlarms.Add(Alrms);
                    break;
                case "52":
                   
                    break;
                case "60":
                    Alrms.AlrmCodes = AlarmCodes.FatgDrv;
                    Alrms.Description = "Fatigue Driving";
                    DtaObj.LstAlarms.Add(Alrms);
                    break;
                case "71":
                    Alrms.AlrmCodes = AlarmCodes.Crsh;
                    Alrms.Description = "Crash";
                    DtaObj.LstAlarms.Add(Alrms);
                    break;
                case "72":
                    Alrms.AlrmCodes = AlarmCodes.HrdAcc;
                    Alrms.Description = "Hard Acceleration";
                    DtaObj.LstAlarms.Add(Alrms);
                    break;
                case "81":
                    Alrms.AlrmCodes = AlarmCodes.FuelLosAlarm;
                    Alrms.Description = "Fuel Loss Alarm";
                    DtaObj.LstAlarms.Add(Alrms);
                    break;            

            }
        }
    }
}
