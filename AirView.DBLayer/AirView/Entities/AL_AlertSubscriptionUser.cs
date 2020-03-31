using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
    public class AL_AlertSubscriptionUser
    {
        public int AlertConfigId { get; set; }
        public int UserId { get; set; }
        public Boolean IsSubscribed { get; set; }
        public Boolean IsPushAlertRequired { get; set; }
        public Boolean IsEmailAlertRequired { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
    }
}
