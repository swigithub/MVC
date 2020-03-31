using AirView.DBLayer.Project.DTO;
using AirView.DBLayer.Project.Model;
using Library.SWI.Project.DAL;
using Library.SWI.Project.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Library.SWI.Project.BLL
{
    /*----MoB!----*/
    /*----19-10-2017----*/

    public class PM_TaskBL
    {
        private PM_TaskDL td = new PM_TaskDL();

        public dynamic Manage(string Filter, PM_Task t)
        {
            return td.Manage(Filter, t.TaskId, t.PTaskId, t.ProjectId, t.PriorityId, t.TaskTypeId, t.StatusId, t.PredecessorId, t.Title, t.PlannedDate, t.ActualStartDate, t.ActualEndDate
                , t.EstimatedStartDate, t.EstimatedEndDate, t.Description, t.IsEstimate, t.IsActive, t.TargetDate, t.ForecastedSites, t.CompletionPercent, t.BudgetCost, t.ActualCost, t.MapCode, t.MapColumn
                , t.Color, t.ScopeId, t.IsStartMilestone, t.IsEndMilestone, t.SortOrder);
        }

        public dynamic InsertPM_siteTask(string Filter, PM_SiteTasks sitetsk)
        {
            
            return td.ManageSiteTask(Filter, sitetsk.SiteTaskId, sitetsk.ProjectSiteId, sitetsk.TaskId, sitetsk.PTaskId, sitetsk.PredecessorId, sitetsk.TaskTypeId,sitetsk.CreatedBy, sitetsk.TaskTitle, sitetsk.StatusId, sitetsk.PriorityId, sitetsk.ForecastDate, sitetsk.ForecastStartDate, sitetsk.ForecastEndDate, sitetsk.PlannedDate, sitetsk.TargetDate, sitetsk.ActualStartDate, sitetsk.ActualEndDate, sitetsk.CompletionPercent, sitetsk.BudgetCost, sitetsk.ActualCost, sitetsk.MapCode, sitetsk.MapColumn, sitetsk.IsActive,sitetsk.Description, sitetsk.AssignTo);
        }
        public dynamic InsertPM_siteTask(string Filter, List<PM_SiteTasks> sitetsks,Int64 CreatedBy=0)
        {

            dbDataTable dbdt = new dbDataTable();
            PM_TaskDL ptd = new PM_TaskDL();
            var dt1 = dbdt.TaskList();
            int Counter = 1;
            foreach (var item in sitetsks)
            {
               
                myDataTable.AddRow(dt1, "Value1", item.ProjectSiteId, "Value2", item.SiteTaskId,
                    "Value3", item.ActualStartDate, "Value4", item.ActualEndDate, "Value5", item.ForecastStartDate,
                    "Value6", item.ForecastEndDate, "Value7", item.PlannedDate, "Value8", item.TargetDate
                    , "Value9", item.StatusId, "Value10", item.AssignTo, "Value11", CreatedBy, "Value12", Convert.ToString(item.TaskStageId)
                    );
                Counter++;
            }
            return td.ManageSiteTask(Filter, dt1);
        }
        public List<PM_Task> ToList(string Filter="", string value1 = "0", string value2 = "0", Int64 ProjectId = 0, Int64 TaskId = 0)
        {
            DataTable dt = td.GetDataTable(Filter, value1, value2,string.Empty,ProjectId,TaskId);
            return dt.ToList<PM_Task>();
        }
        public List<PM_Task_DTO> Radiness(string Filter = "", string value1 = "", string value2 = "", string value3 = "", Int64 ProjectId = 0, Int64 TaskId = 0)
        {
            DataTable dt = td.GetDataTable(Filter, value1, value2, value3, ProjectId, TaskId);
            return dt.ToList<PM_Task_DTO>();
        }


        public bool SaveTodo(PM_Todo todo, string Filter)
        {
            var res = td.SaveTodo(Filter, todo.TodoId, todo.Description, todo.Type, todo.Status, todo.CreatedById, todo.CreatedOn, todo.ToDoDateTime, todo.ToDoTitle, todo.ProjectId, todo.SiteId, todo.TaskId,todo.AssignedToIds);
            return res;
        }

        public bool EditTodo(PM_Todo todo, string Filter)
        {
            var res = td.EditTodo(Filter, todo.TodoId, todo.Description, todo.Type, todo.Status, todo.ToDoDateTime, todo.ToDoTitle,todo.SiteId,todo.TaskId,todo.AssignedToIds);
            return res;
        }
    }
}