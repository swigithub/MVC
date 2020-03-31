using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class TaskModel
    {
        public Int64 TaskId { get; set; }
        public Int64 PTaskId { get; set; }
        public Int64 ProjectId { get; set; }
        public string Task { get; set; }
        public float ActualSites { get; set; }
        public float TotalSites { get; set; }
        public string Color { get; set; }
    }
}
