using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using System.Net.Http.Headers;
using System.Linq;
using AirViewTracker.DBLayer.Fleet;
using AirViewTracker.DBLayer;
using System.Text.RegularExpressions;
using System.Text;
using TcpService.Parsers;

namespace TcpService
{
    public partial class Service1 : ServiceBase
    {
        private TcpListener listener { get; set; }
        private UdpClient Udplistener { get; set; }
        private List<DataPacket> LstDataPacket = new List<DataPacket>();
        private string FilePath = "";
        private string ExceptionLogPath = "";
        private string TrackerId = "";
        private string SignalRApi = "";
        private bool GpsDataPacketTaskStatus = true;
        private bool LoginConfirmationTaskStatus;
        private bool accept { get; set; } = false;
                  
        private System.Timers.Timer DeviceIDDbTimer;
        private System.Timers.Timer UdpMsgTimer;
        private int DeviceIDDbTime;
        private static List<ClientInfo> tcpClientsList = new List<ClientInfo>();
        private List<string> TrackerIDCollection = new List<string>();
        private int ListnerCounter = 0;
        static string ClientToTrack = "";
        private FM_VehicleBL ObjVehicle;
        private List<string> IMEICollect = new List<string>();
        private TrackerCommands ObjTrackerCommands;
        string IPAddres;
        int PortNo;
        int SinocastelGpsPackageInterval;
        CommonOperation ObjCmnOp;
        private bool WriteExceptionLog;
        private bool WriteLog;
        private String ConnectionString;
        private bool CheckIndividualTrackerFromDb;
        private List<UdpClientInfo> lstUDPClnt;
        private static int testUdpCounter = 0;
        private int TrackerTimeOut = 0;
        private bool IsClntWhileLopRunning = false;
        private int IdleTime = 0;
        private static readonly Object LockThread = new Object();

