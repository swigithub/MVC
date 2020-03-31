using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;


namespace AirView.DBLayer.AirView.DAL
{
    /*----MoB!----*/
    public class AV_SiteTestSummaryDL
    {


        public DataSet Get(int SiteId, int BandId,int Carrier,int NetworkMode,Int64 UserId,DateTime ReportDate )
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetNetLayerReport");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@SiteId", SiteId, "@BandId", BandId, "@Carrier", Carrier,"@NetworkMode", NetworkMode, "@UserId",UserId, "@ReportDate", ReportDate));
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
        public DataTable NetLayerSummary(int SiteId, int BandId, string Carrier, string NetworkMode, int UserId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetNetLayerSummary");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@SiteId", SiteId, "@BandId", BandId, "@Carrier", Carrier, "@NetworkMode", NetworkMode, "@UserId", UserId));
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

        public DataTable NetLayersBySiteCode(string SiteCode, int UserId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetNetLayersBySiteCode");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@SiteCode", SiteCode, "@UserId", UserId));
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


        public DataSet AV_GetNetworkLayerProcessed(int SiteId, int BandId, string Carrier, string NetworkMode)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetNetworkLayerProcessed");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@SiteId", SiteId, "@BandId", BandId, "@Carrier", Carrier, "@NetworkMode", NetworkMode));
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
        public bool Manage(string Filter, string SiteId, string SectorId, string NetworkModeId, string BandId, string CarrierId, string ScopeId, string TestType, string Ping,
                           string Value4, string Value3, string ImagePath,string Value1,string Value2, Int64 UserId, string CWComments = null, string CCWComments = null, string PDSCHComments = null, string PUSCHComments = null,bool isManual = false)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageTestSummary");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId, "@SectorId", SectorId, "@NetworkModeId", NetworkModeId, "@BandId", BandId, "@CarrierId", CarrierId, "@ScopeId", ScopeId, "@TestType", TestType,
                                                            "@Ping", Ping, "@Value4", Value4, "@Value3", Value3, "@ImagePath", ImagePath, "@Value1", Value1, "@Value2", Value2, "@UserId", UserId,
                                                            "@CWComments",CWComments,"@CCWComments",CCWComments,"@PDSCHComments",PDSCHComments,"@PUSCHComments",PUSCHComments, "@isManual", isManual));
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
                loCommand.Dispose();
            }
        }
        public bool Update( DataTable data,Int64 UserId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_UpdateSiteTestSummary");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@List", data, "@UserId",UserId));
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
                loCommand.Dispose();
            }
        }

       

    }
}
