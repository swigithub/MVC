using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
    public class AL_GetNotification
    {
        public int AlertRecieverId { get; set; }
        public int AlertCategoryId { get; set; }
        public int AlertId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Picture { get; set; }
        public Boolean IsPushAlertSent { get; set; }
        public Boolean IsPushAlertRead { get; set; }
        public Boolean IsEmailAlertSent { get; set; }

        public DateTime CreatedOn { get; set; }
        public string Notification { get; set; }
        public string ParentName { get; set; }
    }
}
