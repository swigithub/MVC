using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
   public class PM_Targets
    {
     
        public Int64? ProjectId { get; set; }

        public Int64? MilestoneId { get; set; }
        public Int64? StageId { get; set; }
        public string TargetType { get; set; }    
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TargetValue { get; set; }
        public int SiteCount { get; set; }
        public Int64 UserId { get; set; }
        public Int32 RevisionId { get; set; }
        public Int32 TargetYear { get; set; }
        public Int32 Task { get; set; }

    }

    public class PM_Target_File
    {
        public string Project { get; set; }
        public string Task{ get; set; }

       // public string MileStone { get; set; }
        public string Stage { get; set; }
        public string TargetType { get; set; }
        public string TargetValue { get; set; }
        public int SiteCount { get; set; }
      
    }
}
