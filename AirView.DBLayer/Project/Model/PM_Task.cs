using Library.SWI.Project.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Project.Model
{
    /*----MoB!----*/
    /*----19-10-2017----*/
    public class PM_Task
    {
        public PM_Task()
        {
            ProjectResources = new List<PM_ProjectResource>();
        }
        public int Level { get; set; }
        public string id { get; set; }

        public Int64 Duration { get; set; }
        public Int64 TaskId { get; set; }
        public string PTaskId { get; set; }
        public Int64 ProjectId { get; set; }
        public Int64 TaskTypeId { get; set; }
        public string TaskTypeColor { get; set; }
        public Int64 StatusId { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public string PredecessorId { get; set; }
        public Int64 PriorityId { get; set; }
        public string Title { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
        public DateTime? PlannedDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public string Description { get; set; }
        public bool IsPriority { get; set; }
        public bool IsActive { get; set; }
        public bool IsEstimate { get; set; }
        public int ForecastedSites { get; set; }
        public float CompletionPercent { get; set; }
        public float BudgetCost { get; set; }
        public float ActualCost { get; set; }
        public string MapCode { get; set; }
        public string MapColumn { get; set; }
        public string Color { get; set; }
        public Int64 ScopeId { get; set; }

        public bool IsStartMilestone { get; set; }
        public bool IsEndMilestone { get; set; }

        public Int64 SortOrder { get; set; }
        public int CompletedSiteTasks { get; set; }
        public Int64 maxSortOrder { get; set; }

        public Int64 ChildTasks { get; set; }

        public Int64 IssueCount { get; set; }

        public string SavingType { get; set; }
        public List<PM_ProjectResource> ProjectResources { get; set; }
    }
}
