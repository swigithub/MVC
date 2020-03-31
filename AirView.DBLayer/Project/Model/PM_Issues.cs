using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AirView.DBLayer.Project.Model
{
    public class PM_Issues
    {
        public Int64 IssueId { get; set; }
        public Int64 ProjectId { get; set; }
        public Int64 ProjectSiteId { get; set; }
        public Int64 TaskTypeId { get; set; }
        public Int64 TaskId { get; set; }
        public Int64 IssuePriorityId { get; set; }
        public Int64 IssueStatusId { get; set; }
        public Int64 RequestedById { get; set; }
        public Int64 IssueCategoryId { get; set; }
        public Int64 ItemTypeId { get; set; }
        public DateTime? RequestDate { get; set; }
        public string AssignedToId { get; set; }
        public string UpdatedBy { get; set; }

        public string LastUpdated { get; set; }


        public string FACode { get; set; }
        public Int64 ReasonId { get; set; }
        public Int64 IssueById { get; set; }
        public Int64 SeverityId { get; set; }
        public Int64 ActivityTypeId { get; set; }
        public Int64 GNGId { get; set; }
        public Int64 AlarmId { get; set; }

        public Int64 MSWindowId { get; set; }
        public string ExtendedeNB { get; set; }
        public string eNB { get; set; }
        public string EquipmentId { get; set; }
        public string AOTSCR { get; set; }
        public string FilePath { get; set; }
        public string RequestedBy { get; set; }
        public DateTime? RequestedDate { get; set; }
        public DateTime? ForecastDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Task { get; set; }
        public string LogStatus { get; set; }
        public string AssingedTo { get; set; }
        public string PriorityColor { get; set; }
        public string TicketType { get; set; }
        public int Count { get; set; }
        public bool IsUnavoidable { get; set; }
        public string TicketTypeId { get; set; }

        

    }
}
