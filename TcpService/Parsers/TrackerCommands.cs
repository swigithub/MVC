using AirViewTracker.DBLayer.Fleet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpService
{
    internal class TrackerCommands
    {
        CommonOperation ObjComnOp;
        string ExceptionLogPath = "";
        string LogFilePath = "";
        string SgnalRApi = "";
        List<ClientInfo> ClientLst;
        bool WriteLogFile;
        bool WriteExceptionLog;
        List<String> DeviceIDCollection;
        FM_VehicleBL ObjVehicles;
        bool ChkIndividualTrackerFromDb;
        List<string> AlarmsWthFloatVal;
        private static readonly Object LockThread= new Object();
        int IdlTime = 0;
        //ArmCommand
        public TrackerCommands(string ExceptionLogPth, string FilePth,string SignalRAPi,ref List<ClientInfo> LstClntInfo,bool WriteLgFile,bool WriteExcetionLg, List<string> IMEICollect,string ConnectionStr,bool CheckIndividualTrackerFromDb, int IdleTime)
        {
            ExceptionLogPath = ExceptionLogPth;            
            LogFilePath = FilePth;
            SgnalRApi = SignalRAPi;
            ObjComnOp = new CommonOperation(LogFilePath, ExceptionLogPath, SignalRAPi);
            ClientLst = LstClntInfo;
            WriteLogFile = WriteLgFile;
            WriteExceptionLog = WriteExcetionLg;
            DeviceIDCollection = IMEICollect;
            ObjVehicles = new FM_VehicleBL(ConnectionStr);
            ChkIndividualTrackerFromDb = CheckIndividualTrackerFromDb;
            IdlTime = IdleTime;
            AddFloatAlarmValues();

        }

        public void AddFloatAlarmValues()
        {
            AlarmsWthFloatVal = new List<string>();
            AlarmsWthFloatVal.Add("LwVltg");
            AlarmsWthFloatVal.Add("HrdAcc");
            AlarmsWthFloatVal.Add("HrdDecel");
            AlarmsWthFloatVal.Add("HrdDecel");
            AlarmsWthFloatVal.Add("QkLnChg");
            AlarmsWthFloatVal.Add("ShrpTrn");

        }
        internal List<List<byte>> ArmDisarmCar(List<String> IMEIColl, int StatusArmDisArm)
        {
            try
            {
                return ArmDisArmCommand(IMEIColl, StatusArmDisArm);
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog) {
                    string exception = Environment.NewLine + DateTime.Now.ToString() +" ArmDisarmCar : " + ex.ToString();
                    ObjComnOp.WriteExceptionFile(exception);                    
                }
                return null;

            }
        }

        private List<List<byte>> ArmDisArmCommand(List<String> IMEIColl, int StatusArmDisArm)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();

            foreach (string item in IMEIColl)
            {
                //40 40 00 12 58 04 20 80 04 57 92 41 16 01 B8 F2 0D 0A 
                List<Byte> ByteList = new List<byte>();
                ByteList.Add(0x40);
                ByteList.Add(0x40);
                ByteList.Add(0x00);
                ByteList.Add(0x12);

                // Create IMEI Byte
                byte[] IMEIArr = CommonOperation.StringToByteArray(item);

                foreach (byte bte in IMEIArr)
                {
                    ByteList.Add(bte);
                }

                //4116 
                ByteList.Add(0x41);
                ByteList.Add(0x16);
                //ArmCommand

                if (StatusArmDisArm == 1)
                {
                    ByteList.Add(0x01);
                }
                else if (StatusArmDisArm == 0)
                {
                    ByteList.Add(0x00);
                }
                else if (StatusArmDisArm == 2)
                {
                    ByteList.Add(0x02);
                }

                //01 B8 F2 0D 0A
                ByteList.Add(0xB8);
                ByteList.Add(0xF2);
                //'\r\n'
                ByteList.Add(0x0D);
                ByteList.Add(0x0A);

                CommandsList.Add(ByteList);
            }

            return CommandsList;
        }

        internal List<List<byte>> SetGprsTimeIntervalPacket(List<String> IMEIColl, string TimeToSet)
        {
            try
            {
                int TimeToSt = Convert.ToInt32(TimeToSet);

                string HexString = TimeToSt.ToString("X");

                if (HexString.Length == 1)
                {
                    HexString = "000" + HexString;
                }
                else if (HexString.Length == 2)
                {
                    HexString = "00" + HexString;
                }
                else if (HexString.Length == 3)
                {
                    HexString = "0" + HexString;
                }

                return SetGprsTimeIntervalCommand(IMEIColl, TimeToSt.ToString("X"));
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() +" ArmDisarmCar : " + ex.ToString();
                    ObjComnOp.WriteExceptionFile(exception);
                }
                return null;
            }
        }
        private List<List<Byte>> SetGprsTimeIntervalCommand(List<String> IMEIColl, string TimeToSet)

        {
            List<List<Byte>> CommandsList = new List<List<byte>>();

            foreach (string item in IMEIColl)
            {
                //40 40 00 12 58 04 20 80 04 57 92 41 05 0B A8 51 0D 0A
                List<Byte> ByteList = new List<byte>();
                ByteList.Add(0x40);
                ByteList.Add(0x40);
                ByteList.Add(0x00);
                ByteList.Add(0x13);


                // Create IMEI Byte
                byte[] IMEIArr = CommonOperation.StringToByteArray(item);

                foreach (byte bte in IMEIArr)
                {
                    ByteList.Add(bte);
                }

                //41 02
                ByteList.Add(0x41);
                ByteList.Add(0x02);

                //Time
                byte[] SpeedArr = CommonOperation.StringToByteArray(TimeToSet);

                if (SpeedArr.Length > 1)
                {
                    foreach (byte Spd in SpeedArr)
                    {
                        ByteList.Add(Spd);
                    }
                }
                else if (SpeedArr.Length == 1)
                {
                    ByteList.Add(0x00);

                    foreach (byte Spd in SpeedArr)
                    {
                        ByteList.Add(Spd);
                    }

                }

                //ByteList.Add( Convert.ToByte(SpeedToHexaDecimal(Speed)));
                //ByteList.Add(0x02);
                //CheckSum Values 51 0D 0A
                ByteList.Add(0xA4);
                ByteList.Add(0xC3);

                //'\r\n'

                ByteList.Add(0x0D);
                ByteList.Add(0x0A);

                CommandsList.Add(ByteList);
            }

            return CommandsList;

        }

        internal void HandleIncomingSinoCastelPacket(string[] HexCollection, byte[] bytesFrom,string TrackerHexaResp,string DatafrmClnt,ref ClientInfo tpclient, NetworkStream networkStream,int SinocastelGpsPackageInterval)
        {
            try
            {
                string data;
                string ClientSideTrackerID;
                string TrackerHexaResponse = TrackerHexaResp;
                string dataFromClient = DatafrmClnt;

                //Login Pkt
                if (HexCollection[25] == "10" && HexCollection[26] == "01")
                {
                    //ClientSideTrackerID = BitConverter.ToString(bytesFrom, 5, 20).Replace("-", "");
                    ClientSideTrackerID = System.Text.Encoding.ASCII.GetString(bytesFrom, 5, 20);
                    ClientSideTrackerID = ClientSideTrackerID.Replace("\0", "");

                    if (WriteLogFile)
                    {
                        data = "";
                        data = Environment.NewLine + DateTime.Now.ToString() + " Client: Login ";
                        data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                        data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                        data = data + Environment.NewLine + "TrackerId: " + ": " + ClientSideTrackerID;

                        data = data + Environment.NewLine;
                        ObjComnOp.WriteFile(data);
                    }
                                                          
                    if (ChkIndividualTrackerFromDb)
                    {
                        bool ClientIMEI = ObjVehicles.ValidateVehicleIMEI("Validate_IMEI", ClientSideTrackerID);

                        if (ClientIMEI)
                        {
                            DeviceIDCollection.Add(ClientSideTrackerID);

                            string counter = "";
                            counter = ObjVehicles.GetCurrentTripCounterForClient("GetCurrentTrackerTrips", ClientSideTrackerID, DateTime.UtcNow.ToString("d"));

                            if (string.IsNullOrEmpty(counter))
                            {
                                tpclient.CurrentTripCounter = -1;                                
                            }
                            else
                            {
                                tpclient.CurrentTripCounter = (Convert.ToInt16(counter) + 1);
                            }

                            counter = ObjVehicles.GetTripIdleValue("GetTripIdleConfig", ClientSideTrackerID);

                            if (string.IsNullOrEmpty(counter))
                            {
                                tpclient.TripIdleTime = IdlTime;
                            }
                            else
                            {
                                tpclient.TripIdleTime =(int)Convert.ToDecimal(counter);
                            }


                            tpclient.IMEI = ClientSideTrackerID;
                            Byte[] ConfirmationCommanad = SinocastalParseLoginRequestAndConfirm(bytesFrom, TrackerHexaResponse, ClientSideTrackerID, tpclient);                            
                            networkStream.Write(ConfirmationCommanad, 0, ConfirmationCommanad.Length);
                            //ClientLst.RemoveAll(x => x.IMEI == ClientSideTrackerID);
                            
                            tpclient.Manufacturer = TrackerManufacturer.Sinocastel;
                            networkStream.Flush();                           
                            //Set Gps Time Interval
                            Thread.Sleep(2000);
                            ConfirmationCommanad = CreateGpsTimeIntervalCommand(bytesFrom, SinocastelGpsPackageInterval);
                            //TripHandling(ref DataPktobj, ref tpclnt);
                            networkStream.Write(ConfirmationCommanad, 0, ConfirmationCommanad.Length);
                            networkStream.Flush();
                            
                           
                            
                            //tpclient.IMEI = ClientSideTrackerID;
                        }
                        else
                        {
                            tpclient.IMEI = ClientSideTrackerID;
                            tpclient.Manufacturer = TrackerManufacturer.Sinocastel;
                            ClientLst.RemoveAll(x => x.IMEI == ClientSideTrackerID);
                        }
                    }
                    else
                    {
                        tpclient.IMEI = ClientSideTrackerID;
                        ClientLst.RemoveAll(x => x.IMEI == ClientSideTrackerID);
                    }
                    
                }

                //Sinocastel GpsPacket
                if (HexCollection[25] == "40" && HexCollection[26] == "01")
                {
                    ClientSideTrackerID = System.Text.Encoding.ASCII.GetString(bytesFrom, 5, 20);
                    ClientSideTrackerID = ClientSideTrackerID.Replace("\0", "");

                    if (WriteLogFile)
                    {
                        data = "";
                        data = Environment.NewLine + DateTime.Now.ToString() + " Client: GpsPacket ";
                        data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                        data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                        data = data + Environment.NewLine + "TrackerId: " + ": " + ClientSideTrackerID;
                        data = data + Environment.NewLine;
                        ObjComnOp.WriteFile(data);
                    }

                    ParseGpsPacket(ref tpclient,HexCollection, TrackerHexaResponse, ClientSideTrackerID, true,false,false);
                }

                if (HexCollection[25] == "10" && HexCollection[26] == "03")
                {
                    ClientSideTrackerID = System.Text.Encoding.ASCII.GetString(bytesFrom, 5, 20);
                    ClientSideTrackerID = ClientSideTrackerID.Replace("\0", "");

                    Byte[] ConfirmationCommanad = SinocastelHeartBeatReponse(bytesFrom);
                    networkStream.Write(ConfirmationCommanad, 0, ConfirmationCommanad.Length);
                    networkStream.Flush();

                    if (WriteLogFile)
                    {
                        data = "";
                        data = Environment.NewLine + DateTime.Now.ToString() + " Client: HeartBeat Packet ";
                        data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                        data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                        data = data + Environment.NewLine + "TrackerId: " + ": " + ClientSideTrackerID;
                        data = data + Environment.NewLine;
                        ObjComnOp.WriteFile(data);
                    }
                    //ObjTrackerCommands.ParseGpsPacket(HexCollection);
                }

                if (HexCollection[25] == "10" && HexCollection[26] == "03")
                {
                    ClientSideTrackerID = System.Text.Encoding.ASCII.GetString(bytesFrom, 5, 20);
                    ClientSideTrackerID = ClientSideTrackerID.Replace("\0", "");

                    if (WriteLogFile)
                    {
                        data = "";
                        data = Environment.NewLine + DateTime.Now.ToString() + " Client: HeartBeat Packet ";
                        data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                        data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                        data = data + Environment.NewLine + "TrackerId: " + ": " + ClientSideTrackerID;
                        data = data + Environment.NewLine;
                        ObjComnOp.WriteFile(data);
                    }
                    //ObjTrackerCommands.ParseGpsPacket(HexCollection);
                }

                if (HexCollection[25] == "10" && HexCollection[26] == "02")
                {
                    ClientSideTrackerID = System.Text.Encoding.ASCII.GetString(bytesFrom, 5, 20);
                    ClientSideTrackerID = ClientSideTrackerID.Replace("\0", "");

                    if (WriteLogFile)
                    {
                        data = "";
                        data = Environment.NewLine + DateTime.Now.ToString() + " Client:LogOut packet ";
                        data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                        data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                        data = data + Environment.NewLine + "TrackerId: " + ": " + ClientSideTrackerID;
                        data = data + Environment.NewLine;
                        ObjComnOp.WriteFile(data);
                    }
                    tpclient.objTrackerTrip = null;
                    ClientLst.RemoveAll(x => x.IMEI == ClientSideTrackerID);
                    //ObjTrackerCommands.ParseGpsPacket(HexCollection);
                }

                if (HexCollection[25] == "40" && HexCollection[26] == "02")
                {
                    ClientSideTrackerID = System.Text.Encoding.ASCII.GetString(bytesFrom, 5, 20);
                    ClientSideTrackerID = ClientSideTrackerID.Replace("\0", "");

                    if (WriteLogFile)
                    {
                        data = "";
                        data = Environment.NewLine + DateTime.Now.ToString() + " Client:PID Data ";
                        data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                        data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                        data = data + Environment.NewLine + "TrackerId: " + ": " + ClientSideTrackerID;
                        data = data + Environment.NewLine;
                        ObjComnOp.WriteFile(data);
                    }
                    //ObjTrackerCommands.ParseGpsPacket(HexCollection);
                }

                if (HexCollection[25] == "40" && HexCollection[26] == "03")
                {
                    ClientSideTrackerID = System.Text.Encoding.ASCII.GetString(bytesFrom, 5, 20);
                    ClientSideTrackerID = ClientSideTrackerID.Replace("\0", "");

                    if (WriteLogFile)
                    {
                        data = "";
                        data = Environment.NewLine + DateTime.Now.ToString() + " Client:G Sensor Data ";
                        data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                        data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                        data = data + Environment.NewLine + "TrackerId: " + ": " + ClientSideTrackerID;
                        data = data + Environment.NewLine;
                        ObjComnOp.WriteFile(data);
                    }
                    //ObjTrackerCommands.ParseGpsPacket(HexCollection);
                }                

                //TrackOnDemand
                if (HexCollection[25] == "B0" && HexCollection[26] == "01")
                {
                    ClientSideTrackerID = System.Text.Encoding.ASCII.GetString(bytesFrom, 5, 20);
                    ClientSideTrackerID = ClientSideTrackerID.Replace("\0", "");

                    if (WriteLogFile)
                    {
                        data = "";
                        data = Environment.NewLine + DateTime.Now.ToString() + " Client: GpsPacket on Demand ";
                        data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                        data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                        data = data + Environment.NewLine + "TrackerId: " + ": " + ClientSideTrackerID;
                        data = data + Environment.NewLine;
                        ObjComnOp.WriteFile(data);
                    }

                    ParseGpsPacket( ref tpclient,HexCollection, TrackerHexaResponse, ClientSideTrackerID, false,true,false);
                }

                //Alarms
                if (HexCollection[25] == "40" && HexCollection[26] == "07")
                {
                    ClientSideTrackerID = System.Text.Encoding.ASCII.GetString(bytesFrom, 5, 20);
                    ClientSideTrackerID = ClientSideTrackerID.Replace("\0", "");

                    if (WriteLogFile)
                    {
                        data = "";
                        data = Environment.NewLine + DateTime.Now.ToString() + " Client: GpsPacket on Demand ";
                        data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                        data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                        data = data + Environment.NewLine + "TrackerId: " + ": " + ClientSideTrackerID;
                        data = data + Environment.NewLine;
                        ObjComnOp.WriteFile(data);
                    }

                    ParseGpsPacket(ref tpclient,HexCollection, TrackerHexaResponse, ClientSideTrackerID, false,false, true );
                    Byte[] ConfirmationCommanad = AlarmResponse(bytesFrom);
                    networkStream.Write(ConfirmationCommanad, 0, ConfirmationCommanad.Length);
                    networkStream.Flush();
                }
                //Response of Alarms
                if (HexCollection[25] == "A0" && HexCollection[26] == "01")
                {
                    ClientSideTrackerID = System.Text.Encoding.ASCII.GetString(bytesFrom, 5, 20);
                    ClientSideTrackerID = ClientSideTrackerID.Replace("\0", "");                    

                    if (HexCollection.Length>35)
                    {
                        if (HexCollection[29] == "01")
                        {
                            string ResponseAlarmCode = HexCollection[31].ToString() + HexCollection[30].ToString();
                            string AlarmVal = UpdateIsAppliedFlag(ClientSideTrackerID, ResponseAlarmCode);

                            if (WriteLogFile)
                            {
                                data = "";
                                data = Environment.NewLine + DateTime.Now.ToString()+ "  TrackerId: " + ": " + ClientSideTrackerID + " Client: Alarm Response, Alarm Type:"+ AlarmVal;
                                data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');                                
                                data = data + Environment.NewLine;
                                ObjComnOp.WriteFile(data);
                            }
                        }

                    }
                }

                }
            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + " HandleIncomingSinoCastelPacket : " + ex.ToString();
                    ObjComnOp.WriteExceptionFile(exception);
                }
            }
        }
      
        internal Byte[] SinocastalParseLoginRequestAndConfirm(byte[] ReceivedDtaPkt, string TrackerHexaResponse, string TrackerId, ClientInfo tpclient)
        {
            string[] HexCollection = TrackerHexaResponse.Split(' ');
            Byte[] ResponseStream;
            string ServerHexaResponse = "";
            string data = "";
            Byte[] IPByt;
            string PortNoHex = "";
          
            //IPByt = ParseIP(IpAddress);
            //PortNoHex = PortNo.ToString("X");
            ResponseStream = SinocastalCreateLoginConfirmation(ReceivedDtaPkt);
            //conversion to Hexa from Server Response Byte
            ServerHexaResponse = BitConverter.ToString(ResponseStream, 0, ResponseStream.Length).Replace("-", " ");

            #region "File Writing"
            if (WriteLogFile)
            {
                data = Environment.NewLine + DateTime.Now.ToString() + " Server: Confirmation ";
                data = data + Environment.NewLine + "Hexa: " + ": " + ServerHexaResponse;
                data = data + Environment.NewLine + "TrackerId: " + ": " + TrackerId;
                data = data + Environment.NewLine;
                ObjComnOp.WriteFile(data);
            }
            #endregion
            //File.AppendAllText(LogFilePath, data);
            DataPacket DataPktobj= ParseGpsPacket(ref tpclient,HexCollection,  TrackerHexaResponse, TrackerId,false,false,false);
            //parseStatusData(HexCollection, DataPktobj, false);

            // To keep record of Track tab
            if (tpclient.CurrentTripCounter < 0)
            {
                SettingNewTripCurrentValues(DataPktobj, tpclient, (tpclient.CurrentTripCounter) * -1);
            }
            else
            {
                SettingNewTripCurrentValues(DataPktobj, tpclient, (tpclient.CurrentTripCounter));
            }
            
            TripHandling(ref DataPktobj, ref tpclient);
            //SetTripValuesInClient(DataPktobj, tpclient, true);            
            //TrackerTrip objTkrTrip = new TrackerTrip();
           // tpclient.objTrackerTrip.TripStartTime = DataPktobj.UTCTimeAndDate;
            return ResponseStream;

        }

        private Byte[] SinocastalCreateLoginConfirmation(byte[] ReceivedDtaPkt)
        {
            List<Byte> ByteList = new List<byte>();
            ByteList.Add(0x40);
            ByteList.Add(0x40);
            ByteList.Add(0x29);
            ByteList.Add(0x00);
            ByteList.Add(0x03);

            for (int counter = 5; counter <= 24; counter++)
            {
                ByteList.Add(ReceivedDtaPkt[counter]);
            }

            //Confirmation Protocol
            ByteList.Add(0x90);
            ByteList.Add(0x01);

            //IpAddress            
            ByteList.Add(0xFF);
            ByteList.Add(0xFF);
            ByteList.Add(0xFF);
            ByteList.Add(0xFF);

            //PortNumber
            ByteList.Add(0x00);
            ByteList.Add(0x00);          

            //TimeStamp
            int UnixDateTime = Convert.ToInt32(ObjComnOp.DateTimeToUnixTimestamp(DateTime.Now));
            String HexTimeStamp = UnixDateTime.ToString("X");

            byte[] HexTimeStamprBytes = CommonOperation.StringToByteArray(HexTimeStamp);

            for (int counter = HexTimeStamprBytes.Length - 1; counter >= 0; counter--)
            {
                ByteList.Add(HexTimeStamprBytes[counter]);
            }

            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList= CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte item in CRCCollection)
            {
                ByteList.Add(item);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);
            return ByteList.ToArray();
        }
        private byte[] ParseIP(string IPAddress)
        {
            string[] ipcoll = IPAddress.Split('.');
            List<byte> ipByteColl = new List<byte>();

            foreach (string val in ipcoll)
            {
                string HexIp = Convert.ToInt16(val).ToString("X");
                byte[] Collection = CommonOperation.StringToByteArray(HexIp);

                foreach (byte item in Collection)
                {
                    ipByteColl.Add(item);
                }
            }

            return ipByteColl.ToArray();
        }

        private void parseStatusData(ref ClientInfo tpclnt,string[] HexCollection, DataPacket DataPktobj,bool GpsPkt, bool TrackOnDmnd = false,bool AlarmPkt= false)
        { //27 to 60
            InputOutputStatus objStatus = new InputOutputStatus();
            List<VehicleStates> LstVStat = new List<VehicleStates>();
            string TrackerTime = "";
            DateTime GMTDateTime;
            double TotalMileageKM = 0;
            double CurrentTripMil = 0;
            //Acc On time

            if (!GpsPkt && !TrackOnDmnd && !AlarmPkt)
            {
                for (int counter = 30; counter >= 27; counter--)
                {
                    TrackerTime = TrackerTime + HexCollection[counter];
                }
            }
            else if (GpsPkt)
            {
                for (int counter = 31; counter >= 28; counter--)
                {
                    TrackerTime = TrackerTime + HexCollection[counter];
                }
            }
            else if (TrackOnDmnd)
            {

                for (int counter = 32; counter >= 29; counter--)
                {
                    TrackerTime = TrackerTime + HexCollection[counter];
                }

            }
            else if (AlarmPkt)
            {

                for (int counter = 34; counter >= 31; counter--)
                {
                    TrackerTime = TrackerTime + HexCollection[counter];
                }

            }

            //Conversion to Decimal
            Int64 UnixDateTime = CommonOperation.HexaDecimalToDecimalConversion(TrackerTime);
            DateTime dt = ObjComnOp.UnixTimeStampToDateTime(UnixDateTime);

            //UtC Time
            TrackerTime = "";
            if (!GpsPkt && !TrackOnDmnd && !AlarmPkt)
            {
                for (int counter = 34; counter >= 31; counter--)
                {
                    TrackerTime = TrackerTime + HexCollection[counter];
                }
            }

            else if(GpsPkt)
            {
                for (int counter = 35; counter >= 32; counter--)
                {
                    TrackerTime = TrackerTime + HexCollection[counter];
                }

            }
            else if (TrackOnDmnd)
            {
                for (int counter = 36; counter >= 33; counter--)
                {
                    TrackerTime = TrackerTime + HexCollection[counter];
                }
            }

            else if (AlarmPkt)
            {
                for (int counter = 38; counter >= 35; counter--)
                {
                    TrackerTime = TrackerTime + HexCollection[counter];
                }
            }
            //Conversion to Decimal
            UnixDateTime = CommonOperation.HexaDecimalToDecimalConversion(TrackerTime);            
            DataPktobj.Odometer = TotalMileageInKM(HexCollection, GpsPkt, TrackOnDmnd, AlarmPkt);//TotalMilage
            DataPktobj.CurrentTripMileage = CurrentTripMilleage(HexCollection, GpsPkt, TrackOnDmnd, AlarmPkt);
            DataPktobj.TotalFuelConsumed=TotalFuelConsumed(HexCollection, GpsPkt, TrackOnDmnd, AlarmPkt);
            DataPktobj.CurrentTripFuelConsumed = CurrentTripFuelConsumer(HexCollection, GpsPkt, TrackOnDmnd, AlarmPkt);
            
            VStatus(HexCollection, LstVStat, GpsPkt, TrackOnDmnd,AlarmPkt);

            if (LstVStat.Count>0)
            {
                DataPktobj.LstVState = LstVStat;
            }

            objStatus.InEngineOnOff = tpclnt.IsEngineOn;
            DataPktobj.ObjInpOutStatus = objStatus;
            //DataPktobj.ObjAlarms = objAlarm;
        }

        private double TotalMileageInKM(string[] HexCollection,bool gpsPkt, bool TrackOnDmnd = false, bool AlarmPkt = false)
        {
            long TotMil = 0;
            string StrMileage = "";

            if (!gpsPkt && !TrackOnDmnd && !AlarmPkt)
            {
                for (int counter = 38; counter >= 35; counter--)
                {
                    StrMileage = StrMileage + HexCollection[counter];
                }
            }
            else if(gpsPkt)
            {
                for (int counter = 39; counter >= 36; counter--)
                {
                    StrMileage = StrMileage + HexCollection[counter];
                }
            }
            else if (TrackOnDmnd)
            {
                for (int counter = 40; counter >= 37; counter--)
                {
                    StrMileage = StrMileage + HexCollection[counter];
                }

            }
            else if (AlarmPkt)
            {
                for (int counter = 42; counter >= 39; counter--)
                {
                    StrMileage = StrMileage + HexCollection[counter];
                }

            }

            TotMil = CommonOperation.HexaDecimalToDecimalConversion(StrMileage);
            return TotMil / 1000;
        }

        private double CurrentTripMilleage(string[] HexCollection,bool GpsPkt, bool TrackOnDmnd = false, bool AlarmPkt = false)
        {
            double CurrentTripMilleage = 0;
            string StrCurrentTripMil = "";
            int HighCounter = 0;
            int EndCounter = 0;

            if (!GpsPkt && !TrackOnDmnd && !AlarmPkt)
            {
                HighCounter = 42;
                EndCounter = 39;
            }
            else if(GpsPkt)
            {
                HighCounter = 43;
                EndCounter = 40;
            }
             else if (TrackOnDmnd)
            {
                HighCounter = 44;
                EndCounter = 41;
            }

            else if (AlarmPkt)
            {
                HighCounter = 46;
                EndCounter = 43;
            }
            for (int counter = HighCounter; counter >= EndCounter; counter--)
            {
                StrCurrentTripMil = StrCurrentTripMil + HexCollection[counter];
            }

            CurrentTripMilleage = CommonOperation.HexaDecimalToDecimalConversion(StrCurrentTripMil);
            return CurrentTripMilleage / 1000;
        }

      

        private double TotalFuelConsumed(string[] HexCollection,bool GpsPkt, bool TrackOnDmnd = false, bool AlarmPkt = false)
        {
            double TotalFuelConsumed = 0;
            string StrTotalFuelConsumed = "";


            int HighCounter = 0;
            int EndCounter = 0;

            if (!GpsPkt && !TrackOnDmnd && !AlarmPkt)
            {
                HighCounter = 46;
                EndCounter = 43;
            }
            else if (GpsPkt)
            {
                HighCounter = 47;
                EndCounter = 44;
            }
            else if (TrackOnDmnd)
            {
                HighCounter = 48;
                EndCounter = 45;
            }
            else if (AlarmPkt)
            {
                HighCounter = 50;
                EndCounter = 47;
            }

                for (int counter = HighCounter; counter >= EndCounter; counter--)
            {
                StrTotalFuelConsumed = StrTotalFuelConsumed + HexCollection[counter];
            }

            TotalFuelConsumed = CommonOperation.HexaDecimalToDecimalConversion(StrTotalFuelConsumed);
            return TotalFuelConsumed *0.01;            
        }

        private double CurrentTripFuelConsumer(string[] HexCollection, bool GpsPacket, bool TrackOnDmnd = false, bool AlarmPkt = false)
        {
            double CurrentTripFuelConsumed = 0;
            string StrCurrentTripFuelConsumed = "";
            int HighCounter = 0;
            int EndCounter = 0;

            if (!GpsPacket && !TrackOnDmnd && !AlarmPkt) 
            {
                HighCounter = 48;
                EndCounter = 47;
            }
            else if (GpsPacket)
            {
                HighCounter = 49;
                EndCounter = 48;
            }
            else if (TrackOnDmnd)
            {
                HighCounter = 50;
                EndCounter = 49;

            }

            else if (AlarmPkt)
            {
                HighCounter = 52;
                EndCounter = 51;

            }

            for (int counter = HighCounter; counter >= EndCounter; counter--)
            {
                StrCurrentTripFuelConsumed = StrCurrentTripFuelConsumed + HexCollection[counter];
            }

            CurrentTripFuelConsumed = CommonOperation.HexaDecimalToDecimalConversion(StrCurrentTripFuelConsumed);
            return CurrentTripFuelConsumed *0.01;

        }

        private void VStatus(string[] HexCollection, List<VehicleStates> LstVStatus, bool gpspkt,bool TrackOnDmnd=false, bool AlarmPkt = false)
        {
            int ByteCounter = 0;
            string HexaValue = "";
            int HighCounter = 0;
            int lowCounter = 0;

            if (!gpspkt && !TrackOnDmnd && !AlarmPkt)
            {
                HighCounter = 52;
                lowCounter = 49;
            }
            else if(gpspkt)
            {
                HighCounter = 53;
                lowCounter = 50;
            }
            else if (TrackOnDmnd)
            {
                HighCounter = 54;
                lowCounter = 51;
            }

            else if (AlarmPkt)
            {
                HighCounter = 56;
                lowCounter = 53;
            }

            for (int counter = lowCounter; counter<= HighCounter; counter++)
            {                
                HexaValue = HexCollection[counter];
                String HexaBytes =CommonOperation.HexStringToBinary(HexaValue);
                if (!gpspkt && !TrackOnDmnd)
                {
                    switch (counter)
                    {
                        case 49:
                            VehicleStatusS0(HexaBytes, LstVStatus);
                            break;
                        case 50:
                            VehicleStatusS1(HexaBytes, LstVStatus);
                            break;
                        case 51:
                            VehicleStatusS2(HexaBytes, LstVStatus);
                            break;
                        case 52:
                            VehicleStatusS3(HexaBytes, LstVStatus);
                            break;
                    }
                }
                else if (gpspkt)
                {
                    switch (counter)
                    {
                        case 50:
                            VehicleStatusS0(HexaBytes, LstVStatus);
                            break;
                        case 51:
                            VehicleStatusS1(HexaBytes, LstVStatus);
                            break;
                        case 52:
                            VehicleStatusS2(HexaBytes, LstVStatus);
                            break;
                        case 53:
                            VehicleStatusS3(HexaBytes, LstVStatus);
                            break;
                    }

                }  
                else if(TrackOnDmnd)
                {
                    switch (counter)
                    {
                        case 51:
                            VehicleStatusS0(HexaBytes, LstVStatus);
                            break;
                        case 52:
                            VehicleStatusS1(HexaBytes, LstVStatus);
                            break;
                        case 53:
                            VehicleStatusS2(HexaBytes, LstVStatus);
                            break;
                        case 54:
                            VehicleStatusS3(HexaBytes, LstVStatus);
                            break;
                    }
                }
                else if (AlarmPkt)
                {
                    switch (counter)
                    {
                        case 53:
                            VehicleStatusS0(HexaBytes, LstVStatus);
                            break;
                        case 54:
                            VehicleStatusS1(HexaBytes, LstVStatus);
                            break;
                        case 55:
                            VehicleStatusS2(HexaBytes, LstVStatus);
                            break;
                        case 56:
                            VehicleStatusS3(HexaBytes, LstVStatus);
                            break;
                    }
                }
            }
        }

        private void VehicleStatusS0(string BytesCollection, List<VehicleStates> lstVStatus)
        {
            int counter = 7;
            bool IsBitOn = false;
            foreach (char chr in BytesCollection)
            {
                switch (counter)
                {
                    case 7:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));

                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.ExhstEmson;
                            objVState.Description = "ExhaustEmission";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }
                        
                        break;
                    case 6:
                        IsBitOn  = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.IdlEngn;
                            objVState.Description = "Idle Engine";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }                        
                        break;
                    case 5:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.HrdDecel;
                            objVState.Description = "Hard Decelration";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }
                      
                        break;
                    case 4:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.HrdAcc;
                            objVState.Description = "Hard Accelration";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }                       
                        break;
                    case 3:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.EngnTmp;
                            objVState.Description = "Engine Temperature";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }                      
                        break;
                    case 2:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.Spd;
                            objVState.Description = "Speeding";
                            objVState.Status = true;       
                            lstVStatus.Add(objVState);
                        }                       
                        break;
                    case 1:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {                            
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.Twng;
                            objVState.Description = "Towing";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }                      
                        break;
                    case 0:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.LwVltg;
                            objVState.Description = "Low voltage";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }                       
                        break;
                }

                counter = counter - 1;
            }

        }

        private void VehicleStatusS1(string BytesCollection, List<VehicleStates> lstVStatus)
        {
            int counter = 7;
            bool IsBitOn = false;
            foreach (char chr in BytesCollection)
            {
                switch (counter)
                {
                    case 7:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {                            
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.Tmpr;
                            objVState.Description = "Temper";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }                       
                        break;
                    case 6:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.Crsh;
                            objVState.Description = "Crash";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);                            
                        }
                      
                        break;

                    case 5:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.Emrgncy;
                            objVState.Description = "Emergency";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);                            
                        }
                                          
                        break;

                    case 4:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.FatgDrv;
                            objVState.Description = "Fatigue Driving";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);                            
                        }
                       
                        break;

                    case 3:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.ShrpTrn;
                            objVState.Description = "Sharp Turn";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }
                        break;

                    case 2:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.QkLnChg;
                            objVState.Description = "Quick Lane Change";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }                       
                        break;

                    case 1:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.PwrOn;
                            objVState.Description = "Power On";
                            objVState.Status = true;
                            lstVStatus.Add(objVState); 
                        }                       
                        break;
                        
                    case 0:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.HghRPM;
                            objVState.Description = "High RPM";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }                      
                        break;
                }

                counter = counter - 1;
            }
        }

        private void VehicleStatusS2(string BytesCollection, List<VehicleStates> lstVStatus)
        {
            int counter = 7;
            bool IsBitOn = false;

            foreach (char chr in BytesCollection)
            {
                switch (counter)
                {
                    case 7:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.MIL;
                            objVState.Description = "MIL";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);                            
                        }
                       
                        break;

                    case 6:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {                            
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.OBDErr;
                            objVState.Description = "OBD communication error";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }
                      
                        break;

                    case 5:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.PwrOf;
                            objVState.Description = "Power Off";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }                      
                        break;
                    case 4:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.NoGPS;
                            objVState.Description = "No GPS";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);
                        }                       
                        break;
                    case 3:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.PrvcyStatus;
                            objVState.Description = "Privacy Status";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);                            
                        }
                      
                        break;
                    case 2:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.IgntOn;
                            objVState.Description = "Ignition ON";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);                            
                        }                        
                        break;
                    case 1:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.IllglIgnton;
                            objVState.Description = "Illegal Ignition";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);                            
                        }                      
                        break;
                    case 0:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.IllglEntr;
                            objVState.Description = "Illegal Enter";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);                          
                        }                      
                        break;
                }

                counter = counter - 1;
            }

        }

        private void VehicleStatusS3(string BytesCollection, List<VehicleStates> lstVStatus)
        {
            int counter = 7;
            bool IsBitOn = false;

            foreach (char chr in BytesCollection)
            {
                switch (counter)
                {
                    case 7:
                        break;
                    case 6:
                        break;
                    case 5:
                        break;
                    case 4:
                        break;
                    case 3:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.Vib;
                            objVState.Description = "Vibration";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);                            
                        }                       
                        break;
                    case 2:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.DngrousDrvng;
                            objVState.Description = "Dangerous Driving";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);                            
                        }                     
                        break;
                    case 1:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.NoCrd;
                            objVState.Description = "No card";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);                            
                        }                                                
                        break;
                    case 0:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                            VehicleStates objVState = new VehicleStates();
                            objVState.VStatusCode = AlarmCodes.UnLk;
                            objVState.Description = "Unlock";
                            objVState.Status = true;
                            lstVStatus.Add(objVState);                            
                        }                      
                        break;
                }
                counter = counter - 1;
            }
        }

        public DataPacket ParseGpsPacket(ref ClientInfo tpclnt,string[] HexCollection, string TrackerHexaResponse, string TrackerId, bool GpsPacket = false, bool TrackOnDmnd = false, bool AlarmPkt = false )
        {
            DataPacket DataPktobj = new DataPacket();            
            DataPktobj.UTCTimeAndDate = GpsPktTime(HexCollection, GpsPacket, TrackOnDmnd, AlarmPkt); ;           
            DataPktobj.Latitude = ParseLatitude(HexCollection, GpsPacket,TrackOnDmnd, AlarmPkt);            
            DataPktobj.Longitude = ParseLongitude(HexCollection, GpsPacket, TrackOnDmnd, AlarmPkt);            
            DataPktobj.Speed = SpeedInKmh(HexCollection, GpsPacket, TrackOnDmnd, AlarmPkt);

            if (GpsPacket == true || TrackOnDmnd == true )
            {
                TripHandling(ref DataPktobj, ref tpclnt);
            }
            
            DataPktobj.GPSSignalStatus = "Valid";
            DataPktobj.TrackerID = TrackerId;
            DataPktobj.TrackerStream = TrackerHexaResponse;
            DataPktobj.IsOfflineData = false;
            //Degrees(HexCollection);
            DataPktobj.Direction= Degrees(HexCollection, GpsPacket, TrackOnDmnd, AlarmPkt);
            DataPktobj.Rotation =DegreesToCardinal(DataPktobj.Direction, TrackOnDmnd);

            //Double Degree = Degrees(HexCollection);
            LatitudeAndLongitudeDirection(HexCollection,  DataPktobj.Latitude,  DataPktobj.Longitude, DataPktobj, GpsPacket, TrackOnDmnd, AlarmPkt);
            string data = "";

            if (WriteLogFile)
            {
                data = Environment.NewLine + DateTime.Now.ToString() + " Gps Data Parsed ";
                data = data + Environment.NewLine + "Date Time: " + ": " + DataPktobj.UTCTimeAndDate.ToString();
                data = data + Environment.NewLine + "Latitude: " + ": " + DataPktobj.Latitude.ToString();
                data = data + Environment.NewLine + "Longitutude: " + ": " + DataPktobj.Longitude.ToString();
                data = data + Environment.NewLine + "Speed: " + ": " + DataPktobj.Speed.ToString();
                data = data + Environment.NewLine + "Degree: " + ": " + DataPktobj.Direction.ToString();
                data = data + Environment.NewLine + "Direction: " + ": " + DataPktobj.Rotation.ToString();
                
            }
            //Track Tab DrivingTime
            parseStatusData(ref tpclnt, HexCollection, DataPktobj, GpsPacket, TrackOnDmnd, AlarmPkt);

            if (WriteLogFile)
            {
                data = data + Environment.NewLine + "Odometer: " + ": " + DataPktobj.Odometer.ToString();
                data = data + Environment.NewLine + "CurrentTripMilage : " + ": " + DataPktobj.CurrentTripMileage.ToString();
                data = data + Environment.NewLine + "TotalFuelConsumed: " + ": " + DataPktobj.TotalFuelConsumed.ToString();
                data = data + Environment.NewLine + "Current Trip Fuel Consumed: " + ": " + DataPktobj.CurrentTripFuelConsumed.ToString();
                data = data + Environment.NewLine;
                ObjComnOp.WriteFile(data);
            }

            if (AlarmPkt)
            {
                List<TrackerAlarms> LstAlrms = new List<TrackerAlarms>();
                AlarmParser(HexCollection, LstAlrms, ref tpclnt, ref DataPktobj);
                DataPktobj.LstAlarms = LstAlrms;
            }

           if (GpsPacket ==true || TrackOnDmnd == true ||  AlarmPkt == true)
           {
                SetTripValuesInClient(DataPktobj, tpclnt,false);
                
            }


            ObjComnOp.SendDataToApi(DataPktobj);
            return DataPktobj;
        }

        internal void TripHandling(ref DataPacket dtaPkt, ref ClientInfo tpclnt,bool IsNewIgnition = false)
        {
            TimeSpan TimeDiff;
            
            //First client connected and 
            if (tpclnt.LastPktWithZeroSpeedTime == DateTime.MinValue)
            {
                tpclnt.LastPktWithZeroSpeedTime = DateTime.Now;
                
//                tpclnt.CurrentTripCounter = -1;
                tpclnt.IsNewTrip = true;
                tpclnt.ContinuousZeroPkt = true;

            }
            
            //Change trip number as per the new packet
            if ((tpclnt.LstPktRcvdTime - tpclnt.PktBeforeLastRcvdTime).Days > 0)
            {
                tpclnt.CurrentTripCounter = -1;
                tpclnt.IsNewTrip = true;
            }

            //Car Started
            if (IsNewIgnition )
            {
                //Current Trip is 
                if (!tpclnt.IsNewTrip) {
                    TimeDiff = DateTime.UtcNow - tpclnt.PktBeforeLastRcvdTime;
                    tpclnt.LstPktRcvdTime = DateTime.Now;

                    if (TimeDiff.Minutes >= tpclnt.TripIdleTime)
                    {
                        // Idle Condition meet now check for the next day to reset the counter
                        if (TimeDiff.Days > 0)
                        {
                            tpclnt.CurrentTripCounter = -1;
                            tpclnt.IsNewTrip = true;
                        }
                        else
                        {
                            tpclnt.IsNewTrip = true;

                        }
                    }
                    else if (TimeDiff.Days > 0)
                    {
                        tpclnt.CurrentTripCounter = -1;
                        tpclnt.IsNewTrip = true;
                    }
                    else
                    {
                        tpclnt.IsNewTrip = true;
                    }
                }
                tpclnt.IsEngineOn = true;
                //dtaPkt.ObjInpOutStatus.InEngineOnOff = true; 
            }           

            if (dtaPkt.Speed == 0)
            {              
                //if the current trip is not new
                if (!tpclnt.IsNewTrip)
                {              

                    // if its the first packet with 0 speed set the zero pkt time
                    if (tpclnt.LastPktWithZeroSpeedTime == DateTime.MinValue)
                    {
                        tpclnt.LastPktWithZeroSpeedTime = DateTime.Now;
                        tpclnt.CurrentTripCounter = -1;
                        tpclnt.ContinuousZeroPkt = true;
                    }

                    //if (IsNewIgnition)
                    //{
                    //    tpclnt.LastPktWithZeroSpeedTime = DateTime.Now;
                    //    tpclnt.ContinuousZeroPkt = true;
                    //}

                    if (!tpclnt.ContinuousZeroPkt)
                    {
                        tpclnt.ContinuousZeroPkt = true;
                        tpclnt.LastPktWithZeroSpeedTime = DateTime.Now;
                    }

                    if (tpclnt.LastPktWithZeroSpeedTime != DateTime.MinValue)
                    {
                        TimeDiff = DateTime.UtcNow - tpclnt.LastPktWithZeroSpeedTime;
                        tpclnt.ContinuousZeroPkt = true;
                        //Checking the time difference in reference to the Idle Time Set 
                        if (TimeDiff.Minutes >= tpclnt.TripIdleTime)
                        {
                            // Idle Condition meet now check for the next day to reset the counter
                            if (TimeDiff.Days > 0)
                            {
                                tpclnt.CurrentTripCounter = -1;
                                tpclnt.IsNewTrip = true;
                            }
                            else
                            {
                                tpclnt.IsNewTrip = true;
                            }
                        }
                        else if (TimeDiff.Days > 0)
                        {
                            tpclnt.CurrentTripCounter = -1;
                            tpclnt.IsNewTrip = true;
                        }
                        else
                        {
                            //dtaPkt.objTrackerTrip.TripNo = tpclnt.CurrentTripCounter;
                        }
                    }
                }
            }

            else if (dtaPkt.Speed > 0 && tpclnt.IsNewTrip)
            {                               
                if (tpclnt.CurrentTripCounter == -1)
                {
                    SettingNewTripCurrentValues(dtaPkt, tpclnt,1);
                    tpclnt.CurrentTripCounter = 1; 
                    tpclnt.IsEngineOn = true;
                   
                }
                else
                {
                    //to get the exact trip counter from DB 
                    string counter = "";
                    counter = ObjVehicles.GetCurrentTripCounterForClient("GetCurrentTrackerTrips", tpclnt.IMEI, DateTime.UtcNow.ToString("d"));

                    if (string.IsNullOrEmpty(counter))
                    {
                        tpclnt.CurrentTripCounter = 1;
                    }
                    else
                    {
                        tpclnt.CurrentTripCounter = (Convert.ToInt16(counter) + 1);
                    }

                    counter = ObjVehicles.GetTripIdleValue("GetTripIdleConfig", tpclnt.IMEI);

                    if (string.IsNullOrEmpty(counter))
                    {
                        tpclnt.TripIdleTime = IdlTime;
                    }
                    else
                    {
                        tpclnt.TripIdleTime = Convert.ToInt16(counter);
                    }

                    //tpclnt.CurrentTripCounter = tpclnt.CurrentTripCounter + 1;
                    SettingNewTripCurrentValues(dtaPkt, tpclnt, (tpclnt.CurrentTripCounter));
                    tpclnt.IsNewTrip = false;
                    tpclnt.IsEngineOn = true;
                }

                tpclnt.ContinuousZeroPkt = false;
            }
            else if (dtaPkt.Speed > 0)
            {
                //dtaPkt.objTrackerTrip.TripNo = tpclnt.CurrentTripCounter;
                tpclnt.ContinuousZeroPkt = false;
            }
        }
        public string DegreesToCardinal(double degrees, bool TrackOnDmnd = false)
        {
            string[] caridnals = { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };
            return caridnals[(int)Math.Round(((double)degrees % 360) / 45)];
        }
        private DateTime GpsPktTime(string[] HexCollection,bool GpsPacket= false,bool TrackOnDmnd= false, bool AlarmPkt=false )
        {
            List<long> lstIntval = new List<long>();
            string Hexvalue = "";

            if (!GpsPacket && !TrackOnDmnd && !AlarmPkt)
            {                               
                for (int counter = 62; counter <= 67; counter++)
                {
                    Hexvalue = HexCollection[counter];
                    lstIntval.Add(CommonOperation.HexaDecimalToDecimalConversion(Hexvalue));
                }

                string Year = 20 + lstIntval[2].ToString();
                return new DateTime(Convert.ToInt16(Year), Convert.ToInt32(lstIntval[1]), Convert.ToInt32(lstIntval[0]), Convert.ToInt32(lstIntval[3]), Convert.ToInt32(lstIntval[4]), Convert.ToInt32(lstIntval[5]));
            }
            else if(GpsPacket)
            {
                for (int counter = 63; counter <= 68; counter++)
                {
                    Hexvalue = HexCollection[counter];
                    lstIntval.Add(CommonOperation.HexaDecimalToDecimalConversion(Hexvalue));
                }

                string Year = 20 + lstIntval[2].ToString();
                return new DateTime(Convert.ToInt16(Year), Convert.ToInt32(lstIntval[1]), Convert.ToInt32(lstIntval[0]), Convert.ToInt32(lstIntval[3]), Convert.ToInt32(lstIntval[4]), Convert.ToInt32(lstIntval[5]));                
            }
            else if(TrackOnDmnd)
            {
                for (int counter = 64; counter <= 69; counter++)
                {
                    Hexvalue = HexCollection[counter];
                    lstIntval.Add(CommonOperation.HexaDecimalToDecimalConversion(Hexvalue));
                }

                string Year = 20 + lstIntval[2].ToString();
                return new DateTime(Convert.ToInt16(Year), Convert.ToInt32(lstIntval[1]), Convert.ToInt32(lstIntval[0]), Convert.ToInt32(lstIntval[3]), Convert.ToInt32(lstIntval[4]), Convert.ToInt32(lstIntval[5]));
            }
            else if (AlarmPkt)
            {
                for (int counter = 66; counter <= 71; counter++)
                {
                    Hexvalue = HexCollection[counter];
                    lstIntval.Add(CommonOperation.HexaDecimalToDecimalConversion(Hexvalue));
                }

                string Year = 20 + lstIntval[2].ToString();
                return new DateTime(Convert.ToInt16(Year), Convert.ToInt32(lstIntval[1]), Convert.ToInt32(lstIntval[0]), Convert.ToInt32(lstIntval[3]), Convert.ToInt32(lstIntval[4]), Convert.ToInt32(lstIntval[5]));
            }
            else
            {
                return new DateTime();
            }
        }

        private Double ParseLatitude(string[] HexCollection,bool GpsPacket,bool TrackOnDmnd= false, bool AlarmPkt= false)
        {
            string Hexvalue = "";
            double Latitude = 0;
            int HighCounter = 0;
            int EndCounter = 0;

            if (!GpsPacket && !TrackOnDmnd && !AlarmPkt)
            {
                HighCounter = 71;
                EndCounter = 68;
            }
            else if(GpsPacket)
            {
                HighCounter = 72;
                EndCounter = 69;
            }
            else if (TrackOnDmnd)
            {
                HighCounter = 73;
                EndCounter = 70;
            }

            else if (AlarmPkt)
            {
                HighCounter = 75;
                EndCounter = 72;
            }

            for (int counter = HighCounter; counter >= EndCounter; counter--)
            {
                Hexvalue = Hexvalue + HexCollection[counter];
            }

            Latitude = CommonOperation.HexaDecimalToDecimalConversion(Hexvalue);
            return Latitude / 3600000;
            
        }

        private Double ParseLongitude(string[] HexCollection,bool GpsPacket,bool TrackOnDmnd= false, bool AlarmPkt = false)
        {
            string Hexvalue = "";
            double Longitude = 0;
            int HighCounter = 0;
            int EndCounter = 0;
                        
            if (!GpsPacket && !TrackOnDmnd && !AlarmPkt)
            {
                HighCounter = 75;
                EndCounter = 72;
            }
            else if (GpsPacket)
            {
                HighCounter = 76;
                EndCounter = 73;
            }
            else if(TrackOnDmnd)
            {
                HighCounter = 77;
                EndCounter = 74;
            }
            else if (AlarmPkt)
            {
                HighCounter = 79;
                EndCounter = 76;
            }

            for (int counter = HighCounter; counter >= EndCounter; counter--)
            {
                Hexvalue = Hexvalue + HexCollection[counter];
            }

            Longitude = CommonOperation.HexaDecimalToDecimalConversion(Hexvalue);
            return Longitude / 3600000;            
        }

        private Double SpeedInKmh(string[] HexCollection, bool GpsPacket,bool TrackOnDmnd= false, bool AlarmPkt = false)
        {
            string Hexvalue = "";
            double Speed = 0;
            int HighCounter = 0;
            int EndCounter = 0;
          
            if (!GpsPacket && !TrackOnDmnd && !AlarmPkt)
            {
                HighCounter = 77;
                EndCounter = 76;
            }
            else if (GpsPacket)
            {
                HighCounter = 78;
                EndCounter = 77;
            }
            else if (TrackOnDmnd)
            {
                HighCounter = 79;
                EndCounter = 78;
            }
            else if (AlarmPkt)
            {
                HighCounter = 81;
                EndCounter = 80;
            }

            for (int counter = HighCounter; counter >= EndCounter; counter--)
            {
                Hexvalue = Hexvalue + HexCollection[counter];
            }

            Speed = CommonOperation.HexaDecimalToDecimalConversion(Hexvalue);
            Double FinalSpeed = Speed * 0.036;
            return FinalSpeed ;
            
        }

        private Double Degrees(string[] HexCollection, bool GpsPacket, bool TrackOnDmnd = false,bool AlarmPkt= false)
        {
            string Hexvalue = "";
            double Degrees = 0;

            int HighCounter = 0;
            int EndCounter = 0;

            if (!GpsPacket && !TrackOnDmnd && !AlarmPkt)
            {
                HighCounter = 79;
                EndCounter = 78;
            }
            else if(GpsPacket)
            {
                HighCounter = 80;
                EndCounter = 79;
            }
            else if (TrackOnDmnd)
            {
                HighCounter = 81;
                EndCounter = 80;
            }
            else if (AlarmPkt)
            {
                HighCounter = 83;
                EndCounter = 82;
            }

            for (int counter = HighCounter; counter >= EndCounter; counter--)
            {
                Hexvalue = Hexvalue + HexCollection[counter];
            }

            Degrees = CommonOperation.HexaDecimalToDecimalConversion(Hexvalue);
            return Degrees/10;
        }

        private void LatitudeAndLongitudeDirection(string[] HexCollection, Double LatitudeVal,  Double Longval, DataPacket DataPktobj,bool GpsPkt, bool TrackOnDmnd = false,bool AlertPkt= false)
        {
            string Hexvalue = "";
            int counter = 7;

            if (!GpsPkt && !TrackOnDmnd && !AlertPkt)
            {
                Hexvalue = HexCollection[80];
            }
            else if (GpsPkt)
            {
                Hexvalue = HexCollection[81];
            }
            else if (TrackOnDmnd)
            {
                Hexvalue = HexCollection[82];
            }
            else if (AlertPkt)
            {
                Hexvalue = HexCollection[84];
            }
            String BinaryString = CommonOperation.HexStringToBinary(Hexvalue);

            foreach (char chr in BinaryString)
            {
                bool IsBitOn = false;
                string LatitudeDirection = "";
                string LongitudeDirection = "";

                switch (counter)
                {
                    case 1:
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        //Latitude
                        if (IsBitOn)
                        {
                            //LatitudeDirection = "N";
                            DataPktobj.LatitudeDirection = "N";
                        }
                        else
                        {
                            //LatitudeDirection = "S"; //*-1
                            DataPktobj.Latitude = LatitudeVal * -1;
                            DataPktobj.LatitudeDirection = "S";
                        }
                        break;

                    case 0:
                        //Longitude
                        IsBitOn = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                        if (IsBitOn)
                        {
                           // LongitudeDirection = "E";
                            DataPktobj.LongitudeDirection = "E";
                        }
                        else
                        {
                            //LongitudeDirection = "W"; //*-1
                            DataPktobj.Longitude = Longval * -1;
                            DataPktobj.LongitudeDirection = "W";
                        }
                        break;
                }

                counter = counter - 1;
            }
        }
        public  Byte[] CreateGpsTimeIntervalCommand(byte[] ReceivedDtaPkt, int TimeInSeconds=0)
        {            
            int TimeToSt = Convert.ToInt32(TimeInSeconds);
            string HexString = TimeToSt.ToString("X");

            if (HexString.Length == 1)
            {
                HexString = "000" + HexString;
            }
            else if (HexString.Length == 2)
            {
                HexString = "00" + HexString;
            }
            else if (HexString.Length == 3)
            {
                HexString = "0" + HexString;
            }

            Char[] TimeCollection = HexString.ToArray();
            string FinalCombination = TimeCollection[2].ToString() + TimeCollection[3].ToString() + TimeCollection[0].ToString() + TimeCollection[1].ToString();
            byte[] TimeInBytes = CommonOperation.StringToByteArray(HexString);

            List<Byte> ByteList = new List<byte>();
            ByteList.Add(0x40);
            ByteList.Add(0x40);
            ByteList.Add(0x28);
            ByteList.Add(0x00);
            ByteList.Add(0x03);

            for (int counter = 5; counter <= 24; counter++)
            {
                ByteList.Add(ReceivedDtaPkt[counter]);
            }

            //Command Package of parameter setting
            ByteList.Add(0x20);
            ByteList.Add(0x01);

            ByteList.Add(0x01);
            ByteList.Add(0x00);
            //Number of Parameters
            ByteList.Add(0x01);

            //Instruction Command           
            ByteList.Add(0x02);
            ByteList.Add(0x11);
            ByteList.Add(0x02);
            ByteList.Add(0x00);
            //Time Intervel 2 bytes
            for (int counter = TimeInBytes.Length - 1; counter >= 0; counter--)
            {
                ByteList.Add(TimeInBytes[counter]);
            }

           // ByteList.Add(0x05);
           // ByteList.Add(0x00);

            


            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte item in CRCCollection)
            {
                ByteList.Add(item);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);
            return ByteList.ToArray();
        }

        public List<List<Byte>> SinocastelCreateGpsTimeIntervalCommand(List<String> IMEIColl, string TimeInSeconds)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();
            int TimeToSt = Convert.ToInt32(TimeInSeconds);
            string HexString = TimeToSt.ToString("X");

            if (HexString.Length == 1)
            {
                HexString = "000" + HexString;
            }
            else if (HexString.Length == 2)
            {
                HexString = "00" + HexString;
            }
            else if (HexString.Length == 3)
            {
                HexString = "0" + HexString;
            }

            Char[] TimeCollection = HexString.ToArray();
            string FinalCombination = TimeCollection[2].ToString() + TimeCollection[3].ToString() + TimeCollection[0].ToString() + TimeCollection[1].ToString();
            byte[] TimeInBytes = CommonOperation.StringToByteArray(HexString);

            foreach (string item in IMEIColl)
            {
                List<Byte> ByteList = new List<byte>();
                ByteList.Add(0x40);
                ByteList.Add(0x40);
                ByteList.Add(0x28);
                ByteList.Add(0x00);
                ByteList.Add(0x03);

                byte[] IMEIArr = CommonOperation.StringToByteArray(IMEIColl[0]);

                foreach (byte bte in IMEIArr)
                {
                    ByteList.Add(bte);
                }

                for (int counter = (5 + IMEIArr.Length); counter <= 24; counter++)
                {
                    ByteList.Add(0x00);
                }

                //Command Package of parameter setting
                ByteList.Add(0x20);
                ByteList.Add(0x01);

                ByteList.Add(0x01);
                ByteList.Add(0x00);
                //Number of Parameters
                ByteList.Add(0x01);

                //Instruction Command           
                ByteList.Add(0x02);
                ByteList.Add(0x11);
                ByteList.Add(0x02);
                ByteList.Add(0x00);
                //Time Intervel 2 bytes
                for (int counter = TimeInBytes.Length - 1; counter >= 0; counter--)
                {
                    ByteList.Add(TimeInBytes[counter]);
                }

                // ByteList.Add(0x05);
                // ByteList.Add(0x00);

                //Dynamic CheckSum
                ushort BytArrlth = (ushort)ByteList.Count;
                int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
                List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
                Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

                foreach (byte items in CRCCollection)
                {
                    ByteList.Add(items);
                }

                //'\r\n'
                ByteList.Add(0x0D);
                ByteList.Add(0x0A);                
                CommandsList.Add(ByteList);
            }
            return CommandsList;
        }

        public Byte[] SinocastelHeartBeatReponse(byte[] ReceivedDtaPkt)
        {
            List<Byte> ByteList = new List<byte>();
            ByteList.Add(0x40);
            ByteList.Add(0x40);
            ByteList.Add(0x1F);
            ByteList.Add(0x00);
            ByteList.Add(0x03);

            for (int counter = 5; counter <= 24; counter++)
            {
                ByteList.Add(ReceivedDtaPkt[counter]);
            }


            //Command Package of parameter setting
            ByteList.Add(0x90);
            ByteList.Add(0x03);

            
            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte item in CRCCollection)
            {
                ByteList.Add(item);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);
            return ByteList.ToArray();
        }

        public List<List<Byte>> SinocatelTrackOnDemand(List<String> IMEIColl)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();

            foreach (string item in IMEIColl)
            {
                List<Byte> ByteList = new List<byte>();
                ByteList.Add(0x40);
                ByteList.Add(0x40);
                ByteList.Add(0x21);
                ByteList.Add(0x00);
                ByteList.Add(0x03);

                byte[] IMEIArr = CommonOperation.StringToByteArray(IMEIColl[0]);

                foreach (byte bte in IMEIArr)
                {
                    ByteList.Add(bte);
                }

                for (int counter = (5 + IMEIArr.Length); counter <= 24; counter++)
                {
                    ByteList.Add(0x00);
                }
                //Command Package of parameter setting
                ByteList.Add(0x30);
                ByteList.Add(0x01);

                ByteList.Add(0x01);
                ByteList.Add(0x00);

                //Dynamic CheckSum
                ushort BytArrlth = (ushort)ByteList.Count;
                int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
                List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
                Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

                foreach (byte items in CRCCollection)
                {
                    ByteList.Add(items);
                }

                //'\r\n'
                ByteList.Add(0x0D);
                ByteList.Add(0x0A);
                CommandsList.Add(ByteList);
            }
            return CommandsList;

        }

        private void AlarmParser(string[] HexCollection, List<TrackerAlarms> LstAlrms,ref ClientInfo tpclnt,ref  DataPacket DtaPkt)
        {
            int NoOfAlarms= Convert.ToInt16(CommonOperation.HexaDecimalToDecimalConversion(HexCollection[85]));
            int TotalAlarmBytes = NoOfAlarms * 6;
            int LoopEnd = TotalAlarmBytes + 85; //85 the location of No Of Alarm bytes

            int counter = 1;
            string HexaPkt = "";
            bool AlaramState = false;
            TrackerAlarms objTrkrAlrms = new TrackerAlarms();

            for (int BytCntr = 86; BytCntr <= LoopEnd; BytCntr++)
            {
                if (counter > 6)
                {
                    
                }

                switch (counter)
                {
                    case 1:

                        objTrkrAlrms.Status = Convert.ToBoolean (Convert.ToInt16(CommonOperation.HexaDecimalToDecimalConversion(HexCollection[BytCntr])));
                        break;

                    case 2:                        
                            AlarmMapping(HexCollection[BytCntr], objTrkrAlrms,ref tpclnt, ref DtaPkt);                                            
                        break;

                    case 3:
                        HexaPkt = "";
                        HexaPkt = HexCollection[BytCntr];
                        break;

                    case 4:
                        HexaPkt = HexCollection[BytCntr]+ HexaPkt  ;
                        long CurrentVal = CommonOperation.HexaDecimalToDecimalConversion(HexaPkt);
                        objTrkrAlrms.CurrentVal = Convert.ToDouble(CurrentVal);

                        if (AlarmsWthFloatVal.Contains(objTrkrAlrms.AlrmCodes.ToString()))
                        {
                            objTrkrAlrms.CurrentVal = (Convert.ToDouble(CurrentVal)/10);
                        }

                        
                        break;

                    case 5:
                        HexaPkt = "";
                        HexaPkt = HexCollection[BytCntr];
                        break;

                    case 6:
                        HexaPkt =  HexCollection[BytCntr]+ HexaPkt ;
                        CurrentVal = CommonOperation.HexaDecimalToDecimalConversion(HexaPkt);
                        objTrkrAlrms.ThresholdVal = Convert.ToDouble(CurrentVal);

                        if (AlarmsWthFloatVal.Contains(objTrkrAlrms.AlrmCodes.ToString()))

                        {
                            objTrkrAlrms.ThresholdVal = (Convert.ToDouble(CurrentVal) / 10);
                        }

                        LstAlrms.Add(objTrkrAlrms);
                        
                        //Reset Values
                        HexaPkt = "";
                        counter = 0;
                        AlaramState = false;
                        objTrkrAlrms = new TrackerAlarms();
                        break;                   
                }

                counter = counter + 1;
            }           
        }

        public void AlarmMapping(string HexByte,TrackerAlarms Alrms,ref ClientInfo tpclient, ref DataPacket DtaPkt)
        {      
            if (HexByte.Equals("01"))
            {
                Alrms.AlrmCodes = AlarmCodes.Spd;
                Alrms.Description = "Speed (KM/H)";
            }
            else if (HexByte.Equals("02"))
            {
                Alrms.AlrmCodes = AlarmCodes.LwVltg;
                Alrms.Description = "Low Voltage (V)";
            }
            else if (HexByte.Equals("03"))
            {
                Alrms.AlrmCodes = AlarmCodes.EngnTmp;
                Alrms.Description = "Engine Temperature (Celsius)";
            }

            else if (HexByte.Equals("06"))
            {
                Alrms.AlrmCodes = AlarmCodes.IdlEngn;
                Alrms.Description = "Idle Engine (Minutes)";
            }
            else if (HexByte.Equals("07"))
            {
                Alrms.AlrmCodes = AlarmCodes.Twng;
                Alrms.Description = "Towing";
            }
            else if (HexByte.Equals("08"))
            {
                Alrms.AlrmCodes = AlarmCodes.HghRPM;
                Alrms.Description = "High RPM (RPM)";
            }
            else if (HexByte.Equals("0A"))
            {
                Alrms.AlrmCodes = AlarmCodes.ExhstEmson;
                Alrms.Description = "Exhaust Emission";
            }
            else if (HexByte.Equals("0D"))
            {
                Alrms.AlrmCodes = AlarmCodes.FatgDrv;
                Alrms.Description = "Fatigue Driving (Minutes)";
            }
            else if (HexByte.Equals("18"))
            {
                Alrms.AlrmCodes = AlarmCodes.MIL;
                Alrms.Description = "MIL";

            }
            else if (HexByte.Equals("04"))
            {
                Alrms.AlrmCodes = AlarmCodes.HrdAcc;
                Alrms.Description = "Hard Acceleration (g)";
            }
            else if (HexByte.Equals("05"))
            {
                Alrms.AlrmCodes = AlarmCodes.HrdDecel;
                Alrms.Description = "Hard Decelaration (g)";
            }
            else if (HexByte.Equals("09"))
            {
                Alrms.AlrmCodes = AlarmCodes.PwrOn  ;
                Alrms.Description = "Power On";
            }
            else if (HexByte.Equals("0B"))
            {
                Alrms.AlrmCodes = AlarmCodes.QkLnChg ;
                Alrms.Description = "Quick Lane Change";
            }
            else if (HexByte.Equals("0C"))
            {
                Alrms.AlrmCodes = AlarmCodes.ShrpTrn;
                Alrms.Description = "Sharp Turn (g)";
            }
            else if (HexByte.Equals("0E"))
            {
                Alrms.AlrmCodes = AlarmCodes.PwrOf;
                Alrms.Description = "Power Off";
            }
            else if (HexByte.Equals("0F"))
            {
                Alrms.AlrmCodes = AlarmCodes.GeoFnceAlrm;
                Alrms.Description = "Geo-Fence Alarm";
            }
            else if (HexByte.Equals("10"))
            {
                Alrms.AlrmCodes = AlarmCodes.Emrgncy;
                Alrms.Description = "Emergency";
            }
            else if (HexByte.Equals("11"))
            {
                Alrms.AlrmCodes = AlarmCodes.Crsh   ;
                Alrms.Description = "Crash";
            }          
            else if (HexByte.Equals("12"))
            {
                Alrms.AlrmCodes = AlarmCodes.Tmpr;
                Alrms.Description = "Temper";
            }
            else if (HexByte.Equals("13"))
            {
                Alrms.AlrmCodes = AlarmCodes.IllglEntr;
                Alrms.Description = "Illegal Enter";
            }
            else if (HexByte.Equals("14"))
            {
                Alrms.AlrmCodes = AlarmCodes.IllglIgnton;
                Alrms.Description = "Illegal Ignition";
            }
            else if (HexByte.Equals("15"))
            {
                Alrms.AlrmCodes = AlarmCodes.OBDErr;
                Alrms.Description = "OBD Communication Error";
            }
            else if (HexByte.Equals("16"))
            {
                Alrms.AlrmCodes = AlarmCodes.IgntOn;
                Alrms.Description = "Ignition On";
                tpclient.IsEngineOn = true;
                TripHandling(ref DtaPkt, ref tpclient, true);
            }
            else if (HexByte.Equals("17"))
            {
                Alrms.AlrmCodes = AlarmCodes.IgntOff;
                Alrms.Description = "Ignition Off";
                tpclient.IsEngineOn = false;
                DtaPkt.ObjInpOutStatus.InEngineOnOff = false;
            }
            else if (HexByte.Equals("19"))
            {
                Alrms.AlrmCodes = AlarmCodes.UnLk;
                Alrms.Description = "Unlock";
            }
            else if (HexByte.Equals("1A"))
            {
                Alrms.AlrmCodes = AlarmCodes.NoCrd;
                Alrms.Description = "No Card";
            }
            else if (HexByte.Equals("1B"))
            {
                Alrms.AlrmCodes = AlarmCodes.DngrousDrvng;
                Alrms.Description = "Dangerous Driving";
            }
            else if (HexByte.Equals("1C"))
            {
                Alrms.AlrmCodes = AlarmCodes.Vib;
                Alrms.Description = "Vibration";
            }            
        }

        public Byte[] AlarmResponse(byte[] ReceivedDtaPkt)
        {
            List<Byte> ByteList = new List<byte>();
            int counter = 0;

            ByteList.Add(0x40);
            ByteList.Add(0x40);
            ByteList.Add(0x23);
            ByteList.Add(0x00);
            ByteList.Add(ReceivedDtaPkt[4]);

            for ( counter = 5; counter <= 24; counter++)
            {
                ByteList.Add(ReceivedDtaPkt[counter]);
            }

            ByteList.Add(0xC0);
            ByteList.Add(0x07);

            for ( counter = 27; counter<= 30; counter ++)
            {
                ByteList.Add(ReceivedDtaPkt[counter]);
            }

            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte item in CRCCollection)
            {
                ByteList.Add(item);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);
            return ByteList.ToArray();
        }

        private void SettingNewTripCurrentValues(DataPacket DtaPkt, ClientInfo tpclient,int tripNo)
        {
            TrackerTrip objTrackerTrp = new TrackerTrip();
            objTrackerTrp.TripStartTime = DtaPkt.UTCTimeAndDate;
            tpclient.objTrackerTrip = objTrackerTrp;
            tpclient.objTrackerTrip.PktNo = 1;
            tpclient.objTrackerTrip.TotalSpeed = Convert.ToSingle(DtaPkt.Speed);
            tpclient.objTrackerTrip.CurrentSpeed = Convert.ToSingle(DtaPkt.Speed);
            tpclient.objTrackerTrip.AvgSpeed = Convert.ToSingle(DtaPkt.Speed);
            tpclient.objTrackerTrip.CurrentRotation = Convert.ToSingle(DtaPkt.Direction);
            tpclient.objTrackerTrip.MaxRotation = Convert.ToSingle(DtaPkt.Direction);
            tpclient.objTrackerTrip.MaxSpeed = Convert.ToSingle(DtaPkt.Speed);
            tpclient.objTrackerTrip.NoOfAcc = 0;
            tpclient.objTrackerTrip.NoOfDec = 0;
            tpclient.objTrackerTrip.TripNo = tripNo;
            tpclient.IsNewTrip = false;
            tpclient.objTrackerTrip.TripStartTime = DtaPkt.UTCTimeAndDate;
            DtaPkt.objTrackerTrip = tpclient.objTrackerTrip;
        }

        private void SetTripValuesInClient(DataPacket DtaPkt, ClientInfo tpclient, bool IsloginCommand)
        {
            lock (LockThread)
            {
                if (IsloginCommand)
                {
                    TrackerTrip objTrackerTrp = new TrackerTrip();
                    objTrackerTrp.TripStartTime = DtaPkt.UTCTimeAndDate;
                    tpclient.objTrackerTrip = objTrackerTrp;
                    tpclient.objTrackerTrip.PktNo = 1;
                    tpclient.objTrackerTrip.TotalSpeed= Convert.ToSingle(DtaPkt.Speed);
                    tpclient.objTrackerTrip.CurrentSpeed = Convert.ToSingle(DtaPkt.Speed);
                    tpclient.objTrackerTrip.AvgSpeed = Convert.ToSingle(DtaPkt.Speed);
                    tpclient.objTrackerTrip.CurrentRotation = Convert.ToSingle(DtaPkt.Direction);
                    tpclient.objTrackerTrip.MaxRotation = Convert.ToSingle(DtaPkt.Direction);
                    tpclient.objTrackerTrip.MaxSpeed = Convert.ToSingle(DtaPkt.Speed);
                    tpclient.objTrackerTrip.NoOfAcc = 0;
                    tpclient.objTrackerTrip.NoOfDec = 0;                    
                    DtaPkt.objTrackerTrip = tpclient.objTrackerTrip;
                }
                else
                {
                    TimeSpan DrvngTime = DtaPkt.UTCTimeAndDate.Subtract(tpclient.objTrackerTrip.TripStartTime);

                    tpclient.objTrackerTrip.DrivingTime = DrvngTime.TotalMinutes;
                    tpclient.objTrackerTrip.IgnitionNo = 1;
                    tpclient.objTrackerTrip.PktNo = tpclient.objTrackerTrip.PktNo + 1;
                    tpclient.objTrackerTrip.TotalSpeed = tpclient.objTrackerTrip.TotalSpeed + DtaPkt.Speed;
                    tpclient.objTrackerTrip.CurrentRotation = Convert.ToSingle(DtaPkt.Direction);
                    tpclient.objTrackerTrip.CurrentSpeed = Convert.ToSingle(DtaPkt.Speed);

                    
                    if (tpclient.objTrackerTrip.CurrentSpeed > tpclient.objTrackerTrip.MaxSpeed)
                    {
                        tpclient.objTrackerTrip.MaxSpeed = tpclient.objTrackerTrip.CurrentSpeed;
                    }

                    if (tpclient.objTrackerTrip.CurrentRotation > tpclient.objTrackerTrip.MaxRotation)
                    {
                        tpclient.objTrackerTrip.MaxRotation = tpclient.objTrackerTrip.CurrentRotation ;
                    }

                    if  (DtaPkt.LstAlarms!= null && DtaPkt.LstAlarms.Count>0)
                    {
                        if (DtaPkt.LstAlarms.Where(x => x.AlrmCodes == AlarmCodes.HrdAcc && x.Status ==true).ToList().Count > 0)
                        {
                            tpclient.objTrackerTrip.NoOfAcc = tpclient.objTrackerTrip.NoOfAcc + 1;
                        }

                        if (DtaPkt.LstAlarms.Where(x => x.AlrmCodes == AlarmCodes.HrdDecel && x.Status == true).ToList().Count > 0)
                        {
                            tpclient.objTrackerTrip.NoOfDec = tpclient.objTrackerTrip.NoOfDec + 1;
                        }

                        if ((DtaPkt.LstAlarms.Where(x => x.AlrmCodes == AlarmCodes.IdlEngn && x.Status == true).ToList().Count > 0))
                        {                            
                            var val = DtaPkt.LstAlarms.Where(x => x.AlrmCodes == AlarmCodes.IdlEngn && x.Status == true).ToList()[0];
                            tpclient.objTrackerTrip.IdleTime =val.CurrentVal  ;
                        }

                        //if ((DtaPkt.LstAlarms.Where(x => x.AlrmCodes == AlarmCodes.IdlEngn && x.Status == false).ToList().Count > 0))
                        //{
                        //    tpclient.objTrackerTrip.IdleTimeEnd  = DtaPkt.UTCTimeAndDate;
                        //    TimeSpan IdlTime = tpclient.objTrackerTrip.IdleTimeEnd.Subtract(tpclient.objTrackerTrip.IdleTimeStart);
                        //    tpclient.objTrackerTrip.IdleTime = IdlTime.TotalMinutes;
                        //}
                    }

                    tpclient.objTrackerTrip.AvgSpeed = Convert.ToSingle(tpclient.objTrackerTrip.TotalSpeed / tpclient.objTrackerTrip.PktNo);
                    DtaPkt.objTrackerTrip = tpclient.objTrackerTrip;
                }
            }

        }

        private byte[] SetSpeedAlarmHardCode(byte[] ReceivedDtaPkt)
        {
            List<Byte> ByteList = new List<byte>();
            ByteList.Add(0x40);
            ByteList.Add(0x40);
            ByteList.Add(0x2A);
            ByteList.Add(0x00);
            ByteList.Add(0x03);

            for (int counter = 5; counter <= 24; counter++)
            {
                ByteList.Add(ReceivedDtaPkt[counter]);
            }

            //Command Package of parameter setting
            ByteList.Add(0x20);
            ByteList.Add(0x01);

            ByteList.Add(0x01);
            ByteList.Add(0x00);
            //Number of Parameters
            ByteList.Add(0x01);

            //Instruction Command           
            ByteList.Add(0x01);
            ByteList.Add(0x10);
            //Length
            ByteList.Add(0x04);
            ByteList.Add(0x00);

            ByteList.Add(0x01);
            ByteList.Add(0x00);
            ByteList.Add(0x64);
            ByteList.Add(0x00);



            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte item in CRCCollection)
            {
                ByteList.Add(item);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);
            return ByteList.ToArray();

        }


        private string UpdateIsAppliedFlag(string TrackerID, string AlarmCode)
        {
            string Filter = "UpdateConfig_ByTID";

            switch (AlarmCode)
            {
                case "1001":
                    ObjVehicles.UpdateAlarmsSettings_ByTID(Filter, TrackerID, AlarmCodes.Spd.ToString());
                    return AlarmCodes.Spd.ToString();                    

                case "1004":
                    ObjVehicles.UpdateAlarmsSettings_ByTID(Filter, TrackerID, AlarmCodes.HrdAcc.ToString());
                    return AlarmCodes.HrdAcc.ToString();                   

                case "1003":
                    ObjVehicles.UpdateAlarmsSettings_ByTID(Filter, TrackerID, AlarmCodes.EngnTmp.ToString());
                    return AlarmCodes.EngnTmp.ToString();                    

                case "1008":
                    ObjVehicles.UpdateAlarmsSettings_ByTID(Filter, TrackerID, AlarmCodes.HghRPM.ToString());
                    return AlarmCodes.HghRPM.ToString();

                case "1006":
                    ObjVehicles.UpdateAlarmsSettings_ByTID(Filter, TrackerID, AlarmCodes.IdlEngn.ToString());
                    return AlarmCodes.IdlEngn.ToString();

                case "1002":
                    ObjVehicles.UpdateAlarmsSettings_ByTID(Filter, TrackerID, AlarmCodes.LwVltg.ToString());
                    return AlarmCodes.LwVltg.ToString();

                case "100D":
                    ObjVehicles.UpdateAlarmsSettings_ByTID(Filter, TrackerID, AlarmCodes.FatgDrv.ToString());
                    return AlarmCodes.FatgDrv.ToString();

                case "100B":
                    ObjVehicles.UpdateAlarmsSettings_ByTID(Filter, TrackerID, AlarmCodes.QkLnChg.ToString());
                    return AlarmCodes.QkLnChg.ToString();

                case "100C":
                    ObjVehicles.UpdateAlarmsSettings_ByTID(Filter, TrackerID, AlarmCodes.ShrpTrn.ToString());
                    return AlarmCodes.ShrpTrn.ToString();

                case "1011":
                    ObjVehicles.UpdateAlarmsSettings_ByTID(Filter, TrackerID, AlarmCodes.Crsh.ToString());
                    return AlarmCodes.Crsh.ToString();

                case "3401":
                    Filter = "UpdateWifiConfig_ByTID";
                    ObjVehicles.UpdateWifiSettings_ByTID(Filter, TrackerID, AlarmCodes.WifiEnable.ToString());
                    return AlarmCodes.WifiEnable.ToString();

                case "3402":
                    Filter = "UpdateWifiConfig_ByTID";
                    ObjVehicles.UpdateWifiSettings_ByTID(Filter, TrackerID, AlarmCodes.WifiSSID.ToString());
                    return AlarmCodes.WifiSSID.ToString();

                case "3404":
                    Filter = "UpdateWifiConfig_ByTID";
                    ObjVehicles.UpdateWifiSettings_ByTID(Filter, TrackerID, AlarmCodes.WifiPwd.ToString());
                    return AlarmCodes.WifiPwd.ToString();

                default:
                    return "";
            }
        }



        public List<List<Byte>> SetSpeedingAlarm(List<String> IMEIColl,bool AlarmStatus,string value)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();         
            byte[] ValInBytes = CreateParameterHexString(Convert.ToSingle(value));
            List<Byte> ByteList= CreateCommandForSettingParameters(IMEIColl)[0];

            //for (int counter = ValInBytes.Length - 1; counter >= 0; counter--)
            //{
            //    ByteList.Add(ValInBytes[counter]);
            //}

            //Instruction Command           
            ByteList.Add(0x01);
            ByteList.Add(0x10);
            //Length
            ByteList.Add(0x03);
            ByteList.Add(0x00);
            //Param

            //Alarm

            if (AlarmStatus)
            {
                ByteList.Add(0x01);
            }
            else
            {
                ByteList.Add(0x00);
            }
                
            //Sound
            //ByteList.Add(0x00);

            //Provided Speed Value
            
            ////ByteList.Add(0x64);
            ////ByteList.Add(0x00);

            if ((Convert.ToSingle(value)) > 0)
            {
                for (int counter = ValInBytes.Length - 1; counter >= 0; counter--)
                {
                    ByteList.Add(ValInBytes[counter]);
                }
            }
            else
            {
                ByteList.Add(0x00);
                ByteList.Add(0x00);
            }

            int MaxCount = ByteList.Count+4;
            ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);
            
            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte items in CRCCollection)
            {
                ByteList.Add(items);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);

            string TrackerHexaRespon = BitConverter.ToString(ByteList.ToArray(), 0, ByteList.Count - 1).Replace("-", " ");
            string    data = "";
            data = Environment.NewLine + DateTime.Now.ToString() + ":Speed Alarm Setting";
            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaRespon;
                       
            data = data + Environment.NewLine;
            ObjComnOp.WriteFile(data);
            
            CommandsList.Add(ByteList);
            return CommandsList;

        }

        private byte[] CreateParameterHexString(float CrrntVal)
        {
            int CurrentVal = Convert.ToInt32(CrrntVal);
            string HexString = CurrentVal.ToString("X");

            if (HexString.Length == 1)
            {
                HexString = "000" + HexString;
            }
            else if (HexString.Length == 2)
            {
                HexString = "00" + HexString;
            }
            else if (HexString.Length == 3)
            {
                HexString = "0" + HexString;
            }

            Char[] ValCollection = HexString.ToArray();
            string FinalCombination = ValCollection[2].ToString() + ValCollection[3].ToString() + ValCollection[0].ToString() + ValCollection[1].ToString();
            return CommonOperation.StringToByteArray(HexString);
        }
        public List<List<Byte>> CreateCommandForSettingParameters(List<String> IMEIColl)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();                        

            foreach (string item in IMEIColl)
            {
                List<Byte> ByteList = new List<byte>();
                ByteList.Add(0x40);
                ByteList.Add(0x40);
                ByteList.Add(0x28);
                ByteList.Add(0x00);
                ByteList.Add(0x03);

                byte[] IMEIArr = CommonOperation.StringToByteArray(IMEIColl[0]);

                foreach (byte bte in IMEIArr)
                {
                    ByteList.Add(bte);
                }

                for (int counter = (5 + IMEIArr.Length); counter <= 24; counter++)
                {
                    ByteList.Add(0x00);
                }

                //Command Package of parameter setting
                ByteList.Add(0x20);
                ByteList.Add(0x01);

                ByteList.Add(0x01);
                ByteList.Add(0x00);
                //Number of Parameters
                ByteList.Add(0x01);

                
                CommandsList.Add(ByteList);
            }
            return CommandsList;
        }
        public bool isDecimal(string value)
        {        
            try
            {
                Decimal.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public  List<List<Byte>> HardAcceleration(List<String> IMEIColl, bool AlarmStatus, string value)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();
            float Threshval = Convert.ToSingle(value);
            Threshval = Threshval * 10;
            
            byte[] ValInBytes = CreateParameterHexString(Threshval);
            List<Byte> ByteList = CreateCommandForSettingParameters(IMEIColl)[0];
          
            //Instruction Command           
            ByteList.Add(0x04);
            ByteList.Add(0x10);
            //Length
            ByteList.Add(0x03);
            ByteList.Add(0x00);
            //Param

            //Alarm
            if (AlarmStatus)
            {
                ByteList.Add(0x01);
            }
            else
            {
                ByteList.Add(0x00);
            }
            
            //Sound
            //ByteList.Add(0x00);

            if ((Convert.ToSingle(value)) > 0)
            {
                for (int counter = ValInBytes.Length - 1; counter >= 0; counter--)
                {
                    ByteList.Add(ValInBytes[counter]);
                }
            }
            else
            {
                ByteList.Add(0x00);
                ByteList.Add(0x00);
            }

            int MaxCount = ByteList.Count + 4;
            ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);

            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte items in CRCCollection)
            {
                ByteList.Add(items);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);

            string TrackerHexaRespon = BitConverter.ToString(ByteList.ToArray(), 0, ByteList.Count - 1).Replace("-", " ");
            string data = "";
            data = Environment.NewLine + DateTime.Now.ToString() + ":HardAcceleration Alarm Setting";
            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaRespon;

            data = data + Environment.NewLine;
            ObjComnOp.WriteFile(data);

            CommandsList.Add(ByteList);
            return CommandsList;
        }

        public List<List<Byte>> EngineCoolantTemperature(List<String> IMEIColl, bool AlarmStatus, string value)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();
            byte[] ValInBytes = CreateParameterHexString(Convert.ToSingle(value));
            List<Byte> ByteList = CreateCommandForSettingParameters(IMEIColl)[0];

            //Instruction Command           
            ByteList.Add(0x03);
            ByteList.Add(0x10);
            //Length
            ByteList.Add(0x03);
            ByteList.Add(0x00);
            //Param

            //Alarm

            if (AlarmStatus)
            {
                ByteList.Add(0x01);
            }
            else
            {
                ByteList.Add(0x00);
            }

            //Sound
            //ByteList.Add(0x00);

            //Param
            if ((Convert.ToSingle(value)) > 0)
            {
                for (int counter = ValInBytes.Length - 1; counter >= 0; counter--)
                {
                    ByteList.Add(ValInBytes[counter]);
                }
            }
            else
            {
                ByteList.Add(0x00);
                ByteList.Add(0x00);
            }

            int MaxCount = ByteList.Count + 4;
            ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);

            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte items in CRCCollection)
            {
                ByteList.Add(items);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);

            string TrackerHexaRespon = BitConverter.ToString(ByteList.ToArray(), 0, ByteList.Count - 1).Replace("-", " ");
            string data = "";
            data = Environment.NewLine + DateTime.Now.ToString() + ":EngineCoolantTemperature Alarm Setting";
            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaRespon;

            data = data + Environment.NewLine;
            ObjComnOp.WriteFile(data);
            //int MaxCount = ByteList.Count;

            //ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);
            CommandsList.Add(ByteList);
            return CommandsList;

        }

        public List<List<Byte>> HighEngineRPM(List<String> IMEIColl, bool AlarmStatus, string value)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();
            byte[] ValInBytes = CreateParameterHexString(Convert.ToSingle(value));
            List<Byte> ByteList = CreateCommandForSettingParameters(IMEIColl)[0];

            //Instruction Command           
            ByteList.Add(0x08);
            ByteList.Add(0x10);
            //Length
            ByteList.Add(0x03);
            ByteList.Add(0x00);
            //Param

            //Alarm

            if (AlarmStatus)
            {
                ByteList.Add(0x01);
            }
            else
            {
                ByteList.Add(0x00);
            }

            //Sound
           // ByteList.Add(0x00);

            //Param
            if ((Convert.ToSingle(value)) > 0)
            {
                for (int counter = ValInBytes.Length - 1; counter >= 0; counter--)
                {
                    ByteList.Add(ValInBytes[counter]);
                }
            }
            else
            {
                ByteList.Add(0x00);
                ByteList.Add(0x00);
            }

            int MaxCount = ByteList.Count + 4;
            ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);

            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte items in CRCCollection)
            {
                ByteList.Add(items);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);

            string TrackerHexaRespon = BitConverter.ToString(ByteList.ToArray(), 0, ByteList.Count - 1).Replace("-", " ");
            string data = "";
            data = Environment.NewLine + DateTime.Now.ToString() + ":HighEngineRPM Alarm Setting";
            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaRespon;

            data = data + Environment.NewLine;
            ObjComnOp.WriteFile(data);
            //int MaxCount = ByteList.Count;

            //ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);
            CommandsList.Add(ByteList);
            return CommandsList;

        }

        public List<List<Byte>> GeneralSettingsWithoutDecimalValues(List<String> IMEIColl,string AlarmCode ,bool AlarmStatus, string value)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();
            string AlarmType = "";
            
            List<Byte> ByteList = CreateCommandForSettingParameters(IMEIColl)[0];

            //Instruction Command  
            byte[] ValInBytes = CreateParameterHexString(Convert.ToSingle(value));
            if (AlarmCode.Equals("IdlEngn"))
            {
                ByteList.Add(0x06);
                AlarmType = "IdlEngn";
            }
            else if (AlarmCode.Equals("LwVltg"))
            {
                ByteList.Add(0x02);
                AlarmType = "LwVltg";
            }
            else if ((AlarmCode.Equals("FatgDrv")))
            {
                ByteList.Add(0x0D);
                AlarmType = "FatgDrv";
            }
            else if ((AlarmCode.Equals("QkLnChg")))
            {
                ByteList.Add(0x0B);
                ValInBytes = CreateParameterHexString((Convert.ToSingle(value)*10));
                AlarmType = "QkLnChg";
            }

            else if ((AlarmCode.Equals("ShrpTrn")))
            {
                ByteList.Add(0x0C);
                ValInBytes = CreateParameterHexString((Convert.ToSingle(value) * 10));
                AlarmType = "ShrpTrn";
            }
           
            else if ((AlarmCode.Equals("Crsh")))
            {
                ByteList.Add(0x11);
                ValInBytes = CreateParameterHexString((Convert.ToSingle(value) * 10));
                AlarmType = "Crsh";
            }

                ByteList.Add(0x10);
            //Length
            ByteList.Add(0x03);
            ByteList.Add(0x00);
            //Param

            //Alarm

            if (AlarmStatus)
            {
                ByteList.Add(0x01);
            }
            else
            {
                ByteList.Add(0x00);
            }

            //Sound
            //ByteList.Add(0x00);

            //Param
            if ((Convert.ToSingle(value)) > 0)
            {
                for (int counter = ValInBytes.Length - 1; counter >= 0; counter--)
                {
                    ByteList.Add(ValInBytes[counter]);
                }
            }
            else
            {
                ByteList.Add(0x00);
                ByteList.Add(0x00);
            }

            int MaxCount = ByteList.Count + 4;
            ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);

            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte items in CRCCollection)
            {
                ByteList.Add(items);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);



            string TrackerHexaRespon = BitConverter.ToString(ByteList.ToArray(), 0, ByteList.Count - 1).Replace("-", " ");
            string data = "";
            data = Environment.NewLine + DateTime.Now.ToString() + ":" + AlarmType + " Alarm Setting";
            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaRespon;

            data = data + Environment.NewLine;
            ObjComnOp.WriteFile(data);
            //int MaxCount = ByteList.Count;

            //ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);
            CommandsList.Add(ByteList);
            return CommandsList;

        }

        public List<List<Byte>> TrackerMILSettings(List<String> IMEIColl, bool AlarmStatus, string value)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();
            byte[] ValInBytes = CreateParameterHexString(Convert.ToSingle(value));
            List<Byte> ByteList = CreateCommandForSettingParameters(IMEIColl)[0];

            //Instruction Command           
            ByteList.Add(0x18);
            ByteList.Add(0x10);
            //Length
            ByteList.Add(0x02);
            ByteList.Add(0x00);
            //Param

            //Alarm

            if (AlarmStatus)
            {
                ByteList.Add(0x01);
            }
            else
            {
                ByteList.Add(0x00);
            }

            //Sound
            //ByteList.Add(0x00);
            //Value Reserved for MIL
            // ByteList.Add(0x00);
            //ByteList.Add(0x00);

            int MaxCount = ByteList.Count + 4;
            ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);

            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte items in CRCCollection)
            {
                ByteList.Add(items);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);

            //int MaxCount = ByteList.Count;

            //ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);
            CommandsList.Add(ByteList);
            return CommandsList;
        }

        public List<List<Byte>> WIFIHandling(List<String> IMEIColl, bool AlarmStatus, string value)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();
            byte[] ValInBytes = CreateParameterHexString(Convert.ToSingle(value));
            List<Byte> ByteList = CreateCommandForSettingParameters(IMEIColl)[0];

            //Instruction Command           
            ByteList.Add(0x01);
            ByteList.Add(0x34);
            //Length
            ByteList.Add(0x01);
            ByteList.Add(0x00);
            //Param

            //Alarm

            if (AlarmStatus)
            {
                ByteList.Add(0x01);
            }
            else
            {
                ByteList.Add(0x00);
            }      

            int MaxCount = ByteList.Count + 4;
            ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);

            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte items in CRCCollection)
            {
                ByteList.Add(items);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);

            string TrackerHexaRespon = BitConverter.ToString(ByteList.ToArray(), 0, ByteList.Count - 1).Replace("-", " ");
            string data = "";
            data = Environment.NewLine + DateTime.Now.ToString() + ":WIFI Settings";
            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaRespon;

            data = data + Environment.NewLine;
            ObjComnOp.WriteFile(data);
            //int MaxCount = ByteList.Count;

            //ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);
            CommandsList.Add(ByteList);
            return CommandsList;

        }

        public List<List<Byte>> WIFISSID(List<String> IMEIColl, string SSIDValue)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();
            //byte[] ValInBytes = CreateParameterHexString(Convert.ToSingle(value));
            List<Byte> ByteList = CreateCommandForSettingParameters(IMEIColl)[0];


            //Creating an SSID ASCII byte array and get the length
            byte[] ba = Encoding.Default.GetBytes(SSIDValue);//Pwd from Ascii to byte
            string hexString = BitConverter.ToString(ba);
            string HexIMEI = hexString.Replace("-", "");
            byte[] baAscii = CommonOperation.StringToByteArray(HexIMEI);            

            int SSIDVal = (int)(ba.Length);//getting parameter length
            string HexSSID = CommonOperation.DecimalToHexaDecimal(SSIDVal);//converting to SSID Length value to string 
            byte[] SSIDLength = CommonOperation.StringToByteArray(HexSSID);//
            
            //Instruction Command           
            ByteList.Add(0x02);
            ByteList.Add(0x34);


            //Length

            foreach (byte SSIDByt in SSIDLength)
            {
                ByteList.Add(SSIDByt);
            }

            ByteList.Add(0x00);


            //Param          
            foreach (byte SSIDByt in baAscii)
            {
                ByteList.Add(SSIDByt);
            }

            int MaxCount = ByteList.Count + 4;
            ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);

            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte items in CRCCollection)
            {
                ByteList.Add(items);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);

            string TrackerHexaRespon = BitConverter.ToString(ByteList.ToArray(), 0, ByteList.Count - 1).Replace("-", " ");
            string data = "";
            data = Environment.NewLine + DateTime.Now.ToString() + ":WIFI SSID Setting";
            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaRespon;

            data = data + Environment.NewLine;
            ObjComnOp.WriteFile(data);
            //int MaxCount = ByteList.Count;

            //ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);
            CommandsList.Add(ByteList);
            return CommandsList;            
            
            //string hexString = BitConverter.ToString(ba);
            //string HexIMEI = hexString.Replace("-", "");
            //byte[] IMEIArr = CommonOperation.StringToByteArray(HexIMEI);            
        }

        public List<List<Byte>> WIFIPwd(List<String> IMEIColl, string WIFIPwd)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();
            //byte[] ValInBytes = CreateParameterHexString(Convert.ToSingle(value));
            List<Byte> ByteList = CreateCommandForSettingParameters(IMEIColl)[0];


            //Creating an SSID ASCII byte array and get the length
            byte[] ba = Encoding.Default.GetBytes(WIFIPwd);//Pwd from Ascii to byte
            string hexString = BitConverter.ToString(ba);
            string HexIMEI = hexString.Replace("-", "");
            byte[] baAscii = CommonOperation.StringToByteArray(HexIMEI);

            int SSIDVal = (int)(ba.Length);//getting parameter length
            string HexSSID = CommonOperation.DecimalToHexaDecimal(SSIDVal);//converting to SSID Length value to string 
            byte[] SSIDLength = CommonOperation.StringToByteArray(HexSSID);//

            //Instruction Command           
            ByteList.Add(0x04);
            ByteList.Add(0x34);


            //Length

            foreach (byte SSIDByt in SSIDLength)
            {
                ByteList.Add(SSIDByt);
            }

            ByteList.Add(0x00);


            //Param          
            foreach (byte SSIDByt in baAscii)
            {
                ByteList.Add(SSIDByt);
            }

            int MaxCount = ByteList.Count + 4;
            ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);

            //Dynamic CheckSum
            ushort BytArrlth = (ushort)ByteList.Count;
            int Crc = CRC16Util.CRC_MakeCrc(ByteList.ToArray(), BytArrlth);
            List<string> CRCList = CommonOperation.listGetCRCSinocastelForm(CommonOperation.DecimalToHexaDecimal(Crc));
            Byte[] CRCCollection = CommonOperation.StringToByteArray(CRCList[0].ToString());

            foreach (byte items in CRCCollection)
            {
                ByteList.Add(items);
            }

            //'\r\n'
            ByteList.Add(0x0D);
            ByteList.Add(0x0A);

            string TrackerHexaRespon = BitConverter.ToString(ByteList.ToArray(), 0, ByteList.Count - 1).Replace("-", " ");
            string data = "";
            data = Environment.NewLine + DateTime.Now.ToString() + ":WIFI Pwd Setting";
            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaRespon;

            data = data + Environment.NewLine;
            ObjComnOp.WriteFile(data);
            //int MaxCount = ByteList.Count;

            //ByteList[2] = (CommonOperation.StringToByteArray(CommonOperation.DecimalToHexaDecimal(MaxCount))[0]);
            CommandsList.Add(ByteList);
            return CommandsList;
         
        }
    }

    
}
