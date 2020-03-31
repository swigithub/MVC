using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.AirView.Entities;
using Newtonsoft.Json;
using SWI.AirView.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using WebApplication.Areas.Alert.SRNotificationHub;

namespace WebApplication.Services
{
    [EnableCors(origins: "http://localhost:18459", headers: "*", methods: "*")]
    public class AlertAPIController : ApiController
    {
        // GET: AlertAPI
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [System.Web.Http.Route("swi/Subscribe"), System.Web.Http.HttpPost]
        public HttpResponseMessage Subscribe(string KeyCode, int EntityId, string ConfigId, int UserId)
        {
            Response result = new Response();
            AL_AlertBL model = new AL_AlertBL();

            model.Subscribe("Subscribe_Insert", KeyCode, EntityId, ConfigId, UserId);

            result.Message = "Alerts Subscribed Succesfully!";
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //working
        [System.Web.Http.Route("swi/POSTUserAlertSubscription"), System.Web.Http.HttpPost]
        public HttpResponseMessage Subscription(AL_Subscription Subscription)
        {
            Response result = new Response();
            AL_AlertBL model = new AL_AlertBL();

            model.Subscription("Subscribe_Insert_List", Subscription.KeyCode, Subscription.SubscriptionList);

            result.Message = "User Alert Permissions Saved Successfully.";
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //-- Story 1649
        [System.Web.Http.Route("swi/GETUserAlertConfiguration"), System.Web.Http.HttpPost]
        public HttpResponseMessage Configuration(AL_GetAlertConfiguration Configuration)
        {
            Response result = new Response();
            AL_AlertBL model = new AL_AlertBL();
            


            if (Configuration.UserId != 0)
            {
                List<AL_GetAlertConfiguration> ModelResult = model.Configuration("Get_User_Alert_Configurations", Configuration);
                return Request.CreateResponse(HttpStatusCode.OK, ModelResult);
            }
            else
            {
                result.Message = "UserId is required!";
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }

        // -- Story #1650
        [System.Web.Http.Route("swi/POSTUserAlertConfiguration"), System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateConfiguration(List<AL_GetAlertConfiguration> Configuration)
        {
            Response result = new Response();
            AL_AlertBL model = new AL_AlertBL();

            model.UpdateConfiguration("Update_User_Alert_Configurations", Configuration);
            result.Message = "User Alert Permissions Saved Successfully.";
            result.Status = "true";
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //working
        //[System.Web.Http.Route("swi/GETAlertConfiguration"), System.Web.Http.HttpPost]
        //public HttpResponseMessage GlobalConfiguration(AL_GetAlertConfiguration Configuration)
        //{
        //    Response result = new Response();
        //    AL_AlertBL model = new AL_AlertBL();

        //    List<AL_GetAlertConfiguration> ModelResult = model.Configuration("GET_Configurations_List", Configuration);


        //    return Request.CreateResponse(HttpStatusCode.OK, ModelResult);
        //}

        //-- Story 1696 -- Story 1697
        [System.Web.Http.Route("swi/GETAlertConfiguration"), System.Web.Http.HttpPost]
        public HttpResponseMessage GlobalConfiguration(AL_GetAlertConfiguration Configuration)
        {
            Response result = new Response();
            AL_AlertBL model = new AL_AlertBL();
            var dd = System.Web.HttpContext.Current.Application["User"];
            if (Configuration.KeyCode != null)
            {
                List<AL_GetAlertConfiguration> ModelResult = model.Configuration("Get_Alert_Configurations", Configuration);
                return Request.CreateResponse(HttpStatusCode.OK, ModelResult);
            }
            else
            {
                result.Message = "KeyCode is required!";
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }  
        }

        // -- Story #1698
        [System.Web.Http.Route("swi/POSTAlertConfiguration"), System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateGlobalConfiguration(List<AL_GetAlertConfiguration> Configuration)
        {
            Response result = new Response();
            AL_AlertBL model = new AL_AlertBL();

            model.UpdateConfiguration("Update_Alert_Configurations", Configuration);
            result.Message = "Alerts configured successfully.";
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        //-- Role
        [System.Web.Http.Route("swi/GETAlertRoleConfiguration"), System.Web.Http.HttpPost]
        public HttpResponseMessage RoleConfiguration(AL_GetAlertConfiguration Configuration)
        {
            Response result = new Response();
            AL_AlertBL model = new AL_AlertBL();
           
            if (Configuration.KeyCode != null)
            {
                List<AL_GetAlertConfiguration> ModelResult = model.Configuration("Get_Alert_Role_Configurations", Configuration);
                return Request.CreateResponse(HttpStatusCode.OK, ModelResult);
            }
            else
            {
                result.Message = "KeyCode is required!";
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }

        // -- Role
        [System.Web.Http.Route("swi/POSTAlertRoleConfiguration"), System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateRoleConfiguration(List<AL_GetAlertConfiguration> Configuration)
        {
            Response result = new Response();
            AL_AlertBL model = new AL_AlertBL();

            model.UpdateConfiguration("Update_Alert_Role_Configurations", Configuration);
            result.Message = "Role Alert Permissions Saved Successfully.";
            result.Status = "true";
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [System.Web.Http.Route("swi/Notification"),System.Web.Http.HttpPost]
        public HttpResponseMessage SendNotification(AL_Notification Notification)
        {
            NotificationHub SRNotificationHub = new NotificationHub();
            AL_Notification ReadNotification = new AL_Notification();
            ReadNotification.SentTo = Notification.UserID;

            //SRNotificationHub.SendNotification(ReadNotification.SentTo);
            AL_GetNotification UserNotify = new AL_GetNotification();
            UserNotify.AlertRecieverId = 11;
            AL_AlertBL model = new AL_AlertBL();
            List<AL_GetNotification> ModelResult = model.GetNotification("NotificationBell", UserNotify);
            var json = JsonConvert.SerializeObject(ModelResult);
            SRNotificationHub.SendNotification(ReadNotification.SentTo, json);

            Response result = new Response();
            result.Message = "Alerts Sent Succesfully!";
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [System.Web.Http.Route("swi/NotificationBell"), System.Web.Http.HttpPost]
        public HttpResponseMessage NotificationBell(AL_GetNotification Info)
        {
            AL_AlertBL model = new AL_AlertBL();
            List<AL_GetNotification> ModelResult = model.GetNotification("NotificationBell", Info);
            //AL_GetNotification model = new AL_GetNotification();
            //NotificationHub SRNotificationHub = new NotificationHub();
            //var json = JsonConvert.SerializeObject(ModelResult);
            //SRNotificationHub.SendNotification(Info.AlertRecieverId.ToString(), json);

            return Request.CreateResponse(HttpStatusCode.OK, ModelResult);
        }

        [System.Web.Http.Route("swi/NotificationBellUpdate"), System.Web.Http.HttpPost]
        public HttpResponseMessage NotificationBellUpdate(List<AL_GetNotification> Info)
        {
            AL_AlertBL model = new AL_AlertBL();
            model.UpdateNotification("NotificationBellUpdate", Info);
            Response result = new Response();
            result.Message = "Alerts Read Succesfully!";
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //[System.Web.Http.HttpGet]
        //public JsonResult Configuration(string KeyCode)
        //{
        //    //AL_AlertBL model = new AL_AlertBL();
        //    //List<AL_Configuration> result = new List<AL_Configuration>();
        //    //result = model.Configuration("Configuration", KeyCode).ToList();
        //    return Json('a');
        //}
    }
}