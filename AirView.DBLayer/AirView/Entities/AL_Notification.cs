using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
    public class AL_Notification
    {
        public string UserID { get; set; }
        public string Message { get; set; }

        public int Id { get; set; }
        public Nullable<int> Type { get; set; }
        public string Details { get; set; }
        public string Title { get; set; }
        public string DetailsURL { get; set; }
        public string SentTo { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsReminder { get; set; }
        public string Code { get; set; }
        public string NotificationType { get; set; }
    }
}
