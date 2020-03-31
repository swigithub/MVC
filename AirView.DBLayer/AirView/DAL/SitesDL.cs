using System;
using System.Data;
using System.Data.SqlClient;

using SWI.Libraries.Common;

namespace SWI.Libraries.AirView.DAL
{
    public  class SitesDL
    {
        public  Int64 SaveNewSite(string Filter,string SiteId, string SiteCode, double Latitude, double Longitude, Int64 ClusterId, string ClientId, string Description, string Status, Int64 SubmittedById, string Market,DateTime ReceivedOn,string Scope,DataTable dt)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageSites");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.BigInt);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);
                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter",Filter, "@SiteId",SiteId, "@SiteCode", SiteCode, "@Latitude", Latitude, "@Longitude", Longitude, "@ClusterId", ClusterId
                    , "@ClientId", ClientId, "@Description", Description, "@Status", Status, "@SubmittedById", SubmittedById, "@Market",Market, "@ReceivedOn",ReceivedOn, "@Scope", Scope, "@List", dt));
                DataContext.EndTransaction(loCommand);
                Int64 result = Convert.ToInt64(loCommand.Parameters["@RETURN_VALUE"].Value);
                return result;
            }
            catch (Exception ex)
            {
                DataContext.CancelTransaction(loCommand);
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public  Int64 SaveNewClustor(string ClustorCode, string Region, string Market, string Client)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageClusters");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.BigInt);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);
                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@ClustorCode", ClustorCode, "@Region", Region, "@Market", Market, "@Client", Client));
                DataContext.EndTransaction(loCommand);
                Int64 result = Convert.ToInt64(loCommand.Parameters["@RETURN_VALUE"].Value);
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
       
        public  DataTable GetAllNetworkModesBandsCarriers()
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetNetworkInfo");
                return DataContext.Select(DataContext.AddParameters(loCommand));
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
        
        public  DataTable GetSiteBands(string Filter ,Int64 SiteId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetBands");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@SITEID", SiteId, "@Filter", Filter));
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public  DataTable GetSectors(string Filter, Int64 SiteId = 0, Int64 NetworkModeId = 0, Int64 BandId = 0, Int64 CarrierId = 0, Int64 ScopeId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSectors");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter",Filter, "@SiteId", SiteId, "@NetworkModeId", NetworkModeId, "@BandId", BandId, "@CarrierId", CarrierId, "@ScopeId", ScopeId));
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
        public  bool AssignTester(Int64 SiteId, Int64 TesterId, Int64 TesterAssignedById, DateTime SchduledDate, string Status, int NetworkMode, int Band, int Carrier,int UserDeviceId,string TestTypes,int? SequenceId,long? LayerStatusId=null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ScheduleSite");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@SiteId", SiteId, "@TesterId", TesterId, "@TesterassignedById", TesterAssignedById, "@SchduledOn", SchduledDate, "@Status", Status, "@NetworkModeId",NetworkMode, "@BandId",Band, "@CarrierId",Carrier, "@UserDeviceId", UserDeviceId, "@TestTypes", TestTypes, "@SequenceId", 0, "@DeviceScheduleId", 0,"@LayerStatusId",LayerStatusId));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch(Exception ex)
            {
               // DataContext.CancelTransaction(loCommand);
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public bool AssignTesterCLS(Int64 SiteId, Int64 TesterId, Int64 TesterAssignedById, DateTime SchduledDate, string Status, int NetworkMode, int Band, int Carrier, int UserDeviceId, string TestTypes, int? SequenceId,int Layer,int? DeviceScheduleId=0,bool IsMaster=false)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ScheduleSite");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand,"@SiteId", SiteId, "@TesterId", TesterId, "@TesterassignedById", TesterAssignedById, "@SchduledOn", SchduledDate, "@NetworkModeId", NetworkMode, "@BandId", Band, "@CarrierId", Carrier, "@Status", Status, "@UserDeviceId", UserDeviceId, "@TestTypes", TestTypes, "@SequenceId", SequenceId, "@NetLayerId",Layer, "@DeviceScheduleId", DeviceScheduleId,"@isMaster", IsMaster));
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

        public DataTable GetSiteData(string Filter,string value)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSite");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@value",value));
            }
            catch 
            {
              return  null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public  DataSet GetSiteTestSummary(Int64 SiteId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSiteTestSummary");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@SITEID", SiteId));
            }
            catch (Exception)
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
