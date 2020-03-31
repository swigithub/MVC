using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace WebApplication.Areas.Fleet.SRHubs
{
    public class ClientHub : Hub
    {
        public void Send(string vobject)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.VehicleStream(vobject);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
   }
}