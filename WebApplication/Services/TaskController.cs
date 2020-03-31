using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.Model;
using Library.SWI.Project.BLL;
using Library.SWI.Project.DAL;
using Library.SWI.Project.Model;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.Security.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using WebApplication.Areas.Project.View_Models;

namespace WebApplication.Services
{
    public class TaskController : ApiController
    {
        [System.Web.Http.Route("swi/pm/_tasks"), System.Web.Http.HttpPost, System.Web.Http.HttpGet]
        public Response _Tasks(string filter = "", string value = "", string value2 = "", bool Resources = false, Int64 ProjectId = 0, Int64 TaskId = 0)
        {
            Response _res = new Response();
            try
            {
                PM_TaskBL tb = new PM_TaskBL();
                var rec = tb.ToList(filter, value, ProjectId.ToString(), ProjectId, TaskId);
                if (Resources)
                {
                    PM_ProjectResourceBL prb = new PM_ProjectResourceBL();

                    foreach (var item in rec)
                    {
                        item.ProjectResources = prb.ToList("byTaskId", item.TaskId.ToString(), ProjectId, TaskId);
                    }
                }
                if (filter == "ByTaskTypeKeyCode")
                {
                    AD_DefinationBL db = new AD_DefinationBL();
                    List<AD_Defination> ProjectStatus = db.ToList("byDefinationType", "Project Task Status");
                    foreach (var item in rec)
                    {

                        var Splited = ProjectStatus.Where(x => x.DefinationId == item.StatusId).FirstOrDefault();
                        if (Splited != null)
                        {
                            var CurrentStatus = Splited.DefinationName.ToLower();
                            switch (CurrentStatus)
                            {
                                case "active":
                                    item.Status = "STATUS_ACTIVE";
                                    break;
                                case "suspended":
                                    item.Status = "STATUS_SUSPENDED";
                                    break;
                                case "completed":
                                    item.Status = "STATUS_DONE";
                                    break;
                                case "cancelled":
                                    item.Status = "STATUS_FAILED";
                                    break;
                                case "undefined":
                                    item.Status = "STATUS_UNDEFINED";
                                    break;
                                case "planned":
                                    item.Status = "STATUS_PLANNED";
                                    break;
                                    //default :
                                    //   item.StatusId = 0;
                            }

                            //   item.Status = GanttStatusses.Where(x=>x.Split('_')[1].ToLower() == Splited.DefinationName.ToLower()).FirstOrDefault();
                        }
                        if (item.Status == null || item.Status == "")
                        {
                            item.Status = "STATUS_PLANNED";
                            item.StatusId = ProjectStatus.Where(x => x.DefinationName == "Planned").FirstOrDefault().PDefinationId;
                        }
                    }
                }
                _res.Value = rec;
                _res.Message = "Ok";
                _res.Status = "true";
                return _res;
            }

            catch (Exception _ex)
            {
                _res.Message = _ex.Message;
                _res.Status = "false";
                return _res;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.Route("swi/pm/_lookupdata")]
        public Response _LookupData(int _ProjectId)
        {
            Response _res = new Response();
            PM_LookupData _lookup = new PM_LookupData();
            try
            {
                PM_TaskEntryBL te = new PM_TaskEntryBL();
                AD_DefinationBL db = new AD_DefinationBL();
                _lookup.GroupResources = te.GET_Group_LIST("PopulateGroupResources").ToList();
                _lookup.ProjectResources = te.GET_Group_LIST("PopulateUserResourcesProject", _ProjectId).Where(x => x.ResourceName != null && x.ResourceName != "").ToList();

                _lookup.ProjectStatus = db.ToList("byDefinationType", "Project Task Status");
                _lookup.Priorities = db.ToList("byDefinationType", "Priority");
                Sec_UserBL ud = new Sec_UserBL();
                _res.Message = "Ok";
                _res.Status = "true";
                _res.Value = _lookup;
                return _res;
            }
            catch (Exception _ex)
            {
               _res.Message = _ex.Message;
                _res.Status = "false";
                _res.Value = _lookup;
                return _res;
            }
        }


        [Route("swi/pm/_task_add_update"),HttpPost]
        public Response _New(List<PM_Task> t)
        {
            Response res = new Response();
            try
            {
                if (t.Count > 0)
                {
                    AD_DefinationBL db = new AD_DefinationBL();
                    List<AD_Defination> ProjectStatus = db.ToList("byDefinationType", "Project Task Status");
                    PM_TaskBL pb = new PM_TaskBL();

                    dbDataTable dbdt = new dbDataTable();
                    PM_TaskDL ptd = new PM_TaskDL();
                    var dt1 = dbdt.TaskList();
                    int Counter = 1;
                    foreach (var item in t)
                    {
                        var Status = item.Status.Split('_');
                        if (Status.Length > 0)
                        {
                            switch (Status[1].ToLower())
                            {
                                case "active":
                                    var active = ProjectStatus.Where(x => x.DefinationName.ToLower() == "active").FirstOrDefault();
                                    if (active != null)
                                    {
                                        item.StatusId = active.DefinationId;
                                    }
                                    else
                                    {
                                        item.StatusId = 0;
                                    }
                                    break;
                                case "suspended":
                                    var suspend = ProjectStatus.Where(x => x.DefinationName.ToLower() == "suspended").FirstOrDefault();
                                    if (suspend != null)
                                    {
                                        item.StatusId = suspend.DefinationId;
                                    }
                                    else
                                    {
                                        item.StatusId = 0;
                                    }
                                    break;
                                case "done":
                                    var done = ProjectStatus.Where(x => x.DefinationName.ToLower() == "completed").FirstOrDefault();
                                    if (done != null)
                                    {
                                        item.StatusId = done.DefinationId;
                                    }
                                    else
                                    {
                                        item.StatusId = 0;
                                    }
                                    break;
                                case "failed":
                                    var failed = ProjectStatus.Where(x => x.DefinationName.ToLower() == "cancelled").FirstOrDefault();
                                    if (failed != null)
                                    {
                                        item.StatusId = failed.DefinationId;
                                    }
                                    else
                                    {
                                        item.StatusId = 0;
                                    }
                                    break;
                                case "undefined":
                                    var undefined = ProjectStatus.Where(x => x.DefinationName.ToLower() == "undefined").FirstOrDefault();
                                    if (undefined != null)
                                    {
                                        item.StatusId = undefined.DefinationId;
                                    }
                                    else
                                    {
                                        item.StatusId = 0;
                                    }
                                    break;
                                case "planned":
                                    var planned = ProjectStatus.Where(x => x.DefinationName.ToLower() == "planned").FirstOrDefault();
                                    if (planned != null)
                                    {
                                        item.StatusId = planned.DefinationId;
                                    }
                                    else
                                    {
                                        item.StatusId = 0;
                                    }
                                    break;
                                    //default :
                                    //   item.StatusId = 0;
                            }

                            if (item.StatusId == 0)
                            {
                                item.StatusId = ProjectStatus.Where(x => x.DefinationName.ToLower() == "planned").FirstOrDefault().PDefinationId;
                                item.Status = "STATUS_PLANNED";
                            }

                        }
                        var Parentid = item.PTaskId;
                        var Preid = item.PredecessorId;
                        if (string.IsNullOrEmpty(item.PTaskId) || item.PTaskId.StartsWith("tmp_"))
                        {

                            Parentid = "0";
                        }
                        else if (string.IsNullOrEmpty(item.PredecessorId) || item.PredecessorId.StartsWith("tmp_"))
                        {
                            Preid = "0";
                        }
                        myDataTable.AddRow(dt1, "Value1", item.TaskId, "Value2", Parentid,
                            "Value3", item.ProjectId, "Value4", item.TaskTypeId, "Value5", item.StatusId,
                            "Value6", item.PriorityId, "Value7", Preid, "Value8", item.Title
                            , "Value9", item.PlannedDate, "Value10", item.TargetDate, "Value11", item.ActualStartDate, "Value12", item.ActualEndDate,
                             "Value13", item.EstimatedStartDate, "Value14", item.EstimatedEndDate, "Value15", item.Description,
                            "Value16", item.IsEstimate, "Value17", item.ForecastedSites, "Value18", item.CompletionPercent, "Value19", item.BudgetCost,
                            "Value20", item.ActualCost, "Value21", item.MapCode, "Value22", item.MapColumn, "Value23", item.Color,
                            "Value24", item.IsActive, "Value25", item.ScopeId, "Value26", item.IsStartMilestone, "Value27", item.IsEndMilestone, "Value28", Counter
                            , "Value29", item.Level, "Value30", item.Duration,"Value31","Mobile"
                            );
                        Counter++;
                    }

                    DataTable NewTasks = ptd.Manage("Insert2", dt1);
                    List<PM_Task> ParentUpdate = new List<PM_Task>();

                    List<int> NewTasksId = (from row in NewTasks.AsEnumerable() select Convert.ToInt32(row["ID"])).ToList();
                    int NewTask = 0;
                     for (int i = 0; i < t.Count; i++)
                    {
                            if (t[i].TaskId == 0)
                            NewTask = NewTasksId[i];
                        t[i].TaskId = NewTasksId[i];
                    }
                    for (int i = 0; i < t.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(t[i].PTaskId) && t[i].PTaskId.StartsWith("tmp_"))
                        {

                            t[i].PTaskId = t.Where(x => x.id == t[i].PTaskId).FirstOrDefault().TaskId.ToString();
                            ParentUpdate.Add(t[i]);

                        }
                        else if (!string.IsNullOrEmpty(t[i].PredecessorId) && t[i].PredecessorId.StartsWith("tmp_"))
                        {
                            t[i].PredecessorId = t.Where(x => x.id == t[i].PredecessorId).FirstOrDefault().TaskId.ToString();
                            ParentUpdate.Add(t[i]);
                        }

                    }

                    if (ParentUpdate.Count > 0)
                    {
                        var dt3 = dbdt.TaskList();
                        foreach (var item in ParentUpdate)
                        {
                            myDataTable.AddRow(dt3, "Value1", item.TaskId, "Value2", item.PTaskId,
                                "Value3", item.ProjectId, "Value4", item.TaskTypeId, "Value5", item.StatusId,
                                "Value6", item.PriorityId, "Value7", item.PredecessorId, "Value8", item.Title
                                , "Value9", item.PlannedDate, "Value10", item.TargetDate, "Value11", item.ActualStartDate, "Value12", item.ActualEndDate,
                                 "Value13", item.EstimatedStartDate, "Value14", item.EstimatedEndDate, "Value15", item.Description,
                                "Value16", item.IsEstimate, "Value17", item.ForecastedSites, "Value18", item.CompletionPercent, "Value19", item.BudgetCost,
                                "Value20", item.ActualCost, "Value21", item.MapCode, "Value22", item.MapColumn, "Value23", item.Color,
                                "Value24", item.IsActive, "Value25", item.ScopeId, "Value26", item.IsStartMilestone, "Value27", item.IsEndMilestone, "Value28", item.SortOrder
                                 , "Value29", item.Level
                                );
                        }
                        DataTable NewTasks2 = ptd.Manage("UpdateParent", dt3);
                        List<int> NewTasksIds = (from row in NewTasks2.AsEnumerable() select Convert.ToInt32(row["ID"])).ToList();

                    }
                    res.Value = NewTask;
                    res.Status = "true";
                    res.Message = "Save successfully";
                }
                else { 
                res.Status = "false";
                res.Message = "No row effected";
                }
               
            }
            catch (Exception ex)
            {
                res.Status = "false";
                res.Message = ex.Message;
            }
            return res;
        }

