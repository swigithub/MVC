using System;
using System.Data;
using System.Data.SqlClient;

using SWI.Libraries.Common;

namespace SWI.Libraries.AirView.DAL
{
    public  class DashboardDL
    {
        public  DataSet GetDashboardData(string ParentFilter, string ChildFilter, DateTime fromDate, DateTime toDate, DataTable dtCities, string Panel1Option, Int64 Panel1Value, string Panel2Option, Int64 Panel2Value,Int64 UserId,string CountryId,string Client,string Scopes,string Markets, string Projects)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetDashboardData");
                if (fromDate.ToShortDateString().ToString() == "1/1/2000")
                {
                    return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@PARENTFILTER", ParentFilter, "@CHILDFILTER", ChildFilter, "@FROMDATE", DBNull.Value, "@TODATE", DBNull.Value, "@CITEIES", dtCities, "@Panel1Option", Panel1Option, "@Panel1Value", Panel1Value, "@Panel2Option", Panel2Option, "@Panel2Value", Panel2Value, "@UserID",UserId, "@CountryId", CountryId, "@Client", Client, "@Scopes", Scopes, "@Markets", Markets, "@Projects", Projects));
                }
                else
                {
                    return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@PARENTFILTER", ParentFilter, "@CHILDFILTER", ChildFilter, "@FROMDATE", fromDate, "@TODATE", toDate, "@CITEIES", dtCities, "@Panel1Option", Panel1Option, "@Panel1Value", Panel1Value, "@Panel2Option", Panel2Option, "@Panel2Value", Panel2Value, "@UserID", UserId, "@CountryId", CountryId, "@Client", Client, "@Scopes",Scopes, "@Markets", Markets, "@Projects", Projects));
                }                                 
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
                loCommand.Dispose();
            }
        }



        public DataSet GetProjectDashboardData(string ParentFilter, string ChildFilter, DateTime fromDate, DateTime toDate, DataTable dtCities, string Panel1Option, Int64 Panel1Value, string Panel2Option, Int64 Panel2Value, Int64 UserId, string CountryId, string Client, string Scopes, string Markets)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetProjectDashboardData");
                if (fromDate.ToShortDateString().ToString() == "1/1/2000")
                {
                    return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@PARENTFILTER", ParentFilter, "@CHILDFILTER", ChildFilter, "@FROMDATE", DBNull.Value, "@TODATE", DBNull.Value, "@CITEIES", dtCities, "@Panel1Option", Panel1Option, "@Panel1Value", Panel1Value, "@Panel2Option", Panel2Option, "@Panel2Value", Panel2Value, "@UserID", UserId, "@CountryId", CountryId, "@Client", Client, "@Scopes", Scopes, "@Markets", Markets));
                }
                else
                {
                    return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@PARENTFILTER", ParentFilter, "@CHILDFILTER", ChildFilter, "@FROMDATE", fromDate, "@TODATE", toDate, "@CITEIES", dtCities, "@Panel1Option", Panel1Option, "@Panel1Value", Panel1Value, "@Panel2Option", Panel2Option, "@Panel2Value", Panel2Value, "@UserID", UserId, "@CountryId", CountryId, "@Client", Client, "@Scopes", Scopes, "@Markets", Markets));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
                loCommand.Dispose();
            }
        }




        public DataSet GetDashboardSites(string ParentFilter, string ChildFilter, DateTime fromDate, DateTime toDate,  string Panel1Option, Int64 Panel1Value, string Panel2Option, Int64 Panel2Value,Int64 UserId, string FilterOption,int Offset, int PageSize,string CountryId,string Client,string Scopes,string Projects)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
             
                
                  loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetDashboardSites");
                if (fromDate.ToShortDateString().ToString() == "1/1/2000")
                {
                    return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@PARENTFILTER", ParentFilter, "@CHILDFILTER", ChildFilter, "@FROMDATE", DBNull.Value, "@TODATE", DBNull.Value,"@Panel1Option", Panel1Option, "@Panel1Value", Panel1Value, "@Panel2Option", Panel2Option, "@Panel2Value", Panel2Value, "@UserID", UserId, "@FilterOption", FilterOption, "@Offset",Offset, "@PageSize",PageSize, "@CountryId", CountryId, "@Client", Client, "@Scopes", Scopes, "@Projects",Projects));
                }
                else
                {
                    return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@PARENTFILTER", ParentFilter, "@CHILDFILTER", ChildFilter, "@FROMDATE", fromDate, "@TODATE", toDate,  "@Panel1Option", Panel1Option, "@Panel1Value", Panel1Value, "@Panel2Option", Panel2Option, "@Panel2Value", Panel2Value, "@UserID", UserId, "@FilterOption", FilterOption, "@Offset", Offset, "@PageSize", PageSize,"@CountryId", CountryId, "@Client", Client, "@Scopes", Scopes, "@Projects", Projects));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
                loCommand.Dispose();
            }
        }

        public  DataTable GetPartialRegionalSites(string ParentFilter, string ChildFilter, DateTime StartDate, DateTime EndDate, string RegionFilter, Int64 UserId, Int64 FilterValue,string FilterType2,string FilterValue2)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_SummaryView");
                    return DataContext.Select(DataContext.AddParameters(loCommand, "@PARENTFILTER", ParentFilter, "@CHILDFILTER", ChildFilter, "@FROMDATE", StartDate, "@TODATE", EndDate, "@FilterType", RegionFilter, "@FilterValue", FilterValue, "@UserID",UserId, "@FilterType2", FilterType2, "@FilterValue2", FilterValue2));

            }
            catch
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
                loCommand.Dispose();
            }
        }

        public  DataTable GetSiteLocationsForSchedule(string ParentFilter, string ChildFilter, DateTime fromDate, DateTime toDate,string Panel1Option, Int64 Panel1Value, string Panel2Option, Int64 Panel2Value, Int64 UserId, string CountryId, string Client, string Scopes, string Markets)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSitesToSchedule");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@PARENTFILTER", ParentFilter, "@CHILDFILTER", ChildFilter, "@FROMDATE", fromDate, "@TODATE", toDate, "@Panel1Option", Panel1Option, "@Panel1Value", Panel1Value,
                                                                    "@Panel2Option", Panel2Option, "@Panel2Value", Panel2Value, "@UserID", UserId, "@CountryId", CountryId, "@Client", Client, "@Scopes", Scopes, "@Markets", Markets));

            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
                loCommand.Dispose();
            }
        }



        
    }
}
