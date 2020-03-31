using System;
using System.Collections.Generic;

namespace AirView.DBLayer.Project.Model
{
    public class PM_TaskEntry
    {
        public Int64 EntryId { get; set; }

        public bool IsDeleted { get; set; }
        public string Title { get; set; }
        public Int64 Revision { get; set; }
        public Int64 ProjectId { get; set; }
        public Int64 TaskId { get; set; }
        public Int64 ProjectSiteId { get; set; }
        public Int64 FormId { get; set; }
        public string FormValue { get; set; }
        public Int64 CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }

        public Int64 ResourceId { get; set; }
        public Int64 GroupId { get; set; }
        public Int64 RACIId { get; set; }

        public string ResourceName { get; set; }
        public string GroupName { get; set; }
        public string RACIName { get; set; }
        public string Comments { get; set; }

        public Int64 PMTRId { get; set; }
        public Int64 RatePerHour { get; set; }
        public List<PM_TaskEntry> CurrentRevision { get; set; }
        public Int64? UserID { get; set; }

    }
}