        [HttpPost, System.Web.Http.Route("swi/pm/task_active")]
        public bool _Task_Active(PM_Task _task)
        {
            try
            {
                PM_TaskDL ptd = new PM_TaskDL();
                return ptd.Manage("Active_Deactive", _task.TaskId, _task.IsActive,_task.StatusId);
            }
            catch
            {
                return false;
            }
        }


        [HttpPost, Route("swi/pm/task_resource_save")]
        public Response SetResources(List<PM_TaskEntry> model)
        {
            Response res = new Response();
            try
            {
                PM_TaskEntryBL te = new PM_TaskEntryBL();
                for (int x = 0; x < model.Count; x++)
                {

                    if (model[x].PMTRId != 0)
                    {
                        res.Value = te.ManageResources("UpdateResources", model[x].ResourceId, model[x].GroupId, model[x].RACIId, model[x].ProjectId, model[x].TaskId, model[x].PMTRId, model[x].RatePerHour, model[x].IsDeleted);
                    }
                    else
                    {
                        res.Value = te.ManageResources("InsertResources", model[x].ResourceId, model[x].GroupId, model[x].RACIId, model[x].ProjectId, model[x].TaskId, model[x].RatePerHour, model[x].IsDeleted);
                    }

                    
                }
                res.Status = "true";
                res.Message = "Ok";
                return res;
            }
            catch (Exception ex)
            {
                res.Status = "false";
                res.Message = ex.Message;
                return res;
            }
            
        }

        [HttpGet,Route("swi/pm/task_resource")]
        public Response GetResources(int ProjectId, int TaskId)
        {
            Response _response = new Response();

            try {
                PM_TaskEntryBL te = new PM_TaskEntryBL();
                _response.Value = te.GET_Group_LIST("GetResources", ProjectId, TaskId).ToList();
                _response.Message = "Ok";
                _response.Status = "true";
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.Status = "false";
                return _response;
            }
        }
        
        [HttpGet,Route("swi/pm/task_resource_delete")]
        public Response DeleteResources(int ResourceId)
        {
            PM_TaskEntryBL te = new PM_TaskEntryBL();
            Response _res = new Response();
            try
            {
                te.ManageResources("DeleteResources", ResourceId);
                _res.Status = "true";
                _res.Message = "Ok";
                return _res;
            }
            catch (Exception ex)
            {
                _res.Status = "false";
                _res.Message = ex.Message;
                return _res;
            }
            
        }


    }
}
