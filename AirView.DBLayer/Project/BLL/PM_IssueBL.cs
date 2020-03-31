using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using Library.SWI.Project.Model;
using SWI.Libraries.Common;
using SWI.Libraries.Security.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace AirView.DBLayer.Project.BLL
{
    public class PM_IssueBL
    {
        private PM_IssueDL dal = new PM_IssueDL();

        public string Filter_Insert = "INSERT_ISSUE";
        public string Filter_Update = "UPDATE_ISSUE";

        public List<Sec_User> GetUsers(string filter, Int64 projectId)
        {
            DataTable dt = dal.ToList(filter, projectId);

            List<Sec_User> lst = new List<Sec_User>();
            foreach (DataRow item in dt.Rows)
            {
                Sec_User user = new Sec_User();
                user.UserId = Convert.ToInt64(item["UserId"]);
                user.UserName = item["Username"].ToString();
                lst.Add(user);
            }

            return lst;
        }

        public List<Sec_User> GetUsers(string filter, Int64 projectId, Int64 TaskId)
        {
            DataTable dt = dal.ToList(filter, projectId, TaskId);

            List<Sec_User> lst = new List<Sec_User>();
            foreach (DataRow item in dt.Rows)
            {
                Sec_User user = new Sec_User();
                user.UserId = Convert.ToInt64(item["UserId"]);
                user.UserName = item["Username"].ToString();
                lst.Add(user);
            }

            return lst;
        }

        public List<PM_Task> GetTasks(string filter, Int64 projectId, Int64 TaskId)
        {
            DataTable dt = dal.ToList(filter, projectId, TaskId);

            List<PM_Task> lst = new List<PM_Task>();
            foreach (DataRow item in dt.Rows)
            {
                PM_Task task = new PM_Task();
                task.TaskId = Convert.ToInt64(item["TaskId"]);
                task.Title = item["Task"].ToString();
                lst.Add(task);
            }

            return lst;
        }

        public bool ManageIssueLog(string Filter, PM_IssuesLog issueLog)
        {
            issueLog.CreatedOn = DateTime.Now;
            return dal.ManageIssueLog(Filter, issueLog.IssueId, issueLog.StatusId, issueLog.UserId, issueLog.Description, issueLog.CreatedOn);

        }

        public bool Manage(string Filter, PM_Issues i, Int64 UserId = 0)
        {
            i.RequestDate = DateTime.Now;
            return dal.Manage(Filter, i.ProjectId, i.IssueId, i.TaskId, i.ProjectSiteId, i.TaskTypeId, i.IssuePriorityId, i.IssueStatusId, i.SeverityId, i.RequestedBy, i.Description, i.TicketTypeId, i.RequestedById, i.RequestDate, i.AssignedToId, i.IssueCategoryId, i.ReasonId, i.IssueById, i.ForecastDate, i.TargetDate, (i.ActualStartDate == null) ? null : i.ActualStartDate, i.ActualEndDate, i.RequestedDate, i.IsUnavoidable, i.ActivityTypeId, i.ItemTypeId, i.eNB, i.ExtendedeNB, i.EquipmentId, i.AOTSCR, i.FilePath, UserId, i.AlarmId, i.MSWindowId);
        }

        public List<PM_IssuesLog> GetIssueLog(string Filter, long IssueId)
        {
            DataTable dt = dal.GetIssueLog(Filter, IssueId);
            List<PM_IssuesLog> lst = new List<PM_IssuesLog>();
            foreach (DataRow item in dt.Rows)
            {
                PM_IssuesLog issue = new PM_IssuesLog();
                issue.IssueId = Convert.ToInt64(item["IssueId"]);
                issue.IssueLogId = Convert.ToInt64(item["IssueLogId"]);
                issue.UserId = Convert.ToInt64(item["UserId"]);
                issue.StatusId = Convert.ToInt64(item["StatusId"]);
                issue.Description = item["Description"].ToString();
                issue.Status = item["Status"].ToString();
                issue.Picture = item["Picture"].ToString();
                issue.UserName = item["UserName"].ToString();
                issue.Task = item["Task"].ToString();
                issue.Priority= item["Priority"].ToString();
                issue.AssignedToId = item["AssignedToId"].ToString();
                issue.CreatedOn = Convert.ToDateTime(item["CreatedOn"].ToString());


                if (!string.IsNullOrEmpty(Convert.ToString(item["ActivityTypeId"])))
                {
                    issue.ActivityTypeId = Convert.ToInt64(item["ActivityTypeId"]);
                }              
                issue.IssueCategoryId = Convert.ToInt64(item["IssueCategoryId"]);
                issue.eNB = item["eNB"].ToString();
                issue.ExtendedeNB = item["ExtendedeNB"].ToString();
                issue.AOTSCR = item["AOTSCR"].ToString();
                issue.RequestedDate = Convert.ToDateTime(item["RequestedDate"].ToString());
               
                if (!string.IsNullOrEmpty(Convert.ToString(item["ReasonId"])))
                {
                    issue.ReasonId = Convert.ToInt64(item["ReasonId"]);
                }
                if (!String.IsNullOrEmpty(Convert.ToString(item["MSWindowId"])))
                {
                    issue.MSWindowId = Convert.ToInt64(item["MSWindowId"]);
                }

                if (!String.IsNullOrEmpty(Convert.ToString(item["AlarmId"])))
                {
                    issue.AlarmId = Convert.ToInt64(item["AlarmId"]);
                }


                if (!String.IsNullOrEmpty(Convert.ToString(item["EquipmentId"])))
                {
                    issue.EquipmentId = item["EquipmentId"].ToString();
                }

                if (!String.IsNullOrEmpty(Convert.ToString(item["AOTSCR"])))
                {
                    issue.AOTSCR = item["AOTSCR"].ToString();
                }

                if (!string.IsNullOrEmpty(Convert.ToString(item["ReasonId"])))
                {
                    issue.ReasonId = Convert.ToInt64(item["ReasonId"]);
                }

                if (!String.IsNullOrEmpty(Convert.ToString(item["SeverityId"])))
                {
                    issue.SeverityId = Convert.ToInt64(item["SeverityId"]);
                }

                if (!String.IsNullOrEmpty(Convert.ToString(item["RequestedBy"])))
                {
                    issue.RequestedBy = item["RequestedBy"].ToString();
                }
                lst.Add(issue);
            }
            return lst;
        }


        public List<PM_Issues> ToList(string Filter,Int64 ProjectId = 0, Int64 IssueId = 0)
        {
            DataTable dt = dal.GetDataTable(Filter, ProjectId, IssueId);
            return dt.ToList<PM_Issues>();
        }

        public List<PM_IssuesLog> GetIssue(string Filter, long IssueId)
        {
            DataTable dt = dal.GetIssueLog(Filter, IssueId);
            List<PM_IssuesLog> lst = new List<PM_IssuesLog>();
            foreach (DataRow item in dt.Rows)
            {
                PM_IssuesLog issue = new PM_IssuesLog();
                issue.ActivityTypeId = (!string.IsNullOrEmpty(item["ActivityTypeId"].ToString())) ? int.Parse(item["ActivityTypeId"].ToString()) : 0;  // Convert.ToInt64(item["ActivityTypeId"]);
                issue.IssueCategoryId = (!string.IsNullOrEmpty(item["IssueCategoryId"].ToString())) ? int.Parse(item["IssueCategoryId"].ToString()) : 0;  /// Convert.ToInt64(item["IssueCategoryId"]);
                issue.eNB = item["eNB"].ToString();
                issue.IsUnavoidable = Convert.ToBoolean(item["IsUnavoidable"]);
                issue.ExtendedeNB = item["ExtendedeNB"].ToString();
                issue.AOTSCR = item["AOTSCR"].ToString();
                issue.RequestedDate = Convert.ToDateTime(item["RequestedDate"].ToString());
                issue.ReasonId = (!string.IsNullOrEmpty(item["ReasonId"].ToString())) ? int.Parse(item["ReasonId"].ToString()) : 0; //Convert.ToInt64(item["ReasonId"]);
                issue.MSWindowId = (!string.IsNullOrEmpty(item["MSWindowId"].ToString())) ? int.Parse(item["MSWindowId"].ToString()) : 0; //Convert.ToInt64(item["MSWindowId"]);
                issue.AlarmId = (!string.IsNullOrEmpty(item["AlarmId"].ToString())) ? int.Parse(item["AlarmId"].ToString()) : 0; // Convert.ToInt64(item["AlarmId"]);
                issue.EquipmentId = item["EquipmentId"].ToString();
                issue.AOTSCR = item["AOTSCR"].ToString();
                issue.ReasonId = (!string.IsNullOrEmpty(item["ReasonId"].ToString())) ? int.Parse(item["ReasonId"].ToString()) : 0; // Convert.ToInt64(item["ReasonId"]);
                issue.SeverityId = (!string.IsNullOrEmpty(item["SeverityId"].ToString())) ? int.Parse(item["SeverityId"].ToString()) : 0; // Convert.ToInt64(item["SeverityId"]);
                issue.RequestedBy = item["RequestedBy"].ToString();
                issue.ItemTypeId = (!string.IsNullOrEmpty(item["ItemTypeId"].ToString())) ? int.Parse(item["ItemTypeId"].ToString()) : 0; // Convert.ToInt64(item["ItemTypeId"]);
                lst.Add(issue);
            }
            return lst;
        }
    }
}