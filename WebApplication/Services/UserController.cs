namespace WebApplication.Services
{
    using AirView.DBLayer.AirView.BLL;
    using SWI.AirView.Common;
    using SWI.AirView.Models;
    using SWI.Libraries.AD.BLL;
    using SWI.Libraries.AD.Entities;
    using SWI.Libraries.Common;
    using SWI.Libraries.Security.BLL;
    using SWI.Libraries.Security.DAL;
    using SWI.Libraries.Security.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using WI.Libraries.Common;

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        
        [Route("swi/device/login"), HttpPost]
        public Sec_User DeviceLogin(string username, string password, string imei)
        {
            Sec_User rec = new Sec_User();
            try
            {
                Sec_UserBL ubl = new Sec_UserBL();
                rec = ubl.Single("DeviceLogin", username, imei);
                if (rec != null)
                {
                    Sec_PermissionBL pl = new Sec_PermissionBL();
                    rec.Permissions = pl.ToList("byUserId_ModuleId", rec.UserId.ToString(), "AIRVIEW_ANDROID");
                    string TempPass = Encryption.Decrypt(rec.Password, true);
                    rec.Message = true;
                    if (password != TempPass)
                    {
                        rec = new Sec_User();
                        rec.Message = false;
                    }
                }
                else
                {
                    rec.Message = false;
                }
            }
            catch (Exception ex)
            {
                rec = new Sec_User();
                rec.Message = false;

            }

            return rec;
        }

    
        [Route("swi/WorkOrder/ChangePassword"), HttpPost]
        public Response ChangePassword(string UserId, string IMEI, string CurrentPassword, string NewPassword)
        {
            Response r = new Response();
            try
            {
                CurrentPassword = Encryption.Encrypt(CurrentPassword, true);
                NewPassword = Encryption.Encrypt(NewPassword, true);
                Sec_UserDL ud = new Sec_UserDL();

                ud.Manage("ChangePassword", UserId, IMEI, CurrentPassword, NewPassword);

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

       
        [Route("swi/Tester/list"), HttpPost]
        public List<Sec_User> UsersList()
        {

            try
            {
                Sec_UserBL ub = new Sec_UserBL();
                return ub.ToList("byRoleName", "Tester").Select(m => new Sec_User { UserName = m.FirstName + " " + m.LastName, Id = Convert.ToInt32(m.UserId) }).ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }



        [Route("swi/Users/list"), HttpGet]
        public List<Sec_User> GetUsersList()
        {

            try
            {
                Sec_UserBL ub = new Sec_UserBL();
                return ub.ToList("UsersForBPMN").Select(m => new Sec_User { UserName = m.FirstName + " " + m.LastName, Id = Convert.ToInt32(m.UserId) }).ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }


        [Route("swi/set/Token"), HttpPost]
        public Response ChangeToken(string IMEI, string Token)
        {
            Response r = new Response();
            try
            {
                AD_UserEquipmentsBL ueb = new AD_UserEquipmentsBL();
                AD_UserEquipment ue = new AD_UserEquipment();
                ue.SerialNo = IMEI;
                ue.Token = Token;

                ueb.Manage("Set_Token", ue);

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

     
        [Route("swi/User/Applications"), HttpPost]
        public DataTable UserApplications(Int64 UserId)
        {

            try
            {
                Sec_UserSettingsDL udl = new Sec_UserSettingsDL();
                return udl.GetDataTable("UserApplications", UserId.ToString());

            }
            catch (Exception)
            {
                throw;

            }
        }

      
        [Route("swi/User/ApplicationRequest"), HttpPost]
        public Response ApplicationRequest(List<Sec_UserApplicaton> app)
        {
            Response r = new Response();

            try
            {
                Sec_UserSettingsDL usd = new Sec_UserSettingsDL();
                bool Result = false;

                foreach (var item in app)
                {
                    Result = usd.Manage("Set_IsRequested", item.UserId, item.ApplicationId, item.ApplicationId.ToString(),item.IMEI);

                    if (Result)
                    {
                        r.Status = "success";
                        r.Message = "success";
                    }
                    else
                    {
                        r.Status = "error";
                        r.Message = "record not found.";
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



      
        [Route("swi/User/DownloadAppBundle"), HttpPost]
        public DataTable DownloadAppBundle(Int64 UserId, string AppId, string EPin, string TPin)
        {

            try
            {
                Sec_UserSettingsDL udl = new Sec_UserSettingsDL();
                return udl.GetDataTable("Download_App_Bundle", AppId, UserId.ToString(), EPin, TPin);



            }
            catch (Exception)
            {
                throw;

            }
        }

      
        [Route("swi/User/PendingApps"), HttpPost]
        public DataTable PendingApps(Int64 UserId)
        {

            try
            {
                Sec_UserSettingsDL udl = new Sec_UserSettingsDL();

                return udl.GetDataTable("Pendding_Apps", UserId.ToString(), null, null);

            }
            catch (Exception)
            {
                throw;

            }
        }



        [Route("swi/Approve/AppRequest"), HttpPost]
        public Response ApproveAppRequest(Int64 AppId,Int64 UserId)
        {
            Response r = new Response();
            try
            {

                FireBase fb = new FireBase();
                WebConfig wc = new WebConfig();
                string FirebaseKey = wc.AppSettings("AirViewStore");
                Sec_UserSettingsDL usd = new Sec_UserSettingsDL();
                bool  Result = usd.Manage("Set_IsRequestApproved", UserId, AppId, AppId.ToString(),null);

                if (Result)
                {

                    var dt = usd.GetDataTable("Get_UserAppToken", UserId.ToString(), AppId.ToString());
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        dynamic result = fb.SendNotification(FirebaseKey, dt.Rows[0]["Token"].ToString(), "{ \"EmailPIN\": \""+ dt.Rows[0]["EmailPIN"].ToString() + "\", \"MobilePIN\": \"" + dt.Rows[0]["MobilePIN"].ToString() + "\"}");

                        r.Status = "success";
                        r.Message = "success";
                         r.Value = result;

                    }
                    else
                    {
                        r.Status = "error";
                        r.Message = "Device not found.";
                    }
                }
                else
                {
                    r.Status = "error";
                    r.Message = "Request not fund.";
                }
               

                

               

            }
            catch (Exception ex)
            {

                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;
        }


        [Route("swi/Applicaton/IsDownloaded"), HttpPost]
        public Response ApplicatonIsDownloaded(Int64 UserId, Int64 AppId)
        {
            Response r = new Response();
            try
            {
                Sec_UserSettingsDL usd = new Sec_UserSettingsDL();
                usd.Manage("Set_IsDownloaded", UserId, AppId, AppId.ToString(), null);
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


        [Route("swi/User/PendingNotifications"), HttpPost]
        public DataTable PendingNotifications(Int64 UserId)
        {

            try
            {
                Sec_UserSettingsDL udl = new Sec_UserSettingsDL();

                return udl.GetDataTable("Pending_Notifications", UserId.ToString(), null, null);

            }
            catch (Exception)
            {
                throw;

            }
        }


        [Route("swi/User/GetSectorColor"), HttpPost]
        public HttpResponseMessage SectorColors(Int64 UserId)
        {
            try
            {
                AV_SectorColorBL sCol = new AV_SectorColorBL();
                var result= sCol.ToList("GetSectorColors", UserId);
                if (result != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Record with Id " + UserId.ToString() + " not found");
                }
            }
            catch (Exception)
            {
                throw;

            }
        } 
    }
}
