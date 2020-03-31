using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library.SWI.Project.DAL
{
    /*----MoB!----*/
    /*----19-10-2017----*/

    public class PM_TaskDL
    {

         public dynamic Manage(string Filter, Int64 TaskId, string PTaskId, Int64 ProjectId, Int64 PriorityId, Int64 TaskTypeId, Int64 StatusId, string PredecessorId, string Title, DateTime? PlannedDate,
                      DateTime? ActualStartDate, DateTime? ActualEndDate, DateTime? EstimatedStartDate, DateTime? EstimatedEndDate, string Description, bool IsEstimate
                      , bool IsActive, DateTime? TargetDate, int ForecastedSites, float CompletionPercent, float BudgetCost, float ActualCost, string MapCode, string MapColumn, string Color, Int64 ScopeId, bool IsStartMilestone, bool IsEndMilestone, Int64 SortOrder)

        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageTasks");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);

                var id = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TaskId", TaskId, "@PTaskId", PTaskId, "@ProjectId", ProjectId, "@TaskTypeId", TaskTypeId, "@StatusId", StatusId, "@PriorityId", PriorityId, "@PredecessorId", PredecessorId, "@Title", Title, "@PlannedDate", PlannedDate
                    , "@TargetDate", TargetDate, "@ActualStartDate", ActualStartDate, "@ActualEndDate", ActualEndDate, "@EstimatedStartDate", EstimatedStartDate, "@EstimatedEndDate", EstimatedEndDate, "@Description", Description, "@IsEstimate", IsEstimate, "@ForecastedSites", ForecastedSites, "@CompletionPercent", CompletionPercent,
                    "@BudgetCost", BudgetCost, "@ActualCost", ActualCost, "@MapCode", MapCode, "@MapColumn", MapColumn, "@Color", Color, "@IsActive", IsActive, "@ScopeId", ScopeId, "@IsStartMilestone", IsStartMilestone, "@IsEndMilestone", IsEndMilestone, "@SortOrder", SortOrder));
                DataContext.EndTransaction(loCommand);
                int result = Convert.ToInt32(loCommand.Parameters["@RETURN_VALUE"].Value);
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

        public bool Manage(string Filter, Int64 TaskId,bool IsActive,Int64 StatusId)

        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageTasks");

                loCommand = DataContext.StartTransaction(loCommand);

                bool _result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TaskId", TaskId, "@IsActive", IsActive, "@StatusId", StatusId));
                DataContext.EndTransaction(loCommand);
                return _result;
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
        internal bool SaveTodo(string Filter, long TodoId, string Description, string Type, string Status, long CreatedById, DateTime CreatedOn, DateTime ToDoDateTime, string ToDoTitle, string ProjectId, Int64? SiteId, Int64? TaskId,string AssigntoIds)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageToDo");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TodoId", TodoId, "@Description", Description, "@Type", Type, "@Status", Status, "@CreatedById", CreatedById, "CreatedOn", CreatedOn, "@ToDoDateTime", ToDoDateTime, "@ToDoTitle", ToDoTitle, "@ProjectId", ProjectId, "@SiteId", SiteId, "@TaskId", TaskId, "@AssignedToIds", AssigntoIds));
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

        internal bool EditTodo(string Filter, long TodoId, string Description, string Type, string Status, DateTime ToDoDateTime, string ToDoTitle,long? SiteId,long? TaskId, string AssigntoIds)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageToDo");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TodoId", TodoId, "@Description", Description, "@Type", Type, "@Status", Status, "@ToDoDateTime", ToDoDateTime, "@ToDoTitle", ToDoTitle, "SiteId",SiteId,"TaskId",TaskId, "@AssignedToIds", AssigntoIds));
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

        public DataTable GetDataTable(string filter, string Value1 = null, string Value2 = null, string Value3 = null, Int64 ProjectId = 0, Int64 TaskId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetTasks");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value1", Value1, "@Value2", Value2, "@Value3", Value3, "@ProjectId", ProjectId, "@TaskId", TaskId));
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

        public dynamic ManageSiteTask(string Filter, Int64 SiteTaskId, Int64 ProjectSiteId, Int64 TaskId, Int64 PTaskId, Int64 PredecessorId, Int64 TaskTypeId,Int64 CreatedBy ,string TaskTitle, Int64 StatusId, Int64 PriorityId, DateTime? ForecastDate, DateTime? ForecastStartDate, DateTime? ForecastEndDate, DateTime? PlannedDate, DateTime? TargetDate, DateTime? ActualStartDate, DateTime? ActualEndDate, float CompletionPercent, float BudgetCost, float ActualCost, string MapCode, string MapColumn, bool IsActive,string Description=null,string AssignTo=null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageSiteTasks");
                loCommand = DataContext.StartTransaction(loCommand);

                //int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteTaskId", SiteTaskId, "@ProjectSiteId", ProjectSiteId, "@TaskId", TaskId, "@PTaskId", PTaskId, "@PredecessorId", PredecessorId,
                //    "@TaskTypeId", TaskTypeId, "@TaskTitle", TaskTitle,"@StatusId", StatusId, "@PriorityId", PriorityId, "@ForecastDate", "@ForecastDate", ForecastDate, "@PlannedDate", PlannedDate,
                //    "@TargetDate", TargetDate, "@ActualStartDate", "@ActualStartDate", ActualStartDate, "@ActualEndDate", ActualEndDate, "@CompletionPercent", CompletionPercent,
                //    "@BudgetCost", BudgetCost, "@ActualCost", ActualCost, "@MapCode", MapCode, "@MapColumn", MapColumn,"@",IsActive));

                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectSiteId", ProjectSiteId, "@TaskId", TaskId, "@CreatedBy",CreatedBy, "@ForecastDate", ForecastDate, "@PlannedDate", PlannedDate,
                    "@TargetDate", TargetDate, "@StatusId",StatusId, "@SiteTaskId", SiteTaskId, "@ActualStartDate", ActualStartDate, "@ActualEndDate", ActualEndDate, "@Description", Description, "@AssignTo",AssignTo,"@ForecastStartDate",ForecastStartDate,"@ForecastEndDate",ForecastEndDate));
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

        public dynamic ManageSiteTask(string Filter, DataTable dt)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageSiteTasks");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List", dt));
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
        public dynamic ManageTaskSortOrder(string Filter, Int64 TaskId,  string SortDirection)

        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageTasks");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);

                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TaskId", TaskId, "@SortDirection", SortDirection));
                DataContext.EndTransaction(loCommand);
                int result = Convert.ToInt32(loCommand.Parameters["@RETURN_VALUE"].Value);
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
        public dynamic Manage(string Filter, DataTable dt)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageTasks");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List", dt));
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



    }
}