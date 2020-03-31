using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.DAL
{
    public class PM_ProjectSitesDL
    {
        //public bool Manage(string Filter, DataTable list,Int64 UserId,string Value1=null,string value2=null, string value17 = null)
        //{
        //    SqlCommand loCommand = DataContext.OpenConnection();

        //    try
        //    {
        //        loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageProjectSites");
        //        loCommand = DataContext.StartTransaction(loCommand);
        //        bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List",list,"@UserId",UserId, "@Value1", Value1, "@Value2", value2, "@Value17", value17));
        //        DataContext.EndTransaction(loCommand);
        //        return result;

        //    }
        //    catch (Exception ex)
        //    {
        //        //  DataContext.CancelTransaction(loCommand);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        DataContext.CloseConnection(loCommand);
        //    }
        //}
        public bool Manage(string Filter, DataTable list, Int64? UserId, Int64? ProjectSiteId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageProjectSites");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@PrjSites", list, "@UserId", UserId, "@ProjectSiteId", ProjectSiteId));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch (Exception ex)
            {
                //  DataContext.CancelTransaction(loCommand);
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public bool Manage(string Filter, Int64? ProjectSiteId, bool IsActive)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageProjectSites");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectSiteId", ProjectSiteId, "@IsActive", IsActive));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch (Exception ex)
            {
                //  DataContext.CancelTransaction(loCommand);
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }



        public DataTable Get(string Filter, string value1 = null, string value2 = null, string value3 = null, Int64? ProjectSiteId = null, Int64? ProjectId = null, string SiteCode = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetProjectSites");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@value1", value1, "@value2", value2, "@value3", value3, "@ProjectSiteId", ProjectSiteId, "@ProjectId", ProjectId, "@SiteCode", SiteCode));
            }
            catch
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public int GetScaler(string Filter, string value1 = null, string value2 = null, string value3 = null, Int64? ProjectSiteId = null, Int64? ProjectId = null, string SiteCode = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetProjectSites");
                return DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@value1", value1, "@value2", value2, "@value3", value3, "@ProjectSiteId", ProjectSiteId, "@ProjectId", ProjectId, "@SiteCode", SiteCode));
            }
            catch(Exception er)
            {
                return -1;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }


        public DataSet GetDataSet(string filter, string Value1 = null, string Value2 = null, string Value3 = null, string Value4 = null, string Value5 = null, string Value6 = null, string Value7 = null, int offset = 10, string ProjectId = null, bool IsActive = true)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetProjectSites");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value1", Value1, "@Value2", Value2, "@Value3", Value3, "@value4", Value4, "@value5", Value5, "@value6", Value6, "@value7", Value7, "@Offset", offset, "@ProjectId", ProjectId, "@IsActive", IsActive));
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public bool ManageStatus(string Filter, DataTable list, Int64? ProjectSiteId, bool IsActive = true)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ManageProjectSites");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@PrjSites", null, "@ProjectSiteId", ProjectSiteId, "@IsActive", IsActive));
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
        public DataTable GetProjectSites(string filter, string Value1 = null, string Value2 = null, string Value3 = null, string Value4 = null, string Value5 = null, string Value6 = null, string UserId = null, int offset = 10, string ProjectId = null, bool IsActive = true)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_GetProjectSites");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value1", Value1, "@Value2", Value2, "@Value3", Value3, "@value4", Value4, "@value5", Value5, "@value6", Value6, "@UserId", UserId, "@Offset", offset, "@ProjectId", ProjectId, "@IsActive", IsActive));
            }
            catch
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
