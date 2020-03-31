using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Project.DTO;
using AirView.DBLayer.Project.Model;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.BLL
{
  public  class PM_ProjectSitesBL
    {
        PM_ProjectSitesDL wod = new PM_ProjectSitesDL();

     

        //public bool Insert(string filter, List<ProjectSites> wo,long UserId,string Value1=null,string Value2=null)
        //{
        //    try
        //    {
        //        dbDataTable ddt = new dbDataTable();
        //        DataTable dt = ddt.ListProjectSites();
        //        if (wo != null)
        //        {
        //            foreach (var item in wo)
        //            {
        //                myDataTable.AddRow(dt, "Value1",item.Project, "Value2", item.SiteCode, "Value3", item.ReceivedOn, "Value4", item.SiteTypeId, "Value5", item.ClusterCode,
        //                                    "Value6", item.Market, "Value7", item.Color, "Value8",item.Scope, "Value9",UserId,"Value10",item.SiteClassId,"Value11",item.siteLatitude,"Value12",item.siteLongitude,"Value13",item.Description,"Value14",item.SiteName,"Value15",item.Client);
        //            }
        //        }

        //        wod.Manage(filter, dt, UserId,Value1);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        public bool Insert(string filter, List<ProjectSites> wo, long UserId, Int64 ProjectSiteId = 0, string Value2 = null)
        {
            try
            {
                dbDataTable ddt = new dbDataTable();
                DataTable dt = ddt.ListProjectSites();
                if (wo != null)
                {
                    foreach (var item in wo)
                    {
                        //myDataTable.AddRow(dt, "ProjectId", item.Project, "SiteCode", item.SiteCode, "ReceivedOn", item.ReceivedOn, "SiteTypeId", item.SiteTypeId, "ClusterCode", item.ClusterCode,
                        //                    "MarketId", item.Market, "ColorId", item.Color, "ScopeId", item.Scope, "UserId", UserId, "SiteClassId", item.SiteClassId, "Latitude", item.siteLatitude, "Longitude", item.siteLongitude, 
                        //                    "Description", item.Description, "SiteName", item.SiteName, "ClientId", item.Client,
                        //                    "USID", item.USID, "SubMarket", item.SubMarket, "vMME", item.vMME, "ControlledIntro", item.ControlledIntro,
                        //                    "SuperBowl", item.SuperBowl, "DASInBuild", item.DASInBuild, "FirstNetRAN", item.FirstNetRAN, "IPlanJob", item.IPlanJob,
                        //                    "PaceNo", item.PaceNo, "IPlanIssueDate", item.IPlanIssueDate);

                        myDataTable.AddRow(dt,
                            "WoRefId", null,
                            "ProjectId", item.ProjectId,
                            "SiteCode", item.SiteCode,
                            "SiteName", item.SiteName,
                            "SiteDate", null,
                            "SiteTypeId", item.SiteTypeId,
                            "SiteClassId", item.SiteClassId,
                           "Latitude", item.siteLatitude,
                           "Longitude", item.siteLongitude,
                           "RevisionId", null,
                           "PMCode", null,
                           "PMRefId", null,
                           "ClusterId", item.clusterId,
                           "ClusterCode", item.ClusterCode,
                           "CityId", null,
                           "StatusId", null,
                           "MSWindowId", null,
                           "PriorityId", null,
                           "ColorId", item.Color,
                           "CreatedOn", null,
                           "CreatedBy", null,
                           "IsActive", null,
                           "BudgetCost", null,
                           "ActualCost", null,
                           "Description", item.Description,
                           "ClientId", item.Client,
                           "ScopeId", item.Scope,
                           "ReceivedOn", item.ReceivedOn,
                           "Address", null,
                           "PlannedDate", null,
                           "TargetDate", null,
                           "ActualStartDate", null,
                           "ActualEndDate", null,
                           "EstimatedStartDate", null,
                           "EstimatedEndDate", null,
                           "MilestoneId", null,
                           "StageId", null,
                           "FACode", item.SiteCode,
                           "USID", item.USID,
                           "CommonId", null,
                           "MarketId", item.Market,
                           "SubMarketId", null,
                           "CountyId", null,
                           "vMME", item.vMME,
                           "ControlledIntro", item.ControlledIntro,
                           "SuperBowl", item.SuperBowl,
                           "isDASInBuild", item.isDASInBuild,
                           "FirstNetRAN", item.FirstNetRAN,
                           "IPlanJob", item.IPlanJob,
                           "PaceNo", item.PaceNo,
                           "IPlanIssueDate", item.IPlanIssueDate,
                           "SubMarket", item.SubMarket,
                           "County", null,
                           "AlarmId", null,
                           "Value1", null,
                           "Value2", null,
                           "Value3", null,
                           "Value4", null,
                           "Value5", null,
                           "Value6", null,
                           "Value7", null,
                           "Value8", null,
                           "Value9", null,
                           "Value10", null);
                    }
                }

              return  wod.Manage(filter, dt, UserId, ProjectSiteId);
                //return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Update(string filter,ProjectSites wo, long? UserId)
        {
            try
            {
                dbDataTable ddt = new dbDataTable();
                //DataTable dt = ddt.List();
                //      myDataTable.AddRow(dt, "Value1", wo.ProjectId, "Value2", wo.SiteCode, "Value3", wo.ReceivedOn, "Value4", wo.SiteTypeId, "Value5", wo.ClusterCode,
                //                            "Value6", wo.Market, "Value7", wo.Color, "Value8", wo.Scope, "Value9", wo.SiteName, "Value10", wo.SiteClassId, "Value11", wo.siteLatitude, 
                //"Value12", wo.siteLongitude, "Value13", wo.Description, "Value14", wo.SiteAddress, "Value15", wo.Client);

                DataTable dt = ddt.ListProjectSites();
                myDataTable.AddRow(dt,
                            "WoRefId", null,
                            "ProjectId", wo.ProjectId,
                            "SiteCode", wo.SiteCode,
                            "SiteName", wo.SiteName,
                            "SiteDate", null,
                            "SiteTypeId", wo.SiteTypeId,
                            "SiteClassId", wo.SiteClassId,
                           "Latitude", wo.siteLatitude,
                           "Longitude", wo.siteLongitude,
                           "RevisionId", null,
                           "PMCode", null,
                           "PMRefId", null,
                           "ClusterId", null,
                           "ClusterCode", wo.ClusterCode,
                           "CityId", wo.Market,
                           "StatusId", null,
                           "MSWindowId", null,
                           "PriorityId", null,
                           "ColorId", wo.Color,
                           "CreatedOn", null,
                           "CreatedBy", null,
                           "IsActive", null,
                           "BudgetCost", null,
                           "ActualCost", null,
                           "Description", wo.Description,
                           "ClientId", wo.Client,
                           "ScopeId", wo.Scope,
                           "ReceivedOn", wo.ReceivedOn,
                           "Address", wo.SiteAddress,
                           "PlannedDate", null,
                           "TargetDate", null,
                           "ActualStartDate", null,
                           "ActualEndDate", null,
                           "EstimatedStartDate", null,
                           "EstimatedEndDate", null,
                           "MilestoneId", null,
                           "StageId", null,
                           "FACode", wo.SiteCode,
                           "USID", wo.USID,
                           "CommonId", null,
                           "MarketId", wo.Market,
                           "SubMarketId", null,
                           "CountyId", null,
                           "vMME", wo.vMME,
                           "ControlledIntro", wo.ControlledIntro,
                           "SuperBowl", wo.SuperBowl,
                           "isDASInBuild", wo.isDASInBuild,
                           "FirstNetRAN", wo.FirstNetRAN,
                           "IPlanJob", wo.IPlanJob,
                           "PaceNo", wo.PaceNo,
                           "IPlanIssueDate", wo.IPlanIssueDate,
                           "SubMarket", wo.SubMarket,
                           "County", null,
                           "AlarmId", null,
                           "Value1", null,
                           "Value2", null,
                           "Value3", null,
                           "Value4", null,
                           "Value5", null,
                           "Value6", null,
                           "Value7", null,
                           "Value8", null,
                           "Value9", null,
                           "Value10", null);
                wod.Manage(filter, dt, UserId,wo.ProjectSiteId);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        //public bool ActiveDeactive(string filter,Int64 UserId=0,string Value1=null,string Value2=null)
        //{
        //    try
        //    {

        //        wod.Manage(filter, null, UserId,Value1,Value2);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}


        public bool ActiveDeactive(string filter, Int64 ProjectSiteId, bool IsActive)
        {
            try
            {
                return wod.Manage(filter, ProjectSiteId, IsActive);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<ProjectSites> ToList(string filter, string value = null, string value1 = null, string value2 = null, string value3 = null, string value4 = null)
        {
            DataTable dt = wod.Get(filter, value, value1, value2);

            List<ProjectSites> ps = dt.ToList<ProjectSites>();
            return ps;
        }

        public int IsSiteCodeExistInProject(string filter, Int64? ProjectSiteId, Int64? ProjectId, string SiteCode)
        {
            int value = wod.GetScaler(filter,null, null,null, ProjectSiteId, ProjectId, SiteCode);
            return value;
        }

        public List<ProjectSites> Paging(int Skip, int Take, string Search, ref int TotalCount, string Id = null, bool IsActive = true)
        {
            try
            {

                DataSet ds = wod.GetDataSet("Paging", Skip.ToString(), Take.ToString(), Search , null,null,null, null,Skip, Id.ToString(), IsActive);
                DataTable Count = ds.Tables[1];
                if (Count != null && Count.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Count.Rows[0]["TotalRecord"].ToString());
                    var List = ds.Tables[0].ToList<ProjectSites>();
                    return List;
                }
                
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<PM_ProjectEntity_DTO> GetEntitiesByProjectId(string filter, int projectId, long _userId,string SearchKey = null, string statusIds=null, string priorityIds = null, string clientIds = null, DateTime? toDate=null, DateTime? fromDate = null)
        {
            DataTable dt = wod.GetProjectSites(filter,statusIds,priorityIds,clientIds,toDate.ToString(),fromDate.ToString(),SearchKey, _userId.ToString(),0, projectId.ToString(),true);
            var ps = dt.ToList<PM_ProjectEntity_DTO>(); 
            return ps;
        }
        public PM_ProjectEntityFilters_DTO GetEntitiesFilters(string filter, int projectId, long _userId, string SearchKey = null, string statusIds = null, string priorityIds = null, string clientIds = null, DateTime? toDate = null, DateTime? fromDate = null)
        {
            DataSet ds = wod.GetDataSet(filter, statusIds, priorityIds, clientIds, toDate.ToString(), fromDate.ToString(), SearchKey, _userId.ToString(), 0, projectId.ToString(), true);

            PM_ProjectEntityFilters_DTO pl = new PM_ProjectEntityFilters_DTO();
            pl.Statuses = ds.Tables[0].ToList<Project.DTO.Status>();
            if (ds.Tables.Count > 1)
            {
                pl.Types = ds.Tables[1].ToList<Project.DTO.Type>();
            }
            if (ds.Tables.Count > 2)
            {
                pl.clients = ds.Tables[2].ToList<Client>();
            }
            if (ds.Tables.Count > 3)
            {
                pl.Markets = ds.Tables[3].ToList<Market>();
            }
            return pl;
        }
    }
}
