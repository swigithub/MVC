using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.EFModel;
using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SWI.AirView.Common;
using SWI.AirView.Controllers;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.Security.BLL;
using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebApplication.Services
{
    public class WorkOrderController : ApiController
    {
        // [System.Web.Http.ActionName("Report"), System.Web.Http.HttpPost]
        [Route("swi/WorkOrder/Report"), HttpPost]
        public List<GetWorkOrder> Report(int TesterId, string IMEI)
        {
            WorkOrderBL wob = new WorkOrderBL();
            var data = wob.Report("WorkOrderReport", TesterId.ToString(), "91", IMEI.ToString());
            foreach (var item in data)
            {
                foreach (var site in item.Site)
                {
                    List<AD_ClusterScheduleVM> Devices = wob.Report1("ClusterSchedule", site.SiteId.ToString(), TesterId.ToString(), IMEI.ToString());
                    if (Devices.Count > 0)
                        site.Devices.AddRange(Devices);
                }
            }
            return data;
        }

        //[Route("swi/WorkOrder/Report"), HttpPost]
        //public List<AV_SiteScript> Report(Int64 SiteId,Int64 LayerStatusId,Int64 SequenceId,Int64 UserId,Int64 DeviceScheduleId,string IMEI)
        //{
        //    WorkOrderBL wob = new WorkOrderBL();
        //    List<AV_SiteScript> Script = wob.Report("ClusterScripts",SiteId,LayerStatusId,SequenceId,UserId,DeviceScheduleId,IMEI);

        //    return Script;
        //}

        [System.Web.Http.Route("swi/work/sync"), System.Web.Http.HttpPost]
        public List<GetWorkOrder> WorkOrder(int SiteId, int TesterId)
        {
            WorkOrderBL wob = new WorkOrderBL();
            var data = wob.Report("bySiteId", SiteId.ToString(), TesterId.ToString());
            return data;
        }

        [System.Web.Http.ActionName("UpdateDownload"), System.Web.Http.HttpPost]
        // POST: swi/WorkOrder/UpdateDownload
        public Response UpdateDownload(string TesterId, string IMEI)
        {
            Response r = new Response();
            try
            {
                AV_SitesDL sd = new AV_SitesDL();
                if (sd.Manage("UpdateDownload", TesterId, IMEI))
                {
                    r.Status = "success";
                    r.Message = "success";
                }
                else
                {
                    r.Status = "error";
                    r.Message = "some error on update";
                }
            }
            catch (Exception ex)
            {
                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;
        }

        [System.Web.Http.Route("swi/save/TestLog"), System.Web.Http.HttpPost]
        //swi/WorkOrder/saveTestLog
        public Response saveTestLog(string Status, AV_SiteTestLog rec)
        {
            Response r = new Response();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                List<AV_SiteTestLog> myDeserializedObjList = (List<AV_SiteTestLog>)Newtonsoft.Json.JsonConvert.DeserializeObject(rec.MccId, typeof(List<AV_SiteTestLog>));

                AV_SiteTestLogBL stlb = new AV_SiteTestLogBL();
                var save = stlb.Save(myDeserializedObjList.OrderBy(m => m.TimeStamp).ToList(), Status);
                //var save = stlb.Save(rec, Status);

                //DataTable dt = JsonConvert.DeserializeObject<DataTable>(rec.MccId.ToString());
                //AV_SiteTestLogDL stld = new AV_SiteTestLogDL();
                //stld.Insert(dt, Status);
                if (save)
                {
                    r.Message = "success";
                    r.Status = "success";
                }
                else
                {
                    r.Message = "Not saved";
                    r.Status = "failed";
                }



            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                r.Status = "error";
            }
            return r;
        }


        [System.Web.Http.Route("swi/save/BeamTestLog"), System.Web.Http.HttpPost]
        public List<Response> saveBeamTestLog(List<AV_BeamTestLog> rec, string Status = null)
        {

            Response r = new Response();
            try
            {
                //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                //List<AV_BeamTestLog> myDeserializedObjList = (List<AV_BeamTestLog>)Newtonsoft.Json.JsonConvert.DeserializeObject(rec.PCIId.ToString(), typeof(List<AV_BeamTestLog>));

                AV_SiteTestLogBL stlb = new AV_SiteTestLogBL();
                var save = stlb.SaveBeamTest(rec, Status);
                //var save = stlb.Save(rec, Status);

                //DataTable dt = JsonConvert.DeserializeObject<DataTable>(rec.MccId.ToString());
                //AV_SiteTestLogDL stld = new AV_SiteTestLogDL();
                //stld.Insert(dt, Status);
                if (save)
                {
                    r.Message = "success";
                    r.Status = "success";
                }
                else
                {
                    r.Message = "Not saved";
                    r.Status = "failed";
                }



            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                r.Status = "error";
            }
            List<Response> res = new List<Response>();
            res.Add(r);
            return res;
        }

        [System.Web.Http.Route("swi/Draw/Sector"), System.Web.Http.HttpPost]
        public string DrawSector(int SiteId)//int SiteId,int NetworkModeId,int BandId
        {
            string urlData = String.Empty;
            WebClient wc = new WebClient();
            var urlData1 = wc.DownloadData("http://localhost:18459/Site/DrawSector?SiteId=10098");
            return urlData;
        }

        [System.Web.Http.Route("swi/site/kpi"), System.Web.Http.HttpPost]
        public List<AV_SiteConfigurations> SiteKpi(int SiteId, string NetworkModeId, string BandId)//int SiteId,int NetworkModeId,int BandId
        {
            AV_SiteConfigurationsBL site = new AV_SiteConfigurationsBL();
            var SiteData = site.ToList("GET_Configuration", SiteId.ToString(), NetworkModeId, BandId);//,  NetworkModeId,  BandId
            return SiteData;
        }

        [System.Web.Http.Route("swi/site/scannerkpi"), System.Web.Http.HttpPost]
        public List<AV_SiteScannerConfigurations> SiteScanerKpi(int SiteId, string NetworkModeId, string BandId)//int SiteId,int NetworkModeId,int BandId
        {
            AV_SiteScannerConfigurationsBL site = new AV_SiteScannerConfigurationsBL();
            var SiteData = site.ToList("GET_Configuration", SiteId.ToString(), NetworkModeId, BandId);//,  NetworkModeId,  BandId
            return SiteData;
        }
        [System.Web.Http.Route("swi/site/deviceconfig"), System.Web.Http.HttpPost]
        public List<AV_SiteConfigurations> DeviceSchduleKpi(int SiteId, string NetworkModeId, string BandId, Int64? UserDeviceId)
        {
            AV_SiteConfigurationsBL site = new AV_SiteConfigurationsBL();
            List<AV_SiteConfigurations> SiteData = new List<AV_SiteConfigurations>();

            SiteData = site.ToList("GET_Configuration", SiteId.ToString(), NetworkModeId, BandId, UserDeviceId.ToString());

            //var SiteData = site.ToList("GET_Configuration", SiteId.ToString(), NetworkModeId, BandId, UserDeviceId.ToString());
            return SiteData;
        }

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("UpdateSiteStatus")]

        // POST: swi/WorkOrder/UpdateSiteStatus
        public Response UpdateSiteStatus(string SiteId, string Status, string NetworkMode, String Band, String Carrier)
        {
            Response r = new Response();
            try
            {
                AV_SitesDL sd = new AV_SitesDL();

                /* == Check the Status of WO == */

                if (sd.GetStatus("CheckStatus", SiteId, NetworkMode, Band, Carrier).Rows.Count > 0)
                {
                    if (sd.Manage("UpdateStatus", SiteId, Status, NetworkMode, Band, Carrier))
                    {
                        r.Status = "success";
                        r.Message = "success";
                    }
                    else
                    {
                        r.Status = "error";
                        r.Message = "error";
                    }
                }
                else
                {
                    r.Status = "Current Status is not In Progress";
                    r.Message = "error";
                }


            }
            catch (Exception ex)
            {
                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;
        }

        [System.Web.Http.Route("swi/save/ReportIssue"), System.Web.Http.HttpPost]
        // POST: swi/WorkOrder/ReportIssue
        public Response ReportIssue(AV_SiteIssueTracker sit)
        {
            Response r = new Response();
            try
            {
                AV_SiteIssueTrackerBL sitd = new AV_SiteIssueTrackerBL();

                sitd.Manage("Insert", sit, 0);
                r.Message = "success";
                r.Status = "success";
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                r.Status = "error";
            }

            return r;
        }

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("ImagePath")]
        // POST: swi/WorkOrder/ImagePath
        public Response ImagePath(ApiImagePath rec)
        {
            Response r = new Response();
            AV_SiteTestSummaryDL stsd = new AV_SiteTestSummaryDL();
            bool result = false;
            try
            {
                //if (rec.TestType == "OOKLA_TEST")
                //{
                rec.ImagePath = "/Content/AirViewLogs/" + rec.ImagePath;
                result = stsd.Manage("UpdateImages", rec.SiteId, rec.SectorId, rec.NetworkModeId, rec.BandId, rec.CarrierId, rec.ScopeId, rec.TestType, rec.Ping, rec.DL, rec.UL, rec.ImagePath, null, null, 0, "", "", "", "", rec.isManual);
                //  }
                if (result)
                {
                    r.Status = "success";
                    r.Message = "success";
                }
                else
                {
                    r.Status = "error";
                    r.Message = "Record not found.";
                }
            }
            catch (Exception ex)
            {
                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;
        }

        [System.Web.Http.Route("swi/CwCcw/HandoverStatus"), System.Web.Http.HttpPost]
        // POST: swi/WorkOrder/HandoverStatus
        public Response HandoverStatus(ApiHandoverStatus rec)
        {
            Response r = new Response();
            AV_SiteTestSummaryDL stsd = new AV_SiteTestSummaryDL();
            bool result = false;
            try
            {
                result = stsd.Manage("UpdateHandoverStatus", rec.SiteId, rec.SectorId, rec.NetworkModeId, rec.BandId, rec.CarrierId, null, null, null, null, null, null, rec.IsHandover, rec.HoType, 0);
                if (result)
                {
                    r.Status = "success";
                    r.Message = "success";
                }
                else
                {
                    r.Status = "error";
                    r.Message = "error";
                }
            }
            catch (Exception ex)
            {
                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;
        }

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("NewWoTracker")]
        // POST: /swi/WorkOrder/NewWoTracker
        public Response NewWoTracker(AV_WoTracker rec)
        {
            Response r = new Response();
            AV_WoTrackerDL wtd = new AV_WoTrackerDL();
            bool result = false;
            try
            {
                if (rec.TestType.ToUpper() == "AVX")
                {
                    MarketController Mc = new MarketController();
                    result = Mc.FileDataParser(rec.SiteId);
                }
                else
                {
                    result = wtd.Save(rec.SiteId, rec.SectorId, rec.NetworkModeId, rec.BandId, rec.CarrierId, rec.WoRefId, rec.Latitude, rec.Longitude, rec.TesterId, rec.TestType, rec.AppVersion, rec.AndroidVersion);
                }
                if (result)
                {
                    r.Status = "success";
                    r.Message = "success";
                }
                else
                {
                    r.Status = "error";
                    r.Message = "error";
                }
            }
            catch (Exception ex)
            {
                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;
        }

        [Route("swi/WorkOrder/LogTracker"), HttpPost]
        public Response LogTracker(AV_WoTracker rec)
        {
            Response r = new Response();
            AV_WoTrackerDL wtd = new AV_WoTrackerDL();
            bool result = false;
            try
            {
                result = wtd.Save(rec.SiteId, rec.SectorId, rec.NetworkModeId, rec.BandId, rec.CarrierId, rec.WoRefId, rec.Latitude, rec.Longitude, rec.TesterId, rec.TestType, rec.AppVersion, rec.AndroidVersion, rec.IMEI);
                if (result)
                {
                    r.Status = "success";
                    r.Message = "success";
                }
                else
                {
                    r.Status = "error";
                    r.Message = "error";
                }
            }
            catch (Exception ex)
            {
                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;
        }

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("NewUser")]
        // POST: /swi/WorkOrder/NewUser
        public Response NewUser(Sec_User u)
        {
            Response r = new Response();
            AV_SiteTestSummaryDL stsd = new AV_SiteTestSummaryDL();
            bool result = false;
            try
            {
                u.Password = Encryption.Encrypt(u.Password, true);
                u.RoleId = 13;
                u.IsAdmin = false;
                u.ActiveStatus = false;
                u.Picture = "/Content/Images/Profile/Default.svg";

                Sec_UserDL ud = new Sec_UserDL();
                int UserId = ud.SaveNew_Update(Convert.ToInt64(u.UserId), Convert.ToInt64(u.RoleId), u.FirstName, u.LastName, u.UserName, u.Password, u.Email, u.Address, u.Contact, 0, 0);
                if (UserId == -1)
                {
                    r.Status = "error";
                    r.Message = "Username already exist";
                }
                else if (UserId == -2)
                {
                    r.Status = "error";
                    r.Message = "Email Already Exist";
                }
                else if (UserId > 0)
                {
                    Sec_UserDevicesDL udd = new Sec_UserDevicesDL();
                    result = udd.Manage("Insert", 0, UserId, u.IMEI, u.MAC, u.Manufacturer, u.Model, false, null);
                    if (result)
                    {
                        r.Message = "success";
                        r.Status = "success";
                    }
                    else
                    {
                        r.Status = "error";
                        r.Message = "error";
                    }
                }
            }
            catch (Exception ex)
            {
                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;
        }

        [Route("swi/domain/configuration"), HttpPost]
        // POST: /swi/WorkOrder/DomainConfig
        public Sec_DomainConfiguration DomainConfig()
        {
            Sec_DomainConfigurationBL dcb = new Sec_DomainConfigurationBL();
            return dcb.Single("DeviceConfig");
        }

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("TransferDevice")]

        // POST: swi/WorkOrder/TransferDevice
        public Response TransferDevice(int UserId, string IMEI, string Password, int TranferToId)
        {
            Response r = new Response();
            try
            {
                Sec_UserDevicesBL udbl = new Sec_UserDevicesBL();
                Sec_UserDevices dev = new Sec_UserDevices();
                dev.UserId = UserId;
                dev.IMEI = IMEI;
                dev.Password = Encryption.Encrypt(Password, true);
                dev.TranferToId = TranferToId;
                var rec = udbl.Manage("TransferDevice", dev);

                r.Status = "success";
                r.Message = "success";
            }
            catch (Exception ex)
            {
                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;
        }

        [Route("swi/Site/DriveRoute"), HttpPost]
        public List<AV_DriveRoutes> DriveRoute(int SiteId, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, string RouteType)
        {
            try
            {
                AV_DriveRoutesBL drb = new AV_DriveRoutesBL();
                return drb.ToList("GetSelectedBySiteId", SiteId.ToString(), true.ToString(), RouteType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Route("swi/locking/command"), HttpPost]
        public AV_DeviceLockCommands LockingCommand(string LockType, Int64 NetworkModeId, Int64 BandId, string Carrier, string DeviceModel)
        {
            try
            {
                AV_DeviceLockCommands lc = new AV_DeviceLockCommands();
                lc.MenuType = LockType;
                lc.NetworkModeId = NetworkModeId;
                lc.BandId = BandId;
                lc.DeviceModel = DeviceModel;

                AV_DeviceLockCommandsBL dcb = new AV_DeviceLockCommandsBL();

                return dcb.ToSingle(lc);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Route("swi/get/SelectedList"), HttpPost]
        public List<SWI.Libraries.Common.SelectedList> SelectedList(string filter, string value)
        {
            try
            {
                List<SWI.Libraries.Common.SelectedList> rec = new List<SWI.Libraries.Common.SelectedList>();
                AD_DefinationBL db = new AD_DefinationBL();
                if (filter == "NetworkModes")
                {
                    rec = db.SelectedList("NetworkModes", null, null);
                }
                else if (filter == "Bands" || filter == "Carriers")
                {
                    rec = db.SelectedList("PDefinationId", value, null);
                }
                else if (filter == "Scopes")
                {
                    rec = db.SelectedList("UserScopes", value, null);
                }
                else if (filter == "Cities")
                {
                    rec = db.SelectedList("UserCities", value, null);
                }
                else if (filter == "Regions")
                {
                    rec = db.SelectedList("UserRegions", value, null);
                }
                else if (filter == "SiteTypes")
                {
                    rec = db.SelectedList("SiteTypes", value, null);
                }
                else if (filter == "Clients")
                {
                    ClientsBL cbl = new ClientsBL();
                    rec = cbl.SelectedList("UserClients", value, null);
                }

                return rec;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Route("swi/getProjectClient"), HttpPost]
        public HttpResponseMessage ToList(string filter, string value = null)
        {
            ClientsDL cd = new ClientsDL();
            DataTable dt = cd.Get(filter, value);
            var result = dt.ToList<AD_Clients>();
            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Record not found");
            }
        }


        [Route("swi/site/workOrder"), HttpPost]
        public List<Response> workOrder(List<Workorder> wolst)
        {
            List<Response> rl = new List<Response>();
            Response r = new Response();
            try
            {
                WorkOrderBL wb = new WorkOrderBL();
                Workorder wo = new Workorder();
                if (wolst.Count() > 0)
                {
                    wo = wolst.FirstOrDefault();
                    wb.Insert("UEWorkOrder", wo, wolst, wo.UserId, wo.IMEI);

                    var rec = wolst.GroupBy(test => test.networkMode).Select(grp => grp.First()).ToList();
                    DirectoryHandler dh = new DirectoryHandler();
                    foreach (var item in rec)
                    {
                        string site = item.ClientPrefix + "/" + item.siteCode + "/" + item.networkMode + "_" + item.Band + "_" + item.Carrier;
                        dh.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/AirViewLogs/" + site));
                    }

                    r.Status = "success";
                    r.Message = "success";
                    rl.Add(r);
                }
                else
                {
                    r.Status = "error";
                    r.Message = "no record.";
                    rl.Add(r);
                }
            }
            catch (Exception ex)
            {
                r.Status = "error";
                r.Message = ex.Message;
                rl.Add(r);
            }

            return rl;
        }

        [Route("swi/get/Widgets"), HttpPost]
        public List<AV_WidgetCategory> GetWidgets()
        {
            try
            {
                AV_WidgetsBL wb = new AV_WidgetsBL();
                return wb.ToListCategory("GetAll");
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Route("swi/Widget/Data"), HttpPost]
        public List<TempDataset> WidgetData(string Id, string Value)
        {
            try
            {
                List<TempDataset> tdlst = new List<TempDataset>();
                TempDataset td;
                string[] Ids = Id.Split(',');
                AD_GetQueryResultDL gr = new AD_GetQueryResultDL();
                for (int i = 0; i < Ids.Length; i++)
                {
                    td = new TempDataset();
                    td.Name = Ids[i];
                    td.Table = gr.Get("WidgetQueryResult", Ids[i], Value);
                    tdlst.Add(td);
                }
                return tdlst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("swi/workorder/statusCheck"), HttpPost]
        public Response WoStatusCheck(Int64 SiteId)
        {
            Response r = new Response();

            try
            {
                WorkOrderBL wob = new WorkOrderBL();
                var rec = wob.StatusCheck(SiteId.ToString());

                SendMessage("fNFROsaeNXw:APA91bHvKRAjA6HI4gFOYi6Y_iMlgtYPY8EEqskwaLDBJUq3Pf3o13ZLmA1_JvSnTMfhylSKWsk97bqMlg7i4HSYpyw1pVHjtkys0xRyzGC2Z58y6v3r2XgbJok0NV6CbB135RCE6qGJ", rec);
                r.Status = "success";
                r.Message = "success";
            }
            catch (Exception ex)
            {
                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;
        }

        private void SendMessage(string DevieToken, WoStatusCheck obj)
        {
            try
            {
                string jsonObject = JsonConvert.SerializeObject(
                            new
                            {
                                data = obj,
                                to = DevieToken
                            });
                WebConfig wc = new WebConfig();
                var FirebaseKey = wc.AppSettings("FirebaseKey");
                var result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=AAAAUsKbphY:APA91bHsUzKVbX0y9hUWL1U_kwsikmTIOE69lCmBiWOdGkrrLvQNCDYdwfbTKGycXpjSa1B8vMFnvmyAE49wCtZXt8TnLKORkMXBCvfEFBWO9ZRfChSzGSysk9UhK8Xb5oBTUEuqhgkj");
                httpWebRequest.Method = "POST";
                // VMT,MT,SMSMT
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //  string json = "{\"to\": \"dfk90rb_znE:APA91bHLegH5Ru1zVNj-aDnV6zrm7ewOFmjl9QvM7N9wL-wnInpqBlHWfWyICpcH6lJgRSux2M-EkVXhtRlGdb2REnwftdE-4xgXAOxm54U-c35PIB7k7Q-o-wepxhC0_30QQp3D2jhe\",\"data\": {\"message\": \"This is a Firebase Cloud Messaging Topic Message!\",}}";
                    //string json = "{ \"data\": {\"SiteId\": \"" + obj.SiteId + "\",\"WoStatus\":\"" + obj.WoStatus  + "\",\"LicenseType\":\"" + obj.LicenseType + "\",\"ScheduledOn\": \""+ obj.ScheduledOn+"\"},\"to\" : \"" + DevieToken + "\"}";
                    string json = jsonObject;
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                // return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("swi/workorder/Summary"), HttpPost]
        public List<WoSummary> Summary(string filter, Int64 UserId, DateTime FromDate, DateTime ToDate, string RegionId, string MarketId, string ScopeId)
        {
            List<WoSummary> record = new List<WoSummary>();

            try
            {
                WoSummary ws = new WoSummary();
                ws.Id = 0;
                ws.Name = "test";
                ws.Value = "15";
                ws.Color = "#FF4091";
                record.Add(ws);
                record.Add(ws);
                record.Add(ws);
                record.Add(ws);
                record.Add(ws);
                record.Add(ws);

                return record;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("swi/workorder/MapView"), HttpPost]
        public List<WoMapView> MapView(string filter, Int64 UserId, DateTime FromDate, DateTime ToDate, string RegionId, string MarketId, string ScopeId)
        {
            List<WoMapView> record = new List<WoMapView>();

            WoMapViewTester mvt = new WoMapViewTester();

            try
            {
                mvt.TesterId = 0;
                mvt.FullName = "test test";
                mvt.Picture = "/Content/Images/Profile/thumb-11.png";
                WoMapView mv = new WoMapView();
                mv.SiteId = 1;
                mv.SiteCode = "test";
                mv.Latitude = 29.191169;
                mv.Longitude = -81.0772696;
                mv.MarketIcon = "/Content/Images/Common/DRIVE_COMPLETED_MARKER.png";
                mv.SubmittedOn = DateTime.Now.ToShortDateString();
                mv.ApprovedOn = DateTime.Now.ToShortDateString();
                mv.ReceivedOn = DateTime.Now.ToShortDateString();
                mv.ScheduledOn = DateTime.Now.ToShortDateString();
                mv.DriveCompletedOn = DateTime.Now.ToShortDateString();
                mv.ReportSubmittedOn = DateTime.Now.ToShortDateString();
                mv.Client = "Nokia";
                mv.Market = "Tempa";
                mv.WoStatus = "Approved";
                mv.WoStatusColor = "#247524";
                mv.MapViewTester.Add(mvt);

                mv.MapViewTester.Add(mvt);

                record.Add(mv);

                mv.Latitude = 28.697444;
                mv.Longitude = -81.357372;
                record.Add(mv);

                return record;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("swi/Test/Script"), HttpPost]
        public Response TestScript(Int64 SiteId, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 ScopeId, Int64? DeviceScheduleId)
        {
            Response _response = new Response();
            List<AV_SiteScript> _Scripts = new List<AV_SiteScript>();
            try
            {
                List<AV_NodesProperties> _Form = new List<AV_NodesProperties>();
                List<AV_ScriptScannerConfigurations> _Scanner = new List<AV_ScriptScannerConfigurations>();
                AV_SiteScriptDL ssd = new AV_SiteScriptDL();
                DataSet _dt = ssd._Get("Script_ByNetworkLayer", SiteId.ToString(), NetworkModeId.ToString(), BandId.ToString(), CarrierId.ToString(), ScopeId.ToString(), DeviceScheduleId.ToString());
                if (_dt != null && _dt.Tables.Count > 0)
                {
                    _Scripts = _dt.Tables[0].ToList<AV_SiteScript>().Count > 0 ? _dt.Tables[0].ToList<AV_SiteScript>() : null;
                    _Form = _dt.Tables[1].ToList<AV_NodesProperties>().Count > 0 ? _dt.Tables[1].ToList<AV_NodesProperties>() : null;

                    if (_Form != null)
                    {
                        foreach (var item in _Scripts)
                        {
                            item.ActionDialogue = _Form != null ? _Form.Where(x => x.NodeTypeId == item.ScriptId).ToList() : null;
                        }
                    }
                    if (_Scanner != null)
                    {
                        foreach (var item in _Scripts)
                        {
                            //MeasurementRSSI data = new MeasurementRSSI();
                            ////RSSITree(_Scanner, data);
                            JObject scObj = new JObject();
                            DataSet _scanerConfigDbData = ssd._Get("ScannerConfig", SiteId.ToString(), NetworkModeId.ToString(), BandId.ToString(), item.EventCommand,item.ScriptId.ToString());
                            if (_scanerConfigDbData.Tables[0].Rows.Count>0)
                            {
                                _Scanner = _scanerConfigDbData.Tables[0].ToList<AV_ScriptScannerConfigurations>().Count > 0 ? _scanerConfigDbData.Tables[0].ToList<AV_ScriptScannerConfigurations>() : null;
                                if (_Scanner.Count() > 0)
                                {
                                    var mainList = _Scanner.FirstOrDefault(x => x.KpiId == Convert.ToInt32(item.EventCommand));
                                    if (mainList != null && mainList.KpiId > 0)
                                    {
                                        string type = string.Empty;
                                        string body = string.Empty;
                                        if (mainList.KpiCode == "ANTENA")
                                        {
                                            type = "REQUEST_TYPE";
                                            body = "ANTENNA_LIST";
                                        }
                                        else
                                        {
                                            type = "SCAN_TYPE";
                                            body = "SCAN_REQUEST_BODY";
                                        }
                                        scObj.Add(type, mainList.KpiName);
                                        _Scanner.Remove(mainList);
                                        JObject jd = new JObject();
                                        JsonRecursive(_Scanner, jd, mainList.KpiId);
                                        scObj[body] = jd;
                                        item.ScannerConfig = JsonConvert.SerializeObject(scObj);
                                    }
                                }
                            }
                        }
                    }
                    _response.Message = "Success !";  //+ex.Message;
                    _response.Status = "true";
                    _response.Value = _Scripts;
                }
                else
                {
                    _response.Message = "Success !";  //+ex.Message;
                    _response.Status = "true";
                    _response.Value = _Scripts;
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error occured !";  //+ex.Message;
                _response.Status = "false";
                _response.Value = _Scripts;
                return _response;
            }
        }
        public void JsonRecursive(List<AV_ScriptScannerConfigurations> list, JObject jbody, int id)
        {
            try
            {
                foreach (var item in list.Where(a => a.pKpiId == id))
                {
                    var child = list.Where(a => a.pKpiId == item.KpiId).ToList();
                    if (child.Count() > 0 || item.KpiCode == "SCAN_SAMPLING_MODE_ARRAY" || item.KpiCode == "DATA_MODE_LIST" || item.KpiCode == "BLIND_SCAN_REQUEST_ELEMENT_LIST")
                    {
                        if (item.KpiCode == "SCAN_SAMPLING_MODE_ARRAY" || item.KpiCode == "DATA_MODE_LIST")
                        {
                            JArray jjj = new JArray();
                            if (item.KpiValue.StartsWith("0x"))
                            {
                                if (!string.IsNullOrEmpty(item.KpiValue))
                                    jjj.Add(Convert.ToInt64(item.KpiValue, 16));

                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(item.KpiValue))
                                    jjj.Add(Convert.ToInt64(item.KpiValue));

                            }
                            jbody[item.KpiCode] = jjj;
                        }
                        else if (item.KpiCode == "BLIND_SCAN_REQUEST_ELEMENT_LIST")
                        {
                            JArray jjj = new JArray();
                            JObject jd = new JObject();
                            foreach (var ch in child)
                            {
                                var child1 = list.Where(a => a.pKpiId == ch.KpiId).ToList();
                                if (child1.Count() > 0 || ch.KpiCode == "SCAN_SAMPLING_MODE_ARRAY" || ch.KpiCode == "DATA_MODE_LIST")
                                {
                                    if (ch.KpiCode == "SCAN_SAMPLING_MODE_ARRAY" || ch.KpiCode == "DATA_MODE_LIST")
                                    {
                                        JArray jj = new JArray();
                                        if (ch.KpiValue.StartsWith("0x"))
                                        {
                                            if (!string.IsNullOrEmpty(ch.KpiValue))
                                                jj.Add(Convert.ToInt64(ch.KpiValue, 16));

                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(ch.KpiValue))
                                                jj.Add(Convert.ToInt64(ch.KpiValue));

                                        }
                                        jd[ch.KpiCode] = jj;
                                    }
                                    else if (ch.KpiCode == "CHANNEL_ARRAY" || ch.KpiCode == "BLIND_SCAN_REQUEST_BAND_ELEMENT_LIST" || ch.KpiCode == "THRESHOLD_LIST")
                                    {
                                        JArray jj = new JArray();
                                        JObject jd1 = new JObject();
                                        foreach (var ch1 in child1)
                                        {
                                            if (ch1.KpiValue.StartsWith("0x"))
                                            {
                                                if (!string.IsNullOrEmpty(ch1.KpiValue))
                                                    jd1.Add(ch1.KpiCode, Convert.ToInt64(ch1.KpiValue, 16));
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(ch1.KpiValue))
                                                    jd1.Add(ch1.KpiCode, Convert.ToInt64(ch1.KpiValue));
                                            }

                                        }
                                        jj.Add(jd1);
                                        jd[ch.KpiCode] = jj;
                                    }
                                    else
                                    {
                                        JObject jd1 = new JObject();
                                        foreach (var ch1 in child1)
                                        {
                                            var child2 = list.Where(a => a.pKpiId == ch1.KpiId).ToList();
                                            if (child2.Count() > 0 || ch1.KpiCode == "SCAN_SAMPLING_MODE_ARRAY" || ch1.KpiCode == "DATA_MODE_LIST")
                                            {
                                                if (ch1.KpiCode == "SCAN_SAMPLING_MODE_ARRAY" || ch1.KpiCode == "DATA_MODE_LIST")
                                                {
                                                    JArray j = new JArray();
                                                    if (ch1.KpiValue.StartsWith("0x"))
                                                    {
                                                        if (!string.IsNullOrEmpty(ch1.KpiValue))
                                                            j.Add(Convert.ToInt64(ch1.KpiValue, 16));
                                                    }
                                                    else
                                                    {
                                                        if (!string.IsNullOrEmpty(ch1.KpiValue))
                                                            j.Add(Convert.ToInt64(ch1.KpiValue));

                                                    }
                                                    jd1[ch1.KpiCode] = j;
                                                }
                                                else
                                                {
                                                    JObject jd2 = new JObject();
                                                    foreach (var ch2 in child2)
                                                    {
                                                        if (ch2.KpiValue.StartsWith("0x"))
                                                        {
                                                            if (!string.IsNullOrEmpty(ch2.KpiValue))
                                                                jd2.Add(ch2.KpiCode, Convert.ToInt64(ch2.KpiValue, 16));
                                                        }
                                                        else
                                                        {
                                                            if (!string.IsNullOrEmpty(ch2.KpiValue))
                                                                jd2.Add(ch2.KpiCode, Convert.ToInt64(ch2.KpiValue));

                                                        }

                                                    }
                                                    jd1[ch1.KpiCode] = jd2;
                                                }

                                            }
                                            else
                                            {
                                                if (ch1.KpiValue.StartsWith("0x"))
                                                {
                                                    if (!string.IsNullOrEmpty(ch1.KpiValue))
                                                        jd1.Add(ch1.KpiCode, Convert.ToInt64(ch1.KpiValue, 16));

                                                }
                                                else if (ch1.KpiCode == "CHANNEL_SPECIFIED")
                                                {
                                                    if (!string.IsNullOrEmpty(ch1.KpiValue))
                                                        jd1.Add(ch1.KpiCode, ch1.KpiValue == "0" ? false : true);

                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(ch1.KpiValue))
                                                        jd1.Add(ch1.KpiCode, Convert.ToInt64(ch1.KpiValue));

                                                }

                                            }
                                        }
                                        jd[ch.KpiCode] = jd1;
                                    }

                                }
                                else
                                {
                                    if (ch.KpiValue.StartsWith("0x"))
                                    {
                                        if (!string.IsNullOrEmpty(ch.KpiValue))
                                            jd.Add(ch.KpiCode, Convert.ToInt64(ch.KpiValue, 16));

                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(ch.KpiValue))
                                            jd.Add(ch.KpiCode, Convert.ToInt64(ch.KpiValue));

                                    }



                                }
                            }
                            jjj.Add(jd);
                            jbody[item.KpiCode] = jjj;
                        }
                        else
                        {
                            JObject jd = new JObject();
                            foreach (var ch in child)
                            {
                                var child1 = list.Where(a => a.pKpiId == ch.KpiId).ToList();
                                if (child1.Count() > 0 || ch.KpiCode == "SCAN_SAMPLING_MODE_ARRAY" || ch.KpiCode == "DATA_MODE_LIST")
                                {
                                    if (ch.KpiCode == "SCAN_SAMPLING_MODE_ARRAY" || ch.KpiCode == "DATA_MODE_LIST")
                                    {
                                        JArray jj = new JArray();
                                        if (ch.KpiValue.StartsWith("0x"))
                                        {
                                            if (!string.IsNullOrEmpty(ch.KpiValue))
                                                jj.Add(Convert.ToInt64(ch.KpiValue, 16));

                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(ch.KpiValue))
                                                jj.Add(Convert.ToInt64(ch.KpiValue));

                                        }
                                        jd[ch.KpiCode] = jj;
                                    }
                                    else if (ch.KpiCode == "CHANNEL_ARRAY")
                                    {
                                        JArray jj = new JArray();
                                        var channels = child1.FirstOrDefault(a => a.KpiCode == "CHANNEL_NUMBER");
                                        var styles = child1.FirstOrDefault(a => a.KpiCode == "CHANNEL_STYLE");
                                        var chnlArr = channels.KpiValue.Split(',');
                                        for (int i = 0; i < chnlArr.Count(); i++)
                                        {
                                            JObject jd1 = new JObject();
                                            //foreach (var ch1 in child1)
                                            //{
                                            //    if (!string.IsNullOrEmpty(ch1.KpiValue))
                                            //        jd1.Add(ch1.KpiCode, Convert.ToInt64(ch1.KpiValue));
                                            //}
                                            if (!string.IsNullOrEmpty(chnlArr[i]))
                                                jd1.Add(channels.KpiCode, Convert.ToInt64(chnlArr[i]));
                                            if (!string.IsNullOrEmpty(styles.KpiValue))
                                                jd1.Add(styles.KpiCode, Convert.ToInt64(styles.KpiValue));
                                            jj.Add(jd1);
                                        }
                                        
                                        jd[ch.KpiCode] = jj;
                                    }
                                    else
                                    {
                                        JObject jd1 = new JObject();
                                        foreach (var ch1 in child1)
                                        {
                                            var child2 = list.Where(a => a.pKpiId == ch1.KpiId).ToList();
                                            if (child2.Count() > 0 || ch1.KpiCode == "SCAN_SAMPLING_MODE_ARRAY" || ch1.KpiCode == "DATA_MODE_LIST")
                                            {
                                                if (ch1.KpiCode == "SCAN_SAMPLING_MODE_ARRAY" || ch1.KpiCode == "DATA_MODE_LIST")
                                                {
                                                    JArray j = new JArray();
                                                    if (ch1.KpiValue.StartsWith("0x"))
                                                    {
                                                        if (!string.IsNullOrEmpty(ch1.KpiValue))
                                                            j.Add(Convert.ToInt64(ch1.KpiValue, 16));
                                                    }
                                                    else
                                                    {
                                                        if (!string.IsNullOrEmpty(ch1.KpiValue))
                                                            j.Add(Convert.ToInt64(ch1.KpiValue));

                                                    }
                                                    jd1[ch1.KpiCode] = j;
                                                }
                                                else if (ch1.KpiCode == "CHANNEL")
                                                {
                                                    JObject jj = new JObject();
                                                    var channels = child2.FirstOrDefault(a => a.KpiCode == "CHANNEL_NUMBER");
                                                    var styles = child2.FirstOrDefault(a => a.KpiCode == "CHANNEL_STYLE");
                                                    var chnlArr = channels.KpiValue.Split(',');
                                                    for (int i = 0; i < 1; i++)
                                                    {
                                                        if (!string.IsNullOrEmpty(chnlArr[i]))
                                                            jj.Add(channels.KpiCode, Convert.ToInt64(chnlArr[i]));
                                                        if (!string.IsNullOrEmpty(styles.KpiValue))
                                                            jj.Add(styles.KpiCode, Convert.ToInt64(styles.KpiValue));
                                                    }

                                                    jd1[ch1.KpiCode] = jj;
                                                }
                                                else
                                                {
                                                    JObject jd2 = new JObject();
                                                    foreach (var ch2 in child2)
                                                    {
                                                        if (ch2.KpiValue.StartsWith("0x"))
                                                        {
                                                            if (!string.IsNullOrEmpty(ch2.KpiValue))
                                                                jd2.Add(ch2.KpiCode, Convert.ToInt64(ch2.KpiValue, 16));
                                                        }
                                                        else
                                                        {
                                                            if (!string.IsNullOrEmpty(ch2.KpiValue))
                                                                jd2.Add(ch2.KpiCode, Convert.ToInt64(ch2.KpiValue));

                                                        }

                                                    }
                                                    jd1[ch1.KpiCode] = jd2;
                                                }

                                            }
                                            else
                                            {
                                                if (ch1.KpiValue.StartsWith("0x"))
                                                {
                                                    if (!string.IsNullOrEmpty(ch1.KpiValue))
                                                        jd1.Add(ch1.KpiCode, Convert.ToInt64(ch1.KpiValue, 16));

                                                }
                                                else if (ch1.KpiCode == "CHANNEL_SPECIFIED")
                                                {
                                                    if (!string.IsNullOrEmpty(ch1.KpiValue))
                                                        jd1.Add(ch1.KpiCode, ch1.KpiValue == "0" ? false : true);

                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(ch1.KpiValue))
                                                        jd1.Add(ch1.KpiCode, Convert.ToInt64(ch1.KpiValue));

                                                }

                                            }
                                        }
                                        jd[ch.KpiCode] = jd1;
                                    }

                                }
                                else
                                {
                                    if (ch.KpiValue.StartsWith("0x"))
                                    {
                                        if (!string.IsNullOrEmpty(ch.KpiValue))
                                            jd.Add(ch.KpiCode, Convert.ToInt64(ch.KpiValue, 16));

                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(ch.KpiValue))
                                            jd.Add(ch.KpiCode, Convert.ToInt64(ch.KpiValue));

                                    }



                                }
                            }
                            jbody[item.KpiCode] = jd;
                        }

                    }
                    else
                    {
                        if (item.KpiValue.StartsWith("0x"))
                        {
                            if (!string.IsNullOrEmpty(item.KpiValue))
                                jbody.Add(item.KpiCode, Convert.ToInt64(item.KpiValue, 16));
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(item.KpiValue))
                                jbody.Add(item.KpiCode, Convert.ToInt64(item.KpiValue));

                        }



                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        [HttpPost, Route("swi/Test/SaveDynamicForm")]
        public List<Response> FormEntry(List<AV_SiteScriptFormEntry> task)
        {
            Response res = new Response();
            List<Response> _res = new List<Response>();
            try
            {

                dbDataTable dbdt = new dbDataTable();
                var dt1 = dbdt.TaskList();
                AV_SiteScriptDL _ssd = new AV_SiteScriptDL();
                foreach (var item in task)
                {
                    myDataTable.AddRow(dt1, "Value1", item.FormId, "Value2", item.ActualValue);
                }
                res.Value = _ssd._Manage("Update_UI", dt1);
                res.Status = "true";
                res.Message = "Save successfully";
                _res.Add(res);
                return _res;
            }
            catch (Exception ex)
            {
                res.Status = "false";
                res.Message = ex.Message;
                _res.Add(res);
                return _res;
            }

        }



        [Route("swi/Script/Definations"), HttpPost]
        public List<AD_Defination> ScriptDefinations()
        {
            try
            {
                AD_DefinationBL db = new AD_DefinationBL();
                var def = db.ToList("byDefinationType", "Script Type");
                var ScriptEvent = db.ToList("byDefinationType", "Script Events");
                var SubEvents = db.ToList("byDefinationType", "Script Sub Events");
                foreach (var item in def)
                {
                    var types = DefinationTree(item.DefinationId, ScriptEvent);
                    item.Definations.AddRange(types);
                    foreach (var item2 in types)
                    {
                        item2.Definations.AddRange(DefinationTree(item2.DefinationId, SubEvents));
                    }
                }

                return def;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("swi/Script/byDefinationType"), HttpPost]
        public List<AD_Defination> ByDefinationType(string value = null)
        {
            try
            {
                AD_DefinationBL db = new AD_DefinationBL();
                var def = db.ToList("byDefinationType", value);
                return def;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("swi/Script/ProjectRes_Task"), HttpPost]
        public HttpResponseMessage GetTasks(string Filter, long ProjectId = 0, Int64 TaskId = 0)

        {
            //GET_PROJECT_RESOURCES OR  GET_PROJECT_TASK, ProjectId, TaskId
            PM_IssueBL bal = new PM_IssueBL();

            if (Filter == "GET_PROJECT_RESOURCES")
            {
                var result = bal.GetUsers(Filter, ProjectId);
                if (result != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "No data found");
                }
            }
            else
            {
                var result = bal.GetTasks(Filter, ProjectId, TaskId);
                if (result != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "No data found");
                }
            }
        }


        [Route("swi/Script/DefinationsOld"), HttpPost]
        public List<AD_Defination> ScriptDefinationsOld()
        {
            try
            {
                AD_DefinationBL db = new AD_DefinationBL();
                var def = db.ToList("byDefinationType", "Script Type");
                var ScriptEvent = db.ToList("byDefinationType", "Script Events");
                var SubEvents = db.ToList("byDefinationType", "Script Sub Events");
                def.AddRange(ScriptEvent);
                def.AddRange(SubEvents);
                return def;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<AD_Defination> DefinationTree(Int64 DefinationId, List<AD_Defination> def)
        {
            return def.Where(m => m.PDefinationId == DefinationId).ToList();
        }

        [Route("swi/E911/Status"), HttpPost]
        public Response E911Status(ApiE911Status e)
        {
            Response r = new Response();
            AV_SiteTestSummaryDL stsd = new AV_SiteTestSummaryDL();
            bool result = false;
            try
            {
                result = stsd.Manage("E911Status", e.SiteId.ToString(), e.SectorId.ToString(), e.NetworkModeId.ToString(), e.BandId.ToString(), e.CarrierId.ToString(), e.ScopeId.ToString(), "E911", null, null, e.Comment, null, e.IsPerformed.ToString(), e.TesterName, e.TesterId);
                if (result)
                {
                    r.Status = "success";
                    r.Message = "success";
                }
                else
                {
                    r.Status = "error";
                    r.Message = "Record not found.";
                }
            }
            catch (Exception ex)
            {
                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;
        }

        [Route("swi/E911/ConfirmStatus"), HttpPost]
        public Response E911ConfirmStatus(ApiE911ConfirmStatus e)
        {
            Response r = new Response();
            AV_SiteTestSummaryDL stsd = new AV_SiteTestSummaryDL();
            bool result = false;
            try
            {
                result = stsd.Manage("E911ConfirmStatus", e.SiteId.ToString(), e.SectorId.ToString(), e.NetworkModeId.ToString(), e.BandId.ToString(), e.CarrierId.ToString(), e.ScopeId.ToString(), "E911", null, null, e.Comment, null, e.TestStatus.ToString(), e.SwitchTechName, e.TesterId);
                if (result)
                {
                    r.Status = "success";
                    r.Message = "success";
                }
                else
                {
                    r.Status = "error";
                    r.Message = "Record not found.";
                }
            }
            catch (Exception ex)
            {
                r.Status = "error";
                r.Message = ex.Message;
            }
            return r;
        }

        [Route("swi/SiteScript/New"), HttpPost]
        public Response SiteScriptNew(List<AV_SiteScript> script)
        {
            List<Response> res = new List<Response>();
            Response r = new Response();
            try
            {
                dbDataTable dbdt = new dbDataTable();
                AV_SiteScriptBL scriptBL = new AV_SiteScriptBL();

                DataTable dt = dbdt.List();
                foreach (var s in script)
                {
                    myDataTable.AddRow(dt,

                        "Value1", s.EventTypeId,
                        "Value2", s.EventValue,
                        "Value3", s.IsValue,
                        "Value4", s.IsL3Enabled,
                        "Value5", s.Color,
                        "Value6", s.SequenceId,

                        "Value7", s.SiteId,
                        "Value8", s.BandId,
                        "Value9", s.CarrierId,
                        "Value10", s.NetworkModeId,
                        "Value12", s.SortOrder
                        );
                }

                if (scriptBL.Save("InsertScripts", dt))
                {
                    r.Value = true;
                    r.Message = "success";
                    r.Status = "Script saved successfully";
                }
                else
                {
                    r.Value = false;
                    r.Status = "error";
                    r.Message = "record not save.";
                }
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                r.Status = "error";
                res.Add(r);
            }
            return r;
        }

        [Route("swi/Site/TestSummary"), HttpPost]
        public List<AV_SiteTestSummary> SiteTestSummary(Int32 SiteId, Int32 BandId, string Carrier, string NetworkMode, Int32 UserId)
        {
            AV_SiteTestSummaryDL siteSummary = new AV_SiteTestSummaryDL();

            DataTable dt = siteSummary.NetLayerSummary(SiteId, BandId, Carrier, NetworkMode, UserId);

            List<AV_SiteTestSummary> lst = new List<AV_SiteTestSummary>();
            if (dt != null)
            {
                lst = dt.ToList<AV_SiteTestSummary>();
            }
            return lst;
        }

        [Route("swi/Site/NetLayers"), HttpPost]
        public DataTable SiteNetLayers(string SiteCode, Int32 UserId)
        {
            AV_SiteTestSummaryDL siteNetLayers = new AV_SiteTestSummaryDL();
            try
            {
                DataTable dt = siteNetLayers.NetLayersBySiteCode(SiteCode, UserId);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }





        //[Route("Api/uploadLogs/{siteID}/{sectorID}/{networkModeID}/{bandID}/{carrierID}/{scopeID}/{filePath}"), HttpGet]
        //[Route("swi/uploadLogs/{siteID}/{sectorID}/{networkModeID}/{bandID}/{carrierID}/{scopeID}/{filePath}"), HttpGet]


        [Route("swi/NemoLogs"), HttpPost]
        //public HttpResponseMessage addCsvFile(int siteID, int sectorID, int networkModeID, int bandID, int carrierID, int scopeID, string filePath)
        public Response addCsvFile(AV_NemoFiles nemoFiles)
        {
            int file_ID = 0;
            Response resp = new Response();
            try
            {
                int siteID = 0;
                int sectorID = 0;
                int networkModeID = 0;
                int bandID = 0;
                int carrierID = 0;
                int scopeID = 0;
                string filePath = "";


                siteID = nemoFiles.siteID;
                sectorID = nemoFiles.sectorID;
                networkModeID = nemoFiles.networkModeID;
                bandID = nemoFiles.bandID;
                carrierID = nemoFiles.carrierID;
                scopeID = nemoFiles.scopeID;
                //filePath = nemoFiles.filePath + "/'" + nemoFiles.networkMode + "_" + nemoFiles.band + "_" + nemoFiles.carrier + "'/" + nemoFiles.fileName;
                filePath = nemoFiles.filePath + "/" + nemoFiles.fileName;
                var fileName = Path.GetFileNameWithoutExtension(nemoFiles.fileName);
                #region processing on exe file

                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/exeFile/nemo_test.exe");
                // string pathOfNmf = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/exeFile/ping.nmf");
                string pathOfNmf = System.Web.Hosting.HostingEnvironment.MapPath(filePath);


                //  string pathOfTsv = System.Web.Hosting.HostingEnvironment.MapPath(nemoFiles.filePath + "/" + fileName + ".tsv");
                string pathOfCsv = System.Web.Hosting.HostingEnvironment.MapPath(nemoFiles.filePath + "/" + fileName + ".csv");
                //// string pathOfCsv = System.Web.Hosting.HostingEnvironment.MapPath(nemoFiles.filePath);

                string finalPathForTsvFile = path + " " + pathOfNmf + " " + pathOfCsv + " -siteacceptance";

                //StringBuilder finalPathForTsvFile = new StringBuilder();

                //finalPathForTsvFile.Append('"' + path + '"' + " ");
                //finalPathForTsvFile.Append('"' + pathOfNmf + '"' + " ");
                //finalPathForTsvFile.Append('"' + pathOfTsv + '"' + " ");
                //finalPathForTsvFile.Append(" -siteacceptance");



                //int ExitCode;
                //ProcessStartInfo ProcessInfo;
                //Process Process;
                //ProcessInfo = new ProcessStartInfo("cmd.exe", "/C " + finalPathForTsvFile);
                //ProcessInfo.CreateNoWindow = false;
                //ProcessInfo.UseShellExecute = false;
                //Process = Process.Start(ProcessInfo);
                //Process.WaitForExit(1500);
                //ExitCode = Process.ExitCode;
                //Process.Close();

                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c " + finalPathForTsvFile; // Note the /c command (*)
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();
                //* Read the output (or the error)
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
                string err = process.StandardError.ReadToEnd();
                Console.WriteLine(err);
                process.WaitForExit();

                //#endregion

                //#region write csv file from tsv file
                //var lines = System.IO.File.ReadAllLines(pathOfTsv);
                //var csv = lines.Select(row => string.Join(",", row.Split('\t')));
                //System.IO.File.WriteAllLines(pathOfCsv, csv);
                bool av_logsInot = true;

                #endregion

                if (File.Exists(pathOfCsv))
                {
                    using (Entities dbContext = new Entities())
                    {
                        #region file process on csv file
                        List<AV_NemoSiteLogs> fileTsvList = new List<AV_NemoSiteLogs>();
                        using (StreamReader sr = new StreamReader(pathOfCsv))
                        {

                            string[] headers = sr.ReadLine().Split(',');
                            string fileType = "";


                            #region For all data read from csv file and would save into database
                            while (!sr.EndOfStream)
                            {
                                AV_NemoSiteLogs tsfFileinfo = new AV_NemoSiteLogs();
                                string[] rows = sr.ReadLine().Split(',');

                                #region save file information which will receive from other server
                                if (av_logsInot)
                                {
                                    fileType = rows[0];
                                    AV_LogsInfo fileInfo = new AV_LogsInfo();
                                    fileInfo.fileName = fileType;
                                    fileInfo.pathFile = filePath;
                                    fileInfo.networkModeID = networkModeID;
                                    fileInfo.siteID = siteID;
                                    fileInfo.sectorID = sectorID;
                                    fileInfo.bandID = bandID;
                                    fileInfo.carrierID = carrierID;
                                    fileInfo.scopeID = scopeID;
                                    fileInfo.createDate = DateTime.Now;
                                    fileInfo.fileType = fileType;
                                    dbContext.AV_LogsInfo.Add(fileInfo);
                                    dbContext.SaveChanges();
                                    file_ID = fileInfo.fileID;
                                    av_logsInot = false;
                                }
                                #endregion

                                for (int i = 1; i < rows.Length; i++)
                                {
                                    #region for add rows of csv file and common fields
                                    switch (i)
                                    {
                                        case 1:
                                            tsfFileinfo.Time = rows[i].ToString();
                                            break;
                                        case 2:
                                            tsfFileinfo.Current_System = rows[i];
                                            break;
                                        case 3:
                                            if (rows[i] != "")
                                                tsfFileinfo.Longitude = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 4:
                                            if (rows[i] != "")
                                                tsfFileinfo.Latitude = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 5:
                                            tsfFileinfo.Cell = rows[i].ToString();
                                            break;
                                        case 6:
                                            tsfFileinfo.Band = rows[i].ToString();
                                            break;
                                        case 7:
                                            tsfFileinfo.Channel = rows[i].ToString();
                                            break;
                                        case 8:
                                            if (rows[i] != "")
                                                tsfFileinfo.PCI_PN = Convert.ToInt32(rows[i]);
                                            break;
                                        case 9:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSSI_dBm = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 10:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRP_RSCP_dBm = rows[i];
                                            break;
                                        case 11:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRQ_Ec_I0_db = rows[i];
                                            break;
                                        case 12:
                                            if (rows[i] != "")
                                                tsfFileinfo.SNR = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 13:
                                            tsfFileinfo.Cell1 = rows[i].ToString();
                                            break;
                                        case 14:
                                            tsfFileinfo.Band1 = rows[i].ToString();
                                            break;
                                        case 15:
                                            tsfFileinfo.Channel1 = rows[i].ToString();
                                            break;
                                        case 16:
                                            if (rows[i] != "")
                                                tsfFileinfo.PCI_PN1 = Convert.ToInt32(rows[i]);
                                            break;
                                        case 17:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSSI_dBm1 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 18:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRP_RSCP_dBm1 = rows[i];
                                            break;
                                        case 19:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRQ_Ec_I0_db1 = rows[i];
                                            break;
                                        case 20:
                                            if (rows[i] != "")
                                                tsfFileinfo.SNR1 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 21:
                                            tsfFileinfo.Cell2 = rows[i].ToString();
                                            break;
                                        case 22:
                                            tsfFileinfo.Band2 = rows[i].ToString();
                                            break;
                                        case 23:
                                            tsfFileinfo.Channel2 = rows[i].ToString();
                                            break;
                                        case 24:
                                            if (rows[i] != "")
                                                tsfFileinfo.PCI_PN2 = Convert.ToInt32(rows[i]);
                                            break;
                                        case 25:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSSI_dBm2 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 26:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRP_RSCP_dBm2 = rows[i];
                                            break;
                                        case 27:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRQ_Ec_I0_db2 = rows[i];
                                            break;
                                        case 28:
                                            if (rows[i] != "")
                                                tsfFileinfo.SNR2 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 29:
                                            tsfFileinfo.Cell3 = rows[i].ToString();
                                            break;
                                        case 30:
                                            tsfFileinfo.Band3 = rows[i].ToString();
                                            break;
                                        case 31:
                                            tsfFileinfo.Channel3 = rows[i].ToString();
                                            break;
                                        case 32:
                                            if (rows[i] != "")
                                                tsfFileinfo.PCI_PN3 = Convert.ToInt32(rows[i]);
                                            break;
                                        case 33:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSSI_dBm3 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 34:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRP_RSCP_dBm3 = rows[i];
                                            break;
                                        case 35:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRQ_Ec_I0_db3 = rows[i];
                                            break;
                                        case 36:
                                            if (rows[i] != "")
                                                tsfFileinfo.SNR3 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 37:
                                            tsfFileinfo.Cell4 = rows[i].ToString();
                                            break;
                                        case 38:
                                            tsfFileinfo.Band4 = rows[i].ToString();
                                            break;
                                        case 39:
                                            tsfFileinfo.Channel4 = rows[i].ToString();
                                            break;
                                        case 40:
                                            if (rows[i] != "")
                                                tsfFileinfo.PCI_PN4 = Convert.ToInt32(rows[i]);
                                            break;
                                        case 41:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSSI_dBm4 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 42:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRP_RSCP_dBm4 = rows[i];
                                            break;
                                        case 43:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRQ_Ec_I0_db4 = rows[i];
                                            break;
                                        case 44:
                                            if (rows[i] != "")
                                                tsfFileinfo.SNR4 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 45:
                                            tsfFileinfo.Cell5 = rows[i].ToString();
                                            break;
                                        case 46:
                                            tsfFileinfo.Band5 = rows[i].ToString();
                                            break;
                                        case 47:
                                            tsfFileinfo.Channel5 = rows[i].ToString();
                                            break;
                                        case 48:
                                            if (rows[i] != "")
                                                tsfFileinfo.PCI_PN5 = Convert.ToInt32(rows[i]);
                                            break;
                                        case 49:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSSI_dBm5 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 50:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRP_RSCP_dBm5 = rows[i];
                                            break;
                                        case 51:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRQ_Ec_I0_db5 = rows[i];
                                            break;
                                        case 52:
                                            if (rows[i] != "")
                                                tsfFileinfo.SNR5 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 53:
                                            tsfFileinfo.Cell6 = rows[i].ToString();
                                            break;
                                        case 54:
                                            tsfFileinfo.Band6 = rows[i].ToString();
                                            break;
                                        case 55:
                                            tsfFileinfo.Channel6 = rows[i].ToString();
                                            break;
                                        case 56:
                                            if (rows[i] != "")
                                                tsfFileinfo.PCI_PN6 = Convert.ToInt32(rows[i]);
                                            break;
                                        case 57:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSSI_dBm6 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 58:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRP_RSCP_dBm6 = rows[i];
                                            break;
                                        case 59:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRQ_Ec_I0_db6 = rows[i];
                                            break;
                                        case 60:
                                            if (rows[i] != "")
                                                tsfFileinfo.SNR6 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 61:
                                            tsfFileinfo.Cell7 = rows[i].ToString();
                                            break;
                                        case 62:
                                            tsfFileinfo.Band7 = rows[i].ToString();
                                            break;
                                        case 63:
                                            tsfFileinfo.Channel7 = rows[i].ToString();
                                            break;
                                        case 64:
                                            if (rows[i] != "")
                                                tsfFileinfo.PCI_PN7 = Convert.ToInt32(rows[i]);
                                            break;
                                        case 65:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSSI_dBm7 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 66:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRP_RSCP_dBm7 = rows[i];
                                            break;
                                        case 67:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRQ_Ec_I0_db7 = rows[i];
                                            break;
                                        case 68:
                                            if (rows[i] != "")
                                                tsfFileinfo.SNR7 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 69:
                                            tsfFileinfo.Cell8 = rows[i].ToString();
                                            break;
                                        case 70:
                                            tsfFileinfo.Band8 = rows[i].ToString();
                                            break;
                                        case 71:
                                            tsfFileinfo.Channel8 = rows[i].ToString();
                                            break;
                                        case 72:
                                            if (rows[i] != "")
                                                tsfFileinfo.PCI_PN8 = Convert.ToInt32(rows[i]);
                                            break;
                                        case 73:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSSI_dBm8 = Convert.ToDecimal(rows[i]);
                                            break;
                                        case 74:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRP_RSCP_dBm8 = rows[i];
                                            break;
                                        case 75:
                                            if (rows[i] != "")
                                                tsfFileinfo.RSRQ_Ec_I0_db8 = rows[i];
                                            break;
                                        case 76:
                                            if (rows[i] != "")
                                                tsfFileinfo.SNR8 = Convert.ToDecimal(rows[i]);
                                            break;
                                        default:
                                            break;
                                    }
                                    #endregion

                                    #region add dynamic field on type base
                                    if (fileType == "PING")
                                    {
                                        if (i == 77)
                                        {
                                            tsfFileinfo.Ping_Size_bytes = rows[i].ToString();
                                        }
                                        if (i == 78)
                                        {
                                            tsfFileinfo.RTT_ms = rows[i];
                                        }
                                    }
                                    if (fileType == "CONNECTION_SETUP")
                                    {
                                        if (i == 77)
                                        {
                                            tsfFileinfo.Connection_Setup_Time_ms = rows[i].ToString();
                                        }

                                    }

                                    if (fileType == "DL" || fileType == "UL")
                                    {
                                        if (i == 77)
                                        {
                                            tsfFileinfo.PDSCH_DL_Throughput_bits = rows[i].ToString();
                                        }
                                        if (i == 78)
                                        {
                                            tsfFileinfo.PUSCH_UL_Throughput_bits = rows[i].ToString();
                                        }
                                    }


                                    if (fileType == "MO" || fileType == "MT")
                                    {
                                        if (i == 77)
                                        {
                                            tsfFileinfo.Direction = rows[i].ToString();
                                        }
                                        if (i == 78)
                                        {
                                            tsfFileinfo.Event = rows[i];
                                        }
                                        if (i == 79)
                                        {
                                            tsfFileinfo.EventFields = rows[i];
                                        }
                                        //if (i == 70)
                                        //{
                                        //    tsfFileinfo.MO_MTPhone_Number = rows[i];
                                        //}
                                        //if (i == 71)
                                        //{
                                        //    tsfFileinfo.Call_Connection_Status = rows[i];
                                        //}
                                    }

                                    if (fileType == "CW" || fileType == "CCW")
                                    {
                                        if (i == 77)
                                        {
                                            tsfFileinfo.HO_Type = rows[i].ToString();
                                        }
                                        if (i == 78)
                                        {
                                            tsfFileinfo.Current_Channel = rows[i];
                                        }
                                        if (i == 79)
                                        {
                                            tsfFileinfo.Current_PCI = rows[i];
                                        }
                                        if (i == 80)
                                        {
                                            tsfFileinfo.Current_Band = rows[i];
                                        }
                                        if (i == 81)
                                        {
                                            tsfFileinfo.Attempted_System = rows[i];
                                        }
                                        if (i == 82)
                                        {
                                            tsfFileinfo.Attempted_Channel = rows[i];
                                        }
                                        if (i == 83)
                                        {
                                            tsfFileinfo.Attempted_PCI = rows[i];
                                        }
                                        if (i == 84)
                                        {
                                            tsfFileinfo.Attempted_Band = rows[i];
                                        }
                                        if (i == 85)
                                        {
                                            tsfFileinfo.HO_Duration_ms = rows[i];
                                        }
                                        if (i == 86)
                                        {
                                            tsfFileinfo.HO_Uplane_interruption_ms = rows[i];
                                        }
                                    }
                                }
                                #endregion
                                tsfFileinfo.fileID_Fk = file_ID;
                                dbContext.AV_NemoSiteLogs.Add(tsfFileinfo);
                            }
                            dbContext.SaveChanges();
                            #endregion
                        }
                        #endregion
                    }

                    using (Entities db = new Entities())
                    {
                        // db.AV_ProcessNemoLogs1(fkOffile);
                        DbCommand cmd = db.Database.Connection.CreateCommand();
                        cmd.CommandText = "[dbo].[AV_ProcessNemoLogs]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FileID", file_ID));
                        cmd.Connection.Open();
                        var res = cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    resp.Status = "true";
                    resp.Message = "The process has been compeleted";

                }
                else
                {

                    resp.Status = "false";
                    resp.Message = "Sorry there was an error! please try again1";

                }
            }
            catch (Exception ex)
            {
                Entities dbContext = new Entities();
                var AV_NemoSiteLogs = dbContext.AV_NemoSiteLogs.Where(x => x.fileID_Fk == file_ID).ToList();
                var AV_LogsInfo = dbContext.AV_LogsInfo.Find(file_ID);
                if (AV_NemoSiteLogs.Count() > 0)
                    dbContext.AV_NemoSiteLogs.RemoveRange(AV_NemoSiteLogs);
                if (AV_LogsInfo != null)
                    dbContext.AV_LogsInfo.Remove(AV_LogsInfo);
                dbContext.SaveChanges();
                resp.Status = "false";
                resp.Message = "Sorry there was an error! please try again";

                //return resp;
            }


            return resp;


        }









        //[Route("swi/NemoLogsTest"), HttpPost]
        ////public HttpResponseMessage addCsvFile(int siteID, int sectorID, int networkModeID, int bandID, int carrierID, int scopeID, string filePath)
        //public HttpResponseMessage addCsvFileTest(AV_NemoFiles nemoFiles)
        //{

        //    int siteID = 0;
        //    int sectorID = 0;
        //    int networkModeID = 0;
        //    int bandID = 0;
        //    int carrierID = 0;
        //    int scopeID = 0;
        //    string filePath = "";

        //    siteID = nemoFiles.siteID;
        //    sectorID = nemoFiles.sectorID;
        //    networkModeID = nemoFiles.networkModeID;
        //    bandID = nemoFiles.bandID;
        //    carrierID = nemoFiles.carrierID;
        //    scopeID = nemoFiles.scopeID;
        //    filePath = nemoFiles.filePath + "'" + nemoFiles.networkMode + "_" + nemoFiles.band + "_" + nemoFiles.carrier + "'/" + nemoFiles.fileName;



        //    string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/exeFile/nemo_test.exe");
        //    string pathOfNmf = System.Web.Hosting.HostingEnvironment.MapPath(filePath);


        //    string pathOfTsv = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/CsvTsvFiles/test.tsv");
        //    string pathOfCsv = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/CsvTsvFiles/logsFile.csv");

        //    string finalPathForTsvFile = path + " " + pathOfNmf + " " + pathOfTsv + " -siteacceptance";


        //    HttpResponseMessage response = new HttpResponseMessage();
        //    response.Content = new StringContent(finalPathForTsvFile);
        //    return response;
        //}


        [Route("swi/workorder/ResetPin"), HttpPost]
        public DataTable ResetPin(string Filter, string UserId, string IMEI)
        {
            AV_SiteTestSummaryDL siteNetLayers = new AV_SiteTestSummaryDL();

            Sec_UserSettingsDL secUserSetting = new Sec_UserSettingsDL();
            try
            {
                DataTable dt = secUserSetting.GetDataTable(Filter, UserId, IMEI, null, null);

                if (dt.Rows.Count > 0)
                {
                    string _UserId = dt.Rows[0]["UserId"].ToString();
                    string _IMEI = dt.Rows[0]["IMEI"].ToString();
                    string _EPin = dt.Rows[0]["EPin"].ToString();
                    string _TPin = dt.Rows[0]["TPin"].ToString();
                    string _ToEmail = dt.Rows[0]["Email"].ToString();  //"msraza_173@yahoo.com";// usr.Email;


                    string Subject = "Pin Code Reset";
                    string Body = " " +
                         "<p>New Pin Code</p>" +
                         "<table border=" + 1 + " cellpadding=" + 2 + " cellspacing=" + 0 + " width = " + 400 + ">" +
                         "<tr bgcolor='#F5F5F5'><td>Tester Id</td><td>" + _UserId + "</td></tr>" +
                         "<tr bgcolor='#F5F5F5'><td>IMEI</td><td>" + _IMEI + "</td></tr>" +
                         "<tr bgcolor='#F5F5F5'><td>EPIN</td><td>" + _EPin + "</td></tr>" +
                         "<tr bgcolor='#F5F5F5'><td>TPIN</td><td>" + _TPin + "</td></tr>" +
                         "</table>";

                    Thread thread = new Thread(() => SendEmail(Subject, _ToEmail, Body));
                    thread.Start();

                }

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void SendEmail(string Subject, string ToEmail, string Body)
        {
            WebConfig wc = new WebConfig();
            string SmtpServer = wc.AppSettings("SmtpServer");
            string SmtpServerPort = wc.AppSettings("SmtpServerPort");
            string FromEmail = wc.AppSettings("FromEmail");
            string FromEmailPassword = wc.AppSettings("FromEmailPassword");

            //string ToEmail = wc.AppSettings("ToEmail");

            FromEmailPassword = Encryption.Decrypt(FromEmailPassword, true);

            Email em = new Email(SmtpServer, Convert.ToInt32(SmtpServerPort), FromEmail, FromEmailPassword);
            try
            {
                if (SmtpServer != null && SmtpServerPort != null && FromEmail != null && FromEmailPassword != null && ToEmail != null)
                {
                    em.SendEmail(Subject, Body, ToEmail);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}