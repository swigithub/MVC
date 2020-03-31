using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_Todo
    {
        public Int64 TodoId { get; set; }
        public string ToDoTitle { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime ToDoDateTime { get; set; } 
        public Int64 CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ProjectId { get; set; }
        public Int64? SiteId { get; set; }
        public Int64? TaskId { get; set; }
        public string SiteName { get; set; } = "";
        public string TaskName { get; set; } = "";
        public string AssignedToIds { get; set; } = "";
    }
}
