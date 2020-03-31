using System.Web.Mvc;
using SWI.Security.Filters;
using AirView.DBLayer.Fleet.BLL;
using System.Linq;
using AirView.DBLayer.Fleet.Model;
using System.Net;
using System.Collections.Generic;
using System;
using SWI.Libraries.Common;
using System.Web;
using SWI.AirView.Common.ResizeUploadImg;
using System.IO;
using Microsoft.AspNet.SignalR;
using System.Net.Sockets;
using SWI.AirView.Common;
using System.Text;
using System.Threading;
using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.AirView.Entities;
using WebApplication.Areas.Alert.SRNotificationHub;
using Newtonsoft.Json;
using System.Net.Mail;
using SWI.Libraries.Security.Entities;

namespace WebApplication.Areas.Fleet.Controllers
{
    [IsLogin, ErrorHandling, HandleError]
    public class VehicleController : Controller

    {
        TcpClient tcpclnt;
        FM_VehicleBL model = new FM_VehicleBL();
        AL_AlertBL AlertModel = new AL_AlertBL();
        string ErrorMsg = "";

        // GET: Fleet/Vehicle
        [IsLogin(CheckPermission = false)]
        public ActionResult Index()
        {
            //@ViewBag.VehicleEntry = "Initiated";
            FM_Vehicle modelBind = new FM_Vehicle();            
            return View(modelBind);
        }