        public Service1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// On Service Start From Services.msc
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                IPAddres = ConfigurationManager.AppSettings["ServerIP"].ToString();
                PortNo = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                FilePath = ConfigurationManager.AppSettings["FilePath"];
                ExceptionLogPath = ConfigurationManager.AppSettings["ExceptionLogPath"];                                
                SignalRApi = ConfigurationManager.AppSettings["SignalRApi"];
                TrackerTimeOut = Convert.ToInt16(ConfigurationManager.AppSettings["TrackerTimeOut"]);
                StartServer(IPAddres, PortNo);                
                File.AppendAllText(FilePath, Environment.NewLine + DateTime.Now.ToString() + ": Service Started Successfullly");
                ObjCmnOp = new CommonOperation(FilePath, ExceptionLogPath, SignalRApi);
                SinocastelGpsPackageInterval = Convert.ToInt16(ConfigurationManager.AppSettings["SinocastelGpsPackageInterval"]);
                WriteExceptionLog = Convert.ToBoolean(ConfigurationManager.AppSettings["WriteExceptionLog"]);
                WriteLog = Convert.ToBoolean(ConfigurationManager.AppSettings["WriteLogFile"]);
                DeviceIDDbTime = Convert.ToInt16(ConfigurationManager.AppSettings["TrackerListUpdateInterval"]);
                IdleTime = Convert.ToInt16(ConfigurationManager.AppSettings["IdleTime"]);
                CheckIndividualTrackerFromDb = true;
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
            }
        }

        /// <summary>
        /// On Service Stop From Services.msc
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                listener.Stop();
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
            }
        }

        /// <summary>
        /// Starting a TCP Server to listen client request 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void StartServer(string ip, int port)
        {            
            IPAddress address = IPAddress.Parse(ip);
            listener = new TcpListener(address, port);
            listener.Start();
            accept = true;
            listener.Start();
            StartListener();
            lstUDPClnt = new List<UdpClientInfo>();
            //UDPListener();
            //Thread ThrdUDPListener = new Thread(() => UDPListener());
            //ThrdUDPListener.Start();

            ConnectionString = ConfigurationManager.ConnectionStrings["AirViewConnectionString"].ConnectionString;
            ObjVehicle = new FM_VehicleBL(ConnectionString);
            IMEICollect = ObjVehicle.GetAllVehicleIMEI("Get_All_IMEI");


        }

        public void UDPListener()
        {
            
            UdpMsgTimer = new System.Timers.Timer();
            string TrackerHexaResponse = "";
            string WriteRspn = "";
            try
            {
                Udplistener = new UdpClient(587);
                IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 587);
                byte[] RspnStream;

                //UdpMsgTimer.Interval = 10 * 1000;   // every 10 sec
                //UdpMsgTimer.Elapsed += (sender, e) => SendParameterMsgOnUDp();
                //UdpMsgTimer.AutoReset = false;

                while (true)
                {
                    byte[] data = Udplistener.Receive(ref serverEP);
                    UdpClientInfo ObjUdpClientInfo = new UdpClientInfo();

                    ObjUdpClientInfo.IPAddress = serverEP.Address;
                    ObjUdpClientInfo.PortNo = serverEP.Port;
                    ObjUdpClientInfo.Manufacturer = TrackerManufacturer.Calamp;

                    RspnStream = ParseUDPPacket(serverEP.Address, serverEP.Port, data, ObjUdpClientInfo);

                    if (RspnStream != null)
                    {
                        TrackerHexaResponse = "";

                        TrackerHexaResponse = BitConverter.ToString(RspnStream, 0, RspnStream.Length).Replace("-", " ");
                        WriteRspn = "";
                        WriteRspn = "Server IP: "+ serverEP.Address.ToString() + " Port: " + serverEP.Port;
                        WriteRspn = WriteRspn + Environment.NewLine + DateTime.Now.ToString() + " Response Packet without processing ";
                        WriteRspn = WriteRspn + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                        WriteRspn = WriteRspn + Environment.NewLine;
                        File.AppendAllText(FilePath, WriteRspn);
                        
                        Udplistener.Send(RspnStream, RspnStream.Length, serverEP);
                    }

                    //UdpMsgTimer.Start();

                 // makes it fire only on

                    // RaiseDataReceived(new ReceivedDataArgs(serverEP.Address,serverEP.Port,data));
                }
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
            }
        }

        private void SendParameterMsgOnUDp()
        {
            string WriteRspn = "";
            string TrackerHexaResponse = "";

            try
            {
                
                for (int counter = 0; counter<lstUDPClnt.Count;counter++)
                {
                    CalampParser ObjcalAmp = new CalampParser(lstUDPClnt[counter], ConnectionString);
                    Byte[] RspnStream;

                    if (testUdpCounter < 1)
                    {
                        RspnStream = ObjcalAmp.SetResponseMessage();
                        testUdpCounter = testUdpCounter + 1;
                    }
                    else if(testUdpCounter==1)
                    {
                        RspnStream = ObjcalAmp.UpdateBeginRequest();
                        testUdpCounter = testUdpCounter + 1;
                    }
                    else if (testUdpCounter == 2)
                    {
                        RspnStream = ObjcalAmp.UpdateEndRequest();
                        testUdpCounter = testUdpCounter + 1;
                    }
                    else
                    {
                        RspnStream = ObjcalAmp.TrackOnDemand();
                    }
                    
                    IPEndPoint serverEP = new IPEndPoint(lstUDPClnt[counter].IPAddress, lstUDPClnt[counter].PortNo);

                    if (RspnStream!= null)
                    {
                        TrackerHexaResponse = BitConverter.ToString(RspnStream, 0, RspnStream.Length).Replace("-", " ");
                        WriteRspn = "";
                        WriteRspn = "Server IP: " + serverEP.Address.ToString() + " Port: " + serverEP.Port;
                        WriteRspn = WriteRspn + Environment.NewLine + DateTime.Now.ToString() + " Response Packet for Timer";
                        WriteRspn = WriteRspn + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                        WriteRspn = WriteRspn + Environment.NewLine;
                        File.AppendAllText(FilePath, WriteRspn);

                        Udplistener.Send(RspnStream, RspnStream.Length, serverEP);
                    }
                }
                UdpMsgTimer.Start();
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
            }

        }
        private byte[] ParseUDPPacket(IPAddress address, int Port, Byte[] data, UdpClientInfo ObjUdpClientInfo)
        {          
            string TrackerHexaResponse = "";
            string dataFromClient = "";
            string[] HexCollection;
            string UDPData = "";
            byte[] AckStream;

            TrackerHexaResponse = BitConverter.ToString(data, 0, data.Length).Replace("-", " ");
            HexCollection = TrackerHexaResponse.Split(' ');
            dataFromClient = System.Text.Encoding.ASCII.GetString(data);

            if (WriteLog)
            {
                UDPData = Environment.NewLine + DateTime.Now.ToString() + "Initial UDP Packet: ";
                UDPData = UDPData + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                UDPData = UDPData + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                UDPData = UDPData + Environment.NewLine;
                File.AppendAllText(FilePath, UDPData);
            }
            
            CalampParser ObjcalAmp = new CalampParser(data, ExceptionLogPath, FilePath, WriteLog, WriteExceptionLog, SignalRApi, ref lstUDPClnt, ref ObjUdpClientInfo, IMEICollect,ConnectionString);

            //ObjTrackerCommands = new TrackerCommands(ExceptionLogPath, FilePath, SignalRApi, ref tcpClientsList, WriteLog, WriteExceptionLog, IMEICollect, ConnectionString, CheckIndividualTrackerFromDb);                      
            AckStream = ObjcalAmp.ParseEventRpt();
            return AckStream;           
        }

        /// <summary>
        /// Start Listening to client
        /// </summary>
        public async void StartListener()
        {

            while (true)
            {
                try
                {
                    TcpClient client = await listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    // AddClientInList(ref client);

                    //new Thread(() => AddClientInList(ref client)).Start();
                    Task.Run(() => AddClientInList(ref client));
                }

                catch (Exception ex)
                {
                    //string exception = Environment.NewLine + ex.ToString();
                    //ObjCmnOp.WriteExceptionFile(exception)
                }
            }
        }

        private void AddClientInList(ref TcpClient client)
        {
            ClientInfo ClntInfo = new ClientInfo();
            ClntInfo.client = client;
            ClntInfo.IMEI = "";
            tcpClientsList.Add(ClntInfo);
            ListnerCounter = ListnerCounter + 1;
            ConnectionString = ConfigurationManager.ConnectionStrings["AirViewConnectionString"].ConnectionString;

            if (ListnerCounter ==1)
            {
                ObjVehicle = new FM_VehicleBL(ConnectionString);
                IMEICollect = ObjVehicle.GetAllVehicleIMEI("Get_All_IMEI");
                if (DeviceIDDbTime > 0)
                {
                    DeviceIDDbTimer = new System.Timers.Timer();
                    DeviceIDDbTimer.Interval = DeviceIDDbTime * 1000;   // every 10 sec
                    DeviceIDDbTimer.Elapsed += (sender, e) => UpdateTrackerIDList();
                    DeviceIDDbTimer.AutoReset = false;  // makes it fire only on
                    DeviceIDDbTimer.Start();
                }
                IsClntWhileLopRunning = true;
                HandleClient();
            }

            if (!IsClntWhileLopRunning)
            {
                IsClntWhileLopRunning = true;
                HandleClient();
            }
        }

        private void HandleClient()
        {
                 
            while (true)
            {
                try
                {
                    if (tcpClientsList.ToList().Count == 0)
                    {
                        IsClntWhileLopRunning = false;
                        break;
                    }
                    foreach (ClientInfo ClintInf in tcpClientsList.ToList())
                    {
                        if (ClintInf.client.Connected)
                        {
                            
                            NetworkStream networkStream = ClintInf.client.GetStream();
                            if (networkStream.DataAvailable)
                            {
                                if (ClintInf.LstPktRcvdTime == DateTime.MinValue)
                                {
                                    ClintInf.PktBeforeLastRcvdTime = DateTime.Now;
                                }
                                else
                                {
                                    ClintInf.PktBeforeLastRcvdTime = ClintInf.LstPktRcvdTime;
                                }
                                
                                ClintInf.LstPktRcvdTime = DateTime.Now;
                                Task.Run(() => ParseNetworkStream(ClintInf));
                                //ParseNetworkStream(ClintInf);
                                //new Thread(() => ParseNetworkStream(ClintInf)).Start();
                            }
                            LogOutTrackerOnSleep(ClintInf);
                        }

                        // Task.Run(() => ParseNetworkStream(networkStream));

                    }
                    Thread.Sleep(200);
                }
                catch (Exception ex)
                {
                    if (WriteExceptionLog)
                    {
                        string exception = Environment.NewLine + DateTime.Now.ToString() + ex.ToString();
                        ObjCmnOp.WriteExceptionFile(exception);
                    }
                }
            }
        }

        private void LogOutTrackerOnSleep(ClientInfo clntInfo)
        {
            if (!(default(DateTime) == clntInfo.LstPktRcvdTime))
            {

                int MinutesDifference = ((int)clntInfo.LstPktRcvdTime.Subtract(DateTime.UtcNow).TotalMinutes) * -1;

                if (MinutesDifference >= TrackerTimeOut)
                {
                    clntInfo.client.GetStream().Close();
                    clntInfo.client.Close();
                    tcpClientsList.RemoveAll(x => x.IMEI == clntInfo.IMEI);
                }
            }
        }

        private void UpdateTrackerIDList()
        {
            List<string> UpdatedDeviceID = new List<string>();
            try
            {
                UpdatedDeviceID = ObjVehicle.GetAllVehicleIMEI("Get_All_IMEI");
                IMEICollect = UpdatedDeviceID;
                DeviceIDDbTimer.Start();
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
            }
            
        }
        public void ParseNetworkStream(ClientInfo tpclient)
        {
            byte[] bytesFrom = new byte[2000];
            string ClientSideTrackerID = "";
            //bool ConnectedClient = false;
            string TrackerHexaResponse = "";
            string dataFromClient = "";
            string data = "";
            string Hexa = "";
            string[] HexCollection;
            bool ClientData = false;
            NetworkStream networkStream = tpclient.client.GetStream();

            if (networkStream.DataAvailable)
            {
                data = "";

                try
                {
                    //TrackerIDCollection = TrackerId.Split(',').ToArray().ToList();
                    networkStream.Read(bytesFrom, 0, 1500);

                    ////changes for Mobile Testing
                    //TrackerHexaResponse = BitConverter.ToString(bytesFrom, 0, 1500).Replace("-", " ");
                    //HexCollection = TrackerHexaResponse.Split(' ');
                    //dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    //data = "";
                    //data = Environment.NewLine + DateTime.Now.ToString() + " Initial Packet without processing ";
                    //data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                    //data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                    //data = data + Environment.NewLine + "TrackerId: " + ": " + ClientSideTrackerID;
                    //data = data + Environment.NewLine;
                    //File.AppendAllText(FilePath, data);
                    // //end changes for initial CAlemp

                    ClientSideTrackerID = getTrackerID(bytesFrom);
                 

                    TrackerHexaResponse = BitConverter.ToString(bytesFrom, 0, 1500).Replace("-", " ");
                    HexCollection = TrackerHexaResponse.Split(' ');

                    int ActualLength = 0;

                    if (!(HexCollection[0] == "25" && HexCollection[1] == "25") && !(HexCollection[0] == "24" && HexCollection[1] == "24") && !(HexCollection[0] == "40" && HexCollection[1] == "40"))
                    {
                        tcpClientsList.RemoveAll(x => x==tpclient && (tpclient.IMEI=="" || String.IsNullOrEmpty(tpclient.IMEI)));
                        return;
                    }

                    if (HexCollection[0] == "25" && HexCollection[1] == "25")
                    {
                        dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                        ActualLength = Convert.ToInt32(dataFromClient.Split(',')[1]);
                        ClientData = true;
                    }
                    else
                    {
                        ActualLength = GetActualLength(bytesFrom);
                    }

                    if (ActualLength > 0 && !ClientData)
                    {


                        if (HexCollection[0] == "40" && HexCollection[1] == "40")
                        {
                            if (ActualLength > 250)
                            {
                                return;
                            }
                            TrackerHexaResponse = BitConverter.ToString(bytesFrom, 0, ActualLength).Replace("-", " ");
                            dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                            HexCollection = TrackerHexaResponse.Split(' ');

                            ObjTrackerCommands = new TrackerCommands(ExceptionLogPath, FilePath, SignalRApi, ref tcpClientsList, WriteLog, WriteExceptionLog, IMEICollect, ConnectionString, CheckIndividualTrackerFromDb, IdleTime);
                            ObjTrackerCommands.HandleIncomingSinoCastelPacket(HexCollection, bytesFrom, TrackerHexaResponse, dataFromClient.Trim('\0'), ref tpclient, networkStream, SinocastelGpsPackageInterval);
                        }

                        else
                        { 
                            TrackerHexaResponse = BitConverter.ToString(bytesFrom, 0, ActualLength).Replace("-", " ");
                            dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                            HexCollection = TrackerHexaResponse.Split(' ');
                            bool IsOfflinePkt = IsGPSOfflinePkt(bytesFrom, ActualLength);
                            (BitConverter.ToString(bytesFrom, 0, ActualLength).Replace("-", " ")).Split(' ');

                            if (HexCollection.Length > 12)
                            {
                                if (HexCollection[3] == "11" && HexCollection[11] == "50" && HexCollection[12] == "00" && IsOfflinePkt == false)
                                {
                                    if (WriteLog)
                                    {
                                        data = "";
                                        data = Environment.NewLine + DateTime.Now.ToString() + " Client: Login ";
                                        data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                        data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                        data = data + Environment.NewLine + "TrackerId: " + ": " + ClientSideTrackerID;
                                        data = data + Environment.NewLine;
                                        ObjCmnOp.WriteFile(data);
                                    }

                                  //  bool IfExists = IMEICollect.Contains(ClientSideTrackerID);
                                                                     
                                    if (CheckIndividualTrackerFromDb)
                                    {
                                        bool ClientIMEI = ObjVehicle.ValidateVehicleIMEI("Validate_IMEI", ClientSideTrackerID);

                                        if (ClientIMEI)
                                        {
                                            IMEICollect.Add(ClientSideTrackerID);
                                            ParseLoginRequestAndConfirm(bytesFrom, TrackerHexaResponse, networkStream);
                                            tcpClientsList.RemoveAll(x => x.IMEI == ClientSideTrackerID);
                                            tpclient.IMEI = ClientSideTrackerID;
                                            tpclient.Manufacturer = TrackerManufacturer.Top10;

                                            string counter = "";
                                            counter = ObjVehicle.GetCurrentTripCounterForClient("GetCurrentTrackerTrips", tpclient.IMEI, DateTime.UtcNow.ToString("d"));

                                            if (string.IsNullOrEmpty(counter))
                                            {
                                                tpclient.CurrentTripCounter = -1;
                                            }
                                            else
                                            {
                                                tpclient.CurrentTripCounter = (Convert.ToInt16(counter)+1);
                                            }

                                            counter = ObjVehicle.GetTripIdleValue("GetTripIdleConfig", tpclient.IMEI);

                                            if (string.IsNullOrEmpty(counter))
                                            {
                                                tpclient.TripIdleTime = IdleTime;
                                            }
                                            else
                                            {
                                                tpclient.TripIdleTime = (int)Convert.ToDecimal(counter); ;
                                            }


                                        }
                                        else
                                        {
                                            tpclient.IMEI = ClientSideTrackerID;
                                            tpclient.Manufacturer = TrackerManufacturer.Top10;
                                            tcpClientsList.RemoveAll(x => x.IMEI == ClientSideTrackerID);
                                        }
                                    }
                                    else
                                    {
                                        tpclient.IMEI = ClientSideTrackerID;
                                        tpclient.Manufacturer = TrackerManufacturer.Top10;
                                        tcpClientsList.RemoveAll(x => x.IMEI == ClientSideTrackerID);
                                    }                                    
                                }

                                if (HexCollection[11] == "99" && HexCollection[12] == "55" && IsOfflinePkt == false)
                                {
                                    if (WriteLog)
                                    {
                                        data = "";
                                        data = Environment.NewLine + DateTime.Now.ToString() + " Client: Standard GPS Packet";
                                        data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                        data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                        data = data + Environment.NewLine;
                                        ObjCmnOp.WriteFile(data);
                                    }

                                    ParseGpsTrackerPacket(tpclient,bytesFrom, ActualLength, ClientSideTrackerID, TrackerHexaResponse);
                                }

                                if (HexCollection[11] == "99" && HexCollection[12] == "99")
                                {
                                    data = "";
                                    data = Environment.NewLine + DateTime.Now.ToString() + " Client: Alarm Packet T10";
                                    data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                    data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                    data = data + Environment.NewLine;
                                    ObjCmnOp.WriteFile(data);
                                    DataPacket objDtPkt = new DataPacket();

                                    TopTen ObjTopTenCmds = new TopTen(ExceptionLogPath, FilePath, SignalRApi, ref tcpClientsList, WriteLog, WriteExceptionLog, IMEICollect, ConnectionString, CheckIndividualTrackerFromDb);
                                    ObjTopTenCmds.ParseAlarmPcket(HexCollection, TrackerHexaResponse, ClientSideTrackerID, objDtPkt);
                                    ParseGpsTrackerPacket(tpclient,bytesFrom, ActualLength, ClientSideTrackerID, TrackerHexaResponse, false, objDtPkt, true);
                                }

                                if (HexCollection[11] == "41" && HexCollection[12] == "02" && IsOfflinePkt == false)
                                {
                                    if (HexCollection[13] == "00")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " Client: Gps TimeSetting Response";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Time Set " + ": " + "Failure";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }
                                    }

                                    else if (HexCollection[13] == "01")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " Client: Gps TimeSetting Response";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Time Set " + ": " + "Success";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }
                                    }
                                }

                                // Phone Authorization Response 41 03
                                if (HexCollection[11] == "41" && HexCollection[12] == "03" && IsOfflinePkt == false)
                                {
                                    if (HexCollection[13] == "01")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " Client: PhoneAuthorization Successful Implemented";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Phone Authorized: " + ": " + "Success";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }
                                    }

                                    else if (HexCollection[13] == "00")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " Client: PhoneAuthorization Successful Implemented";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Phone Authorized: " + ": " + "Failure";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }
                                    }
                                }

                                if (HexCollection[11] == "51" && HexCollection[12] == "00" && IsOfflinePkt == false)
                                {
                                    if (HexCollection[13] == "00")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " Client: Gps TimeSetting Response";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Time Set " + ": " + "Failure";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }
                                    }

                                    else if (HexCollection[13] == "01")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " Client: Gps TimeSetting Response";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Time Set " + ": " + "Success";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }
                                    }
                                }

                                //Speed Limit Response
                                if (HexCollection[11] == "41" && HexCollection[12] == "05" && IsOfflinePkt == false)
                                {
                                    if (HexCollection[13] == "00")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " Speed Limit response";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Speed Limit Response " + ": " + "Failure";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);                                            
                                        }
                                    }

                                    else if (HexCollection[13] == "01")
                                    {
                                        string Filter = "UpdateConfig_ByTID";
                                        ObjVehicle.UpdateAlarmsSettings_ByTID(Filter, ClientSideTrackerID, AlarmCodes.Spd.ToString());

                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " Speed Limit Response";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Speed Limit Response " + ": " + "Success";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }

                                    }
                                }

                                //40 40 00 11 58 04 20 80 04 57 92 41 10 01/00 CF B4 0D 0A  
                                //Initialization request   
                                //The tracker might not send this response but it take effect
                                if (HexCollection[11] == "41" && HexCollection[12] == "10" && IsOfflinePkt == false)
                                {
                                    if (HexCollection[13] == "00")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + "Initialization Request";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Initialization Request" + ": " + "Failure";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }
                                    }

                                    else if (HexCollection[13] == "01")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " Speed Limit Response";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Initialization Request " + ": " + "Success";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }

                                    }
                                }

                                //Reboot Device
                                if (HexCollection[11] == "49" && HexCollection[12] == "02" && IsOfflinePkt == false)
                                {
                                    if (HexCollection[13] == "00")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + "Tracker Reboot";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Tracker Reboot" + ": " + "Failure";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }
                                    }

                                    else if (HexCollection[13] == "01")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " Tracker Reboot";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Tracker Reboot" + ": " + "Success";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }

                                    }
                                }

                                //5503 Clear DataLogger
                                if (HexCollection[11] == "55" && HexCollection[12] == "03" && IsOfflinePkt == false)
                                {
                                    if (HexCollection[13] == "00")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + "Clear DataLog";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Clear DataLog" + ": " + "Failure";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }
                                    }

                                    else if (HexCollection[13] == "01")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " Tracker Reboot";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "Clear DataLog" + ": " + "Success";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }

                                    }
                                }

                                //4116 Arm And Disarm vehicle
                                if (HexCollection[11] == "41" && HexCollection[12] == "16" && IsOfflinePkt == false)
                                {
                                    if (HexCollection[13] == "00")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + "ArmDisarm Vehicle";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "ArmDisarm Vehicle" + ": " + "Failure";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }
                                    }

                                    else if (HexCollection[13] == "01")
                                    {
                                        if (WriteLog)
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " ArmDisarm Vehicle";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "ArmDisarm Vehicle" + ": " + "Success";
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);
                                        }
                                    }
                                }

                                if (IsOfflinePkt)
                                {
                                    ParsingOfflineStream(ref tpclient, HexCollection, bytesFrom, ClientSideTrackerID);
                                }

                                if (GpsDataPacketTaskStatus)
                                {
                                    // GpsDataPacketTimer.Start();
                                }
                                else
                                {
                                    //GpsDataPacketTimer.Stop();
                                }
                            }
                         }
                        }
                    // }

                    else

                    {
                        TrackerHexaResponse = BitConverter.ToString(bytesFrom, 0, ActualLength).Replace("-", " ");
                        dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom,0,ActualLength);
                        HexCollection = TrackerHexaResponse.Split(' ');
                        tpclient.IMEI = "WebClient";

                        ObjTrackerCommands =new TrackerCommands(ExceptionLogPath, FilePath,SignalRApi, ref tcpClientsList,WriteLog, WriteExceptionLog, IMEICollect, ConnectionString, CheckIndividualTrackerFromDb,IdleTime);
                        //trackerOnDemand
                        if (HexCollection[0] == "25" && HexCollection[1] == "25")
                        {
                            if (HexCollection[6] == "34" && HexCollection[7] == "31" && HexCollection[8] == "30" && HexCollection[9] == "31")
                            {
                                int NumberOfIMEI = 0;
                                string[] CommandCollection = dataFromClient.Split(',');
                                NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);

                                int IMEIStartPt = 4;
                                List<string> IMEILst = new List<string>();

                                string CurrentIMEI = "";
                                int AlphaCounter = Regex.Matches(CommandCollection[IMEIStartPt].ToString(), @"[a-zA-Z]").Count;
                                //IMEI
                                for (int counter = 1; counter <= NumberOfIMEI; counter++)
                                {
                                    string HexIMEI;
                                    if (AlphaCounter == 0)
                                    {
                                        Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                                        HexIMEI = String.Format("{0:X}", IMEIVal.ToString());
                                        CurrentIMEI = IMEIVal.ToString();                                       
                                    }
                                    else
                                    {
                                        byte[] ba = Encoding.Default.GetBytes(CommandCollection[IMEIStartPt]);
                                        string hexString = BitConverter.ToString(ba);
                                        HexIMEI = hexString.Replace("-", "");
                                        CurrentIMEI = CommandCollection[IMEIStartPt];
                                    }

                                    IMEILst.Add(HexIMEI);                                   
                                }

                                List<ClientInfo> ObjClnt = tcpClientsList.Where(x => x.IMEI == CurrentIMEI).ToList();
                                List<List<Byte>> ListByt;
                                if (ObjClnt.Count > 0)
                                {
                                    if (ObjClnt[0].Manufacturer == TrackerManufacturer.Top10)
                                    {
                                        ListByt = CreatingOnDemandRequest(IMEILst);
                                    }
                                    else 
                                    { //if(ObjClnt[0].Manufacturer == TrackerManufacturer.Sinocastel){                                   
                                        ListByt = ObjTrackerCommands.SinocatelTrackOnDemand(IMEILst);
                                    }
                                    
                                    //List<List<Byte>> Commands = CreatingOnDemandRequest(IMEILst);
                                    if (ListByt != null)
                                    {
                                        BroadCastRequest(ListByt, CurrentIMEI);
                                    }
                                }
                                else
                                {
                                    List<UdpClientInfo> LstUdpClientInfo = lstUDPClnt.Where(x => x.TrackerID == CurrentIMEI).ToList();
                                    UdpClientInfo objUdpClient = LstUdpClientInfo[0];
                                    CalampParser ObjcalAmp = new CalampParser(objUdpClient, ConnectionString);
                                    byte[] ResponseStream = ObjcalAmp.TrackOnDemand();
                                    UdpBroadCastRequest(ResponseStream, objUdpClient);
                                    ListByt = null;
                                }
                                //BroadCastRequest(Commands);
                               
                            }

                            //Tracker To track 33 32 31 30
                            if (HexCollection[6] == "33" && HexCollection[7] == "32" && HexCollection[8] == "31" && HexCollection[9] == "30")
                            {
                                int NumberOfIMEI = 0;
                                string[] CommandCollection = dataFromClient.Split(',');
                                NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);

                                int IMEIStartPt = 4;
                                List<string> IMEILst = new List<string>();

                                for (int counter = 1; counter <= NumberOfIMEI; counter++)
                                {
                                    Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                                    string HexIMEI = String.Format("{0:X}", IMEIVal.ToString());
                                    IMEILst.Add(HexIMEI);
                                    ClientToTrack = IMEIVal.ToString();
                                }

                                //List<List<Byte>> Commands = CreatingOnDemandRequest(IMEILst);
                                //BroadCastRequest(Commands);
                            }

                            //4103 Set PhoneNumber Authorization
                            if (HexCollection[6] == "34" && HexCollection[7] == "31" && HexCollection[8] == "30" && HexCollection[9] == "33")
                            {
                                int NumberOfIMEI = 0;
                                string[] CommandCollection = dataFromClient.Split(',');
                                NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);

                                int IMEIStartPt = 4;
                                List<string> IMEILst = new List<string>();
                                string CurrentIMEI = "";
                                for (int counter = 1; counter <= NumberOfIMEI; counter++)
                                {
                                    Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                                    string HexIMEI = String.Format("{0:X}", IMEIVal.ToString());
                                    IMEILst.Add(HexIMEI);
                                    CurrentIMEI = IMEIVal.ToString();
                                }

                                //PhoneNumber
                                int PHNoStartPoint = 5;                                
                                long Phone = Convert.ToInt64(CommandCollection[PHNoStartPoint]);
                                string HexPhone = String.Format("{0:X}", Phone.ToString());
                               
                                char[] charValues = Phone.ToString().ToCharArray();
                                int countr = 0;
                                string hexOutput = "";
                                Byte[] PhoneByte = new byte[16];

                                foreach (char _eachChar in charValues)
                                {
                                    int value = Convert.ToInt32(_eachChar);                                    
                                    hexOutput += String.Format("{0:X}", value);
                                    countr = countr + 1;
                                }

                                int PhoneLth = hexOutput.Length;
                                for (int extraBits = PhoneLth; extraBits < 32; extraBits++)
                                {
                                    hexOutput += String.Format("{0:X}", 0);
                                }
                              
                                AuthorizationPhoneNumber(IMEILst, hexOutput, CurrentIMEI);                               
                            }

                            //4105 Set Speed Limit
                            if (HexCollection[6] == "34" && HexCollection[7] == "31" && HexCollection[8] == "30" && HexCollection[9] == "35")
                            {
                                int NumberOfIMEI = 0;
                                string[] CommandCollection = dataFromClient.Split(',');
                                NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);

                                int IMEIStartPt = 4;
                                List<string> IMEILst = new List<string>();
                                string CurrentIMEI = "";
                                for (int counter = 1; counter <= NumberOfIMEI; counter++)
                                {
                                    Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                                    string HexIMEI = String.Format("{0:X}", IMEIVal.ToString());
                                    IMEILst.Add(HexIMEI);                                    
                                    CurrentIMEI = IMEIVal.ToString();
                                }

                                //PhoneNumber
                                int SpeedLimit = 5;                                
                                int Speed = Convert.ToInt16(CommandCollection[SpeedLimit]);
                                //HandleSpeedLimit(IMEILst, Speed, CurrentIMEI);                           
                            }

                            if (HexCollection[6] == "34" && HexCollection[7] == "31" && HexCollection[8] == "30" && HexCollection[9] == "35")
                            {
                                int NumberOfIMEI = 0;
                                string[] CommandCollection = dataFromClient.Split(',');
                                NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);

                                int IMEIStartPt = 4;
                                List<string> IMEILst = new List<string>();
                                string CurrentIMEI = "";
                                for (int counter = 1; counter <= NumberOfIMEI; counter++)
                                {
                                    Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                                    string HexIMEI = String.Format("{0:X}", IMEIVal.ToString());
                                    IMEILst.Add(HexIMEI);
                                    CurrentIMEI = IMEIVal.ToString();
                                }

                                //PhoneNumber
                                int SpeedLimit = 5;
                                int Speed = Convert.ToInt16(CommandCollection[SpeedLimit]);
                                //HandleSpeedLimit(IMEILst, Speed, CurrentIMEI);
                            }

                            //Initilization Request
                            if (HexCollection[6] == "34" && HexCollection[7] == "31" && HexCollection[8] == "31" && HexCollection[9] == "30")
                            {
                                int NumberOfIMEI = 0;
                                string[] CommandCollection = dataFromClient.Split(',');
                                NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);

                                int IMEIStartPt = 4;
                                List<string> IMEILst = new List<string>();
                                string CurrentIMEI = "";
                                for (int counter = 1; counter <= NumberOfIMEI; counter++)
                                {
                                    Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                                    string HexIMEI = String.Format("{0:X}", IMEIVal.ToString());
                                    IMEILst.Add(HexIMEI);
                                    CurrentIMEI = IMEIVal.ToString();
                                }

                                InitilizeRequest(IMEILst, CurrentIMEI);                                
                            }

                            //reset request

                            if (HexCollection[6] == "34" && HexCollection[7] == "39" && HexCollection[8] == "30" && HexCollection[9] == "32")
                            {
                                int NumberOfIMEI = 0;
                                string[] CommandCollection = dataFromClient.Split(',');
                                NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);

                                int IMEIStartPt = 4;
                                List<string> IMEILst = new List<string>();
                                string CurrentIMEI = "";
                                for (int counter = 1; counter <= NumberOfIMEI; counter++)
                                {
                                    Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                                    string HexIMEI = String.Format("{0:X}", IMEIVal.ToString());
                                    IMEILst.Add(HexIMEI);
                                    CurrentIMEI = IMEIVal.ToString();
                                }

                                RebootRequest(IMEILst, CurrentIMEI);
                            }

                            //Clear Data Logger

                            if (HexCollection[6] == "35" && HexCollection[7] == "35" && HexCollection[8] == "30" && HexCollection[9] == "33")
                            {
                                int NumberOfIMEI = 0;
                                string[] CommandCollection = dataFromClient.Split(',');
                                NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);

                                int IMEIStartPt = 4;
                                List<string> IMEILst = new List<string>();
                                string CurrentIMEI = "";
                                for (int counter = 1; counter <= NumberOfIMEI; counter++)
                                {
                                    Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                                    string HexIMEI = String.Format("{0:X}", IMEIVal.ToString());
                                    IMEILst.Add(HexIMEI);
                                    CurrentIMEI = IMEIVal.ToString();
                                }

                                CleartheDataLogger(IMEILst, CurrentIMEI);
                               
                            }

                            //ArmDisArm Request
                            if (HexCollection[6] == "34" && HexCollection[7] == "31" && HexCollection[8] == "31" && HexCollection[9] == "36")
                            {
                                int NumberOfIMEI = 0;
                                string[] CommandCollection = dataFromClient.Split(',');
                                NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);
                                int CommandToSend = Convert.ToInt32(CommandCollection[5]);
                                int IMEIStartPt = 4;
                                List<string> IMEILst = new List<string>();
                                string CurrentIMEI = "";
                                for (int counter = 1; counter <= NumberOfIMEI; counter++)
                                {
                                    Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                                    string HexIMEI = String.Format("{0:X}", IMEIVal.ToString());
                                    IMEILst.Add(HexIMEI);
                                    CurrentIMEI = IMEIVal.ToString();
                                }

                                List<List<Byte>> ListByt = ObjTrackerCommands.ArmDisarmCar(IMEILst, CommandToSend);

                                if (ListByt != null)
                                {
                                    BroadCastRequest(ListByt, CurrentIMEI);
                                }
                            }

                            //4102
                            if (HexCollection[6] == "34" && HexCollection[7] == "31" && HexCollection[8] == "30" && HexCollection[9] == "32")
                            {
                                int NumberOfIMEI = 0;
                                string[] CommandCollection = dataFromClient.Split(',');
                                NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);

                                int IMEIStartPt = 4;
                                List<string> IMEILst = new List<string>();
                                string CurrentIMEI = "";

                                int AlphaCounter = Regex.Matches(CommandCollection[IMEIStartPt].ToString(), @"[a-zA-Z]").Count;

                                for (int counter = 1; counter <= NumberOfIMEI; counter++)
                                {
                                    string HexIMEI;
                                    if (AlphaCounter == 0)
                                    {
                                        Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                                        HexIMEI = String.Format("{0:X}", IMEIVal.ToString());
                                        CurrentIMEI = IMEIVal.ToString();
                                    }
                                    else
                                    {
                                        byte[] ba = Encoding.Default.GetBytes(CommandCollection[IMEIStartPt]);
                                        string hexString = BitConverter.ToString(ba);
                                        HexIMEI = hexString.Replace("-","");
                                        CurrentIMEI = CommandCollection[IMEIStartPt];
                                    }

                                    IMEILst.Add(HexIMEI);                                    
                                }

                                // Get time to set
                                int Time = 5;
                                string time = CommandCollection[Time].Replace("\0","");
                                List<List<Byte>> ListByt;

                                List<ClientInfo> ObjClnt = tcpClientsList.Where(x => x.IMEI == CurrentIMEI).ToList();

                                if (ObjClnt.Count > 0)
                                {
                                    if (ObjClnt[0].Manufacturer == TrackerManufacturer.Top10)
                                    {
                                        ListByt = ObjTrackerCommands.SetGprsTimeIntervalPacket(IMEILst, time.ToString());
                                    }
                                    else
                                    { //if(ObjClnt[0].Manufacturer == TrackerManufacturer.Sinocastel){                                   
                                        ListByt = ObjTrackerCommands.SinocastelCreateGpsTimeIntervalCommand(IMEILst, time.ToString());
                                    }

                                    if (ListByt != null)
                                    {
                                        BroadCastRequest(ListByt, CurrentIMEI);
                                    }
                                }
                            }

                            //logout Command 1002
                            if (HexCollection[6] == "31" && HexCollection[7] == "30" && HexCollection[8] == "30" && HexCollection[9] == "32")
                            {
                                int NumberOfIMEI = 0;
                                string[] CommandCollection = dataFromClient.Split(',');
                                NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);

                                int IMEIStartPt = 4;
                                List<string> IMEILst = new List<string>();
                                string CurrentIMEI = "";
                                string PreviousIMEI = "";

                                int AlphaCounter = Regex.Matches(CommandCollection[IMEIStartPt].ToString(), @"[a-zA-Z]").Count;

                                for (int counter = 1; counter <= NumberOfIMEI; counter++)
                                {
                                    string HexIMEI;

                                    if (!String.IsNullOrEmpty(CommandCollection[IMEIStartPt]))
                                    { 
                                        if (AlphaCounter == 0)
                                        {
                                            Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                                            HexIMEI = String.Format("{0:X}", IMEIVal.ToString());
                                            CurrentIMEI = IMEIVal.ToString();
                                        }
                                        else
                                        {
                                            byte[] ba = Encoding.Default.GetBytes(CommandCollection[IMEIStartPt]);
                                            string hexString = BitConverter.ToString(ba);
                                            HexIMEI = hexString.Replace("-", "");
                                            CurrentIMEI = CommandCollection[IMEIStartPt];
                                        }

                                        IMEILst.Add(HexIMEI);
                                        //Adding an IMEI
                                        if (!(IMEICollect.Contains(HexIMEI)))
                                        {
                                            IMEICollect.Add(HexIMEI);
                                        }
                                    }
                                    int PreviousIMEIPosition = 5;
                                    AlphaCounter = Regex.Matches(CommandCollection[PreviousIMEIPosition].ToString(), @"[a-zA-Z]").Count;

                                    if (AlphaCounter == 0)
                                    {
                                        Int64 IMEIVal = Convert.ToInt64(CommandCollection[PreviousIMEIPosition]);
                                        HexIMEI = String.Format("{0:X}", IMEIVal.ToString());
                                        PreviousIMEI = IMEIVal.ToString();
                                    }
                                    else
                                    {
                                        byte[] ba = Encoding.Default.GetBytes(CommandCollection[PreviousIMEIPosition]);
                                        string hexString = BitConverter.ToString(ba);
                                        HexIMEI = hexString.Replace("-", "");
                                        PreviousIMEI = CommandCollection[IMEIStartPt];
                                    }
                                    
                                    //Previous IMEI need to be removed from the  tcplist and IMEICollect
                                    IMEICollect.RemoveAll( x => x == HexIMEI);
                                    //lstUDPClnt.RemoveAll( x => x.TrackerID == HexIMEI);
                                    LogOutTracker(HexIMEI);
                                }
                            }

                            //Single Tracker SettingCommands
                            if (HexCollection[6] == "34" && HexCollection[7] == "34" && HexCollection[8] == "34" && HexCollection[9] == "34")
                            {
                                string[] CommandCollection = dataFromClient.Split(',');
                                ParsingSettingsCommand(CommandCollection);
                            }
                            //Multiple Trackers SettingCommand
                            else if (HexCollection[6] == "34" && HexCollection[7] == "34" && HexCollection[8] == "34" && HexCollection[9] == "35")
                            {
                                string[] CommandCollection = dataFromClient.Split(',');
                                ParsingSettingsCommandForMultipleTrackers(CommandCollection);
                                //ParsingSettingsCommand(CommandCollection);
                            }


                            if (HexCollection[6] == "37" && HexCollection[7] == "35" && HexCollection[8] == "30" && HexCollection[9] == "30")
                            {
                                string[] CommandCollection = dataFromClient.Split(',');

                                foreach (ClientInfo clnt in tcpClientsList.ToList())
                                {
                                    // ClientInfo clnt = ClntInfo[0];
                                    if (clnt.IMEI.Equals(CommandCollection[4]))
                                    {
                                        clnt.TripIdleTime = Convert.ToInt32(CommandCollection[5]);
                                    }
                                }

                            }

                            if (HexCollection[6] == "35" && HexCollection[7] == "38" && HexCollection[8] == "37" && HexCollection[9] == "30")
                            {
                                string[] CommandCollection = dataFromClient.Split(',');                                
                                SetWifiSetting(CommandCollection);     
                            }
                        }

                        tcpClientsList.RemoveAll(x => x.IMEI == "WebClient");
                    }
                }
                catch (Exception ex)
                {
                    //ConnectedClient = false;
                    if (WriteExceptionLog)
                    {
                        string exception = Environment.NewLine + DateTime.Now.ToString() + ex.ToString();
                        ObjCmnOp.WriteExceptionFile(exception);
                    }
                    //tcpClientsList.RemoveAll(x => x.Connected == false);
                }
            }
        }

        void SetWifiSetting(string[] CommandCollection)
        {
            int IMEIStartPt = 4;
            List<string> IMEILst = new List<string>();
            string CurrentIMEI = "";
           
            int NumberOfIMEI = 0;
            NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);

            int AlphaCounter = Regex.Matches(CommandCollection[IMEIStartPt].ToString(), @"[a-zA-Z]").Count;

            for (int counter = 1; counter <= NumberOfIMEI; counter++)
            {
                string HexIMEI;
                if (AlphaCounter == 0)
                {
                    Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                    HexIMEI = ConvertValueToHexa(IMEIVal.ToString());//String.Format("{0:X}", IMEIVal.ToString());
                    CurrentIMEI = IMEIVal.ToString();
                }
                else
                {
                    byte[] ba = Encoding.Default.GetBytes(CommandCollection[IMEIStartPt]);
                    string hexString = BitConverter.ToString(ba);
                    HexIMEI = hexString.Replace("-", "");
                    CurrentIMEI = CommandCollection[IMEIStartPt];
                }

                IMEILst.Add(HexIMEI);

                bool IsWifiEnabled = Convert.ToBoolean(Convert.ToInt32(CommandCollection[5]));
                List<List<Byte>> ListByt;

                if (IsWifiEnabled)
                {
                    ListByt = ObjTrackerCommands.WIFIHandling(IMEILst, true, CommandCollection[5]);
                    if (ListByt != null)
                    {
                        BroadCastRequest(ListByt, CurrentIMEI);
                    }

                    ListByt = ObjTrackerCommands.WIFISSID(IMEILst, CommandCollection[6]);
                    if (ListByt != null)
                    {
                        BroadCastRequest(ListByt, CurrentIMEI);
                    }

                    string Pwd = CommandCollection[7].ToString().Trim('\0');
                    ListByt = ObjTrackerCommands.WIFIPwd(IMEILst, Pwd);
                    if (ListByt != null)
                    {
                        BroadCastRequest(ListByt, CurrentIMEI);
                    }
                }

                else
                {
                    ListByt = ObjTrackerCommands.WIFIHandling(IMEILst, false, CommandCollection[5]);
                    if (ListByt != null)
                    {
                        BroadCastRequest(ListByt, CurrentIMEI);
                    }
                }               
            }
        }
        

        /// <summary>
        /// Configuration for Single Tracker
        /// </summary>
        /// <param name="CommandCollection"></param>
        void ParsingSettingsCommand(string[] CommandCollection)
        {
            int IMEIStartPt = 4;
            List<string> IMEILst = new List<string>();
            string CurrentIMEI = "";
            string Manufacturer = "";

            int NumberOfIMEI = 0;            
            NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);

            int AlphaCounter = Regex.Matches(CommandCollection[IMEIStartPt].ToString(), @"[a-zA-Z]").Count;

            for (int counter = 1; counter <= NumberOfIMEI; counter++)
            {
                string HexIMEI;
                if (AlphaCounter == 0)
                {
                    Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                    HexIMEI = ConvertValueToHexa(IMEIVal.ToString());//String.Format("{0:X}", IMEIVal.ToString());
                    CurrentIMEI = IMEIVal.ToString();
                }
                else
                {
                    byte[] ba = Encoding.Default.GetBytes(CommandCollection[IMEIStartPt]);
                    string hexString = BitConverter.ToString(ba);
                    HexIMEI = hexString.Replace("-", "");
                    CurrentIMEI = CommandCollection[IMEIStartPt];
                }

                IMEILst.Add(HexIMEI);
            }
            
            List<TrackerAlarmConfig> LstConfig = ObjVehicle.GetAllAlarmsToApply("GetAllAlarmsToApply_ByTID", CurrentIMEI);
            if (LstConfig != null && LstConfig.Count != 0)
            {
                foreach (TrackerAlarmConfig item in LstConfig)
                {
                    if (IMEILst.Count > 1)
                    {
                        foreach (string itm in  IMEILst)
                        {
                            List<string> TempLst = new List<string>();
                            TempLst.Add(itm);
                            SinocastelParamSettings(TempLst, CurrentIMEI, item.AlrmCodes, item.IsEnabled, item.ThresholdValues.ToString());
                        }
                    }
                    else
                    {

                        List<ClientInfo> TrakrToConfig = tcpClientsList.Where(x => x.IMEI == CurrentIMEI).ToList();// IMEILst[0]
                        if (TrakrToConfig.Count > 0)
                        {
                            if (TrakrToConfig[0].Manufacturer == TrackerManufacturer.Sinocastel)
                            {
                                SinocastelParamSettings(IMEILst, CurrentIMEI, item.AlrmCodes, item.IsEnabled, item.ThresholdValues.ToString());
                            }
                            else
                            {
                                IMEILst.Clear();
                                IMEILst.Add(CurrentIMEI);
                                TopTenConfiguration(IMEILst, CurrentIMEI, item.AlrmCodes, item.IsEnabled, item.ThresholdValues.ToString());
                            }
                        }                    
                    }                    
                    Thread.Sleep(200);
                }
            }           
        }

        /// <summary>
        /// Configuration for Multiple Trackers
        /// </summary>
        /// <param name="CommandCollection"></param>
        void ParsingSettingsCommandForMultipleTrackers(string[] CommandCollection)
        {
            int IMEIStartPt = 4;
            List<string> IMEILst = new List<string>();
            string CurrentIMEI = "";

            int NumberOfIMEI = 0;
            NumberOfIMEI = Convert.ToInt32(CommandCollection[3]);

            int AlphaCounter = Regex.Matches(CommandCollection[IMEIStartPt].ToString(), @"[a-zA-Z]").Count;

            for (int counter = 1; counter <= NumberOfIMEI; counter++)
            {
                string HexIMEI;
                if (AlphaCounter == 0)
                {
                    Int64 IMEIVal = Convert.ToInt64(CommandCollection[IMEIStartPt]);
                    HexIMEI = ConvertValueToHexa(IMEIVal.ToString());//String.Format("{0:X}", IMEIVal.ToString());
                    CurrentIMEI = IMEIVal.ToString();
                }
                else
                {
                    byte[] ba = Encoding.Default.GetBytes(CommandCollection[IMEIStartPt]);
                    string hexString = BitConverter.ToString(ba);
                    HexIMEI = hexString.Replace("-", "");
                    CurrentIMEI = CommandCollection[IMEIStartPt];
                }

                IMEILst.Add(HexIMEI);
            }

            foreach (string ImeiItm in IMEILst)
            {
                CurrentIMEI = CommandCollection[IMEIStartPt];
                IMEIStartPt = IMEIStartPt + 1;

                List<TrackerAlarmConfig> LstConfig = ObjVehicle.GetAllAlarmsToApply("GetAllAlarmsToApply_ByTID", CurrentIMEI);
                if (LstConfig != null && LstConfig.Count != 0)
                {
                    foreach (TrackerAlarmConfig item in LstConfig)
                    {
                        if (IMEILst.Count > 1)
                        {                            
                            List<string> TempLst = new List<string>();
                            TempLst.Add(ImeiItm);
                            SinocastelParamSettings(TempLst, CurrentIMEI, item.AlrmCodes, item.IsEnabled, item.ThresholdValues.ToString());                            
                        }
                        else
                        {
                            SinocastelParamSettings(IMEILst, CurrentIMEI, item.AlrmCodes, item.IsEnabled, item.ThresholdValues.ToString());
                        }
                        Thread.Sleep(200);
                    }
                }
            }
        }


        private string ConvertValueToHexa(string text)
        {            
            char[] chars = text.ToCharArray();
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in chars)
            {
                stringBuilder.Append(((Int16)c).ToString("x"));
            }

            String textAsHex = stringBuilder.ToString();
            return textAsHex;
        }

        public void TopTenConfiguration(List<string> IMEILst, string CurrentIMEI, string Alarm, bool State, string ThresholdVal = "")
        {
            List<List<Byte>> ListByt;
            string Filter = "UpdateConfig_ByTID";
            switch (Alarm)
            {
                case "Spd":
                    if ((!State) && (ThresholdVal == "0"))
                    {
                        ThresholdVal = "120";

                    }
                    ListByt = HandleSpeedLimit(IMEILst, Convert.ToInt16(ThresholdVal), CurrentIMEI);

                    if (ListByt != null)
                    {
                        bool Status = BroadCastRequest(ListByt, CurrentIMEI);
                    }
                    break;
            }
        }

        void SinocastelParamSettings(List<string> IMEILst,string CurrentIMEI,string Alarm, bool State, string ThresholdVal= "")
        {
            List<List<Byte>> ListByt;
            string Filter = "UpdateConfig_ByTID";

            switch (Alarm)
            {
                case "Spd":
                    if ((!State) && (ThresholdVal == "0"))
                    {
                        ThresholdVal = "120";

                    }
                     ListByt = ObjTrackerCommands.SetSpeedingAlarm(IMEILst, State, ThresholdVal);

                    if (ListByt != null)
                    {
                        bool Status = BroadCastRequest(ListByt, CurrentIMEI);                       
                    }
                    break;

                case "HrdAcc":
                    if ((!State) && (ThresholdVal == "0"))
                    {
                        ThresholdVal = "0.4";

                    }

                    ListByt = ObjTrackerCommands.HardAcceleration(IMEILst, State, ThresholdVal);

                    if (ListByt != null)
                    {
                        bool Status = BroadCastRequest(ListByt, CurrentIMEI);                      
                    }
                    break;

                case "EngnTmp":
                    if ((!State) && (ThresholdVal == "0"))
                    {
                        ThresholdVal = "98";

                    }
                    ListByt =ObjTrackerCommands.EngineCoolantTemperature(IMEILst, State, ThresholdVal);

                    if (ListByt != null)
                    {
                        bool Status = BroadCastRequest(ListByt, CurrentIMEI);                       
                    }
                    break;

                case "HghRPM":
                    if ((!State) && (ThresholdVal == "0"))
                    {
                        ThresholdVal = "4500";

                    }
                    ListByt = ObjTrackerCommands.HighEngineRPM(IMEILst, State, ThresholdVal);

                    if (ListByt != null)
                    {
                        bool Status = BroadCastRequest(ListByt, CurrentIMEI);                    
                    }
                    break;

                case "IdlEngn":
                    if ((!State) && (ThresholdVal == "0"))
                    {
                        ThresholdVal = "15";

                    }

                    ListByt = ObjTrackerCommands.GeneralSettingsWithoutDecimalValues(IMEILst, "IdlEngn", State, ThresholdVal);

                    if (ListByt != null)
                    {
                        bool Status = BroadCastRequest(ListByt, CurrentIMEI);
                    }
                    break;

                case "LwVltg":
                    if ((!State) && (ThresholdVal == "0"))
                    {
                        ThresholdVal = "10.5";

                    }
                    ListByt = ObjTrackerCommands.GeneralSettingsWithoutDecimalValues(IMEILst, AlarmCodes.LwVltg.ToString(), State, ThresholdVal);

                    if (ListByt != null)
                    {
                        bool Status = BroadCastRequest(ListByt, CurrentIMEI);                    
                    }
                    break;
                case "FatgDrv":
                    if ((!State) && (ThresholdVal == "0"))
                    {
                        ThresholdVal = "240";

                    }
                    ListByt = ObjTrackerCommands.GeneralSettingsWithoutDecimalValues(IMEILst, AlarmCodes.FatgDrv.ToString(), State, ThresholdVal);

                    if (ListByt != null)
                    {
                        bool Status = BroadCastRequest(ListByt, CurrentIMEI);                                           
                    }
                    break;
                case "QkLnChg":
                    if ((!State) && (ThresholdVal == "0"))
                    {
                        ThresholdVal = "0.4";

                    }
                    ListByt = ObjTrackerCommands.GeneralSettingsWithoutDecimalValues(IMEILst, AlarmCodes.QkLnChg.ToString(), State, ThresholdVal);

                    if (ListByt != null)
                    {
                        bool Status = BroadCastRequest(ListByt, CurrentIMEI);
                    }
                    break;
                case "ShrpTrn":
                    if ((!State) && (ThresholdVal == "0"))
                    {
                        ThresholdVal = "0.5";

                    }
                    ListByt = ObjTrackerCommands.GeneralSettingsWithoutDecimalValues(IMEILst, AlarmCodes.ShrpTrn.ToString(), State, ThresholdVal);

                    if (ListByt != null)
                    {
                        bool Status = BroadCastRequest(ListByt, CurrentIMEI);                      
                    }
                    break;
                case "Crsh":
                    if ((!State) && (ThresholdVal == "0"))
                    {
                        ThresholdVal = "1.5";

                    }
                    ListByt = ObjTrackerCommands.GeneralSettingsWithoutDecimalValues(IMEILst, AlarmCodes.Crsh.ToString(), State, ThresholdVal);

                    if (ListByt != null)
                    {
                        bool Status = BroadCastRequest(ListByt, CurrentIMEI);
                       
                    }
                    break;
                case "MIL":
                    ListByt = ObjTrackerCommands.TrackerMILSettings(IMEILst, State, ThresholdVal);
                    if (ListByt != null)
                    {
                        bool Status = BroadCastRequest(ListByt, CurrentIMEI);

                        //if (Status)
                        //{
                        //    ObjVehicle.UpdateAlarmsSettings_ByTID(Filter, CurrentIMEI, Alarm);
                        //}
                    }
                    break;
            }
        }
        
        private void LogOutTracker(string DetachedIMEI)
        {
            try
            {
                List<ClientInfo> FilteredIMEILst=tcpClientsList.Where(x => x.IMEI == DetachedIMEI).ToList();                
                foreach (ClientInfo clnt in FilteredIMEILst)
                {
                    clnt.client.GetStream().Close();
                    clnt.client.Close();
                    tcpClientsList.RemoveAll(x => x.IMEI == clnt.IMEI);
                }               
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + ex.ToString();
                    ObjCmnOp.WriteExceptionFile("Method Name: LogOutTracker " + exception);
                }
            }
        }

        /// <summary>
        /// Method to Listen client messaege 
        /// </summary>
        /// <param name="client"> client thats is sending data to service</param>
        private void HandleClient(TcpClient client)
        {
            string Hexa = "";

            try
            {
                NetworkStream networkStream = client.GetStream();
                byte[] bytesFrom = new byte[250];
                string ClientSideTrackerID = "";
                //bool ConnectedClient = false;
                string TrackerHexaResponse = "";
                string dataFromClient = "";
                string data = "";

                //GpsDataPacketTimer = new System.Timers.Timer();
                //GpsDataPacketTimer.Interval = TrackOnDemandPktTimeSpan;   // every 10 sec
                //GpsDataPacketTimer.Elapsed += (sender, e) => GpsDataPacket_Elapsed(sender, e, networkStream, bytesFrom);
                //GpsDataPacketTimer.AutoReset = false;  // makes it fire only on



                //GpsDataPacketTimer = new System.Timers.Timer();
                //GpsDataPacketTimer.Interval = TrackOnDemandPktTimeSpan;   // every 10 sec
                //GpsDataPacketTimer.Elapsed += (sender, e) => SetGprsTimeInterval(sender, e, networkStream, bytesFrom, client);
                //GpsDataPacketTimer.AutoReset = false;  // makes it fire only once
                //ConnectedClient = client.Connected;



                while (client.Connected)
                {

                    if (networkStream.DataAvailable)
                    {
                        data = "";
                        try
                        {
                            int test = client.ReceiveBufferSize;
                            TrackerIDCollection = TrackerId.Split(',').ToArray().ToList();
                            networkStream.Read(bytesFrom, 0, 1000);
                            ClientSideTrackerID = getTrackerID(bytesFrom);

                            if (TrackerIDCollection.Contains(ClientSideTrackerID))
                            {

                                string datastr = Environment.NewLine + "Before Actual Length: " + ": ";
                                File.AppendAllText(FilePath, datastr);
                                int ActualLength = GetActualLength(bytesFrom);

                                if (ActualLength > 0)
                                {
                                    TrackerHexaResponse = BitConverter.ToString(bytesFrom, 0, ActualLength).Replace("-", " ");
                                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                                    string[] HexCollection = TrackerHexaResponse.Split(' ');

                                    if (HexCollection.Length > 12)
                                    {
                                        if (HexCollection[3] == "11" && HexCollection[11] == "50" && HexCollection[12] == "00")
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " Client: Login ";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine + "TrackerId: " + ": " + ClientSideTrackerID;

                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);

                                             ParseLoginRequestAndConfirm(bytesFrom, TrackerHexaResponse, networkStream);

                                        }

                                        if (HexCollection[11] == "99" && HexCollection[12] == "55")
                                        {
                                            data = "";
                                            data = Environment.NewLine + DateTime.Now.ToString() + " Client: Standard GPS Packet";
                                            data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                            data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                            data = data + Environment.NewLine;
                                            ObjCmnOp.WriteFile(data);

                                            //ParseGpsTrackerPacket(bytesFrom, ActualLength, ClientSideTrackerID, TrackerHexaResponse);
                                        }

                                      

                                        if (HexCollection[11] == "41" && HexCollection[12] == "02")
                                        {
                                            if (HexCollection[13] == "00")
                                            {
                                                data = "";
                                                data = Environment.NewLine + DateTime.Now.ToString() + " Client: Gps TimeSetting Response";
                                                data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                                data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                                data = data + Environment.NewLine + "Time Set " + ": " + "Failure";
                                                data = data + Environment.NewLine;
                                                ObjCmnOp.WriteFile(data);
                                            }

                                            else if (HexCollection[13] == "01")
                                            {
                                                data = "";
                                                data = Environment.NewLine + DateTime.Now.ToString() + " Client: Gps TimeSetting Response";
                                                data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                                data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                                data = data + Environment.NewLine + "Time Set " + ": " + "Success";
                                                data = data + Environment.NewLine;
                                                ObjCmnOp.WriteFile(data);

                                            }
                                        }

                                        if (HexCollection[11] == "51" && HexCollection[12] == "00")
                                        {
                                            if (HexCollection[13] == "00")
                                            {
                                                data = "";
                                                data = Environment.NewLine + DateTime.Now.ToString() + " Client: Gps TimeSetting Response";
                                                data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                                data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                                data = data + Environment.NewLine + "Time Set " + ": " + "Failure";
                                                data = data + Environment.NewLine;
                                                ObjCmnOp.WriteFile(data);
                                            }

                                            else if (HexCollection[13] == "01")
                                            {
                                                data = "";
                                                data = Environment.NewLine + DateTime.Now.ToString() + " Client: Gps TimeSetting Response";
                                                data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaResponse;
                                                data = data + Environment.NewLine + "Ascii: " + ": " + dataFromClient.Trim('\0');
                                                data = data + Environment.NewLine + "Time Set " + ": " + "Success";
                                                data = data + Environment.NewLine;
                                                ObjCmnOp.WriteFile(data);

                                            }
                                        }

                                        if (GpsDataPacketTaskStatus)
                                        {
                                            // GpsDataPacketTimer.Start();
                                        }
                                        else
                                        {
                                            //GpsDataPacketTimer.Stop();
                                        }
                                    }
                                }
                            }

                            else

                            {
                                //ConnectedClient = false;
                                //tcpClientsList.RemoveAll(x => x.Connected == false);
                            }
                        }
                        catch (Exception ex)
                        {
                            //ConnectedClient = false;
                            if (WriteExceptionLog)
                            {
                                string exception = Environment.NewLine + DateTime.Now.ToString() + ex.ToString();
                                ObjCmnOp.WriteExceptionFile(exception);
                            }
                            //tcpClientsList.RemoveAll(x => x.Connected == false);
                        }
                    }
                    //else
                    //{
                    //    try
                    //    {
                    //        List<Byte> ByteList = new List<byte>();
                    //        ByteList.Add(0x40);
                    //        ByteList.Add(0x40);

                    //        networkStream.Write(ByteList.ToArray(), 0, ByteList.Count - 1);
                    //        networkStream.Flush();
                    //    }

                    //    catch (Exception ex)
                    //    {
                    //        ConnectedClient = false;
                    //        string exception = Environment.NewLine + ex.Message.ToString();
                    //        ObjCmnOp.WriteExceptionFile(exception)
                    //        tcpClientsList.RemoveAll(x => x.Connected == false);
                    //    }

                    //}
                }
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
            }
        }

        /// <summary>
        /// Get Tracker ID from Client Stream
        /// </summary>
        /// <param name="RecievedData">received Byte Array from client</param>
        /// <returns></returns>
        private string getTrackerID(byte[] RecievedData)
        {
            string TrackerHexaResponse = BitConverter.ToString(RecievedData, 4, 7).Replace("-", "");
            return TrackerHexaResponse;
        }
        /// <summary>
        /// Method to get the start point of extra bytes
        /// </summary>
        /// <param name="bytesFrom"></param>
        /// <returns></returns>
        private int GetActualLengthOfBytes(byte[] bytesFrom)
        {

            try
            {
                Boolean ExtraBytes = false;
                int StartOfExtraBytes = 0;
                for (int counter = 0; counter < bytesFrom.Length; counter++)
                {
                    if (bytesFrom[counter] == 0)
                    {
                        ExtraBytes = true;

                        if (StartOfExtraBytes < 1)
                        {
                            StartOfExtraBytes = counter;
                        }

                    }
                    else
                    {
                        ExtraBytes = false;
                        StartOfExtraBytes = 0;
                    }
                }

                return StartOfExtraBytes;
            }
            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + "GetActualLengthOfBytes : " + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
                return 0;
            }
        }

        private int GetActualLength(byte[] bytesFrom)
        {
            try
            {
                string ActualLengthOfStream = BitConverter.ToString(bytesFrom, 2, 2).Replace("-", "");
                string FourthByte = ActualLengthOfStream.Substring(2, 2);

                if (FourthByte.Equals("00"))
                {
                    int decval = System.Convert.ToInt32(ActualLengthOfStream.Substring(0,2), 16);
                    return decval;
                }
                else
                {
                    int decval = System.Convert.ToInt32(ActualLengthOfStream, 16);
                    return decval;
                }
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + "GetActualLength : " + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
                return 0;
            }
        }

        public bool IsGPSOfflinePkt(byte[] bytesFromClient, int PktLength)
        {
            try
            {
                string[] bytesparsed = (BitConverter.ToString(bytesFromClient, 0, bytesFromClient.Length).Replace("-", " ")).Split(' ');

                if (bytesparsed[PktLength] == "24" && bytesparsed[PktLength + 1] == "24")
                    return true;
                else
                    return false;
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + "GetActualLength : " + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
                return false;
            }
        }
        /// <summary>
        /// Create a respnse message for Login Confirmation
        /// </summary>
        /// <param name="TID">Byte array conatins all data passed from tranmitter</param>
        /// <returns></returns>
        private Byte[] CreateResponseMessageToTracker(byte[] TID)
        {
            //40 40 00 12 58 04 20 80 04 57 92 40 00 01 23 6F 0D 0A

            List<Byte> ByteList = new List<byte>();
            ByteList.Add(0x40);
            ByteList.Add(0x40);
            ByteList.Add(0x00);
            ByteList.Add(0x12);

            //for (int counter = 4; counter < 11; counter++)
            //{
            //    ByteList.Add(TID[counter]);
            //}
            // 64 50 70 32 77 01 65
            ByteList.Add(0x64);
            ByteList.Add(0x50);
            ByteList.Add(0x70);
            ByteList.Add(0x32);
            ByteList.Add(0x77);
            ByteList.Add(0x01);
            ByteList.Add(0x65);

            ByteList.Add(0x40);
            ByteList.Add(0x00);

            //Login Success
            ByteList.Add(0x01);
            //CheckSum Values
            ByteList.Add(0x23);
            ByteList.Add(0x6f);

            //'\r\n'

            ByteList.Add(0x0D);
            ByteList.Add(0x0A);
            return ByteList.ToArray();
        }



        //private void GpsDataPacket_Elapsed(object sender, System.Timers.ElapsedEventArgs e, NetworkStream networkStream, byte[] bytesFrom, TcpClient client)
        //{
        //    try
        //    {
        //        if (client.Connected)
        //        {
        //            Byte[] ResponseArray = CreateGpsPacketTracker(bytesFrom);
        //            //Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
        //            //networkStream.Write(sendBytes, 0, sendBytes.Length);
        //            networkStream.Write(ResponseArray, 0, ResponseArray.Length);
        //            networkStream.Flush();
        //            GpsDataPacketTaskStatus = false;

        //            string ServerHexaResponse = BitConverter.ToString(ResponseArray, 0, ResponseArray.Length).Replace("-", " ");
        //            string data = Environment.NewLine + DateTime.Now.ToString() + " Server: Tracker on demand Request ";
        //            data = data + Environment.NewLine + "Hexa: " + ": " + ServerHexaResponse;
        //            data = data + Environment.NewLine;
        //            ObjCmnOp.WriteFile(data)
        //        }
        //        else
        //        {
        //            GpsDataPacketTaskStatus = false;
        //            GpsDataPacketTimer.Stop();
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        GpsDataPacketTaskStatus = false;
        //        GpsDataPacketTimer.Stop();
        //    }

        //    finally
        //    {
        //        if (GpsDataPacketTaskStatus)
        //        {
        //            GpsDataPacketTimer.Start();
        //        }
        //    }
        //}

       
        //private void SetGprsTimeInterval(object sender, System.Timers.ElapsedEventArgs e, NetworkStream networkStream, byte[] bytesFrom, TcpClient client)
        //{
        //    try
        //    {
        //        if (client.Connected)
        //        {
        //           // Byte[] ResponseArray = SetGprsTimeIntervalPacket(bytesFrom);
        //            Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
        //            networkStream.Write(sendBytes, 0, sendBytes.Length);
        //            networkStream.Write(ResponseArray, 0, ResponseArray.Length);
        //            networkStream.Flush();
        //            GpsDataPacketTaskStatus = true;

        //            string ServerHexaResponse = BitConverter.ToString(ResponseArray, 0, ResponseArray.Length).Replace("-", " ");
        //            string data = Environment.NewLine + DateTime.Now.ToString() + " Server: Set GPRS Time Interval ";
        //            data = data + Environment.NewLine + "Hexa: " + ": " + ServerHexaResponse;
        //            data = data + Environment.NewLine;
        //            ObjCmnOp.WriteFile(data)
        //            GpsDataPacketTaskStatus = false;
        //            GpsDataPacketTimer.Stop();
        //        }
        //        else
        //        {
        //            GpsDataPacketTaskStatus = false;
        //            GpsDataPacketTimer.Stop();
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        GpsDataPacketTaskStatus = false;
        //        GpsDataPacketTimer.Stop();
        //    }

        //    finally
        //    {
        //        if (GpsDataPacketTaskStatus)
        //        {
        //            GpsDataPacketTimer.Start();
        //        }
        //    }
        //}

        private void ParseLoginRequestAndConfirm(byte[] ReceivedDtaPkt, string TrackerHexaResponse, NetworkStream stream)
        {
            //24 24 00 11 58 04 20 80 04 57 92 50   00 EA   9F    0D    0A
            //0   1 2   3  4  5  6  7 8   9 10  11  12  13   14   15     16

            string[] HexCollection = TrackerHexaResponse.Split(' ');
            Byte[] ResponseStream;
            string ServerHexaResponse = "";
            string data = "";

            //Login Request is 0x5000 at the position of 11 & 12

            ResponseStream = CreateLoginConfirmation(ReceivedDtaPkt);
            stream.Write(ResponseStream, 0, ResponseStream.Length);
            stream.Flush();

            ServerHexaResponse = BitConverter.ToString(ResponseStream, 0, ResponseStream.Length).Replace("-", " ");
            data = Environment.NewLine + DateTime.Now.ToString() + " Server: Confirmation ";
            data = data + Environment.NewLine + "Hexa: " + ": " + ServerHexaResponse;
            data = data + Environment.NewLine + "TrackerId: " + ": " + TrackerId;
            data = data + Environment.NewLine;
            ObjCmnOp.WriteFile(data);
        }

        private Byte[] CreateLoginConfirmation(byte[] TID)
        {
            //40 40 00 12 58 04 20 80 04 57 92 40 00 01 23 6F 0D 0A
            //Login Confirmation Command is 0x4000 and On Login Success the flag is 01
            List<Byte> ByteList = new List<byte>();
            ByteList.Add(0x40);
            ByteList.Add(0x40);
            ByteList.Add(0x00);
            ByteList.Add(0x12);

            for (int counter = 4; counter < 11; counter++)
            {
                ByteList.Add(TID[counter]);
            }

            ByteList.Add(0x40);
            ByteList.Add(0x00);

            //Login Success
            ByteList.Add(0x01);
            //CheckSum Values
            ByteList.Add(0x23);
            ByteList.Add(0x6f);

            //'\r\n'

            ByteList.Add(0x0D);
            ByteList.Add(0x0A);
            return ByteList.ToArray();
        }

        private Byte[] CreateGpsPacketTrackerRequest(byte[] TID)
        {
            //40 40 00 12 58 04 20 80 04 57 92 40 00 01 23 6F 0D 0A

            // 40 40 00 11 58 04 20 80 04 57 92 41 01 CE BC 0D 0A

            List<Byte> ByteList = new List<byte>();
            ByteList.Add(0x40);
            ByteList.Add(0x40);
            ByteList.Add(0x00);
            ByteList.Add(0x11);

            for (int counter = 4; counter < 11; counter++)
            {
                ByteList.Add(TID[counter]);
            }

            ByteList.Add(0x41);
            ByteList.Add(0x01);

            //CheckSum Values
            ByteList.Add(0xCE);
            ByteList.Add(0xBC);

            //'\r\n'

            ByteList.Add(0x0D);
            ByteList.Add(0x0A);
            return ByteList.ToArray();
        }

        async void ParseGpsTrackerPacket(ClientInfo tpclient,Byte[] bytesFrom, int ActualDataLengthSize, String pClientSideTrackerID, string HexaStream,bool offlinePacket= false,DataPacket DataPktobj= null,bool IsAlarmPkt= false)
        {
            //24 24 00 75 58 04 20 80 04 57 92 99 55 30 35 35 30 35 38 2E 30 30 30 2C 41 2C 32 33
            //30 38 2E 34 33 39 30 36 2C 4E 2C 31 31 33 32 32 2E 37 37 35 37 35 2C 45 2C 30 2E 33
            //2C 30 2E 30 2C 31 37 30 35 31 37 2C 2C 2A 32 42 7C 31 2E 32 36 7C 34 2E 33 7C 30 30
            //30 31 7C 30 32 43 43 2C 30 38 36 44 7C 30 30 30 30 32 33 35 35 33 7C 30 30 30 30 30
            //30 F6 F9 0D 0A
            try
            {
                int DataLengthSize = ActualDataLengthSize;
                int TotalCharToRead = 0;
                int BytesToSkip = 14;

                TrackerCommands objTrackerCmds = new TrackerCommands(ExceptionLogPath, FilePath, SignalRApi, ref tcpClientsList, WriteLog, WriteExceptionLog, IMEICollect, ConnectionString, CheckIndividualTrackerFromDb, IdleTime);
                
                if (!IsAlarmPkt)
                {
                    DataPktobj = new DataPacket();
                    BytesToSkip = 13;
                }

                TotalCharToRead = (DataLengthSize - 4) - BytesToSkip;

                string dataFromTracker = System.Text.Encoding.ASCII.GetString(bytesFrom, BytesToSkip, TotalCharToRead);
                String[] MainGPRSTrackerPkt = dataFromTracker.Split('|');
                String[] LatLongPkt = MainGPRSTrackerPkt[0].Split(',');

                if (LatLongPkt.Length >= 8)
                {
                    GetUTCDateTime(ref DataPktobj, LatLongPkt);
                    GprsSignalValidOrInvalid(ref DataPktobj, LatLongPkt[1].ToString());
                    //LatitudeParser(ref DataPktobj, LatLongPkt[2]);
                    TestLatitudeParser(ref DataPktobj, LatLongPkt[2]);
                    LatitudeDirection(ref DataPktobj, LatLongPkt[3]);
                    //LongitudeParser(ref DataPktobj, LatLongPkt[4]);
                    TestLongitudeParser(ref DataPktobj, LatLongPkt[4]);
                    LongitudeDirection(ref DataPktobj, LatLongPkt[5]);
                    ParseSpeed(ref DataPktobj, LatLongPkt[6]);

                    objTrackerCmds.TripHandling(ref DataPktobj, ref tpclient, false);
                    //(DataPktobj.Speed);

                    ParseInHeadingDirectionInDegrees(ref DataPktobj, LatLongPkt[7]);
                    DataPktobj.Rotation = DegreesToCardinal(DataPktobj.Direction);
                }

                if (MainGPRSTrackerPkt.Length <= 7)
                {
                    ParseAltitude(ref DataPktobj, MainGPRSTrackerPkt[2]);
                    ParseOdoMeter(ref DataPktobj, MainGPRSTrackerPkt[5]);
                    ParseStatusOfInputsAndOutputs(ref DataPktobj, MainGPRSTrackerPkt[3]);
                }

                DataPktobj.TrackerID = pClientSideTrackerID;
                DataPktobj.TrackerStream = HexaStream;
                DataPktobj.IsOfflineData = offlinePacket;

                string Data = "TrackerID : " + DataPktobj.TrackerID.ToString();
                Data = Data + Environment.NewLine + "UTC_Date_Time : " + DataPktobj.UTCTimeAndDate.ToString();
                Data = Data + Environment.NewLine + "Latitude : " + DataPktobj.Latitude.ToString();
                Data = Data + Environment.NewLine + "Longitude : " + DataPktobj.Longitude.ToString();
                Data = Data + Environment.NewLine + "Speed km/h : " + DataPktobj.Speed.ToString();
                Data = Data + Environment.NewLine + "Gps Signal Status : " + DataPktobj.GPSSignalStatus.ToString();
                Data = Data + Environment.NewLine + "Direction : " + DataPktobj.Direction.ToString();
                Data = Data + Environment.NewLine + "Rotation : " + DataPktobj.Rotation.ToString();
                Data = Data + Environment.NewLine + "Altitude : " + DataPktobj.Altitude.ToString();
                Data = Data + Environment.NewLine + "Odometer : " + DataPktobj.Odometer.ToString();
                Data = Data + Environment.NewLine + "OfflinePacket : " + DataPktobj.IsOfflineData.ToString();
                Data = Data + Environment.NewLine;
                ObjCmnOp.WriteFile(Data);

                /* == Populate Vehicle Object == */

                //List<DataPacket> vObject = new List<DataPacket>();

                //for (int x = 0; x < 1000; x++)
                //{
                //    DataPacket tem_object = new DataPacket();
                //    string temp_lat = "31.478" + x;
                //    string temp_long = "74.281" + x;
                //    tem_object.Latitude = Double.Parse(temp_lat);
                //    tem_object.Longitude = Double.Parse(temp_long);
                //    vObject.Add(tem_object);

                //    Thread.Sleep(2000);
                //}

                if (tpclient.objTrackerTrip == null)
                {
                    SetTripValuesInClient(DataPktobj, tpclient, false);
                }
                else if (tpclient.objTrackerTrip.TripStartTime != DataPktobj.UTCTimeAndDate)
                {
                    SetTripValuesInClient(DataPktobj, tpclient, false);
                }
                

                using (var clientObj = new HttpClient())
                {
                    clientObj.BaseAddress = new Uri(SignalRApi.ToString());
                    clientObj.DefaultRequestHeaders.Accept.Clear();
                    clientObj.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await clientObj.PostAsJsonAsync("/swi/FleetAPI/post", DataPktobj);
                    if (response.IsSuccessStatusCode)
                    {
                        Uri ncrUrl = response.Headers.Location;
                    }
                }
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + "ParseGpsTrackerPacket" + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
            }
        }     
      

        private void SetTripValuesInClient(DataPacket DtaPkt, ClientInfo tpclient, bool IsloginCommand)
        {
            lock (LockThread)
            {

                TimeSpan DrvngTime;                
                if (tpclient.objTrackerTrip == null||tpclient.objTrackerTrip.PktNo == 0)
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
                    tpclient.objTrackerTrip.TripStartTime = DtaPkt.UTCTimeAndDate;
                    tpclient.objTrackerTrip.NoOfDec = 0;
                    DtaPkt.objTrackerTrip = tpclient.objTrackerTrip;
                    //tpclient.objTrackerTrp.TripStartTime = DtaPkt.UTCTimeAndDate;
                }
                else
                {
                    DrvngTime = DtaPkt.UTCTimeAndDate.Subtract(tpclient.objTrackerTrip.TripStartTime);
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
                        tpclient.objTrackerTrip.MaxRotation = tpclient.objTrackerTrip.CurrentRotation;
                    }

                    if (DtaPkt.LstAlarms != null && DtaPkt.LstAlarms.Count > 0)
                    {
                        if (DtaPkt.LstAlarms.Where(x => x.AlrmCodes == AlarmCodes.HrdAcc && x.Status == true).ToList().Count > 0)
                        {
                            tpclient.objTrackerTrip.NoOfAcc = tpclient.objTrackerTrip.NoOfAcc + 1;
                        }

                        tpclient.objTrackerTrip.AvgSpeed = Convert.ToSingle(tpclient.objTrackerTrip.TotalSpeed / tpclient.objTrackerTrip.PktNo);
                        DtaPkt.objTrackerTrip = tpclient.objTrackerTrip;
                    }
                }               
            }
        }

        
        private void GetUTCDateTime(ref DataPacket pDataPktobj, String[] pLatLongPkt)
        {
            string Time = pLatLongPkt[0];
            TimeSpan TimeSpn = ParseTime(Time);

            string Dte = pLatLongPkt[8];
            DateTime Dtetime = ParseDate(Dte);
            //
            pDataPktobj.UTCTimeAndDate = Dtetime.Date + TimeSpn;
        }

        private TimeSpan ParseTime(string pTime)
        {
            string Hour = "";
            string Minutes = "";
            string Seconds = "";
            int Counter = 0;
            foreach (char chr in pTime)
            {

                Counter = Counter + 1;

                if (Counter <= 2)
                {
                    Hour = Hour + chr;
                }
                else if (Counter > 2 && Counter <= 4)
                {
                    Minutes = Minutes + chr;
                }
                else if (Counter > 4 && Counter <= pTime.Length - 1)
                {
                    Seconds = Seconds + chr;
                }
            }

            double Secnds = Convert.ToDouble(Seconds);
            return new TimeSpan(Convert.ToInt32(Hour), Convert.ToInt32(Minutes), Convert.ToInt32(Secnds));

        }

        private DateTime ParseDate(string pDte)
        {
            string dd = "";
            string mm = "";
            string yy = "";
            int Counter = 0;

            foreach (char chr in pDte)
            {
                Counter = Counter + 1;

                if (Counter <= 2)
                {
                    dd = dd + chr;
                }
                else if (Counter > 2 && Counter <= 4)
                {
                    mm = mm + chr;
                }
                else if (Counter > 4 && Counter <= pDte.Length)
                {
                    yy = yy + chr;
                }

            }
            yy = "20" + yy;
            return new DateTime(Convert.ToInt32(yy), Convert.ToInt32(mm), Convert.ToInt32(dd));
        }   

        private void TestLatitudeParser(ref DataPacket pDataPktobj, string pLatitude)
        {
            //xxmm.dddd Latitude xx = degrees; mm = minutes; dddd = decimal part of minutes
            //22 deg. 32.6083 min.
            //2232.6083

            String Degrees = "";
            String Minutes = "";
            String Seconds = "";

            String[] LatCollection = pLatitude.Split('.');
            int Counter = 0;

            foreach (char chr in LatCollection[0])
            {

                Counter = Counter + 1;

                if (Counter <= 2)
                {
                    Degrees = Degrees + chr;
                }
                else if (Counter > 2 && Counter <= 4)
                {
                    Minutes = Minutes + chr;
                }
            }

            Seconds = "." + LatCollection[1];
            Minutes = Minutes + Seconds;
            Double LatInDegress = TestConvertDegreeAngleToDouble(Convert.ToDouble(Degrees), Convert.ToDouble(Minutes));
            pDataPktobj.Latitude = LatInDegress;
        }

        private double TestConvertDegreeAngleToDouble(double degrees, double minutes)
        {
            return degrees + (minutes / 60);
        }

        private void TestLongitudeParser(ref DataPacket pDataPktobj, string pLongitude)
        {
            //yyymm.dddd 
            String Degrees = "";
            String Minutes = "";
            String Seconds = "";

            String[] LatCollection = pLongitude.Split('.');
            int Counter = 0;

            foreach (char chr in LatCollection[0])
            {

                Counter = Counter + 1;

                if (Counter <= 3)
                {
                    Degrees = Degrees + chr;
                }
                else if (Counter > 3 && Counter <= 5)
                {
                    Minutes = Minutes + chr;
                }
            }

            Seconds = "." + LatCollection[1];
            Minutes = Minutes + Seconds;
            Double LongInDegress = TestConvertDegreeAngleToDouble(Convert.ToDouble(Degrees), Convert.ToDouble(Minutes));
            pDataPktobj.Longitude = LongInDegress;

        }

        private void LatitudeDirection(ref DataPacket pDataPktobj, string Direction)
        {   // N|S
            if (Direction.Equals("N"))
            {
                pDataPktobj.LatitudeDirection = "N";
                pDataPktobj.Latitude = pDataPktobj.Latitude * 1;
            }
            else
            {
                pDataPktobj.LatitudeDirection = "S";
                pDataPktobj.Latitude = pDataPktobj.Latitude * (-1);
            }
        }        

        private void LongitudeDirection(ref DataPacket pDataPktobj, string Direction)
        {   // N|S
            if (Direction.Equals("E"))
            {
                pDataPktobj.LongitudeDirection = "E";
                pDataPktobj.Longitude = (pDataPktobj.Longitude * 1);
            }
            else
            {
                pDataPktobj.LongitudeDirection = "W";
                pDataPktobj.Longitude = (pDataPktobj.Longitude * (-1));
            }
        }

        private void GprsSignalValidOrInvalid(ref DataPacket pDataPktobj, string Signal)
        {
            // A = valid, V = invalid
            if (Signal.Equals("A"))
            {
                pDataPktobj.GPSSignalStatus = "Valid";
            }
            else
            {
                pDataPktobj.GPSSignalStatus = "Invalid";
            }
        }

        private void ParseSpeed(ref DataPacket pDataPktobj, string pSpeed)
        {
            pDataPktobj.Speed = (Convert.ToDouble(pSpeed) * 1.852);
        }

        private void ParseInHeadingDirectionInDegrees(ref DataPacket pDataPktobj, string pDirectionInDegrees)
        {
            pDataPktobj.Direction = Convert.ToDouble(pDirectionInDegrees);
        }

        private double ConvertDegreeAngleToDouble(double degrees, double minutes, double seconds)
        {
            return degrees + (minutes / 60) + (seconds / 3600);
        }

        private void ParseOdoMeter(ref DataPacket pDataPktObj, string pOdometer)
        {
            string Odometer = "";

            foreach (char ch in pOdometer)
            {
                if (Char.IsNumber(ch))
                {
                    Odometer = Odometer + ch;
                }
            }

            pDataPktObj.Odometer = Convert.ToDouble(Odometer);
            pDataPktObj.Odometer = pDataPktObj.Odometer / 1000;
        }

        private void ParseAltitude(ref DataPacket pDataPktObj, string pAltitude)
        {
            string Altit = "";

            foreach (char ch in pAltitude)
            {
                if (Char.IsNumber(ch))
                {
                    Altit = Altit + ch;
                }
            }

            pDataPktObj.Altitude = Convert.ToDouble(Altit);
        }

        public string DegreesToCardinal(double degrees)
        {
            string[] caridnals = { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };
            return caridnals[(int)Math.Round(((double)degrees % 360) / 45)];
        }

        private void ParseStatusOfInputsAndOutputs(ref DataPacket pDataPktObj, string pStatus)
        {
            try
            {
                InputOutputStatus objStatus = new InputOutputStatus();
                String BinaryString = HexStringToBinary(pStatus);
                int counter = 15;

                foreach (char chr in BinaryString)
                {
                    switch (counter)
                    {
                        case 12:
                            objStatus.InEngineOnOff = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                            break;
                        case 11:
                            objStatus.InEngineOnOff = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                            break;
                        case 10:
                            objStatus.InDoorOpenClose = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                            break;
                        case 9:
                            objStatus.InAntiTemper = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                            break;
                        case 8:
                            objStatus.InSOS = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                            break;
                        case 4:
                            objStatus.OutUnlockthedoor = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                            break;
                        case 3:
                            objStatus.OutLockthedoor = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                            break;
                        case 1:
                            objStatus.OutSirenSound = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                            break;
                        case 0:
                            objStatus.OutRelyToStopCar = Convert.ToBoolean(Convert.ToInt16(chr.ToString()));
                            break;
                    }
                    counter = counter - 1;
                }
                pDataPktObj.ObjInpOutStatus = objStatus;
            }

            catch (Exception ex)
            {
                DefaultInputOutputStatus(ref pDataPktObj);
                if (WriteExceptionLog)
                {
                    File.AppendAllText(ExceptionLogPath, Environment.NewLine + "ParseStatusOfInputsAndOutputs" + ex.ToString());
                }
            }
        }

        private void DefaultInputOutputStatus(ref DataPacket pDataPktObj)
        {
            InputOutputStatus objStatus = new InputOutputStatus();

            objStatus.InAntiTemper = false;
            objStatus.InDoorOpenClose = false;
            objStatus.InEngineOnOff = false;
            objStatus.InSOS = false;
            objStatus.InUnlockDoor = false;
            objStatus.OutLockthedoor = false;
            objStatus.OutRelyToStopCar = false;
            objStatus.OutSirenSound = false;
            objStatus.OutUnlockthedoor = false;

            pDataPktObj.ObjInpOutStatus = objStatus;
        }

        string HexStringToBinary(string hexString)
        {
            var lup = new Dictionary<char, string>{
            { '0', "0000"},
            { '1', "0001"},
            { '2', "0010"},
            { '3', "0011"},

            { '4', "0100"},
            { '5', "0101"},
            { '6', "0110"},
            { '7', "0111"},

            { '8', "1000"},
            { '9', "1001"},
            { 'A', "1010"},
            { 'B', "1011"},

            { 'C', "1100"},
            { 'D', "1101"},
            { 'E', "1110"},
            { 'F', "1111"}};

            var ret = string.Join("", from character in hexString
                                      select lup[character]);
            return ret;
        }
        
        private void ParsingMT05TK208AD01AD02(DataPacket objDPkt, string pHexString)
        {
            try
            {
                string[] Collection = pHexString.Split(',');
                string HexString = Collection[0];

                //Internal Battery Level
                int InternalBatteryLvl = Convert.ToInt32(HexString, 16);
                objDPkt.InternalBatteryLvl = (InternalBatteryLvl * 6) / 1024;//>(170*6)/1024=0.99609375V(voltage)

                //External Battery Level
                HexString = Collection[1];  //(615*6)/1024=3.603515625V(voltage)
                InternalBatteryLvl = Convert.ToInt32(HexString, 16);
                objDPkt.ExternalBatteryLvl = (InternalBatteryLvl * 6) / 1024;
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + "ParsingMT05TK208AD01AD02" + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
            }

        }


        private List<List<Byte>> CreatingOnDemandRequest(List<String> IMEIColl)
        {
            //40 40 00 12 58 04 20 80 04 57 92 40 00 01 23 6F 0D 0A
            // 40 40 00 11 58 04 20 80 04 57 92 41 01 CE BC 0D 0A

            //40 40 00 11 58 04 20 80 04 57 92 41 01 CE BC 0D 0A

            List<List<Byte>> CommandsList = new List<List<byte>>();

            foreach (string item in IMEIColl)
            {
                List<Byte> ByteList = new List<byte>();
                ByteList.Add(0x40);
                ByteList.Add(0x40);
                ByteList.Add(0x00);
                ByteList.Add(0x11);


                // Create IMEI Byte
                int counter = 1;
                string byt = "";

                
                byte[] IMEIArr = StringToByteArray(item);

                foreach (byte bte in IMEIArr)
                {
                    ByteList.Add(bte);
                }


                ByteList.Add(0x41);
                ByteList.Add(0x01);

                //CheckSum Values
                ByteList.Add(0xCE);
                ByteList.Add(0xBC);

                //'\r\n'

                ByteList.Add(0x0D);
                ByteList.Add(0x0A);

                CommandsList.Add(ByteList);
            }
            return CommandsList;
        }

        private bool BroadCastRequest(List<List<Byte>> Commands, string tcpIMEI = "")
        {
            try
            {
                foreach (List<Byte> Command in Commands)
                {
                    //List<ClientInfo> ClntInfo = tcpClientsList.Where(x => x.IMEI == tcpIMEI).ToList();

                    //if (ClntInfo.Count > 0)
                    foreach (ClientInfo clnt in tcpClientsList.ToList())
                    {
                        // ClientInfo clnt = ClntInfo[0];
                        if (clnt.IMEI.Equals(tcpIMEI))
                        { 
                            if (clnt.client.Connected)
                            {
                                NetworkStream sWriter = (clnt.client.GetStream());
                              
                                Thread.Sleep(1000);
                                sWriter.Flush();
                                sWriter.Write(Command.ToArray(), 0, Command.Count);

                                string TrackerHexaRespon = BitConverter.ToString(Command.ToArray(), 0, Command.Count-1).Replace("-", " ");

                                string data = "";
                                data = Environment.NewLine + DateTime.Now.ToString() + "BroadCast";
                                data = data + Environment.NewLine + "Hexa: " + ": " + TrackerHexaRespon;

                                data = data + Environment.NewLine;
                                ObjCmnOp.WriteFile(data);

                                return true;
                          }
                    }
                   }
                }
                return false;
            }
     
            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + "BroadCastRequest" + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);                    
                }
                return false;
            }
        }

        private void UdpBroadCastRequest(byte[] RspnStream,UdpClientInfo objClient)
        {
            try
            {                
                IPEndPoint serverEP = new IPEndPoint(objClient.IPAddress, 587);
                Udplistener.Send(RspnStream, RspnStream.Length, serverEP);
            }

            catch(Exception ex)
            {
                if (WriteExceptionLog)
                {

                }
            }
        }

        private void ParsingOfflineStream(ref ClientInfo tpclient,string[] HexCollection, Byte[] bytesFromClient, string ClientSideTrackerID)
        {
            try
            {
                int StartPoint = 0;
                int EndPoint = 0;

                for (int GpsOfflinePktCounter = 0; GpsOfflinePktCounter <= 7; GpsOfflinePktCounter++)
                {
                    EndPoint = GetActualLength(bytesFromClient, StartPoint + 2);
                    Byte[] OfflineStream = new Byte[EndPoint];
                    Array.Copy(bytesFromClient, StartPoint, OfflineStream, 0, EndPoint);
                    String TrackerHexaResponse = BitConverter.ToString(OfflineStream, 0, OfflineStream.Length).Replace("-", " ");

                    ParseGpsTrackerPacket( tpclient,OfflineStream, EndPoint, ClientSideTrackerID, TrackerHexaResponse,true);
                    StartPoint = StartPoint + EndPoint;
                    //ParseGpsTrackerPacket(Byte[] bytesFrom, int ActualDataLengthSize, String pClientSideTrackerID, );
                }

            }
            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + "ParsingOfflineStream" + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
            }
        }

        private int GetActualLength(byte[] bytesFromClient, int ReadLocation)
        {
            try
            {
                string ActualLengthOfStream = BitConverter.ToString(bytesFromClient, ReadLocation, 2).Replace("-", "");
                int decval = System.Convert.ToInt32(ActualLengthOfStream, 16);
                return decval;
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + "GetActualLength OverLoad : " + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
                return 0;
            }
        }
        #region Phone Authorization
        private void AuthorizationPhoneNumber(List<String> IMEIColl, string PhoneNumber, string CurrentIMEI)
        {
            try
            {
                List<List<Byte>> ListByt = GeneratePhoneNumberCommand(IMEIColl, PhoneNumber);
                BroadCastRequest(ListByt, CurrentIMEI);
            }
            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + "AuthorizationPhoneNumber : " + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }         
            }
        }

        public List<List<Byte>> GeneratePhoneNumberCommand(List<String> IMEIColl, string PhoneNumber)
        {
            //40 40 00 32 58 04 20 80 04 57 92 41 03 01 31 33 38 38 38 38 38 38 38 38 38 00 00 00 00 00 
            //CF 68 0D 0A
            List<List<Byte>> CommandsList = new List<List<byte>>();

            foreach (string item in IMEIColl)
            {
                List<Byte> ByteList = new List<byte>();
                ByteList.Add(0x40);
                ByteList.Add(0x40);
                ByteList.Add(0x00);
                ByteList.Add(0x22);


                // Create IMEI Byte      
                byte[] IMEIArr = StringToByteArray(item);
                foreach (byte bte in IMEIArr)
                {
                    ByteList.Add(bte);
                }


                //41 03
                ByteList.Add(0x41);
                ByteList.Add(0x03);

                //PhNo.

                byte[] PhArr = StringToByteArray(PhoneNumber);
                int counter = IMEIArr.Length;

                foreach (byte bte in PhArr)
                {
                    ByteList.Add(bte);
                }

                //foreach (Byte bt in PhoneNumber)
                //{
                //    ByteList.Add(Convert.ToByte(bt));
                //}

                //CheckSum Values CF 68
                ByteList.Add(0xCF);
                ByteList.Add(0x68);

                //'\r\n'
                ByteList.Add(0x0D);
                ByteList.Add(0x0A);

                CommandsList.Add(ByteList);
            }
            return CommandsList;

        }
        #endregion
        #region Speed Limit 
        public List<List<Byte>> HandleSpeedLimit(List<String> IMEIColl, int Speed, string CurrentIMEI)
        {
            try
            {
                int Spd = Speed / 10;
                Spd = Spd * 10;
                List<List<Byte>> ListByt = GenerateSpeedCommand(IMEIColl, Spd);
                return ListByt;

            }
            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + "GetActualLength OverLoad : " + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
                return null;               
            }
        }

        private List<List<Byte>> GenerateSpeedCommand(List<String> IMEIColl, int Speed)
        {
            List<List<Byte>> CommandsList = new List<List<byte>>();

            foreach (string item in IMEIColl)
            {
                //40 40 00 12 58 04 20 80 04 57 92 41 05 0B A8 51 0D 0A
                List<Byte> ByteList = new List<byte>();
                ByteList.Add(0x40);
                ByteList.Add(0x40);
                ByteList.Add(0x00);
                ByteList.Add(0x12);


                // Create IMEI Byte
                byte[] IMEIArr = StringToByteArray(item);

                foreach (byte bte in IMEIArr)
                {
                    ByteList.Add(bte);
                }

                //41 03
                ByteList.Add(0x41);
                ByteList.Add(0x05);

                //Speed
                byte[] SpeedArr = StringToByteArray(SpeedToHexaDecimal(Speed));

                foreach(byte Spd in SpeedArr)
                {
                    ByteList.Add(Spd);
                }

                //ByteList.Add( Convert.ToByte(SpeedToHexaDecimal(Speed)));
                //ByteList.Add(0x02);
                //CheckSum Values 51 0D 0A
                ByteList.Add(0xA8);
                ByteList.Add(0x51);

                //'\r\n'
                ByteList.Add(0x0D);
                ByteList.Add(0x0A);

                CommandsList.Add(ByteList);
            }

            return CommandsList;
        }

        #endregion
        private string SpeedToHexaDecimal(int SpeedKMH)
        {
            var SpeedHexaDict = new Dictionary<int, string>{
            { 10, "1"},
            { 20, "2"},
            { 30, "3"},
            { 40, "4"},

            { 50, "5"},
            { 60, "6"},
            { 70, "7"},
            { 80, "8"},

            { 90, "9"},
            { 100, "A"},
            { 110, "B"},
            { 120, "C"},

            { 130, "D"},
            { 140, "E"},
            { 150, "F"},
            { 160, "16"},
            { 170, "11"},
            { 180, "12"},
            { 190, "13"},
            { 200, "14"},

        };
            string  ret = SpeedHexaDict[SpeedKMH];
            //returning as HexaDecimal
           return String.Format("{0:X}", ret);
            
        }

        public static byte[] StringToByteArray(String hex)
        {
            double NumberChars = hex.Length;
            double FinalVal = Math.Ceiling(NumberChars / 2);
            byte[] bytes = new byte[Convert.ToInt16(FinalVal)];

            for (int i = 0; i < NumberChars; i += 2)
                if (i + 2 > NumberChars)
                {
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 1), 16);

                }
                else
                {
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                }

            return bytes;
        }
        //Initialization request

        #region Tracker Init
        public void InitilizeRequest(List<String> IMEIColl, string CurrentIMEI)
        {
            try
            {
                List<List<Byte>>ListByt =TrackerInitializationCommand(IMEIColl);
                BroadCastRequest(ListByt, CurrentIMEI);
            }
            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + "InitilizeRequest : " + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
            }
        }
        public List<List<Byte>> TrackerInitializationCommand(List<String> IMEIColl)
        {          
            List<List<Byte>> CommandsList = new List<List<byte>>();

            foreach (string item in IMEIColl)
            {
                //40 40 00 11 58 04 20 80 04 57 92 41 10 CF B4 0D 0A     
                List<Byte> ByteList = new List<byte>();
                ByteList.Add(0x40);
                ByteList.Add(0x40);
                ByteList.Add(0x00);
                ByteList.Add(0x11);

                // Create IMEI Byte
                byte[] IMEIArr = StringToByteArray(item);

                foreach (byte bte in IMEIArr)
                {
                    ByteList.Add(bte);
                }

                //41 10 Initialization Command
                ByteList.Add(0x41);
                ByteList.Add(0x10);

                //CheckSum Values CF B4 0D 0A
                ByteList.Add(0xCF);
                ByteList.Add(0xB4);

                //'\r\n'
                ByteList.Add(0x0D);
                ByteList.Add(0x0A);

                CommandsList.Add(ByteList);
            }

            return CommandsList;
        }
        #endregion

        #region Tracker Reboot
        public void RebootRequest(List<String> IMEIColl, string CurrentIMEI)
        {
            try
            {
                List<List<Byte>> ListByt = RebootCommand(IMEIColl);
                BroadCastRequest(ListByt, CurrentIMEI);
            }
            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + "RebootRequest : " + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }

            }
        }
        public List<List<byte>> RebootCommand(List<String> IMEIColl)
        {
            //40 40 00 11 58 04 20 80 04 57 92 49 02 32 E7 0D 0A
            List<List<Byte>> CommandsList = new List<List<byte>>();

            foreach (string item in IMEIColl)
            {
                //40 40 00 11 58 04 20 80 04 57 92 49 02 32 E7 0D 0A
                List<Byte> ByteList = new List<byte>();
                ByteList.Add(0x40);
                ByteList.Add(0x40);
                ByteList.Add(0x00);
                ByteList.Add(0x11);


                // Create IMEI Byte
                byte[] IMEIArr = StringToByteArray(item);

                foreach (byte bte in IMEIArr)
                {
                    ByteList.Add(bte);
                }

                //49 02 32 E7 0D 0A
                ByteList.Add(0x49);
                ByteList.Add(0x02);

                ByteList.Add(0x32);
                ByteList.Add(0xE7);

                //'\r\n'
                ByteList.Add(0x0D);
                ByteList.Add(0x0A);

                CommandsList.Add(ByteList);
            }

            return CommandsList;
        }
        #endregion

        #region ClearDataLogger
        public void CleartheDataLogger(List<String> IMEIColl, string CurrentIMEI)
        {
            try
            {
                List<List<Byte>> ListByt = DataLoggerClearCommands(IMEIColl);
                BroadCastRequest(ListByt, CurrentIMEI);
            }

            catch (Exception ex)
            {
                if (WriteExceptionLog)
                {
                    string exception = Environment.NewLine + DateTime.Now.ToString() + "CleartheDataLogger : " + ex.ToString();
                    ObjCmnOp.WriteExceptionFile(exception);
                }
            }
        }

        public List<List<byte>> DataLoggerClearCommands(List<String> IMEIColl)
        {
            //40 40 00 11 58 04 20 80 04 57 92 49 02 32 E7 0D 0A
            List<List<Byte>> CommandsList = new List<List<byte>>();

            foreach (string item in IMEIColl)
            {
                //40 40 00 11 58 04 20 80 04 57 92 55 03 1F 5F 0D 0A 
                List<Byte> ByteList = new List<byte>();
                ByteList.Add(0x40);
                ByteList.Add(0x40);
                ByteList.Add(0x00);
                ByteList.Add(0x11);


                // Create IMEI Byte
                byte[] IMEIArr = StringToByteArray(item);

                foreach (byte bte in IMEIArr)
                {
                    ByteList.Add(bte);
                }

                //55 03 1F 5F 0D 0A 
                ByteList.Add(0x55);
                ByteList.Add(0x03);

                ByteList.Add(0x1F);
                ByteList.Add(0x5F);

                //'\r\n'
                ByteList.Add(0x0D);
                ByteList.Add(0x0A);

                CommandsList.Add(ByteList);
            }

            return CommandsList;
        }

        #endregion
    }
}

