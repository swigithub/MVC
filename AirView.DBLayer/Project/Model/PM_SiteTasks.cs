using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_SiteTasks
    {
        public Int64 SiteTaskId { get; set; }
        public Int64 ProjectSiteId { get; set; }
        public Int64 TaskId { get; set; }
        public Int64 PTaskId { get; set; }
        public Int64 PredecessorId { get; set; }
        public Int64 TaskTypeId { get; set; }
        public Int64 CreatedBy { get; set; }
        public string TaskTitle { get; set; }
        public string AssignTo { get; set; }
        public string Description { get; set; }
        public Int64 StatusId { get; set; }
        public Int64 PriorityId { get; set; }
        public DateTime? ForecastDate { get; set; }
        public DateTime? ForecastStartDate { get; set; }
        public DateTime? ForecastEndDate { get; set; }
        public DateTime? PlannedDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public float CompletionPercent { get; set; }
        public float BudgetCost { get; set; }
        public float ActualCost { get; set; }
        public string MapCode { get; set; }
        public string MapColumn { get; set; }
        public bool IsActive { get; set; }
        public int? TaskStageId { get; set; }
    }
}