        private string ApiMapKey()
        {
            WebConfig wc = new WebConfig();
            string MapKey = wc.AppSettings("ApiMapKey").ToString();
            return MapKey;
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult SignalR()
        {
            //@ViewBag.VehicleEntry = "Initiated";
            return View();
        }

        [IsLogin(CheckPermission = true), HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FM_Vehicle modelBind)
        {
            if (ModelState.IsValid)
            {
                string Picture = null;
                FM_Vehicle modelResult = new FM_Vehicle();

                Picture = UploadImg("/Content/Images/Vehicles/Original/", "v-" + modelBind.ChassisNumber.Replace(" ", "_"), 150, 150);
                string Thumb = (!string.IsNullOrEmpty(Picture)) ? UploadImg("/Content/Images/Vehicles/Thumb/", "v-" + modelBind.ChassisNumber.Replace(" ", "_"), 32, 32) : null;
                if (!string.IsNullOrEmpty(Picture))
                {
                    // ud.Manage("UpdatePicture", Id.ToString(), Picture);xyz.Replace("  ", string.empty);
                    modelResult = model.Insert_Vehicle("Insert_Vehicle_With_Image", modelBind.TypeId, modelBind.ManuId, modelBind.ModelId, modelBind.SubModelId, modelBind.Year, modelBind.ChassisNumber, modelBind.RegistrationNumber, true, Picture.Replace(" ", "_"), modelBind.VehicleGroupId , modelBind.IMEIId);
                }
                else
                {
                    modelResult = model.Insert_Vehicle("Insert_Vehicle", modelBind.TypeId, modelBind.ManuId, modelBind.ModelId, modelBind.SubModelId, modelBind.Year, modelBind.ChassisNumber, modelBind.RegistrationNumber, true, modelBind.VehicleGroupId, modelBind.IMEIId);
                }

                ViewBag.VehicleEntry = "True";
                TempData["EditResp"] = "CreatedSuccessfully";

                /* == Insert Notification When Create New Vehicle Start == */
                AL_SetNotification DumyObject = new AL_SetNotification();
                DumyObject.AlertConfigId = 7;
                DumyObject.EntityId = modelResult.VehicleId;
                DumyObject.Notification = "Vehicle \""+ modelBind.RegistrationNumber + "\" Created.";
                DumyObject.AlertRecieverId = 11;
                DumyObject.UserId = 29;
                DumyObject.IsPushAlertSent = 0;
                DumyObject.IsPushAlertRead = 0;
                DumyObject.IsEmailAlertSent = 0;

                AlertModel.SendNotification("InsertNotification", DumyObject);
                /* == Insert Notification When Create New Vehicle End == */

                return RedirectToAction("Edit", new { id = modelResult.VehicleId});
            }
            else
            {
                ViewBag.VehicleEntry = "False";

                ViewBag.VehicleGroupId = modelBind.VehicleGroupId;
                ViewBag.ManuId = modelBind.ManuId;
                ViewBag.TypeId = modelBind.TypeId;
                ViewBag.ModelId = modelBind.ModelId;
                ViewBag.SubModelId = modelBind.SubModelId;
                ViewBag.IMEI = modelBind.IMEI;
            }

            return View(modelBind);

        }
        [IsLogin(CheckPermission = false)]
        public string UploadImg(string UploadPath, string file_name, int height, int width)
        {

            DirectoryHandler dir = new DirectoryHandler();
            dir.CreateDirectory(Server.MapPath("~" + UploadPath));

            foreach (string item in Request.Files)
            {
                int counter = 1;
                string tempUploadPath = "~" + UploadPath;
                HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
                if (file.ContentLength == 0)
                    continue;

                if (file.ContentLength > 0)
                {

                    ImageUpload imageUpload = new ImageUpload { Width = width, Height = height };
                    ImageResult imageResult = new ImageResult();
                    string fileName = Path.GetFileName(file.FileName);
                    if (file_name == null)
                    {

                        string prepend = "Sec_";
                        string finalFileName = prepend + ((counter).ToString()) + "_" + fileName;
                        imageResult = imageUpload.UploadFile(file, finalFileName, tempUploadPath);
                    }

                    if (!string.IsNullOrEmpty(file_name))
                    {
                        var Extension = Path.GetExtension(file.FileName);
                        imageResult = imageUpload.UploadFile(file, file_name + Extension, tempUploadPath);


                    }



                    if (imageResult.Success)
                    {
                        return imageResult.ImageName;
                    }
                    else
                    {
                        return imageResult.ErrorMessage;
                    }
                }
            }

            return null;
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

          

            FM_Vehicle modelList = model.Get_EditVehicle("Get_EditVehicle", id);

            if (modelList == null)
            {
                return Redirect("/Error/index?type=noUrlFound");
            }

            ViewBag.EditResponse = "Initiated";
            if (TempData["EditResp"] != null)
            {
                ViewBag.EditResponse =  "CreatedSuccessfully";                
                TempData.Remove("EditResp");
            }
            
            
            return View(modelList);
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult IMEIForTcpCommands(List<string> id,string Command)
        {
            try
            {
                String IMEI = id[0].ToString();
                String[] IMEICollection = IMEI.Split(',');

                for (int counter=0;counter < IMEICollection.Length; counter++)
                {
                    if (!string.IsNullOrEmpty(IMEICollection[counter]))
                    {

                        bool ConnectionEstb = CreateTCPClient();
                        if (!ConnectionEstb)
                        {
                            //Connection Failed
                            return this.Json(new { success = false, message = ErrorMsg.ToString() });
                        }

                        bool retval =ServerCommunication(IMEICollection[counter].ToString(), Command);
                        if (!retval)
                        {
                            //failure message removed as per requirement.
                            return this.Json(new { success = false, message = ErrorMsg.ToString() });
                        }
                        Thread.Sleep(500);
                    }
                }

                return this.Json(new { success = true, message = "Success" });
            }
            catch(Exception ex)
            {
                return this.Json(new { success = false, message = ex.ToString() });
            }
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult SetGpsTimeIntervalRequest(List<string> id, string Command,string TimeInterval)
        {
            try
            {
                String IMEI = id[0].ToString();
                String[] IMEICollection = IMEI.Split(',');

                

                for (int counter = 0; counter < IMEICollection.Length; counter++)
                {
                    if (!string.IsNullOrEmpty(IMEICollection[counter]))
                    {
                        if (Command == "7500")
                        {
                            var usrid = Session["user"];
                            var userId = (LoginInformation)usrid;                            
                            model.Insert_TripIdleTime("Insert_UpdateTripIdleConfig", IMEICollection[counter].ToString(), userId.UserId, TimeInterval);
                        }

                        bool ConnectionEstb= CreateTCPClient();
                        if (!ConnectionEstb)
                        {
                            //Connection Failed
                            return this.Json(new { success = false, message = ErrorMsg.ToString() });
                        }
                        bool retval = ServerCommunication(IMEICollection[counter].ToString(), Command, TimeInterval);
                        if (!retval)
                        {
                            //failure message removed as per requirement.
                            return this.Json(new { success = false, message = ErrorMsg.ToString() });
                        }
                        Thread.Sleep(500);
                    }
                }

                return this.Json(new { success = true, message = "Success" });
            }
            catch (Exception ex)
            {
                return this.Json(new { success = false, message = ex.ToString() });
            }
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult WifiSettings(bool id)
        {
            if (id)
            {

            }
            
            return this.Json(new { success = true, message = "Success"});
        }
        public void sendEmail()
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.office365.com";//"smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("airview.report@swius.com", "F3bTKMZaC0W6Cyo5Bp5+6w==");

            MailMessage mm = new MailMessage("donotreply@domain.com", "junaid.shahid.1@gmail.com", "test", "test");
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
        }

        [IsLogin(CheckPermission = false),HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FM_Vehicle modelBind)
        {
            try
            {
                FM_Vehicle modelList = new FM_Vehicle();
                if (ModelState.IsValid)
                {
                    string Picture = null;
                    Picture = UploadImg("/Content/Images/Vehicles/Original/", "v-" + modelBind.ChassisNumber.Replace(" ", "_"), 150, 150);
                    string Thumb = (!string.IsNullOrEmpty(Picture)) ? UploadImg("/Content/Images/Vehicles/Thumb/", "v-" + modelBind.ChassisNumber.Replace(" ", "_"), 32, 32) : null;
                    if (!string.IsNullOrEmpty(Picture))
                    {
                        model.Update_Vehicle("Edit_Vehicle_With_Image", modelBind.TypeId, modelBind.ManuId, modelBind.ModelId, modelBind.SubModelId, modelBind.Year, modelBind.ChassisNumber, modelBind.RegistrationNumber, true, modelBind.VehicleId, Picture.Replace(" ", "_"), modelBind.VehicleGroupId);
                    }
                    else
                    {
                        model.Update_Vehicle("Edit_Vehicle", modelBind.TypeId, modelBind.ManuId, modelBind.ModelId, modelBind.SubModelId, modelBind.Year, modelBind.ChassisNumber, modelBind.RegistrationNumber, true, modelBind.VehicleId, modelBind.VehicleGroupId);
                    }
                    ViewBag.EditResponse = "Success";
                    modelList = model.Get_EditVehicle("Get_EditVehicle", modelBind.VehicleId);
                    //return RedirectToAction("List");
                    /* == Insert Notification When Create New Vehicle Start == */
                    AL_SetNotification DumyObject = new AL_SetNotification();
                    AL_GetNotification UserNotify = new AL_GetNotification();
                    DumyObject.AlertConfigId = 7;
                    DumyObject.EntityId = modelBind.VehicleId;
                    DumyObject.Notification = "Vehicle \"" + modelBind.RegistrationNumber + "\" Updated.";
                    DumyObject.AlertRecieverId = 11;
                    DumyObject.UserId = (int)ViewBag.UserId;
                    DumyObject.IsPushAlertSent = 0;
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


                return View(modelList);
            }
            catch (Exception ex)
            {
                return this.Json(new { success = false, message = ex.ToString() }); 
            }
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult List()
        {
            return View();
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult ListManufacturer(int Id)
        {
            var jsonReturn = model.Get_Vehicle("Get_Manufacturer", Id).ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult CheckVehiclePermission(int Id)
        {
            var jsonReturn = model.Check_Vehicle_Permission("FleetManagementPermission", Id);
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult ValidateChassisNumber(string Id, string RN, string IMEI)
        {
            var CN_Result = model.ValidateChassisNumber("Validate_ChassisNumber", Id);
            var RN_Result = model.ValidateRegistrationNumber("Validate_RegistrationNumber", RN);
            var IMEI_Result = model.ValidateRegistrationNumber("Validate_IMEI", IMEI);

            FM_Vehicle ReturnObject = new FM_Vehicle();
            if(CN_Result == 1)
            {
                ReturnObject.ChassisNumber = "1";
            }
            else
            {
                ReturnObject.ChassisNumber = "0";
            }

            if (RN_Result == 1)
            {
                ReturnObject.RegistrationNumber = "1";
            }
            else
            {
                ReturnObject.RegistrationNumber = "0";
            }

            if (IMEI_Result == 1)
            {
                ReturnObject.IMEI = "1";
            }
            else
            {
                ReturnObject.IMEI = "0";
            }

            return Json(ReturnObject, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult ValidateChassisNumberOnUpdate(string id, string RN,int VID)
        {
            var CN_Result = model.ValidateChassisNumberOnUpdation("Validate_ChassisNumber_OnUpd", id, VID);
            var RN_Result = model.ValidateRegistrationNumberOnUpdate("Validate_RegistrationNumber_OnUpd" , RN, VID);

            FM_Vehicle ReturnObject = new FM_Vehicle();
            if (CN_Result == 1)
            {
                ReturnObject.ChassisNumber = "1";
            }
            else
            {
                ReturnObject.ChassisNumber = "0";
            }

            if (RN_Result == 1)
            {
                ReturnObject.RegistrationNumber = "1";
            }
            else
            {
                ReturnObject.RegistrationNumber = "0";
            }

            return Json(ReturnObject, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult ListModel(int Id, int TypeId)
        {
            var jsonReturn = model.Get_ListModel("Get_VehicleModel", Id, TypeId).ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult ListSubModel(int Id, int ManuId)
        {
            var jsonReturn = model.Get_Vehicle("Get_VehicleSubModel", Id, ManuId).ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult ListIMEI()
        {
            var jsonReturn = model.Get_List_IMEI("List_IMEI").ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult CurrentIMEI(int Id)
        {
            var jsonReturn = model.Get_List_IMEI("List_IMEI_By_Id", Id);
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult GetCurrentIMEIWithoutHistory(int Id)
        {
            var jsonReturn = model.GetCurrentIMEIDetails("List_IMEI_By_IdWithoutHistory", Id);
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult SaveIMEI(int Id, int TrackerIMEI, string PreviousIMEI,string CurrentTrackerIMEI)
        {
            model.Update_Vehicle("Edit_Vehicle_For_Tracker", Id, TrackerIMEI);
            ViewBag.EditResponse = "Success";
            FM_Vehicle modelList = new FM_Vehicle();

            modelList = model.Get_EditVehicle("Get_EditVehicle_By_Id", Id);

            if (TrackerIMEI != 0)
            {
                model.Insert_VehicleTracker("Insert_FM_VehicleTrackerHistory", modelList.IMEIId, modelList.VehicleId);
            }
            else
            {
                FM_Vehicle temp = new FM_Vehicle();
                temp.IMEI = "";
                modelList = temp;
            }

            if (!(PreviousIMEI == CurrentTrackerIMEI))
            {
                bool ConnectionEstb = CreateTCPClient();

                if (ConnectionEstb)
                {
                    ServerCommunication(CurrentTrackerIMEI.ToString(), "1002", "", PreviousIMEI.ToString());
                }                
            }

            return Json(modelList, JsonRequestBehavior.AllowGet);
        }
  
        [IsLogin(CheckPermission = false)]
        public JsonResult ListIMEIAll(int id)
        {
            var jsonReturn = model.Get_List_IMEI_ByUserID("GetTrackersByUserID_Assigned", id).ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult ListType()
        {
            var jsonReturn = model.Get_Vehicle("Get_VehicleType").ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult ListGroup()
        {
            var jsonReturn = model.Get_Vehicle("List_VehicleGroup").ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult DeleteRecord(int Id)
        {
            var jsonReturn = model.Delete_Vehicle("Delete_Vehicle", Id);
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult ListDeleteRecord(List<FM_Vehicle> modelBind)
        {
            var jsonReturn = 0;
            for (int x = 0; x < modelBind.Count; x++)
            {
                jsonReturn = model.Delete_Vehicle("Delete_Vehicle", modelBind[x].VehicleId);
            }
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult SetStatus(int Id, int Status)
        {
            var jsonReturn = model.Set_Vehicle_Status("Set_Vehicle_Status", Id, Status);
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult ListSetStatus(List<FM_Vehicle> modelBind)
        {
            //var jsonReturn = model.Set_Vehicle_Status("Set_Vehicle_Status", Id, Status);
            //return Json(jsonReturn, JsonRequestBehavior.AllowGet);

            var jsonReturn = 0;
            for (int x = 0; x < modelBind.Count; x++)
            {
                    jsonReturn = model.Set_Vehicle_Status("Set_Vehicle_Status", modelBind[x].VehicleId, modelBind[x].TypeId);
            }
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult GetRecord(int Id)
        {
            var jsonReturn = model.Get_Record_Status("Get_Vehicle_List", Id).ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult GetRecordSearch(string Id)
        {
            var jsonReturn = model.Get_Record_Status("Get_Vehicle_List_Search", Id).ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult GetRecordSearchAssigned(string Id)
        {
            var jsonReturn = model.Get_Record_Status("Get_Vehicle_List_Search_Assigned", Id).ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult GetAssignedRecord()
        {
            var jsonReturn = model.Get_Assigned_Record_Status("Get_Assigned_Vehicle_List").ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult AssignSingleVehicle(FM_Vehicle_Assignment modelBind)
        {

            if (ModelState.IsValid)
            {
                var jsonReturn = model.Assign_Single_Vehicle("ASSIGN_VEHICLE_TO_SINGLE_DT", modelBind.UserId, modelBind.VehicleId, modelBind.DateAssign, modelBind.TrackerVal);
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonReturn = "Error!";
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
            
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult AssignListVehicle(List<FM_Vehicle_Assignment> modelBind)
        {
            var jsonReturn = 0;
            for (int x = 0; x < modelBind.Count; x++)
            {
                if (ModelState.IsValid)
                {
                     jsonReturn = model.Assign_Single_Vehicle("ASSIGN_VEHICLE_TO_SINGLE_DT", modelBind[x].UserId, modelBind[x].VehicleId, modelBind[x].DateAssign, modelBind[x].TrackerVal);
                   
                }
                else
                {
                     jsonReturn = 5;
                   
                }
            }
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);

        }
        [IsLogin(CheckPermission = false)]
        public JsonResult ReturnSingleVehicle(FM_Vehicle_Assignment modelBind)
        {

            if (ModelState.IsValid)
            {
                var jsonReturn = model.Return_Single_Vehicle("RETURN_VEHICLE_FROM_SINGLE_DT", modelBind.VehicleId, modelBind.DateReturn, modelBind.VehicleAssignmentId);

                if (modelBind.PreviousTracker != null)
                {
                    if (!String.IsNullOrEmpty(modelBind.PreviousTracker.Trim()))
                    {
                        bool ConnectionEstb = CreateTCPClient();

                        if (ConnectionEstb)
                        {
                            string CurrentTracker = "";
                            if (modelBind.NewTracker != null)
                            {
                                CurrentTracker = modelBind.NewTracker;
                            }
                            ServerCommunication(CurrentTracker, "1002", "", modelBind.PreviousTracker.ToString());
                        }
                    }
                }              

                return Json("Done!", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonReturn = "Error!";
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }

        }
        [IsLogin(CheckPermission = false)]
        public JsonResult ReturnListVehicle(List<FM_Vehicle_Assignment> modelBind)
        {
            var jsonReturn = 0;
            for (int x = 0; x < modelBind.Count; x++)
            {
                if (ModelState.IsValid)
                {
                    jsonReturn = model.Return_Single_Vehicle("RETURN_VEHICLE_FROM_SINGLE_DT", modelBind[x].VehicleId, modelBind[x].DateReturn, modelBind[x].VehicleAssignmentId);
                    LogOutCurrentTrackers(modelBind[x].NewTracker, modelBind[x].PreviousTracker);
                }
                else
                {
                    jsonReturn = 5;

                }
            }
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);

        }
        [IsLogin(CheckPermission = false)]
        public JsonResult TransferSingleVehicle(FM_Vehicle_Assignment modelBind)
        {

            if (ModelState.IsValid)
            {
                var jsonReturn = model.Transfer_Single_Vehicle("TRANSFER_VEHICLE_TO_SINGLE_DT", modelBind.UserId, modelBind.VehicleId, modelBind.DateAssign, modelBind.VehicleAssignmentId, modelBind.TrackerVal);

                if (modelBind.PreviousTracker != null)
                {
                    if (!(modelBind.PreviousTracker == modelBind.NewTracker))
                    {
                        if (!String.IsNullOrEmpty(modelBind.PreviousTracker.Trim()))
                        {
                            bool ConnectionEstb = CreateTCPClient();

                            if (ConnectionEstb)
                            {
                                string CurrentTracker = "";

                                if (modelBind.NewTracker != null)
                                {
                                    CurrentTracker = modelBind.NewTracker;
                                }

                                ServerCommunication(CurrentTracker, "1002", "", modelBind.PreviousTracker.ToString());
                            }
                        }
                    }
                }

                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonReturn = "Error!";
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }

        }


        [IsLogin(CheckPermission = false)]
        public JsonResult TransferListVehicle(List<FM_Vehicle_Assignment> modelBind)
        {
            var jsonReturn = 0;
            for (int x = 0; x < modelBind.Count; x++)
            {
                if (ModelState.IsValid)
                {
                    jsonReturn = model.Transfer_Single_Vehicle("TRANSFER_VEHICLE_TO_SINGLE_DT", modelBind[x].UserId, modelBind[x].VehicleId, modelBind[x].DateAssign, modelBind[x].VehicleAssignmentId, modelBind[x].TrackerVal);
                    LogOutCurrentTrackers(modelBind[x].NewTracker, modelBind[x].PreviousTracker);
                }
                else
                {
                    jsonReturn = 5;

                }
            }
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);

        }

        private bool LogOutCurrentTrackers(string NewTracker,string PreviousTracker)
        {
            try
            {
                if (PreviousTracker != null)
                {
                    if (!(PreviousTracker == NewTracker))
                    {
                        if (!String.IsNullOrEmpty(PreviousTracker.Trim()))
                        {
                            bool ConnectionEstb = CreateTCPClient();

                            if (ConnectionEstb)
                            {
                                string CurrentTracker = "";

                                if (NewTracker != null)
                                {
                                    CurrentTracker = NewTracker;
                                }

                                ServerCommunication(CurrentTracker, "1002", "", PreviousTracker.ToString());
                            }
                        }
                    }
                }
                return true;
            }
                
            catch(Exception ex)
            {
                return false;
            }                      
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult GetDTList()
        {
            var jsonReturn = model.GET_DT_LIST("GET_DT_LIST").ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult Tracking()
        {
            //@ViewBag.VehicleEntry = "Initiated";
            ViewBag.ApiMapKey = ApiMapKey();            
            return View();
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult Group()
        {
            return View();
        }

        [IsLogin(CheckPermission = false), HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Group(List<FM_VehicleGroup> modelBind)
        {
            for (int x = 0; x < modelBind.Count; x++)
            {
                if (ModelState.IsValid)
                {
                    model.List_Vehicle_Group("Insert_VehicleGroup", modelBind[x].Title, modelBind[x].Description, modelBind[x].IsActive, false);
                }
                else
                {
                   
                }
            }

            return View(modelBind);

        }
        [IsLogin(CheckPermission = false)]
        public JsonResult InsertListVehicleGroup(List<FM_VehicleGroup> modelBind)
        {
            var jsonReturn = 0;
            for (int x = 0; x < modelBind.Count; x++)
            {
               
                    if (modelBind[x].VehicleGroupId != 0)
                    {
                        jsonReturn = model.List_Vehicle_Group("Update_VehicleGroup", modelBind[x].Title, modelBind[x].Description, modelBind[x].IsActive, false, modelBind[x].VehicleGroupId);
                    }
                    else
                    {
                        jsonReturn = model.List_Vehicle_Group("Insert_VehicleGroup", modelBind[x].Title, modelBind[x].Description, modelBind[x].IsActive, false);
                    }

           
            }
            var jsonReturnList = model.GET_Group_LIST("Get_VehicleGroup").ToList();
            
            return Json(jsonReturnList, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult GetListVehicleGroup()
        {
            var jsonReturn = model.GET_Group_LIST("Get_VehicleGroup").ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult DeleteVehicleGroup(int id)
        {
            FM_VehicleGroup CurrentGroup = model.GET_Group_LIST("Get_VehicleGroup_By_Id", id);
            var jsonReturn = 0;
            if (CurrentGroup.IsAssign == false)
            {
                jsonReturn = model.Delete_VehicleGroup("Delete_VehicleGroup", id);
            }
            else
            {
                jsonReturn = 0;
            }
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult sendAPI(string name, string msg)
        {
  
            //var context = GlobalHost.ConnectionManager.GetHubContext<MapApi>();
            //context.Clients.All.addNewMessageToPage(name, msg);

            return Json("Message Sent !", JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false), ]
        public JsonResult CreateKML(int VehicleId, DateTime FromDate, DateTime ToDate , Nullable<bool> tripStatus = null)
        {
            FM_KML_VM finalObject = new FM_KML_VM();

            finalObject.KML = model.Get_KML("Get_Drive_Date_KML", VehicleId, FromDate, ToDate,tripStatus);
            string address = "/Areas/Fleet/TrackerKML/";
            DirectoryHandler dh = new DirectoryHandler();
            dh.CreateDirectory(Server.MapPath(address));
            System.IO.File.WriteAllText(Server.MapPath(address) + "TrackerDriveRoute" + ".kml", finalObject.KML.KML);
            finalObject.IsKML = true;
            /* == Fetch KML Information == */
            finalObject.Vehicle = model.Get_KML_details("Get_Drive_Date_KML_details", VehicleId, FromDate, ToDate);
            var jsonResult = Json(finalObject, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [IsLogin(CheckPermission = false),]
        public JsonResult DrivePlayDates(int VehicleId)
        {

            List<FM_RouteKML> objectKML = model.Get_DrivePlayDate("Get_Drive_Dates", VehicleId).ToList();
            

            return Json(objectKML, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult GetTrackersByUserID(int Id)
        {
            //var jsonReturn = model.Get_Vehicle("Get_Manufacturer", Id).ToList();
            var jsonReturn = model.GetTrackersByUserID("GetTrackersByUserID", Id).ToList();
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

        public bool ServerCommunication(string id,string ReqCommand,string TimeInterval="",string PreviousIMEI ="0", bool WifiStatus = false)
        {try
            {
                string IMEI = id;

                //if (tcpclnt == null)
                //{
                    
                //}               

                // %% - Command from WebClient
                // 22 - length of packet
                // 4101 - Command for on demand Gps Packet 3210 - to parse gps packet for the provided IMEI only
                // 1 - Number of IMEIs(TrackerID)   
                //6450703222 - IMEI #                    

                string Command = "%%,,4101,1,64507032770165";
                NetworkStream Stream = tcpclnt.GetStream();

                if (String.Equals(ReqCommand, "4101"))
                {
                    int CommandLth = Command.Length + 2;
                    Command = "%%," + CommandLth + "," + ReqCommand + ",1," + id;
                }
                else if(String.Equals(ReqCommand, "4102"))
                {

                    Command = "%%,"+ "," + ReqCommand + ",1," + id + ","+TimeInterval;
                    int CommandLth = Command.Length + 2;
                    Command = "%%," + CommandLth + "," + ReqCommand + ",1," + id + "," + TimeInterval;
                }
                else if(String.Equals(ReqCommand, "1002")) //Logout Command
                {
                    Command = "%%," + "," + ReqCommand + ",1," + id + "," + PreviousIMEI;
                    int CommandLth = Command.Length + 2;
                    Command = "%%," + CommandLth + "," + ReqCommand + ",1," + id + "," + PreviousIMEI;

                }
                else if (String.Equals(ReqCommand, "7500"))// Trip Idle Time
                {
                    Command = "%%," + "," + ReqCommand + ",1," + id + "," + TimeInterval;
                    int CommandLth = Command.Length + 2;
                    Command = "%%," + CommandLth + "," + ReqCommand + ",1," + id + "," + TimeInterval ;
                }
                else if (String.Equals(ReqCommand, "5870"))//Time Inerval as SSID ,PreviousIMEI as PWD
                {
                    Command = "%%," + "," + ReqCommand + ",1," + id + "," + Convert.ToInt16(WifiStatus)+ "," + TimeInterval + "," + PreviousIMEI;
                    int CommandLth = Command.Length + 2;
                    Command = "%%," + CommandLth + "," + ReqCommand + ",1," + id + "," + Convert.ToInt16(WifiStatus) + ","+ TimeInterval + "," + PreviousIMEI;
                }

                ASCIIEncoding AsciEncod = new ASCIIEncoding();
                byte[] AsciiByteColl = AsciEncod.GetBytes(Command);
                Stream.Write(AsciiByteColl, 0, AsciiByteColl.Length);

                byte[] SrvrRspnse = new byte[200];

                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.ToString();
                return false;
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
                ErrorMsg = ex.ToString();
                return false;
            }
        }
        [IsLogin(CheckPermission = false)]
        public JsonResult InsertWifiSettings(FM_TrackerWifiSetting wifimodel)
        {
            try
            {
                var result = model.Insert_WifiSettings("Insert_TrackerWifiSettings", wifimodel);
                bool ConnectionEstb = CreateTCPClient();
                if (wifimodel.WifiStatus)
                {
                    
                    if (!ConnectionEstb)
                    {
                        //Connection Failed
                        return this.Json(new { success = false, message = ErrorMsg.ToString() });
                    }

                    bool retval = ServerCommunication(wifimodel.TrackerID.ToString(), "5870",wifimodel.SSID,wifimodel.WifiPassword,wifimodel.WifiStatus);
                    if (!retval)
                    {
                        //failure message removed as per requirement.
                        return this.Json(new { success = false, message = ErrorMsg.ToString(), wifimodel.SSID, wifimodel.WifiPassword, wifimodel.WifiStatus });
                    }
                }
                else if (!wifimodel.WifiStatus)
                {
                    bool retval = ServerCommunication(wifimodel.TrackerID.ToString(), "5870","","", wifimodel.WifiStatus);
                }
                
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return this.Json(new { success = false, message = ex.ToString() });
            }
        }
    
        [IsLogin(CheckPermission = false)]
        public JsonResult GetWifiSettings(string trackerSerialNo)
        {
            try {
                var jsonReturn = model.GetWifiSettings("GetTrackerWifiSettings", trackerSerialNo);
                return Json(jsonReturn, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return this.Json(new { success = false, message = ex.ToString() });
            }
            
        }
    }
}