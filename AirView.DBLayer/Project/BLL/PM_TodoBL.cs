using Library.SWI.Project.DAL;
using Library.SWI.Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using SWI.Libraries.Common;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Project.Model;
using AirView.DBLayer.Project.DTO;
using AirView.DBLayer.Project.DAL;

namespace AirView.DBLayer.Project.BLL
{
    public class PM_TodoBL
    {
        PM_TodoDL dal = new PM_TodoDL();
        public List<PM_Todo> GetTodo(string filter, Int64 projectId,Int64 UserId, string WhereClause = "",DateTime? todate=null,DateTime? fromdate = null)
        {

            if(todate == fromdate)
            {
                todate = todate.Value.AddDays(1).Date.AddSeconds(-1);
            }
            if (todate != null && fromdate != null)
            {
       //         WhereClause = $" AND CONVERT(varchar, ToDoDateTime) >= '{fromdate.ToString().Trim()}' AND CONVERT(varchar, ToDoDateTime, 1) <= '{todate.ToString().Trim()}'";
                WhereClause = $" AND CONVERT(datetime, " +"'"+fromdate.ToString() + "'"+ ") <= ToDoDateTime AND CONVERT(datetime, " +"'" + todate.ToString() + "'" + ") >= ToDoDateTime ";
            }

            DataTable dt = dal.GetDataTable(filter, projectId,UserId, WhereClause);

            List<PM_Todo> lst = new List<PM_Todo>();
            foreach (DataRow item in dt.Rows)
            {
                PM_Todo todo = new PM_Todo();                
                todo.CreatedOn = Convert.ToDateTime(item["CreatedOn"]);
                todo.Description = item["Description"].ToString();
                todo.Status = item["Status"].ToString();
                todo.TodoId = Convert.ToInt64(item["TodoId"].ToString());
                todo.Type = item["Type"].ToString();
                todo.ToDoDateTime = Convert.ToDateTime(item["ToDoDateTime"]);
                todo.ToDoTitle = Convert.ToString(item["ToDoTitle"]);
                todo.SiteName = Convert.ToString(item["SiteName"]);
                todo.TaskName = Convert.ToString(item["TaskName"]);
                todo.AssignedToIds = Convert.ToString(item["AssignedToIds"]);
                todo.SiteId = !string.IsNullOrEmpty(Convert.ToString(item["SiteId"])) ? Convert.ToInt64(item["SiteId"]) : 0;
                todo.TaskId = !string.IsNullOrEmpty(Convert.ToString(item["TaskId"])) ? Convert.ToInt64(item["TaskId"]) : 0;
                lst.Add(todo);
            }
            
            return lst;
        }

        public PM_ProjectEventFilters_DTO GetTodofilters(string filter, Int64 projectId, Int64 UserId, string WhereClause = "")
        {


            DataSet ds= dal.GetDataset(filter, projectId, UserId);

           PM_ProjectEventFilters_DTO pl = new PM_ProjectEventFilters_DTO();
            
            pl.Statuses = ds.Tables[0].ToList<DTO.Status>();
            if (ds.Tables.Count > 1)
            {
                pl.Task = ds.Tables[1].ToList<PM_Task_DTO>();
            }
            if (ds.Tables.Count > 2)
            {
                pl.Entities = ds.Tables[2].ToList<PM_Entity_DTO>();
            }
            if (ds.Tables.Count >3)
            {
                pl.UserProjects = ds.Tables[3].ToList<Security.Entities.Sec_UserProjects>();
            }
            if (ds.Tables.Count > 4)
            {
                pl.Types = ds.Tables[4].ToList<PM_Type_DTO>();
            }
            return pl;
        }
    }
}
