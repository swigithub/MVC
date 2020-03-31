using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_Tracker
    {
        public int TrackerId { get; set; }
        public string TrackerGroup { get; set; }
        public string GroupCode { get; set; }
        public string TrackerGroupId { get; set; }
        public string Title { get; set; }
        public string Budget { get; set; }
        public string Actual { get; set; }
        public string Unit { get; set; }
        public string IsDeleted { get; set; }
        public string  TaskId { get; set; }
        public int SiteTaskTrackerId { get; set; }
        
    }
}
