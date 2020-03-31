
using System.Web.Http;
using AirView.DBLayer.Fleet.BLL;
using System;
using System.Net.Http;
using AirView.DBLayer.Fleet.Model;
using System.Net;
using System.Collections.Generic;
using SWI.AirView.Models;
using SWI.AirView.Common;
using System.Net.Sockets;
using System.Text;

namespace WebApplication.Services
{
    public class FleetSettingsController : ApiController
    {
        FM_TrackerAlarmConfigurationBL model = new FM_TrackerAlarmConfigurationBL();
        TcpClient tcpclnt;

        [Route("swi/Fleet/GetAllTrackers"), HttpGet]
        public HttpResponseMessage GetAllTrackers()
        {
            try
            {
                string Filter = "List_IMEI_All";
                List<FM_Vehicle> TrackersList ;
                TrackersList = model.GetAllTrackers(Filter);                                
                
                if (TrackersList == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,"No Tracker Available");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TrackersList);
                }
            }

            catch (Exception x)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(x.Message) });                
            }
        }

        [Route("swi/Fleet/SaveTrackerAlarmConfig"), HttpPost]
        public Response SaveTrackerAlarmConfig(List<FM_TrackerAlarmConfiguration> ConfigList)
        {
            try
            {
                string Filter = "Insert_UpdateTrackerAlarmConfig";

                if (ConfigList == null || ConfigList.Count==0)
                {
                    return new Response()
                    {
                        Value = false,
                        Status = "error",
                        Message = "No Configurations Found",
                    };
                }

                else
                {
                    foreach (FM_TrackerAlarmConfiguration item in ConfigList)
                    { //string Filter,string AlrmCodes, int TrackerID, bool IsEnabled,bool IsNotification, float ThresholdValues,int ModifiedOn, DateTime ModifiedBy
                        model.Insert_TrackingAlarmConfig(Filter, item.AlrmCodes.ToString(), item.TrackerId, item.IsEnabled, item.IsNotification, item.ThresholdValues, item.ModifiedBy, item.ModifiedOn);
                    }

                    bool ConnectionEstb = CreateTCPClient();

                    if (ConnectionEstb)
                    {
                        ServerCommunication(ConfigList[0].IMEI, "4444");
                    }

                    return new Response()
                    {
                        Message = "Configurations Saved Successfully",
                        Status = "Success",
                        Value = true
                    };
                }
            }

            catch (Exception ex)
            {
                return new Response()
                {
                    Message = ex.Message,
                    Status = "Error",
                    Value = false
                };
            }
        }

        [Route("swi/Fleet/SaveMultipleTrackersAlarmConfig"), HttpPost]
        public Response SaveMultipleTrackersAlarmConfig(List<FM_MultipleTrackersAlarmConfiguration> ConfigList)
        {
            try
            {
                string Filter = "Insert_UpdateMultipleTrackerAlarmConfig";
                string MultipleIMEIs = "";

                if (ConfigList == null || ConfigList.Count == 0)
                {
                    return new Response()
                    {
                        Value = false,
                        Status = "error",
                        Message = "No Configurations Found",
                    };
                }

                else
                {
                    foreach (FM_MultipleTrackersAlarmConfiguration item in ConfigList)
                    { //string Filter,string AlrmCodes, int TrackerID, bool IsEnabled,bool IsNotification, float ThresholdValues,int ModifiedOn, DateTime ModifiedBy
                        foreach (FM_AlarmConfig alrmConfig in item.AlarmConfig)
                        {
                            model.Insert_TrackingAlarmConfig(Filter, alrmConfig.AlrmCodes.ToString(), item.TrackerId, alrmConfig.IsEnabled, alrmConfig.IsNotification, alrmConfig.ThresholdValues, item.ModifiedBy, item.ModifiedOn);
                        }

                        if (MultipleIMEIs == "")
                        {
                            MultipleIMEIs = item.IMEI;
                        }
                        else
                        {
                            MultipleIMEIs = MultipleIMEIs + "," + item.IMEI;
                        }                        
                    }

                    bool ConnectionEstb = CreateTCPClient();


                    if (ConnectionEstb)
                    {
                        ServerCommunication(MultipleIMEIs, "4445", ConfigList.Count.ToString());
                    }

                    return new Response()
                    {
                        Message = "Configurations Saved Successfully",
                        Status = "Success",
                        Value = true
                    };
                }
            }

            catch (Exception ex)
            {
                return new Response()
                {
                    Message = ex.Message,
                    Status = "Error",
                    Value = false
                };
            }
        }

        //UpdateTrackerAlarmConfig
        [Route("swi/Fleet/UpdateTrackerAlarmConfig"), HttpPost]
        public Response UpdateTrackerAlarmConfig(List<FM_TrackerAlarmConfiguration> ConfigList)
        {
            try
            {
                string Filter = "UpdateTrackerAlarmConfig";

                if (ConfigList == null || ConfigList.Count == 0)
                {
                    return new Response()
                    {
                        Value = false,
                        Status = "error",
                        Message = "No Configurations Found",
                    };
                }

                else
                {
                    foreach (FM_TrackerAlarmConfiguration item in ConfigList)
                    { //string Filter,string AlrmCodes, int TrackerID, bool IsEnabled,bool IsNotification, float ThresholdValues,int ModifiedOn, DateTime ModifiedBy
                        model.Insert_TrackingAlarmConfig(Filter, item.AlrmCodes.ToString(), item.TrackerId, item.IsEnabled, item.IsNotification, item.ThresholdValues, item.ModifiedBy, item.ModifiedOn);
                    }

                    bool ConnectionEstb = CreateTCPClient();

                    if (ConnectionEstb)
                    {
                        ServerCommunication(ConfigList[0].IMEI, "4444");
                    }

                    return new Response()
                    {
                        Message = "Configurations Updated Successfully",
                        Status = "Success",
                        Value = true
                    };
                }
            }

            catch (Exception ex)
            {
                return new Response()
                {
                    Message = ex.Message,
                    Status = "Error",
                    Value = false
                };
            }
        }

        [Route("swi/Fleet/GetAlarmConfigByTID"),HttpGet]
        public HttpResponseMessage GetAlarmConfigByTID(int TID)
        {
            try
            {
                string Filter = "Get_AlarmSettings_ByTID";
                List<FM_TrackerAlarmConfiguration> TrackersConfig = model.GetTrackerAlarmConfig(Filter,TID);

                if (TrackersConfig == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Setting Available");
                }

                return Request.CreateResponse(HttpStatusCode.OK, TrackersConfig);

            }
            catch (Exception x)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(x.Message) });
            }
        }

        private bool CreateTCPClient()
        {
            try
            {
                WebConfig wc = new WebConfig();
                string TcpServer = wc.AppSettings("TcpServerIP");
                int TcpPort = Convert.ToInt32(wc.AppSettings("TcpPortNumber"));
                tcpclnt = new TcpClient();
                tcpclnt.ConnectAsync(TcpServer, TcpPort).Wait();
                return true;
            }
            catch (Exception ex)
            {                
                return false;
            }
        }

        public bool ServerCommunication(string id, string ReqCommand,string NoOfTrackers="1")
        {
            try
            {
                string IMEI = id;                

                string Command = "%%,,4101,1,64507032770165";
                NetworkStream Stream = tcpclnt.GetStream();

                if (String.Equals(ReqCommand, "4444"))
                {                    
                    Command = "%%,"   + "," + ReqCommand + ",1," + id;
                    int CommandLth = Command.Length + 2;
                    Command = "%%," + CommandLth + "," + ReqCommand + ",1," + id;
                }  
                else if(String.Equals(ReqCommand,"4445"))
                {
                    Command = "%%," + "," + ReqCommand + "," + NoOfTrackers +"," + id;
                    int CommandLth = Command.Length + 2;
                    Command = "%%," + CommandLth + "," + ReqCommand + "," + NoOfTrackers + "," + id;
                }             

                ASCIIEncoding AsciEncod = new ASCIIEncoding();
                byte[] AsciiByteColl = AsciEncod.GetBytes(Command);
                Stream.Write(AsciiByteColl, 0, AsciiByteColl.Length);

                byte[] SrvrRspnse = new byte[200];
                return true;
            }

            catch (Exception ex)
            {                
                return false;
            }
        }

    }
}

