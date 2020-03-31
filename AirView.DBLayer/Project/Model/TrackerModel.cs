using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
   public class TrackerModel
    {
        public TrackerModel()
        {
            Tracker = new List<TrackerModel>();
        }
        public int TrackerId { get; set; }
        public int TrackerGroupId { get; set; }
        public int ProjectId { get; set; }
        public int TaskId { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public List<TrackerModel> Tracker { get; set; }

    }
}
