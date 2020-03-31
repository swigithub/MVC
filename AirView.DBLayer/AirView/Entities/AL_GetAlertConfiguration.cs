using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
    public class AL_GetAlertConfiguration
    {
        public int AlertConfigId { get; set; }
        public int AlertCategoryId { get; set; }
        public string AlertCategoryName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Boolean IsSubscribed { get; set; }
        public Boolean IsPushAlertRequired { get; set; }
        public Boolean IsEmailAlertRequired { get; set; }

        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string KeyCode { get; set; }
        public int ParentId { get; set; }

        public string ParentName { get; set; }
    }
}
