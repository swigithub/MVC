using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library.SWI.Project.DAL
{
    /*----MoB!----*/
    /*----19-10-2017----*/

    public class PM_ProjectDL
    {
        public int Manage(string Filter, Int64 ProjectId, string ProjectName, string ScopeId, Int64 ClientId, Int64 EndClientId, DateTime? ActualStartDate, DateTime? ActualEndDate,
                             Int64 StatusId, string Color, string Description, bool IsActive, Int64 PriorityId, bool IsEstimate, float BudgetCost, DateTime? TargetDate, Int64 TaskTypeId
                             , double CompletionPercent, bool IsWoLinked, DateTime? PlannedDate,string WorkingDays, Int64 ProjectManagerId,int EntityId, bool? IsWorkflowAllowed,Int64 CategoryId, Int64 CurrencyId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageProjects");
                loCommand = DataContext.StartTransaction(loCommand);
                int result = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId, "@ProjectName", ProjectName, "@ScopeId", ScopeId,
                                                        "@ClientId", ClientId, "@EndClientId", EndClientId, "@ActualStartDate", ActualStartDate, "@ActualEndDate", ActualEndDate, "@StatusId", StatusId,
                                                        "@Color", Color, "@Description", Description, "@IsActive", IsActive, "@PriorityId", PriorityId, "@IsEstimate", IsEstimate, "@BudgetCost", BudgetCost, "@TargetDate", TargetDate,
                                                        "@TaskTypeId", TaskTypeId, "@CompletionPercent", CompletionPercent, "IsWoLinked", IsWoLinked, "@PlannedDate", PlannedDate , "@WorkingDays", WorkingDays, "@ProjectManagerId", ProjectManagerId,"@EntityId", EntityId, "@IsWorkflowAllowed", IsWorkflowAllowed, "@CategoryId", CategoryId, "@CurrencyId", CurrencyId));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public int Manage(string Filter, Int64 ProjectManagerId, Int64 ProjectId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageProjects");
                loCommand = DataContext.StartTransaction(loCommand);
                int result = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@PID", ProjectId, "@MID", ProjectManagerId));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public dynamic ManageProjectPlan(string Filter, Int64 ProjectId, DataTable dt)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageProjectPlan");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId, "@Plan", dt));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public dynamic UpdateMsWindow(string filter, long projectSiteId, long statusId, long mSWindowId,Int64 AlarmId,Int64 CreatedBy,string Notes,Int64 GNGId,bool IsAddionalSite, long activityTypeId, long itemTypeId, string eNB, string extendedeNB, string equipmentId, string aOTSCR, string filePath)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageProjectSites");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectSiteId", projectSiteId,
                    "@StatusId", statusId, "@MSWindowId", mSWindowId,
                    "@AlarmId", AlarmId, "@CreatedBy", CreatedBy, "@Notes",Notes, "@GNGId", GNGId, "@IsAddionalSite", IsAddionalSite,
                    "@ActivityTypeId", activityTypeId, "@ItemTypeId", itemTypeId, "@ENB",
                    eNB, "@ExtendedeNB", extendedeNB, "@EquipmentId", equipmentId, "@AOTSCR", aOTSCR, "@FilePath", filePath));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataSet GetDataset(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetProjects");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value", Value));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataSet GetDataset(string filter, string Value = null, Int64 _userId = 0, string StatusIds = null, string ProritysIds = null, string ClientsIds = null, DateTime? ToDate = null, DateTime? FromDate = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetProjects");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value", Value, "@StatusIds", StatusIds, "@ProritysIds", ProritysIds, "@ClientsIds", ClientsIds, "@ToDate", ToDate, "@FromDate", FromDate, "@UserId", _userId));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable GetDataTable(string filter, string Value = null, Int64 _userId = 0, string StatusIds = null, string ProritysIds = null, string ClientsIds = null, DateTime? ToDate=null , DateTime? FromDate = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetProjects");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value", Value, "@StatusIds", StatusIds, "@ProritysIds", ProritysIds, "@ClientsIds", ClientsIds, "@ToDate", ToDate, "@FromDate", FromDate, "@UserId", _userId));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable GetProjectSiteTable(string filter,Int64 ProjectId, Int64 SiteId, int UserId = 0, string Value1 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetProjectSites");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value1", SiteId));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable GetProjectSite(string filter, Int64 ProjectId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetProjectSites");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", ProjectId));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable GetProjectSiteTasks(string filter, Int64 ProjectSiteId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetProjectSites");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectSiteId", ProjectSiteId));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable GetPM_PlanProject(string filter, Int64 projectId = 0, Int64 revisionId = 0, DateTime? fromDate = null, DateTime? toDate = null, string locationIds = null, string taskIds = null, string stiteStatus = null, Int64 userId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_PlanProject");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", projectId, "@RevisionId", revisionId, "@FromDate", fromDate, "@ToDate", toDate, "@Markets", locationIds, "@Tasks", taskIds, "@SiteStatus", stiteStatus, "@UserId", userId));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
    }
}