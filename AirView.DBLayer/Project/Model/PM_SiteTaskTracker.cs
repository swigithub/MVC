using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_SiteTaskTracker
    {
        public int TaskTrackerId { get; set; }
        public int CreatedById { get; set; }
        public int TrackerGroupid { get; set; }
        public string TrackerGroup { get; set; }
        public int TaskId { get; set; }
        public string SiteTaskTrackerid { get; set; }
        //public int SiteTaskTrackerId { get; set; }
        public string Actual { get; set; }
        public string IsDeleted { get; set; }
        public int SiteId { get; set; }

    }
}
