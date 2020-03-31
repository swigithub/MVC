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

namespace Library.SWI.Project.BLL
{
    /*----MoB!----*/
    /*----19-10-2017----*/
    public class PM_ProjectBL
    {
        PM_ProjectDL pd = new PM_ProjectDL();

        public int Manage(string Filter, PM_Project p)
        {
            return pd.Manage(Filter,p.ProjectId,p.ProjectName,p.ScopeId,p.ClientId,p.EndClientId,p.ActualStartDate,p.ActualEndDate,p.StatusId,p.Color
                ,p.Description,p.IsActive,p.PriorityId, p.IsEstimate,p.BudgetCost,p.TargetDate,p.TaskTypeId,p.CompletionPercent,p.IsWoLinked,p.PlannedDate,p.WorkingDays, p.ManagerId,p.EntityId, p.IsWorkflowAllowed,p.CategoryId,p.CurrencyId);
        }

        public bool ProjectUserPermission(string filter,Int64 UserID, Int64 ProjectId)
        {
            pd.Manage(filter, UserID, ProjectId);
            return true;
        }

        public bool ManageProjectPlan(string Filter, DataTable dt)
        {
            return true;  // pd.Manage(Filter, p.ProjectId);
        }

        public bool UpdateMsWindow(string Filter, PM_ProjectSite p)
        {
            return pd.UpdateMsWindow(Filter, p.ProjectSiteId, p.StatusId,p.MSWindowId,p.AlarmId,p.CreatedBy,p.Notes,p.GNGId,p.IsAddionalSite,p.ActivityTypeId,p.ItemTypeId,p.eNB,p.ExtendedeNB,p.EquipmentId,p.AOTSCR,p.FilePath);
        }
        
         public List<PM_Projects_DTO> DTOToList(string Filter, string value = null, Int64 _userId = 0)
        {
            DataTable dt = pd.GetDataTable(Filter, value, _userId);
            return dt.ToList<PM_Projects_DTO>();
        }

        public List<PM_Project> ToList(string Filter, string value = null,Int64 _userId=0)
        {
            DataTable dt = pd.GetDataTable(Filter, value,_userId);
            return dt.ToList<PM_Project>();
        }

        public PM_ProjectLookup GetLookup(string Filter, string @Value = null, Int64 _userId = 0, string StatusIds = null, string ProritysIds = null, string ClientsIds = null, DateTime? ToDate = null, DateTime? FromDate = null)
        {
            DataSet ds = pd.GetDataset(Filter, Value, _userId, StatusIds, ProritysIds, ClientsIds, ToDate, FromDate);
            PM_ProjectLookup pl = new PM_ProjectLookup();
            pl.Statuses=ds.Tables[0].ToList<AirView.DBLayer.Project.Model.Status>();
            if(ds.Tables.Count >1) { 
            pl.Priorities = ds.Tables[1].ToList<Priority>();
            }
            if (ds.Tables.Count > 2)
            {
                pl.clients = ds.Tables[2].ToList<Client>();
            }
            return pl;
        }
        public List<PM_Projects_DTO> ToList(string Filter, string IsActive = null, string StatusIds = null, string ProritysIds = null, string ClientsIds = null,  DateTime? ToDate = null, DateTime? FromDate = null, Int64 _userId=0)
        {
            DataTable dt = pd.GetDataTable(Filter, IsActive,_userId,StatusIds,ProritysIds,ClientsIds,ToDate,FromDate);
            return dt.ToList<PM_Projects_DTO>();
        }
        public List<PM_Project> Paging(string Filter,string value, int Skip, int Take, string Search, ref int TotalCount)
        {
            try
            {

             //   DataSet ds = ud.GetDataSet("Paging", Skip.ToString(), Take.ToString(), Search, Id);
                DataTable ds= pd.GetDataTable(Filter, value);
               
              
                if(Search != "")
                {
                    var obj = ds.ToList<PM_Project>().Where(x=>x.ProjectName.Contains(Search)).Skip(Take*Skip).Take(Take).ToList();
                    TotalCount = ds.ToList<PM_Project>().Where(x => x.ProjectName == Search).Count();
                    return obj;
                }
                else
                {
                    var obj = ds.ToList<PM_Project>().Skip(Take * Skip).Take(Take).ToList();
                    TotalCount = ds.ToList<PM_Project>().Count();
                    return obj;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<PM_ProjectSite> ProjectSiteList(string filter, Int64 ProjectId, Int64 SiteId, int UserId = 0, string Value1 = null)
        {
            DataTable dt = pd.GetProjectSiteTable(filter, ProjectId,SiteId,UserId,Value1);
            return dt.ToList<PM_ProjectSite>();
        }

        public List<PM_ProjectFACode> ProjectFACode(string filter, Int64 ProjectId)
        {
            DataTable dt = pd.GetProjectSite(filter, ProjectId);
            return dt.ToList<PM_ProjectFACode>();
        }

        public List<ProjectSites> Get(string filter, Int64 ProjectId)
        {
            DataTable dt = pd.GetProjectSite(filter, ProjectId);
            return dt.ToList<ProjectSites>();
        }

        public List<PM_Task> GetTasks(string filter, Int64 ProjectSiteId)
        {
            DataTable dt = pd.GetProjectSiteTasks(filter, ProjectSiteId);
            return dt.ToList<PM_Task>();
        }

        public List<PM_ProjectSite> PM_PlanProject(string filter, Int64 projectId = 0, Int64 revisionId = 0, DateTime? fromDate = null, DateTime? toDate = null, string locationIds = null, string taskIds = null, string siteStatus = null, Int64 userId = 0)
        {
            DataTable dt = pd.GetPM_PlanProject(filter, projectId, revisionId, fromDate, toDate, locationIds, taskIds, siteStatus, userId);
            return dt.ToList<PM_ProjectSite>();
        }

        public PM_Project ToSingle(string Filter, string value = null)
        {
            try
            {
                DataTable dt = pd.GetDataTable(Filter, value);
                return dt.ToList<PM_Project>().FirstOrDefault();
            }
            catch (Exception)
            {

                return null;
            }
            
        }
    }
}
