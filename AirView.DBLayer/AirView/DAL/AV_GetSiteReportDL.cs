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
    /*----MoB!----*/
    public class AV_GetSiteReportDL
    {
        public DataTable Get(string Filter, Int64 SiteId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSiteReport");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId));
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

        public DataSet GetDs(string Filter, Int64 SiteId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSiteReport");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId));
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

        public DataTable GetDataTable(string filter, Int64 SiteId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSiteReport");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@SiteId", SiteId));
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

        public DataSet GetDataSet(string filter, Int64 SiteId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSiteReport");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@Filter", filter, "@SiteId", SiteId));
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
    }
}
