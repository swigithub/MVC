using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_SiteTaskParents
    {
        public Int64 TaskId { get; set; }
        public Int64 Task { get; set; }
        public Int64 ProjectId { get; set; }
        public string Milestone { get; set; }
        public string ForecastStartDate { get; set; }
        public DateTime ForecastEndDate { get; set; }
        public DateTime PlanDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime TargetDate { get; set; }
        public string Priority { get; set; }
        public string PriorityColor { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public string StatusId { get; set; }
        public string AssignTo { get; set; }
        public int Count { get; set; }
        public List<PM_SiteTaskParents> ChildLayer { get; set; } 
    }
}
