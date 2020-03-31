using AirView.DBLayer.AirView.Entities;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication.Areas.Alert.SRNotificationHub
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<string, AL_UserHubModel> Users = new ConcurrentDictionary<string, AL_UserHubModel>(StringComparer.InvariantCultureIgnoreCase);
        public void SendNotification(string SentTo, string ModelResult)
        {
            // Call the addNewMessageToPage method to update clients.
            try
            {
                //Get TotalNotification
                //string totalNotif = "hellow";

                 //Send To
                 AL_UserHubModel receiver;

                var dd = Users.TryGetValue(SentTo, out receiver);
                if (Users.TryGetValue(SentTo, out receiver))
                {
                    HashSet<string> cid = receiver.ConnectionIds;
                    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

                    foreach (var id in cid)
                    {
                        context.Clients.Client(id).broadcaastNotif(ModelResult);
                    }

                    
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void GetNotification(string SentTo)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.VehicleStream(SentTo);
        }

        /* == When User Load SignalR FrontEnd Libs it will triggered and store the instance of user as new client == */
        public override Task OnConnected()

        {
            var CurrentUser = Context.QueryString["CurrentUser"];

            if (CurrentUser == null)
            {
                CurrentUser = "null";
            }
            //string[] dd = System.Web.HttpContext.Current.Application.AllKeys;
            string connectionId = Context.ConnectionId;//System.Web.HttpContext.Current.Application["UserId"].ToString();
            string userName = CurrentUser.ToString();//System.Web.HttpContext.Current.Application["UserId"].ToString();
            var user = Users.GetOrAdd(userName, _ => new AL_UserHubModel
            {
                UserName = userName,
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(connectionId);
                if (user.ConnectionIds.Count == 1)
                {
                    Clients.Others.userConnected(userName);
                }
            }

            return base.OnConnected();
        }
    }
}