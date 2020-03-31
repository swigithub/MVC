using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
    public class AL_Subscription
    {
        public string KeyCode { get; set; }
        public List<AL_AlertSubscriptionUser> SubscriptionList { get; set; }
    }
}
