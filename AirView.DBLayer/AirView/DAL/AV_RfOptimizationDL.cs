using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
    public class AV_RfOptimizationDL
    {

        public DataTable GetSector(string Filter, Int64 SiteId, string SiteCode, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 ScopeId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSectors");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId, "@SiteCode", SiteCode, "@NetworkModeId", NetworkModeId, "@BandId", BandId, "@CarrierId", CarrierId, "@ScopeId", ScopeId));

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

        public DataTable GetPci(Int64 siteId)
        {
            SqlCommand command = DataContext.OpenConnection();
            try
            {
                command = DataContext.SetStoredProcedure(command, "Test_GetPci");
                return DataContext.Select(DataContext.AddParameters(command, "@siteId", siteId));
            }
            catch (Exception)
            {

                throw;
            }
        }


        public DataTable GetrFPlot(string Filter, Int64 siteId, Int64 netWorkModeId)
        {
            SqlCommand command = DataContext.OpenConnection();
            try
            {
                command = DataContext.SetStoredProcedure(command, "AV_RFLegends");
                return DataContext.Select(DataContext.AddParameters(command, "@Filter", Filter, "@SiteId", siteId, "@networkModeId", netWorkModeId));
            }
            catch (Exception)
            {

                throw;
            }
        }


        public  DataTable CreateKml(string Filter, Int64 SiteId, string SiteCode, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 ScopeId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetKmlLayer");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId, "@SiteCode", SiteCode, "@NetworkModeId", NetworkModeId, "@BandId", BandId, "@CarrierId", CarrierId, "@ScopeId", ScopeId));

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


        public DataTable GetSiteSector(string Filter,DataTable siteSector, long siteId, Int64 NetworkLayeId, string whereClause)
        {
            
            SqlCommand command = DataContext.OpenConnection();
            try
            {
                command = DataContext.SetStoredProcedure(command, "AV_Optimization");
                return DataContext.Select(DataContext.AddParameters(command, "@Filter", Filter, "@SitePci", siteSector, "@SiteId",siteId, "@NetworkLayeId", NetworkLayeId, "@whereClause", whereClause));
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetCollectedPci(string Filter, DataTable collectedPci, Int16 siteId, Int64 NetworkLayeId, string whereClause)
        {
            SqlCommand command = DataContext.OpenConnection();
            try
            {
                command = DataContext.SetStoredProcedure(command, "AV_Optimization");
                return DataContext.Select(DataContext.AddParameters(command, "@Filter", Filter, "@SitePci", collectedPci, "@SiteId", siteId, "@NetworkModeId", NetworkLayeId, "@whereClause", whereClause));
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetRFLegends(string Filter, DataTable RfLegend, Int16 siteId, Int64 NetworkModeId)
        {
            SqlCommand command = DataContext.OpenConnection();
            try
            {
                command = DataContext.SetStoredProcedure(command, "AV_Optimization");
                return DataContext.Select(DataContext.AddParameters(command, "@Filter", Filter, "@RfLegend", RfLegend, "@SiteId", siteId, "@NetworkModeId", NetworkModeId, "@whereClause", null));
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
