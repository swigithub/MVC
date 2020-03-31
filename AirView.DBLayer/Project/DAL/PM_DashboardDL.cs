using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirView.DBLayer.Project.DAL
{
    public class PM_DashboardDL
    {
        public DataTable ToList(string filter, Int64 projectId = 0, Int64 MilestoneId = 0, int Page = 0, int Offset = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Dashboard");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", projectId, "@Page", Page, @"Offset", Offset, "@MilestoneId", MilestoneId));
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

        public DataTable GetDataTable(string filter, Int64 projectId, int Page, int Offset, Int64 SiteId = 0, string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null,Int64 MilestoneId=0,Int64 UserId=0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Dashboard");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", projectId, "@Page", Page, @"Offset", Offset, "@Tasks", TaskIds, "@Markets", LocationIds, "@FromDate", FromDate, "@ToDate", ToDate, "@Value1", SiteId, "@MilestoneId", MilestoneId,"@UserId",UserId));
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

        public DataTable GetStages(string filter, Int64 projectId, Int64 MilestoneId, Int64 SiteId = 0, string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null,Int64 UserId=0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Dashboard");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", projectId, "@MilestoneId", MilestoneId, "@Tasks", TaskIds, "@Markets", LocationIds, "@FromDate", FromDate, "@ToDate", ToDate, "@Value1", SiteId,"@UserId",UserId));
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

        public DataSet GetDashboardWO(string filter, Int64 projectId, int Page, int Offset, string searchoption, string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string UserId = null, bool IsActive = true)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Dashboard");

                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", projectId, "@Page", Page, "@Offset", Offset, "@FilterOption", searchoption, "@Tasks", TaskIds, "@Markets", LocationIds, "@FromDate", FromDate, "@ToDate", ToDate, "@Value1", Convert.ToInt64(UserId), "@IsActive", IsActive));
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
                loCommand.Dispose();
            }
        }

        public DataTable GetMileStoneValues(string filter, Int64 projectId = 0, Int64 MilestoneId = 0, string filterOption = null, string Value1 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Dashboard");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@MilestoneId", MilestoneId, "@FilterOption", filterOption, "@ProjectId", projectId, "Value1", Value1));
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

        public DataTable GetDashboardCharts(string Filter, Int64 ProjectId = 0, Int64 MilestoneId = 0, string Value1 = null, string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string SearchFilter = null, string FilterOption = null, int Page = 0, int Offset = 0, string MapStatus = null, string MapType = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Dashboard");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId, "@MilestoneId", MilestoneId, "@Value1", Value1, "@Tasks", TaskIds, "@Markets", LocationIds, "@FromDate", FromDate, "@ToDate", ToDate, "@SearchFilter", SearchFilter, "@FilterOption", FilterOption, "@Page", Page, "@Offset", Offset, "@MapStatus", MapStatus, "@MapType", MapType));
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

        public DataTable GetSandbox(string Filter, Int64 ProjectId, string Value, string Category, string Name, Int64 MilestoneId = 0, string Value1 = null, string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string SearchFilter = null, string FilterOption = null, int Page = 0, int Offset = 0, string MapStatus = null, string MapType = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Sandbox");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId, "@DataValue", Value, "@XDataSeries", Category, "@YDataSeries", Name,"@Tasks", TaskIds, "@Markets", LocationIds, "@FromDate", FromDate, "@ToDate", ToDate));
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

        public bool SaveSites(string Filter, Int64 ProjectId,string TaskIds, string Value, string Category, string Name, string Selected, string Unselected)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Sandbox");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result= DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId, "@DataValue", Value, "@XDataSeries", Category, "@YDataSeries", Name, "@Selected", Selected, "@Unselected", Unselected, "@Task", TaskIds));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch(Exception ex)
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