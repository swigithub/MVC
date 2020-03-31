using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
   public class ProjectApiObject
    {
        public Int64 userId { get; set; }
        public string statusIds { get; set; }
        public string typeIds { get; set; }
        public string marketIds { get; set; }
        public int projectId { get; set; }
        public string clientIds { get; set; }
        public string priorityIds { get; set; }
        public DateTime? toDate { get; set; }
        public DateTime? fromDate { get; set; }

        public string searchKey { get; set; }
    }
    
}
