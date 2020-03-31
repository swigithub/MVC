using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_WorkLog
    {
        public Int64 WLogId { get; set; }
        public Int64 ProjectId { get; set; }
        public Int64 ProjectSiteId { get; set; }
        public Int64 TaskId { get; set; }
        public Int64 IssueId { get; set; }
        public string LogType { get; set; }
        public Int64 UserId { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public DateTime LogDate { get; set; }
        public float LogHours { get; set; }
        public float RatePerUnit { get; set; }
        public float Cost { get; set; }
        public string Description { get; set; }
        public bool IsApproved { get; set; }

        public string ProjectName { get; set; }
        public string SiteName { get; set; }
        public string Task { get; set; }
        public string WorkGroup { get; set; }
        public Int64 RatePerHour { get; set; }
        public bool IsAttended { get; set; }

        public string FACode { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }
    }
   
    public class p_pm_worklog
    {
        public p_pm_worklog()
        {
            List<p_pm_worklog> childp_pm_worklog = new List<p_pm_worklog>();

        }
        public string LogType { get; set; }
        public string Name { get; set; }
        public float LogHours { get; set; }
        public List<p_pm_worklog> childp_pm_worklog { get; set; }
    }
    public class f_pmworklog
    {
        public string LogType { get; set; }
        public string Name { get; set; }
        public float LogHours { get; set; }
    }
}
