using AirViewTracker.DBLayer.Fleet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpService
{
    class CalampParser
    {
        Byte[] ClntData;
        CommonOperation ObjComnOp;
        string ExceptionLogPath;
        string LogFilePath;
        string SgnalRApi;
        int StdDevIDLth = 5;
        int ClntDevIDLth = 0; 
        bool WriteLogFileflg, WriteExceptionLgflg;
        List<UdpClientInfo> lstUDPClnt ;
        UdpClientInfo objUDPClnt;
        FM_VehicleBL ObjVehicleObj;
        List<string> AllIMEILst = new List<string>();


        public CalampParser(Byte[] ClientData, string ExceptionLogPth, string FilePth, bool WriteLgFile, bool WriteExcetionLg, string SignalRAPi,ref List<UdpClientInfo> UDPClntList, ref UdpClientInfo UDPClnt,List<string> LstALLIMEI,string ConnectionStr)
        {
            ClntData = ClientData;
            ExceptionLogPath = ExceptionLogPth;
            LogFilePath = FilePth;
            SgnalRApi = SignalRAPi;
            WriteLogFileflg = WriteLgFile;
            WriteExceptionLgflg = WriteExcetionLg;
            ObjComnOp = new TcpService.CommonOperation(LogFilePath, ExceptionLogPath, SgnalRApi);
            objUDPClnt = UDPClnt;
            lstUDPClnt = UDPClntList;
            ObjVehicleObj = new FM_VehicleBL(ConnectionStr);
            AllIMEILst = LstALLIMEI;
        }

        public CalampParser(UdpClientInfo objUDPClnt, string ConnectionStr)
        {
            this.objUDPClnt = objUDPClnt;
            ObjVehicleObj = new FM_VehicleBL(ConnectionStr);
        }

        public Byte[] ParseEventRpt()
        {
            try
            {
                string TrackerHexaResponse = "";
                string dataFromClient = "";
                string[] HexCollection;
                string UDPData = "";

                TrackerHexaResponse = BitConverter.ToString(ClntData, 0, ClntData.Length).Replace("-", " ");
                HexCollection = TrackerHexaResponse.Split(' ');
                ClntDevIDLth = GetDeviceIDLength(HexCollection);

                int CounterDiff = StdDevIDLth - ClntDevIDLth;
                int MessageIdentifierLocation = 10 - CounterDiff;
                string CurrentPktIMEI = GetDeviceID(HexCollection);

                if (!(lstUDPClnt.Exists(x => x.TrackerID == CurrentPktIMEI)))
                {
                    bool ClientIMEI = AllIMEILst.Contains(CurrentPktIMEI);
                    //bool ClientIMEI = ObjVehicleObj.ValidateVehicleIMEI("Validate_IMEI", objUDPClnt.TrackerID);

                    if (!ClientIMEI)
                    {
                        return null;
                    }
                }

                if (HexCollection[MessageIdentifierLocation] == "2")
                {                   
                    dataFromClient = System.Text.Encoding.ASCII.GetString(ClntData);
                    DataPacket DataPktobj = new DataPacket();

                    DataPktobj.UTCTimeAndDate = ParseUpdateTime(HexCollection);
                    DataPktobj.UTCTimeAndDate = ParseTimeOfFix(HexCollection);
                    DataPktobj.Latitude = Parselat(HexCollection);
                    DataPktobj.Longitude = ParseLong(HexCollection);
                    DataPktobj.Altitude = ParseAltitude(HexCollection);
                    DataPktobj.Speed = ParseSpeed(HexCollection);
                    DataPktobj.Direction = ParseHeadingDirection(HexCollection);
                    DataPktobj.Rotation = DegreesToCardinal(DataPktobj.Direction);
                    DataPktobj.TrackerID = GetDeviceID(HexCollection);
                    DataPktobj.GPSSignalStatus = "Valid";
                    DataPktobj.IsOfflineData = false;

                    objUDPClnt.TrackerID = DataPktobj.TrackerID;

                    if (lstUDPClnt.Exists(x => x.TrackerID == objUDPClnt.TrackerID))
                    {
                        UdpClientInfo UDPClnt = lstUDPClnt.FirstOrDefault(x => x.TrackerID == objUDPClnt.TrackerID);
                        if (UDPClnt != null)
                        {
                            UDPClnt.IPAddress = objUDPClnt.IPAddress;
                            UDPClnt.LstPktRcvdTime = DateTime.Now;
                        }
                    }
                    else
                    {                                                                        
                            lstUDPClnt.Add(objUDPClnt);                                                
                    }

                    string data = "";

                    if (WriteLogFileflg)
                    {
                        data = Environment.NewLine + DateTime.Now.ToString() + " UDP Gps Data Parsed ";
                        data = data + Environment.NewLine + "DeviceID : " + DataPktobj.TrackerID;
                        data = data + Environment.NewLine + "Date Time: " + ": " + DataPktobj.UTCTimeAndDate.ToString();
                        data = data + Environment.NewLine + "Latitude: " + ": " + DataPktobj.Latitude.ToString();
                        data = data + Environment.NewLine + "Longitutude: " + ": " + DataPktobj.Longitude.ToString();
                        data = data + Environment.NewLine + "Speed: " + ": " + DataPktobj.Speed.ToString();
                        data = data + Environment.NewLine + "Degree: " + ": " + DataPktobj.Direction.ToString();
                        data = data + Environment.NewLine + "Direction: " + ": " + DataPktobj.Rotation.ToString();
                        data = data + Environment.NewLine;
                        ObjComnOp.WriteFile(data);
                    }

                    InputOutputStatus objStatus = new InputOutputStatus();
                    objStatus.InEngineOnOff = true;
                    DataPktobj.ObjInpOutStatus = objStatus;

                    ObjComnOp.SendDataToApi(DataPktobj);

                    if (HexCollection[9] == "01")
                    {
                        return SentAck();
                    }
                    else
                    {
                        return null;
                    }
                }

                return null;   
            }

            catch (Exception ex)
            {
                if (WriteExceptionLgflg)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + " ParseEventRpt : " + ex.ToString();
                    ObjComnOp.WriteExceptionFile(exception);                    
                }
                return null;
            }
        }

        private int GetDeviceIDLength(string[] HexCollection)
        {
           return Convert.ToInt16(HexCollection[1]);
        }

        private string GetDeviceID(string[] HexCollection)
        {
            string Hexvalue = "";

            for (int counter =2; counter<2+ClntDevIDLth;counter++)
            {
                Hexvalue = Hexvalue + HexCollection[counter];
            }
            return Hexvalue;
        }
        private Byte[] SentAck()
        {
            List<byte> RspnStream = new List<byte>();
            

            for (int counter = 0; counter < 9; counter++)
            {
                RspnStream.Add(ClntData[counter]);
            }

            //MessageHeader
            RspnStream.Add(0x02);
            RspnStream.Add(0x01);

            //Message Sequence Number
            for (int counter = 11; counter < 13; counter++)
            {
                RspnStream.Add(ClntData[counter]);
            }
           

            //Acknowledge Message
            RspnStream.Add(ClntData[10]); //Message type
            RspnStream.Add(0x00);       //Acknowledge

            RspnStream.Add(0x00);       //Spare
            RspnStream.Add(0x00);       //App ver
            RspnStream.Add(0x00);       //App ver
            RspnStream.Add(0x00);       //App ver

            return RspnStream.ToArray();
        }

       private DateTime ParseUpdateTime(string[] HexCollection)
        {
            string Hexvalue = "";
            int CounterDiff = StdDevIDLth - ClntDevIDLth;
            int StartCounter = 13 - (CounterDiff);
            int EndCounter = 16 - (CounterDiff);

            for (int counter = StartCounter; counter<= EndCounter; counter++)
            {
                Hexvalue = Hexvalue + HexCollection[counter];
            }

            Int64 UnixDateTime = CommonOperation.HexaDecimalToDecimalConversion(Hexvalue);
            DateTime dt = ObjComnOp.UnixTimeStampToDateTime(UnixDateTime);
            return dt;

        }

        private DateTime ParseTimeOfFix(string[] HexCollection)
        {
            string Hexvalue = "";
            int CounterDiff = StdDevIDLth - ClntDevIDLth;
            int StartCounter = 17 - (CounterDiff);
            int EndCounter = 20 - (CounterDiff);

            for (int counter = StartCounter; counter <= EndCounter; counter++)
            {
                Hexvalue = Hexvalue + HexCollection[counter];
            }
            Int64 UnixDateTime = CommonOperation.HexaDecimalToDecimalConversion(Hexvalue);
            DateTime dt = ObjComnOp.UnixTimeStampToDateTime(UnixDateTime);
            return dt;
        }

        private double Parselat(string[] HexCollection)
        {
            string Hexvalue = "";
            double Latitude = 0;
            int CounterDiff = StdDevIDLth - ClntDevIDLth;
            int StartCounter = 21 - (CounterDiff);
            int EndCounter = 24 - (CounterDiff);

            for (int counter = StartCounter; counter <= EndCounter; counter++)
            {
                Hexvalue = Hexvalue + HexCollection[counter];
            }

            //Double HexTODeciVal = CommonOperation.HexaDecimalToDecimalConversion(Hexvalue);
            Latitude= ParseCalAmpLatLongValues(Hexvalue.ToString());
            return Latitude;
        }

        private double ParseLong(string[] HexCollection)
        {
            string Hexvalue = "";
            double Longitude = 0;
            int CounterDiff = StdDevIDLth - ClntDevIDLth;
            int StartCounter = 25 - (CounterDiff);
            int EndCounter = 28 - (CounterDiff);

            for (int counter = StartCounter; counter <= EndCounter; counter++)
            {
                Hexvalue = Hexvalue + HexCollection[counter];
            }

            //Double HexTODeciVal = CommonOperation.HexaDecimalToDecimalConversion(Hexvalue);
            Longitude = ParseCalAmpLatLongValues(Hexvalue.ToString());
            return Longitude;
        }

        private double ParseAltitude(string[] HexCollection)
        {
            string Hexvalue = "";
            double Altitude = 0;
            int CounterDiff = StdDevIDLth - ClntDevIDLth;
            int StartCounter = 29 - (CounterDiff);
            int EndCounter = 32 - (CounterDiff);

            for (int counter = StartCounter; counter <= EndCounter; counter++)
            {
                Hexvalue = Hexvalue + HexCollection[counter];
            }

            Double HexTODeciVal = CommonOperation.HexaDecimalToDecimalConversion(Hexvalue);
            //Altitude Parsing not completed yet
            return Altitude;
        }

        private double ParseSpeed(string[] HexCollection)
        {
            string Hexvalue = "";
            double Speed = 0;
            int CounterDiff = StdDevIDLth - ClntDevIDLth;
            int StartCounter = 33 - (CounterDiff);
            int EndCounter = 36 - (CounterDiff);

            for (int counter = StartCounter; counter <= EndCounter; counter++)
            {
                Hexvalue = Hexvalue + HexCollection[counter];
            }

            Double HexTODeciVal = CommonOperation.HexaDecimalToDecimalConversion(Hexvalue);
            Speed= HexTODeciVal * 0.036;
            return Speed;
        }

        private double ParseHeadingDirection(string[] HexCollection)
        {
            string Hexvalue = "";
            double Direction = 0;
            int CounterDiff = StdDevIDLth - ClntDevIDLth;
            int StartCounter = 37 - (CounterDiff);
            int EndCounter = 38 - (CounterDiff);

            for (int counter = StartCounter; counter <= EndCounter; counter++)
            {
                Hexvalue = Hexvalue + HexCollection[counter];
            }

            Double HexTODeciVal = CommonOperation.HexaDecimalToDecimalConversion(Hexvalue);
            return HexTODeciVal;
        }

        private double ParseCalAmpLatLongValues(string hexNumber)
        {
            long dec = Int64.Parse(hexNumber, System.Globalization.NumberStyles.HexNumber);
            return (dec < Int64.Parse("7FFFFFFF", System.Globalization.NumberStyles.HexNumber)) ? dec * 0.0000001 : 0 - ((Int64.Parse("FFFFFFFF", System.Globalization.NumberStyles.HexNumber) - dec) * 0.0000001);
        }

        private string DegreesToCardinal(double degrees)
        {
            string[] caridnals = { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };
            return caridnals[(int)Math.Round(((double)degrees % 360) / 45)];
        }

        public byte[]  SetResponseMessage(string TrackerID = "")
        {
            List<byte> RspnStream = new List<byte>();

            //83 02 F9 99 01 01
            //01 06 00 00 01 04 0B 00 03 00 00 B3 00 00
            
            //83 05 45 62 40 27 06
            RspnStream.Add(0x83);

            //Device ID Length
            RspnStream.Add(0x05);
            
            //Device ID
            RspnStream.Add(0x45);
            RspnStream.Add(0x62);
            RspnStream.Add(0x40);
            RspnStream.Add(0x27);
            RspnStream.Add(0x06);

            RspnStream.Add(0x01);
            RspnStream.Add(0x01);

            //Service type 01 06
            RspnStream.Add(0x01);
            //Message type
            RspnStream.Add(0x06);

            //Sequence type 00 00 01 04 0B 00 03 00 00 B3 00 00
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);
            //End Sequence type

            //Action
            RspnStream.Add(0x01);
            //End Action

            //Parameter Command
            RspnStream.Add(0x01);
            RspnStream.Add(0x06);

            //Length
            RspnStream.Add(0x00);
            RspnStream.Add(0x05);

            //Index
            RspnStream.Add(0x00);

            //sec 
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);
            RspnStream.Add(0x3C);
            //0000
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);
            //0000
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);

            return  RspnStream.ToArray();
        }

        
        public byte[] TimeDistanceReporting(string TrackerID="")
        {
            List<byte> RspnStream = new List<byte>();
            TrackerID = "4562402706";
            int counter = 1;
            string DevIDCombination = "";
            //83 02 F9 99 01 01
            //01 06 00 00 01 04 0B 00 03 00 00 B3 00 00

            //83 05 45 62 40 27 06                      

            RspnStream.Add(0x83);

            //Device ID Length
            RspnStream.Add(0x05);

            //Device ID
            foreach (char DevIDChar in TrackerID)
            {
                DevIDCombination = DevIDCombination + DevIDChar;

                if (counter % 2 == 0)
                {
                    RspnStream.Add(Convert.ToByte(Convert.ToInt16(DevIDCombination)));
                    DevIDCombination = "";
                }
                counter = counter + 1;
            }
            //RspnStream.Add(0x45);
            //RspnStream.Add(0x62);
            //RspnStream.Add(0x40);
            //RspnStream.Add(0x27);
            //RspnStream.Add(0x06);

            RspnStream.Add(0x01);
            RspnStream.Add(0x01);

            //Service type 01 06
            RspnStream.Add(0x01);
            //Message type
            RspnStream.Add(0x06);

            //Sequence type 00 00 01 04 0B 00 03 00 00 B3 00 00
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);
            //End Sequence type

            //Action
            RspnStream.Add(0x01);
            //End Action

            //Parameter Command
            RspnStream.Add(0x01);
            RspnStream.Add(0x06);

            //Length
            RspnStream.Add(0x00);
            RspnStream.Add(0x05);

            //Index
            RspnStream.Add(0x00);

            //sec 
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);
            RspnStream.Add(0x3C);

            //Second Param
            RspnStream.Add(0x01);
            RspnStream.Add(0x07);
            //Length
            RspnStream.Add(0x00);
            RspnStream.Add(0x05);
            //Index
            RspnStream.Add(0x00);
            //Param Val
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);
            RspnStream.Add(0x03);
            RspnStream.Add(0xE8);

            //0000
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);
            //0000
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);

            return RspnStream.ToArray();
        }

        public byte[]  TrackOnDemand(string TrackerID = "")
        {
            TrackerID = "4562402706";
            List<byte> RspnStream = new List<byte>();
            RspnStream.Add(0x83);
           // RspnStream.Add(0x05);
            byte[] Lnth = CommonOperation.StringToByteArray(Convert.ToString(ClntDevIDLth));
            RspnStream.Add(Lnth[0]);

            byte[] IMEIArr = CommonOperation.StringToByteArray(TrackerID);

            foreach (byte bte in IMEIArr)
            {
                RspnStream.Add(bte);
            }


            //Device ID 
            //RspnStream.Add(0x45);
            //RspnStream.Add(0x62);
            //RspnStream.Add(0x40);
            //RspnStream.Add(0x27);
            //RspnStream.Add(0x06);

            //
            RspnStream.Add(0x01);
            RspnStream.Add(0x01);
            RspnStream.Add(0x01);

            //
            RspnStream.Add(0x07);

            //
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);

            //
            RspnStream.Add(0x0A);
            //Data 8 
            RspnStream.Add(0x00);
            //Data 16
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);

            //Data32
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);

            return RspnStream.ToArray();
        }

        public byte[]  UpdateEndRequest(string TrackerID="")
        {
            TrackerID = "4562402706";
            List<byte> RspnStream = new List<byte>();
            string DevIDCombination = "";

            RspnStream.Add(0x83);
            byte[] Lnth = CommonOperation.StringToByteArray(Convert.ToString(ClntDevIDLth));
            RspnStream.Add(Lnth[0]);

            byte[] IMEIArr = CommonOperation.StringToByteArray(TrackerID);

            foreach (byte bte in IMEIArr)
            {
                RspnStream.Add(bte);
            }

            RspnStream.Add(0x01);
            RspnStream.Add(0x01);

            RspnStream.Add(0x01);
            RspnStream.Add(0x06);
            //Sequence
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);

            return new Byte[2];
        }

        public byte[] UpdateBeginRequest(string TrackerID = "")
        {
            TrackerID = "4562402706";
            List<byte> RspnStream = new List<byte>();            
            string DevIDCombination = "";

            RspnStream.Add(0x83);
            byte[] Lnth= CommonOperation.StringToByteArray(Convert.ToString(ClntDevIDLth));
            RspnStream.Add(Lnth[0]);

            byte[] IMEIArr = CommonOperation.StringToByteArray(TrackerID);

            foreach (byte bte in IMEIArr)
            {
                RspnStream.Add(bte);
            }           

            RspnStream.Add(0x01);
            RspnStream.Add(0x01);

            RspnStream.Add(0x01);
            RspnStream.Add(0x06);
            //Sequence
            RspnStream.Add(0x00);
            RspnStream.Add(0x00);
            //Action
            RspnStream.Add(0x05);

            RspnStream.Add(0x00);
            RspnStream.Add(0x00);

            RspnStream.Add(0x00);
            RspnStream.Add(0x00);

            return RspnStream.ToArray();
        }
    }
}
