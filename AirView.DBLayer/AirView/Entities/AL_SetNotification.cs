using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
    public class AL_SetNotification
    {
        public int AlertConfigId { get; set; }
        public int EntityId { get; set; }
        public string Notification { get; set; }

        public int AlertRecieverId { get; set; }
        public int UserId { get; set; }

        public int IsPushAlertSent { get; set; }
        public int IsPushAlertRead { get; set; }
        public int IsEmailAlertSent { get; set; }
    }
}
