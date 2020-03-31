using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirView.DBLayer.Project.DAL
{
    public class PM_IssueDL
    {
        public dynamic Manage(string Filter, Int64 ProjectId, Int64 IssueId, Int64 TaskId, Int64 ProjectSiteId, Int64 TaskTypeId, Int64 IssuePriorityId, Int64 IssueStatusId, Int64 SeverityId,string RequestedBy
                             ,string Description, string TicketTypeId, Int64 RequestedById, DateTime? RequestDate, string AssignedToId,Int64 IssueCategoryId, Int64 ReasonId, Int64 IssueById, DateTime? ForecastDate
                             , DateTime? TargetDate, DateTime? ActualStartDate, DateTime? ActualEndDate,DateTime? RequestedByDate, bool IsUnavoidable, long activityTypeId, long itemTypeId, string eNB, string extendedeNB, string equipmentId, string aOTSCR, string filePath,Int64 UserId=0,Int64 AlarmId=0, Int64 MSWindowId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageIssues");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId, "@IssueId", IssueId, "@TaskId", TaskId,
                                                        "@ProjectSiteId", ProjectSiteId, "@TaskTypeId", TaskTypeId, "@IssuePriorityId", IssuePriorityId, "@IssueStatusId", IssueStatusId, "@Description", Description, "@SeverityId", SeverityId,
                                                        "@RequestedBy", RequestedBy, "@RequestedById", RequestedById, "@RequestDate", RequestDate, "@AssignedToId", AssignedToId, "@ReasonId", ReasonId, "@IssueById", IssueById, "@ForecastDate", ForecastDate
                                                        , "@TargetDate", TargetDate, "@ActualStartDate", ActualStartDate, "@ActualEndDate", ActualEndDate, "@RequestedByDate", RequestedByDate, "@IsUnavoidable", IsUnavoidable,
                                                        "@ActivityTypeId", activityTypeId, "@ItemTypeId", itemTypeId, "@ENB",
                                                        eNB, "@ExtendedeNB", extendedeNB, "@EquipmentId", equipmentId, "@AOTSCR", aOTSCR, "@FilePath", filePath, "@UserId", UserId, "@IssueCategoryId", IssueCategoryId, "@AlarmId",AlarmId, "@MSWindowId", MSWindowId, "@TicketTypeId", TicketTypeId));
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

        public dynamic ManageIssueLog(string filter, long issueId, long statusId, long userId, string description, DateTime createdOn)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageIssues");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", filter, "@IssueId", issueId,"@StatusId", statusId, "@UserId", userId,"@Description", description, "@CreatedOn", createdOn));
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

        internal DataTable GetIssueLog(string filter, long issueId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetIssues");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@IssueId",issueId));
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

        public DataTable ToList(string filter, long projectId, Int64 TaskId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetResources");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", projectId, "@TaskId", TaskId));
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


        public DataTable GetDataTable(string filter, Int64 ProjectId=0, Int64 IssueId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetIssues");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", ProjectId, "@IssueId", IssueId));
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