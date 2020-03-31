using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.DAL
{
    public class PM_WorkLogDL
    {
        public dynamic Manage(string Filter, Int64 WLogId, Int64 ProjectId, Int64 ProjectSiteId, Int64 TaskId, Int64 IssueId, string LogType, Int64 UserId, DateTime LogDate, float LogHours, string Description, bool IsApproved)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_WorkLogManage");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@WLogId", WLogId, "@ProjectId", ProjectId, "@TaskId", TaskId, "@IssueId", IssueId, "@ProjectSiteId", ProjectSiteId, "@LogType", LogType,
                    "@UserId", UserId, "@LogDate", LogDate, "@LogHours", LogHours, "@Description", Description, "@IsApproved", IsApproved));
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

        public DataTable GetDataTable(string filter, string value1, string value2, string selectOption)
         {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetWorkLogs");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@StartDate", value1, "@EndDate", value2, "@SelectOption", selectOption));
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

        public DataTable GetDataTable(string filter, Int64 projectid = 0, string workgroups = "", string users = "",string logtype="", string startdate = "", string enddate = "",string userid = "0", string sitetaskid = "0", string projectsiteid = "0", string workgroupid = "0")
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetWorkLogs");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", projectid, "@WorkGroups", workgroups, "@Users", users,"@LogType",logtype, "@StartDate", startdate, "@EndDate", enddate,"@UserId",userid,"@SiteTaskId",sitetaskid,"@ProjectSiteId",projectsiteid,"@WorkgroupId",workgroupid));
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

        public DataTable GetDataTable(string Filter, Int64 TaskId, Int64 ProjectSiteId, string LogType, Int64 ProjectId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetWorkLogs");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TaskId", TaskId, "@ProjectSiteId", ProjectSiteId, "@LogType", LogType, "@ProjectId", ProjectId));
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

        public DataSet GetDataset(string filter="",Int64 projectId = 0,Int64 userid=0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetWorkLogs");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", projectId,"@UserId",userid));
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

        public bool Manage(string Filter, DataTable dt)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_WorkLogManage");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@Data", dt));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch
            {
                DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
    }
}
