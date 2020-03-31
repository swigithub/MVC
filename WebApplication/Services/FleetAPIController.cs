using Accord.Extensions;
using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Fleet.BLL;
using AirView.DBLayer.Fleet.Model;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using SWI.AirView.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using WebApplication.Areas.Alert.SRNotificationHub;
using WebApplication.Areas.Fleet.SRHubs;

namespace WebApplication.Services
{
    public class FleetAPIController : ApiController
    {
        public FM_VehicleBL model = new FM_VehicleBL();
        public List<FM_ManageTracker> TrackerInfo = new List<FM_ManageTracker>();
        public FM_ManageTracker TrackerInfoModel = new FM_ManageTracker();

        // GET api/<controller>/5
        public string Get()
        {
            //var context = GlobalHost.ConnectionManager.GetHubContext<ClientHub>();
            
           // context.Clients.All.addNewMessageToPage(id, id);

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ClientHub>();
            hubContext.Clients.All.addNewMessageToPage("Junaid","Shahid");


            //return Json("Message Sent !", JsonRequestBehavior.AllowGet);

            return "done";
        }

        // POST api/<controller>
        public void Post([FromBody] FM_Vehicle m)
        {
            //TrackerInfoModel.IMEI = m.TrackerID;
            //TrackerInfoModel.Status = true;

            //var Result = TrackerInfo.Find(x => x.IMEI == m.TrackerID);


            //if (Convert.ToBoolean(Result))
            //{
            //    if(TrackerInfo.Find(x => x.IMEI == m.IMEI).Status)
            //    {
            //        if (m.GPSSignalStatus != "invalid" && m.ObjInpOutStatus.InEngineOnOff != false)
            //        {
            //            model.Insert_Tracker("InsertTrackerLog", m.TrackerID, m.Latitude, m.Longitude, m.Speed, m.Odometer, m.Direction, m.Rotation, m.Altitude, m.TrackerStream, m.GPSSignalStatus, m.UTCTimeAndDate, m.ObjInpOutStatus, m.IsOfflineData);
            //        }
            //        else
            //        {
            //            TrackerInfo.Find(x => x.IMEI == m.IMEI).Status = false;
            //        }
            //    }

            //}
            //else
            //{
            //    TrackerInfo.Add(TrackerInfoModel);
            //    //insert record
            //}

            if (m.GPSSignalStatus != "invalid" && m.ObjInpOutStatus.InEngineOnOff != false)
            {
                if (m.objTrackerTrip!= null)
                {
                    if (m.objTrackerTrip.TripNo ==-1)
                    {
                        m.objTrackerTrip.TripNo = 1;
                    }                    
                }
                model.Insert_Tracker("InsertTrackerLog", m.TrackerID, m.Latitude, m.Longitude, m.Speed, m.Odometer, m.Direction, m.Rotation, m.Altitude, m.TrackerStream, m.GPSSignalStatus, m.UTCTimeAndDate, m.ObjInpOutStatus, m.IsOfflineData,m.DeviceState, m.ExtendState,m.Address,m.FuelConsumedPercentage,m.CurrentTripFuelConsumed,m.Temperature,m.CurrentTripMileage,m.GSMSignal, m.objTrackerTrip.TripNo);

                if (m.LstVState!= null && m.LstVState.Count>0)
                {
                    foreach (FM_VehicleStates VState in m.LstVState)
                    {
                        model.Insert_VState("Insert_VState", m.TrackerID, m.VehicleId, m.Latitude, m.Longitude, VState);
                    }
                }

                if (m.LstAlarms != null)
                {
                    foreach (FM_Tracker_Alarms Alrm in m.LstAlarms )
                    {
                        model.Insert_TrackingAlarms("Insert_TrackingAlarms", m.TrackerID, m.VehicleId,m.Latitude,m.Longitude,Alrm);

                        List<FM_TrackerAlarmConfiguration> lstAlrmsToNotify=model.GetAlarmsToSendNotification("GetAlarmsForNotification", m.TrackerID);

                        if  (lstAlrmsToNotify!= null)
                        {
                            if (lstAlrmsToNotify.Where(x => x.AlrmCodes == Alrm.AlrmCodes.ToString()).ToList().Count > 0)
                            {
                                //Here Trackerid property contains the AssignTo value 
                                if(lstAlrmsToNotify[0].AssignedUserID != "")
                                {
                                    SendNotification(Alrm, lstAlrmsToNotify[0].IMEI, m.VehicleId, Convert.ToInt32(lstAlrmsToNotify[0].AssignedUserID));
                                }                                
                            }
                        }                        
                    }                    
                }                            
            }

            //FM_Vehicle x = new FM_Vehicle();
            //Random rnd = new Random();
            //int month = rnd.Next(1, 10);
            //x = m;
            //x.Latitude = x.Latitude + (0.00001* month);

            var json = JsonConvert.SerializeObject(m);
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ClientHub>();
            hubContext.Clients.All.VehicleStream(json);
        }
        public void Postlist([FromBody] List<FM_Vehicle> m)
        {
            var json = JsonConvert.SerializeObject(m);
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ClientHub>();
            hubContext.Clients.All.VehicleStream(json);
        }

        [HttpPost]
        public string mydata(string v1, string v2)
        {
            string data = v1 + v2;
            return data;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        public void SendNotification(FM_Tracker_Alarms modelBind,string Vreg,int VID,int AssignTo)
        {
            /* == Insert Notification When Create New Vehicle Start == */
            AL_SetNotification DumyObject = new AL_SetNotification();
            AL_GetNotification UserNotify = new AL_GetNotification();
            AL_AlertBL AlertModel = new AL_AlertBL();
            
            string AlarmDescriptions = GetDescription(modelBind.AlrmCodes);

            WebConfig wc = new WebConfig();
            string UserIDForNotication = wc.AppSettings("NotificationUserID");

            DumyObject.AlertConfigId = 1005;
            DumyObject.EntityId = VID;
            DumyObject.Notification = "Car:" + Vreg + " generates " + AlarmDescriptions +" Alarm, current value: " + modelBind.CurrentVal;
            DumyObject.AlertRecieverId = 29;//11; //AssignTo;
            DumyObject.UserId = Convert.ToInt32(UserIDForNotication);//12;//(int)ViewBag.UserId;
            DumyObject.IsPushAlertSent = 1;
            DumyObject.IsPushAlertRead = 0;
            DumyObject.IsEmailAlertSent = 0;
            //sendEmail();

            NotificationHub SRNotificationHub = new NotificationHub();
            AL_Notification ReadNotification = new AL_Notification();
            ReadNotification.SentTo = "11";

            /* == Send Alerts based on Users == */
            List<AL_GetNotification> ModelResultReceiver = AlertModel.SendNotification("InsertNotification", DumyObject);
            for (var x = 0; x < ModelResultReceiver.Count; x++)
            {
                UserNotify.AlertRecieverId = ModelResultReceiver[x].AlertRecieverId;
                List<AL_GetNotification> ModelResult = AlertModel.GetNotification("NotificationBell", UserNotify);
                var json = JsonConvert.SerializeObject(ModelResult);
                SRNotificationHub.SendNotification(ModelResultReceiver[x].AlertRecieverId.ToString(), json);
            }
            /* == Insert Notification When Create New Vehicle End == */

        }

        public static string GetDescription(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description;
        }        
    }
}