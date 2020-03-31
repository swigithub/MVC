using AirView.DBLayer.Project.Model;
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
    public class PM_Project
    {
        public float ActualCost { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public float BudgetCost { get; set; }
        public string Client { get; set; }
        public Int64 ClientId { get; set; }
        public string Color { get; set; }
        public float CompletionPercent { get; set; }
        public string Description { get; set; }
        public string EndClient { get; set; }
        public Int64 EndClientId { get; set; }
        public DateTime? EstimateEndDate { get; set; }
        public DateTime? EstimateStartDate { get; set; }
        public List<PM_GazetteHolidays> GH { get; set; }
        public List<PM_TaskStages> TS { get; set; }
        public bool IsActive { get; set; }
        public bool IsEstimate { get; set; }
        public bool IsWoLinked { get; set; }
        public DateTime? PlannedDate { get; set; }
        public string Priority { get; set; }
        public Int64 PriorityId { get; set; }

        public Int64 ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ScopeId { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public Int64 StatusId { get; set; }
        public DateTime? TargetDate { get; set; }

        public Int64 TaskTypeId { get; set; }

        public string WorkingDays { get; set; }

        public Int64 ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string Entity { get; set; }
        public int EntityId { get; set; }
        public int CategoryId { get; set; }
        public int CurrencyId { get; set; }
        public string WorkGroups { get; set; }
        public string Groups { get; set; }
        public bool IsWorkflowAllowed { get; set; } = false;
    


    }
}