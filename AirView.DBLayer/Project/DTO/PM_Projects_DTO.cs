using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.DTO
{
    public class PM_Projects_DTO
    {
        public float ActualCost { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public float BudgetCost { get; set; }
        public string Client { get; set; }
        public string Color { get; set; }
        public float CompletionPercent { get; set; }
        public string Description { get; set; }
        public string EndClient { get; set; }

        public DateTime? EstimateEndDate { get; set; }
        public DateTime? EstimateStartDate { get; set; }
       
        public DateTime? PlannedDate { get; set; }
        public string Priority { get; set; }
        public string PriorityColor { get; set; }

        public Int64 ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public DateTime? TargetDate { get; set; }
    }
}
