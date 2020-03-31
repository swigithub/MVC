using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
    public class AV_SiteTestLogDL
    {
        public bool Insert(DataTable SiteConfiguration, string Status)
        {
            int num = SiteConfiguration.Columns.Count;
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_Insert_SiteTestLog");
                loCommand = DataContext.StartTransaction(loCommand);

                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@List", SiteConfiguration, "@TestStatus", Status));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch (Exception ex)
            {
                // DataContext.CancelTransaction(loCommand);
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public bool InsertBeamTest(DataTable SiteConfiguration, string Status)
        {
            int num = SiteConfiguration.Columns.Count;
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_Insert_BeamTestLog");
                loCommand = DataContext.StartTransaction(loCommand);

                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@List", SiteConfiguration, "@TestStatus", Status));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch (Exception ex)
            {
                // DataContext.CancelTransaction(loCommand);
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }


        public DataTable Get(string Filter, Int64 SiteId, Int64 NetworkmodeId, Int64 BandId, Int64 CarrierId, Int64 ScopeId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSiteTestLog");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId, "@NetworkmodeId", NetworkmodeId, "@BandId", BandId, "@CarrierId", CarrierId, "@ScopeId", ScopeId));
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

        public bool Manage(string Filter, Int64 SiteId, Int64 NetworkModeId, Int64 BandId, Int64 ScopeId, Int64 CarrierId, string value = null, string value1 = null, string value2 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManagSiteTestLog");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId, "@NetworkModeId", NetworkModeId, "@BandId", @BandId, "@ScopeId", @ScopeId, "@CarrierId", CarrierId, "@value", value, "@value1", value1, "@value2", value2));
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


        public bool RemoveSiteTestLogs(string Filter, string SiteId, bool IsActive, DataTable dt)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManagSiteTestLog");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId, "@value", IsActive, "@List", dt));
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



        public bool DisableServerTimestamp(string Filter, string SiteId, bool IsActive, string selectedVal)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManagSiteTestLog");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId, "@value", IsActive, "@value1", selectedVal));
                DataContext.EndTransaction(loCommand);
                return result;
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



        public bool DisablePcis(string Filter, string SiteId, bool IsActive, string selectedLayers = null, string selectedPcis = null, float? AngleFrom = null, float? AngleTo = null, float? DistanceFrom = null, float? DistanceTo = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManagSiteTestLog");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId, "@value", IsActive,
                    "@value1", selectedLayers, "@value2", selectedPcis, "@AngleFrom", AngleFrom, "@AngleTo", AngleTo, "@DistanceFrom", DistanceFrom, "@DistanceTo", DistanceTo));
                DataContext.EndTransaction(loCommand);
                return result;
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
    }
}
