using AirView.DBLayer.Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.DTO
{
   public class PM_ProjectEntity_DTO
    {
        public int EntityId { get; set; }
        public string EntityCode { get; set; }
        public string Name { get; set; }
        public string EntityCategory { get; set; }
        public string Status { get; set; }
        public string StatusColor{ get; set; }
        public string Market { get; set; }
        public string SubMarket { get; set; }
        public int Completetion { get; set; }
        public int PendingTasksCount { get; set; }
        public string TaskStatusPendingColor { get; set; }
        public string TaskStatusCompletedColor { get; set; }
        public int CompletedTasksCount { get; set; }
        public int NumberOfIssues { get; set; }
    }
}
