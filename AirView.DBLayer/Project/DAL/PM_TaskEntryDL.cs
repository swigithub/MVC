using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirView.DBLayer.Project.DAL
{
    public class PM_TaskEntryDL
    {
        public DataTable Get(string filter, Int64 NodeId = 0,Int64 SiteId=0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageTaskEntry");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectSiteId", SiteId, "@TaskId", NodeId));
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


        public dynamic Manage(string Filter, DataTable dt)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageTaskEntry");
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

        public dynamic Manage(string Filter, Int64 EntryId, Int64 ProjectId,Int64 ProjectSiteId, Int64 TaskId, Int64 FormId, string FormValue, Int64 CreatedById, DateTime CreatedOn)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageTaskEntry");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@EntryId", EntryId, "@ProjectId", ProjectId, "@ProjectSiteId", ProjectSiteId, "@TaskId", TaskId, "@FormId", FormId, "@FormValue", FormValue, "@CreatedById", CreatedById, "@CreatedOn", CreatedOn));
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
        

        public dynamic ManageResources(string Filter, Int64 ResourceId, Int64 GroupId, Int64 RACIId, Int64 ProjectId, Int64 TaskId, Int64 RatePerHour,bool IsDeleted)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_TaskResource");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ResourceId", ResourceId, "@GroupId", GroupId, "@RACIId", RACIId, "@ProjectId", ProjectId, "@TaskId", TaskId, "@RatePerHour", RatePerHour, "@IsDeleted", IsDeleted));
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

        public dynamic ManageResources(string Filter, Int64 PMTRId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_TaskResource");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@PMTRId", PMTRId));
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

        public dynamic ManageResources(string Filter, Int64 ResourceId, Int64 GroupId, Int64 RACIId, Int64 ProjectId, Int64 TaskId, Int64 PMTRId, Int64 RatePerHour,bool IsDeleted)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_TaskResource");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ResourceId", ResourceId, "@GroupId", GroupId, "@RACIId", RACIId, "@ProjectId", ProjectId, "@TaskId", TaskId, "@PMTRId", PMTRId, "@RatePerHour", RatePerHour, "@IsDeleted", IsDeleted));
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

        public DataTable GET_Group_LIST(string Filter , int ProjectId, int TaskId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_TaskResource");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId, "@TaskId", TaskId));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable GET_Group_LIST(string Filter, int ProjectId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_TaskResource");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable GET_Group_LIST(string Filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_TaskResource");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable Get_UE_Type_List(string Filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetSiteTaskInventory");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable Get_UE_Item_List(string Filter,long UETypeId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetSiteTaskInventory");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UETypeId",UETypeId));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Get_SiteTask_Inventory(string Filter, long SiteTaskId,long SiteId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetSiteTaskInventory");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteTaskId", SiteTaskId,"@SiteId",SiteId));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable Get_SiteTask_Inventory_Attachments(string Filter, long SiteTaskInventoryId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetSiteTaskInventory");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteTaskInventoryId", SiteTaskInventoryId));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable Manage_SiteTask_Inventory(string Filter, DataTable dt=null,long SiteTaskId=0,long SiteId=0,long UserId= 0,long SiteTaskInventoryId = 0,string FileName=null,string SubDirectory="",long ContentLength=0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageSiteTaskInventory");
                return DataContext.Select(DataContext.AddParameters(loCommand, 
                    "@Filter", Filter, 
                    "@SiteTaskInventoryList", dt, 
                    "@SiteTaskId", SiteTaskId,
                    "@SiteId",SiteId,
                    "@UserId",UserId,
                    "@FileName",FileName, 
                    "@SiteTaskInventoryId", @SiteTaskInventoryId, 
                    "@SubDirectory",SubDirectory,
                    "@ContentLength", ContentLength));
                 ;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

    }
}